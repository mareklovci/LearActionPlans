using System;
using System.Collections.Generic;
using System.Linq;
using LearActionPlans.DataMappers;

namespace LearActionPlans.ViewModels
{
    public class SeznamPozadavkuViewModel
    {
        //zaměstnanec
        public int Id { get; set; }
        public string OdpovednaOsoba1 { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string PrihlasovaciJmeno { get; set; }
        public string Email { get; set; }
        public byte StavObjektu { get; set; }

        //oddělení
        public string Nazev { get; set; }

        //ukončení akce
        public int UkonceniAkceId { get; set; }
        public int AkceId { get; set; }
        public DateTime DatumUkonceni { get; set; }
        public string Poznamka { get; set; }
        public byte StavZadosti { get; set; }
        public byte StavObjektuUkonceniAkce { get; set; }

        //akce
        public int BodyAPId { get; set; }
        public string NapravnaOpatreni { get; set; }
        public int OdpovednaOsoba1Id { get; set; }
        public int? OdpovednaOsoba2Id { get; set; }
        public DateTime? KontrolaEfektivnosti { get; set; }
        public int? OddeleniId { get; set; }
        public string Priloha { get; set; }

        public static SeznamPozadavkuViewModel Zamestnanec(int id, string jmeno, string prihlasovaciJmeno, byte stavObjektu)
        {
            var seznamPozadavkuViewModel = new SeznamPozadavkuViewModel
            {
                Id = id,
                Jmeno = jmeno,
                PrihlasovaciJmeno = prihlasovaciJmeno,
                StavObjektu = stavObjektu
            };

            return seznamPozadavkuViewModel;
        }

        public static SeznamPozadavkuViewModel Oddeleni(int id, string nazev)
        {
            var seznamPozadavkuViewModel = new SeznamPozadavkuViewModel
            {
                Id = id,
                Nazev = nazev
            };

            return seznamPozadavkuViewModel;
        }

        public static SeznamPozadavkuViewModel UkonceniAkce(int ukonceniAkceId, int akceId, DateTime datumUkonceni, string Poznamka, byte stavZadosti, byte stavObjektuUkonceniAkce)
        {
            var seznamPozadavkuViewModel = new SeznamPozadavkuViewModel
            {
                UkonceniAkceId = ukonceniAkceId,
                AkceId = akceId,
                DatumUkonceni = datumUkonceni,
                Poznamka = Poznamka,
                StavObjektu = stavZadosti,
                StavObjektuUkonceniAkce = stavObjektuUkonceniAkce
            };

            return seznamPozadavkuViewModel;
        }

        public static SeznamPozadavkuViewModel AkceZadost(int bodyAPId, string napravnaOpatreni, int odpovednaOsobaId1, int? odpovednaOsobaId2, DateTime? kontrolaEfektivnosti, int? oddeleniId, string priloha)
        {
            var seznamPozadavkuViewModel = new SeznamPozadavkuViewModel
            {
                BodyAPId = bodyAPId,
                NapravnaOpatreni = napravnaOpatreni,
                OdpovednaOsoba1Id = odpovednaOsobaId1,
                OdpovednaOsoba2Id = odpovednaOsobaId2,
                KontrolaEfektivnosti = kontrolaEfektivnosti,
                OddeleniId = oddeleniId,
                Priloha = priloha
            };

            return seznamPozadavkuViewModel;
        }

        public static IEnumerable<SeznamPozadavkuViewModel> GetZamestnanciAll()
        {
            var zamestnanci = ZamestnanciDataMapper.GetZamestnanciAll().ToList();

            var query = from z in zamestnanci
                        select Zamestnanec(z.Id, z.Prijmeni + " " + z.Jmeno, z.PrihlasovaciJmeno, z.StavObjektu);

            //where z.Storno is false
            //where z.Storno = false

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<SeznamPozadavkuViewModel> GetOddeleniAll()
        {
            var oddeleni = OddeleniDataMapper.GetOddeleniAll().ToList();
            if (oddeleni[0] == null || oddeleni.Count() == 0)
            {
                //yield return null;
                yield break;
            }

            var query = from o in oddeleni
                        where o.StavObjektu == 1
                        select Oddeleni(o.Id, o.Nazev);

            foreach (var q in query)
            {
                yield return q;
            }
        }

        //to nebudu asi potřebovat
        //to asi pak vrátím ale opravím
        //public static IEnumerable<SeznamPozadavkuViewModel> GetUkonceniAkceZadost()
        //{
        //    var ukonceniAkce = BodAPDataMapper.GetUkonceniAkceZadost().ToList();

        //    if (ukonceniAkce == null || ukonceniAkce.Count() == 0)
        //    {
        //        yield break;
        //    }

        //    var query = from ua in ukonceniAkce
        //                where ua.StavZadosti == 3
        //                select UkonceniAkce(ua.Id, ua.AkceId, ua.DatumUkonceni, ua.Poznamka, ua.StavZadosti, ua.StavObjektuUkonceniAkce);

        //    foreach (var q in query)
        //    {
        //        yield return q;
        //    }
        //}

        //to asi pak vrátím ale opravím
        //public static IEnumerable<SeznamPozadavkuViewModel> GetAkceZadost()
        //{

        //    var ukonceniAkce = BodAPDataMapper.GetUkonceniAkceZadost().ToList();
        //    var akce = BodAPDataMapper.GetAkceAll().ToList();

        //    if (ukonceniAkce == null || ukonceniAkce.Count() == 0 || akce == null || akce.Count() == 0)
        //    {
        //        yield break;
        //    }

        //    var query = from ua in ukonceniAkce
        //                join a in akce
        //                    on ua.AkceId equals a.Id into gZadost
        //                from subZadost in gZadost.DefaultIfEmpty()
        //                where ua.StavZadosti == 3
        //                orderby subZadost.Id, ua.DatumUkonceni
        //                select AkceZadost(subZadost.BodAPId, subZadost.NapravnaOpatreni, subZadost.OdpovednaOsoba1Id, subZadost.OdpovednaOsoba2Id, subZadost.KontrolaEfektivnosti, subZadost.Oddeleni_Id, subZadost.Priloha);

        //    foreach (var q in query)
        //    {
        //        yield return q;
        //    }
        //}
    }
}
