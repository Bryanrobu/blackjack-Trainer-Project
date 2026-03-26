using System;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class StartScreen : Form
    {
        public int NumberOfPlayers { get; private set; }
        public StartScreen()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            button1.Text = "Start Game";
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            NumberOfPlayers = (int)numericUpDownPlayers.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}