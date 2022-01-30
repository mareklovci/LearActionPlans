using System;
using System.Collections.Generic;
using System.Linq;
using LearActionPlans.DataMappers;

namespace LearActionPlans.ViewModels
{
    public class ZadaniBoduAPViewModel
    {
        //body AP
        public int BodId { get; set; }
        public int IdAP { get; set; }
        public int CisloBoduAP { get; set; }
        public DateTime DatumZalozeni { get; set; }
        public string OdkazNaNormu { get; set; }
        public string HodnoceniNeshody { get; set; }
        public string PopisProblemu { get; set; }
        public int OdpovednaOsoba1Id { get; set; }
        public int? OdpovednaOsoba2Id { get; set; }
        public string SkutecnaPricinaWM { get; set; }
        public string NapravnaOpatreniWM { get; set; }
        public string SkutecnaPricinaWS { get; set; }
        public string NapravnaOpatreniWS { get; set; }
        public int? OddeleniId { get; set; }
        public DateTime? KontrolaEfektivnosti { get; set; }
        public string Priloha { get; set; }

        public bool ZnovuOtevrit { get; set; }
        public byte StavObjektuBodAP { get; set; }

        public int ZamestnanecId { get; set; }
        public string OdpovednaOsoba1 { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }

        public int OddeleniId_ { get; set; }
        public string Nazev { get; set; }

        public class NovyBodAP
        {
            public int CisloBoduAP { get; set; }
            public string OdkazNaNormu { get; set; }
            public string HodnoceniNeshody { get; set; }
            public string PopisProblemu { get; set; }
            public string SkutecnaPricinaWM { get; set; }
        }

        public static ZadaniBoduAPViewModel Zamestnanec(int id, string jmeno)
        {
            var zadaniBoduAPViewModel = new ZadaniBoduAPViewModel {ZamestnanecId = id, Jmeno = jmeno};

            return zadaniBoduAPViewModel;
        }

        public static ZadaniBoduAPViewModel Oddeleni(int id, string nazev)
        {
            var zadaniBoduAPViewModel = new ZadaniBoduAPViewModel {OddeleniId_ = id, Nazev = nazev};

            return zadaniBoduAPViewModel;
        }

        public static ZadaniBoduAPViewModel BodyAP(int id, int idAP, int cisloBoduAP, DateTime datumZalozeni,
            string odkazNaNormu,
            string hodnoceniNeshody, string popisProblemu, string skurecnaPricinaWM, string napravnaOpatreniWM,
            string skutecnaPricinaWS, string napravnaOpatreniWS,
            int odpovednaOsoba1Id, int? odpovednaOsoba2Id, int? oddeleniId, DateTime? kontrolaEfektivnosti,
            string priloha, bool znovuOtevrit, byte stavObjektuBodAP)
        {
            var zadaniBoduAPViewModel = new ZadaniBoduAPViewModel
            {
                BodId = id,
                IdAP = idAP,
                CisloBoduAP = cisloBoduAP,
                DatumZalozeni = datumZalozeni,
                OdkazNaNormu = odkazNaNormu,
                HodnoceniNeshody = hodnoceniNeshody,
                PopisProblemu = popisProblemu,
                SkutecnaPricinaWM = skurecnaPricinaWM,
                NapravnaOpatreniWM = napravnaOpatreniWM,
                SkutecnaPricinaWS = skutecnaPricinaWS,
                NapravnaOpatreniWS = napravnaOpatreniWS,
                OdpovednaOsoba1Id = odpovednaOsoba1Id,
                OdpovednaOsoba2Id = odpovednaOsoba2Id,
                OddeleniId = oddeleniId,
                KontrolaEfektivnosti = kontrolaEfektivnosti,
                Priloha = priloha,
                ZnovuOtevrit = znovuOtevrit,
                StavObjektuBodAP = stavObjektuBodAP
            };
            return zadaniBoduAPViewModel;
        }

        public static IEnumerable<ZadaniBoduAPViewModel> GetZamestnanciAll()
        {
            var zamestnanci = ZamestnanciDataMapper.GetZamestnanciAll().ToList();

            var query = zamestnanci.Where(z => z.StavObjektu == 1)
                .OrderBy(z => z.Prijmeni)
                .ThenBy(z => z.Jmeno)
                .Select(z => Zamestnanec(z.Id, z.Prijmeni + " " + z.Jmeno)).ToList();

            if (!query.Any())
            {
                yield break;
            }

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<ZadaniBoduAPViewModel> GetOddeleniAll()
        {
            var oddeleni = OddeleniDataMapper.GetOddeleniAll().ToList();

            if (oddeleni[0] == null || oddeleni.Count == 0)
            {
                yield break;
            }

            var query = oddeleni.Where(o => o.StavObjektu == 1)
                .OrderBy(o => o.Nazev)
                .Select(o => Oddeleni(o.Id, o.Nazev)).ToList();

            if (!query.Any())
            {
                yield break;
            }

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<ZadaniBoduAPViewModel> GetBodId(int idBodAP)
        {
            var bodyAP = BodAPDataMapper.GetBodId(idBodAP).ToList();

            if (bodyAP.Count == 0)
            {
                yield break;
            }

            var query = from b in bodyAP
                select BodyAP(b.Id, b.AkcniPlanId, b.CisloBoduAP, b.DatumZalozeni, b.OdkazNaNormu, b.HodnoceniNeshody,
                    b.PopisProblemu,
                    b.SkutecnaPricinaWM, b.NapravnaOpatreniWM, b.SkutecnaPricinaWS, b.NapravnaOpatreniWS,
                    b.OdpovednaOsoba1Id, b.OdpovednaOsoba2Id, b.OddeleniId, b.KontrolaEfektivnosti, b.Priloha,
                    b.ZnovuOtevrit, b.StavObjektu);

            foreach (var q in query)
            {
                yield return q;
            }
        }
    }
}
