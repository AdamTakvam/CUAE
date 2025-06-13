<Application name="VoiceMail" trigger="Metreos.Providers.H323.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="VoiceMail">
    <outline>
      <treenode type="evh" id="632355263044138907" level="1" text="Metreos.Providers.H323.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632355263044138904" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632355263044138903" path="Metreos.Providers.H323.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632355263044138990" level="2" text="Metreos.Providers.H323.AnswerCall_Complete: OnAnswerCall_Complete">
        <node type="function" name="OnAnswerCall_Complete" id="632355263044138987" path="Metreos.StockTools" />
        <node type="event" name="AnswerCall_Complete" id="632355263044138986" path="Metreos.Providers.H323.AnswerCall_Complete" />
        <references>
          <ref id="632375918692194165" actid="632355263044139001" />
          <ref id="632375918692194176" actid="632361225879369007" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632355263044138995" level="2" text="Metreos.Providers.H323.AnswerCall_Failed: OnAnswerCall_Failed">
        <node type="function" name="OnAnswerCall_Failed" id="632355263044138992" path="Metreos.StockTools" />
        <node type="event" name="AnswerCall_Failed" id="632355263044138991" path="Metreos.Providers.H323.AnswerCall_Failed" />
        <references>
          <ref id="632375918692194166" actid="632355263044139001" />
          <ref id="632375918692194177" actid="632361225879369007" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632355263044139000" level="2" text="Metreos.Providers.H323.Hangup: OnHangup">
        <node type="function" name="OnHangup" id="632355263044138997" path="Metreos.StockTools" />
        <node type="event" name="Hangup" id="632355263044138996" path="Metreos.Providers.H323.Hangup" />
        <references>
          <ref id="632375918692194167" actid="632355263044139001" />
          <ref id="632375918692194178" actid="632361225879369007" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632361225879369031" level="2" text="Metreos.Providers.MediaServer.PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632361225879369028" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632361225879369027" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <references>
          <ref id="632375918692194188" actid="632361225879369037" />
          <ref id="632375918692194213" actid="632361391698431607" />
          <ref id="632375918692194247" actid="632368201375937917" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632361225879369036" level="2" text="Metreos.Providers.MediaServer.PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632361225879369033" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632361225879369032" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <references>
          <ref id="632375918692194189" actid="632361225879369037" />
          <ref id="632375918692194214" actid="632361391698431607" />
          <ref id="632375918692194248" actid="632368201375937917" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632361225879369044" level="2" text="Metreos.Providers.H323.Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632361225879369041" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632361225879369040" path="Metreos.Providers.H323.Hangup_Complete" />
        <references>
          <ref id="632375918692194191" actid="632361225879369050" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632361225879369049" level="2" text="Metreos.Providers.H323.Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632361225879369046" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632361225879369045" path="Metreos.Providers.H323.Hangup_Failed" />
        <references>
          <ref id="632375918692194192" actid="632361225879369050" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632362211266244117" level="2" text="Metreos.Providers.MediaServer.ReceiveDigits_Complete: OnReceiveDigits_Complete">
        <node type="function" name="OnReceiveDigits_Complete" id="632362211266244114" path="Metreos.StockTools" />
        <node type="event" name="ReceiveDigits_Complete" id="632362211266244113" path="Metreos.Providers.MediaServer.ReceiveDigits_Complete" />
        <references>
          <ref id="632375918692194219" actid="632362211266244123" />
        </references>
        <Properties type="asyncCallback">
          <ep name="userData" type="literal">AddFeat</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632362211266244122" level="2" text="Metreos.Providers.MediaServer.ReceiveDigits_Failed: OnReceiveDigits_Failed">
        <node type="function" name="OnReceiveDigits_Failed" id="632362211266244119" path="Metreos.StockTools" />
        <node type="event" name="ReceiveDigits_Failed" id="632362211266244118" path="Metreos.Providers.MediaServer.ReceiveDigits_Failed" />
        <references>
          <ref id="632375918692194220" actid="632362211266244123" />
          <ref id="632375918692194224" actid="632374998352818153" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632368096905937901" level="2" text="Metreos.Providers.MediaServer.RecordAudio_Failed: OnRecordAudio_Failed">
        <node type="function" name="OnRecordAudio_Failed" id="632368096905937898" path="Metreos.StockTools" />
        <node type="event" name="RecordAudio_Failed" id="632368096905937897" path="Metreos.Providers.MediaServer.RecordAudio_Failed" />
        <references>
          <ref id="632375918692194268" actid="632368096905937902" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632368181503906665" level="2" text="Metreos.Providers.MediaServer.RecordAudio_Complete: OnRecordAudio_Complete_NewMsg">
        <node type="function" name="OnRecordAudio_Complete_NewMsg" id="632368181503906661" path="Metreos.StockTools" />
        <node type="event" name="RecordAudio_Complete" id="632368096905937892" path="Metreos.Providers.MediaServer.RecordAudio_Complete" />
        <references>
          <ref id="632375918692194267" actid="632368096905937902" />
        </references>
        <Properties type="asyncCallback">
          <ep name="userData" type="literal">NewMsg</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632374998352818160" level="2" text="Metreos.Providers.MediaServer.ReceiveDigits_Complete: OnReceiveDigits_Complete_AddFeat">
        <node type="function" name="OnReceiveDigits_Complete_AddFeat" id="632374998352818156" path="Metreos.StockTools" />
        <node type="event" name="ReceiveDigits_Complete" id="632362211266244113" path="Metreos.Providers.MediaServer.ReceiveDigits_Complete" />
        <references>
          <ref id="632375918692194223" actid="632374998352818153" />
        </references>
        <Properties type="asyncCallback">
          <ep name="userData" type="literal">AddFeat</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632375918692193185" level="2" text="Metreos.Providers.H323.GotDigits: OnGotDigits">
        <node type="function" name="OnGotDigits" id="632375918692193182" path="Metreos.StockTools" />
        <node type="event" name="GotDigits" id="632375918692193181" path="Metreos.Providers.H323.GotDigits" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632375918692193190" level="2" text="Metreos.Providers.H323.SignallingChange: OnSignallingChange">
        <node type="function" name="OnSignallingChange" id="632375918692193187" path="Metreos.StockTools" />
        <node type="event" name="SignallingChange" id="632375918692193186" path="Metreos.Providers.H323.SignallingChange" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632361391698431643" level="1" text="MainMenu">
        <node type="function" name="MainMenu" id="632361391698431640" path="Metreos.StockTools" />
        <calls>
          <ref actid="632361391698431639" />
        </calls>
      </treenode>
      <treenode type="fun" id="632362211266244130" level="1" text="RecordMessage">
        <node type="function" name="RecordMessage" id="632362211266244127" path="Metreos.StockTools" />
        <calls>
          <ref actid="632362211266244126" />
        </calls>
      </treenode>
      <treenode type="fun" id="632375918692194291" level="1" text="UserNotifyAction">
        <node type="function" name="UserNotifyAction" id="632375918692194288" path="Metreos.StockTools" />
        <calls>
          <ref actid="632375918692194287" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_connectionId_caller" id="632375918692194115" vid="632355263044138913">
        <Properties type="Int">g_connectionId_caller</Properties>
      </treenode>
      <treenode text="g_callRecordId" id="632375918692194117" vid="632355263044138915">
        <Properties type="UInt">g_callRecordId</Properties>
      </treenode>
      <treenode text="g_userId" id="632375918692194119" vid="632355263044139038">
        <Properties type="UInt">g_userId</Properties>
      </treenode>
      <treenode text="g_callId_caller" id="632375918692194121" vid="632355263044139042">
        <Properties type="String">g_callId_caller</Properties>
      </treenode>
      <treenode text="g_mode" id="632375918692194123" vid="632355263044139051">
        <Properties type="String">g_mode</Properties>
      </treenode>
      <treenode text="g_accountStatus" id="632375918692194125" vid="632358705072494026">
        <Properties type="String">g_accountStatus</Properties>
      </treenode>
      <treenode text="g_isFirstLogin" id="632375918692194127" vid="632358740489681482">
        <Properties type="Bool">g_isFirstLogin</Properties>
      </treenode>
      <treenode text="g_greetingFilename" id="632375918692194129" vid="632358740489681484">
        <Properties type="String">g_greetingFilename</Properties>
      </treenode>
      <treenode text="g_sortingOrder" id="632375918692194131" vid="632358740489681486">
        <Properties type="String">g_sortingOrder</Properties>
      </treenode>
      <treenode text="g_notifyMethod" id="632375918692194133" vid="632358740489681488">
        <Properties type="String">g_notifyMethod</Properties>
      </treenode>
      <treenode text="g_notificationAddress" id="632375918692194135" vid="632358740489681490">
        <Properties type="String">g_notificationAddress</Properties>
      </treenode>
      <treenode text="g_maxMessageLength" id="632375918692194137" vid="632358740489681492">
        <Properties type="UInt">g_maxMessageLength</Properties>
      </treenode>
      <treenode text="g_maxNumberMessages" id="632375918692194139" vid="632358740489681494">
        <Properties type="UInt">g_maxNumberMessages</Properties>
      </treenode>
      <treenode text="g_maxStorageDays" id="632375918692194141" vid="632358740489681496">
        <Properties type="UInt">g_maxStorageDays</Properties>
      </treenode>
      <treenode text="g_describeEachMessage" id="632375918692194143" vid="632358740489681498">
        <Properties type="Bool">g_describeEachMessage</Properties>
      </treenode>
      <treenode text="g_voiceMailSettingsId" id="632375918692194145" vid="632358740489681500">
        <Properties type="UInt">g_voiceMailSettingsId</Properties>
      </treenode>
      <treenode text="g_userAccountStatus" id="632375918692194147" vid="632361225879369004">
        <Properties type="String">g_userAccountStatus</Properties>
      </treenode>
      <treenode text="g_authenticated" id="632375918692194149" vid="632361225879369022">
        <Properties type="Bool" defaultInitWith="false">g_authenticated</Properties>
      </treenode>
      <treenode text="g_introFilename" id="632375918692194151" vid="632361391698431610">
        <Properties type="String">g_introFilename</Properties>
      </treenode>
      <treenode text="g_recordingFilename" id="632375918692194153" vid="632368181503906657">
        <Properties type="String">g_recordingFilename</Properties>
      </treenode>
      <treenode text="g_voiceMailRecord" id="632375918692194155" vid="632368181503906659">
        <Properties type="Metreos.ApplicationSuite.Types.VoiceMailRecord">g_voiceMailRecord</Properties>
      </treenode>
      <treenode text="g_voiceMailRecordCollection" id="632375918692194157" vid="632368181503906882">
        <Properties type="Metreos.ApplicationSuite.Types.VoiceMailRecordCollection">g_voiceMailRecordCollection</Properties>
      </treenode>
      <treenode text="g_voiceMailRecordCount" id="632375918692194159" vid="632368201375937911">
        <Properties type="String">g_voiceMailRecordCount</Properties>
      </treenode>
      <treenode text="g_isRecording" id="632375918692194161" vid="632375918692193165">
        <Properties type="Bool" defaultInitWith="false">g_isRecording</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632355263044138906" treenode="632355263044138907" appnode="632355263044138904" handlerfor="632355263044138903">
    <node type="Start" id="632355263044138906" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="303">
      <linkto id="632361225879369011" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632355263044138985" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="831" y="301">
      <linkto id="632355263044139001" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632361225879369007" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="literal">0</ap>
        <ap name="connectionId" type="literal">0</ap>
        <ap name="callId" type="variable">incomingCallId</ap>
        <ap name="remoteIp" type="literal">0</ap>
        <rd field="connectionId">g_connectionId_caller</rd>
      </Properties>
    </node>
    <node type="Action" id="632355263044139001" name="AnswerCall" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="900" y="284" mx="971" my="300">
      <items count="3">
        <item text="OnAnswerCall_Complete" treenode="632355263044138990" />
        <item text="OnAnswerCall_Failed" treenode="632355263044138995" />
        <item text="OnHangup" treenode="632355263044139000" />
      </items>
      <linkto id="632355263044139044" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="answer" type="literal">true</ap>
        <ap name="callId" type="variable">incomingCallId</ap>
        <ap name="userData" type="literal">none</ap>
        <rd field="callId">g_callId_caller</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: Answering call</log>
      </Properties>
    </node>
    <node type="Action" id="632355263044139037" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="506" y="301">
      <linkto id="632358705072494025" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="OriginNumber" type="variable">from</ap>
        <ap name="DestinationNumber" type="variable">to</ap>
        <ap name="UserId" type="variable">g_userId</ap>
        <rd field="CallRecordsId">g_callRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632355263044139041" name="GetUserByPrimaryDN" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="262" y="302">
      <linkto id="632358740489681502" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632361225879369006" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native">
        <ap name="DirectoryNumber" type="variable">to</ap>
        <rd field="UserId">g_userId</rd>
        <rd field="UserStatus">g_userAccountStatus</rd>
      </Properties>
    </node>
    <node type="Action" id="632355263044139044" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1134" y="300">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632358705072494025" name="GetVoiceMailSettings" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="653" y="301">
      <linkto id="632355263044138985" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632358740489681503" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
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
      </Properties>
    </node>
    <node type="Action" id="632358740489681502" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="262" y="422">
      <linkto id="632358740489681503" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="OriginNumber" type="variable">from</ap>
        <ap name="DestinationNumber" type="variable">to</ap>
        <rd field="CallRecordsId">g_callRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632358740489681503" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="652" y="420">
      <linkto id="632361225879369018" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.Invalid</ap>
      </Properties>
    </node>
    <node type="Action" id="632361225879369006" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="395" y="301">
      <linkto id="632355263044139037" type="Labeled" style="Bezier" ortho="true" label="Ok" />
      <linkto id="632358740489681502" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">g_userAccountStatus</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: checking user account status</log>
      </Properties>
    </node>
    <node type="Action" id="632361225879369007" name="AnswerCall" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="762.4707" y="404" mx="834" my="420">
      <items count="3">
        <item text="OnAnswerCall_Complete" treenode="632355263044138990" />
        <item text="OnAnswerCall_Failed" treenode="632355263044138995" />
        <item text="OnHangup" treenode="632355263044139000" />
      </items>
      <linkto id="632358740489681503" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="answer" type="literal">false</ap>
        <ap name="callId" type="variable">incomingCallId</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: could not half-connect, rejecting call...</log>
      </Properties>
    </node>
    <node type="Action" id="632361225879369011" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="132" y="303">
      <linkto id="632355263044139041" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">incomingCallId</ap>
        <rd field="ResultData">g_callId_caller</rd>
      </Properties>
    </node>
    <node type="Action" id="632361225879369018" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="652" y="558">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: Exiting application.</log>
      </Properties>
    </node>
    <node type="Comment" id="632361225879369019" text="Need to add better handling for&#xD;&#xA;disabled accounts and what-not." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="298" y="242" />
    <node type="Variable" id="632355263044138908" name="incomingCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callId" refType="reference">incomingCallId</Properties>
    </node>
    <node type="Variable" id="632355263044138909" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632355263044138910" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="from" refType="reference">from</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnAnswerCall_Complete" startnode="632355263044138989" treenode="632355263044138990" appnode="632355263044138987" handlerfor="632355263044138986">
    <node type="Start" id="632355263044138989" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="38" y="232">
      <linkto id="632361225879369026" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632361225879369026" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="161" y="232">
      <linkto id="632361225879369037" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632361225879369054" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="variable">remoteMediaPort</ap>
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="remoteIp" type="variable">remoteMediaIp</ap>
        <rd field="connectionId">g_connectionId_caller</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Complete: finalizing MMS connection</log>
      </Properties>
    </node>
    <node type="Action" id="632361225879369037" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="231" y="217" mx="324" my="233">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632361225879369031" />
        <item text="OnPlayAnnouncement_Failed" treenode="632361225879369036" />
      </items>
      <linkto id="632361225879369055" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632361225879369058" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="termCondDigitList" type="literal">1,*</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <ap name="filename" type="variable">g_greetingFilename</ap>
        <ap name="filename2" type="literal">vm_recordingInstructions.wav</ap>
        <ap name="filename3" type="literal">beep.wav</ap>
        <ap name="userData" type="literal">greeting</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Complete: playing greeting message</log>
      </Properties>
    </node>
    <node type="Action" id="632361225879369050" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="175" y="444" mx="237" my="460">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632361225879369044" />
        <item text="OnHangup_Failed" treenode="632361225879369049" />
      </items>
      <linkto id="632361225879369064" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="csharp">failureReason + " Hanging up call, exiting...";</log>
      </Properties>
    </node>
    <node type="Action" id="632361225879369054" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="160" y="368">
      <linkto id="632361225879369050" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">OnAnswerCall_Complete: Completing MMS connection failed.</ap>
        <rd field="ResultData">failureReason</rd>
      </Properties>
    </node>
    <node type="Action" id="632361225879369055" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="320" y="371">
      <linkto id="632361225879369050" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">OnAnswerCall_Complete: PlayAnnouncement failed.</ap>
        <rd field="ResultData">failureReason</rd>
      </Properties>
    </node>
    <node type="Action" id="632361225879369058" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="470" y="231">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632361225879369064" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="234" y="612">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632361225879369024" name="remoteMediaIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="mediaIP" refType="reference">remoteMediaIp</Properties>
    </node>
    <node type="Variable" id="632361225879369025" name="remoteMediaPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UShort" initWith="mediaPort" refType="reference">remoteMediaPort</Properties>
    </node>
    <node type="Variable" id="632361225879369056" name="failureReason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">failureReason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnAnswerCall_Failed" startnode="632355263044138994" treenode="632355263044138995" appnode="632355263044138992" handlerfor="632355263044138991">
    <node type="Start" id="632355263044138994" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="320">
      <linkto id="632361225879369059" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632361225879369059" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="153" y="320">
      <linkto id="632361225879369061" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAnswerCall_Failed: Deleting MMS connection, exiting...</log>
      </Properties>
    </node>
    <node type="Action" id="632361225879369061" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="302" y="322">
      <linkto id="632361225879369062" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.OtherOrUnknown</ap>
      </Properties>
    </node>
    <node type="Action" id="632361225879369062" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="443" y="321">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup" startnode="632355263044138999" treenode="632355263044139000" appnode="632355263044138997" handlerfor="632355263044138996">
    <node type="Start" id="632355263044138999" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="258">
      <linkto id="632375918692194282" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632375918692193172" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="555" y="258">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632375918692193173" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="407" y="258">
      <linkto id="632375918692193172" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.NormalCallClearing</ap>
      </Properties>
    </node>
    <node type="Action" id="632375918692193174" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="267" y="257">
      <linkto id="632375918692193173" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632375918692193175" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_isRecording</ap>
      </Properties>
    </node>
    <node type="Action" id="632375918692193175" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="266" y="100">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632375918692194282" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="144" y="258">
      <linkto id="632375918692193174" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632375918692194284" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
      </Properties>
    </node>
    <node type="Action" id="632375918692194283" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="146" y="483">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="csharp">"OnHangup: Failure occured while attempting to delete connection with connectionId: " + (g_connectionId_caller == null ? "NULL!" : g_connectionId_caller);</log>
      </Properties>
    </node>
    <node type="Action" id="632375918692194284" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="146" y="371">
      <linkto id="632375918692194283" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.SystemFailure</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Complete" startnode="632361225879369030" treenode="632361225879369031" appnode="632361225879369028" handlerfor="632361225879369027">
    <node type="Start" id="632361225879369030" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="220" y="507">
      <linkto id="632361391698431600" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632361391698431600" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="352" y="507">
      <linkto id="632361391698431644" type="Labeled" style="Bezier" ortho="true" label="greeting" />
      <linkto id="632374998352818153" type="Labeled" style="Bezier" ortho="true" label="additionalOptions" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632361391698431606" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="688" y="331">
      <linkto id="632361391698431607" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_isFirstLogin</ap>
      </Properties>
    </node>
    <node type="Action" id="632361391698431607" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="584" y="185" mx="677" my="201">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632361225879369031" />
        <item text="OnPlayAnnouncement_Failed" treenode="632361225879369036" />
      </items>
      <linkto id="632361391698431638" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <ap name="filename" type="variable">g_introFilename</ap>
        <ap name="userData" type="literal">setup</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: First login, playing introduction...</log>
      </Properties>
    </node>
    <node type="Action" id="632361391698431638" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="696" y="118">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632361391698431639" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="771.825867" y="195" mx="809" my="211">
      <items count="1">
        <item text="MainMenu" />
      </items>
      <Properties final="false" type="appControl">
        <ap name="FunctionName" type="literal">MainMenu</ap>
      </Properties>
    </node>
    <node type="Action" id="632361391698431644" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="352" y="411">
      <linkto id="632362211266244123" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <linkto id="632362211266244126" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632362211266244123" name="ReceiveDigits" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="153" y="257" mx="231" my="273">
      <items count="2">
        <item text="OnReceiveDigits_Complete" treenode="632362211266244117" />
        <item text="OnReceiveDigits_Failed" treenode="632362211266244122" />
      </items>
      <Properties final="false" type="provider">
        <ap name="state" type="literal">`</ap>
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <ap name="userData" type="literal">greeting</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: Receiving digits...</log>
      </Properties>
    </node>
    <node type="Action" id="632362211266244126" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="417.30957" y="251" mx="465" my="267">
      <items count="1">
        <item text="RecordMessage" />
      </items>
      <Properties final="false" type="appControl">
        <ap name="FunctionName" type="literal">RecordMessage</ap>
      </Properties>
    </node>
    <node type="Action" id="632374998352818153" name="ReceiveDigits" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="419.176147" y="493" mx="523" my="509">
      <items count="2">
        <item text="OnReceiveDigits_Complete_AddFeat" treenode="632374998352818160" />
        <item text="OnReceiveDigits_Failed" treenode="632362211266244122" />
      </items>
      <linkto id="632374998352818161" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncement_Complete: Retrieving user input for Additional Options, calling ReceiveDigits</log>
      </Properties>
    </node>
    <node type="Action" id="632374998352818161" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="699.168335" y="506">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632361225879369072" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="userData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632361225879369073" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="terminationCondition" refType="reference">termCond</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Failed" startnode="632361225879369035" treenode="632361225879369036" appnode="632361225879369033" handlerfor="632361225879369032">
    <node type="Start" id="632361225879369035" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="260">
      <linkto id="632361225879369071" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632361225879369071" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="166" y="260">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup_Complete" startnode="632361225879369043" treenode="632361225879369044" appnode="632361225879369041" handlerfor="632361225879369040">
    <node type="Start" id="632361225879369043" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="91" y="359">
      <linkto id="632375918692193170" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632361225879369065" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="481" y="359">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632361225879369066" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="340" y="360">
      <linkto id="632361225879369065" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.SystemFailure</ap>
      </Properties>
    </node>
    <node type="Action" id="632375918692193170" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="194" y="359">
      <linkto id="632361225879369066" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632375918692193171" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_isRecording</ap>
      </Properties>
    </node>
    <node type="Action" id="632375918692193171" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="195" y="237">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup_Failed" startnode="632361225879369048" treenode="632361225879369049" appnode="632361225879369046" handlerfor="632361225879369045">
    <node type="Start" id="632361225879369048" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="311">
      <linkto id="632361225879369068" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632361225879369067" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="291.360016" y="311">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632361225879369068" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="158.360016" y="311">
      <linkto id="632361225879369067" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.SystemFailure</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnReceiveDigits_Complete" startnode="632362211266244116" treenode="632362211266244117" appnode="632362211266244114" handlerfor="632362211266244113">
    <node type="Start" id="632362211266244116" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="335" />
  </canvas>
  <canvas type="Function" name="OnReceiveDigits_Failed" startnode="632362211266244121" treenode="632362211266244122" appnode="632362211266244119" handlerfor="632362211266244118">
    <node type="Start" id="632362211266244121" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="322">
      <linkto id="632375918692193180" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632375918692193180" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="139" y="319">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecordAudio_Failed" startnode="632368096905937900" treenode="632368096905937901" appnode="632368096905937898" handlerfor="632368096905937897">
    <node type="Start" id="632368096905937900" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="280">
      <linkto id="632375918692193168" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632375918692193163" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="282" y="279">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632375918692193168" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="140.048828" y="280">
      <linkto id="632375918692193163" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_isRecording</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnRecordAudio_Failed: setting g_isRecording to false</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecordAudio_Complete_NewMsg" startnode="632368181503906663" treenode="632368181503906665" appnode="632368181503906661" handlerfor="632368096905937892">
    <node type="Start" id="632368181503906663" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="384">
      <linkto id="632375918692193167" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632368201375937916" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="260" y="384">
      <linkto id="632368201375937920" type="Labeled" style="Bezier" ortho="true" label="userstop" />
      <linkto id="632368201375937917" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">termCond</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnRecordAudio_Complete_NewMsg: checking termination condition</log>
      </Properties>
    </node>
    <node type="Action" id="632368201375937917" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="170" y="224" mx="263" my="240">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632361225879369031" />
        <item text="OnPlayAnnouncement_Failed" treenode="632361225879369036" />
      </items>
      <linkto id="632368201375937922" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="termCondDigitList" type="literal">1,7,9</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <ap name="filename" type="literal">vm_additionalOptions.wav</ap>
        <ap name="userData" type="literal">additionalOptions</ap>
      </Properties>
    </node>
    <node type="Action" id="632368201375937920" name="AddVoiceMailRecordToCollection" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="418" y="384">
      <linkto id="632375918692194287" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Record" type="variable">g_voiceMailRecord</ap>
        <ap name="RecordCollection" type="variable">g_voiceMailRecordCollection</ap>
        <rd field="RecordCount">g_voiceMailRecordCount</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnRecordAudio_Complete_NewMsg: Adding VoiceMail record to collection.</log>
      </Properties>
    </node>
    <node type="Action" id="632368201375937921" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="724.768066" y="382">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnRecordAudio_Complete_NewMsg: Terminating Application</log>
      </Properties>
    </node>
    <node type="Action" id="632368201375937922" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="259" y="139">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632375918692193167" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="134" y="384">
      <linkto id="632368201375937916" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_isRecording</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnRecordAudio_Complete_NewMsg: setting g_isRecording to false</log>
      </Properties>
    </node>
    <node type="Action" id="632375918692194287" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="544.01416" y="368" mx="593" my="384">
      <items count="1">
        <item text="UserNotifyAction" />
      </items>
      <linkto id="632368201375937921" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="FunctionName" type="literal">UserNotifyAction</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnRecordAudio_Complete_NewMsg</log>
      </Properties>
    </node>
    <node type="Variable" id="632368201375937915" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="terminationCondition" refType="reference">termCond</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnReceiveDigits_Complete_AddFeat" startnode="632374998352818158" treenode="632374998352818160" appnode="632374998352818156" handlerfor="632362211266244113">
    <node type="Start" id="632374998352818158" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="60" y="321">
      <linkto id="632375247668755449" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632375247668755449" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="171" y="321">
      <linkto id="632375247668755450" type="Labeled" style="Bezier" ortho="true" label="maxdigits" />
      <linkto id="632375247668755452" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">termCond</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete_AddFeat: Checking termination condition</log>
      </Properties>
    </node>
    <node type="Action" id="632375247668755450" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="381" y="320">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632375247668755451" text="Add code here to extract digit and&#xD;&#xA;handle additional features" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="185" y="363" />
    <node type="Action" id="632375247668755452" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="169" y="159">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632375247668755453" text="handle other return&#xD;&#xA;conditions like&#xD;&#xA;time-out" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="229" y="183" />
    <node type="Variable" id="632374998352818162" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="digits" refType="reference">digits</Properties>
    </node>
    <node type="Variable" id="632375247668755448" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="terminationCondition" refType="reference">termCond</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotDigits" startnode="632375918692193184" treenode="632375918692193185" appnode="632375918692193182" handlerfor="632375918692193181">
    <node type="Start" id="632375918692193184" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="358">
      <linkto id="632375918692194274" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632375918692194274" name="SendDigits" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="131" y="359">
      <linkto id="632375918692194276" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="digits" type="variable">digits</ap>
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnGotDigits: Proxying digits \"" + digits + "\"";</log>
      </Properties>
    </node>
    <node type="Action" id="632375918692194276" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="264" y="359">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632375918692194275" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">digits</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSignallingChange" startnode="632375918692193189" treenode="632375918692193190" appnode="632375918692193187" handlerfor="632375918692193186">
    <node type="Start" id="632375918692193189" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="364">
      <linkto id="632375918692194280" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632375918692194280" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="148" y="364">
      <linkto id="632375918692194281" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="variable">newRemotePort</ap>
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <ap name="callId" type="variable">g_callId_caller</ap>
        <ap name="remoteIp" type="variable">newRemoteIp</ap>
        <rd field="connectionId">g_connectionId_caller</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnSignalingChange: re-binding media connection with connectionId: " + g_connectionId_caller + " to remoteIp: " + newRemoteIp + " remotePort: " + newRemotePort;</log>
      </Properties>
    </node>
    <node type="Action" id="632375918692194281" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="280" y="364">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632375918692194277" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632375918692194278" name="newRemoteIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="mediaIP" refType="reference">newRemoteIp</Properties>
    </node>
    <node type="Variable" id="632375918692194279" name="newRemotePort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UShort" initWith="mediaPort" refType="reference">newRemotePort</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="MainMenu" startnode="632361391698431642" treenode="632361391698431643" appnode="632361391698431640" handlerfor="632375918692193186">
    <node type="Start" id="632361391698431642" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="351">
      <linkto id="632375918692193164" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632375918692193164" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="129" y="351">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="RecordMessage" startnode="632362211266244129" treenode="632362211266244130" appnode="632362211266244127" handlerfor="632375918692193186">
    <node type="Start" id="632362211266244129" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="339">
      <linkto id="632368181503906884" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632368096905937891" name="CreateVoiceMailRecord" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="383" y="339">
      <linkto id="632375918692194286" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native">
        <ap name="UserId" type="variable">g_userId</ap>
        <ap name="TimeStamp" type="csharp">System.DateTime.Now;</ap>
        <ap name="Status" type="literal">New</ap>
        <ap name="Length" type="literal">0</ap>
        <rd field="Record">g_voiceMailRecord</rd>
        <rd field="Filename">g_recordingFilename</rd>
        <log condition="entry" on="true" level="Info" type="literal">RecordMessage: Creating VoiceMail record</log>
      </Properties>
    </node>
    <node type="Action" id="632368096905937902" name="RecordAudio" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="584.351563" y="321" mx="687" my="337">
      <items count="2">
        <item text="OnRecordAudio_Complete_NewMsg" treenode="632368181503906665" />
        <item text="OnRecordAudio_Failed" treenode="632368096905937901" />
      </items>
      <linkto id="632368201375937914" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxTime" type="variable">g_maxMessageLength</ap>
        <ap name="termCondNonSilence" type="variable">g_maxMessageLength</ap>
        <ap name="termCondSilence" type="variable">g_maxMessageLength</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="termCondDigit" type="literal">#</ap>
        <ap name="connectionId" type="variable">g_connectionId_caller</ap>
        <ap name="filename" type="variable">g_recordingFilename</ap>
        <ap name="commandTimeout" type="variable">g_maxMessageLength</ap>
        <ap name="expires" type="variable">g_maxStorageDays</ap>
        <ap name="userData" type="literal">NewMsg</ap>
        <log condition="entry" on="true" level="Info" type="literal">RecordMessage: Invoking RecordAudio</log>
      </Properties>
    </node>
    <node type="Action" id="632368181503906884" name="GetVoiceMailRecords" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="148" y="339">
      <linkto id="632368201375937913" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native">
        <ap name="UserId" type="variable">g_userId</ap>
        <ap name="Status" type="literal">All</ap>
        <ap name="SortingOrder" type="literal">Increasing</ap>
        <rd field="RecordCollection">g_voiceMailRecordCollection</rd>
        <rd field="RecordCount">g_voiceMailRecordCount</rd>
        <log condition="entry" on="true" level="Info" type="literal">RecordMessage: retrieving message records</log>
      </Properties>
    </node>
    <node type="Action" id="632368201375937913" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="269" y="339">
      <linkto id="632368096905937891" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">g_voiceMailRecordCount &lt; g_maxNumberMessages</ap>
      </Properties>
    </node>
    <node type="Action" id="632368201375937914" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="840" y="334">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632375918692194286" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="524.4707" y="338">
      <linkto id="632368096905937902" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_isRecording</rd>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="UserNotifyAction" activetab="true" startnode="632375918692194290" treenode="632375918692194291" appnode="632375918692194288">
    <node type="Start" id="632375918692194290" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="364">
      <linkto id="632375918692194292" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632375918692194292" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="132" y="365">
      <linkto id="632375918692194293" type="Labeled" style="Bezier" label="default" />
      <linkto id="632375918692194293" type="Labeled" style="Bezier" label="mail" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">g_notifyMethod</ap>
        <log condition="entry" on="true" level="Info" type="literal">UserNotifyAction: determining notification method...</log>
      </Properties>
    </node>
    <node type="Action" id="632375918692194293" name="Send" class="MaxActionNode" group="" path="Metreos.Native.Mail" x="345" y="362">
      <Properties final="false" type="native">
        <ap name="From" type="literal">Voicemail</ap>
        <ap name="MailServer" type="literal">g_MailServer</ap>
        <ap name="Username" type="literal">g_MailServerUsername</ap>
        <ap name="Password" type="literal">g_MailServerPassword</ap>
        <ap name="Subject" type="literal">You have a new voice mail!</ap>
        <ap name="SendAsHtml" type="literal">true</ap>
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>