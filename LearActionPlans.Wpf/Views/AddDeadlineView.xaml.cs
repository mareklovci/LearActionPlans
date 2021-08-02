using System.Windows;
using System.Windows.Controls;
using LearActionPlans.Wpf.Models;
// ReSharper disable IdentifierTypo

namespace LearActionPlans.Wpf.Views
{
    public partial class AddDeadlineView : Window
    {
        private readonly UkonceniAkce _ukonceniAkce;
        
        public AddDeadlineView(Akce akce)
        {
            InitializeComponent();
            _ukonceniAkce = new UkonceniAkce
            {
                AkceID = akce.AkceID
            };
        }

        private void DeadlineDatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDate = DeadlineDatePicker.SelectedDate;
            if (selectedDate.HasValue) _ukonceniAkce.DatumUkonceni = selectedDate.Value;
        }

        private void DeadlineNoteField_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var newText = DeadlineNoteField.Text;
            _ukonceniAkce.Poznamka = newText;
        }

        private void OnClickSaveBtn(object sender, RoutedEventArgs e)
        {
            using (var context = new LearDataAllEntities())
            {
                var actionDealine = context.Set<UkonceniAkce>();
                actionDealine.Add(_ukonceniAkce);
                context.SaveChanges();
            }

            Close();
        }

        private void OnClickCloseBtn(object sender, RoutedEventArgs e) => Close();
    }
}