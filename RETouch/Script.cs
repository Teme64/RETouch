using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace RETouch
{
    public partial class frmScript : Form
    {
        //--------------------------------------------------------
        // frmScript.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Script Selection Form
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Copyright © 2017-2018 Tmi CaseWare
        //
        // Language: C#/.NET 4.6
        //
        // Version: 1.0.0
        //
        // Created: 5.11.2017 teme64
        //
        // Modified: 14.3.2018 teme64
        //
        // License: GNU GPLv3. See http://www.gnu.org/licenses/gpl.html
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------

        public libScripts scriptColl { get; set; }
        public libScriptItem selectedScript { get; set; }
        public MainCode.FORM_RETURN_VALUE FormRetValue { get; internal set; }

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        private string _helpContent;
        private bool _helpVisible;

        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        public frmScript()
        {
            InitializeComponent();
            //
            this.Load += frmScript_Load;
            lstScripts.SelectedIndexChanged += lstScripts_SelectedIndexChanged;
            lstScripts.Font = frmMain._monoSpaceFont;
            txtHelp.Font = frmMain._monoSpaceFont;
        }

        ~frmScript()
        {
        }

        //--------------------------------------------------------
        // Private procedures
        //--------------------------------------------------------

        private void LoadScripts()
        {
            for(int i = 0; i < scriptColl.Count();i++)
            {
                lstScripts.Items.Add(scriptColl.Item(i).ScriptName);
            }
            _helpVisible = false;
            this.Width = 300;
        }

        //--------------------------------------------------------
        // Event handlers
        //--------------------------------------------------------

        private void frmScript_Load(object sender, EventArgs e)
        {
            LoadScripts();
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            string scriptName;

            this.FormRetValue = MainCode.FORM_RETURN_VALUE.CANCEL;
            if (lstScripts.SelectedIndices.Count == 1)
            {
                this.FormRetValue = MainCode.FORM_RETURN_VALUE.OK;
                scriptName = lstScripts.Items[lstScripts.SelectedIndex].ToString();
                this.selectedScript = scriptColl.ItemByName(scriptName);
            }
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.FormRetValue = MainCode.FORM_RETURN_VALUE.CANCEL;
            this.Close();
        }

        private void lstScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            string scriptName;
            libScriptItem currItem;

            if(_helpVisible)
            {
                this.Width = 300;
                txtHelp.Text = "";
                _helpVisible = false;
            }
            if (lstScripts.SelectedIndices.Count == 1)
            {
                scriptName = lstScripts.Items[lstScripts.SelectedIndex].ToString();
                currItem = scriptColl.ItemByName(scriptName);
                if (currItem != null && !string.IsNullOrEmpty(currItem.ScriptHelpFilename))
                {
                    if(File.Exists(currItem.ScriptHelpFilename))
                    {
                        _helpContent = File.ReadAllText(currItem.ScriptHelpFilename);
                        this.Width = 600;
                        txtHelp.Text = _helpContent;
                        _helpVisible = true;
                    }
                }
            }
        }

        //--------------------------------------------------------
        // Public procedures
        //--------------------------------------------------------

    } // Class frmScript
} // Namespace
