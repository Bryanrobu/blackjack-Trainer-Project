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
            numericUpDownPlayers.Minimum = 1;
            numericUpDownPlayers.Maximum = 4;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            NumberOfPlayers = (int)numericUpDownPlayers.Value;
            if (NumberOfPlayers < 1 || NumberOfPlayers > 4)
            {
                MessageBox.Show("Kies a.u.b. een aantal spelers tussen de 1 en 4.");
                return;
            }
            switch (Form1.currentState)
            {
                case Form1.gameState.SETUP:
                    Form1.currentState = Form1.gameState.START;
                    break;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}