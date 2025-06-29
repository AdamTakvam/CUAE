<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632143672065468898" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632143672065468896" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632143672065468895" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.CallFunction.script1</ep>
        </Properties>
      </treenode>
      <treenode type="fun" id="632143672065468904" level="1" text="RegularFunction">
        <node type="function" name="RegularFunction" id="632143672065468902" path="Metreos.StockTools" />
      </treenode>
    </outline>
    <variables>
      <treenode text="S_FromCallFunction" id="632224788543125164" vid="632143672065468899">
        <Properties type="Metreos.Types.String" initWith="S_FromCallFunction">S_FromCallFunction</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" varsy="692" startnode="632143672065468897" treenode="632143672065468898" appnode="632143672065468896" handlerfor="632143672065468895">
    <node type="Start" id="632143672065468897" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="131">
      <linkto id="632143672065468901" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632143672065468901" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="246.902344" y="133" mx="295" my="149">
      <items count="1">
        <item text="RegularFunction" />
      </items>
      <linkto id="632224788543125174" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="FunctionName" type="literal">RegularFunction</ap>
      </Properties>
    </node>
    <node type="Action" id="632224788543125174" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="487" y="150">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="RegularFunction" activetab="true" varsy="692" startnode="632143672065468903" treenode="632143672065468904" appnode="632143672065468902" handlerfor="632143672065468895">
    <node type="Start" id="632143672065468903" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632143672065468906" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632143672065468906" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="338" y="177">
      <linkto id="632224788543125175" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_FromCallFunction</ap>
      </Properties>
    </node>
    <node type="Action" id="632224788543125175" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="611" y="176">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
