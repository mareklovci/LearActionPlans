using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using LearActionPlans.Models;
using LearActionPlans.Repositories;
using LearActionPlans.ViewModels;

namespace LearActionPlans.Views
{
    public partial class FormPrehledBoduAP : Form
    {
        private readonly FormZadaniBoduAP formZadaniBoduAp;
        private readonly EmployeeRepository employeeRepository;
        private readonly ActionPlanPointRepository actionPlanPointRepository;
        private readonly DepartmentRepository departmentRepository;
        private readonly ActionPlanEndRepository actionPlanEndRepository;
        private readonly EmailRepository emailRepository;

        //při volání konstruktoru vytvořím novou prom jako readonly, protože ji nastavím při volání konstruktoru a pak už její hodnoty neměním
        private FormNovyAkcniPlan.AkcniPlanTmp akcniPlany_;

        //tady udržuji všechny informace o bodech AP
        public static List<BodAP> bodyAP;

        //do této proměnné uložím pouze hodnoty, které se zobrazí v DGV
        private DataTable dtBodyAP;

        private BindingSource bindingSource;
        //public static bool UlozitBodyAP { get; set; }

        private byte volani_;
        //1 - založení nového AP
        //2 - aktualizace již vytvořeného AP
        //3 - bude volání z formuláře FormVsechnyBodyAP

        private bool spusteniBezParametru_;

        public FormPrehledBoduAP(
            FormZadaniBoduAP formZadaniBoduAp,
            EmployeeRepository employeeRepository,
            ActionPlanPointRepository actionPlanPointRepository,
            DepartmentRepository departmentRepository,
            ActionPlanEndRepository actionPlanEndRepository,
            EmailRepository emailRepository)
        {
            // Forms
            this.formZadaniBoduAp = formZadaniBoduAp;

            // Repositories
            this.employeeRepository = employeeRepository;
            this.actionPlanPointRepository = actionPlanPointRepository;
            this.departmentRepository = departmentRepository;
            this.actionPlanEndRepository = actionPlanEndRepository;
            this.emailRepository = emailRepository;

            // Initialize
            this.InitializeComponent();
        }

        public void CreateFormPrehledBoduAP(bool spusteniBezParametru, FormNovyAkcniPlan.AkcniPlanTmp akcniPlany,
            byte volani)
        {
            this.bindingSource = new BindingSource();
            bodyAP = new List<BodAP>();
            this.dtBodyAP = new DataTable();

            this.spusteniBezParametru_ = spusteniBezParametru;
            this.akcniPlany_ = akcniPlany;
            this.volani_ = volani;
        }

        public IEnumerable<PrehledBoduAPViewModel> GetUkonceniAPId(int idAP)
        {
            var ukonceniAP = this.actionPlanEndRepository.GetUkonceniAP(idAP).ToList();

            if (!ukonceniAP.Any())
            {
                yield break;
            }

            var query = from u in ukonceniAP
                where u.APId == idAP
                orderby u.Id descending
                select PrehledBoduAPViewModel.UkonceniAP(u.Id, u.DatumUkonceni);

            foreach (var q in query)
            {
                yield return q;
            }
        }

        private void FormNovyBodAP_Load(object sender, EventArgs e)
        {
            this.bindingSource.DataSource = this.dtBodyAP;
            this.DataGridViewBodyAP.DataSource = this.bindingSource;

            this.labelCisloAP.Text = this.akcniPlany_.CisloAPRok ?? "";
            this.labelZadavatel1Zadano.Text = this.akcniPlany_.Zadavatel1Jmeno ?? "";
            this.labelZadavatel2Zadano.Text =
                this.akcniPlany_.Zadavatel2Jmeno ?? (this.akcniPlany_.Zadavatel2Jmeno = "");
            this.labelTemaAP.Text = this.akcniPlany_.Tema ?? "";
            this.labelProjektAP.Text = this.akcniPlany_.ProjektNazev ?? "";
            this.labelDatumZahajeniAP.Text = Convert.ToDateTime(this.akcniPlany_.DatumZalozeni).ToShortDateString();
            this.labelDatumUkonceniAP.Text = string.Empty;
            this.labelZakaznikAP.Text = this.akcniPlany_.ZakaznikNazev ?? "";

            var ukonceniAP = this.GetUkonceniAPId(this.akcniPlany_.Id);

            foreach (var u in ukonceniAP)
            {
                this.labelDatumUkonceniAP.Text = u.DatumUkonceniAP.ToShortDateString();
                break;
            }

            this.CreateColumns();

            //tento if je pro aktualizaci bodů AP
            if (this.volani_ == 2)
            {
                this.ZobrazeniDGV();
            }

            if (this.DataGridViewBodyAP.Rows.Count < 1)
            {
                this.ButtonOpravitBodAP.Enabled = false;
            }

            if (this.akcniPlany_.APUzavren)
            {
                this.ButtonNovyBodAP.Visible = false;
                this.ButtonOdeslatEmail.Visible = false;
                this.ButtonZavrit.Text = "Close";
                this.ButtonOpravitBodAP.Text = "Display Point AP";
            }
            else
            {
                this.ButtonNovyBodAP.Visible = true;
                this.ButtonOdeslatEmail.Visible = true;
                this.ButtonZavrit.Text = "Close";
                this.ButtonOpravitBodAP.Text = "Edit Point AP";
            }

            //tato část bude spuštěna z emailu
            if (this.spusteniBezParametru_)
            {
                return;
            }

            using var form = this.formZadaniBoduAp;
            form.CreateFormZadaniBoduAP(this.labelCisloAP.Text, this.akcniPlany_,
                this.DataGridViewBodyAP.CurrentCell.RowIndex, false);

            form.ShowDialog();
            this.NacistDGV();

            //this.ButtonOdeslatEmail.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonOdeslatEmail_MouseClick);

        }

        public IEnumerable<PrehledBoduAPViewModel> GetUkonceniBodAP(int bodAPId)
        {
            var ukonceniBodAP = this.actionPlanPointRepository.GetUkonceniBodAP(bodAPId).ToList();

            if (!ukonceniBodAP.Any())
            {
                yield break;
            }

            var query = from u in ukonceniBodAP
                where u.StavObjektuUkonceni == 1
                orderby u.Id
                select PrehledBoduAPViewModel.UkonceniBodAP(u.Id, u.BodAPId, u.DatumUkonceni, u.Poznamka, u.Odpoved, u.StavZadosti, u.StavObjektuUkonceni);

            foreach (var q in query)
            {
                yield return q;
            }
        }

        private void ZobrazeniDGV()
        {
            //tady se naplní prom bodyAP z databáze
            var bodyAP_ = this.GetBodyIdAPAll(this.akcniPlany_.Id).ToList();
            var odpOsoba2 = this.employeeRepository.GetZamestnanciAll().ToList();

            var i = 0;
            DateTime? datumUkonceni_;
            var aktivovatTlacitkoOdeslatEmail = false;

            foreach (var b in bodyAP_)
            {
                var datumUkonceni = this.GetUkonceniBodAP(b.Id).ToList();
                datumUkonceni.Reverse();
                datumUkonceni_ = null;
                foreach (var du in datumUkonceni)
                {
                    if (du.StavZadosti != 1 && du.StavZadosti != 4 && du.StavZadosti != 5)
                    {
                        continue;
                    }

                    datumUkonceni_ = du.DatumUkonceni;
                    break;
                }

                string odpovednaOsoba2;
                if (b.OdpovednaOsoba2Id == null)
                {
                    odpovednaOsoba2 = string.Empty;
                }
                else
                {
                    var id = Convert.ToInt32(b.OdpovednaOsoba2Id);
                    var vyhledaneJmeno = odpOsoba2.Find(x => x.Id == id);
                    odpovednaOsoba2 = vyhledaneJmeno.Jmeno;
                }

                bodyAP.Add(new BodAP(b.Id, b.IdAP, b.CisloBoduAP, b.DatumZalozeni, b.OdkazNaNormu,
                    b.HodnoceniNeshody, b.PopisProblemu,
                    b.SkutecnaPricinaWM, b.NapravnaOpatreniWM, b.SkutecnaPricinaWS, b.NapravnaOpatreniWS,
                    b.OdpovednaOsoba1Id, b.OdpovednaOsoba2Id, b.OdpovednaOsoba1, odpovednaOsoba2, b.KontrolaEfektivnosti,
                    b.OddeleniId, b.Oddeleni, b.Priloha,
                    b.ZamitnutiTerminu, b.ZmenaTerminu, b.ZnovuOtevrit, true, b.EmailOdeslan, b.StavObjektuBodAP));
                //tabulka pro DGV
                this.dtBodyAP.Rows.Add(b.CisloBoduAP, b.OdkazNaNormu, b.HodnoceniNeshody, b.PopisProblemu,
                    b.OdpovednaOsoba1, odpovednaOsoba2, b.Oddeleni,
                    b.SkutecnaPricinaWM ?? string.Empty,
                    b.NapravnaOpatreniWM ?? string.Empty,
                    b.SkutecnaPricinaWS ?? string.Empty,
                    b.NapravnaOpatreniWS ?? string.Empty,
                    datumUkonceni_ == null ? string.Empty : Convert.ToDateTime(datumUkonceni_).ToShortDateString(),
                    b.KontrolaEfektivnosti == null
                        ? string.Empty
                        : Convert.ToDateTime(b.KontrolaEfektivnosti).ToShortDateString(), b.ZnovuOtevrit);

                //var ukonceniBodAP = PrehledBoduAPViewModel.GetUkonceniBodAP(b.Id).ToList();
                datumUkonceni.Reverse();

                if (datumUkonceni.Count != 0)
                {
                    bodyAP[i].UkonceniBodAP = new List<UkonceniBodAP>();
                    foreach (var u in datumUkonceni)
                    {
                        bodyAP[i].UkonceniBodAP.Add(new UkonceniBodAP(u.Id, u.BodAPId, u.DatumUkonceni, u.Poznamka,
                            u.Odpoved, u.StavZadosti, u.StavObjektuBodAP, true));
                    }
                }

                if (b.EmailOdeslan == false)
                {
                    aktivovatTlacitkoOdeslatEmail = true;
                }

                i++;
            }

            this.ButtonOdeslatEmail.Enabled = aktivovatTlacitkoOdeslatEmail;

            i = 0;
            foreach (var b in bodyAP_)
            {
                if (b.KontrolaEfektivnosti != null)
                {
                    this.DataGridViewBodyAP.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                i++;
            }
        }

        public IEnumerable<PrehledBoduAPViewModel> GetBodyIdAPAll(int idAP)
        {
            var bodyAP = this.actionPlanPointRepository.GetBodyIdAP(idAP).ToList();
            var zamestnanci = this.employeeRepository.GetZamestnanciAll().ToList();
            var oddeleni = this.departmentRepository.GetOddeleniAll();

            if (!bodyAP.Any())
            {
                yield break;
            }

            var query = from b in bodyAP
                join zam in zamestnanci
                    on b.OdpovednaOsoba1Id equals zam.Id into gZam
                from subZam in gZam.DefaultIfEmpty()
                join odd in oddeleni
                    on b.OddeleniId equals odd.Id into gOdd
                from subOdd in gOdd.DefaultIfEmpty()
                orderby b.CisloBoduAP
                select PrehledBoduAPViewModel.BodyAP(b.Id, b.AkcniPlanId, b.CisloBoduAP, b.DatumZalozeni, b.OdkazNaNormu, b.HodnoceniNeshody, b.PopisProblemu,
                    b.SkutecnaPricinaWM, b.NapravnaOpatreniWM, b.SkutecnaPricinaWS, b.NapravnaOpatreniWS,
                    b.OdpovednaOsoba1Id, b.OdpovednaOsoba2Id, subZam.Prijmeni + " " + subZam.Jmeno, b.KontrolaEfektivnosti, b.OddeleniId, subOdd.Nazev, b.Priloha,
                    b.ZamitnutiTerminu, b.ZmenaTerminu, b.ZnovuOtevrit, b.EmailOdeslan, b.StavObjektu);

            foreach (var q in query)
            {
                yield return q;
            }
        }

        private void CreateColumns()
        {
            this.dtBodyAP.Columns.Add(new DataColumn("cisloBodAP", typeof(int)));
            this.dtBodyAP.Columns.Add(new DataColumn("textBoxOdkazNaNormu", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("textBoxHodnoceniNeshody", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("textBoxPopisProblemu", typeof(string)));

            this.dtBodyAP.Columns.Add(new DataColumn("OdpovednyPracovnik1", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("OdpovednyPracovnik2", typeof(string)));

            this.dtBodyAP.Columns.Add(new DataColumn("Oddeleni", typeof(string)));

            this.dtBodyAP.Columns.Add(new DataColumn("WMPricina", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("WMOpatreni", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("WSPricina", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("WSOpatreni", typeof(string)));

            this.dtBodyAP.Columns.Add(new DataColumn("DatumUkonceni", typeof(string)));
            this.dtBodyAP.Columns.Add(new DataColumn("Efektivita", typeof(string)));

            this.DataGridViewBodyAP.Columns["cisloBodAP"].HeaderText = @"AP point number";
            this.DataGridViewBodyAP.Columns["cisloBodAP"].Width = 120;

            this.DataGridViewBodyAP.Columns["textBoxOdkazNaNormu"].HeaderText = @"Standard chapter";
            this.DataGridViewBodyAP.Columns["textBoxOdkazNaNormu"].Width = 140;

            this.DataGridViewBodyAP.Columns["textBoxHodnoceniNeshody"].HeaderText = @"Evaluation";
            this.DataGridViewBodyAP.Columns["textBoxHodnoceniNeshody"].Width = 120;

            this.DataGridViewBodyAP.Columns["textBoxPopisProblemu"].HeaderText = @"Description of the problem";
            this.DataGridViewBodyAP.Columns["textBoxPopisProblemu"].Width = 200;

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

            this.dtBodyAP.Columns.Add(new DataColumn("reopen", typeof(bool)));
            this.DataGridViewBodyAP.Columns["reopen"].HeaderText = @"After reopen";
            this.DataGridViewBodyAP.Columns["reopen"].Width = 110;

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

        private void ButtonNovyBodAP_MouseClick(object sender, MouseEventArgs e)
        {
            var cisloRadkyDGV = -1;

            using var form = this.formZadaniBoduAp;
            form.CreateFormZadaniBoduAP(this.labelCisloAP.Text, this.akcniPlany_, cisloRadkyDGV, true);
            form.ShowDialog();
            this.NacistDGV();
        }

        private void ButtonOpravitBodAP_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.DataGridViewBodyAP.CurrentCell.RowIndex < 0)
            {
                return;
            }

            using var form = this.formZadaniBoduAp;
            form.CreateFormZadaniBoduAP(this.labelCisloAP.Text, this.akcniPlany_,
                this.DataGridViewBodyAP.CurrentCell.RowIndex, false);
            form.ShowDialog();
            this.NacistDGV();
        }

        private void NacistDGV()
        {
            //nejdřív odstraním všechny řádky z dtBody
            // poznámka
            if (this.dtBodyAP.Rows.Count > 0)
            {
                for (var i = this.dtBodyAP.Rows.Count - 1; i >= 0; i--)
                {
                    this.dtBodyAP.Rows.Remove(this.dtBodyAP.Rows[i]);
                }

                this.dtBodyAP.Rows.Clear();
                bodyAP = new List<BodAP>();
            }

            var bodyAP_ = this.GetBodyIdAPAll(this.akcniPlany_.Id).ToList();
            var odpOsoba2 = this.employeeRepository.GetZamestnanciAll().ToList();

            DateTime? datumUkonceni_;
            var j = 0;
            foreach (var b in bodyAP_)
            {
                var datumUkonceni = this.GetUkonceniBodAP(b.Id).ToList();
                datumUkonceni.Reverse();
                datumUkonceni_ = null;
                foreach (var du in datumUkonceni)
                {
                    if (du.StavZadosti != 1 && du.StavZadosti != 4 && du.StavZadosti != 5)
                    {
                        continue;
                    }

                    datumUkonceni_ = du.DatumUkonceni;
                    break;
                }

                string odpovednaOsoba2;
                if (b.OdpovednaOsoba2Id == null)
                {
                    odpovednaOsoba2 = string.Empty;
                }
                else
                {
                    var id = Convert.ToInt32(b.OdpovednaOsoba2Id);
                    var vyhledaneJmeno = odpOsoba2.Find(x => x.Id == id);
                    odpovednaOsoba2 = vyhledaneJmeno.Jmeno;
                }

                bodyAP.Add(new BodAP(b.Id, b.IdAP, b.CisloBoduAP, b.DatumZalozeni, b.OdkazNaNormu, b.HodnoceniNeshody,
                    b.PopisProblemu,
                    b.SkutecnaPricinaWM, b.NapravnaOpatreniWM, b.SkutecnaPricinaWS, b.NapravnaOpatreniWS,
                    b.OdpovednaOsoba1Id, b.OdpovednaOsoba2Id, b.OdpovednaOsoba1, b.OdpovednaOsoba2, b.KontrolaEfektivnosti, b.OddeleniId,
                    b.Oddeleni, b.Priloha,
                    b.ZamitnutiTerminu, b.ZmenaTerminu, b.ZnovuOtevrit, true, b.EmailOdeslan, b.StavObjektuBodAP));

                this.dtBodyAP.Rows.Add(b.CisloBoduAP, b.OdkazNaNormu, b.HodnoceniNeshody, b.PopisProblemu,
                    b.OdpovednaOsoba1, odpovednaOsoba2, b.Oddeleni, b.SkutecnaPricinaWM ?? string.Empty,
                    b.NapravnaOpatreniWM ?? string.Empty, b.SkutecnaPricinaWS ?? string.Empty,
                    b.NapravnaOpatreniWS ?? string.Empty,
                    datumUkonceni_ == null ? string.Empty : Convert.ToDateTime(datumUkonceni_).ToShortDateString(),
                    b.KontrolaEfektivnosti == null
                        ? string.Empty
                        : Convert.ToDateTime(b.KontrolaEfektivnosti).ToShortDateString(), b.ZnovuOtevrit);

                if (datumUkonceni.Count != 0)
                {
                    datumUkonceni.Reverse();
                    bodyAP[j].UkonceniBodAP = new List<UkonceniBodAP>();
                    foreach (var u in datumUkonceni)
                    {
                        bodyAP[j].UkonceniBodAP.Add(new UkonceniBodAP(u.Id, u.BodAPId, u.DatumUkonceni, u.Poznamka,
                            u.Odpoved, u.StavZadosti, u.StavObjektuBodAP, true));
                    }
                }
                j++;
            }

            j = 0;
            // nastaví zelenou barvu pro řádky, které mají nastavenou efektivnost
            // povolí tlačítko pro odeslání emailů nových bodů AP
            var aktivovatTlacitkoOdeslatEmail = false;
            foreach (var b in bodyAP_)
            {
                if (b.KontrolaEfektivnosti != null)
                {
                    this.DataGridViewBodyAP.Rows[j].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                if (b.EmailOdeslan == false)
                {
                    aktivovatTlacitkoOdeslatEmail = true;
                }
                j++;
            }

            this.ButtonOdeslatEmail.Enabled = aktivovatTlacitkoOdeslatEmail;

            this.ButtonOpravitBodAP.Enabled = this.DataGridViewBodyAP.Rows.Count >= 1;
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e) => this.Close();

        private void FormPrehledBoduAP_FormClosing(object sender, FormClosingEventArgs e) { }

        private void ButtonOdeslatEmail_MouseClick(object sender, MouseEventArgs e)
        {
            bool naselOdpPrac;
            // seznam bodů AP pro jednoho odpovědného pracovníka
            var emailyKOdeslani1 = new List<List<int>>();
            var emailyKOdeslani2 = new List<List<int>>();
            // pro jaké body AP bude nastaveno pro sloupec EmailOdeslan true
            var odeslaneEmaily1 = new List<List<int>>();

            foreach (var b in bodyAP)
            {
                if (b.EmailOdeslan)
                {
                    continue;
                }

                // odpovědny 1
                if (emailyKOdeslani1.Count == 0)
                {
                    emailyKOdeslani1.Add(new List<int>());
                    emailyKOdeslani1[0].Add(b.OdpovednaOsoba1Id);
                    emailyKOdeslani1[0].Add(b.CisloBoduAP);

                    odeslaneEmaily1.Add(new List<int>());
                    odeslaneEmaily1[0].Add(b.Id);
                }
                else
                {
                    // odeslat email s informací o nových bodech
                    naselOdpPrac = false;
                    var j = 0;
                    foreach (var jedenEmail in emailyKOdeslani1)
                    {
                        // pro daného pracovníka přidá další bod AP
                        if (b.OdpovednaOsoba1Id == jedenEmail[0])
                        {
                            naselOdpPrac = true;
                            // bude přidán další bodAP do seznamu
                            jedenEmail.Add(b.CisloBoduAP);

                            odeslaneEmaily1[j].Add(b.Id);
                        }
                        j++;
                    }
                    if (naselOdpPrac == false)
                    {
                        // bude přidán další odpovědný pracovník do seznamu
                        emailyKOdeslani1.Add(new List<int>());
                        var lastItem = emailyKOdeslani1.Last();

                        lastItem.Add(b.OdpovednaOsoba1Id);
                        lastItem.Add(b.CisloBoduAP);

                        odeslaneEmaily1.Add(new List<int>());
                        var lastItemOdeslaneEmaily = odeslaneEmaily1.Last();
                        lastItemOdeslaneEmaily.Add(b.Id);
                    }
                }

                // odpovědný 2
                if (emailyKOdeslani2.Count == 0)
                {
                    if (b.OdpovednaOsoba2Id == null)
                    {
                        continue;
                    }

                    emailyKOdeslani2.Add(new List<int>());
                    emailyKOdeslani2[0].Add(Convert.ToInt32(b.OdpovednaOsoba2Id));
                    emailyKOdeslani2[0].Add(b.CisloBoduAP);
                }
                else
                {
                    if (b.OdpovednaOsoba2Id == null)
                    {
                        continue;
                    }

                    // odeslat email s informací o nových bodech
                    naselOdpPrac = false;
                    var j = 0;
                    foreach (var jedenEmail in emailyKOdeslani2)
                    {
                        // pro daného pracovníka přidá další bod AP
                        if (Convert.ToInt32(b.OdpovednaOsoba2Id) == jedenEmail[0])
                        {
                            naselOdpPrac = true;
                            // bude přidán další bodAP do seznamu
                            jedenEmail.Add(b.CisloBoduAP);
                        }
                        j++;
                    }

                    if (naselOdpPrac)
                    {
                        continue;
                    }

                    // bude přidán další odpovědný pracovník do seznamu
                    emailyKOdeslani2.Add(new List<int>());
                    var lastItem = emailyKOdeslani2.Last();

                    lastItem.Add(Convert.ToInt32(b.OdpovednaOsoba2Id));
                    lastItem.Add(b.CisloBoduAP);
                }
            }

            // vytvořit email k odeslání
            var i = 0;
            if (emailyKOdeslani1.Count > 0)
            {
                foreach (var jedenEmail in emailyKOdeslani1)
                {
                    // id odpovědného pracovníka
                    var emailTo = this.employeeRepository.GetOdpovednyPracovnikId(jedenEmail[0]).FirstOrDefault();
                    var emailOdpovPrac = Convert.ToString(emailTo?.Email);

                    var htmlText = @"<p>Action plan: " + this.akcniPlany_.CisloAPRok + @"</p>";
                    htmlText += @"<p>Responsible #1:<br>";
                    htmlText += emailTo?.Jmeno + @"</p>";
                    htmlText += @"<p>Points AP: ";

                    var j = 0;
                    foreach (var bodyAP in jedenEmail)
                    {
                        if (j != 0)
                        {
                            if (j >= 2)
                            {
                                htmlText += ", " + bodyAP;
                            }
                            else
                            {
                                htmlText += bodyAP;
                            }
                        }

                        j++;
                    }
                    htmlText += @"</p>";
                    // uloží zprávu do tabulky OdeslatEmail
                    this.emailRepository.InsertEmailOdpovedny1(emailOdpovPrac, @"New points AP", htmlText, odeslaneEmaily1[i]);

                    i++;
                }
            }

            if (emailyKOdeslani2.Count > 0)
            {
                foreach (var jedenEmail in emailyKOdeslani2)
                {
                    // id odpovědného pracovníka
                    var emailTo = this.employeeRepository.GetOdpovednyPracovnikId(jedenEmail[0]).FirstOrDefault();
                    var emailOdpovPrac = Convert.ToString(emailTo?.Email);

                    var htmlText = @"<p>Action plan: " + this.akcniPlany_.CisloAPRok + @"</p>";
                    htmlText += @"<p>Responsible #2:<br>";
                    htmlText += emailTo?.Jmeno + @"</p>";
                    htmlText += @"<p>Points AP: ";

                    var j = 0;
                    foreach (var bodyAP in jedenEmail)
                    {
                        if (j != 0)
                        {
                            if (j >= 2)
                            {
                                htmlText += ", " + bodyAP;
                            }
                            else
                            {
                                htmlText += bodyAP;
                            }
                        }

                        j++;
                    }
                    htmlText += @"</p>";
                    // uloží zprávu do tabulky OdeslatEmail
                    this.emailRepository.InsertEmailOdpovedny2(emailOdpovPrac, @"New points AP", htmlText);
                }
            }

            this.NacistDGV();
            // spustí se externí aplikace pro odeslání emailů
        }
    }
}
