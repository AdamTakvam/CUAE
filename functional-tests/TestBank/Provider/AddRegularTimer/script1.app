<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632471279791850563" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632284635299843988" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632284635299843987" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.AddRegularTimer.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632471279791850564" level="1" text="Metreos.Providers.TimerFacility.TimerFire: OnTimerFire">
        <node type="function" name="OnTimerFire" id="632284635299844009" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632284635299844008" path="Metreos.Providers.TimerFacility.TimerFire" />
        <references>
          <ref id="632471279791850603" actid="632471279791850602" />
        </references>
        <Properties type="hybrid">
          <ep name="timerUserData" type="literal">gee</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Trigger" id="632471279791850589" vid="632284635299844000">
        <Properties type="Metreos.Types.String" initWith="S_Trigger">S_Trigger</Properties>
      </treenode>
      <treenode text="S_TimerFire" id="632471279791850591" vid="632284635299844002">
        <Properties type="Metreos.Types.String" initWith="S_TimerFire">S_TimerFire</Properties>
      </treenode>
      <treenode text="timerId" id="632471279791850593" vid="632284635299844005">
        <Properties type="Metreos.Types.String">timerId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632284635299843990" treenode="632471279791850563" appnode="632284635299843988" handlerfor="632284635299843987">
    <node type="Start" id="632284635299843990" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="362">
      <linkto id="632471279791850602" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632284635299844007" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="548" y="364">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632471279791850602" name="AddNonTriggerTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="278" y="347" mx="340" my="363">
      <items count="1">
        <item text="OnTimerFire" treenode="632471279791850564" />
      </items>
      <linkto id="632284635299844007" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now</ap>
        <ap name="timerUserData" type="literal">gee</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnTimerFire" startnode="632284635299844011" treenode="632471279791850564" appnode="632284635299844009" handlerfor="632284635299844008">
    <node type="Start" id="632284635299844011" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="364">
      <linkto id="632284635299844013" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632284635299844013" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="284" y="364">
      <linkto id="632284635299844014" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_TimerFire</ap>
      </Properties>
    </node>
    <node type="Action" id="632284635299844014" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="498" y="363">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
