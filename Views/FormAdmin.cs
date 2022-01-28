using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            InitializeComponent();

            RadioButtonNovyZamestnanec.Checked = true;
            RadioButtonAktualizaceZamestnance.Checked = false;
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            //init zaměstnanci
            NaplnitComboBoxZamestnanci();
            if (RadioButtonNovyZamestnanec.Checked == true)
            {
                labelSeznamZamestnancu.Enabled = false;
                ComboBoxZamestnanci.Enabled = false;

                textBoxKrestniJmeno.Text = string.Empty;
                textBoxPrijmeni.Text = string.Empty;
                textBoxLogin.Text = string.Empty;
                textBoxEmail.Text = string.Empty;
            }
            NaplnitComboBoxOddeleni();

            //init zaměstnanci
        }

        public class Oddeleni
        {
            public int OddeleniId { get; set; }
            public string Nazev { get; set; }

            public Oddeleni(int oddeleniId, string nazev)
            {
                OddeleniId = oddeleniId;
                Nazev = nazev;
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
                CeleJmeno = celeJmeno;
                Jmeno = jmeno;
                Prijmeni = prijmeni;
                ZamestnanecId = zamestnanecId;
                Login = login;
                Email = email;
                AdminAP = adminAP;
                OddeleniId = oddeleniId;
                StavObjektu = stavObjektu;
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
                List<Oddeleni> odd = new List<Oddeleni>
                {
                    new Oddeleni(0, "(select department)")
                };

                foreach (var o in oddeleni)
                {
                    odd.Add(new Oddeleni(o.OddeleniId, o.NazevOddeleni));
                }
                comboBoxOddeleniZamestnanci.DataSource = odd;
                comboBoxOddeleniZamestnanci.DisplayMember = "Nazev";
                comboBoxOddeleniZamestnanci.ValueMember = "OddeleniId";
                comboBoxOddeleniZamestnanci.SelectedIndex = 0;
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
                List<Emploee> zam = new List<Emploee>
                {
                    new Emploee("(select employee)", null, null, 0, null, null, false, 0, 0)
                };

                foreach (var z in zamestnanci)
                {
                    zam.Add(new Emploee(z.Prijmeni + " " + z.Jmeno, z.Jmeno, z.Prijmeni, z.ZamestnanecId, z.Login, z.Email, z.AdminAP, z.OddeleniId, z.StavObjektu));
                }
                ComboBoxZamestnanci.DataSource = zam;
                ComboBoxZamestnanci.DisplayMember = "CeleJmeno";
                ComboBoxZamestnanci.ValueMember = "ZamestnanecId";
                ComboBoxZamestnanci.SelectedIndex = 0;
                return true;
            }
        }

        private void RadioButtonNovyZamestnanec_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButtonNovyZamestnanec.Checked == true)
            {
                labelSeznamZamestnancu.Enabled = false;
                ComboBoxZamestnanci.SelectedValue = 0;
                ComboBoxZamestnanci.Enabled = false;
                comboBoxOddeleniZamestnanci.SelectedValue = 0;

                radioButtonAktivni.Checked = false;
                radioButtonNeaktivni.Checked = false;
                radioButtonOdstranen.Checked = false;

                checkBoxAdmin.Checked = false;

                textBoxKrestniJmeno.Text = string.Empty;
                textBoxPrijmeni.Text = string.Empty;
                textBoxLogin.Text = string.Empty;
                textBoxEmail.Text = string.Empty;
            }
        }

        private void RadioButtonAktualizaceZamestnance_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButtonAktualizaceZamestnance.Checked == true)
            {
                labelSeznamZamestnancu.Enabled = true;
                ComboBoxZamestnanci.SelectedValue = 0;
                ComboBoxZamestnanci.Enabled = true;
                comboBoxOddeleniZamestnanci.SelectedValue = 0;

                radioButtonAktivni.Checked = false;
                radioButtonNeaktivni.Checked = false;
                radioButtonOdstranen.Checked = false;

                checkBoxAdmin.Checked = false;

                textBoxKrestniJmeno.Text = string.Empty;
                textBoxPrijmeni.Text = string.Empty;
                textBoxLogin.Text = string.Empty;
                textBoxEmail.Text = string.Empty;
            }
        }

        private void ComboBoxZamestnanci_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender is ComboBox combo))
            {
                return;
            }
            selectedZamestnanec = combo.SelectedItem as Emploee;
            if (selectedZamestnanec == null) { }
            else
            {
                textBoxKrestniJmeno.Text = selectedZamestnanec.Jmeno;
                textBoxPrijmeni.Text = selectedZamestnanec.Prijmeni;
                textBoxLogin.Text = selectedZamestnanec.Login;
                textBoxEmail.Text = selectedZamestnanec.Email;
                if (selectedZamestnanec.AdminAP == false)
                {
                    checkBoxAdmin.Checked = false;
                    oldAdmin = false;
                }
                else
                {
                    checkBoxAdmin.Checked = true;
                    oldAdmin = true;
                }

                if (selectedZamestnanec.StavObjektu == 0)
                {
                    radioButtonAktivni.Checked = false;
                    radioButtonNeaktivni.Checked = false;
                    radioButtonOdstranen.Checked = false;
                }
                else if (selectedZamestnanec.StavObjektu == 1)
                {
                    radioButtonAktivni.Checked = true;
                    radioButtonNeaktivni.Checked = false;
                    radioButtonOdstranen.Checked = false;
                }
                else if (selectedZamestnanec.StavObjektu == 2)
                {
                    radioButtonAktivni.Checked = false;
                    radioButtonNeaktivni.Checked = true;
                    radioButtonOdstranen.Checked = false;
                }
                else if (selectedZamestnanec.StavObjektu == 3)
                {
                    radioButtonAktivni.Checked = false;
                    radioButtonNeaktivni.Checked = false;
                    radioButtonOdstranen.Checked = true;
                }
                comboBoxOddeleniZamestnanci.SelectedValue = selectedZamestnanec.OddeleniId;
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

            if (textBoxKrestniJmeno.Text == string.Empty)
            {
                MessageBox.Show("You must fill in the item 'First nam'.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ulozit = false;
            }
            else if (textBoxPrijmeni.Text == string.Empty)
            {
                MessageBox.Show("You must fill in the item 'Last name'.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ulozit = false;
            }
            else if (textBoxLogin.Text == string.Empty)
            {
                MessageBox.Show("You must fill in the item 'Login name'.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ulozit = false;
            }
            else if (textBoxEmail.Text == string.Empty)
            {
                MessageBox.Show("You must fill in the item 'Email'.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ulozit = false;
            }
            else if (radioButtonAktivni.Checked == false && radioButtonNeaktivni.Checked == false && radioButtonOdstranen.Checked == false)
            {
                MessageBox.Show("You must fill in the item 'State'.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ulozit = false;
            }
            else if (Convert.ToInt16(comboBoxOddeleniZamestnanci.SelectedValue) == 0)
            {
                MessageBox.Show("You must fill in the item 'Department'.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ulozit = false;
            }
            else
                ulozit = true;

            return ulozit;
        }

        private void ButtonUlozit_MouseClick(object sender, MouseEventArgs e)
        {
            byte stav = 0;

            if (KontrolaZamestnance() == false) { }
            else
            {
                bool pokracovat = true;
                if (((radioButtonNeaktivni.Checked == true || radioButtonOdstranen.Checked == true) && oldAdmin == true) || (checkBoxAdmin.Checked == false && oldAdmin == true))
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
                    if (AdminViewModel.VybranyZamestnanec(textBoxLogin.Text) == false)
                    {
                        MessageBox.Show("The new employee already exists.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (radioButtonAktivni.Checked == true)
                            stav = 1;

                        if (radioButtonNeaktivni.Checked == true)
                            stav = 2;

                        if (radioButtonOdstranen.Checked == true)
                            stav = 3;

                        if (RadioButtonNovyZamestnanec.Checked == true)
                        {
                            ZamestnanciDataMapper.InsertZamestnanec(textBoxKrestniJmeno.Text,
                                textBoxPrijmeni.Text,
                                textBoxLogin.Text,
                                Convert.ToInt32(comboBoxOddeleniZamestnanci.SelectedValue),
                                textBoxEmail.Text,
                                checkBoxAdmin.Checked,
                                stav
                                );

                            MessageBox.Show("A new employee has been saved.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        if (RadioButtonAktualizaceZamestnance.Checked == true)
                        {
                            ZamestnanciDataMapper.UpdateZamestnanec(selectedZamestnanec.ZamestnanecId,
                                textBoxKrestniJmeno.Text,
                                textBoxPrijmeni.Text,
                                textBoxLogin.Text,
                                Convert.ToInt32(comboBoxOddeleniZamestnanci.SelectedValue),
                                textBoxEmail.Text,
                                checkBoxAdmin.Checked,
                                stav
                                );

                            MessageBox.Show("The selected employee has been saved.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        NaplnitComboBoxZamestnanci();

                        ComboBoxZamestnanci.SelectedValue = 0;
                        comboBoxOddeleniZamestnanci.SelectedValue = 0;

                        radioButtonAktivni.Checked = false;
                        radioButtonNeaktivni.Checked = false;
                        radioButtonOdstranen.Checked = false;

                        checkBoxAdmin.Checked = false;

                        textBoxKrestniJmeno.Text = string.Empty;
                        textBoxPrijmeni.Text = string.Empty;
                        textBoxLogin.Text = string.Empty;
                        textBoxEmail.Text = string.Empty;
                    }
                }
            }
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }
    }
}
