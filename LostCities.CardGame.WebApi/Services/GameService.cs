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

        public Game PlayTurnX(Game game)
        {
            // First sort the cards
            game.BotCards.Cards = game.BotCards.Cards.OrderBy(c => c.ExpeditionType.Name).ThenBy(c => c.Value).ToList();

            var botCardsPerExpeditions = GetBotCardsPerExpedition(game.BotCards);

            foreach (var botCardsPerExpedition in botCardsPerExpeditions)
            {
                EvaluateBotCardsPerExpedition(botCardsPerExpedition, game);
            }

            return game;
        }

        private void EvaluateBotCardsPerExpedition(IEnumerable<Card> botCardsPerExpedition, Game game)
        {
            // indicator: low value            
            // indicator: number of cards
            // indicator: small delta's

            var expedition = game.BotExpeditions.Where(e => e.Cards.First().ExpeditionType.Equals(botCardsPerExpedition.First().ExpeditionType)).FirstOrDefault();
            int lastValueExpedition = expedition == null ? -1 : expedition.Cards.Max(c => c.Value);


        }

        private IEnumerable<IEnumerable<Card>> GetBotCardsPerExpedition(IPile botCards)
        {
            var distinctExpeditionTypeCodes = botCards.Cards.Select(c => c.ExpeditionType.Code).Distinct();

            List<IEnumerable<Card>> botCardsPerExpeditions = new List<IEnumerable<Card>>();

            foreach (var expeditionTypeCode in distinctExpeditionTypeCodes)
                botCardsPerExpeditions.Add(botCards.Cards.Where(c => c.ExpeditionType.Code == expeditionTypeCode));

            return botCardsPerExpeditions;
        }


        public Game PlayTurn(Game game)
        {
            Random r = new Random();
            int i = r.Next(0, 7);
            Card randomCard = game.BotCards.Cards.ElementAt(i);

            Card card = GetLowestCardOfBotHand(game, randomCard.ExpeditionType);
            Card highestExpeditionCard = GetHighestCardOfExpedition(game, randomCard.ExpeditionType);

            if (highestExpeditionCard == null)
            {
                if (game.BotExpeditions.Count >= 4)
                {
                    game.BotCards.MoveCardToPile(card, game.DiscardPiles);
                    game.DescriptionLastTurn = $"Bot moved card {card.Id} to discard pile {card.ExpeditionType.Name}";
                }
                else
                {
                    game.BotCards.MoveCardToPile(card, game.BotExpeditions);
                    game.DescriptionLastTurn = $"Bot moved card {card.Id} to expedition {card.ExpeditionType.Name}";
                }
            }
            else
            {
                if (card.Value < highestExpeditionCard.Value)
                {
                    game.BotCards.MoveCardToPile(card, game.DiscardPiles);
                    game.DescriptionLastTurn = $"Bot moved card {card.Id} to discard pile {card.ExpeditionType.Name}";
                }
                else
                {
                    game.BotCards.MoveCardToPile(card, game.BotExpeditions);
                    game.DescriptionLastTurn = $"Bot moved card {card.Id} to expedition {card.ExpeditionType.Name}";
                }
            }

            // Never take from a discard pile :)
            game.DrawPile.DrawCard(game.BotCards);
            game.DescriptionLastTurn += " and drew a card from the draw pile.";

            return game;
        }

        private Card GetLowestCardOfBotHand(Game game, ExpeditionType expeditionType)
        {
            return game.BotCards.Cards.Where(c => c.ExpeditionType.Equals(expeditionType)).OrderBy(x => x.Value).First();
        }

        private Card GetHighestCardOfExpedition(Game game, ExpeditionType expeditionType)
        {
            var expedition = game.BotExpeditions.Where(e => e.Cards.First().ExpeditionType.Equals(expeditionType)).FirstOrDefault();

            if (expedition == null)
                return null;

            return expedition.Cards.Where(c => c.ExpeditionType.Equals(expeditionType)).OrderByDescending(x => x.Value).First();
        }
    }
}
