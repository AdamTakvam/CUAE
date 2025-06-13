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
          <ref id="633160355538281752" actid="632641046819961013" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632641046819961007" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632641046819961004" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632641046819961003" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="633160355538281753" actid="632641046819961013" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632641046819961012" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632641046819961009" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632641046819961008" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="633160355538281754" actid="632641046819961013" />
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
      <treenode text="IncomingCallId" id="633160355538281741" vid="632641046819961017">
        <Properties type="String">IncomingCallId</Properties>
      </treenode>
      <treenode text="OutgoingCallId" id="633160355538281743" vid="632641046819961019">
        <Properties type="String">OutgoingCallId</Properties>
      </treenode>
      <treenode text="g_ripOffDigits" id="633160355538281745" vid="632669879598485443">
        <Properties type="Int" initWith="rip">g_ripOffDigits</Properties>
      </treenode>
      <treenode text="g_to" id="633160355538281747" vid="632866700589172840">
        <Properties type="String" defaultInitWith="NONE" initWith="to">g_to</Properties>
      </treenode>
      <treenode text="g_useConfigDN" id="633160355538281749" vid="632866700589173493">
        <Properties type="Bool" defaultInitWith="false" initWith="useConfigDN">g_useConfigDN</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632641046819960993" treenode="632641046819960994" appnode="632641046819960991" handlerfor="632641046819960990">
    <node type="Start" id="632641046819960993" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="172">
      <linkto id="632641046819961025" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632641046819961013" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="296" y="156" mx="362" my="172">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632641046819961002" />
        <item text="OnMakeCall_Failed" treenode="632641046819961007" />
        <item text="OnRemoteHangup" treenode="632641046819961012" />
      </items>
      <linkto id="632641046819961021" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="633160355538281783" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_to</ap>
        <ap name="From" type="variable">From</ap>
        <ap name="Conference" type="literal">True</ap>
        <ap name="ConferenceId" type="literal">0</ap>
        <ap name="Hairpin" type="literal">true</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">OutgoingCallId</rd>
        <log condition="default" on="true" level="Warning" type="literal">MakeCall failed to proceed.</log>
      </Properties>
    </node>
    <node type="Action" id="632641046819961021" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="662" y="173">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632641046819961025" name="AcceptCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="124" y="172">
      <linkto id="632866700589172844" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">CallId</ap>
        <rd field="CallId">IncomingCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632668258134564088" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="360" y="438">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632866700589172844" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="235" y="172">
      <linkto id="632641046819961013" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(ref string g_to, bool g_useConfigDN, string OriginalTo, int g_ripOffDigits, LogWriter log)
{	
  // 6/23/06 Warren Yetman| Add to provide flexability when testing SMA-1150 
  if (!g_useConfigDN){
    g_to=OriginalTo.Substring(g_ripOffDigits);
  }
  return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="633160355538281783" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="360" y="323">
      <linkto id="632668258134564088" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">IncomingCallId</ap>
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
  <canvas type="Function" name="OnMakeCall_Complete" activetab="true" startnode="632641046819961001" treenode="632641046819961002" appnode="632641046819960999" handlerfor="632641046819960998">
    <node type="Start" id="632641046819961001" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="69" y="200">
      <linkto id="632641046819961024" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632641046819961024" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="324" y="200">
      <linkto id="632641046819961026" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="633160355538281784" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">IncomingCallId</ap>
        <ap name="Conference" type="literal">True</ap>
        <ap name="ConferenceId" type="variable">ConferenceId</ap>
        <log condition="default" on="true" level="Warning" type="literal">AnswerCall failed.</log>
      </Properties>
    </node>
    <node type="Action" id="632641046819961026" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="577" y="200">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632668258134564089" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="324" y="400">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633160355538281784" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="324" y="295">
      <linkto id="632668258134564089" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">OutgoingCallId</ap>
      </Properties>
    </node>
    <node type="Variable" id="632641046819961023" name="ConferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConferenceId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">ConferenceId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632641046819961006" treenode="632641046819961007" appnode="632641046819961004" handlerfor="632641046819961003">
    <node type="Start" id="632641046819961006" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="51" y="168">
      <linkto id="632641046819961027" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632641046819961027" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="162" y="168">
      <linkto id="632641046819961028" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">IncomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632641046819961028" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="330" y="168">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632641046819961011" treenode="632641046819961012" appnode="632641046819961009" handlerfor="632641046819961008">
    <node type="Start" id="632641046819961011" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="55" y="188">
      <linkto id="632641046819961030" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632641046819961030" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="268" y="188">
      <linkto id="632641046819961031" type="Labeled" style="Bevel" ortho="true" label="equal" />
      <linkto id="632641046819961032" type="Labeled" style="Bevel" ortho="true" label="unequal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">IncomingCallId</ap>
        <ap name="Value2" type="variable">CallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632641046819961031" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="438" y="188">
      <linkto id="632641046819961033" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">OutgoingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632641046819961032" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="268.000031" y="328">
      <linkto id="632641046819961033" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">IncomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632641046819961033" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="438" y="328">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632641046819961029" name="CallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">CallId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632641277348658414" treenode="632641277348658415" appnode="632641277348658412" handlerfor="632641277348658411">
    <node type="Start" id="632641277348658414" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="52" y="207">
      <linkto id="632641277348658421" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632641277348658421" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="247" y="207">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632641277348658419" treenode="632641277348658420" appnode="632641277348658417" handlerfor="632641277348658416">
    <node type="Start" id="632641277348658419" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="74" y="218">
      <linkto id="632641277348658422" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632641277348658422" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="224" y="218">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632641277348658426" treenode="632641277348658427" appnode="632641277348658424" handlerfor="632641277348658423">
    <node type="Start" id="632641277348658426" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="77" y="228">
      <linkto id="632641277348658428" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632641277348658428" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="198" y="228">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>