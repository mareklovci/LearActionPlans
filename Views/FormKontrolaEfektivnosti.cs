using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LearActionPlans.Repositories;

namespace LearActionPlans.Views
{
    public partial class FormKontrolaEfektivnosti : Form
    {
        private readonly EffectivityControlRepository effectivityControlRepository;
        private readonly ActionPlanPointRepository actionPlanPointRepository;
        public DateTime? ReturnValuePuvodniDatum { get; set; }
        public DateTime? ReturnValueDatum { get; set; }
        public string ReturnValuePoznamka { get; set; }
        public bool ReturnValuePotvrdit { get; set; }

        private int bodApId;
        private bool kontrolaEfektivnosti;
        private bool apUzavren_;
        private bool novyBodAp;
        private bool deadLineZadan_;

        private DateTime? datum;

        private List<GroupBox> groupBoxPuvodniTerminy;
        private List<Label> labelOdstraneno;
        private List<Label> labelOdstranenoDatum;
        private List<Label> labelPuvodniTerminy;
        private List<Label> labelPuvodniTerminyDatum;
        private List<RichTextBox> richTextBoxPoznamka;

        public FormKontrolaEfektivnosti(
            EffectivityControlRepository effectivityControlRepository,
            ActionPlanPointRepository actionPlanPointRepository)
        {
            // Repositories
            this.effectivityControlRepository = effectivityControlRepository;
            this.actionPlanPointRepository = actionPlanPointRepository;

            // Initialize
            this.InitializeComponent();
        }

        private void InitFormLoad()
        {
            //ButtonOk.Visible = true;
            this.dateTimePickerKontrolaEfektivnosti.Visible = true;
            this.labelDatumEfektivnosti.Visible = false;

            switch (this.kontrolaEfektivnosti)
            {
                case true:
                    //datum efektivnosti je zadáno
                    this.labelDatumEfektivnosti.Text = Convert.ToDateTime(this.datum).ToShortDateString();
                    this.labelDatumEfektivnosti.Visible = true;
                    break;
                case false:
                    this.labelDatumEfektivnosti.Visible = false;
                    break;
            }

            //efektivita není zadána a AP není uzavřen, deadLine bodu AP je nastaven
            //nemohu jenom odstranit
            if (this.apUzavren_ == false)
            {
                if (this.deadLineZadan_ && this.kontrolaEfektivnosti == false)
                {
                    this.groupBoxNovaKontrolaEfektivnosti.Visible = true;
                    this.groupBoxOdstranitEfektivitu.Visible = false;
                }

                //efektivita je zadána a bod AP má deadLine
                //mohu jenom odstranit
                if (this.kontrolaEfektivnosti)
                {
                    this.groupBoxNovaKontrolaEfektivnosti.Visible = false;
                    this.groupBoxOdstranitEfektivitu.Location = new Point(20, 80);
                    this.groupBoxOdstranitEfektivitu.Visible = true;
                }
            }

            //při uzavření AP nebo u bodu AP není zadán deadLine už nemohu editovat
            if (this.apUzavren_ || this.deadLineZadan_ == false)
            {
                this.groupBoxNovaKontrolaEfektivnosti.Visible = false;
                this.groupBoxOdstranitEfektivitu.Visible = false;
            }

            this.panelPuvodniDatumy.HorizontalScroll.Enabled = false;
            this.panelPuvodniDatumy.HorizontalScroll.Visible = false;
            this.panelPuvodniDatumy.HorizontalScroll.Maximum = 0;

            //zobrazení původních termínů kontrol efektivity
            var efektivita =
                this.effectivityControlRepository.GetKontrolaEfektivnostiBodAPId(Convert.ToInt32(this.bodApId));

            var i = 0;
            foreach (var ef in efektivita)
            {
                this.groupBoxPuvodniTerminy.Add(new GroupBox
                {
                    Name = "groupBoxPuvodniTerminy" + i + 1,
                    Location = new Point(10, 10 + (i * 130)),
                    Size = new Size(420, 130),
                    Text = $@"{i + 1}. term"
                });
                this.labelPuvodniTerminy.Add(new Label
                {
                    Name = "labelPuvodniTerminy" + i + 1,
                    Location = new Point(10, 20),
                    AutoSize = true,
                    Text = @"The original date",
                    ForeColor = Color.Black
                });
                this.labelPuvodniTerminyDatum.Add(new Label
                {
                    Name = "labelPuvodniTerminyDatum" + i + 1,
                    Location = new Point(10, 40),
                    AutoSize = true,
                    Text = ef.PuvodniDatum.ToShortDateString(),
                    ForeColor = Color.Black
                });
                this.labelOdstraneno.Add(new Label
                {
                    Name = "labelOdstraneno" + i + 1,
                    Location = new Point(150, 20),
                    AutoSize = true,
                    Text = @"Removed",
                    ForeColor = Color.Black
                });
                this.labelOdstranenoDatum.Add(new Label()
                {
                    Name = "labelOdstranenoDatum" + i + 1,
                    Location = new Point(150, 40),
                    AutoSize = true,
                    Text = ef.OdstranitDatum.ToShortDateString(),
                    ForeColor = Color.Black
                });
                this.richTextBoxPoznamka.Add(new RichTextBox()
                {
                    Name = "richTextBoxPoznamka" + i + 1,
                    Location = new Point(10, 60),
                    Size = new Size(400, 60),
                    Text = ef.Poznamka,
                    Enabled = false,
                    ForeColor = Color.Black
                });

                i++;
            }

            for (i = 0; i < this.groupBoxPuvodniTerminy.Count; i++)
            {
                var itemGrpBox = this.groupBoxPuvodniTerminy[i];
                this.panelPuvodniDatumy.Controls.Add(this.groupBoxPuvodniTerminy[i]);
                this.groupBoxPuvodniTerminy[i].Controls.Add(this.labelPuvodniTerminy[i]);
                this.groupBoxPuvodniTerminy[i].Controls.Add(this.labelPuvodniTerminyDatum[i]);

                this.groupBoxPuvodniTerminy[i].Controls.Add(this.labelOdstraneno[i]);
                this.groupBoxPuvodniTerminy[i].Controls.Add(this.labelOdstranenoDatum[i]);

                this.groupBoxPuvodniTerminy[i].Controls.Add(this.richTextBoxPoznamka[i]);
            }

            this.ReturnValuePotvrdit = false;
        }

        public void CreateFormKontrolaEfektivnosti(bool novyBodAP, bool apUzavren, bool deadLineZadan, int bodAPId,
            bool kontrolaEfektivnosti, DateTime? datum)
        {
            var podminkaDatum = datum == null;

            this.novyBodAp = novyBodAP;
            this.bodApId = bodAPId;
            this.kontrolaEfektivnosti = kontrolaEfektivnosti;
            this.apUzavren_ = apUzavren;
            this.deadLineZadan_ = deadLineZadan;
            this.datum = datum;

            this.dateTimePickerKontrolaEfektivnosti.Value = podminkaDatum ? DateTime.Now : Convert.ToDateTime(datum);

            this.groupBoxPuvodniTerminy = new List<GroupBox>();
            this.labelOdstraneno = new List<Label>();
            this.labelOdstranenoDatum = new List<Label>();
            this.labelPuvodniTerminy = new List<Label>();
            this.labelPuvodniTerminyDatum = new List<Label>();
            this.richTextBoxPoznamka = new List<RichTextBox>();
        }

        private void FormKontrolaEfektivnosti_Load(object sender, EventArgs e)
        {
            this.InitFormLoad();
            this.ButtonNoveDatum.Text = this.novyBodAp ? "OK" : "Save";
        }

        private void ButtonNoveDatum_MouseClick(object sender, MouseEventArgs e)
        {
            var dialogResult = MessageBox.Show(@"You really want to create effectiveness.", @"Notice",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.Yes)
            {
                this.dateTimePickerKontrolaEfektivnosti.Visible = true;
                this.ButtonNoveDatum.Enabled = false;

                this.ReturnValueDatum = this.dateTimePickerKontrolaEfektivnosti.Value;
                this.ReturnValuePuvodniDatum = null;
                this.ReturnValuePoznamka = string.Empty;

                if (this.novyBodAp == false)
                {
                    this.actionPlanPointRepository.UpdateKontrolaEfektivity(this.bodApId,
                        Convert.ToDateTime(this.dateTimePickerKontrolaEfektivnosti.Value));
                }

                //znovu načíst hodnoty
                //InitFormLoad();
                this.ReturnValuePotvrdit = true;
                //DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void ButtonOdstranitDatum_MouseClick(object sender, MouseEventArgs e)
        {
            var dialogResult = MessageBox.Show(@"You really want to cancel effectiveness.", @"Notice",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dialogResult != DialogResult.Yes)
            {
                return;
            }

            if (this.richTextBoxPoznamkaOdstranitDatum.Text == string.Empty)
            {
                MessageBox.Show(@"You must fill out a note.", @"Notice", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                this.labelDatumEfektivnosti.Text = string.Empty;
                //ButtonOk.Enabled = true;

                //ButtonNoveDatum.Visible = true;
                this.ButtonOdstranitDatum.Enabled = false;
                this.richTextBoxPoznamkaOdstranitDatum.Enabled = false;

                this.ReturnValueDatum = null;
                this.ReturnValuePuvodniDatum = this.dateTimePickerKontrolaEfektivnosti.Value;
                this.ReturnValuePoznamka = this.richTextBoxPoznamkaOdstranitDatum.Text;

                this.actionPlanPointRepository.RemoveKontrolaEfektivity(this.bodApId, this.datum,
                    this.richTextBoxPoznamkaOdstranitDatum.Text);

                //znovu načíst hodnoty
                //InitFormLoad();
                this.ReturnValuePotvrdit = true;
                this.Close();
            }
        }

        private void RemoveControls()
        {
            for (var i = this.groupBoxPuvodniTerminy.Count - 1; i >= 0; i--)
            {
                this.groupBoxPuvodniTerminy[i].Controls.Remove(this.labelOdstraneno[i]);
                this.groupBoxPuvodniTerminy[i].Controls.Remove(this.labelOdstranenoDatum[i]);
                this.groupBoxPuvodniTerminy[i].Controls.Remove(this.labelPuvodniTerminy[i]);
                this.groupBoxPuvodniTerminy[i].Controls.Remove(this.labelPuvodniTerminyDatum[i]);
                this.groupBoxPuvodniTerminy[i].Controls.Remove(this.richTextBoxPoznamka[i]);

                this.panelPuvodniDatumy.Controls.Remove(this.groupBoxPuvodniTerminy[i]);

                this.labelOdstraneno[i].Dispose();
                this.labelOdstranenoDatum[i].Dispose();
                this.labelPuvodniTerminy[i].Dispose();
                this.labelPuvodniTerminyDatum[i].Dispose();
                this.richTextBoxPoznamka[i].Dispose();

                this.groupBoxPuvodniTerminy[i].Dispose();
            }

            this.groupBoxPuvodniTerminy = new List<GroupBox>();
            this.labelOdstraneno = new List<Label>();
            this.labelOdstranenoDatum = new List<Label>();
            this.labelPuvodniTerminy = new List<Label>();
            this.labelPuvodniTerminyDatum = new List<Label>();
            this.richTextBoxPoznamka = new List<RichTextBox>();
        }

        private void ButtonClose_MouseClick(object sender, MouseEventArgs e) => this.Close();
    }
}
