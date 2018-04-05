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
using System.Web;
using CaseWare;

namespace RETouch
{
    public partial class frmMain : Form
    {
        //--------------------------------------------------------
        // frmMain.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Main Form
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Copyright © 2017-2018 Tmi CaseWare
        //
        // Language: C#/.NET 4.6
        //
        // Version: 1.0.0
        //
        // Created: 23.9.2017 teme64
        //
        // Modified: 12.3.2018 teme64
        //
        // License: GNU GPLv3. See http://www.gnu.org/licenses/gpl.html
        //--------------------------------------------------------

        // TODO: String matching should be case-insensitive and check also partial matches
        // TODO: Open/Save/Save As... Workspace saving and restoring
        // TODO: Data files as resources?

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------

        public byte[] gSrcBuffer;
        public byte[] gDestBuffer;
        //
        public static Font _monoSpaceFont;
        public static Color _normalBackColor = Color.White;
        public static Color _warningBackColor = Color.FromArgb(0xF0, 0x80, 0x80);

        // Paths
        public static string appDataPathRoot;
        public static string appDataPathScripts;

        public string _lastDataFilename;
        public string _lastDataPath;
        public string _argFilename;

        // New UX
        public libClipCollection oClipColl;
        //
        public int _lastMenuState;

        // Set hex viewer as static and global so every form can reuse it
        public static CaseWare.libTextViewer oTextViewer;

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        private List<MainCode.SupportedApplications> _builtinSupportedApps;
        // Lists
        private string cfgFilename = @"D:\CSharp-NET46\ReTouch\RETouch\RETouch\Data\RETouch.cfg";
        private Dictionary<string, string> _apiDictionary;
        private List<string> _wordList;
        private List<string> _redWordList;
        //private string[] scriptFilenames;
        private Dictionary<string, string> _scriptDictionary;
        private Dictionary<string, string> _documentDictionary;
        private libScripts _scriptCollection;

        private UserSettings _Settings;
        private string _settingsFilename;
        private bool _settingsIsDirty;
        //
        private MainCode.TEXT_ENCODING _encoding; // Redundant, preferred to use _encodingCurrent
        private Encoding _encodingCurrent;
        // Workspace
        private string _lastFilename;
        private string _lastPath;
        private bool _isDirty; // Flag to save workspace

        private enum STATUS_STRIP_INDEX : int
        {
            STATE = 0,
            IMP_EXP_INFO = 1,
            DEC_HEX_DISPLAY = 2,
            HISTORY_TITLE = 3,
            TEXT_ENCODING = 4,
            BYTE_BUFFER_STATUS = 5,
            SRC_INFO = 6
        }

        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        public frmMain()
        {
            InitializeComponent();
            //
            InitForm();
        }

        ~frmMain()
        {
        }

        //--------------------------------------------------------
        // Private procedures
        //--------------------------------------------------------

        private void InitForm()
        {
            ToolStripItem tsItem;
            string menuName;
            MainCode.SupportedApplications calcEXE;

            this.Icon = RETouch.Properties.Resources.retouch;
            this.FormClosing += frmMain_FormClosing;
            this.KeyPreview = true;
            this.KeyDown += frmMain_KeyDown;
            //
            _Settings = new UserSettings();
            _settingsIsDirty = false;
            //_settingsFilename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            appDataPathRoot = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            appDataPathRoot = Path.Combine(appDataPathRoot, "RETouch");
            if (!Directory.Exists(appDataPathRoot))
            {
                Directory.CreateDirectory(appDataPathRoot);
            }
            appDataPathScripts = Path.Combine(appDataPathRoot, "Scripts");
            if (!Directory.Exists(appDataPathScripts))
            {
                Directory.CreateDirectory(appDataPathScripts);
            }
            if (!File.Exists(Path.Combine(appDataPathScripts, "script.cfg")))
            {
                // Create file
                File.WriteAllText(Path.Combine(appDataPathScripts, "script.cfg"), "");
            }
            // Script config
            _scriptCollection = new libScripts();
            MainCode.LoadScriptConfig(Path.Combine(appDataPathScripts, "script.cfg"), appDataPathScripts, _scriptCollection);
            //
            _settingsFilename = Path.Combine(appDataPathRoot, "userdata.cfg");
            if(!File.Exists(_settingsFilename))
            {
                _settingsIsDirty = true;
            }
            MainCode.LoadUserSettings(_settingsFilename, out _Settings);
            //
            cfgFilename = Path.Combine(appDataPathRoot, "RETouch.cfg");
            if (!File.Exists(cfgFilename))
            {
                // Create file
                File.WriteAllText(cfgFilename, "");
            }
            _builtinSupportedApps = MainCode.GetSupportedApplications(cfgFilename);
            // Insert WinCalc to list
            if(File.Exists(@"%SYSTEM32%\calc.exe"))
            { 
                calcEXE = new MainCode.SupportedApplications();
                calcEXE.appExecutable = @"%SYSTEM32%\calc.exe";
                calcEXE.appName = "Calculator";
                calcEXE.arguments = "";
                _builtinSupportedApps.Insert(0, calcEXE);
            }
            //
            for (int i = 0; i < _builtinSupportedApps.Count; i++)
            {
                menuName = _builtinSupportedApps.ElementAt(i).appName;
                // Clean up text
                menuName.Replace("&", "");
                mnuExternal.DropDownItems.Add(menuName, null, mnuExternalApplication_Click);
            }
            //
            _encoding = MainCode.TEXT_ENCODING.Default;
            _monoSpaceFont = new Font("Consolas", 9F, FontStyle.Regular);
            //
            // Notice: ctlToolImageList is public so images can be used outside main form
            ctlToolImageList.Images.Add("Back", RETouch.Properties.Resources.back);
            ctlToolImageList.Images.Add("Forward", RETouch.Properties.Resources.forward);
            ctlToolImageList.Images.Add("Copy", RETouch.Properties.Resources.copy);
            ctlToolImageList.Images.Add("CopyBinary", RETouch.Properties.Resources.copybinary);
            ctlToolImageList.Images.Add("Cut", RETouch.Properties.Resources.cut);
            ctlToolImageList.Images.Add("Paste", RETouch.Properties.Resources.paste);
            ctlToolImageList.Images.Add("Clear", RETouch.Properties.Resources.clear);
            ctlToolImageList.Images.Add("Numbers", RETouch.Properties.Resources.numbers);
            ctlToolImageList.Images.Add("Encoding", RETouch.Properties.Resources.encoding); // String encoding
            ctlToolImageList.Images.Add("Checksum", RETouch.Properties.Resources.checksum);
            ctlToolImageList.Images.Add("Strings", RETouch.Properties.Resources.strings);
            ctlToolImageList.Images.Add("MoveUp", RETouch.Properties.Resources.MoveUp);
            ctlToolImageList.Images.Add("Help", RETouch.Properties.Resources.help);
            MainCode.FillToolStrip(ctlToolStripMain, ctlToolImageList, OnToolStrip_Click);
            //
            tsItem = new ToolStripMenuItem();
            tsItem.Name = "Copy";
            tsItem.Text = "Copy";
            tsItem.Click += OnContextMenu_Click;
            ctlResultBoxContextMenu.Items.Add(tsItem);
            tsItem = new ToolStripMenuItem();
            tsItem.Name = "Cut";
            tsItem.Text = "Cut";
            tsItem.Click += OnContextMenu_Click;
            ctlResultBoxContextMenu.Items.Add(tsItem);
            tsItem = new ToolStripMenuItem();
            tsItem.Name = "Paste";
            tsItem.Text = "Paste";
            tsItem.Click += OnContextMenu_Click;
            ctlResultBoxContextMenu.Items.Add(tsItem);
            txtSource.ContextMenuStrip = ctlResultBoxContextMenu;
            // Set table layout
            ctlTableLayout.Dock = DockStyle.Fill;
            //
            txtSource.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            txtSource.Font = _monoSpaceFont;
            txtSource.SelectionChanged += OnTextChanged_Click;
            //
            lstClip.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            lstClip.Font = _monoSpaceFont;
            lstClip.SelectedIndexChanged += lstClip_SelectedIndexChanged;
            oClipColl = new libClipCollection();
            //
            txtResult.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            txtResult.Font = _monoSpaceFont;
            ctlTableLayout.SetColumnSpan(txtResult, 2);
            //
            gSrcBuffer = new byte[0];
            gDestBuffer = new byte[0];
            _lastDataFilename = "";
            _lastDataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            _lastFilename = "";
            _lastPath = appDataPathRoot;
            _argFilename = "";
            oTextViewer = new CaseWare.libTextViewer();
            oTextViewer.SetFormData(this.Icon);
            //oTextViewer.setGetFileFunction()
            oTextViewer.SetOkButtonCaption("Close");
            //
            SetTitleText("");
            SetStatusText("Ready", (int)STATUS_STRIP_INDEX.STATE);
            SetStatusText("No imports", (int)STATUS_STRIP_INDEX.IMP_EXP_INFO);
            SetStatusText(_Settings.DisplayNumberFormatDecimal ? "Decimal" : "Hex", (int)STATUS_STRIP_INDEX.DEC_HEX_DISPLAY);
            SetStatusText("", (int)STATUS_STRIP_INDEX.HISTORY_TITLE);
            SetStatusText("Encoding: Default", (int)STATUS_STRIP_INDEX.TEXT_ENCODING);
            SetStatusText("Buffer is empty", (int)STATUS_STRIP_INDEX.BYTE_BUFFER_STATUS);
            SetMenuState(0);
            _isDirty = false;
        }

        #region // Status and state handlers

        private void SetMenuState(int stateIndex)
        {
            _lastMenuState = stateIndex;
            if (stateIndex == 0)
            {
                mnuEditCopy.Enabled = true;
                mnuEditCopyAsBinary.Enabled = true;
                mnuEditCut.Enabled = true;
                mnuEditPaste.Enabled = true;
                mnuEditPasteSpecialText.Enabled = true;
                mnuEditPasteSpecialBinary.Enabled = true;
                //
                mnuToolsStrings.Enabled = true;
                mnuToolsChecksums.Enabled = false;
                mnuToolsEntropy.Enabled = false;
                mnuToolsFileInfo.Enabled = false;
                mnuExternal.Enabled = false;
                ctlToolStripMain.Items["Copy"].Enabled = true;
                ctlToolStripMain.Items["CopyBinary"].Enabled = true;
                ctlToolStripMain.Items["Cut"].Enabled = true;
                ctlToolStripMain.Items["Paste"].Enabled = true;
                ctlToolStripMain.Items["Clear"].Enabled = true;
                ctlToolStripMain.Items["Checksum"].Enabled = false;
                ctlToolStripMain.Items["Strings"].Enabled = false;
                ctlToolStripMain.Items["Numbers"].Enabled = true;
                ctlToolStripMain.Items["Encoding"].Enabled = true;
            }
            else if (stateIndex == 1)
            {
                mnuEditCopy.Enabled = true;
                mnuEditCopyAsBinary.Enabled = true;
                mnuEditCut.Enabled = true;
                mnuEditPaste.Enabled = true;
                mnuEditPasteSpecialText.Enabled = true;
                mnuEditPasteSpecialBinary.Enabled = true;
                //
                mnuToolsStrings.Enabled = true;
                mnuToolsChecksums.Enabled = true;
                mnuToolsEntropy.Enabled = true;
                mnuToolsFileInfo.Enabled = true;
                mnuExternal.Enabled = true;
                ctlToolStripMain.Items["Copy"].Enabled = true;
                ctlToolStripMain.Items["CopyBinary"].Enabled = true;
                ctlToolStripMain.Items["Cut"].Enabled = true;
                ctlToolStripMain.Items["Paste"].Enabled = true;
                ctlToolStripMain.Items["Clear"].Enabled = true;
                ctlToolStripMain.Items["Checksum"].Enabled = true;
                ctlToolStripMain.Items["Strings"].Enabled = true;
                ctlToolStripMain.Items["Numbers"].Enabled = true;
                ctlToolStripMain.Items["Encoding"].Enabled = true;
            }
            if (oClipColl.Count() < 1)
            {
                ctlToolStripMain.Items["Back"].Enabled = false;
                ctlToolStripMain.Items["Forward"].Enabled = false;
            }
            else
            {
                ctlToolStripMain.Items["Back"].Enabled = true;
                ctlToolStripMain.Items["Forward"].Enabled = true;
                if (lstClip.SelectedIndex < 1)
                {
                    ctlToolStripMain.Items["Back"].Enabled = false;
                }
                else if (lstClip.SelectedIndex >= oClipColl.Count() - 1)
                {
                    ctlToolStripMain.Items["Forward"].Enabled = false;
                }
            }
        }

        private void SetStatusText(string msg, int index)
        {
            ToolStripStatusLabel newItem;

            while (index >= sbrStatusStrip.Items.Count)
            {
                newItem = new ToolStripStatusLabel("");
                newItem.BorderSides = ToolStripStatusLabelBorderSides.All;
                newItem.BorderStyle = Border3DStyle.Sunken;
                newItem.Margin = new Padding(2, 0, 0, 2);
                sbrStatusStrip.Items.Add(newItem);
            }
            sbrStatusStrip.Items[index].Text = msg;
        }

        private void SetTitleText(string msg)
        {
            if(string.IsNullOrEmpty(msg))
            {
                this.Text = "RETouch";
            }
            else
            {
                this.Text = msg + " - RETouch";
            }
        }

        private void SetWaitState(bool isWaiting)
        {
            if(isWaiting)
            {
                SetStatusText("Wait...", (int)STATUS_STRIP_INDEX.STATE);
                this.Cursor = Cursors.WaitCursor;
                Cursor.Position = Cursor.Position;
            }
            else
            {
                SetStatusText("Ready", (int)STATUS_STRIP_INDEX.STATE);
                this.Cursor = Cursors.Default;
                Cursor.Position = Cursor.Position;
            }
            Application.DoEvents();
        }

        #endregion

        private void DumpBuffer()
        {
            string tempStr;

            tempStr = MainCode.DumpByteBuffer(gSrcBuffer, !_Settings.DisplayNumberFormatDecimal);
            txtResult.Text = tempStr;
            txtResult.AppendText(Environment.NewLine);
            SetStatusText(string.Format("Lines {0}, Chars {1}, Selected Chars {2}",
                txtSource.Lines.Length, txtSource.Text.Length, txtSource.SelectedText.Length),
                (int)STATUS_STRIP_INDEX.SRC_INFO);
            //
            NewUIClip("Dump Buffer");
        }

        private void NewUIClip(string title = "")
        {
            libClipItem newItem;

            if (string.IsNullOrEmpty(txtResult.Text)) return;
            //
            newItem = new libClipItem();
            newItem.ClipContentSource= txtSource.Text;
            newItem.ClipContentResult = txtResult.Text;
            newItem.ClipNote = "";
            newItem.ClipTitle = "";
            newItem.SaveClipState(sbrStatusStrip.Items);
            oClipColl.Add(newItem);
            if (string.IsNullOrEmpty(title))
            {
                newItem.ClipTitle = "Clip" + newItem.ClipID;
            }
            else
            {
                newItem.ClipTitle = "Clip" + newItem.ClipID + " " + title;
            }
            SetStatusText(title, (int)STATUS_STRIP_INDEX.HISTORY_TITLE);
            lstClip.Items.Add(newItem.ClipTitle);
            lstClip.SelectedIndex = -1;
            _isDirty = true;
        }

        //--------------------------------------------------------
        // Event handlers
        //--------------------------------------------------------

        #region // Main form's handlers

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult retVal;

            // Save user settings if needed
            if (_settingsIsDirty)
            {
                MainCode.SaveUserSettings(_settingsFilename, _Settings);
                _settingsIsDirty = false;
            }
            if(_isDirty)
            {
                retVal = MessageBox.Show("Save Changes to Workspace?", "Save Changes", 
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if(retVal == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (retVal == DialogResult.Yes)
                {
                    mnuFileSave_Click(sender, new EventArgs());
                }
            }
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            string currSelection;
            char[] charBuffer;
            string tempStr;
            string statusMsg;

            // FIX 20180201: Disabled command history
            if (e.KeyCode == Keys.Up && oClipColl != null && oClipColl.Count() > 0)
            {
                
            }
            else if (e.Control && e.KeyCode == Keys.V) // Paste'n'Decode
            {
                // TODO: This must be optional feature, user should be able to swith it off
                if (Clipboard.ContainsText())
                {
                    currSelection = Clipboard.GetText();
                    statusMsg = "";
                    if (!string.IsNullOrEmpty(currSelection))
                    {
                        try
                        {
                            gSrcBuffer = MainCode.ConvertFromBase64(currSelection, out statusMsg);
                            charBuffer = Encoding.Unicode.GetChars(gSrcBuffer);
                            tempStr = new string(charBuffer);
                            txtSource.AppendText(tempStr);
                            txtSource.AppendText(Environment.NewLine);
                        }
                        catch (FormatException)
                        {
                            txtSource.AppendText(statusMsg);
                            txtSource.AppendText(Environment.NewLine);
                        }
                    }
                } // if(Clipboard...)
            }
        }

        #endregion

        // See https://msdn.microsoft.com/en-us/library/windows/desktop/aa384187(v=vs.85).aspx for System32/SysWOW64 hassle
        // Check TrIDEngine, see http://mark0.net/code-tridengine-e.html and local file TESampleCS.zip
        private void mnuExternalApplication_Click(object sender, EventArgs e)
        {
            MainCode.SupportedApplications appItem;
            string _currentExtApp;
            string _currentExtArgs;
            string appMenuName;
            int appIndex;
            StreamReader stdoutReader;
            string stdoutStr;

            appMenuName = ((ToolStripItem)(sender)).Text;
            for(appIndex = 0; appIndex < _builtinSupportedApps.Count; appIndex++)
            {
                if(_builtinSupportedApps.ElementAt(appIndex).appName.ToLower() == appMenuName.ToLower())
                {
                    appItem = _builtinSupportedApps.ElementAt(appIndex);
                    _currentExtApp = appItem.appExecutable;
                    _currentExtArgs = appItem.arguments;
                    _currentExtArgs = _currentExtArgs.Replace("[FILE]", _argFilename);
                    MainCode.StartExternalApplication(appItem, _currentExtApp, _currentExtArgs,
                        out stdoutReader, appItem.shellExecute);
                    if(appItem.useStdout && stdoutReader != null)
                    {
                        stdoutStr = stdoutReader.ReadToEnd();
                        txtResult.Text = stdoutStr;
                        txtResult.AppendText(Environment.NewLine);
                        NewUIClip("Called external application");
                    }
                }
            }
        }

        #region // File menu handlers

        private void mnuFileOpenDatafile_Click(object sender, EventArgs e)
        {
            string fileFilter;
            string strBuffer;

            fileFilter = @"Executables (*.exe;*.dll)|*.exe;*.dll|";
            fileFilter += @"Text Files (*.txt;*.csv)|*.txt;*.csv|";
            fileFilter += @"All Files (*.*)|*.*";
            ctlOpenFileDialog.AddExtension = true;
            ctlOpenFileDialog.CheckFileExists = true;
            ctlOpenFileDialog.CheckPathExists = true;
            ctlOpenFileDialog.DefaultExt = "exe";
            ctlOpenFileDialog.FileName = _lastDataFilename;
            ctlOpenFileDialog.Filter = fileFilter;
            ctlOpenFileDialog.InitialDirectory = _lastDataPath;
            ctlOpenFileDialog.Multiselect = false;
            ctlOpenFileDialog.SupportMultiDottedExtensions = true;
            if (ctlOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                _lastDataFilename = Path.GetFileName(ctlOpenFileDialog.FileName);
                _lastDataPath = Path.GetDirectoryName(ctlOpenFileDialog.FileName);
                //
                _argFilename = ctlOpenFileDialog.FileName;
                // Q&D patch
                if (ctlOpenFileDialog.FilterIndex == 2)
                {
                    strBuffer = File.ReadAllText(_argFilename);
                    txtSource.AppendText(strBuffer);
                    txtSource.AppendText(Environment.NewLine);
                    gSrcBuffer = Encoding.Default.GetBytes(strBuffer);
                    SetStatusText(string.Format("Buffer size: {0}", gSrcBuffer.Length),
                    (int)STATUS_STRIP_INDEX.BYTE_BUFFER_STATUS);
                }
                else
                {
                    gSrcBuffer = File.ReadAllBytes(_argFilename);
                    SetStatusText(string.Format("Buffer size: {0}", gSrcBuffer.Length),
                        (int)STATUS_STRIP_INDEX.BYTE_BUFFER_STATUS);
                }
                //SetTitleText(Path.GetFileName(_argFilename));
                SetMenuState(1);
                SetStatusText("", (int)STATUS_STRIP_INDEX.IMP_EXP_INFO); // Clear current text(s)
            }
        }

        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            DialogResult retVal;
            string fileFilter;

            if (_isDirty)
            {
                retVal = MessageBox.Show("Save Changes to Workspace?", "Save Changes",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (retVal == DialogResult.Cancel)
                {
                    return;
                }
                else if (retVal == DialogResult.Yes)
                {
                    mnuFileSave_Click(sender, new EventArgs());
                }
            }
            fileFilter = @"RETouch Workspace (*.rws)|*.rws|";
            fileFilter += @"All Files (*.*)|*.*";
            ctlOpenFileDialog.AddExtension = true;
            ctlOpenFileDialog.CheckFileExists = true;
            ctlOpenFileDialog.CheckPathExists = true;
            ctlOpenFileDialog.DefaultExt = "rws";
            ctlOpenFileDialog.FileName = _lastFilename;
            ctlOpenFileDialog.Filter = fileFilter;
            ctlOpenFileDialog.InitialDirectory = _lastPath;
            ctlOpenFileDialog.Multiselect = false;
            ctlOpenFileDialog.SupportMultiDottedExtensions = true;
            if (ctlOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                _lastFilename = Path.GetFileName(ctlOpenFileDialog.FileName);
                _lastPath = Path.GetDirectoryName(ctlOpenFileDialog.FileName);
                //
                MainCode.LoadWorkspace(Path.Combine(_lastPath, _lastFilename), this);
                oClipColl.SetToListBox(this.lstClip);
                SetMenuState(_lastMenuState); // Restore saved menu state
                SetTitleText(_lastFilename);
                _isDirty = false;
            }
        }

        private void mnuFileSave_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(_lastFilename))
            {
                mnuFileSaveAs_Click(sender, e);
                return;
            }
            //
            MainCode.SaveWorkspace(Path.Combine(_lastPath, _lastFilename), this);
            _isDirty = false;
        }

        private void mnuFileSaveAs_Click(object sender, EventArgs e)
        {
            string fileFilter;

            fileFilter = @"RETouch Workspace (*.rws)|*.rws|";
            fileFilter += @"All Files (*.*)|*.*";
            ctlSaveFileDialog.AddExtension = true;
            ctlSaveFileDialog.CheckFileExists = false;
            ctlSaveFileDialog.CheckPathExists = true;
            ctlSaveFileDialog.DefaultExt = "rws";
            ctlSaveFileDialog.FileName = _lastFilename;
            ctlSaveFileDialog.Filter = fileFilter;
            ctlSaveFileDialog.InitialDirectory = _lastPath;
            ctlSaveFileDialog.SupportMultiDottedExtensions = true;
            if (ctlSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _lastFilename = Path.GetFileName(ctlSaveFileDialog.FileName);
                _lastPath = Path.GetDirectoryName(ctlSaveFileDialog.FileName);
                //
                MainCode.SaveWorkspace(Path.Combine(_lastPath, _lastFilename), this);
                SetTitleText(_lastFilename);
                _isDirty = false;
            }
        }

        private void mnuFileSaveData_Click(object sender, EventArgs e)
        {
            ctlSaveFileDialog.AddExtension = true;
            ctlSaveFileDialog.CheckPathExists = true;
            ctlSaveFileDialog.DefaultExt = "txt";
            ctlSaveFileDialog.FileName = _lastDataFilename;
            ctlSaveFileDialog.Filter = @"Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            ctlSaveFileDialog.InitialDirectory = _lastDataPath;
            ctlSaveFileDialog.OverwritePrompt = true;
            ctlSaveFileDialog.SupportMultiDottedExtensions = true;
            if (ctlSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _lastDataFilename = Path.GetFileName(ctlSaveFileDialog.FileName);
                _lastDataPath = Path.GetDirectoryName(ctlSaveFileDialog.FileName);
                //
                File.WriteAllText(Path.Combine(_lastDataPath, _lastDataFilename), txtSource.Text);
            }
        }

        private void mnuFileImportWordList_Click(object sender, EventArgs e)
        {
            string wordListFilename;

            ctlOpenFileDialog.AddExtension = true;
            ctlOpenFileDialog.CheckFileExists = true;
            ctlOpenFileDialog.CheckPathExists = true;
            ctlOpenFileDialog.DefaultExt = "txt";
            ctlOpenFileDialog.FileName = "";
            ctlOpenFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            ctlOpenFileDialog.InitialDirectory = _lastPath;
            ctlOpenFileDialog.Multiselect = false;
            ctlOpenFileDialog.SupportMultiDottedExtensions = true;
            if (ctlOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                wordListFilename = Path.GetFileName(ctlOpenFileDialog.FileName);
                _lastPath = Path.GetDirectoryName(ctlOpenFileDialog.FileName);
                _wordList = MainCode.ImportWordList(Path.Combine(_lastPath, wordListFilename));
                //
                _argFilename = ctlOpenFileDialog.FileName;
                SetStatusText(string.Format("Imported {0} string(s)", _wordList.Count), (int)STATUS_STRIP_INDEX.IMP_EXP_INFO); // Clear current text(s)
            }
        }

        private void mnuFileImportRedWordList_Click(object sender, EventArgs e)
        {
            string redWordListFilename;

            ctlOpenFileDialog.AddExtension = true;
            ctlOpenFileDialog.CheckFileExists = true;
            ctlOpenFileDialog.CheckPathExists = true;
            ctlOpenFileDialog.DefaultExt = "txt";
            ctlOpenFileDialog.FileName = "";
            ctlOpenFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            ctlOpenFileDialog.InitialDirectory = _lastPath;
            ctlOpenFileDialog.Multiselect = false;
            ctlOpenFileDialog.SupportMultiDottedExtensions = true;
            if (ctlOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                redWordListFilename = Path.GetFileName(ctlOpenFileDialog.FileName);
                _lastPath = Path.GetDirectoryName(ctlOpenFileDialog.FileName);
                _redWordList = MainCode.ImportWordList(Path.Combine(_lastPath, redWordListFilename));
                //
                _argFilename = ctlOpenFileDialog.FileName;
                SetStatusText(string.Format("Imported {0} string(s)", _redWordList.Count), (int)STATUS_STRIP_INDEX.IMP_EXP_INFO); // Clear current text(s)
            }
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuEditCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSource.SelectedText))
            {
                Clipboard.SetText(txtSource.SelectedText);
            }
        }

        #endregion

        #region // Edit menu handlers
        private void mnuEditCopyAsBinary_Click(object sender, EventArgs e)
        {
            byte[] buffer;

            if (!string.IsNullOrEmpty(txtSource.SelectedText))
            {
                buffer = Encoding.Default.GetBytes(txtSource.SelectedText);
                Clipboard.SetData(DataFormats.Serializable, buffer);
            }
        }

        private void mnuEditCut_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSource.SelectedText))
            {
                Clipboard.SetText(txtSource.SelectedText);
            }
        }

        private void mnuEditPaste_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Clipboard.GetText()))
            {
                txtSource.Paste();
                SetStatusText(string.Format("Lines {0}, Chars {1}, Selected Chars {2}", 
                    txtSource.Lines.Length, txtSource.Text.Length, txtSource.SelectedText.Length),
                    (int)STATUS_STRIP_INDEX.SRC_INFO);
            }
        }

        private void mnuEditPasteSpecialBinary_Click(object sender, EventArgs e)
        {
            byte[] buffer;
            string visualStr;

            if (Clipboard.GetData(DataFormats.Serializable) != null)
            {
                buffer = (byte[])Clipboard.GetData(DataFormats.Serializable);
                // Show visual
                visualStr = MainCode.ByteArrayToString(buffer);
                txtSource.AppendText(visualStr + Environment.NewLine);
                SetStatusText(string.Format("Lines {0}, Chars {1}, Selected Chars {2}",
                    txtSource.Lines.Length, txtSource.Text.Length, txtSource.SelectedText.Length),
                    (int)STATUS_STRIP_INDEX.SRC_INFO);
            }
        }

        private void mnuEditPasteSpecialText_Click(object sender, EventArgs e)
        {
            byte[] buffer;
            string visualStr;

            if (Clipboard.GetData(DataFormats.Serializable) != null)
            {
                buffer = (byte[])Clipboard.GetData(DataFormats.Serializable);
                // Show visual
                visualStr = Encoding.Default.GetString(buffer);
                txtSource.AppendText(visualStr + Environment.NewLine);
                SetStatusText(string.Format("Lines {0}, Chars {1}, Selected Chars {2}",
                    txtSource.Lines.Length, txtSource.Text.Length, txtSource.SelectedText.Length),
                    (int)STATUS_STRIP_INDEX.SRC_INFO);
            }
            else if (Clipboard.ContainsText())
            {
                visualStr = Clipboard.GetText();
                txtSource.AppendText(visualStr + Environment.NewLine);
                SetStatusText(string.Format("Lines {0}, Chars {1}, Selected Chars {2}",
                    txtSource.Lines.Length, txtSource.Text.Length, txtSource.SelectedText.Length),
                    (int)STATUS_STRIP_INDEX.SRC_INFO);
            }
        }

        #endregion

        #region // View menu handlers

        private void mnuViewEncodingDefault_Click(object sender, EventArgs e)
        {
            char[] charArr;

            if (_encoding != MainCode.TEXT_ENCODING.Default && gSrcBuffer != null)
            {
                // Change current encoding using data from source buffer
                charArr = Encoding.Default.GetChars(gSrcBuffer);
                txtResult.Text = new string(charArr);
                txtResult.AppendText(Environment.NewLine);
                _encodingCurrent = Encoding.Default;
                SetStatusText("Text Encoding: Default", (int)STATUS_STRIP_INDEX.TEXT_ENCODING);
                NewUIClip("Changed Encoding To Default");
            }
        }

        private void mnuViewEncodingASCII_Click(object sender, EventArgs e)
        {
            char[] charArr;

            if (_encoding != MainCode.TEXT_ENCODING.ASCII && gSrcBuffer != null)
            {
                // Change current encoding
                charArr = Encoding.ASCII.GetChars(gSrcBuffer);
                txtResult.Text = new string(charArr);
                txtResult.AppendText(Environment.NewLine);
                _encodingCurrent = Encoding.ASCII;
                SetStatusText("Text Encoding: ASCII", (int)STATUS_STRIP_INDEX.TEXT_ENCODING);
                NewUIClip("Changed Encoding To ASCII");
            }
        }

        private void mnuViewEncodingUTF8_Click(object sender, EventArgs e)
        {
            char[] charArr;

            if (_encoding != MainCode.TEXT_ENCODING.UTF8 && gSrcBuffer != null)
            {
                // Change current encoding
                charArr = Encoding.UTF8.GetChars(gSrcBuffer);
                txtResult.Text = new string(charArr);
                txtResult.AppendText(Environment.NewLine);
                _encodingCurrent = Encoding.UTF8;
                SetStatusText("Text Encoding: UTF8", (int)STATUS_STRIP_INDEX.TEXT_ENCODING);
                NewUIClip("Changed Encoding To UTF8");
            }
        }

        private void mnuViewEncodingUnicode_Click(object sender, EventArgs e)
        {
            char[] charArr;

            if (_encoding != MainCode.TEXT_ENCODING.UNICODE && gSrcBuffer != null)
            {
                // Change current encoding
                charArr = Encoding.Unicode.GetChars(gSrcBuffer);
                txtResult.Text = new string(charArr);
                txtResult.AppendText(Environment.NewLine);
                _encodingCurrent = Encoding.Unicode;
                SetStatusText("Text Encoding: Unicode", (int)STATUS_STRIP_INDEX.TEXT_ENCODING);
                NewUIClip("Changed Encoding To Unicode");
            }
        }

        private void mnuViewEncodingUTF32_Click(object sender, EventArgs e)
        {
            char[] charArr;

            if (_encoding != MainCode.TEXT_ENCODING.UTF32 && gSrcBuffer != null)
            {
                // Change current encoding
                charArr = Encoding.UTF32.GetChars(gSrcBuffer);
                txtResult.Text = new string(charArr);
                txtResult.AppendText(Environment.NewLine);
                _encodingCurrent = Encoding.UTF32;
                SetStatusText("Text Encoding: UTF32", (int)STATUS_STRIP_INDEX.TEXT_ENCODING);
                NewUIClip("Changed Encoding To UTF32");
            }
        }

        private void mnuViewEncodingToBytes_Click(object sender, EventArgs e)
        {
            string currSelection;

            if (_encodingCurrent == null) _encodingCurrent = Encoding.Default;
            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            gSrcBuffer = _encodingCurrent.GetBytes(currSelection);
            SetStatusText(string.Format("Buffer size: {0}", gSrcBuffer.Length),
                (int)STATUS_STRIP_INDEX.BYTE_BUFFER_STATUS);
            //
            NewUIClip("Encoded Text to Bytes");
        }

        private void mnuViewEncodingFromBytes_Click(object sender, EventArgs e)
        {
            string currSelection;

            if (gSrcBuffer == null || _encodingCurrent == null) return;
            currSelection = new string(_encodingCurrent.GetChars(gSrcBuffer));
            txtResult.Text = currSelection;
            txtResult.AppendText(Environment.NewLine);
            //
            NewUIClip("Decoded Bytes to Text");
        }

        private void mnuViewConvertDecimalToBinary_Click(object sender, EventArgs e)
        {
            string currSelection;

            // Text to text conversion
            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            currSelection = MainCode.ConvertDecimalTextToBinary(currSelection);
            txtResult.Text = currSelection;
            txtResult.AppendText(Environment.NewLine);
            NewUIClip("Convert Decimal Text To Binary Text");
        }

        private void mnuViewConvertDecimalToOctal_Click(object sender, EventArgs e)
        {
            string currSelection;

            // Text to text conversion
            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            currSelection = MainCode.ConvertDecimalTextToOctal(currSelection);
            txtResult.Text = currSelection;
            txtResult.AppendText(Environment.NewLine);
            NewUIClip("Convert Decimal Text To Octal Text");
        }

        private void mnuViewConvertDesimalToHex_Click(object sender, EventArgs e)
        {
            string currSelection;

            // Text to text conversion
            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            currSelection = MainCode.ConvertDecimalTextToHex(currSelection);
            txtResult.Text = currSelection;
            txtResult.AppendText(Environment.NewLine);
            NewUIClip("Convert Decimal Text To Hexadecimal Text");
        }

        private void mnuViewConvertHexToBinary_Click(object sender, EventArgs e)
        {
            string currSelection;

            // Text to text conversion
            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            currSelection = MainCode.ConvertHexTextToBinary(currSelection);
            txtResult.Text = currSelection;
            txtResult.AppendText(Environment.NewLine);
            NewUIClip("Convert Hexadecimal Text To Binary Text");
        }

        private void mnuViewConvertHexToOctal_Click(object sender, EventArgs e)
        {
            string currSelection;

            // Text to text conversion
            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            if (!string.IsNullOrEmpty(currSelection))
            {
                currSelection = MainCode.ConvertHexTextToOctal(currSelection);
            }
            else
            {
                currSelection = MainCode.ConvertHexTextToOctal(txtSource.Text);
            }
            txtResult.Text = currSelection;
            txtResult.AppendText(Environment.NewLine);
            NewUIClip("Convert Hexadecimal Text To Octal Text");
        }

        private void mnuViewConvertHexToDecimal_Click(object sender, EventArgs e)
        {
            string currSelection;

            // Text to text conversion
            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            currSelection = MainCode.ConvertHexTextToDecimal(currSelection);
            txtResult.Text = currSelection;
            txtResult.AppendText(Environment.NewLine);
            NewUIClip("Convert Hexadecimal Text To Decimal Text");
        }

        private void mnuViewConvertBinaryToDecimal7bit_Click(object sender, EventArgs e)
        {
            string currSelection;

            // Text to text conversion
            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            currSelection = MainCode.ConvertBinaryTextToDecimal(currSelection, 7);
            txtResult.Text = currSelection;
            txtResult.AppendText(Environment.NewLine);
            NewUIClip("Convert 7-bit Binary To Decimal");
        }

        private void mnuViewConvertBinaryToDecimal8bit_Click(object sender, EventArgs e)
        {
            string currSelection;

            // Text to text conversion
            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            currSelection = MainCode.ConvertBinaryTextToDecimal(currSelection, 8);
            txtResult.Text = currSelection;
            txtResult.AppendText(Environment.NewLine);
            NewUIClip("Convert 8-bit Binary To Decimal");
        }

        private void mnuViewConvertDecimalToBytes_Click(object sender, EventArgs e)
        {
            string currSelection;

            // Text to bytes conversion
            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            if (!string.IsNullOrEmpty(currSelection))
            {
                gSrcBuffer = MainCode.ConvertDecimalTextToBytes(currSelection);
                SetStatusText(string.Format("Buffer size: {0}", gSrcBuffer.Length),
                    (int)STATUS_STRIP_INDEX.BYTE_BUFFER_STATUS);

            }
            NewUIClip("Convert Decimal Text To Bytes");
        }

        private void mnuViewConvertHexToBytes_Click(object sender, EventArgs e)
        {
            string currSelection;

            // Text to bytes conversion
            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            if (!string.IsNullOrEmpty(currSelection))
            { 
                gSrcBuffer = MainCode.ConvertHexToBytes(txtSource.Text);
                SetStatusText(string.Format("Buffer size: {0}", gSrcBuffer.Length),
                    (int)STATUS_STRIP_INDEX.BYTE_BUFFER_STATUS);
            }
            NewUIClip("Convert Hexadecimal Text To Bytes");
        }

        private void mnuViewConvertBase64ToBytes_Click(object sender, EventArgs e)
        {
            string currSelection;

            // Text to bytes conversion
            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            try
            {
                gSrcBuffer = Convert.FromBase64String(currSelection);
                txtResult.Text = MainCode.DumpByteBuffer(gSrcBuffer, true);
                txtResult.AppendText(Environment.NewLine);
                SetStatusText(string.Format("Buffer size: {0}", gSrcBuffer.Length),
                    (int)STATUS_STRIP_INDEX.BYTE_BUFFER_STATUS);
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.Message;
                txtResult.AppendText(Environment.NewLine);
            }
            NewUIClip("Convert Base64 Text To Bytes");
        }

        private void mnuViewConvertBytesToDecimal_Click(object sender, EventArgs e)
        {
            StringBuilder sb;

            // Bytes to text conversion
            if (gSrcBuffer == null || gSrcBuffer.Length < 1) return;
            sb = new StringBuilder();
            for (int i = 0; i < gSrcBuffer.Length; i++)
            {
                sb.Append(gSrcBuffer[i]);
                sb.Append(" ");
            }
            txtResult.Text = sb.ToString();
            txtResult.AppendText(Environment.NewLine);
            NewUIClip("Convert Bytes To Decimal Text");
        }

        private void mnuViewConvertBytesToHex_Click(object sender, EventArgs e)
        {
            StringBuilder sb;

            // Bytes to text conversion
            if (gSrcBuffer == null || gSrcBuffer.Length < 1) return;
            sb = new StringBuilder();
            for (int i = 0; i < gSrcBuffer.Length; i++)
            {
                sb.Append(string.Format("{0:X2} ", gSrcBuffer[i]));
            }
            txtResult.Text = sb.ToString();
            txtResult.AppendText(Environment.NewLine);
            NewUIClip("Convert Bytes To Hexadecimal Text");
        }

        private void mnuViewConvertBytesToBase64_Click(object sender, EventArgs e)
        {
            // Bytes to text conversion
            if (gSrcBuffer == null || gSrcBuffer.Length < 1) return;
            txtResult.AppendText(Convert.ToBase64String(gSrcBuffer, Base64FormattingOptions.InsertLineBreaks));
            txtResult.AppendText(Environment.NewLine);
            NewUIClip("Convert Bytes To Base64 Text");
        }

        private void mnuViewSpecialURLEncodeText_Click(object sender, EventArgs e)
        {
            string currSelection;

            // Text to text conversion
            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            currSelection = HttpUtility.UrlEncode(currSelection);
            txtResult.Text = currSelection;
            txtResult.AppendText(Environment.NewLine);
            NewUIClip("URL Encode Text");
        }

        private void mnuViewSpecialURLDecodeText_Click(object sender, EventArgs e)
        {
            string currSelection;

            // Text to text conversion
            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            currSelection = HttpUtility.UrlDecode(currSelection);
            txtResult.Text = currSelection;
            txtResult.AppendText(Environment.NewLine);
            NewUIClip("URL Decode Text");
        }

        private void mnuViewSpecialURLEncodeBuffer_Click(object sender, EventArgs e)
        {
            string currSelection;

            // Bytes to text conversion
            if (gSrcBuffer == null | gSrcBuffer.Length < 1) return;
            currSelection = HttpUtility.UrlEncode(gSrcBuffer);
            txtResult.Text = currSelection;
            txtResult.AppendText(Environment.NewLine);
            NewUIClip("URL Encode Buffer");
        }

        private void mnuViewSpecialURLDecodeBuffer_Click(object sender, EventArgs e)
        {
            string currSelection;

            // Bytes to text conversion
            if (gSrcBuffer == null | gSrcBuffer.Length < 1) return;
            currSelection = HttpUtility.UrlDecode(gSrcBuffer, Encoding.Default);
            txtResult.Text = currSelection;
            txtResult.AppendText(Environment.NewLine);
            NewUIClip("URL Decode Buffer");
        }

        public CaseWare.libTextViewer.fileParameter GetViewFile(int arg)
        {
            CaseWare.libTextViewer.fileParameter param;

            // We have only single file so the value of int arg does not matter
            param = new CaseWare.libTextViewer.fileParameter();
            param.displayMode = CaseWare.libTextViewer.DISPLAY_MODE.HEX;
            //param.fileInformation;
            param.fileName = _argFilename;
            param.fileStream = null;
            param.hasValidData = true;
            param.isFirstFile = true;
            param.isLastFile = true;
            param.selectFileMode = CaseWare.libTextViewer.SELECT_MODE.CURRENT_FILE;
            //
            return param;
        }

        private void mnuViewHexView_Click(object sender, EventArgs e)
        {
            CaseWare.libTextViewer.fileParameter oParam;

            oParam = new CaseWare.libTextViewer.fileParameter();
            oParam.dataBuffer = null;
            oParam.displayMode = CaseWare.libTextViewer.DISPLAY_MODE.HEX;
            oParam.fileInformation = null;
            oParam.fileName = _argFilename;
            oParam.fileStream = null;
            oParam.hasValidData = true;
            oParam.isFirstFile = true;
            oParam.isLastFile = true;
            oParam.maxBuffer = 0;
            oParam.selectFileMode = CaseWare.libTextViewer.SELECT_MODE.CURRENT_FILE;
            //
            oTextViewer.SetGetFileFunction(GetViewFile);
            oTextViewer.ShowDataContent(oParam);
            oTextViewer.ShowDialog();
            // Do not call closeControl() because it disposes static oTextViewer instance. Call
            // closeControl() in main form's closing event
            //oTextViewer.closeControl();
        }

        private void mnuViewSpecialVBEncode_Click(object sender, EventArgs e)
        {
            string currSelection;

            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            if (!string.IsNullOrEmpty(currSelection))
            {
                try
                {
                    currSelection = MainCode.VBScriptEncode(currSelection);
                    txtResult.AppendText(currSelection);
                    txtResult.AppendText(Environment.NewLine);
                    //
                    NewUIClip("Encode String VBEncoding");
                }
                catch (Exception ex)
                {
                    txtSource.AppendText(string.Format("Unable to encode: {0}", ex.Message));
                    txtSource.AppendText(Environment.NewLine);
                }
            }
        }

        private void mnuViewShowBuffer_Click(object sender, EventArgs e)
        {
            string tempStr;

            if (gSrcBuffer != null)
            {
                tempStr = Encoding.Default.GetString(gSrcBuffer);
                txtResult.Text = tempStr;
            }
        }

        private void mnuViewDistribution_Click(object sender, EventArgs e)
        {
            frmDistribution oForm;
            int[] intArr;

            if (gSrcBuffer == null) return;
            intArr = MainCode.Distribution(gSrcBuffer);
            oForm = new frmDistribution();
            oForm.Icon = this.Icon;
            oForm.PlotDistribution(intArr);
            oForm.ShowDialog();
            oForm = null;
        }

        private void mnuViewFileVisualizer_Click(object sender, EventArgs e)
        {
            frmVisualizer oForm;

            oForm = new frmVisualizer();
            oForm.Icon = this.Icon;
            oForm.PlotChartData(_argFilename);
            oForm.ShowDialog();
            oForm = null;
        }

        private void mnuViewASCIITable_Click(object sender, EventArgs e)
        {
            frmViewTable oForm;

            oForm = new frmViewTable();
            oForm.ViewTable = MainCode.FORM_VIEW_TABLE.ASCII;
            oForm.ShowDialog();
            oForm = null;
        }

        private void mnuViewByteViewer_Click(object sender, EventArgs e)
        {
            frmByteViewer oForm;
            string currSelection;
            int bufSize;
            byte[] buffer;

            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            if (string.IsNullOrEmpty(currSelection)) return;
            // Load to buffer
            bufSize = currSelection.Length > 8 ? 8 : currSelection.Length;
            buffer = new byte[bufSize];
            for (int i = 0; i < bufSize; i++)
            {
                buffer[i] = (byte)currSelection[i];
            }
            //
            oForm = new frmByteViewer();
            oForm.InputBytes = buffer;
            oForm.ShowDialog();
            oForm = null;
        }

        #endregion

        #region // Tools menu handlers

        private void mnuToolsStringsExtract_Click(object sender, EventArgs e)
        {
            CaseWare.libStrings oStrings;
            string[] resultArr;
            MemoryStream ms;
            string currSelection;
            byte[] streamBuffer;

            if(string.IsNullOrEmpty(_argFilename))
            {
                currSelection = txtSource.SelectedText;
                if(string.IsNullOrEmpty(currSelection))
                {
                    currSelection = txtSource.Text;
                }
                // create mem stream
                streamBuffer = Encoding.Default.GetBytes(currSelection);
                ms = new MemoryStream(streamBuffer);
                SetWaitState(true);
                oStrings = new CaseWare.libStrings();
                oStrings.inputStream = ms;
                oStrings.minStringLength = _Settings.StringMinimumLength;
                resultArr = oStrings.parseStrings();
                SetWaitState(false);
            }
            else
            {
                SetWaitState(true);
                oStrings = new CaseWare.libStrings();
                oStrings.inputFileName = _argFilename;
                oStrings.minStringLength = _Settings.StringMinimumLength;
                resultArr = oStrings.parseStrings();
                SetWaitState(false);
            }
            //
            txtResult.Lines = resultArr;
            txtResult.AppendText(Environment.NewLine);
            SetStatusText(string.Format("{0} String(s) found", resultArr.Length), (int)STATUS_STRIP_INDEX.IMP_EXP_INFO);
            //
            NewUIClip("Extract Strings");
        }

        private void mnuToolsStringsMatchAPI_Click(object sender, EventArgs e)
        {
            List<string> matches;
            string matchStr;

            if (_apiDictionary == null)
            {
                _apiDictionary = RETouchResource.libResx.GetAPIDictionary(_apiDictionary);
                //_apiDictionary =RETouch
                //for (int i = 0; i < apiFilenames.Length; i++)
                //{ 
                //    _apiDictionary = MainCode.GetAPIDictionary(apiFilenames[i], _apiDictionary);
                //}
            }
            // Match txtResult.Lines
            matches = new List<string>();
            SetWaitState(true);
            if(_Settings.StringMatchingCaseSensitive)
            { 
                foreach (string s in txtSource.Lines)
                {
                    if(_apiDictionary.ContainsKey(s))
                    {
                        matchStr = _apiDictionary[s] + ": " + s;
                        if (!matches.Contains(matchStr)) matches.Add(matchStr);
                    }
                }
            }
            else // Case insensitive search
            {
                foreach (string s in txtSource.Lines)
                {
                    for(int i = 0; i < _apiDictionary.Keys.Count; i++)
                    {
                        if(_apiDictionary.Keys.ElementAt(i).IndexOf(s , StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            matchStr = _apiDictionary[_apiDictionary.Keys.ElementAt(i)] + ": " + s;
                            if (!matches.Contains(matchStr)) matches.Add(matchStr);
                            break; // Break inner loop
                        }
                    }
                }
            }
            SetWaitState(false);
            if (matches.Count > 0)
            {
                txtResult.Lines = matches.ToArray();
                txtResult.AppendText(Environment.NewLine);
                SetStatusText(string.Format("{0} String(s) found", matches.Count), (int)STATUS_STRIP_INDEX.IMP_EXP_INFO);
            }
            else
            {
                txtResult.Text = "No matches";
                txtResult.AppendText(Environment.NewLine);
                SetStatusText("0 String(s) found", (int)STATUS_STRIP_INDEX.IMP_EXP_INFO);
            }
            txtResult.AppendText(Environment.NewLine);
            //
            NewUIClip("Match With Windows API");
        }

        private void mnuToolsStringsMatchWordList_Click(object sender, EventArgs e)
        {
            List<string> matches;
            string matchStr;

            if (_wordList == null)
            {
                _wordList = RETouchResource.libResx.GetWordList(_wordList);
                return;
            }
            // Match txtResult.Lines
            matches = new List<string>();
            SetWaitState(true);
            foreach (string s in txtSource.Lines)
            {
                for(int i = 0; i < _wordList.Count; i++)
                {
                    if(s.IndexOf(_wordList.ElementAt(i), StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        matchStr = s;
                        if (!matches.Contains(matchStr)) matches.Add(matchStr);
                        break;
                    }
                }
            }
            SetWaitState(false);
            if (matches.Count > 0)
            {
                txtResult.Lines = matches.ToArray();
                txtResult.AppendText(Environment.NewLine);
                SetStatusText(string.Format("{0} String(s) found", matches.Count), (int)STATUS_STRIP_INDEX.IMP_EXP_INFO);
            }
            else
            {
                txtResult.Text = "No matches";
                txtResult.AppendText(Environment.NewLine);
                SetStatusText("0 String(s) found", (int)STATUS_STRIP_INDEX.IMP_EXP_INFO);
            }
            txtResult.AppendText(Environment.NewLine);
            //
            NewUIClip("Match With Wordlists");
        }

        private void mnuToolsStringsMatchRedWordList_Click(object sender, EventArgs e)
        {
            List<string> matches;
            string matchStr;

            if (_redWordList == null)
            {
                _redWordList = RETouchResource.libResx.GetRedWordList(_redWordList);
                return;
            }
            // Match txtResult.Lines
            matches = new List<string>();
            SetWaitState(true);
            foreach (string s in txtSource.Lines)
            {
                if (_redWordList.Contains(s))
                {
                    matchStr = s;
                    if (!matches.Contains(matchStr)) matches.Add(matchStr);
                }
            }
            SetWaitState(false);
            if (matches.Count > 0)
            {
                txtResult.Lines = matches.ToArray();
                txtResult.AppendText(Environment.NewLine);
                SetStatusText(string.Format("{0} String(s) found", matches.Count), (int)STATUS_STRIP_INDEX.IMP_EXP_INFO);
            }
            else
            {
                txtResult.Text = "No matches";
                txtResult.AppendText(Environment.NewLine);
                SetStatusText("0 String(s) found", (int)STATUS_STRIP_INDEX.IMP_EXP_INFO);
            }
            txtResult.AppendText(Environment.NewLine);
            //
            NewUIClip("Match Strings with Red Wordlist");
        }

        private void mnuToolsStringsExtractBOM_Click(object sender, EventArgs e)
        {
            CaseWare.libStrings oStrings;
            string[] resultArr;

            SetWaitState(true);
            oStrings = new CaseWare.libStrings();
            oStrings.inputFileName = _argFilename;
            //oStrings.minStringLength = 5;
            oStrings.AddSequence(MainCode.UTF8_BOM);
            oStrings.AddSequence(MainCode.UTF16_LE_BOM);
            oStrings.AddSequence(MainCode.UTF16_BE_BOM);
            resultArr = oStrings.parseSequences();
            SetWaitState(false);
            //
            txtSource.Lines = resultArr;
            SetStatusText(string.Format("{0} Match(es) found", resultArr.Length), (int)STATUS_STRIP_INDEX.IMP_EXP_INFO);
        }

        private void mnuToolsStringsSort_Click(object sender, EventArgs e)
        {
            string[] allLines;

            allLines = txtSource.Lines;
            Array.Sort(allLines);
            txtSource.Lines = allLines;
            //
            NewUIClip("Sort Strings");
        }

        private void mnuToolsStringsRemoveDuplicates_Click(object sender, EventArgs e)
        {
            txtSource.Lines = txtSource.Lines.Distinct().ToArray();
            SetStatusText(string.Format("{0} String(s) found", txtSource.Lines.Length), (int)STATUS_STRIP_INDEX.IMP_EXP_INFO);
            //
            NewUIClip("Remove Duplicate Strings");
        }

        private void mnuToolsChecksums_Click(object sender, EventArgs e)
        {
            libCheckSum.libCheckSum oChecksum;
            string sumStr;

            oChecksum = new libCheckSum.libCheckSum();
            //
            sumStr = "";
            sumStr += string.Format("MD5: {0}", oChecksum.CalculateMD5(_argFilename)) + Environment.NewLine;
            sumStr += string.Format("SHA1: {0}", oChecksum.CalculateSHA1(_argFilename)) + Environment.NewLine;
            sumStr += string.Format("SHA256: {0}", oChecksum.CalculateSHA256(_argFilename)) + Environment.NewLine;
            txtResult.SelectionBackColor = _warningBackColor;
            txtResult.Text = sumStr;
            txtResult.SelectionBackColor = _normalBackColor;
            //
            NewUIClip("File Checksums");
        }

        private void mnuToolsEntropy_Click(object sender, EventArgs e)
        {
            CaseWare.libEntropy oEntropy;
            string infoStr;

            oEntropy = new CaseWare.libEntropy();
            //
            SetWaitState(true);
            oEntropy.CalcBinaryFileEntropy(_argFilename);
            SetWaitState(false);
            infoStr = string.Format("File entropy (bits per byte): {0:N4}", oEntropy.Entropy) + Environment.NewLine;
            txtResult.Text = infoStr;
            //
            NewUIClip("File Entropy");
        }

        private void mnuToolsFileInfo_Click(object sender, EventArgs e)
        {
            FileInfo fi;
            string infoStr;

            fi = new FileInfo(_argFilename);
            //
            infoStr = "";
            infoStr += string.Format("Fullname: {0}", fi.FullName) + Environment.NewLine;
            infoStr += string.Format("Length: {0} bytes", MainCode.FileSize2String(fi.Length)) + Environment.NewLine;
            infoStr += string.Format("Attributes: {0}", MainCode.Attributes2String(fi.Attributes)) + Environment.NewLine;
            infoStr += string.Format("Created: {0}", fi.CreationTime) + Environment.NewLine;
            infoStr += string.Format("Written: {0}", fi.LastWriteTime) + Environment.NewLine;
            infoStr += string.Format("Accessed: {0}", fi.LastAccessTime) + Environment.NewLine;
            txtResult.AppendText(infoStr);
            //
            NewUIClip("File Information");
        }

        // Init and open interactive(?) Python environment
        private void mnuToolsPython_Click(object sender, EventArgs e)
        {
            string pathToPyLauncher;
            StreamReader stdoutReader;

            //pathToPyLauncher = @"D:\CSharp-NET46\ReTouch\RETouch\RETouch\Data\retouch-python.py";
            // Assume Python is in the PATH environment variable
            pathToPyLauncher = @"py.exe";
            // StartExternalApplication(string appPath, string argFilename, bool useShell = false)
            //MainCode.StartExternalApplication("cmd.exe", " - k " + pathToPyLauncher, true);
            MainCode.StartExternalApplication(new MainCode.SupportedApplications(), pathToPyLauncher, "", out stdoutReader, true);
        }

        //private void mnuToolsConvertToBinary_Click(object sender, EventArgs e)
        //{
        //    string currSelection;
        //    long tempLng;

        //    // TODO: should accept array
        //    // Parse text first
        //    currSelection = txtSource.SelectedText;
        //    if (string.IsNullOrEmpty(currSelection)) return;
        //    //
        //    if (long.TryParse(currSelection, out tempLng))
        //    {
        //        currSelection = MainCode.ConvertULongToBinary((ulong)tempLng);
        //        //
        //        txtResult.Text = currSelection;
        //        txtResult.AppendText(Environment.NewLine);
        //        //
        //        NewUIClip("Convert to Binary");
        //    }
        //}

        //private void mnuToolsConvertToDecimal_Click(object sender, EventArgs e)
        //{
        //    string currSelection;
        //    long tempLng;

        //    currSelection = txtSource.SelectedText;
        //    if (string.IsNullOrEmpty(currSelection)) return;
        //    //
        //    if (long.TryParse(currSelection, out tempLng))
        //    {
        //        currSelection = ((ulong)tempLng).ToString();
        //        //
        //        txtResult.Text = currSelection;
        //        txtResult.AppendText(Environment.NewLine);
        //        //
        //        NewUIClip("Convert to Decimal");
        //    }
        //}

        // See https://stackoverflow.com/questions/4247037/octal-equivalent-in-c-sharp
        // int i = Convert.ToInt32("12", 8);
        //private void mnuToolsConvertToOct_Click(object sender, EventArgs e)
        //{
        //    string currSelection;
        //    long tempLng;

        //    currSelection = txtSource.SelectedText;
        //    if (string.IsNullOrEmpty(currSelection)) return;
        //    //
        //    if (long.TryParse(currSelection, out tempLng))
        //    {
        //        currSelection = ((ulong)tempLng).ToString("X8");
        //        //
        //        txtResult.Text = currSelection;
        //        txtResult.AppendText(Environment.NewLine);
        //        //
        //        NewUIClip("Convert to Octal");
        //    }
        //}

        //private void mnuToolsConvertToHex_Click(object sender, EventArgs e)
        //{
        //    string currSelection;
        //    long tempLng;

        //    currSelection = txtSource.SelectedText;
        //    if (string.IsNullOrEmpty(currSelection)) return;
        //    //
        //    if (long.TryParse(currSelection, out tempLng))
        //    {
        //        currSelection = ((ulong)tempLng).ToString("X8");
        //        //
        //        txtResult.Text = currSelection;
        //        txtResult.AppendText(Environment.NewLine);
        //        //
        //        NewUIClip("Convert to Hex");
        //    }
        //}

        private void mnuToolsSearchWithGoogle_Click(object sender, EventArgs e)
        {
            string currSelection;
            string searchUri;
            StreamReader stdoutReader;

            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection)) return;
            //
            searchUri = MainCode.GetGoogleQueryUrl(currSelection);
            MainCode.StartExternalApplication(new MainCode.SupportedApplications(), searchUri, "", out stdoutReader, true);
        }

        private void mnuToolsSearchWithMSDN_Click(object sender, EventArgs e)
        {
            string currSelection;
            string searchUri;
            StreamReader stdoutReader;

            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection)) return;
            //
            searchUri = MainCode.GetMSDNQueryUrl(currSelection);
            MainCode.StartExternalApplication(new MainCode.SupportedApplications(), searchUri, "", out stdoutReader, true);
        }

        private void mnuToolsTransform_Click(object sender, EventArgs e)
        {
            // xor and rot, both need input buffer and array of numeric argument(s)
            MainCode.TRANSFORM_ARG param;
            frmPromptUserInput oForm;
            string currSelection;
            string transformResult;
            string opString;
            byte[] buffer;
            byte[] paramBuffer;
            int paramStartValue;
            int paramEndValue;
            byte[] resultBuffer;
            byte[] argValue;

            opString = "";
            // Get data first
            currSelection = txtSource.SelectedText;
            if (!string.IsNullOrEmpty(currSelection))
            {
                buffer = MainCode.ParseStringToByteArray(currSelection);
            }
            else
            {
                if (gSrcBuffer == null || gSrcBuffer.Length < 1) return;
                buffer = new byte[gSrcBuffer.Length];
                gSrcBuffer.CopyTo(buffer, 0);
            }
            // Prompt op and arg
            oForm = new frmPromptUserInput();
            param = new MainCode.TRANSFORM_ARG();
            oForm.monoSpaceFont = _monoSpaceFont;
            oForm.transformArg = param;
            //oForm.commandHistory = _commandHistory;
            oForm.ShowDialog();
            if (oForm.FormRetValue == MainCode.FORM_RETURN_VALUE.OK)
            {
                //_commandHistory = oForm.commandHistory;
                param = oForm.transformArg;
                paramStartValue = 0;
                paramEndValue = param.args.Length;
                paramBuffer = new byte[param.args.Length];
                param.args.CopyTo(paramBuffer, 0);
                if (buffer.Length > 0)
                {
                    if(param.opCode == MainCode.TRANSFORM_OP.Caesar)
                    {
                        resultBuffer = MainCode.TransformBlob(buffer, param);
                        if (resultBuffer != null && resultBuffer.Length > 0)
                        {
                            gDestBuffer = new byte[resultBuffer.Length];
                            resultBuffer.CopyTo(gDestBuffer, 0);
                        }
                        argValue = new byte[] { param.args[0] };
                        opString = param.opCode.ToString() + " " + (_Settings.DisplayNumberFormatDecimal ?
                            MainCode.ConvertByteArrayToDecimal(argValue) : MainCode.ConvertByteArrayToHex(argValue));
                        transformResult = MainCode.ConvertByteArrayToDecimal(resultBuffer);
                        txtResult.AppendText(opString + ": ");
                        transformResult = MainCode.ByteArrayToString(resultBuffer);
                        txtResult.AppendText(transformResult);
                        txtResult.AppendText(Environment.NewLine);
                    }
                    for(int i = paramStartValue; i < paramEndValue; i++)
                    {
                        if(param.opCode == MainCode.TRANSFORM_OP.Replace) // Replace needs two values!!
                        {
                            param.args = new byte[] { paramBuffer[i], paramBuffer[i + 1] };
                            i++; // Skip one because we use two values in here
                            resultBuffer = MainCode.TransformBlob(buffer, param);
                        }
                        else
                        {
                            param.args = new byte[] { paramBuffer[i] };
                            resultBuffer = MainCode.TransformBlob(buffer, param);
                        }
                        if(resultBuffer != null && resultBuffer.Length > 0)
                        {
                            gDestBuffer = new byte[resultBuffer.Length];
                            resultBuffer.CopyTo(gDestBuffer, 0);
                        }
                        argValue = new byte[] { param.args[0] };
                        opString = param.opCode.ToString() + " " + (_Settings.DisplayNumberFormatDecimal ?
                            MainCode.ConvertByteArrayToDecimal(argValue) : MainCode.ConvertByteArrayToHex(argValue));
                        transformResult = MainCode.ConvertByteArrayToDecimal(resultBuffer);
                        txtResult.AppendText(opString + ": ");
                        transformResult = MainCode.ByteArrayToString(resultBuffer);
                        txtResult.AppendText(transformResult);
                        txtResult.AppendText(Environment.NewLine);
                    }
                }
                //
                NewUIClip("Transform Bytes");
            } // if()
            oForm = null;
        }

        private void mnuToolsMoveToBuffer_Click(object sender, EventArgs e)
        {
            if (gDestBuffer != null && gDestBuffer.Length > 0)
            {
                gSrcBuffer = new byte[gDestBuffer.Length];
                gDestBuffer.CopyTo(gSrcBuffer, 0);
            }
        }

        private void mnuToolsStringMatchLanguage_Click(object sender, EventArgs e)
        {
            List<string> matches;
            string[] matchesArr;
            string matchStr;
            string keyStr;
            int totalMatches = 0;
            double prob;
            Dictionary<string, int> wordCount;

            if (_scriptDictionary == null)
            {
                _scriptDictionary = RETouchResource.libResx.GetScriptWordDictionary(_scriptDictionary);
            }
            // Match txtResult.Lines
            matches = new List<string>();
            wordCount = new Dictionary<string, int>();
            SetWaitState(true);
            foreach (string s in txtSource.Lines)
            {
                for(int i = 0; i <_scriptDictionary.Keys.Count; i++)
                {
                    keyStr = _scriptDictionary.Keys.ElementAt(i);
                    //if (_scriptDictionary.ContainsKey(s))
                    if (s.IndexOf(keyStr, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        matchStr = _scriptDictionary[keyStr] + ": " + s;
                        if (wordCount.ContainsKey(_scriptDictionary[keyStr]))
                        {
                            wordCount[_scriptDictionary[keyStr]]++;
                        }
                        else
                        {
                            wordCount.Add(_scriptDictionary[keyStr], 1);
                        }
                        if (!matches.Contains(matchStr)) matches.Add(matchStr);
                        totalMatches++;
                    }
                }
            }
            SetWaitState(false);
            if (matches.Count > 0)
            {
                matchesArr = matches.ToArray();
                Array.Sort(matchesArr);
                txtResult.Lines = matchesArr;
                txtResult.AppendText(Environment.NewLine);
                txtResult.AppendText(Environment.NewLine);
                foreach (string key in wordCount.Keys)
                {
                    prob = (double)wordCount[key] / (double)totalMatches;
                    txtResult.AppendText(string.Format("{0}: {1:0.00} %", key, prob * 100));
                    txtResult.AppendText(Environment.NewLine);
                }
                SetStatusText(string.Format("{0} String(s) found", matchesArr.Length), (int)STATUS_STRIP_INDEX.IMP_EXP_INFO);
            }
            else
            {
                txtResult.Text = "No matches";
                txtResult.AppendText(Environment.NewLine);
                SetStatusText("0 String(s) found", (int)STATUS_STRIP_INDEX.IMP_EXP_INFO);
            }
            txtResult.AppendText(Environment.NewLine);
            //
            NewUIClip("Match Language");
        }

        private void mnuToolsStringsMatchDocumentWordList_Click(object sender, EventArgs e)
        {
            List<string> matches;
            string[] matchesArr;
            string matchStr;
            int totalMatches = 0;
            double prob;
            Dictionary<string, int> wordCount;

            if (_documentDictionary == null)
            {
                //for (int i = 0; i < documentWordListFilenames.Length; i++)
                //{
                //    _documentDictionary = MainCode.GetAPIDictionary(documentWordListFilenames[i], _documentDictionary);
                //}
            }
            // Match txtResult.Lines
            matches = new List<string>();
            wordCount = new Dictionary<string, int>();
            SetWaitState(true);
            foreach (string s in txtSource.Lines)
            {
                if (_documentDictionary.ContainsKey(s))
                {
                    matchStr = _documentDictionary[s] + ": " + s;
                    if (wordCount.ContainsKey(_documentDictionary[s]))
                    {
                        wordCount[_documentDictionary[s]]++;
                    }
                    else
                    {
                        wordCount.Add(_documentDictionary[s], 1);
                    }
                    if (!matches.Contains(matchStr)) matches.Add(matchStr);
                    totalMatches++;
                }
            }
            SetWaitState(false);
            if (matches.Count > 0)
            {
                matchesArr = matches.ToArray();
                Array.Sort(matchesArr);
                txtResult.Lines = matchesArr;
                txtResult.AppendText(Environment.NewLine);
                foreach (string key in wordCount.Keys)
                {
                    prob = (double)wordCount[key] / (double)totalMatches;
                    txtResult.AppendText(string.Format("{0}: {1:0.00} %", key, prob * 100));
                    txtResult.AppendText(Environment.NewLine);
                }
                SetStatusText(string.Format("{0} String(s) found", matchesArr.Length), (int)STATUS_STRIP_INDEX.IMP_EXP_INFO);
            }
            else
            {
                txtResult.Text = "No matches";
                txtResult.AppendText(Environment.NewLine);
                SetStatusText("0 String(s) found", (int)STATUS_STRIP_INDEX.IMP_EXP_INFO);
            }
            txtResult.AppendText(Environment.NewLine);
            //
            NewUIClip("Match Document Wordlist");
        }

        private void mnuToolsConvertToBase64_Click(object sender, EventArgs e)
        {
            string convertedString;

            if (gSrcBuffer != null)
            {
                convertedString = Convert.ToBase64String(gSrcBuffer, Base64FormattingOptions.InsertLineBreaks);
                txtResult.Text = convertedString;
                txtResult.AppendText(Environment.NewLine);
                //
                NewUIClip("Convert To Base64");
            }
        }

        private void mnuToolsConvertFromBase64_Click(object sender, EventArgs e)
        {
            string currSelection;
            char[] charBuffer;
            string tempStr;
            string statusMsg;

            currSelection = txtSource.SelectedText;
            statusMsg = "";
            if (!string.IsNullOrEmpty(currSelection))
            {
                try
                {
                    //gSrcBuffer = Convert.FromBase64String(currSelection);
                    gSrcBuffer = MainCode.ConvertFromBase64(currSelection, out statusMsg);
                    charBuffer = Encoding.Unicode.GetChars(gSrcBuffer);
                    tempStr = new string(charBuffer);
                    txtResult.Text = tempStr;
                    txtResult.AppendText(Environment.NewLine);
                    //
                    NewUIClip("Convert From Base64");
                }
                catch (FormatException)
                {
                    txtResult.Text = statusMsg;
                    txtResult.AppendText(Environment.NewLine);
                }
            }
        }

        private void mnuToolsEncodeStringVBEncoding_Click(object sender, EventArgs e)
        {
            string currSelection;

            currSelection = txtSource.SelectedText;
            if (!string.IsNullOrEmpty(currSelection))
            {
                try
                {
                    currSelection = MainCode.VBScriptEncode(currSelection);
                    txtResult.AppendText(currSelection);
                    txtResult.AppendText(Environment.NewLine);
                    //
                    NewUIClip("Encode String VBEncoding");
                }
                catch (Exception ex)
                {
                    txtResult.AppendText(string.Format("Unable to encode: {0}", ex.Message));
                    txtResult.AppendText(Environment.NewLine);
                }
            }
        }

        private void mnuToolsConvertFromUTF16Base64_Click(object sender, EventArgs e)
        {
            string currSelection;
            byte[] byteBuffer;
            char[] charBuffer;

            currSelection = txtSource.SelectedText;
            if(string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            if (!string.IsNullOrEmpty(currSelection))
            {
                try
                {
                    // From wChar
                    byteBuffer = Encoding.Unicode.GetBytes(currSelection);
                    // To 8-bit
                    charBuffer = Encoding.UTF8.GetChars(byteBuffer);
                    currSelection = new string(charBuffer);
                    // Display new encoding
                    txtResult.AppendText(currSelection);
                    txtResult.AppendText(Environment.NewLine);
                    //
                    gSrcBuffer = Convert.FromBase64String(currSelection);
                    //
                    NewUIClip("Convert From Base64");
                }
                catch (FormatException)
                {
                    txtResult.AppendText("Selection is not a valid BASE64 string");
                    txtResult.AppendText(Environment.NewLine);
                }
            }
        }

        private void mnuToolsReplaceChar_Click(object sender, EventArgs e)
        {
            string currSelection;
            //char src;
            //char dest;

            currSelection = txtSource.SelectedText;
            if (!string.IsNullOrEmpty(currSelection))
            {
            }
        }

        // Code copied from private void mnuToolsTransform_Click(object sender, EventArgs e)
        private void mnuToolsTransformText_Click(object sender, EventArgs e)
        {
            MainCode.TRANSFORM_ARG param;
            frmPromptUserInput oForm;
            string currSelection;
            //string transformResult;
            //string opString;
            string buffer;
            byte[] paramBuffer;
            //int paramStartValue;
            //int paramEndValue;
            string resultBuffer;
            //byte[] argValue;

            // Get data first
            currSelection = txtSource.SelectedText;
            if(string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            if (!string.IsNullOrEmpty(currSelection))
            {
                //buffer = MainCode.ParseStringToByteArray(currSelection);
                buffer = currSelection;
            }
            else
            {
                if (gSrcBuffer == null || gSrcBuffer.Length < 1) return;
                buffer = new string(Encoding.ASCII.GetChars(gSrcBuffer));
            }
            // Prompt op and arg
            oForm = new frmPromptUserInput();
            param = new MainCode.TRANSFORM_ARG();
            oForm.monoSpaceFont = _monoSpaceFont;
            oForm.transformArg = param;
            //oForm.commandHistory = _commandHistory;
            oForm.ShowDialog();
            if (oForm.FormRetValue == MainCode.FORM_RETURN_VALUE.OK)
            {
                //_commandHistory = oForm.commandHistory;
                param = oForm.transformArg;
                //paramStartValue = 0;
                //paramEndValue = param.args.Length;
                paramBuffer = new byte[param.args.Length];
                param.args.CopyTo(paramBuffer, 0);
                if (buffer.Length > 0)
                {
                    resultBuffer = MainCode.TransformText(buffer, param);
                    txtResult.AppendText(resultBuffer);
                    txtResult.AppendText(Environment.NewLine);
                }
                //
                NewUIClip("Transform Text");
            } // if()
            oForm = null;
        }

        private void mnuToolsScript_Click(object sender, EventArgs e)
        {
            frmScript oForm;
            StreamReader stdoutReader;
            //StreamWriter stdinWriter;
            string args;
            string currSelection;

            currSelection = txtSource.SelectedText;
            if(string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            oForm = new frmScript();
            oForm.scriptColl = _scriptCollection;
            oForm.ShowDialog();
            if (oForm.FormRetValue == MainCode.FORM_RETURN_VALUE.OK)
            {
                args = "";
                if (string.IsNullOrEmpty(oForm.selectedScript.ScriptArgs))
                {
                    
                    args = currSelection; // + Convert.ToChar(26);
                }
                else
                {
                    args = oForm.selectedScript.ScriptArgs.Replace("[FILE]", _argFilename);
                    if (args.Contains(" "))
                    {
                        args = '"' + args + '"';
                    }
                }
                //MainCode.StartExternalScript(@"cmd.exe", "/C " + oForm.selectedScript.ScriptFilename
                //    + " " + args, "", out stdoutReader);
                MainCode.StartExternalScript(@"cmd.exe", "/C " + oForm.selectedScript.ScriptFilename
                    + " " + args, args, out stdoutReader);
                //
                //currSelection = stdoutReader.ReadToEnd();
                currSelection = stdoutReader.ReadLine();
                stdoutReader.Close();
                stdoutReader = null;
                if (!string.IsNullOrEmpty(currSelection))
                {
                    txtResult.AppendText(currSelection);
                    txtResult.AppendText(Environment.NewLine);
                }
                else
                {
                    txtResult.AppendText("<null>");
                    txtResult.AppendText(Environment.NewLine);
                }
                NewUIClip("External Script");
            }
            oForm = null;
        }

        private void mnuToolsSplitInput_Click(object sender, EventArgs e)
        {
            frmPromptUserInput oForm;
            string currSelection;
            string oldStr;
            string newStr;

            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            // Prompt user for split character
            oForm = new frmPromptUserInput();
            oForm.monoSpaceFont = _monoSpaceFont;
            oForm.transformArg = new MainCode.TRANSFORM_ARG();
            oForm.ShowDialog();
            if (oForm.FormRetValue == MainCode.FORM_RETURN_VALUE.OK)
            {
                if (oForm.transformArg.args.Length < 1) return; // Empty args
                //
                oldStr = new string(new char[] { (char)oForm.transformArg.args[0] });
                newStr = Environment.NewLine;
                currSelection = currSelection.Replace(oldStr, newStr);
                txtResult.Text = currSelection;
                //
                NewUIClip("Split Text");
            }
        }

        private void mnuToolsCscript_Click(object sender, EventArgs e)
        {
            string currSelection;
            StreamReader stdoutReader;
            //StreamWriter stdinWriter;
            //string args;
            string tempFilename;

            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            // Write currselection to tempfile
            tempFilename = Path.GetTempFileName();
            tempFilename = tempFilename + ".vbs";
            File.WriteAllText(tempFilename, currSelection);
            MainCode.StartExternalScript(@"cscript.exe", "//NOLOGO " + tempFilename, "", out stdoutReader);
            //
            //currSelection = stdoutReader.ReadToEnd();
            currSelection = "";
            while (!stdoutReader.EndOfStream)
            {
                currSelection += stdoutReader.ReadLine();
            }
            stdoutReader.Close();
            stdoutReader = null;
            if (File.Exists(tempFilename)) File.Delete(tempFilename);
            if (!string.IsNullOrEmpty(currSelection))
            {
                //txtSource.AppendText(Environment.NewLine);
                txtResult.Text = currSelection;
                txtResult.AppendText(Environment.NewLine);
            }
            else
            {
                //txtSource.AppendText(Environment.NewLine);
                txtResult.Text = "<null>";
                txtResult.AppendText(Environment.NewLine);
            }
            //
            NewUIClip("Execute Cscript");
        }

        private void mnuToolsPowershell_Click(object sender, EventArgs e)
        {
            string currSelection;
            string[] stdout;

            // See https://stackoverflow.com/questions/30120301/capture-standart-output-of-powershell-script-executed-from-net-application

            currSelection = txtSource.SelectedText;
            if (string.IsNullOrEmpty(currSelection))
            {
                currSelection = txtSource.Text;
            }
            SetWaitState(true);
            MainCode.StartExternalPowershell(currSelection, out stdout);
            //
            currSelection = "";
            foreach (string s in stdout)
            {
                currSelection += s + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(currSelection))
            {
                //txtSource.AppendText(Environment.NewLine);
                txtResult.Text = currSelection;
                txtResult.AppendText(Environment.NewLine);
            }
            else
            {
                //txtSource.AppendText(Environment.NewLine);
                txtResult.Text = "<null>";
                txtResult.AppendText(Environment.NewLine);
            }
            SetWaitState(false);
            //
            NewUIClip("Execute Powershell");
        }

        private void mnuToolsOptions_Click(object sender, EventArgs e)
        {
            frmOptions oForm;

            oForm = new frmOptions();
            oForm.UserSettingsColl = _Settings;
            oForm.ShowDialog();
            _Settings = oForm.UserSettingsColl;
            _settingsIsDirty = true;
            oForm = null;
        }

        #endregion

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            libAboutBox.libAboutBox oForm;

            oForm = new libAboutBox.libAboutBox();
            oForm.ShowDesciption = "Malware deobfuscation tool";
            oForm.ShowDialog();
            oForm = null;
        }

        #region // Toolstrip, Context menu and control event handlers

        private void OnToolStrip_Click(object sender, EventArgs e)
        {
            byte[] buffer;
            char[] charBuffer;
            string currSelection;
            string tempStr;

            currSelection = txtSource.SelectedText;
            switch (((ToolStripItem)sender).Name)
            {
                case "Copy":
                    {
                        if (!string.IsNullOrEmpty(currSelection))
                        {
                            Clipboard.SetText(currSelection);
                        }
                        else
                        {
                            Clipboard.SetText(txtSource.Text);
                        }
                        break;
                    }
                case "CopyBinary":
                    {
                        if (!string.IsNullOrEmpty(currSelection))
                        {
                            buffer = MainCode.GetBytes(currSelection, _encoding);
                            Clipboard.SetDataObject(buffer, false);
                        }
                        else
                        {
                            buffer = MainCode.GetBytes(txtSource.Text, _encoding);
                            Clipboard.SetDataObject(buffer, false);
                        }
                        break;
                    }
                case "Cut":
                    {
                        if (!string.IsNullOrEmpty(currSelection))
                        {
                            Clipboard.SetText(currSelection);
                            txtSource.SelectedText = ""; // Cut
                        }
                        else
                        {
                            Clipboard.SetText(txtSource.Text);
                            txtSource.Text = ""; // Cut
                        }
                        SetStatusText(string.Format("Lines {0}, Chars {1}, Selected Chars {2}",
                            txtSource.Lines.Length, txtSource.Text.Length, txtSource.SelectedText.Length),
                            (int)STATUS_STRIP_INDEX.SRC_INFO);
                        break;
                    }
                case "Paste":
                    {
                        if (!string.IsNullOrEmpty(Clipboard.GetText()))
                        {
                            txtSource.Paste();
                            SetStatusText(string.Format("Lines {0}, Chars {1}, Selected Chars {2}",
                                txtSource.Lines.Length, txtSource.Text.Length, txtSource.SelectedText.Length),
                                (int)STATUS_STRIP_INDEX.SRC_INFO);
                        }
                        break;
                    }
                case "Clear":
                    {
                        txtSource.Clear();
                        SetStatusText("", (int)STATUS_STRIP_INDEX.IMP_EXP_INFO);
                        SetStatusText(string.Format("Lines {0}, Chars {1}, Selected Chars {2}",
                            txtSource.Lines.Length, txtSource.Text.Length, txtSource.SelectedText.Length),
                            (int)STATUS_STRIP_INDEX.SRC_INFO);
                        break;
                    }
                case "Checksum":
                    {
                        txtSource.SelectionBackColor = _warningBackColor;
                        mnuToolsChecksums_Click(sender, e);
                        txtSource.SelectionBackColor = _normalBackColor;
                        break;
                    }
                case "Strings":
                    {
                        mnuToolsStringsExtract_Click(sender, e);
                        break;
                    }
                case "Numbers": // Change default numeric display format
                    {
                        _Settings.DisplayNumberFormatDecimal = !_Settings.DisplayNumberFormatDecimal;
                        SetStatusText(_Settings.DisplayNumberFormatDecimal ? "Decimal" : "Hex", (int)STATUS_STRIP_INDEX.DEC_HEX_DISPLAY);
                        _settingsIsDirty = true;
                        break;
                    }
                case "Encoding":
                    {
                        if (_encoding == MainCode.TEXT_ENCODING.Default)
                        {
                            if(!string.IsNullOrEmpty(currSelection))
                            {
                                buffer = MainCode.GetBytes(currSelection, MainCode.TEXT_ENCODING.Default);
                                charBuffer = MainCode.GetChars(buffer, MainCode.TEXT_ENCODING.ASCII);
                                txtSource.SelectedText = new string(charBuffer);
                            }
                            else
                            {
                                buffer = MainCode.GetBytes(txtSource.Text, MainCode.TEXT_ENCODING.Default);
                                charBuffer = MainCode.GetChars(buffer, MainCode.TEXT_ENCODING.ASCII);
                                txtSource.Text = new string(charBuffer);
                            }
                            //
                            NewUIClip("Text Encode to ASCII");
                            _encoding = MainCode.TEXT_ENCODING.ASCII;
                            SetStatusText("Text Encoding: ASCII", (int)STATUS_STRIP_INDEX.TEXT_ENCODING);
                        }
                        else if (_encoding == MainCode.TEXT_ENCODING.ASCII)
                        {
                            if (!string.IsNullOrEmpty(currSelection))
                            {
                                buffer = MainCode.GetBytes(currSelection, MainCode.TEXT_ENCODING.ASCII);
                                charBuffer = MainCode.GetChars(buffer, MainCode.TEXT_ENCODING.UTF8);
                                txtSource.SelectedText = new string(charBuffer);
                            }
                            else
                            {
                                buffer = MainCode.GetBytes(txtSource.Text, MainCode.TEXT_ENCODING.ASCII);
                                charBuffer = MainCode.GetChars(buffer, MainCode.TEXT_ENCODING.UTF8);
                                txtSource.Text = new string(charBuffer);
                            }
                            //
                            NewUIClip("Text Encode to UTF8");
                            _encoding = MainCode.TEXT_ENCODING.UTF8;
                            SetStatusText("Text Encoding: UTF8", (int)STATUS_STRIP_INDEX.TEXT_ENCODING);
                        }
                        else if (_encoding == MainCode.TEXT_ENCODING.UTF8)
                        {
                            if (!string.IsNullOrEmpty(currSelection))
                            {
                                buffer = MainCode.GetBytes(currSelection, MainCode.TEXT_ENCODING.UTF8);
                                charBuffer = MainCode.GetChars(buffer, MainCode.TEXT_ENCODING.UNICODE);
                                txtSource.SelectedText = new string(charBuffer);
                            }
                            else
                            {
                                buffer = MainCode.GetBytes(txtSource.Text, MainCode.TEXT_ENCODING.UTF8);
                                charBuffer = MainCode.GetChars(buffer, MainCode.TEXT_ENCODING.UNICODE);
                                txtSource.Text = new string(charBuffer);
                            }
                            //
                            NewUIClip("Text Encode to Unicode");
                            _encoding = MainCode.TEXT_ENCODING.UNICODE;
                            SetStatusText("Text Encoding: Unicode", (int)STATUS_STRIP_INDEX.TEXT_ENCODING);
                        }
                        else if (_encoding == MainCode.TEXT_ENCODING.UNICODE)
                        {
                            if (!string.IsNullOrEmpty(currSelection))
                            {
                                buffer = MainCode.GetBytes(currSelection, MainCode.TEXT_ENCODING.UNICODE);
                                charBuffer = MainCode.GetChars(buffer, MainCode.TEXT_ENCODING.UTF32);
                                txtSource.SelectedText = new string(charBuffer);
                            }
                            else
                            {
                                buffer = MainCode.GetBytes(txtSource.Text, MainCode.TEXT_ENCODING.UNICODE);
                                charBuffer = MainCode.GetChars(buffer, MainCode.TEXT_ENCODING.UTF32);
                                txtSource.Text = new string(charBuffer);
                            }
                            //
                            NewUIClip("Text Encode to UTF32");
                            _encoding = MainCode.TEXT_ENCODING.UTF32;
                            SetStatusText("Text Encoding: UTF32", (int)STATUS_STRIP_INDEX.TEXT_ENCODING);
                        }
                        else if (_encoding == MainCode.TEXT_ENCODING.UTF32)
                        {
                            if (!string.IsNullOrEmpty(currSelection))
                            {
                                buffer = MainCode.GetBytes(currSelection, MainCode.TEXT_ENCODING.UTF32);
                                charBuffer = MainCode.GetChars(buffer, MainCode.TEXT_ENCODING.Default);
                                txtSource.SelectedText = new string(charBuffer);
                            }
                            else
                            {
                                buffer = MainCode.GetBytes(txtSource.Text, MainCode.TEXT_ENCODING.UTF32);
                                charBuffer = MainCode.GetChars(buffer, MainCode.TEXT_ENCODING.Default);
                                txtSource.Text = new string(charBuffer);
                            }
                            //
                            NewUIClip("Text Encode to Default");
                            _encoding = MainCode.TEXT_ENCODING.Default;
                            SetStatusText("Text Encoding: Default", (int)STATUS_STRIP_INDEX.TEXT_ENCODING);
                        }
                        SetStatusText(string.Format("Lines {0}, Chars {1}, Selected Chars {2}",
                            txtSource.Lines.Length, txtSource.Text.Length, txtSource.SelectedText.Length),
                            (int)STATUS_STRIP_INDEX.SRC_INFO);
                        break;
                    }
                case "StringEncoding":
                    {
                        if (string.IsNullOrEmpty(txtSource.SelectedText)) return;
                        switch (((ToolStripComboBox)sender).SelectedIndex)
                        {
                            case 0: // Default
                                {
                                    buffer = Encoding.Default.GetBytes(txtSource.SelectedText);
                                    charBuffer = Encoding.Default.GetChars(buffer);
                                    tempStr = new string(charBuffer);
                                    txtSource.AppendText(tempStr);
                                    txtSource.AppendText(Environment.NewLine);
                                    //
                                    NewUIClip("String Encode to Default");
                                    break;
                                }
                            case 1: // ASCII
                                {
                                    buffer = Encoding.Default.GetBytes(txtSource.SelectedText);
                                    charBuffer = Encoding.ASCII.GetChars(buffer);
                                    tempStr = new string(charBuffer);
                                    txtSource.AppendText(tempStr);
                                    txtSource.AppendText(Environment.NewLine);
                                    //
                                    NewUIClip("String Encode to ASCII");
                                    break;
                                }
                            case 2: // UTF-8
                                {
                                    buffer = Encoding.Default.GetBytes(txtSource.SelectedText);
                                    charBuffer = Encoding.UTF8.GetChars(buffer);
                                    tempStr = new string(charBuffer);
                                    txtSource.AppendText(tempStr);
                                    txtSource.AppendText(Environment.NewLine);
                                    //
                                    NewUIClip("String Encode to UTF8");
                                    break;
                                }
                            case 3: // Unicode
                                {
                                    buffer = Encoding.Default.GetBytes(txtSource.SelectedText);
                                    charBuffer = Encoding.Unicode.GetChars(buffer);
                                    tempStr = new string(charBuffer);
                                    txtSource.AppendText(tempStr);
                                    txtSource.AppendText(Environment.NewLine);
                                    //
                                    NewUIClip("String Encode to Unicode");
                                    break;
                                }
                            case 4: // UTF32
                                {
                                    buffer = Encoding.Default.GetBytes(txtSource.SelectedText);
                                    charBuffer = Encoding.UTF32.GetChars(buffer);
                                    tempStr = new string(charBuffer);
                                    txtSource.AppendText(tempStr);
                                    txtSource.AppendText(Environment.NewLine);
                                    //
                                    NewUIClip("String Encode to UTF32");
                                    break;
                                }
                        }
                        SetStatusText(string.Format("Lines {0}, Chars {1}, Selected Chars {2}",
                            txtSource.Lines.Length, txtSource.Text.Length, txtSource.SelectedText.Length),
                            (int)STATUS_STRIP_INDEX.SRC_INFO);
                        break;
                    }
                case "Back":
                    {
                        lstClip.SelectedIndex--;
                        if (lstClip.SelectedIndex < 0) lstClip.SelectedIndex = oClipColl.Count() - 1;
                        if (lstClip.SelectedIndex >= 0)
                        {
                            txtSource.Text = oClipColl.Item(lstClip.SelectedIndex).ClipContentSource;
                            txtResult.Text = oClipColl.Item(lstClip.SelectedIndex).ClipContentResult;
                            SetStatusText(oClipColl.Item(lstClip.SelectedIndex).ClipTitle, (int)STATUS_STRIP_INDEX.HISTORY_TITLE);
                        }
                        SetMenuState(1);
                        SetStatusText(string.Format("Lines {0}, Chars {1}, Selected Chars {2}",
                            txtSource.Lines.Length, txtSource.Text.Length, txtSource.SelectedText.Length),
                            (int)STATUS_STRIP_INDEX.SRC_INFO);
                        break;
                    }
                case "Forward":
                    {
                        lstClip.SelectedIndex++;
                        if (lstClip.SelectedIndex >= oClipColl.Count()) lstClip.SelectedIndex = 0;
                        if (lstClip.SelectedIndex >= 0)
                        {
                            txtSource.Text = oClipColl.Item(lstClip.SelectedIndex).ClipContentSource;
                            txtResult.Text = oClipColl.Item(lstClip.SelectedIndex).ClipContentResult;
                            SetStatusText(oClipColl.Item(lstClip.SelectedIndex).ClipTitle, (int)STATUS_STRIP_INDEX.HISTORY_TITLE);
                        }
                        SetMenuState(1);
                        SetStatusText(string.Format("Lines {0}, Chars {1}, Selected Chars {2}",
                            txtSource.Lines.Length, txtSource.Text.Length, txtSource.SelectedText.Length),
                            (int)STATUS_STRIP_INDEX.SRC_INFO);
                        break;
                    }
                case "MoveUp":
                    {
                        txtSource.Text = txtResult.Text;
                        txtResult.Text = "";
                        break;
                    }
            } // switch()
        }

        private void OnContextMenu_Click(object sender, EventArgs e)
        {
            switch (((ToolStripMenuItem)sender).Name)
            {
                case "Copy":
                    {
                        txtSource.Copy();
                        break;
                    }
                case "Cut":
                    {
                        txtSource.Cut();
                        SetStatusText(string.Format("Lines {0}, Chars {1}, Selected Chars {2}",
                            txtSource.Lines.Length, txtSource.Text.Length, txtSource.SelectedText.Length),
                            (int)STATUS_STRIP_INDEX.SRC_INFO);
                        break;
                    }
                case "Paste":
                    {
                        txtSource.Paste();
                        SetStatusText(string.Format("Lines {0}, Chars {1}, Selected Chars {2}",
                            txtSource.Lines.Length, txtSource.Text.Length, txtSource.SelectedText.Length),
                            (int)STATUS_STRIP_INDEX.SRC_INFO);
                        break;
                    }
            }
        }

        private void OnTextChanged_Click(object sender, EventArgs e)
        {
            SetStatusText(string.Format("Lines {0}, Chars {1}, Selected Chars {2}",
                txtSource.Lines.Length, txtSource.Text.Length, txtSource.SelectedText.Length),
                (int)STATUS_STRIP_INDEX.SRC_INFO);
        }

        private void lstClip_SelectedIndexChanged(object sender, EventArgs e)
        {
            libClipItem currItem;
            int itemID;
            string tempStr;

            if (lstClip.SelectedIndex >= 0)
            {
                tempStr = lstClip.Items[lstClip.SelectedIndex].ToString().Replace("Clip", "");
                if (tempStr.IndexOf(" ") > 0) tempStr = tempStr.Substring(0, tempStr.IndexOf(" "));
                if (int.TryParse(tempStr, out itemID))
                {
                    currItem = oClipColl.ItemById(itemID);
                    //Update
                    if (currItem != null)
                    {
                        txtSource.Text = oClipColl.Item(lstClip.SelectedIndex).ClipContentSource;
                        txtResult.Text = oClipColl.Item(lstClip.SelectedIndex).ClipContentResult;
                        if (currItem.ClipState != null)
                        {
                            for (int i = 0; i < currItem.ClipState.Count; i++)
                            {
                                SetStatusText(currItem.ClipState[i], i);
                            }
                        }
                    }
                }
            }
        }
        
        #endregion

        // TODO: Script management

        //--------------------------------------------------------
        // Public procedures
        //--------------------------------------------------------

    } // Class frmMain
} // Namespace


