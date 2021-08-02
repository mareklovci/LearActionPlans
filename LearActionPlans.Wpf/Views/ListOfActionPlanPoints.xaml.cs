using System.Globalization;
using System.Linq;
using System.Windows;
using LearActionPlans.Wpf.Models;
using LearActionPlans.Wpf.Utilities;
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

namespace LearActionPlans.Wpf.Views
{
    public partial class ListOfActionPlanPoints : Window
    {
        private readonly AkcniPlan _akcniPlan;
        
        public ListOfActionPlanPoints(AkcniPlan akcniPlan)
        {
            InitializeComponent();
            _akcniPlan = akcniPlan;
            
            using (var context = new LearDataAllEntities())
            {
                var actionPlan = (from z in context.AkcniPlan
                                  where z.AkcniPlanID == akcniPlan.AkcniPlanID
                                  select z).FirstOrDefault();

                if (actionPlan == null) return;
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

                // Get Datum Ukonceni
                var datumUkonceni = (from z in context.UkonceniAP
                    where z.AkcniPlanID == actionPlan.AkcniPlanID
                    orderby z.UkonceniAPID
                    select z).FirstOrDefault();

                if (datumUkonceni != null)
                    ActionPlanEndDate.Text = datumUkonceni.DatumUkonceni.ToString(CultureInfo.InvariantCulture);

                var customer = Helpers.CustomerById(actionPlan.ZakaznikID);
                ActionPlanCustomer.Text = $"{customer.Nazev}";

                // Populate list
                var query = (from ap in context.BodAP
                    where ap.AkcniPlanID == actionPlan.AkcniPlanID
                    select ap).ToList();
                listView.ItemsSource = query;
            }
        }

        private void NewPointBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new NewActionPlanPoint(_akcniPlan);
            win.Show();
        }
    }
}