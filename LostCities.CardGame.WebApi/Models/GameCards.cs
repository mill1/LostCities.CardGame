using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostCities.CardGame.WebApi.Models
{
    public class GameCards
    {
        public CardCollection PlayerCards { get; }
        public CardCollection BotCards { get; }
        public CardCollection DrawPile { get; }
        public List<CardCollection> PlayerExpeditions { get; }
        public List<CardCollection> BotExpeditions { get; }        
        public List<CardCollection> DiscardPiles { get; }
        
        public GameCards(List<Card> deck)
        {
            List<Card> hand = deck.GetRange(0, 8);
            PlayerCards = new CardCollection(hand);
            deck.RemoveRange(0, 8);

            hand = deck.GetRange(0, 8);
            BotCards = new CardCollection(hand);
            deck.RemoveRange(0, 8);

            DrawPile = new CardCollection(deck);
            PlayerExpeditions = new List<CardCollection>();
            BotExpeditions = new List<CardCollection>();
            DiscardPiles = new List<CardCollection>();
        }
    }
}
