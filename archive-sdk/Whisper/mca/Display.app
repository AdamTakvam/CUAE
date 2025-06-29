<Application name="Display" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="Display">
    <outline>
      <treenode type="evh" id="632968429478798714" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632968429478798711" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632968429478798710" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Whisper/Display</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632968429478798713" treenode="632968429478798714" appnode="632968429478798711" handlerfor="632968429478798710">
    <node type="Start" id="632968429478798713" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="96" y="280">
      <linkto id="632968429478798717" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632968429478798717" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="232" y="280">
      <linkto id="632968429478798719" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Incoming Whisper</ap>
        <ap name="Prompt" type="literal">Choose an Option</ap>
        <ap name="Text" type="literal">You are being whispered to!</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798719" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="368" y="280">
      <linkto id="632968429478798799" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Confirm</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Whisper/Confirm?homeDevice=" + query["homeDevice"] + "&amp;pageto=" + query["pageto"]</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Comment" id="632968429478798798" text="pageto&#xD;&#xA;homeDevice" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="238" y="94" />
    <node type="Action" id="632968429478798799" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="504" y="280">
      <linkto id="632968429478799048" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Stop</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Whisper/Stop?homeDevice=" + query["homeDevice"] + "&amp;pageto=" + query["pageto"]</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798803" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="720" y="280">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632968429478799048" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="616" y="280">
      <linkto id="632968429478798803" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Variable" id="632968429478798715" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632968429478798716" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632968429478798718" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632968429478798826" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>