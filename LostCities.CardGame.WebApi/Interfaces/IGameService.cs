using LostCities.CardGame.WebApi.Models;
using System.Collections.Generic;

namespace LostCities.CardGame.WebApi.Interfaces
{
    public interface IGameService
    {
        public Game GetNewGame();
        public int CalculateScore(IEnumerable<IPile> expeditions);
    }
}
