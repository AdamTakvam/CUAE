<Application name="HandleMakeCall" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="HandleMakeCall">
    <outline>
      <treenode type="evh" id="632929999374687933" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632929999374687930" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632929999374687929" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/MakeCall</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632929999374687939" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632929999374687936" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632929999374687935" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632968511415200412" actid="632929999374687950" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632929999374687944" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632929999374687941" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632929999374687940" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632968511415200413" actid="632929999374687950" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632929999374687949" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632929999374687946" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632929999374687945" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632968511415200414" actid="632929999374687950" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632929999374687998" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632929999374687995" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632929999374687994" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632968511415200423" actid="632929999374688004" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632929999374688003" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632929999374688000" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632929999374687999" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632968511415200424" actid="632929999374688004" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632968500575340481" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632968500575340478" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632968500575340477" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632968500575340486" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632968500575340483" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632968500575340482" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632968500575340491" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632968500575340488" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632968500575340487" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="to" id="632968511415200407" vid="632929999374687984">
        <Properties type="String" initWith="To">to</Properties>
      </treenode>
      <treenode text="g_callId" id="632968511415200409" vid="632929999374688018">
        <Properties type="String">g_callId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632929999374687932" treenode="632929999374687933" appnode="632929999374687930" handlerfor="632929999374687929">
    <node type="Start" id="632929999374687932" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="444">
      <linkto id="632929999374687950" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632929999374687950" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="288" y="424" mx="354" my="440">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632929999374687939" />
        <item text="OnMakeCall_Failed" treenode="632929999374687944" />
        <item text="OnRemoteHangup" treenode="632929999374687949" />
      </items>
      <linkto id="632929999374687988" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632929999374687989" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">to</ap>
        <ap name="From" type="literal">MakeCallApp</ap>
        <ap name="DisplayName" type="literal">HandleMakeCall</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_callId</rd>
        <log condition="entry" on="true" level="Info" type="literal">Attempting to Make Call</log>
      </Properties>
    </node>
    <node type="Comment" id="632929999374687986" text="Using the 'to' global variable,&#xD;&#xA;which is initialized from the&#xD;&#xA;application install file, &#xD;&#xA;the application will make a call&#xD;&#xA;to the configured number." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="273" y="260" />
    <node type="Action" id="632929999374687988" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="635" y="441">
      <linkto id="632929999374687992" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="literal">The call was initiated successfully.
If the specified endpoint does not ring, check the Application Server log for errors.</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <log condition="entry" on="true" level="Info" type="literal">The MakeCall application successfully initiated the call...</log>
      </Properties>
    </node>
    <node type="Action" id="632929999374687989" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="353" y="697">
      <linkto id="632929999374687991" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="literal">The call did not initiate successfully.
Check the Application Server logs for errors.</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <log condition="entry" on="true" level="Error" type="literal">The MakeCall application did not successfully initiate the call...</log>
      </Properties>
    </node>
    <node type="Action" id="632929999374687991" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="353" y="842">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632929999374687992" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="884" y="441">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632929999374687934" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" activetab="true" startnode="632929999374687938" treenode="632929999374687939" appnode="632929999374687936" handlerfor="632929999374687935">
    <node type="Start" id="632929999374687938" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="436">
      <linkto id="632929999374688004" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632929999374688004" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="245" y="418" mx="298" my="434">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632929999374687998" />
        <item text="OnPlay_Failed" treenode="632929999374688003" />
      </items>
      <linkto id="632929999374688008" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632929999374688010" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">makecall_good_bye.wav</ap>
        <ap name="Prompt3" type="literal">makecall_good_bye.wav</ap>
        <ap name="Prompt1" type="literal">makecall_good_bye.wav</ap>
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="ResultCode">resultCode</rd>
        <log condition="entry" on="true" level="Info" type="literal">The call made by the MakeCall application completed.  Playing sample wav file.</log>
      </Properties>
    </node>
    <node type="Action" id="632929999374688008" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="663" y="434">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">The play command in MakeCall was initiated successfully...</log>
      </Properties>
    </node>
    <node type="Action" id="632929999374688010" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="296" y="688">
      <linkto id="632929999374688013" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="entry" on="true" level="Error" type="csharp">"The play command in MakeCall was not initiated successfully.  The 'resultcode' for the failed play operation was: " + resultCode</log>
      </Properties>
    </node>
    <node type="Action" id="632929999374688013" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="295" y="837">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632929999374688007" name="connId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="connectionId" refType="reference">connId</Properties>
    </node>
    <node type="Variable" id="632929999374688011" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632929999374688012" name="resultCode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="-1" refType="reference">resultCode</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632929999374687943" treenode="632929999374687944" appnode="632929999374687941" handlerfor="632929999374687940">
    <node type="Start" id="632929999374687943" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="84" y="437">
      <linkto id="632929999374688014" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632929999374688014" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="345" y="433">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="csharp">"The call did not complete successfully in the MakeCall application.  The endReason for the failed call was: " + endReason</log>
      </Properties>
    </node>
    <node type="Variable" id="632929999374687954" name="endReason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="endReason" refType="reference">endReason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632929999374687948" treenode="632929999374687949" appnode="632929999374687946" handlerfor="632929999374687945">
    <node type="Start" id="632929999374687948" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="88" y="440">
      <linkto id="632929999374688016" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632929999374688016" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="414" y="436">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">The called party in the MakeCall application hung up.</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632929999374687997" treenode="632929999374687998" appnode="632929999374687995" handlerfor="632929999374687994">
    <node type="Start" id="632929999374687997" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="66" y="431">
      <linkto id="632929999374688028" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632929999374688017" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="369" y="427">
      <linkto id="632929999374688020" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
        <log condition="entry" on="true" level="Info" type="literal">The play in the MakeCall application completed successfully.  Hanging up the called party...</log>
      </Properties>
    </node>
    <node type="Action" id="632929999374688020" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="548" y="425">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632929999374688028" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="168" y="430">
      <linkto id="632929999374688017" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632929999374688029" type="Labeled" style="Bezier" ortho="true" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">endReason</ap>
      </Properties>
    </node>
    <node type="Action" id="632929999374688029" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="171" y="653">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">The call ended for reason autostop in the MakeCall application, which indicates that the party hung up.</log>
      </Properties>
    </node>
    <node type="Variable" id="632929999374688027" name="endReason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Play_Complete">endReason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632929999374688002" treenode="632929999374688003" appnode="632929999374688000" handlerfor="632929999374687999">
    <node type="Start" id="632929999374688002" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="101" y="464">
      <linkto id="632929999374688021" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632929999374688021" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="282.1237" y="461">
      <linkto id="632929999374688022" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
        <log condition="entry" on="true" level="Info" type="literal">"The play in the MakeCall application did not complete successfully.  The result code reported was: " + resultCode</log>
      </Properties>
    </node>
    <node type="Action" id="632929999374688022" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="494.123657" y="457">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632929999374688025" name="resultCode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ResultCode" refType="reference" name="Metreos.MediaControl.Play_Failed">resultCode</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632968500575340480" treenode="632968500575340481" appnode="632968500575340478" handlerfor="632968500575340477">
    <node type="Start" id="632968500575340480" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632968500575340494" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632968500575340494" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="120" y="60">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632968500575340485" treenode="632968500575340486" appnode="632968500575340483" handlerfor="632968500575340482">
    <node type="Start" id="632968500575340485" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632968500575340493" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632968500575340493" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="118" y="64">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632968500575340490" treenode="632968500575340491" appnode="632968500575340488" handlerfor="632968500575340487">
    <node type="Start" id="632968500575340490" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632968500575340492" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632968500575340492" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="124" y="36">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>