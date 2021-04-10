using System;

namespace LostCities.CardGame.WebApi.Interfaces
{
    public interface IAssemblyService
    {
        public string GetProperties();
        public string GetPropertyValue(string property);
    }
}
