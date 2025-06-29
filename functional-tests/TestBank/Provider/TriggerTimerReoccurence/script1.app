<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632150277987187828" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632150277987187826" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632150277987187825" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.TriggerTimerReoccurence.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632150277987187867" level="1" text="Metreos.Providers.FunctionalTest.Event: RemoveTimer">
        <node type="function" name="RemoveTimer" id="632150277987187865" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632150277987187864" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.TriggerTimerReoccurence.script1.E_RemoveTimer</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="timerId" id="632472773040883619" vid="632150277987187868">
        <Properties type="Metreos.Types.String">timerId</Properties>
      </treenode>
      <treenode text="S_Load" id="632472773040883621" vid="632150328113281376">
        <Properties type="Metreos.Types.String" initWith="S_Load">S_Load</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632150277987187827" treenode="632150277987187828" appnode="632150277987187826" handlerfor="632150277987187825">
    <node type="Start" id="632150277987187827" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="272">
      <linkto id="632150328113281375" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150328113281375" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="133" y="269">
      <linkto id="632472750912489769" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Load</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907728" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="475" y="260">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632472750912489769" name="AddTriggerTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="295" y="266">
      <linkto id="632224881663907728" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now</ap>
        <ap name="timerRecurrenceInterval" type="csharp">System.TimeSpan.FromMilliseconds(5000)</ap>
        <ap name="timerUserData" type="literal">onlyTimer</ap>
        <rd field="timerId">timerId</rd>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="RemoveTimer" startnode="632150277987187866" treenode="632150277987187867" appnode="632150277987187865" handlerfor="632150277987187864">
    <node type="Start" id="632150277987187866" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="157">
      <linkto id="632150277987187870" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150277987187870" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="187" y="383">
      <linkto id="632224881663907729" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="timerId" type="variable">timerId</ap>
        <log condition="entry" on="true" level="Info" type="literal">Entering</log>
      </Properties>
    </node>
    <node type="Action" id="632224881663907729" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="488" y="370">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
