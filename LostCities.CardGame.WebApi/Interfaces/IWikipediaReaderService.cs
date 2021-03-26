namespace LostCities.CardGame.WebApi.Interfaces
{
    public interface IWikipediaReaderService
    {
        public string GetRawArticleText(ref string article);
    }
}
