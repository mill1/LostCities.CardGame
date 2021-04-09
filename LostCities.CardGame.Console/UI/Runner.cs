
using LostCities.CardGame.Console.Services;
using System;
using System.Collections.Generic;

namespace LostCities.CardGame.Console.UI
{
    public class Runner
    {
        private const string NewDuel = "n";
        private const string Wiki = "w";
        private const string ApiProps = "a";
        private const string Quit = "q";
        private bool quit;

        private readonly Http http;
        private readonly Duel duel;

        public Runner(Http http, Duel duel)
        {
            this.http = http;
            this.duel = duel;

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
                $"{NewDuel}:\tNew duel",
                $"{Wiki}:\tShow wiki",
                $"{ApiProps}:\tShow API properties",
                $"{Quit}:\tQuit application"
            };
        }

        private void ProcessAnswer(string answer)
        {
            switch (answer)
            {
                case NewDuel:                    
                    duel.Play();
                    break;
                case Wiki:
                    string rawArticleText = http.GetWikipediaRawArticleText("Lost_Cities");
                    Console.Write(ConsoleColor.Green, rawArticleText, beautify: true);
                    break;
                case ApiProps:
                    string assemblyProperties = http.GetWebApiAssemblyProperties();
                    Console.Write(ConsoleColor.Green, assemblyProperties, beautify: true);
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
