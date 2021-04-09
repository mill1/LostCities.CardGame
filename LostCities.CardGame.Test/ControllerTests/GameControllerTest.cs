using System;
using Xunit;

namespace LostCities.CardGame.Test.ControllerTests
{
    [Collection("Realm tests")]
    public class GameControllerTest: IDisposable
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
