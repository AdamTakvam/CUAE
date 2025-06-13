<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632533330281727298" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632533330281727295" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632533330281727294" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632533330281727306" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632533330281727303" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632533330281727302" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632533354758304440" actid="632533330281727384" />
          <ref id="632533354758304443" actid="632533330281727385" />
          <ref id="632533354758304446" actid="632533330281727386" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632533330281727311" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632533330281727308" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632533330281727307" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632533354758304441" actid="632533330281727384" />
          <ref id="632533354758304444" actid="632533330281727385" />
          <ref id="632533354758304447" actid="632533330281727386" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632533330281727377" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632533330281727374" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632533330281727373" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632533330281727383" level="1" text="PlayText">
        <node type="function" name="PlayText" id="632533330281727380" path="Metreos.StockTools" />
        <calls>
          <ref actid="632533330281727379" />
          <ref actid="632533330281727423" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_prompt1" id="632533354758304409" vid="632533330281727315">
        <Properties type="String" defaultInitWith="Empty" initWith="Prompt1">g_prompt1</Properties>
      </treenode>
      <treenode text="g_prompt2" id="632533354758304411" vid="632533330281727317">
        <Properties type="String" defaultInitWith="Empty" initWith="Prompt2">g_prompt2</Properties>
      </treenode>
      <treenode text="g_prompt3" id="632533354758304413" vid="632533330281727319">
        <Properties type="String" defaultInitWith="Empty" initWith="Prompt3">g_prompt3</Properties>
      </treenode>
      <treenode text="g_callId" id="632533354758304415" vid="632533330281727371">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_playList" id="632533354758304417" vid="632533330281727415">
        <Properties type="ArrayList">g_playList</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632533354758304419" vid="632533330281727429">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_loopPause" id="632533354758304421" vid="632533354758304384">
        <Properties type="Int" initWith="PauseTime">g_loopPause</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632533330281727297" treenode="632533330281727298" appnode="632533330281727295" handlerfor="632533330281727294">
    <node type="Start" id="632533330281727297" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="39" y="266">
      <linkto id="632533330281727300" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632533330281727300" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="132" y="266">
      <linkto id="632533330281727351" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632533330281727378" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="ConnectionId">g_connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632533330281727351" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="246" y="266">
      <linkto id="632533330281727379" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string g_prompt1, string g_prompt2, string g_prompt3, ref ArrayList g_playList)
{
	g_playList = new ArrayList();

	if (g_prompt1 != null &amp;&amp; g_prompt1 != string.Empty)
		g_playList.Add(g_prompt1);

	if (g_prompt2 != null &amp;&amp; g_prompt2 != string.Empty)
		g_playList.Add(g_prompt2);

	if (g_prompt3 != null &amp;&amp; g_prompt3 != string.Empty)
		g_playList.Add(g_prompt3);

	return IApp.VALUE_SUCCESS;	
}
</Properties>
    </node>
    <node type="Action" id="632533330281727378" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="133" y="376">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="literal">Failed to answer call. Exiting script.</log>
      </Properties>
    </node>
    <node type="Action" id="632533330281727379" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="334.825867" y="252" mx="372" my="268">
      <items count="1">
        <item text="PlayText" />
      </items>
      <linkto id="632533330281727409" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="abc" type="literal">abc</ap>
        <ap name="FunctionName" type="literal">PlayText</ap>
      </Properties>
    </node>
    <node type="Action" id="632533330281727409" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="491" y="268">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632533330281727299" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" activetab="true" startnode="632533330281727305" treenode="632533330281727306" appnode="632533330281727303" handlerfor="632533330281727302">
    <node type="Start" id="632533330281727305" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="291">
      <linkto id="632533330281727422" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632533330281727422" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="135" y="291">
      <linkto id="632533354758304458" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"Play action completed. Sleeping for " + g_loopPause + "ms and invoking PlayText function"</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">TTS-Test</ap>
      </Properties>
    </node>
    <node type="Action" id="632533330281727423" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="328.000031" y="274" mx="365" my="290">
      <items count="1">
        <item text="PlayText" />
      </items>
      <linkto id="632533330281727425" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="abc" type="literal">abc</ap>
        <ap name="FunctionName" type="literal">PlayText</ap>
      </Properties>
    </node>
    <node type="Action" id="632533330281727425" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="486" y="291">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632533354758304458" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="244" y="290">
      <linkto id="632533330281727423" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="variable">g_loopPause</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632533330281727310" treenode="632533330281727311" appnode="632533330281727308" handlerfor="632533330281727307">
    <node type="Start" id="632533330281727310" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="338">
      <linkto id="632533330281727426" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632533330281727426" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="146" y="337">
      <linkto id="632533330281727427" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
        <log condition="entry" on="true" level="Error" type="literal">Play command failed (final response). Hanging up call and exiting.</log>
      </Properties>
    </node>
    <node type="Action" id="632533330281727427" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="242" y="337">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632533330281727376" treenode="632533330281727377" appnode="632533330281727374" handlerfor="632533330281727373">
    <node type="Start" id="632533330281727376" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="331">
      <linkto id="632533330281727428" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632533330281727428" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="136" y="332">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">RemoteHangup, ending script.</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="PlayText" startnode="632533330281727382" treenode="632533330281727383" appnode="632533330281727380" handlerfor="632533330281727373">
    <node type="Start" id="632533330281727382" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="75" y="266">
      <linkto id="632533330281727417" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632533330281727384" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="438.132324" y="72" mx="491" my="88">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632533330281727306" />
        <item text="OnPlay_Failed" treenode="632533330281727311" />
      </items>
      <linkto id="632533330281727387" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632533330281727420" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="variable">p1</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632533330281727385" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="437.132324" y="252" mx="490" my="268">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632533330281727306" />
        <item text="OnPlay_Failed" treenode="632533330281727311" />
      </items>
      <linkto id="632533330281727388" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632533330281727420" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="variable">p1</ap>
        <ap name="Prompt2" type="variable">p2</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632533330281727386" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="441.132324" y="427" mx="494" my="443">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632533330281727306" />
        <item text="OnPlay_Failed" treenode="632533330281727311" />
      </items>
      <linkto id="632533330281727389" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632533330281727420" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="variable">g_prompt1</ap>
        <ap name="Prompt2" type="variable">g_prompt2</ap>
        <ap name="Prompt3" type="variable">g_prompt3</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Label" id="632533330281727387" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="488.132324" y="215" />
    <node type="Label" id="632533330281727388" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="487.132324" y="390" />
    <node type="Label" id="632533330281727389" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="493.132324" y="571" />
    <node type="Label" id="632533330281727402" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="138" y="621">
      <linkto id="632533330281727404" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632533330281727403" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="326" y="622">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632533330281727404" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="218" y="621">
      <linkto id="632533330281727403" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
        <log condition="entry" on="true" level="Error" type="literal">Play command failed provisionally. Hanging up call, exiting script.</log>
      </Properties>
    </node>
    <node type="Action" id="632533330281727417" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="233" y="267">
      <linkto id="632533330281727384" type="Labeled" style="Bezier" ortho="true" label="1" />
      <linkto id="632533330281727385" type="Labeled" style="Bezier" ortho="true" label="2" />
      <linkto id="632533330281727386" type="Labeled" style="Bezier" ortho="true" label="3" />
      <Properties language="csharp">
public static string Execute(ArrayList g_playList, ref string p1, ref string p2)
{
	switch (g_playList.Count)
	{
		case 0 : p1 = "No text specified."; break;
		case 1 : p1 = g_playList[0] as string; break;
		case 2 : p1 = g_playList[0] as string; p2 = g_playList[1] as string; break;
		default : break;
	}

	return Convert.ToString(g_playList.Count);
}
</Properties>
    </node>
    <node type="Action" id="632533330281727420" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="761" y="270">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">success</ap>
      </Properties>
    </node>
    <node type="Variable" id="632533330281727418" name="p1" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" defaultInitWith="Empty" refType="reference">p1</Properties>
    </node>
    <node type="Variable" id="632533330281727419" name="p2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" defaultInitWith="Empty" refType="reference">p2</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>