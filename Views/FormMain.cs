using System;
using System.Windows.Forms;
using LearActionPlans.Utilities;

namespace LearActionPlans.Views
{
    public partial class FormMain : Form
    {
        private readonly ArgumentOptions argumentOptions;

        private readonly FormPrehledAp formActionPlanOverview;
        private readonly FormNovyAkcniPlan formNewActionPlan;
        private readonly FormAdmin formAdmin;
        private readonly FormVsechnyBodyAP formActionPlanPoints;
        private readonly FormSeznamPozadavku formRequirements;

        public FormMain(ArgumentOptions argumentOptions,
            FormPrehledAp formActionPlanOverview,
            FormNovyAkcniPlan formNewActionPlan,
            FormAdmin formAdmin,
            FormVsechnyBodyAP formActionPlanPoints,
            FormSeznamPozadavku formRequirements)
        {
            // Arguments
            this.argumentOptions = argumentOptions;

            // Forms
            this.formActionPlanOverview = formActionPlanOverview;
            this.formNewActionPlan = formNewActionPlan;
            this.formAdmin = formAdmin;
            this.formActionPlanPoints = formActionPlanPoints;
            this.formRequirements = formRequirements;

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

        private void ButtonActionPlanPoints_MouseClick(object sender, MouseEventArgs e) =>
            this.formActionPlanPoints.ShowDialog();

        private void ButtonRequirements_MouseClick(object sender, MouseEventArgs e) =>
            this.formRequirements.ShowDialog();

        private void ButtonAdmin_MouseClick(object sender, MouseEventArgs e) =>
            this.formAdmin.ShowDialog();

        private void ButtonLogin_MouseClick(object sender, MouseEventArgs e) { }
    }
}
