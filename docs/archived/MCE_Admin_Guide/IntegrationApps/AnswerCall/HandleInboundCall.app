<Application name="HandleInboundCall" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="HandleInboundCall">
    <outline>
      <treenode type="evh" id="632930018989531683" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632930018989531680" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632930018989531679" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632930018989531700" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632930018989531697" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632930018989531696" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632968511415200489" actid="632930018989531706" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632930018989531705" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632930018989531702" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632930018989531701" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632968511415200490" actid="632930018989531706" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632930018989531733" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632930018989531730" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632930018989531729" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632968511415200354" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632968511415200351" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632968511415200350" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632968511415200359" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632968511415200356" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632968511415200355" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632968511415200364" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632968511415200361" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632968511415200360" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="632968511415200483" vid="632930018989531685">
        <Properties type="String">g_callId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632930018989531682" treenode="632930018989531683" appnode="632930018989531680" handlerfor="632930018989531679">
    <node type="Start" id="632930018989531682" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="52" y="434">
      <linkto id="632930018989531687" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632930018989531687" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="249" y="433">
      <linkto id="632930018989531689" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632930018989531706" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DisplayName" type="literal">HandleInboundCall</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="ConnectionId">connId</rd>
        <log condition="entry" on="true" level="Info" type="literal">Attempting to answer incoming call</log>
      </Properties>
    </node>
    <node type="Action" id="632930018989531689" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="248.360046" y="640">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="literal">The AnswerCall application did not successfully answer the call...</log>
      </Properties>
    </node>
    <node type="Action" id="632930018989531693" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="668.360046" y="430">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">The play command in AnswerCall was initiated successfully...</log>
      </Properties>
    </node>
    <node type="Action" id="632930018989531706" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="392" y="416" mx="445" my="432">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632930018989531700" />
        <item text="OnPlay_Failed" treenode="632930018989531705" />
      </items>
      <linkto id="632930018989531693" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632930018989531710" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">makecall_good_bye.wav</ap>
        <ap name="Prompt3" type="literal">makecall_good_bye.wav</ap>
        <ap name="Prompt1" type="literal">makecall_good_bye.wav</ap>
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="ResultCode">resultCode</rd>
        <log condition="entry" on="true" level="Info" type="literal">The AnswerCall application successfully answered the call... Now attempting to play media</log>
      </Properties>
    </node>
    <node type="Action" id="632930018989531710" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="440.942047" y="646">
      <linkto id="632930018989531711" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="entry" on="true" level="Error" type="csharp">"The play command in AnswerCall was not initiated successfully.  The 'resultcode' for the failed play operation was: " + resultCode</log>
      </Properties>
    </node>
    <node type="Action" id="632930018989531711" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="439.942047" y="795">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632930018989531684" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632930018989531709" name="connId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" refType="reference">connId</Properties>
    </node>
    <node type="Variable" id="632930018989531714" name="resultCode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" refType="reference">resultCode</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632930018989531699" treenode="632930018989531700" appnode="632930018989531697" handlerfor="632930018989531696">
    <node type="Start" id="632930018989531699" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="65" y="449">
      <linkto id="632930018989531717" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632930018989531715" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="413.942078" y="445">
      <linkto id="632930018989531716" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
        <log condition="entry" on="true" level="Info" type="literal">The play in the AnswerCall application completed successfully.  Hanging up the called party...</log>
      </Properties>
    </node>
    <node type="Action" id="632930018989531716" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="592.942" y="443">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632930018989531717" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="212.942078" y="448">
      <linkto id="632930018989531715" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632930018989531718" type="Labeled" style="Bezier" ortho="true" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">endReason</ap>
      </Properties>
    </node>
    <node type="Action" id="632930018989531718" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="215.942078" y="671">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">The call ended for reason autostop in the AnswerCall application, which indicates that the party hung up.</log>
      </Properties>
    </node>
    <node type="Variable" id="632930018989531723" name="endReason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Play_Complete">endReason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632930018989531704" treenode="632930018989531705" appnode="632930018989531702" handlerfor="632930018989531701">
    <node type="Start" id="632930018989531704" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="77" y="463">
      <linkto id="632930018989531724" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632930018989531724" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="211.123672" y="462">
      <linkto id="632930018989531725" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
        <log condition="entry" on="true" level="Info" type="literal">"The play in the AnswerCall application did not complete successfully.  The result code reported was: " + resultCode</log>
      </Properties>
    </node>
    <node type="Action" id="632930018989531725" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="423.123657" y="458">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632930018989531728" name="resultCode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ResultCode" refType="reference" name="Metreos.MediaControl.Play_Failed">resultCode</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632930018989531732" treenode="632930018989531733" appnode="632930018989531730" handlerfor="632930018989531729">
    <node type="Start" id="632930018989531732" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="177" y="458">
      <linkto id="632930018989531734" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632930018989531734" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="402.942078" y="464">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">The called party in the AnswerCall application hung up.</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632968511415200353" treenode="632968511415200354" appnode="632968511415200351" handlerfor="632968511415200350">
    <node type="Start" id="632968511415200353" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632968511415200365" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632968511415200365" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="486" y="233">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632968511415200358" treenode="632968511415200359" appnode="632968511415200356" handlerfor="632968511415200355">
    <node type="Start" id="632968511415200358" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632968511415200366" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632968511415200366" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="548" y="299">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632968511415200363" treenode="632968511415200364" appnode="632968511415200361" handlerfor="632968511415200360">
    <node type="Start" id="632968511415200363" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632968511415200367" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632968511415200367" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="473" y="289">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>