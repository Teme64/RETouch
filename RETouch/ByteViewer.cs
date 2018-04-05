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
    public partial class frmByteViewer : Form
    {
        //--------------------------------------------------------
        // frmByteViewer.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Byte Data Converter and Viewer
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Copyright © 2018 Tmi CaseWare
        //
        // Language: C#/.NET 4.6
        //
        // Version: 1.0.0
        //
        // Created: 22.1.2018 teme64
        //
        // Modified: 30.1.2018 teme64
        //
        // License: GNU GPLv3. See http://www.gnu.org/licenses/gpl.html
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------

        public byte[] InputBytes { get; set; }

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        public frmByteViewer()
        {
            InitializeComponent();
            //
            InitForm();
        }

        ~frmByteViewer()
        {

        }

        //--------------------------------------------------------
        // Private procedures
        //--------------------------------------------------------

        private void InitForm()
        {
            this.Load += frmByteViewer_Load;
            this.KeyPreview = true;
            this.KeyUp += frmByteViewer_KeyUp;
        }

        //--------------------------------------------------------
        // Event handlers
        //--------------------------------------------------------

        private void frmByteViewer_Load(object sender, EventArgs e)
        {
            ConvertBytes();
        }

        private void frmByteViewer_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter || e.KeyCode== Keys.Escape)
            {
                this.Close();
            }
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //--------------------------------------------------------
        // Public procedures
        //--------------------------------------------------------

        /// <summary>
        /// Converts a byte array to values of byte, short, int etc.
        /// </summary>
        public void ConvertBytes()
        {
            byte byteValue;
            ushort shortValue;
            uint intValue;
            ulong longValue;

            if (InputBytes == null) return;
            if(InputBytes.Length < 1) return;
            //
            txtRaw.Text = "";
            foreach(byte b in InputBytes)
            {
                txtRaw.Text += string.Format("{0:X2} ", b);
            }
            //
            if(InputBytes.Length > 0) // One byte values
            {
                byteValue = InputBytes[0];
                //
                txtByte.Text = byteValue.ToString();
                txtSByte.Text =((sbyte)byteValue).ToString();
            }
            if (InputBytes.Length > 1) // Two byte values
            {
                shortValue = InputBytes[0];
                shortValue <<= 8;
                shortValue |= InputBytes[1];
                //
                txtUShort.Text = shortValue.ToString();
                txtShort.Text = ((short)shortValue).ToString();
            }
            if (InputBytes.Length > 3) // Four byte values
            {
                intValue = 0;
                for (int i = 0; i < 4; i++)
                {
                    intValue <<= 8;
                    intValue |= InputBytes[i];
                }
                //
                txtUint.Text = intValue.ToString();
                txtInt.Text = ((int)intValue).ToString();
                //
                txtFloat.Text = BitConverter.ToSingle(InputBytes, 0).ToString();
            }
            if (InputBytes.Length > 7) // Eight byte values
            {
                longValue = 0;
                for (int i = 0; i < 8; i++)
                {
                    longValue <<= 8;
                    longValue |= InputBytes[i];
                }
                //
                txtULong.Text = longValue.ToString();
                txtLong.Text = ((long)longValue).ToString();
                //
                txtDouble.Text = BitConverter.ToDouble(InputBytes, 0).ToString();
                //
                txtDateTime.Text = DateTime.FromBinary((int)longValue).ToString();
            }
            // As String
            txtString.Text = "";
            for (int i = 0; i < InputBytes.Length; i++)
            {
                if(char.IsWhiteSpace((char)InputBytes[i]) || char.IsControl((char)InputBytes[i]))
                {
                    break;
                }
                else
                {
                    txtString.Text += ((char)InputBytes[i]);
                }
            }
        }

    } // Class frmByteViewer
} // Namespace
