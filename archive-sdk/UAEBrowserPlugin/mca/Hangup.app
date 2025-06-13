<Application name="Hangup" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Hangup">
    <outline>
      <treenode type="evh" id="632924593738246358" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632924593738246355" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632924593738246354" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/CiscoDirectoryPlugin/Hangup</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632924593738246357" treenode="632924593738246358" appnode="632924593738246355" handlerfor="632924593738246354">
    <node type="Start" id="632924593738246357" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="457">
      <linkto id="632924593738246383" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632924593738246382" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="711.869141" y="458">
      <linkto id="632924593738246390" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"OK"</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632924593738246383" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="196.869141" y="461">
      <linkto id="632924593738246384" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">Key:Soft2</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632924593738246384" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="468.869141" y="461">
      <linkto id="632924593738246382" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="csharp">query["phoneIP"]</ap>
        <ap name="Username" type="csharp">query["username"]</ap>
        <ap name="Password" type="csharp">query["password"]</ap>
      </Properties>
    </node>
    <node type="Action" id="632924593738246390" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="864" y="459">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632924593738246359" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632924593738246388" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632924593738246389" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>