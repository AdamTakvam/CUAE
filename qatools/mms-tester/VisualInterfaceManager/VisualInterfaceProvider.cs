using System;
using System.IO;
using System.Threading;
using System.Collections;

using Metreos.MmsTester.Interfaces;
using Metreos.MmsTester.VisualInterfaceFramework;

namespace Metreos.MmsTester.VisualInterfaceManager
{
	/// <summary>
	/// Scans and provides visual interfaces to the VisualInterfaceManager
	/// </summary>
	public class VisualInterfaceProvider
	{
        ArrayList availableVisualInterfaces;

		public VisualInterfaceProvider()
		{
		    availableVisualInterfaces = new ArrayList();	

            availableVisualInterfaces = LoadAssemblies();
		}

        public string[] AvailableVisualAdapters()
        {
            string[] availableVisualInterfacesNames = new string[availableVisualInterfaces.Count];

            for(int i = 0; i < availableVisualInterfaces.Count; i++)
            {
                object[] attributes = ((System.Reflection.Assembly)availableVisualInterfaces[i]).GetCustomAttributes(false);
                
                for(int j = 0; j < attributes.Length; j++)
                {
                    if(attributes[j] is VisualInterfaceAttribute)
                    {
                        availableVisualInterfacesNames[i] = ( (VisualInterfaceAttribute) attributes[j]).DisplayName;
                        break;
                    }
                }
            }

            return availableVisualInterfacesNames;
        }

        public bool GrabVisualInterface(string displayName, out System.Reflection.Assembly assembly)
        {
            assembly = null;

            for(int i = 0; i < availableVisualInterfaces.Count; i++)
            {
                object[] attributes = ((System.Reflection.Assembly)availableVisualInterfaces[i]).GetCustomAttributes(false);
                
                for(int j = 0; j < attributes.Length; j++)
                {
                    if(attributes[j] is VisualInterfaceAttribute)
                    {
                        string displayNameFromAssembly = ( (VisualInterfaceAttribute) attributes[j]).DisplayName;

                        if( displayName == displayNameFromAssembly )
                        {
                            assembly = (System.Reflection.Assembly) availableVisualInterfaces[i];
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

            string[] files = Directory.GetFiles(ICompilation.PATH_TO_VISUAL_INTERFACES, "Metreos.MmsTester.*");
            
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
                            if(attr is VisualInterfaceAttribute)
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
