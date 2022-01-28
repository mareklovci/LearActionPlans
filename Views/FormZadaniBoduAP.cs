using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using LearActionPlans.ViewModels;
using LearActionPlans.DataMappers;
using LearActionPlans.Utilities;
using LearActionPlans.Models;

namespace LearActionPlans.Views
{
    public partial class FormZadaniBoduAP : Form
    {
        private readonly FormNovyAkcniPlan.AkcniPlanTmp akcniPlany_;

        private readonly string cisloAPStr_;
        private DataTable dtActionsWM;
        private DataTable dtActionsWS;

        private bool novyBodAP;

        private int cisloRadkyDGVBody;

        private readonly BindingSource _bindingSourceWM = new BindingSource();
        private readonly BindingSource _bindingSourceWS = new BindingSource();

        private bool changedDGV;

        private List<int> opraveneAkce;

        private byte znovuOtevritAP;
        private DateTime? kontrolaEfektivnostiDatum;
        private string priloha;
        private DateTime? datumUkonceni;
        private string poznamkaDatumUkonceni;
        private bool deadLineZadan;

        public FormZadaniBoduAP(string cisloAPStr, FormNovyAkcniPlan.AkcniPlanTmp akcniPlany, int cisloRadkyDGV, bool novyBod)
        {
            InitializeComponent();
            dtActionsWM = new DataTable();
            dtActionsWS = new DataTable();

            cisloAPStr_ = cisloAPStr;
            akcniPlany_ = akcniPlany;

            cisloRadkyDGVBody = cisloRadkyDGV;
            novyBodAP = novyBod;

            opraveneAkce = new List<int>();
            kontrolaEfektivnostiDatum = null;
            priloha = string.Empty;
            datumUkonceni = null;
            poznamkaDatumUkonceni = string.Empty;
        }

        private void FormZadaniBoduAP_Load(object sender, EventArgs e)
        {
            if (NaplnitComboBoxZamestnanec1a2() == false)
            {
                ComboBoxOdpovednaOsoba1.Enabled = false;
                ComboBoxOdpovednaOsoba2.Enabled = false;
                //Nejsou dostupní žádní zaměstnanci.
                //Zadání nového Akčního plánu bude ukončeno.
                MessageBox.Show("No employees available." + (char)10 + "Entering a new Action plan will be completed.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                ComboBoxOdpovednaOsoba1.Enabled = true;
                ComboBoxOdpovednaOsoba2.Enabled = true;
            }

            if (NaplnitComboBoxOddeleni() == false)
            {
                ComboBoxOddeleni.Enabled = false;
                //Nejsou dostupné žádné projekty.
                //Zadání nového Akčního plánu bude ukončeno.
                MessageBox.Show("No departments available." + (char)10 + "Entering a new Action plan will be completed.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                ComboBoxOddeleni.Enabled = true;
            }

            labelDatumUkonceni.Text = string.Empty;
            labelEfektivita.Text = string.Empty;

            _bindingSourceWM.DataSource = dtActionsWM;
            DataGridViewWMAkce.DataSource = _bindingSourceWM;

            _bindingSourceWS.DataSource = dtActionsWS;
            DataGridViewWSAkce.DataSource = _bindingSourceWS;

            deadLineZadan = true;

            var znovuOtevrit = EditAPViewModel.GetZnovuOtevritAP(akcniPlany_.Id).ToList();
            //jestliže proměnná znovuotevritAP je rovna 0, byl AP znovuotevřen
            znovuOtevritAP = znovuOtevrit[0].ZnovuOtevrit;

            if (TestZamOdd() == true)
            {
                //var zam = ZadaniBoduAPViewModel.GetZamestnanciAll().ToList();
                //var oddeleni = ZadaniBoduAPViewModel.GetOddeleniAll().ToList();

                //CreateColumnsWM(zam, oddeleni);
                //CreateColumnsWS(zam, oddeleni);

                ZobrazeniDGV();

                ButtonUlozit.Enabled = false;
                changedDGV = false;

                if (akcniPlany_.APUzavren == true)
                {
                    TextBoxOdkazNaNormu.Enabled = false;
                    TextBoxHodnoceniNeshody.Enabled = false;
                    RichTextBoxPopisProblemu.Enabled = false;
                    RichTextBoxSkutecnaPricinaWM.Enabled = false;

                    ButtonNovaAkce.Visible = false;
                    ButtonOdstranitAkci.Visible = false;
                    ButtonUlozit.Visible = false;
                    ButtonZavrit.Text = "Close";
                }
                else
                {
                    TextBoxOdkazNaNormu.Enabled = true;
                    TextBoxHodnoceniNeshody.Enabled = true;
                    RichTextBoxPopisProblemu.Enabled = true;
                    RichTextBoxSkutecnaPricinaWM.Enabled = true;

                    ButtonUlozit.Visible = true;
                    ButtonUlozit.Text = "Save";
                    ButtonZavrit.Text = "Close";
                    ButtonNovaAkce.Visible = true;
                    ButtonOdstranitAkci.Visible = true;
                }
            }
        }

        private class Zam
        {
            public string Jmeno { get; set; }
            public int ZamestnanecId { get; set; }

            public Zam(string jmeno, int zamestnanecId)
            {
                Jmeno = jmeno;
                ZamestnanecId = zamestnanecId;
            }
        }

        private bool NaplnitComboBoxZamestnanec1a2()
        {
            var zamestnanci = ZadaniBoduAPViewModel.GetZamestnanciAll().ToList();

            if (zamestnanci.Count == 0)
            {
                return false;
            }
            else
            {
                List<Zam> zam1 = new List<Zam>
                {
                    new Zam("(select employee)", 0)
                };

                List<Zam> zam2 = new List<Zam>
                {
                    new Zam("(select employee)", 0)
                };

                foreach (var z in zamestnanci)
                {
                    zam1.Add(new Zam(z.Jmeno, z.ZamestnanecId));
                    zam2.Add(new Zam(z.Jmeno, z.ZamestnanecId));
                }
                ComboBoxOdpovednaOsoba1.DataSource = zam1;
                ComboBoxOdpovednaOsoba2.DataSource = zam2;
                ComboBoxOdpovednaOsoba1.DisplayMember = "Jmeno";
                ComboBoxOdpovednaOsoba2.DisplayMember = "Jmeno";
                ComboBoxOdpovednaOsoba1.ValueMember = "ZamestnanecId";
                ComboBoxOdpovednaOsoba2.ValueMember = "ZamestnanecId";
                ComboBoxOdpovednaOsoba1.SelectedIndex = 0;
                ComboBoxOdpovednaOsoba2.SelectedIndex = 0;
                return true;
            }
        }

        private class Oddeleni
        {
            public string NazevOddeleni { get; set; }
            public int OddeleniId { get; set; }

            public Oddeleni(string nazevOddeleni, int oddeleniId)
            {
                NazevOddeleni = nazevOddeleni;
                OddeleniId = oddeleniId;
            }
        }

        private bool NaplnitComboBoxOddeleni()
        {
            var oddeleni = ZadaniBoduAPViewModel.GetOddeleniAll().ToList();

            if (oddeleni.Count == 0)
            {
                return false;
            }
            else
            {
                List<Oddeleni> odd = new List<Oddeleni>
                {
                    new Oddeleni("(select a department)", 0)
                };
                foreach (var o in oddeleni)
                {
                    odd.Add(new Oddeleni(o.Nazev, o.OddeleniId_));
                }
                ComboBoxOddeleni.DataSource = odd;
                ComboBoxOddeleni.DisplayMember = "NazevOddeleni";
                ComboBoxOddeleni.ValueMember = "OddeleniId";
                ComboBoxOddeleni.SelectedIndex = 0;
                return true;
            }
        }

        private bool TestZamOdd()
        {
            //pokud nebudou zaměstnanci nebo oddělení vrátím false a nemohu pokračovat dál
            var zam = ZadaniBoduAPViewModel.GetZamestnanciAll().ToList();
            if (zam[0] == null)
            {
                MessageBox.Show("No Employees available.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            var oddeleni = ZadaniBoduAPViewModel.GetOddeleniAll().ToList();
            if (oddeleni[0] == null)
            {
                MessageBox.Show("No Departments are available.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void ZobrazeniDGV()
        {
            //DataGridViewWMAkce.DataSource = null;
            //DataGridViewWSAkce.DataSource = null;
            //DataGridViewWMAkce.DataSource = _bindingSourceWM;
            //DataGridViewWSAkce.DataSource = _bindingSourceWS;
            //DataGridViewWMAkce.Rows.Clear();
            //DataGridViewWMAkce.Refresh();

            if (novyBodAP == false)
            {
                TextBoxOdkazNaNormu.Text = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].OdkazNaNormu;
                TextBoxHodnoceniNeshody.Text = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].HodnoceniNeshody;
                RichTextBoxPopisProblemu.Text = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].PopisProblemu;
                ComboBoxOdpovednaOsoba1.SelectedValue = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].OdpovednaOsoba1Id;
                if (FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].OdpovednaOsoba2Id == null) { }
                else
                {
                    ComboBoxOdpovednaOsoba2.SelectedValue = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].OdpovednaOsoba2Id;
                }
                if (FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].OddeleniId == null) { }
                else
                {
                    ComboBoxOddeleni.SelectedValue = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].OddeleniId;
                }
                if (FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].Priloha == null) { }
                else
                {
                    
                }
                labelDatumUkonceni.Text = string.Empty;
                labelEfektivita.Text = string.Empty;

                if (FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].UkonceniBodAP == null)
                {
                    deadLineZadan = false;
                }
                else
                {
                    int i = 0;
                    List<UkonceniBodAP> copyBodAP = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].UkonceniBodAP.ToList();
                    foreach (var c in copyBodAP)
                    {
                        if (i == 0)
                        {
                            if (c.StavZadosti == 3 || c.StavZadosti == 6)
                            {
                                deadLineZadan = false;
                            }
                        }
                        if (c.StavZadosti == 1 || c.StavZadosti == 4 || c.StavZadosti == 5)
                        {
                            labelDatumUkonceni.Text = c.DatumUkonceni.ToShortDateString();
                        }
                        i++;
                    }
                }
                //if (FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].DatumUkonceni == null) { }
                //else
                //{
                //    labelDatumUkonceni.Text = Convert.ToDateTime(FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].DatumUkonceni).ToShortDateString();
                //}
                if (FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].KontrolaEfektivnosti == null) { }
                else
                {
                    labelEfektivita.Text = Convert.ToDateTime(FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].KontrolaEfektivnosti).ToShortDateString();
                    ZablokovatPole();
                }
                RichTextBoxSkutecnaPricinaWM.Text = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].SkutecnaPricinaWM;
                RichTextBoxNapravnaOpatreniWM.Text = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].NapravnaOpatreniWM;
                RichTextBoxSkutecnaPricinaWS.Text = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].SkutecnaPricinaWS;
                RichTextBoxNapravnaOpatreniWS.Text = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].NapravnaOpatreniWS;

            }


            ComboBoxOdpovednaOsoba1.SelectedIndexChanged += ComboBoxOdpovednaOsoba1_SelectedIndexChanged;
            ComboBoxOddeleni.SelectedIndexChanged += ComboBoxOddeleni_SelectedIndexChanged;
        }

        private void CreateColumnsWM<T>(IEnumerable<T> zam, IEnumerable<T> oddeleni)
        {
            dtActionsWM.Columns.Add(new DataColumn("textBoxNapravnaOpatreni", typeof(string)));
            dtActionsWM.Columns.Add(new DataColumn("comboBoxOdpovednaOsoba1Id", typeof(int)));
            dtActionsWM.Columns.Add(new DataColumn("comboBoxOdpovednaOsoba2Id", typeof(int)));

            //to je kvůli vytvoření prvnímu deadlinu
            dtActionsWM.Columns.Add(new DataColumn("textBoxDatumUkonceni", typeof(DateTime)));
            DataGridViewWMAkce.Columns["textBoxDatumUkonceni"].ReadOnly = true;

            //to je kvůli vytvoření prvnímu deadlinu
            dtActionsWM.Columns.Add(new DataColumn("textBoxPoznamka", typeof(string)));
            DataGridViewWMAkce.Columns["textBoxPoznamka"].ReadOnly = true;

            var btn = new DataGridViewButtonColumn
            {
                Name = "buttonDatumUkonceni",
                HeaderText = @"Deadline",
                Width = 100,
                DataPropertyName = "textBoxDatumUkonceni",
                FlatStyle = FlatStyle.Flat,
                ReadOnly = true
            };
            DataGridViewWMAkce.Columns.Add(btn);

            DataGridViewWMAkce.Columns["textBoxDatumUkonceni"].Visible = false;
            DataGridViewWMAkce.Columns["textBoxPoznamka"].Visible = false;

            dtActionsWM.Columns.Add(new DataColumn("textBoxKontrolaEfektivnosti", typeof(DateTime)));
            DataGridViewWMAkce.Columns["textBoxKontrolaEfektivnosti"].Visible = false;

            dtActionsWM.Columns.Add(new DataColumn("textBoxKontrolaEfektivnostiPuvodniDatum", typeof(string)));
            DataGridViewWMAkce.Columns["textBoxKontrolaEfektivnostiPuvodniDatum"].Visible = false;

            dtActionsWM.Columns.Add(new DataColumn("textBoxKontrolaEfektivnostiOdstranit", typeof(string)));
            DataGridViewWMAkce.Columns["textBoxKontrolaEfektivnostiOdstranit"].Visible = false;

            btn = new DataGridViewButtonColumn
            {
                Name = "buttonKontrolaEfektivnosti",
                HeaderText = @"Effectiveness",
                Width = 120,
                DataPropertyName = "textBoxKontrolaEfektivnosti",
                FlatStyle = FlatStyle.Flat,
                ReadOnly = true
            };
            DataGridViewWMAkce.Columns.Add(btn);

            dtActionsWM.Columns.Add(new DataColumn("comboBoxOddeleniId", typeof(int)));

            var dtZam = DataTableConverter.ConvertToDataTable(zam);

            DataRow drZam;
            drZam = dtZam.NewRow();
            drZam["Jmeno"] = "(select responsible)";
            drZam["ZamestnanecId"] = 0;
            dtZam.Rows.InsertAt(drZam, 0);

            DataGridViewWMAkce.Columns["textBoxNapravnaOpatreni"].HeaderText = @"Corrective actions";
            DataGridViewWMAkce.Columns["textBoxNapravnaOpatreni"].Width = 200;

            var cbox1 = new DataGridViewComboBoxColumn
            {
                Name = "comboBoxOdpovednaOsoba1",
                HeaderText = @"Responsible #1",
                Width = 200,
                FlatStyle = FlatStyle.Flat,
                DataSource = new BindingSource { DataSource = dtZam },
                ReadOnly = false,
                DisplayMember = "Jmeno",
                DataPropertyName = "comboBoxOdpovednaOsoba1Id",
                ValueMember = "ZamestnanecId"
            };
            DataGridViewWMAkce.Columns["comboBoxOdpovednaOsoba1Id"].Visible = false;
            DataGridViewWMAkce.Columns.Add(cbox1); // Add new 

            var cbox2 = new DataGridViewComboBoxColumn
            {
                Name = "comboBoxOdpovednaOsoba2",
                HeaderText = @"Responsible #2",
                Width = 200,
                FlatStyle = FlatStyle.Flat,
                DataSource = new BindingSource { DataSource = dtZam },
                ReadOnly = false,
                DisplayMember = "Jmeno",
                DataPropertyName = "comboBoxOdpovednaOsoba2Id",
                ValueMember = "ZamstnanecId"
            };
            DataGridViewWMAkce.Columns["comboBoxOdpovednaOsoba2Id"].Visible = false;
            DataGridViewWMAkce.Columns.Add(cbox2);

            var dtOdd = DataTableConverter.ConvertToDataTable(oddeleni);

            DataRow drOdd;
            drOdd = dtOdd.NewRow();
            drOdd["Nazev"] = "(select department)";
            drOdd["Id"] = 0;
            dtOdd.Rows.InsertAt(drOdd, 0);

            var department = new DataGridViewComboBoxColumn
            {
                Name = "comboBoxOddeleni",
                HeaderText = @"Department",
                Width = 180,
                FlatStyle = FlatStyle.Flat,
                DataSource = new BindingSource { DataSource = dtOdd },
                ReadOnly = false,
                DisplayMember = "Nazev",
                DataPropertyName = "comboBoxOddeleniId",
                ValueMember = "Id"
            };
            DataGridViewWMAkce.Columns["comboBoxOddeleniId"].Visible = false;
            DataGridViewWMAkce.Columns.Add(department);
            
            dtActionsWM.Columns.Add(new DataColumn("textBoxPriloha", typeof(string)));

            DataGridViewWMAkce.Columns["textBoxPriloha"].Visible = false;
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
            DataGridViewWMAkce.Columns.Add(btn);
            DataGridViewWMAkce.Columns["buttonPriloha"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtActionsWM.Columns.Add(new DataColumn("prilohaTmp", typeof(string)));
            dtActionsWM.Columns["prilohaTmp"].Expression = string.Format("IIF([textBoxPriloha] = {0}, '', '(attachment)')", "''");

            dtActionsWM.Columns.Add(new DataColumn("akceId", typeof(int)));
            DataGridViewWMAkce.Columns["akceId"].Visible = false;

            dtActionsWM.Columns.Add(new DataColumn("reopen", typeof(bool)));
            DataGridViewWMAkce.Columns["reopen"].HeaderText = @"After reopen";
            DataGridViewWMAkce.Columns["reopen"].Width = 120;

            DataGridViewWMAkce.MultiSelect = false;
            DataGridViewWMAkce.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            DataGridViewWMAkce.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            DataGridViewWMAkce.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            DataGridViewWMAkce.AllowUserToResizeRows = false;
            DataGridViewWMAkce.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            DataGridViewWMAkce.AllowUserToResizeColumns = false;
            DataGridViewWMAkce.AllowUserToAddRows = false;
            DataGridViewWMAkce.ReadOnly = false;
            DataGridViewWMAkce.EditMode = DataGridViewEditMode.EditOnEnter;
            DataGridViewWMAkce.AutoGenerateColumns = false;
            DataGridViewWMAkce.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            foreach (DataGridViewColumn column in DataGridViewWMAkce.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void CreateColumnsWS<T>(IEnumerable<T> zam, IEnumerable<T> oddeleni)
        {
            dtActionsWS.Columns.Add(new DataColumn("textBoxNapravnaOpatreni", typeof(string)));
            dtActionsWS.Columns.Add(new DataColumn("comboBoxOdpovednaOsoba1Id", typeof(int)));
            dtActionsWS.Columns.Add(new DataColumn("comboBoxOdpovednaOsoba2Id", typeof(int)));

            //to je kvůli vytvoření prvnímu deadlinu
            dtActionsWS.Columns.Add(new DataColumn("textBoxDatumUkonceni", typeof(DateTime)));
            DataGridViewWSAkce.Columns["textBoxDatumUkonceni"].ReadOnly = true;

            //to je kvůli vytvoření prvnímu deadlinu
            dtActionsWS.Columns.Add(new DataColumn("textBoxPoznamka", typeof(string)));
            DataGridViewWSAkce.Columns["textBoxPoznamka"].ReadOnly = true;

            var btn = new DataGridViewButtonColumn
            {
                Name = "buttonDatumUkonceni",
                HeaderText = @"Deadline",
                Width = 100,
                DataPropertyName = "textBoxDatumUkonceni",
                FlatStyle = FlatStyle.Flat,
                ReadOnly = true
            };
            DataGridViewWSAkce.Columns.Add(btn);

            DataGridViewWSAkce.Columns["textBoxDatumUkonceni"].Visible = false;
            DataGridViewWSAkce.Columns["textBoxPoznamka"].Visible = false;

            dtActionsWS.Columns.Add(new DataColumn("textBoxKontrolaEfektivnosti", typeof(DateTime)));
            DataGridViewWSAkce.Columns["textBoxKontrolaEfektivnosti"].Visible = false;

            dtActionsWS.Columns.Add(new DataColumn("textBoxKontrolaEfektivnostiPuvodniDatum", typeof(string)));
            DataGridViewWSAkce.Columns["textBoxKontrolaEfektivnostiPuvodniDatum"].Visible = false;

            dtActionsWS.Columns.Add(new DataColumn("textBoxKontrolaEfektivnostiOdstranit", typeof(string)));
            DataGridViewWSAkce.Columns["textBoxKontrolaEfektivnostiOdstranit"].Visible = false;

            btn = new DataGridViewButtonColumn
            {
                Name = "buttonKontrolaEfektivnosti",
                HeaderText = @"Effectiveness",
                Width = 120,
                DataPropertyName = "textBoxKontrolaEfektivnosti",
                FlatStyle = FlatStyle.Flat,
                ReadOnly = true
            };
            DataGridViewWSAkce.Columns.Add(btn);

            dtActionsWS.Columns.Add(new DataColumn("comboBoxOddeleniId", typeof(int)));

            var dtZam = DataTableConverter.ConvertToDataTable(zam);

            DataRow drZam;
            drZam = dtZam.NewRow();
            drZam["Jmeno"] = "(select responsible)";
            drZam["Id"] = 0;
            dtZam.Rows.InsertAt(drZam, 0);

            DataGridViewWSAkce.Columns["textBoxNapravnaOpatreni"].HeaderText = @"Corrective actions";
            DataGridViewWSAkce.Columns["textBoxNapravnaOpatreni"].Width = 200;

            var cbox1 = new DataGridViewComboBoxColumn
            {
                Name = "comboBoxOdpovednaOsoba1",
                HeaderText = @"Responsible #1",
                Width = 200,
                FlatStyle = FlatStyle.Flat,
                DataSource = new BindingSource { DataSource = dtZam },
                ReadOnly = false,
                DisplayMember = "Jmeno",
                DataPropertyName = "comboBoxOdpovednaOsoba1Id",
                ValueMember = "Id"
            };
            DataGridViewWSAkce.Columns["comboBoxOdpovednaOsoba1Id"].Visible = false;
            DataGridViewWSAkce.Columns.Add(cbox1); // Add new 

            var cbox2 = new DataGridViewComboBoxColumn
            {
                Name = "comboBoxOdpovednaOsoba2",
                HeaderText = @"Responsible #2",
                Width = 200,
                FlatStyle = FlatStyle.Flat,
                DataSource = new BindingSource { DataSource = dtZam },
                ReadOnly = false,
                DisplayMember = "Jmeno",
                DataPropertyName = "comboBoxOdpovednaOsoba2Id",
                ValueMember = "Id"
            };
            DataGridViewWSAkce.Columns["comboBoxOdpovednaOsoba2Id"].Visible = false;
            DataGridViewWSAkce.Columns.Add(cbox2);

            var dtOdd = DataTableConverter.ConvertToDataTable(oddeleni);

            DataRow drOdd;
            drOdd = dtOdd.NewRow();
            drOdd["Nazev"] = "(select department)";
            drOdd["Id"] = 0;
            dtOdd.Rows.InsertAt(drOdd, 0);

            var department = new DataGridViewComboBoxColumn
            {
                Name = "comboBoxOddeleni",
                HeaderText = @"Department",
                Width = 180,
                FlatStyle = FlatStyle.Flat,
                DataSource = new BindingSource { DataSource = dtOdd },
                ReadOnly = false,
                DisplayMember = "Nazev",
                DataPropertyName = "comboBoxOddeleniId",
                ValueMember = "Id"
            };
            DataGridViewWSAkce.Columns["comboBoxOddeleniId"].Visible = false;
            DataGridViewWSAkce.Columns.Add(department);

            dtActionsWS.Columns.Add(new DataColumn("textBoxPriloha", typeof(string)));

            DataGridViewWSAkce.Columns["textBoxPriloha"].Visible = false;
            btn = new DataGridViewButtonColumn
            {
                Name = "buttonPriloha",
                HeaderText = @"Attachment",
                Width = 120,
                FlatStyle = FlatStyle.Flat,
                DataPropertyName = "prilohaTmp"
                //ReadOnly = true
            };
            DataGridViewWSAkce.Columns.Add(btn);
            DataGridViewWSAkce.Columns["buttonPriloha"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtActionsWS.Columns.Add(new DataColumn("prilohaTmp", typeof(string)));
            dtActionsWS.Columns["prilohaTmp"].Expression = string.Format("IIF([textBoxPriloha] = {0}, '', '(attachment)')", "''");

            dtActionsWS.Columns.Add(new DataColumn("akceId", typeof(int)));
            DataGridViewWSAkce.Columns["akceId"].Visible = false;

            dtActionsWS.Columns.Add(new DataColumn("reopen", typeof(bool)));
            DataGridViewWSAkce.Columns["reopen"].HeaderText = @"After reopen";
            DataGridViewWSAkce.Columns["reopen"].Width = 120;

            DataGridViewWSAkce.MultiSelect = false;
            DataGridViewWSAkce.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            DataGridViewWSAkce.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            DataGridViewWSAkce.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            DataGridViewWSAkce.AllowUserToResizeRows = false;
            DataGridViewWSAkce.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            DataGridViewWSAkce.AllowUserToResizeColumns = false;
            DataGridViewWSAkce.AllowUserToAddRows = false;
            DataGridViewWSAkce.ReadOnly = false;
            DataGridViewWSAkce.EditMode = DataGridViewEditMode.EditOnEnter;
            DataGridViewWSAkce.AutoGenerateColumns = false;
            DataGridViewWSAkce.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            foreach (DataGridViewColumn column in DataGridViewWSAkce.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //private void UlozitAkce()
        //{
        //    bool ulozit = true;

        //    //test na vyplnění akcí
        //    DataGridViewRowCollection radkyDGVAkce = DataGridViewWMAkce.Rows;
        //    for (int i = 0; i < 2; i++)
        //    {
        //        foreach (DataGridViewRow row in radkyDGVAkce)
        //        {
        //            if (string.IsNullOrWhiteSpace(Convert.ToString(row.Cells["textBoxNapravnaOpatreni"].Value)) == true)
        //            {
        //                row.Cells["textBoxNapravnaOpatreni"].Style.BackColor = Color.Yellow;
        //                ulozit = false;
        //            }
        //            else
        //            {
        //                row.Cells["textBoxNapravnaOpatreni"].Style.BackColor = SystemColors.Window;
        //            }

        //            if (Convert.ToInt32(row.Cells["comboBoxOdpovednaOsoba1"].Value) == 0)
        //            {
        //                row.Cells["comboBoxOdpovednaOsoba1"].Style.BackColor = Color.Yellow;
        //                ulozit = false;
        //            }
        //            else
        //            {
        //                row.Cells["comboBoxOdpovednaOsoba1"].Style.BackColor = SystemColors.Window;
        //            }

        //            if (string.IsNullOrWhiteSpace(Convert.ToString(row.Cells["buttonDatumUkonceni"].Value)) == true)
        //            {
        //                row.Cells["buttonDatumUkonceni"].Style.BackColor = Color.Yellow;
        //                ulozit = false;
        //            }
        //            else
        //            {
        //                row.Cells["buttonDatumUkonceni"].Style.BackColor = SystemColors.Window;
        //            }

        //            radkyDGVAkce = DataGridViewWSAkce.Rows;
        //        }
        //    }

        //    if (ulozit == false)
        //        MessageBox.Show("The yellow cells must be filled.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //    if (ulozit == true)
        //    {
        //        BodyAP ulozitBodAP;

        //        ulozitBodAP = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody];

        //        DataRowCollection radkyAkci;
        //        byte typAkce;

        //        bool reopen;
        //        if (znovuOtevritAP == 0)
        //        {
        //            //akční plán je znovuotevřen
        //            reopen = true;
        //        }
        //        else
        //            reopen = false;

        //        for (int i = 0; i < 2; i++)
        //        {
        //            if (i == 0)
        //            {
        //                radkyAkci = dtActionsWM.Rows;
        //                typAkce = 1;
        //            }
        //            else
        //            {
        //                radkyAkci = dtActionsWS.Rows;
        //                typAkce = 2;
        //            }

        //            foreach (DataRow dtRow in radkyAkci)
        //            {
        //                var podminkaOsoba2 = Convert.ToInt32(dtRow["comboBoxOdpovednaOsoba2Id"]) == 0;
        //                var podminkaOddeleni = Convert.ToInt32(dtRow["comboBoxOddeleniId"]) == 0;
        //                var podminkaKontrolaEfektivity = dtRow["textBoxKontrolaEfektivnosti"] == DBNull.Value;
        //                var podminkaPriloha = dtRow["textBoxPriloha"] == null;

        //                //if (dtRow["akceId"] == DBNull.Value)
        //                if (Convert.ToInt32(dtRow["akceId"]) == 0)
        //                {
        //                    //nově vytvořená akce
        //                    ulozitBodAP.TypAkce.Add(new Akce(
        //                        Convert.ToString(dtRow["textBoxNapravnaOpatreni"]),
        //                        Convert.ToInt32(dtRow["comboBoxOdpovednaOsoba1Id"]),
        //                        podminkaOsoba2 ? (int?)null : Convert.ToInt32(dtRow["comboBoxOdpovednaOsoba2Id"]),
        //                        podminkaKontrolaEfektivity ? (DateTime?)null : Convert.ToDateTime(dtRow["textBoxKontrolaEfektivnosti"]),
        //                        (DateTime?)null,
        //                        null,
        //                        podminkaOddeleni ? (int?)null : Convert.ToInt32(dtRow["comboBoxOddeleniId"]),
        //                        podminkaPriloha ? null : Convert.ToString(dtRow["textBoxPriloha"]),
        //                        typAkce,
        //                        1,
        //                        false,
        //                        reopen));
        //                    string poznamka;
        //                    if (string.IsNullOrWhiteSpace(Convert.ToString(dtRow["textBoxPoznamka"])) == true)
        //                        poznamka = null;
        //                    else
        //                        poznamka = Convert.ToString(dtRow["textBoxPoznamka"]);
        //                    var posledniAkce = ulozitBodAP.TypAkce.Last();
        //                    posledniAkce.UkonceniAkce.Add(new UkonceniAkce(Convert.ToDateTime(dtRow["textBoxDatumUkonceni"]), poznamka, null, 1, false));
        //                }
        //                else
        //                {
        //                    int akceId = Convert.ToInt32(dtRow["akceId"]);

        //                    foreach (var akce in ulozitBodAP.TypAkce)
        //                    {
        //                        if (akce.Id == akceId)
        //                        {
        //                            akce.NapravnaOpatreni = Convert.ToString(dtRow["textBoxNapravnaOpatreni"]);
        //                            akce.OdpovednaOsoba1Id = Convert.ToInt32(dtRow["comboBoxOdpovednaOsoba1Id"]);
        //                            if (Convert.ToInt32(dtRow["comboBoxOdpovednaOsoba2Id"]) == 0)
        //                                akce.OdpovednaOsoba2Id = null;
        //                            else
        //                                akce.OdpovednaOsoba2Id = Convert.ToInt32(dtRow["comboBoxOdpovednaOsoba2Id"]);
        //                            if (dtRow["textBoxKontrolaEfektivnosti"] == DBNull.Value)
        //                                akce.KontrolaEfektivnosti = null;
        //                            else
        //                                akce.KontrolaEfektivnosti = Convert.ToDateTime(dtRow["textBoxKontrolaEfektivnosti"]);
        //                            if (Convert.ToInt32(dtRow["comboBoxOddeleniId"]) == 0)
        //                                akce.Oddeleni_Id = null;
        //                            else
        //                                akce.Oddeleni_Id = Convert.ToInt32(dtRow["comboBoxOddeleniId"]);
        //                            if (dtRow["textBoxPriloha"] == null)
        //                                akce.Priloha = null;
        //                            else
        //                                akce.Priloha = Convert.ToString(dtRow["textBoxPriloha"]);

        //                            string poznamka;
        //                            if (string.IsNullOrWhiteSpace(Convert.ToString(dtRow["textBoxPoznamka"])) == true)
        //                                poznamka = null;
        //                            else
        //                                poznamka = Convert.ToString(dtRow["textBoxPoznamka"]);
        //                            akce.UkonceniAkce.Add(new UkonceniAkce(Convert.ToDateTime(dtRow["textBoxDatumUkonceni"]), poznamka, null, 1, false));

        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        //zavře se formulář
        //        //pokud byly smazány akce, budou odstraněny fyzicky
        //        dtActionsWM.AcceptChanges();
        //        dtActionsWS.AcceptChanges();
        //        AkceDataMapper.UpdateAkce(cisloRadkyDGVBody, opraveneAkce);

        //        //obarví řádky, které patří majiteli akce
        //        DataGridViewRowCollection radkyDGV = DataGridViewWMAkce.Rows;
        //        var nazevAktualniStranky = TabControlAkce.SelectedTab;
        //        string nazevDruheStranky = "tabPageWM";

        //        if (nazevAktualniStranky.Name == "tabPageWM")
        //        {
        //            radkyDGV = DataGridViewWMAkce.Rows;
        //            nazevDruheStranky = "tabPageWS";
        //        }
        //        if (nazevAktualniStranky.Name == "tabPageWS")
        //        {
        //            radkyDGV = DataGridViewWSAkce.Rows;
        //            nazevDruheStranky = "tabPageWM";
        //        }

        //        for (int i = 0; i < 2; i++)
        //        {
        //            foreach (DataGridViewRow row in radkyDGV)
        //            {
        //                opraveneAkce.Add(Convert.ToInt32(row.Cells["akceId"].Value));

        //                if (akcniPlany_.APUzavren == true)
        //                {
        //                    row.Cells["textBoxNapravnaOpatreni"].ReadOnly = true;
        //                    row.Cells["comboBoxOdpovednaOsoba1"].ReadOnly = true;
        //                    row.Cells["comboBoxOdpovednaOsoba2"].ReadOnly = true;
        //                    row.Cells["comboBoxOddeleni"].ReadOnly = true;
        //                }
        //                else
        //                {
        //                    row.Cells["textBoxNapravnaOpatreni"].ReadOnly = false;
        //                    row.Cells["comboBoxOdpovednaOsoba1"].ReadOnly = false;
        //                    row.Cells["comboBoxOdpovednaOsoba2"].ReadOnly = false;
        //                    row.Cells["comboBoxOddeleni"].ReadOnly = false;
        //                }
        //                if (row.Cells["textBoxKontrolaEfektivnosti"].Value == DBNull.Value) { }
        //                else
        //                {
        //                    //ukončené akce - je nastaven datum efektivnosti
        //                    row.DefaultCellStyle.BackColor = Color.LightGreen;
        //                }
        //            }
        //            if (nazevDruheStranky == "tabPageWM")
        //            {
        //                TabControlAkce.SelectedTab = tabPageWM;
        //                radkyDGV = DataGridViewWMAkce.Rows;
        //            }
        //            if (nazevDruheStranky == "tabPageWS")
        //            {
        //                TabControlAkce.SelectedTab = tabPageWS;
        //                radkyDGV = DataGridViewWSAkce.Rows;
        //            }
        //        }
        //        TabControlAkce.SelectedTab = nazevAktualniStranky;

        //        BodyAPDataMapper.InsertUpdateBodyAP(ulozitBodAP);
        //    }
        //}

        private void UlozitBodAP()
        {
            bool ulozit = true;

            //nejdřív proběhne test na vyplnění položek
            RichTextBoxPopisProblemu.BackColor = SystemColors.Window;
            //RichTextBoxSkutecnaPricinaWM.BackColor = SystemColors.Window;
            //RichTextBoxNapravnaOpatreniWM.BackColor = SystemColors.Window;
            //RichTextBoxSkutecnaPricinaWS.BackColor = SystemColors.Window;
            //RichTextBoxNapravnaOpatreniWS.BackColor = SystemColors.Window;
            if (string.IsNullOrWhiteSpace(RichTextBoxPopisProblemu.Text) == true)
            {
                ulozit = false;
                RichTextBoxPopisProblemu.BackColor = Color.Yellow;
            }

            if (ulozit == false)
                MessageBox.Show("The yellow cells must be filled.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (Convert.ToInt32(ComboBoxOdpovednaOsoba1.SelectedValue) == 0)
            {
                ulozit = false;
                MessageBox.Show("You must select a responsible employee #1.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (Convert.ToInt32(ComboBoxOddeleni.SelectedValue) == 0)
            {
                ulozit = false;
                MessageBox.Show("You must select a department.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (novyBodAP == true)
            {
                if (datumUkonceni == null)
                {
                    ulozit = false;
                    MessageBox.Show("You must fill in the deadline.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (ulozit == true)
            {
                bool reopen;
                if (znovuOtevritAP == 0)
                {
                    //akční plán je znovuotevřen
                    reopen = true;
                }
                else
                    reopen = false;

                //proměnnou  ulozitBodAP asi potřebovat nebudu
                BodAP ulozitBodAP;

                if (novyBodAP == false)
                {
                    //aktualizace stávajícího bodu
                    ulozitBodAP = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody];

                    ulozitBodAP.OdkazNaNormu = TextBoxOdkazNaNormu.Text;
                    ulozitBodAP.HodnoceniNeshody = TextBoxHodnoceniNeshody.Text;
                    ulozitBodAP.PopisProblemu = RichTextBoxPopisProblemu.Text;
                    ulozitBodAP.SkutecnaPricinaWM = RichTextBoxSkutecnaPricinaWM.Text;
                    ulozitBodAP.NapravnaOpatreniWM = RichTextBoxNapravnaOpatreniWM.Text;
                    ulozitBodAP.SkutecnaPricinaWS = RichTextBoxSkutecnaPricinaWS.Text;
                    ulozitBodAP.NapravnaOpatreniWS = RichTextBoxNapravnaOpatreniWS.Text;
                    ulozitBodAP.OdpovednaOsoba1Id = Convert.ToInt32(ComboBoxOdpovednaOsoba1.SelectedValue);
                    if (ComboBoxOdpovednaOsoba2.SelectedIndex == 0)
                        ulozitBodAP.OdpovednaOsoba2Id = 0;
                    else
                        ulozitBodAP.OdpovednaOsoba2Id = Convert.ToInt32(ComboBoxOdpovednaOsoba2.SelectedValue);
                    if (ComboBoxOddeleni.SelectedIndex == 0)
                        ulozitBodAP.OddeleniId = 0;
                    else
                        ulozitBodAP.OddeleniId = Convert.ToInt32(ComboBoxOddeleni.SelectedValue);
                    if (string.IsNullOrEmpty(RichTextBoxSkutecnaPricinaWM.Text))
                        ulozitBodAP.SkutecnaPricinaWM = null;
                    else
                        ulozitBodAP.SkutecnaPricinaWM = RichTextBoxSkutecnaPricinaWM.Text;
                    if (string.IsNullOrEmpty(RichTextBoxNapravnaOpatreniWM.Text))
                        ulozitBodAP.NapravnaOpatreniWM = null;
                    else
                        ulozitBodAP.NapravnaOpatreniWM = RichTextBoxNapravnaOpatreniWM.Text;
                    if (string.IsNullOrEmpty(RichTextBoxSkutecnaPricinaWS.Text))
                        ulozitBodAP.SkutecnaPricinaWS = null;
                    else
                        ulozitBodAP.SkutecnaPricinaWS = RichTextBoxSkutecnaPricinaWS.Text;
                    if (string.IsNullOrEmpty(RichTextBoxNapravnaOpatreniWS.Text))
                        ulozitBodAP.NapravnaOpatreniWS = null;
                    else
                        ulozitBodAP.NapravnaOpatreniWS = RichTextBoxNapravnaOpatreniWS.Text;

                }
                else
                {
                    int? odpovednaOsoba2Id;
                    if (Convert.ToInt32(ComboBoxOdpovednaOsoba2.SelectedValue) == 0)
                        odpovednaOsoba2Id = null;
                    else
                        odpovednaOsoba2Id = Convert.ToInt32(ComboBoxOdpovednaOsoba2.SelectedValue);

                    int? oddeleniId;
                    if (Convert.ToInt32(ComboBoxOddeleni.SelectedValue) == 0)
                        oddeleniId = null;
                    else
                        oddeleniId = Convert.ToInt32(ComboBoxOddeleni.SelectedValue);

                    //založí nový bod
                    FormPrehledBoduAP.bodyAP.Add(new BodAP(akcniPlany_.Id,
                            FormPrehledBoduAP.bodyAP.Count + 1,
                            DateTime.Now,
                            TextBoxOdkazNaNormu.Text,
                            TextBoxHodnoceniNeshody.Text,
                            RichTextBoxPopisProblemu.Text,
                            RichTextBoxSkutecnaPricinaWM.Text,
                            RichTextBoxNapravnaOpatreniWM.Text,
                            RichTextBoxSkutecnaPricinaWS.Text,
                            RichTextBoxNapravnaOpatreniWS.Text,
                            Convert.ToInt32(ComboBoxOdpovednaOsoba1.SelectedValue),
                            odpovednaOsoba2Id,
                            kontrolaEfektivnostiDatum,
                            oddeleniId,
                            priloha,
                            datumUkonceni,
                            poznamkaDatumUkonceni,
                            reopen,
                            false,
                            1));

                    ulozitBodAP = FormPrehledBoduAP.bodyAP.Last();
                }

                //------------------------------------------------------------------------------------------------------------------------------------
                //DataRowCollection radkyAkci;
                //byte typAkce;

                //for (int i = 0; i < 2; i++)
                //{
                //    if (i == 0)
                //    {
                //        radkyAkci = dtActionsWM.Rows;
                //        typAkce = 1;
                //    }
                //    else
                //    {
                //        radkyAkci = dtActionsWS.Rows;
                //        typAkce = 2;
                //    }

                //    foreach (DataRow dtRow in radkyAkci)
                //    {
                //        var podminkaOsoba2 = Convert.ToInt32(dtRow["comboBoxOdpovednaOsoba2Id"]) == 0;
                //        var podminkaOddeleni = Convert.ToInt32(dtRow["comboBoxOddeleniId"]) == 0;
                //        var podminkaKontrolaEfektivity = dtRow["textBoxKontrolaEfektivnosti"] == DBNull.Value;
                //        var podminkaKontrolaEfektivityPuvodniDatum = dtRow["textBoxKontrolaEfektivnostiPuvodniDatum"] == DBNull.Value;
                //        var podminkaPriloha = dtRow["textBoxPriloha"] == null;

                //        //akce není uložena
                //        if (Convert.ToInt32(dtRow["akceId"]) == 0)
                //        {
                //            //nová akce
                //            ulozitBodAP.TypAkce.Add(new Akce(
                //                Convert.ToString(dtRow["textBoxNapravnaOpatreni"]),
                //                Convert.ToInt32(dtRow["comboBoxOdpovednaOsoba1Id"]),
                //                podminkaOsoba2 ? (int?)null : Convert.ToInt32(dtRow["comboBoxOdpovednaOsoba2Id"]),
                //                podminkaKontrolaEfektivity ? (DateTime?)null : Convert.ToDateTime(dtRow["textBoxKontrolaEfektivnosti"]),
                //                podminkaKontrolaEfektivityPuvodniDatum ? (DateTime?)null : Convert.ToDateTime(dtRow["textBoxKontrolaEfektivnostiPuvodniDatum"]),
                //                Convert.ToString(dtRow["textBoxKontrolaEfektivnostiOdstranit"]),
                //                podminkaOddeleni ? (int?)null : Convert.ToInt32(dtRow["comboBoxOddeleniId"]),
                //                podminkaPriloha ? null : Convert.ToString(dtRow["textBoxPriloha"]),
                //                typAkce,
                //                1,
                //                false,
                //                reopen));
                //            string poznamka;
                //            if (string.IsNullOrWhiteSpace(Convert.ToString(dtRow["textBoxPoznamka"])) == true)
                //                poznamka = null;
                //            else
                //                poznamka = Convert.ToString(dtRow["textBoxPoznamka"]);
                //            var posledniAkce = ulozitBodAP.TypAkce.Last();
                //            posledniAkce.UkonceniAkce.Add(new UkonceniAkce(Convert.ToDateTime(dtRow["textBoxDatumUkonceni"]), poznamka, null, 1, false));
                //        }
                //        else
                //        {
                //            //akce je uložena
                //            int akceId = Convert.ToInt32(dtRow["akceId"]);

                //            foreach (var akce in ulozitBodAP.TypAkce)
                //            {
                //                if (akce.Id == akceId)
                //                {
                //                    akce.NapravnaOpatreni = Convert.ToString(dtRow["textBoxNapravnaOpatreni"]);
                //                    akce.OdpovednaOsoba1Id = Convert.ToInt32(dtRow["comboBoxOdpovednaOsoba1Id"]);
                //                    if (Convert.ToInt32(dtRow["comboBoxOdpovednaOsoba2Id"]) == 0)
                //                        akce.OdpovednaOsoba2Id = null;
                //                    else
                //                        akce.OdpovednaOsoba2Id = Convert.ToInt32(dtRow["comboBoxOdpovednaOsoba2Id"]);
                //                    if (dtRow["textBoxKontrolaEfektivnosti"] == DBNull.Value)
                //                        akce.KontrolaEfektivnosti = null;
                //                    else
                //                        akce.KontrolaEfektivnosti = Convert.ToDateTime(dtRow["textBoxKontrolaEfektivnosti"]);
                //                    if (dtRow["textBoxKontrolaEfektivnostiPuvodniDatum"] == DBNull.Value)
                //                        akce.KontrolaEfektivnostiPuvodniDatum = null;
                //                    else
                //                        akce.KontrolaEfektivnostiPuvodniDatum = Convert.ToDateTime(dtRow["textBoxKontrolaEfektivnostiPuvodniDatum"]);

                //                    akce.KontrolaEfektivnostiOdstranit = Convert.ToString(dtRow["textBoxKontrolaEfektivnostiOdstranit"]);
                //                    if (Convert.ToInt32(dtRow["comboBoxOddeleniId"]) == 0)
                //                        akce.Oddeleni_Id = null;
                //                    else
                //                        akce.Oddeleni_Id = Convert.ToInt32(dtRow["comboBoxOddeleniId"]);
                //                    if (dtRow["textBoxPriloha"] == null)
                //                        akce.Priloha = null;
                //                    else
                //                        akce.Priloha = Convert.ToString(dtRow["textBoxPriloha"]);

                //                    string poznamka;
                //                    if (string.IsNullOrWhiteSpace(Convert.ToString(dtRow["textBoxPoznamka"])) == true)
                //                        poznamka = null;
                //                    else
                //                        poznamka = Convert.ToString(dtRow["textBoxPoznamka"]);
                //                    akce.UkonceniAkce.Add(new UkonceniAkce(Convert.ToDateTime(dtRow["textBoxDatumUkonceni"]), poznamka, null, 1, false));

                //                    break;
                //                }
                //            }
                //        }
                //    }
                //}

                //dtActionsWM.AcceptChanges();
                //dtActionsWS.AcceptChanges();
                ////FormPrehledBoduAP.UlozitBodyAP = true;

                //ObarveniRadek(DataGridViewWMAkce.Rows, dtActionsWM);
                //ObarveniRadek(DataGridViewWSAkce.Rows, dtActionsWS);
                //------------------------------------------------------------------------------------------------------------------------------------

                //vytvoření nebo aktualizace nového bodu
                int bodAPId = BodAPDataMapper.InsertUpdateBodAP(ulozitBodAP);

                //při uložení nového bodu je přepsána rpměnná novyBodAP na false, protože dále mohu editovat tento bod jako již uložený
                if (novyBodAP == true)
                {
                    novyBodAP = false;
                    cisloRadkyDGVBody = FormPrehledBoduAP.bodyAP.Count - 1;
                    FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].Id = bodAPId;
                    FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].BodUlozen = true;
                }

                //znovu natažení uloženého bodu do bodyAP
                //var bodyAP_ = ZadaniBoduAPViewModel.GetBodId(FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].Id).ToList();
                ////int bodId = 0;
                ////int i = 0;
                //foreach (var b in bodyAP_)
                //{
                //    //FormPrehledBoduAP.bodyAP.Add(new BodyAP(b.Id, b.IdAP, b.CisloBoduAP, b.DatumZalozeni, b.OdkazNaNormu, b.HodnoceniNeshody, b.PopisProblemu, b.SkutecnaPricina, new List<Akce>(), b.StavObjektuBodAP, true));
                //    FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody] = null;
                //    FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody] = new BodAP(b.BodId, b.IdAP, b.CisloBoduAP, b.DatumZalozeni, b.OdkazNaNormu, b.HodnoceniNeshody, b.PopisProblemu, 
                //        b.SkutecnaPricinaWM, b.NapravnaOpatreniWM, b.SkutecnaPricinaWS, b.NapravnaOpatreniWS,
                //        b.OdpovednaOsoba1Id, b.OdpovednaOsoba2Id, b.KontrolaEfektivnosti, b.OddeleniId, b.Priloha, b.ZnovuOtevrit, true, b.StavObjektuBodAP);

                //    //------------------------------------------------------------------------------------------------------------------------------------
                //    //var akce_ = PrehledBoduAPViewModel.GetAkceBodyId(b.BodId).ToList();

                //    //if (akce_.Count == 0)
                //    //{ }
                //    //else
                //    //{
                //    //    //tady navážu akce na body
                //    //    int i = 0;
                //    //    foreach (var a in akce_)
                //    //    {
                //    //        FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].TypAkce.Add(new Akce(a.Id, a.BodAPId, a.NapravnaOpatreni,
                //    //            a.OdpovednaOsoba1Id, a.OdpovednaOsoba2Id, new List<UkonceniAkce>(), a.KontrolaEfektivnosti, a.OddeleniId,
                //    //            a.Priloha, a.Typ, a.StavObjektuAkce, true, a.Reopen));

                //    //        var ukonceniAkce = PrehledBoduAPViewModel.GetUkonceniAkceId(a.Id).ToList();

                //    //        if (ukonceniAkce.Count == 0)
                //    //        { }
                //    //        else
                //    //        {
                //    //            foreach (var u in ukonceniAkce)
                //    //            {
                //    //                FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].TypAkce[i].UkonceniAkce.Add(new UkonceniAkce(u.Id, u.AkceId, u.DatumUkonceni, u.Poznamka, u.Odpoved, u.StavZadosti, u.StavObjektuAkce, true));
                //    //            }
                //    //        }
                //    //        i++;
                //    //    }
                //    //}
                //    //------------------------------------------------------------------------------------------------------------------------------------

                //}
                ZobrazeniDGV();
            }
        }

        private void ButtonUlozit_MouseClick(object sender, MouseEventArgs e)
        {
            //if (FormMain.VlastnikAkce == true)
            //{
            //    //přihlášen je vlastník akce
            //    UlozitAkce();
            //    ButtonUlozit.Enabled = false;
            //    changedDGV = false;
            //}
            //if (FormMain.VlastnikAP == true)
            //{
            //    //přihlášen je vlastník AP
            //    UlozitBodAP();
            //    ButtonUlozit.Enabled = false;
            //    changedDGV = false;
            //}
            UlozitBodAP();
            ButtonUlozit.Enabled = false;
            changedDGV = false;
        }

        private void ButtonNovaAkce_MouseClick(object sender, MouseEventArgs e)
        {
            if (TabControlAkce.SelectedTab == TabControlAkce.TabPages["tabPageWM"]) //your specific tabname
            {
                dtActionsWM.Rows.Add(new object[] { string.Empty, 0, 0, null, null, null, null, null, 0, string.Empty, null, 0, false });
                //dtActionsWM = (DataTable)_bindingSourceWM.DataSource;

                if (DataGridViewWMAkce.Rows.Count > 0)
                    ButtonOdstranitAkci.Enabled = true;
                else
                    ButtonOdstranitAkci.Enabled = false;
            }
            if (TabControlAkce.SelectedTab == TabControlAkce.TabPages["tabPageWS"]) //your specific tabname
            {
                //dtActionsWS.Rows.Add(new object[] { string.Empty, 0, 0, null, string.Empty, null, 0, string.Empty, string.Empty, 0 });
                dtActionsWS.Rows.Add(new object[] { string.Empty, 0, 0, null, null, null, null, null, 0, string.Empty, null, 0, false });
                //dtActionsWM = (DataTable)_bindingSourceWM.DataSource;

                if (DataGridViewWSAkce.Rows.Count > 0)
                    ButtonOdstranitAkci.Enabled = true;
                else
                    ButtonOdstranitAkci.Enabled = false;
            }
            ButtonUlozit.Enabled = true;
        }

        private void TabControlAkce_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabControlAkce.SelectedTab == TabControlAkce.TabPages["tabPageWM"])
            {
                if (DataGridViewWMAkce.Rows.Count > 0)
                    ButtonOdstranitAkci.Enabled = true;
                else
                    ButtonOdstranitAkci.Enabled = false;
            }

            if (TabControlAkce.SelectedTab == TabControlAkce.TabPages["tabPageWS"])
            {
                if (DataGridViewWSAkce.Rows.Count > 0)
                    ButtonOdstranitAkci.Enabled = true;
                else
                    ButtonOdstranitAkci.Enabled = false;
            }
        }

        private void ComboBoxOdpovednaOsoba1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ButtonUlozit.Enabled = true;
            changedDGV = true;

            if (ComboBoxOdpovednaOsoba1.SelectedIndex == 0)
            {
                MessageBox.Show("A responsible employee must be selected.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ComboBoxOddeleni_SelectedIndexChanged(object sender, EventArgs e)
        {
            ButtonUlozit.Enabled = true;
            changedDGV = true;

            if (ComboBoxOddeleni.SelectedIndex == 0)
            {
                MessageBox.Show("A department must be selected.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void RichTextBoxPopisProblemu_TextChanged(object sender, EventArgs e)
        {
            ButtonUlozit.Enabled = true;
            changedDGV = true;
        }

        private void RichTextBoxSkutecnaPricinaWM_TextChanged(object sender, EventArgs e)
        {
            ButtonUlozit.Enabled = true;
            changedDGV = true;
        }

        private void RichTextBoxNapravnaOpatreniWM_TextChanged(object sender, EventArgs e)
        {
            ButtonUlozit.Enabled = true;
            changedDGV = true;
        }

        private void RichTextBoxSkutecnaPricinaWS_TextChanged(object sender, EventArgs e)
        {
            ButtonUlozit.Enabled = true;
            changedDGV = true;
        }

        private void RichTextBoxNapravnaOpatreniWS_TextChanged(object sender, EventArgs e)
        {
            ButtonUlozit.Enabled = true;
            changedDGV = true;
        }

        //private void RichTextBoxSkutecnaPricina_TextChanged(object sender, EventArgs e)
        //{
        //    ButtonUlozit.Enabled = true;
        //    changedDGV = true;
        //}

        //private void ButtonOdstranitAkci_MouseClick(object sender, MouseEventArgs e)
        //{
        //    DialogResult dialogResult = MessageBox.Show("You want to cancel the selected action.", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

        //    if (dialogResult == DialogResult.Yes)
        //    {
        //        if (TabControlAkce.SelectedTab == TabControlAkce.TabPages["tabPageWM"])
        //        {
        //            if (DataGridViewWMAkce.CurrentCell.RowIndex >= 0)
        //            {
        //                foreach (var akce in FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].TypAkce)
        //                {
        //                    if (akce.Id == Convert.ToInt32(dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["akceId"]))
        //                    {
        //                        akce.StavObjektuAkce = 2;
        //                    }
        //                }
        //                dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex].Delete();
        //            }

        //            if (DataGridViewWMAkce.Rows.Count > 0)
        //                ButtonOdstranitAkci.Enabled = true;
        //            else
        //                ButtonOdstranitAkci.Enabled = false;
        //        }

        //        if (TabControlAkce.SelectedTab == TabControlAkce.TabPages["tabPageWS"])
        //        {
        //            if (DataGridViewWSAkce.CurrentCell.RowIndex >= 0)
        //            {
        //                foreach (var akce in FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].TypAkce)
        //                {
        //                    if (akce.Id == Convert.ToInt32(dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["akceId"]))
        //                    {
        //                        akce.StavObjektuAkce = 2;
        //                    }
        //                }
        //                dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex].Delete();
        //            }

        //            if (DataGridViewWSAkce.Rows.Count > 0)
        //                ButtonOdstranitAkci.Enabled = true;
        //            else
        //                ButtonOdstranitAkci.Enabled = false;
        //        }

        //        ButtonUlozit.Enabled = true;
        //        changedDGV = true;
        //    }
        //    else if (dialogResult == DialogResult.No)
        //    {
        //    }
        //}

        private void DataGridViewWMAkce_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewWMAkce.ClearSelection();
        }

        private void DataGridViewWSAkce_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewWSAkce.ClearSelection();
        }

        private void ButtonTerminUkonceni_MouseClick(object sender, MouseEventArgs e)
        {
            if (novyBodAP == false)
            {
                bool kontrolaEfektivnosti = true;
                if (FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].KontrolaEfektivnosti == null)
                {
                    //neí zadána kontrola efektivnossti
                    kontrolaEfektivnosti = false;
                }

                //if (kontrolaEfektivnosti == false)
                //{
                //    using (var form = new FormDatumUkonceni(FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].DatumUkonceni, FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].UkonceniPoznamka))
                //    {
                //        var result = form.ShowDialog();
                //        if (result == DialogResult.OK)
                //        {
                //            FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].DatumUkonceni = form.ReturnValueDatum;
                //            FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].UkonceniPoznamka = form.ReturnValuePoznamka;
                //            labelDatumUkonceni.Text = Convert.ToDateTime(FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].DatumUkonceni).ToShortDateString();

                //            //FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].DatumUkonceni = datumUkonceni;
                //            changedDGV = true;
                //            ButtonUlozit.Enabled = true;
                //            //podminkaPoznamka = poznamka == null;
                //            //FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].UkonceniPoznamka = podminkaPoznamka ? null : poznamka;
                //        }
                //    }
                //}
                //else
                //{
                //}

                //prom opravitTermin povolí nebo zablokuje možnost pracovat s prodloužením termínu
                //editovat se dají pouze akce, které patří maijteli akce, nebo vlastníkovi AP
                //bool opravitTermin = false;
                bool opravitTermin = true;
                if (akcniPlany_.APUzavren == true || kontrolaEfektivnosti == true)
                    opravitTermin = false;

                using (var form = new FormPosunutiTerminuBodAP(opravitTermin, cisloAPStr_, cisloRadkyDGVBody))
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        //string dateString = form.ReturnValueDatum;
                        //string poznamka = form.ReturnValuePoznamka;

                        //dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxDatumUkonceni"] = Convert.ToDateTime(dateString);
                        //changedDGV = true;
                        //podminkaPoznamka = poznamka == null;
                        //dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxPoznamka"] = podminkaPoznamka ? null : Convert.ToString(poznamka);
                    }
                }
            }
            else
            {
                //var podminkaDatum = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].DatumUkonceni == null;
                //var podminkaPoznamka = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].UkonceniPoznamka == null;

                //using (var form = new FormDatumUkonceni(podminkaDatum ? (DateTime?)null : Convert.ToDateTime(FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].DatumUkonceni),
                //podminkaPoznamka ? null : FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].UkonceniPoznamka))
                //tady bude dvakrát null, protže bodAP není ještě založen
                // toto FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody] musím vyřešit, protože bodAP není ještě založen
                using (var form = new FormDatumUkonceni(datumUkonceni, poznamkaDatumUkonceni))
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        datumUkonceni = form.ReturnValueDatum;
                        poznamkaDatumUkonceni = form.ReturnValuePoznamka;
                        labelDatumUkonceni.Text = Convert.ToDateTime(datumUkonceni).ToShortDateString();
                        deadLineZadan = true;
                        //FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].DatumUkonceni = datumUkonceni;
                        changedDGV = true;
                        //podminkaPoznamka = poznamka == null;
                        //FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].UkonceniPoznamka = podminkaPoznamka ? null : poznamka;
                    }
                }
            }
        }

        private void ButtonKontrolaEfektivnosti_MouseClick(object sender, MouseEventArgs e)
        {
            bool kontrolaEfektivnosti = false;
            int bodAPId = 0;
            DateTime? datumKontrolEfekt = null;

            if (novyBodAP == true)
            {
                if (kontrolaEfektivnostiDatum == null)
                {
                    kontrolaEfektivnosti = false;
                }
                else
                {
                    datumKontrolEfekt = kontrolaEfektivnostiDatum;
                }
            }
            else
            {
                //není zadána kontrola efektivnosti
                
                bodAPId = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].Id;
                if (FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].KontrolaEfektivnosti == null)
                {
                    kontrolaEfektivnosti = false;
                }
                else
                {
                    datumKontrolEfekt = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].KontrolaEfektivnosti;
                    kontrolaEfektivnosti = true;
                }
            }


            //var podminkaDatum = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].KontrolaEfektivnosti == null;

            //using (var form = new FormKontrolaEfektivnosti(akcniPlany_.APUzavren, bodAPId,
            //    kontrolaEfektivnosti,
            //    podminkaDatum ? (DateTime?)null : Convert.ToDateTime(FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].KontrolaEfektivnosti)))

            using (var form = new FormKontrolaEfektivnosti(novyBodAP, akcniPlany_.APUzavren, deadLineZadan, bodAPId, kontrolaEfektivnosti, datumKontrolEfekt))
            {
                //var result = form.ShowDialog();
                form.ShowDialog();
                var result = form.ReturnValuePotvrdit;
                if (result == true)
                {
                    DateTime? datumKontrolaEfektivnosti = form.ReturnValueDatum;
                    string poznamka = form.ReturnValuePoznamka;
                    DateTime? puvodniDatum = form.ReturnValuePuvodniDatum;
                    if (novyBodAP == true)
                    {
                        if (datumKontrolaEfektivnosti == null)
                        {
                            kontrolaEfektivnostiDatum = null;
                            labelEfektivita.Text = string.Empty;
                        }
                        else
                        {
                            kontrolaEfektivnostiDatum = datumKontrolaEfektivnosti;
                            labelEfektivita.Text = Convert.ToDateTime(datumKontrolaEfektivnosti).ToShortDateString();
                        }
                    }
                    else
                    {
                        if (datumKontrolaEfektivnosti == null)
                        {
                            //povolit editaci
                            FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].KontrolaEfektivnosti = null;
                            FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].KontrolaEfektivnostiPuvodniDatum = puvodniDatum;
                            FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].KontrolaEfektivnostiOdstranit = poznamka;
                            labelEfektivita.Text = string.Empty;

                            OdblokovatPole();
                        }
                        else
                        {
                            //zablokovat editaci
                            FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].KontrolaEfektivnosti = datumKontrolaEfektivnosti;
                            FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].KontrolaEfektivnostiPuvodniDatum = null;
                            FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].KontrolaEfektivnostiOdstranit = string.Empty;
                            labelEfektivita.Text = Convert.ToDateTime(datumKontrolaEfektivnosti).ToShortDateString();

                            ZablokovatPole();
                        }
                    }
                    //to tady nebude, protože to bylo již uloženo
                    //changedDGV = true;
                    //ButtonUlozit.Enabled = true;
                }
            }   
        }

        private void ZablokovatPole()
        {
            TextBoxOdkazNaNormu.Enabled = true;
            TextBoxOdkazNaNormu.ReadOnly = true;
            TextBoxOdkazNaNormu.GotFocus += new System.EventHandler(TextBoxOdkazNaNormuGotFocus);

            TextBoxHodnoceniNeshody.Enabled = true;
            TextBoxHodnoceniNeshody.ReadOnly = true;
            TextBoxHodnoceniNeshody.GotFocus += new System.EventHandler(TextBoxHodnoceniNeshodyGotFocus);

            ComboBoxOdpovednaOsoba1.Enabled = false;
            ComboBoxOdpovednaOsoba2.Enabled = false;
            ComboBoxOddeleni.Enabled = false;

            RichTextBoxPopisProblemu.Enabled = true;
            RichTextBoxPopisProblemu.ReadOnly = true;
            RichTextBoxPopisProblemu.GotFocus += new System.EventHandler(RichTextBoxPopisProblemuGotFocus);

            RichTextBoxSkutecnaPricinaWM.Enabled = true;
            RichTextBoxSkutecnaPricinaWM.ReadOnly = true;
            RichTextBoxSkutecnaPricinaWM.GotFocus += new System.EventHandler(RichTextBoxSkutecnaPricinaWMGotFocus);

            RichTextBoxNapravnaOpatreniWM.Enabled = true;
            RichTextBoxNapravnaOpatreniWM.ReadOnly = true;
            RichTextBoxNapravnaOpatreniWM.GotFocus += new System.EventHandler(RichTextBoxNapravnaOpatreniWMGotFocus);

            RichTextBoxSkutecnaPricinaWS.Enabled = true;
            RichTextBoxSkutecnaPricinaWS.ReadOnly = true;
            RichTextBoxSkutecnaPricinaWS.GotFocus += new System.EventHandler(RichTextBoxSkutecnaPricinaWSGotFocus);

            RichTextBoxNapravnaOpatreniWS.Enabled = true;
            RichTextBoxNapravnaOpatreniWS.ReadOnly = true;
            RichTextBoxNapravnaOpatreniWS.GotFocus += new System.EventHandler(RichTextBoxNapravnaOpatreniWSGotFocus);
        }

        private void OdblokovatPole()
        {
            TextBoxOdkazNaNormu.Enabled = true;
            TextBoxOdkazNaNormu.ReadOnly = false;
            TextBoxOdkazNaNormu.GotFocus -= new System.EventHandler(TextBoxOdkazNaNormuGotFocus);

            TextBoxHodnoceniNeshody.Enabled = true;
            TextBoxHodnoceniNeshody.ReadOnly = false;
            TextBoxHodnoceniNeshody.GotFocus -= new System.EventHandler(TextBoxHodnoceniNeshodyGotFocus);

            ComboBoxOdpovednaOsoba1.Enabled = true;
            ComboBoxOdpovednaOsoba2.Enabled = true;
            ComboBoxOddeleni.Enabled = true;

            RichTextBoxPopisProblemu.Enabled = true;
            RichTextBoxPopisProblemu.ReadOnly = false;
            RichTextBoxPopisProblemu.GotFocus -= new System.EventHandler(RichTextBoxPopisProblemuGotFocus);

            RichTextBoxSkutecnaPricinaWM.Enabled = true;
            RichTextBoxSkutecnaPricinaWM.ReadOnly = false;
            RichTextBoxSkutecnaPricinaWM.GotFocus -= new System.EventHandler(RichTextBoxSkutecnaPricinaWMGotFocus);

            RichTextBoxNapravnaOpatreniWM.Enabled = true;
            RichTextBoxNapravnaOpatreniWM.ReadOnly = false;
            RichTextBoxNapravnaOpatreniWM.GotFocus -= new System.EventHandler(RichTextBoxNapravnaOpatreniWMGotFocus);

            RichTextBoxSkutecnaPricinaWS.Enabled = true;
            RichTextBoxSkutecnaPricinaWS.ReadOnly = false;
            RichTextBoxSkutecnaPricinaWS.GotFocus -= new System.EventHandler(RichTextBoxSkutecnaPricinaWSGotFocus);

            RichTextBoxNapravnaOpatreniWS.Enabled = true;
            RichTextBoxNapravnaOpatreniWS.ReadOnly = false;
            RichTextBoxNapravnaOpatreniWS.GotFocus -= new System.EventHandler(RichTextBoxNapravnaOpatreniWSGotFocus);
        }

        void TextBoxOdkazNaNormuGotFocus(object sender, System.EventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("{tab}");
        }

        void TextBoxHodnoceniNeshodyGotFocus(object sender, System.EventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("{tab}");
        }

        void RichTextBoxPopisProblemuGotFocus(object sender, System.EventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("{tab}");
        }

        void RichTextBoxSkutecnaPricinaWMGotFocus(object sender, System.EventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("{tab}");
        }

        void RichTextBoxNapravnaOpatreniWMGotFocus(object sender, System.EventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("{tab}");
        }

        void RichTextBoxSkutecnaPricinaWSGotFocus(object sender, System.EventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("{tab}");
        }

        void RichTextBoxNapravnaOpatreniWSGotFocus(object sender, System.EventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("{tab}");
        }

        private void ButtonPriloha_MouseClick(object sender, MouseEventArgs e)
        {
            string attachment = priloha;
            bool readOnly = false;

            if (novyBodAP == false)
            {
                //var podminkaPriloha = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].Priloha == string.Empty;
                //podminkaPriloha ? string.Empty : FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].Priloha;
                if (FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].Priloha == string.Empty)
                    attachment = string.Empty;
                else
                    attachment = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].Priloha;

                if (FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].KontrolaEfektivnosti == null)
                    readOnly = false;
                else
                    readOnly = true;
            }

            using (var form = new FormPriloha(novyBodAP, readOnly, attachment, cisloRadkyDGVBody))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (novyBodAP == true)
                        priloha = form.ReturnValueFolder;
                    else
                        FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].Priloha = form.ReturnValueFolder;

                    //DataGridViewWMAkce.Rows[DataGridViewWMAkce.CurrentCell.RowIndex].Cells["buttonPriloha"].Value = "(attachment)";
                    changedDGV = true;
                    ButtonUlozit.Enabled = true;
                }
                if (result == DialogResult.Cancel)
                {
                }
                if (result == DialogResult.Abort)
                {
                    if (novyBodAP == true)
                        priloha = string.Empty;
                    else
                        FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].Priloha = string.Empty;

                    //DataGridViewWMAkce.Rows[DataGridViewWMAkce.CurrentCell.RowIndex].Cells["buttonPriloha"].Value = string.Empty;
                    changedDGV = true;
                    ButtonUlozit.Enabled = true;
                }
            }
        }

        //private void DataGridViewWMAkce_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    var senderGrid = (DataGridView)sender;

        //    if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
        //    {
        //        if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
        //        {
        //            if (senderGrid.Columns[e.ColumnIndex].Name == "buttonDatumUkonceni")
        //            {
        //                var podminkaDatum = dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxDatumUkonceni"] == DBNull.Value;
        //                var podminkaPoznamka = dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxPoznamka"] == DBNull.Value;

        //                int radekIndex = DataGridViewWMAkce.CurrentCell.RowIndex;

        //                //if (FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].TypAkce[radekIndex].AkceUlozena == true)
        //                //pokud již je akce uložena, volám FormPosunutiTerminuAkce, protože potřebuji posouvat termín ukončení
        //                if (Convert.ToInt32(dtActionsWM.Rows[radekIndex]["akceId"]) > 0)
        //                {
        //                    bool kontrolaEfektivnosti = true;
        //                    if (dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnosti"] == DBNull.Value)
        //                    {
        //                        kontrolaEfektivnosti = false;
        //                    }

        //                    //prom opravitTermin povolí nebo zablokuje možnost pracovat s prodloužením termínu
        //                    //editovat se dají pouze akce, které patří maijteli akce, nebo vlastníkovi AP
        //                    //bool opravitTermin = false;
        //                    bool opravitTermin = true;
        //                    if (akcniPlany_.APUzavren == true || kontrolaEfektivnosti == true)
        //                        opravitTermin = false;

        //                    using (var form = new FormPosunutiTerminuBodAP(opravitTermin, cisloAPStr_, cisloRadkyDGVBody, dtActionsWM.Rows[radekIndex]))
        //                    {
        //                        var result = form.ShowDialog();
        //                        if (result == DialogResult.OK)
        //                        {
        //                            //string dateString = form.ReturnValueDatum;
        //                            //string poznamka = form.ReturnValuePoznamka;

        //                            //dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxDatumUkonceni"] = Convert.ToDateTime(dateString);
        //                            //changedDGV = true;
        //                            //podminkaPoznamka = poznamka == null;
        //                            //dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxPoznamka"] = podminkaPoznamka ? null : Convert.ToString(poznamka);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    using (var form = new FormDatumUkonceni(podminkaDatum ? (DateTime?)null : Convert.ToDateTime(dtActionsWM.Rows[radekIndex]["textBoxDatumUkonceni"]),
        //                        podminkaPoznamka ? null : Convert.ToString(dtActionsWM.Rows[radekIndex]["textBoxPoznamka"]), cisloRadkyDGVBody, dtActionsWM.Rows[radekIndex]))
        //                    {
        //                        var result = form.ShowDialog();
        //                        if (result == DialogResult.OK)
        //                        {
        //                            string dateString = form.ReturnValueDatum;
        //                            string poznamka = form.ReturnValuePoznamka;

        //                            dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxDatumUkonceni"] = Convert.ToDateTime(dateString);
        //                            changedDGV = true;
        //                            podminkaPoznamka = poznamka == null;
        //                            dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxPoznamka"] = podminkaPoznamka ? null : Convert.ToString(poznamka);
        //                        }
        //                    }
        //                }
        //            }

        //            if (senderGrid.Columns[e.ColumnIndex].Name == "buttonKontrolaEfektivnosti")
        //            {
        //                if (Convert.ToInt32(dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["akceId"]) == 0)
        //                {
        //                    MessageBox.Show("Actions must be saved first.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                }
        //                else
        //                {
        //                    bool kontrolaEfektivnosti = true;
        //                    if (dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnosti"] == DBNull.Value)
        //                    {
        //                        kontrolaEfektivnosti = false;
        //                    }

        //                    var podminkaDatum = dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnosti"] == DBNull.Value;

        //                    using (var form = new FormKontrolaEfektivnosti(akcniPlany_.APUzavren, Convert.ToInt32(dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["akceId"]),
        //                        kontrolaEfektivnosti,
        //                        podminkaDatum ? (DateTime?)null : Convert.ToDateTime(dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnosti"])))
        //                    {
        //                        //var result = form.ShowDialog();
        //                        form.ShowDialog();
        //                        var result = form.ReturnValuePotvrdit;
        //                        if (result == true)
        //                        {
        //                            string dateString = form.ReturnValueDatum;
        //                            string poznamka = form.ReturnValuePoznamka;
        //                            string puvodniDatum = form.ReturnValuePuvodniDatum;
        //                            if (string.IsNullOrEmpty(dateString))
        //                            {
        //                                dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnosti"] = DBNull.Value;
        //                                dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnostiPuvodniDatum"] = puvodniDatum;
        //                                dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnostiOdstranit"] = poznamka;
        //                            }
        //                            else
        //                            {
        //                                dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnosti"] = Convert.ToDateTime(dateString);
        //                                dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnostiPuvodniDatum"] = DBNull.Value;
        //                                dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnostiOdstranit"] = DBNull.Value;
        //                            }
        //                            //to tady nebude, protože to bylo již uloženo
        //                            //changedDGV = true;
        //                            //ButtonUlozit.Enabled = true;
        //                        }
        //                    }

        //                    //obarví řádky
        //                    ObarveniRadek(DataGridViewWMAkce.Rows, dtActionsWM);
        //                }
        //            }

        //            if (senderGrid.Columns[e.ColumnIndex].Name == "buttonPriloha")
        //            {
        //                if (DataGridViewWMAkce.CurrentCell.RowIndex >= 0)
        //                {
        //                    //bool vlastnik = false;
        //                    //if (Convert.ToInt32(dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["comboBoxOdpovednaOsoba1Id"]) == FormMain.VlastnikIdAkce ||
        //                    //    Convert.ToInt32(dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["comboBoxOdpovednaOsoba2Id"]) == FormMain.VlastnikIdAkce || FormMain.VlastnikAP == true)
        //                    //{
        //                    //    vlastnik = true;
        //                    //}

        //                    var podminkaPriloha = dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxPriloha"].ToString() == string.Empty;
        //                    bool readOnly = true;
        //                    if (dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnosti"] == DBNull.Value)
        //                        readOnly = false;

        //                    using (var form = new FormPriloha(readOnly, podminkaPriloha ? string.Empty : Convert.ToString(dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxPriloha"]), cisloRadkyDGVBody))
        //                    {
        //                        var result = form.ShowDialog();
        //                        if (result == DialogResult.OK)
        //                        {
        //                            dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxPriloha"] = form.ReturnValueFolder;
        //                            //DataGridViewWMAkce.Rows[DataGridViewWMAkce.CurrentCell.RowIndex].Cells["buttonPriloha"].Value = "(attachment)";
        //                            changedDGV = true;
        //                            ButtonUlozit.Enabled = true;
        //                        }
        //                        if (result == DialogResult.Cancel)
        //                        {
        //                        }
        //                        if (result == DialogResult.Abort)
        //                        {
        //                            dtActionsWM.Rows[DataGridViewWMAkce.CurrentCell.RowIndex]["textBoxPriloha"] = string.Empty;
        //                            //DataGridViewWMAkce.Rows[DataGridViewWMAkce.CurrentCell.RowIndex].Cells["buttonPriloha"].Value = string.Empty;
        //                            changedDGV = true;
        //                            ButtonUlozit.Enabled = true;
        //                        }
        //                    }
        //                    DataGridViewWMAkce.EndEdit();
        //                    DataGridViewWMAkce.Refresh();
        //                }
        //            }
        //        }
        //    }
        //    DataGridViewWMAkce.EndEdit();
        //    DataGridViewWMAkce.Refresh();
        //}

        //private void ObarveniRadek(DataGridViewRowCollection radkyDGV, DataTable tabulkaDGV)
        //{
        //    int radek = 0;
        //    foreach (DataGridViewRow row in radkyDGV)
        //    {
        //        //musím přebarvit řádky, protože před tím mohli být zelené
        //        if (radek % 2 == 0)
        //            row.DefaultCellStyle.BackColor = Color.White;
        //        else
        //            row.DefaultCellStyle.BackColor = Color.LightGray;

        //        radek++;
        //    }
        //    radek = 0;
        //    foreach (DataGridViewRow row in radkyDGV)
        //    {
        //        //if (FormMain.VlastnikAkce == true)
        //        //{
        //        //    if (Convert.ToInt32(row.Cells["comboBoxOdpovednaOsoba1Id"].Value) == FormMain.VlastnikIdAkce || Convert.ToInt32(row.Cells["comboBoxOdpovednaOsoba2Id"].Value) == FormMain.VlastnikIdAkce)
        //        //    {
        //        //        //pro řádky, které patří majiteli akce, nastavím barvu pozadí modrou
        //        //        row.DefaultCellStyle.BackColor = Color.LightBlue;
        //        //    }
        //        //}
        //        if (tabulkaDGV.Rows[radek]["textBoxKontrolaEfektivnosti"] == DBNull.Value)
        //        {
        //            row.ReadOnly = false;
        //        }
        //        else
        //        {
        //            //ukončené akce - je nastaven datum efektivnosti
        //            row.DefaultCellStyle.BackColor = Color.LightGreen;
        //            row.ReadOnly = true;
        //        }
        //        radek++;
        //    }
        //}

        //private void DataGridViewWSAkce_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    var senderGrid = (DataGridView)sender;
        //    if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
        //    {
        //        if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
        //        {
        //            if (senderGrid.Columns[e.ColumnIndex].Name == "buttonDatumUkonceni")
        //            {
        //                var podminkaDatum = dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxDatumUkonceni"] == DBNull.Value;
        //                var podminkaPoznamka = dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxPoznamka"] == DBNull.Value;

        //                int radekIndex = DataGridViewWSAkce.CurrentCell.RowIndex;

        //                //if (FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].TypAkce[radekIndex].AkceUlozena == true)
        //                //pokud již je akce uložena, volám FormPosunutiTerminuAkce, protože potřebuji posouvat termín ukončení
        //                if (Convert.ToInt32(dtActionsWS.Rows[radekIndex]["akceId"]) > 0)
        //                {
        //                    bool kontrolaEfektivnosti = true;
        //                    if (dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnosti"] == DBNull.Value)
        //                    {
        //                        kontrolaEfektivnosti = false;
        //                    }

        //                    //prom opravitTermin povolí nebo zablokuje 
        //                    bool opravitTermin = true;
        //                    if (akcniPlany_.APUzavren == true || kontrolaEfektivnosti == true)
        //                        opravitTermin = false;

        //                    //if (FormMain.VlastnikAP == true || opraveneAkce.Contains(Convert.ToInt32(dtActionsWS.Rows[radekIndex]["akceId"])))
        //                    //{
        //                    //    opravitTermin = true;
        //                    //}
        //                    //kontrolaEfektivnosti, 
        //                    using (var form = new FormPosunutiTerminuBodAP(opravitTermin, cisloAPStr_, cisloRadkyDGVBody, dtActionsWS.Rows[radekIndex]))
        //                    {
        //                        var result = form.ShowDialog();
        //                        if (result == DialogResult.OK)
        //                        {
        //                            //string dateString = form.ReturnValueDatum;
        //                            //string poznamka = form.ReturnValuePoznamka;

        //                            //dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxDatumUkonceni"] = Convert.ToDateTime(dateString);
        //                            //changedDGV = true;
        //                            //podminkaPoznamka = poznamka == null;
        //                            //dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxPoznamka"] = podminkaPoznamka ? null : Convert.ToString(poznamka);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    using (var form = new FormDatumUkonceni(podminkaDatum ? (DateTime?)null : Convert.ToDateTime(dtActionsWS.Rows[radekIndex]["textBoxDatumUkonceni"]),
        //                        podminkaPoznamka ? null : Convert.ToString(dtActionsWS.Rows[radekIndex]["textBoxPoznamka"]), cisloRadkyDGVBody, dtActionsWS.Rows[radekIndex]))
        //                    {
        //                        var result = form.ShowDialog();
        //                        if (result == DialogResult.OK)
        //                        {
        //                            string dateString = form.ReturnValueDatum;
        //                            string poznamka = form.ReturnValuePoznamka;

        //                            dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxDatumUkonceni"] = Convert.ToDateTime(dateString);
        //                            changedDGV = true;
        //                            podminkaPoznamka = poznamka == null;
        //                            dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxPoznamka"] = podminkaPoznamka ? null : Convert.ToString(poznamka);
        //                        }
        //                    }
        //                }
        //            }

        //            if (senderGrid.Columns[e.ColumnIndex].Name == "buttonKontrolaEfektivnosti")
        //            {
        //                if (Convert.ToInt32(dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["akceId"]) == 0)
        //                {
        //                    MessageBox.Show("Actions must be saved first.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                }
        //                else
        //                {
        //                    bool kontrolaEfektivnosti = true;
        //                    if (dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnosti"] == DBNull.Value)
        //                    {
        //                        kontrolaEfektivnosti = false;
        //                    }

        //                    var podminkaDatum = dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnosti"] == DBNull.Value;

        //                    using (var form = new FormKontrolaEfektivnosti(akcniPlany_.APUzavren, Convert.ToInt32(dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["akceId"]),
        //                        kontrolaEfektivnosti,
        //                        podminkaDatum ? (DateTime?)null : Convert.ToDateTime(dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnosti"])))
        //                    {
        //                        //var result = form.ShowDialog();
        //                        form.ShowDialog();
        //                        var result = form.ReturnValuePotvrdit;
        //                        if (result == true)
        //                        {
        //                            string dateString = form.ReturnValueDatum;
        //                            string poznamka = form.ReturnValuePoznamka;
        //                            string puvodniDatum = form.ReturnValuePuvodniDatum;
        //                            if (string.IsNullOrEmpty(dateString))
        //                            {
        //                                dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnosti"] = DBNull.Value;
        //                                dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnostiPuvodniDatum"] = puvodniDatum;
        //                                dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnostiOdstranit"] = poznamka;
        //                            }
        //                            else
        //                            {
        //                                dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnosti"] = Convert.ToDateTime(dateString);
        //                                dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnostiPuvodniDatum"] = DBNull.Value;
        //                                dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnostiOdstranit"] = DBNull.Value;
        //                            }
        //                            changedDGV = true;
        //                            ButtonUlozit.Enabled = true;
        //                        }
        //                    }
        //                    //obarví řádky
        //                    ObarveniRadek(DataGridViewWSAkce.Rows, dtActionsWS);
        //                }
        //            }

        //            if (senderGrid.Columns[e.ColumnIndex].Name == "buttonPriloha")
        //            {
        //                //MessageBox.Show("Attachment", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                if (DataGridViewWSAkce.CurrentCell.RowIndex >= 0)
        //                {
        //                    //bool vlastnik = false;
        //                    //if (Convert.ToInt32(dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["comboBoxOdpovednaOsoba1Id"]) == FormMain.VlastnikIdAkce ||
        //                    //    Convert.ToInt32(dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["comboBoxOdpovednaOsoba2Id"]) == FormMain.VlastnikIdAkce || FormMain.VlastnikAP == true)
        //                    //{
        //                    //    vlastnik = true;
        //                    //}

        //                    var podminkaPriloha = dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxPriloha"].ToString() == string.Empty;
        //                    bool readOnly = true;
        //                    if (dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxKontrolaEfektivnosti"] == DBNull.Value)
        //                        readOnly = false;

        //                    using (FormPriloha form = new FormPriloha(readOnly, podminkaPriloha ? string.Empty : Convert.ToString(dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxPriloha"]), cisloRadkyDGVBody))
        //                    {
        //                        var result = form.ShowDialog();
        //                        if (result == DialogResult.OK)
        //                        {
        //                            dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxPriloha"] = form.ReturnValueFolder;
        //                            //DataGridViewWSAkce.Rows[DataGridViewWSAkce.CurrentCell.RowIndex].Cells["buttonPriloha"].Value = "(attachment)";
        //                            changedDGV = true;
        //                            ButtonUlozit.Enabled = true;

        //                        }
        //                        if (result == DialogResult.Cancel)
        //                        {
        //                        }
        //                        if (result == DialogResult.Abort)
        //                        {
        //                            dtActionsWS.Rows[DataGridViewWSAkce.CurrentCell.RowIndex]["textBoxPriloha"] = string.Empty;
        //                            //DataGridViewWSAkce.Rows[DataGridViewWSAkce.CurrentCell.RowIndex].Cells["buttonPriloha"].Value = string.Empty;
        //                            changedDGV = true;
        //                            ButtonUlozit.Enabled = true;

        //                        }
        //                    }
        //                    DataGridViewWSAkce.EndEdit();
        //                    DataGridViewWSAkce.Refresh();
        //                }
        //            }
        //        }
        //    }
        //    DataGridViewWSAkce.EndEdit();
        //    DataGridViewWSAkce.Refresh();
        //}

        private void DataGridViewWMAkce_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                ButtonUlozit.Enabled = true;
                changedDGV = true;
            }
        }

        private void DataGridViewWSAkce_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                ButtonUlozit.Enabled = true;
                changedDGV = true;
            }
        }

        private void DataGridViewWMAkce_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (DataGridViewWMAkce.IsCurrentCellDirty)
            {
                DataGridViewWMAkce.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DataGridViewWSAkce_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (DataGridViewWSAkce.IsCurrentCellDirty)
            {
                DataGridViewWSAkce.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void TextBoxOdkazNaNormu_TextChanged(object sender, EventArgs e)
        {
            ButtonUlozit.Enabled = true;
            changedDGV = true;
        }

        private void TextBoxHodnoceniNeshody_TextChanged(object sender, EventArgs e)
        {
            ButtonUlozit.Enabled = true;
            changedDGV = true;
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void FormZadaniBoduAP_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (changedDGV == true)
            {
                DialogResult dialogResult;

                dialogResult = MessageBox.Show("You want to save your changes.", "Notice", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                //if (FormMain.VlastnikAkce == true)
                //{
                //    dialogResult = MessageBox.Show("You want to save your changes.", "Notice", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                //}
                //else
                //{
                //    dialogResult = MessageBox.Show("You want to save your changes.", "Notice", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                //}

                if (dialogResult == DialogResult.Yes)
                {
                    UlozitBodAP();
                    changedDGV = false;
                    //if (FormMain.VlastnikAkce == true)
                    //{
                    //    //zapíše bod AP do třídy
                    //    UlozitAkce();
                    //    changedDGV = false;
                    //}
                    //else
                    //{
                    //    //zapíše bod AP do třídy
                    //    UlozitBodAP();
                    //    changedDGV = false;
                    //    //Close();
                    //}
                }
                else if (dialogResult == DialogResult.No) { }
                else if (dialogResult == DialogResult.Cancel)
                {
                    //nic se dít nebude
                    e.Cancel = true;
                }
            }
        }
    }
}
