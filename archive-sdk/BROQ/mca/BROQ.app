<Application name="BROQ" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="BROQ">
    <outline>
      <treenode type="evh" id="632732619173444329" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632732619173444326" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632732619173444325" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632732619173444357" level="2" text="Metreos.MediaControl.Play_Complete: OnPlayGreeting_Complete">
        <node type="function" name="OnPlayGreeting_Complete" id="632732619173444352" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632732619173444355" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632793997228777680" actid="632732619173444348" />
          <ref id="632793997228777780" actid="632736035018051284" />
        </references>
        <Properties type="asyncCallback">
          <ep name="UserData" type="literal">greeting</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632732619173444369" level="2" text="Metreos.MediaControl.Play_Failed: OnPlayGreeting_Failed">
        <node type="function" name="OnPlayGreeting_Failed" id="632732619173444364" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632732619173444367" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632793997228777681" actid="632732619173444348" />
          <ref id="632793997228777781" actid="632736035018051284" />
        </references>
        <Properties type="asyncCallback">
          <ep name="UserData" type="literal">greeting</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632732619173444376" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632732619173444373" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632732619173444372" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632793997228777785" actid="632736035018051762" />
          <ref id="632793997228777798" actid="632736367389367271" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632732619173444381" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632732619173444378" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632732619173444377" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632793997228777786" actid="632736035018051762" />
          <ref id="632793997228777799" actid="632736367389367271" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632732619173444386" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632732619173444383" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632732619173444382" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632793997228777787" actid="632736035018051762" />
          <ref id="632793997228777800" actid="632736367389367271" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632754171020704644" level="2" text="Metreos.MediaControl.Play_Complete: OnPlayConfirmation_Complete">
        <node type="function" name="OnPlayConfirmation_Complete" id="632754171020704641" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632754171020704640" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632793997228777703" actid="632754171020704688" />
        </references>
        <Properties type="asyncCallback">
          <ep name="UserData" type="literal">confirm</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632754171020704649" level="2" text="Metreos.MediaControl.Play_Failed: OnPlayConfirmation_Failed">
        <node type="function" name="OnPlayConfirmation_Failed" id="632754171020704646" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632754171020704645" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632793997228777704" actid="632754171020704688" />
        </references>
        <Properties type="asyncCallback">
          <ep name="UserData" type="literal">confirm</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632755165891457764" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632755165891457761" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632755165891457760" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632755165891457770" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632755165891457767" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632755165891457766" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632755165891457776" level="2" text="Metreos.CallControl.GotDigits: OnGotDigits">
        <node type="function" name="OnGotDigits" id="632755165891457773" path="Metreos.StockTools" />
        <node type="event" name="GotDigits" id="632755165891457772" path="Metreos.CallControl.GotDigits" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632761375934535544" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632761375934535541" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632761375934535540" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632778377920126519" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632778377920126516" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632778377920126515" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632793997228777717" actid="632778377920126525" />
          <ref id="632793997228777762" actid="632778377920126868" />
          <ref id="632793997228777793" actid="632778377920127250" />
        </references>
        <Properties type="asyncCallback">
          <ep name="UserData" type="literal">noReps</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632778377920126524" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632778377920126521" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632778377920126520" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632793997228777718" actid="632778377920126525" />
          <ref id="632793997228777763" actid="632778377920126868" />
          <ref id="632793997228777794" actid="632778377920127250" />
        </references>
        <Properties type="asyncCallback">
          <ep name="UserData" type="literal">noReps</ep>
        </Properties>
      </treenode>
      <treenode type="fun" id="632733443517611623" level="1" text="OpenDBconnection">
        <node type="function" name="OpenDBconnection" id="632733443517611620" path="Metreos.StockTools" />
        <calls>
          <ref actid="632733443517611619" />
        </calls>
      </treenode>
      <treenode type="fun" id="632733740850633188" level="1" text="Failover">
        <node type="function" name="Failover" id="632733740850633185" path="Metreos.StockTools" />
        <calls>
          <ref actid="632733740850633184" />
        </calls>
      </treenode>
      <treenode type="fun" id="632736367389367270" level="1" text="MakeSupportCalls">
        <node type="function" name="MakeSupportCalls" id="632736367389367267" path="Metreos.StockTools" />
        <calls>
          <ref actid="632778377920126880" />
        </calls>
      </treenode>
      <treenode type="fun" id="632736367389367292" level="1" text="HangUpPhones">
        <node type="function" name="HangUpPhones" id="632736367389367289" path="Metreos.StockTools" />
        <calls>
          <ref actid="632736367389367300" />
          <ref actid="632754171020704661" />
        </calls>
      </treenode>
      <treenode type="fun" id="632755235320319588" level="1" text="SendEmail">
        <node type="function" name="SendEmail" id="632755235320319585" path="Metreos.StockTools" />
        <calls>
          <ref actid="632778377920127259" />
          <ref actid="632755235320319602" />
        </calls>
      </treenode>
      <treenode type="fun" id="632757630277268903" level="1" text="GetSupportNumsToDial">
        <node type="function" name="GetSupportNumsToDial" id="632757630277268900" path="Metreos.StockTools" />
        <calls>
          <ref actid="632778377920126886" />
        </calls>
      </treenode>
      <treenode type="fun" id="632778377920126876" level="1" text="PrepareSupportCalls">
        <node type="function" name="PrepareSupportCalls" id="632778377920126873" path="Metreos.StockTools" />
        <calls>
          <ref actid="632778377920126872" />
          <ref actid="632778377920127247" />
          <ref actid="632778377920127248" />
        </calls>
      </treenode>
      <treenode type="fun" id="632791332348387861" level="1" text="MakeJIRAEntry">
        <node type="function" name="MakeJIRAEntry" id="632791332348387858" path="Metreos.StockTools" />
        <calls>
          <ref actid="632791332348387857" />
          <ref actid="632793997228777981" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_inboundCallId" id="632793997228777610" vid="632732619173444330">
        <Properties type="String">g_inboundCallId</Properties>
      </treenode>
      <treenode text="g_inboundConnId" id="632793997228777612" vid="632732619173444336">
        <Properties type="String">g_inboundConnId</Properties>
      </treenode>
      <treenode text="g_outboundConnId" id="632793997228777614" vid="632732619173444472">
        <Properties type="String">g_outboundConnId</Properties>
      </treenode>
      <treenode text="g_outboundCallId" id="632793997228777616" vid="632732619173444474">
        <Properties type="String">g_outboundCallId</Properties>
      </treenode>
      <treenode text="g_confId" id="632793997228777618" vid="632732619173444476">
        <Properties type="String">g_confId</Properties>
      </treenode>
      <treenode text="g_outboundCalls" id="632793997228777620" vid="632732619173444480">
        <Properties type="ArrayList">g_outboundCalls</Properties>
      </treenode>
      <treenode text="db_poolConnections" id="632793997228777622" vid="632733459209400231">
        <Properties type="Bool" defaultInitWith="true" initWith="DbConnPooling">db_poolConnections</Properties>
      </treenode>
      <treenode text="db_ConnectionName" id="632793997228777624" vid="632733459209400233">
        <Properties type="String" initWith="DbConnectionName">db_ConnectionName</Properties>
      </treenode>
      <treenode text="db_Master_DbName" id="632793997228777626" vid="632733459209400235">
        <Properties type="String" initWith="DbName">db_Master_DbName</Properties>
      </treenode>
      <treenode text="db_Master_DbServer" id="632793997228777628" vid="632733459209400331">
        <Properties type="String" initWith="Server">db_Master_DbServer</Properties>
      </treenode>
      <treenode text="db_Master_Port" id="632793997228777630" vid="632733459209400333">
        <Properties type="UInt" initWith="Port">db_Master_Port</Properties>
      </treenode>
      <treenode text="db_Master_Username" id="632793997228777632" vid="632733459209400335">
        <Properties type="String" initWith="Username">db_Master_Username</Properties>
      </treenode>
      <treenode text="db_Master_Password" id="632793997228777634" vid="632733459209400337">
        <Properties type="String" initWith="Password">db_Master_Password</Properties>
      </treenode>
      <treenode text="db_Slave_DbName" id="632793997228777636" vid="632733459209400339">
        <Properties type="String" initWith="SlaveDBName">db_Slave_DbName</Properties>
      </treenode>
      <treenode text="db_Slave_DbServer" id="632793997228777638" vid="632733459209400341">
        <Properties type="String" initWith="SlaveDBServerAddress">db_Slave_DbServer</Properties>
      </treenode>
      <treenode text="db_Slave_Port" id="632793997228777640" vid="632733459209400343">
        <Properties type="UInt" initWith="SlaveDBServerPort">db_Slave_Port</Properties>
      </treenode>
      <treenode text="db_Slave_Username" id="632793997228777642" vid="632733459209400345">
        <Properties type="String" initWith="SlaveDBServerUsername">db_Slave_Username</Properties>
      </treenode>
      <treenode text="db_Slave_Password" id="632793997228777644" vid="632733459209400347">
        <Properties type="String" initWith="SlaveDBServerPassword">db_Slave_Password</Properties>
      </treenode>
      <treenode text="g_DbWriteEnabled" id="632793997228777646" vid="632733459209400350">
        <Properties type="Bool" defaultInitWith="true">g_DbWriteEnabled</Properties>
      </treenode>
      <treenode text="g_FailoverSupportNumbers" id="632793997228777648" vid="632736035018051276">
        <Properties type="ArrayList" initWith="FailoverSupportNumbers">g_FailoverSupportNumbers</Properties>
      </treenode>
      <treenode text="g_SupportStaffUserNames" id="632793997228777650" vid="632736035018051780">
        <Properties type="ArrayList" initWith="SupportStaffUserNames">g_SupportStaffUserNames</Properties>
      </treenode>
      <treenode text="g_from" id="632793997228777652" vid="632736258434522872">
        <Properties type="String" defaultInitWith="null">g_from</Properties>
      </treenode>
      <treenode text="g_callTimeStart" id="632793997228777654" vid="632736258434523028">
        <Properties type="DateTime">g_callTimeStart</Properties>
      </treenode>
      <treenode text="g_callTimeEnd" id="632793997228777656" vid="632736258434523030">
        <Properties type="DateTime">g_callTimeEnd</Properties>
      </treenode>
      <treenode text="g_callAnswered" id="632793997228777658" vid="632736367389367285">
        <Properties type="Bool" defaultInitWith="false">g_callAnswered</Properties>
      </treenode>
      <treenode text="g_SupportStaffEmailAddress" id="632793997228777660" vid="632754171020704451">
        <Properties type="String" defaultInitWith="support@metreos.com" initWith="SupportStaffEmailAddress">g_SupportStaffEmailAddress</Properties>
      </treenode>
      <treenode text="g_CallIDtoUserID" id="632793997228777662" vid="632754171020704630">
        <Properties type="Hashtable">g_CallIDtoUserID</Properties>
      </treenode>
      <treenode text="g_intCallsAttempted" id="632793997228777664" vid="632754171020704676">
        <Properties type="Int" defaultInitWith="0">g_intCallsAttempted</Properties>
      </treenode>
      <treenode text="g_intCallsHungUp" id="632793997228777666" vid="632754273892655822">
        <Properties type="Int" defaultInitWith="0">g_intCallsHungUp</Properties>
      </treenode>
      <treenode text="g_htSupportStaffIDtoUsername" id="632793997228777668" vid="632754418062850952">
        <Properties type="Hashtable">g_htSupportStaffIDtoUsername</Properties>
      </treenode>
      <treenode text="db_unavailable" id="632793997228777670" vid="632755106743946539">
        <Properties type="Bool" defaultInitWith="false">db_unavailable</Properties>
      </treenode>
      <treenode text="g_answeredBy" id="632793997228777672" vid="632755235320319600">
        <Properties type="String" defaultInitWith="Caller hung up">g_answeredBy</Properties>
      </treenode>
      <treenode text="g_mmsId" id="632793997228777674" vid="632767263316802546">
        <Properties type="UInt">g_mmsId</Properties>
      </treenode>
      <treenode text="g_remoteIssue" id="632793997228777676" vid="632791332348388206">
        <Properties type="WebServices.NativeTypes.JiraSoapServiceService.RemoteIssueType">g_remoteIssue</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632732619173444328" treenode="632732619173444329" appnode="632732619173444326" handlerfor="632732619173444325">
    <node type="Start" id="632732619173444328" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="60.75" y="218.75">
      <linkto id="632736258434522875" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632732619173444334" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="318.25" y="215.75">
      <linkto id="632732619173444348" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_inboundCallId</ap>
        <rd field="MmsId">g_mmsId</rd>
        <rd field="ConnectionId">g_inboundConnId</rd>
      </Properties>
    </node>
    <node type="Action" id="632732619173444348" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="349.25" y="199" mx="418" my="215">
      <items count="2">
        <item text="OnPlayGreeting_Complete" treenode="632732619173444357" />
        <item text="OnPlayGreeting_Failed" treenode="632732619173444369" />
      </items>
      <linkto id="632732619173444491" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">Please hold while we connect you to the first available support representative.</ap>
        <ap name="ConnectionId" type="variable">g_inboundConnId</ap>
        <ap name="Prompt1" type="literal">Thank you for contacting Metreos.</ap>
        <ap name="UserData" type="literal">greeting</ap>
        <log condition="entry" on="true" level="Info" type="literal">Playing greeting to incoming call</log>
      </Properties>
    </node>
    <node type="Action" id="632732619173444491" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="634.75" y="218">
      <linkto id="632736035018051931" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632778377920126872" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_SupportStaffUserNames.Count == 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632733443517611619" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="170.427734" y="59" mx="221" my="75">
      <items count="1">
        <item text="OpenDBconnection" />
      </items>
      <linkto id="632754418062850962" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632755130397238115" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">OpenDBconnection</ap>
        <log condition="entry" on="true" level="Info" type="literal">Connecting and opening database</log>
      </Properties>
    </node>
    <node type="Action" id="632733740850633184" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="591.2425" y="60" mx="625" my="76">
      <items count="1">
        <item text="Failover" />
      </items>
      <linkto id="632736035018051772" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Failover</ap>
        <log condition="entry" on="true" level="Warning" type="literal">Database could not be opened so attempting to use failover numbers</log>
      </Properties>
    </node>
    <node type="Label" id="632736035018051772" text="X" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="770.75" y="78.25" />
    <node type="Label" id="632736035018051931" text="X" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="773" y="218.25" />
    <node type="Action" id="632736258434522875" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="62" y="76">
      <linkto id="632733443517611619" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">from</ap>
        <ap name="Value2" type="variable">inboundCallId</ap>
        <rd field="ResultData">g_from</rd>
        <rd field="ResultData2">g_inboundCallId</rd>
        <log condition="entry" on="true" level="Info" type="literal">Timestamping beginning of call</log>
      </Properties>
    </node>
    <node type="Action" id="632754418062850962" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="219" y="216">
      <linkto id="632732619173444334" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="exit" on="true" level="Info" type="csharp">"Time-stamping calTimeStart at: " + g_callTimeStart</log>
public static string Execute(SessionData sessionData, string db_ConnectionName, LogWriter log, ref DateTime g_callTimeStart)
{
	IDbConnection conn = sessionData.DbConnections[db_ConnectionName];

	conn.Open();

	using(IDbCommand command = conn.CreateCommand())
	{
		command.CommandText = "select convert_tz(NOW(), 'SYSTEM', 'US/Central')";
		object obj = command.ExecuteScalar();
		g_callTimeStart = (DateTime) obj;	
	}

	conn.Close();

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632755130397238115" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="458" y="76">
      <linkto id="632733740850633184" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">db_unavailable</rd>
      </Properties>
    </node>
    <node type="Comment" id="632755235320319604" text="callTimeStart stamp" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="161.9707" y="246" />
    <node type="Comment" id="632757630277268597" text="set:&#xD;&#xA;g_from&#xD;&#xA;g_inboundCallId" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="63" y="112" />
    <node type="Comment" id="632757630277268598" text="set to true:&#xD;&#xA;db_unavailable" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="414" y="32" />
    <node type="Comment" id="632757630277268599" text="play greeting to inbound call" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="341" y="177" />
    <node type="Comment" id="632757630277268600" text="SupportStaffUserNames == 0" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="555" y="182" />
    <node type="Action" id="632778377920126872" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="583.0398" y="290" mx="637" my="306">
      <items count="1">
        <item text="PrepareSupportCalls" />
      </items>
      <linkto id="632778377920126900" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">PrepareSupportCalls</ap>
      </Properties>
    </node>
    <node type="Action" id="632778377920126900" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="634.3175" y="419.25">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632778377920126901" text="X" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="772.8175" y="419.25">
      <linkto id="632778377920126900" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Variable" id="632732619173444335" name="inboundCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">inboundCallId</Properties>
    </node>
    <node type="Variable" id="632732619173444478" name="outboundCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">outboundCallId</Properties>
    </node>
    <node type="Variable" id="632736258434522874" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">from</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632732619173444375" treenode="632732619173444376" appnode="632732619173444373" handlerfor="632732619173444372">
    <node type="Start" id="632732619173444375" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="101">
      <linkto id="632754171020704688" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632754171020704688" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="39" y="85" mx="117" my="101">
      <items count="2">
        <item text="OnPlayConfirmation_Complete" treenode="632754171020704644" />
        <item text="OnPlayConfirmation_Failed" treenode="632754171020704649" />
      </items>
      <linkto id="632754368995253037" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="csharp">"Support call from " + g_from</ap>
        <ap name="State" type="variable">outboundCallId</ap>
        <ap name="Prompt3" type="csharp">"Support call from " + g_from</ap>
        <ap name="ConnectionId" type="variable">outboundConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="csharp">"Support call from " + g_from</ap>
        <ap name="UserData" type="literal">confirm</ap>
        <log condition="entry" on="true" level="Info" type="literal">Playing call confirmation on outbound call to support rep</log>
      </Properties>
    </node>
    <node type="Action" id="632754368995253037" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="243" y="101">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632757630277268596" text="play confirmation to rep" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="59" y="63" />
    <node type="Variable" id="632732619173444471" name="outboundConnId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">outboundConnId</Properties>
    </node>
    <node type="Variable" id="632732619173444484" name="outboundCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">outboundCallId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632732619173444380" treenode="632732619173444381" appnode="632732619173444378" handlerfor="632732619173444377">
    <node type="Start" id="632732619173444380" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="163" y="162">
      <linkto id="632778377920126504" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632778377920126503" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="642" y="295">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632778377920126504" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="162.998688" y="295">
      <linkto id="632778377920126505" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="csharp">"Calls Attempted: " +g_intCallsAttempted + "; Calls Hung Up: " + g_intCallsHungUp</log>
public static string Execute(ref int g_intCallsHungUp)
{
	//increment number of calls hung up
	g_intCallsHungUp++;

	return IApp.VALUE_SUCCESS;

}
</Properties>
    </node>
    <node type="Action" id="632778377920126505" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="314.998657" y="295">
      <linkto id="632778377920126503" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632778377920126525" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_intCallsAttempted == g_intCallsHungUp</ap>
      </Properties>
    </node>
    <node type="Comment" id="632778377920126507" text="inc #calls hung up" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="114" y="325" />
    <node type="Comment" id="632778377920126508" text="g_intCallsAttempted == g_intCallsHungUp" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="206" y="261" />
    <node type="Action" id="632778377920127247" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="488.0398" y="332" mx="542" my="348">
      <items count="1">
        <item text="PrepareSupportCalls" />
      </items>
      <linkto id="632778377920126503" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">PrepareSupportCalls</ap>
      </Properties>
    </node>
    <node type="Action" id="632778377920126525" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="368" y="332" mx="416" my="348">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632778377920126519" />
        <item text="OnPlay_Failed" treenode="632778377920126524" />
      </items>
      <linkto id="632778377920127247" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_inboundConnId</ap>
        <ap name="Prompt1" type="literal">Please continue to hold while a support representative becomes available</ap>
        <ap name="UserData" type="literal">noReps</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632732619173444385" treenode="632732619173444386" appnode="632732619173444383" handlerfor="632732619173444382">
    <node type="Start" id="632732619173444385" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="150">
      <linkto id="632736035018051769" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632732619173444501" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="781" y="149">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632736035018051769" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="167" y="150">
      <linkto id="632736035018051771" type="Labeled" style="Bezier" ortho="true" label="unequal" />
      <linkto id="632736367389367300" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_inboundCallId</ap>
        <log condition="entry" on="true" level="Info" type="literal">Deciding which call to hang up...</log>
      </Properties>
    </node>
    <node type="Action" id="632736035018051771" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="167" y="32">
      <linkto id="632754273892655825" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_inboundCallId</ap>
        <log condition="entry" on="true" level="Info" type="literal">outbound call hung up so system will hang up the inbound call</log>
      </Properties>
    </node>
    <node type="Action" id="632736367389367300" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="286.6338" y="134" mx="329" my="150">
      <items count="1">
        <item text="HangUpPhones" />
      </items>
      <linkto id="632778377920127258" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="DoNotHangUp" type="variable">callId</ap>
        <ap name="FunctionName" type="literal">HangUpPhones</ap>
      </Properties>
    </node>
    <node type="Action" id="632754273892655825" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="326" y="32">
      <linkto id="632736367389367300" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(int g_intCallsHungUp)
{
	//increment number of calls hung up
	g_intCallsHungUp++;

	return IApp.VALUE_SUCCESS;

}
</Properties>
    </node>
    <node type="Action" id="632754418062851180" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="674.90625" y="94.25">
      <linkto id="632732619173444501" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(SessionData sessionData, string db_ConnectionName, LogWriter log, ref DateTime g_callTimeEnd)
{
	IDbConnection conn = sessionData.DbConnections[db_ConnectionName];

	conn.Open();

	using(IDbCommand command = conn.CreateCommand())
	{
		command.CommandText = "select convert_tz(NOW(), 'SYSTEM', 'US/Central')";
		object obj = command.ExecuteScalar();
		g_callTimeEnd = (DateTime) obj;	
	}

	conn.Close();

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632755106743946541" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="571" y="149">
      <linkto id="632754418062851180" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632732619173444501" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">db_unavailable</ap>
      </Properties>
    </node>
    <node type="Comment" id="632755984114474322" text="callTimeEnd stamp" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="622" y="58" />
    <node type="Comment" id="632757630277268569" text="inc #of calls hung up" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="276" y="65" />
    <node type="Comment" id="632757630277268570" text="if db_unavailable" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="522" y="182" />
    <node type="Comment" id="632757630277268571" text="compare callId to inboundCallId" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="77" y="184" />
    <node type="Action" id="632778377920127258" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="427" y="150">
      <linkto id="632755106743946541" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632791332348387857" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_confId == ""</ap>
      </Properties>
    </node>
    <node type="Action" id="632778377920127259" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="540.6981" y="279" mx="574" my="295">
      <items count="1">
        <item text="SendEmail" />
      </items>
      <linkto id="632755106743946541" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">SendEmail</ap>
      </Properties>
    </node>
    <node type="Action" id="632791332348387857" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="387.661072" y="279" mx="429" my="295">
      <items count="1">
        <item text="MakeJIRAEntry" />
      </items>
      <linkto id="632778377920127259" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">MakeJIRAEntry</ap>
        <log condition="entry" on="true" level="Info" type="literal">CallFunction: MakeJIRA Entry</log>
      </Properties>
    </node>
    <node type="Comment" id="632793997228777983" text="g_confId == &quot;&quot;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="389.927246" y="182" />
    <node type="Variable" id="632736035018051768" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayConfirmation_Complete" startnode="632754171020704643" treenode="632754171020704644" appnode="632754171020704641" handlerfor="632754171020704640">
    <node type="Start" id="632754171020704643" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="76">
      <linkto id="632754171020704655" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632754171020704655" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="96" y="76">
      <linkto id="632754171020704672" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632754418062850947" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">termCondition == "maxdigits"</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Checking if call confirmation button was pressed"</log>
      </Properties>
    </node>
    <node type="Action" id="632754171020704657" name="CreateConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="525.998657" y="366">
      <linkto id="632754418062850943" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="SoundToneOnJoin" type="literal">False</ap>
        <ap name="ConnectionId" type="variable">g_outboundConnId</ap>
        <rd field="ConferenceId">g_confId</rd>
        <log condition="entry" on="true" level="Info" type="literal">Creating conference with outbound call leg</log>
      </Properties>
    </node>
    <node type="Action" id="632754171020704658" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="873.5" y="365">
      <linkto id="632793997228777981" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_inboundConnId</ap>
        <ap name="ConferenceId" type="variable">g_confId</ap>
        <log condition="entry" on="true" level="Info" type="literal">Joining inbound call leg to the conference</log>
      </Properties>
    </node>
    <node type="Action" id="632754171020704659" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="872" y="76">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632754171020704660" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="207.998688" y="237">
      <linkto id="632755165891457758" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">outboundConnId</ap>
        <ap name="Value2" type="variable">outboundCallId</ap>
        <ap name="Value3" type="literal">true</ap>
        <rd field="ResultData">g_outboundConnId</rd>
        <rd field="ResultData2">g_outboundCallId</rd>
        <rd field="ResultData3">g_callAnswered</rd>
      </Properties>
    </node>
    <node type="Action" id="632754171020704661" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="367.9987" y="349" mx="410" my="365">
      <items count="1">
        <item text="HangUpPhones" />
      </items>
      <linkto id="632754171020704657" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="DoNotHangUp" type="variable">outboundCallId</ap>
        <ap name="FunctionName" type="literal">HangUpPhones</ap>
        <log condition="entry" on="true" level="Info" type="literal">Call confirmation button was pressed... begin hanging up non-answered phones</log>
      </Properties>
    </node>
    <node type="Action" id="632754171020704663" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="500.9987" y="237">
      <linkto id="632754171020704661" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">Determining which rep answered the call</log>
public static string Execute(String outboundCallId, Hashtable g_CallIDtoUserID, Hashtable g_htSupportStaffIDtoUsername, ref String g_answeredBy)
{
	//resolve who answered the phone
	String userID = (String) g_CallIDtoUserID[outboundCallId];
	if (userID == null) return IApp.VALUE_FAILURE;
	g_answeredBy = (String) g_htSupportStaffIDtoUsername[userID];
	return IApp.VALUE_SUCCESS;

}
</Properties>
    </node>
    <node type="Action" id="632754171020704672" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="252.998688" y="76">
      <linkto id="632754368995253038" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">outboundCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632754368995253038" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="390.9987" y="76">
      <linkto id="632754368995253039" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref int g_intCallsHungUp)
{
	//increment number of calls hung up
	g_intCallsHungUp++;

	return IApp.VALUE_SUCCESS;

}
</Properties>
    </node>
    <node type="Action" id="632754368995253039" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="539.998657" y="76">
      <linkto id="632754171020704659" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632778377920126868" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_intCallsAttempted == g_intCallsHungUp</ap>
      </Properties>
    </node>
    <node type="Action" id="632754418062850943" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="689" y="366">
      <linkto id="632754171020704658" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_inboundConnId</ap>
        <ap name="Block" type="literal">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632754418062850947" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="96" y="237">
      <linkto id="632754418062850949" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632754171020704660" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_callAnswered</ap>
      </Properties>
    </node>
    <node type="Action" id="632754418062850949" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="96" y="352">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632755165891457758" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="318.9987" y="237">
      <linkto id="632754171020704661" type="Labeled" style="Bevel" ortho="true" label="true" />
      <linkto id="632754171020704663" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">db_unavailable</ap>
      </Properties>
    </node>
    <node type="Action" id="632755235320319602" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="841.1168" y="140" mx="874" my="156">
      <items count="1">
        <item text="SendEmail" />
      </items>
      <linkto id="632754171020704659" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">SendEmail</ap>
      </Properties>
    </node>
    <node type="Comment" id="632755984114474323" text="resolve username that answered call" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="411.733673" y="197" />
    <node type="Comment" id="632757630277268585" text="if maxdigits" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="63" y="106" />
    <node type="Comment" id="632757630277268586" text="if call already answered" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="35" y="267" />
    <node type="Comment" id="632757630277268587" text="set:&#xD;&#xA;g_outboundConnId&#xD;&#xA;g_outboundCallId&#xD;&#xA;g_callAnswered" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="148" y="162" />
    <node type="Comment" id="632757630277268588" text="if db_unavailable" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="269" y="202" />
    <node type="Comment" id="632757630277268589" text="hangup outboundCallId" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="181" y="40" />
    <node type="Comment" id="632757630277268590" text="inc #calls hung up" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="343" y="106" />
    <node type="Comment" id="632757630277268591" text="g_intCallsAttempted == g_intCallsHungUp" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="435" y="42" />
    <node type="Comment" id="632757630277268593" text="create conf w/ rep" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="475" y="329" />
    <node type="Comment" id="632757630277268594" text="add inbound call to conf" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="809" y="395" />
    <node type="Action" id="632778377920126868" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="605.541" y="139.5" mx="653" my="156">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632778377920126519" />
        <item text="OnPlay_Failed" treenode="632778377920126524" />
      </items>
      <linkto id="632778377920127248" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_inboundConnId</ap>
        <ap name="Prompt1" type="literal">Please continue to hold while a support representative becomes available</ap>
        <ap name="UserData" type="literal">noReps</ap>
      </Properties>
    </node>
    <node type="Action" id="632778377920127248" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="706.86676" y="140" mx="761" my="156">
      <items count="1">
        <item text="PrepareSupportCalls" />
      </items>
      <linkto id="632754171020704659" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">PrepareSupportCalls</ap>
      </Properties>
    </node>
    <node type="Action" id="632793997228777981" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="833" y="241" mx="874" my="257">
      <items count="1">
        <item text="MakeJIRAEntry" />
      </items>
      <linkto id="632755235320319602" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">MakeJIRAEntry</ap>
        <log condition="entry" on="true" level="Info" type="literal">CallFunction: MakeJIRA Entry</log>
      </Properties>
    </node>
    <node type="Variable" id="632754171020704653" name="outboundConnId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.MediaControl.Play_Complete">outboundConnId</Properties>
    </node>
    <node type="Variable" id="632754171020704654" name="termCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Play_Complete">termCondition</Properties>
    </node>
    <node type="Variable" id="632754171020704671" name="outboundCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="state" refType="reference">outboundCallId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="MakeSupportCalls" startnode="632736367389367269" treenode="632736367389367270" appnode="632736367389367267" handlerfor="632778377920126520">
    <node type="Loop" id="632736367389367278" name="Loop" text="loop (var)" cx="449" cy="254" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="142" y="88" mx="366" my="215">
      <linkto id="632736367389367271" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632736367389367283" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="variable">dialList</Properties>
    </node>
    <node type="Start" id="632736367389367269" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="215">
      <linkto id="632736367389367278" port="1" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632736367389367271" name="MakeCall" container="632736367389367278" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="192" y="198" mx="252" my="214">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632732619173444376" />
        <item text="OnMakeCall_Failed" treenode="632732619173444381" />
        <item text="OnRemoteHangup" treenode="632732619173444386" />
      </items>
      <linkto id="632736367389367272" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="csharp">loopEnum.Current</ap>
        <ap name="From" type="variable">g_from</ap>
        <ap name="DisplayName" type="csharp">"BROQ: "+ g_from </ap>
        <ap name="MmsId" type="variable">g_mmsId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">outboundCallId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"Calling: " + loopEnum.Current</log>
      </Properties>
    </node>
    <node type="Action" id="632736367389367272" name="CustomCode" container="632736367389367278" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="477" y="213">
      <linkto id="632736367389367278" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(String userID, String outboundCallId, ArrayList g_outboundCalls, Hashtable g_CallIDtoUserID, ref int g_intCallsAttempted, StringCollection dialList)
{
	//extra code to get rid of warning that states dialList is unreferenced
	//even though it is actually used for determining #of loop iterations
	dialList = dialList;

	//add outboundCallID and userID to hashtable
	g_CallIDtoUserID[outboundCallId] = userID;

	//accumulate numbers to dial in the array
	g_outboundCalls.Add(outboundCallId);
	
	//increment number of calls attempted
	g_intCallsAttempted++; 

	return IApp.VALUE_SUCCESS;

}
</Properties>
    </node>
    <node type="Action" id="632736367389367283" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="685" y="215">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Comment" id="632757630277268577" text="make call to loopEnum.Current" container="632736367389367278" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="166" y="182" />
    <node type="Comment" id="632757630277268578" text="make outboundCallID, userID hashtable&#xD;&#xA;accum # to dial in array&#xD;&#xA;inc # of calls attempted" container="632736367389367278" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="376" y="246" />
    <node type="Comment" id="632757630277268579" text="loop 1..dialList" container="632736367389367278" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="147" y="104" />
    <node type="Variable" id="632736367389367279" name="outboundCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">outboundCallId</Properties>
    </node>
    <node type="Variable" id="632736367389367280" name="dialList" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="StringCollection" initWith="DialList" refType="reference">dialList</Properties>
    </node>
    <node type="Variable" id="632754171020704634" name="userID" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserID" refType="reference">userID</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HangUpPhones" startnode="632736367389367291" treenode="632736367389367292" appnode="632736367389367289" handlerfor="632778377920126520">
    <node type="Loop" id="632736367389367297" name="Loop" text="loop (var)" cx="511" cy="244" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="149" y="42" mx="404" my="164">
      <linkto id="632736367389367293" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632736367389367303" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="variable">g_outboundCalls</Properties>
    </node>
    <node type="Start" id="632736367389367291" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="164">
      <linkto id="632791332348388209" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632736367389367293" name="Compare" container="632736367389367297" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="240" y="165">
      <linkto id="632736367389367294" type="Labeled" style="Bezier" ortho="true" label="unequal" />
      <linkto id="632736367389367297" port="4" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">loopEnum.Current</ap>
        <ap name="Value2" type="variable">dnh</ap>
      </Properties>
    </node>
    <node type="Action" id="632736367389367294" name="Hangup" container="632736367389367297" class="MaxActionNode" group="" path="Metreos.CallControl" x="370" y="165">
      <linkto id="632736367389367301" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="csharp">loopEnum.Current</ap>
      </Properties>
    </node>
    <node type="Action" id="632736367389367299" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="765" y="294">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632736367389367301" name="CustomCode" container="632736367389367297" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="521" y="164">
      <linkto id="632736367389367297" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(StringCollection hungUpCalls, IEnumerator loopEnum, int g_intCallsHungUp)
{
	//add currently hung up call to hungUpCalls list
	hungUpCalls.Add(loopEnum.Current as String);
	
	//increment number of hung up calls
	g_intCallsHungUp++;

	return IApp.VALUE_SUCCESS;

}
</Properties>
    </node>
    <node type="Action" id="632736367389367303" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="764" y="165">
      <linkto id="632736367389367299" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(StringCollection hungUpCalls, ArrayList g_outboundCalls)
{
	//removes calls that were hung up from the outboundCalls list
	foreach(string callId in hungUpCalls)
		g_outboundCalls.Remove(callId);
	
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Comment" id="632757630277268572" text="loop 1..g_outboundcalls" container="632736367389367297" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="154" y="57" />
    <node type="Comment" id="632757630277268573" text="compare dnh to loopEnum.Current" container="632736367389367297" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="152" y="130" />
    <node type="Comment" id="632757630277268574" text="add hung up call to hungUpCalls list&#xD;&#xA;inc #of hung up calls" container="632736367389367297" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="419" y="106" />
    <node type="Comment" id="632757630277268575" text="remove hung up calls from outboundCalls list" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="648" y="125" />
    <node type="Action" id="632791332348388209" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="106" y="164">
      <linkto id="632736367389367297" port="1" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632791332348388211" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_outboundCalls.Count == 0</ap>
      </Properties>
    </node>
    <node type="Label" id="632791332348388210" text="j" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="678" y="294">
      <linkto id="632736367389367299" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632791332348388211" text="j" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="107" y="304" />
    <node type="Variable" id="632736367389367298" name="dnh" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DoNotHangUp" refType="reference">dnh</Properties>
    </node>
    <node type="Variable" id="632736367389367302" name="hungUpCalls" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="StringCollection" refType="reference">hungUpCalls</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="SendEmail" activetab="true" startnode="632755235320319587" treenode="632755235320319588" appnode="632755235320319585" handlerfor="632778377920126520">
    <node type="Start" id="632755235320319587" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="98">
      <linkto id="632755235320319589" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632755235320319589" name="Send" class="MaxActionNode" group="" path="Metreos.Native.Mail" x="144" y="98">
      <linkto id="632755235320319591" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="To" type="variable">g_SupportStaffEmailAddress</ap>
        <ap name="From" type="literal">BROQ@metreos.com</ap>
        <ap name="MailServer" type="literal">springfield.metreos.com</ap>
        <ap name="Subject" type="csharp">"[BROQ CDR] From: " + g_from + " To: " + g_answeredBy</ap>
        <ap name="Body" type="csharp">"BROQ Call Detail \n\n\nFrom:\t\t\t\t" + g_from + "\nAnswered By:\t\t\t" + g_answeredBy + "\nCall Time (CST):\t\t" + g_callTimeStart + "\n\nhttp://jira.metreos.com/jira/secure/EditIssue!default.jspa?id=" + g_remoteIssue.Value.id</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Sending email with CDR [From: "+ g_from +" ] [To: "+ g_answeredBy +"] [Time: " + g_callTimeStart +"]"</log>
        <log condition="success" on="true" level="Info" type="literal">send mail succeded</log>
        <log condition="failure" on="true" level="Error" type="literal">send mail failed</log>
      </Properties>
    </node>
    <node type="Action" id="632755235320319591" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="270" y="98">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">success</ap>
      </Properties>
    </node>
    <node type="Comment" id="632757630277268576" text="Send email using: &#xD;&#xA; - g_supportStaffEmailAddress&#xD;&#xA; - g_from&#xD;&#xA; - g_answeredBy&#xD;&#xA; - g_callTimeStart" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="77" y="135" />
  </canvas>
  <canvas type="Function" name="GetSupportNumsToDial" startnode="632757630277268902" treenode="632757630277268903" appnode="632757630277268900" handlerfor="632778377920126520">
    <node type="Start" id="632757630277268902" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="89">
      <linkto id="632757630277268906" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632757630277268906" name="GetUserCurrentDateTime" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="180" y="89">
      <linkto id="632757630277268911" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">userId</ap>
        <rd field="DateTime">userTime</rd>
      </Properties>
    </node>
    <node type="Action" id="632757630277268911" name="GetActiveRelayNumbers" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="369" y="90">
      <linkto id="632757630277268913" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632757630277268916" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">userId</ap>
        <ap name="IncludeDisabled" type="literal">true</ap>
        <rd field="NumberTable">numbersTable</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"userID:" + userId</log>
      </Properties>
    </node>
    <node type="Action" id="632757630277268913" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="523.90625" y="90">
      <linkto id="632757630277268915" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632757630277268916" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(DataTable numbersTable, DateTime userTime, StringCollection dialNumList, LogWriter log)
{
	// array list that contains references to rows in numbersTalble
	// that will be removed should it be determined below that they
	// contain entries that we do not want dialed. 
	ArrayList rowsToRemove = new ArrayList();

	// day of week in user's timezone
	DayOfWeek day = userTime.DayOfWeek;

	string name;
	string numberToDial;
	// iterate through all rows in the numbers table
	foreach (DataRow row in numbersTable.Rows)
	{
		name = row[SqlConstants.Tables.ExternalNumbers.Name] as string;
		numberToDial = row[SqlConstants.Tables.ExternalNumbers.PhoneNumber] as string;

		if (name != string.Empty)
		{
			if( name.ToLower().IndexOf("support") == -1 )
			{
				rowsToRemove.Add(row);
				continue;
			}
		}
		else
		{
			rowsToRemove.Add(row);
			continue;
		}
		// make sure that the number to dial is valid, and not equal to the originating number.
 		if (numberToDial == null || numberToDial == string.Empty)
		{
			rowsToRemove.Add(row);
			continue;
		}
	

		// if current entry does not have day/time restrictions
		// we skip it.
		if ( ! Convert.ToBoolean(row[SqlConstants.Tables.ExternalNumbers.TimeOfDayEnabled]))
			continue;
		
		// get day of week setting
		TimeOfDayWeekendValues weekendVal = (TimeOfDayWeekendValues) row[SqlConstants.Tables.ExternalNumbers.TimeOfDayWeekend];
				
		
		switch (day)
		{
			// if it's saturday in the user's timezone...
			case DayOfWeek.Saturday :
			{
				// if the user wants calls on this number on saturdays or
				// saturday and sunday, break out of switch...
				if (weekendVal == TimeOfDayWeekendValues.Saturday || weekendVal == TimeOfDayWeekendValues.SaturdayAndSunday)
					break;
				// otherwise add this row to remove list and move to next entry
				else
				{
					rowsToRemove.Add(row);
					continue;
				}
			}
			case DayOfWeek.Sunday :
			{
				if (weekendVal == TimeOfDayWeekendValues.Sunday || weekendVal == TimeOfDayWeekendValues.SaturdayAndSunday)
					break;
				else
				{
					rowsToRemove.Add(row);
					continue;
				}
			}
			default : break; // weekday
		}

		// retrieve time of day settings for entry
		try
		{
			TimeSpan startTime = (TimeSpan) row[SqlConstants.Tables.ExternalNumbers.TimeOfDayStart];
			TimeSpan stopTime = (TimeSpan) row[SqlConstants.Tables.ExternalNumbers.TimeOfDayEnd];
			TimeSpan currentTime = userTime.TimeOfDay;
		
			// if start time &lt;= stop time, check that the user's current time
			// falls into the range. otherwise, add entry to remove list
			// and continue.
			if (startTime &lt;= stopTime) 
			{
				if (currentTime &gt;= startTime &amp;&amp; currentTime &lt;= stopTime)
					continue;
				else
					rowsToRemove.Add(row);
			}
			// if stopTime &lt; startTime, the stop time is past midnight, and start
			// time is before midnight. thus, we check that the user's current
			// time is NOT in the range of hours between the stop time and the
			// start time. if it is, we add it to the list of rows to remove
			// and continue.
			else
			{
				if ( ! (currentTime &gt;= stopTime &amp;&amp; currentTime &lt;= startTime))
					continue;
				else
					rowsToRemove.Add(row);
			}
		}
		// should a problem occur while dealing with the time of day stuff,
		// we err on the side of getting the call to the user. 
		catch (Exception e)
		{
			log.Write(TraceLevel.Warning, "OnIncomingCall: Exception was thrown while extracting external number start/stop times. Number will be called regardless.");
		}
	}
	
	// remove unnecessary rows from table.
	foreach (DataRow rowToRemove in rowsToRemove)
		numbersTable.Rows.Remove(rowToRemove);
		
	foreach (DataRow row in numbersTable.Rows)
	{
		dialNumList.Add(row[SqlConstants.Tables.ExternalNumbers.PhoneNumber] as string);
	}

	log.Write(TraceLevel.Verbose, "GetSupportNumsToDial: Number of rows in table: " + numbersTable.Rows.Count);
	return (numbersTable.Rows.Count &gt; 0) ? IApp.VALUE_SUCCESS : "EmptyList";
}</Properties>
    </node>
    <node type="Action" id="632757630277268915" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="682" y="89">
      <Properties final="true" type="appControl" log="On">
        <ap name="DialList" type="variable">dialNumList</ap>
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632757630277268916" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="447" y="205">
      <Properties final="true" type="appControl" log="On">
        <ap name="DialList" type="variable">dialNumList</ap>
        <ap name="ReturnValue" type="literal">Failure</ap>
      </Properties>
    </node>
    <node type="Variable" id="632757630277268904" name="userId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserId" refType="reference">userId</Properties>
    </node>
    <node type="Variable" id="632757630277268905" name="dialNumList" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="StringCollection" refType="reference">dialNumList</Properties>
    </node>
    <node type="Variable" id="632757630277268907" name="userTime" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DateTime" refType="reference">userTime</Properties>
    </node>
    <node type="Variable" id="632757630277268912" name="numbersTable" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">numbersTable</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="PrepareSupportCalls" startnode="632778377920126875" treenode="632778377920126876" appnode="632778377920126873" handlerfor="632778377920126520">
    <node type="Loop" id="632778377920126877" name="Loop" text="loop (var)" cx="720" cy="351.75" entry="2" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="42" y="174.375" mx="402" my="350">
      <linkto id="632778377920126879" fromport="2" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632778377920126887" fromport="4" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="variable">g_SupportStaffUserNames</Properties>
    </node>
    <node type="Start" id="632778377920126875" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="203.25" y="104.5">
      <linkto id="632778377920127254" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632778377920126878" name="If" container="632778377920126877" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="490.75" y="351.625">
      <linkto id="632778377920126880" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632778377920126877" port="4" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">empNumbers.Count == 0 </ap>
      </Properties>
    </node>
    <node type="Action" id="632778377920126879" name="GetUserByUsername" container="632778377920126877" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="131.5" y="265.875">
      <linkto id="632778377920126881" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Username" type="csharp">loopEnum.Current</ap>
        <rd field="UsersId">SupportStaffId</rd>
        <log condition="exit" on="true" level="Info" type="csharp">"Getting userID: \"" + SupportStaffId + "\" from username: \"" + loopEnum.Current +"\""</log>
      </Properties>
    </node>
    <node type="Action" id="632778377920126880" name="CallFunction" container="632778377920126877" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="564.6543" y="334.875" mx="613" my="351">
      <items count="1">
        <item text="MakeSupportCalls" />
      </items>
      <linkto id="632778377920126877" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="DialList" type="variable">empNumbers</ap>
        <ap name="UserID" type="variable">SupportStaffId</ap>
        <ap name="FunctionName" type="literal">MakeSupportCalls</ap>
      </Properties>
    </node>
    <node type="Action" id="632778377920126881" name="CustomCode" container="632778377920126877" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="127.7207" y="352.875">
      <linkto id="632778377920126886" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(IEnumerator loopEnum, String SupportStaffId, ref Hashtable g_htSupportStaffIDtoUsername)
{
	//add supportStaffID and Username to hashtable
	g_htSupportStaffIDtoUsername[SupportStaffId] = loopEnum.Current;

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Comment" id="632778377920126882" text="loop 1..SupportStaffUserNames" container="632778377920126877" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="66" y="190.125" />
    <node type="Comment" id="632778377920126883" text="make supportStaffID, username hashtable" container="632778377920126877" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="43" y="388.125" />
    <node type="Comment" id="632778377920126884" text="employee FindMe numbers == 0" container="632778377920126877" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="424" y="313.125" />
    <node type="Comment" id="632778377920126885" text="get AR Enabled FindMe#s&#xD;&#xA;for employee" container="632778377920126877" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="236" y="297.125" />
    <node type="Action" id="632778377920126886" name="CallFunction" container="632778377920126877" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="268.235352" y="335.125" mx="329" my="351">
      <items count="1">
        <item text="GetSupportNumsToDial" />
      </items>
      <linkto id="632778377920126878" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632778377920126877" port="4" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="UserId" type="variable">SupportStaffId</ap>
        <ap name="FunctionName" type="literal">GetSupportNumsToDial</ap>
        <rd field="DialList">empNumbers</rd>
      </Properties>
    </node>
    <node type="Action" id="632778377920126887" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="405.25" y="591.625">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632778377920127254" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="406.25" y="90">
      <linkto id="632778377920126877" port="2" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ArrayList g_outboundCalls, Hashtable g_CallIDtoUserID)
{
	g_outboundCalls.Clear();
	g_CallIDtoUserID.Clear();

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Variable" id="632778377920126908" name="empNumbers" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="StringCollection" refType="reference">empNumbers</Properties>
    </node>
    <node type="Variable" id="632778377920127246" name="SupportStaffId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">SupportStaffId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="MakeJIRAEntry" startnode="632791332348387860" treenode="632791332348387861" appnode="632791332348387858" handlerfor="632778377920126520">
    <node type="Start" id="632791332348387860" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="67">
      <linkto id="632793997228777979" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632791332348388208" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="390" y="68">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632793997228777976" name="login" class="MaxActionNode" group="" path="WebServices.NativeActions.JiraSoapServiceService" x="205" y="68">
      <linkto id="632793997228777977" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="In0" type="literal">dbethke</ap>
        <ap name="In1" type="literal">metreos</ap>
        <rd field="Result">token</rd>
        <log condition="entry" on="true" level="Info" type="literal">JIRA Login via WebService -&gt; creating token</log>
      </Properties>
    </node>
    <node type="Action" id="632793997228777977" name="createIssue" class="MaxActionNode" group="" path="WebServices.NativeActions.JiraSoapServiceService" x="285" y="69">
      <linkto id="632791332348388208" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="In1_metreos_assignee" type="variable">g_answeredBy</ap>
        <ap name="In1_metreos_created" type="variable">g_callTimeStart</ap>
        <ap name="In1_metreos_description" type="literal">Call Details:</ap>
        <ap name="In1_metreos_environment" type="csharp">"[BROQ CDR] From: " + g_from + " To: " + g_answeredBy</ap>
        <ap name="In1_metreos_priority" type="literal">4</ap>
        <ap name="In1_metreos_project" type="literal">CUST</ap>
        <ap name="In1_metreos_reporter" type="variable">g_answeredBy</ap>
        <ap name="In1_metreos_resolution" type="literal">Unresolved</ap>
        <ap name="In1_metreos_status" type="literal">1</ap>
        <ap name="In1_metreos_summary" type="csharp">"[BROQ CDR] From: " + g_from + " To: " + g_answeredBy</ap>
        <ap name="In1_metreos_type" type="literal">6</ap>
        <ap name="In1_metreos_updated" type="variable">g_callTimeStart</ap>
        <ap name="In0" type="variable">token</ap>
        <rd field="Result">g_remoteIssue</rd>
        <log condition="entry" on="true" level="Info" type="literal">Creating JIRA Issue via WebService</log>
      </Properties>
    </node>
    <node type="Action" id="632793997228777979" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="113" y="69">
      <linkto id="632793997228777976" type="Labeled" style="Bezier" label="default" />
      <Properties language="csharp">
public static string Execute(string g_answeredBy, ref DateTime tktDateTime, DateTime g_callTimeStart, string empAnswered)
{
	

	//if caller hung up then set empAnswered to Unknown otherwise it is g_answeredBy
	empAnswered = (g_answeredBy == "Caller hung up" ? "Unknown" : g_answeredBy);

	//create the correct time formatting for the webservice 
	//CCYY-MM-DDTHH:MM:SS Example: 2006-01-01T00:00:00
	//DateTime tmpTime = g_callTimeStart.Year + "-" + g_callTimeStart.Month +"-"+ g_callTimeStart.Day +"T"+ g_callTimeStart.Hour +":"+ g_callTimeStart.Minute +":"+ g_callTimeStart.Second;
	//tktDateTime = tmpTime;
	
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Variable" id="632793992032008410" name="token" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">token</Properties>
    </node>
    <node type="Variable" id="632793997228777978" name="tktDateTime" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DateTime" defaultInitWith="2006-01-01T00:00:00" refType="reference">tktDateTime</Properties>
    </node>
    <node type="Variable" id="632793997228777980" name="empAnswered" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">empAnswered</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <canvas type="Function" name="OnGotDigits" show="false" startnode="632755165891457775" treenode="632755165891457776" appnode="632755165891457773" handlerfor="632755165891457772">
    <node type="Start" id="632755165891457775" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="79">
      <linkto id="632755165891457777" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632755165891457777" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="126" y="82">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" show="false" startnode="632778377920126518" treenode="632778377920126519" appnode="632778377920126516" handlerfor="632778377920126515">
    <node type="Start" id="632778377920126518" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="74" y="158">
      <linkto id="632778377920127255" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632778377920127255" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="359" y="199">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayConfirmation_Failed" show="false" startnode="632754171020704648" treenode="632754171020704649" appnode="632754171020704646" handlerfor="632754171020704645">
    <node type="Start" id="632754171020704648" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="253" y="36">
      <linkto id="632754171020704656" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632754171020704656" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="253" y="127">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Failover" show="false" startnode="632733740850633187" treenode="632733740850633188" appnode="632733740850633185" handlerfor="632778377920126520">
    <node type="Loop" id="632736035018051753" name="Loop" text="loop (var)" cx="341" cy="214" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="430" y="59" mx="600" my="166">
      <linkto id="632736035018051762" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632736035018051767" fromport="4" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="variable">g_FailoverSupportNumbers</Properties>
    </node>
    <node type="Start" id="632733740850633187" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="165">
      <linkto id="632736035018051283" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632736035018051282" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="331.75" y="166">
      <linkto id="632736035018051753" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632778377920127250" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_FailoverSupportNumbers.Count == 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632736035018051283" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="130.792313" y="166.5">
      <linkto id="632736035018051284" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_inboundCallId</ap>
        <rd field="MmsId">g_mmsId</rd>
        <rd field="ConnectionId">g_inboundConnId</rd>
      </Properties>
    </node>
    <node type="Action" id="632736035018051284" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="150.792313" y="149" mx="219" my="165">
      <items count="2">
        <item text="OnPlayGreeting_Complete" treenode="632732619173444357" />
        <item text="OnPlayGreeting_Failed" treenode="632732619173444369" />
      </items>
      <linkto id="632736035018051282" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_inboundConnId</ap>
        <ap name="Prompt1" type="literal">BROQ_greeting.wav</ap>
        <ap name="UserData" type="literal">greeting</ap>
      </Properties>
    </node>
    <node type="Action" id="632736035018051767" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="332.5" y="381.75">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632736035018051335" name="CustomCode" container="632736035018051753" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="641" y="168">
      <linkto id="632736035018051753" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(String outboundCallId, ArrayList g_outboundCalls)
{
	//accumulate numbers to dial in the outboundCalls array
	g_outboundCalls.Add(outboundCallId);

	return IApp.VALUE_SUCCESS;

}
</Properties>
    </node>
    <node type="Action" id="632736035018051762" name="MakeCall" container="632736035018051753" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="456" y="150" mx="516" my="166">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632732619173444376" />
        <item text="OnMakeCall_Failed" treenode="632732619173444381" />
        <item text="OnRemoteHangup" treenode="632732619173444386" />
      </items>
      <linkto id="632736035018051335" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="csharp">loopEnum.Current</ap>
        <ap name="From" type="variable">g_from</ap>
        <ap name="DisplayName" type="csharp">"BROQ: " + g_from</ap>
        <ap name="MmsId" type="variable">g_mmsId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">outboundCallId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"Calling: " + loopEnum.Current</log>
      </Properties>
    </node>
    <node type="Comment" id="632757630277268580" text="loop 1..g_failoverSupportNumbers" container="632736035018051753" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="435" y="77" />
    <node type="Comment" id="632757630277268581" text="accum # to dial in array" container="632736035018051753" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="604" y="199" />
    <node type="Comment" id="632757630277268582" text="make call to loopEnum.Current" container="632736035018051753" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="443" y="129" />
    <node type="Comment" id="632757630277268584" text="FailoverSupportNumbers == 0" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="247" y="128" />
    <node type="Action" id="632778377920127250" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="287" y="240" mx="335" my="256">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632778377920126519" />
        <item text="OnPlay_Failed" treenode="632778377920126524" />
      </items>
      <linkto id="632736035018051767" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">Please call back or send an email to support@metreos.com</ap>
        <ap name="ConnectionId" type="variable">g_inboundCallId</ap>
        <ap name="Prompt1" type="literal">We are sorry but there are no available support representatives at the moment.</ap>
        <ap name="UserData" type="literal">noReps</ap>
      </Properties>
    </node>
    <node type="Variable" id="632736035018051323" name="outboundCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">outboundCallId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OpenDBconnection" show="false" startnode="632733443517611622" treenode="632733443517611623" appnode="632733443517611620" handlerfor="632778377920126520">
    <node type="Start" id="632733443517611622" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="40">
      <linkto id="632733443517611625" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632733443517611624" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="296" y="40">
      <linkto id="632733443517611628" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632733443517611629" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="csharp">dsn + "; connection timeout=1;"</ap>
        <ap name="Name" type="variable">db_ConnectionName</ap>
        <ap name="Type" type="literal">mysql</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OpenDBConnection: attempting connect to Primary</log>
        <log condition="default" on="true" level="Warning" type="literal">OpenDBConnection: Connection to Primary failed.</log>
      </Properties>
    </node>
    <node type="Action" id="632733443517611625" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="120" y="40">
      <linkto id="632733443517611624" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632733443517611629" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">db_Master_DbName</ap>
        <ap name="Server" type="variable">db_Master_DbServer</ap>
        <ap name="Port" type="variable">db_Master_Port</ap>
        <ap name="Username" type="variable">db_Master_Username</ap>
        <ap name="Password" type="variable">db_Master_Password</ap>
        <ap name="Pooling" type="variable">db_poolConnections</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632733443517611626" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="496" y="248">
      <linkto id="632733443517611628" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632733443517611630" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="csharp">dsn + "; connection timeout=1;"</ap>
        <ap name="Name" type="variable">db_ConnectionName</ap>
        <ap name="Type" type="literal">mysql</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OpenDBConnection: attempting connect to Secondary</log>
        <log condition="default" on="true" level="Info" type="literal">OpenDBConnection: Connection to Secondary failed.
</log>
      </Properties>
    </node>
    <node type="Action" id="632733443517611627" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="336" y="248">
      <linkto id="632733443517611626" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632733443517611630" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">db_Slave_DbName</ap>
        <ap name="Server" type="variable">db_Slave_DbServer</ap>
        <ap name="Port" type="variable">db_Slave_Port</ap>
        <ap name="Username" type="variable">db_Slave_Username</ap>
        <ap name="Password" type="variable">db_Slave_Password</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632733443517611628" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="496" y="40">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OpenDBConnection: connection to database established.</log>
      </Properties>
    </node>
    <node type="Action" id="632733443517611629" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="208" y="144">
      <linkto id="632733443517611631" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_DbWriteEnabled</rd>
      </Properties>
    </node>
    <node type="Action" id="632733443517611630" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="417" y="422">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
        <log condition="entry" on="true" level="Error" type="literal">OpenDBConnection: AppSuite DB connections failed. Check application settings.</log>
      </Properties>
    </node>
    <node type="Action" id="632733443517611631" name="SetSessionData" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="336" y="144">
      <linkto id="632733443517611627" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="AllowDBWrite" type="csharp">false</ap>
      </Properties>
    </node>
    <node type="Variable" id="632733459209400349" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" show="false" startnode="632755165891457769" treenode="632755165891457770" appnode="632755165891457767" handlerfor="632755165891457766">
    <node type="Start" id="632755165891457769" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632755165891457771" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632755165891457771" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="197" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayGreeting_Failed" show="false" startnode="632732619173444366" treenode="632732619173444369" appnode="632732619173444364" handlerfor="632732619173444367">
    <node type="Start" id="632732619173444366" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="266" y="32">
      <linkto id="632732619173444370" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632732619173444370" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="382" y="36">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="literal">Error playing initial incoming call greeting</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" show="false" startnode="632778377920126523" treenode="632778377920126524" appnode="632778377920126521" handlerfor="632778377920126520">
    <node type="Start" id="632778377920126523" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="437" y="260">
      <linkto id="632778377920127256" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632778377920127256" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="200" y="40">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" show="false" startnode="632761375934535543" treenode="632761375934535544" appnode="632761375934535541" handlerfor="632761375934535540">
    <node type="Start" id="632761375934535543" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632761375934535545" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632761375934535545" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="170" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayGreeting_Complete" show="false" startnode="632732619173444354" treenode="632732619173444357" appnode="632732619173444352" handlerfor="632732619173444355">
    <node type="Start" id="632732619173444354" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="332" y="32">
      <linkto id="632732619173444500" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632732619173444500" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="233" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" show="false" startnode="632755165891457763" treenode="632755165891457764" appnode="632755165891457761" handlerfor="632755165891457760">
    <node type="Start" id="632755165891457763" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632755165891457765" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632755165891457765" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="187" y="39">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>