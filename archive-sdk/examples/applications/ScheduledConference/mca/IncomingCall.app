<Application name="IncomingCall" trigger="Metreos.CallControl.IncomingCall" version="0.7" single="false">
  <!--//// app tree ////-->
  <global name="IncomingCall">
    <outline>
      <treenode type="evh" id="632273883766250154" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632273883766250151" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632273883766250150" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
          <ep name="to" type="variable">g_appServerDn</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632274041751875167" level="2" text="AnswerCall_Complete: OnAnswerCall_Complete">
        <node type="function" name="OnAnswerCall_Complete" id="632274041751875164" path="Metreos.StockTools" />
        <node type="event" name="AnswerCall_Complete" id="632274041751875163" path="Metreos.CallControl.AnswerCall_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632274041751875187" level="3" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632274041751875184" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632274041751875183" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632277533806093985" level="4" text="ReceiveDigits_Complete: OnReceiveDigits_Complete">
        <node type="function" name="OnReceiveDigits_Complete" id="632277533806093982" path="Metreos.StockTools" />
        <node type="event" name="ReceiveDigits_Complete" id="632277533806093981" path="Metreos.Providers.MediaServer.ReceiveDigits_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632279345072304566" level="5" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632274041751875184" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632274041751875183" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632279345072304567" level="5" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632274041751875189" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632274041751875188" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632279345072304575" level="5" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632274041751875184" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632274041751875183" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632279345072304576" level="5" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632274041751875189" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632274041751875188" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632279941842773096" level="5" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632274041751875184" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632274041751875183" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632279941842773097" level="5" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632274041751875189" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632274041751875188" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632282702739376383" level="5" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632274041751875184" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632274041751875183" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632282702739376384" level="5" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632274041751875189" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632274041751875188" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632283434660469425" level="5" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632274041751875184" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632274041751875183" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632283434660469426" level="5" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632274041751875189" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632274041751875188" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632284809869219195" level="5" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632274041751875207" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632274041751875206" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632284809869219196" level="5" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632274041751875212" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632274041751875211" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632277533806093990" level="4" text="ReceiveDigits_Failed: OnReceiveDigits_Failed">
        <node type="function" name="OnReceiveDigits_Failed" id="632277533806093987" path="Metreos.StockTools" />
        <node type="event" name="ReceiveDigits_Failed" id="632277533806093986" path="Metreos.Providers.MediaServer.ReceiveDigits_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632279180679221096" level="5" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632274041751875184" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632274041751875183" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632282702739375533" level="6" text="RecordAudio_Complete: OnRecordAudio_Complete">
        <node type="function" name="OnRecordAudio_Complete" id="632282702739375530" path="Metreos.StockTools" />
        <node type="event" name="RecordAudio_Complete" id="632282702739375529" path="Metreos.Providers.MediaServer.RecordAudio_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632282702739375543" level="7" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632274041751875184" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632274041751875183" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632282702739375544" level="7" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632274041751875189" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632274041751875188" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632283434660469431" level="7" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632274041751875184" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632274041751875183" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632283434660469432" level="7" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632274041751875189" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632274041751875188" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632282702739375538" level="6" text="RecordAudio_Failed: OnRecordAudio_Failed">
        <node type="function" name="OnRecordAudio_Failed" id="632282702739375535" path="Metreos.StockTools" />
        <node type="event" name="RecordAudio_Failed" id="632282702739375534" path="Metreos.Providers.MediaServer.RecordAudio_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632283434660469436" level="7" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632274041751875184" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632274041751875183" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632284286984375440" level="8" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632274041751875207" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632274041751875206" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632284286984375441" level="8" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632274041751875212" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632274041751875211" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632283434660469437" level="7" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632274041751875189" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632274041751875188" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632279180679221097" level="5" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632274041751875189" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632274041751875188" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632284768439844195" level="5" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632274041751875207" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632274041751875206" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632284768439844196" level="5" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632274041751875212" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632274041751875211" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632274041751875192" level="3" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632274041751875189" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632274041751875188" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632279180679219015" level="4" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632274041751875207" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632274041751875206" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632279180679219016" level="4" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632274041751875212" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632274041751875211" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632274041751875172" level="2" text="AnswerCall_Failed: OnAnswerCall_Failed">
        <node type="function" name="OnAnswerCall_Failed" id="632274041751875169" path="Metreos.StockTools" />
        <node type="event" name="AnswerCall_Failed" id="632274041751875168" path="Metreos.CallControl.AnswerCall_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632274041751875177" level="2" text="Hangup: OnHangup">
        <node type="function" name="OnHangup" id="632274041751875174" path="Metreos.StockTools" />
        <node type="event" name="Hangup" id="632274041751875173" path="Metreos.CallControl.Hangup" />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632274041751875196" level="2" text="AnswerCall_Complete: OnAnswerCall_Complete">
        <node type="function" name="OnAnswerCall_Complete" id="632274041751875164" path="Metreos.StockTools" />
        <node type="event" name="AnswerCall_Complete" id="632274041751875163" path="Metreos.CallControl.AnswerCall_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632277456088750259" level="3" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632274041751875207" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632274041751875206" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632277456088750260" level="3" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632274041751875212" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632274041751875211" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632274041751875197" level="2" text="AnswerCall_Failed: OnAnswerCall_Failed">
        <node type="function" name="OnAnswerCall_Failed" id="632274041751875169" path="Metreos.StockTools" />
        <node type="event" name="AnswerCall_Failed" id="632274041751875168" path="Metreos.CallControl.AnswerCall_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632274041751875198" level="2" text="Hangup: OnHangup">
        <node type="function" name="OnHangup" id="632274041751875174" path="Metreos.StockTools" />
        <node type="event" name="Hangup" id="632274041751875173" path="Metreos.CallControl.Hangup" />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632279180679221066" level="1" text="PerformHangup">
        <node type="function" name="PerformHangup" id="632279180679221063" path="Metreos.StockTools" />
      </treenode>
    </outline>
    <variables>
      <treenode text="g_incomingConnectionId" id="632302536409689513" vid="632273883766250161">
        <Properties type="Metreos.Types.Int">g_incomingConnectionId</Properties>
      </treenode>
      <treenode text="g_incomingCallId" id="632302536409689515" vid="632274041751875217">
        <Properties type="Metreos.Types.String">g_incomingCallId</Properties>
      </treenode>
      <treenode text="g_CallerIp" id="632302536409689517" vid="632277635633125265">
        <Properties type="Metreos.Types.String">g_CallerIp</Properties>
      </treenode>
      <treenode text="g_CallerPort" id="632302536409689519" vid="632277635633125267">
        <Properties type="Metreos.Types.Int">g_CallerPort</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632302536409689521" vid="632277635633125682">
        <Properties type="Metreos.Types.Int">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_confNumber" id="632302536409689523" vid="632279180679219022">
        <Properties type="Metreos.Types.Int">g_confNumber</Properties>
      </treenode>
      <treenode text="g_InConference" id="632302536409689525" vid="632279180679219028">
        <Properties type="Metreos.Types.Bool">g_InConference</Properties>
      </treenode>
      <treenode text="g_ReceiveDigitsFailed" id="632302536409689527" vid="632279180679221093">
        <Properties type="Metreos.Types.Bool">g_ReceiveDigitsFailed</Properties>
      </treenode>
      <treenode text="g_ProcessDigits" id="632302536409689529" vid="632279180679221101">
        <Properties type="Metreos.Types.Bool">g_ProcessDigits</Properties>
      </treenode>
      <treenode text="g_dbConnected" id="632302536409689531" vid="632279345072304570">
        <Properties type="Metreos.Types.Bool">g_dbConnected</Properties>
      </treenode>
      <treenode text="g_AddingToConference" id="632302536409689533" vid="632279397050429492">
        <Properties type="Metreos.Types.Bool">g_AddingToConference</Properties>
      </treenode>
      <treenode text="g_ExitApp" id="632302536409689535" vid="632282702739376381">
        <Properties type="Metreos.Types.Bool">g_ExitApp</Properties>
      </treenode>
      <treenode text="g_appServerDn" id="632302536409689537" vid="632302536409688535">
        <Properties type="String" initWith="appServerDn">g_appServerDn</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632273883766250153" treenode="632273883766250154" appnode="632273883766250151" handlerfor="632273883766250150">
    <node type="Start" id="632273883766250153" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="289">
      <linkto id="632279180679221103" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632273883766250157" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="243" y="292">
      <linkto id="632274041751875199" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632273883766250163" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">localIncomingCallId</ap>
        <ap name="remotePort" type="literal">0</ap>
        <ap name="connectionId" type="literal">0</ap>
        <ap name="remoteIp" type="literal">0</ap>
        <rd field="connectionId">g_incomingConnectionId</rd>
        <rd field="port">mediaPortForIncoming</rd>
        <rd field="ipAddress">mediaIpForIncoming</rd>
        <log condition="failure" on="true" level="Info" type="literal">OnIncomingCall: Create connection failed... Exiting.</log>
        <log condition="default" on="true" level="Info" type="literal">OnIncomingCall: Default CreateConnection Log output.</log>
      </Properties>
    </node>
    <node type="Action" id="632273883766250163" name="SetMedia" class="MaxActionNode" group="" path="Metreos.CallControl" x="361" y="292">
      <linkto id="632274041751875178" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632274041751875201" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">localIncomingCallId</ap>
        <ap name="mediaPort" type="variable">mediaPortForIncoming</ap>
        <ap name="mediaIP" type="variable">mediaIpForIncoming</ap>
        <rd field="callId">g_incomingCallId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: MediaIpForIncoming: " + mediaIpForIncoming + " mediaPortForIncoming: " + mediaPortForIncoming + " connectionId for incoming: " + g_incomingConnectionId;</log>
        <log condition="failure" on="true" level="Info" type="literal">OnIncomingCall: Failed to set media on incoming connection...exiting.</log>
      </Properties>
    </node>
    <node type="Action" id="632274041751875178" name="AnswerCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="424" y="273" mx="495" my="289">
      <items count="3">
        <item text="OnAnswerCall_Complete" treenode="632274041751875167" />
        <item text="OnAnswerCall_Failed" treenode="632274041751875172" />
        <item text="OnHangup" treenode="632274041751875177" />
      </items>
      <linkto id="632274041751875195" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632274041751875202" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="answer" type="literal">true</ap>
        <ap name="callId" type="variable">localIncomingCallId</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Attempting to answer call with callId: " + localIncomingCallId + ".... Global incoming Call Id is: " + g_incomingCallId;</log>
        <log condition="failure" on="true" level="Info" type="literal">OnIncomingCall: Failed to answer call... exiting.</log>
      </Properties>
    </node>
    <node type="Action" id="632274041751875195" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="662" y="289">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632274041751875199" name="AnswerCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="176" y="432" mx="247" my="448">
      <items count="3">
        <item text="OnAnswerCall_Complete" treenode="632274041751875196" />
        <item text="OnAnswerCall_Failed" treenode="632274041751875197" />
        <item text="OnHangup" treenode="632274041751875198" />
      </items>
      <linkto id="632274041751875200" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="answer" type="literal">false</ap>
        <ap name="callId" type="variable">localIncomingCallId</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632274041751875200" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="245" y="611">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632274041751875201" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="365" y="449">
      <linkto id="632274041751875199" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632274041751875202" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="493" y="441">
      <linkto id="632274041751875200" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632279180679221103" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="134" y="292">
      <linkto id="632273883766250157" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_ProcessDigits)
	{
		g_ProcessDigits = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Comment" id="632282873043281617" text="initialize a boolean variable to true, request from the media server a&#xD;&#xA;connectionId, mediaIP and mediaPort for the incoming connection,&#xD;&#xA;and associates them with the incoming call. The CreateConnection returns&#xD;&#xA;the connectionId that the above settings are associated with.&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="191" />
    <node type="Comment" id="632282873043281618" text="if anything fails at this point,&#xD;&#xA;we delete connections&#xD;&#xA;to the media server, reject the&#xD;&#xA;call, and exit the script" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="329" y="588" />
    <node type="Comment" id="632282873043281619" text="answer the call, exit the function" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="502" y="227" />
    <node type="Variable" id="632273883766250156" name="incomingCallFrom" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="from" refType="reference">incomingCallFrom</Properties>
    </node>
    <node type="Variable" id="632273883766250158" name="localIncomingCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="callId" refType="reference">localIncomingCallId</Properties>
    </node>
    <node type="Variable" id="632273883766250159" name="mediaIpForIncoming" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">mediaIpForIncoming</Properties>
    </node>
    <node type="Variable" id="632276470660000238" name="mediaPortForIncoming" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Int" refType="reference">mediaPortForIncoming</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnAnswerCall_Complete" startnode="632274041751875166" treenode="632274041751875167" appnode="632274041751875164" handlerfor="632274041751875163">
    <node type="Start" id="632274041751875166" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="250">
      <linkto id="632277635633125269" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632274041751875181" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="306" y="254">
      <linkto id="632274041751875193" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632277456088750261" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">localCallId</ap>
        <ap name="remotePort" type="variable">incomingPort</ap>
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="remoteIp" type="variable">incomingMedia</ap>
      </Properties>
    </node>
    <node type="Action" id="632274041751875193" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="448" y="238" mx="541" my="254">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632274041751875187" />
        <item text="OnPlayAnnouncement_Failed" treenode="632274041751875192" />
      </items>
      <linkto id="632274041751875194" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632277456088750261" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="filename" type="literal">hello.wav</ap>
        <ap name="filename2" type="literal">enter_conference_pin.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">Entry into Play Announcement</log>
      </Properties>
    </node>
    <node type="Action" id="632274041751875194" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="843" y="253">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632277456088750261" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="246" y="398" mx="308" my="414">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632277456088750259" />
        <item text="OnHangup_Failed" treenode="632277456088750260" />
      </items>
      <linkto id="632277456088750262" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_incomingCallId</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632277456088750262" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="306" y="578">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632277635633125269" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="162" y="252">
      <linkto id="632274041751875181" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref string g_CallerIp, ref int g_CallerPort, string incomingMedia, int incomingPort)
	{
		g_CallerIp = incomingMedia;
		g_CallerPort = incomingPort;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Comment" id="632282873043281621" text="This function is entered if the call is successfully answered" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="242" y="32" />
    <node type="Comment" id="632282873043281622" text="We assign the media ip and port of the calling&#xD;&#xA;device to global variables. We use CreateConnection&#xD;&#xA;to associate the above with the mediaIp and mediaPort&#xD;&#xA;the MMS" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="158" />
    <node type="Comment" id="632282873043281623" text="If the createconnection fails at&#xD;&#xA;this point, we hang up the call&#xD;&#xA;and end function" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="58" y="440" />
    <node type="Comment" id="632282873043281624" text="if the media server and incoming client are properly connected,&#xD;&#xA;play intro prompt to the user" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="483.4707" y="177" />
    <node type="Variable" id="632274041751875179" name="incomingMedia" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="mediaIP" refType="reference">incomingMedia</Properties>
    </node>
    <node type="Variable" id="632274041751875180" name="incomingPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Int" initWith="mediaPort" refType="reference">incomingPort</Properties>
    </node>
    <node type="Variable" id="632283434660469478" name="localCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="callId" refType="reference">localCallId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Complete" startnode="632274041751875186" treenode="632274041751875187" appnode="632274041751875184" handlerfor="632274041751875183">
    <node type="Start" id="632274041751875186" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="38" y="366">
      <linkto id="632282702739376377" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277533806093991" name="ReceiveDigits" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="578" y="471" mx="656" my="487">
      <items count="2">
        <item text="OnReceiveDigits_Complete" treenode="632277533806093985" />
        <item text="OnReceiveDigits_Failed" treenode="632277533806093990" />
      </items>
      <linkto id="632279397050429129" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632283434660469444" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="termCondDigit" type="literal">#</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayAnnouncementComplete: Entering ReceiveDigits....</log>
      </Properties>
    </node>
    <node type="Action" id="632277533806094000" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="836.5742" y="371">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632279180679221100" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="654" y="371">
      <linkto id="632277533806093991" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632277533806094000" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">g_ProcessDigits</ap>
        <log condition="exit" on="true" level="Info" type="csharp">"OnPlayAnnouncement_Complete: g_ProcessDigits is: " + g_ProcessDigits;</log>
      </Properties>
    </node>
    <node type="Action" id="632279397050429129" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="837.5742" y="489">
      <linkto id="632277533806094000" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_ProcessDigits)
	{
		g_ProcessDigits = false;
		return IApp.VALUE_SUCCESS;

	}
</Properties>
    </node>
    <node type="Action" id="632279941842772863" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="454" y="370">
      <linkto id="632279180679221100" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632279941842772866" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">g_AddingToConference</ap>
        <log condition="exit" on="true" level="Info" type="csharp">"OnPlayAnnouncement_Complete: g_AddingToConference is: " + g_AddingToConference;</log>
      </Properties>
    </node>
    <node type="Action" id="632279941842772866" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="452" y="192">
      <linkto id="632282702739375539" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_AddingToConference)
	{
		g_AddingToConference = false;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632282702739375539" name="RecordAudio" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="580" y="177" mx="656" my="193">
      <items count="2">
        <item text="OnRecordAudio_Complete" treenode="632282702739375533" />
        <item text="OnRecordAudio_Failed" treenode="632282702739375538" />
      </items>
      <linkto id="632279180679221100" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632283434660469459" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxTime" type="literal">10000</ap>
        <ap name="termCondSilence" type="literal">9900</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="termCondDigit" type="literal">#</ap>
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632282702739376377" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="153" y="367">
      <linkto id="632279941842772863" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632284286984375443" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">g_ExitApp</ap>
        <log condition="exit" on="true" level="Info" type="csharp">"OnPlayAnnouncement_Complete: g_ExitApp is: " + g_ExitApp;</log>
      </Properties>
    </node>
    <node type="Action" id="632282702739376380" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="152" y="608">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632282873043281625" text="We read a boolean, g_ExitApp.&#xD;&#xA;if it is true, that means&#xD;&#xA;that we want to exit the application&#xD;&#xA;because of some error, we played and&#xD;&#xA;announcement to the user, and needed to wait for the&#xD;&#xA;announcement to complete before hanging up.&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="231" />
    <node type="Comment" id="632282873043281626" text="we go down this path when we're adding a user to conference, and the user&#xD;&#xA;is not the first one in. We set g_AddingToConferece to&#xD;&#xA;false so we don't go down this path more than necessary, record the users name&#xD;&#xA;and add him to the conference after the recording is complete,&#xD;&#xA;in OnRecordAudio_Complete" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="215.90625" y="45" />
    <node type="Comment" id="632282873043281627" text="We need to specify the encoding and format. We use a wav&#xD;&#xA;file encoded in ulaw. The terminating conditions are set to kick off&#xD;&#xA;a 'silence' terminating condition after 9900ms, and to cease recording&#xD;&#xA;after either 1000ms or after # is pressed" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="664.289063" y="39" />
    <node type="Comment" id="632282873043281629" text="if the boolean g_ProcessDigits is true, that means that at some other point in the app we&#xD;&#xA;wanted to process user input and take action based on what the user entered.&#xD;&#xA;These actions will be taken in OnReceiveDigits_Complete and Failed. We set g_ProcessDigits to&#xD;&#xA;false before we exit, so this branch is not taken if not necessary." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="777.9551" y="530" />
    <node type="Comment" id="632282873043281630" text="If all the booleans are false, we simply&#xD;&#xA;exit the function" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="887.9551" y="356" />
    <node type="Action" id="632283434660469444" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="652.8613" y="629">
      <linkto id="632283434660469446" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_ExitApp)
	{
		g_ExitApp = true;
		return IApp.VALUE_SUCCESS;
	}

</Properties>
    </node>
    <node type="Action" id="632283434660469446" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="562.9551" y="726" mx="656" my="742">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632283434660469431" />
        <item text="OnPlayAnnouncement_Failed" treenode="632283434660469432" />
      </items>
      <linkto id="632283434660469448" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632284286984375442" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="filename" type="literal">technical_difficulties.wav</ap>
        <ap name="filename2" type="literal">goodbye.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632283434660469448" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="652.9551" y="894">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632283434660469449" text="If the media operation fails, try to inform the user, and exit.&#xD;&#xA;If PlayAnnouncement fails, just hang up" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="301.955078" y="676" />
    <node type="Action" id="632283434660469459" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="801.8613" y="198">
      <linkto id="632283434660469460" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_ExitApp)
	{
		g_ExitApp = true;
		return IApp.VALUE_SUCCESS;
	}

</Properties>
    </node>
    <node type="Action" id="632283434660469460" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="835.9551" y="182" mx="929" my="198">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632283434660469431" />
        <item text="OnPlayAnnouncement_Failed" treenode="632283434660469432" />
      </items>
      <linkto id="632283434660469461" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632284286984375445" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="filename" type="literal">technical_difficulties.wav</ap>
        <ap name="filename2" type="literal">goodbye.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632283434660469461" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1113.95508" y="80">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632283434660469466" text="If the media operation fails, try to inform the user, and exit.&#xD;&#xA;If PlayAnnouncement fails, just hang up" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="996.9551" y="267" />
    <node type="Action" id="632284286984375442" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="792.664063" y="790" mx="855" my="806">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632284286984375440" />
        <item text="OnHangup_Failed" treenode="632284286984375441" />
      </items>
      <linkto id="632283434660469448" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_incomingCallId</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632284286984375443" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="93.66406" y="453" mx="156" my="469">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632284286984375440" />
        <item text="OnHangup_Failed" treenode="632284286984375441" />
      </items>
      <linkto id="632282702739376380" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_incomingCallId</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632284286984375445" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="1054.66406" y="180" mx="1117" my="196">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632284286984375440" />
        <item text="OnHangup_Failed" treenode="632284286984375441" />
      </items>
      <linkto id="632283434660469461" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_incomingCallId</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Variable" id="632277456088751688" name="terminationCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="terminationCondition" refType="reference">terminationCondition</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnReceiveDigits_Complete" startnode="632277533806093984" treenode="632277533806093985" appnode="632277533806093982" handlerfor="632277533806093981">
    <node type="Start" id="632277533806093984" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="393">
      <linkto id="632277533806093996" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277533806093996" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="148" y="392">
      <linkto id="632277656496563596" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="csharp">"OnReceiveDigits_Complete: Digits received: " + restOfDigits;</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete: Function Entry.</log>
      </Properties>
    </node>
    <node type="Action" id="632277545203907390" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1882.4707" y="514">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632277545203907392" name="CreateConnectionConference" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="1270" y="515">
      <linkto id="632283434660469424" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632279397050429310" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="conferenceId" type="literal">0</ap>
        <rd field="conferenceId">g_conferenceId</rd>
        <log condition="exit" on="true" level="Info" type="csharp">"OnReceiveDigits_Complete: Conference id was 0 but now is: " + g_conferenceId;</log>
      </Properties>
    </node>
    <node type="Action" id="632277635633125894" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1106" y="394">
      <linkto id="632277545203907392" type="Labeled" style="Bezier" ortho="true" label="0" />
      <linkto id="632279941842772867" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">g_conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632277656496563596" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="248" y="276">
      <linkto id="632279397050429128" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632284286984375449" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties language="csharp">
	public static string Execute(ref int g_confNumber, string restOfDigits)
	{
		string digits = restOfDigits.Replace("*",string.Empty);
		digits = digits.Replace("#",string.Empty);
		if (digits.Equals(string.Empty))
			return IApp.VALUE_FAILURE;

		g_confNumber = int.Parse(digits);
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632278257577031522" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="402" y="534">
      <linkto id="632279345072304573" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632282873043281863" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="DSN" type="literal">DSN=metreosMySQL;UID=metreos;PWD=metreos</ap>
        <ap name="Name" type="literal">dbconnection</ap>
        <ap name="Type" type="literal">odbc</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete: Opening database Connection...</log>
        <log condition="success" on="true" level="Info" type="literal">OnReceiveDigits_Complete: OpenDatabase successful</log>
        <log condition="failure" on="true" level="Error" type="literal">OnReceiveDigits_Complete: OpenDatabase: FAILURE OPENING DATABASE!</log>
      </Properties>
    </node>
    <node type="Action" id="632278345632812773" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="614" y="395">
      <linkto id="632278345632812778" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Query" type="csharp">"select * from scheduledconferences where confNumber = " + g_confNumber.ToString() + ";";</ap>
        <ap name="Name" type="literal">dbconnection</ap>
        <rd field="ResultSet">conferenceDBTable</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete: Retrieving data from database...</log>
      </Properties>
    </node>
    <node type="Action" id="632278345632812778" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="747" y="396">
      <linkto id="632279397050429128" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <linkto id="632282702739375960" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties language="csharp">
	public static string Execute(System.Data.DataTable conferenceDBTable, ref System.DateTime scheduledDate, ref int numberOfParticipants, ref int g_conferenceId, ref int numHours)
	{
		if (conferenceDBTable.Rows.Count == 0)
		{
			return IApp.VALUE_FAILURE;	
		}
		System.Data.DataRow row = conferenceDBTable.Rows[0];
		g_conferenceId = (int)row["confId"];
		numberOfParticipants = (int)row["numParticipants"];
		scheduledDate = (System.DateTime)row["scheduledFor"];
		numHours = (int)row["numHours"];
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632278345632812781" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="1758.94141" y="515">
      <linkto id="632277545203907390" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Query" type="csharp">"update scheduledconferences set confId=" + g_conferenceId + "," + "numParticipants=" + numberOfParticipants + " where confNumber=" + g_confNumber + ";";</ap>
        <ap name="Name" type="literal">dbconnection</ap>
      </Properties>
    </node>
    <node type="Action" id="632279180679219014" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1609.94141" y="516">
      <linkto id="632278345632812781" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref int numberOfParticipants, ref bool g_InConference)
	{
		numberOfParticipants++;
		g_InConference = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632279345072304568" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="654" y="118" mx="747" my="134">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632279345072304566" />
        <item text="OnPlayAnnouncement_Failed" treenode="632279345072304567" />
      </items>
      <linkto id="632279345072304569" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="filename" type="literal">conference_pin_invalid.wav</ap>
        <ap name="filename2" type="literal">enter_conference_pin.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632279345072304569" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="743" y="32">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632279345072304573" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="614" y="532">
      <linkto id="632278345632812773" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_dbConnected)
	{
		g_dbConnected = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632279345072304577" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="309" y="739" mx="402" my="755">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632279345072304575" />
        <item text="OnPlayAnnouncement_Failed" treenode="632279345072304576" />
      </items>
      <linkto id="632279397050429124" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632284809869219202" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="filename" type="literal">technical_difficulties.wav</ap>
        <ap name="filename2" type="literal">later.wav</ap>
        <ap name="filename3" type="literal">goodbye.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632279397050429124" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="401" y="912">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632279397050429128" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="744" y="273">
      <linkto id="632279345072304568" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_ProcessDigits)
	{
		g_ProcessDigits = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632279941842772867" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1291" y="382">
      <linkto id="632279941842773098" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_AddingToConference)
	{
		g_AddingToConference = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632279941842773098" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="1520" y="364" mx="1613" my="380">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632279941842773096" />
        <item text="OnPlayAnnouncement_Failed" treenode="632279941842773097" />
      </items>
      <linkto id="632279180679219014" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="filename" type="literal">state_name_company.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632282702739375960" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="885" y="395">
      <linkto id="632282771458750365" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_ExitApp, ref int numHours, ref System.DateTime scheduledDate)
	{
		System.DateTime minEntryTime = scheduledDate.AddMinutes(-15);
		if ((minEntryTime.CompareTo(System.DateTime.Now)) &lt; 0)
			g_ExitApp = false;
		else
			g_ExitApp = true;

		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632282702739376385" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="907" y="511" mx="1000" my="527">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632282702739376383" />
        <item text="OnPlayAnnouncement_Failed" treenode="632282702739376384" />
      </items>
      <linkto id="632282771458750367" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632284809869219200" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="filename" type="literal">not_yet_scheduled.wav</ap>
        <ap name="filename2" type="literal">goodbye.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnReceiveDigits_Complete: User attemted login too early. Notifying user and exiting...</log>
      </Properties>
    </node>
    <node type="Action" id="632282771458750367" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="998" y="730">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632282771458750365" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="999" y="393">
      <linkto id="632277635633125894" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632282702739376385" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_ExitApp</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnReceiveDigits_Complete: The value of g_ExitApp is: " + g_ExitApp;</log>
      </Properties>
    </node>
    <node type="Comment" id="632282873043281631" text="Here, we process the user input. We&#xD;&#xA;remove all *s and #s in from the input,&#xD;&#xA;using CustomCode. If the length of the user input is zero, then &#xD;&#xA;either the user failed to give proper input, or there was another failure&#xD;&#xA;and we take appropriate action." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="33" y="153" />
    <node type="Comment" id="632282873043281632" text="We check to see if we're connected to the database. &#xD;&#xA;If not, we open a connection using OpenDatabase,&#xD;&#xA;Specifying the DSN as DSN=metreosMySQL;&#xD;&#xA;UID=metreos;PWD=metreos where metreosMySQL is the &#xD;&#xA;name of the ODBC component, UID the username,&#xD;&#xA;and PWD the password. We specify 'dbconnection' as the &#xD;&#xA;connection name that we will use to refer to this connection&#xD;&#xA;in further database interaction. If the action fails, we play&#xD;&#xA;an announcement to the user and exit the script." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="646" />
    <node type="Action" id="632282873043281863" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="400" y="648">
      <linkto id="632279345072304577" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_ExitApp)
	{
		g_ExitApp = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Comment" id="632282873043281866" text="We set g_dbConnected to True" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="535" y="578" />
    <node type="Comment" id="632282873043281867" text="Look at comment #1&#xD;&#xA;on bottom of canvas" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="541" y="335" />
    <node type="Comment" id="632282873043281868" text="#1. here we send a query to the database. We use dbconnection to refer to&#xD;&#xA;the connection we opened previously, specify the query using the user input and put the results in&#xD;&#xA;the specified variable we created" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="587" y="950" />
    <node type="Comment" id="632282873043281869" text="Comment #2&#xD;&#xA;on bottom" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="695" y="428" />
    <node type="Comment" id="632282873043281870" text="#2. Here we get data we need out of the values returned from the database, and cast them&#xD;&#xA;into the proper type. If the number of rows returned was 0, we return failure - this means that&#xD;&#xA;the conference pin was not found for some reason. Otherwise, we get the actual conference number,&#xD;&#xA;date scheduled for, and number of participants out of the result" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="587" y="1016" />
    <node type="Comment" id="632282873043281871" text="Here we check to see if the&#xD;&#xA;user is logging in withing 15&#xD;&#xA;minutes of the scheduled time" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="870" y="314" />
    <node type="Comment" id="632282873043281872" text="The user logged in too early, we play an announcement,&#xD;&#xA;g_ExitApp already set in the CustomCode, which will cause&#xD;&#xA;the application to exit aftert the announcement is done playing" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="830" y="773" />
    <node type="Comment" id="632282873043281873" text="We check to see if this user is the first one in. We do this by seeing&#xD;&#xA;if the conferenceId we pulled out of the database is zero.&#xD;&#xA;If so, we create a mediaServer conferenceId, play a welcoming &#xD;&#xA;announcement to the user, set the global g_conferenceId" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1392" y="590" />
    <node type="Comment" id="632282873043281874" text="If we branch here, the user is not the first one in. We set&#xD;&#xA;g_AddingToConference to true, for use in &#xD;&#xA;OnPlayAnnouncement_Complete, play the announcement &#xD;&#xA;asking the user for his name and company information. &#xD;&#xA;He will be added to the conference in OnRecordAudio_Complete." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1229" y="269" />
    <node type="Comment" id="632282873043281875" text="We set the boolean g_InConference to signify that we're conferenced, and send a query to the database&#xD;&#xA;associated to the connection 'dbconnection' updating the number of participants currently in the conferece,&#xD;&#xA;then we exit" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1620.20178" y="299" />
    <node type="Comment" id="632282873043281876" text="We turn digit processing on by setting g_ProcessDigits (OnPlayAnnouncement_Complete&#xD;&#xA;checks if it's set, and calls ReceiveDigits accordingly), PlayAnnouncement specifying that&#xD;&#xA;user input was invalid, exit this function" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="814" y="32" />
    <node type="Action" id="632283434660469424" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1270" y="615">
      <linkto id="632283434660469427" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_ExitApp)
	{
		g_ExitApp = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632283434660469427" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="1204" y="698" mx="1297" my="714">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632283434660469425" />
        <item text="OnPlayAnnouncement_Failed" treenode="632283434660469426" />
      </items>
      <linkto id="632283434660469428" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632284809869219197" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="filename" type="literal">technical_difficulties.wav</ap>
        <ap name="filename2" type="literal">goodbye.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632283434660469428" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1294" y="853">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632283434660469429" text="If we could not create the conference for whatever&#xD;&#xA;reason, we try to inform the user, and exit the application" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1419" y="844" />
    <node type="Action" id="632284286984375449" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="401" y="397">
      <linkto id="632278345632812773" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632278257577031522" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_dbConnected</ap>
      </Properties>
    </node>
    <node type="Action" id="632279397050429310" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="1361.94141" y="499" mx="1455" my="515">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632283434660469436" />
        <item text="OnPlayAnnouncement_Failed" treenode="632274041751875192" />
      </items>
      <linkto id="632279180679219014" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="filename" type="literal">welcome_to_conference.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632284809869219197" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="1433" y="699" mx="1495" my="715">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632284809869219195" />
        <item text="OnHangup_Failed" treenode="632284809869219196" />
      </items>
      <linkto id="632283434660469428" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_incomingCallId</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632284809869219200" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="755" y="515" mx="817" my="531">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632284809869219195" />
        <item text="OnHangup_Failed" treenode="632284809869219196" />
      </items>
      <linkto id="632282771458750367" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_incomingCallId</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632284809869219202" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="541" y="740" mx="603" my="756">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632284809869219195" />
        <item text="OnHangup_Failed" treenode="632284809869219196" />
      </items>
      <linkto id="632279397050429124" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_incomingCallId</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Variable" id="632277533806093995" name="restOfDigits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="digits" refType="reference">restOfDigits</Properties>
    </node>
    <node type="Variable" id="632278345632812776" name="conferenceDBTable" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.DataTable" refType="reference">conferenceDBTable</Properties>
    </node>
    <node type="Variable" id="632278345632812779" name="scheduledDate" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.DateTime" refType="reference">scheduledDate</Properties>
    </node>
    <node type="Variable" id="632278345632812780" name="numberOfParticipants" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Int" refType="reference">numberOfParticipants</Properties>
    </node>
    <node type="Variable" id="632282702739375961" name="numHours" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Int" refType="reference">numHours</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Failed" startnode="632274041751875191" treenode="632279345072304567" appnode="632274041751875189" handlerfor="632274041751875188">
    <node type="Start" id="632274041751875191" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="316">
      <linkto id="632282873043281864" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632274041751875205" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="302" y="192">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632279180679219017" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="242" y="298" mx="304" my="314">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632279180679219015" />
        <item text="OnHangup_Failed" treenode="632279180679219016" />
      </items>
      <linkto id="632274041751875205" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_incomingCallId</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="exit" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed: Hanging up user...</log>
      </Properties>
    </node>
    <node type="Action" id="632282873043281864" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="169" y="316">
      <linkto id="632279180679219017" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632274041751875205" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_ExitApp</ap>
      </Properties>
    </node>
    <node type="Comment" id="632282873043281865" text="If the PlayAnnouncement failed,&#xD;&#xA;we check to see if g_ExitApp is set,&#xD;&#xA;if so, we hangup the call and exit function. Otherwise&#xD;&#xA;we just exit the function." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="383" y="239" />
  </canvas>
  <canvas type="Function" name="OnHangup_Complete" startnode="632274041751875209" treenode="632284809869219195" appnode="632274041751875207" handlerfor="632274041751875206">
    <node type="Start" id="632274041751875209" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32.9981232" y="300">
      <linkto id="632279180679221091" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632279180679221091" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="153.642578" y="282" mx="200" my="298">
      <items count="1">
        <item text="PerformHangup" treenode="632284809869219195" />
      </items>
      <linkto id="632279180679221092" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="callId" type="variable">callIdToHangup</ap>
        <ap name="FunctionName" type="literal">PerformHangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632279180679221092" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="380" y="300">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632282873043281877" text="We call a custom function 'PerformHangup' specifying the callId that was hung up, end exit this function" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="33" y="210" />
    <node type="Variable" id="632279180679221090" name="callIdToHangup" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="callId" refType="reference">callIdToHangup</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup_Failed" startnode="632274041751875214" treenode="632284809869219196" appnode="632274041751875212" handlerfor="632274041751875211">
    <node type="Start" id="632274041751875214" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="296">
      <linkto id="632284768439844200" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632274041751875221" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="481" y="301">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632282873043281878" text="We call PerformHangup to delete the connections" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="194" />
    <node type="Action" id="632284768439844200" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="239.642578" y="282" mx="286" my="298">
      <items count="1">
        <item text="PerformHangup" treenode="632284809869219196" />
      </items>
      <linkto id="632274041751875221" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="callId" type="variable">g_incomingCallId</ap>
        <ap name="FunctionName" type="literal">PerformHangup</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnReceiveDigits_Failed" startnode="632277533806093989" treenode="632277533806093990" appnode="632277533806093987" handlerfor="632277533806093986">
    <node type="Start" id="632277533806093989" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="437">
      <linkto id="632279180679221095" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277533806093994" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="844" y="441">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632279180679221095" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="409" y="439">
      <linkto id="632279180679221098" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_ProcessDigits, ref bool g_ExitApp)
	{
		g_ProcessDigits = false;
		g_ExitApp = true;
		return IApp.VALUE_SUCCESS;

	}
</Properties>
    </node>
    <node type="Action" id="632279180679221098" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="469" y="425" mx="562" my="441">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632279180679221096" />
        <item text="OnPlayAnnouncement_Failed" treenode="632279180679221097" />
      </items>
      <linkto id="632277533806093994" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632284768439844197" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="filename" type="literal">technical_difficulties.wav</ap>
        <ap name="filename2" type="literal">goodbye.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Comment" id="632282873043281880" text="If ReceiveDigits fails, we prevent ReceiveDigits from being called&#xD;&#xA;again by PlayAnnouncement and set g_ExitApp, we PlayAnnouncement saying that there&#xD;&#xA;were technical difficulties, and since the g_ExitApp flag is set, the call will be hung up when&#xD;&#xA;the announcement is done playing." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="446" y="329" />
    <node type="Action" id="632284768439844197" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="501" y="568" mx="563" my="584">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632284768439844195" />
        <item text="OnHangup_Failed" treenode="632284768439844196" />
      </items>
      <linkto id="632277533806093994" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_incomingCallId</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecordAudio_Complete" startnode="632282702739375532" treenode="632282702739375533" appnode="632282702739375530" handlerfor="632282702739375529">
    <node type="Start" id="632282702739375532" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="300">
      <linkto id="632282702739375749" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632282702739375545" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="389" y="142" mx="482" my="158">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632282702739375543" />
        <item text="OnPlayAnnouncement_Failed" treenode="632282702739375544" />
      </items>
      <linkto id="632282702739375750" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="conferenceId" type="variable">g_conferenceId</ap>
        <ap name="filename" type="variable">fileName</ap>
        <ap name="filename2" type="literal">has_entered_conference.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632282702739375548" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="384" y="403" mx="477" my="419">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632283434660469436" />
        <item text="OnPlayAnnouncement_Failed" treenode="632274041751875192" />
      </items>
      <linkto id="632282702739375750" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="conferenceId" type="variable">g_conferenceId</ap>
        <ap name="filename" type="literal">caller_entered_conference.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632282702739375749" name="CreateConnectionConference" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="182" y="300">
      <linkto id="632282702739375540" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632283434660469430" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="conferenceId" type="variable">g_conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632282702739375750" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="673" y="295">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632282873043281881" text="We switch on the terminating condition of the RecordAudio action.&#xD;&#xA;If it terminated because it detected the specified length of silence, &#xD;&#xA;we play a standard prompt, otherwise we play the intro recorder by&#xD;&#xA;by the caller to the conference, using the filename passed into this&#xD;&#xA;event handler, which we assigned to the local variable fileName" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="362" y="43" />
    <node type="Comment" id="632282873043281882" text="Generic announcement" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="590" y="419" />
    <node type="Comment" id="632282873043281883" text="We add the user to the conference" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="81.4707" y="232" />
    <node type="Action" id="632282702739375540" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="324" y="299">
      <linkto id="632282702739375545" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632282702739375548" type="Labeled" style="Bezier" ortho="true" label="silence" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632283434660469430" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="182.4707" y="394">
      <linkto id="632283434660469433" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_ExitApp)
	{
		g_ExitApp = true;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632283434660469433" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="92.4707" y="458" mx="186" my="474">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632283434660469431" />
        <item text="OnPlayAnnouncement_Failed" treenode="632283434660469432" />
      </items>
      <linkto id="632283434660469434" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632284809869219204" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="filename" type="literal">technical_difficulties.wav</ap>
        <ap name="filename2" type="literal">goodbye.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632283434660469434" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="185.4707" y="614">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632284809869219204" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="299" y="506" mx="361" my="522">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632279180679219015" />
        <item text="OnHangup_Failed" treenode="632279180679219016" />
      </items>
      <linkto id="632283434660469434" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_incomingCallId</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="exit" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed: Hanging up user...</log>
      </Properties>
    </node>
    <node type="Variable" id="632282702739375541" name="fileName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="filename" refType="reference">fileName</Properties>
    </node>
    <node type="Variable" id="632282702739375542" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="terminationCondition" refType="reference">termCond</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecordAudio_Failed" startnode="632282702739375537" treenode="632282702739375538" appnode="632282702739375535" handlerfor="632282702739375534">
    <node type="Start" id="632282702739375537" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="264">
      <linkto id="632282702739375753" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632282702739375753" name="CreateConnectionConference" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="232" y="266">
      <linkto id="632283434660469435" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632282702739375751" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="conferenceId" type="variable">g_conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632282702739375754" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="629" y="265">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632282873043281884" text="If RecordAudio returned failed, we play a generic user entry into conference announcement,&#xD;&#xA;add the user to the conference and exit function. This way, even if the user is unable to speak&#xD;&#xA;because of some media problem, he may at least listen" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="104" y="150" />
    <node type="Action" id="632283434660469435" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="232" y="366">
      <linkto id="632283434660469438" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref bool g_ExitApp)
	{
		g_ExitApp = true;
		return IApp.VALUE_SUCCESS;
	}

</Properties>
    </node>
    <node type="Action" id="632283434660469438" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="140" y="455" mx="233" my="471">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632283434660469436" />
        <item text="OnPlayAnnouncement_Failed" treenode="632283434660469437" />
      </items>
      <linkto id="632283434660469439" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632284809869219206" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="filename" type="literal">technical_difficulties.wav</ap>
        <ap name="filename2" type="literal">goodbye.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632283434660469439" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="230" y="616">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632283434660469441" text="If Adding user to conference fails, try to&#xD;&#xA;play prompt and exit, otherwise just exit" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="539" y="520" />
    <node type="Action" id="632282702739375751" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="362" y="250" mx="455" my="266">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632283434660469436" />
        <item text="OnPlayAnnouncement_Failed" treenode="632274041751875192" />
      </items>
      <linkto id="632282702739375754" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="conferenceId" type="variable">g_conferenceId</ap>
        <ap name="filename" type="literal">caller_entered_conference.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632284809869219206" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="380" y="504" mx="442" my="520">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632279180679219015" />
        <item text="OnHangup_Failed" treenode="632279180679219016" />
      </items>
      <linkto id="632283434660469439" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">g_incomingCallId</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="exit" on="true" level="Info" type="literal">OnPlayAnnouncement_Failed: Hanging up user...</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnAnswerCall_Failed" startnode="632274041751875171" treenode="632274041751875172" appnode="632274041751875169" handlerfor="632274041751875168">
    <node type="Start" id="632274041751875171" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="292">
      <linkto id="632274041751875224" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632274041751875224" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="189" y="292">
      <linkto id="632274041751875225" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632274041751875225" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="408" y="290">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632282873043281885" text="If the AnswerCall action failed, we delete the MMS connection and exit the app" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="199" />
  </canvas>
  <canvas type="Function" name="OnHangup" startnode="632274041751875176" treenode="632274041751875177" appnode="632274041751875174" handlerfor="632274041751875173">
    <node type="Start" id="632274041751875176" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="299">
      <linkto id="632279180679221067" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632274041751875222" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="472" y="299">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632279180679221067" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="204.642578" y="282" mx="251" my="298">
      <items count="1">
        <item text="PerformHangup" treenode="632274041751875177" />
      </items>
      <linkto id="632274041751875222" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="callId" type="variable">hangingUpCallId</ap>
        <ap name="FunctionName" type="literal">PerformHangup</ap>
      </Properties>
    </node>
    <node type="Comment" id="632282873043281886" text="This gets called when the user hangs up. We call the&#xD;&#xA;custom function 'PerformHangup' passing in the callId of&#xD;&#xA;the hanging up user" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="100" y="206" />
    <node type="Variable" id="632279180679219475" name="hangingUpCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="callId" refType="reference">hangingUpCallId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="PerformHangup" startnode="632279180679221065" treenode="632279180679221066" appnode="632279180679221063" handlerfor="632274041751875173">
    <node type="Start" id="632279180679221065" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="528">
      <linkto id="632279180679221075" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632279180679221070" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="282.001862" y="281">
      <linkto id="632279180679221071" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Query" type="csharp">"select * from scheduledconferences where confNumber=" + g_confNumber;</ap>
        <ap name="Name" type="literal">dbconnection</ap>
        <rd field="ResultSet">queryResult</rd>
        <log condition="entry" on="true" level="Info" type="literal">PerformHangup: Retrieving information from database...</log>
      </Properties>
    </node>
    <node type="Action" id="632279180679221071" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="392.0019" y="281">
      <linkto id="632279180679221072" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(System.Data.DataTable queryResult, ref int numParticipants)
	{
		if (queryResult.Rows.Count == 0)
			return IApp.VALUE_FAILURE;
		
		System.Data.DataRow row = queryResult.Rows[0];
		numParticipants = (int)row["numParticipants"];
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632279180679221072" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="501.001831" y="282">
      <linkto id="632279180679221076" type="Labeled" style="Bezier" ortho="true" label="1" />
      <linkto id="632279180679221077" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">numParticipants</ap>
        <log condition="entry" on="true" level="Info" type="literal">PerformHangup: checking whether there is more than one person in conference...</log>
      </Properties>
    </node>
    <node type="Action" id="632279180679221073" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="604.0019" y="530">
      <linkto id="632283434660469483" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"PerformHangup: Removing connection, user not in conference. The connection id being removed is: " + g_incomingConnectionId;</log>
      </Properties>
    </node>
    <node type="Action" id="632279180679221075" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="284.001862" y="529">
      <linkto id="632279180679221070" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632279180679221073" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">g_InConference</ap>
        <log condition="entry" on="true" level="Info" type="literal">PerformHangup: Hangup successfull... removing connections to MMS.</log>
      </Properties>
    </node>
    <node type="Action" id="632279180679221076" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="621.730347" y="179">
      <linkto id="632283434660469479" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Query" type="csharp">"delete from scheduledconferences where confNumber=" + g_confNumber +";";</ap>
        <ap name="Name" type="literal">dbconnection</ap>
      </Properties>
    </node>
    <node type="Action" id="632279180679221077" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="501.001831" y="374">
      <linkto id="632283434660469480" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Query" type="csharp">"update scheduledconferences set numParticipants=" + (numParticipants-1) + " where confNumber=" + g_confNumber + ";";</ap>
        <ap name="Name" type="literal">dbconnection</ap>
      </Properties>
    </node>
    <node type="Action" id="632279397050429307" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="732.944" y="355" mx="826" my="371">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632279180679221096" />
        <item text="OnPlayAnnouncement_Failed" treenode="632274041751875192" />
      </items>
      <linkto id="632283434660469485" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="conferenceId" type="variable">g_conferenceId</ap>
        <ap name="filename" type="literal">caller_exited_conference.wav</ap>
        <ap name="audioFileEncoding" type="literal">ulaw</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Comment" id="632282873043281887" text="We check to see if there is a conference. If so, we need to either&#xD;&#xA;destroy it, or remove a user from it" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="142" y="573" />
    <node type="Comment" id="632282873043281888" text="user was not in conferece, so just&#xD;&#xA;delete media server connection and exit" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="575.0781" y="575" />
    <node type="Comment" id="632282873043281889" text="The user is in a conference. We query the database&#xD;&#xA;to see how many participants are in the conference. &#xD;&#xA;We pull data out of the query using the CustomCode node.&#xD;&#xA;We then switch on the number of participants in the conference" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="86" y="168" />
    <node type="Comment" id="632282873043281890" text="If There is only one, we delete the entry for the conference from the database,&#xD;&#xA;and call DeleteConnection, connectionId. This creates an empty conference which is automatically removed. " class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="564.977539" y="109" />
    <node type="Action" id="632283434660469479" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="928.0781" y="179">
      <linkto id="632283434660469486" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"PerformHangup: Attempting to delete conference with conferenceId: " + g_conferenceId;</log>
      </Properties>
    </node>
    <node type="Action" id="632283434660469480" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="643.0781" y="373">
      <linkto id="632279397050429307" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">g_incomingConnectionId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"PerformHangup: conference not empty, removing user with connectionId " + g_incomingConnectionId;</log>
      </Properties>
    </node>
    <node type="Action" id="632283434660469483" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="774.944" y="530">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632283434660469485" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="978.944" y="372">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632283434660469486" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1127.944" y="177">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632282873043281891" text="If there is more than one participant, we decrease the number of participants in the&#xD;&#xA;database, call DeleteConnection specifying connectionId, which&#xD;&#xA;removes the user connection from the conference and removes the connection, play an announcement to the &#xD;&#xA;conference saying that a user left, then exit the app" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="579.977539" y="274" />
    <node type="Variable" id="632279345072304108" name="queryResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.DataTable" refType="reference">queryResult</Properties>
    </node>
    <node type="Variable" id="632279345072304109" name="numParticipants" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Int" refType="reference">numParticipants</Properties>
    </node>
    <node type="Variable" id="632279345072304110" name="hungUpCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="callId" refType="reference">hungUpCallId</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>