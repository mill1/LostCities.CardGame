using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using LostCities.CardGame.WebApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace LostCities.CardGame.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IGameService gameService;
        private readonly ILogger<GameController> logger;

        public GameController(IMapper mapper, IGameService gameService, ILogger<GameController> logger)
        {
            this.mapper = mapper;
            this.gameService = gameService;
            this.logger = logger;
        }

        [HttpGet("new")]
        public IActionResult NewGame()
        {
            try
            {
                var newGame = gameService.GetNewGame();

                return Ok(mapper.MapToDto(newGame));
            }
            catch (Exception e)
            {
                string message = $"Getting the new game failed.\r\nException:\r\n{e}";
                logger.LogError($"{message}", e);
                return BadRequest(message);
            }
        }

        [HttpPost("playturn")]
        public IActionResult PlayTurn(Dtos.Game gameDto)
        {
            try
            {
                if (gameDto == null)
                    return BadRequest("game object to cannot be null.");

                var game = mapper.MapToModel(gameDto);

                // TODO dummy move
                Thread.Sleep(2500);                

                Random r = new Random();
                int rInt = r.Next(0, 7);
                Models.Card card = game.BotCards.Cards.ElementAt(rInt);

                if (card.Value % 2 == 0)
                {
                    card = GetLowestValueOfExpeditionType(game, card.ExpeditionType);
                    game.BotCards.MoveCardToPile(card, game.BotExpeditions);
                    game.DescriptionLastTurn = $"Bot moved card {card.Id} to expedition {card.ExpeditionType.Name}";
                }
                else
                {
                    game.BotCards.MoveCardToPile(card, game.DiscardPiles);
                    game.DescriptionLastTurn = $"Bot moved card {card.Id} to discard pile {card.ExpeditionType.Name}";
                }                    

                // Never take from a discard pile :)
                game.DrawPile.DrawCard(game.BotCards);
                game.DescriptionLastTurn += " and drew a card from the draw pile.";

                return Ok(mapper.MapToDto(game));
            }
            catch (Exception e)
            {
                string message = $"Playing the turn failed.\r\nException:\r\n{e}";
                logger.LogError($"{message}", e);
                return BadRequest(message);
            }
        }

        private Models.Card GetLowestValueOfExpeditionType(Models.Game game, Models.ExpeditionType expeditionType)
        {
            return game.BotCards.Cards.Where(c => c.ExpeditionType.Code == expeditionType.Code).OrderBy(x => x.Value).First();
        }
    }
}