<Application name="IncomingCall" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="IncomingCall">
    <outline>
      <treenode type="evh" id="632472835761598880" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632472835761598877" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632472835761598876" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632472838633997878" level="2" text="Metreos.Providers.TimerFacility.TimerFire: OnTimerFire">
        <node type="function" name="OnTimerFire" id="632472838633997875" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632472838633997874" path="Metreos.Providers.TimerFacility.TimerFire" />
        <references>
          <ref id="633131741263157250" actid="632660197747417594" />
          <ref id="633131741263157272" actid="632663646668754482" />
        </references>
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="632472838633998021" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632472838633998018" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632472838633998017" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="633131741263156923" actid="632490133710724958" />
          <ref id="633131741263157047" actid="632701531416387420" />
          <ref id="633131741263157071" actid="632472966202366250" />
          <ref id="633131741263157116" actid="632472838633998030" />
          <ref id="633131741263157119" actid="632472838633998033" />
          <ref id="633131741263157124" actid="632472838633998040" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632472838633998026" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632472838633998023" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632472838633998022" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="633131741263156924" actid="632490133710724958" />
          <ref id="633131741263157048" actid="632701531416387420" />
          <ref id="633131741263157072" actid="632472966202366250" />
          <ref id="633131741263157117" actid="632472838633998030" />
          <ref id="633131741263157120" actid="632472838633998033" />
          <ref id="633131741263157125" actid="632472838633998040" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632472838633998064" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632472838633998061" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632472838633998060" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="633131741263157264" actid="632663009636381122" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632472838633998069" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632472838633998066" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632472838633998065" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="633131741263157265" actid="632663009636381122" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632472838633998074" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632472838633998071" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632472838633998070" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="633131741263157266" actid="632663009636381122" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632472838633998180" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632472838633998177" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632472838633998176" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="633131741263156880" actid="632490133710724968" />
          <ref id="633131741263156937" actid="632696551309578545" />
          <ref id="633131741263156972" actid="632696551309578550" />
          <ref id="633131741263157034" actid="632699947423161355" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632472838633998185" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632472838633998182" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632472838633998181" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="633131741263156881" actid="632490133710724968" />
          <ref id="633131741263156938" actid="632696551309578545" />
          <ref id="633131741263156973" actid="632696551309578550" />
          <ref id="633131741263157035" actid="632699947423161355" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632472966202366241" level="2" text="Metreos.Events.ActiveRelay.SwapRequest: OnSwapRequest">
        <node type="function" name="OnSwapRequest" id="632472966202366238" path="Metreos.StockTools" />
        <node type="event" name="SwapRequest" id="632472966202366237" path="Metreos.Events.ActiveRelay.SwapRequest" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632482910352503780" level="2" text="Metreos.CallControl.GotDigits: OnGotDigits">
        <node type="function" name="OnGotDigits" id="632482910352503777" path="Metreos.StockTools" />
        <node type="event" name="GotDigits" id="632482910352503776" path="Metreos.CallControl.GotDigits" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632482910352503785" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632482910352503782" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632482910352503781" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632482910352503790" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632482910352503787" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632482910352503786" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632482910352503795" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632482910352503792" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632482910352503791" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632472838633997901" level="1" text="HandleError">
        <node type="function" name="HandleError" id="632472838633997898" path="Metreos.StockTools" />
        <calls>
          <ref actid="632660197747416296" />
          <ref actid="632666540064688477" />
          <ref actid="632666540064688473" />
          <ref actid="632696551309577165" />
          <ref actid="632671561491540666" />
          <ref actid="632666540064688467" />
          <ref actid="632670653296732627" />
          <ref actid="632699824275512004" />
          <ref actid="632699947423161363" />
          <ref actid="632701531416387424" />
          <ref actid="632484810722400426" />
        </calls>
      </treenode>
      <treenode type="fun" id="632472838633998016" level="1" text="ProcessHangup">
        <node type="function" name="ProcessHangup" id="632472838633998013" path="Metreos.StockTools" />
        <calls>
          <ref actid="632472838633998174" />
          <ref actid="632472838633998208" />
          <ref actid="632472838633998373" />
          <ref actid="632472838633998012" />
        </calls>
      </treenode>
      <treenode type="fun" id="632472838633998132" level="1" text="HangUpList">
        <node type="function" name="HangUpList" id="632472838633998129" path="Metreos.StockTools" />
        <calls>
          <ref actid="632699824275512001" />
          <ref actid="632472838633998128" />
          <ref actid="632668954769677294" />
          <ref actid="632484810722400425" />
          <ref actid="632491005669511438" />
          <ref actid="632491005669511440" />
          <ref actid="632674146742757027" />
        </calls>
      </treenode>
      <treenode type="fun" id="632472838633998172" level="1" text="SendSwapResponse">
        <node type="function" name="SendSwapResponse" id="632472838633998169" path="Metreos.StockTools" />
        <calls>
          <ref actid="632472838633998168" />
          <ref actid="632472838633998173" />
          <ref actid="632472838633998205" />
          <ref actid="632472838633998206" />
          <ref actid="632472838633998322" />
          <ref actid="632472838633998214" />
          <ref actid="632472838633998216" />
          <ref actid="632673314455190707" />
          <ref actid="632472838633998221" />
          <ref actid="632675094376283225" />
          <ref actid="632484810722400424" />
        </calls>
      </treenode>
      <treenode type="fun" id="632488360389763888" level="1" text="Answer">
        <node type="function" name="Answer" id="632488360389763885" path="Metreos.StockTools" />
        <calls>
          <ref actid="632484810722400466" />
          <ref actid="632674146742757024" />
          <ref actid="632484810722400399" />
        </calls>
      </treenode>
      <treenode type="fun" id="632658601492437817" level="1" text="OpenDBConnection">
        <node type="function" name="OpenDBConnection" id="632658601492437814" path="Metreos.StockTools" />
        <calls>
          <ref actid="632658601492437813" />
        </calls>
      </treenode>
      <treenode type="fun" id="632659276838564901" level="1" text="GetExternalNumbersInfo">
        <node type="function" name="GetExternalNumbersInfo" id="632659276838564898" path="Metreos.StockTools" />
        <calls>
          <ref actid="632659276838564897" />
        </calls>
      </treenode>
      <treenode type="fun" id="632660197747417588" level="1" text="ProcessDialTable">
        <node type="function" name="ProcessDialTable" id="632660197747417585" path="Metreos.StockTools" />
        <calls>
          <ref actid="632660197747417584" />
        </calls>
      </treenode>
      <treenode type="fun" id="632663009636381109" level="1" text="DialNumber">
        <node type="function" name="DialNumber" id="632663009636381106" path="Metreos.StockTools" />
        <calls>
          <ref actid="632663009636381113" />
          <ref actid="632699824275512009" />
          <ref actid="632703305376792802" />
          <ref actid="632663009636381105" />
        </calls>
      </treenode>
      <treenode type="fun" id="632663009636381118" level="1" text="HangupOutboundCall">
        <node type="function" name="HangupOutboundCall" id="632663009636381115" path="Metreos.StockTools" />
        <calls>
          <ref actid="632663009636381114" />
          <ref actid="632696551309577162" />
          <ref actid="632670653296732624" />
          <ref actid="632699947423161360" />
          <ref actid="632664669790126397" />
        </calls>
      </treenode>
      <treenode type="fun" id="632736982806206995" level="1" text="Exit">
        <node type="function" name="Exit" id="632736982806206992" path="Metreos.StockTools" />
        <calls>
          <ref actid="632736982806206996" />
        </calls>
      </treenode>
      <treenode type="fun" id="632797562403354949" level="1" text="WriteFindMeRecord">
        <node type="function" name="WriteFindMeRecord" id="632797562403354946" path="Metreos.StockTools" />
        <calls>
          <ref actid="632797562403354945" />
          <ref actid="632800221463702413" />
          <ref actid="632800221463702409" />
          <ref actid="632800221463702411" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="db_poolConnections" id="633131741263156677" vid="632674146742755441">
        <Properties type="Bool" defaultInitWith="true" initWith="Config.DbConnPooling">db_poolConnections</Properties>
      </treenode>
      <treenode text="db_ConnectionName" id="633131741263156679" vid="632347619057191312">
        <Properties type="String" initWith="Config.DbConnectionName">db_ConnectionName</Properties>
      </treenode>
      <treenode text="db_Master_DbName" id="633131741263156681" vid="632346722572969731">
        <Properties type="String" initWith="Config.DbName">db_Master_DbName</Properties>
      </treenode>
      <treenode text="db_Master_DbServer" id="633131741263156683" vid="632346722572969733">
        <Properties type="String" initWith="Config.Server">db_Master_DbServer</Properties>
      </treenode>
      <treenode text="db_Master_Port" id="633131741263156685" vid="632346722572969735">
        <Properties type="UInt" initWith="Config.Port">db_Master_Port</Properties>
      </treenode>
      <treenode text="db_Master_Username" id="633131741263156687" vid="632346722572969737">
        <Properties type="String" initWith="Config.Username">db_Master_Username</Properties>
      </treenode>
      <treenode text="db_Master_Password" id="633131741263156689" vid="632346722572969739">
        <Properties type="String" initWith="Config.Password">db_Master_Password</Properties>
      </treenode>
      <treenode text="db_Slave_DbName" id="633131741263156691" vid="632346722572969731">
        <Properties type="String" initWith="Config.SlaveDBName">db_Slave_DbName</Properties>
      </treenode>
      <treenode text="db_Slave_DbServer" id="633131741263156693" vid="632346722572969733">
        <Properties type="String" initWith="Config.SlaveDBServerAddress">db_Slave_DbServer</Properties>
      </treenode>
      <treenode text="db_Slave_Port" id="633131741263156695" vid="632346722572969735">
        <Properties type="UInt" initWith="Config.SlaveDBServerPort">db_Slave_Port</Properties>
      </treenode>
      <treenode text="db_Slave_Username" id="633131741263156697" vid="632346722572969737">
        <Properties type="String" initWith="Config.SlaveDBServerUsername">db_Slave_Username</Properties>
      </treenode>
      <treenode text="db_Slave_Password" id="633131741263156699" vid="632346722572969739">
        <Properties type="String" initWith="Config.SlaveDBServerPassword">db_Slave_Password</Properties>
      </treenode>
      <treenode text="ccm_Username" id="633131741263156701" vid="632347532121406910">
        <Properties type="String" initWith="Config.CCM_Device_Username">ccm_Username</Properties>
      </treenode>
      <treenode text="ccm_Password" id="633131741263156703" vid="632347532121406912">
        <Properties type="String" initWith="Config.CCM_Device_Password">ccm_Password</Properties>
      </treenode>
      <treenode text="g_callId_callee" id="633131741263156705" vid="632334492909687667">
        <Properties type="String">g_callId_callee</Properties>
      </treenode>
      <treenode text="g_callId_caller" id="633131741263156707" vid="632338694434063778">
        <Properties type="String">g_callId_caller</Properties>
      </treenode>
      <treenode text="g_numFailedDialedCalls" id="633131741263156709" vid="632347352120469817">
        <Properties type="Int" defaultInitWith="0">g_numFailedDialedCalls</Properties>
      </treenode>
      <treenode text="g_numDialedCalls" id="633131741263156711" vid="632347352120469819">
        <Properties type="Int" defaultInitWith="0">g_numDialedCalls</Properties>
      </treenode>
      <treenode text="g_connectionId_caller" id="633131741263156713" vid="632334492909687669">
        <Properties type="Int">g_connectionId_caller</Properties>
      </treenode>
      <treenode text="g_connectionId_callee" id="633131741263156715" vid="632334492909687671">
        <Properties type="Int">g_connectionId_callee</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="633131741263156717" vid="632334492909687673">
        <Properties type="String" defaultInitWith="0">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_to" id="633131741263156719" vid="632334492909687822">
        <Properties type="String">g_to</Properties>
      </treenode>
      <treenode text="g_from" id="633131741263156721" vid="632334492909687824">
        <Properties type="String">g_from</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="633131741263156723" vid="632334492909687830">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
      <treenode text="g_callAnswered" id="633131741263156725" vid="632337881797031614">
        <Properties type="Bool">g_callAnswered</Properties>
      </treenode>
      <treenode text="g_allowSwap" id="633131741263156727" vid="632337881797031679">
        <Properties type="Bool" defaultInitWith="false">g_allowSwap</Properties>
      </treenode>
      <treenode text="g_didCallerHangup" id="633131741263156729" vid="632337881797031684">
        <Properties type="Bool" defaultInitWith="false">g_didCallerHangup</Properties>
      </treenode>
      <treenode text="g_isCalleeConnected" id="633131741263156731" vid="632337881797031686">
        <Properties type="Bool" defaultInitWith="false">g_isCalleeConnected</Properties>
      </treenode>
      <treenode text="g_conferenceCreated" id="633131741263156733" vid="632337881797032116">
        <Properties type="Bool" defaultInitWith="false">g_conferenceCreated</Properties>
      </treenode>
      <treenode text="config_numSecondsToWait" id="633131741263156735" vid="632334492909687839">
        <Properties type="Int" defaultInitWith="60" initWith="Config.TimeToWait">config_numSecondsToWait</Properties>
      </treenode>
      <treenode text="g_userId" id="633131741263156737" vid="632338694434063826">
        <Properties type="Int" defaultInitWith="-1">g_userId</Properties>
      </treenode>
      <treenode text="g_calleeMmsIp" id="633131741263156739" vid="632338694434065181">
        <Properties type="String">g_calleeMmsIp</Properties>
      </treenode>
      <treenode text="g_calleeMmsPort" id="633131741263156741" vid="632338694434065183">
        <Properties type="UShort">g_calleeMmsPort</Properties>
      </treenode>
      <treenode text="g_swapNumber" id="633131741263156743" vid="632346509622661683">
        <Properties type="String">g_swapNumber</Properties>
      </treenode>
      <treenode text="g_swapUserId" id="633131741263156745" vid="632346509622661685">
        <Properties type="UInt">g_swapUserId</Properties>
      </treenode>
      <treenode text="g_swapRoutingGuid" id="633131741263156747" vid="632346509622661687">
        <Properties type="String">g_swapRoutingGuid</Properties>
      </treenode>
      <treenode text="g_swapConfirmWaitAmount" id="633131741263156749" vid="632346509622661706">
        <Properties type="Int" initWith="Config.swapScript_confirmWaitTime">g_swapConfirmWaitAmount</Properties>
      </treenode>
      <treenode text="g_swapConnectionId" id="633131741263156751" vid="632346722572969305">
        <Properties type="Int">g_swapConnectionId</Properties>
      </treenode>
      <treenode text="g_swapMmsIp" id="633131741263156753" vid="632346722572969307">
        <Properties type="String">g_swapMmsIp</Properties>
      </treenode>
      <treenode text="g_swapMmsPort" id="633131741263156755" vid="632346722572969309">
        <Properties type="UShort">g_swapMmsPort</Properties>
      </treenode>
      <treenode text="g_swapRequestReceived" id="633131741263156757" vid="632346722572969334">
        <Properties type="Bool" defaultInitWith="false">g_swapRequestReceived</Properties>
      </treenode>
      <treenode text="g_swapCallRecordId" id="633131741263156759" vid="632347416185000619">
        <Properties type="String">g_swapCallRecordId</Properties>
      </treenode>
      <treenode text="g_swapPerformed" id="633131741263156761" vid="632347416185000625">
        <Properties type="Bool" defaultInitWith="false">g_swapPerformed</Properties>
      </treenode>
      <treenode text="g_ciscoPhoneIp" id="633131741263156763" vid="632347526402658339">
        <Properties type="String">g_ciscoPhoneIp</Properties>
      </treenode>
      <treenode text="g_responseUrl" id="633131741263156765" vid="632347532121406901">
        <Properties type="String">g_responseUrl</Properties>
      </treenode>
      <treenode text="g_callRecordId" id="633131741263156767" vid="632347321830938338">
        <Properties type="Int">g_callRecordId</Properties>
      </treenode>
      <treenode text="g_waitingForDigit" id="633131741263156769" vid="632484810722400404">
        <Properties type="Bool" defaultInitWith="true">g_waitingForDigit</Properties>
      </treenode>
      <treenode text="g_makeCallState" id="633131741263156771" vid="632484810722400462">
        <Properties type="Hashtable">g_makeCallState</Properties>
      </treenode>
      <treenode text="g_acceptDigit" id="633131741263156773" vid="632484810722400944">
        <Properties type="String" defaultInitWith="#" initWith="Config.AcceptDigit">g_acceptDigit</Properties>
      </treenode>
      <treenode text="g_timeForDigit" id="633131741263156775" vid="632485420083422504">
        <Properties type="Int" initWith="Config.TimeToWait">g_timeForDigit</Properties>
      </treenode>
      <treenode text="g_timerRemoved" id="633131741263156777" vid="632488360389763880">
        <Properties type="String" defaultInitWith="false">g_timerRemoved</Properties>
      </treenode>
      <treenode text="g_playTime" id="633131741263156779" vid="632490133710724961">
        <Properties type="DateTime">g_playTime</Properties>
      </treenode>
      <treenode text="g_callerIdTranslations" id="633131741263156781" vid="632533122359157455">
        <Properties type="Hashtable" initWith="Config.CallerIdTranslations">g_callerIdTranslations</Properties>
      </treenode>
      <treenode text="g_significantDigits" id="633131741263156783" vid="632534136230500061">
        <Properties type="Int" initWith="Config.ExternSignDigits">g_significantDigits</Properties>
      </treenode>
      <treenode text="g_DbWriteEnabled" id="633131741263156785" vid="632659276838564881">
        <Properties type="Bool" defaultInitWith="true">g_DbWriteEnabled</Properties>
      </treenode>
      <treenode text="g_numbersTable" id="633131741263156787" vid="632660197747416280">
        <Properties type="DataTable">g_numbersTable</Properties>
      </treenode>
      <treenode text="g_translatedFrom" id="633131741263156789" vid="632660197747416286">
        <Properties type="String" defaultInitWith="UNKNOWN">g_translatedFrom</Properties>
      </treenode>
      <treenode text="g_displayName" id="633131741263156791" vid="632664669790126405">
        <Properties type="String">g_displayName</Properties>
      </treenode>
      <treenode text="g_hairpinMedia" id="633131741263156793" vid="632666340189714113">
        <Properties type="Bool" initWith="Config.MediaHairPin">g_hairpinMedia</Properties>
      </treenode>
      <treenode text="g_callIdToTableIndexMap" id="633131741263156795" vid="632670430957416629">
        <Properties type="Hashtable">g_callIdToTableIndexMap</Properties>
      </treenode>
      <treenode text="g_dialPrefix" id="633131741263156797" vid="632670653296732618">
        <Properties type="String" defaultInitWith="NONE" initWith="Config.DialPrefix">g_dialPrefix</Properties>
      </treenode>
      <treenode text="g_ARRecordExists" id="633131741263156799" vid="632678332830509270">
        <Properties type="Bool" defaultInitWith="false">g_ARRecordExists</Properties>
      </treenode>
      <treenode text="g_internationalPrefix" id="633131741263156801" vid="632695717290428612">
        <Properties type="String" initWith="Config.InterNatPrefix">g_internationalPrefix</Properties>
      </treenode>
      <treenode text="g_transferPrefix" id="633131741263156803" vid="632696483794716602">
        <Properties type="String" defaultInitWith="NONE" initWith="Config.TransferNumPrefix">g_transferPrefix</Properties>
      </treenode>
      <treenode text="g_staticTransferPattern" id="633131741263156805" vid="632696483794716604">
        <Properties type="String" initWith="Config.StaticTransferPattern">g_staticTransferPattern</Properties>
      </treenode>
      <treenode text="g_transferNumber" id="633131741263156807" vid="632696551309577156">
        <Properties type="String" defaultInitWith="NONE">g_transferNumber</Properties>
      </treenode>
      <treenode text="g_mmsId" id="633131741263156809" vid="632697370120313537">
        <Properties type="String" defaultInitWith="0">g_mmsId</Properties>
      </treenode>
      <treenode text="g_confirmReceiptAudioFile" id="633131741263156811" vid="632700150959162740">
        <Properties type="String" defaultInitWith="confirm_forward.wav">g_confirmReceiptAudioFile</Properties>
      </treenode>
      <treenode text="g_sessionCountIncremented" id="633131741263156813" vid="632736982806207001">
        <Properties type="Bool" defaultInitWith="false">g_sessionCountIncremented</Properties>
      </treenode>
      <treenode text="g_useSpecificPrompt" id="633131741263156815" vid="632749205663098929">
        <Properties type="Bool" initWith="Config.UseSpecificPrompt">g_useSpecificPrompt</Properties>
      </treenode>
      <treenode text="g_findMeRecords" id="633131741263156817" vid="632796852009379563">
        <Properties type="Hashtable">g_findMeRecords</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632472835761598879" treenode="632472835761598880" appnode="632472835761598877" handlerfor="632472835761598876">
    <node type="Start" id="632472835761598879" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="336">
      <linkto id="632658601492437813" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632472835761598897" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="361" y="336">
      <linkto id="632478222105050052" type="Labeled" style="Bevel" ortho="true" label="success" />
      <linkto id="632659276838564902" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="default" on="true" level="Warning" type="literal">OnIncomingCall: Could not retrieve 'To' number.</log>	
	public static string Execute(ref string g_routingGuid, ref string g_to,
		ref string g_from, string routingGuid, string to, ref string from, ref string g_callId_caller, string callerCallId, ref string g_displayName, string displayName, ref string g_dialPrefix, ref string g_transferPrefix)
	{
		if (from == null || from == string.Empty || from == "NONE")
			g_from = from = "UNKNOWN";

		if (to == null || to == string.Empty || to == "NONE")
			return IApp.VALUE_FAILURE;

		g_to = to;		
		g_from = from;

		if (displayName == null || displayName == "NONE")
			g_displayName = displayName = string.Empty;
		else
			g_displayName = displayName;
		
		g_routingGuid = routingGuid;
		g_callId_caller = callerCallId;
	
		if (g_dialPrefix.Trim().ToUpper() == "NONE")
			g_dialPrefix = string.Empty;
		else
			g_dialPrefix = g_dialPrefix.Trim();

		if (g_transferPrefix.Trim().ToUpper() == "NONE")
			g_transferPrefix = string.Empty;
		else
			g_transferPrefix = g_transferPrefix.Trim();
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632472838633997882" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="511" y="596">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632478222105050052" name="AcceptCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="508" y="334">
      <linkto id="632659276838564902" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632660197747416288" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callerCallId</ap>
        <rd field="MmsId">g_mmsId</rd>
        <log condition="default" on="true" level="Warning" type="literal">OnIncomingCall: Accept action on incoming call failed.</log>
      </Properties>
    </node>
    <node type="Action" id="632658601492437813" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="118.594727" y="319" mx="176" my="335">
      <items count="1">
        <item text="OpenDBConnection" />
      </items>
      <linkto id="632472835761598897" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632659276838564886" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">OpenDBConnection</ap>
      </Properties>
    </node>
    <node type="Action" id="632659276838564886" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="171.4707" y="589">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632659276838564887" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="647.1953" y="466.168274">
      <linkto id="632659276838564895" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="OriginNumber" type="variable">from</ap>
        <ap name="DestinationNumber" type="variable">to</ap>
        <rd field="CallRecordsId">g_callRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632659276838564889" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2466.19531" y="335.168274">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632659276838564895" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="649.8307" y="595">
      <linkto id="632472838633997882" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.Unknown</ap>
      </Properties>
    </node>
    <node type="Action" id="632659276838564897" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="924.4033" y="317" mx="994" my="333">
      <items count="1">
        <item text="GetExternalNumbersInfo" />
      </items>
      <linkto id="632667132589785680" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632667132589785681" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">GetExternalNumbersInfo</ap>
      </Properties>
    </node>
    <node type="Action" id="632659276838564902" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="509.4707" y="467">
      <linkto id="632659276838564887" type="Labeled" style="Bevel" ortho="true" label="true" />
      <linkto id="632472838633997882" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_DbWriteEnabled</ap>
      </Properties>
    </node>
    <node type="Action" id="632660197747416288" name="FormatAddress" class="MaxActionNode" group="" path="Metreos.Native.DialPlan" x="782.040344" y="334">
      <linkto id="632659276838564897" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632660197747416291" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DialedNumber" type="variable">from</ap>
        <ap name="DialingRules" type="variable">g_callerIdTranslations</ap>
        <rd field="ResultData">g_translatedFrom</rd>
      </Properties>
    </node>
    <node type="Action" id="632660197747416291" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="782.4707" y="466">
      <linkto id="632659276838564897" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">from</ap>
        <rd field="ResultData">g_translatedFrom</rd>
      </Properties>
    </node>
    <node type="Action" id="632660197747416296" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="956.874634" y="547" mx="994" my="563">
      <items count="1">
        <item text="HandleError" />
      </items>
      <linkto id="632667132589785683" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="action" type="literal">error_exit</ap>
        <ap name="announceTo" type="literal">default</ap>
        <ap name="error" type="literal">default</ap>
        <ap name="errorType" type="literal">database</ap>
        <ap name="FunctionName" type="literal">HandleError</ap>
      </Properties>
    </node>
    <node type="Action" id="632660197747416301" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="2181.87451" y="335">
      <linkto id="632660197747417584" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632666540064688477" type="Labeled" style="Bevel" ortho="true" label="EmptyList" />
      <Properties language="csharp">public static string Execute(DataTable g_numbersTable, DateTime userTime, string g_translatedFrom, int g_significantDigits, string g_from, DataTable filters, LogWriter log)
{
    // array list that contains references to rows in g_numbersTalble
    // that will be removed should it be determined below that they
    // contain entries that we do not want dialed. 
    ArrayList rowsToRemove = new ArrayList();

    // day of week in user's timezone
    DayOfWeek day = userTime.DayOfWeek;

    // Iterate through list of AR filters, and attempt to match the from number 
    bool isAllowed = false;
    bool isBlocked = false;
    if (filters != null)
    {
        string expression = null;
        foreach (DataRow row in filters.Rows)
        {
            object numberObject = row[SqlConstants.Tables.ActiveRelayFilterNumbers.Number];
            if (Convert.IsDBNull(numberObject))
                continue;
            else
            {
                expression = numberObject as string;
                if (expression == null || expression == string.Empty)
                    continue;

                Regex filterExp = new Regex(expression);
                Match m         = filterExp.Match(g_from);
                if (m.Success)
                {
                    FilterType type = (FilterType) row[SqlConstants.Tables.ActiveRelayFilterNumbers.Type];
                    if (type == FilterType.Allow)
                        isAllowed = true;
                    else
                        isBlocked = true;
                }
            }
        }
    }
    else
        log.Write(TraceLevel.Verbose, "OnIncomingCall: The list of ActiveRelay filters for this user is empty.");

    string numberToDial;
    // If the From is in the block list and is NOT in the allow list, try to find voicemail number and then return.
    if (isBlocked &amp;&amp; (isAllowed == false))
    {
        DataRow[] matchedRows = g_numbersTable.Select(string.Format("{0} = {1}", SqlConstants.Tables.ExternalNumbers.IsCorporate, "true"));

        if (matchedRows.Length != 1)
        {
            log.Write(TraceLevel.Info, "OnIncomingCall: No voicemail box defined for user.");
            
            // clear the numbers table to empty the list of numbers to dial
            g_numbersTable.Clear();
        }
        else
        {
            // save matched row prior to clearing table.
            object[] rowArray = matchedRows[0].ItemArray;
            
            // clear the numbers table to empty the list of numbers to dial
            g_numbersTable.Clear();
            DataRow vmailRow = g_numbersTable.NewRow();
            vmailRow.ItemArray = rowArray;
            g_numbersTable.Rows.Add(vmailRow);
        }
    }
    else
    {
        // iterate through all rows in the numbers table
        foreach (DataRow row in g_numbersTable.Rows)
        {
            numberToDial = row[SqlConstants.Tables.ExternalNumbers.PhoneNumber] as string;

            // make sure that the number to dial is valid, and not equal to the originating number.
            if (numberToDial == null || numberToDial == string.Empty || numberToDial.Equals(g_from))
            {
                rowsToRemove.Add(row);
                continue;
            }

            if (g_translatedFrom != null &amp;&amp; numberToDial.Length &gt;= g_significantDigits)
            {
                string baseTo = numberToDial.Substring(numberToDial.Length - g_significantDigits);
                if (g_translatedFrom.IndexOf(baseTo) != -1)
                {
                    log.Write(TraceLevel.Info, "OnIncomingCall: Determined that number '" + numberToDial + "' should not be dialed because the ActiveRelay triggering call originated from it.");
                    rowsToRemove.Add(row);
                    continue;
                }
            }

            // if current entry does not have day/time restrictions
            // or the from number is in the Allow whitelist,
            // do not check the Day/Time settings.
            if ( ! Convert.ToBoolean(row[SqlConstants.Tables.ExternalNumbers.TimeOfDayEnabled]) || isAllowed)
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
    }

    // remove unnecessary rows from table.
    foreach (DataRow rowToRemove in rowsToRemove)
        g_numbersTable.Rows.Remove(rowToRemove);

    // add callId column to DataTable
    DataColumn datacol = new DataColumn("call_id");
    datacol.DataType = typeof(string);
    datacol.DefaultValue = string.Empty;
    g_numbersTable.Columns.Add(datacol);

    datacol = new DataColumn("timer_id");
    datacol.DataType = typeof(string);
    datacol.DefaultValue = string.Empty;
    g_numbersTable.Columns.Add(datacol);

    datacol = new DataColumn("call_state");
    datacol.DataType = typeof(CallState);
    datacol.DefaultValue = CallState.NONE;
    g_numbersTable.Columns.Add(datacol);

    datacol = new DataColumn("user_data");
    datacol.DataType = typeof(string);
    datacol.DefaultValue = "newCall";
    g_numbersTable.Columns.Add(datacol);

    log.Write(TraceLevel.Verbose, "OnIncomingCall: Number of rows in table: " + g_numbersTable.Rows.Count);
    return (g_numbersTable.Rows.Count &gt; 0) ? IApp.VALUE_SUCCESS : "EmptyList";
}
</Properties>
    </node>
    <node type="Action" id="632660197747417584" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="2240.65381" y="319" mx="2292" my="335">
      <items count="1">
        <item text="ProcessDialTable" />
      </items>
      <linkto id="632659276838564889" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632666540064688477" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">ProcessDialTable</ap>
      </Properties>
    </node>
    <node type="Action" id="632666540064688477" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="2254" y="465" mx="2291" my="481">
      <items count="1">
        <item text="HandleError" />
      </items>
      <linkto id="632659276838564889" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="action" type="literal">error_exit</ap>
        <ap name="announceTo" type="literal">default</ap>
        <ap name="error" type="literal">default</ap>
        <ap name="errorType" type="literal">callControl</ap>
        <ap name="FunctionName" type="literal">HandleError</ap>
      </Properties>
    </node>
    <node type="Action" id="632667132589785680" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1133.666" y="334">
      <linkto id="632735766161435742" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="OriginNumber" type="variable">g_from</ap>
        <ap name="DestinationNumber" type="variable">g_to</ap>
        <ap name="UserId" type="variable">g_userId</ap>
        <rd field="CallRecordsId">g_callRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632667132589785681" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="990.666" y="455">
      <linkto id="632660197747416296" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="OriginNumber" type="variable">g_from</ap>
        <ap name="DestinationNumber" type="variable">g_to</ap>
        <rd field="CallRecordsId">g_callRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632667132589785683" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="991" y="693">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632667132589785684" name="SetActiveRelayInfo" class="MaxActionNode" group="" path="Metreos.Native.ActiveRelay" x="1500.666" y="332">
      <linkto id="632678332830509275" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632694114076275528" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
        <ap name="AppRoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="FromNumber" type="variable">g_translatedFrom</ap>
        <ap name="ToNumber" type="variable">g_to</ap>
        <ap name="WasSwapped" type="literal">false</ap>
      </Properties>
    </node>
    <node type="Action" id="632678332830509275" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1645.666" y="221">
      <linkto id="632694114076275528" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_ARRecordExists</rd>
      </Properties>
    </node>
    <node type="Action" id="632694114076275528" name="GetUserCurrentDateTime" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1793.666" y="331">
      <linkto id="632768389716344918" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
        <rd field="DateTime">userTime</rd>
      </Properties>
    </node>
    <node type="Action" id="632735766161435742" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="1266" y="334">
      <linkto id="632667132589785684" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632736982806207000" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">"UPDATE as_users SET current_active_sessions=current_active_sessions + 1 WHERE as_users_id=" + g_userId</ap>
        <ap name="Name" type="variable">db_ConnectionName</ap>
      </Properties>
    </node>
    <node type="Action" id="632736982806207000" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1386" y="222">
      <linkto id="632667132589785684" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_sessionCountIncremented</rd>
      </Properties>
    </node>
    <node type="Action" id="632768389716344918" name="GetActiveRelayFiltersForUser" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="2001.00012" y="331">
      <linkto id="632660197747416301" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632660197747416301" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
        <ap name="IncludeAdmin" type="literal">true</ap>
        <rd field="Filters">filters</rd>
        <log condition="entry" on="true" level="Verbose" type="literal">OnIncomingCall: Retrieving list of blocked/priority-allow numbers.</log>
        <log condition="default" on="true" level="Info" type="literal">OnIncomingCall: Could not obtain list of blocked/priority-allow numbers for user.</log>
      </Properties>
    </node>
    <node type="Variable" id="632472966202366269" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" defaultInitWith="NONE" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632472966202366270" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" defaultInitWith="NONE" refType="reference">from</Properties>
    </node>
    <node type="Variable" id="632472966202366271" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632472966202366272" name="callerCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callerCallId</Properties>
    </node>
    <node type="Variable" id="632664669790126404" name="displayName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DisplayName" defaultInitWith="NONE" refType="reference" name="Metreos.CallControl.IncomingCall">displayName</Properties>
    </node>
    <node type="Variable" id="632694067362331362" name="userTime" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DateTime" refType="reference">userTime</Properties>
    </node>
    <node type="Variable" id="632768389716344917" name="filters" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">filters</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnTimerFire" startnode="632472838633997877" treenode="632472838633997878" appnode="632472838633997875" handlerfor="632472838633997874">
    <node type="Start" id="632472838633997877" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="343">
      <linkto id="632660197747417601" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Comment" id="632488360389763882" text="Check for race condition where we tried&#xD;&#xA;to remove a timer which has already fired,&#xD;&#xA;so it is sitting in our queue, waiting to execute" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="255" y="32" />
    <node type="Action" id="632488360389763884" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="452" y="344">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632660197747417601" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="157" y="344">
      <linkto id="632663009636381113" type="Labeled" style="Bevel" ortho="true" label="Dial" />
      <linkto id="632663009636381114" type="Labeled" style="Bevel" ortho="true" label="Hangup" />
      <linkto id="632666174173462293" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(DataTable g_numbersTable, ref int tableIndex, string userData, LogWriter log)
{
	DataRow row;
	try
	{
		tableIndex = Int32.Parse(userData);
		row = g_numbersTable.Rows[tableIndex];
	}
	catch
	{
		log.Write(TraceLevel.Warning, "OnTimerFire: Unable to retrieve information for data row at index: " + userData);
		return IApp.VALUE_FAILURE;
	} 

	string callId = row["call_id"] as string;
	row["timer_id"] = string.Empty;
	CallState state = (CallState) row["call_state"];

	switch (state)
	{
		case CallState.RING_OUT : return "Hangup";
		case CallState.DIAL_PENDING : return "Dial";
		default : break;
	}
	
	log.Write(TraceLevel.Verbose, "OnTimerFire: Taking no action.");
	return IApp.VALUE_SUCCESS;		  
}
</Properties>
    </node>
    <node type="Action" id="632663009636381113" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="261.825836" y="328" mx="299" my="344">
      <items count="1">
        <item text="DialNumber" />
      </items>
      <linkto id="632488360389763884" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Index" type="variable">tableIndex</ap>
        <ap name="FunctionName" type="literal">DialNumber</ap>
      </Properties>
    </node>
    <node type="Action" id="632663009636381114" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="97.21777" y="509" mx="159" my="525">
      <items count="1">
        <item text="HangupOutboundCall" />
      </items>
      <linkto id="632666540064688472" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Index" type="variable">tableIndex</ap>
        <ap name="FunctionName" type="literal">HangupOutboundCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632666174173462293" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="157" y="210">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632666540064688471" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="290.4707" y="655.1666">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632666540064688472" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="289.4707" y="525.1666">
      <linkto id="632666540064688471" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632666540064688473" type="Labeled" style="Bevel" ortho="true" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_numFailedDialedCalls</ap>
        <ap name="Value2" type="csharp">g_numbersTable.Rows.Count</ap>
      </Properties>
    </node>
    <node type="Action" id="632666540064688473" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="380.4707" y="509.168243" mx="418" my="525">
      <items count="1">
        <item text="HandleError" />
      </items>
      <linkto id="632666540064688471" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="action" type="literal">error_exit</ap>
        <ap name="announceTo" type="literal">default</ap>
        <ap name="error" type="literal">default</ap>
        <ap name="errorType" type="literal">callControl</ap>
        <ap name="FunctionName" type="literal">HandleError</ap>
      </Properties>
    </node>
    <node type="Variable" id="632660197747417599" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="timerUserData" refType="reference" name="Metreos.Providers.TimerFacility.TimerFire">userData</Properties>
    </node>
    <node type="Variable" id="632660197747417600" name="tableIndex" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">tableIndex</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632472838633998020" treenode="632472838633998021" appnode="632472838633998018" handlerfor="632472838633998017">
    <node type="Start" id="632472838633998020" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="760">
      <linkto id="632472838633998147" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632472838633998146" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="133" y="895">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632472838633998147" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="240" y="760">
      <linkto id="632472838633998152" type="Labeled" style="Bevel" ortho="true" label="error_exit" />
      <linkto id="632472838633998148" type="Labeled" style="Bevel" ortho="true" label="exit" />
      <linkto id="632472838633998146" type="Labeled" style="Bezier" label="action" />
      <linkto id="632472838633998146" type="Labeled" style="Bezier" label="none" />
      <linkto id="632696551309577170" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998148" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="239" y="627">
      <linkto id="632472838633998149" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.Unknown</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998149" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="239" y="523">
      <linkto id="632472838633998150" type="Labeled" style="Bevel" ortho="true" label="true" />
      <linkto id="632472838633998151" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_swapPerformed</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998150" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="287" y="443">
      <linkto id="632678332830509272" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_swapCallRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.Unknown</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998151" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="159" y="442">
      <linkto id="632472838633998168" type="Labeled" style="Bevel" ortho="true" label="true" />
      <linkto id="632678332830509272" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_swapRequestReceived</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998152" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="495" y="627">
      <linkto id="632472838633998154" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.InternalError</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998153" name="RemoveActiveRelayRecord" class="MaxActionNode" group="" path="Metreos.Native.ActiveRelay" x="520" y="169">
      <linkto id="632472838633998174" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998154" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="495" y="523">
      <linkto id="632472838633998155" type="Labeled" style="Bevel" ortho="true" label="true" />
      <linkto id="632472838633998156" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_swapPerformed</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998155" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="431" y="443">
      <linkto id="632678332830509272" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_swapCallRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.InternalError</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998156" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="575" y="435">
      <linkto id="632472838633998173" type="Labeled" style="Bevel" ortho="true" label="true" />
      <linkto id="632678332830509272" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_swapRequestReceived</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998168" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="59" y="291" mx="119" my="307">
      <items count="1">
        <item text="SendSwapResponse" />
      </items>
      <linkto id="632678332830509272" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="response" type="literal">fail</ap>
        <ap name="FunctionName" type="literal">SendSwapResponse</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998173" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="551.958" y="296" mx="612" my="312">
      <items count="1">
        <item text="SendSwapResponse" />
      </items>
      <linkto id="632678332830509272" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="response" type="literal">fail</ap>
        <ap name="FunctionName" type="literal">SendSwapResponse</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998174" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="317" y="154" mx="364" my="170">
      <items count="1">
        <item text="ProcessHangup" />
      </items>
      <linkto id="632472838633998175" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="origin" type="literal">caller</ap>
        <ap name="userData" type="literal">default</ap>
        <ap name="FunctionName" type="literal">ProcessHangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998175" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="361" y="67">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632490133710724968" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="894" y="749" mx="968" my="765">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632472838633998180" />
        <item text="OnGatherDigits_Failed" treenode="632472838633998185" />
      </items>
      <linkto id="632490133710724984" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632696551309577171" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxTime" type="variable">getDigitMaxTime</ap>
        <ap name="TermCondDigit" type="variable">g_acceptDigit</ap>
        <ap name="TermCondDigitPattern" type="variable">g_staticTransferPattern</ap>
        <ap name="CommandTimeout" type="literal">0</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="UserData" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632490133710724984" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1157.4707" y="765.166748">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632678332830509272" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="360" y="338">
      <linkto id="632472838633998153" type="Labeled" style="Vector" ortho="true" label="true" />
      <linkto id="632472838633998174" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_ARRecordExists</ap>
      </Properties>
    </node>
    <node type="Action" id="632696551309577161" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="710" y="764">
      <linkto id="632696551309577171" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632697251404431636" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties language="csharp">public static string Execute(DateTime g_playTime, ref int getDigitMaxTime, int g_timeForDigit, int g_swapConfirmWaitAmount, ref int index, string userData, DataTable g_numbersTable)
{
		try
		{
			index = Int32.Parse(userData);
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}

		string internalUserData = g_numbersTable.Rows[index]["user_data"] as string;
		if (internalUserData == null || internalUserData == string.Empty)
			return IApp.VALUE_FAILURE;
		
		int maxTime;
		switch (internalUserData)
		{
			case "swapCall" : maxTime = g_swapConfirmWaitAmount; break;
			default : maxTime = g_timeForDigit; break;
		}

		// calculate the number of milliseconds b/w play start and play end
		// in order to figure out the remaining time that the ReceiveDigits
		// action should be allowed to execute for
		TimeSpan timeElapsedSpan = DateTime.Now.Subtract(g_playTime);
		getDigitMaxTime = (maxTime * 1000) - timeElapsedSpan.Milliseconds;

		// Play took longer than it was supposed to, should never happen
		if (getDigitMaxTime &lt;= 0)
			return IApp.VALUE_FAILURE;
		
		return IApp.VALUE_SUCCESS;		
}</Properties>
    </node>
    <node type="Action" id="632696551309577162" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="652" y="1004.83325" mx="714" my="1021">
      <items count="1">
        <item text="HangupOutboundCall" />
      </items>
      <linkto id="632696551309577164" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Index" type="variable">index</ap>
        <ap name="FunctionName" type="literal">HangupOutboundCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632696551309577163" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="986.2529" y="1186">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632696551309577164" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="847.2528" y="1020">
      <linkto id="632696551309577163" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632696551309577165" type="Labeled" style="Bevel" ortho="true" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_numFailedDialedCalls</ap>
        <ap name="Value2" type="csharp">g_numbersTable.Rows.Count</ap>
      </Properties>
    </node>
    <node type="Action" id="632696551309577165" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="952.2529" y="1004" mx="989" my="1020">
      <items count="1">
        <item text="HandleError" />
      </items>
      <linkto id="632696551309577163" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="action" type="literal">error_exit</ap>
        <ap name="announceTo" type="literal">default</ap>
        <ap name="error" type="literal">default</ap>
        <ap name="errorType" type="literal">media</ap>
        <ap name="FunctionName" type="literal">HandleError</ap>
      </Properties>
    </node>
    <node type="Action" id="632696551309577170" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="583" y="764">
      <linkto id="632696551309577161" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632696551309577171" type="Labeled" style="Bevel" ortho="true" label="maxtime" />
      <linkto id="632700819239849659" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632700819239849659" type="Labeled" style="Bezier" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">terminationCondition</ap>
      </Properties>
    </node>
    <node type="Action" id="632696551309577171" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="709.90625" y="916">
      <linkto id="632696551309577162" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632696551309577164" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string userData, ref int index, ref int g_numFailedDialedCalls, LogWriter log)
{
	try
	{
		index = Int32.Parse(userData);
	}
	catch
	{
		log.Write(TraceLevel.Warning, "OnPlay_Complete: Exception was thrown while attempting to parse userData into table index.");
		g_numFailedDialedCalls++;
		return IApp.VALUE_FAILURE;
	}
	
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632697251404431636" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="821" y="764">
      <linkto id="632490133710724968" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="Block" type="literal">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632700819239849659" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="704" y="650">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632472838633998145" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632490133710724967" name="getDigitMaxTime" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">getDigitMaxTime</Properties>
    </node>
    <node type="Variable" id="632490133710724972" name="terminationCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Play_Complete">terminationCondition</Properties>
    </node>
    <node type="Variable" id="632490921693314367" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference">connectionId</Properties>
    </node>
    <node type="Variable" id="632696551309577173" name="index" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">index</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632472838633998025" treenode="632472838633998026" appnode="632472838633998023" handlerfor="632472838633998022">
    <node type="Start" id="632472838633998025" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="53" y="728">
      <linkto id="632472838633998195" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632472838633998190" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="247" y="579">
      <linkto id="632472838633998192" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.InternalError</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998191" name="RemoveActiveRelayRecord" class="MaxActionNode" group="" path="Metreos.Native.ActiveRelay" x="432" y="106">
      <linkto id="632472838633998208" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998192" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="243" y="449">
      <linkto id="632472838633998193" type="Labeled" style="Bevel" ortho="true" label="true" />
      <linkto id="632472838633998194" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_swapPerformed</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998193" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="419" y="345">
      <linkto id="632678332830509273" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_swapCallRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.InternalError</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998194" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="243" y="353">
      <linkto id="632472838633998206" type="Labeled" style="Bevel" ortho="true" label="true" />
      <linkto id="632678332830509273" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_swapRequestReceived</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998195" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="245" y="728">
      <linkto id="632472838633998196" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632472838633998190" type="Labeled" style="Bezier" label="error_exit" />
      <linkto id="632472838633998190" type="Labeled" style="Bezier" label="exit" />
      <linkto id="632472838633998205" type="Labeled" style="Bevel" ortho="true" label="swapCall" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998196" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="245" y="832">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632472838633998205" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="357" y="712" mx="417" my="728">
      <items count="1">
        <item text="SendSwapResponse" />
      </items>
      <linkto id="632472838633998207" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="response" type="literal">fail</ap>
        <ap name="FunctionName" type="literal">SendSwapResponse</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998206" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="41" y="338" mx="101" my="354">
      <items count="1">
        <item text="SendSwapResponse" />
      </items>
      <linkto id="632678332830509273" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="response" type="literal">fail</ap>
        <ap name="FunctionName" type="literal">SendSwapResponse</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998207" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="576" y="728">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632472838633998208" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="195" y="91" mx="242" my="107">
      <items count="1">
        <item text="ProcessHangup" />
      </items>
      <linkto id="632472838633998209" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="origin" type="literal">caller</ap>
        <ap name="userData" type="literal">default</ap>
        <ap name="FunctionName" type="literal">ProcessHangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998209" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="240" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632678332830509273" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="241" y="243">
      <linkto id="632472838633998191" type="Labeled" style="Vector" ortho="true" label="true" />
      <linkto id="632472838633998208" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_ARRecordExists</ap>
      </Properties>
    </node>
    <node type="Variable" id="632472838633998204" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632472838633998063" treenode="632472838633998064" appnode="632472838633998061" handlerfor="632472838633998060">
    <node type="Start" id="632472838633998063" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="373">
      <linkto id="632703305376792804" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632472838633998342" text="If we get OnHangup&#xD;&#xA;before the timer fires,&#xD;&#xA;then the call was answered&#xD;&#xA;by the dialed party, and appserver&#xD;&#xA;does not need to handle this call.&#xD;&#xA;Thus, we remove the media connection&#xD;&#xA;and we &#xD;&#xA;exit the application." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="81" y="50" />
    <node type="Action" id="632484810722400396" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="618" y="371">
      <linkto id="632703532838705736" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632700150959162742" type="Labeled" style="Bevel" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">isCorporate</ap>
      </Properties>
    </node>
    <node type="Action" id="632484810722400464" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="438" y="372">
      <linkto id="632484810722400396" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632663646668755178" type="Labeled" style="Bevel" ortho="true" label="RemoveTimer" />
      <linkto id="632673314455189200" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(int index, string userData, ref string timerId, ref bool isCorporate, DataTable g_numbersTable, Hashtable g_makeCallState, string calleeCallId, string calleeMediaIp, ushort calleeMediaPort, string swapResult, string conferenceId, string connectionId, string mmsId, LogWriter log)
{
	DataRow row;
	try { row = g_numbersTable.Rows[index]; }
	catch { return IApp.VALUE_FAILURE; }

	CallState callState = (CallState) row["call_state"];
	if (callState == CallState.ENDED)
	{
		log.Write(TraceLevel.Info, "OnMakeCall_Complete: Call already ended.");
		return "CallEnded";
	}
	row["call_state"] = CallState.CONNECTED;	

	timerId = row["timer_id"] as string;
	if (timerId != null)
	{
		timerId = string.Copy(timerId);
		row["timer_id"] = string.Empty;
	}
	else
		row["timer_id"] = string.Empty;

	isCorporate = Convert.ToBoolean(row[SqlConstants.Tables.ExternalNumbers.IsCorporate]);
	if (isCorporate)
		row["user_data"] = "transfer";
			
	object[] state = new object[] { calleeCallId, calleeMediaIp, calleeMediaPort, "newCall", swapResult, conferenceId, mmsId };

	g_makeCallState[connectionId] = state;

	if (timerId == null || timerId == string.Empty)
		return IApp.VALUE_SUCCESS;
	else
		return "RemoveTimer";
}
</Properties>
    </node>
    <node type="Comment" id="632484810722400465" text="If a digit press is required&#xD;&#xA;on the forwarded device,&#xD;&#xA;we save the state of the make &#xD;&#xA;call complete event params, &#xD;&#xA;and initiate GatherDigits" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="466" y="129" />
    <node type="Action" id="632484810722400466" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="584" y="558" mx="621" my="574">
      <items count="1">
        <item text="Answer" />
      </items>
      <linkto id="632696551309578545" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="connectionId" type="variable">connectionId</ap>
        <ap name="FunctionName" type="literal">Answer</ap>
      </Properties>
    </node>
    <node type="Action" id="632484810722400471" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="619" y="842">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632484810722400947" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1184" y="370">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632485420083424241" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="908" y="370">
      <linkto id="632490133710724958" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">DateTime.Now</ap>
        <rd field="ResultData">g_playTime</rd>
      </Properties>
    </node>
    <node type="Action" id="632490133710724958" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="987" y="355" mx="1040" my="371">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632472838633998021" />
        <item text="OnPlay_Failed" treenode="632472838633998026" />
      </items>
      <linkto id="632484810722400947" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632673314455189199" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_confirmReceiptAudioFile</ap>
        <ap name="Prompt3" type="variable">g_confirmReceiptAudioFile</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="TermCondMaxTime" type="csharp">g_timeForDigit * 1000</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_confirmReceiptAudioFile</ap>
        <ap name="UserData" type="variable">userData</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnMakeCall_Complete: playing digit confirmation announcement.</log>
        <log condition="default" on="true" level="Warning" type="literal">OnMakeCall_Complete: failed to play request for call receipt confirmation.</log>
      </Properties>
    </node>
    <node type="Action" id="632663646668755178" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="523" y="250">
      <linkto id="632484810722400396" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">timerId</ap>
      </Properties>
    </node>
    <node type="Action" id="632671561491540663" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1036.94141" y="736.1667">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632671561491540664" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1036.94141" y="610.1667">
      <linkto id="632671561491540663" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632671561491540666" type="Labeled" style="Bevel" ortho="true" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_numFailedDialedCalls</ap>
        <ap name="Value2" type="csharp">g_numbersTable.Rows.Count</ap>
      </Properties>
    </node>
    <node type="Comment" id="632671561491540665" text="All calls failed..." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1101.94141" y="541.1667" />
    <node type="Action" id="632671561491540666" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1126.94141" y="594.1667" mx="1164" my="610">
      <items count="1">
        <item text="HandleError" />
      </items>
      <linkto id="632671561491540663" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="action" type="literal">error_exit</ap>
        <ap name="announceTo" type="literal">default</ap>
        <ap name="error" type="literal">default</ap>
        <ap name="errorType" type="literal">media</ap>
        <ap name="FunctionName" type="literal">HandleError</ap>
      </Properties>
    </node>
    <node type="Action" id="632673314455189199" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1036" y="502">
      <linkto id="632671561491540664" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">g_numFailedDialedCalls + 1</ap>
        <rd field="ResultData">g_numFailedDialedCalls</rd>
      </Properties>
    </node>
    <node type="Label" id="632673314455189200" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="438" y="491" />
    <node type="Label" id="632673314455189201" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="915" y="502">
      <linkto id="632673314455189199" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632674146742757022" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="266" y="372">
      <linkto id="632484810722400464" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632674146742757023" type="Labeled" style="Bevel" ortho="true" label="swapCall" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">internalUserData</ap>
      </Properties>
    </node>
    <node type="Action" id="632674146742757023" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="266" y="532">
      <linkto id="632674146742757024" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Hashtable g_makeCallState, string calleeCallId, string calleeMediaIp, ushort calleeMediaPort, string swapResult, string conferenceId, string connectionId, string mmsId, int index, DataTable g_numbersTable)
{
	object[] state = new object[] { calleeCallId, calleeMediaIp, calleeMediaPort, "swapCall", swapResult, conferenceId, mmsId };

	g_makeCallState[connectionId] = state;
	g_numbersTable.Rows[index]["call_state"] = CallState.CONNECTED;
	g_numbersTable.Rows[index]["user_data"] = "transfer";

	return IApp.VALUE_SUCCESS;
}

</Properties>
    </node>
    <node type="Action" id="632674146742757024" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="232" y="674" mx="269" my="690">
      <items count="1">
        <item text="Answer" />
      </items>
      <linkto id="632696551309578545" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="connectionId" type="variable">connectionId</ap>
        <ap name="FunctionName" type="literal">Answer</ap>
      </Properties>
    </node>
    <node type="Action" id="632696551309578545" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="547.4707" y="676" mx="622" my="692">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632472838633998180" />
        <item text="OnGatherDigits_Failed" treenode="632472838633998185" />
      </items>
      <linkto id="632484810722400471" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigitPattern" type="variable">g_staticTransferPattern</ap>
        <ap name="CommandTimeout" type="literal">0</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="UserData" type="variable">userData</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnMakeCall_Complete: Listening for Static Transfer digit pattern.</log>
      </Properties>
    </node>
    <node type="Action" id="632700150959162743" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="769.4707" y="574">
      <linkto id="632485420083424241" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">confirm_forward_pound.wav</ap>
        <rd field="ResultData">g_confirmReceiptAudioFile</rd>
      </Properties>
    </node>
    <node type="Action" id="632703305376792804" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="134" y="372">
      <linkto id="632674146742757022" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632703305376792806" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="default" on="true" level="Warning" type="literal">OnMakeCall_Complete: Could not associate event to an entry in call object table.</log>
public static string Execute(ref int index, string userData, ref string internalUserData, DataTable g_numbersTable)
{
	try {	index = Convert.ToInt32(userData); }
	catch { return IApp.VALUE_FAILURE; }

	internalUserData = g_numbersTable.Rows[index]["user_data"] as string;
	if (internalUserData == null || internalUserData == string.Empty)
	{
		internalUserData = string.Empty;
		return IApp.VALUE_FAILURE;
	}
	else
		return IApp.VALUE_SUCCESS;
}	
</Properties>
    </node>
    <node type="Action" id="632703305376792806" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="134" y="495">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632703532838705736" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="619" y="478">
      <linkto id="632484810722400466" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">connectionId</ap>
        <ap name="Value2" type="variable">calleeCallId</ap>
        <rd field="ResultData">g_connectionId_callee</rd>
        <rd field="ResultData2">g_callId_callee</rd>
      </Properties>
    </node>
    <node type="Action" id="632749212435103242" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="769.4707" y="472">
      <linkto id="632700150959162743" type="Labeled" style="Bevel" ortho="true" label="true" />
      <linkto id="632485420083424241" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_useSpecificPrompt</ap>
      </Properties>
    </node>
    <node type="Action" id="632700150959162742" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="769.4707" y="370">
      <linkto id="632485420083424241" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632749212435103242" type="Labeled" style="Bevel" ortho="true" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_acceptDigit</ap>
        <ap name="Value2" type="literal">#</ap>
      </Properties>
    </node>
    <node type="Variable" id="632472966202366275" name="calleeCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference">calleeCallId</Properties>
    </node>
    <node type="Variable" id="632472966202366276" name="calleeMediaIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="MediaTxIP" refType="reference">calleeMediaIp</Properties>
    </node>
    <node type="Variable" id="632472966202366277" name="calleeMediaPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UShort" initWith="MediaTxPort" refType="reference">calleeMediaPort</Properties>
    </node>
    <node type="Variable" id="632472966202366278" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632472966202366279" name="swapResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">swapResult</Properties>
    </node>
    <node type="Variable" id="632473570363593482" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference">connectionId</Properties>
    </node>
    <node type="Variable" id="632482183326619271" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConferenceId" refType="reference">conferenceId</Properties>
    </node>
    <node type="Variable" id="632663646668755176" name="index" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">index</Properties>
    </node>
    <node type="Variable" id="632663646668755177" name="isCorporate" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">isCorporate</Properties>
    </node>
    <node type="Variable" id="632663646668755193" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">timerId</Properties>
    </node>
    <node type="Variable" id="632697370120313535" name="mmsId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="MmsId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">mmsId</Properties>
    </node>
    <node type="Variable" id="632703305376792805" name="internalUserData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">internalUserData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632472838633998068" treenode="632472838633998069" appnode="632472838633998066" handlerfor="632472838633998065">
    <node type="Start" id="632472838633998068" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="369">
      <linkto id="632669923407908720" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632472838633998307" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="687" y="369">
      <linkto id="632472838633998313" type="Labeled" style="Bevel" ortho="true" label="swapCall" />
      <linkto id="632666540064688465" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">internalUserData</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Failed: Function Entry</log>
      </Properties>
    </node>
    <node type="Action" id="632472838633998309" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="965" y="368">
      <linkto id="632472838633998310" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="OriginNumber" type="variable">g_from</ap>
        <ap name="DestinationNumber" type="variable">g_to</ap>
        <ap name="UserId" type="variable">g_userId</ap>
        <rd field="CallRecordsId">g_swapCallRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632472838633998310" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1135" y="368">
      <linkto id="632472838633998322" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_swapCallRecordId</ap>
        <ap name="EndReason" type="variable">endReason</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998312" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="292" y="369">
      <linkto id="632665597252790820" type="Labeled" style="Bevel" ortho="true" label="RemoveTimer" />
      <linkto id="632666540064688465" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632797562403354945" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties language="csharp">
	public static string Execute(ref int g_numFailedDialedCalls, ref int index, string userData, ref string timerId, ref string endReason, DataTable g_numbersTable, ref string internalUserData)
	{
		// Metreos.Interfaces.ICallControl should provide string constants that
		// can be used here.
		switch (endReason)
		{
			case "Busy" : 
			case "Normal" : 
			case "Ringout" :
			case "Unreachable" :
			case "InternalError" : break;
			default : endReason = "Unknown"; break;
		}

		// increment the number of failed dialed calls
		g_numFailedDialedCalls++;
		
		DataRow row;
		try
		{
			index = Convert.ToInt32(userData);
			row = g_numbersTable.Rows[index];
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
		
		internalUserData = row["user_data"] as string;
		if (internalUserData == "swapCall")
			return IApp.VALUE_SUCCESS;

		timerId = row["timer_id"] as string;
		row["call_state"] = CallState.ENDED;

		if (timerId != null)
		{
			timerId = string.Copy(timerId);
			row["timer_id"] = string.Empty;
		}
		else
			row["timer_id"] = string.Empty;
		
		if (timerId == null || timerId == string.Empty)
			return IApp.VALUE_SUCCESS;
		else
			return "RemoveTimer";
}
</Properties>
    </node>
    <node type="Action" id="632472838633998313" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="845" y="368">
      <linkto id="632472838633998309" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="literal">true</ap>
        <rd field="ResultData">g_callAnswered</rd>
        <rd field="ResultData2">g_allowSwap</rd>
      </Properties>
    </node>
    <node type="Action" id="632472838633998322" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1223" y="352" mx="1283" my="368">
      <items count="1">
        <item text="SendSwapResponse" />
      </items>
      <linkto id="632696551309578550" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="response" type="literal">fail</ap>
        <ap name="FunctionName" type="literal">SendSwapResponse</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998323" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1589" y="368">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632665597252790820" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="412.998016" y="265">
      <linkto id="632797562403354945" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">timerId</ap>
      </Properties>
    </node>
    <node type="Action" id="632665597252790824" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="688" y="693">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632666540064688465" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="687" y="530">
      <linkto id="632665597252790824" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632666540064688467" type="Labeled" style="Bevel" ortho="true" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_numFailedDialedCalls</ap>
        <ap name="Value2" type="csharp">g_numbersTable.Rows.Count</ap>
      </Properties>
    </node>
    <node type="Comment" id="632666540064688466" text="All calls failed..." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="861" y="478" />
    <node type="Action" id="632666540064688467" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="777" y="514" mx="814" my="530">
      <items count="1">
        <item text="HandleError" />
      </items>
      <linkto id="632665597252790824" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="action" type="literal">error_exit</ap>
        <ap name="announceTo" type="literal">default</ap>
        <ap name="error" type="literal">default</ap>
        <ap name="errorType" type="literal">callControl</ap>
        <ap name="reason" type="variable">endReason</ap>
        <ap name="FunctionName" type="literal">HandleError</ap>
      </Properties>
    </node>
    <node type="Action" id="632669923407908720" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="155" y="369">
      <linkto id="632472838633998312" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"OnMakeCall_Failed: Call failed with following EndReason: " + endReason</ap>
        <ap name="LogLevel" type="literal">Warning</ap>
        <ap name="ApplicationName" type="literal">ActiveRelay</ap>
      </Properties>
    </node>
    <node type="Action" id="632696551309578550" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1374.4707" y="352" mx="1449" my="368">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632472838633998180" />
        <item text="OnGatherDigits_Failed" treenode="632472838633998185" />
      </items>
      <linkto id="632472838633998323" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigitPattern" type="variable">g_staticTransferPattern</ap>
        <ap name="CommandTimeout" type="literal">0</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_callee</ap>
        <ap name="UserData" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632797562403354945" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="479.580078" y="353" mx="537" my="369">
      <items count="1">
        <item text="WriteFindMeRecord" />
      </items>
      <linkto id="632472838633998307" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Index" type="variable">index</ap>
        <ap name="EndReason" type="variable">endReason</ap>
        <ap name="FunctionName" type="literal">WriteFindMeRecord</ap>
      </Properties>
    </node>
    <node type="Variable" id="632472966202366280" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632472966202366281" name="reason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">reason</Properties>
    </node>
    <node type="Variable" id="632665597252790818" name="index" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">index</Properties>
    </node>
    <node type="Variable" id="632665597252790819" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">timerId</Properties>
    </node>
    <node type="Variable" id="632669923407908719" name="endReason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="EndReason" defaultInitWith="NONE" refType="reference" name="Metreos.CallControl.MakeCall_Failed">endReason</Properties>
    </node>
    <node type="Variable" id="632696551309578549" name="internalUserData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">internalUserData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632472838633998073" treenode="632472838633998074" appnode="632472838633998071" handlerfor="632472838633998070">
    <node type="Start" id="632472838633998073" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="360">
      <linkto id="632678332830509277" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632472838633998348" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="428" y="481">
      <linkto id="632472838633998351" type="Labeled" style="Bevel" ortho="true" label="equal" />
      <linkto id="632670653296732623" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_callId_callee</ap>
      </Properties>
    </node>
    <node type="Comment" id="632472838633998349" text="If the hangup didn't come&#xD;&#xA;from the caller, we &#xD;&#xA;check to see if it came&#xD;&#xA;from the callee. If so,&#xD;&#xA;we set &quot;origin&quot; to equal&#xD;&#xA;&quot;callee&quot;&#xD;&#xA;If the hangup didn't come&#xD;&#xA;from the callee, we end &#xD;&#xA;end function, since at this&#xD;&#xA;point we also know that the &#xD;&#xA;hangup did not come from&#xD;&#xA;the caller.&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="57" y="501" />
    <node type="Action" id="632472838633998350" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="427" y="748">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnHangup: Hangup received, but callId \"" + callId + "\" was not recognized. Exiting function."</log>
      </Properties>
    </node>
    <node type="Action" id="632472838633998351" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="536" y="481">
      <linkto id="632800221463702415" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">callee</ap>
        <ap name="Value2" type="literal">false</ap>
        <ap name="Value3" type="literal">default</ap>
        <rd field="ResultData">origin</rd>
        <rd field="ResultData2">g_isCalleeConnected</rd>
        <rd field="ResultData3">typeOfHangup</rd>
      </Properties>
    </node>
    <node type="Action" id="632472838633998352" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="472" y="360">
      <linkto id="632472838633998373" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">caller</ap>
        <ap name="Value2" type="literal">true</ap>
        <ap name="Value3" type="literal">all_calls</ap>
        <rd field="ResultData">origin</rd>
        <rd field="ResultData2">g_didCallerHangup</rd>
        <rd field="ResultData3">typeOfHangup</rd>
      </Properties>
    </node>
    <node type="Action" id="632472838633998354" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="272" y="360">
      <linkto id="632472838633998348" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632472838633998352" type="Labeled" style="Bevel" ortho="true" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_callId_caller</ap>
      </Properties>
    </node>
    <node type="Comment" id="632472838633998356" text="We check to see if the Hangup came from the caller.&#xD;&#xA;If so, we set the &quot;origin&quot; variable to &#xD;&#xA;contain the value &quot;caller&quot;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="336" y="224" />
    <node type="Comment" id="632472838633998357" text="We call &quot;ProcessHangup&quot;, passing&#xD;&#xA;in the value representing the originator&#xD;&#xA;of the hangup and the callId that hung-up." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="656" y="156" />
    <node type="Comment" id="632472838633998371" text="If we get OnHangup&#xD;&#xA;before the timer fires,&#xD;&#xA;then the call was answered&#xD;&#xA;by the dialed party, and appserver&#xD;&#xA;does not need to handle this call.&#xD;&#xA;Thus, we remove the media connection&#xD;&#xA;and we &#xD;&#xA;exit the application." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="726" />
    <node type="Action" id="632472838633998373" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="763" y="345" mx="810" my="361">
      <items count="1">
        <item text="ProcessHangup" />
      </items>
      <linkto id="632472838633998374" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="origin" type="variable">origin</ap>
        <ap name="userData" type="variable">typeOfHangup</ap>
        <ap name="reason" type="variable">endReason</ap>
        <ap name="FunctionName" type="literal">ProcessHangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998374" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="939" y="361">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632670653296732623" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="428" y="580">
      <linkto id="632670653296732624" type="Labeled" style="Bevel" ortho="true" label="Hangup" />
      <linkto id="632472838633998350" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Hashtable g_callIdToTableIndexMap, string callId, ref int index)
{
	object indobj = g_callIdToTableIndexMap[callId];
	if (indobj != null)
	{
		index = (int) indobj;
		return "Hangup";
	}
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632670653296732624" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="496.217773" y="565" mx="558" my="581">
      <items count="1">
        <item text="HangupOutboundCall" />
      </items>
      <linkto id="632670653296732626" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Index" type="variable">index</ap>
        <ap name="FunctionName" type="literal">HangupOutboundCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632670653296732625" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="685.470642" y="706.1667">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632670653296732626" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="685.470642" y="582.1667">
      <linkto id="632670653296732625" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632670653296732627" type="Labeled" style="Bevel" ortho="true" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_numFailedDialedCalls</ap>
        <ap name="Value2" type="csharp">g_numbersTable.Rows.Count</ap>
      </Properties>
    </node>
    <node type="Action" id="632670653296732627" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="775.470642" y="567.1667" mx="813" my="583">
      <items count="1">
        <item text="HandleError" />
      </items>
      <linkto id="632670653296732625" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="action" type="literal">error_exit</ap>
        <ap name="announceTo" type="literal">default</ap>
        <ap name="error" type="literal">default</ap>
        <ap name="errorType" type="literal">callControl</ap>
        <ap name="reason" type="variable">endReason</ap>
        <ap name="FunctionName" type="literal">HandleError</ap>
      </Properties>
    </node>
    <node type="Action" id="632678332830509277" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="136" y="360">
      <linkto id="632472838633998354" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string endReason)
{
	// Metreos.Interfaces.ICallControl should provide string constants that
	// can be used here.
	switch (endReason)
	{
		case "Busy" : 
		case "Normal" : 
		case "Ringout" :
		case "Unreachable" :
		case "InternalError" : break;
		default : endReason = "Unknown"; break;
	}

	return IApp.VALUE_SUCCESS;
}</Properties>
    </node>
    <node type="Action" id="632800221463702413" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="752" y="466" mx="809" my="482">
      <items count="1">
        <item text="WriteFindMeRecord" />
      </items>
      <linkto id="632472838633998373" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Index" type="variable">index</ap>
        <ap name="EndReason" type="variable">endReason</ap>
        <ap name="FunctionName" type="literal">WriteFindMeRecord</ap>
      </Properties>
    </node>
    <node type="Action" id="632800221463702415" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="653.8131" y="482">
      <linkto id="632472838633998373" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632800221463702413" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties language="csharp">public static string Execute(Hashtable g_callIdToTableIndexMap, string callId, ref int index, LogWriter log)
{
	object indobj = g_callIdToTableIndexMap[callId];
	if (indobj != null)
	{
		index = (int) indobj;
		return IApp.VALUE_SUCCESS;
	}

	log.Write(TraceLevel.Info, "Could not find table index for FindMe call with callId: " + callId);
	return IApp.VALUE_FAILURE;
}

</Properties>
    </node>
    <node type="Variable" id="632472966202366282" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632472966202366283" name="origin" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">origin</Properties>
    </node>
    <node type="Variable" id="632472966202366284" name="typeOfHangup" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">typeOfHangup</Properties>
    </node>
    <node type="Variable" id="632670653296732622" name="index" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">index</Properties>
    </node>
    <node type="Variable" id="632678332830509278" name="endReason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="EndReason" defaultInitWith="NONE" refType="reference" name="Metreos.CallControl.RemoteHangup">endReason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" startnode="632472838633998179" treenode="632472838633998180" appnode="632472838633998177" handlerfor="632472838633998176">
    <node type="Start" id="632472838633998179" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="239">
      <linkto id="632696551309578534" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632472838633998214" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1198" y="1079" mx="1258" my="1095">
      <items count="1">
        <item text="SendSwapResponse" />
      </items>
      <linkto id="632485420083421747" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="response" type="literal">attempting</ap>
        <ap name="FunctionName" type="literal">SendSwapResponse</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998216" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="635.958" y="1237" mx="696" my="1253">
      <items count="1">
        <item text="SendSwapResponse" />
      </items>
      <linkto id="632472838633998217" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="response" type="literal">rejected</ap>
        <ap name="FunctionName" type="literal">SendSwapResponse</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Digit confirmation failure.  Termination Condition was: " + terminationReason</log>
      </Properties>
    </node>
    <node type="Action" id="632472838633998217" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="694" y="1411">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632484810722400399" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1018.82593" y="286" mx="1056" my="302">
      <items count="1">
        <item text="Answer" />
      </items>
      <linkto id="632673314455190684" type="Labeled" style="Bevel" ortho="true" label="Failure" />
      <linkto id="632699947423161355" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="connectionId" type="variable">connectionId</ap>
        <ap name="FunctionName" type="literal">Answer</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnGatherDigits_Complete: Answering call based on digit confirmation</log>
      </Properties>
    </node>
    <node type="Action" id="632485420083421740" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="692" y="1096">
      <linkto id="632472838633998216" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632673314455190693" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">terminationReason</ap>
      </Properties>
    </node>
    <node type="Action" id="632485420083421747" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1420" y="1095">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632673314455190679" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="367" y="239">
      <linkto id="632699947423161351" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632699947423161353" type="Labeled" style="Bezier" ortho="true" label="swapCall" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">internalUserData</ap>
      </Properties>
    </node>
    <node type="Action" id="632673314455190680" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="228" y="240">
      <linkto id="632673314455190679" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632696551309578535" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string userData, ref string internalUserData, ref int index, DataTable g_numbersTable, LogWriter log)
{
	try
	{
		index = Int32.Parse(userData);
	}
	catch
	{
		log.Write(TraceLevel.Warning, "OnGatherDigits_Complete: Exception was thrown while attempting to parse userData into table index.");
		return IApp.VALUE_FAILURE;
	}

	internalUserData = g_numbersTable.Rows[index]["user_data"] as string;
	if (internalUserData == null || internalUserData == string.Empty)
	{
		internalUserData = string.Empty;
		return IApp.VALUE_FAILURE;
	}	

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632673314455190682" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="726" y="302">
      <linkto id="632696551309578539" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <linkto id="632699947423161350" type="Labeled" style="Vector" ortho="true" label="maxtime" />
      <linkto id="632701531416387402" type="Labeled" style="Bezier" ortho="true" label="digitpattern" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">terminationReason</ap>
      </Properties>
    </node>
    <node type="Action" id="632673314455190684" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1229" y="302">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632673314455190693" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="833" y="1096">
      <linkto id="632472838633998216" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632673314455190696" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties language="csharp">
public static string Execute(DataTable g_numbersTable, string g_swapNumber, uint g_swapUserId, string callId, ref int index, LogWriter log)
{
	// real ghetto - since we don't have an ID factory, we use a timestamp
	// to find the record.
	string dtNow = DateTime.Now.ToString();
	bool success = false;

	DataRow newRow = g_numbersTable.NewRow();
	newRow[SqlConstants.Tables.ExternalNumbers.Id] = 0;
	newRow[SqlConstants.Tables.ExternalNumbers.UserId] = g_swapUserId;
	newRow[SqlConstants.Tables.ExternalNumbers.Name] = "SwapNumber";
	newRow[SqlConstants.Tables.ExternalNumbers.PhoneNumber] = g_swapNumber;
	newRow[SqlConstants.Tables.ExternalNumbers.DelayCallTime] = 0;
	newRow[SqlConstants.Tables.ExternalNumbers.CallAttemptTimeout] = 0;
	newRow[SqlConstants.Tables.ExternalNumbers.IsCorporate] = true;
	newRow[SqlConstants.Tables.ExternalNumbers.ArEnabled] = true;
	newRow[SqlConstants.Tables.ExternalNumbers.TimeOfDayEnabled] = false;
	newRow["call_state"]  = CallState.NONE;
	newRow["call_id"] = string.Empty;
	newRow["user_data"] = dtNow;
	g_numbersTable.Rows.Add(newRow);

	for (int i = g_numbersTable.Rows.Count; i &gt; 0; i--)
	{
		if ((g_numbersTable.Rows[i - 1]["user_data"] as string) == dtNow)
		{
			index = i - 1;
			newRow["user_data"] = "swapCall";
			success = true;
			break;
		}	
	}

	if (success)
		return IApp.VALUE_SUCCESS;
	else
	{
		log.Write(TraceLevel.Info, "OnGatherDigits_Complete: could not determine index of swapCall record.");
		return IApp.VALUE_FAILURE;
	}
}
</Properties>
    </node>
    <node type="Action" id="632673314455190696" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="963" y="1095">
      <linkto id="632703305376792802" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">0</ap>
        <rd field="ResultData">g_numFailedDialedCalls</rd>
      </Properties>
    </node>
    <node type="Action" id="632673314455190707" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1037" y="1231" mx="1097" my="1247">
      <items count="1">
        <item text="SendSwapResponse" />
      </items>
      <linkto id="632673314455190709" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="response" type="literal">fail</ap>
        <ap name="FunctionName" type="literal">SendSwapResponse</ap>
      </Properties>
    </node>
    <node type="Action" id="632673314455190709" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1094" y="1410">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632696551309578534" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="129" y="240">
      <linkto id="632696551309578535" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632673314455190680" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632696551309578535" type="Labeled" style="Bezier" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">terminationReason</ap>
      </Properties>
    </node>
    <node type="Action" id="632696551309578535" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="130" y="99">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632696551309578539" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="862" y="302">
      <linkto id="632484810722400399" type="Labeled" style="Bezier" ortho="true" label="Answer" />
      <linkto id="632699947423161355" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string digits, DataTable g_numbersTable, int index, string internalUserData, LogWriter log)
{ 
	if (digits == null)
		return IApp.VALUE_FAILURE;
	else
	{
		DataRow row = g_numbersTable.Rows[index];
		switch (internalUserData)
		{
			case "newCall" :
				{
					row["user_data"] = "transfer";
					if (Convert.ToBoolean(row[SqlConstants.Tables.ExternalNumbers.IsCorporate]))
						return "default";

					return "Answer";
				}
			default : return "default";
		}
	}

	return "default";
}

</Properties>
    </node>
    <node type="Action" id="632697042194556595" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="929.4707" y="142">
      <linkto id="632699824275512009" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(string digits, DataTable g_numbersTable, int index, string internalUserData, LogWriter log, int g_userId, string g_transferPrefix, string g_transferNumber, string g_internationalPrefix, ref int g_numFailedDialedCalls, ref int g_numDialedCalls)
{
	g_numDialedCalls = g_numFailedDialedCalls = 0;
	g_numbersTable.Rows.Clear();

	DataRow newRow = g_numbersTable.NewRow();
	newRow[SqlConstants.Tables.ExternalNumbers.Id] = 0;
	newRow[SqlConstants.Tables.ExternalNumbers.UserId] = g_userId;
	newRow[SqlConstants.Tables.ExternalNumbers.Name] = "Forward Number";
	newRow[SqlConstants.Tables.ExternalNumbers.PhoneNumber] = g_transferPrefix + g_transferNumber.Replace("+", g_internationalPrefix);
	newRow[SqlConstants.Tables.ExternalNumbers.DelayCallTime] = 0;
	newRow[SqlConstants.Tables.ExternalNumbers.CallAttemptTimeout] = 0;
	newRow[SqlConstants.Tables.ExternalNumbers.IsCorporate] = true;
	newRow[SqlConstants.Tables.ExternalNumbers.ArEnabled] = true;
	newRow[SqlConstants.Tables.ExternalNumbers.TimeOfDayEnabled] = false;
	newRow["call_state"]  = CallState.NONE;
	newRow["user_data"] = "transfer";
	g_numbersTable.Rows.Add(newRow);

	return IApp.VALUE_SUCCESS;
}</Properties>
    </node>
    <node type="Action" id="632699824275512001" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="796" y="127" mx="833" my="143">
      <items count="1">
        <item text="HangUpList" />
      </items>
      <linkto id="632697042194556595" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="exception" type="literal">none</ap>
        <ap name="userData" type="literal">all_calls</ap>
        <ap name="FunctionName" type="literal">HangUpList</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnGatherDigits_Complete: Hanging up outstanding calls.</log>
      </Properties>
    </node>
    <node type="Action" id="632699824275512004" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1192.34607" y="125" mx="1230" my="141">
      <items count="1">
        <item text="HandleError" />
      </items>
      <linkto id="632673314455190684" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="action" type="literal">error_exit</ap>
        <ap name="announceTo" type="literal">default</ap>
        <ap name="error" type="literal">default</ap>
        <ap name="errorType" type="literal">callControl</ap>
        <ap name="FunctionName" type="literal">HandleError</ap>
      </Properties>
    </node>
    <node type="Action" id="632699824275512009" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1009.62988" y="125" mx="1047" my="141">
      <items count="1">
        <item text="DialNumber" />
      </items>
      <linkto id="632673314455190684" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632699824275512004" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Index" type="literal">0</ap>
        <ap name="FunctionName" type="literal">DialNumber</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnGatherDigits_Complete: Dialing transfer number.</log>
      </Properties>
    </node>
    <node type="Action" id="632699947423161350" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="856" y="461">
      <linkto id="632699947423161355" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632699947423161360" type="Labeled" style="Bezier" ortho="true" label="TimedOut" />
      <Properties language="csharp">public static string Execute(DataTable g_numbersTable, int index, string internalUserData, LogWriter log)
{ 
	DataRow row = g_numbersTable.Rows[index];
	switch (internalUserData)
	{
		case "newCall" :
			{
				if (Convert.ToBoolean(row[SqlConstants.Tables.ExternalNumbers.IsCorporate]))
				{
					row["user_data"] = "transfer";
					return "default";
				}
					return "TimedOut";
			}
		default : return "default";
	}

	return "default";
}


</Properties>
    </node>
    <node type="Label" id="632699947423161351" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="479" y="238" />
    <node type="Label" id="632699947423161352" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="651" y="302">
      <linkto id="632673314455190682" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632699947423161353" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="369" y="405" />
    <node type="Label" id="632699947423161354" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="618" y="1096">
      <linkto id="632485420083421740" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632699947423161355" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="984" y="445" mx="1058" my="461">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632472838633998180" />
        <item text="OnGatherDigits_Failed" treenode="632472838633998185" />
      </items>
      <linkto id="632699947423161359" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigitPattern" type="variable">g_staticTransferPattern</ap>
        <ap name="CommandTimeout" type="literal">0</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="UserData" type="variable">userData</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnGatherDigits_Complete: Waiting for Static Transfer digit pattern</log>
      </Properties>
    </node>
    <node type="Action" id="632699947423161359" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1223" y="460">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632699947423161360" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="800" y="650.833252" mx="862" my="667">
      <items count="1">
        <item text="HangupOutboundCall" />
      </items>
      <linkto id="632699947423161362" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Index" type="variable">index</ap>
        <ap name="FunctionName" type="literal">HangupOutboundCall</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnGatherDigits_Complete: Call confirmation timed out. Hanging up call.</log>
      </Properties>
    </node>
    <node type="Action" id="632699947423161361" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1143.25293" y="666">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632699947423161362" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="975.2529" y="666">
      <linkto id="632699947423161361" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632699947423161363" type="Labeled" style="Bevel" ortho="true" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_numFailedDialedCalls</ap>
        <ap name="Value2" type="csharp">g_numbersTable.Rows.Count</ap>
      </Properties>
    </node>
    <node type="Action" id="632699947423161363" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1109.25293" y="773" mx="1146" my="789">
      <items count="1">
        <item text="HandleError" />
      </items>
      <linkto id="632699947423161361" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="action" type="literal">error_exit</ap>
        <ap name="announceTo" type="literal">default</ap>
        <ap name="error" type="literal">default</ap>
        <ap name="errorType" type="literal">media</ap>
        <ap name="FunctionName" type="literal">HandleError</ap>
      </Properties>
    </node>
    <node type="Label" id="632701531416387400" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1452" y="407">
      <linkto id="632701531416387420" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632701531416387401" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="567" y="143" />
    <node type="Action" id="632701531416387402" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="726" y="142">
      <linkto id="632699824275512001" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632701531416387401" type="Labeled" style="Bezier" ortho="true" label="PlayPrompt" />
      <linkto id="632701531416387435" type="Labeled" style="Bezier" ortho="true" label="TransferUndef" />
      <Properties language="csharp">
public static string Execute(string g_transferNumber, string g_internationalPrefix, string internalUserData, LogWriter log)
{
	// make sure that there is a valid transfer number for us to dial
	if (g_transferNumber.Replace("+", g_internationalPrefix) == string.Empty)
	{
		log.Write(TraceLevel.Info, "OnGatherDigits_Complete: transfer number field is empty, aborting static forward.");

		if (internalUserData == "newCall")
			return "PlayPrompt";
		
		return "TransferUndef";
	}

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Comment" id="632701531416387407" text="All calls failed..." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="599.9414" y="340.1667" />
    <node type="Action" id="632701531416387419" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1712" y="405">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632701531416387420" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1515" y="390" mx="1568" my="406">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632472838633998021" />
        <item text="OnPlay_Failed" treenode="632472838633998026" />
      </items>
      <linkto id="632701531416387419" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632701531416387425" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_confirmReceiptAudioFile</ap>
        <ap name="Prompt3" type="variable">g_confirmReceiptAudioFile</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="TermCondMaxTime" type="csharp">g_timeForDigit * 1000</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_confirmReceiptAudioFile</ap>
        <ap name="UserData" type="variable">userData</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnGatherDigits_Complete: playing digit confirmation announcement.</log>
        <log condition="default" on="true" level="Warning" type="literal">OnGatherDigits_Complete: failed to play request for call receipt confirmation.</log>
      </Properties>
    </node>
    <node type="Action" id="632701531416387421" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1564.94141" y="771.1667">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632701531416387422" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1564.94128" y="645.1667">
      <linkto id="632701531416387421" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632701531416387424" type="Labeled" style="Bevel" ortho="true" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_numFailedDialedCalls</ap>
        <ap name="Value2" type="csharp">g_numbersTable.Rows.Count</ap>
      </Properties>
    </node>
    <node type="Comment" id="632701531416387423" text="All calls failed..." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1629.94141" y="576.1667" />
    <node type="Action" id="632701531416387424" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1654.94141" y="629.1667" mx="1692" my="645">
      <items count="1">
        <item text="HandleError" />
      </items>
      <linkto id="632701531416387421" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="action" type="literal">error_exit</ap>
        <ap name="announceTo" type="literal">default</ap>
        <ap name="error" type="literal">default</ap>
        <ap name="errorType" type="literal">media</ap>
        <ap name="FunctionName" type="literal">HandleError</ap>
      </Properties>
    </node>
    <node type="Action" id="632701531416387425" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1564" y="537">
      <linkto id="632701531416387422" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">g_numFailedDialedCalls + 1</ap>
        <rd field="ResultData">g_numFailedDialedCalls</rd>
      </Properties>
    </node>
    <node type="Label" id="632701531416387435" text="G" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="727" y="48" />
    <node type="Label" id="632701531416387436" text="G" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1054" y="582">
      <linkto id="632699947423161355" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632703305376792802" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1060.62988" y="1079" mx="1098" my="1095">
      <items count="1">
        <item text="DialNumber" />
      </items>
      <linkto id="632673314455190707" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632472838633998214" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Index" type="variable">index</ap>
        <ap name="FunctionName" type="literal">DialNumber</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnGatherDigits_Complete: Dialing transfer number.</log>
      </Properties>
    </node>
    <node type="Variable" id="632472838633998219" name="terminationReason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">terminationReason</Properties>
    </node>
    <node type="Variable" id="632472838633998220" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632484810722400470" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference">connectionId</Properties>
    </node>
    <node type="Variable" id="632673314455190681" name="index" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">index</Properties>
    </node>
    <node type="Variable" id="632673314455190706" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632696551309578538" name="internalUserData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">internalUserData</Properties>
    </node>
    <node type="Variable" id="632696551309578540" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" defaultInitWith="EMPTY" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">digits</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632472838633998184" treenode="632472838633998185" appnode="632472838633998182" handlerfor="632472838633998181">
    <node type="Start" id="632472838633998184" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="376">
      <linkto id="632472838633998221" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632472838633998221" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="260" y="360" mx="320" my="376">
      <items count="1">
        <item text="SendSwapResponse" />
      </items>
      <linkto id="632472838633998222" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="response" type="literal">fail</ap>
        <ap name="FunctionName" type="literal">SendSwapResponse</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998222" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="468" y="376">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSwapRequest" startnode="632472966202366240" treenode="632472966202366241" appnode="632472966202366238" handlerfor="632472966202366237">
    <node type="Start" id="632472966202366240" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="339">
      <linkto id="632472966202366243" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632472966202366242" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="128" y="584">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnSwapRequest: g_allowSwap was false, exiting function</log>
      </Properties>
    </node>
    <node type="Action" id="632472966202366243" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="130" y="342">
      <linkto id="632568047565415699" type="Labeled" style="Bevel" ortho="true" label="true" />
      <linkto id="632735702052551402" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_allowSwap</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnSwapRequest: Function Entry</log>
      </Properties>
    </node>
    <node type="Action" id="632472966202366250" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="828" y="325" mx="881" my="341">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632472838633998021" />
        <item text="OnPlay_Failed" treenode="632472838633998026" />
      </items>
      <linkto id="632472966202366253" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632675094376283224" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">beep.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_callee</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="literal">beep.wav</ap>
        <ap name="UserData" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632472966202366253" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1000" y="341">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632568047565415692" text="If there are no ongoing swap attempts (g_allowSwap == true), &#xD;&#xA;we can then proceed to attempt a new swap,&#xD;&#xA;and lock future concurrent swaps,&#xD;&#xA;'g_allowSwap = false'&#xD;&#xA;&#xD;&#xA;Because its a requirement for good application behavior that all &#xD;&#xA;paths send a SwapResponse to the requesting client, we set&#xD;&#xA;g_allowSwap = true in there, to release the swap lock." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="138" />
    <node type="Action" id="632568047565415695" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="372" y="343">
      <linkto id="632568047565415696" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">userId</ap>
        <ap name="Value2" type="variable">numberToDial</ap>
        <ap name="Value3" type="variable">swapAppRoutingGuid</ap>
        <ap name="Value4" type="literal">true</ap>
        <rd field="ResultData">g_swapUserId</rd>
        <rd field="ResultData2">g_swapNumber</rd>
        <rd field="ResultData3">g_swapRoutingGuid</rd>
        <rd field="ResultData4">g_swapRequestReceived</rd>
        <log condition="entry" on="false" level="Info" type="csharp">"\nOnSwapRequest:\nuserId: " + userId + "\nnumberToDial: " + numberToDial + "\nswapAppRoutingGuid: " + swapAppRoutingGuid + "\nciscoPhoneIp: " + ciscoPhoneIp + "\n";</log>
      </Properties>
    </node>
    <node type="Action" id="632568047565415696" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="491" y="340">
      <linkto id="632696551309577174" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">ciscoPhoneIp</ap>
        <ap name="Value2" type="variable">responseUrl</ap>
        <ap name="Value3" type="literal">false</ap>
        <rd field="ResultData">g_ciscoPhoneIp</rd>
        <rd field="ResultData2">g_responseUrl</rd>
        <rd field="ResultData3">g_callAnswered</rd>
      </Properties>
    </node>
    <node type="Action" id="632568047565415699" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="258" y="342">
      <linkto id="632568047565415695" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">false</ap>
        <rd field="ResultData">g_allowSwap</rd>
      </Properties>
    </node>
    <node type="Comment" id="632673314455189806" text="need provisional failure&#xD;&#xA;error branch here" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="532" y="185" />
    <node type="Action" id="632675094376283224" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="878" y="487">
      <linkto id="632675094376283225" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_allowSwap</rd>
      </Properties>
    </node>
    <node type="Action" id="632675094376283225" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="822" y="573" mx="882" my="589">
      <items count="1">
        <item text="SendSwapResponse" />
      </items>
      <linkto id="632675094376283227" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="response" type="literal">fail</ap>
        <ap name="FunctionName" type="literal">SendSwapResponse</ap>
      </Properties>
    </node>
    <node type="Action" id="632675094376283227" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="878" y="732">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632696551309577174" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="593" y="339">
      <linkto id="632675094376283224" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632700819239849658" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties language="csharp">
public static string Execute(DataTable g_numbersTable, ref string userData, string g_callId_callee)
{
	bool found = false;
	string tempCallId = null;

	for (int i = 0; i &lt; g_numbersTable.Rows.Count; i++)
	{
		try
		{
			tempCallId = g_numbersTable.Rows[i]["call_id"] as string;
		}
		catch
		{
			continue;
		}

		if (tempCallId == null || tempCallId == string.Empty)
			continue;
		
		if (tempCallId == g_callId_callee)
		{
			userData = i.ToString();
			found = true;
			g_numbersTable.Rows[i]["user_data"] = "swapCall";
			break;
		}
	}

	return found ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
}
</Properties>
    </node>
    <node type="Action" id="632700819239849658" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="746.4707" y="340">
      <linkto id="632472966202366250" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId_callee</ap>
        <ap name="Block" type="literal">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632735702052551402" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="129.617188" y="462">
      <linkto id="632472966202366242" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="response" type="literal">locked</ap>
        <ap name="EventName" type="literal">Metreos.Events.ActiveRelay.SwapRequestResponse</ap>
        <ap name="ToGuid" type="variable">swapAppRoutingGuid</ap>
      </Properties>
    </node>
    <node type="Variable" id="632472966202366285" name="userId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="userId" refType="reference">userId</Properties>
    </node>
    <node type="Variable" id="632472966202366286" name="numberToDial" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="directoryNumber" refType="reference">numberToDial</Properties>
    </node>
    <node type="Variable" id="632472966202366287" name="swapAppRoutingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="senderRoutingGuid" refType="reference">swapAppRoutingGuid</Properties>
    </node>
    <node type="Variable" id="632472966202366288" name="ciscoPhoneIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ciscoPhoneIp" refType="reference">ciscoPhoneIp</Properties>
    </node>
    <node type="Variable" id="632472966202366289" name="responseUrl" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="responseUrl" refType="reference">responseUrl</Properties>
    </node>
    <node type="Variable" id="632696551309577175" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotDigits" startnode="632482910352503779" treenode="632482910352503780" appnode="632482910352503777" handlerfor="632482910352503776">
    <node type="Start" id="632482910352503779" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632482910352503796" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632482910352503796" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="171" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632482910352503784" treenode="632482910352503785" appnode="632482910352503782" handlerfor="632482910352503781">
    <node type="Start" id="632482910352503784" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632482910352503797" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632482910352503797" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="158" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632482910352503789" treenode="632482910352503790" appnode="632482910352503787" handlerfor="632482910352503786">
    <node type="Start" id="632482910352503789" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632482910352503798" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632482910352503798" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="216" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632482910352503794" treenode="632482910352503795" appnode="632482910352503792" handlerfor="632482910352503791">
    <node type="Start" id="632482910352503794" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632482910352503799" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632482910352503799" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="213" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HandleError" startnode="632472838633997900" treenode="632472838633997901" appnode="632472838633997898" handlerfor="632482910352503791">
    <node type="Start" id="632472838633997900" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="512">
      <linkto id="632472838633997982" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632472838633997982" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="135" y="514">
      <linkto id="632472838633997983" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632472838633997984" type="Labeled" style="Bevel" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">announceTo != null;</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633997983" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="135" y="635">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633997984" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="363" y="514">
      <linkto id="632472838633997985" type="Labeled" style="Bevel" ortho="true" label="callControl" />
      <linkto id="632472838633997987" type="Labeled" style="Bevel" ortho="true" label="database" />
      <linkto id="632472838633997986" type="Labeled" style="Bevel" ortho="true" label="media" />
      <linkto id="632472838633997988" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">errorType</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633997985" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="569" y="305">
      <linkto id="632472838633997989" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">error</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633997986" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="569" y="516">
      <linkto id="632472838633997989" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">error</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633997987" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="568" y="729">
      <linkto id="632472838633997989" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">error</ap>
      </Properties>
    </node>
    <node type="Label" id="632472838633997988" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="365" y="335" />
    <node type="Action" id="632472838633997989" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="754" y="516">
      <linkto id="632472838633997990" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">ar_error_CantProcessCall.wav</ap>
        <rd field="ResultData">filename1</rd>
      </Properties>
    </node>
    <node type="Action" id="632472838633997990" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="963.084" y="514">
      <linkto id="632472838633997991" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632472838633998036" type="Labeled" style="Bevel" ortho="true" label="caller" />
      <linkto id="632472838633998037" type="Labeled" style="Bevel" ortho="true" label="callee" />
      <linkto id="632472838633998030" type="Labeled" style="Bevel" ortho="true" label="conference" />
      <linkto id="632472838633998040" type="Labeled" style="Bevel" ortho="true" label="both" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">announceTo</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633997991" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="964" y="242">
      <linkto id="632472838633997994" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632472838633997992" type="Labeled" style="Bezier" label="exit" />
      <linkto id="632472838633997992" type="Labeled" style="Bezier" label="error_exit" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">action</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633997992" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="779" y="243">
      <linkto id="632472838633998012" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="variable">endReason</ap>
      </Properties>
    </node>
    <node type="Label" id="632472838633997993" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="753" y="334">
      <linkto id="632472838633997989" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632472838633997994" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="965" y="127">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633997996" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="779" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632472838633998012" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="736" y="112" mx="783" my="128">
      <items count="1">
        <item text="ProcessHangup" />
      </items>
      <linkto id="632472838633997996" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="origin" type="literal">caller</ap>
        <ap name="userData" type="literal">default</ap>
        <ap name="reason" type="variable">endReason</ap>
        <ap name="FunctionName" type="literal">ProcessHangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998030" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1136" y="200" mx="1189" my="216">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632472838633998021" />
        <item text="OnPlay_Failed" treenode="632472838633998026" />
      </items>
      <linkto id="632472838633998046" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">filename2</ap>
        <ap name="Prompt3" type="variable">filename3</ap>
        <ap name="ConferenceId" type="variable">conferenceId</ap>
        <ap name="Prompt1" type="variable">filename1</ap>
        <ap name="UserData" type="literal">action</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998033" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1360" y="488" mx="1413" my="504">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632472838633998021" />
        <item text="OnPlay_Failed" treenode="632472838633998026" />
      </items>
      <linkto id="632472838633998043" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">filename2</ap>
        <ap name="Prompt3" type="variable">filename3</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="Prompt1" type="variable">filename1</ap>
        <ap name="UserData" type="literal">action</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998036" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1192" y="424">
      <linkto id="632472838633998033" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">g_connectionId_caller</ap>
        <rd field="ResultData">connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632472838633998037" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1192" y="576">
      <linkto id="632472838633998033" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">g_connectionId_callee</ap>
        <rd field="ResultData">connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632472838633998040" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1139" y="720" mx="1192" my="736">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632472838633998021" />
        <item text="OnPlay_Failed" treenode="632472838633998026" />
      </items>
      <linkto id="632472838633998037" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">filename2</ap>
        <ap name="Prompt3" type="variable">filename3</ap>
        <ap name="ConnectionId" type="variable">g_connectionId_caller</ap>
        <ap name="Prompt1" type="variable">filename1</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998043" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1536" y="504">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Comment" id="632472838633998044" text="Play to the caller first" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="984" y="728" />
    <node type="Action" id="632472838633998046" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1416" y="216">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Variable" id="632472838633997972" name="announceTo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="announceTo" refType="reference">announceTo</Properties>
    </node>
    <node type="Variable" id="632472838633997973" name="error" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="error" refType="reference">error</Properties>
    </node>
    <node type="Variable" id="632472838633997974" name="errorType" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="errorType" refType="reference">errorType</Properties>
    </node>
    <node type="Variable" id="632472838633997975" name="action" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="action" defaultInitWith="exit" refType="reference">action</Properties>
    </node>
    <node type="Variable" id="632472838633997976" name="filename1" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">filename1</Properties>
    </node>
    <node type="Variable" id="632472838633997977" name="filename2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">filename2</Properties>
    </node>
    <node type="Variable" id="632472838633997978" name="filename3" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">filename3</Properties>
    </node>
    <node type="Variable" id="632472838633997979" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">connectionId</Properties>
    </node>
    <node type="Variable" id="632472838633997980" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632472838633997981" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">conferenceId</Properties>
    </node>
    <node type="Variable" id="632678332830509276" name="endReason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="reason" defaultInitWith="Unknown" refType="reference">endReason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ProcessHangup" startnode="632472838633998015" treenode="632472838633998016" appnode="632472838633998013" handlerfor="632482910352503791">
    <node type="Start" id="632472838633998015" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="47" y="431">
      <linkto id="632472838633998110" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Comment" id="632472838633998104" text="In this function, we perform actions&#xD;&#xA;necessary in order to insure that both&#xD;&#xA;parties are disconnected and that all&#xD;&#xA;the media connections are released." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="55" />
    <node type="Comment" id="632472838633998105" text="Here we check to see if the callee is connected, then we &#xD;&#xA;hang up all outstanding calls, with the exception of the&#xD;&#xA;passed in callId, since that call was already hungup.Next,&#xD;&#xA;we delete the media connection, then we check to see if the&#xD;&#xA;caller is still connected. If so, we move to the section of the&#xD;&#xA;code responsible for hanging up the caller." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="428" y="32" />
    <node type="Comment" id="632472838633998106" text="If the origin parameter&#xD;&#xA;specifies that it is the&#xD;&#xA;caller that we want to hang&#xD;&#xA;up first, we first want to make&#xD;&#xA;sure that the caller is in fact&#xD;&#xA;connected. We do this by&#xD;&#xA;checking that the g_didCallerHangup&#xD;&#xA;boolean is false. If it is, we hangup the caller&#xD;&#xA;and then delete his media connection.&#xD;&#xA;If it is not, we simply&#xD;&#xA;attempt to delete the media connection&#xD;&#xA;associated with the caller. " class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="777" y="476" />
    <node type="Action" id="632472838633998110" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="263" y="431">
      <linkto id="632472838633998112" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632482910352503771" type="Labeled" style="Bevel" ortho="true" label="callee" />
      <linkto id="632472838633998116" type="Labeled" style="Bevel" ortho="true" label="caller" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">origin</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998112" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="263" y="631">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998116" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="467" y="171">
      <linkto id="632668954769677294" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_isCalleeConnected</rd>
      </Properties>
    </node>
    <node type="Action" id="632472838633998127" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="700" y="432">
      <linkto id="632472838633998128" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId_caller</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">ProcessHangup: Hanging up caller.</log>
      </Properties>
    </node>
    <node type="Action" id="632472838633998128" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="665" y="294" mx="702" my="310">
      <items count="1">
        <item text="HangUpList" />
      </items>
      <linkto id="632678332830509274" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="exception" type="variable">g_callId_callee</ap>
        <ap name="userData" type="literal">default</ap>
        <ap name="FunctionName" type="literal">HangUpList</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998136" name="RemoveActiveRelayRecord" class="MaxActionNode" group="" path="Metreos.Native.ActiveRelay" x="980" y="286">
      <linkto id="632472838633998138" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998137" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1251" y="168">
      <linkto id="632472838633998139" type="Labeled" style="Bevel" ortho="true" label="true" />
      <linkto id="632736982806206996" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_swapPerformed</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998138" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1143" y="167">
      <linkto id="632472838633998137" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="variable">endReason</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998139" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1331" y="74">
      <linkto id="632736982806206996" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_swapCallRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.Normal</ap>
      </Properties>
    </node>
    <node type="Action" id="632482910352503771" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="454" y="432">
      <linkto id="632472838633998128" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632482910352503772" type="Labeled" style="Bevel" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_didCallerHangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632482910352503772" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="590" y="432">
      <linkto id="632472838633998127" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_didCallerHangup</rd>
      </Properties>
    </node>
    <node type="Action" id="632668954769677294" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="658.8258" y="155" mx="696" my="171">
      <items count="1">
        <item text="HangUpList" />
      </items>
      <linkto id="632678332830509274" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="userData" type="literal">all_calls</ap>
        <ap name="FunctionName" type="literal">HangUpList</ap>
      </Properties>
    </node>
    <node type="Action" id="632678332830509274" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="829" y="171">
      <linkto id="632472838633998136" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632472838633998138" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_ARRecordExists</ap>
      </Properties>
    </node>
    <node type="Action" id="632736982806206996" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1380.28455" y="151" mx="1417" my="167">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632736982806207007" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632736982806207007" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1517.627" y="167">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632472838633998133" name="origin" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="origin" refType="reference">origin</Properties>
    </node>
    <node type="Variable" id="632472838633998134" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="userData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632678332830509279" name="endReason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="reason" defaultInitWith="Normal" refType="reference">endReason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HangUpList" startnode="632472838633998131" treenode="632472838633998132" appnode="632472838633998129" handlerfor="632482910352503791">
    <node type="Loop" id="632472838633998272" name="Loop" text="loop (expr)" cx="602" cy="424" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="214" y="77" mx="515" my="289">
      <linkto id="632664669790126399" fromport="1" type="Basic" style="Bevel" ortho="true" />
      <linkto id="632472838633998263" fromport="3" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties iteratorType="int" type="csharp">g_numbersTable.Rows.Count</Properties>
    </node>
    <node type="Start" id="632472838633998131" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="288">
      <linkto id="632472838633998249" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Comment" id="632472838633998237" text="We check to see if the&#xD;&#xA;list of calls to hang up&#xD;&#xA;is empty. If it is, we have&#xD;&#xA;nothing to do, so we exit&#xD;&#xA;the function." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="142" />
    <node type="Comment" id="632472838633998239" text="Otherwise,we compare the&#xD;&#xA;currently enumerated element&#xD;&#xA;to the passed in &quot;exception&quot;&#xD;&#xA;If they are equal, we move on to&#xD;&#xA;the next iteration of the loop.&#xD;&#xA;Otherwise, we attempt to hang up&#xD;&#xA;the current element. " class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="470" y="512" />
    <node type="Comment" id="632472838633998242" text="We remove all the hung-up&#xD;&#xA;calls from the passed-in list&#xD;&#xA;of calls to hangup" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="900.4707" y="152" />
    <node type="Action" id="632472838633998249" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="112" y="288">
      <linkto id="632472838633998250" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632472838633998272" port="1" type="Labeled" style="Bevel" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_numbersTable.Rows.Count &gt; 0;</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">HangUpList: Hanging up outstanding calls.</log>
        <log condition="default" on="true" level="Verbose" type="literal">HangUpList: No calls to hang up</log>
      </Properties>
    </node>
    <node type="Action" id="632472838633998250" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="112" y="424">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
        <log condition="entry" on="true" level="Info" type="literal">HangUpList: List was empty, exiting...</log>
      </Properties>
    </node>
    <node type="Action" id="632472838633998263" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="981" y="288">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998292" name="Switch" container="632472838633998272" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="369.999969" y="290">
      <linkto id="632472838633998294" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632664669790126397" type="Labeled" style="Bevel" ortho="true" label="all_calls" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998294" name="Compare" container="632472838633998272" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="545" y="436">
      <linkto id="632472838633998272" port="3" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632664669790126397" type="Labeled" style="Bevel" ortho="true" label="unequal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">exception</ap>
        <ap name="Value2" type="variable">callId</ap>
        <log condition="default" on="true" level="Verbose" type="csharp">"HangUpList: Omitting hangup of call with callId: " + callId</log>
      </Properties>
    </node>
    <node type="Action" id="632472838633998298" name="Write" container="632472838633998272" class="MaxActionNode" group="" path="Metreos.Native.Log" x="666" y="194">
      <linkto id="632472838633998272" port="3" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"HangUpList: Failure occured attempting to hang up call with callId: " + callId</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">ActiveRelay</ap>
      </Properties>
    </node>
    <node type="Comment" id="632472838633998301" text="We add all&#xD;&#xA;the hung up&#xD;&#xA;calls to a list." container="632472838633998272" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="717" y="103" />
    <node type="Comment" id="632472838633998302" text="If the Hangup action failed, we print&#xD;&#xA;and error message." container="632472838633998272" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="382" y="109" />
    <node type="Comment" id="632472838633998305" text="We switch on userData for the&#xD;&#xA;purpose of determining behavior.&#xD;&#xA;If userData is set to &quot;all_calls&quot; we&#xD;&#xA;try to hangup all calls. Otherwise,&#xD;&#xA;we compare the currently enumerated&#xD;&#xA;element to the passed in &quot;exception&quot;" container="632472838633998272" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="246" y="165" />
    <node type="Action" id="632664669790126397" name="CallFunction" container="632472838633998272" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="485.217773" y="275" mx="547" my="291">
      <items count="1">
        <item text="HangupOutboundCall" />
      </items>
      <linkto id="632472838633998298" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632472838633998272" port="3" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Index" type="csharp">loopIndex</ap>
        <ap name="FunctionName" type="literal">HangupOutboundCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632664669790126399" name="CustomCode" container="632472838633998272" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="284" y="290">
      <linkto id="632472838633998292" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632664669790126400" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string callId, int loopIndex, DataTable g_numbersTable, LogWriter log)
{
	callId = string.Empty;
	DataRow row;
	try
	{
		row = g_numbersTable.Rows[loopIndex];
	}
	catch
	{
		log.Write(TraceLevel.Warning, "HangUpList: failed to retrieve information for index: " + loopIndex);
		return IApp.VALUE_FAILURE;
	}

	CallState state = (CallState) row["call_state"];
	callId = row["call_id"] as string;

	switch (state)
	{
		case CallState.DIAL_PENDING :
			return IApp.VALUE_SUCCESS;
		case CallState.RING_OUT :
		case CallState.CONNECTED :
		{
			if (callId == null || callId == string.Empty)
			{
				callId = string.Empty;
				log.Write(TraceLevel.Info, "HangUpList: could not retrieve callId");
				return IApp.VALUE_FAILURE;
			}
			else
			{
				log.Write(TraceLevel.Verbose, "HangUpList: hanging up call with callId: " + callId);
				return IApp.VALUE_SUCCESS;
			}
		}
		default : 
		{
			// mark call state as ENDED, so that any queued timers ignore this
			// record. 
			row["call_state"] = CallState.ENDED;
			log.Write(TraceLevel.Verbose, "HangUpList: Call with callId '" + callId + "' not in proper state to hang up.");
			return "NoHangup";
		}
	}
}</Properties>
    </node>
    <node type="Label" id="632664669790126400" text="E" container="632472838633998272" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="285" y="378" />
    <node type="Label" id="632664669790126401" text="E" container="632472838633998272" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="765" y="387">
      <linkto id="632472838633998272" port="3" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Variable" id="632472838633998269" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="userData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632664669790126394" name="exception" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="exception" defaultInitWith="NONE" refType="reference">exception</Properties>
    </node>
    <node type="Variable" id="632664669790126398" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="SendSwapResponse" startnode="632472838633998171" treenode="632472838633998172" appnode="632472838633998169" handlerfor="632482910352503791">
    <node type="Start" id="632472838633998171" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="385">
      <linkto id="632568047565415690" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632472838633998223" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="336" y="384">
      <linkto id="632472838633998224" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="response" type="variable">response</ap>
        <ap name="EventName" type="literal">Metreos.Events.ActiveRelay.SwapRequestResponse</ap>
        <ap name="ToGuid" type="variable">g_swapRoutingGuid</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998224" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="464" y="384">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998225" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="240" y="384">
      <linkto id="632472838633998223" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632472838633998228" type="Labeled" style="Bezier" label="fail" />
      <linkto id="632472838633998228" type="Labeled" style="Bezier" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">response</ap>
      </Properties>
    </node>
    <node type="Action" id="632472838633998226" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="240" y="128">
      <linkto id="632472838633998227" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="variable">g_responseUrl</ap>
        <ap name="Priority1" type="literal">0</ap>
        <rd field="ResultData">ciscoExecute</rd>
      </Properties>
    </node>
    <node type="Action" id="632472838633998227" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="464" y="128">
      <linkto id="632472838633998224" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">ciscoExecute.ToString()</ap>
        <ap name="URL" type="csharp">g_ciscoPhoneIp + "/CGI/Execute";</ap>
        <ap name="Username" type="variable">ccm_Username</ap>
        <ap name="Password" type="variable">ccm_Password</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"SendSwapResponse: SendingExecute to IP: " + g_ciscoPhoneIp;</log>
      </Properties>
    </node>
    <node type="Action" id="632472838633998228" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="240" y="248">
      <linkto id="632472838633998226" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">g_responseUrl + "?result=" + response + "&amp;metreosSessionId=" + g_swapRoutingGuid</ap>
        <rd field="ResultData">g_responseUrl</rd>
      </Properties>
    </node>
    <node type="Action" id="632568047565415690" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="131" y="385">
      <linkto id="632472838633998225" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <rd field="ResultData">g_allowSwap</rd>
      </Properties>
    </node>
    <node type="Comment" id="632568047565415691" text="Allow swaps again" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="85" y="329" />
    <node type="Variable" id="632472838633998235" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="response" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632472838633998236" name="ciscoExecute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">ciscoExecute</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Answer" startnode="632484810722400402" treenode="632488360389763888" appnode="632488360389763885" handlerfor="632482910352503791">
    <node type="Start" id="632484810722400402" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="336">
      <linkto id="632484810722400948" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632484810722400410" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="478" y="336">
      <linkto id="632484810722400425" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632484810722400428" type="Labeled" style="Bevel" ortho="true" label="newCall" />
      <linkto id="632484810722400418" type="Labeled" style="Bevel" ortho="true" label="swapCall" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"MakeCallComplete: UserData is "  + userData</log>
      </Properties>
    </node>
    <node type="Comment" id="632484810722400411" text="We switch on the passed-in&#xD;&#xA;userData, which tells us&#xD;&#xA;whether we're dealing with &#xD;&#xA;establishing a new call, or if&#xD;&#xA;we're dealing with a swap&#xD;&#xA;request" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="352" y="400" />
    <node type="Action" id="632484810722400412" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="344" y="336">
      <linkto id="632484810722400410" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="literal">true</ap>
        <ap name="Value3" type="variable">calleeCallId</ap>
        <ap name="Value4" type="variable">connectionId</ap>
        <rd field="ResultData">g_callAnswered</rd>
        <rd field="ResultData2">g_isCalleeConnected</rd>
        <rd field="ResultData3">g_callId_callee</rd>
        <rd field="ResultData4">g_connectionId_callee</rd>
      </Properties>
    </node>
    <node type="Action" id="632484810722400413" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="224" y="336">
      <linkto id="632484810722400414" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632484810722400412" type="Labeled" style="Bevel" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_callAnswered</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Complete: Function entry</log>
      </Properties>
    </node>
    <node type="Action" id="632484810722400414" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="224" y="536">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Comment" id="632484810722400415" text="If the call&#xD;&#xA;was already&#xD;&#xA;answered,&#xD;&#xA;we exit. Else&#xD;&#xA;we proceed." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="176" y="232" />
    <node type="Comment" id="632484810722400416" text="We complete the&#xD;&#xA;media connection&#xD;&#xA;for the callee." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="136" y="120" />
    <node type="Action" id="632484810722400418" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="477" y="164">
      <linkto id="632484810722400420" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">success</ap>
        <ap name="Value2" type="literal">true</ap>
        <rd field="ResultData">swapResult</rd>
        <rd field="ResultData2">g_swapPerformed</rd>
      </Properties>
    </node>
    <node type="Action" id="632484810722400420" name="UpdateActiveRelayInfo" class="MaxActionNode" group="" path="Metreos.Native.ActiveRelay" x="582" y="164">
      <linkto id="632484810722400422" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_swapUserId</ap>
        <ap name="AppRoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="FromNumber" type="variable">g_from</ap>
        <ap name="ToNumber" type="variable">g_to</ap>
        <ap name="WasSwapped" type="literal">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632484810722400422" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="732" y="164">
      <linkto id="632674146742757027" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="OriginNumber" type="variable">g_from</ap>
        <ap name="DestinationNumber" type="variable">g_to</ap>
        <ap name="UserId" type="variable">g_userId</ap>
        <rd field="CallRecordsId">g_swapCallRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632484810722400424" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1014" y="152" mx="1074" my="168">
      <items count="1">
        <item text="SendSwapResponse" />
      </items>
      <linkto id="632484810722400427" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="response" type="literal">swapResult</ap>
        <ap name="FunctionName" type="literal">SendSwapResponse</ap>
      </Properties>
    </node>
    <node type="Action" id="632484810722400425" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="911" y="320" mx="948" my="336">
      <items count="1">
        <item text="HangUpList" />
      </items>
      <linkto id="632484810722400427" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="exception" type="variable">calleeCallId</ap>
        <ap name="userData" type="literal">none</ap>
        <ap name="FunctionName" type="literal">HangUpList</ap>
      </Properties>
    </node>
    <node type="Action" id="632484810722400426" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="849" y="752" mx="886" my="768">
      <items count="1">
        <item text="HandleError" />
      </items>
      <linkto id="632484810722400431" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="action" type="literal">error_exit</ap>
        <ap name="announceTo" type="literal">callee</ap>
        <ap name="error" type="literal">default</ap>
        <ap name="errorType" type="literal">callControl</ap>
        <ap name="FunctionName" type="literal">HandleError</ap>
      </Properties>
    </node>
    <node type="Action" id="632484810722400427" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1072" y="336">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632484810722400428" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="884" y="476">
      <linkto id="632491005669511440" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632697370120313539" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId_caller</ap>
        <ap name="MmsId" type="variable">mmsId</ap>
        <ap name="ProxyDTMFCallId" type="variable">calleeCallId</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">conferenceId</ap>
        <rd field="ConnectionId">g_connectionId_caller</rd>
        <rd field="ConferenceId">g_conferenceId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"Answering call with ID " + g_callId_caller + " mmsId: " + mmsId</log>
        <log condition="success" on="true" level="Info" type="literal">Call answered successfully</log>
        <log condition="default" on="true" level="Info" type="literal">Failed to answer call</log>
      </Properties>
    </node>
    <node type="Comment" id="632484810722400429" text="If answering the call fails, process the&#xD;&#xA;error and exit." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="992" y="640" />
    <node type="Comment" id="632484810722400430" text="If this is a new call that we're&#xD;&#xA;dealing with, we need to pick&#xD;&#xA;up the initiating call." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="677" y="512" />
    <node type="Action" id="632484810722400431" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1012" y="768">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
      </Properties>
    </node>
    <node type="Action" id="632484810722400432" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1193" y="477">
      <linkto id="632484810722400433" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="literal">true</ap>
        <ap name="Value3" type="literal">0</ap>
        <ap name="Value4" type="literal">true</ap>
        <rd field="ResultData">g_didCallerHangup</rd>
        <rd field="ResultData2">g_conferenceCreated</rd>
        <rd field="ResultData3">g_numFailedDialedCalls</rd>
        <rd field="ResultData4">g_allowSwap</rd>
      </Properties>
    </node>
    <node type="Action" id="632484810722400433" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1310" y="477">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Comment" id="632484810722400435" text="We add the callee into the conference" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1318" y="556" />
    <node type="Action" id="632484810722400948" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="120" y="336">
      <linkto id="632484810722400413" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Hashtable g_makeCallState, ref string calleeCallId, ref string calleeMediaIp, ref ushort calleeMediaPort, ref string userData, ref string swapResult, ref string conferenceId, string connectionId, ref string mmsId)
{
	object[] state = (object[]) g_makeCallState[connectionId];

	calleeCallId = state[0] as string;
	calleeMediaIp = state[1] as string;
	calleeMediaPort = (ushort) state[2];
	userData = state[3] as string;
	swapResult = state[4] as string;
	conferenceId = state[5] as string;
	mmsId = state[6] as string;

	return String.Empty; 
}
</Properties>
    </node>
    <node type="Action" id="632491005669511438" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1054" y="461" mx="1091" my="477">
      <items count="1">
        <item text="HangUpList" />
      </items>
      <linkto id="632484810722400432" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="exception" type="variable">calleeCallId</ap>
        <ap name="userData" type="literal">none</ap>
        <ap name="FunctionName" type="literal">HangUpList</ap>
      </Properties>
    </node>
    <node type="Action" id="632491005669511440" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="850" y="567" mx="887" my="583">
      <items count="1">
        <item text="HangUpList" />
      </items>
      <linkto id="632484810722400426" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="exception" type="variable">calleeCallId</ap>
        <ap name="userData" type="literal">none</ap>
        <ap name="FunctionName" type="literal">HangUpList</ap>
      </Properties>
    </node>
    <node type="Action" id="632674146742757026" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="950" y="166">
      <linkto id="632484810722400424" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="Hairpin" type="variable">g_hairpinMedia</ap>
        <rd field="ConnectionId">g_connectionId_callee</rd>
      </Properties>
    </node>
    <node type="Action" id="632674146742757027" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="804" y="149" mx="841" my="165">
      <items count="1">
        <item text="HangUpList" />
      </items>
      <linkto id="632674146742757026" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="exception" type="variable">calleeCallId</ap>
        <ap name="userData" type="literal">none</ap>
        <ap name="FunctionName" type="literal">HangUpList</ap>
      </Properties>
    </node>
    <node type="Action" id="632697370120313539" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="990" y="477">
      <linkto id="632491005669511438" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">mmsId</ap>
        <rd field="ResultData">g_mmsId</rd>
      </Properties>
    </node>
    <node type="Variable" id="632484810722400949" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="connectionId" refType="reference">connectionId</Properties>
    </node>
    <node type="Variable" id="632484810722400950" name="calleeCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">calleeCallId</Properties>
    </node>
    <node type="Variable" id="632484810722400951" name="calleeMediaIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">calleeMediaIp</Properties>
    </node>
    <node type="Variable" id="632484810722400952" name="calleeMediaPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UShort" refType="reference">calleeMediaPort</Properties>
    </node>
    <node type="Variable" id="632484810722400953" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632484810722400954" name="swapResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">swapResult</Properties>
    </node>
    <node type="Variable" id="632484810722400955" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">conferenceId</Properties>
    </node>
    <node type="Variable" id="632697370120313536" name="mmsId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">mmsId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OpenDBConnection" startnode="632658601492437816" treenode="632658601492437817" appnode="632658601492437814" handlerfor="632482910352503791">
    <node type="Start" id="632658601492437816" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="91" y="143">
      <linkto id="632658601492437819" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632658601492437818" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="406.188782" y="141.5">
      <linkto id="632659276838564880" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632659276838564883" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="variable">db_ConnectionName</ap>
        <ap name="Type" type="literal">mysql</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OpenDBConnection: attempting connect to Primary</log>
        <log condition="default" on="true" level="Warning" type="literal">OpenDBConnection: Connection to Primary failed.</log>
      </Properties>
    </node>
    <node type="Action" id="632658601492437819" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="234.189087" y="142.5">
      <linkto id="632658601492437818" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632659276838564883" type="Labeled" style="Bevel" ortho="true" label="default" />
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
    <node type="Action" id="632658601492437823" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="695.188843" y="419.5">
      <linkto id="632659276838564880" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632659276838564884" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="csharp">dsn + "; connection timeout=1;"</ap>
        <ap name="Name" type="variable">db_ConnectionName</ap>
        <ap name="Type" type="literal">mysql</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OpenDBConnection: attempting connect to Secondary</log>
        <log condition="default" on="true" level="Info" type="literal">OpenDBConnection: Connection to Secondary failed.
</log>
      </Properties>
    </node>
    <node type="Action" id="632658601492437824" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="541.1891" y="420.5">
      <linkto id="632658601492437823" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632659276838564884" type="Labeled" style="Bevel" ortho="true" label="default" />
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
    <node type="Action" id="632659276838564880" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="689" y="141">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OpenDBConnection: connection to database established.</log>
      </Properties>
    </node>
    <node type="Action" id="632659276838564883" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="409" y="318">
      <linkto id="632667132589786870" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_DbWriteEnabled</rd>
      </Properties>
    </node>
    <node type="Action" id="632659276838564884" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="695" y="619">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
        <log condition="entry" on="true" level="Error" type="literal">OpenDBConnection: AppSuite DB connections failed. Check application settings.</log>
      </Properties>
    </node>
    <node type="Action" id="632667132589786870" name="SetSessionData" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="541" y="318">
      <linkto id="632658601492437824" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="AllowDBWrite" type="csharp">false</ap>
      </Properties>
    </node>
    <node type="Variable" id="632658601492437822" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="GetExternalNumbersInfo" startnode="632659276838564900" treenode="632659276838564901" appnode="632659276838564898" handlerfor="632482910352503791">
    <node type="Start" id="632659276838564900" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="90" y="320">
      <linkto id="632659276838564904" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632659276838564904" name="GetUserByPrimaryDN" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="222.847015" y="320.168274">
      <linkto id="632660197747416292" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632735813309230653" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="DirectoryNumber" type="variable">g_to</ap>
        <rd field="UserId">g_userId</rd>
        <rd field="UserStatus">userStatus</rd>
        <log condition="success" on="true" level="Verbose" type="csharp">"GetExternalNumbersInfo: DN " + g_to + " matched UID " + g_userId</log>
        <log condition="default" on="true" level="Info" type="csharp">"GetExternalNumbersInfo: Failed to match DN " + g_to + " to a UID"</log>
      </Properties>
    </node>
    <node type="Action" id="632659276838564912" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="455.847" y="320.168274">
      <linkto id="632660197747416279" type="Labeled" style="Bevel" ortho="true" label="equal" />
      <linkto id="632660197747416292" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="literal">Ok</ap>
        <ap name="Value2" type="variable">userStatus</ap>
        <log condition="default" on="true" level="Info" type="csharp">String.Format("GetExternalNumbersInfo: User with id '{0}' and primary line '{1}' is not active. Status is '{2}'", g_userId, g_to, userStatus)
</log>
      </Properties>
    </node>
    <node type="Action" id="632660197747416279" name="GetActiveRelayNumbers" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="620" y="320">
      <linkto id="632660197747416292" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="632660197747416295" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
        <ap name="ExcludeBlacklisted" type="literal">true</ap>
        <rd field="NumberTable">g_numbersTable</rd>
        <rd field="TransferNumber">g_transferNumber</rd>
        <log condition="default" on="true" level="Info" type="csharp">"GetExternalNumbersInfo: No numbers associated with userId: " + g_userId</log>
      </Properties>
    </node>
    <node type="Action" id="632660197747416292" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="403.020264" y="506">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
      </Properties>
    </node>
    <node type="Action" id="632660197747416295" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="781.020264" y="320">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632735813309230653" name="IsSessionAllowed" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="354" y="321">
      <linkto id="632659276838564912" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632660197747416292" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
        <log condition="default" on="true" level="Info" type="csharp">"GetExternalNumbers: The maximum number of sessions has been reached for user with userId: " + g_userId</log>
      </Properties>
    </node>
    <node type="Variable" id="632659276838564925" name="userStatus" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">userStatus</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ProcessDialTable" startnode="632660197747417587" treenode="632660197747417588" appnode="632660197747417585" handlerfor="632482910352503791">
    <node type="Loop" id="632660197747417592" name="Loop" text="loop (expr)" cx="658.4707" cy="507" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="276" y="103" mx="605" my="356">
      <linkto id="632660197747417593" fromport="1" type="Basic" style="Bevel" ortho="true" />
      <linkto id="632666540064688480" fromport="3" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties iteratorType="int" type="csharp">g_numbersTable.Rows.Count</Properties>
    </node>
    <node type="Start" id="632660197747417587" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="55" y="357">
      <linkto id="632660197747417597" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632660197747417593" name="CustomCode" container="632660197747417592" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="416" y="287">
      <linkto id="632660197747417594" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632663009636381105" type="Labeled" style="Bevel" ortho="true" label="DialNow" />
      <linkto id="632664669790126402" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(int loopIndex, DataTable g_numbersTable, ref uint delayCallTime, ref int g_numFailedDialedCalls, LogWriter log)
{
	DataRow row = null; 
	try
	{
		row = g_numbersTable.Rows[loopIndex];
	}
	catch
	{
		log.Write(TraceLevel.Info, "ProcessDialTable: failed to retrieve info for loop index: " + loopIndex);
		g_numFailedDialedCalls++;
		return IApp.VALUE_FAILURE;
	}

	delayCallTime = Convert.ToUInt32(row[SqlConstants.Tables.ExternalNumbers.DelayCallTime]);

	row["call_id"] = string.Empty;
	row["timer_id"] = string.Empty;

	if (delayCallTime == 0)
	{
		log.Write(TraceLevel.Verbose, "ProcessDialTable: no delay defined, invoking DialNumber function");
		return "DialNow";
	}
	else
		return IApp.VALUE_SUCCESS;		
}</Properties>
    </node>
    <node type="Action" id="632660197747417594" name="AddNonTriggerTimer" container="632660197747417592" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="509" y="148" mx="575" my="164">
      <items count="1">
        <item text="OnTimerFire" treenode="632472838633997878" />
      </items>
      <linkto id="632660197747417610" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632663009636381105" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerUserData" type="csharp">loopIndex</ap>
        <ap name="timerDateTime" type="csharp">currentTime.AddSeconds(delayCallTime)</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="timerId">timerId</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"ProcessDialTable: adding dial timer to fire in " + delayCallTime + " seconds"</log>
        <log condition="default" on="true" level="Warning" type="literal">ProcessDialTable: could not set call delay timer. Dialing entry now.</log>
      </Properties>
    </node>
    <node type="Action" id="632660197747417597" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="154" y="357">
      <linkto id="632660197747417592" port="1" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">System.DateTime.Now</ap>
        <rd field="ResultData">currentTime</rd>
      </Properties>
    </node>
    <node type="Action" id="632660197747417610" name="CustomCode" container="632660197747417592" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="801" y="164">
      <linkto id="632660197747417592" port="3" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(DataTable g_numbersTable, string timerId, int loopIndex)
{
	g_numbersTable.Rows[loopIndex]["timer_id"] = timerId;
	g_numbersTable.Rows[loopIndex]["call_state"] = CallState.DIAL_PENDING;

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632663009636381105" name="CallFunction" container="632660197747417592" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="533.825867" y="343" mx="571" my="359">
      <items count="1">
        <item text="DialNumber" />
      </items>
      <linkto id="632660197747417592" port="3" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Index" type="csharp">loopIndex</ap>
        <ap name="FunctionName" type="literal">DialNumber</ap>
      </Properties>
    </node>
    <node type="Action" id="632663009636381110" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1177" y="356">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Label" id="632664669790126402" text="E" container="632660197747417592" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="417" y="384" />
    <node type="Label" id="632664669790126403" text="E" container="632660197747417592" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="798" y="498">
      <linkto id="632660197747417592" port="3" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632666540064688479" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1054.4707" y="493">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
      </Properties>
    </node>
    <node type="Action" id="632666540064688480" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1053.4707" y="357">
      <linkto id="632666540064688479" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632663009636381110" type="Labeled" style="Bevel" ortho="true" label="unequal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_numFailedDialedCalls</ap>
        <ap name="Value2" type="csharp">g_numbersTable.Rows.Count</ap>
      </Properties>
    </node>
    <node type="Variable" id="632660197747417589" name="currentTime" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DateTime" refType="reference">currentTime</Properties>
    </node>
    <node type="Variable" id="632660197747417590" name="delayCallTime" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">delayCallTime</Properties>
    </node>
    <node type="Variable" id="632660197747417609" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">timerId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="DialNumber" startnode="632663009636381108" treenode="632663009636381109" appnode="632663009636381106" handlerfor="632482910352503791">
    <node type="Start" id="632663009636381108" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="399">
      <linkto id="632663646668754480" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632663009636381122" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="261" y="383" mx="327" my="399">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632472838633998064" />
        <item text="OnMakeCall_Failed" treenode="632472838633998069" />
        <item text="OnRemoteHangup" treenode="632472838633998074" />
      </items>
      <linkto id="632663646668754485" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632800221463702409" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">numberToDial</ap>
        <ap name="From" type="variable">g_translatedFrom</ap>
        <ap name="DisplayName" type="variable">g_displayName</ap>
        <ap name="MmsId" type="variable">g_mmsId</ap>
        <ap name="Conference" type="variable">shouldConference</ap>
        <ap name="ConferenceId" type="variable">conferenceId</ap>
        <ap name="Hairpin" type="variable">g_hairpinMedia</ap>
        <ap name="UserData" type="variable">index</ap>
        <rd field="CallId">callId</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"DialNumber: Dialing number '" + numberToDial + "'"</log>
        <log condition="default" on="true" level="Info" type="literal">DialNumber: MakeCall action failed provisionally.</log>
      </Properties>
    </node>
    <node type="Action" id="632663009636381127" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="863" y="397">
      <linkto id="632663009636381128" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632663646668754489" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(string callId, string timerId, int index, DataTable g_numbersTable, int g_numDialedCalls, LogWriter log, Hashtable g_callIdToTableIndexMap)
{
	g_numDialedCalls++;

	// unfortunately we need to keep a mapping of callIds to g_numbersTable indeces
	// because of unsolicited events. Since the event is unsolicited, there will be
	// no UserData for us to pull the index out of, necessitating a search. 
	// keeping a hash is the quick, dirty approach.
	g_callIdToTableIndexMap[callId] = index;
	
	try
	{
		g_numbersTable.Rows[index]["call_id"] = callId;
		if (timerId != null &amp;&amp; timerId != string.Empty)
			g_numbersTable.Rows[index]["timer_id"] = timerId;

		g_numbersTable.Rows[index]["call_state"] = CallState.RING_OUT;
		return IApp.VALUE_SUCCESS;
	}
	catch
	{
		log.Write(TraceLevel.Warning, "DialNumber: failed to set information for index " + index);
		return IApp.VALUE_FAILURE;
	}
}</Properties>
    </node>
    <node type="Action" id="632663009636381128" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1017" y="397">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632663646668754478" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="324.90625" y="649">
      <linkto id="632663646668754488" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">DialList: MakeCall failed...</log>
	public static string Execute(ref int g_numFailedDialedCalls, ref int g_numDialedCalls, DataTable g_numbersTable, int index)
	{
		g_numFailedDialedCalls++;
		g_numDialedCalls++;
		
		try
		{
			g_numbersTable.Rows[index]["call_state"] = CallState.ENDED; 	
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}

		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632663646668754480" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="164" y="399">
      <linkto id="632663009636381122" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632670653296732620" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(int index, DataTable g_numbersTable, ref string numberToDial, ref uint callAttemptTimeout, string g_dialPrefix, LogWriter log, ref int g_numFailedDialedCalls, string g_internationalPrefix, ref string conferenceId, string g_conferenceId, ref bool shouldConference)
{
	CallState status = (CallState) g_numbersTable.Rows[index]["call_state"];
	
	switch (status)
	{
		case CallState.RING_OUT :
		case CallState.CONNECTED : 
		{
			log.Write(TraceLevel.Warning, "DialNumber: DialNumber was issued on a call that's already outbound/connected.");
			return IApp.VALUE_FAILURE;
		}
	}

	string internalUserData = g_numbersTable.Rows[index]["user_data"] as string;
	if (internalUserData == "transfer")
		conferenceId = g_conferenceId;
	else if (internalUserData == "swapCall")
		shouldConference = false;

	numberToDial = g_numbersTable.Rows[index][SqlConstants.Tables.ExternalNumbers.PhoneNumber] as string;
	if (numberToDial == null || numberToDial == string.Empty)
	{
		log.Write(TraceLevel.Warning, "DialNumber: number-to-dial is empty!");
		g_numFailedDialedCalls++;
		g_numbersTable.Rows[index]["call_state"] = CallState.ENDED;
		return IApp.VALUE_FAILURE;
	}

	numberToDial = numberToDial.Replace("+", g_internationalPrefix);

	if (internalUserData != "transfer")
		numberToDial = g_dialPrefix + numberToDial;
	
	callAttemptTimeout = (uint) g_numbersTable.Rows[index][SqlConstants.Tables.ExternalNumbers.CallAttemptTimeout];

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632663646668754482" name="AddNonTriggerTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="653" y="382" mx="719" my="398">
      <items count="1">
        <item text="OnTimerFire" treenode="632472838633997878" />
      </items>
      <linkto id="632663009636381127" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerUserData" type="csharp">index.ToString()</ap>
        <ap name="timerDateTime" type="csharp">DateTime.Now.AddSeconds(callAttemptTimeout)</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="timerId">timerId</rd>
      </Properties>
    </node>
    <node type="Action" id="632663646668754485" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="561" y="398">
      <linkto id="632663646668754482" type="Labeled" style="Bevel" ortho="true" label="true" />
      <linkto id="632663646668754486" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">callAttemptTimeout &gt; 0</ap>
      </Properties>
    </node>
    <node type="Label" id="632663646668754486" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="561" y="509.999969" />
    <node type="Label" id="632663646668754487" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="862" y="273">
      <linkto id="632663009636381127" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632663646668754488" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="325" y="756">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
      </Properties>
    </node>
    <node type="Action" id="632663646668754489" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="864" y="551">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
      </Properties>
    </node>
    <node type="Action" id="632670653296732620" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="164" y="556">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
      </Properties>
    </node>
    <node type="Action" id="632800221463702409" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="270" y="524" mx="327" my="540">
      <items count="1">
        <item text="WriteFindMeRecord" />
      </items>
      <linkto id="632663646668754478" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Index" type="variable">index</ap>
        <ap name="EndReason" type="literal">Unknown</ap>
        <ap name="FunctionName" type="literal">WriteFindMeRecord</ap>
      </Properties>
    </node>
    <node type="Variable" id="632663009636381111" name="index" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="Index" refType="reference">index</Properties>
    </node>
    <node type="Variable" id="632663009636381112" name="numberToDial" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" refType="reference">numberToDial</Properties>
    </node>
    <node type="Variable" id="632663009636381126" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632663646668754476" name="callAttemptTimeout" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" initWith="" refType="reference">callAttemptTimeout</Properties>
    </node>
    <node type="Variable" id="632663646668754481" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">timerId</Properties>
    </node>
    <node type="Variable" id="632701531416387438" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" defaultInitWith="0" refType="reference">conferenceId</Properties>
    </node>
    <node type="Variable" id="632703532838706722" name="shouldConference" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" defaultInitWith="true" refType="reference">shouldConference</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HangupOutboundCall" startnode="632663009636381117" treenode="632663009636381118" appnode="632663009636381115" handlerfor="632482910352503791">
    <node type="Start" id="632663009636381117" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="373">
      <linkto id="632663646668754491" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632663009636381119" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="369" y="373">
      <linkto id="632663646668754494" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"HangupOutboundCall: hanging up call with callId: " + callId</log>
      </Properties>
    </node>
    <node type="Action" id="632663009636381121" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="494" y="760">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632663646668754491" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="140" y="373">
      <linkto id="632663646668754492" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632663646668754496" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(int index, DataTable g_numbersTable, ref string callId, ref string timerId, ref int g_numFailedDialedCalls)
{
	g_numFailedDialedCalls++;
	DataRow row = null;
	try
	{
		row = g_numbersTable.Rows[index];
	}
	catch
	{ return IApp.VALUE_FAILURE; }

	callId = row["call_id"] as string;
	timerId = row["timer_id"] as string;
	row["call_state"] = CallState.ENDED;
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632663646668754492" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="257" y="373">
      <linkto id="632663009636381119" type="Labeled" style="Bevel" ortho="true" label="unequal" />
      <linkto id="632663646668754494" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="csharp">string.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632663646668754494" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="370" y="502">
      <linkto id="632663646668754495" type="Labeled" style="Bevel" ortho="true" label="unequal" />
      <linkto id="632800221463702411" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">timerId</ap>
        <ap name="Value2" type="csharp">string.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632663646668754495" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="493" y="502">
      <linkto id="632800221463702411" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">timerId</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">HangupOutoundCall: Removing outstanding timer.</log>
      </Properties>
    </node>
    <node type="Action" id="632663646668754496" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="140" y="498">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
      </Properties>
    </node>
    <node type="Action" id="632800221463702411" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="439" y="608" mx="496" my="624">
      <items count="1">
        <item text="WriteFindMeRecord" />
      </items>
      <linkto id="632663009636381121" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Index" type="variable">index</ap>
        <ap name="EndReason" type="literal">Normal</ap>
        <ap name="FunctionName" type="literal">WriteFindMeRecord</ap>
      </Properties>
    </node>
    <node type="Variable" id="632663009636381120" name="index" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="Index" refType="reference">index</Properties>
    </node>
    <node type="Variable" id="632663646668754493" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">timerId</Properties>
    </node>
    <node type="Variable" id="632664669790126395" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Exit" startnode="632736982806206994" treenode="632736982806206995" appnode="632736982806206992" handlerfor="632482910352503791">
    <node type="Start" id="632736982806206994" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="96" y="236">
      <linkto id="632736982806207003" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632736982806207003" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="196" y="236">
      <linkto id="632736982806207004" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632736982806207006" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_sessionCountIncremented</ap>
      </Properties>
    </node>
    <node type="Action" id="632736982806207004" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="325.840454" y="236">
      <linkto id="632736982806207006" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">"UPDATE as_users SET current_active_sessions=current_active_sessions - 1 WHERE ((as_users_id=" + g_userId + ") AND (current_active_sessions &gt; 0))"</ap>
        <ap name="Name" type="variable">db_ConnectionName</ap>
      </Properties>
    </node>
    <node type="Action" id="632736982806207006" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="326" y="351">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="WriteFindMeRecord" startnode="632797562403354948" treenode="632797562403354949" appnode="632797562403354946" handlerfor="632482910352503791">
    <node type="Start" id="632797562403354948" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="137">
      <linkto id="632800221463702399" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632797562403354950" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="441" y="136">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632800221463702395" name="WriteFindMeCallRecord" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="294" y="136">
      <linkto id="632797562403354950" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632800221463702400" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordId" type="variable">g_callRecordId</ap>
        <ap name="OriginNumber" type="variable">g_from</ap>
        <ap name="DestinationNumber" type="variable">to</ap>
        <ap name="TypeOfCall" type="variable">callType</ap>
        <ap name="CallEndReason" type="variable">endReason</ap>
      </Properties>
    </node>
    <node type="Action" id="632800221463702399" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="148" y="137">
      <linkto id="632800221463702400" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632800221463702395" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties language="csharp">
public static string Execute(ref int index, ref string callType, ref string to, DataTable g_numbersTable, ref string endReason, LogWriter log)
{
	DataRow row = null;
	string internalUserData = null;

	try
	{
		row = g_numbersTable.Rows[index];
		internalUserData = row["user_data"] as string;
		to = row[SqlConstants.Tables.ExternalNumbers.PhoneNumber] as string;
	}
	catch
	{
		log.Write(TraceLevel.Info, "Unable to retrieve information for call with index '" + index + "' from the FindMe calls table.");
		return IApp.VALUE_FAILURE;
	}


	// determine type of call that was just hung up
	switch (internalUserData)
	{
		case "newCall"	: callType = "FindMe"; break;
		case "swapCall"   : callType = "Swap"; break;
		case "transfer"	: callType = "Transfer"; break;
		default		: callType = "FindMe"; break;
	}

      switch (endReason)
      {
      	case "Busy"          :
            case "Normal"        :
            case "Ringout"       :
            case "Unreachable"   :
            case "InternalError" : break;
            default              : endReason = "Unknown"; break;
	}  

	return IApp.VALUE_SUCCESS;			
}
</Properties>
    </node>
    <node type="Action" id="632800221463702400" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="149" y="248">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
      </Properties>
    </node>
    <node type="Variable" id="632797562403354951" name="index" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="Index" defaultInitWith="0" refType="reference">index</Properties>
    </node>
    <node type="Variable" id="632797562403354952" name="endReason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="EndReason" defaultInitWith="Unknown" refType="reference" name="Metreos.CallControl.MakeCall_Failed">endReason</Properties>
    </node>
    <node type="Variable" id="632800221463702397" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632800499137806707" name="callType" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="FindMe" refType="reference">callType</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>