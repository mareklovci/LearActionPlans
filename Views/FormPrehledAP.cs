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

        FormNovyAkcniPlan.AkcniPlanTmp akcniPlany;

        private string Odpovedny1Filtr;
        private string Odpovedny2Filtr;
        private string ProjektyFiltr;
        private string TypAPFiltr;
        private string RokZalozeniFiltr;
        private string OtevreneUzavreneFiltr;

        private readonly bool spusteniBezParamatru_;
        private readonly string[] args_;

        private string cisloAPStr;
        private readonly int apId;
        private readonly int bodAPId;
        private readonly int akceId;
        private readonly int ukonceniAkceId;
        private readonly int vlastnikAkceId;

        private readonly string[] values;

        ////proměnné pro přihlášení uživatele
        //private bool uzivatelPrihlasen;
        //private bool vlastnikAP;
        //private bool vlastnikAkce;
        //private int vlastnikIdAkce;
        //private int iDLoginUser;
        //private bool uzivatelOvereny;

        public FormPrehledAP(bool spusteniBezParametru, string[] args)
        {
            InitializeComponent();
            spusteniBezParamatru_ = spusteniBezParametru;
            args_ = (string[])args.Clone();

            values = null;

            cisloAPStr = string.Empty;
            apId = 0;
            bodAPId = 0;
            akceId = 0;
            ukonceniAkceId = 0;
            vlastnikAkceId = 0;

            //uzivatelPrihlasen = false;
            //vlastnikAP = false;
            //vlastnikAkce = false;
            //vlastnikIdAkce = 0;
            ////to je proto, aby neplatilo, že 0 == 0
            //iDLoginUser = -1;
            //uzivatelOvereny = false;

            if (spusteniBezParametru == false)
            {
                string[] param = args_[1].Split('?');
                values = param[1].Split('&');

                int i = 0;
                foreach (var v in values)
                {
                    if (i == 0)
                    {
                        cisloAPStr = Convert.ToString(v);
                        cisloAPStr = cisloAPStr.Replace("%20", " ");
                    }
                    if (i == 1)
                    {
                        apId = Convert.ToInt32(v);
                    }
                    if (i == 2)
                    {
                        bodAPId = Convert.ToInt32(v);
                    }
                    //if (i == 3)
                    //{
                    //    akceId = Convert.ToInt32(v);
                    //}
                    if (i == 3)
                    {
                        ukonceniAkceId = Convert.ToInt32(v);
                    }
                    if (i == 4)
                    {
                        vlastnikAkceId = Convert.ToInt32(v);
                    }
                    i++;
                }
            }

            bindingSourceAP = new BindingSource();
            dtAP = new DataTable();

            akcniPlany = new FormNovyAkcniPlan.AkcniPlanTmp();
        }

        private void FormPrehledAP_Load(object sender, EventArgs e)
        {
            bindingSourceAP.DataSource = dtAP;
            DataGridViewAP.DataSource = bindingSourceAP;

            //urychlení načítání dat v DGV
            if (!System.Windows.Forms.SystemInformation.TerminalServerSession)
            {
                Type dgvType = DataGridViewAP.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(DataGridViewAP, true, null);
            }

            CreateColumnsAP();
            if (ZobrazitDGV() == true)
            {
                //nastaví filtry na string.empty
                InitFiltr();

                //naplní comboBoxy
                InitFiltryComboBox();

                //v comboBoxech nastaví vybrané položky
                NastavitVybranouPolozku();

                PridatHandlery();
                ObarvitLabel();

                ComboBoxRoky.Enabled = true;
                ComboBoxProjekty.Enabled = true;
                ComboBoxOdpovedny1.Enabled = true;
                ComboBoxOdpovedny2.Enabled = true;
                ComboBoxTypAP.Enabled = true;
                ComboBoxOtevreneUzavrene.Enabled = true;
                ButtonEditAP.Enabled = true; ;
            }
            else
            {
                ComboBoxRoky.Enabled = false;
                ComboBoxProjekty.Enabled = false;
                ComboBoxOdpovedny1.Enabled = false;
                ComboBoxOdpovedny2.Enabled = false;
                ComboBoxTypAP.Enabled = false;
                ComboBoxOtevreneUzavrene.Enabled = false;
                ButtonEditAP.Enabled = false;
            }

            //tato část bude spuštěna z emailu
            if (spusteniBezParamatru_ == false)
            {
                int idAP = Convert.ToInt32(apId);
                akcniPlany.Id = idAP;
                string cisloAPRok;
                //cisloAPRok = Convert.ToInt32(dvAP[DataGridViewAP.CurrentCell.RowIndex]["CisloAP"]).ToString("D3") + " / " + Convert.ToDateTime(dvAP[DataGridViewAP.CurrentCell.RowIndex]["DatumZalozeni"]).Year;
                cisloAPRok = cisloAPStr;
                akcniPlany.CisloAPRok = cisloAPRok;

                using (var form = new FormPrehledBoduAP(false, akcniPlany, 2))
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

        private void CreateColumnsAP()
        {
            dtAP.Columns.Add(new DataColumn("APId", typeof(int)));
            dtAP.Columns.Add(new DataColumn("CisloAP", typeof(int)));
            dtAP.Columns.Add(new DataColumn("CisloAPRok", typeof(string)));

            var colBtn = new DataGridViewButtonColumn
            {
                Name = "ButtonCisloAPRok",
                HeaderText = @"AP number",
                Width = 100,
                DataPropertyName = "CisloAPRok",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            DataGridViewAP.Columns.Add(colBtn);

            dtAP.Columns.Add(new DataColumn("DatumZalozeni", typeof(DateTime)));
            dtAP.Columns.Add(new DataColumn("DatumZalozeniRok", typeof(int)));
            dtAP.Columns.Add(new DataColumn("Zadavatel1Id", typeof(int)));
            dtAP.Columns.Add(new DataColumn("Zadavatel2Id", typeof(int)));
            dtAP.Columns.Add(new DataColumn("Zadavatel1Jmeno", typeof(string)));
            dtAP.Columns.Add(new DataColumn("Zadavatel2Jmeno", typeof(string)));
            dtAP.Columns.Add(new DataColumn("Tema", typeof(string)));
            dtAP.Columns.Add(new DataColumn("ProjektId", typeof(int)));
            dtAP.Columns.Add(new DataColumn("ProjektNazev", typeof(string)));
            dtAP.Columns.Add(new DataColumn("ZakaznikId", typeof(int)));
            dtAP.Columns.Add(new DataColumn("ZakaznikNazev", typeof(string)));
            dtAP.Columns.Add(new DataColumn("TypAP", typeof(byte)));
            dtAP.Columns.Add(new DataColumn("TypAPNazev", typeof(string)));
            // asi možná nebudu potřebovat
            dtAP.Columns.Add(new DataColumn("StavObjektu", typeof(byte)));

            DataGridViewAP.Columns["APId"].Visible = false;
            DataGridViewAP.Columns["CisloAP"].Visible = false;
            DataGridViewAP.Columns["CisloAPRok"].Visible = false;
            DataGridViewAP.Columns["DatumZalozeniRok"].Visible = false;
            DataGridViewAP.Columns["Zadavatel1Id"].Visible = false;
            DataGridViewAP.Columns["Zadavatel2Id"].Visible = false;
            DataGridViewAP.Columns["ZakaznikId"].Visible = false;
            DataGridViewAP.Columns["ProjektId"].Visible = false;
            DataGridViewAP.Columns["TypAP"].Visible = false;
            DataGridViewAP.Columns["StavObjektu"].Visible = false;

            DataGridViewAP.Columns["DatumZalozeni"].HeaderText = @"Created Date";
            DataGridViewAP.Columns["DatumZalozeni"].Width = 120;
            DataGridViewAP.Columns["DatumZalozeni"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            DataGridViewAP.Columns["Zadavatel1Jmeno"].HeaderText = @"Responsible #1";
            DataGridViewAP.Columns["Zadavatel1Jmeno"].Width = 200;
            DataGridViewAP.Columns["Zadavatel1Jmeno"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            DataGridViewAP.Columns["Zadavatel2Jmeno"].HeaderText = @"Responsible #2";
            DataGridViewAP.Columns["Zadavatel2Jmeno"].Width = 200;
            DataGridViewAP.Columns["Zadavatel2Jmeno"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            DataGridViewAP.Columns["Tema"].HeaderText = @"Topic";
            DataGridViewAP.Columns["Tema"].Width = 120;
            DataGridViewAP.Columns["Tema"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            DataGridViewAP.Columns["ZakaznikNazev"].HeaderText = @"Customer Name";
            DataGridViewAP.Columns["ZakaznikNazev"].Width = 150;
            DataGridViewAP.Columns["ZakaznikNazev"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            DataGridViewAP.Columns["ProjektNazev"].HeaderText = @"Project Name";
            DataGridViewAP.Columns["ProjektNazev"].Width = 120;
            DataGridViewAP.Columns["ProjektNazev"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            DataGridViewAP.Columns["TypAPNazev"].HeaderText = @"Type AP";
            DataGridViewAP.Columns["TypAPNazev"].Width = 120;
            DataGridViewAP.Columns["TypAPNazev"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dtAP.Columns.Add(new DataColumn("OtevrenoUzavreno", typeof(string)));
            DataGridViewAP.Columns["OtevrenoUzavreno"].Visible = false;
            var textBox = new DataGridViewTextBoxColumn
            {
                Name = "TextBoxStavOtevrenoUzavreno",
                HeaderText = @"Open / Closed",
                Width = 120,
                DataPropertyName = "OtevrenoUzavreno",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            DataGridViewAP.Columns.Add(textBox);

            DataGridViewCellStyle style = DataGridViewAP.ColumnHeadersDefaultCellStyle;
            style.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0F, GraphicsUnit.Pixel);

            DataGridViewAP.MultiSelect = false;
            DataGridViewAP.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataGridViewAP.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            DataGridViewAP.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            DataGridViewAP.RowHeadersVisible = true;
            DataGridViewAP.AllowUserToResizeRows = false;
            DataGridViewAP.AllowUserToResizeColumns = true;
            DataGridViewAP.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            DataGridViewAP.AllowUserToAddRows = false;
            DataGridViewAP.AllowUserToResizeRows = false;
            DataGridViewAP.ReadOnly = true;

            //Enable column edit
            foreach (DataGridViewColumn column in DataGridViewAP.Columns)
            {
                column.ReadOnly = true;
                //column.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0F, GraphicsUnit.Pixel);
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            DataGridViewAP.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            DataGridViewAP.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            DataGridViewAP.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
        }

        private bool ZobrazitDGV()
        {
            bool zobrazitZaznamy = false;

            var ap = PrehledAPViewModel.GetAPAll().ToList();
            var zad2 = PrehledAPViewModel.GetZadavatel2().ToList();

            //pokud počet záznamů bude roven 0, namohu naplnit filtry
            if (ap.Count > 0)
            {
                zobrazitZaznamy = true;
                var dt = DataTableConverter.ConvertToDataTable(ap);
                dt.DefaultView.Sort = "DatumZalozeni asc";

                dtAP.Rows.Clear();

                foreach (DataRow row in dt.Rows)
                {

                    string cisloAPRok;
                    cisloAPRok = Convert.ToInt32(row["CisloAP"]).ToString("D3") + " / " + Convert.ToDateTime(row["DatumZalozeni"]).Year;
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

                    string typAP = string.Empty;
                    if (Convert.ToInt32(row["TypAP"]) == 1)
                    {
                        typAP = "Audits";
                    }
                    if (Convert.ToInt32(row["TypAP"]) == 2)
                    {
                        typAP = "Other";
                    }
                    int rok = Convert.ToDateTime(row["DatumZalozeni"]).Year;

                    string otevrenoUzavreno = string.Empty;
                    if (DatabaseReader.ConvertDateTimeRow(row, "DatumUzavreni") == null)
                    {
                        otevrenoUzavreno = "Open";
                    }
                    else
                    {
                        otevrenoUzavreno = "Closed";
                    }

                    dtAP.Rows.Add(new object[] { row["Id"],
                            row["CisloAP"],
                            cisloAPRok,
                            row["DatumZalozeni"],
                            rok,
                            row["Zadavatel1Id"],
                            row["Zadavatel2Id"],
                            row["Zadavatel1"],
                            zadavatel2,
                            row["Tema"],
                            row["ProjektId"],
                            row["Projekt"],
                            row["ZakaznikId"],
                            row["Zakaznik"],
                            row["TypAP"],
                            typAP,
                            row["StavObjektu"],
                            otevrenoUzavreno
                            });
                }
                dvAP = dtAP.DefaultView;
                //dvAP.RowFilter = string.Format("DatumZalozeniRok = {0}", DateTime.Now.Year);
                //dvAP.RowFilter = string.Empty;
                //dvAP.Sort = "CisloAP";
            }

            return zobrazitZaznamy;
        }

        private void DataGridViewAP_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                int columnIndex = e.ColumnIndex;
                string columnName = DataGridViewAP.Columns[columnIndex].Name;
                if (columnName == "TextBoxStavOtevrenoUzavreno")
                {
                    if (Convert.ToString(DataGridViewAP.Rows[e.RowIndex].Cells["OtevrenoUzavreno"].Value) == "Open")
                    {
                        e.CellStyle.BackColor = Color.LightGreen;
                    }
                    else if (Convert.ToString(DataGridViewAP.Rows[e.RowIndex].Cells["OtevrenoUzavreno"].Value) == "Closed")
                    {
                        e.CellStyle.BackColor = Color.LightPink;
                    }
                }
            }
        }

        private void DataGridViewAP_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            //bool vlastniPrihlasen;

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                {
                    if (senderGrid.Columns[e.ColumnIndex].Name == "ButtonCisloAPRok")
                    {
                        //List<int> zadavatele = new List<int>();
                        //var bodyAP = PrehledAPViewModel.GetBodyAPId(Convert.ToInt32(dvAP[DataGridViewAP.CurrentCell.RowIndex]["APId"])).ToList();

                        //foreach (var bodAP in bodyAP)
                        //{
                        //    var akceBodu = PrehledAPViewModel.GetAkceBodId(bodAP.BodAPId);
                        //    foreach (var akce in akceBodu)
                        //    {
                        //        zadavatele.Add(akce.Zadavatel1IdAkce);
                        //        if (akce.Zadavatel2IdAkce != null)
                        //            zadavatele.Add(Convert.ToInt32(akce.Zadavatel2IdAkce));
                        //    }
                        //}
                        //List<int> zadavateleAkci = zadavatele.Distinct().ToList();

                        //nejdřív ověřím uživatele
                        //pokud bude zvolena varianta Anyone, bude vše pouze pro čtení
                        //int zadavatel1AP = Convert.ToInt32(dvAP[e.RowIndex]["zadavatel1Id"]);
                        //int zadavatel2AP;
                        //if (DatabaseReader.ConvertIntegerRow(dvAP[e.RowIndex].Row, "Zadavatel2Id") == null)
                        //    zadavatel2AP = 0;
                        //else
                        //    zadavatel2AP = Convert.ToInt32(dvAP[e.RowIndex]["zadavatel2Id"]);

                        //bool majitelAP = false;
                        //bool majitelAkce = false;
                        //přihlášený uživatel je majitel AP

                        //if ((FormMain.IDLoginUser == zadavatel1AP || FormMain.IDLoginUser == zadavatel2AP) && FormMain.UzivatelOvereny == true)
                        //{
                        //    //majitelAP = true;
                        //    FormMain.VlastnikAP = true;
                        //}
                        //else
                        //{
                        //    //pokud majitel AP bude zároveň majitel akce, tak do této části už je zbytečné se dostat
                        //    //přihlášený uživatel je majitel akce
                        //    if (zadavateleAkci.Contains(FormMain.IDLoginUser) && FormMain.UzivatelOvereny == true)
                        //    {
                        //        //majitelAkce = true;
                        //        FormMain.VlastnikAkce = true;
                        //        FormMain.VlastnikIdAkce = FormMain.IDLoginUser;
                        //    }
                        //}

                        //bez přihlášení se otevře přehled bodů ReadOnly
                        int idAP = Convert.ToInt32(dvAP[DataGridViewAP.CurrentCell.RowIndex]["APId"]);
                        akcniPlany.Id = idAP;
                        string cisloAPRok;
                        cisloAPRok = Convert.ToInt32(dvAP[DataGridViewAP.CurrentCell.RowIndex]["CisloAP"]).ToString("D3") + " / " + Convert.ToDateTime(dvAP[DataGridViewAP.CurrentCell.RowIndex]["DatumZalozeni"]).Year;
                        akcniPlany.CisloAPRok = cisloAPRok;
                        akcniPlany.Zadavatel1Jmeno = Convert.ToString(dvAP[DataGridViewAP.CurrentCell.RowIndex]["Zadavatel1Jmeno"]);
                        if (dvAP[DataGridViewAP.CurrentCell.RowIndex]["Zadavatel1Jmeno"] == null)
                        {
                            akcniPlany.Zadavatel2Jmeno = null;
                        }
                        else
                        {
                            akcniPlany.Zadavatel2Jmeno = Convert.ToString(dvAP[DataGridViewAP.CurrentCell.RowIndex]["Zadavatel2Jmeno"]);
                        }
                        akcniPlany.Tema = Convert.ToString(dvAP[DataGridViewAP.CurrentCell.RowIndex]["Tema"]);
                        if (dvAP[DataGridViewAP.CurrentCell.RowIndex]["ProjektNazev"] == null)
                        {
                            akcniPlany.ProjektNazev = null;
                        }
                        else
                        {
                            akcniPlany.ProjektNazev = Convert.ToString(dvAP[DataGridViewAP.CurrentCell.RowIndex]["ProjektNazev"]);
                        }
                        akcniPlany.ZakaznikNazev = Convert.ToString(dvAP[DataGridViewAP.CurrentCell.RowIndex]["ZakaznikNazev"]);

                        bool apUzavren = false;
                        if (Convert.ToString(dvAP[DataGridViewAP.CurrentCell.RowIndex]["OtevrenoUzavreno"]) == "Closed") { apUzavren = true; }
                        akcniPlany.APUzavren = apUzavren;
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
                            //FormMain.VlastnikAP = false;
                            //FormMain.VlastnikAkce = false;
                            //FormMain.VlastnikIdAkce = 0;
                        }
                    }
                }
            }
        }


        //filtrování dat, nastavení filtrů
        private void FiltrOdpovedny1()
        {
            DataTable dtOdpovedny = new DataTable();
            dtOdpovedny.Columns.Add("Odpovedny");

            DataRow radek = dtOdpovedny.NewRow();
            radek["Odpovedny"] = "(select all)";
            dtOdpovedny.Rows.Add(radek);

            for (int i = 0; i < dvAP.Count; i++)
            {
                radek = dtOdpovedny.NewRow();
                bool contains = dtOdpovedny.AsEnumerable().Any(vyhledanyRadek => dvAP[i]["Zadavatel1Jmeno"].ToString() == vyhledanyRadek.Field<string>("Odpovedny"));
                if (contains == false)
                {
                    radek["Odpovedny"] = dvAP[i]["Zadavatel1Jmeno"];
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

        private void FiltrOdpovedny2()
        {
            DataTable dtOdpovedny = new DataTable();
            dtOdpovedny.Columns.Add("Odpovedny");

            DataRow radek = dtOdpovedny.NewRow();
            radek["Odpovedny"] = "(select all)";
            dtOdpovedny.Rows.Add(radek);

            for (int i = 0; i < dvAP.Count; i++)
            {
                if (dvAP[i]["Zadavatel2Jmeno"].ToString() == string.Empty) { }
                else
                {
                    radek = dtOdpovedny.NewRow();
                    bool contains = dtOdpovedny.AsEnumerable().Any(vyhledanyRadek => dvAP[i]["Zadavatel2Jmeno"].ToString() == vyhledanyRadek.Field<string>("Odpovedny"));
                    if (contains == false)
                    {
                        radek["Odpovedny"] = dvAP[i]["Zadavatel2Jmeno"];
                        dtOdpovedny.Rows.Add(radek);
                    }
                }
            }

            string sortColumn = dtOdpovedny.Columns[0].ColumnName;
            //setřídí položky podle abecedy
            dtOdpovedny.DefaultView.Sort = sortColumn;
            dtOdpovedny = dtOdpovedny.DefaultView.ToTable();

            ComboBoxOdpovedny2.DataSource = dtOdpovedny;
            ComboBoxOdpovedny2.DisplayMember = "Odpovedny";
        }

        private void ComboBoxOdpovedny2_SelectedIndexChanged(object sender, EventArgs e)
        {
            OdebratHandlery();

            if (ComboBoxOdpovedny2.SelectedIndex == 0)
            {
                Odpovedny2Filtr = string.Empty;
            }
            else
            {
                Odpovedny2Filtr = ComboBoxOdpovedny2.GetItemText(ComboBoxOdpovedny2.SelectedItem);
            }
            FiltrovatData();

            InitFiltryComboBox();

            NastavitVybranouPolozku();

            PridatHandlery();
            ObarvitLabel();
        }

        private void FiltrProjekty()
        {
            DataTable dtProjekty = new DataTable();
            dtProjekty.Columns.Add("Projekt");

            DataRow radek = dtProjekty.NewRow();
            radek["Projekt"] = "(select all)";
            dtProjekty.Rows.Add(radek);

            for (int i = 0; i < dvAP.Count; i++)
            {
                radek = dtProjekty.NewRow();
                bool contains = dtProjekty.AsEnumerable().Any(vyhledanyRadek => dvAP[i]["ProjektNazev"].ToString() == vyhledanyRadek.Field<string>("Projekt"));
                if (contains == false)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(dvAP[i]["ProjektNazev"])))
                    { }
                    else
                    {
                        radek["Projekt"] = dvAP[i]["ProjektNazev"];
                        dtProjekty.Rows.Add(radek);
                    }
                }
            }

            string sortColumn = dtProjekty.Columns[0].ColumnName;
            //setřídí položky podle abecedy
            dtProjekty.DefaultView.Sort = sortColumn;
            dtProjekty = dtProjekty.DefaultView.ToTable();

            ComboBoxProjekty.DataSource = dtProjekty;
            ComboBoxProjekty.DisplayMember = "Projekt";
        }

        private void ComboBoxProjekty_SelectedIndexChanged(object sender, EventArgs e)
        {
            OdebratHandlery();

            if (ComboBoxProjekty.SelectedIndex == 0)
            {
                ProjektyFiltr = string.Empty;
            }
            else
            {
                ProjektyFiltr = ComboBoxProjekty.GetItemText(ComboBoxProjekty.SelectedItem);
            }
            FiltrovatData();

            InitFiltryComboBox();

            NastavitVybranouPolozku();

            PridatHandlery();
            ObarvitLabel();
        }

        private void FiltrTypAP()
        {
            DataTable dtTypAP = new DataTable();
            dtTypAP.Columns.Add("TypAP");

            DataRow radek = dtTypAP.NewRow();
            radek["TypAP"] = "(select all)";
            dtTypAP.Rows.Add(radek);

            for (int i = 0; i < dvAP.Count; i++)
            {
                radek = dtTypAP.NewRow();
                bool contains = dtTypAP.AsEnumerable().Any(vyhledanyRadek => dvAP[i]["TypAPNazev"].ToString() == vyhledanyRadek.Field<string>("TypAP"));
                if (contains == false)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(dvAP[i]["TypAPNazev"])))
                    { }
                    else
                    {
                        radek["TypAP"] = dvAP[i]["TypAPNazev"];
                        dtTypAP.Rows.Add(radek);
                    }
                }
            }

            string sortColumn = dtTypAP.Columns[0].ColumnName;
            //setřídí položky podle abecedy
            dtTypAP.DefaultView.Sort = sortColumn;
            dtTypAP = dtTypAP.DefaultView.ToTable();

            ComboBoxTypAP.DataSource = dtTypAP;
            ComboBoxTypAP.DisplayMember = "TypAP";
        }

        private void ComboBoxTypAP_SelectedIndexChanged(object sender, EventArgs e)
        {
            OdebratHandlery();

            if (ComboBoxTypAP.SelectedIndex == 0)
            {
                TypAPFiltr = string.Empty;
            }
            else
            {
                TypAPFiltr = ComboBoxTypAP.GetItemText(ComboBoxTypAP.SelectedItem);
            }
            FiltrovatData();

            InitFiltryComboBox();

            NastavitVybranouPolozku();

            PridatHandlery();
            ObarvitLabel();
        }

        private void FiltrRokZalozeni()
        {
            DataTable dtRokZalozeni = new DataTable();
            dtRokZalozeni.Columns.Add("RokZalozeni");

            DataRow radek = dtRokZalozeni.NewRow();
            radek["RokZalozeni"] = "(select all)";
            dtRokZalozeni.Rows.Add(radek);

            for (int i = 0; i < dvAP.Count; i++)
            {
                radek = dtRokZalozeni.NewRow();
                bool contains = dtRokZalozeni.AsEnumerable().Any(vyhledanyRadek => dvAP[i]["DatumZalozeniRok"].ToString() == vyhledanyRadek.Field<string>("RokZalozeni"));
                if (contains == false)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(dvAP[i]["DatumZalozeniRok"])))
                    { }
                    else
                    {
                        radek["RokZalozeni"] = dvAP[i]["DatumZalozeniRok"];
                        dtRokZalozeni.Rows.Add(radek);
                    }
                }
            }

            string sortColumn = dtRokZalozeni.Columns[0].ColumnName;
            //setřídí položky podle abecedy
            dtRokZalozeni.DefaultView.Sort = sortColumn;
            dtRokZalozeni = dtRokZalozeni.DefaultView.ToTable();

            ComboBoxRoky.DataSource = dtRokZalozeni;
            ComboBoxRoky.DisplayMember = "RokZalozeni";
        }

        private void ComboBoxRoky_SelectedIndexChanged(object sender, EventArgs e)
        {
            OdebratHandlery();

            if (ComboBoxRoky.SelectedIndex == 0)
            {
                RokZalozeniFiltr = string.Empty;
            }
            else
            {
                RokZalozeniFiltr = ComboBoxRoky.GetItemText(ComboBoxRoky.SelectedItem);
            }
            FiltrovatData();

            InitFiltryComboBox();

            NastavitVybranouPolozku();

            PridatHandlery();
            ObarvitLabel();
        }

        private void FiltrOtevreneUzavrene()
        {
            DataTable dtOtevreneUzavrene = new DataTable();
            dtOtevreneUzavrene.Columns.Add("OtevreneUzavrene");

            DataRow radek = dtOtevreneUzavrene.NewRow();
            radek["OtevreneUzavrene"] = "(select all)";
            dtOtevreneUzavrene.Rows.Add(radek);

            for (int i = 0; i < dvAP.Count; i++)
            {
                radek = dtOtevreneUzavrene.NewRow();
                bool contains = dtOtevreneUzavrene.AsEnumerable().Any(vyhledanyRadek => dvAP[i]["OtevrenoUzavreno"].ToString() == vyhledanyRadek.Field<string>("OtevreneUzavrene"));
                if (contains == false)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(dvAP[i]["OtevrenoUzavreno"])))
                    { }
                    else
                    {
                        radek["OtevreneUzavrene"] = dvAP[i]["OtevrenoUzavreno"];
                        dtOtevreneUzavrene.Rows.Add(radek);
                    }
                }
            }

            string sortColumn = dtOtevreneUzavrene.Columns[0].ColumnName;
            //setřídí položky podle abecedy
            dtOtevreneUzavrene.DefaultView.Sort = sortColumn;
            dtOtevreneUzavrene = dtOtevreneUzavrene.DefaultView.ToTable();

            ComboBoxOtevreneUzavrene.DataSource = dtOtevreneUzavrene;
            ComboBoxOtevreneUzavrene.DisplayMember = "OtevreneUzavrene";
        }

        private void ComboBoxOtevreneUzavrene_SelectedIndexChanged(object sender, EventArgs e)
        {
            OdebratHandlery();

            if (ComboBoxOtevreneUzavrene.SelectedIndex == 0)
            {
                OtevreneUzavreneFiltr = string.Empty;
            }
            else
            {
                OtevreneUzavreneFiltr = ComboBoxOtevreneUzavrene.GetItemText(ComboBoxOtevreneUzavrene.SelectedItem);
            }
            FiltrovatData();

            InitFiltryComboBox();

            NastavitVybranouPolozku();

            PridatHandlery();
            ObarvitLabel();
        }

        private void InitFiltryComboBox()
        {
            FiltrOdpovedny1();
            FiltrOdpovedny2();
            FiltrProjekty();
            FiltrTypAP();
            FiltrRokZalozeni();
            FiltrOtevreneUzavrene();
        }

        private void PridatHandlery()
        {
            ComboBoxOdpovedny1.SelectedIndexChanged += ComboBoxOdpovedny1_SelectedIndexChanged;
            ComboBoxOdpovedny2.SelectedIndexChanged += ComboBoxOdpovedny2_SelectedIndexChanged;
            ComboBoxProjekty.SelectedIndexChanged += ComboBoxProjekty_SelectedIndexChanged;
            ComboBoxTypAP.SelectedIndexChanged += ComboBoxTypAP_SelectedIndexChanged;
            ComboBoxRoky.SelectedIndexChanged += ComboBoxRoky_SelectedIndexChanged;
            ComboBoxOtevreneUzavrene.SelectedIndexChanged += ComboBoxOtevreneUzavrene_SelectedIndexChanged;
        }

        private void OdebratHandlery()
        {
            ComboBoxOdpovedny1.SelectedIndexChanged -= ComboBoxOdpovedny1_SelectedIndexChanged;
            ComboBoxOdpovedny2.SelectedIndexChanged -= ComboBoxOdpovedny2_SelectedIndexChanged;
            ComboBoxProjekty.SelectedIndexChanged -= ComboBoxProjekty_SelectedIndexChanged;
            ComboBoxTypAP.SelectedIndexChanged -= ComboBoxTypAP_SelectedIndexChanged;
            ComboBoxRoky.SelectedIndexChanged -= ComboBoxRoky_SelectedIndexChanged;
            ComboBoxOtevreneUzavrene.SelectedIndexChanged -= ComboBoxOtevreneUzavrene_SelectedIndexChanged;
        }

        private void InitFiltr()
        {
            Odpovedny1Filtr = string.Empty;
            Odpovedny2Filtr = string.Empty;
            ProjektyFiltr = string.Empty;
            TypAPFiltr = string.Empty;
            RokZalozeniFiltr = string.Empty;
            OtevreneUzavreneFiltr = string.Empty;
        }

        private void NastavitVybranouPolozku()
        {
            if (Odpovedny1Filtr == string.Empty)
                ComboBoxOdpovedny1.SelectedIndex = 0;
            else
                ComboBoxOdpovedny1.SelectedIndex = ComboBoxOdpovedny1.FindStringExact(Odpovedny1Filtr);

            if (Odpovedny2Filtr == string.Empty)
                ComboBoxOdpovedny2.SelectedIndex = 0;
            else
                ComboBoxOdpovedny2.SelectedIndex = ComboBoxOdpovedny2.FindStringExact(Odpovedny2Filtr);

            if (ProjektyFiltr == string.Empty)
                ComboBoxProjekty.SelectedIndex = 0;
            else
                ComboBoxProjekty.SelectedIndex = ComboBoxProjekty.FindStringExact(ProjektyFiltr);

            if (TypAPFiltr == string.Empty)
                ComboBoxTypAP.SelectedIndex = 0;
            else
                ComboBoxTypAP.SelectedIndex = ComboBoxTypAP.FindStringExact(TypAPFiltr);

            if (RokZalozeniFiltr == string.Empty)
                ComboBoxRoky.SelectedIndex = 0;
            else
                ComboBoxRoky.SelectedIndex = ComboBoxRoky.FindStringExact(RokZalozeniFiltr);

            if (OtevreneUzavreneFiltr == string.Empty)
                ComboBoxOtevreneUzavrene.SelectedIndex = 0;
            else
                ComboBoxOtevreneUzavrene.SelectedIndex = ComboBoxOtevreneUzavrene.FindStringExact(OtevreneUzavreneFiltr);
        }

        private void ObarvitLabel()
        {
            if (Odpovedny1Filtr == string.Empty)
                labelOdpovedny1.ForeColor = Color.Black;
            else
                labelOdpovedny1.ForeColor = Color.Blue;

            if (Odpovedny2Filtr == string.Empty)
                labelOdpovedny2.ForeColor = Color.Black;
            else
                labelOdpovedny2.ForeColor = Color.Blue;

            if (ProjektyFiltr == string.Empty)
                labelProjekty.ForeColor = Color.Black;
            else
                labelProjekty.ForeColor = Color.Blue;

            if (TypAPFiltr == string.Empty)
                labelTypAP.ForeColor = Color.Black;
            else
                labelTypAP.ForeColor = Color.Blue;

            if (RokZalozeniFiltr == string.Empty)
                labelRoky.ForeColor = Color.Black;
            else
                labelRoky.ForeColor = Color.Blue;

            if (OtevreneUzavreneFiltr == string.Empty)
                labelOtevreneUzavrene.ForeColor = Color.Black;
            else
                labelOtevreneUzavrene.ForeColor = Color.Blue;
        }

        public void FiltrovatData()
        {
            if (dvAP.Count == 0)
            {
                //když bude počet vyfiltrovaných řádek roven 0, vynulují se všechny fitlry
                //to nastane, když bude vystornován poslední vyfiltrovaný řádek
                //v tomto případě budou odstraněny všechny filtry, počáteční stav
                InitFiltr();
                dvAP.RowFilter = string.Empty;
            }
            else
            {
                dvAP.RowFilter = string.Empty;
                if (!string.IsNullOrEmpty(Odpovedny1Filtr))
                {
                    dvAP.RowFilter = string.Format("Zadavatel1Jmeno = '{0}'", Odpovedny1Filtr);
                }

                if (!string.IsNullOrEmpty(Odpovedny2Filtr))
                {
                    if (dvAP.RowFilter == string.Empty)
                    {
                        dvAP.RowFilter = string.Format("Zadavatel2Jmeno = '{0}'", Odpovedny2Filtr);
                    }
                    else
                    {
                        dvAP.RowFilter += string.Format(" AND Zadavatel2Jmeno = '{0}'", Odpovedny2Filtr);
                    }
                }

                if (!string.IsNullOrEmpty(ProjektyFiltr))
                {
                    if (dvAP.RowFilter == string.Empty)
                    {
                        dvAP.RowFilter = string.Format("ProjektNazev = '{0}'", ProjektyFiltr);
                    }
                    else
                    {
                        dvAP.RowFilter += string.Format(" AND ProjektNazev = '{0}'", ProjektyFiltr);
                    }
                }

                if (!string.IsNullOrEmpty(TypAPFiltr))
                {
                    if (dvAP.RowFilter == string.Empty)
                    {
                        dvAP.RowFilter = string.Format("TypAPNazev = '{0}'", TypAPFiltr);
                    }
                    else
                    {
                        dvAP.RowFilter += string.Format(" AND TypAPNazev = '{0}'", TypAPFiltr);
                    }
                }

                if (!string.IsNullOrEmpty(RokZalozeniFiltr))
                {
                    if (dvAP.RowFilter == string.Empty)
                    {
                        dvAP.RowFilter = string.Format("DatumZalozeniRok = {0}", RokZalozeniFiltr);
                    }
                    else
                    {
                        dvAP.RowFilter += string.Format(" AND DatumZalozeniRok = {0}", RokZalozeniFiltr);
                    }
                }

                if (!string.IsNullOrEmpty(OtevreneUzavreneFiltr))
                {
                    if (dvAP.RowFilter == string.Empty)
                    {
                        dvAP.RowFilter = string.Format("OtevrenoUzavreno = '{0}'", OtevreneUzavreneFiltr);
                    }
                    else
                    {
                        dvAP.RowFilter += string.Format(" AND OtevrenoUzavreno = '{0}'", OtevreneUzavreneFiltr);
                    }
                }
            }
        }

        private void ButtonEditAP_MouseClick(object sender, MouseEventArgs e)
        {
            //nejdřív zjistím, jestli 
            //int zadavatel1AP = Convert.ToInt32(dvAP[DataGridViewAP.CurrentCell.RowIndex]["zadavatel1Id"]);
            //int zadavatel2AP;
            //if (DatabaseReader.ConvertIntegerRow(dvAP[DataGridViewAP.CurrentCell.RowIndex].Row, "Zadavatel2Id") == null)
            //    zadavatel2AP = 0;
            //else
            //    zadavatel2AP = Convert.ToInt32(dvAP[DataGridViewAP.CurrentCell.RowIndex]["zadavatel2Id"]);

            //bool editAPPovolen = false;
            ////majitel AP může editovat jeho parametry
            //if (FormMain.VlastnikIdAkce == zadavatel1AP || FormMain.VlastnikIdAkce == zadavatel2AP)
            //{
            //    editAPPovolen = true;
            //}

            int currentIndexDGV = DataGridViewAP.CurrentCell.RowIndex;

            using (var form = new FormEditAP( 
                Convert.ToInt32(dvAP[currentIndexDGV]["APId"]),
                Convert.ToString(dvAP[currentIndexDGV]["CisloAPRok"]),
                Convert.ToString(dvAP[currentIndexDGV]["Zadavatel1Jmeno"]),
                Convert.ToString(dvAP[currentIndexDGV]["Zadavatel2Jmeno"]),
                Convert.ToString(dvAP[currentIndexDGV]["Tema"]),
                Convert.ToString(dvAP[currentIndexDGV]["ProjektNazev"]),
                Convert.ToString(dvAP[currentIndexDGV]["ZakaznikNazev"]),
                Convert.ToInt32(dvAP[currentIndexDGV]["Zadavatel1Id"]),
                dvAP[currentIndexDGV]["Zadavatel2Id"] == DBNull.Value ? (int?)null : Convert.ToInt32(dvAP[currentIndexDGV]["Zadavatel2Id"]),
                dvAP[currentIndexDGV]["ProjektId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dvAP[currentIndexDGV]["ProjektId"]),
                Convert.ToInt32(dvAP[currentIndexDGV]["ZakaznikId"])))
            {
                form.ShowDialog();
                ZobrazitDGV();

                //string searchExpression = string.Format("APId = {0}", Convert.ToInt32(dvAP[DataGridViewAP.CurrentCell.RowIndex]["APId"]));
                //DataRow[] foundRows = dtAP.Select(searchExpression);
                //foreach (DataRow dr in foundRows)
                //{ 
                //    dr["Zadavatel1Jmeno"] = 
                //}

                //var result = form.ShowDialog();
                //if (result == DialogResult.OK)
                //{
                //    string dateString = form.ReturnValueDatum;
                //    string poznamka = form.ReturnValuePoznamka;

                //    dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxDatumUkonceni"] = Convert.ToDateTime(dateString);
                //    changedDGV = true;
                //    podminkaPoznamka = poznamka == null;
                //    dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxPoznamka"] = podminkaPoznamka ? null : Convert.ToString(poznamka);
                //}
            }
            //nastaví původně vybraný řádek
            DataGridViewAP.Rows[currentIndexDGV].Selected = true;
            DataGridViewAP.CurrentCell = DataGridViewAP.Rows[currentIndexDGV].Cells["DatumZalozeni"];
        }


        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }
    }
}
