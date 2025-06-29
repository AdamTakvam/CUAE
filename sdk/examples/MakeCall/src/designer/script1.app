<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="633136160585469214" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="633136160585469211" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633136160585469210" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/MakeCall</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="633136160585469219" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="633136160585469216" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="633136160585469215" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="633136160585469522" actid="633136160585469230" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633136160585469224" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="633136160585469221" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="633136160585469220" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="633136160585469523" actid="633136160585469230" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633136160585469229" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="633136160585469226" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="633136160585469225" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="633136160585469524" actid="633136160585469230" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="633136160585469244" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="633136160585469241" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="633136160585469240" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="633136160585469532" actid="633136160585469250" />
          <ref id="633136160585469539" actid="633136160585469396" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633136160585469249" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="633136160585469246" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="633136160585469245" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="633136160585469533" actid="633136160585469250" />
          <ref id="633136160585469540" actid="633136160585469396" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_To" id="633136160585469507" vid="633136160585469263">
        <Properties type="String" initWith="Config.To">g_To</Properties>
      </treenode>
      <treenode text="g_CallID" id="633136160585469509" vid="633136160585469265">
        <Properties type="String">g_CallID</Properties>
      </treenode>
      <treenode text="g_UseTTS" id="633136160585469511" vid="633136160585469391">
        <Properties type="Bool" initWith="Config.UseTTS">g_UseTTS</Properties>
      </treenode>
      <treenode text="g_TTSMessage" id="633136160585469513" vid="633136160585469393">
        <Properties type="String" initWith="Config.TTSMessage">g_TTSMessage</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="633136160585469213" treenode="633136160585469214" appnode="633136160585469211" handlerfor="633136160585469210">
    <node type="Start" id="633136160585469213" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633136160585469230" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="633136160585469230" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="219" y="32" mx="285" my="48">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="633136160585469219" />
        <item text="OnMakeCall_Failed" treenode="633136160585469224" />
        <item text="OnRemoteHangup" treenode="633136160585469229" />
      </items>
      <linkto id="633136160585469234" type="Labeled" style="Vector" ortho="true" label="Success" />
      <linkto id="633136160585469236" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_To</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
        <rd field="CallId">g_CallID</rd>
      </Properties>
    </node>
    <node type="Action" id="633136160585469234" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="520" y="96">
      <linkto id="633136160585469235" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="body" type="literal">Success</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
      </Properties>
    </node>
    <node type="Action" id="633136160585469235" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="690" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633136160585469236" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="298" y="261">
      <linkto id="633136160585469238" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="body" type="literal">Failure</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
      </Properties>
    </node>
    <node type="Action" id="633136160585469238" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="360" y="428">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633136160585469239" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" activetab="true" startnode="633136160585469218" treenode="633136160585469219" appnode="633136160585469216" handlerfor="633136160585469215">
    <node type="Start" id="633136160585469218" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633136160585469395" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="633136160585469250" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="202" y="32" mx="255" my="48">
      <items count="2">
        <item text="OnPlay_Complete" treenode="633136160585469244" />
        <item text="OnPlay_Failed" treenode="633136160585469249" />
      </items>
      <linkto id="633136160585469253" type="Labeled" style="Vector" ortho="true" label="Success" />
      <linkto id="633136160585469255" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">makecall_good_bye.wav</ap>
        <ap name="Prompt2" type="literal">makecall_good_bye.wav</ap>
        <ap name="ConnectionId" type="variable">connectionID</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="633136160585469253" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="451" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633136160585469255" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="242" y="183">
      <linkto id="633136160585469256" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_CallID</ap>
      </Properties>
    </node>
    <node type="Action" id="633136160585469256" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="268" y="295">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633136160585469395" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="113" y="32">
      <linkto id="633136160585469250" type="Labeled" style="Vector" ortho="true" label="false" />
      <linkto id="633136160585469396" type="Labeled" style="Vector" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">g_UseTTS</ap>
      </Properties>
    </node>
    <node type="Action" id="633136160585469396" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="68" y="160" mx="121" my="176">
      <items count="2">
        <item text="OnPlay_Complete" treenode="633136160585469244" />
        <item text="OnPlay_Failed" treenode="633136160585469249" />
      </items>
      <linkto id="633136160585469255" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="633136160585469399" type="Labeled" style="Vector" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="variable">g_TTSMessage</ap>
        <ap name="ConnectionId" type="variable">connectionID</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"TTS: " + g_TTSMessage</log>
      </Properties>
    </node>
    <node type="Action" id="633136160585469399" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="112" y="305">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633136160585469268" name="connectionID" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connectionID</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="633136160585469223" treenode="633136160585469224" appnode="633136160585469221" handlerfor="633136160585469220">
    <node type="Start" id="633136160585469223" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633136160585469254" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="633136160585469254" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="213" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="633136160585469228" treenode="633136160585469229" appnode="633136160585469226" handlerfor="633136160585469225">
    <node type="Start" id="633136160585469228" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633136160585469257" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="633136160585469257" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="765" y="244">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="633136160585469243" treenode="633136160585469244" appnode="633136160585469241" handlerfor="633136160585469240">
    <node type="Start" id="633136160585469243" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633136160585469258" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="633136160585469258" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="210" y="121">
      <linkto id="633136160585469259" type="Labeled" style="Vector" ortho="true" label="autostop" />
      <linkto id="633136160585469260" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">terminationCondition</ap>
      </Properties>
    </node>
    <node type="Action" id="633136160585469259" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="204" y="312">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633136160585469260" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="384" y="67">
      <linkto id="633136160585469261" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_CallID</ap>
      </Properties>
    </node>
    <node type="Action" id="633136160585469261" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="578" y="57">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633136160585469267" name="terminationCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Play_Complete">terminationCondition</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="633136160585469248" treenode="633136160585469249" appnode="633136160585469246" handlerfor="633136160585469245">
    <node type="Start" id="633136160585469248" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633136160585469262" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="633136160585469262" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="414" y="119">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>