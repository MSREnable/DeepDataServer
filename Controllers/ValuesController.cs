using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Microsoft.Research.EyeCatcher.Library;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeepDataServer.Controllers
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
            //var cloudStorageAccount = CloudStorageAccount.Parse(ConnectionString);
            //var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            // Get blob.
            //var container = cloudBlobClient.GetContainerReference("defaultdata");
            //var reference = container.GetBlobReference("DefaultData.json");
            var stream = new MemoryStream();
            //await reference.DownloadToStreamAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            return json;
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

            ISessionData sd = JsonSerializer.Deserialize<ISessionData>(json);

            /*
                // Second format of Json Object has session properties two levels down.
                if (ob.TryGetValue(nameof(LeafSerializableImageData.Position), out IJsonValue position) && position.ValueType == JsonValueType.Object)
                {
                    if (position.GetObject().TryGetValue(nameof(LeafSerializablePositionData.Session), out IJsonValue session) && session.ValueType == JsonValueType.Object)
                    {
                        ob = session.GetObject();
                    }
                }
            */

            var result = Naming.GetBlobHierarchyNames(sd.InstanceId, sd.UserId, sd.SessionId, sd.SessionTimestamp);
            return result;
        }

        // POST api/values
        [HttpPost]
        public async Task Post()
        {

            // Prepare to read body;
            var contentType = MediaTypeHeaderValue.Parse(Request.ContentType);
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary);
            var reader = new MultipartReader(boundary.Value, Request.Body);

            // Directory we will write into
            var storageDirectory = new DirectoryInfo(@"\Data");

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
                    // Generate storage directory hierarchy
                    var hierarchyNames = await GetHierarchyNamesAsync(annotationStream);

                    storageDirectory = storageDirectory.CreateSubdirectory(hierarchyNames[0]);
                    for (var i = 1; i < hierarchyNames.Length; i++)
                    {
                        storageDirectory = storageDirectory.CreateSubdirectory(hierarchyNames[i]);
                    }

                    var guid = Naming.GetFileBaseName();
                    var imageFileName = Naming.GetImageFileName(guid);
                    var annotationFileName = Naming.GetAnnotationFileName(guid);

                    using (var imageFileStream = new FileInfo(Path.Combine(storageDirectory.FullName, imageFileName)).Open(FileMode.OpenOrCreate))
                    {
                        await imageStream.CopyToAsync(imageFileStream);
                    }

                    using (var annotationFileStream = new FileInfo(Path.Combine(storageDirectory.FullName, annotationFileName)).Open(FileMode.OpenOrCreate))
                    {
                        await annotationStream.CopyToAsync(annotationFileStream);
                    }
                }
            }
        }
    }
}
