using System;
using Microsoft.Win32;


namespace Metreos.Max.Core
{
    /// <summary>Max Designer registry access</summary>
    public sealed class Reg
    {
        #region singleton
        private Reg() {}
        private static Reg instance;
        public  static Reg Instance
        { get 
          { if (instance == null)
            {
                instance = new Reg();
                instance.Init();
            }
            return instance;
          }
        }
        #endregion

        private void Init()
        {
        }

   
        public static string GetStringValue(string key, string subkey)
        { 
            RegistryKey mainKey = Const.regMainKey;
            string regval = null;
      
            try
            {   RegistryKey regKey = mainKey.OpenSubKey(key, true);
                if (regKey == null)
                    regKey  = mainKey.CreateSubKey(key);
              
                regval = (string)regKey.GetValue(subkey);
            }
            catch { }

            return regval;
        }


        public static bool SetStringValue(string key, string subkey, string val)
        {
            RegistryKey mainKey = Const.regMainKey;
            bool result = false;

            try 
            {   RegistryKey regKey = mainKey.OpenSubKey(key, true);
                if (regKey == null)
                    regKey  = mainKey.CreateSubKey(key);
       
                regKey.SetValue(subkey, val);
                result = true;         
            } 
            catch { }

            return result;
        } 


    } // class Reg
}     // namespace
