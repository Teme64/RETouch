namespace RETouch
{
    partial class frmAbout
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
            this.lblProductName = new System.Windows.Forms.Label();
            this.lblVersionInfo = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblCopyrightExceptions = new System.Windows.Forms.Label();
            this.lblLicensedTo = new System.Windows.Forms.Label();
            this.lblProductDetails = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.Location = new System.Drawing.Point(16, 16);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(82, 13);
            this.lblProductName.TabIndex = 0;
            this.lblProductName.Text = "lblProductName";
            // 
            // lblVersionInfo
            // 
            this.lblVersionInfo.AutoSize = true;
            this.lblVersionInfo.Location = new System.Drawing.Point(16, 40);
            this.lblVersionInfo.Name = "lblVersionInfo";
            this.lblVersionInfo.Size = new System.Drawing.Size(70, 13);
            this.lblVersionInfo.TabIndex = 1;
            this.lblVersionInfo.Text = "lblVersionInfo";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Location = new System.Drawing.Point(16, 64);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(61, 13);
            this.lblCopyright.TabIndex = 2;
            this.lblCopyright.Text = "lblCopyright";
            // 
            // lblCopyrightExceptions
            // 
            this.lblCopyrightExceptions.AutoSize = true;
            this.lblCopyrightExceptions.Location = new System.Drawing.Point(16, 88);
            this.lblCopyrightExceptions.Name = "lblCopyrightExceptions";
            this.lblCopyrightExceptions.Size = new System.Drawing.Size(113, 13);
            this.lblCopyrightExceptions.TabIndex = 3;
            this.lblCopyrightExceptions.Text = "lblCopyrightExceptions";
            // 
            // lblLicensedTo
            // 
            this.lblLicensedTo.AutoSize = true;
            this.lblLicensedTo.Location = new System.Drawing.Point(16, 112);
            this.lblLicensedTo.Name = "lblLicensedTo";
            this.lblLicensedTo.Size = new System.Drawing.Size(73, 13);
            this.lblLicensedTo.TabIndex = 4;
            this.lblLicensedTo.Text = "lblLicensedTo";
            // 
            // lblProductDetails
            // 
            this.lblProductDetails.AutoSize = true;
            this.lblProductDetails.Location = new System.Drawing.Point(16, 136);
            this.lblProductDetails.Name = "lblProductDetails";
            this.lblProductDetails.Size = new System.Drawing.Size(86, 13);
            this.lblProductDetails.TabIndex = 5;
            this.lblProductDetails.Text = "lblProductDetails";
            // 
            // frmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 262);
            this.Controls.Add(this.lblProductDetails);
            this.Controls.Add(this.lblLicensedTo);
            this.Controls.Add(this.lblCopyrightExceptions);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.lblVersionInfo);
            this.Controls.Add(this.lblProductName);
            this.Name = "frmAbout";
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.Label lblVersionInfo;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblCopyrightExceptions;
        private System.Windows.Forms.Label lblLicensedTo;
        private System.Windows.Forms.Label lblProductDetails;
    }
}