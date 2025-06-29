<Application name="Main" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="Main">
    <outline>
      <treenode type="evh" id="632419389328631748" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632419389328631745" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632419389328631744" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/simpleaxl</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632419389328631747" treenode="632419389328631748" appnode="632419389328631745" handlerfor="632419389328631744">
    <node type="Start" id="632419389328631747" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="199">
      <linkto id="632419389328631749" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632419389328631749" name="GetPhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="208" y="200">
      <linkto id="632419389328631756" type="Labeled" style="Bezier" ortho="true" label="fault" />
      <linkto id="632419389328631757" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <linkto id="632419389328631750" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="PhoneName" type="literal">SEP000750527A68</ap>
        <ap name="CallManagerIP" type="literal">10.1.10.25</ap>
        <ap name="AdminPassword" type="literal">metreos</ap>
        <rd field="GetPhoneResponse">getPhoneResponse</rd>
        <rd field="FaultMessage">fault</rd>
        <rd field="FaultCode">code</rd>
      </Properties>
    </node>
    <node type="Action" id="632419389328631750" name="UpdatePhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="384" y="200">
      <linkto id="632419389328631763" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <linkto id="632419389328631765" type="Labeled" style="Bezier" ortho="true" label="fault" />
      <linkto id="632419389328631768" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Description" type="literal">DateTime.Now.ToString()</ap>
        <ap name="PhoneName" type="literal">SEP000750527A68</ap>
        <ap name="CallManagerIP" type="literal">10.1.10.25</ap>
        <ap name="AdminPassword" type="literal">metreos</ap>
      </Properties>
    </node>
    <node type="Action" id="632419389328631752" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="616" y="200">
      <linkto id="632419389328631755" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="variable">formattedMessage</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
      </Properties>
    </node>
    <node type="Action" id="632419389328631755" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="712" y="200">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632419389328631756" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="296" y="360">
      <linkto id="632419389328631759" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref string formattedMessage, string fault, int code)
	{
		formattedMessage = 
		"GetPhone SOAP Fault occurred: \n" +
		"Fault Code: " + code + "\n" + 
		"Fault Message: " + fault;

		return String.Empty; 
	}
</Properties>
    </node>
    <node type="Action" id="632419389328631757" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="208" y="360">
      <linkto id="632419389328631760" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref string formattedMessage)
	{
		formattedMessage = "GetPhone experienced an unknown error.  Check the " + 						 "Application Server log for more details.";

		return String.Empty;
	}
</Properties>
    </node>
    <node type="Label" id="632419389328631758" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="712" y="120">
      <linkto id="632419389328631755" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632419389328631759" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="296" y="448" />
    <node type="Label" id="632419389328631760" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="208" y="448" />
    <node type="Action" id="632419389328631763" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="384" y="360">
      <linkto id="632419389328631764" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref string formattedMessage)
	{
		formattedMessage = "UpdatePhone experienced an unknown error.  Check the " +  					 "Application Server log for more details.";

		return String.Empty;
	}
</Properties>
    </node>
    <node type="Label" id="632419389328631764" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="384" y="448" />
    <node type="Action" id="632419389328631765" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="472" y="360">
      <linkto id="632419389328631767" type="Labeled" style="Bezier" ortho="true" label="default" />
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
    <node type="Label" id="632419389328631767" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="472" y="448" />
    <node type="Action" id="632419389328631768" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="504" y="200">
      <linkto id="632419389328631752" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(Metreos.Types.AxlSoap333.GetPhoneResponse getPhoneResponse, ref string formattedMessage)
	{
		int numLines = getPhoneResponse.Response.@return.device.lines == null ? 0 : 
			         getPhoneResponse.Response.@return.device.lines.Items.Length;

		formattedMessage = "The device has " + numLines + " lines";

		return String.Empty; 
	}
</Properties>
    </node>
    <node type="Comment" id="632419503266382531" text="Shows sample usage of GetPhone and UpdatePhone&#xD;&#xA;in a very simple and limited fashion." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="32" />
    <node type="Comment" id="632419503266382532" text="The usage of UpdatePhone shows the minimal amount of fields necessary to use this action &#xD;&#xA;The only Action Parameter that is unneccssary to specify is description, which is used to &#xD;&#xA;simply demonstrate that UpdatePhone works without changing a pertinent aspect of this&#xD;&#xA;phone device).&#xD;&#xA;&#xD;&#xA;The Action Parameters which are currently blank will be ignored by the AXL SOAP API, &#xD;&#xA;which is helpful, as one can change specific fields in this manner, by leaving others blank.&#xD;&#xA;&#xD;&#xA;The response variable to the UpdatePhone method only contains a GUID for this &#xD;&#xA;transaction.  Not necessarily useful in many instances." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="536" y="248" />
    <node type="Comment" id="632419503266382533" text="In the last CustomCode action, we determine the amount&#xD;&#xA;of lines on the device and send that value back in the &#xD;&#xA;SendResponse action." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="336" y="104" />
    <node type="Comment" id="632419503266382534" text="A 'fault' path is a SOAP specific error.&#xD;&#xA;In this case, we use the fault code and &#xD;&#xA;fault message result data to display &#xD;&#xA;back to the browser which made this&#xD;&#xA;request.&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="192" y="480" />
    <node type="Variable" id="632419389328631751" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632419389328631753" name="formattedMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">formattedMessage</Properties>
    </node>
    <node type="Variable" id="632419389328631754" name="getPhoneResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap333.GetPhoneResponse" refType="reference">getPhoneResponse</Properties>
    </node>
    <node type="Variable" id="632419389328631761" name="code" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">code</Properties>
    </node>
    <node type="Variable" id="632419389328631762" name="fault" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">fault</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>