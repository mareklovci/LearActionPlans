using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LearActionPlans.Models;
using LearActionPlans.DataMappers;
using LearActionPlans.ViewModels;

namespace LearActionPlans.Views
{
    public partial class FormPrehledBoduAP : Form
    {
        //při volání konstruktoru vytvořím novou prom jako readonly, protože ji nastavím při volání konstruktoru a pak už její hodnoty neměním
        private readonly FormNovyAkcniPlan.AkcniPlanTmp akcniPlany_;
        //tady udržuji všechny informace o bodech AP
        public static List<BodAP> bodyAP;
        //do této proměnné uložím pouze hodnoty, které se zobrazí v DGV
        private DataTable dtBodyAP;

        private readonly BindingSource bindingSource;
        //public static bool UlozitBodyAP { get; set; }

        private readonly byte volani_;
        //1 - založení nového AP
        //2 - aktualizace již vytvořeného AP
        //3 - bude volání z formuláře FormVsechnyBodyAP

        //private readonly bool vlastnikAP_;
        //private readonly bool vlastnikAkce_;
        //private readonly int vlastnikAkceId_;

        private readonly bool spusteniBezParametru_;

        public FormPrehledBoduAP(bool spusteniBezParametru, FormNovyAkcniPlan.AkcniPlanTmp akcniPlany, byte volani)
        {
            InitializeComponent();
            bindingSource = new BindingSource();
            bodyAP = new List<BodAP>();
            dtBodyAP = new DataTable();

            spusteniBezParametru_ = spusteniBezParametru;

            //vlastnikAP_ = vlastnikAP;
            //vlastnikAkce_ = vlastnikAkce;
            //vlastnikAkceId_ = vlastnikAkceId;

            akcniPlany_ = akcniPlany;
            //UlozitBodyAP = false;
            volani_ = volani;
        }

        private void FormNovyBodAP_Load(object sender, EventArgs e)
        {
            bindingSource.DataSource = dtBodyAP;
            DataGridViewBodyAP.DataSource = bindingSource;
            
            if (akcniPlany_.CisloAPRok == null)
                labelCisloAP.Text = "";
            else
                labelCisloAP.Text = akcniPlany_.CisloAPRok;

            if (akcniPlany_.Zadavatel1Jmeno == null)
                labelZadavatel1Zadano.Text = "";
            else
                labelZadavatel1Zadano.Text = akcniPlany_.Zadavatel1Jmeno;

            if (akcniPlany_.Zadavatel2Jmeno == null)
                labelZadavatel2Zadano.Text = akcniPlany_.Zadavatel2Jmeno = "";
            else
                labelZadavatel2Zadano.Text = akcniPlany_.Zadavatel2Jmeno;

            if (akcniPlany_.Tema == null)
                labelTemaAP.Text = "";
            else
                labelTemaAP.Text = akcniPlany_.Tema;

            if (akcniPlany_.ProjektNazev == null)
                labelProjektAP.Text = "";
            else
                labelProjektAP.Text = akcniPlany_.ProjektNazev;

            //labelDatumUkonceniAP.Text = akcniPlany_.DatumUkonceni.ToShortDateString();
            labelDatumUkonceniAP.Text = string.Empty;

            if (akcniPlany_.ZakaznikNazev == null)
                labelZakaznikAP.Text = "";
            else
                labelZakaznikAP.Text = akcniPlany_.ZakaznikNazev;

            var ukonceniAP = PrehledBoduAPViewModel.GetUkonceniAPId(akcniPlany_.Id);
            foreach (var u in ukonceniAP)
            {
                labelDatumUkonceniAP.Text = u.DatumUkonceniAP.ToShortDateString();
                break;
            }
            
            CreateColumns();

            //ButtonUlozit.Enabled = false;

            //tento if je pro aktualizaci bodů AP
            if (volani_ == 2)
            { 
                //tady se naplní prom bodyAP z databáze
                var bodyAP_ = PrehledBoduAPViewModel.GetBodyIdAPAll(akcniPlany_.Id).ToList();
                //int bodId = 0;
                int i = 0;
                foreach (var b in bodyAP_)
                {
                    bodyAP.Add(new BodAP(b.Id, b.IdAP, b.CisloBoduAP, b.DatumZalozeni, b.OdkazNaNormu, b.HodnoceniNeshody, b.PopisProblemu, 
                        b.SkutecnaPricinaWM, b.NapravnaOpatreniWM, b.SkutecnaPricinaWS, b.NapravnaOpatreniWS, 
                        b.OdpovednaOsoba1Id, b.OdpovednaOsoba2Id, b.OdpovednaOsoba1, b.KontrolaEfektivnosti, b.OddeleniId, b.Oddeleni, b.Priloha, 
                        b.ZamitnutiTerminu, b.ZmenaTerminu, b.ZnovuOtevrit, true, b.StavObjektuBodAP) );
                    //tabulka pro DGV
                    dtBodyAP.Rows.Add(new object[] { b.CisloBoduAP, b.OdkazNaNormu, b.HodnoceniNeshody, b.PopisProblemu,
                        b.OdpovednaOsoba1, string.Empty, b.Oddeleni,
                        b.SkutecnaPricinaWM == null ? string.Empty : b.SkutecnaPricinaWM, b.NapravnaOpatreniWM == null ? string.Empty : b.NapravnaOpatreniWM, 
                        b.SkutecnaPricinaWS == null ? string.Empty : b.SkutecnaPricinaWS, b.NapravnaOpatreniWS == null ? string.Empty : b.NapravnaOpatreniWS,
                        string.Empty, b.KontrolaEfektivnosti == null ? string.Empty : Convert.ToDateTime(b.KontrolaEfektivnosti).ToShortDateString(), b.ZnovuOtevrit });

                    var ukonceniBodAP = PrehledBoduAPViewModel.GetUkonceniBodAP(b.Id).ToList();

                    if (ukonceniBodAP.Count == 0)
                    { }
                    else
                    {
                        bodyAP[i].UkonceniBodAP = new List<UkonceniBodAP>();
                        foreach (var u in ukonceniBodAP)
                        {
                            bodyAP[i].UkonceniBodAP.Add(new UkonceniBodAP(u.Id, u.BodAPId, u.DatumUkonceni, u.Poznamka, u.Odpoved, u.StavZadosti, u.StavObjektuBodAP, true));

                            //bodyAP[i].TypAkce.Add(new Akce(a.Id, a.BodAPId, a.NapravnaOpatreni,
                            //    a.OdpovednaOsoba1Id, a.OdpovednaOsoba2Id, new List<UkonceniBodAP>(), a.KontrolaEfektivnosti, a.OddeleniId,
                            //    a.Priloha, a.Typ, a.StavObjektuAkce, true, a.Reopen));

                            //var ukonceniAkce = PrehledBoduAPViewModel.GetUkonceniAkceId(a.Id).ToList();

                            //if (ukonceniAkce.Count == 0)
                            //{ }
                            //else
                            //{
                            //    foreach (var u in ukonceniAkce)
                            //    {
                            //        bodyAP[i].TypAkce[j].UkonceniAkce.Add(new UkonceniBodAP(u.Id, u.AkceId, u.DatumUkonceni, u.Poznamka, u.Odpoved, u.StavZadosti, u.StavObjektuAkce, true));
                            //    }
                            //}
                            //j++;
                        }
                    }
                    i++;
                }
            }

            if (DataGridViewBodyAP.Rows.Count < 1)
                ButtonOpravitBodAP.Enabled = false;

            if (akcniPlany_.APUzavren == true)
            {
                ButtonNovyBodAP.Visible = false;
                ButtonZavrit.Text = "Close";
                ButtonOpravitBodAP.Text = "Display AP point";
            }
            else
            {
                ButtonNovyBodAP.Visible = true;
                ButtonZavrit.Text = "Close";
                ButtonOpravitBodAP.Text = "Edit AP point";
                //if (FormMain.VlastnikAP == true)
                //{
                //    ButtonNovyBodAP.Visible = true;
                //    ButtonZavrit.Text = "Close";
                //    ButtonOpravitBodAP.Text = "Edit AP point";
                //}
                //if (FormMain.VlastnikAP == false)
                //{
                //    ButtonNovyBodAP.Visible = false;
                //    ButtonZavrit.Text = "Close";
                //    ButtonOpravitBodAP.Text = "Display AP point";
                //}
            }

            //tato část bude spuštěna z emailu
            if (spusteniBezParametru_ == false)
            {
                using (var form = new FormZadaniBoduAP(labelCisloAP.Text, akcniPlany_, DataGridViewBodyAP.CurrentCell.RowIndex, false))
                {
                    form.ShowDialog();
                    //if (UlozitBodyAP == true)
                    //    ButtonUlozit.Enabled = true;
                    NacistDGV();
                }
            }
        }

        private void CreateColumns()
        {
            dtBodyAP.Columns.Add(new DataColumn("cisloBodAP", typeof(int)));
            dtBodyAP.Columns.Add(new DataColumn("textBoxOdkazNaNormu", typeof(string)));
            dtBodyAP.Columns.Add(new DataColumn("textBoxHodnoceniNeshody", typeof(string)));
            dtBodyAP.Columns.Add(new DataColumn("textBoxPopisProblemu", typeof(string)));

            dtBodyAP.Columns.Add(new DataColumn("OdpovednyPracovnik1", typeof(string)));
            dtBodyAP.Columns.Add(new DataColumn("OdpovednyPracovnik2", typeof(string)));

            dtBodyAP.Columns.Add(new DataColumn("Oddeleni", typeof(string)));

            dtBodyAP.Columns.Add(new DataColumn("WMPricina", typeof(string)));
            dtBodyAP.Columns.Add(new DataColumn("WMOpatreni", typeof(string)));
            dtBodyAP.Columns.Add(new DataColumn("WSPricina", typeof(string)));
            dtBodyAP.Columns.Add(new DataColumn("WSOpatreni", typeof(string)));

            dtBodyAP.Columns.Add(new DataColumn("DatumUkonceni", typeof(string)));
            dtBodyAP.Columns.Add(new DataColumn("Efektivita", typeof(string)));

            DataGridViewBodyAP.Columns["cisloBodAP"].HeaderText = @"AP point number";
            DataGridViewBodyAP.Columns["cisloBodAP"].Width = 120;

            DataGridViewBodyAP.Columns["textBoxOdkazNaNormu"].HeaderText = @"Standard chapter";
            DataGridViewBodyAP.Columns["textBoxOdkazNaNormu"].Width = 140;

            DataGridViewBodyAP.Columns["textBoxHodnoceniNeshody"].HeaderText = @"Evaluation";
            DataGridViewBodyAP.Columns["textBoxHodnoceniNeshody"].Width = 120;

            DataGridViewBodyAP.Columns["textBoxPopisProblemu"].HeaderText = @"Description of the problem";
            DataGridViewBodyAP.Columns["textBoxPopisProblemu"].Width = 200;

            DataGridViewBodyAP.Columns["OdpovednyPracovnik1"].HeaderText = @"Responsible #1";
            DataGridViewBodyAP.Columns["OdpovednyPracovnik1"].Width = 200;

            DataGridViewBodyAP.Columns["OdpovednyPracovnik2"].HeaderText = @"Responsible #2";
            DataGridViewBodyAP.Columns["OdpovednyPracovnik2"].Width = 200;

            DataGridViewBodyAP.Columns["Oddeleni"].HeaderText = @"Department";
            DataGridViewBodyAP.Columns["Oddeleni"].Width = 150;

            DataGridViewBodyAP.Columns["WMPricina"].HeaderText = @"WM Root cause";
            DataGridViewBodyAP.Columns["WMPricina"].Width = 200;

            DataGridViewBodyAP.Columns["WMOpatreni"].HeaderText = @"WM Corrective action";
            DataGridViewBodyAP.Columns["WMOpatreni"].Width = 200;

            DataGridViewBodyAP.Columns["WSPricina"].HeaderText = @"WS Root cause";
            DataGridViewBodyAP.Columns["WSPricina"].Width = 200;

            DataGridViewBodyAP.Columns["WSOpatreni"].HeaderText = @"WS Corrective action";
            DataGridViewBodyAP.Columns["WSOpatreni"].Width = 200;

            DataGridViewBodyAP.Columns["DatumUkonceni"].HeaderText = @"Deadline";
            DataGridViewBodyAP.Columns["DatumUkonceni"].Width = 100;

            DataGridViewBodyAP.Columns["Efektivita"].HeaderText = @"Effectiveness";
            DataGridViewBodyAP.Columns["Efektivita"].Width = 100;

            dtBodyAP.Columns.Add(new DataColumn("reopen", typeof(bool)));
            DataGridViewBodyAP.Columns["reopen"].HeaderText = @"After reopen";
            DataGridViewBodyAP.Columns["reopen"].Width = 110;

            //DataGridViewCellStyle style = DataGridViewBodyAP.ColumnHeadersDefaultCellStyle;
            //style.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0F, GraphicsUnit.Pixel);

            //foreach (DataGridViewColumn c in DataGridViewBodyAP.Columns)
            //    c.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0F, GraphicsUnit.Pixel);

            DataGridViewBodyAP.MultiSelect = false;
            DataGridViewBodyAP.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            DataGridViewBodyAP.AllowUserToResizeRows = false;
            DataGridViewBodyAP.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            DataGridViewBodyAP.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            DataGridViewBodyAP.AllowUserToResizeColumns = false;
            DataGridViewBodyAP.AllowUserToAddRows = false;
            DataGridViewBodyAP.ReadOnly = true;
            DataGridViewBodyAP.EditMode = DataGridViewEditMode.EditOnEnter;
            DataGridViewBodyAP.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            foreach (DataGridViewColumn column in DataGridViewBodyAP.Columns)
            {
                //column.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0F, GraphicsUnit.Pixel);
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //DataGridViewBodyAP.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            //DataGridViewBodyAP.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
        }

        private void ButtonNovyBodAP_MouseClick(object sender, MouseEventArgs e)
        {
            int cisloRadkyDGV = -1;

            //if (DataGridViewBodyAP.Rows.Count == 0)
            //    cisloRadkyDGV = 0;
            //else
            //    cisloRadkyDGV = DataGridViewBodyAP.Rows.Count - 1;

            using (var form = new FormZadaniBoduAP(labelCisloAP.Text, akcniPlany_, cisloRadkyDGV, true))
            {
                form.ShowDialog();
                //if (UlozitBodyAP == true)
                //    ButtonUlozit.Enabled = true;
                NacistDGV();
            }
        }

        private void ButtonOpravitBodAP_MouseClick(object sender, MouseEventArgs e)
        {
            if (DataGridViewBodyAP.CurrentCell.RowIndex >= 0)
            {
                using (var form = new FormZadaniBoduAP(labelCisloAP.Text, akcniPlany_, DataGridViewBodyAP.CurrentCell.RowIndex, false))
                {
                    form.ShowDialog();
                    //if (UlozitBodyAP == true)
                    //    ButtonUlozit.Enabled = true;
                    NacistDGV();
                }
            }
        }

        private void NacistDGV()
        {

            //nejdřív odstraním všechny řádky z dtBody
            if (dtBodyAP.Rows.Count > 0)
            {
                for (int i = dtBodyAP.Rows.Count - 1; i >= 0; i--)
                    dtBodyAP.Rows.Remove(dtBodyAP.Rows[i]);
                dtBodyAP.Rows.Clear();
                bodyAP = new List<BodAP>();
            }

            var bodyAP_ = PrehledBoduAPViewModel.GetBodyIdAPAll(akcniPlany_.Id).ToList();
            foreach (var b in bodyAP_)
            {
                bodyAP.Add(new BodAP(b.Id, b.IdAP, b.CisloBoduAP, b.DatumZalozeni, b.OdkazNaNormu, b.HodnoceniNeshody, b.PopisProblemu,
                    b.SkutecnaPricinaWM, b.NapravnaOpatreniWM, b.SkutecnaPricinaWS, b.NapravnaOpatreniWS,
                    b.OdpovednaOsoba1Id, b.OdpovednaOsoba2Id, b.OdpovednaOsoba1, b.KontrolaEfektivnosti, b.OddeleniId, b.Oddeleni, b.Priloha,
                    b.ZamitnutiTerminu, b.ZmenaTerminu, b.ZnovuOtevrit, true, b.StavObjektuBodAP));
            }

            //pak naplním tabulku znovu
            foreach (BodAP b in bodyAP)
            {
                dtBodyAP.Rows.Add(new object[] { b.CisloBoduAP, b.OdkazNaNormu, b.HodnoceniNeshody, b.PopisProblemu,
                    b.OdpovednaOsoba1, string.Empty, b.Oddeleni,
                        b.SkutecnaPricinaWM == null ? string.Empty : b.SkutecnaPricinaWM, b.NapravnaOpatreniWM == null ? string.Empty : b.NapravnaOpatreniWM,
                        b.SkutecnaPricinaWS == null ? string.Empty : b.SkutecnaPricinaWS, b.NapravnaOpatreniWS == null ? string.Empty : b.NapravnaOpatreniWS,
                        string.Empty, b.KontrolaEfektivnosti == null ? string.Empty : Convert.ToDateTime(b.KontrolaEfektivnosti).ToShortDateString(), b.ZnovuOtevrit });
            }

            if (DataGridViewBodyAP.Rows.Count < 1)
                ButtonOpravitBodAP.Enabled = false;
            else
                ButtonOpravitBodAP.Enabled = true;
        }

        //private void ButtonUlozit_MouseClick(object sender, MouseEventArgs e)
        //{
        //    //UlozitNoveBodyAP();
        //    ButtonUlozit.Enabled = false;
        //    //UlozitBodyAP = false;
        //}

        //private void UlozitNoveBodyAP()
        //{
        //    //Insert nebo update
        //    //BodyAPDataMapper.InsertUpdateBodyAP(akcniPlany_.Id);
        //}

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void FormPrehledBoduAP_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (UlozitBodyAP == true)
            //{
            //    DialogResult dialogResult = MessageBox.Show("You want to save your changes.", "Notice", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

            //    if (dialogResult == DialogResult.Yes)
            //    {
            //        //zapíše bod AP do třídy
            //        UlozitNoveBodyAP();
            //    }
            //    else if (dialogResult == DialogResult.No)
            //    {
            //    }
            //    else if (dialogResult == DialogResult.Cancel)
            //    {
            //        e.Cancel = true;
            //    }
            //}
        }
    }
}