﻿using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using LearActionPlans.Wpf.Models;
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo

namespace LearActionPlans.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ActionPlansFilterView.xaml
    /// </summary>
    public partial class ActionPlansFilterView
    {
        private readonly CollectionView _view;
        
        public ActionPlansFilterView()
        {
            InitializeComponent();

            using (var context = new LearDataAllEntities())
            {
                // Populate list
                var query = (from ap in context.AkcniPlan select ap).ToList();
                ActionPlansList.ItemsSource = query;

                // Select only employees which are authorities
                var allEmployeeIds1 = query.Select(akcniPlan => akcniPlan.Zadavatel1.ZamestnanecID).ToList();
                var authority1Query = (from z in context.Zamestnanec
                    where z.JeZamestnanec && allEmployeeIds1.Contains(z.ZamestnanecID)
                    select z).ToList();
                Authority1ComboBox.ItemsSource = authority1Query;

                // Select only employees which are authorities
                var allEmployeeIds2 = (from akcniPlan in query 
                    where akcniPlan.Zadavatel2ID != null 
                    select (int) akcniPlan.Zadavatel2ID).ToList();
                var authority2Query = (from z in context.Zamestnanec
                    where allEmployeeIds2.Contains(z.ZamestnanecID)
                    select z).ToList();
                Authority2ComboBox.ItemsSource = authority2Query;
            }
            
            _view = (CollectionView) CollectionViewSource.GetDefaultView(ActionPlansList.ItemsSource);
        }

        private bool Authority1Filter(object obj)
        {
            if (!(obj is AkcniPlan item)) return false;
            var empl = (Zamestnanec) Authority1ComboBox.SelectedItem;
            return empl == null || item.Zadavatel1.ZamestnanecID == empl.ZamestnanecID;
        }
        
        private bool Authority2Filter(object obj)
        {
            if (!(obj is AkcniPlan item)) return false;
            var empl = (Zamestnanec) Authority2ComboBox.SelectedItem;
            return empl == null || item.Zadavatel2 == null || item.Zadavatel2.ZamestnanecID == empl.ZamestnanecID;
        }

        private void Authority1ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Reset the other ComboBox
            Authority2ComboBox.SelectedIndex = -1;
            
            _view.Filter += Authority1Filter;
            _view.Refresh();
        }
        
        private void Authority2ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Reset the other ComboBox
            Authority1ComboBox.SelectedIndex = -1;
            
            _view.Filter += Authority2Filter;
            _view.Refresh();
        }

        private void StornoBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var akcniPlan = (AkcniPlan) ActionPlansList.SelectedItem;

            using (var context = new LearDataAllEntities())
            {
                var selectedAkcniPlan = (from ap in context.AkcniPlan
                    where ap.AkcniPlanID == akcniPlan.AkcniPlanID
                    select ap).FirstOrDefault();

                if (selectedAkcniPlan != null) context.AkcniPlan.Remove(selectedAkcniPlan);
                context.SaveChanges();
            }
        }

        private void UpdateBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new UpdateActionPlanView((AkcniPlan) ActionPlansList.SelectedItem);
            win.Show();
        }
    }
}