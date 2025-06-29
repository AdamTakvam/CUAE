<Application name="NewErrors" trigger="Metreos.Providers.DatabaseScraper.NewErrors" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="NewErrors">
    <outline>
      <treenode type="evh" id="632588433343350435" level="1" text="Metreos.Providers.DatabaseScraper.NewErrors (trigger): OnNewErrors">
        <node type="function" name="OnNewErrors" id="632588433343350432" path="Metreos.StockTools" />
        <node type="event" name="NewErrors" id="632588433343350431" path="Metreos.Providers.DatabaseScraper.NewErrors" trigger="true" />
        <Properties type="Triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632588433343350453" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632588433343350450" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632588433343350449" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632590322813683447" actid="632588433343350464" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632588433343350458" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632588433343350455" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632588433343350454" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632590322813683448" actid="632588433343350464" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632588433343350463" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632588433343350460" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632588433343350459" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632590322813683449" actid="632588433343350464" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632588433343350482" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632588433343350479" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632588433343350478" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632590322813683457" actid="632588433343350488" />
          <ref id="632590322813683460" actid="632588433343350495" />
          <ref id="632590322813683472" actid="632588433343350504" />
          <ref id="632590322813683482" actid="632588433343350521" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632588433343350487" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632588433343350484" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632588433343350483" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632590322813683458" actid="632588433343350488" />
          <ref id="632590322813683461" actid="632588433343350495" />
          <ref id="632590322813683473" actid="632588433343350504" />
          <ref id="632590322813683483" actid="632588433343350521" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_adminCallId" id="632590322813683435" vid="632588433343350468">
        <Properties type="String">g_adminCallId</Properties>
      </treenode>
      <treenode text="g_adminDn" id="632590322813683437" vid="632588433343350470">
        <Properties type="String" initWith="adminNumber">g_adminDn</Properties>
      </treenode>
      <treenode text="g_adminConnectionId" id="632590322813683439" vid="632588433343350474">
        <Properties type="String">g_adminConnectionId</Properties>
      </treenode>
      <treenode text="g_errors" id="632590322813683441" vid="632588433343350492">
        <Properties type="Metreos.Types.DatabaseScraper.ErrorDataCollection">g_errors</Properties>
      </treenode>
      <treenode text="g_errorCount" id="632590322813683443" vid="632588433343350511">
        <Properties type="Int" defaultInitWith="0">g_errorCount</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnNewErrors" startnode="632588433343350434" treenode="632588433343350435" appnode="632588433343350432" handlerfor="632588433343350431">
    <node type="Start" id="632588433343350434" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="288">
      <linkto id="632588433343350494" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632588433343350448" text="Call the administrator to tell him of new errors" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="164" y="186" />
    <node type="Action" id="632588433343350464" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="191" y="274" mx="257" my="290">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632588433343350453" />
        <item text="OnMakeCall_Failed" treenode="632588433343350458" />
        <item text="OnRemoteHangup" treenode="632588433343350463" />
      </items>
      <linkto id="632588433343350472" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632588433343350473" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_adminDn</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_adminCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632588433343350472" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="250" y="471">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">Unable to place a call to the administrator</log>
      </Properties>
    </node>
    <node type="Action" id="632588433343350473" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="481" y="292">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632588433343350494" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="128" y="288">
      <linkto id="632588433343350464" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">errorsData</ap>
        <rd field="ResultData">g_errors</rd>
      </Properties>
    </node>
    <node type="Variable" id="632588433343350436" name="errorsData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.DatabaseScraper.ErrorDataCollection" initWith="Data" refType="reference">errorsData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632588433343350452" treenode="632588433343350453" appnode="632588433343350450" handlerfor="632588433343350449">
    <node type="Start" id="632588433343350452" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="280">
      <linkto id="632588433343350476" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632588433343350476" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="173" y="280">
      <linkto id="632588433343350499" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">connectionId</ap>
        <rd field="ResultData">g_adminConnectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632588433343350488" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="400" y="328" mx="453" my="344">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632588433343350482" />
        <item text="OnPlay_Failed" treenode="632588433343350487" />
      </items>
      <linkto id="632588433343350500" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="csharp">"There is one new error"</ap>
        <ap name="ConnectionId" type="variable">g_adminConnectionId</ap>
        <ap name="UserData" type="literal">listerrors</ap>
      </Properties>
    </node>
    <node type="Action" id="632588433343350495" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="400" y="184" mx="453" my="200">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632588433343350482" />
        <item text="OnPlay_Failed" treenode="632588433343350487" />
      </items>
      <linkto id="632588433343350500" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="csharp">"There are " + g_errors.Collection.Count + " new errors"</ap>
        <ap name="ConnectionId" type="variable">g_adminConnectionId</ap>
        <ap name="UserData" type="literal">listerrors</ap>
      </Properties>
    </node>
    <node type="Action" id="632588433343350499" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="280" y="280">
      <linkto id="632588433343350488" type="Labeled" style="Bezier" ortho="true" label="1" />
      <linkto id="632588433343350495" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">g_errors.Collection.Count</ap>
      </Properties>
    </node>
    <node type="Action" id="632588433343350500" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="635" y="285">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632590322813683492" text="&quot;There are &quot; + g_errors.Collection.Count + &quot; new errors&quot;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="310" y="139" />
    <node type="Comment" id="632590322813683493" text="There is one new error" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="395" y="431" />
    <node type="Comment" id="632590322813683494" text="Switch on the number of errors" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="200" y="328" />
    <node type="Variable" id="632588433343350477" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632588433343350457" treenode="632588433343350458" appnode="632588433343350455" handlerfor="632588433343350454">
    <node type="Start" id="632588433343350457" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="296">
      <linkto id="632588433343350501" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632588433343350501" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="368" y="288">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">The call failed</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632588433343350462" treenode="632588433343350463" appnode="632588433343350460" handlerfor="632588433343350459">
    <node type="Start" id="632588433343350462" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="288">
      <linkto id="632588433343350532" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632588433343350532" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="384" y="296">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" activetab="true" startnode="632588433343350481" treenode="632588433343350482" appnode="632588433343350479" handlerfor="632588433343350478">
    <node type="Start" id="632588433343350481" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="288">
      <linkto id="632588433343350503" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632588433343350503" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="136" y="288">
      <linkto id="632588433343350520" type="Labeled" style="Bezier" ortho="true" label="listerrors" />
      <linkto id="632588433343350526" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632588433343350504" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="376" y="264" mx="429" my="280">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632588433343350482" />
        <item text="OnPlay_Failed" treenode="632588433343350487" />
      </items>
      <linkto id="632588433343350513" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="csharp">"Error " + (g_errorCount + 1)</ap>
        <ap name="Prompt2" type="csharp">"The time of the error is, " + g_errors.Collection[g_errorCount].Time.ToLongDateString()</ap>
        <ap name="Prompt3" type="csharp">"The error message is, " + g_errors.Collection[g_errorCount].Description</ap>
        <ap name="ConnectionId" type="variable">g_adminConnectionId</ap>
        <ap name="UserData" type="literal">listerrors</ap>
      </Properties>
    </node>
    <node type="Action" id="632588433343350513" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="664" y="288">
      <linkto id="632588433343350516" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref int g_errorCount)
{
 	g_errorCount++;
	return "";
}
</Properties>
    </node>
    <node type="Comment" id="632588433343350514" text="Play the next error" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="392" y="207" />
    <node type="Comment" id="632588433343350515" text="Increment the error index" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="592" y="209" />
    <node type="Action" id="632588433343350516" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="848" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632588433343350519" text="Check that there are no more errors" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="184" y="208" />
    <node type="Action" id="632588433343350520" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="288" y="288">
      <linkto id="632588433343350504" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632588433343350521" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_errors.Collection.Count == g_errorCount</ap>
      </Properties>
    </node>
    <node type="Action" id="632588433343350521" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="240" y="384" mx="293" my="400">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632588433343350482" />
        <item text="OnPlay_Failed" treenode="632588433343350487" />
      </items>
      <linkto id="632588433343350525" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">There are no more errors</ap>
        <ap name="ConnectionId" type="variable">g_adminConnectionId</ap>
        <ap name="UserData" type="literal">hangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632588433343350525" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="288" y="576">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632588433343350526" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="132" y="414">
      <linkto id="632588433343350527" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_adminCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632588433343350527" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="136" y="576">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632588433343350528" text="Tell the admin there are no more errors" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="199" y="442" />
    <node type="Variable" id="632588433343350502" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632588433343350486" treenode="632588433343350487" appnode="632588433343350484" handlerfor="632588433343350483">
    <node type="Start" id="632588433343350486" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="288">
      <linkto id="632588433343350530" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632588433343350530" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="352" y="300">
      <linkto id="632588433343350531" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_adminCallId</ap>
        <log condition="entry" on="true" level="Info" type="literal">A play failed</log>
      </Properties>
    </node>
    <node type="Action" id="632588433343350531" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="544" y="296">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>