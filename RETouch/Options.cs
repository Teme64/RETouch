using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RETouch
{
    public partial class frmOptions : Form
    {
        //--------------------------------------------------------
        // frmOptions.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // frmOptions UI for User's Settings
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Copyright © 2017-2018 Tmi CaseWare
        //
        // Language: C#/.NET 4.6
        //
        // Version: 1.0.0
        //
        // Created: 8.10.2017 teme64
        //
        // Modified: 1.3.2018 teme64
        //
        // License: GNU GPLv3. See http://www.gnu.org/licenses/gpl.html
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------

        public UserSettings UserSettingsColl { get; set; }

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        private bool _isFormLoaded;
        private bool _isDirty;

        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        public frmOptions()
        {
            InitializeComponent();
            _isFormLoaded = false;
            this.Activated += frmOptions_Activated;
        }

        ~frmOptions()
        {
        }

        //--------------------------------------------------------
        // Private procedures
        //--------------------------------------------------------

        private void InitForm()
        {
            ctlNumericUpDown.Minimum = 2;
            ctlNumericUpDown.Maximum = 10;
            ctlNumericUpDown.Value = UserSettingsColl.StringMinimumLength; // User choice
            ctlNumericUpDown.ValueChanged += OnProperty_Changed;
            chkSkipUnlikeStrings.Checked = UserSettingsColl.StringSkipUnlike;
            chkSkipUnlikeStrings.CheckedChanged += OnProperty_Changed;
            chkSearchBOMs.Checked = UserSettingsColl.StringSearchBOMs;
            chkSearchBOMs.CheckedChanged += OnProperty_Changed;
            chkCaseSensitiveStringMatching.Checked = UserSettingsColl.StringMatchingCaseSensitive;
            chkCaseSensitiveStringMatching.CheckedChanged += OnProperty_Changed;
            //
            cmdApply.Enabled = false;
            _isDirty = false;
        }
        
        //--------------------------------------------------------
        // Event handlers
        //--------------------------------------------------------

        private void frmOptions_Activated(object sender, EventArgs e)
        {
            if(!_isFormLoaded)
            {
                InitForm();
                _isFormLoaded = true;
            }
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            if(_isDirty)
            {
                // Apply changes and exit
                cmdApply_Click(sender, e);
            }
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdApply_Click(object sender, EventArgs e)
        {
            // Validate
            // Copy values
            UserSettingsColl.StringMinimumLength = (int)ctlNumericUpDown.Value;
            UserSettingsColl.StringSkipUnlike = chkSkipUnlikeStrings.Checked;
            UserSettingsColl.StringSearchBOMs = chkSearchBOMs.Checked;
            UserSettingsColl.StringMatchingCaseSensitive = chkCaseSensitiveStringMatching.Checked;
            // Finally
            cmdApply.Enabled = false;
            _isDirty = false;
        }

        private void OnProperty_Changed(object sender, EventArgs e)
        {
            cmdApply.Enabled = true;
            _isDirty = true;
        }

        //--------------------------------------------------------
        // Public procedures
        //--------------------------------------------------------

    } // Class Options
} // Namespace
