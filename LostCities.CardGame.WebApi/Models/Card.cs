using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostCities.CardGame.WebApi.Models
{
    public class Card
    {
        public Card(string id, ConsoleColor color, int value)
        {
            Id = id;
            Color = color;
            Value = value;
        }

        public string Id { get; }
        public ConsoleColor Color { get; }
        public int Value { get; } // Betting card: Value == 0, otherwise expedition card
    }
}
