<Application name="script1" trigger="Metreos.Providers.JTapi.JTapiIncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632520303397145547" level="1" text="Metreos.Providers.JTapi.JTapiIncomingCall (trigger): OnJTapiIncomingCall">
        <node type="function" name="OnJTapiIncomingCall" id="632520303397145544" path="Metreos.StockTools" />
        <node type="event" name="JTapiIncomingCall" id="632520303397145543" path="Metreos.Providers.JTapi.JTapiIncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632826046959761263" level="2" text="Metreos.Providers.FunctionalTest.Event: OnBlind">
        <node type="function" name="OnBlind" id="632826046959761262" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632826046959761261" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.JTapiIncomingCallControlPanel.script1.E_Blind</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632826046959761267" level="2" text="Metreos.Providers.FunctionalTest.Event: OnHangup">
        <node type="function" name="OnHangup" id="632826046959761266" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632826046959761265" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.JTapiIncomingCallControlPanel.script1.E_Hangup</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632826046959761271" level="2" text="Metreos.Providers.FunctionalTest.Event: OnConference">
        <node type="function" name="OnConference" id="632826046959761270" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632826046959761269" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.JTapiIncomingCallControlPanel.script1.E_Conference</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632826054857105459" level="2" text="Metreos.Providers.JTapi.JTapiCallActive: OnJTapiCallActive">
        <node type="function" name="OnJTapiCallActive" id="632826054857105456" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallActive" id="632826054857105455" path="Metreos.Providers.JTapi.JTapiCallActive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632826054857105464" level="2" text="Metreos.Providers.JTapi.JTapiCallInactive: OnJTapiCallInactive">
        <node type="function" name="OnJTapiCallInactive" id="632826054857105461" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallInactive" id="632826054857105460" path="Metreos.Providers.JTapi.JTapiCallInactive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632826054857105469" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632826054857105466" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632826054857105465" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632826054857105485" level="2" text="Metreos.Providers.JTapi.JTapiHangup: OnJTapiHangup">
        <node type="function" name="OnJTapiHangup" id="632826054857105482" path="Metreos.StockTools" />
        <node type="event" name="JTapiHangup" id="632826054857105481" path="Metreos.Providers.JTapi.JTapiHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632826144270395438" level="2" text="Metreos.Providers.JTapi.JTapiGotDigits: OnJTapiGotDigits">
        <node type="function" name="OnJTapiGotDigits" id="632826144270395435" path="Metreos.StockTools" />
        <node type="event" name="JTapiGotDigits" id="632826144270395434" path="Metreos.Providers.JTapi.JTapiGotDigits" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632829412705063129" level="2" text="Metreos.Providers.FunctionalTest.Event: OnIncomingCallResponse">
        <node type="function" name="OnIncomingCallResponse" id="632829412705063126" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632829412705063125" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.JTapiIncomingCallControlPanel.script1.E_IncomingCall</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632832992816546025" level="2" text="Metreos.Providers.FunctionalTest.Event: OnUpdateGlobals">
        <node type="function" name="OnUpdateGlobals" id="632832992816546022" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632832992816546021" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.JTapiIncomingCallControlPanel.script1.E_UpdateGlobals</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_AnswerCallComplete" id="632833709696802835" vid="632826046959761252">
        <Properties type="String" initWith="S_AnswerCallComplete">S_AnswerCallComplete</Properties>
      </treenode>
      <treenode text="S_AnswerCallFail" id="632833709696802837" vid="632826046959761254">
        <Properties type="String" initWith="S_AnswerCallFail">S_AnswerCallFail</Properties>
      </treenode>
      <treenode text="S_Hangup" id="632833709696802841" vid="632826046959761258">
        <Properties type="String" initWith="S_Hangup">S_Hangup</Properties>
      </treenode>
      <treenode text="g_incomingCallId" id="632833709696802843" vid="632826047357105382">
        <Properties type="String">g_incomingCallId</Properties>
      </treenode>
      <treenode text="S_BlindSuccess" id="632833709696802845" vid="632826053401792502">
        <Properties type="String" initWith="S_BlindSuccess">S_BlindSuccess</Properties>
      </treenode>
      <treenode text="S_BlindFail" id="632833709696802847" vid="632826053401792504">
        <Properties type="String" initWith="S_BlindFail">S_BlindFail</Properties>
      </treenode>
      <treenode text="S_ConferenceSuccess" id="632833709696802849" vid="632826054722261252">
        <Properties type="String" initWith="S_ConferenceSuccess">S_ConferenceSuccess</Properties>
      </treenode>
      <treenode text="S_ConferenceFail" id="632833709696802851" vid="632826054722261254">
        <Properties type="String" initWith="S_ConferenceFail">S_ConferenceFail</Properties>
      </treenode>
      <treenode text="S_IncomingCall" id="632833709696802853" vid="632829412705063444">
        <Properties type="String" initWith="S_IncomingCall">S_IncomingCall</Properties>
      </treenode>
      <treenode text="g_CallHeld" id="632833709696802855" vid="632829554320480459">
        <Properties type="Bool" defaultInitWith="false">g_CallHeld</Properties>
      </treenode>
      <treenode text="g_TimerPeriod" id="632833709696802857" vid="632829554320480473">
        <Properties type="Int" defaultInitWith="0" initWith="ReversionTime">g_TimerPeriod</Properties>
      </treenode>
      <treenode text="g_TimerId" id="632833709696802859" vid="632829554320480475">
        <Properties type="String">g_TimerId</Properties>
      </treenode>
      <treenode text="g_PhoneIP" id="632833709696802861" vid="632829554320480477">
        <Properties type="String">g_PhoneIP</Properties>
      </treenode>
      <treenode text="g_version" id="632833709696802863" vid="632829554320480800">
        <Properties type="String" defaultInitWith="16.1">g_version</Properties>
      </treenode>
      <treenode text="S_CallActive" id="632833709696802865" vid="632832233378517335">
        <Properties type="String" initWith="S_CallActive">S_CallActive</Properties>
      </treenode>
      <treenode text="g_totalCalls" id="632833709696802867" vid="632832839349427065">
        <Properties type="Int" defaultInitWith="0">g_totalCalls</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnJTapiIncomingCall" startnode="632520303397145546" treenode="632520303397145547" appnode="632520303397145544" handlerfor="632520303397145543">
    <node type="Start" id="632520303397145546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="94">
      <linkto id="632829554320480489" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632826047357105384" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="399.0483" y="97">
      <linkto id="632829412705063340" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">callId</ap>
        <rd field="ResultData">g_incomingCallId</rd>
        <log condition="exit" on="true" level="Verbose" type="csharp">"DEBUG - exit: OnJtapiIn" + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632829412705063340" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="532" y="98">
      <linkto id="632829412705063341" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallID" type="variable">callId</ap>
        <ap name="signalName" type="variable">S_IncomingCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632829412705063341" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="675" y="98">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="csharp">"DEBUG: Version: " + g_version</log>
      </Properties>
    </node>
    <node type="Action" id="632829554320480489" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="131.1543" y="96">
      <linkto id="632829554320480490" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632829554320480491" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">deviceName</ap>
        <rd field="ResultData">dlxResult</rd>
      </Properties>
    </node>
    <node type="Action" id="632829554320480490" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="264.1543" y="97">
      <linkto id="632826047357105384" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632829554320480491" type="Labeled" style="Bezier" ortho="true" label="default" />
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
    <node type="Action" id="632829554320480491" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="187.1543" y="226">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="csharp">"Could not obtain IP address for device: " + deviceName</log>
      </Properties>
    </node>
    <node type="Comment" id="632830374197016735" text="Note: This signal sends a parameter along&#xD;&#xA;that is the caller ID to be used when creating&#xD;&#xA;a conference call later on." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="414" y="32" />
    <node type="Variable" id="632826047357105394" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.Providers.JTapi.JTapiIncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632829554320480798" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.JTapi.JTapiIncomingCall">deviceName</Properties>
    </node>
    <node type="Variable" id="632829554320480799" name="dlxResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">dlxResult</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnBlind" startnode="632826046959761264" treenode="632826046959761263" appnode="632826046959761262" handlerfor="632826046959761261">
    <node type="Start" id="632826046959761264" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="83" y="240">
      <linkto id="632826047357105395" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632826047357105395" name="JTapiBlindTransfer" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="230.064453" y="241">
      <linkto id="632826047357105396" type="Labeled" style="Bezier" ortho="true" label="Fail" />
      <linkto id="632826047357105397" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
        <ap name="To" type="variable">to</ap>
        <log condition="exit" on="true" level="Verbose" type="csharp">"DEBUG: JTapi Blindtransfer exit " + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632826047357105396" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="230.064453" y="370">
      <linkto id="632826047357105398" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_BlindFail</ap>
      </Properties>
    </node>
    <node type="Action" id="632826047357105397" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="446.064453" y="240">
      <linkto id="632826047357105398" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_BlindSuccess</ap>
      </Properties>
    </node>
    <node type="Action" id="632826047357105398" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="445.064453" y="368">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632826144270395676" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to" refType="reference">to</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup" startnode="632826046959761268" treenode="632826046959761267" appnode="632826046959761266" handlerfor="632826046959761265">
    <node type="Start" id="632826046959761268" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="62.9979553" y="215">
      <linkto id="632826054857105470" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632826054857105470" name="JTapiHangup" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="208.997955" y="216">
      <linkto id="632829525213831704" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632829525213831705" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632826054857105471" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="388.997955" y="217">
      <linkto id="632826054857105472" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Hangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632826054857105472" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="564" y="217">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632829525213831704" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="289.997955" y="103">
      <linkto id="632826054857105471" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"DEBUG: Success - JTapiHangup" + " - " + DateTime.Now</ap>
        <ap name="LogLevel" type="literal">Verbose</ap>
      </Properties>
    </node>
    <node type="Action" id="632829525213831705" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="287.997955" y="342">
      <linkto id="632826054857105471" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"DEBUG: Fail - JTapiHangup" + " - " + DateTime.Now</ap>
        <ap name="LogLevel" type="literal">Verbose</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnConference" startnode="632826046959761272" treenode="632826046959761271" appnode="632826046959761270" handlerfor="632826046959761269">
    <node type="Start" id="632826046959761272" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="52" y="109">
      <linkto id="632830374197016731" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632830374197016731" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="192" y="109">
      <linkto id="632830374197016736" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"DEBUG: MAX E_Conference CallId1: " + callId1 + ", CallId2: " + callId2 + " - " + DateTime.Now</ap>
        <ap name="LogLevel" type="literal">Verbose</ap>
      </Properties>
    </node>
    <node type="Action" id="632830374197016736" name="JTapiConference" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="333" y="111">
      <linkto id="632830374197016739" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632830374197016738" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
        <ap name="VolatileCallId" type="variable">callId2</ap>
      </Properties>
    </node>
    <node type="Action" id="632830374197016738" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="333.064453" y="241">
      <linkto id="632830374197016740" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_ConferenceFail</ap>
      </Properties>
    </node>
    <node type="Action" id="632830374197016739" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="549.064453" y="111">
      <linkto id="632830374197016740" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_ConferenceSuccess</ap>
      </Properties>
    </node>
    <node type="Action" id="632830374197016740" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="548.064453" y="239">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632826130220551684" name="callId2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId2" refType="reference">callId2</Properties>
    </node>
    <node type="Variable" id="632832992816545707" name="callId1" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId1" refType="reference">callId1</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallActive" startnode="632826054857105458" treenode="632826054857105459" appnode="632826054857105456" handlerfor="632826054857105455">
    <node type="Start" id="632826054857105458" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="157">
      <linkto id="632829554320480479" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632829554320480479" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="172.4707" y="159">
      <linkto id="632829554320480480" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632832839349427063" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_CallHeld</ap>
      </Properties>
    </node>
    <node type="Action" id="632829554320480480" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="258.4707" y="158">
      <linkto id="632829554320480482" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">g_TimerId</ap>
        <log condition="default" on="true" level="Verbose" type="csharp">"DEBUG: RemoveTimer"  + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632829554320480481" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="606.4707" y="250">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632829554320480482" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="382.4707" y="158">
      <linkto id="632832233378517475" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_CallHeld</rd>
      </Properties>
    </node>
    <node type="Comment" id="632829554320480647" text="Note: Since this app is designed to interact with FTF&#xD;&#xA;with an Incoming Call trigger, the call inactive hold/resume test&#xD;&#xA;has not been replicated for a JTapi outgoing call (see sdk\HoldReversion)" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="106" y="71" />
    <node type="Action" id="632832233378517475" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="494" y="156">
      <linkto id="632832233378517476" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallID" type="variable">callId</ap>
        <ap name="signalName" type="variable">S_CallActive</ap>
        <log condition="exit" on="true" level="Verbose" type="csharp">"DEBUG - MAX Sending S_CallActive exit" + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632832233378517476" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="609" y="156">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632832839349427061" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="382" y="252">
      <linkto id="632829554320480481" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallID" type="variable">callId</ap>
        <ap name="signalName" type="variable">S_CallActive</ap>
        <log condition="exit" on="true" level="Verbose" type="csharp">"DEBUG - MAX Sending S_CallActive exit" + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632832839349427063" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="172" y="254">
      <linkto id="632832839349427061" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"DEBUG - MAX sending call active. The current value of g_totalCalls is " + g_totalCalls + " - " + DateTime.Now</ap>
        <ap name="LogLevel" type="literal">Verbose</ap>
        <log condition="default" on="true" level="Verbose" type="csharp">"DEBUG - MAX sending call active 1" + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Variable" id="632832233378517477" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.Providers.JTapi.JTapiCallActive">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallInactive" startnode="632826054857105463" treenode="632826054857105464" appnode="632826054857105461" handlerfor="632826054857105460">
    <node type="Start" id="632826054857105463" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="151">
      <linkto id="632832992816545697" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632829554320480461" name="AddTriggerTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="430.4707" y="152">
      <linkto id="632829554320480462" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632832992816546038" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerDateTime" type="csharp">DateTime.Now.Add(new TimeSpan(0,0,g_TimerPeriod))</ap>
        <ap name="timerRecurrenceInterval" type="csharp">new TimeSpan(0,0,g_TimerPeriod)</ap>
        <ap name="timerUserData" type="variable">g_PhoneIP</ap>
        <rd field="timerId">g_TimerId</rd>
        <log condition="exit" on="true" level="Verbose" type="csharp">"DEBUG: AddTimer"  + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632829554320480462" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="556.4707" y="153">
      <linkto id="632832992816546040" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_CallHeld</rd>
      </Properties>
    </node>
    <node type="Action" id="632829554320480466" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="299.4699" y="154">
      <linkto id="632829554320480461" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632832992816546032" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_TimerPeriod == 0 ? "true" : "false";</ap>
        <log condition="default" on="true" level="Verbose" type="csharp">"DEBUG: CallInactive TimerPeriod: " + g_TimerPeriod</log>
      </Properties>
    </node>
    <node type="Comment" id="632830363015766751" text="Note: This will not add the timer &#xD;&#xA;unless the Config Value Reversion time is &gt; 0" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="203" y="77" />
    <node type="Action" id="632832992816545697" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="147.110657" y="152">
      <linkto id="632832992816545699" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632829554320480466" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_totalCalls == 1 ? "true" : "false";
</ap>
      </Properties>
    </node>
    <node type="Action" id="632832992816545699" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="146.110657" y="263">
      <linkto id="632832992816545703" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"DEBUG - MAX sending call Inactive. 2 Calls on the same line, bypassing Call Inactive test" + g_totalCalls + " - " + DateTime.Now</ap>
        <ap name="LogLevel" type="literal">Verbose</ap>
        <log condition="default" on="true" level="Verbose" type="csharp">"DEBUG - MAX sending call active 1" + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632832992816545703" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="145.48761" y="357">
      <linkto id="632832992816545704" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">g_TimerId</ap>
        <log condition="default" on="true" level="Verbose" type="csharp">"DEBUG: RemoveTimer"  + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632832992816545704" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="281.4876" y="358">
      <linkto id="632832992816546033" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_CallHeld</rd>
      </Properties>
    </node>
    <node type="Label" id="632832992816546032" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="300" y="243" />
    <node type="Label" id="632832992816546033" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="376" y="359" />
    <node type="Action" id="632832992816546035" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="662" y="344">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632832992816546036" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="662" y="286">
      <linkto id="632832992816546035" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632832992816546038" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="431" y="244" />
    <node type="Label" id="632832992816546040" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="654" y="153" />
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632826054857105468" treenode="632826054857105469" appnode="632826054857105466" handlerfor="632826054857105465">
    <node type="Start" id="632826054857105468" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="169">
      <linkto id="632826054857105491" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632826054857105491" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="144.610016" y="173">
      <linkto id="632826054857105492" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Hangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632826054857105492" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="290.610016" y="181">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="literal">OnRemoteHangup</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiHangup" startnode="632826054857105484" treenode="632826054857105485" appnode="632826054857105482" handlerfor="632826054857105481">
    <node type="Start" id="632826054857105484" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="109" y="242">
      <linkto id="632826054857105487" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632826054857105486" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="404.610016" y="249">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="csharp">"DEBUG: OnJTapiHangup END" + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632826054857105487" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="259.610016" y="247">
      <linkto id="632826054857105486" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Hangup</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiGotDigits" startnode="632826144270395437" treenode="632826144270395438" appnode="632826144270395435" handlerfor="632826144270395434">
    <node type="Start" id="632826144270395437" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="141">
      <linkto id="632826144270395439" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632826144270395439" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="222" y="141">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="csharp">"DEBUG -digit: " + digit</log>
      </Properties>
    </node>
    <node type="Variable" id="632826144270395440" name="digit" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" refType="reference" name="Metreos.Providers.JTapi.JTapiGotDigits">digit</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnIncomingCallResponse" startnode="632829412705063128" treenode="632829412705063129" appnode="632829412705063126" handlerfor="632829412705063125">
    <node type="Start" id="632829412705063128" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="274">
      <linkto id="632829412705063330" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632829412705063325" name="JTapiRejectCall" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="321.048828" y="381">
      <linkto id="632829412705063552" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
        <log condition="default" on="true" level="Verbose" type="csharp">"DEBUG Exit Action JTapiReject" + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632829412705063326" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="564.0488" y="213">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632829412705063330" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="145.5" y="275">
      <linkto id="632829412705063325" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632832954377657231" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">reject</ap>
      </Properties>
    </node>
    <node type="Action" id="632829412705063332" name="JTapiAnswerCall" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="317.133453" y="95">
      <linkto id="632829412705063335" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632829412705063333" type="Labeled" style="Bezier" ortho="true" label="Fail" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
        <log condition="default" on="true" level="Verbose" type="csharp">"DEBUG Exit Action JTapiAnswer g_incomingCallID " + g_incomingCallId</log>
      </Properties>
    </node>
    <node type="Action" id="632829412705063333" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="316.133484" y="212">
      <linkto id="632829412705063326" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_AnswerCallFail</ap>
        <log condition="exit" on="true" level="Error" type="csharp">"DEBUG - JTapiAnswerCall Failed" + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632829412705063335" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="565.1334" y="94">
      <linkto id="632829412705063326" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_AnswerCallComplete</ap>
        <log condition="exit" on="true" level="Verbose" type="csharp">"DEBUG - MAX: JTapi Answer Call Complete"</log>
      </Properties>
    </node>
    <node type="Action" id="632829412705063552" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="571" y="384">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632832954377657231" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="146" y="163">
      <linkto id="632829412705063332" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">totalCalls</ap>
        <rd field="ResultData">g_totalCalls</rd>
        <log condition="exit" on="true" level="Verbose" type="csharp">"DEBUG - Assign IncomingCallResponse exit: g_totalCalls: " + g_totalCalls + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Variable" id="632829412705063551" name="reject" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="reject" refType="reference">reject</Properties>
    </node>
    <node type="Variable" id="632832954377657230" name="totalCalls" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="totalCalls" defaultInitWith="0" refType="reference">totalCalls</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnUpdateGlobals" startnode="632832992816546024" treenode="632832992816546025" appnode="632832992816546022" handlerfor="632832992816546021">
    <node type="Start" id="632832992816546024" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="150">
      <linkto id="632832992816546028" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632832992816546026" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="322" y="155">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="csharp">"DEBUG - exit: OnUpdateGlobals - g_totalCalls: " + g_totalCalls + " - " + DateTime.Now</log>
      </Properties>
    </node>
    <node type="Action" id="632832992816546028" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="161" y="154">
      <linkto id="632832992816546026" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">totalCalls</ap>
        <rd field="ResultData">g_totalCalls</rd>
      </Properties>
    </node>
    <node type="Variable" id="632832992816546027" name="totalCalls" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="totalCalls" refType="reference">totalCalls</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>