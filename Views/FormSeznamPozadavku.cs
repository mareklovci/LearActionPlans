using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

using LearActionPlans.ViewModels;
using LearActionPlans.Utilities;

namespace LearActionPlans.Views
{
    public partial class FormSeznamPozadavku : Form
    {
        private DataTable dtActions;
        private readonly BindingSource bindingSourceAkce = new BindingSource();

        protected IList<Zamestnanec> itemsZamestnanec = new BindingList<Zamestnanec>();

        protected Zamestnanec selectedZamestnanec = null;

        public class Zamestnanec
        {
            public int ZamestnanecId { get; set; }
            public string Jmeno { get; set; }
            public string PrihlasovaciJmeno { get; set; }
            public string Email { get; set; }
            public byte StavObjektu { get; set; }
        }

        public FormSeznamPozadavku()
        {
            InitializeComponent();
            dtActions = new DataTable();


        }

        private void FormSeznamPozadavku_Load(object sender, EventArgs e)
        {
            bindingSourceAkce.DataSource = dtActions;
            DataGridViewSeznamPozadavku.DataSource = bindingSourceAkce;

            var zam = SeznamPozadavkuViewModel.GetZamestnanciAll().ToList();
            //var oddeleni = ZadaniBoduAPViewModel.GetOddeleniAll().ToList();

            //var dtZam = DataTableConverter.ConvertToDataTable(zam);

            //DataRow drZam;
            //drZam = dtZam.NewRow();
            //drZam["Jmeno"] = "(select responsible)";
            //drZam["Id"] = 0;
            //dtZam.Rows.InsertAt(drZam, 0);
            //ComboBoxZamestnanci.DataSource = new BindingSource { DataSource = dtZam };
            //ComboBoxZamestnanci.DisplayMember = "Jmeno";
            //ComboBoxZamestnanci.ValueMember = "Id";

            //ComboBoxZamestnanci.SelectedIndex = 0;
            NaplnitCmbZamestnanec();

            CreateColumns(zam);

            //ZobrazitDGVAkce();
        }

        //private void ZobrazitDGVAkce()
        //{
        //    var akceZadost = SeznamPozadavkuViewModel.GetAkceZadost().ToList();

        //    foreach (var a in akceZadost)
        //    {
        //        dtActions.Rows.Add(new object[] { a.BodyAPId,
        //                    a.
        //                    Convert.ToString(radekDGV.OdpovednaOsoba1Id),
        //                    podminkaOsoba2 ? 0 : radekDGV.OdpovednaOsoba2Id,
        //                    radekDGV.UkonceniAkce[0].DatumUkonceni,
        //                    podminkaPoznamka ? null : radekDGV.UkonceniAkce[0].Poznamka,
        //                    podminkaKontrEfekt ? (DateTime?)null : Convert.ToDateTime(radekDGV.KontrolaEfektivnosti),
        //                    podminkaOddeleni ? 0 : radekDGV.Oddeleni_Id,
        //                    podminkaPriloha ? "" : radekDGV.Priloha,
        //                    null,
        //                    radekDGV.Id});
        //    }
        //}

        private void NaplnitCmbZamestnanec()
        {
            var zamestnanci = SeznamPozadavkuViewModel.GetZamestnanciAll();

            itemsZamestnanec.Add(new Zamestnanec { ZamestnanecId = 0, Jmeno = "(Select emploees)", PrihlasovaciJmeno = null, Email = null, StavObjektu = 1 });
            foreach (var z in zamestnanci)
            {
                itemsZamestnanec.Add(new Zamestnanec { ZamestnanecId = z.Id, Jmeno = z.Jmeno, PrihlasovaciJmeno = z.PrihlasovaciJmeno, Email = z.Email, StavObjektu = z.StavObjektu });
            }

            ComboBoxZamestnanci.DataSource = itemsZamestnanec;

            ComboBoxZamestnanci.DisplayMember = "Jmeno";
            ComboBoxZamestnanci.ValueMember = "ZamestnanecId";
            ComboBoxZamestnanci.SelectedIndex = 0;
            ComboBoxZamestnanci.SelectedIndexChanged += ComboBoxZamestnanci_SelectedIndexChanged;
        }

        private void ComboBoxZamestnanci_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender is ComboBox combo))
            {
                return;
            }
            selectedZamestnanec = combo.SelectedItem as Zamestnanec;
            if (selectedZamestnanec == null)
            {
                //MessageBox.Show("You didn't select anything at the moment");
                ;
            }
            else
            {
                if (combo.SelectedIndex == 0)
                {
                    //emailDisponent = false;
                    //nakladkyVykladky.Email = null;
                }
                else
                {
                    //emailDisponent = true;
                    //nakladkyVykladky.Email = selectedDisponent.Email;
                    //tady načtu akce, u kterých je žádáno o prodloužení termínu

                }
            }
        }

        private void CreateColumns<T>(IEnumerable<T> zam)
        {
            dtActions.Columns.Add(new DataColumn("textBoxNapravnaOpatreni", typeof(string)));

            dtActions.Columns.Add(new DataColumn("textBoxDatumUkonceni", typeof(DateTime)));
            var btn = new DataGridViewButtonColumn
            {
                Name = "buttonDatumUkonceni",
                HeaderText = @"Deadline",
                Width = 100,
                DataPropertyName = "textBoxDatumUkonceni",
                FlatStyle = FlatStyle.Flat,
                ReadOnly = true
            };
            DataGridViewSeznamPozadavku.Columns.Add(btn);

            dtActions.Columns.Add(new DataColumn("textBoxKontrolaEfektivnosti", typeof(DateTime)));
            DataGridViewSeznamPozadavku.Columns["textBoxKontrolaEfektivnosti"].Visible = false;
            
            var dtZam = DataTableConverter.ConvertToDataTable(zam);

            DataRow drZam;
            drZam = dtZam.NewRow();
            drZam["Jmeno"] = "(select responsible)";
            drZam["Id"] = 0;
            dtZam.Rows.InsertAt(drZam, 0);

            dtActions.Columns.Add(new DataColumn("textBoxOdpovednaOsoba1", typeof(string)));
            dtActions.Columns.Add(new DataColumn("textBoxOdpovednaOsoba2", typeof(string)));
            dtActions.Columns.Add(new DataColumn("textBoxOddeleni", typeof(string)));
            DataGridViewSeznamPozadavku.Columns["textBoxNapravnaOpatreni"].HeaderText = @"Corrective actions";
            DataGridViewSeznamPozadavku.Columns["textBoxOdpovednaOsoba1"].HeaderText = @"Responsibel #1";
            DataGridViewSeznamPozadavku.Columns["textBoxOdpovednaOsoba2"].HeaderText = @"Responsibel #2";
            DataGridViewSeznamPozadavku.Columns["textBoxOddeleni"].HeaderText = @"Department";

            dtActions.Columns.Add(new DataColumn("textBoxPriloha", typeof(string)));

            DataGridViewSeznamPozadavku.Columns["textBoxPriloha"].Visible = false;
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
            DataGridViewSeznamPozadavku.Columns.Add(btn);
            DataGridViewSeznamPozadavku.Columns["buttonPriloha"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtActions.Columns.Add(new DataColumn("prilohaTmp", typeof(string)));
            dtActions.Columns["prilohaTmp"].Expression = string.Format("IIF([textBoxPriloha] = {0}, '', '(attachment)')", "''");

            dtActions.Columns.Add(new DataColumn("akceId", typeof(int)));
            DataGridViewSeznamPozadavku.Columns["akceId"].Visible = false;

            DataGridViewSeznamPozadavku.Columns["textBoxNapravnaOpatreni"].Width = 200;
            DataGridViewSeznamPozadavku.Columns["textBoxOdpovednaOsoba1"].Width = 200;
            DataGridViewSeznamPozadavku.Columns["textBoxOdpovednaOsoba2"].Width = 200;
            DataGridViewSeznamPozadavku.Columns["textBoxOddeleni"].Width = 150;
            //DataGridViewSeznamPozadavku.Columns[""].Width = 200;

            DataGridViewSeznamPozadavku.MultiSelect = false;
            DataGridViewSeznamPozadavku.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            DataGridViewSeznamPozadavku.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            DataGridViewSeznamPozadavku.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            DataGridViewSeznamPozadavku.AllowUserToResizeRows = false;
            DataGridViewSeznamPozadavku.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            DataGridViewSeznamPozadavku.AllowUserToResizeColumns = false;
            DataGridViewSeznamPozadavku.AllowUserToAddRows = false;
            DataGridViewSeznamPozadavku.ReadOnly = false;
            DataGridViewSeznamPozadavku.EditMode = DataGridViewEditMode.EditOnEnter;
            DataGridViewSeznamPozadavku.AutoGenerateColumns = false;

            foreach (DataGridViewColumn column in DataGridViewSeznamPozadavku.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            DataGridViewSeznamPozadavku.Columns["textBoxDatumUkonceni"].Visible = false;
            DataGridViewSeznamPozadavku.Columns["textBoxKontrolaEfektivnosti"].ReadOnly = true;
            DataGridViewSeznamPozadavku.Columns["buttonPriloha"].ReadOnly = true;
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void ButtonZavrit_Click(object sender, EventArgs e)
        {

        }
    }
}
