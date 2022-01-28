using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using LearActionPlans.ViewModels;
using LearActionPlans.DataMappers;
using System.Drawing;

namespace LearActionPlans.Views
{
    public partial class FormEditAP : Form
    {
        //public string Zadavatel1 { get; set; }

        private readonly int apId_;
        private readonly string cisloAPRok_;
        private readonly string zadavatel1_;
        private readonly string zadavatel2_;
        private readonly string tema_;
        private readonly string projekt_;
        private readonly string zakaznik_;

        private readonly int zadavatel1Id_;
        private readonly int? zadavatel2Id_;
        private readonly int? projektId_;
        private readonly int zakaznikId_;

        private Label labelZam1 = new Label();
        private Label labelZam2 = new Label();
        private Label labelProjekt = new Label();
        private Label labelZakaznik = new Label();


        private bool zmenaDat;

        private byte znovuOtevritAP;

        //historie termínů ukončení
        private Panel panelTerminy;
        private List<GroupBox> groupBoxTerminy;
        private List<Label> labelTerminyDatum;
        private List<RichTextBox> richTextBoxTermin;

        public FormEditAP(int apId, string cisloAPRok, string zadavatel1, string zadavatel2, string tema, string projekt, string zakaznik, int zadavatel1Id, int? zadavatel2Id, int? projektId, int zakaznikId)
        {
            InitializeComponent();

            apId_ = apId;
            cisloAPRok_ = cisloAPRok;
            zadavatel1_ = zadavatel1;
            zadavatel2_ = zadavatel2;
            tema_ = tema;
            projekt_ = projekt;
            zakaznik_ = zakaznik;

            zadavatel1Id_ = zadavatel1Id;
            zadavatel2Id_ = zadavatel2Id;
            projektId_ = projektId;
            zakaznikId_ = zakaznikId;

            zmenaDat = false;

            panelTerminy = new Panel();
            groupBoxTerminy = new List<GroupBox>();
            labelTerminyDatum = new List<Label>();
            richTextBoxTermin = new List<RichTextBox>();
        }

        private void FormEditAP_Load(object sender, EventArgs e)
        {
            labelNumber.Text = cisloAPRok_;

            InitForm();
        }

        private void InitForm()
        {
            //načíst všechny akce daného AP a zjistit, jestli mají všechny vyplněné datum efektivnosti
            //jestli ano, tak mohu aktivovat tlačítko pro ukončení AP
            //po uzavření AP uložím datum ukončení
            var uzavreneAkce = EditAPViewModel.GetUkonceniAkce(apId_).ToList();
            var znovuOtevrit = EditAPViewModel.GetZnovuOtevritAP(apId_).ToList();

            bool akceUkonceny = true;
            if (uzavreneAkce.Count == 0)
            {
                akceUkonceny = false;
            }
            else
            {
                foreach (var uzavrenaAkce in uzavreneAkce)
                {
                    if (uzavrenaAkce.KontrolaEfektivnosti == null)
                    {
                        akceUkonceny = false;
                    }
                }
            }

            //pokud je AP uzavřen a ještě nebyl reopen, tak aktivuji tlačítko Reopen
            ButtonZnovuOtevrit.Enabled = false;
            labelDuvodZnovuOtevreni.Enabled = false;
            richTextBoxDuvodZnovuOtevreni.Enabled = false;
            richTextBoxDuvodZnovuOtevreni.Text = string.Empty;
            //když už byl AP uzavřen a ještě nebyl Reopen, bude tlačítko Reopen aktivní
            znovuOtevritAP = znovuOtevrit[0].ZnovuOtevrit;
            //AP byl uzavřen a ještě nebyl znovuotevřen
            if (!(znovuOtevrit[0].UzavreniAP == null) && znovuOtevritAP == 1)
            {
                ButtonZnovuOtevrit.Enabled = true;
                labelDuvodZnovuOtevreni.Enabled = true;
                richTextBoxDuvodZnovuOtevreni.Enabled = true;
            }
            if (znovuOtevritAP == 0)
            {
                ButtonZnovuOtevrit.Visible = false;
                richTextBoxDuvodZnovuOtevreni.Text = znovuOtevrit[0].DuvodZnovuotevreni;
            }
            
            labelDatumUzavreni.Text = string.Empty;
            if (!(znovuOtevrit[0].UzavreniAP == null))
            {
                labelDatumUzavreni.Text = Convert.ToDateTime(znovuOtevrit[0].UzavreniAP).ToShortDateString();
            }

            //počet zbývajícíh termínů
            var pocetTerminu = EditAPViewModel.GetPocetTerminu(apId_).ToList();
            labelZbyvajiciPocetTerminu.Text = pocetTerminu[0].ZmenaTerminu.ToString();
            //pokud ještě mám volné termíny a AP nebyl uzavřen
            if (pocetTerminu[0].ZmenaTerminu == 0 || !(znovuOtevrit[0].UzavreniAP == null))
            {
                dateTimePickerDatumUkonceni.Enabled = false;
                labelPoznamkaTermin.Enabled = false;
                richTextBoxNovaPoznamka.Enabled = false;

                ButtonNovyTermin.Enabled = false;
            }
            else
            {
                dateTimePickerDatumUkonceni.Enabled = true;
                labelPoznamkaTermin.Enabled = true;
                richTextBoxNovaPoznamka.Enabled = true;

                ButtonNovyTermin.Enabled = true;
            }
            //počet zbývajícíh termínů

            if (pocetTerminu[0].ZmenaTerminu == 0)
            {
                dateTimePickerDatumUkonceni.Visible = false;
                labelPoznamkaTermin.Visible = false;
                richTextBoxNovaPoznamka.Visible = false;
                ButtonNovyTermin.Visible = false;
            }

            ButtonUkoncitAP.Enabled = false;
            //v případě, že jsou všechny akce uzavřené a AP ještě nebyl uzavřen, povolím tlačítko pro ukončení AP
            if (akceUkonceny == true && znovuOtevrit[0].UzavreniAP == null)
            {
                ButtonUkoncitAP.Enabled = true;
            }
            //v případě, že nebyl AP ještě uzavřen, mohu upravovat jeho parametry
            if (znovuOtevrit[0].UzavreniAP == null)
            {
                InitComboBox();
            }
            else
            {
                ComboBoxZadavatel1.SelectedIndexChanged -= ComboBoxZadavatel1_SelectedIndexChanged;
                ComboBoxZadavatel2.SelectedIndexChanged -= ComboBoxZadavatel2_SelectedIndexChanged;
                ComboBoxProjekty.SelectedIndexChanged -= ComboBoxProjekty_SelectedIndexChanged;
                ComboBoxZakaznici.SelectedIndexChanged -= ComboBoxZakaznici_SelectedIndexChanged;

                ComboBoxZadavatel1.Visible = false;
                ComboBoxZadavatel2.Visible = false;
                ComboBoxProjekty.Visible = false;
                ComboBoxZakaznici.Visible = false;

                //tlačítko pro uzavření AP musí být neaktivní, když je aktivní tlačítko pro Reopen
                ButtonUkoncitAP.Enabled = false;
                var zamestnanec1 = EditAPViewModel.GetZamestnanecId(zadavatel1Id_).ToList();
                labelZam1.Location = new Point(20, 105);
                labelZam1.Visible = true;
                labelZam1.AutoSize = true;
                labelZam1.Text = zamestnanec1[0].Jmeno;
                groupBoxEditAP.Controls.Add(labelZam1);

                if (zadavatel2Id_ == null) { }
                else
                {
                    var zamestnanec2 = EditAPViewModel.GetZamestnanecId(Convert.ToInt32(zadavatel2Id_)).ToList();
                    labelZam2.Location = new Point(20, 165);
                    labelZam2.Visible = true;
                    labelZam1.AutoSize = true;
                    labelZam2.Text = zamestnanec2[0].Jmeno;
                    groupBoxEditAP.Controls.Add(labelZam2);
                }
                if (projektId_ == null) { }
                else
                {
                    var projekt = EditAPViewModel.GetProjektId(Convert.ToInt32(projektId_)).ToList();
                    labelProjekt.Location = new Point(20, 225);
                    labelProjekt.Visible = true;
                    labelProjekt.AutoSize = true;
                    labelProjekt.Text = projekt[0].NazevProjektu;
                    groupBoxEditAP.Controls.Add(labelProjekt);
                }
                var zakaznik = EditAPViewModel.GetZakaznikId(Convert.ToInt32(zakaznikId_)).ToList();
                labelZakaznik.Location = new Point(20, 285);
                labelZakaznik.Visible = true;
                labelZakaznik.AutoSize = true;
                labelZakaznik.Text = zakaznik[0].NazevZakaznika;
                groupBoxEditAP.Controls.Add(labelZakaznik);
            }

            ButtonUlozit.Enabled = false;

            HistorieTerminu();
        }

        private void RemoveControl()
        {
            for (int i = groupBoxTerminy.Count - 1; i >= 0; i--)
            {
                groupBoxTerminy[i].Controls.Remove(labelTerminyDatum[i]);
                groupBoxTerminy[i].Controls.Remove(richTextBoxTermin[i]);

                panelTerminy.Controls.Remove(groupBoxTerminy[i]);

                labelTerminyDatum[i].Dispose();
                richTextBoxTermin[i].Dispose();

                groupBoxTerminy[i].Dispose();
            }
            Controls.Remove(panelTerminy);
            panelTerminy.Dispose();

            panelTerminy = new Panel();
            groupBoxTerminy = new List<GroupBox>();
            labelTerminyDatum = new List<Label>();
            richTextBoxTermin = new List<RichTextBox>();
        }

        private void InitComboBox()
        {
            ComboBoxZadavatel1.SelectedIndexChanged -= ComboBoxZadavatel1_SelectedIndexChanged;
            ComboBoxZadavatel2.SelectedIndexChanged -= ComboBoxZadavatel2_SelectedIndexChanged;
            ComboBoxProjekty.SelectedIndexChanged -= ComboBoxProjekty_SelectedIndexChanged;
            ComboBoxZakaznici.SelectedIndexChanged -= ComboBoxZakaznici_SelectedIndexChanged;

            //inicializace comboboxů ----------------------------------------------------------------------------
            labelZam1.Visible = false;
            labelZam2.Visible = false;
            labelProjekt.Visible = false;
            labelZakaznik.Visible = false;

            ComboBoxZadavatel1.Visible = true;
            ComboBoxZadavatel2.Visible = true;
            ComboBoxProjekty.Visible = true;
            ComboBoxZakaznici.Visible = true;

            NaplnitComboBoxZamestnanec1a2();
            NaplnitComboBoxProjekty();
            NaplnitComboBoxZakaznici();

            ComboBoxZadavatel1.SelectedIndex = Convert.ToInt32(ComboBoxZadavatel1.FindStringExact(zadavatel1_));
            if (string.IsNullOrEmpty(zadavatel2_))
                ComboBoxZadavatel2.SelectedIndex = 0;
            else
                ComboBoxZadavatel2.SelectedIndex = Convert.ToInt32(ComboBoxZadavatel2.FindStringExact(zadavatel2_));

            if (string.IsNullOrEmpty(projekt_))
                ComboBoxProjekty.SelectedIndex = 0;
            else
                ComboBoxProjekty.SelectedIndex = Convert.ToInt32(ComboBoxProjekty.FindStringExact(projekt_));

            ComboBoxZakaznici.SelectedIndex = Convert.ToInt32(ComboBoxZakaznici.FindStringExact(zakaznik_));

            ComboBoxZadavatel1.SelectedIndexChanged += ComboBoxZadavatel1_SelectedIndexChanged;
            ComboBoxZadavatel2.SelectedIndexChanged += ComboBoxZadavatel2_SelectedIndexChanged;
            ComboBoxProjekty.SelectedIndexChanged += ComboBoxProjekty_SelectedIndexChanged;
            ComboBoxZakaznici.SelectedIndexChanged += ComboBoxZakaznici_SelectedIndexChanged;
            //inicializace comboboxů ----------------------------------------------------------------------------
        }

        private void HistorieTerminu()
        {
            //zjištění posledního termínu
            var datumUkonceni = EditAPViewModel.GetDatumUkonceniAP(apId_).ToList();

            panelTerminy.Size = new Size(390, 550);
            panelTerminy.Name = "panelPrehledTerminu";
            panelTerminy.Location = new Point(290, 60);
            panelTerminy.BorderStyle = BorderStyle.FixedSingle;
            panelTerminy.BackColor = SystemColors.Control;
            panelTerminy.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            panelTerminy.AutoScroll = false;
            panelTerminy.HorizontalScroll.Enabled = false;
            panelTerminy.HorizontalScroll.Visible = false;
            panelTerminy.HorizontalScroll.Maximum = 0;
            panelTerminy.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelTerminy.AutoScroll = true;

            groupBoxNovyTermin.Controls.Add(panelTerminy);

            var datumUkonceniASC = datumUkonceni.OrderBy(index => index.UkonceniAPId);
            foreach (var du in datumUkonceni)
            {
                dateTimePickerDatumUkonceni.Value = du.DatumUkonceni.AddDays(1);
                dateTimePickerDatumUkonceni.MinDate = du.DatumUkonceni.AddDays(1);
                if (Convert.ToDateTime(du.DatumUkonceni.ToShortDateString()) < Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                {
                    dateTimePickerDatumUkonceni.Value = DateTime.Now;
                }

                break;
            }

            int i = 0;
            foreach (var du in datumUkonceniASC)
            {
                groupBoxTerminy.Add(new GroupBox()
                {
                    Name = "groupBoxTerminy" + i + 1,
                    Location = new Point(10, 10 + (i * 150)),
                    Size = new Size(340, 140),
                    Text = (i + 1).ToString() + ". term",
                });

                labelTerminyDatum.Add(new Label()
                {
                    Name = "labelTerminyDatum" + i + 1,
                    Location = new Point(10, 20),
                    AutoSize = true,
                    Text = du.DatumUkonceni.ToShortDateString(),
                    ForeColor = Color.Black
                });
                int delkaRetezce = 0;
                using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1)))
                {
                    SizeF size = graphics.MeasureString(du.DatumUkonceni.ToShortDateString(), new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Pixel));
                    delkaRetezce = Convert.ToInt32(size.Width);
                }

                richTextBoxTermin.Add(new RichTextBox()
                {
                    Name = "RichTextBoxTermin" + i + 1,
                    Location = new Point(10, 45),
                    Size = new Size(320, 80),
                    Text = du.Poznamka,
                    Enabled = false,
                    Tag = du.UkonceniAPId.ToString(),
                    ForeColor = Color.Black
                });

                i++;
            }

            for (i = 0; i < groupBoxTerminy.Count; i++)
            {
                panelTerminy.Controls.Add(groupBoxTerminy[i]);
                groupBoxTerminy[i].Controls.Add(labelTerminyDatum[i]);
                groupBoxTerminy[i].Controls.Add(richTextBoxTermin[i]);
            }
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
            var zamestnanci = EditAPViewModel.GetZamestnanci().ToList();

            if (zamestnanci.Count == 0)
            {
                return false;
            }
            else
            {
                List<Zam> zam1 = new List<Zam>
                {
                    new Zam("(select employees)", 0)
                };

                List<Zam> zam2 = new List<Zam>
                {
                    new Zam("(select employees)", 0)
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
            var projekty = EditAPViewModel.GetProjekty().ToList();

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
            var zakaznici = EditAPViewModel.GetZakaznici().ToList();

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
            zmenaDat = true;
            ButtonUlozit.Enabled = true;
        }

        private void ComboBoxZadavatel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            zmenaDat = true;
            ButtonUlozit.Enabled = true;
        }

        private void ComboBoxProjekty_SelectedIndexChanged(object sender, EventArgs e)
        {
            zmenaDat = true;
            ButtonUlozit.Enabled = true;
        }

        private void ComboBoxZakaznici_SelectedIndexChanged(object sender, EventArgs e)
        {
            zmenaDat = true;
            ButtonUlozit.Enabled = true;
        }

        private void ButtonUlozit_MouseClick(object sender, MouseEventArgs e)
        {
            UlozitAP();
        }

        private void UlozitAP()
        {
            int? zadavatel2ID;
            int? projektID;

            if (ComboBoxZadavatel2.SelectedIndex == 0)
                zadavatel2ID = null;
            else
                zadavatel2ID = Convert.ToInt32(ComboBoxZadavatel2.SelectedValue);

            if (ComboBoxProjekty.SelectedIndex == 0)
                projektID = null;
            else
                projektID = Convert.ToInt32(ComboBoxProjekty.SelectedValue);

            AkcniPlanyDataMapper.UpdateAP(apId_, Convert.ToInt32(ComboBoxZadavatel1.SelectedValue), zadavatel2ID,
                tema_, projektID, Convert.ToInt32(ComboBoxZakaznici.SelectedValue));

            //MessageBox.Show("Data has been saved.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

            zmenaDat = false;
            ButtonUlozit.Enabled = false;
        }

        private void ButtonNovyTermin_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to change the deadline of the action plan?.", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.Yes)
            {
                if (richTextBoxNovaPoznamka.Text == string.Empty)
                {
                    //Musí být vyplněn důvod prodloužení.
                    MessageBox.Show("The reason for the extension must be filled in.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    AkcniPlanyDataMapper.ZmenaTerminuAP(apId_, Convert.ToInt32(labelZbyvajiciPocetTerminu.Text) - 1, Convert.ToDateTime(dateTimePickerDatumUkonceni.Value), richTextBoxNovaPoznamka.Text);
                    dateTimePickerDatumUkonceni.Enabled = false;
                    labelPoznamkaTermin.Enabled = false;
                    richTextBoxNovaPoznamka.Enabled = false;
                    richTextBoxNovaPoznamka.Text = string.Empty;

                    var pocetTerminu = EditAPViewModel.GetPocetTerminu(apId_).ToList();
                    labelZbyvajiciPocetTerminu.Text = pocetTerminu[0].ZmenaTerminu.ToString();

                    RemoveControl();
                    InitForm();
                }
            }
        }

        private void ButtonUkoncitAP_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("You really want to close the Action plan.", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.Yes)
            {
                AkcniPlanyDataMapper.UpdateUkonceniAP(apId_);

                RemoveControl();
                InitForm();
            }
        }

        private void ButtonZnovuOtevrit_MouseClick(object sender, MouseEventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("You really want to reopen the AP.", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.Yes)
            {
                if (richTextBoxDuvodZnovuOtevreni.Text == string.Empty)
                {
                    MessageBox.Show("You must fill in the reason for reopening the Action plan.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    AkcniPlanyDataMapper.UpdateZnovuOtevritAP(apId_, richTextBoxDuvodZnovuOtevreni.Text);

                    RemoveControl();
                    InitForm();
                }
            }
        }

        private void FormEditAP_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (zmenaDat == true)
            {
                DialogResult dialogResult = MessageBox.Show("You want to save your changes.", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dialogResult == DialogResult.Yes)
                {
                    UlozitAP();
                }
                else if (dialogResult == DialogResult.No)
                {
                }
            }
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }
    }
}
