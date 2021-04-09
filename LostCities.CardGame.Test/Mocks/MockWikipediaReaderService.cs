using LostCities.CardGame.WebApi.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace LostCities.CardWikipediaReader.Test.Mocks
{
    public class MockWikipediaReaderService
    {
        public IWikipediaReaderService GetMockWikipediaReaderService()
        {
            Mock<IWikipediaReaderService> mockWikipediaReaderService = new Mock<IWikipediaReaderService>();

            string articleOk = "Article Ok";
            mockWikipediaReaderService.Setup(s => s.GetRawArticleText(ref articleOk)).Returns("Raw article text");

            string articleBadRequest = "Article BadRequest";
            mockWikipediaReaderService.Setup(s => s.GetRawArticleText(ref articleBadRequest)).Throws(new Exception("Mock exception: GetRawArticleText"));

            return mockWikipediaReaderService.Object;
        }
    }
}
