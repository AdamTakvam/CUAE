<Application name="Register" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Register">
    <outline>
      <treenode type="evh" id="632807257980623458" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632807257980623455" path="Metreos.StockTools" />
        <calls>
          <ref actid="632807285797321892" />
          <ref actid="632807285797321910" />
        </calls>
        <node type="event" name="GotRequest" id="632807257980623454" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Demo/Register</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632807285797321879" level="2" text="Metreos.Providers.Http.GotRequest: OnGotRequest1">
        <node type="function" name="OnGotRequest1" id="632807285797321876" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632807285797321875" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Demo/Createuser</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632807973088997900" level="2" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="632807973088997897" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="632807973088997896" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632807257980623457" treenode="632807257980623458" appnode="632807257980623455" handlerfor="632807257980623454">
    <node type="Start" id="632807257980623457" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="376">
      <linkto id="632807257980623461" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632807257980623461" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="288" y="376">
      <linkto id="632807257980623463" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string response, string host, string sessionVar, string routingGuid)
{
	response = "&lt;html&gt;&lt;body&gt;";
	response += "&lt;h2&gt;New User Registration&lt;/h2&gt;";

	if(sessionVar != "NONE")
	{
		response += "&lt;span style=\"color:red\"&gt;" + sessionVar + "&lt;/span&gt;";
	}	

	response += "&lt;form method=\"post\" action=\"http://" + host + "/Demo/Createuser?metreosSessionId=" + routingGuid + "\" &gt;";
	response += "&lt;label for=\"user\"&gt;Username:&lt;/label&gt;&lt;br /&gt;&lt;input type=\"text\" name=\"user\" /&gt;&lt;br /&gt;&lt;br /&gt;";
	response += "&lt;label for=\"first\"&gt;First Name:&lt;/label&gt;&lt;br /&gt;&lt;input type=\"text\" name=\"first\" /&gt;&lt;br /&gt;&lt;br /&gt;";
	response += "&lt;label for=\"last\"&gt;Last Name:&lt;/label&gt;&lt;br /&gt;&lt;input type=\"text\" name=\"last\" /&gt;&lt;br /&gt;&lt;br /&gt;";
	response += "&lt;label for=\"phone\"&gt;Phone Number:&lt;/label&gt;&lt;br /&gt;&lt;input type=\"text\" name=\"phone\" /&gt;&lt;br /&gt;&lt;br /&gt;";
	response += "&lt;input type=\"submit\" name=\"add\" value=\"Register\" /&gt;&lt;/form&gt;";

	response += "&lt;/body&gt;&lt;/html&gt;";

	return "";
}
</Properties>
    </node>
    <node type="Comment" id="632807257980623462" text="Build up a new user form" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="216" y="312" />
    <node type="Action" id="632807257980623463" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="488" y="376">
      <linkto id="632807257980623467" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="variable">response</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Comment" id="632807257980623464" text="Set a query parm with the name-value pair,&#xD;&#xA;'metreosSessionId=RoutingGuid',&#xD;&#xA;to cause subsequent requests to come back to this script" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="392" y="288" />
    <node type="Action" id="632807257980623467" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="688" y="376">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632807257980623459" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632807257980623460" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632807257980623465" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632807257980623466" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632807285797321889" name="sessionVar" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="sessionVar" defaultInitWith="NONE" refType="reference">sessionVar</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotRequest1" activetab="true" startnode="632807285797321878" treenode="632807285797321879" appnode="632807285797321876" handlerfor="632807285797321875">
    <node type="Start" id="632807285797321878" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="328">
      <linkto id="632807285797321886" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632807285797321886" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="169" y="328">
      <linkto id="632807285797321898" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632807285797321907" type="Labeled" style="Bezier" ortho="true" label="invalid" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">Registration request received</log>
public static string Execute(Metreos.Types.Http.FormCollection postVars, ref string first, ref string last, ref string phone, ref string user, ref string sessionVar)
{
	user = postVars["user"];
	first = postVars["first"];
	last = postVars["last"];
	phone = postVars["phone"];

	// 'Clean' inputs--if an empty string or whitespace, force it all into null
	if(user != null)
	{
		user = user.Trim();
		if(user == String.Empty)
		{
			user = null;
		}
	}	
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

	if(first == null || last == null || phone == null || user == null)
	{
		return "invalid";
	}
	
	return "continue";
}
</Properties>
    </node>
    <node type="Comment" id="632807285797321887" text="Check the input variables" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="102" y="269" />
    <node type="Action" id="632807285797321892" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="128" y="536" mx="172" my="552">
      <items count="1">
        <item text="OnGotRequest" treenode="632807257980623458" />
      </items>
      <linkto id="632807285797321894" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="host" type="variable">host</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="RoutingGuid" type="variable">routingGuid</ap>
        <ap name="sessionVar" type="variable">sessionVar</ap>
        <ap name="FunctionName" type="literal">OnGotRequest</ap>
      </Properties>
    </node>
    <node type="Action" id="632807285797321894" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="168" y="688">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632807285797321895" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="712" y="680">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632807285797321896" text="With first, last, and phone specified,&#xD;&#xA;insert new user into db" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="752" y="248" />
    <node type="Comment" id="632807285797321897" text="check for existing user first" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="496" y="272" />
    <node type="Action" id="632807285797321898" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="328" y="328">
      <linkto id="632807285797321899" type="Labeled" style="Bezier" ortho="true" label="default" />
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
    <node type="Action" id="632807285797321899" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="440" y="328">
      <linkto id="632807285797321902" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">demo</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Action" id="632807285797321902" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="568" y="328">
      <linkto id="632807285797321904" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">String.Format("SELECT * from users WHERE username = '{0}'", user);</ap>
        <ap name="Name" type="literal">demo</ap>
        <rd field="ResultSet">checkExistingResults</rd>
      </Properties>
    </node>
    <node type="Action" id="632807285797321904" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="712" y="328">
      <linkto id="632807285797321908" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632807285797321912" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">checkExistingResults.Rows.Count == 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632807285797321907" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="168" y="448">
      <linkto id="632807285797321892" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">All inputs must be specified</ap>
        <rd field="ResultData">sessionVar</rd>
      </Properties>
    </node>
    <node type="Action" id="632807285797321908" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="712" y="456">
      <linkto id="632807285797321910" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Username already exists</ap>
        <rd field="ResultData">sessionVar</rd>
      </Properties>
    </node>
    <node type="Action" id="632807285797321910" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="672" y="528" mx="716" my="544">
      <items count="1">
        <item text="OnGotRequest" treenode="632807257980623458" />
      </items>
      <linkto id="632807285797321895" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="host" type="variable">host</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="RoutingGuid" type="variable">routingGuid</ap>
        <ap name="sessionVar" type="variable">sessionVar</ap>
        <ap name="FunctionName" type="literal">OnGotRequest</ap>
      </Properties>
    </node>
    <node type="Action" id="632807285797321912" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="837" y="329">
      <linkto id="632807973088997076" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("INSERT INTO users (username, firstname, lastname, phone) VALUES ('{0}', '{1}', '{2}', '{3}')", user, first, last, phone);</ap>
        <ap name="Name" type="literal">demo</ap>
      </Properties>
    </node>
    <node type="Action" id="632807973088997076" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="984.494141" y="331">
      <linkto id="632807973088997077" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="sessionVar" type="literal">You must login with your new account...</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">"&lt;html&gt;&lt;META HTTP-EQUIV=\"refresh\" content=\"3;URL=http://" + host + "/Demo/Login\"&gt;&lt;body&gt;&lt;p&gt;Account created.  Redirecting to login...&lt;/p&gt;&lt;/body&gt;&lt;/html&gt;"</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632807973088997077" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1160.49414" y="333">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632807285797321880" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632807285797321881" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632807285797321882" name="postVars" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.FormCollection" initWith="body" refType="reference">postVars</Properties>
    </node>
    <node type="Variable" id="632807285797321883" name="first" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">first</Properties>
    </node>
    <node type="Variable" id="632807285797321884" name="last" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">last</Properties>
    </node>
    <node type="Variable" id="632807285797321885" name="phone" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">phone</Properties>
    </node>
    <node type="Variable" id="632807285797321890" name="sessionVar" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="sessionVar" defaultInitWith="NONE" refType="reference">sessionVar</Properties>
    </node>
    <node type="Variable" id="632807285797321893" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632807285797321900" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632807285797321903" name="checkExistingResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkExistingResults</Properties>
    </node>
    <node type="Variable" id="632807285797321905" name="user" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">user</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="632807973088997899" treenode="632807973088997900" appnode="632807973088997897" handlerfor="632807973088997896">
    <node type="Start" id="632807973088997899" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="392" y="320">
      <linkto id="632807973088997901" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632807973088997901" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="544" y="320">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>