<Application name="IncomingRequest" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="IncomingRequest">
    <outline>
      <treenode type="evh" id="633151727981563024" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="633151727981563021" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633151727981563020" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ResourceWaster</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="633151770643355194" level="2" text="Metreos.MediaControl.PlayTone_Complete: OnPlayTone_Complete">
        <node type="function" name="OnPlayTone_Complete" id="633151770643355191" path="Metreos.StockTools" />
        <node type="event" name="PlayTone_Complete" id="633151770643355190" path="Metreos.MediaControl.PlayTone_Complete" />
        <references>
          <ref id="633193260922044619" actid="633151770643355200" />
          <ref id="633193260922044631" actid="633161104896696170" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633151770643355199" level="2" text="Metreos.MediaControl.PlayTone_Failed: OnPlayTone_Failed">
        <node type="function" name="OnPlayTone_Failed" id="633151770643355196" path="Metreos.StockTools" />
        <node type="event" name="PlayTone_Failed" id="633151770643355195" path="Metreos.MediaControl.PlayTone_Failed" />
        <references>
          <ref id="633193260922044620" actid="633151770643355200" />
          <ref id="633193260922044632" actid="633161104896696170" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633151770643355222" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="633151770643355219" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="633151770643355218" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="633151770643355227" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="633151770643355224" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="633151770643355223" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="633151770643355232" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="633151770643355229" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="633151770643355228" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="633151770643355255" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="633151770643355252" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="633151770643355251" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="633193260922044639" actid="633151770643355261" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633151770643355260" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="633151770643355257" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="633151770643355256" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="633193260922044640" actid="633151770643355261" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633151770643355277" level="2" text="Metreos.MediaControl.VoiceRecognition_Complete: OnVoiceRecognition_Complete">
        <node type="function" name="OnVoiceRecognition_Complete" id="633151770643355274" path="Metreos.StockTools" />
        <node type="event" name="VoiceRecognition_Complete" id="633151770643355273" path="Metreos.MediaControl.VoiceRecognition_Complete" />
        <references>
          <ref id="633193260922044666" actid="633151770643355283" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633151770643355282" level="2" text="Metreos.MediaControl.VoiceRecognition_Failed: OnVoiceRecognition_Failed">
        <node type="function" name="OnVoiceRecognition_Failed" id="633151770643355279" path="Metreos.StockTools" />
        <node type="event" name="VoiceRecognition_Failed" id="633151770643355278" path="Metreos.MediaControl.VoiceRecognition_Failed" />
        <references>
          <ref id="633193260922044667" actid="633151770643355283" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633154218393036879" level="2" text="Metreos.MediaControl.Record_Complete: OnRecord_Complete">
        <node type="function" name="OnRecord_Complete" id="633154218393036876" path="Metreos.StockTools" />
        <node type="event" name="Record_Complete" id="633154218393036875" path="Metreos.MediaControl.Record_Complete" />
        <references>
          <ref id="633193260922044628" actid="633154288648683128" />
          <ref id="633193260922044657" actid="633154218393036885" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633154218393036884" level="2" text="Metreos.MediaControl.Record_Failed: OnRecord_Failed">
        <node type="function" name="OnRecord_Failed" id="633154218393036881" path="Metreos.StockTools" />
        <node type="event" name="Record_Failed" id="633154218393036880" path="Metreos.MediaControl.Record_Failed" />
        <references>
          <ref id="633193260922044629" actid="633154288648683128" />
          <ref id="633193260922044658" actid="633154218393036885" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="fun" id="633151770643355082" level="1" text="WasteRTP">
        <node type="function" name="WasteRTP" id="633151770643355079" path="Metreos.StockTools" />
        <calls>
          <ref actid="633151770643355078" />
        </calls>
      </treenode>
      <treenode type="fun" id="633151770643355090" level="1" text="WasteERTP">
        <node type="function" name="WasteERTP" id="633151770643355087" path="Metreos.StockTools" />
        <calls>
          <ref actid="633151770643355091" />
        </calls>
      </treenode>
      <treenode type="fun" id="633151770643355098" level="1" text="WasteVoice">
        <node type="function" name="WasteVoice" id="633151770643355095" path="Metreos.StockTools" />
        <calls>
          <ref actid="633151770643355093" />
        </calls>
      </treenode>
      <treenode type="fun" id="633151770643355110" level="1" text="WasteTTS">
        <node type="function" name="WasteTTS" id="633151770643355107" path="Metreos.StockTools" />
        <calls>
          <ref actid="633151770643355105" />
        </calls>
      </treenode>
      <treenode type="fun" id="633151770643355114" level="1" text="WasteConference">
        <node type="function" name="WasteConference" id="633151770643355111" path="Metreos.StockTools" />
        <calls>
          <ref actid="633151770643355099" />
        </calls>
      </treenode>
      <treenode type="fun" id="633151770643355118" level="1" text="WasteSpeech">
        <node type="function" name="WasteSpeech" id="633151770643355115" path="Metreos.StockTools" />
        <calls>
          <ref actid="633151770643355101" />
        </calls>
      </treenode>
      <treenode type="fun" id="633151770643355122" level="1" text="WasteScripts">
        <node type="function" name="WasteScripts" id="633151770643355119" path="Metreos.StockTools" />
        <calls>
          <ref actid="633151770643355103" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_remoteHost" id="633193260922044574" vid="633151733514375471">
        <Properties type="String">g_remoteHost</Properties>
      </treenode>
      <treenode text="g_testType" id="633193260922044576" vid="633151733514375473">
        <Properties type="String">g_testType</Properties>
      </treenode>
      <treenode text="g_numResources" id="633193260922044578" vid="633151770643355076">
        <Properties type="Int" defaultInitWith="1">g_numResources</Properties>
      </treenode>
      <treenode text="g_time" id="633193260922044580" vid="633151770643355139">
        <Properties type="Int" defaultInitWith="5000">g_time</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="633151727981563023" treenode="633151727981563024" appnode="633151727981563021" handlerfor="633151727981563020">
    <node type="Start" id="633151727981563023" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="110" y="78">
      <linkto id="633151733514375475" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633151733514375475" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="110" y="227">
      <linkto id="633151733514375477" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">queryParams["testType"]</ap>
        <ap name="Value2" type="csharp">queryParams["numResources"]</ap>
        <ap name="Value3" type="variable">remoteHost</ap>
        <ap name="Value4" type="csharp">queryParams["time"]</ap>
        <rd field="ResultData">g_testType</rd>
        <rd field="ResultData2">g_numResources</rd>
        <rd field="ResultData3">g_remoteHost</rd>
        <rd field="ResultData4">g_time</rd>
        <log condition="entry" on="true" level="Verbose" type="literal">Parsing query parameters.</log>
        <log condition="exit" on="true" level="Info" type="csharp">"testType=" + g_testType + " numResources=" + g_numResources + " time=" + g_time</log>
      </Properties>
    </node>
    <node type="Action" id="633151733514375477" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="110" y="369">
      <linkto id="633151770643355078" type="Labeled" style="Bezier" ortho="true" label="RTP" />
      <linkto id="633151770643355105" type="Labeled" style="Bezier" ortho="true" label="TTS" />
      <linkto id="633151770643355091" type="Labeled" style="Bezier" ortho="true" label="ERTP" />
      <linkto id="633151770643355093" type="Labeled" style="Bezier" ortho="true" label="Voice" />
      <linkto id="633151770643355099" type="Labeled" style="Bezier" ortho="true" label="Conference" />
      <linkto id="633151770643355101" type="Labeled" style="Bezier" ortho="true" label="Speech" />
      <linkto id="633151770643355103" type="Labeled" style="Bezier" ortho="true" label="Script" />
      <linkto id="633151918543241185" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">g_testType</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Deciding type of test</log>
      </Properties>
    </node>
    <node type="Action" id="633151770643355078" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="446.825836" y="116" mx="484" my="132">
      <items count="1">
        <item text="WasteRTP" />
      </items>
      <linkto id="633151770643355123" type="Labeled" style="Bezier" ortho="true" label="SUCCESS" />
      <linkto id="633151770643355124" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">WasteRTP</ap>
        <log condition="entry" on="true" level="Info" type="literal">Calling WasteRTP</log>
      </Properties>
    </node>
    <node type="Action" id="633151770643355091" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="444.825836" y="197" mx="482" my="213">
      <items count="1">
        <item text="WasteERTP" />
      </items>
      <linkto id="633151770643355124" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="633151770643355123" type="Labeled" style="Bezier" ortho="true" label="SUCCESS" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">WasteERTP</ap>
        <log condition="entry" on="true" level="Info" type="literal">Calling WasteERTP</log>
      </Properties>
    </node>
    <node type="Action" id="633151770643355093" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="439.825836" y="401" mx="477" my="417">
      <items count="1">
        <item text="WasteVoice" />
      </items>
      <linkto id="633151770643355124" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="633151770643355123" type="Labeled" style="Bezier" ortho="true" label="SUCCESS" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">WasteVoice</ap>
        <log condition="entry" on="true" level="Info" type="literal">Calling WasteVoice</log>
      </Properties>
    </node>
    <node type="Action" id="633151770643355099" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="422.973633" y="499" mx="475" my="515">
      <items count="1">
        <item text="WasteConference" />
      </items>
      <linkto id="633151770643355124" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="633151770643355123" type="Labeled" style="Bezier" ortho="true" label="SUCCESS" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">WasteConference</ap>
        <log condition="entry" on="true" level="Info" type="literal">Calling WasteConference</log>
      </Properties>
    </node>
    <node type="Action" id="633151770643355101" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="439.643555" y="600" mx="481" my="616">
      <items count="1">
        <item text="WasteSpeech" />
      </items>
      <linkto id="633151770643355124" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="633151770643355123" type="Labeled" style="Bezier" ortho="true" label="SUCCESS" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">WasteSpeech</ap>
        <log condition="entry" on="true" level="Info" type="literal">Calling WasteSpeech</log>
      </Properties>
    </node>
    <node type="Action" id="633151770643355103" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="437.6211" y="684" mx="477" my="700">
      <items count="1">
        <item text="WasteScripts" />
      </items>
      <linkto id="633151770643355124" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="633151770643355123" type="Labeled" style="Bezier" ortho="true" label="SUCCESS" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">WasteScripts</ap>
        <log condition="entry" on="true" level="Info" type="literal">Calling WasteScripts</log>
      </Properties>
    </node>
    <node type="Action" id="633151770643355105" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="447.825836" y="33" mx="485" my="49">
      <items count="1">
        <item text="WasteTTS" />
      </items>
      <linkto id="633151770643355124" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="633151770643355123" type="Labeled" style="Bezier" ortho="true" label="SUCCESS" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">WasteTTS</ap>
        <log condition="entry" on="true" level="Info" type="literal">Calling WasteTTS</log>
      </Properties>
    </node>
    <node type="Action" id="633151770643355123" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="929" y="58">
      <linkto id="633151770643355142" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">g_remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="literal">The resources were wasted successfully</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="633151770643355124" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="967" y="639">
      <linkto id="633151770643355142" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">g_remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="literal">The resources were NOT wasted</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Comment" id="633151770643355126" text="Send Successful response" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="758" y="36" />
    <node type="Comment" id="633151770643355127" text="Send Unsuccessful response" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="884" y="673" />
    <node type="Action" id="633151770643355142" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="988" y="342">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633151918543241185" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="477.999939" y="327.673157">
      <linkto id="633151770643355142" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">g_remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">"Query parameters were not correct. " + queryParams </ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Comment" id="633151918543241186" text="Send Bad Query Params Response&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="412" y="283" />
    <node type="Variable" id="633151733514375467" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">queryParams</Properties>
    </node>
    <node type="Variable" id="633151733514375468" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="WasteRTP" startnode="633151770643355081" treenode="633151770643355082" appnode="633151770643355079" handlerfor="633154218393036880">
    <node type="Loop" id="633151770643355130" name="Loop" text="loop (var)" cx="270" cy="238" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="63" y="211" mx="198" my="330">
      <linkto id="633154298512133895" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="633151770643355141" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">g_numResources</Properties>
    </node>
    <node type="Loop" id="633153303015976774" name="Loop" text="loop (expr)" cx="227" cy="240" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="516" y="211" mx="630" my="331">
      <linkto id="633153303015976775" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="633151770643355131" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">connectionList</Properties>
    </node>
    <node type="Start" id="633151770643355081" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633151770643355130" port="1" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633151770643355131" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="910" y="343">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">SUCCESS</ap>
      </Properties>
    </node>
    <node type="Action" id="633151770643355141" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="428" y="329">
      <linkto id="633153303015976774" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="variable">g_time</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Sleeping...</log>
      </Properties>
    </node>
    <node type="Action" id="633153303015976771" name="CustomCode" container="633151770643355130" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="257" y="381">
      <linkto id="633151770643355130" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(String connectionId, StringCollection connectionList)
{
	//add connectionId to connectionList
	connectionList.Add(connectionId);

	return IApp.VALUE_SUCCESS;
}</Properties>
    </node>
    <node type="Action" id="633153303015976775" name="DeleteConnection" container="633153303015976774" class="MaxActionNode" group="" path="Metreos.MediaControl" x="611" y="333">
      <linkto id="633153303015976774" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="csharp">loopEnum.Current</ap>
      </Properties>
    </node>
    <node type="Action" id="633154298512133895" name="CreateConnection" container="633151770643355130" class="MaxActionNode" group="" path="Metreos.MediaControl" x="144" y="252">
      <linkto id="633153303015976771" type="Labeled" style="Bezier" label="Success" />
      <linkto id="633153303015976771" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxIP" type="literal">169.254.100.100</ap>
        <ap name="MediaTxPort" type="literal">9999</ap>
        <rd field="ConnectionId">connectionId</rd>
        <log condition="success" on="true" level="Verbose" type="csharp">"SUCCESSFULLY created connection# " +  (loopIndex +1)</log>
        <log condition="default" on="true" level="Warning" type="csharp">"FAILURE creating connection# " + (loopIndex + 1)</log>
      </Properties>
    </node>
    <node type="Variable" id="633153303015976772" name="connectionList" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="StringCollection" refType="reference">connectionList</Properties>
    </node>
    <node type="Variable" id="633153303015976773" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="WasteERTP" startnode="633151770643355089" treenode="633151770643355090" appnode="633151770643355087" handlerfor="633154218393036880">
    <node type="Loop" id="633151770643355143" name="Loop" text="loop (var)" cx="313" cy="216" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="46" y="223" mx="202" my="331">
      <linkto id="633154298512133896" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="633151770643355146" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">g_numResources</Properties>
    </node>
    <node type="Loop" id="633153303015976780" name="Loop" text="loop (expr)" cx="231" cy="232" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="522" y="227" mx="638" my="343">
      <linkto id="633153303015976781" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="633151770643355145" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">connectionList</Properties>
    </node>
    <node type="Start" id="633151770643355089" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633151770643355143" port="1" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633151770643355145" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="868" y="342">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">SUCCESS</ap>
      </Properties>
    </node>
    <node type="Action" id="633151770643355146" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="430" y="330">
      <linkto id="633153303015976780" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="variable">g_time</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Sleeping...</log>
      </Properties>
    </node>
    <node type="Action" id="633153303015976778" name="CustomCode" container="633151770643355143" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="259.90625" y="334">
      <linkto id="633151770643355143" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(String connectionId, StringCollection connectionList)
{
	//add connectionId to connectionList
	connectionList.Add(connectionId);

	return IApp.VALUE_SUCCESS;
}</Properties>
    </node>
    <node type="Action" id="633153303015976781" name="DeleteConnection" container="633153303015976780" class="MaxActionNode" group="" path="Metreos.MediaControl" x="627" y="344">
      <linkto id="633153303015976780" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="csharp">loopEnum.Current</ap>
      </Properties>
    </node>
    <node type="Action" id="633154298512133896" name="CreateConnection" container="633151770643355143" class="MaxActionNode" group="" path="Metreos.MediaControl" x="141" y="329">
      <linkto id="633153303015976778" type="Labeled" style="Bezier" label="Success" />
      <linkto id="633153303015976778" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaRxCodec" type="literal">G729</ap>
        <ap name="MediaTxIP" type="literal">169.254.100.100</ap>
        <ap name="MediaTxPort" type="literal">9999</ap>
        <ap name="MediaTxCodec" type="literal">G729</ap>
        <rd field="ConnectionId">connectionId</rd>
        <log condition="success" on="true" level="Verbose" type="csharp">"SUCCESSFULLY created connection# " +  (loopIndex +1)</log>
        <log condition="default" on="true" level="Warning" type="csharp">"FAILURE creating connection# " + (loopIndex +1)</log>
      </Properties>
    </node>
    <node type="Variable" id="633153303015976776" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
    <node type="Variable" id="633153303015976777" name="connectionList" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="StringCollection" refType="reference">connectionList</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="WasteVoice" activetab="true" startnode="633151770643355097" treenode="633151770643355098" appnode="633151770643355095" handlerfor="633154218393036880">
    <node type="Loop" id="633154288648683101" name="Loop" text="loop (expr)" cx="388.223969" cy="421" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="234.776031" y="39" mx="429" my="250">
      <linkto id="633154288648683102" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="633154288648683105" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="csharp">g_numResources - 2</Properties>
    </node>
    <node type="Loop" id="633154288648683132" name="Loop" text="loop (expr)" cx="209" cy="235" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="728" y="101" mx="832" my="218">
      <linkto id="633154288648683109" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="633151770643355154" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">connectionList</Properties>
    </node>
    <node type="Start" id="633151770643355097" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633154288648683124" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633151770643355154" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1043" y="208">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">SUCCESS</ap>
      </Properties>
    </node>
    <node type="Action" id="633151770643355200" name="PlayTone" container="633154288648683101" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="453" y="79" mx="519" my="95">
      <items count="2">
        <item text="OnPlayTone_Complete" treenode="633151770643355194" />
        <item text="OnPlayTone_Failed" treenode="633151770643355199" />
      </items>
      <linkto id="633193260922044716" type="Labeled" style="Bezier" label="Success" />
      <linkto id="633193260922044716" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Frequency2" type="literal">1200</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="Frequency1" type="literal">800</ap>
        <ap name="Amplitude1" type="literal">-20</ap>
        <ap name="Amplitude2" type="literal">-20</ap>
        <ap name="Duration" type="literal">5000</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
        <log condition="success" on="true" level="Verbose" type="csharp">"SUCCESS using Voice resource# " + (loopIndex +3)</log>
        <log condition="default" on="true" level="Warning" type="csharp">"FAILURE using Voice resource# " + (loopIndex + 3)</log>
      </Properties>
    </node>
    <node type="Action" id="633154288648683102" name="CreateConnection" container="633154288648683101" class="MaxActionNode" group="" path="Metreos.MediaControl" x="315.776031" y="149">
      <linkto id="633154288648683104" type="Labeled" style="Bezier" label="Success" />
      <linkto id="633154288648683104" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxIP" type="literal">169.254.100.100</ap>
        <ap name="MediaTxPort" type="literal">9999</ap>
        <rd field="ConnectionId">connectionId</rd>
        <log condition="success" on="true" level="Verbose" type="csharp">"SUCCESS - making connection# "+(loopIndex+2)+" (RTP) so that a playtone (voice) can be used "</log>
        <log condition="default" on="true" level="Warning" type="csharp">"FAILURE - making connection# " +(loopIndex+2)+"  (RTP) so that a playtone (Voice) can be used "</log>
      </Properties>
    </node>
    <node type="Action" id="633154288648683104" name="CustomCode" container="633154288648683101" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="380.776031" y="71">
      <linkto id="633151770643355200" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(String connectionId, StringCollection connectionList)
{
	//add connectionId to connectionList
	connectionList.Add(connectionId);

	return IApp.VALUE_SUCCESS;
}</Properties>
    </node>
    <node type="Action" id="633154288648683105" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="674.776062" y="273">
      <linkto id="633154288648683132" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="variable">g_time</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Sleeping...</log>
      </Properties>
    </node>
    <node type="Action" id="633154288648683109" name="DeleteConnection" container="633154288648683132" class="MaxActionNode" group="" path="Metreos.MediaControl" x="830.776062" y="200">
      <linkto id="633154288648683132" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="csharp">loopEnum.Current</ap>
      </Properties>
    </node>
    <node type="Action" id="633154288648683124" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="132.776031" y="49">
      <linkto id="633154288648683126" type="Labeled" style="Bezier" label="Success" />
      <linkto id="633154288648683126" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="Off">
        <ap name="MediaTxIP" type="literal">169.254.100.100</ap>
        <ap name="MediaTxPort" type="literal">9999</ap>
        <rd field="ConnectionId">connectionId</rd>
        <log condition="success" on="true" level="Verbose" type="csharp">"SUCCESS - making connection# 1 (RTP) so that a playtone (Voice) can be used "</log>
        <log condition="default" on="true" level="Warning" type="csharp">"FAILURE - making connection# 1 (RTP) so that a playtone (Voice) can be used "</log>
      </Properties>
    </node>
    <node type="Action" id="633154288648683126" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="55.77603" y="141">
      <linkto id="633154288648683128" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(String connectionId, StringCollection connectionList)
{
	//add connectionId to connectionList
	connectionList.Add(connectionId);

	return IApp.VALUE_SUCCESS;
}</Properties>
    </node>
    <node type="Action" id="633154288648683128" name="Record" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="14" y="252" mx="74" my="268">
      <items count="2">
        <item text="OnRecord_Complete" treenode="633154218393036879" />
        <item text="OnRecord_Failed" treenode="633154218393036884" />
      </items>
      <linkto id="633161104896696170" type="Labeled" style="Bezier" label="default" />
      <linkto id="633161104896696170" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="TermCondMaxTime" type="variable">g_time</ap>
        <ap name="Expires" type="literal">1</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
        <log condition="success" on="true" level="Verbose" type="csharp">"SUCCESS using Voice resource# 1"</log>
        <log condition="default" on="true" level="Warning" type="csharp">"FAILURE using Voice resource# 1"</log>
      </Properties>
    </node>
    <node type="Action" id="633161104896696170" name="PlayTone" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="23" y="439" mx="89" my="455">
      <items count="2">
        <item text="OnPlayTone_Complete" treenode="633151770643355194" />
        <item text="OnPlayTone_Failed" treenode="633151770643355199" />
      </items>
      <linkto id="633193260922044715" type="Labeled" style="Bezier" label="default" />
      <linkto id="633193260922044715" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Frequency2" type="literal">1200</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="Frequency1" type="literal">800</ap>
        <ap name="Amplitude1" type="literal">-20</ap>
        <ap name="Amplitude2" type="literal">-20</ap>
        <ap name="Duration" type="literal">5000</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
        <log condition="success" on="true" level="Verbose" type="csharp">"SUCCESS using Voice resource# 2"</log>
        <log condition="default" on="true" level="Warning" type="csharp">"FAILURE using Voice resource# 2"</log>
      </Properties>
    </node>
    <node type="Action" id="633193260922044715" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="170" y="289">
      <linkto id="633154288648683101" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="literal">0</ap>
      </Properties>
    </node>
    <node type="Action" id="633193260922044716" name="Sleep" container="633154288648683101" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="536" y="266">
      <linkto id="633154288648683101" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="literal">0</ap>
      </Properties>
    </node>
    <node type="Variable" id="633154288648683122" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
    <node type="Variable" id="633154288648683123" name="connectionList" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="StringCollection" refType="reference">connectionList</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="WasteTTS" startnode="633151770643355109" treenode="633151770643355110" appnode="633151770643355107" handlerfor="633154218393036880">
    <node type="Loop" id="633151770643355242" name="Loop" text="loop (var)" cx="380" cy="216" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="96" y="47" mx="286" my="155">
      <linkto id="633154298512133898" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="633151770643355245" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">g_numResources</Properties>
    </node>
    <node type="Loop" id="633154298512133902" name="Loop" text="loop (expr)" cx="231" cy="232" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="629" y="51" mx="744" my="167">
      <linkto id="633154298512133903" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="633151770643355244" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">connectionList</Properties>
    </node>
    <node type="Start" id="633151770643355109" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="38" y="46">
      <linkto id="633151770643355242" port="1" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633151770643355244" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="961" y="166">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">SUCCESS</ap>
      </Properties>
    </node>
    <node type="Action" id="633151770643355245" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="557" y="168">
      <linkto id="633154298512133902" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="variable">g_time</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Sleeping...</log>
      </Properties>
    </node>
    <node type="Action" id="633151770643355261" name="Play" container="633151770643355242" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="329" y="131" mx="382" my="147">
      <items count="2">
        <item text="OnPlay_Complete" treenode="633151770643355255" />
        <item text="OnPlay_Failed" treenode="633151770643355260" />
      </items>
      <linkto id="633193260922044717" type="Labeled" style="Bezier" label="Success" />
      <linkto id="633193260922044717" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">The Play action allows one to specify up to 3 prompts, which can be either free-formed strings or the names of WAV files. If more than one prompt is specified, the format of the supplied WAV files must all be the same (sample rate, sample size, encoding, mono, file type) and also match the format of generated TTS WAV files, which is a sample rate of 8 kHz, a sample size of 8 bit, and an audio format of PCM. For instance, one can not specify VOX prompts in conjunction with text-to-spech prompts</ap>
        <ap name="Prompt3" type="literal">The Play action allows one to specify up to 3 prompts, which can be either free-formed strings or the names of WAV files. If more than one prompt is specified, the format of the supplied WAV files must all be the same (sample rate, sample size, encoding, mono, file type) and also match the format of generated TTS WAV files, which is a sample rate of 8 kHz, a sample size of 8 bit, and an audio format of PCM. For instance, one can not specify VOX prompts in conjunction with text-to-spech prompts</ap>
        <ap name="Prompt1" type="literal">The Play action allows one to specify up to 3 prompts, which can be either free-formed strings or the names of WAV files. If more than one prompt is specified, the format of the supplied WAV files must all be the same (sample rate, sample size, encoding, mono, file type) and also match the format of generated TTS WAV files, which is a sample rate of 8 kHz, a sample size of 8 bit, and an audio format of PCM. For instance, one can not specify VOX prompts in conjunction with text-to-spech prompts</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
        <log condition="success" on="true" level="Verbose" type="csharp">"SUCCESS playing tts# " + (loopIndex + 1)</log>
        <log condition="default" on="true" level="Warning" type="csharp">"FAILURE playing tts# " + (loopIndex + 1)</log>
      </Properties>
    </node>
    <node type="Action" id="633154298512133898" name="CreateConnection" container="633151770643355242" class="MaxActionNode" group="" path="Metreos.MediaControl" x="186" y="154">
      <linkto id="633154298512133899" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxIP" type="literal">169.254.100.100</ap>
        <ap name="MediaTxPort" type="literal">9999</ap>
        <rd field="ConnectionId">connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="633154298512133899" name="CustomCode" container="633151770643355242" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="280.90625" y="97">
      <linkto id="633151770643355261" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(String connectionId, StringCollection connectionList)
{
	//add connectionId to connectionList
	connectionList.Add(connectionId);

	return IApp.VALUE_SUCCESS;
}</Properties>
    </node>
    <node type="Action" id="633154298512133903" name="DeleteConnection" container="633154298512133902" class="MaxActionNode" group="" path="Metreos.MediaControl" x="734" y="168">
      <linkto id="633154298512133902" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="csharp">loopEnum.Current</ap>
      </Properties>
    </node>
    <node type="Action" id="633193260922044717" name="Sleep" container="633151770643355242" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="436" y="73">
      <linkto id="633151770643355242" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="literal">0</ap>
      </Properties>
    </node>
    <node type="Variable" id="633151954596525329" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
    <node type="Variable" id="633154298512133901" name="connectionList" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="StringCollection" refType="reference">connectionList</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="WasteConference" startnode="633151770643355113" treenode="633151770643355114" appnode="633151770643355111" handlerfor="633154218393036880">
    <node type="Loop" id="633151770643355160" name="Loop" text="loop (expr)" cx="313" cy="216" entry="4" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="198" y="166" mx="354" my="274">
      <linkto id="633154218393036885" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="633151770643355174" fromport="4" type="Basic" style="Bezier" ortho="true" />
      <Properties iteratorType="int" type="csharp">g_numResources - 2</Properties>
    </node>
    <node type="Start" id="633151770643355113" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="52" y="36">
      <linkto id="633151770643355172" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633151770643355162" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="994" y="291">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">SUCCESS</ap>
      </Properties>
    </node>
    <node type="Action" id="633151770643355163" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="738" y="292">
      <linkto id="633153303015977008" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="variable">g_time</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Sleeping...</log>
      </Properties>
    </node>
    <node type="Action" id="633151770643355167" name="CreateConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="62" y="249">
      <linkto id="633193260922044712" type="Labeled" style="Bezier" label="default" />
      <linkto id="633193260922044712" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Hairpin" type="literal">False</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <rd field="ConferenceId">conferenceId</rd>
        <log condition="success" on="true" level="Verbose" type="csharp">"SUCCESS creating conference"</log>
        <log condition="default" on="true" level="Warning" type="csharp">"FAILURE creating conference"</log>
      </Properties>
    </node>
    <node type="Action" id="633151770643355172" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="149" y="49">
      <linkto id="633153303015977004" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxIP" type="literal">169.254.100.100</ap>
        <ap name="MediaTxPort" type="literal">9999</ap>
        <rd field="ConnectionId">connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="633151770643355174" name="CreateConnection" container="633151770643355160" class="MaxActionNode" group="" path="Metreos.MediaControl" x="279" y="276">
      <linkto id="633153303015977005" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxIP" type="literal">169.254.100.100</ap>
        <ap name="MediaTxPort" type="literal">9999</ap>
        <rd field="ConnectionId">connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="633151770643355176" name="JoinConference" container="633151770643355160" class="MaxActionNode" group="" path="Metreos.MediaControl" x="390" y="280">
      <linkto id="633193260922044713" type="Labeled" style="Bezier" label="default" />
      <linkto id="633193260922044713" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="ConferenceId" type="variable">conferenceId</ap>
        <ap name="Hairpin" type="literal">False</ap>
        <log condition="success" on="true" level="Verbose" type="csharp">"SUCCESS adding resource# " + (loopIndex + 2) + " to conference"</log>
        <log condition="default" on="true" level="Info" type="literal">"FAILURE adding resource# " + (loopIndex + 2) + " to conference"</log>
      </Properties>
    </node>
    <node type="Action" id="633153303015977004" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="58.0000038" y="130">
      <linkto id="633151770643355167" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(String connectionId, StringCollection connectionList)
{
	//add connectionId to connectionList
	connectionList.Add(connectionId);

	return IApp.VALUE_SUCCESS;
}</Properties>
    </node>
    <node type="Action" id="633153303015977005" name="CustomCode" container="633151770643355160" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="344" y="198">
      <linkto id="633151770643355176" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(String connectionId, StringCollection connectionList)
{
	//add connectionId to connectionList
	connectionList.Add(connectionId);

	return IApp.VALUE_SUCCESS;
}</Properties>
    </node>
    <node type="Action" id="633153303015977008" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="876" y="291">
      <linkto id="633151770643355162" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConferenceId" type="variable">conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="633154218393036885" name="Record" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="562" y="261" mx="622" my="277">
      <items count="2">
        <item text="OnRecord_Complete" treenode="633154218393036879" />
        <item text="OnRecord_Failed" treenode="633154218393036884" />
      </items>
      <linkto id="633151770643355163" type="Labeled" style="Bezier" label="Success" />
      <linkto id="633151770643355163" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxTime" type="variable">g_time</ap>
        <ap name="ConferenceId" type="variable">conferenceId</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
        <log condition="success" on="true" level="Verbose" type="csharp">"SUCCESS using last conference resource"</log>
        <log condition="default" on="true" level="Warning" type="csharp">"FAILURE using last conference resource"</log>
      </Properties>
    </node>
    <node type="Action" id="633193260922044712" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="188" y="507">
      <linkto id="633151770643355160" port="4" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="literal">0</ap>
      </Properties>
    </node>
    <node type="Action" id="633193260922044713" name="Sleep" container="633151770643355160" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="458" y="345">
      <linkto id="633151770643355160" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="literal">0</ap>
      </Properties>
    </node>
    <node type="Variable" id="633151770643355168" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
    <node type="Variable" id="633151770643355173" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">conferenceId</Properties>
    </node>
    <node type="Variable" id="633153303015977002" name="connectionList" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="StringCollection" refType="reference">connectionList</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="WasteSpeech" startnode="633151770643355117" treenode="633151770643355118" appnode="633151770643355115" handlerfor="633154218393036880">
    <node type="Loop" id="633151770643355264" name="Loop" text="loop (expr)" cx="313" cy="216" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="115.75" y="136" mx="272" my="244">
      <linkto id="633151770643355265" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="633151770643355268" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="csharp">g_numResources - 1</Properties>
    </node>
    <node type="Loop" id="633154298512133905" name="Loop" text="loop (expr)" cx="231" cy="232" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="564" y="122" mx="680" my="238">
      <linkto id="633154298512133906" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="633151770643355267" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">connectionList</Properties>
    </node>
    <node type="Start" id="633151770643355117" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="173">
      <linkto id="633151770643355264" port="1" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633151770643355265" name="CreateConnection" container="633151770643355264" class="MaxActionNode" group="" path="Metreos.MediaControl" x="196.75" y="246">
      <linkto id="633154298512133908" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxIP" type="literal">169.254.100.100</ap>
        <ap name="MediaTxPort" type="literal">9999</ap>
        <rd field="ConnectionId">connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="633151770643355267" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="837.75" y="260">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">SUCCESS</ap>
      </Properties>
    </node>
    <node type="Action" id="633151770643355283" name="VoiceRecognition" container="633151770643355264" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="218" y="263" mx="306" my="279">
      <items count="2">
        <item text="OnVoiceRecognition_Complete" treenode="633151770643355277" />
        <item text="OnVoiceRecognition_Failed" treenode="633151770643355282" />
      </items>
      <linkto id="633193260922044718" type="Labeled" style="Bezier" label="default" />
      <linkto id="633193260922044718" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="Grammar1" type="literal">digits.grxml</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
        <log condition="success" on="true" level="Verbose" type="csharp">"SUCCESS using Speech resource# " + (loopIndex +1)</log>
        <log condition="default" on="true" level="Warning" type="csharp">"FAILURE using Speech resource# " + (loopIndex +1)</log>
      </Properties>
    </node>
    <node type="Action" id="633151770643355268" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="513.75" y="243">
      <linkto id="633154298512133905" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="variable">g_time</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Sleeping...</log>
      </Properties>
    </node>
    <node type="Action" id="633154298512133906" name="DeleteConnection" container="633154298512133905" class="MaxActionNode" group="" path="Metreos.MediaControl" x="669" y="239">
      <linkto id="633154298512133905" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="csharp">loopEnum.Current</ap>
      </Properties>
    </node>
    <node type="Action" id="633154298512133908" name="CustomCode" container="633151770643355264" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="261.90625" y="189">
      <linkto id="633151770643355283" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(String connectionId, StringCollection connectionList)
{
	//add connectionId to connectionList
	connectionList.Add(connectionId);

	return IApp.VALUE_SUCCESS;
}</Properties>
    </node>
    <node type="Action" id="633193260922044718" name="Sleep" container="633151770643355264" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="382" y="164">
      <linkto id="633151770643355264" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="literal">0</ap>
      </Properties>
    </node>
    <node type="Variable" id="633151770643355286" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
    <node type="Variable" id="633154298512133910" name="connectionList" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="StringCollection" refType="reference">connectionList</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="WasteScripts" startnode="633151770643355121" treenode="633151770643355122" appnode="633151770643355119" handlerfor="633154218393036880">
    <node type="Loop" id="633151770643355465" name="Loop" text="loop (expr)" cx="134" cy="122" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="154" y="32" mx="221" my="93">
      <linkto id="633151770643355483" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="633151770643355469" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="csharp">g_numResources - 1</Properties>
    </node>
    <node type="Start" id="633151770643355121" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="79">
      <linkto id="633151770643355465" port="1" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633151770643355468" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="497" y="90">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">SUCCESS</ap>
      </Properties>
    </node>
    <node type="Action" id="633151770643355469" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="395" y="89">
      <linkto id="633151770643355468" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="variable">g_time</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Sleeping...</log>
      </Properties>
    </node>
    <node type="Action" id="633151770643355483" name="SendEvent" container="633151770643355465" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="210" y="87">
      <linkto id="633151770643355465" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="url" type="literal">/WastedScript</ap>
        <ap name="EventName" type="literal">Metreos.Providers.Http.GotRequest</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Spawning WastedScript #" + (loopIndex +2)</log>
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <canvas type="Function" name="OnVoiceRecognition_Failed" show="false" startnode="633151770643355281" treenode="633151770643355282" appnode="633151770643355279" handlerfor="633151770643355278">
    <node type="Start" id="633151770643355281" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="53" y="44">
      <linkto id="633153303015976812" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633153303015976810" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="310.629547" y="48">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="literal">Playing TTS  Completed</log>
      </Properties>
    </node>
    <node type="Action" id="633153303015976812" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="183" y="56">
      <linkto id="633153303015976810" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Deleting Connection.</log>
      </Properties>
    </node>
    <node type="Variable" id="633153303015976816" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayTone_Complete" show="false" startnode="633151770643355193" treenode="633151770643355194" appnode="633151770643355191" handlerfor="633151770643355190">
    <node type="Start" id="633151770643355193" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633153303015976790" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633153303015976789" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="319.629547" y="58">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="literal">Playing TTS  Completed</log>
      </Properties>
    </node>
    <node type="Action" id="633153303015976790" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="174.089844" y="49">
      <linkto id="633153303015976789" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="variable">g_time</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Sleeping...</log>
      </Properties>
    </node>
    <node type="Variable" id="633153303015976795" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" show="false" startnode="633151770643355221" treenode="633151770643355222" appnode="633151770643355219" handlerfor="633151770643355218">
    <node type="Start" id="633151770643355221" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633151770643355233" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633151770643355233" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="550" y="107">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnVoiceRecognition_Complete" show="false" startnode="633151770643355276" treenode="633151770643355277" appnode="633151770643355274" handlerfor="633151770643355273">
    <node type="Start" id="633151770643355276" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633153303015976804" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633153303015976803" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="313.629517" y="59">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="literal">Playing TTS  Completed</log>
      </Properties>
    </node>
    <node type="Action" id="633153303015976804" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="182.089844" y="56">
      <linkto id="633153303015976803" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="variable">g_time</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Sleeping...</log>
      </Properties>
    </node>
    <node type="Variable" id="633153303015976809" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" show="false" startnode="633151770643355231" treenode="633151770643355232" appnode="633151770643355229" handlerfor="633151770643355228">
    <node type="Start" id="633151770643355231" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633151770643355241" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633151770643355241" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="503" y="84">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Complete" show="false" startnode="633154218393036878" treenode="633154218393036879" appnode="633154218393036876" handlerfor="633154218393036875">
    <node type="Start" id="633154218393036878" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633154298512133897" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633154218393036888" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="258" y="34">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="literal">Recording conference</log>
      </Properties>
    </node>
    <node type="Action" id="633154298512133897" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="136" y="38">
      <linkto id="633154218393036888" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="variable">g_time</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" show="false" startnode="633151770643355254" treenode="633151770643355255" appnode="633151770643355252" handlerfor="633151770643355251">
    <node type="Start" id="633151770643355254" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633153303015976782" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633151770643355300" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="419.629547" y="45">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="literal">Playing TTS  Completed</log>
      </Properties>
    </node>
    <node type="Action" id="633153303015976782" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="169.089844" y="39">
      <linkto id="633151770643355300" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="variable">g_time</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Sleeping...</log>
      </Properties>
    </node>
    <node type="Variable" id="633153303015976785" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.MediaControl.Play_Complete">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" show="false" startnode="633151770643355226" treenode="633151770643355227" appnode="633151770643355224" handlerfor="633151770643355223">
    <node type="Start" id="633151770643355226" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633151770643355240" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633151770643355240" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="213" y="59">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayTone_Failed" show="false" startnode="633151770643355198" treenode="633151770643355199" appnode="633151770643355196" handlerfor="633151770643355195">
    <node type="Start" id="633151770643355198" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="51">
      <linkto id="633153303015976798" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633153303015976796" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="283.629547" y="69">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="literal">Playing TTS  Completed</log>
      </Properties>
    </node>
    <node type="Action" id="633153303015976798" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="156" y="77">
      <linkto id="633153303015976796" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Deleting Connection.</log>
      </Properties>
    </node>
    <node type="Variable" id="633153303015976802" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" show="false" startnode="633151770643355259" treenode="633151770643355260" appnode="633151770643355257" handlerfor="633151770643355256">
    <node type="Start" id="633151770643355259" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="52" y="39">
      <linkto id="633153303015976786" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633151770643355296" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="474.629547" y="42">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="literal">Playing TTS Failed</log>
      </Properties>
    </node>
    <node type="Action" id="633153303015976786" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="294.076172" y="52">
      <linkto id="633151770643355296" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">Deleting Connection.</log>
      </Properties>
    </node>
    <node type="Variable" id="633153303015976788" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.MediaControl.Play_Failed">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Failed" show="false" startnode="633154218393036883" treenode="633154218393036884" appnode="633154218393036881" handlerfor="633154218393036880">
    <node type="Start" id="633154218393036883" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633154218393036889" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633154218393036889" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="189" y="58">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>