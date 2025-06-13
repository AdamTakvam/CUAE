<Application name="script1" trigger="Metreos.Providers.JTapi.JTapiIncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632935022220433298" level="1" text="Metreos.Providers.JTapi.JTapiIncomingCall (trigger): OnJTapiIncomingCall">
        <node type="function" name="OnJTapiIncomingCall" id="632935022220433295" path="Metreos.StockTools" />
        <node type="event" name="JTapiIncomingCall" id="632935022220433294" path="Metreos.Providers.JTapi.JTapiIncomingCall" trigger="true" />
        <Properties type="triggering">
          <ep name="To" type="literal">SEP000011112222</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_phoneUser" id="632935068412376044" vid="632935022220433346">
        <Properties type="String" initWith="phoneUser">g_phoneUser</Properties>
      </treenode>
      <treenode text="g_phonePass" id="632935068412376046" vid="632935022220433348">
        <Properties type="String" initWith="phonePass">g_phonePass</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnJTapiIncomingCall" activetab="true" startnode="632935022220433297" treenode="632935022220433298" appnode="632935022220433295" handlerfor="632935022220433294">
    <node type="Start" id="632935022220433297" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="76" y="458">
      <linkto id="632935022220433323" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632935022220433323" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="504" y="464">
      <linkto id="632935022220433342" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Customer Call</ap>
        <ap name="Prompt" type="literal">Record This!</ap>
        <ap name="Text" type="csharp">"This is a sensitive customer call!\nFrom: " + from + "\nDistance: " + distance</ap>
        <rd field="ResultData">textXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632935022220433324" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="984" y="464">
      <linkto id="632935022220433357" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">textXml.ToString()</ap>
        <ap name="URL" type="csharp">deviceCacheSearchResults.Rows[0]["IP"] as string</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="632935022220433342" name="QueryByDevice" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="672" y="464">
      <linkto id="632935022220433344" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DeviceName" type="variable">devicename</ap>
        <rd field="ResultData">deviceCacheSearchResults</rd>
        <rd field="Count">resultCount</rd>
      </Properties>
    </node>
    <node type="Action" id="632935022220433344" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="800" y="464">
      <linkto id="632935022220433345" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632935022220433324" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">resultCount != 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632935022220433345" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="800" y="672">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="csharp">"Unable to find devicename: " + devicename + " in DeviceListX/SNMP cache!  Invoke the DeviceListX Provider in mceadmin"</log>
      </Properties>
    </node>
    <node type="Comment" id="632935022220433355" text="As a convenience, clear the screen after some amount of time.&#xD;&#xA;&#xD;&#xA;Not a requirement--just highlighting what can be done." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1216" y="360" />
    <node type="Action" id="632935022220433357" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1296" y="464">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632935022220433320" name="distance" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Double" refType="reference">distance</Properties>
    </node>
    <node type="Variable" id="632935022220433321" name="devicename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.JTapi.JTapiIncomingCall">devicename</Properties>
    </node>
    <node type="Variable" id="632935022220433322" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.Providers.JTapi.JTapiIncomingCall">from</Properties>
    </node>
    <node type="Variable" id="632935022220433325" name="textXml" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">textXml</Properties>
    </node>
    <node type="Variable" id="632935022220433343" name="resultCount" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">resultCount</Properties>
    </node>
    <node type="Variable" id="632935022220433350" name="deviceCacheSearchResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">deviceCacheSearchResults</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>