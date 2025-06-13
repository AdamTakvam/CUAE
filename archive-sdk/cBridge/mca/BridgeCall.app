<Application name="BridgeCall" trigger="Metreos.Providers.H323.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="BridgeCall">
    <outline>
      <treenode type="evh" id="632398268734829107" level="1" text="Metreos.Providers.H323.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632398268734829104" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632398268734829103" path="Metreos.Providers.H323.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632398435435766514" level="2" text="Metreos.Providers.H323.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632398435435766511" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632398435435766510" path="Metreos.Providers.H323.MakeCall_Complete" />
        <references>
          <ref id="632405982989929572" actid="632398435435766525" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632398435435766519" level="2" text="Metreos.Providers.H323.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632398435435766516" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632398435435766515" path="Metreos.Providers.H323.MakeCall_Failed" />
        <references>
          <ref id="632405982989929573" actid="632398435435766525" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632398435435766524" level="2" text="Metreos.Providers.H323.Hangup: OnHangup">
        <node type="function" name="OnHangup" id="632398435435766521" path="Metreos.StockTools" />
        <node type="event" name="Hangup" id="632398435435766520" path="Metreos.Providers.H323.Hangup" />
        <references>
          <ref id="632405982989929574" actid="632398435435766525" />
          <ref id="632405982989929588" actid="632399642734546690" />
          <ref id="632405982989929606" actid="632398435435766547" />
          <ref id="632405982989929638" actid="632405492896871841" />
          <ref id="632405982989929729" actid="632399556409545356" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632398435435766541" level="2" text="Metreos.Providers.H323.AnswerCall_Complete: OnAnswerCall_Complete">
        <node type="function" name="OnAnswerCall_Complete" id="632398435435766538" path="Metreos.StockTools" />
        <node type="event" name="AnswerCall_Complete" id="632398435435766537" path="Metreos.Providers.H323.AnswerCall_Complete" />
        <references>
          <ref id="632405982989929586" actid="632399642734546690" />
          <ref id="632405982989929604" actid="632398435435766547" />
          <ref id="632405982989929636" actid="632405492896871841" />
          <ref id="632405982989929727" actid="632399556409545356" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632398435435766546" level="2" text="Metreos.Providers.H323.AnswerCall_Failed: OnAnswerCall_Failed">
        <node type="function" name="OnAnswerCall_Failed" id="632398435435766543" path="Metreos.StockTools" />
        <node type="event" name="AnswerCall_Failed" id="632398435435766542" path="Metreos.Providers.H323.AnswerCall_Failed" />
        <references>
          <ref id="632405982989929587" actid="632399642734546690" />
          <ref id="632405982989929605" actid="632398435435766547" />
          <ref id="632405982989929637" actid="632405492896871841" />
          <ref id="632405982989929728" actid="632399556409545356" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632398601705766676" level="2" text="Metreos.Events.cBridge.HandleCallResponse: OnHandleCallResponse">
        <node type="function" name="OnHandleCallResponse" id="632398601705766673" path="Metreos.StockTools" />
        <node type="event" name="HandleCallResponse" id="632398601705766672" path="Metreos.Events.cBridge.HandleCallResponse" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632399315498735746" level="2" text="Metreos.Providers.H323.Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632399315498735743" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632399315498735742" path="Metreos.Providers.H323.Hangup_Complete" />
        <references>
          <ref id="632405982989929627" actid="632405492896871836" />
          <ref id="632405982989929633" actid="632405492896871840" />
          <ref id="632405982989929686" actid="632399556409545431" />
          <ref id="632405982989929689" actid="632399556409545434" />
          <ref id="632405982989929699" actid="632399642734546704" />
          <ref id="632405982989929708" actid="632400013523579294" />
          <ref id="632405982989929711" actid="632400013523579295" />
          <ref id="632405982989929721" actid="632400013523579301" />
          <ref id="632405982989929750" actid="632399556409545398" />
          <ref id="632405982989929753" actid="632399556409545401" />
          <ref id="632405982989929761" actid="632399556409545413" />
          <ref id="632405982989929766" actid="632400013523579260" />
          <ref id="632405982989929778" actid="632400013523579273" />
          <ref id="632405982989929791" actid="632400013523579287" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632399315498735751" level="2" text="Metreos.Providers.H323.Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632399315498735748" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632399315498735747" path="Metreos.Providers.H323.Hangup_Failed" />
        <references>
          <ref id="632405982989929628" actid="632405492896871836" />
          <ref id="632405982989929634" actid="632405492896871840" />
          <ref id="632405982989929687" actid="632399556409545431" />
          <ref id="632405982989929690" actid="632399556409545434" />
          <ref id="632405982989929700" actid="632399642734546704" />
          <ref id="632405982989929709" actid="632400013523579294" />
          <ref id="632405982989929712" actid="632400013523579295" />
          <ref id="632405982989929722" actid="632400013523579301" />
          <ref id="632405982989929751" actid="632399556409545398" />
          <ref id="632405982989929754" actid="632399556409545401" />
          <ref id="632405982989929762" actid="632399556409545413" />
          <ref id="632405982989929767" actid="632400013523579260" />
          <ref id="632405982989929779" actid="632400013523579273" />
          <ref id="632405982989929792" actid="632400013523579287" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632399556409545377" level="2" text="Metreos.Providers.MediaServer.PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632399556409545374" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632399556409545373" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <references>
          <ref id="632405982989929692" actid="632399556409545437" />
          <ref id="632405982989929714" actid="632400013523579296" />
          <ref id="632405982989929743" actid="632399556409545383" />
          <ref id="632405982989929757" actid="632399556409545409" />
          <ref id="632405982989929773" actid="632400013523579267" />
          <ref id="632405982989929785" actid="632400013523579280" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632399556409545382" level="2" text="Metreos.Providers.MediaServer.PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632399556409545379" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632399556409545378" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <references>
          <ref id="632405982989929693" actid="632399556409545437" />
          <ref id="632405982989929715" actid="632400013523579296" />
          <ref id="632405982989929744" actid="632399556409545383" />
          <ref id="632405982989929758" actid="632399556409545409" />
          <ref id="632405982989929774" actid="632400013523579267" />
          <ref id="632405982989929786" actid="632400013523579280" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="fun" id="632399556409545351" level="1" text="ErrorHandler">
        <node type="function" name="ErrorHandler" id="632399556409545348" path="Metreos.StockTools" />
        <calls>
          <ref actid="632399556409545347" />
          <ref actid="632399556409545469" />
          <ref actid="632399556409545472" />
          <ref actid="632399642734546697" />
          <ref actid="632399556409545474" />
          <ref actid="632399556409545476" />
          <ref actid="632399556409545478" />
          <ref actid="632399556409545483" />
          <ref actid="632400013523579222" />
          <ref actid="632400013523579225" />
          <ref actid="632400013523579230" />
          <ref actid="632399556409545442" />
          <ref actid="632400013523579298" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_dbName" id="632405982989929524" vid="632392235808125180">
        <Properties type="String" initWith="DbName">g_dbName</Properties>
      </treenode>
      <treenode text="g_dbConnectionName" id="632405982989929526" vid="632392235808125182">
        <Properties type="String" initWith="DbConnectionName">g_dbConnectionName</Properties>
      </treenode>
      <treenode text="g_dbHost" id="632405982989929528" vid="632392235808125184">
        <Properties type="String" initWith="DbServer">g_dbHost</Properties>
      </treenode>
      <treenode text="g_dbPort" id="632405982989929530" vid="632392235808125186">
        <Properties type="String" initWith="DbPort">g_dbPort</Properties>
      </treenode>
      <treenode text="g_dbUsername" id="632405982989929532" vid="632392235808125188">
        <Properties type="String" initWith="DbUsername">g_dbUsername</Properties>
      </treenode>
      <treenode text="g_dbPassword" id="632405982989929534" vid="632392235808125190">
        <Properties type="String" initWith="DbPassword">g_dbPassword</Properties>
      </treenode>
      <treenode text="g_connectionId_caller" id="632405982989929536" vid="632398299705141605">
        <Properties type="Int">g_connectionId_caller</Properties>
      </treenode>
      <treenode text="g_callId_caller" id="632405982989929538" vid="632398299705141607">
        <Properties type="String">g_callId_caller</Properties>
      </treenode>
      <treenode text="g_from" id="632405982989929540" vid="632398299705141616">
        <Properties type="String">g_from</Properties>
      </treenode>
      <treenode text="g_to" id="632405982989929542" vid="632398299705141618">
        <Properties type="String">g_to</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632405982989929544" vid="632398299705141623">
        <Properties type="Int">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_timeStamp" id="632405982989929546" vid="632398299705141625">
        <Properties type="Long" defaultInitWith="0">g_timeStamp</Properties>
      </treenode>
      <treenode text="g_connectionId_callee" id="632405982989929548" vid="632398435435766506">
        <Properties type="Int">g_connectionId_callee</Properties>
      </treenode>
      <treenode text="g_callId_callee" id="632405982989929550" vid="632398435435766508">
        <Properties type="String">g_callId_callee</Properties>
      </treenode>
      <treenode text="g_callManagerIP" id="632405982989929552" vid="632398435435766530">
        <Properties type="String" initWith="CallManagerIP">g_callManagerIP</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="632405982989929554" vid="632398595693266520">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
      <treenode text="g_calleeConnected" id="632405982989929556" vid="632399556409545492">
        <Properties type="Bool" defaultInitWith="false">g_calleeConnected</Properties>
      </treenode>
      <treenode text="g_callerConnected" id="632405982989929558" vid="632399556409545494">
        <Properties type="Bool" defaultInitWith="false">g_callerConnected</Properties>
      </treenode>
      <treenode text="g_bridgeCallPattern" id="632405982989929560" vid="632399642734545932">
        <Properties type="String" initWith="BridgeCallPattern">g_bridgeCallPattern</Properties>
      </treenode>
      <treenode text="g_confAppRoutingGuid" id="632405982989929562" vid="632399642734545934">
        <Properties type="String">g_confAppRoutingGuid</Properties>
      </treenode>
      <treenode text="g_calleeDialed" id="632405982989929564" vid="632405492896871861">
        <Properties type="Bool" defaultInitWith="false">g_calleeDialed</Properties>
      </treenode>
      <treenode text="g_callerAttempted" id="632405982989929566" vid="632405492896871863">
        <Properties type="Bool" defaultInitWith="false">g_callerAttempted</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632398268734829106" treenode="632398268734829107" appnode="632398268734829104" handlerfor="632398268734829103">
    <node type="Start" id="632398268734829106" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="378">
      <linkto id="632399642734545936" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632398299705141614" name="SetMedia" class="MaxActionNode" group="" path="Metreos.Providers.H323" x="436" y="381">
      <linkto id="632399556409545469" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632399642734545938" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">callId</ap>
        <ap name="mediaPort" type="variable">mmsPort</ap>
        <ap name="mediaIP" type="variable">mmsIP</ap>
        <rd field="callId">g_callId_caller</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: setting media...</log>
        <log condition="failure" on="true" level="Error" type="literal">OnIncomingCall: failed to set media for caller...</log>
        <log condition="default" on="true" level="Error" type="literal">OnIncomingCall: failed to set media for caller...</log>
      </Properties>
    </node>
    <node type="Action" id="632398299705141615" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="202" y="380">
      <linkto id="632398435435766505" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">to</ap>
        <ap name="Value2" type="variable">from</ap>
        <ap name="Value3" type="variable">routingGuid</ap>
        <rd field="ResultData">g_to</rd>
        <rd field="ResultData2">g_from</rd>
        <rd field="ResultData3">g_routingGuid</rd>
      </Properties>
    </node>
    <node type="Action" id="632398435435766505" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="309" y="381">
      <linkto id="632399556409545347" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632398299705141614" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="literal">0</ap>
        <ap name="connectionId" type="literal">0</ap>
        <ap name="callId" type="variable">callId</ap>
        <ap name="remoteIp" type="literal">0</ap>
        <rd field="connectionId">g_connectionId_caller</rd>
        <rd field="port">mmsPort</rd>
        <rd field="ipAddress">mmsIP</rd>
        <log condition="default" on="true" level="Error" type="literal">OnIncomingCall: failed to create MMS connection for caller.</log>
      </Properties>
    </node>
    <node type="Action" id="632398435435766525" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="821" y="369" mx="887" my="385">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632398435435766514" />
        <item text="OnMakeCall_Failed" treenode="632398435435766519" />
        <item text="OnHangup" treenode="632398435435766524" />
      </items>
      <linkto id="632399556409545472" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632405492896871865" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="to" type="csharp">to + "@" + g_callManagerIP</ap>
        <ap name="connectionId" type="variable">g_connectionId_callee</ap>
        <ap name="mediaIP" type="variable">mmsIP2</ap>
        <ap name="mediaPort" type="variable">mmsPort2</ap>
        <ap name="from" type="variable">from</ap>
        <ap name="userData" type="literal">none</ap>
        <rd field="callId">g_callId_callee</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: completing outgoing call, dialing destination: " + to</log>
        <log condition="default" on="true" level="Error" type="literal">OnIncomingCall: MakeCall action failed</log>
      </Properties>
    </node>
    <node type="Action" id="632398435435766529" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="716" y="383">
      <linkto id="632399556409545469" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632398435435766525" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="literal">0</ap>
        <ap name="connectionId" type="literal">0</ap>
        <ap name="remoteIp" type="literal">0</ap>
        <rd field="connectionId">g_connectionId_callee</rd>
        <rd field="port">mmsPort2</rd>
        <rd field="ipAddress">mmsIP2</rd>
        <log condition="default" on="true" level="Error" type="literal">OnIncomingCall: failed to create mms connection for callee</log>
      </Properties>
    </node>
    <node type="Action" id="632398435435766532" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1172" y="385">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545347" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="334.6172" y="512" mx="373" my="528">
      <items count="1">
        <item text="ErrorHandler" />
      </items>
      <linkto id="632399556409545471" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">a</ap>
        <ap name="playTo" type="literal">noone</ap>
        <ap name="FunctionName" type="literal">ErrorHandler</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: Calling ErrorHandler a</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545469" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="523.6172" y="476" mx="562" my="492">
      <items count="1">
        <item text="ErrorHandler" />
      </items>
      <linkto id="632399556409545471" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">b</ap>
        <ap name="playTo" type="literal">noone</ap>
        <ap name="FunctionName" type="literal">ErrorHandler</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: Calling ErrorHandler b</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545471" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="559" y="643">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545472" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="679.6172" y="507" mx="718" my="523">
      <items count="1">
        <item text="ErrorHandler" />
      </items>
      <linkto id="632399556409545471" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">c</ap>
        <ap name="playTo" type="literal">noone</ap>
        <ap name="FunctionName" type="literal">ErrorHandler</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: Calling ErrorHandler c</log>
      </Properties>
    </node>
    <node type="Action" id="632399642734545936" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="111.90625" y="379">
      <linkto id="632398299705141615" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: removing prepended digits...</log>
	public static string Execute(string g_bridgeCallPattern, ref string to, LogWriter log)
	{
		string regexDeclaration = "regex:";

		int periodStart = g_bridgeCallPattern.IndexOf('.');
	
		// Ripping off regex: declartion
		periodStart -= regexDeclaration.Length;

		if(periodStart &gt; -1 &amp;&amp; to.Length &gt; periodStart)
		{
			to = to.Substring(periodStart);
		}

		log.Write(TraceLevel.Verbose, "The 'to' field is: " + to);

		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632399642734545938" name="GetConference" class="MaxActionNode" group="" path="Metreos.Native.CBridge" x="557.211548" y="382">
      <linkto id="632398435435766529" type="Labeled" style="Bezier" ortho="true" label="NotFound" />
      <linkto id="632399642734545941" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632399556409545469" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">g_to</ap>
        <rd field="Timestamp">g_timeStamp</rd>
        <rd field="RoutingGuid">g_confAppRoutingGuid</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: checking to see if specified conference exists...</log>
      </Properties>
    </node>
    <node type="Label" id="632399642734545941" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="556.4707" y="283" />
    <node type="Label" id="632399642734546683" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="150" y="780">
      <linkto id="632399642734546690" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632399642734546690" name="AnswerCall" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="215" y="763.1666" mx="286" my="779">
      <items count="3">
        <item text="OnAnswerCall_Complete" treenode="632398435435766541" />
        <item text="OnAnswerCall_Failed" treenode="632398435435766546" />
        <item text="OnHangup" treenode="632398435435766524" />
      </items>
      <linkto id="632399642734546697" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632405492896871866" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="answer" type="literal">true</ap>
        <ap name="callId" type="variable">callId</ap>
        <ap name="userData" type="literal">found</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: conference found, answering call...</log>
      </Properties>
    </node>
    <node type="Action" id="632399642734546691" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="577" y="777.1666">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632399642734546697" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="542.6172" y="868" mx="581" my="884">
      <items count="1">
        <item text="ErrorHandler" />
      </items>
      <linkto id="632399642734546691" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="csharp">(int)1</ap>
        <ap name="playTo" type="literal">noone</ap>
        <ap name="FunctionName" type="literal">ErrorHandler</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: Calling ErrorHandler a</log>
      </Properties>
    </node>
    <node type="Action" id="632405492896871865" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1034.4707" y="385">
      <linkto id="632398435435766532" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_calleeDialed</rd>
      </Properties>
    </node>
    <node type="Action" id="632405492896871866" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="471" y="763">
      <linkto id="632399642734546691" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_callerAttempted</rd>
      </Properties>
    </node>
    <node type="Variable" id="632398299705141601" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632398299705141602" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632398299705141603" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="from" refType="reference">from</Properties>
    </node>
    <node type="Variable" id="632398299705141612" name="mmsIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">mmsIP</Properties>
    </node>
    <node type="Variable" id="632398299705141613" name="mmsPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UShort" refType="reference">mmsPort</Properties>
    </node>
    <node type="Variable" id="632398435435766503" name="mmsIP2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">mmsIP2</Properties>
    </node>
    <node type="Variable" id="632398435435766504" name="mmsPort2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UShort" refType="reference">mmsPort2</Properties>
    </node>
    <node type="Variable" id="632398595693266519" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632398435435766513" treenode="632398435435766514" appnode="632398435435766511" handlerfor="632398435435766510">
    <node type="Start" id="632398435435766513" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="340">
      <linkto id="632398435435766536" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632398435435766536" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="134" y="339">
      <linkto id="632398435435766547" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632399556409545474" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="variable">calleePort</ap>
        <ap name="connectionId" type="variable">g_connectionId_callee</ap>
        <ap name="callId" type="variable">callId</ap>
        <ap name="remoteIp" type="variable">calleeIP</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Complete: callee connected, completing MMS connection.</log>
        <log condition="default" on="true" level="Error" type="literal">OnMakeCall_Complete: failed to fully connect caller MMS connection</log>
      </Properties>
    </node>
    <node type="Action" id="632398435435766547" name="AnswerCall" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="254" y="323" mx="325" my="339">
      <items count="3">
        <item text="OnAnswerCall_Complete" treenode="632398435435766541" />
        <item text="OnAnswerCall_Failed" treenode="632398435435766546" />
        <item text="OnHangup" treenode="632398435435766524" />
      </items>
      <linkto id="632399556409545476" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632399556409545496" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="answer" type="literal">true</ap>
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="userData" type="literal">notFound</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Complete: answering call from caller...</log>
        <log condition="default" on="true" level="Error" type="literal">OnMakeCall_Complete: AnswerCall action failed provisionally</log>
      </Properties>
    </node>
    <node type="Action" id="632398435435766551" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="565" y="340">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632399556409545474" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="96.61719" y="460" mx="135" my="476">
      <items count="1">
        <item text="ErrorHandler" />
      </items>
      <linkto id="632399556409545475" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">c</ap>
        <ap name="playTo" type="literal">noone</ap>
        <ap name="FunctionName" type="literal">ErrorHandler</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Complete: Calling ErrorHandler c</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545475" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="219" y="620">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632399556409545476" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="286.6172" y="463" mx="325" my="479">
      <items count="1">
        <item text="ErrorHandler" />
      </items>
      <linkto id="632399556409545475" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">d</ap>
        <ap name="playTo" type="literal">callee</ap>
        <ap name="FunctionName" type="literal">ErrorHandler</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Complete: Calling ErrorHandler d</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545496" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="462" y="339">
      <linkto id="632398435435766551" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="literal">true</ap>
        <rd field="ResultData">g_calleeConnected</rd>
        <rd field="ResultData2">g_callerAttempted</rd>
      </Properties>
    </node>
    <node type="Variable" id="632398435435766533" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632398435435766534" name="calleeIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="mediaIP" refType="reference">calleeIP</Properties>
    </node>
    <node type="Variable" id="632398435435766535" name="calleePort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UShort" initWith="mediaPort" refType="reference">calleePort</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632398435435766518" treenode="632398435435766519" appnode="632398435435766516" handlerfor="632398435435766515">
    <node type="Start" id="632398435435766518" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="39" y="394">
      <linkto id="632399556409545478" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632399556409545478" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="144" y="380" mx="182" my="396">
      <items count="1">
        <item text="ErrorHandler" />
      </items>
      <linkto id="632399556409545480" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">c</ap>
        <ap name="playTo" type="literal">noone</ap>
        <ap name="FunctionName" type="literal">ErrorHandler</ap>
        <log condition="entry" on="true" level="Error" type="literal">OnMakeCall_Failed: Calling ErrorHandler c</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545480" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="317" y="397">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632399315498735735" name="reason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="reason" defaultInitWith="NOT SPECIFIED" refType="reference">reason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup" startnode="632398435435766523" treenode="632398435435766524" appnode="632398435435766521" handlerfor="632398435435766520">
    <node type="Start" id="632398435435766523" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="205">
      <linkto id="632405492896871831" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632405492896871830" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1002" y="360">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632405492896871831" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="143" y="205">
      <linkto id="632405492896871834" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Deleting caller MMS connection</log>
      </Properties>
    </node>
    <node type="Action" id="632405492896871832" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="403" y="358">
      <linkto id="632405492896871837" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_callee</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Deleting callee MMS connection</log>
      </Properties>
    </node>
    <node type="Action" id="632405492896871833" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="848" y="542">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632405492896871834" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="400" y="207">
      <linkto id="632405492896871835" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632405492896871832" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_callId_caller</ap>
      </Properties>
    </node>
    <node type="Action" id="632405492896871835" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="511.4707" y="50">
      <linkto id="632405492896871836" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632405492896871838" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">(g_calleeConnected || g_calleeDialed)</ap>
      </Properties>
    </node>
    <node type="Action" id="632405492896871836" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="627" y="32" mx="689" my="48">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632405492896871838" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_callee</ap>
        <ap name="userData" type="literal">exit</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Hanging up callee</log>
      </Properties>
    </node>
    <node type="Action" id="632405492896871837" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="507.4707" y="358">
      <linkto id="632405492896871838" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632405492896871839" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_callerAttempted</ap>
      </Properties>
    </node>
    <node type="Action" id="632405492896871838" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="683.4707" y="209">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632405492896871839" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="674.4707" y="359">
      <linkto id="632405492896871840" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632405492896871841" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_callerConnected</ap>
      </Properties>
    </node>
    <node type="Action" id="632405492896871840" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="790.5625" y="345" mx="853" my="361">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632405492896871830" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632405492896871833" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="userData" type="literal">exit</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Hanging up caller</log>
      </Properties>
    </node>
    <node type="Action" id="632405492896871841" name="AnswerCall" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="604.5625" y="526" mx="676" my="542">
      <items count="3">
        <item text="OnAnswerCall_Complete" treenode="632398435435766541" />
        <item text="OnAnswerCall_Failed" treenode="632398435435766546" />
        <item text="OnHangup" treenode="632398435435766524" />
      </items>
      <linkto id="632405492896871833" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="answer" type="literal">false</ap>
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Rejecting caller</log>
      </Properties>
    </node>
    <node type="Variable" id="632405492896871868" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callId" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnAnswerCall_Complete" activetab="true" startnode="632398435435766540" treenode="632398435435766541" appnode="632398435435766538" handlerfor="632398435435766537">
    <node type="Start" id="632398435435766540" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="39" y="168">
      <linkto id="632405492896871867" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632398435435766555" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="264" y="455">
      <linkto id="632399642734546701" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632399556409545483" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="variable">callerPort</ap>
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="remoteIp" type="variable">calleeIP</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Complete: completing MMS connection for caller...</log>
        <log condition="default" on="true" level="Error" type="literal">OnAnswerCall_Complete: failed to complete mms connection for caller</log>
      </Properties>
    </node>
    <node type="Action" id="632398435435766556" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="501" y="168">
      <linkto id="632399556409545497" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632400013523579225" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="lineId" type="variable">g_to</ap>
        <ap name="callType" type="literal">bridge</ap>
        <ap name="callIdSlave" type="variable">g_callId_caller</ap>
        <ap name="connectionIdSlave" type="variable">g_connectionId_caller</ap>
        <ap name="fromNumberSlave" type="variable">g_from</ap>
        <ap name="requestingGuid" type="variable">g_routingGuid</ap>
        <ap name="timestamp" type="variable">g_timeStamp</ap>
        <ap name="EventName" type="literal">Metreos.Events.cBridge.HandleCall</ap>
        <ap name="ToGuid" type="variable">g_confAppRoutingGuid</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Complete: SendEvent existing instance of application</log>
        <log condition="failure" on="true" level="Info" type="literal">OnAnswerCall_Complete: SendEvent existing instance of application</log>
        <log condition="default" on="true" level="Error" type="literal">OnAnswerCall_Complete: SendEvent existing instance of application</log>
      </Properties>
    </node>
    <node type="Action" id="632398601705766670" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="727" y="166">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632399556409545483" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="227.617188" y="552" mx="266" my="568">
      <items count="1">
        <item text="ErrorHandler" />
      </items>
      <linkto id="632400013523579224" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">e</ap>
        <ap name="playTo" type="literal">callee</ap>
        <ap name="FunctionName" type="literal">ErrorHandler</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Complete: Calling error handler 5</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545486" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="559" y="454">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632399556409545497" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="619" y="167">
      <linkto id="632398601705766670" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_callerConnected</rd>
      </Properties>
    </node>
    <node type="Action" id="632399642734546700" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="266" y="169">
      <linkto id="632400013523579220" type="Labeled" style="Bezier" ortho="true" label="found" />
      <linkto id="632398435435766555" type="Labeled" style="Bezier" ortho="true" label="notFound" />
      <linkto id="632400013523579256" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632399642734546701" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="426" y="453">
      <linkto id="632400013523579222" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632399556409545486" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="appControl">
        <ap name="lineId" type="variable">g_to</ap>
        <ap name="callType" type="literal">incoming</ap>
        <ap name="callIdMaster" type="variable">g_callId_callee</ap>
        <ap name="callIdSlave" type="variable">g_callId_caller</ap>
        <ap name="connectionIdMaster" type="variable">g_connectionId_callee</ap>
        <ap name="connectionIdSlave" type="variable">g_connectionId_caller</ap>
        <ap name="fromNumberMaster" type="variable">g_to</ap>
        <ap name="fromNumberSlave" type="variable">g_from</ap>
        <ap name="requestingGuid" type="variable">g_routingGuid</ap>
        <ap name="timestamp" type="csharp">(Int64)0</ap>
        <ap name="EventName" type="literal">Metreos.Events.cBridge.HandleCall</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Complete: Conference was not found, SendEvent to kick off new instance of application</log>
        <log condition="default" on="true" level="Error" type="literal">OnAnswerCall_Complete: SendEvent to conference app failed, notifying caller, exiting</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579220" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="385" y="169">
      <linkto id="632398435435766556" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632400013523579225" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="variable">callerPort</ap>
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="remoteIp" type="variable">calleeIP</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Complete: completing MMS connection for caller...</log>
        <log condition="default" on="true" level="Error" type="literal">OnAnswerCall_Complete: failed to complete mms connection for caller</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579222" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="391.6172" y="555" mx="430" my="571">
      <items count="1">
        <item text="ErrorHandler" />
      </items>
      <linkto id="632400013523579224" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">e</ap>
        <ap name="playTo" type="literal">both</ap>
        <ap name="FunctionName" type="literal">ErrorHandler</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Complete: Calling error handler 5</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579224" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="350" y="691">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632400013523579225" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="407.708984" y="253" mx="446" my="269">
      <items count="1">
        <item text="ErrorHandler" />
      </items>
      <linkto id="632400013523579226" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">f</ap>
        <ap name="playTo" type="literal">caller</ap>
        <ap name="FunctionName" type="literal">ErrorHandler</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Complete: Calling ErrorHandler f</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579226" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="581.0918" y="269">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632400013523579256" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="266" y="36">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632405492896871867" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="138" y="169">
      <linkto id="632399642734546700" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_callerConnected</rd>
      </Properties>
    </node>
    <node type="Variable" id="632398435435766553" name="calleeIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="mediaIP" refType="reference">calleeIP</Properties>
    </node>
    <node type="Variable" id="632398435435766554" name="callerPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UShort" initWith="mediaPort" refType="reference">callerPort</Properties>
    </node>
    <node type="Variable" id="632399642734546699" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="userData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnAnswerCall_Failed" startnode="632398435435766545" treenode="632398435435766546" appnode="632398435435766543" handlerfor="632398435435766542">
    <node type="Start" id="632398435435766545" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="304">
      <linkto id="632400013523579232" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632400013523579230" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="265.392822" y="450" mx="304" my="466">
      <items count="1">
        <item text="ErrorHandler" />
      </items>
      <linkto id="632400013523579237" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">g</ap>
        <ap name="playTo" type="literal">callee</ap>
        <ap name="FunctionName" type="literal">ErrorHandler</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Failed: Calling error handler 7</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579232" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="149.776" y="304">
      <linkto id="632400013523579233" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="literal">false</ap>
        <rd field="ResultData">g_callerConnected</rd>
        <rd field="ResultData2">g_callerAttempted</rd>
      </Properties>
    </node>
    <node type="Action" id="632400013523579233" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="299.776062" y="303">
      <linkto id="632400013523579253" type="Labeled" style="Bezier" ortho="true" label="found" />
      <linkto id="632400013523579230" type="Labeled" style="Bezier" ortho="true" label="notFound" />
      <linkto id="632400013523579255" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632400013523579237" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="299.776" y="616">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632400013523579253" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="422" y="303">
      <linkto id="632400013523579254" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Failed: Deleting caller MMS connection, exiting.</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579254" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="539" y="303">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632400013523579255" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="299" y="169">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632399642734546721" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHandleCallResponse" startnode="632398601705766675" treenode="632398601705766676" appnode="632398601705766673" handlerfor="632398601705766672">
    <node type="Start" id="632398601705766675" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="439">
      <linkto id="632398601705766678" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632398601705766678" name="Forward" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="144" y="438">
      <Properties final="true" type="appControl">
        <ap name="ToGuid" type="variable">respondingGuid</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHandleCallReponse: Forwarding all events to handling application</log>
      </Properties>
    </node>
    <node type="Variable" id="632398601705766677" name="respondingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="respondingGuid" refType="reference">respondingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup_Complete" startnode="632399315498735745" treenode="632399315498735746" appnode="632399315498735743" handlerfor="632399315498735742">
    <node type="Start" id="632399315498735745" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="212">
      <linkto id="632399556409545420" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632399556409545420" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="137.4707" y="211">
      <linkto id="632399556409545422" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632399556409545421" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">userData</ap>
        <ap name="Value2" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545421" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="267.4707" y="211">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632399556409545422" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="138.4707" y="337">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632399556409545426" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="userData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup_Failed" startnode="632399315498735750" treenode="632399315498735751" appnode="632399315498735748" handlerfor="632399315498735747">
    <node type="Start" id="632399315498735750" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="38" y="323">
      <linkto id="632399556409545416" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632399556409545416" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="138" y="322">
      <linkto id="632399556409545419" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632399556409545418" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">userData</ap>
        <ap name="Value2" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545418" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="268" y="322">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632399556409545419" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="139" y="448">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632399556409545417" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="userData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Complete" startnode="632399556409545376" treenode="632399556409545377" appnode="632399556409545374" handlerfor="632399556409545373">
    <node type="Start" id="632399556409545376" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="39" y="340">
      <linkto id="632399556409545428" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632399556409545428" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="138" y="340">
      <linkto id="632399556409545429" type="Labeled" style="Bezier" ortho="true" label="callee" />
      <linkto id="632399556409545430" type="Labeled" style="Bezier" ortho="true" label="both" />
      <linkto id="632399556409545489" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632400013523579257" type="Labeled" style="Bezier" ortho="true" label="caller" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545429" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="293" y="338">
      <linkto id="632399556409545431" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_callee</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: delete callee mms connection</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545430" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="294" y="481">
      <linkto id="632399556409545434" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: deleting caller MMS connection</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545431" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="368" y="324" mx="430" my="340">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632399556409545440" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_callee</ap>
        <ap name="userData" type="literal">exit</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: Hanging up callee and exiting...</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545434" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="367" y="466" mx="429" my="482">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632399556409545437" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: hanging up caller...</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545437" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="531" y="467" mx="624" my="483">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632399556409545377" />
        <item text="OnPlayAnnouncement_Failed" treenode="632399556409545382" />
      </items>
      <linkto id="632399556409545440" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632399556409545442" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="connectionId" type="variable">g_connectionId_callee</ap>
        <ap name="filename" type="literal">cb_failure.wav</ap>
        <ap name="userData" type="literal">callee</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545440" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="621" y="339">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632399556409545442" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="585.6172" y="619" mx="624" my="635">
      <items count="1">
        <item text="ErrorHandler" />
      </items>
      <linkto id="632399556409545443" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">d</ap>
        <ap name="playTo" type="literal">callee</ap>
        <ap name="FunctionName" type="literal">ErrorHandler</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: PlayAnnouncement to caller failed, calling error handler...</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545443" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="619" y="756">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632399556409545489" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="138" y="560">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: exiting function..</log>
      </Properties>
    </node>
    <node type="Action" id="632399642734546704" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="561" y="173" mx="623" my="189">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632399556409545440" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="userData" type="literal">exit</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: hanging up caller...</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579257" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="294" y="190">
      <linkto id="632399642734546704" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
      </Properties>
    </node>
    <node type="Variable" id="632399556409545427" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="userData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Failed" startnode="632399556409545381" treenode="632399556409545382" appnode="632399556409545379" handlerfor="632399556409545378">
    <node type="Start" id="632399556409545381" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="55" y="322">
      <linkto id="632400013523579291" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632400013523579291" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="135.4707" y="322">
      <linkto id="632400013523579292" type="Labeled" style="Bezier" ortho="true" label="callee" />
      <linkto id="632400013523579293" type="Labeled" style="Bezier" ortho="true" label="both" />
      <linkto id="632400013523579300" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632400013523579302" type="Labeled" style="Bezier" ortho="true" label="caller" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632400013523579292" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="291.4707" y="320">
      <linkto id="632400013523579294" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_callee</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed: delete callee mms connection</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579293" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="291.4707" y="462">
      <linkto id="632400013523579295" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed: deleting caller MMS connection</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579294" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="366.4707" y="306" mx="429" my="322">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632400013523579297" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_callee</ap>
        <ap name="userData" type="literal">exit</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed: Hanging up callee and exiting...</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579295" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="364.4707" y="448" mx="427" my="464">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632400013523579296" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed: hanging up caller...</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579296" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="528.4707" y="449" mx="622" my="465">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632399556409545377" />
        <item text="OnPlayAnnouncement_Failed" treenode="632399556409545382" />
      </items>
      <linkto id="632400013523579297" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632400013523579298" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="connectionId" type="variable">g_connectionId_callee</ap>
        <ap name="filename" type="literal">cb_failure.wav</ap>
        <ap name="userData" type="literal">callee</ap>
      </Properties>
    </node>
    <node type="Action" id="632400013523579297" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="618.4707" y="321">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632400013523579298" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="583.0879" y="601" mx="621" my="617">
      <items count="1">
        <item text="ErrorHandler" />
      </items>
      <linkto id="632400013523579299" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">d</ap>
        <ap name="playTo" type="literal">callee</ap>
        <ap name="FunctionName" type="literal">ErrorHandler</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed: PlayAnnouncement to caller failed, calling error handler...</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579299" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="616.4707" y="738">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632400013523579300" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="135.4707" y="542">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: exiting function..</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579301" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="557.4707" y="156" mx="620" my="172">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632400013523579297" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="userData" type="literal">exit</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed: hanging up caller...</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579302" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="291.4707" y="175">
      <linkto id="632400013523579301" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed: deleting caller connection</log>
      </Properties>
    </node>
    <node type="Variable" id="632399556409545468" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="userData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ErrorHandler" startnode="632399556409545350" treenode="632399556409545351" appnode="632399556409545348" handlerfor="632399556409545378">
    <node type="Start" id="632399556409545350" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="267">
      <linkto id="632405192676764757" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632399556409545356" name="AnswerCall" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="150" y="504" mx="221" my="520">
      <items count="3">
        <item text="OnAnswerCall_Complete" treenode="632398435435766541" />
        <item text="OnAnswerCall_Failed" treenode="632398435435766546" />
        <item text="OnHangup" treenode="632398435435766524" />
      </items>
      <linkto id="632399556409545366" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="answer" type="literal">false</ap>
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">ErrorHandler: rejecting caller, exiting</log>
      </Properties>
    </node>
    <node type="Label" id="632399556409545360" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="141" y="144" />
    <node type="Label" id="632399556409545361" text="b" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="224" y="157" />
    <node type="Label" id="632399556409545362" text="c" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="295" y="189" />
    <node type="Label" id="632399556409545363" text="d" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="309" y="266" />
    <node type="Label" id="632399556409545364" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="289" y="324" />
    <node type="Label" id="632399556409545365" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="55" y="518">
      <linkto id="632399556409545356" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632399556409545366" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="405" y="520">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Label" id="632399556409545367" text="b" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="53" y="680">
      <linkto id="632399556409545368" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632399556409545368" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="217" y="681">
      <linkto id="632399556409545356" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <log condition="entry" on="true" level="Info" type="literal">ErrorHandler: deleting callers MMS connection</log>
      </Properties>
    </node>
    <node type="Label" id="632399556409545369" text="c" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="52" y="789">
      <linkto id="632399556409545370" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632399556409545370" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="217" y="791">
      <linkto id="632399556409545368" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_callee</ap>
        <log condition="entry" on="true" level="Info" type="literal">ErrorHandler: deleting callee MMS connection</log>
      </Properties>
    </node>
    <node type="Label" id="632399556409545371" text="d" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="54" y="937">
      <linkto id="632399556409545409" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632399556409545383" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="448" y="1241" mx="541" my="1257">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632399556409545377" />
        <item text="OnPlayAnnouncement_Failed" treenode="632399556409545382" />
      </items>
      <linkto id="632399556409545405" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="connectionId" type="variable">connIdToUse</ap>
        <ap name="filename" type="literal">cb_failure.wav</ap>
        <ap name="userData" type="variable">playTo</ap>
      </Properties>
    </node>
    <node type="Label" id="632399556409545386" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="32" y="1254">
      <linkto id="632399556409545390" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632399556409545390" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="163" y="1252">
      <linkto id="632399556409545391" type="Labeled" style="Bezier" ortho="true" label="callee" />
      <linkto id="632399556409545392" type="Labeled" style="Bezier" ortho="true" label="both" />
      <linkto id="632400013523579263" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">playTo</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545391" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="255" y="1142">
      <linkto id="632400013523579258" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">g_connectionId_callee</ap>
        <rd field="ResultData">connIdToUse</rd>
      </Properties>
    </node>
    <node type="Action" id="632399556409545392" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="352" y="1254">
      <linkto id="632399556409545383" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">g_connectionId_caller</ap>
        <rd field="ResultData">connIdToUse</rd>
      </Properties>
    </node>
    <node type="Action" id="632399556409545398" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="437" y="1380" mx="499" my="1396">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632399556409545401" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545401" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="627" y="1378" mx="689" my="1394">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632399556409545405" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632400013523579276" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_callee</ap>
        <ap name="userData" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545405" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="684" y="1256">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545409" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="94" y="920" mx="187" my="936">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632399556409545377" />
        <item text="OnPlayAnnouncement_Failed" treenode="632399556409545382" />
      </items>
      <linkto id="632399556409545412" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632399556409545413" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="connectionId" type="variable">g_connectionId_callee</ap>
        <ap name="filename" type="literal">cb_failure.wav</ap>
        <ap name="userData" type="variable">playTo</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545412" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="414" y="900">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545413" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="354" y="993" mx="416" my="1009">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632399556409545412" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632400013523579278" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_callee</ap>
        <ap name="userData" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Comment" id="632399556409545355" text="1. Reject caller&#xD;&#xA;2. reject caller and his connection&#xD;&#xA;3. reject caller and both connections&#xD;&#xA;4. hang up callee, answer call already failed, delete both connections&#xD;&#xA;5. hang up both calls and both connections&#xD;&#xA;6. hang up caller, delete caller connection &#xD;&#xA;7. hang up callee, delete both, play to callee" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="282" y="32" />
    <node type="Action" id="632400013523579258" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="372" y="1142">
      <linkto id="632400013523579260" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <log condition="entry" on="true" level="Info" type="literal">ErrorHandler: deleting caller connection</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579260" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="478" y="1126" mx="540" my="1142">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632399556409545383" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">ErrorHandler: hanging up caller</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579263" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="249" y="1394">
      <linkto id="632400013523579264" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
      </Properties>
    </node>
    <node type="Action" id="632400013523579264" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="385" y="1394">
      <linkto id="632399556409545398" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_callee</ap>
      </Properties>
    </node>
    <node type="Label" id="632400013523579265" text="f" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="32" y="1563">
      <linkto id="632400013523579266" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632400013523579266" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="154" y="1563">
      <linkto id="632400013523579267" type="Labeled" style="Bezier" ortho="true" label="caller" />
      <linkto id="632400013523579272" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">playTo</ap>
      </Properties>
    </node>
    <node type="Action" id="632400013523579267" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="246" y="1547" mx="339" my="1563">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632399556409545377" />
        <item text="OnPlayAnnouncement_Failed" treenode="632399556409545382" />
      </items>
      <linkto id="632400013523579271" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632400013523579272" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <ap name="filename" type="literal">cb_failure.wav</ap>
        <ap name="userData" type="literal">caller</ap>
      </Properties>
    </node>
    <node type="Action" id="632400013523579271" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="509" y="1563">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632400013523579272" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="339" y="1740">
      <linkto id="632400013523579273" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <log condition="entry" on="true" level="Info" type="literal">ErrorHandler: deleting caller connection</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579273" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="453" y="1724" mx="515" my="1740">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632400013523579271" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632400013523579277" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="userData" type="literal">exit</ap>
        <log condition="entry" on="true" level="Info" type="literal">ErrorHandler: hanging up caller</log>
      </Properties>
    </node>
    <node type="Action" id="632400013523579276" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="794.3203" y="1394">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632400013523579277" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="644.3203" y="1739">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632400013523579278" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="533.3203" y="1008">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Label" id="632400013523579279" text="g" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="32" y="1910">
      <linkto id="632400013523579284" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632400013523579280" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="205" y="1892" mx="298" my="1908">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632399556409545377" />
        <item text="OnPlayAnnouncement_Failed" treenode="632399556409545382" />
      </items>
      <linkto id="632400013523579285" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632400013523579286" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="connectionId" type="variable">g_connectionId_callee</ap>
        <ap name="filename" type="literal">cb_failure.wav</ap>
        <ap name="userData" type="literal">callee</ap>
      </Properties>
    </node>
    <node type="Action" id="632400013523579284" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="138" y="1908">
      <linkto id="632400013523579280" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
      </Properties>
    </node>
    <node type="Action" id="632400013523579285" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="456" y="1906">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632400013523579286" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="296" y="2049">
      <linkto id="632400013523579287" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_callee</ap>
      </Properties>
    </node>
    <node type="Action" id="632400013523579287" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="399" y="2031" mx="461" my="2047">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632400013523579285" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632400013523579290" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="userData" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632400013523579290" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="589" y="2047">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Label" id="632400013523579323" text="f" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="223" y="367" />
    <node type="Label" id="632400013523579324" text="g" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="142" y="410" />
    <node type="Action" id="632405192676764757" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="141" y="266">
      <linkto id="632399556409545360" type="Labeled" style="Bezier" ortho="true" label="a" />
      <linkto id="632399556409545362" type="Labeled" style="Bezier" ortho="true" label="c" />
      <linkto id="632399556409545361" type="Labeled" style="Bezier" ortho="true" label="b" />
      <linkto id="632399556409545363" type="Labeled" style="Bezier" ortho="true" label="d" />
      <linkto id="632399556409545364" type="Labeled" style="Bezier" ortho="true" label="e" />
      <linkto id="632400013523579323" type="Labeled" style="Bezier" ortho="true" label="f" />
      <linkto id="632400013523579324" type="Labeled" style="Bezier" ortho="true" label="g" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">action</ap>
      </Properties>
    </node>
    <node type="Variable" id="632399556409545352" name="action" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="action" refType="reference">action</Properties>
    </node>
    <node type="Variable" id="632399556409545389" name="playTo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="playTo" defaultInitWith="noone" refType="reference">playTo</Properties>
    </node>
    <node type="Variable" id="632399556409545397" name="connIdToUse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">connIdToUse</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>