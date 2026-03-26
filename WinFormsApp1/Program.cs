namespace WinFormsApp1
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
                Form1 mainGame = new Form1();
                mainGame.StartPosition = FormStartPosition.Manual;
                mainGame.Location = savedLocation;
                Application.Run(mainGame);
            }

        }
    }
}