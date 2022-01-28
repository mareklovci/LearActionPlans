using System;
using System.Windows.Forms;
using System.DirectoryServices.AccountManagement;

using LearActionPlans.Utilities;
using System.IO;
using System.Configuration;

namespace LearActionPlans.Views
{
    public partial class FormMain : Form
    {
        private readonly bool spusteniBezParametru;
        private readonly string[] args_;

        //proměnné pro přihlášení uživatele
        //public static bool UzivatelPrihlasen { get; set; }
        //public static bool VlastnikAP { get; set; }
        //public static bool VlastnikAkce { get; set; }
        //public static int VlastnikIdAkce { get; set; }
        //public static int IDLoginUser { get; set; }
        //public static bool UzivatelOvereny { get; set; }

        public FormMain(string[] args)
        {
            InitializeComponent();
            args = Environment.GetCommandLineArgs();
            args_ = (string[])args.Clone();

            // pokud je spuštěn program bez parametrů, tak jediným parametrem je samotný název programu
            if (args.Length > 1)
            {
                spusteniBezParametru = false;
            }
            else
            {
                spusteniBezParametru = true;
                //MessageBox.Show("Bez parametrů.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //UzivatelPrihlasen = false;
            //VlastnikAP = false;
            //VlastnikAkce = false;
            //VlastnikIdAkce = 0;
            //to je proto, aby neplatilo, že 0 == 0
            //IDLoginUser = -1;
            //UzivatelOvereny = false;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //Helper.RegisterMyProtocol("LearAPConfirmation", "C:\\Users\\pc\\source\\repos\\LearConfirmation\\bin\\Release\\netcoreapp3.1\\LearConfirmation.exe");
            //LearAPReply - ten nebude
            //Helper.RegisterMyProtocol("LearAPReply", "C:\\Users\\pc\\source\\repos\\LearReply\\bin\\Release\\netcoreapp3.1\\LearReply.exe");
            //Helper.RegisterMyProtocol("LearActionPlans", "C:\\Users\\pc\\source\\repos\\LearActionPlans\\bin\\Release\\netcoreapp3.1\\LearActionPlans.exe");

            //string learAP = string.Empty;
            //string learConfirmation = string.Empty;

            if (spusteniBezParametru == true)
            {
                //try
                //{
                //    string currentPath = AppContext.BaseDirectory;
                //    MessageBox.Show(currentPath + "Data\\path.ini");
                //    if (File.Exists(currentPath + "Data\\path.ini"))
                //    {
                //        string path = Directory.GetCurrentDirectory();
                //        string[] lines = System.IO.File.ReadAllLines(currentPath + "Data\\path.ini");

                //        int i = 0;
                //        foreach (string line in lines)
                //        {
                //            //if (i == 0)
                //            //    nazevUctu = line;
                //            if (i == 1)
                //                learAP = line;
                //            //if (i == 2)
                //            //    smtpServer = line;
                //            if (i == 3)
                //                learConfirmation = line;
                //            //if (i == 4)
                //            //    emailTo = line;

                //            i++;
                //        }

                //        //LearAPReply - ten nebude
                //        //Helper.RegisterMyProtocol("LearAPReply", "C:\\Users\\pc\\source\\repos\\LearReply\\bin\\Release\\netcoreapp3.1\\LearReply.exe");
                //        if (!string.IsNullOrEmpty(learAP) && !string.IsNullOrEmpty(learConfirmation))
                //        {
                //            Helper.RegisterMyProtocol("LearActionPlans", learAP);
                //            Helper.RegisterMyProtocol("LearAPConfirmation", learConfirmation);
                //        }
                //    }
                //    else
                //    {
                //         MessageBox.Show("There is no path.ini file.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.ToString(), @"Upozornění", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                string learAP = ConfigurationManager.AppSettings["LearActionPlans"];
                string learConfirmation = ConfigurationManager.AppSettings["LearConfirmation"];

                Helper.RegisterMyProtocol("LearActionPlans", learAP);
                Helper.RegisterMyProtocol("LearAPConfirmation", learConfirmation);

            }

            if (spusteniBezParametru == false)
            {
                using (var form = new FormPrehledAP(spusteniBezParametru, args_))
                {
                    form.ShowDialog();
                }
            }

            //to potřebovat nebudu
            //jméno přihlášeného uživatele
            //string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            //string[] user;

            //user = userName.Split('\\');
            //přihlašovací jmnéno uživatele
            //MessageBox.Show(user[1]);
            //jméno přihlášeného uživatele
            //string s = UserPrincipal.Current.DisplayName;
            //MessageBox.Show(s);

            ButtonAdmin.Enabled = true;
            //UzivatelOvereny = true;
            //VlastnikAP = true;
        }

        private void ButtonNovyAkcniPlan_MouseClick(object sender, MouseEventArgs e)
        {
            using (var form = new FormNovyAkcniPlan())
            {
                form.ShowDialog();
            }
        }

        private void ButtonOpravaAkcnihoPlanu_MouseClick(object sender, MouseEventArgs e)
        {
            using (var form = new FormPrehledAP(spusteniBezParametru, args_))
            {
                form.ShowDialog();
            }
        }

        private void ButtonVsechnyBodyAP_MouseClick(object sender, MouseEventArgs e)
        {
            using (var form = new FormVsechnyBodyAP())
            {
                form.ShowDialog();
            }
        }

        private void ButtonSeznamZadosti_MouseClick(object sender, MouseEventArgs e)
        {
            using (var form = new FormSeznamPozadavku())
            {
                form.ShowDialog();
            }
        }

        private void ButtonAdmin_MouseClick(object sender, MouseEventArgs e)
        {
            using (var form = new FormAdmin())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                }
            }
        }

        private void ButtonLogin_MouseClick(object sender, MouseEventArgs e)
        {
            //if (UzivatelPrihlasen == false)
            //{
            //    using (var form = new FormOvereniUzivatele())
            //    {
            //        //var result = form.ShowDialog();
            //        form.ShowDialog();
            //        UzivatelOvereny = form.UzivatelOvereny;
            //        //if (result == DialogResult.OK)
            //        if (UzivatelOvereny == true)
            //        {
            //            IDLoginUser = form.IdLoginUser;
            //            if (form.Admin == true)
            //                ButtonAdmin.Enabled = true;

            //            UzivatelPrihlasen = true;
            //            //VlastnikAP = true;
            //            ButtonLogin.Text = "Logout";
            //        }
            //        if (UzivatelOvereny == false) { }
            //    }
            //}
            //else
            //{
            //    IDLoginUser = -1;
            //    UzivatelOvereny = false;
            //    UzivatelPrihlasen = false;

            //    VlastnikAP = false;
            //    VlastnikAkce = false;
            //    VlastnikIdAkce = 0;
                
            //    ButtonAdmin.Enabled = false;
            //    ButtonLogin.Text = "Login";
            //}
        }
    }
}
