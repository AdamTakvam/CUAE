<Application name="CallRouter" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="CallRouter">
    <outline>
      <treenode type="evh" id="632341380293338945" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632128891017895337" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632128891017895336" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632341380293338968" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632130617186142166" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632130617186142165" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632767396136245232" actid="632467498335206452" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632341380293338970" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632130617186142170" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632130617186142169" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632767396136245233" actid="632467498335206452" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632341380293338972" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632128930879012599" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632128930879012598" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632767396136245068" actid="632466802523010642" />
          <ref id="632767396136245087" actid="632130681307343951" />
          <ref id="632767396136245091" actid="632132133427608992" />
          <ref id="632767396136245094" actid="632132133427608995" />
          <ref id="632767396136245097" actid="632132133427608998" />
          <ref id="632767396136245163" actid="632129680802394352" />
          <ref id="632767396136245202" actid="632130617186142155" />
          <ref id="632767396136245205" actid="632130617186142158" />
          <ref id="632767396136245208" actid="632130617186142177" />
          <ref id="632767396136245215" actid="632132376626152581" />
          <ref id="632767396136245218" actid="632152926948472030" />
          <ref id="632767396136245239" actid="632129965743418574" />
          <ref id="632767396136245242" actid="632129965743418578" />
          <ref id="632767396136245245" actid="632129965743418581" />
          <ref id="632767396136245248" actid="632129965743418584" />
          <ref id="632767396136245251" actid="632129965743418587" />
          <ref id="632767396136245260" actid="632130596839485065" />
          <ref id="632767396136245264" actid="632152926948472027" />
          <ref id="632767396136245293" actid="632133259292691500" />
          <ref id="632767396136245296" actid="632133259292691503" />
          <ref id="632767396136245299" actid="632133259292691506" />
          <ref id="632767396136245302" actid="632133259292691509" />
          <ref id="632767396136245305" actid="632133259292691512" />
          <ref id="632767396136245308" actid="632133259292691515" />
          <ref id="632767396136245311" actid="632133259292691518" />
          <ref id="632767396136245337" actid="632345638121320674" />
          <ref id="632767396136245361" actid="632467498335206472" />
          <ref id="632767396136245384" actid="632130488620974478" />
          <ref id="632767396136245387" actid="632130488620974481" />
          <ref id="632767396136245390" actid="632130488620974484" />
          <ref id="632767396136245401" actid="632130488620974419" />
          <ref id="632767396136245405" actid="632130488620974428" />
          <ref id="632767396136245408" actid="632130488620974431" />
          <ref id="632767396136245411" actid="632130488620974435" />
          <ref id="632767396136245428" actid="632130427597727240" />
          <ref id="632767396136245431" actid="632130427597727243" />
          <ref id="632767396136245436" actid="632130488620974445" />
          <ref id="632767396136245448" actid="632130488620974461" />
          <ref id="632767396136245451" actid="632130488620974464" />
          <ref id="632767396136245454" actid="632130488620974467" />
          <ref id="632767396136245460" actid="632155553466581347" />
          <ref id="632767396136245473" actid="632130617186142139" />
          <ref id="632767396136245476" actid="632130617186142142" />
          <ref id="632767396136245491" actid="632345739338163357" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632341380293339017" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632128930879012603" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632128930879012602" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632767396136245069" actid="632466802523010642" />
          <ref id="632767396136245088" actid="632130681307343951" />
          <ref id="632767396136245092" actid="632132133427608992" />
          <ref id="632767396136245095" actid="632132133427608995" />
          <ref id="632767396136245098" actid="632132133427608998" />
          <ref id="632767396136245164" actid="632129680802394352" />
          <ref id="632767396136245203" actid="632130617186142155" />
          <ref id="632767396136245206" actid="632130617186142158" />
          <ref id="632767396136245209" actid="632130617186142177" />
          <ref id="632767396136245216" actid="632132376626152581" />
          <ref id="632767396136245219" actid="632152926948472030" />
          <ref id="632767396136245240" actid="632129965743418574" />
          <ref id="632767396136245243" actid="632129965743418578" />
          <ref id="632767396136245246" actid="632129965743418581" />
          <ref id="632767396136245249" actid="632129965743418584" />
          <ref id="632767396136245252" actid="632129965743418587" />
          <ref id="632767396136245261" actid="632130596839485065" />
          <ref id="632767396136245265" actid="632152926948472027" />
          <ref id="632767396136245294" actid="632133259292691500" />
          <ref id="632767396136245297" actid="632133259292691503" />
          <ref id="632767396136245300" actid="632133259292691506" />
          <ref id="632767396136245303" actid="632133259292691509" />
          <ref id="632767396136245306" actid="632133259292691512" />
          <ref id="632767396136245309" actid="632133259292691515" />
          <ref id="632767396136245312" actid="632133259292691518" />
          <ref id="632767396136245338" actid="632345638121320674" />
          <ref id="632767396136245362" actid="632467498335206472" />
          <ref id="632767396136245385" actid="632130488620974478" />
          <ref id="632767396136245388" actid="632130488620974481" />
          <ref id="632767396136245391" actid="632130488620974484" />
          <ref id="632767396136245402" actid="632130488620974419" />
          <ref id="632767396136245406" actid="632130488620974428" />
          <ref id="632767396136245409" actid="632130488620974431" />
          <ref id="632767396136245412" actid="632130488620974435" />
          <ref id="632767396136245429" actid="632130427597727240" />
          <ref id="632767396136245432" actid="632130427597727243" />
          <ref id="632767396136245437" actid="632130488620974445" />
          <ref id="632767396136245449" actid="632130488620974461" />
          <ref id="632767396136245452" actid="632130488620974464" />
          <ref id="632767396136245455" actid="632130488620974467" />
          <ref id="632767396136245461" actid="632155553466581347" />
          <ref id="632767396136245474" actid="632130617186142139" />
          <ref id="632767396136245477" actid="632130617186142142" />
          <ref id="632767396136245492" actid="632345739338163357" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632341380293339062" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632129680802394332" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632129680802394331" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632767396136245081" actid="632385585941563433" />
          <ref id="632767396136245118" actid="632129680802394339" />
          <ref id="632767396136245122" actid="632130427597727246" />
          <ref id="632767396136245125" actid="632130427597727256" />
          <ref id="632767396136245128" actid="632130427597727259" />
          <ref id="632767396136245131" actid="632130427597727262" />
          <ref id="632767396136245134" actid="632130596839485071" />
          <ref id="632767396136245137" actid="632130617186142145" />
          <ref id="632767396136245329" actid="632152926948470170" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632341380293339071" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632129680802394336" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632129680802394335" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632767396136245082" actid="632385585941563433" />
          <ref id="632767396136245119" actid="632129680802394339" />
          <ref id="632767396136245123" actid="632130427597727246" />
          <ref id="632767396136245126" actid="632130427597727256" />
          <ref id="632767396136245129" actid="632130427597727259" />
          <ref id="632767396136245132" actid="632130427597727262" />
          <ref id="632767396136245135" actid="632130596839485071" />
          <ref id="632767396136245138" actid="632130617186142145" />
          <ref id="632767396136245330" actid="632152926948470170" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632471036673718768" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632471036673718765" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632471036673718764" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632471036673718773" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632471036673718770" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632471036673718769" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632471036673718778" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632471036673718775" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632471036673718774" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632341380293339080" level="1" text="HandleChangePIN">
        <node type="function" name="HandleChangePIN" id="632130596839485075" path="Metreos.StockTools" />
        <calls>
          <ref actid="632130596839485074" />
        </calls>
      </treenode>
      <treenode type="fun" id="632341380293339081" level="1" text="HandleClientCode">
        <node type="function" name="HandleClientCode" id="632130488620974411" path="Metreos.StockTools" />
        <calls>
          <ref actid="632130488620974410" />
        </calls>
      </treenode>
      <treenode type="fun" id="632341380293339082" level="1" text="HandleDN">
        <node type="function" name="HandleDN" id="632130488620974494" path="Metreos.StockTools" />
        <calls>
          <ref actid="632130488620974489" />
        </calls>
      </treenode>
      <treenode type="fun" id="632341380293339083" level="1" text="HandleLogin">
        <node type="function" name="HandleLogin" id="632129930790358507" path="Metreos.StockTools" />
        <calls>
          <ref actid="632129930790358505" />
        </calls>
      </treenode>
      <treenode type="fun" id="632341380293339084" level="1" text="HandleLoginSuccess">
        <node type="function" name="HandleLoginSuccess" id="632130427597727233" path="Metreos.StockTools" />
        <calls>
          <ref actid="632130427597727232" />
        </calls>
      </treenode>
      <treenode type="fun" id="632341380293339085" level="1" text="HandleMatterCode">
        <node type="function" name="HandleMatterCode" id="632130488620974407" path="Metreos.StockTools" />
        <calls>
          <ref actid="632130488620974388" />
        </calls>
      </treenode>
      <treenode type="fun" id="632341380293339086" level="1" text="HandleProjCode">
        <node type="function" name="HandleProjCode" id="632130488620974398" path="Metreos.StockTools" />
        <calls>
          <ref actid="632130488620974386" />
        </calls>
      </treenode>
      <treenode type="fun" id="632341380293339087" level="1" text="HandleGatherDigitsTimeout">
        <node type="function" name="HandleGatherDigitsTimeout" id="632133259292691488" path="Metreos.StockTools" />
        <calls>
          <ref actid="632133259292691486" />
        </calls>
      </treenode>
      <treenode type="fun" id="632341380293339088" level="1" text="HandleRingback">
        <node type="function" name="HandleRingback" id="632132376626152585" path="Metreos.StockTools" />
        <calls>
          <ref actid="632132376626152583" />
        </calls>
      </treenode>
      <treenode type="fun" id="632341380293339089" level="1" text="HandleUserEndCall">
        <node type="function" name="HandleUserEndCall" id="632155553466580693" path="Metreos.StockTools" />
        <calls>
          <ref actid="632155553466580692" />
        </calls>
      </treenode>
      <treenode type="evh" id="632341380293339093" level="1" text="Metreos.CallControl.GotDigits: OnGotDigits">
        <node type="function" name="OnGotDigits" id="632131420292420901" path="Metreos.StockTools" />
        <node type="event" name="GotDigits" id="632131420292420900" path="Metreos.CallControl.GotDigits" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632467498335206451" level="1" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632467498335206448" path="Metreos.StockTools" />
        <calls>
          <ref actid="632132483097451345" />
          <ref actid="632132483097451346" />
          <ref actid="632132483097451347" />
          <ref actid="632132483097451348" />
          <ref actid="632467498335206463" />
          <ref actid="632467498335206465" />
          <ref actid="632467553614951522" />
        </calls>
        <node type="event" name="RemoteHangup" id="632467498335206447" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632767396136245234" actid="632467498335206452" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_hostConnId" id="632767396136244979" vid="632128911668489437">
        <Properties type="Metreos.Types.String">g_hostConnId</Properties>
      </treenode>
      <treenode text="g_calleeConnId" id="632767396136244981" vid="632129643373072715">
        <Properties type="Metreos.Types.String">g_calleeConnId</Properties>
      </treenode>
      <treenode text="g_confId" id="632767396136244983" vid="632129643373072717">
        <Properties type="Metreos.Types.String">g_confId</Properties>
      </treenode>
      <treenode text="g_acctCodeLength" id="632767396136244985" vid="632129680802394342">
        <Properties type="Metreos.Types.String" defaultInitWith="0">g_acctCodeLength</Properties>
      </treenode>
      <treenode text="g_origNumber" id="632767396136244987" vid="632129930790358512">
        <Properties type="Metreos.Types.String">g_origNumber</Properties>
      </treenode>
      <treenode text="g_accessNumber" id="632767396136244989" vid="632129930790358514">
        <Properties type="Metreos.Types.String">g_accessNumber</Properties>
      </treenode>
      <treenode text="g_accountCode" id="632767396136244991" vid="632129940925832619">
        <Properties type="Metreos.Types.String">g_accountCode</Properties>
      </treenode>
      <treenode text="g_pinCodeLength" id="632767396136244993" vid="632129940925832622">
        <Properties type="Metreos.Types.String" defaultInitWith="0">g_pinCodeLength</Properties>
      </treenode>
      <treenode text="g_hostCallId" id="632767396136244995" vid="632129965743418600">
        <Properties type="Metreos.Types.String">g_hostCallId</Properties>
      </treenode>
      <treenode text="g_useProjCodes" id="632767396136244997" vid="632130427597727228">
        <Properties type="Metreos.Types.Bool" defaultInitWith="false">g_useProjCodes</Properties>
      </treenode>
      <treenode text="g_useClientCodes" id="632767396136244999" vid="632130427597727230">
        <Properties type="Metreos.Types.Bool" defaultInitWith="false">g_useClientCodes</Properties>
      </treenode>
      <treenode text="g_projCodeLength" id="632767396136245001" vid="632130427597727248">
        <Properties type="Metreos.Types.Int" defaultInitWith="3">g_projCodeLength</Properties>
      </treenode>
      <treenode text="g_clientCodeLength" id="632767396136245003" vid="632130427597727250">
        <Properties type="Metreos.Types.Int" defaultInitWith="3">g_clientCodeLength</Properties>
      </treenode>
      <treenode text="g_matterCodeLength" id="632767396136245005" vid="632130427597727252">
        <Properties type="Metreos.Types.Int" defaultInitWith="3">g_matterCodeLength</Properties>
      </treenode>
      <treenode text="g_projCode" id="632767396136245007" vid="632130488620974392">
        <Properties type="Metreos.Types.String">g_projCode</Properties>
      </treenode>
      <treenode text="g_clientCode" id="632767396136245009" vid="632130488620974394">
        <Properties type="Metreos.Types.String">g_clientCode</Properties>
      </treenode>
      <treenode text="g_matterCode" id="632767396136245011" vid="632130488620974396">
        <Properties type="Metreos.Types.String">g_matterCode</Properties>
      </treenode>
      <treenode text="g_theDestinationNumber" id="632767396136245013" vid="632130488620974492">
        <Properties type="Metreos.Types.String">g_theDestinationNumber</Properties>
      </treenode>
      <treenode text="g_callRecordId" id="632767396136245015" vid="632130617186142161">
        <Properties type="UInt">g_callRecordId</Properties>
      </treenode>
      <treenode text="g_intlDialingPrefix" id="632767396136245017" vid="632130681307343942">
        <Properties type="Metreos.Types.String" defaultInitWith="011">g_intlDialingPrefix</Properties>
      </treenode>
      <treenode text="g_localCountryCode" id="632767396136245019" vid="632130681307343944">
        <Properties type="Metreos.Types.String" defaultInitWith="1">g_localCountryCode</Properties>
      </treenode>
      <treenode text="g_calleeCallId" id="632767396136245021" vid="632130681307343947">
        <Properties type="Metreos.Types.String">g_calleeCallId</Properties>
      </treenode>
      <treenode text="g_cdrOpen" id="632767396136245023" vid="632130681307343982">
        <Properties type="Metreos.Types.Bool">g_cdrOpen</Properties>
      </treenode>
      <treenode text="g_EndReason" id="632767396136245025" vid="632130716512766898">
        <Properties type="Metreos.Types.String">g_EndReason</Properties>
      </treenode>
      <treenode text="g_loggedIn" id="632767396136245027" vid="632131420292420908">
        <Properties type="Metreos.Types.Bool">g_loggedIn</Properties>
      </treenode>
      <treenode text="g_inCall" id="632767396136245029" vid="632132483097451343">
        <Properties type="Metreos.Types.String">g_inCall</Properties>
      </treenode>
      <treenode text="g_dialPlan" id="632767396136245031" vid="632133216637116971">
        <Properties type="Metreos.Types.Hashtable" initWith="DialPlan">g_dialPlan</Properties>
      </treenode>
      <treenode text="g_GatherDigitsTimeout" id="632767396136245033" vid="632152240292190227">
        <Properties type="Metreos.Types.String" initWith="Receive_Digits_Timeout">g_GatherDigitsTimeout</Properties>
      </treenode>
      <treenode text="g_interDigitDelay" id="632767396136245035" vid="632152240292191449">
        <Properties type="Metreos.Types.String" initWith="Inter_Digit_Delay">g_interDigitDelay</Properties>
      </treenode>
      <treenode text="g_numFailedLoginAttempts" id="632767396136245037" vid="632152926948472021">
        <Properties type="Metreos.Types.Int">g_numFailedLoginAttempts</Properties>
      </treenode>
      <treenode text="g_GatherDigitsEnabled" id="632767396136245039" vid="632155553466580711">
        <Properties type="Metreos.Types.String">g_GatherDigitsEnabled</Properties>
      </treenode>
      <treenode text="g_numFailedLoginAttemptsAllowed" id="632767396136245041" vid="632155553466580717">
        <Properties type="Metreos.Types.Int" initWith="Num_Failed_Logins_Allowed">g_numFailedLoginAttemptsAllowed</Properties>
      </treenode>
      <treenode text="g_authRecordsId" id="632767396136245043" vid="632344780282204336">
        <Properties type="UInt">g_authRecordsId</Properties>
      </treenode>
      <treenode text="g_loggedInUserId" id="632767396136245045" vid="632344780282204338">
        <Properties type="UInt">g_loggedInUserId</Properties>
      </treenode>
      <treenode text="g_dbUsername" id="632767396136245047" vid="632344780282204340">
        <Properties type="String" initWith="DB_Username">g_dbUsername</Properties>
      </treenode>
      <treenode text="g_dbPassword" id="632767396136245049" vid="632344780282204342">
        <Properties type="String" initWith="DB_Password">g_dbPassword</Properties>
      </treenode>
      <treenode text="g_dbHost" id="632767396136245051" vid="632344780282204344">
        <Properties type="String" initWith="DB_Address">g_dbHost</Properties>
      </treenode>
      <treenode text="g_dbPort" id="632767396136245053" vid="632344780282204346">
        <Properties type="String" initWith="DB_Port">g_dbPort</Properties>
      </treenode>
      <treenode text="g_dbDatabaseName" id="632767396136245055" vid="632344780282204348">
        <Properties type="String" defaultInitWith="application_suite">g_dbDatabaseName</Properties>
      </treenode>
      <treenode text="g_sessionRecordsId" id="632767396136245057" vid="632344780282204354">
        <Properties type="String">g_sessionRecordsId</Properties>
      </treenode>
      <treenode text="g_mmsId" id="632767396136245496" vid="632767396136245495">
        <Properties type="UInt">g_mmsId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632128891017895338" treenode="632341380293338945" appnode="632128891017895337" handlerfor="632128891017895336">
    <node type="Start" id="632128891017895338" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="104">
      <linkto id="632344880108146991" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632341383106383923" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="373" y="105">
      <linkto id="632431125209478117" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">origNumber</ap>
        <ap name="Value2" type="variable">callId</ap>
        <ap name="Value3" type="variable">accessNum</ap>
        <rd field="ResultData">g_origNumber</rd>
        <rd field="ResultData2">g_hostCallId</rd>
        <rd field="ResultData3">g_accessNumber</rd>
      </Properties>
    </node>
    <node type="Comment" id="632341383106383925" text="Assign event parameters to globals." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="299" y="57" />
    <node type="Action" id="632341440574919589" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="218" y="358">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632344880108146991" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="104" y="104">
      <linkto id="632344880108146993" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">g_dbDatabaseName</ap>
        <ap name="Server" type="variable">g_dbHost</ap>
        <ap name="Port" type="variable">g_dbPort</ap>
        <ap name="Username" type="variable">g_dbUsername</ap>
        <ap name="Password" type="variable">g_dbPassword</ap>
        <rd field="DSN">databaseDSN</rd>
      </Properties>
    </node>
    <node type="Action" id="632344880108146993" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="216" y="104">
      <linkto id="632341383106383923" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632466802523010639" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">databaseDSN</ap>
        <ap name="Name" type="literal">ApplicationSuite</ap>
        <ap name="Type" type="literal">mysql</ap>
        <log condition="entry" on="false" level="Info" type="csharp">"DSN: " + databaseDSN</log>
        <log condition="default" on="true" level="Info" type="csharp">"Failed to open database. DSN: " + databaseDSN</log>
      </Properties>
    </node>
    <node type="Comment" id="632344880108146994" text="Deny call because we can't&#xD;&#xA;connect to the database." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="268" y="227" />
    <node type="Action" id="632431125209478117" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="507" y="105">
      <linkto id="632466802523010661" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref string origNumber)
	{
		if ((origNumber == null) || (origNumber == string.Empty))
			origNumber = "UNAVAILABLE";

		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632466802523010639" name="RejectCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="217" y="248">
      <linkto id="632341440574919589" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632466802523010642" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="726.4258" y="89" mx="779" my="105">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632466802523010644" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632470177828526515" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">enter_acct.wav</ap>
        <ap name="Prompt1" type="literal">welcome.wav</ap>
        <ap name="Prompt3" type="literal">pound_sign.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_acct</ap>
      </Properties>
    </node>
    <node type="Action" id="632466802523010644" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="984.4258" y="105">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632466802523010660" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="646" y="396">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632466802523010661" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="647" y="105">
      <linkto id="632466802523010642" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632466802523010660" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DisplayName" type="literal">VoiceTunnel</ap>
        <ap name="Conference" type="literal">true</ap>
        <rd field="MmsId">g_mmsId</rd>
        <rd field="ConnectionId">g_hostConnId</rd>
        <rd field="ConferenceId">g_confId</rd>
      </Properties>
    </node>
    <node type="Action" id="632470177828526515" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="776" y="253">
      <linkto id="632466802523010660" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Variable" id="632128901502571285" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="callId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632129930790358516" name="origNumber" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="from" refType="reference">origNumber</Properties>
    </node>
    <node type="Variable" id="632129930790358517" name="accessNum" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="to" refType="reference">accessNum</Properties>
    </node>
    <node type="Variable" id="632344880108146992" name="databaseDSN" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">databaseDSN</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632130617186142167" treenode="632341380293338968" appnode="632130617186142166" handlerfor="632130617186142165">
    <node type="Start" id="632130617186142167" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="108">
      <linkto id="632385585941563437" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632341440574919615" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="607" y="107">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632385585941563433" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="411" y="92" mx="485" my="108">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632341380293339062" />
        <item text="OnGatherDigits_Failed" treenode="632341380293339071" />
      </items>
      <linkto id="632341440574919615" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondDigitPattern" type="literal">###</ap>
        <ap name="CommandTimeout" type="literal">0</ap>
        <ap name="UserData" type="literal">user_end_call</ap>
      </Properties>
    </node>
    <node type="Action" id="632385585941563437" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="185" y="108">
      <linkto id="632485594227550373" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
      </Properties>
    </node>
    <node type="Action" id="632485594227550373" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="357" y="108">
      <linkto id="632385585941563433" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute()
{
	System.Threading.Thread.Sleep(400);
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632130617186142171" treenode="632341380293338970" appnode="632130617186142170" handlerfor="632130617186142169">
    <node type="Start" id="632130617186142171" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="475.999" y="112">
      <linkto id="632132376626152596" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632130681307343951" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="989.000061" y="249" mx="1042" my="265">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919622" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">enter_dn.wav</ap>
        <ap name="Prompt1" type="literal">error_call_unreachable.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_dn</ap>
      </Properties>
    </node>
    <node type="Action" id="632132133427608989" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="597" y="251">
      <linkto id="632132483097451348" type="Labeled" style="Bezier" ortho="true" label="Unreachable" />
      <linkto id="632132483097451347" type="Labeled" style="Bezier" ortho="true" label="Busy" />
      <linkto id="632132483097451346" type="Labeled" style="Bezier" ortho="true" label="Ringout" />
      <linkto id="632132483097451345" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">reason</ap>
      </Properties>
    </node>
    <node type="Action" id="632132133427608992" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="736" y="540" mx="789" my="556">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919621" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">enter_dn.wav</ap>
        <ap name="Prompt1" type="literal">error_call_busy.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_dn</ap>
      </Properties>
    </node>
    <node type="Action" id="632132133427608995" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="510.999" y="532" mx="564" my="548">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919620" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">enter_dn.wav</ap>
        <ap name="Prompt1" type="literal">error_call_ringout.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_dn</ap>
      </Properties>
    </node>
    <node type="Action" id="632132133427608998" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="236" y="250" mx="289" my="266">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919619" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">enter_dn.wav</ap>
        <ap name="Prompt1" type="literal">error_call.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_dn</ap>
      </Properties>
    </node>
    <node type="Comment" id="632132133427609002" text="Call failed because the number was unreachable" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="844" y="203" />
    <node type="Comment" id="632132133427609003" text="Call failed because the line was busy" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="760" y="389" />
    <node type="Comment" id="632132133427609004" text="Call failed because the line&#xD;&#xA;was not answered" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="327.999" y="454" />
    <node type="Comment" id="632132133427609005" text="Call failed for other or unknown reason." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="247.999" y="210" />
    <node type="Action" id="632132376626152596" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="594.998962" y="112">
      <linkto id="632132133427608989" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
      </Properties>
    </node>
    <node type="Action" id="632132483097451345" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="385.792633" y="250" mx="440" my="266">
      <items count="1">
        <item text="OnRemoteHangup" treenode="632467498335206451" />
      </items>
      <linkto id="632132133427608998" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="callId" type="variable">g_calleeCallId</ap>
        <ap name="EndReason" type="variable">reason</ap>
        <ap name="FunctionName" type="literal">OnRemoteHangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632132483097451346" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="509.792664" y="415" mx="564" my="431">
      <items count="1">
        <item text="OnRemoteHangup" treenode="632467498335206451" />
      </items>
      <linkto id="632132133427608995" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="callId" type="variable">g_calleeCallId</ap>
        <ap name="EndReason" type="variable">reason</ap>
        <ap name="FunctionName" type="literal">OnRemoteHangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632132483097451347" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="734.792664" y="418" mx="789" my="434">
      <items count="1">
        <item text="OnRemoteHangup" treenode="632467498335206451" />
      </items>
      <linkto id="632132133427608992" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="callId" type="variable">g_calleeCallId</ap>
        <ap name="EndReason" type="variable">reason</ap>
        <ap name="FunctionName" type="literal">OnRemoteHangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632132483097451348" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="852.792664" y="249" mx="907" my="265">
      <items count="1">
        <item text="OnRemoteHangup" treenode="632467498335206451" />
      </items>
      <linkto id="632130681307343951" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="callId" type="variable">g_calleeCallId</ap>
        <ap name="EndReason" type="variable">reason</ap>
        <ap name="FunctionName" type="literal">OnRemoteHangup</ap>
      </Properties>
    </node>
    <node type="Comment" id="632133119091953914" text="Stop ringing" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="563" y="68" />
    <node type="Label" id="632341440574919617" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="738" y="111">
      <linkto id="632341440574919618" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632341440574919618" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="819" y="111">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632341440574919619" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="188" y="266" />
    <node type="Label" id="632341440574919620" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="560" y="686" />
    <node type="Label" id="632341440574919621" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="785" y="693" />
    <node type="Label" id="632341440574919622" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1164.04419" y="265" />
    <node type="Variable" id="632132133427608988" name="reason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="EndReason" refType="reference">reason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632128930879012600" treenode="632341380293338972" appnode="632128930879012599" handlerfor="632128930879012598">
    <node type="Start" id="632128930879012600" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="565" y="38">
      <linkto id="632129680802394341" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632129680802394339" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="709" y="183" mx="783" my="199">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632341380293339062" />
        <item text="OnGatherDigits_Failed" treenode="632341380293339071" />
      </items>
      <linkto id="632341440574919634" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondInterDigitDelay" type="variable">g_interDigitDelay</ap>
        <ap name="TermCondMaxDigits" type="variable">g_acctCodeLength</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxTime" type="variable">g_GatherDigitsTimeout</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">enter_acct</ap>
      </Properties>
    </node>
    <node type="Action" id="632129680802394341" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="562" y="292">
      <linkto id="632129680802394339" type="Labeled" style="Bezier" ortho="true" label="enter_acct" />
      <linkto id="632130427597727246" type="Labeled" style="Bezier" ortho="true" label="enter_proj_code" />
      <linkto id="632130427597727256" type="Labeled" style="Bezier" ortho="true" label="enter_client_code" />
      <linkto id="632130427597727259" type="Labeled" style="Bezier" ortho="true" label="enter_matter_code" />
      <linkto id="632130427597727262" type="Labeled" style="Bezier" ortho="true" label="enter_dn" />
      <linkto id="632130596839485071" type="Labeled" style="Bezier" ortho="true" label="change_pin_notice" />
      <linkto id="632130617186142145" type="Labeled" style="Bezier" ortho="true" label="enter_pin" />
      <linkto id="632132376626152583" type="Labeled" style="Bezier" ortho="true" label="ringback" />
      <linkto id="632341440574919695" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632467498335206462" type="Labeled" style="Bezier" ortho="true" label="fatal_error" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">state</ap>
      </Properties>
    </node>
    <node type="Action" id="632130427597727246" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="690" y="372" mx="764" my="388">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632341380293339062" />
        <item text="OnGatherDigits_Failed" treenode="632341380293339071" />
      </items>
      <linkto id="632341440574919631" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondInterDigitDelay" type="variable">g_interDigitDelay</ap>
        <ap name="TermCondMaxDigits" type="variable">g_projCodeLength</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxTime" type="variable">g_GatherDigitsTimeout</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">enter_proj_code</ap>
        <log condition="entry" on="true" level="Info" type="csharp">String.Format("Begging for '{0}' digits for project code", g_projCodeLength.ToString())</log>
      </Properties>
    </node>
    <node type="Action" id="632130427597727256" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="595" y="497" mx="669" my="513">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632341380293339062" />
        <item text="OnGatherDigits_Failed" treenode="632341380293339071" />
      </items>
      <linkto id="632341440574919630" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondInterDigitDelay" type="variable">g_interDigitDelay</ap>
        <ap name="TermCondMaxDigits" type="variable">g_clientCodeLength</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxTime" type="variable">g_GatherDigitsTimeout</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">enter_client_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632130427597727259" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="460" y="413" mx="534" my="429">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632341380293339062" />
        <item text="OnGatherDigits_Failed" treenode="632341380293339071" />
      </items>
      <linkto id="632341440574919629" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondInterDigitDelay" type="variable">g_interDigitDelay</ap>
        <ap name="TermCondMaxDigits" type="variable">g_matterCodeLength</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxTime" type="variable">g_GatherDigitsTimeout</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">enter_matter_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632130427597727262" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="299" y="274" mx="373" my="290">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632341380293339062" />
        <item text="OnGatherDigits_Failed" treenode="632341380293339071" />
      </items>
      <linkto id="632341440574919626" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxTime" type="literal">30000</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">enter_dn</ap>
      </Properties>
    </node>
    <node type="Action" id="632130596839485071" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="247.238922" y="143" mx="322" my="159">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632341380293339062" />
        <item text="OnGatherDigits_Failed" treenode="632341380293339071" />
      </items>
      <linkto id="632341440574919625" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondInterDigitDelay" type="variable">g_interDigitDelay</ap>
        <ap name="TermCondMaxDigits" type="variable">g_pinCodeLength</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxTime" type="variable">g_GatherDigitsTimeout</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">change_pin_notice</ap>
      </Properties>
    </node>
    <node type="Action" id="632130617186142145" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="776.000061" y="273" mx="850" my="289">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632341380293339062" />
        <item text="OnGatherDigits_Failed" treenode="632341380293339071" />
      </items>
      <linkto id="632341440574919632" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondInterDigitDelay" type="variable">g_interDigitDelay</ap>
        <ap name="TermCondMaxDigits" type="variable">g_pinCodeLength</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxTime" type="variable">g_GatherDigitsTimeout</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">enter_pin</ap>
      </Properties>
    </node>
    <node type="Action" id="632130681307343959" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="180.477844" y="432">
      <linkto id="632341440574919627" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632467498335206456" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">g_calleeCallId == String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632132376626152583" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="719.8994" y="104" mx="768" my="120">
      <items count="1">
        <item text="HandleRingback" />
      </items>
      <linkto id="632341440574919633" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="terminationCondition" type="variable">termCondition</ap>
        <ap name="FunctionName" type="literal">HandleRingback</ap>
      </Properties>
    </node>
    <node type="Label" id="632341440574919623" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="681" y="43">
      <linkto id="632341440574919624" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632341440574919624" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="767" y="42">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632341440574919625" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="213" y="159" />
    <node type="Label" id="632341440574919626" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="266" y="291" />
    <node type="Label" id="632341440574919627" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="89" y="432" />
    <node type="Label" id="632341440574919628" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="182" y="734" />
    <node type="Label" id="632341440574919629" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="536.2832" y="563" />
    <node type="Label" id="632341440574919630" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="770.2832" y="514" />
    <node type="Label" id="632341440574919631" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="881.2832" y="390" />
    <node type="Label" id="632341440574919632" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="960.2832" y="289" />
    <node type="Label" id="632341440574919633" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="865.2832" y="120" />
    <node type="Label" id="632341440574919634" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="886.2832" y="200" />
    <node type="Label" id="632341440574919695" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="475" y="106" />
    <node type="Action" id="632467498335206456" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="181" y="532">
      <linkto id="632467498335206465" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_calleeCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632467498335206462" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="405" y="432">
      <linkto id="632467498335206463" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_hostCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632467498335206463" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="239.792633" y="416" mx="294" my="432">
      <items count="1">
        <item text="OnRemoteHangup" treenode="632467498335206451" />
      </items>
      <linkto id="632130681307343959" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="callId" type="variable">g_hostCallId</ap>
        <ap name="EndReason" type="literal">InternalError</ap>
        <ap name="FunctionName" type="literal">OnRemoteHangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632467498335206465" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="129.792633" y="606" mx="184" my="622">
      <items count="1">
        <item text="OnRemoteHangup" treenode="632467498335206451" />
      </items>
      <linkto id="632341440574919628" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="callId" type="variable">g_hostCallId</ap>
        <ap name="EndReason" type="literal">InternalError</ap>
        <ap name="FunctionName" type="literal">OnRemoteHangup</ap>
      </Properties>
    </node>
    <node type="Variable" id="632129680802394327" name="state" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="userData" refType="reference">state</Properties>
    </node>
    <node type="Variable" id="632132376626152582" name="termCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="terminationCondition" refType="reference">termCondition</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" startnode="632129680802394333" treenode="632341380293339062" appnode="632129680802394332" handlerfor="632129680802394331">
    <node type="Start" id="632129680802394333" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="218" y="85">
      <linkto id="632133259292691486" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632129680802394347" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="400" y="291">
      <linkto id="632129930790358505" type="Labeled" style="Bezier" ortho="true" label="enter_pin" />
      <linkto id="632130596839485074" type="Labeled" style="Bezier" ortho="true" label="change_pin_notice" />
      <linkto id="632155553466580692" type="Labeled" style="Bezier" ortho="true" label="user_end_call" />
      <linkto id="632341383106383949" type="Labeled" style="Bezier" ortho="true" label="enter_dn" />
      <linkto id="632341383106383950" type="Labeled" style="Bezier" ortho="true" label="enter_matter_code" />
      <linkto id="632341383106383951" type="Labeled" style="Bezier" ortho="true" label="enter_client_code" />
      <linkto id="632341383106383952" type="Labeled" style="Bezier" ortho="true" label="enter_proj_code" />
      <linkto id="632341383106383953" type="Labeled" style="Bezier" ortho="true" label="enter_acct" />
      <linkto id="632341440574919608" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">state</ap>
      </Properties>
    </node>
    <node type="Action" id="632129680802394352" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="631" y="66" mx="684" my="82">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919603" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">pound_sign.wav</ap>
        <ap name="Prompt1" type="literal">enter_pin.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_pin</ap>
      </Properties>
    </node>
    <node type="Action" id="632129930790358505" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="582.2324" y="175" mx="620" my="191">
      <items count="1">
        <item text="HandleLogin" />
      </items>
      <linkto id="632341440574919602" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="pin" type="variable">digits</ap>
        <ap name="FunctionName" type="literal">HandleLogin</ap>
      </Properties>
    </node>
    <node type="Action" id="632130488620974386" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="700.8994" y="274" mx="749" my="290">
      <items count="1">
        <item text="HandleProjCode" />
      </items>
      <linkto id="632341440574919600" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">HandleProjCode</ap>
      </Properties>
    </node>
    <node type="Action" id="632130488620974388" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="603.6006" y="474" mx="658" my="490">
      <items count="1">
        <item text="HandleMatterCode" />
      </items>
      <linkto id="632341440574919601" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">HandleMatterCode</ap>
      </Properties>
    </node>
    <node type="Action" id="632130488620974410" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="735.194336" y="381" mx="788" my="397">
      <items count="1">
        <item text="HandleClientCode" />
      </items>
      <linkto id="632341440574919599" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">HandleClientCode</ap>
      </Properties>
    </node>
    <node type="Action" id="632130488620974489" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="63.8258972" y="332" mx="101" my="348">
      <items count="1">
        <item text="HandleDN" />
      </items>
      <linkto id="632341440574919605" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">HandleDN</ap>
      </Properties>
    </node>
    <node type="Action" id="632130596839485074" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="183.928772" y="155" mx="238" my="171">
      <items count="1">
        <item text="HandleChangePIN" />
      </items>
      <linkto id="632341440574919606" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="pin" type="variable">digits</ap>
        <ap name="FunctionName" type="literal">HandleChangePIN</ap>
      </Properties>
    </node>
    <node type="Action" id="632133259292691486" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="271.4541" y="70" mx="350" my="86">
      <items count="1">
        <item text="HandleGatherDigitsTimeout" />
      </items>
      <linkto id="632129680802394347" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632341440574919607" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="appControl" log="On">
        <ap name="terminationCondition" type="variable">terminationCondition</ap>
        <ap name="state" type="variable">state</ap>
        <ap name="FunctionName" type="literal">HandleGatherDigitsTimeout</ap>
        <log condition="entry" on="false" level="Info" type="csharp">"Receive digits complete: " + terminationCondition</log>
      </Properties>
    </node>
    <node type="Comment" id="632133259292691497" text="Check for digit timeout" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="303" y="34" />
    <node type="Action" id="632155553466580692" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="147.198242" y="471" mx="204" my="487">
      <items count="1">
        <item text="HandleUserEndCall" />
      </items>
      <linkto id="632341440574919604" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="terminationCondition" type="variable">terminationCondition</ap>
        <ap name="FunctionName" type="literal">HandleUserEndCall</ap>
        <log condition="entry" on="true" level="Info" type="literal">User requesting end call</log>
      </Properties>
    </node>
    <node type="Action" id="632341383106383949" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="236" y="350">
      <linkto id="632130488620974489" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">digits.Replace("#", String.Empty)</ap>
        <rd field="ResultData">g_theDestinationNumber</rd>
      </Properties>
    </node>
    <node type="Action" id="632341383106383950" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="526" y="491">
      <linkto id="632130488620974388" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">digits</ap>
        <rd field="ResultData">g_matterCode</rd>
      </Properties>
    </node>
    <node type="Action" id="632341383106383951" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="654" y="399">
      <linkto id="632130488620974410" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">digits</ap>
        <rd field="ResultData">g_clientCode</rd>
      </Properties>
    </node>
    <node type="Action" id="632341383106383952" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="638" y="290">
      <linkto id="632130488620974386" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">digits</ap>
        <rd field="ResultData">g_projCode</rd>
      </Properties>
    </node>
    <node type="Action" id="632341383106383953" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="559" y="82">
      <linkto id="632129680802394352" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">digits</ap>
        <rd field="ResultData">g_accountCode</rd>
      </Properties>
    </node>
    <node type="Action" id="632341440574919597" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="439" y="583">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632341440574919598" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="365" y="583">
      <linkto id="632341440574919597" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632341440574919599" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="872" y="398" />
    <node type="Label" id="632341440574919600" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="841" y="290" />
    <node type="Label" id="632341440574919601" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="754" y="492" />
    <node type="Label" id="632341440574919602" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="722.7611" y="191" />
    <node type="Label" id="632341440574919603" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="855.7611" y="82" />
    <node type="Label" id="632341440574919604" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="81" y="488" />
    <node type="Label" id="632341440574919605" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="97" y="266" />
    <node type="Label" id="632341440574919606" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="114" y="171" />
    <node type="Label" id="632341440574919607" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="458" y="87" />
    <node type="Label" id="632341440574919608" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="401" y="497" />
    <node type="Variable" id="632129680802394348" name="state" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="userData" refType="reference">state</Properties>
    </node>
    <node type="Variable" id="632129930790358510" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="digits" defaultInitWith="None" refType="reference">digits</Properties>
    </node>
    <node type="Variable" id="632133259292691487" name="terminationCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="terminationCondition" refType="reference">terminationCondition</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632471036673718767" treenode="632471036673718768" appnode="632471036673718765" handlerfor="632471036673718764">
    <node type="Start" id="632471036673718767" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="90">
      <linkto id="632471036673718779" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632471036673718779" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="190" y="89">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632471036673718772" treenode="632471036673718773" appnode="632471036673718770" handlerfor="632471036673718769">
    <node type="Start" id="632471036673718772" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="126">
      <linkto id="632471036673718780" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632471036673718780" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="166" y="127">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632471036673718777" treenode="632471036673718778" appnode="632471036673718775" handlerfor="632471036673718774">
    <node type="Start" id="632471036673718777" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="175">
      <linkto id="632471036673718781" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632471036673718781" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="150" y="175">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HandleDN" startnode="632130488620974495" treenode="632341380293339082" appnode="632130488620974494" handlerfor="632471036673718774">
    <node type="Start" id="632130488620974495" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="104">
      <linkto id="632344804922034677" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632130617186142155" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="233" y="248" mx="286" my="264">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919678" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">error_internal.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="UserData" type="literal">fatal_error</ap>
      </Properties>
    </node>
    <node type="Action" id="632130617186142158" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="456" y="248" mx="509" my="264">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919676" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">enter_dn.wav</ap>
        <ap name="Prompt1" type="literal">error_permission.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_dn</ap>
      </Properties>
    </node>
    <node type="Action" id="632130617186142177" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="711" y="547" mx="764" my="563">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919677" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">error_call.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="UserData" type="literal">fatal_error</ap>
      </Properties>
    </node>
    <node type="Comment" id="632130617186142179" text="No media" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="808" y="549" />
    <node type="Comment" id="632130617186142180" text="Not authorized" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="568" y="240" />
    <node type="Comment" id="632130617186142181" text="Database error" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="348" y="252" />
    <node type="Action" id="632131788159139081" name="FormatAddress" class="MaxActionNode" group="" path="Metreos.Native.DialPlan" x="616" y="104">
      <linkto id="632132376626152581" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DialedNumber" type="variable">g_theDestinationNumber</ap>
        <ap name="DialingRules" type="variable">g_dialPlan</ap>
        <rd field="ResultData">g_theDestinationNumber</rd>
      </Properties>
    </node>
    <node type="Action" id="632132376626152581" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="709" y="88" mx="762" my="104">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632467498335206452" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">beep.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">ringback</ap>
      </Properties>
    </node>
    <node type="Action" id="632152926948472030" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="78" y="407" mx="131" my="423">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919675" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">enter_dn.wav</ap>
        <ap name="Prompt1" type="literal">error_invalid_number.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_dn</ap>
      </Properties>
    </node>
    <node type="Comment" id="632152926948472032" text="Invalid number" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="34" y="370" />
    <node type="Action" id="632341383106383954" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="479" y="104">
      <linkto id="632131788159139081" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="literal">true</ap>
        <rd field="ResultData">g_cdrOpen</rd>
        <rd field="ResultData2">g_inCall</rd>
      </Properties>
    </node>
    <node type="Label" id="632341440574919673" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="328" y="480">
      <linkto id="632341440574919674" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632341440574919674" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="417" y="479">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Label" id="632341440574919675" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="128" y="576" />
    <node type="Label" id="632341440574919676" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="506" y="398" />
    <node type="Label" id="632341440574919677" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="761" y="700" />
    <node type="Label" id="632341440574919678" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="283" y="394" />
    <node type="Label" id="632341440574919679" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="914.75" y="241" />
    <node type="Action" id="632344780282204364" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="328" y="104">
      <linkto id="632341383106383954" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="OriginNumber" type="variable">g_origNumber</ap>
        <ap name="DestinationNumber" type="variable">g_theDestinationNumber</ap>
        <ap name="SessionRecordsId" type="variable">g_sessionRecordsId</ap>
        <ap name="UserId" type="variable">g_loggedInUserId</ap>
        <ap name="AuthRecordsId" type="variable">g_authRecordsId</ap>
        <rd field="CallRecordsId">g_callRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632344804922034677" name="ValidateAccountAcl" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="128" y="104">
      <linkto id="632130617186142155" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <linkto id="632152926948472030" type="Labeled" style="Bezier" ortho="true" label="InvalidNumber" />
      <linkto id="632130617186142158" type="Labeled" style="Bezier" ortho="true" label="NotAuthorized" />
      <linkto id="632344780282204364" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_loggedInUserId</ap>
        <ap name="DestinationNumber" type="variable">g_theDestinationNumber</ap>
      </Properties>
    </node>
    <node type="Action" id="632467498335206452" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="696.9453" y="225" mx="763" my="241">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632341380293338968" />
        <item text="OnMakeCall_Failed" treenode="632341380293338970" />
        <item text="OnRemoteHangup" treenode="632467498335206451" />
      </items>
      <linkto id="632341440574919679" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632471036673718782" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_theDestinationNumber</ap>
        <ap name="From" type="variable">g_origNumber</ap>
        <ap name="MmsId" type="variable">g_mmsId</ap>
        <ap name="ProxyDTMFCallId" type="variable">g_hostCallId</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_confId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_calleeCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632471036673718782" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="760.75" y="390">
      <linkto id="632471100140789668" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
      </Properties>
    </node>
    <node type="Action" id="632471100140789668" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="760.75" y="480">
      <linkto id="632130617186142177" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute()
{
  System.Threading.Thread.Sleep(500);
  return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HandleLogin" startnode="632129930790358508" treenode="632341380293339083" appnode="632129930790358507" handlerfor="632471036673718774">
    <node type="Start" id="632129930790358508" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="56">
      <linkto id="632344914725624479" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632129965743418574" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="264" y="336" mx="317" my="352">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919680" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">error_internal.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="UserData" type="literal">fatal_error</ap>
      </Properties>
    </node>
    <node type="Action" id="632129965743418578" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="752" y="704" mx="805" my="720">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919686" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">error_num_sessions.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="UserData" type="literal">fatal_error</ap>
      </Properties>
    </node>
    <node type="Action" id="632129965743418581" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="848" y="128" mx="901" my="144">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919687" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">error_quota.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="UserData" type="literal">fatal_error</ap>
      </Properties>
    </node>
    <node type="Action" id="632129965743418584" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="848" y="264" mx="901" my="280">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919688" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">error_acct_locked.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="UserData" type="literal">fatal_error</ap>
      </Properties>
    </node>
    <node type="Action" id="632129965743418587" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1008" y="544" mx="1061" my="560">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919685" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">enter_acct.wav</ap>
        <ap name="Prompt1" type="literal">error_login.wav</ap>
        <ap name="Prompt3" type="literal">pound_sign.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_acct</ap>
      </Properties>
    </node>
    <node type="Comment" id="632129965743418602" text="Too many concurrent logins" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="824" y="672" />
    <node type="Comment" id="632129965743418603" text="Not enough quota remaining" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="919" y="99" />
    <node type="Comment" id="632129965743418604" text="Locked out due to multiple failed login attempts" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="912" y="240" />
    <node type="Comment" id="632129965743418605" text="Bad Account Code or PIN" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="912" y="416" />
    <node type="Comment" id="632129965743418606" text="Successful login" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="576" y="728" />
    <node type="Action" id="632130427597727232" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="488" y="728" mx="548" my="744">
      <items count="1">
        <item text="HandleLoginSuccess" />
      </items>
      <linkto id="632341440574919683" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">HandleLoginSuccess</ap>
      </Properties>
    </node>
    <node type="Action" id="632130596839485065" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="264" y="624" mx="317" my="640">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919682" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">change_pin_notice.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">change_pin_notice</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Requesting PIN change from account " + g_accountCode</log>
      </Properties>
    </node>
    <node type="Comment" id="632130596839485067" text="Prompt for new PIN" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="304" y="600" />
    <node type="Action" id="632152926948472027" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1016" y="432" mx="1069" my="448">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919684" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">too_many_failed_logins.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="UserData" type="literal">fatal_error</ap>
      </Properties>
    </node>
    <node type="Action" id="632341383106383957" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="544" y="536">
      <linkto id="632344780282204350" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_loggedIn</rd>
      </Properties>
    </node>
    <node type="Action" id="632341383106383959" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="920" y="464">
      <linkto id="632341383106383960" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref int g_numFailedLoginAttempts)
	{
		g_numFailedLoginAttempts++;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632341383106383960" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="920" y="560">
      <linkto id="632152926948472027" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632129965743418587" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_numFailedLoginAttempts &gt;= g_numFailedLoginAttemptsAllowed</ap>
      </Properties>
    </node>
    <node type="Action" id="632341440574919680" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="232" y="352">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Label" id="632341440574919681" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="152" y="352">
      <linkto id="632341440574919680" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632341440574919682" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="184" y="640" />
    <node type="Label" id="632341440574919683" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="552" y="872" />
    <node type="Label" id="632341440574919684" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1256" y="448" />
    <node type="Label" id="632341440574919685" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1256" y="560" />
    <node type="Label" id="632341440574919686" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="984" y="720" />
    <node type="Label" id="632341440574919687" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1056" y="144" />
    <node type="Label" id="632341440574919688" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1056" y="280" />
    <node type="Action" id="632344768095180254" name="PhoneLogin" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="360" y="224">
      <linkto id="632129965743418574" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632129965743418581" type="Labeled" style="Bezier" ortho="true" label="QuotaExceeded" />
      <linkto id="632129965743418584" type="Labeled" style="Bezier" ortho="true" label="NotAllowedDueLocked" />
      <linkto id="632341383106383959" type="Labeled" style="Bezier" label="InvalidAccountCodeOrPin" />
      <linkto id="632344780282204353" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632341383106383959" type="Labeled" style="Bezier" label="NotAllowedDueToDeleted" />
      <linkto id="632341383106383959" type="Labeled" style="Bezier" label="NotAllowedDueToDisabled" />
      <Properties final="false" type="native" log="On">
        <ap name="AccountCode" type="variable">g_accountCode</ap>
        <ap name="Pin" type="variable">pin</ap>
        <ap name="UserPhoneNumber" type="variable">g_origNumber</ap>
        <rd field="UserId">g_loggedInUserId</rd>
        <rd field="AuthenticationRecordId">g_authRecordsId</rd>
        <rd field="PinChangeRequired">pinChangeRequired</rd>
      </Properties>
    </node>
    <node type="Action" id="632344780282204350" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="544" y="640">
      <linkto id="632130596839485065" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632130427597727232" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">pinChangeRequired</ap>
      </Properties>
    </node>
    <node type="Comment" id="632344780282204351" text="g_loggedIn = true" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="416" y="520" />
    <node type="Action" id="632344780282204353" name="WriteSessionStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="544" y="432">
      <linkto id="632341383106383957" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632129965743418578" type="Labeled" style="Bezier" ortho="true" label="TooManyConcurrentLogins" />
      <Properties final="false" type="native" log="On">
        <ap name="AuthRecordsId" type="variable">g_authRecordsId</ap>
        <rd field="SessionRecordsId">g_sessionRecordsId</rd>
      </Properties>
    </node>
    <node type="Action" id="632344914725624479" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="200" y="57">
      <linkto id="632345089973518577" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">pin.Replace("#", String.Empty)</ap>
        <ap name="Value2" type="csharp">g_accountCode.Replace("#", String.Empty)</ap>
        <rd field="ResultData">pin</rd>
        <rd field="ResultData2">g_accountCode</rd>
      </Properties>
    </node>
    <node type="Action" id="632345089973518577" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="56" y="136">
      <linkto id="632345089973518578" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632345089973518580" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">pin</ap>
        <ap name="Value2" type="csharp">String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632345089973518578" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="168" y="216">
      <linkto id="632345089973518580" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">0</ap>
        <rd field="ResultData">pin</rd>
      </Properties>
    </node>
    <node type="Action" id="632345089973518579" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="440" y="136">
      <linkto id="632344768095180254" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">0</ap>
        <rd field="ResultData">g_accountCode</rd>
      </Properties>
    </node>
    <node type="Action" id="632345089973518580" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="280" y="136">
      <linkto id="632345089973518579" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632344768095180254" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_accountCode</ap>
        <ap name="Value2" type="csharp">String.Empty</ap>
      </Properties>
    </node>
    <node type="Variable" id="632129930790358511" name="pin" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="pin" defaultInitWith="0" refType="reference">pin</Properties>
    </node>
    <node type="Variable" id="632344780282204335" name="pinChangeRequired" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">pinChangeRequired</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HandleGatherDigitsTimeout" startnode="632133259292691489" treenode="632341380293339087" appnode="632133259292691488" handlerfor="632471036673718774">
    <node type="Start" id="632133259292691489" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="49" y="209">
      <linkto id="632133259292691494" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632133259292691494" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="129" y="209">
      <linkto id="632133259292691495" type="Labeled" style="Bezier" ortho="true" label="maxtime" />
      <linkto id="632341440574919660" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">terminationCondition</ap>
      </Properties>
    </node>
    <node type="Action" id="632133259292691495" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="271" y="209">
      <linkto id="632133259292691518" type="Labeled" style="Bezier" ortho="true" label="enter_dn" />
      <linkto id="632133259292691515" type="Labeled" style="Bezier" ortho="true" label="enter_matter_code" />
      <linkto id="632133259292691512" type="Labeled" style="Bezier" ortho="true" label="enter_client_code" />
      <linkto id="632133259292691509" type="Labeled" style="Bezier" ortho="true" label="enter_proj_code" />
      <linkto id="632133259292691506" type="Labeled" style="Bezier" ortho="true" label="change_pin_notice" />
      <linkto id="632133259292691503" type="Labeled" style="Bezier" ortho="true" label="enter_pin" />
      <linkto id="632133259292691500" type="Labeled" style="Bezier" ortho="true" label="enter_acct" />
      <linkto id="632341440574919660" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">state</ap>
      </Properties>
    </node>
    <node type="Action" id="632133259292691500" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="461" y="16" mx="514" my="32">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919652" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">enter_acct.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_acct</ap>
      </Properties>
    </node>
    <node type="Action" id="632133259292691503" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="456" y="120" mx="509" my="136">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919653" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">enter_pin.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_pin</ap>
      </Properties>
    </node>
    <node type="Action" id="632133259292691506" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="457" y="215" mx="510" my="231">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919654" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">change_pin_notice.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">change_pin_notice</ap>
      </Properties>
    </node>
    <node type="Action" id="632133259292691509" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="448" y="314" mx="501" my="330">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919655" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">enter_proj_code.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_proj_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632133259292691512" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="452" y="416" mx="505" my="432">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919656" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">enter_client_code.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_client_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632133259292691515" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="229" y="415" mx="282" my="431">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919657" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">enter_matter_code.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_matter_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632133259292691518" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="34" y="415" mx="87" my="431">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919658" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">enter_dn.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxTime" type="literal">10000</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_dn</ap>
      </Properties>
    </node>
    <node type="Comment" id="632341440574919649" text="Return false" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="171" y="32" />
    <node type="Label" id="632341440574919650" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="503" y="635">
      <linkto id="632341440574919651" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632341440574919651" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="578" y="635">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">success</ap>
      </Properties>
    </node>
    <node type="Label" id="632341440574919652" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="721" y="33" />
    <node type="Label" id="632341440574919653" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="717" y="135" />
    <node type="Label" id="632341440574919654" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="717" y="231" />
    <node type="Label" id="632341440574919655" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="719" y="330" />
    <node type="Label" id="632341440574919656" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="710" y="433" />
    <node type="Label" id="632341440574919657" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="279" y="566" />
    <node type="Label" id="632341440574919658" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="84" y="560" />
    <node type="Comment" id="632341440574919659" text="Return success" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="504" y="586" />
    <node type="Action" id="632341440574919660" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="208" y="83">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">false</ap>
      </Properties>
    </node>
    <node type="Variable" id="632133259292691492" name="terminationCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="terminationCondition" refType="reference">terminationCondition</Properties>
    </node>
    <node type="Variable" id="632133259292691493" name="state" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="state" refType="reference">state</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HandleRingback" startnode="632132376626152586" treenode="632341380293339088" appnode="632132376626152585" handlerfor="632471036673718774">
    <node type="Start" id="632132376626152586" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="56" y="136">
      <linkto id="632152926948470175" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632152926948470170" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="454" y="56" mx="528" my="72">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632341380293339062" />
        <item text="OnGatherDigits_Failed" treenode="632341380293339071" />
      </items>
      <linkto id="632341383106383961" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondDigitPattern" type="literal">###</ap>
        <ap name="UserData" type="literal">user_end_call</ap>
        <log condition="entry" on="true" level="Info" type="literal">Receive Digit Pattern of ### Initiated ***********</log>
      </Properties>
    </node>
    <node type="Comment" id="632152926948470172" text="Start looking for '###' which ends the call." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="438" y="32" />
    <node type="Action" id="632152926948470175" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="231" y="136">
      <linkto id="632152926948470170" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <linkto id="632345638121320672" type="Labeled" style="Bezier" ortho="true" label="userstop" />
      <linkto id="632345638121320674" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">terminationCondition</ap>
      </Properties>
    </node>
    <node type="Action" id="632341383106383961" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="678" y="72">
      <linkto id="632345638121320678" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <rd field="ResultData">g_GatherDigitsEnabled</rd>
      </Properties>
    </node>
    <node type="Action" id="632345638121320672" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="231" y="232">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Label" id="632345638121320673" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="231" y="339">
      <linkto id="632345638121320672" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632345638121320674" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="446" y="184" mx="499" my="200">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632345638121320677" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">beep.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">ringback</ap>
      </Properties>
    </node>
    <node type="Label" id="632345638121320677" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="670" y="200" />
    <node type="Label" id="632345638121320678" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="782" y="72" />
    <node type="Variable" id="632132376626152588" name="terminationCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="terminationCondition" refType="reference">terminationCondition</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HandleUserEndCall" startnode="632155553466580694" treenode="632341380293339089" appnode="632155553466580693" handlerfor="632471036673718774">
    <node type="Start" id="632155553466580694" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="45" y="133">
      <linkto id="632155553466580699" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632155553466580699" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="128" y="134">
      <linkto id="632341440574919645" type="Labeled" style="Bezier" ortho="true" label="userstop" />
      <linkto id="632467553614951521" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">terminationCondition</ap>
        <log condition="entry" on="true" level="Warning" type="csharp">"OnUserEndCall: TermCond = " + terminationCondition</log>
      </Properties>
    </node>
    <node type="Comment" id="632155553466580716" text="The application caused the digits to stop being received,&#xD;&#xA;so don't do anything." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="150" y="257" />
    <node type="Comment" id="632155553466580715" text="The user requested that the&#xD;&#xA;call be terminated." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="179" y="62" />
    <node type="Action" id="632341440574919644" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="513" y="134">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632341440574919645" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="129" y="323">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632467553614951521" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="260.4707" y="134">
      <linkto id="632467553614951522" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_calleeCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632467553614951522" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="328.6087" y="118" mx="383" my="134">
      <items count="1">
        <item text="OnRemoteHangup" treenode="632467498335206451" />
      </items>
      <linkto id="632341440574919644" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="callId" type="variable">g_calleeCallId</ap>
        <ap name="EndReason" type="literal">Normal</ap>
        <ap name="FunctionName" type="literal">OnRemoteHangup</ap>
      </Properties>
    </node>
    <node type="Variable" id="632155553466580709" name="terminationCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="terminationCondition" refType="reference">terminationCondition</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotDigits" startnode="632131420292420902" treenode="632341380293339093" appnode="632131420292420901" handlerfor="632131420292420900">
    <node type="Start" id="632131420292420902" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="82" y="126">
      <linkto id="632341440574919639" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632341440574919639" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="208" y="126">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632467578293212868" text="Telephony Manager handles digit hairpinning,&#xD;&#xA;so there's nothing we have to do" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="114" y="68" />
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632467498335206450" treenode="632467498335206451" appnode="632467498335206448" handlerfor="632467498335206447">
    <node type="Start" id="632467498335206450" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="72">
      <linkto id="632467498335206467" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632467498335206467" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="112.998169" y="136">
      <linkto id="632467498335206481" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632467498335206482" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">g_cdrOpen</ap>
      </Properties>
    </node>
    <node type="Action" id="632467498335206468" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="383" y="305">
      <linkto id="632467498335206489" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632467498335206475" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">g_loggedIn</ap>
      </Properties>
    </node>
    <node type="Comment" id="632467498335206469" text="The host hung up. Close&#xD;&#xA;out the session." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="239" y="433" />
    <node type="Comment" id="632467498335206470" text="Call completed: &#xD;&#xA;Tear down the conference and prompt host for another number to dial" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="370.000732" y="73" />
    <node type="Comment" id="632467498335206471" text="Host or callee hung up" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="57.99817" y="473" />
    <node type="Action" id="632467498335206472" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="561" y="134" mx="614" my="150">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632467498335206483" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">enter_dn.wav</ap>
        <ap name="Prompt1" type="literal">call_end.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_dn</ap>
      </Properties>
    </node>
    <node type="Comment" id="632467498335206473" text="The callee hung up." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="320.999268" y="32" />
    <node type="Action" id="632467498335206475" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="567" y="393">
      <linkto id="632467498335206486" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632467498335206523" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">g_calleeCallId == String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632467498335206476" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="289" y="150">
      <linkto id="632467498335206488" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632485594227550376" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">g_inCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632467498335206481" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="113" y="270">
      <linkto id="632467498335206491" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="variable">endReason</ap>
        <rd field="ResultData">g_cdrOpen</rd>
        <rd field="ResultData2">g_EndReason</rd>
      </Properties>
    </node>
    <node type="Action" id="632467498335206482" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="289" y="305">
      <linkto id="632467498335206468" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632467498335206476" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">callId == g_hostCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632467498335206483" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="727.0006" y="150">
      <linkto id="632467498335206487" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="csharp">String.Empty</ap>
        <rd field="ResultData">g_inCall</rd>
        <rd field="ResultData2">g_calleeConnId</rd>
      </Properties>
    </node>
    <node type="Action" id="632467498335206484" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="383" y="505">
      <linkto id="632467498335206475" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_loggedIn</rd>
      </Properties>
    </node>
    <node type="Action" id="632467498335206485" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="567" y="583">
      <linkto id="632467498335206486" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">String.Empty</ap>
        <rd field="ResultData">g_calleeCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632467498335206486" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="701.5679" y="485">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632467498335206487" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="859.5679" y="150">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632467498335206488" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="289.567871" y="43">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632467498335206489" name="WriteSessionStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="383" y="393">
      <linkto id="632467498335206484" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SessionRecordsId" type="variable">g_sessionRecordsId</ap>
      </Properties>
    </node>
    <node type="Comment" id="632467498335206490" text="g_loggedIn = false" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="343" y="537" />
    <node type="Action" id="632467498335206491" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="113" y="433">
      <linkto id="632467498335206482" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="variable">g_EndReason</ap>
      </Properties>
    </node>
    <node type="Action" id="632467498335206523" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="567" y="486">
      <linkto id="632467498335206485" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_calleeCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632485594227550376" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="384.998169" y="150">
      <linkto id="632485594227550377" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
      </Properties>
    </node>
    <node type="Action" id="632485594227550377" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="507.90625" y="150">
      <linkto id="632467498335206472" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute()
{
	System.Threading.Thread.Sleep(400);
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Variable" id="632470098332909979" name="endReason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="EndReason" refType="reference">endReason</Properties>
    </node>
    <node type="Variable" id="632470130464741383" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632128930879012604" treenode="632341380293339017" appnode="632128930879012603" handlerfor="632128930879012602">
    <node type="Start" id="632128930879012604" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="54" y="125">
      <linkto id="632341440574919635" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632341440574919635" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="169" y="125">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632129680802394337" treenode="632341380293339071" appnode="632129680802394336" handlerfor="632129680802394335">
    <node type="Start" id="632129680802394337" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="168" y="186">
      <linkto id="632341440574919636" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632341440574919636" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="310" y="186">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HandleChangePIN" startnode="632130596839485076" treenode="632341380293339080" appnode="632130596839485075" handlerfor="632471036673718774">
    <node type="Start" id="632130596839485076" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="104">
      <linkto id="632345739338163352" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632130617186142139" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="320" y="220" mx="373" my="236">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919595" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">error_internal.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="UserData" type="literal">fatal_error</ap>
      </Properties>
    </node>
    <node type="Action" id="632130617186142142" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="705" y="87" mx="758" my="103">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919596" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">enter_pin.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_pin</ap>
      </Properties>
    </node>
    <node type="Action" id="632341440574919595" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="369" y="369">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632341440574919596" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="961" y="103">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632344780282204358" name="WriteSessionStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="537" y="103">
      <linkto id="632344780282204360" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SessionRecordsId" type="variable">g_sessionRecordsId</ap>
      </Properties>
    </node>
    <node type="Action" id="632344780282204360" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="657" y="103">
      <linkto id="632130617186142142" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_loggedIn</rd>
      </Properties>
    </node>
    <node type="Comment" id="632344780282204361" text="g_loggedIn = false" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="617" y="63" />
    <node type="Comment" id="632344780282204362" text="Close session to force user&#xD;&#xA;to log in agian with new pin." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="473" y="143" />
    <node type="Action" id="632344780282204363" name="ChangePinCode" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="369" y="103">
      <linkto id="632130617186142139" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632344780282204358" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632345739338163357" type="Labeled" style="Bezier" ortho="true" label="InvalidPin" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_loggedInUserId</ap>
        <ap name="Pin" type="variable">newPin</ap>
      </Properties>
    </node>
    <node type="Action" id="632345739338163352" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="119" y="104">
      <linkto id="632345739338163353" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">newPin.Replace("#", String.Empty)</ap>
        <rd field="ResultData">newPin</rd>
      </Properties>
    </node>
    <node type="Action" id="632345739338163353" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="224" y="104">
      <linkto id="632344780282204363" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632345739338163356" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">newPin == String.Empty</ap>
      </Properties>
    </node>
    <node type="Comment" id="632345739338163354" text="newPin == String.Empty" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="167" y="61" />
    <node type="Label" id="632345739338163355" text="I" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="536" y="424">
      <linkto id="632345739338163357" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632345739338163356" text="I" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="224" y="200" />
    <node type="Action" id="632345739338163357" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="608" y="408" mx="661" my="424">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632345739338163360" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">enter_pin.wav</ap>
        <ap name="Prompt1" type="literal">error_invalid_number.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">change_pin_notice</ap>
      </Properties>
    </node>
    <node type="Action" id="632345739338163360" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="877.4707" y="425">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Variable" id="632130596839485078" name="newPin" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="pin" refType="reference">newPin</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HandleClientCode" startnode="632130488620974412" treenode="632341380293339081" appnode="632130488620974411" handlerfor="632471036673718774">
    <node type="Start" id="632130488620974412" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="92">
      <linkto id="632344871810615736" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632130488620974461" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="336" y="272" mx="389" my="288">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919672" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">error_internal.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="UserData" type="literal">fatal_error</ap>
      </Properties>
    </node>
    <node type="Action" id="632130488620974464" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="574" y="276" mx="627" my="292">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919671" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">enter_client_code.wav</ap>
        <ap name="Prompt1" type="literal">error_client_code.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_client_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632130488620974467" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="520" y="80" mx="573" my="96">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919669" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">enter_matter_code.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_matter_code</ap>
      </Properties>
    </node>
    <node type="Comment" id="632130488620974468" text="Prompt for matter code" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="544" y="48" />
    <node type="Comment" id="632130488620974469" text="Invalid code" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="688" y="264" />
    <node type="Comment" id="632130488620974470" text="Database error" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="448" y="264" />
    <node type="Action" id="632155553466581347" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="93" y="194" mx="146" my="210">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919670" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">enter_dn.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_dn</ap>
      </Properties>
    </node>
    <node type="Comment" id="632155553466581349" text="The user entered '#' to bypass &#xD;&#xA;the code entry. Just prompt for&#xD;&#xA;the DN now." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="171" y="188" />
    <node type="Label" id="632341440574919667" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="64" y="408">
      <linkto id="632341440574919668" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632341440574919668" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="144" y="408">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632341440574919669" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="682" y="96" />
    <node type="Label" id="632341440574919670" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="142" y="349" />
    <node type="Label" id="632341440574919671" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="627" y="419" />
    <node type="Label" id="632341440574919672" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="386" y="418" />
    <node type="Action" id="632344871810615736" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="143" y="94">
      <linkto id="632155553466581347" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632344871810615737" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_clientCode</ap>
        <ap name="Value2" type="literal">#</ap>
      </Properties>
    </node>
    <node type="Action" id="632344871810615737" name="ValidateClientMatterCode" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="312" y="93">
      <linkto id="632130488620974461" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632130488620974464" type="Labeled" style="Bezier" ortho="true" label="InvalidCode" />
      <linkto id="632130488620974467" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="ClientCode" type="variable">g_clientCode</ap>
        <ap name="MatterCode" type="variable">g_matterCode</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HandleLoginSuccess" startnode="632130427597727234" treenode="632341380293339084" appnode="632130427597727233" handlerfor="632471036673718774">
    <node type="Start" id="632130427597727234" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="91">
      <linkto id="632130427597727236" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632130427597727236" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="189" y="91">
      <linkto id="632130427597727237" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632130427597727240" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">g_useProjCodes</ap>
      </Properties>
    </node>
    <node type="Action" id="632130427597727237" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="333" y="91">
      <linkto id="632130427597727243" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632130488620974445" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">g_useClientCodes</ap>
      </Properties>
    </node>
    <node type="Action" id="632130427597727240" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="138.999786" y="229" mx="192" my="245">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919691" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">enter_proj_code.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_proj_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632130427597727243" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="285" y="228" mx="338" my="244">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919690" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">enter_client_code.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_client_code</ap>
      </Properties>
    </node>
    <node type="Comment" id="632130488620974441" text="Prompt for project code" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32.9998779" y="230" />
    <node type="Comment" id="632130488620974442" text="Prompt for client code" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="369" y="246" />
    <node type="Action" id="632130488620974445" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="424" y="75" mx="477" my="91">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919689" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">enter_dn.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_dn</ap>
      </Properties>
    </node>
    <node type="Comment" id="632130488620974449" text="Prompt for number to dial" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="418.956421" y="47" />
    <node type="Action" id="632341440574919689" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="625" y="91">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632341440574919690" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="336" y="385">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632341440574919691" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="189" y="394">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HandleMatterCode" startnode="632130488620974408" treenode="632341380293339085" appnode="632130488620974407" handlerfor="632471036673718774">
    <node type="Start" id="632130488620974408" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="104">
      <linkto id="632344871810615734" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632130488620974478" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="328" y="232" mx="381" my="248">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919693" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">enter_matter_code.wav</ap>
        <ap name="Prompt1" type="literal">error_matter_code.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_matter_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632130488620974481" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="424" y="88" mx="477" my="104">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919692" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">enter_dn.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_dn</ap>
      </Properties>
    </node>
    <node type="Action" id="632130488620974484" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="64" y="232" mx="117" my="248">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919694" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">error_internal.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="UserData" type="literal">fatal_error</ap>
      </Properties>
    </node>
    <node type="Comment" id="632130488620974485" text="Prompt for number to dial" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="436.283142" y="52" />
    <node type="Comment" id="632130488620974486" text="Invalid code" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="418" y="236" />
    <node type="Comment" id="632130488620974487" text="Database error" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="176" y="224" />
    <node type="Action" id="632341440574919692" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="664" y="104">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632341440574919693" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="379" y="383">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632341440574919694" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="114" y="384">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632344871810615734" name="ValidateClientMatterCode" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="152" y="104">
      <linkto id="632130488620974484" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632130488620974478" type="Labeled" style="Bezier" ortho="true" label="InvalidCode" />
      <linkto id="632130488620974481" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="ClientCode" type="variable">g_clientCode</ap>
        <ap name="MatterCode" type="variable">g_matterCode</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HandleProjCode" startnode="632130488620974399" treenode="632341380293339086" appnode="632130488620974398" handlerfor="632471036673718774">
    <node type="Start" id="632130488620974399" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="120">
      <linkto id="632344870152831957" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632130488620974419" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="40" y="207" mx="93" my="223">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919666" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">error_internal.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="UserData" type="literal">fatal_error</ap>
      </Properties>
    </node>
    <node type="Action" id="632130488620974421" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="431" y="119">
      <linkto id="632130488620974431" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632130488620974435" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">g_useClientCodes</ap>
      </Properties>
    </node>
    <node type="Action" id="632130488620974428" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="241" y="210" mx="294" my="226">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919665" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">error_invalid_number.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_proj_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632130488620974431" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="492.044434" y="100" mx="545" my="116">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919663" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">enter_client_code.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_client_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632130488620974435" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="381" y="232" mx="434" my="248">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341380293338972" />
        <item text="OnPlay_Failed" treenode="632341380293339017" />
      </items>
      <linkto id="632341440574919664" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">enter_dn.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">enter_dn</ap>
      </Properties>
    </node>
    <node type="Comment" id="632130488620974437" text="Prompt for client code, if configured" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="438" y="73" />
    <node type="Comment" id="632130488620974438" text="Prompt for number to dial" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="457" y="243" />
    <node type="Comment" id="632130488620974439" text="Database error" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="122" y="213" />
    <node type="Comment" id="632130488620974440" text="Invalid code" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="271" y="184" />
    <node type="Action" id="632341440574919661" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="223" y="445">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Label" id="632341440574919662" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="127" y="444">
      <linkto id="632341440574919661" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632341440574919663" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="676" y="115" />
    <node type="Label" id="632341440574919664" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="431" y="373" />
    <node type="Label" id="632341440574919665" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="289" y="359" />
    <node type="Label" id="632341440574919666" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="90" y="359" />
    <node type="Action" id="632344870152831957" name="ValidateProjectCode" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="131" y="119">
      <linkto id="632130488620974419" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632130488620974428" type="Labeled" style="Bezier" ortho="true" label="InvalidCode" />
      <linkto id="632130488620974421" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="ProjectCode" type="variable">g_projCode</ap>
      </Properties>
    </node>
  </canvas>
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>