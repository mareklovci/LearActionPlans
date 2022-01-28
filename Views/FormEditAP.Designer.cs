
namespace LearActionPlans.Views
{
    partial class FormEditAP
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
            this.ButtonZavrit = new System.Windows.Forms.Button();
            this.labelOdpovedny1 = new System.Windows.Forms.Label();
            this.labelOdpovedny2 = new System.Windows.Forms.Label();
            this.ComboBoxZadavatel1 = new System.Windows.Forms.ComboBox();
            this.ComboBoxZadavatel2 = new System.Windows.Forms.ComboBox();
            this.ComboBoxProjekty = new System.Windows.Forms.ComboBox();
            this.labelProjekty = new System.Windows.Forms.Label();
            this.ComboBoxZakaznici = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonUlozit = new System.Windows.Forms.Button();
            this.labelCisloAP = new System.Windows.Forms.Label();
            this.labelNumber = new System.Windows.Forms.Label();
            this.ButtonUkoncitAP = new System.Windows.Forms.Button();
            this.dateTimePickerDatumUkonceni = new System.Windows.Forms.DateTimePicker();
            this.labelPocetTerminu = new System.Windows.Forms.Label();
            this.labelZbyvajiciPocetTerminu = new System.Windows.Forms.Label();
            this.ButtonNovyTermin = new System.Windows.Forms.Button();
            this.groupBoxNovyTermin = new System.Windows.Forms.GroupBox();
            this.labelNovyTerminProdlouzeni = new System.Windows.Forms.Label();
            this.labelHistorieTerminu = new System.Windows.Forms.Label();
            this.labelPoznamkaTermin = new System.Windows.Forms.Label();
            this.richTextBoxNovaPoznamka = new System.Windows.Forms.RichTextBox();
            this.ButtonZnovuOtevrit = new System.Windows.Forms.Button();
            this.groupBoxEditAP = new System.Windows.Forms.GroupBox();
            this.groupBoxUzavreniAP = new System.Windows.Forms.GroupBox();
            this.labelDatumUzavreniNadpis = new System.Windows.Forms.Label();
            this.labelDatumUzavreni = new System.Windows.Forms.Label();
            this.groupBoxZnovuotevreniAP = new System.Windows.Forms.GroupBox();
            this.labelDuvodZnovuOtevreni = new System.Windows.Forms.Label();
            this.richTextBoxDuvodZnovuOtevreni = new System.Windows.Forms.RichTextBox();
            this.groupBoxNovyTermin.SuspendLayout();
            this.groupBoxEditAP.SuspendLayout();
            this.groupBoxUzavreniAP.SuspendLayout();
            this.groupBoxZnovuotevreniAP.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonZavrit
            // 
            this.ButtonZavrit.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonZavrit.Location = new System.Drawing.Point(949, 669);
            this.ButtonZavrit.Name = "ButtonZavrit";
            this.ButtonZavrit.Size = new System.Drawing.Size(103, 30);
            this.ButtonZavrit.TabIndex = 0;
            this.ButtonZavrit.Text = "Close";
            this.ButtonZavrit.UseVisualStyleBackColor = true;
            this.ButtonZavrit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonZavrit_MouseClick);
            // 
            // labelOdpovedny1
            // 
            this.labelOdpovedny1.AutoSize = true;
            this.labelOdpovedny1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.labelOdpovedny1.Location = new System.Drawing.Point(20, 85);
            this.labelOdpovedny1.Name = "labelOdpovedny1";
            this.labelOdpovedny1.Size = new System.Drawing.Size(109, 19);
            this.labelOdpovedny1.TabIndex = 1;
            this.labelOdpovedny1.Text = "Responsible #1";
            // 
            // labelOdpovedny2
            // 
            this.labelOdpovedny2.AutoSize = true;
            this.labelOdpovedny2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.labelOdpovedny2.Location = new System.Drawing.Point(20, 145);
            this.labelOdpovedny2.Name = "labelOdpovedny2";
            this.labelOdpovedny2.Size = new System.Drawing.Size(109, 19);
            this.labelOdpovedny2.TabIndex = 2;
            this.labelOdpovedny2.Text = "Responsible #2";
            // 
            // ComboBoxZadavatel1
            // 
            this.ComboBoxZadavatel1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxZadavatel1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ComboBoxZadavatel1.FormattingEnabled = true;
            this.ComboBoxZadavatel1.Location = new System.Drawing.Point(20, 107);
            this.ComboBoxZadavatel1.Name = "ComboBoxZadavatel1";
            this.ComboBoxZadavatel1.Size = new System.Drawing.Size(280, 27);
            this.ComboBoxZadavatel1.TabIndex = 3;
            // 
            // ComboBoxZadavatel2
            // 
            this.ComboBoxZadavatel2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxZadavatel2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ComboBoxZadavatel2.FormattingEnabled = true;
            this.ComboBoxZadavatel2.Location = new System.Drawing.Point(20, 167);
            this.ComboBoxZadavatel2.Name = "ComboBoxZadavatel2";
            this.ComboBoxZadavatel2.Size = new System.Drawing.Size(280, 27);
            this.ComboBoxZadavatel2.TabIndex = 4;
            // 
            // ComboBoxProjekty
            // 
            this.ComboBoxProjekty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxProjekty.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ComboBoxProjekty.FormattingEnabled = true;
            this.ComboBoxProjekty.Location = new System.Drawing.Point(20, 227);
            this.ComboBoxProjekty.Name = "ComboBoxProjekty";
            this.ComboBoxProjekty.Size = new System.Drawing.Size(280, 27);
            this.ComboBoxProjekty.TabIndex = 6;
            // 
            // labelProjekty
            // 
            this.labelProjekty.AutoSize = true;
            this.labelProjekty.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.labelProjekty.Location = new System.Drawing.Point(20, 205);
            this.labelProjekty.Name = "labelProjekty";
            this.labelProjekty.Size = new System.Drawing.Size(57, 19);
            this.labelProjekty.TabIndex = 5;
            this.labelProjekty.Text = "Project";
            // 
            // ComboBoxZakaznici
            // 
            this.ComboBoxZakaznici.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxZakaznici.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ComboBoxZakaznici.FormattingEnabled = true;
            this.ComboBoxZakaznici.Location = new System.Drawing.Point(20, 287);
            this.ComboBoxZakaznici.Name = "ComboBoxZakaznici";
            this.ComboBoxZakaznici.Size = new System.Drawing.Size(280, 27);
            this.ComboBoxZakaznici.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(20, 265);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = "Customer";
            // 
            // ButtonUlozit
            // 
            this.ButtonUlozit.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonUlozit.Location = new System.Drawing.Point(197, 320);
            this.ButtonUlozit.Name = "ButtonUlozit";
            this.ButtonUlozit.Size = new System.Drawing.Size(103, 30);
            this.ButtonUlozit.TabIndex = 9;
            this.ButtonUlozit.Text = "Save";
            this.ButtonUlozit.UseVisualStyleBackColor = true;
            this.ButtonUlozit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonUlozit_MouseClick);
            // 
            // labelCisloAP
            // 
            this.labelCisloAP.AutoSize = true;
            this.labelCisloAP.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelCisloAP.Location = new System.Drawing.Point(20, 30);
            this.labelCisloAP.Name = "labelCisloAP";
            this.labelCisloAP.Size = new System.Drawing.Size(109, 21);
            this.labelCisloAP.TabIndex = 10;
            this.labelCisloAP.Text = "Number of AP";
            // 
            // labelNumber
            // 
            this.labelNumber.AutoSize = true;
            this.labelNumber.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.labelNumber.Location = new System.Drawing.Point(20, 51);
            this.labelNumber.Name = "labelNumber";
            this.labelNumber.Size = new System.Drawing.Size(73, 21);
            this.labelNumber.TabIndex = 11;
            this.labelNumber.Text = "Number";
            // 
            // ButtonUkoncitAP
            // 
            this.ButtonUkoncitAP.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonUkoncitAP.Location = new System.Drawing.Point(191, 73);
            this.ButtonUkoncitAP.Name = "ButtonUkoncitAP";
            this.ButtonUkoncitAP.Size = new System.Drawing.Size(103, 30);
            this.ButtonUkoncitAP.TabIndex = 12;
            this.ButtonUkoncitAP.Text = "Close AP";
            this.ButtonUkoncitAP.UseVisualStyleBackColor = true;
            this.ButtonUkoncitAP.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonUkoncitAP_MouseClick);
            // 
            // dateTimePickerDatumUkonceni
            // 
            this.dateTimePickerDatumUkonceni.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dateTimePickerDatumUkonceni.Location = new System.Drawing.Point(20, 110);
            this.dateTimePickerDatumUkonceni.Name = "dateTimePickerDatumUkonceni";
            this.dateTimePickerDatumUkonceni.Size = new System.Drawing.Size(250, 26);
            this.dateTimePickerDatumUkonceni.TabIndex = 14;
            // 
            // labelPocetTerminu
            // 
            this.labelPocetTerminu.AutoSize = true;
            this.labelPocetTerminu.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelPocetTerminu.Location = new System.Drawing.Point(20, 35);
            this.labelPocetTerminu.Name = "labelPocetTerminu";
            this.labelPocetTerminu.Size = new System.Drawing.Size(201, 19);
            this.labelPocetTerminu.TabIndex = 15;
            this.labelPocetTerminu.Text = "Number of remaining deadlines";
            // 
            // labelZbyvajiciPocetTerminu
            // 
            this.labelZbyvajiciPocetTerminu.AutoSize = true;
            this.labelZbyvajiciPocetTerminu.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.labelZbyvajiciPocetTerminu.Location = new System.Drawing.Point(20, 54);
            this.labelZbyvajiciPocetTerminu.Name = "labelZbyvajiciPocetTerminu";
            this.labelZbyvajiciPocetTerminu.Size = new System.Drawing.Size(64, 19);
            this.labelZbyvajiciPocetTerminu.TabIndex = 16;
            this.labelZbyvajiciPocetTerminu.Text = "Number";
            // 
            // ButtonNovyTermin
            // 
            this.ButtonNovyTermin.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonNovyTermin.Location = new System.Drawing.Point(167, 376);
            this.ButtonNovyTermin.Name = "ButtonNovyTermin";
            this.ButtonNovyTermin.Size = new System.Drawing.Size(103, 30);
            this.ButtonNovyTermin.TabIndex = 17;
            this.ButtonNovyTermin.Text = "New deadline";
            this.ButtonNovyTermin.UseVisualStyleBackColor = true;
            this.ButtonNovyTermin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonNovyTermin_MouseClick);
            // 
            // groupBoxNovyTermin
            // 
            this.groupBoxNovyTermin.Controls.Add(this.labelNovyTerminProdlouzeni);
            this.groupBoxNovyTermin.Controls.Add(this.labelHistorieTerminu);
            this.groupBoxNovyTermin.Controls.Add(this.labelPoznamkaTermin);
            this.groupBoxNovyTermin.Controls.Add(this.richTextBoxNovaPoznamka);
            this.groupBoxNovyTermin.Controls.Add(this.labelPocetTerminu);
            this.groupBoxNovyTermin.Controls.Add(this.ButtonNovyTermin);
            this.groupBoxNovyTermin.Controls.Add(this.labelZbyvajiciPocetTerminu);
            this.groupBoxNovyTermin.Controls.Add(this.dateTimePickerDatumUkonceni);
            this.groupBoxNovyTermin.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.groupBoxNovyTermin.Location = new System.Drawing.Point(355, 20);
            this.groupBoxNovyTermin.Name = "groupBoxNovyTermin";
            this.groupBoxNovyTermin.Size = new System.Drawing.Size(695, 620);
            this.groupBoxNovyTermin.TabIndex = 18;
            this.groupBoxNovyTermin.TabStop = false;
            this.groupBoxNovyTermin.Text = "Deadlines";
            // 
            // labelNovyTerminProdlouzeni
            // 
            this.labelNovyTerminProdlouzeni.AutoSize = true;
            this.labelNovyTerminProdlouzeni.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelNovyTerminProdlouzeni.Location = new System.Drawing.Point(20, 88);
            this.labelNovyTerminProdlouzeni.Name = "labelNovyTerminProdlouzeni";
            this.labelNovyTerminProdlouzeni.Size = new System.Drawing.Size(91, 19);
            this.labelNovyTerminProdlouzeni.TabIndex = 24;
            this.labelNovyTerminProdlouzeni.Text = "New deadline";
            // 
            // labelHistorieTerminu
            // 
            this.labelHistorieTerminu.AutoSize = true;
            this.labelHistorieTerminu.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.labelHistorieTerminu.Location = new System.Drawing.Point(290, 35);
            this.labelHistorieTerminu.Name = "labelHistorieTerminu";
            this.labelHistorieTerminu.Size = new System.Drawing.Size(145, 19);
            this.labelHistorieTerminu.TabIndex = 23;
            this.labelHistorieTerminu.Text = "History of deadlines";
            // 
            // labelPoznamkaTermin
            // 
            this.labelPoznamkaTermin.AutoSize = true;
            this.labelPoznamkaTermin.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelPoznamkaTermin.Location = new System.Drawing.Point(20, 147);
            this.labelPoznamkaTermin.Name = "labelPoznamkaTermin";
            this.labelPoznamkaTermin.Size = new System.Drawing.Size(136, 19);
            this.labelPoznamkaTermin.TabIndex = 22;
            this.labelPoznamkaTermin.Text = "Reason for extension";
            // 
            // richTextBoxNovaPoznamka
            // 
            this.richTextBoxNovaPoznamka.Location = new System.Drawing.Point(20, 170);
            this.richTextBoxNovaPoznamka.Name = "richTextBoxNovaPoznamka";
            this.richTextBoxNovaPoznamka.Size = new System.Drawing.Size(250, 200);
            this.richTextBoxNovaPoznamka.TabIndex = 21;
            this.richTextBoxNovaPoznamka.Text = "";
            // 
            // ButtonZnovuOtevrit
            // 
            this.ButtonZnovuOtevrit.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonZnovuOtevrit.Location = new System.Drawing.Point(191, 149);
            this.ButtonZnovuOtevrit.Name = "ButtonZnovuOtevrit";
            this.ButtonZnovuOtevrit.Size = new System.Drawing.Size(103, 30);
            this.ButtonZnovuOtevrit.TabIndex = 19;
            this.ButtonZnovuOtevrit.Text = "Reopen AP";
            this.ButtonZnovuOtevrit.UseVisualStyleBackColor = true;
            this.ButtonZnovuOtevrit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonZnovuOtevrit_MouseClick);
            // 
            // groupBoxEditAP
            // 
            this.groupBoxEditAP.Controls.Add(this.groupBoxUzavreniAP);
            this.groupBoxEditAP.Controls.Add(this.groupBoxZnovuotevreniAP);
            this.groupBoxEditAP.Controls.Add(this.labelNumber);
            this.groupBoxEditAP.Controls.Add(this.labelCisloAP);
            this.groupBoxEditAP.Controls.Add(this.labelOdpovedny1);
            this.groupBoxEditAP.Controls.Add(this.labelOdpovedny2);
            this.groupBoxEditAP.Controls.Add(this.ComboBoxZadavatel1);
            this.groupBoxEditAP.Controls.Add(this.ComboBoxZadavatel2);
            this.groupBoxEditAP.Controls.Add(this.labelProjekty);
            this.groupBoxEditAP.Controls.Add(this.ButtonUlozit);
            this.groupBoxEditAP.Controls.Add(this.ComboBoxProjekty);
            this.groupBoxEditAP.Controls.Add(this.label1);
            this.groupBoxEditAP.Controls.Add(this.ComboBoxZakaznici);
            this.groupBoxEditAP.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.groupBoxEditAP.Location = new System.Drawing.Point(12, 20);
            this.groupBoxEditAP.Name = "groupBoxEditAP";
            this.groupBoxEditAP.Size = new System.Drawing.Size(320, 680);
            this.groupBoxEditAP.TabIndex = 20;
            this.groupBoxEditAP.TabStop = false;
            this.groupBoxEditAP.Text = "Action plan";
            // 
            // groupBoxUzavreniAP
            // 
            this.groupBoxUzavreniAP.Controls.Add(this.ButtonUkoncitAP);
            this.groupBoxUzavreniAP.Controls.Add(this.labelDatumUzavreniNadpis);
            this.groupBoxUzavreniAP.Controls.Add(this.labelDatumUzavreni);
            this.groupBoxUzavreniAP.Location = new System.Drawing.Point(6, 368);
            this.groupBoxUzavreniAP.Name = "groupBoxUzavreniAP";
            this.groupBoxUzavreniAP.Size = new System.Drawing.Size(308, 116);
            this.groupBoxUzavreniAP.TabIndex = 25;
            this.groupBoxUzavreniAP.TabStop = false;
            this.groupBoxUzavreniAP.Text = "Closing AP";
            // 
            // labelDatumUzavreniNadpis
            // 
            this.labelDatumUzavreniNadpis.AutoSize = true;
            this.labelDatumUzavreniNadpis.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.labelDatumUzavreniNadpis.Location = new System.Drawing.Point(14, 29);
            this.labelDatumUzavreniNadpis.Name = "labelDatumUzavreniNadpis";
            this.labelDatumUzavreniNadpis.Size = new System.Drawing.Size(105, 21);
            this.labelDatumUzavreniNadpis.TabIndex = 22;
            this.labelDatumUzavreniNadpis.Text = "Closing date";
            // 
            // labelDatumUzavreni
            // 
            this.labelDatumUzavreni.AutoSize = true;
            this.labelDatumUzavreni.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelDatumUzavreni.Location = new System.Drawing.Point(14, 50);
            this.labelDatumUzavreni.Name = "labelDatumUzavreni";
            this.labelDatumUzavreni.Size = new System.Drawing.Size(40, 21);
            this.labelDatumUzavreni.TabIndex = 23;
            this.labelDatumUzavreni.Text = "date";
            // 
            // groupBoxZnovuotevreniAP
            // 
            this.groupBoxZnovuotevreniAP.Controls.Add(this.ButtonZnovuOtevrit);
            this.groupBoxZnovuotevreniAP.Controls.Add(this.labelDuvodZnovuOtevreni);
            this.groupBoxZnovuotevreniAP.Controls.Add(this.richTextBoxDuvodZnovuOtevreni);
            this.groupBoxZnovuotevreniAP.Location = new System.Drawing.Point(6, 484);
            this.groupBoxZnovuotevreniAP.Name = "groupBoxZnovuotevreniAP";
            this.groupBoxZnovuotevreniAP.Size = new System.Drawing.Size(308, 190);
            this.groupBoxZnovuotevreniAP.TabIndex = 24;
            this.groupBoxZnovuotevreniAP.TabStop = false;
            this.groupBoxZnovuotevreniAP.Text = "Reopening AP";
            // 
            // labelDuvodZnovuOtevreni
            // 
            this.labelDuvodZnovuOtevreni.AutoSize = true;
            this.labelDuvodZnovuOtevreni.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelDuvodZnovuOtevreni.Location = new System.Drawing.Point(14, 24);
            this.labelDuvodZnovuOtevreni.Name = "labelDuvodZnovuOtevreni";
            this.labelDuvodZnovuOtevreni.Size = new System.Drawing.Size(161, 19);
            this.labelDuvodZnovuOtevreni.TabIndex = 21;
            this.labelDuvodZnovuOtevreni.Text = "Reason for reopening AP";
            // 
            // richTextBoxDuvodZnovuOtevreni
            // 
            this.richTextBoxDuvodZnovuOtevreni.Location = new System.Drawing.Point(14, 48);
            this.richTextBoxDuvodZnovuOtevreni.Name = "richTextBoxDuvodZnovuOtevreni";
            this.richTextBoxDuvodZnovuOtevreni.Size = new System.Drawing.Size(280, 90);
            this.richTextBoxDuvodZnovuOtevreni.TabIndex = 20;
            this.richTextBoxDuvodZnovuOtevreni.Text = "";
            // 
            // FormEditAP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 711);
            this.Controls.Add(this.groupBoxEditAP);
            this.Controls.Add(this.groupBoxNovyTermin);
            this.Controls.Add(this.ButtonZavrit);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1080, 750);
            this.MinimumSize = new System.Drawing.Size(1080, 750);
            this.Name = "FormEditAP";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Action plan";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEditAP_FormClosing);
            this.Load += new System.EventHandler(this.FormEditAP_Load);
            this.groupBoxNovyTermin.ResumeLayout(false);
            this.groupBoxNovyTermin.PerformLayout();
            this.groupBoxEditAP.ResumeLayout(false);
            this.groupBoxEditAP.PerformLayout();
            this.groupBoxUzavreniAP.ResumeLayout(false);
            this.groupBoxUzavreniAP.PerformLayout();
            this.groupBoxZnovuotevreniAP.ResumeLayout(false);
            this.groupBoxZnovuotevreniAP.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonZavrit;
        private System.Windows.Forms.Label labelOdpovedny1;
        private System.Windows.Forms.Label labelOdpovedny2;
        private System.Windows.Forms.ComboBox ComboBoxZadavatel1;
        private System.Windows.Forms.ComboBox ComboBoxZadavatel2;
        private System.Windows.Forms.ComboBox ComboBoxProjekty;
        private System.Windows.Forms.Label labelProjekty;
        private System.Windows.Forms.ComboBox ComboBoxZakaznici;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ButtonUlozit;
        private System.Windows.Forms.Label labelCisloAP;
        private System.Windows.Forms.Label labelNumber;
        private System.Windows.Forms.Button ButtonUkoncitAP;
        private System.Windows.Forms.DateTimePicker dateTimePickerDatumUkonceni;
        private System.Windows.Forms.Label labelPocetTerminu;
        private System.Windows.Forms.Label labelZbyvajiciPocetTerminu;
        private System.Windows.Forms.Button ButtonNovyTermin;
        private System.Windows.Forms.GroupBox groupBoxNovyTermin;
        private System.Windows.Forms.Button ButtonZnovuOtevrit;
        private System.Windows.Forms.GroupBox groupBoxEditAP;
        private System.Windows.Forms.Label labelPoznamkaTermin;
        private System.Windows.Forms.RichTextBox richTextBoxNovaPoznamka;
        private System.Windows.Forms.Label labelDuvodZnovuOtevreni;
        private System.Windows.Forms.RichTextBox richTextBoxDuvodZnovuOtevreni;
        private System.Windows.Forms.Label labelHistorieTerminu;
        private System.Windows.Forms.Label labelDatumUzavreni;
        private System.Windows.Forms.Label labelDatumUzavreniNadpis;
        private System.Windows.Forms.RichTextBox richTextBoxPoznamkaTermin;
        private System.Windows.Forms.GroupBox groupBoxZnovuotevreniAP;
        private System.Windows.Forms.GroupBox groupBoxUzavreniAP;
        private System.Windows.Forms.Label labelNovyTerminProdlouzeni;
    }
}