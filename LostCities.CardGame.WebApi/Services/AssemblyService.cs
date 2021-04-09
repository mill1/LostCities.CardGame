using LostCities.CardGame.WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LostCities.CardGame.WebApi.Services
{
    public class AssemblyService: IAssemblyService
    {
        private readonly Assembly executingAssembly;

        public AssemblyService()
        {
            executingAssembly = Assembly.GetExecutingAssembly();
        }

        public string GetProperties()
        {
            var assemblyName = executingAssembly.GetName();

            string properties = string.Empty;

            assemblyName.GetType().GetProperties().ToList().ForEach(p =>
            {
                properties += $"{p.Name}: {GetAssemblyPropertyValue(assemblyName, p.Name)}\n";
            });

            return properties;
        }

        public string GetPropertyValue(string property)
        {
            var assemblyName = executingAssembly.GetName();

            string assemblyPropertyValue = GetAssemblyPropertyValue(assemblyName, property);

            return assemblyPropertyValue;
        }

        private string GetAssemblyPropertyValue(AssemblyName assemblyName, string property)
        {
            try
            {
                // property
                if (property.Equals("KeyPair") || property.Contains("CodeBase"))
                    return "[hidden]";
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
