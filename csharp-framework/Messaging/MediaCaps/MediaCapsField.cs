using System;
using System.Collections;

using Metreos.Interfaces;
using System.Runtime.Serialization;

namespace Metreos.Messaging.MediaCaps
{
	/// <summary>
	/// Collection of capabilities which must be used as the value of the localMediaCaps or remoteMediaCaps fields
	/// </summary>
	[Serializable]
	public class MediaCapsField : IEnumerable, ISerializable
	{
        // key: codec name, value: ArrayList of uint framesizes
        private readonly Hashtable codecTable;

        public int Count
        {
            get { return codecTable.Count; }
        }

        public uint[] this[IMediaControl.Codecs codec]
        {
            get { return GetFramesizes(codec); }
            set { Add(codec, value); }
        }

        public MediaCapsField()
        {
            codecTable = new Hashtable();
        }

        #region ISerializable

        protected MediaCapsField(SerializationInfo info, StreamingContext context)
        {
            codecTable = new Hashtable();

            foreach(SerializationEntry member in info)
            {
                object[] newEntry = member.Value as object[];
                IMediaControl.Codecs codec = (IMediaControl.Codecs) newEntry[0];
                ArrayList framesizes = new ArrayList(newEntry[1] as uint[]);

                codecTable[codec] = framesizes;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            int count = 0;
            foreach(DictionaryEntry entry in codecTable)
            {
                IMediaControl.Codecs codec = (IMediaControl.Codecs) entry.Key;
                ArrayList framesizes = entry.Value as ArrayList;
                uint[] staticFramesizes = new uint[framesizes.Count];
                framesizes.CopyTo(staticFramesizes);

                object[] newEntry = new object[] { (ushort) codec, staticFramesizes };
                info.AddValue(count++.ToString(), newEntry);
            }
        }
        #endregion



        public MediaCapsField(IMediaControl.Codecs codec, uint framesize)
            : this()
        {
            Add(codec, framesize);
        }

        public void Add(IMediaControl.Codecs codec, params uint[] framesizes)
        {
            if(codec == IMediaControl.Codecs.Unspecified || framesizes == null ||
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

        public uint[] GetFramesizes(IMediaControl.Codecs codec)
        {
            ArrayList fsArray = codecTable[codec] as ArrayList;
            if(fsArray == null) { return null; }

            return (uint[]) fsArray.ToArray(typeof(uint));
        }

        public bool Contains(IMediaControl.Codecs codec)
        {
            return codecTable.Contains(codec);
        }

        public bool MatchCaps(IMediaControl.Codecs codec, uint[] framesizes, out uint framesize)
        {
            ArrayList framesArray = new ArrayList(framesizes);
            return MatchCaps(codec, framesArray, out framesize);
        }

        public bool MatchCaps(IMediaControl.Codecs codec, ArrayList framesizes, out uint framesize)
        {
            framesize = 0;
            if(framesizes == null)
                return false;

            lock(codecTable.SyncRoot)
            {
                foreach(DictionaryEntry de in codecTable)
                {
                    IMediaControl.Codecs _codec = (IMediaControl.Codecs) de.Key;
                    ArrayList _framesizes = de.Value as ArrayList;

                    if(_codec == codec)
                    {
                        foreach(uint _framesize in framesizes)
                        {
                            if(_framesizes.Contains(_framesize))
                            {
                                framesize = _framesize;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public void Clear()
        {
            codecTable.Clear();
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if(codecTable.Count > 0)
            {
                int i = 0;
                foreach(IMediaControl.Codecs codec in codecTable.Keys)
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

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return codecTable.GetEnumerator();
        }

        #endregion
    }
}
