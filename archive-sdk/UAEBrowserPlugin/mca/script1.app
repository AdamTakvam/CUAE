<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632923860333299752" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632923860333299749" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632923860333299748" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/CiscoDirectoryPlugin/DirectDial</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632923860333299751" treenode="632923860333299752" appnode="632923860333299749" handlerfor="632923860333299748">
    <node type="Start" id="632923860333299751" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="237">
      <linkto id="632923860333299757" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632923860333299756" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="331" y="238">
      <linkto id="632923860333299760" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"OK"</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632923860333299757" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="114" y="236">
      <linkto id="632923860333299759" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"Dial:" + query["number"]</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632923860333299759" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="224" y="237">
      <linkto id="632923860333299756" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="csharp">query["phoneIP"]</ap>
        <ap name="Username" type="csharp">query["username"]</ap>
        <ap name="Password" type="csharp">query["password"]</ap>
      </Properties>
    </node>
    <node type="Action" id="632923860333299760" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="447.884277" y="239">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632923860333299753" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632923860333299755" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632923860333299758" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>