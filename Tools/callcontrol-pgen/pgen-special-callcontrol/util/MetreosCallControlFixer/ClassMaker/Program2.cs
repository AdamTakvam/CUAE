using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;

using Metreos.PackageGeneratorCore.PackageXml;

namespace ClassMaker
{
    class Program
    {
        private static Regex isAction = new Regex(@"<action\s.*name=\\""(?<name>\w*)\\""", RegexOptions.Compiled);
        private static Regex isActionName = new Regex(@"name=\\""(?<name>\w*)\\""", RegexOptions.Compiled);
        private static Regex isActionDisplay = new Regex(@"displayName=\\""(?<name>\w*)\\""", RegexOptions.Compiled);
        private static Regex isActionDescription = new Regex(@"description=\\""(?<name>(\w|\.|\s|\,|\(|\)|\-)*)\\""", RegexOptions.Compiled);
       

        private static Regex getActionParamName = new Regex(@"<actionParam.*name=\\""(?<name>\w*)\\""", RegexOptions.Compiled);
        private static Regex isActionParamName = new Regex(@"name=\\""(?<name>\w*)\\""", RegexOptions.Compiled);
        private static Regex isActionParamDisplay = new Regex(@"displayName=\\""(?<name>\w*)\\""", RegexOptions.Compiled);
        private static Regex isActionParamDescription = new Regex(@"description=\\""(?<name>(\w|\.|\s|\,|\(|\)|\-)*)\\""", RegexOptions.Compiled);
        private static Regex getResultDataName = new Regex(@"<resultData.*>(?<name>\w*)</resultData>", RegexOptions.Compiled);
        private static Regex isResultDataName = new Regex(@">(?<name>\w*)</resultData>", RegexOptions.Compiled);
        private static Regex isResultDataDisplay = new Regex(@"displayName=\\""(?<name>\w*)\\""", RegexOptions.Compiled);
        private static Regex isResultDataDescription = new Regex(@"description=\\""(?<name>(\w|\.|\s|\,|\(|\)|\-)*)\\""", RegexOptions.Compiled);

        private static Regex isEvent = new Regex(@"<event\s.*name=\\""(?<name>\w*)\\""", RegexOptions.Compiled);
        private static Regex isEventName = new Regex(@"name=\\""(?<name>\w*)\\""", RegexOptions.Compiled);
        private static Regex isEventDisplay = new Regex(@"displayName=\\""(?<name>\w*)\\""", RegexOptions.Compiled);
        private static Regex isEventDescription = new Regex(@"description=\\""(?<name>(\w|\.|\s|\,|\(|\)|\-)*)\\""", RegexOptions.Compiled);
       
        private static Regex getEventParamName = new Regex(@"<eventParam.*name=\\""(?<name>\w*)\\""", RegexOptions.Compiled);
        private static Regex isEventParamName = new Regex(@"name=\\""(?<name>\w*)\\""", RegexOptions.Compiled);
        private static Regex isEventParamDisplay = new Regex(@"displayName=\\""(?<name>\w*)\\""", RegexOptions.Compiled);
        private static Regex isEventParamDescription = new Regex(@"description=\\""(?<name>(\w|\.|\s|\,|\(|\)|\-)*)\\""", RegexOptions.Compiled);
       
        
        static void Main(string[] args)
        {
            //DirectoryInfo directory = new DirectoryInfo(@"C:\Documents and Settings\secall\Desktop\test");
            DirectoryInfo directory = new DirectoryInfo(@"C:\workspace\head\appserver-addins\NativeActions");

            Process();
        }

        private static void Process()
        {
            bool success = true;
            StringBuilder builder = new StringBuilder();
            try
            {
                StringReader reader = new StringReader(CallControl);

                string currentReplacerBase = null;  // Action.ACTIONNAME.Params/Results, // Event.EVENTNAME.Params
                string currentLine = null;
                while (reader.Peek() != -1)
                {
                    currentLine = reader.ReadLine();

                    if (isAction.IsMatch(currentLine))
                    {
                        string actionName = isAction.Match(currentLine).Groups["name"].Value;
                        currentReplacerBase = "Action." + actionName;

                        currentLine = isActionName.Replace(currentLine, "name=\\\"\" + XmlEncode(" + currentReplacerBase + ".NAME) + \"\\\"");
                        currentLine = isActionDescription.Replace(currentLine, "description=\\\"\" + XmlEncode(" + currentReplacerBase + ".DESCRIPTION) + \"\\\"");
                        currentLine = isActionDisplay.Replace(currentLine, "displayName=\\\"\" + XmlEncode(" + currentReplacerBase + ".DISPLAY) + \"\\\"");
                    }
                    else if (isEvent.IsMatch(currentLine))
                    {
                        string eventName = isEvent.Match(currentLine).Groups["name"].Value;
                        currentReplacerBase = "Event." + eventName;

                        currentLine = isEventName.Replace(currentLine, "name=\\\"\" + XmlEncode(" + currentReplacerBase + ".NAME) + \"\\\"");
                        currentLine = isEventDescription.Replace(currentLine, "description=\\\"\" + XmlEncode(" + currentReplacerBase + ".DESCRIPTION) + \"\\\"");
                        currentLine = isEventDisplay.Replace(currentLine, "displayName=\\\"\" + XmlEncode(" + currentReplacerBase + ".DISPLAY) + \"\\\"");
                    }
                    else if (getActionParamName.IsMatch(currentLine))
                    {
                        string actionParamName = getActionParamName.Match(currentLine).Groups["name"].Value;

                        currentLine = isActionParamName.Replace(currentLine, "name=\\\"\" + XmlEncode(" + currentReplacerBase + ".Params." + actionParamName + ".NAME) + \"\\\"");
                        currentLine = isActionParamDescription.Replace(currentLine, "description=\\\"\" + XmlEncode(" + currentReplacerBase + ".Params." + actionParamName + ".DESCRIPTION) + \"\\\"");
                        currentLine = isActionParamDisplay.Replace(currentLine, "displayName=\\\"\" + XmlEncode(" + currentReplacerBase + ".Params." + actionParamName + ".DISPLAY) + \"\\\"");
                    }
                    else if (getResultDataName.IsMatch(currentLine))
                    {
                        string resultDataName = getResultDataName.Match(currentLine).Groups["name"].Value;

                        currentLine = isResultDataName.Replace(currentLine, ">\" + XmlEncode(" + currentReplacerBase + ".Results." + resultDataName + ".NAME) + \"</resultData>");
                        currentLine = isResultDataDescription.Replace(currentLine, "description=\\\"\" + XmlEncode(" + currentReplacerBase + ".Results." + resultDataName + ".DESCRIPTION) + \"\\\"");
                        currentLine = isResultDataDisplay.Replace(currentLine, "displayName=\\\"\" + XmlEncode(" + currentReplacerBase + ".Results." + resultDataName + ".DISPLAY) + \"\\\"");
                    }
                    else if (getEventParamName.IsMatch(currentLine))
                    {
                        string eventParamName = getEventParamName.Match(currentLine).Groups["name"].Value;

                        currentLine = isEventParamName.Replace(currentLine, "name=\\\"\" + XmlEncode(" + currentReplacerBase + ".Params." + eventParamName + ".NAME) + \"");
                        currentLine = isEventParamDescription.Replace(currentLine, "description=\\\"\" + XmlEncode(" + currentReplacerBase + ".Params." + eventParamName + ".DESCRIPTION) + \"\\\"");
                        currentLine = isEventParamDisplay.Replace(currentLine, "displayName=\\\"\" + XmlEncode(" + currentReplacerBase + ".Params." + eventParamName + ".DISPLAY) + \"\\\"");
                    }

                    builder.Append("\"" + currentLine + "\\n\" + \n");
                }
            }
            catch (Exception e)
            {
                success = false;
            }

            if (success)
            {
                FileStream writer = File.Open("C:\\out.cs", FileMode.Create);
                string fileContents = builder.ToString();
                int length = System.Text.Encoding.Default.GetBytes(fileContents).Length;
                writer.SetLength(length);
                StreamWriter streamWriter = new StreamWriter(writer);
                MemoryStream mem = new MemoryStream(length);
                mem.Write(System.Text.Encoding.Default.GetBytes(fileContents), 0, length);
                mem.WriteTo(writer);
            }

        }

        private static string XmlEncode(string stuff)
        {
            stuff = stuff.Replace("<", "&lt;");
            stuff = stuff.Replace("&", "&amp;");
            return stuff;
        }
      
        private static string CallControl =
"<?xml version=\"1.0\"?>\n" + 
"<package xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" name=\"Metreos.CallControl\" description=\"Suite of actions and events for first-party call control\" xmlns=\"http://metreos.com/ActionEventPackage.xsd\">\n" + 
"  <actionList>\n" + 
"    <action name=\"" + XmlEncode(Action.MakeCall.NAME) + "\" type=\"provider\" allowCustomParams=\"false\" final=\"false\" displayName=\"Make Call\" description=\"" + XmlEncode(Action.MakeCall.DESCRIPTION) + "\">\n" + 
"      <actionParam name=\"" + XmlEncode(Action.MakeCall.Params.To.NAME) + "\" displayName=\"" + XmlEncode(Action.MakeCall.Params.To.DISPLAY) + "\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.MakeCall.Params.To.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.MakeCall.Params.From.NAME) + "\" displayName=\"" + XmlEncode(Action.MakeCall.Params.From.DISPLAY) + "\" type=\"System.String\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.MakeCall.Params.From.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.MakeCall.Params.DisplayName.NAME) + "\" displayName=\"" + XmlEncode(Action.MakeCall.Params.DisplayName.DISPLAY) + "\" type=\"System.String\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.MakeCall.Params.DisplayName.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.MakeCall.Params.MmsId.NAME) + "\" displayName=\"" + XmlEncode(Action.MakeCall.Params.MmsId.DISPLAY) + "\" type=\"System.UInt32\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.MakeCall.Params.MmsId.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.MakeCall.Params.PeerCallId.NAME) + "\" displayName=\"" + XmlEncode(Action.MakeCall.Params.PeerCallId.DISPLAY) + "\" type=\"System.String\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.MakeCall.Params.PeerCallId.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.MakeCall.Params.ProxyDTMFCallId.NAME) + "\" displayName=\"" + XmlEncode(Action.MakeCall.Params.ProxyDTMFCallId.DISPLAY) + "\" type=\"System.String\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.MakeCall.Params.ProxyDTMFCallId.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.MakeCall.Params.WaitForMedia.NAME) + "\" displayName=\"" + XmlEncode(Action.MakeCall.Params.WaitForMedia.DISPLAY) + "\" type=\"Metreos.AppServer.TelephonyManager.WaitMedia\" use=\"optional\" allowMultiple=\"false\" default=\"TxRx\" description=\"Indicates that the async response should not be sent until media has been established. Valid values: None, Tx, Rx, TxRx. The value of this field will affect the media values returned in the callback.\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.MakeCall.Params.Conference.NAME) + "\" displayName=\"" + XmlEncode(Action.MakeCall.Params.Conference.DISPLAY) + "\" type=\"System.Boolean\" use=\"optional\" allowMultiple=\"false\" default=\"false\" description=\"" + XmlEncode(Action.MakeCall.Params.Conference.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.MakeCall.Params.ConferenceId.NAME) + "\" displayName=\"" + XmlEncode(Action.MakeCall.Params.ConferenceId.DISPLAY) + "\" type=\"System.String\" use=\"optional\" allowMultiple=\"false\" description=\"The ID of the conference to add this call to. Specify 0 if this is the first party in the conference. This field is ignored if Conference=false.\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.MakeCall.Params.Hairpin.NAME) + "\" displayName=\"" + XmlEncode(Action.MakeCall.Params.Hairpin.DISPLAY) + "\" type=\"System.Boolean\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.MakeCall.Params.Hairpin.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.MakeCall.Params.Timeout.NAME) + "\" type=\"System.Int32\" displayName=\"" + XmlEncode(Action.MakeCall.Params.Timeout.DISPLAY) + "\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.MakeCall.Params.Timeout.DESCRIPTION) + "\" />\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.MakeCall.Results.CallId.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.MakeCall.Results.CallId.DESCRIPTION) + "\">" + XmlEncode(Action.MakeCall.Results.CallId.NAME) + "</resultData>\n" + 
"      <returnValue>\n" + 
"        <EnumItem>Success</EnumItem>\n" + 
"        <EnumItem>Failure</EnumItem>\n" + 
"        <EnumItem>Timeout</EnumItem>\n" + 
"      </returnValue>\n" + 
"      <asyncCallback>Metreos.CallControl.MakeCall_Complete</asyncCallback>\n" + 
"      <asyncCallback>Metreos.CallControl.MakeCall_Failed</asyncCallback>\n" + 
"      <asyncCallback>Metreos.CallControl.RemoteHangup</asyncCallback>\n" + 
"    </action>\n" + 
"    <action name=\"" + XmlEncode(Action.Barge.NAME) + "\" type=\"provider\" allowCustomParams=\"false\" final=\"false\" displayName=\"" + XmlEncode(Action.Barge.DISPLAY) + "\" description=\"" + XmlEncode(Action.Barge.DESCRIPTION) + "\">\n" + 
"      <actionParam name=\"" + XmlEncode(Action.Barge.Params.From.NAME) + "\" displayName=\"Line DN\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.Barge.Params.From.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.Barge.Params.MediaRxIP.NAME) + "\" displayName=\"Rx IP\" type=\"System.String\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.Barge.Params.MediaRxIP.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.Barge.Params.MediaRxPort.NAME) + "\" displayName=\"Rx Port\" type=\"System.UInt32\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.Barge.Params.MediaRxPort.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.Barge.Params.Timeout.NAME) + "\" type=\"System.Int32\" displayName=\"" + XmlEncode(Action.Barge.Params.Timeout.DISPLAY) + "\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.Barge.Params.Timeout.DESCRIPTION) + "\" />\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.Barge.Results.CallId.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.Barge.Results.CallId.DESCRIPTION) + "\">" + XmlEncode(Action.Barge.Results.CallId.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.Barge.Results.MmsId.DISPLAY) + "\" type=\"System.UInt32\" description=\"" + XmlEncode(Action.Barge.Results.MmsId.DESCRIPTION) + "\">" + XmlEncode(Action.Barge.Results.MmsId.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.Barge.Results.ConnectionId.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.Barge.Results.ConnectionId.DESCRIPTION) + "\">" + XmlEncode(Action.Barge.Results.ConnectionId.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.Barge.Results.MediaTxIP.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.Barge.Results.MediaTxIP.DESCRIPTION) + "\">" + XmlEncode(Action.Barge.Results.MediaTxIP.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.Barge.Results.MediaTxPort.DISPLAY) + "\" type=\"System.UInt32\" description=\"" + XmlEncode(Action.Barge.Results.MediaTxPort.DESCRIPTION) + "\">" + XmlEncode(Action.Barge.Results.MediaTxPort.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.Barge.Results.MediaTxCodec.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.Barge.Results.MediaTxCodec.DESCRIPTION) + "\">" + XmlEncode(Action.Barge.Results.MediaTxCodec.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.Barge.Results.MediaTxFramesize.DISPLAY) + "\" type=\"System.UInt32\" description=\"" + XmlEncode(Action.Barge.Results.MediaTxFramesize.DESCRIPTION) + "\">" + XmlEncode(Action.Barge.Results.MediaTxFramesize.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.Barge.Results.MediaRxIP.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.Barge.Results.MediaRxIP.DESCRIPTION) + "\">" + XmlEncode(Action.Barge.Results.MediaRxIP.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.Barge.Results.MediaRxPort.DISPLAY) + "\" type=\"System.UInt32\" description=\"" + XmlEncode(Action.Barge.Results.MediaRxPort.DESCRIPTION) + "\">" + XmlEncode(Action.Barge.Results.MediaRxPort.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.Barge.Results.MediaRxCodec.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.Barge.Results.MediaRxCodec.DESCRIPTION) + "\">" + XmlEncode(Action.Barge.Results.MediaRxCodec.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.Barge.Results.MediaRxFramesize.DISPLAY) + "\" type=\"System.UInt32\" description=\"" + XmlEncode(Action.Barge.Results.MediaRxFramesize.DESCRIPTION) + "\">" + XmlEncode(Action.Barge.Results.MediaRxFramesize.NAME) + "</resultData>\n" + 
"      <returnValue>\n" + 
"        <EnumItem>Success</EnumItem>\n" + 
"        <EnumItem>Failure</EnumItem>\n" + 
"        <EnumItem>Timeout</EnumItem>\n" + 
"      </returnValue>\n" + 
"    </action>\n" + 
"    <action name=\"" + XmlEncode(Action.AcceptCall.NAME) + "\" type=\"provider\" allowCustomParams=\"false\" final=\"false\" displayName=\"Accept Call\" description=\"" + XmlEncode(Action.AcceptCall.DESCRIPTION) + "\">\n" + 
"      <actionParam name=\"" + XmlEncode(Action.AcceptCall.Params.CallId.NAME) + "\" displayName=\"" + XmlEncode(Action.AcceptCall.Params.CallId.DISPLAY) + "\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.AcceptCall.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.AcceptCall.Params.Timeout.NAME) + "\" type=\"System.Int32\" displayName=\"" + XmlEncode(Action.AcceptCall.Params.Timeout.DISPLAY) + "\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.AcceptCall.Params.Timeout.DESCRIPTION) + "\" />\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.AcceptCall.Results.CallId.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.AcceptCall.Results.CallId.DESCRIPTION) + "\">" + XmlEncode(Action.AcceptCall.Results.CallId.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.AcceptCall.Results.MmsId.DISPLAY) + "\" type=\"System.UInt32\" description=\"" + XmlEncode(Action.AcceptCall.Results.MmsId.DESCRIPTION) + "\">" + XmlEncode(Action.AcceptCall.Results.MmsId.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.AcceptCall.Results.ConnectionId.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.AcceptCall.Results.ConnectionId.DESCRIPTION) + "\">" + XmlEncode(Action.AcceptCall.Results.ConnectionId.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.AcceptCall.Results.MediaRxIP.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.AcceptCall.Results.MediaRxIP.DESCRIPTION) + "\">" + XmlEncode(Action.AcceptCall.Results.MediaRxIP.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.AcceptCall.Results.MediaRxPort.DISPLAY) + "\" type=\"System.UInt32\" description=\"" + XmlEncode(Action.AcceptCall.Results.MediaRxPort.DESCRIPTION) + "\">" + XmlEncode(Action.AcceptCall.Results.MediaRxPort.NAME) + "</resultData>\n" + 
"      <returnValue>\n" + 
"        <EnumItem>Success</EnumItem>\n" + 
"        <EnumItem>Failure</EnumItem>\n" + 
"        <EnumItem>Timeout</EnumItem>\n" + 
"      </returnValue>\n" + 
"    </action>\n" + 
"    <action name=\"" + XmlEncode(Action.AnswerCall.NAME) + "\" type=\"provider\" allowCustomParams=\"false\" final=\"false\" displayName=\"Answer Call\" description=\"" + XmlEncode(Action.AnswerCall.DESCRIPTION) + "\">\n" + 
"      <actionParam name=\"" + XmlEncode(Action.AnswerCall.Params.CallId.NAME) + "\" displayName=\"" + XmlEncode(Action.AnswerCall.Params.CallId.DISPLAY) + "\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.AnswerCall.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.AnswerCall.Params.DisplayName.NAME) + "\" displayName=\"" + XmlEncode(Action.AnswerCall.Params.DisplayName.DISPLAY) + "\" type=\"System.String\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.AnswerCall.Params.DisplayName.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.AnswerCall.Params.MmsId.NAME) + "\" displayName=\"" + XmlEncode(Action.AnswerCall.Params.MmsId.DISPLAY) + "\" type=\"System.UInt32\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.AnswerCall.Params.MmsId.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.AnswerCall.Params.ProxyDTMFCallId.NAME) + "\" displayName=\"" + XmlEncode(Action.AnswerCall.Params.ProxyDTMFCallId.DISPLAY) + "\" type=\"System.String\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.AnswerCall.Params.ProxyDTMFCallId.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.AnswerCall.Params.WaitForMedia.NAME) + "\" displayName=\"" + XmlEncode(Action.AnswerCall.Params.WaitForMedia.DISPLAY) + "\" type=\"Metreos.AppServer.TelephonyManager.WaitMedia\" use=\"optional\" allowMultiple=\"false\" default=\"TxRx\" description=\"" + XmlEncode(Action.AnswerCall.Params.WaitForMedia.DESCRIPTION) + "\" >\n" + 
"        <EnumItem>None</EnumItem>\n" + 
"        <EnumItem>Tx</EnumItem>\n" + 
"        <EnumItem>Rx</EnumItem>\n" + 
"        <EnumItem>TxRx</EnumItem>\n" + 
"      </actionParam>\n" + 
"      <actionParam name=\"" + XmlEncode(Action.AnswerCall.Params.Conference.NAME) + "\" displayName=\"" + XmlEncode(Action.AnswerCall.Params.Conference.DISPLAY) + "\" type=\"System.Boolean\" use=\"optional\" allowMultiple=\"false\" default=\"false\" description=\"" + XmlEncode(Action.AnswerCall.Params.Conference.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.AnswerCall.Params.ConferenceId.NAME) + "\" displayName=\"" + XmlEncode(Action.AnswerCall.Params.ConferenceId.DISPLAY) + "\" type=\"System.String\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.AnswerCall.Params.ConferenceId.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.AnswerCall.Params.Hairpin.NAME) + "\" displayName=\"" + XmlEncode(Action.AnswerCall.Params.Hairpin.DISPLAY) + "\" type=\"System.Boolean\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.AnswerCall.Params.Hairpin.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.AnswerCall.Params.Timeout.NAME) + "\" type=\"System.Int32\" displayName=\"" + XmlEncode(Action.AnswerCall.Params.Timeout.DISPLAY) + "\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.AnswerCall.Params.Timeout.DESCRIPTION) + "\" />\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.AnswerCall.Results.CallId.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.AnswerCall.Results.CallId.DESCRIPTION) + "\">" + XmlEncode(Action.AnswerCall.Results.CallId.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.AnswerCall.Results.MmsId.DISPLAY) + "\" type=\"System.UInt32\" description=\"" + XmlEncode(Action.AnswerCall.Results.MmsId.DESCRIPTION) + "\">" + XmlEncode(Action.AnswerCall.Results.MmsId.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.AnswerCall.Results.ConnectionId.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.AnswerCall.Results.ConnectionId.DESCRIPTION) + "\">" + XmlEncode(Action.AnswerCall.Results.ConnectionId.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.AnswerCall.Results.ConferenceId.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.AnswerCall.Results.ConferenceId.DESCRIPTION) + "\">" + XmlEncode(Action.AnswerCall.Results.ConferenceId.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.AnswerCall.Results.MediaTxIP.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.AnswerCall.Results.MediaTxIP.DESCRIPTION) + "\">" + XmlEncode(Action.AnswerCall.Results.MediaTxIP.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.AnswerCall.Results.MediaTxPort.DISPLAY) + "\" type=\"System.UInt32\" description=\"" + XmlEncode(Action.AnswerCall.Results.MediaTxPort.DESCRIPTION) + "\">" + XmlEncode(Action.AnswerCall.Results.MediaTxPort.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.AnswerCall.Results.MediaTxCodec.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.AnswerCall.Results.MediaTxCodec.DESCRIPTION) + "\">" + XmlEncode(Action.AnswerCall.Results.MediaTxCodec.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.AnswerCall.Results.MediaTxFramesize.DISPLAY) + "\" type=\"System.UInt32\" description=\"" + XmlEncode(Action.AnswerCall.Results.MediaTxFramesize.DESCRIPTION) + "\">" + XmlEncode(Action.AnswerCall.Results.MediaTxFramesize.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.AnswerCall.Results.MediaRxIP.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.AnswerCall.Results.MediaRxIP.DESCRIPTION) + "\">" + XmlEncode(Action.AnswerCall.Results.MediaRxIP.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.AnswerCall.Results.MediaRxPort.DISPLAY) + "\" type=\"System.UInt32\" description=\"" + XmlEncode(Action.AnswerCall.Results.MediaRxPort.DESCRIPTION) + "\">" + XmlEncode(Action.AnswerCall.Results.MediaRxPort.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.AnswerCall.Results.MediaRxCodec.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.AnswerCall.Results.MediaRxCodec.DESCRIPTION) + "\">" + XmlEncode(Action.AnswerCall.Results.MediaRxCodec.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.AnswerCall.Results.MediaRxFramesize.DISPLAY) + "\" type=\"System.UInt32\" description=\"" + XmlEncode(Action.AnswerCall.Results.MediaRxFramesize.DESCRIPTION) + "\">" + XmlEncode(Action.AnswerCall.Results.MediaRxFramesize.NAME) + "</resultData>\n" + 
"      <returnValue>\n" + 
"        <EnumItem>Success</EnumItem>\n" + 
"        <EnumItem>Failure</EnumItem>\n" + 
"        <EnumItem>Timeout</EnumItem>\n" + 
"      </returnValue>\n" + 
"    </action>\n" + 
"    <action name=\"" + XmlEncode(Action.RejectCall.NAME) + "\" type=\"provider\" allowCustomParams=\"false\" final=\"false\" displayName=\"Reject Call\" description=\"" + XmlEncode(Action.RejectCall.DESCRIPTION) + "\">\n" + 
"      <actionParam name=\"" + XmlEncode(Action.RejectCall.Params.CallId.NAME) + "\" displayName=\"" + XmlEncode(Action.RejectCall.Params.CallId.DISPLAY) + "\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.RejectCall.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.RejectCall.Params.Timeout.NAME) + "\" type=\"System.Int32\" displayName=\"" + XmlEncode(Action.RejectCall.Params.Timeout.DISPLAY) + "\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.RejectCall.Params.Timeout.DESCRIPTION) + "\" />\n" + 
"      <returnValue>\n" + 
"        <EnumItem>Success</EnumItem>\n" + 
"        <EnumItem>Failure</EnumItem>\n" + 
"        <EnumItem>Timeout</EnumItem>\n" + 
"      </returnValue>\n" + 
"    </action>\n" + 
"    <action name=\"" + XmlEncode(Action.Hangup.NAME) + "\" type=\"provider\" allowCustomParams=\"false\" final=\"false\" displayName=\"Hang up\" description=\"" + XmlEncode(Action.Hangup.DESCRIPTION) + "\">\n" + 
"      <actionParam name=\"" + XmlEncode(Action.Hangup.Params.CallId.NAME) + "\" displayName=\"" + XmlEncode(Action.Hangup.Params.CallId.DISPLAY) + "\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.Hangup.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.Hangup.Params.Timeout.NAME) + "\" type=\"System.Int32\" displayName=\"" + XmlEncode(Action.Hangup.Params.Timeout.DISPLAY) + "\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.Hangup.Params.Timeout.DESCRIPTION) + "\" />\n" + 
"      <returnValue>\n" + 
"        <EnumItem>Success</EnumItem>\n" + 
"        <EnumItem>Failure</EnumItem>\n" + 
"        <EnumItem>Timeout</EnumItem>\n" + 
"      </returnValue>\n" + 
"    </action>\n" + 
"    <action name=\"" + XmlEncode(Action.BridgeCalls.NAME) + "\" type=\"provider\" allowCustomParams=\"false\" final=\"false\" displayName=\"Bridge Calls\" description=\"" + XmlEncode(Action.BridgeCalls.DESCRIPTION) + "\">\n" + 
"      <actionParam name=\"" + XmlEncode(Action.BridgeCalls.Params.CallId.NAME) + "\" displayName=\"" + XmlEncode(Action.BridgeCalls.Params.CallId.DISPLAY) + "\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.BridgeCalls.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.BridgeCalls.Params.Timeout.NAME) + "\" type=\"System.Int32\" displayName=\"" + XmlEncode(Action.BridgeCalls.Params.Timeout.DISPLAY) + "\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.BridgeCalls.Params.Timeout.DESCRIPTION) + "\" />\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.BridgeCalls.Results.MmsId.DISPLAY) + "\" type=\"System.UInt32\" description=\"" + XmlEncode(Action.BridgeCalls.Results.MmsId.DESCRIPTION) + "\">" + XmlEncode(Action.BridgeCalls.Results.MmsId.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.BridgeCalls.Results.ConnectionId.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.BridgeCalls.Results.ConnectionId.DESCRIPTION) + "\">" + XmlEncode(Action.BridgeCalls.Results.ConnectionId.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.BridgeCalls.Results.PeerConnectionId.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.BridgeCalls.Results.PeerConnectionId.DESCRIPTION) + "\">" + XmlEncode(Action.BridgeCalls.Results.PeerConnectionId.NAME) + "</resultData> \n" + 
"      <resultData displayName=\"" + XmlEncode(Action.BridgeCalls.Results.ConferenceId.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.BridgeCalls.Results.ConferenceId.DESCRIPTION) + "\">" + XmlEncode(Action.BridgeCalls.Results.ConferenceId.NAME) + "</resultData>     \n" + 
"      <resultData displayName=\"" + XmlEncode(Action.BridgeCalls.Results.MediaRxIP.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.BridgeCalls.Results.MediaRxIP.DESCRIPTION) + "\">" + XmlEncode(Action.BridgeCalls.Results.MediaRxIP.NAME) + "</resultData>\n" + 
"      <returnValue>\n" + 
"        <EnumItem>Success</EnumItem>\n" + 
"        <EnumItem>Failure</EnumItem>\n" + 
"        <EnumItem>Timeout</EnumItem>\n" + 
"      </returnValue>\n" + 
"    </action>\n" + 
"    <action name=\"" + XmlEncode(Action.UnbridgeCalls.NAME) + "\" type=\"provider\" allowCustomParams=\"false\" final=\"false\" displayName=\"Unbridge Calls\" description=\"" + XmlEncode(Action.UnbridgeCalls.DESCRIPTION) + "\">\n" + 
"      <actionParam name=\"" + XmlEncode(Action.UnbridgeCalls.Params.CallId.NAME) + "\" displayName=\"" + XmlEncode(Action.UnbridgeCalls.Params.CallId.DISPLAY) + "\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.UnbridgeCalls.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.UnbridgeCalls.Params.PeerCallId.NAME) + "\" displayName=\"" + XmlEncode(Action.UnbridgeCalls.Params.PeerCallId.DISPLAY) + "\" type=\"System.String\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.UnbridgeCalls.Params.PeerCallId.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.UnbridgeCalls.Params.Timeout.NAME) + "\" type=\"System.Int32\" displayName=\"" + XmlEncode(Action.UnbridgeCalls.Params.Timeout.DISPLAY) + "\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.UnbridgeCalls.Params.Timeout.DESCRIPTION) + "\" />\n" + 
"      <returnValue>\n" + 
"        <EnumItem>Success</EnumItem>\n" + 
"        <EnumItem>Failure</EnumItem>\n" + 
"        <EnumItem>Timeout</EnumItem>\n" + 
"      </returnValue>\n" + 
"    </action>\n" + 
"    <action name=\"" + XmlEncode(Action.BlindTransfer.NAME) + "\" type=\"provider\" allowCustomParams=\"false\" final=\"false\" displayName=\"Blind Transfer\" description=\"" + XmlEncode(Action.BlindTransfer.DESCRIPTION) + "\">\n" + 
"      <actionParam name=\"" + XmlEncode(Action.BlindTransfer.Params.CallId.NAME) + "\" displayName=\"" + XmlEncode(Action.BlindTransfer.Params.CallId.DISPLAY) + "\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.BlindTransfer.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.BlindTransfer.Params.To.NAME) + "\" displayName=\"" + XmlEncode(Action.BlindTransfer.Params.To.DISPLAY) + "\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.BlindTransfer.Params.To.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.BlindTransfer.Params.Timeout.NAME) + "\" type=\"System.Int32\" displayName=\"" + XmlEncode(Action.BlindTransfer.Params.Timeout.DISPLAY) + "\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.BlindTransfer.Params.Timeout.DESCRIPTION) + "\" />\n" + 
"      <returnValue>\n" + 
"        <EnumItem>Success</EnumItem>\n" + 
"        <EnumItem>Failure</EnumItem>\n" + 
"        <EnumItem>Timeout</EnumItem>\n" + 
"      </returnValue>\n" + 
"    </action>\n" + 
"    <!-- Not surrently supported\n" + 
"    <action name=\"" + XmlEncode(Action.BeginConsultationTransfer.NAME) + "\" type=\"provider\" allowCustomParams=\"false\" final=\"false\" displayName=\"Begin Consultation Transfer\" description=\"" + XmlEncode(Action.BeginConsultationTransfer.DESCRIPTION) + "\">\n" + 
"      <actionParam name=\"" + XmlEncode(Action.BeginConsultationTransfer.Params.CallId.NAME) + "\" displayName=\"" + XmlEncode(Action.BeginConsultationTransfer.Params.CallId.DISPLAY) + "\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.BeginConsultationTransfer.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.BeginConsultationTransfer.Params.To.NAME) + "\" displayName=\"" + XmlEncode(Action.BeginConsultationTransfer.Params.To.DISPLAY) + "\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.BeginConsultationTransfer.Params.To.DESCRIPTION) + "\" />\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.BeginConsultationTransfer.Results.TransferCallId.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.BeginConsultationTransfer.Results.TransferCallId.DESCRIPTION) + "\">" + XmlEncode(Action.BeginConsultationTransfer.Results.TransferCallId.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.BeginConsultationTransfer.Results.MmsId.DISPLAY) + "\" type=\"System.UInt32\" description=\"" + XmlEncode(Action.BeginConsultationTransfer.Results.MmsId.DESCRIPTION) + "\">" + XmlEncode(Action.BeginConsultationTransfer.Results.MmsId.NAME) + "</resultData>\n" + 
"      <resultData displayName=\"" + XmlEncode(Action.BeginConsultationTransfer.Results.ConnectionId.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.BeginConsultationTransfer.Results.ConnectionId.DESCRIPTION) + "\">" + XmlEncode(Action.BeginConsultationTransfer.Results.ConnectionId.NAME) + "</resultData>\n" + 
"      <actionParam name=\"" + XmlEncode(Action.BeginConsultationTransfer.Params.Timeout.NAME) + "\" type=\"System.Int32\" displayName=\"" + XmlEncode(Action.BeginConsultationTransfer.Params.Timeout.DISPLAY) + "\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.BeginConsultationTransfer.Params.Timeout.DESCRIPTION) + "\" />\n" + 
"      <returnValue>\n" + 
"        <EnumItem>Success</EnumItem>\n" + 
"        <EnumItem>Failure</EnumItem>\n" + 
"        <EnumItem>Timeout</EnumItem>\n" + 
"      </returnValue>\n" + 
"    </action>\n" + 
"    <action name=\"" + XmlEncode(Action.EndConsultationTransfer.NAME) + "\" type=\"provider\" allowCustomParams=\"false\" final=\"false\" displayName=\"End Consultation Transfer\" description=\"" + XmlEncode(Action.EndConsultationTransfer.DESCRIPTION) + "\">\n" + 
"      <actionParam name=\"" + XmlEncode(Action.EndConsultationTransfer.Params.CallId.NAME) + "\" displayName=\"" + XmlEncode(Action.EndConsultationTransfer.Params.CallId.DISPLAY) + "\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.EndConsultationTransfer.Params.CallId.DESCRIPTION) + "\" />\n" + 
"    <actionParam name=\"" + XmlEncode(Action.EndConsultationTransfer.Params.TransferCallId.NAME) + "\" displayName=\"" + XmlEncode(Action.EndConsultationTransfer.Params.TransferCallId.DISPLAY) + "\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.EndConsultationTransfer.Params.TransferCallId.DESCRIPTION) + "\" />\n" + 
"    <actionParam name=\"" + XmlEncode(Action.EndConsultationTransfer.Params.Timeout.NAME) + "\" type=\"System.Int32\" displayName=\"" + XmlEncode(Action.EndConsultationTransfer.Params.Timeout.DISPLAY) + "\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.EndConsultationTransfer.Params.Timeout.DESCRIPTION) + "\" />\n" + 
"      <returnValue>\n" + 
"        <EnumItem>Success</EnumItem>\n" + 
"        <EnumItem>Failure</EnumItem>\n" + 
"        <EnumItem>Timeout</EnumItem>\n" + 
"      </returnValue>\n" + 
"    </action>\n" + 
"    -->\n" + 
"    <action name=\"" + XmlEncode(Action.Redirect.NAME) + "\" type=\"provider\" allowCustomParams=\"false\" final=\"false\" displayName=\"" + XmlEncode(Action.Redirect.DISPLAY) + "\" description=\"" + XmlEncode(Action.Redirect.DESCRIPTION) + "\">\n" + 
"      <actionParam name=\"" + XmlEncode(Action.Redirect.Params.CallId.NAME) + "\" displayName=\"" + XmlEncode(Action.Redirect.Params.CallId.DISPLAY) + "\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.Redirect.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.Redirect.Params.To.NAME) + "\" displayName=\"" + XmlEncode(Action.Redirect.Params.To.DISPLAY) + "\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.Redirect.Params.To.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.Redirect.Params.Timeout.NAME) + "\" type=\"System.Int32\" displayName=\"" + XmlEncode(Action.Redirect.Params.Timeout.DISPLAY) + "\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.Redirect.Params.Timeout.DESCRIPTION) + "\" />\n" + 
"      <returnValue>\n" + 
"        <EnumItem>Success</EnumItem>\n" + 
"        <EnumItem>Failure</EnumItem>\n" + 
"        <EnumItem>Timeout</EnumItem>\n" + 
"      </returnValue>\n" + 
"    </action>\n" + 
"    <action name=\"" + XmlEncode(Action.SendUserInput.NAME) + "\" type=\"provider\" allowCustomParams=\"false\" final=\"false\" displayName=\"Send User Input\" description=\"" + XmlEncode(Action.SendUserInput.DESCRIPTION) + "\">\n" + 
"      <actionParam name=\"" + XmlEncode(Action.SendUserInput.Params.CallId.NAME) + "\" displayName=\"" + XmlEncode(Action.SendUserInput.Params.CallId.DISPLAY) + "\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.SendUserInput.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.SendUserInput.Params.Digits.NAME) + "\" displayName=\"" + XmlEncode(Action.SendUserInput.Params.Digits.DISPLAY) + "\" type=\"System.String\" use=\"required\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.SendUserInput.Params.Digits.DESCRIPTION) + "\" />\n" + 
"      <actionParam name=\"" + XmlEncode(Action.SendUserInput.Params.Timeout.NAME) + "\" type=\"System.Int32\" displayName=\"" + XmlEncode(Action.SendUserInput.Params.Timeout.DISPLAY) + "\" use=\"optional\" allowMultiple=\"false\" description=\"" + XmlEncode(Action.SendUserInput.Params.Timeout.DESCRIPTION) + "\" />\n" + 
"      <returnValue>\n" + 
"        <EnumItem>Success</EnumItem>\n" + 
"        <EnumItem>Failure</EnumItem>\n" + 
"        <EnumItem>Timeout</EnumItem>\n" + 
"      </returnValue>\n" + 
"    </action>\n" + 
"  </actionList>\n" + 
"  <eventList>\n" + 
"    <event name=\"" + XmlEncode(Event.IncomingCall.NAME) + "\" type=\"triggering\" expects=\"Metreos.CallControl.AnswerCall\" displayName=\"Incoming Call\" description=\"" + XmlEncode(Event.IncomingCall.DESCRIPTION) + "\">\n" + 
"      <eventParam name=\"" + XmlEncode(Event.IncomingCall.Params.CallId.NAME) + " displayName=\"" + XmlEncode(Event.IncomingCall.Params.CallId.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.IncomingCall.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.IncomingCall.Params.From.NAME) + " displayName=\"" + XmlEncode(Event.IncomingCall.Params.From.DISPLAY) + "\" type=\"System.String\" guaranteed=\"false\" description=\"" + XmlEncode(Event.IncomingCall.Params.From.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.IncomingCall.Params.To.NAME) + " displayName=\"" + XmlEncode(Event.IncomingCall.Params.To.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.IncomingCall.Params.To.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.IncomingCall.Params.OriginalTo.NAME) + " displayName=\"" + XmlEncode(Event.IncomingCall.Params.OriginalTo.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.IncomingCall.Params.OriginalTo.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.IncomingCall.Params.DisplayName.NAME) + " displayName=\"" + XmlEncode(Event.IncomingCall.Params.DisplayName.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"The caller's friendly name.\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.IncomingCall.Params.StackToken.NAME) + " displayName=\"" + XmlEncode(Event.IncomingCall.Params.StackToken.DISPLAY) + "\" type=\"System.String\" guaranteed=\"false\" description=\"" + XmlEncode(Event.IncomingCall.Params.StackToken.DESCRIPTION) + "\" />\n" + 
"    </event>\n" + 
"    <event name=\"" + XmlEncode(Event.RemoteHangup.NAME) + "\" type=\"nontriggering\" displayName=\"Remote Hang up\" description=\"" + XmlEncode(Event.RemoteHangup.DESCRIPTION) + "\">\n" + 
"      <eventParam name=\"" + XmlEncode(Event.RemoteHangup.Params.CallId.NAME) + " displayName=\"" + XmlEncode(Event.RemoteHangup.Params.CallId.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.RemoteHangup.Params.CallId.DESCRIPTION) + "\" />\n" + 
"	  <eventParam name=\"" + XmlEncode(Event.RemoteHangup.Params.EndReason.NAME) + " displayName=\"" + XmlEncode(Event.RemoteHangup.Params.EndReason.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.RemoteHangup.Params.EndReason.DESCRIPTION) + "\" />\n" + 
"    </event>\n" + 
"    <event name=\"" + XmlEncode(Event.GotDigits.NAME) + "\" type=\"nontriggering\" displayName=\"Got Digits\" description=\"" + XmlEncode(Event.GotDigits.DESCRIPTION) + "\">\n" + 
"      <eventParam name=\"" + XmlEncode(Event.GotDigits.Params.CallId.NAME) + " displayName=\"" + XmlEncode(Event.GotDigits.Params.CallId.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.GotDigits.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.GotDigits.Params.Digits.NAME) + " displayName=\"" + XmlEncode(Event.GotDigits.Params.Digits.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.GotDigits.Params.Digits.DESCRIPTION) + "\" />\n" + 
"    </event>\n" + 
"    <event name=\"" + XmlEncode(Event.StartTx.NAME) + "\" type=\"nontriggering\" displayName=\"Start Tx\" description=\"" + XmlEncode(Event.StartTx.DESCRIPTION) + "\">\n" + 
"      <eventParam name=\"" + XmlEncode(Event.StartTx.Params.CallId.NAME) + " displayName=\"" + XmlEncode(Event.StartTx.Params.CallId.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.StartTx.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.StartTx.Params.MmsId.NAME) + " displayName=\"" + XmlEncode(Event.StartTx.Params.MmsId.DISPLAY) + "\" type=\"System.UInt32\" guaranteed=\"true\" description=\"" + XmlEncode(Event.StartTx.Params.MmsId.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.StartTx.Params.ConnectionId.NAME) + " displayName=\"" + XmlEncode(Event.StartTx.Params.ConnectionId.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.StartTx.Params.ConnectionId.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.StartTx.Params.MediaTxIP.NAME) + " displayName=\"" + XmlEncode(Event.StartTx.Params.MediaTxIP.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.StartTx.Params.MediaTxIP.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.StartTx.Params.MediaTxPort.NAME) + " displayName=\"" + XmlEncode(Event.StartTx.Params.MediaTxPort.DISPLAY) + "\" type=\"System.UInt32\" guaranteed=\"true\" description=\"" + XmlEncode(Event.StartTx.Params.MediaTxPort.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.StartTx.Params.MediaTxCodec.NAME) + " displayName=\"" + XmlEncode(Event.StartTx.Params.MediaTxCodec.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.StartTx.Params.MediaTxCodec.DESCRIPTION) + "\"  />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.StartTx.Params.MediaTxFramesize.NAME) + " displayName=\"" + XmlEncode(Event.StartTx.Params.MediaTxFramesize.DISPLAY) + "\" type=\"System.UInt32\" guaranteed=\"true\" description=\"" + XmlEncode(Event.StartTx.Params.MediaTxFramesize.DESCRIPTION) + "\" />\n" + 
"    </event>\n" + 
"    <event name=\"" + XmlEncode(Event.StopTx.NAME) + "\" type=\"nontriggering\" displayName=\"Stop Tx\" description=\"Indicates that an outbound media channel has been closed. This may occur if the remote party presses 'hold', for instance.\">\n" + 
"      <eventParam name=\"" + XmlEncode(Event.StopTx.Params.CallId.NAME) + " displayName=\"" + XmlEncode(Event.StopTx.Params.CallId.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.StopTx.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.StopTx.Params.MmsId.NAME) + " displayName=\"" + XmlEncode(Event.StopTx.Params.MmsId.DISPLAY) + "\" type=\"System.UInt32\" guaranteed=\"true\" description=\"" + XmlEncode(Event.StopTx.Params.MmsId.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.StopTx.Params.ConnectionId.NAME) + " displayName=\"" + XmlEncode(Event.StopTx.Params.ConnectionId.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.StopTx.Params.ConnectionId.DESCRIPTION) + "\" />\n" + 
"    </event>    \n" + 
"    <event name=\"" + XmlEncode(Event.StartRx.NAME) + "\" type=\"nontriggering\" displayName=\"Start Rx\" description=\"" + XmlEncode(Event.StartRx.DESCRIPTION) + "\">\n" + 
"      <eventParam name=\"" + XmlEncode(Event.StartRx.Params.CallId.NAME) + " displayName=\"" + XmlEncode(Event.StartRx.Params.CallId.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.StartRx.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.StartRx.Params.MmsId.NAME) + " displayName=\"" + XmlEncode(Event.StartRx.Params.MmsId.DISPLAY) + "\" type=\"System.UInt32\" guaranteed=\"true\" description=\"" + XmlEncode(Event.StartRx.Params.MmsId.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.StartRx.Params.ConnectionId.NAME) + " displayName=\"" + XmlEncode(Event.StartRx.Params.ConnectionId.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.StartRx.Params.ConnectionId.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.StartRx.Params.MediaRxIP.NAME) + " displayName=\"" + XmlEncode(Event.StartRx.Params.MediaRxIP.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.StartRx.Params.MediaRxIP.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.StartRx.Params.MediaRxPort.NAME) + " displayName=\"" + XmlEncode(Event.StartRx.Params.MediaRxPort.DISPLAY) + "\" type=\"System.UInt32\" guaranteed=\"true\" description=\"" + XmlEncode(Event.StartRx.Params.MediaRxPort.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.StartRx.Params.MediaRxCodec.NAME) + " displayName=\"" + XmlEncode(Event.StartRx.Params.MediaRxCodec.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.StartRx.Params.MediaRxCodec.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.StartRx.Params.MediaRxFramesize.NAME) + " displayName=\"" + XmlEncode(Event.StartRx.Params.MediaRxFramesize.DISPLAY) + "\" type=\"System.UInt32\" guaranteed=\"true\" description=\"" + XmlEncode(Event.StartRx.Params.MediaRxFramesize.DESCRIPTION) + "\" />\n" + 
"    </event>  \n" + 
"    <event name=\"" + XmlEncode(Event.CallChanged.NAME) + "\" type=\"nontriggering\" displayName=\"Call Changed\" description=\"" + XmlEncode(Event.CallChanged.DESCRIPTION) + "\">  \n" + 
"      <eventParam name=\"" + XmlEncode(Event.CallChanged.Params.CallId.NAME) + " displayName=\"" + XmlEncode(Event.CallChanged.Params.CallId.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.CallChanged.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.CallChanged.Params.To.NAME) + " displayName=\"" + XmlEncode(Event.CallChanged.Params.To.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.CallChanged.Params.To.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.CallChanged.Params.From.NAME) + " displayName=\"" + XmlEncode(Event.CallChanged.Params.From.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.CallChanged.Params.From.DESCRIPTION) + "\" />\n" + 
"    </event>\n" + 
"    <event name=\"" + XmlEncode(Event.MakeCall_Complete.NAME) + "\" type=\"asyncCallback\" displayName=\"Make Call Complete\" description=\"" + XmlEncode(Event.MakeCall_Complete.DESCRIPTION) + "\">\n" + 
"      <eventParam name=\"" + XmlEncode(Event.MakeCall_Complete.Params.CallId.NAME) + " displayName=\"" + XmlEncode(Event.MakeCall_Complete.Params.CallId.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.MakeCall_Complete.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.MakeCall_Complete.Params.MmsId.NAME) + " displayName=\"" + XmlEncode(Event.MakeCall_Complete.Params.MmsId.DISPLAY) + "\" type=\"System.UInt32\" guaranteed=\"true\" description=\"" + XmlEncode(Event.MakeCall_Complete.Params.MmsId.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.MakeCall_Complete.Params.To.NAME) + " displayName=\"" + XmlEncode(Event.MakeCall_Complete.Params.To.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.MakeCall_Complete.Params.To.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.MakeCall_Complete.Params.From.NAME) + " displayName=\"" + XmlEncode(Event.MakeCall_Complete.Params.From.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.MakeCall_Complete.Params.From.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.MakeCall_Complete.Params.OriginalTo.NAME) + " displayName=\"" + XmlEncode(Event.MakeCall_Complete.Params.OriginalTo.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.MakeCall_Complete.Params.OriginalTo.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.MakeCall_Complete.Params.ConnectionId.NAME) + " displayName=\"" + XmlEncode(Event.MakeCall_Complete.Params.ConnectionId.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.MakeCall_Complete.Params.ConnectionId.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.MakeCall_Complete.Params.ConferenceId.NAME) + " displayName=\"" + XmlEncode(Event.MakeCall_Complete.Params.ConferenceId.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.MakeCall_Complete.Params.ConferenceId.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.MakeCall_Complete.Params.MediaTxIP.NAME) + " displayName=\"" + XmlEncode(Event.MakeCall_Complete.Params.MediaTxIP.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.MakeCall_Complete.Params.MediaTxIP.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.MakeCall_Complete.Params.MediaTxPort.NAME) + " displayName=\"" + XmlEncode(Event.MakeCall_Complete.Params.MediaTxPort.DISPLAY) + "\" type=\"System.UInt32\" guaranteed=\"true\" description=\"" + XmlEncode(Event.MakeCall_Complete.Params.MediaTxPort.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.MakeCall_Complete.Params.MediaTxCodec.NAME) + " displayName=\"" + XmlEncode(Event.MakeCall_Complete.Params.MediaTxCodec.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.MakeCall_Complete.Params.MediaTxCodec.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.MakeCall_Complete.Params.MediaTxFramesize.NAME) + " displayName=\"" + XmlEncode(Event.MakeCall_Complete.Params.MediaTxFramesize.DISPLAY) + "\" type=\"System.UInt32\" guaranteed=\"true\" description=\"" + XmlEncode(Event.MakeCall_Complete.Params.MediaTxFramesize.DESCRIPTION) + "\" />\n" + 
"    </event>\n" + 
"    <event name=\"" + XmlEncode(Event.MakeCall_Failed.NAME) + "\" type=\"asyncCallback\" displayName=\"Make Call Failed\" description=\"" + XmlEncode(Event.MakeCall_Failed.DESCRIPTION) + "\">\n" + 
"      <eventParam name=\"" + XmlEncode(Event.MakeCall_Failed.Params.CallId.NAME) + " displayName=\"" + XmlEncode(Event.MakeCall_Failed.Params.CallId.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.MakeCall_Failed.Params.CallId.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.MakeCall_Failed.Params.To.NAME) + " displayName=\"" + XmlEncode(Event.MakeCall_Failed.Params.To.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.MakeCall_Failed.Params.To.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.MakeCall_Failed.Params.From.NAME) + " displayName=\"" + XmlEncode(Event.MakeCall_Failed.Params.From.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.MakeCall_Failed.Params.From.DESCRIPTION) + "\" />\n" + 
"      <eventParam name=\"" + XmlEncode(Event.MakeCall_Failed.Params.EndReason.NAME) + " displayName=\"" + XmlEncode(Event.MakeCall_Failed.Params.EndReason.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.MakeCall_Failed.Params.EndReason.DESCRIPTION) + "\" />\n" + 
"    </event>\n" + 
"  </eventList>\n" + 
"</package>\n";
    }
}