using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using LostCities.CardGame.WebApi.Interfaces;
using System.Collections.Generic;
using System.Linq;

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

                var game = gameService.PlayTurn(mapper.MapToModel(gameDto));

                return Ok(mapper.MapToDto(game));
            }
            catch (Exception e)
            {
                string message = $"Playing the turn failed.\r\nException:\r\n{e}";
                logger.LogError($"{message}", e);
                return BadRequest(message);
            }
        }        

        [HttpPost ("calculatescore")]
        public IActionResult CalculateScore(IEnumerable<IEnumerable<Dtos.Card>> expeditionsDto)
        {
            try
            {
                if (expeditionsDto == null)
                    return BadRequest("game object to cannot be null.");

                var expeditions = mapper.MapToModel(expeditionsDto);
                return Ok(gameService.CalculateScore(expeditions));
            }
            catch (Exception e)
            {
                string message = $"Calculating the score failed.\r\nException:\r\n{e}";
                logger.LogError($"{message}", e);
                return BadRequest(message);
            }
        }     
    }
}