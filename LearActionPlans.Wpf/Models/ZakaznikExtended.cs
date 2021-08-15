// ReSharper disable IdentifierTypo
namespace LearActionPlans.Wpf.Models
{
    public partial class Zakaznik
    {
        public static readonly Zakaznik Null = new NullCustomer();
        
        private class NullCustomer : Zakaznik
        {
            protected internal NullCustomer() => Nazev = "(Select a Customer)";
        }
    }
}