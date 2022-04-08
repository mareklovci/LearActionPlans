using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using LearActionPlans.ViewModels;
using LearActionPlans.Utilities;
using System.Collections.Generic;

namespace LearActionPlans.Views
{
    public partial class FormPrehledAP : Form
    {
        private readonly BindingSource bindingSourceAP;
        private DataTable dtAP;
        private DataView dvAP;

        private FormNovyAkcniPlan.AkcniPlanTmp akcniPlany;

        private string odpovedny1Filtr;
        private string odpovedny2Filtr;
        private string projektyFiltr;
        private string typAPFiltr;
        private string rokZalozeniFiltr;
        private string otevreneUzavreneFiltr;

        private readonly string[] args_;
        private readonly ArgumentOptions arguments;

        private readonly string cisloAPStr;
        private readonly int apId;
        private readonly int bodAPId;
        private readonly int ukonceniAkceId;
        private readonly int vlastnikAkceId;

        public FormPrehledAP(ArgumentOptions arguments)
        {
            this.arguments = arguments;

            this.InitializeComponent();

            this.cisloAPStr = arguments.ActionPlanNumber;
            this.apId = arguments.ActionPlanId;
            this.bodAPId = arguments.ActionPlanPointId;
            this.ukonceniAkceId = arguments.ActionEndId;
            this.vlastnikAkceId = arguments.ActionOwnerId;

            this.bindingSourceAP = new BindingSource();
            this.dtAP = new DataTable();

            this.akcniPlany = new FormNovyAkcniPlan.AkcniPlanTmp();
        }

        private void FormPrehledAP_Load(object sender, EventArgs e)
        {
            this.bindingSourceAP.DataSource = this.dtAP;
            this.DataGridViewAP.DataSource = this.bindingSourceAP;

            //urychlení načítání dat v DGV
            if (!SystemInformation.TerminalServerSession)
            {
                var dgvType = this.DataGridViewAP.GetType();
                var pi = dgvType.GetProperty("DoubleBuffered",
                    BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(this.DataGridViewAP, true, null);
            }

            this.CreateColumnsAP();
            if (this.ZobrazitDGV() == true)
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
            if (!this.arguments.RunWithoutParameters)
            {
                // podle apId vyhledám řádek, který tomu odpovídá
                var vyhledanyRadek = 0;
                for (var i = 0; i < this.dvAP.Count; i++)
                {
                    if (Convert.ToInt32(this.dvAP[i]["APId"]) == Convert.ToInt32(this.apId))
                    {
                        this.HlavickaAP(vyhledanyRadek);
                        break;
                    }
                    vyhledanyRadek++;
                }

                //var idAP = Convert.ToInt32(this.apId);
                //this.akcniPlany.Id = idAP;
                //string cisloAPRok;
                //cisloAPRok = this.cisloAPStr;
                //this.akcniPlany.CisloAPRok = cisloAPRok;
                //this.akcniPlany.Zadavatel1Id = this.vlastnikAkceId;
                using var form = new FormPrehledBoduAP(false, this.akcniPlany, 2, this.bodAPId);
                _ = form.ShowDialog();
                //var result = form.ShowDialog();
                //if (result == DialogResult.OK)
                //{
                //}
            }
        }

        private void CreateColumnsAP()
        {
            this.dtAP.Columns.Add(new DataColumn("APId", typeof(int)));
            this.dtAP.Columns.Add(new DataColumn("CisloAP", typeof(int)));
            this.dtAP.Columns.Add(new DataColumn("CisloAPRok", typeof(string)));

            var colBtn = new DataGridViewButtonColumn
            {
                Name = "ButtonCisloAPRok",
                HeaderText = @"AP number",
                Width = 100,
                DataPropertyName = "CisloAPRok",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            this.DataGridViewAP.Columns.Add(colBtn);

            this.dtAP.Columns.Add(new DataColumn("DatumZalozeni", typeof(DateTime)));
            //this.dtAP.Columns.Add(new DataColumn("DatumUkonceni", typeof(DateTime)));
            this.dtAP.Columns.Add(new DataColumn("DatumZalozeniRok", typeof(int)));
            this.dtAP.Columns.Add(new DataColumn("Zadavatel1Id", typeof(int)));
            this.dtAP.Columns.Add(new DataColumn("Zadavatel2Id", typeof(int)));
            this.dtAP.Columns.Add(new DataColumn("Zadavatel1Jmeno", typeof(string)));
            this.dtAP.Columns.Add(new DataColumn("Zadavatel2Jmeno", typeof(string)));
            this.dtAP.Columns.Add(new DataColumn("Tema", typeof(string)));
            this.dtAP.Columns.Add(new DataColumn("ProjektId", typeof(int)));
            this.dtAP.Columns.Add(new DataColumn("ProjektNazev", typeof(string)));
            this.dtAP.Columns.Add(new DataColumn("ZakaznikId", typeof(int)));
            this.dtAP.Columns.Add(new DataColumn("ZakaznikNazev", typeof(string)));
            this.dtAP.Columns.Add(new DataColumn("TypAP", typeof(byte)));
            this.dtAP.Columns.Add(new DataColumn("TypAPNazev", typeof(string)));
            // asi možná nebudu potřebovat
            this.dtAP.Columns.Add(new DataColumn("StavObjektu", typeof(byte)));

            this.DataGridViewAP.Columns["APId"].Visible = false;
            this.DataGridViewAP.Columns["CisloAP"].Visible = false;
            this.DataGridViewAP.Columns["CisloAPRok"].Visible = false;
            //this.DataGridViewAP.Columns["DatumUkonceni"].Visible = false;
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

            this.dtAP.Columns.Add(new DataColumn("OtevrenoUzavreno", typeof(string)));
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

        private bool ZobrazitDGV()
        {
            var zobrazitZaznamy = false;

            var ap = PrehledAPViewModel.GetAPAll().ToList();
            var zad2 = PrehledAPViewModel.GetZadavatel2().ToList();

            //pokud počet záznamů bude roven 0, namohu naplnit filtry
            if (ap.Count > 0)
            {
                zobrazitZaznamy = true;
                var dt = DataTableConverter.ConvertToDataTable(ap);
                dt.DefaultView.Sort = "DatumZalozeni asc";

                this.dtAP.Rows.Clear();

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
                        zadavatel2 = vyhledaneJmeno.Zadavatel2;
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

                    var otevrenoUzavreno = string.Empty;
                    if (DatabaseReader.ConvertDateTimeRow(row, "DatumUzavreni") == null)
                    {
                        otevrenoUzavreno = "Open";
                    }
                    else
                    {
                        otevrenoUzavreno = "Closed";
                    }

                    this.dtAP.Rows.Add(new object[]
                    {
                        row["Id"], row["CisloAP"], cisloAPRok, row["DatumZalozeni"], rok, row["Zadavatel1Id"],
                        row["Zadavatel2Id"], row["Zadavatel1"], zadavatel2, row["Tema"], row["ProjektId"],
                        row["Projekt"], row["ZakaznikId"], row["Zakaznik"], row["TypAP"], typAP, row["StavObjektu"],
                        otevrenoUzavreno
                    });
                }

                this.dvAP = this.dtAP.DefaultView;
                //dvAP.RowFilter = string.Format("DatumZalozeniRok = {0}", DateTime.Now.Year);
                //dvAP.RowFilter = string.Empty;
                //dvAP.Sort = "CisloAP";
            }

            return zobrazitZaznamy;
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
            //if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            //{
            //}
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                if (senderGrid.Columns[e.ColumnIndex].Name == "ButtonCisloAPRok")
                {
                    this.HlavickaAP(this.DataGridViewAP.CurrentCell.RowIndex);
                    using var form = new FormPrehledBoduAP(true, this.akcniPlany, 2, -1);
                    _ = form.ShowDialog();
                    //var result = form.ShowDialog();
                    //if (result == DialogResult.OK) { }
                }
            }
        }

        // naplní hlavičku vybraného AP před jeho zobrazením
        private void HlavickaAP(int radek)
        {
            //bez přihlášení se otevře přehled bodů ReadOnly
            this.akcniPlany.Id = Convert.ToInt32(this.dvAP[radek]["APId"]);
            string cisloAPRok;
            cisloAPRok =
                Convert.ToInt32(this.dvAP[radek]["CisloAP"])
                    .ToString("D3") + " / " +
                Convert.ToDateTime(this.dvAP[radek]["DatumZalozeni"])
                    .Year;
            this.akcniPlany.CisloAPRok = cisloAPRok;
            this.akcniPlany.Zadavatel1Jmeno =
                Convert.ToString(this.dvAP[radek]["Zadavatel1Jmeno"]);

            this.akcniPlany.Zadavatel1Id = Convert.ToInt32(
                        this.dvAP[radek]["Zadavatel1Id"]);

            if (string.IsNullOrEmpty(Convert.ToString(this.dvAP[radek]["Zadavatel2Jmeno"])))
            {
                this.akcniPlany.Zadavatel2Jmeno = null;
            }
            else
            {
                this.akcniPlany.Zadavatel2Jmeno =
                    Convert.ToString(
                        this.dvAP[radek]["Zadavatel2Jmeno"]);
                this.akcniPlany.Zadavatel2Id = Convert.ToInt32(
                            this.dvAP[radek]["Zadavatel2Id"]);
            }

            this.akcniPlany.Tema =
                Convert.ToString(this.dvAP[radek]["Tema"]);
            if (string.IsNullOrEmpty(Convert.ToString(this.dvAP[radek]["ProjektNazev"])))
            {
                this.akcniPlany.ProjektNazev = null;
            }
            else
            {
                this.akcniPlany.ProjektNazev =
                    Convert.ToString(this.dvAP[radek]["ProjektNazev"]);
            }

            this.akcniPlany.DatumZalozeni = Convert.ToDateTime(this.dvAP[radek]["DatumZalozeni"]);
            //this.akcniPlany.DatumUkonceni = Convert.ToDateTime(this.dvAP[radek]["DatumUkonceni"]);

            this.akcniPlany.ZakaznikNazev =
                Convert.ToString(this.dvAP[radek]["ZakaznikNazev"]);

            var apUzavren =
                Convert.ToString(this.dvAP[radek]["OtevrenoUzavreno"]) ==
                "Closed";

            this.akcniPlany.APUzavren = apUzavren;
        }

        //filtrování dat, nastavení filtrů
        private void FiltrOdpovedny1()
        {
            var dtOdpovedny = new DataTable();
            dtOdpovedny.Columns.Add("Odpovedny");

            var radek = dtOdpovedny.NewRow();
            radek["Odpovedny"] = "(select all)";
            dtOdpovedny.Rows.Add(radek);

            for (var i = 0; i < this.dvAP.Count; i++)
            {
                radek = dtOdpovedny.NewRow();
                var contains = dtOdpovedny.AsEnumerable().Any(vyhledanyRadek =>
                    this.dvAP[i]["Zadavatel1Jmeno"].ToString() == vyhledanyRadek.Field<string>("Odpovedny"));
                if (contains == false)
                {
                    radek["Odpovedny"] = this.dvAP[i]["Zadavatel1Jmeno"];
                    dtOdpovedny.Rows.Add(radek);
                }
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

            if (this.ComboBoxOdpovedny1.SelectedIndex == 0)
            {
                this.odpovedny1Filtr = string.Empty;
            }
            else
            {
                this.odpovedny1Filtr = this.ComboBoxOdpovedny1.GetItemText(this.ComboBoxOdpovedny1.SelectedItem);
            }

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

            for (var i = 0; i < this.dvAP.Count; i++)
            {
                if (this.dvAP[i]["Zadavatel2Jmeno"].ToString() == string.Empty) { }
                else
                {
                    radek = dtOdpovedny.NewRow();
                    var contains = dtOdpovedny.AsEnumerable().Any(vyhledanyRadek =>
                        this.dvAP[i]["Zadavatel2Jmeno"].ToString() == vyhledanyRadek.Field<string>("Odpovedny"));
                    if (contains == false)
                    {
                        radek["Odpovedny"] = this.dvAP[i]["Zadavatel2Jmeno"];
                        dtOdpovedny.Rows.Add(radek);
                    }
                }
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

            if (this.ComboBoxOdpovedny2.SelectedIndex == 0)
            {
                this.odpovedny2Filtr = string.Empty;
            }
            else
            {
                this.odpovedny2Filtr = this.ComboBoxOdpovedny2.GetItemText(this.ComboBoxOdpovedny2.SelectedItem);
            }

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

            for (var i = 0; i < this.dvAP.Count; i++)
            {
                radek = dtProjekty.NewRow();
                var contains = dtProjekty.AsEnumerable().Any(vyhledanyRadek =>
                    this.dvAP[i]["ProjektNazev"].ToString() == vyhledanyRadek.Field<string>("Projekt"));
                if (contains == false)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(this.dvAP[i]["ProjektNazev"])))
                    {
                    }
                    else
                    {
                        radek["Projekt"] = this.dvAP[i]["ProjektNazev"];
                        dtProjekty.Rows.Add(radek);
                    }
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

            if (this.ComboBoxProjekty.SelectedIndex == 0)
            {
                this.projektyFiltr = string.Empty;
            }
            else
            {
                this.projektyFiltr = this.ComboBoxProjekty.GetItemText(this.ComboBoxProjekty.SelectedItem);
            }

            this.FiltrovatData();

            this.InitFiltryComboBox();

            this.NastavitVybranouPolozku();

            this.PridatHandlery();
            this.ObarvitLabel();
        }

        private void FiltrTypAP()
        {
            var dtTypAP = new DataTable();
            dtTypAP.Columns.Add("TypAP");

            var radek = dtTypAP.NewRow();
            radek["TypAP"] = "(select all)";
            dtTypAP.Rows.Add(radek);

            for (var i = 0; i < this.dvAP.Count; i++)
            {
                radek = dtTypAP.NewRow();
                var contains = dtTypAP.AsEnumerable().Any(vyhledanyRadek =>
                    this.dvAP[i]["TypAPNazev"].ToString() == vyhledanyRadek.Field<string>("TypAP"));
                if (contains == false)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(this.dvAP[i]["TypAPNazev"])))
                    {
                    }
                    else
                    {
                        radek["TypAP"] = this.dvAP[i]["TypAPNazev"];
                        dtTypAP.Rows.Add(radek);
                    }
                }
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

            if (this.ComboBoxTypAP.SelectedIndex == 0)
            {
                this.typAPFiltr = string.Empty;
            }
            else
            {
                this.typAPFiltr = this.ComboBoxTypAP.GetItemText(this.ComboBoxTypAP.SelectedItem);
            }

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

            for (var i = 0; i < this.dvAP.Count; i++)
            {
                radek = dtRokZalozeni.NewRow();
                var contains = dtRokZalozeni.AsEnumerable().Any(vyhledanyRadek =>
                    this.dvAP[i]["DatumZalozeniRok"].ToString() == vyhledanyRadek.Field<string>("RokZalozeni"));
                if (contains == false)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(this.dvAP[i]["DatumZalozeniRok"])))
                    {
                    }
                    else
                    {
                        radek["RokZalozeni"] = this.dvAP[i]["DatumZalozeniRok"];
                        dtRokZalozeni.Rows.Add(radek);
                    }
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

            if (this.ComboBoxRoky.SelectedIndex == 0)
            {
                this.rokZalozeniFiltr = string.Empty;
            }
            else
            {
                this.rokZalozeniFiltr = this.ComboBoxRoky.GetItemText(this.ComboBoxRoky.SelectedItem);
            }

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

            for (var i = 0; i < this.dvAP.Count; i++)
            {
                radek = dtOtevreneUzavrene.NewRow();
                var contains = dtOtevreneUzavrene.AsEnumerable().Any(vyhledanyRadek =>
                    this.dvAP[i]["OtevrenoUzavreno"].ToString() == vyhledanyRadek.Field<string>("OtevreneUzavrene"));
                if (contains == false)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(this.dvAP[i]["OtevrenoUzavreno"])))
                    {
                    }
                    else
                    {
                        radek["OtevreneUzavrene"] = this.dvAP[i]["OtevrenoUzavreno"];
                        dtOtevreneUzavrene.Rows.Add(radek);
                    }
                }
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

            if (this.ComboBoxOtevreneUzavrene.SelectedIndex == 0)
            {
                this.otevreneUzavreneFiltr = string.Empty;
            }
            else
            {
                this.otevreneUzavreneFiltr =
                    this.ComboBoxOtevreneUzavrene.GetItemText(this.ComboBoxOtevreneUzavrene.SelectedItem);
            }

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
            this.FiltrTypAP();
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
            this.typAPFiltr = string.Empty;
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
            this.ComboBoxTypAP.SelectedIndex = this.typAPFiltr == string.Empty
                ? 0
                : this.ComboBoxTypAP.FindStringExact(this.typAPFiltr);
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
            this.labelTypAP.ForeColor = this.typAPFiltr == string.Empty ? Color.Black : Color.Blue;
            this.labelRoky.ForeColor = this.rokZalozeniFiltr == string.Empty ? Color.Black : Color.Blue;
            this.labelOtevreneUzavrene.ForeColor =
                this.otevreneUzavreneFiltr == string.Empty ? Color.Black : Color.Blue;
        }

        public void FiltrovatData()
        {
            if (this.dvAP.Count == 0)
            {
                //když bude počet vyfiltrovaných řádek roven 0, vynulují se všechny fitlry
                //to nastane, když bude vystornován poslední vyfiltrovaný řádek
                //v tomto případě budou odstraněny všechny filtry, počáteční stav
                this.InitFiltr();
                this.dvAP.RowFilter = string.Empty;
            }
            else
            {
                this.dvAP.RowFilter = string.Empty;
                if (!string.IsNullOrEmpty(this.odpovedny1Filtr))
                {
                    this.dvAP.RowFilter = string.Format("Zadavatel1Jmeno = '{0}'", this.odpovedny1Filtr);
                }

                if (!string.IsNullOrEmpty(this.odpovedny2Filtr))
                {
                    if (this.dvAP.RowFilter == string.Empty)
                    {
                        this.dvAP.RowFilter = string.Format("Zadavatel2Jmeno = '{0}'", this.odpovedny2Filtr);
                    }
                    else
                    {
                        this.dvAP.RowFilter += string.Format(" AND Zadavatel2Jmeno = '{0}'", this.odpovedny2Filtr);
                    }
                }

                if (!string.IsNullOrEmpty(this.projektyFiltr))
                {
                    if (this.dvAP.RowFilter == string.Empty)
                    {
                        this.dvAP.RowFilter = string.Format("ProjektNazev = '{0}'", this.projektyFiltr);
                    }
                    else
                    {
                        this.dvAP.RowFilter += string.Format(" AND ProjektNazev = '{0}'", this.projektyFiltr);
                    }
                }

                if (!string.IsNullOrEmpty(this.typAPFiltr))
                {
                    if (this.dvAP.RowFilter == string.Empty)
                    {
                        this.dvAP.RowFilter = string.Format("TypAPNazev = '{0}'", this.typAPFiltr);
                    }
                    else
                    {
                        this.dvAP.RowFilter += string.Format(" AND TypAPNazev = '{0}'", this.typAPFiltr);
                    }
                }

                if (!string.IsNullOrEmpty(this.rokZalozeniFiltr))
                {
                    if (this.dvAP.RowFilter == string.Empty)
                    {
                        this.dvAP.RowFilter = string.Format("DatumZalozeniRok = {0}", this.rokZalozeniFiltr);
                    }
                    else
                    {
                        this.dvAP.RowFilter += string.Format(" AND DatumZalozeniRok = {0}", this.rokZalozeniFiltr);
                    }
                }

                if (!string.IsNullOrEmpty(this.otevreneUzavreneFiltr))
                {
                    if (this.dvAP.RowFilter == string.Empty)
                    {
                        this.dvAP.RowFilter = string.Format("OtevrenoUzavreno = '{0}'", this.otevreneUzavreneFiltr);
                    }
                    else
                    {
                        this.dvAP.RowFilter +=
                            string.Format(" AND OtevrenoUzavreno = '{0}'", this.otevreneUzavreneFiltr);
                    }
                }
            }
        }

        private void ButtonEditAP_MouseClick(object sender, MouseEventArgs e)
        {
            var currentIndexDGV = this.DataGridViewAP.CurrentCell.RowIndex;

            using (var form = new FormEditAP(
                       Convert.ToInt32(this.dvAP[currentIndexDGV]["APId"]),
                       Convert.ToString(this.dvAP[currentIndexDGV]["CisloAPRok"]),
                       Convert.ToString(this.dvAP[currentIndexDGV]["Zadavatel1Jmeno"]),
                       Convert.ToString(this.dvAP[currentIndexDGV]["Zadavatel2Jmeno"]),
                       Convert.ToString(this.dvAP[currentIndexDGV]["Tema"]),
                       Convert.ToString(this.dvAP[currentIndexDGV]["ProjektNazev"]),
                       Convert.ToString(this.dvAP[currentIndexDGV]["ZakaznikNazev"]),
                       Convert.ToInt32(this.dvAP[currentIndexDGV]["Zadavatel1Id"]),
                       this.dvAP[currentIndexDGV]["Zadavatel2Id"] == DBNull.Value
                           ? (int?)null
                           : Convert.ToInt32(this.dvAP[currentIndexDGV]["Zadavatel2Id"]),
                       this.dvAP[currentIndexDGV]["ProjektId"] == DBNull.Value
                           ? (int?)null
                           : Convert.ToInt32(this.dvAP[currentIndexDGV]["ProjektId"]),
                       Convert.ToInt32(this.dvAP[currentIndexDGV]["ZakaznikId"])))
            {
                form.ShowDialog();
                this.ZobrazitDGV();
            }

            //nastaví původně vybraný řádek
            this.DataGridViewAP.Rows[currentIndexDGV].Selected = true;
            this.DataGridViewAP.CurrentCell = this.DataGridViewAP.Rows[currentIndexDGV].Cells["DatumZalozeni"];
        }


        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
    }
}
