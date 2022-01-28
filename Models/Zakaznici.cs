namespace LearActionPlans.Models
{
    public class Zakaznici : DatabaseTable
    {
        public sealed override int Id { get; set; }
        public string Nazev { get; set; }
        public byte StavObjektu { get; set; }
        public Zakaznici(int id, string nazev, byte stavObjektu)
        {
            Id = id;
            Nazev = nazev;
            StavObjektu = stavObjektu;
        }
    }
}
