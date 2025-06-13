<Application name="InboundCall" trigger="Metreos.Providers.JTapi.JTapiIncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="InboundCall">
    <outline>
      <treenode type="evh" id="632640389962655981" level="1" text="Metreos.Providers.JTapi.JTapiIncomingCall (trigger): OnJTapiIncomingCall">
        <node type="function" name="OnJTapiIncomingCall" id="632640389962655978" path="Metreos.StockTools" />
        <node type="event" name="JTapiIncomingCall" id="632640389962655977" path="Metreos.Providers.JTapi.JTapiIncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnJTapiIncomingCall" activetab="true" startnode="632640389962655980" treenode="632640389962655981" appnode="632640389962655978" handlerfor="632640389962655977">
    <node type="Start" id="632640389962655980" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="137">
      <linkto id="632640389962656009" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632640389962656008" name="Forward" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="295" y="138">
      <Properties final="true" type="appControl" log="On">
        <ap name="ToGuid" type="variable">destGuid</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnJTapiIncomingCall: Forward from: " + routingGuid + "\nto: " + destGuid</log>
      </Properties>
    </node>
    <node type="Action" id="632640389962656009" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="156" y="138">
      <linkto id="632640389962656008" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="To" type="variable">to</ap>
        <ap name="OriginalTo" type="variable">originalTo</ap>
        <ap name="From" type="variable">from</ap>
        <ap name="CallType" type="literal">Incoming</ap>
        <ap name="LineId" type="variable">originalTo</ap>
        <ap name="EventName" type="literal">Metreos.Events.RecWithBarge.RecWithBargeCall</ap>
        <rd field="DestinationGuid">destGuid</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnJTapiIncomingCall: SendEvent(LineId = originalTo = " + originalTo + ")"</log>
      </Properties>
    </node>
    <node type="Variable" id="632640389962655996" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.Providers.JTapi.JTapiIncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632641167141715468" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.Providers.JTapi.JTapiIncomingCall">to</Properties>
    </node>
    <node type="Variable" id="632641167141715469" name="originalTo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="OriginalTo" refType="reference" name="Metreos.Providers.JTapi.JTapiIncomingCall">originalTo</Properties>
    </node>
    <node type="Variable" id="632641167141715470" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.Providers.JTapi.JTapiIncomingCall">from</Properties>
    </node>
    <node type="Variable" id="632641251906402716" name="destGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">destGuid</Properties>
    </node>
    <node type="Variable" id="632645798363016553" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>