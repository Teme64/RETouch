using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace RETouch
{
    public class libScriptItem
    {
        //--------------------------------------------------------
        // libScriptItem.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Script Object
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Copyright © 2017 Tmi CaseWare
        //
        // Language: C#/.NET 4.6
        //
        // Version: 1.0.0
        //
        // Created: 2.11.2017 teme64
        //
        // Modified: 5.11.2017 teme64
        //
        // License: GNU GPLv3. See http://www.gnu.org/licenses/gpl.html
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------

        internal int ScriptID { get; set; }
        public string ScriptName { get; set; }
        public string ScriptFilename { get; set; }
        public string ScriptArgs { get; set; }
        public string ScriptHelpFilename { get; set; }

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        public libScriptItem()
        {
            ScriptName = "";
            ScriptFilename = "";
            ScriptArgs = "";
            ScriptHelpFilename = "";
        }

        ~libScriptItem()
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

    } // Class libScriptItem

    public class libScripts
    {
        //--------------------------------------------------------
        // libScripts.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Script Collection
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Copyright © 2017-2018 Tmi CaseWare
        //
        // Language: C#/.NET 4.6
        //
        // Version: 1.0.0
        //
        // Created: 2.11.2017 teme64
        //
        // Modified: 1.2.2018 teme64
        //
        // License: GNU GPLv3. See http://www.gnu.org/licenses/gpl.html
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        private List<libScriptItem> _coll;
        private int _nextFreeId;

        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        public libScripts()
        {
            _coll = new List<libScriptItem>();
        }

        ~libScripts()
        {
            _coll = null;
        }

        //--------------------------------------------------------
        // Private procedures
        //--------------------------------------------------------

        private int GetNextFreeID()
        {
            _nextFreeId++;
            return _nextFreeId;
        }

        //--------------------------------------------------------
        // Event handlers
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public procedures
        //--------------------------------------------------------

        public void Add(libScriptItem newItem)
        {
            if (newItem.ScriptID == 0)
            {
                newItem.ScriptID = GetNextFreeID();
            }
            else if (newItem.ScriptID > _nextFreeId)
            {
                _nextFreeId = newItem.ScriptID;
            }
            _coll.Add(newItem);
        }

        public libScriptItem Item(int index)
        {
            return _coll.ElementAt(index);
        }

        public libScriptItem ItemById(int itemId)
        {
            for (int i = 0; i < _coll.Count; i++)
            {
                if (_coll.ElementAt(i).ScriptID == itemId) return _coll.ElementAt(i);
            }
            return null;
        }

        public libScriptItem ItemByName(string itemName)
        {
            for (int i = 0; i < _coll.Count; i++)
            {
                if (_coll.ElementAt(i).ScriptName.ToLower() == itemName.ToLower()) return _coll.ElementAt(i);
            }
            return null;
        }

        public int Count()
        {
            return _coll.Count;
        }

        // Special

        public List<string> GetNames()
        {
            List<string> scriptNames;

            scriptNames = new List<string>();
            for (int i = 0; i < _coll.Count; i++)
            {
                scriptNames.Add(_coll.ElementAt(i).ScriptName);
            }
            return scriptNames;
        }

    } // Class libScripts

} // Namespace
