<Application name="ForceRefresh" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="ForceRefresh">
    <outline>
      <treenode type="evh" id="632588598465696442" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632588598465696439" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632588598465696438" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/forcerefresh</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632588598465696441" treenode="632588598465696442" appnode="632588598465696439" handlerfor="632588598465696438">
    <node type="Start" id="632588598465696441" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="268">
      <linkto id="632588598465696443" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632588598465696443" name="Scrape" class="MaxActionNode" group="" path="Metreos.Providers.DatabaseScraper" x="276" y="270">
      <linkto id="632588598465696445" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
      </Properties>
    </node>
    <node type="Action" id="632588598465696444" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="532" y="270">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632588598465696445" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="413" y="271">
      <linkto id="632588598465696444" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="literal">A scrape has been requested</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Variable" id="632588598465696446" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>