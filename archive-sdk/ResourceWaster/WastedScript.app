<Application name="WastedScript" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="WastedScript">
    <outline>
      <treenode type="evh" id="633151770643355490" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="633151770643355487" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633151770643355486" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/WastedScript</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="633151770643355489" treenode="633151770643355490" appnode="633151770643355487" handlerfor="633151770643355486">
    <node type="Start" id="633151770643355489" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633151770643355492" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633151770643355492" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="144" y="41">
      <linkto id="633151770643355493" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="literal">30000</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">WastedScript has been triggered and is now sleeping for 30 sec.</log>
      </Properties>
    </node>
    <node type="Action" id="633151770643355493" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="241" y="46">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>