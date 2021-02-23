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
            if (!Users.CheckUserConsent(userId))
            {
                _logger.LogInformation($"Denied {userId}");
                return new StatusCodeResult(403);
            }

            _logger.LogInformation($"Consent Found for {userId}");
            return Ok();
        }

        // Support for uploading session metadata
        [HttpPut("{deviceSku}/{userId}/{sessionId}/{fileName}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Put(string deviceSku, string userId, string sessionId, string fileName)
        {
            if (!Users.CheckUserConsent(userId))
            {
                _logger.LogInformation($"Denied {deviceSku} {userId} {sessionId} {fileName}");
                return new StatusCodeResult(403);
            }

            var fileDirectory = Storage.StorageDirectory.CreateSubdirectory($"{Storage.SanitizeFileName(deviceSku)}/{Users.CreateUserHash(userId)}/{Storage.SanitizeFileName(sessionId)}");
            using (var fileStream = new FileInfo(Path.Combine(fileDirectory.FullName, $"{Storage.SanitizeFileName(fileName)}")).OpenWrite())
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
            if (!Users.CheckUserConsent(userId))
            {
                _logger.LogInformation($"Denied {deviceSku} {userId} {sessionId} {folderName} {fileName}");
                return new StatusCodeResult(403);
            }

            var fileDirectory = Storage.StorageDirectory.CreateSubdirectory(
                $"{Storage.SanitizeFileName(deviceSku)}/{Users.CreateUserHash(userId)}/{Storage.SanitizeFileName(sessionId)}/frames/"
            );

            using (var fileStream = new FileInfo(
                Path.Combine(fileDirectory.FullName, $"{Storage.SanitizeFileName(folderName)}-{Storage.SanitizeFileName(fileName)}")
            ).OpenWrite())
            {
                await Request.Body.CopyToAsync(fileStream);
            }

            _logger.LogInformation($"Uploaded {deviceSku} {userId} {sessionId} {folderName} {fileName}");
            return Ok();
        }
    }
}