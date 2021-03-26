using System.Collections.Generic;
using System.Text;
using System;
using LostCities.CardGame.WebApi.Services;

namespace LostCities.CardGame.Console.Services
{
    public class Duel
    {
        public void Play(WebApi.Dtos.Game game)
        {
            //UI.Console.WriteLine(System.ConsoleColor.Cyan, 666);

            bool ended = false;

            // Set things up
            // Bot should not know human's cards obviously.

            //while (!ended)
            //{
            //    throw new Exception("TODO");
            //}
        }

        private bool PlayerStarts()
        {
            UI.Console.WriteLine("Do you want to start? (y/n)");
            string answer = UI.Console.ReadLine();
            return answer.Equals("y", StringComparison.OrdinalIgnoreCase);
        }
    }
}
