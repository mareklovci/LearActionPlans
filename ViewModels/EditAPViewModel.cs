using System;
using System.Collections.Generic;
using System.Linq;
using LearActionPlans.DataMappers;

namespace LearActionPlans.ViewModels
{
    public class EditAPViewModel
    {
        //Start Akční plán
        public int AkcniPlanId { get; set; }
        public byte ZmenaTerminu { get; set; }
        public byte ZnovuOtevrit { get; set; }
        public string DuvodZnovuotevreni { get; set; }

        public DateTime? UzavreniAP { get; set; }
        //End Akční plán

        //Start UkonceniAP
        public int UkonceniAPId { get; set; }
        public int APId { get; set; }
        public DateTime DatumUkonceni { get; set; }

        public string Poznamka { get; set; }
        //End UkonceniAP

        //Start Akce
        public DateTime? KontrolaEfektivnosti { get; set; }
        //End Akce

        //Start Zaměstnanci
        public int ZamestnanecId { get; set; }
        public string Jmeno { get; set; }

        public string Prijmeni { get; set; }
        //End Zaměstannci

        //Start Projekty
        public int ProjektId { get; set; }
        public string NazevProjektu { get; set; }

        public bool StornoProjektu { get; set; }
        //End Projekty

        //Start Zákazníci
        public int ZakaznikId { get; set; }
        public string NazevZakaznika { get; set; }

        public bool StornoZakaznika { get; set; }
        //End Zákazníci

        public static EditAPViewModel Zamestnanec(int zamestnanecId, string jmeno)
        {
            var editAPViewModel = new EditAPViewModel {ZamestnanecId = zamestnanecId, Jmeno = jmeno};
            return editAPViewModel;
        }

        public static EditAPViewModel Projekt(int projektId, string nazev)
        {
            var editAPViewModel = new EditAPViewModel {ProjektId = projektId, NazevProjektu = nazev};
            return editAPViewModel;
        }

        public static EditAPViewModel Zakaznik(int zakaznikId, string nazev)
        {
            var editAPViewModel = new EditAPViewModel {ZakaznikId = zakaznikId, NazevZakaznika = nazev};
            return editAPViewModel;
        }

        public static EditAPViewModel ZmenaTerminuAP(int akcniPlanId, byte zmenaTerminu)
        {
            var editAPViewModel = new EditAPViewModel {AkcniPlanId = akcniPlanId, ZmenaTerminu = zmenaTerminu};

            return editAPViewModel;
        }

        public static EditAPViewModel ZnovuOtevritAP(int akcniPlanId, byte znovuOtevrit, DateTime? uzavreniAP,
            string duvod)
        {
            var editAPViewModel = new EditAPViewModel
            {
                AkcniPlanId = akcniPlanId,
                ZnovuOtevrit = znovuOtevrit,
                UzavreniAP = uzavreniAP,
                DuvodZnovuotevreni = duvod
            };

            return editAPViewModel;
        }

        public static EditAPViewModel DatumUkonceniAP(int id, int akcniPlanId, DateTime datumUkonceni, string poznamka)
        {
            var editAPViewModel = new EditAPViewModel
            {
                UkonceniAPId = id, AkcniPlanId = akcniPlanId, DatumUkonceni = datumUkonceni, Poznamka = poznamka
            };

            return editAPViewModel;
        }

        public static IEnumerable<EditAPViewModel> GetZamestnanecId(int id)
        {
            var zamestnanci = EmployeeRepository.GetZamestnanciAll().ToList();

            var query = zamestnanci.Where(z => z.Id == id).Select(z => Zamestnanec(z.Id, z.Prijmeni + " " + z.Jmeno))
                .ToList();

            if (!query.Any())
            {
                yield break;
            }

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<EditAPViewModel> GetZamestnanci()
        {
            var zamestnanci = EmployeeRepository.GetZamestnanciAll().ToList();

            var query = zamestnanci.OrderBy(z => z.Prijmeni)
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

        public static IEnumerable<EditAPViewModel> GetProjektId(int id)
        {
            var projekty = ProjektyDataMapper.GetProjektyAll().ToList();

            var query = projekty.Where(p => p.Id == id).Select(p => Projekt(p.Id, p.Nazev)).ToList();

            if (!query.Any())
            {
                yield break;
            }

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<EditAPViewModel> GetProjekty()
        {
            var projekty = ProjektyDataMapper.GetProjektyAll().ToList();

            var query = projekty.OrderBy(p => p.Nazev).Select(p => Projekt(p.Id, p.Nazev)).ToList();

            if (!query.Any())
            {
                yield break;
            }

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<EditAPViewModel> GetZakaznikId(int id)
        {
            var zakaznici = ZakazniciDataMapper.GetZakazniciAll().ToList();

            var query = zakaznici.Where(z => z.Id == id).Select(z => Zakaznik(z.Id, z.Nazev)).ToList();

            if (!query.Any())
            {
                yield break;
            }

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<EditAPViewModel> GetZakaznici()
        {
            var zakaznici = ZakazniciDataMapper.GetZakazniciAll().ToList();

            var query = zakaznici.OrderBy(z => z.Nazev).Select(z => Zakaznik(z.Id, z.Nazev)).ToList();

            if (!query.Any())
            {
                yield break;
            }

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static EditAPViewModel UkonceniAkce(DateTime? kontrolaEfektivnosti)
        {
            var editAPViewModel = new EditAPViewModel {KontrolaEfektivnosti = kontrolaEfektivnosti};
            return editAPViewModel;
        }

        public static IEnumerable<EditAPViewModel> GetUkonceniAkce(int idAP)
        {
            var uzavreneAkce = UkonceniBodAPDataMapper.GetUkonceniAkceAll(idAP).ToList();

            if (!uzavreneAkce.Any())
            {
                yield break;
            }

            var query = uzavreneAkce.Select(ua => UkonceniAkce(ua.KontrolaEfektivnosti));

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<EditAPViewModel> GetPocetTerminu(int idAP)
        {
            var terminy = AkcniPlanyDataMapper.GetPocetTerminuAP(idAP).ToList();

            if (!terminy.Any())
            {
                yield break;
            }

            var query = terminy.Select(t => ZmenaTerminuAP(t.Id, t.ZmenaTerminu));

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<EditAPViewModel> GetDatumUkonceniAP(int idAP)
        {
            var datumUkonceni = UkonceniAPDataMapper.GetUkonceniAP(idAP).ToList();

            if (!datumUkonceni.Any())
            {
                yield break;
            }

            var query = datumUkonceni.Select(du => DatumUkonceniAP(du.Id, du.APId, du.DatumUkonceni, du.Poznamka));

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<EditAPViewModel> GetZnovuOtevritAP(int idAP)
        {
            var znovuOtevrit = AkcniPlanyDataMapper.GetZnovuOtevritAP(idAP).ToList();

            if (!znovuOtevrit.Any())
            {
                yield break;
            }

            var query = znovuOtevrit.Select(zo =>
                ZnovuOtevritAP(zo.Id, zo.ZnovuOtevrit, zo.UzavreniAP, zo.DuvodZnovuotevreni));

            foreach (var q in query)
            {
                yield return q;
            }
        }
    }
}
