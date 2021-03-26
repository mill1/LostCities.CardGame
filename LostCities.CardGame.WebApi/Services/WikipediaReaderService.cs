using LostCities.CardGame.WebApi.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LostCities.CardGame.WebApi.Services
{
    public class WikipediaReaderService : IWikipediaReaderService
    {
        private const string UrlWikipediaRawBase = "https://en.wikipedia.org/w/index.php?action=raw&title=";
        private const int NoInfobox = -1;

        private readonly ILogger logger;

        public WikipediaReaderService(ILogger<WikipediaReaderService> logger)
        {
            this.logger = logger;
        }


        private string GetRawArticleMarkup(ref string article, out bool isRedirect)
        {
            isRedirect = false;
            string rawText = GetRawWikiPageText(article);

            if (rawText.Contains("#REDIRECT"))
            {
                isRedirect = true;
                article = GetRedirectPage(rawText);
            }
            return rawText;
        }

        public string GetRawArticleText(ref string article)
        {
            bool isRedirect;

            string rawText = GetRawArticleMarkup(ref article, out isRedirect);

            if (isRedirect)
                rawText = GetRawWikiPageText(article);

            return rawText;
        }

        private int GetStartPositionInfobox(string rawText)
        {
            int pos = rawText.IndexOf("infobox", StringComparison.OrdinalIgnoreCase);

            if (pos == -1)
                return NoInfobox;

            if (rawText.Contains("Please do not add an infobox", StringComparison.OrdinalIgnoreCase))
                return NoInfobox;

            // Find the opening accolades of the listbox
            pos = rawText.LastIndexOf("{{", pos);

            return pos;
        }

        private string GetRawWikiPageText(string wikiPage)
        {
            string uri = UrlWikipediaRawBase + wikiPage.Replace(" ", "_");

            using WebClient client = new WebClient();
            return client.DownloadString(uri);
        }

        private string GetRedirectPage(string rawText)
        {
            // #REDIRECT[[Robert McG. Thomas Jr.]]
            int pos = rawText.IndexOf("[[");
            string redirectPage = rawText.Substring(pos + 2);
            pos = redirectPage.IndexOf("]]");

            return redirectPage.Substring(0, pos);
        }
    }
}
