using System;
using Xunit;

namespace LostCities.CardGame.Test.ServiceTests
{
    [Collection("Realm tests")]
    public class GameServiceTest: IDisposable
    {
        [Fact]
        public void Test1()
        {
            Assert.True(true);
        }

        public void Dispose()
        {
        }
    }
}
