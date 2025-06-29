<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632145384312031418" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632145370382656373" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632145370382656372" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Max.LevelOneChildLoop2.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_LevelOne" id="632224906580625239" vid="632145370382656376">
        <Properties type="Metreos.Types.String" initWith="S_LevelOne">S_LevelOne</Properties>
      </treenode>
      <treenode text="S_LevelOneB" id="632224906580625241" vid="632145384312031398">
        <Properties type="Metreos.Types.String" initWith="S_LevelOneB">S_LevelOneB</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" varsy="667" startnode="632145370382656374" treenode="632145384312031418" appnode="632145370382656373" handlerfor="632145370382656372">
    <node type="Loop" id="632145370382656378" name="Loop" text="loop (var)" cx="442" cy="296" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="217" y="118" mx="438" my="266">
      <linkto id="632145370382656379" port="1" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632224906580625249" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">levelZeroCount</Properties>
    </node>
    <node type="Loop" id="632145370382656379" name="Loop" text="loop (var)" cx="167" cy="153" entry="1" container="632145370382656378" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="239" y="187" mx="322" my="264">
      <linkto id="632145370382656380" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632145384312031396" port="1" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">levelOneCount</Properties>
    </node>
    <node type="Loop" id="632145384312031396" name="Loop" text="loop (var)" cx="174" cy="156" entry="1" container="632145370382656378" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="439" y="188" mx="526" my="266">
      <linkto id="632145384312031397" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632145370382656378" port="3" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">levelOneCount</Properties>
    </node>
    <node type="Start" id="632145370382656374" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="47" y="259">
      <linkto id="632145370382656378" port="1" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632145370382656380" name="Signal" container="632145370382656379" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="325" y="255">
      <linkto id="632145370382656379" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_LevelOne</ap>
      </Properties>
    </node>
    <node type="Variable" id="632145370382656382" name="levelZeroCount" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="685">
      <Properties type="Metreos.Types.String" initWith="levelZeroCount" refType="reference">levelZeroCount</Properties>
    </node>
    <node type="Variable" id="632145370382656383" name="levelOneCount" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="141.5729" y="685">
      <Properties type="Metreos.Types.String" initWith="levelOneCount" refType="reference">levelOneCount</Properties>
    </node>
    <node type="Action" id="632145384312031397" name="Signal" container="632145384312031396" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="527" y="264">
      <linkto id="632145384312031396" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_LevelOneB</ap>
      </Properties>
    </node>
    <node type="Action" id="632224906580625249" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="749" y="267">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
