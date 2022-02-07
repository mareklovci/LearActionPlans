using System;
using System.Windows.Forms;
using LearActionPlans.Utilities;

namespace LearActionPlans.Views
{
    public partial class FormMain : Form
    {
        private readonly ArgumentOptions argumentOptions;

        private readonly FormPrehledAP formActionPlanOverview;
        private readonly FormNovyAkcniPlan formNewActionPlan;
        private readonly FormAdmin formAdmin;

        public FormMain(ArgumentOptions argumentOptions,
            FormPrehledAP formActionPlanOverview,
            FormNovyAkcniPlan formNewActionPlan, FormAdmin formAdmin)
        {
            // Arguments
            this.argumentOptions = argumentOptions;

            // Forms
            this.formActionPlanOverview = formActionPlanOverview;
            this.formNewActionPlan = formNewActionPlan;
            this.formAdmin = formAdmin;

            // Initialize
            this.InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (!this.argumentOptions.RunWithoutParameters)
            {
                this.formActionPlanOverview.ShowDialog();
            }

            this.ButtonAdmin.Enabled = true;
        }

        private void ButtonNewActionPlan_MouseClick(object sender, MouseEventArgs e) =>
            this.formNewActionPlan.ShowDialog();

        private void ButtonFixActionPlan_MouseClick(object sender, MouseEventArgs e) =>
            this.formActionPlanOverview.ShowDialog();

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

        private void ButtonAdmin_MouseClick(object sender, MouseEventArgs e) => this.formAdmin.ShowDialog();

        private void ButtonLogin_MouseClick(object sender, MouseEventArgs e) { }
    }
}
