<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632580609502868875" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632144263780000139" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632144263780000138" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.Forward.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632580609502868876" level="1" text="Metreos.Providers.FunctionalTest.Event: ForwardTest">
        <node type="function" name="ForwardTest" id="632144263780000151" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632144263780000150" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">ARE.Forward.script1.E_ForwardTest</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632580609502868877" level="1" text="Metreos.Providers.FunctionalTest.Event: ForwardTo">
        <node type="function" name="ForwardTo" id="632144263780000147" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632144263780000146" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">ARE.Forward.script1.E_ForwardTo</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Script1Load" id="632580609502868894" vid="632144263780000144">
        <Properties type="Metreos.Types.String" initWith="S_Script1Load">S_Script1Load</Properties>
      </treenode>
      <treenode text="S_ForwardTest" id="632580609502868896" vid="632144263780000158">
        <Properties type="Metreos.Types.String" initWith="S_ForwardTest">S_ForwardTest</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632144263780000140" treenode="632580609502868875" appnode="632144263780000139" handlerfor="632144263780000138">
    <node type="Start" id="632144263780000140" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632144263780000143" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632144263780000143" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="275" y="229">
      <linkto id="632224796097968996" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Script1Load</ap>
      </Properties>
    </node>
    <node type="Action" id="632224796097968996" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="495" y="226">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ForwardTest" startnode="632144263780000152" treenode="632580609502868876" appnode="632144263780000151" handlerfor="632144263780000150">
    <node type="Start" id="632144263780000152" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632144263780000160" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632144263780000160" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="159" y="179">
      <linkto id="632224796097968997" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_ForwardTest</ap>
      </Properties>
    </node>
    <node type="Action" id="632224796097968997" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="415" y="183">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ForwardTo" activetab="true" startnode="632144263780000148" treenode="632580609502868877" appnode="632144263780000147" handlerfor="632144263780000146">
    <node type="Start" id="632144263780000148" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632144263780000155" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632144263780000155" name="Forward" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="222" y="179">
      <Properties final="true" type="appControl" log="On">
        <ap name="ToGuid" type="variable">toGuid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Forwarding from " + routingGuid + " to " + toGuid</log>
      </Properties>
    </node>
    <node type="Variable" id="632144263780000156" name="toGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="toGuid" refType="reference">toGuid</Properties>
    </node>
    <node type="Variable" id="632580609502868906" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>