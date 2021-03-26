using LostCities.CardGame.WebApi.Models;
using System.Collections.Generic;

namespace LostCities.CardGame.WebApi.Interfaces
{
    public interface IGameService
    {
        public GameCards GetNewGame();
    }
}
