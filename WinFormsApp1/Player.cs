using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackOOP
{
    public class Player
    {
        public string Name { get; set; }
        public List<Card> Hand { get; private set; } = new List<Card>();

        public string LaatsteMening { get; private set; }

        public string Status { get; set; } = "Bezig";

        public string getOpinion()
        {
            LaatsteMening = GetMoveOpinion();
            return LaatsteMening;
        }

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
            int totaal = Hand.Sum(card => card.Value);

            int aantalAzen = Hand.Count(card => card.Value == 11);

            while (totaal > 21 && aantalAzen > 0)
            {
                totaal -= 10;
                aantalAzen--;
            }

            return totaal;
        }

        public virtual string GetMoveOpinion()
        {
            int total = GetCurrentHandValue();

            if (total < 14) return "Hit";
            if (total > 18) return "Stand";

            return _random.Next(0, 100) < 50 ? "Hit" : "Stand";
        }

        public string ShowHand()
        {
            return string.Join(", ", Hand);
        }

        public bool IsBust()
        {
            return GetCurrentHandValue() > 21;
        }
        public virtual bool HeeftGewonnenVan(Player dealer)
        {
            if (this.IsBust()) return false;
            if (dealer.IsBust()) return true;
            return this.GetCurrentHandValue() > dealer.GetCurrentHandValue();
        }
    }
}