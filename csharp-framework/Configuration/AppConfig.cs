using System;
using System.Collections;
using System.Configuration;

namespace Metreos.Configuration
{
    /// <summary>
    /// Collection of methods for manipulating app.config files.
    /// </summary>
    public abstract class AppConfig
    {
        public static string GetEntry(string entryName)
        {
            string entryValue = null;

            try
            {
                entryValue = ConfigurationManager.AppSettings.Get(entryName);
            }
            catch (System.Configuration.ConfigurationException)
            {
                // ignore ConfigurationExceptions only
            }
            
            return entryValue;
        }

        /// <summary>
        /// Retrieves all appSettings from the app.config file, and returns a Hashtable
        /// where the keys in the table correspond to keys in the appSettings section.
        /// Does not support multiple instances of an appSetting key.
        /// </summary>
        /// <returns>Hashtable of key->value appSettings, or null if no keys were retrieved</returns>
        public static Hashtable GetTable()
        {
            Hashtable settingsTable = null;
            System.Collections.Specialized.NameValueCollection nvCollection = null;

            try
            {
                nvCollection = ConfigurationManager.AppSettings;
            }
            catch (System.Configuration.ConfigurationException)
            {
                // ignore ConfigurationExceptions only
            }
            
            if (nvCollection != null && nvCollection.Count > 0)
            {
                settingsTable = new Hashtable();
                foreach (string key in nvCollection.Keys)
                    settingsTable[key] = nvCollection[key];        
            }

            return settingsTable;
        }
    }
}