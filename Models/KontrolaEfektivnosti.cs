using System;
using System.Collections.Generic;


namespace LearActionPlans.Models
{
    public class KontrolaEfektivnosti : DatabaseTable
    {
        public sealed override int Id { get; set; }
        public DateTime PuvodniDatum { get; set; }
        public DateTime OdstranitDatum { get; set; }
        public string Poznamka { get; set; }

        public KontrolaEfektivnosti(DateTime puvodniDatum, DateTime odstranitDatum, string poznamka)
        {
            PuvodniDatum = puvodniDatum;
            OdstranitDatum = odstranitDatum;
            Poznamka = poznamka;
        }
    }
}
