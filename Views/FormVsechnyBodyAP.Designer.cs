
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
            this.ButtonFiltr = new System.Windows.Forms.Button();
            this.ComboBoxOddeleni = new System.Windows.Forms.ComboBox();
            this.labelOddeleni = new System.Windows.Forms.Label();
            this.ComboBoxOdpovedny2 = new System.Windows.Forms.ComboBox();
            this.labelOdpovedny2 = new System.Windows.Forms.Label();
            this.labelOdpovedny1 = new System.Windows.Forms.Label();
            this.ComboBoxOdpovedny1 = new System.Windows.Forms.ComboBox();
            this.TextBoxOdkazNaNormu = new System.Windows.Forms.TextBox();
            this.labelOdkazNaNormu = new System.Windows.Forms.Label();
            this.TextBoxHodnoceniNeshody = new System.Windows.Forms.TextBox();
            this.labelHodnoceniNeshody = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewBodyAP)).BeginInit();
            this.groupBoxFiltry.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonZavrit
            // 
            this.ButtonZavrit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonZavrit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonZavrit.Location = new System.Drawing.Point(1262, 133);
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
            this.groupBoxFiltry.Controls.Add(this.ButtonFiltr);
            this.groupBoxFiltry.Controls.Add(this.ComboBoxOddeleni);
            this.groupBoxFiltry.Controls.Add(this.labelOddeleni);
            this.groupBoxFiltry.Controls.Add(this.ComboBoxOdpovedny2);
            this.groupBoxFiltry.Controls.Add(this.labelOdpovedny2);
            this.groupBoxFiltry.Controls.Add(this.labelOdpovedny1);
            this.groupBoxFiltry.Controls.Add(this.ComboBoxOdpovedny1);
            this.groupBoxFiltry.Controls.Add(this.TextBoxOdkazNaNormu);
            this.groupBoxFiltry.Controls.Add(this.labelOdkazNaNormu);
            this.groupBoxFiltry.Controls.Add(this.TextBoxHodnoceniNeshody);
            this.groupBoxFiltry.Controls.Add(this.labelHodnoceniNeshody);
            this.groupBoxFiltry.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.groupBoxFiltry.Location = new System.Drawing.Point(15, 7);
            this.groupBoxFiltry.Name = "groupBoxFiltry";
            this.groupBoxFiltry.Size = new System.Drawing.Size(1000, 145);
            this.groupBoxFiltry.TabIndex = 27;
            this.groupBoxFiltry.TabStop = false;
            this.groupBoxFiltry.Text = "Filters";
            // 
            // ButtonFiltr
            // 
            this.ButtonFiltr.Location = new System.Drawing.Point(870, 100);
            this.ButtonFiltr.Name = "ButtonFiltr";
            this.ButtonFiltr.Size = new System.Drawing.Size(110, 30);
            this.ButtonFiltr.TabIndex = 22;
            this.ButtonFiltr.Text = "Filter";
            this.ButtonFiltr.UseVisualStyleBackColor = true;
            this.ButtonFiltr.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonFiltr_MouseClick);
            // 
            // ComboBoxOddeleni
            // 
            this.ComboBoxOddeleni.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxOddeleni.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ComboBoxOddeleni.FormattingEnabled = true;
            this.ComboBoxOddeleni.Location = new System.Drawing.Point(300, 40);
            this.ComboBoxOddeleni.Name = "ComboBoxOddeleni";
            this.ComboBoxOddeleni.Size = new System.Drawing.Size(150, 25);
            this.ComboBoxOddeleni.TabIndex = 21;
            // 
            // labelOddeleni
            // 
            this.labelOddeleni.AutoSize = true;
            this.labelOddeleni.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelOddeleni.Location = new System.Drawing.Point(300, 20);
            this.labelOddeleni.Name = "labelOddeleni";
            this.labelOddeleni.Size = new System.Drawing.Size(82, 17);
            this.labelOddeleni.TabIndex = 20;
            this.labelOddeleni.Text = "Department";
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
            // TextBoxOdkazNaNormu
            // 
            this.TextBoxOdkazNaNormu.Location = new System.Drawing.Point(478, 40);
            this.TextBoxOdkazNaNormu.Name = "TextBoxOdkazNaNormu";
            this.TextBoxOdkazNaNormu.Size = new System.Drawing.Size(150, 23);
            this.TextBoxOdkazNaNormu.TabIndex = 5;
            // 
            // labelOdkazNaNormu
            // 
            this.labelOdkazNaNormu.AutoSize = true;
            this.labelOdkazNaNormu.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelOdkazNaNormu.Location = new System.Drawing.Point(478, 20);
            this.labelOdkazNaNormu.Name = "labelOdkazNaNormu";
            this.labelOdkazNaNormu.Size = new System.Drawing.Size(118, 17);
            this.labelOdkazNaNormu.TabIndex = 4;
            this.labelOdkazNaNormu.Text = "Standard chapter";
            // 
            // TextBoxHodnoceniNeshody
            // 
            this.TextBoxHodnoceniNeshody.Location = new System.Drawing.Point(478, 95);
            this.TextBoxHodnoceniNeshody.Name = "TextBoxHodnoceniNeshody";
            this.TextBoxHodnoceniNeshody.Size = new System.Drawing.Size(150, 23);
            this.TextBoxHodnoceniNeshody.TabIndex = 2;
            // 
            // labelHodnoceniNeshody
            // 
            this.labelHodnoceniNeshody.AutoSize = true;
            this.labelHodnoceniNeshody.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelHodnoceniNeshody.Location = new System.Drawing.Point(478, 75);
            this.labelHodnoceniNeshody.Name = "labelHodnoceniNeshody";
            this.labelHodnoceniNeshody.Size = new System.Drawing.Size(74, 17);
            this.labelHodnoceniNeshody.TabIndex = 1;
            this.labelHodnoceniNeshody.Text = "Evaluation";
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
        private System.Windows.Forms.Label labelHodnoceniNeshody;
        private System.Windows.Forms.TextBox TextBoxHodnoceniNeshody;
        private System.Windows.Forms.TextBox TextBoxOdkazNaNormu;
        private System.Windows.Forms.Label labelOdkazNaNormu;
        private System.Windows.Forms.ComboBox ComboBoxOdpovedny2;
        private System.Windows.Forms.Label labelOdpovedny2;
        private System.Windows.Forms.Label labelOdpovedny1;
        private System.Windows.Forms.ComboBox ComboBoxOdpovedny1;
        private System.Windows.Forms.ComboBox ComboBoxOddeleni;
        private System.Windows.Forms.Label labelOddeleni;
        private System.Windows.Forms.Button ButtonFiltr;
    }
}
