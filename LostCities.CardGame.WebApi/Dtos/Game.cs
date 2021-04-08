using System;
using System.Collections.Generic;

namespace LostCities.CardGame.WebApi.Dtos
{
    public class Game
    {
        public IEnumerable<Card> PlayerCards { get; set; }
        public IEnumerable<Card> BotCards { get; set; }
        public IEnumerable<Card> DrawPile { get; set; }
        public IEnumerable<IEnumerable<Card>> PlayerExpeditions { get; set; }
        public IEnumerable<IEnumerable<Card>> BotExpeditions { get; set; }
        public IEnumerable<IEnumerable<Card>> DiscardPiles { get; set; }
        public string DescriptionLastTurn { get; set; }
    }
}
