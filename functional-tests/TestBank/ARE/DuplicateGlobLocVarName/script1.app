<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632241798842656415" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632241798842656412" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632241798842656411" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.DuplicateGlobLocVarName.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="dupName" id="632241798842656425" vid="632241798842656424">
        <Properties type="Metreos.Types.Int" defaultInitWith="1">dupName</Properties>
      </treenode>
      <treenode text="S_Signal" id="632241798842656428" vid="632241798842656427">
        <Properties type="Metreos.Types.String" initWith="S_Signal">S_Signal</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" varsy="667" startnode="632241798842656414" treenode="632241798842656415" appnode="632241798842656412" handlerfor="632241798842656411">
    <node type="Start" id="632241798842656414" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="332">
      <linkto id="632241798842656429" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Variable" id="632241798842656426" name="dupName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="685">
      <Properties type="Metreos.Types.Int" defaultInitWith="2" refType="reference">dupName</Properties>
    </node>
    <node type="Action" id="632241798842656429" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="308" y="336">
      <linkto id="632241798842656430" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="duplicate" type="variable">dupName</ap>
        <ap name="signalName" type="variable">S_Signal</ap>
      </Properties>
    </node>
    <node type="Action" id="632241798842656430" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="563" y="340">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
