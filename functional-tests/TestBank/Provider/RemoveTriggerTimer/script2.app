<Application name="script2" trigger="Metreos.Providers.TimerFacility.TimerFire" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script2">
    <outline>
      <treenode type="evh" id="632471912136003529" level="1" text="Metreos.Providers.TimerFacility.TimerFire (trigger): OnTimerFire">
        <node type="function" name="OnTimerFire" id="632150277987187689" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632150277987187688" path="Metreos.Providers.TimerFacility.TimerFire" trigger="true" />
        <Properties type="hybrid">
          <ep name="timerUserData" type="literal">onlyTimer</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632471912136003530" level="1" text="Metreos.Providers.FunctionalTest.Event: Shutdown">
        <node type="function" name="Shutdown" id="632150277987187699" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632150277987187698" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.RemoveTriggerTimer.script2.E_Shutdown</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Fired" id="632471912136003520" vid="632150277987187694">
        <Properties type="Metreos.Types.String" initWith="S_Fired">S_Fired</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTimerFire" startnode="632150277987187690" treenode="632150277987187691" appnode="632150277987187689" handlerfor="632150277987187688">
    <node type="Start" id="632150277987187690" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632150277987187696" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150277987187692" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="341" y="167">
      <linkto id="632224881663907520" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="timerId" type="variable">timerId</ap>
      </Properties>
    </node>
    <node type="Action" id="632150277987187696" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="168" y="166">
      <linkto id="632150277987187692" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Fired</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907520" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="524" y="169">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632150277987187693" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="timerId" refType="reference">timerId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Shutdown" startnode="632150277987187700" treenode="632150277987187701" appnode="632150277987187699" handlerfor="632150277987187698">
    <node type="Start" id="632150277987187700" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224881663907521" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632224881663907521" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="587" y="304">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
