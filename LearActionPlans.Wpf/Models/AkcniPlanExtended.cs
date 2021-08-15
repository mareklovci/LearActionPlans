using System;
using System.Linq;
// ReSharper disable IdentifierTypo

namespace LearActionPlans.Wpf.Models
{
    public partial class AkcniPlan
    {
        public Zamestnanec Zadavatel1 => GetZadavatel(Zadavatel1ID);
        public Zamestnanec Zadavatel2 => GetZadavatel(Zadavatel2ID);
        public Projekt Projekt => GetProjekt();
        public Zakaznik Zakaznik => GetCustomer();
        public UkonceniAP Deadline => GetActionPlanDeadline();

        private UkonceniAP GetActionPlanDeadline()
        {
            using (var context = new LearDataAllEntities())
            {
                var query = (from z in context.UkonceniAP
                    where z.AkcniPlanID == AkcniPlanID
                    orderby z.DatumUkonceni descending 
                    select z).FirstOrDefault();
                return query;
            }
        }

        private Zakaznik GetCustomer()
        {
            using (var context = new LearDataAllEntities())
            {
                var query = (from z in context.Zakaznik
                    where z.ZakaznikID == ZakaznikID
                    select z).FirstOrDefault();
                return query;
            }
        }

        private Projekt GetProjekt()
        {
            using (var context = new LearDataAllEntities())
            {
                var query = (from z in context.Projekt
                    where z.ProjektID == ProjektID
                    select z).FirstOrDefault();
                return query;
            }
        }

        private static Zamestnanec GetZadavatel(int? zadavatelId)
        {
            if (zadavatelId == null) return null;
            using (var context = new LearDataAllEntities())
            {
                var query = (from z in context.Zamestnanec
                    where z.ZamestnanecID == zadavatelId
                    select z).FirstOrDefault();
                return query;
            }
        }
    }
}