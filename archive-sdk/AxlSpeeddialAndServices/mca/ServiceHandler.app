<Application name="ServiceHandler" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="ServiceHandler">
    <outline>
      <treenode type="evh" id="632423474098293125" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632423474098293122" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632423474098293121" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/testscript</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632423474098293124" treenode="632423474098293125" appnode="632423474098293122" handlerfor="632423474098293121">
    <node type="Start" id="632423474098293124" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="368">
      <linkto id="632423474098293126" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632423474098293126" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="264" y="368">
      <linkto id="632423474098293128" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Url Worked!</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
      </Properties>
    </node>
    <node type="Action" id="632423474098293128" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="496" y="368">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632423474098293129" text="A script to handle the services we added to the device." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="261" y="201" />
    <node type="Variable" id="632423474098293127" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>