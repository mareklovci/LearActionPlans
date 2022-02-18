
namespace LearActionPlans.Views
{
    partial class FormPosunutiTerminuBodAP
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
            this.richTextBoxNovaPoznamka = new System.Windows.Forms.RichTextBox();
            this.labelNovaPoznamka = new System.Windows.Forms.Label();
            this.dateTimePickerNovyTerminUkonceni = new System.Windows.Forms.DateTimePicker();
            this.ButtonZadost = new System.Windows.Forms.Button();
            this.ButtonZavrit = new System.Windows.Forms.Button();
            this.groupBoxZadost = new System.Windows.Forms.GroupBox();
            this.CheckBoxPoslatZadost = new System.Windows.Forms.CheckBox();
            this.ButtonUlozit = new System.Windows.Forms.Button();
            this.groupBoxZadost.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxNovaPoznamka
            // 
            this.richTextBoxNovaPoznamka.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.richTextBoxNovaPoznamka.Location = new System.Drawing.Point(11, 107);
            this.richTextBoxNovaPoznamka.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.richTextBoxNovaPoznamka.Name = "richTextBoxNovaPoznamka";
            this.richTextBoxNovaPoznamka.Size = new System.Drawing.Size(340, 120);
            this.richTextBoxNovaPoznamka.TabIndex = 19;
            this.richTextBoxNovaPoznamka.Text = "";
            // 
            // labelNovaPoznamka
            // 
            this.labelNovaPoznamka.AutoSize = true;
            this.labelNovaPoznamka.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelNovaPoznamka.Location = new System.Drawing.Point(11, 88);
            this.labelNovaPoznamka.Name = "labelNovaPoznamka";
            this.labelNovaPoznamka.Size = new System.Drawing.Size(38, 17);
            this.labelNovaPoznamka.TabIndex = 18;
            this.labelNovaPoznamka.Text = "Note";
            // 
            // dateTimePickerNovyTerminUkonceni
            // 
            this.dateTimePickerNovyTerminUkonceni.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dateTimePickerNovyTerminUkonceni.Location = new System.Drawing.Point(11, 55);
            this.dateTimePickerNovyTerminUkonceni.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateTimePickerNovyTerminUkonceni.Name = "dateTimePickerNovyTerminUkonceni";
            this.dateTimePickerNovyTerminUkonceni.Size = new System.Drawing.Size(229, 23);
            this.dateTimePickerNovyTerminUkonceni.TabIndex = 17;
            // 
            // ButtonZadost
            // 
            this.ButtonZadost.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonZadost.Location = new System.Drawing.Point(151, 236);
            this.ButtonZadost.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ButtonZadost.Name = "ButtonZadost";
            this.ButtonZadost.Size = new System.Drawing.Size(200, 30);
            this.ButtonZadost.TabIndex = 23;
            this.ButtonZadost.Text = "Request for a new Deadline";
            this.ButtonZadost.UseVisualStyleBackColor = true;
            this.ButtonZadost.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonZadost_MouseClick);
            // 
            // ButtonZavrit
            // 
            this.ButtonZavrit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonZavrit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonZavrit.Location = new System.Drawing.Point(1024, 620);
            this.ButtonZavrit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ButtonZavrit.Name = "ButtonZavrit";
            this.ButtonZavrit.Size = new System.Drawing.Size(111, 30);
            this.ButtonZavrit.TabIndex = 21;
            this.ButtonZavrit.Text = "Close";
            this.ButtonZavrit.UseVisualStyleBackColor = true;
            this.ButtonZavrit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonZavrit_MouseClick);
            // 
            // groupBoxZadost
            // 
            this.groupBoxZadost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxZadost.Controls.Add(this.CheckBoxPoslatZadost);
            this.groupBoxZadost.Controls.Add(this.ButtonZadost);
            this.groupBoxZadost.Controls.Add(this.richTextBoxNovaPoznamka);
            this.groupBoxZadost.Controls.Add(this.dateTimePickerNovyTerminUkonceni);
            this.groupBoxZadost.Controls.Add(this.labelNovaPoznamka);
            this.groupBoxZadost.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.groupBoxZadost.Location = new System.Drawing.Point(775, 10);
            this.groupBoxZadost.Name = "groupBoxZadost";
            this.groupBoxZadost.Size = new System.Drawing.Size(360, 280);
            this.groupBoxZadost.TabIndex = 24;
            this.groupBoxZadost.TabStop = false;
            this.groupBoxZadost.Text = "New deadline";
            // 
            // CheckBoxPoslatZadost
            // 
            this.CheckBoxPoslatZadost.AutoSize = true;
            this.CheckBoxPoslatZadost.Location = new System.Drawing.Point(11, 27);
            this.CheckBoxPoslatZadost.Name = "CheckBoxPoslatZadost";
            this.CheckBoxPoslatZadost.Size = new System.Drawing.Size(112, 23);
            this.CheckBoxPoslatZadost.TabIndex = 20;
            this.CheckBoxPoslatZadost.Text = "New Deadline";
            this.CheckBoxPoslatZadost.UseVisualStyleBackColor = true;
            // 
            // ButtonUlozit
            // 
            this.ButtonUlozit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonUlozit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonUlozit.Location = new System.Drawing.Point(660, 620);
            this.ButtonUlozit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ButtonUlozit.Name = "ButtonUlozit";
            this.ButtonUlozit.Size = new System.Drawing.Size(111, 30);
            this.ButtonUlozit.TabIndex = 25;
            this.ButtonUlozit.Text = "Save";
            this.ButtonUlozit.UseVisualStyleBackColor = true;
            this.ButtonUlozit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonSave_MouseClick);
            // 
            // FormPosunutiTerminuAkce
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 661);
            this.Controls.Add(this.ButtonUlozit);
            this.Controls.Add(this.groupBoxZadost);
            this.Controls.Add(this.ButtonZavrit);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.MaximumSize = new System.Drawing.Size(1160, 950);
            this.MinimumSize = new System.Drawing.Size(1160, 700);
            this.Name = "FormPosunutiTerminuAkce";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Overview of deadlines";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPosunutiTerminuAkce_FormClosing);
            this.Load += new System.EventHandler(this.FormPosunutiTerminuAkce_Load);
            this.groupBoxZadost.ResumeLayout(false);
            this.groupBoxZadost.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox richTextBoxNovaPoznamka;
        private System.Windows.Forms.Label labelNovaPoznamka;
        private System.Windows.Forms.DateTimePicker dateTimePickerNovyTerminUkonceni;
        private System.Windows.Forms.Button ButtonZadost;
        private System.Windows.Forms.Button ButtonZavrit;
        private System.Windows.Forms.GroupBox groupBoxZadost;
        private System.Windows.Forms.CheckBox CheckBoxPoslatZadost;
        private System.Windows.Forms.Button ButtonUlozit;
    }
}