using LostCities.CardGame.Test.Mocks;
using LostCities.CardGame.WebApi.Interfaces;
using LostCities.CardGame.WebApi.Models;
using LostCities.CardGame.WebApi.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Xunit;

namespace LostCities.CardGame.Test.ServiceTests
{
    [Collection("Realm tests")]
    public class GameServiceTest: IDisposable
    {
        private readonly IGameService gameService;
        private readonly MockFactory mockFactory;

        public GameServiceTest()
        {
            ILogger<GameService> mockLogger = MockLogger<GameService>.CreateMockLogger();
            gameService = new GameService(mockLogger);
            mockFactory = new MockFactory();
        }

        [Fact]
        public void GetNewGameTest()
        {
            Game game = gameService.GetNewGame();

            Assert.NotNull(game);
        }

        [Fact]
        public void CalculateScoreTest()
        {
            IEnumerable<IPile> expeditions = new List<IPile>() 
            {
                 new Pile(
                        new List<Card>()
                        {
                            mockFactory.CreateCard("B4"),
                            mockFactory.CreateCard("B8"),
                            mockFactory.CreateCard("B10"),
                        }),
                 new Pile(
                        new List<Card>()
                        {
                            mockFactory.CreateCard("Y2"),
                            mockFactory.CreateCard("Y5"),
                            mockFactory.CreateCard("Y8"),
                            mockFactory.CreateCard("Y9"),
                        }),
            };

            Assert.Equal(2, gameService.CalculateScore(expeditions));
        }

        public void Dispose()
        {
        }
    }
}
