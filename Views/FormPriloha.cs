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
            InitializeComponent();
            folderBrowserDialogFolder = new System.Windows.Forms.FolderBrowserDialog();
            richTextBoxPridanaSlozka.Text = priloha;
            ReturnValueFolder = priloha;
            cisloRadkyDGVBody_ = cisloRadkyDGVBody;
            zavritOkno = false;
            pridatOdstranit = false;
            readOnly_ = readOnly;
            novyBodAP_ = novyBodAP;
        }

        private void FormPriloha_Load(object sender, EventArgs e)
        {
            if (ReturnValueFolder == string.Empty)
                ButtonOdstranitSlozku.Enabled = false;
            else
                ButtonPridatSlozku.Enabled = false;
            
            ButtonPotvrdit.Enabled = false;

            if (readOnly_ == true)
            {
                ButtonPridatSlozku.Visible = false;
                ButtonOdstranitSlozku.Visible = false;
                ButtonPotvrdit.Visible = false;
            }
            else
            {
                ButtonPridatSlozku.Visible = true;
                ButtonOdstranitSlozku.Visible = true;
                ButtonPotvrdit.Visible = true;
            }
        }

        private void ButtonOdstranitSlozku_MouseClick(object sender, MouseEventArgs e)
        {
            ButtonPridatSlozku.Enabled = true;
            ButtonOdstranitSlozku.Enabled = false;
            ButtonPotvrdit.Enabled = true;
            ReturnValueFolder = string.Empty;
            richTextBoxPridanaSlozka.Text = string.Empty;
            if (novyBodAP_ == false)
                FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody_].Priloha = null;
            DialogResult = DialogResult.Abort;
            zavritOkno = false;
            pridatOdstranit = false;
        }

        private void ButtonZavrit_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            zavritOkno = true;
            Close();
        }

        private void ButtonPridatSlozku_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult result = folderBrowserDialogFolder.ShowDialog();
            if (result == DialogResult.OK)
            {
                ButtonOdstranitSlozku.Enabled = true;
                ButtonPridatSlozku.Enabled = false;
                ButtonPotvrdit.Enabled = true;
                ReturnValueFolder = folderBrowserDialogFolder.SelectedPath;
                string priloha = folderBrowserDialogFolder.SelectedPath;
                //labelPridanaSlozka.Text = folderBrowserDialogFolder.SelectedPath;
                //labelPridanaSlozka.Text = priloha;
                richTextBoxPridanaSlozka.Text = priloha;
                if (novyBodAP_ == false)
                    FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody_].Priloha = priloha;
                DialogResult = DialogResult.OK;
                pridatOdstranit = true;
            }
            if (result == DialogResult.Cancel)
            {
                //nebyla vybrána žádná složka
                DialogResult = DialogResult.Cancel;
                pridatOdstranit = false;
            }
            zavritOkno = false;
        }

        private void FormPriloha_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (zavritOkno == false)
                e.Cancel = true;
        }

        private void ButtonPotvrdit_MouseClick(object sender, MouseEventArgs e)
        {
            if (pridatOdstranit == true)
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Abort;
            zavritOkno = true;
            Close();
        }
    }
}
