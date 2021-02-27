using LostCities.CardGame.Console.UI;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LostCities.CardGame.Console
{
    public class Program
    {
        public static void Main()
        {
            IServiceCollection services = new ServiceCollection();
            new Startup().ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var runner = serviceProvider.GetService<Runner>();
            runner.Run();
        }
    }
}
