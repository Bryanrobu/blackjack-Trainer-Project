using System.Net.Quic;

namespace BlackjackOOP
{
    public partial class Form1 : Form
    {

        private Deck deck;
        private int currentIndex = 0;
        public Form1()
        {
            InitializeComponent();

            deck = new Deck();
            UpdateDisplay();
            button1.Text = "Next Card";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void UpdateDisplay()
        {
            Card currentCard = deck.cards[currentIndex];
            label1.Text = currentCard.ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            currentIndex++;

            if (currentIndex >= deck.cards.Count)
            {
                MessageBox.Show("End of the deck!");
                return;
            }

            UpdateDisplay();
        }
    }
}
