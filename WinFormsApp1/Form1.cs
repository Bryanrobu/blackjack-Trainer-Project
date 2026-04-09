using System.Net.Quic;
using System.Reflection.Emit;
using static BlackjackOOP.Form1;

namespace BlackjackOOP
{
    public partial class Form1 : Form
    {

        private Deck deck;
        private int currentIndex = 0;
        private int huidigeSpelerIndex = 0;
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
                actieveSpelers.Add(nieuweSpeler);

                ToolStripMenuItem spelerMenu = new ToolStripMenuItem(nieuweSpeler.Name);

                ToolStripMenuItem hitItem = new ToolStripMenuItem("Hit");
                ToolStripMenuItem standItem = new ToolStripMenuItem("Stand");
                ToolStripMenuItem actieItem = new ToolStripMenuItem("Vraag actie");
                ToolStripMenuItem bustItem = new ToolStripMenuItem("Bust");
                ToolStripMenuItem scoreItem = new ToolStripMenuItem("Bekijk score");

                hitItem.Tag = nieuweSpeler;
                standItem.Tag = nieuweSpeler;
                actieItem.Tag = nieuweSpeler;
                bustItem.Tag = nieuweSpeler;
                scoreItem.Tag = nieuweSpeler;

                hitItem.Click += PlayerMenuItem_Click;
                standItem.Click += PlayerMenuItem_Click;
                actieItem.Click += PlayerMenuItem_Click;
                bustItem.Click += PlayerMenuItem_Click;
                scoreItem.Click += ScoreItem_Click;

                spelerMenu.DropDownItems.Add(hitItem);
                spelerMenu.DropDownItems.Add(standItem);
                spelerMenu.DropDownItems.Add(actieItem);
                spelerMenu.DropDownItems.Add(bustItem);
                spelerMenu.DropDownItems.Add(scoreItem);

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

            if (huidigeSpelerIndex >= actieveSpelers.Count)
            {
                MessageBox.Show("Je hebt alle spelers gehad");
                mistakes++;
                UpdateDisplay();
                return;
            }

            if (geselecteerdeSpeler != actieveSpelers[huidigeSpelerIndex])
            {
                mistakes++;
                UpdateDisplay();
                return;
            }

            switch (actie)
            {
                case "Vraag actie":
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
                        return;
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
                        return;
                    }
                    if (currentState == gameState.ASKED)
                    {
                        currentState = gameState.MOVE;
                        UpdateDisplay();
                    }
                    MessageBox.Show($"{geselecteerdeSpeler.Name} blijft staan op {geselecteerdeSpeler.GetCurrentHandValue()}.");
                    huidigeSpelerIndex++;
                    break;

                case "Bust":
                    if (geselecteerdeSpeler.GetCurrentHandValue() <= 21)
                    {
                        mistakes++;
                        UpdateDisplay();
                        return;
                    }
                    switch (currentState)
                    {
                        case gameState.MOVE:
                            break;
                        default:
                            mistakes++;
                            UpdateDisplay();
                            return;
                    }
                    MessageBox.Show($"{geselecteerdeSpeler.Name} is nu bust met {geselecteerdeSpeler.GetCurrentHandValue()}.");
                    huidigeSpelerIndex++;
                    break;
            }
        }
        private void ScoreItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            Player geselecteerdeSpeler = (Player)clickedItem.Tag;

            int huidigeScore = geselecteerdeSpeler.GetCurrentHandValue();

            MessageBox.Show($"De huidige handwaarde van {geselecteerdeSpeler.Name} is: {huidigeScore}");
        }
    }
}