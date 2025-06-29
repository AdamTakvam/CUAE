<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632521168379182131" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632521168379182128" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632521168379182127" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">IVT.SimpleConference.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632521168379182153" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632521168379182150" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632521168379182149" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632527174907346308" actid="632521168379182164" />
          <ref id="632527174907346322" actid="632521168379182178" />
          <ref id="632527174907346326" actid="632521168379182182" />
          <ref id="632527174907346330" actid="632521168379182186" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632521168379182158" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632521168379182155" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632521168379182154" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632527174907346309" actid="632521168379182164" />
          <ref id="632527174907346323" actid="632521168379182178" />
          <ref id="632527174907346327" actid="632521168379182182" />
          <ref id="632527174907346331" actid="632521168379182186" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632521168379182163" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632521168379182160" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632521168379182159" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632527174907346310" actid="632521168379182164" />
          <ref id="632527174907346324" actid="632521168379182178" />
          <ref id="632527174907346328" actid="632521168379182182" />
          <ref id="632527174907346332" actid="632521168379182186" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_MakeCallComplete" id="632527174907346291" vid="632521168379182132">
        <Properties type="String" initWith="S_MakeCallComplete">S_MakeCallComplete</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632527174907346293" vid="632521168379182169">
        <Properties type="String">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_firstCall" id="632527174907346295" vid="632521168379182172">
        <Properties type="Bool" defaultInitWith="true">g_firstCall</Properties>
      </treenode>
      <treenode text="g_to2" id="632527174907346297" vid="632521168379182191">
        <Properties type="String">g_to2</Properties>
      </treenode>
      <treenode text="g_to3" id="632527174907346299" vid="632521168379182193">
        <Properties type="String">g_to3</Properties>
      </treenode>
      <treenode text="g_to4" id="632527174907346301" vid="632521168379182195">
        <Properties type="String">g_to4</Properties>
      </treenode>
      <treenode text="g_outstandingCalls" id="632527174907346303" vid="632521168379182200">
        <Properties type="Int" defaultInitWith="0">g_outstandingCalls</Properties>
      </treenode>
      <treenode text="S_AllHangup" id="632527174907346305" vid="632521168379182278">
        <Properties type="String" initWith="S_AllHangup">S_AllHangup</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632521168379182130" treenode="632521168379182131" appnode="632521168379182128" handlerfor="632521168379182127">
    <node type="Start" id="632521168379182130" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="336">
      <linkto id="632521168379182197" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632521168379182164" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="264" y="320" mx="330" my="336">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632521168379182153" />
        <item text="OnMakeCall_Failed" treenode="632521168379182158" />
        <item text="OnRemoteHangup" treenode="632521168379182163" />
      </items>
      <linkto id="632521168379182171" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">to1</ap>
        <ap name="From" type="literal">IVT Test Conference</ap>
        <ap name="DisplayName" type="literal">IVT Test Conference</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="literal">0</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632521168379182171" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="520" y="336">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632521168379182197" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="148" y="339">
      <linkto id="632521168379182164" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">to2</ap>
        <ap name="Value2" type="variable">to3</ap>
        <ap name="Value3" type="variable">to4</ap>
        <rd field="ResultData">g_to2</rd>
        <rd field="ResultData2">g_to3</rd>
        <rd field="ResultData3">g_to4</rd>
      </Properties>
    </node>
    <node type="Variable" id="632521168379182144" name="to1" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to1" refType="reference">to1</Properties>
    </node>
    <node type="Variable" id="632521168379182145" name="to2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to2" refType="reference">to2</Properties>
    </node>
    <node type="Variable" id="632521168379182146" name="to3" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to3" refType="reference">to3</Properties>
    </node>
    <node type="Variable" id="632521168379182147" name="to4" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to4" refType="reference">to4</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" activetab="true" startnode="632521168379182152" treenode="632521168379182153" appnode="632521168379182150" handlerfor="632521168379182149">
    <node type="Start" id="632521168379182152" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="352">
      <linkto id="632521168379182202" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632521168379182174" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="312" y="352">
      <linkto id="632521168379182176" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632521168379182190" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_firstCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632521168379182176" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="480" y="352">
      <linkto id="632521168379182178" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">false</ap>
        <ap name="Value2" type="variable">conferenceId</ap>
        <rd field="ResultData">g_firstCall</rd>
        <rd field="ResultData2">g_conferenceId</rd>
      </Properties>
    </node>
    <node type="Action" id="632521168379182177" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="200" y="352">
      <linkto id="632521168379182174" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_MakeCallComplete</ap>
      </Properties>
    </node>
    <node type="Action" id="632521168379182178" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="592" y="336" mx="658" my="352">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632521168379182153" />
        <item text="OnMakeCall_Failed" treenode="632521168379182158" />
        <item text="OnRemoteHangup" treenode="632521168379182163" />
      </items>
      <linkto id="632521168379182182" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_to2</ap>
        <ap name="From" type="literal">IVT Test Conference</ap>
        <ap name="DisplayName" type="literal">IVT Test Conference</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632521168379182182" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="752" y="336" mx="818" my="352">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632521168379182153" />
        <item text="OnMakeCall_Failed" treenode="632521168379182158" />
        <item text="OnRemoteHangup" treenode="632521168379182163" />
      </items>
      <linkto id="632521168379182186" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_to3</ap>
        <ap name="From" type="literal">IVT Test Conference</ap>
        <ap name="DisplayName" type="literal">IVT Test Conference</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632521168379182186" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="920" y="336" mx="986" my="352">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632521168379182153" />
        <item text="OnMakeCall_Failed" treenode="632521168379182158" />
        <item text="OnRemoteHangup" treenode="632521168379182163" />
      </items>
      <linkto id="632521168379182198" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_to4</ap>
        <ap name="From" type="literal">IVT Test Conference</ap>
        <ap name="DisplayName" type="literal">IVT Test Conference</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632521168379182190" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="312" y="544">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">Not first call</log>
      </Properties>
    </node>
    <node type="Action" id="632521168379182198" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1168" y="352">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632521168379182202" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="112" y="352">
      <linkto id="632521168379182177" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref int g_outstandingCalls)
{
	g_outstandingCalls++;
	return "";
}
</Properties>
    </node>
    <node type="Variable" id="632521168379182175" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConferenceId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">conferenceId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632521168379182157" treenode="632521168379182158" appnode="632521168379182155" handlerfor="632521168379182154">
    <node type="Start" id="632521168379182157" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="352">
      <linkto id="632521168379182280" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632521168379182280" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="224" y="352">
      <linkto id="632521168379182282" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_MakeCallComplete</ap>
      </Properties>
    </node>
    <node type="Action" id="632521168379182282" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="376" y="352">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632521168379182162" treenode="632521168379182163" appnode="632521168379182160" handlerfor="632521168379182159">
    <node type="Start" id="632521168379182162" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="336">
      <linkto id="632521168379182203" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632521168379182203" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="232" y="336">
      <linkto id="632521168379182205" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632521168379182277" type="Labeled" style="Bezier" ortho="true" label="exit" />
      <Properties language="csharp">
public static string Execute(ref int g_outstandingCalls)
{
	g_outstandingCalls--;

	if(g_outstandingCalls == 0)
	{
		return "exit";
	}
	else
	{
		return "";
	}
}
</Properties>
    </node>
    <node type="Action" id="632521168379182204" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="472" y="336">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632521168379182205" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="233" y="476">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632521168379182277" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="352" y="336">
      <linkto id="632521168379182204" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_AllHangup</ap>
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
