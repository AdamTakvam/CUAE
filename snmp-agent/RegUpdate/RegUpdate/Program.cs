using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace RegUpdate
{
    class Program
    {
        string oFindExtensionKey(int nType,bool bCreateKey,string oPath)
        {
            Microsoft.Win32.RegistryKey regKey;
            regKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\SNMP\\Parameters\\ExtensionAgents", true);

            //Now put in our own key for the agent
            string oAgentValue;
            oAgentValue = String.Format("SOFTWARE\\SNMP-CUAEAGENT\\CurrentVersion");

            //Start in the second position and check a max of 50
            bool bFound = false;
            int i = 2;
            for(i=2; i<50; i++)
            {
                string oStr;
                oStr = String.Format("{0}", i);
                object ob = regKey.GetValue(oStr);

                //Did not find a valid key so break
                if(ob == null)
                {
                    break;
                }

                //Found our Agent already installed
                string oRegValue = ob.ToString();
                if(oRegValue.CompareTo(oAgentValue) == 0)
                {
                    bFound = true;
                    break;
                }
            }

            if(bCreateKey == true)
            {
                string oKeyValue;
                //If I dont have my agent installed
                if(bFound == false)
                {
                    oKeyValue = String.Format("{0}", i);
                    regKey.SetValue(oKeyValue, oAgentValue);
                }
            }

            //Clost the service key
            regKey.Close();

            string oFinalKey;
            if(nType == 0)
            {
                oFinalKey = String.Format("{0}\\{1}", oAgentValue, i);

                string oKeyValue;
                oKeyValue = String.Format("{0}\\Pathname", oAgentValue);
                Microsoft.Win32.RegistryKey regKey1;
                regKey1 = Registry.LocalMachine.OpenSubKey(oAgentValue, true);

                //I did not find the key I was looking for
                if(regKey1 == null)
                {
                    //Create the key
                    Registry.LocalMachine.CreateSubKey(oAgentValue);
                    regKey1 = Registry.LocalMachine.OpenSubKey(oAgentValue, true);

                    //Set the pathname vaule
                    oKeyValue = String.Format("{0}", "Pathname");
                    regKey1.SetValue(oKeyValue, oPath);

                    //close the software key
                    regKey1.Close();
                }

            }
            else
            {
                oFinalKey = String.Format("{0}",i);
            }

            return oFinalKey;
        }

        static void Main(string[] args)
        {
            //New instance
            Program oMain = new Program();

            if(args.Length == 0)
            {
                Console.WriteLine("-i to Install, -r to Remove");
                return;
            }

            //Do remove of key and value
            if(args[0].CompareTo("-r") == 0)
            {
                Microsoft.Win32.RegistryKey regKey;
                regKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\SNMP\\Parameters\\ExtensionAgents", true);

                //Find the Agent Key and create if it does to exist
                string oAgentValue;
                oAgentValue = oMain.oFindExtensionKey(1,false,"");

                try
                {
                    regKey.DeleteValue(oAgentValue);
                    regKey.Close();
                 }
                catch(SystemException se)
                {
                    string oE = se.GetBaseException().ToString();
                }

                
                oAgentValue = String.Format("SOFTWARE\\SNMP-CUAEAGENT\\CurrentVersion");
                try
                {
                    try
                    {
                        regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\SNMP-CUAEAGENT\\CurrentVersion", true);
                        regKey.DeleteValue("Pathname");
                        regKey.Close();

                        Registry.LocalMachine.DeleteSubKey("SOFTWARE\\SNMP-CUAEAGENT\\CurrentVersion");
                        Registry.LocalMachine.DeleteSubKey("SOFTWARE\\SNMP-CUAEAGENT");
                    }
                    catch(SystemException se)
                    {
                        string oE = se.GetBaseException().ToString();
                    }

                }
                catch(SystemException se)
                {
                    string oE = se.GetBaseException().ToString();
                }

                Console.WriteLine("Removed SNMP-CUAEAGENT from Registry");
            }

            //Need to Do an Install
            else if(args[0].CompareTo("-i") == 0)
            {
                string oPath;

                int nVal = args.Length;
                if (nVal == 2)
                {
                    oPath = args[1];
                }
                else
                {
                    oPath = String.Format("{0}", "C:\\Program Files\\Cisco Systems\\Unified Application Environment\\StatsService\\CUAEAgent.dll");
                }

                //Find the Agent Key and create if it does to exist
                string oAgentValue;
                oAgentValue = oMain.oFindExtensionKey(0, true,oPath);
                Console.WriteLine("Installed SNMP-CUAEAGENT to Registry");

            }
            else
            {
                Console.WriteLine("-i to Install, -r to Remove");
            }
        }
    }
}
