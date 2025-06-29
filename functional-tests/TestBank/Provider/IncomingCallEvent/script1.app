<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632364804899375200" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632364804899375197" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632364804899375196" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632593816798517179" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632593816798517176" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632593816798517175" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632593816798517155" vid="632364804899375217">
        <Properties type="String" defaultInitWith="Provider.IncomingCallEvent.script1.S_Simple" initWith="S_Simple">S_Simple</Properties>
      </treenode>
      <treenode text="g_CallId" id="632593816798517157" vid="632413169871914689">
        <Properties type="String">g_CallId</Properties>
      </treenode>
      <treenode text="S_Hangup" id="632593816798517164" vid="632593816798517163">
        <Properties type="String" initWith="S_Hangup">S_Hangup</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632364804899375199" treenode="632364804899375200" appnode="632364804899375197" handlerfor="632364804899375196">
    <node type="Start" id="632364804899375199" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="38" y="362">
      <linkto id="632413169871914688" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632380359791875196" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="286" y="362">
      <linkto id="632593816798517165" type="Labeled" style="Bevel" label="Success" />
      <linkto id="632593816798517166" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <rd field="ConnectionId">connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632413169871914688" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="152" y="363">
      <linkto id="632380359791875196" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">callId</ap>
        <rd field="ResultData">g_CallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632593816798517165" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="445" y="360">
      <linkto id="632593816798517168" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632593816798517166" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="286" y="517">
      <linkto id="632593816798517169" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632593816798517168" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="595" y="360">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632593816798517169" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="288" y="622">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632380359791875198" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632413169871914684" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632593816798517178" treenode="632593816798517179" appnode="632593816798517176" handlerfor="632593816798517175">
    <node type="Start" id="632593816798517178" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="366">
      <linkto id="632593816798517180" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632593816798517180" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="277" y="369">
      <linkto id="632593816798517181" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Hangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632593816798517181" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="534" y="374">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>