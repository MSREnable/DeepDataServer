using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.Research.EyeCatcher.Library;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DeepDataServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        private const string UserIdSalt = "NeverPunt-";
        private const string SchemaVersion = "200407";
        private readonly DirectoryInfo storageDirectory = Directory.CreateDirectory($"/data/{SchemaVersion}");

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var defaultDataPath = Path.Combine(Directory.GetCurrentDirectory(), "DefaultData.json");
            using (var defaultDataStream = new FileInfo(defaultDataPath).OpenText())
            {
                return await defaultDataStream.ReadToEndAsync();
            }
        }

        // Support for uploading session metadata
        [HttpPut("{deviceSku}/{userId}/{sessionId}/{fileName}")]
        public async Task Put(string deviceSku, string userId, string sessionId, string fileName)
        {
            // Directory we will write into
            _logger.LogInformation($"Uploaded {deviceSku} {userId} {sessionId} {fileName}");

            using (var hash = MD5.Create())            
            {
                var userIdHash = Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(UserIdSalt + userId.ToLowerInvariant()))).Replace('/','_');
    
                var fileDirectory = storageDirectory.CreateSubdirectory($"{deviceSku}/{userIdHash}/{sessionId}");
                using (var fileStream = new FileInfo(Path.Combine(fileDirectory.FullName, $"{fileName}")).OpenWrite())
                {
                    await Request.Body.CopyToAsync(fileStream);
                }
            }
        }
        
        [HttpPut("{deviceSku}/{userId}/{sessionId}/{folderName}/{fileName}")]
        public async Task Put(string deviceSku, string userId, string sessionId, string folderName, string fileName)
        {
            // Directory we will write into
            _logger.LogInformation($"Uploaded {deviceSku} {userId} {sessionId} {folderName} {fileName}");

            using (var hash = MD5.Create())            
            {
                var userIdHash = Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(UserIdSalt + userId.ToLowerInvariant()))).Replace('/','_');
    
                var fileDirectory = storageDirectory.CreateSubdirectory($"{deviceSku}/{userIdHash}/{sessionId}");
                using (var fileStream = new FileInfo(Path.Combine(fileDirectory.FullName, $"{folderName}-{fileName}")).OpenWrite())
                {
                    await Request.Body.CopyToAsync(fileStream);
                }
            }
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

        // POST api/values
        [HttpPost]
        public async Task Post(string UserId, string SessionId)
        {

            // Prepare to read body;
            var contentType = MediaTypeHeaderValue.Parse(Request.ContentType);
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary);
            var reader = new MultipartReader(boundary.Value, Request.Body);
            var section1 = await reader.ReadNextSectionAsync();
            var fileSection1 = section1.AsFileSection();
            var section2 = await reader.ReadNextSectionAsync();
            var fileSection2 = section2.AsFileSection();

            // Directory we will write into
            var storageDirectory = new DirectoryInfo(@"/Data");

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
                    var userStorageDirectory = storageDirectory.CreateSubdirectory(UserId); // TODO: Hash this in the future
                    var sessionStorageDirectory = userStorageDirectory.CreateSubdirectory(SessionId);

                    var guid = Naming.GetFileBaseName();
                    var imageFileName = Naming.GetImageFileName(guid);
                    var annotationFileName = Naming.GetAnnotationFileName(guid);

                    using (var imageFileStream = new FileInfo(Path.Combine(sessionStorageDirectory.FullName, imageFileName)).Open(FileMode.OpenOrCreate))
                    {
                        await imageStream.CopyToAsync(imageFileStream);
                    }

                    using (var annotationFileStream = new FileInfo(Path.Combine(sessionStorageDirectory.FullName, annotationFileName)).Open(FileMode.OpenOrCreate))
                    {
                        await annotationStream.CopyToAsync(annotationFileStream);
                    }
                }
            }
        }
    }
}
