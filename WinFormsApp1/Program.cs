namespace BlackjackOOP
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            StartScreen startScreen = new StartScreen();

            if (startScreen.ShowDialog() == DialogResult.OK)
            {
                Form1 mainGame = new Form1(startScreen.NumberOfPlayers);
                mainGame.StartPosition = FormStartPosition.Manual;
                mainGame.Location = startScreen.Location;
                mainGame.Size = startScreen.Size;
                Application.Run(mainGame);
            }

        }
    }
}