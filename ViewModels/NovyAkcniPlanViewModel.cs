using System.Collections.Generic;
using System.Linq;

using LearActionPlans.DataMappers;

namespace LearActionPlans.ViewModels
{
    public class NovyAkcniPlanViewModel
    {
        //Start Zaměstnanci
        public int ZamestnanecId { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        //End Zaměstannci

        //Start Projekty
        public int ProjektId { get; set; }
        public string NazevProjektu { get; set; }
        //End Projekty

        //Start Zákazníci
        public int ZakaznikId { get; set; }
        public string NazevZakaznika { get; set; }
        //End Projekty

        public NovyAkcniPlanViewModel()
        { 
        }

        public static NovyAkcniPlanViewModel Zamestnanec(int zamestnanecId, string jmeno)
        {
            var novyAkcniPlanViewModel = new NovyAkcniPlanViewModel
            {
                ZamestnanecId = zamestnanecId,
                Jmeno = jmeno
            };
            return novyAkcniPlanViewModel;
        }

        public static NovyAkcniPlanViewModel Projekt(int projektId, string nazev)
        {
            var novyAkcniPlanViewModel = new NovyAkcniPlanViewModel
            {
                ProjektId = projektId,
                NazevProjektu = nazev
            };
            return novyAkcniPlanViewModel;
        }

        public static NovyAkcniPlanViewModel Zakaznik(int zakaznikId, string nazev)
        {
            var novyAkcniPlanViewModel = new NovyAkcniPlanViewModel
            {
                ZakaznikId = zakaznikId,
                NazevZakaznika = nazev
            };
            return novyAkcniPlanViewModel;
        }

        public static IEnumerable<NovyAkcniPlanViewModel> GetZamestnanci()
        {
            var zamestnanci = ZamestnanciDataMapper.GetZamestnanciAll().ToList();

            var query = from z in zamestnanci
                        where z.StavObjektu == 1
                        orderby z.Prijmeni, z.Jmeno
                        select NovyAkcniPlanViewModel.Zamestnanec(z.Id, z.Prijmeni + " " + z.Jmeno);

            //where z.JeZamestnanec == true && z.Storno == false

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

        public static IEnumerable<NovyAkcniPlanViewModel> GetZakaznici()
        {
            var zakaznici = ZakazniciDataMapper.GetZakazniciAll().ToList();

            var query = from z in zakaznici
                        where z.StavObjektu == 1
                        orderby z.Nazev
                        select NovyAkcniPlanViewModel.Zakaznik(z.Id, z.Nazev);

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

        public static int GetPosledniCisloAP(int rok)
        {
            int posledniCisloAP = AkcniPlanyDataMapper.GetPosledniCisloAP(rok);
            
            return posledniCisloAP;
            //var query = from z in zakaznici
            //            where z.Storno == false
            //            select new NovyAkcniPlanViewModel(z.Id, z.Nazev, false, false);

            //if (query.Count() == 0)
            //{
            //    yield break;
            //}
            //else
            //{
            //    foreach (var q in query)
            //        yield return q;
            //}
        }
    }
}
