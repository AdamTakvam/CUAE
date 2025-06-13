<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632422597965332845" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632422597965332842" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632422597965332841" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/axlsetforwardall</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_devicename" id="632589664069272981" vid="632586594266446388">
        <Properties type="String" initWith="devicename">g_devicename</Properties>
      </treenode>
      <treenode text="g_ccmIp" id="632589664069272983" vid="632586594266446390">
        <Properties type="String" initWith="callManagerIP">g_ccmIp</Properties>
      </treenode>
      <treenode text="g_ccmUsername" id="632589664069272985" vid="632586594266446392">
        <Properties type="String" initWith="ccmUsername">g_ccmUsername</Properties>
      </treenode>
      <treenode text="g_ccmPassword" id="632589664069272987" vid="632586594266446394">
        <Properties type="String" initWith="ccmPassword">g_ccmPassword</Properties>
      </treenode>
      <treenode text="g_linePosition" id="632589664069272989" vid="632586594266446396">
        <Properties type="Int" initWith="linePosition">g_linePosition</Properties>
      </treenode>
      <treenode text="g_forwardDn" id="632589664069272991" vid="632586594266446398">
        <Properties type="String" initWith="forwardDN">g_forwardDn</Properties>
      </treenode>
      <treenode text="g_forwardCss" id="632589664069272993" vid="632586594266446400">
        <Properties type="String" initWith="forwardCss">g_forwardCss</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632422597965332844" treenode="632422597965332845" appnode="632422597965332842" handlerfor="632422597965332841">
    <node type="Start" id="632422597965332844" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="360">
      <linkto id="632422663463697336" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632422610569637355" name="UpdateLine" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="448" y="360">
      <linkto id="632422663463697344" type="Labeled" style="Bevel" label="success" />
      <linkto id="632589664069273013" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Uuid" type="csharp">lineId</ap>
        <ap name="CallForwardAll" type="variable">callForwardAll</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIp</ap>
        <ap name="AdminUsername" type="variable">g_ccmUsername</ap>
        <ap name="AdminPassword" type="variable">g_ccmPassword</ap>
      </Properties>
    </node>
    <node type="Action" id="632422663463697334" name="CreateForward" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="336" y="360">
      <linkto id="632422610569637355" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Destination" type="variable">g_forwardDn</ap>
        <ap name="CallingSearchSpaceName" type="variable">g_forwardCss</ap>
        <rd field="Forward">callForwardAll</rd>
      </Properties>
    </node>
    <node type="Action" id="632422663463697336" name="GetPhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="112" y="360">
      <linkto id="632422663463697339" type="Labeled" style="Bevel" label="success" />
      <linkto id="632425466128232863" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="variable">g_devicename</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIp</ap>
        <ap name="AdminUsername" type="variable">g_ccmUsername</ap>
        <ap name="AdminPassword" type="variable">g_ccmPassword</ap>
        <rd field="GetPhoneResponse">getPhoneResponse</rd>
      </Properties>
    </node>
    <node type="Action" id="632422663463697339" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="224" y="360">
      <linkto id="632422663463697334" type="Labeled" style="Bevel" label="default" />
      <linkto id="632425466128232860" type="Labeled" style="Bevel" label="nolines" />
      <Properties language="csharp">
	public static string Execute(Metreos.Types.AxlSoap333.GetPhoneResponse getPhoneResponse, ref string lineId, ref string formattedMessage, int g_linePosition)
	{
		// if we can find no line information, then this device must have
            // no lines, and so is not useful for this demo.
		if(getPhoneResponse.Response.@return.device.lines == null ||
               getPhoneResponse.Response.@return.device.lines.Items == null ||
               getPhoneResponse.Response.@return.device.lines.Items.Length == 0)
            {
		     formattedMessage = "Use a device with phone lines";
                 return "nolines";
            }

            if(getPhoneResponse.Response.@return.device.lines.Items.Length &lt; g_linePosition)
            {
			formattedMessage = "This device does not have a line at position " + g_linePosition;
			return "nolines";
		}
 		Metreos.AxlSoap333.XLine lineInfo = (Metreos.AxlSoap333.XLine) getPhoneResponse.Response.@return.device.lines.Items[g_linePosition - 1]; 

		lineId = lineInfo.Item.uuid;

		return String.Empty;
	}
</Properties>
    </node>
    <node type="Comment" id="632422663463697340" text="In this example, we want to change the call forwarding information for a particular line.&#xD;&#xA;&#xD;&#xA;This requires the use of the AXL SOAP UpdateLine.&#xD;&#xA;&#xD;&#xA;To identify the line to update, you must do 1 of 2 things:&#xD;&#xA;One: specifiy the pattern and partition of the line, or&#xD;&#xA;Two: specify the UUID of the line.&#xD;&#xA;&#xD;&#xA;The first option presents problems in deployments where partitions for lines are not necessarily a constant.&#xD;&#xA;The second option works well; as long as we can determine the UUID of the line.&#xD;&#xA;&#xD;&#xA;This is what leads us to first use GetPhone.   GetPhone will return all lines for a given phone,&#xD;&#xA;returning UUIDs for each line.  So, if we know the device that we want to alter forwarding &#xD;&#xA;information on, then the problem of identifying the line becomes solvable, allowing us to then&#xD;&#xA;change the forwarding information of that line." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="33" y="33" />
    <node type="Comment" id="632422663463697341" text="The 'CreateForward' action is needed if you want to change forwarding information for a given line.&#xD;&#xA;&#xD;&#xA;Use CreateForward for each forwarding parameter you want to change.&#xD;&#xA;&#xD;&#xA;There are 5 forwarding parameters in Callmanager: (not all are valid on every type of device)&#xD;&#xA;Forward All&#xD;&#xA;Forward Busy&#xD;&#xA;Forward No Answer&#xD;&#xA;Forward Alternate Party&#xD;&#xA;Forward On Failure&#xD;&#xA;&#xD;&#xA;So, if you wish to change Forward All and Forward Busy, you will need to use 2 'CreateForward' actions;&#xD;&#xA;one for each forward parameter.&#xD;&#xA;&#xD;&#xA;In this example, we are simply going to change the Forward All parameter for this line." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="440" />
    <node type="Action" id="632422663463697344" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="568" y="360">
      <linkto id="632422663463697345" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="variable">formattedMessage</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632422663463697345" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="672" y="360">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632425466128232858" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="568" y="272">
      <linkto id="632422663463697344" type="Basic" style="Bevel" />
    </node>
    <node type="Label" id="632425466128232860" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="272" y="432" />
    <node type="Label" id="632425466128232863" text="b" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="160" y="432" />
    <node type="Action" id="632425466128232864" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="496" y="272">
      <linkto id="632422663463697344" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Be sure that the device name action parameter is a valid phone</ap>
        <rd field="ResultData">formattedMessage</rd>
      </Properties>
    </node>
    <node type="Label" id="632425466128232865" text="b" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="408" y="272">
      <linkto id="632425466128232864" type="Basic" style="Bevel" />
    </node>
    <node type="Label" id="632589664069273013" text="b" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="503" y="430" />
    <node type="Variable" id="632422663463697335" name="callForwardAll" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap333.CallForward" refType="reference">callForwardAll</Properties>
    </node>
    <node type="Variable" id="632422663463697337" name="lineId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">lineId</Properties>
    </node>
    <node type="Variable" id="632422663463697338" name="getPhoneResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap333.GetPhoneResponse" refType="reference">getPhoneResponse</Properties>
    </node>
    <node type="Variable" id="632422663463697343" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632425466128232862" name="formattedMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="Check Callmanager ccmadmin to be sure these changes took effect." refType="reference">formattedMessage</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>