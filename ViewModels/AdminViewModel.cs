using System.Collections.Generic;
using System.Linq;

using LearActionPlans.DataMappers;

namespace LearActionPlans.ViewModels
{
    public class AdminViewModel
    {
        //Start Zaměstnanci
        public int ZamestnanecId { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public bool AdminAP { get; set; }
        public int OddeleniId_ { get; set; }
        public byte StavObjektu { get; set; }
        //End Zaměstannci

        //Start Projekty
        public int ProjektId { get; set; }
        public string NazevProjektu { get; set; }
        //End Projekty

        //Start Zákazníci
        public int ZakaznikId { get; set; }
        public string NazevZakaznika { get; set; }
        //End Projekty

        //Start Oddělení
        public int OddeleniId { get; set; }
        public string NazevOddeleni { get; set; }
        //End Projekty

        public static AdminViewModel Zamestnanec(int zamestnanecId, string jmeno, string prijmeni, string login, string email, bool adminAP, int oddeleniId, byte stavObjektu)
        {
            var adminViewModel = new AdminViewModel
            {
                ZamestnanecId = zamestnanecId,
                Jmeno = jmeno,
                Prijmeni = prijmeni,
                Login = login,
                Email = email,
                AdminAP = adminAP,
                OddeleniId = oddeleniId,
                StavObjektu = stavObjektu
        };
            return adminViewModel;
        }

        public static AdminViewModel Admin(int zamestnanecId)
        {
            var adminViewModel = new AdminViewModel
            {
                ZamestnanecId = zamestnanecId
            };
            return adminViewModel;
        }

        public static AdminViewModel Oddeleni(int oddeleniId, string nazev)
        {
            var adminViewModel = new AdminViewModel
            {
                OddeleniId = oddeleniId,
                NazevOddeleni = nazev
            };
            return adminViewModel;
        }

        public static AdminViewModel Projekt(int projektId, string nazev)
        {
            var adminViewModel = new AdminViewModel
            {
                ProjektId = projektId,
                NazevProjektu = nazev
            };
            return adminViewModel;
        }

        public static AdminViewModel Zakaznik(int zakaznikId, string nazev)
        {
            var adminViewModel = new AdminViewModel
            {
                ZakaznikId = zakaznikId,
                NazevZakaznika = nazev
            };
            return adminViewModel;
        }

        public static IEnumerable<AdminViewModel> GetZamestnanci()
        {
            var zamestnanci = ZamestnanciDataMapper.GetZamestnanciAll().ToList();

            var query = from z in zamestnanci
                        orderby z.Prijmeni, z.Jmeno
                        select AdminViewModel.Zamestnanec(z.Id, z.Jmeno, z.Prijmeni, z.PrihlasovaciJmeno, z.Email, z.AdminAP, z.OddeleniId, z.StavObjektu);

            if (query.Count() == 0)
            {
                yield break;
            }
            else
            {
                foreach (var q in query)
                    yield return q;
            }
        }

        public static bool VybranyZamestnanec(string login)
        {
            var zamestnanci = ZamestnanciDataMapper.GetZamestnanciAll().ToList();

            var query = from z in zamestnanci
                        where z.PrihlasovaciJmeno == login
                        select AdminViewModel.Zamestnanec(z.Id, z.Jmeno, z.Prijmeni, z.PrihlasovaciJmeno, z.Email, z.AdminAP, z.OddeleniId, z.StavObjektu);

            if (query.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static IEnumerable<AdminViewModel> GetOddeleni()
        {
            var oddeleni = OddeleniDataMapper.GetOddeleniAll().ToList();

            var query = from o in oddeleni
                        orderby o.Nazev
                        select AdminViewModel.Oddeleni(o.Id, o.Nazev);

            if (query.Count() == 0)
            {
                yield break;
            }
            else
            {
                foreach (var q in query)
                    yield return q;
            }
        }

        public static IEnumerable<NovyAkcniPlanViewModel> GetProjekty()
        {
            var projekty = ProjektyDataMapper.GetProjektyAll().ToList();

            var query = from p in projekty
                        where p.StavObjektu == 1
                        orderby p.Nazev
                        select NovyAkcniPlanViewModel.Projekt(p.Id, p.Nazev);

            if (query.Count() == 0)
            {
                yield break;
            }
            else
            {
                foreach (var q in query)
                    yield return q;
            }
        }

        public static IEnumerable<AdminViewModel> GetZakaznici()
        {
            var zakaznici = ZakazniciDataMapper.GetZakazniciAll().ToList();

            var query = from z in zakaznici
                        where z.StavObjektu == 1
                        orderby z.Nazev
                        select AdminViewModel.Zakaznik(z.Id, z.Nazev);

            if (query.Count() == 0)
            {
                yield break;
            }
            else
            {
                foreach (var q in query)
                    yield return q;
            }
        }

        public static IEnumerable<AdminViewModel> GetPocetAdmin()
        {
            var zamestnanci = ZamestnanciDataMapper.GetZamestnanciAll().ToList();

            var query = from z in zamestnanci
                        where z.StavObjektu == 1 && z.AdminAP is true
                        select AdminViewModel.Admin(z.Id);

            if (query.Count() == 0)
            {
                yield break;
            }
            else
            {
                foreach (var q in query)
                    yield return q;
            }
        }
    }
}
