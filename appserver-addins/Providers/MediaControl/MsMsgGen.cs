using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.ProviderFramework;

namespace Metreos.MediaControl
{
	/// <summary>Creates MMS messages</summary>
	public abstract class MsMsgGen
	{
        #region Message Generators

        internal static MediaServerMessage CreateConnectMessage(string remoteIp, uint remotePort,
            uint connectionId, string routingGuid, StringCollection connectionAttributes, bool modify)
        {
            // Build the connect message to the media server and send it out.
            MediaServerMessage msConnect = new MediaServerMessage(IMediaServer.Messages.Connect);

            msConnect.AddField(ICommands.Fields.ROUTING_GUID, routingGuid);
            msConnect.AddField(IMediaServer.Fields.ConnectionId, connectionId.ToString());
            msConnect.AddField(IMediaServer.Fields.IPAddress, remoteIp != null ? remoteIp : "0");
            msConnect.AddField(IMediaServer.Fields.Port, remotePort.ToString());

            if(connectionAttributes != null)
                msConnect.AddFields(IMediaServer.Fields.ConnectionAttr, connectionAttributes);

            if(modify)
                msConnect.AddField(IMediaServer.Fields.Modify, "1");

            return msConnect;
        }

        internal static MediaServerMessage CreateConferenceConnectMessage(string remoteIp,
            uint remotePort, uint connectionId, uint conferenceId, bool hairpin, uint hairpinPromote, 
            StringCollection connectionAttributes, StringCollection conferenceAttributes, 
            StringCollection confereeAttributes)
        {
            // Build the connect message to the media server and send it out.
            MediaServerMessage msConfConnect = new MediaServerMessage(IMediaServer.Messages.Connect);

			msConfConnect.AddField(IMediaServer.Fields.ConnectionId, connectionId.ToString());
			msConfConnect.AddField(IMediaServer.Fields.ConferenceId, conferenceId.ToString());

            if(remoteIp != null && remotePort != 0)
            {
                msConfConnect.AddField(IMediaServer.Fields.IPAddress, remoteIp);
                msConfConnect.AddField(IMediaServer.Fields.Port, remotePort.ToString());
            }
            
            if(hairpin)
            {
                msConfConnect.AddField(IMediaServer.Fields.Haipin, "1");
                msConfConnect.AddField(IMediaServer.Fields.HairpinPromote, hairpinPromote.ToString());
            }

            if(connectionAttributes != null)
                msConfConnect.AddFields(IMediaServer.Fields.ConnectionAttr, connectionAttributes);

            if(conferenceAttributes != null)
                msConfConnect.AddFields(IMediaServer.Fields.ConferenceAttr, conferenceAttributes);

            if(confereeAttributes != null)
                msConfConnect.AddFields(IMediaServer.Fields.ConfereeAttr, confereeAttributes);

            return msConfConnect;
        }

        internal static MediaServerMessage CreateDisconnectMessage(uint connectionId, uint conferenceId)
        {
            if(connectionId == 0 && conferenceId == 0)
            {
                Debug.Fail("connectionId and conferenceId are null in CreateDisconnectMessage()");
                return null;
            }

            // Build the disconnect message to the media server and send it out.
            MediaServerMessage msDisconnect = new MediaServerMessage(IMediaServer.Messages.Disconnect);
            
            if(connectionId != 0)
                msDisconnect.AddField(IMediaServer.Fields.ConnectionId, connectionId.ToString());

            if(conferenceId != 0)
                msDisconnect.AddField(IMediaServer.Fields.ConferenceId, conferenceId.ToString());

            return msDisconnect;
        }

        internal static MediaServerMessage CreatePlayMessage(uint connectionId, uint conferenceId,
            string appName, string locale, int cmdTimeout, string[] filenames, int volume, int speed, 
            StringCollection terminationConditions, StringCollection audioFileAttributes)
        {
            if(connectionId == 0 && conferenceId == 0)
            {
                Debug.Fail("connectionId and conferenceId can not both be null in CreatePlayMessage()");
                return null;
            }
            if(filenames == null)
            {
                Debug.Fail("no filenames passed into CreatePlayMessage()");
                return null;
            }

            MediaServerMessage playMsg = new MediaServerMessage(IMediaServer.Messages.Play);

            playMsg.AddField(IMediaServer.Fields.AppName, appName);
            playMsg.AddField(IMediaServer.Fields.Locale, locale);

            for(int i=0; i<filenames.Length; i++)
            {
                if(filenames[i] != null)
                    playMsg.AddField(IMediaServer.Fields.Filename, filenames[i]);
            }

            if(connectionId != 0)
                playMsg.AddField(IMediaServer.Fields.ConnectionId, connectionId.ToString());
            else if(conferenceId != 0)
                playMsg.AddField(IMediaServer.Fields.ConferenceId, conferenceId.ToString());

            if(volume >= -10 && volume <= 10)
                playMsg.AddField(IMediaServer.Fields.Volume, volume.ToString());
            if(speed >= -10 && speed <= 10)
                playMsg.AddField(IMediaServer.Fields.Speed, speed.ToString());

            if(audioFileAttributes != null)
                playMsg.AddFields(IMediaServer.Fields.AudioFileAttr, audioFileAttributes);

			if(terminationConditions != null)
				playMsg.AddFields(IMediaServer.Fields.TerminationCondition, terminationConditions);

            if(cmdTimeout != -1)
                playMsg.AddField(IMediaServer.Fields.CommandTimeout, cmdTimeout.ToString());

            return playMsg;
        }

        internal static MediaServerMessage CreateAdjustPlayMessage(uint connectionId, int volume,
            int speed, IMediaControl.AdjustmentType adjType, uint toggleType)
        {
            if(connectionId == 0)
            {
                Debug.Fail("connectionId and conferenceId can not both be null in CreateAdjustPlayMessage()");
                return null;
            }

            MediaServerMessage adjPlayMsg = new MediaServerMessage(IMediaServer.Messages.AdjustPlay);

            adjPlayMsg.AddField(IMediaServer.Fields.ConnectionId, connectionId.ToString());

            if(volume >= -10 && volume <= 10)
                adjPlayMsg.AddField(IMediaServer.Fields.Volume, volume.ToString());
            if(speed >= -10 && speed <= 10)
                adjPlayMsg.AddField(IMediaServer.Fields.Speed, speed.ToString());

            // Defaults have already been applied if no value was given
            adjPlayMsg.AddField(IMediaServer.Fields.AdjustmentType, ConvertAdjustmentType(adjType));
            adjPlayMsg.AddField(IMediaServer.Fields.ToggleType, toggleType.ToString());

            return adjPlayMsg;
        }

        internal static MediaServerMessage CreatePlayToneMessage(uint connectionId, uint conferenceId,
            StringCollection terminationConditions, StringCollection toneAttributes)
        {
            if(connectionId == 0 && conferenceId == 0)
            {
                Debug.Fail("connectionId and conferenceId can not both be null in CreatePlayToneMessage()");
                return null;
            }

            MediaServerMessage playMsg = new MediaServerMessage(IMediaServer.Messages.PlayTone);
            
            if(connectionId != 0)
                playMsg.AddField(IMediaServer.Fields.ConnectionId, connectionId.ToString());
            else if(conferenceId != 0)
                playMsg.AddField(IMediaServer.Fields.ConferenceId, conferenceId.ToString());

            if(toneAttributes != null)
                playMsg.AddFields(IMediaServer.Fields.AudioToneAttr, toneAttributes);

            if(terminationConditions != null)
                playMsg.AddFields(IMediaServer.Fields.TerminationCondition, terminationConditions);

            return playMsg;
        }

        internal static MediaServerMessage CreateVoiceRecMessage(uint connectionId, int cmdTimeout, 
            string appName, string locale, string[] filenames, string[] grammars, int volume, 
            int speed, StringCollection terminationConditions, StringCollection audioFileAttributes, 
            bool voiceBargeIn, bool cancelOnDigit)
        {
            if(connectionId == 0)
            {
                Debug.Fail("connectionId and conferenceId can not both be null in CreateVoiceRecMessage()");
                return null;
            }
            if(grammars == null)
            {
                Debug.Fail("no filenames passed into CreateVoiceRecMessage()");
                return null;
            }

            MediaServerMessage playMsg = new MediaServerMessage(IMediaServer.Messages.VoiceRecognition);

            playMsg.AddField(IMediaServer.Fields.AppName, appName);
            playMsg.AddField(IMediaServer.Fields.Locale, locale);

            for(int i=0; i<filenames.Length; i++)
            {
                if(filenames[i] != null)
                    playMsg.AddField(IMediaServer.Fields.Filename, filenames[i]);
            }

            for(int i=0; i<grammars.Length; i++)
            {
                if(grammars[i] != null)
                    playMsg.AddField(IMediaServer.Fields.GrammarFile, grammars[i]);
            }

            if(connectionId != 0)
                playMsg.AddField(IMediaServer.Fields.ConnectionId, connectionId.ToString());

            if(volume >= -10 && volume <= 10)
                playMsg.AddField(IMediaServer.Fields.Volume, volume.ToString());
            if(speed >= -10 && speed <= 10)
                playMsg.AddField(IMediaServer.Fields.Speed, speed.ToString());

            if(audioFileAttributes != null)
                playMsg.AddFields(IMediaServer.Fields.AudioFileAttr, audioFileAttributes);

            if(terminationConditions != null)
                playMsg.AddFields(IMediaServer.Fields.TerminationCondition, terminationConditions);

            if(cmdTimeout != -1)
                playMsg.AddField(IMediaServer.Fields.CommandTimeout, cmdTimeout.ToString());

            if(voiceBargeIn)
                playMsg.AddField(IMediaServer.Fields.VoiceBarge, "1");

            if(cancelOnDigit)
                playMsg.AddField(IMediaServer.Fields.CancelOnDigit, "1");

            return playMsg;
        }

        internal static MediaServerMessage CreateRecordMessage(uint connectionId, uint conferenceId,
            string appName, string locale, string filename, uint expires, int cmdTimeout, 
            StringCollection terminationConditions, StringCollection audioFileAttributes)
        {
            if(connectionId == 0 && conferenceId == 0)
            {
                Debug.Fail("connectionId and conferenceId can not both be null in CreateRecordMessage()");
                return null;
            }

            MediaServerMessage recordMsg = new MediaServerMessage(IMediaServer.Messages.Record);

            recordMsg.AddField(IMediaServer.Fields.AppName, appName);
            recordMsg.AddField(IMediaServer.Fields.Locale, locale);

            if(connectionId != 0)
                recordMsg.AddField(IMediaServer.Fields.ConnectionId, connectionId.ToString());
            else if(conferenceId != 0)
                recordMsg.AddField(IMediaServer.Fields.ConferenceId, conferenceId.ToString());

            if(filename != null)
                recordMsg.AddField(IMediaServer.Fields.Filename, filename);

            if(expires != 0)
                recordMsg.AddField(IMediaServer.Fields.Expires, expires.ToString());

            if(audioFileAttributes != null)
                recordMsg.AddFields(IMediaServer.Fields.AudioFileAttr, audioFileAttributes);

            if(terminationConditions != null)
                recordMsg.AddFields(IMediaServer.Fields.TerminationCondition, terminationConditions);

            if(cmdTimeout != -1)
                recordMsg.AddField(IMediaServer.Fields.CommandTimeout, cmdTimeout.ToString());

            return recordMsg;
        }

        internal static MediaServerMessage CreateReceiveDigitsMessage(uint connectionId,
            int cmdTimeout, StringCollection terminationConditions)
        {
            if(connectionId == 0)
            {
                Debug.Fail("connectionId is null in CreateReceiveDigitsMessage()");
                return null;
            }

            MediaServerMessage receiveDigitsMsg = new MediaServerMessage(IMediaServer.Messages.ReceiveDigits);            
            receiveDigitsMsg.AddField(IMediaServer.Fields.ConnectionId, connectionId.ToString());

            if(terminationConditions != null)
                receiveDigitsMsg.AddFields(IMediaServer.Fields.TerminationCondition, terminationConditions);

            if(cmdTimeout != -1)
                receiveDigitsMsg.AddField(IMediaServer.Fields.CommandTimeout, cmdTimeout.ToString());

            return receiveDigitsMsg;
        }

        internal static MediaServerMessage CreateSendDigitsMessage(uint connectionId, string digitList)
        {
            if(connectionId == 0)
            {
                Debug.Fail("connectionId is null in CreateSendDigitsMessage()");
                return null;
            }
            if(digitList == null)
            {
                Debug.Fail("digitList is null in CreateSendDigitsMessage()");
                return null;
            }

            MediaServerMessage sendDigitsMsg = new MediaServerMessage(IMediaServer.Messages.SendDigits);
            sendDigitsMsg.AddField(IMediaServer.Fields.ConnectionId, connectionId.ToString());
            sendDigitsMsg.AddField(IMediaServer.Fields.Digits, digitList);

            return sendDigitsMsg;
        }
        
        internal static MediaServerMessage CreateSetAttrMessage(uint connectionId, uint conferenceId,
            bool mute, bool muteExists, bool tariffTone, bool tariffToneExists, bool coach, bool coachExists,
            bool pupil, bool pupilExists)
        {
            if(connectionId == 0 && conferenceId == 0)
            {
                Debug.Fail("connectionId and conferenceId can not both be null in CreateSetAttrMessage()");
                return null;
            }

            MediaServerMessage setAttrMsg = new MediaServerMessage(IMediaServer.Messages.ConfereeSetAttribute);
            setAttrMsg.AddField(IMediaServer.Fields.ConnectionId, connectionId.ToString());
            setAttrMsg.AddField(IMediaServer.Fields.ConferenceId, conferenceId.ToString());
            
            if(muteExists)
                setAttrMsg.AddField(IMediaServer.Fields.ReceiveOnly, mute ? "1" : "0");
            if(tariffToneExists)
                setAttrMsg.AddField(IMediaServer.Fields.TariffTone, tariffTone ? "1" : "0");
            if(coachExists)
                setAttrMsg.AddField(IMediaServer.Fields.Coach, coach ? "1" : "0");
            if(pupilExists)
                setAttrMsg.AddField(IMediaServer.Fields.Pupil, pupil ? "1" : "0");

            return setAttrMsg;
        }

        //internal static MediaServerMessage CreateUnmuteMessage(uint connectionId, uint conferenceId)
        //{
        //    if(connectionId == 0 && conferenceId == 0)
        //    {
        //        Debug.Fail("connectionId and conferenceId can not both be null in CreateUnmuteMessage()");
        //        return null;
        //    }

        //    MediaServerMessage unMuteMsg = new MediaServerMessage(IMediaServer.MSG_MS_CONFEREE_SET_ATTRIBUTE);
        //    unMuteMsg.AddField(IMediaServer.Fields.ConnectionId, connectionId.ToString());            
        //    unMuteMsg.AddField(IMediaServer.Fields.ConferenceId, conferenceId.ToString());
        //    unMuteMsg.AddField(IMediaServer.Fields.ReceiveOnly, "0");

        //    return unMuteMsg;
        //}

        internal static MediaServerMessage CreateStopMediaOperation(uint connectionId, string operationId, bool block)
        {
            if(connectionId == 0)
            {
                Debug.Fail("connectionId is null in CreateStopMediaOperation()");
                return null;
            }

            MediaServerMessage stopMediaMsg = new MediaServerMessage(IMediaServer.Messages.StopMediaOperation);
            stopMediaMsg.AddField(IMediaServer.Fields.ConnectionId, connectionId.ToString());
            stopMediaMsg.AddField(IMediaServer.Fields.Block, block ? "1" : "0");

            if(operationId != null && operationId != String.Empty)
                stopMediaMsg.AddField(IMediaServer.Fields.OperationId, operationId);

            return stopMediaMsg;
        }

        internal static MediaServerMessage CreateDetectSilence(uint connectionId, uint silenceTime,
            int cmdTimeout)
        {
            if(connectionId == 0)
            {
                Debug.Fail("connectionId is null in CreateDetectSilence()");
                return null;
            }

            // Build a parameter of the form: "silence 12345"
            string silenceCallState = String.Format("{0} {1}", IMediaServer.CallState.Silence, silenceTime.ToString());

            MediaServerMessage detectSilenceMsg = new MediaServerMessage(IMediaServer.Messages.MonitorCallState);
            detectSilenceMsg.AddField(IMediaServer.Fields.ConnectionId, connectionId.ToString());
            detectSilenceMsg.AddField(IMediaServer.Fields.CallState, silenceCallState);

            if(cmdTimeout != -1)
                detectSilenceMsg.AddField(IMediaServer.Fields.CommandTimeout, cmdTimeout.ToString());

            return detectSilenceMsg;
        }

        internal static MediaServerMessage CreateDetectNonSilence(uint connectionId, uint nonSilenceTime,
            int cmdTimeout)
        {
            if(connectionId == 0)
            {
                Debug.Fail("connectionId is null in CreateDetectNonSilence()");
                return null;
            }

            // Build a parameter of the form: "nonsilence 12345"
            string nonSilenceCallState = String.Format("{0} {1}", IMediaServer.CallState.NonSilence, nonSilenceTime.ToString());

            MediaServerMessage detectNonSilenceMsg = new MediaServerMessage(IMediaServer.Messages.MonitorCallState);
            detectNonSilenceMsg.AddField(IMediaServer.Fields.ConnectionId, connectionId.ToString());
            detectNonSilenceMsg.AddField(IMediaServer.Fields.CallState, nonSilenceCallState);

            if(cmdTimeout != -1)
                detectNonSilenceMsg.AddField(IMediaServer.Fields.CommandTimeout, cmdTimeout.ToString());

            return detectNonSilenceMsg;
        }
        #endregion

        #region Helpers

        /// <summary>
        /// Creates a list of connection attributes from a Attribute Name -> Value map.
        /// </summary>
        /// <param name="isConferenceAttr">if 'true', the attributes will be treated as conference attributes</param>
        /// <param name="attributes">Hashtable that maps attribute names to attribute values.</param>
        /// <returns></returns>
        internal static StringCollection CreateConnectionAttributes(string txIP, IMediaControl.Codecs txCodec, 
            uint txFramesize, IMediaControl.Codecs rxCodec, uint rxFramesize)
        {
            StringCollection attrs = new StringCollection();

            bool txCodecValid = false, rxCodecValid = false;
            bool txFrameValid = false, rxFrameValid = false;

            if(txCodec != IMediaControl.Codecs.Unspecified)
            {
                IMediaServer.Codecs txCodecMms = (IMediaServer.Codecs)txCodec;
                attrs.Add(IMediaServer.ConnectionAttrs.RemoteCoder + " " + (ushort) txCodecMms);
                txCodecValid = true;
            }

            if(rxCodec != IMediaControl.Codecs.Unspecified)
            {
                IMediaServer.Codecs rxCodecMms = (IMediaServer.Codecs)rxCodec;
                attrs.Add(IMediaServer.ConnectionAttrs.LocalCoder + " " + (ushort)rxCodecMms);
                rxCodecValid = true;
            }

            if(txFramesize > 0)
            {
                attrs.Add(IMediaServer.ConnectionAttrs.RemoteFramesize + " " + txFramesize);
                txFrameValid = true;
            }

            if(rxFramesize > 0)
            {
                attrs.Add(IMediaServer.ConnectionAttrs.LocalFramesize + " " + rxFramesize);
                rxFrameValid = true;
            }

            // Compute media direction
            string dir = null;
            if(IsMulticast(txIP))
                dir = IMediaServer.Direction.Tx_MC;
            else if ((txCodecValid && txFrameValid) && !(rxCodecValid && rxFrameValid))
                dir = IMediaServer.Direction.Tx;
            else if((rxCodecValid && rxFrameValid) && !(txCodecValid && txFrameValid))
                dir = IMediaServer.Direction.Rx;
            else
                dir = IMediaServer.Direction.Bi;
            
        
            attrs.Add(IMediaServer.ConnectionAttrs.Direction + " " + dir);

            return attrs;
        }

        internal static StringCollection CreateConferenceAttributes(bool toneJoin)
        {
            StringCollection attrs = new StringCollection();

            if (toneJoin)
                attrs.Add(IMediaServer.ConferenceAttrs.SoundTone);
            else
                attrs.Add(IMediaServer.ConferenceAttrs.NoTone);

            return attrs;
        }

        /// <summary>
        /// Creates a list of conferee attributes from an Attribute Name -> Value map.
        /// </summary>
        /// <param name="attributes">Hashtable that maps attributes names to attribute values.</param>
        internal static StringCollection CreateConfereeAttributes(bool monitor, bool tariff, bool coach, 
            bool pupil, bool receiveOnly)
        {
            StringCollection attrs = new StringCollection();

            if(monitor)
                attrs.Add(IMediaServer.Fields.Monitor);
            if(tariff)
                attrs.Add(IMediaServer.Fields.TariffTone);
            if(coach)
                attrs.Add(IMediaServer.Fields.Coach);
            if(pupil)
                attrs.Add(IMediaServer.Fields.Pupil);
            if(receiveOnly)
                attrs.Add(IMediaServer.Fields.ReceiveOnly);

            if(attrs.Count == 0)    
                return null;            

            return attrs;
        }

        /// <summary>
        /// Retrieves the individual termination conditions from a media
        /// server action and formats the properly so the media server
        /// can parse them.
        /// </summary>
        /// <param name="im">The InternalMessage to extract the termination conditions from.</param>
        /// <param name="paramNames">The possible termination conditions to search for.</param>
        /// <param name="mmsFieldNames">Key values of the termination conditions when found.</param>
        /// <returns>A string array of properly formatted termination conditions.</returns>
        internal static StringCollection GetTerminationConditions(
            InternalMessage im,
            string[] paramNames,
            object[] mmsFieldNames)
        {
            StringCollection terminationConditions = new StringCollection();
            object paramValue = null;

            // Search the array of possible termination conditions.
            for(int i = 0; i < paramNames.Length; i++)
            {
                paramValue = im[paramNames[i]];
                if(paramValue != null)
                {
                    // If we found a termination condition in the array of possible conditions
                    // then format it appropriately.  For example, if we found the termination
                    // condition "TermCondDigit" then we format it to: "digit <value>".
                    terminationConditions.Add(String.Format("{0} {1}", mmsFieldNames[i] as string, paramValue));
                }
            }

            return terminationConditions;
        }


        /// <summary>
        /// Retrieves the individual audio file attributes from a media
        /// server action and formats the properly so the media server
        /// can parse them.
        /// </summary>
        /// <param name="im">The InternalMessage to extract the audio file attributes from.</param>
        /// <param name="paramNames">The possible audio file attributes to search for.</param>
        /// <param name="mmsFieldNames">Key values of the audio file attributes when found.</param>
        /// <returns>A string array of properly formatted audio file attributes.</returns>
        internal static StringCollection GetAudioFileAttributes(
            InternalMessage im,
            string[] paramNames,
            string[] mmsFieldNames)
        {
            StringCollection attributes = new StringCollection();
            string paramValue = null;
            bool formatSpecified = false;

            // Search the array of possible audio file attributes.
            for(int i = 0; i < paramNames.Length; i++)
            {
                paramValue = im[paramNames[i]] as string;
                if(paramValue != null)
                {
                    // If we found an audio file attribute in the array of possible attributes
                    // then format it appropriately.  For example, if we found the attribute
                    // "audioFileBitRate" then we format it to: "bitrate <value>".
                    attributes.Add(String.Format("{0} {1}", mmsFieldNames[i], paramValue));

                    if(paramNames[i] == IMediaControl.Fields.AUDIO_FILE_FORMAT)
                        formatSpecified = true;
                }
            }

            if(formatSpecified == false)
            {
                // Determine format type by extension
                string filename = Convert.ToString(im[IMediaControl.Fields.FILENAME]);
                if(filename == null || filename == String.Empty)
                    filename = Convert.ToString(im[IMediaControl.Fields.PROMPT_ONE]);
                if(filename != null)
                {
                    if(filename.EndsWith("." + IMediaControl.AudioFileFormat.wav))
                    {
                        attributes.Add(String.Format("{0} {1}", 
                            IMediaServer.AudioFileAttrs.Format, IMediaControl.AudioFileFormat.wav.ToString()));
                    }
                    else if(filename.EndsWith("." + IMediaControl.AudioFileFormat.vox))
                    {
                        attributes.Add(String.Format("{0} {1}",
                            IMediaServer.AudioFileAttrs.Format, IMediaControl.AudioFileFormat.vox.ToString()));
                    }
                }
            }

            return attributes;
        }

        /// <summary>
        /// Retrieves the individual attributes from a PlayTone action and formats them 
        /// properly so the media server can parse them.
        /// </summary>
        /// <param name="im">The InternalMessage to extract the attributes from.</param>
        /// <param name="paramNames">The possible attributes to search for.</param>
        /// <param name="mmsFieldNames">Key values of the attributes when found.</param>
        /// <returns>A string array of properly formatted attributes.</returns>
        internal static StringCollection GetToneAttributes(
            InternalMessage im,
            string[] paramNames,
            object[] mmsFieldNames)
        {
            StringCollection attributes = new StringCollection();
            string paramValue = null;

            // Search the array of possible attributes.
            for(int i = 0; i < paramNames.Length; i++)
            {
                paramValue = Convert.ToString(im[paramNames[i]]);
                if(paramValue != null)
                {
                    // If we found a termination condition in the array of possible conditions
                    // then format it appropriately.  For example, if we found the termination
                    // condition "termCondDigit" then we format it to: "digit <value".
                    attributes.Add(String.Format("{0} {1}", mmsFieldNames[i] as string, paramValue));
                }
            }

            return attributes;
        }

        internal static string ConvertAdjustmentType(IMediaControl.AdjustmentType adjType)
        {
            switch(adjType)
            {
                case IMediaControl.AdjustmentType.absolute:
                    return IMediaServer.AdjustmentType.Absolute;
                case IMediaControl.AdjustmentType.relative:
                    return IMediaServer.AdjustmentType.Relative;
                case IMediaControl.AdjustmentType.toggle:
                    return IMediaServer.AdjustmentType.Toggle;
                default:
                    return IMediaServer.AdjustmentType.Absolute;
            }
        }

        internal static bool IsMulticast(string ipAddr)
        {
            if(ipAddr == null)
                return false;

            // Class D range: 224.0.0.0 to 239.255.255.2553
            string[] bits = ipAddr.Split('.');
            if(bits == null || bits.Length == 0)
                return false;

            try 
            { 
                byte highByte = byte.Parse(bits[0]);

                if(highByte >= 224 && highByte <= 239)
                    return true;
            }
            catch { }

            return false;
        }
        #endregion
	}
}
