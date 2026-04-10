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
        private int uitgedeeldeStartkaarten = 0;
        private List<Player> actieveSpelers = new List<Player>();

        public enum gameState
        {
            SETUP,
            START,
            SHUFFLED,
            STARTCARD,
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

                ToolStripMenuItem deelItem = new ToolStripMenuItem("Ontvang startkaart");
                ToolStripMenuItem hitItem = new ToolStripMenuItem("Hit");
                ToolStripMenuItem standItem = new ToolStripMenuItem("Stand");
                ToolStripMenuItem actieItem = new ToolStripMenuItem("Vraag actie");
                ToolStripMenuItem bustItem = new ToolStripMenuItem("Bust");
                ToolStripMenuItem scoreItem = new ToolStripMenuItem("Bekijk Kaarten");

                deelItem.Tag = nieuweSpeler;
                hitItem.Tag = nieuweSpeler;
                standItem.Tag = nieuweSpeler;
                actieItem.Tag = nieuweSpeler;
                bustItem.Tag = nieuweSpeler;
                scoreItem.Tag = nieuweSpeler;

                deelItem.Click += PlayerMenuItem_Click;
                hitItem.Click += PlayerMenuItem_Click;
                standItem.Click += PlayerMenuItem_Click;
                actieItem.Click += PlayerMenuItem_Click;
                bustItem.Click += PlayerMenuItem_Click;
                scoreItem.Click += ScoreItem_Click;

                spelerMenu.DropDownItems.Add(deelItem);
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
            UpdateDisplay();
        }
        private void UpdateDisplay()
        {
            label3.Text = currentState.ToString();
            label4.Text = "mistakes: " + mistakes.ToString();

            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                if (item.DropDownItems.Count > 0 && item.DropDownItems[0].Tag is Player speler)
                {
                    item.Text = $"{speler.Name} (Score: {speler.GetCurrentHandValue()})";

                    if (item.Tag != null)
                    {
                        item.Text += $" - {item.Tag}";
                    }
                }
            }

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
                default:
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

            if (clickedItem.HasDropDownItems)
            {
                return;
            }

            Player geselecteerdeSpeler = (Player)clickedItem.Tag;
            string actie = clickedItem.Text;

            if (huidigeSpelerIndex >= actieveSpelers.Count)
            {
                mistakes++;
                UpdateDisplay();
                MessageBox.Show("Je hebt alle spelers gehad");
                return;
            }

            if (actie != "Ontvang startkaart")
            {
                if (huidigeSpelerIndex >= actieveSpelers.Count)
                {
                    mistakes++;
                    UpdateDisplay();
                    MessageBox.Show("Je hebt alle spelers gehad");
                    return;
                }

                if (geselecteerdeSpeler != actieveSpelers[huidigeSpelerIndex])
                {
                    mistakes++;
                    UpdateDisplay();
                    MessageBox.Show("Deze speler is niet aan de beurt");
                    return;
                }
            }

            switch (actie)
            {
                case "Ontvang startkaart":
                    switch (currentState)
                    {
                        case gameState.SHUFFLED:
                            break;

                        default:
                            mistakes++;
                            UpdateDisplay();
                            MessageBox.Show("Je moet je deck eerst schudden");
                            return;
                    }

                    int totaalPersonen = actieveSpelers.Count;
                    int wieIsAanDeBeurt = uitgedeeldeStartkaarten % totaalPersonen;

                    if (uitgedeeldeStartkaarten >= totaalPersonen * 2)
                    {
                        mistakes++;
                        UpdateDisplay();
                        MessageBox.Show("Alle startkaarten zijn al uitgedeeld");
                        return;
                    }

                    if (wieIsAanDeBeurt >= actieveSpelers.Count || geselecteerdeSpeler != actieveSpelers[wieIsAanDeBeurt])
                    {
                        mistakes++;
                        UpdateDisplay();
                        MessageBox.Show("Deze speler is niet aan de beurt");
                        return;
                    }

                    geselecteerdeSpeler.ReceiveCard(deck.cards[currentIndex]);
                    currentIndex++;
                    uitgedeeldeStartkaarten++;

                    if (uitgedeeldeStartkaarten >= totaalPersonen * 2)
                    {
                        currentState = gameState.STARTCARD;
                    }

                    UpdateDisplay();
                    MessageBox.Show($"{geselecteerdeSpeler.Name} heeft een startkaart ontvangen.");
                    break;

                case "Vraag actie":
                    switch (currentState)
                    {

                        case gameState.MOVE:
                        case gameState.STARTCARD:
                            currentState = gameState.ASKED;
                            UpdateDisplay();
                            break;

                        case gameState.ASKED:
                            mistakes++;
                            UpdateDisplay();
                            MessageBox.Show("Je hebt al om advies gevraagd..");
                            return;

                        default:
                            mistakes++;
                            UpdateDisplay();
                            MessageBox.Show("Je mag pas om advies vragen als alle spelers hun startkaarten hebben ontvangen.");
                            return;
                    }
                    string advies = geselecteerdeSpeler.getOpinion();
                    MessageBox.Show($"{geselecteerdeSpeler.Name} wilt: {advies}");
                    break;

                case "Hit":
                    if (currentState != gameState.ASKED)
                    {
                        mistakes++;
                        UpdateDisplay();
                        MessageBox.Show("Je moet eerst om advies vragen voordat je mag Hitten.");
                        return;
                    }
                    if (geselecteerdeSpeler.LaatsteMening != "Hit")
                    {
                        mistakes++;
                        UpdateDisplay();
                        MessageBox.Show("de speler wilde 'Stand', niet 'Hit'.");
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
                        MessageBox.Show($"{geselecteerdeSpeler.Name} hit en krijgt: {kaart}");
                    }
                    break;

                case "Stand":
                    if (currentState != gameState.ASKED)
                    {
                        mistakes++;
                        UpdateDisplay();
                        MessageBox.Show("Je moet eerst om advies vragen voordat je mag Standen.");
                        return;
                    }
                    if (geselecteerdeSpeler.LaatsteMening != "Stand")
                    {
                        mistakes++;
                        UpdateDisplay();
                        MessageBox.Show("de speler wilde 'Hit', niet 'Stand'.");
                        return;
                    }
                    if (currentState == gameState.ASKED)
                    {
                        currentState = gameState.MOVE;
                        UpdateDisplay();
                    }
                    clickedItem.OwnerItem.Tag = "Stand";
                    UpdateDisplay();
                    MessageBox.Show($"{geselecteerdeSpeler.Name} blijft staan op {geselecteerdeSpeler.GetCurrentHandValue()}.");
                    huidigeSpelerIndex++;
                    break;

                case "Bust":
                    if (geselecteerdeSpeler.GetCurrentHandValue() <= 21)
                    {
                        mistakes++;
                        UpdateDisplay();
                        MessageBox.Show("Deze persoon is nog niet boven de 21.");
                        return;
                    }
                    switch (currentState)
                    {
                        case gameState.MOVE:
                            break;
                        default:
                            mistakes++;
                            UpdateDisplay();
                            MessageBox.Show("je mag nu niet busten");
                            return;
                    }
                    clickedItem.OwnerItem.Tag = "Bust!";
                    UpdateDisplay();
                    MessageBox.Show($"{geselecteerdeSpeler.Name} is nu bust met {geselecteerdeSpeler.GetCurrentHandValue()}.");
                    huidigeSpelerIndex++;
                    break;
            }
        }
        private void ScoreItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            Player geselecteerdeSpeler = (Player)clickedItem.Tag;

            string kaartenOverzicht = "";

            foreach (var kaart in geselecteerdeSpeler.Hand)
            {
                kaartenOverzicht += kaart.ToString() + "\n";
            }

            if (string.IsNullOrEmpty(kaartenOverzicht))
            {
                kaartenOverzicht = "Geen kaarten in hand.";
            }

            MessageBox.Show($"Kaarten van {geselecteerdeSpeler.Name}:\n\n{kaartenOverzicht}\nTotale score: {geselecteerdeSpeler.GetCurrentHandValue()}");
        }
    }
}