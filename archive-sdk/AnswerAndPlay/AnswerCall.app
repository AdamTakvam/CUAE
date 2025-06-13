<Application name="AnswerCall" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="AnswerCall">
    <outline>
      <treenode type="evh" id="632536737987951441" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632536737987951438" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632536737987951437" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632536737987951447" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632536737987951444" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632536737987951443" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632570485099856208" actid="632570485099856204" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632536737987951452" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632536737987951449" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632536737987951448" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632570485099856209" actid="632570485099856204" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632570485099856180" level="2" text="Metreos.CallControl.CallChanged: OnCallChanged">
        <node type="function" name="OnCallChanged" id="632570485099856177" path="Metreos.StockTools" />
        <node type="event" name="CallChanged" id="632570485099856176" path="Metreos.CallControl.CallChanged" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632570485099856196" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632570485099856193" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632570485099856192" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632570485099856201" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632570485099856198" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632570485099856197" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632570485099856216" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632570485099856213" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632570485099856212" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_CallId" id="632570485099856159" vid="632536737987951466">
        <Properties type="String">g_CallId</Properties>
      </treenode>
      <treenode text="g_ConnId" id="632570485099856184" vid="632570485099856183">
        <Properties type="String">g_ConnId</Properties>
      </treenode>
      <treenode text="g_CallChanged" id="632570485099856220" vid="632570485099856219">
        <Properties type="Bool" defaultInitWith="false">g_CallChanged</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632536737987951440" treenode="632536737987951441" appnode="632536737987951438" handlerfor="632536737987951437">
    <node type="Start" id="632536737987951440" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="215">
      <linkto id="632536737987951442" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632536737987951442" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="130" y="215">
      <linkto id="632536737987951458" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632536737987951459" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <rd field="CallId">g_CallId</rd>
        <rd field="ConnectionId">g_ConnId</rd>
      </Properties>
    </node>
    <node type="Action" id="632536737987951458" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="129" y="369">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632536737987951459" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="407" y="215">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632536737987951464" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" activetab="true" startnode="632536737987951446" treenode="632536737987951447" appnode="632536737987951444" handlerfor="632536737987951443">
    <node type="Start" id="632536737987951446" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="163">
      <linkto id="632570485099856223" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632570485099856223" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="149" y="163">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632536737987951451" treenode="632536737987951452" appnode="632536737987951449" handlerfor="632536737987951448">
    <node type="Start" id="632536737987951451" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="172">
      <linkto id="632536737987951460" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632536737987951460" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="145" y="172">
      <linkto id="632536737987951461" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_CallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632536737987951461" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="255" y="172">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnCallChanged" startnode="632570485099856179" treenode="632570485099856180" appnode="632570485099856177" handlerfor="632570485099856176">
    <node type="Start" id="632570485099856179" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="183">
      <linkto id="632570485099856221" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632570485099856203" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="328" y="182">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632570485099856221" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="157" y="181">
      <linkto id="632570485099856203" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_CallChanged</rd>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632570485099856195" treenode="632570485099856196" appnode="632570485099856193" handlerfor="632570485099856192">
    <node type="Start" id="632570485099856195" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="129">
      <linkto id="632570485099856202" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632570485099856202" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="129" y="128">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632570485099856200" treenode="632570485099856201" appnode="632570485099856198" handlerfor="632570485099856197">
    <node type="Start" id="632570485099856200" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="179">
      <linkto id="632570485099856204" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632570485099856204" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="106" y="162.166656" mx="159" my="178">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632536737987951447" />
        <item text="OnPlay_Failed" treenode="632536737987951452" />
      </items>
      <linkto id="632570485099856222" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="AudioFileEncoding" type="literal">ulaw</ap>
        <ap name="Prompt1" type="literal">welcome.wav</ap>
        <ap name="ConnectionId" type="variable">g_ConnId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632570485099856222" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="312" y="178">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632570485099856215" treenode="632570485099856216" appnode="632570485099856213" handlerfor="632570485099856212">
    <node type="Start" id="632570485099856215" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="55" y="179">
      <linkto id="632570485099856217" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632570485099856217" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="176" y="179">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>