<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632994701834575202" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632994701834575199" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632994701834575198" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/axlgetuser</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_userId" id="632994785469030474" vid="632994701834575213">
        <Properties type="String" initWith="userId">g_userId</Properties>
      </treenode>
      <treenode text="g_ccmUser" id="632994785469030476" vid="632994701834575215">
        <Properties type="String" initWith="ccmUsername">g_ccmUser</Properties>
      </treenode>
      <treenode text="g_ccmPass" id="632994785469030478" vid="632994701834575217">
        <Properties type="String" initWith="ccmPassword">g_ccmPass</Properties>
      </treenode>
      <treenode text="g_ccmIp" id="632994785469030480" vid="632994701834575219">
        <Properties type="String" initWith="callManagerIP">g_ccmIp</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632994701834575201" treenode="632994701834575202" appnode="632994701834575199" handlerfor="632994701834575198">
    <node type="Start" id="632994701834575201" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="152" y="464">
      <linkto id="632994701834575203" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632994701834575203" name="GetUser" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="360" y="464">
      <linkto id="632994701834575224" type="Labeled" style="Vector" label="failure" />
      <linkto id="632994701834575226" type="Labeled" style="Vector" label="fault" />
      <linkto id="632994701834575239" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIp</ap>
        <ap name="AdminUsername" type="variable">g_ccmUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmPass</ap>
        <rd field="GetUserResponse">getUserResponse</rd>
        <rd field="FaultMessage">message</rd>
        <rd field="FaultCode">code</rd>
      </Properties>
    </node>
    <node type="Action" id="632994701834575224" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="360" y="592">
      <linkto id="632994701834575225" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
	public static string Execute(ref string formattedMessage)
	{
		formattedMessage = "UpdatePhone experienced an unknown error.  Check the " +  					 "Application Server log for more details.";

		return String.Empty;
	}
</Properties>
    </node>
    <node type="Label" id="632994701834575225" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="368" y="720" />
    <node type="Action" id="632994701834575226" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="560" y="584">
      <linkto id="632994701834575227" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
	public static string Execute(ref string formattedMessage, string fault, int code)
	{
		formattedMessage = 
		"UpdatePhone SOAP Fault occurred: \n" +
		"Fault Code: " + code + "\n" + 
		"Fault Message: " + fault;

		return String.Empty; 
	}
</Properties>
    </node>
    <node type="Label" id="632994701834575227" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="568" y="720" />
    <node type="Action" id="632994701834575228" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="728" y="464">
      <linkto id="632994701834575229" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="variable">formattedMessage</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632994701834575229" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="824" y="464">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632994701834575230" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="728" y="368">
      <linkto id="632994701834575228" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632994701834575239" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="560" y="464">
      <linkto id="632994701834575228" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.AxlSoap413.GetUserResponse getUserResponse, ref string formattedMessage)
{

	string associated = "";
	if(getUserResponse.Response.@return.user.associatedDevices != null)
	{
		foreach(string associatedDevices in getUserResponse.Response.@return.user.associatedDevices)
		{
			associated += associatedDevices + ":";
		}
		
	}

      string dps = "";
	if(getUserResponse.Response.@return.user.phoneProfiles != null &amp;&amp; 
getUserResponse.Response.@return.user.phoneProfiles.Items != null)
	{
		foreach(object phoneProfileObj in getUserResponse.Response.@return.user.phoneProfiles.Items )
		{
			// I check either because the code which is auto-generated says either can be returned
			if(phoneProfileObj is string)
			{
				dps += phoneProfileObj + ":";
			}
			else if(phoneProfileObj is XPhoneProfile)
			{
				XPhoneProfile phoneProfile = phoneProfileObj as XPhoneProfile;
				dps += phoneProfile.uuid;
			}
		}
		
	}

      XUser user = getUserResponse.Response.@return.user as XUser;


	formattedMessage = String.Format(@"
firstname: {0}
lastname: {1}
userid: {2}
password: {3}
pin: {4}
telephoneNumber: {5}
extension: {6}
department: {7}
manager: {8}
associatedDevices: {9}
associatedPC: {10}
iaqExtension: {11}
phoneProfiles: {12}",

user.firstname,
user.lastname,
user.userid,
user.password,
user.pin,
user.telephoneNumber,
user.extension,
user.department,
user.manager,
associated,
user.associatedPC,
user.iaqExtension,
dps);

	return "";



}
</Properties>
    </node>
    <node type="Variable" id="632994701834575221" name="code" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">code</Properties>
    </node>
    <node type="Variable" id="632994701834575222" name="message" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">message</Properties>
    </node>
    <node type="Variable" id="632994701834575223" name="getUserResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.GetUserResponse" refType="reference">getUserResponse</Properties>
    </node>
    <node type="Variable" id="632994701834575238" name="formattedMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">formattedMessage</Properties>
    </node>
    <node type="Variable" id="632994701834575240" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>