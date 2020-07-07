using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Microsoft.Research.EyeCatcher.Library;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Microsoft.Research.EyeCatcher.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// Connection string from environment.
        /// </summary>
        static readonly string ConnectionString = Environment.GetEnvironmentVariable("BLOB_STORE_STRING");

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            await Task.Yield();

            // Connect to blob store.
            var cloudStorageAccount = CloudStorageAccount.Parse(ConnectionString);
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            // Get blob.
            var container = cloudBlobClient.GetContainerReference("defaultdata");
            var reference = container.GetBlobReference("DefaultData.json");
            var stream = new MemoryStream();
            await reference.DownloadToStreamAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            return json;
        }

        private async Task<string> GetValidUserInfoAsync()
        {
            string userInfo;

            if (!Request.Headers.TryGetValue("Identity-Token", out var tokens) || tokens.Count != 1)
            {
                throw new InvalidOperationException("Not one identity token");
            }

            using (var client = new HttpClient())
            {
                var restApi = new Uri(@"https://apis.live.net/v5.0/me?access_token=" + tokens[0]);
                var infoResult = await client.GetAsync(restApi);
                if (!infoResult.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException("Invalid user");
                }
                userInfo = await infoResult.Content.ReadAsStringAsync();
            }

            return userInfo;
        }

        private static async Task<Stream> ReadStreamAsync(MultipartReader reader, string expectedFileName)
        {
            Stream value;

            var section = await reader.ReadNextSectionAsync();

            if (section != null)
            {
                var header = ContentDispositionHeaderValue.Parse(section.ContentDisposition);

                if (!header.DispositionType.Equals("form-data"))
                {
                    throw new InvalidDataException("Header missing");
                }

                if (header.FileName != expectedFileName)
                {
                    throw new InvalidDataException("Unexpected part: " + header.FileName);
                }

                // Copy part into stream.
                value = new MemoryStream();
                await section.Body.CopyToAsync(value);
                value.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                value = null;
            }

            return value;
        }

        private static async Task<string[]> GetHierarchyNamesAsync(Stream stream)
        {
            var instanceId = Guid.Empty;
            var userId = Guid.Empty;
            var sessionTimestamp = DateTimeOffset.UtcNow;
            sessionTimestamp -= sessionTimestamp.TimeOfDay;
            var sessionId = Guid.Empty;

            string json;
            using (var reader = new StreamReader(stream, leaveOpen: true))
            {
                json = await reader.ReadToEndAsync();
            }
            stream.Seek(0, SeekOrigin.Begin);

            try
            {
                var ob = JsonObject.Parse(json);

                // Second format of Json Object has session properties two levels down.
                if (ob.TryGetValue(nameof(LeafSerializableImageData.Position), out IJsonValue position) && position.ValueType == JsonValueType.Object)
                {
                    if (position.GetObject().TryGetValue(nameof(LeafSerializablePositionData.Session), out IJsonValue session) && session.ValueType == JsonValueType.Object)
                    {
                        ob = session.GetObject();
                    }
                }

                instanceId = new Guid(ob[nameof(ISessionData.InstanceId)].GetString());
                userId = new Guid(ob[nameof(ISessionData.UserId)].GetString());
                sessionTimestamp = DateTimeOffset.Parse(ob[nameof(ISessionData.SessionTimestamp)].GetString());
                sessionId = new Guid(ob[nameof(ISessionData.SessionId)].GetString());
            }
            catch
            {

            }

            var result = Naming.GetBlobHierarchyNames(instanceId, userId, sessionId, sessionTimestamp);
            return result;
        }

        // POST api/values
        [HttpPost]
        public async Task Post()
        {
            // // Check we have a valid MSA user identity token token.
            // var userInfo = await GetValidUserInfoAsync();

            // // Pull user ID from user info and create stream from user info.
            // GetUserIdAndStream(userInfo, out var userId, out var identityStream);

            //if (!Request.Headers.TryGetValue("Session-Token", out var tokens) || tokens.Count != 1)
            //{
            //    throw new InvalidOperationException("Not one identity token");
            //}
            //var userId = Guid.Parse(tokens[0]).ToString();


            // Prepare to read body;
            var contentType = MediaTypeHeaderValue.Parse(Request.ContentType);
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary);
            var reader = new MultipartReader(boundary.Value, Request.Body);

            // Directory we will write into
            CloudBlobDirectory directory = null;

            // Work through sections.
            var moreTodo = true;
            while (moreTodo)
            {
                // Get the annoation and image streams, if present.
                var annotationStream = await ReadStreamAsync(reader, Naming.AnnotationPartFile);
                var imageStream = annotationStream == null ? null : await ReadStreamAsync(reader, Naming.ImagePartFile);

                if (imageStream == null)
                {
                    // Finish if no more to do.
                    moreTodo = false;
                }
                else
                {
                    // If this is the first time around...
                    if (directory == null)
                    {
                        var hierarchyNames = await GetHierarchyNamesAsync(annotationStream);

                        // Connect to blob store.
                        var cloudStorageAccount = CloudStorageAccount.Parse(ConnectionString);
                        var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                        // Get blob container.
                        var parentContainer = cloudBlobClient.GetContainerReference("hier");
                        await parentContainer.CreateIfNotExistsAsync();

                        // Get directory for data.
                        directory = parentContainer.GetDirectoryReference(hierarchyNames[0]);

                        for (var i = 1; i < hierarchyNames.Length; i++)
                        {
                            directory = directory.GetDirectoryReference(hierarchyNames[i]);
                        }
                    }

                    // Unique identifier for data.
                    var guid = Naming.GetFileBaseName();

                    // Record user identity. (Perhaps delete in production system.)
                    // identityStream.Seek(0, SeekOrigin.Begin);
                    // var identityBlob = directory.GetBlockBlobReference(guid + ".identity");
                    // await identityBlob.UploadFromStreamAsync(identityStream);

                    // Record the image
                    var imageBlob = directory.GetBlockBlobReference(Naming.GetImageFileName(guid));
                    await imageBlob.UploadFromStreamAsync(imageStream);

                    // Record the annotations.
                    var annotationBlob = directory.GetBlockBlobReference(Naming.GetAnnotationFileName(guid));
                    await annotationBlob.UploadFromStreamAsync(annotationStream);
                }
            }
        }
    }
}
