
using LostCities.CardGame.Console.Services;
using System;
using System.Collections.Generic;

namespace LostCities.CardGame.Console.UI
{
    public class Runner
    {
        private const string NewGame = "n";
        private const string Wiki = "w";
        private const string Quit = "q";
        private bool quit;

        private readonly Http http;

        public Runner(Http http)
        {
            this.http = http;

            quit = false;

            string apiName = http.GetWebApiAssemblyProperty("Name");
            string apiVersion = http.GetWebApiAssemblyProperty("Version");

            Console.DisplayAssemblyInfo(apiName, apiVersion);
        }

        public void Run()
        {
            while (!quit)
            {
                Console.DisplayMenu(ConsoleColor.Yellow, GetMenuItems());

                string answer = Console.ReadLine();

                try
                {
                    ProcessAnswer(answer);
                }
                catch (Exception e)
                {
                    Console.WriteLine(ConsoleColor.Red, e);
                }
            }
        }

        private List<string> GetMenuItems()
        {
            return new List<string>
            {
                $"{NewGame}:\tNew game",
                $"{Wiki}:\tShow wiki",
                $"{Quit}:\tQuit application"
            };
        }

        private void ProcessAnswer(string answer)
        {
            switch (answer)
            {
                case NewGame:
                    var tmp = http.Test();
                    break;
                case Wiki:
                    string rawArticleText = http.GetWikipediaRawArticleText("Lost_Cities");
                    Console.Write(ConsoleColor.Green, rawArticleText, beautify: true);
                    break;
                case Quit:
                    quit = true;
                    break;
                default:
                    Console.WriteLine(ConsoleColor.Red, $"Invalid choice: {answer}");
                    break;
            }
        }
    }
}
