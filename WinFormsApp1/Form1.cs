using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
                CreatePlayerMenu(new Player("Speler " + i));
            }

            CreatePlayerMenu(new Dealer());

        }

        private void CreatePlayerMenu(Player nieuweSpeler) 
        {
            actieveSpelers.Add(nieuweSpeler);

            ToolStripMenuItem spelerMenu = new ToolStripMenuItem(nieuweSpeler.Name);

            ToolStripMenuItem deelItem = new ToolStripMenuItem("Geef Startkaart");
            ToolStripMenuItem downcardItem = new ToolStripMenuItem("Geef Downcard");
            ToolStripMenuItem hitItem = new ToolStripMenuItem("Hit");
            ToolStripMenuItem standItem = new ToolStripMenuItem("Stand");
            ToolStripMenuItem actieItem = new ToolStripMenuItem("Vraag actie");
            ToolStripMenuItem bustItem = new ToolStripMenuItem("Bust");
            ToolStripMenuItem scoreItem = new ToolStripMenuItem("Bekijk Kaarten");

            deelItem.Tag = nieuweSpeler;
            downcardItem.Tag = nieuweSpeler;
            hitItem.Tag = nieuweSpeler;
            standItem.Tag = nieuweSpeler;
            actieItem.Tag = nieuweSpeler;
            bustItem.Tag = nieuweSpeler;
            scoreItem.Tag = nieuweSpeler;

            deelItem.Click += PlayerMenuItem_Click;
            downcardItem.Click += PlayerMenuItem_Click;
            hitItem.Click += PlayerMenuItem_Click;
            standItem.Click += PlayerMenuItem_Click;
            actieItem.Click += PlayerMenuItem_Click;
            bustItem.Click += PlayerMenuItem_Click;
            scoreItem.Click += ScoreItem_Click;

            spelerMenu.DropDownItems.AddRange(new ToolStripItem[] {
                deelItem, downcardItem, hitItem, standItem, actieItem, bustItem, scoreItem
            });

            if (nieuweSpeler is Dealer)
            {
                actieItem.Visible = false;
            } else
            {
                downcardItem.Visible = false;
            }

            menuStrip1.Items.Add(spelerMenu);
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
            
            if ( currentState == gameState.START)
            {
                currentState = gameState.SHUFFLED;
            }

            deck.Shuffle();
            UpdateDisplay();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            currentState = gameState.START;
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
            Dealer dealer = geselecteerdeSpeler as Dealer;

            if (huidigeSpelerIndex >= actieveSpelers.Count)
            {
                RegisterMistake("Je hebt alle spelers gehad.");
                return;
            }

            if (actie != "Geef Startkaart" && actie != "Geef Downcard")
            {
                if (currentState == gameState.START || currentState == gameState.SHUFFLED)
                {
                    RegisterMistake("De startkaarten moeten eerst worden uitgedeeld voordat je deze actie mag doen.");
                    return;
                }

                if (geselecteerdeSpeler != actieveSpelers[huidigeSpelerIndex])
                {
                    RegisterMistake("Deze speler is niet aan de beurt.");
                    return;
                }
            }

            switch (actie)
            {
                case "Geef Startkaart":
                    if (dealer != null && dealer.HeeftDowncardNodig())
                    {
                        RegisterMistake("De tweede kaart van de dealer moet gesloten zijn! Gebruik de 'Geef Downcard' knop.");
                        return;
                    }

                    if (MagStartkaartOntvangen(geselecteerdeSpeler))
                    {
                        geselecteerdeSpeler.ReceiveCard(deck.cards[currentIndex]);
                        currentIndex++;
                        UpdateDisplay();
                        MessageBox.Show($"{geselecteerdeSpeler.Name} heeft een startkaart ontvangen.");
                    }
                    break;

                case "Geef Downcard":
                    if (dealer == null)
                    {
                        RegisterMistake("Alleen de dealer kan een Downcard ontvangen.");
                        return;
                    }
                    if (!dealer.HeeftDowncardNodig())
                    {
                        RegisterMistake("De dealer moet eerst 1 open kaart hebben, of heeft beide kaarten al.");
                        return;
                    }

                    if (MagStartkaartOntvangen(geselecteerdeSpeler))
                    {
                        Card downCard = deck.cards[currentIndex];
                        downCard.Flip();
                        geselecteerdeSpeler.ReceiveCard(downCard);
                        currentIndex++;
                        UpdateDisplay();
                        MessageBox.Show("De dealer heeft een Downcard (gesloten kaart) ontvangen.");
                    }
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
                            RegisterMistake("Je hebt al om advies gevraagd.");
                            return;

                        default:
                            RegisterMistake("Je mag nu nog geen advies vragen.");
                            return;
                    }
                    string advies = geselecteerdeSpeler.getOpinion();
                    MessageBox.Show($"{geselecteerdeSpeler.Name} wilt: {advies}");
                    break;

                case "Hit":
                    if (dealer != null)
                    {
                        geselecteerdeSpeler.getOpinion();

                        if (geselecteerdeSpeler.LaatsteMening != "Hit")
                        {
                            RegisterMistake($"U heeft een waarde van {geselecteerdeSpeler.GetCurrentHandValue()} en moet dus '{geselecteerdeSpeler.LaatsteMening}'.");
                            return;
                        }

                        currentState = gameState.ASKED;
                    }
                    if (dealer == null && currentState != gameState.ASKED)
                    {
                        RegisterMistake("Je moet eerst om advies vragen voordat je mag Hitten.");
                        return;
                    }
                    if (dealer == null && geselecteerdeSpeler.LaatsteMening != "Hit")
                    {
                        RegisterMistake ("de speler wilde 'Stand', niet 'Hit'.");
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

                    if (geselecteerdeSpeler.IsBust())
                    {
                        RegisterMistake("Je hebt meer dan 21 punten, dus je moet bust kiezen");
                        return;
                    }

                    if (dealer != null)
                    {
                        geselecteerdeSpeler.getOpinion();

                        if (geselecteerdeSpeler.LaatsteMening != "Stand")
                        {
                            RegisterMistake($"U heeft een waarde van {geselecteerdeSpeler.GetCurrentHandValue()} en moet dus '{geselecteerdeSpeler.LaatsteMening}'.");
                            return;
                        }

                        currentState = gameState.ASKED;
                    }
                    if (dealer == null && currentState != gameState.ASKED)
                    {
                        RegisterMistake("Je moet eerst om advies vragen voordat je mag Standen.");
                        return;
                    }
                    if (dealer == null && geselecteerdeSpeler.LaatsteMening != "Stand")
                    {
                        RegisterMistake("de speler wilde 'Hit', niet 'Stand'.");
                        return;
                    }
                    if (currentState == gameState.ASKED)
                    {
                        currentState = gameState.MOVE;
                        UpdateDisplay();
                    }
                    BeurtVoorbij(geselecteerdeSpeler, (ToolStripMenuItem)clickedItem.OwnerItem, "Stand");
                    break;

                case "Bust":
                    if (!geselecteerdeSpeler.IsBust())
                    {
                        RegisterMistake("Je mag alleen 'Bust' kiezen als je handwaarde hoger is dan 21.");
                        return;
                    }
                    switch (currentState)
                    {
                        case gameState.MOVE:
                            break;
                        default:
                            RegisterMistake("Je mag nu nog geen 'Bust' kiezen.");
                            return;
                    }
                    BeurtVoorbij(geselecteerdeSpeler, (ToolStripMenuItem)clickedItem.OwnerItem, "Bust");
                    break;
            }
        }
        private void ScoreItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            Player geselecteerdeSpeler = (Player)clickedItem.Tag;

            string kaartenOverzicht = geselecteerdeSpeler.Hand.Count > 0
                ? string.Join("\n", geselecteerdeSpeler.Hand)
                : "Geen kaarten in hand.";

            MessageBox.Show($"Kaarten van {geselecteerdeSpeler.Name}:\n\n{kaartenOverzicht}\nTotale score: {geselecteerdeSpeler.GetCurrentHandValue()}");
        }
        private void RegisterMistake(string message)
        {
            mistakes++;
            UpdateDisplay();
            MessageBox.Show(message);
        }
        private void BeurtVoorbij(Player speler, ToolStripMenuItem spelerMenu, string status)
        {
            spelerMenu.Tag = status;
            UpdateDisplay();
            MessageBox.Show($"{speler.Name} is klaar ({status}) met een score van {speler.GetCurrentHandValue()}.");

            huidigeSpelerIndex++;

            if (huidigeSpelerIndex >= actieveSpelers.Count)
            {
                MessageBox.Show("Iedereen is aan de beurt geweest");
            }
        }

        private bool MagStartkaartOntvangen(Player speler)
        {
            if (currentState != gameState.SHUFFLED)
            {
                RegisterMistake("Je moet je deck eerst shuffelen of je mag nu geen startkaarten meer uitdelen.");
                return false;
            }

            int totaalPersonen = actieveSpelers.Count;
            int wieIsAanDeBeurt = uitgedeeldeStartkaarten % totaalPersonen;

            if (uitgedeeldeStartkaarten >= totaalPersonen * 2)
            {
                RegisterMistake("Alle startkaarten zijn al uitgedeeld.");
                return false;
            }

            if (speler != actieveSpelers[wieIsAanDeBeurt])
            {
                RegisterMistake("Deze speler is niet aan de beurt.");
                return false;
            }

            uitgedeeldeStartkaarten++;

            if (uitgedeeldeStartkaarten >= totaalPersonen * 2)
            {
                currentState = gameState.STARTCARD;
            }

            return true;
        }
    }
}