using LostCities.CardGame.WebApi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostCities.CardGame.WebApi.Interfaces
{
    public interface IMapper
    {
        public Models.Game MapToModel(Dtos.Game gameDto);
        public Dtos.Game MapToDto(Models.Game game);
        public List<IPile> MapToModel(IEnumerable<IEnumerable<Card>> cardsListDto);
        public IEnumerable<IEnumerable<Card>> MapToDto(IEnumerable<IPile> piles);
    }
}
