<Application name="JoinConf" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="JoinConf">
    <outline>
      <treenode type="evh" id="633156903708231137" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="633156903708231134" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633156903708231133" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/JoinConf</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="633156903708231142" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="633156903708231139" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="633156903708231138" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="633159432699289970" actid="633156903708231153" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633156903708231147" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="633156903708231144" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="633156903708231143" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="633159432699289971" actid="633156903708231153" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633156903708231152" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="633156903708231149" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="633156903708231148" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="633159432699289972" actid="633156903708231153" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_CallId" id="633159432699289963" vid="633156903708231157">
        <Properties type="String">g_CallId</Properties>
      </treenode>
      <treenode text="g_ConnId" id="633159432699289965" vid="633156903708231160">
        <Properties type="String">g_ConnId</Properties>
      </treenode>
      <treenode text="g_ConfId" id="633159432699289967" vid="633157044661810840">
        <Properties type="String">g_ConfId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="633156903708231136" treenode="633156903708231137" appnode="633156903708231134" handlerfor="633156903708231133">
    <node type="Start" id="633156903708231136" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="302">
      <linkto id="633156903708231153" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633156903708231153" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="46" y="286" mx="112" my="302">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="633156903708231142" />
        <item text="OnMakeCall_Failed" treenode="633156903708231147" />
        <item text="OnRemoteHangup" treenode="633156903708231152" />
      </items>
      <linkto id="633157044661810857" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="633157044661810858" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="literal">210116</ap>
        <ap name="From" type="literal">210001</ap>
        <ap name="WaitForMedia" type="literal">TxRx</ap>
        <ap name="Conference" type="literal">True</ap>
        <ap name="ConferenceId" type="literal">0</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
        <rd field="CallId">g_CallId</rd>
      </Properties>
    </node>
    <node type="Action" id="633157044661810851" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="464" y="299">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633157044661810855" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="312" y="554">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633157044661810857" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="263" y="301">
      <linkto id="633157044661810851" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">RemoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">ok</ap>
        <ap name="body" type="literal">Multicasting started successfully!!!</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="633157044661810858" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="106" y="555">
      <linkto id="633157044661810855" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">RemoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">ok</ap>
        <ap name="body" type="literal">The call could not be placed!!!</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Variable" id="633157044661810856" name="RemoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">RemoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="633156903708231141" treenode="633156903708231142" appnode="633156903708231139" handlerfor="633156903708231138">
    <node type="Start" id="633156903708231141" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="53" y="244">
      <linkto id="633157044661810839" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633156903708231167" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="815" y="242">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633157044661810839" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="136" y="245">
      <linkto id="633157044661810844" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">ConfId</ap>
        <rd field="ResultData">g_ConfId</rd>
      </Properties>
    </node>
    <node type="Action" id="633157044661810843" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="376" y="244">
      <linkto id="633157044661810846" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaRxCodec" type="literal">G711u</ap>
        <ap name="MediaRxFramesize" type="literal">20</ap>
        <ap name="ConnectionId" type="variable">g_ConnId</ap>
        <ap name="ConferenceId" type="variable">g_ConfId</ap>
        <ap name="MediaTxIP" type="literal">234.10.10.10</ap>
        <ap name="MediaTxPort" type="literal">22480</ap>
        <ap name="MediaTxCodec" type="literal">G711u</ap>
        <ap name="MediaTxFramesize" type="literal">20</ap>
        <ap name="CallId" type="variable">g_CallId</ap>
        <rd field="ConnectionId">g_ConnId</rd>
      </Properties>
    </node>
    <node type="Action" id="633157044661810844" name="ReserveConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="258.590485" y="244">
      <linkto id="633157044661810843" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaRxCodec" type="literal">G711u</ap>
        <ap name="MediaTxCodec" type="literal">G711u</ap>
        <rd field="ConnectionId">g_ConnId</rd>
      </Properties>
    </node>
    <node type="Action" id="633157044661810846" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="516.0983" y="242">
      <linkto id="633157044661810848" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">RTPMRx:234.10.10.10:22480:100</ap>
        <ap name="Priority1" type="literal">0</ap>
        <rd field="ResultData">IpExec</rd>
      </Properties>
    </node>
    <node type="Action" id="633157044661810848" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="642.6582" y="242">
      <linkto id="633156903708231167" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">IpExec.ToString()</ap>
        <ap name="URL" type="literal">10.77.34.231</ap>
        <ap name="Username" type="literal">kartkuma</ap>
        <ap name="Password" type="literal">cisco123</ap>
      </Properties>
    </node>
    <node type="Variable" id="633157044661810838" name="ConfId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConferenceId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">ConfId</Properties>
    </node>
    <node type="Variable" id="633157044661810854" name="IpExec" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">IpExec</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="633156903708231146" treenode="633156903708231147" appnode="633156903708231144" handlerfor="633156903708231143">
    <node type="Start" id="633156903708231146" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="105" y="294">
      <linkto id="633156903708231168" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633156903708231168" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="369" y="291">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="633156903708231151" treenode="633156903708231152" appnode="633156903708231149" handlerfor="633156903708231148">
    <node type="Start" id="633156903708231151" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="161" y="314">
      <linkto id="633157044661810859" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633156903708231170" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="536" y="312">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633157044661810859" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="331" y="312">
      <linkto id="633156903708231170" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_ConnId</ap>
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>