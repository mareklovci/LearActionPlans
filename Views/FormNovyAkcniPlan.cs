using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LearActionPlans.ViewModels;
using LearActionPlans.DataMappers;

namespace LearActionPlans.Views
{
    public partial class FormNovyAkcniPlan : Form
    {
        private readonly FormPrehledBoduAP formPrehledBoduAp;
        private readonly EmployeeRepository employeeRepository;
        private AkcniPlanTmp akcniPlan;
        private int posledniCisloAP;

        private bool zadavatel1Povolen;
        private bool temaPovoleno;
        private bool datumUkonceniPovolen;
        private bool zakaznikPovolen;

        private bool ulozeniDat;

        public class AkcniPlanTmp
        {
            public int Id { get; set; }
            public DateTime DatumZalozeni { get; set; }
            public int CisloAP { get; set; }
            public string CisloAPRok { get; set; }
            public int Zadavatel1Id { get; set; }
            public int? Zadavatel2Id { get; set; }
            public string Zadavatel1Jmeno { get; set; }
            public string Zadavatel2Jmeno { get; set; }
            public string Tema { get; set; }
            public int? ProjektId { get; set; }
            public string ProjektNazev { get; set; }
            public int ZakaznikId { get; set; }
            public string ZakaznikNazev { get; set; }
            public byte TypAP { get; set; }
            public byte StavObjektu { get; set; }
            public DateTime DatumUkonceni { get; set; }
            public bool APUzavren { get; set; }
            public string Poznamka { get; set; }
        }

        public FormNovyAkcniPlan(
            FormPrehledBoduAP formPrehledBoduAp,
            EmployeeRepository employeeRepository)
        {
            this.formPrehledBoduAp = formPrehledBoduAp;
            this.employeeRepository = employeeRepository;

            this.InitializeComponent();
            this.ulozeniDat = false;

            this.akcniPlan = new AkcniPlanTmp();
            this.zadavatel1Povolen = false;
            this.temaPovoleno = false;
            this.datumUkonceniPovolen = false;
            this.zakaznikPovolen = false;

            this.ButtonUlozit.Enabled = false;
        }

        private void FormNovyAkcniPlan_Load(object sender, EventArgs e)
        {
            //tady zjistím poslední obsazené číslo Akčního plánu
            this.posledniCisloAP = NewActionPlanViewModel.GetLastActionPlanNumber(DateTime.Now.Year);
            //když bude posledniCisloAP = -1, došlo k problému při práci s databází a zadání AP se ukončí
            if (this.posledniCisloAP == -1)
            {
                this.Close();
            }

            //posledniCisloAP++;
            this.labelCisloAPVygenerovat.Text = $@"{this.posledniCisloAP:D3} / DateTime.Now.Year";
            this.akcniPlan.CisloAPRok = this.labelCisloAPVygenerovat.Text;

            if (this.NaplnitComboBoxZamestnanec1a2() == false)
            {
                this.ComboBoxZadavatel1.Enabled = false;
                this.ComboBoxZadavatel2.Enabled = false;
                //Nejsou dostupní žádní zaměstnanci.
                //Zadání nového Akčního plánu bude ukončeno.
                MessageBox.Show($@"No employees available. \nEntering a new Action plan will be completed.",
                    @"Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                this.ComboBoxZadavatel1.Enabled = true;
                this.ComboBoxZadavatel2.Enabled = true;
            }

            if (this.NaplnitComboBoxProjekty() == false)
            {
                this.ComboBoxProjekty.Enabled = false;
                //Nejsou dostupné žádné projekty.
                //Zadání nového Akčního plánu bude ukončeno.
                MessageBox.Show($@"No projects available. \nEntering a new Action plan will be completed.",
                    @"Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                this.ComboBoxProjekty.Enabled = true;
            }

            if (this.NaplnitComboBoxZakaznici() == false)
            {
                this.ComboBoxZakaznici.Enabled = false;
                //Nejsou dostupní žádní zákazníci.
                //Zadání nového Akčního plánu bude ukončeno.
                MessageBox.Show("No customers available." + (char)10 + "Entering a new Action plan will be completed.",
                    @"Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                this.ComboBoxZakaznici.Enabled = true;
            }

            //tady se vytvoří event handlery
            this.ComboBoxZadavatel1.SelectedIndexChanged += this.ComboBoxZadavatel1_SelectedIndexChanged;
            this.ComboBoxZadavatel2.SelectedIndexChanged += this.ComboBoxZadavatel2_SelectedIndexChanged;
            this.TextBoxTema.TextChanged += this.TextBoxTema_TextChanged;
            this.ComboBoxProjekty.SelectedIndexChanged += this.ComboBoxProjekty_SelectedIndexChanged;
            this.DateTimePickerDatumUkonceni.ValueChanged += this.DateTimePickerDatumUkonceni_ValueChanged;
            this.ComboBoxZakaznici.SelectedIndexChanged += this.ComboBoxZakaznici_SelectedIndexChanged;
            this.RadioButtonAudity.CheckedChanged += this.RadioButtonAudity_CheckedChanged;
            this.RadioButtonOstatni.CheckedChanged += this.RadioButtonOstatni_CheckedChanged;

            this.akcniPlan.DatumUkonceni = this.DateTimePickerDatumUkonceni.Value;

            this.RadioButtonAudity.Checked = true;
            this.RadioButtonOstatni.Checked = false;

            this.akcniPlan.TypAP = 1;

            this.datumUkonceniPovolen = true;
        }

        private class Zam
        {
            public string Jmeno { get; set; }
            public int ZamestnanecId { get; set; }

            public Zam(string jmeno, int zamestnanecId)
            {
                this.Jmeno = jmeno;
                this.ZamestnanecId = zamestnanecId;
            }
        }

        private bool NaplnitComboBoxZamestnanec1a2()
        {
            var zamestnanci = this.employeeRepository.GetEmployeesOriginalViewModel().ToList();

            if (zamestnanci.Count == 0)
            {
                return false;
            }

            var zam1 = new List<Zam> {new Zam("(select employee)", 0)};

            var zam2 = new List<Zam> {new Zam("(select employee)", 0)};

            foreach (var z in zamestnanci)
            {
                zam1.Add(new Zam(z.Jmeno, z.Id));
                zam2.Add(new Zam(z.Jmeno, z.Id));
            }

            this.ComboBoxZadavatel1.DataSource = zam1;
            this.ComboBoxZadavatel2.DataSource = zam2;
            this.ComboBoxZadavatel1.DisplayMember = "Jmeno";
            this.ComboBoxZadavatel2.DisplayMember = "Jmeno";
            this.ComboBoxZadavatel1.ValueMember = "ZamestnanecId";
            this.ComboBoxZadavatel2.ValueMember = "ZamestnanecId";
            this.ComboBoxZadavatel1.SelectedIndex = 0;
            this.ComboBoxZadavatel2.SelectedIndex = 0;
            return true;
        }

        private class Proj
        {
            public string NazevProjektu { get; set; }
            public int ProjektId { get; set; }

            public Proj(string nazevProjektu, int projektId)
            {
                this.NazevProjektu = nazevProjektu;
                this.ProjektId = projektId;
            }
        }

        private bool NaplnitComboBoxProjekty()
        {
            var projekty = NewActionPlanViewModel.GetProjects().ToList();

            if (projekty.Count == 0)
            {
                return false;
            }

            var proj = new List<Proj> {new Proj("(select a project)", 0)};

            proj.AddRange(projekty.Select(p => new Proj(p.ProjectName, p.ProjectId)));

            this.ComboBoxProjekty.DataSource = proj;
            this.ComboBoxProjekty.DisplayMember = "NazevProjektu";
            this.ComboBoxProjekty.ValueMember = "ProjektId";
            this.ComboBoxProjekty.SelectedIndex = 0;
            return true;
        }

        private class Zak
        {
            public string NazevZakaznika { get; set; }
            public int ZakaznikId { get; set; }

            public Zak(string nazevZakaznika, int zakaznikId)
            {
                this.NazevZakaznika = nazevZakaznika;
                this.ZakaznikId = zakaznikId;
            }
        }

        private bool NaplnitComboBoxZakaznici()
        {
            var zakaznici = NewActionPlanViewModel.GetCustomers().ToList();

            if (zakaznici.Count == 0)
            {
                return false;
            }

            var zak = new List<Zak> {new Zak("(choose a customer)", 0)};

            zak.AddRange(zakaznici.Select(z => new Zak(z.CustomerName, z.CustomerId)));

            this.ComboBoxZakaznici.DataSource = zak;
            this.ComboBoxZakaznici.DisplayMember = "NazevZakaznika";
            this.ComboBoxZakaznici.ValueMember = "ZakaznikId";
            this.ComboBoxZakaznici.SelectedIndex = 0;
            return true;
        }

        private void ComboBoxZadavatel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ComboBoxZadavatel1.SelectedIndex > 0)
            {
                this.akcniPlan.Zadavatel1Id = (int)this.ComboBoxZadavatel1.SelectedValue;
                this.akcniPlan.Zadavatel1Jmeno = this.ComboBoxZadavatel1.Text;
                this.zadavatel1Povolen = true;
            }
            else if (this.ComboBoxZadavatel1.SelectedIndex == 0)
            {
                this.akcniPlan.Zadavatel1Id = (int)this.ComboBoxZadavatel1.SelectedValue;
                this.akcniPlan.Zadavatel1Jmeno = null;
                this.zadavatel1Povolen = false;
            }

            // kontrola, jsetli mohu zaktivnit talčítko pro uložení AP
            if (this.zadavatel1Povolen && this.temaPovoleno && this.datumUkonceniPovolen && this.zakaznikPovolen)
            {
                this.ButtonUlozit.Enabled = true;
            }
            else
            {
                this.ButtonUlozit.Enabled = false;
            }
        }

        private void ComboBoxZadavatel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ComboBoxZadavatel2.SelectedIndex > 0)
            {
                this.akcniPlan.Zadavatel2Id = (int)this.ComboBoxZadavatel2.SelectedValue;
                this.akcniPlan.Zadavatel2Jmeno = this.ComboBoxZadavatel2.Text;
            }
            else if (this.ComboBoxZadavatel2.SelectedIndex == 0)
            {
                this.akcniPlan.Zadavatel2Id = (int)this.ComboBoxZadavatel2.SelectedValue;
                this.akcniPlan.Zadavatel2Jmeno = null;
            }

            // kontrola, jestli mohu zaktivnit talčítko pro uložení AP
            if (this.zadavatel1Povolen && this.temaPovoleno && this.datumUkonceniPovolen && this.zakaznikPovolen)
            {
                this.ButtonUlozit.Enabled = true;
            }
            else
            {
                this.ButtonUlozit.Enabled = false;
            }
        }

        private void TextBoxTema_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TextBoxTema.Text))
            {
                this.akcniPlan.Tema = null;
                this.temaPovoleno = false;
            }
            else
            {
                this.akcniPlan.Tema = this.TextBoxTema.Text;
                this.temaPovoleno = true;
            }

            // kontrola, jestli mohu zaktivnit talčítko pro uložení AP
            if (this.zadavatel1Povolen && this.temaPovoleno && this.datumUkonceniPovolen && this.zakaznikPovolen)
            {
                this.ButtonUlozit.Enabled = true;
            }
            else
            {
                this.ButtonUlozit.Enabled = false;
            }
        }

        private void ComboBoxProjekty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ComboBoxProjekty.SelectedIndex > 0)
            {
                this.akcniPlan.ProjektId = (int)this.ComboBoxProjekty.SelectedValue;
                this.akcniPlan.ProjektNazev = this.ComboBoxProjekty.Text;
            }
            else if (this.ComboBoxProjekty.SelectedIndex == 0)
            {
                this.akcniPlan.ProjektId = (int)this.ComboBoxProjekty.SelectedValue;
                this.akcniPlan.ProjektNazev = null;
            }
        }

        private void DateTimePickerDatumUkonceni_ValueChanged(object sender, EventArgs e)
        {
            this.akcniPlan.DatumUkonceni = this.DateTimePickerDatumUkonceni.Value;
            this.datumUkonceniPovolen = true;
        }

        private void ComboBoxZakaznici_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ComboBoxZakaznici.SelectedIndex > 0)
            {
                this.akcniPlan.ZakaznikId = (int)this.ComboBoxZakaznici.SelectedValue;
                this.akcniPlan.ZakaznikNazev = this.ComboBoxZakaznici.Text;
                this.zakaznikPovolen = true;
            }
            else if (this.ComboBoxZakaznici.SelectedIndex == 0)
            {
                this.akcniPlan.ZakaznikId = (int)this.ComboBoxZakaznici.SelectedValue;
                this.zakaznikPovolen = false;
            }

            // kontrola, jsetli mohu zaktivnit talčítko pro uložení AP
            if (this.zadavatel1Povolen && this.temaPovoleno && this.datumUkonceniPovolen && this.zakaznikPovolen)
            {
                this.ButtonUlozit.Enabled = true;
            }
            else
            {
                this.ButtonUlozit.Enabled = false;
            }
        }

        private void RadioButtonAudity_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioButtonAudity.Checked)
            {
                this.akcniPlan.TypAP = 1;
            }
        }

        private void RadioButtonOstatni_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioButtonOstatni.Checked)
            {
                this.akcniPlan.TypAP = 2;
            }
        }

        private void ButtonUlozit_MouseClick(object sender, MouseEventArgs e)
        {
            this.ulozeniDat = true;
            this.Close();
            //UlozitAP();
        }

        private void UlozitAP()
        {
            this.akcniPlan.CisloAP = this.posledniCisloAP;
            //akcniPlan.Rok = DateTime.Now.Year;

            this.akcniPlan.Id = AkcniPlanyDataMapper.InsertAP(this.akcniPlan);
            //nastavím vlastníkAP na true, protože musí být editovány jednotlivé body
            //FormMain.VlastnikAP = true;
            using var form = this.formPrehledBoduAp;
            form.CreateFormPrehledBoduAP(true, this.akcniPlan, 1);
            this.Hide();
            form.ShowDialog();
        }

        private void ZavritOkno()
        {
            if (this.ulozeniDat)
            {
                this.UlozitAP();
            }
            else
            {
                //byly založeny všechny údaje pro uložení dat
                if (!this.ButtonUlozit.Enabled)
                {
                    return;
                }

                var dialogResult = MessageBox.Show(@"Do you want to create AP.", @"Notice", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        this.UlozitAP();
                        break;
                    case DialogResult.No:
                    case DialogResult.None:
                    case DialogResult.OK:
                    case DialogResult.Cancel:
                    case DialogResult.Abort:
                    case DialogResult.Retry:
                    case DialogResult.Ignore:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void FormNovyAkcniPlan_FormClosing(object sender, FormClosingEventArgs e) => this.ZavritOkno();

        private void ButtonClose_MouseClick(object sender, MouseEventArgs e) => this.Close();
    }
}
