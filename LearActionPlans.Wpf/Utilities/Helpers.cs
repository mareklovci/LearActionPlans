using LearActionPlans.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearActionPlans.Wpf.Utilities
{
    class Helpers
    {
        public static AkcniPlan LastActionPlan()
        {
            AkcniPlan actionPlan = null;

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
            Zamestnanec employee = null;

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
            Projekt project = null;

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
            Zakaznik customer = null;

            using (var context = new LearDataAllEntities())
            {
                customer = (from z in context.Zakaznik
                            where z.ZakaznikID == id
                            select z).FirstOrDefault();
            }

            return customer;
        }
    }
}
