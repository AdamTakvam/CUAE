<Application name="DispositionRequest" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="DispositionRequest">
    <outline>
      <treenode type="evh" id="632792624545791726" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632792624545791723" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632792624545791722" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/DoNotCall/Disposition</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632793214843695076" level="2" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="632793214843695073" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="632793214843695072" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632793214843695084" level="2" text="Metreos.Providers.Http.GotRequest: Add">
        <node type="function" name="Add" id="632793214843695081" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632793214843695080" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Disposition/Add</ep>
        </Properties>
      </treenode>
      <treenode type="fun" id="632793087463411141" level="1" text="AddToDatabase">
        <node type="function" name="AddToDatabase" id="632793087463411138" path="Metreos.StockTools" />
        <calls>
          <ref actid="632793087463411137" />
          <ref actid="632793214843695086" />
        </calls>
      </treenode>
      <treenode type="fun" id="632793214843695027" level="1" text="Confirm">
        <node type="function" name="Confirm" id="632793214843695024" path="Metreos.StockTools" />
        <calls>
          <ref actid="632793214843695023" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_dbUsername" id="632796826816036667" vid="632793087463410108">
        <Properties type="String" initWith="DbUsername">g_dbUsername</Properties>
      </treenode>
      <treenode text="g_dbPassword" id="632796826816036669" vid="632793087463410110">
        <Properties type="String" initWith="DbPassword">g_dbPassword</Properties>
      </treenode>
      <treenode text="g_dbHost" id="632796826816036671" vid="632793087463410112">
        <Properties type="String" initWith="DbHost">g_dbHost</Properties>
      </treenode>
      <treenode text="g_dbLicense" id="632796826816036673" vid="632793087463410114">
        <Properties type="String" initWith="DbLicense">g_dbLicense</Properties>
      </treenode>
      <treenode text="g_confirm" id="632796826816036675" vid="632793087463411135">
        <Properties type="Bool">g_confirm</Properties>
      </treenode>
      <treenode text="phoneDn" id="632796826816036677" vid="632793087463411142">
        <Properties type="String">phoneDn</Properties>
      </treenode>
      <treenode text="toNumber" id="632796826816036679" vid="632793087463411144">
        <Properties type="String">toNumber</Properties>
      </treenode>
      <treenode text="g_remoteIp" id="632796826816036681" vid="632793214843695063">
        <Properties type="String">g_remoteIp</Properties>
      </treenode>
      <treenode text="g_useMetreosDnc" id="632796826816036683" vid="632796826816036180">
        <Properties type="String" initWith="UseMetreosDNC">g_useMetreosDnc</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632792624545791725" treenode="632792624545791726" appnode="632792624545791723" handlerfor="632792624545791722">
    <node type="Start" id="632792624545791725" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="386">
      <linkto id="632792624545791736" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632792624545791736" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="122" y="387">
      <linkto id="632793087463409970" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Http.QueryParamCollection queryParams, ref string deviceName, ref bool g_confirm, ref string g_remoteIp, string remoteIp)
{
	g_remoteIp = remoteIp;
	g_confirm = queryParams["confirm"] == "true" || queryParams["confirm"] == null || queryParams["confirm"] == String.Empty;

	deviceName = queryParams["device"];

	if(deviceName == null || deviceName == String.Empty)
	{
		return "nodevice";
	}
	else
	{
		return "success";
	}
}
</Properties>
    </node>
    <node type="Action" id="632793087463409970" name="GetDeviceInformation" class="MaxActionNode" group="" path="Metreos.Native.VendorDb" x="245" y="388">
      <linkto id="632793214843695040" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneIP" type="variable">remoteIp</ap>
        <rd field="PhoneDN">phoneDn</rd>
      </Properties>
    </node>
    <node type="Action" id="632793087463411137" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="676.634766" y="450" mx="724" my="466">
      <items count="1">
        <item text="AddToDatabase" />
      </items>
      <linkto id="632793214843695028" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">AddToDatabase</ap>
      </Properties>
    </node>
    <node type="Action" id="632793214843695020" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="611" y="388">
      <linkto id="632793087463411137" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632793214843695023" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_confirm</ap>
      </Properties>
    </node>
    <node type="Action" id="632793214843695023" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="682.8258" y="278" mx="720" my="294">
      <items count="1">
        <item text="Confirm" />
      </items>
      <linkto id="632793214843695028" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="host" type="variable">host</ap>
        <ap name="RoutingGuid" type="variable">routingGuid</ap>
        <ap name="FunctionName" type="literal">Confirm</ap>
      </Properties>
    </node>
    <node type="Action" id="632793214843695028" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="842" y="391">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632793214843695030" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="680" y="136">
      <linkto id="632793214843695033" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">noLatestCallFound.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632793214843695031" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="456" y="136">
      <linkto id="632793214843695032" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Unable to Process Request</ap>
        <ap name="Text" type="literal">Could not find the latest call to mark as DNC.</ap>
        <rd field="ResultData">noLatestCallFound</rd>
      </Properties>
    </node>
    <node type="Action" id="632793214843695032" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="576" y="136">
      <linkto id="632793214843695030" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">noLatestCallFound</rd>
      </Properties>
    </node>
    <node type="Label" id="632793214843695033" text="s" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="776" y="136" />
    <node type="Label" id="632793214843695034" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="392" y="136">
      <linkto id="632793214843695031" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793214843695040" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="348.013672" y="387">
      <linkto id="632793214843695041" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">String.Format("SELECT * from latestCall where fromNumber = '{0}'", phoneDn)</ap>
        <ap name="Name" type="literal">DoNotCall</ap>
        <rd field="ResultSet">checkDbResults</rd>
      </Properties>
    </node>
    <node type="Action" id="632793214843695041" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="462.013672" y="387">
      <linkto id="632793214843695042" type="Labeled" style="Bezier" ortho="true" label="noToNumber" />
      <linkto id="632793214843695043" type="Labeled" style="Bezier" ortho="true" label="noEntry" />
      <linkto id="632793214843695020" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(DataTable checkDbResults, ref string toNumber)
{
	if(checkDbResults == null || checkDbResults.Rows.Count == 0)
	{
		return "noEntry";
	}
	else
	{
		toNumber = checkDbResults.Rows[0]["toNumber"] as string;
		if(toNumber == null || toNumber == String.Empty)
		{
			return "noToNumber";
		}
	}

	return "success";
}
</Properties>
    </node>
    <node type="Label" id="632793214843695042" text="t" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="530.0137" y="474" />
    <node type="Label" id="632793214843695043" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="405.013672" y="474" />
    <node type="Action" id="632793214843695048" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="680" y="200">
      <linkto id="632793214843695051" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">noToNumber.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632793214843695049" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="456" y="200">
      <linkto id="632793214843695050" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Unable to Process Request</ap>
        <ap name="Text" type="literal">Unable to determine the number last dialed.</ap>
        <rd field="ResultData">noToNumber</rd>
      </Properties>
    </node>
    <node type="Action" id="632793214843695050" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="576" y="200">
      <linkto id="632793214843695048" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">noToNumber</rd>
      </Properties>
    </node>
    <node type="Label" id="632793214843695051" text="s" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="776" y="200" />
    <node type="Label" id="632793214843695052" text="t" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="384" y="200">
      <linkto id="632793214843695049" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632793214843695053" text="Error paths" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="352" y="240" />
    <node type="Action" id="632793214843695088" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="960" y="168">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632793214843695089" text="s" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="888" y="168">
      <linkto id="632793214843695088" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Variable" id="632792624545791735" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">queryParams</Properties>
    </node>
    <node type="Variable" id="632792624545791737" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">deviceName</Properties>
    </node>
    <node type="Variable" id="632793062168128680" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632793062168128681" name="remoteIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteIpAddress" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteIp</Properties>
    </node>
    <node type="Variable" id="632793087463409989" name="checkDbResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkDbResults</Properties>
    </node>
    <node type="Variable" id="632793214843695078" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632793214843695093" name="noLatestCallFound" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">noLatestCallFound</Properties>
    </node>
    <node type="Variable" id="632793214843695094" name="noToNumber" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">noToNumber</Properties>
    </node>
    <node type="Variable" id="632793214843695095" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="632793214843695075" treenode="632793214843695076" appnode="632793214843695073" handlerfor="632793214843695072">
    <node type="Start" id="632793214843695075" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632793214843695077" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793214843695077" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="575" y="255">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Add" startnode="632793214843695083" treenode="632793214843695084" appnode="632793214843695081" handlerfor="632793214843695080">
    <node type="Start" id="632793214843695083" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="328">
      <linkto id="632793214843695086" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793214843695086" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="160" y="312" mx="207" my="328">
      <items count="1">
        <item text="AddToDatabase" />
      </items>
      <linkto id="632793214843695087" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">AddToDatabase</ap>
      </Properties>
    </node>
    <node type="Action" id="632793214843695087" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="360" y="328">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632793214843695085" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="AddToDatabase" activetab="true" startnode="632793087463411140" treenode="632793087463411141" appnode="632793087463411138" handlerfor="632793214843695080">
    <node type="Start" id="632793087463411140" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="348">
      <linkto id="632796826816035962" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793087463411146" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="722.0137" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632793087463411155" name="AddToGryphon" class="MaxActionNode" group="" path="Metreos.Native.VendorDb" x="298.013672" y="289">
      <linkto id="632793087463411166" type="Labeled" style="Bezier" ortho="true" label="Connectivity" />
      <linkto id="632793087463411168" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <linkto id="632793087463411162" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_dbUsername</ap>
        <ap name="Password" type="variable">g_dbPassword</ap>
        <ap name="Host" type="variable">g_dbHost</ap>
        <ap name="PhoneNumber" type="variable">toNumber</ap>
        <ap name="License" type="variable">g_dbLicense</ap>
        <ap name="ConnectionTimeout" type="literal">3</ap>
        <rd field="ReturnCode">addReturnCode</rd>
        <log condition="failure" on="true" level="Error" type="csharp">"General failure in connecting to Gryphon"</log>
        <log condition="Connectivity" on="true" level="Error" type="literal">Unable to communicate with Gryphon</log>
      </Properties>
    </node>
    <node type="Action" id="632793087463411156" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="430.013672" y="99">
      <linkto id="632793087463411170" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">unableAddNumber.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632793087463411157" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="203.013672" y="99">
      <linkto id="632793087463411158" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Unable to Process Request</ap>
        <ap name="Text" type="literal">Unable to communicate to the DNC database.</ap>
        <rd field="ResultData">unableAddNumber</rd>
        <log condition="entry" on="true" level="Error" type="csharp">"Error in communicating with Gryphon.  Result code: " + addReturnCode</log>
      </Properties>
    </node>
    <node type="Action" id="632793087463411158" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="321.013672" y="99">
      <linkto id="632793087463411156" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">unableAddNumber</rd>
      </Properties>
    </node>
    <node type="Action" id="632793087463411159" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="461.013672" y="619">
      <linkto id="632793087463411172" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">addNumberSuccess.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632793087463411160" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="235.013672" y="619">
      <linkto id="632793087463411161" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Added DNC Number</ap>
        <ap name="Text" type="csharp">String.Format("Successfully added {0} to the DNC database", toNumber);</ap>
        <rd field="ResultData">addNumberSuccess</rd>
      </Properties>
    </node>
    <node type="Action" id="632793087463411161" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="352.013672" y="619">
      <linkto id="632793087463411159" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">addNumberSuccess</rd>
      </Properties>
    </node>
    <node type="Action" id="632793087463411162" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="431.9557" y="289">
      <linkto id="632793087463411169" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632793087463411174" type="Labeled" style="Bezier" ortho="true" label="0" />
      <linkto id="632793087463411177" type="Labeled" style="Bezier" ortho="true" label="2" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">addReturnCode</ap>
      </Properties>
    </node>
    <node type="Action" id="632793087463411163" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="460.013672" y="551">
      <linkto id="632793087463411176" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">addNumberSuccess.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632793087463411164" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="231.013672" y="552">
      <linkto id="632793087463411165" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Added DNC Number</ap>
        <ap name="Text" type="csharp">String.Format("The number {0} is already in the DNC database", toNumber);</ap>
        <rd field="ResultData">addNumberSuccess</rd>
      </Properties>
    </node>
    <node type="Action" id="632793087463411165" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="351.013672" y="551">
      <linkto id="632793087463411163" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">addNumberSuccess</rd>
      </Properties>
    </node>
    <node type="Label" id="632793087463411166" text="c" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="237.955688" y="220" />
    <node type="Label" id="632793087463411167" text="c" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="138.955688" y="99">
      <linkto id="632793087463411157" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632793087463411168" text="c" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="349.9557" y="219" />
    <node type="Label" id="632793087463411169" text="c" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="516.9557" y="289" />
    <node type="Label" id="632793087463411170" text="s" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="522.9557" y="99" />
    <node type="Label" id="632793087463411171" text="s" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="609.9557" y="287">
      <linkto id="632793087463411146" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632793087463411172" text="s" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="545.9557" y="619" />
    <node type="Label" id="632793087463411173" text="0" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="153.955688" y="619">
      <linkto id="632793087463411160" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632793087463411174" text="0" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="521.9557" y="230" />
    <node type="Label" id="632793087463411175" text="d" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="151.955688" y="552">
      <linkto id="632793087463411164" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632793087463411176" text="s" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="549.9557" y="551" />
    <node type="Label" id="632793087463411177" text="d" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="516.9557" y="347.999969" />
    <node type="Comment" id="632793087463411184" text="Success!" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="87.01367" y="565" />
    <node type="Action" id="632796826816035962" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="139" y="348">
      <linkto id="632793087463411155" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632796826816036182" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_useMetreosDnc</ap>
      </Properties>
    </node>
    <node type="Action" id="632796826816036182" name="ExecuteScalar" class="MaxActionNode" group="" path="Metreos.Native.Database" x="304.794922" y="424">
      <linkto id="632796826816036183" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">"SELECT count(*) from metreosDnc where toNumber = " + toNumber</ap>
        <ap name="Name" type="literal">DoNotCall</ap>
        <rd field="Scalar">metreosDncCount</rd>
      </Properties>
    </node>
    <node type="Action" id="632796826816036183" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="402.265625" y="424">
      <linkto id="632796826816036187" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632796826816036188" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">metreosDncCount == 0</ap>
      </Properties>
    </node>
    <node type="Label" id="632796826816036187" text="d" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="518" y="369" />
    <node type="Action" id="632796826816036188" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="552" y="424">
      <linkto id="632796826816036189" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("INSERT INTO metreosDnc (toNumber) VALUES ('{0}')", toNumber)</ap>
        <ap name="Name" type="literal">DoNotCall</ap>
      </Properties>
    </node>
    <node type="Label" id="632796826816036189" text="0" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="649.9557" y="424" />
    <node type="Comment" id="632796826816036191" text="Check to see if DNC in Metreos db... if it is, say so, otherwise, add it" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="196" y="456" />
    <node type="Variable" id="632793087463411225" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632793087463410123" name="unableAddNumber" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">unableAddNumber</Properties>
    </node>
    <node type="Variable" id="632793087463410130" name="addNumberSuccess" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">addNumberSuccess</Properties>
    </node>
    <node type="Variable" id="632793214843695019" name="addReturnCode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">addReturnCode</Properties>
    </node>
    <node type="Variable" id="632796826816036186" name="metreosDncCount" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">metreosDncCount</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Confirm" startnode="632793214843695026" treenode="632793214843695027" appnode="632793214843695024" handlerfor="632793214843695080">
    <node type="Start" id="632793214843695026" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="360">
      <linkto id="632793214843695067" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793214843695067" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="200" y="360">
      <linkto id="632793214843695068" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Confirm</ap>
        <ap name="Text" type="csharp">toNumber + "\nwill be added to the DNC database."</ap>
        <rd field="ResultData">confirm</rd>
      </Properties>
    </node>
    <node type="Action" id="632793214843695068" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="320" y="360">
      <linkto id="632793214843695069" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Confirm</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Disposition/Add?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">confirm</rd>
      </Properties>
    </node>
    <node type="Action" id="632793214843695069" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="456" y="360">
      <linkto id="632793214843695070" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">confirm</rd>
      </Properties>
    </node>
    <node type="Action" id="632793214843695070" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="584" y="360">
      <linkto id="632793214843695071" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">confirm.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632793214843695071" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="704" y="360">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632793214843695029" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632793214843695065" name="confirm" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">confirm</Properties>
    </node>
    <node type="Variable" id="632793214843695066" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632793214843695079" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>