using LostCities.CardGame.Test.Mocks;
using LostCities.CardGame.WebApi;
using LostCities.CardGame.WebApi.Controllers;
using LostCities.CardGame.WebApi.Interfaces;
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

        public GameControllerTest()
        {
            mockGameService = new MockGameService().GetMockGameService();
            ILogger<GameController> mockLogger = MockLogger<GameController>.CreateMockLogger();
            gameController = new GameController(new Mapper(), mockGameService, mockLogger);
        }

        [Fact]
        public void NewGameOkTest()
        {
            IActionResult result = gameController.NewGame();

            Assert.IsType<OkObjectResult>(result);
        }

        public void Dispose()
        {
        }
    }
}
