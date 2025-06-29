<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632472724054133736" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632150277987187704" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632150277987187703" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.ScriptTimerReoccurrence.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632472724054133737" level="2" text="Metreos.Providers.TimerFacility.TimerFire: OnTimerFire">
        <node type="function" name="OnTimerFire" id="632150277987187713" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632150277987187712" path="Metreos.Providers.TimerFacility.TimerFire" />
        <references>
          <ref id="632472740973430893" actid="632472740973430892" />
        </references>
        <Properties type="hybrid">
          <ep name="timerUserData" type="literal">onlyTimer</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632472724054133739" level="1" text="Metreos.Providers.FunctionalTest.Event: RemoveTimer">
        <node type="function" name="RemoveTimer" id="632150277987187728" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632150277987187727" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.ScriptTimerReoccurrence.script1.E_RemoveTimer</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632472724054133740" level="1" text="Metreos.Providers.FunctionalTest.Event: Shutdown">
        <node type="function" name="Shutdown" id="632150277987187721" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632150277987187720" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.ScriptTimerReoccurrence.script1.E_Shutdown</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Load" id="632472740973430871" vid="632150277987187707">
        <Properties type="Metreos.Types.String" initWith="S_Load">S_Load</Properties>
      </treenode>
      <treenode text="S_Fired" id="632472740973430873" vid="632150277987187709">
        <Properties type="Metreos.Types.String" initWith="S_Fired">S_Fired</Properties>
      </treenode>
      <treenode text="timerId" id="632472740973430875" vid="632150277987187732">
        <Properties type="Metreos.Types.String">timerId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632150277987187705" treenode="632472724054133736" appnode="632150277987187704" handlerfor="632150277987187703">
    <node type="Start" id="632150277987187705" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="279">
      <linkto id="632150277987187711" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150277987187711" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="304" y="284">
      <linkto id="632472740973430892" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Load</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907565" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="635" y="278">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632472740973430892" name="AddNonTriggerTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="409" y="261" mx="471" my="277">
      <items count="1">
        <item text="OnTimerFire" treenode="632472724054133737" />
      </items>
      <linkto id="632224881663907565" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now</ap>
        <ap name="timerRecurrenceInterval" type="csharp">System.TimeSpan.FromMilliseconds(5000)</ap>
        <ap name="timerUserData" type="literal">onlyTimer</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="timerId">timerId</rd>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnTimerFire" startnode="632150277987187714" treenode="632472724054133737" appnode="632150277987187713" handlerfor="632150277987187712">
    <node type="Start" id="632150277987187714" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632150277987187718" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150277987187718" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="258" y="165">
      <linkto id="632224881663907564" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Fired</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907564" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="499" y="234">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="RemoveTimer" startnode="632150277987187729" treenode="632472724054133739" appnode="632150277987187728" handlerfor="632150277987187727">
    <node type="Start" id="632150277987187729" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632150277987187731" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150277987187731" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="224" y="186">
      <linkto id="632224881663907562" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="timerId" type="variable">timerId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">" Timer Id  Value : " + timerId.ToString()</log>
      </Properties>
    </node>
    <node type="Action" id="632224881663907562" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="576" y="216">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Shutdown" activetab="true" startnode="632150277987187722" treenode="632472724054133740" appnode="632150277987187721" handlerfor="632150277987187720">
    <node type="Start" id="632150277987187722" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224881663907563" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632224881663907563" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="618" y="397">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
