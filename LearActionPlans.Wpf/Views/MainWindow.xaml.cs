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

        private void OnClick1(object sender, RoutedEventArgs e)
        {
            var win2 = new NewActionPlanView();
            win2.Show();
        }
    }
}