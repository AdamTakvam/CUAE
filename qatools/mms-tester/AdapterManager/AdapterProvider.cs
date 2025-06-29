using System;
using System.IO;
using System.Collections;

using Metreos.MmsTester.Interfaces;
using Metreos.MmsTester.AdapterFramework;

namespace Metreos.MmsTester.AdapterManager
{
	/// <summary>
	/// Provides adapters to the AdapterManager
	/// </summary>
	public class AdapterProvider
	{ 
        public ArrayList availableAdapters;

		public AdapterProvider()
		{
			availableAdapters = LoadAssemblies();
		}

        // Called on startup
        public string[] AvailableAdapters()
        {
            string[] availableAdapterNames = new string[availableAdapters.Count];

            for(int i = 0; i < availableAdapters.Count; i++)
            {
                object[] attributes = ((System.Reflection.Assembly)availableAdapters[i]).GetCustomAttributes(false);
                
                for(int j = 0; j < attributes.Length; j++)
                {
                    if(attributes[j] is AdapterAttribute)
                    {
                        availableAdapterNames[i] = ( (AdapterAttribute) attributes[j]).DisplayName;
                        break;
                    }
                }
            }

            return availableAdapterNames;
        }

        public bool GrabAdapter(string displayName, out System.Reflection.Assembly assembly)
        {
            assembly = null;

            for(int i = 0; i < availableAdapters.Count; i++)
            {
                object[] attributes = ((System.Reflection.Assembly)availableAdapters[i]).GetCustomAttributes(false);
                
                for(int j = 0; j < attributes.Length; j++)
                {
                    if(attributes[j] is AdapterAttribute)
                    {
                        string displayNameFromAssembly = ( (AdapterAttribute) attributes[j]).DisplayName;

                        if( displayName == displayNameFromAssembly )
                        {
                            assembly = (System.Reflection.Assembly) availableAdapters[i];
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static ArrayList LoadAssemblies()
        { 
            ArrayList appropriateAssemblies = new ArrayList();

            string[] files = Directory.GetFiles(ICompilation.PATH_TO_ADAPTERS, "Metreos.MmsTester.*");
            
            for(int i = 0; i < files.Length; i++)
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(files[i]);
                
                Type[] types = assembly.GetTypes();

                foreach(System.Type t in types)
                {
                    if(t.IsClass == true)
                    {                    
                        foreach(System.Attribute attr in t.GetCustomAttributes(false))
                        {
                            if(attr is AdapterAttribute)
                            {
                                appropriateAssemblies.Add(assembly);
                            }
                        }
                    }
                }
            }

            return appropriateAssemblies;
        }
	}
}
