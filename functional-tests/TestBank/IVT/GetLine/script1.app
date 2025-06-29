<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632531378747997245" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632531378747997242" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632531378747997241" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">IVT.GetLine.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_GetLineResponse" id="632531378747997738" vid="632531378747997303">
        <Properties type="String" initWith="S_GetLineResponse">S_GetLineResponse</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632531378747997244" treenode="632531378747997245" appnode="632531378747997242" handlerfor="632531378747997241">
    <node type="Start" id="632531378747997244" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="372">
      <linkto id="632531378747997651" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632531378747997314" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="441" y="373">
      <linkto id="632531378747997315" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.AxlSoap333.GetLineResponse response, ref string message)
{
	string networkHoldMOHAudioSourceId = response.Response.@return.directoryNumber.networkHoldMOHAudioSourceId;

	string userHoldMOHAudioSourceId = response.Response.@return.directoryNumber.userHoldMOHAudioSourceId;

	string vmProfile = response.Response.@return.directoryNumber.Item3 != null ? response.Response.@return.directoryNumber.Item3.ToString() : "&lt;None&gt;";

	string css = response.Response.@return.directoryNumber.Item2 != null ? (response.Response.@return.directoryNumber.Item2 as Metreos.AxlSoap333.XNPDirectoryNumberShareLineAppearanceCSS).uuid : "&lt;None&gt;";

	
	message = "NetworkHoldMOHAudioSourceId : " + networkHoldMOHAudioSourceId  + "\n" + "UserHoldMOHAudioSourceId : " + userHoldMOHAudioSourceId  + "\n" + "Voice Mail Profile: " + vmProfile  + "\n" + "CSS: " + css;

	return "";
}
</Properties>
    </node>
    <node type="Action" id="632531378747997315" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="595" y="373">
      <linkto id="632531378747997318" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="message" type="variable">message</ap>
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_GetLineResponse</ap>
      </Properties>
    </node>
    <node type="Action" id="632531378747997316" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="441" y="504">
      <linkto id="632531378747997318" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="message" type="variable">fault</ap>
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_GetLineResponse</ap>
      </Properties>
    </node>
    <node type="Action" id="632531378747997318" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="591" y="504">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632531378747997651" name="GetLine" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="244" y="375">
      <linkto id="632531378747997314" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632531378747997316" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RoutePartitionName" type="variable">routepartition</ap>
        <ap name="Pattern" type="variable">pattern</ap>
        <ap name="CallManagerIP" type="variable">callmanagerIp</ap>
        <ap name="AdminUsername" type="variable">username</ap>
        <ap name="AdminPassword" type="variable">password</ap>
        <rd field="GetLineResponse">response</rd>
        <rd field="FaultMessage">fault</rd>
      </Properties>
    </node>
    <node type="Variable" id="632531378747997305" name="routepartition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="routepartition" refType="reference">routepartition</Properties>
    </node>
    <node type="Variable" id="632531378747997307" name="callmanagerIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callmanagerIp" refType="reference">callmanagerIp</Properties>
    </node>
    <node type="Variable" id="632531378747997308" name="username" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="username" refType="reference">username</Properties>
    </node>
    <node type="Variable" id="632531378747997309" name="password" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="password" refType="reference">password</Properties>
    </node>
    <node type="Variable" id="632531378747997310" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap333.GetLineResponse" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632531378747997311" name="fault" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">fault</Properties>
    </node>
    <node type="Variable" id="632531378747997313" name="message" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">message</Properties>
    </node>
    <node type="Variable" id="632531378747997652" name="pattern" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="pattern" refType="reference">pattern</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
