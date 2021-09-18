using System.Collections.ObjectModel;
using System.Linq;
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
    public partial class ActionPlansEditableView
    {
        private readonly CollectionView _view;

        public ActionPlansEditableView()
        {
            InitializeComponent();

            using (var context = new LearDataAllEntities())
            {
                // Populate list
                var query = (from ap in context.AkcniPlan select ap).ToList();
                ActionPlansList.ItemsSource = query;
            }

            _view = (CollectionView)CollectionViewSource.GetDefaultView(ActionPlansList.ItemsSource);
        }

        private void StornoBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var akcniPlan = (AkcniPlan)ActionPlansList.SelectedItem;

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
            var win = new UpdateActionPlanView((AkcniPlan)ActionPlansList.SelectedItem);
            win.Closed += (s, eventArg) =>
            {
                _view.Refresh();
            };
            win.Show();
        }

        private void ActionPlansList_OnRowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var actionPlan = (AkcniPlan)e.Row.Item;
            
            using (var db = new LearDataAllEntities())
            {
                var result = db.AkcniPlan.SingleOrDefault(ap => ap.AkcniPlanID == actionPlan.AkcniPlanID);
                if (result == null) return;
                
                result.Zadavatel1.Jmeno = actionPlan.Zadavatel1.Jmeno;
                result.Zadavatel1.Prijmeni = actionPlan.Zadavatel1.Prijmeni;
                result.Tema = actionPlan.Tema;
                
                db.SaveChanges();
            }
        }
    }
}