<Application name="FAC" trigger="Metreos.CallControl.IncomingCall" version="0.7" single="false">
  <!--//// app tree ////-->
  <global name="FAC">
    <outline>
      <treenode type="evh" id="632285030923669435" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632285030923669432" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632285030923669431" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
          <ep name="to" type="variable">g_appServerDn</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632285030923669446" level="2" text="AnswerCall_Complete: OnAnswerCall_Complete">
        <node type="function" name="OnAnswerCall_Complete" id="632285030923669443" path="Metreos.StockTools" />
        <node type="event" name="AnswerCall_Complete" id="632285030923669442" path="Metreos.CallControl.AnswerCall_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632285030923669471" level="3" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632285030923669468" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632285030923669467" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632285030923669485" level="4" text="ReceiveDigits_Complete: OnReceiveDigits_Complete">
        <node type="function" name="OnReceiveDigits_Complete" id="632285030923669482" path="Metreos.StockTools" />
        <node type="event" name="ReceiveDigits_Complete" id="632285030923669481" path="Metreos.Providers.MediaServer.ReceiveDigits_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632285030923669497" level="5" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632285030923669468" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632285030923669467" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632285030923669498" level="5" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632285030923669473" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632285030923669472" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632285030923669509" level="5" text="MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632285030923669506" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632285030923669505" path="Metreos.CallControl.MakeCall_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370288" level="6" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632285030923669468" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632285030923669467" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370289" level="6" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632285030923669473" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632285030923669472" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370291" level="6" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632285030923669468" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632285030923669467" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370292" level="6" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632285030923669473" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632285030923669472" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632285030923669514" level="5" text="MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632285030923669511" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632285030923669510" path="Metreos.CallControl.MakeCall_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297293797464137" level="6" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632285030923669468" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632285030923669467" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297293797464138" level="6" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632285030923669473" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632285030923669472" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297293797464140" level="6" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632297204531370255" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632297204531370254" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297293797464141" level="6" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632297204531370260" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632297204531370259" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632285030923669515" level="5" text="Hangup: OnHangup">
        <node type="function" name="OnHangup" id="632285030923669453" path="Metreos.StockTools" />
        <node type="event" name="Hangup" id="632285030923669452" path="Metreos.CallControl.Hangup" />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370267" level="5" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632297204531370255" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632297204531370254" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370268" level="5" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632297204531370260" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632297204531370259" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370274" level="5" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632285030923669468" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632285030923669467" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370451" level="6" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632297204531370255" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632297204531370254" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370452" level="6" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632297204531370260" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632297204531370259" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370460" level="6" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632297204531370255" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632297204531370254" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370461" level="6" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632297204531370260" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632297204531370259" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370474" level="6" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632285030923669468" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632285030923669467" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370475" level="7" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632285030923669473" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632285030923669472" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370275" level="5" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632285030923669473" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632285030923669472" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370277" level="5" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632297204531370255" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632297204531370254" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370278" level="5" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632297204531370260" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632297204531370259" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632285030923669490" level="4" text="ReceiveDigits_Failed: OnReceiveDigits_Failed">
        <node type="function" name="OnReceiveDigits_Failed" id="632285030923669487" path="Metreos.StockTools" />
        <node type="event" name="ReceiveDigits_Failed" id="632285030923669486" path="Metreos.Providers.MediaServer.ReceiveDigits_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297293797464125" level="5" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632285030923669468" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632285030923669467" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297293797464126" level="5" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632285030923669473" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632285030923669472" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297293797464129" level="5" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632297204531370255" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632297204531370254" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297293797464130" level="5" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632297204531370260" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632297204531370259" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632285030923669476" level="3" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632285030923669473" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632285030923669472" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297293797464673" level="4" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632297204531370255" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632297204531370254" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297293797464674" level="4" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632297204531370260" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632297204531370259" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297293797464677" level="4" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632297204531370255" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632297204531370254" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297293797464678" level="4" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632297204531370260" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632297204531370259" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632285030923669451" level="2" text="AnswerCall_Failed: OnAnswerCall_Failed">
        <node type="function" name="OnAnswerCall_Failed" id="632285030923669448" path="Metreos.StockTools" />
        <node type="event" name="AnswerCall_Failed" id="632285030923669447" path="Metreos.CallControl.AnswerCall_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632285030923669456" level="2" text="Hangup: OnHangup">
        <node type="function" name="OnHangup" id="632285030923669453" path="Metreos.StockTools" />
        <node type="event" name="Hangup" id="632285030923669452" path="Metreos.CallControl.Hangup" />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370429" level="2" text="AnswerCall_Complete: OnAnswerCall_Complete">
        <node type="function" name="OnAnswerCall_Complete" id="632285030923669443" path="Metreos.StockTools" />
        <node type="event" name="AnswerCall_Complete" id="632285030923669442" path="Metreos.CallControl.AnswerCall_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370437" level="3" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632297204531370255" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632297204531370254" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370438" level="3" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632297204531370260" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632297204531370259" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370430" level="2" text="AnswerCall_Failed: OnAnswerCall_Failed">
        <node type="function" name="OnAnswerCall_Failed" id="632285030923669448" path="Metreos.StockTools" />
        <node type="event" name="AnswerCall_Failed" id="632285030923669447" path="Metreos.CallControl.AnswerCall_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297204531370431" level="2" text="Hangup: OnHangup">
        <node type="function" name="OnHangup" id="632285030923669453" path="Metreos.StockTools" />
        <node type="event" name="Hangup" id="632285030923669452" path="Metreos.CallControl.Hangup" />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297293797464090" level="3" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632297204531370255" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632297204531370254" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297293797464091" level="3" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632297204531370260" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632297204531370259" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297293797464093" level="3" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632297204531370255" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632297204531370254" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632297293797464094" level="3" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632297204531370260" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632297204531370259" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_connID" id="632302536409689813" vid="632285030923669437">
        <Properties type="Int">g_connID</Properties>
      </treenode>
      <treenode text="g_dialedNumber" id="632302536409689815" vid="632285030923669500">
        <Properties type="String">g_dialedNumber</Properties>
      </treenode>
      <treenode text="g_connID2" id="632302536409689817" vid="632285030923669520">
        <Properties type="String">g_connID2</Properties>
      </treenode>
      <treenode text="g_conferenceID" id="632302536409689819" vid="632285030923669528">
        <Properties type="String">g_conferenceID</Properties>
      </treenode>
      <treenode text="g_PerformHangup" id="632302536409689821" vid="632297204531370441">
        <Properties type="Bool" defaultInitWith="false">g_PerformHangup</Properties>
      </treenode>
      <treenode text="g_UserToHangup" id="632302536409689823" vid="632297204531370449">
        <Properties type="UInt" defaultInitWith="0">g_UserToHangup</Properties>
      </treenode>
      <treenode text="g_CallId1" id="632302536409689825" vid="632297204531370463">
        <Properties type="String">g_CallId1</Properties>
      </treenode>
      <treenode text="g_CallId2" id="632302536409689827" vid="632297204531370465">
        <Properties type="String">g_CallId2</Properties>
      </treenode>
      <treenode text="g_ReceiveDigits" id="632302536409689829" vid="632297204531370469">
        <Properties type="Bool" defaultInitWith="false">g_ReceiveDigits</Properties>
      </treenode>
      <treenode text="g_IsDisc1" id="632302536409689831" vid="632297293797464074">
        <Properties type="Bool" defaultInitWith="true">g_IsDisc1</Properties>
      </treenode>
      <treenode text="g_IsDisc2" id="632302536409689833" vid="632297293797464076">
        <Properties type="Bool" defaultInitWith="true">g_IsDisc2</Properties>
      </treenode>
      <treenode text="g_ConferenceCreated" id="632302536409689835" vid="632297293797464120">
        <Properties type="Bool" defaultInitWith="false">g_ConferenceCreated</Properties>
      </treenode>
      <treenode text="g_authCode" id="632302536409689837" vid="632297293797464636">
        <Properties type="String" initWith="authCode">g_authCode</Properties>
      </treenode>
      <treenode text="g_callerIdentification" id="632302536409689839" vid="632297293797464638">
        <Properties type="String">g_callerIdentification</Properties>
      </treenode>
      <treenode text="g_ExitApp" id="632302536409689841" vid="632297293797464650">
        <Properties type="Bool" defaultInitWith="false">g_ExitApp</Properties>
      </treenode>
      <treenode text="g_callManagerIp" id="632302536409689843" vid="632298258783491993">
        <Properties type="String" initWith="callManagerIp">g_callManagerIp</Properties>
      </treenode>
      <treenode text="g_appServerDn" id="632302536409690006" vid="632302536409690005">
        <Properties type="String" initWith="appServerDn">g_appServerDn</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632285030923669434" treenode="632285030923669435" appnode="632285030923669432" handlerfor="632285030923669431">
    <node type="Start" id="632285030923669434" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="218">
      <linkto id="632285030923669504" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632285030923669436" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="292" y="219">
      <linkto id="632285030923669441" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632297204531370432" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">callID</ap>
        <ap name="remotePort" type="literal">0</ap>
        <ap name="connectionId" type="literal">0</ap>
        <ap name="remoteIp" type="literal">0</ap>
        <rd field="connectionId">g_connID</rd>
        <rd field="port">port</rd>
        <rd field="ipAddress">ipAddress</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: Creating MMS Connection...</log>
      </Properties>
    </node>
    <node type="Action" id="632285030923669441" name="SetMedia" class="MaxActionNode" group="" path="Metreos.CallControl" x="438" y="220">
      <linkto id="632285030923669457" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632297204531370433" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">callID</ap>
        <ap name="mediaPort" type="variable">port</ap>
        <ap name="mediaIP" type="variable">ipAddress</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: Setting Media...</log>
      </Properties>
    </node>
    <node type="Action" id="632285030923669457" name="AnswerCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="507" y="204" mx="578" my="220">
      <items count="3">
        <item text="OnAnswerCall_Complete" treenode="632285030923669446" />
        <item text="OnAnswerCall_Failed" treenode="632285030923669451" />
        <item text="OnHangup" treenode="632285030923669456" />
      </items>
      <linkto id="632285030923669459" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632297204531370434" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="answer" type="literal">true</ap>
        <ap name="callId" type="variable">callID</ap>
        <ap name="userData" type="literal">none</ap>
        <rd field="callId">g_CallId1</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: Answering call</log>
      </Properties>
    </node>
    <node type="Action" id="632285030923669459" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="730" y="220">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632285030923669504" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="169" y="219">
      <linkto id="632285030923669436" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref string g_dialedNumber, string dialedNumber, string from, ref string g_callerIdentification)
	{
		g_dialedNumber = dialedNumber;
		g_callerIdentification = from;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632297204531370432" name="AnswerCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="224" y="346" mx="295" my="362">
      <items count="3">
        <item text="OnAnswerCall_Complete" treenode="632297204531370429" />
        <item text="OnAnswerCall_Failed" treenode="632297204531370430" />
        <item text="OnHangup" treenode="632297204531370431" />
      </items>
      <linkto id="632297204531370435" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="answer" type="literal">false</ap>
        <ap name="callId" type="variable">callID</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: Fail... dropping incoming call</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370433" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="437" y="361">
      <linkto id="632297204531370432" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connID</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: SetMedia action failed, deleting connection...</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370434" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="577" y="362">
      <linkto id="632297204531370435" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connID</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: AnswerCall action failed... deleting connection</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370435" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="443" y="533">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: ending script</log>
      </Properties>
    </node>
    <node type="Comment" id="632297293797464144" text="when the call comes in, we create a connection to the MMS,&#xD;&#xA;and we set the media on the caller, then answer the call." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="220" y="147" />
    <node type="Comment" id="632297293797464145" text="If the action fails, delete MMS connection and exit script" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="584" y="435" />
    <node type="Comment" id="632297293797464146" text="If we fail at this point,&#xD;&#xA;drop the call and exit" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="126" y="343" />
    <node type="Variable" id="632285030923669439" name="ipAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">ipAddress</Properties>
    </node>
    <node type="Variable" id="632285030923669440" name="port" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">port</Properties>
    </node>
    <node type="Variable" id="632285030923669458" name="callID" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callId" refType="reference">callID</Properties>
    </node>
    <node type="Variable" id="632285030923669502" name="dialedNumber" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to" refType="reference">dialedNumber</Properties>
    </node>
    <node type="Variable" id="632298246905210457" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="from" refType="reference">from</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnAnswerCall_Complete" startnode="632285030923669445" treenode="632285030923669446" appnode="632285030923669443" handlerfor="632285030923669442">
    <node type="Start" id="632285030923669445" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="159">
      <linkto id="632297293797464078" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632285030923669460" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="586" y="158">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632285030923669466" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="247" y="158">
      <linkto id="632285030923669477" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632297204531370439" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">callID</ap>
        <ap name="remotePort" type="variable">port</ap>
        <ap name="connectionId" type="variable">g_connID</ap>
        <ap name="remoteIp" type="variable">ipAddress</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Complete: Completing half-connect....</log>
      </Properties>
    </node>
    <node type="Action" id="632285030923669477" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="321" y="142" mx="414" my="158">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632285030923669471" />
        <item text="OnPlayAnnouncement_Failed" treenode="632285030923669476" />
      </items>
      <linkto id="632285030923669460" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632297204531370439" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="connectionId" type="variable">g_connID</ap>
        <ap name="filename" type="literal">enter_pin.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Complete: Playing announcement to caller 1...</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370436" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="251" y="504">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632297204531370439" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="191" y="310" mx="253" my="326">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632297204531370437" />
        <item text="OnHangup_Failed" treenode="632297204531370438" />
      </items>
      <linkto id="632297204531370436" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">callID</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Complete: Failure... hanging up call...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464078" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="130" y="159">
      <linkto id="632285030923669466" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_IsDisc1, ref bool g_ReceiveDigits)
	{
		g_IsDisc1 = false;
		g_ReceiveDigits = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Comment" id="632297293797464147" text="now that caller1 is connected, we set the g_IsDisc1 (IsDisconnected) flag to false.&#xD;&#xA;we complete the connection to the MMS by providing it with the calling devices ip &#xD;&#xA;and port, then we play the &quot;Enter Pin&quot; announcement" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="78" y="87" />
    <node type="Comment" id="632297293797464148" text="If these actions fail, we hang up the call" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="312" y="332" />
    <node type="Variable" id="632285030923669463" name="callID" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callId" refType="reference">callID</Properties>
    </node>
    <node type="Variable" id="632285030923669464" name="ipAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="mediaIP" refType="reference">ipAddress</Properties>
    </node>
    <node type="Variable" id="632285030923669465" name="port" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" initWith="mediaPort" refType="reference">port</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Complete" startnode="632285030923669470" treenode="632285030923669471" appnode="632285030923669468" handlerfor="632285030923669467">
    <node type="Start" id="632285030923669470" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="421">
      <linkto id="632297204531370457" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632285030923669478" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="844" y="420">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete:  ending function</log>
      </Properties>
    </node>
    <node type="Action" id="632285030923669491" name="ReceiveDigits" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="548" y="404" mx="626" my="420">
      <items count="2">
        <item text="OnReceiveDigits_Complete" treenode="632285030923669485" />
        <item text="OnReceiveDigits_Failed" treenode="632285030923669490" />
      </items>
      <linkto id="632285030923669478" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632297204531370473" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">4</ap>
        <ap name="connectionId" type="variable">g_connID</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete:  receivedigits flag was set, attempting to retrieve digits...</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370265" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="622" y="923">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: ending function...</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370448" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="270" y="670">
      <linkto id="632297204531370458" type="Labeled" style="Bezier" ortho="true" label="2" />
      <linkto id="632297204531370453" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">userData</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete:  The flag is set...</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370453" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="374.000977" y="654" mx="436" my="670">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632297204531370451" />
        <item text="OnHangup_Failed" treenode="632297204531370452" />
      </items>
      <linkto id="632297204531370467" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_CallId1</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete:  Hanging up caller 1</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370457" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="272" y="420">
      <linkto id="632297204531370468" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632297204531370448" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_PerformHangup</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: checking if the perform hangup flag is set...</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370458" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="270" y="789">
      <linkto id="632297204531370462" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632297204531370467" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_IsDisc2</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete:  checking to see if caller 2 is connected before hanging him up...</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370462" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="208" y="916" mx="270" my="932">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632297204531370460" />
        <item text="OnHangup_Failed" treenode="632297204531370461" />
      </items>
      <linkto id="632297204531370467" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_CallId2</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: hanging up caller 2</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370467" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="431" y="933">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632297204531370468" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="438" y="420">
      <linkto id="632285030923669491" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632297204531370471" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_ReceiveDigits</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete:  flag is not set. Checking for ReceiveDigits flag...</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370471" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="436" y="226">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: receive digits flag not set, ending function.</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370473" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="624" y="558">
      <linkto id="632297204531370476" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete:  ReceiveDigits action failed...</log>
	public static string Execute(ref bool g_ReceiveDigits, ref bool g_PerformHangup)
	{
		g_ReceiveDigits = false;
		g_PerformHangup = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632297204531370476" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="533" y="654" mx="626" my="670">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632297204531370474" />
        <item text="OnPlayAnnouncement_Failed" treenode="632297204531370475" />
      </items>
      <linkto id="632297204531370265" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632297204531370453" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="connectionId" type="variable">g_connID</ap>
        <ap name="filename" type="literal">sys_unavailable_trylater.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete:  playing announcement to user specifying failure...</log>
      </Properties>
    </node>
    <node type="Comment" id="632297293797464116" text="hang up first caller" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="393" y="581" />
    <node type="Comment" id="632297293797464117" text="We take this branch if previously&#xD;&#xA;we set the g_PerformHangup flag,&#xD;&#xA;so that we hang up the call after &#xD;&#xA;the announcement." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="70" y="439" />
    <node type="Comment" id="632297293797464149" text="hang up&#xD;&#xA;second&#xD;&#xA;caller" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="41" y="858" />
    <node type="Comment" id="632297293797464150" text="Check to see&#xD;&#xA;if the second&#xD;&#xA;caller is connected&#xD;&#xA;before hanging&#xD;&#xA;him up" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="702" />
    <node type="Comment" id="632297293797464151" text="if the g_ReceiveDigits flag is set, &#xD;&#xA;we take this branch to receive input" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="546" y="345" />
    <node type="Comment" id="632297293797464152" text="Otherwise, we set the g_PerformHangup flag, and &#xD;&#xA;play an announcement, announcing that an error occured,&#xD;&#xA;which should exit the app. If the action fails, we default to&#xD;&#xA;a Hangup action to remove the call" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="695" y="651" />
    <node type="Comment" id="632297293797464153" text="If none of the flags are set, simply exit" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="339" y="173" />
    <node type="Variable" id="632298258783491996" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="userData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnReceiveDigits_Complete" startnode="632285030923669484" treenode="632285030923669485" appnode="632285030923669482" handlerfor="632285030923669481">
    <node type="Start" id="632285030923669484" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="112" y="314">
      <linkto id="632285030923669495" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632285030923669493" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1053" y="317">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete:  Ending function...</log>
      </Properties>
    </node>
    <node type="Action" id="632285030923669495" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="316.999969" y="317">
      <linkto id="632297204531370477" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632285030923669519" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">digits</ap>
        <ap name="Value2" type="variable">g_authCode</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete: Comparing user input to configured pin...</log>
      </Properties>
    </node>
    <node type="Action" id="632285030923669496" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="105" y="490">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete: playannouncement complete, ending function</log>
      </Properties>
    </node>
    <node type="Action" id="632285030923669499" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="228" y="476" mx="321" my="492">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632285030923669497" />
        <item text="OnPlayAnnouncement_Failed" treenode="632285030923669498" />
      </items>
      <linkto id="632285030923669496" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632297204531370269" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="connectionId" type="variable">g_connID</ap>
        <ap name="filename" type="literal">error_login.wav</ap>
        <ap name="filename2" type="literal">enter_pin.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete: Re-requesting proper ping number from user</log>
      </Properties>
    </node>
    <node type="Action" id="632285030923669516" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="805" y="302" mx="871" my="318">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632285030923669509" />
        <item text="OnMakeCall_Failed" treenode="632285030923669514" />
        <item text="OnHangup" treenode="632285030923669515" />
      </items>
      <linkto id="632285030923669493" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632297204531370279" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="to" type="csharp">g_dialedNumber + '@' + g_callManagerIp</ap>
        <ap name="mediaIP" type="variable">ipAddress</ap>
        <ap name="mediaPort" type="variable">port</ap>
        <ap name="from" type="variable">g_callerIdentification</ap>
        <ap name="userData" type="literal">none</ap>
        <rd field="callId">g_CallId2</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete:  Making outgoing call...</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370266" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="319" y="780">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632297204531370269" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="258" y="621" mx="320" my="637">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632297204531370267" />
        <item text="OnHangup_Failed" treenode="632297204531370268" />
      </items>
      <linkto id="632297204531370266" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_CallId1</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete: error playing announcement, hanging up caller 1</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370273" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="595" y="413">
      <linkto id="632297204531370276" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_ReceiveDigits, ref bool g_PerformHangup)
	{
		g_ReceiveDigits = false;
		g_PerformHangup = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632297204531370276" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="503.999" y="497" mx="597" my="513">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632297204531370274" />
        <item text="OnPlayAnnouncement_Failed" treenode="632297204531370275" />
      </items>
      <linkto id="632297204531370279" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632297204531370281" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="connectionId" type="variable">g_connID</ap>
        <ap name="filename" type="literal">sys_unavailable_trylater.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete: playing announcement specifying a system error</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370279" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="995" y="496" mx="1057" my="512">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632297204531370277" />
        <item text="OnHangup_Failed" treenode="632297204531370278" />
      </items>
      <linkto id="632285030923669493" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_CallId1</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete: hanging up caller 1</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370281" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="595" y="669">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete: Ending function...</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370472" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="729" y="319">
      <linkto id="632285030923669516" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_ReceiveDigits)
	{
		g_ReceiveDigits = false;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632297204531370477" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="317" y="407">
      <linkto id="632285030923669499" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete: mismatch...</log>
	public static string Execute(ref bool g_ReceiveDigits)
	{
		g_ReceiveDigits = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Comment" id="632297293797464640" text="check to see if the number entered matches the configured pin" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="167" y="253" />
    <node type="Comment" id="632297293797464641" text="if not, set the&#xD;&#xA;ReceiveDigits&#xD;&#xA;flag, and play&#xD;&#xA;announcement&#xD;&#xA;requesting re-&#xD;&#xA;entry of pin" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="188" y="377" />
    <node type="Comment" id="632297293797464642" text="if that play fails, we hangup the call" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="54" y="641" />
    <node type="Comment" id="632297293797464643" text="if the correct number was entered, we request a media server&#xD;&#xA;connection for the user we want to call, then disable the g_ReceiveDigits flag, and&#xD;&#xA;send out the call to the desired extension" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="598.0918" y="242" />
    <node type="Comment" id="632297293797464644" text="if the createconnection fails, we set the g_PerformHangup&#xD;&#xA;flag, disable g_ReceiveDigits, and play an error announcement.&#xD;&#xA;If the play fails, we manually call a hangup.&#xD;&#xA;Remeber, the default path will be taken if the PlayAnnouncement&#xD;&#xA;fails to start (Provisional response from the MMS is non-ok).&#xD;&#xA;OnPlayAnnouncement_Failed will occur if the provisional response is OK,&#xD;&#xA;but then later fails on the MMS for some reason." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="656.0918" y="605" />
    <node type="Action" id="632285030923669519" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="595" y="318">
      <linkto id="632297204531370273" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632297204531370472" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="literal">0</ap>
        <ap name="connectionId" type="literal">0</ap>
        <ap name="remoteIp" type="literal">0</ap>
        <rd field="connectionId">g_connID2</rd>
        <rd field="port">port</rd>
        <rd field="ipAddress">ipAddress</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete: Match. Creating MMS connection for callee</log>
      </Properties>
    </node>
    <node type="Variable" id="632285030923669492" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="digits" refType="reference">digits</Properties>
    </node>
    <node type="Variable" id="632285030923669522" name="ipAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">ipAddress</Properties>
    </node>
    <node type="Variable" id="632285030923669523" name="port" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.UInt" refType="reference">port</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Failed" startnode="632285030923669475" treenode="632285030923669498" appnode="632285030923669473" handlerfor="632285030923669472">
    <node type="Start" id="632285030923669475" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="238">
      <linkto id="632297293797464671" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632285030923669479" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="806" y="240">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed:  Both callers are already disconnected, ending function...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464671" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="185" y="238">
      <linkto id="632297293797464672" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632297293797464681" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_ExitApp</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed: checking to see if g_ExitApp flag is set...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464672" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="396" y="239">
      <linkto id="632297293797464675" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632297293797464676" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_IsDisc1</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed: flag is set... checking if caller 1 is disconnected....</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464675" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="337" y="370" mx="399" my="386">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632297293797464673" />
        <item text="OnHangup_Failed" treenode="632297293797464674" />
      </items>
      <linkto id="632297293797464680" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_CallId1</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed: hanging up caller 1...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464676" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="629" y="239">
      <linkto id="632297293797464679" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632285030923669479" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_IsDisc2</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed: caller 1 is disconnected, checking if caller 2 is disconnected...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464679" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="567" y="373" mx="629" my="389">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632297293797464677" />
        <item text="OnHangup_Failed" treenode="632297293797464678" />
      </items>
      <linkto id="632297293797464680" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_CallId2</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed: hanging up caller 2...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464680" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="514" y="579">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed:  ending function...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464681" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="184" y="382">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed: It is not. Ignoring failure, ending function...</log>
      </Properties>
    </node>
    <node type="Comment" id="632297293797464682" text="we check if the g_ExitApp flag is set." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="66" y="165" />
    <node type="Comment" id="632297293797464683" text="If not, ignore failure." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="122" y="429" />
    <node type="Comment" id="632297293797464684" text="if it is set, check if&#xD;&#xA;g_IsDisc1 is true.&#xD;&#xA;If it is false, then &#xD;&#xA;Caller1 is still connected. Hang&#xD;&#xA;him up." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="316" y="128" />
    <node type="Comment" id="632297293797464685" text="if g_IsDisc1 is set, check if&#xD;&#xA;g_IsDisc2 is true.&#xD;&#xA;If it is false, then &#xD;&#xA;Caller2 is still connected. Hang&#xD;&#xA;him up. Otherwise, end function" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="577" y="128" />
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632285030923669508" treenode="632285030923669509" appnode="632285030923669506" handlerfor="632285030923669505">
    <node type="Start" id="632285030923669508" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="68" y="134">
      <linkto id="632297293797464079" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632285030923669517" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="898" y="136">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Complete: ending function...</log>
      </Properties>
    </node>
    <node type="Action" id="632285030923669527" name="CreateConnectionConference" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="399" y="135">
      <linkto id="632285030923669530" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632297204531370290" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connID2</ap>
        <ap name="remoteIp" type="variable">ipAddress</ap>
        <ap name="remotePort" type="variable">port</ap>
        <ap name="conferenceId" type="literal">0</ap>
        <rd field="conferenceId">g_conferenceID</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Complete: Creating conference, placing caller2 into conference....</log>
      </Properties>
    </node>
    <node type="Action" id="632285030923669530" name="CreateConnectionConference" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="605" y="135">
      <linkto id="632297293797464119" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632297204531370290" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connID</ap>
        <ap name="conferenceId" type="variable">g_conferenceID</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Complete: Adding caller1 to conference....</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370290" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="413" y="297" mx="506" my="313">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632297204531370288" />
        <item text="OnPlayAnnouncement_Failed" treenode="632297204531370289" />
      </items>
      <linkto id="632297204531370479" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="connectionId" type="variable">g_connID2</ap>
        <ap name="filename" type="literal">sys_unavailable_trylater.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">2</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Complete: CreateConnectionConference action failed, playing announcement to and hanging up user 2</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370293" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="617" y="611" mx="710" my="627">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632297204531370291" />
        <item text="OnPlayAnnouncement_Failed" treenode="632297204531370292" />
      </items>
      <linkto id="632297204531370294" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="connectionId" type="variable">g_connID</ap>
        <ap name="filename" type="literal">sys_unavailable_trylater.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">1</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Complete: CreateConnectionConference action failed, playing announcement to and hanging up caller 1...</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370294" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="911" y="628">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Complete: Ending function</log>
      </Properties>
    </node>
    <node type="Action" id="632297204531370479" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="503" y="627">
      <linkto id="632297204531370293" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">	public static string Execute(ref bool g_ReceiveDigits, ref bool g_PerformHangup, ref uint g_UserToHangup)
	{
		g_ReceiveDigits = false;
		g_PerformHangup = true;
		g_UserToHangup = 1;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632297293797464079" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="196" y="135">
      <linkto id="632285030923669527" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_IsDisc2)
	{
		g_IsDisc2 = false;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632297293797464119" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="766" y="135">
      <linkto id="632285030923669517" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_ConferenceCreated)
	{
		g_ConferenceCreated = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Comment" id="632297293797464645" text="If we get here, we completed the make call, and the second user is now connected. We&#xD;&#xA;set g_IsDisc2 to false, create a conference with caller 2 in it, then add caller 1 to it. " class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="145" y="72" />
    <node type="Comment" id="632297293797464646" text="we set g_ConferenceCreated and exit the function" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="692" y="86" />
    <node type="Comment" id="632297293797464647" text="If either one of the&#xD;&#xA;CreateConenctionConference&#xD;&#xA;actions fails, we start&#xD;&#xA;by disabling g_ReceiveDigits,&#xD;&#xA;disabling g_PerformHangup, &#xD;&#xA;playing announcement to Caller2&#xD;&#xA;announcing an error" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="231" y="304" />
    <node type="Comment" id="632297293797464648" text="Regardless of whether the above play&#xD;&#xA;succeeds or fails, we move to set g_PerformHangup to true,&#xD;&#xA;and PlayAnnouncement to user1, annoucning an error" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="396" y="719" />
    <node type="Variable" id="632285030923669525" name="ipAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="mediaIP" refType="reference">ipAddress</Properties>
    </node>
    <node type="Variable" id="632285030923669526" name="port" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.UInt" initWith="mediaPort" refType="reference">port</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632285030923669513" treenode="632285030923669514" appnode="632285030923669511" handlerfor="632285030923669510">
    <node type="Start" id="632285030923669513" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="165">
      <linkto id="632297293797464136" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632285030923669518" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="546" y="167">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Failed:  ending function...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464136" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="134" y="166">
      <linkto id="632297293797464139" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_PerformHangup)
	{
		g_PerformHangup = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632297293797464139" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="223" y="149" mx="316" my="165">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632297293797464137" />
        <item text="OnPlayAnnouncement_Failed" treenode="632297293797464138" />
      </items>
      <linkto id="632285030923669518" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632297293797464142" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="connectionId" type="variable">g_connID</ap>
        <ap name="filename" type="literal">sys_unavailable_trylater.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Failed:  Playing announcement to caller 1 announcing failure, exiting...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464142" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="255" y="322" mx="317" my="338">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632297293797464140" />
        <item text="OnHangup_Failed" treenode="632297293797464141" />
      </items>
      <linkto id="632285030923669518" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_CallId1</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Failed: playannouncement action failed, hanging up caller 1 manually...</log>
      </Properties>
    </node>
    <node type="Comment" id="632297293797464649" text="If the MakeCall action failed, we enable the g_PerformHangup flag and the ExitApp flag, in case the play announcement fails,&#xD;&#xA;then play an announcement to the user, announcing that there was a problem.&#xD;&#xA;The OnPlayAnnouncement handlers will then exit." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="35" y="79" />
    <node type="Comment" id="632297293797464653" text="If action fails, then call hangup manually" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="391" y="343" />
  </canvas>
  <canvas type="Function" name="OnHangup_Complete" startnode="632297204531370257" treenode="632297293797464140" appnode="632297204531370255" handlerfor="632297204531370254">
    <node type="Start" id="632297204531370257" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="229">
      <linkto id="632297293797464102" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632297293797464102" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="157" y="229">
      <linkto id="632297293797464103" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632297293797464108" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_CallId1</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup_Complete: checking to see if hanging up caller is caller 1....</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464103" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="293" y="229">
      <linkto id="632297293797464105" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">OnHangup_Complete: Yes, caller 1 hung up</log>
	public static string Execute(ref bool g_IsDisc1)
	{
		g_IsDisc1 = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632297293797464105" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="446" y="230">
      <linkto id="632297293797464122" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connID</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup_Complete: Deleting MMS Connection for caller 1...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464106" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="572" y="461">
      <linkto id="632297293797464107" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="conferenceId" type="variable">g_conferenceID</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup_Complete: deleting conference connection...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464107" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="504" y="652">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnHangup_Complete:  ending script...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464108" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="155" y="356">
      <linkto id="632297293797464113" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">OnHangup_Complete: No, Caller 2 hung up...</log>
	public static string Execute(ref bool g_IsDisc2)
	{
		g_IsDisc2 = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632297293797464113" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="153" y="459">
      <linkto id="632297293797464122" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connID2</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup_Complete:  Deleting MMS Connection for Caller 2</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464122" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="445" y="461">
      <linkto id="632297293797464106" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632297293797464107" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">(g_IsDisc1 ^ g_IsDisc2) &amp;&amp; g_ConferenceCreated</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup_Complete: Checking if there exists a conference to delete...</log>
      </Properties>
    </node>
    <node type="Comment" id="632297293797464654" text="First we check if&#xD;&#xA;the call that was&#xD;&#xA;just hung up was&#xD;&#xA;Caller1" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="76" y="140" />
    <node type="Comment" id="632297293797464655" text="yes-&gt;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="214" y="166" />
    <node type="Comment" id="632297293797464657" text="the caller&#xD;&#xA;hung up was&#xD;&#xA;caller2" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="57" y="274" />
    <node type="Comment" id="632297293797464658" text="we set g_IsDisc1 to true,&#xD;&#xA;then delete the caller 1 media&#xD;&#xA;connection" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="303" y="154" />
    <node type="Comment" id="632297293797464659" text="we set g_IsDisc2 to&#xD;&#xA;true, then delete the&#xD;&#xA;caller 1 media connection" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="206" y="383" />
    <node type="Comment" id="632297293797464662" text="caller2 already&#xD;&#xA;disconnected" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="225" y="519" />
    <node type="Comment" id="632297293797464663" text="we check to see if the second caller&#xD;&#xA;is already disconnected. If not, we set&#xD;&#xA;g_IsDisc2 to true and delete the caller 2 &#xD;&#xA;connection" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="645" y="133" />
    <node type="Comment" id="632297293797464664" text="if both are disconnected,&#xD;&#xA;then we will delete the &#xD;&#xA;conference to be sure &#xD;&#xA;our media connections&#xD;&#xA;are cleaned up; &#xD;&#xA;otherwise just end&#xD;&#xA;the script" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="632" y="398" />
    <node type="Variable" id="632297293797464080" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callId" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup_Failed" startnode="632297204531370262" treenode="632297293797464141" appnode="632297204531370260" handlerfor="632297204531370259">
    <node type="Start" id="632297204531370262" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="374">
      <linkto id="632297293797464115" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632297293797464115" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="235" y="376">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632297293797464135" text="We don't need to take any measures to&#xD;&#xA;correct for this here. " class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="49" y="308" />
  </canvas>
  <canvas type="Function" name="OnHangup" startnode="632285030923669455" treenode="632285030923669515" appnode="632285030923669453" handlerfor="632285030923669452">
    <node type="Start" id="632285030923669455" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="197">
      <linkto id="632297293797464086" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632297293797464086" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="171" y="198">
      <linkto id="632297293797464087" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632297293797464088" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_CallId1</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: checking to see if caller that hung up was caller 1....</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464087" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="303" y="198">
      <linkto id="632297293797464099" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_IsDisc1)
	{
		g_IsDisc1 = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632297293797464088" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="171" y="331">
      <linkto id="632297293797464100" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_IsDisc2)
	{
		g_IsDisc2 = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632297293797464092" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="111" y="641" mx="173" my="657">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632297293797464090" />
        <item text="OnHangup_Failed" treenode="632297293797464091" />
      </items>
      <linkto id="632297293797464101" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_CallId1</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: No, hanging up caller 1...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464095" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="691" y="183" mx="753" my="199">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632297293797464093" />
        <item text="OnHangup_Failed" treenode="632297293797464094" />
      </items>
      <linkto id="632297293797464101" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_CallId2</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Hanging up caller 2....</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464096" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="170" y="530">
      <linkto id="632297293797464092" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632297293797464669" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_IsDisc1</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: checking to see if caller 1 is disconnected....</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464097" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="607" y="199">
      <linkto id="632297293797464095" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632297293797464669" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_IsDisc2</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: checking to see if caller 2 is disconnected...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464099" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="465" y="200">
      <linkto id="632297293797464097" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connID</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Yes. Deleting caller 1 MMS Connection</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464100" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="170" y="441">
      <linkto id="632297293797464096" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connID2</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: No. Deleting caller 2 MMS Connection...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464101" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="751" y="656">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Ending function....</log>
      </Properties>
    </node>
    <node type="Comment" id="632297293797464665" text="check to see if the prson that hung&#xD;&#xA;up was caller 1" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="112" />
    <node type="Comment" id="632297293797464666" text="If so, set g_IsDisc1 to true, then delete the caller 1&#xD;&#xA;connection to the MMS" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="245" y="129" />
    <node type="Comment" id="632297293797464667" text="If not, set g_IsDisc2 to true, then delete the caller 2&#xD;&#xA;connection to the MMS" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="210" y="370" />
    <node type="Comment" id="632297293797464668" text="Check to see if caller2 is disconnected. If not,&#xD;&#xA;Hangup caller2, else end script." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="566" y="126" />
    <node type="Action" id="632297293797464669" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="605" y="530">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: No hangup needed, exiting script...</log>
      </Properties>
    </node>
    <node type="Comment" id="632297293797464670" text="Check to see if caller1 is disconnected. If not,&#xD;&#xA;Hangup caller1, else end script." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="199" y="539" />
    <node type="Variable" id="632297293797464071" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="callId" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnReceiveDigits_Failed" startnode="632285030923669489" treenode="632285030923669490" appnode="632285030923669487" handlerfor="632285030923669486">
    <node type="Start" id="632285030923669489" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="236">
      <linkto id="632297293797464128" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632285030923669494" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="544" y="234">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Failed:  Ending function...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464127" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="246" y="219" mx="339" my="235">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632297293797464125" />
        <item text="OnPlayAnnouncement_Failed" treenode="632297293797464126" />
      </items>
      <linkto id="632285030923669494" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632297293797464131" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="connectionId" type="variable">g_connID</ap>
        <ap name="filename" type="literal">sys_unavailable_trylater.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Failed: Playing failure announcement, and hanging up caller 1...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464128" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="159" y="236">
      <linkto id="632297293797464127" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_PerformHangup)
	{
		g_PerformHangup = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632297293797464131" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="277" y="429" mx="339" my="445">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632297293797464129" />
        <item text="OnHangup_Failed" treenode="632297293797464130" />
      </items>
      <linkto id="632285030923669494" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_CallId1</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Failed:  PlayAnnouncement action failed, hanging up caller 1...</log>
      </Properties>
    </node>
    <node type="Comment" id="632297293797464132" text="Hangup user one if the play failed" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="425" y="490" />
    <node type="Comment" id="632297293797464133" text="set the g_PerformHangup flag, and play announcement announcing&#xD;&#xA;failure. The user is then hung up on." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="82" y="160" />
  </canvas>
  <canvas type="Function" name="OnAnswerCall_Failed" startnode="632285030923669450" treenode="632285030923669451" appnode="632285030923669448" handlerfor="632285030923669447">
    <node type="Start" id="632285030923669450" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="261">
      <linkto id="632297293797464124" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632297293797464123" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="367" y="264">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Failed: ending script...</log>
      </Properties>
    </node>
    <node type="Action" id="632297293797464124" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="186" y="263">
      <linkto id="632297293797464123" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connID</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Failed:  Deleting MMS Connection...</log>
      </Properties>
    </node>
    <node type="Comment" id="632297293797464134" text="We could not answer the call, so we free the&#xD;&#xA;MMS connection." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="60" y="185" />
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>