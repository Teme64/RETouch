using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RETouch
{
    public class libByteArray
    {
        //--------------------------------------------------------
        // libByteArray.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // libByteArray Object
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Copyright © 2017 Tmi CaseWare
        //
        // Language: C#/.NET 4.6
        //
        // Version: 1.0.0
        //
        // Created: 16.10.2017 teme64
        //
        // Modified: 16.10.2017 teme64
        //
        // License: GNU GPLv3. See http://www.gnu.org/licenses/gpl.html
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------

        public const byte Value = 0;
        public const byte Empty = 1;
        public const byte Missing = 2;

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        private byte[] _data;
        private byte[] _flags;

        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        public libByteArray()
        {
        }

        public libByteArray(byte[] value) : this()
        {
            _data = value;
        }

        ~libByteArray()
        {
        }

        //--------------------------------------------------------
        // Private procedures
        //--------------------------------------------------------

        private static string[] Slice(string value, int sliceSize)
        {
            List<string> slices;
            int sliceStartIndex;
            int sliceEndIndex;

            if (string.IsNullOrEmpty(value) || sliceSize < 1) return new string[0];
            if (sliceSize >= value.Length) return new string[] { value };
            sliceStartIndex = 0;
            sliceEndIndex = sliceSize - 1;
            slices = new List<string>();
            while (sliceEndIndex < value.Length)
            {
                slices.Add(value.Substring(sliceStartIndex, sliceEndIndex));
                sliceStartIndex += sliceSize;
                sliceEndIndex += sliceSize;
            }
            if (sliceStartIndex <= value.Length - 1) // Has at least one character left
            {
                slices.Add(value.Substring(sliceStartIndex, value.Length - sliceStartIndex));
            }
            //
            return slices.ToArray();
        }

        //--------------------------------------------------------
        // Event handlers
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public procedures
        //--------------------------------------------------------

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public byte[] Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        public int Length
        {
            get { return _data.Length; }
        }

        // Indexer for libByteArray
        // See https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/indexers/using-indexers
        public byte this[int index]
        {
            get { return _data[index]; }

            set { _data[index] = value; }
        }

        public int Compare(libByteArray byteArray)
        {
            int compareLength;

            if (byteArray == null) return 0;
            compareLength = (_data.Length > byteArray.Length) ? _data.Length : byteArray.Length;
            for (int i = 0; i < compareLength; i++)
            {
                if (i == this._data.Length) return -1;
                if (i == byteArray.Length) return 1;
                if (this._data[i] < byteArray[i]) return -1;
                if (this._data[i] > byteArray[i]) return 1;
            }
            return 0;
        }

        public string ToString(string formatString = "X2")
        {
            StringBuilder dataStr;

            if (_data == null) return "";
            dataStr = new StringBuilder();
            if(formatString == "X2")
            {
                for (int i = 0; i < _data.Length; i++)
                {
                    dataStr.Append(_data[i].ToString("X2"));
                    if (i < _data.Length - 1) dataStr.Append(" ");
                }
            }
            else
            {
                for (int i = 0; i < _data.Length; i++)
                {
                    dataStr.Append(_data[i].ToString());
                    if (i < _data.Length - 1) dataStr.Append(" ");
                }
            }
            return dataStr.ToString();
        }

        public bool MatchExact(libByteArray byteArray)
        {
            return (this.Compare(byteArray) == 0);
        }

        // Match in any position (IsSubstring kind)
        public bool Match(libByteArray byteArray)
        {
            int compareLength;
            int dataIndex;

            if (byteArray == null) return false;
            if (this.Length > byteArray.Length) return false;
            compareLength = byteArray.Length -  _data.Length;
            dataIndex = 0;
            for (int i = 0; i < compareLength; i++)
            {
                if (this._data[dataIndex] != byteArray[i])
                {
                    if(this._flags[dataIndex] == Value) // Not Missing and Not Empty
                    {
                        dataIndex = -1;
                    }
                }
                dataIndex++;
                if (dataIndex >= this.Length) return true; // Matched all bytes
            }
            return false;
        }

        public static bool TryParse(string byteStr, out libByteArray value, bool isHexInput = false)
        {
            string HEX_ALPHA_CHAR = "ABCDEF";
            string HEX_ALPHA = "0123456789ABCDEF";
            List<string> prefixes = new List<string>();
            string sepChar = " "; // Assume space as separator. Hex may be coded to constant two hex-digit stream
            string[] tokens;
            List<byte> buffer;
            byte tempByte;

            value = new libByteArray();
            // Common hex prefixes
            prefixes.Add("0x"); prefixes.Add(@"\x");
            // Check if contains any hex_alpha character
            if (!isHexInput)
            { 
                foreach(char c in HEX_ALPHA_CHAR)
                {
                    if(byteStr.Contains(c))
                    {
                        isHexInput = true;
                        break;
                    }
                }
            }
            buffer = new List<byte>();
            if (isHexInput)
            {
                if (byteStr.Contains(sepChar))
                {
                    tokens = byteStr.Split(new string[] { sepChar }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    tokens = Slice(byteStr, 2);
                }
                foreach (string s in tokens)
                {
                    tempByte = (byte)HEX_ALPHA.IndexOf(s[0]);
                    tempByte <<= 4;
                    tempByte |= (byte)HEX_ALPHA.IndexOf(s[1]);
                    buffer.Add(tempByte);
                }
            }
            else // decimal
            {
                tokens = byteStr.Split(new string[] { sepChar }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in tokens)
                {
                    if(byte.TryParse(s, out tempByte))
                    {
                        buffer.Add(tempByte);
                    }
                }
            }
            //
            return true;
        }

    }// Class libByteArray
} // Namespace
