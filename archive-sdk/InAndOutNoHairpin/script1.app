<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632641046819960994" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632641046819960991" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632641046819960990" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632641046819961002" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632641046819960999" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632641046819960998" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632669883389363705" actid="632641046819961013" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632641046819961007" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632641046819961004" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632641046819961003" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632669883389363706" actid="632641046819961013" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632641046819961012" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632641046819961009" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632641046819961008" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632669883389363707" actid="632641046819961013" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632641277348658415" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632641277348658412" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632641277348658411" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632641277348658420" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632641277348658417" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632641277348658416" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632641277348658427" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632641277348658424" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632641277348658423" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="IncomingCallId" id="632669883389363700" vid="632641046819961017">
        <Properties type="String">IncomingCallId</Properties>
      </treenode>
      <treenode text="OutgoingCallId" id="632669883389363702" vid="632641046819961019">
        <Properties type="String">OutgoingCallId</Properties>
      </treenode>
      <treenode text="g_ripOffDigits" id="632669883389363734" vid="632669883389363733">
        <Properties type="Int" initWith="rip">g_ripOffDigits</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632641046819960993" treenode="632641046819960994" appnode="632641046819960991" handlerfor="632641046819960990">
    <node type="Start" id="632641046819960993" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632641046819961025" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632641046819961013" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="296" y="171" mx="362" my="187">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632641046819961002" />
        <item text="OnMakeCall_Failed" treenode="632641046819961007" />
        <item text="OnRemoteHangup" treenode="632641046819961012" />
      </items>
      <linkto id="632641046819961021" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="csharp">OriginalTo.Substring(g_ripOffDigits)</ap>
        <ap name="From" type="variable">From</ap>
        <ap name="Conference" type="literal">True</ap>
        <ap name="ConferenceId" type="literal">0</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">OutgoingCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632641046819961021" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="534" y="329">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632641046819961025" name="AcceptCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="185" y="85">
      <linkto id="632641046819961013" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">CallId</ap>
        <rd field="CallId">IncomingCallId</rd>
      </Properties>
    </node>
    <node type="Variable" id="632641046819960995" name="OriginalTo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="OriginalTo" refType="reference" name="Metreos.CallControl.IncomingCall">OriginalTo</Properties>
    </node>
    <node type="Variable" id="632641046819960996" name="From" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">From</Properties>
    </node>
    <node type="Variable" id="632641046819960997" name="CallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">CallId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632641046819961001" treenode="632641046819961002" appnode="632641046819960999" handlerfor="632641046819960998">
    <node type="Start" id="632641046819961001" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632641046819961024" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632641046819961024" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="272" y="146">
      <linkto id="632641046819961026" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">IncomingCallId</ap>
        <ap name="Conference" type="literal">True</ap>
        <ap name="ConferenceId" type="variable">ConferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632641046819961026" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="500" y="335">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632641046819961022" name="ConnectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">ConnectionId</Properties>
    </node>
    <node type="Variable" id="632641046819961023" name="ConferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConferenceId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">ConferenceId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632641046819961006" treenode="632641046819961007" appnode="632641046819961004" handlerfor="632641046819961003">
    <node type="Start" id="632641046819961006" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632641046819961027" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632641046819961027" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="273" y="168">
      <linkto id="632641046819961028" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">IncomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632641046819961028" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="513" y="329">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632641046819961011" treenode="632641046819961012" appnode="632641046819961009" handlerfor="632641046819961008">
    <node type="Start" id="632641046819961011" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632641046819961030" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632641046819961030" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="268" y="188">
      <linkto id="632641046819961031" type="Labeled" style="Bevel" label="equal" />
      <linkto id="632641046819961032" type="Labeled" style="Bevel" label="unequal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">IncomingCallId</ap>
        <ap name="Value2" type="variable">CallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632641046819961031" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="469" y="232">
      <linkto id="632641046819961033" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">OutgoingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632641046819961032" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="402.000031" y="323">
      <linkto id="632641046819961033" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">IncomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632641046819961033" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="659" y="378">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632641046819961029" name="CallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">CallId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632641277348658414" treenode="632641277348658415" appnode="632641277348658412" handlerfor="632641277348658411">
    <node type="Start" id="632641277348658414" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632641277348658421" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632641277348658421" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="341" y="268">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632641277348658419" treenode="632641277348658420" appnode="632641277348658417" handlerfor="632641277348658416">
    <node type="Start" id="632641277348658419" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632641277348658422" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632641277348658422" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="338" y="233">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632641277348658426" treenode="632641277348658427" appnode="632641277348658424" handlerfor="632641277348658423">
    <node type="Start" id="632641277348658426" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632641277348658428" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632641277348658428" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="330" y="240">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>