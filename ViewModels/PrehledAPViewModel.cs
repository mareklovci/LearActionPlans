using System;

namespace LearActionPlans.ViewModels
{
    public class PrehledAPViewModel
    {
        //AP
        public int Id { get; set; }
        public DateTime DatumZalozeni { get; set; }
        public int CisloAP { get; set; }
        public int Zadavatel1Id { get; set; }
        public int? Zadavatel2Id { get; set; }
        public string Zadavatel1 { get; set; }
        public string Tema { get; set; }
        public int? ProjektId { get; set; }
        public string Projekt { get; set; }
        public int ZakaznikId { get; set; }
        public string Zakaznik { get; set; }
        public byte TypAP { get; set; }
        public byte StavObjektu { get; set; }
        public DateTime? DatumUzavreni { get; set; }

        public PrehledAPViewModel(int id, DateTime datumZalozeni, int cisloAP, int zadavatel1Id, int? zadavatel2Id,
            string zadavatel1Jmeno, string tema, int? projektId, string projekt,
            int zakaznikId, string zakaznik, byte typAP, byte stavObjektu, DateTime? datumUzavreni)
        {
            this.Id = id;
            this.DatumZalozeni = datumZalozeni;
            this.CisloAP = cisloAP;
            this.Zadavatel1Id = zadavatel1Id;
            this.Zadavatel2Id = zadavatel2Id;
            this.Zadavatel1 = zadavatel1Jmeno;
            this.Tema = tema;
            this.ProjektId = projektId;
            this.Projekt = projekt;
            this.ZakaznikId = zakaznikId;
            this.Zakaznik = zakaznik;
            this.TypAP = typAP;
            this.StavObjektu = stavObjektu;
            this.DatumUzavreni = datumUzavreni;
        }
    }
}
