
namespace LearActionPlans.Views
{
    partial class FormAdmin
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
            this.tabControlAdmin = new System.Windows.Forms.TabControl();
            this.tabPageZamestnanci = new System.Windows.Forms.TabPage();
            this.ButtonUlozitZamestnance = new System.Windows.Forms.Button();
            this.groupBoxStavZamestnance = new System.Windows.Forms.GroupBox();
            this.labelMaterskaDovolena = new System.Windows.Forms.Label();
            this.radioButtonOdstranenZam = new System.Windows.Forms.RadioButton();
            this.radioButtonNeaktivniZam = new System.Windows.Forms.RadioButton();
            this.radioButtonAktivniZam = new System.Windows.Forms.RadioButton();
            this.checkBoxAdmin = new System.Windows.Forms.CheckBox();
            this.RadioButtonAktualizaceZamestnance = new System.Windows.Forms.RadioButton();
            this.RadioButtonNovyZamestnanec = new System.Windows.Forms.RadioButton();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.labelEmail = new System.Windows.Forms.Label();
            this.textBoxLogin = new System.Windows.Forms.TextBox();
            this.labelLogin = new System.Windows.Forms.Label();
            this.comboBoxOddeleniZamestnanci = new System.Windows.Forms.ComboBox();
            this.labelOddeleniZamestnanci = new System.Windows.Forms.Label();
            this.textBoxPrijmeni = new System.Windows.Forms.TextBox();
            this.labelPrijmeni = new System.Windows.Forms.Label();
            this.textBoxKrestniJmeno = new System.Windows.Forms.TextBox();
            this.labelSeznamZamestnancu = new System.Windows.Forms.Label();
            this.labelKrestniJmeno = new System.Windows.Forms.Label();
            this.ComboBoxZamestnanci = new System.Windows.Forms.ComboBox();
            this.tabPageOddeleni = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxDepartment = new System.Windows.Forms.TextBox();
            this.labelOddeleni = new System.Windows.Forms.Label();
            this.RadioButtonAktualizaceOddeleni = new System.Windows.Forms.RadioButton();
            this.RadioButtonNoveOddeleni = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tabPageProjekty = new System.Windows.Forms.TabPage();
            this.ButtonUlozitProjekt = new System.Windows.Forms.Button();
            this.groupBoxStavProjektu = new System.Windows.Forms.GroupBox();
            this.radioButtonNeaktivniProjekt = new System.Windows.Forms.RadioButton();
            this.radioButtonAktivniProjekt = new System.Windows.Forms.RadioButton();
            this.RadioButtonAktualizaceProjektu = new System.Windows.Forms.RadioButton();
            this.RadioButtonNovyProjekt = new System.Windows.Forms.RadioButton();
            this.textBoxNazevProjektu = new System.Windows.Forms.TextBox();
            this.labelSeznamProjektu = new System.Windows.Forms.Label();
            this.labelNazevProjektu = new System.Windows.Forms.Label();
            this.ComboBoxProjekty = new System.Windows.Forms.ComboBox();
            this.tabPageZakaznici = new System.Windows.Forms.TabPage();
            this.labelUkonceniPracovniCinnosti = new System.Windows.Forms.Label();
            this.tabControlAdmin.SuspendLayout();
            this.tabPageZamestnanci.SuspendLayout();
            this.groupBoxStavZamestnance.SuspendLayout();
            this.tabPageOddeleni.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageProjekty.SuspendLayout();
            this.groupBoxStavProjektu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonZavrit
            // 
            this.ButtonZavrit.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonZavrit.Location = new System.Drawing.Point(762, 469);
            this.ButtonZavrit.Name = "ButtonZavrit";
            this.ButtonZavrit.Size = new System.Drawing.Size(110, 30);
            this.ButtonZavrit.TabIndex = 0;
            this.ButtonZavrit.Text = "Close";
            this.ButtonZavrit.UseVisualStyleBackColor = true;
            this.ButtonZavrit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonZavrit_MouseClick);
            // 
            // tabControlAdmin
            // 
            this.tabControlAdmin.Controls.Add(this.tabPageZamestnanci);
            this.tabControlAdmin.Controls.Add(this.tabPageOddeleni);
            this.tabControlAdmin.Controls.Add(this.tabPageProjekty);
            this.tabControlAdmin.Controls.Add(this.tabPageZakaznici);
            this.tabControlAdmin.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tabControlAdmin.Location = new System.Drawing.Point(15, 15);
            this.tabControlAdmin.Name = "tabControlAdmin";
            this.tabControlAdmin.SelectedIndex = 0;
            this.tabControlAdmin.Size = new System.Drawing.Size(855, 440);
            this.tabControlAdmin.TabIndex = 1;
            // 
            // tabPageZamestnanci
            // 
            this.tabPageZamestnanci.Controls.Add(this.ButtonUlozitZamestnance);
            this.tabPageZamestnanci.Controls.Add(this.groupBoxStavZamestnance);
            this.tabPageZamestnanci.Controls.Add(this.checkBoxAdmin);
            this.tabPageZamestnanci.Controls.Add(this.RadioButtonAktualizaceZamestnance);
            this.tabPageZamestnanci.Controls.Add(this.RadioButtonNovyZamestnanec);
            this.tabPageZamestnanci.Controls.Add(this.textBoxEmail);
            this.tabPageZamestnanci.Controls.Add(this.labelEmail);
            this.tabPageZamestnanci.Controls.Add(this.textBoxLogin);
            this.tabPageZamestnanci.Controls.Add(this.labelLogin);
            this.tabPageZamestnanci.Controls.Add(this.comboBoxOddeleniZamestnanci);
            this.tabPageZamestnanci.Controls.Add(this.labelOddeleniZamestnanci);
            this.tabPageZamestnanci.Controls.Add(this.textBoxPrijmeni);
            this.tabPageZamestnanci.Controls.Add(this.labelPrijmeni);
            this.tabPageZamestnanci.Controls.Add(this.textBoxKrestniJmeno);
            this.tabPageZamestnanci.Controls.Add(this.labelSeznamZamestnancu);
            this.tabPageZamestnanci.Controls.Add(this.labelKrestniJmeno);
            this.tabPageZamestnanci.Controls.Add(this.ComboBoxZamestnanci);
            this.tabPageZamestnanci.Location = new System.Drawing.Point(4, 28);
            this.tabPageZamestnanci.Name = "tabPageZamestnanci";
            this.tabPageZamestnanci.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageZamestnanci.Size = new System.Drawing.Size(847, 408);
            this.tabPageZamestnanci.TabIndex = 0;
            this.tabPageZamestnanci.Text = "Emploees";
            this.tabPageZamestnanci.UseVisualStyleBackColor = true;
            // 
            // ButtonUlozitZamestnance
            // 
            this.ButtonUlozitZamestnance.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonUlozitZamestnance.Location = new System.Drawing.Point(714, 359);
            this.ButtonUlozitZamestnance.Name = "ButtonUlozitZamestnance";
            this.ButtonUlozitZamestnance.Size = new System.Drawing.Size(110, 30);
            this.ButtonUlozitZamestnance.TabIndex = 2;
            this.ButtonUlozitZamestnance.Text = "Save";
            this.ButtonUlozitZamestnance.UseVisualStyleBackColor = true;
            this.ButtonUlozitZamestnance.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonUlozitZamestnance_MouseClick);
            // 
            // groupBoxStavZamestnance
            // 
            this.groupBoxStavZamestnance.Controls.Add(this.labelUkonceniPracovniCinnosti);
            this.groupBoxStavZamestnance.Controls.Add(this.labelMaterskaDovolena);
            this.groupBoxStavZamestnance.Controls.Add(this.radioButtonOdstranenZam);
            this.groupBoxStavZamestnance.Controls.Add(this.radioButtonNeaktivniZam);
            this.groupBoxStavZamestnance.Controls.Add(this.radioButtonAktivniZam);
            this.groupBoxStavZamestnance.Location = new System.Drawing.Point(570, 130);
            this.groupBoxStavZamestnance.Name = "groupBoxStavZamestnance";
            this.groupBoxStavZamestnance.Size = new System.Drawing.Size(254, 154);
            this.groupBoxStavZamestnance.TabIndex = 15;
            this.groupBoxStavZamestnance.TabStop = false;
            this.groupBoxStavZamestnance.Text = "State";
            // 
            // labelMaterskaDovolena
            // 
            this.labelMaterskaDovolena.AutoSize = true;
            this.labelMaterskaDovolena.Location = new System.Drawing.Point(33, 73);
            this.labelMaterskaDovolena.Name = "labelMaterskaDovolena";
            this.labelMaterskaDovolena.Size = new System.Drawing.Size(104, 19);
            this.labelMaterskaDovolena.TabIndex = 17;
            this.labelMaterskaDovolena.Text = "Maternity leave";
            // 
            // radioButtonOdstranenZam
            // 
            this.radioButtonOdstranenZam.AutoSize = true;
            this.radioButtonOdstranenZam.Location = new System.Drawing.Point(17, 97);
            this.radioButtonOdstranenZam.Name = "radioButtonOdstranenZam";
            this.radioButtonOdstranenZam.Size = new System.Drawing.Size(84, 23);
            this.radioButtonOdstranenZam.TabIndex = 2;
            this.radioButtonOdstranenZam.TabStop = true;
            this.radioButtonOdstranenZam.Text = "Removed";
            this.radioButtonOdstranenZam.UseVisualStyleBackColor = true;
            // 
            // radioButtonNeaktivniZam
            // 
            this.radioButtonNeaktivniZam.AutoSize = true;
            this.radioButtonNeaktivniZam.Location = new System.Drawing.Point(17, 47);
            this.radioButtonNeaktivniZam.Name = "radioButtonNeaktivniZam";
            this.radioButtonNeaktivniZam.Size = new System.Drawing.Size(74, 23);
            this.radioButtonNeaktivniZam.TabIndex = 1;
            this.radioButtonNeaktivniZam.TabStop = true;
            this.radioButtonNeaktivniZam.Text = "Inactive";
            this.radioButtonNeaktivniZam.UseVisualStyleBackColor = true;
            // 
            // radioButtonAktivniZam
            // 
            this.radioButtonAktivniZam.AutoSize = true;
            this.radioButtonAktivniZam.Location = new System.Drawing.Point(17, 22);
            this.radioButtonAktivniZam.Name = "radioButtonAktivniZam";
            this.radioButtonAktivniZam.Size = new System.Drawing.Size(64, 23);
            this.radioButtonAktivniZam.TabIndex = 0;
            this.radioButtonAktivniZam.TabStop = true;
            this.radioButtonAktivniZam.Text = "Active";
            this.radioButtonAktivniZam.UseVisualStyleBackColor = true;
            // 
            // checkBoxAdmin
            // 
            this.checkBoxAdmin.AutoSize = true;
            this.checkBoxAdmin.Location = new System.Drawing.Point(300, 209);
            this.checkBoxAdmin.Name = "checkBoxAdmin";
            this.checkBoxAdmin.Size = new System.Drawing.Size(68, 23);
            this.checkBoxAdmin.TabIndex = 14;
            this.checkBoxAdmin.Text = "Admin";
            this.checkBoxAdmin.UseVisualStyleBackColor = true;
            // 
            // RadioButtonAktualizaceZamestnance
            // 
            this.RadioButtonAktualizaceZamestnance.AutoSize = true;
            this.RadioButtonAktualizaceZamestnance.Location = new System.Drawing.Point(150, 15);
            this.RadioButtonAktualizaceZamestnance.Name = "RadioButtonAktualizaceZamestnance";
            this.RadioButtonAktualizaceZamestnance.Size = new System.Drawing.Size(139, 23);
            this.RadioButtonAktualizaceZamestnance.TabIndex = 13;
            this.RadioButtonAktualizaceZamestnance.TabStop = true;
            this.RadioButtonAktualizaceZamestnance.Text = "Employee updates";
            this.RadioButtonAktualizaceZamestnance.UseVisualStyleBackColor = true;
            this.RadioButtonAktualizaceZamestnance.CheckedChanged += new System.EventHandler(this.RadioButtonAktualizaceZamestnance_CheckedChanged);
            // 
            // RadioButtonNovyZamestnanec
            // 
            this.RadioButtonNovyZamestnanec.AutoSize = true;
            this.RadioButtonNovyZamestnanec.Location = new System.Drawing.Point(25, 15);
            this.RadioButtonNovyZamestnanec.Name = "RadioButtonNovyZamestnanec";
            this.RadioButtonNovyZamestnanec.Size = new System.Drawing.Size(110, 23);
            this.RadioButtonNovyZamestnanec.TabIndex = 12;
            this.RadioButtonNovyZamestnanec.TabStop = true;
            this.RadioButtonNovyZamestnanec.Text = "New emploee";
            this.RadioButtonNovyZamestnanec.UseVisualStyleBackColor = true;
            this.RadioButtonNovyZamestnanec.CheckedChanged += new System.EventHandler(this.RadioButtonNovyZamestnanec_CheckedChanged);
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Location = new System.Drawing.Point(25, 262);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(350, 26);
            this.textBoxEmail.TabIndex = 11;
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(25, 240);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(41, 19);
            this.labelEmail.TabIndex = 10;
            this.labelEmail.Text = "Email";
            // 
            // textBoxLogin
            // 
            this.textBoxLogin.Location = new System.Drawing.Point(25, 207);
            this.textBoxLogin.Name = "textBoxLogin";
            this.textBoxLogin.Size = new System.Drawing.Size(254, 26);
            this.textBoxLogin.TabIndex = 9;
            // 
            // labelLogin
            // 
            this.labelLogin.AutoSize = true;
            this.labelLogin.Location = new System.Drawing.Point(25, 185);
            this.labelLogin.Name = "labelLogin";
            this.labelLogin.Size = new System.Drawing.Size(81, 19);
            this.labelLogin.TabIndex = 8;
            this.labelLogin.Text = "Login name";
            // 
            // comboBoxOddeleniZamestnanci
            // 
            this.comboBoxOddeleniZamestnanci.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOddeleniZamestnanci.FormattingEnabled = true;
            this.comboBoxOddeleniZamestnanci.Location = new System.Drawing.Point(25, 317);
            this.comboBoxOddeleniZamestnanci.Name = "comboBoxOddeleniZamestnanci";
            this.comboBoxOddeleniZamestnanci.Size = new System.Drawing.Size(250, 27);
            this.comboBoxOddeleniZamestnanci.TabIndex = 7;
            // 
            // labelOddeleniZamestnanci
            // 
            this.labelOddeleniZamestnanci.AutoSize = true;
            this.labelOddeleniZamestnanci.Location = new System.Drawing.Point(25, 295);
            this.labelOddeleniZamestnanci.Name = "labelOddeleniZamestnanci";
            this.labelOddeleniZamestnanci.Size = new System.Drawing.Size(83, 19);
            this.labelOddeleniZamestnanci.TabIndex = 6;
            this.labelOddeleniZamestnanci.Text = "Department";
            // 
            // textBoxPrijmeni
            // 
            this.textBoxPrijmeni.Location = new System.Drawing.Point(300, 152);
            this.textBoxPrijmeni.Name = "textBoxPrijmeni";
            this.textBoxPrijmeni.Size = new System.Drawing.Size(254, 26);
            this.textBoxPrijmeni.TabIndex = 5;
            // 
            // labelPrijmeni
            // 
            this.labelPrijmeni.AutoSize = true;
            this.labelPrijmeni.Location = new System.Drawing.Point(300, 130);
            this.labelPrijmeni.Name = "labelPrijmeni";
            this.labelPrijmeni.Size = new System.Drawing.Size(72, 19);
            this.labelPrijmeni.TabIndex = 4;
            this.labelPrijmeni.Text = "Last name";
            // 
            // textBoxKrestniJmeno
            // 
            this.textBoxKrestniJmeno.Location = new System.Drawing.Point(25, 152);
            this.textBoxKrestniJmeno.Name = "textBoxKrestniJmeno";
            this.textBoxKrestniJmeno.Size = new System.Drawing.Size(254, 26);
            this.textBoxKrestniJmeno.TabIndex = 3;
            // 
            // labelSeznamZamestnancu
            // 
            this.labelSeznamZamestnancu.AutoSize = true;
            this.labelSeznamZamestnancu.Location = new System.Drawing.Point(25, 55);
            this.labelSeznamZamestnancu.Name = "labelSeznamZamestnancu";
            this.labelSeznamZamestnancu.Size = new System.Drawing.Size(115, 19);
            this.labelSeznamZamestnancu.TabIndex = 2;
            this.labelSeznamZamestnancu.Text = "List of employees";
            // 
            // labelKrestniJmeno
            // 
            this.labelKrestniJmeno.AutoSize = true;
            this.labelKrestniJmeno.Location = new System.Drawing.Point(25, 130);
            this.labelKrestniJmeno.Name = "labelKrestniJmeno";
            this.labelKrestniJmeno.Size = new System.Drawing.Size(73, 19);
            this.labelKrestniJmeno.TabIndex = 1;
            this.labelKrestniJmeno.Text = "First name";
            // 
            // ComboBoxZamestnanci
            // 
            this.ComboBoxZamestnanci.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ComboBoxZamestnanci.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboBoxZamestnanci.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxZamestnanci.FormattingEnabled = true;
            this.ComboBoxZamestnanci.Location = new System.Drawing.Point(25, 77);
            this.ComboBoxZamestnanci.Name = "ComboBoxZamestnanci";
            this.ComboBoxZamestnanci.Size = new System.Drawing.Size(250, 27);
            this.ComboBoxZamestnanci.TabIndex = 0;
            this.ComboBoxZamestnanci.SelectedIndexChanged += new System.EventHandler(this.ComboBoxZamestnanci_SelectedIndexChanged);
            // 
            // tabPageOddeleni
            // 
            this.tabPageOddeleni.Controls.Add(this.groupBox1);
            this.tabPageOddeleni.Controls.Add(this.button1);
            this.tabPageOddeleni.Controls.Add(this.textBoxDepartment);
            this.tabPageOddeleni.Controls.Add(this.labelOddeleni);
            this.tabPageOddeleni.Controls.Add(this.RadioButtonAktualizaceOddeleni);
            this.tabPageOddeleni.Controls.Add(this.RadioButtonNoveOddeleni);
            this.tabPageOddeleni.Controls.Add(this.label1);
            this.tabPageOddeleni.Controls.Add(this.comboBox1);
            this.tabPageOddeleni.Location = new System.Drawing.Point(4, 28);
            this.tabPageOddeleni.Name = "tabPageOddeleni";
            this.tabPageOddeleni.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOddeleni.Size = new System.Drawing.Size(847, 408);
            this.tabPageOddeleni.TabIndex = 1;
            this.tabPageOddeleni.Text = "Departments";
            this.tabPageOddeleni.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Location = new System.Drawing.Point(305, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(254, 120);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "State";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(17, 72);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(84, 23);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Removed";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(17, 47);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(74, 23);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Inactive";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(17, 22);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(64, 23);
            this.radioButton3.TabIndex = 0;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Active";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.button1.Location = new System.Drawing.Point(169, 200);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 30);
            this.button1.TabIndex = 20;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBoxDepartment
            // 
            this.textBoxDepartment.Location = new System.Drawing.Point(25, 152);
            this.textBoxDepartment.Name = "textBoxDepartment";
            this.textBoxDepartment.Size = new System.Drawing.Size(254, 26);
            this.textBoxDepartment.TabIndex = 19;
            // 
            // labelOddeleni
            // 
            this.labelOddeleni.AutoSize = true;
            this.labelOddeleni.Location = new System.Drawing.Point(25, 130);
            this.labelOddeleni.Name = "labelOddeleni";
            this.labelOddeleni.Size = new System.Drawing.Size(121, 19);
            this.labelOddeleni.TabIndex = 18;
            this.labelOddeleni.Text = "Department name";
            // 
            // RadioButtonAktualizaceOddeleni
            // 
            this.RadioButtonAktualizaceOddeleni.AutoSize = true;
            this.RadioButtonAktualizaceOddeleni.Location = new System.Drawing.Point(160, 15);
            this.RadioButtonAktualizaceOddeleni.Name = "RadioButtonAktualizaceOddeleni";
            this.RadioButtonAktualizaceOddeleni.Size = new System.Drawing.Size(154, 23);
            this.RadioButtonAktualizaceOddeleni.TabIndex = 17;
            this.RadioButtonAktualizaceOddeleni.TabStop = true;
            this.RadioButtonAktualizaceOddeleni.Text = "Department updates";
            this.RadioButtonAktualizaceOddeleni.UseVisualStyleBackColor = true;
            // 
            // RadioButtonNoveOddeleni
            // 
            this.RadioButtonNoveOddeleni.AutoSize = true;
            this.RadioButtonNoveOddeleni.Location = new System.Drawing.Point(25, 15);
            this.RadioButtonNoveOddeleni.Name = "RadioButtonNoveOddeleni";
            this.RadioButtonNoveOddeleni.Size = new System.Drawing.Size(130, 23);
            this.RadioButtonNoveOddeleni.TabIndex = 16;
            this.RadioButtonNoveOddeleni.TabStop = true;
            this.RadioButtonNoveOddeleni.Text = "New department";
            this.RadioButtonNoveOddeleni.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 19);
            this.label1.TabIndex = 15;
            this.label1.Text = "List of departments";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(25, 77);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(250, 27);
            this.comboBox1.TabIndex = 14;
            // 
            // tabPageProjekty
            // 
            this.tabPageProjekty.Controls.Add(this.ButtonUlozitProjekt);
            this.tabPageProjekty.Controls.Add(this.groupBoxStavProjektu);
            this.tabPageProjekty.Controls.Add(this.RadioButtonAktualizaceProjektu);
            this.tabPageProjekty.Controls.Add(this.RadioButtonNovyProjekt);
            this.tabPageProjekty.Controls.Add(this.textBoxNazevProjektu);
            this.tabPageProjekty.Controls.Add(this.labelSeznamProjektu);
            this.tabPageProjekty.Controls.Add(this.labelNazevProjektu);
            this.tabPageProjekty.Controls.Add(this.ComboBoxProjekty);
            this.tabPageProjekty.Location = new System.Drawing.Point(4, 28);
            this.tabPageProjekty.Name = "tabPageProjekty";
            this.tabPageProjekty.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProjekty.Size = new System.Drawing.Size(847, 408);
            this.tabPageProjekty.TabIndex = 2;
            this.tabPageProjekty.Text = "Projects";
            this.tabPageProjekty.UseVisualStyleBackColor = true;
            // 
            // ButtonUlozitProjekt
            // 
            this.ButtonUlozitProjekt.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ButtonUlozitProjekt.Location = new System.Drawing.Point(169, 195);
            this.ButtonUlozitProjekt.Name = "ButtonUlozitProjekt";
            this.ButtonUlozitProjekt.Size = new System.Drawing.Size(110, 30);
            this.ButtonUlozitProjekt.TabIndex = 21;
            this.ButtonUlozitProjekt.Text = "Save";
            this.ButtonUlozitProjekt.UseVisualStyleBackColor = true;
            this.ButtonUlozitProjekt.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonUlozitProjekt_MouseClick);
            // 
            // groupBoxStavProjektu
            // 
            this.groupBoxStavProjektu.Controls.Add(this.radioButtonNeaktivniProjekt);
            this.groupBoxStavProjektu.Controls.Add(this.radioButtonAktivniProjekt);
            this.groupBoxStavProjektu.Location = new System.Drawing.Point(300, 55);
            this.groupBoxStavProjektu.Name = "groupBoxStavProjektu";
            this.groupBoxStavProjektu.Size = new System.Drawing.Size(254, 154);
            this.groupBoxStavProjektu.TabIndex = 20;
            this.groupBoxStavProjektu.TabStop = false;
            this.groupBoxStavProjektu.Text = "State";
            // 
            // radioButtonNeaktivniProjekt
            // 
            this.radioButtonNeaktivniProjekt.AutoSize = true;
            this.radioButtonNeaktivniProjekt.Location = new System.Drawing.Point(17, 47);
            this.radioButtonNeaktivniProjekt.Name = "radioButtonNeaktivniProjekt";
            this.radioButtonNeaktivniProjekt.Size = new System.Drawing.Size(74, 23);
            this.radioButtonNeaktivniProjekt.TabIndex = 1;
            this.radioButtonNeaktivniProjekt.TabStop = true;
            this.radioButtonNeaktivniProjekt.Text = "Inactive";
            this.radioButtonNeaktivniProjekt.UseVisualStyleBackColor = true;
            // 
            // radioButtonAktivniProjekt
            // 
            this.radioButtonAktivniProjekt.AutoSize = true;
            this.radioButtonAktivniProjekt.Location = new System.Drawing.Point(17, 22);
            this.radioButtonAktivniProjekt.Name = "radioButtonAktivniProjekt";
            this.radioButtonAktivniProjekt.Size = new System.Drawing.Size(64, 23);
            this.radioButtonAktivniProjekt.TabIndex = 0;
            this.radioButtonAktivniProjekt.TabStop = true;
            this.radioButtonAktivniProjekt.Text = "Active";
            this.radioButtonAktivniProjekt.UseVisualStyleBackColor = true;
            // 
            // RadioButtonAktualizaceProjektu
            // 
            this.RadioButtonAktualizaceProjektu.AutoSize = true;
            this.RadioButtonAktualizaceProjektu.Location = new System.Drawing.Point(150, 15);
            this.RadioButtonAktualizaceProjektu.Name = "RadioButtonAktualizaceProjektu";
            this.RadioButtonAktualizaceProjektu.Size = new System.Drawing.Size(122, 23);
            this.RadioButtonAktualizaceProjektu.TabIndex = 19;
            this.RadioButtonAktualizaceProjektu.TabStop = true;
            this.RadioButtonAktualizaceProjektu.Text = "Project updates";
            this.RadioButtonAktualizaceProjektu.UseVisualStyleBackColor = true;
            this.RadioButtonAktualizaceProjektu.CheckedChanged += new System.EventHandler(this.RadioButtonAktualizaceProjektu_CheckedChanged);
            // 
            // RadioButtonNovyProjekt
            // 
            this.RadioButtonNovyProjekt.AutoSize = true;
            this.RadioButtonNovyProjekt.Location = new System.Drawing.Point(25, 15);
            this.RadioButtonNovyProjekt.Name = "RadioButtonNovyProjekt";
            this.RadioButtonNovyProjekt.Size = new System.Drawing.Size(100, 23);
            this.RadioButtonNovyProjekt.TabIndex = 18;
            this.RadioButtonNovyProjekt.TabStop = true;
            this.RadioButtonNovyProjekt.Text = "New project";
            this.RadioButtonNovyProjekt.UseVisualStyleBackColor = true;
            this.RadioButtonNovyProjekt.CheckedChanged += new System.EventHandler(this.RadioButtonNovyProjekt_CheckedChanged);
            // 
            // textBoxNazevProjektu
            // 
            this.textBoxNazevProjektu.Location = new System.Drawing.Point(25, 152);
            this.textBoxNazevProjektu.Name = "textBoxNazevProjektu";
            this.textBoxNazevProjektu.Size = new System.Drawing.Size(254, 26);
            this.textBoxNazevProjektu.TabIndex = 17;
            // 
            // labelSeznamProjektu
            // 
            this.labelSeznamProjektu.AutoSize = true;
            this.labelSeznamProjektu.Location = new System.Drawing.Point(25, 55);
            this.labelSeznamProjektu.Name = "labelSeznamProjektu";
            this.labelSeznamProjektu.Size = new System.Drawing.Size(98, 19);
            this.labelSeznamProjektu.TabIndex = 16;
            this.labelSeznamProjektu.Text = "List of projects";
            // 
            // labelNazevProjektu
            // 
            this.labelNazevProjektu.AutoSize = true;
            this.labelNazevProjektu.Location = new System.Drawing.Point(25, 130);
            this.labelNazevProjektu.Name = "labelNazevProjektu";
            this.labelNazevProjektu.Size = new System.Drawing.Size(89, 19);
            this.labelNazevProjektu.TabIndex = 15;
            this.labelNazevProjektu.Text = "Project name";
            // 
            // ComboBoxProjekty
            // 
            this.ComboBoxProjekty.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ComboBoxProjekty.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboBoxProjekty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxProjekty.FormattingEnabled = true;
            this.ComboBoxProjekty.Location = new System.Drawing.Point(25, 77);
            this.ComboBoxProjekty.Name = "ComboBoxProjekty";
            this.ComboBoxProjekty.Size = new System.Drawing.Size(250, 27);
            this.ComboBoxProjekty.TabIndex = 14;
            // 
            // tabPageZakaznici
            // 
            this.tabPageZakaznici.Location = new System.Drawing.Point(4, 28);
            this.tabPageZakaznici.Name = "tabPageZakaznici";
            this.tabPageZakaznici.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageZakaznici.Size = new System.Drawing.Size(847, 408);
            this.tabPageZakaznici.TabIndex = 3;
            this.tabPageZakaznici.Text = "Customers";
            this.tabPageZakaznici.UseVisualStyleBackColor = true;
            // 
            // labelUkonceniPracovniCinnosti
            // 
            this.labelUkonceniPracovniCinnosti.AutoSize = true;
            this.labelUkonceniPracovniCinnosti.Location = new System.Drawing.Point(33, 123);
            this.labelUkonceniPracovniCinnosti.Name = "labelUkonceniPracovniCinnosti";
            this.labelUkonceniPracovniCinnosti.Size = new System.Drawing.Size(104, 19);
            this.labelUkonceniPracovniCinnosti.TabIndex = 18;
            this.labelUkonceniPracovniCinnosti.Text = "Maternity leave";
            // 
            // FormAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 511);
            this.Controls.Add(this.tabControlAdmin);
            this.Controls.Add(this.ButtonZavrit);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 550);
            this.MinimumSize = new System.Drawing.Size(900, 550);
            this.Name = "FormAdmin";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormAdmin_Load);
            this.tabControlAdmin.ResumeLayout(false);
            this.tabPageZamestnanci.ResumeLayout(false);
            this.tabPageZamestnanci.PerformLayout();
            this.groupBoxStavZamestnance.ResumeLayout(false);
            this.groupBoxStavZamestnance.PerformLayout();
            this.tabPageOddeleni.ResumeLayout(false);
            this.tabPageOddeleni.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPageProjekty.ResumeLayout(false);
            this.tabPageProjekty.PerformLayout();
            this.groupBoxStavProjektu.ResumeLayout(false);
            this.groupBoxStavProjektu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonZavrit;
        private System.Windows.Forms.TabControl tabControlAdmin;
        private System.Windows.Forms.TabPage tabPageZamestnanci;
        private System.Windows.Forms.TabPage tabPageOddeleni;
        private System.Windows.Forms.TabPage tabPageProjekty;
        private System.Windows.Forms.TabPage tabPageZakaznici;
        private System.Windows.Forms.ComboBox ComboBoxZamestnanci;
        private System.Windows.Forms.RadioButton RadioButtonAktualizaceZamestnance;
        private System.Windows.Forms.RadioButton RadioButtonNovyZamestnanec;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.TextBox textBoxLogin;
        private System.Windows.Forms.Label labelLogin;
        private System.Windows.Forms.ComboBox comboBoxOddeleniZamestnanci;
        private System.Windows.Forms.Label labelOddeleniZamestnanci;
        private System.Windows.Forms.TextBox textBoxPrijmeni;
        private System.Windows.Forms.Label labelPrijmeni;
        private System.Windows.Forms.TextBox textBoxKrestniJmeno;
        private System.Windows.Forms.Label labelSeznamZamestnancu;
        private System.Windows.Forms.Label labelKrestniJmeno;
        private System.Windows.Forms.CheckBox checkBoxAdmin;
        private System.Windows.Forms.GroupBox groupBoxStavZamestnance;
        private System.Windows.Forms.RadioButton radioButtonNeaktivniZam;
        private System.Windows.Forms.RadioButton radioButtonAktivniZam;
        private System.Windows.Forms.RadioButton radioButtonOdstranenZam;
        private System.Windows.Forms.Button ButtonUlozitZamestnance;
        private System.Windows.Forms.Label labelMaterskaDovolena;
        private System.Windows.Forms.RadioButton RadioButtonAktualizaceOddeleni;
        private System.Windows.Forms.RadioButton RadioButtonNoveOddeleni;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBoxDepartment;
        private System.Windows.Forms.Label labelOddeleni;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton RadioButtonAktualizaceProjektu;
        private System.Windows.Forms.RadioButton RadioButtonNovyProjekt;
        private System.Windows.Forms.TextBox textBoxNazevProjektu;
        private System.Windows.Forms.Label labelSeznamProjektu;
        private System.Windows.Forms.Label labelNazevProjektu;
        private System.Windows.Forms.ComboBox ComboBoxProjekty;
        private System.Windows.Forms.GroupBox groupBoxStavProjektu;
        private System.Windows.Forms.RadioButton radioButtonNeaktivniProjekt;
        private System.Windows.Forms.RadioButton radioButtonAktivniProjekt;
        private System.Windows.Forms.Button ButtonUlozitProjekt;
        private System.Windows.Forms.Label labelUkonceniPracovniCinnosti;
    }
}
