using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using LostCities.CardGame.WebApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostCities.CardGame.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService gameService;
        private readonly ILogger<GameController> logger;

        public GameController(IGameService gameService, ILogger<GameController> logger)
        {
            this.gameService = gameService;
            this.logger = logger;
        }

        [HttpGet("new")]
        public IActionResult NewGame()
        {
            try
            {
                var newGame = gameService.GetNewGame();

                return Ok(MapToDto(newGame));
            }
            catch (Exception e)
            {
                string message = $"Getting the new game failed.\r\nException:\r\n{e}";
                logger.LogError($"{message}", e);
                return BadRequest(message);
            }
        }

        private Dtos.Game MapToDto(Models.Game game)
        {
            Dtos.Game gameCardsDto = new Dtos.Game
            {
                PlayerCards = MapToDto(game.PlayerCards.Cards),
                BotCards = MapToDto(game.BotCards.Cards),
                DrawPile = MapToDto(game.DrawPile.Cards),
                PlayerExpeditions = MapToDto(game.PlayerExpeditions),
                BotExpeditions = MapToDto(game.BotExpeditions),               
                DiscardPiles = MapToDto(game.DiscardPiles)
            };
            return gameCardsDto;
        }

        private IEnumerable<IEnumerable<Dtos.Card>> MapToDto(IEnumerable<Models.CardCollection> cardCollections)
        {
            List<IEnumerable<Dtos.Card>> cardsListDto = new List<IEnumerable<Dtos.Card>>();

            foreach (Models.CardCollection cardCollection in cardCollections)
                cardsListDto.Add(MapToDto(cardCollection.Cards));

            return cardsListDto;
        }

        private IEnumerable<Dtos.Card> MapToDto(IEnumerable<Models.Card> cards)
        {
            List<Dtos.Card> cardsDto = new List<Dtos.Card>();

            foreach (var card in cards)
                cardsDto.Add(MapToDto(card));

            return cardsDto;
        }            

        private Dtos.Card MapToDto(Models.Card card)
        {
            return new Dtos.Card()
            {
                Id = card.Id,
                ExpeditionType = card.ExpeditionType.Name.ToString(),
                Value = card.Value
            };
        }
    }
}