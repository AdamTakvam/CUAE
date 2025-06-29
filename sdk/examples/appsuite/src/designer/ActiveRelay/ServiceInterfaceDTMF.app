<Application name="ServiceInterfaceDTMF" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="ServiceInterfaceDTMF">
    <outline>
      <treenode type="evh" id="632527335560196792" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632527335560196789" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632527335560196788" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632527335560196867" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632527335560196864" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632527335560196863" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632804601979661986" actid="632527335560196873" />
          <ref id="632804601979662042" actid="632528071621735656" />
          <ref id="632804601979662051" actid="632528130761068044" />
          <ref id="632804601979662059" actid="632538493910671444" />
          <ref id="632804601979662064" actid="632549474022185571" />
          <ref id="632804601979662069" actid="632555905720470068" />
          <ref id="632804601979662077" actid="632555905720470080" />
          <ref id="632804601979662103" actid="632779433073575536" />
          <ref id="632804601979662128" actid="632779433073575972" />
          <ref id="632804601979662132" actid="632779433073575980" />
          <ref id="632804601979662140" actid="632802867924430994" />
          <ref id="632804601979662156" actid="632779433073575528" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632527335560196872" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632527335560196869" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632527335560196868" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632804601979661987" actid="632527335560196873" />
          <ref id="632804601979662043" actid="632528071621735656" />
          <ref id="632804601979662052" actid="632528130761068044" />
          <ref id="632804601979662060" actid="632538493910671444" />
          <ref id="632804601979662065" actid="632549474022185571" />
          <ref id="632804601979662070" actid="632555905720470068" />
          <ref id="632804601979662078" actid="632555905720470080" />
          <ref id="632804601979662104" actid="632779433073575536" />
          <ref id="632804601979662129" actid="632779433073575972" />
          <ref id="632804601979662133" actid="632779433073575980" />
          <ref id="632804601979662141" actid="632802867924430994" />
          <ref id="632804601979662157" actid="632779433073575528" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632527335560197530" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632527335560197527" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632527335560197526" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632804601979662007" actid="632527335560197536" />
          <ref id="632804601979662017" actid="632528071621735662" />
          <ref id="632804601979662023" actid="632538493910671414" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632527335560197535" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632527335560197532" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632527335560197531" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632804601979662008" actid="632527335560197536" />
          <ref id="632804601979662018" actid="632528071621735662" />
          <ref id="632804601979662024" actid="632538493910671414" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632538425211820328" level="2" text="Metreos.Providers.TimerFacility.TimerFire: OnTimerFire">
        <node type="function" name="OnTimerFire" id="632538425211820325" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632538425211820324" path="Metreos.Providers.TimerFacility.TimerFire" />
        <references>
          <ref id="632804601979662150" actid="632538493910671462" />
        </references>
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="632550392566729311" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632550392566729308" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632550392566729307" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632555905720469428" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632555905720469425" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632555905720469424" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632555905720469433" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632555905720469430" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632555905720469429" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632593879973704241" level="2" text="Metreos.CallControl.GotDigits: OnGotDigits">
        <node type="function" name="OnGotDigits" id="632593879973704238" path="Metreos.StockTools" />
        <node type="event" name="GotDigits" id="632593879973704237" path="Metreos.CallControl.GotDigits" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632593879973704247" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632593879973704244" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632593879973704243" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632779433073574714" level="2" text="Metreos.Events.ActiveRelay.FindMeServiceRequestResp: OnFindMeServiceRequestResp">
        <node type="function" name="OnFindMeServiceRequestResp" id="632779433073574711" path="Metreos.StockTools" />
        <node type="event" name="FindMeServiceRequestResp" id="632779433073574710" path="Metreos.Events.ActiveRelay.FindMeServiceRequestResp" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632538493910671430" level="1" text="PerformAction">
        <node type="function" name="PerformAction" id="632538493910671427" path="Metreos.StockTools" />
        <calls>
          <ref actid="632538493910671426" />
        </calls>
      </treenode>
      <treenode type="fun" id="632555905720470075" level="1" text="Exit">
        <node type="function" name="Exit" id="632555905720470072" path="Metreos.StockTools" />
        <calls>
          <ref actid="632555905720470739" />
          <ref actid="632555905720470760" />
          <ref actid="632555905720470071" />
          <ref actid="632555905720470083" />
          <ref actid="632555905720470746" />
          <ref actid="632555905720470757" />
          <ref actid="632555905720470761" />
          <ref actid="632779433073575537" />
          <ref actid="632779433073575981" />
          <ref actid="632779433073575987" />
          <ref actid="632802867924431317" />
          <ref actid="632779433073575531" />
        </calls>
      </treenode>
      <treenode type="fun" id="632779329832771649" level="1" text="OpenDBConnection">
        <node type="function" name="OpenDBConnection" id="632779329832771646" path="Metreos.StockTools" />
        <calls>
          <ref actid="632779329832771645" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="db_poolConnections" id="632804601979661912" vid="632674146742755441">
        <Properties type="Bool" defaultInitWith="true" initWith="DbConnPooling">db_poolConnections</Properties>
      </treenode>
      <treenode text="db_ConnectionName" id="632804601979661914" vid="632347619057191312">
        <Properties type="String" initWith="DbConnectionName">db_ConnectionName</Properties>
      </treenode>
      <treenode text="db_Master_DbName" id="632804601979661916" vid="632346722572969731">
        <Properties type="String" initWith="DbName">db_Master_DbName</Properties>
      </treenode>
      <treenode text="db_Master_DbServer" id="632804601979661918" vid="632346722572969733">
        <Properties type="String" initWith="Server">db_Master_DbServer</Properties>
      </treenode>
      <treenode text="db_Master_Port" id="632804601979661920" vid="632346722572969735">
        <Properties type="UInt" initWith="Port">db_Master_Port</Properties>
      </treenode>
      <treenode text="db_Master_Username" id="632804601979661922" vid="632346722572969737">
        <Properties type="String" initWith="Username">db_Master_Username</Properties>
      </treenode>
      <treenode text="db_Master_Password" id="632804601979661924" vid="632346722572969739">
        <Properties type="String" initWith="Password">db_Master_Password</Properties>
      </treenode>
      <treenode text="db_Slave_DbName" id="632804601979661926" vid="632346722572969731">
        <Properties type="String" initWith="SlaveDBName">db_Slave_DbName</Properties>
      </treenode>
      <treenode text="db_Slave_DbServer" id="632804601979661928" vid="632346722572969733">
        <Properties type="String" initWith="SlaveDBServerAddress">db_Slave_DbServer</Properties>
      </treenode>
      <treenode text="db_Slave_Port" id="632804601979661930" vid="632346722572969735">
        <Properties type="UInt" initWith="SlaveDBServerPort">db_Slave_Port</Properties>
      </treenode>
      <treenode text="db_Slave_Username" id="632804601979661932" vid="632346722572969737">
        <Properties type="String" initWith="SlaveDBServerUsername">db_Slave_Username</Properties>
      </treenode>
      <treenode text="db_Slave_Password" id="632804601979661934" vid="632346722572969739">
        <Properties type="String" initWith="SlaveDBServerPassword">db_Slave_Password</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="632804601979661936" vid="632527335560197491">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
      <treenode text="g_callId" id="632804601979661938" vid="632527335560196924">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632804601979661940" vid="632527335560196926">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_to" id="632804601979661942" vid="632527335560197517">
        <Properties type="String" defaultInitWith="UNAVAILABLE">g_to</Properties>
      </treenode>
      <treenode text="g_from" id="632804601979661944" vid="632527335560197519">
        <Properties type="String" defaultInitWith="UNAVAILABLE">g_from</Properties>
      </treenode>
      <treenode text="g_callRecordId" id="632804601979661946" vid="632527335560197506">
        <Properties type="UInt" defaultInitWith="0">g_callRecordId</Properties>
      </treenode>
      <treenode text="g_userId" id="632804601979661948" vid="632528071621735692">
        <Properties type="UInt">g_userId</Properties>
      </treenode>
      <treenode text="g_accountNumber" id="632804601979661950" vid="632528071621735674">
        <Properties type="String" defaultInitWith="NONE">g_accountNumber</Properties>
      </treenode>
      <treenode text="g_outstandingEventsList" id="632804601979661952" vid="632538425211819376">
        <Properties type="ArrayList">g_outstandingEventsList</Properties>
      </treenode>
      <treenode text="g_timerFireDelay" id="632804601979661954" vid="632538425211820333">
        <Properties type="UInt" defaultInitWith="5">g_timerFireDelay</Properties>
      </treenode>
      <treenode text="ui_enableAllKey" id="632804601979661956" vid="632528130761068049">
        <Properties type="String" defaultInitWith="1">ui_enableAllKey</Properties>
      </treenode>
      <treenode text="ui_disableAllKey" id="632804601979661958" vid="632528130761068051">
        <Properties type="String" defaultInitWith="2">ui_disableAllKey</Properties>
      </treenode>
      <treenode text="ui_enableVMKey" id="632804601979661960" vid="632528130761068053">
        <Properties type="String" defaultInitWith="3">ui_enableVMKey</Properties>
      </treenode>
      <treenode text="ui_disableAllButVMKey" id="632804601979661962" vid="632604822630455060">
        <Properties type="String" defaultInitWith="4">ui_disableAllButVMKey</Properties>
      </treenode>
      <treenode text="g_poundAudio" id="632804601979661964" vid="632527335560196915">
        <Properties type="String" defaultInitWith="ar_pound_sign.wav">g_poundAudio</Properties>
      </treenode>
      <treenode text="g_acctNumAudio" id="632804601979661966" vid="632527335560196917">
        <Properties type="String" defaultInitWith="ar_enter_acct.wav">g_acctNumAudio</Properties>
      </treenode>
      <treenode text="g_pinNumAudio" id="632804601979661968" vid="632527335560196919">
        <Properties type="String" defaultInitWith="ar_enter_pin.wav">g_pinNumAudio</Properties>
      </treenode>
      <treenode text="g_unrecognizedInputAudio" id="632804601979661970" vid="632538493910671442">
        <Properties type="String" defaultInitWith="ar_entry_unrec.wav">g_unrecognizedInputAudio</Properties>
      </treenode>
      <treenode text="g_mainMenuAudio" id="632804601979661972" vid="632538493910671457">
        <Properties type="String" defaultInitWith="ar_findme_main_menu.wav">g_mainMenuAudio</Properties>
      </treenode>
      <treenode text="g_loginFailedAudio" id="632804601979661974" vid="632549474022185574">
        <Properties type="String" defaultInitWith="ar_login_failed.wav">g_loginFailedAudio</Properties>
      </treenode>
      <treenode text="g_systemErrorAudio" id="632804601979661976" vid="632779433073575533">
        <Properties type="String" defaultInitWith="ar_system_error.wav">g_systemErrorAudio</Properties>
      </treenode>
      <treenode text="g_operationSuccessAudio" id="632804601979661978" vid="632779433073575975">
        <Properties type="String" defaultInitWith="ar_operation_success.wav">g_operationSuccessAudio</Properties>
      </treenode>
      <treenode text="g_goodByeAudio" id="632804601979661980" vid="632779433073575991">
        <Properties type="String" defaultInitWith="ar_good_bye.wav">g_goodByeAudio</Properties>
      </treenode>
      <treenode text="g_noVmBoxAudio" id="632804601979661982" vid="632802867924431315">
        <Properties type="String" defaultInitWith="ar_no_vm_box_def.wav">g_noVmBoxAudio</Properties>
      </treenode>
      <treenode text="g_callRecordCreated" id="632804601979662176" vid="632804601979662175">
        <Properties type="Bool" defaultInitWith="false">g_callRecordCreated</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632527335560196791" treenode="632527335560196792" appnode="632527335560196789" handlerfor="632527335560196788">
    <node type="Start" id="632527335560196791" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="228">
      <linkto id="632527335560197487" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632527335560196849" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1023" y="228">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632527335560196873" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="566" y="212" mx="619" my="228">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632555905720470066" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632555905720470751" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_poundAudio</ap>
        <ap name="Prompt1" type="variable">g_acctNumAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">account_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632527335560196923" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="404" y="228">
      <linkto id="632527335560196873" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720470066" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DisplayName" type="literal">ActiveRelay FindMe</ap>
        <ap name="Conference" type="literal">false</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="ConnectionId">g_connectionId</rd>
        <log condition="default" on="true" level="Warning" type="csharp">"OnIncomingCall: Failed to answer incoming call with callId: " + callId</log>
      </Properties>
    </node>
    <node type="Action" id="632527335560197295" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="407.7246" y="488">
      <linkto id="632527335560197296" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632527335560197297" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="OriginNumber" type="variable">from</ap>
        <ap name="DestinationNumber" type="variable">to</ap>
        <rd field="CallRecordsId">g_callRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632527335560197296" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="407.724548" y="603">
      <linkto id="632527335560197297" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="literal">InternalError</ap>
      </Properties>
    </node>
    <node type="Action" id="632527335560197297" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="276.724548" y="603">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632527335560197487" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="139.90625" y="229">
      <linkto id="632779329832771645" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string from, ref string g_routingGuid, string routingGuid, ref string to, ref string g_to, ref string g_from,  ref ArrayList g_outstandingEventsList)
{
	if (from == null || from == string.Empty)
		g_from = from = "UNAVAILABLE";
	else
		g_from = from;

	if (to == null || to == string.Empty)
		g_to = to = "UNAVAILABLE";
	else
		g_to = to;

	if (g_outstandingEventsList == null)
		g_outstandingEventsList = new ArrayList();

	g_routingGuid = routingGuid;
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Comment" id="632527335560197521" text="need to specify commandtimeout for media actions" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="390" y="32" />
    <node type="Action" id="632555905720470066" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="406" y="373">
      <linkto id="632527335560197295" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnIncomingCall: hanging up call with callId: " + callId</log>
      </Properties>
    </node>
    <node type="Action" id="632555905720470751" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="759" y="228">
      <linkto id="632555905720470752" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632804601979662174" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="OriginNumber" type="variable">g_from</ap>
        <ap name="DestinationNumber" type="variable">g_to</ap>
        <rd field="CallRecordsId">g_callRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632555905720470752" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="895" y="329">
      <linkto id="632527335560196849" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">0</ap>
        <rd field="ResultData">g_callRecordId</rd>
      </Properties>
    </node>
    <node type="Comment" id="632585988688887076" text="Initialize variables, check for callerId integrity,&#xD;&#xA;open Application Suite database, and answer the call.&#xD;&#xA;Play announcement to caller, prompting for user account&#xD;&#xA;number. If any of those operations fail, hang up the call, write &#xD;&#xA;call record, and exit. Otherwise, write start of call record, and &#xD;&#xA;wait for Play to complete." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="69" y="100" />
    <node type="Action" id="632779329832771645" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="200.594727" y="213" mx="258" my="229">
      <items count="1">
        <item text="OpenDBConnection" />
      </items>
      <linkto id="632527335560196923" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632555905720470066" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">OpenDBConnection</ap>
      </Properties>
    </node>
    <node type="Action" id="632804601979662174" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="892.4707" y="229">
      <linkto id="632527335560196849" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_callRecordCreated</rd>
      </Properties>
    </node>
    <node type="Variable" id="632527335560196862" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632527335560196930" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.CallControl.IncomingCall">to</Properties>
    </node>
    <node type="Variable" id="632527335560196931" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">from</Properties>
    </node>
    <node type="Variable" id="632527335560197489" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632527335560197493" name="DSN" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">DSN</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632527335560196866" treenode="632527335560196867" appnode="632527335560196864" handlerfor="632527335560196863">
    <node type="Start" id="632527335560196866" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="276">
      <linkto id="632527335560197525" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632527335560197525" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="125" y="276">
      <linkto id="632527335560197539" type="Labeled" style="Bezier" ortho="true" label="account_code" />
      <linkto id="632527335560197543" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632528071621735660" type="Labeled" style="Bezier" ortho="true" label="pin_code" />
      <linkto id="632538493910671412" type="Labeled" style="Bezier" ortho="true" label="mainMenu" />
      <linkto id="632555905720470737" type="Labeled" style="Bezier" ortho="true" label="exit" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnPlay_Complete: Value of UserData is: " + userData
</log>
      </Properties>
    </node>
    <node type="Action" id="632527335560197536" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="493" y="44" mx="567" my="60">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632527335560197530" />
        <item text="OnGatherDigits_Failed" treenode="632527335560197535" />
      </items>
      <linkto id="632527335560197541" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Label" id="632527335560197539" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="126" y="156" />
    <node type="Label" id="632527335560197540" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="462" y="59">
      <linkto id="632527335560197536" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632527335560197541" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="703" y="58">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632527335560197542" text="Make sure to handle potential flake&#xD;&#xA;that can be caused by a RemoteHangup&#xD;&#xA;while a media op is in progress, if any." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="43" y="61" />
    <node type="Action" id="632527335560197543" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="124" y="428">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632528071621735660" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="299" y="155" />
    <node type="Label" id="632528071621735661" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="456" y="268">
      <linkto id="632528071621735662" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632528071621735662" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="493" y="252" mx="567" my="268">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632527335560197530" />
        <item text="OnGatherDigits_Failed" treenode="632527335560197535" />
      </items>
      <linkto id="632528071621735665" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632528071621735665" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="694" y="267">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632538493910671412" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="294" y="276" />
    <node type="Label" id="632538493910671413" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="452" y="490">
      <linkto id="632538493910671414" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632538493910671414" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="495" y="473" mx="569" my="489">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632527335560197530" />
        <item text="OnGatherDigits_Failed" treenode="632527335560197535" />
      </items>
      <linkto id="632538493910671417" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632538493910671417" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="690" y="487">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632555905720470737" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="299" y="428" />
    <node type="Label" id="632555905720470738" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="448" y="710">
      <linkto id="632555905720470739" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555905720470739" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="515.825867" y="694" mx="553" my="710">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632555905720470740" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632555905720470740" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="683" y="709">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632585988688887077" text="Based on the UserData, different actions may be taken.&#xD;&#xA;Most of these involve receiving digits, while using specific &#xD;&#xA;termination conditions. Play may be used for error handling,&#xD;&#xA;and as such, a branch for the exit path is provided." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="34" y="523" />
    <node type="Variable" id="632527335560197524" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632527335560196871" treenode="632527335560196872" appnode="632527335560196869" handlerfor="632527335560196868">
    <node type="Start" id="632527335560196871" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="86" y="198">
      <linkto id="632555905720470760" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632538493910671459" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="328" y="197">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555905720470760" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="161.825851" y="181" mx="199" my="197">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632538493910671459" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" activetab="true" startnode="632527335560197529" treenode="632527335560197530" appnode="632527335560197527" handlerfor="632527335560197526">
    <node type="Start" id="632527335560197529" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="225">
      <linkto id="632605107870147738" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632527335560197547" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="293" y="225">
      <linkto id="632527335560197548" type="Labeled" style="Bezier" ortho="true" label="account_code" />
      <linkto id="632528071621735666" type="Labeled" style="Bezier" ortho="true" label="pin_code" />
      <linkto id="632538493910671408" type="Labeled" style="Bezier" ortho="true" label="mainMenu" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Label" id="632527335560197548" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="292" y="114" />
    <node type="Label" id="632527335560197549" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="508.998657" y="135">
      <linkto id="632527335560197550" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632527335560197550" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="589" y="135">
      <linkto id="632528071621735672" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <linkto id="632555905720470067" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632555905720470068" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632555905720470067" type="Labeled" style="Bezier" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632527335560197551" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="211" y="225">
      <linkto id="632527335560197547" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="false" level="Verbose" type="csharp">"OnGatherDigits_Complete: Value of UserData is: " + userData + ", and the RoutingGuid is: " + g_routingGuid + ", and digits contains: " + digits</log>
public static string Execute(ref string digits)
{
	if (digits == null)
		digits = string.Empty;

	digits = digits.Replace("#", string.Empty);
	digits = digits.Replace("*", string.Empty);	
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632528071621735656" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="736" y="118" mx="789" my="134">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632528071621735659" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720470071" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_poundAudio</ap>
        <ap name="Prompt1" type="variable">g_pinNumAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">pin_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632528071621735659" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="898" y="133">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632528071621735666" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="415" y="115" />
    <node type="Label" id="632528071621735667" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="509" y="676">
      <linkto id="632528071621735668" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632528071621735668" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="591" y="676">
      <linkto id="632542558789588978" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <linkto id="632555905720470077" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632555905720470078" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632555905720470077" type="Labeled" style="Bezier" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632528071621735672" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="680" y="134">
      <linkto id="632528071621735656" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">digits</ap>
        <rd field="ResultData">g_accountNumber</rd>
      </Properties>
    </node>
    <node type="Action" id="632528071621735676" name="PhoneLogin" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1028" y="676">
      <linkto id="632549474022185571" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632804601979662178" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="AccountCode" type="csharp">UInt32.Parse(g_accountNumber)</ap>
        <ap name="Pin" type="csharp">UInt32.Parse(digits)</ap>
        <ap name="UserPhoneNumber" type="variable">g_from</ap>
        <rd field="UserId">g_userId</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnGatherDigits_Complete: UserData: " + userData + " - Validating user credentials"</log>
      </Properties>
    </node>
    <node type="Action" id="632528130761068044" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1231.4707" y="660" mx="1284" my="676">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632555905720470746" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632555905720470747" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="variable">g_mainMenuAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">mainMenu</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnGatherDigits_Complete: UserData: " + userData + " - playing main menu message."
</log>
      </Properties>
    </node>
    <node type="Label" id="632538493910671408" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="439" y="225" />
    <node type="Label" id="632538493910671409" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="519" y="1225">
      <linkto id="632538493910671410" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632538493910671410" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="609" y="1225">
      <linkto id="632538493910671419" type="Labeled" style="Bezier" ortho="true" label="maxdigits" />
      <linkto id="632555905720470748" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632555905720470748" type="Labeled" style="Bezier" label="autostop" />
      <linkto id="632538493910671444" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632538493910671419" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="826" y="1225.5">
      <linkto id="632538493910671426" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632538493910671444" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string ui_enableAllKey, string ui_disableAllKey, string ui_enableVMKey, string ui_disableAllButVMKey, ref string action, string digits)
{
	if (digits == null)
		return IApp.VALUE_FAILURE;

	bool success = true;	
	if (digits.Equals(ui_enableAllKey))
		action = "enable_all";
	else if (digits.Equals(ui_disableAllKey))
		action = "disable_all";
	else if (digits.Equals(ui_enableVMKey))
		action = "enable_vmail";
	else if (digits.Equals(ui_disableAllButVMKey))
		action = "disable_except_vmail";
	else
		success = false;
	
	return (success ? IApp.VALUE_SUCCESS : "Unrecognized");
}
</Properties>
    </node>
    <node type="Action" id="632538493910671426" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="967.9492" y="1209" mx="1010" my="1225">
      <items count="1">
        <item text="PerformAction" />
      </items>
      <linkto id="632538493910671456" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="variable">action</ap>
        <ap name="FunctionName" type="literal">PerformAction</ap>
      </Properties>
    </node>
    <node type="Action" id="632538493910671444" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="778.4707" y="1379" mx="831" my="1395">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632555905720470757" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632555905720470758" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_mainMenuAudio</ap>
        <ap name="Prompt1" type="variable">g_unrecognizedInputAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">mainMenu</ap>
      </Properties>
    </node>
    <node type="Action" id="632538493910671456" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1151" y="1225">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632542558789588978" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="740.9414" y="676">
      <linkto id="632528071621735676" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632555905720470080" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string digits, string g_accountNumber)
{
	// maybe we should do cast here, but then we need to mess around with having two
	// local variables just for account number and pin. Why are those UInts?

	if (g_accountNumber == null || g_accountNumber.Equals(string.Empty) || digits.Equals(string.Empty))
		return IApp.VALUE_FAILURE;

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632549474022185571" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="981.4707" y="822" mx="1034" my="838">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632549474022185876" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720470083" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_acctNumAudio</ap>
        <ap name="Prompt3" type="variable">g_poundAudio</ap>
        <ap name="Prompt1" type="variable">g_loginFailedAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">account_code</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnGatherDigits_Complete: UserData: " + userData + " - playing invalid login message."</log>
      </Properties>
    </node>
    <node type="Action" id="632549474022185876" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="901" y="839">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555905720470067" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="589" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555905720470068" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="541.4707" y="249" mx="594" my="265">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632555905720470071" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632555905720470076" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_poundAudio</ap>
        <ap name="Prompt1" type="variable">g_acctNumAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">account_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632555905720470071" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="752.825867" y="249" mx="790" my="265">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632555905720470086" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632555905720470076" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="592.4707" y="410">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555905720470077" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="590.4707" y="571">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632555905720470078" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="591.4707" y="788" />
    <node type="Label" id="632555905720470079" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="783.4707" y="36">
      <linkto id="632528071621735656" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555905720470080" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="693.4707" y="824" mx="746" my="840">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632549474022185876" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720470083" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_acctNumAudio</ap>
        <ap name="Prompt3" type="variable">g_poundAudio</ap>
        <ap name="Prompt1" type="variable">g_unrecognizedInputAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">account_code</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnGatherDigits_Complete: UserData: " + userData + " - playing invalid input message."</log>
      </Properties>
    </node>
    <node type="Action" id="632555905720470083" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="868.2966" y="952" mx="905" my="968">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632549474022185876" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632555905720470086" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="900.4707" y="264">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632555905720470088" text="Exit userdata for play" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1253.4707" y="417" />
    <node type="Action" id="632555905720470746" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1423.82581" y="803" mx="1461" my="819">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632555905720470747" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632555905720470747" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1455" y="675">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555905720470748" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="609" y="1118">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555905720470757" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="794.8258" y="1546" mx="832" my="1562">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632555905720470758" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632555905720470758" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1012" y="1395">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632605107870147738" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="116" y="225">
      <linkto id="632527335560197551" type="Labeled" style="Bezier" ortho="true" label="unequal" />
      <linkto id="632605107870147739" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">userData</ap>
        <ap name="Value2" type="literal">mainMenu</ap>
      </Properties>
    </node>
    <node type="Action" id="632605107870147739" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="211" y="338">
      <linkto id="632527335560197547" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(ref string digits)
{
	if (digits == null)
		digits = string.Empty;
	
	return IApp.VALUE_SUCCESS;
}

</Properties>
    </node>
    <node type="Comment" id="632605107870147740" text="Don't strip * or # when dealing with&#xD;&#xA;main menu." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="115" y="386" />
    <node type="Action" id="632804601979662177" name="UpdateCallRecord" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1215" y="561">
      <linkto id="632528130761068044" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="UserId" type="variable">g_userId</ap>
      </Properties>
    </node>
    <node type="Action" id="632804601979662178" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1148.16821" y="676">
      <linkto id="632804601979662177" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632528130761068044" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_callRecordCreated</ap>
      </Properties>
    </node>
    <node type="Variable" id="632527335560197544" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" defaultInitWith="NONE" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632527335560197545" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" defaultInitWith="NONE" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">digits</Properties>
    </node>
    <node type="Variable" id="632527335560197546" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" defaultInitWith="NONE" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">termCond</Properties>
    </node>
    <node type="Variable" id="632779433073574762" name="action" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">action</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632527335560197534" treenode="632527335560197535" appnode="632527335560197532" handlerfor="632527335560197531">
    <node type="Start" id="632527335560197534" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="71">
      <linkto id="632555905720470761" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632538493910671460" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="294" y="71">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555905720470761" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="119.825851" y="55" mx="157" my="71">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632538493910671460" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnTimerFire" startnode="632538425211820327" treenode="632538425211820328" appnode="632538425211820325" handlerfor="632538425211820324">
    <node type="Start" id="632538425211820327" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="116">
      <linkto id="632538425211820330" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632538425211820330" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="131" y="116">
      <linkto id="632538425211820335" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">timerId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnTimerFire: Removing timer with ID: " + timerId</log>
      </Properties>
    </node>
    <node type="Action" id="632538425211820335" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="252.90625" y="116">
      <linkto id="632543463644885294" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(LogWriter log, ArrayList g_outstandingEventsList, string timerId)
{
	if (g_outstandingEventsList == null)
	{
		log.Write(TraceLevel.Error, "OnTimerFire: g_outstandingEventsList is null!");
		return IApp.VALUE_FAILURE;
	}

	if (g_outstandingEventsList.Contains(timerId))
	{
		log.Write(TraceLevel.Verbose, "OnTimerFire: timer fired for outstanding event with TimerId: " + timerId + ". Removing from Outstanding list.");
		g_outstandingEventsList.Remove(timerId);
		return "Valid";
	}
	else
	{
		log.Write(TraceLevel.Verbose, "OnTimerFire: timer fired for event that is not in outstanding event list. TimerId: " + timerId );
		return "Invalid";
	}
}
</Properties>
    </node>
    <node type="Action" id="632543463644885294" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="360" y="116">
      <linkto id="632779433073575536" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632779433073575535" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="632" y="115.166687">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632779433073575536" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="428" y="99.16669" mx="481" my="115">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632779433073575535" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632779433073575537" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_goodByeAudio</ap>
        <ap name="Prompt1" type="variable">g_systemErrorAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632779433073575537" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="444" y="276.1667" mx="481" my="292">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632779433073575535" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Variable" id="632538425211820329" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="timerId" refType="reference" name="Metreos.Providers.TimerFacility.TimerFire">timerId</Properties>
    </node>
    <node type="Variable" id="632543463644884955" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="timerUserData" refType="reference" name="Metreos.Providers.TimerFacility.TimerFire">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632550392566729310" treenode="632550392566729311" appnode="632550392566729308" handlerfor="632550392566729307">
    <node type="Start" id="632550392566729310" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="261">
      <linkto id="632555905720470754" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632550392566729312" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="318" y="260">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555905720470754" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="170.360016" y="260">
      <linkto id="632550392566729312" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="literal">Normal</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632555905720469427" treenode="632555905720469428" appnode="632555905720469425" handlerfor="632555905720469424">
    <node type="Start" id="632555905720469427" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632555905720469434" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555905720469434" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="119" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632555905720469432" treenode="632555905720469433" appnode="632555905720469430" handlerfor="632555905720469429">
    <node type="Start" id="632555905720469432" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632555905720469435" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555905720469435" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="98" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotDigits" startnode="632593879973704240" treenode="632593879973704241" appnode="632593879973704238" handlerfor="632593879973704237">
    <node type="Start" id="632593879973704240" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="47" y="159">
      <linkto id="632593879973704242" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632593879973704242" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="133" y="160">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632593879973704246" treenode="632593879973704247" appnode="632593879973704244" handlerfor="632593879973704243">
    <node type="Start" id="632593879973704246" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="107">
      <linkto id="632593879973704963" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632593879973704963" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="110" y="107">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnFindMeServiceRequestResp" startnode="632779433073574713" treenode="632779433073574714" appnode="632779433073574711" handlerfor="632779433073574710">
    <node type="Start" id="632779433073574713" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="46" y="139">
      <linkto id="632779433073574750" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632779433073574718" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="223" y="480">
      <linkto id="632779433073575972" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632779433073574750" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="169.90625" y="140.5">
      <linkto id="632779433073574754" type="Labeled" style="Bezier" label="StaleEvent" />
      <linkto id="632779433073574754" type="Labeled" style="Bezier" label="default" />
      <linkto id="632779433073574755" type="Labeled" style="Bezier" ortho="true" label="ValidEvent" />
      <Properties language="csharp">
        <log condition="entry" on="false" level="Info" type="literal">OnFindMeServiceRequestResp: function entry</log>
public static string Execute(LogWriter log, ArrayList g_outstandingEventsList, string timerId)
{
	if (g_outstandingEventsList == null)
	{
		log.Write(TraceLevel.Error, "OnFindMeServiceRequestResp: g_outstandingEventsList is null!");
		return IApp.VALUE_FAILURE;
	}

	if (g_outstandingEventsList.Contains(timerId))
	{
		log.Write(TraceLevel.Verbose, "OnFindMeServiceRequestResp: received valid event with TimerId: " + timerId + ". Removing from Outstanding list.");
		g_outstandingEventsList.Remove(timerId);
		return "ValidEvent";
	}
	else
	{
		log.Write(TraceLevel.Verbose, "OnFindMeServiceRequestResp: received STALE event with TimerId: " + timerId);				
		return "StaleEvent";
	}
}

</Properties>
    </node>
    <node type="Action" id="632779433073574751" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="390.90625" y="140.5">
      <linkto id="632779433073574752" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632779433073574753" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <linkto id="632802867924431932" type="Labeled" style="Bezier" ortho="true" label="MailboxNotDefined" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">response</ap>
      </Properties>
    </node>
    <node type="Label" id="632779433073574752" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="516.90625" y="32" />
    <node type="Label" id="632779433073574753" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="512.90625" y="254.5" />
    <node type="Action" id="632779433073574754" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="169.90625" y="332.5">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632779433073574755" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="292.90625" y="140.5">
      <linkto id="632779433073574751" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">timerId</ap>
        <log condition="entry" on="false" level="Info" type="csharp">"OnFindMeServiceRequestResp: Removing timer with timerId: " + timerId</log>
      </Properties>
    </node>
    <node type="Action" id="632779433073575972" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="308.4707" y="465" mx="361" my="481">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632779433073575987" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632779433073575989" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_mainMenuAudio</ap>
        <ap name="Prompt1" type="variable">g_operationSuccessAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">mainMenu</ap>
      </Properties>
    </node>
    <node type="Action" id="632779433073575979" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="518.4707" y="838.1667">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632779433073575980" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="314.4707" y="822.1667" mx="367" my="838">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632779433073575979" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632779433073575981" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_goodByeAudio</ap>
        <ap name="Prompt1" type="variable">g_systemErrorAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632779433073575981" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="330.4707" y="999.166748" mx="368" my="1015">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632779433073575979" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632779433073575987" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="323.4707" y="623.166748" mx="361" my="639">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632779433073575989" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632779433073575989" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="537.9414" y="481.166748">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632779433073575990" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="238.9414" y="839.166748">
      <linkto id="632779433073575980" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632802867924430993" text="N" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="840.509766" y="483.166748">
      <linkto id="632802867924430994" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632802867924430994" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="921.509766" y="468.166748" mx="974" my="484">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632802867924431317" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632802867924431318" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_mainMenuAudio</ap>
        <ap name="Prompt1" type="variable">g_noVmBoxAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">mainMenu</ap>
      </Properties>
    </node>
    <node type="Action" id="632802867924431317" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="936.806335" y="623.166748" mx="974" my="639">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632802867924431318" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632802867924431318" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1194.92188" y="485.166748">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632802867924431932" text="N" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="596" y="142" />
    <node type="Variable" id="632779433073575543" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Response" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632779433073575544" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TimerId" refType="reference">timerId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="PerformAction" startnode="632538493910671429" treenode="632538493910671430" appnode="632538493910671427" handlerfor="632779433073574710">
    <node type="Start" id="632538493910671429" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="169">
      <linkto id="632538493910671462" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632538493910671461" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="398" y="169">
      <linkto id="632539304763653382" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632593879973704971" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SenderRoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="TimerId" type="variable">timerId</ap>
        <ap name="UserId" type="variable">g_userId</ap>
        <ap name="OperationType" type="variable">action</ap>
        <ap name="EventName" type="literal">Metreos.Events.ActiveRelay.FindMeServiceRequest</ap>
      </Properties>
    </node>
    <node type="Action" id="632538493910671462" name="AddNonTriggerTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="93" y="153" mx="155" my="169">
      <items count="1">
        <item text="OnTimerFire" treenode="632538425211820328" />
      </items>
      <linkto id="632538493910671464" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now.AddSeconds(g_timerFireDelay)</ap>
        <ap name="timerUserData" type="variable">action</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="timerId">timerId</rd>
        <log condition="exit" on="false" level="Info" type="csharp">"MonitorAgent: Added timer with timerId: " + timerId
</log>
      </Properties>
    </node>
    <node type="Action" id="632538493910671464" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="281.90625" y="169">
      <linkto id="632538493910671461" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(LogWriter log, ArrayList g_outstandingEventsList, string timerId)
{
	if (g_outstandingEventsList == null)
	{
		log.Write(TraceLevel.Error, "PerformAction: g_outstandingEventsList is null!");
		return IApp.VALUE_FAILURE;
	}

	g_outstandingEventsList.Add(timerId);
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632539304763653382" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="537" y="168">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632549697335393901" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="554" y="367">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632593879973704971" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="399" y="260">
      <linkto id="632779433073575528" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">timerId</ap>
      </Properties>
    </node>
    <node type="Action" id="632779433073575528" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="350" y="351" mx="403" my="367">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632549697335393901" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632779433073575531" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_goodByeAudio</ap>
        <ap name="Prompt1" type="variable">g_systemErrorAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632779433073575531" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="366" y="528" mx="403" my="544">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632549697335393901" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Variable" id="632538493910671466" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" refType="reference">timerId</Properties>
    </node>
    <node type="Variable" id="632779433073575527" name="action" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Action" refType="reference">action</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Exit" startnode="632555905720470074" treenode="632555905720470075" appnode="632555905720470072" handlerfor="632779433073574710">
    <node type="Start" id="632555905720470074" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="95">
      <linkto id="632555905720470750" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555905720470749" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="299" y="97">
      <linkto id="632555905720470753" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"Exit: hanging up call with callId: " + g_callId</log>
      </Properties>
    </node>
    <node type="Action" id="632555905720470750" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="175" y="96">
      <linkto id="632555905720470749" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="literal">Normal</ap>
      </Properties>
    </node>
    <node type="Action" id="632555905720470753" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="420" y="98">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OpenDBConnection" startnode="632779329832771648" treenode="632779329832771649" appnode="632779329832771646" handlerfor="632779433073574710">
    <node type="Start" id="632779329832771648" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="84" y="82">
      <linkto id="632779329832772801" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632779329832772800" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="373.188477" y="80.5">
      <linkto id="632779329832772804" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632779329832772807" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="variable">db_ConnectionName</ap>
        <ap name="Type" type="literal">mysql</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OpenDBConnection: attempting connect to Primary</log>
        <log condition="default" on="true" level="Warning" type="literal">OpenDBConnection: Connection to Primary failed.</log>
      </Properties>
    </node>
    <node type="Action" id="632779329832772801" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="201.1888" y="81.5">
      <linkto id="632779329832772800" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632779329832772807" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">db_Master_DbName</ap>
        <ap name="Server" type="variable">db_Master_DbServer</ap>
        <ap name="Port" type="variable">db_Master_Port</ap>
        <ap name="Username" type="variable">db_Master_Username</ap>
        <ap name="Password" type="variable">db_Master_Password</ap>
        <ap name="Pooling" type="variable">db_poolConnections</ap>
        <ap name="ConnectionTimeout" type="literal">1</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632779329832772802" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="661.1886" y="245.5">
      <linkto id="632779329832772804" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632779329832772806" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="variable">db_ConnectionName</ap>
        <ap name="Type" type="literal">mysql</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OpenDBConnection: attempting connect to Secondary</log>
        <log condition="default" on="true" level="Info" type="literal">OpenDBConnection: Connection to Secondary failed.
</log>
      </Properties>
    </node>
    <node type="Action" id="632779329832772803" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="508.188782" y="245.5">
      <linkto id="632779329832772802" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632779329832772806" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">db_Slave_DbName</ap>
        <ap name="Server" type="variable">db_Slave_DbServer</ap>
        <ap name="Port" type="variable">db_Slave_Port</ap>
        <ap name="Username" type="variable">db_Slave_Username</ap>
        <ap name="Password" type="variable">db_Slave_Password</ap>
        <ap name="Pooling" type="variable">db_poolConnections</ap>
        <ap name="ConnectionTimeout" type="literal">1</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632779329832772804" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="660.9997" y="80">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OpenDBConnection: connection to database established.</log>
      </Properties>
    </node>
    <node type="Action" id="632779329832772806" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="661.9997" y="444">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
        <log condition="entry" on="true" level="Error" type="literal">OpenDBConnection: AppSuite DB connections failed. Check application settings.</log>
      </Properties>
    </node>
    <node type="Action" id="632779329832772807" name="SetSessionData" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="373.9997" y="245">
      <linkto id="632779329832772803" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="AllowDBWrite" type="csharp">false</ap>
      </Properties>
    </node>
    <node type="Variable" id="632779329832772816" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>