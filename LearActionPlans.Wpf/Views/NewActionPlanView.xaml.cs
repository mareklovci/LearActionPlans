using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LearActionPlans.Wpf.Models;

namespace LearActionPlans.Wpf.Views
{
    public partial class NewActionPlanView : Window
    {
        public NewActionPlanView()
        {
            InitializeComponent();
            
            var id = Guid.NewGuid();

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