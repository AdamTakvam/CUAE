<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632599905279436745" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632599905279436742" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632599905279436741" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/AxlReconfigureLine</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_profileName" id="632599905279436779" vid="632599905279436778">
        <Properties type="String" initWith="profileName">g_profileName</Properties>
      </treenode>
      <treenode text="g_callManagerIP" id="632599905279436781" vid="632599905279436780">
        <Properties type="String" initWith="callManagerIP">g_callManagerIP</Properties>
      </treenode>
      <treenode text="g_administrator" id="632599905279436783" vid="632599905279436782">
        <Properties type="String" initWith="ccmUsername">g_administrator</Properties>
      </treenode>
      <treenode text="g_password" id="632599905279436785" vid="632599905279436784">
        <Properties type="String" initWith="ccmPassword">g_password</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632599905279436744" treenode="632599905279436745" appnode="632599905279436742" handlerfor="632599905279436741">
    <node type="Start" id="632599905279436744" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="144" y="312">
      <linkto id="632599905279436786" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632599905279436786" name="GetDeviceProfile" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="288" y="312">
      <linkto id="632599905279436787" type="Labeled" style="Bevel" label="default" />
      <linkto id="632599905279436790" type="Labeled" style="Bevel" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="ProfileName" type="variable">g_profileName</ap>
        <ap name="CallManagerIP" type="variable">g_callManagerIP</ap>
        <ap name="AdminUsername" type="variable">g_administrator</ap>
        <ap name="AdminPassword" type="variable">g_password</ap>
        <rd field="GetDeviceProfileResponse">getDeviceProfile</rd>
      </Properties>
    </node>
    <node type="Label" id="632599905279436787" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="288" y="448" />
    <node type="Action" id="632599905279436788" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="557" y="236">
      <linkto id="632599905279436796" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Error</ap>
        <rd field="ResultData">message</rd>
      </Properties>
    </node>
    <node type="Action" id="632599905279436790" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="416" y="312">
      <linkto id="632599905279436791" type="Labeled" style="Bevel" label="default" />
      <linkto id="632599905279436797" type="Labeled" style="Bevel" label="success" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.AxlSoap413.GetDeviceProfileResponse getDeviceProfile, Metreos.Types.AxlSoap413.Lines lines)
{
	// Checking for any nulls leading up to the lines structure
	if(	getDeviceProfile == null ||
		getDeviceProfile.Response == null ||
		getDeviceProfile.Response.@return == null ||
		getDeviceProfile.Response.@return.profile == null ||
		getDeviceProfile.Response.@return.profile.lines == null ||
		getDeviceProfile.Response.@return.profile.lines.Items == null ||
		getDeviceProfile.Response.@return.profile.lines.Items.Length == 0) 
	{
		return "nolines";
	}

	// Initialize the line data structure
	Metreos.AxlSoap413.XLine[] lineData = new Metreos.AxlSoap413.XLine[getDeviceProfile.Response.@return.profile.lines.Items.Length];

	Array.Copy(getDeviceProfile.Response.@return.profile.lines.Items,
			lineData, lineData.Length);

   	lineData[0].e164Mask = "2000";

	lines.Data = lineData;

	return "success";
}
</Properties>
    </node>
    <node type="Label" id="632599905279436791" text="n" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="416" y="448" />
    <node type="Label" id="632599905279436792" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="472" y="240">
      <linkto id="632599905279436788" type="Basic" style="Bevel" />
    </node>
    <node type="Label" id="632599905279436793" text="n" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="536" y="160">
      <linkto id="632599905279436794" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632599905279436794" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="648" y="160">
      <linkto id="632599905279436796" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">No lines to update on specified profile</ap>
        <rd field="ResultData">message</rd>
      </Properties>
    </node>
    <node type="Action" id="632599905279436796" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="696" y="312">
      <linkto id="632599905279436798" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="variable">message</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632599905279436797" name="UpdateDeviceProfile" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="552" y="312">
      <linkto id="632599905279436796" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ProfileName" type="variable">g_profileName</ap>
        <ap name="Lines" type="variable">lines</ap>
        <ap name="CallManagerIP" type="variable">g_callManagerIP</ap>
        <ap name="AdminUsername" type="variable">g_administrator</ap>
        <ap name="AdminPassword" type="variable">g_password</ap>
      </Properties>
    </node>
    <node type="Action" id="632599905279436798" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="824" y="312">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632599905279436799" text="There is currently no elegant way to update the line items &#xD;&#xA;found in the 'AddLineItem' action *while* simultaneously &#xD;&#xA;preserving the lines already configured on a phone.&#xD;&#xA;&#xD;&#xA;The CustomCode action shows how to alter the external &#xD;&#xA;number mask for an existing line, in position 0." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="112" y="72" />
    <node type="Variable" id="632599905279436746" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632599905279436749" name="getDeviceProfile" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.GetDeviceProfileResponse" refType="reference">getDeviceProfile</Properties>
    </node>
    <node type="Variable" id="632599905279436750" name="lines" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.Lines" refType="reference">lines</Properties>
    </node>
    <node type="Variable" id="632599905279436789" name="message" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="Success" refType="reference">message</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>