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
            Id = apId;
            ZnovuOtevrit = znovuOtevrit;
            UzavreniAP = uzavreniAP;
            DuvodZnovuotevreni = duvod;
        }

        public AkcniPlany(int akcniPlanId, byte zmenaTerminu)
        {
            Id = akcniPlanId;
            ZmenaTerminu = zmenaTerminu;
        }

        public AkcniPlany()
        {
            DatumZalozeni = DateTime.Now;
            CisloAP = 0;
            Zadavatel1Id = 0;
            Zadavatel2Id = null;
            Tema = null;
            ProjektId = null;
            //DatumUkonceni = DateTime.Now;
            ZakaznikId = 0;
            TypAP = 1;
            //Poznamka = null;
            StavObjektu = 1;
        }

        public AkcniPlany(int id, DateTime datumZalozeni, int cisloAP, int zadavatel1Id, int? zadavatel2Id, string tema, int? projektId,
            int zakaznikId, byte typAP, byte stavObjektu, DateTime? datumUzavreni)
        {
            Id = id;
            DatumZalozeni = datumZalozeni;
            CisloAP = cisloAP;
            Zadavatel1Id = zadavatel1Id;
            Zadavatel2Id = zadavatel2Id;
            Tema = tema;
            ZakaznikId = zakaznikId;
            ProjektId = projektId;
            TypAP = typAP;
            StavObjektu = stavObjektu;
            UzavreniAP = datumUzavreni;
        }
    }
}
