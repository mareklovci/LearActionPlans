using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LearActionPlans.Models;
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

        private readonly bool spusteniBezParametru_;

        public FormPrehledBoduAP(bool spusteniBezParametru, FormNovyAkcniPlan.AkcniPlanTmp akcniPlany, byte volani)
        {
            this.InitializeComponent();
            this.bindingSource = new BindingSource();
            bodyAP = new List<BodAP>();
            this.dtBodyAP = new DataTable();

            this.spusteniBezParametru_ = spusteniBezParametru;
            this.akcniPlany_ = akcniPlany;
            this.volani_ = volani;
        }

        private void FormNovyBodAP_Load(object sender, EventArgs e)
        {
            this.bindingSource.DataSource = this.dtBodyAP;
            this.DataGridViewBodyAP.DataSource = this.bindingSource;
            this.labelCisloAP.Text = this.akcniPlany_.CisloAPRok ?? "";
            this.labelZadavatel1Zadano.Text = this.akcniPlany_.Zadavatel1Jmeno ?? "";
            this.labelZadavatel2Zadano.Text =
                this.akcniPlany_.Zadavatel2Jmeno ?? (this.akcniPlany_.Zadavatel2Jmeno = "");
            this.labelTemaAP.Text = this.akcniPlany_.Tema ?? "";
            this.labelProjektAP.Text = this.akcniPlany_.ProjektNazev ?? "";
            this.labelDatumUkonceniAP.Text = string.Empty;
            this.labelZakaznikAP.Text = this.akcniPlany_.ZakaznikNazev ?? "";

            var ukonceniAP = PrehledBoduAPViewModel.GetUkonceniAPId(this.akcniPlany_.Id);
            foreach (var u in ukonceniAP)
            {
                this.labelDatumUkonceniAP.Text = u.DatumUkonceniAP.ToShortDateString();
                break;
            }

            this.CreateColumns();

            //tento if je pro aktualizaci bodů AP
            if (this.volani_ == 2)
            {
                //tady se naplní prom bodyAP z databáze
                var bodyAP_ = PrehledBoduAPViewModel.GetBodyIdAPAll(this.akcniPlany_.Id).ToList();
                //int bodId = 0;
                var i = 0;
                foreach (var b in bodyAP_)
                {
                    bodyAP.Add(new BodAP(b.Id, b.IdAP, b.CisloBoduAP, b.DatumZalozeni, b.OdkazNaNormu,
                        b.HodnoceniNeshody, b.PopisProblemu,
                        b.SkutecnaPricinaWM, b.NapravnaOpatreniWM, b.SkutecnaPricinaWS, b.NapravnaOpatreniWS,
                        b.OdpovednaOsoba1Id, b.OdpovednaOsoba2Id, b.OdpovednaOsoba1, b.KontrolaEfektivnosti,
                        b.OddeleniId, b.Oddeleni, b.Priloha,
                        b.ZamitnutiTerminu, b.ZmenaTerminu, b.ZnovuOtevrit, true, b.StavObjektuBodAP));
                    //tabulka pro DGV
                    this.dtBodyAP.Rows.Add(b.CisloBoduAP, b.OdkazNaNormu, b.HodnoceniNeshody, b.PopisProblemu,
                        b.OdpovednaOsoba1, string.Empty, b.Oddeleni,
                        b.SkutecnaPricinaWM ?? string.Empty,
                        b.NapravnaOpatreniWM ?? string.Empty,
                        b.SkutecnaPricinaWS ?? string.Empty,
                        b.NapravnaOpatreniWS ?? string.Empty, string.Empty,
                        b.KontrolaEfektivnosti == null
                            ? string.Empty
                            : Convert.ToDateTime(b.KontrolaEfektivnosti).ToShortDateString(), b.ZnovuOtevrit);

                    var ukonceniBodAP = PrehledBoduAPViewModel.GetUkonceniBodAP(b.Id).ToList();

                    if (ukonceniBodAP.Count != 0)
                    {
                        bodyAP[i].UkonceniBodAP = new List<UkonceniBodAP>();
                        foreach (var u in ukonceniBodAP)
                        {
                            bodyAP[i].UkonceniBodAP.Add(new UkonceniBodAP(u.Id, u.BodAPId, u.DatumUkonceni, u.Poznamka,
                                u.Odpoved, u.StavZadosti, u.StavObjektuBodAP, true));
                        }
                    }

                    i++;
                }
            }

            if (this.DataGridViewBodyAP.Rows.Count < 1)
            {
                this.ButtonOpravitBodAP.Enabled = false;
            }

            if (this.akcniPlany_.APUzavren == true)
            {
                this.ButtonNovyBodAP.Visible = false;
                this.ButtonZavrit.Text = "Close";
                this.ButtonOpravitBodAP.Text = "Display AP point";
            }
            else
            {
                this.ButtonNovyBodAP.Visible = true;
                this.ButtonZavrit.Text = "Close";
                this.ButtonOpravitBodAP.Text = "Edit AP point";
            }

            //tato část bude spuštěna z emailu
            if (this.spusteniBezParametru_)
            {
                return;
            }

            using var form = new FormZadaniBoduAP(this.labelCisloAP.Text, this.akcniPlany_,
                this.DataGridViewBodyAP.CurrentCell.RowIndex, false);
            form.ShowDialog();
            this.NacistDGV();
        }

        private void CreateColumns()
        {
            this.dtBodyAP.Columns.Add(new DataColumn("cisloBodAP", typeof(int)));
            this.dtBodyAP.Columns.Add(new DataColumn("textBoxOdkazNaNormu", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("textBoxHodnoceniNeshody", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("textBoxPopisProblemu", typeof(string)));

            this.dtBodyAP.Columns.Add(new DataColumn("OdpovednyPracovnik1", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("OdpovednyPracovnik2", typeof(string)));

            this.dtBodyAP.Columns.Add(new DataColumn("Oddeleni", typeof(string)));

            this.dtBodyAP.Columns.Add(new DataColumn("WMPricina", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("WMOpatreni", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("WSPricina", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("WSOpatreni", typeof(string)));

            this.dtBodyAP.Columns.Add(new DataColumn("DatumUkonceni", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("Efektivita", typeof(string)));

            this.DataGridViewBodyAP.Columns["cisloBodAP"].HeaderText = @"AP point number";
            this.DataGridViewBodyAP.Columns["cisloBodAP"].Width = 120;

            this.DataGridViewBodyAP.Columns["textBoxOdkazNaNormu"].HeaderText = @"Standard chapter";
            this.DataGridViewBodyAP.Columns["textBoxOdkazNaNormu"].Width = 140;

            this.DataGridViewBodyAP.Columns["textBoxHodnoceniNeshody"].HeaderText = @"Evaluation";
            this.DataGridViewBodyAP.Columns["textBoxHodnoceniNeshody"].Width = 120;

            this.DataGridViewBodyAP.Columns["textBoxPopisProblemu"].HeaderText = @"Description of the problem";
            this.DataGridViewBodyAP.Columns["textBoxPopisProblemu"].Width = 200;

            this.DataGridViewBodyAP.Columns["OdpovednyPracovnik1"].HeaderText = @"Responsible #1";
            this.DataGridViewBodyAP.Columns["OdpovednyPracovnik1"].Width = 200;

            this.DataGridViewBodyAP.Columns["OdpovednyPracovnik2"].HeaderText = @"Responsible #2";
            this.DataGridViewBodyAP.Columns["OdpovednyPracovnik2"].Width = 200;

            this.DataGridViewBodyAP.Columns["Oddeleni"].HeaderText = @"Department";
            this.DataGridViewBodyAP.Columns["Oddeleni"].Width = 150;

            this.DataGridViewBodyAP.Columns["WMPricina"].HeaderText = @"WM Root cause";
            this.DataGridViewBodyAP.Columns["WMPricina"].Width = 200;

            this.DataGridViewBodyAP.Columns["WMOpatreni"].HeaderText = @"WM Corrective action";
            this.DataGridViewBodyAP.Columns["WMOpatreni"].Width = 200;

            this.DataGridViewBodyAP.Columns["WSPricina"].HeaderText = @"WS Root cause";
            this.DataGridViewBodyAP.Columns["WSPricina"].Width = 200;

            this.DataGridViewBodyAP.Columns["WSOpatreni"].HeaderText = @"WS Corrective action";
            this.DataGridViewBodyAP.Columns["WSOpatreni"].Width = 200;

            this.DataGridViewBodyAP.Columns["DatumUkonceni"].HeaderText = @"Deadline";
            this.DataGridViewBodyAP.Columns["DatumUkonceni"].Width = 100;

            this.DataGridViewBodyAP.Columns["Efektivita"].HeaderText = @"Effectiveness";
            this.DataGridViewBodyAP.Columns["Efektivita"].Width = 100;

            this.dtBodyAP.Columns.Add(new DataColumn("reopen", typeof(bool)));
            this.DataGridViewBodyAP.Columns["reopen"].HeaderText = @"After reopen";
            this.DataGridViewBodyAP.Columns["reopen"].Width = 110;

            this.DataGridViewBodyAP.MultiSelect = false;
            this.DataGridViewBodyAP.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            this.DataGridViewBodyAP.AllowUserToResizeRows = false;
            this.DataGridViewBodyAP.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DataGridViewBodyAP.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            this.DataGridViewBodyAP.AllowUserToResizeColumns = false;
            this.DataGridViewBodyAP.AllowUserToAddRows = false;
            this.DataGridViewBodyAP.ReadOnly = true;
            this.DataGridViewBodyAP.EditMode = DataGridViewEditMode.EditOnEnter;
            this.DataGridViewBodyAP.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            foreach (DataGridViewColumn column in this.DataGridViewBodyAP.Columns)
            {
                //column.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0F, GraphicsUnit.Pixel);
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void ButtonNovyBodAP_MouseClick(object sender, MouseEventArgs e)
        {
            var cisloRadkyDGV = -1;

            using var form = new FormZadaniBoduAP(this.labelCisloAP.Text, this.akcniPlany_, cisloRadkyDGV, true);
            form.ShowDialog();
            this.NacistDGV();
        }

        private void ButtonOpravitBodAP_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.DataGridViewBodyAP.CurrentCell.RowIndex < 0)
            {
                return;
            }

            using var form = new FormZadaniBoduAP(this.labelCisloAP.Text, this.akcniPlany_,
                this.DataGridViewBodyAP.CurrentCell.RowIndex, false);
            form.ShowDialog();
            this.NacistDGV();
        }

        private void NacistDGV()
        {
            //nejdřív odstraním všechny řádky z dtBody
            // poznámka
            if (this.dtBodyAP.Rows.Count > 0)
            {
                for (var i = this.dtBodyAP.Rows.Count - 1; i >= 0; i--)
                {
                    this.dtBodyAP.Rows.Remove(this.dtBodyAP.Rows[i]);
                }

                this.dtBodyAP.Rows.Clear();
                bodyAP = new List<BodAP>();
            }

            var bodyAP_ = PrehledBoduAPViewModel.GetBodyIdAPAll(this.akcniPlany_.Id).ToList();
            foreach (var b in bodyAP_)
            {
                bodyAP.Add(new BodAP(b.Id, b.IdAP, b.CisloBoduAP, b.DatumZalozeni, b.OdkazNaNormu, b.HodnoceniNeshody,
                    b.PopisProblemu,
                    b.SkutecnaPricinaWM, b.NapravnaOpatreniWM, b.SkutecnaPricinaWS, b.NapravnaOpatreniWS,
                    b.OdpovednaOsoba1Id, b.OdpovednaOsoba2Id, b.OdpovednaOsoba1, b.KontrolaEfektivnosti, b.OddeleniId,
                    b.Oddeleni, b.Priloha,
                    b.ZamitnutiTerminu, b.ZmenaTerminu, b.ZnovuOtevrit, true, b.StavObjektuBodAP));
            }

            //pak naplním tabulku znovu
            foreach (var b in bodyAP)
            {
                this.dtBodyAP.Rows.Add(b.CisloBoduAP, b.OdkazNaNormu, b.HodnoceniNeshody, b.PopisProblemu,
                    b.OdpovednaOsoba1, string.Empty, b.Oddeleni, b.SkutecnaPricinaWM ?? string.Empty,
                    b.NapravnaOpatreniWM ?? string.Empty, b.SkutecnaPricinaWS ?? string.Empty,
                    b.NapravnaOpatreniWS ?? string.Empty, string.Empty, b.KontrolaEfektivnosti == null
                        ? string.Empty
                        : Convert.ToDateTime(b.KontrolaEfektivnosti).ToShortDateString(), b.ZnovuOtevrit);
            }

            this.ButtonOpravitBodAP.Enabled = this.DataGridViewBodyAP.Rows.Count >= 1;
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e) => this.Close();

        private void FormPrehledBoduAP_FormClosing(object sender, FormClosingEventArgs e) { }
    }
}
