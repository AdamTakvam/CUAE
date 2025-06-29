<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632368373726385079" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632155715076406469" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632155715076406468" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.ContinuousCalls.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632368373726385080" level="2" text="Metreos.CallControl.Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632155715076406494" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632155715076406493" path="Metreos.CallControl.Hangup_Complete" />
        <references>
          <ref id="632368373726385081" actid="632155715076406501" />
        </references>
        <Properties>
        </Properties>
      </treenode>
      <treenode type="evh" id="632368373726385082" level="2" text="Metreos.CallControl.Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632155715076406498" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632155715076406497" path="Metreos.CallControl.Hangup_Failed" />
        <references>
          <ref id="632368373726385083" actid="632155715076406501" />
        </references>
        <Properties>
        </Properties>
      </treenode>
      <treenode type="evh" id="632368373726385084" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632155715076406504" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632155715076406503" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632368373726385085" actid="632155715076406515" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632368373726385086" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632155715076406508" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632155715076406507" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632368373726385087" actid="632155715076406515" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632368373726385088" level="2" text="Metreos.Providers.TimerFacility.TimerFire: OnTimerFire">
        <node type="function" name="OnTimerFire" id="632155715076406477" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632155715076406476" path="Metreos.Providers.TimerFacility.TimerFire" />
        <references>
          <ref id="632368373726385089" actid="632155715076406517" />
          <ref id="632368373726385090" actid="632155715076406480" />
        </references>
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="fun" id="632368373726385091" level="1" text="OnHangup">
        <node type="function" name="OnHangup" id="632155715076406512" path="Metreos.StockTools" />
      </treenode>
      <treenode type="evh" id="632368373726385092" level="1" text="Metreos.Providers.FunctionalTest.Event: StopTimer">
        <node type="function" name="StopTimer" id="632155773063906419" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632155773063906418" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.ContinuousCalls.script1.E_StopTimer</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Trigger" id="632368373726385040" vid="632155715076406472">
        <Properties type="Metreos.Types.String" initWith="S_Trigger">S_Trigger</Properties>
      </treenode>
      <treenode text="S_MadeCall" id="632368373726385042" vid="632155715076406474">
        <Properties type="Metreos.Types.String" initWith="S_MadeCall">S_MadeCall</Properties>
      </treenode>
      <treenode text="timerId" id="632368373726385044" vid="632155715076406484">
        <Properties type="Metreos.Types.String">timerId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632155715076406470" treenode="632155715076406471" appnode="632155715076406469" handlerfor="632155715076406468">
    <node type="Start" id="632155715076406470" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="138">
      <linkto id="632155715076406480" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632155715076406480" name="AddScriptTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="268" y="124" mx="314" my="140">
      <items count="1">
        <item text="OnTimerFire" treenode="632368373726385088" />
      </items>
      <linkto id="632155715076406486" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now.ToString()</ap>
        <ap name="timerRecurrenceInterval" type="csharp">TimeSpan.FromMilliseconds(double.Parse(callPeriod.ToString())).ToString()</ap>
        <ap name="timerUserData" type="variable">callPeriod</ap>
        <ap name="userData" type="variable">to</ap>
        <rd field="timerId">timerId</rd>
      </Properties>
    </node>
    <node type="Action" id="632155715076406486" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="473" y="142">
      <linkto id="632224814145469447" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Trigger</ap>
      </Properties>
    </node>
    <node type="Action" id="632224814145469447" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="636" y="148">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632155715076406481" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="to" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632155715076406482" name="callDuration" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="callDuration" refType="reference">callDuration</Properties>
    </node>
    <node type="Variable" id="632155715076406483" name="callPeriod" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="callPeriod" refType="reference">callPeriod</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnTimerFire" startnode="632155715076406478" treenode="632155715076406479" appnode="632155715076406477" handlerfor="632155715076406476">
    <node type="Start" id="632155715076406478" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632155715076406488" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632155715076406488" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="38" y="308">
      <linkto id="632155715076406515" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632155715076406501" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <Properties final="false" type="native">
      </Properties>
    </node>
    <node type="Action" id="632155715076406501" name="Hangup" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="253" y="412" mx="315" my="428">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632368373726385080" />
        <item text="OnHangup_Failed" treenode="632368373726385082" />
      </items>
      <linkto id="632224814145469446" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
      </Properties>
    </node>
    <node type="Action" id="632155715076406515" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="158" y="155" mx="224" my="171">
      <items count="2">
        <item text="OnMakeCall_Complete" treenode="632368373726385084" />
        <item text="OnMakeCall_Failed" treenode="632368373726385086" />
      </items>
      <linkto id="632155715076406517" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632155715076406517" name="AddScriptTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="330" y="155" mx="376" my="171">
      <items count="1">
        <item text="OnTimerFire" treenode="632368373726385088" />
      </items>
      <linkto id="632155715076406519" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="timerDateTime" type="csharp">DateTime.Now.AddMilliseconds(double.Parse(timerUserData.ToString())).ToString()</ap>
        <ap name="timerUserData" type="variable">callId</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632155715076406519" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="514" y="172">
      <linkto id="632224814145469445" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_MadeCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632224814145469445" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="651" y="183">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632224814145469446" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="671" y="460">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632155715076406489" name="timerUserData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="timerUserData" refType="reference">timerUserData</Properties>
    </node>
    <node type="Variable" id="632155715076406490" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="userData" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632155715076406491" name="timerId2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="timerId" refType="reference">timerId2</Properties>
    </node>
    <node type="Variable" id="632155715076406518" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup_Complete" startnode="632155715076406495" treenode="632155715076406496" appnode="632155715076406494" handlerfor="632155715076406493">
    <node type="Start" id="632155715076406495" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224814145469448" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632224814145469448" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="408" y="265">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup_Failed" startnode="632155715076406499" treenode="632155715076406500" appnode="632155715076406498" handlerfor="632155715076406497">
    <node type="Start" id="632155715076406499" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224814145469449" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632224814145469449" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="455" y="271">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632155715076406505" treenode="632155715076406506" appnode="632155715076406504" handlerfor="632155715076406503">
    <node type="Start" id="632155715076406505" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224814145469450" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632224814145469450" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="495" y="256">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632155715076406509" treenode="632155715076406510" appnode="632155715076406508" handlerfor="632155715076406507">
    <node type="Start" id="632155715076406509" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224814145469451" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632224814145469451" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="574" y="288">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup" startnode="632155715076406513" treenode="632155715076406514" appnode="632155715076406512" handlerfor="632155715076406511">
    <node type="Start" id="632155715076406513" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224814145469452" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632224814145469452" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="484" y="274">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="StopTimer" startnode="632155773063906420" treenode="632155773063906421" appnode="632155773063906419" handlerfor="632155773063906418">
    <node type="Start" id="632155773063906420" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632155773063906422" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632155773063906422" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="367" y="232">
      <linkto id="632224814145469453" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="timerId" type="variable">timerId</ap>
      </Properties>
    </node>
    <node type="Action" id="632224814145469453" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="576" y="229">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
