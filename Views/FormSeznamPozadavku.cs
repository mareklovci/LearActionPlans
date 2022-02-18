using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using LearActionPlans.Repositories;
using LearActionPlans.Utilities;

namespace LearActionPlans.Views
{
    public partial class FormSeznamPozadavku : Form
    {
        private readonly EmployeeRepository employeeRepository;

        private DataTable dtActions;
        private readonly BindingSource bindingSourceAkce = new BindingSource();

        protected IList<Zamestnanec> itemsZamestnanec = new BindingList<Zamestnanec>();

        public class Zamestnanec
        {
            public int ZamestnanecId { get; set; }
            public string Jmeno { get; set; }
            public string PrihlasovaciJmeno { get; set; }
            public string Email { get; set; }
            public byte StavObjektu { get; set; }
        }

        public FormSeznamPozadavku(
            EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;

            this.InitializeComponent();
            this.dtActions = new DataTable();
        }

        private void FormSeznamPozadavku_Load(object sender, EventArgs e)
        {
            this.bindingSourceAkce.DataSource = this.dtActions;
            this.DataGridViewSeznamPozadavku.DataSource = this.bindingSourceAkce;

            var zam = this.employeeRepository.GetEmployeesOriginalViewModel().ToList();
            this.NaplnitCmbZamestnanec();

            this.CreateColumns(zam);
        }

        private void NaplnitCmbZamestnanec()
        {
            var zamestnanci = this.employeeRepository.GetEmployeesOriginalViewModel();

            this.itemsZamestnanec.Add(new Zamestnanec
            {
                ZamestnanecId = 0,
                Jmeno = "(Select emploees)",
                PrihlasovaciJmeno = null,
                Email = null,
                StavObjektu = 1
            });

            foreach (var z in zamestnanci)
            {
                this.itemsZamestnanec.Add(new Zamestnanec
                {
                    ZamestnanecId = z.Id,
                    Jmeno = z.Jmeno,
                    PrihlasovaciJmeno = z.PrihlasovaciJmeno,
                    Email = z.Email,
                    StavObjektu = z.StavObjektu
                });
            }

            this.ComboBoxZamestnanci.DataSource = this.itemsZamestnanec;

            this.ComboBoxZamestnanci.DisplayMember = "Jmeno";
            this.ComboBoxZamestnanci.ValueMember = "ZamestnanecId";
            this.ComboBoxZamestnanci.SelectedIndex = 0;
        }

        private void CreateColumns<T>(IEnumerable<T> zam)
        {
            this.dtActions.Columns.Add(new DataColumn("textBoxNapravnaOpatreni", typeof(string)));

            this.dtActions.Columns.Add(new DataColumn("textBoxDatumUkonceni", typeof(DateTime)));
            var btn = new DataGridViewButtonColumn
            {
                Name = "buttonDatumUkonceni",
                HeaderText = @"Deadline",
                Width = 100,
                DataPropertyName = "textBoxDatumUkonceni",
                FlatStyle = FlatStyle.Flat,
                ReadOnly = true
            };
            this.DataGridViewSeznamPozadavku.Columns.Add(btn);

            this.dtActions.Columns.Add(new DataColumn("textBoxKontrolaEfektivnosti", typeof(DateTime)));
            this.DataGridViewSeznamPozadavku.Columns["textBoxKontrolaEfektivnosti"].Visible = false;

            var dtZam = DataTableConverter.ConvertToDataTable(zam);

            DataRow drZam;
            drZam = dtZam.NewRow();
            drZam["Jmeno"] = "(select responsible)";
            drZam["Id"] = 0;
            dtZam.Rows.InsertAt(drZam, 0);

            this.dtActions.Columns.Add(new DataColumn("textBoxOdpovednaOsoba1", typeof(string)));
            this.dtActions.Columns.Add(new DataColumn("textBoxOdpovednaOsoba2", typeof(string)));
            this.dtActions.Columns.Add(new DataColumn("textBoxOddeleni", typeof(string)));
            this.DataGridViewSeznamPozadavku.Columns["textBoxNapravnaOpatreni"].HeaderText = @"Corrective actions";
            this.DataGridViewSeznamPozadavku.Columns["textBoxOdpovednaOsoba1"].HeaderText = @"Responsibel #1";
            this.DataGridViewSeznamPozadavku.Columns["textBoxOdpovednaOsoba2"].HeaderText = @"Responsibel #2";
            this.DataGridViewSeznamPozadavku.Columns["textBoxOddeleni"].HeaderText = @"Department";

            this.dtActions.Columns.Add(new DataColumn("textBoxPriloha", typeof(string)));

            this.DataGridViewSeznamPozadavku.Columns["textBoxPriloha"].Visible = false;
            btn = new DataGridViewButtonColumn
            {
                Name = "buttonPriloha",
                HeaderText = @"Attachment",
                Width = 120,
                FlatStyle = FlatStyle.Flat,
                DataPropertyName = "prilohaTmp",
                ReadOnly = true
            };
            //DataPropertyName = "buttonPrilohaTmp",
            this.DataGridViewSeznamPozadavku.Columns.Add(btn);
            this.DataGridViewSeznamPozadavku.Columns["buttonPriloha"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            this.dtActions.Columns.Add(new DataColumn("prilohaTmp", typeof(string)));
            this.dtActions.Columns["prilohaTmp"].Expression =
                string.Format("IIF([textBoxPriloha] = {0}, '', '(attachment)')", "''");

            this.dtActions.Columns.Add(new DataColumn("akceId", typeof(int)));
            this.DataGridViewSeznamPozadavku.Columns["akceId"].Visible = false;

            this.DataGridViewSeznamPozadavku.Columns["textBoxNapravnaOpatreni"].Width = 200;
            this.DataGridViewSeznamPozadavku.Columns["textBoxOdpovednaOsoba1"].Width = 200;
            this.DataGridViewSeznamPozadavku.Columns["textBoxOdpovednaOsoba2"].Width = 200;
            this.DataGridViewSeznamPozadavku.Columns["textBoxOddeleni"].Width = 150;
            //DataGridViewSeznamPozadavku.Columns[""].Width = 200;

            this.DataGridViewSeznamPozadavku.MultiSelect = false;
            this.DataGridViewSeznamPozadavku.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            this.DataGridViewSeznamPozadavku.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            this.DataGridViewSeznamPozadavku.RowHeadersWidthSizeMode =
                DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DataGridViewSeznamPozadavku.AllowUserToResizeRows = false;
            this.DataGridViewSeznamPozadavku.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            this.DataGridViewSeznamPozadavku.AllowUserToResizeColumns = false;
            this.DataGridViewSeznamPozadavku.AllowUserToAddRows = false;
            this.DataGridViewSeznamPozadavku.ReadOnly = false;
            this.DataGridViewSeznamPozadavku.EditMode = DataGridViewEditMode.EditOnEnter;
            this.DataGridViewSeznamPozadavku.AutoGenerateColumns = false;

            foreach (DataGridViewColumn column in this.DataGridViewSeznamPozadavku.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            this.DataGridViewSeznamPozadavku.Columns["textBoxDatumUkonceni"].Visible = false;
            this.DataGridViewSeznamPozadavku.Columns["textBoxKontrolaEfektivnosti"].ReadOnly = true;
            this.DataGridViewSeznamPozadavku.Columns["buttonPriloha"].ReadOnly = true;
        }

        private void ButtonClose_MouseClick(object sender, MouseEventArgs e) => this.Close();

        private void ButtonZavrit_Click(object sender, EventArgs e)
        {
        }
    }
}
