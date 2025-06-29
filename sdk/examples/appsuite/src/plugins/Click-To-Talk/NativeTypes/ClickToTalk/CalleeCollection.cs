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
	public class CalleeCollection : IVariable
	{
        public object Value;

        public int Count { get { return Value == null ? 0 : ((DataRowCollection)Value).Count; } }

        public CalleeCollection()
        {}

        public string GetAddress(int index)
        {
            if(Value == null) { return ""; }

            DataRowCollection rows = Value as DataRowCollection;
            
            if((index < 0) || (index > rows.Count)) { return ""; }

            return rows[index][IDatabase.ADDRESS] as string;
        }

        public string GetName(int index)
        {
            if(Value == null) { return ""; }

            DataRowCollection rows = Value as DataRowCollection;
            
            if((index < 0) || (index > rows.Count)) { return ""; }

            return rows[index][IDatabase.NAME] as string;
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

            Value = ds.Tables[0].Rows;
            return true;
        }

        [TypeInput("DataRowCollection", "The result of a Metreos.Native.ClickToTalk.GetCallees action")]
        public bool Parse(object obj)
        {
            if(obj == null) { return false; }

            if(obj is DataRowCollection)
            {
                Value = obj as DataRowCollection;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            Value = null;
        }

        public override string ToString()
        {
            if(Value == null) { return ""; }

            DataRowCollection drc = (DataRowCollection) Value;

            DataRow[] rows = new DataRow[drc.Count];
            for(int i=0; i<drc.Count; i++)
            {
                rows[i] = drc[i];
            }

            DataSet ds = new DataSet();
            ds.Merge(rows);

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            bf.Serialize(stream, ds);
            byte[] buf = stream.GetBuffer();

            return System.Convert.ToBase64String(buf);
        }
    }
}
