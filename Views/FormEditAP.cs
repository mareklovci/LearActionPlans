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
            this.InitializeComponent();

            this.apId_ = apId;
            this.cisloAPRok_ = cisloAPRok;
            this.zadavatel1_ = zadavatel1;
            this.zadavatel2_ = zadavatel2;
            this.tema_ = tema;
            this.projekt_ = projekt;
            this.zakaznik_ = zakaznik;

            this.zadavatel1Id_ = zadavatel1Id;
            this.zadavatel2Id_ = zadavatel2Id;
            this.projektId_ = projektId;
            this.zakaznikId_ = zakaznikId;

            this.zmenaDat = false;

            this.panelTerminy = new Panel();
            this.groupBoxTerminy = new List<GroupBox>();
            this.labelTerminyDatum = new List<Label>();
            this.richTextBoxTermin = new List<RichTextBox>();
        }

        private void FormEditAP_Load(object sender, EventArgs e)
        {
            this.labelNumber.Text = this.cisloAPRok_;

            this.InitForm();
        }

        private void InitForm()
        {
            //načíst všechny akce daného AP a zjistit, jestli mají všechny vyplněné datum efektivnosti
            //jestli ano, tak mohu aktivovat tlačítko pro ukončení AP
            //po uzavření AP uložím datum ukončení
            var uzavreneAkce = EditAPViewModel.GetUkonceniAkce(this.apId_).ToList();
            var znovuOtevrit = EditAPViewModel.GetZnovuOtevritAP(this.apId_).ToList();

            var akceUkonceny = true;
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
            this.ButtonZnovuOtevrit.Enabled = false;
            this.labelDuvodZnovuOtevreni.Enabled = false;
            this.richTextBoxDuvodZnovuOtevreni.Enabled = false;
            this.richTextBoxDuvodZnovuOtevreni.Text = string.Empty;
            //když už byl AP uzavřen a ještě nebyl Reopen, bude tlačítko Reopen aktivní
            this.znovuOtevritAP = znovuOtevrit[0].ZnovuOtevrit;
            //AP byl uzavřen a ještě nebyl znovuotevřen
            if (!(znovuOtevrit[0].UzavreniAP == null) && this.znovuOtevritAP == 1)
            {
                this.ButtonZnovuOtevrit.Enabled = true;
                this.labelDuvodZnovuOtevreni.Enabled = true;
                this.richTextBoxDuvodZnovuOtevreni.Enabled = true;
            }
            if (this.znovuOtevritAP == 0)
            {
                this.ButtonZnovuOtevrit.Visible = false;
                this.richTextBoxDuvodZnovuOtevreni.Text = znovuOtevrit[0].DuvodZnovuotevreni;
            }

            this.labelDatumUzavreni.Text = string.Empty;
            if (!(znovuOtevrit[0].UzavreniAP == null))
            {
                this.labelDatumUzavreni.Text = Convert.ToDateTime(znovuOtevrit[0].UzavreniAP).ToShortDateString();
            }

            //počet zbývajícíh termínů
            var pocetTerminu = EditAPViewModel.GetPocetTerminu(this.apId_).ToList();
            this.labelZbyvajiciPocetTerminu.Text = pocetTerminu[0].ZmenaTerminu.ToString();
            //pokud ještě mám volné termíny a AP nebyl uzavřen
            if (pocetTerminu[0].ZmenaTerminu == 0 || !(znovuOtevrit[0].UzavreniAP == null))
            {
                this.dateTimePickerDatumUkonceni.Enabled = false;
                this.labelPoznamkaTermin.Enabled = false;
                this.richTextBoxNovaPoznamka.Enabled = false;

                this.ButtonNovyTermin.Enabled = false;
            }
            else
            {
                this.dateTimePickerDatumUkonceni.Enabled = true;
                this.labelPoznamkaTermin.Enabled = true;
                this.richTextBoxNovaPoznamka.Enabled = true;

                this.ButtonNovyTermin.Enabled = true;
            }
            //počet zbývajícíh termínů

            if (pocetTerminu[0].ZmenaTerminu == 0)
            {
                this.dateTimePickerDatumUkonceni.Visible = false;
                this.labelPoznamkaTermin.Visible = false;
                this.richTextBoxNovaPoznamka.Visible = false;
                this.ButtonNovyTermin.Visible = false;
            }

            this.ButtonUkoncitAP.Enabled = false;
            //v případě, že jsou všechny akce uzavřené a AP ještě nebyl uzavřen, povolím tlačítko pro ukončení AP
            if (akceUkonceny == true && znovuOtevrit[0].UzavreniAP == null)
            {
                this.ButtonUkoncitAP.Enabled = true;
            }
            //v případě, že nebyl AP ještě uzavřen, mohu upravovat jeho parametry
            if (znovuOtevrit[0].UzavreniAP == null)
            {
                this.InitComboBox();
            }
            else
            {
                this.ComboBoxZadavatel1.SelectedIndexChanged -= this.ComboBoxZadavatel1_SelectedIndexChanged;
                this.ComboBoxZadavatel2.SelectedIndexChanged -= this.ComboBoxZadavatel2_SelectedIndexChanged;
                this.ComboBoxProjekty.SelectedIndexChanged -= this.ComboBoxProjekty_SelectedIndexChanged;
                this.ComboBoxZakaznici.SelectedIndexChanged -= this.ComboBoxZakaznici_SelectedIndexChanged;

                this.ComboBoxZadavatel1.Visible = false;
                this.ComboBoxZadavatel2.Visible = false;
                this.ComboBoxProjekty.Visible = false;
                this.ComboBoxZakaznici.Visible = false;

                //tlačítko pro uzavření AP musí být neaktivní, když je aktivní tlačítko pro Reopen
                this.ButtonUkoncitAP.Enabled = false;
                var zamestnanec1 = EditAPViewModel.GetZamestnanecId(this.zadavatel1Id_).ToList();
                this.labelZam1.Location = new Point(20, 105);
                this.labelZam1.Visible = true;
                this.labelZam1.AutoSize = true;
                this.labelZam1.Text = zamestnanec1[0].Jmeno;
                this.groupBoxEditAP.Controls.Add(this.labelZam1);

                if (this.zadavatel2Id_ == null) { }
                else
                {
                    var zamestnanec2 = EditAPViewModel.GetZamestnanecId(Convert.ToInt32(this.zadavatel2Id_)).ToList();
                    this.labelZam2.Location = new Point(20, 165);
                    this.labelZam2.Visible = true;
                    this.labelZam1.AutoSize = true;
                    this.labelZam2.Text = zamestnanec2[0].Jmeno;
                    this.groupBoxEditAP.Controls.Add(this.labelZam2);
                }
                if (this.projektId_ == null) { }
                else
                {
                    var projekt = EditAPViewModel.GetProjektId(Convert.ToInt32(this.projektId_)).ToList();
                    this.labelProjekt.Location = new Point(20, 225);
                    this.labelProjekt.Visible = true;
                    this.labelProjekt.AutoSize = true;
                    this.labelProjekt.Text = projekt[0].NazevProjektu;
                    this.groupBoxEditAP.Controls.Add(this.labelProjekt);
                }
                var zakaznik = EditAPViewModel.GetZakaznikId(Convert.ToInt32(this.zakaznikId_)).ToList();
                this.labelZakaznik.Location = new Point(20, 285);
                this.labelZakaznik.Visible = true;
                this.labelZakaznik.AutoSize = true;
                this.labelZakaznik.Text = zakaznik[0].NazevZakaznika;
                this.groupBoxEditAP.Controls.Add(this.labelZakaznik);
            }

            this.ButtonUlozit.Enabled = false;

            this.HistorieTerminu();
        }

        private void RemoveControl()
        {
            for (var i = this.groupBoxTerminy.Count - 1; i >= 0; i--)
            {
                this.groupBoxTerminy[i].Controls.Remove(this.labelTerminyDatum[i]);
                this.groupBoxTerminy[i].Controls.Remove(this.richTextBoxTermin[i]);

                this.panelTerminy.Controls.Remove(this.groupBoxTerminy[i]);

                this.labelTerminyDatum[i].Dispose();
                this.richTextBoxTermin[i].Dispose();

                this.groupBoxTerminy[i].Dispose();
            }

            this.Controls.Remove(this.panelTerminy);
            this.panelTerminy.Dispose();

            this.panelTerminy = new Panel();
            this.groupBoxTerminy = new List<GroupBox>();
            this.labelTerminyDatum = new List<Label>();
            this.richTextBoxTermin = new List<RichTextBox>();
        }

        private void InitComboBox()
        {
            this.ComboBoxZadavatel1.SelectedIndexChanged -= this.ComboBoxZadavatel1_SelectedIndexChanged;
            this.ComboBoxZadavatel2.SelectedIndexChanged -= this.ComboBoxZadavatel2_SelectedIndexChanged;
            this.ComboBoxProjekty.SelectedIndexChanged -= this.ComboBoxProjekty_SelectedIndexChanged;
            this.ComboBoxZakaznici.SelectedIndexChanged -= this.ComboBoxZakaznici_SelectedIndexChanged;

            //inicializace comboboxů ----------------------------------------------------------------------------
            this.labelZam1.Visible = false;
            this.labelZam2.Visible = false;
            this.labelProjekt.Visible = false;
            this.labelZakaznik.Visible = false;

            this.ComboBoxZadavatel1.Visible = true;
            this.ComboBoxZadavatel2.Visible = true;
            this.ComboBoxProjekty.Visible = true;
            this.ComboBoxZakaznici.Visible = true;

            this.NaplnitComboBoxZamestnanec1a2();
            this.NaplnitComboBoxProjekty();
            this.NaplnitComboBoxZakaznici();

            this.ComboBoxZadavatel1.SelectedIndex = Convert.ToInt32(this.ComboBoxZadavatel1.FindStringExact(this.zadavatel1_));
            if (string.IsNullOrEmpty(this.zadavatel2_))
            {
                this.ComboBoxZadavatel2.SelectedIndex = 0;
            }
            else
            {
                this.ComboBoxZadavatel2.SelectedIndex = Convert.ToInt32(this.ComboBoxZadavatel2.FindStringExact(this.zadavatel2_));
            }

            if (string.IsNullOrEmpty(this.projekt_))
            {
                this.ComboBoxProjekty.SelectedIndex = 0;
            }
            else
            {
                this.ComboBoxProjekty.SelectedIndex = Convert.ToInt32(this.ComboBoxProjekty.FindStringExact(this.projekt_));
            }

            this.ComboBoxZakaznici.SelectedIndex = Convert.ToInt32(this.ComboBoxZakaznici.FindStringExact(this.zakaznik_));

            this.ComboBoxZadavatel1.SelectedIndexChanged += this.ComboBoxZadavatel1_SelectedIndexChanged;
            this.ComboBoxZadavatel2.SelectedIndexChanged += this.ComboBoxZadavatel2_SelectedIndexChanged;
            this.ComboBoxProjekty.SelectedIndexChanged += this.ComboBoxProjekty_SelectedIndexChanged;
            this.ComboBoxZakaznici.SelectedIndexChanged += this.ComboBoxZakaznici_SelectedIndexChanged;
            //inicializace comboboxů ----------------------------------------------------------------------------
        }

        private void HistorieTerminu()
        {
            //zjištění posledního termínu
            var datumUkonceni = EditAPViewModel.GetDatumUkonceniAP(this.apId_).ToList();

            this.panelTerminy.Size = new Size(390, 550);
            this.panelTerminy.Name = "panelPrehledTerminu";
            this.panelTerminy.Location = new Point(290, 60);
            this.panelTerminy.BorderStyle = BorderStyle.FixedSingle;
            this.panelTerminy.BackColor = SystemColors.Control;
            this.panelTerminy.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            this.panelTerminy.AutoScroll = false;
            this.panelTerminy.HorizontalScroll.Enabled = false;
            this.panelTerminy.HorizontalScroll.Visible = false;
            this.panelTerminy.HorizontalScroll.Maximum = 0;
            this.panelTerminy.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.panelTerminy.AutoScroll = true;

            this.groupBoxNovyTermin.Controls.Add(this.panelTerminy);

            var datumUkonceniASC = datumUkonceni.OrderBy(index => index.UkonceniAPId);
            foreach (var du in datumUkonceni)
            {
                this.dateTimePickerDatumUkonceni.Value = du.DatumUkonceni.AddDays(1);
                this.dateTimePickerDatumUkonceni.MinDate = du.DatumUkonceni.AddDays(1);
                if (Convert.ToDateTime(du.DatumUkonceni.ToShortDateString()) < Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                {
                    this.dateTimePickerDatumUkonceni.Value = DateTime.Now;
                }

                break;
            }

            var i = 0;
            foreach (var du in datumUkonceniASC)
            {
                this.groupBoxTerminy.Add(new GroupBox()
                {
                    Name = "groupBoxTerminy" + i + 1,
                    Location = new Point(10, 10 + (i * 150)),
                    Size = new Size(340, 140),
                    Text = (i + 1).ToString() + ". term",
                });

                this.labelTerminyDatum.Add(new Label()
                {
                    Name = "labelTerminyDatum" + i + 1,
                    Location = new Point(10, 20),
                    AutoSize = true,
                    Text = du.DatumUkonceni.ToShortDateString(),
                    ForeColor = Color.Black
                });
                var delkaRetezce = 0;
                using (var graphics = Graphics.FromImage(new Bitmap(1, 1)))
                {
                    var size = graphics.MeasureString(du.DatumUkonceni.ToShortDateString(), new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Pixel));
                    delkaRetezce = Convert.ToInt32(size.Width);
                }

                this.richTextBoxTermin.Add(new RichTextBox()
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

            for (i = 0; i < this.groupBoxTerminy.Count; i++)
            {
                this.panelTerminy.Controls.Add(this.groupBoxTerminy[i]);
                this.groupBoxTerminy[i].Controls.Add(this.labelTerminyDatum[i]);
                this.groupBoxTerminy[i].Controls.Add(this.richTextBoxTermin[i]);
            }
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
            var zamestnanci = EditAPViewModel.GetZamestnanci().ToList();

            if (zamestnanci.Count == 0)
            {
                return false;
            }
            else
            {
                var zam1 = new List<Zam>
                {
                    new Zam("(select employees)", 0)
                };

                var zam2 = new List<Zam>
                {
                    new Zam("(select employees)", 0)
                };

                foreach (var z in zamestnanci)
                {
                    zam1.Add(new Zam(z.Jmeno, z.ZamestnanecId));
                    zam2.Add(new Zam(z.Jmeno, z.ZamestnanecId));
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
            var projekty = EditAPViewModel.GetProjekty().ToList();

            if (projekty.Count == 0)
            {
                return false;
            }
            else
            {
                var proj = new List<Proj>
                {
                    new Proj("(select a project)", 0)
                };
                foreach (var p in projekty)
                {
                    proj.Add(new Proj(p.NazevProjektu, p.ProjektId));
                }

                this.ComboBoxProjekty.DataSource = proj;
                this.ComboBoxProjekty.DisplayMember = "NazevProjektu";
                this.ComboBoxProjekty.ValueMember = "ProjektId";
                this.ComboBoxProjekty.SelectedIndex = 0;
                return true;
            }
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
            var zakaznici = EditAPViewModel.GetZakaznici().ToList();

            if (zakaznici.Count == 0)
            {
                return false;
            }
            else
            {
                var zak = new List<Zak>
                {
                    new Zak("(choose a customer)", 0)
                };
                //new Zak() { NazevZakaznika = "(choose a customer)", ZakaznikId = 0 }
                foreach (var z in zakaznici)
                {
                    zak.Add(new Zak(z.NazevZakaznika, z.ZakaznikId));
                }
                //zak.Add(new Zak() { NazevZakaznika = z.NazevZakaznika, ZakaznikId = z.ZakaznikId });
                this.ComboBoxZakaznici.DataSource = zak;
                this.ComboBoxZakaznici.DisplayMember = "NazevZakaznika";
                this.ComboBoxZakaznici.ValueMember = "ZakaznikId";
                this.ComboBoxZakaznici.SelectedIndex = 0;
                return true;
            }
        }

        private void ComboBoxZadavatel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.zmenaDat = true;
            this.ButtonUlozit.Enabled = true;
        }

        private void ComboBoxZadavatel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.zmenaDat = true;
            this.ButtonUlozit.Enabled = true;
        }

        private void ComboBoxProjekty_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.zmenaDat = true;
            this.ButtonUlozit.Enabled = true;
        }

        private void ComboBoxZakaznici_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.zmenaDat = true;
            this.ButtonUlozit.Enabled = true;
        }

        private void ButtonUlozit_MouseClick(object sender, MouseEventArgs e)
        {
            this.UlozitAP();
        }

        private void UlozitAP()
        {
            int? zadavatel2ID;
            int? projektID;

            if (this.ComboBoxZadavatel2.SelectedIndex == 0)
            {
                zadavatel2ID = null;
            }
            else
            {
                zadavatel2ID = Convert.ToInt32(this.ComboBoxZadavatel2.SelectedValue);
            }

            if (this.ComboBoxProjekty.SelectedIndex == 0)
            {
                projektID = null;
            }
            else
            {
                projektID = Convert.ToInt32(this.ComboBoxProjekty.SelectedValue);
            }

            AkcniPlanyDataMapper.UpdateAP(this.apId_, Convert.ToInt32(this.ComboBoxZadavatel1.SelectedValue), zadavatel2ID, this.tema_, projektID, Convert.ToInt32(this.ComboBoxZakaznici.SelectedValue));

            //MessageBox.Show("Data has been saved.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.zmenaDat = false;
            this.ButtonUlozit.Enabled = false;
        }

        private void ButtonNovyTermin_MouseClick(object sender, MouseEventArgs e)
        {
            var dialogResult = MessageBox.Show("Do you really want to change the deadline of the action plan?.", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.Yes)
            {
                if (this.richTextBoxNovaPoznamka.Text == string.Empty)
                {
                    //Musí být vyplněn důvod prodloužení.
                    MessageBox.Show("The reason for the extension must be filled in.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    AkcniPlanyDataMapper.ZmenaTerminuAP(this.apId_, Convert.ToInt32(this.labelZbyvajiciPocetTerminu.Text) - 1, Convert.ToDateTime(this.dateTimePickerDatumUkonceni.Value), this.richTextBoxNovaPoznamka.Text);
                    this.dateTimePickerDatumUkonceni.Enabled = false;
                    this.labelPoznamkaTermin.Enabled = false;
                    this.richTextBoxNovaPoznamka.Enabled = false;
                    this.richTextBoxNovaPoznamka.Text = string.Empty;

                    var pocetTerminu = EditAPViewModel.GetPocetTerminu(this.apId_).ToList();
                    this.labelZbyvajiciPocetTerminu.Text = pocetTerminu[0].ZmenaTerminu.ToString();

                    this.RemoveControl();
                    this.InitForm();
                }
            }
        }

        private void ButtonUkoncitAP_MouseClick(object sender, MouseEventArgs e)
        {
            var dialogResult = MessageBox.Show("You really want to close the Action plan.", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.Yes)
            {
                AkcniPlanyDataMapper.UpdateUkonceniAP(this.apId_);

                this.RemoveControl();
                this.InitForm();
            }
        }

        private void ButtonZnovuOtevrit_MouseClick(object sender, MouseEventArgs e)
        {

            var dialogResult = MessageBox.Show("You really want to reopen the AP.", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.Yes)
            {
                if (this.richTextBoxDuvodZnovuOtevreni.Text == string.Empty)
                {
                    MessageBox.Show("You must fill in the reason for reopening the Action plan.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    AkcniPlanyDataMapper.UpdateZnovuOtevritAP(this.apId_, this.richTextBoxDuvodZnovuOtevreni.Text);

                    this.RemoveControl();
                    this.InitForm();
                }
            }
        }

        private void FormEditAP_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.zmenaDat == true)
            {
                var dialogResult = MessageBox.Show("You want to save your changes.", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dialogResult == DialogResult.Yes)
                {
                    this.UlozitAP();
                }
                else if (dialogResult == DialogResult.No)
                {
                }
            }
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
    }
}
