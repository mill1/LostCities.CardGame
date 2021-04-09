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

            switch (property)
            {
                case "Name":
                    return "LostCities.CardGame.WebApi";
                case "Version":
                    return "0.0.0.0";
                case "KeyPair":
                    return "[hidden]";
                case "ThrowException":
                    throw new Exception("Mock Exception");
                default:
                    return "not available";                    
            }
        }
    }
}
