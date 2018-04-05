using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RETouch
{
    [Serializable]
    public class UserSettings
    {
        //--------------------------------------------------------
        // UserSettings.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // UserSettings Class Library
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Copyright © 2017-2018 Tmi CaseWare
        //
        // Language: C#/.NET 4.6
        //
        // Version: 1.0.0
        //
        // Created: 6.10.2017 teme64
        //
        // Modified: 1.3.2018 teme64
        //
        // License: GNU GPLv3. See http://www.gnu.org/licenses/gpl.html
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------

        public int StringMinimumLength { get; set; }
        public bool StringSkipUnlike { get; set; }
        public bool StringSearchBOMs { get; set; }
        public bool DisplayNumberFormatDecimal { get; set; }
        public bool StringMatchingCaseSensitive { get; set; }

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        public UserSettings()
        {
            StringMinimumLength = 5;
            StringSkipUnlike = false;
            StringSearchBOMs = false;
            DisplayNumberFormatDecimal = false;
            StringMatchingCaseSensitive = true;
        }

        ~UserSettings()
        {
        }

        //--------------------------------------------------------
        // Private procedures
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Event handlers
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public procedures
        //--------------------------------------------------------

    } // Class UserSettings
} // Namespace
