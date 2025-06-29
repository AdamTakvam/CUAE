<Application name="script2" trigger="Metreos.Providers.TimerFacility.TimerFire" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script2">
    <outline>
      <treenode type="evh" id="632472750912489742" level="1" text="Metreos.Providers.TimerFacility.TimerFire (trigger): OnTimerFire">
        <node type="function" name="OnTimerFire" id="632150277987187832" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632150277987187831" path="Metreos.Providers.TimerFacility.TimerFire" trigger="true" />
        <Properties type="hybrid">
          <ep name="timerUserData" type="literal">onlyTimer</ep>
        </Properties>
      </treenode>
      <treenode type="fun" id="632472750912489743" level="1" text="assignTimerId">
        <node type="function" name="assignTimerId" id="632150277987187841" path="Metreos.StockTools" />
        <calls>
          <ref actid="632150277987187840" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Fired" id="632472750912489730" vid="632150277987187835">
        <Properties type="Metreos.Types.String" initWith="S_Fired">S_Fired</Properties>
      </treenode>
      <treenode text="g_timerId" id="632472750912489732" vid="632150277987187837">
        <Properties type="Metreos.Types.String">g_timerId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTimerFire" startnode="632150277987187833" treenode="632150277987187834" appnode="632150277987187832" handlerfor="632150277987187831">
    <node type="Start" id="632150277987187833" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="136">
      <linkto id="632150277987187840" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150277987187840" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="54.2480469" y="266" mx="96" my="282">
      <items count="1">
        <item text="assignTimerId" />
      </items>
      <linkto id="632150277987187846" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="timerId" type="variable">timerId</ap>
        <ap name="FunctionName" type="literal">assignTimerId</ap>
        <rd field="timerId">g_timerId</rd>
      </Properties>
    </node>
    <node type="Action" id="632150277987187846" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="291" y="282">
      <linkto id="632224881663907753" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Fired</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907753" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="582" y="288">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632150277987187839" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="timerId" refType="reference">timerId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="assignTimerId" startnode="632150277987187842" treenode="632150277987187843" appnode="632150277987187841" handlerfor="632150277987187831">
    <node type="Start" id="632150277987187842" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224881663907754" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632224881663907754" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="492" y="253">
      <Properties final="true" type="appControl">
        <ap name="timerId" type="variable">timerId</ap>
      </Properties>
    </node>
    <node type="Variable" id="632150277987187844" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="timerId" refType="reference">timerId</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
