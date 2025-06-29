<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632654273780720049" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632654273780720046" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632654273780720045" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/MakeCall</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632654273780720055" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632654273780720052" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632654273780720051" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="633087872057344184" actid="632654273780720066" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632654273780720060" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632654273780720057" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632654273780720056" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="633087872057344185" actid="632654273780720066" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632654273780720065" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632654273780720062" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632654273780720061" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="633087872057344186" actid="632654273780720066" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632654273780720115" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632654273780720112" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632654273780720111" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="633087872057344196" actid="632654273780720121" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632654273780720120" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632654273780720117" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632654273780720116" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="633087872057344197" actid="632654273780720121" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632654273780720132" level="2" text="Metreos.Providers.TimerFacility.TimerFire: OnTimerFire">
        <node type="function" name="OnTimerFire" id="632654273780720129" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632654273780720128" path="Metreos.Providers.TimerFacility.TimerFire" />
        <references>
          <ref id="633087872057344200" actid="632654273780720133" />
        </references>
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="632651848548273634" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632651848548273631" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632651848548273630" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632651848548273639" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632651848548273636" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632651848548273635" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632651848548273646" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632651848548273643" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632651848548273642" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="633054465557969229" level="2" text="Metreos.ApplicationControl.InstanceDestruction: OnInstanceDestruction">
        <node type="function" name="OnInstanceDestruction" id="633054465557969226" path="Metreos.StockTools" />
        <node type="event" name="InstanceDestruction" id="633054465557969225" path="Metreos.ApplicationControl.InstanceDestruction" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="633087872057344169" vid="632654273780720078">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_playFile" id="633087872057344171" vid="632654273780720126">
        <Properties type="Bool" initWith="playMedia">g_playFile</Properties>
      </treenode>
      <treenode text="g_talkTime" id="633087872057344173" vid="632654273780720190">
        <Properties type="Double" initWith="talkTime">g_talkTime</Properties>
      </treenode>
      <treenode text="g_timer" id="633087872057344175" vid="632654273780720192">
        <Properties type="String">g_timer</Properties>
      </treenode>
      <treenode text="g_to" id="633087872057344177" vid="632654273780720195">
        <Properties type="String">g_to</Properties>
      </treenode>
      <treenode text="g_remoteHost" id="633087872057344179" vid="632661005749269631">
        <Properties type="String">g_remoteHost</Properties>
      </treenode>
      <treenode text="g_answered" id="633087872057344181" vid="632661005749269639">
        <Properties type="Bool" defaultInitWith="false">g_answered</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632654273780720048" treenode="632654273780720049" appnode="632654273780720046" handlerfor="632654273780720045">
    <node type="Start" id="632654273780720048" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="127" y="38">
      <linkto id="632654273780720197" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632654273780720066" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="63" y="393" mx="129" my="409">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632654273780720055" />
        <item text="OnMakeCall_Failed" treenode="632654273780720060" />
        <item text="OnRemoteHangup" treenode="632654273780720065" />
      </items>
      <linkto id="632654273780720077" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632661005749269636" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_to</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_callId</rd>
      </Properties>
    </node>
    <node type="Action" id="632654273780720077" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="389" y="409">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632654273780720197" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="126" y="253">
      <linkto id="632654273780720066" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">queryParams["to"]</ap>
        <ap name="Value2" type="variable">remoteHost</ap>
        <rd field="ResultData">g_to</rd>
        <rd field="ResultData2">g_remoteHost</rd>
        <log condition="entry" on="true" level="Warning" type="csharp">String.Format("Locale: {0}", sessionData.Culture.DisplayName);</log>
      </Properties>
    </node>
    <node type="Action" id="632651848548273648" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="256" y="574">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632661005749269636" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="125.589188" y="574">
      <linkto id="632651848548273648" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">g_remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="literal">false</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <log condition="entry" on="true" level="Error" type="literal">MakeCall Provisional Failed</log>
      </Properties>
    </node>
    <node type="Variable" id="632654273780720050" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">queryParams</Properties>
    </node>
    <node type="Variable" id="632651848548273628" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632654273780720054" treenode="632654273780720055" appnode="632654273780720052" handlerfor="632654273780720051">
    <node type="Start" id="632654273780720054" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="350">
      <linkto id="632661005749269633" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632654273780720121" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="449" y="175" mx="502" my="191">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632654273780720115" />
        <item text="OnPlay_Failed" treenode="632654273780720120" />
      </items>
      <linkto id="632654273780720212" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632654273780720213" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="Prompt1" type="literal">welcome.wav</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632654273780720124" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="257" y="352">
      <linkto id="632654273780720121" type="Labeled" style="Bevel" ortho="true" label="true" />
      <linkto id="632654273780720133" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_playFile</ap>
      </Properties>
    </node>
    <node type="Action" id="632654273780720133" name="AddNonTriggerTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="345" y="486" mx="411" my="502">
      <items count="1">
        <item text="OnTimerFire" treenode="632654273780720132" />
      </items>
      <linkto id="632654273780720213" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerDateTime" type="csharp">DateTime.Now.AddMilliseconds(g_talkTime)</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="timerId">g_timer</rd>
      </Properties>
    </node>
    <node type="Action" id="632654273780720212" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="751" y="190">
      <linkto id="632654273780720214" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632654273780720213" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="635" y="385">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632654273780720214" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="815" y="239">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="csharp">"Play failed to :" + g_to</log>
      </Properties>
    </node>
    <node type="Action" id="632661005749269633" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="136" y="350">
      <linkto id="632661005749269638" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">g_remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="literal">true</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632661005749269638" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="136" y="458">
      <linkto id="632654273780720124" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <rd field="ResultData">g_answered</rd>
      </Properties>
    </node>
    <node type="Variable" id="632654273780720125" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632654273780720059" treenode="632654273780720060" appnode="632654273780720057" handlerfor="632654273780720056">
    <node type="Start" id="632654273780720059" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632661005749269634" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632654273780720194" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="516" y="303">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="csharp">"Failed call attempt to: " + g_to</log>
      </Properties>
    </node>
    <node type="Action" id="632661005749269634" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="102.589188" y="79">
      <linkto id="632654273780720194" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">g_remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="literal">false</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632654273780720064" treenode="632654273780720065" appnode="632654273780720062" handlerfor="632654273780720061">
    <node type="Start" id="632654273780720064" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632661005749269641" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632654273780720198" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="535" y="234">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632661005749269641" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="369" y="346">
      <linkto id="632654273780720198" type="Labeled" style="Bevel" ortho="true" label="true" />
      <linkto id="632661005749269642" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_answered</ap>
      </Properties>
    </node>
    <node type="Action" id="632661005749269642" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="518.5892" y="354">
      <linkto id="632654273780720198" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">g_remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="literal">false</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <log condition="entry" on="true" level="Error" type="literal">MakeCall Failed - indicated by Hangup before async MakeCall Response</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632654273780720114" treenode="632654273780720115" appnode="632654273780720112" handlerfor="632654273780720111">
    <node type="Start" id="632654273780720114" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632654273780720199" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632654273780720199" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="543" y="251">
      <linkto id="632654273780720200" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632654273780720200" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="763" y="293">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632654273780720119" treenode="632654273780720120" appnode="632654273780720117" handlerfor="632654273780720116">
    <node type="Start" id="632654273780720119" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632654273780720201" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632654273780720201" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="94.3528442" y="80">
      <linkto id="632654273780720202" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632654273780720202" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="275.352844" y="90">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnTimerFire" startnode="632654273780720131" treenode="632654273780720132" appnode="632654273780720129" handlerfor="632654273780720128">
    <node type="Start" id="632654273780720131" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="225">
      <linkto id="632654273780720205" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632654273780720205" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="55.3528442" y="48">
      <linkto id="632654273780720206" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632654273780720206" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="275.352844" y="90">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632651848548273633" treenode="632651848548273634" appnode="632651848548273631" handlerfor="632651848548273630">
    <node type="Start" id="632651848548273633" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632651848548273640" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632651848548273640" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="716" y="269">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632651848548273638" treenode="632651848548273639" appnode="632651848548273636" handlerfor="632651848548273635">
    <node type="Start" id="632651848548273638" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632651848548273641" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632651848548273641" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="623" y="325">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632651848548273645" treenode="632651848548273646" appnode="632651848548273643" handlerfor="632651848548273642">
    <node type="Start" id="632651848548273645" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632651848548273647" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632651848548273647" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="615" y="319">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnInstanceDestruction" startnode="633054465557969228" treenode="633054465557969229" appnode="633054465557969226" handlerfor="633054465557969225">
    <node type="Start" id="633054465557969228" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="53" y="162">
      <linkto id="633054465557969358" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633054465557969358" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="183.942047" y="165">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Warning" type="csharp">String.Format("Destructor called (errorCode={0}, errorText={1})", errorCode, errorText);</log>
      </Properties>
    </node>
    <node type="Variable" id="633054465557969360" name="errorCode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ErrorCode" refType="reference" name="Metreos.ApplicationControl.InstanceDestruction">errorCode</Properties>
    </node>
    <node type="Variable" id="633054465557969361" name="errorText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ErrorText" refType="reference" name="Metreos.ApplicationControl.InstanceDestruction">errorText</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>