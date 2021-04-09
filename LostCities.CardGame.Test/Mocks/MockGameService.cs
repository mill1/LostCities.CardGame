using LostCities.CardGame.WebApi.Interfaces;
using LostCities.CardGame.WebApi.Models;
using System;
using System.Collections.Generic;
using Moq;

namespace LostCities.CardGame.Test.Mocks
{
    public class MockGameService
    {
        public Game NewGame;
        public Game EmptyGame;
        public Game NewGameThrowsException; 
        private Dictionary<string, ExpeditionType> expeditions;

        public MockGameService()
        {
            expeditions = new Dictionary<string, ExpeditionType>()
            {
                { "B", new ExpeditionType("Blue") },
                { "Y", new ExpeditionType("Yellow") },
                { "R", new ExpeditionType("Red") },
                { "W", new ExpeditionType("White") },
                { "G", new ExpeditionType("Green") }
            };

            NewGame = new Game(
                GetPlayerCards(),
                GetBotCards(),
                GetDrawPile(),
                new List<IPile>(),
                new List<IPile>(),
                new List<IPile>());

            NewGame.DescriptionLastTurn = "Game initialization completed succesfully.";

            EmptyGame = new Game(
                new Pile(),
                new Pile(),
                new Pile(),
                new List<IPile>(),
                new List<IPile>(),
                new List<IPile>());

            List<Card> deck = new List<Card>();
            deck.AddRange(GetPlayerCards().Cards);
            deck.AddRange(GetBotCards().Cards);
            deck.AddRange(GetDrawPile().Cards);

            NewGameThrowsException = new Game(deck);
        }

        public IGameService GetMockGameService()
        {
            Mock<IGameService> mockGameService = new Mock<IGameService>();

            mockGameService.Setup(s => s.GetNewGame()).Returns(NewGame);

            return mockGameService.Object;
        }

        private IPile GetPlayerCards()
        {
            return new Pile(
                new List<Card>()
                {
                    CreateCard("R3"), CreateCard("RA"), CreateCard("G4"), CreateCard("B8"),
                    CreateCard("BA"), CreateCard("R2"), CreateCard("W9"), CreateCard("B4")
                });
        }

        private IPile GetBotCards()
        {
            return new Pile(
                new List<Card>()
                {
                    CreateCard("R4"), CreateCard("BB"), CreateCard("WA"), CreateCard("Y5"),
                    CreateCard("R9"), CreateCard("G2"), CreateCard("Y10"), CreateCard("YA")
                });
        }

        private IPile GetDrawPile()
        {
            return new Pile(
                new List<Card>()
                {
                    CreateCard("YB"), CreateCard("Y3"), CreateCard("RB"), CreateCard("B9"),
                    CreateCard("GA"), CreateCard("Y7"), CreateCard("R10"), CreateCard("Y8"),
                    CreateCard("B3"), CreateCard("G7"), CreateCard("G6"), CreateCard("Y2"),
                    CreateCard("W5"), CreateCard("GB"), CreateCard("G10"), CreateCard("W4"),
                    CreateCard("B7"), CreateCard("YC"), CreateCard("W10"), CreateCard("Y4"),
                    CreateCard("Y6"), CreateCard("W6"), CreateCard("Y9"), CreateCard("R8"),
                    CreateCard("WB"), CreateCard("W8"), CreateCard("B6"), CreateCard("W2"),
                    CreateCard("B10"), CreateCard("R5"), CreateCard("B2"), CreateCard("B5"),
                    CreateCard("BC"), CreateCard("GC"), CreateCard("R7"), CreateCard("RC"),
                    CreateCard("G8"), CreateCard("R6"), CreateCard("W3"), CreateCard("G9"),
                    CreateCard("WC"), CreateCard("G3"), CreateCard("W7"), CreateCard("G5"),
                });
        }

        private Card CreateCard(string CardId)
        {
            int value;
            switch (CardId.Substring(1))
            {
                case "A": case "B": case "C":
                    value = 0;
                    break;
                default:
                    value = int.Parse(CardId.Substring(1));
                    break;
            }
            return new Card(CardId, expeditions[CardId.Substring(0,1)], value);
        }
    }
}
