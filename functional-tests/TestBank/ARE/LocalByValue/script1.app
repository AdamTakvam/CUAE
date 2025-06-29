<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632229256274375151" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632229256274375148" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632229256274375147" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.LocalByValue.script1</ep>
        </Properties>
      </treenode>
      <treenode type="fun" id="632229256274375173" level="1" text="ByValueFunction">
        <node type="function" name="ByValueFunction" id="632229256274375170" path="Metreos.StockTools" />
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632229294682656405" vid="632229256274375179">
        <Properties type="Metreos.Types.String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" varsy="570" startnode="632229256274375150" treenode="632229256274375151" appnode="632229256274375148" handlerfor="632229256274375147">
    <node type="Start" id="632229256274375150" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="319">
      <linkto id="632229256274375169" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Variable" id="632229256274375168" name="testVar" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="588">
      <Properties type="Metreos.Types.String" defaultInitWith="specificValue" refType="reference">testVar</Properties>
    </node>
    <node type="Action" id="632229256274375169" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="311.5664" y="314" mx="361" my="330">
      <items count="1">
        <item text="ByValueFunction" treenode="632229256274375151" />
      </items>
      <linkto id="632229256274375181" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="testVarValue" type="variable">testVar</ap>
        <ap name="FunctionName" type="literal">ByValueFunction</ap>
      </Properties>
    </node>
    <node type="Action" id="632229256274375174" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="613" y="331">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632229256274375181" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="493" y="330">
      <linkto id="632229256274375174" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="testVarValue" type="variable">testVar</ap>
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ByValueFunction" activetab="true" varsy="570" startnode="632229256274375172" treenode="632229256274375173" appnode="632229256274375170" handlerfor="632229256274375147">
    <node type="Start" id="632229256274375172" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="417">
      <linkto id="632229256274375176" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Variable" id="632229256274375175" name="testVar" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="588">
      <Properties type="Metreos.Types.String" initWith="testVarValue" refType="value">testVar</Properties>
    </node>
    <node type="Action" id="632229256274375176" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="330" y="423">
      <linkto id="632229256274375177" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref string testVar)
	{
	 testVar = String.Empty;
return String.Empty;

	}
</Properties>
    </node>
    <node type="Action" id="632229256274375177" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="482" y="425">
      <linkto id="632229256274375178" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="testVarValue" type="variable">testVar</ap>
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632229256274375178" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="589" y="426">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
