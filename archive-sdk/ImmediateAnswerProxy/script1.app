<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632488437416819486" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632488437416819483" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632488437416819482" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632488437416819491" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632488437416819488" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632488437416819487" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632586748868584129" actid="632488437416819502" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632488437416819496" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632488437416819493" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632488437416819492" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632586748868584130" actid="632488437416819502" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632488437416819501" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632488437416819498" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632488437416819497" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632586748868584131" actid="632488437416819502" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callIdOutgoing" id="632586748868584122" vid="632488437416819506">
        <Properties type="String">g_callIdOutgoing</Properties>
      </treenode>
      <treenode text="g_callIdIncoming" id="632586748868584124" vid="632488437416819508">
        <Properties type="String">g_callIdIncoming</Properties>
      </treenode>
      <treenode text="g_ripOffCount" id="632586748868584126" vid="632586745119605971">
        <Properties type="Int" initWith="RipOffDigits">g_ripOffCount</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632488437416819485" treenode="632488437416819486" appnode="632488437416819483" handlerfor="632488437416819482">
    <node type="Start" id="632488437416819485" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="347">
      <linkto id="632586745119605921" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632488437416819502" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="450" y="330" mx="516" my="346">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632488437416819491" />
        <item text="OnMakeCall_Failed" treenode="632488437416819496" />
        <item text="OnRemoteHangup" treenode="632488437416819501" />
      </items>
      <linkto id="632488437416819523" type="Labeled" style="Bevel" label="default" />
      <linkto id="632488464955180950" type="Labeled" style="Bevel" label="Failure" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">to</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">confId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_callIdOutgoing</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"Calling to: " + to</log>
      </Properties>
    </node>
    <node type="Action" id="632488437416819510" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="319" y="347">
      <linkto id="632488437416819502" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">callId</ap>
        <rd field="ResultData">g_callIdIncoming</rd>
      </Properties>
    </node>
    <node type="Action" id="632488437416819523" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="684" y="350">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632488437416819524" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="532" y="554">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">Provisional MakeCall Failure</log>
      </Properties>
    </node>
    <node type="Action" id="632488464955180950" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="216" y="569">
      <linkto id="632488437416819524" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632488899860440174" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="211" y="348">
      <linkto id="632488437416819510" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="literal">0</ap>
        <rd field="ConferenceId">confId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">String.Format("!IncomingCall:  From '{0}'  To '{1}'", from, to)</log>
      </Properties>
    </node>
    <node type="Action" id="632586745119605921" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="99" y="350">
      <linkto id="632488899860440174" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
public static string Execute(int g_ripOffCount, ref string to)
{
	to = to.Substring(g_ripOffCount);
	return "";
}
</Properties>
    </node>
    <node type="Variable" id="632488437416819511" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632488437416819525" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.CallControl.IncomingCall">to</Properties>
    </node>
    <node type="Variable" id="632488899860440175" name="confId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">confId</Properties>
    </node>
    <node type="Variable" id="632496740545442504" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">from</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632488437416819490" treenode="632488437416819491" appnode="632488437416819488" handlerfor="632488437416819487">
    <node type="Start" id="632488437416819490" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="345">
      <linkto id="632488437416819515" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632488437416819515" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="553" y="340">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632488437416819495" treenode="632488437416819496" appnode="632488437416819493" handlerfor="632488437416819492">
    <node type="Start" id="632488437416819495" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="76" y="245">
      <linkto id="632488437416819516" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632488437416819516" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="327" y="246">
      <linkto id="632488437416819517" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callIdIncoming</ap>
        <log condition="entry" on="true" level="Info" type="literal">Async MakeCall Failure</log>
      </Properties>
    </node>
    <node type="Action" id="632488437416819517" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="485" y="245">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632488437416819500" treenode="632488437416819501" appnode="632488437416819498" handlerfor="632488437416819497">
    <node type="Start" id="632488437416819500" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="330">
      <linkto id="632488437416819519" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632488437416819519" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="193" y="319">
      <linkto id="632488437416819520" type="Labeled" style="Bevel" label="true" />
      <linkto id="632488437416819521" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">callId == g_callIdOutgoing</ap>
      </Properties>
    </node>
    <node type="Action" id="632488437416819520" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="362" y="290">
      <linkto id="632488437416819522" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callIdIncoming</ap>
      </Properties>
    </node>
    <node type="Action" id="632488437416819521" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="352" y="392">
      <linkto id="632488437416819522" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callIdOutgoing</ap>
      </Properties>
    </node>
    <node type="Action" id="632488437416819522" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="509" y="350">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632488437416819518" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">callId</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>