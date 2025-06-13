<Application name="AnswerCall" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="AnswerCall">
    <outline>
      <treenode type="evh" id="632841699350341625" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632841699350341622" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632841699350341621" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632841699350341633" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632841699350341630" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632841699350341629" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632841699350341755" actid="632841699350341639" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632841699350341638" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632841699350341635" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632841699350341634" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632841699350341756" actid="632841699350341639" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632841699350341653" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632841699350341650" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632841699350341649" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632841699350341761" actid="632841699350341664" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632841699350341658" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632841699350341655" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632841699350341654" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632841699350341762" actid="632841699350341664" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632841699350341663" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632841699350341660" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632841699350341659" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632841699350341763" actid="632841699350341664" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_in_callId" id="632841699350341747" vid="632841699350341642">
        <Properties type="String">g_in_callId</Properties>
      </treenode>
      <treenode text="g_confId" id="632841699350341749" vid="632841699350341644">
        <Properties type="String">g_confId</Properties>
      </treenode>
      <treenode text="g_dn" id="632841699350341751" vid="632841699350341717">
        <Properties type="String" initWith="dn">g_dn</Properties>
      </treenode>
      <treenode text="g_out_callId" id="632841699350341775" vid="632841699350341774">
        <Properties type="String">g_out_callId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632841699350341624" treenode="632841699350341625" appnode="632841699350341622" handlerfor="632841699350341621">
    <node type="Start" id="632841699350341624" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="38" y="136">
      <linkto id="632841699350341626" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632841699350341626" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="144" y="136">
      <linkto id="632841699350341639" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="Conference" type="literal">true</ap>
        <rd field="CallId">g_in_callId</rd>
        <rd field="ConferenceId">g_confId</rd>
      </Properties>
    </node>
    <node type="Action" id="632841699350341639" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="225" y="120" mx="278" my="136">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632841699350341633" />
        <item text="OnPlay_Failed" treenode="632841699350341638" />
      </items>
      <linkto id="632841699350341646" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">connecting.wav</ap>
        <ap name="ConferenceId" type="variable">g_confId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632841699350341646" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="419" y="136">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632841699350341627" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632841699350341632" treenode="632841699350341633" appnode="632841699350341630" handlerfor="632841699350341629">
    <node type="Start" id="632841699350341632" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="46" y="194">
      <linkto id="632841699350341664" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632841699350341664" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="142" y="178" mx="208" my="194">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632841699350341653" />
        <item text="OnMakeCall_Failed" treenode="632841699350341658" />
        <item text="OnRemoteHangup" treenode="632841699350341663" />
      </items>
      <linkto id="632841699350341770" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_dn</ap>
        <ap name="ProxyDTMFCallId" type="variable">g_in_callId</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_confId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_out_callId</rd>
      </Properties>
    </node>
    <node type="Action" id="632841699350341770" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="363" y="194">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632841699350341637" treenode="632841699350341638" appnode="632841699350341635" handlerfor="632841699350341634">
    <node type="Start" id="632841699350341637" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="52" y="186">
      <linkto id="632841699350341647" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632841699350341647" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="162" y="186">
      <linkto id="632841699350341648" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_in_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632841699350341648" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="273" y="186">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632841699350341652" treenode="632841699350341653" appnode="632841699350341650" handlerfor="632841699350341649">
    <node type="Start" id="632841699350341652" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="192">
      <linkto id="632841699350341773" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632841699350341773" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="163" y="192">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632841699350341657" treenode="632841699350341658" appnode="632841699350341655" handlerfor="632841699350341654">
    <node type="Start" id="632841699350341657" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="51" y="214">
      <linkto id="632841699350341771" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632841699350341771" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="167" y="214">
      <linkto id="632841699350341772" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_in_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632841699350341772" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="286" y="214">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" activetab="true" startnode="632841699350341662" treenode="632841699350341663" appnode="632841699350341660" handlerfor="632841699350341659">
    <node type="Start" id="632841699350341662" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="59" y="183">
      <linkto id="632841699350341776" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632841699350341776" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="163" y="183">
      <linkto id="632841699350341777" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_in_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632841699350341777" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="280" y="183">
      <linkto id="632841699350341778" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_out_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632841699350341778" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="395" y="183">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>