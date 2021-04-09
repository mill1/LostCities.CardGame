using LostCities.CardGame.Test.Mocks;
using LostCities.CardGame.WebApi.Interfaces;
using LostCities.CardGame.WebApi.Models;
using LostCities.CardGame.WebApi.Services;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace LostCities.CardGame.Test.ServiceTests
{
    [Collection("Realm tests")]
    public class GameServiceTest: IDisposable
    {
        private readonly IGameService gameService;

        public GameServiceTest()
        {
            ILogger<GameService> mockLogger = MockLogger<GameService>.CreateMockLogger();
            gameService = new GameService(mockLogger);
        }

        [Fact]
        public void GetNewGameTest()
        {
            Game game = gameService.GetNewGame();

            Assert.NotNull(game);
        }

        public void Dispose()
        {
        }
    }
}
