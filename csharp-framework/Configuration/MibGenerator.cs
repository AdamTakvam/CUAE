using System;
using System.IO;
using System.Text;
using System.Collections;

using Metreos.Interfaces;
using Metreos.Core.ConfigData;

namespace Metreos.Configuration
{
    public abstract class MibGenerator
    {
        public static void GenerateMIB()
        {
            GenerateMIB(AppDomain.CurrentDomain.BaseDirectory);
        }

        /// <summary>Generates a MIB file from config DB data</summary>
        /// <param name="path">Directory in which to create the MIB file</param>
        /// <returns>success</returns>
        /// <remarks>Exceptions:
        /// System.ArgumentException
        /// System.ArgumentNullException
        /// System.UnauthorizedAccessException
        /// System.NotSupportedException
        /// System.IO.IOException
        /// System.IO.PathTooLongException
        /// System.IO.DirectoryNotFoundException
        /// </remarks>
        public static void GenerateMIB(string path)
        {
            // Create MIB string
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(Consts.MibPrefix, 
                DateTime.Now.ToString("MMMM dd, yyyy"),
                Config.Instance.GetOidRoot());
            sb.AppendLine();

            foreach(DictionaryEntry de in Config.Instance.GetOIDs())
            {
                string oid = Convert.ToString(de.Key);
                SnmpOid data = de.Value as SnmpOid;
                
                string description = String.Format("\"{0}\"", data.Description);

                if(data.Type == IConfig.SnmpOidType.Alarm)
                {
                    string trap = String.Format(Consts.TrapFormat, data.Name,
                        Consts.EnterpriseName, description, oid);
                    sb.AppendLine(trap);
                }
                else if(data.Type == IConfig.SnmpOidType.Statistic)
                {
                    string get = String.Format(Consts.GetFormat, data.Name,
                        data.DataType, description, Consts.EnterpriseName, oid);
                    sb.AppendLine(get);
                }
            }

            sb.Append(Consts.MibSuffix);

            // Write it to disk
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = Path.Combine(path, Consts.MibFilename);
            FileStream fStream = File.Open(path, FileMode.Create, FileAccess.Write);
            byte[] buffer = Encoding.Default.GetBytes(sb.ToString());
            fStream.Write(buffer, 0, buffer.Length);
            fStream.Flush();
            fStream.Close();
        }

        public abstract class Consts
        {
            // Keep this name until MIB is cleared by Cisco and integrated with their base OID
            public const string MibFilename = "metreos-mce.mib";
            public const string EnterpriseName  = "mce";

            public const string TrapFormat = @"
    {0} TRAP-TYPE
		ENTERPRISE {1}
		DESCRIPTION
			{2}
	::= {3}";
            public const string GetFormat = @"
    {0}	OBJECT-TYPE
	    SYNTAX	   {1}
	    MAX-ACCESS read-only
	    STATUS     current
	    DESCRIPTION
	        {2}
	::= {{ {3} {4} }}";

            public const string MibSuffix = "END";
            public const string MibPrefix = @"METREOS-MCE-MIB DEFINITIONS ::= BEGIN

	--  metreos-mce.mib
	--  Revision: 2.4
    --  Date: {0}

	--  Metreos Corporation 
	--  170 West Tasman Dr.
	--  San Jose, CA. 95134 
	--  www.metreos.com

	--  This module provides definitions for Cisco UAE specific traps.
    --  OID Root: {1}

	--  This file is generated programmatically. Do not modify.

	IMPORTS
		--  These definitions use the enterprises macro as
		--  defined in RFC 1155-SMI
		enterprises
			FROM RFC1155-SMI
		--  These definitions use the OBJECT-TYPE macro as
		--  defined in RFC 1212
		OBJECT-TYPE
			FROM RFC-1212
		--  These definitions use the TRAP-TYPE macro as
		--  defined in RFC 1215
		TRAP-TYPE
			FROM RFC-1215
        DisplayString
                FROM RFC-1213;
			
	metreos		OBJECT IDENTIFIER ::= {{ enterprises 22720 }}
	mce			OBJECT IDENTIFIER ::= {{ metreos 1 }}
	
	trapText  OBJECT-TYPE
	SYNTAX DisplayString
	ACCESS read-only
	STATUS mandatory
	DESCRIPTION 
        ""Alarm Message Text String""
	::= {{ mce 1 }}

	trapID  OBJECT-TYPE
	SYNTAX Integer32
	ACCESS read-only
	STATUS mandatory
	DESCRIPTION 
        ""Specific Trap Number from Alarm Message""
	::= {{ mce 2 }}";
        }
    }
}
