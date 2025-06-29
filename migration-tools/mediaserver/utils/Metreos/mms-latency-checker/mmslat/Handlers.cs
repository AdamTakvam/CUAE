using System;
using System.Xml;


namespace mmslat
{
	public class Handlers
	{		
        public class MmsResultData
        {
            public bool xmlparserResult = false;
            public string messageID = String.Empty;          
            public string ipaddr    = String.Empty;
            public string termcond  = String.Empty;
            public int connectionID = 0;
            public int conferenceID = 0;
            public int transID    = 0;
            public int clientID   = 0;
            public int serverID   = 0;
            public int resultcode = 0;
            public int reasoncode = 0;
            public int port = 0;
        }
       

        public static MmsResultData InterpretMmsResult(string xml)
        {
            MmsResultData r = new MmsResultData(); 
            r.messageID = GetTagValue  (xml, Const.idMessageId);
            r.transID   = atoi(GetFieldValue(xml, Const.idTransID));
            r.clientID  = atoi(GetFieldValue(xml, Const.idClientID));
            r.serverID  = atoi(GetFieldValue(xml, Const.idServerID));
            r.port      = atoi(GetFieldValue(xml, Const.idPort));
            r.resultcode  = atoi(GetFieldValue(xml, Const.idResultCode));
            r.reasoncode  = atoi(GetFieldValue(xml, Const.idResultCode));

            r.connectionID  = atoi(GetFieldValue(xml, Const.idConnectionID));
            r.connectionID &= Const.CONXID_MASK;

            uint serverid = ((uint)r.connectionID & 0xff000000) >> 24;
            if (r.serverID == 0) r.serverID = (int)serverid;

            r.conferenceID  = atoi(GetFieldValue(xml, Const.idConferenceID));
            r.conferenceID &= Const.CONXID_MASK;

            r.ipaddr   = GetFieldValue(xml, Const.idIpAddress);
            r.termcond = GetFieldValue(xml, Const.idTermCond);
            r.xmlparserResult = true;  
            return r;
        }


        public static string GetTagValue(string xml, string id)
        {           
            string tag = id + Const.rangle;
            int offset = xml.IndexOf(tag);
            if (offset == -1) return String.Empty;
            string sub = xml.Substring(offset);
            offset = sub.IndexOf(Const.rangle);
            sub = sub.Substring(offset+1);
            string[] s = sub.Split(Const.cSplitBy(Const.langlec));
            return s[0];
        }


        public static string GetFieldValue(string xml, string id)
        {
            string tag = id + Const.dquote + Const.rangle;
            int offset = xml.IndexOf(tag);
            if (offset == -1) return String.Empty;
            string sub = null;
            try  {  sub = xml.Substring(offset); } 
            catch{ }
            if (sub == null) return String.Empty;

            offset = sub.IndexOf(Const.rangle);
            sub = sub.Substring(offset+1);
            string[] s = sub.Split(Const.cSplitBy(Const.langlec));
            return s[0];
        }


        /// <summary>Convert alpha to int with behavior as c library atoi</summary>
        public static int atoi(string s)
        {
            int    n = 0;
            try  { n = System.Convert.ToInt32(s); } catch { }
            return n;
        }


        /// <summary>Convert alpha to long with behavior as c library atol</summary>
        public static long atol(string s)
        {
            long   n = 0;
            try  { n = System.Convert.ToInt64(s); } catch { }
            return n;
        }
	}
}
