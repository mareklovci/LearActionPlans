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
        /// <param name="argumentOptions">Arguments to be registered and globally accessible</param>
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

            // Initialize MVVM
            RegisterForms(services);
            RegisterRepositories(services);
            RegisterViewModels(services);
        }

        private static void RegisterForms(IServiceCollection services)
        {
            services.AddScoped<FormMain>();
            services.AddScoped<FormAdmin>();
            services.AddScoped<FormDatumUkonceni>();
            services.AddScoped<FormEditAP>();
            services.AddScoped<FormKontrolaEfektivnosti>();
            services.AddScoped<FormNovyAkcniPlan>();
            services.AddScoped<FormPosunutiTerminuBodAP>();
            services.AddScoped<FormPrehledAp>();
            services.AddScoped<FormPrehledBoduAP>();
            services.AddScoped<FormAttachment>();
            services.AddScoped<FormSeznamPozadavku>();
            services.AddScoped<FormVsechnyBodyAP>();
            services.AddScoped<FormZadaniBoduAP>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddSingleton<ActionRepository>();
            services.AddSingleton<EmployeeRepository>();
            services.AddSingleton<DepartmentRepository>();
        }

        private static void RegisterViewModels(IServiceCollection services)
        {
            // services.AddSingleton<AdminViewModel>();
        }
    }
}
