<Application name="ReportInput" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="ReportInput">
    <outline>
      <treenode type="evh" id="632520406017277146" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632520406017277143" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632520406017277142" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ReportInput</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632520406017277145" treenode="632520406017277146" appnode="632520406017277143" handlerfor="632520406017277142">
    <node type="Start" id="632520406017277145" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="424">
      <linkto id="632520406017277150" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632520406017277150" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="257" y="424">
      <linkto id="632520406017277179" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">IVT Input</ap>
        <ap name="Prompt" type="literal">Options</ap>
        <ap name="Text" type="csharp">"Received Request--Values entered:\n\nName:" + queryParams["name"] + "\nPassword:" + queryParams["pass"]</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632520406017277179" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="392" y="424">
      <linkto id="632520406017277180" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Text</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/CreateText"</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632520406017277180" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="512" y="424">
      <linkto id="632520413815259527" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632520406017277183" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="715" y="423">
      <linkto id="632520413815259529" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">Ok</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632520413815259527" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="608" y="423">
      <linkto id="632520406017277183" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="name" type="csharp">queryParams["name"]</ap>
        <ap name="pass" type="csharp">queryParams["pass"]</ap>
        <ap name="signalName" type="literal">IVT.SendExecute.script1.S_Signal</ap>
      </Properties>
    </node>
    <node type="Action" id="632520413815259529" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="814.589233" y="428">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632520406017277147" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">queryParams</Properties>
    </node>
    <node type="Variable" id="632520406017277148" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632520406017277149" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632520413815259530" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
