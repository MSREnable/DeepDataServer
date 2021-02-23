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
    public class RegisterController : ControllerBase
    {
        private readonly ILogger _logger;

        public RegisterController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<StatusCodeResult> Get(string userId)
        {
            _logger.LogInformation($"Registering Consent for {userId}");
            Users.SetUserConsent(userId);
            return Ok();
        }

    }
}
