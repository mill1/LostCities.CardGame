using LostCities.CardGame.Test.Mocks;
using LostCities.CardGame.WebApi;
using LostCities.CardGame.WebApi.Controllers;
using LostCities.CardGame.WebApi.Interfaces;
using LostCities.CardWikipediaReader.Test.Mocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace LostCities.CardWikipediaReader.Test.ControllerTests
{

    [Collection("Realm tests")]
    public class WikipediaReaderControllerTest : IDisposable
    {
        private readonly WikipediaReaderController wikipediaReaderController;
        private readonly IWikipediaReaderService mockWikipediaReaderService;

        public WikipediaReaderControllerTest()
        {
            mockWikipediaReaderService = new MockWikipediaReaderService().GetMockWikipediaReaderService();
            ILogger<WikipediaReaderController> mockLogger = MockLogger<WikipediaReaderController>.CreateMockLogger();
            wikipediaReaderController = new WikipediaReaderController(mockWikipediaReaderService, mockLogger);
        }

        [Fact]
        public void NewWikipediaReaderOkTest()
        {
            IActionResult result = wikipediaReaderController.GetArticle("Article Ok");

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void NewWikipediaReaderBadRequestTest()
        {
            IActionResult result = wikipediaReaderController.GetArticle("Article BadRequest");

            Assert.IsType<BadRequestObjectResult>(result);
        }

        public void Dispose()
        {
        }
    }
}
