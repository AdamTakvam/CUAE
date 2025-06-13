<Application name="DialIn" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="DialIn">
    <outline>
      <treenode type="evh" id="632934875050470938" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632934875050470935" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632934875050470934" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632934875050470955" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632934875050470952" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632934875050470951" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632934875050471364" actid="632934875050470961" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632934875050470960" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632934875050470957" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632934875050470956" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632934875050471365" actid="632934875050470961" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632934875050471017" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632934875050471014" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632934875050471013" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632934875050471370" actid="632934875050471028" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632934875050471022" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632934875050471019" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632934875050471018" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632934875050471371" actid="632934875050471028" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632934875050471027" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632934875050471024" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632934875050471023" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632934875050471372" actid="632934875050471028" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632934875050471107" level="2" text="Metreos.MediaControl.Play_Complete: OnPlayMPCallFailure_Complete">
        <node type="function" name="OnPlayMPCallFailure_Complete" id="632934875050471102" path="Metreos.StockTools" />
        <node type="event" name="OnPlayMPCallFailure_Complete" id="632934875050471105" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632934875050471374" actid="632934875050471098" />
          <ref id="632934875050471393" actid="632934875050471136" />
        </references>
        <Properties type="asyncCallback">
          <ep name="UserData" type="literal">Ending</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632934875050471113" level="2" text="Metreos.MediaControl.Play_Failed: OnPlayMPCallFailure_Failed">
        <node type="function" name="OnPlayMPCallFailure_Failed" id="632934875050471108" path="Metreos.StockTools" />
        <node type="event" name="OnPlayMPCallFailure_Failed" id="632934875050471111" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632934875050471375" actid="632934875050471098" />
          <ref id="632934875050471394" actid="632934875050471136" />
        </references>
        <Properties type="asyncCallback">
          <ep name="UserData" type="literal">Ending</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632934875050471156" level="2" text="Metreos.MediaControl.Play_Complete: OnPlayEnding_Complete">
        <node type="function" name="OnPlayEnding_Complete" id="632934875050471151" path="Metreos.StockTools" />
        <node type="event" name="OnPlayEnding_Complete" id="632934875050471154" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632934875050471402" actid="632934875050471148" />
        </references>
        <Properties type="asyncCallback">
          <ep name="UserData" type="literal">Hangup</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632934875050471162" level="2" text="Metreos.MediaControl.Play_Failed: OnPlayEnding_Failed">
        <node type="function" name="OnPlayEnding_Failed" id="632934875050471157" path="Metreos.StockTools" />
        <node type="event" name="OnPlayEnding_Failed" id="632934875050471160" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632934875050471403" actid="632934875050471148" />
        </references>
        <Properties type="asyncCallback">
          <ep name="UserData" type="literal">Hangup</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_confId" id="632934875050471342" vid="632934875050470941">
        <Properties type="String">g_confId</Properties>
      </treenode>
      <treenode text="g_incomingCallId" id="632934875050471344" vid="632934875050470943">
        <Properties type="String">g_incomingCallId</Properties>
      </treenode>
      <treenode text="g_incomingConnId" id="632934875050471346" vid="632934875050470945">
        <Properties type="String">g_incomingConnId</Properties>
      </treenode>
      <treenode text="g_outgoingCallId" id="632934875050471348" vid="632934875050470947">
        <Properties type="String">g_outgoingCallId</Properties>
      </treenode>
      <treenode text="g_outgoingConnId" id="632934875050471350" vid="632934875050470949">
        <Properties type="String">g_outgoingConnId</Properties>
      </treenode>
      <treenode text="g_digits" id="632934875050471352" vid="632934875050471001">
        <Properties type="String" initWith="Digits">g_digits</Properties>
      </treenode>
      <treenode text="g_mpAccount" id="632934875050471354" vid="632934875050471003">
        <Properties type="String" initWith="MPAccount">g_mpAccount</Properties>
      </treenode>
      <treenode text="g_mpPin" id="632934875050471356" vid="632934875050471005">
        <Properties type="String" initWith="MPPin">g_mpPin</Properties>
      </treenode>
      <treenode text="g_commaDelay" id="632934875050471358" vid="632934875050471007">
        <Properties type="String" initWith="CommaDelay">g_commaDelay</Properties>
      </treenode>
      <treenode text="g_mpDialin" id="632934875050471360" vid="632934875050471096">
        <Properties type="String" initWith="MPDialIn">g_mpDialin</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632934875050470937" treenode="632934875050470938" appnode="632934875050470935" handlerfor="632934875050470934">
    <node type="Start" id="632934875050470937" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="96" y="464">
      <linkto id="632934875050470940" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632934875050470940" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="304" y="464">
      <linkto id="632934875050470961" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DisplayName" type="literal">MeetingPlace</ap>
        <ap name="Conference" type="literal">true</ap>
        <rd field="CallId">g_incomingCallId</rd>
        <rd field="ConnectionId">g_incomingConnId</rd>
        <rd field="ConferenceId">g_confId</rd>
      </Properties>
    </node>
    <node type="Action" id="632934875050470961" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="464" y="448" mx="517" my="464">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632934875050470955" />
        <item text="OnPlay_Failed" treenode="632934875050470960" />
      </items>
      <linkto id="632934875050470964" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">Beginning MeetingPlace DTMF automation </ap>
        <ap name="ConnectionId" type="variable">g_incomingConnId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632934875050470964" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="688" y="464">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632934875050471419" text="Tips:&#xD;&#xA;&#xD;&#xA;Use meetingplaceinternal.cisco.com to schedule meetings (not meetingsint.cisco.com, because it will not allow dial in from our lab gateways).&#xD;&#xA;&#xD;&#xA;Make sure students use the 'A' and 'P' symbol and create password installers for account code / pin, to promote secure coding.&#xD;&#xA;&#xD;&#xA;This particular application can't be run concurrently because it's keyed to one user.&#xD;&#xA;&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="184" />
    <node type="Variable" id="632934875050470939" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callId" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632934875050470954" treenode="632934875050470955" appnode="632934875050470952" handlerfor="632934875050470951">
    <node type="Start" id="632934875050470954" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="96" y="472">
      <linkto id="632934875050471028" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632934875050471028" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="176" y="456" mx="242" my="472">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632934875050471017" />
        <item text="OnMakeCall_Failed" treenode="632934875050471022" />
        <item text="OnRemoteHangup" treenode="632934875050471027" />
      </items>
      <linkto id="632934875050471098" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632934875050471123" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_mpDialin</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_confId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_outgoingCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632934875050471098" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="154.1836" y="688" mx="245" my="704">
      <items count="2">
        <item text="OnPlayMPCallFailure_Complete" treenode="632934875050471107" />
        <item text="OnPlayMPCallFailure_Failed" treenode="632934875050471113" />
      </items>
      <linkto id="632934875050471101" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">Call failed to MeetingPlace</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnId</ap>
        <ap name="UserData" type="literal">Ending</ap>
      </Properties>
    </node>
    <node type="Action" id="632934875050471101" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="240" y="864">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632934875050471123" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="512" y="472">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632934875050470959" treenode="632934875050470960" appnode="632934875050470957" handlerfor="632934875050470956">
    <node type="Start" id="632934875050470959" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="240" y="368">
      <linkto id="632934875050470965" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632934875050470965" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="408" y="368">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632934875050471016" treenode="632934875050471017" appnode="632934875050471014" handlerfor="632934875050471013">
    <node type="Loop" id="632934875050471124" name="Loop" text="loop (var)" cx="800" cy="600" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="368" y="112" mx="768" my="412">
      <linkto id="632934875050471125" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632934875050471135" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="variable">g_digits</Properties>
    </node>
    <node type="Loop" id="632934875050471173" name="Loop" text="loop (var)" cx="117.882813" cy="113" entry="1" container="632934875050471124" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="728" y="232" mx="787" my="288">
      <linkto id="632934875050471130" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632934875050471124" port="3" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="variable">g_mpAccount</Properties>
    </node>
    <node type="Loop" id="632934875050471174" name="Loop" text="loop (var)" cx="118" cy="116" entry="1" container="632934875050471124" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="720" y="424" mx="779" my="482">
      <linkto id="632934875050471175" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632934875050471124" port="3" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="variable">g_mpPin</Properties>
    </node>
    <node type="Start" id="632934875050471016" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="80" y="416">
      <linkto id="632934875050471129" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632934875050471125" name="Switch" container="632934875050471124" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="496" y="360">
      <linkto id="632934875050471127" type="Labeled" style="Bezier" ortho="true" label="," />
      <linkto id="632934875050471133" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632934875050471173" port="1" type="Labeled" style="Bezier" ortho="true" label="A" />
      <linkto id="632934875050471174" port="1" type="Labeled" style="Bezier" ortho="true" label="P" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">loopEnum.Current.ToString()</ap>
      </Properties>
    </node>
    <node type="Action" id="632934875050471127" name="Sleep" container="632934875050471124" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="784" y="152">
      <linkto id="632934875050471124" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="variable">g_commaDelay</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Delaying for " + g_commaDelay + "ms"</log>
      </Properties>
    </node>
    <node type="Action" id="632934875050471129" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="184" y="416">
      <linkto id="632934875050471124" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">connId</ap>
        <rd field="ResultData">g_outgoingConnId</rd>
      </Properties>
    </node>
    <node type="Action" id="632934875050471130" name="SendUserInput" container="632934875050471173" class="MaxActionNode" group="" path="Metreos.CallControl" x="792" y="288">
      <linkto id="632934875050471173" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_outgoingCallId</ap>
        <ap name="Digits" type="csharp">loopEnum.Current.ToString()</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Sending account code " + loopEnum.Current.ToString()</log>
      </Properties>
    </node>
    <node type="Action" id="632934875050471133" name="SendUserInput" container="632934875050471124" class="MaxActionNode" group="" path="Metreos.CallControl" x="784" y="648">
      <linkto id="632934875050471124" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_outgoingCallId</ap>
        <ap name="Digits" type="csharp">loopEnum.Current.ToString()</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Sending DTMF: " + loopEnum.Current.ToString()</log>
      </Properties>
    </node>
    <node type="Action" id="632934875050471135" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1296" y="424">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632934875050471171" text="Hard to see, but that's a comma..." container="632934875050471124" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="408" y="208" />
    <node type="Comment" id="632934875050471172" text="loopEnum.Current is the magical keyword for the current character in the digit string, when looping" container="632934875050471124" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="424" y="120" />
    <node type="Action" id="632934875050471175" name="SendUserInput" container="632934875050471174" class="MaxActionNode" group="" path="Metreos.CallControl" x="784" y="480">
      <linkto id="632934875050471174" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_outgoingCallId</ap>
        <ap name="Digits" type="csharp">loopEnum.Current.ToString()</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Sending account pin " + loopEnum.Current.ToString()</log>
      </Properties>
    </node>
    <node type="Variable" id="632934875050471128" name="connId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632934875050471021" treenode="632934875050471022" appnode="632934875050471019" handlerfor="632934875050471018">
    <node type="Start" id="632934875050471021" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="72" y="424">
      <linkto id="632934875050471136" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632934875050471136" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="200" y="408" mx="291" my="424">
      <items count="2">
        <item text="OnPlayMPCallFailure_Complete" treenode="632934875050471107" />
        <item text="OnPlayMPCallFailure_Failed" treenode="632934875050471113" />
      </items>
      <linkto id="632934875050471140" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">Call failed to MeetingPlace</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnId</ap>
        <ap name="UserData" type="literal">Ending</ap>
      </Properties>
    </node>
    <node type="Action" id="632934875050471140" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="488" y="424">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632934875050471026" treenode="632934875050471027" appnode="632934875050471024" handlerfor="632934875050471023">
    <node type="Start" id="632934875050471026" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="72" y="448">
      <linkto id="632934875050471142" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632934875050471142" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="240" y="448">
      <linkto id="632934875050471144" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632934875050471148" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632934875050471144" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="432" y="352">
      <linkto id="632934875050471164" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_outgoingCallId</ap>
      </Properties>
    </node>
    <node type="Comment" id="632934875050471145" text="If user hangs up, just immediately hang up on MP" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="240" y="256" />
    <node type="Comment" id="632934875050471146" text="If MP hangs up, inform user, then hangup" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="336" y="664" />
    <node type="Action" id="632934875050471148" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="357.282227" y="536" mx="429" my="552">
      <items count="2">
        <item text="OnPlayEnding_Complete" treenode="632934875050471156" />
        <item text="OnPlayEnding_Failed" treenode="632934875050471162" />
      </items>
      <linkto id="632934875050471163" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">MeetingPlace has hung up the call.</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnId</ap>
        <ap name="UserData" type="literal">Hangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632934875050471163" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="608" y="552">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632934875050471164" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="608" y="352">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632934875050471143" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayMPCallFailure_Complete" startnode="632934875050471104" treenode="632934875050471107" appnode="632934875050471102" handlerfor="632934875050471105">
    <node type="Start" id="632934875050471104" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="88" y="432">
      <linkto id="632934875050471114" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632934875050471114" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="328" y="432">
      <linkto id="632934875050471116" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632934875050471116" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="504" y="432">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayMPCallFailure_Failed" startnode="632934875050471110" treenode="632934875050471113" appnode="632934875050471108" handlerfor="632934875050471111">
    <node type="Start" id="632934875050471110" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="88" y="384">
      <linkto id="632934875050471117" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632934875050471117" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="312" y="384">
      <linkto id="632934875050471118" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632934875050471118" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="520" y="376">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayEnding_Complete" startnode="632934875050471153" treenode="632934875050471156" appnode="632934875050471151" handlerfor="632934875050471154">
    <node type="Start" id="632934875050471153" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="224" y="440">
      <linkto id="632934875050471165" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632934875050471165" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="368" y="440">
      <linkto id="632934875050471166" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632934875050471166" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="552" y="440">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayEnding_Failed" startnode="632934875050471159" treenode="632934875050471162" appnode="632934875050471157" handlerfor="632934875050471160">
    <node type="Start" id="632934875050471159" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="200" y="424">
      <linkto id="632934875050471167" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632934875050471167" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="360" y="424">
      <linkto id="632934875050471168" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632934875050471168" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="512" y="424">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>