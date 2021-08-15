// ReSharper disable IdentifierTypo
namespace LearActionPlans.Wpf.Models
{
    public partial class Zamestnanec
    {
        public static readonly Zamestnanec Null = new NullEmployee();
        
        private class NullEmployee : Zamestnanec
        {
            protected internal NullEmployee() => Jmeno = "(Select an Employee)";
        }
    }
}