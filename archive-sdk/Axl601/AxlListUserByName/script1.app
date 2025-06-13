<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="633096755035469141" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="633096755035469138" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633096755035469137" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/AxlListUserByName</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_ccmIp" id="633117081144844312" vid="633096755035469150">
        <Properties type="String" initWith="callManagerIP">g_ccmIp</Properties>
      </treenode>
      <treenode text="g_user" id="633117081144844314" vid="633096755035469152">
        <Properties type="String" initWith="ccmUsername">g_user</Properties>
      </treenode>
      <treenode text="g_pass" id="633117081144844316" vid="633096755035469154">
        <Properties type="String" initWith="ccmPassword">g_pass</Properties>
      </treenode>
      <treenode text="g_first" id="633117081144844318" vid="633096755035469156">
        <Properties type="String" initWith="firstName">g_first</Properties>
      </treenode>
      <treenode text="g_last" id="633117081144844320" vid="633096755035469158">
        <Properties type="String" initWith="lastName">g_last</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="633096755035469140" treenode="633096755035469141" appnode="633096755035469138" handlerfor="633096755035469137">
    <node type="Start" id="633096755035469140" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="79" y="297">
      <linkto id="633096755035469161" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="633096755035469161" name="ListUserByName" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap601" x="260" y="294">
      <linkto id="633096755035469163" type="Labeled" style="Vector" ortho="true" label="success" />
      <linkto id="633096755035469167" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="FirstName" type="variable">g_first</ap>
        <ap name="LastName" type="variable">g_last</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIp</ap>
        <ap name="AdminUsername" type="variable">g_user</ap>
        <ap name="AdminPassword" type="variable">g_pass</ap>
        <rd field="ListUserByNameResponse">response</rd>
      </Properties>
    </node>
    <node type="Action" id="633096755035469162" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="511" y="296">
      <linkto id="633096755035469166" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="variable">body</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="633096755035469163" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="393" y="294">
      <linkto id="633096755035469162" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.AxlSoap601.ListUserByNameResponse response, ref string body)
{
	body = String.Empty;

	if(response.Response != null &amp;&amp;
		response.Response.@return != null)
	{
		for(int i = 0; i &lt; response.Response.@return.Length; i++)
		{
			body += String.Format("User {0}, First {1}, Last {2}\n", 
				response.Response.@return[i].userid, 
				response.Response.@return[i].firstname, 
				response.Response.@return[i].lastname);
		}
		

	}	
	else
	{
		body = "No users matched";
	}

	return "success";
}
</Properties>
    </node>
    <node type="Action" id="633096755035469166" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="630" y="296">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633096755035469167" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="260" y="425">
      <linkto id="633096755035469168" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Axl Failure</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="633096755035469168" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="261" y="543">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633096755035469160" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap601.ListUserByNameResponse" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="633096755035469164" name="body" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">body</Properties>
    </node>
    <node type="Variable" id="633096755035469165" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>