using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

using LearActionPlans.Models;
using LearActionPlans.DataMappers;
using LearActionPlans.ViewModels;
using LearActionPlans.Utilities;

namespace LearActionPlans.Views
{
    public partial class FormPrehledBoduAP : Form
    {
        //při volání konstruktoru vytvořím novou prom jako readonly, protože ji nastavím při volání konstruktoru a pak už její hodnoty neměním
        private readonly FormNovyAkcniPlan.AkcniPlanTmp akcniPlany_;

        //tady udržuji všechny informace o bodech AP
        public static List<BodAP> bodyAP;

        //do této proměnné uložím pouze hodnoty, které se zobrazí v DGV
        private DataTable dtBodyAP;

        private readonly BindingSource bindingSource;
        //public static bool UlozitBodyAP { get; set; }

        private readonly byte volani_;
        //1 - založení nového AP
        //2 - aktualizace již vytvořeného AP
        //3 - bude volání z formuláře FormVsechnyBodyAP

        private readonly bool spusteniBezParametru_;
        private readonly int vybranyBodId;
        private int vybranyRadek;

        // vybranyRadek - používá se při volbě z přehledu všech bodů AP, to abych zvýraznil vybraný bod k editaci
        public FormPrehledBoduAP(bool spusteniBezParametru, FormNovyAkcniPlan.AkcniPlanTmp akcniPlany, byte volani, int vybranyBodId)
        {
            this.InitializeComponent();
            this.bindingSource = new BindingSource();
            bodyAP = new List<BodAP>();
            this.dtBodyAP = new DataTable();

            this.spusteniBezParametru_ = spusteniBezParametru;
            this.akcniPlany_ = akcniPlany;
            this.volani_ = volani;
            this.vybranyBodId = vybranyBodId;
        }

        private void FormPrehledBoduAP_Load(object sender, EventArgs e)
        {
            this.bindingSource.DataSource = this.dtBodyAP;
            this.DataGridViewBodyAP.DataSource = this.bindingSource;

            this.labelCisloAP.Text = this.akcniPlany_.CisloAPRok ?? "";
            this.labelZadavatel1Zadano.Text = this.akcniPlany_.Zadavatel1Jmeno ?? "";
            //this.labelZadavatel2Zadano.Text = this.akcniPlany_.Zadavatel2Jmeno ?? (this.akcniPlany_.Zadavatel2Jmeno = "");
            this.labelZadavatel2Zadano.Text = (this.akcniPlany_.Zadavatel2Jmeno ??= "");
            this.labelTemaAP.Text = this.akcniPlany_.Tema ?? "";
            this.labelProjektAP.Text = this.akcniPlany_.ProjektNazev ?? "";
            this.labelDatumZahajeniAP.Text = this.akcniPlany_.DatumZalozeni == null ? "" : Convert.ToDateTime(this.akcniPlany_.DatumZalozeni).ToShortDateString();
            this.labelDatumUkonceniAP.Text = string.Empty;
            this.labelDatumUkonceniAP.Text = this.akcniPlany_.DatumUkonceni == null ? "" : Convert.ToDateTime(this.akcniPlany_.DatumUkonceni).ToShortDateString();
            this.labelZakaznikAP.Text = this.akcniPlany_.ZakaznikNazev ?? "";

            var ukonceniAP = PrehledBoduAPViewModel.GetUkonceniAPId(this.akcniPlany_.Id);

            foreach (var u in ukonceniAP)
            {
                this.labelDatumUkonceniAP.Text = u.DatumUkonceniAP.ToShortDateString();
                break;
            }

            this.CreateColumns();

            //tento if je pro aktualizaci bodů AP
            // 1 - volání z vytvoření nového AP, žádné body zatím nejsou vytvořeny, tak se nebude ani zobrazovat seznam bodů
            // 2 - volání z přehledu AP i z přehledu bodů AP
            if (this.volani_ == 2)
            {
                this.ZobrazeniDGV();
            }

            if (this.DataGridViewBodyAP.Rows.Count < 1)
            {
                this.ButtonOpravitBodAP.Enabled = false;
            }

            if (this.akcniPlany_.APUzavren == true)
            {
                this.ButtonNovyBodAP.Visible = false;
                this.ButtonOdeslatEmail.Visible = false;
                this.ButtonZavrit.Text = "Close";
                this.ButtonOpravitBodAP.Text = "Display AP Point";
            }
            else
            {
                this.ButtonNovyBodAP.Visible = true;
                this.ButtonOdeslatEmail.Visible = true;
                this.ButtonZavrit.Text = "Close";
                this.ButtonOpravitBodAP.Text = "Edit AP Point";
            }

            if (this.spusteniBezParametru_)
            {
                return;
            }

            // tato část se vykoná, když bude program spuštěn z poštovního programu
            using var form = new FormZadaniBoduAP(this.spusteniBezParametru_, this.labelCisloAP.Text, this.akcniPlany_,
                this.vybranyRadek, false);

            _ = form.ShowDialog();
            // proč je tady this.NacistDGV(), mohu odstranit ??
            //this.NacistDGV();

            //this.ButtonOdeslatEmail.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonOdeslatEmail_MouseClick);
        }

        private void ZobrazeniDGV()
        {
            //tady se naplní prom bodyAP z databáze
            var bodyAP_ = PrehledBoduAPViewModel.GetBodyIdAPAll(this.akcniPlany_.Id).ToList();
            var odpOsoba2 = PrehledBoduAPViewModel.GetOdpovednaOsoba2().ToList();

            var i = 0;
            var datumUkonceni_ = (DateTime?)null;
            var aktivovatTlacitkoOdeslatEmail = false;

            foreach (var b in bodyAP_)
            {
                var datumUkonceni = PrehledBoduAPViewModel.GetUkonceniBodAP(b.Id).ToList();
                datumUkonceni.Reverse();
                datumUkonceni_ = null;
                foreach (var du in datumUkonceni)
                {
                    if (du.StavZadosti == 1 || du.StavZadosti == 2 || du.StavZadosti == 4 || du.StavZadosti == 5)
                    {
                        datumUkonceni_ = du.DatumUkonceni;
                        break;
                    }
                }

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
                    datumUkonceni_ == null
                        ? string.Empty
                        : Convert.ToDateTime(datumUkonceni_).ToShortDateString(),
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

            if (aktivovatTlacitkoOdeslatEmail == true)
            {
                this.ButtonOdeslatEmail.Enabled = true;
            }
            else
            {
                this.ButtonOdeslatEmail.Enabled = false;
            }

            i = 0;
            foreach (var b in bodyAP_)
            {
                if (b.KontrolaEfektivnosti != null)
                {
                    // obarví se řídky, kde je již vyplněna daum efektivity
                    this.DataGridViewBodyAP.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                if (b.Id == this.vybranyBodId)
                {
                    this.DataGridViewBodyAP.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                    this.vybranyRadek = i;
                }
                i++;
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

            this.dtBodyAP.Columns.Add(new DataColumn("Reopen", typeof(bool)));

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

            this.DataGridViewBodyAP.Columns["Reopen"].HeaderText = @"After reopen";
            this.DataGridViewBodyAP.Columns["Reopen"].Width = 110;

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

            using var form = new FormZadaniBoduAP(this.spusteniBezParametru_, this.labelCisloAP.Text, this.akcniPlany_, cisloRadkyDGV, true);
            _ = form.ShowDialog();
            this.NacistDGV();
        }

        private void ButtonOpravitBodAP_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.DataGridViewBodyAP.CurrentCell.RowIndex < 0)
            {
                return;
            }

            using var form = new FormZadaniBoduAP(this.spusteniBezParametru_, this.labelCisloAP.Text, this.akcniPlany_,
                this.DataGridViewBodyAP.CurrentCell.RowIndex, false);
            _ = form.ShowDialog();
            this.NacistDGV();
        }

        private void NacistDGV()
        {
            //nejdřív odstraním všechny řádky z dtBody
            if (this.dtBodyAP.Rows.Count > 0)
            {
                for (var i = this.dtBodyAP.Rows.Count - 1; i >= 0; i--)
                {
                    this.dtBodyAP.Rows.Remove(this.dtBodyAP.Rows[i]);
                }

                this.dtBodyAP.Rows.Clear();
                bodyAP = new List<BodAP>();
            }

            var bodyAP_ = PrehledBoduAPViewModel.GetBodyIdAPAll(this.akcniPlany_.Id).ToList();
            var odpOsoba2 = PrehledBoduAPViewModel.GetOdpovednaOsoba2().ToList();

            var datumUkonceni_ = (DateTime?)null;
            var j = 0;
            foreach (var b in bodyAP_)
            {
                var datumUkonceni = PrehledBoduAPViewModel.GetUkonceniBodAP(b.Id).ToList();
                datumUkonceni.Reverse();
                datumUkonceni_ = null;
                foreach (var du in datumUkonceni)
                {
                    if (du.StavZadosti == 1 || du.StavZadosti == 2 || du.StavZadosti == 4 || du.StavZadosti == 5)
                    {
                        datumUkonceni_ = du.DatumUkonceni;
                        break;
                    }
                }

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

                //var ukonceniBodAP = PrehledBoduAPViewModel.GetUkonceniBodAP(b.Id).ToList();

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
                if (b.Id == this.vybranyBodId)
                {
                    this.DataGridViewBodyAP.Rows[j].DefaultCellStyle.BackColor = Color.LightBlue;
                }
                if (b.EmailOdeslan == false)
                {
                    aktivovatTlacitkoOdeslatEmail = true;
                }
                j++;
            }

            if (aktivovatTlacitkoOdeslatEmail == true)
            {
                this.ButtonOdeslatEmail.Enabled = true;
            }
            else
            {
                this.ButtonOdeslatEmail.Enabled = false;
            }

            //pak naplním tabulku znovu
            //foreach (var b in bodyAP)
            //{
            //    this.dtBodyAP.Rows.Add(b.CisloBoduAP, b.OdkazNaNormu, b.HodnoceniNeshody, b.PopisProblemu,
            //        b.OdpovednaOsoba1, string.Empty, b.Oddeleni, b.SkutecnaPricinaWM ?? string.Empty,
            //        b.NapravnaOpatreniWM ?? string.Empty, b.SkutecnaPricinaWS ?? string.Empty,
            //        b.NapravnaOpatreniWS ?? string.Empty,
            //        datumUkonceni_ == null ? string.Empty : Convert.ToDateTime(datumUkonceni_).ToShortDateString(),
            //        b.KontrolaEfektivnosti == null
            //            ? string.Empty
            //            : Convert.ToDateTime(b.KontrolaEfektivnosti).ToShortDateString(), b.ZnovuOtevrit);
            //}

            this.ButtonOpravitBodAP.Enabled = this.DataGridViewBodyAP.Rows.Count >= 1;
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e) => this.Close();

        private void FormPrehledBoduAP_FormClosing(object sender, FormClosingEventArgs e) { }

        private void ButtonOdeslatEmail_MouseClick(object sender, MouseEventArgs e)
        {
            var naselOdpPrac = false;
            // seznam bodů AP pro jednoho odpovědného pracovníka
            var emailyKOdeslani1 = new List<List<int>>();
            var emailyKOdeslani2 = new List<List<int>>();
            // pro jaké body AP bude nastaveno pro sloupec EmailOdeslan true
            var odeslaneEmaily1 = new List<List<int>>();

            foreach (var b in bodyAP)
            {
                if (b.EmailOdeslan == false)
                {
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
                        if (!(b.OdpovednaOsoba2Id == null))
                        {
                            emailyKOdeslani2.Add(new List<int>());
                            emailyKOdeslani2[0].Add(Convert.ToInt32(b.OdpovednaOsoba2Id));
                            emailyKOdeslani2[0].Add(b.CisloBoduAP);
                        }
                    }
                    else
                    {
                        if (!(b.OdpovednaOsoba2Id == null))
                        {
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
                            if (naselOdpPrac == false)
                            {
                                // bude přidán další odpovědný pracovník do seznamu
                                emailyKOdeslani2.Add(new List<int>());
                                var lastItem = emailyKOdeslani2.Last();

                                lastItem.Add(Convert.ToInt32(b.OdpovednaOsoba2Id));
                                lastItem.Add(b.CisloBoduAP);
                            }
                        }
                    }
                }
            }

            // vytvořit email k odeslání
            var i = 0;
            if (emailyKOdeslani1.Count > 0)
            {
                foreach (var jedenEmail in emailyKOdeslani1)
                {
                    // id odpovědného pracovníka
                    var emailTo = PrehledBoduAPViewModel.GetOdpovednyPracovnik(jedenEmail[0]).ToList();
                    var emailOdpovPrac = Convert.ToString(emailTo[0].EmailOdpovednyPracovnik);

                    var htmlText = @"<p>Action plan: " + this.akcniPlany_.CisloAPRok + @"</p>";
                    htmlText += @"<p>Responsible #1:<br>";
                    htmlText += emailTo[0].JmenoPracovnika + @"</p>";
                    htmlText += @"<p>Points AP: ";

                    var j = 0;
                    foreach (var bodAkcniPlan in jedenEmail)
                    {
                        if (j == 0)
                        { }
                        else
                        {
                            if (j >= 2)
                            {
                                htmlText += ", " + bodAkcniPlan;
                            }
                            else
                            {
                                htmlText += bodAkcniPlan;
                            }
                        }
                        j++;
                    }
                    htmlText += @"</p>";
                    // uloží zprávu do tabulky OdeslatEmail
                    OdeslatEmailDataMapper.InsertEmailOdpovedny1(emailOdpovPrac, @"New points AP", htmlText, odeslaneEmaily1[i]);

                    i++;
                }
            }

            if (emailyKOdeslani2.Count > 0)
            {
                foreach (var jedenEmail in emailyKOdeslani2)
                {
                    // id odpovědného pracovníka
                    var emailTo = PrehledBoduAPViewModel.GetOdpovednyPracovnik(jedenEmail[0]).ToList();
                    var emailOdpovPrac = Convert.ToString(emailTo[0].EmailOdpovednyPracovnik);

                    var htmlText = @"<p>Action plan: " + this.akcniPlany_.CisloAPRok + @"</p>";
                    htmlText += @"<p>Responsible #2:<br>";
                    htmlText += emailTo[0].JmenoPracovnika + @"</p>";
                    htmlText += @"<p>Points AP: ";

                    var j = 0;
                    foreach (var bodyAP in jedenEmail)
                    {
                        if (j == 0)
                        { }
                        else
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
                    OdeslatEmailDataMapper.InsertEmailOdpovedny2(emailOdpovPrac, @"New points AP", htmlText);
                }
            }
            // spustí se externí aplikace pro odeslání emailů
            // tady odešlu emaily o založení nových bodů
            Helper.OdeslatEmail();

            this.NacistDGV();
        }
    }
}
