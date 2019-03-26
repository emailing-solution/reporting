using System;
using System.Windows.Forms;

namespace reporting
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += new EventHandler(appexit);
            //AppDomain.CurrentDomain.ProcessExit += new EventHandler(appexit);

            Application.Run(new Report());
            //
        }

        static void appexit(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("CMD.exe", "/C taskkill /F /IM chromedriver.exe /T");
            Environment.Exit(Environment.ExitCode);
        }
    }
}
