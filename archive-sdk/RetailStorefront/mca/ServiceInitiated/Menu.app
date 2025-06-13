<Application name="Menu" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Menu">
    <outline>
      <treenode type="evh" id="632875147412707623" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632875147412707620" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875147412707619" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/Menu</ep>
        </Properties>
      </treenode>
      <treenode type="fun" id="632875376037936885" level="1" text="CheckFileExists">
        <node type="function" name="CheckFileExists" id="632875376037936882" path="Metreos.StockTools" />
        <calls>
          <ref actid="632875376037936881" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_device" id="633168961358425831" vid="632898133783976920">
        <Properties type="String">g_device</Properties>
      </treenode>
      <treenode text="g_mainMenuClosedPath" id="633168961358425833" vid="632898133783976931">
        <Properties type="String" initWith="MainMenuClosedFilePath">g_mainMenuClosedPath</Properties>
      </treenode>
      <treenode text="g_mainMenuPath" id="633168961358425835" vid="632898133783976933">
        <Properties type="String" initWith="MainMenuFilePath">g_mainMenuPath</Properties>
      </treenode>
      <treenode text="g_dbUsername" id="633168961358425837" vid="632986053913955827">
        <Properties type="String" initWith="DatabaseUser">g_dbUsername</Properties>
      </treenode>
      <treenode text="g_dbPassword" id="633168961358425839" vid="632986053913955829">
        <Properties type="String" initWith="DatabasePassword">g_dbPassword</Properties>
      </treenode>
      <treenode text="g_mceadminInstallPath" id="633168961358425906" vid="633168961358425905">
        <Properties type="String" initWith="Config.MceAdminInstallPath">g_mceadminInstallPath</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632875147412707622" treenode="632875147412707623" appnode="632875147412707620" handlerfor="632875147412707619">
    <node type="Loop" id="632875376037936320" name="Loop" text="loop (expr)" cx="154" cy="148" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="1534" y="55" mx="1611" my="129">
      <linkto id="632875376037936321" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632875147412707696" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">checkedLoggedIn.Rows</Properties>
    </node>
    <node type="Loop" id="632904600971400567" name="Loop" text="loop (expr)" cx="280.188843" cy="164" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="1411.00232" y="543" mx="1551" my="625">
      <linkto id="632904600971400570" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632875376037936881" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">checkedLoggedIn.Rows</Properties>
    </node>
    <node type="Start" id="632875147412707622" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="72" y="304">
      <linkto id="632896050178120256" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707686" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="638.0023" y="304">
      <linkto id="632875147412707687" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632898160147305349" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">accept != null ? accept.IndexOf("image/png") &gt; -1 : false</ap>
      </Properties>
    </node>
    <node type="Action" id="632875147412707687" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="792.0023" y="240">
      <linkto id="632875147412707689" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Prompt" type="literal">Make your selection</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707689" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="920" y="240">
      <linkto id="632875147412707694" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Store Inventory</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/StoreInventory?metreosSessionId=" + query["appId"]</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707693" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1325" y="240">
      <linkto id="632875376037936319" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Check Out</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/CheckOut?metreosSessionId=" + query["appId"]</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707694" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1157" y="240">
      <linkto id="632875147412707693" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Check In</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/CheckIn?metreosSessionId=" + query["appId"]</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707696" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1909" y="240">
      <linkto id="632875376037936312" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936310" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1909" y="424">
      <linkto id="632875376037936312" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">graphicMenu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936312" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2037" y="360">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632875376037936314" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="200" y="304">
      <linkto id="632875376037936316" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="literal">RetailDemo</ap>
        <ap name="Server" type="literal">127.0.0.1</ap>
        <ap name="Port" type="literal">3306</ap>
        <ap name="Username" type="variable">g_dbUsername</ap>
        <ap name="Password" type="variable">g_dbPassword</ap>
        <ap name="Pooling" type="literal">true</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936316" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="292" y="304">
      <linkto id="632875376037936317" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">Retail</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936317" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="390" y="304">
      <linkto id="632898037366592803" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="literal">SELECT username, extension, userId, checkedIn from users</ap>
        <ap name="Name" type="literal">Retail</ap>
        <rd field="ResultSet">checkedLoggedIn</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936319" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1461" y="240">
      <linkto id="632875376037936320" port="1" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632875147412707696" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">checkedLoggedIn.Rows.Count &gt; 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936321" name="AddMenuItem" container="632875376037936320" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1622" y="127">
      <linkto id="632875376037936320" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">(loopEnum.Current as DataRow)["username"].ToString()</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/Call?metreosSessionId=" + query["appId"] + "&amp;ext=" + (loopEnum.Current as DataRow)["extension"] + "&amp;userId=" + (loopEnum.Current as DataRow)["userId"]</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936881" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1717" y="408" mx="1764" my="424">
      <items count="1">
        <item text="CheckFileExists" />
      </items>
      <linkto id="632875376037936310" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">CheckFileExists</ap>
      </Properties>
    </node>
    <node type="Action" id="632896050178120256" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="70.90625" y="48">
      <linkto id="632875376037936314" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string g_device, Metreos.Types.Http.QueryParamCollection query)
{
	//x1, y1, x2, y2
	g_device = query["device"];


	return String.Empty;
}
</Properties>
    </node>
    <node type="Action" id="632898037366592803" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="473" y="304">
      <linkto id="632898037366592806" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632875147412707686" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">query["open"] == null || query["open"] == String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632898037366592806" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="552" y="452">
      <linkto id="632875147412707686" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">"INSERT INTO preferences (open, device) VALUES (" + query["open"] + ", '" + g_device + "') ON DUPLICATE KEY UPDATE open = " + query["open"]</ap>
        <ap name="Name" type="literal">Retail</ap>
      </Properties>
    </node>
    <node type="Action" id="632898160147305349" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="638.0137" y="619">
      <linkto id="632898160147305350" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">"SELECT open from preferences WHERE device = '" + g_device + "'"</ap>
        <ap name="Name" type="literal">Retail</ap>
        <rd field="ResultSet">checkPref</rd>
      </Properties>
    </node>
    <node type="Action" id="632898160147305350" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="784" y="620">
      <linkto id="632904600971400574" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(DataTable checkedLoggedIn, DataTable checkPref, ref bool anyAvailable, ref bool open, ref string callNumber, ref string callName, ref string callId, Hashtable positions)
{
	positions["11111"] = new string[] {"7", "104", "206", "124"};
	positions["22222"] = new string[] {"7", "125", "206", "144"};
	positions["33333"] = new string[] {"7", "145", "206", "164"};

	open = !(checkPref == null || checkPref.Rows.Count == 0 || checkPref.Rows[0]["open"].ToString() == "0");
	
	anyAvailable = false;
	if(checkedLoggedIn != null)
	{
		foreach(DataRow row in checkedLoggedIn.Rows)
		{
			anyAvailable = row["checkedIn"].ToString() == "1";
			if(anyAvailable)
			{
				callId = row["userId"] as string;
				callNumber = row["extension"] as string;
				callName = row["username"] as string;
				break;
			}

		}
	}
	

	return String.Empty;
}
</Properties>
    </node>
    <node type="Action" id="632904600971400568" name="AddMenuItem" container="632904600971400567" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1567" y="578">
      <linkto id="632904600971400569" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">(loopEnum.Current as DataRow)["username"].ToString()</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/Call?metreosSessionId=" + query["appId"] + "&amp;ext=" + (loopEnum.Current as DataRow)["extension"] + "&amp;userId=" + (loopEnum.Current as DataRow)["userId"]</ap>
        <ap name="TouchAreaX1" type="csharp">(positions[(loopEnum.Current as DataRow)["userId"]] as string[])[0]</ap>
        <ap name="TouchAreaX2" type="csharp">(positions[(loopEnum.Current as DataRow)["userId"]] as string[])[2]</ap>
        <ap name="TouchAreaY1" type="csharp">(positions[(loopEnum.Current as DataRow)["userId"]] as string[])[1]</ap>
        <ap name="TouchAreaY2" type="csharp">(positions[(loopEnum.Current as DataRow)["userId"]] as string[])[3]</ap>
        <rd field="ResultData">graphicMenu</rd>
        <log condition="entry" on="true" level="Info" type="csharp">(loopEnum.Current as DataRow)["username"]</log>
      </Properties>
    </node>
    <node type="Action" id="632904600971400569" name="Assign" container="632904600971400567" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1653.189" y="624">
      <linkto id="632904600971400567" port="4" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">count + 1</ap>
        <rd field="ResultData">count</rd>
      </Properties>
    </node>
    <node type="Action" id="632904600971400570" name="If" container="632904600971400567" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1495" y="626">
      <linkto id="632904600971400568" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632904600971400569" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">(loopEnum.Current as DataRow)["checkedIn"].ToString() == "1"</ap>
      </Properties>
    </node>
    <node type="Action" id="632904600971400571" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="926.1887" y="475">
      <linkto id="632904600971400576" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Store Inventory</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/StoreInventory?metreosSessionId=" + query["appId"]</ap>
        <ap name="TouchAreaX1" type="literal">4</ap>
        <ap name="TouchAreaX2" type="literal">69</ap>
        <ap name="TouchAreaY1" type="literal">5</ap>
        <ap name="TouchAreaY2" type="literal">95</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632904600971400572" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1261.189" y="474">
      <linkto id="632904600971400573" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Update Status</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/UpdateStatus?metreosSessionId=" + query["appId"]</ap>
        <ap name="TouchAreaX1" type="literal">257</ap>
        <ap name="TouchAreaX2" type="literal">291</ap>
        <ap name="TouchAreaY1" type="literal">5</ap>
        <ap name="TouchAreaY2" type="literal">32</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632904600971400573" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1369.18884" y="474">
      <linkto id="632904600971400567" port="1" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632875376037936881" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">open</ap>
      </Properties>
    </node>
    <node type="Action" id="632904600971400574" name="CreateGraphicFileMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="784.1888" y="474">
      <linkto id="632904600971400571" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Prompt" type="literal">Make your selection</ap>
        <ap name="LocationX" type="literal">-1</ap>
        <ap name="LocationY" type="literal">-1</ap>
        <ap name="URL" type="csharp">open ? "http://" + hostname + "/mceadmin/menu.png" : "http://" + hostname + "/mceadmin/menuClosed.png"</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Comment" id="632904600971400575" text="Build 7970/IP Communicator menu&#xD;&#xA;(png graphics)" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="702" y="416" />
    <node type="Action" id="632904600971400576" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1009" y="474">
      <linkto id="632904600971400577" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632904600971400578" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">callNumber != "NONE"</ap>
      </Properties>
    </node>
    <node type="Action" id="632904600971400577" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1081" y="602">
      <linkto id="632904600971400578" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">callName</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/Call?metreosSessionId=" + query["appId"] + "&amp;ext=" + callNumber + "&amp;userId=" + callId + "&amp;name=" + callName.Replace(" ", "%20")</ap>
        <ap name="TouchAreaX1" type="literal">75</ap>
        <ap name="TouchAreaX2" type="literal">141</ap>
        <ap name="TouchAreaY1" type="literal">5</ap>
        <ap name="TouchAreaY2" type="literal">95</ap>
        <rd field="ResultData">graphicMenu</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"http://" + host + "/Retail/Call?metreosSessionId=" + query["appId"] + "&amp;ext=" + callNumber + "&amp;userId=" + callId + "&amp;name=" + callName.Replace(" ", "%20")</log>
      </Properties>
    </node>
    <node type="Action" id="632904600971400578" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1166" y="474">
      <linkto id="632904600971400572" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Expand</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/Menu?appId=" + query["appId"] + "&amp;open=" + (open ? "0" : "1") + "&amp;device=" + g_device</ap>
        <ap name="TouchAreaX1" type="literal">148</ap>
        <ap name="TouchAreaX2" type="literal">213</ap>
        <ap name="TouchAreaY1" type="literal">5</ap>
        <ap name="TouchAreaY2" type="literal">95</ap>
        <rd field="ResultData">graphicMenu</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"http://" + host + "/Retail/Menu?appId=" + query["appId"] + "&amp;open=" + (open ? "0" : "1") + "&amp;device=" + g_device</log>
      </Properties>
    </node>
    <node type="Comment" id="632904600971400579" text="SKU Lookup button" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="875" y="509" />
    <node type="Comment" id="632904600971400580" text="Call inv helpdesk group" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1024" y="639" />
    <node type="Comment" id="632904600971400581" text="Toggle helpdesk" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1114" y="432" />
    <node type="Variable" id="632875147412707685" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">query</Properties>
    </node>
    <node type="Variable" id="632875147412707688" name="menu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">menu</Properties>
    </node>
    <node type="Variable" id="632875147412707695" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632875147412707697" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875376037936303" name="accept" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Accept" defaultInitWith="NONE" refType="reference">accept</Properties>
    </node>
    <node type="Variable" id="632875376037936305" name="graphicMenu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.GraphicFileMenu" refType="reference">graphicMenu</Properties>
    </node>
    <node type="Variable" id="632875376037936313" name="hostname" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="hostname" refType="reference" name="Metreos.Providers.Http.GotRequest">hostname</Properties>
    </node>
    <node type="Variable" id="632875376037936315" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632875376037936318" name="checkedLoggedIn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkedLoggedIn</Properties>
    </node>
    <node type="Variable" id="632875376037936331" name="count" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" defaultInitWith="0" refType="reference">count</Properties>
    </node>
    <node type="Variable" id="632875376037936606" name="image" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Imaging.ImageBuilder" refType="reference">image</Properties>
    </node>
    <node type="Variable" id="632896050178120258" name="positions" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Hashtable" refType="reference">positions</Properties>
    </node>
    <node type="Variable" id="632898160147305345" name="checkPref" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkPref</Properties>
    </node>
    <node type="Variable" id="632898160147305346" name="anyAvailable" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" defaultInitWith="false" refType="reference">anyAvailable</Properties>
    </node>
    <node type="Variable" id="632898160147305347" name="open" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" defaultInitWith="false" refType="reference">open</Properties>
    </node>
    <node type="Variable" id="632898160147305353" name="callNumber" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="NONE" refType="reference">callNumber</Properties>
    </node>
    <node type="Variable" id="632898160147305354" name="callName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">callName</Properties>
    </node>
    <node type="Variable" id="632898160147305355" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="CheckFileExists" startnode="632875376037936884" treenode="632875376037936885" appnode="632875376037936882" handlerfor="632875147412707619">
    <node type="Start" id="632875376037936884" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="328">
      <linkto id="632898581293710545" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632898581293710542" name="CreateImageBuilder" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="299.90625" y="240">
      <linkto id="632898581293710543" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <ap name="BackgroundColor" type="csharp">System.Drawing.Color.Black</ap>
        <rd field="ImageBuilder">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632898581293710543" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="452.90625" y="240">
      <linkto id="632898581293710544" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="variable">g_mainMenuPath</ap>
        <ap name="Top" type="literal">0</ap>
        <ap name="Left" type="literal">0</ap>
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <rd field="Image">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632898581293710544" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="581.90625" y="240">
      <linkto id="632898581293710551" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
          public static string Execute(Metreos.Types.Imaging.ImageBuilder image, string g_mceadminInstallPath)
          {

          image.Save(g_mceadminInstallPath + "\\public\\menu.png");

          return String.Empty;
          }
      </Properties>
    </node>
    <node type="Action" id="632898581293710545" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="180.90625" y="328">
      <linkto id="632898581293710542" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632898581293710551" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
          public static string Execute(string g_mceadminInstallPath)
          {
          return System.IO.File.Exists(g_mceadminInstallPath + "\\public\\menu.png").ToString();
          }
      </Properties>
    </node>
    <node type="Comment" id="632898581293710546" text="Check if the basic background menu exists..." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="284.90625" y="366" />
    <node type="Action" id="632898581293710547" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1212.06836" y="329">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632898581293710548" name="CreateImageBuilder" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="764" y="240">
      <linkto id="632898581293710549" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <ap name="BackgroundColor" type="csharp">System.Drawing.Color.Black</ap>
        <rd field="ImageBuilder">imageClosed</rd>
      </Properties>
    </node>
    <node type="Action" id="632898581293710549" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="921.90625" y="241">
      <linkto id="632898581293710550" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="variable">g_mainMenuClosedPath</ap>
        <ap name="Top" type="literal">0</ap>
        <ap name="Left" type="literal">0</ap>
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <rd field="Image">imageClosed</rd>
      </Properties>
    </node>
    <node type="Action" id="632898581293710550" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1074.90625" y="241">
      <linkto id="632898581293710547" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
          public static string Execute(Metreos.Types.Imaging.ImageBuilder imageClosed, string g_mceadminInstallPath)
          {

          imageClosed.Save(g_mceadminInstallPath + "\\public\\menuClosed.png");

          return "";
          }
      </Properties>
    </node>
    <node type="Action" id="632898581293710551" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="672" y="326">
      <linkto id="632898581293710548" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632898581293710547" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
          public static string Execute(string g_mceadminInstallPath)
          {
          return System.IO.File.Exists(g_mceadminInstallPath + "\\public\\menuClosed.png").ToString();
          }
      </Properties>
    </node>
    <node type="Variable" id="632875376037936897" name="image" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Imaging.ImageBuilder" refType="reference">image</Properties>
    </node>
    <node type="Variable" id="632898581293710562" name="imageClosed" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Imaging.ImageBuilder" refType="reference">imageClosed</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>