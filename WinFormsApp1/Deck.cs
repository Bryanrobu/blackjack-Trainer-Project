using BlackjackOOP;

internal class Deck
{
    public List<Card> cards = new List<Card>();
    private Random rng = new Random();

    public Deck()
    {
        foreach (Suit s in Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank r in Enum.GetValues(typeof(Rank)))
            {
                cards.Add(new Card(r, s));
            }
        }
        Shuffle();
    }

    public void Shuffle()
    {
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);

            Card temp = cards[k];
            cards[k] = cards[n];
            cards[n] = temp;
        }
    }
}