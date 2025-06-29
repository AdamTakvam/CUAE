<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632520303397145547" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632520303397145544" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632520303397145543" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632750758362195657" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632750758362195654" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632750758362195653" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632767400355554338" actid="632762222265292973" />
          <ref id="632767400355554354" actid="632762222265292978" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632750758362195662" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632750758362195659" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632750758362195658" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632767400355554339" actid="632762222265292973" />
          <ref id="632767400355554355" actid="632762222265292978" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632762144451467710" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632762144451467707" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632762144451467706" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632767400355554345" actid="632762144451467716" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632762144451467715" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632762144451467712" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632762144451467711" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632767400355554346" actid="632762144451467716" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_incomingCallId" id="632767400355554320" vid="632750758362195643">
        <Properties type="String">g_incomingCallId</Properties>
      </treenode>
      <treenode text="g_incomingConnId" id="632767400355554322" vid="632750758362195645">
        <Properties type="String">g_incomingConnId</Properties>
      </treenode>
      <treenode text="g_from" id="632767400355554324" vid="632750758362195651">
        <Properties type="String">g_from</Properties>
      </treenode>
      <treenode text="S_HangUp" id="632767400355554326" vid="632750773820039642">
        <Properties type="String" initWith="S_HangUp">S_HangUp</Properties>
      </treenode>
      <treenode text="S_ReceiveCall" id="632767400355554328" vid="632751022297996672">
        <Properties type="String" initWith="S_ReceiveCall">S_ReceiveCall</Properties>
      </treenode>
      <treenode text="g_to" id="632767400355554330" vid="632751022365024621">
        <Properties type="String">g_to</Properties>
      </treenode>
      <treenode text="g_digits" id="632767400355554332" vid="632762144451467721">
        <Properties type="String">g_digits</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632520303397145546" treenode="632520303397145547" appnode="632520303397145544" handlerfor="632520303397145543">
    <node type="Start" id="632520303397145546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="161">
      <linkto id="632751022365024623" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632751016614600571" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="641" y="159">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632751022365024623" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="130.048828" y="160">
      <linkto id="632762222265292971" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">from</ap>
        <rd field="ResultData">g_from</rd>
      </Properties>
    </node>
    <node type="Action" id="632762222265292971" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="255" y="159">
      <linkto id="632762222265292973" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">incomingCallId</ap>
        <rd field="CallId">g_incomingCallId</rd>
        <rd field="ConnectionId">g_incomingConnId</rd>
      </Properties>
    </node>
    <node type="Action" id="632762222265292973" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="324" y="142" mx="377" my="158">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632750758362195657" />
        <item text="OnPlay_Failed" treenode="632750758362195662" />
      </items>
      <linkto id="632751016614600571" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_incomingConnId</ap>
        <ap name="Prompt1" type="literal">enter_pswd.wav</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Variable" id="632750954803710330" name="incomingCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">incomingCallId</Properties>
    </node>
    <node type="Variable" id="632750954803710331" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">from</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632750758362195656" treenode="632750758362195657" appnode="632750758362195654" handlerfor="632750758362195653">
    <node type="Start" id="632750758362195656" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="52" y="207">
      <linkto id="632762144451467716" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632750758362195686" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="556" y="209">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632762144451467716" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="176" y="199" mx="250" my="215">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632762144451467710" />
        <item text="OnGatherDigits_Failed" treenode="632762144451467715" />
      </items>
      <linkto id="632750758362195686" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnId</ap>
        <ap name="TermCondMaxTime" type="literal">30000</ap>
        <ap name="UserData" type="literal">none</ap>
        <log condition="exit" on="false" level="Info" type="csharp">"DEBUG: digits received: "</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632750758362195661" treenode="632750758362195662" appnode="632750758362195659" handlerfor="632750758362195658">
    <node type="Start" id="632750758362195661" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="74" y="246">
      <linkto id="632750758362195687" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632750758362195687" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="219" y="247">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" activetab="true" startnode="632762144451467709" treenode="632762144451467710" appnode="632762144451467707" handlerfor="632762144451467706">
    <node type="Start" id="632762144451467709" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="196">
      <linkto id="632762144451467723" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632762144451467723" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="123" y="196">
      <linkto id="632762222265292982" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">password</ap>
        <rd field="ResultData">g_digits</rd>
        <log condition="exit" on="true" level="Info" type="csharp">"DEBUG: digits recieved: " + g_digits</log>
      </Properties>
    </node>
    <node type="Action" id="632762144451467726" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="496" y="157">
      <linkto id="632762144451467727" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632762144451467727" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="609" y="157">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632762222265292978" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="231" y="239" mx="284" my="255">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632750758362195657" />
        <item text="OnPlay_Failed" treenode="632750758362195662" />
      </items>
      <linkto id="632762222265292981" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_digits</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnId</ap>
        <ap name="Prompt1" type="literal">digits received</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632762222265292981" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="367" y="156">
      <linkto id="632762144451467726" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">5000</ap>
      </Properties>
    </node>
    <node type="Action" id="632762222265292982" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="124" y="313">
      <linkto id="632762222265292978" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(ref string g_digits)
{
	string tempPassword = g_digits;
	g_digits = string.Empty;
	foreach (char c in tempPassword)
	{
		g_digits += c;
		g_digits += ",";
	}

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Comment" id="632762222265292983" text="Insert a pause between digits entered" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="39" y="351" />
    <node type="Comment" id="632762222265292984" text="Use the Text To Speech application&#xD;&#xA;to tell the user what was entered." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="310" y="239" />
    <node type="Variable" id="632762144451467719" name="password" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" defaultInitWith="NONE" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">password</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632762144451467714" treenode="632762144451467715" appnode="632762144451467712" handlerfor="632762144451467711">
    <node type="Start" id="632762144451467714" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="163">
      <linkto id="632762144451467728" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632762144451467728" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="181" y="170">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>