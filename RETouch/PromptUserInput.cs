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
    public partial class frmPromptUserInput : Form
    {
        //--------------------------------------------------------
        // frmPromptUserInput.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Prompt User's Input
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Copyright © 2017-2018 Tmi CaseWare
        //
        // Language: C#/.NET 4.6
        //
        // Version: 1.0.0
        //
        // Created: 7.10.2017 teme64
        //
        // Modified: 12.3.2018 teme64
        //
        // License: GNU GPLv3. See http://www.gnu.org/licenses/gpl.html
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------

        public Font monoSpaceFont { get; set; }

        public List<string> commandHistory { get; set; }
        public MainCode.TRANSFORM_ARG transformArg { get; set;}
        public MainCode.FORM_RETURN_VALUE FormRetValue { get; internal set; }

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        private List<string> _tokens;
        private MainCode.TRANSFORM_ARG _newParam;
        
        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        public frmPromptUserInput()
        {
            InitializeComponent();
            //
            InitForm();
        }

        ~frmPromptUserInput()
        {
        }

        //--------------------------------------------------------
        // Private procedures
        //--------------------------------------------------------

        private void InitForm()
        {
            this.Activated += frmPromptUserInput_Activated;
            //
            //txtUserInput.KeyDown += txtUserInput_KeyDown;
            txtParameter1.Text = "";
            txtParameter2.Text = "";
            //
            _tokens = new List<string>();
            _tokens.Add("xor");
            _tokens.Add("rot");
            _tokens.Add("caesar");
            _tokens.Add("<<");
            _tokens.Add(">>");
            _tokens.Add("*");
            _tokens.Add("+");
            _tokens.Add("-");
            _tokens.Add("replace");
            _tokens.Add("split");

            cboOp.DropDownStyle = ComboBoxStyle.DropDownList;
            cboOp.Items.Add("XOR");
            cboOp.Items.Add("ROT");
            cboOp.Items.Add("CAESAR");
            cboOp.Items.Add("<<");
            cboOp.Items.Add(">>");
            cboOp.Items.Add("*");
            cboOp.Items.Add("+");
            cboOp.Items.Add("-");
            cboOp.Items.Add("Replace");
            cboOp.Items.Add("Split");
            cboOp.SelectedIndexChanged += cboOp_SelectedIndexChanged;

            txtParameter1.Enabled = false;
            txtParameter2.Enabled = false;

            //_tokens.Add("");
            //_tokens.Add("");
            //_tokens.Add("");
            FormRetValue = MainCode.FORM_RETURN_VALUE.CANCEL;
        }

        private void cboOp_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cboOp.SelectedItem.ToString())
            {
                case "XOR":
                    {
                        txtParameter1.Enabled = true;
                        txtParameter2.Enabled = false;
                        break;
                    }
                case "ROT":
                    {
                        txtParameter1.Enabled = true;
                        txtParameter2.Enabled = false;
                        break;
                    }
                case "CAESAR":
                    {
                        txtParameter1.Enabled = true;
                        txtParameter2.Enabled = false;
                        break;
                    }
                case "<<":
                    {
                        txtParameter1.Enabled = true;
                        txtParameter2.Enabled = false;
                        break;
                    }
                case ">>":
                    {
                        txtParameter1.Enabled = true;
                        txtParameter2.Enabled = false;
                        break;
                    }
                case "*":
                    {
                        txtParameter1.Enabled = true;
                        txtParameter2.Enabled = false;
                        break;
                    }
                case "+":
                    {
                        txtParameter1.Enabled = true;
                        txtParameter2.Enabled = false;
                        break;
                    }
                case "-":
                    {
                        txtParameter1.Enabled = true;
                        txtParameter2.Enabled = false;
                        break;
                    }
                case "Replace":
                    {
                        txtParameter1.Enabled = true;
                        txtParameter2.Enabled = true;
                        break;
                    }
                case "Split":
                    {
                        txtParameter1.Enabled = true;
                        txtParameter2.Enabled = false;
                        break;
                    }
                case "Swap":
                    {
                        txtParameter1.Enabled = true;
                        txtParameter2.Enabled = false;
                        break;
                    }
                case "Digraph":
                    {
                        txtParameter1.Enabled = true;
                        txtParameter2.Enabled = false;
                        break;
                    }
            } // switch()
        }

        private MainCode.TRANSFORM_ARG Tokenize(string inputStr)
        {
            string[] strArr;
            byte tempByte;
            //byte tempByte2;
            List<byte> argsList;
            int expectedArgCount;

            inputStr = inputStr.ToLower();
            strArr = inputStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            argsList = new List<byte>();
            _newParam = new MainCode.TRANSFORM_ARG();
            _newParam.alphabet = Alphabet();
            //switch(strArr[i].ToLower().Trim())
            expectedArgCount = 1;
            if(cboOp.SelectedItem == null)
            {
                _newParam.opCode = MainCode.TRANSFORM_OP.NOP;
            }
            else
            { 
                switch ((cboOp.SelectedItem).ToString().ToLower())
                {
                    case "xor":
                        {
                            _newParam.opCode = MainCode.TRANSFORM_OP.Xor;
                            break;
                        }
                    case "rot":
                        {
                            _newParam.opCode = MainCode.TRANSFORM_OP.Rot;
                            break;
                        }
                    case "caesar": 
                        {
                            _newParam.opCode = MainCode.TRANSFORM_OP.Caesar;
                            break;
                        }
                    case "+":
                        {
                            _newParam.opCode = MainCode.TRANSFORM_OP.Plus;
                            break;
                        }
                    case "-":
                        {
                            _newParam.opCode = MainCode.TRANSFORM_OP.Minus;
                            break;
                        }
                    case "shl": // Shift left
                        {
                            _newParam.opCode = MainCode.TRANSFORM_OP.ShiftLeft;
                            break;
                        }
                    case "shr": // Shift right
                        {
                            _newParam.opCode = MainCode.TRANSFORM_OP.ShiftRight;
                            break;
                        }
                    case "replace":
                        {
                            _newParam.opCode = MainCode.TRANSFORM_OP.Replace;
                            expectedArgCount = 2;
                            break;
                        }
                    case "split":
                        {
                            _newParam.opCode = MainCode.TRANSFORM_OP.Replace;
                            expectedArgCount = 1;
                            break;
                        }
                    case "swap":
                        {
                            _newParam.opCode = MainCode.TRANSFORM_OP.Swap;
                            break;
                        }
                    default:
                        {
                            _newParam.opCode = MainCode.TRANSFORM_OP.NOP;
                            break;
                        }
                } // switch()
            }
            if (!string.IsNullOrEmpty(txtParameter1.Text))
            {
                if(txtParameter1.Text.StartsWith("[") && txtParameter1.Text.EndsWith("]") &&
                    txtParameter1.Text.IndexOf(":") > 1)
                { // Has numeric range format
                txtParameter1.Text = txtParameter1.Text.Replace("[", "").Replace("]", "");
                    if (byte.TryParse(txtParameter1.Text.Substring(0, txtParameter1.Text.IndexOf(":")), out tempByte))
                    {
                        argsList.Add(tempByte);
                        if (byte.TryParse(txtParameter1.Text.Substring(txtParameter1.Text.IndexOf(":") + 1,
                            txtParameter1.Text.Length - txtParameter1.Text.IndexOf(":") - 1), out tempByte))
                        {
                            for(byte j = (byte)(argsList[0] + 1); j <= tempByte; j++)
                            {
                                argsList.Add(j);
                            }
                        }
                    }
                }
            }
            if(byte.TryParse(txtParameter1.Text, out tempByte))
            {
                argsList.Add(tempByte);
            }
            else if (!string.IsNullOrEmpty(txtParameter1.Text) && byte.TryParse(Convert.ToInt32(txtParameter1.Text[0]).ToString(), out tempByte)) // Convert char to int
            {
                argsList.Add(tempByte);
            }
            if (expectedArgCount == 2)
            { 
                if (byte.TryParse(txtParameter2.Text, out tempByte))
                {
                    argsList.Add(tempByte);
                }
                else if (!string.IsNullOrEmpty(txtParameter2.Text) && 
                    byte.TryParse(Convert.ToInt32(txtParameter2.Text[0]).ToString(), out tempByte)) // Convert char to int
                {
                    argsList.Add(tempByte);
                }
            }
            _newParam.args = argsList.ToArray();
            //
            return _newParam;
        }

        private MainCode.TRANSFORM_ALPHABET Alphabet()
        {
            MainCode.TRANSFORM_ALPHABET selectedAlpha;

            selectedAlpha = MainCode.TRANSFORM_ALPHABET.DEFAULT;
            if (chk8Bit.Checked)
            {
                selectedAlpha = MainCode.TRANSFORM_ALPHABET.DEFAULT;
            }
            else if(chkASCII.Checked)
            {
                selectedAlpha = MainCode.TRANSFORM_ALPHABET.ASCII;
            }
            else if (chkLetter.Checked)
            {
                selectedAlpha = MainCode.TRANSFORM_ALPHABET.LETTERS;
            }
            else if (chkUpperCaseASCII.Checked)
            {
                selectedAlpha = MainCode.TRANSFORM_ALPHABET.UPPERCASE;
            }
            else if (chkLowerCaseASCII.Checked)
            {
                selectedAlpha = MainCode.TRANSFORM_ALPHABET.LOWERCASE;
            }
            else if (chkDigits.Checked)
            {
                selectedAlpha = MainCode.TRANSFORM_ALPHABET.DIGITS;
            }
            //
            return selectedAlpha;
        }

        //--------------------------------------------------------
        // Event handlers
        //--------------------------------------------------------

        private void frmPromptUserInput_Activated(object sender, EventArgs e)
        {
            txtParameter1.Font = monoSpaceFont;
            txtParameter2.Font = monoSpaceFont;
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            //Tokenize(txtUserInput.Text);
            //this.transformArg = _newParam;
            if(commandHistory == null)
            {
                commandHistory = new List<string>();
            }
            //if (!string.IsNullOrEmpty(txtUserInput.Text)) commandHistory.Add(txtUserInput.Text);
            //this.transformArg = Tokenize(txtUserInput.Text);
            // FIX 20180201:
            this.transformArg = Tokenize("");
            this.FormRetValue = MainCode.FORM_RETURN_VALUE.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.transformArg = new MainCode.TRANSFORM_ARG();
            this.FormRetValue = MainCode.FORM_RETURN_VALUE.CANCEL;
            this.Close();
        }

        //--------------------------------------------------------
        // Public procedures
        //--------------------------------------------------------

    } // Class frmPromptUserInput
} // Namespace
