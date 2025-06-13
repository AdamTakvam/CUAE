using System;
using System.Collections;

namespace TaskQueueTest
{
    [Flags]
    public enum Codecs : ushort
    {
        Unspecified=0,
        G711u=0x01,
        G711a=0x02,
        G723=0x04,
        G729=0x08
    }

	/// <summary>
	/// Collection of capabilities which must be used as the value of the localMediaCaps or remoteMediaCaps fields
	/// </summary>
	[Serializable]
	public class MediaCapsField
	{
        // key: codec name, value: ArrayList of uint framesizes
        private readonly Hashtable codecTable;

		public MediaCapsField()
		{
            codecTable = Hashtable.Synchronized(new Hashtable());
		}

        public void Add(Codecs codec, params uint[] framesizes)
        {
            if (codec == Codecs.Unspecified || framesizes == null ||
                framesizes.Length == 0)
                return;

            ArrayList fsArray = codecTable[codec] as ArrayList;

            if(fsArray == null)
            {
                fsArray = new ArrayList(framesizes);
                codecTable[codec] = fsArray;
            }
            else
            {
                foreach(uint frame in framesizes)
                {
                    if(!fsArray.Contains(frame))
                        fsArray.Add(frame);
                }
            }
        }

        public uint[] GetFramesizes(Codecs codec)
        {
            ArrayList fsArray = codecTable[codec] as ArrayList;
            if(fsArray == null) { return null; }

            return (uint[]) fsArray.ToArray(typeof(uint));
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if(codecTable.Count > 0)
            {
                int i = 0;
                foreach(Codecs codec in codecTable.Keys)
                {
                    sb.AppendFormat("{0}: ", codec.ToString());

                    uint[] fsArray = GetFramesizes(codec);
                    if(fsArray.Length > 0)
                        foreach(uint fs in fsArray)
                            sb.AppendFormat("{0} ", fs);

                    i++;
                    if(i < codecTable.Keys.Count)
                        sb.Append("\n");
                }
            }

            return codecTable.Count == 0 ? "Empty" : sb.ToString();
        }
    }
}
