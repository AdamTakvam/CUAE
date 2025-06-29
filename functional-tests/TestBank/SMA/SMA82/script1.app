<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632146103040000179" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632146103040000177" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632146103040000176" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">SMA.SMA82.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632224881663907813" vid="632146103040000180">
        <Properties type="Metreos.Types.String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" varsy="667" startnode="632146103040000178" treenode="632146103040000179" appnode="632146103040000177" handlerfor="632146103040000176">
    <node type="Loop" id="632146103040000183" name="Loop" text="loop (var)" cx="148" cy="112" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="292" y="304" mx="366" my="360">
      <linkto id="632146103040000184" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632224881663907819" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">loopCount</Properties>
    </node>
    <node type="Start" id="632146103040000178" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="52" y="184">
      <linkto id="632146103040000183" port="1" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Variable" id="632146103040000182" name="loopCount" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="685">
      <Properties type="Metreos.Types.String" initWith="loopCount" refType="reference">loopCount</Properties>
    </node>
    <node type="Action" id="632146103040000184" name="Signal" container="632146103040000183" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="368" y="356">
      <linkto id="632146103040000183" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907819" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="550" y="306">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
