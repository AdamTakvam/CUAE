<Application name="CreateBridge" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="CreateBridge">
    <outline>
      <treenode type="evh" id="632540148486924088" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632540148486924085" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632540148486924084" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632540148486924103" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632540148486924100" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632540148486924099" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632887426622187872" actid="632540148486924114" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632540148486924108" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632540148486924105" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632540148486924104" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632887426622187873" actid="632540148486924114" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632540148486924113" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632540148486924110" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632540148486924109" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632887426622187874" actid="632540148486924114" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_DN" id="632887426622187869" vid="632540148486924118">
        <Properties type="String" initWith="DN">g_DN</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632540148486924087" treenode="632540148486924088" appnode="632540148486924085" handlerfor="632540148486924084">
    <node type="Start" id="632540148486924087" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="181">
      <linkto id="632540148486924114" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632540148486924114" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="112" y="165" mx="178" my="181">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632540148486924103" />
        <item text="OnMakeCall_Failed" treenode="632540148486924108" />
        <item text="OnRemoteHangup" treenode="632540148486924113" />
      </items>
      <linkto id="632540148486924120" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632540148486924121" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_DN</ap>
        <ap name="PeerCallId" type="variable">callId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632540148486924120" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="174" y="346">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632540148486924121" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="339" y="181">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632540148486924089" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" activetab="true" startnode="632540148486924102" treenode="632540148486924103" appnode="632540148486924100" handlerfor="632540148486924099">
    <node type="Start" id="632540148486924102" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="189">
      <linkto id="632887426622187887" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632540148486924122" name="BridgeCalls" class="MaxActionNode" group="" path="Metreos.CallControl" x="276" y="189">
      <linkto id="632540148486924124" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632540148486924124" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="414" y="188">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632866570987888690" text="6/22/06: Warren Yetman| Removed 10 second &#xD;&#xA;sleep action prior to BridgeCalls action per suggestion made by Seth Call." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="117" />
    <node type="Action" id="632887426622187887" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="142" y="189">
      <linkto id="632540148486924122" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">10000</ap>
      </Properties>
    </node>
    <node type="Variable" id="632540148486924123" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632540148486924107" treenode="632540148486924108" appnode="632540148486924105" handlerfor="632540148486924104">
    <node type="Start" id="632540148486924107" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="156">
      <linkto id="632540148486924125" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632540148486924125" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="139" y="159">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632540148486924112" treenode="632540148486924113" appnode="632540148486924110" handlerfor="632540148486924109">
    <node type="Start" id="632540148486924112" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="187">
      <linkto id="632540148486924126" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632540148486924126" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="126" y="187">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>