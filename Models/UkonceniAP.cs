using System;

namespace LearActionPlans.Models
{    public class UkonceniAP : DatabaseTable
    {
        public sealed override int Id { get; set; }
        public int APId { get; set; }
        public DateTime DatumUkonceni { get; set; }
        public string Poznamka { get; set; }

        public UkonceniAP(int id, int apId, DateTime datumUkonceni, string poznamka)
        {
            this.Id = id;
            this.APId = apId;
            this.DatumUkonceni = datumUkonceni;
            this.Poznamka = poznamka;
        }
    }
}
