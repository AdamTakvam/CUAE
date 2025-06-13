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
          <ref id="632767263316805015" actid="632575509038098292" />
          <ref id="632767263316805028" actid="632628402403078458" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575509038098286" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632575509038098283" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632575509038098282" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632767263316805016" actid="632575509038098292" />
          <ref id="632767263316805029" actid="632628402403078458" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575509038098291" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632575509038098288" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632575509038098287" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632767263316805017" actid="632575509038098292" />
          <ref id="632767263316805030" actid="632628402403078458" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575509038098309" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632575509038098306" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632575509038098305" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632767263316805020" actid="632575509038098646" />
          <ref id="632767263316805053" actid="632630728879074520" />
          <ref id="632767263316805069" actid="632575509038098315" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575509038098314" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632575509038098311" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632575509038098310" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632767263316805021" actid="632575509038098646" />
          <ref id="632767263316805054" actid="632630728879074520" />
          <ref id="632767263316805070" actid="632575509038098315" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632633443747024024" level="2" text="Metreos.CallControl.GotDigits: OnGotDigits">
        <node type="function" name="OnGotDigits" id="632633443747024021" path="Metreos.StockTools" />
        <node type="event" name="GotDigits" id="632633443747024020" path="Metreos.CallControl.GotDigits" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632633443747024029" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632633443747024026" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632633443747024025" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632633443747024034" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632633443747024031" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632633443747024030" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632633443747024039" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632633443747024036" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632633443747024035" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632633443747024044" level="2" text="Metreos.CallControl.CallChanged: OnCallChanged">
        <node type="function" name="OnCallChanged" id="632633443747024041" path="Metreos.StockTools" />
        <node type="event" name="CallChanged" id="632633443747024040" path="Metreos.CallControl.CallChanged" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_useP2P" id="632767263316804984" vid="632628402403078360">
        <Properties type="Bool" initWith="UseP2P">g_useP2P</Properties>
      </treenode>
      <treenode text="g_callId" id="632767263316804986" vid="632575509038098262">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_callId_callee" id="632767263316804988" vid="632575509038098636">
        <Properties type="String">g_callId_callee</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632767263316804990" vid="632575509038098264">
        <Properties type="String">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_connectionId_caller" id="632767263316804992" vid="632628402403078482">
        <Properties type="String">g_connectionId_caller</Properties>
      </treenode>
      <treenode text="g_numDigitsToStrip" id="632767263316804994" vid="632575509038098275">
        <Properties type="Int" defaultInitWith="0" initWith="NumDigitsToStrip">g_numDigitsToStrip</Properties>
      </treenode>
      <treenode text="g_numberHash" id="632767263316804996" vid="632575509038098266">
        <Properties type="Hashtable" initWith="SpeedDialMap">g_numberHash</Properties>
      </treenode>
      <treenode text="g_destinationNumber" id="632767263316804998" vid="632575509038098273">
        <Properties type="String">g_destinationNumber</Properties>
      </treenode>
      <treenode text="g_from" id="632767263316805000" vid="632628402403078470">
        <Properties type="String" defaultInitWith="UNKNOWN">g_from</Properties>
      </treenode>
      <treenode text="g_playPrompts" id="632767263316805002" vid="632630728879074512">
        <Properties type="Bool" defaultInitWith="true" initWith="PlayOnFail">g_playPrompts</Properties>
      </treenode>
      <treenode text="g_invalidAudio" id="632767263316805004" vid="632630728879073679">
        <Properties type="String" initWith="InvalidAudio">g_invalidAudio</Properties>
      </treenode>
      <treenode text="g_checkNumAudio" id="632767263316805006" vid="632630728879073681">
        <Properties type="String" initWith="CheckNumAudio">g_checkNumAudio</Properties>
      </treenode>
      <treenode text="g_callFailedAudio" id="632767263316805008" vid="632633428514348703">
        <Properties type="String" initWith="CallFailedAudio">g_callFailedAudio</Properties>
      </treenode>
      <treenode text="g_noResourcesAudio" id="632767263316805010" vid="632633428514348705">
        <Properties type="String" initWith="NoResourcesAudio">g_noResourcesAudio</Properties>
      </treenode>
      <treenode text="g_mmsId" id="632767263316805106" vid="632767263316805105">
        <Properties type="UInt">g_mmsId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632575509038098260" treenode="632575509038098261" appnode="632575509038098258" handlerfor="632575509038098257">
    <node type="Start" id="632575509038098260" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="187">
      <linkto id="632575509038098272" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575509038098271" name="AcceptCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="745" y="440">
      <linkto id="632575509038098292" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632628402403078472" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <rd field="CallId">g_callId</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: accepting call.</log>
        <log condition="default" on="true" level="Info" type="literal">OnIncomingCall: AcceptCall action took default path</log>
      </Properties>
    </node>
    <node type="Action" id="632575509038098272" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="143" y="186">
      <linkto id="632628402403078455" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632630728879074514" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string g_destinationNumber, ref string g_from, string from, string to, int g_numDigitsToStrip, Hashtable g_numberHash, LogWriter log)
{

	if (g_numberHash == null || g_numberHash.Count == 0)
	{
		log.Write(TraceLevel.Warning, "OnIncomingCall: the SpeedDial Map contains no entires. Please check the application configuration.");
		return IApp.VALUE_FAILURE;
	}

	if (from == null || from.Equals(string.Empty))
	{
		g_from = "UNAVAILABLE";
	}
	else
		g_from = from;

	try
	{
		g_destinationNumber = null;
		g_destinationNumber = Convert.ToString(g_numberHash[to.Substring(g_numDigitsToStrip)]);
	}
	catch {}


	if (g_destinationNumber == null || g_destinationNumber.Equals(string.Empty))
	{
		log.Write(TraceLevel.Warning, "OnIncomingCall: The dialed number (" + to + ") did not map to any entry in the SpeedDial Map. Please check the number dialed, followed by checking application settings.");
		return IApp.VALUE_FAILURE;
	}

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632575509038098292" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="859" y="424" mx="925" my="440">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632575509038098281" />
        <item text="OnMakeCall_Failed" treenode="632575509038098286" />
        <item text="OnRemoteHangup" treenode="632575509038098291" />
      </items>
      <linkto id="632575509038098298" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632630728879074518" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_destinationNumber</ap>
        <ap name="From" type="variable">g_from</ap>
        <ap name="DisplayName" type="variable">g_from</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="literal">0</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_callId_callee</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: placing outbound call to: " + g_destinationNumber</log>
      </Properties>
    </node>
    <node type="Action" id="632575509038098298" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1076" y="438">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575509038098646" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="320.000977" y="335" mx="373" my="351">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632575509038098309" />
        <item text="OnPlay_Failed" treenode="632575509038098314" />
      </items>
      <linkto id="632575509038098647" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632575509038098657" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_checkNumAudio</ap>
        <ap name="Prompt1" type="variable">g_invalidAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632575509038098647" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="369.352783" y="480">
      <linkto id="632575509038098648" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632575509038098648" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="255.3529" y="480">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575509038098654" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="253.79248" y="351">
      <linkto id="632575509038098646" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575509038098648" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DisplayName" type="literal">SpeedDial</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="ConnectionId">g_connectionId_caller</rd>
        <log condition="entry" on="true" level="Info" type="literal">MakeCall failed.</log>
      </Properties>
    </node>
    <node type="Action" id="632575509038098657" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="484" y="350">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632628402403078455" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="742" y="183">
      <linkto id="632575509038098271" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632628402403078458" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_useP2P</ap>
      </Properties>
    </node>
    <node type="Action" id="632628402403078458" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="856" y="168" mx="922" my="184">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632575509038098281" />
        <item text="OnMakeCall_Failed" treenode="632575509038098286" />
        <item text="OnRemoteHangup" treenode="632575509038098291" />
      </items>
      <linkto id="632628402403078464" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632630728879074517" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_destinationNumber</ap>
        <ap name="From" type="variable">g_from</ap>
        <ap name="DisplayName" type="variable">g_from</ap>
        <ap name="PeerCallId" type="variable">callId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_callId_callee</rd>
      </Properties>
    </node>
    <node type="Action" id="632628402403078464" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1058.41211" y="182">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632628402403078472" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="746.8828" y="587">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632630728879074514" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="143" y="351">
      <linkto id="632575509038098654" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632630728879074515" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_playPrompts</ap>
      </Properties>
    </node>
    <node type="Action" id="632630728879074515" name="RejectCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="143" y="481">
      <linkto id="632575509038098648" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Label" id="632630728879074516" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="60" y="351">
      <linkto id="632630728879074514" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632630728879074517" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="919.4707" y="333" />
    <node type="Label" id="632630728879074518" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="922.4707" y="592" />
    <node type="Comment" id="632633327127713515" text="The 'to' variable is being initialized with OriginalTo, since we're using JTAPI&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="67" y="547" />
    <node type="Variable" id="632575509038098268" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632575509038098269" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">from</Properties>
    </node>
    <node type="Variable" id="632575509038098270" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="OriginalTo" refType="reference" name="Metreos.CallControl.IncomingCall">to</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632575509038098280" treenode="632575509038098281" appnode="632575509038098278" handlerfor="632575509038098277">
    <node type="Start" id="632575509038098280" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="202">
      <linkto id="632628402403078466" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575509038098301" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="317" y="361">
      <linkto id="632575509038098302" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575509038098658" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
        <ap name="DisplayName" type="variable">to</ap>
        <ap name="MmsId" type="variable">mmsId</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">conferenceId</ap>
        <rd field="ConnectionId">g_connectionId_caller</rd>
        <rd field="ConferenceId">g_conferenceId</rd>
      </Properties>
    </node>
    <node type="Action" id="632575509038098302" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="483" y="359">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Complete: call successfully connected.</log>
      </Properties>
    </node>
    <node type="Action" id="632575509038098658" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="317.999878" y="488">
      <linkto id="632575509038098659" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId_callee</ap>
      </Properties>
    </node>
    <node type="Action" id="632575509038098659" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="491" y="486">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632628402403078466" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="121" y="202">
      <linkto id="632628402403078467" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632628402403078468" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_useP2P</ap>
      </Properties>
    </node>
    <node type="Action" id="632628402403078467" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="326" y="201">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632628402403078468" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="120.90625" y="362">
      <linkto id="632575509038098301" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632630728879074519" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="default" on="true" level="Warning" type="literal">OnMakeCall_Complete: Conference could not be created. </log>
public static string Execute(string conferenceId, ref string g_conferenceId, LogWriter log)
{
	if (conferenceId == null || conferenceId == string.Empty || conferenceId == "0")
	{
		log.Write(TraceLevel.Error, "OnMakeCall_Complete: Failed to create a conference.");
		return IApp.VALUE_FAILURE;
	}

	g_conferenceId = conferenceId;
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Comment" id="632628402403078476" text="The mmsId variable may cause trouble here. I seem to recall from RemoteAgent testing that it never gets populated&#xD;&#xA;and an error is thrown.Verify." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="160" y="40" />
    <node type="Action" id="632630728879074519" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="123" y="639">
      <linkto id="632630728879074523" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632630728879074529" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_playPrompts</ap>
      </Properties>
    </node>
    <node type="Action" id="632630728879074520" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="319" y="623" mx="372" my="639">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632575509038098309" />
        <item text="OnPlay_Failed" treenode="632575509038098314" />
      </items>
      <linkto id="632630728879074530" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632633327127713518" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="variable">g_noResourcesAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632630728879074523" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="243" y="639">
      <linkto id="632630728879074520" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632630728879074524" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
        <ap name="DisplayName" type="literal">SpeedDial</ap>
        <rd field="ConnectionId">g_connectionId_caller</rd>
      </Properties>
    </node>
    <node type="Action" id="632630728879074524" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="242.999878" y="783">
      <linkto id="632630991182304885" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId_callee</ap>
      </Properties>
    </node>
    <node type="Action" id="632630728879074526" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="606" y="637">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632630728879074529" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="122" y="783">
      <linkto id="632630728879074524" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632630728879074530" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="369" y="782">
      <linkto id="632630728879074524" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632630991182304885" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="243" y="887">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632633327127713518" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="489" y="638">
      <linkto id="632630728879074526" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId_callee</ap>
      </Properties>
    </node>
    <node type="Variable" id="632575509038098299" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConferenceId" defaultInitWith="0" refType="reference" name="Metreos.CallControl.MakeCall_Complete">conferenceId</Properties>
    </node>
    <node type="Variable" id="632575509038098303" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">callId</Properties>
    </node>
    <node type="Variable" id="632628402403078473" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" defaultInitWith="UKNOWN" refType="reference" name="Metreos.CallControl.MakeCall_Complete">to</Properties>
    </node>
    <node type="Variable" id="632628402403078475" name="mmsId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" initWith="MmsId" defaultInitWith="0" refType="reference" name="Metreos.CallControl.MakeCall_Complete">mmsId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632575509038098285" treenode="632575509038098286" appnode="632575509038098283" handlerfor="632575509038098282">
    <node type="Start" id="632575509038098285" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="235">
      <linkto id="632628402403078479" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575509038098304" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="347" y="362">
      <linkto id="632575509038098315" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575509038098643" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
        <ap name="DisplayName" type="literal">SpeedDial</ap>
        <rd field="ConnectionId">g_connectionId_caller</rd>
        <log condition="entry" on="true" level="Info" type="literal">MakeCall failed.</log>
      </Properties>
    </node>
    <node type="Action" id="632575509038098315" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="427" y="345" mx="480" my="361">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632575509038098309" />
        <item text="OnPlay_Failed" treenode="632575509038098314" />
      </items>
      <linkto id="632575509038098319" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575509038098642" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="variable">g_callFailedAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632575509038098319" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="612" y="360">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575509038098642" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="476.352844" y="517">
      <linkto id="632575509038098643" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632575509038098643" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="347.3529" y="517">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632628402403078479" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="213" y="234">
      <linkto id="632628402403078481" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632633327127713516" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_useP2P</ap>
      </Properties>
    </node>
    <node type="Action" id="632628402403078481" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="343" y="234">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632633327127713516" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="214" y="362">
      <linkto id="632575509038098304" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632633327127713517" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_playPrompts</ap>
      </Properties>
    </node>
    <node type="Action" id="632633327127713517" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="215" y="517">
      <linkto id="632575509038098643" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632575509038098290" treenode="632575509038098291" appnode="632575509038098288" handlerfor="632575509038098287">
    <node type="Start" id="632575509038098290" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="188">
      <linkto id="632628402403078477" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575509038098320" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="322" y="413">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575509038098638" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="180" y="312">
      <linkto id="632575509038098640" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632575509038098641" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632575509038098640" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="323" y="311">
      <linkto id="632575509038098320" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId_callee</ap>
      </Properties>
    </node>
    <node type="Action" id="632575509038098641" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="180" y="413">
      <linkto id="632575509038098320" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632628402403078477" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="180" y="188">
      <linkto id="632575509038098638" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632628402403078478" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_useP2P</ap>
      </Properties>
    </node>
    <node type="Action" id="632628402403078478" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="319" y="187">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632575509038098639" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632575509038098308" treenode="632575509038098309" appnode="632575509038098306" handlerfor="632575509038098305">
    <node type="Start" id="632575509038098308" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="280">
      <linkto id="632575509038098321" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575509038098321" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="142" y="280">
      <linkto id="632575509038098324" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632575509038098322" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632575509038098324" type="Labeled" style="Bezier" label="autostop" />
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
    <node type="Variable" id="632575509038098325" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Play_Complete">termCond</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632575509038098313" treenode="632575509038098314" appnode="632575509038098311" handlerfor="632575509038098310">
    <node type="Start" id="632575509038098313" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="47" y="262">
      <linkto id="632575509038098335" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575509038098328" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="243.674438" y="260">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575509038098335" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="138" y="261">
      <linkto id="632575509038098328" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotDigits" startnode="632633443747024023" treenode="632633443747024024" appnode="632633443747024021" handlerfor="632633443747024020">
    <node type="Start" id="632633443747024023" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="83">
      <linkto id="632633443747024045" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632633443747024045" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="144" y="84">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632633443747024028" treenode="632633443747024029" appnode="632633443747024026" handlerfor="632633443747024025">
    <node type="Start" id="632633443747024028" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632633443747024046" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632633443747024046" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="100" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632633443747024033" treenode="632633443747024034" appnode="632633443747024031" handlerfor="632633443747024030">
    <node type="Start" id="632633443747024033" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632633443747024047" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632633443747024047" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="103" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632633443747024038" treenode="632633443747024039" appnode="632633443747024036" handlerfor="632633443747024035">
    <node type="Start" id="632633443747024038" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632633443747024048" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632633443747024048" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="119" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnCallChanged" activetab="true" startnode="632633443747024043" treenode="632633443747024044" appnode="632633443747024041" handlerfor="632633443747024040">
    <node type="Start" id="632633443747024043" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632633443747024049" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632633443747024049" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="98" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>