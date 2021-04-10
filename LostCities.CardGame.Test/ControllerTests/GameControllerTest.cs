using LostCities.CardGame.Test.Mocks;
using LostCities.CardGame.WebApi;
using LostCities.CardGame.WebApi.Controllers;
using LostCities.CardGame.WebApi.Interfaces;
using LostCities.CardGame.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace LostCities.CardGame.Test.ControllerTests
{
    [Collection("Realm tests")]
    public class GameControllerTest: IDisposable
    {
        private readonly GameController gameController;
        private readonly IGameService mockGameService;
        private readonly IMapper mapper;

        public GameControllerTest()
        {
            mapper = new Mapper();
            mockGameService = new MockGameService().GetMockGameService();
            ILogger<GameController> mockLogger = MockLogger<GameController>.CreateMockLogger();
            gameController = new GameController(mapper, mockGameService, mockLogger);
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

        public void Dispose()
        {
        }
    }
}
