using LostCities.CardGame.WebApi.Interfaces;
using LostCities.CardGame.WebApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
 Bot: thinking...
System.Exception: response did not indicate succes. Result ='"Playing the turn failed.\r\nException:\r\nSystem.InvalidOperationException: Sequence contains no elements\r\n   at System.Linq.ThrowHelper.ThrowNoElementsException()\r\n   at System.Linq.Enumerable.First[TSource](IEnumerable`1 source)\r\n   at LostCities.CardGame.WebApi.Services.GameService.DeterminePlaceCard(ExpeditionCandidate expeditionCandidate) in D:\\repos\\LostCities.CardGame\\LostCities.CardGame.WebApi\\Services\\GameService.cs:line 144\r\n   at LostCities.CardGame.WebApi.Services.GameService.PlayTurn(Game game) in D:\\repos\\LostCities.CardGame\\LostCities.CardGame.WebApi\\Services\\GameService.cs:line 115\r\n   at LostCities.CardGame.WebApi.Controllers.GameController.PlayTurn(Game gameDto) in D:\\repos\\LostCities.CardGame\\LostCities.CardGame.WebApi\\Controllers\\GameController.cs:line 50"'
   at LostCities.CardGame.Console.Services.Http.HandleResponse(HttpResponseMessage response) in D:\repos\LostCities.CardGame\LostCities.CardGame.Console\Services\Http.cs:line 89
   at LostCities.CardGame.Console.Services.Http.PostBotTurn(Game gameDto) in D:\repos\LostCities.CardGame\LostCities.CardGame.Console\Services\Http.cs:line 41
   at LostCities.CardGame.Console.Services.Duel.ProcessTurnBot(Game game) in D:\repos\LostCities.CardGame\LostCities.CardGame.Console\Services\Duel.cs:line 65
   at LostCities.CardGame.Console.Services.Duel.Play() in D:\repos\LostCities.CardGame\LostCities.CardGame.Console\Services\Duel.cs:line 33
   at LostCities.CardGame.Console.UI.Runner.ProcessAnswer(String answer) in D:\repos\LostCities.CardGame\LostCities.CardGame.Console\UI\Runner.cs:line 67
   at LostCities.CardGame.Console.UI.Runner.Run() in D:\repos\LostCities.CardGame\LostCities.CardGame.Console\UI\Runner.cs:line 42
 */

namespace LostCities.CardGame.WebApi.Services
{
    public class GameService : IGameService
    {
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

        public Game PlayTurn(Game game)
        {
            // First sort the cards
            game.BotCards.Cards = game.BotCards.Cards.OrderBy(c => c.ExpeditionType.Name).ThenBy(c => c.Value).ToList();

            var botExpeditionCandidates = GetExpeditionCandidates(game.BotCards, game);

            foreach (var botExpeditionCandidate in botExpeditionCandidates)
            {
                // Current aproach:

                // 1. PLACE A CARD
                // First evaluate the hand per expedition by assigning a 'score' based on different criteria
                // Select the appropiate card of the expedition with the best 'score':
                // - if the expedition does not exist: the hand card with the lowest value
                // - if expedition exists: the hand card following the highest card of the expedition (except wager cards: can be equal)

                // When to discard a card (tricky):
                // - hand card available with value beneath highest value of corresponding player expedition AND
                // - highest score is beneath a defined threshold

                // 2. DRAW A CARD
                // - Draw from the discard pile when you spot a useful card (define)
                // - Draw from the discard pile in order prevent player to draw a high value card based on his expeditions.
                // - Draw from the discard pile to prolong the game (add more cards to expeditions with limited draw pile cards left)
                // - otherwise draw a card from the draw pile.

                EvaluateBotExpeditionCandidate(botExpeditionCandidate, game);
            }

            // TODO For now: never discard a card
            var bestCandidate = botExpeditionCandidates.OrderByDescending(ec => ec.Score).First();

            Card card = DeterminePlaceCard(bestCandidate);

            // TODO if card == null: discard card; Wanneer? zou de score dit niet moeten regelen?

            game.BotCards.MoveCardToPile(card, game.BotExpeditions);
            game.DescriptionLastTurn = $"Bot moved card {card.Id} to expedition {card.ExpeditionType.Name}";

            // TODO For now: always draw a card from the draw pile
            game.DrawPile.DrawCard(game.BotCards);
            game.DescriptionLastTurn += " and drew a card from the draw pile.";

            return game;
        }

        private Card DeterminePlaceCard(ExpeditionCandidate expeditionCandidate)
        {
            // Select the appropiate card of the candidate with the best 'score':
            // - if the expedition does not exist: the hand card with the lowest value
            // - if expedition exists: the hand card following the highest card of the expedition (except wager cards: can be equal)

            if (expeditionCandidate.ExpeditionCards == null)
                return expeditionCandidate.HandCards.Lowest();
            else
            {
                var highestValueExpedition = expeditionCandidate.ExpeditionCards.Highest().Value;

                // TODO anders; zie andere todo plek                             
                var availableCards = expeditionCandidate.HandCards.Where(hc => hc.Value - highestValueExpedition >= 0);

                return availableCards.OrderBy(c => c.Value).First();
            }
        }

        private void EvaluateBotExpeditionCandidate(ExpeditionCandidate expeditionCandidate, Game game)
        {
            // indicator: value of lowest card (compared to corresponding started expedition, if any)
            // indicator: number of hand cards
            // indicator: number of expedition cards
            // indicator: small delta's between hand cards
            // indicator: number of started expeditions

            int highestValueExpedition = expeditionCandidate.ExpeditionCards == null ? -1 : expeditionCandidate.ExpeditionCards.Max(c => c.Value); // TODO werkt dit?

            int scoreLowestCard = expeditionCandidate.HandCards.Min(c => c.Value) - highestValueExpedition;
            scoreLowestCard = 12 - scoreLowestCard; // TODO var

            int scoreHandCardsCount = expeditionCandidate.HandCards.Count() + 1;             // TODO var 
            int scoreExpeditionCardsCount = (expeditionCandidate.ExpeditionCards == null ? 0 : expeditionCandidate.ExpeditionCards.Count()) + 1; // TODO var

            expeditionCandidate.Score = scoreLowestCard * scoreHandCardsCount * scoreExpeditionCardsCount;
        }

        private IEnumerable<ExpeditionCandidate> GetExpeditionCandidates(IPile botCards, Game game)
        {
            var distinctExpeditionTypeCodes = botCards.Cards.Select(c => c.ExpeditionType.Code).Distinct();

            List<ExpeditionCandidate> expeditionCandidates = new List<ExpeditionCandidate>();

            // TODO: bepaal eerst 'available' hand cards: hand cards met een waarde lager dan de hoogste exp. waarde zijn niet 'available'

            foreach (var expeditionTypeCode in distinctExpeditionTypeCodes)
            {
                expeditionCandidates.Add(new ExpeditionCandidate()
                {
                    HandCards = botCards.Cards.Where(c => c.ExpeditionType.Code == expeditionTypeCode),
                    ExpeditionCards = game.BotExpeditions.Where(e => e.Cards.First().ExpeditionType.Code == expeditionTypeCode).FirstOrDefault()?.Cards,
                    Score = 0
                });
            }                
            return expeditionCandidates;
        }


        public Game PlayTurnDummy(Game game)
        {
            Random r = new Random();
            int i = r.Next(0, 7);
            Card randomCard = game.BotCards.Cards.ElementAt(i);

            Card card = GetLowestCardOfBotHandDummy(game, randomCard.ExpeditionType);
            Card highestExpeditionCard = GetHighestCardOfBotExpeditionDummy(game, randomCard.ExpeditionType);

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
        private Card GetLowestCardOfBotHandDummy(Game game, ExpeditionType expeditionType)
        {
            return game.BotCards.Cards.Where(c => c.ExpeditionType.Equals(expeditionType)).Lowest();
        }

        private Card GetHighestCardOfBotExpeditionDummy(Game game, ExpeditionType expeditionType)
        {
            var expedition = game.BotExpeditions.Where(e => e.Cards.First().ExpeditionType.Equals(expeditionType)).FirstOrDefault();

            if (expedition == null)
                return null;

            return expedition.Cards.Where(c => c.ExpeditionType.Equals(expeditionType)).Highest();
        }
    }
}
