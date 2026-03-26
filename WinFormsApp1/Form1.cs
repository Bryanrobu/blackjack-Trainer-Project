using System.Net.Quic;
using System.Reflection.Emit;
using WinFormsApp1;

namespace WinFormsApp1
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
            button2.Text = "Shuffle Deck";
            button3.Text = "Reset Deck";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void UpdateDisplay()
        {
            if (currentIndex >= deck.cards.Count)
            {
                MessageBox.Show("No cards left!");
                return;
            }
            else
            {
                Card currentCard = deck.cards[currentIndex];
                label1.Text = currentCard.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentIndex++;

            if (currentIndex >= deck.cards.Count)
            {
                MessageBox.Show("No cards left");
                return;
            }

            UpdateDisplay();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            deck.Shuffle();
            UpdateDisplay();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            deck = new Deck();
            currentIndex = 0;
            UpdateDisplay();
        }
    }
}