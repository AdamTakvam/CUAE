using System;
using System.Collections;
using System.Collections.Specialized;

namespace Metreos.ApplicationFramework.Collections
{
	/// <summary>
	/// A clonable StringDictionary.
	/// </summary>
	public class MStringDictionary : StringDictionary
	{
        public override string this[string key]
        {
            get { return base[key.ToLower()]; }
            set { base[key.ToLower()] = value; }
        }

        public MStringDictionary()
            : base() {}

        public override void Add(string key, string value)
        {
            base.Add(key.ToLower(), value);
        }

        public MStringDictionary Clone()
        {
            MStringDictionary newDict = new MStringDictionary();

            foreach(DictionaryEntry de in this)
				newDict.Add(de.Key as string, de.Value as string);

            return newDict;
        }
	}
}
