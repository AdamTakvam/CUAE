<Application name="Main" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Main">
    <outline>
      <treenode type="evh" id="632579938068362726" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632579938068362723" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632579938068362722" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632579942630031723" level="2" text="Metreos.Providers.TimerFacility.TimerFire: OnTimerFire">
        <node type="function" name="OnTimerFire" id="632579942630031720" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632579942630031719" path="Metreos.Providers.TimerFacility.TimerFire" />
        <references>
          <ref id="632651551399770583" actid="632579942630031724" />
        </references>
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="632579942630031735" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632579942630031732" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632579942630031731" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="gCallId" id="632651551399770577" vid="632579942630031728">
        <Properties type="String">gCallId</Properties>
      </treenode>
      <treenode text="gTimerId" id="632651551399770579" vid="632579942630031740">
        <Properties type="String">gTimerId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632579938068362725" treenode="632579938068362726" appnode="632579938068362723" handlerfor="632579938068362722">
    <node type="Start" id="632579938068362725" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632579942630031730" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632579942630031718" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="301" y="304">
      <linkto id="632579942630031724" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632579942630031724" name="AddNonTriggerTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="386" y="283" mx="448" my="299">
      <items count="1">
        <item text="OnTimerFire" treenode="632579942630031723" />
      </items>
      <linkto id="632579942630031739" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now.AddSeconds(2)</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="timerId">gTimerId</rd>
      </Properties>
    </node>
    <node type="Action" id="632579942630031730" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="115" y="315">
      <linkto id="632579942630031718" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">callId</ap>
        <rd field="ResultData">gCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632579942630031739" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="565" y="291">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632579938068362731" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnTimerFire" startnode="632579942630031722" treenode="632579942630031723" appnode="632579942630031720" handlerfor="632579942630031719">
    <node type="Start" id="632579942630031722" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632579942630031736" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632579942630031726" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="302" y="120">
      <linkto id="632579942630031727" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">gCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632579942630031727" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="487" y="137">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632579942630031736" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="105" y="149">
      <linkto id="632579942630031726" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">gTimerId</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632579942630031734" treenode="632579942630031735" appnode="632579942630031732" handlerfor="632579942630031731">
    <node type="Start" id="632579942630031734" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632579942630031737" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632579942630031737" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="115" y="103">
      <linkto id="632579942630031738" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">gTimerId</ap>
      </Properties>
    </node>
    <node type="Action" id="632579942630031738" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="315" y="75">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>