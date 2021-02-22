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

        private string CreateUserHash(string userId)
        {
            using (var hash = MD5.Create())            
            {
                var userHash = Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(UserIdSalt + userId.ToLowerInvariant()))).Replace('/','_');

                var fullPath = Path.Combine(usersDirectory.FullName, $"{userHash}");
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
                return Forbid();
            }

            var fileDirectory = storageDirectory.CreateSubdirectory($"{deviceSku}/{CreateUserHash(userId)}/{sessionId}");
            using (var fileStream = new FileInfo(Path.Combine(fileDirectory.FullName, $"{fileName}")).OpenWrite())
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
                return Forbid();
            }

            var fileDirectory = storageDirectory.CreateSubdirectory($"{deviceSku}/{CreateUserHash(userId)}/{sessionId}/frames/");
            using (var fileStream = new FileInfo(Path.Combine(fileDirectory.FullName, $"{folderName}-{fileName}")).OpenWrite())
            {
                await Request.Body.CopyToAsync(fileStream);
            }

            _logger.LogInformation($"Uploaded {deviceSku} {userId} {sessionId} {folderName} {fileName}");
            return Ok();
        }
    }
}