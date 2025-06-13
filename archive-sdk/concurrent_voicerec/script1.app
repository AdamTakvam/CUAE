<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632798476226325834" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632798476226325831" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632798476226325830" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632798476226325846" level="2" text="Metreos.MediaControl.VoiceRecognition_Complete: OnVoiceRecognition_Complete">
        <node type="function" name="OnVoiceRecognition_Complete" id="632798476226325843" path="Metreos.StockTools" />
        <node type="event" name="VoiceRecognition_Complete" id="632798476226325842" path="Metreos.MediaControl.VoiceRecognition_Complete" />
        <references>
          <ref id="632799102359390005" actid="632798476226325852" />
          <ref id="632799102359390008" actid="632798476226325855" />
          <ref id="632799102359390024" actid="632798476226326153" />
          <ref id="632799102359390027" actid="632798476226326156" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632798476226325851" level="2" text="Metreos.MediaControl.VoiceRecognition_Failed: OnVoiceRecognition_Failed">
        <node type="function" name="OnVoiceRecognition_Failed" id="632798476226325848" path="Metreos.StockTools" />
        <node type="event" name="VoiceRecognition_Failed" id="632798476226325847" path="Metreos.MediaControl.VoiceRecognition_Failed" />
        <references>
          <ref id="632799102359390006" actid="632798476226325852" />
          <ref id="632799102359390009" actid="632798476226325855" />
          <ref id="632799102359390025" actid="632798476226326153" />
          <ref id="632799102359390028" actid="632798476226326156" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632798476226326132" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632798476226326129" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632798476226326128" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632798476226326139" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632798476226326136" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632798476226326135" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632799102359390020" actid="632798476226326145" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632798476226326144" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632798476226326141" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632798476226326140" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632799102359390021" actid="632798476226326145" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="632799102359389998" vid="632798476226325836">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632799102359390000" vid="632798476226325838">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632798476226325833" treenode="632798476226325834" appnode="632798476226325831" handlerfor="632798476226325830">
    <node type="Start" id="632798476226325833" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="105">
      <linkto id="632798476226325835" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632798476226325835" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="181" y="105">
      <linkto id="632798476226325852" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632798476226325841" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="WaitForMedia" type="literal">true</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="ConnectionId">g_connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632798476226325841" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="181" y="245">
      <linkto id="632798476226326127" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632798476226325852" name="VoiceRecognition" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="310" y="90" mx="398" my="106">
      <items count="2">
        <item text="OnVoiceRecognition_Complete" treenode="632798476226325846" />
        <item text="OnVoiceRecognition_Failed" treenode="632798476226325851" />
      </items>
      <linkto id="632798476226325855" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CommandTimeout" type="literal">3600000</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Grammar1" type="literal">help.grxml</ap>
        <ap name="UserData" type="literal">vr_help</ap>
      </Properties>
    </node>
    <node type="Action" id="632798476226325855" name="VoiceRecognition" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="574" y="90" mx="662" my="106">
      <items count="2">
        <item text="OnVoiceRecognition_Complete" treenode="632798476226325846" />
        <item text="OnVoiceRecognition_Failed" treenode="632798476226325851" />
      </items>
      <linkto id="632798476226325858" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sample_music.wav</ap>
        <ap name="VoiceBargeIn" type="literal">true</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Grammar1" type="literal">digits.grxml</ap>
        <ap name="UserData" type="literal">vr_digits</ap>
      </Properties>
    </node>
    <node type="Action" id="632798476226325858" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="658" y="252">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632798476226326127" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="181" y="391">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632798476226325840" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnVoiceRecognition_Complete" activetab="true" startnode="632798476226325845" treenode="632798476226325846" appnode="632798476226325843" handlerfor="632798476226325842">
    <node type="Start" id="632798476226325845" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="90">
      <linkto id="632798476226325861" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632798476226325861" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="171" y="90">
      <linkto id="632798476226325862" type="Labeled" style="Bezier" ortho="true" label="vr_help" />
      <linkto id="632798476226325863" type="Labeled" style="Bezier" ortho="true" label="vr_digits" />
      <linkto id="632798476226325864" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632798476226325862" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="339" y="91">
      <linkto id="632798476226326134" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(int score, string meaning, string userData, ref string action, LogWriter log)
{
      action = "none";

      if (score &lt; 900)
        action = "redo_help";

      if (meaning != "help")
        action = "redo_help";

	log.Write(TraceLevel.Verbose, "userData is {0}, score is {1}, meaning is {2}, action is {3}", userData, score, meaning, action);

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632798476226325863" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="335" y="218">
      <linkto id="632798476226326134" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(int score, string meaning, string userData, ref string action, LogWriter log)
{
      action = "none";

      if (score &lt; 500)
        action = "redo_digits";

      if (meaning != "123")
        action = "redo_digits";

	log.Write(TraceLevel.Verbose, "userData is {0}, score is {1}, meaning is {2}, action is {3}", userData, score, meaning, action);

	return IApp.VALUE_SUCCESS;
}

</Properties>
    </node>
    <node type="Action" id="632798476226325864" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="172" y="217">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632798476226326134" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="535" y="93">
      <linkto id="632798476226326145" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632798476226326153" type="Labeled" style="Bezier" ortho="true" label="redo_help" />
      <linkto id="632798476226326156" type="Labeled" style="Bezier" ortho="true" label="redo_digits" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">action</ap>
      </Properties>
    </node>
    <node type="Action" id="632798476226326145" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="485" y="202" mx="538" my="218">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632798476226326139" />
        <item text="OnPlay_Failed" treenode="632798476226326144" />
      </items>
      <linkto id="632798476226326148" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="AudioFileSampleSize" type="literal">4</ap>
        <ap name="AudioFileEncoding" type="literal">adpcm</ap>
        <ap name="Prompt1" type="literal">goodbye.vox</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="AudioFileSampleRate" type="literal">6</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632798476226326148" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="533" y="417">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632798476226326153" name="VoiceRecognition" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="703" y="80" mx="791" my="96">
      <items count="2">
        <item text="OnVoiceRecognition_Complete" treenode="632798476226325846" />
        <item text="OnVoiceRecognition_Failed" treenode="632798476226325851" />
      </items>
      <linkto id="632798476226326159" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CommandTimeout" type="literal">3600000</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Grammar1" type="literal">help.grxml</ap>
        <ap name="UserData" type="literal">vr_help</ap>
      </Properties>
    </node>
    <node type="Action" id="632798476226326156" name="VoiceRecognition" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="707" y="320" mx="795" my="336">
      <items count="2">
        <item text="OnVoiceRecognition_Complete" treenode="632798476226325846" />
        <item text="OnVoiceRecognition_Failed" treenode="632798476226325851" />
      </items>
      <linkto id="632798476226326159" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sample_music.wav</ap>
        <ap name="VoiceBargeIn" type="literal">true</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Grammar1" type="literal">digits.grxml</ap>
        <ap name="UserData" type="literal">vr_digits</ap>
      </Properties>
    </node>
    <node type="Action" id="632798476226326159" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1051" y="98">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632798476226325860" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632798476226325865" name="meaning" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Meaning" refType="reference" name="Metreos.MediaControl.VoiceRecognition_Complete">meaning</Properties>
    </node>
    <node type="Variable" id="632798476226325866" name="score" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="Score" refType="reference" name="Metreos.MediaControl.VoiceRecognition_Complete">score</Properties>
    </node>
    <node type="Variable" id="632798476226326126" name="action" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">action</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnVoiceRecognition_Failed" startnode="632798476226325850" treenode="632798476226325851" appnode="632798476226325848" handlerfor="632798476226325847">
    <node type="Start" id="632798476226325850" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="91">
      <linkto id="632798476226325859" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632798476226325859" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="235" y="91">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632798476226326131" treenode="632798476226326132" appnode="632798476226326129" handlerfor="632798476226326128">
    <node type="Start" id="632798476226326131" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="148">
      <linkto id="632798476226326133" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632798476226326133" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="226" y="148">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632798476226326138" treenode="632798476226326139" appnode="632798476226326136" handlerfor="632798476226326135">
    <node type="Start" id="632798476226326138" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="50" y="94">
      <linkto id="632798476226326151" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632798476226326151" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="168" y="94">
      <linkto id="632798476226326152" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632798476226326152" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="317" y="94">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632798476226326143" treenode="632798476226326144" appnode="632798476226326141" handlerfor="632798476226326140">
    <node type="Start" id="632798476226326143" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="38" y="85">
      <linkto id="632798476226326149" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632798476226326149" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="187" y="85">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>