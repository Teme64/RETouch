﻿namespace RETouch
{
    partial class frmDistribution
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
            this.picChart = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picChart)).BeginInit();
            this.SuspendLayout();
            // 
            // picChart
            // 
            this.picChart.Location = new System.Drawing.Point(13, 12);
            this.picChart.Name = "picChart";
            this.picChart.Size = new System.Drawing.Size(475, 183);
            this.picChart.TabIndex = 4;
            this.picChart.TabStop = false;
            // 
            // frmDistribution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 257);
            this.Controls.Add(this.picChart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDistribution";
            this.Text = "Distribution";
            ((System.ComponentModel.ISupportInitialize)(this.picChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox picChart;
    }
}