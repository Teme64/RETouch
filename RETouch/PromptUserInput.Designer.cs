namespace RETouch
{
    partial class frmPromptUserInput
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
            this.cmdOk = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.chk8Bit = new System.Windows.Forms.CheckBox();
            this.chkLowerCaseASCII = new System.Windows.Forms.CheckBox();
            this.chkUpperCaseASCII = new System.Windows.Forms.CheckBox();
            this.chkDigits = new System.Windows.Forms.CheckBox();
            this.chkASCII = new System.Windows.Forms.CheckBox();
            this.fraAlphabet = new System.Windows.Forms.GroupBox();
            this.chkLetter = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboOp = new System.Windows.Forms.ComboBox();
            this.lblSelect = new System.Windows.Forms.Label();
            this.lblParameter1 = new System.Windows.Forms.Label();
            this.lblParameter = new System.Windows.Forms.Label();
            this.txtParameter1 = new System.Windows.Forms.TextBox();
            this.txtParameter2 = new System.Windows.Forms.TextBox();
            this.fraAlphabet.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOk
            // 
            this.cmdOk.Location = new System.Drawing.Point(126, 321);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(75, 23);
            this.cmdOk.TabIndex = 2;
            this.cmdOk.Text = "Ok";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(216, 321);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 3;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // chk8Bit
            // 
            this.chk8Bit.AutoSize = true;
            this.chk8Bit.Location = new System.Drawing.Point(17, 25);
            this.chk8Bit.Name = "chk8Bit";
            this.chk8Bit.Size = new System.Drawing.Size(47, 17);
            this.chk8Bit.TabIndex = 5;
            this.chk8Bit.Text = "8 Bit";
            this.chk8Bit.UseVisualStyleBackColor = true;
            // 
            // chkLowerCaseASCII
            // 
            this.chkLowerCaseASCII.AutoSize = true;
            this.chkLowerCaseASCII.Location = new System.Drawing.Point(17, 48);
            this.chkLowerCaseASCII.Name = "chkLowerCaseASCII";
            this.chkLowerCaseASCII.Size = new System.Drawing.Size(108, 17);
            this.chkLowerCaseASCII.TabIndex = 6;
            this.chkLowerCaseASCII.Text = "Lowercase ASCII";
            this.chkLowerCaseASCII.UseVisualStyleBackColor = true;
            // 
            // chkUpperCaseASCII
            // 
            this.chkUpperCaseASCII.AutoSize = true;
            this.chkUpperCaseASCII.Location = new System.Drawing.Point(17, 71);
            this.chkUpperCaseASCII.Name = "chkUpperCaseASCII";
            this.chkUpperCaseASCII.Size = new System.Drawing.Size(108, 17);
            this.chkUpperCaseASCII.TabIndex = 7;
            this.chkUpperCaseASCII.Text = "Uppercase ASCII";
            this.chkUpperCaseASCII.UseVisualStyleBackColor = true;
            // 
            // chkDigits
            // 
            this.chkDigits.AutoSize = true;
            this.chkDigits.Location = new System.Drawing.Point(17, 118);
            this.chkDigits.Name = "chkDigits";
            this.chkDigits.Size = new System.Drawing.Size(52, 17);
            this.chkDigits.TabIndex = 8;
            this.chkDigits.Text = "Digits";
            this.chkDigits.UseVisualStyleBackColor = true;
            // 
            // chkASCII
            // 
            this.chkASCII.AutoSize = true;
            this.chkASCII.Location = new System.Drawing.Point(17, 141);
            this.chkASCII.Name = "chkASCII";
            this.chkASCII.Size = new System.Drawing.Size(53, 17);
            this.chkASCII.TabIndex = 9;
            this.chkASCII.Text = "ASCII";
            this.chkASCII.UseVisualStyleBackColor = true;
            // 
            // fraAlphabet
            // 
            this.fraAlphabet.Controls.Add(this.chkLetter);
            this.fraAlphabet.Controls.Add(this.chk8Bit);
            this.fraAlphabet.Controls.Add(this.chkASCII);
            this.fraAlphabet.Controls.Add(this.chkUpperCaseASCII);
            this.fraAlphabet.Controls.Add(this.chkDigits);
            this.fraAlphabet.Controls.Add(this.chkLowerCaseASCII);
            this.fraAlphabet.Location = new System.Drawing.Point(12, 139);
            this.fraAlphabet.Name = "fraAlphabet";
            this.fraAlphabet.Size = new System.Drawing.Size(305, 166);
            this.fraAlphabet.TabIndex = 10;
            this.fraAlphabet.TabStop = false;
            this.fraAlphabet.Text = "Alphabet";
            // 
            // chkLetter
            // 
            this.chkLetter.AutoSize = true;
            this.chkLetter.Location = new System.Drawing.Point(17, 95);
            this.chkLetter.Name = "chkLetter";
            this.chkLetter.Size = new System.Drawing.Size(53, 17);
            this.chkLetter.TabIndex = 10;
            this.chkLetter.Text = "Letter";
            this.chkLetter.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtParameter2);
            this.groupBox1.Controls.Add(this.txtParameter1);
            this.groupBox1.Controls.Add(this.lblParameter);
            this.groupBox1.Controls.Add(this.lblParameter1);
            this.groupBox1.Controls.Add(this.lblSelect);
            this.groupBox1.Controls.Add(this.cboOp);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(305, 121);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Transformation";
            // 
            // cboOp
            // 
            this.cboOp.FormattingEnabled = true;
            this.cboOp.Location = new System.Drawing.Point(57, 34);
            this.cboOp.Name = "cboOp";
            this.cboOp.Size = new System.Drawing.Size(86, 21);
            this.cboOp.TabIndex = 0;
            // 
            // lblSelect
            // 
            this.lblSelect.AutoSize = true;
            this.lblSelect.Location = new System.Drawing.Point(14, 34);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(37, 13);
            this.lblSelect.TabIndex = 1;
            this.lblSelect.Text = "Select";
            // 
            // lblParameter1
            // 
            this.lblParameter1.AutoSize = true;
            this.lblParameter1.Location = new System.Drawing.Point(158, 19);
            this.lblParameter1.Name = "lblParameter1";
            this.lblParameter1.Size = new System.Drawing.Size(64, 13);
            this.lblParameter1.TabIndex = 2;
            this.lblParameter1.Text = "Parameter 1";
            // 
            // lblParameter
            // 
            this.lblParameter.AutoSize = true;
            this.lblParameter.Location = new System.Drawing.Point(228, 19);
            this.lblParameter.Name = "lblParameter";
            this.lblParameter.Size = new System.Drawing.Size(64, 13);
            this.lblParameter.TabIndex = 3;
            this.lblParameter.Text = "Parameter 2";
            // 
            // txtParameter1
            // 
            this.txtParameter1.Location = new System.Drawing.Point(161, 35);
            this.txtParameter1.Name = "txtParameter1";
            this.txtParameter1.Size = new System.Drawing.Size(61, 20);
            this.txtParameter1.TabIndex = 4;
            // 
            // txtParameter2
            // 
            this.txtParameter2.Location = new System.Drawing.Point(228, 35);
            this.txtParameter2.Name = "txtParameter2";
            this.txtParameter2.Size = new System.Drawing.Size(61, 20);
            this.txtParameter2.TabIndex = 5;
            // 
            // frmPromptUserInput
            // 
            this.AcceptButton = this.cmdOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(328, 356);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.fraAlphabet);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPromptUserInput";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Prompt User Input";
            this.fraAlphabet.ResumeLayout(false);
            this.fraAlphabet.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.CheckBox chk8Bit;
        private System.Windows.Forms.CheckBox chkLowerCaseASCII;
        private System.Windows.Forms.CheckBox chkUpperCaseASCII;
        private System.Windows.Forms.CheckBox chkDigits;
        private System.Windows.Forms.CheckBox chkASCII;
        private System.Windows.Forms.GroupBox fraAlphabet;
        private System.Windows.Forms.CheckBox chkLetter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtParameter2;
        private System.Windows.Forms.TextBox txtParameter1;
        private System.Windows.Forms.Label lblParameter;
        private System.Windows.Forms.Label lblParameter1;
        private System.Windows.Forms.Label lblSelect;
        private System.Windows.Forms.ComboBox cboOp;
    }
}