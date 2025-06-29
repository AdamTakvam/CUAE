<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.7" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632277666755469218" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632277666755469215" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632277666755469214" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.Connect.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Success" id="632277756578281495" vid="632277666755469246">
        <Properties type="Metreos.Types.String" initWith="S_Success">S_Success</Properties>
      </treenode>
      <treenode text="S_Failure" id="632277756578281497" vid="632277666755469259">
        <Properties type="Metreos.Types.String" initWith="S_Failure">S_Failure</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632277666755469217" treenode="632277666755469218" appnode="632277666755469215" handlerfor="632277666755469214">
    <node type="Start" id="632277666755469217" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="369">
      <linkto id="632277747735000185" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277666755469275" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="466" y="310">
      <linkto id="632277666755469277" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632277666755469276" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="467" y="499">
      <linkto id="632277666755469277" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Failure</ap>
      </Properties>
    </node>
    <node type="Action" id="632277666755469277" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="629" y="407">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632277747735000185" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="305" y="379">
      <linkto id="632277666755469275" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632277666755469276" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <Properties final="false" type="native">
        <ap name="DSN" type="literal">DSN=testframework;UID=root;PWD=</ap>
        <ap name="Name" type="literal">hello</ap>
        <ap name="Type" type="literal">odbc</ap>
      </Properties>
    </node>
    <node type="Variable" id="632277666755469274" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="dsn" refType="reference">dsn</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
