using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LearActionPlans.ViewModels;
using LearActionPlans.Models;
using LearActionPlans.DataMappers;

namespace LearActionPlans.Views
{
    public partial class FormNovyAkcniPlan : Form
    {
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

        public FormNovyAkcniPlan()
        {
            InitializeComponent();
            ulozeniDat = false;

            akcniPlan = new AkcniPlanTmp();
            zadavatel1Povolen = false;
            temaPovoleno = false;
            datumUkonceniPovolen = false;
            zakaznikPovolen = false;

            ButtonUlozit.Enabled = false;
        }

        private void FormNovyAkcniPlan_Load(object sender, EventArgs e)
        {
            //tady zjistím poslední obsazené číslo Akčního plánu
            posledniCisloAP = NovyAkcniPlanViewModel.GetPosledniCisloAP(DateTime.Now.Year);
            //když bude posledniCisloAP = -1, došlo k problému při práci s databází a zadání AP se ukončí
            if (posledniCisloAP == -1)
                Close();
            //posledniCisloAP++;
            labelCisloAPVygenerovat.Text = posledniCisloAP.ToString("D3") + " / " + DateTime.Now.Year.ToString();
            akcniPlan.CisloAPRok = labelCisloAPVygenerovat.Text;

            if (NaplnitComboBoxZamestnanec1a2() == false)
            {
                ComboBoxZadavatel1.Enabled = false;
                ComboBoxZadavatel2.Enabled = false;
                //Nejsou dostupní žádní zaměstnanci.
                //Zadání nového Akčního plánu bude ukončeno.
                MessageBox.Show("No employees available." + (char)10 + "Entering a new Action plan will be completed.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                ComboBoxZadavatel1.Enabled = true;
                ComboBoxZadavatel2.Enabled = true;
            }
                        
            if (NaplnitComboBoxProjekty() == false)
            {
                ComboBoxProjekty.Enabled = false;
                //Nejsou dostupné žádné projekty.
                //Zadání nového Akčního plánu bude ukončeno.
                MessageBox.Show("No projects available." + (char)10 + "Entering a new Action plan will be completed.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                ComboBoxProjekty.Enabled = true;
            }
                        
            if (NaplnitComboBoxZakaznici() == false)
            {
                ComboBoxZakaznici.Enabled = false;
                //Nejsou dostupní žádní zákazníci.
                //Zadání nového Akčního plánu bude ukončeno.
                MessageBox.Show("No customers available." + (char)10 + "Entering a new Action plan will be completed.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                ComboBoxZakaznici.Enabled = true;
            }

            //tady se vytvoří event handlery
            ComboBoxZadavatel1.SelectedIndexChanged +=
            new System.EventHandler(ComboBoxZadavatel1_SelectedIndexChanged);

            ComboBoxZadavatel2.SelectedIndexChanged +=
            new System.EventHandler(ComboBoxZadavatel2_SelectedIndexChanged);

            TextBoxTema.TextChanged +=
            new System.EventHandler(TextBoxTema_TextChanged);

            ComboBoxProjekty.SelectedIndexChanged +=
            new System.EventHandler(ComboBoxProjekty_SelectedIndexChanged);
            
            DateTimePickerDatumUkonceni.ValueChanged +=
            new System.EventHandler(DateTimePickerDatumUkonceni_ValueChanged);

            ComboBoxZakaznici.SelectedIndexChanged +=
            new System.EventHandler(ComboBoxZakaznici_SelectedIndexChanged);

            RadioButtonAudity.CheckedChanged +=
            new System.EventHandler(RadioButtonAudity_CheckedChanged);

            RadioButtonOstatni.CheckedChanged +=
            new System.EventHandler(RadioButtonOstatni_CheckedChanged);

            akcniPlan.DatumUkonceni = DateTimePickerDatumUkonceni.Value;

            RadioButtonAudity.Checked = true;
            RadioButtonOstatni.Checked = false;

            akcniPlan.TypAP = 1;

            datumUkonceniPovolen = true;
        }

        private class Zam
        {
            public string Jmeno { get; set; }
            public int ZamestnanecId { get; set; }

            public Zam(string jmeno, int zamestnanecId)
            {
                Jmeno = jmeno;
                ZamestnanecId = zamestnanecId;
            }
        }

        private bool NaplnitComboBoxZamestnanec1a2()
        {
            var zamestnanci = NovyAkcniPlanViewModel.GetZamestnanci().ToList();

            if (zamestnanci.Count == 0)
            {
                return false;
            }
            else
            {
                List<Zam> zam1 = new List<Zam>
                {
                    new Zam("(select employee)", 0)
                };

                List<Zam> zam2 = new List<Zam>
                {
                    new Zam("(select employee)", 0)
                };

                foreach (var z in zamestnanci)
                {
                    zam1.Add(new Zam(z.Jmeno, z.ZamestnanecId));
                    zam2.Add(new Zam(z.Jmeno, z.ZamestnanecId));
                }
                ComboBoxZadavatel1.DataSource = zam1;
                ComboBoxZadavatel2.DataSource = zam2;
                ComboBoxZadavatel1.DisplayMember = "Jmeno";
                ComboBoxZadavatel2.DisplayMember = "Jmeno";
                ComboBoxZadavatel1.ValueMember = "ZamestnanecId";
                ComboBoxZadavatel2.ValueMember = "ZamestnanecId";
                ComboBoxZadavatel1.SelectedIndex = 0;
                ComboBoxZadavatel2.SelectedIndex = 0;
                return true;
            }
        }

        private class Proj
        {
            public string NazevProjektu { get; set; }
            public int ProjektId { get; set; }

            public Proj(string nazevProjektu, int projektId)
            {
                NazevProjektu = nazevProjektu;
                ProjektId = projektId;
            }
        }

        private bool NaplnitComboBoxProjekty()
        {
            var projekty = NovyAkcniPlanViewModel.GetProjekty().ToList();

            if (projekty.Count == 0)
            {
                return false;
            }
            else
            {
                List<Proj> proj = new List<Proj>
                {
                    new Proj("(select a project)", 0)
                };
                foreach (var p in projekty)
                {
                    proj.Add(new Proj(p.NazevProjektu, p.ProjektId));
                }
                ComboBoxProjekty.DataSource = proj;
                ComboBoxProjekty.DisplayMember = "NazevProjektu";
                ComboBoxProjekty.ValueMember = "ProjektId";
                ComboBoxProjekty.SelectedIndex = 0;
                return true;
            }
        }

        private class Zak
        {
            public string NazevZakaznika { get; set; }
            public int ZakaznikId { get; set; }

            public Zak(string nazevZakaznika, int zakaznikId)
            {
                NazevZakaznika = nazevZakaznika;
                ZakaznikId = zakaznikId;
            }
        }

        private bool NaplnitComboBoxZakaznici()
        {
            var zakaznici = NovyAkcniPlanViewModel.GetZakaznici().ToList();

            if (zakaznici.Count == 0)
            {
                return false;
            }
            else
            {
                List<Zak> zak = new List<Zak>
                {
                    new Zak("(choose a customer)", 0)
                };
                //new Zak() { NazevZakaznika = "(choose a customer)", ZakaznikId = 0 }
                foreach (var z in zakaznici)
                {
                    zak.Add(new Zak(z.NazevZakaznika, z.ZakaznikId));
                }
                //zak.Add(new Zak() { NazevZakaznika = z.NazevZakaznika, ZakaznikId = z.ZakaznikId });
                ComboBoxZakaznici.DataSource = zak;
                ComboBoxZakaznici.DisplayMember = "NazevZakaznika";
                ComboBoxZakaznici.ValueMember = "ZakaznikId";
                ComboBoxZakaznici.SelectedIndex = 0;
                return true;
            }
        }

        private void ComboBoxZadavatel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxZadavatel1.SelectedIndex > 0)
            {
                akcniPlan.Zadavatel1Id = (int)ComboBoxZadavatel1.SelectedValue;
                akcniPlan.Zadavatel1Jmeno = ComboBoxZadavatel1.Text;
                zadavatel1Povolen = true;
            }
            else if (ComboBoxZadavatel1.SelectedIndex == 0)
            {
                akcniPlan.Zadavatel1Id = (int)ComboBoxZadavatel1.SelectedValue;
                akcniPlan.Zadavatel1Jmeno = null;
                zadavatel1Povolen = false;
            }

            // kontrola, jsetli mohu zaktivnit talčítko pro uložení AP
            if (zadavatel1Povolen == true && temaPovoleno == true && datumUkonceniPovolen == true && zakaznikPovolen == true)
                ButtonUlozit.Enabled = true;
            else
                ButtonUlozit.Enabled = false;
        }

        private void ComboBoxZadavatel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxZadavatel2.SelectedIndex > 0)
            {
                akcniPlan.Zadavatel2Id = (int)ComboBoxZadavatel2.SelectedValue;
                akcniPlan.Zadavatel2Jmeno = ComboBoxZadavatel2.Text;
            }
            else if (ComboBoxZadavatel2.SelectedIndex == 0)
            {
                akcniPlan.Zadavatel2Id = (int)ComboBoxZadavatel2.SelectedValue;
                akcniPlan.Zadavatel2Jmeno = null;
            }

            // kontrola, jestli mohu zaktivnit talčítko pro uložení AP
            if (zadavatel1Povolen == true && temaPovoleno == true && datumUkonceniPovolen == true && zakaznikPovolen == true)
                ButtonUlozit.Enabled = true;
            else
                ButtonUlozit.Enabled = false;
        }

        private void TextBoxTema_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxTema.Text))
            {
                akcniPlan.Tema = null;
                temaPovoleno = false;
            }
            else
            {
                akcniPlan.Tema = TextBoxTema.Text;
                temaPovoleno = true;
            }
                
            // kontrola, jestli mohu zaktivnit talčítko pro uložení AP
            if (zadavatel1Povolen == true && temaPovoleno == true && datumUkonceniPovolen == true && zakaznikPovolen == true)
                ButtonUlozit.Enabled = true;
            else
                ButtonUlozit.Enabled = false;
        }

        private void ComboBoxProjekty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxProjekty.SelectedIndex > 0)
            {
                akcniPlan.ProjektId = (int)ComboBoxProjekty.SelectedValue;
                akcniPlan.ProjektNazev = ComboBoxProjekty.Text;
            }
            else if (ComboBoxProjekty.SelectedIndex == 0)
            {
                akcniPlan.ProjektId = (int)ComboBoxProjekty.SelectedValue;
                akcniPlan.ProjektNazev = null;
            }
        }

        private void DateTimePickerDatumUkonceni_ValueChanged(object sender, EventArgs e)
        {
            akcniPlan.DatumUkonceni = DateTimePickerDatumUkonceni.Value;
            datumUkonceniPovolen = true;
        }

        private void ComboBoxZakaznici_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxZakaznici.SelectedIndex > 0)
            {
                akcniPlan.ZakaznikId = (int)ComboBoxZakaznici.SelectedValue;
                akcniPlan.ZakaznikNazev = ComboBoxZakaznici.Text;
                zakaznikPovolen = true;
            }
            else if (ComboBoxZakaznici.SelectedIndex == 0)
            {
                akcniPlan.ZakaznikId = (int)ComboBoxZakaznici.SelectedValue;
                zakaznikPovolen = false;
            }

            // kontrola, jsetli mohu zaktivnit talčítko pro uložení AP
            if (zadavatel1Povolen == true && temaPovoleno == true && datumUkonceniPovolen == true && zakaznikPovolen == true)
                ButtonUlozit.Enabled = true;
            else
                ButtonUlozit.Enabled = false;
        }

        private void RadioButtonAudity_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButtonAudity.Checked == true)
                akcniPlan.TypAP = 1;
        }

        private void RadioButtonOstatni_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButtonOstatni.Checked == true)
                akcniPlan.TypAP = 2;
        }

        private void ButtonUlozit_MouseClick(object sender, MouseEventArgs e)
        {
            ulozeniDat = true;
            Close();
            //UlozitAP();
        }

        private void UlozitAP()
        {
            akcniPlan.CisloAP = posledniCisloAP;
            //akcniPlan.Rok = DateTime.Now.Year;

            akcniPlan.Id = AkcniPlanyDataMapper.InsertAP(akcniPlan);
            //nastavím vlastníkAP na true, protože musí být editovány jednotlivé body
            //FormMain.VlastnikAP = true;
            using (var form = new FormPrehledBoduAP(true, akcniPlan, 1))
            {
                Hide();
                form.ShowDialog();
                //Close();
            }
        }

        private void ZavritOkno()
        {
            if (ulozeniDat == true)
            {
                UlozitAP();
            }
            else
            {
                //byly založeny všechny údaje pro uložení dat
                if (ButtonUlozit.Enabled == true)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to create AP.", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (dialogResult == DialogResult.Yes)
                    {
                        UlozitAP();
                        //Close();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                    }
                }
            }
        }

        private void FormNovyAkcniPlan_FormClosing(object sender, FormClosingEventArgs e)
        {
            ZavritOkno();
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }
    }
}
