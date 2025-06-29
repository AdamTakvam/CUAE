<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="633136160585469214" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="633136160585469211" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633136160585469210" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/MakeMyConference</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="633136160585469219" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="633136160585469216" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="633136160585469215" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="633136234860938209" actid="633136160585469230" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633136160585469224" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="633136160585469221" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="633136160585469220" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="633136234860938210" actid="633136160585469230" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633136160585469229" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="633136160585469226" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="633136160585469225" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="633136234860938211" actid="633136160585469230" />
          <ref id="633136234860938249" actid="633136234860938246" />
          <ref id="633136234860938253" actid="633136234860938250" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="633136234860938269" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall2_Complete" actid="633136234860938246">
        <node type="function" name="OnMakeCall2_Complete" id="633136234860938264" path="Metreos.StockTools" />
        <node type="event" name="OnMakeCall2_Complete" id="633136234860938267" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="633136234860938268" actid="633136234860938246" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633136234860938275" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall2_Failed" actid="633136234860938246">
        <node type="function" name="OnMakeCall2_Failed" id="633136234860938270" path="Metreos.StockTools" />
        <node type="event" name="OnMakeCall2_Failed" id="633136234860938273" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="633136234860938274" actid="633136234860938246" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633136234860938281" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall3_Failed" actid="633136234860938250">
        <node type="function" name="OnMakeCall3_Failed" id="633136234860938276" path="Metreos.StockTools" />
        <node type="event" name="OnMakeCall3_Failed" id="633136234860938279" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="633136234860938280" actid="633136234860938250" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633136234860938287" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall3_Complete" actid="633136234860938250">
        <node type="function" name="OnMakeCall3_Complete" id="633136234860938282" path="Metreos.StockTools" />
        <node type="event" name="OnMakeCall3_Complete" id="633136234860938285" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="633136234860938286" actid="633136234860938250" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_To1" id="633136234860938200" vid="633136160585469263">
        <Properties type="String" initWith="Config.To1">g_To1</Properties>
      </treenode>
      <treenode text="g_CallID" id="633136234860938202" vid="633136160585469265">
        <Properties type="String">g_CallID</Properties>
      </treenode>
      <treenode text="g_To2" id="633136234860938204" vid="633136234860938167">
        <Properties type="String" initWith="Config.To2">g_To2</Properties>
      </treenode>
      <treenode text="g_To3" id="633136234860938206" vid="633136234860938169">
        <Properties type="String" initWith="Config.To3">g_To3</Properties>
      </treenode>
      <treenode text="g_ConferenceID" id="633136234860938244" vid="633136234860938243">
        <Properties type="String">g_ConferenceID</Properties>
      </treenode>
      <treenode text="g_firstCallCompleted" id="633136234860938299" vid="633136234860938298">
        <Properties type="Bool" defaultInitWith="false">g_firstCallCompleted</Properties>
      </treenode>
      <treenode text="g_CallID2" id="633136234860938335" vid="633136234860938334">
        <Properties type="String">g_CallID2</Properties>
      </treenode>
      <treenode text="g_CallID3" id="633136234860938337" vid="633136234860938336">
        <Properties type="String">g_CallID3</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="633136160585469213" treenode="633136160585469214" appnode="633136160585469211" handlerfor="633136160585469210">
    <node type="Start" id="633136160585469213" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633136234860938300" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="633136160585469230" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="226" y="16" mx="292" my="32">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="633136160585469219" />
        <item text="OnMakeCall_Failed" treenode="633136160585469224" />
        <item text="OnRemoteHangup" treenode="633136160585469229" />
      </items>
      <linkto id="633136160585469236" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="633136234860938246" type="Labeled" style="Vector" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_To1</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
        <rd field="CallId">g_CallID</rd>
      </Properties>
    </node>
    <node type="Action" id="633136160585469234" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="737" y="95">
      <linkto id="633136160585469235" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="body" type="literal">Success</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
      </Properties>
    </node>
    <node type="Action" id="633136160585469235" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="863" y="32">
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
    <node type="Action" id="633136234860938246" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="449.6631" y="32" mx="519" my="48">
      <items count="3">
        <item text="OnMakeCall2_Complete" treenode="633136234860938269" />
        <item text="OnMakeCall2_Failed" treenode="633136234860938275" />
        <item text="OnRemoteHangup" treenode="633136160585469229" />
      </items>
      <linkto id="633136234860938250" type="Labeled" style="Vector" ortho="true" label="Success" />
      <linkto id="633136160585469236" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_To2</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
        <rd field="CallId">g_CallID2</rd>
      </Properties>
    </node>
    <node type="Action" id="633136234860938250" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="465.6631" y="245" mx="535" my="261">
      <items count="3">
        <item text="OnMakeCall3_Complete" treenode="633136234860938287" />
        <item text="OnMakeCall3_Failed" treenode="633136234860938281" />
        <item text="OnRemoteHangup" treenode="633136160585469229" />
      </items>
      <linkto id="633136160585469236" type="Labeled" style="Vector" ortho="true" label="default" />
      <linkto id="633136160585469234" type="Labeled" style="Vector" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_To3</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
        <rd field="CallId">g_CallID3</rd>
      </Properties>
    </node>
    <node type="Action" id="633136234860938300" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="129" y="32">
      <linkto id="633136160585469230" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">false</ap>
        <rd field="ResultData">g_firstCallCompleted</rd>
      </Properties>
    </node>
    <node type="Variable" id="633136160585469239" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="633136160585469218" treenode="633136160585469219" appnode="633136160585469216" handlerfor="633136160585469215">
    <node type="Start" id="633136160585469218" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633136234860938304" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="633136234860938302" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="465" y="33">
      <linkto id="633136234860938305" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <rd field="ResultData">g_firstCallCompleted</rd>
      </Properties>
    </node>
    <node type="Action" id="633136234860938304" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="310" y="33">
      <linkto id="633136234860938302" type="Labeled" style="Vector" ortho="true" label="false" />
      <linkto id="633136234860938307" type="Labeled" style="Vector" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">g_firstCallCompleted</ap>
      </Properties>
    </node>
    <node type="Action" id="633136234860938305" name="CreateConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="527.4759" y="124">
      <linkto id="633136234860938311" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_CallID</ap>
        <ap name="ConnectionId" type="variable">connectionID</ap>
        <rd field="ConferenceId">g_ConferenceID</rd>
      </Properties>
    </node>
    <node type="Action" id="633136234860938307" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="310" y="276">
      <linkto id="633136234860938311" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionID</ap>
        <ap name="ConferenceId" type="variable">g_ConferenceID</ap>
      </Properties>
    </node>
    <node type="Action" id="633136234860938311" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="528" y="276">
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
  <canvas type="Function" name="OnRemoteHangup" activetab="true" startnode="633136160585469228" treenode="633136160585469229" appnode="633136160585469226" handlerfor="633136160585469225">
    <node type="Start" id="633136160585469228" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633136234860938338" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="633136160585469257" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="765" y="244">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633136234860938338" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="305" y="74">
      <linkto id="633136234860938339" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_CallID</ap>
      </Properties>
    </node>
    <node type="Action" id="633136234860938339" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="305" y="233">
      <linkto id="633136234860938340" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_CallID2</ap>
      </Properties>
    </node>
    <node type="Action" id="633136234860938340" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="300" y="417">
      <linkto id="633136160585469257" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_CallID3</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall2_Complete" startnode="633136234860938266" treenode="633136234860938269" appnode="633136234860938264">
    <node type="Start" id="633136234860938266" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="32">
      <linkto id="633136234860938314" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="633136234860938313" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="449" y="49">
      <linkto id="633136234860938315" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <rd field="ResultData">g_firstCallCompleted</rd>
      </Properties>
    </node>
    <node type="Action" id="633136234860938314" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="294" y="49">
      <linkto id="633136234860938313" type="Labeled" style="Vector" ortho="true" label="false" />
      <linkto id="633136234860938316" type="Labeled" style="Vector" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">g_firstCallCompleted</ap>
      </Properties>
    </node>
    <node type="Action" id="633136234860938315" name="CreateConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="511.4759" y="140">
      <linkto id="633136234860938317" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_CallID</ap>
        <ap name="ConnectionId" type="variable">connectionID</ap>
        <rd field="ConferenceId">g_ConferenceID</rd>
      </Properties>
    </node>
    <node type="Action" id="633136234860938316" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="294" y="292">
      <linkto id="633136234860938317" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionID</ap>
        <ap name="ConferenceId" type="variable">g_ConferenceID</ap>
      </Properties>
    </node>
    <node type="Action" id="633136234860938317" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="512" y="292">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633136234860938291" name="connectionID" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connectionID</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall2_Failed" startnode="633136234860938272" treenode="633136234860938275" appnode="633136234860938270">
    <node type="Start" id="633136234860938272" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633136234860938292" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="633136234860938292" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="401" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall3_Failed" startnode="633136234860938278" treenode="633136234860938281" appnode="633136234860938276">
    <node type="Start" id="633136234860938278" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633136234860938293" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="633136234860938293" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="505" y="79">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall3_Complete" startnode="633136234860938284" treenode="633136234860938287" appnode="633136234860938282">
    <node type="Start" id="633136234860938284" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633136234860938325" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="633136234860938324" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="346.735016" y="38">
      <linkto id="633136234860938326" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <rd field="ResultData">g_firstCallCompleted</rd>
      </Properties>
    </node>
    <node type="Action" id="633136234860938325" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="191.735016" y="38">
      <linkto id="633136234860938324" type="Labeled" style="Vector" ortho="true" label="false" />
      <linkto id="633136234860938327" type="Labeled" style="Vector" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">g_firstCallCompleted</ap>
      </Properties>
    </node>
    <node type="Action" id="633136234860938326" name="CreateConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="409.210876" y="129">
      <linkto id="633136234860938328" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_CallID</ap>
        <ap name="ConnectionId" type="variable">connectionID</ap>
        <rd field="ConferenceId">g_ConferenceID</rd>
      </Properties>
    </node>
    <node type="Action" id="633136234860938327" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="191.735016" y="281">
      <linkto id="633136234860938328" type="Labeled" style="Vector" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionID</ap>
        <ap name="ConferenceId" type="variable">g_ConferenceID</ap>
      </Properties>
    </node>
    <node type="Action" id="633136234860938328" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="409.735016" y="281">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633136234860938294" name="connectionID" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connectionID</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>