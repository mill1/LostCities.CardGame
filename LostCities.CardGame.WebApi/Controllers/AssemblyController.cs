using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LostCities.CardGame.WebApi.Interfaces;
using System;

namespace LostCities.CardGame.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssemblyController : ControllerBase
    {
        private readonly IAssemblyService assemblyService;
        private readonly ILogger<AssemblyController> logger;        

        public AssemblyController(IAssemblyService assemblyService, ILogger<AssemblyController> logger)
        {
            this.assemblyService = assemblyService;
            this.logger = logger;            
        }

        [HttpGet("properties")]
        public IActionResult GetProperties()
        {
            try
            {
                return Ok(assemblyService.GetProperties());
            }
            catch (Exception e)
            {
                string message = $"Getting the properties failed.\r\nException:\r\n{e}";
                logger.LogError($"{message}", e);
                return BadRequest(message);
            }
        }

        [HttpGet("property/{property}")]
        public IActionResult GetPropertyValue(string property)
        {
            try
            {
                return Ok(assemblyService.GetPropertyValue(property));
            }
            catch (Exception e)
            {
                string message = $"Getting the propery value failed. Property = {property}\r\n" +
                                 $"Exception:\r\n{e}";
                logger.LogError($"{message}", e);
                return BadRequest(message);
            }
        }        
    }
}
