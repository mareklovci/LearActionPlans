using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LearActionPlans.DataMappers;
using LearActionPlans.Models;

namespace LearActionPlans.Views
{
    public partial class FormAdmin : Form
    {
        private readonly EmployeeRepository employeeRepository;
        private readonly DepartmentRepository departmentRepository;

        private Emploee selectedZamestnanec;
        private Oddeleni selectedOddeleni = null;

        private bool oldAdmin;

        public FormAdmin(
            EmployeeRepository employeeRepository,
            DepartmentRepository departmentRepository)
        {
            this.employeeRepository = employeeRepository;
            this.departmentRepository = departmentRepository;

            this.InitializeComponent();

            this.RadioButtonNovyZamestnanec.Checked = true;
            this.RadioButtonAktualizaceZamestnance.Checked = false;
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            this.NaplnitComboBoxZamestnanci();

            if (this.RadioButtonNovyZamestnanec.Checked)
            {
                this.labelSeznamZamestnancu.Enabled = false;
                this.ComboBoxZamestnanci.Enabled = false;

                this.textBoxKrestniJmeno.Text = string.Empty;
                this.textBoxPrijmeni.Text = string.Empty;
                this.textBoxLogin.Text = string.Empty;
                this.textBoxEmail.Text = string.Empty;
            }

            this.NaplnitComboBoxOddeleni();
        }

        public class Oddeleni
        {
            private int OddeleniId { get; }
            private string Nazev { get; }

            public Oddeleni(int oddeleniId, string nazev)
            {
                this.OddeleniId = oddeleniId;
                this.Nazev = nazev;
            }
        }

        private void NaplnitComboBoxOddeleni()
        {
            var oddeleni = this.departmentRepository.GetOddeleniOriginallyViewModel().ToList();

            if (oddeleni.Count == 0)
            {
                return;
            }

            var odd = new List<Oddeleni> {new Oddeleni(0, "(select department)")};
            odd.AddRange(oddeleni.Select(o => new Oddeleni(o.Id, o.Nazev)));

            this.comboBoxOddeleniZamestnanci.DataSource = odd;
            this.comboBoxOddeleniZamestnanci.DisplayMember = "Nazev";
            this.comboBoxOddeleniZamestnanci.ValueMember = "OddeleniId";
            this.comboBoxOddeleniZamestnanci.SelectedIndex = 0;
        }

        private void NaplnitComboBoxZamestnanci()
        {
            var zamestnanci = this.employeeRepository.GetEmployeesOriginalViewModel().ToList();

            if (zamestnanci.Count == 0)
            {
                return;
            }

            var zam = new List<Emploee> {new Emploee("(select employee)", null, null, 0, null, null, false, 0, 0)};

            zam.AddRange(zamestnanci.Select(z => new Emploee(z.Prijmeni + " " + z.Jmeno, z.Jmeno, z.Prijmeni,
                z.Id, z.PrihlasovaciJmeno, z.Email, z.AdminAP, z.OddeleniId, z.StavObjektu)));

            this.ComboBoxZamestnanci.DataSource = zam;
            this.ComboBoxZamestnanci.DisplayMember = "CeleJmeno";
            this.ComboBoxZamestnanci.ValueMember = "ZamestnanecId";
            this.ComboBoxZamestnanci.SelectedIndex = 0;
        }

        private void RadioButtonNovyZamestnanec_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.RadioButtonNovyZamestnanec.Checked)
            {
                return;
            }

            this.labelSeznamZamestnancu.Enabled = false;
            this.ComboBoxZamestnanci.SelectedValue = 0;
            this.ComboBoxZamestnanci.Enabled = false;
            this.comboBoxOddeleniZamestnanci.SelectedValue = 0;

            this.radioButtonAktivni.Checked = false;
            this.radioButtonNeaktivni.Checked = false;
            this.radioButtonOdstranen.Checked = false;

            this.checkBoxAdmin.Checked = false;

            this.textBoxKrestniJmeno.Text = string.Empty;
            this.textBoxPrijmeni.Text = string.Empty;
            this.textBoxLogin.Text = string.Empty;
            this.textBoxEmail.Text = string.Empty;
        }

        private void RadioButtonAktualizaceZamestnance_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.RadioButtonAktualizaceZamestnance.Checked)
            {
                return;
            }

            this.labelSeznamZamestnancu.Enabled = true;
            this.ComboBoxZamestnanci.SelectedValue = 0;
            this.ComboBoxZamestnanci.Enabled = true;
            this.comboBoxOddeleniZamestnanci.SelectedValue = 0;

            this.radioButtonAktivni.Checked = false;
            this.radioButtonNeaktivni.Checked = false;
            this.radioButtonOdstranen.Checked = false;

            this.checkBoxAdmin.Checked = false;

            this.textBoxKrestniJmeno.Text = string.Empty;
            this.textBoxPrijmeni.Text = string.Empty;
            this.textBoxLogin.Text = string.Empty;
            this.textBoxEmail.Text = string.Empty;
        }

        private void ComboBoxZamestnanci_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender is ComboBox combo))
            {
                return;
            }

            this.selectedZamestnanec = combo.SelectedItem as Emploee;
            if (this.selectedZamestnanec == null) { }
            else
            {
                this.textBoxKrestniJmeno.Text = this.selectedZamestnanec.Jmeno;
                this.textBoxPrijmeni.Text = this.selectedZamestnanec.Prijmeni;
                this.textBoxLogin.Text = this.selectedZamestnanec.Login;
                this.textBoxEmail.Text = this.selectedZamestnanec.Email;
                if (this.selectedZamestnanec.AdminAP == false)
                {
                    this.checkBoxAdmin.Checked = false;
                    this.oldAdmin = false;
                }
                else
                {
                    this.checkBoxAdmin.Checked = true;
                    this.oldAdmin = true;
                }

                switch (this.selectedZamestnanec.StavObjektu)
                {
                    case 0:
                        this.radioButtonAktivni.Checked = false;
                        this.radioButtonNeaktivni.Checked = false;
                        this.radioButtonOdstranen.Checked = false;
                        break;
                    case 1:
                        this.radioButtonAktivni.Checked = true;
                        this.radioButtonNeaktivni.Checked = false;
                        this.radioButtonOdstranen.Checked = false;
                        break;
                    case 2:
                        this.radioButtonAktivni.Checked = false;
                        this.radioButtonNeaktivni.Checked = true;
                        this.radioButtonOdstranen.Checked = false;
                        break;
                    case 3:
                        this.radioButtonAktivni.Checked = false;
                        this.radioButtonNeaktivni.Checked = false;
                        this.radioButtonOdstranen.Checked = true;
                        break;
                }

                this.comboBoxOddeleniZamestnanci.SelectedValue = this.selectedZamestnanec.OddeleniId;
            }
        }

        private bool KontrolaZamestnance()
        {
            bool ulozit;

            if (this.textBoxKrestniJmeno.Text == string.Empty)
            {
                MessageBox.Show(@"You must fill in the item 'First name'.", @"Notice", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                ulozit = false;
            }
            else if (this.textBoxPrijmeni.Text == string.Empty)
            {
                MessageBox.Show(@"You must fill in the item 'Last name'.", @"Notice", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                ulozit = false;
            }
            else if (this.textBoxLogin.Text == string.Empty)
            {
                MessageBox.Show(@"You must fill in the item 'Login name'.", @"Notice", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                ulozit = false;
            }
            else if (this.textBoxEmail.Text == string.Empty)
            {
                MessageBox.Show(@"You must fill in the item 'Email'.", @"Notice", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                ulozit = false;
            }
            else if (this.radioButtonAktivni.Checked == false && this.radioButtonNeaktivni.Checked == false &&
                     this.radioButtonOdstranen.Checked == false)
            {
                MessageBox.Show(@"You must fill in the item 'State'.", @"Notice", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                ulozit = false;
            }
            else if (Convert.ToInt16(this.comboBoxOddeleniZamestnanci.SelectedValue) == 0)
            {
                MessageBox.Show(@"You must fill in the item 'Department'.", @"Notice", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                ulozit = false;
            }
            else
            {
                ulozit = true;
            }

            return ulozit;
        }

        private void ButtonUlozit_MouseClick(object sender, MouseEventArgs e)
        {
            byte stav = 0;

            if (!this.KontrolaZamestnance())
            {
                return;
            }

            var pokracovat = true;

            if (((this.radioButtonNeaktivni.Checked || this.radioButtonOdstranen.Checked) && this.oldAdmin) ||
                (this.checkBoxAdmin.Checked == false && this.oldAdmin))
            {
                //pokud nyní bude v databázi jeden admin, nemohu ho odebrat, protože je jediný
                var pocetAdmin = this.employeeRepository.GetPocetAdmin();

                if (pocetAdmin.Count() <= 1)
                {
                    //nelze odstranit admina, protože je poslední
                    // Vybraný zaměstnanec je poslední admin.
                    // Nejdřív vytvořte nového.
                    MessageBox.Show(
                        @"The selected employee is the last admin." + (char)10 + @"Create a new one first.", @"Notice",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pokracovat = false;
                }
            }

            //nejdřív kontrola, jestli ukládám nového zaměstnance, který již v databázi existuje
            //kontrola login name
            if (!pokracovat)
            {
                return;
            }

            if (this.employeeRepository.VybranyZamestnanec(this.textBoxLogin.Text) == false)
            {
                MessageBox.Show(@"The new employee already exists.", @"Notice", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                if (this.radioButtonAktivni.Checked)
                {
                    stav = 1;
                }

                if (this.radioButtonNeaktivni.Checked)
                {
                    stav = 2;
                }

                if (this.radioButtonOdstranen.Checked)
                {
                    stav = 3;
                }

                if (this.RadioButtonNovyZamestnanec.Checked)
                {
                    this.employeeRepository.InsertZamestnanec(this.textBoxKrestniJmeno.Text,
                        this.textBoxPrijmeni.Text, this.textBoxLogin.Text,
                        Convert.ToInt32(this.comboBoxOddeleniZamestnanci.SelectedValue), this.textBoxEmail.Text,
                        this.checkBoxAdmin.Checked,
                        stav
                    );

                    MessageBox.Show(@"A new employee has been saved.", @"Notice", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                if (this.RadioButtonAktualizaceZamestnance.Checked)
                {
                    this.employeeRepository.UpdateZamestnanec(this.selectedZamestnanec.ZamestnanecId,
                        this.textBoxKrestniJmeno.Text, this.textBoxPrijmeni.Text, this.textBoxLogin.Text,
                        Convert.ToInt32(this.comboBoxOddeleniZamestnanci.SelectedValue), this.textBoxEmail.Text,
                        this.checkBoxAdmin.Checked,
                        stav
                    );

                    MessageBox.Show(@"The selected employee has been saved.", @"Notice", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                this.NaplnitComboBoxZamestnanci();

                this.ComboBoxZamestnanci.SelectedValue = 0;
                this.comboBoxOddeleniZamestnanci.SelectedValue = 0;

                this.radioButtonAktivni.Checked = false;
                this.radioButtonNeaktivni.Checked = false;
                this.radioButtonOdstranen.Checked = false;

                this.checkBoxAdmin.Checked = false;

                this.textBoxKrestniJmeno.Text = string.Empty;
                this.textBoxPrijmeni.Text = string.Empty;
                this.textBoxLogin.Text = string.Empty;
                this.textBoxEmail.Text = string.Empty;
            }
        }

        private void ButtonClose_MouseClick(object sender, MouseEventArgs e) => this.Close();
    }
}
