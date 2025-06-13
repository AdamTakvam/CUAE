<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632793959313906605" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632793959313906602" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632793959313906601" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632793959313906648" level="2" text="Metreos.MediaControl.VoiceRecognition_Complete: OnVoiceRecognition_Complete">
        <node type="function" name="OnVoiceRecognition_Complete" id="632793959313906645" path="Metreos.StockTools" />
        <node type="event" name="VoiceRecognition_Complete" id="632793959313906644" path="Metreos.MediaControl.VoiceRecognition_Complete" />
        <references>
          <ref id="632797641869375435" actid="632793959313906654" />
          <ref id="632797641869375487" actid="632793959313906825" />
          <ref id="632797641869375507" actid="632793959313906847" />
          <ref id="632797641869375539" actid="632793959313906868" />
          <ref id="632797641869375546" actid="632793959313906900" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632793959313906653" level="2" text="Metreos.MediaControl.VoiceRecognition_Failed: OnVoiceRecognition_Failed">
        <node type="function" name="OnVoiceRecognition_Failed" id="632793959313906650" path="Metreos.StockTools" />
        <node type="event" name="VoiceRecognition_Failed" id="632793959313906649" path="Metreos.MediaControl.VoiceRecognition_Failed" />
        <references>
          <ref id="632797641869375436" actid="632793959313906654" />
          <ref id="632797641869375488" actid="632793959313906825" />
          <ref id="632797641869375508" actid="632793959313906847" />
          <ref id="632797641869375540" actid="632793959313906868" />
          <ref id="632797641869375547" actid="632793959313906900" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632793959313906676" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632793959313906673" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632793959313906672" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632797641869375444" actid="632793959313906682" />
          <ref id="632797641869375523" actid="632793959313906814" />
          <ref id="632797641869375526" actid="632793959313906817" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632793959313906681" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632793959313906678" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632793959313906677" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632797641869375445" actid="632793959313906682" />
          <ref id="632797641869375524" actid="632793959313906814" />
          <ref id="632797641869375527" actid="632793959313906817" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632793959313906696" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632793959313906693" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632793959313906692" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632797641869375499" actid="632793959313906920" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632793959313906792" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632793959313906789" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632793959313906788" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632797641869375439" actid="632793959313906798" />
          <ref id="632797641869375494" actid="632793959313906842" />
          <ref id="632797641869375510" actid="632793959313906850" />
          <ref id="632797641869375536" actid="632793959313906865" />
          <ref id="632797641869375550" actid="632793959313906929" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632793959313906797" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632793959313906794" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632793959313906793" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632797641869375440" actid="632793959313906798" />
          <ref id="632797641869375495" actid="632793959313906842" />
          <ref id="632797641869375511" actid="632793959313906850" />
          <ref id="632797641869375537" actid="632793959313906865" />
          <ref id="632797641869375551" actid="632793959313906929" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632793959313906832" level="2" text="Metreos.MediaControl.Record_Complete: OnRecord_Complete">
        <node type="function" name="OnRecord_Complete" id="632793959313906829" path="Metreos.StockTools" />
        <node type="event" name="Record_Complete" id="632793959313906828" path="Metreos.MediaControl.Record_Complete" />
        <references>
          <ref id="632797641869375490" actid="632793959313906838" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632793959313906837" level="2" text="Metreos.MediaControl.Record_Failed: OnRecord_Failed">
        <node type="function" name="OnRecord_Failed" id="632793959313906834" path="Metreos.StockTools" />
        <node type="event" name="Record_Failed" id="632793959313906833" path="Metreos.MediaControl.Record_Failed" />
        <references>
          <ref id="632797641869375491" actid="632793959313906838" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632793959313906914" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632793959313906911" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632793959313906910" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632797641869375497" actid="632793959313906920" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632793959313906919" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632793959313906916" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632793959313906915" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632797641869375498" actid="632793959313906920" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="fun" id="632793959313906811" level="1" text="VerifyId">
        <node type="function" name="VerifyId" id="632793959313906808" path="Metreos.StockTools" />
        <calls>
          <ref actid="632793959313906821" />
          <ref actid="632793959313906807" />
        </calls>
      </treenode>
      <treenode type="fun" id="632793959313906857" level="1" text="VerifyOption">
        <node type="function" name="VerifyOption" id="632793959313906854" path="Metreos.StockTools" />
        <calls>
          <ref actid="632793959313906858" />
          <ref actid="632793959313906853" />
        </calls>
      </treenode>
      <treenode type="fun" id="632793959313906891" level="1" text="ResetSuccess">
        <node type="function" name="ResetSuccess" id="632793959313906888" path="Metreos.StockTools" />
        <calls>
          <ref actid="632793959313906884" />
          <ref actid="632793959313906894" />
          <ref actid="632793959313906892" />
        </calls>
      </treenode>
      <treenode type="fun" id="632793959313907139" level="1" text="CloseGDOp">
        <node type="function" name="CloseGDOp" id="632793959313907136" path="Metreos.StockTools" />
        <calls>
          <ref actid="632793959313907135" />
          <ref actid="632793959313907140" />
          <ref actid="632793959313907141" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_helpDesk" id="632797641869375412" vid="632793959313906618">
        <Properties type="String" initWith="HelpDesk">g_helpDesk</Properties>
      </treenode>
      <treenode text="g_callId" id="632797641869375414" vid="632793959313906622">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632797641869375416" vid="632793959313906624">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_employeeId" id="632797641869375418" vid="632793959313906803">
        <Properties type="String" initWith="EmployeeID">g_employeeId</Properties>
      </treenode>
      <treenode text="g_opIdGDOnIncomingCall" id="632797641869375420" vid="632793959313906932">
        <Properties type="String" defaultInitWith="0">g_opIdGDOnIncomingCall</Properties>
      </treenode>
      <treenode text="g_opIdGDOnPlayComplete" id="632797641869375422" vid="632793959313906934">
        <Properties type="String" defaultInitWith="0">g_opIdGDOnPlayComplete</Properties>
      </treenode>
      <treenode text="g_opIdGDOnRecordComplete" id="632797641869375424" vid="632793959313906936">
        <Properties type="String" defaultInitWith="0">g_opIdGDOnRecordComplete</Properties>
      </treenode>
      <treenode text="g_opIdGDVerifyOption" id="632797641869375426" vid="632793959313906938">
        <Properties type="String" defaultInitWith="0">g_opIdGDVerifyOption</Properties>
      </treenode>
      <treenode text="g_opIdGDResetSuccess" id="632797641869375428" vid="632793959313906940">
        <Properties type="String" defaultInitWith="0">g_opIdGDResetSuccess</Properties>
      </treenode>
      <treenode text="g_resetOption" id="632797641869375430" vid="632796133890156812">
        <Properties type="String">g_resetOption</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632793959313906604" treenode="632793959313906605" appnode="632793959313906602" handlerfor="632793959313906601">
    <node type="Start" id="632793959313906604" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="53">
      <linkto id="632793959313906620" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793959313906620" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="171" y="53">
      <linkto id="632793959313906626" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <linkto id="632793959313906798" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="WaitForMedia" type="literal">True</ap>
        <ap name="Conference" type="literal">False</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="ConnectionId">g_connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632793959313906626" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="172" y="236">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632793959313906654" name="VoiceRecognition" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="432" y="36" mx="520" my="52">
      <items count="2">
        <item text="OnVoiceRecognition_Complete" treenode="632793959313906648" />
        <item text="OnVoiceRecognition_Failed" treenode="632793959313906653" />
      </items>
      <linkto id="632793959313906691" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">pr_enterid.wav</ap>
        <ap name="Grammar1" type="literal">digits.grxml</ap>
        <ap name="Grammar2" type="literal">help.grxml</ap>
        <ap name="VoiceBargeIn" type="literal">True</ap>
        <ap name="CancelOnDigit" type="literal">True</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="literal">pr_welcome.wav</ap>
        <ap name="UserData" type="literal">enterid</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906691" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="672" y="51">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632793959313906798" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="250" y="36" mx="324" my="52">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632793959313906792" />
        <item text="OnGatherDigits_Failed" treenode="632793959313906797" />
      </items>
      <linkto id="632793959313906654" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondInterDigitDelay" type="literal">3000</ap>
        <ap name="UserData" type="literal">enterid</ap>
        <rd field="OperationId">g_opIdGDOnIncomingCall</rd>
      </Properties>
    </node>
    <node type="Variable" id="632793959313906621" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnVoiceRecognition_Complete" startnode="632793959313906647" treenode="632793959313906648" appnode="632793959313906645" handlerfor="632793959313906644">
    <node type="Start" id="632793959313906647" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="110">
      <linkto id="632795082941563059" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793959313906682" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="914" y="266" mx="967" my="282">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632793959313906676" />
        <item text="OnPlay_Failed" treenode="632793959313906681" />
      </items>
      <linkto id="632793959313906685" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">pr_helpprompt.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">helpdesk</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906685" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="887" y="498">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632793959313906698" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="676" y="112">
      <linkto id="632793959313906700" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(string userData, ref string meaning, int score, LogWriter log, ref string action)
{
      action = "";

      if (score &gt; 0) 
      {    
	  if (meaning.IndexOf("help") &gt;= 0)        
          action = "help";
        else         
          action = userData;

	  if (meaning.IndexOf("windows") &gt;= 0)        
          meaning = "1";
	  if (meaning.IndexOf("voicemail") &gt;= 0)        
          meaning = "2";
	  if (meaning.IndexOf("vpn") &gt;= 0)        
          meaning = "3";
      }

	log.Write(TraceLevel.Verbose, "VoiceRec meaning is [{0}], score is [{1}], userData is  {2}", meaning, score, userData);
               
      return IApp.VALUE_SUCCESS; 
}

</Properties>
    </node>
    <node type="Action" id="632793959313906700" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="883" y="112">
      <linkto id="632793959313906682" type="Labeled" style="Bezier" ortho="true" label="help" />
      <linkto id="632793959313906821" type="Labeled" style="Bezier" ortho="true" label="enterid" />
      <linkto id="632793959313906858" type="Labeled" style="Bezier" ortho="true" label="resetoptions" />
      <linkto id="632793959313906885" type="Labeled" style="Bezier" ortho="true" label="resetsuccess" />
      <linkto id="632793959313906685" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">action</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906701" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="156" y="271">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632793959313906710" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="203" y="110">
      <linkto id="632793959313906701" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632793959313907135" type="Labeled" style="Bezier" ortho="true" label="resetsuccess" />
      <linkto id="632793959313907140" type="Labeled" style="Bezier" ortho="true" label="enterid" />
      <linkto id="632793959313907141" type="Labeled" style="Bezier" ortho="true" label="resetoptions" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906821" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="774.8258" y="273" mx="812" my="289">
      <items count="1">
        <item text="VerifyId" />
      </items>
      <linkto id="632793959313906685" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="id" type="variable">meaning</ap>
        <ap name="FunctionName" type="literal">VerifyId</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906858" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="631.8258" y="271" mx="669" my="287">
      <items count="1">
        <item text="VerifyOption" />
      </items>
      <linkto id="632793959313906685" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="option" type="variable">meaning</ap>
        <ap name="FunctionName" type="literal">VerifyOption</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906884" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="479.647461" y="483" mx="522" my="499">
      <items count="1">
        <item text="ResetSuccess" />
      </items>
      <linkto id="632793959313906685" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">ResetSuccess</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906885" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="520" y="289">
      <linkto id="632793959313906884" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632793959313906886" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">meaning == "repeat"</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906886" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="412" y="289">
      <linkto id="632793959313906887" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906887" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="409" y="500">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632793959313907135" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="398.825836" y="169" mx="436" my="185">
      <items count="1">
        <item text="CloseGDOp" />
      </items>
      <linkto id="632793959313906698" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="opid1" type="variable">g_opIdGDResetSuccess</ap>
        <ap name="opid2" type="literal">0</ap>
        <ap name="FunctionName" type="literal">CloseGDOp</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313907140" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="394.825836" y="96" mx="432" my="112">
      <items count="1">
        <item text="CloseGDOp" />
      </items>
      <linkto id="632793959313906698" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="opid1" type="variable">g_opIdGDOnIncomingCall</ap>
        <ap name="opid2" type="variable">g_opIdGDOnPlayComplete</ap>
        <ap name="FunctionName" type="literal">CloseGDOp</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313907141" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="390.825836" y="16" mx="428" my="32">
      <items count="1">
        <item text="CloseGDOp" />
      </items>
      <linkto id="632793959313906698" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="opid1" type="variable">g_opIdGDOnRecordComplete</ap>
        <ap name="opid2" type="variable">g_opIdGDVerifyOption</ap>
        <ap name="FunctionName" type="literal">CloseGDOp</ap>
      </Properties>
    </node>
    <node type="Action" id="632795082941563059" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="114" y="110">
      <linkto id="632793959313906710" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632793959313906701" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">score == 0</ap>
      </Properties>
    </node>
    <node type="Variable" id="632793959313906671" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632793959313906689" name="score" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="Score" refType="reference" name="Metreos.MediaControl.VoiceRecognition_Complete">score</Properties>
    </node>
    <node type="Variable" id="632793959313906690" name="meaning" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Meaning" refType="reference" name="Metreos.MediaControl.VoiceRecognition_Complete">meaning</Properties>
    </node>
    <node type="Variable" id="632793959313906699" name="action" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" defaultInitWith="String.Empty" refType="reference">action</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnVoiceRecognition_Failed" startnode="632793959313906652" treenode="632793959313906653" appnode="632793959313906650" handlerfor="632793959313906649">
    <node type="Start" id="632793959313906652" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632793959313906688" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793959313906688" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="191" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632793959313906675" treenode="632793959313906676" appnode="632793959313906673" handlerfor="632793959313906672">
    <node type="Start" id="632793959313906675" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="37">
      <linkto id="632793959313906824" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793959313906686" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="381" y="37">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632793959313906824" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="188" y="37">
      <linkto id="632793959313906686" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632793959313906838" type="Labeled" style="Bezier" ortho="true" label="passphrase" />
      <linkto id="632793959313906842" type="Labeled" style="Bezier" label="enterid" />
      <linkto id="632793959313906920" type="Labeled" style="Bezier" ortho="true" label="helpdesk" />
      <linkto id="632793959313906842" type="Labeled" style="Bezier" label="eidnotfound" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906825" name="VoiceRecognition" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="204" y="440" mx="292" my="456">
      <items count="2">
        <item text="OnVoiceRecognition_Complete" treenode="632793959313906648" />
        <item text="OnVoiceRecognition_Failed" treenode="632793959313906653" />
      </items>
      <linkto id="632793959313906841" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Grammar1" type="literal">digits.grxml</ap>
        <ap name="Grammar2" type="literal">help.grxml</ap>
        <ap name="VoiceBargeIn" type="literal">True</ap>
        <ap name="CancelOnDigit" type="literal">True</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="literal">pr_enterid.wav</ap>
        <ap name="UserData" type="literal">enterid</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906838" name="Record" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="35" y="167" mx="95" my="183">
      <items count="2">
        <item text="OnRecord_Complete" treenode="632793959313906832" />
        <item text="OnRecord_Failed" treenode="632793959313906837" />
      </items>
      <linkto id="632793959313906841" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondSilence" type="literal">3000</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906841" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="92" y="456">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632793959313906842" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="216" y="170" mx="290" my="186">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632793959313906792" />
        <item text="OnGatherDigits_Failed" treenode="632793959313906797" />
      </items>
      <linkto id="632793959313906825" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondInterDigitDelay" type="literal">3000</ap>
        <ap name="UserData" type="literal">enterid</ap>
        <rd field="OperationId">g_opIdGDOnPlayComplete</rd>
      </Properties>
    </node>
    <node type="Action" id="632793959313906920" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="430" y="160" mx="496" my="176">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632793959313906914" />
        <item text="OnMakeCall_Failed" treenode="632793959313906919" />
        <item text="OnRemoteHangup" treenode="632793959313906696" />
      </items>
      <linkto id="632793959313906928" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_helpDesk</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="literal">0</ap>
        <ap name="Hairpin" type="literal">true</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906928" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="497" y="460">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632793959313906705" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632793959313906695" treenode="632793959313906696" appnode="632793959313906693" handlerfor="632793959313906692">
    <node type="Start" id="632793959313906695" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="43">
      <linkto id="632793959313906697" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793959313906697" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="191" y="43">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" startnode="632793959313906791" treenode="632793959313906792" appnode="632793959313906789" handlerfor="632793959313906788">
    <node type="Start" id="632793959313906791" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="46" y="63">
      <linkto id="632795082941563056" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793959313906801" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="498" y="468">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632793959313906806" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="486" y="65">
      <linkto id="632793959313906801" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632793959313906807" type="Labeled" style="Bezier" ortho="true" label="enterid" />
      <linkto id="632793959313906853" type="Labeled" style="Bezier" ortho="true" label="resetoptions" />
      <linkto id="632793959313906893" type="Labeled" style="Bezier" ortho="true" label="resetsuccess" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906807" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="557.825867" y="220" mx="595" my="236">
      <items count="1">
        <item text="VerifyId" />
      </items>
      <linkto id="632793959313906801" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="id" type="variable">digits</ap>
        <ap name="FunctionName" type="literal">VerifyId</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906853" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="355.825867" y="218" mx="393" my="234">
      <items count="1">
        <item text="VerifyOption" />
      </items>
      <linkto id="632793959313906801" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="option" type="variable">digits</ap>
        <ap name="FunctionName" type="literal">VerifyOption</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906893" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="697" y="65">
      <linkto id="632793959313906894" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632793959313906895" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">digits == "1"</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906894" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="656.647461" y="453" mx="699" my="469">
      <items count="1">
        <item text="ResetSuccess" />
      </items>
      <linkto id="632793959313906801" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">ResetSuccess</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906895" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="879" y="65">
      <linkto id="632793959313906896" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906896" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="884" y="476">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632793959313906905" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="368" y="64">
      <linkto id="632793959313906806" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(string digits, LogWriter log)
{
	log.Write(TraceLevel.Verbose, "Received digits are [{0}]", digits);
               
      return IApp.VALUE_SUCCESS; 
}

</Properties>
    </node>
    <node type="Action" id="632795082941563056" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="155" y="63">
      <linkto id="632795082941563057" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632795082941563057" type="Labeled" style="Bezier" label="autostop" />
      <linkto id="632795082941563058" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">tc</ap>
      </Properties>
    </node>
    <node type="Action" id="632795082941563057" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="192" y="466">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632795082941563058" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="245" y="64">
      <linkto id="632795082941563057" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632793959313906905" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">digits.Length ==  0 || digits == "NONE"</ap>
      </Properties>
    </node>
    <node type="Variable" id="632793959313906805" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632793959313906823" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" defaultInitWith="NONE" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">digits</Properties>
    </node>
    <node type="Variable" id="632795082941563055" name="tc" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">tc</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Complete" startnode="632793959313906831" treenode="632793959313906832" appnode="632793959313906829" handlerfor="632793959313906828">
    <node type="Start" id="632793959313906831" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="73">
      <linkto id="632793959313906850" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793959313906846" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="536" y="74">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632793959313906847" name="VoiceRecognition" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="243" y="57" mx="331" my="73">
      <items count="2">
        <item text="OnVoiceRecognition_Complete" treenode="632793959313906648" />
        <item text="OnVoiceRecognition_Failed" treenode="632793959313906653" />
      </items>
      <linkto id="632793959313906846" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Grammar1" type="literal">options.grxml</ap>
        <ap name="Grammar2" type="literal">help.grxml</ap>
        <ap name="VoiceBargeIn" type="literal">True</ap>
        <ap name="CancelOnDigit" type="literal">True</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="literal">pr_resetoptions.wav</ap>
        <ap name="UserData" type="literal">resetoptions</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906850" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="78" y="58" mx="152" my="74">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632793959313906792" />
        <item text="OnGatherDigits_Failed" treenode="632793959313906797" />
      </items>
      <linkto id="632793959313906847" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondInterDigitDelay" type="literal">3000</ap>
        <ap name="UserData" type="literal">resetoptions</ap>
        <rd field="OperationId">g_opIdGDOnRecordComplete</rd>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Failed" startnode="632793959313906836" treenode="632793959313906837" appnode="632793959313906834" handlerfor="632793959313906833">
    <node type="Start" id="632793959313906836" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="43">
      <linkto id="632793959313906845" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793959313906845" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="188" y="44">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632793959313906913" treenode="632793959313906914" appnode="632793959313906911" handlerfor="632793959313906910">
    <node type="Start" id="632793959313906913" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="45" y="95">
      <linkto id="632793959313906925" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793959313906925" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="211" y="95">
      <linkto id="632793959313906926" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="ConferenceId" type="variable">confId</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906926" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="358" y="95">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632793959313906924" name="confId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConferenceId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">confId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632793959313906918" treenode="632793959313906919" appnode="632793959313906916" handlerfor="632793959313906915">
    <node type="Start" id="632793959313906918" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="55" y="110">
      <linkto id="632793959313906927" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793959313906927" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="253" y="108">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="VerifyId" startnode="632793959313906810" treenode="632793959313906811" appnode="632793959313906808" handlerfor="632793959313906915">
    <node type="Start" id="632793959313906810" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="69">
      <linkto id="632793959313906813" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793959313906813" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="138" y="68">
      <linkto id="632793959313906814" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632796133890156811" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">eid == g_employeeId</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906814" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="86" y="199" mx="139" my="215">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632793959313906676" />
        <item text="OnPlay_Failed" treenode="632793959313906681" />
      </items>
      <linkto id="632793959313906820" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">pr_passphrase.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">passphrase</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906817" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="291" y="53" mx="344" my="69">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632793959313906676" />
        <item text="OnPlay_Failed" treenode="632793959313906681" />
      </items>
      <linkto id="632793959313906820" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt3" type="literal">pr_passwordnotfound.wav</ap>
        <ap name="Prompt1" type="literal">pr_id.wav</ap>
        <ap name="Prompt2" type="variable">eidtts</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">eidnotfound</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906820" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="341" y="215">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632796133890156811" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="232" y="68">
      <linkto id="632793959313906817" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string eid, ref string eidtts, LogWriter log)
{
  eidtts = "";
  for (int i=0; i&lt;eid.Length; i++) 
      eidtts = eidtts + "'" + eid[i] + "' ";
   
  log.Write(TraceLevel.Verbose, "eid for tts is {0}", eidtts);

  return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Variable" id="632793959313906812" name="eid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="id" refType="reference">eid</Properties>
    </node>
    <node type="Variable" id="632796133890156810" name="eidtts" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">eidtts</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="VerifyOption" startnode="632793959313906856" treenode="632793959313906857" appnode="632793959313906854" handlerfor="632793959313906915">
    <node type="Start" id="632793959313906856" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="78">
      <linkto id="632793959313906860" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793959313906860" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="166" y="77">
      <linkto id="632793959313906865" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632796133890156814" type="Labeled" style="Bezier" label="2" />
      <linkto id="632796133890156814" type="Labeled" style="Bezier" label="3" />
      <linkto id="632796133890156814" type="Labeled" style="Bezier" label="1" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">resetOption</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906864" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="434" y="381">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632793959313906865" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="94" y="176" mx="168" my="192">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632793959313906792" />
        <item text="OnGatherDigits_Failed" treenode="632793959313906797" />
      </items>
      <linkto id="632793959313906868" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondInterDigitDelay" type="literal">3000</ap>
        <ap name="UserData" type="literal">resetoptions</ap>
        <rd field="OperationId">g_opIdGDVerifyOption</rd>
      </Properties>
    </node>
    <node type="Action" id="632793959313906868" name="VoiceRecognition" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="80" y="367" mx="168" my="383">
      <items count="2">
        <item text="OnVoiceRecognition_Complete" treenode="632793959313906648" />
        <item text="OnVoiceRecognition_Failed" treenode="632793959313906653" />
      </items>
      <linkto id="632793959313906864" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">pr_resetoptions.wav</ap>
        <ap name="Grammar1" type="literal">digits.grxml</ap>
        <ap name="Grammar2" type="literal">help.grxml</ap>
        <ap name="VoiceBargeIn" type="literal">True</ap>
        <ap name="CancelOnDigit" type="literal">True</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="literal">pr_optionnotfound.wav</ap>
        <ap name="UserData" type="literal">resetoptions</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906892" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="396.647461" y="65" mx="439" my="81">
      <items count="1">
        <item text="ResetSuccess" />
      </items>
      <linkto id="632793959313906864" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">ResetSuccess</ap>
      </Properties>
    </node>
    <node type="Action" id="632796133890156814" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="290" y="79">
      <linkto id="632793959313906892" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">resetOption</ap>
        <rd field="ResultData">g_resetOption</rd>
      </Properties>
    </node>
    <node type="Variable" id="632793959313906859" name="resetOption" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="option" refType="reference">resetOption</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ResetSuccess" startnode="632793959313906890" treenode="632793959313906891" appnode="632793959313906888" handlerfor="632793959313906915">
    <node type="Start" id="632793959313906890" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="59">
      <linkto id="632796133890156816" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793959313906900" name="VoiceRecognition" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="231" y="221" mx="319" my="237">
      <items count="2">
        <item text="OnVoiceRecognition_Complete" treenode="632793959313906648" />
        <item text="OnVoiceRecognition_Failed" treenode="632793959313906653" />
      </items>
      <linkto id="632793959313906903" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">pr_newpassword.wav</ap>
        <ap name="Grammar1" type="literal">digits.grxml</ap>
        <ap name="Grammar2" type="literal">repeat.grxml</ap>
        <ap name="Grammar3" type="literal">help.grxml</ap>
        <ap name="VoiceBargeIn" type="literal">True</ap>
        <ap name="CancelOnDigit" type="literal">True</ap>
        <ap name="Prompt3" type="literal">pr_repeat.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="literal">pr_resetsuccess.wav</ap>
        <ap name="UserData" type="literal">resetsuccess</ap>
      </Properties>
    </node>
    <node type="Action" id="632793959313906903" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="315" y="452">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632793959313906929" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="245" y="45" mx="319" my="61">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632793959313906792" />
        <item text="OnGatherDigits_Failed" treenode="632793959313906797" />
      </items>
      <linkto id="632793959313906900" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxTime" type="literal">10000</ap>
        <ap name="UserData" type="literal">resetsuccess</ap>
        <rd field="OperationId">g_opIdGDResetSuccess</rd>
      </Properties>
    </node>
    <node type="Action" id="632796133890156816" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="155" y="60">
      <linkto id="632793959313906929" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string g_resetOption, ref string optionstts, LogWriter log)
{
      optionstts = "";
	if (g_resetOption == "1")
      	optionstts = "your windows domain password has been reset";
	else if  (g_resetOption == "2")
      	optionstts = "your windows voice mail password has been reset";
	else if (g_resetOption == "3")
      	optionstts = "your windows v p n password has been reset";

      log.Write(TraceLevel.Verbose, "optionstts is {0}", optionstts);

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Variable" id="632796133890156818" name="optionstts" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">optionstts</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="CloseGDOp" activetab="true" startnode="632793959313907138" treenode="632793959313907139" appnode="632793959313907136" handlerfor="632793959313906915">
    <node type="Start" id="632793959313907138" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="52" y="95">
      <linkto id="632795082941563054" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632795082941563051" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="311" y="95">
      <linkto id="632795082941563052" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632795082941563053" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">id1 == "0"</ap>
      </Properties>
    </node>
    <node type="Action" id="632795082941563052" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="311" y="254">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632795082941563053" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="483" y="95">
      <linkto id="632795082941563060" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="OperationId" type="variable">id1</ap>
      </Properties>
    </node>
    <node type="Action" id="632795082941563054" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="161" y="95">
      <linkto id="632795082941563051" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(string userData, string id1, string id2, LogWriter log)
{
	log.Write(TraceLevel.Verbose, "CloseDGOp, id1 = {0}, id2 = {1}", id1, id2);
               
      return IApp.VALUE_SUCCESS; 
}


</Properties>
    </node>
    <node type="Action" id="632795082941563060" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="483" y="255">
      <linkto id="632795082941563052" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632795082941563061" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">id2 == "0"</ap>
      </Properties>
    </node>
    <node type="Action" id="632795082941563061" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="401" y="446">
      <linkto id="632795082941563052" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="OperationId" type="variable">id2</ap>
      </Properties>
    </node>
    <node type="Variable" id="632793959313907142" name="id1" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="opid1" refType="reference">id1</Properties>
    </node>
    <node type="Variable" id="632793959313907143" name="id2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="opid2" refType="reference">id2</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632793959313906796" treenode="632793959313906797" appnode="632793959313906794" handlerfor="632793959313906793">
    <node type="Start" id="632793959313906796" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632793959313906802" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793959313906802" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="210" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <canvas type="Function" name="OnPlay_Failed" show="false" startnode="632793959313906680" treenode="632793959313906681" appnode="632793959313906678" handlerfor="632793959313906677">
    <node type="Start" id="632793959313906680" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632793959313906687" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632793959313906687" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="204" y="33">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>