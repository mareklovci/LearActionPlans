using System.Linq;
using System.Windows;
using LearActionPlans.Wpf.Models;
using LearActionPlans.Wpf.Utilities;

namespace LearActionPlans.Wpf.Views
{
    public partial class ListOfActionPlanPoints : Window
    {
        public ListOfActionPlanPoints(AkcniPlan akcniPlan)
        {
            InitializeComponent();

            using (var context = new LearDataAllEntities())
            {
                var actionPlan = (from z in context.AkcniPlan
                                  where z.AkcniPlanID == akcniPlan.AkcniPlanID
                                  select z).FirstOrDefault();
                
                ActionPlanNumber.Text = actionPlan.AkcniPlanID.ToString();

                var zadavatel1 = Helpers.EmployeeById(actionPlan.Zadavatel1ID);
                ActionPlanContractingAuthority1.Text = $"{zadavatel1.Jmeno} {zadavatel1.Prijmeni}";

                if (actionPlan.Zadavatel2ID != null)
                {
                    var zadavatel2 = Helpers.EmployeeById((int) actionPlan.Zadavatel2ID);
                    ActionPlanContractingAuthority2.Text = $"{zadavatel2.Jmeno} {zadavatel2.Prijmeni}";
                }

                ActionPlanTopic.Text = actionPlan.Tema;

                if (actionPlan.ProjektID != null)
                {
                    var project = Helpers.ProjectById((int) actionPlan.ProjektID);
                    ActionPlanProject.Text = $"{project.Nazev}";
                }

                ActionPlanEndDate.Text = actionPlan.DatumUkonceni.ToString();

                var customer = Helpers.CustomerById(actionPlan.ZakaznikID);
                ActionPlanCustomer.Text = $"{customer.Nazev}";

                // Populate list
                var query = (from ap in context.BodAP
                             where ap.AkcniPlanID == actionPlan.AkcniPlanID
                             select ap).ToList();
                listView.ItemsSource = query;
            }
        }
    }
}