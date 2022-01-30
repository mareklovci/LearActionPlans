using System;
using System.Windows.Forms;
using LearActionPlans.Utilities;

namespace LearActionPlans.Views
{
    public partial class FormMain : Form
    {
        private ArgumentOptions arguments;

        public FormMain(ArgumentOptions arguments)
        {
            this.InitializeComponent();
            this.arguments = arguments;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (!this.arguments.RunWithoutParameters)
            {
                using var form = new FormPrehledAP(this.arguments);
                form.ShowDialog();
            }

            this.ButtonAdmin.Enabled = true;
        }

        private void ButtonNovyAkcniPlan_MouseClick(object sender, MouseEventArgs e)
        {
            using var form = new FormNovyAkcniPlan();
            form.ShowDialog();
        }

        private void ButtonOpravaAkcnihoPlanu_MouseClick(object sender, MouseEventArgs e)
        {
            using var form = new FormPrehledAP(this.arguments);
            form.ShowDialog();
        }

        private void ButtonVsechnyBodyAP_MouseClick(object sender, MouseEventArgs e)
        {
            using var form = new FormVsechnyBodyAP();
            form.ShowDialog();
        }

        private void ButtonSeznamZadosti_MouseClick(object sender, MouseEventArgs e)
        {
            using var form = new FormSeznamPozadavku();
            form.ShowDialog();
        }

        private void ButtonAdmin_MouseClick(object sender, MouseEventArgs e)
        {
            using var form = new FormAdmin();
            var result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
            }
        }

        private void ButtonLogin_MouseClick(object sender, MouseEventArgs e) { }
    }
}
