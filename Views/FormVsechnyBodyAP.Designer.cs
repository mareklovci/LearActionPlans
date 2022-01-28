
namespace LearActionPlans.Views
{
    partial class FormVsechnyBodyAP
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
            this.DataGridViewBodyAP = new System.Windows.Forms.DataGridView();
            this.groupBoxFiltry = new System.Windows.Forms.GroupBox();
            this.ButtonFiltrPricina = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelPricina = new System.Windows.Forms.Label();
            this.ButtonFiltrPopisProblemu = new System.Windows.Forms.Button();
            this.TextBoxFiltrPopisProblemu = new System.Windows.Forms.TextBox();
            this.labelNadrizeni = new System.Windows.Forms.Label();
            this.ComboBoxOdpovedny2 = new System.Windows.Forms.ComboBox();
            this.labelOdpovedny2 = new System.Windows.Forms.Label();
            this.labelOdpovedny1 = new System.Windows.Forms.Label();
            this.ComboBoxOdpovedny1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewBodyAP)).BeginInit();
            this.groupBoxFiltry.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonZavrit
            // 
            this.ButtonZavrit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonZavrit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonZavrit.Location = new System.Drawing.Point(1262, 77);
            this.ButtonZavrit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ButtonZavrit.Name = "ButtonZavrit";
            this.ButtonZavrit.Size = new System.Drawing.Size(110, 30);
            this.ButtonZavrit.TabIndex = 25;
            this.ButtonZavrit.Text = "Close";
            this.ButtonZavrit.UseVisualStyleBackColor = true;
            this.ButtonZavrit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonZavrit_MouseClick);
            // 
            // DataGridViewBodyAP
            // 
            this.DataGridViewBodyAP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGridViewBodyAP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewBodyAP.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DataGridViewBodyAP.Location = new System.Drawing.Point(15, 171);
            this.DataGridViewBodyAP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DataGridViewBodyAP.Name = "DataGridViewBodyAP";
            this.DataGridViewBodyAP.Size = new System.Drawing.Size(1357, 677);
            this.DataGridViewBodyAP.TabIndex = 26;
            this.DataGridViewBodyAP.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewBodyAP_CellMouseClick);
            // 
            // groupBoxFiltry
            // 
            this.groupBoxFiltry.Controls.Add(this.ComboBoxOdpovedny2);
            this.groupBoxFiltry.Controls.Add(this.labelOdpovedny2);
            this.groupBoxFiltry.Controls.Add(this.labelOdpovedny1);
            this.groupBoxFiltry.Controls.Add(this.ComboBoxOdpovedny1);
            this.groupBoxFiltry.Controls.Add(this.ButtonFiltrPricina);
            this.groupBoxFiltry.Controls.Add(this.textBox1);
            this.groupBoxFiltry.Controls.Add(this.labelPricina);
            this.groupBoxFiltry.Controls.Add(this.ButtonFiltrPopisProblemu);
            this.groupBoxFiltry.Controls.Add(this.TextBoxFiltrPopisProblemu);
            this.groupBoxFiltry.Controls.Add(this.labelNadrizeni);
            this.groupBoxFiltry.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.groupBoxFiltry.Location = new System.Drawing.Point(15, 7);
            this.groupBoxFiltry.Name = "groupBoxFiltry";
            this.groupBoxFiltry.Size = new System.Drawing.Size(700, 145);
            this.groupBoxFiltry.TabIndex = 27;
            this.groupBoxFiltry.TabStop = false;
            this.groupBoxFiltry.Text = "Filters";
            // 
            // ButtonFiltrPricina
            // 
            this.ButtonFiltrPricina.Location = new System.Drawing.Point(546, 100);
            this.ButtonFiltrPricina.Name = "ButtonFiltrPricina";
            this.ButtonFiltrPricina.Size = new System.Drawing.Size(110, 30);
            this.ButtonFiltrPricina.TabIndex = 6;
            this.ButtonFiltrPricina.Text = "Filtrovat";
            this.ButtonFiltrPricina.UseVisualStyleBackColor = true;
            this.ButtonFiltrPricina.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonFiltrPricina_MouseClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(290, 100);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(250, 23);
            this.textBox1.TabIndex = 5;
            // 
            // labelPricina
            // 
            this.labelPricina.AutoSize = true;
            this.labelPricina.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelPricina.Location = new System.Drawing.Point(290, 80);
            this.labelPricina.Name = "labelPricina";
            this.labelPricina.Size = new System.Drawing.Size(80, 17);
            this.labelPricina.TabIndex = 4;
            this.labelPricina.Text = "Root cause";
            // 
            // ButtonFiltrPopisProblemu
            // 
            this.ButtonFiltrPopisProblemu.Location = new System.Drawing.Point(546, 40);
            this.ButtonFiltrPopisProblemu.Name = "ButtonFiltrPopisProblemu";
            this.ButtonFiltrPopisProblemu.Size = new System.Drawing.Size(110, 30);
            this.ButtonFiltrPopisProblemu.TabIndex = 3;
            this.ButtonFiltrPopisProblemu.Text = "Filtrovat";
            this.ButtonFiltrPopisProblemu.UseVisualStyleBackColor = true;
            this.ButtonFiltrPopisProblemu.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonFiltrPopisProblemu_MouseClick);
            // 
            // TextBoxFiltrPopisProblemu
            // 
            this.TextBoxFiltrPopisProblemu.Location = new System.Drawing.Point(290, 40);
            this.TextBoxFiltrPopisProblemu.Name = "TextBoxFiltrPopisProblemu";
            this.TextBoxFiltrPopisProblemu.Size = new System.Drawing.Size(250, 23);
            this.TextBoxFiltrPopisProblemu.TabIndex = 2;
            // 
            // labelNadrizeni
            // 
            this.labelNadrizeni.AutoSize = true;
            this.labelNadrizeni.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelNadrizeni.Location = new System.Drawing.Point(290, 20);
            this.labelNadrizeni.Name = "labelNadrizeni";
            this.labelNadrizeni.Size = new System.Drawing.Size(174, 17);
            this.labelNadrizeni.TabIndex = 1;
            this.labelNadrizeni.Text = "Description of the problem";
            // 
            // ComboBoxOdpovedny2
            // 
            this.ComboBoxOdpovedny2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxOdpovedny2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ComboBoxOdpovedny2.FormattingEnabled = true;
            this.ComboBoxOdpovedny2.Location = new System.Drawing.Point(20, 95);
            this.ComboBoxOdpovedny2.Name = "ComboBoxOdpovedny2";
            this.ComboBoxOdpovedny2.Size = new System.Drawing.Size(250, 25);
            this.ComboBoxOdpovedny2.TabIndex = 19;
            // 
            // labelOdpovedny2
            // 
            this.labelOdpovedny2.AutoSize = true;
            this.labelOdpovedny2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelOdpovedny2.Location = new System.Drawing.Point(20, 75);
            this.labelOdpovedny2.Name = "labelOdpovedny2";
            this.labelOdpovedny2.Size = new System.Drawing.Size(106, 17);
            this.labelOdpovedny2.TabIndex = 18;
            this.labelOdpovedny2.Text = "Responsible #2";
            // 
            // labelOdpovedny1
            // 
            this.labelOdpovedny1.AutoSize = true;
            this.labelOdpovedny1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelOdpovedny1.Location = new System.Drawing.Point(20, 20);
            this.labelOdpovedny1.Name = "labelOdpovedny1";
            this.labelOdpovedny1.Size = new System.Drawing.Size(106, 17);
            this.labelOdpovedny1.TabIndex = 17;
            this.labelOdpovedny1.Text = "Responsible #1";
            // 
            // ComboBoxOdpovedny1
            // 
            this.ComboBoxOdpovedny1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxOdpovedny1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ComboBoxOdpovedny1.FormattingEnabled = true;
            this.ComboBoxOdpovedny1.Location = new System.Drawing.Point(20, 40);
            this.ComboBoxOdpovedny1.Name = "ComboBoxOdpovedny1";
            this.ComboBoxOdpovedny1.Size = new System.Drawing.Size(250, 25);
            this.ComboBoxOdpovedny1.TabIndex = 16;
            // 
            // FormVsechnyBodyAP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 861);
            this.Controls.Add(this.groupBoxFiltry);
            this.Controls.Add(this.DataGridViewBodyAP);
            this.Controls.Add(this.ButtonZavrit);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.MinimumSize = new System.Drawing.Size(1400, 900);
            this.Name = "FormVsechnyBodyAP";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormVsechnyBodyAP_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewBodyAP)).EndInit();
            this.groupBoxFiltry.ResumeLayout(false);
            this.groupBoxFiltry.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonZavrit;
        private System.Windows.Forms.DataGridView DataGridViewBodyAP;
        private System.Windows.Forms.GroupBox groupBoxFiltry;
        private System.Windows.Forms.Label labelNadrizeni;
        private System.Windows.Forms.TextBox TextBoxFiltrPopisProblemu;
        private System.Windows.Forms.Button ButtonFiltrPopisProblemu;
        private System.Windows.Forms.Button ButtonFiltrPricina;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label labelPricina;
        private System.Windows.Forms.ComboBox ComboBoxOdpovedny2;
        private System.Windows.Forms.Label labelOdpovedny2;
        private System.Windows.Forms.Label labelOdpovedny1;
        private System.Windows.Forms.ComboBox ComboBoxOdpovedny1;
    }
}