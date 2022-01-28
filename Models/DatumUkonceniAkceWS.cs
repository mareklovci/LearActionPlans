using System;

namespace LearActionPlans.Models
{
    public class DatumUkonceniAkceWS : DatabaseTable
    {
        public sealed override int Id { get; set; }
        public int BodAPAudityOstatniId { get; set; }
        public DateTime DatumUkonceni { get; set; }
        public string Poznamka { get; set; }

        public DatumUkonceniAkceWS()
        {
        }
        public DatumUkonceniAkceWS(DateTime datumUkonceni, string poznamka)
        {
            DatumUkonceni = datumUkonceni;
            Poznamka = poznamka;
        }

    }
}
