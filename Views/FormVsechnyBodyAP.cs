using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;

using LearActionPlans.ViewModels;
using LearActionPlans.Utilities;

namespace LearActionPlans.Views
{
    public partial class FormVsechnyBodyAP : Form
    {
        FormNovyAkcniPlan.AkcniPlanTmp akcniPlany;

        private readonly BindingSource bindingSource;
        private DataTable dtBodyAP;
        private DataView dvBodyAP;

        private string Odpovedny1Filtr;
        private string Odpovedny2Filtr;
        private string OddeleniFiltr;
        private string PopisProblemuFiltr;
        private string PricinaFiltr;

        public FormVsechnyBodyAP()
        {
            this.InitializeComponent();
            this.bindingSource = new BindingSource();
            this.dtBodyAP = new DataTable();
        }

        private void FormVsechnyBodyAP_Load(object sender, EventArgs e)
        {
            this.bindingSource.DataSource = this.dtBodyAP;
            this.DataGridViewBodyAP.DataSource = this.bindingSource;

            //tato část je kvůli urychlení načítání dat v DGV
            if (!SystemInformation.TerminalServerSession)
            {
                var dgvType = this.DataGridViewBodyAP.GetType();
                var pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(this.DataGridViewBodyAP, true, null);
            }

            this.CreateColumns();

            //nastaví filtry na string.empty
            this.InitFiltr();
            this.ZobrazitDGV(string.Empty);
        }

        private void ZobrazitDGV(string filtr)
        {
            var datumUkonceni_ = (DateTime?)null;
            var i = 0;
            //nejdřív odstraním všechny řádky z dtBody
            if (this.dtBodyAP.Rows.Count > 0)
            {
                for (var j = this.dtBodyAP.Rows.Count - 1; j >= 0; j--)
                {
                    this.dtBodyAP.Rows.Remove(this.dtBodyAP.Rows[j]);
                }

                this.dtBodyAP.Rows.Clear();
            }

            var bodyAP_ = VsechnyBodyAPViewModel.GetBodyAPAll().ToList();
            var odpOsoba2 = VsechnyBodyAPViewModel.GetOdpovednaOsoba2().ToList();

            foreach (var b in bodyAP_)
            {
                var datumUkonceni = PrehledBoduAPViewModel.GetUkonceniBodAP(b.IdBodAP).ToList();
                datumUkonceni.Reverse();
                datumUkonceni_ = null;
                foreach (var du in datumUkonceni)
                {
                    if (du.StavZadosti == 1 || du.StavZadosti == 4 || du.StavZadosti == 5)
                    {
                        datumUkonceni_ = du.DatumUkonceni;
                        break;
                    }
                }

                string cisloAPRok;
                cisloAPRok = Convert.ToInt32(b.CisloAP).ToString("D3") + " / " + Convert.ToDateTime(b.DatumZalozeniAP).Year;

                string odpovednaOsoba2;
                if (b.OdpovednaOsoba2Id == null)
                {
                    odpovednaOsoba2 = string.Empty;
                }
                else
                {
                    var id = Convert.ToInt32(b.OdpovednaOsoba2Id);
                    var vyhledaneJmeno = odpOsoba2.Find(x => x.OdpovednaOsoba2Id == id);
                    odpovednaOsoba2 = vyhledaneJmeno.OdpovednaOsoba2;
                }
                
                this.dtBodyAP.Rows.Add(new object[] { b.IdBodAP, b.IdAP, b.CisloAP, cisloAPRok, b.DatumZalozeniAP, b.CisloBoduAP,
                    b.OdkazNaNormu, b.HodnoceniNeshody, b.PopisProblemu, b.OdpovednaOsoba1, odpovednaOsoba2, b.NazevOddeleni,
                    b.SkutecnaPricinaWM, b.NapravnaOpatreniWM, b.SkutecnaPricinaWS, b.NapravnaOpatreniWS,
                    datumUkonceni_ == null
                        ? string.Empty
                        : Convert.ToDateTime(datumUkonceni_).ToShortDateString(),
                    b.KontrolaEfektivnosti == null
                        ? string.Empty
                        : Convert.ToDateTime(b.KontrolaEfektivnosti).ToShortDateString(),
                    b.ZnovuOtevrit});

                this.dvBodyAP = this.dtBodyAP.DefaultView;
                //dvAP.RowFilter = string.Format("DatumZalozeniRok = {0}", DateTime.Now.Year);
            }

            this.dvBodyAP.RowFilter = filtr;

            // odebere handlery
            this.OdebratHandlery();

            //naplní comboBoxy
            this.InitFiltryComboBox();

            //v comboBoxech nastaví vybrané položky
            this.NastavitVybranouPolozku();

            this.PridatHandlery();
            this.ObarvitLabel();

            // nastaví filtr pro dataview
            //this.dvBodyAP.RowFilter = filtr;

            i = 0;
            foreach (DataRowView rowView in this.dvBodyAP)
            {
                var row = rowView.Row;
                if (Convert.ToString(row["Efektivita"]) != string.Empty)
                {
                    this.DataGridViewBodyAP.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                i++;
            }
            this.DataGridViewBodyAP.Refresh();
        }

        private void CreateColumns()
        {
            this.dtBodyAP.Columns.Add(new DataColumn("BodAPId", typeof(int)));
            this.dtBodyAP.Columns.Add(new DataColumn("APId", typeof(int)));
            this.dtBodyAP.Columns.Add(new DataColumn("CisloAP", typeof(int)));
            this.dtBodyAP.Columns.Add(new DataColumn("CisloAPRok", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("DatumZalozeni", typeof(DateTime)));

            var colBtn = new DataGridViewButtonColumn
            {
                Name = "ButtonCisloAPRok",
                HeaderText = @"AP number",
                Width = 100,
                DataPropertyName = "CisloAPRok",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            this.DataGridViewBodyAP.Columns.Add(colBtn);

            this.dtBodyAP.Columns.Add(new DataColumn("CisloBodAP", typeof(int)));
            this.dtBodyAP.Columns.Add(new DataColumn("TextBoxOdkazNaNormu", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("TextBoxHodnoceniNeshody", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("TextBoxPopisProblemu", typeof(string)));

            this.dtBodyAP.Columns.Add(new DataColumn("OdpovednyPracovnik1", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("OdpovednyPracovnik2", typeof(string)));

            this.dtBodyAP.Columns.Add(new DataColumn("Oddeleni", typeof(string)));

            this.dtBodyAP.Columns.Add(new DataColumn("WMPricina", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("WMOpatreni", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("WSPricina", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("WSOpatreni", typeof(string)));

            this.dtBodyAP.Columns.Add(new DataColumn("DatumUkonceni", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("Efektivita", typeof(string)));

            this.dtBodyAP.Columns.Add(new DataColumn("Reopen", typeof(bool)));

            this.DataGridViewBodyAP.Columns["CisloBodAP"].HeaderText = @"AP point number";
            this.DataGridViewBodyAP.Columns["CisloBodAP"].Width = 120;

            this.DataGridViewBodyAP.Columns["TextBoxOdkazNaNormu"].HeaderText = @"Standard chapter";
            this.DataGridViewBodyAP.Columns["TextBoxOdkazNaNormu"].Width = 140;

            this.DataGridViewBodyAP.Columns["TextBoxHodnoceniNeshody"].HeaderText = @"Evaluation";
            this.DataGridViewBodyAP.Columns["TextBoxHodnoceniNeshody"].Width = 120;

            this.DataGridViewBodyAP.Columns["TextBoxPopisProblemu"].HeaderText = @"Description of the problem";
            this.DataGridViewBodyAP.Columns["TextBoxPopisProblemu"].Width = 200;

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

            this.DataGridViewBodyAP.Columns["Reopen"].HeaderText = @"After reopen";
            this.DataGridViewBodyAP.Columns["Reopen"].Width = 110;

            this.DataGridViewBodyAP.Columns["BodAPId"].Visible = false;
            this.DataGridViewBodyAP.Columns["APId"].Visible = false;
            this.DataGridViewBodyAP.Columns["CisloAP"].Visible = false;
            this.DataGridViewBodyAP.Columns["CisloAPRok"].Visible = false;
            this.DataGridViewBodyAP.Columns["DatumZalozeni"].Visible = false;

            //DataGridViewCellStyle style = DataGridViewBodyAP.ColumnHeadersDefaultCellStyle;
            //style.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0F, GraphicsUnit.Pixel);

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

            //DataGridViewBodyAP.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            //DataGridViewBodyAP.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
        }

        private void DataGridViewBodyAP_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (e.ColumnIndex >= 0  && e.RowIndex >= 0)
            {
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                {
                    if (senderGrid.Columns[e.ColumnIndex].Name == "ButtonCisloAPRok")
                    {
                        this.akcniPlany = new FormNovyAkcniPlan.AkcniPlanTmp();

                        var idAP = Convert.ToInt32(this.dvBodyAP[this.DataGridViewBodyAP.CurrentCell.RowIndex]["APId"]);
                        this.akcniPlany.Id = idAP;

                        var ap = VsechnyBodyAPViewModel.GetSelectedAP(idAP).ToList();
                        var dtAP = DataTableConverter.ConvertToDataTable(ap);
                        var zad2 = PrehledAPViewModel.GetZadavatel2().ToList();

                        foreach (DataRow row in dtAP.Rows)
                        {
                            this.akcniPlany.Zadavatel1Jmeno = Convert.ToString(row["Zadavatel1"]);

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

                            this.akcniPlany.Zadavatel2Jmeno = zadavatel2;

                            this.akcniPlany.Tema = Convert.ToString(row["Tema"]);
                            if (row["ProjektId"] == null)
                            {
                                this.akcniPlany.ProjektNazev = null;
                            }
                            else
                            {
                                this.akcniPlany.ProjektNazev = Convert.ToString(row["Projekt"]);
                            }

                            this.akcniPlany.ZakaznikNazev = Convert.ToString(row["Zakaznik"]);

                        }

                        string cisloAPRok;
                        cisloAPRok = Convert.ToInt32(this.dvBodyAP[this.DataGridViewBodyAP.CurrentCell.RowIndex]["CisloAP"]).ToString("D3") + " / " + Convert.ToDateTime(this.dvBodyAP[this.DataGridViewBodyAP.CurrentCell.RowIndex]["DatumZalozeni"]).Year;
                        this.akcniPlany.CisloAPRok = cisloAPRok;
                        using var form = new FormPrehledBoduAP(true, this.akcniPlany, 2,
                            Convert.ToInt32(this.dvBodyAP[this.DataGridViewBodyAP.CurrentCell.RowIndex]["BodAPId"]));
                        var result = form.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            //string dateString = form.ReturnValueDatum;
                            //string poznamka = form.ReturnValuePoznamka;

                            //dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxDatumUkonceni"] = Convert.ToDateTime(dateString);
                            //changedDGV = true;
                            //podminkaPoznamka = poznamka == null;
                            //dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxPoznamka"] = podminkaPoznamka ? null : Convert.ToString(poznamka);
                        }

                        // zapamatuje si nastavení filtrů
                        var filtr = this.dvBodyAP.RowFilter;
                        //var filtrOdpovedny1 = this.Odpovedny1Filtr;
                        //var filtrOdpovedny2 = this.Odpovedny2Filtr;
                        //var filtrOddeleni = this.OddeleniFiltr;

                        this.ZobrazitDGV(filtr);

                        // znou nastaví filtry
                        //this.Odpovedny1Filtr = filtrOdpovedny1;
                        //this.Odpovedny2Filtr = filtrOdpovedny2;
                        //this.OddeleniFiltr = filtrOddeleni;
                    }
                }
            }
        }

        private void ButtonFiltrPopisProblemu_MouseClick(object sender, MouseEventArgs e)
        {
            //OdebratHandlery();
            if (this.TextBoxFiltrPopisProblemu.Text == string.Empty)
            {
                this.PopisProblemuFiltr = string.Empty;
            }
            else
            {
                this.PopisProblemuFiltr = this.TextBoxFiltrPopisProblemu.Text;
            }
            //if (ComboBoxOdpovedny1.SelectedIndex == 0)
            //{
            //    Odpovedny1Filtr = string.Empty;
            //}
            //else
            //{
            //    Odpovedny1Filtr = ComboBoxOdpovedny1.GetItemText(ComboBoxOdpovedny1.SelectedItem);
            //}
            this.FiltrovatData();

            //InitFiltryComboBox();

            //NastavitVybranouPolozku();

            //PridatHandlery();
            //ObarvitLabel();
        }


        private void ButtonFiltrPricina_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void FiltrPopisProblemu()
        {

        }

        private void InitFiltr()
        {
            this.Odpovedny1Filtr = string.Empty;
            this.Odpovedny2Filtr = string.Empty;
            this.OddeleniFiltr = string.Empty;
            //PopisProblemuFiltr = string.Empty;
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e) => this.Close();

        private void FiltrOdpovedny1()
        {
            var dtOdpovedny = new DataTable();
            dtOdpovedny.Columns.Add("Odpovedny");

            var radek = dtOdpovedny.NewRow();
            radek["Odpovedny"] = "(select all)";
            dtOdpovedny.Rows.Add(radek);

            for (var i = 0; i < this.dvBodyAP.Count; i++)
            {
                radek = dtOdpovedny.NewRow();
                var contains = dtOdpovedny.AsEnumerable().Any(vyhledanyRadek => this.dvBodyAP[i]["OdpovednyPracovnik1"].ToString() == vyhledanyRadek.Field<string>("Odpovedny"));
                if (contains == false)
                {
                    radek["Odpovedny"] = this.dvBodyAP[i]["OdpovednyPracovnik1"];
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
                this.Odpovedny1Filtr = string.Empty;
            }
            else
            {
                this.Odpovedny1Filtr = this.ComboBoxOdpovedny1.GetItemText(this.ComboBoxOdpovedny1.SelectedItem);
            }

            this.SetFilter();
        }

        private void FiltrOdpovedny2()
        {
            var dtOdpovedny = new DataTable();
            dtOdpovedny.Columns.Add("Odpovedny");

            var radek = dtOdpovedny.NewRow();
            radek["Odpovedny"] = "(select all)";
            dtOdpovedny.Rows.Add(radek);

            for (var i = 0; i < this.dvBodyAP.Count; i++)
            {
                if (this.dvBodyAP[i]["OdpovednyPracovnik2"].ToString() == string.Empty) { }
                else
                {
                    radek = dtOdpovedny.NewRow();
                    var contains = dtOdpovedny.AsEnumerable().Any(vyhledanyRadek => this.dvBodyAP[i]["OdpovednyPracovnik2"].ToString() == vyhledanyRadek.Field<string>("Odpovedny"));
                    if (contains == false)
                    {
                        radek["Odpovedny"] = this.dvBodyAP[i]["OdpovednyPracovnik2"];
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
                this.Odpovedny2Filtr = string.Empty;
            }
            else
            {
                this.Odpovedny2Filtr = this.ComboBoxOdpovedny2.GetItemText(this.ComboBoxOdpovedny2.SelectedItem);
            }

            this.SetFilter();
        }

        private void FiltrOddeleni()
        {
            var dtOddeleni = new DataTable();
            dtOddeleni.Columns.Add("Oddeleni");

            var radek = dtOddeleni.NewRow();
            radek["Oddeleni"] = "(select all)";
            dtOddeleni.Rows.Add(radek);

            for (var i = 0; i < this.dvBodyAP.Count; i++)
            {
                radek = dtOddeleni.NewRow();
                var contains = dtOddeleni.AsEnumerable().Any(vyhledanyRadek => this.dvBodyAP[i]["Oddeleni"].ToString() == vyhledanyRadek.Field<string>("Oddeleni"));
                if (contains == false)
                {
                    radek["Oddeleni"] = this.dvBodyAP[i]["Oddeleni"];
                    dtOddeleni.Rows.Add(radek);
                }
            }

            var sortColumn = dtOddeleni.Columns[0].ColumnName;
            //setřídí položky podle abecedy
            dtOddeleni.DefaultView.Sort = sortColumn;
            dtOddeleni = dtOddeleni.DefaultView.ToTable();

            this.ComboBoxOddeleni.DataSource = dtOddeleni;
            this.ComboBoxOddeleni.DisplayMember = "Oddeleni";
        }

        private void ComboBoxOddeleni_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OdebratHandlery();

            if (this.ComboBoxOddeleni.SelectedIndex == 0)
            {
                this.OddeleniFiltr = string.Empty;
            }
            else
            {
                this.OddeleniFiltr = this.ComboBoxOddeleni.GetItemText(this.ComboBoxOddeleni.SelectedItem);
            }

            this.SetFilter();
        }

        private void SetFilter()
        {
            this.FiltrovatData();

            this.InitFiltryComboBox();

            this.NastavitVybranouPolozku();

            this.PridatHandlery();
            this.ObarvitLabel();

            var i = 0;
            foreach (DataRowView rowView in this.dvBodyAP)
            {
                var row = rowView.Row;
                if (Convert.ToString(row["Efektivita"]) != string.Empty)
                {
                    this.DataGridViewBodyAP.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                i++;
            }
            this.DataGridViewBodyAP.Refresh();
        }

        public void FiltrovatData()
        {
            if (this.dvBodyAP.Count == 0)
            {
                this.InitFiltr();
                this.dvBodyAP.RowFilter = string.Empty;
            }
            else
            {
                this.dvBodyAP.RowFilter = string.Empty;
                if (!string.IsNullOrEmpty(this.Odpovedny1Filtr))
                {
                    this.dvBodyAP.RowFilter = string.Format("OdpovednyPracovnik1 = '{0}'", this.Odpovedny1Filtr);
                }

                if (!string.IsNullOrEmpty(this.Odpovedny2Filtr))
                {
                    if (this.dvBodyAP.RowFilter == string.Empty)
                    {
                        this.dvBodyAP.RowFilter = string.Format("OdpovednyPracovnik2 = '{0}'", this.Odpovedny2Filtr);
                        //dvBodyAP.RowFilter = str;
                    }
                    else
                    {
                        this.dvBodyAP.RowFilter += string.Format(" AND OdpovednyPracovnik2 = '{0}'", this.Odpovedny2Filtr);
                    }
                }

                if (!string.IsNullOrEmpty(this.OddeleniFiltr))
                {
                    if (this.dvBodyAP.RowFilter == string.Empty)
                    {
                        this.dvBodyAP.RowFilter = string.Format("Oddeleni = '{0}'", this.OddeleniFiltr);
                        //dvBodyAP.RowFilter = str;
                    }
                    else
                    {
                        this.dvBodyAP.RowFilter += string.Format(" AND Oddeleni = '{0}'", this.OddeleniFiltr);
                    }
                }

                //if (!string.IsNullOrEmpty(TextBoxFiltrPopisProblemu.Text))
                //{
                //    dvBodyAP.RowFilter = string.Format("TextBoxPopisProblemu LIKE '%{0}%'", PopisProblemuFiltr);
                //    //dvBodyAP.RowFilter = str;
                //}
                //else
                //{
                //    dvBodyAP.RowFilter += string.Format(" AND TextBoxPopisProblemu LIKE '%{0}%'", PopisProblemuFiltr);
                //}

            }
        }

        private void InitFiltryComboBox()
        {
            this.FiltrOdpovedny1();
            this.FiltrOdpovedny2();
            this.FiltrOddeleni();
            //FiltrPopisProblemu();
        }

        private void NastavitVybranouPolozku()
        {
            if (this.Odpovedny1Filtr == string.Empty)
            {
                this.ComboBoxOdpovedny1.SelectedIndex = 0;
            }
            else
            {
                this.ComboBoxOdpovedny1.SelectedIndex = this.ComboBoxOdpovedny1.FindStringExact(this.Odpovedny1Filtr);
            }

            if (this.Odpovedny2Filtr == string.Empty)
            {
                this.ComboBoxOdpovedny2.SelectedIndex = 0;
            }
            else
            {
                this.ComboBoxOdpovedny2.SelectedIndex = this.ComboBoxOdpovedny2.FindStringExact(this.Odpovedny2Filtr);
            }

            if (this.OddeleniFiltr == string.Empty)
            {
                this.ComboBoxOddeleni.SelectedIndex = 0;
            }
            else
            {
                this.ComboBoxOddeleni.SelectedIndex = this.ComboBoxOddeleni.FindStringExact(this.OddeleniFiltr);
            }
        }

        private void PridatHandlery()
        {
            this.ComboBoxOdpovedny1.SelectedIndexChanged += this.ComboBoxOdpovedny1_SelectedIndexChanged;
            this.ComboBoxOdpovedny2.SelectedIndexChanged += this.ComboBoxOdpovedny2_SelectedIndexChanged;
            this.ComboBoxOddeleni.SelectedIndexChanged += this.ComboBoxOddeleni_SelectedIndexChanged;
            //ComboBoxOdpovedny2.SelectedIndexChanged += ComboBoxOdpovedny2_SelectedIndexChanged;
        }

        private void OdebratHandlery()
        {
            this.ComboBoxOdpovedny1.SelectedIndexChanged -= this.ComboBoxOdpovedny1_SelectedIndexChanged;
            this.ComboBoxOdpovedny2.SelectedIndexChanged -= this.ComboBoxOdpovedny2_SelectedIndexChanged;
            this.ComboBoxOddeleni.SelectedIndexChanged -= this.ComboBoxOddeleni_SelectedIndexChanged;
            //ComboBoxOdpovedny2.SelectedIndexChanged -= ComboBoxOdpovedny2_SelectedIndexChanged;
        }

        private void ObarvitLabel()
        {
            if (this.Odpovedny1Filtr == string.Empty)
            {
                this.labelOdpovedny1.ForeColor = Color.Black;
            }
            else
            {
                this.labelOdpovedny1.ForeColor = Color.Blue;
            }

            if (this.Odpovedny2Filtr == string.Empty)
            {
                this.labelOdpovedny2.ForeColor = Color.Black;
            }
            else
            {
                this.labelOdpovedny2.ForeColor = Color.Blue;
            }

            if (this.OddeleniFiltr == string.Empty)
            {
                this.labelOddeleni.ForeColor = Color.Black;
            }
            else
            {
                this.labelOddeleni.ForeColor = Color.Blue;
            }
            //if (Odpovedny2Filtr == string.Empty)
            //    labelOdpovedny2.ForeColor = Color.Black;
            //else
            //    labelOdpovedny2.ForeColor = Color.Blue;
        }
    }
}
