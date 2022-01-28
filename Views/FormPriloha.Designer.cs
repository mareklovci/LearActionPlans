
namespace LearActionPlans.Views
{
    partial class FormPriloha
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ButtonOdstranitSlozku = new System.Windows.Forms.Button();
            this.ButtonZavrit = new System.Windows.Forms.Button();
            this.ButtonPridatSlozku = new System.Windows.Forms.Button();
            this.labelSlozka = new System.Windows.Forms.Label();
            this.ButtonPotvrdit = new System.Windows.Forms.Button();
            this.richTextBoxPridanaSlozka = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // ButtonOdstranitSlozku
            // 
            this.ButtonOdstranitSlozku.Location = new System.Drawing.Point(178, 169);
            this.ButtonOdstranitSlozku.Name = "ButtonOdstranitSlozku";
            this.ButtonOdstranitSlozku.Size = new System.Drawing.Size(110, 30);
            this.ButtonOdstranitSlozku.TabIndex = 0;
            this.ButtonOdstranitSlozku.Text = "Remove folder";
            this.ButtonOdstranitSlozku.UseVisualStyleBackColor = true;
            this.ButtonOdstranitSlozku.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonOdstranitSlozku_MouseClick);
            // 
            // ButtonZavrit
            // 
            this.ButtonZavrit.Location = new System.Drawing.Point(412, 169);
            this.ButtonZavrit.Name = "ButtonZavrit";
            this.ButtonZavrit.Size = new System.Drawing.Size(110, 30);
            this.ButtonZavrit.TabIndex = 1;
            this.ButtonZavrit.Text = "Close";
            this.ButtonZavrit.UseVisualStyleBackColor = true;
            this.ButtonZavrit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonZavrit_MouseClick);
            // 
            // ButtonPridatSlozku
            // 
            this.ButtonPridatSlozku.Location = new System.Drawing.Point(62, 169);
            this.ButtonPridatSlozku.Name = "ButtonPridatSlozku";
            this.ButtonPridatSlozku.Size = new System.Drawing.Size(110, 30);
            this.ButtonPridatSlozku.TabIndex = 3;
            this.ButtonPridatSlozku.Text = "Add folder";
            this.ButtonPridatSlozku.UseVisualStyleBackColor = true;
            this.ButtonPridatSlozku.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonPridatSlozku_MouseClick);
            // 
            // labelSlozka
            // 
            this.labelSlozka.AutoSize = true;
            this.labelSlozka.Location = new System.Drawing.Point(20, 25);
            this.labelSlozka.Name = "labelSlozka";
            this.labelSlozka.Size = new System.Drawing.Size(87, 17);
            this.labelSlozka.TabIndex = 4;
            this.labelSlozka.Text = "Folder name";
            // 
            // ButtonPotvrdit
            // 
            this.ButtonPotvrdit.Location = new System.Drawing.Point(294, 169);
            this.ButtonPotvrdit.Name = "ButtonPotvrdit";
            this.ButtonPotvrdit.Size = new System.Drawing.Size(110, 30);
            this.ButtonPotvrdit.TabIndex = 5;
            this.ButtonPotvrdit.Text = "OK";
            this.ButtonPotvrdit.UseVisualStyleBackColor = true;
            this.ButtonPotvrdit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonPotvrdit_MouseClick);
            // 
            // richTextBoxPridanaSlozka
            // 
            this.richTextBoxPridanaSlozka.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxPridanaSlozka.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxPridanaSlozka.Location = new System.Drawing.Point(23, 45);
            this.richTextBoxPridanaSlozka.Name = "richTextBoxPridanaSlozka";
            this.richTextBoxPridanaSlozka.Size = new System.Drawing.Size(490, 80);
            this.richTextBoxPridanaSlozka.TabIndex = 6;
            this.richTextBoxPridanaSlozka.Text = "";
            // 
            // FormPriloha
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 211);
            this.Controls.Add(this.richTextBoxPridanaSlozka);
            this.Controls.Add(this.ButtonPotvrdit);
            this.Controls.Add(this.labelSlozka);
            this.Controls.Add(this.ButtonPridatSlozku);
            this.Controls.Add(this.ButtonZavrit);
            this.Controls.Add(this.ButtonOdstranitSlozku);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(550, 250);
            this.MinimumSize = new System.Drawing.Size(550, 250);
            this.Name = "FormPriloha";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Attachment";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPriloha_FormClosing);
            this.Load += new System.EventHandler(this.FormPriloha_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonOdstranitSlozku;
        private System.Windows.Forms.Button ButtonZavrit;
        private System.Windows.Forms.Button ButtonPridatSlozku;
        private System.Windows.Forms.Label labelSlozka;
        private System.Windows.Forms.Button ButtonPotvrdit;
        private System.Windows.Forms.RichTextBox richTextBoxPridanaSlozka;
    }
}