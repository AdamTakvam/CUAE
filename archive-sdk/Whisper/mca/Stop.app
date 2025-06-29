<Application name="Stop" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="Stop">
    <outline>
      <treenode type="evh" id="632968429478799055" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632968429478799052" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632968429478799051" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Whisper/Stop</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_phoneUser" id="632968429478799126" vid="632968429478799061">
        <Properties type="String" initWith="PhoneUsername">g_phoneUser</Properties>
      </treenode>
      <treenode text="g_phonePass" id="632968429478799128" vid="632968429478799063">
        <Properties type="String" initWith="PhonePassword">g_phonePass</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632968429478799054" treenode="632968429478799055" appnode="632968429478799052" handlerfor="632968429478799051">
    <node type="Start" id="632968429478799054" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="88" y="288">
      <linkto id="632968429478799136" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632968429478799058" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="768" y="288">
      <linkto id="632968429478799059" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">RTPTx:Stop</ap>
        <rd field="ResultData">stopHome</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478799059" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="928" y="288">
      <linkto id="632968429478799065" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">stopHome.ToString()</ap>
        <ap name="URL" type="csharp">deviceResult.IP</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478799065" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="760" y="432">
      <linkto id="632968429478799067" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">RTPRx:Stop</ap>
        <rd field="ResultData">stopPageto</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478799067" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="928" y="432">
      <linkto id="632968429478799160" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">stopPageto.ToString()</ap>
        <ap name="URL" type="variable">remoteIP</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478799136" name="QueryByDevice" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="240" y="288">
      <linkto id="632968429478799137" type="Labeled" style="Vector" label="default" />
      <linkto id="632968429478799140" type="Labeled" style="Vector" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="DeviceName" type="csharp">query["homeDevice"]</ap>
        <rd field="ResultData">deviceResult</rd>
        <rd field="Count">resultCount</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478799137" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="240" y="408">
      <linkto id="632968429478799138" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">App Error</ap>
        <ap name="Text" type="literal">Unable to use real-time cache</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478799138" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="240" y="528">
      <linkto id="632968429478799139" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478799139" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="240" y="632">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632968429478799140" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="408" y="288">
      <linkto id="632968429478799141" type="Labeled" style="Vector" label="equal" />
      <linkto id="632968429478799158" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">resultCount</ap>
        <ap name="Value2" type="csharp">0</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478799141" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="408" y="416">
      <linkto id="632968429478799142" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Config Error</ap>
        <ap name="Text" type="literal">Could not find IP address of phone in real-time cache</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478799142" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="408" y="536">
      <linkto id="632968429478799143" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478799143" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="408" y="640">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632968429478799158" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="549" y="287">
      <linkto id="632968429478799159" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Stop</ap>
        <ap name="Text" type="literal">Stopping Page</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478799159" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="656" y="288">
      <linkto id="632968429478799058" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478799160" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1096" y="432">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632968429478799056" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632968429478799060" name="stopHome" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">stopHome</Properties>
    </node>
    <node type="Variable" id="632968429478799152" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632968429478799153" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632968429478799154" name="deviceResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoDeviceList.Device" refType="reference">deviceResult</Properties>
    </node>
    <node type="Variable" id="632968429478799155" name="resultCount" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">resultCount</Properties>
    </node>
    <node type="Variable" id="632968429478799156" name="stopPageto" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">stopPageto</Properties>
    </node>
    <node type="Variable" id="632968429478799157" name="remoteIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteIpAddress" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteIP</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>