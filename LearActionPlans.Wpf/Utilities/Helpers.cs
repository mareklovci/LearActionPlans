using LearActionPlans.Wpf.Models;
using System.Collections.Generic;
using System.Linq;

namespace LearActionPlans.Wpf.Utilities
{
    internal static class Helpers
    {
        public static AkcniPlan LastActionPlan()
        {
            AkcniPlan actionPlan;

            using (var context = new LearDataAllEntities())
            {
                actionPlan = (from z in context.AkcniPlan
                              orderby z.AkcniPlanID descending
                              select z).FirstOrDefault();
            }

            return actionPlan;
        }

        public static Zamestnanec EmployeeById(int id)
        {
            Zamestnanec employee;

            using (var context = new LearDataAllEntities())
            {
                employee = (from z in context.Zamestnanec
                            where z.ZamestnanecID == id
                            select z).FirstOrDefault();
            }

            return employee;
        }

        public static Projekt ProjectById(int id)
        {
            Projekt project;

            using (var context = new LearDataAllEntities())
            {
                project = (from z in context.Projekt
                           where z.ProjektID == id
                           select z).FirstOrDefault();
            }

            return project;
        }

        public static Zakaznik CustomerById(int id)
        {
            Zakaznik customer;

            using (var context = new LearDataAllEntities())
            {
                customer = (from z in context.Zakaznik
                            where z.ZakaznikID == id
                            select z).FirstOrDefault();
            }

            return customer;
        }

        public static void AppendNullObject<T>(List<T> appendable, T append) => appendable.Insert(0, append);
    }
}
