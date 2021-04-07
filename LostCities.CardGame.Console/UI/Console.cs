using LostCities.CardGame.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SysConsole = System.Console;

namespace LostCities.CardGame.Console.UI
{
    // Wrapper for System.Console
    public static class Console
    {
        private static readonly int NumberOfChars = 50;
        private static ConsoleColor foregroundColor;

        public static ConsoleColor ForegroundColor
        {
            get { return foregroundColor; }
            set
            {
                SysConsole.ForegroundColor = value;
                foregroundColor = value;
            }
        }

        public static void Write(object value)
        {
            SysConsole.Write(value);
        }

        public static void Write(ConsoleColor color, object value, bool beautify=false)
        {
            ForegroundColor = color;

            if (!beautify)
                SysConsole.Write(value);
            else
            {
                var array = value.ToString().Split("\\n").ToList();
                array.ForEach(l => WriteLine(ConsoleColor.Green, l));
            }

            SysConsole.ResetColor();
        }

        public static void WriteLine(object value)
        {
            SysConsole.WriteLine(value);
        }

        public static void WriteLine(ConsoleColor color, object value)
        {
            ForegroundColor = color;
            SysConsole.WriteLine(value);
            SysConsole.ResetColor();
        }

        public static string ReadLine()
        {
            return SysConsole.ReadLine();
        }

        public static void DisplayMenu(ConsoleColor consoleColor, List<string> menuItems)
        {
            ForegroundColor = consoleColor;

            SysConsole.WriteLine(new String('-', NumberOfChars));

            foreach (string menuItem in menuItems)
                SysConsole.WriteLine(menuItem);

            SysConsole.WriteLine(new String('-', NumberOfChars));
            SysConsole.ResetColor();
        }

        public static void DisplayAssemblyInfo(string assemblyName, string assemblyVersion)
        {
            ForegroundColor = ConsoleColor.Green;
            SysConsole.WriteLine(new String('*', NumberOfChars) + "\r\n");
            SysConsole.WriteLine($"      {assemblyName}");
            SysConsole.WriteLine($"      version: {assemblyVersion}" + "\r\n");
            SysConsole.WriteLine(new String('*', NumberOfChars) + "\r\n");
            SysConsole.ResetColor();
        }

        public static void DisplayGame(Game game)
        {
            DisplayDrawPile(game.DrawPile);
            DisplayBotCards(game);
            DisplayDiscardPiles(game.DiscardPiles);
            DisplayPlayerCards(game);
        }

        private static void DisplayDrawPile(CardCollection drawPile)
        {
            Write(ConsoleColor.Cyan, "\r\nDraw pile:      ");
            Write(ConsoleColor.Gray, "XX");
            Write(ConsoleColor.DarkCyan, $"  ({drawPile.Cards.Count()} card left)\r\n\r\n");
        }

        private static void DisplayDiscardPiles(IEnumerable<CardCollection> discardPiles)
        {
            Write(ConsoleColor.Cyan, "\r\nDiscard piles:  ");

            if (!discardPiles.Any())
                Write(ConsoleColor.DarkCyan, "[no discard piles yet]");
            else
            {
                foreach (var discardPile in discardPiles)
                    DisplayCard(discardPile.Cards.Last());
            }
            WriteLine("\r\n");
        }


        private static void DisplayBotCards(Game game)
        {
            WriteLine(ConsoleColor.Cyan, "BOT: (that's your opponent)");

            Write(ConsoleColor.Cyan, "Cards:          ");

            foreach (Card card in game.BotCards.Cards)
                Write(ConsoleColor.Gray, "XX  ");

            WriteLine("");

            DisplayExpeditions(game.BotExpeditions);
        }

        private static void DisplayPlayerCards(Game game)
        {
            WriteLine(ConsoleColor.Cyan, "PLAYER: (that's you)");

            DisplayExpeditions(game.PlayerExpeditions);

            game.PlayerCards.Cards = game.PlayerCards.Cards.OrderBy(c => c.Id).ToList();

            Write(ConsoleColor.Cyan, "Cards:          ");

            foreach (Card card in game.PlayerCards.Cards)
                DisplayCard(card);

            WriteLine("\r\n");
        }

        private static void DisplayExpeditions(IEnumerable<CardCollection> expeditions)
        {
            Write(ConsoleColor.Cyan, "Expeditions:    ");

            if (!expeditions.Any())
                Write(ConsoleColor.DarkCyan, "[no expeditions yet]");
            else
            {
                foreach (var expedition in expeditions)
                    DisplayCard(expedition.Cards.Last());
            }

            WriteLine("");
        }

        private static void DisplayCard(Card card)
        {
            Write(card.ExpeditionType.Color, $"{card.Id}  ");
        }
    }
}
