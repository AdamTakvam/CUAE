<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632471135206409356" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632144433257031394" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632144433257031393" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.Switch.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632471135206409357" level="1" text="Metreos.Providers.FunctionalTest.Event: SwitchEvent">
        <node type="function" name="SwitchEvent" id="632144433257031408" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632144433257031407" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">ARE.Switch.script1.E_Switch</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Load" id="632471135206409334" vid="632144433257031397">
        <Properties type="Metreos.Types.String" initWith="S_Load">S_Load</Properties>
      </treenode>
      <treenode text="S_One" id="632471135206409336" vid="632144433257031401">
        <Properties type="Metreos.Types.String" initWith="S_One">S_One</Properties>
      </treenode>
      <treenode text="S_Two" id="632471135206409338" vid="632144433257031403">
        <Properties type="Metreos.Types.String" initWith="S_Two">S_Two</Properties>
      </treenode>
      <treenode text="S_Three" id="632471135206409340" vid="632144433257031405">
        <Properties type="Metreos.Types.String" initWith="S_Three">S_Three</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632144433257031395" treenode="632144433257031396" appnode="632144433257031394" handlerfor="632144433257031393">
    <node type="Start" id="632144433257031395" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="145">
      <linkto id="632144433257031399" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632144433257031399" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="415" y="142">
      <linkto id="632224814145469339" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Load</ap>
      </Properties>
    </node>
    <node type="Action" id="632224814145469339" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="644" y="141">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="SwitchEvent" startnode="632144433257031409" treenode="632144433257031410" appnode="632144433257031408" handlerfor="632144433257031407">
    <node type="Start" id="632144433257031409" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632144433257031411" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632144433257031411" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="315" y="208">
      <linkto id="632144433257031418" type="Labeled" style="Bezier" ortho="true" label="one" />
      <linkto id="632144433257031419" type="Labeled" style="Bezier" ortho="true" label="two" />
      <linkto id="632144433257031420" type="Labeled" style="Bezier" ortho="true" label="three" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">switchValue</ap>
      </Properties>
    </node>
    <node type="Action" id="632144433257031418" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="658" y="64">
      <linkto id="632224814145469338" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_One</ap>
      </Properties>
    </node>
    <node type="Action" id="632144433257031419" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="660" y="208">
      <linkto id="632224814145469338" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Two</ap>
      </Properties>
    </node>
    <node type="Action" id="632144433257031420" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="658" y="337">
      <linkto id="632224814145469337" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Three</ap>
      </Properties>
    </node>
    <node type="Action" id="632224814145469337" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="787" y="331">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632224814145469338" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="764" y="146">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632144433257031417" name="switchValue" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="switchValue" refType="reference">switchValue</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
