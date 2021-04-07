using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostCities.CardGame.WebApi.Models
{
    public class Game
    {
        public Pile PlayerCards { get; }
        public Pile BotCards { get; }
        public Pile DrawPile { get; }
        public List<Pile> PlayerExpeditions { get; }
        public List<Pile> BotExpeditions { get; }        
        public List<Pile> DiscardPiles { get; }
        
        public Game(List<Card> deck)
        {
            List<Card> hand = deck.GetRange(0, 8);
            PlayerCards = new Pile(hand);
            deck.RemoveRange(0, 8);

            hand = deck.GetRange(0, 8);
            BotCards = new Pile(hand);
            deck.RemoveRange(0, 8);

            DrawPile = new Pile(deck);
            PlayerExpeditions = new List<Pile>();
            BotExpeditions = new List<Pile>();
            DiscardPiles = new List<Pile>();
        }

        public Game(Pile playerCards, Pile botCards, Pile drawPile,
                    List<Pile> playerExpeditions, List<Pile> botExpeditions, List<Pile> discardPiles)            
        {
            PlayerCards = playerCards;
            BotCards = botCards;
            DrawPile = drawPile;
            PlayerExpeditions = playerExpeditions;
            BotExpeditions = botExpeditions;
            DiscardPiles = discardPiles;
        }
    }
}
