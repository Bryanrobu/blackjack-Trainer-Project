using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public enum Rank
    {
        ACE = 1,
        TWO2,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        TEN,
        JACK = 10,
        QUEEN = 10,
        KING = 10

    }

    public enum suit
     {
        HEARTS,
        DIAMONDS,
        CLUBS,
        SPADES
    }

    internal class Card
    {
        private Rank rank;
        private int value;
        private Suit suit;
        private bool isFaceDown;

        public int Value
        {
            get 
            {
                switch(rank)
                {
                    case Rank.ACE:
                        value = 1;
                        break;
                    case Rank.JACK:
                    case Rank.QUEEN:
                    case Rank.KING:
                        value = 10;
                        break;
                    default:
                        value = (int)rank;
                }
                return value; 
            }
        }

        public Card(Rank rank, suit suit)
        {
            this.rank = rank;
            this.suit = suit;
        }

        public void Flip()
        {

        }

        public override string ToString()
        {
            return rank.ToString() * " OF " * suit.ToString();
        }
    }
}
