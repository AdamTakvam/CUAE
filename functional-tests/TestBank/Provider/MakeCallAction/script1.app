<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632367314251718912" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632367314251718909" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632367314251718908" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.MakeCallAction.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632367353426875210" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632367353426875207" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632367353426875206" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632397636994375300" actid="632367353426875216" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632367353426875215" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632367353426875212" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632367353426875211" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632397636994375301" actid="632367353426875216" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632397636994375295" vid="632367314251718921">
        <Properties type="String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
      <treenode text="S_Failed" id="632397636994375297" vid="632367314251718923">
        <Properties type="String" initWith="S_Failed">S_Failed</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632367314251718911" treenode="632367314251718912" appnode="632367314251718909" handlerfor="632367314251718908">
    <node type="Start" id="632367314251718911" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="194">
      <linkto id="632367353426875216" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632367353426875216" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="69" y="179" mx="135" my="195">
      <items count="2">
        <item text="OnMakeCall_Complete" treenode="632367353426875210" />
        <item text="OnMakeCall_Failed" treenode="632367353426875215" />
      </items>
      <linkto id="632367353426875221" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632367353426875222" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="To" type="literal">1002</ap>
        <ap name="From" type="literal">1000</ap>
        <ap name="MmsId" type="literal">1</ap>
        <ap name="userData" type="literal">none</ap>
        <rd field="CallId">callId</rd>
      </Properties>
    </node>
    <node type="Action" id="632367353426875221" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="295" y="194">
      <linkto id="632367353426875227" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632367353426875226" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <Properties final="false" type="native">
        <ap name="Value1" type="literal">2000</ap>
        <ap name="Value2" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632367353426875222" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="134" y="369">
      <linkto id="632367353426875223" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Failed</ap>
      </Properties>
    </node>
    <node type="Action" id="632367353426875223" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="135" y="492">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632367353426875226" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="419.420227" y="194">
      <linkto id="632397636994375319" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632367353426875227" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="298.420166" y="329">
      <linkto id="632367503131875230" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Failed</ap>
      </Properties>
    </node>
    <node type="Action" id="632367503131875230" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="298.130249" y="457">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632397636994375319" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="553" y="195">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632367353426875219" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" activetab="true" startnode="632367353426875209" treenode="632367353426875210" appnode="632367353426875207" handlerfor="632367353426875206">
    <node type="Start" id="632367353426875209" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="349">
      <linkto id="632367503131875227" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632367503131875227" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="137" y="348">
      <linkto id="632391504173281523" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632367503131875228" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="551" y="347">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632391504173281523" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="291" y="348">
      <linkto id="632391504173281524" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SleepTime" type="literal">5000</ap>
      </Properties>
    </node>
    <node type="Action" id="632391504173281524" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="425" y="347">
      <linkto id="632367503131875228" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Variable" id="632391504173281525" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632367353426875214" treenode="632367353426875215" appnode="632367353426875212" handlerfor="632367353426875211">
    <node type="Start" id="632367353426875214" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632367503131875232" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632367503131875231" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="509" y="369">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632367503131875232" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="369" y="313">
      <linkto id="632367503131875231" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Failed</ap>
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
