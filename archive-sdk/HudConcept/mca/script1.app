<Application name="script1" trigger="Metreos.Providers.JTapi.JTapiIncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632516905419448557" level="1" text="Metreos.Providers.JTapi.JTapiIncomingCall (trigger): OnJTapiIncomingCall">
        <node type="function" name="OnJTapiIncomingCall" id="632516905419448554" path="Metreos.StockTools" />
        <node type="event" name="JTapiIncomingCall" id="632516905419448553" path="Metreos.Providers.JTapi.JTapiIncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632516967055630440" level="1" text="Metreos.Providers.JTapi.JTapiCallActive: OnJTapiCallActive">
        <node type="function" name="OnJTapiCallActive" id="632516967055630437" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallActive" id="632516967055630436" path="Metreos.Providers.JTapi.JTapiCallActive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632516967055630445" level="1" text="Metreos.Providers.JTapi.JTapiHangup: OnJTapiHangup">
        <node type="function" name="OnJTapiHangup" id="632516967055630442" path="Metreos.StockTools" />
        <node type="event" name="JTapiHangup" id="632516967055630441" path="Metreos.Providers.JTapi.JTapiHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_appServerIp" id="632516987579207181" vid="632516905419448582">
        <Properties type="String" initWith="appServerIp">g_appServerIp</Properties>
      </treenode>
      <treenode text="g_phoneIp" id="632516987579207183" vid="632516967055630464">
        <Properties type="String">g_phoneIp</Properties>
      </treenode>
      <treenode text="g_deviceName" id="632516987579207185" vid="632516983129010849">
        <Properties type="String">g_deviceName</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnJTapiIncomingCall" startnode="632516905419448556" treenode="632516905419448557" appnode="632516905419448554" handlerfor="632516905419448553">
    <node type="Start" id="632516905419448556" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="368">
      <linkto id="632516983129010851" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632516967055630467" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="461" y="369">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632516983129010851" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="248" y="369">
      <linkto id="632516967055630467" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties language="csharp">
public static string Execute(ref string g_deviceName, string deviceName)
{
	g_deviceName = deviceName;
	return "success";

}
</Properties>
    </node>
    <node type="Variable" id="632516905419448568" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" initWith="RoutingGuid" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="632516905419448569" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference">deviceName</Properties>
    </node>
    <node type="Variable" id="632516905419448585" name="queryResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">queryResults</Properties>
    </node>
    <node type="Variable" id="632516905419448589" name="phoneIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">phoneIp</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallActive" activetab="true" startnode="632516967055630439" treenode="632516967055630440" appnode="632516967055630437" handlerfor="632516967055630436">
    <node type="Start" id="632516967055630439" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="395">
      <linkto id="632516967055630448" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632516967055630446" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="691.942" y="397">
      <linkto id="632516967055630451" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632516967055630468" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">phoneIp</ap>
        <ap name="Username" type="literal">metreos</ap>
        <ap name="Password" type="literal">metreos</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632516967055630447" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="507.942017" y="397">
      <linkto id="632516967055630446" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="URL1" type="csharp">"http://" + g_appServerIp + ":8000/PushCallStats?device=" + g_deviceName</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632516967055630448" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="179.942047" y="397">
      <linkto id="632516967055630449" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632516967055630450" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="variable">g_deviceName</ap>
        <rd field="ResultData">queryResults</rd>
      </Properties>
    </node>
    <node type="Action" id="632516967055630449" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="323.942047" y="397">
      <linkto id="632516967055630450" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632516967055630447" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties language="csharp">
public static string Execute(DataTable queryResults, string g_deviceName, ref string phoneIp, LogWriter log, ref string g_phoneIp)
{
	bool success = false;
	try
	{
		phoneIp = queryResults.Rows[0]["IP"] as string;
		g_phoneIp = phoneIp;
		if(phoneIp == null || phoneIp == String.Empty)
		{
			log.Write(TraceLevel.Error, "The IP of the device {0} was not defined after DLX query, but was in the DLX results", g_deviceName);
		}
		else
		{
			success = true;
		}
	}
	catch(Exception e)
	{
		log.Write(TraceLevel.Error, "Unable to retreive IP address for the device {0}", g_deviceName);
	}

	if(success) return "success";
	else		return "failure";

}
</Properties>
    </node>
    <node type="Action" id="632516967055630450" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="179.942047" y="565">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="csharp">String.Format("DeviceList Query Failed for Device {0}", deviceName)</log>
      </Properties>
    </node>
    <node type="Action" id="632516967055630451" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="691.942" y="541">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="csharp">String.Format("Unable to send execute message to phone device {0}", deviceName)</log>
      </Properties>
    </node>
    <node type="Action" id="632516967055630468" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="870.884033" y="398">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632516983129010844" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="632516983129010845" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">deviceName</Properties>
    </node>
    <node type="Variable" id="632516983129010847" name="phoneIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">phoneIp</Properties>
    </node>
    <node type="Variable" id="632516983129010848" name="queryResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">queryResults</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiHangup" startnode="632516967055630444" treenode="632516967055630445" appnode="632516967055630442" handlerfor="632516967055630441">
    <node type="Start" id="632516967055630444" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="239">
      <linkto id="632516967055630461" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632516967055630460" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="408.0983" y="240">
      <linkto id="632516967055630471" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">g_phoneIp</ap>
        <ap name="Username" type="literal">metreos</ap>
        <ap name="Password" type="literal">metreos</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632516967055630461" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="224.0983" y="240">
      <linkto id="632516967055630460" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="URL1" type="literal">Key:Services</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632516967055630471" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="601" y="237">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632516967055630469" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" initWith="RoutingGuid" refType="reference">execute</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>