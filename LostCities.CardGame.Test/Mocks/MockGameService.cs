using LostCities.CardGame.WebApi.Interfaces;
using LostCities.CardGame.WebApi.Models;
using System;
using System.Collections.Generic;
using Moq;

namespace LostCities.CardGame.Test.Mocks
{
    public class MockGameService
    {
        private readonly MockFactory mockFactory;

        public MockGameService()
        {
            mockFactory = new MockFactory();
        }

        public IGameService GetMockGameService()
        {
            Mock<IGameService> mockGameService = new Mock<IGameService>();

            mockGameService.Setup(s => s.GetNewGame()).Returns(mockFactory.GetNewGame());

            return mockGameService.Object;
        }
    }
}
