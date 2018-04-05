using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RETouch
{
    [Serializable]
    public class libByteTemplate
    {
        //--------------------------------------------------------
        // libByteTemplate.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // libByteTemplate Class for Byte Output
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Copyright © 2017 Tmi CaseWare
        //
        // Language: C#/.NET 4.6
        //
        // Version: 1.0.0
        //
        // Created: 14.10.2017 teme64
        //
        // Modified: 14.10.2017 teme64
        //
        // License: GNU GPLv3. See http://www.gnu.org/licenses/gpl.html
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------

        public int byteTemplateId { get; set; }
        public string byteTemplateName { get; set; }
        public string byteTemplateAsString { get; set; }
        public int[] byteTemplate { get; set; }
        public string templateSeparator { get; set; }

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        public libByteTemplate()
        {
            byteTemplateName = "";
            byteTemplateAsString = "";
            byteTemplate = new int[0];
            templateSeparator = "";
        }

        ~libByteTemplate()
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

    } // Class libByteTemplate

    [Serializable]
    public class libByteTemplates
    {
        //--------------------------------------------------------
        // libByteTemplates.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // libByteTemplates Collection
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Copyright © 2017 Tmi CaseWare
        //
        // Language: C#/.NET 4.6
        //
        // Version: 1.0.0
        //
        // Created: 14.10.2017 teme64
        //
        // Modified: 14.10.2017 teme64
        //
        // License: GNU GPLv3. See http://www.gnu.org/licenses/gpl.html
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------

        public enum SPECIAL_VALUE : int
        {
            MATCH_ANY = 0,
            MISSING = 1
        }

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        private List<libByteTemplate> _coll;
        private int _lastId;

        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        public libByteTemplates()
        {
            _coll = new List<libByteTemplate>();
        }

        ~libByteTemplates()
        {
            _coll = null;
        }

        //--------------------------------------------------------
        // Private procedures
        //--------------------------------------------------------

        private int getNextId()
        {
            _lastId++;
            return _lastId;
        }

        //--------------------------------------------------------
        // Event handlers
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public procedures
        //--------------------------------------------------------

        public void Add(libByteTemplate newItem)
        {
            if (newItem.byteTemplateId == 0)
            {
                newItem.byteTemplateId = getNextId();
            }
            else
            {
                if (newItem.byteTemplateId > _lastId) _lastId = newItem.byteTemplateId;
            }
            _coll.Add(newItem);
        }

        public libByteTemplate Item(int index)
        {
            return _coll.ElementAt(index);
        }

        public libByteTemplate ItemById(int itemId)
        {
            for (int i = 0; i < _coll.Count; i++)
            {
                if (_coll.ElementAt(i).byteTemplateId == itemId) return _coll.ElementAt(i);
            }
            return null;
        }

        public libByteTemplate ItemByName(string templateName)
        {
            for (int i = 0; i < _coll.Count; i++)
            {
                if (_coll.ElementAt(i).byteTemplateName.ToLower() == templateName.ToLower()) return _coll.ElementAt(i);
            }
            return null;
        }

        public int Count()
        {
            return _coll.Count;
        }

    } // Class libFolders
} // Namespace
