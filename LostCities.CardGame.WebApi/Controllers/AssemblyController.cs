using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;

namespace VersionReferences.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssemblyController : ControllerBase
    {
        private readonly ILogger<AssemblyController> logger;
        public readonly Assembly ExecutingAssembly;

        public AssemblyController(ILogger<AssemblyController> logger)
        {
            this.logger = logger;
            ExecutingAssembly = Assembly.GetExecutingAssembly();
        }

        [HttpGet("properties")]
        public IActionResult GetProperties()
        {
            try
            {
                var assemblyName = ExecutingAssembly.GetName();

                var properties = assemblyName.GetType().GetProperties();

                string response = string.Empty;

                properties.ToList().ForEach(p =>
                {
                    response += $"{p.Name}: {GetAssemblyPropertyValue(assemblyName, p.Name)}\r\n";
                });

                return Ok(response);
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
                var assemblyName = ExecutingAssembly.GetName();
                string assemblyPropertyValue = GetAssemblyPropertyValue(assemblyName, property);
                return Ok(assemblyPropertyValue);
            }
            catch (Exception e)
            {
                string message = $"Getting the propery value failed. Property = {property}\r\n" +
                                 $"Exception:\r\n{e}";
                logger.LogError($"{message}", e);
                return BadRequest(message);
            }
        }

        private string GetAssemblyPropertyValue(AssemblyName assemblyName, string property)
        {
            try
            {
                if (property.Contains("CodeBase"))
                    return "not available";
                else
                    return assemblyName.GetType().GetProperty(property).GetValue(assemblyName, null).ToString();
            }
            catch (Exception)
            {
                return "not available";
            }
        }
    }
}
