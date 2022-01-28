
namespace LearActionPlans.Views
{
    partial class FormDatumUkonceni
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
            this.ButtonOk = new System.Windows.Forms.Button();
            this.ButtonZavrit = new System.Windows.Forms.Button();
            this.labelDatumUkonceni = new System.Windows.Forms.Label();
            this.dateTimePickerDatumUkonceni = new System.Windows.Forms.DateTimePicker();
            this.labelPoznamka = new System.Windows.Forms.Label();
            this.richTextBoxPoznamka = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // ButtonOk
            // 
            this.ButtonOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonOk.Location = new System.Drawing.Point(246, 269);
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.Size = new System.Drawing.Size(110, 30);
            this.ButtonOk.TabIndex = 7;
            this.ButtonOk.Text = "OK";
            this.ButtonOk.UseVisualStyleBackColor = true;
            this.ButtonOk.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonOk_MouseClick);
            // 
            // ButtonZavrit
            // 
            this.ButtonZavrit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonZavrit.Location = new System.Drawing.Point(362, 269);
            this.ButtonZavrit.Name = "ButtonZavrit";
            this.ButtonZavrit.Size = new System.Drawing.Size(110, 30);
            this.ButtonZavrit.TabIndex = 6;
            this.ButtonZavrit.Text = "Close";
            this.ButtonZavrit.UseVisualStyleBackColor = true;
            this.ButtonZavrit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonZavrit_MouseClick);
            // 
            // labelDatumUkonceni
            // 
            this.labelDatumUkonceni.AutoSize = true;
            this.labelDatumUkonceni.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelDatumUkonceni.Location = new System.Drawing.Point(20, 20);
            this.labelDatumUkonceni.Name = "labelDatumUkonceni";
            this.labelDatumUkonceni.Size = new System.Drawing.Size(64, 17);
            this.labelDatumUkonceni.TabIndex = 5;
            this.labelDatumUkonceni.Text = "Deadline";
            // 
            // dateTimePickerDatumUkonceni
            // 
            this.dateTimePickerDatumUkonceni.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dateTimePickerDatumUkonceni.Location = new System.Drawing.Point(23, 40);
            this.dateTimePickerDatumUkonceni.Name = "dateTimePickerDatumUkonceni";
            this.dateTimePickerDatumUkonceni.Size = new System.Drawing.Size(229, 23);
            this.dateTimePickerDatumUkonceni.TabIndex = 4;
            // 
            // labelPoznamka
            // 
            this.labelPoznamka.AutoSize = true;
            this.labelPoznamka.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelPoznamka.Location = new System.Drawing.Point(20, 72);
            this.labelPoznamka.Name = "labelPoznamka";
            this.labelPoznamka.Size = new System.Drawing.Size(38, 17);
            this.labelPoznamka.TabIndex = 8;
            this.labelPoznamka.Text = "Note";
            // 
            // richTextBoxPoznamka
            // 
            this.richTextBoxPoznamka.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.richTextBoxPoznamka.Location = new System.Drawing.Point(23, 92);
            this.richTextBoxPoznamka.Name = "richTextBoxPoznamka";
            this.richTextBoxPoznamka.Size = new System.Drawing.Size(350, 80);
            this.richTextBoxPoznamka.TabIndex = 9;
            this.richTextBoxPoznamka.Text = "";
            // 
            // FormDatumUkonceni
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 311);
            this.Controls.Add(this.labelDatumUkonceni);
            this.Controls.Add(this.dateTimePickerDatumUkonceni);
            this.Controls.Add(this.richTextBoxPoznamka);
            this.Controls.Add(this.labelPoznamka);
            this.Controls.Add(this.ButtonOk);
            this.Controls.Add(this.ButtonZavrit);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 350);
            this.MinimumSize = new System.Drawing.Size(500, 350);
            this.Name = "FormDatumUkonceni";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormDatumUkonceni_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonOk;
        private System.Windows.Forms.Button ButtonZavrit;
        private System.Windows.Forms.Label labelDatumUkonceni;
        private System.Windows.Forms.DateTimePicker dateTimePickerDatumUkonceni;
        private System.Windows.Forms.Label labelPoznamka;
        private System.Windows.Forms.RichTextBox richTextBoxPoznamka;
    }
}