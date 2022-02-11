using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using LearActionPlans.ViewModels;
using LearActionPlans.Utilities;
using Microsoft.Extensions.Options;

namespace LearActionPlans.Views
{
    public partial class FormPrehledAp : Form
    {
        private readonly ArgumentOptions arguments;
        private readonly FormPrehledBoduAP formPrehledBoduAp;
        private readonly FormEditAP formEditAp;

        private readonly BindingSource bindingSourceAp;
        private DataTable dtAp;
        private DataView dvAp;

        FormNovyAkcniPlan.AkcniPlanTmp akcniPlany;

        private string odpovedny1Filtr;
        private string odpovedny2Filtr;
        private string projektyFiltr;
        private string typApFiltr;
        private string rokZalozeniFiltr;
        private string otevreneUzavreneFiltr;

        private string cisloApStr;
        private readonly int apId;

        public FormPrehledAp(IOptionsMonitor<ArgumentOptions> optionsMonitor,
            FormPrehledBoduAP formPrehledBoduAp,
            FormEditAP formEditAp)
        {
            this.formPrehledBoduAp = formPrehledBoduAp;
            this.formEditAp = formEditAp;
            this.arguments = optionsMonitor.CurrentValue;

            this.InitializeComponent();

            this.cisloApStr = this.arguments.ActionPlanNumber;
            this.apId = this.arguments.ActionPlanId;

            this.bindingSourceAp = new BindingSource();
            this.dtAp = new DataTable();

            this.akcniPlany = new FormNovyAkcniPlan.AkcniPlanTmp();
        }

        private void FormPrehledAP_Load(object sender, EventArgs e)
        {
            this.bindingSourceAp.DataSource = this.dtAp;
            this.DataGridViewAP.DataSource = this.bindingSourceAp;

            //urychlení načítání dat v DGV
            if (!SystemInformation.TerminalServerSession)
            {
                var dgvType = this.DataGridViewAP.GetType();
                var pi = dgvType.GetProperty("DoubleBuffered",
                    BindingFlags.Instance | BindingFlags.NonPublic);

                if (pi != null)
                {
                    pi.SetValue(this.DataGridViewAP, true, null);
                }
            }

            this.CreateColumnsAp();
            if (this.ZobrazitDgv())
            {
                //nastaví filtry na string.empty
                this.InitFiltr();

                //naplní comboBoxy
                this.InitFiltryComboBox();

                //v comboBoxech nastaví vybrané položky
                this.NastavitVybranouPolozku();

                this.PridatHandlery();
                this.ObarvitLabel();

                this.ComboBoxRoky.Enabled = true;
                this.ComboBoxProjekty.Enabled = true;
                this.ComboBoxOdpovedny1.Enabled = true;
                this.ComboBoxOdpovedny2.Enabled = true;
                this.ComboBoxTypAP.Enabled = true;
                this.ComboBoxOtevreneUzavrene.Enabled = true;
                this.ButtonEditAP.Enabled = true;
            }
            else
            {
                this.ComboBoxRoky.Enabled = false;
                this.ComboBoxProjekty.Enabled = false;
                this.ComboBoxOdpovedny1.Enabled = false;
                this.ComboBoxOdpovedny2.Enabled = false;
                this.ComboBoxTypAP.Enabled = false;
                this.ComboBoxOtevreneUzavrene.Enabled = false;
                this.ButtonEditAP.Enabled = false;
            }

            //tato část bude spuštěna z emailu
            if (this.arguments.RunWithoutParameters)
            {
                return;
            }

            var idAP = Convert.ToInt32(this.apId);
            this.akcniPlany.Id = idAP;
            var cisloAPRok = this.cisloApStr;
            this.akcniPlany.CisloAPRok = cisloAPRok;

            using var form = this.formPrehledBoduAp;
            form.CreateFormPrehledBoduAP(false, this.akcniPlany, 2);
            var result = form.ShowDialog();
            if (result == DialogResult.OK) { }
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private void CreateColumnsAp()
        {
            this.dtAp.Columns.Add(new DataColumn("APId", typeof(int)));
            this.dtAp.Columns.Add(new DataColumn("CisloAP", typeof(int)));
            this.dtAp.Columns.Add(new DataColumn("CisloAPRok", typeof(string)));

            var colBtn = new DataGridViewButtonColumn
            {
                Name = "ButtonCisloAPRok",
                HeaderText = @"AP number",
                Width = 100,
                DataPropertyName = "CisloAPRok",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            this.DataGridViewAP.Columns.Add(colBtn);

            this.dtAp.Columns.Add(new DataColumn("DatumZalozeni", typeof(DateTime)));
            this.dtAp.Columns.Add(new DataColumn("DatumZalozeniRok", typeof(int)));
            this.dtAp.Columns.Add(new DataColumn("Zadavatel1Id", typeof(int)));
            this.dtAp.Columns.Add(new DataColumn("Zadavatel2Id", typeof(int)));
            this.dtAp.Columns.Add(new DataColumn("Zadavatel1Jmeno", typeof(string)));
            this.dtAp.Columns.Add(new DataColumn("Zadavatel2Jmeno", typeof(string)));
            this.dtAp.Columns.Add(new DataColumn("Tema", typeof(string)));
            this.dtAp.Columns.Add(new DataColumn("ProjektId", typeof(int)));
            this.dtAp.Columns.Add(new DataColumn("ProjektNazev", typeof(string)));
            this.dtAp.Columns.Add(new DataColumn("ZakaznikId", typeof(int)));
            this.dtAp.Columns.Add(new DataColumn("ZakaznikNazev", typeof(string)));
            this.dtAp.Columns.Add(new DataColumn("TypAP", typeof(byte)));
            this.dtAp.Columns.Add(new DataColumn("TypAPNazev", typeof(string)));
            // asi možná nebudu potřebovat
            this.dtAp.Columns.Add(new DataColumn("StavObjektu", typeof(byte)));

            this.DataGridViewAP.Columns["APId"].Visible = false;
            this.DataGridViewAP.Columns["CisloAP"].Visible = false;
            this.DataGridViewAP.Columns["CisloAPRok"].Visible = false;
            this.DataGridViewAP.Columns["DatumZalozeniRok"].Visible = false;
            this.DataGridViewAP.Columns["Zadavatel1Id"].Visible = false;
            this.DataGridViewAP.Columns["Zadavatel2Id"].Visible = false;
            this.DataGridViewAP.Columns["ZakaznikId"].Visible = false;
            this.DataGridViewAP.Columns["ProjektId"].Visible = false;
            this.DataGridViewAP.Columns["TypAP"].Visible = false;
            this.DataGridViewAP.Columns["StavObjektu"].Visible = false;

            this.DataGridViewAP.Columns["DatumZalozeni"].HeaderText = @"Created Date";
            this.DataGridViewAP.Columns["DatumZalozeni"].Width = 120;
            this.DataGridViewAP.Columns["DatumZalozeni"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            this.DataGridViewAP.Columns["Zadavatel1Jmeno"].HeaderText = @"Responsible #1";
            this.DataGridViewAP.Columns["Zadavatel1Jmeno"].Width = 200;
            this.DataGridViewAP.Columns["Zadavatel1Jmeno"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            this.DataGridViewAP.Columns["Zadavatel2Jmeno"].HeaderText = @"Responsible #2";
            this.DataGridViewAP.Columns["Zadavatel2Jmeno"].Width = 200;
            this.DataGridViewAP.Columns["Zadavatel2Jmeno"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            this.DataGridViewAP.Columns["Tema"].HeaderText = @"Topic";
            this.DataGridViewAP.Columns["Tema"].Width = 120;
            this.DataGridViewAP.Columns["Tema"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            this.DataGridViewAP.Columns["ZakaznikNazev"].HeaderText = @"Customer Name";
            this.DataGridViewAP.Columns["ZakaznikNazev"].Width = 150;
            this.DataGridViewAP.Columns["ZakaznikNazev"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            this.DataGridViewAP.Columns["ProjektNazev"].HeaderText = @"Project Name";
            this.DataGridViewAP.Columns["ProjektNazev"].Width = 120;
            this.DataGridViewAP.Columns["ProjektNazev"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            this.DataGridViewAP.Columns["TypAPNazev"].HeaderText = @"Type AP";
            this.DataGridViewAP.Columns["TypAPNazev"].Width = 120;
            this.DataGridViewAP.Columns["TypAPNazev"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            this.dtAp.Columns.Add(new DataColumn("OtevrenoUzavreno", typeof(string)));
            this.DataGridViewAP.Columns["OtevrenoUzavreno"].Visible = false;
            var textBox = new DataGridViewTextBoxColumn
            {
                Name = "TextBoxStavOtevrenoUzavreno",
                HeaderText = @"Open / Closed",
                Width = 120,
                DataPropertyName = "OtevrenoUzavreno",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            this.DataGridViewAP.Columns.Add(textBox);

            var style = this.DataGridViewAP.ColumnHeadersDefaultCellStyle;
            style.Font = new Font("Microsoft Sans Serif", 14.0F, GraphicsUnit.Pixel);

            this.DataGridViewAP.MultiSelect = false;
            this.DataGridViewAP.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.DataGridViewAP.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            this.DataGridViewAP.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DataGridViewAP.RowHeadersVisible = true;
            this.DataGridViewAP.AllowUserToResizeRows = false;
            this.DataGridViewAP.AllowUserToResizeColumns = true;
            this.DataGridViewAP.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            this.DataGridViewAP.AllowUserToAddRows = false;
            this.DataGridViewAP.AllowUserToResizeRows = false;
            this.DataGridViewAP.ReadOnly = true;

            //Enable column edit
            foreach (DataGridViewColumn column in this.DataGridViewAP.Columns)
            {
                column.ReadOnly = true;
                //column.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0F, GraphicsUnit.Pixel);
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            this.DataGridViewAP.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            this.DataGridViewAP.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            this.DataGridViewAP.ColumnHeadersDefaultCellStyle.Font =
                new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
        }

        private bool ZobrazitDgv()
        {
            var ap = PrehledAPViewModel.GetAPAll().ToList();
            var zad2 = PrehledAPViewModel.GetZadavatel2().ToList();

            //pokud počet záznamů bude roven 0, namohu naplnit filtry
            if (ap.Count <= 0)
            {
                return false;
            }

            var dt = DataTableConverter.ConvertToDataTable(ap);
            dt.DefaultView.Sort = "DatumZalozeni asc";

            this.dtAp.Rows.Clear();

            foreach (DataRow row in dt.Rows)
            {
                string cisloAPRok;
                cisloAPRok = Convert.ToInt32(row["CisloAP"]).ToString("D3") + " / " +
                             Convert.ToDateTime(row["DatumZalozeni"]).Year;
                string zadavatel2;
                if (DatabaseReader.ConvertIntegerRow(row, "Zadavatel2Id") == null)
                {
                    zadavatel2 = string.Empty;
                }
                else
                {
                    var id = Convert.ToInt32(row["Zadavatel2Id"]);
                    var vyhledaneJmeno = zad2.Find(x => x.Zadavatel2Id == id);
                    zadavatel2 = vyhledaneJmeno?.Zadavatel2;
                }

                var typAP = string.Empty;
                if (Convert.ToInt32(row["TypAP"]) == 1)
                {
                    typAP = "Audits";
                }

                if (Convert.ToInt32(row["TypAP"]) == 2)
                {
                    typAP = "Other";
                }

                var rok = Convert.ToDateTime(row["DatumZalozeni"]).Year;

                var openedClosed = DatabaseReader.ConvertDateTimeRow(row, "DatumUzavreni") == null ? "Open" : "Closed";

                this.dtAp.Rows.Add(
                    row["Id"], row["CisloAP"], cisloAPRok, row["DatumZalozeni"], rok,
                    row["Zadavatel1Id"], row["Zadavatel2Id"], row["Zadavatel1"], zadavatel2, row["Tema"],
                    row["ProjektId"], row["Projekt"], row["ZakaznikId"], row["Zakaznik"], row["TypAP"], typAP,
                    row["StavObjektu"], openedClosed);
            }

            this.dvAp = this.dtAp.DefaultView;

            return true;
        }

        private void DataGridViewAP_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex < 0)
            {
                return;
            }

            var columnIndex = e.ColumnIndex;
            var columnName = this.DataGridViewAP.Columns[columnIndex].Name;
            if (columnName != "TextBoxStavOtevrenoUzavreno")
            {
                return;
            }

            e.CellStyle.BackColor =
                Convert.ToString(this.DataGridViewAP.Rows[e.RowIndex].Cells["OtevrenoUzavreno"].Value) switch
                {
                    "Open" => Color.LightGreen,
                    "Closed" => Color.LightPink,
                    _ => e.CellStyle.BackColor
                };
        }

        private void DataGridViewAP_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            //bool vlastniPrihlasen;

            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }

            if (!(senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn))
            {
                return;
            }

            if (senderGrid.Columns[e.ColumnIndex].Name != "ButtonCisloAPRok")
            {
                return;
            }

            //bez přihlášení se otevře přehled bodů ReadOnly
            var idAP = Convert.ToInt32(this.dvAp[this.DataGridViewAP.CurrentCell.RowIndex]["APId"]);
            this.akcniPlany.Id = idAP;
            string cisloAPRok;
            cisloAPRok =
                Convert.ToInt32(this.dvAp[this.DataGridViewAP.CurrentCell.RowIndex]["CisloAP"])
                    .ToString("D3") + " / " +
                Convert.ToDateTime(this.dvAp[this.DataGridViewAP.CurrentCell.RowIndex]["DatumZalozeni"])
                    .Year;
            this.akcniPlany.CisloAPRok = cisloAPRok;
            this.akcniPlany.Zadavatel1Jmeno =
                Convert.ToString(this.dvAp[this.DataGridViewAP.CurrentCell.RowIndex]["Zadavatel1Jmeno"]);
            if (this.dvAp[this.DataGridViewAP.CurrentCell.RowIndex]["Zadavatel1Jmeno"] == null)
            {
                this.akcniPlany.Zadavatel2Jmeno = null;
            }
            else
            {
                this.akcniPlany.Zadavatel2Jmeno =
                    Convert.ToString(
                        this.dvAp[this.DataGridViewAP.CurrentCell.RowIndex]["Zadavatel2Jmeno"]);
            }

            this.akcniPlany.Tema =
                Convert.ToString(this.dvAp[this.DataGridViewAP.CurrentCell.RowIndex]["Tema"]);
            this.akcniPlany.ProjektNazev =
                this.dvAp[this.DataGridViewAP.CurrentCell.RowIndex]["ProjektNazev"] == null
                    ? null
                    : Convert.ToString(this.dvAp[this.DataGridViewAP.CurrentCell.RowIndex]["ProjektNazev"]);

            this.akcniPlany.DatumZalozeni =
                Convert.ToDateTime(this.dvAp[this.DataGridViewAP.CurrentCell.RowIndex]["DatumZalozeni"]);
            this.akcniPlany.ZakaznikNazev =
                Convert.ToString(this.dvAp[this.DataGridViewAP.CurrentCell.RowIndex]["ZakaznikNazev"]);

            var apUzavren =
                Convert.ToString(this.dvAp[this.DataGridViewAP.CurrentCell.RowIndex]["OtevrenoUzavreno"]) ==
                "Closed";

            this.akcniPlany.APUzavren = apUzavren;
            using var form = this.formPrehledBoduAp;
            form.CreateFormPrehledBoduAP(true, this.akcniPlany, 2);
            var result = form.ShowDialog();
            if (result == DialogResult.OK) { }
        }

        /// <summary>
        /// Data filtering and setup
        /// </summary>
        private void FiltrOdpovedny1()
        {
            var dtOdpovedny = new DataTable();
            dtOdpovedny.Columns.Add("Odpovedny");

            var radek = dtOdpovedny.NewRow();
            radek["Odpovedny"] = "(select all)";
            dtOdpovedny.Rows.Add(radek);

            for (var i = 0; i < this.dvAp.Count; i++)
            {
                radek = dtOdpovedny.NewRow();
                var contains = dtOdpovedny.AsEnumerable().Any(vyhledanyRadek =>
                    this.dvAp[i]["Zadavatel1Jmeno"].ToString() == vyhledanyRadek.Field<string>("Odpovedny"));
                if (contains)
                {
                    continue;
                }

                radek["Odpovedny"] = this.dvAp[i]["Zadavatel1Jmeno"];
                dtOdpovedny.Rows.Add(radek);
            }

            var sortColumn = dtOdpovedny.Columns[0].ColumnName;
            //setřídí položky podle abecedy
            dtOdpovedny.DefaultView.Sort = sortColumn;
            dtOdpovedny = dtOdpovedny.DefaultView.ToTable();

            this.ComboBoxOdpovedny1.DataSource = dtOdpovedny;
            this.ComboBoxOdpovedny1.DisplayMember = "Odpovedny";
        }

        private void ComboBoxOdpovedny1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OdebratHandlery();

            this.odpovedny1Filtr = this.ComboBoxOdpovedny1.SelectedIndex == 0
                ? string.Empty
                : this.ComboBoxOdpovedny1.GetItemText(this.ComboBoxOdpovedny1.SelectedItem);

            this.FiltrovatData();

            this.InitFiltryComboBox();

            this.NastavitVybranouPolozku();

            this.PridatHandlery();
            this.ObarvitLabel();
        }

        private void FiltrOdpovedny2()
        {
            var dtOdpovedny = new DataTable();
            dtOdpovedny.Columns.Add("Odpovedny");

            var radek = dtOdpovedny.NewRow();
            radek["Odpovedny"] = "(select all)";
            dtOdpovedny.Rows.Add(radek);

            for (var i = 0; i < this.dvAp.Count; i++)
            {
                if (this.dvAp[i]["Zadavatel2Jmeno"].ToString() == string.Empty)
                {
                    continue;
                }

                radek = dtOdpovedny.NewRow();
                var contains = dtOdpovedny.AsEnumerable().Any(vyhledanyRadek =>
                    this.dvAp[i]["Zadavatel2Jmeno"].ToString() == vyhledanyRadek.Field<string>("Odpovedny"));

                if (contains)
                {
                    continue;
                }

                radek["Odpovedny"] = this.dvAp[i]["Zadavatel2Jmeno"];
                dtOdpovedny.Rows.Add(radek);
            }

            var sortColumn = dtOdpovedny.Columns[0].ColumnName;
            //setřídí položky podle abecedy
            dtOdpovedny.DefaultView.Sort = sortColumn;
            dtOdpovedny = dtOdpovedny.DefaultView.ToTable();

            this.ComboBoxOdpovedny2.DataSource = dtOdpovedny;
            this.ComboBoxOdpovedny2.DisplayMember = "Odpovedny";
        }

        private void ComboBoxOdpovedny2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OdebratHandlery();

            this.odpovedny2Filtr = this.ComboBoxOdpovedny2.SelectedIndex == 0
                ? string.Empty
                : this.ComboBoxOdpovedny2.GetItemText(this.ComboBoxOdpovedny2.SelectedItem);

            this.FiltrovatData();
            this.InitFiltryComboBox();
            this.NastavitVybranouPolozku();
            this.PridatHandlery();
            this.ObarvitLabel();
        }

        private void FiltrProjekty()
        {
            var dtProjekty = new DataTable();
            dtProjekty.Columns.Add("Projekt");

            var radek = dtProjekty.NewRow();
            radek["Projekt"] = "(select all)";
            dtProjekty.Rows.Add(radek);

            for (var i = 0; i < this.dvAp.Count; i++)
            {
                radek = dtProjekty.NewRow();
                var contains = dtProjekty.AsEnumerable().Any(vyhledanyRadek =>
                    this.dvAp[i]["ProjektNazev"].ToString() == vyhledanyRadek.Field<string>("Projekt"));

                if (contains)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(Convert.ToString(this.dvAp[i]["ProjektNazev"])))
                {
                }
                else
                {
                    radek["Projekt"] = this.dvAp[i]["ProjektNazev"];
                    dtProjekty.Rows.Add(radek);
                }
            }

            var sortColumn = dtProjekty.Columns[0].ColumnName;
            //setřídí položky podle abecedy
            dtProjekty.DefaultView.Sort = sortColumn;
            dtProjekty = dtProjekty.DefaultView.ToTable();

            this.ComboBoxProjekty.DataSource = dtProjekty;
            this.ComboBoxProjekty.DisplayMember = "Projekt";
        }

        private void ComboBoxProjekty_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OdebratHandlery();

            this.projektyFiltr = this.ComboBoxProjekty.SelectedIndex == 0
                ? string.Empty
                : this.ComboBoxProjekty.GetItemText(this.ComboBoxProjekty.SelectedItem);

            this.FiltrovatData();

            this.InitFiltryComboBox();

            this.NastavitVybranouPolozku();

            this.PridatHandlery();
            this.ObarvitLabel();
        }

        private void FiltrTypAp()
        {
            var dtTypAP = new DataTable();
            dtTypAP.Columns.Add("TypAP");

            var radek = dtTypAP.NewRow();
            radek["TypAP"] = "(select all)";
            dtTypAP.Rows.Add(radek);

            for (var i = 0; i < this.dvAp.Count; i++)
            {
                radek = dtTypAP.NewRow();
                var contains = dtTypAP.AsEnumerable().Any(vyhledanyRadek =>
                    this.dvAp[i]["TypAPNazev"].ToString() == vyhledanyRadek.Field<string>("TypAP"));

                if (contains)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(Convert.ToString(this.dvAp[i]["TypAPNazev"])))
                {
                    continue;
                }

                radek["TypAP"] = this.dvAp[i]["TypAPNazev"];
                dtTypAP.Rows.Add(radek);
            }

            var sortColumn = dtTypAP.Columns[0].ColumnName;
            //setřídí položky podle abecedy
            dtTypAP.DefaultView.Sort = sortColumn;
            dtTypAP = dtTypAP.DefaultView.ToTable();

            this.ComboBoxTypAP.DataSource = dtTypAP;
            this.ComboBoxTypAP.DisplayMember = "TypAP";
        }

        private void ComboBoxTypAP_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OdebratHandlery();

            this.typApFiltr = this.ComboBoxTypAP.SelectedIndex == 0
                ? string.Empty
                : this.ComboBoxTypAP.GetItemText(this.ComboBoxTypAP.SelectedItem);

            this.FiltrovatData();
            this.InitFiltryComboBox();
            this.NastavitVybranouPolozku();
            this.PridatHandlery();
            this.ObarvitLabel();
        }

        private void FiltrRokZalozeni()
        {
            var dtRokZalozeni = new DataTable();
            dtRokZalozeni.Columns.Add("RokZalozeni");

            var radek = dtRokZalozeni.NewRow();
            radek["RokZalozeni"] = "(select all)";
            dtRokZalozeni.Rows.Add(radek);

            for (var i = 0; i < this.dvAp.Count; i++)
            {
                radek = dtRokZalozeni.NewRow();
                var contains = dtRokZalozeni.AsEnumerable().Any(vyhledanyRadek =>
                    this.dvAp[i]["DatumZalozeniRok"].ToString() == vyhledanyRadek.Field<string>("RokZalozeni"));

                if (contains)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(Convert.ToString(this.dvAp[i]["DatumZalozeniRok"])))
                {
                }
                else
                {
                    radek["RokZalozeni"] = this.dvAp[i]["DatumZalozeniRok"];
                    dtRokZalozeni.Rows.Add(radek);
                }
            }

            var sortColumn = dtRokZalozeni.Columns[0].ColumnName;
            //setřídí položky podle abecedy
            dtRokZalozeni.DefaultView.Sort = sortColumn;
            dtRokZalozeni = dtRokZalozeni.DefaultView.ToTable();

            this.ComboBoxRoky.DataSource = dtRokZalozeni;
            this.ComboBoxRoky.DisplayMember = "RokZalozeni";
        }

        private void ComboBoxRoky_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OdebratHandlery();

            this.rokZalozeniFiltr = this.ComboBoxRoky.SelectedIndex == 0
                ? string.Empty
                : this.ComboBoxRoky.GetItemText(this.ComboBoxRoky.SelectedItem);

            this.FiltrovatData();
            this.InitFiltryComboBox();
            this.NastavitVybranouPolozku();
            this.PridatHandlery();
            this.ObarvitLabel();
        }

        private void FiltrOtevreneUzavrene()
        {
            var dtOtevreneUzavrene = new DataTable();
            dtOtevreneUzavrene.Columns.Add("OtevreneUzavrene");

            var radek = dtOtevreneUzavrene.NewRow();
            radek["OtevreneUzavrene"] = "(select all)";
            dtOtevreneUzavrene.Rows.Add(radek);

            for (var i = 0; i < this.dvAp.Count; i++)
            {
                radek = dtOtevreneUzavrene.NewRow();
                var contains = dtOtevreneUzavrene.AsEnumerable().Any(vyhledanyRadek =>
                    this.dvAp[i]["OtevrenoUzavreno"].ToString() == vyhledanyRadek.Field<string>("OtevreneUzavrene"));

                if (contains)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(Convert.ToString(this.dvAp[i]["OtevrenoUzavreno"])))
                {
                    continue;
                }

                radek["OtevreneUzavrene"] = this.dvAp[i]["OtevrenoUzavreno"];
                dtOtevreneUzavrene.Rows.Add(radek);
            }

            var sortColumn = dtOtevreneUzavrene.Columns[0].ColumnName;
            //setřídí položky podle abecedy
            dtOtevreneUzavrene.DefaultView.Sort = sortColumn;
            dtOtevreneUzavrene = dtOtevreneUzavrene.DefaultView.ToTable();

            this.ComboBoxOtevreneUzavrene.DataSource = dtOtevreneUzavrene;
            this.ComboBoxOtevreneUzavrene.DisplayMember = "OtevreneUzavrene";
        }

        private void ComboBoxOtevreneUzavrene_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OdebratHandlery();

            this.otevreneUzavreneFiltr = this.ComboBoxOtevreneUzavrene.SelectedIndex == 0 ? string.Empty : this.ComboBoxOtevreneUzavrene.GetItemText(this.ComboBoxOtevreneUzavrene.SelectedItem);

            this.FiltrovatData();

            this.InitFiltryComboBox();

            this.NastavitVybranouPolozku();

            this.PridatHandlery();
            this.ObarvitLabel();
        }

        private void InitFiltryComboBox()
        {
            this.FiltrOdpovedny1();
            this.FiltrOdpovedny2();
            this.FiltrProjekty();
            this.FiltrTypAp();
            this.FiltrRokZalozeni();
            this.FiltrOtevreneUzavrene();
        }

        private void PridatHandlery()
        {
            this.ComboBoxOdpovedny1.SelectedIndexChanged += this.ComboBoxOdpovedny1_SelectedIndexChanged;
            this.ComboBoxOdpovedny2.SelectedIndexChanged += this.ComboBoxOdpovedny2_SelectedIndexChanged;
            this.ComboBoxProjekty.SelectedIndexChanged += this.ComboBoxProjekty_SelectedIndexChanged;
            this.ComboBoxTypAP.SelectedIndexChanged += this.ComboBoxTypAP_SelectedIndexChanged;
            this.ComboBoxRoky.SelectedIndexChanged += this.ComboBoxRoky_SelectedIndexChanged;
            this.ComboBoxOtevreneUzavrene.SelectedIndexChanged += this.ComboBoxOtevreneUzavrene_SelectedIndexChanged;
        }

        private void OdebratHandlery()
        {
            this.ComboBoxOdpovedny1.SelectedIndexChanged -= this.ComboBoxOdpovedny1_SelectedIndexChanged;
            this.ComboBoxOdpovedny2.SelectedIndexChanged -= this.ComboBoxOdpovedny2_SelectedIndexChanged;
            this.ComboBoxProjekty.SelectedIndexChanged -= this.ComboBoxProjekty_SelectedIndexChanged;
            this.ComboBoxTypAP.SelectedIndexChanged -= this.ComboBoxTypAP_SelectedIndexChanged;
            this.ComboBoxRoky.SelectedIndexChanged -= this.ComboBoxRoky_SelectedIndexChanged;
            this.ComboBoxOtevreneUzavrene.SelectedIndexChanged -= this.ComboBoxOtevreneUzavrene_SelectedIndexChanged;
        }

        private void InitFiltr()
        {
            this.odpovedny1Filtr = string.Empty;
            this.odpovedny2Filtr = string.Empty;
            this.projektyFiltr = string.Empty;
            this.typApFiltr = string.Empty;
            this.rokZalozeniFiltr = string.Empty;
            this.otevreneUzavreneFiltr = string.Empty;
        }

        private void NastavitVybranouPolozku()
        {
            this.ComboBoxOdpovedny1.SelectedIndex = this.odpovedny1Filtr == string.Empty
                ? 0
                : this.ComboBoxOdpovedny1.FindStringExact(this.odpovedny1Filtr);
            this.ComboBoxOdpovedny2.SelectedIndex = this.odpovedny2Filtr == string.Empty
                ? 0
                : this.ComboBoxOdpovedny2.FindStringExact(this.odpovedny2Filtr);
            this.ComboBoxProjekty.SelectedIndex = this.projektyFiltr == string.Empty
                ? 0
                : this.ComboBoxProjekty.FindStringExact(this.projektyFiltr);
            this.ComboBoxTypAP.SelectedIndex = this.typApFiltr == string.Empty
                ? 0
                : this.ComboBoxTypAP.FindStringExact(this.typApFiltr);
            this.ComboBoxRoky.SelectedIndex = this.rokZalozeniFiltr == string.Empty
                ? 0
                : this.ComboBoxRoky.FindStringExact(this.rokZalozeniFiltr);
            this.ComboBoxOtevreneUzavrene.SelectedIndex = this.otevreneUzavreneFiltr == string.Empty
                ? 0
                : this.ComboBoxOtevreneUzavrene.FindStringExact(this.otevreneUzavreneFiltr);
        }

        private void ObarvitLabel()
        {
            this.labelOdpovedny1.ForeColor = this.odpovedny1Filtr == string.Empty ? Color.Black : Color.Blue;
            this.labelOdpovedny2.ForeColor = this.odpovedny2Filtr == string.Empty ? Color.Black : Color.Blue;
            this.labelProjekty.ForeColor = this.projektyFiltr == string.Empty ? Color.Black : Color.Blue;
            this.labelTypAP.ForeColor = this.typApFiltr == string.Empty ? Color.Black : Color.Blue;
            this.labelRoky.ForeColor = this.rokZalozeniFiltr == string.Empty ? Color.Black : Color.Blue;
            this.labelOtevreneUzavrene.ForeColor =
                this.otevreneUzavreneFiltr == string.Empty ? Color.Black : Color.Blue;
        }

        private void FiltrovatData()
        {
            if (this.dvAp.Count == 0)
            {
                //když bude počet vyfiltrovaných řádek roven 0, vynulují se všechny fitlry
                //to nastane, když bude vystornován poslední vyfiltrovaný řádek
                //v tomto případě budou odstraněny všechny filtry, počáteční stav
                this.InitFiltr();
                this.dvAp.RowFilter = string.Empty;
            }
            else
            {
                this.dvAp.RowFilter = string.Empty;
                if (!string.IsNullOrEmpty(this.odpovedny1Filtr))
                {
                    this.dvAp.RowFilter = $"Zadavatel1Jmeno = '{this.odpovedny1Filtr}'";
                }

                if (!string.IsNullOrEmpty(this.odpovedny2Filtr))
                {
                    if (this.dvAp.RowFilter == string.Empty)
                    {
                        this.dvAp.RowFilter = $"Zadavatel2Jmeno = '{this.odpovedny2Filtr}'";
                    }
                    else
                    {
                        this.dvAp.RowFilter += $" AND Zadavatel2Jmeno = '{this.odpovedny2Filtr}'";
                    }
                }

                if (!string.IsNullOrEmpty(this.projektyFiltr))
                {
                    if (string.IsNullOrEmpty(this.dvAp.RowFilter))
                    {
                        this.dvAp.RowFilter = $"ProjektNazev = '{this.projektyFiltr}'";
                    }
                    else
                    {
                        this.dvAp.RowFilter += $" AND ProjektNazev = '{this.projektyFiltr}'";
                    }
                }

                if (!string.IsNullOrEmpty(this.typApFiltr))
                {
                    if (this.dvAp.RowFilter == string.Empty)
                    {
                        this.dvAp.RowFilter = $"TypAPNazev = '{this.typApFiltr}'";
                    }
                    else
                    {
                        this.dvAp.RowFilter += $" AND TypAPNazev = '{this.typApFiltr}'";
                    }
                }

                if (!string.IsNullOrEmpty(this.rokZalozeniFiltr))
                {
                    if (this.dvAp.RowFilter == string.Empty)
                    {
                        this.dvAp.RowFilter = $"DatumZalozeniRok = {this.rokZalozeniFiltr}";
                    }
                    else
                    {
                        this.dvAp.RowFilter += $" AND DatumZalozeniRok = {this.rokZalozeniFiltr}";
                    }
                }

                if (string.IsNullOrEmpty(this.otevreneUzavreneFiltr))
                {
                    return;
                }

                if (string.IsNullOrEmpty(this.dvAp.RowFilter))
                {
                    this.dvAp.RowFilter = $"OtevrenoUzavreno = '{this.otevreneUzavreneFiltr}'";
                }
                else
                {
                    this.dvAp.RowFilter += $" AND OtevrenoUzavreno = '{this.otevreneUzavreneFiltr}'";
                }
            }
        }

        private void ButtonEditAP_MouseClick(object sender, MouseEventArgs e)
        {
            var currentIndexDGV = this.DataGridViewAP.CurrentCell.RowIndex;

            using (var form = this.formEditAp)
            {
                form.CreateFormEditAp(Convert.ToInt32(this.dvAp[currentIndexDGV]["APId"]),
                    Convert.ToString(this.dvAp[currentIndexDGV]["CisloAPRok"]),
                    Convert.ToString(this.dvAp[currentIndexDGV]["Zadavatel1Jmeno"]),
                    Convert.ToString(this.dvAp[currentIndexDGV]["Zadavatel2Jmeno"]),
                    Convert.ToString(this.dvAp[currentIndexDGV]["Tema"]),
                    Convert.ToString(this.dvAp[currentIndexDGV]["ProjektNazev"]),
                    Convert.ToString(this.dvAp[currentIndexDGV]["ZakaznikNazev"]),
                    Convert.ToInt32(this.dvAp[currentIndexDGV]["Zadavatel1Id"]),
                    this.dvAp[currentIndexDGV]["Zadavatel2Id"] == DBNull.Value
                        ? (int?)null
                        : Convert.ToInt32(this.dvAp[currentIndexDGV]["Zadavatel2Id"]),
                    this.dvAp[currentIndexDGV]["ProjektId"] == DBNull.Value
                        ? (int?)null
                        : Convert.ToInt32(this.dvAp[currentIndexDGV]["ProjektId"]),
                    Convert.ToInt32(this.dvAp[currentIndexDGV]["ZakaznikId"]));
                form.ShowDialog();
                this.ZobrazitDgv();
            }

            //nastaví původně vybraný řádek
            this.DataGridViewAP.Rows[currentIndexDGV].Selected = true;
            this.DataGridViewAP.CurrentCell = this.DataGridViewAP.Rows[currentIndexDGV].Cells["DatumZalozeni"];
        }

        private void ButtonClose_MouseClick(object sender, MouseEventArgs e) => this.Close();
    }
}
