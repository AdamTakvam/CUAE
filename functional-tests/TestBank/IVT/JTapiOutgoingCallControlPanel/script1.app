<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632520303397145547" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632520303397145544" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632520303397145543" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">IVT.JTapiOutgoingCallControlPanel.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632833983648232965" level="2" text="Metreos.Providers.FunctionalTest.Event: OnHangup">
        <node type="function" name="OnHangup" id="632833983648232964" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632833983648232963" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.JTapiOutgoingCallControlPanel.script1.E_Hangup</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632833984007878826" level="2" text="Metreos.Providers.JTapi.JTapiGotDigits: OnJTapiGotDigits">
        <node type="function" name="OnJTapiGotDigits" id="632833984007878823" path="Metreos.StockTools" />
        <node type="event" name="JTapiGotDigits" id="632833984007878822" path="Metreos.Providers.JTapi.JTapiGotDigits" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632833984007878834" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632833984007878831" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632833984007878830" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632833984007878883" level="2" text="Metreos.Providers.JTapi.JTapiHangup: OnJTapiHangup">
        <node type="function" name="OnJTapiHangup" id="632833984007878880" path="Metreos.StockTools" />
        <node type="event" name="JTapiHangup" id="632833984007878879" path="Metreos.Providers.JTapi.JTapiHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632833984007878945" level="2" text="Metreos.Providers.FunctionalTest.Event: OnBlind">
        <node type="function" name="OnBlind" id="632833984007878942" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632833984007878941" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.JTapiOutgoingCallControlPanel.script1.E_Blind</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632835430197446047" level="2" text="Metreos.Providers.FunctionalTest.Event: OnConference">
        <node type="function" name="OnConference" id="632835430197446044" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632835430197446043" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.JTapiOutgoingCallControlPanel.script1.E_Conference</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632835430197446142" level="2" text="Metreos.Providers.JTapi.JTapiCallActive: OnJTapiCallActive">
        <node type="function" name="OnJTapiCallActive" id="632835430197446139" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallActive" id="632835430197446138" path="Metreos.Providers.JTapi.JTapiCallActive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632835430197446147" level="2" text="Metreos.Providers.JTapi.JTapiCallInactive: OnJTapiCallInactive">
        <node type="function" name="OnJTapiCallInactive" id="632835430197446144" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallInactive" id="632835430197446143" path="Metreos.Providers.JTapi.JTapiCallInactive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632835430197446152" level="2" text="Metreos.Providers.FunctionalTest.Event: OnOutgoingCallResponse">
        <node type="function" name="OnOutgoingCallResponse" id="632835430197446149" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632835430197446148" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.JTapiOutgoingCallControlPanel.script1.E_OutgoingCall</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632835430197446264" level="2" text="Metreos.Providers.FunctionalTest.Event: OnUpdateGlobals">
        <node type="function" name="OnUpdateGlobals" id="632835430197446261" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632835430197446260" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.JTapiOutgoingCallControlPanel.script1.E_UpdateGlobals</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_MakeCallComplete" id="632859835544421735" vid="632833983648232962">
        <Properties type="String" initWith="S_MakeCallComplete">S_MakeCallComplete</Properties>
      </treenode>
      <treenode text="g_OutgoingCallId" id="632859835544421737" vid="632833984007878779">
        <Properties type="String">g_OutgoingCallId</Properties>
      </treenode>
      <treenode text="S_Hangup" id="632859835544421739" vid="632833984007878873">
        <Properties type="String" initWith="S_Hangup">S_Hangup</Properties>
      </treenode>
      <treenode text="S_BlindSuccess" id="632859835544421741" vid="632833984007878937">
        <Properties type="String" initWith="S_BlindSuccess">S_BlindSuccess</Properties>
      </treenode>
      <treenode text="S_BlindFail" id="632859835544421743" vid="632833984007878939">
        <Properties type="String" initWith="S_BlindFail">S_BlindFail</Properties>
      </treenode>
      <treenode text="S_Conference" id="632859835544421745" vid="632835430197445956">
        <Properties type="String" initWith="S_Conference">S_Conference</Properties>
      </treenode>
      <treenode text="S_MakeCallFail" id="632859835544421747" vid="632835430197446023">
        <Properties type="String" initWith="S_MakeCallFail">S_MakeCallFail</Properties>
      </treenode>
      <treenode text="S_ConferenceSuccess" id="632859835544421749" vid="632835430197446025">
        <Properties type="String" initWith="S_ConferenceSuccess">S_ConferenceSuccess</Properties>
      </treenode>
      <treenode text="S_ConferenceFail" id="632859835544421751" vid="632835430197446027">
        <Properties type="String" initWith="S_ConferenceFail">S_ConferenceFail</Properties>
      </treenode>
      <treenode text="S_CallActive" id="632859835544421753" vid="632835430197446029">
        <Properties type="String" initWith="S_CallActive">S_CallActive</Properties>
      </treenode>
      <treenode text="g_TimerPeriod" id="632859835544421755" vid="632835430197446031">
        <Properties type="Int" defaultInitWith="0" initWith="ReversionTime">g_TimerPeriod</Properties>
      </treenode>
      <treenode text="g_TimerId" id="632859835544421757" vid="632835430197446033">
        <Properties type="String">g_TimerId</Properties>
      </treenode>
      <treenode text="g_PhoneIP" id="632859835544421759" vid="632835430197446035">
        <Properties type="String">g_PhoneIP</Properties>
      </treenode>
      <treenode text="g_version" id="632859835544421761" vid="632835430197446037">
        <Properties type="String">g_version</Properties>
      </treenode>
      <treenode text="g_totalCalls" id="632859835544421763" vid="632835430197446039">
        <Properties type="Int" defaultInitWith="1">g_totalCalls</Properties>
      </treenode>
      <treenode text="g_CallHeld" id="632859835544421765" vid="632835430197446041">
        <Properties type="Bool" defaultInitWith="false">g_CallHeld</Properties>
      </treenode>
      <treenode text="S_OutgoingCall" id="632859835544421767" vid="632835430197446258">
        <Properties type="String" initWith="S_OutGoingCall">S_OutgoingCall</Properties>
      </treenode>
      <treenode text="g_OutgoingCallId2" id="632859835544421769" vid="632835430197446502">
        <Properties type="String">g_OutgoingCallId2</Properties>
      </treenode>
      <treenode text="g_devicename" id="632859835544421849" vid="632859835544421848">
        <Properties type="String">g_devicename</Properties>
      </treenode>
      <treenode text="g_from" id="632859835544421852" vid="632859835544421851">
        <Properties type="String">g_from</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632520303397145546" treenode="632520303397145547" appnode="632520303397145544" handlerfor="632520303397145543">
    <node type="Start" id="632520303397145546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="76">
      <linkto id="632859835544421850" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632833984007878781" name="JTapiMakeCall" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="442.205719" y="152">
      <linkto id="632833984007878783" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632833984007878782" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="From" type="variable">from</ap>
        <ap name="To" type="variable">to</ap>
        <ap name="DeviceName" type="variable">deviceName</ap>
        <rd field="CallId">g_OutgoingCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632833984007878782" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="591.2057" y="152">
      <linkto id="632833984007878784" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="callId" type="variable">g_OutgoingCallId</ap>
        <ap name="signalName" type="variable">S_MakeCallComplete</ap>
      </Properties>
    </node>
    <node type="Action" id="632833984007878783" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="441.20575" y="378">
      <linkto id="632833984007878784" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_MakeCallComplete</ap>
      </Properties>
    </node>
    <node type="Action" id="632833984007878784" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="589.2057" y="378">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632835430197446480" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="135.1543" y="150">
      <linkto id="632835430197446481" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632835430197446482" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">deviceName</ap>
        <rd field="ResultData">dlxResult</rd>
      </Properties>
    </node>
    <node type="Action" id="632835430197446481" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="272.4629" y="150">
      <linkto id="632835430197446482" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632833984007878781" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties language="csharp">
public static string Execute(DataTable dlxResult, ref string g_PhoneIP)
{
    g_PhoneIP = String.Empty;

    if(dlxResult == null || dlxResult.Rows == null || dlxResult.Rows.Count != 1)
        return IApp.VALUE_FAILURE;

    g_PhoneIP = dlxResult.Rows[0][ICiscoDeviceList.FIELD_IP] as string;

    if(g_PhoneIP == null)
        g_PhoneIP = String.Empty;
    if(g_PhoneIP == String.Empty)
        return IApp.VALUE_FAILURE;
    
    return IApp.VALUE_SUCCESS;
}</Properties>
    </node>
    <node type="Action" id="632835430197446482" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="200.46286" y="271">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="csharp">"Could not obtain IP address for device: " + deviceName</log>
      </Properties>
    </node>
    <node type="Comment" id="632835430197446488" text="Note: This signal sends a parameter along&#xD;&#xA;that is the caller ID to be used when creating&#xD;&#xA;a conference call later on." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="480.000519" y="60" />
    <node type="Action" id="632859835544421850" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="135" y="55">
      <linkto id="632835430197446480" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">deviceName</ap>
        <ap name="Value2" type="variable">from</ap>
        <rd field="ResultData">g_devicename</rd>
        <rd field="ResultData2">g_from</rd>
      </Properties>
    </node>
    <node type="Variable" id="632833984007878789" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632833984007878790" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="devicename" refType="reference">deviceName</Properties>
    </node>
    <node type="Variable" id="632833984007878791" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="from" refType="reference">from</Properties>
    </node>
    <node type="Variable" id="632835430197446690" name="dlxResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">dlxResult</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup" startnode="632833983648232966" treenode="632833983648232965" appnode="632833983648232964" handlerfor="632833983648232963">
    <node type="Start" id="632833983648232966" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="145">
      <linkto id="632833984007878792" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632833984007878792" name="JTapiHangup" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="162.694" y="144">
      <linkto id="632833984007878793" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_OutgoingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632833984007878793" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="354.694" y="144">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiGotDigits" startnode="632833984007878825" treenode="632833984007878826" appnode="632833984007878823" handlerfor="632833984007878822">
    <node type="Start" id="632833984007878825" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="130">
      <linkto id="632833984007878828" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632833984007878828" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="163.4707" y="130">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="csharp">"DEBUG -digit: " + digit</log>
      </Properties>
    </node>
    <node type="Variable" id="632833984007878827" name="digit" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" refType="reference" name="Metreos.Providers.JTapi.JTapiGotDigits">digit</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632833984007878833" treenode="632833984007878834" appnode="632833984007878831" handlerfor="632833984007878830">
    <node type="Start" id="632833984007878833" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="127">
      <linkto id="632833984007878875" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632833984007878875" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="129.610016" y="129">
      <linkto id="632833984007878876" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Hangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632833984007878876" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="271.610016" y="131">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="literal">OnRemoteHangup</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiHangup" startnode="632833984007878882" treenode="632833984007878883" appnode="632833984007878880" handlerfor="632833984007878879">
    <node type="Start" id="632833984007878882" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="105">
      <linkto id="632833984007878885" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632833984007878884" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="297.610016" y="106">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="csharp">"DEBUG: OnJTapiHangup END" + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632833984007878885" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="152.610016" y="104">
      <linkto id="632833984007878884" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Hangup</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnBlind" startnode="632833984007878944" treenode="632833984007878945" appnode="632833984007878942" handlerfor="632833984007878941">
    <node type="Start" id="632833984007878944" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="111">
      <linkto id="632833984007879004" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632833984007879004" name="JTapiBlindTransfer" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="176.064453" y="113">
      <linkto id="632833984007879005" type="Labeled" style="Bezier" ortho="true" label="Fail" />
      <linkto id="632833984007879006" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_OutgoingCallId</ap>
        <ap name="To" type="variable">to</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"** DEBUG: OnBlind - Entry " + DateTime.Now
</log>
        <log condition="exit" on="true" level="Verbose" type="csharp">"DEBUG: JTapi Blindtransfer exit " + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632833984007879005" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="176.064453" y="242">
      <linkto id="632833984007879007" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_BlindFail</ap>
      </Properties>
    </node>
    <node type="Action" id="632833984007879006" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="392.064453" y="112">
      <linkto id="632833984007879007" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_BlindSuccess</ap>
      </Properties>
    </node>
    <node type="Action" id="632833984007879007" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="393.064453" y="243">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632833984007879012" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to" refType="reference">to</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnConference" activetab="true" startnode="632835430197446046" treenode="632835430197446047" appnode="632835430197446044" handlerfor="632835430197446043">
    <node type="Start" id="632835430197446046" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="97">
      <linkto id="632835430197446505" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632835430197446267" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="356" y="232">
      <linkto id="632835430197446268" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"DEBUG: MAX E_Conference CallId1: " + g_OutgoingCallId + ", CallId2: " + g_OutgoingCallId2 + " - " + DateTime.Now</ap>
        <ap name="LogLevel" type="literal">Verbose</ap>
      </Properties>
    </node>
    <node type="Action" id="632835430197446268" name="JTapiConference" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="497" y="234">
      <linkto id="632835430197446270" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632835430197446269" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_OutgoingCallId</ap>
        <ap name="VolatileCallId" type="variable">g_OutgoingCallId2</ap>
      </Properties>
    </node>
    <node type="Action" id="632835430197446269" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="497.064453" y="364">
      <linkto id="632835430197446271" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_ConferenceFail</ap>
      </Properties>
    </node>
    <node type="Action" id="632835430197446270" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="713.064453" y="234">
      <linkto id="632835430197446271" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_ConferenceSuccess</ap>
      </Properties>
    </node>
    <node type="Action" id="632835430197446271" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="712.064453" y="362">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632835430197446494" name="JTapiMakeCall" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="107.205711" y="234">
      <linkto id="632835430197446495" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632835430197446507" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="From" type="variable">g_from</ap>
        <ap name="To" type="variable">with</ap>
        <ap name="DeviceName" type="variable">g_devicename</ap>
        <rd field="CallId">g_OutgoingCallId2</rd>
      </Properties>
    </node>
    <node type="Action" id="632835430197446495" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="256.2057" y="233">
      <linkto id="632835430197446267" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="callId" type="variable">g_OutgoingCallId</ap>
        <ap name="signalName" type="variable">S_MakeCallComplete</ap>
      </Properties>
    </node>
    <node type="Action" id="632835430197446496" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="106.205742" y="459">
      <linkto id="632835430197446497" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_MakeCallComplete</ap>
      </Properties>
    </node>
    <node type="Action" id="632835430197446497" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="254.205688" y="459">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632835430197446504" text="Make a second call and then conference the first and second call together" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="164" y="178" />
    <node type="Action" id="632835430197446505" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="107" y="96">
      <linkto id="632835430197446494" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">2</ap>
        <rd field="ResultData">g_totalCalls</rd>
      </Properties>
    </node>
    <node type="Action" id="632835430197446507" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="107" y="356">
      <linkto id="632835430197446496" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">1</ap>
        <rd field="ResultData">g_totalCalls</rd>
      </Properties>
    </node>
    <node type="Variable" id="632835430197446688" name="with" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="with" refType="reference">with</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallActive" startnode="632835430197446141" treenode="632835430197446142" appnode="632835430197446139" handlerfor="632835430197446138">
    <node type="Start" id="632835430197446141" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="39" y="166">
      <linkto id="632835430197446420" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632835430197446420" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="144.4707" y="166">
      <linkto id="632835430197446421" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632835430197446428" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_CallHeld</ap>
      </Properties>
    </node>
    <node type="Action" id="632835430197446421" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="230.4707" y="165">
      <linkto id="632835430197446423" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">g_TimerId</ap>
        <log condition="default" on="true" level="Verbose" type="csharp">"DEBUG: RemoveTimer"  + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632835430197446422" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="578.4707" y="257">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632835430197446423" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="354.4707" y="165">
      <linkto id="632835430197446425" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_CallHeld</rd>
      </Properties>
    </node>
    <node type="Comment" id="632835430197446424" text="Note: Since this app is designed to interact with FTF&#xD;&#xA;with an Incoming Call trigger, the call inactive hold/resume test&#xD;&#xA;has not been replicated for a JTapi outgoing call (see sdk\HoldReversion)" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="78" y="78" />
    <node type="Action" id="632835430197446425" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="466" y="163">
      <linkto id="632835430197446426" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallID" type="variable">callId</ap>
        <ap name="signalName" type="variable">S_CallActive</ap>
        <log condition="exit" on="true" level="Verbose" type="csharp">"DEBUG - MAX Sending S_CallActive exit" + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632835430197446426" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="581" y="163">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632835430197446427" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="354" y="259">
      <linkto id="632835430197446422" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallID" type="variable">callId</ap>
        <ap name="signalName" type="variable">S_CallActive</ap>
        <log condition="exit" on="true" level="Verbose" type="csharp">"DEBUG - MAX Sending S_CallActive exit" + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632835430197446428" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="144" y="261">
      <linkto id="632835430197446427" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"DEBUG - MAX sending call active. The current value of g_totalCalls is " + g_totalCalls + " - " + DateTime.Now</ap>
        <ap name="LogLevel" type="literal">Verbose</ap>
        <log condition="default" on="true" level="Verbose" type="csharp">"DEBUG - MAX sending call active 1" + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Variable" id="632835430197446466" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.Providers.JTapi.JTapiCallActive">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallInactive" startnode="632835430197446146" treenode="632835430197446147" appnode="632835430197446144" handlerfor="632835430197446143">
    <node type="Start" id="632835430197446146" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="139">
      <linkto id="632835430197446442" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632835430197446438" name="AddTriggerTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="418.880859" y="139">
      <linkto id="632835430197446439" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632835430197446450" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerDateTime" type="csharp">DateTime.Now.Add(new TimeSpan(0,0,g_TimerPeriod))</ap>
        <ap name="timerRecurrenceInterval" type="csharp">new TimeSpan(0,0,g_TimerPeriod)</ap>
        <ap name="timerUserData" type="variable">g_PhoneIP</ap>
        <rd field="timerId">g_TimerId</rd>
        <log condition="exit" on="true" level="Verbose" type="csharp">"DEBUG: AddTimer"  + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632835430197446439" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="544.880859" y="140">
      <linkto id="632835430197446451" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_CallHeld</rd>
      </Properties>
    </node>
    <node type="Action" id="632835430197446440" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="287.880066" y="141">
      <linkto id="632835430197446438" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632835430197446446" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_TimerPeriod == 0 ? "true" : "false";</ap>
        <log condition="default" on="true" level="Verbose" type="csharp">"DEBUG: CallInactive TimerPeriod: " + g_TimerPeriod</log>
      </Properties>
    </node>
    <node type="Comment" id="632835430197446441" text="Note: This will not add the timer &#xD;&#xA;unless the Config Value Reversion time is &gt; 0" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="191.410172" y="64" />
    <node type="Action" id="632835430197446442" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="135.520828" y="139">
      <linkto id="632835430197446443" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632835430197446440" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_totalCalls == 1 ? "true" : "false";
</ap>
      </Properties>
    </node>
    <node type="Action" id="632835430197446443" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="134.520828" y="250">
      <linkto id="632835430197446444" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"DEBUG - MAX sending call Inactive. 2 Calls on the same line, bypassing Call Inactive test" + g_totalCalls + " - " + DateTime.Now</ap>
        <ap name="LogLevel" type="literal">Verbose</ap>
        <log condition="default" on="true" level="Verbose" type="csharp">"DEBUG - MAX sending call active 1" + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632835430197446444" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="133.897781" y="344">
      <linkto id="632835430197446445" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">g_TimerId</ap>
        <log condition="default" on="true" level="Verbose" type="csharp">"DEBUG: RemoveTimer"  + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632835430197446445" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="269.897766" y="345">
      <linkto id="632835430197446447" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_CallHeld</rd>
      </Properties>
    </node>
    <node type="Label" id="632835430197446446" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="288.410156" y="230" />
    <node type="Label" id="632835430197446447" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="364.410156" y="346" />
    <node type="Action" id="632835430197446448" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="650.410156" y="331">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632835430197446449" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="650.410156" y="273">
      <linkto id="632835430197446448" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632835430197446450" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="419.410156" y="231" />
    <node type="Label" id="632835430197446451" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="642.410156" y="140" />
  </canvas>
  <canvas type="Function" name="OnOutgoingCallResponse" startnode="632835430197446151" treenode="632835430197446152" appnode="632835430197446149" handlerfor="632835430197446148">
    <node type="Start" id="632835430197446151" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="154">
      <linkto id="632835430197446468" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632835430197446468" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="183.409485" y="154">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632835430197446467" name="totalCalls" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="totalCalls" defaultInitWith="0" refType="reference">totalCalls</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnUpdateGlobals" startnode="632835430197446263" treenode="632835430197446264" appnode="632835430197446261" handlerfor="632835430197446260">
    <node type="Start" id="632835430197446263" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="138">
      <linkto id="632835430197446509" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632835430197446509" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="263" y="143">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>