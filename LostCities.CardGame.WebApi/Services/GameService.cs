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

        public Game GetNewGame()
        {
            List<ExpeditionType> expeditions = GetExpeditions();
            List<Card> deck = InitializeDeck(expeditions);            
            Game game = new Game(deck);

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
                deck.Add(new Card(id: $"{expedition.Code}{value}", expedition, value));

            string[] wagerCardIds = new string[] { "A", "B", "C" };

            for (int i = 0; i < wagerCardIds.Length; i++)
                deck.Add(new Card(id: $"{expedition.Code}{wagerCardIds[i]}", expedition,  0));
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

        public int CalculateScore(IEnumerable<IPile> expeditions)
        {
            int score = 0;

            foreach (var expedition in expeditions)
                score += CalculateScore(expedition);

            return score;
        }

        private int CalculateScore(IPile expedition)
        {
            int score = 0;

            foreach (var card in expedition.Cards)
                score += card.Value;

            score -= Constants.ExpeditionCosts;

            int wagerCardsCount = expedition.Cards.Where(c => c.Value == 0).Count();

            score *= wagerCardsCount + 1;
            score += expedition.Cards.Count < Constants.MinimumExpeditionLengthForBonus ? 0 : Constants.ExpeditionBonus;

            return score;
        }
    }
}
