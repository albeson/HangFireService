using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
namespace HangFireService.Controllers
{
    [Route("/")]
    [ApiController]
    [AllowAnonymous]  
    public class InfoController : ControllerBase
    {
        private readonly ILogger<InfoController> _logger;

        public InfoController(ILogger<InfoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> Get()
        {

            var info = new
            {
                Description = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationName,
                Version = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationVersion,
                BasePath = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationBasePath,
                RuntimeFramework = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.RuntimeFramework.FullName
            };
            //return await context.Response.WriteAsync("Hello World!");
            return Ok(await Task.FromResult(info));

        }
    }
}
