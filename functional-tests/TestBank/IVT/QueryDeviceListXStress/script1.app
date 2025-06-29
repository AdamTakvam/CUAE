<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632526221257774462" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632526221257774459" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632526221257774458" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">IVT.QueryDeviceListXStress.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632526221257774467" level="2" text="Metreos.Providers.FunctionalTest.Event: OnEvent">
        <node type="function" name="OnEvent" id="632526221257774464" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632526221257774463" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.QueryDeviceListXStress.script1.E_Poll</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632526221257774546" level="2" text="Metreos.Providers.FunctionalTest.Event: OnEvent1">
        <node type="function" name="OnEvent1" id="632526221257774543" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632526221257774542" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.QueryDeviceListXStress.script1.E_Shutdown</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632526323733971482" level="2" text="Metreos.Providers.FunctionalTest.Event: OnEvent2">
        <node type="function" name="OnEvent2" id="632526323733971479" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632526323733971478" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.QueryDeviceListXStress.script1.E_Refresh</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632526323733971533" level="2" text="Metreos.Providers.CiscoDeviceListX.Refresh_Complete: OnRefresh_Complete">
        <node type="function" name="OnRefresh_Complete" id="632526323733971530" path="Metreos.StockTools" />
        <node type="event" name="Refresh_Complete" id="632526323733971529" path="Metreos.Providers.CiscoDeviceListX.Refresh_Complete" />
        <references>
          <ref id="632527174907346650" actid="632526323733971539" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632526323733971538" level="2" text="Metreos.Providers.CiscoDeviceListX.Refresh_Failed: OnRefresh_Failed">
        <node type="function" name="OnRefresh_Failed" id="632526323733971535" path="Metreos.StockTools" />
        <node type="event" name="Refresh_Failed" id="632526323733971534" path="Metreos.Providers.CiscoDeviceListX.Refresh_Failed" />
        <references>
          <ref id="632527174907346651" actid="632526323733971539" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_QueryResults" id="632527174907346622" vid="632526221257774538">
        <Properties type="String" initWith="S_QueryResult">S_QueryResults</Properties>
      </treenode>
      <treenode text="S_Trigger" id="632527174907346624" vid="632526221257774616">
        <Properties type="String" initWith="S_Trigger">S_Trigger</Properties>
      </treenode>
      <treenode text="g_count" id="632527174907346626" vid="632526221257774655">
        <Properties type="Int">g_count</Properties>
      </treenode>
      <treenode text="S_Refresh" id="632527174907346628" vid="632526323733971602">
        <Properties type="String" initWith="S_Refresh">S_Refresh</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632526221257774461" treenode="632526221257774462" appnode="632526221257774459" handlerfor="632526221257774458">
    <node type="Start" id="632526221257774461" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="358">
      <linkto id="632526221257774657" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632526221257774540" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="391" y="357">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632526221257774615" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="243" y="357">
      <linkto id="632526221257774540" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="count" type="variable">count</ap>
        <ap name="signalName" type="variable">S_Trigger</ap>
      </Properties>
    </node>
    <node type="Action" id="632526221257774657" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="98" y="358">
      <linkto id="632526221257774615" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">count</ap>
        <rd field="ResultData">g_count</rd>
      </Properties>
    </node>
    <node type="Variable" id="632526221257774658" name="count" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="count" refType="reference">count</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent" activetab="true" startnode="632526221257774466" treenode="632526221257774467" appnode="632526221257774464" handlerfor="632526221257774463">
    <node type="Start" id="632526221257774466" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632526221257774468" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632526221257774468" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="51.6744843" y="224">
      <linkto id="632526221257774469" type="Labeled" style="Bezier" ortho="true" label="IP" />
      <linkto id="632526221257774470" type="Labeled" style="Bezier" ortho="true" label="DN" />
      <linkto id="632526221257774471" type="Labeled" style="Bezier" ortho="true" label="STATUS" />
      <linkto id="632526221257774472" type="Labeled" style="Bezier" ortho="true" label="TYPE" />
      <linkto id="632526221257774473" type="Labeled" style="Bezier" ortho="true" label="POOL" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">queryType</ap>
      </Properties>
    </node>
    <node type="Action" id="632526221257774469" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="363.6745" y="48">
      <linkto id="632526221257774474" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="IP" type="variable">queryValue</ap>
        <rd field="ResultData">results</rd>
      </Properties>
    </node>
    <node type="Action" id="632526221257774470" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="363.6745" y="152">
      <linkto id="632526221257774474" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">queryValue</ap>
        <rd field="ResultData">results</rd>
      </Properties>
    </node>
    <node type="Action" id="632526221257774471" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="363.6745" y="264">
      <linkto id="632526221257774474" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Status" type="variable">queryValue</ap>
        <rd field="ResultData">results</rd>
      </Properties>
    </node>
    <node type="Action" id="632526221257774472" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="363.6745" y="383">
      <linkto id="632526221257774474" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="variable">queryValue</ap>
        <rd field="ResultData">results</rd>
      </Properties>
    </node>
    <node type="Action" id="632526221257774473" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="363.6745" y="480">
      <linkto id="632526221257774474" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Pool" type="variable">queryValue</ap>
        <rd field="ResultData">results</rd>
      </Properties>
    </node>
    <node type="Action" id="632526221257774474" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="562.6745" y="267">
      <linkto id="632526221257774541" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="results" type="variable">results</ap>
        <ap name="count" type="variable">g_count</ap>
        <ap name="signalName" type="variable">S_QueryResults</ap>
      </Properties>
    </node>
    <node type="Action" id="632526221257774541" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="693" y="269">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632526221257774485" name="queryType" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="queryType" refType="reference">queryType</Properties>
    </node>
    <node type="Variable" id="632526221257774486" name="queryValue" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="queryValue" refType="reference">queryValue</Properties>
    </node>
    <node type="Variable" id="632526221257774487" name="results" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">results</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent1" startnode="632526221257774545" treenode="632526221257774546" appnode="632526221257774543" handlerfor="632526221257774542">
    <node type="Start" id="632526221257774545" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="49" y="296">
      <linkto id="632526221257774580" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632526221257774580" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="484" y="303">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent2" startnode="632526323733971481" treenode="632526323733971482" appnode="632526323733971479" handlerfor="632526323733971478">
    <node type="Start" id="632526323733971481" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="365">
      <linkto id="632526323733971539" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632526323733971539" name="Refresh" class="MaxAsyncActionNode" group="" path="Metreos.Providers.CiscoDeviceListX" x="191" y="345" mx="253" my="361">
      <items count="2">
        <item text="OnRefresh_Complete" treenode="632526323733971533" />
        <item text="OnRefresh_Failed" treenode="632526323733971538" />
      </items>
      <linkto id="632526323733971600" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632526323733971600" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="431" y="357">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRefresh_Complete" startnode="632526323733971532" treenode="632526323733971533" appnode="632526323733971530" handlerfor="632526323733971529">
    <node type="Start" id="632526323733971532" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="319">
      <linkto id="632526323733971601" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632526323733971601" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="339" y="317">
      <linkto id="632526323733971604" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_Refresh</ap>
      </Properties>
    </node>
    <node type="Action" id="632526323733971604" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="534" y="320">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRefresh_Failed" startnode="632526323733971537" treenode="632526323733971538" appnode="632526323733971535" handlerfor="632526323733971534">
    <node type="Start" id="632526323733971537" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="39" y="243">
      <linkto id="632526323733971605" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632526323733971605" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="248.61" y="238">
      <linkto id="632526323733971607" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_Refresh</ap>
      </Properties>
    </node>
    <node type="Action" id="632526323733971607" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="405" y="238">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
