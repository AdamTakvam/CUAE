<Application name="AuthorizeCall" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="AuthorizeCall">
    <outline>
      <treenode type="evh" id="632586707621026821" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632586707621026818" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632586707621026817" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632586707621026846" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632586707621026843" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632586707621026842" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632590852385728091" actid="632586707621026852" />
          <ref id="632590852385728107" actid="632586713909304097" />
          <ref id="632590852385728122" actid="632586713909304043" />
          <ref id="632590852385728133" actid="632586713909304069" />
          <ref id="632590852385728141" actid="632586713909304115" />
          <ref id="632590852385728150" actid="632586713909304125" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632586707621026851" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632586707621026848" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632586707621026847" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632590852385728092" actid="632586707621026852" />
          <ref id="632590852385728108" actid="632586713909304097" />
          <ref id="632590852385728123" actid="632586713909304043" />
          <ref id="632590852385728134" actid="632586713909304069" />
          <ref id="632590852385728142" actid="632586713909304115" />
          <ref id="632590852385728151" actid="632586713909304125" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632586713909304028" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632586713909304025" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632586713909304024" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632590852385728103" actid="632586713909304034" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632586713909304033" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632586713909304030" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632586713909304029" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632590852385728104" actid="632586713909304034" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632586713909304053" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632586713909304050" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632586713909304049" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632590852385728128" actid="632586713909304064" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632586713909304058" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632586713909304055" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632586713909304054" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632590852385728129" actid="632586713909304064" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632586713909304063" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632586713909304060" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632586713909304059" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632590852385728130" actid="632586713909304064" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callerIdentification" id="632590852385728069" vid="632586707621026822">
        <Properties type="String">g_callerIdentification</Properties>
      </treenode>
      <treenode text="g_dialedNumber" id="632590852385728071" vid="632586707621026824">
        <Properties type="String">g_dialedNumber</Properties>
      </treenode>
      <treenode text="g_callId1" id="632590852385728073" vid="632586707621026833">
        <Properties type="String">g_callId1</Properties>
      </treenode>
      <treenode text="g_connectionId1" id="632590852385728075" vid="632586707621026835">
        <Properties type="String">g_connectionId1</Properties>
      </treenode>
      <treenode text="g_authCode" id="632590852385728077" vid="632586713909304022">
        <Properties type="String" initWith="authCode">g_authCode</Properties>
      </treenode>
      <treenode text="g_callId2" id="632590852385728079" vid="632586713909304073">
        <Properties type="String" defaultInitWith="UNDEFINED">g_callId2</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632590852385728081" vid="632586713909304075">
        <Properties type="String">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_leadingDigits" id="632590852385728167" vid="632590852385728166">
        <Properties type="Int" initWith="leadingDigits">g_leadingDigits</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632586707621026820" treenode="632586707621026821" appnode="632586707621026818" handlerfor="632586707621026817">
    <node type="Start" id="632586707621026820" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="280">
      <linkto id="632586707621026826" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632586707621026826" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="168" y="280">
      <linkto id="632586707621026828" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
	public static string Execute(ref string g_dialedNumber, string dialedNumber, string from, ref string g_callerIdentification, int g_leadingDigits)
	{
		g_dialedNumber = dialedNumber.Substring(g_leadingDigits);
		g_callerIdentification = from;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632586707621026828" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="366" y="282">
      <linkto id="632586707621026837" type="Labeled" style="Bevel" label="Success" />
      <linkto id="632586707621026840" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <rd field="ConnectionId">g_connectionId1</rd>
      </Properties>
    </node>
    <node type="Action" id="632586707621026837" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="512" y="280">
      <linkto id="632586707621026852" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">callId</ap>
        <rd field="ResultData">g_callId1</rd>
      </Properties>
    </node>
    <node type="Comment" id="632586707621026838" text="Save the callerId ('from'), &#xD;&#xA;and the intended destination ('to'),&#xD;&#xA; for later usage" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="88" y="208" />
    <node type="Comment" id="632586707621026839" text="Answer the user's call,&#xD;&#xA;so that we can begin &#xD;&#xA;playing media to the call,&#xD;&#xA;directing user to enter &#xD;&#xA;authorization" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="296" y="176" />
    <node type="Action" id="632586707621026840" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="364" y="507">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632586707621026841" text="Failure to answer the call or&#xD;&#xA;to provide the user instruction&#xD;&#xA;are both errors we can do little&#xD;&#xA;about as an application.&#xD;&#xA;&#xD;&#xA;So, here we exit." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="419" y="436" />
    <node type="Action" id="632586707621026852" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="608" y="264" mx="661" my="280">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632586707621026846" />
        <item text="OnPlay_Failed" treenode="632586707621026851" />
      </items>
      <linkto id="632586713909303971" type="Labeled" style="Bevel" label="Success" />
      <linkto id="632586713909303972" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="literal">enter_pin.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId1</ap>
        <ap name="UserData" type="literal">enterpin</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909303971" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="817" y="282">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632586713909303972" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="655" y="400">
      <linkto id="632586713909303973" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909303973" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="656" y="512">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632586713909303974" text="Inform the user to &#xD;&#xA;enter his/her pin." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="609" y="208" />
    <node type="Variable" id="632586707621026830" name="dialedNumber" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.CallControl.IncomingCall">dialedNumber</Properties>
    </node>
    <node type="Variable" id="632586707621026831" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">from</Properties>
    </node>
    <node type="Variable" id="632586707621026832" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632586707621026845" treenode="632586707621026846" appnode="632586707621026843" handlerfor="632586707621026842">
    <node type="Start" id="632586707621026845" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="336">
      <linkto id="632586713909303976" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632586713909303976" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="168" y="336">
      <linkto id="632586713909304034" type="Labeled" style="Bevel" label="enterpin" />
      <linkto id="632586713909304107" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909304034" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="272" y="316" mx="346" my="332">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632586713909304028" />
        <item text="OnGatherDigits_Failed" treenode="632586713909304033" />
      </items>
      <linkto id="632586713909304037" type="Labeled" style="Bevel" label="Success" />
      <linkto id="632586713909304097" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId1</ap>
        <ap name="TermCondMaxDigits" type="csharp">g_authCode.Length</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909304037" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="519" y="334">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632586713909304097" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="288" y="488" mx="341" my="504">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632586707621026846" />
        <item text="OnPlay_Failed" treenode="632586707621026851" />
      </items>
      <linkto id="632586713909304098" type="Labeled" style="Bevel" label="default" />
      <linkto id="632586713909304099" type="Labeled" style="Bevel" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sys_unavailable_trylater.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId1</ap>
        <ap name="UserData" type="literal">error</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909304098" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="336" y="640">
      <linkto id="632586713909304100" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909304099" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="512" y="504">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632586713909304100" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="336" y="752">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632586713909304107" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="168" y="504">
      <linkto id="632586713909304114" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909304114" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="168" y="632">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632586713909303975" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632586707621026850" treenode="632586707621026851" appnode="632586707621026848" handlerfor="632586707621026847">
    <node type="Start" id="632586707621026850" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="320">
      <linkto id="632586713909304109" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632586713909304109" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="168" y="320">
      <linkto id="632586713909304113" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909304113" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="312" y="320">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" startnode="632586713909304027" treenode="632586713909304028" appnode="632586713909304025" handlerfor="632586713909304024">
    <node type="Start" id="632586713909304027" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="224">
      <linkto id="632586713909304040" type="Basic" style="Bevel" />
    </node>
    <node type="Comment" id="632586713909304038" text="check to see if the number entered matches the configured pin" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="168" />
    <node type="Action" id="632586713909304040" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="142" y="224">
      <linkto id="632586713909304043" type="Labeled" style="Bevel" label="unequal" />
      <linkto id="632586713909304068" type="Labeled" style="Bevel" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">digits</ap>
        <ap name="Value2" type="variable">g_authCode</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete: Comparing user input to configured pin...</log>
      </Properties>
    </node>
    <node type="Action" id="632586713909304043" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="88" y="352" mx="141" my="368">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632586707621026846" />
        <item text="OnPlay_Failed" treenode="632586707621026851" />
      </items>
      <linkto id="632586713909304046" type="Labeled" style="Bevel" label="default" />
      <linkto id="632586713909304047" type="Labeled" style="Bevel" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="literal">error_login.wav</ap>
        <ap name="Prompt2" type="literal">enter_pin.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId1</ap>
        <ap name="UserData" type="literal">enterpin</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909304046" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="136" y="512">
      <linkto id="632586713909304048" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909304047" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="264" y="368">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632586713909304048" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="136" y="624">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632586713909304064" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="576" y="208" mx="642" my="224">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632586713909304053" />
        <item text="OnMakeCall_Failed" treenode="632586713909304058" />
        <item text="OnRemoteHangup" treenode="632586713909304063" />
      </items>
      <linkto id="632586713909304069" type="Labeled" style="Bevel" label="default" />
      <linkto id="632586713909304077" type="Labeled" style="Bevel" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_dialedNumber</ap>
        <ap name="From" type="variable">g_callerIdentification</ap>
        <ap name="DisplayName" type="variable">g_callerIdentification</ap>
        <ap name="Conference" type="csharp">true</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_callId2</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"Calling " + g_dialedNumber</log>
      </Properties>
    </node>
    <node type="Action" id="632586713909304068" name="CreateConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="408" y="224">
      <linkto id="632586713909304064" type="Labeled" style="Bevel" label="Success" />
      <linkto id="632586713909304069" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId1</ap>
        <rd field="ConferenceId">g_conferenceId</rd>
      </Properties>
    </node>
    <node type="Action" id="632586713909304069" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="360" y="360" mx="413" my="376">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632586707621026846" />
        <item text="OnPlay_Failed" treenode="632586707621026851" />
      </items>
      <linkto id="632586713909304072" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sys_unavailable_trylater.wav</ap>
        <ap name="State" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId1</ap>
        <ap name="UserData" type="literal">error</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909304072" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="520" y="376">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632586713909304077" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="784" y="224">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632586713909304078" text="With our conference made,&#xD;&#xA;we attempt to call out to the &#xD;&#xA;intended destination." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="568" y="152" />
    <node type="Variable" id="632586713909304042" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">digits</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632586713909304032" treenode="632586713909304033" appnode="632586713909304030" handlerfor="632586713909304029">
    <node type="Start" id="632586713909304032" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="136" y="312">
      <linkto id="632586713909304115" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632586713909304115" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="232" y="296" mx="285" my="312">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632586707621026846" />
        <item text="OnPlay_Failed" treenode="632586707621026851" />
      </items>
      <linkto id="632586713909304116" type="Labeled" style="Bevel" label="default" />
      <linkto id="632586713909304117" type="Labeled" style="Bevel" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sys_unavailable_trylater.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId1</ap>
        <ap name="UserData" type="literal">error</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909304116" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="280" y="456">
      <linkto id="632586713909304118" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909304117" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="448" y="312">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632586713909304118" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="280" y="568">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632586713909304052" treenode="632586713909304053" appnode="632586713909304050" handlerfor="632586713909304049">
    <node type="Start" id="632586713909304052" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="328">
      <linkto id="632586713909304079" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632586713909304079" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="342" y="328">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632586713909304057" treenode="632586713909304058" appnode="632586713909304055" handlerfor="632586713909304054">
    <node type="Start" id="632586713909304057" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="144" y="248">
      <linkto id="632586713909304125" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632586713909304125" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="224" y="232" mx="277" my="248">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632586707621026846" />
        <item text="OnPlay_Failed" treenode="632586707621026851" />
      </items>
      <linkto id="632586713909304126" type="Labeled" style="Bevel" label="default" />
      <linkto id="632586713909304127" type="Labeled" style="Bevel" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sys_unavailable_trylater.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId1</ap>
        <ap name="UserData" type="literal">error</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909304126" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="272" y="392">
      <linkto id="632586713909304128" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909304127" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="440" y="248">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632586713909304128" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="272" y="504">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632586713909304062" treenode="632586713909304063" appnode="632586713909304060" handlerfor="632586713909304059">
    <node type="Start" id="632586713909304062" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="320">
      <linkto id="632586713909304086" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632586713909304086" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="160" y="320">
      <linkto id="632586713909304090" type="Labeled" style="Bevel" label="equal" />
      <linkto id="632586713909304092" type="Labeled" style="Bevel" label="unequal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_callId2</ap>
        <ap name="Value2" type="literal">UNDEFINED</ap>
      </Properties>
    </node>
    <node type="Comment" id="632586713909304088" text="The major consideration when a hangup comes in,&#xD;&#xA;is to determine which person hung up, and if the other&#xD;&#xA;person is still dialed in, to hang up the other user.&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="240" y="32" />
    <node type="Comment" id="632586713909304089" text="If callId2 is undefined,&#xD;&#xA;then we have not yet&#xD;&#xA;made the call out to the&#xD;&#xA;intended DN.  It is safe &#xD;&#xA;to assume then this is the&#xD;&#xA;caller is the user who&#xD;&#xA;initiated the application." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="88" y="184" />
    <node type="Action" id="632586713909304090" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="160" y="432">
      <linkto id="632586713909304096" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909304092" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="304" y="320">
      <linkto id="632586713909304093" type="Labeled" style="Bevel" label="equal" />
      <linkto id="632586713909304094" type="Labeled" style="Bevel" label="unequal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_callId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909304093" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="473" y="235">
      <linkto id="632586713909304095" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId2</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909304094" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="472" y="400">
      <linkto id="632586713909304095" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632586713909304095" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="656" y="328">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632586713909304096" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="160" y="560">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632586713909304087" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">callId</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>