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
        private readonly AkcniPlany _akcniPlan = new AkcniPlany();
        
        public NewActionPlanView()
        {
            InitializeComponent();
            using (var context = new LearDataAllEntities())
            {
                var empOneQuery = from z in context.Zamestnanci select z;
                if (empOneQuery.Any()) ContractingAuthority1.ItemsSource = empOneQuery;
                
                var empTwoQuery = from z in context.Zamestnanci select z;
                if (empTwoQuery.Any()) ContractingAuthority2.ItemsSource = empTwoQuery;

                var projectsQuery = from z in context.Projekty select z;
                if (projectsQuery.Any()) ProjectsComboBox.ItemsSource = projectsQuery;
                
                var customersQuery = from z in context.Projekty select z;
                if (customersQuery.Any()) CustomersComboBox.ItemsSource = customersQuery;
            }
        }

        private void OnClickSaveBtn(object sender, RoutedEventArgs e)
        {
            using (var context = new LearDataAllEntities())
            {
                var actionPlans = context.Set<AkcniPlany>();
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
            var employee = (Zamestnanci) (ContractingAuthority1.SelectionBoxItem as PropertyInfo)?.GetValue(null);
            if (employee != null) _akcniPlan.zadavatel1_Id = employee.zamestnanec_Id;
        }

        private void ContractingAuthority2_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var employee = (Zamestnanci) (ContractingAuthority2.SelectionBoxItem as PropertyInfo)?.GetValue(null);
            if (employee != null) _akcniPlan.zadavatel2_Id = employee.zamestnanec_Id;
        }

        private void CustomersComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var customer = (Zakaznici) (CustomersComboBox.SelectionBoxItem as PropertyInfo)?.GetValue(null);
            if (customer != null) _akcniPlan.zakaznik_Id = customer.zakaznik_Id;
        }

        private void DatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDate = DatePicker.SelectedDate;
            if(selectedDate.HasValue) _akcniPlan.rok = selectedDate.Value.Year;
        }
    }
}