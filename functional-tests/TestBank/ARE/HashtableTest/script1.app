<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632143649841875169" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632143649841875137" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632143649841875136" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.HashtableTest.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632143649841875172" level="1" text="Event: OnEvent">
        <node type="function" name="OnEvent" id="632143649841875141" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632143649841875140" path="Metreos.Providers.FunctionalTest.Event" />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">ARE.HashtableTest.script1.E_Add</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632224814145468935" vid="632143649841875153">
        <Properties type="Metreos.Types.String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
      <treenode text="S_Add" id="632224814145468939" vid="632143649841875161">
        <Properties type="Metreos.Types.String" initWith="S_Add">S_Add</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" varsy="692" startnode="632143649841875138" treenode="632143649841875169" appnode="632143649841875137" handlerfor="632143649841875136">
    <node type="Start" id="632143649841875138" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632143649841875152" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632143649841875152" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="130" y="211">
      <linkto id="632224796097969033" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632224796097969033" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="402" y="206">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent" activetab="true" varsy="667" startnode="632143649841875142" treenode="632143649841875172" appnode="632143649841875141" handlerfor="632143649841875140">
    <node type="Start" id="632143649841875142" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224814145468946" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632143649841875160" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="509" y="222">
      <linkto id="632224796097969032" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="addedValue" type="csharp">hashtable["value1"] as string</ap>
        <ap name="signalName" type="variable">S_Add</ap>
      </Properties>
    </node>
    <node type="Action" id="632224796097969032" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="666" y="220">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632224814145468946" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="248" y="216">
      <linkto id="632143649841875160" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(Hashtable hashtable)
	{
        hashtable.Add("value1", "someValue");

return String.Empty;

	}
</Properties>
    </node>
    <node type="Variable" id="632224814145468948" name="hashtable" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="685">
      <Properties type="Metreos.Types.Hashtable" refType="reference">hashtable</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
