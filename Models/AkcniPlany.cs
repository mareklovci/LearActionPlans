using System;

namespace LearActionPlans.Models
{
    public class AkcniPlany : DatabaseTable
    {
        public sealed override int Id { get; set; }
        public DateTime DatumZalozeni { get; set; }
        public int CisloAP { get; set; }
        //public string CisloAPRok { get; set; }
        public int Zadavatel1Id { get; set; }
        public int? Zadavatel2Id { get; set; }
        //public string Zadavatel1Jmeno { get; set; }
        //public string Zadavatel2Jmeno { get; set; }
        public string Tema { get; set; }
        public int? ProjektId { get; set; }
        //public string ProjektNazev { get; set; }
        //public DateTime DatumUkonceni { get; set; }
        public int ZakaznikId { get; set; }
        //public string ZakaznikNazev { get; set; }
        public byte TypAP { get; set; }
        //public string Poznamka { get; set; }
        public byte ZmenaTerminu { get; set; }
        public byte ZnovuOtevrit { get; set; }
        public DateTime? UzavreniAP { get; set; }
        public string DuvodZnovuotevreni { get; set; }
        public byte StavObjektu { get; set; }
        //public DateTime? DatumUzavreni { get; set; }

        public AkcniPlany(int apId, byte znovuOtevrit, DateTime? uzavreniAP, string duvod)
        {
            this.Id = apId;
            this.ZnovuOtevrit = znovuOtevrit;
            this.UzavreniAP = uzavreniAP;
            this.DuvodZnovuotevreni = duvod;
        }

        public AkcniPlany(int akcniPlanId, byte zmenaTerminu)
        {
            this.Id = akcniPlanId;
            this.ZmenaTerminu = zmenaTerminu;
        }

        public AkcniPlany()
        {
            this.DatumZalozeni = DateTime.Now;
            this.CisloAP = 0;
            this.Zadavatel1Id = 0;
            this.Zadavatel2Id = null;
            this.Tema = null;
            this.ProjektId = null;
            //DatumUkonceni = DateTime.Now;
            this.ZakaznikId = 0;
            this.TypAP = 1;
            //Poznamka = null;
            this.StavObjektu = 1;
        }

        public AkcniPlany(int id, DateTime datumZalozeni, int cisloAP, int zadavatel1Id, int? zadavatel2Id, string tema, int? projektId,
            int zakaznikId, byte typAP, byte stavObjektu, DateTime? datumUzavreni)
        {
            this.Id = id;
            this.DatumZalozeni = datumZalozeni;
            this.CisloAP = cisloAP;
            this.Zadavatel1Id = zadavatel1Id;
            this.Zadavatel2Id = zadavatel2Id;
            this.Tema = tema;
            this.ZakaznikId = zakaznikId;
            this.ProjektId = projektId;
            this.TypAP = typAP;
            this.StavObjektu = stavObjektu;
            this.UzavreniAP = datumUzavreni;
        }
    }
}
