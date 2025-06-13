<Application name="ShowDNC" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="ShowDNC">
    <outline>
      <treenode type="evh" id="632796826816036878" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632796826816036875" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632796826816036874" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/DoNotCall/Edit</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632796826816036877" treenode="632796826816036878" appnode="632796826816036875" handlerfor="632796826816036874">
    <node type="Start" id="632796826816036877" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="324">
      <linkto id="632796826816036885" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632796826816036880" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="443" y="327">
      <linkto id="632796826816036882" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="literal">SELECT toNumber from metreosDnc</ap>
        <ap name="Name" type="literal">DoNotCall</ap>
        <rd field="ResultSet">queryDncResults</rd>
      </Properties>
    </node>
    <node type="Action" id="632796826816036882" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="564" y="327">
      <linkto id="632796826816036884" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string response, DataTable queryDncResults, string host)
{
response = @"&lt;html&gt;
&lt;head&gt;&lt;title&gt;Metreos DoNotCall Demo Database&lt;/title&gt;
&lt;style&gt;
caption            { border: 1px solid #cecece; padding: 2px 2px; background-color:#efefef; color:black; }
th            { width:100px; border: 1px solid #cecece; padding: 2px 2px; background-color:#202020; color:#eeeeee; }
td            { width:100px; border: 1px solid #232323; padding: 2px 2px; text-align:center; }
#formDiv            { text-align:right; border-width:1px 0 0; border-style:solid; border-color:#232323; padding:2px; }
span {color:black; text-align:left;}
&lt;/style&gt;
&lt;/head&gt;
&lt;body&gt;
&lt;div style=""position:relative;width:200px""&gt;
&lt;form action=""http://" + host + @"/DoNotCall/Edit"" method=""post""&gt;
&lt;TABLE&gt;
&lt;caption&gt;&lt;strong&gt;Do Not Call Database&lt;strong&gt;&lt;/caption&gt;
&lt;tr&gt;&lt;th&gt;&lt;/th&gt;&lt;th&gt;Number&lt;/th&gt;&lt;/tr&gt;";

int resultCount = queryDncResults != null ? queryDncResults.Rows.Count : 0;

for(int i = 0; i &lt; resultCount; i++)
{
	response += @"&lt;tr&gt;&lt;td&gt;&lt;input type=""checkbox"" name=""delete"" VALUE=""" + queryDncResults.Rows[i][0] + @""" /&gt;&lt;/td&gt;&lt;td&gt;" + queryDncResults.Rows[i][0] + "&lt;/td&gt;&lt;/tr&gt;";

}

response += @"&lt;/table&gt;&lt;br/&gt;&lt;br/&gt;&lt;br/&gt;&lt;div&gt;&lt;span&gt;&lt;strong&gt;Take Action:&lt;/strong&gt;&lt;/span&gt;&lt;div id=""formDiv""&gt;&lt;input type=""submit"" name=""submit"" value=""Delete Checked""/&gt;&lt;br/&gt;&lt;br/&gt;&lt;input type=""text"" name=""add""/&gt;&lt;input type=""submit"" name=""submit"" value=""Add"" /&gt;&lt;/div&gt;&lt;/form&gt;&lt;/div&gt;&lt;/div&gt;&lt;/body&gt;&lt;/html&gt;";

return "";
}
</Properties>
    </node>
    <node type="Action" id="632796826816036884" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="679" y="328">
      <linkto id="632796826816036891" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="variable">response</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632796826816036885" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="117" y="324">
      <linkto id="632796826816036880" type="Labeled" style="Bezier" ortho="true" label="query" />
      <linkto id="632796826816036890" type="Labeled" style="Bezier" ortho="true" label="delete" />
      <linkto id="632796826816036889" type="Labeled" style="Bezier" ortho="true" label="add" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Http.FormCollection post, ref string add, ref string delete, LogWriter log)
{
	if(post["submit"] == null)
	{
		log.Write(TraceLevel.Info, "Normal DNC Query");
		return "query";
	}	
	else if (post["submit"] == "Add")
	{
		// Add request received
		add = post["add"];
		if(add == null || add == String.Empty)
		{
			return "query";
		}

		add = String.Format("insert into metreosdnc (toNumber) VALUES ('{0}')", add);
		log.Write(TraceLevel.Info, "Executing " + add);
		return "add";
	}
	else if (post["submit"] == "Delete Checked")
	{
		string[] deleteList = post.GetValues("delete");
		if(deleteList == null || deleteList.Length == 0)
		{
			return "query";
		}

		delete = "delete from metreosdnc ";
		for(int i = 0; i &lt; deleteList.Length; i++)
		{
			string toDelete = deleteList[i];
	
			delete += String.Format(" where toNumber = '{0}' || ", toDelete);
		}

		delete = delete.Substring(0, delete.Length - 4);
		log.Write(TraceLevel.Info, "Executing " + delete);
		return "delete";
	}
	else
	{
		return "query";
	}
}
</Properties>
    </node>
    <node type="Action" id="632796826816036889" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="300" y="102">
      <linkto id="632796826816036880" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="variable">add</ap>
        <ap name="Name" type="literal">DoNotCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632796826816036890" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="305" y="238">
      <linkto id="632796826816036880" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="variable">delete</ap>
        <ap name="Name" type="literal">DoNotCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632796826816036891" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="787" y="328">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632796826816036879" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632796826816036881" name="queryDncResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">queryDncResults</Properties>
    </node>
    <node type="Variable" id="632796826816036883" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632796826816036886" name="post" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.FormCollection" initWith="body" refType="reference" name="Metreos.Providers.Http.GotRequest">post</Properties>
    </node>
    <node type="Variable" id="632796826816036887" name="delete" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">delete</Properties>
    </node>
    <node type="Variable" id="632796826816036888" name="add" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">add</Properties>
    </node>
    <node type="Variable" id="632796826816036893" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>