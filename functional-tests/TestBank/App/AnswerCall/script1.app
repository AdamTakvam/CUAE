<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632651831894530060" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632651831894530057" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632651831894530056" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632651831894530074" level="2" text="Metreos.Providers.FunctionalTest.Event: OnEvent">
        <node type="function" name="OnEvent" id="632651831894530071" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632651831894530070" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">App.AnswerCall.script1.E_AnswerCallAck</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632651831894530088" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632651831894530085" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632651831894530084" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632651848548273328" level="2" text="Metreos.Providers.FunctionalTest.Event: OnEvent1">
        <node type="function" name="OnEvent1" id="632651848548273325" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632651848548273324" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">App.AnswerCall.script1.E_Hangup</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632694788302799860" level="2" text="Metreos.Providers.FunctionalTest.Event: OnEvent2">
        <node type="function" name="OnEvent2" id="632694788302799857" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632694788302799856" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">App.AnswerCall.script1.E_SendConfirmDigit</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_IncomingCall" id="632705195481131423" vid="632651831894530067">
        <Properties type="String" initWith="S_IncomingCall">S_IncomingCall</Properties>
      </treenode>
      <treenode text="S_Hangup" id="632705195481131425" vid="632651831894530091">
        <Properties type="String" initWith="S_Hangup">S_Hangup</Properties>
      </treenode>
      <treenode text="S_AnswerCall" id="632705195481131427" vid="632651851430591912">
        <Properties type="String" initWith="S_AnswerCall">S_AnswerCall</Properties>
      </treenode>
      <treenode text="g_callId" id="632705195481131429" vid="632694788302799800">
        <Properties type="String">g_callId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632651831894530059" treenode="632651831894530060" appnode="632651831894530057" handlerfor="632651831894530056">
    <node type="Start" id="632651831894530059" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="366">
      <linkto id="632697499562687509" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632651831894530066" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="303" y="367">
      <linkto id="632651831894530069" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="from" type="variable">from</ap>
        <ap name="to" type="variable">to</ap>
        <ap name="originalTo" type="variable">originalTo</ap>
        <ap name="callId" type="variable">callId</ap>
        <ap name="signalName" type="variable">S_IncomingCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632651831894530069" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="524" y="365">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632697499562687509" name="AcceptCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="149" y="369">
      <linkto id="632651831894530066" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <rd field="CallId">g_callId</rd>
      </Properties>
    </node>
    <node type="Variable" id="632651831894530061" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" defaultInitWith="NONE" refType="reference" name="Metreos.CallControl.IncomingCall">from</Properties>
    </node>
    <node type="Variable" id="632651831894530062" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to" defaultInitWith="NONE" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632651831894530063" name="originalTo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="OriginalTo" defaultInitWith="NONE" refType="reference" name="Metreos.CallControl.IncomingCall">originalTo</Properties>
    </node>
    <node type="Variable" id="632651831894530064" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent" startnode="632651831894530073" treenode="632651831894530074" appnode="632651831894530071" handlerfor="632651831894530070">
    <node type="Start" id="632651831894530073" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="355">
      <linkto id="632651831894530077" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632651831894530077" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="199" y="357">
      <linkto id="632651851430591911" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632651851430591914" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Comment" id="632651831894530082" text="Simulates when user enters '#'&#xD;&#xA;to confirm that they want to accept&#xD;&#xA;the call on this device." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="468" y="243" />
    <node type="Action" id="632651831894530083" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="746" y="354">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632651851430591910" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="198" y="679">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632651851430591911" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="463" y="358">
      <linkto id="632651831894530083" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="callId" type="variable">g_callId</ap>
        <ap name="signalName" type="variable">S_AnswerCall</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"ANSWERED CALL=" + g_callId</log>
      </Properties>
    </node>
    <node type="Action" id="632651851430591914" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="204" y="548">
      <linkto id="632651851430591910" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="callId" type="variable">g_callId</ap>
        <ap name="signalName" type="variable">S_AnswerCall</ap>
        <log condition="entry" on="true" level="Error" type="csharp">"ANSWER CALL FAILED=" + g_callId</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632651831894530087" treenode="632651831894530088" appnode="632651831894530085" handlerfor="632651831894530084">
    <node type="Start" id="632651831894530087" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="327">
      <linkto id="632651831894530090" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632651831894530090" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="264" y="329">
      <linkto id="632651831894530093" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="callId" type="variable">callId</ap>
        <ap name="signalName" type="variable">S_Hangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632651831894530093" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="477" y="331">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632694788302799802" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent1" startnode="632651848548273327" treenode="632651848548273328" appnode="632651848548273325" handlerfor="632651848548273324">
    <node type="Start" id="632651848548273327" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="333">
      <linkto id="632651848548273330" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632651848548273330" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="269" y="333">
      <linkto id="632651848548273331" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632651848548273331" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="455" y="329">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent2" activetab="true" startnode="632694788302799859" treenode="632694788302799860" appnode="632694788302799857" handlerfor="632694788302799856">
    <node type="Start" id="632694788302799859" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="341">
      <linkto id="632694788302799861" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632694788302799861" name="SendUserInput" class="MaxActionNode" group="" path="Metreos.CallControl" x="222.472656" y="337">
      <linkto id="632694788302799863" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
        <ap name="Digits" type="literal">#</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"CONFIRMING CALL=" + g_callId</log>
      </Properties>
    </node>
    <node type="Action" id="632694788302799863" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="362" y="335">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>