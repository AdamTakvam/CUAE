<Application name="Start" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Start">
    <outline>
      <treenode type="evh" id="632362182347312675" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632362182347312672" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632362182347312671" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/threadpool-test</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632362182347312674" treenode="632362182347312675" appnode="632362182347312672" handlerfor="632362182347312671">
    <node type="Loop" id="632362185730437680" name="Loop" text="loop 20x" cx="238" cy="120" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="310" y="151" mx="429" my="211">
      <linkto id="632362185730437681" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632362182347312676" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="literal">20</Properties>
    </node>
    <node type="Start" id="632362182347312674" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="211">
      <linkto id="632362185730437678" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632362182347312676" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="670" y="211">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632362185730437678" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="169" y="211">
      <linkto id="632362185730437680" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
      </Properties>
    </node>
    <node type="Action" id="632362185730437681" name="SendEvent" container="632362185730437680" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="418" y="211">
      <linkto id="632362185730437680" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="url" type="literal">/threadpool-test-slave</ap>
        <ap name="EventName" type="literal">Metreos.Providers.Http.GotRequest</ap>
      </Properties>
    </node>
    <node type="Variable" id="632362185730437679" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>