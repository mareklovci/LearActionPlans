using LearActionPlans.Views;
using System;
using System.Configuration;
using System.Windows.Forms;
using LearActionPlans.Utilities;

namespace LearActionPlans
{
    internal static class Program
    {
        private static string[] args;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // Get Arguments
            args = Environment.GetCommandLineArgs();

            // Parse Arguments
            var arguments = ParseArguments();

            // Modify System Registry
            if (arguments.RunWithoutParameters)
            {
                ModifySystemRegistry();
            }

            // Initialize Application
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain(arguments));
        }

        /// <summary>
        /// Parse arguments
        /// </summary>
        /// <returns>Parsed Arguments object</returns>
        private static ArgumentOptions ParseArguments()
        {
            var arguments = new ArgumentOptions
            {
                // The first parameter is always the name of the program
                RunWithoutParameters = args.Length <= 1
            };

            if (arguments.RunWithoutParameters)
            {
                return arguments;
            }

            var param = args[1].Split('?');
            var values = param[1].Split('&');

            arguments.ActionPlanNumber = Convert.ToString(values[0]).Replace("%20", " ");
            arguments.ActionPlanId = Convert.ToInt32(values[1]);
            arguments.ActionPlanPointId = Convert.ToInt32(values[2]);
            arguments.ActionEndId = Convert.ToInt32(values[3]);
            arguments.ActionOwnerId = Convert.ToInt32(values[4]);

            return arguments;
        }

        /// <summary>
        /// Write new entries into the Windows Registry
        /// </summary>
        private static void ModifySystemRegistry()
        {
            var learAP = ConfigurationManager.AppSettings["LearActionPlans"];
            var learConfirmation = ConfigurationManager.AppSettings["LearConfirmation"];

            try
            {
                Helper.RegisterMyProtocol("LearActionPlans", learAP);
                Helper.RegisterMyProtocol("LearAPConfirmation", learConfirmation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
