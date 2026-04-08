using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackOOP
{
    public class Player
    {
        public string Name { get; set; }
        public List<Card> Hand { get; private set; } = new List<Card>();

        private static Random _random = new Random();

        public Player(string name)
        {
            Name = name;
        }
        public void ReceiveCard(Card card)
        {
            Hand.Add(card);
        }
        public int GetCurrentHandValue()
        {
            return Hand.Sum(card => card.Value);
        }

        public string GetMoveOpinion()
        {
            int total = GetCurrentHandValue();

            if (total < 12) return "Hit";
            if (total > 18) return "Stand";

            return _random.Next(0, 2) == 0 ? "Hit" : "Stand";
        }

        public string ShowHand()
        {
            return string.Join(", ", Hand);
        }
    }
}