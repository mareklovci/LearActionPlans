using System.Collections.Generic;
using System.Linq;

using LearActionPlans.DataMappers;

namespace LearActionPlans.ViewModels
{
    public class AdminViewModel
    {
        private readonly EmployeeRepository employeeRepository;

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

        private AdminViewModel(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

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

        public IEnumerable<AdminViewModel> GetZamestnanci()
        {
            var employees = this.employeeRepository.GetZamestnanciAll().ToList();

            var query = employees.OrderBy(z => z.Prijmeni)
                .ThenBy(z => z.Jmeno)
                .Select(z => Zamestnanec(z.Id, z.Jmeno, z.Prijmeni, z.PrihlasovaciJmeno, z.Email, z.AdminAP,
                    z.OddeleniId, z.StavObjektu)).ToList();

            if (!query.Any())
            {
                yield break;
            }

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public bool VybranyZamestnanec(string login)
        {
            var zamestnanci = this.employeeRepository.GetZamestnanciAll().ToList();

            var query = from z in zamestnanci
                        where z.PrihlasovaciJmeno == login
                        select Zamestnanec(z.Id, z.Jmeno, z.Prijmeni, z.PrihlasovaciJmeno, z.Email, z.AdminAP, z.OddeleniId, z.StavObjektu);

            return query.Count() != 0;
        }

        public static IEnumerable<AdminViewModel> GetOddeleni()
        {
            var oddeleni = OddeleniDataMapper.GetOddeleniAll().ToList();

            var query = from o in oddeleni
                        orderby o.Nazev
                        select Oddeleni(o.Id, o.Nazev);

            if (query.Count() == 0)
            {
                yield break;
            }

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<NewActionPlanViewModel> GetProjekty()
        {
            var projekty = ProjektyDataMapper.GetProjektyAll().ToList();

            var query = from p in projekty
                        where p.StavObjektu == 1
                        orderby p.Nazev
                        select NewActionPlanViewModel.Project(p.Id, p.Nazev);

            if (query.Count() == 0)
            {
                yield break;
            }
            else
            {
                foreach (var q in query)
                {
                    yield return q;
                }
            }
        }

        public static IEnumerable<AdminViewModel> GetZakaznici()
        {
            var zakaznici = ZakazniciDataMapper.GetZakazniciAll().ToList();

            var query = from z in zakaznici
                        where z.StavObjektu == 1
                        orderby z.Nazev
                        select Zakaznik(z.Id, z.Nazev);

            if (query.Count() == 0)
            {
                yield break;
            }
            else
            {
                foreach (var q in query)
                {
                    yield return q;
                }
            }
        }

        public IEnumerable<AdminViewModel> GetPocetAdmin()
        {
            var zamestnanci = this.employeeRepository.GetZamestnanciAll().ToList();

            var query = zamestnanci.Where(z => z.StavObjektu == 1 && z.AdminAP).Select(z => Admin(z.Id)).ToList();

            if (!query.Any())
            {
                yield break;
            }

            foreach (var q in query)
            {
                yield return q;
            }
        }
    }
}
