<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632919681893171693" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632919681893171690" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632919681893171689" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/executeSqlQuery</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_user" id="632996402763278523" vid="632919681893171704">
        <Properties type="String" initWith="Username">g_user</Properties>
      </treenode>
      <treenode text="g_pass" id="632996402763278525" vid="632919681893171706">
        <Properties type="String" initWith="Password">g_pass</Properties>
      </treenode>
      <treenode text="g_ip" id="632996402763278527" vid="632919681893171708">
        <Properties type="String" initWith="CallManagerIP">g_ip</Properties>
      </treenode>
      <treenode text="g_query" id="632996402763278529" vid="632919681893171710">
        <Properties type="String" initWith="query">g_query</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632919681893171692" treenode="632919681893171693" appnode="632919681893171690" handlerfor="632919681893171689">
    <node type="Start" id="632919681893171692" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="60" y="252">
      <linkto id="632919681893171694" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632919681893171694" name="ExecuteSQLQuery" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="279" y="252">
      <linkto id="632920143635856115" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">post["query"] != null ? post["query"] : g_query</ap>
        <ap name="CallManagerIP" type="variable">g_ip</ap>
        <ap name="AdminUsername" type="variable">g_user</ap>
        <ap name="AdminPassword" type="variable">g_pass</ap>
        <rd field="ExecuteSQLQueryResponse">executeResponse</rd>
      </Properties>
    </node>
    <node type="Action" id="632920143635856115" name="ParseSQLQuery" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="465" y="251">
      <linkto id="632920143635856119" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ExecuteSQLQueryResponse" type="variable">executeResponse</ap>
        <rd field="DataTable">table</rd>
      </Properties>
    </node>
    <node type="Action" id="632920143635856117" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="950" y="250">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632920143635856119" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="640" y="251">
      <linkto id="632920143635856122" type="Labeled" style="Bezier" label="default" />
      <Properties language="csharp">
public static string Execute(DataTable table, ref string body)
{
	body = "&lt;html&gt;&lt;style&gt;&lt;/style&gt;&lt;body&gt;";

	if(table.Rows.Count == 0)
	{

	}
	else
	{
		body += "&lt;table border='1'&gt;";

		body += "&lt;tr&gt;";

		foreach(DataColumn column in table.Columns)
		{
			body += "&lt;td&gt;";
			body += column.ColumnName;
			body += "&lt;/td&gt;";
		}

		body += "&lt;/tr&gt;";


		foreach(DataRow row in table.Rows)
		{
			body += "&lt;tr&gt;";

			foreach(DataColumn column in table.Columns)
			{
				body += "&lt;td&gt;";
				body += row[column.ColumnName] == DBNull.Value ? "DBNULL" : row[column.ColumnName].ToString();
				body += "&lt;/td&gt;";
			}

			body += "&lt;/tr&gt;";
		}

		body += "&lt;/table&gt;";
	}

	body += "&lt;/body&gt;&lt;/html&gt;";

	return "";
}
</Properties>
    </node>
    <node type="Action" id="632920143635856122" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="801" y="250">
      <linkto id="632920143635856117" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="variable">body</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Variable" id="632920143635856116" name="executeResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.ExecuteSQLQueryResponse" refType="reference">executeResponse</Properties>
    </node>
    <node type="Variable" id="632920143635856118" name="table" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">table</Properties>
    </node>
    <node type="Variable" id="632920143635856120" name="body" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" refType="reference">body</Properties>
    </node>
    <node type="Variable" id="632920143635856121" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632996402763278540" name="post" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.FormCollection" initWith="body" refType="reference">post</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>