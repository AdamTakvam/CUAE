<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632583244056597571" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632583244056597568" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632583244056597567" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.BasicTTS.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632583244056597596" level="2" text="Metreos.Providers.FunctionalTest.Event: OnEvent">
        <node type="function" name="OnEvent" id="632583244056597593" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632583244056597592" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.BasicTTS.script1.E_Play</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632583244056597603" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632583244056597600" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632583244056597599" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632583518663390402" actid="632583244056597614" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632583244056597608" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632583244056597605" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632583244056597604" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632583518663390403" actid="632583244056597614" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632583244056597613" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632583244056597610" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632583244056597609" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632583518663390404" actid="632583244056597614" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632583244056597682" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632583244056597679" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632583244056597678" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632583518663390411" actid="632583244056597688" />
          <ref id="632583518663390444" actid="632583518663390442" />
          <ref id="632583518663390448" actid="632583518663390446" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632583244056597687" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632583244056597684" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632583244056597683" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632583518663390412" actid="632583244056597688" />
          <ref id="632583518663390445" actid="632583518663390442" />
          <ref id="632583518663390449" actid="632583518663390446" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_PlayComplete" id="632583518663390391" vid="632583244056597588">
        <Properties type="String" initWith="S_PlayComplete">S_PlayComplete</Properties>
      </treenode>
      <treenode text="S_MadeCall" id="632583518663390393" vid="632583244056597590">
        <Properties type="String" initWith="S_MadeCall">S_MadeCall</Properties>
      </treenode>
      <treenode text="g_callId" id="632583518663390395" vid="632583244056597618">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="S_Hangup" id="632583518663390397" vid="632583244056597672">
        <Properties type="String" initWith="S_Hangup">S_Hangup</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632583518663390399" vid="632583244056597691">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632583244056597570" treenode="632583244056597571" appnode="632583244056597568" handlerfor="632583244056597567">
    <node type="Start" id="632583244056597570" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="268">
      <linkto id="632583244056597614" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632583244056597614" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="282" y="249" mx="348" my="265">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632583244056597603" />
        <item text="OnMakeCall_Failed" treenode="632583244056597608" />
        <item text="OnRemoteHangup" treenode="632583244056597613" />
      </items>
      <linkto id="632583244056597620" type="Labeled" style="Bevel" label="Success" />
      <linkto id="632583244056597696" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">to</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_callId</rd>
      </Properties>
    </node>
    <node type="Action" id="632583244056597620" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="567" y="266">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632583244056597695" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="348" y="562">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632583244056597696" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="344" y="432">
      <linkto id="632583244056597695" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_MadeCall</ap>
      </Properties>
    </node>
    <node type="Variable" id="632583244056597597" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to" refType="reference">to</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent" activetab="true" startnode="632583244056597595" treenode="632583244056597596" appnode="632583244056597593" handlerfor="632583244056597592">
    <node type="Start" id="632583244056597595" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="289">
      <linkto id="632583518663390441" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632583244056597688" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="209" y="271" mx="262" my="287">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632583244056597682" />
        <item text="OnPlay_Failed" treenode="632583244056597687" />
      </items>
      <linkto id="632583244056597698" type="Labeled" style="Bevel" label="default" />
      <linkto id="632583244056597702" type="Labeled" style="Bevel" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxTime" type="literal">12000000</ap>
        <ap name="Prompt1" type="variable">play1</ap>
        <ap name="Prompt2" type="variable">play2</ap>
        <ap name="Prompt3" type="variable">play3</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632583244056597698" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="254" y="446">
      <linkto id="632583244056597699" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_PlayComplete</ap>
      </Properties>
    </node>
    <node type="Action" id="632583244056597699" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="254" y="530">
      <linkto id="632583244056597700" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632583244056597700" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="256" y="600">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632583244056597702" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="416" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632583518663390441" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="104" y="292">
      <linkto id="632583244056597688" type="Labeled" style="Bevel" label="three" />
      <linkto id="632583518663390446" type="Labeled" style="Bevel" label="two" />
      <linkto id="632583518663390442" type="Labeled" style="Bevel" label="one" />
      <Properties language="csharp">
public static string Execute(string play1, string play2, string play3)
{
	if(play2 == "NONE" &amp;&amp; play3 == "NONE")
	{
		return "one";
	}

	if(play3 == "NONE")
	{
		return "two";
	}

	return "three";
}
</Properties>
    </node>
    <node type="Action" id="632583518663390442" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="227" y="16" mx="280" my="32">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632583244056597682" />
        <item text="OnPlay_Failed" treenode="632583244056597687" />
      </items>
      <linkto id="632583244056597698" type="Labeled" style="Bevel" label="default" />
      <linkto id="632583244056597702" type="Labeled" style="Bevel" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxTime" type="literal">12000000</ap>
        <ap name="Prompt1" type="variable">play1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632583518663390446" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="208" y="154" mx="261" my="170">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632583244056597682" />
        <item text="OnPlay_Failed" treenode="632583244056597687" />
      </items>
      <linkto id="632583244056597698" type="Labeled" style="Bevel" label="default" />
      <linkto id="632583244056597702" type="Labeled" style="Bevel" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxTime" type="literal">12000000</ap>
        <ap name="Prompt1" type="variable">play1</ap>
        <ap name="Prompt2" type="variable">play2</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Variable" id="632583244056597675" name="play1" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="play1" defaultInitWith="NONE" refType="reference">play1</Properties>
    </node>
    <node type="Variable" id="632583244056597676" name="play2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="play2" defaultInitWith="NONE" refType="reference">play2</Properties>
    </node>
    <node type="Variable" id="632583244056597677" name="play3" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="play3" defaultInitWith="NONE" refType="reference">play3</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632583244056597602" treenode="632583244056597603" appnode="632583244056597600" handlerfor="632583244056597599">
    <node type="Start" id="632583244056597602" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="327">
      <linkto id="632583244056597694" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632583244056597621" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="248" y="328">
      <linkto id="632583244056597622" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_MadeCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632583244056597622" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="441" y="324">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632583244056597694" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="124" y="326">
      <linkto id="632583244056597621" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">connectionId</ap>
        <rd field="ResultData">g_connectionId</rd>
      </Properties>
    </node>
    <node type="Variable" id="632583244056597693" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632583244056597607" treenode="632583244056597608" appnode="632583244056597605" handlerfor="632583244056597604">
    <node type="Start" id="632583244056597607" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="285">
      <linkto id="632583244056597623" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632583244056597623" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="183.610016" y="285">
      <linkto id="632583244056597624" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_MadeCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632583244056597624" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="368.610046" y="285">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632583244056597612" treenode="632583244056597613" appnode="632583244056597610" handlerfor="632583244056597609">
    <node type="Start" id="632583244056597612" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="45" y="287">
      <linkto id="632583244056597671" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632583244056597671" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="266" y="283">
      <linkto id="632583244056597674" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Hangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632583244056597674" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="519" y="284">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632583244056597681" treenode="632583244056597682" appnode="632583244056597679" handlerfor="632583244056597678">
    <node type="Start" id="632583244056597681" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="56" y="328">
      <linkto id="632583244056597703" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632583244056597703" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="248" y="336">
      <linkto id="632583244056597707" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_PlayComplete</ap>
      </Properties>
    </node>
    <node type="Action" id="632583244056597707" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="425" y="327">
      <linkto id="632583244056597708" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632583244056597708" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="650" y="329">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632583244056597686" treenode="632583244056597687" appnode="632583244056597684" handlerfor="632583244056597683">
    <node type="Start" id="632583244056597686" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="264">
      <linkto id="632583244056597705" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632583244056597705" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="160" y="256">
      <linkto id="632583244056597709" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_PlayComplete</ap>
      </Properties>
    </node>
    <node type="Action" id="632583244056597709" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="264" y="248">
      <linkto id="632583244056597710" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632583244056597710" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="360" y="248">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>