<Application name="scriptHttpTest" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="scriptHttpTest">
    <outline>
      <treenode type="evh" id="632514197930701088" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632514197930701085" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632514197930701084" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/mytest</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632514197930701087" treenode="632514197930701088" appnode="632514197930701085" handlerfor="632514197930701084">
    <node type="Start" id="632514197930701087" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="94">
      <linkto id="632514197930701089" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632514197930701089" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="186" y="94">
      <linkto id="632772545148594109" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">reHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Hello from Max!</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <log condition="entry" on="true" level="Info" type="variable">reHost</log>
      </Properties>
    </node>
    <node type="Action" id="632514197930701231" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="442" y="94">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632772545148594109" name="SessionEnd" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="320" y="94">
      <linkto id="632514197930701231" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632514197930701734" name="reHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">reHost</Properties>
    </node>
  </canvas>
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>