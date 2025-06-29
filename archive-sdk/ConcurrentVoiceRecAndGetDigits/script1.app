<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632763010860937855" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632763010860937852" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632763010860937851" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632763010860937861" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632763010860937858" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632763010860937857" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632763867035156661" actid="632763010860937867" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632763010860937866" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632763010860937863" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632763010860937862" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632763867035156662" actid="632763010860937867" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632763686033594217" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632763686033594214" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632763686033594213" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632763867035156670" actid="632763686033594223" />
          <ref id="632763867035156673" actid="632763686033594226" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632763686033594222" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632763686033594219" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632763686033594218" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632763867035156671" actid="632763686033594223" />
          <ref id="632763867035156674" actid="632763686033594226" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632763686033594233" level="2" text="Metreos.MediaControl.VoiceRecognition_Complete: OnVoiceRecognition_Complete">
        <node type="function" name="OnVoiceRecognition_Complete" id="632763686033594230" path="Metreos.StockTools" />
        <node type="event" name="VoiceRecognition_Complete" id="632763686033594229" path="Metreos.MediaControl.VoiceRecognition_Complete" />
        <references>
          <ref id="632763867035156676" actid="632763686033594239" />
          <ref id="632763867035156679" actid="632763686033594242" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632763686033594238" level="2" text="Metreos.MediaControl.VoiceRecognition_Failed: OnVoiceRecognition_Failed">
        <node type="function" name="OnVoiceRecognition_Failed" id="632763686033594235" path="Metreos.StockTools" />
        <node type="event" name="VoiceRecognition_Failed" id="632763686033594234" path="Metreos.MediaControl.VoiceRecognition_Failed" />
        <references>
          <ref id="632763867035156677" actid="632763686033594239" />
          <ref id="632763867035156680" actid="632763686033594242" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="632763867035156630" vid="632763113932344135">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_mmsId" id="632763867035156632" vid="632763113932344137">
        <Properties type="String">g_mmsId</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632763867035156634" vid="632763113932344139">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_keypadEnabled" id="632763867035156636" vid="632763686033594193">
        <Properties type="Bool" initWith="KeypadEnabled">g_keypadEnabled</Properties>
      </treenode>
      <treenode text="g_voiceRecEnabled" id="632763867035156638" vid="632763686033594195">
        <Properties type="Bool" initWith="VoiceRecEnabled">g_voiceRecEnabled</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632763010860937854" treenode="632763010860937855" appnode="632763010860937852" handlerfor="632763010860937851">
    <node type="Start" id="632763010860937854" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="73">
      <linkto id="632763010860937856" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632763010860937856" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="134" y="73">
      <linkto id="632763686033594198" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632763686033594200" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="WaitForMedia" type="csharp">true</ap>
        <ap name="Conference" type="literal">false</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="MmsId">g_mmsId</rd>
        <rd field="ConnectionId">g_connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632763010860937867" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="512" y="169" mx="565" my="185">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632763010860937861" />
        <item text="OnPlay_Failed" treenode="632763010860937866" />
      </items>
      <linkto id="632763686033594201" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">enter_id.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632763686033594198" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="265" y="73">
      <linkto id="632763686033594199" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632763686033594226" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_keypadEnabled == true &amp;&amp; g_voiceRecEnabled == true</ap>
      </Properties>
    </node>
    <node type="Action" id="632763686033594199" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="265" y="185">
      <linkto id="632763686033594208" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632763686033594223" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_keypadEnabled == true</ap>
      </Properties>
    </node>
    <node type="Action" id="632763686033594200" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="134" y="429">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632763686033594201" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="726" y="320">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632763686033594208" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="265" y="320">
      <linkto id="632763686033594211" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632763686033594242" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_voiceRecEnabled == true</ap>
      </Properties>
    </node>
    <node type="Action" id="632763686033594211" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="265" y="429">
      <linkto id="632763686033594200" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632763686033594223" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="333" y="169" mx="407" my="185">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632763686033594217" />
        <item text="OnGatherDigits_Failed" treenode="632763686033594222" />
      </items>
      <linkto id="632763010860937867" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">digits_only</ap>
      </Properties>
    </node>
    <node type="Action" id="632763686033594226" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="336" y="56" mx="410" my="72">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632763686033594217" />
        <item text="OnGatherDigits_Failed" treenode="632763686033594222" />
      </items>
      <linkto id="632763686033594239" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">voicerec_digits</ap>
      </Properties>
    </node>
    <node type="Action" id="632763686033594239" name="VoiceRecognition" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="639" y="56" mx="727" my="72">
      <items count="2">
        <item text="OnVoiceRecognition_Complete" treenode="632763686033594233" />
        <item text="OnVoiceRecognition_Failed" treenode="632763686033594238" />
      </items>
      <linkto id="632763686033594201" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Grammar1" type="literal">digits.grxml</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="literal">say_or_enter_id.wav</ap>
        <ap name="UserData" type="literal">voicerec_digits</ap>
      </Properties>
    </node>
    <node type="Action" id="632763686033594242" name="VoiceRecognition" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="316" y="304" mx="404" my="320">
      <items count="2">
        <item text="OnVoiceRecognition_Complete" treenode="632763686033594233" />
        <item text="OnVoiceRecognition_Failed" treenode="632763686033594238" />
      </items>
      <linkto id="632763686033594201" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Grammar1" type="literal">digits.grxml</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="literal">say_id.wav</ap>
        <ap name="UserData" type="literal">voicerec_only</ap>
      </Properties>
    </node>
    <node type="Variable" id="632763113932344134" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632763010860937860" treenode="632763010860937861" appnode="632763010860937858" handlerfor="632763010860937857">
    <node type="Start" id="632763010860937860" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="76">
      <linkto id="632763113932344147" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632763113932344147" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="188" y="75">
      <linkto id="632763128079062898" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string termCond, LogWriter log)
{
	log.Write(TraceLevel.Verbose, "PLAY completed, termCond is {0}", termCond);
	return IApp.VALUE_SUCCESS;	
}

</Properties>
    </node>
    <node type="Action" id="632763128079062898" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="396" y="76">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632763128079062899" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Play_Complete">termCond</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" startnode="632763686033594216" treenode="632763686033594217" appnode="632763686033594214" handlerfor="632763686033594213">
    <node type="Start" id="632763686033594216" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="81" y="205">
      <linkto id="632763686033594249" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632763686033594249" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="244" y="204">
      <linkto id="632763686033594250" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(ref string digits, string userData, LogWriter log)
{
	if (digits == null)
		digits = string.Empty;

	digits = digits.Replace("#", string.Empty);

	log.Write(TraceLevel.Verbose, "Gather digits: {0} from action {1}", digits, userData);

	return IApp.VALUE_SUCCESS;	
}



</Properties>
    </node>
    <node type="Action" id="632763686033594250" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="410" y="204">
      <linkto id="632763686033594251" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632763686033594251" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="572" y="204">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632763686033594252" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">digits</Properties>
    </node>
    <node type="Variable" id="632763686033594253" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnVoiceRecognition_Complete" startnode="632763686033594232" treenode="632763686033594233" appnode="632763686033594230" handlerfor="632763686033594229">
    <node type="Start" id="632763686033594232" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="103">
      <linkto id="632763686033594254" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632763686033594254" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="198" y="103">
      <linkto id="632763867035156691" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(string userData, string meaning, int score, LogWriter log)
{
	log.Write(TraceLevel.Verbose, "VoiceRec meaning is [{0}], score is [{1}], userData is  {2}", meaning, score, userData);

	return IApp.VALUE_SUCCESS;	
}
</Properties>
    </node>
    <node type="Action" id="632763686033594255" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="332" y="356">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632763686033594256" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="332" y="235">
      <linkto id="632763686033594255" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632763686033594261" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="496" y="103">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632763867035156691" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="332" y="103">
      <linkto id="632763686033594261" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632763686033594256" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">score &gt; 0</ap>
      </Properties>
    </node>
    <node type="Variable" id="632763686033594258" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632763686033594259" name="meaning" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Meaning" refType="reference" name="Metreos.MediaControl.VoiceRecognition_Complete">meaning</Properties>
    </node>
    <node type="Variable" id="632763686033594260" name="score" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="Score" refType="reference" name="Metreos.MediaControl.VoiceRecognition_Complete">score</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <canvas type="Function" name="OnPlay_Failed" show="false" startnode="632763010860937865" treenode="632763010860937866" appnode="632763010860937863" handlerfor="632763010860937862">
    <node type="Start" id="632763010860937865" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="76">
      <linkto id="632763128079062906" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632763113932344143" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="290" y="75">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632763128079062906" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="165" y="75">
      <linkto id="632763113932344143" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnVoiceRecognition_Failed" show="false" startnode="632763686033594237" treenode="632763686033594238" appnode="632763686033594235" handlerfor="632763686033594234">
    <node type="Start" id="632763686033594237" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="120">
      <linkto id="632763686033594248" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632763686033594247" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="316" y="121">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632763686033594248" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="174" y="121">
      <linkto id="632763686033594247" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" show="false" startnode="632763686033594221" treenode="632763686033594222" appnode="632763686033594219" handlerfor="632763686033594218">
    <node type="Start" id="632763686033594221" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="103">
      <linkto id="632763686033594245" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632763686033594245" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="156" y="103">
      <linkto id="632763686033594246" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632763686033594246" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="318" y="103">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>