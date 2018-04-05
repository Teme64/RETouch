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
    public partial class frmViewTable : Form
    {
        //--------------------------------------------------------
        // frmViewTable.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // frmViewTable Form to View Tabular Data
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Copyright © 2017-2018 Tmi CaseWare
        //
        // Language: C#/.NET 4.6
        //
        // Version: 1.0.0
        //
        // Created: 15.10.2017 teme64
        //
        // Modified: 28.1.2018 teme64
        //
        // License: GNU GPLv3. See http://www.gnu.org/licenses/gpl.html
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------

        public MainCode.FORM_VIEW_TABLE ViewTable { get; set; }

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        public frmViewTable()
        {
            InitializeComponent();
            this.Load += frmViewTable_Load;
            this.KeyPreview = true;
            this.KeyDown += frmViewTable_KeyDown;
            //
            lvwTableView.AllowColumnReorder = false;
            //lvwTableView.Columns
            lvwTableView.FullRowSelect = true;
            lvwTableView.HideSelection = false;
            lvwTableView.LabelEdit = false;
            lvwTableView.MultiSelect = false;
            lvwTableView.Scrollable = true;
            lvwTableView.View = View.Details;
        }

        ~frmViewTable()
        {

        }

        //--------------------------------------------------------
        // Private procedures
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Event handlers
        //--------------------------------------------------------

        private void frmViewTable_Load(object sender, EventArgs e)
        {
            ListViewItem newNode;
            string[] strArr;

            // Initialize view
            lvwTableView.Columns.Add("Char", 60);
            lvwTableView.Columns.Add("Dec#", 60);
            lvwTableView.Columns.Add("Hex#", 60);
            //lvwTableView.Columns.Add("Name", 60);
            //lvwTableView.Columns.Add("Ctrl Char", 60);
            strArr = new string[3];
            // Load data
            // Generate data
            for(int i = 0; i < 256; i++)
            {
                strArr[0] = new string(Encoding.ASCII.GetChars(new byte[] { (byte)i }));
                strArr[1] = string.Format("{0:D2}", i);
                strArr[2] = string.Format("{0:X2}", i);
                newNode = new ListViewItem(strArr);
                lvwTableView.Items.Add(newNode);
            }
        }

        private void frmViewTable_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter)
            {
                this.Close();
            }
        }

        //--------------------------------------------------------
        // Public procedures
        //--------------------------------------------------------

    } // Class frmViewTable
} // Namespace
