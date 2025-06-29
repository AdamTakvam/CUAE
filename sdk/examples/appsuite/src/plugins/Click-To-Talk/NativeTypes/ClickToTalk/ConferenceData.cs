using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Native.ClickToTalk;

namespace Metreos.Types.ClickToTalk
{
	/// <summary>Holds the info about the received InitiateCall request</summary>
	[Serializable]
	public class ConferenceData : IVariable
	{
        private object Value;

        public string ID { get { return  Value == null ? "" : ((DataRow)Value)[IDatabase.ID] as string; } }
        public string HostIP { get { return Value == null ? "" : ((DataRow)Value)[IDatabase.HOST] as string; } }
        public string HostDescription { get { return Value == null ? "" : ((DataRow)Value)[IDatabase.HOST_DESC] as string; } }
        public string HostUsername { get { return Value == null ? "" : ((DataRow)Value)[IDatabase.HOST_USER] as string; } }
        public string HostPassword { get { return Value == null ? "" : ((DataRow)Value)[IDatabase.HOST_PASS] as string; } }
        public string EmailAddress { get { return Value == null ? "" : ((DataRow)Value)[IDatabase.EMAIL] as string; } }
        public bool Record { get { return Value == null ? false : bool.Parse(((DataRow)Value)[IDatabase.RECORD] as string); } }
        public bool RecordEnded { get { return Value == null ? false : bool.Parse(((DataRow)Value)[IDatabase.RECORD_END] as string); } }
        public string RecordConnId { get { return Value == null ? "" : ((DataRow)Value)[IDatabase.RECORD_ID].ToString(); } }
        public bool Recording { get { return !RecordEnded && Record; } }

        public ConferenceData() 
        {
            Reset();
        }
                
        public void Reset()
        {
            Value = null;
        }

        public bool Parse(string base64BinHash)
        {
            if((base64BinHash == null) || (base64BinHash == "")) { return true; }

            DataSet ds = null;
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                byte[] buf = System.Convert.FromBase64String(base64BinHash);
                ds = (DataSet) bf.Deserialize(new MemoryStream(buf));
            }
            catch(Exception)
            {
                return false;
            }

            if(ds.Tables.Count == 0) { return false; }
            if(ds.Tables[0].Rows.Count == 0) { return false; }
            
            Value = ds.Tables[0].Rows[0];
            return true;
        }

        [TypeInput("DataRow", "The result of a Metreos.Native.ClickToTalk.GetConferenceData action")]
        public bool Parse(object obj)
        {
            if(obj == null) { return false; }

            if(obj is DataRow)
            {
                Value = obj as DataRow;
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            if(Value == null) { return ""; }

            DataRow row = (DataRow) Value;

            DataSet ds = new DataSet();
            ds.Merge( new DataRow[] {row} );

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            bf.Serialize(stream, ds);
            byte[] buf = stream.GetBuffer();

            return System.Convert.ToBase64String(buf);
        }
	}
}
