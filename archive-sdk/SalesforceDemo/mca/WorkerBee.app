<Application name="WorkerBee" trigger="Metreos.Providers.SF.Worker" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="WorkerBee">
    <outline>
      <treenode type="evh" id="633082867945087972" level="1" text="Metreos.Providers.SF.Worker (trigger): OnWorker">
        <node type="function" name="OnWorker" id="633082867945087969" path="Metreos.StockTools" />
        <node type="event" name="Worker" id="633082867945087968" path="Metreos.Providers.SF.Worker" trigger="true" />
        <Properties type="hybrid">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_phonePassword" id="633083450026342318" vid="633082867945088343">
        <Properties type="String" initWith="PhonePassword">g_phonePassword</Properties>
      </treenode>
      <treenode text="g_phoneUsername" id="633083450026342320" vid="633082867945088345">
        <Properties type="String" initWith="PhoneUsername">g_phoneUsername</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnWorker" activetab="true" startnode="633082867945087971" treenode="633082867945087972" appnode="633082867945087969" handlerfor="633082867945087968">
    <node type="Start" id="633082867945087971" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633082867945088339" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633082867945088339" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="101.869141" y="61.25">
      <linkto id="633082867945088340" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="variable">url</ap>
        <rd field="ResultData">execXML</rd>
      </Properties>
    </node>
    <node type="Action" id="633082867945088340" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="293.869141" y="62">
      <linkto id="633082867945088348" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execXML.ToString()</ap>
        <ap name="URL" type="variable">remoteIPAddress</ap>
        <ap name="Username" type="variable">g_phoneUsername</ap>
        <ap name="Password" type="variable">g_phonePassword</ap>
      </Properties>
    </node>
    <node type="Action" id="633082867945088348" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="447" y="64">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633082867945087973" name="remoteIPAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteIpAddress" refType="reference" name="Metreos.Providers.SF.Worker">remoteIPAddress</Properties>
    </node>
    <node type="Variable" id="633082867945087974" name="url" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="url" refType="reference" name="Metreos.Providers.SF.Worker">url</Properties>
    </node>
    <node type="Variable" id="633082867945088347" name="execXML" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execXML</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>