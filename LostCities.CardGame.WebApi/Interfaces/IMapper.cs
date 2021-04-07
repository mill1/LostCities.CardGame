using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostCities.CardGame.WebApi.Interfaces
{
    public interface IMapper
    {
        public Dtos.Game MapToDto(Models.Game game);
        public Models.Game MapToModel(Dtos.Game gameDto);
    }
}
