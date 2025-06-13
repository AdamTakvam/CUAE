<Application name="IncomingCall" trigger="Metreos.Providers.H323.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="IncomingCall">
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
          <ref id="632423570354993070" actid="632398435435766525" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632398435435766519" level="2" text="Metreos.Providers.H323.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632398435435766516" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632398435435766515" path="Metreos.Providers.H323.MakeCall_Failed" />
        <references>
          <ref id="632423570354993071" actid="632398435435766525" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632398435435766524" level="2" text="Metreos.Providers.H323.Hangup: OnHangup">
        <node type="function" name="OnHangup" id="632398435435766521" path="Metreos.StockTools" />
        <node type="event" name="Hangup" id="632398435435766520" path="Metreos.Providers.H323.Hangup" />
        <references>
          <ref id="632423570354993072" actid="632398435435766525" />
          <ref id="632423570354993093" actid="632398435435766547" />
          <ref id="632423570354993125" actid="632405445832733827" />
          <ref id="632423570354993195" actid="632399556409545356" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632398435435766541" level="2" text="Metreos.Providers.H323.AnswerCall_Complete: OnAnswerCall_Complete">
        <node type="function" name="OnAnswerCall_Complete" id="632398435435766538" path="Metreos.StockTools" />
        <node type="event" name="AnswerCall_Complete" id="632398435435766537" path="Metreos.Providers.H323.AnswerCall_Complete" />
        <references>
          <ref id="632423570354993091" actid="632398435435766547" />
          <ref id="632423570354993123" actid="632405445832733827" />
          <ref id="632423570354993193" actid="632399556409545356" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632398435435766546" level="2" text="Metreos.Providers.H323.AnswerCall_Failed: OnAnswerCall_Failed">
        <node type="function" name="OnAnswerCall_Failed" id="632398435435766543" path="Metreos.StockTools" />
        <node type="event" name="AnswerCall_Failed" id="632398435435766542" path="Metreos.Providers.H323.AnswerCall_Failed" />
        <references>
          <ref id="632423570354993092" actid="632398435435766547" />
          <ref id="632423570354993124" actid="632405445832733827" />
          <ref id="632423570354993194" actid="632399556409545356" />
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
          <ref id="632423570354993114" actid="632405445832733443" />
          <ref id="632423570354993120" actid="632405445832733824" />
          <ref id="632423570354993159" actid="632399556409545431" />
          <ref id="632423570354993162" actid="632399556409545434" />
          <ref id="632423570354993177" actid="632399556409545447" />
          <ref id="632423570354993180" actid="632399556409545448" />
          <ref id="632423570354993216" actid="632399556409545398" />
          <ref id="632423570354993219" actid="632399556409545401" />
          <ref id="632423570354993228" actid="632399556409545413" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632399315498735751" level="2" text="Metreos.Providers.H323.Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632399315498735748" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632399315498735747" path="Metreos.Providers.H323.Hangup_Failed" />
        <references>
          <ref id="632423570354993115" actid="632405445832733443" />
          <ref id="632423570354993121" actid="632405445832733824" />
          <ref id="632423570354993160" actid="632399556409545431" />
          <ref id="632423570354993163" actid="632399556409545434" />
          <ref id="632423570354993178" actid="632399556409545447" />
          <ref id="632423570354993181" actid="632399556409545448" />
          <ref id="632423570354993217" actid="632399556409545398" />
          <ref id="632423570354993220" actid="632399556409545401" />
          <ref id="632423570354993229" actid="632399556409545413" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632399556409545377" level="2" text="Metreos.Providers.MediaServer.PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632399556409545374" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632399556409545373" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <references>
          <ref id="632423570354993165" actid="632399556409545437" />
          <ref id="632423570354993183" actid="632399556409545449" />
          <ref id="632423570354993209" actid="632399556409545383" />
          <ref id="632423570354993224" actid="632399556409545409" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632399556409545382" level="2" text="Metreos.Providers.MediaServer.PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632399556409545379" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632399556409545378" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <references>
          <ref id="632423570354993166" actid="632399556409545437" />
          <ref id="632423570354993184" actid="632399556409545449" />
          <ref id="632423570354993210" actid="632399556409545383" />
          <ref id="632423570354993225" actid="632399556409545409" />
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
          <ref actid="632399556409545474" />
          <ref actid="632399556409545476" />
          <ref actid="632399556409545478" />
          <ref actid="632399556409545483" />
          <ref actid="632399556409545484" />
          <ref actid="632399556409545487" />
          <ref actid="632399556409545442" />
          <ref actid="632399556409545451" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_dbName" id="632423570354993026" vid="632392235808125180">
        <Properties type="String" initWith="DbName">g_dbName</Properties>
      </treenode>
      <treenode text="g_dbConnectionName" id="632423570354993028" vid="632392235808125182">
        <Properties type="String" initWith="DbConnectionName">g_dbConnectionName</Properties>
      </treenode>
      <treenode text="g_dbHost" id="632423570354993030" vid="632392235808125184">
        <Properties type="String" initWith="DbServer">g_dbHost</Properties>
      </treenode>
      <treenode text="g_dbPort" id="632423570354993032" vid="632392235808125186">
        <Properties type="String" initWith="DbPort">g_dbPort</Properties>
      </treenode>
      <treenode text="g_dbUsername" id="632423570354993034" vid="632392235808125188">
        <Properties type="String" initWith="DbUsername">g_dbUsername</Properties>
      </treenode>
      <treenode text="g_dbPassword" id="632423570354993036" vid="632392235808125190">
        <Properties type="String" initWith="DbPassword">g_dbPassword</Properties>
      </treenode>
      <treenode text="g_connectionId_caller" id="632423570354993038" vid="632398299705141605">
        <Properties type="Int">g_connectionId_caller</Properties>
      </treenode>
      <treenode text="g_callId_caller" id="632423570354993040" vid="632398299705141607">
        <Properties type="String">g_callId_caller</Properties>
      </treenode>
      <treenode text="g_from" id="632423570354993042" vid="632398299705141616">
        <Properties type="String">g_from</Properties>
      </treenode>
      <treenode text="g_to" id="632423570354993044" vid="632398299705141618">
        <Properties type="String">g_to</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632423570354993046" vid="632398299705141623">
        <Properties type="Int">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_timeStamp" id="632423570354993048" vid="632398299705141625">
        <Properties type="Long" defaultInitWith="0">g_timeStamp</Properties>
      </treenode>
      <treenode text="g_connectionId_callee" id="632423570354993050" vid="632398435435766506">
        <Properties type="Int">g_connectionId_callee</Properties>
      </treenode>
      <treenode text="g_callId_callee" id="632423570354993052" vid="632398435435766508">
        <Properties type="String">g_callId_callee</Properties>
      </treenode>
      <treenode text="g_callManagerIP" id="632423570354993054" vid="632398435435766530">
        <Properties type="String" initWith="CallManagerIP">g_callManagerIP</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="632423570354993056" vid="632398595693266520">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
      <treenode text="g_calleeConnected" id="632423570354993058" vid="632399556409545492">
        <Properties type="Bool" defaultInitWith="false">g_calleeConnected</Properties>
      </treenode>
      <treenode text="g_callerConnected" id="632423570354993060" vid="632399556409545494">
        <Properties type="Bool" defaultInitWith="false">g_callerConnected</Properties>
      </treenode>
      <treenode text="g_calleeDialed" id="632423570354993062" vid="632405445832733435">
        <Properties type="Bool" defaultInitWith="false">g_calleeDialed</Properties>
      </treenode>
      <treenode text="g_callerAttempted" id="632423570354993064" vid="632405445832733437">
        <Properties type="Bool" defaultInitWith="false">g_callerAttempted</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632398268734829106" treenode="632398268734829107" appnode="632398268734829104" handlerfor="632398268734829103">
    <node type="Start" id="632398268734829106" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="378">
      <linkto id="632398299705141615" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632398299705141614" name="SetMedia" class="MaxActionNode" group="" path="Metreos.Providers.H323" x="350" y="378">
      <linkto id="632398435435766529" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632399556409545469" type="Labeled" style="Bezier" ortho="true" label="default" />
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
    <node type="Action" id="632398299705141615" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="116" y="378">
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
    <node type="Action" id="632398435435766505" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="222" y="378">
      <linkto id="632398299705141614" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632399556409545347" type="Labeled" style="Bezier" ortho="true" label="default" />
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
    <node type="Action" id="632398435435766525" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="624" y="365" mx="690" my="381">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632398435435766514" />
        <item text="OnMakeCall_Failed" treenode="632398435435766519" />
        <item text="OnHangup" treenode="632398435435766524" />
      </items>
      <linkto id="632399556409545472" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632405445832733439" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="to" type="csharp">to + "@" + g_callManagerIP</ap>
        <ap name="connectionId" type="variable">g_connectionId_callee</ap>
        <ap name="mediaIP" type="variable">mmsIP2</ap>
        <ap name="mediaPort" type="variable">mmsPort2</ap>
        <ap name="from" type="variable">from</ap>
        <ap name="userData" type="literal">none</ap>
        <rd field="callId">g_callId_callee</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: completing incoming call, dialing destination: " + to</log>
        <log condition="default" on="true" level="Error" type="literal">OnIncomingCall: MakeCall action failed</log>
      </Properties>
    </node>
    <node type="Action" id="632398435435766529" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="494" y="379">
      <linkto id="632398435435766525" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632399556409545469" type="Labeled" style="Bezier" ortho="true" label="default" />
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
    <node type="Action" id="632398435435766532" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="914" y="382">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545347" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="187.617188" y="463" mx="226" my="479">
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
    <node type="Action" id="632399556409545469" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="313.6172" y="463" mx="352" my="479">
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
    <node type="Action" id="632399556409545471" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="348" y="653">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545472" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="459.6172" y="456" mx="498" my="472">
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
    <node type="Action" id="632405445832733439" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="801.4707" y="381">
      <linkto id="632398435435766532" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_calleeDialed</rd>
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
    <node type="Action" id="632398435435766547" name="AnswerCall" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="255" y="325" mx="326" my="341">
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
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Complete: answering call from caller...</log>
        <log condition="default" on="true" level="Error" type="literal">OnMakeCall_Complete: AnswerCall action failed provisionally</log>
      </Properties>
    </node>
    <node type="Action" id="632398435435766551" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="567" y="342">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632399556409545474" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="97.61719" y="460" mx="136" my="476">
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
    <node type="Action" id="632399556409545496" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="462" y="341">
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
    <node type="Start" id="632398435435766523" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="339">
      <linkto id="632399556409545498" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632398601705766671" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1020" y="493">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632399556409545498" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="161" y="338">
      <linkto id="632399556409545499" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Deleting caller MMS connection</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545499" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="306" y="339">
      <linkto id="632405445832733440" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_callee</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Deleting callee MMS connection</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545508" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="866" y="675">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632405445832733440" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="418" y="340">
      <linkto id="632405445832733442" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632405445832733447" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_callId_caller</ap>
      </Properties>
    </node>
    <node type="Action" id="632405445832733442" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="529.4707" y="181">
      <linkto id="632405445832733443" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632405445832733822" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">(g_calleeConnected || g_calleeDialed)</ap>
      </Properties>
    </node>
    <node type="Action" id="632405445832733443" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="645" y="165" mx="707" my="181">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632405445832733822" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_callee</ap>
        <ap name="userData" type="literal">exit</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Hanging up callee</log>
      </Properties>
    </node>
    <node type="Action" id="632405445832733447" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="525.4707" y="491">
      <linkto id="632405445832733822" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632405445832733823" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_callerAttempted</ap>
      </Properties>
    </node>
    <node type="Action" id="632405445832733822" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="701.4707" y="342">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632405445832733823" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="692.4707" y="492">
      <linkto id="632405445832733824" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632405445832733827" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_callerConnected</ap>
      </Properties>
    </node>
    <node type="Action" id="632405445832733824" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="808.5625" y="478" mx="871" my="494">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632398601705766671" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632399556409545508" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="userData" type="literal">exit</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Hanging up caller</log>
      </Properties>
    </node>
    <node type="Action" id="632405445832733827" name="AnswerCall" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="622.5625" y="659" mx="694" my="675">
      <items count="3">
        <item text="OnAnswerCall_Complete" treenode="632398435435766541" />
        <item text="OnAnswerCall_Failed" treenode="632398435435766546" />
        <item text="OnHangup" treenode="632398435435766524" />
      </items>
      <linkto id="632399556409545508" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="answer" type="literal">false</ap>
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Rejecting caller</log>
      </Properties>
    </node>
    <node type="Variable" id="632405445832733441" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callId" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnAnswerCall_Complete" activetab="true" startnode="632398435435766540" treenode="632398435435766541" appnode="632398435435766538" handlerfor="632398435435766537">
    <node type="Start" id="632398435435766540" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="377">
      <linkto id="632398435435766555" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632398435435766555" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="140" y="378">
      <linkto id="632398435435766556" type="Labeled" style="Bezier" ortho="true" label="success" />
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
    <node type="Action" id="632398435435766556" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="278" y="377">
      <linkto id="632399556409545484" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632399556409545497" type="Labeled" style="Bezier" ortho="true" label="success" />
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
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Complete: SendEvent to kick off new instance of application</log>
        <log condition="default" on="true" level="Error" type="literal">OnAnswerCall_Complete: SendEvent to conference app failed, calling error handler 5</log>
      </Properties>
    </node>
    <node type="Action" id="632398601705766670" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="505" y="377">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632399556409545483" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="106.617188" y="488" mx="145" my="504">
      <items count="1">
        <item text="ErrorHandler" />
      </items>
      <linkto id="632399556409545486" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">e</ap>
        <ap name="playTo" type="literal">callee</ap>
        <ap name="FunctionName" type="literal">ErrorHandler</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Complete: Calling error handler 5</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545484" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="241.617188" y="487" mx="280" my="503">
      <items count="1">
        <item text="ErrorHandler" />
      </items>
      <linkto id="632399556409545486" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">e</ap>
        <ap name="playTo" type="literal">both</ap>
        <ap name="FunctionName" type="literal">ErrorHandler</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Complete: Calling error handler 5</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545486" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="212" y="630">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632399556409545497" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="397" y="377">
      <linkto id="632398601705766670" type="Labeled" style="Bezier" ortho="true" label="default" />
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
  </canvas>
  <canvas type="Function" name="OnAnswerCall_Failed" startnode="632398435435766545" treenode="632398435435766546" appnode="632398435435766543" handlerfor="632398435435766542">
    <node type="Start" id="632398435435766545" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="436">
      <linkto id="632405445832733821" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632399556409545487" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="205.617188" y="422" mx="244" my="438">
      <items count="1">
        <item text="ErrorHandler" />
      </items>
      <linkto id="632399556409545488" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">d</ap>
        <ap name="playTo" type="literal">callee</ap>
        <ap name="FunctionName" type="literal">ErrorHandler</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Failed: Calling ErrorHandler d</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545488" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="343" y="439">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632405445832733821" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="123" y="438">
      <linkto id="632399556409545487" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_callerAttempted</rd>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHandleCallResponse" startnode="632398601705766675" treenode="632398601705766676" appnode="632398601705766673" handlerfor="632398601705766672">
    <node type="Start" id="632398601705766675" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="439">
      <linkto id="632398601705766678" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632398601705766678" name="Forward" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="143" y="439">
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
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545429" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="293" y="263">
      <linkto id="632399556409545431" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_callee</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: delete callee mms connection</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545430" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="292" y="443">
      <linkto id="632399556409545434" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: deleting caller MMS connection</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545431" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="367" y="247" mx="429" my="263">
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
    <node type="Action" id="632399556409545434" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="365" y="427" mx="427" my="443">
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
    <node type="Action" id="632399556409545437" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="530" y="429" mx="623" my="445">
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
    <node type="Action" id="632399556409545440" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="621" y="263">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632399556409545442" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="583.6172" y="567" mx="622" my="583">
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
    <node type="Action" id="632399556409545443" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="617" y="704">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632399556409545489" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="138" y="560">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: exiting function..</log>
      </Properties>
    </node>
    <node type="Variable" id="632399556409545427" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="userData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Failed" startnode="632399556409545381" treenode="632399556409545382" appnode="632399556409545379" handlerfor="632399556409545378">
    <node type="Start" id="632399556409545381" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="127">
      <linkto id="632399556409545444" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632399556409545444" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="125" y="125.166687">
      <linkto id="632399556409545445" type="Labeled" style="Bezier" ortho="true" label="callee" />
      <linkto id="632399556409545446" type="Labeled" style="Bezier" ortho="true" label="both" />
      <linkto id="632399556409545490" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545445" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="280" y="48.166687">
      <linkto id="632399556409545447" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_callee</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: delete callee mms connection</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545446" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="279" y="228.166687">
      <linkto id="632399556409545448" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: deleting caller MMS connection</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545447" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="354" y="32.166687" mx="416" my="48">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632399556409545450" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_callee</ap>
        <ap name="userData" type="literal">exit</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: Hanging up callee and exiting...</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545448" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="352" y="212.166687" mx="414" my="228">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632399556409545449" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: hanging up caller...</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545449" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="517" y="214.166687" mx="610" my="230">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632399556409545377" />
        <item text="OnPlayAnnouncement_Failed" treenode="632399556409545382" />
      </items>
      <linkto id="632399556409545450" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632399556409545451" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="connectionId" type="variable">g_connectionId_callee</ap>
        <ap name="filename" type="literal">cb_failure.wav</ap>
        <ap name="userData" type="literal">callee</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545450" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="608" y="48.166687">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632399556409545451" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="570.6172" y="352.1667" mx="609" my="368">
      <items count="1">
        <item text="ErrorHandler" />
      </items>
      <linkto id="632399556409545452" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">d</ap>
        <ap name="playTo" type="literal">callee</ap>
        <ap name="FunctionName" type="literal">ErrorHandler</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: PlayAnnouncement to caller failed, calling error handler...</log>
      </Properties>
    </node>
    <node type="Action" id="632399556409545452" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="604" y="489.1667">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632399556409545490" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="124.4707" y="348">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed: exiting function..</log>
      </Properties>
    </node>
    <node type="Variable" id="632399556409545468" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="userData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ErrorHandler" startnode="632399556409545350" treenode="632399556409545351" appnode="632399556409545348" handlerfor="632399556409545378">
    <node type="Start" id="632399556409545350" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="267">
      <linkto id="632405179098535217" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632399556409545355" text="1. Reject caller&#xD;&#xA;2. reject caller and his connection&#xD;&#xA;3. reject caller and both connections&#xD;&#xA;4. hang up callee, answer call already failed, delete both connections&#xD;&#xA;5. hang up both calls and both connections" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="108" y="32" />
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
    <node type="Label" id="632399556409545360" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="226" y="135" />
    <node type="Label" id="632399556409545361" text="b" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="281" y="187" />
    <node type="Label" id="632399556409545362" text="c" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="287" y="268" />
    <node type="Label" id="632399556409545363" text="d" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="277" y="335" />
    <node type="Label" id="632399556409545364" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="223" y="397" />
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
    <node type="Action" id="632399556409545383" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="418" y="1208" mx="511" my="1224">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632399556409545377" />
        <item text="OnPlayAnnouncement_Failed" treenode="632399556409545382" />
      </items>
      <linkto id="632399556409545398" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632399556409545405" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="connectionId" type="variable">connIdToUse</ap>
        <ap name="filename" type="literal">cb_failure.wav</ap>
        <ap name="userData" type="variable">playTo</ap>
      </Properties>
    </node>
    <node type="Label" id="632399556409545386" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="32" y="1221">
      <linkto id="632399556409545390" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632399556409545390" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="137" y="1221">
      <linkto id="632399556409545391" type="Labeled" style="Bezier" ortho="true" label="callee" />
      <linkto id="632399556409545392" type="Labeled" style="Bezier" ortho="true" label="both" />
      <linkto id="632399556409545398" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">playTo</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545391" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="326" y="1141">
      <linkto id="632399556409545383" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">g_connectionId_callee</ap>
        <rd field="ResultData">connIdToUse</rd>
      </Properties>
    </node>
    <node type="Action" id="632399556409545392" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="326" y="1223">
      <linkto id="632399556409545383" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">g_connectionId_caller</ap>
        <rd field="ResultData">connIdToUse</rd>
      </Properties>
    </node>
    <node type="Action" id="632399556409545398" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="79" y="1316" mx="141" my="1332">
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
    <node type="Action" id="632399556409545401" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="78" y="1456" mx="140" my="1472">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632399556409545404" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_callee</ap>
        <ap name="userData" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545404" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="276" y="1472">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545405" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="662" y="1224">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545409" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="96" y="921" mx="189" my="937">
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
        <ap name="userData" type="literal">callee</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545412" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="414" y="900">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632399556409545413" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="355" y="992" mx="417" my="1008">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632399315498735746" />
        <item text="OnHangup_Failed" treenode="632399315498735751" />
      </items>
      <linkto id="632399556409545412" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_callee</ap>
        <ap name="userData" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632405179098535217" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="138" y="268">
      <linkto id="632399556409545360" type="Labeled" style="Bezier" ortho="true" label="a" />
      <linkto id="632399556409545361" type="Labeled" style="Bezier" ortho="true" label="b" />
      <linkto id="632399556409545362" type="Labeled" style="Bezier" ortho="true" label="c" />
      <linkto id="632399556409545363" type="Labeled" style="Bezier" ortho="true" label="d" />
      <linkto id="632399556409545364" type="Labeled" style="Bezier" ortho="true" label="e" />
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