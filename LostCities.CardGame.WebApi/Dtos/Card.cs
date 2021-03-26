using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostCities.CardGame.WebApi.Dtos
{
    public class Card
    {
        public string Id { get; set; }
        public string Color { get; set; }
        public int Value { get; set; } // Wager card: Value == 0, otherwise expedition card
    }
}
