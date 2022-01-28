namespace LearActionPlans.Models
{
    public class Projekty : DatabaseTable
    {
        public sealed override int Id { get; set; }
        public string Nazev { get; set; }
        public byte StavObjektu { get; set; }

        public Projekty(int id, string nazev, byte stavObjektu)
        {
            Id = id;
            Nazev = nazev;
            StavObjektu = stavObjektu;
        }
    }
}
