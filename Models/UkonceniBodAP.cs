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


        public UkonceniBodAP(DateTime? kontrolaEfektivnosti) => this.KontrolaEfektivnosti = kontrolaEfektivnosti;

        public UkonceniBodAP(int id, int bodAPId, DateTime datumUkonceni, string poznamka, string odpoved,  byte stavZadosti, byte stavObjektu, bool datUkonBodAPUlozen)
        {
            this.Id = id;
            this.BodAPId = bodAPId;
            this.DatumUkonceni = datumUkonceni;
            this.Poznamka = poznamka;
            this.Odpoved = odpoved;
            this.StavZadosti = stavZadosti;
            this.StavObjektuUkonceni = stavObjektu;
            this.DatUkonBodAPUlozen = datUkonBodAPUlozen;
        }

        public UkonceniBodAP(int id, int bodAPId, DateTime datumUkonceni, string poznamka, string odpoved, byte stavZadosti, byte stavObjektu)
        {
            this.Id = id;
            this.BodAPId = bodAPId;
            this.DatumUkonceni = datumUkonceni;
            this.Poznamka = poznamka;
            this.Odpoved = odpoved;
            this.StavZadosti = stavZadosti;
            this.StavObjektuUkonceni = stavObjektu;
        }

        //konstruktor pro ukládání datumu ukončení
        public UkonceniBodAP(DateTime datumUkonceni, string poznamka, string odpoved, byte stavZadosti, bool datUkonAkceUlozena)
        {
            this.DatumUkonceni = datumUkonceni;
            this.Poznamka = poznamka;
            this.Odpoved = odpoved;
            this.StavZadosti = stavZadosti;
            //DatUkonAkceUlozen = datUkonAkceUlozena;
        }
    }
}
