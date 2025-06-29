<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632471279791850505" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632471279791850502" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632471279791850501" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.Timer.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632471279791850518" level="2" text="Metreos.Providers.TimerFacility.TimerFire: OnTimerFire">
        <node type="function" name="OnTimerFire" id="632471279791850515" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632471279791850514" path="Metreos.Providers.TimerFacility.TimerFire" />
        <references>
          <ref id="632471279791850520" actid="632471279791850519" />
        </references>
        <Properties type="hybrid">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Fired" id="632471279791850524" vid="632471279791850523">
        <Properties type="String" initWith="S_Fired">S_Fired</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632471279791850504" treenode="632471279791850505" appnode="632471279791850502" handlerfor="632471279791850501">
    <node type="Start" id="632471279791850504" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="376">
      <linkto id="632471279791850519" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632471279791850519" name="AddNonTriggerTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="248" y="360" mx="310" my="376">
      <items count="1">
        <item text="OnTimerFire" treenode="632471279791850518" />
      </items>
      <linkto id="632471279791850521" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now.ToString()</ap>
        <ap name="timerUserData" type="literal">onlyTimer</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632471279791850521" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="544" y="376">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnTimerFire" activetab="true" startnode="632471279791850517" treenode="632471279791850518" appnode="632471279791850515" handlerfor="632471279791850514">
    <node type="Start" id="632471279791850517" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="352">
      <linkto id="632471279791850522" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632471279791850522" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="336" y="352">
      <linkto id="632471279791850525" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Fired</ap>
      </Properties>
    </node>
    <node type="Action" id="632471279791850525" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="536" y="352">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
