using LostCities.CardGame.WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostCities.CardGame.WebApi
{
    public class Mapper : IMapper
    {
        public Models.Game MapToModel(Dtos.Game gameDto)
        {
            Models.Game game = new Models.Game(
                new Models.Pile(MapToModel(gameDto.PlayerCards)),
                new Models.Pile(MapToModel(gameDto.BotCards)),
                new Models.Pile(MapToModel(gameDto.DrawPile)),
                MapToModel(gameDto.PlayerExpeditions),
                MapToModel(gameDto.BotExpeditions),
                MapToModel(gameDto.DiscardPiles));

            game.DescriptionLastTurn = gameDto.DescriptionLastTurn;

            return game;
        }

        public Dtos.Game MapToDto(Models.Game game)
        {
            Dtos.Game gameCardsDto = new Dtos.Game
            {
                PlayerCards = MapToDto(game.PlayerCards.Cards),
                BotCards = MapToDto(game.BotCards.Cards),
                DrawPile = MapToDto(game.DrawPile.Cards),
                PlayerExpeditions = MapToDto(game.PlayerExpeditions),
                BotExpeditions = MapToDto(game.BotExpeditions),
                DiscardPiles = MapToDto(game.DiscardPiles),
                DescriptionLastTurn = game.DescriptionLastTurn
            };
            return gameCardsDto;
        }

        public List<IPile> MapToModel(IEnumerable<IEnumerable<Dtos.Card>> cardsListDto)
        {
            List<IPile> piles = new List<IPile>();

            foreach (IEnumerable<Dtos.Card> cardsDto in cardsListDto)
                piles.Add(new Models.Pile(MapToModel(cardsDto)));

            return piles;
        }

        public IEnumerable<IEnumerable<Dtos.Card>> MapToDto(IEnumerable<IPile> piles)
        {
            List<IEnumerable<Dtos.Card>> cardsListDto = new List<IEnumerable<Dtos.Card>>();

            foreach (Models.Pile pile in piles)
                cardsListDto.Add(MapToDto(pile.Cards));

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
                ExpeditionType = card.ExpeditionType.ToString(),
                Value = card.Value
            };
        }

        private IEnumerable<Models.Card> MapToModel(IEnumerable<Dtos.Card> cardsDto)
        {
            List<Models.Card> cards = new List<Models.Card>();

            foreach (var cardDto in cardsDto)
                cards.Add(MapToModel(cardDto));

            return cards;
        }

        private Models.Card MapToModel(Dtos.Card cardDto)
        {
            return new Models.Card(cardDto.Id, new Models.ExpeditionType(cardDto.ExpeditionType), cardDto.Value);
        }
    }
}
