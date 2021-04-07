using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostCities.CardGame.WebApi.Models
{
    public class ExpeditionType
    {
        public string Code { get; }
        public string Name { get; }
        public ConsoleColor Color { get; }

        public ExpeditionType(string name)
        {
            Name = name;
            Code = name.Substring(0, 1);

            ConsoleColor color;
            if (Enum.TryParse(name, out color))
                Color = color;
            else
                throw new Exception($"Invalid color: {name}");
        }
    }
}
