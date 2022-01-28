﻿using System;
using System.Collections.Generic;
using System.Linq;

using LearActionPlans.DataMappers;

namespace LearActionPlans.ViewModels
{
    public class PrehledAPViewModel
    {
        //AP
        public int Id { get; set; }
        public DateTime DatumZalozeni { get; set; }
        public int Rok { get; set; }
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
        public byte StavObjektu { get; set; }
        public DateTime? DatumUzavreni { get; set; }

        //bodAP
        public int BodAPId { get; set; }

        //Akce
        public int AkceAPId { get; set; }
        public int Zadavatel1IdAkce { get; set; }
        public int? Zadavatel2IdAkce { get; set; }


        public PrehledAPViewModel(int zad2Id, string zadavatel2Jmeno)
        {
            Zadavatel2Id = zad2Id;
            Zadavatel2 = zadavatel2Jmeno;
        }

        public PrehledAPViewModel(int akceAPId, int zadavatel1IdAkce, int? zadavatel2IdAkce)
        {
            AkceAPId = akceAPId;
            Zadavatel1IdAkce = zadavatel1IdAkce;
            Zadavatel2IdAkce = zadavatel2IdAkce;
        }

        public PrehledAPViewModel(int id, DateTime datumZalozeni, int cisloAP, int zadavatel1Id, int? zadavatel2Id,
            string zadavatel1Jmeno, string tema, int? projektId, string projekt,
            int zakaznikId, string zakaznik, byte typAP, byte stavObjektu, DateTime? datumUzavreni)
        {
            Id = id;
            DatumZalozeni = datumZalozeni;
            CisloAP = cisloAP;
            Zadavatel1Id = zadavatel1Id;
            Zadavatel2Id = zadavatel2Id;
            Zadavatel1 = zadavatel1Jmeno;
            Tema = tema;
            ProjektId = projektId;
            Projekt = projekt;
            ZakaznikId = zakaznikId;
            Zakaznik = zakaznik;
            TypAP = typAP;
            StavObjektu = stavObjektu;
            DatumUzavreni = datumUzavreni;
        }

        public static IEnumerable<PrehledAPViewModel> GetAPAll()
        {
            var akcniPlany = AkcniPlanyDataMapper.GetAPAll().ToList();
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
                        where ap.StavObjektu == 1
                        orderby ap.DatumZalozeni.Year ascending, ap.CisloAP ascending
                        select new PrehledAPViewModel(ap.Id, ap.DatumZalozeni, ap.CisloAP, ap.Zadavatel1Id, ap?.Zadavatel2Id, subZam.Prijmeni + " " + subZam.Jmeno, ap.Tema, ap?.ProjektId,
                            subPro?.Nazev ?? string.Empty, ap.ZakaznikId, subZak.Nazev, ap.TypAP, ap.StavObjektu, ap.UzavreniAP);

            //where z.Storno = false

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<PrehledAPViewModel> GetZadavatel2()
        {
            var zamestnanci = ZamestnanciDataMapper.GetZamestnanciAll().ToList();

            var query = from zam in zamestnanci
                        select new PrehledAPViewModel(zam.Id, zam.Prijmeni + " " + zam.Jmeno);

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public PrehledAPViewModel(int bodAPId)
        {
            BodAPId = bodAPId;
        }

        public static IEnumerable<PrehledAPViewModel> GetBodyAPId(int idAP)
        {
            var bodyAP = BodAPDataMapper.GetBodyIdAP(idAP).ToList();

            if (bodyAP == null || bodyAP.Count() == 0)
            {
                yield break;
            }

            var query = from b in bodyAP
                        select new PrehledAPViewModel(b.Id);

            foreach (var q in query)
            {
                yield return q;
            }
        }

        //public static IEnumerable<PrehledAPViewModel> GetAkceBodId(int idBody)
        //{
        //    var akce = BodAPDataMapper.GetAkceBodyId(idBody).ToList();

        //    if (akce == null || akce.Count() == 0)
        //    {
        //        yield break;
        //    }

        //    var query = from a in akce
        //                where a.StavObjektuAkce == 1
        //                select new PrehledAPViewModel(a.Id, a.OdpovednaOsoba1Id, a.OdpovednaOsoba2Id);

        //    foreach (var q in query)
        //    {
        //        yield return q;
        //    }
        //}
    }
}
