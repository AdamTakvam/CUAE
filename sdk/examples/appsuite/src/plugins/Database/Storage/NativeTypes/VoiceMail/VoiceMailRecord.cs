using System;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

namespace Metreos.ApplicationSuite.Types
{
	/// <summary>
	/// Summary description for VoiceMailRecord.
	/// </summary>
	public class VoiceMailRecord : IVariable, IComparable
	{
        public uint Id { get { return id; } set { id = value; } }
        private uint id;
        
        public uint UserId { get { return userId; } set { userId = value; } }
        private uint userId;

        public string Filename { get { return filename; } set { filename = value; } }
        private string filename;

        public DateTime TimeStamp { get { return timeStamp; } set { timeStamp = value; } }
        private DateTime timeStamp;

        public uint Status { get { return status; } set { status = value; } }
        private uint status;

        public uint Length { get { return length; } set { length = value; } }
        private uint length;

        public bool IsDirty { get { return isDirty; } set { isDirty = value; } }
        private bool isDirty = false;
        
        public bool IsNew { get { return isNew; } set { isNew = value; } }
        private bool isNew = false;

        public VoiceMailRecord()
		{
            id = userId = length = status = 0;
            filename = null;
        }

        public VoiceMailRecord(uint userId, string filename, uint status, uint length, DateTime timeStamp) : this()
        {
            this.userId = userId;
            this.filename = filename;
            this.status = status;
            this.length = length;
            this.timeStamp = timeStamp;        
        }

        public VoiceMailRecord(uint id, uint userId, string filename, 
                                uint status, uint length, DateTime timeStamp) : this(userId, filename, status, length, timeStamp)
        {
            this.id = id;
        }

        public VoiceMailRecord(VoiceMailRecord rhs) : this(rhs.id, rhs.userId, rhs.filename, rhs.status, rhs.length, rhs.timeStamp)
        {
            isDirty = rhs.isDirty;
            isNew = rhs.isNew;
        }

        #region IVariable Members
        
        [TypeInput("string", "Takes only another VoiceMailRecord object, so this always returns false")]
        public bool Parse(string str)
        {
            return false;
        }

        [TypeInput("object", "Takes only another VoiceMailRecord object")]
        public bool Parse(object obj)
        {
            if (!(obj is VoiceMailRecord))
                return false;
            
            VoiceMailRecord record = obj as VoiceMailRecord;
            this.id = record.id;
            this.userId = record.userId;
            this.filename = record.filename;
            this.status = record.status;
            this.length = record.length;
            this.timeStamp = record.timeStamp;
            this.isNew = record.isNew;
            this.isDirty = record.isDirty;
            return true;
        }
        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (!(obj is VoiceMailRecord))
                throw new Exception("Object is not of type VoiceMailRecord.");

            return timeStamp.CompareTo(obj);
        }

        #endregion
    }
}
