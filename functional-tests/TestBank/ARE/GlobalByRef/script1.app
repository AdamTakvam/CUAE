<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632471147747431351" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632229256274375148" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632229256274375147" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.GlobalByRef.script1</ep>
        </Properties>
      </treenode>
      <treenode type="fun" id="632471147747431352" level="1" text="ByRefFunction">
        <node type="function" name="ByRefFunction" id="632229256274375170" path="Metreos.StockTools" />
        <calls>
          <ref actid="632229256274375169" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632471147747431337" vid="632229256274375179">
        <Properties type="Metreos.Types.String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
      <treenode text="testVarGlobal" id="632471147747431339" vid="632229806375000188">
        <Properties type="Metreos.Types.String" defaultInitWith="specificValue">testVarGlobal</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632229256274375150" treenode="632229256274375151" appnode="632229256274375148" handlerfor="632229256274375147">
    <node type="Start" id="632229256274375150" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="319">
      <linkto id="632229256274375169" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632229256274375169" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="317.610352" y="314" mx="361" my="330">
      <items count="1">
        <item text="ByRefFunction" />
      </items>
      <linkto id="632229256274375181" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="testVarValue" type="variable">testVarGlobal</ap>
        <ap name="FunctionName" type="literal">ByRefFunction</ap>
      </Properties>
    </node>
    <node type="Action" id="632229256274375174" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="613" y="331">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632229256274375181" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="493" y="330">
      <linkto id="632229256274375174" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="testVarValue" type="variable">testVarGlobal</ap>
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ByRefFunction" startnode="632229256274375172" treenode="632229256274375173" appnode="632229256274375170" handlerfor="632229256274375147">
    <node type="Start" id="632229256274375172" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="417">
      <linkto id="632229256274375176" type="Basic" style="Bezier" ortho="true" />
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
    <node type="Variable" id="632229256274375175" name="testVar" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="testVarValue" refType="reference">testVar</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
