<Application name="EntryPoint" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="EntryPoint">
    <outline>
      <treenode type="evh" id="632525516593368630" level="1" text="Metreos.Providers.Http.GotRequest (trigger): InitialRequest">
        <node type="function" name="InitialRequest" id="632525516593368627" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632525516593368626" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/AppSuiteDirectory</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632525573169070556" level="2" text="Metreos.Providers.Http.GotRequest: OnGotRequest">
        <node type="function" name="OnGotRequest" id="632525573169070553" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632525573169070552" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/AppSuiteDirectory/Listing</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632525573169070580" level="2" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="632525573169070577" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="632525573169070576" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="db_DBName" id="632525611933748030" vid="632525516593368700">
        <Properties type="String" initWith="DbName">db_DBName</Properties>
      </treenode>
      <treenode text="db_DBConnectionName" id="632525611933748032" vid="632525516593368702">
        <Properties type="String" initWith="DbConnectionName">db_DBConnectionName</Properties>
      </treenode>
      <treenode text="db_DBServer" id="632525611933748034" vid="632525516593368704">
        <Properties type="String" initWith="DbServer">db_DBServer</Properties>
      </treenode>
      <treenode text="db_DBPort" id="632525611933748036" vid="632525516593368706">
        <Properties type="UInt" initWith="DbPort">db_DBPort</Properties>
      </treenode>
      <treenode text="db_DBUsername" id="632525611933748038" vid="632525516593368708">
        <Properties type="String" initWith="DbUsername">db_DBUsername</Properties>
      </treenode>
      <treenode text="db_DBPassword" id="632525611933748040" vid="632525516593368710">
        <Properties type="String" initWith="DbPassword">db_DBPassword</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="632525611933748042" vid="632525516593368715">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
      <treenode text="db_getUsersQuery" id="632525611933748044" vid="632525516593368719">
        <Properties type="String" defaultInitWith="SELECT D.first_name, D.last_name, C.directory_number FROM as_users D LEFT JOIN (SELECT B.as_users_id, A.directory_number from as_directory_numbers A, as_phone_devices B where ((A.as_phone_devices_id = B.as_phone_devices_id) AND (B.is_primary_device=1))) C ON D.as_users_id = C.as_users_id ORDER BY D.last_name ASC;">db_getUsersQuery</Properties>
      </treenode>
      <treenode text="g_companyName" id="632525611933748046" vid="632525516593368725">
        <Properties type="String" initWith="CompanyName">g_companyName</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="InitialRequest" startnode="632525516593368629" treenode="632525516593368630" appnode="632525516593368627" handlerfor="632525516593368626">
    <node type="Start" id="632525516593368629" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="270">
      <linkto id="632525516593368717" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632525516593368717" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="121" y="270">
      <linkto id="632525573169070548" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">routingGuid</ap>
        <rd field="ResultData">g_routingGuid</rd>
      </Properties>
    </node>
    <node type="Action" id="632525573169070548" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="223" y="270">
      <linkto id="632525573169070551" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="csharp">g_companyName + " Directory"</ap>
        <ap name="Prompt" type="literal">Make Your Selection...</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632525573169070551" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="341" y="270">
      <linkto id="632525573169070590" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">g_companyName + " Directory"</ap>
        <ap name="URL" type="csharp">host + "/AppSuiteDirectory/Listing?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632525573169070587" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="565" y="270">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632525573169070590" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="449" y="271">
      <linkto id="632525573169070587" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Variable" id="632525516593368713" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632525516593368714" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632525573169070549" name="menu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">menu</Properties>
    </node>
    <node type="Variable" id="632525573169070550" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632525573169070555" treenode="632525573169070556" appnode="632525573169070553" handlerfor="632525573169070552">
    <node type="Loop" id="632525573169070582" name="Loop" text="loop (var)" cx="347" cy="293" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="787" y="147" mx="960" my="294">
      <linkto id="632525573169070583" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632525573169070589" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="dictEnum" type="variable">nameToNumberMap</Properties>
    </node>
    <node type="Start" id="632525573169070555" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="80" y="289">
      <linkto id="632525573169070562" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632525573169070562" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="176.1888" y="289">
      <linkto id="632525573169070563" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632525573169070592" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">db_DBName</ap>
        <ap name="Server" type="variable">db_DBServer</ap>
        <ap name="Port" type="variable">db_DBPort</ap>
        <ap name="Username" type="variable">db_DBUsername</ap>
        <ap name="Password" type="variable">db_DBPassword</ap>
        <rd field="DSN">DSN</rd>
        <log condition="default" on="true" level="Info" type="literal">OnIncomingCall: could not format DSN.</log>
      </Properties>
    </node>
    <node type="Action" id="632525573169070563" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="289.188843" y="289">
      <linkto id="632525573169070564" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632525573169070592" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">DSN</ap>
        <ap name="Name" type="variable">db_DBConnectionName</ap>
        <ap name="Type" type="literal">mysql</ap>
        <log condition="default" on="true" level="Info" type="literal">OnIncomingCall: failed to open application suite database connection</log>
      </Properties>
    </node>
    <node type="Action" id="632525573169070564" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="403" y="290">
      <linkto id="632525573169070565" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632525573169070592" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="variable">db_getUsersQuery</ap>
        <ap name="Name" type="variable">db_DBConnectionName</ap>
        <rd field="ResultSet">dataTable</rd>
      </Properties>
    </node>
    <node type="Action" id="632525573169070565" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="515" y="290">
      <linkto id="632525573169070588" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632525573169070592" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(DataTable dataTable, ref SortedList nameToNumberMap)
{
	if (dataTable == null || dataTable.Rows.Count == 0)
		return IApp.VALUE_FAILURE;

	foreach (DataRow row in dataTable.Rows)
	{
		string lastName = row["last_name"] as string;
		string firstName = row["first_name"] as string;
		string directoryNumber = row["directory_number"] as string;
		nameToNumberMap.Add(lastName + ", " + firstName, directoryNumber);
	}

	return IApp.VALUE_SUCCESS;
}</Properties>
    </node>
    <node type="Action" id="632525573169070571" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1391.7179" y="291">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632525573169070583" name="If" container="632525573169070582" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="860" y="294">
      <linkto id="632525573169070584" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632525573169070585" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">loopDictEnum.Value != null</ap>
      </Properties>
    </node>
    <node type="Action" id="632525573169070584" name="AddDirectoryEntry" container="632525573169070582" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="981" y="216">
      <linkto id="632525573169070582" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">loopDictEnum.Key as string</ap>
        <ap name="Telephone" type="literal">Not Found</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632525573169070585" name="AddDirectoryEntry" container="632525573169070582" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="979" y="381">
      <linkto id="632525573169070582" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">loopDictEnum.Key as string</ap>
        <ap name="Telephone" type="csharp">loopDictEnum.Value as string</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632525573169070588" name="CreateDirectory" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="638" y="291">
      <linkto id="632525573169070582" port="1" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632525573169070592" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="csharp">g_companyName + " Directory"</ap>
        <ap name="Prompt" type="literal">Make Your Selection...</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632525573169070589" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1250.66" y="292">
      <linkto id="632525573169070571" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">directory.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632525573169070591" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="401" y="584">
      <linkto id="632525573169070594" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632525573169070592" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="400" y="475">
      <linkto id="632525573169070591" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="csharp">g_companyName + " Directory"</ap>
        <ap name="Text" type="literal">An error occured while trying to retrieve directory</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632525573169070594" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="522" y="584">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632525573169070557" name="DSN" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">DSN</Properties>
    </node>
    <node type="Variable" id="632525573169070558" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632525573169070559" name="dataTable" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">dataTable</Properties>
    </node>
    <node type="Variable" id="632525573169070560" name="nameToNumberMap" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="SortedList" refType="reference">nameToNumberMap</Properties>
    </node>
    <node type="Variable" id="632525573169070561" name="directory" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Directory" refType="reference">directory</Properties>
    </node>
    <node type="Variable" id="632525573169070593" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="632525573169070579" treenode="632525573169070580" appnode="632525573169070577" handlerfor="632525573169070576">
    <node type="Start" id="632525573169070579" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="66" y="322">
      <linkto id="632525573169070581" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632525573169070581" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="160" y="322">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>