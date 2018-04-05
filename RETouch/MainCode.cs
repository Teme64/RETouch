using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

using CaseWare;

using System.Management.Automation; // Powershell

namespace RETouch
{
    public static class MainCode
    {
        //--------------------------------------------------------
        // MainCode.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // MainCode Code Library
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

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------
        
        // 12 first bytes of encoded VBScript
        // 0x23 0x40 0x7E 0x5E 0x45 0x41 0x41 0x41 0x41 0x41 0x3D 0x3D
        // #@~^EAAAAA==

        public struct SupportedApplications
        {
            public string appExecutable;
            public string appName;
            public string arguments;
            public bool shellExecute;
            public bool useStdout;
            public override string ToString() { return appName; }
        }

        public enum TRANSFORM_OP : int
        {
            NOP = 0,
            Xor = 1,
            Rot = 2,
            Plus = 3,
            Minus = 4,
            ShiftLeft = 5,
            ShiftRight = 6,
            Replace = 7,
            Split = 8,
            Swap = 9,
            Caesar = 10,
        }

        public struct TRANSFORM_ARG
        {
            public TRANSFORM_OP opCode;
            public TRANSFORM_ALPHABET alphabet;
            public byte[] args;
        }

        public enum TRANSFORM_ALPHABET : int
        {
            DEFAULT = 0, // 0-255
            BIT8 = 1, // 0-255
            LOWERCASE = 2, // abcdefghijklmnopqrstuvwxyz -> 26 chars
            UPPERCASE = 3, // 26 chars
            LETTERS = 4, // 52 chars
            DIGITS = 5, // 10 chars
            ASCII = 6 // 0-127
        }

        public enum FORM_RETURN_VALUE : int
        {
            CANCEL = 0,
            OK = 1
        }

        public enum FORM_VIEW_TABLE : int
        {
            ASCII = 0
        }

        // Constant values
        public static byte[] UTF8_BOM = new byte[] { 0xEF, 0xBB, 0xBF };
        public static byte[] UTF16_LE_BOM = new byte[] { 0xFF, 0xFE };
        public static byte[] UTF16_BE_BOM = new byte[] { 0xFE, 0xFF };

        public enum STRING_FORMAT : int
        {
            Unknown = 0,
            BINARY = 1,
            OCTAL = 2,
            DECIMAL = 4,
            HEX = 8,
            ARRAY = 16
        }

        public enum NUMBER_FORMAT : int
        {
            Unknown = 0,
            BYTE = 1,
            INT16 = 2,
            INT32 = 4,
            INT64 = 8,
            ARRAY = 16
        }

        public enum TEXT_ENCODING : int
        {
            Default = 0,
            ASCII = 1,
            UTF8 = 2,
            UNICODE = 3,
            UTF32 = 4
        }

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        //public MainCode()
        //{
        //}

        //~MainCode()
        //{
        //}

        //--------------------------------------------------------
        // Private procedures
        //--------------------------------------------------------

        private static string ConvertIntToBinary(int value, int size = 32)
        {
            string newString = "";
            uint mask = 0x80000000;

            if (size == 32)
            {
                mask = 0x80000000;
            }
            else if (size == 16)
            {
                mask = 0x8000;
            }
            else if (size == 8)
            {
                mask = 0x80;
            }
            for (int i = 0; i < size; i++)
            {
                newString += (value & mask) == mask ? "1" : "0";
                mask >>= 1;
            }
            //
            return newString;
        }

        private static string ConvertIntToOctal(int value, int size = 32)
        {
            string newString = "";
            uint mask = 0x00000888;

            if (size == 32)
            {
                mask = 0x00000888;
                size = 3;
            }
            else if (size == 16)
            {
                mask = 0x0888;
                size = 2;
            }
            else if (size == 8)
            {
                mask = 0x888;
                size = 1;
            }
            for (int i = 0; i < size; i++)
            {
                newString += (value & mask).ToString();
                mask <<= 3;
            }
            //
            return newString;
        }

        private static int ConvertHexToInt(string value)
        {
            string HEX_CHARS = "0123456789ABCDEF";
            int tempPos;
            int tempInt = 0;

            for(int i = 0; i < value.Length; i++)
            {
                tempPos = HEX_CHARS.IndexOf(value[i]);
                if (tempPos < 0) return tempInt;
                tempInt = tempInt * 16 + tempPos;
            }
            //
            return tempInt;
        }

        private static string ConvertIntToHex(int value, int size = 32)
        {
            string newString = "0x";
            uint mask = 0xFF000000;

            if (size == 32)
            {
                mask = 0xFF000000;
                size = 4;
            }
            else if (size == 16)
            {
                mask = 0xFF00;
                size = 2;
            }
            else if (size == 8)
            {
                mask = 0xFF;
                size = 1;
            }
            for (int i = 0; i < size; i++)
            {
                newString += string.Format("{0:X2}", (value & mask) > 0 ? (value & mask) >> (3 - i) : 0);
                mask >>= 8;
            }
            //
            return newString;
        }

        /// <summary>
        /// Converts a string of binary values to integers
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        /// <remarks>It is assumed that input is a continuous string of bits</remarks>
        private static List<int> ConvertBinaryToInt(string value, int size = 8)
        {
            string BIN_CHARS = "01";
            int tempPos;
            int tempInt = 0;
            int sizeIX = 0;
            List<int> intList;

            intList = new List<int>();
            if (size < 1) size = 1;
            for (int i = 0; i < value.Length; i++)
            {
                tempPos = BIN_CHARS.IndexOf(value[i]);
                if (tempPos < 0) return intList;
                tempInt = tempInt * 2 + tempPos;
                sizeIX++;
                if(sizeIX == size)
                {
                    intList.Add(tempInt);
                    tempInt = 0;
                    sizeIX = 0;
                }
            }
            if(sizeIX > 0) // Last part
            {
                intList.Add(tempInt);
            }
            //
            return intList;
        }

        private static string[] GetSplitChars(string userDefChar = "")
        {
            List<string> charArr;

            charArr = new List<string> { @" ", @",", @"\x", @"0x" };
            if (!string.IsNullOrEmpty(userDefChar)) charArr.Add(userDefChar);
            //
            return charArr.ToArray();
        }

        //--------------------------------------------------------
        // Event handlers
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public procedures
        //--------------------------------------------------------

        public static bool StartExternalApplication(SupportedApplications thisApp, string appPath, string argFilename,
            out StreamReader stdoutReader, bool useShell = false)
        {
            Process oProcess;
            ProcessStartInfo oStartInfo;
            bool isOk;

            isOk = false;
            stdoutReader = null;
            oStartInfo = new ProcessStartInfo(appPath, argFilename);
            oStartInfo.UseShellExecute = thisApp.shellExecute;
            oStartInfo.RedirectStandardOutput = thisApp.useStdout;
            //
            oProcess = new Process();
            oProcess.StartInfo = oStartInfo;
            // Should check if success or not
            try
            {
                isOk = oProcess.Start();
                if(thisApp.useStdout)
                { 
                    stdoutReader = oProcess.StandardOutput;
                }

            }
            catch(System.ComponentModel.Win32Exception)
            {
                isOk = false;
                // Need to elevate user
            }
            return isOk;
        }

        /// <summary>
        /// Execute external script
        /// </summary>
        /// <param name="appPath">Script context, usually cmd.exe or cscript.exe</param>
        /// <param name="argFilename">Script file's name</param>
        /// <param name="stdinArg">Stdin input</param>
        /// <param name="stdoutReader">Returns stdout stream</param>
        /// <returns>Returns true if script was started succesfully</returns>
        public static bool StartExternalScript(string appPath, string argFilename, string stdinArg,
            out StreamReader stdoutReader)
        {
            Process oProcess;
            ProcessStartInfo oStartInfo;
            bool isOk;
            StreamWriter stdinWriter;

            isOk = false;
            stdoutReader = null;
            oStartInfo = new ProcessStartInfo(appPath, argFilename);
            oStartInfo.UseShellExecute = false;
            oStartInfo.RedirectStandardInput = true;
            oStartInfo.RedirectStandardOutput = true;
            //
            oProcess = new Process();
            oProcess.StartInfo = oStartInfo;
            // Should check if success or not
            try
            {
                isOk = oProcess.Start();
                if (!string.IsNullOrEmpty(stdinArg))
                {
                    stdinWriter = oProcess.StandardInput;
                    stdinWriter.WriteLine(stdinArg);
                    stdinWriter.Close();
                }
                stdoutReader = oProcess.StandardOutput;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                isOk = false;
                // Need to elevate user
            }
            return isOk;
        }

        public static bool StartExternalPowershell(string scriptCode, out string[] stdout)
        {
            bool isOk;
            List<string> stdoutList;
            PowerShell oPSProcess;

            isOk = false;
            oPSProcess = PowerShell.Create();
            oPSProcess.AddScript(scriptCode);
            stdoutList = new List<string>();
            try
            { 
                var results = oPSProcess.Invoke();
                foreach (var psObject in results)
                {
                    stdoutList.Add(psObject.BaseObject.ToString());
                }
                isOk = true;
            }
            catch
            {

            }
            stdout = stdoutList.ToArray();
            //
            return isOk;
        }

        /// <summary>
        /// Reads a list of supported external applications from a configuration file
        /// </summary>
        /// <param name="cfgFilename">Configuration file's name</param>
        /// <returns>List of supported applications</returns>
        public static List<SupportedApplications> GetSupportedApplications(string cfgFilename)
        {
            List<SupportedApplications> newList;
            SupportedApplications newApp;
            string[] allLines;
            string[] oneLine;
            char[] splitChar;

            newList = new List<SupportedApplications>();
            //
            splitChar = new char[] { ';' };
            if (File.Exists(cfgFilename))
            {
                allLines = File.ReadAllLines(cfgFilename);
                for(int i = 0; i < allLines.Length; i++)
                {
                    if (allLines[i].StartsWith("#")) continue; // Comment line, skip it
                    oneLine = allLines[i].Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
                    if(oneLine.Length >= 5)
                    {
                        newApp = new SupportedApplications();
                        newApp.appName = oneLine[0];
                        newApp.appExecutable = oneLine[1];
                        newApp.arguments = oneLine[2];
                        newApp.shellExecute = bool.Parse(oneLine[3]);
                        newApp.useStdout = bool.Parse(oneLine[4]);
                        newList.Add(newApp);
                    }
                }
            }
            //
            return newList;
        }

        public static string Attributes2String(FileAttributes value)
        {
            string attributes = "";

            attributes += (value & FileAttributes.Archive) == FileAttributes.Archive ? "A" : "";
            attributes += (value & FileAttributes.Compressed) == FileAttributes.Compressed ? "C" : "";
            attributes += (value & FileAttributes.Encrypted) == FileAttributes.Encrypted ? "E" : "";
            attributes += (value & FileAttributes.Hidden) == FileAttributes.Hidden ? "H" : "";
            attributes += (value & FileAttributes.System) == FileAttributes.System ? "S" : "";
            attributes += (value & FileAttributes.ReadOnly) == FileAttributes.ReadOnly ? "R" : "";
            //
            return attributes;
        }

        public static string FileSize2String(long value)
        {
            string size = "";

            if (value > (1024 * 1024 * 1024))
            {
                size = (value / (1024 * 1024 * 1024)).ToString() + " GB";
            }
            else if (value > (1024 * 1024))
            {
                size = (value / (1024 * 1024)).ToString() + " MB";
            }
            else if (value > 1024)
            {
                size = (value / 1024).ToString() + " KB";
            }
            else
            {
                size = value.ToString() + " B";
            }

            //
            return size;
        }

        // https://www.google.com/search?q=csharp+idataobject
        //ext:[FILEEXT]
        //inurl:[URLSTRING]
        //intitle:[TITLESTRING]
        //intext:[TEXTSTRING]
        public static string GetGoogleQueryUrl(string searchTerms)
        {
            string urlStr;
            string qTerms;

            // TODO: should be escaped properly
            urlStr = @"https://www.google.com/search?q=[QUERY]";
            qTerms = searchTerms.Replace(" ", "+");
            urlStr = urlStr.Replace(@"[QUERY]", qTerms);
            //
            return urlStr;
        }

        // https://social.msdn.microsoft.com/search/en-US/windows?query=GetSystemTimeAsFileTime
        public static string GetMSDNQueryUrl(string searchTerms)
        {
            string urlStr;
            string qTerms;

            // TODO: should be escaped properly
            urlStr = @"https://social.msdn.microsoft.com/search/en-US/windows?query=[QUERY]";
            qTerms = searchTerms.Replace(" ", "+");
            urlStr = urlStr.Replace(@"[QUERY]", qTerms);
            //
            return urlStr;
        }

        /// <summary>
        /// Load user's preferences
        /// </summary>
        /// <param name="filename">Settings file</param>
        /// <param name="oSettings">Settings collection</param>
        public static void LoadUserSettings(string filename, out UserSettings oSettings)
        {
            BinaryFormatter oReader;
            MemoryStream ms;
            BinaryFormatter oWriter;

            oSettings = new UserSettings();
            if (File.Exists(filename))
            {
                oReader = new BinaryFormatter();
                ms = new MemoryStream(File.ReadAllBytes(filename));
                oSettings = (UserSettings)oReader.Deserialize(ms);
                ms = null;
                oReader = null;
            }
            else // Use default values
            {
                // Create file
                oWriter = new BinaryFormatter();
                ms = new MemoryStream();
                oWriter.Serialize(ms, oSettings);
                File.WriteAllBytes(filename, ms.ToArray());
                ms = null;
                oWriter = null;
            }
        }

        /// <summary>
        /// Save user's preferences
        /// </summary>
        /// <param name="filename">Settings file</param>
        /// <param name="oSettings">Settings collection</param>
        public static void SaveUserSettings(string filename, UserSettings oSettings)
        {
            BinaryFormatter oWriter;
            MemoryStream ms;

            oWriter = new BinaryFormatter();
            ms = new MemoryStream();
            oWriter.Serialize(ms, oSettings);
            File.WriteAllBytes(filename, ms.ToArray());
            ms = null;
            oWriter = null;
        }

        public static string ByteArrayToString(byte[] buffer)
        {
            StringBuilder sb;
            char c;

            sb = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                c = (char)buffer[i];
                if(char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsSymbol(c))
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append('.');
                }
            }
            //
            return sb.ToString();
        }

        // See https://en.wikipedia.org/wiki/Byte_order_mark for BOM
        //  The UTF-8 representation of the BOM is the (hexadecimal) byte sequence 0xEF,0xBB,0xBF
        //      If the 16-bit units are represented in big-endian byte order, this BOM character will appear in the sequence of bytes as 0xFE followed by 0xFF
        //      if the 16-bit units use little-endian order, the sequence of bytes will have 0xFF followed by 0xFE


        public static byte[] TransformBlob(byte[] buffer, byte[] transformBuffer, TRANSFORM_OP opCode, int alphabetSize = 0)
        {
            byte[] newBuffer;
            int loopIndex;
            byte tempByte;

            if (buffer == null || buffer.Length < 1)
            {
                return new byte[] { };
            }
            if (transformBuffer == null || transformBuffer.Length < 1)
            {
                transformBuffer = new byte[] { 0 };
            }
            newBuffer = new byte[buffer.Length];
            loopIndex = 0;
            //
            switch(opCode)
            {
                case TRANSFORM_OP.Xor:
                    {
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            newBuffer[i] = (byte)(buffer[i] ^ transformBuffer[loopIndex++]);
                            if (loopIndex >= transformBuffer.Length) loopIndex = 0;
                        }
                        break;
                    }
                case TRANSFORM_OP.Rot:
                    {
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            newBuffer[i] = (byte)(buffer[i] + transformBuffer[loopIndex++]);
                            if (loopIndex >= transformBuffer.Length) loopIndex = 0;
                        }
                        break;
                    }
                case TRANSFORM_OP.Caesar:
                    {   // Takes three args: alphabet size, base value ie. char A numeric value and rot value
                        libCoding.AlphabetSize = transformBuffer[0];
                        if (transformBuffer[1] > 0)
                        {
                            for (int i = 0; i < newBuffer.Length; i++)
                            {
                                newBuffer[i] = (byte)(newBuffer[i] - transformBuffer[1]);
                            }
                        }
                        newBuffer = libCoding.Caesar(buffer, transformBuffer[2]);
                        if(transformBuffer[1] > 0)
                        { 
                            for(int i = 0; i < newBuffer.Length; i++)
                            {
                                newBuffer[i] = (byte)(newBuffer[i] + transformBuffer[1]);
                            }
                        }
                        break;
                    }
                case TRANSFORM_OP.Plus:
                    {
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            newBuffer[i] = (byte)(buffer[i] + transformBuffer[loopIndex++]);
                            if (loopIndex >= transformBuffer.Length) loopIndex = 0;
                        }
                        break;
                    }
                case TRANSFORM_OP.Minus:
                    {
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            newBuffer[i] = (byte)(buffer[i] - transformBuffer[loopIndex++]);
                            if (loopIndex >= transformBuffer.Length) loopIndex = 0;
                        }
                        break;
                    }
                case TRANSFORM_OP.ShiftLeft:
                    {
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            newBuffer[i] = (byte)(buffer[i] << transformBuffer[loopIndex++]);
                            if (loopIndex >= transformBuffer.Length) loopIndex = 0;
                        }
                        break;
                    }
                case TRANSFORM_OP.ShiftRight:
                    {
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            newBuffer[i] = (byte)(buffer[i] >> transformBuffer[loopIndex++]);
                            if (loopIndex >= transformBuffer.Length) loopIndex = 0;
                        }
                        break;
                    }
                case TRANSFORM_OP.Replace: // Replace with new op
                    {
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            // Replace one value
                            newBuffer[i] = (buffer[i] == transformBuffer[0]) ? transformBuffer[1] : buffer[i];
                        }
                        break;
                    }
                case TRANSFORM_OP.Swap:
                    {
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            if(i < buffer.Length - 1)
                            { 
                                tempByte = buffer[i];
                                buffer[i] = buffer[i + 1];
                                buffer[i + 1] = tempByte;
                            }
                            i++;
                        }
                        break;
                    }
                default: // Invalid op-code, return input buffer
                    {
                        return buffer;
                    }
            } // switch()
            //
            return newBuffer;
        }

        public static byte[] TransformBlob(byte[] buffer, TRANSFORM_ARG param)
        {
            return TransformBlob(buffer, param.args, param.opCode);
        }

        public static string TransformText(string value, TRANSFORM_ARG param)
        {
            char[] buffer;
            List<char> destBuffer;
            int baseCharCode; // "Zero" character (A, a,...)
            int tempInt;
            string returnStr;

            buffer = value.ToCharArray();
            destBuffer = new List<char>();
            returnStr = "";
            baseCharCode = 0;
            switch (param.alphabet)
            {
                case TRANSFORM_ALPHABET.UPPERCASE:
                    {
                        baseCharCode = 65; // 0x41
                        break;
                    }
                case TRANSFORM_ALPHABET.LOWERCASE:
                    {
                        baseCharCode = 97; // 0x61
                        break;
                    }
                case TRANSFORM_ALPHABET.DIGITS:
                    {
                        baseCharCode = 48; // 0x30
                        break;
                    }
                case TRANSFORM_ALPHABET.ASCII:
                    {
                        baseCharCode = 0; // 0x00
                        break;
                    }
                case TRANSFORM_ALPHABET.BIT8:
                    {
                        baseCharCode = 0; // 0x00
                        break;
                    }
            } // switch()
            switch (param.opCode)
            {
                case TRANSFORM_OP.Swap:
                    {
                        for(int i = 0; i < buffer.Length; i += 2)
                        {
                            if(i < buffer.Length - 1) destBuffer.Add(buffer[i + 1]);
                            destBuffer.Add(buffer[i]);
                        }
                        returnStr = new string(destBuffer.ToArray());
                        break;
                    }
                case TRANSFORM_OP.Caesar:
                    {
                        for (int j = 1; j < param.args.Length; j++)
                        {
                            for (int i = 0; i < buffer.Length; i++)
                            {
                                tempInt = buffer[i] - baseCharCode; // tempInt 0 - alphasize - 1
                                tempInt = (tempInt + param.args[j]) % param.args[0];
                                tempInt = baseCharCode + tempInt;
                                destBuffer.Add(Convert.ToChar(tempInt));
                            }
                        }
                        returnStr = new string(destBuffer.ToArray());
                        break;
                    }
                default: // Invalid op-code, return input buffer
                    {
                        return returnStr;
                    }
            }
            return returnStr;
        }

        public static byte[] ParseStringToByteArray(string value)
        {
            List<byte> buffer;
            string token;
            int tempInt;

            buffer = new List<byte>();
            if(string.IsNullOrEmpty(value)) return buffer.ToArray();
            //
            token = "";
            for (int i=0; i < value.Length;i++)
            {
                while(char.IsDigit(value[i]))
                {
                    token += value[i++];
                    if (i >= value.Length) break; // End of string reached
                }
                if(!string.IsNullOrEmpty(token))
                {
                    if(int.TryParse(token, out tempInt))
                    {
                        buffer.Add((byte)tempInt);
                    }
                    token = "";
                }
            }
            if (!string.IsNullOrEmpty(token))
            {
                if (int.TryParse(token, out tempInt))
                {
                    buffer.Add((byte)tempInt);
                }
                token = "";
            }
            //
            return buffer.ToArray();
        }

        public static void DensityMap(string filename)
        {
            int totalBlocks = 1024;
            long fileLen;
            int blockSize;
            byte[] buffer;
            List<double> entropyDistribution;
            List<int> redValues;
            int redValue;
            List<int> greenValues;
            int greenValue;
            int blueValue = 255;
            List<Color> colors;
            FileInfo fi;
            libEntropy oEntropy;
            double entropy; // Entropy of a block
            Single entropyScaleFactorRed; // Scale entropy value to a drawable value (R-G space)
            Single entropyScaleFactorGreen;
            FileStream fs;
            // Drawing
            Size dotSize = new Size(4, 4);

            entropyScaleFactorRed = 31.875F; // entropyScaleFactorRed * entropy: R-value (0-255)
            entropyScaleFactorGreen = 31.875F; // entropyScaleFactorGreen * (8 - entropy): G-value (0-255)
            // B-value constant 255
            //
            if (!File.Exists(filename)) return;
            entropyDistribution = new List<double>();
            redValues = new List<int>();
            greenValues = new List<int>();
            colors = new List<Color>();
            //buffer = new byte[blockSize];
            fi = new FileInfo(filename); // File length to scale distribution
            fileLen = fi.Length;
            blockSize = (int)fileLen / totalBlocks;
            buffer = new byte[blockSize];
            //
            oEntropy = new libEntropy();
            fs = File.Open(filename, FileMode.Open, FileAccess.Read);
            while(fs.Read(buffer,0,blockSize) > 0)
            {
                oEntropy.CalcBufferEntropy(buffer);
                entropy = oEntropy.Entropy;
                entropyDistribution.Add(entropy);
            }
            fs.Dispose();
            // Calculation
            for(int i = 0; i < entropyDistribution.Count; ++i)
            {
                redValue = (int)(entropyDistribution.ElementAt(i) * entropyScaleFactorRed);
                greenValue = (int)((8 - entropyDistribution.ElementAt(i)) * entropyScaleFactorGreen);
                colors.Add(Color.FromArgb(redValue, greenValue, blueValue));
            }
            // Now should have 1024 color-values
        }
        
        public static string ConvertByteArrayToHex(byte[] buffer, string prefix = "", string separator = " ")
        {
            StringBuilder newStr;

            newStr = new StringBuilder();

            if (buffer == null || buffer.Length < 1) return newStr.ToString();
            if (prefix == "")
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    newStr.AppendFormat("{0:X2}", buffer[i]);
                    if (i < buffer.Length - 1) newStr.Append(separator);
                }
            }
            else
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    newStr.Append(prefix);
                    newStr.AppendFormat("{0:X2}", buffer[i]);
                    if (i < buffer.Length - 1) newStr.Append(separator);
                }
            }
            //
            return newStr.ToString();
        }

        public static string ConvertByteArrayToDecimal(byte[] buffer, string separator = " ")
        {
            string newStr;

            newStr = "";

            if (buffer == null || buffer.Length < 1) return newStr;
            for (int i = 0; i < buffer.Length; i++)
            {
                newStr += buffer[i].ToString();
                if (i < buffer.Length - 1) newStr += separator;
            }
            //
            return newStr;
        }

        public static List<string> ImportWordList(string filename)
        {
            List<string> newList;

            newList = new List<string>();
            try
            {
                newList = File.ReadAllLines(filename).ToList();
            }
            catch(Exception)
            {
                // Can't read the file
            }
            //
            return newList;
        }

        /// <summary>
        /// Wrapper for Encoding.<encoding>.GetBytes(<string>)
        /// </summary>
        /// <param name="value">A string to convert</param>
        /// <param name="encoding">Encoding to be used for getting bytes</param>
        /// <returns>String as byte array</returns>
        public static byte[] GetBytes(string value, TEXT_ENCODING encoding)
        {
            byte[] buffer = null;

            if(encoding == TEXT_ENCODING.Default)
            {
                buffer = Encoding.Default.GetBytes(value);
            }
            else if (encoding == TEXT_ENCODING.ASCII)
            {
                buffer = Encoding.ASCII.GetBytes(value);
            }
            else if (encoding == TEXT_ENCODING.UTF8)
            {
                buffer = Encoding.UTF8.GetBytes(value);
            }
            else if (encoding == TEXT_ENCODING.UNICODE)
            {
                buffer = Encoding.Unicode.GetBytes(value);
            }
            else if (encoding == TEXT_ENCODING.UTF32)
            {
                buffer = Encoding.UTF32.GetBytes(value);
            }
            //
            return buffer;
        }

        /// <summary>
        /// Wrapper for Encoding.<encoding>.GetChars(<byte[]>)
        /// </summary>
        /// <param name="buffer">Byte array to convert</param>
        /// <param name="encoding">Encoding to be used for getting chars</param>
        /// <returns>Byte array as char array</returns>
        public static char[] GetChars(byte[] buffer, TEXT_ENCODING encoding)
        {
            char[] charArr = null;

            if (encoding == TEXT_ENCODING.Default)
            {
                charArr = Encoding.Default.GetChars(buffer);
            }
            else if (encoding == TEXT_ENCODING.ASCII)
            {
                charArr = Encoding.ASCII.GetChars(buffer);
            }
            else if (encoding == TEXT_ENCODING.UTF8)
            {
                charArr = Encoding.UTF8.GetChars(buffer);
            }
            else if (encoding == TEXT_ENCODING.UNICODE)
            {
                charArr = Encoding.Unicode.GetChars(buffer);
            }
            else if (encoding == TEXT_ENCODING.UTF32)
            {
                charArr = Encoding.UTF32.GetChars(buffer);
            }
            //
            return charArr;
        }

        //public static void VBScriptDecode(string buffer)
        //{
            // Option Explicit
            const int BIF_NEWDIALOGSTYLE = 0x40;
            const int BIF_NONEWFOLDERBUTTON = 0x200;
            const int BIF_RETURNONLYFSDIRS = 0x1;
            // 
            const int FOR_READING = 1;
            const int FOR_WRITING = 2;
            // 
            // 
            const string TAG_BEGIN1 = @"#@~^";
            const string TAG_BEGIN2 = @"==";
            const int TAG_BEGIN2_OFFSET = 10;
            const int TAG_BEGIN_LEN = 12;
            const string TAG_END = @"==^#~@";
            const int TAG_END_LEN = 6;
        // 
        //private static string argv; // Type?
        //private static object wsoShellApp; // Type?
        //private static object oFolder; // Type?
        //private static string sFolder;
        //private static string sFileSource;
        //private static string sFileDest;
        //private static object fso;
        //private static object fld;
        //private static object fc;
        //private static bool bEncoded;
        //private static object fSource;
        //private static object tsSource;
        //private static object tsDest;
        //private static int iNumExamined;
        //private static int iNumProcessed;
        //private static int iNumSkipped;
        // Dim argv
        // Dim wsoShellApp
        // Dim oFolder
        // Dim sFolder
        // Dim sFileSource
        // Dim sFileDest
        // Dim fso
        // Dim fld
        // Dim fc
        // Dim bEncoded
        // Dim fSource
        // Dim tsSource
        // Dim tsDest
        // Dim iNumExamined
        // Dim iNumProcessed
        // Dim iNumSkipped
        // 
        public static string DecodeVbs(string Chaine)
        {
            object se;
            //int i;
            int c;
            int j;
            int index;
            string ChaineTemp;
            string[] tDecode = new string[127];
            string Combinaison = "1231232332321323132311233213233211323231311231321323112331123132";

            //    se = WSCript.CreateObject("Scripting.Encoder");
            for (int i = 9; i <= 127; i++)
            {
                tDecode[i] = "JLA";
            }
            for (int i = 9; i <= 127; i++)
            {

            }

            // Function Decode(Chaine)
            //  Dim se,i,c,j,index,ChaineTemp
            //  Dim tDecode(127)
            //  Const Combinaison="1231232332321323132311233213233211323231311231321323112331123132"
            // 
            //  Set se=WSCript.CreateObject("Scripting.Encoder")
            //  For i=9 to 127
            //   tDecode(i)="JLA"
            //  Next
            //  For i=9 to 127
            //   ChaineTemp=Mid(se.EncodeScriptFile(".vbs",string(3,i),0,""),13,3)
            //   For j=1 to 3
            //    c=Asc(Mid(ChaineTemp,j,1))
            //    tDecode(c)=Left(tDecode(c),j-1) & chr(i) & Mid(tDecode(c),j+1)
            //   Next
            //  Next
            //  
            //  tDecode(42)=Left(tDecode(42),1) & ")" & Right(tDecode(42),1)
            //  Set se=Nothing
            // 

            Chaine = Chaine.Replace(@"@&", new string(new char[] { (char)10 }));
            Chaine = Chaine.Replace(@"@#", new string(new char[] { (char)13 }));
            //  Chaine=Replace(Replace(Chaine,"@&",chr(10)),"@#",chr(13))


            Chaine = Chaine.Replace(@"@*", new string(new char[] { '>' }));
            Chaine = Chaine.Replace(@"@!", new string(new char[] { '<' }));
            //  Chaine=Replace(Replace(Chaine,"@*",">"),"@!","<")

            Chaine = Chaine.Replace(@"@$", new string(new char[] { '@' }));
            //  Chaine=Replace(Chaine,"@$","@")
            index = -1;
            //  index=-1

            //  For i=1 to Len(Chaine)
            for (int i = 1; i < Chaine.Length; i++)
            {
                c = Chaine[i];
                if (c < 128) index++;
                if (c == 9 || (c > 31 && c < 128))
                {
                    if (c != 60 && c != 62 && c != 64)
                    {
                        // TODO
                        //Chaine = Chaine.Substring(0, i-1) + 
                        Chaine = Chaine.Substring(0, i - 1) +
                        // Mid(tDecode(c),Mid(Combinaison,(index mod 64)+1,1),1)
                        tDecode[c].Substring(int.Parse(Combinaison.Substring((index % 64) + 1, 1)), 1) +
                        // & Mid(Chaine,i+1)
                        Chaine.Substring(i + 1, Chaine.Length - i - 1);
                    }
                }
            }
            //
            return Chaine; // End Function
        }

            //  For i=1 to Len(Chaine)
            //   c=asc(Mid(Chaine,i,1))
            //   If c<128 Then index=index+1
            //   If (c=9) or ((c>31) and (c<128)) Then
            //    If (c<>60) and (c<>62) and (c<>64) Then
            //     Chaine=Left(Chaine,i-1) & Mid(tDecode(c),Mid(Combinaison,(index mod 64)+1,1),1) & Mid(Chaine,i+1)
            //    End If
            //   End If
            //  Next
            //  Decode=Chaine
            // End Function
            // 

            // Sub Process (s)
            public static string VBScriptDecode(string s)
            {
            //bool bProcess;
            int iTagBeginPos;
            int iTagEndPos;
            string dbgStr;

            //  iTagBeginPos = Instr(s, TAG_BEGIN1)
            iTagBeginPos = s.IndexOf(TAG_BEGIN1);

            //    iTagEndPos = Instr(iTagBeginPos, s, TAG_END)
            iTagEndPos = s.IndexOf(TAG_END, iTagBeginPos);

            //       s = Decode(Mid(s, iTagBeginPos + TAG_BEGIN_LEN, iTagEndPos - iTagBeginPos - TAG_BEGIN_LEN - TAG_END_LEN))
            dbgStr = s.Substring(iTagBeginPos + TAG_BEGIN_LEN, iTagEndPos - iTagBeginPos - TAG_BEGIN_LEN - TAG_END_LEN);
            s = DecodeVbs(dbgStr);

            return s;

            // Sub Process (s)
            //  Dim bProcess
            //  Dim iTagBeginPos
            //  Dim iTagEndPos
            // 
            // 
            //  iNumExamined = iNumExamined + 1
            // 
            //  iTagBeginPos = Instr(s, TAG_BEGIN1)
            // 
            //  Select Case iTagBeginPos
            //  Case 0
            //   MsgBox sFileSource & " does not appear to be encoded.  Missing Beginning Tag.  Skipping file."
            //   iNumSkipped = iNumSkipped + 1
            // 
            //  Case 1
            //   If (Instr(iTagBeginPos, s, TAG_BEGIN2) - iTagBeginPos) = TAG_BEGIN2_OFFSET Then
            //    iTagEndPos = Instr(iTagBeginPos, s, TAG_END)
            // 
            //    If iTagEndPos > 0 Then
            //     Select Case Mid(s, iTagEndPos + TAG_END_LEN)
            //     Case "", Chr(0)
            //      bProcess = True
            // 
            //      If fso.FileExists(sFileDest) Then
            //       If MsgBox("File """ & sFileDest & """ exists.  Overwrite?", vbYesNo + vbDefaultButton2) <> vbYes Then
            //        bProcess = False
            //        iNumSkipped = iNumSkipped + 1
            //       End If
            //      End If
            // 
            //      If bProcess Then
            //       s = Decode(Mid(s, iTagBeginPos + TAG_BEGIN_LEN, iTagEndPos - iTagBeginPos - TAG_BEGIN_LEN - TAG_END_LEN))
            // 
            //  
            // 
            //       Set tsDest = fso.CreateTextFile(sFileDest, TRUE, FALSE)
            //       tsDest.Write s
            //       tsDest.Close
            //       Set tsDest = Nothing
            // 
            //       iNumProcessed = iNumProcessed + 1
            //      End If
            // 
            //     Case Else
            //      MsgBox sFileSource & " does not appear to be encoded.  Found " & Len(Mid(s, iTagEndPos + TAG_END_LEN)) & " characters AFTER Ending Tag.  Skipping file."
            //      iNumSkipped = iNumSkipped + 1
            //     End Select
            // 
            //    Else
            //     MsgBox sFileSource & " does not appear to be encoded.  Missing ending Tag.  Skipping file."
            //     iNumSkipped = iNumSkipped + 1
            //    End If
            // 
            //   Else
            //    MsgBox sFileSource & " does not appear to be encoded.  Incomplete Beginning Tag.  Skipping file."
            //    iNumSkipped = iNumSkipped + 1
            //   End If
            // 
            //  Case Else
            //   MsgBox sFileSource & " does not appear to be encoded.  Found " & (iTagBeginPos - 1) & "characters BEFORE Beginning Tag.  Skipping file."
            //   iNumSkipped = iNumSkipped + 1
            //  End Select
            // End Sub
            // 
            // Set argv = WScript.Arguments
            // 
            // sFileSource = ""
            // sFolder = ""
            // iNumExamined = 0
            // iNumProcessed = 0
            // iNumSkipped = 0
            // 
            // Select Case argv.Count
            // Case 0
            //  Set wsoShellApp = WScript.CreateObject("Shell.Application")
            // 
            //  On Error Resume Next
            //  set oFolder = wsoShellApp.BrowseForFolder (0, "Select a folder containing files to decode", BIF_NEWDIALOGSTYLE + BIF_NONEWFOLDERBUTTON + BIF_RETURNONLYFSDIRS)
            //  If Err.Number = 0 Then
            //   If TypeName(oFolder) = "Folder3" Then Set oFolder = oFolder.Items.Item
            //   sFolder = oFolder.Path
            //  End If
            //  On Error GoTo 0
            // 
            //  Set oFolder = Nothing
            //  Set wsoShellApp = Nothing
            // 
            //  If sFolder = "" Then
            //   MsgBox "Please pass a full file spec or select a folder containing encoded files"
            //   WScript.Quit
            //  End If
            // 
            // Case 1
            //  sFileSource = argv(0)
            // 
            //  If InStr(sFileSource, "?") > 0 Then
            //   MsgBox "Pass a full file spec or no arguments (browse for a folder)"
            //   WScript.Quit
            //  End If
            // 
            // Case Else
            //  MsgBox "Pass a full file spec, -?, /?, ?, or no arguments (browse for a folder)"
            //  WScript.Quit
            // End Select
            // 
            // Set fso = WScript.CreateObject("Scripting.FileSystemObject")
            // 
            // If sFolder <> "" Then
            //  On Error Resume Next
            //  Set fld = fso.GetFolder(sFolder)
            //  If Err.Number <> 0 Then
            //   Set fld = Nothing
            //   Set fso = Nothing
            //   MsgBox "Folder """ & sFolder & """ is not valid in this context"
            //   WScript.Quit
            //  End If
            //  On Error GoTo 0
            // 
            //  Set fc = fld.Files
            // 
            //  For Each fSource In fc
            //   sFileSource = fSource.Path
            // 
            //   Select Case LCase(Right(sFileSource, 4))
            //   Case ".vbe"
            //    sFileDest = Left(sFileSource, Len(sFileSource) - 1) & "s"
            //    bEncoded = True
            //  
            //   Case Else
            //    bEncoded = False
            //   End Select
            // 
            //   If bEncoded Then
            //    Set tsSource = fSource.OpenAsTextStream(FOR_READING)
            //    Process tsSource.ReadAll
            //    tsSource.Close
            //    Set tsSource = Nothing
            //   End If
            //  Next
            // 
            //  Set fc = Nothing
            //  Set fld = Nothing
            // 
            // Else
            //  If Not fso.FileExists(sFileSource) Then
            //   MsgBox "File """ & sFileSource & """ not found"
            //  Else
            //   bEncoded = False
            // 
            //   Select Case LCase(Right(sFileSource, 4))
            //   Case ".vbe"
            //    sFileDest = Left(sFileSource, Len(sFileSource) - 1) & "s"
            //    bEncoded = True
            //     Case Else
            //    MsgBox "File """ & sFileSource & """ needs to be of type VBE or JSE"
            //    bEncoded = False
            //   End Select
            // 
            //   If bEncoded Then
            //    Set tsSource = fso.OpenTextFile(sFileSource, FOR_READING)
            //    Process tsSource.ReadAll
            //    tsSource.Close
            //    Set tsSource = Nothing
            //   End If
            //  End If
            // End If
            // 
            // Set fso = Nothing
            // 
            // MsgBox iNumExamined & " Files Examined; " & iNumProcessed & " Files Processed; " & iNumSkipped & " Files Skipped"
            // 
            // 
            // 
            // 
            //} // VBDecode()
        }  // VBDecode()

        /// <summary>
        /// Apply vbscript encoding to a string
        /// </summary>
        /// <param name="buffer">String to encode</param>
        /// <returns>VBScript encoded string</returns>
        public static string VBScriptEncode(string buffer)
        {
            Scripting.Encoder oEncoder;
            string outBuffer;

            // oEncoder = CreateObject("Scripting.Encoder")
            oEncoder = new Scripting.Encoder();
            // sDest = oEncoder.EncodeScriptFile(".bs",sSourceFile,0,"")
            outBuffer = oEncoder.EncodeScriptFile(".vbs", buffer,0, "");
            //
            return outBuffer;
        }

        /// <summary>
        /// Converts base-10 numbers to base-2 numbers
        /// </summary>
        /// <param name="value">Text to convert</param>
        /// <returns>Converted text</returns>
        /// <remarks>Numbers do not have to byte sized. Function tries to determine proper format for numbers</remarks>
        public static string ConvertDecimalTextToBinary(string value)
        {
            string[] splitChars;
            string[] tokens;
            List<string> stringArr;
            List<int> intArr;
            int tempInt;

            splitChars = GetSplitChars();
            stringArr = new List<string>();
            intArr = new List<int>();
            tokens = value.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            // Convert text to integers
            for (int i = 0; i < tokens.Length; i++)
            {
                if (int.TryParse(tokens[i], out tempInt))
                {
                    intArr.Add(tempInt);
                }
            }
            // Convert integers to binary format
            for (int i = 0; i < intArr.Count; i++)
            {
                stringArr.Add(ConvertIntToBinary(intArr.ElementAt(i)));
            }
            //
            return string.Join(" ", stringArr.ToArray());
        }

        public static string ConvertDecimalTextToOctal(string value)
        {
            string[] splitChars;
            string[] tokens;
            List<string> stringArr;
            List<int> intArr;
            int tempInt;

            splitChars = GetSplitChars();
            stringArr = new List<string>();
            intArr = new List<int>();
            tokens = value.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            // Convert text to integers
            for (int i = 0; i < tokens.Length; i++)
            {
                if (int.TryParse(tokens[i], out tempInt))
                {
                    intArr.Add(tempInt);
                }
            }
            // Convert integers to octal format
            for (int i = 0; i < intArr.Count; i++)
            {
                stringArr.Add(ConvertIntToOctal(intArr.ElementAt(i)));
            }
            //
            return string.Join(" ", stringArr.ToArray());
        }

        public static string ConvertDecimalTextToHex(string value)
        {
            string[] splitChars;
            string[] tokens;
            List<string> stringArr;
            List<int> intArr;
            int tempInt;

            splitChars = GetSplitChars();
            stringArr = new List<string>();
            intArr = new List<int>();
            tokens = value.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            // Convert text to integers
            for (int i = 0; i < tokens.Length; i++)
            {
                if (int.TryParse(tokens[i], out tempInt))
                {
                    intArr.Add(tempInt);
                }
            }
            // Convert integers to binary format
            for (int i = 0; i < intArr.Count; i++)
            {
                stringArr.Add(ConvertIntToHex(intArr.ElementAt(i)));
            }
            //
            return string.Join(" ", stringArr.ToArray());
        }

        public static string ConvertHexTextToBinary(string value)
        {
            string[] splitChars;
            string[] tokens;
            List<string> stringArr;
            List<int> intArr;
            int tempInt;

            splitChars = GetSplitChars();
            stringArr = new List<string>();
            intArr = new List<int>();
            tokens = value.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            // Convert text to integers
            for (int i = 0; i < tokens.Length; i++)
            {
                tempInt = ConvertHexToInt(tokens[i]);
                intArr.Add(tempInt);
            }
            // Convert integers to binary format
            for (int i = 0; i < intArr.Count; i++)
            {
                stringArr.Add(ConvertIntToBinary(intArr.ElementAt(i)));
            }
            //
            return string.Join(" ", stringArr.ToArray());
        }

        public static string ConvertHexTextToOctal(string value)
        {
            string[] splitChars;
            string[] tokens;
            List<string> stringArr;
            List<int> intArr;
            int tempInt;

            splitChars = GetSplitChars();
            stringArr = new List<string>();
            intArr = new List<int>();
            tokens = value.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            // Convert text to integers
            for (int i = 0; i < tokens.Length; i++)
            {
                tempInt = ConvertHexToInt(tokens[i]);
                intArr.Add(tempInt);
            }
            // Convert integers to octal format
            for (int i = 0; i < intArr.Count; i++)
            {
                stringArr.Add(ConvertIntToOctal(intArr.ElementAt(i)));
            }
            //
            return string.Join(" ", stringArr.ToArray());
        }

        public static string ConvertHexTextToDecimal(string value)
        {
            string[] splitChars;
            string[] tokens;
            List<string> stringArr;
            List<int> intArr;
            int tempInt;

            splitChars = GetSplitChars();
            stringArr = new List<string>();
            intArr = new List<int>();
            tokens = value.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            // Convert text to integers
            for (int i = 0; i < tokens.Length; i++)
            {
                tempInt = ConvertHexToInt(tokens[i]);
                intArr.Add(tempInt);
            }
            // Convert integers to strings
            for (int i = 0; i < intArr.Count; i++)
            {
                stringArr.Add(intArr.ElementAt(i).ToString());
            }
            //
            return string.Join(" ", stringArr.ToArray());
        }

        public static string ConvertBinaryTextToDecimal(string value, int size)
        {
            string[] splitChars;
            string[] tokens;
            List<string> stringArr;
            List<int> intArr;

            splitChars = GetSplitChars();
            stringArr = new List<string>();
            intArr = new List<int>();
            tokens = value.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            // Binary (text) source may be continuous string w/o any separators
            for (int i = 0; i < tokens.Length; i++)
            {
                intArr.AddRange(ConvertBinaryToInt(tokens[i], size).ToArray());
                
            }
            // Convert integers to strings
            for (int i = 0; i < intArr.Count; i++)
            {
                stringArr.Add(intArr.ElementAt(i).ToString());
            }
            //
            return string.Join(" ", stringArr.ToArray());
        }

        /// <summary>
        /// Converts textual decimal integers to byte values
        /// </summary>
        /// <param name="value">String to convert</param>
        /// <returns>Byte array</returns>
        public static byte[] ConvertDecimalTextToBytes(string value)
        {
            string[] splitChars;
            string[] tokens;
            List<byte> buffer;
            byte tempByte;

            splitChars = GetSplitChars();
            buffer = new List<byte>();
            tokens = value.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0; i < tokens.Length; i++)
            {
                if(byte.TryParse(tokens[i], out tempByte))
                {
                    buffer.Add(tempByte);
                }
            }
            //
            return buffer.ToArray();
        }

        /// <summary>
        /// Converts textual hexadecimal integers to byte values
        /// </summary>
        /// <param name="value">String to convert</param>
        /// <returns>Byte array</returns>
        public static byte[] ConvertHexToBytes(string value)
        {
            string[] splitChars;
            string[] tokens;
            List<byte> buffer;
            byte tempByte;
            string token;
            string hexChars = "0123456789ABCDEF";
            int tempPos;

            splitChars = GetSplitChars();
            buffer = new List<byte>();
            tokens = value.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tokens.Length; i++)
            {
                token = tokens[i].ToUpper();
                if(token.Length > 0)
                {
                    tempPos = hexChars.IndexOf(token[0]);
                    if(tempPos >= 0)
                    {
                        tempByte = (byte)tempPos;
                        if (token.Length > 1)
                        {
                            tempPos = hexChars.IndexOf(token[1]);
                            if (tempPos >= 0)
                            {
                                tempByte <<= 4;
                                tempByte |= (byte)tempPos;
                            }
                        }
                        buffer.Add(tempByte); // Accepts single hex char and two hex chars
                    }
                }
            }
            //
            return buffer.ToArray();
        }

        public static void LoadScriptConfig(string scriptConfigFilename, string path, libScripts scriptColl)
        {
            string[] allLines;
            string oneLine;
            char[] splitChar;
            string[] stringArr;
            string[] stringNameArr;
            libScriptItem newItem;

            allLines = File.ReadAllLines(scriptConfigFilename);
            splitChar = new char[] { ';' };
            // Parse lines
            for (int i = 0; i < allLines.Length; i++)
            {
                oneLine = allLines[i];
                if (string.IsNullOrEmpty(oneLine)) continue; // Skip empty line
                if (oneLine.StartsWith("#")) continue; // Skip comment line
                stringArr = oneLine.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
                if(stringArr.Length > 1) // Has two to four args as should be
                {
                    newItem = new libScriptItem();
                    newItem.ScriptName = stringArr[0];
                    if (stringArr[1].Contains("+"))
                    {
                        stringNameArr = stringArr[1].Split(new char[] { '+' });
                        newItem.ScriptFilename = stringNameArr[0] + " " + Path.Combine(path, stringNameArr[1]);
                    }
                    else
                    { 
                        newItem.ScriptFilename = Path.Combine(path, stringArr[1]);
                    }
                    if (stringArr.Length > 2) newItem.ScriptArgs = stringArr[2];
                    if (stringArr.Length > 3) newItem.ScriptHelpFilename = Path.Combine(path, stringArr[3]);
                    scriptColl.Add(newItem);
                }
            }
        }

        // TODO: Do more stats and show them either graphically or otherwise more usable format
        public static string[] Stats(byte[] buffer)
        {
            int[] distribution;
            int totalBytes;
            List<string> statText;
            string newString;

            statText = new List<string>();
            if (buffer == null || buffer.Length < 1) return statText.ToArray();
            distribution = new int[256];
            totalBytes = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                distribution[buffer[i]]++;
            }
            totalBytes = buffer.Length;
            if (totalBytes < 1) totalBytes = 1;
            for (int i = 0; i < distribution.Length; i++)
            {
                if(distribution[i] > 0)
                { 
                    newString = string.Format("{0:X2}: {1,3} {2,4:N2}%", i, distribution[i],
                        ((double)distribution[i] * 100) / (double)totalBytes) + Environment.NewLine;
                    statText.Add(newString);
                }
                else
                {
                    //newString = string.Format("{0:X2}:", i) + Environment.NewLine;
                    //statText.Add(newString);
                }
            }
            return statText.ToArray();
        }

        public static int[] Distribution(byte[] buffer)
        {
            int[] distribution;

            distribution = new int[256];
            if (buffer == null || buffer.Length < 1) return distribution;
            //
            for (int i = 0; i < buffer.Length; i++)
            {
                distribution[buffer[i]]++;
            }
            //
            return distribution;
        }

        public static byte[] ConvertFromBase64(string currSelection, out string statusMsg)
        {
            byte[] buffer;

            statusMsg = "";
            buffer = new byte[0];
            if (!string.IsNullOrEmpty(currSelection))
            {
                try
                {
                    buffer = Convert.FromBase64String(currSelection);
                    statusMsg = "Convert From Base64";
                }
                catch (FormatException)
                {
                    statusMsg = "Selection is not a valid BASE64 string <Check what is wrong>";
                }
            }
            return buffer;
        }

        /// <summary>
        /// A wrapper for ConvertByteArrayToHex and ConvertByteArrayToDecimal
        /// </summary>
        /// <param name="buffer">Byte buffer</param>
        /// <param name="showAsHex">Hex encode</param>
        /// <returns>Byte buffer as string in hexadecimal or decimal format</returns>
        public static string DumpByteBuffer(byte[] buffer, bool showAsHex)
        {
            string tempStr;

            if(showAsHex)
            {
                tempStr = MainCode.ConvertByteArrayToHex(buffer);
            }
            else
            {
                tempStr = MainCode.ConvertByteArrayToDecimal(buffer);
            }
            return tempStr;
        }

        public static void FillToolStrip(ToolStrip ctl, ImageList imageList, EventHandler OnToolStrip_Click)
        {
            ToolStripButton cmdTsButton;
            ToolStripSeparator tsSeparator;
            //ToolStripItem tsItem;

            cmdTsButton = new ToolStripButton();
            cmdTsButton.Name = "Back";
            cmdTsButton.Click += OnToolStrip_Click;
            cmdTsButton.Image = imageList.Images["Back"];
            cmdTsButton.ToolTipText = "Go to previous view";
            ctl.Items.Add(cmdTsButton);
            cmdTsButton = new ToolStripButton();
            cmdTsButton.Name = "Forward";
            cmdTsButton.Click += OnToolStrip_Click;
            cmdTsButton.Image = imageList.Images["Forward"];
            cmdTsButton.ToolTipText = "Go to next view";
            ctl.Items.Add(cmdTsButton);
            tsSeparator = new ToolStripSeparator();
            ctl.Items.Add(tsSeparator);
            cmdTsButton = new ToolStripButton();
            cmdTsButton.Name = "Copy";
            cmdTsButton.Click += OnToolStrip_Click;
            cmdTsButton.Image = imageList.Images["Copy"];
            cmdTsButton.ToolTipText = "Copy text to clipboard";
            ctl.Items.Add(cmdTsButton);
            cmdTsButton = new ToolStripButton();
            cmdTsButton.Name = "CopyBinary";
            cmdTsButton.Click += OnToolStrip_Click;
            cmdTsButton.Image = imageList.Images["CopyBinary"];
            cmdTsButton.ToolTipText = "Copy binary data to clipboard";
            ctl.Items.Add(cmdTsButton);
            cmdTsButton = new ToolStripButton();
            cmdTsButton.Name = "Cut";
            cmdTsButton.Click += OnToolStrip_Click;
            cmdTsButton.Image = imageList.Images["Cut"];
            cmdTsButton.ToolTipText = "Cut text to clipboard";
            ctl.Items.Add(cmdTsButton);
            cmdTsButton = new ToolStripButton();
            cmdTsButton.Name = "Paste";
            cmdTsButton.Click += OnToolStrip_Click;
            cmdTsButton.Image = imageList.Images["Paste"];
            cmdTsButton.ToolTipText = "Paste text from the clipboard";
            ctl.Items.Add(cmdTsButton);
            cmdTsButton = new ToolStripButton();
            cmdTsButton.Name = "Clear";
            cmdTsButton.Click += OnToolStrip_Click;
            cmdTsButton.Image = imageList.Images["Clear"];
            cmdTsButton.ToolTipText = "Clear the output window";
            ctl.Items.Add(cmdTsButton);
            tsSeparator = new ToolStripSeparator();
            ctl.Items.Add(tsSeparator);
            cmdTsButton = new ToolStripButton();
            cmdTsButton.Name = "Checksum";
            cmdTsButton.Click += OnToolStrip_Click;
            cmdTsButton.Image = imageList.Images["Checksum"];
            cmdTsButton.ToolTipText = "Calculate checksums of the current file";
            ctl.Items.Add(cmdTsButton);
            cmdTsButton = new ToolStripButton();
            cmdTsButton.Name = "Strings";
            cmdTsButton.Click += OnToolStrip_Click;
            cmdTsButton.Image = imageList.Images["Strings"];
            cmdTsButton.ToolTipText = "Extract strings from the current file";
            ctl.Items.Add(cmdTsButton);
            tsSeparator = new ToolStripSeparator();
            ctl.Items.Add(tsSeparator);
            cmdTsButton = new ToolStripButton();
            cmdTsButton.Name = "Numbers";
            cmdTsButton.Click += OnToolStrip_Click;
            cmdTsButton.Image = imageList.Images["Numbers"];
            cmdTsButton.ToolTipText = "Toggle between decimal and hexadecimal format";
            ctl.Items.Add(cmdTsButton);
            cmdTsButton = new ToolStripButton();
            cmdTsButton.Name = "Encoding";
            cmdTsButton.Click += OnToolStrip_Click;
            cmdTsButton.Image = imageList.Images["Encoding"];
            cmdTsButton.ToolTipText = "Switch between string encodings: Default, ASCII, UNICODE";
            ctl.Items.Add(cmdTsButton);
            tsSeparator = new ToolStripSeparator();
            ctl.Items.Add(tsSeparator);
            cmdTsButton = new ToolStripButton();
            cmdTsButton.Name = "MoveUp";
            cmdTsButton.Click += OnToolStrip_Click;
            cmdTsButton.Image = imageList.Images["MoveUp"];
            cmdTsButton.ToolTipText = "Move result text to source text";
            ctl.Items.Add(cmdTsButton);
        }

        /// <summary>
        /// Saves current workspace
        /// </summary>
        /// <param name="filename">File for application's workspace</param>
        /// <param name="oMainForm">Application's main form</param>
        public static void SaveWorkspace(string filename, frmMain oMainForm)
        {
            BinaryFormatter oWriter;
            MemoryStream ms;

            oWriter = new BinaryFormatter();
            ms = new MemoryStream();
            //
            oWriter.Serialize(ms, oMainForm.oClipColl);
            oWriter.Serialize(ms, oMainForm._lastDataFilename);
            oWriter.Serialize(ms, oMainForm._lastDataPath);
            oWriter.Serialize(ms, oMainForm._argFilename);
            oWriter.Serialize(ms, oMainForm._lastMenuState);
            oWriter.Serialize(ms, oMainForm.gSrcBuffer);
            oWriter.Serialize(ms, oMainForm.gDestBuffer);
            // Controls' content
            oWriter.Serialize(ms, oMainForm.txtSource.Rtf);
            oWriter.Serialize(ms, oMainForm.txtResult.Rtf);
            //
            File.WriteAllBytes(filename, ms.ToArray());
            ms = null;
            oWriter = null;
        }

        /// <summary>
        /// Loads previously saved workspace
        /// </summary>
        /// <param name="filename">File for application's workspace</param>
        /// <param name="oMainForm">Application's main form</param>
        public static void LoadWorkspace(string filename, frmMain oMainForm)
        {
            BinaryFormatter oReader;
            MemoryStream ms;

            if (File.Exists(filename))
            {
                oMainForm.oClipColl = new libClipCollection();
                oReader = new BinaryFormatter();
                ms = new MemoryStream(File.ReadAllBytes(filename));
                //
                oMainForm.oClipColl = (libClipCollection)oReader.Deserialize(ms);
                oMainForm._lastDataFilename = (string)oReader.Deserialize(ms);
                oMainForm._lastDataPath = (string)oReader.Deserialize(ms);
                oMainForm._argFilename = (string)oReader.Deserialize(ms);
                oMainForm._lastMenuState = (int)oReader.Deserialize(ms);
                oMainForm.gSrcBuffer = (byte[])oReader.Deserialize(ms); 
                oMainForm.gDestBuffer = (byte[])oReader.Deserialize(ms);
                // Controls' content
                oMainForm.txtSource.Rtf = (string)oReader.Deserialize(ms);
                oMainForm.txtResult.Rtf = (string)oReader.Deserialize(ms);
                //
                ms = null;
                oReader = null;
            }
        }

    } // Class MainCode
} // Namespace

