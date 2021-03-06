using LearActionPlans.DataMappers;
using LearActionPlans.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LearActionPlans.Views
{
    public partial class FormKontrolaEfektivnosti : Form
    {
        public DateTime? ReturnValuePuvodniDatum { get; set; }
        public DateTime? ReturnValueDatum { get; set; }
        public string ReturnValuePoznamka { get; set; }
        public bool ReturnValuePotvrdit { get; set; }

        private readonly int bodAPId_;
        private readonly bool kontrolaEfektivnosti_;
        private bool apUzavren_;
        private readonly bool novyBodAP_;
        private bool deadLineZadan_;

        private readonly DateTime? datum_;

        private List<GroupBox> groupBoxPuvodniTerminy;
        private List<Label> labelOdstraneno;
        private List<Label> labelOdstranenoDatum;
        private List<Label> labelPuvodniTerminy;
        private List<Label> labelPuvodniTerminyDatum;
        private List<RichTextBox> richTextBoxPoznamka;

        public FormKontrolaEfektivnosti(bool novyBodAP, bool apUzavren, bool deadLineZadan, int bodAPId, bool kontrolaEfektivnosti, DateTime? datum)
        {
            this.InitializeComponent();
            var podminkaDatum = datum == null;

            this.novyBodAP_ = novyBodAP;
            this.bodAPId_ = bodAPId;
            this.kontrolaEfektivnosti_ = kontrolaEfektivnosti;
            this.apUzavren_ = apUzavren;
            this.deadLineZadan_ = deadLineZadan;
            this.datum_ = datum;
            //vlastnikAP_ = vlastnikAP;
            //vlastnikAkce_ = vlastnikAkce;

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

            if (this.novyBodAP_ == true)
            {
                if (this.datum_ == null)
                {
                    this.dateTimePickerKontrolaEfektivnosti.Value = DateTime.Now;
                }
                else
                {
                    // p??i vytv????en?? nov??ho bodu si mus??m pamatovat p??vodn?? zadan?? datum efektivnosti
                    this.dateTimePickerKontrolaEfektivnosti.Value = Convert.ToDateTime(this.datum_);
                }

                this.ButtonNoveDatum.Text = "OK";

            }
            else
            {
                this.ButtonNoveDatum.Text = "Save";
            }
        }

        private void InitFormLoad()
        {
            //ButtonOk.Visible = true;
            this.dateTimePickerKontrolaEfektivnosti.Visible = true;
            this.labelDatumEfektivnosti.Visible = false;

            if (this.kontrolaEfektivnosti_ == true)
            {
                //datum efektivnosti je zad??no
                this.labelDatumEfektivnosti.Text = Convert.ToDateTime(this.datum_).ToShortDateString();
                this.labelDatumEfektivnosti.Visible = true;
            }
            if (this.kontrolaEfektivnosti_ == false)
            {
                this.labelDatumEfektivnosti.Visible = false;
            }

            //efektivita nen?? zad??na a AP nen?? uzav??en, deadLine bodu AP je nastaven
            //nemohu jenom odstranit
            if (this.apUzavren_ == false)
            {
                if (this.deadLineZadan_ == true && this.kontrolaEfektivnosti_ == false)
                {
                    this.groupBoxNovaKontrolaEfektivnosti.Visible = true;
                    this.groupBoxOdstranitEfektivitu.Visible = false;
                }

                //efektivita je zad??na a bod AP m?? deadLine
                //mohu jenom odstranit
                if (this.kontrolaEfektivnosti_ == true)
                {
                    this.groupBoxNovaKontrolaEfektivnosti.Visible = false;
                    this.groupBoxOdstranitEfektivitu.Location = new Point(20, 80);
                    this.groupBoxOdstranitEfektivitu.Visible = true;
                }
            }

            //p??i uzav??en?? AP nebo u bodu AP nen?? zad??n deadLine u?? nemohu editovat
            if (this.apUzavren_ == true || this.deadLineZadan_ == false)
            {
                this.groupBoxNovaKontrolaEfektivnosti.Visible = false;
                this.groupBoxOdstranitEfektivitu.Visible = false;
            }
            //if (FormMain.VlastnikAP == false && FormMain.VlastnikAkce == false)
            //{
            //    //nezobraz?? se nic
            //    groupBoxNovaKontrolaEfektivnosti.Visible = false;
            //    groupBoxOdstranitEfektivitu.Visible = false;

            //    //ButtonOk.Visible = false;
            //    //ButtonNoveDatum.Visible = false;
            //    //ButtonOdstranitDatum.Visible = false;
            //    //richTextBoxPoznamkaOdstranitDatum.Visible = false;
            //    //dateTimePickerKontrolaEfektivnosti.Visible = false;
            //}

            this.panelPuvodniDatumy.HorizontalScroll.Enabled = false;
            this.panelPuvodniDatumy.HorizontalScroll.Visible = false;
            this.panelPuvodniDatumy.HorizontalScroll.Maximum = 0;

            //zobrazen?? p??vodn??ch term??n?? kontrol efektivity
            var efektivita = KontrolaEfektivnostiViewModel.PuvodniTerminyEfektivnost(Convert.ToInt32(this.bodAPId_)).ToList();

            var i = 0;
            foreach (var ef in efektivita)
            {
                this.groupBoxPuvodniTerminy.Add(new GroupBox()
                {
                    Name = "groupBoxPuvodniTerminy" + i + 1,
                    Location = new Point(10, 10 + (i * 130)),
                    Size = new Size(420, 130),
                    Text = (i + 1).ToString() + ". term",
                });
                this.labelPuvodniTerminy.Add(new Label()
                {
                    Name = "labelPuvodniTerminy" + i + 1,
                    Location = new Point(10, 20),
                    AutoSize = true,
                    Text = @"The original date",
                    ForeColor = Color.Black
                });
                this.labelPuvodniTerminyDatum.Add(new Label()
                {
                    Name = "labelPuvodniTerminyDatum" + i + 1,
                    Location = new Point(10, 40),
                    AutoSize = true,
                    Text = ef.PuvodniDatum.ToShortDateString(),
                    ForeColor = Color.Black
                });
                this.labelOdstraneno.Add(new Label()
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
                    Text = ef.DatumOdstraneni.ToShortDateString(),
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

        private void ButtonNoveDatum_MouseClick(object sender, MouseEventArgs e)
        {
            var dialogResult = MessageBox.Show("You really want to create effectiveness.", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.Yes)
            {
                this.dateTimePickerKontrolaEfektivnosti.Visible = true;
                this.ButtonNoveDatum.Enabled = false;

                this.ReturnValueDatum = this.dateTimePickerKontrolaEfektivnosti.Value;
                this.ReturnValuePuvodniDatum = null;
                this.ReturnValuePoznamka = string.Empty;

                if (this.novyBodAP_ == false)
                {
                    BodAPDataMapper.UpdateKontrolaEfektivity(this.bodAPId_, Convert.ToDateTime(this.dateTimePickerKontrolaEfektivnosti.Value));
                }

                //znovu na????st hodnoty
                //InitFormLoad();
                this.ReturnValuePotvrdit = true;
                //DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void ButtonOdstranitDatum_MouseClick(object sender, MouseEventArgs e)
        {
            var dialogResult = MessageBox.Show("You really want to cancel effectiveness.", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.Yes)
            {
                if (this.richTextBoxPoznamkaOdstranitDatum.Text == string.Empty)
                {
                    MessageBox.Show("You must fill out a note.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                    BodAPDataMapper.RemoveKontrolaEfektivity(this.bodAPId_, this.datum_, this.richTextBoxPoznamkaOdstranitDatum.Text);

                    //vypr??zdn?? panel pro p??vodn?? efektivitu
                    //RemoveControls();

                    //znovu na????st hodnoty
                    //InitFormLoad();
                    this.ReturnValuePotvrdit = true;
                    //DialogResult = DialogResult.OK;
                    this.Close();
                }
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

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
    }
}
