<Application name="scriptHttpPost" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="scriptHttpPost">
    <outline>
      <treenode type="evh" id="632514932224651721" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632514932224651718" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632514932224651717" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/mypost</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632514932224651720" treenode="632514932224651721" appnode="632514932224651718" handlerfor="632514932224651717">
    <node type="Start" id="632514932224651720" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="32">
      <linkto id="632514932224651723" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632514932224651722" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="418" y="358">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632514932224651723" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="176" y="263">
      <linkto id="632514932224651722" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remoteHost" type="variable">rHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"Query String is " + rQuery + "Body is " + rBody</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Variable" id="632514932224651724" name="rQuery" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">rQuery</Properties>
    </node>
    <node type="Variable" id="632514932224651725" name="rBody" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="body" refType="reference" name="Metreos.Providers.Http.GotRequest">rBody</Properties>
    </node>
    <node type="Variable" id="632514932224651726" name="rHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">rHost</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>