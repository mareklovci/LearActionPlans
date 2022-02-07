using LearActionPlans.DataMappers;
using LearActionPlans.Utilities;
using LearActionPlans.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LearActionPlans
{
    internal static partial class Program
    {
        /// <summary>
        /// Register Dependency Injection services
        /// </summary>
        /// <param name="services">ServiceCollection</param>
        private static void ConfigureServices(IServiceCollection services, ArgumentOptions argumentOptions)
        {
            // Initialize Connection Strings
            var connectionStringsOptions = new ConnectionStringsOptions();
            config.GetSection(ConfigOptions.ConnectionStrings).Bind(connectionStringsOptions);

            // Initialize Components
            var componentsOptions = new ComponentsOptions();
            config.GetSection(ConfigOptions.Components).Bind(componentsOptions);

            // Initialize SMTP Connection
            var smtpOptions = new SmtpOptions();
            config.GetSection(ConfigOptions.Smtp).Bind(smtpOptions);

            // Initialize Privileges
            var privilegesOptions = new PrivilegesOptions();
            config.GetSection(ConfigOptions.Privileges).Bind(privilegesOptions);

            // Initialize Configuration
            services.AddSingleton(connectionStringsOptions);
            services.AddSingleton(componentsOptions);
            services.AddSingleton(smtpOptions);
            services.AddSingleton(privilegesOptions);

            // Initialize Arguments
            services.AddSingleton(argumentOptions);

            // Initialize Forms
            services.AddScoped<FormMain>();
            services.AddScoped<FormAdmin>();
            // services.AddScoped<FormDatumUkonceni>();
            // services.AddScoped<FormEditAP>();
            // services.AddScoped<FormFiltry>();
            // services.AddScoped<FormKontrolaEfektivnosti>();
            services.AddScoped<FormNovyAkcniPlan>();
            // services.AddScoped<FormOvereniUzivatele>();
            // services.AddScoped<FormPosunutiTerminuBodAP>();
            services.AddScoped<FormPrehledAP>();
            // services.AddScoped<FormPrehledBoduAP>();
            // services.AddScoped<FormPriloha>();
            // services.AddScoped<FormSeznamPozadavku>();
            // services.AddScoped<FormVsechnyBodyAP>();
            // services.AddScoped<FormZadaniBoduAP>();

            // Initialize Repositories
            services.AddSingleton<ActionRepository>();
            services.AddSingleton<EmployeeRepository>();
        }
    }
}
