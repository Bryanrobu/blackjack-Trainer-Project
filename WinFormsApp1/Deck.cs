using BlackjackOOP;

internal class Deck
{
    public List<Card> cards = new List<Card>();

    public Deck()
    {
        foreach (Suit s in Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank r in Enum.GetValues(typeof(Rank)))
            {
                cards.Add(new Card(r, s));
            }
        }
    }
}