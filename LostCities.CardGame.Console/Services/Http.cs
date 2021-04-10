using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace LostCities.CardGame.Console.Services
{
    public class Http
    {
        private readonly HttpClient client;

        public Http(IConfiguration configuration, HttpClient client)
        {
            this.client = client;
            var uri = configuration.GetValue<string>("LCWebApi:SchemeAndHost");
            this.client.BaseAddress = new Uri(uri);
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public WebApi.Dtos.Game GetNewGame()
        {
            HttpResponseMessage response = client.GetAsync("game/new").Result;

            string result = HandleResponse(response);

            WebApi.Dtos.Game game = JsonConvert.DeserializeObject<WebApi.Dtos.Game>(result);

            return game;
        }

        public WebApi.Dtos.Game PostBotTurn(WebApi.Dtos.Game gameDto)
        {
            var ser = JsonConvert.SerializeObject(gameDto);

            HttpResponseMessage response = client.PostAsync("game/playturn", new StringContent(ser, Encoding.UTF8, "application/json")).Result;

            string result = HandleResponse(response);

            WebApi.Dtos.Game reponse = JsonConvert.DeserializeObject<WebApi.Dtos.Game>(result);

            return reponse;
        }

        public string GetWikipediaRawArticleText(string articleTitle)
        {
            string uri = $"wikipediareader/rawarticle/{articleTitle}";
            HttpResponseMessage response = client.GetAsync(uri).Result;

            return HandleResponse(response);
        }

        public string GetWebApiAssemblyProperty(string property)
        {
            HttpResponseMessage response = client.GetAsync($"assembly/property/{property}").Result;

            return HandleResponse(response).Replace("\"", "");
        }

        public string GetWebApiAssemblyProperties()
        {
            HttpResponseMessage response = client.GetAsync($"assembly/properties").Result;

            return HandleResponse(response).Replace("\"", ""); 
        }

        private string HandleResponse(HttpResponseMessage response)
        {
            string result = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
                return result;
            else
                throw new Exception($"response did not indicate succes. Result ='{result}'");
        }
    }
}
