using System.Text;

namespace BlackjackOOP
{
    public partial class StatsForm : Form
    {
        public StatsForm(List<Player> spelers, int mistakes)
        {
            InitializeComponent();

            label1.Text = "Eindstatistieken van de Ronde";
            label2.Text = $"Aantal gemaakte fouten: {mistakes}";

            button1.Text = "Opnieuw";
            button2.Text = "Afsluiten";

            VulStatistieken(spelers);
        }

        private void VulStatistieken(List<Player> spelers)
        {
            textBox1.Clear();
            StringBuilder sb = new StringBuilder();

            foreach (Player speler in spelers)
            {
                sb.AppendLine($"NAME: {speler.Name}");
                sb.AppendLine($"SCORE: {speler.GetCurrentHandValue()}");
                sb.AppendLine($"KAARTEN: {string.Join(", ", speler.Hand)}");

                if (speler is Dealer)
                {
                    if (speler.IsBust())
                    {
                        sb.AppendLine("STATUS: Bust");
                    }
                    else
                    {
                        sb.AppendLine("STATUS: Dealer");
                    }
                }

                if (speler.IsBust())
                {
                    sb.AppendLine("STATUS: BUST (Verloren)");
                }
                else if (speler.Status == "Winnaar")
                {
                    sb.AppendLine("STATUS: Gewonnen ");
                }
                else if (speler.Status == "Verliezer")
                {
                    sb.AppendLine("STATUS: Verloren");
                }
                else
                {
                    sb.AppendLine("STATUS: Nog niet klaar");
                }

                sb.AppendLine(" ");
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