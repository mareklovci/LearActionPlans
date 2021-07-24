using System.Linq;
using System.Windows;
using LearActionPlans.Wpf.Models;

namespace LearActionPlans.Wpf.Views
{
    public partial class ListOfActionPlanPoints : Window
    {
        public ListOfActionPlanPoints()
        {
            InitializeComponent();
            
            // Populate list
            using (var context = new LearDataAllEntities())
            {
                var query = from ap in context.AkcniPlany select ap; 

                foreach (var ap in query)
                {
                    listView.Items.Add(ap);
                }
            }
        }
    }
}