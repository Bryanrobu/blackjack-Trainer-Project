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
                Point savedLocation = startScreen.Location;
                int gekozenSpelers = startScreen.NumberOfPlayers;
                Form1 mainGame = new Form1(gekozenSpelers);
                mainGame.StartPosition = FormStartPosition.Manual;
                mainGame.Location = savedLocation;
                Application.Run(mainGame);
            }

        }
    }
}