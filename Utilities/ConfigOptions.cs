using Microsoft.Extensions.Configuration;

namespace LearActionPlans.Utilities
{
    public static class ConfigOptions
    {
        public const string ConnectionStrings = "ConnectionStrings";
        public const string Components = "Components";
        public const string Smtp = "SMTP";
        public const string Privileges = "Privileges";
    }

    public class ConnectionStringsOptions
    {
        public string LearDataAll { get; set; }
    }

    public class ComponentsOptions
    {
        public string LearActionPlans { get; set; }
        public string LearConfirmation { get; set; }
    }

    public class SmtpOptions
    {
        public string Server { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Sender { get; set; }
    }

    public class PrivilegesOptions
    {
        public bool AdminElevation { get; set; }
        public bool RegisterWrite { get; set; }
    }
}
