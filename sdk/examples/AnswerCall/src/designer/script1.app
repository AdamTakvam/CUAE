<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="633118297295156641" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="633118297295156638" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="633118297295156637" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
          <ep name="To" type="literal">2001</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="633118297295156652" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="633118297295156649" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="633118297295156648" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="633130290301723631" actid="633118297295156658" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633118297295156657" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="633118297295156654" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="633118297295156653" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="633130290301723632" actid="633118297295156658" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633118297295156671" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="633118297295156668" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="633118297295156667" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_CallId" id="633130290301723625" vid="633118297295156644">
        <Properties type="String">g_CallId</Properties>
      </treenode>
      <treenode text="g_ConnectionId" id="633130290301723627" vid="633118297295156646">
        <Properties type="String">g_ConnectionId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="633118297295156640" treenode="633118297295156641" appnode="633118297295156638" handlerfor="633118297295156637">
    <node type="Start" id="633118297295156640" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633118297295156642" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633118297295156642" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="170" y="32">
      <linkto id="633118297295156658" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">incomingCallId</ap>
        <rd field="CallId">g_CallId</rd>
        <rd field="ConnectionId">g_ConnectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="633118297295156658" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="317" y="16" mx="370" my="32">
      <items count="2">
        <item text="OnPlay_Complete" treenode="633118297295156652" />
        <item text="OnPlay_Failed" treenode="633118297295156657" />
      </items>
      <linkto id="633118297295156661" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">makecall_good_bye.wav</ap>
        <ap name="Prompt2" type="literal">makecall_good_bye.wav</ap>
        <ap name="ConnectionId" type="variable">g_ConnectionId</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="633118297295156661" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="586" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633118297295156643" name="incomingCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">incomingCallId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="633118297295156651" treenode="633118297295156652" appnode="633118297295156649" handlerfor="633118297295156648">
    <node type="Start" id="633118297295156651" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633118297295156662" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633118297295156662" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="175" y="32">
      <linkto id="633118297295156664" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="633118297295156665" type="Labeled" style="Bezier" ortho="true" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">terminationCondition</ap>
      </Properties>
    </node>
    <node type="Action" id="633118297295156663" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="450" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633118297295156664" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="309" y="32">
      <linkto id="633118297295156663" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_CallId</ap>
      </Properties>
    </node>
    <node type="Action" id="633118297295156665" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="176" y="178">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633118297295156673" name="terminationCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Play_Complete">terminationCondition</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="633118297295156656" treenode="633118297295156657" appnode="633118297295156654" handlerfor="633118297295156653">
    <node type="Start" id="633118297295156656" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633118297295156666" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633118297295156666" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="243" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="633118297295156670" treenode="633118297295156671" appnode="633118297295156668" handlerfor="633118297295156667">
    <node type="Start" id="633118297295156670" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633118297295156672" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633118297295156672" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="244" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>