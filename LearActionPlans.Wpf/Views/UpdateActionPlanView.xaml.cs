using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LearActionPlans.Wpf.Models;
// ReSharper disable IdentifierTypo

namespace LearActionPlans.Wpf.Views
{
    public partial class UpdateActionPlanView : Window
    {
        private AkcniPlan _akcniPlan;
        public UpdateActionPlanView(AkcniPlan akcniPlan)
        {
            _akcniPlan = akcniPlan;
            InitializeComponent();
            
            using (var context = new LearDataAllEntities())
            {
                var numberOfPlans = (from z in context.AkcniPlan select z).Count();
                NumberOfPlans.Text = numberOfPlans.ToString();

                var empOneQuery = (from z in context.Zamestnanec
                    where z.Storno == false && z.JeZamestnanec
                    select z).ToList();
                if (empOneQuery.Any()) ContractingAuthority1.ItemsSource = empOneQuery;
                
                var empTwoQuery = (from z in context.Zamestnanec
                    where z.Storno == false && z.JeZamestnanec
                    select z).ToList();
                if (empTwoQuery.Any()) ContractingAuthority2.ItemsSource = empTwoQuery;

                var projectsQuery = (from z in context.Projekt
                    where z.Storno == false
                    select z).ToList();
                if (projectsQuery.Any()) ProjectsComboBox.ItemsSource = projectsQuery;
                
                var customersQuery = (from z in context.Zakaznik
                    where z.Storno == false
                    select z).ToList();
                if (customersQuery.Any()) CustomersComboBox.ItemsSource = customersQuery;
            }

            //ContractingAuthority1.SelectedItem = _akcniPlan.Zadavatel1;
        }

        private void ContractingAuthority1_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void ContractingAuthority2_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void TopicField_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void ProjectsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var projekt = (Projekt) ProjectsComboBox.SelectedItem;
            _akcniPlan.ProjektID = projekt.ProjektID;
        }

        private void DatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void CustomersComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void BtnAudit_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void BtnOther_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnClickSaveBtn(object sender, RoutedEventArgs e)
        {
            using (var context = new LearDataAllEntities())
            {
                var selectedAkcniPlan = context.AkcniPlan.SingleOrDefault(ap => ap.AkcniPlanID == _akcniPlan.AkcniPlanID);

                if (selectedAkcniPlan == null) return;
                
                selectedAkcniPlan.ProjektID = _akcniPlan.ProjektID;
                selectedAkcniPlan.Zadavatel1ID = _akcniPlan.Zadavatel1ID;
                selectedAkcniPlan.Zadavatel2ID = _akcniPlan.Zadavatel2ID;
                selectedAkcniPlan.ZakaznikID = _akcniPlan.ZakaznikID;
                
                context.SaveChanges();
            }
        }

        private void OnClickCloseBtn(object sender, RoutedEventArgs e)
        {
            Close();
            CloseBtn.IsCancel = true;
        }
    }
}