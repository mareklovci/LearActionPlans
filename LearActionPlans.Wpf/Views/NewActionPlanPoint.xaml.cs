using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LearActionPlans.Wpf.Models;
using LearActionPlans.Wpf.Utilities;

// ReSharper disable IdentifierTypo

namespace LearActionPlans.Wpf.Views
{
    /// <summary>
    /// Interaction logic for NewActionPlanPoint.xaml
    /// </summary>
    public partial class NewActionPlanPoint
    {
        private readonly BodAP _bodAp;

        public NewActionPlanPoint(AkcniPlan akcniPlan)
        {
            InitializeComponent();
            _bodAp = new BodAP
            {
                AkcniPlanID = akcniPlan.AkcniPlanID
            };

            using (var context = new LearDataAllEntities())
            {
                var whyMadeQuery = (from z in context.Akce
                    where z.Storno == false && z.Typ == ActionTypes.Wm.ToString()
                    select z).ToList();
                if (whyMadeQuery.Any()) ListWhyMade.ItemsSource = whyMadeQuery;

                var whyShippedQuery = (from z in context.Akce
                    where z.Storno == false && z.Typ == ActionTypes.Ws.ToString()
                    select z).ToList();
                if (whyShippedQuery.Any()) ListWhyShipped.ItemsSource = whyShippedQuery;
            }
        }

        private void OnClickDeleteBtn(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void NewActionBtn_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SavePointBtn_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnClickCloseBtn(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ResponsibleAuthority1_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ResponsibleAuthority2_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CorrectiveActionsField_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void EffectivityDatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DepartmentComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OpenFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TypeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SaveActionBtn_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddDeadlineBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new AddDeadlineView(new Akce());
            win.Show();
        }
    }
}