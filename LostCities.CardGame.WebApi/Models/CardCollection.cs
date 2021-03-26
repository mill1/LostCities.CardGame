using System;
using System.Collections.Generic;
using System.Linq;

namespace LostCities.CardGame.WebApi.Models
{
    public class CardCollection
    {
        public List<Card> Cards { get; }

        public CardCollection(IEnumerable<Card> initialCards)
        {
            Cards = new List<Card>();
            Cards.AddRange(initialCards);
        }

        public void MoveLastCardTo(IEnumerable<Card> targetCards)
        {
            if (Cards.Count() == 0)
                throw new Exception("Collection does not contain any cards.");

            Card card = Cards.Last();
            Cards.ToList().Remove(card);
            targetCards.ToList().Add(card);
        }

        public void GetLastCardFrom(IEnumerable<Card> sourceCards) 
        {
            if (sourceCards.Count() == 0)
                throw new Exception("Source does not contain any cards.");

            Card card = sourceCards.Last();
            sourceCards.ToList().Remove(card);
            Cards.Add(card);
        }
    }
}