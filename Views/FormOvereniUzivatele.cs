using System;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Windows.Forms;

using LearActionPlans.ViewModels;

namespace LearActionPlans.Views
{
    public partial class FormOvereniUzivatele : Form
    {
        public int IdLoginUser { get; set; }
        public bool UzivatelOvereny { get; set; }
        public bool Admin { get; set; }
        public FormOvereniUzivatele()
        {
            InitializeComponent();
        }

        private void FormOvereniUzivatele_Load(object sender, EventArgs e)
        {
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void ButtonOverit_MouseClick(object sender, MouseEventArgs e)
        {
            Admin = false;

            //to pak odstraním - začátek
            //UzivatelOvereny = true;
            //to pak odstraním - konec

            //ověření loginu v databázi
            var zadavatelLogin = OvereniUzivateleViewModel.GetZadavatelLogin(textBoxLogin.Text).ToList();

            if (zadavatelLogin.Count == 0)
                MessageBox.Show("Login name not found.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);     //zadaný uživatel neexistuje
            else
            {
                //to je Idčko, které budu prohledávat v zadavatel1, zadavatel2 a zadavateleAkci
                //když ho najdu v zadavatel1, zadavatel2 - je to majitel AP
                //když ho najdu v zadavateleAkci je to majitel akce
                //idLoginUser IDčko zjištěné z databáze na základě loginu uživatele
                IdLoginUser = zadavatelLogin[0].ZadavatelId;
                if (zadavatelLogin[0].Admin == true)
                    Admin = true;

                //to pak odstraním - začátek
                UzivatelOvereny = true;
                //to pak odstraním - konec

                //bool isValid;
                ////using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "learDomena"))
                //using (PrincipalContext pc = new PrincipalContext(ContextType.Machine, "ntb-bartos"))
                //{
                //    isValid = pc.ValidateCredentials(textBoxLogin.Text, textBoxHeslo.Text);     //pokud bude isValid true, uživatel je ověřený a může se pokračovat

                //    if (isValid == true)
                //    {
                //        UzivatelOvereny = true;
                //        MessageBox.Show("Your login is valid", "Credentials", MessageBoxButtons.OK);
                //        //labelOvereno.Visible = true;
                //    }
                //    else
                //    {
                //        UzivatelOvereny = false;
                //        MessageBox.Show("Your login is invalid", "Credentials", MessageBoxButtons.OK);
                //        //pokud bude chybně napsáno přihlášení, budou pole vyprázdněna
                //        textBoxLogin.Text = string.Empty;
                //        textBoxHeslo.Text = string.Empty;
                //    }
                //}
            }
        }
    }
}
