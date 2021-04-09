using Microsoft.Extensions.Logging;
using Moq;

namespace LostCities.CardGame.Test.Mocks
{

    public static class MockLogger<T>
    {
        public static ILogger<T> CreateMockLogger()
        {
            var mock = new Mock<ILogger<T>>();
            ILogger<T> logger = mock.Object;

            return logger;
        }
    }
}
