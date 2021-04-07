using LostCities.CardGame.Console.Services;
using LostCities.CardGame.Console.UI;
using LostCities.CardGame.WebApi;
using LostCities.CardGame.WebApi.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace LostCities.CardGame.Console
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddSingleton<HttpClient>();
            services.AddSingleton<Http>();
            services.AddSingleton<Duel>();
            services.AddSingleton<Runner>();
            services.AddSingleton<IMapper, Mapper>();
        }
    }
}
