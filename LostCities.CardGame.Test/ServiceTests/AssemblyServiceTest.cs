using LostCities.CardGame.WebApi.Interfaces;
using LostCities.CardGame.WebApi.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace LostCities.CardGame.Test.ServiceTests
{
    [Collection("Realm tests")]
    public class AssemblyServiceTest : IDisposable
    {
        private readonly IAssemblyService assemblyService;

        public AssemblyServiceTest()
        {
            assemblyService = new AssemblyService();
        }

        [Fact]
        public void GetPropertyValueNameTest()
        {
            string expected = "LostCities.CardGame.WebApi";

            Assert.Equal(expected, assemblyService.GetPropertyValue("Name"));
        }

        [Fact]
        public void GetPropertyValueVersionTest()
        {
            Assert.NotNull(assemblyService.GetPropertyValue("Version"));
        }

        [Fact]
        public void GetPropertyValueUnknownTest()
        {
            string expected = "not available";

            Assert.Equal(expected, assemblyService.GetPropertyValue("UNKNOWN PROPERTY"));
        }

        [Fact]
        public void GetPropertiesTest()
        {
            Assert.NotNull(assemblyService.GetProperties());
        }

        public void Dispose()
        {
        }
    }
}
