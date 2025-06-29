<Application name="slave1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.7" single="false">
  <!--//// app tree ////-->
  <global name="slave1">
    <outline>
      <treenode type="evh" id="632230011505207612" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632230011505207609" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632230011505207608" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.MasterSlaveControl.slave1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632279390627343945" vid="632230011505207613">
        <Properties type="Metreos.Types.String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632230011505207611" treenode="632230011505207612" appnode="632230011505207609" handlerfor="632230011505207608">
    <node type="Start" id="632230011505207611" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632230011505207615" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632230011505207615" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="504" y="237">
      <linkto id="632230011505207616" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple</ap>
        <log condition="exit" on="true" level="Info" type="literal">Exiting slave script</log>
      </Properties>
    </node>
    <node type="Action" id="632230011505207616" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="664" y="234">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="slave" instanceType="multiInstance" desc="">
  </Properties>
</Application>
