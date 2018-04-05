namespace RETouch
{
    partial class frmOptions
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
            this.cmdApply = new System.Windows.Forms.Button();
            this.fraStringExtract = new System.Windows.Forms.GroupBox();
            this.chkCaseSensitiveStringMatching = new System.Windows.Forms.CheckBox();
            this.chkSearchBOMs = new System.Windows.Forms.CheckBox();
            this.ctlNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.chkSkipUnlikeStrings = new System.Windows.Forms.CheckBox();
            this.lblStringMinLength = new System.Windows.Forms.Label();
            this.fraStringExtract.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctlNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOk
            // 
            this.cmdOk.Location = new System.Drawing.Point(92, 162);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(75, 23);
            this.cmdOk.TabIndex = 0;
            this.cmdOk.Text = "Ok";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(173, 162);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdApply
            // 
            this.cmdApply.Location = new System.Drawing.Point(254, 162);
            this.cmdApply.Name = "cmdApply";
            this.cmdApply.Size = new System.Drawing.Size(75, 23);
            this.cmdApply.TabIndex = 2;
            this.cmdApply.Text = "Apply";
            this.cmdApply.UseVisualStyleBackColor = true;
            this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
            // 
            // fraStringExtract
            // 
            this.fraStringExtract.Controls.Add(this.chkCaseSensitiveStringMatching);
            this.fraStringExtract.Controls.Add(this.chkSearchBOMs);
            this.fraStringExtract.Controls.Add(this.ctlNumericUpDown);
            this.fraStringExtract.Controls.Add(this.chkSkipUnlikeStrings);
            this.fraStringExtract.Controls.Add(this.lblStringMinLength);
            this.fraStringExtract.Location = new System.Drawing.Point(12, 12);
            this.fraStringExtract.Name = "fraStringExtract";
            this.fraStringExtract.Size = new System.Drawing.Size(327, 130);
            this.fraStringExtract.TabIndex = 3;
            this.fraStringExtract.TabStop = false;
            this.fraStringExtract.Text = "String Extract";
            // 
            // chkCaseSensitiveStringMatching
            // 
            this.chkCaseSensitiveStringMatching.AutoSize = true;
            this.chkCaseSensitiveStringMatching.Location = new System.Drawing.Point(23, 99);
            this.chkCaseSensitiveStringMatching.Name = "chkCaseSensitiveStringMatching";
            this.chkCaseSensitiveStringMatching.Size = new System.Drawing.Size(173, 17);
            this.chkCaseSensitiveStringMatching.TabIndex = 4;
            this.chkCaseSensitiveStringMatching.Text = "Case Sensitive String Matching";
            this.chkCaseSensitiveStringMatching.UseVisualStyleBackColor = true;
            // 
            // chkSearchBOMs
            // 
            this.chkSearchBOMs.AutoSize = true;
            this.chkSearchBOMs.Location = new System.Drawing.Point(23, 76);
            this.chkSearchBOMs.Name = "chkSearchBOMs";
            this.chkSearchBOMs.Size = new System.Drawing.Size(92, 17);
            this.chkSearchBOMs.TabIndex = 3;
            this.chkSearchBOMs.Text = "Search BOMs";
            this.chkSearchBOMs.UseVisualStyleBackColor = true;
            // 
            // ctlNumericUpDown
            // 
            this.ctlNumericUpDown.Location = new System.Drawing.Point(140, 25);
            this.ctlNumericUpDown.Name = "ctlNumericUpDown";
            this.ctlNumericUpDown.Size = new System.Drawing.Size(39, 20);
            this.ctlNumericUpDown.TabIndex = 2;
            // 
            // chkSkipUnlikeStrings
            // 
            this.chkSkipUnlikeStrings.AutoSize = true;
            this.chkSkipUnlikeStrings.Location = new System.Drawing.Point(23, 53);
            this.chkSkipUnlikeStrings.Name = "chkSkipUnlikeStrings";
            this.chkSkipUnlikeStrings.Size = new System.Drawing.Size(115, 17);
            this.chkSkipUnlikeStrings.TabIndex = 1;
            this.chkSkipUnlikeStrings.Text = "Skip Unlike Strings";
            this.chkSkipUnlikeStrings.UseVisualStyleBackColor = true;
            // 
            // lblStringMinLength
            // 
            this.lblStringMinLength.AutoSize = true;
            this.lblStringMinLength.Location = new System.Drawing.Point(20, 27);
            this.lblStringMinLength.Name = "lblStringMinLength";
            this.lblStringMinLength.Size = new System.Drawing.Size(114, 13);
            this.lblStringMinLength.TabIndex = 0;
            this.lblStringMinLength.Text = "String Minimum Length";
            // 
            // frmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 205);
            this.Controls.Add(this.fraStringExtract);
            this.Controls.Add(this.cmdApply);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOptions";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.fraStringExtract.ResumeLayout(false);
            this.fraStringExtract.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctlNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdApply;
        private System.Windows.Forms.GroupBox fraStringExtract;
        private System.Windows.Forms.NumericUpDown ctlNumericUpDown;
        private System.Windows.Forms.CheckBox chkSkipUnlikeStrings;
        private System.Windows.Forms.Label lblStringMinLength;
        private System.Windows.Forms.CheckBox chkSearchBOMs;
        private System.Windows.Forms.CheckBox chkCaseSensitiveStringMatching;
    }
}