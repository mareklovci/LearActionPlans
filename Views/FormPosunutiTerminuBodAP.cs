using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using LearActionPlans.Models;
using LearActionPlans.Repositories;
using LearActionPlans.Utilities;

namespace LearActionPlans.Views
{
    public partial class FormPosunutiTerminuBodAP : Form
    {
        private readonly ActionPlanPointDeadlineRepository actionPlanPointDeadlineRepository;
        private readonly ActionPlanPointRepository actionPlanPointRepository;
        private readonly EmployeeRepository employeeRepository;

        //private readonly DataRow action_;
        private int cisloRadkyDGVBody;
        private string cisloAPStr_;
        private int apId;
        private int bodAPId;

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
        private bool opravitTermin_;

        private bool zmenaPoznamky;
        //povolení nebo zamítnutí změny poslední poznámky
        private bool zamitnutaZmena;
        private byte zmenaTerminu_;

        private List<UkonceniBodAP> ukonceni;
        //1 = první termín
        //2 = první termín uzavřeno,
        //3 = žádost,
        //4 = nový termín potvrzeno,
        //5 = nový termín změna,
        //6 = nový termín zamítnuto
        //když vytvořím nový termín (3), předchozí první termín nastavím na (2)

        private readonly string[] zadost = { "first term", "first term closed", "request", "new deadline confirmed", "new deadline changed", "new deadline reject" };

        public FormPosunutiTerminuBodAP(
            ActionPlanPointDeadlineRepository actionPlanPointDeadlineRepository,
            ActionPlanPointRepository actionPlanPointRepository,
            EmployeeRepository employeeRepository)
        {
            // Repositories
            this.actionPlanPointDeadlineRepository = actionPlanPointDeadlineRepository;
            this.actionPlanPointRepository = actionPlanPointRepository;
            this.employeeRepository = employeeRepository;

            // Initialize
            this.InitializeComponent();
            this.InitControls();
        }

        public void CreateFormPosunutiTerminuBodAP(bool opravitTermin, string cisloAPStr, int cisloRadkyDGV)
        {
            //když je opravitTermin = true, tak AP ještě není uazavřen
            this.opravitTermin_ = opravitTermin;
            this.zmenaPoznamky = false;
            this.zamitnutaZmena = false;
            //action_ = action;
            this.cisloRadkyDGVBody = cisloRadkyDGV;
            this.cisloAPStr_ = cisloAPStr;
            this.apId = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].AkcniPlanId;
            this.bodAPId = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Id;
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

            this.ukonceni = new List<UkonceniBodAP>();
        }

        private void FormPosunutiTerminuAkce_Load(object sender, EventArgs e)
        {
            //kontrolaEfektivnosti_ == false &&  && (FormMain.VlastnikAP == true || FormMain.VlastnikAkce == true)
            if (this.opravitTermin_)
            {
                this.ButtonUlozit.Visible = true;
                this.ButtonZavrit.Text = "Close";
                this.CheckBoxPoslatZadost.CheckedChanged += this.CheckBoxPoslatZadost_CheckedChanged;
            }
            else
            {
                this.ButtonUlozit.Visible = false;
                this.ButtonZavrit.Text = "Close";
                //zneviditelní groupbox pro vytvoření nové žádosti
                this.groupBoxZadost.Visible = false;
            }

            this.VytvoritObsahPanelu();
        }

        private void VytvoritObsahPanelu()
        {
            this.richTextBoxNovaPoznamka.Text = string.Empty;

            //to je proto aby zareagoval handler
            if (this.opravitTermin_)
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
            var someOtherId = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Id;
            var akce = this.actionPlanPointRepository.GetZbyvajiciTerminyBodAPId(someOtherId).ToList();
            byte zamitnutiTerminu = 0;
            byte zmenaTerminu = 0;
            int i;

            //vyhledá datumy ukončení pro dané ID akce
            //jsou setříděny podle UkonceniAkceID
            var someId = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Id;
            this.ukonceni = this.actionPlanPointDeadlineRepository.GetUkonceniBodAPId(someId).ToList();
            this.dtUkonceni = DataTableConverter.ConvertToDataTable(this.ukonceni);

            //to je kvůli nastavení minimálního datumu pro datetimepicker - založení nového termínu
            var minDatumZmeny = this.ukonceni.OrderByDescending(item => item.BodAPId);

            foreach (var a in akce)
            {
                zamitnutiTerminu = a.ZamitnutiTerminu;
                zmenaTerminu = a.ZmenaTerminu;
                this.zmenaTerminu_ = zmenaTerminu;
                //kontrolaEfektivnosti_ == false &&  && (FormMain.VlastnikAP == true || FormMain.VlastnikAkce == true)
                if (this.opravitTermin_)
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
                    Tag = u.BodAPId.ToString(),
                    ForeColor = Color.Black
                });
                this.richTextBoxPoznamka[i].TextChanged += this.RichTextBoxPoznamka_TextChanged;

                this.labelOdpoved.Add(new Label
                {
                    Name = "labelOdpoved" + i + 1,
                    Location = new Point(370, 20),
                    AutoSize = true,
                    Text = "Reply",
                    ForeColor = Color.Black
                });
                this.richTextBoxOdpoved.Add(new RichTextBox
                {
                    Name = "RichTextBoxOdpoved" + i + 1,
                    Location = new Point(370, 45),
                    Size = new Size(340, 80),
                    Text = u.Odpoved,
                    Enabled = false,
                    Tag = u.BodAPId.ToString(),
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
                    this.richTextBoxPoznamka[i].Enabled = m.StavZadosti != 6 && this.opravitTermin_;
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
                this.panelTerminy.Controls.Add(this.groupBoxTerminy[i]);
                this.groupBoxTerminy[i].Controls.Add(this.labelTerminyDatum[i]);
                this.groupBoxTerminy[i].Controls.Add(this.labelStatus[i]);
                this.groupBoxTerminy[i].Controls.Add(this.richTextBoxPoznamka[i]);

                this.groupBoxTerminy[i].Controls.Add(this.labelOdpoved[i]);
                this.groupBoxTerminy[i].Controls.Add(this.richTextBoxOdpoved[i]);

                if (i != 0)
                {
                    continue;
                }

                this.labelOdpoved[i].Visible = false;
                this.richTextBoxOdpoved[i].Visible = false;
            }

            this.ButtonUlozit.Enabled = false;
        }

        private void RichTextBoxPoznamka_TextChanged(object sender, EventArgs e)
        {
            var poznamka = (RichTextBox)sender;

            if (string.IsNullOrEmpty(poznamka.Text))
            {
                return;
            }

            //tady povolím možnost odeslat žádost, zviditelním pole pro odeslání žádosti
            this.ButtonUlozit.Enabled = true;
            this.zmenaPoznamky = true;
        }

        private void ButtonZadost_MouseClick(object sender, MouseEventArgs e)
        {
            int idZadost;

            if (string.IsNullOrEmpty(this.richTextBoxNovaPoznamka.Text))
            {
                MessageBox.Show(@"You must fill in the note field.", @"Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //v případě, že byly provedeny úpravy v poznámce, bude nejdřív uložena
                if (this.ButtonUlozit.Enabled)
                {
                    this.UlozitPoznamky();
                }

                var firstTermId = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Id;
                var prvniTermin = this.actionPlanPointDeadlineRepository.GetZavritPrvniTermin(firstTermId).ToList();
                if (prvniTermin.Count > 0)
                {
                    //provede se jenom při první změně termínu
                    this.actionPlanPointDeadlineRepository.UpdatePrvniTermin(Convert.ToInt32(prvniTermin.FirstOrDefault()!.BodAPId));
                }
                //nejdřív založím nový termín
                idZadost = this.actionPlanPointDeadlineRepository.InsertUkonceniBodAP(FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Id, this.dateTimePickerNovyTerminUkonceni.Value, this.richTextBoxNovaPoznamka.Text);

                //snížení počtu možných změn termínů dané akce
                this.actionPlanPointDeadlineRepository.UpdateBodAPZmenaTerminu(FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Id, this.zmenaTerminu_);

                //----- odeslání požadavku -----------------------------------------------------------------------------------------------------
                var zamId = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].OdpovednaOsoba1Id;
                var zam = this.employeeRepository.GetById(zamId);

                var odpovedny1 = zam.Prijmeni + " " + zam.Jmeno;
                var odpovedny2 = string.Empty;

                if (FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].OdpovednaOsoba2Id > 0)
                {
                    zamId = Convert.ToInt32(FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].OdpovednaOsoba2Id);
                    zam = this.employeeRepository.GetById(zamId);

                    odpovedny2 = zam.Prijmeni + " " + zam.Jmeno;
                }

                using (var message = new MailMessage())
                {
                    //MailMessage message = new MailMessage();
                    var smtp = new SmtpClient
                    {
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("bartos.grammer@seznam.cz", "stepan12"),
                        //Credentials = new NetworkCredential("LSCEurope@grammer.com", "Grammer123"),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Port = 25,
                        Host = "smtp.seznam.cz",
                        //Host = "smtp.grammer.com",
                        EnableSsl = false
                    };
                    //zajistí zobrazení češtiny v emailu
                    message.BodyEncoding = System.Text.Encoding.UTF8;
                    message.From = new MailAddress("bartos.grammer@seznam.cz");
                    message.Subject = @"Request for a new Deadline";
                    message.IsBodyHtml = true;

                    var htmlTableStart = "<table style=\"border-collapse:collapse; text-align:left; font-family:Arial, Helvetica, Sans-serif;\" >";
                    var htmlTableEnd = "</table>";

                    //string htmlHeaderRowStart = "<tr style=\"background-color:#6FA1D2; color:#ffffff;\">";
                    //string htmlHeaderRowEnd = "</tr>";

                    var htmlTrStart = "<tr style=\"color:#555555;\">";
                    var htmlTrEnd = "</tr>";

                    var htmlTdStartFirstColumn = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding:5px; width:300px\">";
                    var htmlTdEndFirstColumn = "</td>";

                    var htmlTdStartSecondColumn = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding:5px; width:450px\">";
                    var htmlTdEndSecondColumn = "</td>";

                    var htmlTdStart = "<td style='border-color:#5c87b2; border-style:solid; border-width:thin; padding:5px;'>";
                    var htmlTdEnd = "</td>";

                    var htmlTdStartPozadi = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding:5px; background-color:#e1e1ff;\">";
                    var htmlTdEndPozadi = "</td>";

                    message.Body += htmlTableStart;
                    message.Body += htmlTrStart;
                    message.Body += htmlTdStartFirstColumn + @"<b>AP</b>" + htmlTdEndFirstColumn;
                    message.Body += htmlTdStartSecondColumn + this.cisloAPStr_ + htmlTdEndSecondColumn;
                    message.Body += htmlTrEnd;
                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b>Point AP</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].CisloBoduAP + htmlTdEnd;
                    message.Body += htmlTrEnd;
                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b> &nbsp; &nbsp; Standard chapter</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].OdkazNaNormu + htmlTdEnd;
                    message.Body += htmlTrEnd;
                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b> &nbsp; &nbsp; Evaluation</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].HodnoceniNeshody + htmlTdEnd;
                    message.Body += htmlTrEnd;
                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b> &nbsp; &nbsp; Description of the problem</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].PopisProblemu + htmlTdEnd;
                    message.Body += htmlTrEnd;

                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b>Why made</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + "" + htmlTdEnd;
                    message.Body += htmlTrEnd;

                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b> &nbsp; &nbsp; Root cause</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].SkutecnaPricinaWM + htmlTdEnd;
                    message.Body += htmlTrEnd;

                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b> &nbsp; &nbsp; Corrective action" + htmlTdEnd;
                    message.Body += htmlTdStart + FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].NapravnaOpatreniWM + htmlTdEnd;
                    message.Body += htmlTrEnd;

                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b>Why shipped</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + "" + htmlTdEnd;
                    message.Body += htmlTrEnd;

                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b> &nbsp; &nbsp; Root cause</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].SkutecnaPricinaWS + htmlTdEnd;
                    message.Body += htmlTrEnd;

                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b> &nbsp; &nbsp; Corrective action</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].NapravnaOpatreniWS + htmlTdEnd;
                    message.Body += htmlTrEnd;

                    message.Body += htmlTrStart;
                    message.Body += htmlTdStartPozadi + @"<b>Last Deadline</b>" + htmlTdEndPozadi;
                    var posledniMoznyTermin = this.dateTimePickerNovyTerminUkonceni.MinDate;
                    posledniMoznyTermin = posledniMoznyTermin.AddDays(-1);
                    message.Body += htmlTdStartPozadi + @"<b>" + posledniMoznyTermin.ToShortDateString() + @"</b>" + htmlTdEndPozadi;
                    message.Body += htmlTrEnd;
                    message.Body += htmlTrStart;
                    message.Body += htmlTdStartPozadi + @"<b>New Deadline</b>" + htmlTdEndPozadi;
                    message.Body += htmlTdStartPozadi + @"<b>" + this.dateTimePickerNovyTerminUkonceni.Value.ToShortDateString() + @"</b>"  + htmlTdEndPozadi;
                    message.Body += htmlTrEnd;
                    message.Body += htmlTrStart;
                    message.Body += htmlTdStartPozadi + @"<b>Note</b>" + htmlTdEndPozadi;
                    //action_["textBoxPoznamka"]
                    message.Body += htmlTdStartPozadi + this.richTextBoxNovaPoznamka.Text + htmlTdEndPozadi;
                    message.Body += htmlTrEnd;

                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b>Responsible #1</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + odpovedny1 + htmlTdEnd;
                    message.Body += htmlTrEnd;
                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b>Responsible #2</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + odpovedny2 + htmlTdEnd;
                    message.Body += htmlTrEnd;
                    message.Body += htmlTableEnd;
                    //message.Body += "<a href=javascript:runProgram()>Odkaz</a>";
                    //Process the request - vyřídit žádost

                    //message.Body += string.Format(@"<p style='font-family:Arial, Helvetica, Sans-serif; font-size:150 %; '><a href='LearAPConfirmation:?{0}&{1}&{2}&{3}&{4}&{5}' type='application/octet-stream'>Process the request</a></p>", cisloAPStr_, apId, bodAPId, Convert.ToInt32(action_["akceId"]), idZadost, FormMain.VlastnikIdAkce);
                    //message.Body += string.Format(@"<p style='font-family:Arial, Helvetica, Sans-serif; font-size:150 %; '><a href='LearAPConfirmation:?{0}&{1}&{2}&{3}&{4}&{5}' type='application/octet-stream'>Process the request</a></p>", cisloAPStr_, apId, bodAPId, Convert.ToInt32(action_["akceId"]), idZadost, Convert.ToInt32(action_["comboBoxOdpovednaOsoba1Id"]));
                    //odebrán čtvrtý parametr
                    message.Body += string.Format(@"<p style='font-family:Arial, Helvetica, Sans-serif; font-size:150 %; '><a href='LearAPConfirmation:?{0}&{1}&{2}&{3}&{4}' type='application/octet-stream'>Process the request</a></p>", this.cisloAPStr_, this.apId, this.bodAPId, idZadost, FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].OdpovednaOsoba1Id);

                    message.To.Add(new MailAddress("bartos.grammer@seznam.cz"));

                    // tady se uloží email do tabulky EmailOdeslat a zavolá se aplikace pro odeslání emailu LaerEmailOdeslat
                    Helper.OdeslatEmail(smtp, message);
                }
                //----- odeslání požadavku -----------------------------------------------------------------------------------------------------

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
            if (this.CheckBoxPoslatZadost.Checked)
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

        private void ButtonSave_MouseClick(object sender, MouseEventArgs e) => this.UlozitPoznamky();

        private void UlozitPoznamky()
        {
            foreach (var rtb in this.richTextBoxPoznamka)
            {
                if (!rtb.Enabled)
                {
                    continue;
                }

                foreach (DataRow radek in this.dtUkonceni.Rows)
                {
                    if (Convert.ToInt32(rtb.Tag) != Convert.ToInt32(radek["UkonceniBodAPId"]))
                    {
                        continue;
                    }

                    this.actionPlanPointDeadlineRepository.UpdateUkonceniBodAP(Convert.ToInt32(rtb.Tag), rtb.Text);
                    this.ButtonUlozit.Enabled = false;
                    this.zmenaPoznamky = false;
                }
            }
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e) => this.Close();

        private void FormPosunutiTerminuAkce_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.zmenaPoznamky)
            {
                return;
            }

            var dialogResult = MessageBox.Show(@"You want to save your changes.", @"Notice",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

            switch (dialogResult)
            {
                case DialogResult.Yes when !this.opravitTermin_:
                    return;
                //zapíše bod AP do třídy
                case DialogResult.Yes:
                    this.UlozitPoznamky();
                    this.zmenaPoznamky = false;
                    break;
                case DialogResult.Cancel:
                    //nic se dít nebude
                    e.Cancel = true;
                    break;
                case DialogResult.No:
                case DialogResult.None:
                case DialogResult.OK:
                case DialogResult.Abort:
                case DialogResult.Retry:
                case DialogResult.Ignore:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
