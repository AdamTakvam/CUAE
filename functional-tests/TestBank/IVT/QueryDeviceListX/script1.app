<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632514021171073855" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632514021171073852" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632514021171073851" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">IVT.QueryDeviceListX.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_QueryResults" id="632515169199598985" vid="632514021171073864">
        <Properties type="String" initWith="S_QueryResult">S_QueryResults</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632514021171073854" treenode="632514021171073855" appnode="632514021171073852" handlerfor="632514021171073851">
    <node type="Start" id="632514021171073854" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="392">
      <linkto id="632514021171073868" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632514021171073868" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="256" y="392">
      <linkto id="632514021171073869" type="Labeled" style="Bezier" ortho="true" label="IP" />
      <linkto id="632514021171073870" type="Labeled" style="Bezier" ortho="true" label="DN" />
      <linkto id="632514021171073871" type="Labeled" style="Bezier" ortho="true" label="STATUS" />
      <linkto id="632514021171073872" type="Labeled" style="Bezier" ortho="true" label="TYPE" />
      <linkto id="632514021171073874" type="Labeled" style="Bezier" ortho="true" label="POOL" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">queryType</ap>
      </Properties>
    </node>
    <node type="Action" id="632514021171073869" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="568" y="216">
      <linkto id="632514021171073876" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="IP" type="variable">queryValue</ap>
        <rd field="ResultData">results</rd>
      </Properties>
    </node>
    <node type="Action" id="632514021171073870" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="568" y="320">
      <linkto id="632514021171073876" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="variable">queryValue</ap>
        <rd field="ResultData">results</rd>
      </Properties>
    </node>
    <node type="Action" id="632514021171073871" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="568" y="432">
      <linkto id="632514021171073876" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Status" type="variable">queryValue</ap>
        <rd field="ResultData">results</rd>
      </Properties>
    </node>
    <node type="Action" id="632514021171073872" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="568" y="552">
      <linkto id="632514021171073876" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Type" type="variable">queryValue</ap>
        <rd field="ResultData">results</rd>
      </Properties>
    </node>
    <node type="Action" id="632514021171073874" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="568" y="648">
      <linkto id="632514021171073876" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Pool" type="variable">queryValue</ap>
        <rd field="ResultData">results</rd>
      </Properties>
    </node>
    <node type="Action" id="632514021171073876" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="746" y="423">
      <linkto id="632514021171073877" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="results" type="variable">results</ap>
        <ap name="signalName" type="variable">S_QueryResults</ap>
      </Properties>
    </node>
    <node type="Action" id="632514021171073877" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="912" y="424">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632514021171073866" name="queryType" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="queryType" refType="reference">queryType</Properties>
    </node>
    <node type="Variable" id="632514021171073867" name="queryValue" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="queryValue" refType="reference">queryValue</Properties>
    </node>
    <node type="Variable" id="632514021171073875" name="results" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">results</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
