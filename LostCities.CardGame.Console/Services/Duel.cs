using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;
using LostCities.CardGame.WebApi.Interfaces;
using LostCities.CardGame.WebApi.Models;

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

            Game game = mapper.MapToModel(gameDto);

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

        private Game ProcessTurnBot(Game game)
        {
            UI.Console.WriteLine(new string('-', 22));
            UI.Console.WriteLine("Bot: thinking...");
            WebApi.Dtos.Game gameDto = http.PostBotTurn(mapper.MapToDto(game));
            UI.Console.WriteLine(new string('-', 22));

            return mapper.MapToModel(gameDto);
        }

        private void ProcessTurnPlayer(Game game)
        {
            Card card = DetermineWhichCardToPlace(game, out bool moveToExpedition);

            List<IPile> piles = moveToExpedition ? game.PlayerExpeditions : game.DiscardPiles;

            game.PlayerCards.MoveCardToPile(card, piles);

            UI.Console.DisplayGame(game);

            IPile pile = DetermineFromWhichPileToDraw(game, card, out bool drawFromDiscardPile);

            if (drawFromDiscardPile)
            {
                IPile discardPile = game.DiscardPiles.Where(p => p == pile).First();

                discardPile.DrawDiscardCard(game, game.PlayerCards);
            }
            else
                pile.DrawCard(game.PlayerCards);

        }

        private IPile DetermineFromWhichPileToDraw(Game game, Card placedCard, out bool drawFromDiscardPile)
        {
            if (game.DiscardPiles.Count == 0)
            {
                drawFromDiscardPile = false;
                return game.DrawPile;
            }

            while (true)
            {
                string answer = GetPlayerOptionsDrawCard();
                var pile = GetPileToDrawFrom(answer, game, out drawFromDiscardPile);

                if (pile == null)
                    continue;

                if (pile.Cards.Last() == placedCard)
                {
                    UI.Console.WriteLine(ConsoleColor.Red, "It is not allowed to draw the card you just placed.");
                    continue;
                }

                return pile;
            }
        }

        private Card DetermineWhichCardToPlace(Game game, out bool moveToExpedition)
        {
            while (true)
            {
                string answer = GetPlayerOptionsPlaceCard();

                string cardId = GetCardToPlace(answer);

                if (!CardExistsById(cardId, game.PlayerCards.Cards))
                    continue;

                Card card = game.PlayerCards.Cards.Where(c => c.Id.Equals(cardId, StringComparison.OrdinalIgnoreCase)).First();

                if (answer == "e")
                    if (InvalidExpeditionCard(game.PlayerExpeditions, card))
                        continue;

                moveToExpedition = answer == "e";
                return card;
            }
        }

        private bool InvalidExpeditionCard(List<IPile> expeditions, Card card)
        {
            // Is the selected card lower than the max value of the corresponding expedition (if any)?
            List<Card> flattened = expeditions.SelectMany(pile => pile.Cards).ToList();

            var highestCard = flattened.Where(c => c.ExpeditionType.Code == card.ExpeditionType.Code).OrderByDescending(c => c.Value).FirstOrDefault();

            if (highestCard != null)
                if (card.Value < highestCard.Value)
                {
                    UI.Console.WriteLine(ConsoleColor.Red, $"Card to add to expedition cannot be lower than {highestCard.Value}");
                    return true;
                }
            return false;
        }

        private bool CardExistsById(string cardId, List<Card> cards)
        {
            if (cards.Any(c => c.Id.Equals(cardId, StringComparison.OrdinalIgnoreCase)))
                return true;
            else
            {
                UI.Console.WriteLine(ConsoleColor.Red, $"Card not present in player hand: {cardId}");
                return false;
            }
        }

        private string GetPlayerOptionsPlaceCard()
        {
            while (true)
            {
                UI.Console.WriteLine(ConsoleColor.White,
                "Player: What do you want to do?\r\n" +
                "- Add a card to an expedition (e)\r\n" +
                "- Discard a card (d)");

                string answer = UI.Console.ReadLine();

                if (answer == "e" || answer == "d")
                    return answer;
                else
                    UI.Console.WriteLine(ConsoleColor.Red, $"Invalid option: {answer}");                
            }            
        }

        private string GetPlayerOptionsDrawCard()
        {
            while (true)
            {
                UI.Console.WriteLine(ConsoleColor.White,
                "Player: From which pile do you want to draw a card?\r\n" +
                "- From the draw pile (p)\r\n" +
                "- From a discard pile (d)");

                string answer = UI.Console.ReadLine();

                if (answer == "p" || answer == "d")
                    return answer;
                else
                    UI.Console.WriteLine(ConsoleColor.Red, $"Invalid option: {answer}");
            }
        }

        private IPile GetPileToDrawFrom(string answer, Game game, out bool drawFromDiscardPile)
        {
            if (answer == "p")
            {
                drawFromDiscardPile = false;
                return game.DrawPile;
            }                
            else // answer == "d" (discard piles), other options have been handled
            {
                drawFromDiscardPile = true;

                if (game.DiscardPiles.Count == 1)
                    return game.DiscardPiles.First();                
                else                                 
                    return DetermineFromWhichDiscardPileToDraw(game.DiscardPiles);
            }
        }

        private IPile DetermineFromWhichDiscardPileToDraw(List<IPile> discardPiles)
        {
            while (true)
            {
                UI.Console.WriteLine(ConsoleColor.White, "From which pile do you want to draw a card?");

                UI.Console.DisplayExistingDiscardPileNames(discardPiles);

                // TODO
                return null;
            }
        }

        private string GetCardToPlace(string answer)
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
