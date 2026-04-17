using System.Text;

namespace BlackjackOOP
{
    public partial class StatsForm : Form
    {
        public StatsForm(List<Player> players, int mistakes, List<string> mistakeDetails)
        {
            InitializeComponent();

            label1.Text = "Final Statistics of the Round";
            label2.Text = $"Number of mistakes made: {mistakes}";

            button1.Text = "Play Again";
            button2.Text = "Exit";

            VulStatistieken(players, mistakeDetails);
        }

        private void VulStatistieken(List<Player> players, List<string> mistakeDetails)
        {
            textBox1.Clear();
            StringBuilder sb = new StringBuilder();

            foreach (Player player in players)
            {
                sb.AppendLine($"NAME: {player.Name}");
                sb.AppendLine($"SCORE: {player.GetCurrentHandValue()}");
                sb.AppendLine($"Cards: {string.Join(", ", player.Hand)}");
                if (player is Dealer)
                {
                    if (player.IsBust())
                    {
                        sb.AppendLine("STATUS: Bust");
                    }
                    else
                    {
                        sb.AppendLine("STATUS: Dealer");
                    }
                }
                if (player is not Dealer)
                { 
                    if (player.IsBust())
                    {
                        sb.AppendLine("STATUS: BUST (Lost)");
                    }
                    else if (player.Status == "Winner")
                    {
                        sb.AppendLine("STATUS: Won");
                    }
                    else if (player.Status == "Loser")
                    {
                        sb.AppendLine("STATUS: Lost");
                    }
                    else
                    {
                        sb.AppendLine("STATUS: Not finished");
                    }
                }

                sb.AppendLine(" ");
            }

            if (mistakeDetails.Count > 0)
            {
                sb.AppendLine("These are your mistakes:");
                for (int i = 0; i < mistakeDetails.Count; i++)
                {
                    sb.AppendLine($"{i + 1}. {mistakeDetails[i]}");
                }
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine("Perfect play! No mistakes made.");
                sb.AppendLine();
            }

            textBox1.Text = sb.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }
    }
}