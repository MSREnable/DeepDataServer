using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private const string DataRoot = "/data";
        private const string SchemaVersion = "200407";
        private const string UserConsent = "UserConsent";
        private readonly DirectoryInfo storageDirectory = Directory.CreateDirectory($"{DataRoot}/{SchemaVersion}");
        private readonly DirectoryInfo usersDirectory = Directory.CreateDirectory($"{DataRoot}/{SchemaVersion}/private/users");

        private string SanitizeFileName(string fileName)
        {
            return String.Join("_", fileName.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var defaultDataPath = Path.Combine(Directory.GetCurrentDirectory(), "DefaultData.json");
            using (var defaultDataStream = new FileInfo(defaultDataPath).OpenText())
            {
                return await defaultDataStream.ReadToEndAsync();
            }
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<StatusCodeResult> Get(string userId)
        {
            if (!CheckUserConsent(userId))
            {
                _logger.LogInformation($"Denied {userId}");
                return new StatusCodeResult(403);
            }

            _logger.LogInformation($"Consent Found for {userId}");
            return Ok();
        }

        private string CreateUserHash(string userId)
        {
            using (var hash = MD5.Create())            
            {
                var userHash = SanitizeFileName(
                    Convert.ToBase64String(
                        hash.ComputeHash(
                            Encoding.UTF8.GetBytes(
                                UserIdSalt + userId.ToLowerInvariant()
                            )
                        )
                    ).Replace('/','_').TrimEnd('=')
                );

                var userDirectory = usersDirectory.CreateSubdirectory($"{userHash}");
                var fullPath = Path.Combine(userDirectory.FullName, "id");
                if (!(new FileInfo(fullPath).Exists))
                {
                    using (var streamWriter = new StreamWriter(fullPath))
                    {
                        streamWriter.Write(userId);
                    }
                }

                return userHash;
            }
        }

        private bool CheckUserConsent(string userId)
        {
            var userHash = CreateUserHash(userId);
            return new FileInfo(Path.Combine(usersDirectory.FullName, $"{userHash}", $"{UserConsent}")).Exists;
        }

        // Support for uploading session metadata
        [HttpPut("{deviceSku}/{userId}/{sessionId}/{fileName}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Put(string deviceSku, string userId, string sessionId, string fileName)
        {
            if (!CheckUserConsent(userId))
            {
                _logger.LogInformation($"Denied {deviceSku} {userId} {sessionId} {fileName}");
                return new StatusCodeResult(403);
            }

            var fileDirectory = storageDirectory.CreateSubdirectory($"{SanitizeFileName(deviceSku)}/{CreateUserHash(userId)}/{SanitizeFileName(sessionId)}");
            using (var fileStream = new FileInfo(Path.Combine(fileDirectory.FullName, $"{SanitizeFileName(fileName)}")).OpenWrite())
            {
                await Request.Body.CopyToAsync(fileStream);
            }

            _logger.LogInformation($"Uploaded {deviceSku} {userId} {sessionId} {fileName}");
            return Ok();
        }
        
        [HttpPut("{deviceSku}/{userId}/{sessionId}/{folderName}/{fileName}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Put(string deviceSku, string userId, string sessionId, string folderName, string fileName)
        {
            if (!CheckUserConsent(userId))
            {
                _logger.LogInformation($"Denied {deviceSku} {userId} {sessionId} {folderName} {fileName}");
                return new StatusCodeResult(403);
            }

            var fileDirectory = storageDirectory.CreateSubdirectory($"{SanitizeFileName(deviceSku)}/{CreateUserHash(userId)}/{SanitizeFileName(sessionId)}/frames/");
            using (var fileStream = new FileInfo(Path.Combine(fileDirectory.FullName, $"{SanitizeFileName(folderName)}-{SanitizeFileName(fileName)}")).OpenWrite())
            {
                await Request.Body.CopyToAsync(fileStream);
            }

            _logger.LogInformation($"Uploaded {deviceSku} {userId} {sessionId} {folderName} {fileName}");
            return Ok();
        }
    }
}