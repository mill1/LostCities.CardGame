using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostCities.CardGame.WebApi.Models
{
    public class DrawPile : Pile
    {        
        public DrawPile(IEnumerable<Card> initialCards) : base(initialCards)
        {
        }
    }
}
