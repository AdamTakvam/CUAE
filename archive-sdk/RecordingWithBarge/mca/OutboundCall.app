<Application name="OutboundCall" trigger="Metreos.Providers.JTapi.JTapiCallInitiated" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="OutboundCall">
    <outline>
      <treenode type="evh" id="632640389962655988" level="1" text="Metreos.Providers.JTapi.JTapiCallInitiated (trigger): OnJTapiCallInitiated">
        <node type="function" name="OnJTapiCallInitiated" id="632640389962655985" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallInitiated" id="632640389962655984" path="Metreos.Providers.JTapi.JTapiCallInitiated" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnJTapiCallInitiated" activetab="true" startnode="632640389962655987" treenode="632640389962655988" appnode="632640389962655985" handlerfor="632640389962655984">
    <node type="Start" id="632640389962655987" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="118" y="272">
      <linkto id="632642785229682419" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632642785229682418" name="Forward" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="387.6172" y="275">
      <Properties final="true" type="appControl" log="On">
        <ap name="ToGuid" type="variable">destGuid</ap>
      </Properties>
    </node>
    <node type="Action" id="632642785229682419" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="237.617188" y="273">
      <linkto id="632642785229682418" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="To" type="variable">to</ap>
        <ap name="OriginalTo" type="variable">originalTo</ap>
        <ap name="From" type="variable">from</ap>
        <ap name="CallType" type="literal">Outgoing</ap>
        <ap name="LineId" type="variable">from</ap>
        <ap name="EventName" type="literal">Metreos.Events.RecWithBarge.RecWithBargeCall</ap>
        <rd field="DestinationGuid">destGuid</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnJTapiCallInitiated: SendEvent(LineId = from = " + from + ")"
</log>
      </Properties>
    </node>
    <node type="Variable" id="632640389962655996" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInitiated">callId</Properties>
    </node>
    <node type="Variable" id="632641167141715468" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInitiated">to</Properties>
    </node>
    <node type="Variable" id="632641167141715469" name="originalTo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInitiated">originalTo</Properties>
    </node>
    <node type="Variable" id="632641167141715470" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.Providers.JTapi.JTapiIncomingCall">from</Properties>
    </node>
    <node type="Variable" id="632641251906402716" name="destGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">destGuid</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>