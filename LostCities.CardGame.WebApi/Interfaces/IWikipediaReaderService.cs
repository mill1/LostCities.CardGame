namespace LostCities.CardGame.WebApi.Interfaces
{
    public interface IWikipediaReaderService
    {
        public string GetArticleTitle(string nameVersion, int year, int monthId);
        public string GetAuthorsArticle(string author, string source);
        public string GetRawArticleText(ref string article, bool nettoContent);
    }
}
