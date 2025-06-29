<Application name="Logout" trigger="Metreos.Providers.Http.GotRequest" version="0.7" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="Logout">
    <outline>
      <treenode type="evh" id="632338193022187726" level="1" text="Metreos.Providers.Http.GotRequest (trigger): LogoutUser">
        <node type="function" name="LogoutUser" id="632338193022187723" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632338193022187722" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ExtensionMobility/Logout</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_ProxyUserId" id="632338193022188012" vid="632338193022187731">
        <Properties type="String" initWith="ProxyUserId">g_ProxyUserId</Properties>
      </treenode>
      <treenode text="g_ProxyPassword" id="632338193022188014" vid="632338193022187733">
        <Properties type="String" initWith="ProxyUserPassword">g_ProxyPassword</Properties>
      </treenode>
      <treenode text="g_LoginUrl" id="632338193022188016" vid="632338193022187737">
        <Properties type="String" initWith="LoginURL">g_LoginUrl</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="LogoutUser" activetab="true" startnode="632338193022187725" treenode="632338193022187726" appnode="632338193022187723" handlerfor="632338193022187722">
    <node type="Start" id="632338193022187725" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="368">
      <linkto id="632338193022187729" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632338193022187727" name="Logout" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="344" y="368">
      <linkto id="632338193022187746" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632338193022187755" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native">
        <ap name="AppId" type="variable">g_ProxyUserId</ap>
        <ap name="AppCertificate" type="variable">g_ProxyPassword</ap>
        <ap name="DeviceName" type="variable">deviceName</ap>
        <ap name="Url" type="variable">g_LoginUrl</ap>
        <rd field="ErrorCode">errorCode</rd>
        <rd field="ErrorMessage">errorMessage</rd>
        <log condition="entry" on="true" level="Info" type="literal">Attempting to log out user</log>
        <log condition="success" on="true" level="Info" type="literal">Logged out user successfully</log>
        <log condition="default" on="true" level="Info" type="literal">Failed to log out user</log>
      </Properties>
    </node>
    <node type="Action" id="632338193022187729" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="184" y="368">
      <linkto id="632338193022187727" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632338193022187742" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <Properties language="csharp">
	public static string Execute(Metreos.Types.Http.QueryParamCollection queryParams, ref string deviceName)
	{
		deviceName = queryParams["device"];

		if(deviceName == String.Empty || deviceName == null)
		{
			return "failure";
		}
		else
		{
			return "success";
		}
	}
</Properties>
    </node>
    <node type="Action" id="632338193022187742" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="184" y="504">
      <linkto id="632338193022187743" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="literal">This device is not configured properly in CallManager</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
      </Properties>
    </node>
    <node type="Action" id="632338193022187743" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="184" y="632">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632338193022187745" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="344" y="632">
      <linkto id="632338193022187748" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Title" type="literal">Logout failed</ap>
        <ap name="Prompt" type="literal">Logout Again</ap>
        <ap name="Text" type="variable">formattedErrorMessage</ap>
        <rd field="ResultData">errorText</rd>
      </Properties>
    </node>
    <node type="Action" id="632338193022187746" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="344" y="504">
      <linkto id="632338193022187745" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(string errorMessage, string errorCode, ref string formattedErrorMessage)
	{
		formattedErrorMessage = String.Format(@"
Unable to log out user

CallManager Error Message:
{0}

CallManager Error Code:
{1}", errorMessage, errorCode);
	
		return "success";
	}
</Properties>
    </node>
    <node type="Action" id="632338193022187748" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="344" y="744">
      <linkto id="632338193022187749" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Logout</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/ExtensionMobility/Logout"</ap>
        <rd field="ResultData">errorText</rd>
      </Properties>
    </node>
    <node type="Action" id="632338193022187749" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="344" y="848">
      <linkto id="632338193022187750" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">errorText</rd>
      </Properties>
    </node>
    <node type="Action" id="632338193022187750" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="344" y="960">
      <linkto id="632338193022187751" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">errorText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
      </Properties>
    </node>
    <node type="Action" id="632338193022187751" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="344" y="1064">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632338193022187752" text="Check if the device parameter is specified, &#xD;&#xA;and assign it to 'deviceName' variable.&#xD;&#xA;&#xD;&#xA;If it is not specified, warn user that their device&#xD;&#xA;is not configured correctly" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="56" y="240" />
    <node type="Comment" id="632338193022187753" text="Log user out" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="304" y="320" />
    <node type="Comment" id="632338193022187754" text="Logout text message will only show for a split-second" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="408" y="256" />
    <node type="Action" id="632338193022187755" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="520" y="368">
      <linkto id="632338193022187756" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Title" type="literal">Logout Successful</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632338193022187756" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="640" y="368">
      <linkto id="632338193022187757" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
      </Properties>
    </node>
    <node type="Action" id="632338193022187757" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="768" y="368">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632338193022187728" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParams</Properties>
    </node>
    <node type="Variable" id="632338193022187730" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">deviceName</Properties>
    </node>
    <node type="Variable" id="632338193022187735" name="errorCode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">errorCode</Properties>
    </node>
    <node type="Variable" id="632338193022187736" name="errorMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">errorMessage</Properties>
    </node>
    <node type="Variable" id="632338193022187739" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632338193022187740" name="errorText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">errorText</Properties>
    </node>
    <node type="Variable" id="632338193022187741" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632338193022187744" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632338193022187747" name="formattedErrorMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">formattedErrorMessage</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>