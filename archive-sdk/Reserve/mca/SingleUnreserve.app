<Application name="SingleUnreserve" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="SingleUnreserve">
    <outline>
      <treenode type="evh" id="632732119900199516" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632732119900199513" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632732119900199512" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Release</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_triHost" id="632858995898902651" vid="632845355542565934">
        <Properties type="String" initWith="triHost">g_triHost</Properties>
      </treenode>
      <treenode text="g_triPort" id="632858995898902653" vid="632845355542565936">
        <Properties type="String" initWith="triPort">g_triPort</Properties>
      </treenode>
      <treenode text="g_triUser1" id="632858995898902655" vid="632845355542565663">
        <Properties type="String" defaultInitWith="NONE" initWith="triUser1">g_triUser1</Properties>
      </treenode>
      <treenode text="g_triPass1" id="632858995898902657" vid="632845355542565665">
        <Properties type="String" defaultInitWith="NONE" initWith="triPass1">g_triPass1</Properties>
      </treenode>
      <treenode text="g_triUser2" id="632858995898902659" vid="632845355542565667">
        <Properties type="String" defaultInitWith="NONE" initWith="triUser2">g_triUser2</Properties>
      </treenode>
      <treenode text="g_triPass2" id="632858995898902661" vid="632845355542565669">
        <Properties type="String" defaultInitWith="NONE" initWith="triPass2">g_triPass2</Properties>
      </treenode>
      <treenode text="g_triUser3" id="632858995898902663" vid="632845355542565671">
        <Properties type="String" defaultInitWith="NONE" initWith="triUser3">g_triUser3</Properties>
      </treenode>
      <treenode text="g_triPass3" id="632858995898902665" vid="632845355542565673">
        <Properties type="String" defaultInitWith="NONE" initWith="triPass3">g_triPass3</Properties>
      </treenode>
      <treenode text="g_triUser4" id="632858995898902667" vid="632845355542565675">
        <Properties type="String" defaultInitWith="NONE" initWith="triUser4">g_triUser4</Properties>
      </treenode>
      <treenode text="g_triPass4" id="632858995898902669" vid="632845355542565677">
        <Properties type="String" defaultInitWith="NONE" initWith="triPass4">g_triPass4</Properties>
      </treenode>
      <treenode text="g_triUser5" id="632858995898902671" vid="632845355542565679">
        <Properties type="String" defaultInitWith="NONE" initWith="triUser5">g_triUser5</Properties>
      </treenode>
      <treenode text="g_triPass5" id="632858995898902673" vid="632845355542565681">
        <Properties type="String" defaultInitWith="NONE" initWith="triPass5">g_triPass5</Properties>
      </treenode>
      <treenode text="g_triSecure" id="632858995898902675" vid="632845355542565932">
        <Properties type="Bool" initWith="triSecure">g_triSecure</Properties>
      </treenode>
      <treenode text="g_proxyUsername" id="632858995898902677" vid="632732214381185586">
        <Properties type="String" initWith="proxyUser">g_proxyUsername</Properties>
      </treenode>
      <treenode text="g_proxyPassword" id="632858995898902679" vid="632732214381185588">
        <Properties type="String" initWith="proxyPass">g_proxyPassword</Properties>
      </treenode>
      <treenode text="g_ccmIp" id="632858995898902681" vid="632732214381185590">
        <Properties type="String" initWith="ccmIp">g_ccmIp</Properties>
      </treenode>
      <treenode text="g_ccmVersion" id="632858995898902683" vid="632732214381185592">
        <Properties type="String" initWith="ccmVersion">g_ccmVersion</Properties>
      </treenode>
      <treenode text="g_dbUser" id="632858995898902685" vid="632831622223137258">
        <Properties type="String" initWith="dbUser">g_dbUser</Properties>
      </treenode>
      <treenode text="g_dbHost" id="632858995898902687" vid="632831622223137260">
        <Properties type="String" initWith="dbHost">g_dbHost</Properties>
      </treenode>
      <treenode text="g_dbPass" id="632858995898902689" vid="632831622223137262">
        <Properties type="String" initWith="dbPass">g_dbPass</Properties>
      </treenode>
      <treenode text="g_dbPort" id="632858995898902691" vid="632831622223137264">
        <Properties type="Int" initWith="dbPort">g_dbPort</Properties>
      </treenode>
      <treenode text="g_dbType" id="632858995898902693" vid="632831622223137266">
        <Properties type="String" initWith="dbType">g_dbType</Properties>
      </treenode>
      <treenode text="g_dbName" id="632858995898902695" vid="632831622223137268">
        <Properties type="String" initWith="dbName">g_dbName</Properties>
      </treenode>
      <treenode text="g_waitTime" id="632858995898902697" vid="632845355542565806">
        <Properties type="Int" initWith="waitTime">g_waitTime</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632732119900199515" treenode="632732119900199516" appnode="632732119900199513" handlerfor="632732119900199512">
    <node type="Start" id="632732119900199515" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="384">
      <linkto id="632732214381185504" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632732214381185502" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="600" y="384">
      <linkto id="632732214381185585" type="Labeled" style="Bezier" label="4.1" />
      <linkto id="632732214381185585" type="Labeled" style="Bezier" label="4.0" />
      <linkto id="632732214381185585" type="Labeled" style="Bezier" label="default" />
      <linkto id="632732214381185594" type="Labeled" style="Bezier" ortho="true" label="3.3" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">g_ccmVersion</ap>
      </Properties>
    </node>
    <node type="Action" id="632732214381185504" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="152" y="384">
      <linkto id="632732214381185507" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632831622223137254" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="csharp">request.ToString()</log>
public static string Execute(Metreos.Types.Reserve.ReleaseRequest request, Metreos.Types.Reserve.ReleaseResponse response)
{
	string responseMessage = "Success";

	if(request.DeviceName == null || request.DeviceName == String.Empty)
	{
		response.ResultCode = ((int)Messaging.ResultCodes.NoDeviceName).ToString();
		response.ResultMessage = Messaging.ResultMessages.NoDeviceName;
		responseMessage = "Failure";
	}

	return responseMessage;
}
</Properties>
    </node>
    <node type="Action" id="632732214381185505" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1031" y="164">
      <linkto id="632732214381185513" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">response.ToString()</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <log condition="entry" on="true" level="Info" type="csharp">response.ToString()</log>
      </Properties>
    </node>
    <node type="Label" id="632732214381185506" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1031" y="36">
      <linkto id="632732214381185505" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632732214381185507" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="152" y="496" />
    <node type="Label" id="632732214381185509" text="f" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="808" y="344" />
    <node type="Label" id="632732214381185510" text="f" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="800" y="592" />
    <node type="Label" id="632732214381185511" text="f" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1040" y="640">
      <linkto id="632732214381185512" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632732214381185512" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1040" y="519">
      <linkto id="632845940158030995" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string errorCode, string errorMessage, Metreos.Types.Reserve.ReleaseResponse response)
{
	response.ResultCode = ((int) Messaging.ResultCodes.ExtMobError).ToString();
	response.ResultMessage = Messaging.ResultMessages.ExtMobError;
	response.DiagnosticCode = errorCode;
	response.DiagnosticMessage = errorMessage;
	return "Success";
}
</Properties>
    </node>
    <node type="Action" id="632732214381185513" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1159" y="164">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632732214381185585" name="Logout" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="800" y="488">
      <linkto id="632732214381185510" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632845940158030995" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="AppId" type="variable">g_proxyUsername</ap>
        <ap name="AppCertificate" type="variable">g_proxyPassword</ap>
        <ap name="DeviceName" type="csharp">request.DeviceName</ap>
        <ap name="Url" type="csharp">"http://" + g_ccmIp + "/emservice/EMServiceServlet"</ap>
        <rd field="ErrorCode">errorCode</rd>
        <rd field="ErrorMessage">errorMessage</rd>
      </Properties>
    </node>
    <node type="Action" id="632732214381185594" name="Logout" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="808" y="248">
      <linkto id="632732214381185509" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632845940158030995" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="AppId" type="variable">g_proxyUsername</ap>
        <ap name="AppCertificate" type="variable">g_proxyPassword</ap>
        <ap name="DeviceName" type="csharp">request.DeviceName</ap>
        <ap name="Url" type="csharp">"http://" + g_ccmIp + "/LoginService/login.asp"</ap>
        <rd field="ErrorCode">errorCode</rd>
        <rd field="ErrorMessage">errorMessage</rd>
      </Properties>
    </node>
    <node type="Action" id="632831622223137123" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="496" y="384">
      <linkto id="632732214381185502" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">"INSERT IGNORE INTO SkipDisplayDevices (Devicename) VALUES ('" + request.DeviceName + "')"</ap>
        <ap name="Name" type="literal">Reserve</ap>
      </Properties>
    </node>
    <node type="Action" id="632831622223137254" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="256" y="384">
      <linkto id="632831622223137255" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="variable">g_dbType</ap>
        <ap name="DatabaseName" type="variable">g_dbName</ap>
        <ap name="Server" type="variable">g_dbHost</ap>
        <ap name="Port" type="variable">g_dbPort</ap>
        <ap name="Username" type="variable">g_dbUser</ap>
        <ap name="Password" type="variable">g_dbPass</ap>
        <ap name="Pooling" type="literal">true</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632831622223137255" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="376" y="384">
      <linkto id="632831622223137123" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">Reserve</ap>
        <ap name="Type" type="variable">g_dbType</ap>
      </Properties>
    </node>
    <node type="Action" id="632845388240483290" name="CheckOut" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="1171.70483" y="391">
      <linkto id="632845388240483291" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632845388240483298" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="WaitTime" type="variable">g_waitTime</ap>
        <rd field="Username">triUser</rd>
        <rd field="Password">triPass</rd>
      </Properties>
    </node>
    <node type="Action" id="632845388240483291" name="SignOn" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="1283.70532" y="391">
      <linkto id="632845388240483297" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632845388240483293" type="Labeled" style="Bezier" ortho="true" label="Success" />
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
    <node type="Action" id="632845388240483293" name="SignOut" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="1507.7085" y="391">
      <linkto id="632845388240483294" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Host" type="variable">g_triHost</ap>
        <ap name="Port" type="variable">g_triPort</ap>
        <ap name="Secure" type="variable">g_triSecure</ap>
        <ap name="SecurityToken" type="variable">securityToken</ap>
        <ap name="SignOnUserId" type="variable">signOnUserId</ap>
      </Properties>
    </node>
    <node type="Action" id="632845388240483294" name="CheckIn" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="1603.70825" y="391">
      <linkto id="632845388240483295" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Username" type="variable">triUser</ap>
      </Properties>
    </node>
    <node type="Action" id="632845388240483295" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1723.7085" y="391">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632845388240483296" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1627.7085" y="479">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="csharp">String.Format("Unable to Sign On to Tririga using {0} with the SignOn command.  The BO Record ID: {1}", triUser, request.RecordId)</log>
      </Properties>
    </node>
    <node type="Action" id="632845388240483297" name="CheckIn" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="1515.70825" y="479">
      <linkto id="632845388240483296" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Username" type="variable">triUser</ap>
      </Properties>
    </node>
    <node type="Action" id="632845388240483298" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1483.7085" y="527">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="csharp">"Unable to acquire a Tririga User from the CheckOut command.  The BO Record ID: " + request.RecordId</log>
      </Properties>
    </node>
    <node type="Action" id="632845940158030995" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="971" y="390">
      <linkto id="632732214381185505" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632846967484998611" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">false</ap>
      </Properties>
    </node>
    <node type="Action" id="632846967484998611" name="PopulateUsers" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="1069.27612" y="390">
      <linkto id="632845388240483290" type="Labeled" style="Bezier" ortho="true" label="default" />
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
    <node type="Variable" id="632732214381185526" name="request" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Reserve.ReleaseRequest" initWith="body" refType="reference" name="Metreos.Providers.Http.GotRequest">request</Properties>
    </node>
    <node type="Variable" id="632732214381185580" name="errorCode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="Unknown" refType="reference">errorCode</Properties>
    </node>
    <node type="Variable" id="632732214381185581" name="errorMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="Unknown" refType="reference">errorMessage</Properties>
    </node>
    <node type="Variable" id="632732214381185582" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Reserve.ReleaseResponse" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632732214381185583" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632732995304981893" name="placeHolder" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoExtensionMobility.QueryUserResults" refType="reference">placeHolder</Properties>
    </node>
    <node type="Variable" id="632831622223137270" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632845388240483310" name="triUser" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">triUser</Properties>
    </node>
    <node type="Variable" id="632845388240483311" name="triPass" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">triPass</Properties>
    </node>
    <node type="Variable" id="632845388240483312" name="companyId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">companyId</Properties>
    </node>
    <node type="Variable" id="632845388240483313" name="securityToken" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">securityToken</Properties>
    </node>
    <node type="Variable" id="632845388240483314" name="signOnUserId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">signOnUserId</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>