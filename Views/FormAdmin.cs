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
        //protected IList<Emploee> itemsEmploee = new BindingList<Emploee>();

        private Emploee selectedZamestnanec;
        private Projekt selectedProjekt;
        private Oddeleni selectedOddeleni;

        private bool oldAdmin;

        public FormAdmin()
        {
            this.InitializeComponent();

            selectedZamestnanec = null;
            selectedProjekt = null;
            selectedOddeleni = null;

            this.RadioButtonNovyZamestnanec.Checked = true;
            this.RadioButtonAktualizaceZamestnance.Checked = false;

            this.RadioButtonNovyProjekt.Checked = true;
            this.RadioButtonAktualizaceProjektu.Checked = false;
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            // karta zaměstnanci
            this.NaplnitComboBoxZamestnanci();
            this.NaplnitComboBoxOddeleni();

            // karta projekty
            _ = this.NaplnitComboBoxProjekty();

            this.InitZamestnanci();
            this.InitProjekty();
        }

        private void InitZamestnanci()
        {
            if (this.RadioButtonNovyZamestnanec.Checked)
            {
                this.labelSeznamZamestnancu.Enabled = false;
                this.ComboBoxZamestnanci.Enabled = false;

                this.textBoxKrestniJmeno.Text = string.Empty;
                this.textBoxPrijmeni.Text = string.Empty;
                this.textBoxLogin.Text = string.Empty;
                this.textBoxEmail.Text = string.Empty;
            }
        }

        private void InitProjekty()
        {
            if (this.RadioButtonNovyProjekt.Checked)
            {
                this.labelSeznamProjektu.Enabled = false;
                this.ComboBoxProjekty.Enabled = false;

                this.textBoxNazevProjektu.Text = string.Empty;
            }
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

        public class Projekt
        {
            public int ProjektId { get; set; }
            public string NazevProjektu { get; set; }
            public byte StavObjektu { get; set; }

            public Projekt(int projektId, string nazevProjektu, byte stavObjektu)
            {
                this.ProjektId = projektId;
                this.NazevProjektu = nazevProjektu;
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

                this.radioButtonAktivniZam.Checked = false;
                this.radioButtonNeaktivniZam.Checked = false;
                this.radioButtonOdstranenZam.Checked = false;

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

                this.radioButtonAktivniZam.Checked = false;
                this.radioButtonNeaktivniZam.Checked = false;
                this.radioButtonOdstranenZam.Checked = false;

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
                    this.radioButtonAktivniZam.Checked = false;
                    this.radioButtonNeaktivniZam.Checked = false;
                    this.radioButtonOdstranenZam.Checked = false;
                }
                else if (this.selectedZamestnanec.StavObjektu == 1)
                {
                    this.radioButtonAktivniZam.Checked = true;
                    this.radioButtonNeaktivniZam.Checked = false;
                    this.radioButtonOdstranenZam.Checked = false;
                }
                else if (this.selectedZamestnanec.StavObjektu == 2)
                {
                    this.radioButtonAktivniZam.Checked = false;
                    this.radioButtonNeaktivniZam.Checked = true;
                    this.radioButtonOdstranenZam.Checked = false;
                }
                else if (this.selectedZamestnanec.StavObjektu == 3)
                {
                    this.radioButtonAktivniZam.Checked = false;
                    this.radioButtonNeaktivniZam.Checked = false;
                    this.radioButtonOdstranenZam.Checked = true;
                }

                this.comboBoxOddeleniZamestnanci.SelectedValue = this.selectedZamestnanec.OddeleniId;
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
            else if (this.radioButtonAktivniZam.Checked == false && this.radioButtonNeaktivniZam.Checked == false && this.radioButtonOdstranenZam.Checked == false)
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

        private void ButtonUlozitZamestnance_MouseClick(object sender, MouseEventArgs e)
        {
            byte stav = 0;

            if (this.KontrolaZamestnance() == false)
            { }
            else
            {
                var pokracovat = true;
                if (((this.radioButtonNeaktivniZam.Checked == true || this.radioButtonOdstranenZam.Checked == true) && this.oldAdmin == true) || (this.checkBoxAdmin.Checked == false && this.oldAdmin == true))
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
                        if (this.radioButtonAktivniZam.Checked == true)
                        {
                            stav = 1;
                        }

                        if (this.radioButtonNeaktivniZam.Checked == true)
                        {
                            stav = 2;
                        }

                        if (this.radioButtonOdstranenZam.Checked == true)
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

                        this.radioButtonAktivniZam.Checked = false;
                        this.radioButtonNeaktivniZam.Checked = false;
                        this.radioButtonOdstranenZam.Checked = false;

                        this.checkBoxAdmin.Checked = false;

                        this.textBoxKrestniJmeno.Text = string.Empty;
                        this.textBoxPrijmeni.Text = string.Empty;
                        this.textBoxLogin.Text = string.Empty;
                        this.textBoxEmail.Text = string.Empty;
                    }
                }
            }
        }

        private bool NaplnitComboBoxProjekty()
        {
            var projekty = AdminViewModel.GetProjekty().ToList();

            if (projekty.Count == 0)
            {
                return false;
            }
            else
            {
                var proj = new List<Projekt>
                {
                    new Projekt(0, "(select project)", 0)
                };

                foreach (var p in projekty)
                {
                    proj.Add(new Projekt(p.ProjektId, p.NazevProjektu, p.StavObjektuProjekt));
                }

                this.ComboBoxProjekty.DataSource = null;
                this.ComboBoxProjekty.DataSource = proj;
                this.ComboBoxProjekty.DisplayMember = "NazevProjektu";
                this.ComboBoxProjekty.ValueMember = "ProjektId";
                this.ComboBoxProjekty.SelectedIndex = 0;

                this.ComboBoxProjekty.SelectedIndexChanged +=
                    new System.EventHandler(this.ComboBoxProjekty_SelectedIndexChanged);

                return true;
            }
        }

        private void RadioButtonNovyProjekt_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioButtonNovyProjekt.Checked == true)
            {
                this.labelSeznamProjektu.Enabled = false;
                this.ComboBoxProjekty.SelectedValue = 0;
                this.ComboBoxProjekty.Enabled = false;

                this.radioButtonAktivniProjekt.Checked = false;
                this.radioButtonNeaktivniProjekt.Checked = false;

                this.textBoxNazevProjektu.Text = string.Empty;
            }
        }

        private void RadioButtonAktualizaceProjektu_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioButtonAktualizaceProjektu.Checked == true)
            {
                this.labelSeznamProjektu.Enabled = true;
                this.ComboBoxProjekty.SelectedValue = 0;
                this.ComboBoxProjekty.Enabled = true;

                this.radioButtonAktivniProjekt.Checked = false;
                this.radioButtonNeaktivniProjekt.Checked = false;

                this.textBoxNazevProjektu.Text = string.Empty;
            }
        }

        private void ComboBoxProjekty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender is ComboBox combo))
            {
                return;
            }

            this.selectedProjekt = combo.SelectedItem as Projekt;
            if (this.selectedProjekt == null)
            {
                return;
            }

            if (this.selectedProjekt.ProjektId == 0)
            {
                this.textBoxNazevProjektu.Text = string.Empty;
            }
            else
            {
                this.textBoxNazevProjektu.Text = this.selectedProjekt.NazevProjektu;
            }


            if (this.selectedProjekt.StavObjektu == 0)
            {
                this.radioButtonAktivniProjekt.Checked = false;
                this.radioButtonNeaktivniProjekt.Checked = false;
            }
            else if (this.selectedProjekt.StavObjektu == 1)
            {
                this.radioButtonAktivniProjekt.Checked = true;
                this.radioButtonNeaktivniProjekt.Checked = false;
            }
            else if (this.selectedProjekt.StavObjektu == 2)
            {
                this.radioButtonAktivniProjekt.Checked = false;
                this.radioButtonNeaktivniProjekt.Checked = true;
            }
        }

        private void ButtonUlozitProjekt_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.textBoxNazevProjektu.Text == string.Empty)
            {
                MessageBox.Show("The Project name field must be filled in.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.RadioButtonNovyProjekt.Checked == true)
            {
                // uložím nový projekt
                if (this.radioButtonAktivniProjekt.Checked == false && this.radioButtonNeaktivniProjekt.Checked == false)
                {
                    _ = MessageBox.Show("The Active or Inactive state must be selected.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ProjektyDataMapper.InsertProjekt(this.textBoxNazevProjektu.Text);

                    this.ComboBoxProjekty.SelectedIndexChanged -=
                        new System.EventHandler(this.ComboBoxProjekty_SelectedIndexChanged);

                    _ = this.NaplnitComboBoxProjekty();

                    this.textBoxNazevProjektu.Text = string.Empty;
                    this.radioButtonAktivniProjekt.Checked = false;
                    this.radioButtonNeaktivniProjekt.Checked = false;
                }
            }

            if (this.RadioButtonAktualizaceProjektu.Checked == true)
            {
                // aktualizuji stávající projekt
                var podminka = this.radioButtonAktivniProjekt.Checked == true;
                var aktivniNeaktivni = podminka ? (byte)1 : (byte)2;

                ProjektyDataMapper.UpdateProjekt(Convert.ToInt32(this.ComboBoxProjekty.SelectedValue), this.textBoxNazevProjektu.Text, aktivniNeaktivni);

                this.ComboBoxProjekty.SelectedIndexChanged -=
                    new System.EventHandler(this.ComboBoxProjekty_SelectedIndexChanged);

                _ = this.NaplnitComboBoxProjekty();

                this.textBoxNazevProjektu.Text = string.Empty;
                this.radioButtonAktivniProjekt.Checked = false;
                this.radioButtonNeaktivniProjekt.Checked = false;
            }
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e) => this.Close();
    }
}
