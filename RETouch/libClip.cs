using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RETouch
{
    [Serializable]
    public class libClipItem
    {
        //--------------------------------------------------------
        // libClipItem.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // UI Clip Item
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Copyright © 2018 Tmi CaseWare
        //
        // Language: C#/.NET 4.6
        //
        // Version: 1.0.0
        //
        // Created: 31.1.2018 teme64
        //
        // Modified: 14.3.2018 teme64
        //
        // License: GNU GPLv3. See http://www.gnu.org/licenses/gpl.html
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------

        public int ClipID { get; set; }
        public string ClipTitle { get; set; }
        public string ClipContentSource { get; set; }
        public string ClipContentResult { get; set; }
        public string ClipNote { get; set; }
        public List<string> ClipState { get; set; }

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        public libClipItem()
        {
            ClipState = new List<string>();
        }

        ~libClipItem()
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

        public void SaveClipState(ToolStripItemCollection oColl)
        {
            for(int i = 0; i < oColl.Count; i++)
            {
                //if(string.IsNullOrEmpty(oColl[i].Text))
                //{
                //    ClipState.Add("");
                //}
                //else
                //{
                    ClipState.Add(oColl[i].Text);
                //}
            }
        }

    } // Class libClipItem

    [Serializable]
    public class libClipCollection
    {
        //--------------------------------------------------------
        // libClipCollection.cs
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Clip Item Collection
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Copyright © 2018 Tmi CaseWare
        //
        // Language: C#/.NET 4.6
        //
        // Version: 1.0.0
        //
        // Created: 31.1.2018 teme64
        //
        // Modified: 27.2.2018 teme64
        //
        // License: GNU GPLv3. See http://www.gnu.org/licenses/gpl.html
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Public data
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Private data
        //--------------------------------------------------------

        private List<libClipItem> _coll;
        private int _nextFreeId;

        //--------------------------------------------------------
        // Constructors and destructor
        //--------------------------------------------------------

        public libClipCollection()
        {
            _coll = new List<libClipItem>();
        }

        ~libClipCollection()
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

        public void Add(libClipItem newItem)
        {
            if (newItem.ClipID == 0)
            {
                newItem.ClipID = GetNextFreeID();
            }
            else if (newItem.ClipID > _nextFreeId)
            {
                _nextFreeId = newItem.ClipID;
            }
            _coll.Add(newItem);
        }

        public libClipItem Item(int index)
        {
            return _coll.ElementAt(index);
        }

        public libClipItem ItemById(int itemId)
        {
            for (int i = 0; i < _coll.Count; i++)
            {
                if (_coll.ElementAt(i).ClipID == itemId) return _coll.ElementAt(i);
            }
            return null;
        }

        public libClipItem ItemByName(string itemName)
        {
            for (int i = 0; i < _coll.Count; i++)
            {
                if (_coll.ElementAt(i).ClipTitle.ToLower() == itemName.ToLower()) return _coll.ElementAt(i);
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
            List<string> clipNames;

            clipNames = new List<string>();
            for (int i = 0; i < _coll.Count; i++)
            {
                clipNames.Add(_coll.ElementAt(i).ClipTitle);
            }
            return clipNames;
        }

        public void SetToListBox(ListBox oBox)
        {
            oBox.Items.Clear();
            oBox.Items.AddRange(this.GetNames().ToArray());
        }

    } // Class libClipCollection

} // Namespace
