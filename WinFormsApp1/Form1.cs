using System.Net.Quic;
using System.Reflection.Emit;
using static BlackjackOOP.Form1;

namespace BlackjackOOP
{
    public partial class Form1 : Form
    {

        private Deck deck;
        private int currentIndex = 0;
        private int mistakes = 0;
        private List<Player> actieveSpelers = new List<Player>();

        public enum gameState
        {
            SETUP,
            START,
            SHUFFLED,
            ASKED,
            MOVE
        }
        public static gameState currentState = gameState.SETUP;

        public Form1(int spelers)
        {
            InitializeComponent();

            deck = new Deck();
            UpdateDisplay();
            button1.Text = "Next Card";
            button2.Text = "Shuffle Deck";
            button3.Text = "Reset Deck";
            label2.Text = "Players: " + spelers;
            label3.Text = currentState.ToString();
            label4.Text = "mistakes: " + mistakes.ToString();
            for (int i = 1; i <= spelers; i++)
            {
                Player nieuweSpeler = new Player("Speler " + i);

                ToolStripMenuItem spelerMenu = new ToolStripMenuItem(nieuweSpeler.Name);

                ToolStripMenuItem hitItem = new ToolStripMenuItem("Hit");
                ToolStripMenuItem standItem = new ToolStripMenuItem("Stand");
                ToolStripMenuItem adviesItem = new ToolStripMenuItem("Vraag advies");

                hitItem.Tag = nieuweSpeler;
                standItem.Tag = nieuweSpeler;
                adviesItem.Tag = nieuweSpeler;

                hitItem.Click += PlayerMenuItem_Click;
                standItem.Click += PlayerMenuItem_Click;
                adviesItem.Click += PlayerMenuItem_Click;

                spelerMenu.DropDownItems.Add(hitItem);
                spelerMenu.DropDownItems.Add(standItem);
                spelerMenu.DropDownItems.Add(adviesItem);

                menuStrip1.Items.Add(spelerMenu);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void UpdateDisplay()
        {
            label3.Text = currentState.ToString();
            label4.Text = "mistakes: " + mistakes.ToString();
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
            switch (currentState)
            {
                case gameState.SHUFFLED:
                    currentIndex++;
                    break;
                default:
                    mistakes++;
                    break;
            }

            if (currentIndex >= deck.cards.Count)
            {
                MessageBox.Show("No cards left");
                return;
            }

            UpdateDisplay();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch (currentState)
            {
                case gameState.START:
                    currentState = gameState.SHUFFLED;
                    break;

            }
            deck.Shuffle();
            UpdateDisplay();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            switch (currentState)
            {
                case gameState.SHUFFLED:
                    currentState = gameState.START;
                    break;

            }
            deck = new Deck();
            currentIndex = 0;
            UpdateDisplay();
        }

        private void PlayerMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            Player geselecteerdeSpeler = (Player)clickedItem.Tag;
            string actie = clickedItem.Text;

            switch (actie)
            {
                case "Vraag advies":
                    switch (currentState)
                    {

                        case gameState.MOVE:
                        case gameState.SHUFFLED:
                            currentState = gameState.ASKED;
                            UpdateDisplay();
                            break;

                        default:
                            mistakes++;
                            UpdateDisplay();
                            break;
                    }
                    string advies = geselecteerdeSpeler.GetMoveOpinion();
                    MessageBox.Show($"{geselecteerdeSpeler.Name} wilt: {advies}");
                    break;

                case "Hit":
                    if (currentState != gameState.ASKED || geselecteerdeSpeler.GetMoveOpinion() != "Hit")
                    {
                        mistakes++;
                        UpdateDisplay();
                    }
                    if (deck != null && currentIndex < deck.cards.Count)
                    {
                        if (currentState == gameState.ASKED)
                        {
                            currentState = gameState.MOVE;
                        }
                        Card kaart = deck.cards[currentIndex];
                        geselecteerdeSpeler.ReceiveCard(kaart);
                        currentIndex++;
                        UpdateDisplay();
                        MessageBox.Show($"{geselecteerdeSpeler.Name} hit en krijgt: {kaart}\nTotaal nu: {geselecteerdeSpeler.GetCurrentHandValue()}");
                    }
                    break;
                
                case "Stand":
                    if (currentState != gameState.ASKED || geselecteerdeSpeler.GetMoveOpinion() != "Stand")
                    {
                        mistakes++;
                        UpdateDisplay();
                    }
                    if (currentState == gameState.ASKED)
                    {
                        currentState = gameState.MOVE;
                        UpdateDisplay();
                    }
                    MessageBox.Show($"{geselecteerdeSpeler.Name} blijft staan op {geselecteerdeSpeler.GetCurrentHandValue()}.");
                    break;
            }
        }
    }
}