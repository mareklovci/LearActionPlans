using System;
using System.Collections.Generic;
using System.Linq;
using LearActionPlans.DataMappers;

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
        public string Zadavatel2 { get; set; }
        public string Tema { get; set; }
        public int? ProjektId { get; set; }
        public string Projekt { get; set; }
        public int ZakaznikId { get; set; }
        public string Zakaznik { get; set; }
        public byte TypAP { get; set; }
        public byte StavObjektuAP { get; set; }
        public DateTime DatumUkonceni { get; set; }


        //body AP
        public int IdBodAP { get; set; }
        public int IdAP { get; set; }
        public int CisloBoduAP { get; set; }
        public DateTime DatumZalozeniBodAP { get; set; }
        public string OdkazNaNormu { get; set; }
        public string HodnoceniNeshody { get; set; }
        public string PopisProblemu { get; set; }
        public int OdpovednaOsoba1Id { get; set; }
        public int? OdpovednaOsoba2Id { get; set; }
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

        //to asi pak vrátím ale opravím
        public static IEnumerable<VsechnyBodyAPViewModel> GetBodyAPAll()
        {
            var bodyAP = BodAPDataMapper.GetBodyAPAll().ToList();
            var akcniPlany = AkcniPlanyDataMapper.GetAPAll().ToList();
            var zamestnanci = EmployeeRepository.GetZamestnanciAll().ToList();

            if (!bodyAP.Any() || !akcniPlany.Any())
            {
                yield break;
            }

            var query = from b in bodyAP
                join ap in akcniPlany
                    on b.AkcniPlanId equals ap.Id into gAP
                from subAP in gAP.DefaultIfEmpty()
                join zam in zamestnanci
                    on b.OdpovednaOsoba1Id equals zam.Id into gZam
                from subZam in gZam.DefaultIfEmpty()
                orderby b.AkcniPlanId, b.CisloBoduAP
                select BodyAP(subAP.DatumZalozeni, subAP.CisloAP, b.Id, b.AkcniPlanId, b.CisloBoduAP, b.DatumZalozeni,
                    b.OdkazNaNormu, b.HodnoceniNeshody,
                    b.PopisProblemu, subZam.Prijmeni + " " + subZam.Jmeno, b.SkutecnaPricinaWM, b.StavObjektu);

            foreach (var q in query)
            {
                yield return q;
            }
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

        public static IEnumerable<VsechnyBodyAPViewModel> GetSelectedAP(int idAP)
        {
            var akcniPlany = AkcniPlanyDataMapper.GetAPId(idAP).ToList();
            var zamestnanci = EmployeeRepository.GetZamestnanciAll().ToList();
            var projekty = ProjektyDataMapper.GetProjektyAll().ToList();
            var zakaznici = ZakazniciDataMapper.GetZakazniciAll().ToList();

            var query = from ap in akcniPlany
                join zam in zamestnanci
                    on ap.Zadavatel1Id equals zam.Id into gZam
                from subZam in gZam.DefaultIfEmpty()
                join pro in projekty
                    on ap.ProjektId equals pro.Id into gPro
                from subPro in gPro.DefaultIfEmpty()
                join zak in zakaznici
                    on ap.ZakaznikId equals zak.Id into gZak
                from subZak in gZak.DefaultIfEmpty()
                select AP(ap.Zadavatel1Id, ap?.Zadavatel2Id, subZam.Prijmeni + " " + subZam.Jmeno, ap.Tema,
                    ap?.ProjektId,
                    subPro?.Nazev ?? string.Empty, ap.ZakaznikId, subZak.Nazev, ap.TypAP, ap.StavObjektu);

            foreach (var q in query)
            {
                yield return q;
            }
        }
    }
}
