<Application name="Login" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Login">
    <outline>
      <treenode type="evh" id="632589366908797814" level="1" text="Metreos.Providers.Http.GotRequest (trigger): InputUsername">
        <node type="function" name="InputUsername" id="632338063425000165" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632338063425000164" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ExtensionMobility/InputUsername</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632589366908797815" level="1" text="Metreos.Providers.Http.GotRequest: LoginUser">
        <node type="function" name="LoginUser" id="632338063425000180" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632338063425000179" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_deviceName" id="632589366908797770" vid="632338063425000177">
        <Properties type="String">g_deviceName</Properties>
      </treenode>
      <treenode text="g_proxyUserId" id="632589366908797772" vid="632338063425000264">
        <Properties type="String" initWith="ProxyUserId">g_proxyUserId</Properties>
      </treenode>
      <treenode text="g_proxyUserPassword" id="632589366908797774" vid="632338063425000266">
        <Properties type="String" initWith="ProxyUserPassword">g_proxyUserPassword</Properties>
      </treenode>
      <treenode text="g_loginUrl" id="632589366908797776" vid="632338063425000268">
        <Properties type="String" initWith="LoginURL">g_loginUrl</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="InputUsername" startnode="632338063425000167" treenode="632589366660821129" appnode="632338063425000165" handlerfor="632338063425000164">
    <node type="Start" id="632338063425000167" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="360">
      <linkto id="632338063425000169" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632338063425000169" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="224" y="360">
      <linkto id="632338063425000213" type="Labeled" style="Bevel" label="failure" />
      <linkto id="632338063425000215" type="Labeled" style="Bevel" label="success" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">Starting Login Script</log>
	public static string Execute(Metreos.Types.Http.QueryParamCollection queryParams,
ref string g_deviceName, ref string userId)
	{
		string deviceName = queryParams["device"];
		
		if(deviceName == null || deviceName == String.Empty)
		{
			return "failure";
		}

		userId = queryParams["userid"];

		if(userId == null)
		{
			userId = String.Empty;
		}

		g_deviceName = deviceName;
		return "success";
	}</Properties>
    </node>
    <node type="Action" id="632338063425000213" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="224" y="496">
      <linkto id="632338063425000214" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="literal">CallManager Cisco IP Phone Service not set up correctly</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632338063425000214" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="224" y="624">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632338063425000215" name="CreateInput" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="416" y="360">
      <linkto id="632338063425000216" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Login</ap>
        <ap name="Prompt" type="literal">Enter UserID</ap>
        <ap name="URL" type="csharp">host + "/ExtensionMobility/Login?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632338063425000216" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="568" y="360">
      <linkto id="632338063425000219" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">UserID</ap>
        <ap name="QueryStringParam" type="literal">userid</ap>
        <ap name="DefaultValue" type="variable">userId</ap>
        <ap name="InputFlags" type="literal">A</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Comment" id="632338063425000217" text="Set g_deviceName to the incoming &#xD;&#xA;'device' query parameter value for&#xD;&#xA;future use.&#xD;&#xA;&#xD;&#xA;Set incoming userId to local variable if its&#xD;&#xA;present, otherwise set local userId to &#xD;&#xA;empty string.&#xD;&#xA;&#xD;&#xA;If the parameter is not present,&#xD;&#xA;either a non-Cisco IP Phone device&#xD;&#xA;is accessing this page, or CallManager&#xD;&#xA;CIP Service is not set up correctly to &#xD;&#xA;pass in query parameter &#xD;&#xA;device=#DEVICENAME#" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="112" y="104" />
    <node type="Comment" id="632338063425000218" text="Instruct the user to input their UserID" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="432" y="288" />
    <node type="Action" id="632338063425000219" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="712" y="360">
      <linkto id="632338063425000220" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">input.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632338063425000220" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="848" y="360">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632338063425000170" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632338063425000171" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632338063425000172" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632338063425000173" name="input" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Input" refType="reference">input</Properties>
    </node>
    <node type="Variable" id="632338063425000174" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParams</Properties>
    </node>
    <node type="Variable" id="632338193022187814" name="userId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">userId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="LoginUser" startnode="632338063425000182" treenode="632589366660821130" appnode="632338063425000180" handlerfor="632338063425000179">
    <node type="Start" id="632338063425000182" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="360">
      <linkto id="632338063425000226" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632338063425000226" name="Login" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="224" y="360">
      <linkto id="632338063425000274" type="Labeled" style="Bevel" label="default" />
      <linkto id="632338063425000281" type="Labeled" style="Bevel" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="AppId" type="variable">g_proxyUserId</ap>
        <ap name="AppCertificate" type="variable">g_proxyUserPassword</ap>
        <ap name="UserId" type="csharp">queryParams["userid"]</ap>
        <ap name="DeviceName" type="variable">g_deviceName</ap>
        <ap name="NoTimeout" type="literal">true</ap>
        <ap name="Url" type="variable">g_loginUrl</ap>
        <rd field="ErrorCode">errorCode</rd>
        <rd field="ErrorMessage">errorMessage</rd>
        <log condition="entry" on="true" level="Info" type="literal">Attempting to log in user</log>
        <log condition="success" on="true" level="Info" type="literal">User successfully logged in</log>
        <log condition="default" on="true" level="Info" type="literal">User failed to log in</log>
      </Properties>
    </node>
    <node type="Action" id="632338063425000274" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="224" y="480">
      <linkto id="632338063425000276" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
	public static string Execute(ref string formattedErrorMessage, string errorMessage, string errorCode)
	{
		if(errorMessage == null)
		{
			errorMessage = "No error message defined";
		}

		if(errorCode == null)
		{
			errorCode = "No error code defined";
		}

		formattedErrorMessage = String.Format(@"Unable to log in

Error Message from CallManager:
{0} 

Error Code from CallManager:
{1}", errorMessage, errorCode);

		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632338063425000276" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="224" y="600">
      <linkto id="632338063425000277" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Login failed</ap>
        <ap name="Prompt" type="literal">Return to Login</ap>
        <ap name="Text" type="variable">formattedErrorMessage</ap>
        <rd field="ResultData">errorText</rd>
      </Properties>
    </node>
    <node type="Action" id="632338063425000277" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="224" y="728">
      <linkto id="632338063425000282" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Login</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/ExtensionMobility/InputUsername?device=" + g_deviceName + "&amp;userid=" + queryParams["userid"]</ap>
        <rd field="ResultData">errorText</rd>
      </Properties>
    </node>
    <node type="Action" id="632338063425000278" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="224" y="968">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632338063425000279" text="Attempt to log in the user" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="152" y="304" />
    <node type="Comment" id="632338063425000280" text="Error message and a &#xD;&#xA;SoftKey to get back to the main application" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="296" y="608" />
    <node type="Action" id="632338063425000281" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="432" y="360">
      <linkto id="632338063425000283" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Successful</ap>
        <ap name="Text" type="literal">Login Successful</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632338063425000282" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="224" y="848">
      <linkto id="632338063425000278" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">errorText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632338063425000283" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="592" y="360">
      <linkto id="632338193022187718" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632338193022187718" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="724" y="360">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632338193022187719" text="Send phone message that they were logged in,&#xD;&#xA;though the Ext. Mobile profile change makes this &#xD;&#xA;show for only a split second" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="473" y="275" />
    <node type="Variable" id="632338063425000221" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParams</Properties>
    </node>
    <node type="Variable" id="632338063425000222" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632338063425000223" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632338063425000224" name="errorText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">errorText</Properties>
    </node>
    <node type="Variable" id="632338063425000225" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632338063425000270" name="errorMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">errorMessage</Properties>
    </node>
    <node type="Variable" id="632338063425000271" name="errorCode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">errorCode</Properties>
    </node>
    <node type="Variable" id="632338063425000273" name="formattedErrorMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">formattedErrorMessage</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>