<Application name="LaunchIntercom" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="LaunchIntercom">
    <outline>
      <treenode type="evh" id="632347278629466188" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632347278629466185" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632347278629466184" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/LaunchIntercom</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632347457279352803" level="2" text="Metreos.Providers.Http.GotRequest: OnGotExitRequest">
        <node type="function" name="OnGotExitRequest" id="632347457279352800" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632347457279352799" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/IntercomExit</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632347578953311050" level="2" text="Metreos.Providers.Http.GotRequest: OnIntercomAddOk">
        <node type="function" name="OnIntercomAddOk" id="632347578953311047" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632347578953311046" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/IntercomAddOk</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632347578953311055" level="2" text="Metreos.Providers.Http.GotRequest: OnIntercomAddWorkerComplete">
        <node type="function" name="OnIntercomAddWorkerComplete" id="632347578953311052" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632347578953311051" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/IntercomAddWorkerComplete</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632348158383487514" level="2" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="632348158383487511" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="632348158383487510" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_dbUsername" id="632803709347053918" vid="632347358471072828">
        <Properties type="String" initWith="Username">g_dbUsername</Properties>
      </treenode>
      <treenode text="g_dbHost" id="632803709347053920" vid="632347358471072830">
        <Properties type="String" initWith="Server">g_dbHost</Properties>
      </treenode>
      <treenode text="g_dbPort" id="632803709347053922" vid="632347358471072832">
        <Properties type="UInt" initWith="Port">g_dbPort</Properties>
      </treenode>
      <treenode text="g_intercomGroupMembers" id="632803709347053924" vid="632347457279352402">
        <Properties type="StringCollection">g_intercomGroupMembers</Properties>
      </treenode>
      <treenode text="g_currentIndex" id="632803709347053926" vid="632347457279352404">
        <Properties type="Int">g_currentIndex</Properties>
      </treenode>
      <treenode text="g_hostDeviceId" id="632803709347053928" vid="632347457279352523">
        <Properties type="String">g_hostDeviceId</Properties>
      </treenode>
      <treenode text="g_confId" id="632803709347053930" vid="632347457279352536">
        <Properties type="Int">g_confId</Properties>
      </treenode>
      <treenode text="g_hostConxId" id="632803709347053932" vid="632347457279352538">
        <Properties type="Int">g_hostConxId</Properties>
      </treenode>
      <treenode text="g_activeIntercomMembers" id="632803709347053934" vid="632348158383487541">
        <Properties type="StringCollection">g_activeIntercomMembers</Properties>
      </treenode>
      <treenode text="g_talkbackEnabled" id="632803709347053936" vid="632348158383488156">
        <Properties type="Bool">g_talkbackEnabled</Properties>
      </treenode>
      <treenode text="g_intercomName" id="632803709347053938" vid="632348158383488567">
        <Properties type="String">g_intercomName</Properties>
      </treenode>
      <treenode text="g_dbName" id="632803709347053940" vid="632348158383489155">
        <Properties type="String" defaultInitWith="application_suite">g_dbName</Properties>
      </treenode>
      <treenode text="g_dbPassword" id="632803709347053942" vid="632348158383489358">
        <Properties type="String" initWith="Password">g_dbPassword</Properties>
      </treenode>
      <treenode text="g_ccmDeviceUsername" id="632803709347053944" vid="632348158383489575">
        <Properties type="String" initWith="CCM_Device_Username">g_ccmDeviceUsername</Properties>
      </treenode>
      <treenode text="g_ccmDevicePassword" id="632803709347053946" vid="632348158383489577">
        <Properties type="String" initWith="CCM_Device_Password">g_ccmDevicePassword</Properties>
      </treenode>
      <treenode text="g_mmsId" id="632803709347053948" vid="632767255736440614">
        <Properties type="UInt">g_mmsId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632347278629466187" treenode="632347278629466188" appnode="632347278629466185" handlerfor="632347278629466184">
    <node type="Loop" id="632347578953310790" name="Loop" text="loop (expr)" cx="371.4707" cy="189" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="2389" y="89" mx="2575" my="184">
      <linkto id="632347578953311611" fromport="1" type="Basic" style="Vector" ortho="true" />
      <linkto id="632347419297637226" fromport="3" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties iteratorType="int" type="csharp">g_intercomGroupMembers.Count</Properties>
    </node>
    <node type="Start" id="632347278629466187" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="184">
      <linkto id="632347419297637219" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632347419297637219" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="144" y="184">
      <linkto id="632347419297637220" type="Labeled" style="Vector" ortho="true" label="true" />
      <linkto id="632347419297637223" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">queryParams["id"] == null</ap>
      </Properties>
    </node>
    <node type="Action" id="632347419297637220" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="144" y="320">
      <linkto id="632347419297637221" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">No intercom group ID specified.</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632347419297637221" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="144" y="440">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632347419297637223" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="288" y="184">
      <linkto id="632347419297637220" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632347436308898186" type="Labeled" style="Vector" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">System.Convert.ToUInt32(queryParams["id"])</ap>
      </Properties>
    </node>
    <node type="Comment" id="632347419297637224" text="queryParams[&quot;id&quot;] == null" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="136" />
    <node type="Comment" id="632347419297637225" text="System.Convert.ToUInt32(queryParams[&quot;id&quot;])" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="184" y="136" />
    <node type="Action" id="632347419297637226" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="2861" y="185">
      <linkto id="632347457279353096" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">textItem.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632347436308898184" name="GetIntercomGroupMembers" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="784" y="184">
      <linkto id="632347457279352407" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632567129480073553" type="Labeled" style="Vector" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="IntercomGroupsId" type="csharp">queryParams["id"]</ap>
        <ap name="ActiveUsersOnly" type="literal">true</ap>
        <rd field="UserIds">g_intercomGroupMembers</rd>
      </Properties>
    </node>
    <node type="Action" id="632347436308898186" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="376" y="184">
      <linkto id="632347436308898187" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">g_dbName</ap>
        <ap name="Server" type="variable">g_dbHost</ap>
        <ap name="Port" type="variable">g_dbPort</ap>
        <ap name="Username" type="variable">g_dbUsername</ap>
        <ap name="Password" type="variable">g_dbPassword</ap>
        <rd field="DSN">dbDsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632347436308898187" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="504" y="184">
      <linkto id="632347436308898189" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632348158383488566" type="Labeled" style="Vector" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dbDsn</ap>
        <ap name="Name" type="literal">ApplicationSuite</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Action" id="632347436308898189" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="504" y="320">
      <linkto id="632347436308898190" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Unable to open database.</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632347436308898190" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="504" y="440">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632347457279352407" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="784" y="320">
      <linkto id="632347457279352408" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">This intercom group has no members.</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632347457279352408" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="784" y="440">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632347457279352409" text="Error: No intercom group ID&#xD;&#xA;specified in the request." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="192" y="296" />
    <node type="Comment" id="632347457279352410" text="Error: Can not connect&#xD;&#xA;to the database." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="360" y="296" />
    <node type="Comment" id="632347457279352411" text="Error: No members in &#xD;&#xA;intercom group." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="616" y="296" />
    <node type="Action" id="632347457279352525" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1165" y="185">
      <linkto id="632347457279352529" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">queryParams["hostDeviceId"]</ap>
        <rd field="ResultData">g_hostDeviceId</rd>
      </Properties>
    </node>
    <node type="Action" id="632347457279352529" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="1253" y="185">
      <linkto id="632347457279352530" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632347457279352550" type="Labeled" style="Vector" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">queryParams["hostDeviceId"]</ap>
        <ap name="Status" type="literal">2</ap>
        <rd field="ResultData">dlxResult</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"Looking up device: " + queryParams["device"]</log>
      </Properties>
    </node>
    <node type="Action" id="632347457279352530" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1253" y="321">
      <linkto id="632347457279352531" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Unable to locate host device.</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632347457279352531" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1253" y="441">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632347457279352532" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1989" y="185">
      <linkto id="632347457279352533" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"RTPRx:" + mmsIpAddress + ":" + phoneReceivePort.ToString()</ap>
        <ap name="URL2" type="csharp">"RTPTx:" + mmsIpAddress + ":" + mmsPort.ToString()</ap>
        <rd field="ResultData">executeItem</rd>
      </Properties>
    </node>
    <node type="Action" id="632347457279352533" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="2125" y="185">
      <linkto id="632473842452026713" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632473842452026714" type="Labeled" style="Vector" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="variable">executeItem</ap>
        <ap name="URL" type="csharp">dlxResult.Rows[0]["IP"]</ap>
        <ap name="Username" type="variable">g_ccmDeviceUsername</ap>
        <ap name="Password" type="variable">g_ccmDevicePassword</ap>
      </Properties>
    </node>
    <node type="Comment" id="632347457279352542" text="Create a connection on the media server&#xD;&#xA;before we send the HTTP respones to&#xD;&#xA;make sure we have resources." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1541" y="105" />
    <node type="Action" id="632347457279352543" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1565" y="321">
      <linkto id="632347457279352544" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Intercom failed to start because there are no media resources available.</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632347457279352544" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1565" y="441">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632347457279352545" text="Error: No media&#xD;&#xA;resources" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1453" y="297" />
    <node type="Comment" id="632347457279352546" text="Error: Host not&#xD;&#xA;found in DLX" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1141" y="297" />
    <node type="Comment" id="632347457279352549" text="Error: The host sent back&#xD;&#xA;an error in response to our&#xD;&#xA;streaming URIs." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1949" y="265" />
    <node type="Action" id="632347457279352550" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1365" y="185">
      <linkto id="632347457279352530" type="Labeled" style="Vector" ortho="true" label="true" />
      <linkto id="632347457279353959" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">dlxResult.Rows.Count &lt;= 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632347457279352673" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1717" y="185">
      <linkto id="632347457279352674" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Metreos Intercom</ap>
        <ap name="Prompt" type="literal">Press 'Exit' to quit.</ap>
        <ap name="Text" type="csharp">"\n" +
"Group: " + g_intercomName + "\n" +
"Count: " + g_intercomGroupMembers.Count.ToString() + " members"</ap>
        <rd field="ResultData">textItem</rd>
      </Properties>
    </node>
    <node type="Action" id="632347457279352674" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1853" y="185">
      <linkto id="632347457279352532" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">"http://" + appServerIpAddress + ":8000/IntercomExit?host=true&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">textItem</rd>
      </Properties>
    </node>
    <node type="Action" id="632347457279353096" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2989" y="185">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632347457279353959" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1461" y="185">
      <linkto id="632473842452026711" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref ushort phoneReceivePort)
	{
		Random r = new Random();
		phoneReceivePort = (ushort)r.Next(20000,30000);
		
		if(phoneReceivePort % 2 != 0) phoneReceivePort++;

		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Comment" id="632347457279353965" text="Error: No conference available." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="2197" y="265" />
    <node type="Label" id="632347457279354121" text="M" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="2125" y="401" />
    <node type="Label" id="632347457279354122" text="M" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1669" y="321">
      <linkto id="632347457279352543" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Comment" id="632347457279354123" text="Build the execute and send it out&#xD;&#xA;before we send the HTTP response&#xD;&#xA;so we can be sure the phone is going&#xD;&#xA;to cooperate." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1957" y="97" />
    <node type="Action" id="632347578953310791" name="SendEvent" container="632347578953310790" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2597" y="177">
      <linkto id="632347578953310792" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="url" type="literal">/IntercomAddWorker</ap>
        <ap name="userId" type="csharp">g_intercomGroupMembers[loopIndex]</ap>
        <ap name="fromGuid" type="variable">routingGuid</ap>
        <ap name="appServerIpAddress" type="variable">appServerIpAddress</ap>
        <ap name="EventName" type="literal">Metreos.Providers.Http.GotRequest</ap>
      </Properties>
    </node>
    <node type="Action" id="632347578953310792" name="Assign" container="632347578953310790" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2685" y="177">
      <linkto id="632347578953310790" port="3" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">g_currentIndex + 1</ap>
        <rd field="ResultData">g_currentIndex</rd>
      </Properties>
    </node>
    <node type="Action" id="632347578953311611" name="Compare" container="632347578953310790" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="2469" y="177">
      <linkto id="632347578953310791" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632347578953310790" port="2" type="Labeled" style="Vector" ortho="true" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">queryParams["hostUserId"]</ap>
        <ap name="Value2" type="csharp">g_intercomGroupMembers[loopIndex]</ap>
      </Properties>
    </node>
    <node type="Action" id="632348158383488566" name="GetIntercomGroup" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="624" y="184">
      <linkto id="632347436308898184" type="Labeled" style="Vector" ortho="true" label="success" />
      <linkto id="632347457279352407" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="IntercomGroupsId" type="csharp">queryParams["id"]</ap>
        <rd field="Name">g_intercomName</rd>
        <rd field="IsTalkbackEnabled">g_talkbackEnabled</rd>
      </Properties>
    </node>
    <node type="Action" id="632473842452026711" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="1565" y="185">
      <linkto id="632347457279352673" type="Labeled" style="Vector" ortho="true" label="Success" />
      <linkto id="632347457279352543" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxIP" type="variable">phoneIpAddress</ap>
        <ap name="MediaTxPort" type="variable">phoneReceivePort</ap>
        <rd field="MmsId">g_mmsId</rd>
        <rd field="MediaRxIP">mmsIpAddress</rd>
        <rd field="MediaRxPort">mmsPort</rd>
        <rd field="ConnectionId">g_hostConxId</rd>
      </Properties>
    </node>
    <node type="Action" id="632473842452026713" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="2125" y="289">
      <linkto id="632347457279354121" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_hostConxId</ap>
      </Properties>
    </node>
    <node type="Action" id="632473842452026714" name="CreateConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="2269" y="185">
      <linkto id="632347578953310790" port="1" type="Labeled" style="Vector" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="MmsId" type="variable">g_mmsId</ap>
        <ap name="ConnectionId" type="variable">g_hostConxId</ap>
        <rd field="ConferenceId">g_confId</rd>
      </Properties>
    </node>
    <node type="Action" id="632567129480073553" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="977" y="186">
      <linkto id="632347457279352525" type="Labeled" style="Vector" ortho="true" label="validhost" />
      <linkto id="632567129480073554" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Http.QueryParamCollection queryParams, StringCollection g_intercomGroupMembers)
{
	string validHostCondition = "validhost";
	string invalidHostCondition = "default";

	string hostUserId = queryParams["hostUserId"];

	if(g_intercomGroupMembers.Contains(hostUserId))
	{
		return validHostCondition;
	}
	else
	{
		return invalidHostCondition;
	}
}
</Properties>
    </node>
    <node type="Action" id="632567129480073554" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="978" y="324">
      <linkto id="632567129480073555" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">This user is not a part of the selected group.</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632567129480073555" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="978" y="444">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632347279097939821" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632347358471072814" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParams</Properties>
    </node>
    <node type="Variable" id="632347358471072834" name="dbDsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dbDsn</Properties>
    </node>
    <node type="Variable" id="632347387592247051" name="appServerIpAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="hostname" refType="reference">appServerIpAddress</Properties>
    </node>
    <node type="Variable" id="632347419297637218" name="textItem" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">textItem</Properties>
    </node>
    <node type="Variable" id="632347457279352399" name="executeItem" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">executeItem</Properties>
    </node>
    <node type="Variable" id="632347457279352406" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632347457279352528" name="dlxResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">dlxResult</Properties>
    </node>
    <node type="Variable" id="632347457279352540" name="mmsIpAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">mmsIpAddress</Properties>
    </node>
    <node type="Variable" id="632347457279352541" name="mmsPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UShort" refType="reference">mmsPort</Properties>
    </node>
    <node type="Variable" id="632347457279353958" name="phoneReceivePort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UShort" refType="reference">phoneReceivePort</Properties>
    </node>
    <node type="Variable" id="632347457279353960" name="phoneIpAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteIpAddress" refType="reference">phoneIpAddress</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotExitRequest" startnode="632347457279352802" treenode="632347457279352803" appnode="632347457279352800" handlerfor="632347457279352799">
    <node type="Loop" id="632348158383487544" name="Loop" text="loop (expr)" cx="188.942017" cy="143" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="1104" y="96" mx="1198" my="168">
      <linkto id="632348158383487545" fromport="1" type="Basic" style="Vector" ortho="true" />
      <linkto id="632347457279352952" fromport="3" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties iteratorType="int" type="csharp">g_activeIntercomMembers.Count</Properties>
    </node>
    <node type="Start" id="632347457279352802" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="168">
      <linkto id="632347457279352806" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632347457279352806" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="192" y="168">
      <linkto id="632473842452026709" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632473842452026710" type="Labeled" style="Vector" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">queryParams["host"] == null</ap>
      </Properties>
    </node>
    <node type="Action" id="632347457279352808" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="192" y="368">
      <linkto id="632348158383487556" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Intercom closed.</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Comment" id="632347457279352944" text="Non-host requesting exit." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="248" />
    <node type="Action" id="632347457279352946" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="464" y="168">
      <linkto id="632347457279352947" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Intercom closed.</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632347457279352947" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="584" y="168">
      <linkto id="632347457279352949" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute()
	{
		System.Threading.Thread.Sleep(50);
		return "success";
	}
</Properties>
    </node>
    <node type="Action" id="632347457279352949" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="696" y="168">
      <linkto id="632347457279352951" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">RTPRx:Stop</ap>
        <ap name="URL2" type="literal">RTPTx:Stop</ap>
        <ap name="URL3" type="literal">Key:Services</ap>
        <rd field="ResultData">executeItem</rd>
      </Properties>
    </node>
    <node type="Action" id="632347457279352951" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="800" y="168">
      <linkto id="632348158383487561" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="variable">executeItem</ap>
        <ap name="URL" type="variable">phoneIpAddress</ap>
        <ap name="Username" type="literal">metreos</ap>
        <ap name="Password" type="literal">metreos</ap>
      </Properties>
    </node>
    <node type="Action" id="632347457279352952" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1400" y="168">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632347578953311895" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="192" y="744">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632348158383487545" name="SendExecute" container="632348158383487544" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1200" y="168">
      <linkto id="632348158383487544" port="3" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="variable">executeItem</ap>
        <ap name="URL" type="csharp">g_activeIntercomMembers[loopIndex]</ap>
        <ap name="Username" type="literal">metreos</ap>
        <ap name="Password" type="literal">metreos</ap>
      </Properties>
    </node>
    <node type="Action" id="632348158383487546" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1024" y="168">
      <linkto id="632348158383487544" port="1" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">RTPRx:Stop</ap>
        <ap name="URL2" type="literal">RTPTx:Stop</ap>
        <ap name="URL3" type="literal">Key:Services</ap>
        <rd field="ResultData">executeItem</rd>
      </Properties>
    </node>
    <node type="Comment" id="632348158383487548" text="Sleep for 50ms to&#xD;&#xA;give the IP phone &#xD;&#xA;a breather." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="536" y="88" />
    <node type="Comment" id="632348158383487549" text="Exit the host. We use&#xD;&#xA;Key:Services to close&#xD;&#xA;out the host because&#xD;&#xA;we don't want the IP&#xD;&#xA;phone to refetch anything." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="688" y="64" />
    <node type="Comment" id="632348158383487550" text="Clear the screen and stop&#xD;&#xA;streaming for each participant." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1024" y="48" />
    <node type="Comment" id="632348158383487551" text="Delete the conference&#xD;&#xA;so all individual&#xD;&#xA;connections are closed." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="264" y="88" />
    <node type="Action" id="632348158383487552" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="192" y="552">
      <linkto id="632348158383487554" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">RTPRx:Stop</ap>
        <ap name="URL2" type="literal">RTPTx:Stop</ap>
        <ap name="URL3" type="literal">Key:Services</ap>
        <rd field="ResultData">executeItem</rd>
      </Properties>
    </node>
    <node type="Action" id="632348158383487554" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="192" y="648">
      <linkto id="632347578953311895" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="variable">executeItem</ap>
        <ap name="URL" type="variable">phoneIpAddress</ap>
        <ap name="Username" type="literal">metreos</ap>
        <ap name="Password" type="literal">metreos</ap>
      </Properties>
    </node>
    <node type="Action" id="632348158383487556" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="192" y="456">
      <linkto id="632348158383487552" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(string phoneIpAddress, ref StringCollection g_activeIntercomMembers)
	{
		g_activeIntercomMembers.Remove(phoneIpAddress);
		System.Threading.Thread.Sleep(50);
		return "success";
	}
</Properties>
    </node>
    <node type="Action" id="632348158383487561" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="904" y="168">
      <linkto id="632348158383487546" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632348158383487562" type="Labeled" style="Vector" ortho="true" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="literal">0</ap>
        <ap name="Value2" type="csharp">g_activeIntercomMembers.Count.ToString()</ap>
      </Properties>
    </node>
    <node type="Action" id="632348158383487562" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="904" y="296">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632348158383487563" text="There are no active&#xD;&#xA;intercom members so&#xD;&#xA;just end the script." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="760" y="256" />
    <node type="Action" id="632473842452026709" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="328" y="168">
      <linkto id="632347457279352946" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConferenceId" type="variable">g_confId</ap>
      </Properties>
    </node>
    <node type="Action" id="632473842452026710" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="192" y="264">
      <linkto id="632347457279352808" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="csharp">queryParams["conxId"]</ap>
      </Properties>
    </node>
    <node type="Variable" id="632347457279352804" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632347457279352805" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParams</Properties>
    </node>
    <node type="Variable" id="632347457279352948" name="executeItem" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">executeItem</Properties>
    </node>
    <node type="Variable" id="632347457279352950" name="phoneIpAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteIpAddress" refType="reference">phoneIpAddress</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnIntercomAddOk" startnode="632347578953311049" treenode="632347578953311050" appnode="632347578953311047" handlerfor="632347578953311046">
    <node type="Start" id="632347578953311049" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="160">
      <linkto id="632348158383487520" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632347578953311191" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1320" y="160">
      <linkto id="632348158383487543" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">textItem.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632347578953311192" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1536" y="160">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632348158383487520" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="160" y="160">
      <linkto id="632473842452026707" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(
				ref ushort phoneReceivePort, 
				ref ushort mediaServerTransmitPort)
	{
		Random r = new Random();
		phoneReceivePort = (ushort)r.Next(20480, 32768);
		
		if(phoneReceivePort % 2 != 0) phoneReceivePort++;

		mediaServerTransmitPort = phoneReceivePort;

		while(mediaServerTransmitPort == phoneReceivePort)
		{
			mediaServerTransmitPort = (ushort)r.Next(20480, 32768);

			if(mediaServerTransmitPort % 2 != 0) mediaServerTransmitPort++;
		}
		
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632348158383487526" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="296" y="304">
      <linkto id="632348158383487528" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Error: No media available for intercom.</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632348158383487528" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="296" y="424">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632348158383487530" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="632" y="160">
      <linkto id="632348158383487531" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"RTPRx:" + mmsIpAddress + ":" + phoneReceivePort.ToString()</ap>
        <rd field="ResultData">executeItem</rd>
      </Properties>
    </node>
    <node type="Action" id="632348158383487531" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="752" y="160">
      <linkto id="632348158383487532" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632348158383487536" type="Labeled" style="Vector" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="variable">executeItem</ap>
        <ap name="URL" type="variable">phoneIpAddress</ap>
        <ap name="Username" type="literal">metreos</ap>
        <ap name="Password" type="literal">metreos</ap>
      </Properties>
    </node>
    <node type="Action" id="632348158383487532" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="752" y="296">
      <linkto id="632348158383487534" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Error: Can not stop streaming for intercom.</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632348158383487534" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="752" y="416">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632348158383487536" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="872" y="160">
      <linkto id="632348158383487537" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Metreos Intercom</ap>
        <ap name="Prompt" type="literal">Press 'Exit' to quit.</ap>
        <ap name="Text" type="csharp">"\n" +
"Group: " + g_intercomName + "\n" +
"Count: " + g_intercomGroupMembers.Count.ToString() + " members"</ap>
        <rd field="ResultData">textItem</rd>
      </Properties>
    </node>
    <node type="Action" id="632348158383487537" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="976" y="160">
      <linkto id="632348158383487966" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">"http://" + appServerIpAddress + ":8000/IntercomExit?conxId=" + conxId + "&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">textItem</rd>
      </Properties>
    </node>
    <node type="Action" id="632348158383487538" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1208" y="256">
      <linkto id="632347578953311191" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Reply</ap>
        <ap name="Position" type="literal">4</ap>
        <ap name="URL" type="csharp">"RTPTx:" + mmsIpAddress + ":" + mmsPort</ap>
        <rd field="ResultData">textItem</rd>
      </Properties>
    </node>
    <node type="Comment" id="632348158383487539" text="Error: No media available." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="128" y="296" />
    <node type="Comment" id="632348158383487540" text="Error: Can not tell the phone to&#xD;&#xA;start streaming media." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="776" y="272" />
    <node type="Action" id="632348158383487543" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1432" y="160">
      <linkto id="632347578953311192" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(string phoneIpAddress, ref StringCollection g_activeIntercomMembers)
	{
		g_activeIntercomMembers.Add(phoneIpAddress);
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632348158383487966" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1096" y="160">
      <linkto id="632347578953311191" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632348158383487538" type="Labeled" style="Vector" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_talkbackEnabled</ap>
      </Properties>
    </node>
    <node type="Comment" id="632348158383489152" text="g_talkbackEnabled == true" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1032" y="112" />
    <node type="Comment" id="632348158383489153" text="g_activeIntercomMembers.Add(phoneIpAddress)" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1304" y="112" />
    <node type="Comment" id="632348158383489154" text="Tell the participant phone&#xD;&#xA;to start receiving audio." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="632" y="96" />
    <node type="Comment" id="632348158383489814" text="Only add the 'Reply' softkey&#xD;&#xA;if the group is talkback enabled." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1128" y="296" />
    <node type="Action" id="632473842452026707" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="296" y="160">
      <linkto id="632348158383487526" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632473842452026708" type="Labeled" style="Vector" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxIP" type="variable">phoneIpAddress</ap>
        <ap name="MediaTxPort" type="variable">phoneReceivePort</ap>
        <ap name="MmsId" type="variable">g_mmsId</ap>
        <rd field="MediaRxIP">mmsIpAddress</rd>
        <rd field="MediaRxPort">mmsPort</rd>
        <rd field="ConnectionId">conxId</rd>
      </Properties>
    </node>
    <node type="Action" id="632473842452026708" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="464" y="160">
      <linkto id="632348158383487530" type="Labeled" style="Vector" ortho="true" label="Success" />
      <linkto id="632348158383487526" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">conxId</ap>
        <ap name="ConferenceId" type="variable">g_confId</ap>
      </Properties>
    </node>
    <node type="Variable" id="632347578953311189" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632348158383487521" name="phoneReceivePort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UShort" refType="reference">phoneReceivePort</Properties>
    </node>
    <node type="Variable" id="632348158383487522" name="phoneIpAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteIpAddress" refType="reference">phoneIpAddress</Properties>
    </node>
    <node type="Variable" id="632348158383487523" name="conxId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">conxId</Properties>
    </node>
    <node type="Variable" id="632348158383487524" name="mmsIpAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">mmsIpAddress</Properties>
    </node>
    <node type="Variable" id="632348158383487525" name="mmsPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">mmsPort</Properties>
    </node>
    <node type="Variable" id="632348158383487529" name="executeItem" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">executeItem</Properties>
    </node>
    <node type="Variable" id="632348158383487535" name="textItem" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">textItem</Properties>
    </node>
    <node type="Variable" id="632348158383487559" name="appServerIpAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="hostname" refType="reference">appServerIpAddress</Properties>
    </node>
    <node type="Variable" id="632348158383487560" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632351079949687861" name="mediaServerTransmitPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UShort" refType="reference">mediaServerTransmitPort</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnIntercomAddWorkerComplete" startnode="632347578953311054" treenode="632347578953311055" appnode="632347578953311052" handlerfor="632347578953311051">
    <node type="Start" id="632347578953311054" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="160">
      <linkto id="632347578953311188" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632347578953311188" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="184" y="160">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="false" level="Info" type="literal">IntercomAddWorkerComplete</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="632348158383487513" treenode="632348158383487514" appnode="632348158383487511" handlerfor="632348158383487510">
    <node type="Start" id="632348158383487513" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="224">
      <linkto id="632473842452026715" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632348158383487516" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="352" y="224">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632348158383487517" text="If the HTTP session expires we close out&#xD;&#xA;the conference.  Session expiration is&#xD;&#xA;typicall set to 20 minutes.  If you are&#xD;&#xA;intercoming for longer than 20 minutes&#xD;&#xA;you need to call the guy :)." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="264" y="48" />
    <node type="Action" id="632473842452026715" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="200" y="224">
      <linkto id="632348158383487516" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConferenceId" type="variable">g_confId</ap>
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>