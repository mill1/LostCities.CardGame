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

        public GameCards GetNewGame()
        {
            List<ExpeditionType> expeditions = GetExpeditions();
            List<Card> deck = InitializeDeck(expeditions);            
            GameCards game = new GameCards(deck);

            return game;
        }

        private List<Card> InitializeDeck(List<ExpeditionType> expeditions)
        {
            List<Card> deck = new List<Card>();

            expeditions.ForEach(e => AddExpeditionToDeck(e, deck));

            deck.Shuffle();
            return deck;
        }

        private void AddExpeditionToDeck(ExpeditionType expedition, List<Card> deck)
        {
            for (int value = 2; value <= 10; value++)
                deck.Add(new Card(id: $"{expedition.Code}{value}", color:expedition.Color, value:value));

            string[] wagerCardIds = new string[] { "A", "B", "C" };

            for (int i = 0; i < wagerCardIds.Length; i++)
                deck.Add(new Card(id: $"{expedition.Code}{wagerCardIds[i]}", color: expedition.Color, value: 0));
        }

        private List<ExpeditionType> GetExpeditions()
        {
            return  new List<ExpeditionType>()
                {
                    new ExpeditionType("Blue"),
                    new ExpeditionType("Yellow"),
                    new ExpeditionType("Red"),
                    new ExpeditionType("White"),
                    new ExpeditionType("Green")
                };
        }
    }
}
