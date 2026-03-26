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
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            StartScreen startScreen = new StartScreen();

            // 2. Laat het scherm zien en wacht tot de speler op "Start" klikt (DialogResult.OK)
            if (startScreen.ShowDialog() == DialogResult.OK)
            {
                // 3. Haal het gekozen aantal spelers op
                int gekozenSpelers = startScreen.NumberOfPlayers;

                // 4. Start het hoofdspel (Form1) en geef het aantal spelers mee!
                Application.Run(new Form1(gekozenSpelers));
            }
        }
    }
}