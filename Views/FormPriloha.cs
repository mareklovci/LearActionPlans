using System;
using System.Windows.Forms;

namespace LearActionPlans.Views
{
    public partial class FormPriloha : Form
    {
        private FolderBrowserDialog folderBrowserDialogFolder;
        private bool zavritOkno;
        private bool pridatOdstranit;
        private int cisloRadkyDGVBody_;
        private readonly bool novyBodAP_;
        public string ReturnValueFolder { get; set; }

        private readonly bool readOnly_; 

        public FormPriloha(bool novyBodAP, bool readOnly, string priloha, int cisloRadkyDGVBody)
        {
            this.InitializeComponent();
            this.folderBrowserDialogFolder = new FolderBrowserDialog();
            this.richTextBoxPridanaSlozka.Text = priloha;
            this.ReturnValueFolder = priloha;
            this.cisloRadkyDGVBody_ = cisloRadkyDGVBody;
            this.zavritOkno = false;
            this.pridatOdstranit = false;
            this.readOnly_ = readOnly;
            this.novyBodAP_ = novyBodAP;
        }

        private void FormPriloha_Load(object sender, EventArgs e)
        {
            if (this.ReturnValueFolder == string.Empty)
            {
                this.ButtonOdstranitSlozku.Enabled = false;
            }
            else
            {
                this.ButtonPridatSlozku.Enabled = false;
            }

            this.ButtonPotvrdit.Enabled = false;

            if (this.readOnly_ == true)
            {
                this.ButtonPridatSlozku.Visible = false;
                this.ButtonOdstranitSlozku.Visible = false;
                this.ButtonPotvrdit.Visible = false;
            }
            else
            {
                this.ButtonPridatSlozku.Visible = true;
                this.ButtonOdstranitSlozku.Visible = true;
                this.ButtonPotvrdit.Visible = true;
            }
        }

        private void ButtonOdstranitSlozku_MouseClick(object sender, MouseEventArgs e)
        {
            this.ButtonPridatSlozku.Enabled = true;
            this.ButtonOdstranitSlozku.Enabled = false;
            this.ButtonPotvrdit.Enabled = true;
            this.ReturnValueFolder = string.Empty;
            this.richTextBoxPridanaSlozka.Text = string.Empty;
            if (this.novyBodAP_ == false)
            {
                FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody_].Priloha = null;
            }

            this.DialogResult = DialogResult.Abort;
            this.zavritOkno = false;
            this.pridatOdstranit = false;
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.zavritOkno = true;
            this.Close();
        }

        private void ButtonPridatSlozku_MouseClick(object sender, MouseEventArgs e)
        {
            var result = this.folderBrowserDialogFolder.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.ButtonOdstranitSlozku.Enabled = true;
                this.ButtonPridatSlozku.Enabled = false;
                this.ButtonPotvrdit.Enabled = true;
                this.ReturnValueFolder = this.folderBrowserDialogFolder.SelectedPath;
                var priloha = this.folderBrowserDialogFolder.SelectedPath;
                //labelPridanaSlozka.Text = folderBrowserDialogFolder.SelectedPath;
                //labelPridanaSlozka.Text = priloha;
                this.richTextBoxPridanaSlozka.Text = priloha;
                if (this.novyBodAP_ == false)
                {
                    FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody_].Priloha = priloha;
                }

                this.DialogResult = DialogResult.OK;
                this.pridatOdstranit = true;
            }
            if (result == DialogResult.Cancel)
            {
                //nebyla vybrána žádná složka
                this.DialogResult = DialogResult.Cancel;
                this.pridatOdstranit = false;
            }

            this.zavritOkno = false;
        }

        private void FormPriloha_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.zavritOkno == false)
            {
                e.Cancel = true;
            }
        }

        private void ButtonPotvrdit_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.pridatOdstranit == true)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Abort;
            }

            this.zavritOkno = true;
            this.Close();
        }
    }
}
