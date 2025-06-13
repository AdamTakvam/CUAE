<Application name="script" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script">
    <outline>
      <treenode type="evh" id="632515097515522118" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632515097515522115" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632515097515522114" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632515097515522117" treenode="632515097515522118" appnode="632515097515522115" handlerfor="632515097515522114">
    <node type="Start" id="632515097515522117" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="205">
      <linkto id="632515097515522120" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632515097515522120" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="173" y="205">
      <linkto id="632515097515522121" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632515141907453961" type="Labeled" style="Bezier" ortho="true" label="timeout" />
      <Properties final="false" type="provider">
        <ap name="remoteHost" type="variable">remotehost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="timeout" type="literal">15000</ap>
        <log condition="success" on="true" level="Warning" type="literal">Success</log>
        <log condition="failure" on="true" level="Warning" type="literal">Failure</log>
        <log condition="timeout" on="true" level="Warning" type="literal">Timeout</log>
        <log condition="default" on="true" level="Warning" type="literal">Default</log>
      </Properties>
    </node>
    <node type="Action" id="632515097515522121" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="338" y="131">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632515141907453961" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="343" y="288">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Warning" type="literal">Timeout</log>
      </Properties>
    </node>
    <node type="Variable" id="632515097515522119" name="remotehost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remotehost</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>