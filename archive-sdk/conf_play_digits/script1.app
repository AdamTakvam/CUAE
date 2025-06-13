<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632763944396562855" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632763944396562852" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632763944396562851" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632763944396562870" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632763944396562867" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632763944396562866" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632766581140156632" actid="632763944396562876" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632763944396562875" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632763944396562872" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632763944396562871" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632766581140156633" actid="632763944396562876" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632763944396562883" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632763944396562880" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632763944396562879" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632766581140156635" actid="632763944396562889" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632763944396562888" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632763944396562885" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632763944396562884" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632766581140156636" actid="632763944396562889" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="632766581140156622" vid="632763944396562858">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632766581140156624" vid="632763944396562860">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_confId" id="632766581140156626" vid="632763944396562862">
        <Properties type="String">g_confId</Properties>
      </treenode>
      <treenode text="g_mmsId" id="632766581140156628" vid="632763944396562864">
        <Properties type="String">g_mmsId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632763944396562854" treenode="632763944396562855" appnode="632763944396562852" handlerfor="632763944396562851">
    <node type="Start" id="632763944396562854" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="82">
      <linkto id="632763944396562856" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632763944396562856" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="181" y="82">
      <linkto id="632763944396562876" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="WaitForMedia" type="csharp">true</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="Hairpin" type="literal">true</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="MmsId">g_mmsId</rd>
        <rd field="ConnectionId">g_connectionId</rd>
        <rd field="ConferenceId">g_confId</rd>
      </Properties>
    </node>
    <node type="Action" id="632763944396562876" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="291" y="66" mx="344" my="82">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632763944396562870" />
        <item text="OnPlay_Failed" treenode="632763944396562875" />
      </items>
      <linkto id="632763944396562889" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sample_music.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632763944396562889" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="450" y="65" mx="524" my="81">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632763944396562883" />
        <item text="OnGatherDigits_Failed" treenode="632763944396562888" />
      </items>
      <linkto id="632763944396562892" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632763944396562892" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="719" y="82">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632763944396562857" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632763944396562869" treenode="632763944396562870" appnode="632763944396562867" handlerfor="632763944396562866">
    <node type="Start" id="632763944396562869" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632763944396562893" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632763944396562893" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="154" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632763944396562874" treenode="632763944396562875" appnode="632763944396562872" handlerfor="632763944396562871">
    <node type="Start" id="632763944396562874" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="46" y="67">
      <linkto id="632763944396562896" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632763944396562896" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="181" y="67">
      <linkto id="632763944396562897" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632763944396562897" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="331" y="67">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" startnode="632763944396562882" treenode="632763944396562883" appnode="632763944396562880" handlerfor="632763944396562879">
    <node type="Start" id="632763944396562882" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="60" y="72">
      <linkto id="632763944396562900" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632763944396562900" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="177" y="72">
      <linkto id="632763944396562902" type="Labeled" style="Bezier" ortho="true" label="default" />
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
    <node type="Action" id="632763944396562901" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="455" y="72">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632763944396562902" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="328" y="72">
      <linkto id="632763944396562901" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Variable" id="632763944396562903" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">digits</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632763944396562887" treenode="632763944396562888" appnode="632763944396562885" handlerfor="632763944396562884">
    <node type="Start" id="632763944396562887" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="49" y="77">
      <linkto id="632763944396562898" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632763944396562898" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="163" y="77">
      <linkto id="632763944396562899" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632763944396562899" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="298" y="77">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>