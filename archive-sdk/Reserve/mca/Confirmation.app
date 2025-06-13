<Application name="Confirmation" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Confirmation">
    <outline>
      <treenode type="evh" id="632735368746330077" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632735368746330074" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632735368746330073" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Confirm</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_triHost" id="632858995898902330" vid="632845355542565934">
        <Properties type="String" initWith="triHost">g_triHost</Properties>
      </treenode>
      <treenode text="g_triPort" id="632858995898902332" vid="632845355542565936">
        <Properties type="String" initWith="triPort">g_triPort</Properties>
      </treenode>
      <treenode text="g_triUser1" id="632858995898902334" vid="632845355542565663">
        <Properties type="String" defaultInitWith="NONE" initWith="triUser1">g_triUser1</Properties>
      </treenode>
      <treenode text="g_triPass1" id="632858995898902336" vid="632845355542565665">
        <Properties type="String" defaultInitWith="NONE" initWith="triPass1">g_triPass1</Properties>
      </treenode>
      <treenode text="g_triUser2" id="632858995898902338" vid="632845355542565667">
        <Properties type="String" defaultInitWith="NONE" initWith="triUser2">g_triUser2</Properties>
      </treenode>
      <treenode text="g_triPass2" id="632858995898902340" vid="632845355542565669">
        <Properties type="String" defaultInitWith="NONE" initWith="triPass2">g_triPass2</Properties>
      </treenode>
      <treenode text="g_triUser3" id="632858995898902342" vid="632845355542565671">
        <Properties type="String" defaultInitWith="NONE" initWith="triUser3">g_triUser3</Properties>
      </treenode>
      <treenode text="g_triPass3" id="632858995898902344" vid="632845355542565673">
        <Properties type="String" defaultInitWith="NONE" initWith="triPass3">g_triPass3</Properties>
      </treenode>
      <treenode text="g_triUser4" id="632858995898902346" vid="632845355542565675">
        <Properties type="String" defaultInitWith="NONE" initWith="triUser4">g_triUser4</Properties>
      </treenode>
      <treenode text="g_triPass4" id="632858995898902348" vid="632845355542565677">
        <Properties type="String" defaultInitWith="NONE" initWith="triPass4">g_triPass4</Properties>
      </treenode>
      <treenode text="g_triUser5" id="632858995898902350" vid="632845355542565679">
        <Properties type="String" defaultInitWith="NONE" initWith="triUser5">g_triUser5</Properties>
      </treenode>
      <treenode text="g_triPass5" id="632858995898902352" vid="632845355542565681">
        <Properties type="String" defaultInitWith="NONE" initWith="triPass5">g_triPass5</Properties>
      </treenode>
      <treenode text="g_triSecure" id="632858995898902354" vid="632845355542565932">
        <Properties type="Bool" initWith="triSecure">g_triSecure</Properties>
      </treenode>
      <treenode text="g_waitTime" id="632858995898902356" vid="632845355542565806">
        <Properties type="Int" initWith="waitTime">g_waitTime</Properties>
      </treenode>
      <treenode text="g_dbHost" id="632858995898902358" vid="632831622223137291">
        <Properties type="String" initWith="dbHost">g_dbHost</Properties>
      </treenode>
      <treenode text="g_dbPort" id="632858995898902360" vid="632831622223137293">
        <Properties type="String" initWith="dbPort">g_dbPort</Properties>
      </treenode>
      <treenode text="g_dbType" id="632858995898902362" vid="632831622223137295">
        <Properties type="String" initWith="dbType">g_dbType</Properties>
      </treenode>
      <treenode text="g_dbUser" id="632858995898902364" vid="632831622223137297">
        <Properties type="String" initWith="dbUser">g_dbUser</Properties>
      </treenode>
      <treenode text="g_dbPass" id="632858995898902366" vid="632831622223137299">
        <Properties type="String" initWith="dbPass">g_dbPass</Properties>
      </treenode>
      <treenode text="g_dbName" id="632858995898902368" vid="632831622223137301">
        <Properties type="String" initWith="dbName">g_dbName</Properties>
      </treenode>
      <treenode text="g_boId" id="632858995898902370" vid="632852893472969257">
        <Properties type="String" initWith="BoId">g_boId</Properties>
      </treenode>
      <treenode text="g_boName" id="632858995898902372" vid="632852893472969259">
        <Properties type="String" initWith="BoName">g_boName</Properties>
      </treenode>
      <treenode text="g_moduleId" id="632858995898902374" vid="632852893472969261">
        <Properties type="String" initWith="ModuleId">g_moduleId</Properties>
      </treenode>
      <treenode text="g_newRecordId" id="632858995898902376" vid="632852893472969263">
        <Properties type="String" initWith="NewRecordId">g_newRecordId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632735368746330076" treenode="632735368746330077" appnode="632735368746330074" handlerfor="632735368746330073">
    <node type="Start" id="632735368746330076" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="368">
      <linkto id="632845979175498819" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632735368746330079" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1299" y="365">
      <linkto id="632858995898902408" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Confirmed</ap>
        <ap name="Text" type="literal">You have been confirmed by the E&amp;Y Reservation system.</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632735368746330080" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1510" y="365">
      <linkto id="632735368746330082" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632735368746330082" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1629" y="364">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632831622223137283" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="1170" y="365">
      <linkto id="632735368746330079" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">"INSERT IGNORE INTO SkipDisplayDevices (Devicename) VALUES ('" + query["d"] + "')"</ap>
        <ap name="Name" type="literal">Reserve</ap>
      </Properties>
    </node>
    <node type="Action" id="632831622223137284" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="938" y="365">
      <linkto id="632831622223137285" type="Labeled" style="Bezier" ortho="true" label="Success" />
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
    <node type="Action" id="632831622223137285" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="1058" y="365">
      <linkto id="632831622223137283" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">Reserve</ap>
        <ap name="Type" type="variable">g_dbType</ap>
      </Properties>
    </node>
    <node type="Action" id="632845979175498810" name="CheckOut" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="399" y="365">
      <linkto id="632845979175499031" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632845979175498811" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="WaitTime" type="variable">g_waitTime</ap>
        <rd field="Username">triUser</rd>
        <rd field="Password">triPass</rd>
      </Properties>
    </node>
    <node type="Action" id="632845979175498811" name="SignOn" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="519" y="369">
      <linkto id="632852893472969256" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632845979175499025" type="Labeled" style="Bezier" ortho="true" label="default" />
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
    <node type="Action" id="632845979175498812" name="SignOut" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="722" y="365">
      <linkto id="632845979175498813" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Host" type="variable">g_triHost</ap>
        <ap name="Port" type="variable">g_triPort</ap>
        <ap name="Secure" type="variable">g_triSecure</ap>
        <ap name="SecurityToken" type="variable">securityToken</ap>
        <ap name="SignOnUserId" type="variable">signOnUserId</ap>
      </Properties>
    </node>
    <node type="Action" id="632845979175498813" name="CheckIn" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="826" y="365">
      <linkto id="632831622223137284" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Username" type="variable">triUser</ap>
      </Properties>
    </node>
    <node type="Action" id="632845979175498819" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="126" y="368">
      <linkto id="632845979175498820" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632846669383434151" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Http.QueryParamCollection query, LogWriter log)
{
	// check for username 
	if(query["u"] == null || query["u"] == String.Empty)
	{
		// oh no.  Who is this!
		log.Write(TraceLevel.Error, "Unable to extract parameter user from IP Phone confirmation message!");
		return "nouser";
	}

	// check for recordId
	if(query["r"] == null || query["r"] == String.Empty)
	{
		// oh no.  Who is this!
		log.Write(TraceLevel.Error, "Unable to extract parameter recordId from IP Phone confirmation message!");
		return "norecordid";
	}


	return "success";
}
</Properties>
    </node>
    <node type="Action" id="632845979175498820" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="127" y="482">
      <linkto id="632845979175499034" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Error</ap>
        <ap name="Text" type="literal">A difficulty was encountered when confirming your registration.</ap>
        <rd field="ResultData">text</rd>
        <log condition="entry" on="true" level="Error" type="csharp">"Unable to process request from remote host: " + remoteHost</log>
      </Properties>
    </node>
    <node type="Action" id="632845979175499025" name="CheckIn" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="519" y="477">
      <linkto id="632845979175499037" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Username" type="variable">triUser</ap>
      </Properties>
    </node>
    <node type="Action" id="632845979175499031" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="399" y="477">
      <linkto id="632845979175499033" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Error</ap>
        <ap name="Text" type="literal">A difficulty was encountered when confirming your registration.</ap>
        <rd field="ResultData">text</rd>
        <log condition="entry" on="true" level="Error" type="literal">Unable to acquire a Tririga User from the CheckOut command in the Confirmation Script</log>
      </Properties>
    </node>
    <node type="Label" id="632845979175499033" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="399" y="597" />
    <node type="Label" id="632845979175499034" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="128" y="600" />
    <node type="Label" id="632845979175499035" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1510" y="231">
      <linkto id="632858995898902409" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632845979175499036" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="519" y="685" />
    <node type="Action" id="632845979175499037" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="519" y="581">
      <linkto id="632845979175499036" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Error</ap>
        <ap name="Text" type="literal">A difficulty was encountered when confirming your registration.</ap>
        <rd field="ResultData">text</rd>
        <log condition="entry" on="true" level="Error" type="csharp">String.Format("Unable to Sign On to Tririga using {0} with the SignOn command", triUser)</log>
      </Properties>
    </node>
    <node type="Action" id="632846669383434151" name="PopulateUsers" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="273" y="367">
      <linkto id="632845979175498810" type="Labeled" style="Bezier" ortho="true" label="default" />
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
    <node type="Action" id="632852893472969256" name="ConfirmResponse" class="MaxActionNode" group="" path="Metreos.Native.Reserve" x="625" y="368">
      <linkto id="632845979175498812" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Host" type="variable">g_triHost</ap>
        <ap name="Port" type="variable">g_triPort</ap>
        <ap name="Secure" type="variable">g_triSecure</ap>
        <ap name="SignOnUserId" type="variable">signOnUserId</ap>
        <ap name="SecurityToken" type="variable">securityToken</ap>
        <ap name="ModuleId" type="variable">g_moduleId</ap>
        <ap name="CompanyId" type="variable">companyId</ap>
        <ap name="NewRecordId" type="variable">g_newRecordId</ap>
        <ap name="RecordId" type="csharp">query["r"]</ap>
        <ap name="BoId" type="variable">g_boId</ap>
        <ap name="BoName" type="variable">g_boName</ap>
      </Properties>
    </node>
    <node type="Action" id="632858995898902408" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1409" y="365">
      <linkto id="632735368746330080" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632858995898902409" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1510" y="291">
      <linkto id="632735368746330080" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Variable" id="632735368746330078" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632735368746330081" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632831622223137289" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632831622223137290" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632845979175499039" name="triUser" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">triUser</Properties>
    </node>
    <node type="Variable" id="632845979175499040" name="triPass" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">triPass</Properties>
    </node>
    <node type="Variable" id="632845979175499041" name="companyId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Long" refType="reference">companyId</Properties>
    </node>
    <node type="Variable" id="632845979175499042" name="signOnUserId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Long" refType="reference">signOnUserId</Properties>
    </node>
    <node type="Variable" id="632845979175499043" name="securityToken" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">securityToken</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>