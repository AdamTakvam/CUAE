<Application name="Confirm" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="Confirm">
    <outline>
      <treenode type="evh" id="632968429478798810" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632968429478798807" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632968429478798806" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Whisper/Confirm</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_phoneUser" id="632968429478799005" vid="632968429478798972">
        <Properties type="String" initWith="PhoneUsername">g_phoneUser</Properties>
      </treenode>
      <treenode text="g_phonePass" id="632968429478799007" vid="632968429478798974">
        <Properties type="String" initWith="PhonePassword">g_phonePass</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632968429478798809" treenode="632968429478798810" appnode="632968429478798807" handlerfor="632968429478798806">
    <node type="Start" id="632968429478798809" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="96" y="264">
      <linkto id="632968429478798951" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632968429478798863" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="536" y="264">
      <linkto id="632968429478798969" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Confirmation</ap>
        <ap name="Prompt" type="literal">Choose an Option</ap>
        <ap name="Text" type="literal">Whisper Confirmed</ap>
        <rd field="ResultData">confirm</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798866" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1192" y="264">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632968429478798951" name="QueryByDevice" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="208" y="264">
      <linkto id="632968429478798952" type="Labeled" style="Vector" label="default" />
      <linkto id="632968429478798955" type="Labeled" style="Vector" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="DeviceName" type="csharp">query["homeDevice"]</ap>
        <rd field="ResultData">deviceResult</rd>
        <rd field="Count">resultCount</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798952" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="208" y="384">
      <linkto id="632968429478798953" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">App Error</ap>
        <ap name="Text" type="literal">Unable to use real-time cache</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798953" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="208" y="504">
      <linkto id="632968429478798954" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798954" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="208" y="608">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632968429478798955" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="376" y="264">
      <linkto id="632968429478798956" type="Labeled" style="Vector" label="equal" />
      <linkto id="632968429478798863" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">resultCount</ap>
        <ap name="Value2" type="csharp">0</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798956" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="376" y="392">
      <linkto id="632968429478798957" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Config Error</ap>
        <ap name="Text" type="literal">Could not find IP address of phone in real-time cache</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798957" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="376" y="512">
      <linkto id="632968429478798958" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798958" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="376" y="616">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632968429478798969" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="688" y="264">
      <linkto id="632968429478798977" type="Labeled" style="Vector" label="Success" />
      <linkto id="632968429478798979" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">confirm.ToString()</ap>
        <ap name="URL" type="csharp">deviceResult.IP</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798976" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1040" y="264">
      <linkto id="632968429478798866" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798977" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="816" y="264">
      <linkto id="632968429478799030" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Confirmed</ap>
        <ap name="Text" type="literal">Sent confirmation</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798979" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="816" y="408">
      <linkto id="632968429478798976" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">App Error</ap>
        <ap name="Text" type="literal">Unable to send confirmation</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478799030" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="936" y="264">
      <linkto id="632968429478798976" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Stop</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Whisper/Stop?homeDevice=" + query["homeDevice"] + "&amp;pageto=" + query["pageto"]</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Variable" id="632968429478798834" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632968429478798835" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632968429478798836" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632968429478798967" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632968429478798968" name="confirm" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">confirm</Properties>
    </node>
    <node type="Variable" id="632968429478798970" name="deviceResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoDeviceList.Device" refType="reference">deviceResult</Properties>
    </node>
    <node type="Variable" id="632968429478798971" name="resultCount" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">resultCount</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>