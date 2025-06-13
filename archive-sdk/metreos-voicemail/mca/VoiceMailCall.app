<Application name="VoiceMailCall" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="VoiceMailCall">
    <outline>
      <treenode type="evh" id="632488277308008921" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632488277308008918" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632488277308008917" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632488299439989524" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632488299439989521" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632488299439989520" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632720674723867414" actid="632488299439989530" />
          <ref id="632720674723867455" actid="632488299439989640" />
          <ref id="632720674723867488" actid="632488299439989694" />
          <ref id="632720674723867491" actid="632488299439989697" />
          <ref id="632720674723867498" actid="632488299439989708" />
          <ref id="632720674723867501" actid="632488299439989711" />
          <ref id="632720674723867509" actid="632488299439989723" />
          <ref id="632720674723867512" actid="632488299439989726" />
          <ref id="632720674723867515" actid="632488299439989729" />
          <ref id="632720674723867521" actid="632610860136605981" />
          <ref id="632720674723867539" actid="632488299439989756" />
          <ref id="632720674723867544" actid="632488299439989761" />
          <ref id="632720674723867565" actid="632488299439989774" />
          <ref id="632720674723867582" actid="632488299439989667" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632488299439989529" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632488299439989526" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632488299439989525" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632720674723867415" actid="632488299439989530" />
          <ref id="632720674723867456" actid="632488299439989640" />
          <ref id="632720674723867489" actid="632488299439989694" />
          <ref id="632720674723867492" actid="632488299439989697" />
          <ref id="632720674723867499" actid="632488299439989708" />
          <ref id="632720674723867502" actid="632488299439989711" />
          <ref id="632720674723867510" actid="632488299439989723" />
          <ref id="632720674723867513" actid="632488299439989726" />
          <ref id="632720674723867516" actid="632488299439989729" />
          <ref id="632720674723867522" actid="632610860136605981" />
          <ref id="632720674723867540" actid="632488299439989756" />
          <ref id="632720674723867545" actid="632488299439989761" />
          <ref id="632720674723867566" actid="632488299439989774" />
          <ref id="632720674723867583" actid="632488299439989667" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632488299439989547" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632488299439989544" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632488299439989543" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632488299439989575" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632488299439989572" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632488299439989571" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632720674723867435" actid="632488299439989594" />
          <ref id="632720674723867446" actid="632488299439989626" />
          <ref id="632720674723867458" actid="632488299439989643" />
          <ref id="632720674723867554" actid="632610860136605978" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632488299439989580" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632488299439989577" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632488299439989576" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632720674723867436" actid="632488299439989594" />
          <ref id="632720674723867447" actid="632488299439989626" />
          <ref id="632720674723867459" actid="632488299439989643" />
          <ref id="632720674723867555" actid="632610860136605978" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632488299439989615" level="2" text="Metreos.MediaControl.Record_Complete: OnRecord_Complete">
        <node type="function" name="OnRecord_Complete" id="632488299439989612" path="Metreos.StockTools" />
        <node type="event" name="Record_Complete" id="632488299439989611" path="Metreos.MediaControl.Record_Complete" />
        <references>
          <ref id="632720674723867441" actid="632488299439989621" />
          <ref id="632720674723867579" actid="632488299439989664" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632488299439989620" level="2" text="Metreos.MediaControl.Record_Failed: OnRecord_Failed">
        <node type="function" name="OnRecord_Failed" id="632488299439989617" path="Metreos.StockTools" />
        <node type="event" name="Record_Failed" id="632488299439989616" path="Metreos.MediaControl.Record_Failed" />
        <references>
          <ref id="632720674723867442" actid="632488299439989621" />
          <ref id="632720674723867580" actid="632488299439989664" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632488299439989786" level="2" text="Metreos.CallControl.GotDigits: OnGotDigits">
        <node type="function" name="OnGotDigits" id="632488299439989783" path="Metreos.StockTools" />
        <node type="event" name="GotDigits" id="632488299439989782" path="Metreos.CallControl.GotDigits" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632488299439989564" level="1" text="RecordMessage">
        <node type="function" name="RecordMessage" id="632488299439989561" path="Metreos.StockTools" />
        <calls>
          <ref actid="632488299439989590" />
        </calls>
      </treenode>
      <treenode type="fun" id="632488299439989802" level="1" text="UserNotifyAction">
        <node type="function" name="UserNotifyAction" id="632488299439989799" path="Metreos.StockTools" />
        <calls>
          <ref actid="632488299439989798" />
          <ref actid="632488299439989803" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_connectionId_caller" id="632720674723867319" vid="632355263044138913">
        <Properties type="Int">g_connectionId_caller</Properties>
      </treenode>
      <treenode text="g_callRecordId" id="632720674723867321" vid="632355263044138915">
        <Properties type="UInt">g_callRecordId</Properties>
      </treenode>
      <treenode text="g_userId" id="632720674723867323" vid="632355263044139038">
        <Properties type="UInt">g_userId</Properties>
      </treenode>
      <treenode text="g_callId_caller" id="632720674723867325" vid="632355263044139042">
        <Properties type="String">g_callId_caller</Properties>
      </treenode>
      <treenode text="g_mode" id="632720674723867327" vid="632355263044139051">
        <Properties type="String">g_mode</Properties>
      </treenode>
      <treenode text="g_accountStatus" id="632720674723867329" vid="632358705072494026">
        <Properties type="String">g_accountStatus</Properties>
      </treenode>
      <treenode text="g_isFirstLogin" id="632720674723867331" vid="632358740489681482">
        <Properties type="Bool">g_isFirstLogin</Properties>
      </treenode>
      <treenode text="g_greetingFilename" id="632720674723867333" vid="632358740489681484">
        <Properties type="String">g_greetingFilename</Properties>
      </treenode>
      <treenode text="g_sortingOrder" id="632720674723867335" vid="632358740489681486">
        <Properties type="String">g_sortingOrder</Properties>
      </treenode>
      <treenode text="g_notifyMethod" id="632720674723867337" vid="632358740489681488">
        <Properties type="String">g_notifyMethod</Properties>
      </treenode>
      <treenode text="g_notificationAddress" id="632720674723867339" vid="632358740489681490">
        <Properties type="String">g_notificationAddress</Properties>
      </treenode>
      <treenode text="g_maxMessageLength" id="632720674723867341" vid="632358740489681492">
        <Properties type="Int">g_maxMessageLength</Properties>
      </treenode>
      <treenode text="g_maxNumberMessages" id="632720674723867343" vid="632358740489681494">
        <Properties type="UInt">g_maxNumberMessages</Properties>
      </treenode>
      <treenode text="g_maxStorageDays" id="632720674723867345" vid="632358740489681496">
        <Properties type="UInt">g_maxStorageDays</Properties>
      </treenode>
      <treenode text="g_describeEachMessage" id="632720674723867347" vid="632358740489681498">
        <Properties type="Bool">g_describeEachMessage</Properties>
      </treenode>
      <treenode text="g_voiceMailSettingsId" id="632720674723867349" vid="632358740489681500">
        <Properties type="UInt">g_voiceMailSettingsId</Properties>
      </treenode>
      <treenode text="g_userAccountStatus" id="632720674723867351" vid="632361225879369004">
        <Properties type="String">g_userAccountStatus</Properties>
      </treenode>
      <treenode text="g_authenticated" id="632720674723867353" vid="632361225879369022">
        <Properties type="Bool" defaultInitWith="false">g_authenticated</Properties>
      </treenode>
      <treenode text="g_introFilename" id="632720674723867355" vid="632361391698431610">
        <Properties type="String">g_introFilename</Properties>
      </treenode>
      <treenode text="g_recordingFilename" id="632720674723867357" vid="632368181503906657">
        <Properties type="String">g_recordingFilename</Properties>
      </treenode>
      <treenode text="g_isRecording" id="632720674723867359" vid="632375918692193165">
        <Properties type="Bool" defaultInitWith="false">g_isRecording</Properties>
      </treenode>
      <treenode text="g_mediaPath" id="632720674723867361" vid="632378854766942966">
        <Properties type="String" defaultInitWith="C:\Metreos\MediaServer\Audio\" initWith="MediaPath">g_mediaPath</Properties>
      </treenode>
      <treenode text="g_from" id="632720674723867363" vid="632378854766942968">
        <Properties type="String">g_from</Properties>
      </treenode>
      <treenode text="g_messageTimestamp" id="632720674723867365" vid="632388036011250465">
        <Properties type="DateTime">g_messageTimestamp</Properties>
      </treenode>
      <treenode text="g_MailServer" id="632720674723867367" vid="632388036011250968">
        <Properties type="String" initWith="MailServerHost">g_MailServer</Properties>
      </treenode>
      <treenode text="g_MailServerPassword" id="632720674723867369" vid="632388036011250970">
        <Properties type="String" initWith="MailServerPassword">g_MailServerPassword</Properties>
      </treenode>
      <treenode text="g_MailServerUsername" id="632720674723867371" vid="632388036011250972">
        <Properties type="String" initWith="MailServerUsername">g_MailServerUsername</Properties>
      </treenode>
      <treenode text="g_translationPatternMatch" id="632720674723867373" vid="632388233680941689">
        <Properties type="String" initWith="TranslationPattern">g_translationPatternMatch</Properties>
      </treenode>
      <treenode text="g_DbName" id="632720674723867375" vid="632388233680942293">
        <Properties type="String" initWith="DbName">g_DbName</Properties>
      </treenode>
      <treenode text="g_DbConnectionName" id="632720674723867377" vid="632388233680942295">
        <Properties type="String" initWith="DbConnectionName">g_DbConnectionName</Properties>
      </treenode>
      <treenode text="g_DbHost" id="632720674723867379" vid="632388233680942297">
        <Properties type="String" initWith="Server">g_DbHost</Properties>
      </treenode>
      <treenode text="g_DbUserName" id="632720674723867381" vid="632388233680942299">
        <Properties type="String" initWith="Username">g_DbUserName</Properties>
      </treenode>
      <treenode text="g_DbPassword" id="632720674723867383" vid="632388233680942301">
        <Properties type="String" initWith="Password">g_DbPassword</Properties>
      </treenode>
      <treenode text="g_DbPort" id="632720674723867385" vid="632388233680942303">
        <Properties type="String" initWith="Port">g_DbPort</Properties>
      </treenode>
      <treenode text="g_fromEmailAddress" id="632720674723867387" vid="632389047422032451">
        <Properties type="String" initWith="FromEmailAddress">g_fromEmailAddress</Properties>
      </treenode>
      <treenode text="g_accountCode" id="632720674723867389" vid="632406091895236801">
        <Properties type="UInt">g_accountCode</Properties>
      </treenode>
      <treenode text="g_to" id="632720674723867391" vid="632406091895236805">
        <Properties type="String">g_to</Properties>
      </treenode>
      <treenode text="g_tempGreetingFilename" id="632720674723867393" vid="632406091895236840">
        <Properties type="String">g_tempGreetingFilename</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632488277308008920" treenode="632488277308008921" appnode="632488277308008918" handlerfor="632488277308008917">
    <node type="Start" id="632488277308008920" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="304">
      <linkto id="632488277308009059" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632488277308008922" text="                     *** IMPORTANT ***&#xD;&#xA;The application currently expects the configured&#xD;&#xA;translation pattern to be of the form ###.+, &#xD;&#xA;meaning any number of prefix digts followed by &#xD;&#xA;.+, which in Regular Expression means any &#xD;&#xA;character, 1 or more times (representing the &#xD;&#xA;number actually dialed)&#xD;&#xA;&#xD;&#xA;This custom code will:&#xD;&#xA;Rip off the beginning translation pattern digits,&#xD;&#xA;assigning the truncated 'to' field to the 'to' &#xD;&#xA;variable, as well as the 'from' field and 'from' variable." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="589" />
    <node type="Action" id="632488277308009059" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="116.90625" y="304">
      <linkto id="632488277308009061" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(string g_translationPatternMatch, ref string to, ref string from, LogWriter log)
	{
	
		if ((from == null) || (from == string.Empty))
			from = "UNAVAILABLE";

      	string regexDeclaration = "regex:";

		int periodStart = g_translationPatternMatch.IndexOf('.');
	
		// Ripping off regex: declartion
		periodStart -= regexDeclaration.Length;

		if(periodStart &gt; -1 &amp;&amp; to.Length &gt; periodStart)
		{
			to = to.Substring(periodStart);
		}

		log.Write(TraceLevel.Verbose, "The 'to' field is: " + to);

		return IApp.VALUE_SUCCESS;
	}</Properties>
    </node>
    <node type="Action" id="632488277308009061" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="205.048828" y="305">
      <linkto id="632488277308009062" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">incomingCallId</ap>
        <ap name="Value2" type="variable">from</ap>
        <ap name="Value3" type="variable">to</ap>
        <rd field="ResultData">g_callId_caller</rd>
        <rd field="ResultData2">g_from</rd>
        <rd field="ResultData3">g_to</rd>
      </Properties>
    </node>
    <node type="Action" id="632488277308009062" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="299.046875" y="305">
      <linkto id="632488277308009063" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">g_DbName</ap>
        <ap name="Server" type="variable">g_DbHost</ap>
        <ap name="Port" type="variable">g_DbPort</ap>
        <ap name="Username" type="variable">g_DbUserName</ap>
        <ap name="Password" type="variable">g_DbPassword</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632488277308009063" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="411.048828" y="305">
      <linkto id="632488277308009206" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="variable">g_DbConnectionName</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Action" id="632488277308009205" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="807.2311" y="305">
      <linkto id="632488277308009217" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="OriginNumber" type="variable">from</ap>
        <ap name="DestinationNumber" type="variable">to</ap>
        <ap name="UserId" type="variable">g_userId</ap>
        <rd field="CallRecordsId">g_callRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632488277308009206" name="GetUserByPrimaryDN" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="549.2311" y="305">
      <linkto id="632488277308009207" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632488277308009208" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="DirectoryNumber" type="variable">to</ap>
        <rd field="UserId">g_userId</rd>
        <rd field="AccountCode">g_accountCode</rd>
        <rd field="UserStatus">g_userAccountStatus</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: Retrieving primary user for dn: " + to;</log>
      </Properties>
    </node>
    <node type="Action" id="632488277308009207" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="548.2311" y="424">
      <linkto id="632488277308009220" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="OriginNumber" type="variable">from</ap>
        <ap name="DestinationNumber" type="variable">to</ap>
        <rd field="CallRecordsId">g_callRecordId</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: writing CallRecordStart for failed call...</log>
      </Properties>
    </node>
    <node type="Action" id="632488277308009208" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="682.23114" y="305">
      <linkto id="632488277308009205" type="Labeled" style="Bezier" ortho="true" label="Ok" />
      <linkto id="632488277308009207" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">g_userAccountStatus</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: checking user account status</log>
      </Properties>
    </node>
    <node type="Action" id="632488277308009213" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="960.8977" y="545">
      <linkto id="632488277308009214" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.InternalError</ap>
      </Properties>
    </node>
    <node type="Action" id="632488277308009214" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="961.8977" y="662">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: Exiting application.</log>
      </Properties>
    </node>
    <node type="Action" id="632488277308009217" name="GetVoiceMailSettings" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="958.845642" y="305">
      <linkto id="632488277308009219" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632488277308009220" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
        <rd field="IsFirstLogin">g_isFirstLogin</rd>
        <rd field="VoiceMailSettingsId">g_voiceMailSettingsId</rd>
        <rd field="GreetingFilename">g_greetingFilename</rd>
        <rd field="AccountStatus">g_accountStatus</rd>
        <rd field="SortingOrder">g_sortingOrder</rd>
        <rd field="NotifyMethod">g_notifyMethod</rd>
        <rd field="NotificationAddress">g_notificationAddress</rd>
        <rd field="MaxMessageLength">g_maxMessageLength</rd>
        <rd field="MaxNumberMessages">g_maxNumberMessages</rd>
        <rd field="MaxStorageDays">g_maxStorageDays</rd>
        <rd field="DescribeEachMessage">g_describeEachMessage</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: Retrieving VoiceMail settings</log>
      </Properties>
    </node>
    <node type="Action" id="632488277308009219" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="1087.1123" y="306">
      <linkto id="632488277308009223" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632488299439989530" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">incomingCallId</ap>
        <rd field="ConnectionId">g_connectionId_caller</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: Answering call
</log>
      </Properties>
    </node>
    <node type="Action" id="632488277308009220" name="RejectCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="960.1124" y="427">
      <linkto id="632488277308009213" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">incomingCallId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: rejecting incoming call with callId: " + incomingCallId</log>
      </Properties>
    </node>
    <node type="Label" id="632488277308009222" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="843.904663" y="545">
      <linkto id="632488277308009213" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632488277308009223" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1090.26465" y="547">
      <linkto id="632488277308009214" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.InternalError</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989530" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1152.846" y="290" mx="1206" my="306">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632488299439989524" />
        <item text="OnPlay_Failed" treenode="632488299439989529" />
      </items>
      <linkto id="632488299439989541" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632488299439989542" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="TermCondMaxTime" type="literal">240000</ap>
        <ap name="TermCondDigitList" type="literal">1,*</ap>
        <ap name="Prompt1" type="variable">g_greetingFilename</ap>
        <ap name="Prompt2" type="literal">vm_recordingInstructions_b.wav</ap>
        <ap name="UserData" type="literal">greeting</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: playing greeting message to caller.</log>
      </Properties>
    </node>
    <node type="Label" id="632488299439989540" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1207.31665" y="567" />
    <node type="Action" id="632488299439989541" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="1205.31665" y="464">
      <linkto id="632488299439989540" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">incomingCallId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: hanging up incoming call with callId: " + incomingCallId
</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989542" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1319.41431" y="306">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632488277308009054" name="incomingCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">incomingCallId</Properties>
    </node>
    <node type="Variable" id="632488277308009055" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.CallControl.IncomingCall">to</Properties>
    </node>
    <node type="Variable" id="632488277308009056" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">from</Properties>
    </node>
    <node type="Variable" id="632488277308009057" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632488299439989523" treenode="632488299439989524" appnode="632488299439989521" handlerfor="632488299439989520">
    <node type="Start" id="632488299439989523" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="369">
      <linkto id="632488299439989584" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632488299439989584" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="323.674469" y="363">
      <linkto id="632488299439989585" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632488299439989594" type="Labeled" style="Bezier" ortho="true" label="enterPass" />
      <linkto id="632488299439989587" type="Labeled" style="Bezier" ortho="true" label="confirmGreeting" />
      <linkto id="632488299439989586" type="Labeled" style="Bezier" ortho="true" label="recordGreeting" />
      <linkto id="632488299439989588" type="Labeled" style="Bezier" ortho="true" label="greeting" />
      <linkto id="632488299439989589" type="Labeled" style="Bezier" ortho="true" label="greetingConfirmed" />
      <linkto id="632488299439989591" type="Labeled" style="Bezier" ortho="true" label="beep" />
      <linkto id="632488299439989608" type="Labeled" style="Bezier" ortho="true" label="exit" />
      <linkto id="632488299439989609" type="Labeled" style="Bezier" ortho="true" label="error" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989585" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="561.3135" y="361">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632488299439989586" text="R" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="33.08252" y="450.5" />
    <node type="Label" id="632488299439989587" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="101.08252" y="557.5" />
    <node type="Label" id="632488299439989588" text="G" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="119.21582" y="224.5" />
    <node type="Label" id="632488299439989589" text="Y" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="185.21582" y="33.5" />
    <node type="Action" id="632488299439989590" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="331.30957" y="110" mx="379" my="126">
      <items count="1">
        <item text="RecordMessage" />
      </items>
      <linkto id="632488299439989592" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">RecordMessage</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlay_Complete: invoking RecordMessage function</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989591" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="377.674438" y="240">
      <linkto id="632488299439989590" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989592" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="375.4707" y="37">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989593" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="325.4707" y="646">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632488299439989594" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="253" y="495" mx="327" my="511">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632488299439989575" />
        <item text="OnGatherDigits_Failed" treenode="632488299439989580" />
      </items>
      <linkto id="632488299439989593" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="TermCondMaxTime" type="literal">10000</ap>
        <ap name="UserData" type="literal">enterPass</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlay_Complete: obtaining pin via GatherDigits
</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989608" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="561" y="262">
      <linkto id="632488299439989585" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId_caller</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989609" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="563" y="469">
      <linkto id="632488299439989585" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId_caller</ap>
      </Properties>
    </node>
    <node type="Label" id="632488299439989610" text="R" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="104" y="833">
      <linkto id="632488299439989621" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632488299439989621" name="Record" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="181" y="820" mx="241" my="836">
      <items count="2">
        <item text="OnRecord_Complete" treenode="632488299439989615" />
        <item text="OnRecord_Failed" treenode="632488299439989620" />
      </items>
      <linkto id="632488299439989624" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Expires" type="literal">32767</ap>
        <ap name="CommandTimeout" type="literal">240000</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="TermCondMaxTime" type="literal">240000</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="TermCondSilence" type="literal">240000</ap>
        <ap name="TermCondNonSilence" type="literal">240000</ap>
        <ap name="AudioFileEncoding" type="literal">ulaw</ap>
        <ap name="AudioFileFormat" type="literal">wav</ap>
        <ap name="UserData" type="literal">recordGreeting</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlay_Complete: recording greeting message
</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989624" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="393" y="836">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632488299439989625" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="87" y="1005">
      <linkto id="632488299439989626" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632488299439989626" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="150" y="990" mx="224" my="1006">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632488299439989575" />
        <item text="OnGatherDigits_Failed" treenode="632488299439989580" />
      </items>
      <linkto id="632488299439989629" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigitList" type="literal">1,0</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="TermCondMaxTime" type="literal">30000</ap>
        <ap name="UserData" type="literal">confirmGreeting</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlay_Complete: Waiting for greeting confirmation
</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989629" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="373" y="1006">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632488299439989630" text="Y" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="89" y="1179.5">
      <linkto id="632488299439989631" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632488299439989631" name="UpdateVoiceMailGreeting" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="213.123047" y="1180.5">
      <linkto id="632488299439989633" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="VoiceMailSettingsId" type="variable">g_voiceMailSettingsId</ap>
        <ap name="GreetingFilename" type="variable">g_tempGreetingFilename</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlay_Complete: Updating greeting prompt filename, exiting...</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989633" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="350" y="1182.5">
      <linkto id="632488299439990425" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId_caller</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlay_Complete: hanging up caller
</log>
      </Properties>
    </node>
    <node type="Label" id="632488299439989636" text="G" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="712" y="757">
      <linkto id="632488299439989637" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632488299439989637" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="786.674438" y="757">
      <linkto id="632488299439989643" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <linkto id="632488299439989640" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989640" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="898" y="907" mx="951" my="923">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632488299439989524" />
        <item text="OnPlay_Failed" treenode="632488299439989529" />
      </items>
      <linkto id="632488299439990426" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="Prompt1" type="literal">vm_beep.wav</ap>
        <ap name="UserData" type="literal">beep</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989643" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="870" y="568" mx="944" my="584">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632488299439989575" />
        <item text="OnGatherDigits_Failed" treenode="632488299439989580" />
      </items>
      <linkto id="632488299439990426" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigitList" type="literal">1,*</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="UserData" type="literal">greeting</ap>
      </Properties>
    </node>
    <node type="Comment" id="632488299439989647" text="add error functionality&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="739.8887" y="310" />
    <node type="Action" id="632488299439990424" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="598" y="1184.5">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">OnPlay_Complete: VoiceMail settings updated, exiting script</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439990425" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="470" y="1183.5">
      <linkto id="632488299439990424" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.Normal</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439990426" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1063.83069" y="760">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632488299439989639" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Play_Complete">termCond</Properties>
    </node>
    <node type="Variable" id="632488299439989648" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632488299439989528" treenode="632488299439989529" appnode="632488299439989526" handlerfor="632488299439989525">
    <node type="Start" id="632488299439989528" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="320">
      <linkto id="632488299439989651" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632488299439989650" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="403.674469" y="317">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632488299439989651" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="146.674469" y="320">
      <linkto id="632488299439989650" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632488299439989655" type="Labeled" style="Bezier" label="error" />
      <linkto id="632488299439989655" type="Labeled" style="Bezier" label="exit" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Comment" id="632488299439989654" text="add error handling" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="153" y="588" />
    <node type="Action" id="632488299439989655" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="146" y="511">
      <linkto id="632488299439989650" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId_caller</ap>
      </Properties>
    </node>
    <node type="Variable" id="632488299439989649" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632488299439989546" treenode="632488299439989547" appnode="632488299439989544" handlerfor="632488299439989543">
    <node type="Start" id="632488299439989546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="332">
      <linkto id="632488299439989791" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632488299439989791" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="278" y="332">
      <linkto id="632488299439989795" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632488299439989793" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_isRecording</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Checking to see if we're currently recording.</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989793" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="276" y="189">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632488299439989794" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="566.0553" y="330">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Ending script</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989795" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="410.055359" y="330">
      <linkto id="632488299439989794" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.Normal</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Not recording. Writing NormalCallClearing CallRecordStop</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" startnode="632488299439989574" treenode="632488299439989575" appnode="632488299439989572" handlerfor="632488299439989571">
    <node type="Start" id="632488299439989574" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="349">
      <linkto id="632488299439989676" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632488299439989676" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="161.0065" y="347.5">
      <linkto id="632488299439989677" type="Labeled" style="Bezier" ortho="true" label="greeting" />
      <linkto id="632488299439989678" type="Labeled" style="Bezier" ortho="true" label="enterPass" />
      <linkto id="632488299439989679" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632488299439989680" type="Labeled" style="Bezier" ortho="true" label="confirmGreeting" />
      <linkto id="632610860136605975" type="Labeled" style="Bezier" ortho="true" label="recordGreeting" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnReceiveDigits_Complete: userData: " + userData + " Termination Condition: " + termCond + "  Received Digits: " + receivedDigits;</log>
      </Properties>
    </node>
    <node type="Label" id="632488299439989677" text="G" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="324.0065" y="131.5" />
    <node type="Label" id="632488299439989678" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="324.0065" y="450.5" />
    <node type="Action" id="632488299439989679" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="161.006531" y="471.5">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632488299439989680" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="158.0065" y="138.5" />
    <node type="Label" id="632488299439989686" text="G" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="453.21582" y="176">
      <linkto id="632610860136605985" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632488299439989687" text="Ask user for pin" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="464.686523" y="63" />
    <node type="Action" id="632488299439989688" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="689.6865" y="179">
      <linkto id="632488299439989694" type="Labeled" style="Bezier" ortho="true" label="*" />
      <linkto id="632488299439989697" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">receivedDigits</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989692" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="861.4707" y="314">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632488299439989694" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="810" y="164" mx="863" my="180">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632488299439989524" />
        <item text="OnPlay_Failed" treenode="632488299439989529" />
      </items>
      <linkto id="632488299439989692" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="Prompt1" type="literal">vm_pleaseEnterYourPassword.wav</ap>
        <ap name="Prompt2" type="literal">vm_pound_sign.wav</ap>
        <ap name="UserData" type="literal">enterPass</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnGatherDigits_Complete: Prompting user for password
</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989697" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="640" y="297" mx="693" my="313">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632488299439989524" />
        <item text="OnPlay_Failed" treenode="632488299439989529" />
      </items>
      <linkto id="632488299439989692" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="Prompt1" type="literal">vm_beep.wav</ap>
        <ap name="UserData" type="literal">beep</ap>
      </Properties>
    </node>
    <node type="Label" id="632488299439989700" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="480" y="550">
      <linkto id="632610860136605987" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632488299439989701" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="750.4707" y="552">
      <linkto id="632488299439989703" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <linkto id="632488299439989711" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Comment" id="632488299439989702" text="Validate pin" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="576.4707" y="504" />
    <node type="Action" id="632488299439989703" name="PhoneLogin" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="875.145142" y="552">
      <linkto id="632488299439989708" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632488299439989711" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AccountCode" type="variable">g_accountCode</ap>
        <ap name="Pin" type="csharp">int.Parse(receivedDigits)</ap>
        <ap name="UserPhoneNumber" type="variable">g_to</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989708" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="958" y="536" mx="1011" my="552">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632488299439989524" />
        <item text="OnPlay_Failed" treenode="632488299439989529" />
      </items>
      <linkto id="632488299439989714" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt3" type="literal">vm_beep.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="TermCondMaxTime" type="literal">60000</ap>
        <ap name="TermCondSilence" type="literal">60000</ap>
        <ap name="TermCondNonSilence" type="literal">60000</ap>
        <ap name="Prompt1" type="literal">vm_authAccepted.wav</ap>
        <ap name="Prompt2" type="literal">vm_recordGreetPrompt.wav</ap>
        <ap name="UserData" type="literal">recordGreeting</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnGatherDigits_Complete: playing "Record greeting prompt after beep" message
</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989711" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="700" y="737" mx="753" my="753">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632488299439989524" />
        <item text="OnPlay_Failed" treenode="632488299439989529" />
      </items>
      <linkto id="632488299439989714" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="Prompt1" type="literal">vm_authFailed.wav</ap>
        <ap name="Prompt2" type="literal">vm_tryAgain.wav</ap>
        <ap name="UserData" type="literal">enterPass</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989714" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1010" y="753">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632488299439989715" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="80" y="961.5">
      <linkto id="632488299439989716" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632488299439989716" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="209.999939" y="963.5">
      <linkto id="632488299439989717" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <linkto id="632488299439989726" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete: checking user selection regarding greeting confirmation</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989717" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="442.999939" y="967">
      <linkto id="632488299439989723" type="Labeled" style="Bezier" ortho="true" label="1" />
      <linkto id="632488299439989729" type="Labeled" style="Bezier" ortho="true" label="0" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">receivedDigits[receivedDigits.Length - 1]</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989721" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="732.4707" y="968.5">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632488299439989723" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="524" y="951.5" mx="577" my="968">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632488299439989524" />
        <item text="OnPlay_Failed" treenode="632488299439989529" />
      </items>
      <linkto id="632488299439989721" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt3" type="literal">vm_goodBye.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="TermCondMaxTime" type="literal">60000</ap>
        <ap name="timeout" type="literal">60000</ap>
        <ap name="TermCondSilence" type="literal">60000</ap>
        <ap name="TermCondNonSilence" type="literal">60000</ap>
        <ap name="Prompt1" type="literal">vm_promptConfirmed.wav</ap>
        <ap name="Prompt2" type="literal">vm_thankYou.wav</ap>
        <ap name="UserData" type="literal">greetingConfirmed</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989726" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="162" y="1087.5" mx="215" my="1104">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632488299439989524" />
        <item text="OnPlay_Failed" treenode="632488299439989529" />
      </items>
      <linkto id="632488299439989732" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="TermCondMaxTime" type="literal">60000</ap>
        <ap name="timeout" type="literal">60000</ap>
        <ap name="TermCondSilence" type="literal">60000</ap>
        <ap name="TermCondNonSilence" type="literal">60000</ap>
        <ap name="Prompt1" type="literal">vm_noResponse.wav</ap>
        <ap name="Prompt2" type="literal">vm_confirmPrompt.wav</ap>
        <ap name="UserData" type="literal">confirmGreeting</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989729" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="396" y="1090.5" mx="449" my="1106">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632488299439989524" />
        <item text="OnPlay_Failed" treenode="632488299439989529" />
      </items>
      <linkto id="632488299439989732" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="TermCondMaxTime" type="literal">60000</ap>
        <ap name="timeout" type="literal">60000</ap>
        <ap name="TermCondSilence" type="literal">60000</ap>
        <ap name="TermCondNonSilence" type="literal">60000</ap>
        <ap name="Prompt1" type="literal">vm_recordGreetPrompt.wav</ap>
        <ap name="Prompt2" type="literal">vm_beep.wav</ap>
        <ap name="UserData" type="literal">recordGreeting</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete: playing "Record greeting prompt after beep" message</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989732" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="327.4707" y="1252.5">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632610860136605975" text="R" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="337" y="347" />
    <node type="Label" id="632610860136605976" text="R" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="74" y="1542">
      <linkto id="632610860136605981" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632610860136605981" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="132" y="1528" mx="185" my="1544">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632488299439989524" />
        <item text="OnPlay_Failed" treenode="632488299439989529" />
      </items>
      <linkto id="632610860136605986" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt3" type="literal">vm_confirmPrompt.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="TermCondMaxTime" type="literal">240000</ap>
        <ap name="timeout" type="literal">240000</ap>
        <ap name="TermCondDigitList" type="literal">1,0</ap>
        <ap name="TermCondSilence" type="literal">240000</ap>
        <ap name="TermCondNonSilence" type="literal">240000</ap>
        <ap name="Prompt1" type="literal">vm_reviewPrompt.wav</ap>
        <ap name="Prompt2" type="variable">g_tempGreetingFilename</ap>
        <ap name="UserData" type="literal">confirmGreeting</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnGatherDigits_Complete: Playing confirmation message.</log>
      </Properties>
    </node>
    <node type="Action" id="632610860136605985" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="558" y="177">
      <linkto id="632488299439989688" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string receivedDigits)
{
	if (receivedDigits == null)
		receivedDigits = string.Empty;

	if (receivedDigits.Length &gt; 1)
		receivedDigits = receivedDigits.Substring(receivedDigits.Length - 1);

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632610860136605986" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="329" y="1545">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632610860136605987" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="598" y="551">
      <linkto id="632488299439989701" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632488299439989711" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string receivedDigits, LogWriter log)
{
	if (receivedDigits == null)
		receivedDigits = string.Empty;

	receivedDigits = receivedDigits.Replace("#", string.Empty);

	bool success = true;
	try
	{
		uint.Parse(receivedDigits);
	}
	catch
	{
		success = false;
		log.Write(TraceLevel.Info, "OnGatherDigits_Complete: could not parse provided pin into an uint.");
	}

	return success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;	
}
</Properties>
    </node>
    <node type="Variable" id="632488299439989673" name="receivedDigits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" defaultInitWith="X" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">receivedDigits</Properties>
    </node>
    <node type="Variable" id="632488299439989674" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">termCond</Properties>
    </node>
    <node type="Variable" id="632488299439989675" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632488299439989579" treenode="632488299439989580" appnode="632488299439989577" handlerfor="632488299439989576">
    <node type="Start" id="632488299439989579" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="377">
      <linkto id="632488299439989734" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632488299439989734" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="151" y="377">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Failed: function entry
</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Complete" startnode="632488299439989614" treenode="632488299439989615" appnode="632488299439989612" handlerfor="632488299439989611">
    <node type="Start" id="632488299439989614" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="250">
      <linkto id="632488299439989735" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632488299439989735" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="137.048828" y="249.166656">
      <linkto id="632566869913150607" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_isRecording</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnRecordAudio_Complete: the recording is stored in the following file: " + filename</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989737" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="336.048828" y="248.166656">
      <linkto id="632488299439989803" type="Labeled" style="Bezier" ortho="true" label="userstop" />
      <linkto id="632488299439989798" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989738" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="605.0488" y="114.166656">
      <linkto id="632488299439989754" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.Normal</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989740" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="616.0488" y="349.166656">
      <linkto id="632488299439989756" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">error</ap>
        <ap name="Value2" type="literal">vm_technicalDifficulties.wav</ap>
        <rd field="ResultData">outgoingUserData</rd>
        <rd field="ResultData2">fileNameToPlay</rd>
      </Properties>
    </node>
    <node type="Action" id="632488299439989741" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="613.0488" y="467.166656">
      <linkto id="632488299439989756" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">exit</ap>
        <ap name="Value2" type="literal">vm_thankYou.wav</ap>
        <rd field="ResultData">outgoingUserData</rd>
        <rd field="ResultData2">fileNameToPlay</rd>
      </Properties>
    </node>
    <node type="Action" id="632488299439989754" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="770.8261" y="113">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">OnRecordAudio_Complete_NewMsg: Terminating Application</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989756" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="716.8841" y="395" mx="770" my="411">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632488299439989524" />
        <item text="OnPlay_Failed" treenode="632488299439989529" />
      </items>
      <linkto id="632488299439989759" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632488299439989760" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="Prompt1" type="variable">fileNameToPlay</ap>
        <ap name="Prompt2" type="literal">vm_goodBye.wav</ap>
        <ap name="UserData" type="variable">outgoingUserData</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989759" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="766.8841" y="561">
      <linkto id="632488299439989760" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId_caller</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989760" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="991.7096" y="412">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632488299439989761" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="195" y="710" mx="248" my="726">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632488299439989524" />
        <item text="OnPlay_Failed" treenode="632488299439989529" />
      </items>
      <linkto id="632488299439989764" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt3" type="literal">vm_confirmPrompt.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="TermCondMaxTime" type="literal">240000</ap>
        <ap name="timeout" type="literal">240000</ap>
        <ap name="TermCondDigitList" type="literal">1,0</ap>
        <ap name="TermCondSilence" type="literal">240000</ap>
        <ap name="TermCondNonSilence" type="literal">240000</ap>
        <ap name="Prompt1" type="literal">vm_reviewPrompt.wav</ap>
        <ap name="Prompt2" type="variable">filename</ap>
        <ap name="UserData" type="literal">confirmGreeting</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnRecord_Complete: Playing confirmation message.</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989764" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="398.4707" y="727">
      <linkto id="632488299439989765" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">filename</ap>
        <rd field="ResultData">g_tempGreetingFilename</rd>
      </Properties>
    </node>
    <node type="Action" id="632488299439989765" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="400.4707" y="827">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632488299439989769" text="add error functionality" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="703.1803" y="612" />
    <node type="Action" id="632488299439989798" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="437.4264" y="387" mx="486" my="403">
      <items count="1">
        <item text="UserNotifyAction" />
      </items>
      <linkto id="632488299439989740" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632488299439989741" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">UserNotifyAction</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989803" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="419.4264" y="98" mx="468" my="114">
      <items count="1">
        <item text="UserNotifyAction" />
      </items>
      <linkto id="632488299439989738" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">UserNotifyAction</ap>
      </Properties>
    </node>
    <node type="Action" id="632566869913150607" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="237.000015" y="249">
      <linkto id="632488299439989737" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632610860136605977" type="Labeled" style="Bezier" ortho="true" label="recordGreeting" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632610860136605977" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="241" y="552">
      <linkto id="632488299439989761" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632610860136605978" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnRecord_Complete: checking whether we need to clear digit buffer or not...</log>
      </Properties>
    </node>
    <node type="Action" id="632610860136605978" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="324.999939" y="536" mx="399" my="552">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632488299439989575" />
        <item text="OnGatherDigits_Failed" treenode="632488299439989580" />
      </items>
      <linkto id="632488299439989764" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="TermCondMaxTime" type="literal">240000</ap>
        <ap name="timeout" type="literal">240000</ap>
        <ap name="UserData" type="literal">recordGreeting</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnRecord_Complete: invoking GatherDigits with termCondDigit = #</log>
      </Properties>
    </node>
    <node type="Variable" id="632488299439989749" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Record_Complete">termCond</Properties>
    </node>
    <node type="Variable" id="632488299439989750" name="outgoingUserData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">outgoingUserData</Properties>
    </node>
    <node type="Variable" id="632488299439989751" name="fileNameToPlay" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">fileNameToPlay</Properties>
    </node>
    <node type="Variable" id="632488299439989752" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632488299439989753" name="filename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Filename" refType="reference" name="Metreos.MediaControl.Record_Complete">filename</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Failed" startnode="632488299439989619" treenode="632488299439989620" appnode="632488299439989617" handlerfor="632488299439989616">
    <node type="Start" id="632488299439989619" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="276">
      <linkto id="632488299439989770" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632488299439989770" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="140.048828" y="277">
      <linkto id="632488299439989774" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_isRecording</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnRecordAudio_Failed: setting g_isRecording to false</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989772" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="420.4707" y="277">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632488299439989774" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="229.049988" y="261.083344" mx="282" my="277">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632488299439989524" />
        <item text="OnPlay_Failed" treenode="632488299439989529" />
      </items>
      <linkto id="632488299439989775" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632488299439989772" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="Prompt1" type="literal">vm_technicalDifficulties.wav</ap>
        <ap name="UserData" type="literal">error</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989775" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="281.05" y="447.083344">
      <linkto id="632488299439989772" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId_caller</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnRecord_Failed: PlayAnnouncement failed, manual hang-up</log>
      </Properties>
    </node>
    <node type="Comment" id="632488299439989776" text="add error functionality" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="218.346191" y="516.0834" />
  </canvas>
  <canvas type="Function" name="OnGotDigits" startnode="632488299439989785" treenode="632488299439989786" appnode="632488299439989783" handlerfor="632488299439989782">
    <node type="Start" id="632488299439989785" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="294">
      <linkto id="632488299439989788" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632488299439989788" name="SendUserInput" class="MaxActionNode" group="" path="Metreos.CallControl" x="149" y="296">
      <linkto id="632488299439989789" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId_caller</ap>
        <ap name="Digits" type="variable">digits</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989789" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="280" y="297">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632488299439989787" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" refType="reference" name="Metreos.CallControl.GotDigits">digits</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="RecordMessage" startnode="632488299439989563" treenode="632488299439989564" appnode="632488299439989561" handlerfor="632488299439989782">
    <node type="Start" id="632488299439989563" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="303">
      <linkto id="632720674723867597" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632488299439989656" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="274.519531" y="304">
      <linkto id="632488299439989664" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="csharp">string.Format( "{0}-{1}.wav", Convert.ToString(g_userId), g_messageTimestamp.ToString("ddMMyyyyHHmmssff") );</ap>
        <rd field="ResultData">g_isRecording</rd>
        <rd field="ResultData2">g_recordingFilename</rd>
      </Properties>
    </node>
    <node type="Action" id="632488299439989657" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="141.048828" y="445">
      <linkto id="632488299439989656" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">System.DateTime.Now</ap>
        <rd field="ResultData">g_messageTimestamp</rd>
      </Properties>
    </node>
    <node type="Action" id="632488299439989660" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="409.048828" y="464">
      <linkto id="632488299439989667" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_isRecording</rd>
      </Properties>
    </node>
    <node type="Action" id="632488299439989662" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="616.4707" y="304">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989664" name="Record" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="352" y="289" mx="412" my="305">
      <items count="2">
        <item text="OnRecord_Complete" treenode="632488299439989615" />
        <item text="OnRecord_Failed" treenode="632488299439989620" />
      </items>
      <linkto id="632488299439989662" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632488299439989660" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Expires" type="literal">7</ap>
        <ap name="CommandTimeout" type="csharp">g_maxMessageLength  + 5000;
</ap>
        <ap name="Filename" type="variable">g_recordingFilename</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="TermCondMaxTime" type="variable">g_maxMessageLength</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="TermCondSilence" type="variable">g_maxMessageLength</ap>
        <ap name="TermCondNonSilence" type="variable">g_maxMessageLength</ap>
        <ap name="AudioFileFormat" type="literal">wav</ap>
        <ap name="UserData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"RecordMessage: Invoking RecordAudio, recording for " + g_maxMessageLength.ToString()
</log>
        <log condition="success" on="true" level="Info" type="literal">RecordMessage: RecordAudio action succeeded
</log>
        <log condition="failure" on="true" level="Info" type="literal">RecordMessage: RecordAudio action failed!
</log>
        <log condition="default" on="true" level="Info" type="literal">RecordMessage: RecordAudio followed 'default' branch
</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989667" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="358" y="564" mx="411" my="580">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632488299439989524" />
        <item text="OnPlay_Failed" treenode="632488299439989529" />
      </items>
      <linkto id="632488299439989670" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632488299439989671" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="Prompt1" type="literal">vm_technicalDifficulties.wav</ap>
        <ap name="UserData" type="literal">error</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439989670" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="617" y="579">
      <linkto id="632488299439990427" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId_caller</ap>
        <log condition="entry" on="true" level="Info" type="literal">RecordMessage: hanging up call, exiting script</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439989671" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="410" y="775">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632488299439990427" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="617.36" y="678">
      <linkto id="632488299439990429" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.InternalError</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439990429" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="618" y="775">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632720674723867597" name="GetUserCurrentDateTime" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="139" y="304">
      <linkto id="632488299439989657" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632488299439989656" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
        <rd field="DateTime">g_messageTimestamp</rd>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="UserNotifyAction" startnode="632488299439989801" treenode="632488299439989802" appnode="632488299439989799" handlerfor="632488299439989782">
    <node type="Start" id="632488299439989801" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="305">
      <linkto id="632488299439990412" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632488299439990412" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="251.195313" y="307">
      <linkto id="632488299439990416" type="Labeled" style="Bezier" label="default" />
      <linkto id="632488299439990416" type="Labeled" style="Bezier" label="mail" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">g_notifyMethod</ap>
        <log condition="entry" on="true" level="Info" type="literal">UserNotifyAction: determining notification method...</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439990413" name="Send" class="MaxActionNode" group="" path="Metreos.Native.Mail" x="517.1953" y="308">
      <linkto id="632488299439990414" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632488299439990415" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="To" type="variable">g_notificationAddress</ap>
        <ap name="From" type="variable">g_fromEmailAddress</ap>
        <ap name="MailServer" type="variable">g_MailServer</ap>
        <ap name="Subject" type="literal">You have a new voice mail!</ap>
        <ap name="Body" type="csharp">"You have received a new voice mail from: " + (g_from == null ? "CallerId not available" : g_from) + ". \nMessage was received at: " + g_messageTimestamp</ap>
        <ap name="SendAsHtml" type="literal">false</ap>
        <ap name="AttachmentPaths" type="variable">attachmentPaths</ap>
        <log condition="entry" on="false" level="Info" type="csharp">"UserNotifyAction: E-mailing notification to address: " + (g_notificationAddress == null ? "NOT DEFINED!" : g_notificationAddress)</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439990414" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="672.1953" y="264">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">success</ap>
      </Properties>
    </node>
    <node type="Action" id="632488299439990415" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="673.1953" y="364">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
        <log condition="entry" on="true" level="Error" type="literal">UserNotifyAction: Failed to email user!</log>
      </Properties>
    </node>
    <node type="Action" id="632488299439990416" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="404.1953" y="308">
      <linkto id="632488299439990413" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(System.Collections.Specialized.StringCollection attachmentPaths, string g_mediaPath, string g_recordingFilename)
	{
		attachmentPaths.Add(g_mediaPath.ToString() + g_recordingFilename.ToString());
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Variable" id="632488299439990411" name="attachmentPaths" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="StringCollection" refType="reference">attachmentPaths</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>