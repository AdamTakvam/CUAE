using System;
using System.IO;
using System.Collections;

using Metreos.MmsTester.Interfaces;
using Metreos.MmsTester.ClientFramework;

namespace Metreos.MmsTester.ClientManager
{
    /// <summary>
    /// Provides adapters to the AdapterManager
    /// </summary>
    public class ClientProvider
    { 
        public ArrayList availableClients;

        public ClientProvider()
        {
            availableClients = LoadAssemblies();
        }

        // Exposes all the clients evailable
        public string[] AvailableClients()
        {
            string[] availableClientNames = new string[availableClients.Count];

            for(int i = 0; i < availableClientNames.Length; i++)
            {
                object[] attributes = ((System.Reflection.Assembly)availableClients[i]).GetCustomAttributes(false);
                
                for(int j = 0; j < attributes.Length; j++)
                {
                    if(attributes[j] is ClientAttribute)
                    {
                        availableClientNames[i] = ( (ClientAttribute) attributes[j]).DisplayName;
                        break;
                    }
                }
            }

            return availableClientNames;
        }

        public bool GrabClient(string displayName, out System.Reflection.Assembly assembly)
        {
            assembly = null;

            for(int i = 0; i < availableClients.Count; i++)
            {
                object[] attributes = ((System.Reflection.Assembly)availableClients[i]).GetCustomAttributes(false);
                
                for(int j = 0; j < attributes.Length; j++)
                {
                    if(attributes[j] is ClientAttribute)
                    {
                        string displayNameFromAssembly = ( (ClientAttribute) attributes[j]).DisplayName;

                        if( displayName == displayNameFromAssembly )
                        {
                            assembly = (System.Reflection.Assembly) availableClients[i];
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

            string[] files = Directory.GetFiles(ICompilation.PATH_TO_CLIENTS, "Metreos.MmsTester.*");
            
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
                            if(attr is ClientAttribute)
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
