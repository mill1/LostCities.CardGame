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
                var deckDto = MapToDto(gameService.InitializeDeck());

                return Ok(deckDto);
            }
            catch (Exception e)
            {
                string message = $"Getting the properties failed.\r\nException:\r\n{e}";
                logger.LogError($"{message}", e);
                return BadRequest(message);
            }
        }

        private IEnumerable<Dtos.Card> MapToDto(IEnumerable<Models.Card> deck)
        {
            List<Dtos.Card> deckDto = new List<Dtos.Card>();

            foreach (var card in deck)
                deckDto.Add(MapToDto(card));

            return deckDto;
        }            

        private Dtos.Card MapToDto(Models.Card card)
        {
            return new Dtos.Card()
            {
                Id = card.Id,
                Color = card.Color,
                Value = card.Value
            };
        }
    }
}