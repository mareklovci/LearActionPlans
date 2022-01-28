using LearActionPlans.Views;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace LearActionPlans
{
    static class Program
    {
        private readonly static string[] args;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //var hostFile = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            //ConfigurationManager.OpenExeConfiguration(hostFile + ".config");

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain(args));
        }
    }
}
