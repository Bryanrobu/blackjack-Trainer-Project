using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackOOP
{
    public enum Rank
    {
        ACE = 1,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        TEN,
        JACK,
        QUEEN,
        KING

    }

    public enum Suit
    {
        HEARTS,
        DIAMONDS,
        CLUBS,
        SPADES
    }

    public class Card
    {
        private Rank rank;
        private int value;
        private Suit suit;
        private bool isDownCard;

        public int Value
        {
            get
            {
                value = (int)rank;
                switch (rank)
                {
                    case Rank.ACE:
                        value = 11;
                        break;
                    case Rank.JACK:
                    case Rank.QUEEN:
                    case Rank.KING:
                        value = 10;
                        break;
                }
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        public Card(Rank rank, Suit suit)
        {
            this.rank = rank;
            this.suit = suit;
        }

        public void Flip()
        {
            if (isDownCard)
            {
                isDownCard = false;
            }
            else if (!isDownCard)
            {
                isDownCard = true;
            }
        }

        public override string ToString()
        {
            string cardText = rank.ToString() + " OF " + suit.ToString();

            if (isDownCard)
            {
                return cardText + " (DownCard)";
            }

            return cardText;
        }
    }
}