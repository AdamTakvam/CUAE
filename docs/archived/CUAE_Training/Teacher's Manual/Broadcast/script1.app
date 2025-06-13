<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632934096737344270" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632934096737344267" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632934096737344266" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/minitest</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632934096737344269" treenode="632934096737344270" appnode="632934096737344267" handlerfor="632934096737344266">
    <node type="Start" id="632934096737344269" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="56" y="472">
      <linkto id="632934096737344271" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632934096737344271" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="240" y="472">
      <linkto id="632934096737344276" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string host, string routingGuid, ref string body)
{
	body = String.Format(@"
&lt;html&gt;
	&lt;head&gt;&lt;title&gt;Broadcast!&lt;/title&gt;&lt;/head&gt;
	&lt;body&gt;
		&lt;form action='http://{0}/JohnDoe/HandleBroadcastPost?metreosSessionId={1}' method='POST'&gt;
			&lt;label style='font-weight:bold' for='content'&gt;Broadcast Message&lt;/label&gt;&lt;br /&gt;
			&lt;textarea id='content' rows='8' cols='30'&gt;&lt;/textarea&gt;&lt;br /&gt;
			&lt;input type='submit' value='Submit' /&gt;
		&lt;/form&gt;
	&lt;/body&gt;
&lt;/html&gt;", 
host, routingGuid);
	
	return "success";
}

</Properties>
    </node>
    <node type="Action" id="632934096737344276" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="400" y="472">
      <linkto id="632934096737344277" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">Ok</ap>
        <ap name="body" type="variable">body</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632934096737344277" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="608" y="472">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632934096737344272" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632934096737344273" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="routingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632934096737344274" name="body" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">body</Properties>
    </node>
    <node type="Variable" id="632934096737344275" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>