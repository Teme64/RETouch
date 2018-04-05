using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RETouch
{
    public static class libCoding
    {
        //--------------------------------------------------------
        // libCoding.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // libCoding Library for Robust Base64 Decoding
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Copyright © 2017-2018 Tmi CaseWare
        //
        // Language: C#/.NET 4.6
        //
        // Version: 1.0.0
        //
        // Created: 16.10.2017 teme64
        //
        // Modified: 11.3.2018 teme64
        //
        // License: GNU GPLv3. See http://www.gnu.org/licenses/gpl.html
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------

        public static int AlphabetSize { get; set; }
        public static Encoding StringEncoding { get; set; }

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        private static string _Base64Alphabet62 = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private static string _Base64Alphabet2 = @" +/";
        private static string _Base64PadChar = @"=";
        
        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        //public libCoding()
        //{
        //}

        //~libCoding()
        //{
        //}

        //--------------------------------------------------------
        // Private procedures
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Event handlers
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public procedures
        //--------------------------------------------------------

        public static byte[] Caesar(byte[] value, int rot)
        {
            List<byte> output;
            byte newChar;

            if (value == null || value.Length < 1) return new byte[0];
            if (AlphabetSize < 1) return value;
            output = new List<byte>();
            foreach (byte b in value)
            {
                newChar = ((b + rot) > AlphabetSize) ? (byte)((b + rot) % AlphabetSize) : (byte)(b + rot);
                output.Add(newChar);
            }
            //
            return output.ToArray();
        }

        public static string Caesar(string value, int rot)
        {
            string rottedStr = "";
            byte[] buffer;
            byte[] output;

            if (string.IsNullOrEmpty(value)) return rottedStr;
            if (StringEncoding == null) StringEncoding = Encoding.ASCII;
            buffer = StringEncoding.GetBytes(value);
            output = Caesar(buffer, rot);
            rottedStr = new string(StringEncoding.GetChars(output.ToArray()));
            //
            return rottedStr;
        }

        public static string EncodeROT(string msg, string srcAlphabet, string destAlphabet, bool skipMissingChar = false)
        {
            int index;
            string newStr;

            newStr = "";
            for (int i = 0; i < msg.Length; i++)
            {
                index = srcAlphabet.IndexOf(msg[i]);
                if (index >= 0)
                {
                    newStr += destAlphabet[index];
                }
                else
                {
                    if (!skipMissingChar)
                    {
                        newStr += msg[i];
                    }
                }
            }
            //
            return newStr;
        }

        public static byte[] EncodeROT(byte[] msg, byte[] srcAlphabet, byte[] destAlphabet, bool skipMissingChar = false)
        {
            int index;
            List<byte> newStr;

            newStr = new List<byte>();
            for (int i = 0; i < msg.Length; i++)
            {
                index = srcAlphabet.ToList().IndexOf(msg[i]);
                if (index >= 0)
                {
                    newStr.Add(destAlphabet[index]);
                }
                else
                {
                    if (!skipMissingChar)
                    {
                        newStr.Add(msg[i]);
                    }
                }
            }
            //
            return newStr.ToArray();
        }

    } // Class libCoding
} // Namespace
