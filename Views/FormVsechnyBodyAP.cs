using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;

using LearActionPlans.Models;
using LearActionPlans.DataMappers;
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
        private string PopisProblemuFiltr;
        private string PricinaFiltr;

        public FormVsechnyBodyAP()
        {
            InitializeComponent();
            bindingSource = new BindingSource();
            dtBodyAP = new DataTable();
        }

        private void FormVsechnyBodyAP_Load(object sender, EventArgs e)
        {
            bindingSource.DataSource = dtBodyAP;
            DataGridViewBodyAP.DataSource = bindingSource;

            //tato část je kvůli urychlení načítání dat v DGV
            if (!System.Windows.Forms.SystemInformation.TerminalServerSession)
            {
                Type dgvType = DataGridViewBodyAP.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(DataGridViewBodyAP, true, null);
            }

            CreateColumns();

            ZobrazitDGV();
        }

        private void ZobrazitDGV()
        {
            var bodyAP_ = VsechnyBodyAPViewModel.GetBodyAPAll().ToList();

            foreach (var b in bodyAP_)
            {
                
                string cisloAPRok;
                cisloAPRok = Convert.ToInt32(b.CisloAP).ToString("D3") + " / " + Convert.ToDateTime(b.DatumZalozeniAP).Year;

                dtBodyAP.Rows.Add(new object[] { b.IdAP, b.CisloAP, cisloAPRok, b.DatumZalozeniAP, b.CisloBoduAP, b.OdkazNaNormu, b.HodnoceniNeshody, b.PopisProblemu, b.OdpovednaOsoba1, string.Empty, b.SkutecnaPricinaWM });

                dvBodyAP = dtBodyAP.DefaultView;
                //dvAP.RowFilter = string.Format("DatumZalozeniRok = {0}", DateTime.Now.Year);
                //dvBodyAP.RowFilter = string.Empty;
            }

            //nastaví filtry na string.empty
            InitFiltr();

            //naplní comboBoxy
            InitFiltryComboBox();

            //v comboBoxech nastaví vybrané položky
            NastavitVybranouPolozku();

            PridatHandlery();
            ObarvitLabel();
        }

        private void CreateColumns()
        {
            dtBodyAP.Columns.Add(new DataColumn("APId", typeof(int)));
            dtBodyAP.Columns.Add(new DataColumn("CisloAP", typeof(int)));
            dtBodyAP.Columns.Add(new DataColumn("CisloAPRok", typeof(string)));
            dtBodyAP.Columns.Add(new DataColumn("DatumZalozeni", typeof(DateTime)));

            var colBtn = new DataGridViewButtonColumn
            {
                Name = "ButtonCisloAPRok",
                HeaderText = @"AP number",
                Width = 100,
                DataPropertyName = "CisloAPRok",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            DataGridViewBodyAP.Columns.Add(colBtn);

            dtBodyAP.Columns.Add(new DataColumn("CisloBodAP", typeof(int)));
            dtBodyAP.Columns.Add(new DataColumn("TextBoxOdkazNaNormu", typeof(string)));
            dtBodyAP.Columns.Add(new DataColumn("TextBoxHodnoceniNeshody", typeof(string)));
            dtBodyAP.Columns.Add(new DataColumn("TextBoxPopisProblemu", typeof(string)));

            dtBodyAP.Columns.Add(new DataColumn("OdpovednyPracovnik1", typeof(string)));
            dtBodyAP.Columns.Add(new DataColumn("OdpovednyPracovnik2", typeof(string)));

            dtBodyAP.Columns.Add(new DataColumn("TextBoxSkutecnaPricina", typeof(string)));

            DataGridViewBodyAP.Columns["CisloBodAP"].HeaderText = @"AP point number";
            DataGridViewBodyAP.Columns["CisloBodAP"].Width = 120;

            DataGridViewBodyAP.Columns["TextBoxOdkazNaNormu"].HeaderText = @"Standard chapter";
            DataGridViewBodyAP.Columns["TextBoxOdkazNaNormu"].Width = 200;

            DataGridViewBodyAP.Columns["TextBoxHodnoceniNeshody"].HeaderText = @"Evaluation";
            DataGridViewBodyAP.Columns["TextBoxHodnoceniNeshody"].Width = 200;

            DataGridViewBodyAP.Columns["TextBoxPopisProblemu"].HeaderText = @"Description of the problem";
            DataGridViewBodyAP.Columns["TextBoxPopisProblemu"].Width = 200;

            DataGridViewBodyAP.Columns["OdpovednyPracovnik1"].HeaderText = @"Responsible #1";
            DataGridViewBodyAP.Columns["OdpovednyPracovnik1"].Width = 200;

            DataGridViewBodyAP.Columns["OdpovednyPracovnik2"].HeaderText = @"Responsible #2";
            DataGridViewBodyAP.Columns["OdpovednyPracovnik2"].Width = 200;

            DataGridViewBodyAP.Columns["TextBoxSkutecnaPricina"].HeaderText = @"Root cause";
            DataGridViewBodyAP.Columns["TextBoxSkutecnaPricina"].Width = 200;

            DataGridViewBodyAP.Columns["APId"].Visible = false;
            DataGridViewBodyAP.Columns["CisloAP"].Visible = false;
            DataGridViewBodyAP.Columns["CisloAPRok"].Visible = false;
            DataGridViewBodyAP.Columns["DatumZalozeni"].Visible = false;

            //DataGridViewCellStyle style = DataGridViewBodyAP.ColumnHeadersDefaultCellStyle;
            //style.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0F, GraphicsUnit.Pixel);

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

        private void DataGridViewBodyAP_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (e.ColumnIndex >= 0  && e.RowIndex >= 0)
            {
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                {
                    if (senderGrid.Columns[e.ColumnIndex].Name == "ButtonCisloAPRok")
                    {
                        akcniPlany = new FormNovyAkcniPlan.AkcniPlanTmp();

                        int idAP = Convert.ToInt32(dvBodyAP[DataGridViewBodyAP.CurrentCell.RowIndex]["APId"]);
                        akcniPlany.Id = idAP;

                        var ap = VsechnyBodyAPViewModel.GetSelectedAP(idAP).ToList();
                        var dtAP = DataTableConverter.ConvertToDataTable(ap);
                        var zad2 = PrehledAPViewModel.GetZadavatel2().ToList();

                        foreach (DataRow row in dtAP.Rows)
                        {
                            akcniPlany.Zadavatel1Jmeno = Convert.ToString(row["Zadavatel1"]);

                            string zadavatel2;
                            if (DatabaseReader.ConvertIntegerRow(row, "Zadavatel2Id") == null)
                            {
                                zadavatel2 = string.Empty;
                            }
                            else
                            {
                                int id = Convert.ToInt32(row["Zadavatel2Id"]);
                                var vyhledaneJmeno = zad2.Find(x => x.Zadavatel2Id == id);
                                zadavatel2 = vyhledaneJmeno.Zadavatel2;

                            }
                            akcniPlany.Zadavatel2Jmeno = zadavatel2;

                            akcniPlany.Tema = Convert.ToString(row["Tema"]);
                            if (row["ProjektId"] == null)
                            {
                                akcniPlany.ProjektNazev = null;
                            }
                            else
                            {
                                akcniPlany.ProjektNazev = Convert.ToString(row["Projekt"]);
                            }
                            akcniPlany.ZakaznikNazev = Convert.ToString(row["Zakaznik"]);

                        }

                        string cisloAPRok;
                        cisloAPRok = Convert.ToInt32(dvBodyAP[DataGridViewBodyAP.CurrentCell.RowIndex]["CisloAP"]).ToString("D3") + " / " + Convert.ToDateTime(dvBodyAP[DataGridViewBodyAP.CurrentCell.RowIndex]["DatumZalozeni"]).Year;
                        akcniPlany.CisloAPRok = cisloAPRok;
                        //tada se ověří uživatel
                        //volat FormOvereniUzivatele
                        using (var form = new FormPrehledBoduAP(true, akcniPlany, 2))
                        {
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
                        }
                    }
                }
            }
        }

        private void ButtonFiltrPopisProblemu_MouseClick(object sender, MouseEventArgs e)
        {
            //OdebratHandlery();
            if (TextBoxFiltrPopisProblemu.Text == string.Empty)
            {
                PopisProblemuFiltr = string.Empty;
            }
            else
            {
                PopisProblemuFiltr = TextBoxFiltrPopisProblemu.Text;
            }
            //if (ComboBoxOdpovedny1.SelectedIndex == 0)
            //{
            //    Odpovedny1Filtr = string.Empty;
            //}
            //else
            //{
            //    Odpovedny1Filtr = ComboBoxOdpovedny1.GetItemText(ComboBoxOdpovedny1.SelectedItem);
            //}
            FiltrovatData();

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

        private void InitFiltryComboBox()
        {
            FiltrOdpovedny1();
            //FiltrPopisProblemu();
        }

        public void FiltrovatData()
        {
            if (dvBodyAP.Count == 0)
            {
                InitFiltr();
                dvBodyAP.RowFilter = string.Empty;
            }
            else
            {
                dvBodyAP.RowFilter = string.Empty;
                if (!string.IsNullOrEmpty(Odpovedny1Filtr))
                {
                    dvBodyAP.RowFilter = string.Format("OdpovednyPracovnik1 = '{0}'", Odpovedny1Filtr);
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

        private void InitFiltr()
        {
            Odpovedny1Filtr = string.Empty;
            //PopisProblemuFiltr = string.Empty;
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void FiltrOdpovedny1()
        {
            DataTable dtOdpovedny = new DataTable();
            dtOdpovedny.Columns.Add("Odpovedny");

            DataRow radek = dtOdpovedny.NewRow();
            radek["Odpovedny"] = "(select all)";
            dtOdpovedny.Rows.Add(radek);

            for (int i = 0; i < dvBodyAP.Count; i++)
            {
                radek = dtOdpovedny.NewRow();
                bool contains = dtOdpovedny.AsEnumerable().Any(vyhledanyRadek => dvBodyAP[i]["OdpovednyPracovnik1"].ToString() == vyhledanyRadek.Field<string>("Odpovedny"));
                if (contains == false)
                {
                    radek["Odpovedny"] = dvBodyAP[i]["OdpovednyPracovnik1"];
                    dtOdpovedny.Rows.Add(radek);
                }
            }

            string sortColumn = dtOdpovedny.Columns[0].ColumnName;
            //setřídí položky podle abecedy
            dtOdpovedny.DefaultView.Sort = sortColumn;
            dtOdpovedny = dtOdpovedny.DefaultView.ToTable();

            ComboBoxOdpovedny1.DataSource = dtOdpovedny;
            ComboBoxOdpovedny1.DisplayMember = "Odpovedny";
        }

        private void ComboBoxOdpovedny1_SelectedIndexChanged(object sender, EventArgs e)
        {
            OdebratHandlery();

            if (ComboBoxOdpovedny1.SelectedIndex == 0)
            {
                Odpovedny1Filtr = string.Empty;
            }
            else
            {
                Odpovedny1Filtr = ComboBoxOdpovedny1.GetItemText(ComboBoxOdpovedny1.SelectedItem);
            }
            FiltrovatData();

            InitFiltryComboBox();

            NastavitVybranouPolozku();

            PridatHandlery();
            ObarvitLabel();
        }

        private void PridatHandlery()
        {
            ComboBoxOdpovedny1.SelectedIndexChanged += ComboBoxOdpovedny1_SelectedIndexChanged;
            //ComboBoxOdpovedny2.SelectedIndexChanged += ComboBoxOdpovedny2_SelectedIndexChanged;
        }

        private void OdebratHandlery()
        {
            ComboBoxOdpovedny1.SelectedIndexChanged -= ComboBoxOdpovedny1_SelectedIndexChanged;
            //ComboBoxOdpovedny2.SelectedIndexChanged -= ComboBoxOdpovedny2_SelectedIndexChanged;
        }

        private void NastavitVybranouPolozku()
        {
            if (Odpovedny1Filtr == string.Empty)
                ComboBoxOdpovedny1.SelectedIndex = 0;
            else
                ComboBoxOdpovedny1.SelectedIndex = ComboBoxOdpovedny1.FindStringExact(Odpovedny1Filtr);

            //if (Odpovedny2Filtr == string.Empty)
            //    ComboBoxOdpovedny2.SelectedIndex = 0;
            //else
            //    ComboBoxOdpovedny2.SelectedIndex = ComboBoxOdpovedny2.FindStringExact(Odpovedny2Filtr);
        }

        private void ObarvitLabel()
        {
            if (Odpovedny1Filtr == string.Empty)
                labelOdpovedny1.ForeColor = Color.Black;
            else
                labelOdpovedny1.ForeColor = Color.Blue;

            //if (Odpovedny2Filtr == string.Empty)
            //    labelOdpovedny2.ForeColor = Color.Black;
            //else
            //    labelOdpovedny2.ForeColor = Color.Blue;
        }
    }
}
