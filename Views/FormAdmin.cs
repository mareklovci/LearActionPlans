using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using LearActionPlans.DataMappers;
using LearActionPlans.ViewModels;

namespace LearActionPlans.Views
{
    public partial class FormAdmin : Form
    {
        protected IList<Emploee> itemsEmploee = new BindingList<Emploee>();

        private Emploee selectedZamestnanec = null;
        private Oddeleni selectedOddeleni = null;

        private bool oldAdmin;

        public FormAdmin()
        {
            this.InitializeComponent();

            this.RadioButtonNovyZamestnanec.Checked = true;
            this.RadioButtonAktualizaceZamestnance.Checked = false;
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            //init zaměstnanci
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

            //init zaměstnanci
        }

        public class Oddeleni
        {
            public int OddeleniId { get; set; }
            public string Nazev { get; set; }

            public Oddeleni(int oddeleniId, string nazev)
            {
                this.OddeleniId = oddeleniId;
                this.Nazev = nazev;
            }
        }

        public class Emploee
        {
            public string CeleJmeno { get; set; }
            public string Jmeno { get; set; }
            public string Prijmeni { get; set; }
            public int ZamestnanecId { get; set; }
            public string Email { get; set; }
            public string Login { get; set; }
            public bool AdminAP { get; set; }
            public int OddeleniId { get; set; }
            public byte StavObjektu { get; set; }

            public Emploee(string celeJmeno, string jmeno, string  prijmeni, int zamestnanecId, string login, string email, bool adminAP, int oddeleniId, byte stavObjektu)
            {
                this.CeleJmeno = celeJmeno;
                this.Jmeno = jmeno;
                this.Prijmeni = prijmeni;
                this.ZamestnanecId = zamestnanecId;
                this.Login = login;
                this.Email = email;
                this.AdminAP = adminAP;
                this.OddeleniId = oddeleniId;
                this.StavObjektu = stavObjektu;
            }
        }

        private bool NaplnitComboBoxOddeleni()
        {
            var oddeleni = AdminViewModel.GetOddeleni().ToList();

            if (oddeleni.Count == 0)
            {
                return false;
            }
            else
            {
                var odd = new List<Oddeleni>
                {
                    new Oddeleni(0, "(select department)")
                };

                foreach (var o in oddeleni)
                {
                    odd.Add(new Oddeleni(o.OddeleniId, o.NazevOddeleni));
                }

                this.comboBoxOddeleniZamestnanci.DataSource = odd;
                this.comboBoxOddeleniZamestnanci.DisplayMember = "Nazev";
                this.comboBoxOddeleniZamestnanci.ValueMember = "OddeleniId";
                this.comboBoxOddeleniZamestnanci.SelectedIndex = 0;
                return true;
            }
        }

        private bool NaplnitComboBoxZamestnanci()
        {
            var zamestnanci = AdminViewModel.GetZamestnanci().ToList();

            if (zamestnanci.Count == 0)
            {
                return false;
            }
            else
            {
                var zam = new List<Emploee>
                {
                    new Emploee("(select employee)", null, null, 0, null, null, false, 0, 0)
                };

                foreach (var z in zamestnanci)
                {
                    zam.Add(new Emploee(z.Prijmeni + " " + z.Jmeno, z.Jmeno, z.Prijmeni, z.ZamestnanecId, z.Login, z.Email, z.AdminAP, z.OddeleniId, z.StavObjektu));
                }

                this.ComboBoxZamestnanci.DataSource = zam;
                this.ComboBoxZamestnanci.DisplayMember = "CeleJmeno";
                this.ComboBoxZamestnanci.ValueMember = "ZamestnanecId";
                this.ComboBoxZamestnanci.SelectedIndex = 0;
                return true;
            }
        }

        private void RadioButtonNovyZamestnanec_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioButtonNovyZamestnanec.Checked == true)
            {
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
        }

        private void RadioButtonAktualizaceZamestnance_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioButtonAktualizaceZamestnance.Checked == true)
            {
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

                if (this.selectedZamestnanec.StavObjektu == 0)
                {
                    this.radioButtonAktivni.Checked = false;
                    this.radioButtonNeaktivni.Checked = false;
                    this.radioButtonOdstranen.Checked = false;
                }
                else if (this.selectedZamestnanec.StavObjektu == 1)
                {
                    this.radioButtonAktivni.Checked = true;
                    this.radioButtonNeaktivni.Checked = false;
                    this.radioButtonOdstranen.Checked = false;
                }
                else if (this.selectedZamestnanec.StavObjektu == 2)
                {
                    this.radioButtonAktivni.Checked = false;
                    this.radioButtonNeaktivni.Checked = true;
                    this.radioButtonOdstranen.Checked = false;
                }
                else if (this.selectedZamestnanec.StavObjektu == 3)
                {
                    this.radioButtonAktivni.Checked = false;
                    this.radioButtonNeaktivni.Checked = false;
                    this.radioButtonOdstranen.Checked = true;
                }

                this.comboBoxOddeleniZamestnanci.SelectedValue = this.selectedZamestnanec.OddeleniId;
                //if (combo.SelectedIndex == 0)
                //{
                //    emailDopravce1 = null;
                //    emailDopravce = false;
                //}
                //else
                //{
                //    emailDopravce1 = selectedDopravce.Email1.ToString();
                //    emailDopravce = true;
                //}
            }
        }

        private bool KontrolaZamestnance()
        {
            bool ulozit;

            if (this.textBoxKrestniJmeno.Text == string.Empty)
            {
                MessageBox.Show("You must fill in the item 'First nam'.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ulozit = false;
            }
            else if (this.textBoxPrijmeni.Text == string.Empty)
            {
                MessageBox.Show("You must fill in the item 'Last name'.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ulozit = false;
            }
            else if (this.textBoxLogin.Text == string.Empty)
            {
                MessageBox.Show("You must fill in the item 'Login name'.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ulozit = false;
            }
            else if (this.textBoxEmail.Text == string.Empty)
            {
                MessageBox.Show("You must fill in the item 'Email'.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ulozit = false;
            }
            else if (this.radioButtonAktivni.Checked == false && this.radioButtonNeaktivni.Checked == false && this.radioButtonOdstranen.Checked == false)
            {
                MessageBox.Show("You must fill in the item 'State'.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ulozit = false;
            }
            else if (Convert.ToInt16(this.comboBoxOddeleniZamestnanci.SelectedValue) == 0)
            {
                MessageBox.Show("You must fill in the item 'Department'.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (this.KontrolaZamestnance() == false) { }
            else
            {
                var pokracovat = true;
                if (((this.radioButtonNeaktivni.Checked == true || this.radioButtonOdstranen.Checked == true) && this.oldAdmin == true) || (this.checkBoxAdmin.Checked == false && this.oldAdmin == true))
                {
                    //pokud nyní bude v databázi jeden admin, nemohu ho odebrat, protože je jediný
                    var pocetAdmin = AdminViewModel.GetPocetAdmin();

                    if (pocetAdmin.Count() <= 1)
                    {
                        //nelze odstranit admina, protože je poslední
                        // Vybraný zaměstnanec je poslední admin.
                        // Nejdřív vytvořte nového.
                        MessageBox.Show("The selected employee is the last admin." + (char)10 + "Create a new one first.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pokracovat = false;
                    }
                }
                //nejdřív kontrola, jestli ukládám nového zaměstnance, který již v databázi existuje
                //kontrola login name
                if (pokracovat == true)
                {
                    if (AdminViewModel.VybranyZamestnanec(this.textBoxLogin.Text) == false)
                    {
                        MessageBox.Show("The new employee already exists.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (this.radioButtonAktivni.Checked == true)
                        {
                            stav = 1;
                        }

                        if (this.radioButtonNeaktivni.Checked == true)
                        {
                            stav = 2;
                        }

                        if (this.radioButtonOdstranen.Checked == true)
                        {
                            stav = 3;
                        }

                        if (this.RadioButtonNovyZamestnanec.Checked == true)
                        {
                            ZamestnanciDataMapper.InsertZamestnanec(this.textBoxKrestniJmeno.Text, this.textBoxPrijmeni.Text, this.textBoxLogin.Text,
                                Convert.ToInt32(this.comboBoxOddeleniZamestnanci.SelectedValue), this.textBoxEmail.Text, this.checkBoxAdmin.Checked,
                                stav
                                );

                            MessageBox.Show("A new employee has been saved.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        if (this.RadioButtonAktualizaceZamestnance.Checked == true)
                        {
                            ZamestnanciDataMapper.UpdateZamestnanec(this.selectedZamestnanec.ZamestnanecId, this.textBoxKrestniJmeno.Text, this.textBoxPrijmeni.Text, this.textBoxLogin.Text,
                                Convert.ToInt32(this.comboBoxOddeleniZamestnanci.SelectedValue), this.textBoxEmail.Text, this.checkBoxAdmin.Checked,
                                stav
                                );

                            MessageBox.Show("The selected employee has been saved.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            }
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
    }
}
