using System;

namespace LearActionPlans.ViewModels
{
    public class VsechnyBodyAPViewModel
    {
        //AP
        public DateTime DatumZalozeniAP { get; set; }
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
        public byte StavObjektuAP { get; set; }

        //body AP
        public int IdBodAP { get; set; }
        public int IdAP { get; set; }
        public int CisloBoduAP { get; set; }
        public DateTime DatumZalozeniBodAP { get; set; }
        public string OdkazNaNormu { get; set; }
        public string HodnoceniNeshody { get; set; }
        public string PopisProblemu { get; set; }
        public string OdpovednaOsoba1 { get; set; }

        public string SkutecnaPricinaWM { get; set; }
        public byte StavObjektuBodAP { get; set; }

        public static VsechnyBodyAPViewModel BodyAP(DateTime datumZalozeniAP, int cisloAP, int idBodAP, int idAP,
            int cisloBoduAP, DateTime datumZalozeni, string odkazNaNormu,
            string hodnoceniNeshody, string popisProblemu, string odpovednaOsoba1, string skurecnaPricinaWM,
            byte stavObjektuBodAP)
        {
            var vsechnyBodyAPViewModel = new VsechnyBodyAPViewModel
            {
                DatumZalozeniAP = datumZalozeniAP,
                CisloAP = cisloAP,
                IdBodAP = idBodAP,
                IdAP = idAP,
                CisloBoduAP = cisloBoduAP,
                DatumZalozeniBodAP = datumZalozeni,
                OdkazNaNormu = odkazNaNormu,
                HodnoceniNeshody = hodnoceniNeshody,
                PopisProblemu = popisProblemu,
                OdpovednaOsoba1 = odpovednaOsoba1,
                SkutecnaPricinaWM = skurecnaPricinaWM,
                StavObjektuBodAP = stavObjektuBodAP
            };
            return vsechnyBodyAPViewModel;
        }

        public static VsechnyBodyAPViewModel AP(int zadavatel1Id, int? zadavatel2Id,
            string zadavatel1Jmeno, string tema, int? projektId, string projekt,
            int zakaznikId, string zakaznik, byte typAP, byte stavObjektuAP)
        {
            var vsechnyBodyAPViewModel = new VsechnyBodyAPViewModel
            {
                Zadavatel1Id = zadavatel1Id,
                Zadavatel2Id = zadavatel2Id,
                Zadavatel1 = zadavatel1Jmeno,
                Tema = tema,
                ProjektId = projektId,
                Projekt = projekt,
                ZakaznikId = zakaznikId,
                Zakaznik = zakaznik,
                TypAP = typAP,
                StavObjektuAP = stavObjektuAP
            };
            return vsechnyBodyAPViewModel;
        }
    }
}
