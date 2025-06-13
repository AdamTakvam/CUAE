<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="633156221096304710" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="633156221096304707" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633156221096304706" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/CreateConf</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="633156221096304751" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="633156221096304748" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="633156221096304747" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="633156861093365390" actid="633156221096304762" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633156221096304756" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="633156221096304753" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="633156221096304752" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="633156861093365391" actid="633156221096304762" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633156221096304761" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="633156221096304758" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="633156221096304757" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="633156861093365392" actid="633156221096304762" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_ConnId" id="633156861093365378" vid="633156221096304711">
        <Properties type="String">g_ConnId</Properties>
      </treenode>
      <treenode text="g_ConfId" id="633156861093365380" vid="633156221096304713">
        <Properties type="String">g_ConfId</Properties>
      </treenode>
      <treenode text="g_CallId" id="633156861093365382" vid="633156221096304715">
        <Properties type="String">g_CallId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="633156221096304709" treenode="633156221096304710" appnode="633156221096304707" handlerfor="633156221096304706">
    <node type="Start" id="633156221096304709" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="230">
      <linkto id="633156221096304717" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633156221096304717" name="ReserveConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="130" y="228">
      <linkto id="633156221096304718" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="633156861093365427" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaRxCodec" type="literal">G711u</ap>
        <ap name="MediaTxCodec" type="literal">G711u</ap>
        <rd field="ResultCode">ResultCode</rd>
        <rd field="ConnectionId">g_ConnId</rd>
      </Properties>
    </node>
    <node type="Action" id="633156221096304718" name="CreateConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="249" y="226">
      <linkto id="633156221096304720" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxCodec" type="literal">G711u</ap>
        <ap name="MediaTxFramesize" type="literal">20</ap>
        <ap name="ConnectionId" type="variable">g_ConnId</ap>
        <ap name="MediaRxCodec" type="literal">G711u</ap>
        <ap name="MediaRxFramesize" type="literal">20</ap>
        <ap name="MediaTxIP" type="literal">234.10.10.10</ap>
        <ap name="MediaTxPort" type="literal">22480</ap>
        <rd field="ConferenceId">g_ConfId</rd>
      </Properties>
    </node>
    <node type="Action" id="633156221096304720" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="364" y="222">
      <linkto id="633156221096304721" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">RTPMRx:234.10.10.10:22480:100</ap>
        <ap name="Priority1" type="literal">0</ap>
        <rd field="ResultData">IpExec</rd>
      </Properties>
    </node>
    <node type="Action" id="633156221096304721" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="462" y="222">
      <linkto id="633156221096304762" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">IpExec.ToString()</ap>
        <ap name="URL" type="literal">10.77.34.231</ap>
        <ap name="Username" type="literal">kartkuma</ap>
        <ap name="Password" type="literal">cisco123</ap>
      </Properties>
    </node>
    <node type="Action" id="633156221096304746" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="832" y="220">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633156221096304762" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="569" y="205" mx="635" my="221">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="633156221096304751" />
        <item text="OnMakeCall_Failed" treenode="633156221096304756" />
        <item text="OnRemoteHangup" treenode="633156221096304761" />
      </items>
      <linkto id="633156861093365422" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="633156861093365430" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="literal">210116</ap>
        <ap name="From" type="literal">210001</ap>
        <ap name="Conference" type="literal">True</ap>
        <ap name="ConferenceId" type="variable">g_ConfId</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
        <rd field="CallId">g_CallId</rd>
      </Properties>
    </node>
    <node type="Action" id="633156861093365422" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="732" y="220">
      <linkto id="633156221096304746" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">RemoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">ok</ap>
        <ap name="body" type="literal">Call Placed Successfully!!!</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="633156861093365427" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="127" y="443">
      <linkto id="633156861093365429" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">RemoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">ok</ap>
        <ap name="body" type="variable">ResultCode</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="633156861093365429" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="367" y="440">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633156861093365430" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="632" y="438">
      <linkto id="633156861093365431" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">RemoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">ok</ap>
        <ap name="body" type="literal">Call could not be placed!!!</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="633156861093365431" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="781" y="437">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633156221096304719" name="IpExec" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">IpExec</Properties>
    </node>
    <node type="Variable" id="633156861093365425" name="RemoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">RemoteHost</Properties>
    </node>
    <node type="Variable" id="633156861093365428" name="ResultCode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">ResultCode</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="633156221096304750" treenode="633156221096304751" appnode="633156221096304748" handlerfor="633156221096304747">
    <node type="Start" id="633156221096304750" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="66" y="255">
      <linkto id="633156221096304767" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633156221096304767" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="291" y="252">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="633156221096304755" treenode="633156221096304756" appnode="633156221096304753" handlerfor="633156221096304752">
    <node type="Start" id="633156221096304755" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="82" y="254">
      <linkto id="633156221096304768" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633156221096304768" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="354" y="260">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="633156221096304760" treenode="633156221096304761" appnode="633156221096304758" handlerfor="633156221096304757">
    <node type="Start" id="633156221096304760" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="101" y="276">
      <linkto id="633156861093365421" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633156221096304769" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="371" y="276">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633156861093365421" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="236" y="276">
      <linkto id="633156221096304769" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_ConnId</ap>
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>