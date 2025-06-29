<Application name="master1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="master1">
    <outline>
      <treenode type="evh" id="632230011505207584" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632230011505207581" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632230011505207580" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.MasterSlaveControlNegative.master1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632230011505207603" level="1" text="Event: OnEvent">
        <node type="function" name="OnEvent" id="632230011505207600" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632230011505207599" path="Metreos.Providers.FunctionalTest.Event" />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">ARE.MasterSlaveControl.E_Event1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Enabled" id="632230102930988839" vid="632230011505207595">
        <Properties type="Metreos.Types.String" initWith="S_Enabled">S_Enabled</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" varsy="692" startnode="632230011505207583" treenode="632230011505207584" appnode="632230011505207581" handlerfor="632230011505207580">
    <node type="Start" id="632230011505207583" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="363">
      <linkto id="632230011505207594" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632230011505207594" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="425" y="365">
      <linkto id="632230011505207604" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Enabled</ap>
        <log condition="exit" on="true" level="Info" type="literal">Master script triggered.</log>
      </Properties>
    </node>
    <node type="Action" id="632230011505207604" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="679" y="369">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent" varsy="692" startnode="632230011505207602" treenode="632230011505207603" appnode="632230011505207600" handlerfor="632230011505207599">
    <node type="Start" id="632230011505207602" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632230011505207605" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632230011505207605" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="617" y="258">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
