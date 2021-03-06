﻿using LostCities.CardGame.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostCities.CardGame.WebApi.Interfaces
{
    public interface IPile
    {
        public List<Card> Cards { get; set; }
        public void MoveCardToPile(Card card, List<IPile> piles);
        public Card DrawDiscardCard(Game game, IPile handCards);
        public Card DrawCard(IPile handCards);
    }
}
