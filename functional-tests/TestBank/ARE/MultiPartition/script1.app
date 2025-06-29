<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632510813665820060" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632510813665820057" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632510813665820056" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.MultiPartitionTest.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Signal" id="632510813665820095" vid="632510813665820069">
        <Properties type="String" initWith="S_Signal">S_Signal</Properties>
      </treenode>
      <treenode text="g_one" id="632510813665820097" vid="632510813665820081">
        <Properties type="String" initWith="one">g_one</Properties>
      </treenode>
      <treenode text="g_two" id="632510813665820099" vid="632510813665820083">
        <Properties type="String" initWith="two">g_two</Properties>
      </treenode>
      <treenode text="g_three" id="632510813665820101" vid="632510813665820085">
        <Properties type="String" initWith="three">g_three</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632510813665820059" treenode="632510813665820060" appnode="632510813665820057" handlerfor="632510813665820056">
    <node type="Start" id="632510813665820059" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="344">
      <linkto id="632510813665820103" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632510813665820103" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="248" y="344">
      <linkto id="632510813665820104" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="one" type="variable">g_one</ap>
        <ap name="three" type="variable">g_three</ap>
        <ap name="two" type="variable">g_two</ap>
        <ap name="signalName" type="variable">S_Signal</ap>
      </Properties>
    </node>
    <node type="Action" id="632510813665820104" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="401" y="346">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
