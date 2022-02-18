using System;
using System.Collections.Generic;
using System.Linq;
using LearActionPlans.Repositories;

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
    }
}
