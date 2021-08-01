using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LearActionPlans.Wpf.Models;

namespace LearActionPlans.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ActionPlansFilterView.xaml
    /// </summary>
    public partial class ActionPlansFilterView : Window
    {
        public ActionPlansFilterView()
        {
            InitializeComponent();
            
            using (var context = new LearDataAllEntities())
            {
                // Populate list
                var query = (from ap in context.AkcniPlan select ap).ToList();
                ActionPlansList.ItemsSource = query;
            }
        }
    }
}
