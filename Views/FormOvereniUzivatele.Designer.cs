
namespace LearActionPlans.Views
{
    partial class FormOvereniUzivatele
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
            this.labelLogin = new System.Windows.Forms.Label();
            this.textBoxLogin = new System.Windows.Forms.TextBox();
            this.labelLoginName = new System.Windows.Forms.Label();
            this.labelHeslo = new System.Windows.Forms.Label();
            this.textBoxHeslo = new System.Windows.Forms.TextBox();
            this.ButtonZavrit = new System.Windows.Forms.Button();
            this.ButtonOverit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelLogin
            // 
            this.labelLogin.AutoSize = true;
            this.labelLogin.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelLogin.Location = new System.Drawing.Point(30, 55);
            this.labelLogin.Name = "labelLogin";
            this.labelLogin.Size = new System.Drawing.Size(43, 19);
            this.labelLogin.TabIndex = 0;
            this.labelLogin.Text = "Login";
            // 
            // textBoxLogin
            // 
            this.textBoxLogin.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBoxLogin.Location = new System.Drawing.Point(30, 77);
            this.textBoxLogin.Name = "textBoxLogin";
            this.textBoxLogin.Size = new System.Drawing.Size(250, 26);
            this.textBoxLogin.TabIndex = 1;
            // 
            // labelLoginName
            // 
            this.labelLoginName.AutoSize = true;
            this.labelLoginName.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelLoginName.Location = new System.Drawing.Point(30, 30);
            this.labelLoginName.Name = "labelLoginName";
            this.labelLoginName.Size = new System.Drawing.Size(237, 19);
            this.labelLoginName.TabIndex = 2;
            this.labelLoginName.Text = "Enter your login name and password.";
            // 
            // labelHeslo
            // 
            this.labelHeslo.AutoSize = true;
            this.labelHeslo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelHeslo.Location = new System.Drawing.Point(30, 115);
            this.labelHeslo.Name = "labelHeslo";
            this.labelHeslo.Size = new System.Drawing.Size(67, 19);
            this.labelHeslo.TabIndex = 3;
            this.labelHeslo.Text = "Password";
            // 
            // textBoxHeslo
            // 
            this.textBoxHeslo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBoxHeslo.Location = new System.Drawing.Point(30, 137);
            this.textBoxHeslo.Name = "textBoxHeslo";
            this.textBoxHeslo.PasswordChar = '*';
            this.textBoxHeslo.Size = new System.Drawing.Size(250, 26);
            this.textBoxHeslo.TabIndex = 4;
            // 
            // ButtonZavrit
            // 
            this.ButtonZavrit.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonZavrit.Location = new System.Drawing.Point(312, 269);
            this.ButtonZavrit.Name = "ButtonZavrit";
            this.ButtonZavrit.Size = new System.Drawing.Size(110, 30);
            this.ButtonZavrit.TabIndex = 5;
            this.ButtonZavrit.Text = "Close";
            this.ButtonZavrit.UseVisualStyleBackColor = true;
            this.ButtonZavrit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonZavrit_MouseClick);
            // 
            // ButtonOverit
            // 
            this.ButtonOverit.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonOverit.Location = new System.Drawing.Point(170, 179);
            this.ButtonOverit.Name = "ButtonOverit";
            this.ButtonOverit.Size = new System.Drawing.Size(110, 30);
            this.ButtonOverit.TabIndex = 9;
            this.ButtonOverit.Text = "Verify";
            this.ButtonOverit.UseVisualStyleBackColor = true;
            this.ButtonOverit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonOverit_MouseClick);
            // 
            // FormOvereniUzivatele
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 311);
            this.Controls.Add(this.ButtonOverit);
            this.Controls.Add(this.ButtonZavrit);
            this.Controls.Add(this.textBoxHeslo);
            this.Controls.Add(this.labelHeslo);
            this.Controls.Add(this.labelLoginName);
            this.Controls.Add(this.textBoxLogin);
            this.Controls.Add(this.labelLogin);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.MaximizeBox = false;
            this.Name = "FormOvereniUzivatele";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User authentication";
            this.Load += new System.EventHandler(this.FormOvereniUzivatele_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelLogin;
        private System.Windows.Forms.TextBox textBoxLogin;
        private System.Windows.Forms.Label labelLoginName;
        private System.Windows.Forms.Label labelHeslo;
        private System.Windows.Forms.TextBox textBoxHeslo;
        private System.Windows.Forms.Button ButtonZavrit;
        private System.Windows.Forms.Button ButtonOverit;
    }
}