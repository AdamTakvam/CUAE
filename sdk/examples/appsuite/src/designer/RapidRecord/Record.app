<Application name="Record" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Record">
    <outline>
      <treenode type="evh" id="632474470609893620" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632474470609893617" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632474470609893616" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632474473782255450" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632474473782255447" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632474473782255446" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632802647943682702" actid="632474473782255461" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632474473782255455" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632474473782255452" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632474473782255451" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632802647943682703" actid="632474473782255461" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632474473782255460" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632474473782255457" path="Metreos.StockTools" />
        <calls>
          <ref actid="632474473782255813" />
        </calls>
        <node type="event" name="RemoteHangup" id="632474473782255456" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632802647943682704" actid="632474473782255461" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632474473782255486" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632474473782255483" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632474473782255482" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632802647943682713" actid="632474473782255492" />
          <ref id="632802647943682761" actid="632474473782255504" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632474473782255491" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632474473782255488" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632474473782255487" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632802647943682714" actid="632474473782255492" />
          <ref id="632802647943682762" actid="632474473782255504" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632474473782255581" level="2" text="Metreos.Providers.Http.GotRequest: HandleCommandRequest">
        <node type="function" name="HandleCommandRequest" id="632474473782255578" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632474473782255577" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/RecordingCommand</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632474473782255586" level="2" text="Metreos.Providers.Http.GotRequest: HandleKeysRequest">
        <node type="function" name="HandleKeysRequest" id="632474473782255583" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632474473782255582" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/RequestKeys</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632474473782255757" level="2" text="Metreos.MediaControl.Record_Complete: OnRecord_Complete">
        <node type="function" name="OnRecord_Complete" id="632474473782255754" path="Metreos.StockTools" />
        <node type="event" name="Record_Complete" id="632474473782255753" path="Metreos.MediaControl.Record_Complete" />
        <references>
          <ref id="632802647943682731" actid="632474473782255763" />
          <ref id="632802647943682833" actid="632474473782256315" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632474473782255762" level="2" text="Metreos.MediaControl.Record_Failed: OnRecord_Failed">
        <node type="function" name="OnRecord_Failed" id="632474473782255759" path="Metreos.StockTools" />
        <node type="event" name="Record_Failed" id="632474473782255758" path="Metreos.MediaControl.Record_Failed" />
        <references>
          <ref id="632802647943682732" actid="632474473782255763" />
          <ref id="632802647943682834" actid="632474473782256315" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="fun" id="632474473782255545" level="1" text="ClearPhone">
        <node type="function" name="ClearPhone" id="632474473782255542" path="Metreos.StockTools" />
        <calls>
          <ref actid="632474473782255541" />
          <ref actid="632474473782255546" />
        </calls>
      </treenode>
      <treenode type="fun" id="632474473782255649" level="1" text="HandlePhoneNegotiation">
        <node type="function" name="HandlePhoneNegotiation" id="632474473782255646" path="Metreos.StockTools" />
        <calls>
          <ref actid="632474473782255780" />
          <ref actid="632474473782255655" />
          <ref actid="632474473782255672" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_translationPatternMatch" id="632802647943682600" vid="632344880926875169">
        <Properties type="String" defaultInitWith="UNDEFINED" initWith="RecordingPartitionTranslationMatch">g_translationPatternMatch</Properties>
      </treenode>
      <treenode text="g_dbDatabaseName" id="632802647943682602" vid="632344880926875201">
        <Properties type="String" initWith="DatabaseName">g_dbDatabaseName</Properties>
      </treenode>
      <treenode text="g_dbHost" id="632802647943682604" vid="632344880926875203">
        <Properties type="String" initWith="Server">g_dbHost</Properties>
      </treenode>
      <treenode text="g_dbPort" id="632802647943682606" vid="632344880926875205">
        <Properties type="String" initWith="Port">g_dbPort</Properties>
      </treenode>
      <treenode text="g_dbUsername" id="632802647943682608" vid="632344880926875223">
        <Properties type="String" initWith="Username">g_dbUsername</Properties>
      </treenode>
      <treenode text="g_dbPassword" id="632802647943682610" vid="632344880926875225">
        <Properties type="String" initWith="Password">g_dbPassword</Properties>
      </treenode>
      <treenode text="g_recordCall" id="632802647943682612" vid="632344894073125229">
        <Properties type="Bool">g_recordCall</Properties>
      </treenode>
      <treenode text="g_callerConnectionId" id="632802647943682614" vid="632345004006718973">
        <Properties type="Int">g_callerConnectionId</Properties>
      </treenode>
      <treenode text="g_callerCallId" id="632802647943682616" vid="632345004006718984">
        <Properties type="String">g_callerCallId</Properties>
      </treenode>
      <treenode text="g_calleeConnectionId" id="632802647943682618" vid="632345004006718991">
        <Properties type="Int">g_calleeConnectionId</Properties>
      </treenode>
      <treenode text="g_calleeCallId" id="632802647943682620" vid="632345004006718993">
        <Properties type="String">g_calleeCallId</Properties>
      </treenode>
      <treenode text="g_callRecordId" id="632802647943682622" vid="632345004006719043">
        <Properties type="UInt">g_callRecordId</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632802647943682624" vid="632345004006719070">
        <Properties type="UInt">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_recordingConnectionId" id="632802647943682626" vid="632345157762500343">
        <Properties type="UInt">g_recordingConnectionId</Properties>
      </treenode>
      <treenode text="g_mediaServerIp" id="632802647943682628" vid="632345157762500347">
        <Properties type="String">g_mediaServerIp</Properties>
      </treenode>
      <treenode text="g_callerHungup" id="632802647943682630" vid="632345735417344138">
        <Properties type="Bool" defaultInitWith="true">g_callerHungup</Properties>
      </treenode>
      <treenode text="g_calleeHungup" id="632802647943682632" vid="632345735417344140">
        <Properties type="Bool" defaultInitWith="true">g_calleeHungup</Properties>
      </treenode>
      <treenode text="g_recordingDone" id="632802647943682634" vid="632345735417344142">
        <Properties type="Bool" defaultInitWith="true">g_recordingDone</Properties>
      </treenode>
      <treenode text="g_toUserId" id="632802647943682636" vid="632345898300312906">
        <Properties type="UInt" defaultInitWith="0">g_toUserId</Properties>
      </treenode>
      <treenode text="g_fromUserId" id="632802647943682638" vid="632345898300312908">
        <Properties type="UInt" defaultInitWith="0">g_fromUserId</Properties>
      </treenode>
      <treenode text="g_toUserAwareOfRecord" id="632802647943682640" vid="632345898300312912">
        <Properties type="Bool">g_toUserAwareOfRecord</Properties>
      </treenode>
      <treenode text="g_fromUserAwareOfRecord" id="632802647943682642" vid="632345898300312914">
        <Properties type="Bool">g_fromUserAwareOfRecord</Properties>
      </treenode>
      <treenode text="g_toRecordingRecordId" id="632802647943682644" vid="632345971275312920">
        <Properties type="UInt">g_toRecordingRecordId</Properties>
      </treenode>
      <treenode text="g_fromRecordingRecordId" id="632802647943682646" vid="632345971275312922">
        <Properties type="UInt">g_fromRecordingRecordId</Properties>
      </treenode>
      <treenode text="g_toUserRecord" id="632802647943682648" vid="632346813138906686">
        <Properties type="Bool">g_toUserRecord</Properties>
      </treenode>
      <treenode text="g_fromUserRecord" id="632802647943682650" vid="632346813138906688">
        <Properties type="Bool">g_fromUserRecord</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="632802647943682652" vid="632346881090782012">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
      <treenode text="g_toNumber" id="632802647943682654" vid="632346908118125470">
        <Properties type="String">g_toNumber</Properties>
      </treenode>
      <treenode text="g_fromNumber" id="632802647943682656" vid="632346908118125472">
        <Properties type="String">g_fromNumber</Properties>
      </treenode>
      <treenode text="g_toIpAddress" id="632802647943682658" vid="632346908118125485">
        <Properties type="String" defaultInitWith="NONE">g_toIpAddress</Properties>
      </treenode>
      <treenode text="g_fromIpAddress" id="632802647943682660" vid="632346908118125487">
        <Properties type="String" defaultInitWith="NONE">g_fromIpAddress</Properties>
      </treenode>
      <treenode text="g_startingRecording" id="632802647943682662" vid="632346908118125499">
        <Properties type="Bool" defaultInitWith="false">g_startingRecording</Properties>
      </treenode>
      <treenode text="g_applicationServerIp" id="632802647943682664" vid="632346908118126176">
        <Properties type="String" initWith="Application_Server_IP">g_applicationServerIp</Properties>
      </treenode>
      <treenode text="g_applicationServerPort" id="632802647943682666" vid="632346908118126178">
        <Properties type="String" initWith="Application_Server_HTTP_Port">g_applicationServerPort</Properties>
      </treenode>
      <treenode text="g_shutdownCommencing" id="632802647943682668" vid="632346908118127669">
        <Properties type="Bool" defaultInitWith="false">g_shutdownCommencing</Properties>
      </treenode>
      <treenode text="g_saveMedia" id="632802647943682670" vid="632346908118127671">
        <Properties type="Bool" defaultInitWith="false">g_saveMedia</Properties>
      </treenode>
      <treenode text="g_userRecordStop" id="632802647943682672" vid="632346908118128857">
        <Properties type="Bool" defaultInitWith="false">g_userRecordStop</Properties>
      </treenode>
      <treenode text="g_ccm_device_username" id="632802647943682674" vid="632347759787032279">
        <Properties type="String" initWith="CCM_Device_Username">g_ccm_device_username</Properties>
      </treenode>
      <treenode text="g_ccm_device_password" id="632802647943682676" vid="632347759787032281">
        <Properties type="String" initWith="CCM_Device_Password">g_ccm_device_password</Properties>
      </treenode>
      <treenode text="g_record_max_time" id="632802647943682678" vid="632348413338438534">
        <Properties type="Int" initWith="Recording_Max_Time">g_record_max_time</Properties>
      </treenode>
      <treenode text="g_record_silence_time" id="632802647943682680" vid="632348413338438963">
        <Properties type="Int" initWith="Max_Silence_Time">g_record_silence_time</Properties>
      </treenode>
      <treenode text="g_mmsId" id="632802647943682682" vid="632767263316801808">
        <Properties type="UInt">g_mmsId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632474470609893619" treenode="632474470609893620" appnode="632474470609893617" handlerfor="632474470609893616">
    <node type="Start" id="632474470609893619" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="365">
      <linkto id="632495971237096697" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782255401" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="183" y="365">
      <linkto id="632474473782255407" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">g_dbDatabaseName</ap>
        <ap name="Server" type="variable">g_dbHost</ap>
        <ap name="Port" type="variable">g_dbPort</ap>
        <ap name="Username" type="variable">g_dbUsername</ap>
        <ap name="Password" type="variable">g_dbPassword</ap>
        <rd field="DSN">dsn</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"Call Recording application starting.\n\n " + routingGuid + "\n\n"</log>
      </Properties>
    </node>
    <node type="Comment" id="632474473782255402" text="Create the standard ApplicationSuite database connection" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="39" y="311" />
    <node type="Comment" id="632474473782255403" text="We continue if the database failed to open,&#xD;&#xA;because we at least want the users to be &#xD;&#xA;able to communicate.  We won't record though." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="371" y="230" />
    <node type="Action" id="632474473782255404" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="807" y="629">
      <linkto id="632474473782255461" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="literal">false</ap>
        <ap name="Value3" type="literal">false</ap>
        <rd field="ResultData">g_recordCall</rd>
        <rd field="ResultData2">g_toUserRecord</rd>
        <rd field="ResultData3">g_fromUserRecord</rd>
      </Properties>
    </node>
    <node type="Action" id="632474473782255405" name="GetUserByDn" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="655" y="421">
      <linkto id="632474473782255411" type="Labeled" style="Vector" ortho="true" label="success" />
      <linkto id="632474473782255415" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DirectoryNumber" type="variable">from</ap>
        <rd field="UsersId">g_fromUserId</rd>
        <log condition="success" on="true" level="Info" type="csharp">"Found user for " + from</log>
        <log condition="default" on="true" level="Verbose" type="csharp">"No associated user for number '" + from  + "'";</log>
      </Properties>
    </node>
    <node type="Comment" id="632474473782255406" text="Do not record this call" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="615" y="573" />
    <node type="Action" id="632474473782255407" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="344" y="365">
      <linkto id="632474473782255410" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Verbose" type="csharp">"To: " + to + ", Translation Pattern: " + g_translationPatternMatch</log>
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
	}
</Properties>
    </node>
    <node type="Comment" id="632474473782255408" text="                     *** IMPORTANT ***&#xD;&#xA;The application currently expects the configured&#xD;&#xA;translation pattern to be of the form ###.+, &#xD;&#xA;meaning any number of prefix digts followed by &#xD;&#xA;.+, which in Regular Expression means any &#xD;&#xA;character, 1 or more times (representing the &#xD;&#xA;number actually dialed)&#xD;&#xA;&#xD;&#xA;This custom code will:&#xD;&#xA;Rip off the beginning translation pattern digits,&#xD;&#xA;assigning the truncated 'to' field to the 'to' &#xD;&#xA;variable, as well as the 'from' field and 'from' variable." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="257" y="418" />
    <node type="Action" id="632474473782255409" name="GetUserByDn" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="655" y="293">
      <linkto id="632474473782255405" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632474473782255414" type="Labeled" style="Vector" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="DirectoryNumber" type="variable">to</ap>
        <rd field="UsersId">g_toUserId</rd>
        <log condition="success" on="true" level="Info" type="csharp">"Found user for " + to</log>
        <log condition="default" on="true" level="Verbose" type="csharp">"No associated user for number '" + to  + "'";</log>
      </Properties>
    </node>
    <node type="Action" id="632474473782255410" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="495" y="365">
      <linkto id="632474473782255404" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632474473782255409" type="Labeled" style="Vector" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">ApplicationSuite</ap>
        <ap name="Type" type="literal">mysql</ap>
        <log condition="default" on="true" level="Info" type="literal">Unable to open the database</log>
      </Properties>
    </node>
    <node type="Action" id="632474473782255411" name="ShouldUserBeRecorded" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="927" y="421">
      <linkto id="632474473782255415" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UsersId" type="variable">g_fromUserId</ap>
        <rd field="Record">g_fromUserRecord</rd>
        <rd field="RecordingVisible">g_fromUserAwareOfRecord</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"Check if the user with number " + from + " should be recorded"</log>
        <log condition="exit" on="true" level="Verbose" type="csharp">"Record: " + g_fromUserRecord + ", Recording visible to user: " + g_fromUserAwareOfRecord</log>
      </Properties>
    </node>
    <node type="Comment" id="632474473782255412" text="Should user be recorded ?&#xD;&#xA;Should user be aware of being recorded?" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="975" y="285" />
    <node type="Comment" id="632474473782255413" text="Determine user given the 'from' and 'to' number" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="671" y="213" />
    <node type="Action" id="632474473782255414" name="ShouldUserBeRecorded" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="919" y="293">
      <linkto id="632474473782255405" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UsersId" type="variable">g_toUserId</ap>
        <rd field="Record">g_toUserRecord</rd>
        <rd field="RecordingVisible">g_toUserAwareOfRecord</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"Check if the user with number " + to + " should be recorded"</log>
        <log condition="exit" on="true" level="Verbose" type="csharp">"Record: " + g_toUserRecord + ", Recording visible to user: " + g_fromUserAwareOfRecord</log>
      </Properties>
    </node>
    <node type="Action" id="632474473782255415" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1046" y="359">
      <linkto id="632474473782255442" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(
						ref bool g_recordCall, 
						bool g_toUserRecord, 
						bool g_fromUserRecord)
	{
		// If either user should be recorded, then we will have to record the call
		g_recordCall = g_toUserRecord || g_fromUserRecord;

		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632474473782255442" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1240" y="359">
      <linkto id="632474473782255461" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">from</ap>
        <ap name="Value2" type="variable">routingGuid</ap>
        <ap name="Value3" type="variable">to</ap>
        <rd field="ResultData">g_fromNumber</rd>
        <rd field="ResultData2">g_routingGuid</rd>
        <rd field="ResultData3">g_toNumber</rd>
      </Properties>
    </node>
    <node type="Comment" id="632474473782255443" text="Save MediaServer IP for later use&#xD;&#xA;Save RoutingGuid for later use&#xD;&#xA;Save to DN for later use&#xD;&#xA;Save from DN for later use" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1128" y="400" />
    <node type="Action" id="632474473782255461" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="1384" y="344" mx="1450" my="360">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632474473782255450" />
        <item text="OnMakeCall_Failed" treenode="632474473782255455" />
        <item text="OnRemoteHangup" treenode="632474473782255460" />
      </items>
      <linkto id="632474473782255466" type="Labeled" style="Vector" ortho="true" label="Success" />
      <linkto id="632474473782255480" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">to</ap>
        <ap name="From" type="variable">from</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_calleeCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632474473782255465" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1744" y="360">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632474473782255466" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1608" y="360">
      <linkto id="632474473782255465" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="OriginNumber" type="variable">from</ap>
        <ap name="DestinationNumber" type="variable">to</ap>
        <ap name="UserId" type="csharp">g_fromUserId == 0 ? g_toUserId : g_fromUserId</ap>
        <rd field="CallRecordsId">g_callRecordId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"Creating Call Record To: " + to + ", From: " + from</log>
      </Properties>
    </node>
    <node type="Label" id="632474473782255470" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1176" y="600">
      <linkto id="632474473782255471" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782255471" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1296" y="600">
      <linkto id="632474473782255472" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="OriginNumber" type="variable">from</ap>
        <ap name="DestinationNumber" type="variable">to</ap>
        <ap name="UserId" type="csharp">g_fromUserId == 0 ? g_toUserId : g_fromUserId</ap>
        <rd field="CallRecordsId">g_callRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632474473782255472" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1440" y="600">
      <linkto id="632474473782255478" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="variable">systemFailureEndReason</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255478" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="1608" y="600">
      <linkto id="632474473782255492" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callerCallId</ap>
        <rd field="ConnectionId">g_callerConnectionId</rd>
      </Properties>
    </node>
    <node type="Label" id="632474473782255480" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1448" y="511.999969" />
    <node type="Action" id="632474473782255492" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1720" y="584" mx="1773" my="600">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632474473782255486" />
        <item text="OnPlay_Failed" treenode="632474473782255491" />
      </items>
      <linkto id="632474473782255495" type="Labeled" style="Vector" ortho="true" label="Success" />
      <linkto id="632474473782255555" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">system_can_not_call.wav</ap>
        <ap name="ConnectionId" type="variable">g_callerConnectionId</ap>
        <ap name="UserData" type="literal">SystemFailure</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255495" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1920" y="600">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632474473782255555" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="1768" y="736">
      <linkto id="632474473782255556" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callerCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255556" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1768" y="824">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632495971237096697" name="AcceptCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="102" y="458">
      <linkto id="632474473782255401" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <rd field="CallId">g_callerCallId</rd>
      </Properties>
    </node>
    <node type="Comment" id="632495971237096698" text="Need to explicitly accept call before&#xD;&#xA;we do the MakeCall, since otherwise&#xD;&#xA;the call will time out" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="509" />
    <node type="Variable" id="632474473782255435" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632474473782255437" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference">from</Properties>
    </node>
    <node type="Variable" id="632474473782255438" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632474473782255440" name="systemFailureEndReason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="SystemFailure" refType="reference">systemFailureEndReason</Properties>
    </node>
    <node type="Variable" id="632474473782255441" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632474473782255481" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference">to</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" activetab="true" startnode="632474473782255449" treenode="632474473782255450" appnode="632474473782255447" handlerfor="632474473782255446">
    <node type="Start" id="632474473782255449" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="336">
      <linkto id="632474473782255496" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782255496" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="161" y="337">
      <linkto id="632474473782255502" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">false</ap>
        <ap name="Value2" type="variable">conferenceId</ap>
        <ap name="Value3" type="variable">mmsId</ap>
        <rd field="ResultData">g_calleeHungup</rd>
        <rd field="ResultData2">g_conferenceId</rd>
        <rd field="ResultData3">g_mmsId</rd>
      </Properties>
    </node>
    <node type="Comment" id="632474473782255498" text="Answer the call of the&#xD;&#xA;person making the call." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="248" y="240" />
    <node type="Action" id="632474473782255502" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="322" y="335">
      <linkto id="632474473782255763" type="Labeled" style="Vector" ortho="true" label="Success" />
      <linkto id="632474473782255813" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callerCallId</ap>
        <ap name="MmsId" type="variable">g_mmsId</ap>
        <ap name="ProxyDTMFCallId" type="variable">g_calleeCallId</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <rd field="ConnectionId">g_callerConnectionId</rd>
        <rd field="MediaRxIP">g_mediaServerIp</rd>
      </Properties>
    </node>
    <node type="Action" id="632474473782255763" name="Record" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="488" y="320" mx="548" my="336">
      <items count="2">
        <item text="OnRecord_Complete" treenode="632474473782255757" />
        <item text="OnRecord_Failed" treenode="632474473782255762" />
      </items>
      <linkto id="632474473782255770" type="Labeled" style="Vector" ortho="true" label="Success" />
      <linkto id="632474473782255802" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="AudioFileFormat" type="literal">wav</ap>
        <ap name="CommandTimeout" type="variable">g_record_max_time</ap>
        <ap name="Expires" type="literal">0</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="TermCondMaxTime" type="variable">g_record_max_time</ap>
        <ap name="TermCondSilence" type="variable">g_record_silence_time</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="ConnectionId">g_recordingConnectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632474473782255766" name="WriteRecordingStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="944" y="336">
      <linkto id="632474473782255768" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="MediaType" type="csharp">Metreos.ApplicationSuite.Storage.MediaFileType.Wav</ap>
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="UsersId" type="variable">g_toUserId</ap>
        <rd field="RecordingsId">g_toRecordingRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632474473782255767" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="824" y="337">
      <linkto id="632474473782255768" type="Labeled" style="Vector" ortho="true" label="false" />
      <linkto id="632474473782255766" type="Labeled" style="Vector" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_toUserRecord || g_toUserAwareOfRecord</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255768" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="824" y="480">
      <linkto id="632474473782255769" type="Labeled" style="Vector" ortho="true" label="true" />
      <linkto id="632474473782255772" type="Labeled" style="Vector" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_fromUserRecord || g_fromUserAwareOfRecord</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255769" name="WriteRecordingStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="944" y="480">
      <linkto id="632474473782255772" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="MediaType" type="csharp">Metreos.ApplicationSuite.Storage.MediaFileType.Wav</ap>
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="UsersId" type="variable">g_fromUserId</ap>
        <rd field="RecordingsId">g_fromRecordingRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632474473782255770" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="687" y="337">
      <linkto id="632474473782255767" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">false</ap>
        <ap name="Value2" type="csharp">g_toUserRecord || g_fromUserRecord</ap>
        <ap name="Value3" type="csharp">true</ap>
        <rd field="ResultData">g_recordingDone</rd>
        <rd field="ResultData2">g_saveMedia</rd>
        <rd field="ResultData3">g_startingRecording</rd>
      </Properties>
    </node>
    <node type="Comment" id="632474473782255771" text="See if we can retrieve a &#xD;&#xA;device name for the directory &#xD;&#xA;number of a user" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1024" y="520" />
    <node type="Action" id="632474473782255772" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="824" y="632">
      <linkto id="632474473782255773" type="Labeled" style="Vector" ortho="true" label="true" />
      <linkto id="632474473782255776" type="Labeled" style="Vector" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_toUserId != 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255773" name="GetDeviceByDn" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="952" y="632">
      <linkto id="632474473782255776" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632474473782255774" type="Labeled" style="Vector" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="DirectoryNumber" type="variable">g_toNumber</ap>
        <rd field="DeviceName">toDeviceName</rd>
        <log condition="exit" on="true" level="Info" type="csharp">"ToDeviceName: " + toDeviceName</log>
      </Properties>
    </node>
    <node type="Action" id="632474473782255774" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="1112" y="632">
      <linkto id="632474473782255776" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">toDeviceName</ap>
        <rd field="ResultData">toDeviceResults</rd>
      </Properties>
    </node>
    <node type="Label" id="632474473782255775" text="J" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1424" y="880">
      <linkto id="632474473782255781" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782255776" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="952" y="800">
      <linkto id="632474473782255777" type="Labeled" style="Vector" ortho="true" label="true" />
      <linkto id="632474473782255779" type="Labeled" style="Vector" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_fromUserId != 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255777" name="GetDeviceByDn" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1104" y="800">
      <linkto id="632474473782255778" type="Labeled" style="Vector" ortho="true" label="success" />
      <linkto id="632474473782255779" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DirectoryNumber" type="variable">g_fromNumber</ap>
        <rd field="DeviceName">fromDeviceName</rd>
        <log condition="exit" on="true" level="Info" type="csharp">"FromDeviceName: " + fromDeviceName</log>
      </Properties>
    </node>
    <node type="Action" id="632474473782255778" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="1256" y="800">
      <linkto id="632474473782255779" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">fromDeviceName</ap>
        <rd field="ResultData">fromDeviceResults</rd>
      </Properties>
    </node>
    <node type="Action" id="632474473782255779" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1104" y="984">
      <linkto id="632474473782255780" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(
				LogWriter log,
				DataTable toDeviceResults, 
				DataTable fromDeviceResults,
				ref string g_toIpAddress,
				ref string g_fromIpAddress)
	{
		try
		{
			if (toDeviceResults == null)
				log.Write(TraceLevel.Error, "ToDeviceCounts is null");
 			else
				log.Write(TraceLevel.Info, "ToDeviceCount: " + toDeviceResults.Rows.Count);


			if (fromDeviceResults == null)
				log.Write(TraceLevel.Error, "fromDeviceCounts is null");
 			else
				log.Write(TraceLevel.Info, "fromDeviceCount: " + fromDeviceResults.Rows.Count);

			if(toDeviceResults.Rows.Count &gt; 0)
			{
				g_toIpAddress = (string) toDeviceResults.Rows[0]["ip"];
				if(g_toIpAddress == null)
				{
					g_toIpAddress = String.Empty;
				}
			}
			if(fromDeviceResults.Rows.Count &gt; 0)
			{	
				g_fromIpAddress = (string) fromDeviceResults.Rows[0]["ip"];
				if(g_fromIpAddress == null)
				{
					g_fromIpAddress = String.Empty;
				}
			}
		}
		catch (Exception e)
		{
			log.Write(TraceLevel.Error, "Unable to access the results from the DeviceListX query.  Exception message is: " + e.Message);
		}

		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632474473782255780" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1184" y="969" mx="1255" my="985">
      <items count="1">
        <item text="HandlePhoneNegotiation" />
      </items>
      <linkto id="632474473782255781" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="both" type="csharp">true</ap>
        <ap name="command" type="csharp">g_toUserRecord || g_fromUserRecord ? "Stop" : "Start"</ap>
        <ap name="FunctionName" type="literal">HandlePhoneNegotiation</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255781" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1424" y="984">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632474473782255802" text="J" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="544" y="488" />
    <node type="Action" id="632474473782255812" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="324" y="606">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632474473782255813" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="272" y="449" mx="326" my="465">
      <items count="1">
        <item text="OnRemoteHangup" treenode="632474473782255460" />
      </items>
      <linkto id="632474473782255812" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">OnRemoteHangup</ap>
      </Properties>
    </node>
    <node type="Variable" id="632474473782255752" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConferenceId" refType="reference">conferenceId</Properties>
    </node>
    <node type="Variable" id="632474473782255803" name="toDeviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">toDeviceName</Properties>
    </node>
    <node type="Variable" id="632474473782255804" name="fromDeviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">fromDeviceName</Properties>
    </node>
    <node type="Variable" id="632474473782255805" name="toDeviceResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">toDeviceResults</Properties>
    </node>
    <node type="Variable" id="632474473782255806" name="fromDeviceResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">fromDeviceResults</Properties>
    </node>
    <node type="Variable" id="632767263316801807" name="mmsId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" initWith="MmsId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">mmsId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632474473782255454" treenode="632474473782255455" appnode="632474473782255452" handlerfor="632474473782255451">
    <node type="Start" id="632474473782255454" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="296">
      <linkto id="632474473782255509" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782255503" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="248" y="296">
      <linkto id="632474473782255504" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callerCallId</ap>
        <rd field="ConnectionId">g_callerConnectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632474473782255504" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="320" y="280" mx="373" my="296">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632474473782255486" />
        <item text="OnPlay_Failed" treenode="632474473782255491" />
      </items>
      <linkto id="632474473782255516" type="Labeled" style="Vector" ortho="true" label="Success" />
      <linkto id="632474473782255557" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">system_can_not_call.wav</ap>
        <ap name="ConnectionId" type="variable">g_callerConnectionId</ap>
        <ap name="UserData" type="literal">SystemFailure</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255509" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="144" y="296">
      <linkto id="632474473782255503" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="variable">reason</ap>
        <log condition="entry" on="true" level="Warning" type="literal">Call could not be made for call recording application.</log>
      </Properties>
    </node>
    <node type="Comment" id="632474473782255510" text="The caller is still ringing: hang up his/her call" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="104" y="208" />
    <node type="Comment" id="632474473782255511" text="Mark the reason why the call failed, &#xD;&#xA;ending the call record, and clean up." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="384" y="208" />
    <node type="Action" id="632474473782255516" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="552" y="296">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632474473782255557" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="368" y="440">
      <linkto id="632474473782255558" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callerCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255558" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="368" y="528">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632474473782255517" name="reason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="EndReason" refType="reference">reason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632474473782255459" treenode="632474473782255460" appnode="632474473782255457" handlerfor="632474473782255456">
    <node type="Start" id="632474473782255459" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="405">
      <linkto id="632512530479221924" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782255524" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="531" y="404">
      <linkto id="632474473782255546" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="literal">Normal</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255541" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="747" y="388" mx="784" my="404">
      <items count="1">
        <item text="ClearPhone" />
      </items>
      <linkto id="632512530479221925" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="ipAddress" type="variable">g_fromIpAddress</ap>
        <ap name="FunctionName" type="literal">ClearPhone</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255546" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="619" y="388" mx="656" my="404">
      <items count="1">
        <item text="ClearPhone" />
      </items>
      <linkto id="632474473782255541" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="ipAddress" type="variable">g_toIpAddress</ap>
        <ap name="FunctionName" type="literal">ClearPhone</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255547" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="254" y="404">
      <linkto id="632474473782255548" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_calleeCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255548" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="387" y="404">
      <linkto id="632474473782255524" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callerCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255644" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="883" y="531">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632495971237096699" text="Hangup now does a stopmediaop" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="147" y="618" />
    <node type="Action" id="632512530479221924" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="134" y="405">
      <linkto id="632474473782255547" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <ap name="Value2" type="csharp">true</ap>
        <ap name="Value3" type="csharp">true</ap>
        <rd field="ResultData">g_shutdownCommencing</rd>
        <rd field="ResultData2">g_callerHungup</rd>
        <rd field="ResultData3">g_calleeHungup</rd>
      </Properties>
    </node>
    <node type="Action" id="632512530479221925" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="883" y="404">
      <linkto id="632512530479221926" type="Labeled" style="Vector" ortho="true" label="true" />
      <linkto id="632474473782255644" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_startingRecording</ap>
      </Properties>
    </node>
    <node type="Action" id="632512530479221926" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1017" y="404">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632474473782255485" treenode="632474473782255486" appnode="632474473782255483" handlerfor="632474473782255482">
    <node type="Start" id="632474473782255485" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="384">
      <linkto id="632474473782255549" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782255549" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="208" y="384">
      <linkto id="632474473782255550" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callerCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255550" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="384" y="384">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632474473782255490" treenode="632474473782255491" appnode="632474473782255488" handlerfor="632474473782255487">
    <node type="Start" id="632474473782255490" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="400">
      <linkto id="632474473782255551" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782255551" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="128" y="400">
      <linkto id="632474473782255552" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callerCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255552" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="304" y="400">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HandleCommandRequest" startnode="632474473782255580" treenode="632474473782255581" appnode="632474473782255578" handlerfor="632474473782255577">
    <node type="Start" id="632474473782255580" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="392">
      <linkto id="632474473782255651" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Comment" id="632474473782255650" text="Determine command type" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="48" y="336" />
    <node type="Action" id="632474473782255651" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="184" y="392">
      <linkto id="632474473782255652" type="Labeled" style="Vector" ortho="true" label="Start" />
      <linkto id="632474473782255654" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">queryParams["command"]</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255652" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="336" y="288">
      <linkto id="632474473782255655" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <rd field="ResultData">g_saveMedia</rd>
      </Properties>
    </node>
    <node type="Action" id="632474473782255654" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="346" y="496">
      <linkto id="632474473782255657" type="Labeled" style="Vector" ortho="true" label="false" />
      <linkto id="632495971237096700" type="Labeled" style="Vector" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_recordingConnectionId != 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255655" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="539" y="271" mx="610" my="287">
      <items count="1">
        <item text="HandlePhoneNegotiation" />
      </items>
      <linkto id="632474473782255656" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="command" type="csharp">queryParams["command"] == "Start" ? "Stop" : "Start"</ap>
        <ap name="ipAddress" type="variable">remoteHost</ap>
        <ap name="perspective" type="csharp">queryParams["perspective"]</ap>
        <ap name="both" type="csharp">true</ap>
        <ap name="FunctionName" type="literal">HandlePhoneNegotiation</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255656" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="752" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632474473782255657" text="J" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="344" y="616" />
    <node type="Label" id="632474473782255658" text="J" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="608" y="184">
      <linkto id="632474473782255655" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782255659" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="608" y="496">
      <linkto id="632474473782255655" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="literal">0</ap>
        <rd field="ResultData">g_userRecordStop</rd>
        <rd field="ResultData2">g_recordingConnectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632495971237096700" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="461" y="496">
      <linkto id="632474473782255659" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_recordingConnectionId</ap>
      </Properties>
    </node>
    <node type="Variable" id="632474473782255670" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">queryParams</Properties>
    </node>
    <node type="Variable" id="632474473782255671" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HandleKeysRequest" startnode="632474473782255585" treenode="632474473782255586" appnode="632474473782255583" handlerfor="632474473782255582">
    <node type="Start" id="632474473782255585" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="376">
      <linkto id="632474473782255672" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782255672" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="112" y="361" mx="183" my="377">
      <items count="1">
        <item text="HandlePhoneNegotiation" />
      </items>
      <linkto id="632474473782255673" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="command" type="csharp">queryParams["command"]</ap>
        <ap name="both" type="csharp">false</ap>
        <ap name="ipAddress" type="variable">remoteHost</ap>
        <ap name="perspective" type="csharp">queryParams["perspective"]</ap>
        <ap name="FunctionName" type="literal">HandlePhoneNegotiation</ap>
        <log condition="entry" on="true" level="Info" type="literal">Handling Key Request</log>
      </Properties>
    </node>
    <node type="Action" id="632474473782255673" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="328" y="376">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632474473782255676" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">queryParams</Properties>
    </node>
    <node type="Variable" id="632474473782255677" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Complete" startnode="632474473782255756" treenode="632474473782255757" appnode="632474473782255754" handlerfor="632474473782255753">
    <node type="Start" id="632474473782255756" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="72" y="352">
      <linkto id="632474473782256254" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782256252" name="MoveMediaToAppSever" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="464" y="216">
      <linkto id="632474473782256253" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_toUserId</ap>
        <ap name="MediaFilename" type="variable">filename</ap>
        <ap name="MediaServerIp" type="variable">g_mediaServerIp</ap>
        <rd field="MediafileUrl">toUserMediaRelpathUrl</rd>
        <log condition="entry" on="true" level="Verbose" type="literal">Moving media file to Applicaion Server</log>
      </Properties>
    </node>
    <node type="Action" id="632474473782256253" name="WriteRecordingStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="632" y="216">
      <linkto id="632474473782256256" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RecordingsId" type="variable">g_toRecordingRecordId</ap>
        <ap name="Filepath" type="variable">toUserMediaRelpathUrl</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782256254" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="193" y="352">
      <linkto id="632474473782256255" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <ap name="Value2" type="csharp">false</ap>
        <rd field="ResultData">g_recordingDone</rd>
        <rd field="ResultData2">g_startingRecording</rd>
        <log condition="entry" on="true" level="Verbose" type="literal">Record Audio Complete</log>
      </Properties>
    </node>
    <node type="Action" id="632474473782256255" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="335" y="216">
      <linkto id="632474473782256252" type="Labeled" style="Vector" ortho="true" label="true" />
      <linkto id="632474473782256256" type="Labeled" style="Vector" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">(g_toUserRecord || g_toUserAwareOfRecord) &amp;&amp; g_saveMedia</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782256256" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="337" y="472">
      <linkto id="632474473782256259" type="Labeled" style="Vector" ortho="true" label="false" />
      <linkto id="632474473782256258" type="Labeled" style="Vector" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">(g_fromUserRecord || g_fromUserAwareOfRecord) &amp;&amp; g_saveMedia</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782256257" name="WriteRecordingStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="632" y="472">
      <linkto id="632474473782256261" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RecordingsId" type="variable">g_fromRecordingRecordId</ap>
        <ap name="Filepath" type="variable">fromUserMediaRelpathUrl</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782256258" name="MoveMediaToAppSever" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="480" y="472">
      <linkto id="632474473782256257" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_fromUserId</ap>
        <ap name="MediaFilename" type="variable">filename</ap>
        <ap name="MediaServerIp" type="variable">g_mediaServerIp</ap>
        <rd field="MediafileUrl">fromUserMediaRelpathUrl</rd>
      </Properties>
    </node>
    <node type="Label" id="632474473782256259" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="336" y="584" />
    <node type="Label" id="632474473782256260" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="728" y="216">
      <linkto id="632474473782256261" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782256261" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="729" y="333">
      <linkto id="632474473782256265" type="Labeled" style="Vector" ortho="true" label="true" />
      <linkto id="632474473782256263" type="Labeled" style="Vector" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_saveMedia</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782256262" name="DeleteRecordingRecord" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="728" y="600">
      <linkto id="632474473782256264" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RecordingsId" type="variable">g_toRecordingRecordId</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782256263" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="728" y="472">
      <linkto id="632474473782256262" type="Labeled" style="Vector" ortho="true" label="true" />
      <linkto id="632474473782256264" type="Labeled" style="Vector" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_toRecordingRecordId != 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782256264" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="856" y="600">
      <linkto id="632474473782256267" type="Labeled" style="Vector" ortho="true" label="true" />
      <linkto id="632474473782256265" type="Labeled" style="Vector" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_fromRecordingRecordId != 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782256265" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="976" y="335">
      <linkto id="632474473782256266" type="Labeled" style="Vector" ortho="true" label="false" />
      <linkto id="632474473782256315" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_userRecordStop &amp;&amp; !g_shutdownCommencing</ap>
      </Properties>
    </node>
    <node type="Label" id="632474473782256266" text="J" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="976" y="224" />
    <node type="Action" id="632474473782256267" name="DeleteRecordingRecord" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="976" y="600">
      <linkto id="632474473782256265" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RecordingsId" type="variable">g_fromRecordingRecordId</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782256268" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1744" y="608">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632474473782256269" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1624" y="511">
      <linkto id="632474473782256268" type="Labeled" style="Vector" ortho="true" label="false" />
      <linkto id="632474473782256270" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_calleeHungup &amp;&amp; g_callerHungup &amp;&amp; g_recordingDone</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782256270" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1752" y="440">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632474473782256271" text="J" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1624" y="400">
      <linkto id="632474473782256269" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782256272" name="WriteRecordingStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1464" y="333">
      <linkto id="632474473782256277" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="MediaType" type="csharp">Metreos.ApplicationSuite.Storage.MediaFileType.Wav</ap>
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="UsersId" type="variable">g_toUserId</ap>
        <rd field="RecordingsId">g_toRecordingRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632474473782256273" name="WriteRecordingStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1472" y="509">
      <linkto id="632474473782256269" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="MediaType" type="csharp">Metreos.ApplicationSuite.Storage.MediaFileType.Wav</ap>
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="UsersId" type="variable">g_fromUserId</ap>
        <rd field="RecordingsId">g_fromRecordingRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632474473782256274" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1222" y="335">
      <linkto id="632474473782256276" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">false</ap>
        <ap name="Value2" type="csharp">false</ap>
        <ap name="Value3" type="csharp">true</ap>
        <rd field="ResultData">g_recordingDone</rd>
        <rd field="ResultData2">g_saveMedia</rd>
        <rd field="ResultData3">g_startingRecording</rd>
      </Properties>
    </node>
    <node type="Label" id="632474473782256275" text="J" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1336" y="597" />
    <node type="Action" id="632474473782256276" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1336" y="334">
      <linkto id="632474473782256272" type="Labeled" style="Vector" ortho="true" label="true" />
      <linkto id="632474473782256277" type="Labeled" style="Vector" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_toUserRecord || g_toUserAwareOfRecord</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782256277" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1336" y="485">
      <linkto id="632474473782256273" type="Labeled" style="Vector" ortho="true" label="true" />
      <linkto id="632474473782256275" type="Labeled" style="Vector" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_fromUserRecord || g_fromUserAwareOfRecord</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782256315" name="Record" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1045" y="319" mx="1105" my="335">
      <items count="2">
        <item text="OnRecord_Complete" treenode="632474473782255757" />
        <item text="OnRecord_Failed" treenode="632474473782255762" />
      </items>
      <linkto id="632474473782256274" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="AudioFileFormat" type="literal">wav</ap>
        <ap name="CommandTimeout" type="variable">g_record_max_time</ap>
        <ap name="Expires" type="literal">0</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="TermCondMaxTime" type="variable">g_record_max_time</ap>
        <ap name="TermCondSilence" type="variable">g_record_silence_time</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="ConnectionId">g_recordingConnectionId</rd>
      </Properties>
    </node>
    <node type="Variable" id="632474473782256338" name="filename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Filename" refType="reference" name="Metreos.MediaControl.Record_Complete">filename</Properties>
    </node>
    <node type="Variable" id="632474473782256339" name="toUserMediaRelpathUrl" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">toUserMediaRelpathUrl</Properties>
    </node>
    <node type="Variable" id="632474473782256340" name="fromUserMediaRelpathUrl" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">fromUserMediaRelpathUrl</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Failed" startnode="632474473782255761" treenode="632474473782255762" appnode="632474473782255759" handlerfor="632474473782255758">
    <node type="Start" id="632474473782255761" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="144">
      <linkto id="632474473782256326" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782256319" name="WriteRecordingStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="400" y="144">
      <linkto id="632474473782256321" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RecordingsId" type="variable">g_toRecordingRecordId</ap>
        <ap name="Filepath" type="csharp">null</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782256320" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="264" y="144">
      <linkto id="632474473782256319" type="Labeled" style="Vector" ortho="true" label="true" />
      <linkto id="632474473782256321" type="Labeled" style="Vector" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_toUserId != 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782256321" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="264" y="392">
      <linkto id="632474473782256322" type="Labeled" style="Vector" ortho="true" label="true" />
      <linkto id="632474473782256324" type="Labeled" style="Vector" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_fromUserId != 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782256322" name="WriteRecordingStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="400" y="392">
      <linkto id="632474473782256324" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RecordingsId" type="variable">g_fromRecordingRecordId</ap>
        <ap name="Filepath" type="csharp">null</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782256323" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="520" y="648">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632474473782256324" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="400" y="552">
      <linkto id="632474473782256323" type="Labeled" style="Vector" ortho="true" label="false" />
      <linkto id="632474473782256325" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_calleeHungup &amp;&amp; g_callerHungup &amp;&amp; g_recordingDone</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782256325" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="528" y="480">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632474473782256326" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="144" y="145">
      <linkto id="632474473782256320" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <ap name="Value2" type="csharp">false</ap>
        <rd field="ResultData">g_recordingDone</rd>
        <rd field="ResultData2">g_startingRecording</rd>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ClearPhone" startnode="632474473782255544" treenode="632474473782255545" appnode="632474473782255542" handlerfor="632474473782255758">
    <node type="Start" id="632474473782255544" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="400">
      <linkto id="632474473782255564" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782255561" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="440" y="304">
      <linkto id="632474473782255562" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">Key:Services</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632474473782255562" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="616" y="304">
      <linkto id="632474473782255563" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">ipAddress</ap>
        <ap name="Username" type="variable">g_ccm_device_username</ap>
        <ap name="Password" type="variable">g_ccm_device_password</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255563" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="776" y="400">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632474473782255564" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="160" y="400">
      <linkto id="632474473782255563" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632474473782255565" type="Labeled" style="Vector" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">ipAddress != "NONE"</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255565" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="288" y="304">
      <linkto id="632474473782255561" type="Labeled" style="Vector" ortho="true" label="true" />
      <linkto id="632474473782255566" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">((ipAddress == g_toIpAddress) &amp;&amp; g_toUserAwareOfRecord) || ((ipAddress == g_fromIpAddress) &amp;&amp; g_fromUserAwareOfRecord)</ap>
      </Properties>
    </node>
    <node type="Label" id="632474473782255566" text="J" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="288" y="208" />
    <node type="Label" id="632474473782255567" text="J" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="776" y="296">
      <linkto id="632474473782255563" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Variable" id="632474473782255575" name="ipAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ipAddress" refType="reference">ipAddress</Properties>
    </node>
    <node type="Variable" id="632474473782255576" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HandlePhoneNegotiation" startnode="632474473782255648" treenode="632474473782255649" appnode="632474473782255646" handlerfor="632474473782255758">
    <node type="Start" id="632474473782255648" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="360">
      <linkto id="632474473782255679" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Comment" id="632474473782255678" text="Based on the IP Address specified (or lack of),&#xD;&#xA;we decide on which/both phone(s) to send to, &#xD;&#xA;and how.&#xD;&#xA;&#xD;&#xA;If no IP address is specified, we perform 2 SendExecutes.&#xD;&#xA;&#xD;&#xA;If an IP Address is specified and the both parameter is true,&#xD;&#xA;we perform a send execute to the other phone, and directly &#xD;&#xA;send a response to the phone with the specified IP address.&#xD;&#xA;&#xD;&#xA;If an IP address is specified and the both flag is false,&#xD;&#xA;we perform the response only to that specified IP address.&#xD;&#xA;&#xD;&#xA;No responses or SendExecutes are performed if the &#xD;&#xA;recordingVisible is false on a given user." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="40" />
    <node type="Action" id="632474473782255679" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="472" y="360">
      <linkto id="632474473782255686" type="Labeled" style="Vector" ortho="true" label="SendExecuteBoth" />
      <linkto id="632474473782255688" type="Labeled" style="Vector" ortho="true" label="SendExecuteToUser" />
      <linkto id="632474473782255690" type="Labeled" style="Vector" ortho="true" label="SendExecuteFromUser" />
      <linkto id="632474473782255692" type="Labeled" style="Vector" ortho="true" label="SendResponseToUser" />
      <linkto id="632474473782255693" type="Labeled" style="Vector" ortho="true" label="SendResponseFromUser" />
      <linkto id="632474473782255696" type="Labeled" style="Vector" ortho="true" label="SendResponseToUser_SendExecuteFromUser" />
      <linkto id="632474473782255699" type="Labeled" style="Vector" ortho="true" label="SendResponseFromUser_SendExecuteToUser" />
      <linkto id="632474473782255705" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632474473782255680" type="Labeled" style="Vector" ortho="true" label="DoNothing" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Verbose" type="csharp">"\nBoth:        " + both + "\n" + "Perspective: " + perspective + "\n" + "Command:     " + softKeyName + "\n" + "IpAddress:   " + ipAddress</log>public static string Execute(
            string ipAddress, 
            bool both, 
		string g_toNumber,
		string g_fromNumber,
            string g_toIpAddress, 
            string g_fromIpAddress,
            string perspective,
            bool g_toUserAwareOfRecord,
            bool g_fromUserAwareOfRecord,
            ref string textMessage,
            ref string softKeyName)
        {
            bool sendExecuteTo = g_toUserAwareOfRecord &amp;&amp; g_toIpAddress != "NONE";
            bool sendExecuteFrom = g_fromUserAwareOfRecord &amp;&amp; g_fromIpAddress != "NONE";
		
            // Strip off any port information from the ipAddress
            int portIndex = ipAddress.IndexOf(':');
            if(portIndex != -1)
            {
                ipAddress = ipAddress.Substring(0, portIndex);
            }		

            // softKeyName can be equal to "Start" or "Stop" or "NONE"

            if(softKeyName == "Stop" &amp;&amp; perspective == "SELF")
                textMessage = "Recording initiated.";

            else if(softKeyName == "Stop" &amp;&amp; perspective == "OTHER")
                textMessage = "The other end initiated recording.";

            else if(softKeyName == "Stop" &amp;&amp; perspective == "NONE")
                textMessage= "Recording initiated.";

            else if(softKeyName == "Start" &amp;&amp; perspective == "SELF")
                textMessage = "Recording stopped.";

            else if(softKeyName == "Start" &amp;&amp; perspective == "OTHER")
                textMessage = "The other end stopped the recording.";

            else if(softKeyName == "Start" &amp;&amp; perspective == "NONE")
                textMessage = "Recording enabled.";

		else 
		    textMessage = String.Empty;

		if(ipAddress == g_toIpAddress)
		{
			textMessage += "\nFrom " + g_fromNumber;
		}
		else
		{
			textMessage += "\nTo " + g_toNumber;
		}

            // No IpAddress specified, so send to both
            if(ipAddress == "NONE")
            {  
                if(sendExecuteTo &amp;&amp; sendExecuteFrom)
                    return "SendExecuteBoth";

                else if(sendExecuteTo &amp;&amp; !sendExecuteFrom)
                    return "SendExecuteToUser";

                else if(!sendExecuteTo &amp;&amp; sendExecuteFrom)
                    return "SendExecuteFromUser";

                else
                    return "DoNothing";
            }
            else 
            {
                // Another way to think of "NONE" is the first run through the application
                if(perspective == "NONE")
                {
                    if(g_toIpAddress == ipAddress)
                    {
                        return "SendResponseToUser";
                    }
                    else if(g_fromIpAddress == ipAddress)
                    {
                        return "SendResponseFromUser";
                    }
                } 
                // Another way to think of "SELF" is a softkey initiated phone request
                else if(perspective == "SELF")
                {
                    if(g_toIpAddress == ipAddress)
                    {
                        if(g_fromUserAwareOfRecord)
                        {
                            return "SendResponseToUser_SendExecuteFromUser";
                        }
                        else
                        {
                            Console.WriteLine("HHAHAHAH");
                            return "SendResponseToUser";
                        }
                    }
                    else if(g_fromIpAddress == ipAddress)
                    {
                        if(g_toUserAwareOfRecord)
                        {
                            return "SendResponseFromUser_SendExecuteToUser";
                        }
                        else
                        {
                            return "SendResponseFromUser";
                        }
                    } 
                }
                // Another way to think of "OTHER" is SendExecute-initaited phone request
                else if(perspective == "OTHER")
                {
                    if(g_toIpAddress == ipAddress)
                    {
                        return "SendResponseToUser";
                    }
                    else if(g_fromIpAddress == ipAddress)
                    {
                        return "SendResponseFromUser";
                    }
                }             
            }

            return "error";
        }</Properties>
    </node>
    <node type="Action" id="632474473782255680" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="480" y="672">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632474473782255682" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="752" y="48">
      <linkto id="632474473782255683" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="parentRoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="applicationServerIpAndPort" type="csharp"> g_applicationServerIp + ':' + g_applicationServerPort</ap>
        <ap name="phoneIp" type="variable">g_toIpAddress</ap>
        <ap name="perspective" type="csharp">perspective == "NONE" ? "NONE" : "OTHER";</ap>
        <ap name="method" type="literal">InternalSendEvent</ap>
        <ap name="command" type="variable">softKeyName</ap>
        <ap name="EventName" type="literal">Metreos.Providers.Http.GotRequest</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">SendEvent to both</log>
      </Properties>
    </node>
    <node type="Action" id="632474473782255683" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="912" y="48">
      <linkto id="632474473782255702" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="parentRoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="applicationServerIpAndPort" type="csharp"> g_applicationServerIp + ':' + g_applicationServerPort</ap>
        <ap name="phoneIp" type="variable">g_fromIpAddress</ap>
        <ap name="perspective" type="csharp">perspective == "NONE" ? "NONE" : "OTHER";</ap>
        <ap name="method" type="literal">InternalSendEvent</ap>
        <ap name="command" type="variable">softKeyName</ap>
        <ap name="EventName" type="literal">Metreos.Providers.Http.GotRequest</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255684" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="752" y="136">
      <linkto id="632474473782255703" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="parentRoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="applicationServerIpAndPort" type="csharp"> g_applicationServerIp + ':' + g_applicationServerPort</ap>
        <ap name="phoneIp" type="variable">g_toIpAddress</ap>
        <ap name="perspective" type="csharp">perspective == "NONE" ? "NONE" : "OTHER";</ap>
        <ap name="method" type="literal">InternalSendEvent</ap>
        <ap name="command" type="variable">softKeyName</ap>
        <ap name="EventName" type="literal">Metreos.Providers.Http.GotRequest</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Send Event to the 'To User'</log>
      </Properties>
    </node>
    <node type="Action" id="632474473782255685" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="752" y="232">
      <linkto id="632474473782255704" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="parentRoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="applicationServerIpAndPort" type="csharp"> g_applicationServerIp + ':' + g_applicationServerPort</ap>
        <ap name="phoneIp" type="variable">g_fromIpAddress</ap>
        <ap name="perspective" type="csharp">perspective == "NONE" ? "NONE" : "OTHER";</ap>
        <ap name="method" type="literal">InternalSendEvent</ap>
        <ap name="command" type="variable">softKeyName</ap>
        <ap name="EventName" type="literal">Metreos.Providers.Http.GotRequest</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Send Event to the 'From User'</log>
      </Properties>
    </node>
    <node type="Label" id="632474473782255686" text="1" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="472" y="48" />
    <node type="Label" id="632474473782255687" text="1" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="640" y="48">
      <linkto id="632474473782255682" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Label" id="632474473782255688" text="2" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="512" y="136" />
    <node type="Label" id="632474473782255689" text="2" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="640" y="136">
      <linkto id="632474473782255684" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Label" id="632474473782255690" text="3" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="536" y="232" />
    <node type="Label" id="632474473782255691" text="3" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="640" y="232">
      <linkto id="632474473782255685" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Label" id="632474473782255692" text="4" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="672" y="360" />
    <node type="Label" id="632474473782255693" text="4" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="672" y="440" />
    <node type="Label" id="632474473782255694" text="4" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="792" y="360">
      <linkto id="632474473782255695" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782255695" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="928" y="360">
      <linkto id="632474473782255706" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">RapidRecord</ap>
        <ap name="Prompt" type="literal">Recording Options</ap>
        <ap name="Text" type="variable">textMessage</ap>
        <rd field="ResultData">text</rd>
        <log condition="entry" on="true" level="Verbose" type="literal">Send Response</log>
      </Properties>
    </node>
    <node type="Label" id="632474473782255696" text="6" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="680" y="824" />
    <node type="Label" id="632474473782255697" text="6" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="808" y="824">
      <linkto id="632474473782255698" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782255698" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="952" y="824">
      <linkto id="632474473782255711" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="parentRoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="applicationServerIpAndPort" type="csharp"> g_applicationServerIp + ':' + g_applicationServerPort</ap>
        <ap name="phoneIp" type="variable">g_fromIpAddress</ap>
        <ap name="perspective" type="csharp">perspective == "NONE" ? "NONE" : "OTHER";</ap>
        <ap name="method" type="literal">InternalSendEvent</ap>
        <ap name="command" type="variable">softKeyName</ap>
        <ap name="EventName" type="literal">Metreos.Providers.Http.GotRequest</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Send Response 'To User'  + Send Execute 'From User'</log>
      </Properties>
    </node>
    <node type="Label" id="632474473782255699" text="5" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="688" y="664" />
    <node type="Label" id="632474473782255700" text="5" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="808" y="664">
      <linkto id="632474473782255701" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632474473782255701" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="952" y="664">
      <linkto id="632474473782255710" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="parentRoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="applicationServerIpAndPort" type="csharp"> g_applicationServerIp + ':' + g_applicationServerPort</ap>
        <ap name="phoneIp" type="variable">g_toIpAddress</ap>
        <ap name="perspective" type="csharp">perspective == "NONE" ? "NONE" : "OTHER";</ap>
        <ap name="method" type="literal">InternalSendEvent</ap>
        <ap name="command" type="variable">softKeyName</ap>
        <ap name="EventName" type="literal">Metreos.Providers.Http.GotRequest</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Send Response 'From User' + Send Execute 'To User'</log>
      </Properties>
    </node>
    <node type="Action" id="632474473782255702" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1032" y="48">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632474473782255703" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="912" y="136">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632474473782255704" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="912" y="232">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632474473782255705" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="200" y="472">
      <linkto id="632474473782255680" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">ipAddress</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Undefined error</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255706" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1064" y="360">
      <linkto id="632474473782255709" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">softKeyName</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">"http://" +  g_applicationServerIp + ':' + g_applicationServerPort + "/RecordingCommand?metreosSessionId=" + g_routingGuid + "&amp;perspective=SELF" + "&amp;command=" + softKeyName</ap>
        <rd field="ResultData">text</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"http://" +  g_applicationServerIp + ':' + g_applicationServerPort + "/RecordingCommand?metreosSessionId=" + g_routingGuid + "&amp;perspective=" + perspective + "&amp;command=" + softKeyName</log>
      </Properties>
    </node>
    <node type="Action" id="632474473782255707" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1320" y="360">
      <linkto id="632474473782255708" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">ipAddress</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632474473782255708" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1448" y="360">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632474473782255709" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1200" y="360">
      <linkto id="632474473782255707" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Label" id="632474473782255710" text="4" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1120" y="664" />
    <node type="Label" id="632474473782255711" text="4" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1120" y="824" />
    <node type="Variable" id="632474473782255746" name="ipAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ipAddress" defaultInitWith="NONE" refType="reference">ipAddress</Properties>
    </node>
    <node type="Variable" id="632474473782255747" name="both" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" initWith="both" refType="reference">both</Properties>
    </node>
    <node type="Variable" id="632474473782255748" name="perspective" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="perspective" defaultInitWith="NONE" refType="reference">perspective</Properties>
    </node>
    <node type="Variable" id="632474473782255749" name="textMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">textMessage</Properties>
    </node>
    <node type="Variable" id="632474473782255750" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632474473782255751" name="softKeyName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="command" refType="reference">softKeyName</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>