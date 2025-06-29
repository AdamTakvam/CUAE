<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632150685716406381" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632150685716406379" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632150685716406378" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.RecordUtility.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632150685716406390" level="2" text="MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632150685716406388" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632150685716406387" path="Metreos.CallControl.MakeCall_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632150685716406411" level="3" text="RecordAudio_Complete: OnRecordAudio_Complete">
        <node type="function" name="OnRecordAudio_Complete" id="632150685716406409" path="Metreos.StockTools" />
        <node type="event" name="RecordAudio_Complete" id="632150685716406408" path="Metreos.Providers.MediaServer.RecordAudio_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632150685716406530" level="4" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632150685716406528" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632150685716406527" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632150685716406534" level="4" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632150685716406532" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632150685716406531" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632150685716406415" level="3" text="RecordAudio_Failed: OnRecordAudio_Failed">
        <node type="function" name="OnRecordAudio_Failed" id="632150685716406413" path="Metreos.StockTools" />
        <node type="event" name="RecordAudio_Failed" id="632150685716406412" path="Metreos.Providers.MediaServer.RecordAudio_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632150685716406539" level="4" text="Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632150685716406528" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632150685716406527" path="Metreos.CallControl.Hangup_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632150685716406540" level="4" text="Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632150685716406532" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632150685716406531" path="Metreos.CallControl.Hangup_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632150685716406394" level="2" text="MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632150685716406392" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632150685716406391" path="Metreos.CallControl.MakeCall_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632150685716406398" level="2" text="Hangup: OnHangup">
        <node type="function" name="OnHangup" id="632150685716406396" path="Metreos.StockTools" />
        <node type="event" name="Hangup" id="632150685716406395" path="Metreos.CallControl.Hangup" />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632150685716406431" level="1" text="AssignIncomingVars">
        <node type="function" name="AssignIncomingVars" id="632150685716406429" path="Metreos.StockTools" />
      </treenode>
    </outline>
    <variables>
      <treenode text="connectionId" id="632224881663907340" vid="632150685716406382">
        <Properties type="Metreos.Types.Int">connectionId</Properties>
      </treenode>
      <treenode text="recordTime" id="632224881663907342" vid="632150685716406423">
        <Properties type="Metreos.Types.Int">recordTime</Properties>
      </treenode>
      <treenode text="recordFilename" id="632224881663907344" vid="632150685716406425">
        <Properties type="Metreos.Types.String">recordFilename</Properties>
      </treenode>
      <treenode text="callId" id="632224881663907346" vid="632150685716406536">
        <Properties type="Metreos.Types.String">callId</Properties>
      </treenode>
      <treenode text="S_MakeCallFailed" id="632224881663907348" vid="632150685716406545">
        <Properties type="Metreos.Types.String" initWith="S_MakeCallFailed">S_MakeCallFailed</Properties>
      </treenode>
      <treenode text="S_ExternalHangup" id="632224881663907350" vid="632150685716406547">
        <Properties type="Metreos.Types.String" initWith="S_ExternalHangup">S_ExternalHangup</Properties>
      </treenode>
      <treenode text="S_RecordSucceeded" id="632224881663907352" vid="632150685716406549">
        <Properties type="Metreos.Types.String" initWith="S_RecordSucceeded">S_RecordSucceeded</Properties>
      </treenode>
      <treenode text="S_RecordFailed" id="632224881663907354" vid="632150685716406555">
        <Properties type="Metreos.Types.String" initWith="S_RecordFailed">S_RecordFailed</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" varsy="668" startnode="632150685716406380" treenode="632150685716406381" appnode="632150685716406379" handlerfor="632150685716406378">
    <node type="Start" id="632150685716406380" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="193">
      <linkto id="632150685716406428" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150685716406384" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="286" y="188">
      <linkto id="632150685716406399" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="literal">0</ap>
        <ap name="connectionId" type="literal">0</ap>
        <ap name="remoteIp" type="literal">0</ap>
        <rd field="connectionId">connectionId</rd>
        <rd field="port">mediaPort</rd>
        <rd field="ipAddress">mediaIp</rd>
      </Properties>
    </node>
    <node type="Variable" id="632150685716406385" name="mediaIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="686">
      <Properties type="Metreos.Types.String" refType="reference">mediaIp</Properties>
    </node>
    <node type="Variable" id="632150685716406386" name="mediaPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="98.5130157" y="686">
      <Properties type="Metreos.Types.Int" refType="reference">mediaPort</Properties>
    </node>
    <node type="Action" id="632150685716406399" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="381" y="171" mx="447" my="187">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632150685716406390" />
        <item text="OnMakeCall_Failed" treenode="632150685716406394" />
        <item text="OnHangup" treenode="632150685716406398" />
      </items>
      <linkto id="632224881663907414" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="from" type="literal">Record Utility</ap>
        <ap name="mediaPort" type="variable">mediaPort</ap>
        <ap name="to" type="variable">to</ap>
        <ap name="mediaIP" type="variable">mediaIp</ap>
        <ap name="userData" type="literal">none</ap>
        <rd field="callId">callId</rd>
      </Properties>
    </node>
    <node type="Variable" id="632150685716406400" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="193.359375" y="686">
      <Properties type="Metreos.Types.String" initWith="to" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632150685716406401" name="duration" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="238.479172" y="686">
      <Properties type="Metreos.Types.String" initWith="duration" refType="reference">duration</Properties>
    </node>
    <node type="Variable" id="632150685716406427" name="filename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="320.7409" y="686">
      <Properties type="Metreos.Types.String" initWith="filename" refType="reference">filename</Properties>
    </node>
    <node type="Action" id="632150685716406428" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="94.23535" y="174" mx="153" my="190">
      <items count="1">
        <item text="AssignIncomingVars" treenode="632150685716406381" />
      </items>
      <linkto id="632150685716406384" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="incomingDuration" type="variable">duration</ap>
        <ap name="incomingFilename" type="variable">filename</ap>
        <ap name="FunctionName" type="literal">AssignIncomingVars</ap>
        <rd field="outgoingDuration">recordTime</rd>
        <rd field="outgoingFilename">recordFilename</rd>
      </Properties>
    </node>
    <node type="Action" id="632224881663907414" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="608" y="174">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" varsy="667" startnode="632150685716406389" treenode="632150685716406390" appnode="632150685716406388" handlerfor="632150685716406387">
    <node type="Start" id="632150685716406389" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632150685716406403" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150685716406403" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="198" y="275">
      <linkto id="632150685716406406" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="variable">mediaPort</ap>
        <ap name="connectionId" type="variable">connectionId</ap>
        <ap name="remoteIp" type="variable">mediaIP</ap>
      </Properties>
    </node>
    <node type="Variable" id="632150685716406404" name="mediaIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="685">
      <Properties type="Metreos.Types.String" initWith="mediaIP" refType="reference">mediaIP</Properties>
    </node>
    <node type="Variable" id="632150685716406405" name="mediaPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="99.99088" y="685">
      <Properties type="Metreos.Types.Int" initWith="mediaPort" refType="reference">mediaPort</Properties>
    </node>
    <node type="Action" id="632150685716406406" name="SetMedia" class="MaxActionNode" group="" path="Metreos.CallControl" x="379" y="276">
      <linkto id="632150685716406416" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="mediaPort" type="variable">mediaPort</ap>
        <ap name="mediaIP" type="variable">mediaIP</ap>
        <ap name="callId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632150685716406416" name="RecordAudio" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="470" y="261" mx="546" my="277">
      <items count="2">
        <item text="OnRecordAudio_Complete" treenode="632150685716406411" />
        <item text="OnRecordAudio_Failed" treenode="632150685716406415" />
      </items>
      <linkto id="632224881663907415" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondNonSilence" type="literal">999999999999</ap>
        <ap name="termCondMaxTime" type="variable">recordTime</ap>
        <ap name="termCondSilence" type="literal">999999999999</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="connectionId" type="variable">connectionId</ap>
        <ap name="filename" type="variable">recordFilename</ap>
        <ap name="expires" type="literal">365</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Record filename: " + recordFilename + ", recordTime: " + recordTime</log>
      </Properties>
    </node>
    <node type="Action" id="632224881663907415" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="654" y="276">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecordAudio_Complete" varsy="692" startnode="632150685716406410" treenode="632150685716406411" appnode="632150685716406409" handlerfor="632150685716406408">
    <node type="Start" id="632150685716406410" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="194">
      <linkto id="632150685716406526" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150685716406526" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="304" y="226">
      <linkto id="632150685716406535" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632150685716406535" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="446" y="209" mx="508" my="225">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632150685716406530" />
        <item text="OnHangup_Failed" treenode="632150685716406534" />
      </items>
      <linkto id="632224881663907416" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">callId</ap>
        <ap name="userData" type="literal">success</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907416" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="613" y="221">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup_Complete" varsy="667" startnode="632150685716406529" treenode="632150685716406530" appnode="632150685716406528" handlerfor="632150685716406527">
    <node type="Start" id="632150685716406529" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632150685716406551" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150685716406551" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="129" y="234">
      <linkto id="632150685716406554" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <linkto id="632150685716406553" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native">
        <ap name="switchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Variable" id="632150685716406552" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="685">
      <Properties type="Metreos.Types.String" initWith="userData" refType="reference">userData</Properties>
    </node>
    <node type="Action" id="632150685716406553" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="317" y="177">
      <linkto id="632224881663907417" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_RecordSucceeded</ap>
      </Properties>
    </node>
    <node type="Action" id="632150685716406554" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="319" y="321">
      <linkto id="632224881663907417" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_RecordFailed</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907417" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="451" y="235">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup_Failed" varsy="667" startnode="632150685716406533" treenode="632150685716406534" appnode="632150685716406532" handlerfor="632150685716406531">
    <node type="Start" id="632150685716406533" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632150685716406558" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Variable" id="632150685716406557" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="685">
      <Properties type="Metreos.Types.String" initWith="userData" refType="reference">userData</Properties>
    </node>
    <node type="Action" id="632150685716406558" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="143.000015" y="266">
      <linkto id="632150685716406560" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <linkto id="632150685716406559" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native">
        <ap name="switchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632150685716406559" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="390" y="177">
      <linkto id="632224881663907418" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_RecordSucceeded</ap>
      </Properties>
    </node>
    <node type="Action" id="632150685716406560" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="394" y="334">
      <linkto id="632224881663907418" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_RecordFailed</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907418" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="564" y="258">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecordAudio_Failed" varsy="692" startnode="632150685716406414" treenode="632150685716406415" appnode="632150685716406413" handlerfor="632150685716406412">
    <node type="Start" id="632150685716406414" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632150685716406541" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150685716406541" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="340" y="111" mx="402" my="127">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632150685716406539" />
        <item text="OnHangup_Failed" treenode="632150685716406540" />
      </items>
      <linkto id="632224881663907419" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">callId</ap>
        <ap name="userData" type="literal">failure</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907419" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="616" y="125">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" varsy="692" startnode="632150685716406393" treenode="632150685716406394" appnode="632150685716406392" handlerfor="632150685716406391">
    <node type="Start" id="632150685716406393" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632150685716406562" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150685716406417" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="299" y="320">
      <linkto id="632224881663907420" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632150685716406562" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="227" y="177">
      <linkto id="632150685716406417" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_MakeCallFailed</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907420" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="448" y="301">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup" varsy="716" startnode="632150685716406397" treenode="632150685716406398" appnode="632150685716406396" handlerfor="632150685716406395">
    <node type="Start" id="632150685716406397" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632150685716406561" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150685716406419" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="439" y="231">
      <linkto id="632150685716406420" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632150685716406420" name="Sleep" class="MaxActionNode" group="" path="Metreos.Native.ApplicationControl" x="441" y="388">
      <linkto id="632150685716406421" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="timeout" type="literal">500</ap>
      </Properties>
    </node>
    <node type="Action" id="632150685716406421" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="441" y="520">
      <linkto id="632224881663907421" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632150685716406561" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="338" y="183">
      <linkto id="632150685716406419" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_ExternalHangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907421" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="420" y="649">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="AssignIncomingVars" activetab="true" varsy="667" startnode="632150685716406430" treenode="632150685716406431" appnode="632150685716406429" handlerfor="632150685716406395">
    <node type="Start" id="632150685716406430" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224881663907422" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Variable" id="632150685716406432" name="durationAssignee" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="685">
      <Properties type="Metreos.Types.String" initWith="incomingDuration" refType="reference">durationAssignee</Properties>
    </node>
    <node type="Variable" id="632150685716406433" name="filenameAssignee" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="154.1901" y="685">
      <Properties type="Metreos.Types.String" initWith="incomingFilename" refType="reference">filenameAssignee</Properties>
    </node>
    <node type="Action" id="632224881663907422" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="284" y="196">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
