using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BlackjackOOP
{
    public partial class Form1 : Form
    {

        private Deck deck;
        private int currentIndex = 0;
        private int currentPlayerIndex = 0;
        private int mistakes = 0;
        private int dealtStartCards = 0;
        private List<Player> activePlayers = new List<Player>();
        private List<string> mistakeMessages = new List<string>();

        public enum gameState
        {
            SETUP,
            START,
            SHUFFLED,
            STARTCARD,
            ASKED,
            MOVE,
            SCORE,
        }
        public static gameState currentState = gameState.SETUP;

        public Form1(int players)
        {
            InitializeComponent();

            deck = new Deck();
            UpdateDisplay();
            button1.Text = "Shuffle Deck";
            for (int i = 1; i <= players; i++)
            {
                CreatePlayerMenu(new Player("Player " + i));
            }

            CreatePlayerMenu(new Dealer());

        }

        private void CreatePlayerMenu(Player newPlayer) 
        {
            activePlayers.Add(newPlayer);

            ToolStripMenuItem playerMenu = new ToolStripMenuItem(newPlayer.Name);

            ToolStripMenuItem dealItem = new ToolStripMenuItem("Give Start Card");
            ToolStripMenuItem downcardItem = new ToolStripMenuItem("Give Downcard");
            ToolStripMenuItem hitItem = new ToolStripMenuItem("Hit");
            ToolStripMenuItem standItem = new ToolStripMenuItem("Stand");
            ToolStripMenuItem actionItem = new ToolStripMenuItem("Ask Action");
            ToolStripMenuItem bustItem = new ToolStripMenuItem("Bust");
            ToolStripMenuItem scoreItem = new ToolStripMenuItem("View Cards");
            ToolStripMenuItem winItem = new ToolStripMenuItem("Winner");
            ToolStripMenuItem lostItem = new ToolStripMenuItem("Loser");
            dealItem.Tag = newPlayer;
            downcardItem.Tag = newPlayer;
            hitItem.Tag = newPlayer;
            standItem.Tag = newPlayer;
            actionItem.Tag = newPlayer;
            bustItem.Tag = newPlayer;
            scoreItem.Tag = newPlayer;
            winItem.Tag = newPlayer;
            lostItem.Tag = newPlayer;

            dealItem.Click += PlayerMenuItem_Click;
            downcardItem.Click += PlayerMenuItem_Click;
            hitItem.Click += PlayerMenuItem_Click;
            standItem.Click += PlayerMenuItem_Click;
            actionItem.Click += PlayerMenuItem_Click;
            bustItem.Click += PlayerMenuItem_Click;
            scoreItem.Click += CardItem_Click;
            winItem.Click += ResultItem_Click;
            lostItem.Click += ResultItem_Click;

            playerMenu.DropDownItems.AddRange(new ToolStripItem[] {
                dealItem, downcardItem, hitItem, standItem, actionItem, bustItem, scoreItem, winItem, lostItem,
            });

            if (newPlayer is Dealer)
            {
                actionItem.Visible = false;
                winItem.Visible = false;
                lostItem.Visible = false;
            } else
            {
                downcardItem.Visible = false;
            }

            menuStrip1.Items.Add(playerMenu);
        }

        private void PlayerMenu(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateDisplay();
        }
        private void UpdateDisplay()
        {
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                if (item.DropDownItems.Count > 0 && item.DropDownItems[0].Tag is Player player)
                {
                    item.Text = $"{player.Name} (Score: {player.GetCurrentHandValue()})";

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
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if ( currentState == gameState.START)
            {
                deck.Shuffle();
                currentState = gameState.SHUFFLED;
                UpdateDisplay();
                return;
            }
            RegisterMistake("You have shuffled already.");
            UpdateDisplay();
        }

        private void PlayerMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;

            if (clickedItem.HasDropDownItems)
            {
                return;
            }

            if (currentState == gameState.SCORE)
            {
                RegisterMistake("The game is finished, you can only assign winner/loser or look at the statistics.");
            }

            Player selectedPlayer = (Player)clickedItem.Tag;
            string action = clickedItem.Text;
            Dealer dealer = selectedPlayer as Dealer;

            if (currentPlayerIndex >= activePlayers.Count)
            {
                RegisterMistake("You've had all players.");
                return;
            }

            if (action != "Give Start Card" && action != "Give Downcard")
            {
                if (currentState == gameState.START || currentState == gameState.SHUFFLED)
                {
                    RegisterMistake($"The startcards should be dealt before you can run: {action}.");
                    return;
                }

                if (selectedPlayer != activePlayers[currentPlayerIndex])
                {
                    RegisterMistake("It is not this user's turn.");
                    return;
                }
            }

            switch (action)
            {
                case "Give Start Card":
                    if (dealer != null && dealer.NeedsDownCard())
                    {
                        RegisterMistake("The second dealer startcard has to be closed (down), you are trying to give a normal one.");
                        return;
                    }

                    if (CanReceiveStartCard(selectedPlayer))
                    {
                        selectedPlayer.ReceiveCard(deck.cards[currentIndex]);
                        currentIndex++;
                        UpdateDisplay();
                        MessageBox.Show($"{selectedPlayer.Name} has received a start card.");
                    }
                    break;

                case "Give Downcard":
                    if (dealer == null)
                    {
                        RegisterMistake("A player can't receive a downcard.");
                        return;
                    }
                    if (!dealer.NeedsDownCard())
                    {
                        RegisterMistake("u can't give a downcard. The dealer has to have 1 open card first, or already has both.");
                        return;
                    }

                    if (CanReceiveStartCard(selectedPlayer))
                    {
                        Card downCard = deck.cards[currentIndex];
                        downCard.Flip();
                        selectedPlayer.ReceiveCard(downCard);
                        currentIndex++;
                        UpdateDisplay();
                        MessageBox.Show("The dealer has received a downcard (closed card).");
                    }
                    break;

                case "Ask Action":


                    if (selectedPlayer.GetCurrentHandValue() > 21)
                    {
                        RegisterMistake("You are asking for advice while you have over 21 points");
                        return;
                    }

                    switch (currentState)
                    {

                        case gameState.MOVE:
                        case gameState.STARTCARD:
                            currentState = gameState.ASKED;
                            UpdateDisplay();
                            break;

                        case gameState.ASKED:
                            RegisterMistake("You asked for advice a second time without doing the action.");
                            return;

                        default:
                            RegisterMistake("You are asking for advice too early or too late.");
                            return;
                    }
                    string advice = selectedPlayer.getOpinion();
                    MessageBox.Show($"{selectedPlayer.Name} wants: {advice}");
                    break;

                case "Hit":
                    if (dealer != null)
                    {
                        selectedPlayer.getOpinion();

                        wrongDealerChoice(selectedPlayer);

                        currentState = gameState.ASKED;
                    }
                    if (dealer == null && currentState != gameState.ASKED)
                    {
                        RegisterMistake("You have to ask for advice before you can hit.");
                        return;
                    }
                    if (dealer == null && selectedPlayer.lastOpinion != "Hit")
                    {
                        RegisterMistake ("The player wanted 'Stand', not 'Hit'.");
                        return;
                    }
                    if (deck != null && currentIndex < deck.cards.Count)
                    {
                        if (currentState == gameState.ASKED)
                        {
                            currentState = gameState.MOVE;
                        }
                        Card card = deck.cards[currentIndex];
                        selectedPlayer.ReceiveCard(card);
                        currentIndex++;
                        UpdateDisplay();
                        MessageBox.Show($"{selectedPlayer.Name} hits and gets: {card}");
                    }
                    break;

                case "Stand":

                    if (selectedPlayer.IsBust())
                    {
                        RegisterMistake("You have over 21 points, so you have to choose bust");
                        return;
                    }

                    if (dealer != null)
                    {
                        selectedPlayer.getOpinion();

                        wrongDealerChoice(selectedPlayer);

                        currentState = gameState.ASKED;
                    }
                    if (dealer == null && currentState != gameState.ASKED)
                    {
                        RegisterMistake("You have to ask for advice before you can stand.");
                        return;
                    }
                    if (dealer == null && selectedPlayer.lastOpinion != "Stand")
                    {
                        RegisterMistake("The player wanted 'Hit', not 'Stand'.");
                        return;
                    }
                    if (currentState == gameState.ASKED)
                    {
                        currentState = gameState.MOVE;
                        UpdateDisplay();
                    }
                    EndTurn(selectedPlayer, (ToolStripMenuItem)clickedItem.OwnerItem, "Stand");
                    break;

                case "Bust":
                    if (!selectedPlayer.IsBust())
                    {
                        RegisterMistake("You can only choose bust if the score is over 21.");
                        return;
                    }
                    switch (currentState)
                    {
                        case gameState.MOVE:
                            break;
                        default:
                            RegisterMistake("Youre too early to choose 'Bust', the game has not finished.");
                            return;
                    }
                    EndTurn(selectedPlayer, (ToolStripMenuItem)clickedItem.OwnerItem, "Bust");
                    break;
            }
        }
        private void CardItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            Player selectedPlayer = (Player)clickedItem.Tag;

            string cardsOverview = selectedPlayer.Hand.Count > 0
                ? string.Join("\n", selectedPlayer.Hand)
                : "No cards in hand.";

            MessageBox.Show($"Cards of: {selectedPlayer.Name}:\n\n{cardsOverview}\nTotal score: {selectedPlayer.GetCurrentHandValue()}");
        }

        private void ResultItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            Player selectedPlayer = (Player)clickedItem.Tag;
            string result = clickedItem.Text;


            if (currentState != gameState.SCORE)
            {
                RegisterMistake("You can only choose winner/loser when all players have finished their rounds.");
                return;
            }

            Dealer dealer = activePlayers.OfType<Dealer>().FirstOrDefault();
            if (dealer == null) return;

            bool hasWon = selectedPlayer.HasBeaten(dealer);

            int dealerScore = dealer.GetCurrentHandValue();
            int playerScore = selectedPlayer.GetCurrentHandValue();

            switch (result)
            {
                case "Winner":
                    if (!hasWon)
                    {
                        RegisterMistake($"{selectedPlayer.Name} did not win, choose loser.");
                        return;
                    }
                    break;

                case "Loser":
                    if (hasWon)
                    {
                        RegisterMistake($"{selectedPlayer.Name} did not lose, choose winner.");
                        return;
                    }
                    break;
            }
            if (clickedItem.OwnerItem is ToolStripMenuItem playerHeadMenu)
            {
                playerHeadMenu.Tag = result;
                selectedPlayer.Status = result;
                UpdateDisplay();
            }
        }
        private void RegisterMistake(string message)
        {
            mistakes++;
            mistakeMessages.Add(message);
            UpdateDisplay();
            MessageBox.Show(message);
        }
        private void EndTurn(Player player, ToolStripMenuItem playerMenu, string status)
        {
            playerMenu.Tag = status;
            UpdateDisplay();
            MessageBox.Show($"{player.Name} has finished ({status}) with a score of: {player.GetCurrentHandValue()}.");

            currentPlayerIndex++;

            if (currentPlayerIndex >= activePlayers.Count)
            {
                MessageBox.Show("Everyone has had their turn");
                currentState = gameState.SCORE;
            }
        }

        private bool CanReceiveStartCard(Player player)
        {
            if (currentState != gameState.SHUFFLED)
            {
                RegisterMistake("You have to shuffle your deck, or you can't deal start cards anymore.");
                return false;
            }

            int totalPlayers = activePlayers.Count;
            int whosTurnIsIt = dealtStartCards % totalPlayers;

            if (dealtStartCards >= totalPlayers * 2)
            {
                RegisterMistake("All start cards have been dealt already.");
                return false;
            }
            
            if (player != activePlayers[whosTurnIsIt])
            {
                RegisterMistake("It's not this player's turn.");
                return false;
            }

            dealtStartCards++;

            if (dealtStartCards >= totalPlayers * 2)
            {
                currentState = gameState.STARTCARD;
            }

            return true;
        }
        private void btnStats_Click(object sender, EventArgs e)
        {
            StatsForm stats = new StatsForm(activePlayers, mistakes, mistakeMessages);

            stats.StartPosition = FormStartPosition.Manual;
            stats.Location = this.Location;
            stats.Size = this.Size;

            DialogResult result = stats.ShowDialog();
            if (result == DialogResult.Retry)
            {
                Application.Restart();
            }

            if (result == DialogResult.Abort)
            {
                Application.Exit();
            }
        }

        private void wrongDealerChoice(Player selectedPlayer)
        {
            if (selectedPlayer.lastOpinion != "Hit")
            {
                RegisterMistake($"You have a score of: {selectedPlayer.GetCurrentHandValue()} and have to '{selectedPlayer.lastOpinion}'.");
                return;
            }
        }
    }
}