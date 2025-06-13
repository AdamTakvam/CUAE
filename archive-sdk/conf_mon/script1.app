<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632763944396562855" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632763944396562852" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632763944396562851" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632767408473750406" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632767408473750403" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632767408473750402" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632767408473750464" actid="632767408473750417" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632767408473750411" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632767408473750408" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632767408473750407" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632767408473750465" actid="632767408473750417" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632767408473750416" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632767408473750413" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632767408473750412" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632767408473750466" actid="632767408473750417" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632767408473750480" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632767408473750477" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632767408473750476" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632767408473750487" actid="632767408473750486" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632767408473750485" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632767408473750482" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632767408473750481" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632767408473750488" actid="632767408473750486" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="632767408473750447" vid="632763944396562858">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632767408473750449" vid="632763944396562860">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_confId" id="632767408473750451" vid="632763944396562862">
        <Properties type="String">g_confId</Properties>
      </treenode>
      <treenode text="g_mmsId" id="632767408473750453" vid="632763944396562864">
        <Properties type="String">g_mmsId</Properties>
      </treenode>
      <treenode text="g_callId1" id="632767408473750455" vid="632767408473750421">
        <Properties type="String">g_callId1</Properties>
      </treenode>
      <treenode text="g_monOnlyDN" id="632767408473750459" vid="632767408473750425">
        <Properties type="String" initWith="monOnlyDN">g_monOnlyDN</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632763944396562854" treenode="632763944396562855" appnode="632763944396562852" handlerfor="632763944396562851">
    <node type="Start" id="632763944396562854" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="82">
      <linkto id="632763944396562856" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632763944396562856" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="181" y="82">
      <linkto id="632767408473750486" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="WaitForMedia" type="csharp">true</ap>
        <ap name="Conference" type="literal">true</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="MmsId">g_mmsId</rd>
        <rd field="ConnectionId">g_connectionId</rd>
        <rd field="ConferenceId">g_confId</rd>
      </Properties>
    </node>
    <node type="Action" id="632763944396562892" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="626" y="80">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632767408473750417" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="436" y="64" mx="502" my="80">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632767408473750406" />
        <item text="OnMakeCall_Failed" treenode="632767408473750411" />
        <item text="OnRemoteHangup" treenode="632767408473750416" />
      </items>
      <linkto id="632763944396562892" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_monOnlyDN</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_callId1</rd>
      </Properties>
    </node>
    <node type="Action" id="632767408473750486" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="280" y="65" mx="333" my="81">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632767408473750480" />
        <item text="OnPlay_Failed" treenode="632767408473750485" />
      </items>
      <linkto id="632767408473750417" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sample_music.wav</ap>
        <ap name="ConferenceId" type="variable">g_confId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Variable" id="632763944396562857" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632767408473750405" treenode="632767408473750406" appnode="632767408473750403" handlerfor="632767408473750402">
    <node type="Start" id="632767408473750405" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="52" y="93">
      <linkto id="632767408473750474" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632767408473750474" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="225" y="93">
      <linkto id="632767408473750489" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Monitor" type="literal">true</ap>
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="ConferenceId" type="variable">g_confId</ap>
      </Properties>
    </node>
    <node type="Action" id="632767408473750489" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="411" y="94">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632767408473750492" name="connId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" activetab="true" startnode="632767408473750410" treenode="632767408473750411" appnode="632767408473750408" handlerfor="632767408473750407">
    <node type="Start" id="632767408473750410" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="88">
      <linkto id="632767408473750473" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632767408473750473" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="189" y="89">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632767408473750415" treenode="632767408473750416" appnode="632767408473750413" handlerfor="632767408473750412">
    <node type="Start" id="632767408473750415" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="39" y="98">
      <linkto id="632767408473750472" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632767408473750472" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="197" y="96">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632767408473750479" treenode="632767408473750480" appnode="632767408473750477" handlerfor="632767408473750476">
    <node type="Start" id="632767408473750479" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="83">
      <linkto id="632767408473750490" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632767408473750490" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="193" y="83">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632767408473750484" treenode="632767408473750485" appnode="632767408473750482" handlerfor="632767408473750481">
    <node type="Start" id="632767408473750484" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="79">
      <linkto id="632767408473750491" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632767408473750491" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="195" y="78">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>