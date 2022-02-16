using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using LearActionPlans.Utilities;

namespace LearActionPlans.Views
{
    public partial class FormZadaniBoduAP
    {
        private void CreateColumnsWM<T>(IEnumerable<T> zam, IEnumerable<T> oddeleni)
        {
            //this.dtActionsWM.Columns.Add(new DataColumn("textBoxNapravnaOpatreni", typeof(string)));
            //this.dtActionsWM.Columns.Add(new DataColumn("comboBoxOdpovednaOsoba1Id", typeof(int)));
            //this.dtActionsWM.Columns.Add(new DataColumn("comboBoxOdpovednaOsoba2Id", typeof(int)));

            ////to je kvůli vytvoření prvnímu deadlinu
            //this.dtActionsWM.Columns.Add(new DataColumn("textBoxDatumUkonceni", typeof(DateTime)));
            //this.DataGridViewWMAkce.Columns["textBoxDatumUkonceni"].ReadOnly = true;

            ////to je kvůli vytvoření prvnímu deadlinu
            //this.dtActionsWM.Columns.Add(new DataColumn("textBoxPoznamka", typeof(string)));
            //this.DataGridViewWMAkce.Columns["textBoxPoznamka"].ReadOnly = true;

            //var btn = new DataGridViewButtonColumn
            //{
            //    Name = "buttonDatumUkonceni",
            //    HeaderText = @"Deadline",
            //    Width = 100,
            //    DataPropertyName = "textBoxDatumUkonceni",
            //    FlatStyle = FlatStyle.Flat,
            //    ReadOnly = true
            //};
            //this.DataGridViewWMAkce.Columns.Add(btn);

            //this.DataGridViewWMAkce.Columns["textBoxDatumUkonceni"].Visible = false;
            //this.DataGridViewWMAkce.Columns["textBoxPoznamka"].Visible = false;

            //this.dtActionsWM.Columns.Add(new DataColumn("textBoxKontrolaEfektivnosti", typeof(DateTime)));
            //this.DataGridViewWMAkce.Columns["textBoxKontrolaEfektivnosti"].Visible = false;

            //this.dtActionsWM.Columns.Add(new DataColumn("textBoxKontrolaEfektivnostiPuvodniDatum", typeof(string)));
            //this.DataGridViewWMAkce.Columns["textBoxKontrolaEfektivnostiPuvodniDatum"].Visible = false;

            //this.dtActionsWM.Columns.Add(new DataColumn("textBoxKontrolaEfektivnostiOdstranit", typeof(string)));
            //this.DataGridViewWMAkce.Columns["textBoxKontrolaEfektivnostiOdstranit"].Visible = false;

            //btn = new DataGridViewButtonColumn
            //{
            //    Name = "buttonKontrolaEfektivnosti",
            //    HeaderText = @"Effectiveness",
            //    Width = 120,
            //    DataPropertyName = "textBoxKontrolaEfektivnosti",
            //    FlatStyle = FlatStyle.Flat,
            //    ReadOnly = true
            //};
            //this.DataGridViewWMAkce.Columns.Add(btn);

            //this.dtActionsWM.Columns.Add(new DataColumn("comboBoxOddeleniId", typeof(int)));

            //var dtZam = DataTableConverter.ConvertToDataTable(zam);

            //DataRow drZam;
            //drZam = dtZam.NewRow();
            //drZam["Jmeno"] = "(select responsible)";
            //drZam["ZamestnanecId"] = 0;
            //dtZam.Rows.InsertAt(drZam, 0);

            //this.DataGridViewWMAkce.Columns["textBoxNapravnaOpatreni"].HeaderText = @"Corrective actions";
            //this.DataGridViewWMAkce.Columns["textBoxNapravnaOpatreni"].Width = 200;

            //var cbox1 = new DataGridViewComboBoxColumn
            //{
            //    Name = "comboBoxOdpovednaOsoba1",
            //    HeaderText = @"Responsible #1",
            //    Width = 200,
            //    FlatStyle = FlatStyle.Flat,
            //    DataSource = new BindingSource {DataSource = dtZam},
            //    ReadOnly = false,
            //    DisplayMember = "Jmeno",
            //    DataPropertyName = "comboBoxOdpovednaOsoba1Id",
            //    ValueMember = "ZamestnanecId"
            //};
            //this.DataGridViewWMAkce.Columns["comboBoxOdpovednaOsoba1Id"].Visible = false;
            //this.DataGridViewWMAkce.Columns.Add(cbox1); // Add new

            //var cbox2 = new DataGridViewComboBoxColumn
            //{
            //    Name = "comboBoxOdpovednaOsoba2",
            //    HeaderText = @"Responsible #2",
            //    Width = 200,
            //    FlatStyle = FlatStyle.Flat,
            //    DataSource = new BindingSource {DataSource = dtZam},
            //    ReadOnly = false,
            //    DisplayMember = "Jmeno",
            //    DataPropertyName = "comboBoxOdpovednaOsoba2Id",
            //    ValueMember = "ZamstnanecId"
            //};
            //this.DataGridViewWMAkce.Columns["comboBoxOdpovednaOsoba2Id"].Visible = false;
            //this.DataGridViewWMAkce.Columns.Add(cbox2);

            //var dtOdd = DataTableConverter.ConvertToDataTable(oddeleni);

            //DataRow drOdd;
            //drOdd = dtOdd.NewRow();
            //drOdd["Nazev"] = "(select department)";
            //drOdd["Id"] = 0;
            //dtOdd.Rows.InsertAt(drOdd, 0);

            //var department = new DataGridViewComboBoxColumn
            //{
            //    Name = "comboBoxOddeleni",
            //    HeaderText = @"Department",
            //    Width = 180,
            //    FlatStyle = FlatStyle.Flat,
            //    DataSource = new BindingSource {DataSource = dtOdd},
            //    ReadOnly = false,
            //    DisplayMember = "Nazev",
            //    DataPropertyName = "comboBoxOddeleniId",
            //    ValueMember = "Id"
            //};
            //this.DataGridViewWMAkce.Columns["comboBoxOddeleniId"].Visible = false;
            //this.DataGridViewWMAkce.Columns.Add(department);

            //this.dtActionsWM.Columns.Add(new DataColumn("textBoxPriloha", typeof(string)));

            //this.DataGridViewWMAkce.Columns["textBoxPriloha"].Visible = false;
            //btn = new DataGridViewButtonColumn
            //{
            //    Name = "buttonPriloha",
            //    HeaderText = @"Attachment",
            //    Width = 120,
            //    FlatStyle = FlatStyle.Flat,
            //    DataPropertyName = "prilohaTmp",
            //    ReadOnly = true
            //};
            ////DataPropertyName = "buttonPrilohaTmp",
            //this.DataGridViewWMAkce.Columns.Add(btn);
            //this.DataGridViewWMAkce.Columns["buttonPriloha"].DefaultCellStyle.Alignment =
            //    DataGridViewContentAlignment.MiddleCenter;

            //this.dtActionsWM.Columns.Add(new DataColumn("prilohaTmp", typeof(string)));
            //this.dtActionsWM.Columns["prilohaTmp"].Expression =
            //    string.Format("IIF([textBoxPriloha] = {0}, '', '(attachment)')", "''");

            //this.dtActionsWM.Columns.Add(new DataColumn("akceId", typeof(int)));
            //this.DataGridViewWMAkce.Columns["akceId"].Visible = false;

            //this.dtActionsWM.Columns.Add(new DataColumn("reopen", typeof(bool)));
            //this.DataGridViewWMAkce.Columns["reopen"].HeaderText = @"After reopen";
            //this.DataGridViewWMAkce.Columns["reopen"].Width = 120;

            //this.DataGridViewWMAkce.MultiSelect = false;
            //this.DataGridViewWMAkce.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            //this.DataGridViewWMAkce.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //this.DataGridViewWMAkce.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            //this.DataGridViewWMAkce.AllowUserToResizeRows = false;
            //this.DataGridViewWMAkce.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            //this.DataGridViewWMAkce.AllowUserToResizeColumns = false;
            //this.DataGridViewWMAkce.AllowUserToAddRows = false;
            //this.DataGridViewWMAkce.ReadOnly = false;
            //this.DataGridViewWMAkce.EditMode = DataGridViewEditMode.EditOnEnter;
            //this.DataGridViewWMAkce.AutoGenerateColumns = false;
            //this.DataGridViewWMAkce.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            //foreach (DataGridViewColumn column in this.DataGridViewWMAkce.Columns)
            //{
            //    column.SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
        }

        private void CreateColumnsWS<T>(IEnumerable<T> zam, IEnumerable<T> oddeleni)
        {
            //this.dtActionsWS.Columns.Add(new DataColumn("textBoxNapravnaOpatreni", typeof(string)));
            //this.dtActionsWS.Columns.Add(new DataColumn("comboBoxOdpovednaOsoba1Id", typeof(int)));
            //this.dtActionsWS.Columns.Add(new DataColumn("comboBoxOdpovednaOsoba2Id", typeof(int)));

            ////to je kvůli vytvoření prvnímu deadlinu
            //this.dtActionsWS.Columns.Add(new DataColumn("textBoxDatumUkonceni", typeof(DateTime)));
            //this.DataGridViewWSAkce.Columns["textBoxDatumUkonceni"].ReadOnly = true;

            ////to je kvůli vytvoření prvnímu deadlinu
            //this.dtActionsWS.Columns.Add(new DataColumn("textBoxPoznamka", typeof(string)));
            //this.DataGridViewWSAkce.Columns["textBoxPoznamka"].ReadOnly = true;

            //var btn = new DataGridViewButtonColumn
            //{
            //    Name = "buttonDatumUkonceni",
            //    HeaderText = @"Deadline",
            //    Width = 100,
            //    DataPropertyName = "textBoxDatumUkonceni",
            //    FlatStyle = FlatStyle.Flat,
            //    ReadOnly = true
            //};
            //this.DataGridViewWSAkce.Columns.Add(btn);

            //this.DataGridViewWSAkce.Columns["textBoxDatumUkonceni"].Visible = false;
            //this.DataGridViewWSAkce.Columns["textBoxPoznamka"].Visible = false;

            //this.dtActionsWS.Columns.Add(new DataColumn("textBoxKontrolaEfektivnosti", typeof(DateTime)));
            //this.DataGridViewWSAkce.Columns["textBoxKontrolaEfektivnosti"].Visible = false;

            //this.dtActionsWS.Columns.Add(new DataColumn("textBoxKontrolaEfektivnostiPuvodniDatum", typeof(string)));
            //this.DataGridViewWSAkce.Columns["textBoxKontrolaEfektivnostiPuvodniDatum"].Visible = false;

            //this.dtActionsWS.Columns.Add(new DataColumn("textBoxKontrolaEfektivnostiOdstranit", typeof(string)));
            //this.DataGridViewWSAkce.Columns["textBoxKontrolaEfektivnostiOdstranit"].Visible = false;

            //btn = new DataGridViewButtonColumn
            //{
            //    Name = "buttonKontrolaEfektivnosti",
            //    HeaderText = @"Effectiveness",
            //    Width = 120,
            //    DataPropertyName = "textBoxKontrolaEfektivnosti",
            //    FlatStyle = FlatStyle.Flat,
            //    ReadOnly = true
            //};
            //this.DataGridViewWSAkce.Columns.Add(btn);

            //this.dtActionsWS.Columns.Add(new DataColumn("comboBoxOddeleniId", typeof(int)));

            //var dtZam = DataTableConverter.ConvertToDataTable(zam);

            //DataRow drZam;
            //drZam = dtZam.NewRow();
            //drZam["Jmeno"] = "(select responsible)";
            //drZam["Id"] = 0;
            //dtZam.Rows.InsertAt(drZam, 0);

            //this.DataGridViewWSAkce.Columns["textBoxNapravnaOpatreni"].HeaderText = @"Corrective actions";
            //this.DataGridViewWSAkce.Columns["textBoxNapravnaOpatreni"].Width = 200;

            //var cbox1 = new DataGridViewComboBoxColumn
            //{
            //    Name = "comboBoxOdpovednaOsoba1",
            //    HeaderText = @"Responsible #1",
            //    Width = 200,
            //    FlatStyle = FlatStyle.Flat,
            //    DataSource = new BindingSource {DataSource = dtZam},
            //    ReadOnly = false,
            //    DisplayMember = "Jmeno",
            //    DataPropertyName = "comboBoxOdpovednaOsoba1Id",
            //    ValueMember = "Id"
            //};
            //this.DataGridViewWSAkce.Columns["comboBoxOdpovednaOsoba1Id"].Visible = false;
            //this.DataGridViewWSAkce.Columns.Add(cbox1); // Add new

            //var cbox2 = new DataGridViewComboBoxColumn
            //{
            //    Name = "comboBoxOdpovednaOsoba2",
            //    HeaderText = @"Responsible #2",
            //    Width = 200,
            //    FlatStyle = FlatStyle.Flat,
            //    DataSource = new BindingSource {DataSource = dtZam},
            //    ReadOnly = false,
            //    DisplayMember = "Jmeno",
            //    DataPropertyName = "comboBoxOdpovednaOsoba2Id",
            //    ValueMember = "Id"
            //};
            //this.DataGridViewWSAkce.Columns["comboBoxOdpovednaOsoba2Id"].Visible = false;
            //this.DataGridViewWSAkce.Columns.Add(cbox2);

            //var dtOdd = DataTableConverter.ConvertToDataTable(oddeleni);

            //DataRow drOdd;
            //drOdd = dtOdd.NewRow();
            //drOdd["Nazev"] = "(select department)";
            //drOdd["Id"] = 0;
            //dtOdd.Rows.InsertAt(drOdd, 0);

            //var department = new DataGridViewComboBoxColumn
            //{
            //    Name = "comboBoxOddeleni",
            //    HeaderText = @"Department",
            //    Width = 180,
            //    FlatStyle = FlatStyle.Flat,
            //    DataSource = new BindingSource {DataSource = dtOdd},
            //    ReadOnly = false,
            //    DisplayMember = "Nazev",
            //    DataPropertyName = "comboBoxOddeleniId",
            //    ValueMember = "Id"
            //};
            //this.DataGridViewWSAkce.Columns["comboBoxOddeleniId"].Visible = false;
            //this.DataGridViewWSAkce.Columns.Add(department);

            //this.dtActionsWS.Columns.Add(new DataColumn("textBoxPriloha", typeof(string)));

            //this.DataGridViewWSAkce.Columns["textBoxPriloha"].Visible = false;
            //btn = new DataGridViewButtonColumn
            //{
            //    Name = "buttonPriloha",
            //    HeaderText = @"Attachment",
            //    Width = 120,
            //    FlatStyle = FlatStyle.Flat,
            //    DataPropertyName = "prilohaTmp"
            //    //ReadOnly = true
            //};
            //this.DataGridViewWSAkce.Columns.Add(btn);
            //this.DataGridViewWSAkce.Columns["buttonPriloha"].DefaultCellStyle.Alignment =
            //    DataGridViewContentAlignment.MiddleCenter;

            //this.dtActionsWS.Columns.Add(new DataColumn("prilohaTmp", typeof(string)));
            //this.dtActionsWS.Columns["prilohaTmp"].Expression =
            //    string.Format("IIF([textBoxPriloha] = {0}, '', '(attachment)')", "''");

            //this.dtActionsWS.Columns.Add(new DataColumn("akceId", typeof(int)));
            //this.DataGridViewWSAkce.Columns["akceId"].Visible = false;

            //this.dtActionsWS.Columns.Add(new DataColumn("reopen", typeof(bool)));
            //this.DataGridViewWSAkce.Columns["reopen"].HeaderText = @"After reopen";
            //this.DataGridViewWSAkce.Columns["reopen"].Width = 120;

            //this.DataGridViewWSAkce.MultiSelect = false;
            //this.DataGridViewWSAkce.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            //this.DataGridViewWSAkce.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //this.DataGridViewWSAkce.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            //this.DataGridViewWSAkce.AllowUserToResizeRows = false;
            //this.DataGridViewWSAkce.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            //this.DataGridViewWSAkce.AllowUserToResizeColumns = false;
            //this.DataGridViewWSAkce.AllowUserToAddRows = false;
            //this.DataGridViewWSAkce.ReadOnly = false;
            //this.DataGridViewWSAkce.EditMode = DataGridViewEditMode.EditOnEnter;
            //this.DataGridViewWSAkce.AutoGenerateColumns = false;
            //this.DataGridViewWSAkce.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            //foreach (DataGridViewColumn column in this.DataGridViewWSAkce.Columns)
            //{
            //    column.SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
        }
    }
}
