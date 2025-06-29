<Application name="FailOver" trigger="Metreos.Providers.SccpProxy.RegisterTokenReq" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="FailOver">
    <outline>
      <treenode type="evh" id="632458804515449089" level="1" text="Metreos.Providers.SccpProxy.RegisterTokenReq (trigger): OnRegisterTokenReq">
        <node type="function" name="OnRegisterTokenReq" id="632458804515449086" path="Metreos.StockTools" />
        <node type="event" name="RegisterTokenReq" id="632458804515449085" path="Metreos.Providers.SccpProxy.RegisterTokenReq" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632458804515449104" level="2" text="Metreos.Providers.SccpProxy.SessionFailure: OnSessionFailure">
        <node type="function" name="OnSessionFailure" id="632458804515449101" path="Metreos.StockTools" />
        <node type="event" name="SessionFailure" id="632458804515449100" path="Metreos.Providers.SccpProxy.SessionFailure" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632458804515449109" level="2" text="Metreos.Providers.SccpProxy.RegisterTokenAck: OnRegisterTokenAck">
        <node type="function" name="OnRegisterTokenAck" id="632458804515449106" path="Metreos.StockTools" />
        <node type="event" name="RegisterTokenAck" id="632458804515449105" path="Metreos.Providers.SccpProxy.RegisterTokenAck" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632458804515449114" level="2" text="Metreos.Providers.SccpProxy.RegisterTokenReject: OnRegisterTokenReject">
        <node type="function" name="OnRegisterTokenReject" id="632458804515449111" path="Metreos.StockTools" />
        <node type="event" name="RegisterTokenReject" id="632458804515449110" path="Metreos.Providers.SccpProxy.RegisterTokenReject" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_sid" id="632459598713239446" vid="632458804515449090">
        <Properties type="String">g_sid</Properties>
      </treenode>
      <treenode text="g_ccmIP" id="632459598713239448" vid="632458804515449095">
        <Properties type="String" initWith="CallManagerIP">g_ccmIP</Properties>
      </treenode>
      <treenode text="g_ccmPort" id="632459598713239450" vid="632458804515449097">
        <Properties type="Int" initWith="CallManagerPort">g_ccmPort</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnRegisterTokenReq" activetab="true" startnode="632458804515449088" treenode="632458804515449089" appnode="632458804515449086" handlerfor="632458804515449085">
    <node type="Start" id="632458804515449088" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="56" y="404">
      <linkto id="632458804515449094" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632458804515449092" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="315" y="404">
      <linkto id="632458804515449099" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">sid</ap>
        <rd field="ResultData">g_sid</rd>
      </Properties>
    </node>
    <node type="Action" id="632458804515449094" name="RegisterTokenReq" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="172" y="404">
      <linkto id="632458804515449092" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Sid" type="variable">sid</ap>
        <ap name="ToIp" type="variable">g_ccmIP</ap>
        <ap name="ToPort" type="variable">g_ccmPort</ap>
      </Properties>
    </node>
    <node type="Action" id="632458804515449099" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="461" y="405">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632458804515449093" name="sid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Sid" refType="reference" name="Metreos.Providers.SccpProxy.RegisterTokenReq">sid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionFailure" startnode="632458804515449103" treenode="632458804515449104" appnode="632458804515449101" handlerfor="632458804515449100">
    <node type="Start" id="632458804515449103" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="112" y="355">
      <linkto id="632458804515449115" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632458804515449115" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="223" y="356">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Error" type="literal">OnSessionFailure: SessionFailure detected, exiting script</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRegisterTokenAck" startnode="632458804515449108" treenode="632458804515449109" appnode="632458804515449106" handlerfor="632458804515449105">
    <node type="Start" id="632458804515449108" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="67" y="414">
      <linkto id="632458804515449116" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632458804515449116" name="RegisterTokenAck" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="175" y="415">
      <linkto id="632458804515449117" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Sid" type="variable">g_sid</ap>
      </Properties>
    </node>
    <node type="Action" id="632458804515449117" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="311" y="417">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRegisterTokenReject" startnode="632458804515449113" treenode="632458804515449114" appnode="632458804515449111" handlerfor="632458804515449110">
    <node type="Start" id="632458804515449113" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="60" y="297">
      <linkto id="632458804515449119" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632458804515449119" name="RegisterTokenReject" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="185" y="298">
      <linkto id="632458804515449120" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Sid" type="variable">g_sid</ap>
      </Properties>
    </node>
    <node type="Action" id="632458804515449120" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="320" y="296">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>