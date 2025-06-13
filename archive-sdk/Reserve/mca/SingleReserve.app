<Application name="SingleReserve" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="SingleReserve">
    <outline>
      <treenode type="evh" id="632732119900199509" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632732119900199506" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632732119900199505" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Reserve</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_triHost" id="632994440446577504" vid="632845355542565934">
        <Properties type="String" initWith="triHost">g_triHost</Properties>
      </treenode>
      <treenode text="g_triPort" id="632994440446577506" vid="632845355542565936">
        <Properties type="String" initWith="triPort">g_triPort</Properties>
      </treenode>
      <treenode text="g_triUser1" id="632994440446577508" vid="632845355542565663">
        <Properties type="String" defaultInitWith="NONE" initWith="triUser1">g_triUser1</Properties>
      </treenode>
      <treenode text="g_triPass1" id="632994440446577510" vid="632845355542565665">
        <Properties type="String" defaultInitWith="NONE" initWith="triPass1">g_triPass1</Properties>
      </treenode>
      <treenode text="g_triUser2" id="632994440446577512" vid="632845355542565667">
        <Properties type="String" defaultInitWith="NONE" initWith="triUser2">g_triUser2</Properties>
      </treenode>
      <treenode text="g_triPass2" id="632994440446577514" vid="632845355542565669">
        <Properties type="String" defaultInitWith="NONE" initWith="triPass2">g_triPass2</Properties>
      </treenode>
      <treenode text="g_triUser3" id="632994440446577516" vid="632845355542565671">
        <Properties type="String" defaultInitWith="NONE" initWith="triUser3">g_triUser3</Properties>
      </treenode>
      <treenode text="g_triPass3" id="632994440446577518" vid="632845355542565673">
        <Properties type="String" defaultInitWith="NONE" initWith="triPass3">g_triPass3</Properties>
      </treenode>
      <treenode text="g_triUser4" id="632994440446577520" vid="632845355542565675">
        <Properties type="String" defaultInitWith="NONE" initWith="triUser4">g_triUser4</Properties>
      </treenode>
      <treenode text="g_triPass4" id="632994440446577522" vid="632845355542565677">
        <Properties type="String" defaultInitWith="NONE" initWith="triPass4">g_triPass4</Properties>
      </treenode>
      <treenode text="g_triUser5" id="632994440446577524" vid="632845355542565679">
        <Properties type="String" defaultInitWith="NONE" initWith="triUser5">g_triUser5</Properties>
      </treenode>
      <treenode text="g_triPass5" id="632994440446577526" vid="632845355542565681">
        <Properties type="String" defaultInitWith="NONE" initWith="triPass5">g_triPass5</Properties>
      </treenode>
      <treenode text="g_triSecure" id="632994440446577528" vid="632845355542565932">
        <Properties type="Bool" initWith="triSecure">g_triSecure</Properties>
      </treenode>
      <treenode text="g_proxyUsername" id="632994440446577530" vid="632732214381185452">
        <Properties type="String" initWith="proxyUser">g_proxyUsername</Properties>
      </treenode>
      <treenode text="g_proxyPassword" id="632994440446577532" vid="632732214381185471">
        <Properties type="String" initWith="proxyPass">g_proxyPassword</Properties>
      </treenode>
      <treenode text="g_ccmIp" id="632994440446577534" vid="632732214381185473">
        <Properties type="String" initWith="ccmIp">g_ccmIp</Properties>
      </treenode>
      <treenode text="g_ccmVersion" id="632994440446577536" vid="632732214381185475">
        <Properties type="String" initWith="ccmVersion">g_ccmVersion</Properties>
      </treenode>
      <treenode text="g_delayExecute" id="632994440446577538" vid="632735368746330009">
        <Properties type="Int" initWith="delayExecute">g_delayExecute</Properties>
      </treenode>
      <treenode text="g_welcomeFormatter" id="632994440446577540" vid="632831349333412969">
        <Properties type="String" initWith="welcomePrompt">g_welcomeFormatter</Properties>
      </treenode>
      <treenode text="g_pin" id="632994440446577542" vid="632831506313267057">
        <Properties type="String" initWith="globalPin">g_pin</Properties>
      </treenode>
      <treenode text="g_waitTime" id="632994440446577544" vid="632845355542565806">
        <Properties type="Int" initWith="waitTime">g_waitTime</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632732119900199508" treenode="632732119900199509" appnode="632732119900199506" handlerfor="632732119900199505">
    <node type="Start" id="632732119900199508" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="360">
      <linkto id="632732214381185481" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632732214381185477" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="240" y="360">
      <linkto id="632732214381185478" type="Labeled" style="Bezier" label="4.0" />
      <linkto id="632732214381185478" type="Labeled" style="Bezier" label="4.1" />
      <linkto id="632732214381185487" type="Labeled" style="Bezier" label="3.3" />
      <linkto id="632732214381185478" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">g_ccmVersion</ap>
      </Properties>
    </node>
    <node type="Action" id="632732214381185478" name="Login" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="440" y="448">
      <linkto id="632732214381185491" type="Labeled" style="Bezier" label="default" />
      <linkto id="632735251189695157" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="AppId" type="variable">g_proxyUsername</ap>
        <ap name="AppCertificate" type="variable">g_proxyPassword</ap>
        <ap name="UserId" type="csharp">request.CcmUser</ap>
        <ap name="DeviceName" type="csharp">request.DeviceName</ap>
        <ap name="DeviceProfile" type="csharp">request.DeviceProfile</ap>
        <ap name="Timeout" type="variable">timeout</ap>
        <ap name="NoTimeout" type="csharp">timeout == 0</ap>
        <ap name="Url" type="csharp">"http://" + g_ccmIp + "/emservice/EMServiceServlet"</ap>
        <rd field="ErrorCode">errorCode</rd>
        <rd field="ErrorMessage">errorMessage</rd>
      </Properties>
    </node>
    <node type="Action" id="632732214381185481" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="136" y="360">
      <linkto id="632732214381185477" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632732214381185485" type="Labeled" style="Bezier" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="csharp">request.ToString()</log>
public static string Execute(Metreos.Types.Reserve.ReserveRequest request, ref int timeout, Metreos.Types.Reserve.ReserveResponse response, LogWriter log, ref string appendIp, string url, ref string welcomeMsg, string g_welcomeFormatter)
{
	string responseMessage = "Success";

	if(request.DeviceName == null || request.DeviceName == String.Empty)
	{
		response.ResultCode = ((int)Messaging.ResultCodes.NoDeviceName).ToString();
		response.ResultMessage = Messaging.ResultMessages.NoDeviceName;
		responseMessage = "Failure";
		return responseMessage;
	}

	if(request.CcmUser == null || request.CcmUser == String.Empty)
	{
		response.ResultCode = ((int)Messaging.ResultCodes.NoCcmUser).ToString();
		response.ResultMessage = Messaging.ResultMessages.NoCcmUser;
		responseMessage = "Failure";
	}

	if(url != null)
	{
		int index = url.LastIndexOf('/');
		
		if(index &gt; 0 &amp;&amp; index != url.Length - 1)
		{
			appendIp = url.Substring(index + 1);
		}
	}

	if(request.Timeout == null)
	{
		timeout = 0;
	}
	else
	{
		try
		{
			timeout = Convert.ToInt32(uint.Parse(request.Timeout));
		}
		catch
		{
			timeout = 0;
			responseMessage = "Failure";
			response.ResultCode = ((int) Messaging.ResultCodes.InvalidTimeout).ToString();
			response.ResultMessage = Messaging.ResultMessages.InvalidTimeout;
		}
	}

	welcomeMsg = String.Format(g_welcomeFormatter, request.First, request.Last) ;



	return responseMessage;
}
</Properties>
    </node>
    <node type="Action" id="632732214381185482" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1406" y="248">
      <linkto id="632732214381185494" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">response.ToString()</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <log condition="entry" on="true" level="Info" type="csharp">response.ToString()</log>
      </Properties>
    </node>
    <node type="Label" id="632732214381185484" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1406" y="120">
      <linkto id="632732214381185482" type="Basic" style="Bezier" />
    </node>
    <node type="Label" id="632732214381185485" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="136" y="472" />
    <node type="Action" id="632732214381185487" name="Login" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="448" y="224">
      <linkto id="632732214381185490" type="Labeled" style="Bezier" label="default" />
      <linkto id="632735251189695157" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="AppId" type="variable">g_proxyUsername</ap>
        <ap name="AppCertificate" type="variable">g_proxyPassword</ap>
        <ap name="UserId" type="csharp">request.CcmUser</ap>
        <ap name="DeviceName" type="csharp">request.DeviceName</ap>
        <ap name="DeviceProfile" type="csharp">request.DeviceProfile</ap>
        <ap name="Timeout" type="variable">timeout</ap>
        <ap name="NoTimeout" type="csharp">timeout == 0</ap>
        <ap name="Url" type="csharp">"http://" + g_ccmIp + "/LoginService/login.asp"</ap>
        <rd field="ErrorCode">errorCode</rd>
        <rd field="ErrorMessage">errorMessage</rd>
      </Properties>
    </node>
    <node type="Label" id="632732214381185490" text="l" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="448" y="320" />
    <node type="Label" id="632732214381185491" text="l" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="440" y="552" />
    <node type="Label" id="632732214381185492" text="l" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1240" y="768">
      <linkto id="632833645172476452" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632732214381185493" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1515" y="504">
      <linkto id="632845940158030899" type="Labeled" style="Bezier" label="default" />
      <Properties language="csharp">
public static string Execute(string errorCode, string errorMessage, string loggedInUser, StringCollection loggedInDevices, Metreos.Types.Reserve.ReserveResponse response)
{
	response.ResultCode = ((int) Messaging.ResultCodes.ExtMobError).ToString();
	response.ResultMessage = Messaging.ResultMessages.ExtMobError;
	response.DiagnosticCode = errorCode;
	response.DiagnosticMessage = errorMessage;

	// Effort to stop empty loggedInUser tag--(no tag seri on null in .net)
	response.LoggedInUser = loggedInUser == String.Empty ? null : loggedInUser;

	// Effort to stop empty loggedInDevice tag--(no tag seri on null in .net)
	response.LoggedInDevice = loggedInDevices.Count == 0 ? null : loggedInDevices[0];
	
	return "Success";
}
</Properties>
    </node>
    <node type="Action" id="632732214381185494" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1534" y="247">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632734734252642281" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1263" y="361.5">
      <linkto id="632735404124501362" type="Labeled" style="Bezier" label="default" />
      <linkto id="632845940158030899" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">text.ToString()</ap>
        <ap name="URL" type="csharp">queryData.Rows[0]["IP"]</ap>
        <ap name="Username" type="csharp">request.CcmUser</ap>
        <ap name="Password" type="variable">g_pin</ap>
        <rd field="ResultData">phoneResponse</rd>
      </Properties>
    </node>
    <node type="Action" id="632735251189695155" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1041" y="360.5">
      <linkto id="632735368746330069" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Welcome</ap>
        <ap name="Text" type="variable">welcomeMsg</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632735251189695156" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="837" y="359.5">
      <linkto id="632735251189695160" type="Labeled" style="Bezier" label="default" />
      <linkto id="632735368746330008" type="Labeled" style="Bezier" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">count == 1</ap>
      </Properties>
    </node>
    <node type="Action" id="632735251189695157" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="610" y="360.5">
      <linkto id="632735251189695158" type="Labeled" style="Bezier" label="false" />
      <linkto id="632831506313267056" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">(request.First != null &amp;&amp; request.First != String.Empty) || (request.Last != null &amp;&amp; request.Last != String.Empty)</ap>
      </Properties>
    </node>
    <node type="Label" id="632735251189695158" text="d" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="611" y="486.5" />
    <node type="Label" id="632735251189695160" text="q" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="835" y="473.5" />
    <node type="Label" id="632735251189695161" text="q" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1621" y="623.5">
      <linkto id="632735251189695163" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632735251189695163" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1622" y="503.5">
      <linkto id="632845940158030899" type="Labeled" style="Bezier" label="default" />
      <Properties language="csharp">
public static string Execute(ReserveRequest request, ReserveResponse response, int count, LogWriter log)
{
	if(count == 0)
	{
		log.Write(TraceLevel.Error, "Unable to find the IP address of the phone in the CiscoDeviceListX database");
	}
	else // Count &gt; 1
	{
		log.Write(TraceLevel.Error, "Data integrity error:  Found two devices with the same CallManager IP and device name.");
	}

	// Add new error, IP Address of phone could not be found

	response.ResultCode = ((int) Messaging.ResultCodes.DeviceNotFound).ToString();
	response.ResultMessage = Messaging.ResultMessages.DeviceNotFound;

	return "";
}
</Properties>
    </node>
    <node type="Comment" id="632735251189695164" text="DLX Query failed" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1566" y="632.5" />
    <node type="Comment" id="632735251189695165" text="Login failed" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1208" y="776" />
    <node type="Action" id="632735368746330008" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="928.942139" y="360">
      <linkto id="632735251189695155" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="variable">g_delayExecute</ap>
      </Properties>
    </node>
    <node type="Action" id="632735368746330069" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1162.94214" y="360">
      <linkto id="632994440446577633" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Confirm</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Confirm/" + appendIp + "?d=" + request.DeviceName + "&amp;r=" + request.RecordId + "&amp;u=" + request.CcmUser</ap>
        <rd field="ResultData">text</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"http://" + host + "/Confirm/" + appendIp + "?r=" + request.RecordId + "&amp;u=" + request.CcmUser</log>
      </Properties>
    </node>
    <node type="Action" id="632735404124501362" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1262.94214" y="487">
      <linkto id="632735404124501363" type="Labeled" style="Bezier" label="IPPhone" />
      <linkto id="632735404124501370" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">phoneResponse.ErrorType.ToString()</ap>
      </Properties>
    </node>
    <node type="Label" id="632735404124501363" text="p" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1089.94214" y="634" />
    <node type="Label" id="632735404124501364" text="p" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1395.94214" y="621">
      <linkto id="632735404124501366" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632735404124501366" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1397" y="507">
      <linkto id="632845940158030899" type="Labeled" style="Bezier" label="default" />
      <Properties language="csharp">
public static string Execute(ReserveResponse response, Metreos.Types.CiscoIpPhone.Response phoneResponse, LogWriter log)
{
	// Phone specific error occurred

	response.ResultCode = ((int) Messaging.ResultCodes.IPPhoneXmlError).ToString();
	response.ResultMessage = Messaging.ResultMessages.IPPhoneXmlError;
	response.DiagnosticCode = phoneResponse.IPPhoneError.ToString();

	switch(phoneResponse.IPPhoneError)
	{
		case 1:
			response.DiagnosticMessage = "Error parsing CiscoIPPhoneExecute object";
		break;

		case 2:
			response.DiagnosticMessage = "Error framing CiscoIPPhoneResponse object";
		break;

		case 3:
			response.DiagnosticMessage = "Internal file error";
		break;

		case 4:
			response.DiagnosticMessage = "Authentication error";
		break;

		default:
			response.DiagnosticMessage = "Undefined";
		break;
	}
	return "";
}
</Properties>
    </node>
    <node type="Comment" id="632735404124501368" text="Phone-specific Error" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1345" y="632.5" />
    <node type="Label" id="632735404124501370" text="g" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1261.94214" y="633" />
    <node type="Label" id="632735404124501371" text="g" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1741.90625" y="622">
      <linkto id="632735404124501373" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632735404124501373" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1742" y="506.5">
      <linkto id="632845940158030899" type="Labeled" style="Bezier" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.CiscoIpPhone.Response phoneResponse, ReserveResponse response, LogWriter log)
{
	response.ResultCode = ((int) Messaging.ResultCodes.IPPhoneXmlCommError).ToString();
	response.ResultMessage = Messaging.ResultMessages.IPPhoneXmlCommError;

	switch(phoneResponse.ErrorType)
	{
		case Metreos.Types.CiscoIpPhone.Response.ErrorTypeCode.Http:

			response.DiagnosticCode = ((int) Messaging.IPPhoneXmlCommErrors.HttpFailure).ToString();
			response.DiagnosticMessage = Messaging.IPPhoneXmlCommError.HttpFailure + (int)phoneResponse.HttpCode;
			break;

		case Metreos.Types.CiscoIpPhone.Response.ErrorTypeCode.Connectivity:

			response.DiagnosticCode = ((int) Messaging.IPPhoneXmlCommErrors.Unreachable).ToString();
			response.DiagnosticMessage = Messaging.IPPhoneXmlCommError.Unreachable;
			break;

		case Metreos.Types.CiscoIpPhone.Response.ErrorTypeCode.Unknown:

			response.DiagnosticCode = ((int) Messaging.IPPhoneXmlCommErrors.Unknown).ToString();
			response.DiagnosticMessage = Messaging.IPPhoneXmlCommError.Unknown;
			break;


	}
	return "";
}
</Properties>
    </node>
    <node type="Comment" id="632735404124501375" text="Non-specific IP &#xD;&#xA;phone- error occurred" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1681.90625" y="634" />
    <node type="Comment" id="632735404124501376" text="No display name specified" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1257.1897" y="69" />
    <node type="Comment" id="632735404124501377" text="Request did not validate" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1476.1897" y="90" />
    <node type="Action" id="632831506313267056" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="724" y="359">
      <linkto id="632735251189695156" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">request.DeviceName</ap>
        <ap name="Status" type="literal">Registered</ap>
        <rd field="ResultData">queryData</rd>
        <rd field="Count">count</rd>
      </Properties>
    </node>
    <node type="Action" id="632833645172476443" name="QueryDevices" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="1456" y="768">
      <linkto id="632833645172476447" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632833645172476457" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AppId" type="variable">g_proxyUsername</ap>
        <ap name="AppCertificate" type="variable">g_proxyPassword</ap>
        <ap name="DeviceNames" type="csharp">new string[] { request.DeviceName }</ap>
        <ap name="Url" type="csharp">g_ccmVersion == "3.3" ? "http://" + g_ccmIp + "/LoginService/login.asp" : "http://" + g_ccmIp + "/emservice/EMServiceServlet"</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIp</ap>
        <rd field="QueryDevicesResult">queryDevicesRes</rd>
        <rd field="ErrorMessage">secondaryError</rd>
      </Properties>
    </node>
    <node type="Action" id="632833645172476447" name="GetDeviceStatus" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="1568" y="768">
      <linkto id="632833645172476461" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DeviceName" type="csharp">request.DeviceName</ap>
        <ap name="QueryDeviceResults" type="variable">queryDevicesRes</ap>
        <rd field="Username">loggedInUser</rd>
      </Properties>
    </node>
    <node type="Comment" id="632833645172476451" text="On condition 18, find Logged In User" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1352" y="728" />
    <node type="Action" id="632833645172476452" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1336" y="768">
      <linkto id="632833645172476443" type="Labeled" style="Bezier" label="18" />
      <linkto id="632833645172476455" type="Labeled" style="Bezier" label="25" />
      <linkto id="632732214381185493" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">errorCode == null ? "" : errorCode</ap>
      </Properties>
    </node>
    <node type="Comment" id="632833645172476453" text="On condition 25, find Logged In Device" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1384" y="808" />
    <node type="Action" id="632833645172476455" name="QueryUsers" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="1448" y="920">
      <linkto id="632833645172476456" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632833645172476458" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AppId" type="variable">g_proxyUsername</ap>
        <ap name="AppCertificate" type="variable">g_proxyPassword</ap>
        <ap name="Users" type="csharp">new string[] { request.CcmUser }</ap>
        <ap name="Url" type="csharp">g_ccmVersion == "3.3" ? "http://" + g_ccmIp + "/LoginService/login.asp" : "http://" + g_ccmIp + "/emservice/EMServiceServlet"</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIp</ap>
        <rd field="QueryUsersResult">queryUsersRes</rd>
        <rd field="ErrorMessage">secondaryError</rd>
      </Properties>
    </node>
    <node type="Action" id="632833645172476456" name="GetUserDevices" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="1568" y="920">
      <linkto id="632833645172476462" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Username" type="csharp">request.CcmUser</ap>
        <ap name="QueryUserResults" type="variable">queryUsersRes</ap>
        <rd field="Devices">loggedInDevices</rd>
      </Properties>
    </node>
    <node type="Label" id="632833645172476457" text="z" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1512" y="880" />
    <node type="Label" id="632833645172476458" text="z" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1504" y="1032" />
    <node type="Label" id="632833645172476460" text="z" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1472" y="582">
      <linkto id="632732214381185493" type="Basic" style="Bezier" />
    </node>
    <node type="Label" id="632833645172476461" text="b" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1664" y="768" />
    <node type="Label" id="632833645172476462" text="b" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1672" y="920" />
    <node type="Label" id="632833645172476463" text="b" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1552" y="582">
      <linkto id="632732214381185493" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632845355542565683" name="CheckOut" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="1719.99561" y="361">
      <linkto id="632845355542565685" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632845355542565928" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="WaitTime" type="variable">g_waitTime</ap>
        <rd field="Username">triUser</rd>
        <rd field="Password">triPass</rd>
      </Properties>
    </node>
    <node type="Action" id="632845355542565685" name="SignOn" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="1831.99561" y="361">
      <linkto id="632845355542565686" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632845355542566069" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Host" type="variable">g_triHost</ap>
        <ap name="Port" type="variable">g_triPort</ap>
        <ap name="Secure" type="variable">g_triSecure</ap>
        <ap name="UserID" type="variable">triUser</ap>
        <ap name="Password" type="variable">triPass</ap>
        <rd field="CompanyId">companyId</rd>
        <rd field="SignOnUserId">signOnUserId</rd>
        <rd field="SecurityToken">securityToken</rd>
      </Properties>
    </node>
    <node type="Action" id="632845355542565686" name="ReserveResponse" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="1935.99561" y="361">
      <linkto id="632845355542565687" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Host" type="variable">g_triHost</ap>
        <ap name="Port" type="variable">g_triPort</ap>
        <ap name="Secure" type="variable">g_triSecure</ap>
        <ap name="SecurityToken" type="variable">securityToken</ap>
        <ap name="SignOnUserId" type="variable">signOnUserId</ap>
        <ap name="CompanyId" type="variable">companyId</ap>
        <ap name="RecordId" type="csharp">request.RecordId</ap>
        <ap name="ResultCode" type="csharp">response.ResultCode</ap>
        <ap name="ResultMessage" type="csharp">response.ResultMessage</ap>
        <ap name="DiagnosticCode" type="csharp">response.DiagnosticCode</ap>
        <ap name="DiagnosticMessage" type="csharp">response.DiagnosticCode</ap>
        <ap name="LoggedInUser" type="csharp">response.LoggedInUser</ap>
        <ap name="LoggedInDevice" type="csharp">response.LoggedInDevice</ap>
      </Properties>
    </node>
    <node type="Action" id="632845355542565687" name="SignOut" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="2055.99561" y="361">
      <linkto id="632845355542565688" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Host" type="variable">g_triHost</ap>
        <ap name="Port" type="variable">g_triPort</ap>
        <ap name="Secure" type="variable">g_triSecure</ap>
        <ap name="SecurityToken" type="variable">securityToken</ap>
        <ap name="SignOnUserId" type="variable">signOnUserId</ap>
      </Properties>
    </node>
    <node type="Action" id="632845355542565688" name="CheckIn" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="2152" y="361">
      <linkto id="632845355542565689" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Username" type="variable">triUser</ap>
      </Properties>
    </node>
    <node type="Action" id="632845355542565689" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2272" y="361">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632845355542565928" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2031.99561" y="497">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="csharp">"Unable to acquire a Tririga User from the CheckOut command.  The BO Record ID: " + request.RecordId</log>
      </Properties>
    </node>
    <node type="Action" id="632845355542566068" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2176" y="449">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="csharp">String.Format("Unable to Sign On to Tririga using {0} with the SignOn command.  The BO Record ID: {1}", triUser, request.RecordId)</log>
      </Properties>
    </node>
    <node type="Action" id="632845355542566069" name="CheckIn" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="2063.99561" y="449">
      <linkto id="632845355542566068" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Username" type="variable">triUser</ap>
      </Properties>
    </node>
    <node type="Label" id="632845355542566071" text="d" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1478" y="130">
      <linkto id="632732214381185482" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632845940158030899" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1408" y="360">
      <linkto id="632732214381185482" type="Labeled" style="Bezier" label="false" />
      <linkto id="632846967484998515" type="Labeled" style="Bezier" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">false</ap>
      </Properties>
    </node>
    <node type="Action" id="632846967484998515" name="PopulateUsers" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="1577.27612" y="361">
      <linkto id="632845355542565683" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Username1" type="csharp">g_triUser1 == "NONE" ? null : g_triUser1</ap>
        <ap name="Password1" type="csharp">g_triPass1 == "NONE" ? null : g_triPass1</ap>
        <ap name="Username2" type="csharp">g_triUser2 == "NONE" ? null : g_triUser2</ap>
        <ap name="Password2" type="csharp">g_triPass2 == "NONE" ? null : g_triPass2</ap>
        <ap name="Username3" type="csharp">g_triUser3 == "NONE" ? null : g_triUser3</ap>
        <ap name="Password3" type="csharp">g_triPass3 == "NONE" ? null : g_triPass3</ap>
        <ap name="Username4" type="csharp">g_triUser4 == "NONE" ? null : g_triUser4</ap>
        <ap name="Password4" type="csharp">g_triPass4 == "NONE" ? null : g_triPass4</ap>
        <ap name="Username5" type="csharp">g_triUser5 == "NONE" ? null : g_triUser5</ap>
        <ap name="Password5" type="csharp">g_triPass5 == "NONE" ? null : g_triPass5</ap>
      </Properties>
    </node>
    <node type="Action" id="632994440446577633" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1225" y="171">
      <linkto id="632734734252642281" type="Labeled" style="Vector" label="success" />
      <linkto id="632994440446577634" type="Labeled" style="Vector" label="recreate" />
      <Properties language="csharp"> 
public static string Execute(ref string welcomeMsg, Metreos.Types.CiscoIpPhone.Text text, string g_welcomeFormatter, Metreos.Types.Reserve.ReserveRequest request, LogWriter log)
{
	// IP Phones can't be pushed more than 512 bytes 

      char[] specialChars = 
      {
            '&lt;', '&gt;', '/', ' ', '~', '&amp;', '?', '=', ';', '\r', '\n', '"'
      };

	System.Text.StringBuilder encodedText = new System.Text.StringBuilder();

	string unEncodedText = text.ToString();
      CharEnumerator e = unEncodedText.GetEnumerator();

      bool special;

      // Iterate through the string.
      while(e.MoveNext())
      {
          special = false;

          // Iterate through the special characters.
          for(int i = 0; i &lt; specialChars.Length; i++)
          {
              // Is the current character special?
              if(e.Current == specialChars[i])
              {
                  // Yes, convert it to hex.
                  encodedText.Append(Uri.HexEscape(e.Current));
                  special = true;
                  break; 
               }
          }

              // The character was not special, so just copy it over.
              if(special == false)
              {
                  encodedText.Append(e.Current);
              }
      }

      string encoded = encodedText.ToString();

	int length = ("XML=" + encoded).Length;

	welcomeMsg = String.Format(g_welcomeFormatter, request.First, request.Last) ;

	if(length &gt; 511)
	{
		int overLimit = length - 511;

		if(welcomeMsg.Length &gt; overLimit)
		{
			welcomeMsg = welcomeMsg.Remove(welcomeMsg.Length - overLimit, overLimit);

			log.Write(TraceLevel.Warning, "Truncating Welcome Message={0}", welcomeMsg);

			return "recreate";
		}
	}

	return "success";

}
</Properties>
    </node>
    <node type="Action" id="632994440446577634" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1335" y="169.5">
      <linkto id="632734734252642281" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Welcome</ap>
        <ap name="Text" type="variable">welcomeMsg</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Variable" id="632732214381185454" name="request" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Reserve.ReserveRequest" initWith="body" refType="reference" name="Metreos.Providers.Http.GotRequest">request</Properties>
    </node>
    <node type="Variable" id="632732214381185455" name="errorCode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">errorCode</Properties>
    </node>
    <node type="Variable" id="632732214381185456" name="errorMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">errorMessage</Properties>
    </node>
    <node type="Variable" id="632732214381185480" name="timeout" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">timeout</Properties>
    </node>
    <node type="Variable" id="632732214381185483" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Reserve.ReserveResponse" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632732214381185486" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632732995304981860" name="placeHolder" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoExtensionMobility.QueryDeviceResults" refType="reference">placeHolder</Properties>
    </node>
    <node type="Variable" id="632735251189695153" name="count" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">count</Properties>
    </node>
    <node type="Variable" id="632735251189695154" name="queryData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">queryData</Properties>
    </node>
    <node type="Variable" id="632735251189695166" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632735251189695167" name="phoneResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Response" refType="reference">phoneResponse</Properties>
    </node>
    <node type="Variable" id="632735368746330070" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632831506313266858" name="welcomeMsg" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">welcomeMsg</Properties>
    </node>
    <node type="Variable" id="632833645172476444" name="secondaryError" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">secondaryError</Properties>
    </node>
    <node type="Variable" id="632833645172476445" name="queryDevicesRes" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoExtensionMobility.QueryDeviceResults" refType="reference">queryDevicesRes</Properties>
    </node>
    <node type="Variable" id="632833645172476446" name="queryUsersRes" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoExtensionMobility.QueryUserResults" refType="reference">queryUsersRes</Properties>
    </node>
    <node type="Variable" id="632845355542565926" name="triUser" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">triUser</Properties>
    </node>
    <node type="Variable" id="632845355542565927" name="triPass" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">triPass</Properties>
    </node>
    <node type="Variable" id="632845355542565929" name="securityToken" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">securityToken</Properties>
    </node>
    <node type="Variable" id="632845355542565930" name="signOnUserId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">signOnUserId</Properties>
    </node>
    <node type="Variable" id="632845355542565931" name="companyId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Long" refType="reference">companyId</Properties>
    </node>
    <node type="Variable" id="632845940158031131" name="loggedInUser" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">loggedInUser</Properties>
    </node>
    <node type="Variable" id="632845940158031132" name="loggedInDevices" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="StringCollection" refType="reference">loggedInDevices</Properties>
    </node>
    <node type="Variable" id="632846669383434381" name="appendIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">appendIp</Properties>
    </node>
    <node type="Variable" id="632846669383434382" name="url" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="url" refType="reference" name="Metreos.Providers.Http.GotRequest">url</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>