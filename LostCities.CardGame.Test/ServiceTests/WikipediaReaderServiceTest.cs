using LostCities.CardGame.WebApi.Interfaces;
using LostCities.CardGame.WebApi.Services;
using System;
using Xunit;

namespace LostCities.CardGame.Test.ServiceTests
{
    [Collection("Realm tests")]
    public class WikipediaReaderServiceTest : IDisposable
    {
        private readonly IWikipediaReaderService wikipediaReaderService;

        public WikipediaReaderServiceTest()
        {
            wikipediaReaderService = new WikipediaReaderService();
        }

        [Fact]
        public void GetRawArticleTextTest()
        {
            string article = "Lost Cities";

            string response = wikipediaReaderService.GetRawArticleText(ref article);

            Assert.Contains("'''''Lost Cities'''''", response);
        }

        [Fact]
        public void GetRawArticleTextRedirectTest()
        {
            string article = "Lost Cities game";

            wikipediaReaderService.GetRawArticleText(ref article);

            Assert.Equal("Lost Cities", article);
        }

        public void Dispose()
        {
        }
    }
}
