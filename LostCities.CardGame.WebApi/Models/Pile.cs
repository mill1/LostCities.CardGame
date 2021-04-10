using LostCities.CardGame.WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LostCities.CardGame.WebApi.Models
{
    public class Pile : IPile
    {
        public List<Card> Cards { get; set; }

        public Pile() 
        {
            Cards = new List<Card>();
        }

        public Pile(IEnumerable<Card> initialCards) : this()
        {
            Cards.AddRange(initialCards);
        }

        public void MoveCardToPile(Card card, List<IPile> piles)
        {
            if (Cards.Count() == 0)
                throw new Exception("The pile is empty.");

            Cards.Remove(card);
            GetPileByExpedition(card, piles).Cards.Add(card);
        }

        public Card DrawDiscardCard(Game game, IPile handCards)
        {
            Card card = MoveLastCardToHand(handCards);

            if (!this.Cards.Any())
                game.DiscardPiles.Remove(this);

            return card;
        }

        public Card DrawCard(IPile handCards)
        {
            return MoveLastCardToHand(handCards);
        }

        private Card MoveLastCardToHand(IPile handCards)
        {
            if (Cards.Count() == 0)
                throw new Exception("The pile is empty.");

            Card card = Cards.Last();
            Cards.Remove(card);
            handCards.Cards.Add(card);

            return card;
        }

        private IPile GetPileByExpedition(Card card, List<IPile> piles)
        {
            foreach (Pile p in piles)
                if (p.Cards.Where(c => c.ExpeditionType == card.ExpeditionType).Any())
                    return p;

            IPile pile = new Pile();
            piles.Add(pile);
            return pile;
        }
    }
}