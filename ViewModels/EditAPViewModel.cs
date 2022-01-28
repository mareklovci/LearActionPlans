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
        public string DuvodZnovuotevreni{ get; set; }
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

        //Start Ukončení akce
        //public int UkonceniAkceId { get; set; }
        
        //public int AkceId { get; set; }

        //public DateTime DatumUkonceni { get; set; }
        //public byte StavZadosti { get; set; }
        //End Ukončení akce


        //public int IdBodAP { get; set; }
        //public int IdAP  { get; set; }
        //public byte StavObjektuBodAP  { get; set; }

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
            var editAPViewModel = new EditAPViewModel
            {
                ZamestnanecId = zamestnanecId,
                Jmeno = jmeno
            };
            //ZamestnanecId = zamestnanecId;
            //Jmeno = jmeno;
            return editAPViewModel;
        }

        public static EditAPViewModel Projekt(int projektId, string nazev)
        {
            var editAPViewModel = new EditAPViewModel
            {
                ProjektId = projektId,
                NazevProjektu = nazev
            };
            //ProjektId = projektId;
            //NazevProjektu = nazev;
            //StornoProjektu = storno;
            return editAPViewModel;
        }

        public static EditAPViewModel Zakaznik(int zakaznikId, string nazev)
        {
            var editAPViewModel = new EditAPViewModel
            {
                ZakaznikId = zakaznikId,
                NazevZakaznika = nazev
            };
            //ZakaznikId = zakazniktId;
            //NazevZakaznika = nazev;
            //StornoZakaznika = storno;
            return editAPViewModel;
        }

        public static EditAPViewModel ZmenaTerminuAP(int akcniPlanId, byte zmenaTerminu)
        {
            var editAPViewModel = new EditAPViewModel
            {
                AkcniPlanId = akcniPlanId,
                ZmenaTerminu = zmenaTerminu
            };

            return editAPViewModel;
        }

        public static EditAPViewModel ZnovuOtevritAP(int akcniPlanId, byte znovuOtevrit, DateTime? uzavreniAP, string duvod)
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
                UkonceniAPId = id,
                AkcniPlanId = akcniPlanId,
                DatumUkonceni = datumUkonceni,
                Poznamka = poznamka
            };

            return editAPViewModel;
        }

        public static IEnumerable<EditAPViewModel> GetZamestnanecId(int id)
        {
            var zamestnanci = ZamestnanciDataMapper.GetZamestnanciAll().ToList();

            var query = from z in zamestnanci
                        where z.Id == id
                        select EditAPViewModel.Zamestnanec(z.Id, z.Prijmeni + " " + z.Jmeno);

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

        public static IEnumerable<EditAPViewModel> GetZamestnanci()
        {
            var zamestnanci = ZamestnanciDataMapper.GetZamestnanciAll().ToList();

            var query = from z in zamestnanci
                        orderby z.Prijmeni, z.Jmeno
                        select EditAPViewModel.Zamestnanec(z.Id, z.Prijmeni + " " + z.Jmeno);

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

        public static IEnumerable<EditAPViewModel> GetProjektId(int id)
        {
            var projekty = ProjektyDataMapper.GetProjektyAll().ToList();

            var query = from p in projekty
                        where p.Id == id
                        select EditAPViewModel.Projekt(p.Id, p.Nazev);

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

        public static IEnumerable<EditAPViewModel> GetProjekty()
        {
            var projekty = ProjektyDataMapper.GetProjektyAll().ToList();

            var query = from p in projekty
                        orderby p.Nazev
                        select EditAPViewModel.Projekt(p.Id, p.Nazev);

            //where p.Storno == false

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
        public static IEnumerable<EditAPViewModel> GetZakaznikId(int id)
        {
            var zakaznici = ZakazniciDataMapper.GetZakazniciAll().ToList();

            var query = from z in zakaznici
                        where z.Id == id
                        select EditAPViewModel.Zakaznik(z.Id, z.Nazev);

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

        public static IEnumerable<EditAPViewModel> GetZakaznici()
        {
            var zakaznici = ZakazniciDataMapper.GetZakazniciAll().ToList();

            var query = from z in zakaznici
                        orderby z.Nazev
                        select EditAPViewModel.Zakaznik(z.Id, z.Nazev);

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

        public static EditAPViewModel UkonceniAkce(DateTime? kontrolaEfektivnosti)
        {
            var editAPViewModel = new EditAPViewModel
            {
                KontrolaEfektivnosti = kontrolaEfektivnosti
            };
            return editAPViewModel;
        }

        //public static EditAPViewModel Akce(int akceId)
        //{
        //    var editAPViewModel = new EditAPViewModel
        //    {
        //        AkceId = akceId
        //    };
        //    return editAPViewModel;
        //}

        public static IEnumerable<EditAPViewModel> GetUkonceniAkce(int idAP)
        {
            var uzavreneAkce = UkonceniBodAPDataMapper.GetUkonceniAkceAll(idAP).ToList();

            if (uzavreneAkce.Count() == 0)
            {
                yield break;
            }

            var query = from ua in uzavreneAkce
                         select UkonceniAkce(ua.KontrolaEfektivnosti);

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<EditAPViewModel> GetPocetTerminu(int idAP)
        {
            var terminy = AkcniPlanyDataMapper.GetPocetTerminuAP(idAP).ToList();

            if (terminy.Count() == 0)
            {
                yield break;
            }

            var query = from t in terminy
                        select ZmenaTerminuAP(t.Id, t.ZmenaTerminu);

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<EditAPViewModel> GetDatumUkonceniAP(int idAP)
        {
            var datumUkonceni = UkonceniAPDataMapper.GetUkonceniAP(idAP).ToList();

            if (datumUkonceni.Count() == 0)
            {
                yield break;
            }

            var query = from du in datumUkonceni
                        select DatumUkonceniAP(du.Id, du.APId, du.DatumUkonceni, du.Poznamka);

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<EditAPViewModel> GetZnovuOtevritAP(int idAP)
        {
            var znovuOtevrit = AkcniPlanyDataMapper.GetZnovuOtevritAP(idAP).ToList();

            if (znovuOtevrit.Count() == 0)
            {
                yield break;
            }

            var query = from zo in znovuOtevrit
                        select ZnovuOtevritAP(zo.Id, zo.ZnovuOtevrit, zo.UzavreniAP, zo.DuvodZnovuotevreni);

            foreach (var q in query)
            {
                yield return q;
            }
        }
    }
}
