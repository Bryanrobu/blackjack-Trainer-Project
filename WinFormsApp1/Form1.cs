namespace BlackjackOOP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Deck deck1 = new Deck();
            label1.Text = deck1.cards[0].ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
