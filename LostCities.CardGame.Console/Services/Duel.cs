using System.Collections.Generic;
using System.Text;
using System;
using LostCities.CardGame.WebApi.Services;
using LostCities.CardGame.WebApi.Dtos;
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
            Game game = http.GetNewGame();                       

            bool ended = false;

            // Bot should not know human's cards obviously (and the other way around).

            while (!ended)
            {
                DisplayGame(game);

                if (playerStarts)
                {
                    UI.Console.WriteLine("Player: What do you want to do?\r\n1.\r\n2. etc.");
                    string tmp = UI.Console.ReadLine();
                }
                else
                {
                    
                    // TODO
                    UI.Console.WriteLine(new string('-', 22));
                    UI.Console.WriteLine("Bot: thinking...");
                    // http call
                    UI.Console.WriteLine("Bot moved card XY from A to B and drew a card from the draw pile");
                    UI.Console.WriteLine(ConsoleColor.Gray, "(press any key to continue)");
                    UI.Console.WriteLine(new string('-', 22));
                    UI.Console.ReadLine();
                }                    

                playerStarts = !playerStarts;
             }
        }

        // TODO: naar UI
        private void DisplayGame(Game game)
        {
            DisplayDrawPile(game.DrawPile);
            DisplayBotCards(game);
            DisplayDiscardPiles(game.DiscardPiles);
            DisplayPlayerCards(game);
        }

        private void DisplayDrawPile(IEnumerable<Card> drawPile)
        {
            UI.Console.Write(ConsoleColor.Cyan, "\r\nDraw pile:      ");
            UI.Console.Write(ConsoleColor.Gray, "XX");
            UI.Console.Write(ConsoleColor.DarkCyan, $"  ({drawPile.Count()} card left)\r\n\r\n");
        }

        private void DisplayDiscardPiles(IEnumerable<IEnumerable<Card>> discardPiles)
        {
            //System.Console.WriteLine("\r\n666\r\n");
            UI.Console.Write(ConsoleColor.Cyan, "\r\nDiscard piles:  ");

            if (!discardPiles.Any())
                UI.Console.Write(ConsoleColor.DarkCyan, "[no discard piles yet]");
            else
            {
                // TODO
                foreach (var discardPile in discardPiles)
                    System.Console.WriteLine(discardPile.Count());
            }            

            UI.Console.WriteLine("\r\n");
        }


        private void DisplayBotCards(Game game)
        {
            UI.Console.WriteLine(ConsoleColor.Cyan, "BOT: (that's your opponent)");

            UI.Console.Write(ConsoleColor.Cyan, "Cards:          ");

            foreach (Card card in game.PlayerCards)
                UI.Console.Write(ConsoleColor.Gray, "XX  ");

            UI.Console.WriteLine("");

            DisplayExpeditions(game.PlayerExpeditions);
        }

        private void DisplayPlayerCards(Game game)
        {
            UI.Console.WriteLine(ConsoleColor.Cyan, "PLAYER: (that's you)");

            DisplayExpeditions(game.PlayerExpeditions);            

            game.PlayerCards = game.PlayerCards.OrderBy(c => c.Id);

            UI.Console.Write(ConsoleColor.Cyan, "Cards:          ");
            
            foreach (Card card in game.PlayerCards)
                PrintCard(card);

            UI.Console.WriteLine("\r\n");
        }

        private void DisplayExpeditions(IEnumerable<IEnumerable<Card>> expeditions)
        {
            UI.Console.Write(ConsoleColor.Cyan, "Expeditions:    ");            

            if (!expeditions.Any())
                UI.Console.Write(ConsoleColor.DarkCyan, "[no expeditions yet]");
            else
            {
                // TODO
                foreach (var expedition in expeditions)
                    System.Console.WriteLine(expedition.Count());
            }

            UI.Console.WriteLine("");
        }

        private void PrintCard(Card card)
        {
            Enum.TryParse(card.Color, out ConsoleColor consoleColor);
            ConsoleColor color = consoleColor;

            UI.Console.Write(color, $"{card.Id}  ");
        }

        private bool PlayerStarts()
        {
            UI.Console.WriteLine("Do you want to start? (y/n)");
            string answer = UI.Console.ReadLine();
            return answer.Equals("y", StringComparison.OrdinalIgnoreCase);
        }
    }
}
