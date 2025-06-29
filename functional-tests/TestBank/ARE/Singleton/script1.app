<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.7" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632284616052031425" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632284616052031422" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632284616052031421" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.Singleton.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Signal" id="632286319727985176" vid="632284616052031434">
        <Properties type="Metreos.Types.String" initWith="S_Signal">S_Signal</Properties>
      </treenode>
      <treenode text="variable3" id="632286319727985178" vid="632286139785328931">
        <Properties type="Metreos.Types.String">variable3</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632284616052031424" treenode="632284616052031425" appnode="632284616052031422" handlerfor="632284616052031421">
    <node type="Start" id="632284616052031424" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="332">
      <linkto id="632284616052031436" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632284616052031436" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="337" y="330">
      <linkto id="632284616052031438" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Signal</ap>
      </Properties>
    </node>
    <node type="Action" id="632284616052031438" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="553" y="337">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632286132343610180" name="variable2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">variable2</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="singleton" desc="">
  </Properties>
</Application>
