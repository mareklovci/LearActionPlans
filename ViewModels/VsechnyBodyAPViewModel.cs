using System;
using System.Collections.Generic;
using System.Linq;

using LearActionPlans.DataMappers;
using LearActionPlans.Models;

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
        public DateTime? KontrolaEfektivnosti { get; set; }

        public string SkutecnaPricinaWM { get; set; }
        public string NapravnaOpatreniWM { get; set; }
        public string SkutecnaPricinaWS { get; set; }
        public string NapravnaOpatreniWS { get; set; }
        public bool ZnovuOtevrit { get; set; }
        public byte StavObjektuBodAP { get; set; }


        // oddělení
        public string NazevOddeleni { get; set; }

        public static VsechnyBodyAPViewModel BodyAP(DateTime datumZalozeniAP, int cisloAP, int idBodAP, int idAP, int cisloBoduAP,
            DateTime datumZalozeni, string odkazNaNormu, string hodnoceniNeshody, string popisProblemu,
            string odpovednaOsoba1, int? odpovednaOsoba2Id, string nazevOddeleni,
            string skurecnaPricinaWM, string napravnaOpatreniWM, string skurecnaPricinaWS, string napravnaOpatreniWS,
            DateTime? kontrolaEfektivnosti, bool znovuOtevrit, byte stavObjektuBodAP)
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
                OdpovednaOsoba2Id = odpovednaOsoba2Id,
                NazevOddeleni = nazevOddeleni,
                SkutecnaPricinaWM = skurecnaPricinaWM,
                NapravnaOpatreniWM = napravnaOpatreniWM,
                SkutecnaPricinaWS = skurecnaPricinaWS,
                NapravnaOpatreniWS = napravnaOpatreniWS,
                KontrolaEfektivnosti = kontrolaEfektivnosti,
                ZnovuOtevrit = znovuOtevrit,
                StavObjektuBodAP = stavObjektuBodAP
            };
            return vsechnyBodyAPViewModel;
        }

        //to asi pak vrátím ale opravím
        public static IEnumerable<VsechnyBodyAPViewModel> GetBodyAPAll()
        {
            var bodyAP = BodAPDataMapper.GetBodyAPAll().ToList();
            var akcniPlany = AkcniPlanyDataMapper.GetAPAll().ToList();
            var zamestnanci = ZamestnanciDataMapper.GetZamestnanciAll().ToList();
            var oddeleni = OddeleniDataMapper.GetOddeleniAll();

            if (bodyAP == null || bodyAP.Count() == 0 || akcniPlany == null || akcniPlany.Count() == 0)
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
                        join odd in oddeleni
                            on b.OddeleniId equals odd.Id into gOdd
                        from subOdd in gOdd.DefaultIfEmpty()
                        orderby b.AkcniPlanId, b.CisloBoduAP
                        select BodyAP(subAP.DatumZalozeni, subAP.CisloAP, b.Id, b.AkcniPlanId, b.CisloBoduAP, b.DatumZalozeni, b.OdkazNaNormu, b.HodnoceniNeshody, 
                        b.PopisProblemu, subZam.Prijmeni + " " + subZam.Jmeno, b.OdpovednaOsoba2Id, subOdd.Nazev,
                        b.SkutecnaPricinaWM,b.NapravnaOpatreniWM, b.SkutecnaPricinaWS, b.NapravnaOpatreniWS,
                        b.KontrolaEfektivnosti, b.ZnovuOtevrit, b.StavObjektu);

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
            var zamestnanci = ZamestnanciDataMapper.GetZamestnanciAll().ToList();
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
                        select AP(ap.Zadavatel1Id, ap?.Zadavatel2Id, subZam.Prijmeni + " " + subZam.Jmeno, ap.Tema, ap?.ProjektId,
                            subPro?.Nazev ?? string.Empty, ap.ZakaznikId, subZak.Nazev, ap.TypAP, ap.StavObjektu);

            //where z.Storno = false

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static PrehledBoduAPViewModel NacistJmenoOdpOsoba2(int odpOsoba2Id, string odpOsoba2Jmeno)
        {
            var prehledBoduAPViewModel = new PrehledBoduAPViewModel
            {
                OdpovednaOsoba2Id = odpOsoba2Id,
                OdpovednaOsoba2 = odpOsoba2Jmeno
            };

            return prehledBoduAPViewModel;
        }

        public static IEnumerable<PrehledBoduAPViewModel> GetOdpovednaOsoba2()
        {
            var zamestnanci = ZamestnanciDataMapper.GetZamestnanciAll().ToList();

            var query = from zam in zamestnanci
                        select NacistJmenoOdpOsoba2(zam.Id, zam.Prijmeni + " " + zam.Jmeno);

            foreach (var q in query)
            {
                yield return q;
            }
        }
    }
}
