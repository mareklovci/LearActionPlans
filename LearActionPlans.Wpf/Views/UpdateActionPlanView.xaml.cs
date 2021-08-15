using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LearActionPlans.Wpf.Models;
using LearActionPlans.Wpf.Utilities;

// ReSharper disable IdentifierTypo

namespace LearActionPlans.Wpf.Views
{
    public partial class UpdateActionPlanView
    {
        private readonly AkcniPlan _akcniPlan;
        private UkonceniAP _ukonceniPlanu;

        public UpdateActionPlanView(AkcniPlan akcniPlan)
        {
            _akcniPlan = akcniPlan;
            InitializeComponent();

            List<Zamestnanec> empOneQuery;
            List<Zamestnanec> empTwoQuery;
            List<Projekt> projectsQuery;
            List<Zakaznik> customersQuery;

            // Populate ComboBoxes
            using (var context = new LearDataAllEntities())
            {
                _ukonceniPlanu = (from z in context.UkonceniAP
                    where z.AkcniPlanID == _akcniPlan.AkcniPlanID
                    orderby z.DatumUkonceni descending
                    select z).FirstOrDefault();

                var numberOfPlans = (from z in context.AkcniPlan select z).Count();
                NumberOfPlans.Text = numberOfPlans.ToString();

                empOneQuery = (from z in context.Zamestnanec
                    where z.Storno == false && z.JeZamestnanec
                    select z).ToList();
                if (empOneQuery.Any()) ContractingAuthority1.ItemsSource = empOneQuery;
                Helpers.AppendNullObject(empOneQuery, Zamestnanec.Null);
                
                empTwoQuery = (from z in context.Zamestnanec
                    where z.Storno == false && z.JeZamestnanec
                    select z).ToList();
                if (empTwoQuery.Any()) ContractingAuthority2.ItemsSource = empTwoQuery;
                Helpers.AppendNullObject(empTwoQuery, Zamestnanec.Null);

                projectsQuery = (from z in context.Projekt
                    where z.Storno == false
                    select z).ToList();
                if (projectsQuery.Any()) ProjectsComboBox.ItemsSource = projectsQuery;
                Helpers.AppendNullObject(projectsQuery, Projekt.Null);

                customersQuery = (from z in context.Zakaznik
                    where z.Storno == false
                    select z).ToList();
                if (customersQuery.Any()) CustomersComboBox.ItemsSource = customersQuery;
                Helpers.AppendNullObject(customersQuery, Zakaznik.Null);
            }

            // Add Selected Items
            ContractingAuthority1.SelectedItem =
                empOneQuery.FirstOrDefault(x => x.ZamestnanecID == _akcniPlan.Zadavatel1ID) ?? Zamestnanec.Null;

            ContractingAuthority2.SelectedItem =
                empTwoQuery.FirstOrDefault(x => x.ZamestnanecID == _akcniPlan.Zadavatel2ID) ?? Zamestnanec.Null;

            ProjectsComboBox.SelectedItem =
                projectsQuery.FirstOrDefault(x => x.ProjektID == _akcniPlan.Projekt.ProjektID) ?? Projekt.Null;

            CustomersComboBox.SelectedItem =
                customersQuery.FirstOrDefault(x => x.ZakaznikID == _akcniPlan.ZakaznikID) ?? Zakaznik.Null;

            TopicField.Text = _akcniPlan.Tema;

            if (_ukonceniPlanu != null) DatePicker.SelectedDate = _ukonceniPlanu.DatumUkonceni.Date;

            if (_akcniPlan.AudityOstatni)
                BtnAudit.IsChecked = true;
            else
                BtnOther.IsChecked = true;
        }

        private void ContractingAuthority1_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var employee = (Zamestnanec)ContractingAuthority1.SelectedItem;
            if (employee != null) _akcniPlan.Zadavatel1ID = employee.ZamestnanecID;
        }

        private void ContractingAuthority2_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var employee = (Zamestnanec)ContractingAuthority2.SelectedItem;
            if (employee != null) _akcniPlan.Zadavatel2ID = employee.ZamestnanecID;
        }

        private void TopicField_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var topic = TopicField.Text;
            if (string.IsNullOrWhiteSpace(topic)) _akcniPlan.Tema = topic;
        }

        private void ProjectsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var projekt = (Projekt)ProjectsComboBox.SelectedItem;
            _akcniPlan.ProjektID = projekt.ProjektID;
        }

        private void DatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDate = DatePicker.SelectedDate;
            if (!selectedDate.HasValue) return;
            if (_ukonceniPlanu != null)
                _ukonceniPlanu.DatumUkonceni = selectedDate.Value;
            else
            {
                _ukonceniPlanu = new UkonceniAP
                {
                    DatumUkonceni = selectedDate.Value,
                    AkcniPlanID = _akcniPlan.AkcniPlanID
                };
            }
        }

        private void CustomersComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var customer = (Zakaznik)CustomersComboBox.SelectedItem;
            _akcniPlan.ZakaznikID = customer.ZakaznikID;
        }

        private void BtnAudit_Click(object sender, RoutedEventArgs e)
        {
            if (BtnAudit.IsChecked == true) _akcniPlan.AudityOstatni = true;
        }

        private void BtnOther_Click(object sender, RoutedEventArgs e)
        {
            if (BtnOther.IsChecked == true) _akcniPlan.AudityOstatni = false;
        }

        private void OnClickSaveBtn(object sender, RoutedEventArgs e)
        {
            using (var context = new LearDataAllEntities())
            {
                var selectedAkcniPlan =
                    context.AkcniPlan.SingleOrDefault(ap => ap.AkcniPlanID == _akcniPlan.AkcniPlanID);

                if (selectedAkcniPlan == null) return;

                selectedAkcniPlan.ProjektID = _akcniPlan.ProjektID;
                selectedAkcniPlan.Zadavatel1ID = _akcniPlan.Zadavatel1ID;
                selectedAkcniPlan.Zadavatel2ID = _akcniPlan.Zadavatel2ID;
                selectedAkcniPlan.ZakaznikID = _akcniPlan.ZakaznikID;
                selectedAkcniPlan.Tema = _akcniPlan.Tema;
                selectedAkcniPlan.AudityOstatni = _akcniPlan.AudityOstatni;
                selectedAkcniPlan.CisloAP = _akcniPlan.CisloAP;

                if (_ukonceniPlanu != null && _ukonceniPlanu.AkcniPlanID != 0) context.UkonceniAP.Add(_ukonceniPlanu);

                context.SaveChanges();
            }
            
            Close();
        }

        private void OnClickCloseBtn(object sender, RoutedEventArgs e)
        {
            Close();
            CloseBtn.IsCancel = true;
        }
    }
}