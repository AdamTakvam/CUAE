<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632422597965332845" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632422597965332842" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632422597965332841" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/updateline</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632422597965332844" treenode="632422597965332845" appnode="632422597965332842" handlerfor="632422597965332841">
    <node type="Start" id="632422597965332844" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="360">
      <linkto id="632471858221825859" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632422610569637355" name="UpdateLine" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="392" y="360">
      <linkto id="632471858221825854" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="NewPattern" type="literal">2001</ap>
        <ap name="Uuid" type="variable">lineId2</ap>
        <ap name="CallManagerIP" type="literal">10.1.10.25</ap>
        <ap name="AdminPassword" type="literal">metreos</ap>
      </Properties>
    </node>
    <node type="Action" id="632422663463697339" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="224" y="360">
      <linkto id="632425466128232860" type="Labeled" style="Bezier" ortho="true" label="nolines" />
      <linkto id="632471858221825855" type="Labeled" style="Bezier" ortho="true" label="oneline" />
      <linkto id="632422610569637355" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(Metreos.Types.AxlSoap333.GetDeviceProfileResponse getDeviceResponse, ref string lineId1, ref string lineId2, ref string formattedMessage)
	{
		// if we can find no line information, then this device must have
            // no lines, and so is not useful for this demo.
		if(getDeviceResponse.Response.@return.profile.lines == null ||
               getDeviceResponse.Response.@return.profile.lines.Items == null ||
               getDeviceResponse.Response.@return.profile.lines.Items.Length == 0)
            {
		     formattedMessage = "Use a device with phone lines";
                 return "nolines";
            }

 		Metreos.AxlSoap333.XLine lineInfo1 = (Metreos.AxlSoap333.XLine) getDeviceResponse.Response.@return.profile.lines.Items[0]; 

		lineId1 = lineInfo1.Item.uuid;

		if(getDeviceResponse.Response.@return.profile.lines.Items.Length == 1)
		{
			formattedMessage = "Only 1 line was encountered.  The application can continue, but more were expected";
			return "oneline";
		}

		Metreos.AxlSoap333.XLine lineInfo2 = (Metreos.AxlSoap333.XLine) getDeviceResponse.Response.@return.profile.lines.Items[1];

		lineId2 = lineInfo2.Item.uuid;		

		return String.Empty;
	}
</Properties>
    </node>
    <node type="Action" id="632422663463697344" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="694.768066" y="360">
      <linkto id="632422663463697345" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="variable">formattedMessage</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632422663463697345" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="798.768066" y="360">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Label" id="632425466128232858" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="694.768066" y="272">
      <linkto id="632422663463697344" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632425466128232860" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="168" y="560" />
    <node type="Label" id="632425466128232863" text="b" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="112" y="464" />
    <node type="Action" id="632425466128232864" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="622.768066" y="272">
      <linkto id="632422663463697344" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">Be sure that the device name action parameter is a valid phone</ap>
        <rd field="ResultData">formattedMessage</rd>
      </Properties>
    </node>
    <node type="Label" id="632425466128232865" text="b" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="534.768066" y="272">
      <linkto id="632425466128232864" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632471858221825854" name="UpdateLine" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="544" y="360">
      <linkto id="632422663463697344" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="NewPattern" type="literal">2002</ap>
        <ap name="Uuid" type="variable">lineId1</ap>
        <ap name="CallManagerIP" type="literal">10.1.10.25</ap>
        <ap name="AdminPassword" type="literal">metreos</ap>
      </Properties>
    </node>
    <node type="Label" id="632471858221825855" text="c" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="304" y="560" />
    <node type="Label" id="632471858221825856" text="c" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="544" y="480">
      <linkto id="632471858221825854" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632471858221825857" text="In this example, we assume that the device profile&#xD;&#xA;being retreived has 2 lines.  If it does not, then we skip&#xD;&#xA;configuring the 2nd line.&#xD;&#xA;&#xD;&#xA;The CustomCode action determines how many&#xD;&#xA;lines we have, and assigns the line uuids to local variables,&#xD;&#xA;if found.&#xD;&#xA;&#xD;&#xA;To obtain the UUIDs of both lines, we have to use custom code&#xD;&#xA;to parse the results of the GetDeviceProfile response variable.&#xD;&#xA;The line info objects which contain the UUIds are contained in an&#xD;&#xA;array, which in this example, we individually index by using hardcoded &#xD;&#xA;0 and 1, to obtain the first and second lines." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="14" y="72" />
    <node type="Action" id="632471858221825859" name="GetDeviceProfile" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="112" y="360">
      <linkto id="632425466128232863" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632422663463697339" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native">
        <ap name="ProfileName" type="literal">TestProfile</ap>
        <ap name="CallManagerIP" type="literal">10.1.10.25</ap>
        <ap name="AdminPassword" type="literal">metreos</ap>
        <rd field="GetDeviceProfileResponse">getDeviceResponse</rd>
      </Properties>
    </node>
    <node type="Comment" id="632471858221825860" text="Skip configuration of 2nd line, &#xD;&#xA;because it does not exist" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="472" y="504" />
    <node type="Variable" id="632422663463697337" name="lineId1" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">lineId1</Properties>
    </node>
    <node type="Variable" id="632422663463697338" name="getDeviceResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap333.GetDeviceProfileResponse" refType="reference">getDeviceResponse</Properties>
    </node>
    <node type="Variable" id="632422663463697343" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632425466128232862" name="formattedMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="Check Callmanager ccmadmin to be sure these changes took effect." refType="reference">formattedMessage</Properties>
    </node>
    <node type="Variable" id="632471858221825858" name="lineId2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">lineId2</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>