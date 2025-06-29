<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632145978783437815" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632145370382656373" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632145370382656372" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Max.LevelOneChildLoop6.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_LevelOne" id="632471259619968995" vid="632145370382656376">
        <Properties type="Metreos.Types.String" initWith="S_LevelOne">S_LevelOne</Properties>
      </treenode>
      <treenode text="S_LevelOneB" id="632471259619968997" vid="632145384312031398">
        <Properties type="Metreos.Types.String" initWith="S_LevelOneB">S_LevelOneB</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632145370382656374" treenode="632145978783437815" appnode="632145370382656373" handlerfor="632145370382656372">
    <node type="Loop" id="632145370382656378" name="Loop" text="loop (var)" cx="244" cy="239" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="96" y="155" mx="218" my="274">
      <linkto id="632145370382656379" port="1" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632224906580625360" port="1" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">levelZeroCount</Properties>
    </node>
    <node type="Loop" id="632145370382656379" name="Loop" text="loop (var)" cx="167" cy="153" entry="1" container="632145370382656378" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="136" y="197" mx="220" my="274">
      <linkto id="632145370382656380" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632145370382656378" port="3" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">levelOneCount</Properties>
    </node>
    <node type="Loop" id="632224906580625360" name="Loop" text="loop (var)" cx="306" cy="336" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="368" y="109" mx="521" my="277">
      <linkto id="632224906580625362" port="1" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632224906580625363" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">levelZeroCount</Properties>
    </node>
    <node type="Loop" id="632224906580625362" name="Loop" text="loop (var)" cx="120" cy="120" entry="1" container="632224906580625360" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="440" y="203" mx="500" my="263">
      <linkto id="632224906580625361" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632224906580625360" port="3" fromport="4" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">levelOneCount</Properties>
    </node>
    <node type="Start" id="632145370382656374" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="47" y="259">
      <linkto id="632145370382656378" port="1" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632145370382656380" name="Signal" container="632145370382656379" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="225" y="254">
      <linkto id="632145370382656379" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_LevelOne</ap>
      </Properties>
    </node>
    <node type="Action" id="632224906580625361" name="Signal" container="632224906580625362" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="509" y="257">
      <linkto id="632224906580625362" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_LevelOneB</ap>
      </Properties>
    </node>
    <node type="Action" id="632224906580625363" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="736" y="278">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632145370382656382" name="levelZeroCount" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="levelZeroCount" refType="reference">levelZeroCount</Properties>
    </node>
    <node type="Variable" id="632145370382656383" name="levelOneCount" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="levelOneCount" refType="reference">levelOneCount</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
