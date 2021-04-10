using LostCities.CardGame.Test.Mocks;
using LostCities.CardGame.WebApi;
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
        public void CalculateScoreBasicTest()
        {
            int expected = (22 - Constants.ExpeditionCosts) + (24 - Constants.ExpeditionCosts);

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

            Assert.Equal(expected, gameService.CalculateScore(expeditions));
        }

        [Fact]
        public void CalculateScoreWagerCardsTest()
        {
            int expected = (22 - Constants.ExpeditionCosts) * 3;

            IEnumerable<IPile> expeditions = new List<IPile>()
            {
                 new Pile(
                        new List<Card>()
                        {
                            mockFactory.CreateCard("BA"),
                            mockFactory.CreateCard("BB"),
                            mockFactory.CreateCard("B4"),
                            mockFactory.CreateCard("B8"),
                            mockFactory.CreateCard("B10"),
                        })
            };

            Assert.Equal(expected, gameService.CalculateScore(expeditions));
        }

        [Fact]
        public void CalculateScoreBonusTest()
        {
            int expected = (44 - Constants.ExpeditionCosts) + Constants.ExpeditionBonus;

            IEnumerable<IPile> expeditions = new List<IPile>()
            {
                 new Pile(
                        new List<Card>()
                        {
                            mockFactory.CreateCard("B2"),
                            mockFactory.CreateCard("B3"),
                            mockFactory.CreateCard("B4"),
                            mockFactory.CreateCard("B5"),
                            mockFactory.CreateCard("B6"),
                            mockFactory.CreateCard("B7"),
                            mockFactory.CreateCard("B8"),
                            mockFactory.CreateCard("B9"),
                        })
            };

            Assert.Equal(expected, gameService.CalculateScore(expeditions));
        }

        [Fact]
        public void CalculateScoreBadTest()
        {
            int expected = (0 - Constants.ExpeditionCosts) * 3;

            IEnumerable<IPile> expeditions = new List<IPile>()
            {
                 new Pile(
                        new List<Card>()
                        {
                            mockFactory.CreateCard("BA"),
                            mockFactory.CreateCard("BB"),
                        })
            };

            Assert.Equal(expected, gameService.CalculateScore(expeditions));
        }

        [Fact]
        public void CalculateScoreAdvancedTest()
        {
            int expected = ((43 - Constants.ExpeditionCosts) * 3) + Constants.ExpeditionBonus +
                            (24 - Constants.ExpeditionCosts);

            IEnumerable<IPile> expeditions = new List<IPile>()
            {
                 new Pile(
                        new List<Card>()
                        {
                            mockFactory.CreateCard("BA"),
                            mockFactory.CreateCard("BC"),
                            mockFactory.CreateCard("B3"),
                            mockFactory.CreateCard("B4"),
                            mockFactory.CreateCard("B5"),
                            mockFactory.CreateCard("B6"),
                            mockFactory.CreateCard("B7"),
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

            Assert.Equal(expected, gameService.CalculateScore(expeditions));
        }

        [Fact]
        public void PlayTurnDescriptionLastTurnNotNull()
        {
            var game = mockFactory.GetNewGame();

            game = gameService.PlayTurn(game);

            Assert.True(!string.IsNullOrEmpty(game.DescriptionLastTurn) && game.DescriptionLastTurn != "Game initialization completed succesfully.");
        }

        public void Dispose()
        {
        }
    }
}
