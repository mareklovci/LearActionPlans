using System;

namespace LearActionPlans.Models
{
    public class UkonceniBodAP : DatabaseTable
    {
        public sealed override int Id { get; set; }
        public int BodAPId { get; set; }
        public DateTime DatumUkonceni { get; set; }
        public string Poznamka { get; set; }
        public string Odpoved { get; set; }
        public byte StavZadosti { get; set; }
        public byte StavObjektuUkonceni { get; set; }
        public bool DatUkonBodAPUlozen { get; set; }
        public DateTime? KontrolaEfektivnosti { get; set; }
        

        public UkonceniBodAP(DateTime? kontrolaEfektivnosti)
        {
            KontrolaEfektivnosti = kontrolaEfektivnosti;
        }

        public UkonceniBodAP(int id, int bodAPId, DateTime datumUkonceni, string poznamka, string odpoved,  byte stavZadosti, byte stavObjektu, bool datUkonBodAPUlozen)
        {
            Id = id;
            BodAPId = bodAPId;
            DatumUkonceni = datumUkonceni;
            Poznamka = poznamka;
            Odpoved = odpoved;
            StavZadosti = stavZadosti;
            StavObjektuUkonceni = stavObjektu;
            DatUkonBodAPUlozen = datUkonBodAPUlozen;
        }

        public UkonceniBodAP(int id, int bodAPId, DateTime datumUkonceni, string poznamka, string odpoved, byte stavZadosti, byte stavObjektu)
        {
            Id = id;
            BodAPId = bodAPId;
            DatumUkonceni = datumUkonceni;
            Poznamka = poznamka;
            Odpoved = odpoved;
            StavZadosti = stavZadosti;
            StavObjektuUkonceni = stavObjektu;
        }

        //konstruktor pro ukládání datumu ukončení
        public UkonceniBodAP(DateTime datumUkonceni, string poznamka, string odpoved, byte stavZadosti, bool datUkonAkceUlozena)
        {
            DatumUkonceni = datumUkonceni;
            Poznamka = poznamka;
            Odpoved = odpoved;
            StavZadosti = stavZadosti;
            //DatUkonAkceUlozen = datUkonAkceUlozena;
        }
    }
}
