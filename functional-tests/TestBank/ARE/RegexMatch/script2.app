<Application name="script2" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script2">
    <outline>
      <treenode type="evh" id="632145265340625308" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632145265340625189" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632145265340625188" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.RegexMatch.script2</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Two" id="632224814145469083" vid="632145265340625192">
        <Properties type="Metreos.Types.String" initWith="S_Two">S_Two</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" varsy="692" startnode="632145265340625190" treenode="632145265340625308" appnode="632145265340625189" handlerfor="632145265340625188">
    <node type="Start" id="632145265340625190" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632145265340625209" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632145265340625209" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="348" y="335">
      <linkto id="632224814145469088" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Two</ap>
      </Properties>
    </node>
    <node type="Action" id="632224814145469088" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="496" y="334">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
