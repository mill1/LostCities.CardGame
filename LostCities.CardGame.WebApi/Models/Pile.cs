using System;
using System.Collections.Generic;
using System.Linq;

namespace LostCities.CardGame.WebApi.Models
{
    public class Pile
    {
        public List<Card> Cards { get; set;  }

        public Pile() 
        {
            Cards = new List<Card>();
        }

        public Pile(IEnumerable<Card> initialCards) : this()
        {
            Cards.AddRange(initialCards);
        }

        public void MoveCardToPile(Card card, List<Pile> piles)
        {
            if (Cards.Count() == 0)
                throw new Exception("The pile is empty.");

            Cards.Remove(card);
            GetPileByExpedition(card, piles).Cards.Add(card);
        }

        private Pile GetPileByExpedition(Card card, List<Pile> piles)
        {
            foreach (Pile cc in piles)
                if (cc.Cards.Where(c => c.ExpeditionType.Code == card.ExpeditionType.Code).Any())
                    return cc;

            Pile pile = new Pile();
            piles.Add(pile);
            return pile;
        }

        public void MoveLastCardTo(List<Card> targetCards)
        {
            if (Cards.Count() == 0)
                throw new Exception("The pile is empty.");

            Card card = Cards.Last();
            Cards.Remove(card);
            targetCards.Add(card);
        }
    }
}