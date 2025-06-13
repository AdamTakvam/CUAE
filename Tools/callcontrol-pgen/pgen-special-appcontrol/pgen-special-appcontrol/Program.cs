using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Action = Metreos.Interfaces.PackageDefinitions.ApplicationControl.Actions;
using Event = Metreos.Interfaces.PackageDefinitions.ApplicationControl.Events;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string location = @"x:\Build\Framework\1.0\Packages\Metreos.ApplicationControl.xml";
            if (args.Length == 1)
            {
                location = args[0];
            }

            FileStream writer = File.Open(location, FileMode.Create);
           
            StreamWriter streamWriter = new StreamWriter(writer);
            streamWriter.Write(AppControlText);
            streamWriter.Close();
        }

        private static string XmlEncode(string stuff)
        {
            if (stuff == null) return null;
            stuff = stuff.Replace("&", "&amp;");
            stuff = stuff.Replace("<", "&lt;");
            stuff = stuff.Replace("\"", "&quot;");
            stuff = stuff.Replace(">", "&gt;");
            stuff = stuff.Replace("\n", "&#xA;");
            stuff = stuff.Replace("\r", "&#xD;");
            stuff = stuff.Replace("'", "&apos;");
            return stuff;
        }


        public static string AppControlText =
"<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n" + 
"<package name=\"Metreos.ApplicationControl\" \n" + 
"         description=\"The Application Control API defines actions and events which provide capabilities that are inherent to the &lt;link linkend=&quot;ApiGlossary.ARE&quot;&gt;&lt;code&gt;Application Runtime Environment&lt;/code&gt;&lt;/link&gt;.\n\"" + 
"         xmlns=\"http://metreos.com/ActionEventPackage.xsd\"\n" + 
"         xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" \n" + 
"         xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n" + 
"    <actionList>\n" + 
"        <action name=\"" + XmlEncode(Action.Assign.NAME) + "\" type=\"native\" allowCustomParams=\"false\" final=\"false\" description=\"" + XmlEncode(Action.Assign.DESCRIPTION) + "\" displayName=\"" + XmlEncode(Action.Assign.DISPLAY) + "\">\n" + 
"            <actionParam name=\"" + XmlEncode(Action.Assign.Params.Value.NAME) + "\" displayName=\"" + XmlEncode(Action.Assign.Params.Value.DISPLAY) + "\" type=\"System.Object\" use=\"required\" description=\"" + XmlEncode(Action.Assign.Params.Value.DESCRIPTION) + "\" />\n" + 
"            <actionParam name=\"" + XmlEncode(Action.Assign.Params.Value2.NAME) + "\" displayName=\"" + XmlEncode(Action.Assign.Params.Value2.DISPLAY) + "\" type=\"System.Object\" use=\"optional\" description=\"" + XmlEncode(Action.Assign.Params.Value2.DESCRIPTION) + "\" />\n" + 
"            <actionParam name=\"" + XmlEncode(Action.Assign.Params.Value3.NAME) + "\" displayName=\"" + XmlEncode(Action.Assign.Params.Value3.DISPLAY) + "\" type=\"System.Object\" use=\"optional\" description=\"" + XmlEncode(Action.Assign.Params.Value3.DESCRIPTION) + "\" />\n" + 
"            <actionParam name=\"" + XmlEncode(Action.Assign.Params.Value4.NAME) + "\" displayName=\"" + XmlEncode(Action.Assign.Params.Value4.DISPLAY) + "\" type=\"System.Object\" use=\"optional\" description=\"" + XmlEncode(Action.Assign.Params.Value4.DESCRIPTION) + "\" />\n" + 
"\n" + 
"            <resultData displayName=\"" + XmlEncode(Action.Assign.Results.ResultData.DISPLAY) + "\" type=\"System.Object\" description=\"" + XmlEncode(Action.Assign.Results.ResultData.DESCRIPTION) + "\">" + XmlEncode(Action.Assign.Results.ResultData.NAME) + "</resultData>\n" + 
"            <resultData displayName=\"" + XmlEncode(Action.Assign.Results.ResultData2.DISPLAY) + "\" type=\"System.Object\" description=\"" + XmlEncode(Action.Assign.Results.ResultData2.DESCRIPTION) + "\">" + XmlEncode(Action.Assign.Results.ResultData2.NAME) + "</resultData>\n" + 
"            <resultData displayName=\"" + XmlEncode(Action.Assign.Results.ResultData3.DISPLAY) + "\" type=\"System.Object\" description=\"" + XmlEncode(Action.Assign.Results.ResultData3.DESCRIPTION) + "\">" + XmlEncode(Action.Assign.Results.ResultData3.NAME) + "</resultData>\n" + 
"            <resultData displayName=\"" + XmlEncode(Action.Assign.Results.ResultData4.DISPLAY) + "\" type=\"System.Object\" description=\"" + XmlEncode(Action.Assign.Results.ResultData4.DESCRIPTION) + "\">" + XmlEncode(Action.Assign.Results.ResultData4.NAME) + "</resultData>\n" + 
"            <returnValue description=\"success\">\n" + 
"                <EnumItem>success</EnumItem>\n" + 
"            </returnValue>\n" + 
"        </action>\n" + 
"        <action name=\"" + XmlEncode(Action.Sleep.NAME) + "\" type=\"appControl\" allowCustomParams=\"false\" final=\"false\" description=\"" + XmlEncode(Action.Sleep.DESCRIPTION) + "\" displayName=\"" + XmlEncode(Action.Sleep.DISPLAY) + "\">\n" + 
"            <actionParam name=\"" + XmlEncode(Action.Sleep.Params.SleepTime.NAME) + "\" displayName=\"" + XmlEncode(Action.Sleep.Params.SleepTime.DISPLAY) + "\" type=\"System.Int32\" use=\"required\" description=\"" + XmlEncode(Action.Sleep.Params.SleepTime.DESCRIPTION) + "\" />\n" + 
"            <returnValue description=\"success\">\n" + 
"                <EnumItem>success</EnumItem>\n" + 
"                <EnumItem>failure</EnumItem>\n" + 
"            </returnValue>\n" + 
"        </action>\n" + 
"        <action name=\"" + XmlEncode(Action.ChangeLocale.NAME) + "\" type=\"appControl\" allowCustomParams=\"false\" final=\"false\" description=\"" + XmlEncode(Action.ChangeLocale.DESCRIPTION) + "\" displayName=\"" + XmlEncode(Action.ChangeLocale.DISPLAY) + "\">\n" + 
"            <actionParam name=\"" + XmlEncode(Action.ChangeLocale.Params.Locale.NAME) + "\" displayName=\"" + XmlEncode(Action.ChangeLocale.Params.Locale.DISPLAY) + "\" type=\"System.String\" use=\"required\" description=\"" + XmlEncode(Action.ChangeLocale.Params.Locale.DESCRIPTION) + "\" />\n" + 
"            <actionParam name=\"" + XmlEncode(Action.ChangeLocale.Params.ResetStrings.NAME) + "\" displayName=\"" + XmlEncode(Action.ChangeLocale.Params.ResetStrings.DISPLAY) + "\" type=\"System.Boolean\" use=\"optional\" description=\"" + XmlEncode(Action.ChangeLocale.Params.ResetStrings.DESCRIPTION) + "\" />\n" + 
"        </action>\n" +
"        <action name=\"" + XmlEncode(Action.ConstructionComplete.NAME) + "\" type=\"appControl\" allowCustomParams=\"false\" final=\"false\" description=\"" + XmlEncode(Action.ConstructionComplete.DESCRIPTION) + "\" displayName=\"" + XmlEncode(Action.ConstructionComplete.DISPLAY) + "\">\n" + 
"            <actionParam name=\"" + XmlEncode(Action.ConstructionComplete.Params.Success.NAME) + "\" displayName=\"" + XmlEncode(Action.ConstructionComplete.Params.Success.DISPLAY) + "\" type=\"System.Boolean\" use=\"optional\" description=\"" + XmlEncode(Action.ConstructionComplete.Params.Success.DESCRIPTION) + "\" />\n" + 
"        </action>\n" + 
"        <action name=\"" + XmlEncode(Action.EndScript.NAME) + "\" type=\"appControl\" allowCustomParams=\"false\" final=\"true\" description=\"" + XmlEncode(Action.EndScript.DESCRIPTION) + "\" displayName=\"" + XmlEncode(Action.EndScript.DISPLAY) + "\" />\n" + 
"        <action name=\"" + XmlEncode(Action.EndFunction.NAME) + "\" type=\"appControl\" allowCustomParams=\"true\" final=\"true\" description=\"" + XmlEncode(Action.EndFunction.DESCRIPTION) + "\" displayName=\"" + XmlEncode(Action.EndFunction.DISPLAY) + "\" >\n" + 
"            <actionParam name=\"" + XmlEncode(Action.EndFunction.Params.ReturnValue.NAME) + "\" displayName=\"" + XmlEncode(Action.EndFunction.Params.ReturnValue.DISPLAY) + "\" type=\"System.String\" use=\"optional\" description=\"" + XmlEncode(Action.EndFunction.Params.ReturnValue.DESCRIPTION) + "\" />\n" + 
"            <returnValue description=\"success\">\n" + 
"                <EnumItem>success</EnumItem>\n" + 
"                <EnumItem>failure</EnumItem>\n" + 
"            </returnValue>\n" + 
"        </action>\n" + 
"        <action name=\"" + XmlEncode(Action.CallFunction.NAME) + "\" type=\"appControl\" allowCustomParams=\"true\" final=\"false\" description=\"" + XmlEncode(Action.CallFunction.DESCRIPTION) + "\" displayName=\"" + XmlEncode(Action.CallFunction.DISPLAY) + "\" >\n" +
"            <actionParam name=\"" + XmlEncode(Action.CallFunction.Params.FunctionName.NAME) + "\" displayName=\"" + XmlEncode(Action.CallFunction.Params.FunctionName.DISPLAY) + "\" type=\"System.String\" use=\"required\"  description=\"" + XmlEncode(Action.CallFunction.Params.FunctionName.DESCRIPTION) + "\" />\n" + 
"            <returnValue description=\"success\">\n" + 
"                <EnumItem>success</EnumItem>\n" + 
"                <EnumItem>failure</EnumItem>\n" + 
"            </returnValue>\n" + 
"        </action>\n" + 
"        <action name=\"" + XmlEncode(Action.Forward.NAME) + "\" type=\"appControl\" allowCustomParams=\"false\" final=\"true\" description=\"" + XmlEncode(Action.Forward.DESCRIPTION) + "\" displayName=\"" + XmlEncode(Action.Forward.DISPLAY) + "\" >\n" + 
"            <actionParam name=\"" + XmlEncode(Action.Forward.Params.ToGuid.NAME) + "\" displayName=\"" + XmlEncode(Action.Forward.Params.ToGuid.DISPLAY) + "\" type=\"System.String\" use=\"required\" description=\"" + XmlEncode(Action.Forward.Params.ToGuid.DESCRIPTION) + "\" />\n" + 
"        </action>\n" + 
"        <action name=\"" + XmlEncode(Action.SendEvent.NAME) + "\" type=\"appControl\" allowCustomParams=\"true\" final=\"false\" description=\"" + XmlEncode(Action.SendEvent.DESCRIPTION) + "\" displayName=\"" + XmlEncode(Action.SendEvent.DISPLAY) + "\" >\n" +
"            <actionParam name=\"" + XmlEncode(Action.SendEvent.Params.EventName.NAME) + "\" displayName=\"" + XmlEncode(Action.SendEvent.Params.EventName.DISPLAY) + "\" type=\"System.String\" use=\"required\"  description=\"" + XmlEncode(Action.SendEvent.Params.EventName.DESCRIPTION) + "\" />\n" +
"            <actionParam name=\"" + XmlEncode(Action.SendEvent.Params.ToGuid.NAME) + "\" displayName=\"" + XmlEncode(Action.SendEvent.Params.ToGuid.DISPLAY) + "\" type=\"System.String\" use=\"optional\"  description=\"" + XmlEncode(Action.SendEvent.Params.ToGuid.DESCRIPTION) + "\" />\n" + 
"            <resultData displayName=\"" + XmlEncode(Action.SendEvent.Results.DestinationGuid.DISPLAY) + "\" type=\"System.String\" description=\"" + XmlEncode(Action.SendEvent.Results.DestinationGuid.DESCRIPTION) + "\">" + XmlEncode(Action.SendEvent.Results.DestinationGuid.NAME) + "</resultData>\n" + 
"            <returnValue description=\"success\">\n" + 
"                <EnumItem>success</EnumItem>\n" + 
"                <EnumItem>failure</EnumItem>\n" + 
"            </returnValue>\n" + 
"        </action>\n" + 
"        <action name=\"" + XmlEncode(Action.SetSessionData.NAME) + "\" type=\"appControl\" allowCustomParams=\"true\" final=\"false\" description=\"" + XmlEncode(Action.SetSessionData.DESCRIPTION) + "\" displayName=\"" + XmlEncode(Action.SetSessionData.DISPLAY) + "\">\n" + 
"            <returnValue description=\"success\">\n" + 
"                <EnumItem>success</EnumItem>\n" + 
"                <EnumItem>failure</EnumItem>\n" + 
"            </returnValue>\n" + 
"        </action>\n" + 
"    </actionList>\n" + 
"    <eventList>\n" + 
"        <event name=\"" + XmlEncode(Event.StaticConstruction.NAME) + "\" type=\"triggering\" expects=\"Metreos.ApplicationControl.ContructionComplete\" displayName=\"" + XmlEncode(Event.StaticConstruction.DISPLAY) + "\" description=\"" + XmlEncode(Event.StaticConstruction.DESCRIPTION) + "\" />\n" + 
"        <event name=\"" + XmlEncode(Event.InstanceDestruction.NAME) + "\" type=\"nontriggering\" displayName=\"" + XmlEncode(Event.InstanceDestruction.DISPLAY) + "\" description=\"" + XmlEncode(Event.InstanceDestruction.DESCRIPTION) + "\">\n" + 
"            <eventParam name=\"" + XmlEncode(Event.InstanceDestruction.Params.ErrorCode.NAME) + "\" displayName=\"" + XmlEncode(Event.InstanceDestruction.Params.ErrorCode.DISPLAY) + "\" type=\"System.Int32\" guaranteed=\"true\" description=\"" + XmlEncode(Event.InstanceDestruction.Params.ErrorCode.DESCRIPTION) + "\" />\n" + 
"            <eventParam name=\"" + XmlEncode(Event.InstanceDestruction.Params.ErrorText.NAME) + "\" displayName=\"" + XmlEncode(Event.InstanceDestruction.Params.ErrorText.DISPLAY) + "\" type=\"System.String\" guaranteed=\"true\" description=\"" + XmlEncode(Event.InstanceDestruction.Params.ErrorText.DESCRIPTION) + "\" />\n" + 
"        </event>\n" + 
"    </eventList>\n" + 
"</package>\n";


    }
}
