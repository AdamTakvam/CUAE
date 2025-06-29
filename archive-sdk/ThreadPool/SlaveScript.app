<Application name="SlaveScript" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="SlaveScript">
    <outline>
      <treenode type="evh" id="632362185730437688" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632362185730437685" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632362185730437684" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/threadpool-test-slave</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632362185730437687" treenode="632362185730437688" appnode="632362185730437685" handlerfor="632362185730437684">
    <node type="Start" id="632362185730437687" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="248">
      <linkto id="632362185730437689" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632362185730437689" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="175" y="248">
      <linkto id="632362185730437690" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SleepTime" type="literal">20000</ap>
        <log condition="entry" on="true" level="Info" type="literal">Slave started</log>
        <log condition="exit" on="true" level="Info" type="literal">Slave ended</log>
      </Properties>
    </node>
    <node type="Action" id="632362185730437690" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="327" y="248">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>