using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace RETouchResource
{
    public static class libResx
    {
        //--------------------------------------------------------
        // libResx.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // RETouch Resource File
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Copyright © 2018 Tmi CaseWare
        //
        // Language: C#/.NET 4.6
        //
        // Version: 1.0.0
        //
        // Created: 11.3.2018 teme64
        //
        // Modified: 11.3.2018 teme64
        //
        // License: GNU GPLv3. See http://www.gnu.org/licenses/gpl.html
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        private static string[] apiFilenames;
        private static string[] wordListFilenames;
        private static string[] redWordListFilenames;
        private static string[] scriptWordListFilenames;
        private static string[] documentWordListFilenames;

        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        //public libResx()
        //{
        //}

        //~libResx()
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

        //public static List<string> GetAPIFilenames(string cfgFilename)
        //{
        //    List<string> newList;
        //    string[] allLines;
        //    char[] splitChar;

        //    newList = new List<string>();
        //    //
        //    splitChar = new char[] { ';' };
        //    if (File.Exists(cfgFilename))
        //    {
        //        allLines = File.ReadAllLines(cfgFilename);
        //        for (int i = 0; i < allLines.Length; i++)
        //        {
        //            if (allLines[i].StartsWith("#")) continue; // Comment line, skip it
        //            if (allLines[i].ToLower() == "[apifilenames]") // Section starts
        //            {
        //                i++;
        //                while (i < allLines.Length && !allLines[i].StartsWith("["))
        //                {
        //                    newList.Add(allLines[i]);
        //                    i++;
        //                }
        //                return newList;
        //            }
        //        }
        //    }
        //    //
        //    return newList;
        //}

        public static List<string> GetWordList(List<string> currentWordList)
        {
            string oneLine;
            StreamReader reader;

            wordListFilenames = new string[4];
            wordListFilenames[0] = RETouchResource.Properties.Resources.retouch_10_million_passw;
            wordListFilenames[1] = RETouchResource.Properties.Resources.retouch_wordlist;
            wordListFilenames[2] = RETouchResource.Properties.Resources.retouch_wordlist_wechall;
            wordListFilenames[3] = RETouchResource.Properties.Resources.retouch_all3;

            if (currentWordList == null)
            {
                currentWordList = new List<string>();
            }
            //
            for (int i = 0; i < wordListFilenames.Length; i++)
            {
                reader = new StreamReader(wordListFilenames[i]);
                while (!reader.EndOfStream)
                {
                    oneLine = reader.ReadLine();
                    if (!currentWordList.Contains(oneLine))
                    {
                        currentWordList.Add(oneLine);
                    }
                }
                reader.Close();
            }
            //
            return currentWordList;
        }

        public static List<string> GetRedWordList(List<string> currentWordList)
        {
            string oneLine;
            StreamReader reader;

            redWordListFilenames = new string[4];
            redWordListFilenames[0] = RETouchResource.Properties.Resources.retouch_wordlist_redwords;

            if (currentWordList == null)
            {
                currentWordList = new List<string>();
            }
            //
            for (int i = 0; i < redWordListFilenames.Length; i++)
            {
                reader = new StreamReader(redWordListFilenames[i]);
                while (!reader.EndOfStream)
                {
                    oneLine = reader.ReadLine();
                    if (!currentWordList.Contains(oneLine))
                    {
                        currentWordList.Add(oneLine);
                    }
                }
                reader.Close();
            }
            //
            return currentWordList;
        }

        public static Dictionary<string, string> GetAPIDictionary(Dictionary<string, string> currentAPIDictionary)
        {
            string oneLine;
            string[] lineArr;
            //StreamReader reader;
            StringReader reader;
            char[] splitChar;

            apiFilenames = new string[18];

            apiFilenames[0] = RETouchResource.Properties.Resources.retouch_apis_advapi32;
            apiFilenames[1] = RETouchResource.Properties.Resources.retouch_apis_comctl32;
            apiFilenames[2] = RETouchResource.Properties.Resources.retouch_apis_comdlg32;
            apiFilenames[3] = RETouchResource.Properties.Resources.retouch_apis_crypt32;
            apiFilenames[4] = RETouchResource.Properties.Resources.retouch_apis_gdi32;
            apiFilenames[5] = RETouchResource.Properties.Resources.retouch_apis_imm32;
            apiFilenames[6] = RETouchResource.Properties.Resources.retouch_apis_kernel32;
            apiFilenames[7] = RETouchResource.Properties.Resources.retouch_apis_mpr;
            apiFilenames[8] = RETouchResource.Properties.Resources.retouch_apis_netapi32;
            apiFilenames[9] = RETouchResource.Properties.Resources.retouch_apis_ntdll;
            apiFilenames[10] = RETouchResource.Properties.Resources.retouch_apis_ole32;
            apiFilenames[11] = RETouchResource.Properties.Resources.retouch_apis_oleaut32;
            apiFilenames[12] = RETouchResource.Properties.Resources.retouch_apis_shell32;
            apiFilenames[13] = RETouchResource.Properties.Resources.retouch_apis_shlwapi;
            apiFilenames[14] = RETouchResource.Properties.Resources.retouch_apis_user32;
            apiFilenames[15] = RETouchResource.Properties.Resources.retouch_apis_version;
            apiFilenames[16] = RETouchResource.Properties.Resources.retouch_apis_wininet;
            apiFilenames[17] = RETouchResource.Properties.Resources.retouch_apis_winspool;

            if (currentAPIDictionary == null)
            {
                currentAPIDictionary = new Dictionary<string, string>();
            }
            //
            splitChar = new char[] { ';' };
            for (int i = 0; i < apiFilenames.Length; i++)
            {
                try
                {
                    //reader = new StreamReader(apiFilenames[i]);
                    reader = new StringReader(apiFilenames[i]);
                }
                catch (IOException)
                {
                    return currentAPIDictionary;
                }
                //while (!reader.EndOfStream)
                while (reader.Peek() >= 0)
                {
                    oneLine = reader.ReadLine();
                    lineArr = oneLine.Split(splitChar);
                    if (!currentAPIDictionary.ContainsKey(lineArr[1]))
                    {
                        currentAPIDictionary.Add(lineArr[1], lineArr[0]);
                    }
                }
                reader.Close();
            }
            //
            return currentAPIDictionary;
        }

        public static Dictionary<string, string> GetDocumentWordDictionary(Dictionary<string, string> currentAPIDictionary)
        {
            string oneLine;
            string[] lineArr;
            //StreamReader reader;
            StringReader reader;
            char[] splitChar;

            documentWordListFilenames = new string[2];
            documentWordListFilenames[0] = RETouchResource.Properties.Resources.retouch_wordlist_document_pdf;
            documentWordListFilenames[1] = RETouchResource.Properties.Resources.retouch_wordlist_document_docx;

            if (currentAPIDictionary == null)
            {
                currentAPIDictionary = new Dictionary<string, string>();
            }
            //
            splitChar = new char[] { ';' };
            for(int i = 0; i < documentWordListFilenames.Length; i++)
            { 
                try
                {
                    //reader = new StreamReader(documentWordListFilenames[i]);
                    reader = new StringReader(documentWordListFilenames[i]);
                }
                catch (IOException)
                {
                    return currentAPIDictionary;
                }
                //while (!reader.EndOfStream)
                while (reader.Peek() >= 0)
                {
                    oneLine = reader.ReadLine();
                    lineArr = oneLine.Split(splitChar);
                    if (!currentAPIDictionary.ContainsKey(lineArr[1]))
                    {
                        currentAPIDictionary.Add(lineArr[1], lineArr[0]);
                    }
                }
                reader.Close();
            }
            //
            return currentAPIDictionary;
        }

        public static Dictionary<string, string> GetScriptWordDictionary(Dictionary<string, string> currentAPIDictionary)
        {
            string oneLine;
            string[] lineArr;
            //StreamReader reader;
            StringReader reader;
            char[] splitChar;

            scriptWordListFilenames = new string[6];
            scriptWordListFilenames[0] = RETouchResource.Properties.Resources.retouch_wordlist_javascript;
            scriptWordListFilenames[1] = RETouchResource.Properties.Resources.retouch_wordlist_vbscript;
            scriptWordListFilenames[2] = RETouchResource.Properties.Resources.retouch_wordlist_vbe;
            scriptWordListFilenames[3] = RETouchResource.Properties.Resources.retouch_wordlist_csharp;
            scriptWordListFilenames[4] = RETouchResource.Properties.Resources.retouch_wordlist_powershell;
            scriptWordListFilenames[5] = RETouchResource.Properties.Resources.retouch_wordlist_html;

            if (currentAPIDictionary == null)
            {
                currentAPIDictionary = new Dictionary<string, string>();
            }
            //
            splitChar = new char[] { ';' };
            for (int i = 0; i < scriptWordListFilenames.Length; i++)
            {
                try
                {
                    //reader = new StreamReader(scriptWordListFilenames[i]);
                    reader = new StringReader(scriptWordListFilenames[i]);
                }
                catch (IOException)
                {
                    return currentAPIDictionary;
                }
                //while (!reader.EndOfStream)
                while (reader.Peek() >= 0)
                {
                    oneLine = reader.ReadLine();
                    lineArr = oneLine.Split(splitChar);
                    if (!currentAPIDictionary.ContainsKey(lineArr[1]))
                    {
                        currentAPIDictionary.Add(lineArr[1], lineArr[0]);
                    }
                }
                reader.Close();
            }
            //
            return currentAPIDictionary;
        }

    } // Class libResx
} // Namespace
