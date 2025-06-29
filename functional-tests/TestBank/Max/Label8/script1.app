<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632146032163593914" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632146012209687651" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632146012209687650" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Max.Label8.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632224906580625181" vid="632146012209687659">
        <Properties type="Metreos.Types.String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" varsy="667" startnode="632146012209687652" treenode="632146032163593914" appnode="632146012209687651" handlerfor="632146012209687650">
    <node type="Start" id="632146012209687652" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="56" y="192">
      <linkto id="632146012209687697" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632146012209687657" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="293" y="191">
      <linkto id="632162605878281439" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632146012209687658" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="616" y="189">
      <linkto id="632224906580625197" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Label" id="632146012209687697" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="239" y="192" />
    <node type="Variable" id="632146012209687698" name="switchOn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="685">
      <Properties type="Metreos.Types.Int" initWith="switchOn" refType="reference">switchOn</Properties>
    </node>
    <node type="Label" id="632162605878281439" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="380" y="190" />
    <node type="Label" id="632162605878281440" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="442" y="191">
      <linkto id="632162605878281467" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632162605878281467" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="511" y="189">
      <linkto id="632146012209687658" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632224906580625196" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="switchOn" type="csharp">onceThrough.ToString().ToLower()</ap>
      </Properties>
    </node>
    <node type="Label" id="632162605878281468" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="508" y="382" />
    <node type="Variable" id="632162605878281469" name="onceThrough" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="105.908852" y="685">
      <Properties type="Metreos.Types.Bool" refType="reference">onceThrough</Properties>
    </node>
    <node type="Label" id="632162605878281505" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="250" y="254">
      <linkto id="632224906580625196" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632224906580625196" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="507" y="285">
      <linkto id="632162605878281468" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(bool onceThrough)
	{
		onceThrough = true;

return String.Empty;
	}
</Properties>
    </node>
    <node type="Action" id="632224906580625197" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="687" y="188">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
