// ReSharper disable IdentifierTypo
namespace LearActionPlans.Wpf.Models
{
    public partial class Projekt
    {
        public static readonly Projekt Null = new NullProject();

        private class NullProject : Projekt
        {
            protected internal NullProject() => Nazev = "(Select a Project)";
        }
    }
}