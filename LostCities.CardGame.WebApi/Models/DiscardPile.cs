using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostCities.CardGame.WebApi.Models
{
    public class DiscardPile : Pile
    {
        public ExpeditionType Expedition { get; }

        public DiscardPile(IEnumerable<Card> initialCards) : base(initialCards) 
        { 
        }

        public DiscardPile(ExpeditionType expedition, IEnumerable<Card> initialCards) : base(initialCards)
        {
            Expedition = expedition;
        }
    }
}
