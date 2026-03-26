namespace BlackjackOOP
{
    public partial class Form1 : Form
    {

        private Deck deck;
        private int currentIndex = 0;
        public Form1(int players)
        {
            InitializeComponent();

            deck = new Deck();
            UpdateDisplay();
            button1.Text = "Next Card";
            button2.Text = "Shuffle Deck";
            button3.Text = "Reset Deck";
            label2.Text = "players: " + players;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
