using System;
using System.IO;
using System.Collections;

namespace Metreos.RecordAgent
{
    public delegate void OnCallInfoUpdatedDelegate(uint callIdentifier, uint callType, 
                                                    string callerDN, string callerName,
                                                    string calleeDN, string calleeName);

    public delegate void OnCallStateUpdatedDelegate(uint callIdentifier, uint oldCallState, uint newCallState); 

	/// <summary>
	/// Summary description for ActiveCall.
	/// </summary>
	/// 
	public class ActiveCall
	{
        private CallData callData = null;
        public CallData CallData { get { return callData; } set { callData = value.Clone(); } }
        private bool isRecording = false;
        public bool IsRecording { get { return isRecording; } }
        private ArrayList audioFiles = new ArrayList();
        private Config config = Config.Instance;
        private RecordManager recordManager = RecordManager.Instance;
        private DateTime callStartTime; 

        public event OnCallInfoUpdatedDelegate onCallInfoUpdated;
        public event OnCallStateUpdatedDelegate onCallStateUpdated;

        public ActiveCall() 
        {
            callData = new CallData();
            callStartTime = DateTime.Now;
        }

		public ActiveCall(CallData callData)
		{
            this.callData = callData.Clone();
            callStartTime = DateTime.Now;
        }

        public void StartRecording()
        {
            isRecording = true;
        }

        public void StopRecording()
        {
            isRecording = false;
        }

        public void UpdateCallStatus(CallData cd, bool sendEvent)
        {            
            bool bStateChanged = false;
            bool bInfoChanged = false;
            uint oldCallState = this.callData.CallState;
            if (this.callData.CallState != cd.CallState && cd.CallState != 0)
            {
                this.callData.CallState = cd.CallState;
                bStateChanged = true;
            }

            // We do not want to update call info if Caller or Callee DN is empty
            if (cd.CalleeDN == null || cd.CallerDN == null || cd.CalleeDN.Length == 0 || cd.CallerDN.Length == 0)
                bInfoChanged = false;
            else
            {
                if (this.callData.CallType != cd.CallType)
                {
                    if (cd.CallType == CallType.INBOUND_CALL || cd.CallType == CallType.OUTBOUND_CALL)
                    {
                        this.callData.CallType = cd.CallType;
                        bInfoChanged = true;
                    }
                }

                if (cd.CalleeDN != null && this.callData.CalleeDN != cd.CalleeDN)
                {
                    this.callData.CalleeDN = cd.CalleeDN;
                    bInfoChanged = true;
                }

                if (cd.CallerDN != null && this.callData.CallerDN != cd.CallerDN)
                {
                    this.callData.CallerDN = cd.CallerDN;
                    bInfoChanged = true;
                }

                if (cd.CalleeName != null && this.callData.CalleeName != cd.CalleeName)
                {
                    this.callData.CalleeName = cd.CalleeName;
                    bInfoChanged = true;
                }

                if (cd.CallerName != null && this.callData.CallerName != cd.CallerName)
                {
                    this.callData.CallerName = cd.CallerName;
                    bInfoChanged = true;
                }
            }

            if (!sendEvent)
                return;

            if (bInfoChanged && onCallInfoUpdated != null)
                onCallInfoUpdated(this.callData.CallIdentifier, this.callData.CallType, 
                                    this.callData.CallerDN, this.callData.CallerName, 
                                    this.callData.CalleeDN, this.callData.CalleeName);

            if (bStateChanged && onCallStateUpdated != null)
                onCallStateUpdated(this.callData.CallIdentifier, oldCallState, this.callData.CallState);
        }

        public void AddAudioFile(string filePath)
        {    
            if (!this.isRecording)
            {
                File.Delete(filePath);
                return;
            }

            string commonPart = DateTime.Now.ToString("MM-dd-yy") + "_" + this.callData.CallIdentifier.ToString() + "_" + audioFiles.Count.ToString();
            string audioFilePath = Path.Combine(config.UserDataPath, commonPart + ".au");

            if (File.Exists(filePath))
            {
                File.Move(filePath, audioFilePath);
                audioFiles.Add(audioFilePath);
            }
        }

        public void CreateRecord()
        {
            string commonPart = DateTime.Now.ToString("MM-dd-yy") + "_" + this.callData.CallIdentifier.ToString();
            string recordFilePath = Path.Combine(config.UserRecordPath, commonPart + ".xml");
            string noteFilePath = Path.Combine(config.UserDataPath, commonPart + ".rtf");
          
            if (audioFiles.Count == 0)
            {
                if (File.Exists(recordFilePath))
                    File.Delete(recordFilePath);

                if (File.Exists(noteFilePath))
                    File.Delete(noteFilePath);

                return;
            }

            Record r = null;
            if (File.Exists(recordFilePath))
            {
                r = recordManager.ReadRecord(recordFilePath);
                if (r == null)
                    r = new Record();
            }
            else
            {
                r = new Record();
            }

            r.key = commonPart;
           
            r.callDateTime = this.callStartTime.ToString("MM/dd/yy HH:mm:ss");
            if (this.callData.CallType == CallType.INBOUND_CALL)
            {
                r.name = this.callData.CallerName;
                r.number = this.callData.CallerDN;
            }
            else if (this.callData.CallType == CallType.OUTBOUND_CALL)
            {
                r.name = this.callData.CalleeName;
                r.number = this.callData.CalleeDN;
            }

            if (audioFiles.Count > 0)
            {
                r.AtomicAudioFiles = new AtomicAudioFile[audioFiles.Count];
                for (int i=0; i<audioFiles.Count; i++)
                {
                    r.AtomicAudioFiles[i] = new AtomicAudioFile();
                    r.AtomicAudioFiles[i].duration = GetFileDuration(this.audioFiles[i].ToString());
                    r.AtomicAudioFiles[i].fileName = this.audioFiles[i].ToString();
                }
            }

            recordManager.WriteRecord(r, recordFilePath);
        }

        public int GetFileDuration(string audioFile)
        {
            // Assume 8K/Sec
            FileInfo fi = new FileInfo(audioFile);

            if (!fi.Exists)
                return 0;

            return (int)(fi.Length/8000);
        }
	}
}
