using LostCities.CardGame.WebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace LostCities.CardGame.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WikipediaReaderController : ControllerBase
    {

        private readonly IWikipediaReaderService wikipediaService;
        private readonly ILogger<WikipediaReaderController> logger;

        public WikipediaReaderController(IWikipediaReaderService wikipediaService, ILogger<WikipediaReaderController> logger)
        {
            this.wikipediaService = wikipediaService;
            this.logger = logger;
        }

        [HttpGet("rawarticle/{articleTitle}")]
        public IActionResult GetArticle(string articleTitle)
        {
            try
            {
                return Ok(wikipediaService.GetRawArticleText(ref articleTitle));
            }
            catch (Exception e)
            {
                string message = $"Getting the raw article text failed. Requested article: {articleTitle}.\r\n" +
                                 $"Exception:\r\n{e}";

                logger.LogError($"{message}", e);
                return BadRequest(message);
            }
        }
    }
}
