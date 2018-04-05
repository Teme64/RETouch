namespace RETouch
{
    partial class frmPromptSplitInput
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOk = new System.Windows.Forms.Button();
            this.chkSplitCharacter = new System.Windows.Forms.CheckBox();
            this.txtSplitCharacter = new System.Windows.Forms.TextBox();
            this.chkWordList = new System.Windows.Forms.CheckBox();
            this.cboWordlist = new System.Windows.Forms.ComboBox();
            this.chkLineLength = new System.Windows.Forms.CheckBox();
            this.txtLineLength = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(187, 226);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOk
            // 
            this.cmdOk.Location = new System.Drawing.Point(97, 226);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(75, 23);
            this.cmdOk.TabIndex = 4;
            this.cmdOk.Text = "Ok";
            this.cmdOk.UseVisualStyleBackColor = true;
            // 
            // chkSplitCharacter
            // 
            this.chkSplitCharacter.AutoSize = true;
            this.chkSplitCharacter.Location = new System.Drawing.Point(10, 12);
            this.chkSplitCharacter.Name = "chkSplitCharacter";
            this.chkSplitCharacter.Size = new System.Drawing.Size(95, 17);
            this.chkSplitCharacter.TabIndex = 6;
            this.chkSplitCharacter.Text = "Split Character";
            this.chkSplitCharacter.UseVisualStyleBackColor = true;
            // 
            // txtSplitCharacter
            // 
            this.txtSplitCharacter.Location = new System.Drawing.Point(113, 12);
            this.txtSplitCharacter.Name = "txtSplitCharacter";
            this.txtSplitCharacter.Size = new System.Drawing.Size(121, 20);
            this.txtSplitCharacter.TabIndex = 7;
            // 
            // chkWordList
            // 
            this.chkWordList.AutoSize = true;
            this.chkWordList.Location = new System.Drawing.Point(10, 38);
            this.chkWordList.Name = "chkWordList";
            this.chkWordList.Size = new System.Drawing.Size(97, 17);
            this.chkWordList.TabIndex = 8;
            this.chkWordList.Text = "Match Wordlist";
            this.chkWordList.UseVisualStyleBackColor = true;
            // 
            // cboWordlist
            // 
            this.cboWordlist.FormattingEnabled = true;
            this.cboWordlist.Location = new System.Drawing.Point(113, 38);
            this.cboWordlist.Name = "cboWordlist";
            this.cboWordlist.Size = new System.Drawing.Size(121, 21);
            this.cboWordlist.TabIndex = 9;
            // 
            // chkLineLength
            // 
            this.chkLineLength.AutoSize = true;
            this.chkLineLength.Location = new System.Drawing.Point(10, 65);
            this.chkLineLength.Name = "chkLineLength";
            this.chkLineLength.Size = new System.Drawing.Size(82, 17);
            this.chkLineLength.TabIndex = 10;
            this.chkLineLength.Text = "Line Length";
            this.chkLineLength.UseVisualStyleBackColor = true;
            // 
            // txtLineLength
            // 
            this.txtLineLength.Location = new System.Drawing.Point(113, 65);
            this.txtLineLength.Name = "txtLineLength";
            this.txtLineLength.Size = new System.Drawing.Size(121, 20);
            this.txtLineLength.TabIndex = 11;
            // 
            // frmPromptSplitInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.txtLineLength);
            this.Controls.Add(this.chkLineLength);
            this.Controls.Add(this.cboWordlist);
            this.Controls.Add(this.chkWordList);
            this.Controls.Add(this.txtSplitCharacter);
            this.Controls.Add(this.chkSplitCharacter);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPromptSplitInput";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Prompt Split Input";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.CheckBox chkSplitCharacter;
        private System.Windows.Forms.TextBox txtSplitCharacter;
        private System.Windows.Forms.CheckBox chkWordList;
        private System.Windows.Forms.ComboBox cboWordlist;
        private System.Windows.Forms.CheckBox chkLineLength;
        private System.Windows.Forms.TextBox txtLineLength;
    }
}