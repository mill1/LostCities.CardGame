using LostCities.CardGame.Test.Mocks;
using LostCities.CardGame.WebApi;
using LostCities.CardGame.WebApi.Controllers;
using LostCities.CardGame.WebApi.Interfaces;
using LostCities.CardGame.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Xunit;
using LostCities.CardGame.WebApi.Services;
using System.Collections.Generic;

namespace LostCities.CardGame.Test.ControllerTests
{
    [Collection("Realm tests")]
    public class GameControllerTest: IDisposable
    {
        private readonly GameController gameController;
        private readonly IGameService gameService;
        private readonly IMapper mapper;
        private readonly MockFactory mockFactory;

        public GameControllerTest()
        {
            mapper = new Mapper();
            mockFactory = new MockFactory();
            // mockGameService = MockGameService().GetMockGameService(); Not needed yet
            gameService = new GameService(MockLogger<GameService>.CreateMockLogger()); 
            gameController = new GameController(mapper, gameService, MockLogger<GameController>.CreateMockLogger());
        }

        [Fact]
        public void NewGameOkTest()
        {
            IActionResult result = gameController.NewGame();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void PlayTurnOkTest()
        {
            Game gameDto = mapper.MapToDto(new MockFactory().GetNewGame());

            IActionResult result = gameController.PlayTurn(gameDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void PlayTurnBadRequestNullParameterTest()
        {
            IActionResult result = gameController.PlayTurn(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void PlayTurnBadRequestTest()
        {
            Game gameDto = mapper.MapToDto(new MockFactory().GetEmptyGame());

            IActionResult result = gameController.PlayTurn(gameDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CalculateScoreOkTest()
        {
            IEnumerable<IPile> expeditions = new List<IPile>()
            {
                 new WebApi.Models.Pile(
                        new List<WebApi.Models.Card>()
                        {
                            mockFactory.CreateCard("BA"),
                            mockFactory.CreateCard("B4"),
                            mockFactory.CreateCard("B8"),
                            mockFactory.CreateCard("B10"),
                        })
            };

            IActionResult result = gameController.CalculateScore(mapper.MapToDto(expeditions));            

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void CalculateScoreNullParameterTest()
        {
            IActionResult result = gameController.CalculateScore(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        public void Dispose()
        {
        }
    }
}
