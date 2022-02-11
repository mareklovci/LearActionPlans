using System;
using System.Windows.Forms;

namespace LearActionPlans.Views
{
    public partial class FormAttachment : Form
    {
        private FolderBrowserDialog folderBrowserDialogFolder;
        private bool closeForm;
        private bool pridatOdstranit;
        private int cisloRadkyDgvBody;
        private bool newActionPlanPoint;
        public string ReturnValueFolder { get; private set; }

        private bool readOnly;

        public FormAttachment() => this.InitializeComponent();

        public void CreateFormAttachment(bool newPoint, bool readOnlyField, string attachment, int rowNumber)
        {
            this.richTextBoxPridanaSlozka.Text = attachment;
            this.ReturnValueFolder = attachment;
            this.cisloRadkyDgvBody = rowNumber;
            this.closeForm = false;
            this.pridatOdstranit = false;
            this.readOnly = readOnlyField;
            this.newActionPlanPoint = newPoint;

            this.folderBrowserDialogFolder = new FolderBrowserDialog();
        }

        private void FormAttachment_Load(object sender, EventArgs e)
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

            if (this.readOnly)
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

        private void ButtonDeleteFolder_MouseClick(object sender, MouseEventArgs e)
        {
            this.ButtonPridatSlozku.Enabled = true;
            this.ButtonOdstranitSlozku.Enabled = false;
            this.ButtonPotvrdit.Enabled = true;
            this.ReturnValueFolder = string.Empty;
            this.richTextBoxPridanaSlozka.Text = string.Empty;
            if (this.newActionPlanPoint == false)
            {
                FormPrehledBoduAP.bodyAP[this.cisloRadkyDgvBody].Priloha = null;
            }

            this.DialogResult = DialogResult.Abort;
            this.closeForm = false;
            this.pridatOdstranit = false;
        }

        private void ButtonClose_MouseClick(object sender, MouseEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.closeForm = true;
            this.Close();
        }

        private void ButtonAddFolder_MouseClick(object sender, MouseEventArgs e)
        {
            var result = this.folderBrowserDialogFolder.ShowDialog();
            switch (result)
            {
                case DialogResult.OK:
                {
                    this.ButtonOdstranitSlozku.Enabled = true;
                    this.ButtonPridatSlozku.Enabled = false;
                    this.ButtonPotvrdit.Enabled = true;
                    this.ReturnValueFolder = this.folderBrowserDialogFolder.SelectedPath;
                    var priloha = this.folderBrowserDialogFolder.SelectedPath;
                    //labelPridanaSlozka.Text = folderBrowserDialogFolder.SelectedPath;
                    //labelPridanaSlozka.Text = priloha;
                    this.richTextBoxPridanaSlozka.Text = priloha;

                    if (this.newActionPlanPoint == false)
                    {
                        FormPrehledBoduAP.bodyAP[this.cisloRadkyDgvBody].Priloha = priloha;
                    }

                    this.DialogResult = DialogResult.OK;
                    this.pridatOdstranit = true;
                    break;
                }
                case DialogResult.Cancel:
                    //nebyla vybrána žádná složka
                    this.DialogResult = DialogResult.Cancel;
                    this.pridatOdstranit = false;
                    break;
                case DialogResult.None:
                case DialogResult.Abort:
                case DialogResult.Retry:
                case DialogResult.Ignore:
                case DialogResult.Yes:
                case DialogResult.No:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.closeForm = false;
        }

        private void FormAttachment_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.closeForm == false)
            {
                e.Cancel = true;
            }
        }

        private void ButtonConfirm_MouseClick(object sender, MouseEventArgs e)
        {
            this.DialogResult = this.pridatOdstranit ? DialogResult.OK : DialogResult.Abort;
            this.closeForm = true;
            this.Close();
        }
    }
}
