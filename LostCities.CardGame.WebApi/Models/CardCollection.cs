using System;
using System.Collections.Generic;
using System.Linq;

namespace LostCities.CardGame.WebApi.Models
{
    public class CardCollection
    {
        public List<Card> Cards { get; set;  }

        public CardCollection() 
        {
            Cards = new List<Card>();
        }

        public CardCollection(IEnumerable<Card> initialCards) : this()
        {
            //Cards = new List<Card>();
            Cards.AddRange(initialCards);
        }

        public void MoveCardToCardCollection(Card card, List<CardCollection> cardCollections)
        {
            if (Cards.Count() == 0)
                throw new Exception("Collection does not contain any cards.");

            Cards.Remove(card);
            GetCardCollectionByExpedition(card, cardCollections).Cards.Add(card);
        }

        private CardCollection GetCardCollectionByExpedition(Card card, List<CardCollection> cardCollections)
        {
            foreach (CardCollection cc in cardCollections)
                if (cc.Cards.Where(c => c.ExpeditionType.Code == card.ExpeditionType.Code).Any())
                    return cc;

            // Card collection of this expedition type not started.
            CardCollection cardCollection = new CardCollection();
            cardCollections.Add(cardCollection);
            return cardCollection;
        }

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