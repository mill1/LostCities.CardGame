using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostCities.CardGame.WebApi.Models
{
    public class Card
    {
        public Card(string id, ExpeditionType expeditionType, int value)
        {
            Id = id;
            ExpeditionType = expeditionType;
            Value = value;
        }

        public string Id { get; }
        public ExpeditionType ExpeditionType { get; }
        public int Value { get; } // Wager card: Value == 0, otherwise expedition card
    }
}
