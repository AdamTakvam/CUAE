using System;
using System.Diagnostics;
using System.Collections.Specialized;

namespace Metreos.Max.Drawing
{
	/// <summary> Defines a configuration item defined by the user.  Today, that is limited to an enumeration of string values</summary>
	public class UserConfigType
	{
        public string Name { get { return name; } set { name = value; } }
        public StringCollection Values { get { return values; } set { values = value; } }

        private string name;
        private StringCollection values;

        public UserConfigType(string name)
        {
            this.Values = new StringCollection();
            Debug.Assert(name != null && name != String.Empty, "Name must be defined when creating UserConfigType");
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }

        public bool IsSameName(string name)
        {
            return 0 == String.Compare(this.Name, name, true);
        }

        public UserConfigType Copy()
        {
            UserConfigType copy = new UserConfigType(name);

            string[] copyValues = new string[values.Count];
            values.CopyTo(copyValues, 0);
            copy.Values.AddRange(copyValues);

            return copy;
        }
	}
}
