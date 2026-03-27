using System;
using System.Windows.Forms;

namespace BlackjackOOP
{
    public partial class StartScreen : Form
    {
        public int NumberOfPlayers { get; private set; }
        public StartScreen()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            button1.Text = "Start Game";
            label1.Text = Form1.currentState.ToString();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            switch (Form1.currentState)
            {
                case Form1.gameState.SETUP:
                    Form1.currentState = Form1.gameState.START;
                    break;
            }
            NumberOfPlayers = (int)numericUpDownPlayers.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}