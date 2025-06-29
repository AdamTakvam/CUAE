<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632471110322940970" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632144263780000207" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632144263780000206" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.SendEvent.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632471110322940971" level="1" text="Metreos.Providers.FunctionalTest.Event: ReceiveEvent">
        <node type="function" name="ReceiveEvent" id="632144263780000219" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632144263780000218" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">unique</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632471110322940972" level="1" text="Metreos.Providers.FunctionalTest.Event: SendEvent">
        <node type="function" name="SendEvent" id="632144263780000215" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632144263780000214" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">ARE.SendEvent.script1.E_SendEvent</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Load" id="632471130054904154" vid="632144263780000210">
        <Properties type="Metreos.Types.String" initWith="S_Load">S_Load</Properties>
      </treenode>
      <treenode text="S_SendEvent" id="632471130054904156" vid="632144263780000212">
        <Properties type="Metreos.Types.String" initWith="S_SendEvent">S_SendEvent</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632144263780000208" treenode="632471110322940970" appnode="632144263780000207" handlerfor="632144263780000206">
    <node type="Start" id="632144263780000208" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="107">
      <linkto id="632144263780000222" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632144263780000222" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="351" y="198">
      <linkto id="632224814145469165" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Load</ap>
      </Properties>
    </node>
    <node type="Action" id="632224814145469165" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="491" y="196">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ReceiveEvent" startnode="632144263780000220" treenode="632471110322940971" appnode="632144263780000219" handlerfor="632144263780000218">
    <node type="Start" id="632144263780000220" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632144263780000228" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632144263780000228" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="371" y="218">
      <linkto id="632224814145469167" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_SendEvent</ap>
      </Properties>
    </node>
    <node type="Action" id="632224814145469167" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="484" y="196">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="SendEvent" activetab="true" startnode="632144263780000216" treenode="632471110322940972" appnode="632144263780000215" handlerfor="632144263780000214">
    <node type="Start" id="632144263780000216" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632144263780000224" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632224814145469168" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="506" y="192">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632144263780000224" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="294" y="193">
      <linkto id="632224814145469168" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="uniqueEventParam" type="literal">unique</ap>
        <ap name="EventName" type="literal">Metreos.Providers.FunctionalTest.Event</ap>
        <ap name="ToGuid" type="variable">toGuid</ap>
      </Properties>
    </node>
    <node type="Variable" id="632144263780000225" name="toGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="toGuid" refType="reference">toGuid</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
