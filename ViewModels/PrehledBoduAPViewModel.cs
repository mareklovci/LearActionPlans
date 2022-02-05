using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LearActionPlans.DataMappers;
using LearActionPlans.Models;

namespace LearActionPlans.ViewModels
{
    public class PrehledBoduAPViewModel
    {
        //AP
        public int APId { get; set; }
        public DateTime DatumUkonceniAP { get; set; }

        //body AP
        public int Id { get; set; }
        public int IdAP { get; set; }
        public int BodAPId { get; set; }
        public int CisloBoduAP { get; set; }
        public DateTime DatumZalozeni { get; set; }
        public string OdkazNaNormu { get; set; }
        public string HodnoceniNeshody { get; set; }
        public string PopisProblemu { get; set; }
        public string SkutecnaPricinaWM { get; set; }
        public string NapravnaOpatreniWM { get; set; }
        public string SkutecnaPricinaWS { get; set; }
        public string NapravnaOpatreniWS { get; set; }
        public int OdpovednaOsoba1Id { get; set; }
        public int? OdpovednaOsoba2Id { get; set; }
        public string OdpovednaOsoba1 { get; set; }
        public string OdpovednaOsoba2 { get; set; }
        public DateTime? KontrolaEfektivnosti { get; set; }
        public int OddeleniId { get; set; }
        public string Oddeleni { get; set; }
        public string Priloha { get; set; }
        public byte ZamitnutiTerminu { get; set; }
        public byte ZmenaTerminu { get; set; }
        public bool ZnovuOtevrit { get; set; }
        public bool EmailOdeslan { get; set; }
        public byte StavObjektuBodAP { get; set; }

        //ukončení bod AP
        public DateTime DatumUkonceni { get; set; }
        public string Poznamka { get; set; }
        public string Odpoved { get; set; }
        public byte StavZadosti { get; set; }
        public byte StavObjektuUkonceni { get; set; }

        // zaměstnanec
        public string JmenoPracovnika { get; set; }
        public string EmailOdpovednyPracovnik { get; set; }


        public static PrehledBoduAPViewModel NacistJmenoOdpOsoba2(int odpOsoba2Id, string odpOsoba2Jmeno)
        {
            var prehledBoduAPViewModel = new PrehledBoduAPViewModel
            {
                OdpovednaOsoba2Id = odpOsoba2Id,
                OdpovednaOsoba2 = odpOsoba2Jmeno
            };

            return prehledBoduAPViewModel;
        }

        public static PrehledBoduAPViewModel BodyAP(int id, int idAP, int cisloBoduAP, DateTime datumZalozeni, string odkazNaNormu,
            string hodnoceniNeshody, string popisProblemu,
            string skutecnaPricinaWM, string napravnaOpatreniWM, string skutecnaPricinaWS, string napravnaOpatreniWS,
            int odpovednaOsoba1Id, int? odpovednaOsoba2Id, string odpovednaOsoba1, DateTime? kontrolaEfektivnosti, int oddeleniId, string oddeleni, string priloha,
            byte zamitnutiTerminu, byte zmenaTerminu, bool znovuOtevrit, bool emailOdeslan, byte stavObjektuBodAP)
        {
            var prehledBoduAPViewModel = new PrehledBoduAPViewModel
            {
                Id = id,
                IdAP = idAP,
                CisloBoduAP = cisloBoduAP,
                DatumZalozeni = datumZalozeni,
                OdkazNaNormu = odkazNaNormu,
                HodnoceniNeshody = hodnoceniNeshody,
                PopisProblemu = popisProblemu,
                SkutecnaPricinaWM = skutecnaPricinaWM,
                NapravnaOpatreniWM = napravnaOpatreniWM,
                SkutecnaPricinaWS = skutecnaPricinaWS,
                NapravnaOpatreniWS = napravnaOpatreniWS,
                OdpovednaOsoba1Id = odpovednaOsoba1Id,
                OdpovednaOsoba2Id = odpovednaOsoba2Id,
                OdpovednaOsoba1 = odpovednaOsoba1,
                KontrolaEfektivnosti = kontrolaEfektivnosti,
                OddeleniId = oddeleniId,
                Oddeleni = oddeleni,
                Priloha = priloha,
                ZamitnutiTerminu = zamitnutiTerminu,
                ZmenaTerminu = zmenaTerminu,
                ZnovuOtevrit = znovuOtevrit,
                EmailOdeslan = emailOdeslan,
                StavObjektuBodAP = stavObjektuBodAP
            };
            return prehledBoduAPViewModel;
        }

        public static PrehledBoduAPViewModel UkonceniBodAP(int id, int bodAPId, DateTime datumUkonceni, string poznamka, string odpoved, byte stavZadosti, byte stavObjektuUkonceni)
        {
            var prehledBoduAPViewModel = new PrehledBoduAPViewModel
            {
                Id = id,
                BodAPId = bodAPId,
                DatumUkonceni = datumUkonceni,
                Poznamka = poznamka,
                Odpoved = odpoved,
                StavZadosti = stavZadosti,
                StavObjektuUkonceni = stavObjektuUkonceni
            };

            return prehledBoduAPViewModel;
        }

        public static PrehledBoduAPViewModel UkonceniAP(int id, DateTime datumUkonceniAP)
        {
            var prehledBoduAPViewModel = new PrehledBoduAPViewModel
            {
                Id = id,
                DatumUkonceniAP = datumUkonceniAP
            };

            return prehledBoduAPViewModel;
        }

        public static PrehledBoduAPViewModel OdpovednyPracovnik(string jmeno, string email)
        {
            var prehledBoduAPViewModel = new PrehledBoduAPViewModel
            {
                JmenoPracovnika = jmeno,
                EmailOdpovednyPracovnik = email
            };

            return prehledBoduAPViewModel;
        }

        public static IEnumerable<PrehledBoduAPViewModel> GetBodyIdAPAll(int idAP)
        {
            var bodyAP = BodAPDataMapper.GetBodyIdAP(idAP).ToList();
            var zamestnanci = ZamestnanciDataMapper.GetZamestnanciAll().ToList();
            var oddeleni = OddeleniDataMapper.GetOddeleniAll();

            if (!bodyAP.Any())
            {
                yield break;
            }

            var query = from b in bodyAP
                        join zam in zamestnanci
                            on b.OdpovednaOsoba1Id equals zam.Id into gZam
                        from subZam in gZam.DefaultIfEmpty()
                        join odd in oddeleni
                            on b.OddeleniId equals odd.Id into gOdd
                        from subOdd in gOdd.DefaultIfEmpty()
                        orderby b.CisloBoduAP
                        select BodyAP(b.Id, b.AkcniPlanId, b.CisloBoduAP, b.DatumZalozeni, b.OdkazNaNormu, b.HodnoceniNeshody, b.PopisProblemu,
                        b.SkutecnaPricinaWM, b.NapravnaOpatreniWM, b.SkutecnaPricinaWS, b.NapravnaOpatreniWS,
                        b.OdpovednaOsoba1Id, b.OdpovednaOsoba2Id, subZam.Prijmeni + " " + subZam.Jmeno, b.KontrolaEfektivnosti, b.OddeleniId, subOdd.Nazev, b.Priloha,
                        b.ZamitnutiTerminu, b.ZmenaTerminu, b.ZnovuOtevrit, b.EmailOdeslan, b.StavObjektu);

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<PrehledBoduAPViewModel> GetUkonceniBodAP(int bodAPId)
        {
            var ukonceniBodAP = BodAPDataMapper.GetUkonceniBodAP(bodAPId).ToList();

            if (!ukonceniBodAP.Any())
            {
                yield break;
            }

            var query = from u in ukonceniBodAP
                        where u.StavObjektuUkonceni == 1
                        orderby u.Id
                        select UkonceniBodAP(u.Id, u.BodAPId, u.DatumUkonceni, u.Poznamka, u.Odpoved, u.StavZadosti, u.StavObjektuUkonceni);

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<PrehledBoduAPViewModel> GetUkonceniAPId(int idAP)
        {
            var ukonceniAP = UkonceniAPDataMapper.GetUkonceniAP(idAP).ToList();

            if (!ukonceniAP.Any())
            {
                yield break;
            }

            var query = from u in ukonceniAP
                        where u.APId == idAP
                        orderby u.Id descending
                        select UkonceniAP(u.Id, u.DatumUkonceni);

            foreach (var q in query)
            {
                yield return q;
            }
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

        public static IEnumerable<PrehledBoduAPViewModel> GetOdpovednyPracovnik(int idOdpPrac)
        {
            var odpPrac = ZamestnanciDataMapper.GetOdpovednyPracovnikId(idOdpPrac).ToList();

            if (!odpPrac.Any())
            {
                yield break;
            }

            var query = from o in odpPrac
                        select OdpovednyPracovnik(o.Prijmeni + " " + o.Jmeno, o.Email);

            foreach (var q in query)
            {
                yield return q;
            }
        }

    }
}
