<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="633081729937813292" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="633081729937813289" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="633081729937813288" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="633081818935625426" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="633081818935625423" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="633081818935625422" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="633081925946094159" actid="633081818935625432" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633081818935625431" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="633081818935625428" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="633081818935625427" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="633081925946094160" actid="633081818935625432" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633081818935625447" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="633081818935625444" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="633081818935625443" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_conferenceId" id="633081925946094140" vid="633081729937813341">
        <Properties type="String">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_bargeConnId" id="633081925946094142" vid="633081729937813351">
        <Properties type="String">g_bargeConnId</Properties>
      </treenode>
      <treenode text="bargeNumber" id="633081925946094144" vid="633081729937813358">
        <Properties type="String" initWith="Config.SharedLineDN">bargeNumber</Properties>
      </treenode>
      <treenode text="g_mmIP" id="633081925946094146" vid="633081729937813363">
        <Properties type="String">g_mmIP</Properties>
      </treenode>
      <treenode text="g_mmsPort" id="633081925946094148" vid="633081729937813365">
        <Properties type="UInt">g_mmsPort</Properties>
      </treenode>
      <treenode text="g_bargedCallId" id="633081925946094150" vid="633081729937813423">
        <Properties type="String">g_bargedCallId</Properties>
      </treenode>
      <treenode text="g_incomingCallId" id="633081925946094152" vid="633081818935625435">
        <Properties type="String">g_incomingCallId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="633081729937813291" treenode="633081729937813292" appnode="633081729937813289" handlerfor="633081729937813288">
    <node type="Start" id="633081729937813291" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="231">
      <linkto id="633081729937813339" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633081729937813339" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="166" y="232">
      <linkto id="633081818935625438" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="633081925946094177" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">incomingCallId</ap>
        <ap name="DisplayName" type="literal">Barge</ap>
        <ap name="Conference" type="literal">true</ap>
        <rd field="CallId">g_incomingCallId</rd>
        <rd field="MmsId">mmsId</rd>
        <rd field="ConferenceId">g_conferenceId</rd>
        <rd field="MediaRxIP">g_mmIP</rd>
        <rd field="MediaRxPort">g_mmsPort</rd>
      </Properties>
    </node>
    <node type="Action" id="633081729937813344" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="853" y="232">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633081818935625420" name="Barge" class="MaxActionNode" group="" path="Metreos.CallControl" x="510" y="235">
      <linkto id="633081818935625421" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="633081818935625432" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="From" type="variable">bargeNumber</ap>
        <ap name="MediaRxIP" type="variable">g_mmIP</ap>
        <ap name="MediaRxPort" type="variable">g_mmsPort</ap>
      </Properties>
    </node>
    <node type="Action" id="633081818935625421" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="633" y="235">
      <linkto id="633081729937813344" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="633081818935625432" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_bargeConnId</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="633081818935625432" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="518" y="356" mx="571" my="372">
      <items count="2">
        <item text="OnPlay_Complete" treenode="633081818935625426" />
        <item text="OnPlay_Failed" treenode="633081818935625431" />
      </items>
      <linkto id="633081818935625437" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="Prompt1" type="literal">good_bye.wav</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="633081818935625437" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="568" y="538">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633081818935625438" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="166" y="353">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633081925946094177" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="344" y="235">
      <linkto id="633081818935625420" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="633081818935625432" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxIP" type="literal">1.1.1.1</ap>
        <ap name="MediaTxPort" type="literal">254</ap>
        <rd field="MediaRxIP">g_mmIP</rd>
        <rd field="MediaRxPort">g_mmsPort</rd>
        <rd field="ConnectionId">g_bargeConnId</rd>
      </Properties>
    </node>
    <node type="Variable" id="633081729937813340" name="incomingCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">incomingCallId</Properties>
    </node>
    <node type="Variable" id="633081729937813343" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">from</Properties>
    </node>
    <node type="Variable" id="633081925946094176" name="mmsId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">mmsId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="633081818935625425" treenode="633081818935625426" appnode="633081818935625423" handlerfor="633081818935625422">
    <node type="Start" id="633081818935625425" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="176">
      <linkto id="633081818935625440" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633081818935625440" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="185" y="184">
      <linkto id="633081818935625441" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_bargeConnId</ap>
      </Properties>
    </node>
    <node type="Action" id="633081818935625441" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="329" y="189">
      <linkto id="633081818935625442" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="633081818935625442" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="457" y="194">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="633081818935625430" treenode="633081818935625431" appnode="633081818935625428" handlerfor="633081818935625427">
    <node type="Start" id="633081818935625430" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633081818935625439" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633081818935625439" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="137" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="633081818935625446" treenode="633081818935625447" appnode="633081818935625444" handlerfor="633081818935625443">
    <node type="Start" id="633081818935625446" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="169">
      <linkto id="633081818935625448" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633081818935625448" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="184.076172" y="160">
      <linkto id="633081818935625449" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_bargeConnId</ap>
      </Properties>
    </node>
    <node type="Action" id="633081818935625449" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="324.076172" y="163">
      <linkto id="633081818935625450" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="633081818935625450" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="456.076172" y="170">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>