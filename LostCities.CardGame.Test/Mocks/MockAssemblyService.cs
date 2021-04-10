using LostCities.CardGame.WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LostCities.CardGame.Test.Mocks
{
    public class MockAssemblyService : IAssemblyService
    {
        public string GetProperties()
        {
            return "LostCities.CardGame.Test.Mocks.MockAssemblyService.GetProperties()";
        }

        public string GetPropertyValue(string property)
        {
            if (property.Contains("CodeBase"))
                return "[hidden]";

            return property switch
            {
                "Name" => "LostCities.CardGame.WebApi",
                "Version" => "0.0.0.0",
                "KeyPair" => "[hidden]",
                "ThrowException" => throw new Exception("Mock Exception"),
                _ => "not available",
            };
        }
    }
}
