namespace LearActionPlans.Models
{
    public class Oddeleni : DatabaseTable
    {
        public sealed override int Id { get; set; }
        public string Nazev { get; set; }
        public byte StavObjektu { get; set; }

        public Oddeleni(int oddeleniId, string nazev, byte stavObjektu)
        {
            this.Id = oddeleniId;
            this.Nazev = nazev;
            this.StavObjektu = stavObjektu;
        }
    }
}
