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
            bool playersTurn = DoesPlayersStart();
            Game game = InitializeGame();

            while (game.DrawPile.Cards.Count() > 35) // TODO -> 0 haha
            {
                UI.Console.DisplayGame(game, true); // TODO lw true

                if (playersTurn)
                    ProcessTurnPlayer(game);
                else
                    game = ProcessTurnBot(game);

                playersTurn = !playersTurn;
            }
            HandleGameEnd(game);
        }

        private Game InitializeGame()
        {
            UI.Console.WriteLine(ConsoleColor.DarkCyan, "Shuffling and dealing the deck...");
            WebApi.Dtos.Game gameDto = http.GetNewGame();

            Game game = mapper.MapToModel(gameDto);
            return game;
        }

        private void HandleGameEnd(Game game)
        {
            int scorePlayer = http.PostCalculateScore(mapper.MapToDto(game.PlayerExpeditions));
            int scoreBot = http.PostCalculateScore(mapper.MapToDto(game.BotExpeditions));

            string result = scorePlayer == scoreBot ? "It's a draw!" : $"{(scorePlayer > scoreBot ? "Player" : "Bot")} won!";

            UI.Console.DisplayResultGame(scorePlayer, scoreBot, result);
            UI.Console.DisplayGame(game, revealBotCards: true);
        }

        private Game ProcessTurnBot(Game game)
        {
            UI.Console.WriteLine(ConsoleColor.DarkCyan, game.DescriptionLastTurn);
            UI.Console.WriteLine(new string('-', 77));
            UI.Console.WriteLine("Bot: thinking...");
            WebApi.Dtos.Game gameDto = http.PostBotTurn(mapper.MapToDto(game));
            UI.Console.WriteLine(ConsoleColor.DarkCyan, gameDto.DescriptionLastTurn);
            UI.Console.WriteLine(new string('-', 77));

            return mapper.MapToModel(gameDto);
        }

        private void ProcessTurnPlayer(Game game)
        {
            string answer = "s"; // TODO string literals

            while (answer != "e" && answer != "d")
            {
                answer = GetPlayerOptions(game);

                if (answer == "s")
                {
                    ShowDiscardPileCards(game);
                    continue;
                }
            } 

            Card placeCard = ProcessPlayerPlaceCard(game, answer);
            ProcessPlayerDrawCard(game, placeCard);
        }

        private void ShowDiscardPileCards(Game game)
        {
            IPile discardPile = DetermineDiscardPile(game.DiscardPiles, "From which pile do you want to see the cards?");

            UI.Console.DisplayCardIds(discardPile.Cards);
        }

        private void ProcessPlayerDrawCard(Game game, Card placeCard)
        {
            IPile pile = DetermineFromWhichPileToDraw(game, placeCard, out bool drawFromDiscardPile);

            if (drawFromDiscardPile)
            {
                IPile discardPile = game.DiscardPiles.Where(p => p == pile).First();
                Card drawCard = discardPile.DrawDiscardCard(game, game.PlayerCards);
                game.DescriptionLastTurn = $"Player drew card {drawCard.Id} from the {drawCard.ExpeditionType.Name} discard pile.";
            }
            else
            {
                Card drawCard = pile.DrawCard(game.PlayerCards);
                game.DescriptionLastTurn = $"Player drew card {drawCard.Id} from the draw pile.";
            }
        }

        private Card ProcessPlayerPlaceCard(Game game, string answer)
        {
            Card placeCard = DetermineWhichCardToPlace(answer, game, out bool moveToExpedition);

            List<IPile> piles = moveToExpedition ? game.PlayerExpeditions : game.DiscardPiles;

            game.PlayerCards.MoveCardToPile(placeCard, piles);
            return placeCard;
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

        private Card DetermineWhichCardToPlace(string answer, Game game, out bool moveToExpedition)
        {            
            while (true)
            {                                
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

        private string GetPlayerOptions(Game game)
        {
            string optionShowDiscardPileCards = game.DiscardPiles.Any() ? "\r\n- Show discard pile cards (s)" : string.Empty;

            while (true)
            {                
                UI.Console.WriteLine(ConsoleColor.White,
                "Player: What do you want to do?\r\n" +
                "- Add a card to an expedition (e)\r\n" +
                "- Discard a card (d)" + 
                optionShowDiscardPileCards);

                string answer = UI.Console.ReadLine();

                if (answer == "e" || answer == "d" || answer == "s")
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
                    return DetermineDiscardPile(game.DiscardPiles, "From which pile do you want to draw a card?");
            }
        }

        private IPile DetermineDiscardPile(List<IPile> discardPiles, string message)
        {
            while (true)
            {
                UI.Console.WriteLine(ConsoleColor.White, message);
                UI.Console.DisplayExistingDiscardPiles(discardPiles);

                string answer = UI.Console.ReadLine();

                var discardPile = discardPiles.Where(dp => dp.Cards.First().ExpeditionType.Code.Equals(answer, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (discardPile == null)
                    UI.Console.WriteLine(ConsoleColor.Red, $"Invalid expedition code: {answer.ToUpper()}");
                else
                    return discardPile;
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

        private bool DoesPlayersStart()
        {
            UI.Console.WriteLine(ConsoleColor.White, "Do you want to start? (y/n)");
            string answer = UI.Console.ReadLine();

            return answer.Equals("y", StringComparison.OrdinalIgnoreCase);
        }        
    }
}
