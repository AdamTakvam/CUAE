<Application name="Update" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Update">
    <outline>
      <treenode type="evh" id="632925366559301587" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632925366559301584" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632925366559301583" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/updateLineGroup</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_ccmIP" id="633120676783125465" vid="632925366559301596">
        <Properties type="String" initWith="callManagerIP">g_ccmIP</Properties>
      </treenode>
      <treenode text="g_user" id="633120676783125467" vid="632925366559301598">
        <Properties type="String" initWith="ccmUsername">g_user</Properties>
      </treenode>
      <treenode text="g_pass" id="633120676783125469" vid="632925366559301600">
        <Properties type="String" initWith="ccmPassword">g_pass</Properties>
      </treenode>
      <treenode text="g_clear" id="633120676783125471" vid="632925366559301602">
        <Properties type="Bool" initWith="Clear">g_clear</Properties>
      </treenode>
      <treenode text="g_distAlgo" id="633120676783125473" vid="632925366559301604">
        <Properties type="String" initWith="DistributionAlgorithm">g_distAlgo</Properties>
      </treenode>
      <treenode text="g_specifyDistAlgo" id="633120676783125475" vid="632925366559301606">
        <Properties type="Bool" initWith="ModifyDistributionAlgorithm">g_specifyDistAlgo</Properties>
      </treenode>
      <treenode text="g_busyAlgo" id="633120676783125477" vid="632925366559301608">
        <Properties type="String" initWith="HuntAlgoBusy">g_busyAlgo</Properties>
      </treenode>
      <treenode text="g_specifyBusyAlgo" id="633120676783125479" vid="632925366559301610">
        <Properties type="Bool" initWith="ModifyHuntAlgoBusy">g_specifyBusyAlgo</Properties>
      </treenode>
      <treenode text="g_noAnswerAlgo" id="633120676783125481" vid="632925366559301612">
        <Properties type="String" initWith="HuntAlgoNoAnswer">g_noAnswerAlgo</Properties>
      </treenode>
      <treenode text="g_specifyNoAnswerAlgo" id="633120676783125483" vid="632925366559301614">
        <Properties type="Bool" initWith="ModifyHuntAlgoNoAnswer">g_specifyNoAnswerAlgo</Properties>
      </treenode>
      <treenode text="g_notAvailableAlgo" id="633120676783125485" vid="632925366559301616">
        <Properties type="String" initWith="HuntAlgoNotAvailable">g_notAvailableAlgo</Properties>
      </treenode>
      <treenode text="g_specifyNotAvailableAlgo" id="633120676783125487" vid="632925366559301618">
        <Properties type="Bool" initWith="ModifyHuntAlgoNotAvailable">g_specifyNotAvailableAlgo</Properties>
      </treenode>
      <treenode text="g_lineGroupName" id="633120676783125489" vid="632925366559301620">
        <Properties type="String" initWith="LineGroupName">g_lineGroupName</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632925366559301586" treenode="632925366559301587" appnode="632925366559301584" handlerfor="632925366559301583">
    <node type="Loop" id="632925366559301686" name="Loop" text="loop (expr)" cx="366.292236" cy="263" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="1696" y="152" mx="1879" my="284">
      <linkto id="632925366559301693" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632925366559301676" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">axlLineGroup.Response.@return.lineGroup.members</Properties>
    </node>
    <node type="Start" id="632925366559301586" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="176" y="440">
      <linkto id="632925366559301627" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632925366559301623" text="Try to pull a device name from the query string! (using 'device' as query praam name)&#xD;&#xA;&#xD;&#xA;If it's there, then we do a getPhone, and add the primary line to the 1st in the list" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="239" y="59" />
    <node type="Comment" id="632925366559301624" text="Check for clear flag.   If so, just go straight to clear" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="273" y="352" />
    <node type="Action" id="632925366559301625" name="ClearLineGroup" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap601" x="408" y="608">
      <linkto id="632925366559301628" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <rd field="LineGroupMembers">lineGroupMembers</rd>
      </Properties>
    </node>
    <node type="Action" id="632925366559301627" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="408" y="440">
      <linkto id="632925366559301625" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632925366559301650" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_clear</ap>
      </Properties>
    </node>
    <node type="Action" id="632925366559301628" name="UpdateLineGroup" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap601" x="408" y="784">
      <linkto id="632925366559301635" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <linkto id="632925366559301644" type="Labeled" style="Bezier" ortho="true" label="fault" />
      <linkto id="632925366559301646" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Members" type="variable">lineGroupMembers</ap>
        <ap name="LineGroupName" type="variable">g_lineGroupName</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIP</ap>
        <ap name="AdminUsername" type="variable">g_user</ap>
        <ap name="AdminPassword" type="variable">g_pass</ap>
        <rd field="FaultMessage">fault</rd>
        <rd field="FaultCode">code</rd>
      </Properties>
    </node>
    <node type="Action" id="632925366559301631" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="976" y="784">
      <linkto id="632925366559301637" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">General failure in AXL command</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Label" id="632925366559301632" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="864" y="784">
      <linkto id="632925366559301631" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632925366559301635" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="416" y="896" />
    <node type="Action" id="632925366559301637" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1096" y="784">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632925366559301638" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="976" y="688">
      <linkto id="632925366559301640" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"Fault failure in AXL command.  Fault: " + fault + ", Code: " + code</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Label" id="632925366559301639" text="f" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="864" y="688">
      <linkto id="632925366559301638" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632925366559301640" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1096" y="688">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632925366559301644" text="f" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="544" y="896" />
    <node type="Action" id="632925366559301646" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="592" y="784">
      <linkto id="632925366559301647" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">CLEAR OPERATION!   Check CCMAdmin for no lines in line group</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632925366559301647" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="712" y="784">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632925366559301650" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="584" y="440">
      <linkto id="632925366559301657" type="Labeled" style="Bezier" ortho="true" label="nodevice" />
      <linkto id="632925366559301658" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Http.QueryParamCollection query, ref string device)
{
	// check for device name presence in query string

	if(query["device"] == null || query["device"] == String.Empty)
	{
		return "nodevice";
	}
	else
	{
		device = "SEP00001111000F";//query["device"];
		return "success";
	}
}
</Properties>
    </node>
    <node type="Action" id="632925366559301651" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="960" y="608">
      <linkto id="632925366559301653" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"No device query parameter specified.  Please be sure to specify a phone device name with the 'device' query parameter\n\n?device=SEP000011112222"</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Label" id="632925366559301652" text="n" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="848" y="608">
      <linkto id="632925366559301651" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632925366559301653" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1080" y="608">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632925366559301657" text="n" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="584" y="560" />
    <node type="Action" id="632925366559301658" name="GetPhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap601" x="744" y="440">
      <linkto id="632925366559301661" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <linkto id="632925366559301662" type="Labeled" style="Bezier" ortho="true" label="fault" />
      <linkto id="632925366559301665" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="variable">device</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIP</ap>
        <ap name="AdminUsername" type="variable">g_user</ap>
        <ap name="AdminPassword" type="variable">g_pass</ap>
        <rd field="GetPhoneResponse">axlPhone</rd>
        <rd field="FaultMessage">fault</rd>
        <rd field="FaultCode">code</rd>
      </Properties>
    </node>
    <node type="Label" id="632925366559301661" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="728" y="536" />
    <node type="Label" id="632925366559301662" text="f" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="856" y="536" />
    <node type="Action" id="632925366559301665" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1080" y="440">
      <linkto id="632925366559301673" type="Labeled" style="Bezier" ortho="true" label="nolines" />
      <linkto id="632925366559301678" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.AxlSoap601.GetPhoneResponse axlPhone, ref string primaryLineUuid, LogWriter log)
{
	// check for at least one line!

	if(axlPhone != null &amp;&amp;
	   axlPhone.Response != null &amp;&amp; 
	   axlPhone.Response.@return != null &amp;&amp;
	   axlPhone.Response.@return.device != null &amp;&amp;
	   axlPhone.Response.@return.device.lines != null &amp;&amp;
	   axlPhone.Response.@return.device.lines.Items != null &amp;&amp;
         axlPhone.Response.@return.device.lines.Items.Length &gt; 0)
	{
		log.Write(TraceLevel.Info, "*****Here 1******");
		
		XLine line = (XLine)axlPhone.Response.@return.device.lines.Items[0];

		log.Write(TraceLevel.Info,axlPhone.Response.@return.device.lines.Items[0].GetType().ToString());

		if(line == null)
		{

			log.Write(TraceLevel.Info, "*****No Lines******");
			return "nolines";
		}
		else
		{
			log.Write(TraceLevel.Info, "***** Line is not NULL******");
		}

		log.Write(TraceLevel.Info, "*****Here 2******");

		primaryLineUuid = line.Item.uuid;

		log.Write(TraceLevel.Info, "*****Here 3******");

		log.Write(TraceLevel.Info, "Primary Line ID: " + primaryLineUuid);
		
		log.Write(TraceLevel.Info, "*****Here 4******");

		return "success";
	}

	return "nolines";
}
</Properties>
    </node>
    <node type="Action" id="632925366559301667" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="976" y="864">
      <linkto id="632925366559301669" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"No lines are configured on the specified device: " + device</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Label" id="632925366559301668" text="l" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="864" y="864">
      <linkto id="632925366559301667" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632925366559301669" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1096" y="864">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632925366559301673" text="l" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1080" y="552" />
    <node type="Action" id="632925366559301675" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1560" y="440">
      <linkto id="632925366559301686" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632925366559301676" type="Labeled" style="Bezier" ortho="true" label="nomembers" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.AxlSoap601.GetLineGroupResponse axlLineGroup)
{
	if(axlLineGroup.Response.@return.lineGroup.members == null ||
		axlLineGroup.Response.@return.lineGroup.members.Length == 0)
	{
		return "nomembers";
	}

	return "success";
}
</Properties>
    </node>
    <node type="Action" id="632925366559301676" name="UpdateLineGroup" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap601" x="2248" y="440">
      <linkto id="632925366559301694" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <linkto id="632925366559301695" type="Labeled" style="Bezier" ortho="true" label="fault" />
      <linkto id="632925366559301698" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Members" type="variable">lineGroupMembers</ap>
        <ap name="LineGroupName" type="variable">g_lineGroupName</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIP</ap>
        <ap name="AdminUsername" type="variable">g_user</ap>
        <ap name="AdminPassword" type="variable">g_pass</ap>
        <rd field="FaultMessage">fault</rd>
        <rd field="FaultCode">code</rd>
      </Properties>
    </node>
    <node type="Action" id="632925366559301678" name="GetLineGroup" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap601" x="1256" y="440">
      <linkto id="632925366559301680" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <linkto id="632925366559301681" type="Labeled" style="Bezier" ortho="true" label="fault" />
      <linkto id="632925366559301691" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">g_lineGroupName</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIP</ap>
        <ap name="AdminUsername" type="variable">g_user</ap>
        <ap name="AdminPassword" type="variable">g_pass</ap>
        <rd field="GetLineGroupResponse">axlLineGroup</rd>
        <rd field="FaultMessage">fault</rd>
        <rd field="FaultCode">code</rd>
      </Properties>
    </node>
    <node type="Label" id="632925366559301680" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1224" y="560" />
    <node type="Label" id="632925366559301681" text="f" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1352" y="552" />
    <node type="Action" id="632925366559301685" name="AddLineGroupItem" container="632925366559301686" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap601" x="1952" y="280">
      <linkto id="632925366559301686" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DirectoryNumberId" type="variable">lineUuid</ap>
        <ap name="LineSelectionOrder" type="csharp">count</ap>
        <rd field="LineGroupItem">lineGroupMembers</rd>
      </Properties>
    </node>
    <node type="Action" id="632925366559301691" name="AddLineGroupItem" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap601" x="1432" y="440">
      <linkto id="632925366559301675" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DirectoryNumberId" type="csharp">primaryLineUuid</ap>
        <ap name="LineSelectionOrder" type="csharp">1</ap>
        <rd field="LineGroupItem">lineGroupMembers</rd>
      </Properties>
    </node>
    <node type="Action" id="632925366559301693" name="CustomCode" container="632925366559301686" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1800" y="280">
      <linkto id="632925366559301685" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref int count, ref string lineUuid, IEnumerator loopEnum, LogWriter log)
{
	count++;

	XLineGroupMember lineMember = loopEnum.Current as XLineGroupMember;
	XNPDirectoryNumber dn = lineMember.Item as XNPDirectoryNumber;
	lineUuid = dn.uuid;

	// Observed this bug in returned ID... no { } 
	if(!lineUuid.StartsWith("{"))
	{
		lineUuid = "{" + lineUuid;
	}

	if(!lineUuid.EndsWith("}"))
	{
		lineUuid = lineUuid + "}";
	}

	log.Write(TraceLevel.Info, "Adding line ID to line group: " + lineUuid);
	return "success";
}
</Properties>
    </node>
    <node type="Label" id="632925366559301694" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="2248" y="584" />
    <node type="Label" id="632925366559301695" text="f" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="2408" y="584" />
    <node type="Action" id="632925366559301698" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="2424" y="440">
      <linkto id="632925366559301699" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"ADD OPERATION:  Check ccmadmin for changes"</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632925366559301699" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2544" y="440">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632925366559301703" text="This script does not check to see if the same ID is being added to the same line group,&#xD;&#xA;which would be possible if the phone already belonged to the group.  &#xD;&#xA;&#xD;&#xA;It would be straightforward to modify the loop to check that the lineUuid variable is not&#xD;&#xA;equal to the primaryLineUuid (do a case-insenstive compare, and be sure to 'fix' the returned Uuids before comparing with { })&#xD;&#xA;&#xD;&#xA;But, since this doesn't check, a very possible fault error is:&#xD;&#xA;&#xD;&#xA;&lt;axl:Error xmlns:axl=&quot;http://www.cisco.com/AXL/1.0&quot;&gt;&lt;axl:code&gt;-239&lt;/axl:code&gt;&lt;ax&#xD;&#xA;l:message&gt;Could not insert new row - duplicate value in a UNIQUE INDEX column.&lt;/&#xD;&#xA;axl:message&gt;&lt;request&gt;updateLineGroup&lt;/request&gt;&lt;/axl:Error&gt;&#xD;&#xA;&#xD;&#xA;&#xD;&#xA;Fault: -239Could not insert new row - duplicate value in a UNIQUE INDEX column.updateLineGroup&#xD;&#xA;Code: 0&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1552" y="601" />
    <node type="Variable" id="632925366559301622" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632925366559301626" name="lineGroupMembers" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap601.LineGroupMembers" refType="reference">lineGroupMembers</Properties>
    </node>
    <node type="Variable" id="632925366559301629" name="fault" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">fault</Properties>
    </node>
    <node type="Variable" id="632925366559301630" name="code" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">code</Properties>
    </node>
    <node type="Variable" id="632925366559301633" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632925366559301659" name="device" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">device</Properties>
    </node>
    <node type="Variable" id="632925366559301660" name="axlPhone" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap601.GetPhoneResponse" refType="reference">axlPhone</Properties>
    </node>
    <node type="Variable" id="632925366559301666" name="primaryLineUuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">primaryLineUuid</Properties>
    </node>
    <node type="Variable" id="632925366559301679" name="axlLineGroup" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap601.GetLineGroupResponse" refType="reference">axlLineGroup</Properties>
    </node>
    <node type="Variable" id="632925366559301687" name="count" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" defaultInitWith="1" refType="reference">count</Properties>
    </node>
    <node type="Variable" id="632925366559301702" name="lineUuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">lineUuid</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>