<Application name="P2P_Call" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="P2P_Call">
    <outline>
      <treenode type="evh" id="632521942217209877" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632521942217209874" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632521942217209873" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632521943957366142" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632521943957366139" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632521943957366138" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632634098365652381" actid="632521943957366153" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632521943957366147" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632521943957366144" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632521943957366143" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632634098365652382" actid="632521943957366153" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632521943957366152" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632521943957366149" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632521943957366148" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632634098365652383" actid="632521943957366153" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_DN" id="632634098365652378" vid="632521943957366136">
        <Properties type="String" initWith="DN">g_DN</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632521942217209876" treenode="632521942217209877" appnode="632521942217209874" handlerfor="632521942217209873">
    <node type="Start" id="632521942217209876" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="38" y="220">
      <linkto id="632521943957366153" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632521943957366153" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="150" y="204" mx="216" my="220">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632521943957366142" />
        <item text="OnMakeCall_Failed" treenode="632521943957366147" />
        <item text="OnRemoteHangup" treenode="632521943957366152" />
      </items>
      <linkto id="632521943957366159" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_DN</ap>
        <ap name="From" type="variable">from</ap>
        <ap name="DisplayName" type="literal">P2P Call</ap>
        <ap name="PeerCallId" type="variable">callId</ap>
        <ap name="ProxyDTMFCallId" type="variable">callId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">out_CallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632521943957366159" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="402" y="220">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632521943957366157" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632521943957366163" name="out_CallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">out_CallId</Properties>
    </node>
    <node type="Variable" id="632634098365652393" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">from</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632521943957366141" treenode="632521943957366142" appnode="632521943957366139" handlerfor="632521943957366138">
    <node type="Start" id="632521943957366141" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="256">
      <linkto id="632521943957366162" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632521943957366162" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="163" y="256">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632521943957366146" treenode="632521943957366147" appnode="632521943957366144" handlerfor="632521943957366143">
    <node type="Start" id="632521943957366146" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="230">
      <linkto id="632521943957366161" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632521943957366161" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="163" y="230">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632521943957366151" treenode="632521943957366152" appnode="632521943957366149" handlerfor="632521943957366148">
    <node type="Start" id="632521943957366151" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="209">
      <linkto id="632521943957366160" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632521943957366160" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="164" y="212">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>