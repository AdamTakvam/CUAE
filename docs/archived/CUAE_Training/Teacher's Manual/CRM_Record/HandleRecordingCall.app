<Application name="HandleRecordingCall" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="HandleRecordingCall">
    <outline>
      <treenode type="evh" id="632935022220433438" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632935022220433435" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632935022220433434" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632935022220433445" level="2" text="Metreos.MediaControl.PlayTone_Complete: OnPlayTone_Complete">
        <node type="function" name="OnPlayTone_Complete" id="632935022220433442" path="Metreos.StockTools" />
        <node type="event" name="PlayTone_Complete" id="632935022220433441" path="Metreos.MediaControl.PlayTone_Complete" />
        <references>
          <ref id="632935022220433643" actid="632935022220433451" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632935022220433450" level="2" text="Metreos.MediaControl.PlayTone_Failed: OnPlayTone_Failed">
        <node type="function" name="OnPlayTone_Failed" id="632935022220433447" path="Metreos.StockTools" />
        <node type="event" name="PlayTone_Failed" id="632935022220433446" path="Metreos.MediaControl.PlayTone_Failed" />
        <references>
          <ref id="632935022220433644" actid="632935022220433451" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632935022220433464" level="2" text="Metreos.MediaControl.Record_Complete: OnRecord_Complete">
        <node type="function" name="OnRecord_Complete" id="632935022220433461" path="Metreos.StockTools" />
        <node type="event" name="Record_Complete" id="632935022220433460" path="Metreos.MediaControl.Record_Complete" />
        <references>
          <ref id="632935022220433650" actid="632935022220433470" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632935022220433469" level="2" text="Metreos.MediaControl.Record_Failed: OnRecord_Failed">
        <node type="function" name="OnRecord_Failed" id="632935022220433466" path="Metreos.StockTools" />
        <node type="event" name="Record_Failed" id="632935022220433465" path="Metreos.MediaControl.Record_Failed" />
        <references>
          <ref id="632935022220433651" actid="632935022220433470" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632935022220433482" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632935022220433479" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632935022220433478" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="632935022220433637" vid="632935022220433455">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_connId" id="632935022220433639" vid="632935022220433457">
        <Properties type="String">g_connId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632935022220433437" treenode="632935022220433438" appnode="632935022220433435" handlerfor="632935022220433434">
    <node type="Start" id="632935022220433437" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="88" y="448">
      <linkto id="632935022220433440" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632935022220433440" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="344" y="448">
      <linkto id="632935022220433451" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="ConnectionId">g_connId</rd>
      </Properties>
    </node>
    <node type="Action" id="632935022220433451" name="PlayTone" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="488" y="432" mx="554" my="448">
      <items count="2">
        <item text="OnPlayTone_Complete" treenode="632935022220433445" />
        <item text="OnPlayTone_Failed" treenode="632935022220433450" />
      </items>
      <linkto id="632935022220433459" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Duration" type="literal">500</ap>
        <ap name="Frequency1" type="literal">1000</ap>
        <ap name="ConnectionId" type="variable">g_connId</ap>
        <ap name="Frequency2" type="literal">1200</ap>
        <ap name="Amplitude1" type="literal">-20</ap>
        <ap name="Amplitude2" type="literal">-20</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632935022220433459" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="720" y="456">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632935022220433439" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632935022220433454" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="from" refType="reference">from</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayTone_Complete" startnode="632935022220433444" treenode="632935022220433445" appnode="632935022220433442" handlerfor="632935022220433441">
    <node type="Start" id="632935022220433444" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="120" y="448">
      <linkto id="632935022220433470" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632935022220433470" name="Record" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="272" y="432" mx="332" my="448">
      <items count="2">
        <item text="OnRecord_Complete" treenode="632935022220433464" />
        <item text="OnRecord_Failed" treenode="632935022220433469" />
      </items>
      <linkto id="632935022220433474" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="Filename">filename</rd>
      </Properties>
    </node>
    <node type="Action" id="632935022220433474" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="544" y="448">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="csharp">"Recording to filename: "+ filename</log>
      </Properties>
    </node>
    <node type="Variable" id="632935022220433473" name="filename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">filename</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayTone_Failed" startnode="632935022220433449" treenode="632935022220433450" appnode="632935022220433447" handlerfor="632935022220433446">
    <node type="Start" id="632935022220433449" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632935022220433475" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632935022220433475" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="637" y="348">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Complete" startnode="632935022220433463" treenode="632935022220433464" appnode="632935022220433461" handlerfor="632935022220433460">
    <node type="Start" id="632935022220433463" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632935022220433476" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632935022220433476" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="820" y="359">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Failed" startnode="632935022220433468" treenode="632935022220433469" appnode="632935022220433466" handlerfor="632935022220433465">
    <node type="Start" id="632935022220433468" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632935022220433477" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632935022220433477" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="852" y="358">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" activetab="true" startnode="632935022220433481" treenode="632935022220433482" appnode="632935022220433479" handlerfor="632935022220433478">
    <node type="Start" id="632935022220433481" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="96" y="352">
      <linkto id="632935022220433483" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632935022220433483" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="906" y="356">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632935022220433484" text="Really the wav file would probably be stored off box somewhere later it could be referenced--maybe even published back into the CRM!&#xD;&#xA;&#xD;&#xA;Also probably makes sense to push a screen after the recording is done." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="176" y="448" />
  </canvas>
  <Properties desc="">
  </Properties>
</Application>