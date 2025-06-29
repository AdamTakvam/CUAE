<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632145978783437765" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632145370382656373" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632145370382656372" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Max.LevelOneChildLoop3.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_LevelOne" id="632224906580625266" vid="632145370382656376">
        <Properties type="Metreos.Types.String" initWith="S_LevelOne">S_LevelOne</Properties>
      </treenode>
      <treenode text="S_LevelOneB" id="632224906580625268" vid="632145384312031398">
        <Properties type="Metreos.Types.String" initWith="S_LevelOneB">S_LevelOneB</Properties>
      </treenode>
      <treenode text="S_LevelOneC" id="632224906580625270" vid="632145384312031448">
        <Properties type="Metreos.Types.String" initWith="S_LevelOneC">S_LevelOneC</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" varsy="667" startnode="632145370382656374" treenode="632145978783437765" appnode="632145370382656373" handlerfor="632145370382656372">
    <node type="Loop" id="632145370382656378" name="Loop" text="loop (var)" cx="636" cy="296" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="124" y="111" mx="442" my="259">
      <linkto id="632145370382656379" port="1" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632224906580625279" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">levelZeroCount</Properties>
    </node>
    <node type="Loop" id="632145370382656379" name="Loop" text="loop (var)" cx="167" cy="153" entry="1" container="632145370382656378" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="150" y="177" mx="234" my="254">
      <linkto id="632145370382656380" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632145384312031396" port="1" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">levelOneCount</Properties>
    </node>
    <node type="Loop" id="632145384312031396" name="Loop" text="loop (var)" cx="174" cy="156" entry="1" container="632145370382656378" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="344" y="179" mx="431" my="257">
      <linkto id="632145384312031397" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632145384312031450" port="1" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">levelOneCount</Properties>
    </node>
    <node type="Loop" id="632145384312031450" name="Loop" text="loop (var)" cx="156" cy="156" entry="1" container="632145370382656378" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="554" y="181" mx="632" my="259">
      <linkto id="632145384312031451" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632145370382656378" port="3" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">levelOneCount</Properties>
    </node>
    <node type="Start" id="632145370382656374" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="39" y="249">
      <linkto id="632145370382656378" port="1" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632145370382656380" name="Signal" container="632145370382656379" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="241" y="243">
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
    <node type="Action" id="632145384312031397" name="Signal" container="632145384312031396" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="413" y="245">
      <linkto id="632145384312031396" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_LevelOneB</ap>
      </Properties>
    </node>
    <node type="Action" id="632145384312031451" name="Signal" container="632145384312031450" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="641" y="244">
      <linkto id="632145384312031450" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_LevelOneC</ap>
      </Properties>
    </node>
    <node type="Action" id="632224906580625279" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="824" y="243">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
