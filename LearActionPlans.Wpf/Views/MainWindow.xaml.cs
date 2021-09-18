using System.Windows;

namespace LearActionPlans.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnNewActionPlan_Click(object sender, RoutedEventArgs e)
        {
            var win = new NewActionPlanView();
            win.Show();
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            var win = new ActionPlansFilterView();
            win.Show();
        }

        private void BtnEditable_Click(object sender, RoutedEventArgs e)
        {
            var win = new ActionPlansEditableView();
            win.Show();
        }
    }
}