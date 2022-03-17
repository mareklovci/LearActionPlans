using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

using LearActionPlans.ViewModels;
using LearActionPlans.DataMappers;
using LearActionPlans.Utilities;

namespace LearActionPlans.Views
{
    public partial class FormPosunutiTerminuBodAP : Form
    {
        //private readonly DataRow action_;
        private readonly int cisloRadkyDGVBody;
        private readonly string cisloAPStr_;
        private readonly int apId;
        private readonly int bodAPId;

        private readonly bool spusteniBezParametru;

        private readonly int zadavatel1Id_;
        private readonly int? zadavatel2Id_;
        private string zadavatel1Email;
        private string zadavatel2Email;

        private Panel panelTerminy;
        private Label labelZamTer;
        private Label labelZmenyTer;
        private Label labelZamTerPocet;
        private Label labelZmenyTerPocet;

        private List<GroupBox> groupBoxTerminy;
        private List<Label> labelTerminyDatum;
        private List<Label> labelStatus;
        private List<RichTextBox> richTextBoxPoznamka;
        private List<Label> labelOdpoved;
        private List<RichTextBox> richTextBoxOdpoved;
        private DataTable dtUkonceni;

        //private readonly bool vlastnikAP_;
        //private readonly bool vlastnikAkce_;
        //private readonly int vlastnikAkceId_;
        private readonly bool opravitTermin_;

        private bool zmenaPoznamky;
        //povolení nebo zamítnutí změny poslední poznámky
        private bool zamitnutaZmena;
        private byte zmenaTerminu_;

        private List<PosunutiTerminuBodAPViewModel> ukonceni;
        //1 = první termín, 2 = první termín uzavřeno, 3 = žádost, 4 = nový termín potvrzeno, 5 = nový termín změna, 6 = nový termín zamítnuto
        //když vytvořím nový termín (3), předchozí první termín nastavím na (2)

        private readonly string[] zadost = { "first term", "first term closed", "request", "new deadline confirmed", "new deadline changed", "new deadline reject" };
        //private readonly bool kontrolaEfektivnosti_;

        enum Status : byte
        {
            PrvniTermin = 1,
            PrvniterminUzavreno = 2,
            Navrh = 3,
            Potvrzeni = 4,
            Zmena = 5,
            Zamitnuti = 6
        }

        //public FormPosunutiTerminuBodAP(bool opravitTermin, string cisloAPStr, int cisloRadkyDGV, DataRow action)
        public FormPosunutiTerminuBodAP(bool spusteniBezParametru, bool opravitTermin, string cisloAPStr, int cisloRadkyDGV, int zadavatel1Id, int? zadavatel2Id)
        {
            //bool kontrolaEfektivnosti,
            this.InitializeComponent();
            //kontrolaEfektivnosti_ = kontrolaEfektivnosti;

            this.spusteniBezParametru = spusteniBezParametru;

            //vlastnikAP_ = vlastnikAP;
            //vlastnikAkce_ = vlastnikAkce;
            //vlastnikAkceId_ = vlastnikAkceId;
            
            //když je opravitTermin = true, tak AP ještě není uazavřen
            this.opravitTermin_ = opravitTermin;
            this.zmenaPoznamky = false;
            this.zamitnutaZmena = false;
            //action_ = action;
            this.cisloRadkyDGVBody = cisloRadkyDGV;
            this.cisloAPStr_ = cisloAPStr;
            this.apId = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].AkcniPlanId;
            this.bodAPId = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Id;
            this.zadavatel1Id_ = zadavatel1Id;
            this.zadavatel2Id_ = zadavatel2Id;

            this.InitControls();
        }

        private void InitControls()
        {
            this.panelTerminy = new Panel();
            this.groupBoxTerminy = new List<GroupBox>();
            this.labelTerminyDatum = new List<Label>();
            this.labelStatus = new List<Label>();
            this.richTextBoxPoznamka = new List<RichTextBox>();

            this.labelOdpoved = new List<Label>();
            this.richTextBoxOdpoved = new List<RichTextBox>();

            this.ukonceni = new List<PosunutiTerminuBodAPViewModel>();
        }

        private void FormPosunutiTerminuBodAP_Load(object sender, EventArgs e)
        {
            //kontrolaEfektivnosti_ == false &&  && (FormMain.VlastnikAP == true || FormMain.VlastnikAkce == true)
            if (this.opravitTermin_ == true)
            {
                this.ButtonUlozit.Visible = true;
                this.ButtonZavrit.Text = "Close";
                this.CheckBoxPoslatZadost.CheckedChanged += new EventHandler(this.CheckBoxPoslatZadost_CheckedChanged);
            }
            else
            {
                this.ButtonUlozit.Visible = false;
                this.ButtonZavrit.Text = "Close";
                //zneviditelní groupbox pro vytvoření nové žádosti
                this.groupBoxZadost.Visible = false;
            }

            //if (FormMain.VlastnikAP == true)
            //    ButtonZadost.Text = "Save a new Deadline";
            //if (FormMain.VlastnikAkce == true)
            //    ButtonZadost.Text = "Request for a new Deadline";

            this.VytvoritObsahPanelu();

            // získat emaily odpovědných pracovníků za daný AP
            var zadavatel1Email_ = PosunutiTerminuBodAPViewModel.GetEmailOdpovednyPracovnik(this.zadavatel1Id_).ToList();
            this.zadavatel1Email = zadavatel1Email_[0].Email;

            if (this.zadavatel2Id_ != null)
            {
                var zadavatel2Email_ = PosunutiTerminuBodAPViewModel.GetEmailOdpovednyPracovnik(Convert.ToInt32(this.zadavatel2Id_)).ToList();
                this.zadavatel2Email = zadavatel2Email_[0].Email;
            }
        }

        //private void FormPosunutiTerminuAkce_Load(object sender, EventArgs e)
        //{
        //}

        private void VytvoritObsahPanelu()
        {
            this.richTextBoxNovaPoznamka.Text = string.Empty;

            //to je proto aby zareagoval handler
            //kontrolaEfektivnosti_ == false &&  && (FormMain.VlastnikAP == true || FormMain.VlastnikAkce == true)
            if (this.opravitTermin_ == true)
            {
                this.CheckBoxPoslatZadost.Checked = true;
                this.CheckBoxPoslatZadost.Checked = false;
            }

            this.panelTerminy.Size = new Size(760, 600);
            this.panelTerminy.Name = "panelPrehledTerminu";
            this.panelTerminy.Location = new Point(10, 10);
            this.panelTerminy.BorderStyle = BorderStyle.FixedSingle;
            this.panelTerminy.BackColor = SystemColors.Control;
            this.panelTerminy.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            this.panelTerminy.AutoScroll = false;
            this.panelTerminy.HorizontalScroll.Enabled = false;
            this.panelTerminy.HorizontalScroll.Visible = false;
            this.panelTerminy.HorizontalScroll.Maximum = 0;
            this.panelTerminy.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.panelTerminy.AutoScroll = true;

            this.Controls.Add(this.panelTerminy);

            //zbývající počet počet zamítnutí a změn
            
            //var akce = PosunutiTerminuBodAPViewModel.GetZbyvajiciTerminy(Convert.ToInt32(action_["akceId"])).ToList();
            var akce = PosunutiTerminuBodAPViewModel.GetZbyvajiciTerminy(FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Id).ToList();
            byte zamitnutiTerminu = 0;
            byte zmenaTerminu = 0;
            int i;

            //vyhledá datumy ukončení pro dané ID akce
            //jsou setříděny podle UkonceniAkceID
            //ukonceni = PosunutiTerminuBodAPViewModel.GetUkonceniAkce(Convert.ToInt32(action_["akceId"])).ToList();
            this.ukonceni = PosunutiTerminuBodAPViewModel.GetUkonceniBodAP(FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Id).ToList();
            this.dtUkonceni = DataTableConverter.ConvertToDataTable(this.ukonceni);
            //to je kvůli nastavení minimálního datumu pro datetimepicker - založení nového termínu
            var minDatumZmeny = this.ukonceni.OrderByDescending(item => item.UkonceniBodAPId);

            foreach (var a in akce)
            {
                zamitnutiTerminu = a.ZamitnutiTerminu;
                zmenaTerminu = a.ZmenaTerminu;
                this.zmenaTerminu_ = zmenaTerminu;
                //kontrolaEfektivnosti_ == false &&  && (FormMain.VlastnikAP == true || FormMain.VlastnikAkce == true)
                if (this.opravitTermin_ == true)
                {
                    //FormMain.VlastnikAP == true && 
                    if (zmenaTerminu == 0)
                    {
                        this.groupBoxZadost.Visible = false;
                    }

                    //FormMain.VlastnikAkce == true && 
                    if (zamitnutiTerminu == 0 || zmenaTerminu == 0)
                    {
                        //již nelze dále prodlužovat termíny
                        //když bude žádost 3x zamítnuta, nemůže již vlastník akce žádat o prodloužení
                        this.groupBoxZadost.Visible = false;
                        //protože už byly možné termíny majitele akce vyčerpány, bude zablována editace poznámek
                        this.zamitnutaZmena = true;
                    }
                    else
                    {
                        var minDatum = DateTime.Now;
                        i = 0;
                        foreach (var m in minDatumZmeny)
                        {
                            if (i == 0)
                            {
                                //nastavení toho, že můžu poslat žádost nebo ne
                                if (m.StavZadosti == 3)
                                {
                                    this.groupBoxZadost.Visible = false;
                                }
                                else
                                {
                                    this.groupBoxZadost.Visible = true;
                                }
                            }
                            //když bude poslední termín zamítnut, bude přeskočen
                            if (m.StavZadosti == 1 || m.StavZadosti == 2 || m.StavZadosti == 3 || m.StavZadosti == 4 || m.StavZadosti == 5)
                            {
                                //vybere datum pro první možnou změnu termínu
                                minDatum = m.DatumUkonceni.AddDays(1);
                                break;
                            }
                            i++;
                            //zamítnutí bude přeskočeno
                            //musím zjistit, jaký termín bude editovateln0
                        }

                        //nastavení minmálního datumu v datatimepickeru
                        this.dateTimePickerNovyTerminUkonceni.Value = minDatum;
                        this.dateTimePickerNovyTerminUkonceni.MinDate = minDatum;
                        if (minDatum < DateTime.Now)
                        {
                            this.dateTimePickerNovyTerminUkonceni.Value = DateTime.Now;
                        }
                    }
                }
            }

            //počet zamítnutí
            this.labelZamTer = new Label
            {
                Name = "labelZamitnuti",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel),
                Text = "Remaining number of rejections:"
            };
            this.panelTerminy.Controls.Add(this.labelZamTer);
            this.labelZamTerPocet = new Label
            {
                Name = "labelZamitnutiPocet",
                Location = new Point(250, 10),
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel),
                Text = "3"
            };
            this.labelZamTerPocet.Text = Convert.ToString(zamitnutiTerminu);
            this.panelTerminy.Controls.Add(this.labelZamTerPocet);

            //počet změn
            this.labelZmenyTer = new Label
            {
                Name = "labeZmeny",
                Location = new Point(10, 35),
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel),
                Text = "Remaining number of changes:"
            };
            this.panelTerminy.Controls.Add(this.labelZmenyTer);
            this.labelZmenyTerPocet = new Label
            {
                Name = "labelZmenyPocet",
                Location = new Point(250, 35),
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel),
                Text = "5"
            };
            this.labelZmenyTerPocet.Text = Convert.ToString(zmenaTerminu);
            this.panelTerminy.Controls.Add(this.labelZmenyTerPocet);

            i = 0;
            foreach (var u in this.ukonceni)
            {
                this.groupBoxTerminy.Add(new GroupBox()
                {
                    Name = "groupBoxTerminy" + i + 1,
                    Location = new Point(10, 60 + (i * 150)),
                    Size = new Size(720, 140),
                    Text = (i + 1).ToString() + ". term",
                });
                this.labelTerminyDatum.Add(new Label()
                {
                    Name = "labelTerminyDatum" + i + 1,
                    Location = new Point(10, 20),
                    AutoSize = true,
                    Text = u.DatumUkonceni.ToShortDateString(),
                    ForeColor = Color.Black
                });
                var delkaRetezce = 0;
                using (var graphics = Graphics.FromImage(new Bitmap(1, 1)))
                {
                    var size = graphics.MeasureString(u.DatumUkonceni.ToShortDateString(), new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Pixel));
                    delkaRetezce = Convert.ToInt32(size.Width);
                }
                string stavTerminu;
                var barva = Color.Black;
                switch (u.StavZadosti)
                {
                    case 1:
                        stavTerminu = this.zadost[0];
                        barva = Color.Black;
                        break;
                    case 2:
                        stavTerminu = this.zadost[1];
                        barva = Color.Black;
                        break;
                    case 3:
                        stavTerminu = this.zadost[2];
                        barva = Color.Black;
                        break;
                    case 4:
                        stavTerminu = this.zadost[3];
                        barva = Color.Green;
                        break;
                    case 5:
                        stavTerminu = this.zadost[4];
                        barva = Color.Blue;
                        break;
                    case 6:
                        stavTerminu = this.zadost[5];
                        barva = Color.Red;
                        break;
                    default:
                        stavTerminu = string.Empty;
                        break;
                }
                stavTerminu = "(" + stavTerminu + ")";
                this.labelStatus.Add(new Label()
                {
                    Name = "labelStatus" + i + 1,
                    Location = new Point(10 + delkaRetezce + 10, 20),
                    AutoSize = true,
                    Text = stavTerminu,
                    Tag = u.StavZadosti,
                    ForeColor = barva
                });
                //blokování editace pole Poznámka
                //to musím předělat
                //var podminka = u.StavZadosti == 1 || u.StavZadosti == 3 || u.StavZadosti == 4 || u.StavZadosti == 5;
                this.richTextBoxPoznamka.Add(new RichTextBox()
                {
                    Name = "RichTextBoxPoznamka" + i + 1,
                    Location = new Point(10, 45),
                    Size = new Size(340, 80),
                    Text = u.Poznamka,
                    //Enabled = podminka ? true : false,
                    Enabled = false,
                    Tag = u.UkonceniBodAPId.ToString(),
                    ForeColor = Color.Black
                });
                this.richTextBoxPoznamka[i].TextChanged += new EventHandler(this.RichTextBoxPoznamka_TextChanged);

                this.labelOdpoved.Add(new Label()
                {
                    Name = "labelOdpoved" + i + 1,
                    Location = new Point(370, 20),
                    AutoSize = true,
                    Text = "Reply",
                    ForeColor = Color.Black
                });
                this.richTextBoxOdpoved.Add(new RichTextBox()
                {
                    Name = "RichTextBoxOdpoved" + i + 1,
                    Location = new Point(370, 45),
                    Size = new Size(340, 80),
                    Text = u.Odpoved,
                    Enabled = false,
                    Tag = u.UkonceniBodAPId.ToString(),
                    ForeColor = Color.Black
                });

                i++;
            }

            //nastavím poslední Poznámku na povolit zápis
            //zamítnuté termíny nemůžu editovat
            //editovat lze pouze poslední termín, když není zamítnut
            i = this.richTextBoxPoznamka.Count - 1;
            foreach (var m in minDatumZmeny)
            {
                //poznámky projíždím od konce, stejně kako datumy
                if (i == this.richTextBoxPoznamka.Count - 1)
                {
                    if (m.StavZadosti == 6)
                    {
                        //zamítnutí termínu
                        this.richTextBoxPoznamka[i].Enabled = false;
                    }
                    else
                    {
                        //všechny ostatní varianty
                        //kontrolaEfektivnosti_ == true ||  && (FormMain.VlastnikAP == true || FormMain.VlastnikAkce == true)
                        if (this.opravitTermin_ == false)
                        {
                            //AP nebo akce je uzavřena
                            this.richTextBoxPoznamka[i].Enabled = false;
                        }
                        else
                        {
                            //když je počet zbývajících zamítnutí nebo 
                            this.richTextBoxPoznamka[i].Enabled = true;
                            //if (zamitnutaZmena == false)
                            //    richTextBoxPoznamka[i].Enabled = true;
                            //else
                            //    richTextBoxPoznamka[i].Enabled = false;
                        }
                    }
                }
                else
                {
                    this.richTextBoxPoznamka[i].Enabled = false;
                }
                i--;
                break;
            }

            for (i = 0; i < this.groupBoxTerminy.Count; i++)
            {
                //GroupBox itemGrpBox = groupBoxTerminy[i];
                this.panelTerminy.Controls.Add(this.groupBoxTerminy[i]);
                //Label itemLbl = labelTerminyDatum[i];
                this.groupBoxTerminy[i].Controls.Add(this.labelTerminyDatum[i]);
                this.groupBoxTerminy[i].Controls.Add(this.labelStatus[i]);
                this.groupBoxTerminy[i].Controls.Add(this.richTextBoxPoznamka[i]);

                this.groupBoxTerminy[i].Controls.Add(this.labelOdpoved[i]);
                this.groupBoxTerminy[i].Controls.Add(this.richTextBoxOdpoved[i]);

                if (i == 0)
                {
                    this.labelOdpoved[i].Visible = false;
                    this.richTextBoxOdpoved[i].Visible = false;
                }
            }

            this.ButtonUlozit.Enabled = false;
        }

        private void RichTextBoxPoznamka_TextChanged(object sender, EventArgs e)
        {
            var poznamka = (RichTextBox)sender;

            if (!string.IsNullOrEmpty(poznamka.Text))
            {
                //tady povolím možnost odeslat žádost, zviditelním pole pro odeslání žádosti
                this.ButtonUlozit.Enabled = true;
                this.zmenaPoznamky = true;
            }
        }

        private void ButtonZadost_MouseClick(object sender, MouseEventArgs e)
        {
            int idZadost;

            if (string.IsNullOrEmpty(this.richTextBoxNovaPoznamka.Text))
            {
                MessageBox.Show("You must fill in the note field.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //v případě, že byly provedeny úpravy v poznámce, bude nejdřív uložena
                if (this.ButtonUlozit.Enabled == true)
                {
                    this.UlozitPoznamky();
                }

                //var prvniTermin = PosunutiTerminuBodAPViewModel.GetZavritPrvniTermin(Convert.ToInt32(action_["akceId"])).ToList();
                var prvniTermin = PosunutiTerminuBodAPViewModel.GetZavritPrvniTermin(FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Id).ToList();
                if (prvniTermin.Count > 0)
                {
                    //provede se jenom při první změně termínu
                    UkonceniBodAPDataMapper.UpdatePrvniTermin(Convert.ToInt32(prvniTermin[0].UkonceniBodAPId));
                }
                //nejdřív založím nový termín
                //idZadost = UkonceniBodAPDataMapper.InsertUkonceniAkce(Convert.ToInt32(action_["akceId"]), dateTimePickerNovyTerminUkonceni.Value, richTextBoxNovaPoznamka.Text);
                idZadost = UkonceniBodAPDataMapper.InsertUkonceniBodAP(FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Id, this.dateTimePickerNovyTerminUkonceni.Value, this.richTextBoxNovaPoznamka.Text);

                //snížení počtu možných změn termínů dané akce
                //UkonceniBodAPDataMapper.UpdateAkceZmenaTerminu(Convert.ToInt32(action_["akceId"]), zmenaTerminu_);
                UkonceniBodAPDataMapper.UpdateBodAPZmenaTerminu(FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Id, this.zmenaTerminu_);

                //----- odeslání požadavku -----------------------------------------------------------------------------------------------------
                //var zam = DatumUkonceniViewModel.GetZamestnanec(Convert.ToInt32(action_["comboBoxOdpovednaOsoba1Id"])).ToList();
                var zam = DatumUkonceniViewModel.GetZamestnanec(FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].OdpovednaOsoba1Id).ToList();

                var odpovedny1 = zam[0].Prijmeni + " " + zam[0].Jmeno;

                var odpovedny2 = string.Empty;
                //if (Convert.ToInt32(action_["comboBoxOdpovednaOsoba2Id"]) > 0)
                if (FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].OdpovednaOsoba2Id > 0)
                {
                    //zam = DatumUkonceniViewModel.GetZamestnanec(Convert.ToInt32(action_["comboBoxOdpovednaOsoba2Id"])).ToList();
                    zam = DatumUkonceniViewModel.GetZamestnanec(Convert.ToInt32(FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].OdpovednaOsoba2Id)).ToList();

                    odpovedny2 = zam[0].Prijmeni + " " + zam[0].Jmeno;
                }

                Helper.EmailPosunutíTerminuBodAP(this.zadavatel1Email,
                    this.zadavatel2Email, 
                    FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody],
                    this.cisloAPStr_,
                    this.dateTimePickerNovyTerminUkonceni.MinDate,
                    this.dateTimePickerNovyTerminUkonceni.Value,
                    odpovedny1,
                    odpovedny2,
                    this.richTextBoxNovaPoznamka.Text,
                    this.apId,
                    this.bodAPId,
                    idZadost);

                //odstraní vytvořené objekty z panelu
                this.RemoveControl();

                //init controls
                this.InitControls();

                //znovu vytvoří obsah panelu
                this.VytvoritObsahPanelu();
            }
        }

        private void RemoveControl()
        {
            this.panelTerminy.Controls.Remove(this.labelZamTer);
            this.panelTerminy.Controls.Remove(this.labelZamTerPocet);
            this.panelTerminy.Controls.Remove(this.labelZmenyTer);
            this.panelTerminy.Controls.Remove(this.labelZmenyTerPocet);

            this.labelZamTer.Dispose();
            this.labelZamTerPocet.Dispose();
            this.labelZmenyTer.Dispose();
            this.labelZmenyTerPocet.Dispose();

            for (var i = this.groupBoxTerminy.Count - 1; i >= 0; i--)
            {
                this.groupBoxTerminy[i].Controls.Remove(this.labelTerminyDatum[i]);
                this.groupBoxTerminy[i].Controls.Remove(this.labelStatus[i]);
                this.groupBoxTerminy[i].Controls.Remove(this.richTextBoxPoznamka[i]);
                this.groupBoxTerminy[i].Controls.Remove(this.labelOdpoved[i]);
                this.groupBoxTerminy[i].Controls.Remove(this.richTextBoxOdpoved[i]);

                this.panelTerminy.Controls.Remove(this.groupBoxTerminy[i]);

                this.labelTerminyDatum[i].Dispose();
                this.labelStatus[i].Dispose();
                this.richTextBoxPoznamka[i].Dispose();
                this.labelOdpoved[i].Dispose();
                this.richTextBoxOdpoved[i].Dispose();

                this.groupBoxTerminy[i].Dispose();
            }

            this.Controls.Remove(this.panelTerminy);
            this.panelTerminy.Dispose();
        }

        private void CheckBoxPoslatZadost_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CheckBoxPoslatZadost.Checked == true)
            {
                this.dateTimePickerNovyTerminUkonceni.Enabled = true;
                this.labelNovaPoznamka.Enabled = true;
                this.richTextBoxNovaPoznamka.Enabled = true;
                this.ButtonZadost.Enabled = true;
            }
            else
            {
                this.dateTimePickerNovyTerminUkonceni.Enabled = false;
                this.labelNovaPoznamka.Enabled = false;
                this.richTextBoxNovaPoznamka.Enabled = false;
                this.ButtonZadost.Enabled = false;
            }
        }

        private void ButtonUlozit_MouseClick(object sender, MouseEventArgs e) => this.UlozitPoznamky();

        private void UlozitPoznamky()
        {
            foreach (var rtb in this.richTextBoxPoznamka)
            {
                if (rtb.Enabled == true)
                {
                    foreach (DataRow radek in this.dtUkonceni.Rows)
                    {
                        if (Convert.ToInt32(rtb.Tag) == Convert.ToInt32(radek["UkonceniBodAPId"]))
                        {
                            UkonceniBodAPDataMapper.UpdateUkonceniBodAP(Convert.ToInt32(rtb.Tag), rtb.Text);
                            this.ButtonUlozit.Enabled = false;
                            this.zmenaPoznamky = false;
                        }
                    }
                }
            }
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e) => this.Close();

        private void FormPosunutiTerminuAkce_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.zmenaPoznamky == true)
            {
                DialogResult dialogResult;

                dialogResult = MessageBox.Show("You want to save your changes.", "Notice", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

                if (dialogResult == DialogResult.Yes)
                {
                    //kontrolaEfektivnosti_ == false &&  && (FormMain.VlastnikAP == true || FormMain.VlastnikAkce == true)
                    if (this.opravitTermin_ == true)
                    {
                        //zapíše bod AP do třídy
                        this.UlozitPoznamky();
                        this.zmenaPoznamky = false;
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    ;
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    //nic se dít nebude
                    e.Cancel = true;
                }
            }
        }
    }
}
