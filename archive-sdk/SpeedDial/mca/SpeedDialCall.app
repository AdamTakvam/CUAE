<Application name="SpeedDialCall" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="SpeedDialCall">
    <outline>
      <treenode type="evh" id="632575509038098261" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632575509038098258" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632575509038098257" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
          <ep name="To" type="literal">regex:\*.+</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632575509038098281" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632575509038098278" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632575509038098277" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632725368693592335" actid="632575509038098292" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575509038098286" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632575509038098283" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632575509038098282" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632725368693592336" actid="632575509038098292" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575509038098291" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632575509038098288" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632575509038098287" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632725368693592337" actid="632575509038098292" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575509038098309" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632575509038098306" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632575509038098305" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632725368693592340" actid="632575509038098646" />
          <ref id="632725368693592366" actid="632575509038098315" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575509038098314" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632575509038098311" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632575509038098310" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632725368693592341" actid="632575509038098646" />
          <ref id="632725368693592367" actid="632575509038098315" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="632725368693592320" vid="632575509038098262">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632725368693592322" vid="632575509038098264">
        <Properties type="String">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_numberHash" id="632725368693592324" vid="632575509038098266">
        <Properties type="Hashtable" initWith="SpeedDialHash">g_numberHash</Properties>
      </treenode>
      <treenode text="g_destinationNumber" id="632725368693592326" vid="632575509038098273">
        <Properties type="String">g_destinationNumber</Properties>
      </treenode>
      <treenode text="g_numDigitsToStrip" id="632725368693592328" vid="632575509038098275">
        <Properties type="Int" initWith="NumDigitsToStrip">g_numDigitsToStrip</Properties>
      </treenode>
      <treenode text="g_callId_callee" id="632725368693592330" vid="632575509038098636">
        <Properties type="String">g_callId_callee</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632575509038098260" treenode="632575509038098261" appnode="632575509038098258" handlerfor="632575509038098257">
    <node type="Start" id="632575509038098260" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="310" y="327">
      <linkto id="632575509038098271" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575509038098271" name="AcceptCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="392" y="328">
      <linkto id="632575509038098272" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <rd field="CallId">g_callId</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: accepting call.</log>
        <log condition="default" on="true" level="Info" type="literal">OnIncomingCall: AcceptCall action took default path</log>
      </Properties>
    </node>
    <node type="Action" id="632575509038098272" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="511" y="329">
      <linkto id="632575509038098292" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632575509038098654" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string g_destinationNumber, ref string from, ref string to, int g_numDigitsToStrip, Hashtable g_numberHash)
{
	if (from == null || from.Equals(string.Empty))
	{
		from = "UNAVAILABLE";
	}

	try
	{
		g_destinationNumber = Convert.ToString(g_numberHash[to.Substring(g_numDigitsToStrip)]);
	}
	catch
	{
		return IApp.VALUE_FAILURE;
	}


	if (g_destinationNumber == null || g_destinationNumber.Equals(string.Empty))
		return IApp.VALUE_FAILURE;

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632575509038098292" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="595" y="313" mx="661" my="329">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632575509038098281" />
        <item text="OnMakeCall_Failed" treenode="632575509038098286" />
        <item text="OnRemoteHangup" treenode="632575509038098291" />
      </items>
      <linkto id="632575509038098298" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575509038098654" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_destinationNumber</ap>
        <ap name="From" type="variable">from</ap>
        <ap name="DisplayName" type="variable">from</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="literal">0</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_callId_callee</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: placing outbound call to: " + g_destinationNumber</log>
      </Properties>
    </node>
    <node type="Action" id="632575509038098298" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="796" y="329">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575509038098646" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="463" y="537" mx="516" my="553">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632575509038098309" />
        <item text="OnPlay_Failed" treenode="632575509038098314" />
      </items>
      <linkto id="632575509038098647" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632575509038098657" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="Prompt1" type="literal">error_invalid_number.wav</ap>
        <ap name="Prompt2" type="literal">try_again_later.wav</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632575509038098647" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="513.3528" y="692">
      <linkto id="632575509038098648" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632575509038098648" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="649.3529" y="691">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575509038098654" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="511.7923" y="454">
      <linkto id="632575509038098646" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
        <ap name="DisplayName" type="literal">SpeedDial</ap>
        <rd field="ConnectionId">connectionId</rd>
        <log condition="entry" on="true" level="Info" type="literal">MakeCall failed.</log>
      </Properties>
    </node>
    <node type="Action" id="632575509038098657" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="645" y="552">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632725041848452593" text="This script is triggered by an incoming call to a wildcard extension, such as *XXXX.&#xD;&#xA;The application will strip the prefix that was defined at the time of&#xD;&#xA;deployment, and map the remaining, numeric portion of the number to a&#xD;&#xA;corresponding entry in the speed dial table, which is also defined at the time of&#xD;&#xA;deployment. The retrieved numer is then dialed, and after the outbound call&#xD;&#xA;is connected, the inbound call is answered, and the two are connected.&#xD;&#xA;&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="32" />
    <node type="Comment" id="632725041848452594" text="1. We start by accepting the call&#xD;&#xA;using the CallControl AcceptCall&#xD;&#xA;action. This needs to be done&#xD;&#xA;because we won't answer the&#xD;&#xA;incoming call until the outbound&#xD;&#xA;call completes. Depending on the &#xD;&#xA;protocol used, and on the IP PBX&#xD;&#xA;settings, the inbound call will time&#xD;&#xA;out unless it is accepted or answered&#xD;&#xA;within a specific time interval." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="280" y="160" />
    <node type="Comment" id="632725041848452595" text="2. Next we use CustomCode to remove the&#xD;&#xA;dial prefix from the dialed number, and then&#xD;&#xA;we use the resulting numeric string to retrieve&#xD;&#xA;the number to dial fro the Speed Dial hashtable.&#xD;&#xA;If all the above code succeeds, we place a call&#xD;&#xA;to the retrieved number. Otherwise we answer&#xD;&#xA;the incoming call and play a failure message to the &#xD;&#xA;caller." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="532" y="190" />
    <node type="Comment" id="632725041848452596" text="3. We play a failure message to the&#xD;&#xA;caller here. If the action fails&#xD;&#xA;PROVISIONALLY (can't &#xD;&#xA;communicate with MMS, for example),&#xD;&#xA;we hang up the call right away and&#xD;&#xA;EndScript. Otherwise, we will&#xD;&#xA;terminate the script in the&#xD;&#xA;OnPlay_Complete handler.&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="256" y="565" />
    <node type="Variable" id="632575509038098268" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632575509038098269" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">from</Properties>
    </node>
    <node type="Variable" id="632575509038098270" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.CallControl.IncomingCall">to</Properties>
    </node>
    <node type="Variable" id="632575509038098656" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632575509038098280" treenode="632575509038098281" appnode="632575509038098278" handlerfor="632575509038098277">
    <node type="Start" id="632575509038098280" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="204">
      <linkto id="632575509038098300" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575509038098300" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="127" y="204">
      <linkto id="632575509038098301" type="Labeled" style="Bezier" ortho="true" label="unequal" />
      <linkto id="632575509038098660" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_conferenceId</ap>
        <ap name="Value2" type="literal">0</ap>
      </Properties>
    </node>
    <node type="Action" id="632575509038098301" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="243" y="204">
      <linkto id="632575509038098302" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575509038098658" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">conferenceId</ap>
        <rd field="ConferenceId">g_conferenceId</rd>
      </Properties>
    </node>
    <node type="Action" id="632575509038098302" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="372" y="203">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Complete: call successfully connected.</log>
      </Properties>
    </node>
    <node type="Action" id="632575509038098658" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="242" y="309">
      <linkto id="632575509038098659" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632575509038098659" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="369" y="310">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575509038098660" name="RejectCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="126" y="309">
      <linkto id="632575509038098658" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Comment" id="632725368693592389" text="Verify that a MMS conference has been created by verifying that the returned conferenceId is not 0, then answer the call.&#xD;&#xA;Otherwise, an error had occured, and the incoming call must be rejected." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="36" y="108" />
    <node type="Variable" id="632575509038098299" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConferenceId" defaultInitWith="0" refType="reference" name="Metreos.CallControl.MakeCall_Complete">conferenceId</Properties>
    </node>
    <node type="Variable" id="632575509038098303" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632575509038098285" treenode="632575509038098286" appnode="632575509038098283" handlerfor="632575509038098282">
    <node type="Start" id="632575509038098285" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="235">
      <linkto id="632575509038098304" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575509038098304" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="127" y="235">
      <linkto id="632575509038098315" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575509038098643" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
        <ap name="DisplayName" type="literal">SpeedDial</ap>
        <rd field="ConnectionId">connectionId</rd>
        <log condition="entry" on="true" level="Info" type="literal">MakeCall failed.</log>
      </Properties>
    </node>
    <node type="Action" id="632575509038098315" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="207" y="220" mx="260" my="236">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632575509038098309" />
        <item text="OnPlay_Failed" treenode="632575509038098314" />
      </items>
      <linkto id="632575509038098319" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575509038098642" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="Prompt1" type="literal">error_call.wav</ap>
        <ap name="Prompt2" type="literal">try_again_later.wav</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632575509038098319" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="549" y="238">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575509038098642" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="256.352844" y="389">
      <linkto id="632575509038098643" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632575509038098643" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="128.3529" y="389">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632725368693592388" text="If the outbound call failed, try to play error message before hanging up the caller." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="72" y="140" />
    <node type="Variable" id="632575509038098318" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632575509038098290" treenode="632575509038098291" appnode="632575509038098288" handlerfor="632575509038098287">
    <node type="Start" id="632575509038098290" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="188">
      <linkto id="632575509038098638" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575509038098320" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="270" y="291">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575509038098638" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="136" y="189">
      <linkto id="632575509038098640" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632575509038098641" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632575509038098640" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="269" y="190">
      <linkto id="632575509038098320" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId_callee</ap>
      </Properties>
    </node>
    <node type="Action" id="632575509038098641" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="136" y="290">
      <linkto id="632575509038098320" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Comment" id="632725368693592390" text="If caller hung up, hang up callee.&#xD;&#xA;Else, the hangup came from the callee, so hang up the caller." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="80" y="91" />
    <node type="Variable" id="632575509038098639" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632575509038098308" treenode="632575509038098309" appnode="632575509038098306" handlerfor="632575509038098305">
    <node type="Start" id="632575509038098308" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="280">
      <linkto id="632575509038098321" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575509038098321" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="142" y="280">
      <linkto id="632575509038098324" type="Labeled" style="Bezier" ortho="true" label="userstop" />
      <linkto id="632575509038098322" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632575509038098322" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="286" y="394">
      <linkto id="632575509038098323" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632575509038098323" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="396" y="395">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575509038098324" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="286" y="280">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632725041848452597" text="The Play action is used for only one purpose throghout this script: playing error messages before terminating the script.&#xD;&#xA;As soon as the Play operation completes, we want to end the script. The Play operation will complete succesfully whenever&#xD;&#xA;the MMS handling the operation encounters either a defined termination condition, end of data, or if the connection on &#xD;&#xA;which the operation is being performed is deleted. In the case that the connection was deleted before the Play operation&#xD;&#xA;completed, which would happen in the case that the caller hangs up during the announcement, the operation will complete&#xD;&#xA;with a terminating condition of 'userstop'. If this is the case here, we assume that a hangup occured, and simply end the function&#xD;&#xA;with the expectation that the termination of the call and script will be handled in the OnRemoteHangup event handler, which obviously&#xD;&#xA;hasn't executed yet at this point since the script is still running.&#xD;&#xA;&#xD;&#xA;If the Play terminated because of a reason other than a remote hangup, we hang up the caller and end the script here.&#xD;&#xA; &#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="81" y="79" />
    <node type="Variable" id="632575509038098325" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Play_Complete">termCond</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632575509038098313" treenode="632575509038098314" appnode="632575509038098311" handlerfor="632575509038098310">
    <node type="Start" id="632575509038098313" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="47" y="262">
      <linkto id="632575509038098335" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575509038098328" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="241.674438" y="264">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575509038098335" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="129" y="263">
      <linkto id="632575509038098328" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>