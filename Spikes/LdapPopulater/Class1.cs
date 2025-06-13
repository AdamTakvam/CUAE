using System;
using System.IO;

namespace LdapPopulater
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    class LdapPopulater
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                PopulateLDIF.Populate(Path.Combine(System.Environment.CurrentDirectory, "recs.ldif"), "Directory Manager", 2500);
            }
            else if(args.Length == 1)
            {
                PopulateLDIF.Populate(Path.Combine(System.Environment.CurrentDirectory, "recs.ldif"), args[0], 2500);
            }

            else if(args.Length == 2)
            {
                PopulateLDIF.Populate(Path.Combine(System.Environment.CurrentDirectory, "recs.ldif"), args[0], int.Parse(args[1]));
            }
            else if(args.Length == 3)
            {
                PopulateLDIF.Populate(args[2], args[0], int.Parse(args[1]));
            }
        }
    }

    public class PopulateLDIF
    {
        public static void Populate(string filePath, string managerName, int numRecs)
        {
            FileStream stream = File.Open(filePath, FileMode.Create);

            StreamWriter writer = new StreamWriter(stream);

            WriteHeader(writer, managerName);

            WriteDoods(writer, numRecs - 1);

            writer.Close();
            stream.Close();
        }

        public static void WriteDoods(StreamWriter writer, int numRecs)
        {
            for(int i = 0; i < numRecs; i++)
            {
                string firstName = "firstName" + i.ToString();
                string lastName = "lastName" + i.ToString();

                writer.WriteLine(WriteDn(firstName + " " + lastName, "people"));
                writer.WriteLine(WriteObjectClass("inetorgperson"));
                writer.WriteLine(WriteObjectClass("person"));
                writer.WriteLine(WriteFirstName(firstName));
                writer.WriteLine(WriteLastName(lastName));
                writer.WriteLine(WriteCommonName(firstName + " " + lastName));

                if( i < numRecs - 1)
                {
                    WriteSeperator(writer);
                }

                writer.Flush();
            } 
        }

        public static string WriteCommonName(string commonName)
        {
            return "cn: " + commonName;
        }

        public static string WriteFirstName(string firstName)
        {
            return "givenName: " + firstName;
        }

        public static string WriteLastName(string lastName)
        {
            return "sn: " + lastName;
        }

        public static void WriteHeader(StreamWriter writer, string managerName)
        {
            WriteDC(writer);
            WriteSeperator(writer);

            WriteOrgUnit(writer, "people");
            WriteSeperator(writer);
            
            WriteManager(writer, managerName);
            WriteSeperator(writer);   
        }

        public static void WriteDC(StreamWriter writer)
        {
            writer.WriteLine(WriteDnDc());
            writer.WriteLine(WriteObjectClass("organization"));
            writer.WriteLine(WriteObjectClass("dcObject"));
            writer.WriteLine(WriteOrg("metreos.com"));
            writer.WriteLine(WriteDc());
        }

        public static void WriteManager(StreamWriter writer, string managerName)
        {
            writer.WriteLine(WriteDn(managerName));
            writer.WriteLine(WriteObjectClass("organizationalRole"));
            writer.WriteLine(WriteCommonName(managerName));
        }

        public static void WriteOrgUnit(StreamWriter writer, string ou)
        {
            writer.WriteLine(WriteOrgDn(ou));
            writer.WriteLine(WriteObjectClass("organizationalUnit"));
            writer.WriteLine(WriteOrganizationalUnit());
        }

        public static string WriteDn(string commonName, string ou)
        {
            return "dn: cn=" + commonName + ", ou=" + ou + ", dc=metreos, dc=com";
        }
        public static string WriteDn(string commonName)
        {
            return "dn: cn=" + commonName + ", dc=metreos, dc=com";
        }

        public static string WriteOrgDn(string ou)
        {
            return "dn: ou=" + ou + ", dc=metreos, dc=com";
        }

        public static string WriteDnDc()
        {
            return "dn: dc=metreos, dc=com";
        }

        public static string WriteObjectClass(string @class)
        {
            return "objectClass: " + @class;
        }
        
        public static string WriteOrg(string org)
        {
            return "o: " + org;
        }

        public static string WriteDc()
        {
            return "dc: metreos.com";
        }

        public static string WriteOrganizationalUnit()
        {
            return "ou: people";
        }

        public static void WriteSeperator(StreamWriter writer)
        {
            writer.WriteLine();
        }
    }
}
