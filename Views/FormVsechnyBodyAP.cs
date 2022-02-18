using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using LearActionPlans.Repositories;
using LearActionPlans.ViewModels;
using LearActionPlans.Utilities;

namespace LearActionPlans.Views
{
    public partial class FormVsechnyBodyAP : Form
    {
        private readonly FormPrehledBoduAP formPrehledBoduAp;
        private readonly EmployeeRepository employeeRepository;
        private readonly CustomerRepository customerRepository;
        private readonly ActionPlanRepository actionPlanRepository;
        private readonly ActionPlanPointRepository actionPlanPointRepository;
        private readonly ProjectRepository projectRepository;
        FormNovyAkcniPlan.AkcniPlanTmp akcniPlany;

        private readonly BindingSource bindingSource;
        private DataTable dtBodyAP;
        private DataView dvBodyAP;

        private string Odpovedny1Filtr;
        private string PricinaFiltr;

        public FormVsechnyBodyAP(FormPrehledBoduAP formPrehledBoduAp,
            EmployeeRepository employeeRepository,
            CustomerRepository customerRepository,
            ActionPlanRepository actionPlanRepository,
            ActionPlanPointRepository actionPlanPointRepository,
            ProjectRepository projectRepository)
        {
            // Forms
            this.formPrehledBoduAp = formPrehledBoduAp;

            // Repositories
            this.employeeRepository = employeeRepository;
            this.customerRepository = customerRepository;
            this.actionPlanRepository = actionPlanRepository;
            this.actionPlanPointRepository = actionPlanPointRepository;
            this.projectRepository = projectRepository;

            // Initialize
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
                pi?.SetValue(this.DataGridViewBodyAP, true, null);
            }

            this.CreateColumns();

            this.ZobrazitDGV();
        }

        private void ZobrazitDGV()
        {
            var bodyAP_ = this.GetBodyAPAll().ToList();

            foreach (var b in bodyAP_)
            {
                var cisloAPRok = $"{Convert.ToInt32(b.CisloAP):D3} / {Convert.ToDateTime(b.DatumZalozeniAP).Year}";

                this.dtBodyAP.Rows.Add(b.IdAP, b.CisloAP, cisloAPRok, b.DatumZalozeniAP, b.CisloBoduAP, b.OdkazNaNormu,
                    b.HodnoceniNeshody, b.PopisProblemu, b.OdpovednaOsoba1, string.Empty, b.SkutecnaPricinaWM);

                this.dvBodyAP = this.dtBodyAP.DefaultView;
            }

            //nastaví filtry na string.empty
            this.InitFilter();

            //naplní comboBoxy
            this.InitFiltersComboBox();

            //v comboBoxech nastaví vybrané položky
            this.NastavitVybranouPolozku();

            this.PridatHandlery();
            this.ColorLabel();
        }

        private IEnumerable<VsechnyBodyAPViewModel> GetBodyAPAll()
        {
            var bodyAP = this.actionPlanPointRepository.GetBodyAPAll().ToList();
            var akcniPlany = this.actionPlanRepository.GetAll().ToList();
            var zamestnanci = this.employeeRepository.GetZamestnanciAll().ToList();

            if (!bodyAP.Any() || !akcniPlany.Any())
            {
                yield break;
            }

            var query = from b in bodyAP
                join ap in akcniPlany
                    on b.AkcniPlanId equals ap.Id into gAP
                from subAP in gAP.DefaultIfEmpty()
                join zam in zamestnanci
                    on b.OdpovednaOsoba1Id equals zam.Id into gZam
                from subZam in gZam.DefaultIfEmpty()
                orderby b.AkcniPlanId, b.CisloBoduAP
                select VsechnyBodyAPViewModel.BodyAP(subAP.DatumZalozeni, subAP.CisloAP, b.Id, b.AkcniPlanId, b.CisloBoduAP, b.DatumZalozeni,
                    b.OdkazNaNormu, b.HodnoceniNeshody,
                    b.PopisProblemu, subZam.Prijmeni + " " + subZam.Jmeno, b.SkutecnaPricinaWM, b.StavObjektu);

            foreach (var q in query)
            {
                yield return q;
            }
        }

        private void CreateColumns()
        {
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

            this.dtBodyAP.Columns.Add(new DataColumn("TextBoxSkutecnaPricina", typeof(string)));

            this.DataGridViewBodyAP.Columns["CisloBodAP"].HeaderText = @"AP point number";
            this.DataGridViewBodyAP.Columns["CisloBodAP"].Width = 120;

            this.DataGridViewBodyAP.Columns["TextBoxOdkazNaNormu"].HeaderText = @"Standard chapter";
            this.DataGridViewBodyAP.Columns["TextBoxOdkazNaNormu"].Width = 200;

            this.DataGridViewBodyAP.Columns["TextBoxHodnoceniNeshody"].HeaderText = @"Evaluation";
            this.DataGridViewBodyAP.Columns["TextBoxHodnoceniNeshody"].Width = 200;

            this.DataGridViewBodyAP.Columns["TextBoxPopisProblemu"].HeaderText = @"Description of the problem";
            this.DataGridViewBodyAP.Columns["TextBoxPopisProblemu"].Width = 200;

            this.DataGridViewBodyAP.Columns["OdpovednyPracovnik1"].HeaderText = @"Responsible #1";
            this.DataGridViewBodyAP.Columns["OdpovednyPracovnik1"].Width = 200;

            this.DataGridViewBodyAP.Columns["OdpovednyPracovnik2"].HeaderText = @"Responsible #2";
            this.DataGridViewBodyAP.Columns["OdpovednyPracovnik2"].Width = 200;

            this.DataGridViewBodyAP.Columns["TextBoxSkutecnaPricina"].HeaderText = @"Root cause";
            this.DataGridViewBodyAP.Columns["TextBoxSkutecnaPricina"].Width = 200;

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
        }

        private void DataGridViewBodyAP_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                {
                    if (senderGrid.Columns[e.ColumnIndex].Name == "ButtonCisloAPRok")
                    {
                        this.akcniPlany = new FormNovyAkcniPlan.AkcniPlanTmp();

                        var idAP = Convert.ToInt32(this.dvBodyAP[this.DataGridViewBodyAP.CurrentCell.RowIndex]["APId"]);
                        this.akcniPlany.Id = idAP;

                        var ap = this.GetSelectedAP(idAP).ToList();
                        var dtAP = DataTableConverter.ConvertToDataTable(ap);
                        var zad2 = this.employeeRepository.GetAll().ToList();

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
                                var vyhledaneJmeno = zad2.Find(x => x.Id == id);
                                zadavatel2 = vyhledaneJmeno?.Jmeno;
                            }

                            this.akcniPlany.Zadavatel2Jmeno = zadavatel2;

                            this.akcniPlany.Tema = Convert.ToString(row["Tema"]);
                            this.akcniPlany.ProjektNazev =
                                row["ProjektId"] == null ? null : Convert.ToString(row["Projekt"]);

                            this.akcniPlany.ZakaznikNazev = Convert.ToString(row["Zakaznik"]);
                        }

                        string cisloAPRok;
                        cisloAPRok =
                            Convert.ToInt32(this.dvBodyAP[this.DataGridViewBodyAP.CurrentCell.RowIndex]["CisloAP"])
                                .ToString("D3") + " / " +
                            Convert.ToDateTime(
                                this.dvBodyAP[this.DataGridViewBodyAP.CurrentCell.RowIndex]["DatumZalozeni"]).Year;
                        this.akcniPlany.CisloAPRok = cisloAPRok;

                        //tada se ověří uživatel
                        //volat FormOvereniUzivatele
                        using var form = this.formPrehledBoduAp;
                        form.CreateFormPrehledBoduAP(true, this.akcniPlany, 2);
                        form.ShowDialog();
                    }
                }
            }
        }

        public IEnumerable<VsechnyBodyAPViewModel> GetSelectedAP(int idAP)
        {
            var akcniPlany = this.actionPlanRepository.GetAPId(idAP).ToList();
            var zamestnanci = this.employeeRepository.GetZamestnanciAll().ToList();
            var projekty = this.projectRepository.GetProjektyAll().ToList();
            var zakaznici = this.customerRepository.GetAll().ToList();

            var query = from ap in akcniPlany
                join zam in zamestnanci
                    on ap.Zadavatel1Id equals zam.Id into gZam
                from subZam in gZam.DefaultIfEmpty()
                join pro in projekty
                    on ap.ProjektId equals pro.Id into gPro
                from subPro in gPro.DefaultIfEmpty()
                join zak in zakaznici
                    on ap.ZakaznikId equals zak.Id into gZak
                from subZak in gZak.DefaultIfEmpty()
                select VsechnyBodyAPViewModel.AP(ap.Zadavatel1Id, ap?.Zadavatel2Id, subZam.Prijmeni + " " + subZam.Jmeno, ap.Tema,
                    ap?.ProjektId,
                    subPro?.Nazev ?? string.Empty, ap.ZakaznikId, subZak.Nazev, ap.TypAP, ap.StavObjektu);

            foreach (var q in query)
            {
                yield return q;
            }
        }

        private void ButtonFiltrPopisProblemu_MouseClick(object sender, MouseEventArgs e) => this.FilterData();

        private void ButtonFiltrPricina_MouseClick(object sender, MouseEventArgs e) { }

        private void InitFiltersComboBox() => this.FiltrOdpovedny1();

        private void FilterData()
        {
            if (this.dvBodyAP.Count == 0)
            {
                this.InitFilter();
                this.dvBodyAP.RowFilter = string.Empty;
            }
            else
            {
                this.dvBodyAP.RowFilter = string.Empty;
                if (!string.IsNullOrEmpty(this.Odpovedny1Filtr))
                {
                    this.dvBodyAP.RowFilter = string.Format("OdpovednyPracovnik1 = '{0}'", this.Odpovedny1Filtr);
                }
            }
        }

        private void InitFilter() => this.Odpovedny1Filtr = string.Empty;

        private void ButtonClose_MouseClick(object sender, MouseEventArgs e) => this.Close();

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
                var contains = dtOdpovedny.AsEnumerable().Any(vyhledanyRadek =>
                    this.dvBodyAP[i]["OdpovednyPracovnik1"].ToString() == vyhledanyRadek.Field<string>("Odpovedny"));

                if (contains)
                {
                    continue;
                }

                radek["Odpovedny"] = this.dvBodyAP[i]["OdpovednyPracovnik1"];
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

            this.Odpovedny1Filtr = this.ComboBoxOdpovedny1.SelectedIndex == 0
                ? string.Empty
                : this.ComboBoxOdpovedny1.GetItemText(this.ComboBoxOdpovedny1.SelectedItem);

            this.FilterData();

            this.InitFiltersComboBox();

            this.NastavitVybranouPolozku();

            this.PridatHandlery();
            this.ColorLabel();
        }

        private void PridatHandlery()
        {
            this.ComboBoxOdpovedny1.SelectedIndexChanged += this.ComboBoxOdpovedny1_SelectedIndexChanged;
            //ComboBoxOdpovedny2.SelectedIndexChanged += ComboBoxOdpovedny2_SelectedIndexChanged;
        }

        private void OdebratHandlery()
        {
            this.ComboBoxOdpovedny1.SelectedIndexChanged -= this.ComboBoxOdpovedny1_SelectedIndexChanged;
            //ComboBoxOdpovedny2.SelectedIndexChanged -= ComboBoxOdpovedny2_SelectedIndexChanged;
        }

        private void NastavitVybranouPolozku()
        {
            // ReSharper disable once ArrangeMethodOrOperatorBody
            this.ComboBoxOdpovedny1.SelectedIndex = this.Odpovedny1Filtr == string.Empty
                ? 0
                : this.ComboBoxOdpovedny1.FindStringExact(this.Odpovedny1Filtr);
        }

        private void ColorLabel()
        {
            // ReSharper disable once ArrangeMethodOrOperatorBody
            this.labelOdpovedny1.ForeColor = this.Odpovedny1Filtr == string.Empty ? Color.Black : Color.Blue;
        }
    }
}
