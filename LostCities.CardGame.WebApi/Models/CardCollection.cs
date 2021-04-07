using System;
using System.Collections.Generic;
using System.Linq;

namespace LostCities.CardGame.WebApi.Models
{
    public class CardCollection
    {
        public List<Card> Cards { get; set;  }

        public CardCollection(IEnumerable<Card> initialCards)
        {
            Cards = new List<Card>();
            Cards.AddRange(initialCards);
        }

        public void MoveCardToExpedition(Card card, List<CardCollection> expeditions)
        {
            if (Cards.Count() == 0)
                throw new Exception("Collection does not contain any cards.");

            Cards.Remove(card);
            // TODO
            //GetExpeditionCards(card, expeditions).Add(card);
        }

        //private List<CardCollection> GetExpeditionCards(Card card, List<CardCollection> expeditions)
        //{
        //    foreach (CardCollection expedition in expeditions)
        //    {
        //        if (expedition.Cards.Where(c => c.Id.StartsWith( )
        //            return expedition.Cards;
        //    }

        //    return null;
        //}

        public void MoveLastCardTo(List<Card> targetCards)
        {
            if (Cards.Count() == 0)
                throw new Exception("Collection does not contain any cards.");

            Card card = Cards.Last();
            Cards.Remove(card);
            targetCards.Add(card);
        }

        public void GetLastCardFrom(List<Card> sourceCards) 
        {
            if (sourceCards.Count() == 0)
                throw new Exception("Source does not contain any cards.");

            Card card = sourceCards.Last();
            sourceCards.Remove(card);
            Cards.Add(card);
        }
    }
}