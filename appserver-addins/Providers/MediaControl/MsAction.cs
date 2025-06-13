using System;

using Metreos.Interfaces;
using Metreos.ProviderFramework;

namespace Metreos.MediaControl
{
	/// <summary>AppServer action wrapper for MCP-specific mischief</summary>
	public class MsAction
	{
        private abstract class Consts
        {
            public const uint HairpinPromote = 1;     // Promote but don't demote
        }

        private readonly ActionBase action;
        public ActionBase OriginalAction { get { return action; } }

        public bool IsAsync { get { return action is AsyncAction; } }

        private readonly long callId;
        public long CallId { get { return callId; } }

        private readonly string txIP = null;
        public string TxIP { get { return txIP; } }

        private readonly string txCtrlIP = null;
        public string TxCtrlIP { get { return txCtrlIP; } }

        private readonly uint connId = 0;
        public uint ConnId { get { return connId; } }

        private readonly uint confId = 0;
        public uint ConfId { get { return confId; } }

        private readonly uint mmsId = 0;
        public uint MmsId { get { return mmsId; } }

        private readonly string state = null;
        public string State { get { return state; } }

        private readonly int cmdTimeout = -1;
        public int CommandTimeout { get { return cmdTimeout; } }

        private readonly string[] prompts = new string[3];
        public string[] Prompts { get { return prompts; } }

        private readonly string[] grammars = new string[3];
        public string[] Grammars { get { return grammars; } }

        private readonly bool voiceBargeIn = false;
        public bool VoiceBargeIn { get { return voiceBargeIn; } }

        private readonly bool cancelOnDigit = false;
        public bool CancelOnDigit { get { return cancelOnDigit; } }

        private readonly string filename = null;
        public string Filename { get { return filename; } }

        private readonly uint expires = 0;
        public uint Expires { get { return expires; } }

        private readonly uint silenceTime = 0;
        public uint SilenceTime { get { return silenceTime; } }

        private readonly uint nonSilenceTime = 0;
        public uint NonSilenceTime { get { return nonSilenceTime; } }

        private readonly string digits = null;
        public string Digits { get { return digits; } }

        private readonly uint numParticipants = 0;
        public uint NumParticipants { get { return numParticipants; } }

        private readonly uint txPort = 0;
        public uint TxPort { get { return txPort; } }

        private readonly uint txCtrlPort = 0;
        public uint TxCtrlPort { get { return txCtrlPort; } }
        
        private readonly uint rxFrame = 0;
        public uint RxFramesize { get { return rxFrame; } }
        
        private readonly uint txFrame = 0;
        public uint TxFramesize { get { return txFrame; } }
        
        private readonly bool failResources = true;
        public bool FailResources { get { return failResources; } }

        private readonly bool hairpin = false;
        public bool Hairpin { get { return hairpin; } }

        public uint HairpinPromote { get { return Consts.HairpinPromote; } }

        private readonly bool toneJoin = true;
        public bool ToneJoin { get { return toneJoin; } }

        private readonly bool monitor = false;
        public bool Monitor { get { return monitor; } }

        private bool mute = false;
        public bool Mute { get { return mute; } }

        private bool muteSpecified = false;
        public bool MuteSpecified { get { return muteSpecified; } }

        private readonly bool tariff = false;
        public bool Tariff { get { return tariff; } }

        private bool tariffSpecified = false;
        public bool TariffSpecified { get { return tariffSpecified; } }

        private bool coach = false;
        public bool Coach 
        { 
            get { return coach; } 
            set { coach = value; }
        }

        private bool coachSpecified = false;
        public bool CoachSpecified { get { return coachSpecified; } }

        private bool pupil = false;
        public bool Pupil 
        { 
            get { return pupil; } 
            set { pupil = value; }
        }

        private bool pupilSpecified = false;
        public bool PupilSpecified { get { return pupilSpecified; } }

        private readonly bool receiveOnly = false;
        public bool ReceiveOnly { get { return receiveOnly; } }

        private readonly bool block = false;
        public bool Block { get { return block; } }

        private readonly string operationId;
        public string OperationId { get { return operationId; } }

        private readonly int volume = Int32.MinValue;
        public int Volume { get { return volume; } }

        private readonly int speed = Int32.MinValue;
        public int Speed { get { return speed; } }

        public bool VolumeSpecified { get { return volume != Int32.MinValue; } }
        public bool SpeedSpecified { get { return speed != Int32.MinValue; } }

        private readonly IMediaControl.AdjustmentType adjType;
        public IMediaControl.AdjustmentType AdjustmentType { get { return adjType; } }

        private readonly uint toggleType;
        public uint ToggleType { get { return toggleType; } }

        private readonly IMediaControl.Codecs txCodec = IMediaControl.Codecs.Unspecified;
        public IMediaControl.Codecs TxCodec { get { return txCodec; } }

        private readonly IMediaControl.Codecs rxCodec = IMediaControl.Codecs.Unspecified;
        public IMediaControl.Codecs RxCodec { get { return rxCodec; } }

        // The media server message
        private MediaServerMessage msMsg = null;
        public MediaServerMessage MediaServerMessage
        {
            get { return msMsg; }
            set { msMsg = value; }
        }

        // Async action user data
        public string UserData { get { return Convert.ToString(action.InnerMessage.UserData); } }

        /// <summary>Parses out the media fields from an AppServer action message</summary>
        /// <remarks>Throws exceptions if unhappy</remarks>
		public MsAction(ActionBase action)
		{
            this.action = action;

            action.InnerMessage.GetInt64(ICallControl.Fields.CALL_ID, false, out callId);
            action.InnerMessage.GetUInt32(IMediaControl.Fields.CONNECTION_ID, false, out connId);
			action.InnerMessage.GetUInt32(IMediaControl.Fields.CONFERENCE_ID, false, out confId);
            action.InnerMessage.GetString(IMediaControl.Fields.TX_IP, false, out txIP);
            action.InnerMessage.GetUInt32(IMediaControl.Fields.TX_PORT, false, out txPort);
            action.InnerMessage.GetString(IMediaControl.Fields.TX_CONTROL_IP, false, out txCtrlIP);
            action.InnerMessage.GetUInt32(IMediaControl.Fields.TX_CONTROL_PORT, false, out txCtrlPort);
            action.InnerMessage.GetUInt32(IMediaControl.Fields.TX_FRAMESIZE, false, out txFrame);
            action.InnerMessage.GetUInt32(IMediaControl.Fields.MMS_ID, false, out mmsId);
            action.InnerMessage.GetUInt32(IMediaControl.Fields.RX_FRAMESIZE, false, out rxFrame);
            action.InnerMessage.GetString(IMediaControl.Fields.STATE, false, out state);
            action.InnerMessage.GetInt32(IMediaControl.Fields.COMMAND_TIMEOUT, false, out cmdTimeout);
            action.InnerMessage.GetString(IMediaControl.Fields.PROMPT_ONE, false, out prompts[0]);
            action.InnerMessage.GetString(IMediaControl.Fields.PROMPT_TWO, false, out prompts[1]);
            action.InnerMessage.GetString(IMediaControl.Fields.PROMPT_THREE, false, out prompts[2]);
            action.InnerMessage.GetString(IMediaControl.Fields.GRAMMAR_ONE, false, out grammars[0]);
            action.InnerMessage.GetString(IMediaControl.Fields.GRAMMAR_TWO, false, out grammars[1]);
            action.InnerMessage.GetString(IMediaControl.Fields.GRAMMAR_THREE, false, out grammars[2]);
            action.InnerMessage.GetBoolean(IMediaControl.Fields.VOICE_BARGEIN, false, out voiceBargeIn);
            action.InnerMessage.GetBoolean(IMediaControl.Fields.CANCEL_ON_DIGIT, false, out cancelOnDigit);
            action.InnerMessage.GetString(IMediaControl.Fields.FILENAME, false, out filename);
            action.InnerMessage.GetUInt32(IMediaControl.Fields.EXPIRES, false, out expires);
            action.InnerMessage.GetUInt32(IMediaControl.Fields.SILENCE_TIME, false, out silenceTime);
            action.InnerMessage.GetUInt32(IMediaControl.Fields.NONSILENCE_TIME, false, out nonSilenceTime);
            action.InnerMessage.GetString(IMediaControl.Fields.DIGITS, false, out digits);
            action.InnerMessage.GetString(IMediaControl.Fields.OPERATION_ID, false, out operationId);
            action.InnerMessage.GetBoolean(IMediaControl.Fields.HAIRPIN, false, out hairpin);
            action.InnerMessage.GetUInt32(IMediaControl.Fields.TOGGLE_TYPE, false, out toggleType);

            // Optional values with defaults (depends on usage)
            muteSpecified = action.InnerMessage.GetBoolean(IMediaControl.Fields.MUTE,
                false, IMediaControl.Fields.DefaultValues.MUTE, out pupil);
            tariffSpecified = action.InnerMessage.GetBoolean(IMediaControl.Fields.TARIFF_TONE, 
                false, IMediaControl.Fields.DefaultValues.TARIFF_TONE, out tariff);
            coachSpecified = action.InnerMessage.GetBoolean(IMediaControl.Fields.COACH, 
                false, IMediaControl.Fields.DefaultValues.COACH, out coach);
            pupilSpecified = action.InnerMessage.GetBoolean(IMediaControl.Fields.PUPIL, 
                false, IMediaControl.Fields.DefaultValues.PUPIL, out pupil);

            // Optional int values
            if(action.InnerMessage.Contains(IMediaControl.Fields.VOLUME))
                action.InnerMessage.GetInt32(IMediaControl.Fields.VOLUME, false, out volume);
            if(action.InnerMessage.Contains(IMediaControl.Fields.SPEED))
                action.InnerMessage.GetInt32(IMediaControl.Fields.SPEED, false, out speed);

            // Params with defaults
            action.InnerMessage.GetUInt32(IMediaControl.Fields.NUM_PARTICIPANTS, false, 
                IMediaControl.Fields.DefaultValues.NUM_PARTICIPANTS, out numParticipants);
            action.InnerMessage.GetBoolean(IMediaControl.Fields.FAIL_RESOURCES, false, 
                IMediaControl.Fields.DefaultValues.FAIL_RESOURCES, out failResources);
            action.InnerMessage.GetBoolean(IMediaControl.Fields.TONE_JOIN, false, 
                IMediaControl.Fields.DefaultValues.TONE_JOIN, out toneJoin);
            action.InnerMessage.GetBoolean(IMediaControl.Fields.MONITOR, false, 
                IMediaControl.Fields.DefaultValues.MONITOR, out monitor);
            action.InnerMessage.GetBoolean(IMediaControl.Fields.RECEIVE_ONLY, false, 
                IMediaControl.Fields.DefaultValues.RECEIVE_ONLY, out receiveOnly);
            action.InnerMessage.GetBoolean(IMediaControl.Fields.BLOCK, false, 
                IMediaControl.Fields.DefaultValues.BLOCK, out block);

            // Enumerable param values
            txCodec = GetCodecParam(action.InnerMessage[IMediaControl.Fields.TX_CODEC]);
            rxCodec = GetCodecParam(action.InnerMessage[IMediaControl.Fields.RX_CODEC]);
            adjType = GetAdjustmentType(action.InnerMessage[IMediaControl.Fields.ADJUSTMENT_TYPE]);

            // Determine MMS ID
            if(mmsId == 0)
            {
                if(connId != 0)
                    mmsId = IMediaControl.GetMmsId(connId);
                else if(confId != 0)
                    mmsId = IMediaControl.GetMmsId(confId);
            }
		}

        public override string ToString()
        {
            return action != null ? action.InnerMessage.ToString() : base.ToString();
        }

        #region Static helpers

        /// <summary>Converts specified object to an IMediaControl.Codecs enum item</summary>
        internal static IMediaControl.Codecs GetCodecParam(object obj)
        {
            IMediaControl.Codecs codec = IMediaControl.Codecs.Unspecified;

            if(obj != null)
            {
                if(obj is IMediaControl.Codecs)
                {
                    codec = (IMediaControl.Codecs) obj;
                }
                else
                {
                    try 
                    { 
                        codec = (IMediaControl.Codecs) 
                            Enum.Parse(typeof(IMediaControl.Codecs), obj as string, true); 
                    }
                    catch {}
                }
            }

            return codec;
        }

        internal static IMediaControl.AdjustmentType GetAdjustmentType(object obj)
        {
            string adjVal = obj as string;
            if(adjVal == null)
                return IMediaControl.Fields.DefaultValues.ADJUSTMENT_TYPE;

            IMediaControl.AdjustmentType aType;
            try 
            { 
                aType = (IMediaControl.AdjustmentType)Enum.Parse(typeof(IMediaControl.AdjustmentType), adjVal, true); 
            }
            catch 
            {
                return IMediaControl.Fields.DefaultValues.ADJUSTMENT_TYPE;
            }

            return aType;
        }

        #endregion
	}
}
