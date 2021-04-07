using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;

namespace LostCities.CardGame.Console.Services
{
    public class Duel
    {
        private readonly Http http;

        public Duel(Http http)
        {
            this.http = http;
        }

        public void Play()
        {
            bool playerStarts = PlayerStarts();

            UI.Console.WriteLine(ConsoleColor.Cyan, "Shuffling and dealing the deck...");
            WebApi.Dtos.Game gameDto = http.GetNewGame();

            WebApi.Models.Game game = MapToModel(gameDto);

            bool ended = false;

            while (!ended)
            {
                UI.Console.DisplayGame(game);

                if (playerStarts)
                {
                    ProcessPlayerTurn(game);
                }
                else
                {
                    ProcessBotTurn(game);
                }                    

                playerStarts = !playerStarts;
             }
        }

        private void ProcessBotTurn(WebApi.Models.Game game)
        {
            // TODO
            UI.Console.WriteLine(new string('-', 22));
            UI.Console.WriteLine("Bot: thinking...");
            // http call
            // TODO dummy move
            UI.Console.WriteLine("Bot moved card XY from A to B and drew a card from the draw pile");
            UI.Console.WriteLine(ConsoleColor.Gray, "(press any key to continue)");
            UI.Console.WriteLine(new string('-', 22));
            UI.Console.ReadLine();
        }

        private void ProcessPlayerTurn(WebApi.Models.Game game)
        {
            WebApi.Models.Card card = DetermineWhichCardToPlay(game, out bool moveToExpedition);

            List<WebApi.Models.CardCollection> cardCollections = moveToExpedition ? game.PlayerExpeditions : game.DiscardPiles;
            game.PlayerCards.MoveCardToCardCollection(card, cardCollections);

            game.DrawPile.MoveLastCardTo(game.PlayerCards.Cards);
        }

        private WebApi.Models.Card DetermineWhichCardToPlay(WebApi.Models.Game game, out bool moveToExpedition)
        {
            while (true)
            {
                string answer = GetPlayerOption();

                if (answer != "e" && answer != "d")
                {
                    UI.Console.WriteLine(ConsoleColor.Red, $"Invalid choice: {answer}");
                    continue;
                }

                string cardId = GetCardToPlay(answer);

                if (!game.PlayerCards.Cards.Any(c => c.Id.Equals(cardId)))
                    UI.Console.WriteLine(ConsoleColor.Red, $"Card not present in player hand: {cardId}");

                moveToExpedition = answer == "e";
                return game.PlayerCards.Cards.Where(c => c.Id == cardId).First();
            }
        }

        private string GetPlayerOption()
        {
            UI.Console.WriteLine(ConsoleColor.Cyan,
                "Player: What do you want to do?\r\n" +
                "Add a card to an expedition (e)\r\n" +
                "Discard a card (d)");

            return UI.Console.ReadLine();
        }

        private static string GetCardToPlay(string answer)
        {
            switch (answer)
            {
                case "e":
                    UI.Console.WriteLine(ConsoleColor.Cyan, "Enter card to add to expedition:");
                    break;
                case "d":
                    UI.Console.WriteLine(ConsoleColor.Cyan, "Enter card to discard:");
                    break;
            }

            return UI.Console.ReadLine();
        }

        private bool PlayerStarts()
        {
            UI.Console.WriteLine("Do you want to start? (y/n)");
            string answer = UI.Console.ReadLine();

            return answer.Equals("y", StringComparison.OrdinalIgnoreCase);
        }

        private WebApi.Models.Game MapToModel(WebApi.Dtos.Game gameDto)
        {
            WebApi.Models.Game game = new WebApi.Models.Game(
                new WebApi.Models.CardCollection(MapToModel(gameDto.PlayerCards)),
                new WebApi.Models.CardCollection(MapToModel(gameDto.BotCards)),
                new WebApi.Models.CardCollection(MapToModel(gameDto.DrawPile)),
                MapToModel(gameDto.PlayerExpeditions),
                MapToModel(gameDto.BotExpeditions),
                MapToModel(gameDto.DiscardPiles));
                
            return game;
        }
                                                            
        private List<WebApi.Models.CardCollection> MapToModel(IEnumerable<IEnumerable<WebApi.Dtos.Card>> cardCollectionsDto)
        {
            List<WebApi.Models.CardCollection> cardCollections = new List<WebApi.Models.CardCollection>();

            foreach (IEnumerable<WebApi.Dtos.Card> cardsDto in cardCollectionsDto)
                cardCollections.Add(new WebApi.Models.CardCollection(MapToModel(cardsDto)));

            return cardCollections;
        }


        private IEnumerable<WebApi.Models.Card> MapToModel(IEnumerable<WebApi.Dtos.Card> cardsDto)
        {
            List<WebApi.Models.Card> cards = new List<WebApi.Models.Card>();

            foreach (var cardDto in cardsDto)
                cards.Add(MapToModel(cardDto));

            return cards;
        }


        private WebApi.Models.Card MapToModel(WebApi.Dtos.Card cardDto)
        {
            return new WebApi.Models.Card(cardDto.Id, new WebApi.Models.ExpeditionType(cardDto.ExpeditionType) , cardDto.Value);
        }
    }
}
