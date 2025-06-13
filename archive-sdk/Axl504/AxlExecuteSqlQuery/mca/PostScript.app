<Application name="PostScript" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="PostScript">
    <outline>
      <treenode type="evh" id="632996402763278507" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632996402763278504" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632996402763278503" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ExecutePost</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632996402763278506" treenode="632996402763278507" appnode="632996402763278504" handlerfor="632996402763278503">
    <node type="Start" id="632996402763278506" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="112" y="464">
      <linkto id="632996402763278511" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632996402763278511" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="368" y="464">
      <linkto id="632996402763278512" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">public static string Execute(string host, ref string body)
{
	body = String.Format(@"
&lt;html&gt;
	&lt;head&gt;&lt;title&gt;Test ExecuteSqlQuery!&lt;/title&gt;&lt;/head&gt;
	&lt;body&gt;
		&lt;form action='http://{0}/ExecuteSqlQuery' method='POST'&gt;
			&lt;label style='font-weight:bold' for='Query'&gt;Query Test&lt;/label&gt;&lt;br /&gt;
			&lt;textarea name=’query’ id='content' rows='15' cols='50'&gt;
select * device.name from numplan, device, devicenumplanmap where numplan.pkid = devicenumplanmap.fknumplan and device.pkid = devicenumplanmap.fkdevice and numplan.dnorpattern = 'THE DN TO QUERY'
&lt;/textarea&gt;&lt;br /&gt;
			&lt;input type='submit' value='Submit' /&gt;
		&lt;/form&gt;
	&lt;/body&gt;
&lt;/html&gt;", 
host);
	
	return "success";
}
</Properties>
    </node>
    <node type="Action" id="632996402763278512" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="576" y="464">
      <linkto id="632996402763278514" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="literal">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="variable">body</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632996402763278514" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="720" y="464">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632996402763278509" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632996402763278510" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632996402763278513" name="body" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="body" refType="reference">body</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>