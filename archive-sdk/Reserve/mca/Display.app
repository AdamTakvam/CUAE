<Application name="Display" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Display">
    <outline>
      <treenode type="evh" id="632831287183090943" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632831287183090940" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632831287183090939" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Display</ep>
        </Properties>
      </treenode>
      <treenode type="fun" id="632831349333413052" level="1" text="DestroyWorkers">
        <node type="function" name="DestroyWorkers" id="632831349333413049" path="Metreos.StockTools" />
        <calls>
          <ref actid="632831349333413048" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="dbUser" id="632859583510834351" vid="632831324065361548">
        <Properties type="String" initWith="dbUser">dbUser</Properties>
      </treenode>
      <treenode text="dbPass" id="632859583510834353" vid="632831324065361550">
        <Properties type="String" initWith="dbPass">dbPass</Properties>
      </treenode>
      <treenode text="dbType" id="632859583510834355" vid="632831324065361552">
        <Properties type="String" initWith="dbType">dbType</Properties>
      </treenode>
      <treenode text="dbHost" id="632859583510834357" vid="632831324065361554">
        <Properties type="String" initWith="dbHost">dbHost</Properties>
      </treenode>
      <treenode text="dbName" id="632859583510834359" vid="632831324065361556">
        <Properties type="String" initWith="dbName">dbName</Properties>
      </treenode>
      <treenode text="dbPort" id="632859583510834361" vid="632831324065361583">
        <Properties type="Int" initWith="dbPort">dbPort</Properties>
      </treenode>
      <treenode text="workers" id="632859583510834363" vid="632831324065361709">
        <Properties type="ArrayList">workers</Properties>
      </treenode>
      <treenode text="workerCount" id="632859583510834365" vid="632831349333412817">
        <Properties type="Int" initWith="workerCount">workerCount</Properties>
      </treenode>
      <treenode text="welcomeFormatter" id="632859583510834367" vid="632831482999343097">
        <Properties type="String" initWith="welcomePrompt">welcomeFormatter</Properties>
      </treenode>
      <treenode text="pin" id="632859583510834369" vid="632846669383434244">
        <Properties type="String" initWith="globalPin">pin</Properties>
      </treenode>
      <treenode text="minConfTime" id="632859583510834440" vid="632859583510834439">
        <Properties type="Int" initWith="minConf">minConfTime</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632831287183090942" treenode="632831287183090943" appnode="632831287183090940" handlerfor="632831287183090939">
    <node type="Loop" id="632831287183091055" name="Loop" text="loop (expr)" cx="758" cy="352" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="1248" y="88" mx="1627" my="264">
      <linkto id="632831349333412827" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632831349333413034" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">request.Items</Properties>
    </node>
    <node type="Loop" id="632831349333412768" name="Loop" text="loop (var)" cx="207" cy="197" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="832" y="168" mx="936" my="266">
      <linkto id="632831349333412819" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632831349333413032" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">workerCount</Properties>
    </node>
    <node type="Start" id="632831287183090942" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="272">
      <linkto id="632846669383434478" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632831287183091053" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="768" y="272">
      <linkto id="632831287183091058" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632831349333412768" port="1" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">request.Items != null &amp;&amp; request.Items.Length &gt; 0</ap>
      </Properties>
    </node>
    <node type="Comment" id="632831287183091054" text="Check that any items are in the request" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="608" y="168" />
    <node type="Label" id="632831287183091057" text="r" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="768" y="488" />
    <node type="Action" id="632831287183091058" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="768" y="376">
      <linkto id="632831287183091057" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="literal">A DisplayConfirm request received with no devices defined</ap>
        <ap name="LogLevel" type="literal">Warning</ap>
      </Properties>
    </node>
    <node type="Action" id="632831324065361530" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="120" y="272">
      <linkto id="632831324065361531" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632831324065361586" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="variable">dbType</ap>
        <ap name="DatabaseName" type="variable">dbName</ap>
        <ap name="Server" type="variable">dbHost</ap>
        <ap name="Port" type="variable">dbPort</ap>
        <ap name="Username" type="variable">dbUser</ap>
        <ap name="Password" type="variable">dbPass</ap>
        <ap name="Pooling" type="literal">true</ap>
        <rd field="DSN">dsn</rd>
        <log condition="entry" on="true" level="Info" type="csharp">request.ToString()</log>
      </Properties>
    </node>
    <node type="Action" id="632831324065361531" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="256" y="272">
      <linkto id="632831324065361587" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632831349333413042" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">Reserve</ap>
        <ap name="Type" type="variable">dbType</ap>
      </Properties>
    </node>
    <node type="Label" id="632831324065361586" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="120" y="384" />
    <node type="Label" id="632831324065361587" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="256" y="392" />
    <node type="Action" id="632831324065361589" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="568" y="272">
      <linkto id="632831349333413023" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632831324065361591" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("UPDATE Configuration SET CurrentDisplayGuid = '{0}'", routingGuid) </ap>
        <ap name="Name" type="literal">Reserve</ap>
      </Properties>
    </node>
    <node type="Label" id="632831324065361591" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="568" y="384" />
    <node type="Comment" id="632831324065361593" text="Because it can take a while for a SendExecute&#xD;&#xA;to complete, we create n worker-bee scripts&#xD;&#xA;to actually process the SendExecutes &#xD;&#xA;(created in the below loop) to create a properly&#xD;&#xA;timed polling mechanism for all phones.&#xD;&#xA;&#xD;&#xA;The second, larger loop will round-robin through &#xD;&#xA;all worker bees." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="768" y="32" />
    <node type="Action" id="632831349333412819" name="SendEvent" container="632831349333412768" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="895" y="263">
      <linkto id="632831349333412821" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="EventName" type="literal">Metreos.Events.Reserve.LaunchWorkerBee</ap>
        <rd field="DestinationGuid">newGuid</rd>
      </Properties>
    </node>
    <node type="Action" id="632831349333412821" name="CustomCode" container="632831349333412768" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="984" y="264">
      <linkto id="632831349333412768" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ArrayList workers, string newGuid)
{
	if(newGuid != null &amp;&amp; newGuid != String.Empty)
	{
		workers.Add(newGuid);
	}
	return "success";
}
</Properties>
    </node>
    <node type="Action" id="632831349333412827" name="CustomCode" container="632831287183091055" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1376" y="264">
      <linkto id="632831287183091055" port="4" type="Labeled" style="Bezier" ortho="true" label="skip" />
      <linkto id="632831349333413029" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(IEnumerator loopEnum, ref string title, ref string text, ref string user, ref string device, LogWriter log, ref int count, string welcomeFormatter, ref string recordId)
{
	count++;

	Metreos.Common.Reserve.DisplayItem item = loopEnum.Current as Metreos.Common.Reserve.DisplayItem;

	string first = item.First;
	string last = item.Last;

	device = item.Value;
	user = item.User;
	recordId = item.RecordId;

	title = "Welcome";
	text = String.Format(welcomeFormatter, first, last);

	if(device == null || device == String.Empty)
	{
		log.Write(TraceLevel.Error, "Devicename was not set in display confirm command.  Skipping...");
		return "skip";
	}

	if(user == null || user == String.Empty)
	{
		log.Write(TraceLevel.Error, "UserID was not set in display confirm command.  Skipping...");
		return "skip";
	}

	if(recordId == null || recordId == String.Empty)
	{
		log.Write(TraceLevel.Error, "RecordId was not set in display confirm command.  Skipping...");
		return "skip";
	}

	return "success";
}
</Properties>
    </node>
    <node type="Action" id="632831349333413018" name="SendEvent" container="632831287183091055" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1888" y="264">
      <linkto id="632831615034700657" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Title" type="variable">title</ap>
        <ap name="Text" type="variable">text</ap>
        <ap name="User" type="variable">user</ap>
        <ap name="Host" type="variable">host</ap>
        <ap name="DeviceName" type="variable">device</ap>
        <ap name="Pin" type="variable">pin</ap>
        <ap name="AppendIp" type="variable">appendIp</ap>
        <ap name="RecordId" type="variable">recordId</ap>
        <ap name="EventName" type="literal">Metreos.Events.Reserve.Send</ap>
        <ap name="ToGuid" type="csharp">workers[count % workers.Count] as string</ap>
      </Properties>
    </node>
    <node type="Comment" id="632831349333413021" text="Define confirm text, title.&#xD;&#xA;Define devicename, and user.&#xD;&#xA;&#xD;&#xA;Check that device and user are good." container="632831287183091055" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1264" y="152" />
    <node type="Comment" id="632831349333413022" text="Check that the CurrentDisplayGuid&#xD;&#xA; is still set to this script" container="632831287183091055" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1488" y="200" />
    <node type="Action" id="632831349333413023" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="688" y="272">
      <linkto id="632831287183091053" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632831349333413070" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="literal">DELETE FROM SkipDisplayDevices</ap>
        <ap name="Name" type="literal">Reserve</ap>
      </Properties>
    </node>
    <node type="Label" id="632831349333413025" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="688" y="520" />
    <node type="Action" id="632831349333413027" name="If" container="632831287183091055" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1584" y="264">
      <linkto id="632831349333413031" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632831349333413062" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">guidFromDB == routingGuid</ap>
        <log condition="false" on="true" level="Error" type="literal">Another DisplayConfirmation request has been received by the Application Server. This script is aborting...</log>
      </Properties>
    </node>
    <node type="Action" id="632831349333413029" name="ExecuteScalar" container="632831287183091055" class="MaxActionNode" group="" path="Metreos.Native.Database" x="1496" y="264">
      <linkto id="632831349333413027" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="literal">SELECT currentdisplayguid FROM Configuration</ap>
        <ap name="Name" type="literal">Reserve</ap>
        <rd field="Scalar">guidFromDB</rd>
      </Properties>
    </node>
    <node type="Action" id="632831349333413031" name="ExecuteQuery" container="632831287183091055" class="MaxActionNode" group="" path="Metreos.Native.Database" x="1720" y="264">
      <linkto id="632831349333413064" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">"SELECT Devicename FROM SkipDisplayDevices WHERE Devicename = '" + device + "'" </ap>
        <ap name="Name" type="literal">Reserve</ap>
      </Properties>
    </node>
    <node type="Action" id="632831349333413032" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1137" y="264">
      <linkto id="632831287183091055" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">causedDrop ? "DROPPED" : "OK"</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632831349333413034" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="2117" y="264">
      <linkto id="632831349333413048" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="literal">UPDATE Configuration set CurrentDisplayGuid = 'NONE'</ap>
        <ap name="Name" type="literal">Reserve</ap>
      </Properties>
    </node>
    <node type="Label" id="632831349333413036" text="r" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1136" y="144">
      <linkto id="632831349333413032" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632831349333413037" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1056" y="424">
      <linkto id="632831349333413041" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">404</ap>
        <ap name="body" type="literal">ERROR</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Label" id="632831349333413039" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="960" y="424">
      <linkto id="632831349333413037" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632831349333413041" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1160" y="424">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632831349333413042" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="368" y="272">
      <linkto id="632831349333413043" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632831349333413044" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="literal">SELECT CurrentDisplayGuid FROM Configuration WHERE CurrentDisplayGuid = 'NONE'</ap>
        <ap name="Name" type="literal">Reserve</ap>
        <rd field="ResultSet">checkDropResults</rd>
      </Properties>
    </node>
    <node type="Action" id="632831349333413043" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="472" y="272">
      <linkto id="632831324065361589" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">checkDropResults == null || checkDropResults.Rows.Count == 0</ap>
        <ap name="Value2" type="csharp">( (request.PollMinutes * 60000) - (request.PollMinutes * 60000 / 5) ) / (request.Items.Length == 0 ? 1 : request.Items.Length)</ap>
        <rd field="ResultData">causedDrop</rd>
        <rd field="ResultData2">sleepAmount</rd>
      </Properties>
    </node>
    <node type="Label" id="632831349333413044" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="368" y="392" />
    <node type="Comment" id="632831349333413047" text="Check to see if the current GUID is 'NONE',&#xD;&#xA;which is used to indicate that there is no currently&#xD;&#xA;executing DisplayConfirm Script" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="226" y="187" />
    <node type="Action" id="632831349333413048" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="2205" y="248" mx="2252" my="264">
      <items count="1">
        <item text="DestroyWorkers" />
      </items>
      <linkto id="632831349333413059" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">DestroyWorkers</ap>
      </Properties>
    </node>
    <node type="Action" id="632831349333413059" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2373" y="264">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632831349333413060" text="Set CurrentDisplayGuid to NONE&#xD;&#xA;to indicate that there is no DisplayConfirm&#xD;&#xA;script executing at the moment." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1856" y="456" />
    <node type="Label" id="632831349333413061" text="q" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="2117" y="112">
      <linkto id="632831349333413034" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632831349333413062" text="q" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1584" y="32" />
    <node type="Comment" id="632831349333413063" text="Check to see if this &#xD;&#xA;device is in the Skip list" container="632831287183091055" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1656" y="304" />
    <node type="Action" id="632831349333413064" name="If" container="632831287183091055" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1816" y="264">
      <linkto id="632831349333413018" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632831287183091055" port="4" type="Labeled" style="Bezier" ortho="true" label="skip" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">checkDeviceResults == null || checkDeviceResults.Rows.Count == 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632831349333413070" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="688" y="384">
      <linkto id="632831349333413025" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="literal">UPDATE Configuration set CurrentDisplayGuid = 'NONE'</ap>
        <ap name="Name" type="literal">Reserve</ap>
      </Properties>
    </node>
    <node type="Comment" id="632831349333413072" text="Mark CurrentDisplayGuid as&#xD;&#xA;NONE, essentially rolling back." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="560" y="424" />
    <node type="Action" id="632831615034700657" name="Sleep" container="632831287183091055" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1957" y="264">
      <linkto id="632831287183091055" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="csharp">count == request.Items.Length ? 0 : (sleepAmount &gt; minConfTime ? sleepAmount : minConfTime)</ap>
      </Properties>
    </node>
    <node type="Comment" id="632831615034700658" text="Reduce PollMinutes by 20%, &#xD;&#xA;and divide the remaining 80%&#xD;&#xA;of time by DeviceCount.  In a&#xD;&#xA;perfect system, this script would&#xD;&#xA;exit 80% * PollMinutes after&#xD;&#xA;the request came in." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="385" y="304" />
    <node type="Action" id="632846669383434478" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="72" y="178">
      <linkto id="632831324065361530" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string appendIp, string url)
{
	if(url != null)
	{
		int index = url.LastIndexOf('/');
		
		if(index &gt; 0 &amp;&amp; index != url.Length - 1)
		{
			appendIp = url.Substring(index + 1);
		}
	}

	return "";
}
</Properties>
    </node>
    <node type="Variable" id="632831287183091052" name="request" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Reserve.DisplayConfirmRequest" initWith="body" refType="reference">request</Properties>
    </node>
    <node type="Variable" id="632831324065361585" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632831324065361590" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632831349333412820" name="newGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">newGuid</Properties>
    </node>
    <node type="Variable" id="632831349333412822" name="title" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">title</Properties>
    </node>
    <node type="Variable" id="632831349333412823" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632831349333412824" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632831349333412825" name="device" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">device</Properties>
    </node>
    <node type="Variable" id="632831349333412826" name="user" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">user</Properties>
    </node>
    <node type="Variable" id="632831349333413019" name="count" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" defaultInitWith="-1" refType="reference">count</Properties>
    </node>
    <node type="Variable" id="632831349333413030" name="guidFromDB" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">guidFromDB</Properties>
    </node>
    <node type="Variable" id="632831349333413033" name="causedDrop" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">causedDrop</Properties>
    </node>
    <node type="Variable" id="632831349333413035" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632831349333413046" name="checkDropResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkDropResults</Properties>
    </node>
    <node type="Variable" id="632831349333413065" name="checkDeviceResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkDeviceResults</Properties>
    </node>
    <node type="Variable" id="632831615034700656" name="sleepAmount" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">sleepAmount</Properties>
    </node>
    <node type="Variable" id="632846669383434477" name="url" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="url" refType="reference" name="Metreos.Providers.Http.GotRequest">url</Properties>
    </node>
    <node type="Variable" id="632846669383434479" name="appendIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">appendIp</Properties>
    </node>
    <node type="Variable" id="632852893472969450" name="recordId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">recordId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="DestroyWorkers" startnode="632831349333413051" treenode="632831349333413052" appnode="632831349333413049" handlerfor="632831287183090939">
    <node type="Loop" id="632831349333413053" name="Loop" text="loop (var)" cx="435" cy="236" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="208" y="136" mx="426" my="254">
      <linkto id="632831349333413054" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632831349333413055" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="variable">workers</Properties>
    </node>
    <node type="Start" id="632831349333413051" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="256">
      <linkto id="632831349333413056" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632831349333413054" name="SendEvent" container="632831349333413053" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="408" y="248">
      <linkto id="632831349333413053" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="EventName" type="literal">Metreos.Events.Reserve.Exit</ap>
        <ap name="ToGuid" type="csharp">loopEnum.Current as string</ap>
      </Properties>
    </node>
    <node type="Action" id="632831349333413055" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="736" y="256">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632831349333413056" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="120" y="256">
      <linkto id="632831349333413053" port="1" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632831349333413057" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">workers.Count &gt; 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632831349333413057" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="120" y="376">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>