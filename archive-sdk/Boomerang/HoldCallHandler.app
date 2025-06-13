<Application name="HoldCallHandler" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="HoldCallHandler">
    <outline>
      <treenode type="evh" id="632575382506462308" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632575382506462305" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632575382506462304" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575382506462424" level="2" text="Metreos.Providers.TimerFacility.TimerFire: OnTimerFire">
        <node type="function" name="OnTimerFire" id="632575382506462421" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632575382506462420" path="Metreos.Providers.TimerFacility.TimerFire" />
        <references>
          <ref id="632575471786757229" actid="632575382506462425" />
        </references>
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575382506462448" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632575382506462445" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632575382506462444" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632575471786757235" actid="632575382506462459" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575382506462453" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632575382506462450" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632575382506462449" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632575471786757236" actid="632575382506462459" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575382506462458" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632575382506462455" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632575382506462454" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632575471786757237" actid="632575382506462459" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575471786757251" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632575471786757248" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632575471786757247" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575471786757256" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632575471786757253" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632575471786757252" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575471786757261" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632575471786757258" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632575471786757257" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575471786757266" level="2" text="Metreos.CallControl.CallChanged: OnCallChanged">
        <node type="function" name="OnCallChanged" id="632575471786757263" path="Metreos.StockTools" />
        <node type="event" name="CallChanged" id="632575471786757262" path="Metreos.CallControl.CallChanged" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_isCallHeld" id="632575471786757210" vid="632575382506462330">
        <Properties type="String">g_isCallHeld</Properties>
      </treenode>
      <treenode text="g_callId" id="632575471786757212" vid="632575382506462367">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_conxId" id="632575471786757214" vid="632575382506462369">
        <Properties type="String">g_conxId</Properties>
      </treenode>
      <treenode text="g_timerId" id="632575471786757216" vid="632575382506462427">
        <Properties type="String">g_timerId</Properties>
      </treenode>
      <treenode text="g_outboundCallId" id="632575471786757218" vid="632575382506462463">
        <Properties type="String">g_outboundCallId</Properties>
      </treenode>
      <treenode text="g_outboundConxId" id="632575471786757220" vid="632575382506462465">
        <Properties type="String">g_outboundConxId</Properties>
      </treenode>
      <treenode text="g_confId" id="632575471786757222" vid="632575382506462469">
        <Properties type="String">g_confId</Properties>
      </treenode>
      <treenode text="g_from" id="632575471786757224" vid="632575382506462549">
        <Properties type="String">g_from</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632575382506462307" treenode="632575382506462308" appnode="632575382506462305" handlerfor="632575382506462304">
    <node type="Start" id="632575382506462307" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632575382506462552" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575382506462365" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="233" y="144">
      <linkto id="632575382506462425" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DisplayName" type="literal">Timed Hold Service</ap>
        <ap name="Conference" type="literal">true</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="ConnectionId">g_conxId</rd>
        <rd field="ConferenceId">g_confId</rd>
      </Properties>
    </node>
    <node type="Action" id="632575382506462371" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="522" y="144">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575382506462425" name="AddNonTriggerTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="314" y="128" mx="376" my="144">
      <items count="1">
        <item text="OnTimerFire" treenode="632575382506462424" />
      </items>
      <linkto id="632575382506462371" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerDateTime" type="csharp">DateTime.Now.AddSeconds(10);</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="timerId">g_timerId</rd>
      </Properties>
    </node>
    <node type="Action" id="632575382506462552" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="120" y="143">
      <linkto id="632575382506462365" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">from</ap>
        <rd field="ResultData">g_from</rd>
      </Properties>
    </node>
    <node type="Variable" id="632575382506462366" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632575382506462551" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">from</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnTimerFire" startnode="632575382506462423" treenode="632575382506462424" appnode="632575382506462421" handlerfor="632575382506462420">
    <node type="Start" id="632575382506462423" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="110" y="156">
      <linkto id="632575382506462459" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575382506462459" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="267" y="141" mx="333" my="157">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632575382506462448" />
        <item text="OnMakeCall_Failed" treenode="632575382506462453" />
        <item text="OnRemoteHangup" treenode="632575382506462458" />
      </items>
      <linkto id="632575382506462471" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_from</ap>
        <ap name="From" type="literal">123456</ap>
        <ap name="DisplayName" type="literal">Boomerang</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_confId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_outboundCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632575382506462471" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="541" y="155">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632575382506462447" treenode="632575382506462448" appnode="632575382506462445" handlerfor="632575382506462444">
    <node type="Start" id="632575382506462447" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632575382506462472" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575382506462472" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="235" y="134">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632575382506462452" treenode="632575382506462453" appnode="632575382506462450" handlerfor="632575382506462449">
    <node type="Start" id="632575382506462452" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632575382506462473" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575382506462473" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="180" y="160">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632575382506462457" treenode="632575382506462458" appnode="632575382506462455" handlerfor="632575382506462454">
    <node type="Start" id="632575382506462457" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632575382506462474" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575382506462474" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="209" y="180">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632575471786757250" treenode="632575471786757251" appnode="632575471786757248" handlerfor="632575471786757247">
    <node type="Start" id="632575471786757250" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="131">
      <linkto id="632575471786757267" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575471786757267" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="203" y="148">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632575471786757255" treenode="632575471786757256" appnode="632575471786757253" handlerfor="632575471786757252">
    <node type="Start" id="632575471786757255" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632575471786757268" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575471786757268" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="208" y="115">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632575471786757260" treenode="632575471786757261" appnode="632575471786757258" handlerfor="632575471786757257">
    <node type="Start" id="632575471786757260" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632575471786757269" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575471786757269" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="166" y="122">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnCallChanged" activetab="true" startnode="632575471786757265" treenode="632575471786757266" appnode="632575471786757263" handlerfor="632575471786757262">
    <node type="Start" id="632575471786757265" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632575471786757270" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575471786757270" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="274" y="222">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>