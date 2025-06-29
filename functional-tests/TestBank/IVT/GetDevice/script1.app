<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632531378747997245" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632531378747997242" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632531378747997241" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">IVT.GetDevice.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_GetDeviceResponse" id="632531378747997595" vid="632531378747997303">
        <Properties type="String" initWith="S_GetDeviceResponse">S_GetDeviceResponse</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632531378747997244" treenode="632531378747997245" appnode="632531378747997242" handlerfor="632531378747997241">
    <node type="Start" id="632531378747997244" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="372">
      <linkto id="632531378747997306" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632531378747997306" name="GetPhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="264" y="372">
      <linkto id="632531378747997314" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632531378747997316" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="variable">devicename</ap>
        <ap name="CallManagerIP" type="variable">callmanagerIp</ap>
        <ap name="AdminUsername" type="variable">username</ap>
        <ap name="AdminPassword" type="variable">password</ap>
        <rd field="GetPhoneResponse">response</rd>
        <rd field="FaultMessage">fault</rd>
      </Properties>
    </node>
    <node type="Action" id="632531378747997314" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="441" y="373">
      <linkto id="632531378747997315" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.AxlSoap333.GetPhoneResponse response, ref string message)
{
	int numLines = response.Response.@return.device.lines == null ? 0 : 
			         response.Response.@return.device.lines.Items.Length;


	string description = response.Response.@return.device.description;

	string devicePool = response.Response.@return.device.Item3 == null ? "&lt;None&gt;" : response.Response.@return.device.Item3.ToString();

	string auth = response.Response.@return.device.authenticationURL;

	message = "Number of lines on phone: " + numLines + "\n" + "Description: " + description + "\n" + "Device Pool: " + devicePool + "\n" + "Authentication URL: " + auth;

	return "";
}
</Properties>
    </node>
    <node type="Action" id="632531378747997315" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="595" y="373">
      <linkto id="632531378747997318" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="message" type="variable">message</ap>
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_GetDeviceResponse</ap>
      </Properties>
    </node>
    <node type="Action" id="632531378747997316" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="441" y="504">
      <linkto id="632531378747997318" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="message" type="variable">fault</ap>
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_GetDeviceResponse</ap>
      </Properties>
    </node>
    <node type="Action" id="632531378747997318" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="591" y="504">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632531378747997305" name="devicename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="devicename" refType="reference">devicename</Properties>
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
      <Properties type="Metreos.Types.AxlSoap333.GetPhoneResponse" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632531378747997311" name="fault" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">fault</Properties>
    </node>
    <node type="Variable" id="632531378747997313" name="message" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">message</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
