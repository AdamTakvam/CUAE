<Application name="Reset" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Reset">
    <outline>
      <treenode type="evh" id="632739582130944930" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632739582130944927" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632739582130944926" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632739582130944940" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632739582130944937" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632739582130944936" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632743464491504623" actid="632739582130944946" />
          <ref id="632743464491504639" actid="632739582130945084" />
          <ref id="632743464491504652" actid="632742373427124767" />
          <ref id="632743464491504658" actid="632743294726239627" />
          <ref id="632743464491504680" actid="632739582130945098" />
          <ref id="632743464491504686" actid="632739582130945108" />
          <ref id="632743464491504695" actid="632739582130945120" />
          <ref id="632743464491504705" actid="632743441199793185" />
          <ref id="632743464491504717" actid="632739582130945056" />
          <ref id="632743464491504733" actid="632739582130945001" />
          <ref id="632743464491504736" actid="632739582130945004" />
          <ref id="632743464491504746" actid="632739582130945019" />
          <ref id="632743464491504749" actid="632739582130945020" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632739582130944945" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632739582130944942" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632739582130944941" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632743464491504624" actid="632739582130944946" />
          <ref id="632743464491504640" actid="632739582130945084" />
          <ref id="632743464491504653" actid="632742373427124767" />
          <ref id="632743464491504659" actid="632743294726239627" />
          <ref id="632743464491504681" actid="632739582130945098" />
          <ref id="632743464491504687" actid="632739582130945108" />
          <ref id="632743464491504696" actid="632739582130945120" />
          <ref id="632743464491504706" actid="632743441199793185" />
          <ref id="632743464491504718" actid="632739582130945056" />
          <ref id="632743464491504734" actid="632739582130945001" />
          <ref id="632743464491504737" actid="632739582130945004" />
          <ref id="632743464491504747" actid="632739582130945019" />
          <ref id="632743464491504750" actid="632739582130945020" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632739582130944966" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632739582130944963" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632739582130944962" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632743464491504630" actid="632739582130944972" />
          <ref id="632743464491504643" actid="632739582130945088" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632739582130944971" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632739582130944968" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632739582130944967" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632743464491504631" actid="632739582130944972" />
          <ref id="632743464491504644" actid="632739582130945088" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632739582130945041" level="2" text="Metreos.MediaControl.Record_Complete: OnRecord_Complete">
        <node type="function" name="OnRecord_Complete" id="632739582130945038" path="Metreos.StockTools" />
        <node type="event" name="Record_Complete" id="632739582130945037" path="Metreos.MediaControl.Record_Complete" />
        <references>
          <ref id="632743464491504634" actid="632739582130945047" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632739582130945046" level="2" text="Metreos.MediaControl.Record_Failed: OnRecord_Failed">
        <node type="function" name="OnRecord_Failed" id="632739582130945043" path="Metreos.StockTools" />
        <node type="event" name="Record_Failed" id="632739582130945042" path="Metreos.MediaControl.Record_Failed" />
        <references>
          <ref id="632743464491504635" actid="632739582130945047" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632743294726239836" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632743294726239833" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632743294726239832" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632739582130944993" level="1" text="RequestVoicePrint">
        <node type="function" name="RequestVoicePrint" id="632739582130944990" path="Metreos.StockTools" />
        <calls>
          <ref actid="632739582130944989" />
          <ref actid="632739582130945074" />
        </calls>
      </treenode>
      <treenode type="fun" id="632739582130945011" level="1" text="RequestEmployeeId">
        <node type="function" name="RequestEmployeeId" id="632739582130945008" path="Metreos.StockTools" />
        <calls>
          <ref actid="632742373427124765" />
          <ref actid="632739582130945007" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_connectionId" id="632743464491504591" vid="632739582130944934">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="prompt_greeting" id="632743464491504593" vid="632739582130944949">
        <Properties type="String" initWith="prompt_greeting">prompt_greeting</Properties>
      </treenode>
      <treenode text="prompt_employeeId" id="632743464491504595" vid="632739582130944951">
        <Properties type="String" initWith="prompt_employeeId">prompt_employeeId</Properties>
      </treenode>
      <treenode text="prompt_provideVoicePrint" id="632743464491504597" vid="632739582130944953">
        <Properties type="String" initWith="prompt_provideVoicePrint">prompt_provideVoicePrint</Properties>
      </treenode>
      <treenode text="prompt_verifying" id="632743464491504599" vid="632739582130944955">
        <Properties type="String" initWith="prompt_verifying">prompt_verifying</Properties>
      </treenode>
      <treenode text="prompt_pressDigitToResetPassword" id="632743464491504601" vid="632739582130944957">
        <Properties type="String" initWith="prompt_pressDigitToResetPassword">prompt_pressDigitToResetPassword</Properties>
      </treenode>
      <treenode text="g_employeeId" id="632743464491504605" vid="632739582130944982">
        <Properties type="String">g_employeeId</Properties>
      </treenode>
      <treenode text="g_callId" id="632743464491504607" vid="632739582130945013">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="config_silenceMs" id="632743464491504609" vid="632739582130945050">
        <Properties type="UInt" initWith="config_silenceMs">config_silenceMs</Properties>
      </treenode>
      <treenode text="config_recordMaxTimeMs" id="632743464491504611" vid="632739582130945052">
        <Properties type="UInt" initWith="config_recordMaxTimeMs">config_recordMaxTimeMs</Properties>
      </treenode>
      <treenode text="g_newPassword" id="632743464491504613" vid="632739582130945131">
        <Properties type="String" initWith="config_newPassword">g_newPassword</Properties>
      </treenode>
      <treenode text="config_employeeId" id="632743464491504615" vid="632742373427124400">
        <Properties type="String" initWith="employeeId">config_employeeId</Properties>
      </treenode>
      <treenode text="config_ntUsername" id="632743464491504617" vid="632742373427124756">
        <Properties type="String" initWith="ntUsername">config_ntUsername</Properties>
      </treenode>
      <treenode text="prompt_newPassword" id="632743464491504619" vid="632742373427124758">
        <Properties type="String" initWith="prompt_newPassword">prompt_newPassword</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632739582130944929" treenode="632739582130944930" appnode="632739582130944927" handlerfor="632739582130944926">
    <node type="Start" id="632739582130944929" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="55" y="266">
      <linkto id="632739582130944932" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632739582130944932" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="176" y="267">
      <linkto id="632739582130944946" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DisplayName" type="literal">Password Reset</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="ConnectionId">g_connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632739582130944946" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="244" y="251" mx="297" my="267">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632739582130944940" />
        <item text="OnPlay_Failed" treenode="632739582130944945" />
      </items>
      <linkto id="632739582130944959" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="variable">prompt_greeting</ap>
        <ap name="Prompt2" type="variable">prompt_employeeId</ap>
        <ap name="UserData" type="literal">employee</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130944959" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="416" y="267">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632739582130944933" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" activetab="true" startnode="632739582130944939" treenode="632739582130944940" appnode="632739582130944937" handlerfor="632739582130944936">
    <node type="Start" id="632739582130944939" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="88" y="458">
      <linkto id="632739582130945106" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632739582130944961" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="372" y="456">
      <linkto id="632739582130944972" type="Labeled" style="Bezier" ortho="true" label="employee" />
      <linkto id="632739582130945047" type="Labeled" style="Bezier" ortho="true" label="voiceprint" />
      <linkto id="632739582130945088" type="Labeled" style="Bezier" ortho="true" label="reset" />
      <linkto id="632742373427124760" type="Labeled" style="Bezier" ortho="true" label="verifying" />
      <linkto id="632742373427124765" type="Labeled" style="Bezier" ortho="true" label="invalid" />
      <linkto id="632742373427124771" type="Labeled" style="Bezier" ortho="true" label="exit" />
      <linkto id="632743294726239631" type="Labeled" style="Bezier" ortho="true" label="password" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130944972" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="170" y="278" mx="244" my="294">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632739582130944966" />
        <item text="OnGatherDigits_Failed" treenode="632739582130944971" />
      </items>
      <linkto id="632739582130944977" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="csharp">Convert.ToInt32(config_employeeId.Length)</ap>
        <ap name="UserData" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130944977" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="83" y="295">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739582130945047" name="Record" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="557" y="283" mx="617" my="299">
      <items count="2">
        <item text="OnRecord_Complete" treenode="632739582130945041" />
        <item text="OnRecord_Failed" treenode="632739582130945046" />
      </items>
      <linkto id="632739582130945054" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Expires" type="literal">0</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxTime" type="variable">config_recordMaxTimeMs</ap>
        <ap name="TermCondSilence" type="variable">config_silenceMs</ap>
        <ap name="UserData" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945054" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="856" y="296">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739582130945077" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="256" y="761">
      <linkto id="632742373427124762" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">500</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945084" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="475" y="746" mx="528" my="762">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632739582130944940" />
        <item text="OnPlay_Failed" treenode="632739582130944945" />
      </items>
      <linkto id="632739582130945087" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="variable">prompt_pressDigitToResetPassword</ap>
        <ap name="UserData" type="literal">reset</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945087" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="667" y="760">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739582130945088" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="539" y="601" mx="613" my="617">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632739582130944966" />
        <item text="OnGatherDigits_Failed" treenode="632739582130944971" />
      </items>
      <linkto id="632739582130945091" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945091" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="485" y="615">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739582130945106" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="204" y="457">
      <linkto id="632739582130944961" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632739582130944977" type="Labeled" style="Bezier" ortho="true" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">terminationCondition</ap>
      </Properties>
    </node>
    <node type="Label" id="632742373427124760" text="v" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="580" y="453" />
    <node type="Label" id="632742373427124761" text="v" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="153" y="762">
      <linkto id="632739582130945077" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632742373427124762" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="379" y="761">
      <linkto id="632739582130945084" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632742373427124767" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_employeeId</ap>
        <ap name="Value2" type="variable">config_employeeId</ap>
      </Properties>
    </node>
    <node type="Action" id="632742373427124765" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="317" y="597" mx="375" my="613">
      <items count="1">
        <item text="RequestEmployeeId" />
      </items>
      <linkto id="632739582130945091" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Mode" type="literal">normal</ap>
        <ap name="FunctionName" type="literal">RequestEmployeeId</ap>
      </Properties>
    </node>
    <node type="Action" id="632742373427124767" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="331" y="899" mx="384" my="915">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632739582130944940" />
        <item text="OnPlay_Failed" treenode="632739582130944945" />
      </items>
      <linkto id="632742373427124770" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="literal">The provided credentials are not valid.</ap>
        <ap name="Prompt2" type="literal">Please try again.</ap>
        <ap name="UserData" type="literal">invalid</ap>
      </Properties>
    </node>
    <node type="Action" id="632742373427124770" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="538" y="916">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632742373427124771" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="235" y="542">
      <linkto id="632742373427124772" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632742373427124772" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="146" y="637">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632743294726239627" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="328" y="161" mx="381" my="177">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632739582130944940" />
        <item text="OnPlay_Failed" treenode="632739582130944945" />
      </items>
      <linkto id="632743294726239630" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="literal">Your new password is:</ap>
        <ap name="Prompt2" type="csharp">g_newPassword.ToUpper()</ap>
        <ap name="UserData" type="literal">password</ap>
      </Properties>
    </node>
    <node type="Action" id="632743294726239630" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="375" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632743294726239631" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="387" y="327">
      <linkto id="632743294726239627" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="csharp">1000</ap>
      </Properties>
    </node>
    <node type="Variable" id="632739582130944960" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632739582130945105" name="terminationCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" defaultInitWith="NONE" refType="reference" name="Metreos.MediaControl.Play_Complete">terminationCondition</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632739582130944944" treenode="632739582130944945" appnode="632739582130944942" handlerfor="632739582130944941">
    <node type="Start" id="632739582130944944" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="68" y="241">
      <linkto id="632739582130945078" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632739582130945078" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="176" y="242">
      <linkto id="632739582130945079" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945079" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="317" y="243">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" startnode="632739582130944965" treenode="632739582130944966" appnode="632739582130944963" handlerfor="632739582130944962">
    <node type="Start" id="632739582130944965" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="46" y="262">
      <linkto id="632739582130944981" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632739582130944981" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="141" y="262">
      <linkto id="632739582130944996" type="Labeled" style="Bezier" ortho="true" label="employee" />
      <linkto id="632739582130945092" type="Labeled" style="Bezier" ortho="true" label="reset" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130944984" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="495" y="137">
      <linkto id="632739582130944988" type="Labeled" style="Bezier" ortho="true" label="maxdigits" />
      <linkto id="632739582130945007" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632739582130945103" type="Labeled" style="Bezier" ortho="true" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">terminationCondition</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130944985" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="506" y="458">
      <linkto id="632739582130945098" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632739582130945102" type="Labeled" style="Bezier" ortho="true" label="maxdigits" />
      <linkto id="632739582130945104" type="Labeled" style="Bezier" ortho="true" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">terminationCondition</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130944988" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="608" y="138">
      <linkto id="632739582130944989" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">digits</ap>
        <rd field="ResultData">g_employeeId</rd>
      </Properties>
    </node>
    <node type="Action" id="632739582130944989" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="690.2334" y="121" mx="744" my="137">
      <items count="1">
        <item text="RequestVoicePrint" />
      </items>
      <linkto id="632739582130944998" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Mode" type="literal">normal</ap>
        <ap name="FunctionName" type="literal">RequestVoicePrint</ap>
      </Properties>
    </node>
    <node type="Label" id="632739582130944996" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="140" y="135" />
    <node type="Label" id="632739582130944997" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="407" y="138">
      <linkto id="632739582130944984" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632739582130944998" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="741" y="254">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739582130945007" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="552.9297" y="239" mx="611" my="255">
      <items count="1">
        <item text="RequestEmployeeId" />
      </items>
      <linkto id="632739582130944998" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Mode" type="literal">error</ap>
        <ap name="FunctionName" type="literal">RequestEmployeeId</ap>
      </Properties>
    </node>
    <node type="Label" id="632739582130945092" text="R" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="234" y="260" />
    <node type="Label" id="632739582130945093" text="R" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="407" y="459">
      <linkto id="632739582130944985" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632739582130945098" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="456" y="574" mx="509" my="590">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632739582130944940" />
        <item text="OnPlay_Failed" treenode="632739582130944945" />
      </items>
      <linkto id="632742373427124578" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="literal">I'm sorry, I did not get that.</ap>
        <ap name="Prompt2" type="variable">prompt_pressDigitToResetPassword</ap>
        <ap name="UserData" type="literal">reset</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945102" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="650" y="457">
      <linkto id="632739582130945098" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632739582130945112" type="Labeled" style="Bezier" ortho="true" label="0" />
      <linkto id="632739582130945113" type="Labeled" style="Bezier" ortho="true" label="1" />
      <Properties language="csharp">public static string Execute(string digits)
{
	if (digits.EndsWith("1"))
		return "1";
	else if (digits.EndsWith("0"))
		return "0";
 	else
		return "default";
}
</Properties>
    </node>
    <node type="Action" id="632739582130945103" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="495" y="39">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739582130945104" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="505" y="347">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739582130945108" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="469" y="816" mx="522" my="832">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632739582130944940" />
        <item text="OnPlay_Failed" treenode="632739582130944945" />
      </items>
      <linkto id="632739582130945115" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632739582130945117" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="literal">Your password has not been changed.</ap>
        <ap name="Prompt2" type="literal">Good-bye.</ap>
        <ap name="UserData" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Label" id="632739582130945112" text="0" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="762" y="383" />
    <node type="Label" id="632739582130945113" text="1" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="759" y="517" />
    <node type="Label" id="632739582130945114" text="0" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="409" y="833">
      <linkto id="632739582130945108" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632739582130945115" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="650" y="831">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739582130945117" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="519" y="980">
      <linkto id="632739582130945119" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945119" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="647" y="980">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739582130945120" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="502" y="1184" mx="555" my="1200">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632739582130944940" />
        <item text="OnPlay_Failed" treenode="632739582130944945" />
      </items>
      <linkto id="632739582130945121" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632739582130945122" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="literal">Your new password is:</ap>
        <ap name="Prompt2" type="csharp">g_newPassword.ToUpper()</ap>
        <ap name="UserData" type="literal">password</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945121" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="683" y="1199">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739582130945122" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="552" y="1358">
      <linkto id="632739582130945123" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945123" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="683" y="1358">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632739582130945130" text="1" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="189" y="1199">
      <linkto id="632743441199793184" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632742373427124578" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="506" y="738">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632743294726239838" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="421" y="1200">
      <linkto id="632739582130945120" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string g_newPassword)
{
	string tempPassword = g_newPassword;
	g_newPassword = string.Empty;
	foreach (char c in tempPassword)
	{
		g_newPassword += c;
		g_newPassword += ",";
	}

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632743441199793184" name="SetPassword" class="MaxActionNode" group="" path="Metreos.SDK.PasswordReset" x="294" y="1200">
      <linkto id="632743294726239838" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632743441199793185" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Username" type="variable">config_ntUsername</ap>
        <ap name="NewPassword" type="variable">g_newPassword</ap>
      </Properties>
    </node>
    <node type="Action" id="632743441199793185" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="245" y="1341" mx="298" my="1357">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632739582130944940" />
        <item text="OnPlay_Failed" treenode="632739582130944945" />
      </items>
      <linkto id="632739582130945122" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632743441199793188" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="literal">Due to a technical problem, your password could not be reset. Good-bye.</ap>
        <ap name="UserData" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632743441199793188" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="296" y="1511">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632739582130944978" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" defaultInitWith="NONE" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">digits</Properties>
    </node>
    <node type="Variable" id="632739582130944979" name="terminationCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" defaultInitWith="NONE" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">terminationCondition</Properties>
    </node>
    <node type="Variable" id="632739582130944980" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632739582130944970" treenode="632739582130944971" appnode="632739582130944968" handlerfor="632739582130944967">
    <node type="Start" id="632739582130944970" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="163">
      <linkto id="632739582130945080" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632739582130945080" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="145.352859" y="163">
      <linkto id="632739582130945081" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945081" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="258.352844" y="162">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Complete" startnode="632739582130945040" treenode="632739582130945041" appnode="632739582130945038" handlerfor="632739582130945037">
    <node type="Start" id="632739582130945040" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="170" y="167">
      <linkto id="632739582130945055" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632739582130945055" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="312" y="167">
      <linkto id="632739582130945056" type="Labeled" style="Bezier" ortho="true" label="silence" />
      <linkto id="632739582130945074" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632739582130945107" type="Labeled" style="Bezier" ortho="true" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">terminationCondition</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945056" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="420" y="150" mx="473" my="166">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632739582130944940" />
        <item text="OnPlay_Failed" treenode="632739582130944945" />
      </items>
      <linkto id="632739582130945058" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632739582130945059" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="variable">prompt_verifying</ap>
        <ap name="UserData" type="literal">verifying</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945058" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="615" y="165">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739582130945059" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="471" y="303">
      <linkto id="632739582130945060" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945060" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="471" y="465">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739582130945061" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="314" y="468">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739582130945074" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="263" y="278" mx="317" my="294">
      <items count="1">
        <item text="RequestVoicePrint" />
      </items>
      <linkto id="632739582130945061" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Mode" type="literal">error</ap>
        <ap name="FunctionName" type="literal">RequestVoicePrint</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945107" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="313" y="47">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632739582130945073" name="terminationCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" defaultInitWith="NONE" refType="reference" name="Metreos.MediaControl.Record_Complete">terminationCondition</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Failed" startnode="632739582130945045" treenode="632739582130945046" appnode="632739582130945043" handlerfor="632739582130945042">
    <node type="Start" id="632739582130945045" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632739582130945076" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632739582130945076" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="110" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632743294726239835" treenode="632743294726239836" appnode="632743294726239833" handlerfor="632743294726239832">
    <node type="Start" id="632743294726239835" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632743294726239837" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632743294726239837" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="124" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="RequestVoicePrint" startnode="632739582130944992" treenode="632739582130944993" appnode="632739582130944990" handlerfor="632743294726239832">
    <node type="Start" id="632739582130944992" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="65" y="158">
      <linkto id="632739582130945000" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632739582130945000" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="187" y="157">
      <linkto id="632739582130945001" type="Labeled" style="Bezier" ortho="true" label="normal" />
      <linkto id="632739582130945004" type="Labeled" style="Bezier" ortho="true" label="error" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">mode</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945001" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="313" y="140" mx="366" my="156">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632739582130944940" />
        <item text="OnPlay_Failed" treenode="632739582130944945" />
      </items>
      <linkto id="632739582130945012" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632739582130945015" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="variable">prompt_provideVoicePrint</ap>
        <ap name="UserData" type="literal">voiceprint</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945004" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="138" y="309" mx="191" my="325">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632739582130944940" />
        <item text="OnPlay_Failed" treenode="632739582130944945" />
      </items>
      <linkto id="632739582130945015" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632739582130945017" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="literal">I'm sorry, I did not get that.</ap>
        <ap name="Prompt2" type="variable">prompt_provideVoicePrint</ap>
        <ap name="UserData" type="literal">voiceprint</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945012" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="508" y="155">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739582130945015" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="365" y="323">
      <linkto id="632739582130945016" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945016" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="493" y="417">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739582130945017" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="189" y="481">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632739582130944999" name="mode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Mode" refType="reference">mode</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="RequestEmployeeId" startnode="632739582130945010" treenode="632739582130945011" appnode="632739582130945008" handlerfor="632743294726239832">
    <node type="Start" id="632739582130945010" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="175" y="188">
      <linkto id="632739582130945018" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632739582130945018" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="283" y="189">
      <linkto id="632739582130945019" type="Labeled" style="Bezier" ortho="true" label="normal" />
      <linkto id="632739582130945020" type="Labeled" style="Bezier" ortho="true" label="error" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">mode</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945019" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="409" y="172" mx="462" my="188">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632739582130944940" />
        <item text="OnPlay_Failed" treenode="632739582130944945" />
      </items>
      <linkto id="632739582130945021" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632739582130945022" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="variable">prompt_employeeId</ap>
        <ap name="UserData" type="literal">employee</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945020" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="234" y="341" mx="287" my="357">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632739582130944940" />
        <item text="OnPlay_Failed" treenode="632739582130944945" />
      </items>
      <linkto id="632739582130945022" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632739582130945024" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="literal">I'm sorry, I did not get that.</ap>
        <ap name="Prompt2" type="variable">prompt_employeeId</ap>
        <ap name="UserData" type="literal">employee</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945021" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="604" y="187">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739582130945022" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="461" y="355">
      <linkto id="632739582130945023" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632739582130945023" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="589" y="449">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739582130945024" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="285" y="513">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632739582130945036" name="mode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Mode" refType="reference">mode</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>