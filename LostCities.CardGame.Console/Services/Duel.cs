using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;
using LostCities.CardGame.WebApi.Interfaces;

namespace LostCities.CardGame.Console.Services
{
    public class Duel
    {
        private readonly IMapper mapper;
        private readonly Http http;

        public Duel(IMapper mapper, Http http)
        {
            this.mapper = mapper;
            this.http = http;
        }

        public void Play()
        {
            bool playersTurn = PlayersTurn();

            UI.Console.WriteLine(ConsoleColor.DarkCyan, "Shuffling and dealing the deck...");
            WebApi.Dtos.Game gameDto = http.GetNewGame();

            WebApi.Models.Game game = mapper.MapToModel(gameDto);

            while (game.DrawPile.Cards.Count() > 0)
            {
                UI.Console.DisplayGame(game);

                if (playersTurn)
                    ProcessTurnPlayer(game);
                else
                    game = ProcessTurnBot(game);

                playersTurn = !playersTurn;
             }

             // TODO: eindstand uitrekenen
             //TODO : winnaar bepalen en kaarten bot tonen
        }

        private WebApi.Models.Game ProcessTurnBot(WebApi.Models.Game game)
        {
            UI.Console.WriteLine(new string('-', 22));
            UI.Console.WriteLine("Bot: thinking...");
            WebApi.Dtos.Game gameDto = http.PostBotTurn(mapper.MapToDto(game));
            UI.Console.WriteLine(new string('-', 22));

            return mapper.MapToModel(gameDto);
        }

        private void ProcessTurnPlayer(WebApi.Models.Game game)
        {
            WebApi.Models.Card card = DetermineWhichCardToPlace(game, out bool moveToExpedition);

            List<WebApi.Models.Pile> piles = moveToExpedition ? game.PlayerExpeditions : game.DiscardPiles;

            game.PlayerCards.MoveCardToPile(card, piles);

            WebApi.Models.Pile pile = DetermineFromWhichPileToDraw(game);

            pile.MoveLastCardTo(game.PlayerCards.Cards);
        }

        private WebApi.Models.Pile DetermineFromWhichPileToDraw(WebApi.Models.Game game)
        {
            if (game.DiscardPiles.Count == 0)
                return game.DrawPile;

            // TODO hierzoods
            //while (true)
            //{

            //}
            return null;
        }

        private WebApi.Models.Card DetermineWhichCardToPlace(WebApi.Models.Game game, out bool moveToExpedition)
        {
            while (true)
            {
                string answer = GetPlayerOptionsPlaceCard();

                if (!ValidPlayerOptionPlaceCard(answer))
                    continue;

                string cardId = GetCardToPlace(answer);

                if (!CardExistById(cardId, game.PlayerCards.Cards))
                    continue;

                WebApi.Models.Card card = game.PlayerCards.Cards.Where(c => c.Id.Equals(cardId, StringComparison.OrdinalIgnoreCase)).First();

                if (answer == "e")
                    if (InvalidExpeditionCard(game.PlayerExpeditions, card))
                        continue;

                moveToExpedition = answer == "e";
                return card;
            }
        }

        private bool InvalidExpeditionCard(List<WebApi.Models.Pile> expeditions, WebApi.Models.Card card)
        {
            // Is the selected card lower than the max value of the corresponding expedition (if any)?
            List<WebApi.Models.Card> flattened = expeditions.SelectMany(cc => cc.Cards).ToList();

            var highestCard = flattened.Where(c => c.ExpeditionType.Code == card.ExpeditionType.Code).OrderByDescending(c => c.Value).FirstOrDefault();

            if (highestCard != null)
                if (card.Value < highestCard.Value)
                {
                    UI.Console.WriteLine(ConsoleColor.Red, $"Card to add to expedition cannot be lower than {highestCard.Value}");
                    return true;
                }
            return false;
        }

        private bool CardExistById(string cardId, List<WebApi.Models.Card> cards)
        {
            if (cards.Any(c => c.Id.Equals(cardId, StringComparison.OrdinalIgnoreCase)))
                return true;
            else
            {
                UI.Console.WriteLine(ConsoleColor.Red, $"Card not present in player hand: {cardId}");
                return false;
            }
        }

        private bool ValidPlayerOptionPlaceCard(string answer)
        {
            if (answer != "e" && answer != "d")
            {
                UI.Console.WriteLine(ConsoleColor.Red, $"Invalid option: {answer}");
                return false;
            }
            return true;
        }

        private string GetPlayerOptionsPlaceCard()
        {
            UI.Console.WriteLine(ConsoleColor.White,
                "Player: What do you want to do?\r\n" +
                "- Add a card to an expedition (e)\r\n" +
                "- Discard a card (d)");

            return UI.Console.ReadLine();
        }

        private static string GetCardToPlace(string answer)
        {
            switch (answer)
            {
                case "e":
                    UI.Console.WriteLine(ConsoleColor.White, "Enter card to add to expedition:");
                    break;
                case "d":
                    UI.Console.WriteLine(ConsoleColor.White, "Enter card to discard:");
                    break;
            }

            return UI.Console.ReadLine();
        }

        private bool PlayersTurn()
        {
            UI.Console.WriteLine(ConsoleColor.White, "Do you want to start? (y/n)");
            string answer = UI.Console.ReadLine();

            return answer.Equals("y", StringComparison.OrdinalIgnoreCase);
        }        
    }
}
