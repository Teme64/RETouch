using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RETouch
{
    public partial class frmAbout : Form
    {
        //--------------------------------------------------------
        // frmAbout.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // frmAbout Form
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Copyright © 2015-2017 Tmi CaseWare
        //
        // Language: C#/.NET 4.0
        //
        // Version: 1.0.0
        //
        // Created: 13.9.2015 teme64
        //
        // Modified: 8.10.2017 teme64
        //
        // License: GNU GPLv3. See http://www.gnu.org/licenses/gpl.html
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------

        public string viewUrl { get; set; }
        public string viewHTML { get; set; }

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        // Web control
        //private string _thisUrl;
        // private bool _documentLoaded;
        // private bool _documentIsNull;
        //

        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        public frmAbout()
        {
            InitializeComponent();
            //
            InitForm();
        }

        ~frmAbout()
        {
        }

        //--------------------------------------------------------
        // Private procedures
        //--------------------------------------------------------

        // TODO: check for updates, someday in the future
        private void InitForm()
        {
            Font titleFont = new Font("Calibri", 16F, FontStyle.Regular); // Base title font: Calibri 20pt
            // Mid title font: Calibri 14pt
            Font textFont = new Font("Calibri", 9F, FontStyle.Regular); // Base text font: Calibri 12pt
            Font smallTextFont = new Font("Calibri", 7F, FontStyle.Regular); // Small text font: Calibri 10pt
            string productName = Application.ProductName;
            string versionInfo = Application.ProductVersion;
            string copyrightText = "";
            LinkLabel copyrightLink = new LinkLabel();
            string copyrightLinkText = "http://www.gnu.org/licenses/gpl.html";
            string copyrightExceptionsText = "";
            string licensedTo = "";
            string productDetails = "";

            // About boxes' title
            this.Text = "About " + productName;
            // Copyright text
            copyrightText = "This application and its source code is licensed under GNU GPLv3 license" + Environment.NewLine;
            copyrightLink.Text = copyrightLinkText;
            copyrightLink.LinkArea = new LinkArea(0, copyrightLinkText.Length);
            copyrightLink.LinkBehavior = LinkBehavior.AlwaysUnderline;
            copyrightText += "See " + copyrightLink.Text + " for details";
            
            // Copyright exceptions text
            // none

            // LicensedTo
            // none

            // Product details
            // none


            // Assign labels;
            // - product lblProductName
            // - version info lblVersionInfo
            // - copyright (license type, if applicable) lblCopyright
            // - credits and copyright exceptions lblCopyrightExceptions
            // - licensed to (if applicable) lblLicensedTo
            // - product details and links lblProductDetails

            lblProductName.Text = productName;
            lblProductName.Font = titleFont;
            lblVersionInfo.Text = versionInfo;
            lblVersionInfo.Font = textFont;
            lblCopyright.Text = copyrightText;
            lblCopyright.Font = textFont;

            lblCopyrightExceptions.Text = copyrightExceptionsText;
            lblCopyrightExceptions.Font = textFont;
            lblCopyrightExceptions.Visible = false;
            lblLicensedTo.Text = licensedTo;
            lblLicensedTo.Font = textFont;
            lblLicensedTo.Visible = false;
            lblProductDetails.Text = productDetails;
            lblProductDetails.Font = smallTextFont;
            lblProductDetails.Visible = false;

            // Modify label positions
            lblProductName.Top = 16;
            lblVersionInfo.Top = lblProductName.Top + lblProductName.Height + 14;
            lblCopyright.Top = lblVersionInfo.Top + lblVersionInfo.Height + 14;
            lblCopyrightExceptions.Top = lblCopyright.Top + lblCopyright.Height + 14;
            lblLicensedTo.Top = lblCopyrightExceptions.Top + lblCopyrightExceptions.Height + 14;
            lblProductDetails.Top = lblLicensedTo.Top + lblLicensedTo.Height + 14;
            
            // Modify form look
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            // Colors?

            // Attach event handlers
            this.MouseClick += new MouseEventHandler(frmAbout_MouseClick);
            this.KeyDown += new KeyEventHandler(frmAbout_KeyDown);

            // Cleanup
            titleFont = null;
            textFont = null;
            smallTextFont = null;
        }

        //--------------------------------------------------------
        // Event handlers
        //--------------------------------------------------------

        private void frmAbout_MouseClick(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
            this.Close();
        }

        private void frmAbout_KeyDown(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
            this.Close();
        }
        
        //--------------------------------------------------------
        // Public procedures
        //--------------------------------------------------------

    } // Class frmAbout
} // Namespace
