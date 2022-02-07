using LearActionPlans.Views;
using System;
using Microsoft.Extensions.Configuration;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace LearActionPlans
{
    internal static partial class Program
    {
        private static IConfiguration config;
        private static string[] args;

        private const string JsonConfigurationFile = "appsettings.json";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // Assign Configuration
            var builder = new ConfigurationBuilder().AddJsonFile(JsonConfigurationFile, true, true);
            config = builder.Build();

            // Get Arguments
            args = Environment.GetCommandLineArgs();

            // Parse Arguments
            var arguments = ParseArguments();

            // Initialize Dependency Injection
            var services = new ServiceCollection();
            ConfigureServices(services, arguments);

            // Modify System Registry
            if (arguments.RunWithoutParameters)
            {
                ModifySystemRegistry();
            }

            // Initialize Application
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Run Application
            using var serviceProvider = services.BuildServiceProvider();
            var formMain = serviceProvider.GetRequiredService<FormMain>();
            Application.Run(formMain);
        }
    }
}
