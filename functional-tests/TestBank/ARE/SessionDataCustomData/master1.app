<Application name="master1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="master1">
    <outline>
      <treenode type="evh" id="632229926290937681" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632229926290937678" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632229926290937677" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.SessionDataCustomData.master1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632229965754895071" vid="632229926290937691">
        <Properties type="Metreos.Types.String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" varsy="667" startnode="632229926290937680" treenode="632229926290937681" appnode="632229926290937678" handlerfor="632229926290937677">
    <node type="Start" id="632229926290937680" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="359">
      <linkto id="632229926290937696" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632229926290937690" name="SetSessionData" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="572" y="357">
      <linkto id="632229935407082578" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="stringData" type="literal">literalStringValue</ap>
        <ap name="intData" type="variable">intData</ap>
        <ap name="metreosStringData" type="variable">stringData</ap>
        <ap name="hashtableData" type="variable">hashtableData</ap>
        <log condition="exit" on="true" level="Info" type="literal">Set Session Data completed.</log>
      </Properties>
    </node>
    <node type="Variable" id="632229926290937693" name="intData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="685">
      <Properties type="Metreos.Types.Int" defaultInitWith="5" refType="reference">intData</Properties>
    </node>
    <node type="Variable" id="632229926290937694" name="stringData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="93.32422" y="685">
      <Properties type="Metreos.Types.String" defaultInitWith="specificValue" refType="reference">stringData</Properties>
    </node>
    <node type="Variable" id="632229926290937695" name="hashtableData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="188.170563" y="685">
      <Properties type="Metreos.Types.Hashtable" refType="reference">hashtableData</Properties>
    </node>
    <node type="Action" id="632229926290937696" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="303" y="360">
      <linkto id="632229926290937690" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
       public static string Execute(Hashtable hashtableData)
	{
        hashtableData["testKey"] = "testValue";
return String.Empty;
	}
</Properties>
    </node>
    <node type="Action" id="632229935407082578" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="728" y="358">
      <linkto id="632229935407082579" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="intValue" type="csharp">sessionData.CustomData["intData"]</ap>
        <ap name="signalName" type="variable">S_Simple</ap>
        <log condition="exit" on="true" level="Info" type="literal">Sent int value signal</log>
      </Properties>
    </node>
    <node type="Action" id="632229935407082579" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="890.000061" y="360">
      <linkto id="632229935407082581" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="stringValue" type="csharp">sessionData.CustomData["stringData"]</ap>
        <ap name="signalName" type="variable">S_Simple</ap>
        <log condition="exit" on="true" level="Info" type="literal">Sent literal string value signal.</log>
      </Properties>
    </node>
    <node type="Action" id="632229935407082580" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="1196" y="360">
      <linkto id="632229935407082582" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="hashtableValue" type="csharp">sessionData.CustomData["hashtableData"]</ap>
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632229935407082581" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="1030" y="359">
      <linkto id="632229935407082580" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="metreosStringValue" type="csharp">sessionData.CustomData["metreosStringData"]</ap>
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632229935407082582" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1323" y="359">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
