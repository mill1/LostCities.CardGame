using LostCities.CardGame.WebApi.Interfaces;
using LostCities.CardGame.WebApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostCities.CardGame.WebApi.Services
{
    public class GameService : IGameService
    {
        private readonly ILogger logger;

        public GameService(ILogger<GameService> logger)
        {
            this.logger = logger;
        }

        public IEnumerable<Card> InitializeDeck()
        {
            List<Card> deck = new List<Card>();

            var expeditionColors = new[] 
            { 
                new { id = "B", color = ConsoleColor.Blue },
                new { id = "Y", color = ConsoleColor.Yellow },
                new { id = "R", color = ConsoleColor.Red },
                new { id = "W", color = ConsoleColor.White },
                new { id = "G", color = ConsoleColor.Green },
            }.ToList();

            expeditionColors.ForEach(ec => AddExpeditionToDeck(ec, deck));

            return deck;
        }

        private void AddExpeditionToDeck(dynamic expeditionColor, List<Card> deck)
        {
            for (int value = 2; value <= 10; value++)
                deck.Add(new Card(id: $"{expeditionColor.id}{value}", color:expeditionColor.color, value:value));

            string[] bettingCardIds = new string[] { "A", "B", "C" };

            for (int i = 0; i < bettingCardIds.Length; i++)
                deck.Add(new Card(id: $"{expeditionColor.id}{bettingCardIds[i]}", color: expeditionColor.color, value: 0));
        }
    }
}
