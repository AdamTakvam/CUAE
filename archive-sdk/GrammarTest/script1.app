<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632811263452121548" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632811263452121545" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632811263452121544" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/grammar</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632811263452121556" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632811263452121553" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632811263452121552" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632811263452121710" actid="632811263452121567" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632811263452121561" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632811263452121558" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632811263452121557" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632811263452121711" actid="632811263452121567" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632811263452121566" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632811263452121563" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632811263452121562" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632811263452121712" actid="632811263452121567" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632811263452121613" level="2" text="Metreos.MediaControl.VoiceRecognition_Complete: OnVoiceRecognition_Complete">
        <node type="function" name="OnVoiceRecognition_Complete" id="632811263452121610" path="Metreos.StockTools" />
        <node type="event" name="VoiceRecognition_Complete" id="632811263452121609" path="Metreos.MediaControl.VoiceRecognition_Complete" />
        <references>
          <ref id="632811263452121788" actid="632811263452121786" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632811263452121618" level="2" text="Metreos.MediaControl.VoiceRecognition_Failed: OnVoiceRecognition_Failed">
        <node type="function" name="OnVoiceRecognition_Failed" id="632811263452121615" path="Metreos.StockTools" />
        <node type="event" name="VoiceRecognition_Failed" id="632811263452121614" path="Metreos.MediaControl.VoiceRecognition_Failed" />
        <references>
          <ref id="632811263452121789" actid="632811263452121786" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632811263452121771" level="2" text="Metreos.MediaControl.PlayTone_Complete: OnPlayTone_Complete">
        <node type="function" name="OnPlayTone_Complete" id="632811263452121768" path="Metreos.StockTools" />
        <node type="event" name="PlayTone_Complete" id="632811263452121767" path="Metreos.MediaControl.PlayTone_Complete" />
        <references>
          <ref id="632811263452121778" actid="632811263452121777" />
          <ref id="632811263452121782" actid="632811263452121780" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632811263452121776" level="2" text="Metreos.MediaControl.PlayTone_Failed: OnPlayTone_Failed">
        <node type="function" name="OnPlayTone_Failed" id="632811263452121773" path="Metreos.StockTools" />
        <node type="event" name="PlayTone_Failed" id="632811263452121772" path="Metreos.MediaControl.PlayTone_Failed" />
        <references>
          <ref id="632811263452121779" actid="632811263452121777" />
          <ref id="632811263452121783" actid="632811263452121780" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_to" id="632811263452121697" vid="632811263452121600">
        <Properties type="String" initWith="to">g_to</Properties>
      </treenode>
      <treenode text="g_callId" id="632811263452121699" vid="632811263452121602">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632811263452121701" vid="632811263452121604">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_recordId" id="632811263452121703" vid="632811263452121623">
        <Properties type="String">g_recordId</Properties>
      </treenode>
      <treenode text="g_voiceId" id="632811263452121705" vid="632811263452121625">
        <Properties type="String">g_voiceId</Properties>
      </treenode>
      <treenode text="g_ip" id="632811263452121707" vid="632811263452121636">
        <Properties type="String">g_ip</Properties>
      </treenode>
      <treenode text="g_username" id="632811263452121750" vid="632811263452121749">
        <Properties type="String" initWith="HttpUsername">g_username</Properties>
      </treenode>
      <treenode text="g_password" id="632811263452121752" vid="632811263452121751">
        <Properties type="String" initWith="HttpPassword">g_password</Properties>
      </treenode>
      <treenode text="g_grammar1" id="632811263452121754" vid="632811263452121753">
        <Properties type="String" initWith="grammar1">g_grammar1</Properties>
      </treenode>
      <treenode text="g_grammar2" id="632811263452121756" vid="632811263452121755">
        <Properties type="String" initWith="grammar2">g_grammar2</Properties>
      </treenode>
      <treenode text="g_grammar3" id="632811263452121758" vid="632811263452121757">
        <Properties type="String" initWith="grammar3">g_grammar3</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632811263452121547" treenode="632811263452121548" appnode="632811263452121545" handlerfor="632811263452121544">
    <node type="Start" id="632811263452121547" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="244">
      <linkto id="632811263452121567" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632811263452121567" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="171" y="227" mx="237" my="243">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632811263452121556" />
        <item text="OnMakeCall_Failed" treenode="632811263452121561" />
        <item text="OnRemoteHangup" treenode="632811263452121566" />
      </items>
      <linkto id="632811263452121652" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_to</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_callId</rd>
      </Properties>
    </node>
    <node type="Action" id="632811263452121606" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="571" y="244">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632811263452121652" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="432" y="243">
      <linkto id="632811263452121606" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">hi</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Variable" id="632811263452121549" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632811263452121555" treenode="632811263452121556" appnode="632811263452121553" handlerfor="632811263452121552">
    <node type="Start" id="632811263452121555" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="59" y="293">
      <linkto id="632811263452121777" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632811263452121627" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="652.4121" y="287">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632811263452121639" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="529.4121" y="286">
      <linkto id="632811263452121627" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">ip</ap>
        <rd field="ResultData">g_ip</rd>
      </Properties>
    </node>
    <node type="Action" id="632811263452121777" name="PlayTone" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="141" y="271" mx="207" my="287">
      <items count="2">
        <item text="OnPlayTone_Complete" treenode="632811263452121771" />
        <item text="OnPlayTone_Failed" treenode="632811263452121776" />
      </items>
      <linkto id="632811263452121639" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Duration" type="literal">500</ap>
        <ap name="Frequency1" type="literal">1500</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="Amplitude1" type="literal">-10</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="ConnectionId">g_connectionId</rd>
      </Properties>
    </node>
    <node type="Variable" id="632811263452121607" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connectionId</Properties>
    </node>
    <node type="Variable" id="632811263452121638" name="ip" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="MediaTxIP" refType="reference" name="Metreos.CallControl.MakeCall_Complete">ip</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632811263452121560" treenode="632811263452121561" appnode="632811263452121558" handlerfor="632811263452121557">
    <node type="Start" id="632811263452121560" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632811263452121628" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632811263452121628" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="529" y="314">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632811263452121565" treenode="632811263452121566" appnode="632811263452121563" handlerfor="632811263452121562">
    <node type="Start" id="632811263452121565" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="252">
      <linkto id="632811263452121629" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632811263452121629" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="355" y="275">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnVoiceRecognition_Complete" startnode="632811263452121612" treenode="632811263452121613" appnode="632811263452121610" handlerfor="632811263452121609">
    <node type="Start" id="632811263452121612" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="285">
      <linkto id="632811263452121641" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632811263452121641" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="132" y="285">
      <linkto id="632811263452121642" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Text" type="csharp">"Score: " + score + "\nMeaning: " + meaning</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632811263452121642" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="296" y="286">
      <linkto id="632811263452121780" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">text.ToString()</ap>
        <ap name="URL" type="variable">g_ip</ap>
        <ap name="Username" type="variable">g_password</ap>
        <ap name="Password" type="variable">g_username</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Score: " + score + "\nMeaning: " + meaning</log>
      </Properties>
    </node>
    <node type="Action" id="632811263452121650" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="604" y="283">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632811263452121780" name="PlayTone" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="385" y="272.1667" mx="451" my="288">
      <items count="2">
        <item text="OnPlayTone_Complete" treenode="632811263452121771" />
        <item text="OnPlayTone_Failed" treenode="632811263452121776" />
      </items>
      <linkto id="632811263452121650" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Duration" type="literal">500</ap>
        <ap name="Frequency1" type="literal">1500</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Amplitude1" type="literal">-10</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Variable" id="632811263452121630" name="score" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Score" refType="reference" name="Metreos.MediaControl.VoiceRecognition_Complete">score</Properties>
    </node>
    <node type="Variable" id="632811263452121631" name="meaning" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Meaning" refType="reference" name="Metreos.MediaControl.VoiceRecognition_Complete">meaning</Properties>
    </node>
    <node type="Variable" id="632811263452121640" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnVoiceRecognition_Failed" startnode="632811263452121617" treenode="632811263452121618" appnode="632811263452121615" handlerfor="632811263452121614">
    <node type="Start" id="632811263452121617" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="258">
      <linkto id="632811263452121795" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632811263452121651" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="431" y="263">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">VR Failed</log>
      </Properties>
    </node>
    <node type="Action" id="632811263452121795" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="107.746094" y="259">
      <linkto id="632811263452121796" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Text" type="csharp">"Failed Voice Rec--quitting"</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632811263452121796" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="271.7461" y="260">
      <linkto id="632811263452121651" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">text.ToString()</ap>
        <ap name="URL" type="variable">g_ip</ap>
        <ap name="Username" type="variable">g_password</ap>
        <ap name="Password" type="variable">g_username</ap>
      </Properties>
    </node>
    <node type="Variable" id="632811263452121799" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" initWith="" refType="reference">text</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayTone_Complete" activetab="true" startnode="632811263452121770" treenode="632811263452121771" appnode="632811263452121768" handlerfor="632811263452121767">
    <node type="Start" id="632811263452121770" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="78" y="279">
      <linkto id="632811263452121786" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632811263452121784" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="443" y="278">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632811263452121786" name="VoiceRecognition" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="163.140778" y="263" mx="251" my="279">
      <items count="2">
        <item text="OnVoiceRecognition_Complete" treenode="632811263452121613" />
        <item text="OnVoiceRecognition_Failed" treenode="632811263452121618" />
      </items>
      <linkto id="632811263452121784" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632811263452121800" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Grammar3" type="csharp">g_grammar3 == "NONE" ? null : g_grammar3</ap>
        <ap name="VoiceBargeIn" type="literal">True</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Grammar1" type="csharp">g_grammar1 == "NONE" ? null : g_grammar1</ap>
        <ap name="Grammar2" type="csharp">g_grammar2 == "NONE" ? null : g_grammar2</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="ConnectionId">g_connectionId</rd>
        <rd field="OperationId">g_voiceId</rd>
      </Properties>
    </node>
    <node type="Action" id="632811263452121800" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="227.7461" y="445">
      <linkto id="632811263452121801" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Text" type="csharp">"Failed Voice Rec--quitting"</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632811263452121801" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="391.7461" y="446">
      <linkto id="632811263452121804" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">text.ToString()</ap>
        <ap name="URL" type="variable">g_ip</ap>
        <ap name="Username" type="variable">g_password</ap>
        <ap name="Password" type="variable">g_username</ap>
      </Properties>
    </node>
    <node type="Action" id="632811263452121804" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="522" y="446">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632811263452121805" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayTone_Failed" startnode="632811263452121775" treenode="632811263452121776" appnode="632811263452121773" handlerfor="632811263452121772">
    <node type="Start" id="632811263452121775" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632811263452121785" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632811263452121785" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="395" y="157">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>