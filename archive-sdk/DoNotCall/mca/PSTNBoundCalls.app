<Application name="PSTNBoundCalls" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="PSTNBoundCalls">
    <outline>
      <treenode type="evh" id="632458222965860077" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632458222965860074" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632458222965860073" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632458279714143059" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632458279714143056" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632458279714143055" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632796826816036825" actid="632458279714143065" />
          <ref id="632796826816036832" actid="632778331677036354" />
          <ref id="632796826816036838" actid="632778331677036364" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632458279714143064" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632458279714143061" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632458279714143060" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632796826816036826" actid="632458279714143065" />
          <ref id="632796826816036833" actid="632778331677036354" />
          <ref id="632796826816036839" actid="632778331677036364" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_playToConnectionId" id="632796826816036786" vid="632458279714143053">
        <Properties type="String">g_playToConnectionId</Properties>
      </treenode>
      <treenode text="g_callId" id="632796826816036788" vid="632630930838250313">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_dbUser" id="632796826816036790" vid="632778331677036272">
        <Properties type="String" initWith="DbUsername">g_dbUser</Properties>
      </treenode>
      <treenode text="g_dbPass" id="632796826816036792" vid="632778331677036274">
        <Properties type="String" initWith="DbPassword">g_dbPass</Properties>
      </treenode>
      <treenode text="g_dbHost" id="632796826816036794" vid="632778331677036276">
        <Properties type="String" initWith="DbHost">g_dbHost</Properties>
      </treenode>
      <treenode text="g_dbLicense" id="632796826816036796" vid="632778331677036278">
        <Properties type="String" initWith="DbLicense">g_dbLicense</Properties>
      </treenode>
      <treenode text="g_dbRefkey" id="632796826816036798" vid="632778331677036280">
        <Properties type="String" defaultInitWith="NONE" initWith="DbRefKey">g_dbRefkey</Properties>
      </treenode>
      <treenode text="g_dbCurfew" id="632796826816036800" vid="632778331677036282">
        <Properties type="Bool" initWith="RespectCurfew">g_dbCurfew</Properties>
      </treenode>
      <treenode text="g_dbOverride" id="632796826816036802" vid="632778331677036284">
        <Properties type="String" initWith="OverrideCode">g_dbOverride</Properties>
      </treenode>
      <treenode text="g_prependedDigits" id="632796826816036804" vid="632778331677036350">
        <Properties type="String" defaultInitWith="NONE" initWith="PrependDigits">g_prependedDigits</Properties>
      </treenode>
      <treenode text="g_overridePrepend" id="632796826816036806" vid="632790606749459165">
        <Properties type="String" initWith="DTMFOverride">g_overridePrepend</Properties>
      </treenode>
      <treenode text="g_testFailure" id="632796826816036808" vid="632790606749459167">
        <Properties type="Bool" initWith="TestFailure">g_testFailure</Properties>
      </treenode>
      <treenode text="g_prependOnRedirect" id="632796826816036810" vid="632793087463410326">
        <Properties type="String" initWith="PrependOnRedirect">g_prependOnRedirect</Properties>
      </treenode>
      <treenode text="g_forceOutbound" id="632796826816036812" vid="632793087463410328">
        <Properties type="String" initWith="ForceOutbound">g_forceOutbound</Properties>
      </treenode>
      <treenode text="g_useOnlyWireless" id="632796826816036814" vid="632793087463411061">
        <Properties type="Bool" initWith="UseWirelessDNC">g_useOnlyWireless</Properties>
      </treenode>
      <treenode text="g_useMetreosDnc" id="632796826816036816" vid="632796580256765405">
        <Properties type="String" initWith="UseMetreosDNC">g_useMetreosDnc</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632458222965860076" treenode="632458222965860077" appnode="632458222965860074" handlerfor="632458222965860073">
    <node type="Start" id="632458222965860076" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="357">
      <linkto id="632778331677036352" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632458233013594451" text="We do not answer the call (or accept for that matter) &#xD;&#xA;until the call can be verified that it is not in the DNC list" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="273" />
    <node type="Action" id="632458233013594452" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="610" y="509">
      <linkto id="632458279714143065" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DisplayName" type="literal">Do Not Call</ap>
        <ap name="WaitForMedia" type="literal">Tx</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="ConnectionId">g_playToConnectionId</rd>
        <log condition="entry" on="true" level="Info" type="literal">Number found on the DoNotCall list</log>
      </Properties>
    </node>
    <node type="Comment" id="632458233013594454" text="Place DB action here." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="446" y="317" />
    <node type="Action" id="632458233013594459" name="Redirect" class="MaxActionNode" group="" path="Metreos.CallControl" x="397" y="640">
      <linkto id="632793087463410721" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="To" type="variable">to</ap>
        <log condition="entry" on="true" level="Info" type="literal">Redirecting call</log>
      </Properties>
    </node>
    <node type="Comment" id="632458233013594463" text="Temporary DNC-log action" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="412" y="665" />
    <node type="Action" id="632458233013594464" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="397" y="1073">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632458279714143065" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="559" y="611" mx="612" my="627">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632458279714143059" />
        <item text="OnPlay_Failed" treenode="632458279714143064" />
      </items>
      <linkto id="632458286687135567" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">error_permission.wav</ap>
        <ap name="ConnectionId" type="variable">g_playToConnectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632458286687135567" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="610" y="767">
      <linkto id="632458286687135571" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute()
{
	// TODO: add parameters with same name and type as variables
	// TODO: add function body
	return "";
}
</Properties>
    </node>
    <node type="Action" id="632458286687135571" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="610" y="863">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632778331677036223" name="CheckWithGryphon" class="MaxActionNode" group="" path="Metreos.Native.VendorDb" x="455" y="361">
      <linkto id="632778331677036353" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <linkto id="632778331677036363" type="Labeled" style="Bezier" ortho="true" label="Connectivity" />
      <linkto id="632458233013594459" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632793087463411063" type="Labeled" style="Bezier" ortho="true" label="DoNotCall" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_dbUser</ap>
        <ap name="Password" type="variable">g_dbPass</ap>
        <ap name="Host" type="variable">g_dbHost</ap>
        <ap name="PhoneNumber" type="variable">withoutPrependTo</ap>
        <ap name="License" type="variable">g_dbLicense</ap>
        <ap name="From" type="variable">from</ap>
        <ap name="Refkey" type="csharp">g_dbRefkey == "NONE" ? null : g_dbRefkey</ap>
        <ap name="Curfew" type="variable">g_dbCurfew</ap>
        <ap name="Override" type="csharp">0</ap>
        <rd field="Wireless">wireless</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"Checking the number " + withoutPrependTo</log>
      </Properties>
    </node>
    <node type="Action" id="632778331677036353" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="759" y="466">
      <linkto id="632778331677036354" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DisplayName" type="literal">Do Not Call (DB Error)</ap>
        <ap name="WaitForMedia" type="literal">Tx</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="ConnectionId">g_playToConnectionId</rd>
        <log condition="entry" on="true" level="Info" type="literal">General failure encountered in querying the Gryphon database</log>
      </Properties>
    </node>
    <node type="Action" id="632778331677036354" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="709" y="581" mx="762" my="597">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632458279714143059" />
        <item text="OnPlay_Failed" treenode="632458279714143064" />
      </items>
      <linkto id="632778331677036355" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">error_permission.wav</ap>
        <ap name="ConnectionId" type="variable">g_playToConnectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632778331677036355" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="759" y="724">
      <linkto id="632778331677036356" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute()
{
	// TODO: add parameters with same name and type as variables
	// TODO: add function body
	return "";
}
</Properties>
    </node>
    <node type="Action" id="632778331677036356" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="759" y="820">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632778331677036363" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="931" y="363">
      <linkto id="632778331677036364" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DisplayName" type="literal">Do Not Call (DB Down)</ap>
        <ap name="WaitForMedia" type="literal">Tx</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="ConnectionId">g_playToConnectionId</rd>
        <log condition="entry" on="true" level="Info" type="literal">Unable to connect to the Gryphon database</log>
      </Properties>
    </node>
    <node type="Action" id="632778331677036364" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="880" y="465" mx="933" my="481">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632458279714143059" />
        <item text="OnPlay_Failed" treenode="632458279714143064" />
      </items>
      <linkto id="632778331677036365" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">error_permission.wav</ap>
        <ap name="ConnectionId" type="variable">g_playToConnectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632778331677036365" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="931" y="621">
      <linkto id="632778331677036366" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute()
{
	// TODO: add parameters with same name and type as variables
	// TODO: add function body
	return "";
}
</Properties>
    </node>
    <node type="Action" id="632778331677036366" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="931" y="717">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632778331677036352" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="182" y="357">
      <linkto id="632790606749459164" type="Labeled" style="Bezier" ortho="true" label="testFailure" />
      <linkto id="632796580256765409" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632796580256765410" type="Labeled" style="Bezier" ortho="true" label="override" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="csharp">"DNC: To=" + to + " OriginalTo=" + originalTo</log>
public static string Execute(string g_overridePrepend, ref string to, string g_prependedDigits, LogWriter log, bool g_testFailure, ref string withoutPrependTo, string g_forceOutbound, string g_prependOnRedirect)
{
	if(g_testFailure)
	{
		return "testFailure";
	}

	if(to.StartsWith(g_overridePrepend))
	{
		log.Write(TraceLevel.Info, "Override requested by user");

		int removeAmount = g_overridePrepend.Length - 1;

		if(removeAmount &gt; 0)
		{
			to  = to.Substring(removeAmount);

			if(g_prependedDigits != "NONE" &amp;&amp; to.StartsWith(g_prependedDigits))
			{
				withoutPrependTo = to.Substring(g_prependedDigits.Length);
			}
			else if(g_prependedDigits != "NONE")
			{
				withoutPrependTo = to;
			log.Write(TraceLevel.Warning, "The application has been configured with the Prepended Digits={0}, but the incoming call to {1} does not have this prepend.  Unable to remove the prepend to check with the DNC database", g_prependedDigits, to);
			}
			else
			{
				withoutPrependTo = to;
			}
			
		}

		if(g_prependOnRedirect != "NONE")
		{
			to = g_prependOnRedirect + to;
		}

		if(g_forceOutbound != "NONE")
		{
			to = g_forceOutbound;	
		}


		return "override";
	} 

	if(g_prependedDigits != "NONE" &amp;&amp; to.StartsWith(g_prependedDigits))
	{
		withoutPrependTo = to.Substring(g_prependedDigits.Length);
	}
	else if(g_prependedDigits != "NONE")
	{
		withoutPrependTo = to;
		log.Write(TraceLevel.Warning, "The application has been configured with the Prepended Digits={0}, but the incoming call to {1} does not have this prepend.  Unable to remove the prepend to check with the DNC database", g_prependedDigits, to);
	}
	else
	{
		withoutPrependTo = to;
	}

	if(g_prependOnRedirect != "NONE")
	{
		to = g_prependOnRedirect + to;
	}

	if(g_forceOutbound != "NONE")
	{
		to = g_forceOutbound;	
	}

	return "default";
}
</Properties>
    </node>
    <node type="Action" id="632790606749459164" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="118" y="453">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="literal">Simulated error!  Aborting script!</log>
      </Properties>
    </node>
    <node type="Action" id="632790606749459327" name="CheckWithGryphon" class="MaxActionNode" group="" path="Metreos.Native.VendorDb" x="262" y="638">
      <linkto id="632458233013594459" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_dbUser</ap>
        <ap name="Password" type="variable">g_dbPass</ap>
        <ap name="Host" type="variable">g_dbHost</ap>
        <ap name="PhoneNumber" type="variable">withoutPrependTo</ap>
        <ap name="License" type="variable">g_dbLicense</ap>
        <ap name="From" type="variable">from</ap>
        <ap name="Refkey" type="csharp">g_dbRefkey == "NONE" ? null : g_dbRefkey</ap>
        <ap name="Curfew" type="variable">g_dbCurfew</ap>
        <ap name="Override" type="variable">g_dbOverride</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Override--marking the number " + withoutPrependTo</log>
      </Properties>
    </node>
    <node type="Action" id="632793087463410721" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="397" y="736">
      <linkto id="632793087463410722" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632793087463410971" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">from == "N/A" || from == String.Empty || from == null</ap>
      </Properties>
    </node>
    <node type="Action" id="632793087463410722" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="491" y="738">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Warning" type="literal">Unable to add latest call to Metreos state table</log>
      </Properties>
    </node>
    <node type="Action" id="632793087463410971" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="398" y="892">
      <linkto id="632458233013594464" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("INSERT INTO latestCall (fromNumber, toNumber) VALUES ('{0}', '{1}') ON DUPLICATE KEY UPDATE toNumber = '{1}'", from, withoutPrependTo)</ap>
        <ap name="Name" type="literal">DoNotCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632793087463411063" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="509" y="509">
      <linkto id="632458233013594459" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632458233013594452" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">wireless == 0 &amp;&amp; g_useOnlyWireless</ap>
        <log condition="true" on="true" level="Info" type="literal">Number found in the DNC Database, but configuration item UseOnlyWireless is set, and the number was not found on the Wireless DNC.  Redirecting...</log>
      </Properties>
    </node>
    <node type="Comment" id="632793087463411065" text="Use Only Wireless" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="452" y="464" />
    <node type="Action" id="632796580256765409" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="316" y="359">
      <linkto id="632778331677036223" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632796826816035839" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_useMetreosDnc</ap>
      </Properties>
    </node>
    <node type="Action" id="632796580256765410" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="260" y="447">
      <linkto id="632790606749459327" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632458233013594459" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_useMetreosDnc</ap>
      </Properties>
    </node>
    <node type="Action" id="632796826816035839" name="ExecuteScalar" class="MaxActionNode" group="" path="Metreos.Native.Database" x="464" y="191">
      <linkto id="632796826816035846" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">"SELECT count(*) from metreosDnc where toNumber = '" + withoutPrependTo + "'"</ap>
        <ap name="Name" type="literal">DoNotCall</ap>
        <rd field="Scalar">metreosDncCount</rd>
      </Properties>
    </node>
    <node type="Label" id="632796826816035840" text="r" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="678.4707" y="116" />
    <node type="Label" id="632796826816035841" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="680.4707" y="260" />
    <node type="Label" id="632796826816035842" text="r" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="367.4707" y="468">
      <linkto id="632458233013594459" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632796826816035844" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="520.4707" y="606">
      <linkto id="632458233013594452" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632796826816035846" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="571.4707" y="191">
      <linkto id="632796826816035840" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632796826816035841" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">metreosDncCount == 0</ap>
      </Properties>
    </node>
    <node type="Variable" id="632458233013594456" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.CallControl.IncomingCall">to</Properties>
    </node>
    <node type="Variable" id="632458233013594457" name="originalTo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="OriginalTo" refType="reference" name="Metreos.CallControl.IncomingCall">originalTo</Properties>
    </node>
    <node type="Variable" id="632458233013594458" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632778331677036286" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" defaultInitWith="N/A" refType="reference" name="Metreos.CallControl.IncomingCall">from</Properties>
    </node>
    <node type="Variable" id="632790606749459246" name="withoutPrependTo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">withoutPrependTo</Properties>
    </node>
    <node type="Variable" id="632793087463410718" name="checkDbResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkDbResults</Properties>
    </node>
    <node type="Variable" id="632793087463411064" name="wireless" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">wireless</Properties>
    </node>
    <node type="Variable" id="632796826816035741" name="metreosDncCount" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">metreosDncCount</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632458279714143058" treenode="632458279714143059" appnode="632458279714143056" handlerfor="632458279714143055">
    <node type="Start" id="632458279714143058" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="360">
      <linkto id="632630930838250312" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632458286687135572" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="304" y="360">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632630930838250312" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="189" y="358">
      <linkto id="632458286687135572" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632458279714143063" treenode="632458279714143064" appnode="632458279714143061" handlerfor="632458279714143060">
    <node type="Start" id="632458279714143063" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="368">
      <linkto id="632630930838250316" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632630930838250315" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="266.352844" y="367">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632630930838250316" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="146.352859" y="367">
      <linkto id="632630930838250315" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>