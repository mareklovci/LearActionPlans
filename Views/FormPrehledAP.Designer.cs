
namespace LearActionPlans.Views
{
    partial class FormPrehledAp
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
            this.DataGridViewAP = new System.Windows.Forms.DataGridView();
            this.ButtonZavrit = new System.Windows.Forms.Button();
            this.ComboBoxRoky = new System.Windows.Forms.ComboBox();
            this.labelRoky = new System.Windows.Forms.Label();
            this.groupBoxFiltry = new System.Windows.Forms.GroupBox();
            this.ComboBoxOdpovedny2 = new System.Windows.Forms.ComboBox();
            this.labelOdpovedny2 = new System.Windows.Forms.Label();
            this.ComboBoxOtevreneUzavrene = new System.Windows.Forms.ComboBox();
            this.labelOtevreneUzavrene = new System.Windows.Forms.Label();
            this.labelTypAP = new System.Windows.Forms.Label();
            this.ComboBoxTypAP = new System.Windows.Forms.ComboBox();
            this.labelProjekty = new System.Windows.Forms.Label();
            this.ComboBoxProjekty = new System.Windows.Forms.ComboBox();
            this.labelOdpovedny1 = new System.Windows.Forms.Label();
            this.ComboBoxOdpovedny1 = new System.Windows.Forms.ComboBox();
            this.ButtonEditAP = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewAP)).BeginInit();
            this.groupBoxFiltry.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataGridViewAP
            // 
            this.DataGridViewAP.AllowUserToAddRows = false;
            this.DataGridViewAP.AllowUserToDeleteRows = false;
            this.DataGridViewAP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGridViewAP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewAP.Location = new System.Drawing.Point(8, 178);
            this.DataGridViewAP.Name = "DataGridViewAP";
            this.DataGridViewAP.ReadOnly = true;
            this.DataGridViewAP.Size = new System.Drawing.Size(1314, 571);
            this.DataGridViewAP.TabIndex = 2;
            this.DataGridViewAP.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridViewAP_CellFormatting);
            this.DataGridViewAP.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewAP_CellMouseClick);
            // 
            // ButtonZavrit
            // 
            this.ButtonZavrit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonZavrit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonZavrit.Location = new System.Drawing.Point(1212, 142);
            this.ButtonZavrit.Name = "ButtonZavrit";
            this.ButtonZavrit.Size = new System.Drawing.Size(110, 30);
            this.ButtonZavrit.TabIndex = 3;
            this.ButtonZavrit.Text = "Close";
            this.ButtonZavrit.UseVisualStyleBackColor = true;
            this.ButtonZavrit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonClose_MouseClick);
            // 
            // ComboBoxRoky
            // 
            this.ComboBoxRoky.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxRoky.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ComboBoxRoky.FormattingEnabled = true;
            this.ComboBoxRoky.Location = new System.Drawing.Point(25, 40);
            this.ComboBoxRoky.Name = "ComboBoxRoky";
            this.ComboBoxRoky.Size = new System.Drawing.Size(200, 25);
            this.ComboBoxRoky.TabIndex = 4;
            // 
            // labelRoky
            // 
            this.labelRoky.AutoSize = true;
            this.labelRoky.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelRoky.Location = new System.Drawing.Point(25, 20);
            this.labelRoky.Name = "labelRoky";
            this.labelRoky.Size = new System.Drawing.Size(147, 17);
            this.labelRoky.TabIndex = 5;
            this.labelRoky.Text = "Year of creation of AP";
            // 
            // groupBoxFiltry
            // 
            this.groupBoxFiltry.Controls.Add(this.ComboBoxOdpovedny2);
            this.groupBoxFiltry.Controls.Add(this.labelOdpovedny2);
            this.groupBoxFiltry.Controls.Add(this.ComboBoxOtevreneUzavrene);
            this.groupBoxFiltry.Controls.Add(this.labelOtevreneUzavrene);
            this.groupBoxFiltry.Controls.Add(this.labelTypAP);
            this.groupBoxFiltry.Controls.Add(this.ComboBoxTypAP);
            this.groupBoxFiltry.Controls.Add(this.labelProjekty);
            this.groupBoxFiltry.Controls.Add(this.ComboBoxProjekty);
            this.groupBoxFiltry.Controls.Add(this.labelOdpovedny1);
            this.groupBoxFiltry.Controls.Add(this.ComboBoxOdpovedny1);
            this.groupBoxFiltry.Controls.Add(this.labelRoky);
            this.groupBoxFiltry.Controls.Add(this.ComboBoxRoky);
            this.groupBoxFiltry.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.groupBoxFiltry.Location = new System.Drawing.Point(12, 12);
            this.groupBoxFiltry.Name = "groupBoxFiltry";
            this.groupBoxFiltry.Size = new System.Drawing.Size(760, 140);
            this.groupBoxFiltry.TabIndex = 6;
            this.groupBoxFiltry.TabStop = false;
            this.groupBoxFiltry.Text = "Filters";
            // 
            // ComboBoxOdpovedny2
            // 
            this.ComboBoxOdpovedny2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxOdpovedny2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ComboBoxOdpovedny2.FormattingEnabled = true;
            this.ComboBoxOdpovedny2.Location = new System.Drawing.Point(250, 95);
            this.ComboBoxOdpovedny2.Name = "ComboBoxOdpovedny2";
            this.ComboBoxOdpovedny2.Size = new System.Drawing.Size(250, 25);
            this.ComboBoxOdpovedny2.TabIndex = 15;
            // 
            // labelOdpovedny2
            // 
            this.labelOdpovedny2.AutoSize = true;
            this.labelOdpovedny2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelOdpovedny2.Location = new System.Drawing.Point(250, 75);
            this.labelOdpovedny2.Name = "labelOdpovedny2";
            this.labelOdpovedny2.Size = new System.Drawing.Size(106, 17);
            this.labelOdpovedny2.TabIndex = 14;
            this.labelOdpovedny2.Text = "Responsible #2";
            // 
            // ComboBoxOtevreneUzavrene
            // 
            this.ComboBoxOtevreneUzavrene.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxOtevreneUzavrene.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ComboBoxOtevreneUzavrene.FormattingEnabled = true;
            this.ComboBoxOtevreneUzavrene.Location = new System.Drawing.Point(525, 95);
            this.ComboBoxOtevreneUzavrene.Name = "ComboBoxOtevreneUzavrene";
            this.ComboBoxOtevreneUzavrene.Size = new System.Drawing.Size(150, 25);
            this.ComboBoxOtevreneUzavrene.TabIndex = 13;
            // 
            // labelOtevreneUzavrene
            // 
            this.labelOtevreneUzavrene.AutoSize = true;
            this.labelOtevreneUzavrene.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelOtevreneUzavrene.Location = new System.Drawing.Point(525, 75);
            this.labelOtevreneUzavrene.Name = "labelOtevreneUzavrene";
            this.labelOtevreneUzavrene.Size = new System.Drawing.Size(98, 17);
            this.labelOtevreneUzavrene.TabIndex = 12;
            this.labelOtevreneUzavrene.Text = "Open / Closed";
            // 
            // labelTypAP
            // 
            this.labelTypAP.AutoSize = true;
            this.labelTypAP.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelTypAP.Location = new System.Drawing.Point(525, 20);
            this.labelTypAP.Name = "labelTypAP";
            this.labelTypAP.Size = new System.Drawing.Size(62, 17);
            this.labelTypAP.TabIndex = 11;
            this.labelTypAP.Text = "Type AP";
            // 
            // ComboBoxTypAP
            // 
            this.ComboBoxTypAP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxTypAP.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ComboBoxTypAP.FormattingEnabled = true;
            this.ComboBoxTypAP.Location = new System.Drawing.Point(525, 40);
            this.ComboBoxTypAP.Name = "ComboBoxTypAP";
            this.ComboBoxTypAP.Size = new System.Drawing.Size(150, 25);
            this.ComboBoxTypAP.TabIndex = 10;
            // 
            // labelProjekty
            // 
            this.labelProjekty.AutoSize = true;
            this.labelProjekty.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelProjekty.Location = new System.Drawing.Point(25, 75);
            this.labelProjekty.Name = "labelProjekty";
            this.labelProjekty.Size = new System.Drawing.Size(59, 17);
            this.labelProjekty.TabIndex = 9;
            this.labelProjekty.Text = "Projects";
            // 
            // ComboBoxProjekty
            // 
            this.ComboBoxProjekty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxProjekty.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ComboBoxProjekty.FormattingEnabled = true;
            this.ComboBoxProjekty.Location = new System.Drawing.Point(25, 95);
            this.ComboBoxProjekty.Name = "ComboBoxProjekty";
            this.ComboBoxProjekty.Size = new System.Drawing.Size(200, 25);
            this.ComboBoxProjekty.TabIndex = 8;
            // 
            // labelOdpovedny1
            // 
            this.labelOdpovedny1.AutoSize = true;
            this.labelOdpovedny1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelOdpovedny1.Location = new System.Drawing.Point(250, 20);
            this.labelOdpovedny1.Name = "labelOdpovedny1";
            this.labelOdpovedny1.Size = new System.Drawing.Size(106, 17);
            this.labelOdpovedny1.TabIndex = 7;
            this.labelOdpovedny1.Text = "Responsible #1";
            // 
            // ComboBoxOdpovedny1
            // 
            this.ComboBoxOdpovedny1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxOdpovedny1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ComboBoxOdpovedny1.FormattingEnabled = true;
            this.ComboBoxOdpovedny1.Location = new System.Drawing.Point(250, 40);
            this.ComboBoxOdpovedny1.Name = "ComboBoxOdpovedny1";
            this.ComboBoxOdpovedny1.Size = new System.Drawing.Size(250, 25);
            this.ComboBoxOdpovedny1.TabIndex = 6;
            // 
            // ButtonEditAP
            // 
            this.ButtonEditAP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonEditAP.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonEditAP.Location = new System.Drawing.Point(1026, 142);
            this.ButtonEditAP.Name = "ButtonEditAP";
            this.ButtonEditAP.Size = new System.Drawing.Size(180, 30);
            this.ButtonEditAP.TabIndex = 7;
            this.ButtonEditAP.Text = "Update, close, reopen AP";
            this.ButtonEditAP.UseVisualStyleBackColor = true;
            this.ButtonEditAP.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonEditAP_MouseClick);
            // 
            // FormPrehledAP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1334, 761);
            this.Controls.Add(this.ButtonEditAP);
            this.Controls.Add(this.groupBoxFiltry);
            this.Controls.Add(this.ButtonZavrit);
            this.Controls.Add(this.DataGridViewAP);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.MinimumSize = new System.Drawing.Size(1350, 800);
            this.Name = "FormPrehledAp";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Action plans";
            this.Load += new System.EventHandler(this.FormPrehledAP_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewAP)).EndInit();
            this.groupBoxFiltry.ResumeLayout(false);
            this.groupBoxFiltry.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView DataGridViewAP;
        private System.Windows.Forms.Button ButtonZavrit;
        private System.Windows.Forms.ComboBox ComboBoxRoky;
        private System.Windows.Forms.Label labelRoky;
        private System.Windows.Forms.GroupBox groupBoxFiltry;
        private System.Windows.Forms.Label labelOdpovedny1;
        private System.Windows.Forms.ComboBox ComboBoxOdpovedny1;
        private System.Windows.Forms.Label labelProjekty;
        private System.Windows.Forms.ComboBox ComboBoxProjekty;
        private System.Windows.Forms.Label labelTypAP;
        private System.Windows.Forms.ComboBox ComboBoxTypAP;
        private System.Windows.Forms.Button ButtonEditAP;
        private System.Windows.Forms.Label labelOtevreneUzavrene;
        private System.Windows.Forms.ComboBox ComboBoxOtevreneUzavrene;
        private System.Windows.Forms.ComboBox ComboBoxOdpovedny2;
        private System.Windows.Forms.Label labelOdpovedny2;
    }
}