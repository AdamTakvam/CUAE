<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632514071165499124" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632514071165499121" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632514071165499120" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">IVT.MakeCall.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632518396957741859" level="2" text="Metreos.Providers.FunctionalTest.Event: OnEvent">
        <node type="function" name="OnEvent" id="632518396957741856" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632518396957741855" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.MakeCall.script1.E_Hangup</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632521168379182401" level="2" text="Metreos.Providers.FunctionalTest.Event: OnEvent3">
        <node type="function" name="OnEvent3" id="632521168379182398" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632521168379182397" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.MakeCall.script1.E_PlayShort</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632521168379182532" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632521168379182529" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632521168379182528" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632526410749739639" level="2" text="Metreos.Providers.FunctionalTest.Event: OnEvent4">
        <node type="function" name="OnEvent4" id="632526410749739636" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632526410749739635" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.MakeCall.script1.E_PlayTone</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632526410749739647" level="2" text="Metreos.MediaControl.PlayTone_Complete: OnPlayTone_Complete">
        <node type="function" name="OnPlayTone_Complete" id="632526410749739644" path="Metreos.StockTools" />
        <node type="event" name="PlayTone_Complete" id="632526410749739643" path="Metreos.MediaControl.PlayTone_Complete" />
        <references>
          <ref id="632583556188893289" actid="632526410749739653" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632526410749739652" level="2" text="Metreos.MediaControl.PlayTone_Failed: OnPlayTone_Failed">
        <node type="function" name="OnPlayTone_Failed" id="632526410749739649" path="Metreos.StockTools" />
        <node type="event" name="PlayTone_Failed" id="632526410749739648" path="Metreos.MediaControl.PlayTone_Failed" />
        <references>
          <ref id="632583556188893290" actid="632526410749739653" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632527174907346517" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632527174907346514" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632527174907346513" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632517847152576350" level="1" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632517847152576347" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632517847152576346" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632583556188893267" actid="632517847152576361" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632517847152576355" level="1" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632517847152576352" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632517847152576351" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632583556188893268" actid="632517847152576361" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632517847152576360" level="1" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632517847152576357" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632517847152576356" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632583556188893269" actid="632517847152576361" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632521140802621581" level="1" text="Metreos.Providers.FunctionalTest.Event: OnEvent1">
        <node type="function" name="OnEvent1" id="632521140802621578" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632521140802621577" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.MakeCall.script1.E_Play</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632521140802621656" level="1" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632521140802621653" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632521140802621652" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632583556188893281" actid="632521168379182524" />
          <ref id="632583556188893304" actid="632527174907346520" />
          <ref id="632583556188893321" actid="632521140802621662" />
          <ref id="632583556188893341" actid="632521153359450447" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632521140802621661" level="1" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632521140802621658" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632521140802621657" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632583556188893282" actid="632521168379182524" />
          <ref id="632583556188893305" actid="632527174907346520" />
          <ref id="632583556188893322" actid="632521140802621662" />
          <ref id="632583556188893342" actid="632521153359450447" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632521153359450426" level="1" text="Metreos.Providers.FunctionalTest.Event: OnEvent2">
        <node type="function" name="OnEvent2" id="632521153359450423" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632521153359450422" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.MakeCall.script1.E_Record</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632521153359450431" level="1" text="Metreos.MediaControl.Record_Complete: OnRecord_Complete">
        <node type="function" name="OnRecord_Complete" id="632521153359450428" path="Metreos.StockTools" />
        <node type="event" name="Record_Complete" id="632521153359450427" path="Metreos.MediaControl.Record_Complete" />
        <references>
          <ref id="632583556188893334" actid="632521153359450437" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632521153359450436" level="1" text="Metreos.MediaControl.Record_Failed: OnRecord_Failed">
        <node type="function" name="OnRecord_Failed" id="632521153359450433" path="Metreos.StockTools" />
        <node type="event" name="Record_Failed" id="632521153359450432" path="Metreos.MediaControl.Record_Failed" />
        <references>
          <ref id="632583556188893335" actid="632521153359450437" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Trigger" id="632583556188893246" vid="632517847152576341">
        <Properties type="String" initWith="S_Trigger">S_Trigger</Properties>
      </treenode>
      <treenode text="S_MakeCallComplete" id="632583556188893248" vid="632517847152576343">
        <Properties type="String" initWith="S_MakeCallComplete">S_MakeCallComplete</Properties>
      </treenode>
      <treenode text="S_Hangup" id="632583556188893250" vid="632517886269824294">
        <Properties type="String" initWith="S_Hangup">S_Hangup</Properties>
      </treenode>
      <treenode text="g_callId" id="632583556188893252" vid="632518396957741861">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632583556188893254" vid="632521140802621648">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="S_Play" id="632583556188893256" vid="632521151170421716">
        <Properties type="String" initWith="S_Play">S_Play</Properties>
      </treenode>
      <treenode text="S_Record" id="632583556188893258" vid="632521153359450444">
        <Properties type="String" initWith="S_Record">S_Record</Properties>
      </treenode>
      <treenode text="S_PlayShort" id="632583556188893260" vid="632521168379182522">
        <Properties type="String" initWith="S_PlayShort">S_PlayShort</Properties>
      </treenode>
      <treenode text="g_isHoldXferTest" id="632583556188893262" vid="632521168379182533">
        <Properties type="Bool" defaultInitWith="false">g_isHoldXferTest</Properties>
      </treenode>
      <treenode text="S_PlayTone" id="632583556188893264" vid="632526410749739659">
        <Properties type="String" initWith="S_PlayTone">S_PlayTone</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632514071165499123" treenode="632514071165499124" appnode="632514071165499121" handlerfor="632514071165499120">
    <node type="Start" id="632514071165499123" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="376">
      <linkto id="632517847152576361" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632517847152576361" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="328" y="360" mx="394" my="376">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632517847152576350" />
        <item text="OnMakeCall_Failed" treenode="632517847152576355" />
        <item text="OnRemoteHangup" treenode="632517847152576360" />
      </items>
      <linkto id="632521165193939584" type="Labeled" style="Bevel" label="Success" />
      <linkto id="632521165193939585" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">to</ap>
        <ap name="From" type="variable">from</ap>
        <ap name="DisplayName" type="variable">from</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_callId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"To: " + to</log>
      </Properties>
    </node>
    <node type="Action" id="632517847152576365" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="768" y="376">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632517857388795366" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="392" y="712">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632521165193939584" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="595" y="373">
      <linkto id="632517847152576365" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_Trigger</ap>
      </Properties>
    </node>
    <node type="Action" id="632521165193939585" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="392" y="551">
      <linkto id="632517857388795366" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_Trigger</ap>
      </Properties>
    </node>
    <node type="Variable" id="632517847152576345" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632520237582317259" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="from" defaultInitWith="Test Framework" refType="reference">from</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent" startnode="632518396957741858" treenode="632518396957741859" appnode="632518396957741856" handlerfor="632518396957741855">
    <node type="Start" id="632518396957741858" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="45" y="221">
      <linkto id="632518396957741860" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632518396957741860" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="234" y="220">
      <linkto id="632518396957741863" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632518396957741863" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="473" y="223">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent3" startnode="632521168379182400" treenode="632521168379182401" appnode="632521168379182398" handlerfor="632521168379182397">
    <node type="Start" id="632521168379182400" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="360">
      <linkto id="632521168379182535" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632521168379182524" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="256" y="344" mx="309" my="360">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632521140802621656" />
        <item text="OnPlay_Failed" treenode="632521140802621661" />
      </items>
      <linkto id="632521168379182536" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxTime" type="literal">120000</ap>
        <ap name="Prompt1" type="literal">vt_thank_you.wav</ap>
        <ap name="CommandTimeout" type="literal">120000</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632521168379182535" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="158" y="361">
      <linkto id="632521168379182524" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <rd field="ResultData">g_isHoldXferTest</rd>
      </Properties>
    </node>
    <node type="Action" id="632521168379182536" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="560" y="368">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632521168379182531" treenode="632521168379182532" appnode="632521168379182529" handlerfor="632521168379182528">
    <node type="Start" id="632521168379182531" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="368">
      <linkto id="632521168379182548" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632521168379182548" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="604" y="371">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent4" activetab="true" startnode="632526410749739638" treenode="632526410749739639" appnode="632526410749739636" handlerfor="632526410749739635">
    <node type="Start" id="632526410749739638" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="353">
      <linkto id="632526410749739653" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632526410749739653" name="PlayTone" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="257" y="336" mx="323" my="352">
      <items count="2">
        <item text="OnPlayTone_Complete" treenode="632526410749739647" />
        <item text="OnPlayTone_Failed" treenode="632526410749739652" />
      </items>
      <linkto id="632526410749739656" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Frequency1" type="literal">200</ap>
        <ap name="TermCondMaxTime" type="literal">610000</ap>
        <ap name="Amplitude2" type="literal">-20</ap>
        <ap name="Frequency2" type="literal">200</ap>
        <ap name="Amplitude1" type="literal">-20</ap>
        <ap name="Duration" type="literal">60000</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632526410749739656" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="564" y="350">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayTone_Complete" startnode="632526410749739646" treenode="632526410749739647" appnode="632526410749739644" handlerfor="632526410749739643">
    <node type="Start" id="632526410749739646" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="58" y="287">
      <linkto id="632526410749739665" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632526410749739658" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="331" y="290">
      <linkto id="632526410749739664" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_PlayTone</ap>
      </Properties>
    </node>
    <node type="Action" id="632526410749739664" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="517" y="287">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632526410749739665" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="174" y="287">
      <linkto id="632526410749739658" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayTone_Failed" startnode="632526410749739651" treenode="632526410749739652" appnode="632526410749739649" handlerfor="632526410749739648">
    <node type="Start" id="632526410749739651" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="309">
      <linkto id="632526410749739666" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632526410749739661" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="227.610016" y="310">
      <linkto id="632526410749739663" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_PlayTone</ap>
      </Properties>
    </node>
    <node type="Action" id="632526410749739663" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="398" y="312">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632526410749739666" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="127" y="308">
      <linkto id="632526410749739661" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632527174907346516" treenode="632527174907346517" appnode="632527174907346514" handlerfor="632527174907346513">
    <node type="Start" id="632527174907346516" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632527174907346518" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632527174907346518" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="220.4707" y="254">
      <linkto id="632527174907346519" type="Labeled" style="Bevel" label="false" />
      <linkto id="632527174907346520" type="Labeled" style="Bevel" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_isHoldXferTest</ap>
      </Properties>
    </node>
    <node type="Action" id="632527174907346519" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="220.4707" y="382">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632527174907346520" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="372.4707" y="238" mx="425" my="254">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632521140802621656" />
        <item text="OnPlay_Failed" treenode="632521140802621661" />
      </items>
      <linkto id="632527174907346521" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">vt_thank_you.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632527174907346521" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="584.4707" y="257">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632517847152576349" treenode="632517847152576350" appnode="632517847152576347" handlerfor="632517847152576346">
    <node type="Start" id="632517847152576349" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="184">
      <linkto id="632521140802621651" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632517857388795373" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="528" y="184">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632521140802621651" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="165" y="186">
      <linkto id="632521165193939587" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">connectionId</ap>
        <rd field="ResultData">g_connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632521165193939587" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="337" y="188">
      <linkto id="632517857388795373" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_MakeCallComplete</ap>
      </Properties>
    </node>
    <node type="Variable" id="632521140802621650" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632517847152576354" treenode="632517847152576355" appnode="632517847152576352" handlerfor="632517847152576351">
    <node type="Start" id="632517847152576354" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="232">
      <linkto id="632521165193939588" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632517857388795372" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="488" y="232">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632521165193939588" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="241" y="234">
      <linkto id="632517857388795372" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="reason" type="variable">reason</ap>
        <ap name="signalName" type="variable">S_MakeCallComplete</ap>
      </Properties>
    </node>
    <node type="Variable" id="632517857388795374" name="reason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">reason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632517847152576359" treenode="632517847152576360" appnode="632517847152576357" handlerfor="632517847152576356">
    <node type="Start" id="632517847152576359" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="241">
      <linkto id="632521168379182124" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632517857388795371" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="667" y="241">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632521168379182124" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="382" y="250">
      <linkto id="632517857388795371" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Hangup</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent1" startnode="632521140802621580" treenode="632521140802621581" appnode="632521140802621578" handlerfor="632521140802621577">
    <node type="Start" id="632521140802621580" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="329">
      <linkto id="632521140802621662" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632521140802621662" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="299" y="312" mx="352" my="328">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632521140802621656" />
        <item text="OnPlay_Failed" treenode="632521140802621661" />
      </items>
      <linkto id="632521151170421718" type="Labeled" style="Bevel" label="default" />
      <linkto id="632521151170421627" type="Labeled" style="Bevel" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxTime" type="literal">120000</ap>
        <ap name="Prompt1" type="literal">sample_music.wav</ap>
        <ap name="CommandTimeout" type="literal">120000</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632521140802621665" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="483" y="448">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632521151170421627" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="483" y="328">
      <linkto id="632521140802621665" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_Play</ap>
      </Properties>
    </node>
    <node type="Action" id="632521151170421718" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="350" y="448">
      <linkto id="632521140802621665" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_Play</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632521140802621655" treenode="632521140802621656" appnode="632521140802621653" handlerfor="632521140802621652">
    <node type="Start" id="632521140802621655" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="80" y="288">
      <linkto id="632521153359450451" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632521140802621666" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="603" y="287">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632521153359450451" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="316" y="286">
      <linkto id="632521140802621666" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_Play</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632521140802621660" treenode="632521140802621661" appnode="632521140802621658" handlerfor="632521140802621657">
    <node type="Start" id="632521140802621660" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="252">
      <linkto id="632521153359450453" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632521153359450452" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="404.610016" y="252">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632521153359450453" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="182.61" y="255">
      <linkto id="632521153359450452" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_Play</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent2" startnode="632521153359450425" treenode="632521153359450426" appnode="632521153359450423" handlerfor="632521153359450422">
    <node type="Start" id="632521153359450425" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="339">
      <linkto id="632521153359450437" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632521153359450437" name="Record" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="224" y="323" mx="284" my="339">
      <items count="2">
        <item text="OnRecord_Complete" treenode="632521153359450431" />
        <item text="OnRecord_Failed" treenode="632521153359450436" />
      </items>
      <linkto id="632521153359450440" type="Labeled" style="Bevel" label="Success" />
      <linkto id="632532401935421483" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxTime" type="literal">10000</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="AudioFileFormat" type="literal">wav</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632521153359450440" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="534" y="342">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">Success</log>
      </Properties>
    </node>
    <node type="Action" id="632532401935421483" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="289" y="530">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">Fail</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Complete" startnode="632521153359450430" treenode="632521153359450431" appnode="632521153359450428" handlerfor="632521153359450427">
    <node type="Start" id="632521153359450430" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="52" y="349">
      <linkto id="632521153359450443" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632521153359450443" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="281" y="346">
      <linkto id="632521153359450447" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_Record</ap>
      </Properties>
    </node>
    <node type="Action" id="632521153359450447" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="458" y="327" mx="511" my="343">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632521140802621656" />
        <item text="OnPlay_Failed" treenode="632521140802621661" />
      </items>
      <linkto id="632521153359450450" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="variable">filename</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632521153359450450" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="676" y="341">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632521153359450441" name="filename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Filename" refType="reference" name="Metreos.MediaControl.Record_Complete">filename</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Failed" startnode="632521153359450435" treenode="632521153359450436" appnode="632521153359450433" handlerfor="632521153359450432">
    <node type="Start" id="632521153359450435" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="66" y="195">
      <linkto id="632521153359450456" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632521153359450456" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="303.610046" y="196">
      <linkto id="632521153359450458" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_Record</ap>
      </Properties>
    </node>
    <node type="Action" id="632521153359450458" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="528" y="190">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>