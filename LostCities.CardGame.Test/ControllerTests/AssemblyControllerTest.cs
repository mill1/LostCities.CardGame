using LostCities.CardGame.Test.Mocks;
using LostCities.CardGame.WebApi.Controllers;
using LostCities.CardGame.WebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace LostCities.CardGame.Test.ControllerTests
{
    [Collection("Realm tests")]
    public class AssemblyControllerTest: IDisposable
    {
        private readonly AssemblyController assemblyController;
        private readonly IAssemblyService mockAssemblyService;

        public AssemblyControllerTest()
        {
            mockAssemblyService = new MockAssemblyService();
            ILogger<AssemblyController> mockLogger = MockLogger<AssemblyController>.CreateMockLogger();
            assemblyController = new AssemblyController(mockAssemblyService, mockLogger);            
        }

        [Fact]
        public void GetPropertiesOkTest()
        {
            IActionResult result = assemblyController.GetProperties();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetPropertyValueOkTest()
        {
            IActionResult result = assemblyController.GetPropertyValue("PropertyName");

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetPropertyValueBadRequestTest()
        {
            IActionResult result = assemblyController.GetPropertyValue("ThrowException");

            Assert.IsType<BadRequestObjectResult>(result);
        }



        public void Dispose()
        {
        }
    }
}
