<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632520303397145547" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632520303397145544" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632520303397145543" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.MMSVolumeDriver.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632750212047277444" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632750212047277441" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632750212047277440" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632803514214689986" actid="632750212047277455" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632750212047277449" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632750212047277446" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632750212047277445" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632803514214689987" actid="632750212047277455" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632750212047277454" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632750212047277451" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632750212047277450" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632803514214689988" actid="632750212047277455" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632750212047277472" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632750212047277469" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632750212047277468" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632803514214690001" actid="632750212047277478" />
          <ref id="632803514214690004" actid="632750212047277485" />
          <ref id="632803514214690022" actid="632755214438875719" />
          <ref id="632803514214690028" actid="632755214438875774" />
          <ref id="632803514214690042" actid="632755956910089435" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632750212047277477" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632750212047277474" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632750212047277473" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632803514214690002" actid="632750212047277478" />
          <ref id="632803514214690005" actid="632750212047277485" />
          <ref id="632803514214690023" actid="632755214438875719" />
          <ref id="632803514214690029" actid="632755214438875774" />
          <ref id="632803514214690043" actid="632755956910089435" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_HangUp" id="632803514214689967" vid="632750208348953336">
        <Properties type="String" initWith="S_HangUp">S_HangUp</Properties>
      </treenode>
      <treenode text="g_mode" id="632803514214689969" vid="632750212047277460">
        <Properties type="String">g_mode</Properties>
      </treenode>
      <treenode text="S_Failure" id="632803514214689971" vid="632750212047277554">
        <Properties type="String" initWith="S_Failure">S_Failure</Properties>
      </treenode>
      <treenode text="g_callId" id="632803514214689973" vid="632750212047277562">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_scale" id="632803514214689975" vid="632754336019044039">
        <Properties type="String">g_scale</Properties>
      </treenode>
      <treenode text="g_type" id="632803514214689977" vid="632754336019044041">
        <Properties type="String">g_type</Properties>
      </treenode>
      <treenode text="g_toggle" id="632803514214689979" vid="632754336019044043">
        <Properties type="String">g_toggle</Properties>
      </treenode>
      <treenode text="g_tvalue" id="632803514214689981" vid="632754355119024257">
        <Properties type="String">g_tvalue</Properties>
      </treenode>
      <treenode text="g_testScale" id="632803514214689983" vid="632755351618537769">
        <Properties type="String">g_testScale</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632520303397145546" treenode="632520303397145547" appnode="632520303397145544" handlerfor="632520303397145543">
    <node type="Start" id="632520303397145546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="249">
      <linkto id="632750212047277455" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632750212047277455" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="200" y="236" mx="266" my="252">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632750212047277444" />
        <item text="OnMakeCall_Failed" treenode="632750212047277449" />
        <item text="OnRemoteHangup" treenode="632750212047277454" />
      </items>
      <linkto id="632750212047277459" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">to</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_callId</rd>
      </Properties>
    </node>
    <node type="Action" id="632750212047277459" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="401" y="265">
      <linkto id="632754355119024263" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">mode</ap>
        <ap name="Value2" type="variable">type</ap>
        <ap name="Value3" type="variable">scale</ap>
        <ap name="Value4" type="variable">toggle</ap>
        <rd field="ResultData">g_mode</rd>
        <rd field="ResultData2">g_type</rd>
        <rd field="ResultData3">g_scale</rd>
        <rd field="ResultData4">g_toggle</rd>
      </Properties>
    </node>
    <node type="Action" id="632750212047277464" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="645.4707" y="266">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632754355119024263" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="503" y="266">
      <linkto id="632750212047277464" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">tvalue</ap>
        <ap name="Value2" type="variable">testScale</ap>
        <rd field="ResultData">g_tvalue</rd>
        <rd field="ResultData2">g_testScale</rd>
      </Properties>
    </node>
    <node type="Variable" id="632750212047277437" name="mode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="mode" refType="reference">mode</Properties>
    </node>
    <node type="Variable" id="632750212047277438" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632754355119024259" name="type" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="type" refType="reference">type</Properties>
    </node>
    <node type="Variable" id="632754355119024260" name="scale" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="scale" refType="reference">scale</Properties>
    </node>
    <node type="Variable" id="632754355119024261" name="toggle" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="toggle" refType="reference">toggle</Properties>
    </node>
    <node type="Variable" id="632754355119024262" name="tvalue" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="tvalue" refType="reference">tvalue</Properties>
    </node>
    <node type="Variable" id="632755351618537771" name="testScale" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="testScale" refType="reference">testScale</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" activetab="true" startnode="632750212047277443" treenode="632750212047277444" appnode="632750212047277441" handlerfor="632750212047277440">
    <node type="Loop" id="632754242368209785" name="Loop" text="loop 10x" cx="515" cy="107" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="257" y="40" mx="514" my="94">
      <linkto id="632754242368209787" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632754242368209788" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="literal">10</Properties>
    </node>
    <node type="Loop" id="632755214438875710" name="Loop" text="loop 10x" cx="464" cy="110" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="303" y="358" mx="535" my="413">
      <linkto id="632755214438875712" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632755214438875711" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="literal">10</Properties>
    </node>
    <node type="Start" id="632750212047277443" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="68" y="892">
      <linkto id="632799373204358921" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632750212047277478" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="14" y="77" mx="67" my="93">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632750212047277472" />
        <item text="OnPlay_Failed" treenode="632750212047277477" />
      </items>
      <linkto id="632754242368209785" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Volume" type="literal">10</ap>
        <ap name="Prompt1" type="literal">MMSvolumeL.wav</ap>
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632750212047277485" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="243" y="684" mx="296" my="700">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632750212047277472" />
        <item text="OnPlay_Failed" treenode="632750212047277477" />
      </items>
      <linkto id="632756081047792863" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">MMSvolume.wav</ap>
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632750212047277490" name="AdjustPlay" container="632754242368209785" class="MaxActionNode" group="" path="Metreos.MediaControl" x="543.089844" y="93">
      <linkto id="632755214438875697" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="AdjustmentType" type="variable">g_testScale</ap>
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="Volume" type="variable">i</ap>
      </Properties>
    </node>
    <node type="Action" id="632750212047277491" name="Sleep" container="632754242368209785" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="444.089844" y="93">
      <linkto id="632750212047277490" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">2000</ap>
      </Properties>
    </node>
    <node type="Action" id="632754242368209787" name="Assign" container="632754242368209785" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="333" y="93">
      <linkto id="632750212047277491" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">g_testScale == "absolute" ? i-2 : -2</ap>
        <rd field="ResultData">i</rd>
        <log condition="exit" on="true" level="Info" type="csharp">"DEBUG: volume adjustment is " + g_testScale + ", adjusted value is now " + i</log>
      </Properties>
    </node>
    <node type="Label" id="632754242368209788" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="965" y="93" />
    <node type="Label" id="632754242368209790" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="677.439453" y="638.000061">
      <linkto id="632755351618537773" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632754355119024248" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="68" y="699">
      <linkto id="632754355119024249" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632750212047277485" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_mode</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Debug: Connection ID obtained: " + connId</log>
      </Properties>
    </node>
    <node type="Action" id="632754355119024249" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="70" y="413">
      <linkto id="632754355119024250" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632755956910089428" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_type</ap>
      </Properties>
    </node>
    <node type="Action" id="632754355119024250" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="70" y="249">
      <linkto id="632750212047277478" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632755214438875774" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_toggle</ap>
      </Properties>
    </node>
    <node type="Action" id="632755214438875697" name="Sleep" container="632754242368209785" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="621" y="93">
      <linkto id="632754242368209785" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">2000</ap>
      </Properties>
    </node>
    <node type="Comment" id="632755214438875698" text="Adjust Volume Absolute/Relative" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="429" y="32" />
    <node type="Label" id="632755214438875711" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="960" y="413" />
    <node type="Action" id="632755214438875712" name="Assign" container="632755214438875710" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="369" y="413">
      <linkto id="632755214438875713" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">g_testScale == "absolute" ? i-2 : -2</ap>
        <rd field="ResultData">i</rd>
        <log condition="exit" on="true" level="Info" type="csharp">"DEBUG: speed adjustment is " + g_testScale + ", adjusted value is now " + i</log>
      </Properties>
    </node>
    <node type="Action" id="632755214438875713" name="Sleep" container="632755214438875710" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="452" y="413">
      <linkto id="632755214438875714" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">2000</ap>
      </Properties>
    </node>
    <node type="Action" id="632755214438875714" name="AdjustPlay" container="632755214438875710" class="MaxActionNode" group="" path="Metreos.MediaControl" x="564" y="414">
      <linkto id="632755214438875715" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="AdjustmentType" type="variable">g_testScale</ap>
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="Speed" type="variable">i</ap>
      </Properties>
    </node>
    <node type="Action" id="632755214438875715" name="Sleep" container="632755214438875710" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="657" y="413">
      <linkto id="632755214438875710" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">2000</ap>
      </Properties>
    </node>
    <node type="Action" id="632755214438875719" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="187" y="396" mx="240" my="412">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632750212047277472" />
        <item text="OnPlay_Failed" treenode="632750212047277477" />
      </items>
      <linkto id="632755214438875710" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Speed" type="literal">10</ap>
        <ap name="Prompt1" type="literal">MMSvolumeL.wav</ap>
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Comment" id="632755214438875724" text="Constant" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="145" y="737" />
    <node type="Comment" id="632755214438875725" text="Mode: Adjust / Constant" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="657" />
    <node type="Comment" id="632755214438875771" text="Toggle: False/True" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="214" />
    <node type="Action" id="632755214438875774" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="203" y="232" mx="256" my="248">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632750212047277472" />
        <item text="OnPlay_Failed" treenode="632750212047277477" />
      </items>
      <linkto id="632755214438875780" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Volume" type="literal">10</ap>
        <ap name="Prompt1" type="literal">MMSvolumeL.wav</ap>
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632755214438875780" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="359.089844" y="248">
      <linkto id="632755214438875782" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">2000</ap>
      </Properties>
    </node>
    <node type="Action" id="632755214438875782" name="AdjustPlay" class="MaxActionNode" group="" path="Metreos.MediaControl" x="447.089844" y="248">
      <linkto id="632755214438875784" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="AdjustmentType" type="literal">absolute</ap>
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="Volume" type="literal">5</ap>
      </Properties>
    </node>
    <node type="Action" id="632755214438875784" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="529" y="248">
      <linkto id="632756081047792854" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">2000</ap>
      </Properties>
    </node>
    <node type="Label" id="632755214438875786" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="965" y="248" />
    <node type="Action" id="632755351618537773" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="677" y="701">
      <linkto id="632755351618537774" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632755351618537774" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="757.610046" y="701">
      <linkto id="632755351618537776" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_HangUp</ap>
      </Properties>
    </node>
    <node type="Action" id="632755351618537776" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="866" y="700">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632755956910089428" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="158" y="413">
      <linkto id="632755214438875719" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632755956910089435" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_toggle</ap>
      </Properties>
    </node>
    <node type="Comment" id="632755956910089430" text="Toggle: False/True" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="113" y="447" />
    <node type="Comment" id="632755956910089432" text="Type: Volume/Speed" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="37" y="358" />
    <node type="Comment" id="632755956910089433" text="Adjust Speed Absolute/Relative" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="426" y="328" />
    <node type="Action" id="632755956910089435" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="108" y="556" mx="161" my="572">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632750212047277472" />
        <item text="OnPlay_Failed" treenode="632750212047277477" />
      </items>
      <linkto id="632755956910089436" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Volume" type="literal">10</ap>
        <ap name="Prompt1" type="literal">MMSvolumeL.wav</ap>
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632755956910089436" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="264.089844" y="572">
      <linkto id="632755956910089437" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">2000</ap>
      </Properties>
    </node>
    <node type="Action" id="632755956910089437" name="AdjustPlay" class="MaxActionNode" group="" path="Metreos.MediaControl" x="360.089844" y="572">
      <linkto id="632755956910089438" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="AdjustmentType" type="literal">absolute</ap>
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="Speed" type="literal">5</ap>
      </Properties>
    </node>
    <node type="Action" id="632755956910089438" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="451" y="572">
      <linkto id="632756081047792850" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">4000</ap>
      </Properties>
    </node>
    <node type="Label" id="632755956910089439" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="965" y="572" />
    <node type="Comment" id="632755956910089440" text="Speed=10" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="92" y="528" />
    <node type="Comment" id="632755956910089453" text="Adjust Abosolute &#xD;&#xA;Volume=5" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="398" y="189" />
    <node type="Comment" id="632755956910089454" text="Volume=10" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="222" y="205" />
    <node type="Action" id="632756081047792850" name="AdjustPlay" class="MaxActionNode" group="" path="Metreos.MediaControl" x="544" y="572">
      <linkto id="632756081047792852" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="AdjustmentType" type="literal">toggle</ap>
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="Speed" type="literal">0</ap>
        <ap name="ToggleType" type="variable">g_tvalue</ap>
      </Properties>
    </node>
    <node type="Action" id="632756081047792852" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="628" y="572">
      <linkto id="632756081047792873" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">4000</ap>
      </Properties>
    </node>
    <node type="Action" id="632756081047792854" name="AdjustPlay" class="MaxActionNode" group="" path="Metreos.MediaControl" x="635" y="249">
      <linkto id="632756081047792856" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="AdjustmentType" type="literal">toggle</ap>
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="Volume" type="literal">10</ap>
        <ap name="ToggleType" type="variable">g_tvalue</ap>
      </Properties>
    </node>
    <node type="Action" id="632756081047792856" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="723" y="249">
      <linkto id="632756081047792867" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">4000</ap>
      </Properties>
    </node>
    <node type="Comment" id="632756081047792858" text="Adjust Abosolute &#xD;&#xA;Speed=5" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="317" y="510" />
    <node type="Comment" id="632756081047792860" text="Adjust Volume=toggle&#xD;&#xA;Toggel Value=(1 - 4)" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="566" y="190" />
    <node type="Comment" id="632756081047792861" text="Adjust Speed=toggle&#xD;&#xA;Toggel Value=(1 - 4)" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="482" y="513" />
    <node type="Action" id="632756081047792863" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="471" y="701">
      <linkto id="632755351618537773" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">3000</ap>
      </Properties>
    </node>
    <node type="Comment" id="632756081047792865" text="Volume=Default&#xD;&#xA;Speed=Default" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="254" y="648" />
    <node type="Action" id="632756081047792867" name="AdjustPlay" class="MaxActionNode" group="" path="Metreos.MediaControl" x="805" y="249">
      <linkto id="632756081047792869" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="AdjustmentType" type="literal">toggle</ap>
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="Volume" type="literal">10</ap>
        <ap name="ToggleType" type="variable">g_tvalue</ap>
      </Properties>
    </node>
    <node type="Action" id="632756081047792869" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="896" y="248">
      <linkto id="632755214438875786" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">4000</ap>
      </Properties>
    </node>
    <node type="Comment" id="632756081047792871" text="Adjust Volume=toggle&#xD;&#xA;Toggel Value=(1 - 4)" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="741" y="192" />
    <node type="Action" id="632756081047792873" name="AdjustPlay" class="MaxActionNode" group="" path="Metreos.MediaControl" x="728" y="572">
      <linkto id="632756081047792875" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="AdjustmentType" type="literal">toggle</ap>
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="Speed" type="literal">0</ap>
        <ap name="ToggleType" type="variable">g_tvalue</ap>
      </Properties>
    </node>
    <node type="Action" id="632756081047792875" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="847" y="572">
      <linkto id="632755956910089439" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">4000</ap>
      </Properties>
    </node>
    <node type="Comment" id="632756081047792877" text="Adjust Speed=toggle&#xD;&#xA;Toggel Value=(1 - 4)" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="663" y="513" />
    <node type="Action" id="632799373204358921" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="68" y="805">
      <linkto id="632754355119024248" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(string g_type, string g_mode, string g_testScale, LogWriter log)
{

  string sType="Speed";

  if (g_type.ToUpper()=="TRUE") {
    sType = "Volume";
  }


  // adjust
  if (g_mode.ToUpper()=="TRUE") {
    log.Write(TraceLevel.Verbose, "DEBUG: Adjust {0} {1} thru a range from 10 to -10",sType , g_testScale);
  } else {
    log.Write(TraceLevel.Verbose, "DEBUG: {0} one time adjustment",sType);
  }

	return IApp.VALUE_SUCCESS;	
}</Properties>
    </node>
    <node type="Comment" id="632799373204358922" text="Debug statement recording the &#xD;&#xA;parameters of this test run" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="108" y="792" />
    <node type="Variable" id="632750212047277484" name="connId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connId</Properties>
    </node>
    <node type="Variable" id="632754242368209786" name="i" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="" defaultInitWith="10" refType="reference">i</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632750212047277448" treenode="632750212047277449" appnode="632750212047277446" handlerfor="632750212047277445">
    <node type="Start" id="632750212047277448" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="184">
      <linkto id="632750212047277556" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632750212047277556" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="223" y="189">
      <linkto id="632750212047277557" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Failure</ap>
      </Properties>
    </node>
    <node type="Action" id="632750212047277557" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="394" y="190">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632750212047277453" treenode="632750212047277454" appnode="632750212047277451" handlerfor="632750212047277450">
    <node type="Start" id="632750212047277453" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="262">
      <linkto id="632750212047277558" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632750212047277558" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="330" y="249">
      <linkto id="632750212047277559" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_HangUp</ap>
      </Properties>
    </node>
    <node type="Action" id="632750212047277559" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="573" y="220">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632750212047277471" treenode="632750212047277472" appnode="632750212047277469" handlerfor="632750212047277468">
    <node type="Start" id="632750212047277471" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="292">
      <linkto id="632750212047277560" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632750212047277560" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="292" y="282">
      <linkto id="632750212047277561" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632750212047277561" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="464" y="274">
      <linkto id="632750212047277569" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_HangUp</ap>
      </Properties>
    </node>
    <node type="Action" id="632750212047277569" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="656" y="278">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632750212047277476" treenode="632750212047277477" appnode="632750212047277474" handlerfor="632750212047277473">
    <node type="Start" id="632750212047277476" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="189">
      <linkto id="632750212047277564" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632750212047277564" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="174.352875" y="187">
      <linkto id="632750212047277565" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632750212047277565" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="338.3529" y="189">
      <linkto id="632750212047277568" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Failure</ap>
      </Properties>
    </node>
    <node type="Action" id="632750212047277568" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="501" y="188">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>