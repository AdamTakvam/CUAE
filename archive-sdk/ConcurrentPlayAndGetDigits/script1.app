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
          <ref id="633149082295033002" actid="632763010860937867" />
          <ref id="633149082295033042" actid="633149082295033041" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632763010860937866" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632763010860937863" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632763010860937862" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="633149082295033003" actid="632763010860937867" />
          <ref id="633149082295033043" actid="633149082295033041" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632763010860937874" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632763010860937871" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632763010860937870" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="633149082295033005" actid="632763010860937880" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632763010860937879" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632763010860937876" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632763010860937875" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="633149082295033006" actid="632763010860937880" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633149082295033031" level="2" text="Metreos.MediaControl.Record_Complete: OnRecord_Complete">
        <node type="function" name="OnRecord_Complete" id="633149082295033028" path="Metreos.StockTools" />
        <node type="event" name="Record_Complete" id="633149082295033027" path="Metreos.MediaControl.Record_Complete" />
        <references>
          <ref id="633149082295033038" actid="633149082295033037" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633149082295033036" level="2" text="Metreos.MediaControl.Record_Failed: OnRecord_Failed">
        <node type="function" name="OnRecord_Failed" id="633149082295033033" path="Metreos.StockTools" />
        <node type="event" name="Record_Failed" id="633149082295033032" path="Metreos.MediaControl.Record_Failed" />
        <references>
          <ref id="633149082295033039" actid="633149082295033037" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="633149082295032994" vid="632763113932344135">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_mmsId" id="633149082295032996" vid="632763113932344137">
        <Properties type="String">g_mmsId</Properties>
      </treenode>
      <treenode text="g_connectionId" id="633149082295032998" vid="632763113932344139">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632763010860937854" treenode="632763010860937855" appnode="632763010860937852" handlerfor="632763010860937851">
    <node type="Start" id="632763010860937854" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="65" y="177">
      <linkto id="632763010860937856" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632763010860937856" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="237" y="177">
      <linkto id="632763010860937880" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="WaitForMedia" type="csharp">true</ap>
        <ap name="Conference" type="literal">false</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="MmsId">g_mmsId</rd>
        <rd field="ConnectionId">g_connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632763010860937867" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="394" y="343" mx="447" my="359">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632763010860937861" />
        <item text="OnPlay_Failed" treenode="632763010860937866" />
      </items>
      <linkto id="632763010860937883" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">enter pound sign to start recording, start to stop recording.</ap>
        <ap name="Prompt2" type="literal">sample_music.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632763010860937880" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="372" y="161" mx="446" my="177">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632763010860937874" />
        <item text="OnGatherDigits_Failed" treenode="632763010860937879" />
      </items>
      <linkto id="632763010860937867" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632763010860937883" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="443" y="544">
      <Properties final="true" type="appControl" log="On">
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
      <linkto id="632763128079062901" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string termCond, LogWriter log)
{
	log.Write(TraceLevel.Verbose, "PLAY completed, termCond is {0}", termCond);
	return IApp.VALUE_SUCCESS;	
}

</Properties>
    </node>
    <node type="Action" id="632763128079062898" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="187" y="356">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632763128079062901" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="188" y="210">
      <linkto id="632763128079062898" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632763128079062903" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">termCond</ap>
        <ap name="Value2" type="literal">eod</ap>
      </Properties>
    </node>
    <node type="Action" id="632763128079062902" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="452" y="210">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632763128079062903" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="314" y="209">
      <linkto id="632763128079062902" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Variable" id="632763128079062899" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Play_Complete">termCond</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632763010860937865" treenode="632763010860937866" appnode="632763010860937863" handlerfor="632763010860937862">
    <node type="Start" id="632763010860937865" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="76">
      <linkto id="632763128079062906" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632763113932344143" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="287" y="78">
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
  <canvas type="Function" name="OnGatherDigits_Complete" activetab="true" startnode="632763010860937873" treenode="632763010860937874" appnode="632763010860937871" handlerfor="632763010860937870">
    <node type="Start" id="632763010860937873" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="68">
      <linkto id="632763113932344149" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632763113932344149" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="149" y="68">
      <linkto id="633149082295033037" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(ref string digits, LogWriter log)
{
	if (digits == null)
		digits = string.Empty;

	digits = digits.Replace("#", string.Empty);

	log.Write(TraceLevel.Verbose, "Gather digits: {0}", digits);

	return IApp.VALUE_SUCCESS;	
}



</Properties>
    </node>
    <node type="Action" id="633149082295033037" name="Record" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="230" y="51" mx="290" my="67">
      <items count="2">
        <item text="OnRecord_Complete" treenode="633149082295033031" />
        <item text="OnRecord_Failed" treenode="633149082295033036" />
      </items>
      <linkto id="633149082295033045" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Filename" type="literal">myrecord.wav</ap>
        <ap name="AudioFileFormat" type="literal">wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondDigit" type="literal">*</ap>
        <ap name="AudioFileSampleRate" type="literal">8</ap>
        <ap name="AudioFileSampleSize" type="literal">16</ap>
        <ap name="AudioFileEncoding" type="literal">pcm</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="633149082295033045" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="437" y="69">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632763113932344148" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">digits</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632763010860937878" treenode="632763010860937879" appnode="632763010860937876" handlerfor="632763010860937875">
    <node type="Start" id="632763010860937878" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="54" y="100">
      <linkto id="632763128079062907" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632763113932344144" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="291" y="99">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632763128079062907" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="164" y="100">
      <linkto id="632763113932344144" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Complete" startnode="633149082295033030" treenode="633149082295033031" appnode="633149082295033028" handlerfor="633149082295033027">
    <node type="Start" id="633149082295033030" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="57">
      <linkto id="633149082295033041" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633149082295033041" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="174" y="41" mx="227" my="57">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632763010860937861" />
        <item text="OnPlay_Failed" treenode="632763010860937866" />
      </items>
      <linkto id="633149082295033046" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">goodbye</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="633149082295033046" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="391" y="58">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Failed" startnode="633149082295033035" treenode="633149082295033036" appnode="633149082295033033" handlerfor="633149082295033032">
    <node type="Start" id="633149082295033035" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633149082295033040" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633149082295033040" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="154" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>