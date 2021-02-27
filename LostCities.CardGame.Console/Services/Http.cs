using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

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

        public string GetWikipediaRawArticleText(string articleTitle, bool netto)
        {

            string uri = $"wikipediareader/rawarticle/{articleTitle}/netto/{netto}";
            HttpResponseMessage response = client.GetAsync(uri).Result;

            return HandleResponse(response);
        }

        public string GetWebApiAssemblyProperty(string property)
        {
            HttpResponseMessage response = client.GetAsync($"assembly/property/{property}").Result;

            return HandleResponse(response).Replace("\"", "");
        }

        private string HandleResponse(HttpResponseMessage response)
        {
            string result = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
                return result;
            else
                throw new Exception($"response did not indicate succes. Result (between quotes)='{result}'");
        }
    }
}
