using System;
using System.Collections;
using System.Collections.Specialized;

namespace Metreos.Utilities.Collections
{
	/// <summary>Collection of items (keys) witch can be checked or unchecked, like taking a tally</summary>
	/// <remarks>Not safe for multiple writing threads</remarks>
	public class TallyCollection
	{
        /// <summary>Collection of items to be checked off</summary>
        private readonly StringCollection items;

        /// <summary>Collection of items to be checked off</summary>
        private readonly StringCollection checkedItems;

        public bool AllChecked { get { return this.items.Count == this.checkedItems.Count; } }
            
        public TallyCollection()
		{
            this.items = new StringCollection();
            this.checkedItems = new StringCollection();
		}

        public void AddItem(string name)
        {
            if(!this.items.Contains(name))
                this.items.Add(name);
        }

        public void AddItems(string[] names)
        {
            this.items.AddRange(names);
        }

        public void Check(string name)
        {
            if(!this.checkedItems.Contains(name) &&
                this.items.Contains(name))
            {
                this.checkedItems.Add(name);
            }
        }

        public void Uncheck(string name)
        {
            this.checkedItems.Remove(name);
        }

        public void UncheckAll()
        {
            this.checkedItems.Clear();
        }

        public StringCollection GetUncheckedNames()
        {
            StringCollection uncheckedNames = new StringCollection();

            foreach(string name in items)
            {
                if(!this.checkedItems.Contains(name))
                    uncheckedNames.Add(name);
            }
            return uncheckedNames;
        }

        public void Clear()
        {
            this.items.Clear();
            this.checkedItems.Clear();
        }
	}
}
