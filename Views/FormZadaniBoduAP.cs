using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using LearActionPlans.ViewModels;

namespace LearActionPlans.Views
{
    public partial class FormZadaniBoduAP : Form
    {
        private readonly FormNovyAkcniPlan.AkcniPlanTmp akcniPlany_;

        private readonly string cisloAPStr_;
        //private DataTable dtActionsWM;
        //private DataTable dtActionsWS;

        private readonly bool spusteniBezParametru;

        private bool novyBodAP;
        private bool bodAPUlozen;

        private int cisloRadkyDGVBody;

        //private readonly BindingSource _bindingSourceWM = new BindingSource();
        //private readonly BindingSource _bindingSourceWS = new BindingSource();

        private bool changedDGV;

        private byte znovuOtevritAP;
        private DateTime? kontrolaEfektivnostiDatum;
        private string priloha;
        private DateTime? datumUkonceni;
        private string poznamkaDatumUkonceni;
        private bool deadLineZadan;

        public FormZadaniBoduAP(bool spusteniBezParametru, string cisloAPStr, FormNovyAkcniPlan.AkcniPlanTmp akcniPlany, int cisloRadkyDGV,
            bool novyBod)
        {
            this.InitializeComponent();
            //this.dtActionsWM = new DataTable();
            //this.dtActionsWS = new DataTable();

            this.spusteniBezParametru = spusteniBezParametru;

            this.cisloAPStr_ = cisloAPStr;
            this.akcniPlany_ = akcniPlany;

            this.cisloRadkyDGVBody = cisloRadkyDGV;
            this.novyBodAP = novyBod;
            this.bodAPUlozen = false;

            this.kontrolaEfektivnostiDatum = null;
            this.priloha = string.Empty;
            this.datumUkonceni = null;
            this.poznamkaDatumUkonceni = string.Empty;
        }

        private void FormZadaniBoduAP_Load(object sender, EventArgs e)
        {
            if (this.NaplnitComboBoxZamestnanec1a2() == false)
            {
                this.ComboBoxOdpovednaOsoba1.Enabled = false;
                this.ComboBoxOdpovednaOsoba2.Enabled = false;
                //Nejsou dostupní žádní zaměstnanci.
                //Zadání nového Akčního plánu bude ukončeno.
                MessageBox.Show("No employees available." + (char)10 + "Entering a new Action plan will be completed.",
                    "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                this.ComboBoxOdpovednaOsoba1.Enabled = true;
                this.ComboBoxOdpovednaOsoba2.Enabled = true;
            }

            if (this.NaplnitComboBoxOddeleni() == false)
            {
                this.ComboBoxOddeleni.Enabled = false;
                //Nejsou dostupné žádné projekty.
                //Zadání nového Akčního plánu bude ukončeno.
                MessageBox.Show(
                    "No departments available." + (char)10 + "Entering a new Action plan will be completed.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                this.ComboBoxOddeleni.Enabled = true;
            }

            this.labelDatumUkonceni.Text = string.Empty;
            this.labelEfektivita.Text = string.Empty;

            //this._bindingSourceWM.DataSource = this.dtActionsWM;
            //this.DataGridViewWMAkce.DataSource = this._bindingSourceWM;

            //this._bindingSourceWS.DataSource = this.dtActionsWS;
            //this.DataGridViewWSAkce.DataSource = this._bindingSourceWS;

            this.deadLineZadan = true;

            var znovuOtevrit = EditAPViewModel.GetZnovuOtevritAP(this.akcniPlany_.Id).ToList();
            //jestliže proměnná znovuotevritAP je rovna 0, byl AP znovuotevřen
            this.znovuOtevritAP = znovuOtevrit[0].ZnovuOtevrit;

            if (this.TestZamOdd() != true)
            {
                return;
            }

            this.ZobrazeniDGV();

            this.ButtonUlozit.Enabled = false;
            this.changedDGV = false;

            if (this.akcniPlany_.APUzavren)
            {
                this.TextBoxOdkazNaNormu.Enabled = false;
                this.TextBoxHodnoceniNeshody.Enabled = false;
                this.RichTextBoxPopisProblemu.Enabled = false;
                this.RichTextBoxSkutecnaPricinaWM.Enabled = false;

                this.ButtonNovaAkce.Visible = false;
                this.ButtonOdstranitAkci.Visible = false;
                this.ButtonUlozit.Visible = false;
                this.ButtonZavrit.Text = "Close";
            }
            else
            {
                this.TextBoxOdkazNaNormu.Enabled = true;
                this.TextBoxHodnoceniNeshody.Enabled = true;
                this.RichTextBoxPopisProblemu.Enabled = true;
                this.RichTextBoxSkutecnaPricinaWM.Enabled = true;

                this.ButtonUlozit.Visible = true;
                this.ButtonUlozit.Text = "Save";
                this.ButtonZavrit.Text = "Close";
                this.ButtonNovaAkce.Visible = true;
                this.ButtonOdstranitAkci.Visible = true;
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
            var zamestnanci = ZadaniBoduAPViewModel.GetZamestnanciAll().ToList();

            if (zamestnanci.Count == 0)
            {
                return false;
            }

            var zam1 = new List<Zam> {new Zam("(select employee)", 0)};

            var zam2 = new List<Zam> {new Zam("(select employee)", 0)};

            foreach (var z in zamestnanci)
            {
                zam1.Add(new Zam(z.Jmeno, z.ZamestnanecId));
                zam2.Add(new Zam(z.Jmeno, z.ZamestnanecId));
            }

            this.ComboBoxOdpovednaOsoba1.DataSource = zam1;
            this.ComboBoxOdpovednaOsoba2.DataSource = zam2;
            this.ComboBoxOdpovednaOsoba1.DisplayMember = "Jmeno";
            this.ComboBoxOdpovednaOsoba2.DisplayMember = "Jmeno";
            this.ComboBoxOdpovednaOsoba1.ValueMember = "ZamestnanecId";
            this.ComboBoxOdpovednaOsoba2.ValueMember = "ZamestnanecId";
            this.ComboBoxOdpovednaOsoba1.SelectedIndex = 0;
            this.ComboBoxOdpovednaOsoba2.SelectedIndex = 0;
            return true;
        }

        private class Oddeleni
        {
            public string NazevOddeleni { get; set; }
            public int OddeleniId { get; set; }

            public Oddeleni(string nazevOddeleni, int oddeleniId)
            {
                this.NazevOddeleni = nazevOddeleni;
                this.OddeleniId = oddeleniId;
            }
        }

        private bool NaplnitComboBoxOddeleni()
        {
            var oddeleni = ZadaniBoduAPViewModel.GetOddeleniAll().ToList();

            if (oddeleni.Count == 0)
            {
                return false;
            }

            var odd = new List<Oddeleni> {new Oddeleni("(select a department)", 0)};
            odd.AddRange(oddeleni.Select(o => new Oddeleni(o.Nazev, o.OddeleniId_)));

            this.ComboBoxOddeleni.DataSource = odd;
            this.ComboBoxOddeleni.DisplayMember = "NazevOddeleni";
            this.ComboBoxOddeleni.ValueMember = "OddeleniId";
            this.ComboBoxOddeleni.SelectedIndex = 0;
            return true;
        }

        private bool TestZamOdd()
        {
            //pokud nebudou zaměstnanci nebo oddělení vrátím false a nemohu pokračovat dál
            var zam = ZadaniBoduAPViewModel.GetZamestnanciAll().ToList();
            if (zam[0] == null)
            {
                MessageBox.Show("No Employees available.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            var oddeleni = ZadaniBoduAPViewModel.GetOddeleniAll().ToList();
            if (oddeleni[0] != null)
            {
                return true;
            }

            MessageBox.Show("No Departments are available.", "Notice", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return false;
        }

        private void ZobrazeniDGV()
        {
            if (this.novyBodAP == false)
            {
                this.TextBoxOdkazNaNormu.Text = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].OdkazNaNormu;
                this.TextBoxHodnoceniNeshody.Text = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].HodnoceniNeshody;
                this.RichTextBoxPopisProblemu.Text = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].PopisProblemu;
                this.ComboBoxOdpovednaOsoba1.SelectedValue =
                    FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].OdpovednaOsoba1Id;
                if (FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].OdpovednaOsoba2Id == null)
                { }
                else
                {
                    this.ComboBoxOdpovednaOsoba2.SelectedValue =
                        FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].OdpovednaOsoba2Id;
                }

                this.ComboBoxOddeleni.SelectedValue = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].OddeleniId;

                if (FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Priloha == null)
                { }

                this.labelDatumUkonceni.Text = string.Empty;
                this.labelEfektivita.Text = string.Empty;

                if (FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].UkonceniBodAP == null)
                {
                    this.deadLineZadan = false;
                }
                else
                {
                    var i = 0;
                    var copyBodAP = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].UkonceniBodAP.ToList();
                    copyBodAP.Reverse();
                    foreach (var c in copyBodAP)
                    {
                        if (i == 0)
                        {
                            // jestliže poslední záznam pro daný bod je žádost nebo nepotvrzení žádosti, tak je nastavím prom deadLineZadan na false
                            // tedy deadline není zadán
                            if (c.StavZadosti == 3 || c.StavZadosti == 6)
                            {
                                this.deadLineZadan = false;
                            }
                        }

                        if (c.StavZadosti == 1 || c.StavZadosti == 2 || c.StavZadosti == 4 || c.StavZadosti == 5)
                        {
                            this.labelDatumUkonceni.Text = c.DatumUkonceni.ToShortDateString();
                            break;
                        }

                        i++;
                    }
                }

                if (this.deadLineZadan == false)
                {
                    this.ButtonKontrolaEfektivnosti.Enabled = false;
                }
                else
                {
                    if (FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].KontrolaEfektivnosti == null)
                    {
                        this.ButtonKontrolaEfektivnosti.Enabled = true;
                    }
                    else
                    {
                        this.ButtonKontrolaEfektivnosti.Enabled = true;
                        this.labelEfektivita.Text = Convert
                            .ToDateTime(FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].KontrolaEfektivnosti)
                            .ToShortDateString();
                        this.kontrolaEfektivnostiDatum = Convert.ToDateTime(FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].KontrolaEfektivnosti);
                        this.ZablokovatPole();
                    }
                }

                this.RichTextBoxSkutecnaPricinaWM.Text =
                    FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].SkutecnaPricinaWM;
                this.RichTextBoxNapravnaOpatreniWM.Text =
                    FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].NapravnaOpatreniWM;
                this.RichTextBoxSkutecnaPricinaWS.Text =
                    FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].SkutecnaPricinaWS;
                this.RichTextBoxNapravnaOpatreniWS.Text =
                    FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].NapravnaOpatreniWS;
            }
            else
            {
                this.ButtonKontrolaEfektivnosti.Enabled = false;
            }


            this.ComboBoxOdpovednaOsoba1.SelectedIndexChanged += this.ComboBoxOdpovednaOsoba1_SelectedIndexChanged;
            this.ComboBoxOddeleni.SelectedIndexChanged += this.ComboBoxOddeleni_SelectedIndexChanged;
        }

        private void ButtonUlozit_MouseClick(object sender, MouseEventArgs e)
        {
            this.UlozitBodAP();
            this.ButtonUlozit.Enabled = false;
            this.changedDGV = false;
            this.bodAPUlozen = true;
        }

        //private void ButtonNovaAkce_MouseClick(object sender, MouseEventArgs e)
        //{
        //    //if (this.TabControlAkce.SelectedTab == this.TabControlAkce.TabPages["tabPageWM"]) //your specific tabname
        //    //{
        //    //    this.dtActionsWM.Rows.Add(string.Empty, 0, 0, null, null, null, null, null, 0, string.Empty, null, 0,
        //    //        false);
        //    //    this.ButtonOdstranitAkci.Enabled = this.DataGridViewWMAkce.Rows.Count > 0;
        //    //}

        //    //if (this.TabControlAkce.SelectedTab == this.TabControlAkce.TabPages["tabPageWS"]) //your specific tabname
        //    //{
        //    //    this.dtActionsWS.Rows.Add(string.Empty, 0, 0, null, null, null, null, null, 0, string.Empty, null, 0,
        //    //        false);
        //    //    this.ButtonOdstranitAkci.Enabled = this.DataGridViewWSAkce.Rows.Count > 0;
        //    //}

        //    this.ButtonUlozit.Enabled = true;
        //}

        //private void TabControlAkce_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (this.TabControlAkce.SelectedTab == this.TabControlAkce.TabPages["tabPageWM"])
        //    {
        //        this.ButtonOdstranitAkci.Enabled = this.DataGridViewWMAkce.Rows.Count > 0;
        //    }

        //    if (this.TabControlAkce.SelectedTab == this.TabControlAkce.TabPages["tabPageWS"])
        //    {
        //        this.ButtonOdstranitAkci.Enabled = this.DataGridViewWSAkce.Rows.Count > 0;
        //    }
        //}

        private void ComboBoxOdpovednaOsoba1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ButtonUlozit.Enabled = true;
            this.changedDGV = true;

            if (this.ComboBoxOdpovednaOsoba1.SelectedIndex == 0)
            {
                MessageBox.Show("A responsible employee must be selected.", "Notice", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void ComboBoxOddeleni_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ButtonUlozit.Enabled = true;
            this.changedDGV = true;

            if (this.ComboBoxOddeleni.SelectedIndex == 0)
            {
                MessageBox.Show("A department must be selected.", "Notice", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void RichTextBoxPopisProblemu_TextChanged(object sender, EventArgs e)
        {
            this.ButtonUlozit.Enabled = true;
            this.changedDGV = true;
        }

        private void RichTextBoxSkutecnaPricinaWM_TextChanged(object sender, EventArgs e)
        {
            this.ButtonUlozit.Enabled = true;
            this.changedDGV = true;
        }

        private void RichTextBoxNapravnaOpatreniWM_TextChanged(object sender, EventArgs e)
        {
            this.ButtonUlozit.Enabled = true;
            this.changedDGV = true;
        }

        private void RichTextBoxSkutecnaPricinaWS_TextChanged(object sender, EventArgs e)
        {
            this.ButtonUlozit.Enabled = true;
            this.changedDGV = true;
        }

        private void RichTextBoxNapravnaOpatreniWS_TextChanged(object sender, EventArgs e)
        {
            this.ButtonUlozit.Enabled = true;
            this.changedDGV = true;
        }

        private void DataGridViewWMAkce_SelectionChanged(object sender, EventArgs e) =>
            this.DataGridViewWMAkce.ClearSelection();

        private void DataGridViewWSAkce_SelectionChanged(object sender, EventArgs e) =>
            this.DataGridViewWSAkce.ClearSelection();

        private void ButtonTerminUkonceni_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.spusteniBezParametru == true)
            {
                // (this.cisloRadkyDGVBody > -1 ? (FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].UkonceniBodAP == null ? true : false) : false)
                if (this.novyBodAP == true || (this.cisloRadkyDGVBody > -1 && FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].UkonceniBodAP == null))
                {
                    using var form = new FormDatumUkonceni(this.datumUkonceni, this.poznamkaDatumUkonceni);
                    var result = form.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        return;
                    }

                    this.datumUkonceni = form.ReturnValueDatum;
                    this.poznamkaDatumUkonceni = form.ReturnValuePoznamka;
                    this.labelDatumUkonceni.Text = Convert.ToDateTime(this.datumUkonceni).ToShortDateString();
                    this.deadLineZadan = true;
                    this.changedDGV = true;
                    // bylo zadáno datum ukončení, tak bude povolena možnost zadat také fatu efektivity
                    this.ButtonKontrolaEfektivnosti.Enabled = true;
                }
                else
                {
                    // bude false, když není zadaná kontrola efektivnosti
                    var kontrolaEfektivnosti =
                        !(FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].KontrolaEfektivnosti == null);
                    var opravitTermin = !(this.akcniPlany_.APUzavren || kontrolaEfektivnosti);

                    using var form = new FormPosunutiTerminuBodAP(this.spusteniBezParametru, opravitTermin, this.cisloAPStr_, this.cisloRadkyDGVBody, this.akcniPlany_.Zadavatel1Id, this.akcniPlany_.Zadavatel2Id);
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    { }
                }
            }

        }

        private void ButtonKontrolaEfektivnosti_MouseClick(object sender, MouseEventArgs e)
        {
            var kontrolaEfektivnostiUlozena = false;
            var bodAPId = 0;
            DateTime? datumKontrolEfekt = null;

            if (this.novyBodAP)
            {
                if (this.kontrolaEfektivnostiDatum == null)
                {
                    kontrolaEfektivnostiUlozena = false;
                }
                else
                {
                    datumKontrolEfekt = this.kontrolaEfektivnostiDatum;
                }
            }
            else
            {
                //není zadána kontrola efektivnosti
                bodAPId = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Id;
                if (FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].KontrolaEfektivnosti != null)
                //{
                //    kontrolaEfektivnosti = false;
                //}
                //else
                {
                    datumKontrolEfekt = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].KontrolaEfektivnosti;
                    kontrolaEfektivnostiUlozena = true;
                }
            }

            using var form = new FormKontrolaEfektivnosti(this.novyBodAP, this.akcniPlany_.APUzavren,
                this.deadLineZadan, bodAPId, kontrolaEfektivnostiUlozena, datumKontrolEfekt);
            //var result = form.ShowDialog();
            form.ShowDialog();
            var result = form.ReturnValuePotvrdit;
            if (!result)
            {
                return;
            }

            var datumKontrolaEfektivnosti = form.ReturnValueDatum;
            var poznamka = form.ReturnValuePoznamka;
            var puvodniDatum = form.ReturnValuePuvodniDatum;
            if (this.novyBodAP)
            {
                if (datumKontrolaEfektivnosti == null)
                {
                    // datum efektivity bylo odstraněno
                    this.kontrolaEfektivnostiDatum = null;
                    this.labelEfektivita.Text = string.Empty;
                }
                else
                {
                    // zapamtuji si datum pro efektivitu
                    this.kontrolaEfektivnostiDatum = datumKontrolaEfektivnosti;
                    this.labelEfektivita.Text = Convert.ToDateTime(datumKontrolaEfektivnosti).ToShortDateString();
                }
            }
            else
            {
                if (datumKontrolaEfektivnosti == null)
                {
                    //povolit editaci
                    FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].KontrolaEfektivnosti = null;
                    FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].KontrolaEfektivnostiPuvodniDatum = puvodniDatum;
                    FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].KontrolaEfektivnostiOdstranit = poznamka;
                    this.labelEfektivita.Text = string.Empty;

                    this.OdblokovatPole();
                }
                else
                {
                    //zablokovat editaci
                    FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].KontrolaEfektivnosti = datumKontrolaEfektivnosti;
                    FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].KontrolaEfektivnostiPuvodniDatum = null;
                    FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].KontrolaEfektivnostiOdstranit = string.Empty;
                    this.labelEfektivita.Text = Convert.ToDateTime(datumKontrolaEfektivnosti).ToShortDateString();

                    this.ZablokovatPole();
                }
            }
        }

        private void ZablokovatPole()
        {
            this.TextBoxOdkazNaNormu.Enabled = true;
            this.TextBoxOdkazNaNormu.ReadOnly = true;
            this.TextBoxOdkazNaNormu.GotFocus += this.TextBoxOdkazNaNormuGotFocus;

            this.TextBoxHodnoceniNeshody.Enabled = true;
            this.TextBoxHodnoceniNeshody.ReadOnly = true;
            this.TextBoxHodnoceniNeshody.GotFocus += this.TextBoxHodnoceniNeshodyGotFocus;

            this.ComboBoxOdpovednaOsoba1.Enabled = false;
            this.ComboBoxOdpovednaOsoba2.Enabled = false;
            this.ComboBoxOddeleni.Enabled = false;

            this.RichTextBoxPopisProblemu.Enabled = true;
            this.RichTextBoxPopisProblemu.ReadOnly = true;
            this.RichTextBoxPopisProblemu.GotFocus += this.RichTextBoxPopisProblemuGotFocus;

            this.RichTextBoxSkutecnaPricinaWM.Enabled = true;
            this.RichTextBoxSkutecnaPricinaWM.ReadOnly = true;
            this.RichTextBoxSkutecnaPricinaWM.GotFocus += this.RichTextBoxSkutecnaPricinaWMGotFocus;

            this.RichTextBoxNapravnaOpatreniWM.Enabled = true;
            this.RichTextBoxNapravnaOpatreniWM.ReadOnly = true;
            this.RichTextBoxNapravnaOpatreniWM.GotFocus += this.RichTextBoxNapravnaOpatreniWMGotFocus;

            this.RichTextBoxSkutecnaPricinaWS.Enabled = true;
            this.RichTextBoxSkutecnaPricinaWS.ReadOnly = true;
            this.RichTextBoxSkutecnaPricinaWS.GotFocus += this.RichTextBoxSkutecnaPricinaWSGotFocus;

            this.RichTextBoxNapravnaOpatreniWS.Enabled = true;
            this.RichTextBoxNapravnaOpatreniWS.ReadOnly = true;
            this.RichTextBoxNapravnaOpatreniWS.GotFocus += this.RichTextBoxNapravnaOpatreniWSGotFocus;
        }

        private void OdblokovatPole()
        {
            this.TextBoxOdkazNaNormu.Enabled = true;
            this.TextBoxOdkazNaNormu.ReadOnly = false;
            this.TextBoxOdkazNaNormu.GotFocus -= this.TextBoxOdkazNaNormuGotFocus;

            this.TextBoxHodnoceniNeshody.Enabled = true;
            this.TextBoxHodnoceniNeshody.ReadOnly = false;
            this.TextBoxHodnoceniNeshody.GotFocus -= this.TextBoxHodnoceniNeshodyGotFocus;

            this.ComboBoxOdpovednaOsoba1.Enabled = true;
            this.ComboBoxOdpovednaOsoba2.Enabled = true;
            this.ComboBoxOddeleni.Enabled = true;

            this.RichTextBoxPopisProblemu.Enabled = true;
            this.RichTextBoxPopisProblemu.ReadOnly = false;
            this.RichTextBoxPopisProblemu.GotFocus -= this.RichTextBoxPopisProblemuGotFocus;

            this.RichTextBoxSkutecnaPricinaWM.Enabled = true;
            this.RichTextBoxSkutecnaPricinaWM.ReadOnly = false;
            this.RichTextBoxSkutecnaPricinaWM.GotFocus -= this.RichTextBoxSkutecnaPricinaWMGotFocus;

            this.RichTextBoxNapravnaOpatreniWM.Enabled = true;
            this.RichTextBoxNapravnaOpatreniWM.ReadOnly = false;
            this.RichTextBoxNapravnaOpatreniWM.GotFocus -= this.RichTextBoxNapravnaOpatreniWMGotFocus;

            this.RichTextBoxSkutecnaPricinaWS.Enabled = true;
            this.RichTextBoxSkutecnaPricinaWS.ReadOnly = false;
            this.RichTextBoxSkutecnaPricinaWS.GotFocus -= this.RichTextBoxSkutecnaPricinaWSGotFocus;

            this.RichTextBoxNapravnaOpatreniWS.Enabled = true;
            this.RichTextBoxNapravnaOpatreniWS.ReadOnly = false;
            this.RichTextBoxNapravnaOpatreniWS.GotFocus -= this.RichTextBoxNapravnaOpatreniWSGotFocus;
        }

        private void TextBoxOdkazNaNormuGotFocus(object sender, EventArgs e) => SendKeys.Send("{tab}");
        private void TextBoxHodnoceniNeshodyGotFocus(object sender, EventArgs e) => SendKeys.Send("{tab}");
        private void RichTextBoxPopisProblemuGotFocus(object sender, EventArgs e) => SendKeys.Send("{tab}");
        private void RichTextBoxSkutecnaPricinaWMGotFocus(object sender, EventArgs e) => SendKeys.Send("{tab}");
        private void RichTextBoxNapravnaOpatreniWMGotFocus(object sender, EventArgs e) => SendKeys.Send("{tab}");
        private void RichTextBoxSkutecnaPricinaWSGotFocus(object sender, EventArgs e) => SendKeys.Send("{tab}");
        private void RichTextBoxNapravnaOpatreniWSGotFocus(object sender, EventArgs e) => SendKeys.Send("{tab}");

        private void ButtonPriloha_MouseClick(object sender, MouseEventArgs e)
        {
            var attachment = this.priloha;
            var readOnly = false;

            if (this.novyBodAP == false)
            {
                attachment = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Priloha == string.Empty
                    ? string.Empty
                    : FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Priloha;
                readOnly = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].KontrolaEfektivnosti != null;
            }

            using var form = new FormPriloha(this.novyBodAP, readOnly, attachment, this.cisloRadkyDGVBody);
            var result = form.ShowDialog();

            switch (result)
            {
                case DialogResult.OK:
                {
                    this.DoOkAction(form);
                    break;
                }
                case DialogResult.Abort:
                {
                    this.DoAbortAction();
                    break;
                }
                case DialogResult.Cancel:
                case DialogResult.None:
                case DialogResult.Retry:
                case DialogResult.Ignore:
                case DialogResult.Yes:
                case DialogResult.No:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DoOkAction(FormPriloha form)
        {
            if (this.novyBodAP)
            {
                this.priloha = form.ReturnValueFolder;
            }
            else
            {
                FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Priloha = form.ReturnValueFolder;
            }

            this.changedDGV = true;
            this.ButtonUlozit.Enabled = true;
        }

        private void DoAbortAction()
        {
            if (this.novyBodAP)
            {
                this.priloha = string.Empty;
            }
            else
            {
                FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Priloha = string.Empty;
            }

            this.changedDGV = true;
            this.ButtonUlozit.Enabled = true;
        }

        private void DataGridViewWMAkce_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1)
            {
                return;
            }

            this.ButtonUlozit.Enabled = true;
            this.changedDGV = true;
        }

        private void DataGridViewWSAkce_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1)
            {
                return;
            }

            this.ButtonUlozit.Enabled = true;
            this.changedDGV = true;
        }

        private void DataGridViewWMAkce_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (this.DataGridViewWMAkce.IsCurrentCellDirty)
            {
                this.DataGridViewWMAkce.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DataGridViewWSAkce_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (this.DataGridViewWSAkce.IsCurrentCellDirty)
            {
                this.DataGridViewWSAkce.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void TextBoxOdkazNaNormu_TextChanged(object sender, EventArgs e)
        {
            this.ButtonUlozit.Enabled = true;
            this.changedDGV = true;
        }

        private void TextBoxHodnoceniNeshody_TextChanged(object sender, EventArgs e)
        {
            this.ButtonUlozit.Enabled = true;
            this.changedDGV = true;
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e) => this.Close();

        private void FormZadaniBoduAP_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.changedDGV)
            {
                return;
            }

            DialogResult dialogResult;

            dialogResult = MessageBox.Show("You want to save your changes.", "Notice", MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Information);

            switch (dialogResult)
            {
                case DialogResult.Yes:
                    this.UlozitBodAP();
                    this.changedDGV = false;
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                case DialogResult.No:
                case DialogResult.None:
                case DialogResult.OK:
                case DialogResult.Abort:
                case DialogResult.Retry:
                case DialogResult.Ignore:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
