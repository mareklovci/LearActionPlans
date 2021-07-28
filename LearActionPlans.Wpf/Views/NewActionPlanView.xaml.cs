using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using LearActionPlans.Wpf.Models;
// ReSharper disable IdentifierTypo

namespace LearActionPlans.Wpf.Views
{
    public partial class NewActionPlanView
    {
        private readonly AkcniPlan _akcniPlan = new AkcniPlan();
        
        public NewActionPlanView()
        {
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
        }

        private void OnClickSaveBtn(object sender, RoutedEventArgs e)
        {
            using (var context = new LearDataAllEntities())
            {
                var actionPlans = context.Set<AkcniPlan>();
                actionPlans.Add(_akcniPlan);

                context.SaveChanges();
            }
        }
        
        private void OnClickCloseBtn(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ContractingAuthority1_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var employee = (Zamestnanec) ContractingAuthority1.SelectedItem;
            if (employee != null) _akcniPlan.Zadavatel1ID = employee.ZamestnanecID;
        }

        private void ContractingAuthority2_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var employee = (Zamestnanec) ContractingAuthority2.SelectedItem;
            if (employee != null) _akcniPlan.Zadavatel2ID = employee.ZamestnanecID;
        }

        private void CustomersComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var customer = (Zakaznik) CustomersComboBox.SelectedItem;
            if (customer != null) _akcniPlan.ZakaznikID = customer.ZakaznikID;
        }

        private void DatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDate = DatePicker.SelectedDate;
            if(selectedDate.HasValue) _akcniPlan.Rok = selectedDate.Value.Year;
        }

        private void TopicField_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var newText = TopicField.Text;
            _akcniPlan.Tema = newText;
        }

        private void BtnAudit_Click(object sender, RoutedEventArgs e)
        {
            if (BtnAudit.IsChecked == true) _akcniPlan.AudityOstatni = true;
        }

        private void BtnOther_Click(object sender, RoutedEventArgs e)
        {
            if (BtnOther.IsChecked == true) _akcniPlan.AudityOstatni = false;
        }
    }
}