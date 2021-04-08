using LostCities.CardGame.WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostCities.CardGame.WebApi.Models
{
    public class Game
    {
        public IPile PlayerCards { get; }
        public IPile BotCards { get; }
        public IPile DrawPile { get; }
        public List<IPile> PlayerExpeditions { get; }
        public List<IPile> BotExpeditions { get; }        
        public List<IPile> DiscardPiles { get; }

        public string DescriptionLastTurn { get; set; } = String.Empty;

        public Game(List<Card> deck)
        {
            List<Card> hand = deck.GetRange(0, 8);
            PlayerCards = new Pile(hand);
            deck.RemoveRange(0, 8);

            hand = deck.GetRange(0, 8);
            BotCards = new Pile(hand);
            deck.RemoveRange(0, 8);

            DrawPile = new Pile(deck);
            PlayerExpeditions = new List<IPile>();
            BotExpeditions = new List<IPile>();
            DiscardPiles = new List<IPile>();
        }

        public Game(IPile playerCards, IPile botCards, IPile drawPile,
                    List<IPile> playerExpeditions, List<IPile> botExpeditions, List<IPile> discardPiles)            
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
