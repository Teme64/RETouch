namespace RETouch
{
    partial class frmVisualizer
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
            this.picBytePlot = new System.Windows.Forms.PictureBox();
            this.lbl0xFF = new System.Windows.Forms.Label();
            this.lbl0x00 = new System.Windows.Forms.Label();
            this.lblVisibleASCII = new System.Windows.Forms.Label();
            this.lblInvisibleASCII = new System.Windows.Forms.Label();
            this.lblNonASCII = new System.Windows.Forms.Label();
            this.ctlColor0xFF = new System.Windows.Forms.Panel();
            this.ctlColor0x00 = new System.Windows.Forms.Panel();
            this.ctlColorVisibleASCII = new System.Windows.Forms.Panel();
            this.ctlColorInvisibleASCII = new System.Windows.Forms.Panel();
            this.ctlColorNonASCII = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picBytePlot)).BeginInit();
            this.SuspendLayout();
            // 
            // picBytePlot
            // 
            this.picBytePlot.BackColor = System.Drawing.SystemColors.Control;
            this.picBytePlot.Location = new System.Drawing.Point(12, 12);
            this.picBytePlot.Name = "picBytePlot";
            this.picBytePlot.Size = new System.Drawing.Size(260, 476);
            this.picBytePlot.TabIndex = 1;
            this.picBytePlot.TabStop = false;
            // 
            // lbl0xFF
            // 
            this.lbl0xFF.AutoSize = true;
            this.lbl0xFF.Location = new System.Drawing.Point(312, 24);
            this.lbl0xFF.Name = "lbl0xFF";
            this.lbl0xFF.Size = new System.Drawing.Size(30, 13);
            this.lbl0xFF.TabIndex = 2;
            this.lbl0xFF.Text = "0xFF";
            // 
            // lbl0x00
            // 
            this.lbl0x00.AutoSize = true;
            this.lbl0x00.Location = new System.Drawing.Point(312, 48);
            this.lbl0x00.Name = "lbl0x00";
            this.lbl0x00.Size = new System.Drawing.Size(30, 13);
            this.lbl0x00.TabIndex = 3;
            this.lbl0x00.Text = "0x00";
            // 
            // lblVisibleASCII
            // 
            this.lblVisibleASCII.AutoSize = true;
            this.lblVisibleASCII.Location = new System.Drawing.Point(312, 72);
            this.lblVisibleASCII.Name = "lblVisibleASCII";
            this.lblVisibleASCII.Size = new System.Drawing.Size(67, 13);
            this.lblVisibleASCII.TabIndex = 4;
            this.lblVisibleASCII.Text = "Visible ASCII";
            // 
            // lblInvisibleASCII
            // 
            this.lblInvisibleASCII.AutoSize = true;
            this.lblInvisibleASCII.Location = new System.Drawing.Point(312, 96);
            this.lblInvisibleASCII.Name = "lblInvisibleASCII";
            this.lblInvisibleASCII.Size = new System.Drawing.Size(75, 13);
            this.lblInvisibleASCII.TabIndex = 5;
            this.lblInvisibleASCII.Text = "Invisible ASCII";
            // 
            // lblNonASCII
            // 
            this.lblNonASCII.AutoSize = true;
            this.lblNonASCII.Location = new System.Drawing.Point(312, 120);
            this.lblNonASCII.Name = "lblNonASCII";
            this.lblNonASCII.Size = new System.Drawing.Size(57, 13);
            this.lblNonASCII.TabIndex = 6;
            this.lblNonASCII.Text = "Non-ASCII";
            // 
            // ctlColor0xFF
            // 
            this.ctlColor0xFF.BackColor = System.Drawing.Color.White;
            this.ctlColor0xFF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctlColor0xFF.Location = new System.Drawing.Point(294, 24);
            this.ctlColor0xFF.Name = "ctlColor0xFF";
            this.ctlColor0xFF.Size = new System.Drawing.Size(11, 14);
            this.ctlColor0xFF.TabIndex = 7;
            // 
            // ctlColor0x00
            // 
            this.ctlColor0x00.BackColor = System.Drawing.Color.Black;
            this.ctlColor0x00.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctlColor0x00.Location = new System.Drawing.Point(294, 48);
            this.ctlColor0x00.Name = "ctlColor0x00";
            this.ctlColor0x00.Size = new System.Drawing.Size(11, 14);
            this.ctlColor0x00.TabIndex = 8;
            // 
            // ctlColorVisibleASCII
            // 
            this.ctlColorVisibleASCII.BackColor = System.Drawing.Color.Blue;
            this.ctlColorVisibleASCII.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctlColorVisibleASCII.Location = new System.Drawing.Point(294, 72);
            this.ctlColorVisibleASCII.Name = "ctlColorVisibleASCII";
            this.ctlColorVisibleASCII.Size = new System.Drawing.Size(11, 14);
            this.ctlColorVisibleASCII.TabIndex = 9;
            // 
            // ctlColorInvisibleASCII
            // 
            this.ctlColorInvisibleASCII.BackColor = System.Drawing.Color.Lime;
            this.ctlColorInvisibleASCII.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctlColorInvisibleASCII.Location = new System.Drawing.Point(294, 96);
            this.ctlColorInvisibleASCII.Name = "ctlColorInvisibleASCII";
            this.ctlColorInvisibleASCII.Size = new System.Drawing.Size(11, 14);
            this.ctlColorInvisibleASCII.TabIndex = 10;
            // 
            // ctlColorNonASCII
            // 
            this.ctlColorNonASCII.BackColor = System.Drawing.Color.Yellow;
            this.ctlColorNonASCII.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctlColorNonASCII.Location = new System.Drawing.Point(294, 120);
            this.ctlColorNonASCII.Name = "ctlColorNonASCII";
            this.ctlColorNonASCII.Size = new System.Drawing.Size(11, 14);
            this.ctlColorNonASCII.TabIndex = 11;
            // 
            // frmVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 501);
            this.Controls.Add(this.ctlColorNonASCII);
            this.Controls.Add(this.ctlColorInvisibleASCII);
            this.Controls.Add(this.ctlColorVisibleASCII);
            this.Controls.Add(this.ctlColor0x00);
            this.Controls.Add(this.ctlColor0xFF);
            this.Controls.Add(this.lblNonASCII);
            this.Controls.Add(this.lblInvisibleASCII);
            this.Controls.Add(this.lblVisibleASCII);
            this.Controls.Add(this.lbl0x00);
            this.Controls.Add(this.lbl0xFF);
            this.Controls.Add(this.picBytePlot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmVisualizer";
            this.Text = "Content Plot";
            ((System.ComponentModel.ISupportInitialize)(this.picBytePlot)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox picBytePlot;
        private System.Windows.Forms.Label lbl0xFF;
        private System.Windows.Forms.Label lbl0x00;
        private System.Windows.Forms.Label lblVisibleASCII;
        private System.Windows.Forms.Label lblInvisibleASCII;
        private System.Windows.Forms.Label lblNonASCII;
        private System.Windows.Forms.Panel ctlColor0xFF;
        private System.Windows.Forms.Panel ctlColor0x00;
        private System.Windows.Forms.Panel ctlColorVisibleASCII;
        private System.Windows.Forms.Panel ctlColorInvisibleASCII;
        private System.Windows.Forms.Panel ctlColorNonASCII;
    }
}