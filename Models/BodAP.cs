using System;
using System.Collections.Generic;

namespace LearActionPlans.Models
{
    public class BodAP : DatabaseTable
    {
        public sealed override int Id { get; set; }
        public int AkcniPlanId { get; set; }
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
        public DateTime? DatumUkonceni { get; set; }
        public string UkonceniPoznamka { get; set; }
        public List<UkonceniBodAP> UkonceniBodAP { get; set; }
        public DateTime? KontrolaEfektivnosti { get; set; }
        public DateTime? KontrolaEfektivnostiPuvodniDatum { get; set; }

        //poznámka s odůvodněním odstranění
        public string KontrolaEfektivnostiOdstranit { get; set; }
        public int? OddeleniId { get; set; }
        public string Oddeleni { get; set; }
        public string Priloha { get; set; }
        public byte ZamitnutiTerminu { get; set; }
        public byte ZmenaTerminu { get; set; }
        public bool ZnovuOtevrit { get; set; }
        public bool BodUlozen { get; set; }
        public byte StavObjektu { get; set; }

        public BodAP(int id,
            byte zamitnutiTerminu,
            byte zmenaTerminu)
        {
            this.Id = id;
            this.ZamitnutiTerminu = zamitnutiTerminu;
            this.ZmenaTerminu = zmenaTerminu;
        }

        public BodAP(int akcniPlanId, 
            int cisloBoduAP, 
            DateTime datumZalozeni, 
            string odkazNaNormu, 
            string hodnoceniNeshody, 
            string popisProbemu, 
            string skutecnaPricinaWM,
            string napravnaOpatreniWM,
            string skutecnaPricinaWS,
            string napravnaOpatreniWS,
            int odpovednaOsoba1Id,
            int? odpovednaOsoba2Id,
            DateTime? kontrolaEfektivnosti,
            int? oddeleniId,
            string priloha,
            bool znovuOtevrit,
            byte stavObjektu)
        {
            this.AkcniPlanId = akcniPlanId;
            this.CisloBoduAP = cisloBoduAP;
            this.DatumZalozeni = datumZalozeni;
            this.OdkazNaNormu = odkazNaNormu;
            this.HodnoceniNeshody = hodnoceniNeshody;
            this.PopisProblemu = popisProbemu;
            this.SkutecnaPricinaWM = skutecnaPricinaWM;
            this.NapravnaOpatreniWM = napravnaOpatreniWM;
            this.SkutecnaPricinaWS = skutecnaPricinaWS;
            this.NapravnaOpatreniWS = napravnaOpatreniWS;
            this.OdpovednaOsoba1Id = odpovednaOsoba1Id;
            this.OdpovednaOsoba2Id = odpovednaOsoba2Id;
            this.KontrolaEfektivnosti = kontrolaEfektivnosti;
            this.OddeleniId = oddeleniId;
            this.Priloha = priloha;
            this.ZnovuOtevrit = znovuOtevrit;
            this.StavObjektu = stavObjektu;
        }

        public BodAP(int id,
            int akcniPlanId,
            int cisloBoduAP,
            DateTime datumZalozeni,
            string odkazNaNormu,
            string hodnoceniNeshody,
            string popisProblemu,
            string skutecnaPricinaWM,
            string napravnaOpatreniWM,
            string skutecnaPricinaWS,
            string napravnaOpatreniWS,
            int odpovednaOsoba1Id,
            int? odpovednaOsoba2Id,
            DateTime? kontrolaEfektivnosti,
            int? oddeleniId,
            string priloha,
            byte zamitnutiTerminu,
            byte zmenaTerminu,
            bool znovuOtevrit,
            bool bodUlozen,
            byte stavObjektu)
        {
            this.Id = id;
            this.AkcniPlanId = akcniPlanId;
            this.CisloBoduAP = cisloBoduAP;
            this.DatumZalozeni = datumZalozeni;
            this.OdkazNaNormu = odkazNaNormu;
            this.HodnoceniNeshody = hodnoceniNeshody;
            this.PopisProblemu = popisProblemu;
            this.SkutecnaPricinaWM = skutecnaPricinaWM;
            this.NapravnaOpatreniWM = napravnaOpatreniWM;
            this.SkutecnaPricinaWS = skutecnaPricinaWS;
            this.NapravnaOpatreniWS = napravnaOpatreniWS;
            this.OdpovednaOsoba1Id = odpovednaOsoba1Id;
            this.OdpovednaOsoba2Id = odpovednaOsoba2Id;
            this.KontrolaEfektivnosti = kontrolaEfektivnosti;
            this.OddeleniId = oddeleniId;
            this.Priloha = priloha;
            this.ZamitnutiTerminu = zamitnutiTerminu;
            this.ZmenaTerminu = zmenaTerminu;
            this.ZnovuOtevrit = znovuOtevrit;
            this.BodUlozen = bodUlozen;
            this.StavObjektu = stavObjektu;
        }

        public BodAP(int id,
            int akcniPlanId,
            int cisloBoduAP,
            DateTime datumZalozeni,
            string odkazNaNormu,
            string hodnoceniNeshody,
            string popisProblemu,
            string skutecnaPricinaWM,
            string napravnaOpatreniWM,
            string skutecnaPricinaWS,
            string napravnaOpatreniWS,
            int odpovednaOsoba1Id,
            int? odpovednaOsoba2Id,
            string odpovednaOsoba1,
            DateTime? kontrolaEfektivnosti,
            int? oddeleniId,
            string oddeleni,
            string priloha,
            byte zamitnutiTerminu,
            byte zmenaTerminu,
            bool znovuOtevrit,
            bool bodUlozen,
            byte stavObjektu)
        {
            this.Id = id;
            this.AkcniPlanId = akcniPlanId;
            this.CisloBoduAP = cisloBoduAP;
            this.DatumZalozeni = datumZalozeni;
            this.OdkazNaNormu = odkazNaNormu;
            this.HodnoceniNeshody = hodnoceniNeshody;
            this.PopisProblemu = popisProblemu;
            this.SkutecnaPricinaWM = skutecnaPricinaWM;
            this.NapravnaOpatreniWM = napravnaOpatreniWM;
            this.SkutecnaPricinaWS = skutecnaPricinaWS;
            this.NapravnaOpatreniWS = napravnaOpatreniWS;
            this.OdpovednaOsoba1Id = odpovednaOsoba1Id;
            this.OdpovednaOsoba2Id = odpovednaOsoba2Id;
            this.OdpovednaOsoba1 = odpovednaOsoba1;
            this.KontrolaEfektivnosti = kontrolaEfektivnosti;
            this.OddeleniId = oddeleniId;
            this.Oddeleni = oddeleni;
            this.Priloha = priloha;
            this.ZamitnutiTerminu = zamitnutiTerminu;
            this.ZmenaTerminu = zmenaTerminu;
            this.ZnovuOtevrit = znovuOtevrit;
            this.BodUlozen = bodUlozen;
            this.StavObjektu = stavObjektu;
        }

        public BodAP(int id,
            int akcniPlanId,
            int cisloBoduAP,
            DateTime datumZalozeni,
            string odkazNaNormu,
            string hodnoceniNeshody,
            string popisProblemu,
            string skutecnaPricinaWM,
            string napravnaOpatreniWM,
            string skutecnaPricinaWS,
            string napravnaOpatreniWS,
            int odpovednaOsoba1Id,
            int? odpovednaOsoba2Id,
            DateTime? kontrolaEfektivnosti,
            int? oddeleniId,
            string priloha,
            bool znovuOtevrit,
            bool bodUlozen,
            byte stavObjektu)
        {
            this.Id = id;
            this.AkcniPlanId = akcniPlanId;
            this.CisloBoduAP = cisloBoduAP;
            this.DatumZalozeni = datumZalozeni;
            this.OdkazNaNormu = odkazNaNormu;
            this.HodnoceniNeshody = hodnoceniNeshody;
            this.PopisProblemu = popisProblemu;
            this.SkutecnaPricinaWM = skutecnaPricinaWM;
            this.NapravnaOpatreniWM = napravnaOpatreniWM;
            this.SkutecnaPricinaWS = skutecnaPricinaWS;
            this.NapravnaOpatreniWS = napravnaOpatreniWS;
            this.OdpovednaOsoba1Id = odpovednaOsoba1Id;
            this.OdpovednaOsoba2Id = odpovednaOsoba2Id;
            this.KontrolaEfektivnosti = kontrolaEfektivnosti;
            this.OddeleniId = oddeleniId;
            this.Priloha = priloha;
            this.ZnovuOtevrit = znovuOtevrit;
            this.BodUlozen = bodUlozen;
            this.StavObjektu = stavObjektu;
        }

        public BodAP(int akcniPlanId,
            int cisloBoduAP,
            DateTime datumZalozeni,
            string odkazNaNormu,
            string hodnoceniNeshody,
            string popisProblemu,
            string skutecnaPricinaWM,
            string napravnaOpatreniWM,
            string skutecnaPricinaWS,
            string napravnaOpatreniWS,
            int odpovednaOsoba1Id,
            int? odpovednaOsoba2Id,
            DateTime? kontrolaEfektivnosti,
            int? oddeleniId,
            string priloha,
            DateTime? datumUkonceni,
            string ukonceniPoznamka,
            bool znovuOtevrit,
            bool bodUlozen,
            byte stavObjektu)
        {
            this.AkcniPlanId = akcniPlanId;
            this.CisloBoduAP = cisloBoduAP;
            this.DatumZalozeni = datumZalozeni;
            this.OdkazNaNormu = odkazNaNormu;
            this.HodnoceniNeshody = hodnoceniNeshody;
            this.PopisProblemu = popisProblemu;
            this.SkutecnaPricinaWM = skutecnaPricinaWM;
            this.NapravnaOpatreniWM = napravnaOpatreniWM;
            this.SkutecnaPricinaWS = skutecnaPricinaWS;
            this.NapravnaOpatreniWS = napravnaOpatreniWS;
            this.OdpovednaOsoba1Id = odpovednaOsoba1Id;
            this.OdpovednaOsoba2Id = odpovednaOsoba2Id;
            this.KontrolaEfektivnosti = kontrolaEfektivnosti;
            this.OddeleniId = oddeleniId;
            this.Priloha = priloha;
            this.DatumUkonceni = datumUkonceni;
            this.UkonceniPoznamka = ukonceniPoznamka;
            this.ZnovuOtevrit = znovuOtevrit;
            this.BodUlozen = bodUlozen;
            this.StavObjektu = stavObjektu;
        }
    }
}
