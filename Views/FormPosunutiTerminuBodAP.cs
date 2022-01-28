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
        public FormPosunutiTerminuBodAP(bool opravitTermin, string cisloAPStr, int cisloRadkyDGV)
        {
            //bool kontrolaEfektivnosti,
            InitializeComponent();
            //kontrolaEfektivnosti_ = kontrolaEfektivnosti;

            //vlastnikAP_ = vlastnikAP;
            //vlastnikAkce_ = vlastnikAkce;
            //vlastnikAkceId_ = vlastnikAkceId;
            
            //když je opravitTermin = true, tak AP ještě není uazavřen
            opravitTermin_ = opravitTermin;
            zmenaPoznamky = false;
            zamitnutaZmena = false;
            //action_ = action;
            cisloRadkyDGVBody = cisloRadkyDGV;
            cisloAPStr_ = cisloAPStr;
            apId = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].AkcniPlanId;
            bodAPId = FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].Id;
            InitControls();
        }

        private void InitControls()
        {
            panelTerminy = new Panel();
            groupBoxTerminy = new List<GroupBox>();
            labelTerminyDatum = new List<Label>();
            labelStatus = new List<Label>();
            richTextBoxPoznamka = new List<RichTextBox>();

            labelOdpoved = new List<Label>();
            richTextBoxOdpoved = new List<RichTextBox>();

            ukonceni = new List<PosunutiTerminuBodAPViewModel>();
        }

        private void FormPosunutiTerminuAkce_Load(object sender, EventArgs e)
        {
            //kontrolaEfektivnosti_ == false &&  && (FormMain.VlastnikAP == true || FormMain.VlastnikAkce == true)
            if (opravitTermin_ == true)
            {
                ButtonUlozit.Visible = true;
                ButtonZavrit.Text = "Close";
                CheckBoxPoslatZadost.CheckedChanged += new EventHandler(CheckBoxPoslatZadost_CheckedChanged);
            }
            else
            {
                ButtonUlozit.Visible = false;
                ButtonZavrit.Text = "Close";
                //zneviditelní groupbox pro vytvoření nové žádosti
                groupBoxZadost.Visible = false;
            }

            //if (FormMain.VlastnikAP == true)
            //    ButtonZadost.Text = "Save a new Deadline";
            //if (FormMain.VlastnikAkce == true)
            //    ButtonZadost.Text = "Request for a new Deadline";

            VytvoritObsahPanelu();
        }

        private void VytvoritObsahPanelu()
        {
            richTextBoxNovaPoznamka.Text = string.Empty;

            //to je proto aby zareagoval handler
            //kontrolaEfektivnosti_ == false &&  && (FormMain.VlastnikAP == true || FormMain.VlastnikAkce == true)
            if (opravitTermin_ == true)
            {
                CheckBoxPoslatZadost.Checked = true;
                CheckBoxPoslatZadost.Checked = false;
            }

            panelTerminy.Size = new Size(760, 600);
            panelTerminy.Name = "panelPrehledTerminu";
            panelTerminy.Location = new Point(10, 10);
            panelTerminy.BorderStyle = BorderStyle.FixedSingle;
            panelTerminy.BackColor = SystemColors.Control;
            panelTerminy.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            panelTerminy.AutoScroll = false;
            panelTerminy.HorizontalScroll.Enabled = false;
            panelTerminy.HorizontalScroll.Visible = false;
            panelTerminy.HorizontalScroll.Maximum = 0;
            panelTerminy.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelTerminy.AutoScroll = true;

            Controls.Add(panelTerminy);

            //zbývající počet počet zamítnutí a změn
            
            //var akce = PosunutiTerminuBodAPViewModel.GetZbyvajiciTerminy(Convert.ToInt32(action_["akceId"])).ToList();
            var akce = PosunutiTerminuBodAPViewModel.GetZbyvajiciTerminy(FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].Id).ToList();
            byte zamitnutiTerminu = 0;
            byte zmenaTerminu = 0;
            int i;

            //vyhledá datumy ukončení pro dané ID akce
            //jsou setříděny podle UkonceniAkceID
            //ukonceni = PosunutiTerminuBodAPViewModel.GetUkonceniAkce(Convert.ToInt32(action_["akceId"])).ToList();
            ukonceni = PosunutiTerminuBodAPViewModel.GetUkonceniBodAP(FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].Id).ToList();
            dtUkonceni = DataTableConverter.ConvertToDataTable(ukonceni);
            //to je kvůli nastavení minimálního datumu pro datetimepicker - založení nového termínu
            var minDatumZmeny = ukonceni.OrderByDescending(item => item.UkonceniBodAPId);

            foreach (var a in akce)
            {
                zamitnutiTerminu = a.ZamitnutiTerminu;
                zmenaTerminu = a.ZmenaTerminu;
                zmenaTerminu_ = zmenaTerminu;
                //kontrolaEfektivnosti_ == false &&  && (FormMain.VlastnikAP == true || FormMain.VlastnikAkce == true)
                if (opravitTermin_ == true)
                {
                    //FormMain.VlastnikAP == true && 
                    if (zmenaTerminu == 0)
                    {
                        groupBoxZadost.Visible = false;
                    }

                    //FormMain.VlastnikAkce == true && 
                    if (zamitnutiTerminu == 0 || zmenaTerminu == 0)
                    {
                        //již nelze dále prodlužovat termíny
                        //když bude žádost 3x zamítnuta, nemůže již vlastník akce žádat o prodloužení
                        groupBoxZadost.Visible = false;
                        //protože už byly možné termíny majitele akce vyčerpány, bude zablována editace poznámek
                        zamitnutaZmena = true;
                    }
                    else
                    {
                        DateTime minDatum = DateTime.Now;
                        i = 0;
                        foreach (var m in minDatumZmeny)
                        {
                            if (i == 0)
                            {
                                //nastavení toho, že můžu poslat žádost nebo ne
                                if (m.StavZadosti == 3)
                                {
                                    groupBoxZadost.Visible = false;
                                }
                                else
                                    groupBoxZadost.Visible = true;
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
                        dateTimePickerNovyTerminUkonceni.Value = minDatum;
                        dateTimePickerNovyTerminUkonceni.MinDate = minDatum;
                        if (minDatum < DateTime.Now)
                            dateTimePickerNovyTerminUkonceni.Value = DateTime.Now;
                    }
                }
            }

            //počet zamítnutí
            labelZamTer = new Label
            {
                Name = "labelZamitnuti",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel),
                Text = "Remaining number of rejections:"
            };
            panelTerminy.Controls.Add(labelZamTer);
            labelZamTerPocet = new Label
            {
                Name = "labelZamitnutiPocet",
                Location = new Point(250, 10),
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel),
                Text = "3"
            };
            labelZamTerPocet.Text = Convert.ToString(zamitnutiTerminu);
            panelTerminy.Controls.Add(labelZamTerPocet);

            //počet změn
            labelZmenyTer = new Label
            {
                Name = "labeZmeny",
                Location = new Point(10, 35),
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel),
                Text = "Remaining number of changes:"
            };
            panelTerminy.Controls.Add(labelZmenyTer);
            labelZmenyTerPocet = new Label
            {
                Name = "labelZmenyPocet",
                Location = new Point(250, 35),
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel),
                Text = "5"
            };
            labelZmenyTerPocet.Text = Convert.ToString(zmenaTerminu);
            panelTerminy.Controls.Add(labelZmenyTerPocet);

            i = 0;
            foreach (var u in ukonceni)
            {
                groupBoxTerminy.Add(new GroupBox()
                {
                    Name = "groupBoxTerminy" + i + 1,
                    Location = new Point(10, 60 + (i * 150)),
                    Size = new Size(720, 140),
                    Text = (i + 1).ToString() + ". term",
                });
                labelTerminyDatum.Add(new Label()
                {
                    Name = "labelTerminyDatum" + i + 1,
                    Location = new Point(10, 20),
                    AutoSize = true,
                    Text = u.DatumUkonceni.ToShortDateString(),
                    ForeColor = Color.Black
                });
                int delkaRetezce = 0;
                using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1)))
                {
                    SizeF size = graphics.MeasureString(u.DatumUkonceni.ToShortDateString(), new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Pixel));
                    delkaRetezce = Convert.ToInt32(size.Width);
                }
                string stavTerminu;
                Color barva = Color.Black;
                switch (u.StavZadosti)
                {
                    case 1:
                        stavTerminu = zadost[0];
                        barva = Color.Black;
                        break;
                    case 2:
                        stavTerminu = zadost[1];
                        barva = Color.Black;
                        break;
                    case 3:
                        stavTerminu = zadost[2];
                        barva = Color.Black;
                        break;
                    case 4:
                        stavTerminu = zadost[3];
                        barva = Color.Green;
                        break;
                    case 5:
                        stavTerminu = zadost[4];
                        barva = Color.Blue;
                        break;
                    case 6:
                        stavTerminu = zadost[5];
                        barva = Color.Red;
                        break;
                    default:
                        stavTerminu = string.Empty;
                        break;
                }
                stavTerminu = "(" + stavTerminu + ")";
                labelStatus.Add(new Label()
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
                richTextBoxPoznamka.Add(new RichTextBox()
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
                richTextBoxPoznamka[i].TextChanged += new EventHandler(RichTextBoxPoznamka_TextChanged);

                labelOdpoved.Add(new Label()
                {
                    Name = "labelOdpoved" + i + 1,
                    Location = new Point(370, 20),
                    AutoSize = true,
                    Text = "Reply",
                    ForeColor = Color.Black
                });
                richTextBoxOdpoved.Add(new RichTextBox()
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
            i = richTextBoxPoznamka.Count - 1;
            foreach (var m in minDatumZmeny)
            {
                //poznámky projíždím od konce, stejně kako datumy
                if (i == richTextBoxPoznamka.Count - 1)
                {
                    if (m.StavZadosti == 6)
                    {
                        //zamítnutí termínu
                        richTextBoxPoznamka[i].Enabled = false;
                    }
                    else
                    {
                        //všechny ostatní varianty
                        //kontrolaEfektivnosti_ == true ||  && (FormMain.VlastnikAP == true || FormMain.VlastnikAkce == true)
                        if (opravitTermin_ == false)
                        {
                            //AP nebo akce je uzavřena
                            richTextBoxPoznamka[i].Enabled = false;
                        }
                        else
                        {
                            //když je počet zbývajících zamítnutí nebo 
                            richTextBoxPoznamka[i].Enabled = true;
                            //if (zamitnutaZmena == false)
                            //    richTextBoxPoznamka[i].Enabled = true;
                            //else
                            //    richTextBoxPoznamka[i].Enabled = false;
                        }
                    }
                }
                else
                {
                    richTextBoxPoznamka[i].Enabled = false;
                }
                i--;
                break;
            }

            for (i = 0; i < groupBoxTerminy.Count; i++)
            {
                //GroupBox itemGrpBox = groupBoxTerminy[i];
                panelTerminy.Controls.Add(groupBoxTerminy[i]);
                //Label itemLbl = labelTerminyDatum[i];
                groupBoxTerminy[i].Controls.Add(labelTerminyDatum[i]);
                groupBoxTerminy[i].Controls.Add(labelStatus[i]);
                groupBoxTerminy[i].Controls.Add(richTextBoxPoznamka[i]);

                groupBoxTerminy[i].Controls.Add(labelOdpoved[i]);
                groupBoxTerminy[i].Controls.Add(richTextBoxOdpoved[i]);

                if (i == 0)
                {
                    labelOdpoved[i].Visible = false;
                    richTextBoxOdpoved[i].Visible = false;
                }
            }

            ButtonUlozit.Enabled = false;
        }

        private void RichTextBoxPoznamka_TextChanged(object sender, EventArgs e)
        {
            RichTextBox poznamka = (RichTextBox)sender;

            if (!string.IsNullOrEmpty(poznamka.Text))
            {
                //tady povolím možnost odeslat žádost, zviditelním pole pro odeslání žádosti
                ButtonUlozit.Enabled = true;
                zmenaPoznamky = true;
            }
            else
            { 
            }
        }

        private void ButtonZadost_MouseClick(object sender, MouseEventArgs e)
        {
            int idZadost;

            if (string.IsNullOrEmpty(richTextBoxNovaPoznamka.Text))
            {
                MessageBox.Show("You must fill in the note field.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //v případě, že byly provedeny úpravy v poznámce, bude nejdřív uložena
                if (ButtonUlozit.Enabled == true)
                    UlozitPoznamky();
                
                //var prvniTermin = PosunutiTerminuBodAPViewModel.GetZavritPrvniTermin(Convert.ToInt32(action_["akceId"])).ToList();
                var prvniTermin = PosunutiTerminuBodAPViewModel.GetZavritPrvniTermin(FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].Id).ToList();
                if (prvniTermin.Count > 0)
                {
                    //provede se jenom při první změně termínu
                    UkonceniBodAPDataMapper.UpdatePrvniTermin(Convert.ToInt32(prvniTermin[0].UkonceniBodAPId));
                }
                //nejdřív založím nový termín
                //idZadost = UkonceniBodAPDataMapper.InsertUkonceniAkce(Convert.ToInt32(action_["akceId"]), dateTimePickerNovyTerminUkonceni.Value, richTextBoxNovaPoznamka.Text);
                idZadost = UkonceniBodAPDataMapper.InsertUkonceniBodAP(FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].Id, dateTimePickerNovyTerminUkonceni.Value, richTextBoxNovaPoznamka.Text);

                //snížení počtu možných změn termínů dané akce
                //UkonceniBodAPDataMapper.UpdateAkceZmenaTerminu(Convert.ToInt32(action_["akceId"]), zmenaTerminu_);
                UkonceniBodAPDataMapper.UpdateBodAPZmenaTerminu(FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].Id, zmenaTerminu_);

                //if (FormMain.VlastnikAkce == true)
                //{
                //}

                //----- odeslání požadavku -----------------------------------------------------------------------------------------------------
                //var zam = DatumUkonceniViewModel.GetZamestnanec(Convert.ToInt32(action_["comboBoxOdpovednaOsoba1Id"])).ToList();
                var zam = DatumUkonceniViewModel.GetZamestnanec(FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].OdpovednaOsoba1Id).ToList();

                string odpovedny1 = zam[0].Prijmeni + " " + zam[0].Jmeno;

                string odpovedny2 = string.Empty;
                //if (Convert.ToInt32(action_["comboBoxOdpovednaOsoba2Id"]) > 0)
                if (FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].OdpovednaOsoba2Id > 0)
                {
                    //zam = DatumUkonceniViewModel.GetZamestnanec(Convert.ToInt32(action_["comboBoxOdpovednaOsoba2Id"])).ToList();
                    zam = DatumUkonceniViewModel.GetZamestnanec(Convert.ToInt32(FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].OdpovednaOsoba2Id)).ToList();

                    odpovedny2 = zam[0].Prijmeni + " " + zam[0].Jmeno;
                }

                using (var message = new MailMessage())
                {
                    //MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient
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
                    message.Subject = string.Format(@"Request for a new Deadline");
                    message.IsBodyHtml = true;

                    string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:left; font-family:Arial, Helvetica, Sans-serif;\" >";
                    string htmlTableEnd = "</table>";

                    //string htmlHeaderRowStart = "<tr style=\"background-color:#6FA1D2; color:#ffffff;\">";
                    //string htmlHeaderRowEnd = "</tr>";

                    string htmlTrStart = "<tr style=\"color:#555555;\">";
                    string htmlTrEnd = "</tr>";

                    string htmlTdStartFirstColumn = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding:5px; width:300px\">";
                    string htmlTdEndFirstColumn = "</td>";

                    string htmlTdStartSecondColumn = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding:5px; width:450px\">";
                    string htmlTdEndSecondColumn = "</td>";

                    string htmlTdStart = "<td style='border-color:#5c87b2; border-style:solid; border-width:thin; padding:5px;'>";
                    string htmlTdEnd = "</td>";

                    string htmlTdStartPozadi = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding:5px; background-color:#e1e1ff;\">";
                    string htmlTdEndPozadi = "</td>";

                    message.Body += htmlTableStart;
                    message.Body += htmlTrStart;
                    message.Body += htmlTdStartFirstColumn + @"<b>AP</b>" + htmlTdEndFirstColumn;
                    message.Body += htmlTdStartSecondColumn + cisloAPStr_ + htmlTdEndSecondColumn;
                    message.Body += htmlTrEnd;
                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b>Point AP</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].CisloBoduAP.ToString() + htmlTdEnd;
                    message.Body += htmlTrEnd;
                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b> &nbsp; &nbsp; Standard chapter</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].OdkazNaNormu + htmlTdEnd;
                    message.Body += htmlTrEnd;
                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b> &nbsp; &nbsp; Evaluation</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].HodnoceniNeshody + htmlTdEnd;
                    message.Body += htmlTrEnd;
                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b> &nbsp; &nbsp; Description of the problem</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].PopisProblemu + htmlTdEnd;
                    message.Body += htmlTrEnd;

                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b>Why made</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + "" + htmlTdEnd;
                    message.Body += htmlTrEnd;

                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b> &nbsp; &nbsp; Root cause</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].SkutecnaPricinaWM + htmlTdEnd;
                    message.Body += htmlTrEnd;

                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b> &nbsp; &nbsp; Corrective action" + htmlTdEnd;
                    message.Body += htmlTdStart + FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].NapravnaOpatreniWM + htmlTdEnd;
                    message.Body += htmlTrEnd;

                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b>Why shipped</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + "" + htmlTdEnd;
                    message.Body += htmlTrEnd;

                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b> &nbsp; &nbsp; Root cause</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].SkutecnaPricinaWS + htmlTdEnd;
                    message.Body += htmlTrEnd;

                    message.Body += htmlTrStart;
                    message.Body += htmlTdStart + @"<b> &nbsp; &nbsp; Corrective action</b>" + htmlTdEnd;
                    message.Body += htmlTdStart + FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].NapravnaOpatreniWS + htmlTdEnd;
                    message.Body += htmlTrEnd;

                    message.Body += htmlTrStart;
                    message.Body += htmlTdStartPozadi + @"<b>Last Deadline</b>" + htmlTdEndPozadi;
                    DateTime posledniMoznyTermin = dateTimePickerNovyTerminUkonceni.MinDate;
                    posledniMoznyTermin = posledniMoznyTermin.AddDays(-1);
                    message.Body += htmlTdStartPozadi + posledniMoznyTermin.ToShortDateString() + htmlTdEndPozadi;
                    message.Body += htmlTrEnd;
                    message.Body += htmlTrStart;
                    message.Body += htmlTdStartPozadi + @"<b>New Deadline</b>" + htmlTdEndPozadi;
                    message.Body += htmlTdStartPozadi + dateTimePickerNovyTerminUkonceni.Value.ToShortDateString() + htmlTdEndPozadi;
                    message.Body += htmlTrEnd;
                    message.Body += htmlTrStart;
                    message.Body += htmlTdStartPozadi + @"<b>Note</b>" + htmlTdEndPozadi;
                    //action_["textBoxPoznamka"]
                    message.Body += htmlTdStartPozadi + richTextBoxNovaPoznamka.Text + htmlTdEndPozadi;
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
                    message.Body += string.Format(@"<p style='font-family:Arial, Helvetica, Sans-serif; font-size:150 %; '><a href='LearAPConfirmation:?{0}&{1}&{2}&{3}&{4}' type='application/octet-stream'>Process the request</a></p>", cisloAPStr_, apId, bodAPId, idZadost, FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].OdpovednaOsoba1Id);

                    message.To.Add(new MailAddress("bartos.grammer@seznam.cz"));

                    Helper.OdeslatEmail(smtp, message);
                }
                //----- odeslání požadavku -----------------------------------------------------------------------------------------------------

                //odstraní vytvořené objekty z panelu
                RemoveControl();

                //init controls
                InitControls();

                //znovu vytvoří obsah panelu
                VytvoritObsahPanelu();
            }
        }

        private void RemoveControl()
        {
            panelTerminy.Controls.Remove(labelZamTer);
            panelTerminy.Controls.Remove(labelZamTerPocet);
            panelTerminy.Controls.Remove(labelZmenyTer);
            panelTerminy.Controls.Remove(labelZmenyTerPocet);

            labelZamTer.Dispose();
            labelZamTerPocet.Dispose();
            labelZmenyTer.Dispose();
            labelZmenyTerPocet.Dispose();

            for (int i = groupBoxTerminy.Count - 1; i >= 0; i--)
            {

                groupBoxTerminy[i].Controls.Remove(labelTerminyDatum[i]);
                groupBoxTerminy[i].Controls.Remove(labelStatus[i]);
                groupBoxTerminy[i].Controls.Remove(richTextBoxPoznamka[i]);
                groupBoxTerminy[i].Controls.Remove(labelOdpoved[i]);
                groupBoxTerminy[i].Controls.Remove(richTextBoxOdpoved[i]);

                panelTerminy.Controls.Remove(groupBoxTerminy[i]);

                labelTerminyDatum[i].Dispose();
                labelStatus[i].Dispose();
                richTextBoxPoznamka[i].Dispose();
                labelOdpoved[i].Dispose();
                richTextBoxOdpoved[i].Dispose();

                groupBoxTerminy[i].Dispose();
            }

            Controls.Remove(panelTerminy);
            panelTerminy.Dispose();
        }

        private void CheckBoxPoslatZadost_CheckedChanged(object sender, System.EventArgs e)
        {
            if (CheckBoxPoslatZadost.Checked == true)
            {
                dateTimePickerNovyTerminUkonceni.Enabled = true;
                labelNovaPoznamka.Enabled = true;
                richTextBoxNovaPoznamka.Enabled = true;
                ButtonZadost.Enabled = true;
            }
            else
            {
                dateTimePickerNovyTerminUkonceni.Enabled = false;
                labelNovaPoznamka.Enabled = false;
                richTextBoxNovaPoznamka.Enabled = false;
                ButtonZadost.Enabled = false;
            }
        }

        private void ButtonUlozit_MouseClick(object sender, MouseEventArgs e)
        {
            UlozitPoznamky();
        }

        private void UlozitPoznamky()
        {
            foreach (var rtb in richTextBoxPoznamka)
            {
                if (rtb.Enabled == true)
                {
                    foreach (DataRow radek in dtUkonceni.Rows)
                    {
                        if (Convert.ToInt32(rtb.Tag) == Convert.ToInt32(radek["ukonceniAkceId"]))
                        {
                            UkonceniBodAPDataMapper.UpdateUkonceniBodAP(Convert.ToInt32(rtb.Tag), rtb.Text);
                            ButtonUlozit.Enabled = false;
                            zmenaPoznamky = false;
                        }
                    }
                }
            }
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void FormPosunutiTerminuAkce_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (zmenaPoznamky == true)
            {
                DialogResult dialogResult;

                dialogResult = MessageBox.Show("You want to save your changes.", "Notice", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

                if (dialogResult == DialogResult.Yes)
                {
                    //kontrolaEfektivnosti_ == false &&  && (FormMain.VlastnikAP == true || FormMain.VlastnikAkce == true)
                    if (opravitTermin_ == true)
                    {
                        //zapíše bod AP do třídy
                        UlozitPoznamky();
                        zmenaPoznamky = false;
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