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

        private string GetRawWikiPageText(string wikiPage)
        {
            string uri = UrlWikipediaRawBase + wikiPage.Replace(" ", "_");

            using WebClient client = new WebClient();
            return client.DownloadString(uri);
        }

        private string GetRedirectPage(string rawText)
        {
            int pos = rawText.IndexOf("[[");
            string redirectPage = rawText.Substring(pos + 2);
            pos = redirectPage.IndexOf("]]");

            return redirectPage.Substring(0, pos);
        }
    }
}
