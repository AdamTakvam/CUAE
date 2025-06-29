using System;
using System.Collections;
using System.Collections.Generic;

namespace Metreos.AppServer.ARE.Collections
{
    /// <summary>Contains the locale string table data</summary>
    /// <remarks>
    /// This class is a dictionary of string dictionaries.
    /// The first key is the locale, as in:
    /// HashTable localeValues = stringTable[locale] as HashTable;
    /// the second key is the string name, as in:
    /// string localizedString = localeValues[stringName] as string;
    /// </remarks>
    [Serializable]
    public sealed class StringTable
    {
        private readonly Hashtable table;

        public StringTable()
        {
            this.table = new Hashtable();
        }

        public void Add(string locale, Hashtable lValues)
        {
            table.Add(locale, lValues);
        }

        public string GetTableValue(string stringName, string locale)
        {
            Hashtable lTable = table[locale] as Hashtable;
            if(lTable != null)
                return lTable[stringName] as string;

            return null;
        }

        public List<string> GetLocales()
        {
            List<string> locales = new List<string>(table.Keys.Count);

            foreach(string locale in table.Keys)
            {
                locales.Add(locale);
            }

            return locales;
        }
    }
}
