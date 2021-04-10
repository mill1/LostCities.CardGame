using LostCities.CardGame.WebApi.Interfaces;
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

        public static void DisplayGame(Game game, bool revealBotCards=false)
        {
            DisplayDrawPile(game.DrawPile);
            DisplayBotCards(game, revealBotCards);            
            DisplayDiscardPiles(game.DiscardPiles);            
            DisplayPlayerCards(game);
        }

        private static void DisplayDrawPile(IPile drawPile)
        {
            Write(ConsoleColor.Cyan, "\r\nDraw pile:          ");
            Write(ConsoleColor.Gray, "XX");
            Write(ConsoleColor.DarkCyan, $"  ({drawPile.Cards.Count()} cards left)\r\n\r\n");
        }

        private static void DisplayDiscardPiles(IEnumerable<IPile> discardPiles)
        {
            Write(ConsoleColor.Cyan, "Discard piles:      ");

            if (!discardPiles.Any())
                Write(ConsoleColor.DarkCyan, "[no discard piles yet]");
            else
            {
                foreach (var discardPile in discardPiles)
                    DisplayCardId(discardPile.Cards.Last());

                Write(ConsoleColor.Cyan, "\r\nDiscard cards #:    ");

                foreach (var discardPile in discardPiles)
                    DisplayDiscardPileCount(discardPile.Cards);

            }
            SysConsole.WriteLine("\r\n");
        }

        private static void DisplayBotCards(Game game, bool revealBotCards)
        {
            WriteLine(ConsoleColor.Cyan, "BOT: (that's your opponent)");

            Write(ConsoleColor.Cyan, "Cards:              ");

            foreach (Card card in game.BotCards.Cards)
            {
                if(revealBotCards)
                    DisplayCardId(card);
                else
                    Write(ConsoleColor.Gray, "XX  ");
            }
                

            SysConsole.WriteLine();

            DisplayExpeditions(game.BotExpeditions);

            if(!game.BotExpeditions.Any())
                SysConsole.WriteLine();
        }

        private static void DisplayPlayerCards(Game game)
        {
            WriteLine(ConsoleColor.Cyan, "PLAYER: (that's you)");

            DisplayExpeditions(game.PlayerExpeditions);

            game.PlayerCards.Cards = game.PlayerCards.Cards.OrderBy(c => c.ExpeditionType.Name).ThenBy(c => c.Value).ToList();

            Write(ConsoleColor.Cyan, "Cards:              ");

            foreach (Card card in game.PlayerCards.Cards)
                DisplayCardId(card);

            WriteLine("\r\n");
        }

        private static void DisplayExpeditions(IEnumerable<IPile> expeditions)
        {
            Write(ConsoleColor.Cyan, "Expeditions:        ");            

            if (!expeditions.Any())
                WriteLine(ConsoleColor.DarkCyan, "[no expeditions yet]");
            else
            {                
                DisplayExistingExpeditions(expeditions);
                SysConsole.WriteLine();
            }
        }

        private static void DisplayExistingExpeditions(IEnumerable<IPile> expeditions)
        {
            int maxCount = expeditions.Select(pile => pile.Cards.Count).Max();

            for (int i = 1; i <= maxCount; i++)
            {
                DisplayExpeditionsRecord(expeditions, i);
            }
        }

        private static void DisplayExpeditionsRecord(IEnumerable<IPile> expeditions, int i)
        {
            if (i > 1)
                Write(new string(' ', 20));

            foreach (var expedition in expeditions)
            {
                if (i > expedition.Cards.Count)
                    DisplayCardId(new Card("", new ExpeditionType("Blue"), 0));
                else
                    DisplayCardId(expedition.Cards.ElementAt(i - 1));
            }
            SysConsole.WriteLine();
        }

        public static void DisplayExistingDiscardPiles(List<IPile> discardPiles)
        {
            Write(ConsoleColor.White, "Discard piles:      ");

            foreach (var pile in discardPiles)
            {
                Card card = pile.Cards.First();

                Write(card.ExpeditionType.Color, $"{card.ExpeditionType.Name} ({card.ExpeditionType.Code.ToLower()})  ");
            }
            SysConsole.WriteLine();
        }

        public static void DisplayResultGame(int scorePlayer, int scoreBot, string result)
        {
            WriteLine(ConsoleColor.Magenta, new String('#', NumberOfChars));
            WriteLine(ConsoleColor.Magenta, $"Score player: {scorePlayer}");
            WriteLine(ConsoleColor.Magenta, $"Score bot:    {scoreBot}");
            WriteLine(ConsoleColor.Magenta, result);
            WriteLine(ConsoleColor.Magenta, new String('#', NumberOfChars));

        }

        public static void DisplayCardIds(IEnumerable<Card> cards)
        {
            Write(ConsoleColor.White, "Card id's:          ");

            foreach (var card in cards)
                DisplayCardId(card);

            SysConsole.WriteLine();
        }

        private static void DisplayCardId(Card card)
        {            
            Write(card.ExpeditionType.Color, card.Id.PadRight(4, ' '));
        }

        private static void DisplayDiscardPileCount(List<Card> cards)
        {
            string count = $"[{cards.Count}]";

            Write(cards.First().ExpeditionType.Color, count.PadRight(4, ' '));
        }
    }
}
