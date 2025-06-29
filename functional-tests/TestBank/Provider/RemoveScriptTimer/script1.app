<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632471824868440416" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632150277987187612" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632150277987187611" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.RemoveScriptTimer.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632471824868440417" level="2" text="Metreos.Providers.TimerFacility.TimerFire: OnTimerFire">
        <node type="function" name="OnTimerFire" id="632150277987187616" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632150277987187615" path="Metreos.Providers.TimerFacility.TimerFire" />
        <references>
          <ref id="632471902227108392" actid="632471902227108391" />
        </references>
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="632471824868440419" level="1" text="Metreos.Providers.FunctionalTest.Event: Shutdown">
        <node type="function" name="Shutdown" id="632150277987187626" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632150277987187625" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.RemoveScriptTimer.script1.E_Shutdown</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Fired" id="632471902227108373" vid="632150277987187631">
        <Properties type="Metreos.Types.String" initWith="S_Fired">S_Fired</Properties>
      </treenode>
      <treenode text="S_Load" id="632471902227108375" vid="632150277987187654">
        <Properties type="Metreos.Types.String" initWith="S_Load">S_Load</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632150277987187613" treenode="632471824868440416" appnode="632150277987187612" handlerfor="632150277987187611">
    <node type="Start" id="632150277987187613" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="151">
      <linkto id="632150277987187656" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150277987187656" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="129" y="216">
      <linkto id="632471902227108391" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Load</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907455" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="521" y="220">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632471902227108391" name="AddNonTriggerTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="245" y="199" mx="307" my="215">
      <items count="1">
        <item text="OnTimerFire" treenode="632471824868440417" />
      </items>
      <linkto id="632224881663907455" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now</ap>
        <ap name="timerRecurrenceInterval" type="csharp">System.TimeSpan.FromMilliseconds(5000)</ap>
        <ap name="timerUserData" type="literal">onlyTimer</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnTimerFire" activetab="true" startnode="632150277987187617" treenode="632471824868440417" appnode="632150277987187616" handlerfor="632150277987187615">
    <node type="Start" id="632150277987187617" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="180">
      <linkto id="632150277987187630" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150277987187621" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="291" y="314">
      <linkto id="632224881663907456" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="timerId" type="variable">timerId</ap>
      </Properties>
    </node>
    <node type="Action" id="632150277987187630" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="147" y="313">
      <linkto id="632150277987187621" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Fired</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907456" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="546" y="313">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632150277987187620" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="timerId" refType="reference">timerId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Shutdown" startnode="632150277987187627" treenode="632471824868440419" appnode="632150277987187626" handlerfor="632150277987187625">
    <node type="Start" id="632150277987187627" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224881663907457" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632224881663907457" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="467" y="277">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
