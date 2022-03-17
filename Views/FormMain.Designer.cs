
namespace LearActionPlans.Views
{
    partial class FormMain
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
            this.ButtonNovyAkcniPlan = new System.Windows.Forms.Button();
            this.ButtonOpravaAkcnihoPlanu = new System.Windows.Forms.Button();
            this.ButtonVsechnyBodyAP = new System.Windows.Forms.Button();
            this.ButtonSeznamZadosti = new System.Windows.Forms.Button();
            this.ButtonAdmin = new System.Windows.Forms.Button();
            this.ButtonLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonNovyAkcniPlan
            // 
            this.ButtonNovyAkcniPlan.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonNovyAkcniPlan.Location = new System.Drawing.Point(12, 65);
            this.ButtonNovyAkcniPlan.Name = "ButtonNovyAkcniPlan";
            this.ButtonNovyAkcniPlan.Size = new System.Drawing.Size(230, 70);
            this.ButtonNovyAkcniPlan.TabIndex = 0;
            this.ButtonNovyAkcniPlan.Text = "New Action Plan";
            this.ButtonNovyAkcniPlan.UseVisualStyleBackColor = true;
            this.ButtonNovyAkcniPlan.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonNovyAkcniPlan_MouseClick);
            // 
            // ButtonOpravaAkcnihoPlanu
            // 
            this.ButtonOpravaAkcnihoPlanu.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonOpravaAkcnihoPlanu.Location = new System.Drawing.Point(248, 65);
            this.ButtonOpravaAkcnihoPlanu.Name = "ButtonOpravaAkcnihoPlanu";
            this.ButtonOpravaAkcnihoPlanu.Size = new System.Drawing.Size(230, 70);
            this.ButtonOpravaAkcnihoPlanu.TabIndex = 1;
            this.ButtonOpravaAkcnihoPlanu.Text = "Edit the Action Plan";
            this.ButtonOpravaAkcnihoPlanu.UseVisualStyleBackColor = true;
            this.ButtonOpravaAkcnihoPlanu.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonOpravaAkcnihoPlanu_MouseClick);
            // 
            // ButtonVsechnyBodyAP
            // 
            this.ButtonVsechnyBodyAP.Enabled = false;
            this.ButtonVsechnyBodyAP.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonVsechnyBodyAP.Location = new System.Drawing.Point(484, 65);
            this.ButtonVsechnyBodyAP.Name = "ButtonVsechnyBodyAP";
            this.ButtonVsechnyBodyAP.Size = new System.Drawing.Size(230, 70);
            this.ButtonVsechnyBodyAP.TabIndex = 2;
            this.ButtonVsechnyBodyAP.Text = "All AP Points";
            this.ButtonVsechnyBodyAP.UseVisualStyleBackColor = true;
            this.ButtonVsechnyBodyAP.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonVsechnyBodyAP_MouseClick);
            // 
            // ButtonSeznamZadosti
            // 
            this.ButtonSeznamZadosti.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonSeznamZadosti.Location = new System.Drawing.Point(484, 141);
            this.ButtonSeznamZadosti.Name = "ButtonSeznamZadosti";
            this.ButtonSeznamZadosti.Size = new System.Drawing.Size(230, 70);
            this.ButtonSeznamZadosti.TabIndex = 3;
            this.ButtonSeznamZadosti.Text = "List of Requests";
            this.ButtonSeznamZadosti.UseVisualStyleBackColor = true;
            this.ButtonSeznamZadosti.Visible = false;
            this.ButtonSeznamZadosti.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonSeznamZadosti_MouseClick);
            // 
            // ButtonAdmin
            // 
            this.ButtonAdmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonAdmin.Location = new System.Drawing.Point(12, 181);
            this.ButtonAdmin.Name = "ButtonAdmin";
            this.ButtonAdmin.Size = new System.Drawing.Size(110, 30);
            this.ButtonAdmin.TabIndex = 4;
            this.ButtonAdmin.Text = "Admin";
            this.ButtonAdmin.UseVisualStyleBackColor = true;
            this.ButtonAdmin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonAdmin_MouseClick);
            // 
            // ButtonLogin
            // 
            this.ButtonLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonLogin.Location = new System.Drawing.Point(604, 12);
            this.ButtonLogin.Name = "ButtonLogin";
            this.ButtonLogin.Size = new System.Drawing.Size(110, 30);
            this.ButtonLogin.TabIndex = 5;
            this.ButtonLogin.Text = "Login";
            this.ButtonLogin.UseVisualStyleBackColor = true;
            this.ButtonLogin.Visible = false;
            this.ButtonLogin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonLogin_MouseClick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 231);
            this.Controls.Add(this.ButtonLogin);
            this.Controls.Add(this.ButtonAdmin);
            this.Controls.Add(this.ButtonSeznamZadosti);
            this.Controls.Add(this.ButtonVsechnyBodyAP);
            this.Controls.Add(this.ButtonOpravaAkcnihoPlanu);
            this.Controls.Add(this.ButtonNovyAkcniPlan);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(745, 270);
            this.MinimumSize = new System.Drawing.Size(745, 270);
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Action plans";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonNovyAkcniPlan;
        private System.Windows.Forms.Button ButtonOpravaAkcnihoPlanu;
        private System.Windows.Forms.Button ButtonVsechnyBodyAP;
        private System.Windows.Forms.Button ButtonSeznamZadosti;
        private System.Windows.Forms.Button ButtonAdmin;
        private System.Windows.Forms.Button ButtonLogin;
    }
}