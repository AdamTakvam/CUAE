using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Xml;

using MySql.Data;

using Metreos.Common.Mec;
using Metreos.Core;
using Metreos.Utilities;
using WebMessage = Metreos.Common.Mec;

namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// Summary description for DatabaseComparisions.
	/// </summary>
	public class DatabaseComparisions
	{
      	public DatabaseComparisions()
		{
                
		}

        public bool Connect()
        {
            return true;
        }

        public bool Disconnect()
        {
            return true;
        }
        public static bool CheckLocationId(string locationId)
        {
            
            return true;
        }

        public static int CheckNumConnections()
        {
            return 0;
        }
        
        public static bool CheckConferenceId(string conferenceId)
        {
            return true;
        }
	}
}
