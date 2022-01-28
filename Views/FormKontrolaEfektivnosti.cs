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
            InitializeComponent();
            var podminkaDatum = datum == null;

            novyBodAP_ = novyBodAP;
            bodAPId_ = bodAPId;
            kontrolaEfektivnosti_ = kontrolaEfektivnosti;
            apUzavren_ = apUzavren;
            deadLineZadan_ = deadLineZadan;
            datum_ = datum;
            //vlastnikAP_ = vlastnikAP;
            //vlastnikAkce_ = vlastnikAkce;

            dateTimePickerKontrolaEfektivnosti.Value = podminkaDatum ? DateTime.Now : Convert.ToDateTime(datum);

            groupBoxPuvodniTerminy = new List<GroupBox>();
            labelOdstraneno = new List<Label>();
            labelOdstranenoDatum = new List<Label>();
            labelPuvodniTerminy = new List<Label>();
            labelPuvodniTerminyDatum = new List<Label>();
            richTextBoxPoznamka = new List<RichTextBox>();
        }

        private void FormKontrolaEfektivnosti_Load(object sender, EventArgs e)
        {
            InitFormLoad();

            if (novyBodAP_ == true)
                ButtonNoveDatum.Text = "OK";
            else
                ButtonNoveDatum.Text = "Save";

        }

        private void InitFormLoad()
        {
            //ButtonOk.Visible = true;
            dateTimePickerKontrolaEfektivnosti.Visible = true;
            labelDatumEfektivnosti.Visible = false;

            if (kontrolaEfektivnosti_ == true)
            {
                //datum efektivnosti je zadáno
                labelDatumEfektivnosti.Text = Convert.ToDateTime(datum_).ToShortDateString();
                labelDatumEfektivnosti.Visible = true;
            }
            if (kontrolaEfektivnosti_ == false)
            {
                labelDatumEfektivnosti.Visible = false;
            }

            //efektivita není zadána a AP není uzavřen, deadLine bodu AP je nastaven
            //nemohu jenom odstranit
            if (apUzavren_ == false)
            {
                if (deadLineZadan_ == true && kontrolaEfektivnosti_ == false)
                {
                    groupBoxNovaKontrolaEfektivnosti.Visible = true;
                    groupBoxOdstranitEfektivitu.Visible = false;
                }

                //efektivita je zadána a bod AP má deadLine
                //mohu jenom odstranit
                if (kontrolaEfektivnosti_ == true)
                {
                    groupBoxNovaKontrolaEfektivnosti.Visible = false;
                    groupBoxOdstranitEfektivitu.Location = new Point(20, 80);
                    groupBoxOdstranitEfektivitu.Visible = true;
                }
            }

            //při uzavření AP nebo u bodu AP není zadán deadLine už nemohu editovat
            if (apUzavren_ == true || deadLineZadan_ == false)
            {
                groupBoxNovaKontrolaEfektivnosti.Visible = false;
                groupBoxOdstranitEfektivitu.Visible = false;
            }
            //if (FormMain.VlastnikAP == false && FormMain.VlastnikAkce == false)
            //{
            //    //nezobrazí se nic
            //    groupBoxNovaKontrolaEfektivnosti.Visible = false;
            //    groupBoxOdstranitEfektivitu.Visible = false;

            //    //ButtonOk.Visible = false;
            //    //ButtonNoveDatum.Visible = false;
            //    //ButtonOdstranitDatum.Visible = false;
            //    //richTextBoxPoznamkaOdstranitDatum.Visible = false;
            //    //dateTimePickerKontrolaEfektivnosti.Visible = false;
            //}

            panelPuvodniDatumy.HorizontalScroll.Enabled = false;
            panelPuvodniDatumy.HorizontalScroll.Visible = false;
            panelPuvodniDatumy.HorizontalScroll.Maximum = 0;

            //zobrazení původních termínů kontrol efektivity
            var efektivita = KontrolaEfektivnostiViewModel.PuvodniTerminyEfektivnost(Convert.ToInt32(bodAPId_)).ToList();

            int i = 0;
            foreach (var ef in efektivita)
            {
                groupBoxPuvodniTerminy.Add(new GroupBox()
                {
                    Name = "groupBoxPuvodniTerminy" + i + 1,
                    Location = new Point(10, 10 + (i * 130)),
                    Size = new Size(420, 130),
                    Text = (i + 1).ToString() + ". term",
                });
                labelPuvodniTerminy.Add(new Label()
                {
                    Name = "labelPuvodniTerminy" + i + 1,
                    Location = new Point(10, 20),
                    AutoSize = true,
                    Text = @"The original date",
                    ForeColor = Color.Black
                });
                labelPuvodniTerminyDatum.Add(new Label()
                {
                    Name = "labelPuvodniTerminyDatum" + i + 1,
                    Location = new Point(10, 40),
                    AutoSize = true,
                    Text = ef.PuvodniDatum.ToShortDateString(),
                    ForeColor = Color.Black
                });
                labelOdstraneno.Add(new Label()
                {
                    Name = "labelOdstraneno" + i + 1,
                    Location = new Point(150, 20),
                    AutoSize = true,
                    Text = @"Removed",
                    ForeColor = Color.Black
                });
                labelOdstranenoDatum.Add(new Label()
                {
                    Name = "labelOdstranenoDatum" + i + 1,
                    Location = new Point(150, 40),
                    AutoSize = true,
                    Text = ef.DatumOdstraneni.ToShortDateString(),
                    ForeColor = Color.Black
                });
                richTextBoxPoznamka.Add(new RichTextBox()
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

            for (i = 0; i < groupBoxPuvodniTerminy.Count; i++)
            {
                GroupBox itemGrpBox = groupBoxPuvodniTerminy[i];
                panelPuvodniDatumy.Controls.Add(groupBoxPuvodniTerminy[i]);
                groupBoxPuvodniTerminy[i].Controls.Add(labelPuvodniTerminy[i]);
                groupBoxPuvodniTerminy[i].Controls.Add(labelPuvodniTerminyDatum[i]);

                groupBoxPuvodniTerminy[i].Controls.Add(labelOdstraneno[i]);
                groupBoxPuvodniTerminy[i].Controls.Add(labelOdstranenoDatum[i]);

                groupBoxPuvodniTerminy[i].Controls.Add(richTextBoxPoznamka[i]);
            }
            ReturnValuePotvrdit = false;
        }

        private void ButtonNoveDatum_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("You really want to create effectiveness.", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.Yes)
            {
                dateTimePickerKontrolaEfektivnosti.Visible = true;
                ButtonNoveDatum.Enabled = false;

                ReturnValueDatum = dateTimePickerKontrolaEfektivnosti.Value;
                ReturnValuePuvodniDatum = null;
                ReturnValuePoznamka = string.Empty;

                if (novyBodAP_ == false)
                    BodAPDataMapper.UpdateKontrolaEfektivity(bodAPId_, Convert.ToDateTime(dateTimePickerKontrolaEfektivnosti.Value));

                //znovu načíst hodnoty
                //InitFormLoad();
                ReturnValuePotvrdit = true;
                //DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void ButtonOdstranitDatum_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("You really want to cancel effectiveness.", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.Yes)
            {
                if (richTextBoxPoznamkaOdstranitDatum.Text == string.Empty)
                {
                    MessageBox.Show("You must fill out a note.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    labelDatumEfektivnosti.Text = string.Empty;
                    //ButtonOk.Enabled = true;

                    //ButtonNoveDatum.Visible = true;
                    ButtonOdstranitDatum.Enabled = false;
                    richTextBoxPoznamkaOdstranitDatum.Enabled = false;

                    ReturnValueDatum = null;
                    ReturnValuePuvodniDatum = dateTimePickerKontrolaEfektivnosti.Value;
                    ReturnValuePoznamka = richTextBoxPoznamkaOdstranitDatum.Text;

                    BodAPDataMapper.RemoveKontrolaEfektivity(bodAPId_, datum_, richTextBoxPoznamkaOdstranitDatum.Text);

                    //vyprázdní panel pro původní efektivitu
                    //RemoveControls();

                    //znovu načíst hodnoty
                    //InitFormLoad();
                    ReturnValuePotvrdit = true;
                    //DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void RemoveControls()
        {
            for (int i = groupBoxPuvodniTerminy.Count - 1; i >= 0; i--)
            {
                groupBoxPuvodniTerminy[i].Controls.Remove(labelOdstraneno[i]);
                groupBoxPuvodniTerminy[i].Controls.Remove(labelOdstranenoDatum[i]);
                groupBoxPuvodniTerminy[i].Controls.Remove(labelPuvodniTerminy[i]);
                groupBoxPuvodniTerminy[i].Controls.Remove(labelPuvodniTerminyDatum[i]);
                groupBoxPuvodniTerminy[i].Controls.Remove(richTextBoxPoznamka[i]);

                panelPuvodniDatumy.Controls.Remove(groupBoxPuvodniTerminy[i]);

                labelOdstraneno[i].Dispose();
                labelOdstranenoDatum[i].Dispose();
                labelPuvodniTerminy[i].Dispose();
                labelPuvodniTerminyDatum[i].Dispose();
                richTextBoxPoznamka[i].Dispose();

                groupBoxPuvodniTerminy[i].Dispose();
            }

            groupBoxPuvodniTerminy = new List<GroupBox>();
            labelOdstraneno = new List<Label>();
            labelOdstranenoDatum = new List<Label>();
            labelPuvodniTerminy = new List<Label>();
            labelPuvodniTerminyDatum = new List<Label>();
            richTextBoxPoznamka = new List<RichTextBox>();
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }
    }
}
