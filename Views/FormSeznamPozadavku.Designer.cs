
namespace LearActionPlans.Views
{
    partial class FormSeznamPozadavku
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
            this.ComboBoxZamestnanci = new System.Windows.Forms.ComboBox();
            this.DataGridViewSeznamPozadavku = new System.Windows.Forms.DataGridView();
            this.ButtonZavrit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewSeznamPozadavku)).BeginInit();
            this.SuspendLayout();
            // 
            // ComboBoxZamestnanci
            // 
            this.ComboBoxZamestnanci.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxZamestnanci.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ComboBoxZamestnanci.FormattingEnabled = true;
            this.ComboBoxZamestnanci.Location = new System.Drawing.Point(12, 12);
            this.ComboBoxZamestnanci.Name = "ComboBoxZamestnanci";
            this.ComboBoxZamestnanci.Size = new System.Drawing.Size(250, 27);
            this.ComboBoxZamestnanci.TabIndex = 0;
            // 
            // DataGridViewSeznamPozadavku
            // 
            this.DataGridViewSeznamPozadavku.AllowUserToAddRows = false;
            this.DataGridViewSeznamPozadavku.AllowUserToDeleteRows = false;
            this.DataGridViewSeznamPozadavku.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGridViewSeznamPozadavku.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewSeznamPozadavku.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DataGridViewSeznamPozadavku.Location = new System.Drawing.Point(12, 313);
            this.DataGridViewSeznamPozadavku.Name = "DataGridViewSeznamPozadavku";
            this.DataGridViewSeznamPozadavku.ReadOnly = true;
            this.DataGridViewSeznamPozadavku.Size = new System.Drawing.Size(1060, 450);
            this.DataGridViewSeznamPozadavku.TabIndex = 3;
            // 
            // ButtonZavrit
            // 
            this.ButtonZavrit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonZavrit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonZavrit.Location = new System.Drawing.Point(962, 769);
            this.ButtonZavrit.Name = "ButtonZavrit";
            this.ButtonZavrit.Size = new System.Drawing.Size(110, 30);
            this.ButtonZavrit.TabIndex = 4;
            this.ButtonZavrit.Text = "Close";
            this.ButtonZavrit.UseVisualStyleBackColor = true;
            this.ButtonZavrit.Click += new System.EventHandler(this.ButtonZavrit_Click);
            this.ButtonZavrit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonClose_MouseClick);
            // 
            // FormSeznamPozadavku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 811);
            this.Controls.Add(this.ButtonZavrit);
            this.Controls.Add(this.DataGridViewSeznamPozadavku);
            this.Controls.Add(this.ComboBoxZamestnanci);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1100, 850);
            this.MinimumSize = new System.Drawing.Size(1100, 850);
            this.Name = "FormSeznamPozadavku";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormSeznamPozadavku_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewSeznamPozadavku)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox ComboBoxZamestnanci;
        private System.Windows.Forms.DataGridView DataGridViewSeznamPozadavku;
        private System.Windows.Forms.Button ButtonZavrit;
    }
}