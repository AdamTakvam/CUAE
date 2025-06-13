<Application name="Login" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Login">
    <outline>
      <treenode type="evh" id="632807973088997084" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632807973088997081" path="Metreos.StockTools" />
        <calls>
          <ref actid="632807973088997243" />
          <ref actid="632807973088997499" />
        </calls>
        <node type="event" name="GotRequest" id="632807973088997080" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Demo/Login</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632807973088997159" level="2" text="Metreos.Providers.Http.GotRequest: OnGotRequest1">
        <node type="function" name="OnGotRequest1" id="632807973088997156" path="Metreos.StockTools" />
        <calls>
          <ref actid="632807973088997794" />
          <ref actid="632807973088997836" />
        </calls>
        <node type="event" name="GotRequest" id="632807973088997155" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Demo/Panel</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632807973088997688" level="2" text="Metreos.Providers.Http.GotRequest: OnGotRequest2">
        <node type="function" name="OnGotRequest2" id="632807973088997685" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632807973088997684" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Demo/UpdateUser</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632807973088997693" level="2" text="Metreos.Providers.Http.GotRequest: OnGotRequest3">
        <node type="function" name="OnGotRequest3" id="632807973088997690" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632807973088997689" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Demo/DoAction1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632807973088997844" level="2" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="632807973088997841" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="632807973088997840" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_currentUser" id="632808064179578917" vid="632807973088997821">
        <Properties type="String">g_currentUser</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632807973088997083" treenode="632807973088997084" appnode="632807973088997081" handlerfor="632807973088997080">
    <node type="Start" id="632807973088997083" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="312">
      <linkto id="632807973088997148" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632807973088997148" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="216" y="312">
      <linkto id="632807973088997152" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string response, string host, string sessionVar, string routingGuid)
{
	response = "&lt;html&gt;&lt;body&gt;";
	response += "&lt;h2&gt;Login&lt;/h2&gt;";

	if(sessionVar != "NONE")
	{
		response += "&lt;span style=\"color:red\"&gt;" + sessionVar + "&lt;/span&gt;";
	}	

	response += "&lt;form method=\"post\" action=\"http://" + host + "/Demo/Panel?metreosSessionId=" + routingGuid + "\" &gt;";
	response += "&lt;label for=\"user\"&gt;Username:&lt;/label&gt;&lt;br /&gt;&lt;input type=\"text\" name=\"user\" /&gt;&lt;br /&gt;&lt;br /&gt;";
	response += "&lt;input type=\"submit\" name=\"add\" value=\"Login\" /&gt;";

	response += "&lt;/body&gt;&lt;/html&gt;";

	return "";
}
</Properties>
    </node>
    <node type="Comment" id="632807973088997150" text="Create login form" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="160" y="264" />
    <node type="Action" id="632807973088997152" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="392" y="312">
      <linkto id="632807973088997827" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="variable">response</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632807973088997153" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="744" y="312">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632807973088997827" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="512" y="312">
      <linkto id="632807973088997828" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="literal">demo</ap>
        <ap name="Server" type="literal">127.0.0.1</ap>
        <ap name="Port" type="literal">3306</ap>
        <ap name="Username" type="literal">root</ap>
        <ap name="Password" type="literal">metreos</ap>
        <ap name="Pooling" type="literal">true</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632807973088997828" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="624" y="312">
      <linkto id="632807973088997153" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">demo</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Comment" id="632807973088997831" text="You only need to open the database connection once per script..." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="384" y="264" />
    <node type="Variable" id="632807973088997085" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632807973088997086" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632807973088997087" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632807973088997151" name="sessionVar" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="sessionVar" defaultInitWith="NONE" refType="reference">sessionVar</Properties>
    </node>
    <node type="Variable" id="632807973088997154" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632807973088997981" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotRequest1" activetab="true" startnode="632807973088997158" treenode="632807973088997159" appnode="632807973088997156" handlerfor="632807973088997155">
    <node type="Start" id="632807973088997158" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="336">
      <linkto id="632807973088997237" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632807973088997237" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="144" y="336">
      <linkto id="632807973088997238" type="Labeled" style="Bezier" ortho="true" label="invalid" />
      <linkto id="632807973088997407" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">Registration request received</log>
public static string Execute(Metreos.Types.Http.FormCollection postVars, ref string user, ref string sessionVar, ref string g_currentUser)
{
	if(g_currentUser != String.Empty)
	{
		// This method must been reached through CallFunction.  
		//Assume current user is good, and assign name to local user var
		user = g_currentUser;
		return "continue";
	}

	user = postVars["user"];

	// 'Clean' inputs--if an empty string or whitespace, force it all into null
	if(user != null)
	{
		user = user.Trim();
		if(user == String.Empty)
		{
			user = null;
		}
	}	

	if(user == null)
	{
		return "invalid";
	}
	
	return "continue";
}
</Properties>
    </node>
    <node type="Action" id="632807973088997238" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="144" y="440">
      <linkto id="632807973088997243" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Username must be specified</ap>
        <rd field="ResultData">sessionVar</rd>
      </Properties>
    </node>
    <node type="Action" id="632807973088997243" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="104" y="536" mx="148" my="552">
      <items count="1">
        <item text="OnGotRequest" treenode="632807973088997084" />
      </items>
      <linkto id="632807973088997244" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="host" type="variable">host</ap>
        <ap name="sessionVar" type="variable">sessionVar</ap>
        <ap name="RoutingGuid" type="variable">routingGuid</ap>
        <ap name="FunctionName" type="literal">OnGotRequest</ap>
      </Properties>
    </node>
    <node type="Action" id="632807973088997244" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="144" y="680">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632807973088997407" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="288" y="336">
      <linkto id="632807973088997497" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">String.Format("SELECT * from users WHERE username = '{0}'", user);</ap>
        <ap name="Name" type="literal">demo</ap>
        <rd field="ResultSet">getUserResult</rd>
      </Properties>
    </node>
    <node type="Action" id="632807973088997496" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="424" y="688">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632807973088997497" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="424" y="336">
      <linkto id="632807973088997498" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632808064179578978" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">getUserResult.Rows.Count == 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632807973088997498" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="424" y="464">
      <linkto id="632807973088997499" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Username does not exist</ap>
        <rd field="ResultData">sessionVar</rd>
      </Properties>
    </node>
    <node type="Action" id="632807973088997499" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="384" y="536" mx="428" my="552">
      <items count="1">
        <item text="OnGotRequest" treenode="632807973088997084" />
      </items>
      <linkto id="632807973088997496" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="host" type="variable">host</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="RoutingGuid" type="variable">routingGuid</ap>
        <ap name="sessionVar" type="variable">sessionVar</ap>
        <ap name="FunctionName" type="literal">OnGotRequest</ap>
      </Properties>
    </node>
    <node type="Action" id="632807973088997504" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="670" y="337">
      <linkto id="632807973088997832" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string response, string host, string sessionVar, string routingGuid, DataTable getUserResult)
{
	response = "&lt;html&gt;&lt;body&gt;";
	response += "&lt;h2&gt;Control Panel&lt;/h2&gt;";
	response += "&lt;br /&gt;&lt;br /&gt;&lt;form method=\"post\" action=\"http://" + host + "/Demo/DoAction1?metreosSessionId=" + routingGuid + "\" &gt;";
	response += "&lt;input type=\"submit\" name=\"action1\" value=\"Do Action\" /&gt;&lt;/form&gt;";

	response += "&lt;br /&gt;&lt;br /&gt;&lt;h3&gt;Update Your Information&lt;/h3&gt;&lt;form method=\"post\" action=\"http://" + host + "/Demo/UpdateUser?metreosSessionId=" + routingGuid + "\" &gt;";
	response += "&lt;label for=\"first\"&gt;First Name:&lt;/label&gt;&lt;br /&gt;&lt;input type=\"text\" name=\"first\" value=\"" + getUserResult.Rows[0]["firstname"] + "\" /&gt;&lt;br /&gt;&lt;br /&gt;";
	response += "&lt;label for=\"last\"&gt;Last Name:&lt;/label&gt;&lt;br /&gt;&lt;input type=\"text\" name=\"last\" value=\"" + getUserResult.Rows[0]["lastname"] + "\"  /&gt;&lt;br /&gt;&lt;br /&gt;";
	response += "&lt;label for=\"phone\"&gt;Phone Number:&lt;/label&gt;&lt;br /&gt;&lt;input type=\"text\" name=\"phone\" value=\"" + getUserResult.Rows[0]["phone"] + "\" /&gt;&lt;br /&gt;&lt;br /&gt;";
	response += "&lt;input type=\"submit\" name=\"update\" value=\"Update\" /&gt;&lt;/form&gt;";

	response += "&lt;/body&gt;&lt;/html&gt;";

	return "";
}

</Properties>
    </node>
    <node type="Action" id="632807973088997832" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="782" y="337">
      <linkto id="632807973088997833" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="variable">response</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632807973088997833" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="910" y="337">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632808064179578978" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="540" y="336">
      <linkto id="632807973088997504" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">user</ap>
        <rd field="ResultData">g_currentUser</rd>
      </Properties>
    </node>
    <node type="Comment" id="632808064179578980" text="User has been validated--&#xD;&#xA;assign g_currentUser" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="442" y="248" />
    <node type="Variable" id="632807973088997160" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632807973088997161" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632807973088997162" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632807973088997241" name="postVars" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.FormCollection" initWith="body" defaultInitWith="NONE" refType="reference" name="Metreos.Providers.Http.GotRequest">postVars</Properties>
    </node>
    <node type="Variable" id="632807973088997242" name="sessionVar" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="sessionVar" defaultInitWith="NONE" refType="reference">sessionVar</Properties>
    </node>
    <node type="Variable" id="632807973088997411" name="getUserResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">getUserResult</Properties>
    </node>
    <node type="Variable" id="632807973088997982" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632808064179578979" name="user" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">user</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotRequest2" startnode="632807973088997687" treenode="632807973088997688" appnode="632807973088997685" handlerfor="632807973088997684">
    <node type="Start" id="632807973088997687" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="56" y="304">
      <linkto id="632807973088997793" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632807973088997793" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="232" y="304">
      <linkto id="632807973088997801" type="Labeled" style="Bezier" ortho="true" label="invalid" />
      <linkto id="632807973088997804" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">Registration request received</log>
public static string Execute(Metreos.Types.Http.FormCollection postVars, ref string first, ref string last, ref string phone, ref string sessionVar)
{
	first = postVars["first"];
	last = postVars["last"];
	phone = postVars["phone"];

	// 'Clean' inputs--if an empty string or whitespace, force it all into null	
	if(first != null)
	{
		first = first.Trim();
		if(first == String.Empty)
		{
			first = null;
		}
	}	
	if(last != null)
	{
		last = last.Trim();
		if(last == String.Empty)
		{
			last = null;
		}
	}	
	if(phone != null)
	{
		phone = phone.Trim();
		if(phone == String.Empty)
		{
			phone = null;
		}
	}	

	if(first == null || last == null || phone == null)
	{
		return "invalid";
	}
	
	return "continue";
}
</Properties>
    </node>
    <node type="Action" id="632807973088997794" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="188.663086" y="512" mx="236" my="528">
      <items count="1">
        <item text="OnGotRequest1" treenode="632807973088997159" />
      </items>
      <linkto id="632807973088997795" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="host" type="variable">host</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="RoutingGuid" type="variable">routingGuid</ap>
        <ap name="sessionVar" type="variable">sessionVar</ap>
        <ap name="FunctionName" type="literal">OnGotRequest1</ap>
      </Properties>
    </node>
    <node type="Action" id="632807973088997795" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="232" y="664">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632807973088997801" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="232" y="424">
      <linkto id="632807973088997794" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">All inputs must be specified</ap>
        <rd field="ResultData">sessionVar</rd>
      </Properties>
    </node>
    <node type="Action" id="632807973088997804" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="400" y="304">
      <linkto id="632807973088997838" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("UPDATE users set firstname = '{0}', lastname = '{1}', phone = '{2}' WHERE username = '{3}'", first, last, phone, g_currentUser);</ap>
        <ap name="Name" type="literal">demo</ap>
      </Properties>
    </node>
    <node type="Action" id="632807973088997806" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="784" y="304">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632807973088997836" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="608" y="288" mx="655" my="304">
      <items count="1">
        <item text="OnGotRequest1" treenode="632807973088997159" />
      </items>
      <linkto id="632807973088997806" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="host" type="variable">host</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="RoutingGuid" type="variable">routingGuid</ap>
        <ap name="sessionVar" type="variable">sessionVar</ap>
        <ap name="FunctionName" type="literal">OnGotRequest1</ap>
      </Properties>
    </node>
    <node type="Action" id="632807973088997838" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="528" y="304">
      <linkto id="632807973088997836" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Updated</ap>
        <rd field="ResultData">sessionVar</rd>
      </Properties>
    </node>
    <node type="Variable" id="632807973088997823" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632807973088997824" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632807973088997825" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632807973088997826" name="sessionVar" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="sessionVar" defaultInitWith="NONE" refType="reference">sessionVar</Properties>
    </node>
    <node type="Variable" id="632807973088997983" name="first" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">first</Properties>
    </node>
    <node type="Variable" id="632807973088997984" name="last" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">last</Properties>
    </node>
    <node type="Variable" id="632807973088997985" name="phone" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">phone</Properties>
    </node>
    <node type="Variable" id="632807973088997986" name="postVars" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.FormCollection" initWith="body" refType="reference" name="Metreos.Providers.Http.GotRequest">postVars</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotRequest3" startnode="632807973088997692" treenode="632807973088997693" appnode="632807973088997690" handlerfor="632807973088997689">
    <node type="Start" id="632807973088997692" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="328">
      <linkto id="632807973088997978" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632807973088997978" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="264" y="328">
      <linkto id="632807973088997979" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="literal">Poof!</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632807973088997979" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="504" y="328">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632807973088997980" text="With the user logged in, now we can actually do something per their request!!" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="112" y="264" />
    <node type="Variable" id="632807973088997975" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632807973088997976" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632807973088997977" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="632807973088997843" treenode="632807973088997844" appnode="632807973088997841" handlerfor="632807973088997840">
    <node type="Start" id="632807973088997843" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="464" y="240">
      <linkto id="632807973088997845" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632807973088997845" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="660" y="242">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>