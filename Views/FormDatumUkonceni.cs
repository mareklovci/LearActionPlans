using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

using LearActionPlans.ViewModels;

namespace LearActionPlans.Views
{
    public partial class FormDatumUkonceni : Form
    {
        public DateTime? ReturnValueDatum { get; set; }
        public string ReturnValuePoznamka { get; set; }

        public FormDatumUkonceni(DateTime? datum, string poznamka)
        {
            this.InitializeComponent();

            var podminkaDatum = datum == null;
            var podminkaPoznamka = poznamka == string.Empty;
            this.dateTimePickerDatumUkonceni.Value = podminkaDatum ? DateTime.Now : Convert.ToDateTime(datum);
            this.richTextBoxPoznamka.Text = podminkaPoznamka ? string.Empty : poznamka;
        }

        private void FormDatumUkonceni_Load(object sender, EventArgs e)
        {
        }

        private void ButtonOk_MouseClick(object sender, MouseEventArgs e)
        {
            this.ReturnValueDatum = this.dateTimePickerDatumUkonceni.Value;
            var podminka = string.IsNullOrWhiteSpace(Convert.ToString(this.richTextBoxPoznamka.Text)) == true;
            this.ReturnValuePoznamka = podminka ? string.Empty : this.richTextBoxPoznamka.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e) => this.Close();
    }
}
