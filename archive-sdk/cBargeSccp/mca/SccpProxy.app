<Application name="SccpProxy" trigger="Metreos.Providers.SccpProxy.Register" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="SccpProxy">
    <outline>
      <treenode type="evh" id="632451464064027782" level="1" text="Metreos.Providers.SccpProxy.Register (trigger): OnRegister">
        <node type="function" name="OnRegister" id="632451464064027779" path="Metreos.StockTools" />
        <node type="event" name="Register" id="632451464064027778" path="Metreos.Providers.SccpProxy.Register" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632451464064027796" level="2" text="Metreos.Providers.SccpProxy.RegisterAck: OnRegisterAck">
        <node type="function" name="OnRegisterAck" id="632451464064027793" path="Metreos.StockTools" />
        <node type="event" name="RegisterAck" id="632451464064027792" path="Metreos.Providers.SccpProxy.RegisterAck" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632451464064027804" level="2" text="Metreos.Providers.SccpProxy.SessionFailure: OnSessionFailure">
        <node type="function" name="OnSessionFailure" id="632451464064027801" path="Metreos.StockTools" />
        <node type="event" name="SessionFailure" id="632451464064027800" path="Metreos.Providers.SccpProxy.SessionFailure" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632451464064027809" level="2" text="Metreos.Providers.SccpProxy.OpenReceiveChannel: OnOpenReceiveChannel">
        <node type="function" name="OnOpenReceiveChannel" id="632451464064027806" path="Metreos.StockTools" />
        <node type="event" name="OpenReceiveChannel" id="632451464064027805" path="Metreos.Providers.SccpProxy.OpenReceiveChannel" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632451464064027814" level="2" text="Metreos.Providers.SccpProxy.CloseReceiveChannel: OnCloseReceiveChannel">
        <node type="function" name="OnCloseReceiveChannel" id="632451464064027811" path="Metreos.StockTools" />
        <node type="event" name="CloseReceiveChannel" id="632451464064027810" path="Metreos.Providers.SccpProxy.CloseReceiveChannel" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632451464064027819" level="2" text="Metreos.Providers.SccpProxy.StopMediaTransmission: OnStopMediaTransmission">
        <node type="function" name="OnStopMediaTransmission" id="632451464064027816" path="Metreos.StockTools" />
        <node type="event" name="StopMediaTransmission" id="632451464064027815" path="Metreos.Providers.SccpProxy.StopMediaTransmission" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632451464064027824" level="2" text="Metreos.Providers.SccpProxy.CallInfo: OnCallInfo">
        <node type="function" name="OnCallInfo" id="632451464064027821" path="Metreos.StockTools" />
        <node type="event" name="CallInfo" id="632451464064027820" path="Metreos.Providers.SccpProxy.CallInfo" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632451464064027829" level="2" text="Metreos.Providers.SccpProxy.CallState: OnCallState">
        <node type="function" name="OnCallState" id="632451464064027826" path="Metreos.StockTools" />
        <node type="event" name="CallState" id="632451464064027825" path="Metreos.Providers.SccpProxy.CallState" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632451464064027834" level="2" text="Metreos.Providers.SccpProxy.StartMediaTransmission: OnStartMediaTransmission">
        <node type="function" name="OnStartMediaTransmission" id="632451464064027831" path="Metreos.StockTools" />
        <node type="event" name="StartMediaTransmission" id="632451464064027830" path="Metreos.Providers.SccpProxy.StartMediaTransmission" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632451464064027839" level="2" text="Metreos.Providers.SccpProxy.OpenReceiveChannelAck: OnOpenReceiveChannelAck">
        <node type="function" name="OnOpenReceiveChannelAck" id="632451464064027836" path="Metreos.StockTools" />
        <node type="event" name="OpenReceiveChannelAck" id="632451464064027835" path="Metreos.Providers.SccpProxy.OpenReceiveChannelAck" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632458302086556226" level="2" text="Metreos.Providers.SccpProxy.Unregister: OnUnregister">
        <node type="function" name="OnUnregister" id="632458302086556223" path="Metreos.StockTools" />
        <node type="event" name="Unregister" id="632458302086556222" path="Metreos.Providers.SccpProxy.Unregister" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632458302086556231" level="2" text="Metreos.Providers.SccpProxy.UnregisterAck: OnUnregisterAck">
        <node type="function" name="OnUnregisterAck" id="632458302086556228" path="Metreos.StockTools" />
        <node type="event" name="UnregisterAck" id="632458302086556227" path="Metreos.Providers.SccpProxy.UnregisterAck" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632452445701826545" level="1" text="WriteRecordToDatabase">
        <node type="function" name="WriteRecordToDatabase" id="632452445701826542" path="Metreos.StockTools" />
        <calls>
          <ref actid="632454592647767149" />
          <ref actid="632454666735455679" />
        </calls>
      </treenode>
      <treenode type="fun" id="632455736287139051" level="1" text="CleanUpSessions">
        <node type="function" name="CleanUpSessions" id="632455736287139048" path="Metreos.StockTools" />
        <calls>
          <ref actid="632455736287139047" />
          <ref actid="632458302086556233" />
        </calls>
      </treenode>
      <treenode type="fun" id="632467350589690024" level="1" text="EstablishMedia">
        <node type="function" name="EstablishMedia" id="632467350589690021" path="Metreos.StockTools" />
        <calls>
          <ref actid="632470063913234299" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_ccmIP" id="632472631815047980" vid="632451464064027788">
        <Properties type="String" initWith="CallManagerIP">g_ccmIP</Properties>
      </treenode>
      <treenode text="g_ccmPort" id="632472631815047982" vid="632451464064027790">
        <Properties type="Int" initWith="CallManagerPort">g_ccmPort</Properties>
      </treenode>
      <treenode text="g_registration" id="632472631815047984" vid="632451502184049694">
        <Properties type="Metreos.Applications.cBarge.RegistrationSession">g_registration</Properties>
      </treenode>
      <treenode text="g_sid" id="632472631815047986" vid="632452210714027827">
        <Properties type="String">g_sid</Properties>
      </treenode>
      <treenode text="g_applicationName" id="632472631815047988" vid="632452210714027862">
        <Properties type="String" defaultInitWith="cBarge">g_applicationName</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="632472631815047990" vid="632452445701826556">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
      <treenode text="g_connAttemptNum" id="632472631815047992" vid="632455776294324522">
        <Properties type="Int" initWith="ConnAttemptNumber">g_connAttemptNum</Properties>
      </treenode>
      <treenode text="g_mediaDisabled" id="632472631815047994" vid="632455776294324544">
        <Properties type="Bool" defaultInitWith="false">g_mediaDisabled</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnRegister" startnode="632451464064027781" treenode="632451464064027782" appnode="632451464064027779" handlerfor="632451464064027778">
    <node type="Start" id="632451464064027781" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="93" y="278">
      <linkto id="632451464064027783" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632451464064027783" name="Register" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="194" y="278">
      <linkto id="632452210714027826" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Sid" type="variable">sid</ap>
        <ap name="ToIp" type="variable">g_ccmIP</ap>
        <ap name="ToPort" type="variable">g_ccmPort</ap>
      </Properties>
    </node>
    <node type="Action" id="632451464064027787" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="438" y="277">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632452210714027826" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="299" y="278">
      <linkto id="632451464064027787" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">sid</ap>
        <ap name="Value2" type="variable">routingGuid</ap>
        <rd field="ResultData">g_sid</rd>
        <rd field="ResultData2">g_routingGuid</rd>
      </Properties>
    </node>
    <node type="Variable" id="632451464064027785" name="sid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Sid" refType="reference" name="Metreos.Providers.SccpProxy.Register">sid</Properties>
    </node>
    <node type="Variable" id="632451464064027786" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRegisterAck" startnode="632451464064027795" treenode="632451464064027796" appnode="632451464064027793" handlerfor="632451464064027792">
    <node type="Start" id="632451464064027795" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="87" y="313">
      <linkto id="632451464064027798" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632451464064027798" name="RegisterAck" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="190" y="313">
      <linkto id="632458035790033991" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Sid" type="variable">g_sid</ap>
      </Properties>
    </node>
    <node type="Action" id="632451464064027799" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="531" y="313">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632458035790033991" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="291" y="314">
      <linkto id="632459598713239037" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SleepTime" type="literal">1000</ap>
      </Properties>
    </node>
    <node type="Action" id="632459598713239037" name="RemoveCallRecord" class="MaxActionNode" group="" path="Metreos.Applications.cBarge.NativeActions" x="408" y="314">
      <linkto id="632451464064027799" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Sid" type="variable">g_sid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnRegisterAck: Removing stale database records for device: " + g_sid</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionFailure" startnode="632451464064027803" treenode="632451464064027804" appnode="632451464064027801" handlerfor="632451464064027800">
    <node type="Start" id="632451464064027803" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="165" y="402">
      <linkto id="632463971638705260" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632451464064027840" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="636" y="404">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632455736287139047" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="313.234375" y="388" mx="365" my="404">
      <items count="1">
        <item text="CleanUpSessions" />
      </items>
      <linkto id="632466131781361384" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="FunctionName" type="literal">CleanUpSessions</ap>
      </Properties>
    </node>
    <node type="Action" id="632463971638705260" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="259" y="403">
      <linkto id="632455736287139047" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="csharp">"OnSessionFailure: Session: " + g_sid + ": session failed, calling CleanUpConnections."</ap>
        <ap name="LogLevel" type="literal">Warning</ap>
        <ap name="ApplicationName" type="literal">cBarge</ap>
      </Properties>
    </node>
    <node type="Action" id="632466131781361384" name="RemoveCallRecord" class="MaxActionNode" group="" path="Metreos.Applications.cBarge.NativeActions" x="502.877563" y="405">
      <linkto id="632451464064027840" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Sid" type="variable">g_sid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnRegisterAck: Removing stale database records for device: " + g_sid</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnOpenReceiveChannel" startnode="632451464064027808" treenode="632451464064027809" appnode="632451464064027806" handlerfor="632451464064027805">
    <node type="Start" id="632451464064027808" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="259">
      <linkto id="632451464064027848" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632451464064027848" name="OpenReceiveChannel" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="176" y="259">
      <linkto id="632452210714027879" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Sid" type="variable">g_sid</ap>
      </Properties>
    </node>
    <node type="Action" id="632452210714027879" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="315" y="259">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632451890696747149" name="callRef" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="CallReference" refType="reference" name="Metreos.Providers.SccpProxy.OpenReceiveChannel">callRef</Properties>
    </node>
    <node type="Variable" id="632452350078979972" name="mediaConnectionsCreated" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" defaultInitWith="false" refType="reference">mediaConnectionsCreated</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnCloseReceiveChannel" startnode="632451464064027813" treenode="632451464064027814" appnode="632451464064027811" handlerfor="632451464064027810">
    <node type="Start" id="632451464064027813" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="94" y="298">
      <linkto id="632451464064027850" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632451464064027850" name="CloseReceiveChannel" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="236" y="298">
      <linkto id="632453967625239802" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Sid" type="variable">g_sid</ap>
      </Properties>
    </node>
    <node type="Action" id="632451464064027852" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="514" y="299">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632453967625239802" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="388" y="298">
      <linkto id="632451464064027852" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref RegistrationSession g_registration, int callRef)
{
	if (g_registration.CallSessions[callRef].MediaConnectionsCreated)
	{
		g_registration.CallSessions[callRef].Connection.Local.TxOpen = false;
	}
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Variable" id="632454165681352859" name="callRef" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="CallReference" refType="reference" name="Metreos.Providers.SccpProxy.CloseReceiveChannel">callRef</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopMediaTransmission" startnode="632451464064027818" treenode="632451464064027819" appnode="632451464064027816" handlerfor="632451464064027815">
    <node type="Start" id="632451464064027818" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="120" y="293">
      <linkto id="632451464064027870" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632451464064027870" name="StopMediaTransmission" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="242" y="293">
      <linkto id="632453967625239801" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Sid" type="variable">g_sid</ap>
      </Properties>
    </node>
    <node type="Action" id="632451464064027871" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="502" y="291">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632453967625239801" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="377" y="292">
      <linkto id="632451464064027871" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(int callRef, ref RegistrationSession g_registration)
{
	if (g_registration.CallSessions[callRef].MediaConnectionsCreated)
	{
		g_registration.CallSessions[callRef].Connection.Local.RxOpen = false;
	}
	return IApp.VALUE_SUCCESS;
}

</Properties>
    </node>
    <node type="Variable" id="632453967625239800" name="callRef" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="CallReference" refType="reference" name="Metreos.Providers.SccpProxy.StopMediaTransmission">callRef</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnCallInfo" startnode="632451464064027823" treenode="632451464064027824" appnode="632451464064027821" handlerfor="632451464064027820">
    <node type="Start" id="632451464064027823" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="59" y="330">
      <linkto id="632451464064027895" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632451464064027893" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="285" y="329">
      <linkto id="632451464064027896" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="false" level="Info" type="csharp">"OnCallInfo: CustomCode entry: " + System.DateTime.Now.ToString("hh:mm:ss:ffff")
</log>
        <log condition="exit" on="false" level="Info" type="csharp">"OnCallInfo: CustomCode exit: " + System.DateTime.Now.ToString("hh:mm:ss:ffff")

</log>public static string Execute(string callingParty, string calledParty, string originalCalledParty, int callInstance, string lastRedirectingParty, string lastRedirectingReason, string callType, ref Hashtable eventParams, ref RegistrationSession g_registration, int callRef)
{
		CallSession callSession = g_registration.CallSessions[callRef];
		callSession.LastRedirectingReason = lastRedirectingReason;
		callSession.LastRedirectingParty = lastRedirectingParty;
		callSession.CallInstance = callInstance;
		callSession.CallingParty = callingParty;
		callSession.CalledParty = calledParty;
		callSession.OriginalCalledParty = originalCalledParty;
		callSession.CallType = callType;

		// handle forward case
		if (callType.Equals("Outbound"))
		{
			callSession.DirectoryNumber = callingParty;
			if ((calledParty.Length &gt; 0) &amp;&amp; calledParty.StartsWith("*"))
			{
				try
				{
					if (calledParty.Substring(1, calledParty.Length - 2).Equals(callingParty))
						callSession.WriteBargeRecord = false;
				}
				catch 
				{}						
			}
		}
		else
			callSession.DirectoryNumber = calledParty;

		return IApp.VALUE_SUCCESS;
}

</Properties>
    </node>
    <node type="Action" id="632451464064027895" name="CallInfo" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="168" y="330">
      <linkto id="632451464064027893" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Sid" type="variable">g_sid</ap>
        <log condition="entry" on="false" level="Info" type="csharp">"OnCallInfo: Session: " + g_sid + ": CallInfo entry: " + System.DateTime.Now.ToString("hh:mm:ss:ffff")</log>
        <log condition="exit" on="false" level="Info" type="csharp">"OnCallInfo: CallInfo exit: " + System.DateTime.Now.ToString("hh:mm:ss:ffff")</log>
      </Properties>
    </node>
    <node type="Action" id="632451464064027896" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="414" y="327">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632451464064027882" name="callRef" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="CallReference" refType="reference" name="Metreos.Providers.SccpProxy.CallInfo">callRef</Properties>
    </node>
    <node type="Variable" id="632451464064027883" name="inputA" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">inputA</Properties>
    </node>
    <node type="Variable" id="632451464064027884" name="inputB" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">inputB</Properties>
    </node>
    <node type="Variable" id="632451464064027885" name="eventParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Hashtable" refType="reference">eventParams</Properties>
    </node>
    <node type="Variable" id="632451464064027886" name="callingParty" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallingParty" refType="reference" name="Metreos.Providers.SccpProxy.CallInfo">callingParty</Properties>
    </node>
    <node type="Variable" id="632451464064027887" name="calledParty" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CalledParty" refType="reference" name="Metreos.Providers.SccpProxy.CallInfo">calledParty</Properties>
    </node>
    <node type="Variable" id="632451464064027888" name="originalCalledParty" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="OriginalCalledParty" refType="reference" name="Metreos.Providers.SccpProxy.CallInfo">originalCalledParty</Properties>
    </node>
    <node type="Variable" id="632451464064027889" name="callInstance" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="CallInstance" refType="reference" name="Metreos.Providers.SccpProxy.CallInfo">callInstance</Properties>
    </node>
    <node type="Variable" id="632451464064027890" name="lastRedirectingParty" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="LastRedirectingParty" refType="reference" name="Metreos.Providers.SccpProxy.CallInfo">lastRedirectingParty</Properties>
    </node>
    <node type="Variable" id="632451464064027891" name="lastRedirectingReason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="LastRedirectingReason" refType="reference" name="Metreos.Providers.SccpProxy.CallInfo">lastRedirectingReason</Properties>
    </node>
    <node type="Variable" id="632451464064027892" name="callType" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallType" refType="reference" name="Metreos.Providers.SccpProxy.CallInfo">callType</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnCallState" startnode="632451464064027828" treenode="632451464064027829" appnode="632451464064027826" handlerfor="632451464064027825">
    <node type="Start" id="632451464064027828" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="173" y="77">
      <linkto id="632451464064027879" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632451464064027877" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="437" y="76">
      <linkto id="632451464064027880" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632453967625239805" type="Labeled" style="Bezier" ortho="true" label="delete" />
      <linkto id="632453967625239812" type="Labeled" style="Bezier" ortho="true" label="end" />
      <linkto id="632457368103348295" type="Labeled" style="Bezier" ortho="true" label="get_db" />
      <linkto id="632459134416982021" type="Labeled" style="Bezier" ortho="true" label="esm_db" />
      <linkto id="632470214321733458" type="Labeled" style="Bezier" ortho="true" label="del_media" />
      <Properties language="csharp">
        <log condition="entry" on="false" level="Info" type="csharp">"OnCallState: CustomCode entry: " + System.DateTime.Now.ToString("hh:mm:ss:ffff")</log>
        <log condition="exit" on="false" level="Info" type="csharp">"OnCallState: CustomCode exit: " + System.DateTime.Now.ToString("hh:mm:ss:ffff")</log>public static string Execute(int lineInstance, int callRef, ref RegistrationSession g_registration, string callState, LogWriter log, ref string g_sid)
{
	CallSession callSession = g_registration.CallSessions[callRef];

	callSession.LineInstance = lineInstance;
	callSession.CallReference = callRef;
	string previousState = callSession.CurrentState.ToString();

	bool mediaCreated = callSession.MediaConnectionsCreated;
	bool mediaFailure = callSession.MediaFailureOccured;

	//log.Write(TraceLevel.Info, "!OnCallState: Session: " + g_sid + ": the value of MediaConnectionsCreated is: " + mediaCreated.ToString());
	switch(callState)
	{
		case "RingIn" : 
				/*  Trying to conserve MMS connections for incoming calls
				if (!mediaCreated &amp;&amp; (g_registration.ConnectionPool.Size == 0)) 
					return "esm"; 
				else
				*/
					return "default"; 
		case "RingOut" : 
				/*if (!mediaCreated) 
					return "esm"; 
				else*/
					return "default"; 
		case "Connected" : 
				callSession.CurrentState = (CallSession.States)Enum.Parse(typeof(CallSession.States), callState);
				return "default";
		case "CallRemoteMultiline" :
				callSession.CurrentState = (CallSession.States)Enum.Parse(typeof(CallSession.States), callState);
				callSession.WriteDbRecord = callSession.WriteBargeRecord = false;

				if (previousState.Equals("Hold"))
				{
					if (mediaCreated)
						return "del_media";
					else
						return "default";
				}
				else
				{
					// Forgetting about something?
					if (!mediaCreated)
						return "default";
					else
						return "del_media";
				}

					return "default";

		case "Hold" :
				if (previousState.Equals("CallRemoteMultiline"))
				{
					callSession.WriteDbRecord = callSession.WriteBargeRecord = false;
					if (mediaFailure)
						return "default";
					else if (!mediaCreated)
						return "esm_db";
					else
						return "get_db";
				}
				return "default";					
		case "OnHook" : 
				if (mediaCreated)
					return "delete";
				return "end";
		default : return "default";	
	}
}

</Properties>
    </node>
    <node type="Action" id="632451464064027879" name="CallState" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="308" y="77">
      <linkto id="632451464064027877" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Sid" type="variable">g_sid</ap>
        <log condition="entry" on="false" level="Info" type="csharp">"OnCallState: Session: " + g_sid + ": CallState entry: " + System.DateTime.Now.ToString("hh:mm:ss:ffff")</log>
        <log condition="exit" on="false" level="Info" type="csharp">"OnCallState: CallState exit: " + System.DateTime.Now.ToString("hh:mm:ss:ffff")</log>
      </Properties>
    </node>
    <node type="Action" id="632451464064027880" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="689" y="75">
      <Properties final="true" type="appControl">
        <log condition="entry" on="false" level="Info" type="csharp">(g_depth--).ToString() + "\tOnCallState: Function exit: " + System.DateTime.Now.ToString("hh:mm:ss:ffff");
</log>
      </Properties>
    </node>
    <node type="Action" id="632453967625239803" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="500" y="700">
      <linkto id="632453967625239804" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">localConnectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632453967625239804" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="625" y="700">
      <linkto id="632453967625239807" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">remoteConnectionId</ap>
      </Properties>
    </node>
    <node type="Label" id="632453967625239805" text="d" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="582" y="235" />
    <node type="Label" id="632453967625239806" text="d" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="301" y="698">
      <linkto id="632454165681352858" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632453967625239807" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="625" y="799">
      <linkto id="632454754887095910" type="Labeled" style="Bezier" ortho="true" label="delDB" />
      <linkto id="632453967625239809" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(int callRef, ref RegistrationSession g_registration)
{
	CallSession callSession = g_registration.CallSessions[callRef];
/*	if (callSession.DbRecordWritten)
	{*/
		g_registration.CallSessions.RemoveSession(callRef);
		return "delDB";
	/*}
	g_registration.CallSessions.RemoveSession(callRef);
	return IApp.VALUE_SUCCESS;*/
}
</Properties>
    </node>
    <node type="Comment" id="632453967625239808" text="don't forget to remove db record" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="451" y="631" />
    <node type="Action" id="632453967625239809" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="741" y="921">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632453967625239810" text="possible to get messages after this point which&#xD;&#xA;will re-create record for callRef?" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="645" y="631" />
    <node type="Label" id="632453967625239811" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="528" y="798">
      <linkto id="632453967625239807" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632453967625239812" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="247" y="222" />
    <node type="Action" id="632454165681352858" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="388" y="699">
      <linkto id="632453967625239803" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="csharp">"!OnCallState: Session: " + g_sid + ": Deleting connection for callRef: " + callRef.ToString()</log>
public static string Execute(int callRef, ref RegistrationSession g_registration, ref string callState, ref string localConnectionId, ref string remoteConnectionId)
{
	CallSession callSession = g_registration.CallSessions[callRef];
	localConnectionId = callSession.Connection.Remote.ConnectionId;
	remoteConnectionId = callSession.Connection.Local.ConnectionId;
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632454754887095910" name="RemoveCallRecord" class="MaxActionNode" group="" path="Metreos.Applications.cBarge.NativeActions" x="741" y="799">
      <linkto id="632453967625239809" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="CallReference" type="variable">callRef</ap>
        <ap name="Sid" type="variable">g_sid</ap>
        <ap name="LineInstance" type="variable">lineInstance</ap>
        <ap name="RoutingGuid" type="variable">g_routingGuid</ap>
      </Properties>
    </node>
    <node type="Action" id="632455736287139053" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1113" y="219">
      <linkto id="632457368103348300" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632460607443655379" type="Labeled" style="Bezier" ortho="true" label="MediaFailure" />
      <linkto id="632470063913234299" type="Labeled" style="Bezier" ortho="true" label="esm" />
      <Properties language="csharp">public static string Execute(int callRef, ref RegistrationSession g_registration, LogWriter log, ref string g_sid)
{
	CallSession callSession = g_registration.CallSessions[callRef];

	if (callSession.MediaFailureOccured)
	{
		log.Write(TraceLevel.Info, "OnCallState: Session: " + g_sid + ": MediaFailure occured, not attempting to process media for this call session.");
		return "MediaFailure";
	}

	log.Write(TraceLevel.Info, "!OnCallState: Session " + g_sid + ": obtaining connection for callRef: " + callRef.ToString());

	if (!callSession.MediaConnectionsCreated)
		return "esm";

	return IApp.VALUE_SUCCESS;
}


</Properties>
    </node>
    <node type="Label" id="632457368103348295" text="g" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="690.5371" y="155" />
    <node type="Label" id="632457368103348296" text="g" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1418.53711" y="139">
      <linkto id="632457368103348300" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632457368103348297" name="GetCallRecords" class="MaxActionNode" group="" path="Metreos.Applications.cBarge.NativeActions" x="1543.53711" y="219">
      <linkto id="632457368103348301" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632458429553623014" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632466097545477826" type="Labeled" style="Bezier" ortho="true" label="NotFound" />
      <Properties final="false" type="native">
        <ap name="DirectoryNumber" type="variable">directoryNumber</ap>
        <ap name="CallReference" type="variable">callRef</ap>
        <ap name="CallInstance" type="variable">callInstance</ap>
        <ap name="LineInstance" type="variable">lineInstance</ap>
        <ap name="SortOrder" type="literal">DESC</ap>
        <rd field="CallRecordsTable">callRecordsTable</rd>
        <log condition="default" on="true" level="Info" type="csharp">"OnCallState: Session: " + g_sid + ": could not obtain call record from database"</log>
      </Properties>
    </node>
    <node type="Action" id="632457368103348300" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1420.53711" y="220">
      <linkto id="632457368103348297" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632458429553623014" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(ref int callInstance, ref string directoryNumber, ref RegistrationSession g_registration, int callRef)
{
		CallSession callSession = g_registration.CallSessions[callRef];
		
		try
		{		
			directoryNumber = callSession.DirectoryNumber;
			callInstance = callSession.CallInstance;
			return IApp.VALUE_SUCCESS;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
}


</Properties>
    </node>
    <node type="Action" id="632457368103348301" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1663.398" y="219">
      <linkto id="632458429553623014" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <linkto id="632459829755488007" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632459670591860497" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">	public static string Execute(DataTable callRecordsTable, ref string conferenceId, ref string localConnectionId, ref string remoteConnectionId, ref int mmsId, RegistrationSession g_registration, int callRef, LogWriter log, ref string confToDelete)
	{
		try
		{
			CallSession callSession = g_registration.CallSessions[callRef];
			DataRow row = callRecordsTable.Rows[0];
			conferenceId = row[CBargeCallRecords.ConferenceId] as string;
			mmsId = Convert.ToInt32(row[CBargeCallRecords.MmsId]);
			bool success = false;

			if (conferenceId != null)
			{
				if (callSession.MediaConnectionsCreated)
				{
					localConnectionId = callSession.Connection.Local.ConnectionId;
					remoteConnectionId = callSession.Connection.Remote.ConnectionId;
					success = (localConnectionId != null) &amp;&amp; (remoteConnectionId != null);
					
			
					if (conferenceId.Equals(callSession.Connection.ConferenceId))
					{
						callSession.UpdateDbRecord = true;
						callSession.UpdateBargeRecord = false;
						return "default";
					}
					confToDelete = callSession.Connection.ConferenceId;
					callSession.Connection.ConferenceId = conferenceId;
				}
			}	
			return success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
		}
		catch
		{
			log.Write(TraceLevel.Info, "Exception");
			return IApp.VALUE_FAILURE;
		}
	}


</Properties>
    </node>
    <node type="Comment" id="632457368103348640" text="TODO: Address possibility that conference exists on different MMS" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1655.25586" y="163" />
    <node type="Action" id="632457368103348643" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="2042.30151" y="218">
      <linkto id="632457368103348644" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632460607443655380" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">localConnectionId</ap>
        <ap name="ConferenceId" type="variable">conferenceId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnCallState: Session: " + g_sid + ": moving connection: " + localConnectionId + " to conference: " + conferenceId</log>
      </Properties>
    </node>
    <node type="Action" id="632457368103348644" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="2153.3042" y="218">
      <linkto id="632460607443655380" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632466457484345184" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">remoteConnectionId</ap>
        <ap name="ConferenceId" type="variable">conferenceId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnCallState: Session: " + g_sid + ": moving connection: " + remoteConnectionId + " to conference: " + conferenceId</log>
      </Properties>
    </node>
    <node type="Action" id="632457368103348645" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2403.3042" y="217">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632458429553623014" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1544.7749" y="357">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Error" type="literal">OnCallState: connection parameters could NOT be retrieved</log>
      </Properties>
    </node>
    <node type="Label" id="632459134416982021" text="h" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="693" y="225" />
    <node type="Label" id="632459134416982022" text="h" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1006" y="219">
      <linkto id="632455736287139053" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632459670591860497" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1662.7749" y="369">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnCallState: connections are already in proper conference. </log>
      </Properties>
    </node>
    <node type="Action" id="632459829755488007" name="LeaveConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="1784.7749" y="219">
      <linkto id="632459829755488008" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">localConnectionId</ap>
        <ap name="ConferenceId" type="variable">confToDelete</ap>
      </Properties>
    </node>
    <node type="Action" id="632459829755488008" name="LeaveConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="1917.7749" y="218">
      <linkto id="632457368103348643" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">remoteConnectionId</ap>
        <ap name="ConferenceId" type="variable">confToDelete</ap>
      </Properties>
    </node>
    <node type="Action" id="632460607443655379" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1114" y="343">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632460607443655380" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="2100.775" y="323">
      <linkto id="632460607443655381" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(int callRef, ref RegistrationSession g_registration, LogWriter log, string g_sid)
{
	try
	{
		g_registration.CallSessions[callRef].MediaFailureOccured = true;
		log.Write(TraceLevel.Warning, "OnCallState: Session: " + g_sid + ": Media failure occured, not processing media for this call session.");
		return IApp.VALUE_SUCCESS;
	}
	catch
	{
		return IApp.VALUE_FAILURE;
	}
}

</Properties>
    </node>
    <node type="Action" id="632460607443655381" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2102.775" y="434">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632466097545477826" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1542" y="96">
      <linkto id="632466097545477827" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(ref RegistrationSession g_registration, int callRef)
{
		try
		{
			g_registration.CallSessions[callRef].WriteDbRecord = true;
			g_registration.CallSessions[callRef].WriteBargeRecord = true;
			return IApp.VALUE_SUCCESS;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
}
</Properties>
    </node>
    <node type="Action" id="632466097545477827" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1657" y="96">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632466097545477828" text="There needs to be an UpdateCallRecord here" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="2117.55078" y="387" />
    <node type="Action" id="632466457484345184" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="2272.775" y="218">
      <linkto id="632457368103348645" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(RegistrationSession g_registration, int callRef)
{
	g_registration.CallSessions[callRef].UpdateDbRecord = true;
	g_registration.CallSessions[callRef].UpdateBargeRecord = true;
	return IApp.VALUE_SUCCESS;
}</Properties>
    </node>
    <node type="Action" id="632470063913234299" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1273" y="328" mx="1318" my="344">
      <items count="1">
        <item text="EstablishMedia" />
      </items>
      <linkto id="632457368103348300" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632460607443655379" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="callRef" type="variable">callRef</ap>
        <ap name="FunctionName" type="literal">EstablishMedia</ap>
      </Properties>
    </node>
    <node type="Label" id="632470214321733458" text="i" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="401" y="226" />
    <node type="Label" id="632470214321733459" text="i" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="65" y="488">
      <linkto id="632470214321733462" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632470214321733460" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="306" y="488">
      <linkto id="632470214321733461" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">localConnectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632470214321733461" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="437" y="488">
      <linkto id="632470214321733466" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">remoteConnectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632470214321733462" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="176" y="488">
      <linkto id="632470214321733460" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="csharp">"!OnCallState: Session: " + g_sid + ": Deleting connection for callRef: " + callRef.ToString()</log>
public static string Execute(int callRef, ref RegistrationSession g_registration, ref string callState, ref string localConnectionId, ref string remoteConnectionId)
{
	CallSession callSession = g_registration.CallSessions[callRef];
	callSession.MediaConnectionsCreated = false;
	localConnectionId = callSession.Connection.Remote.ConnectionId;
	remoteConnectionId = callSession.Connection.Local.ConnectionId;
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632470214321733466" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="553" y="487">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632451464064027873" name="callState" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallState" refType="reference" name="Metreos.Providers.SccpProxy.CallState">callState</Properties>
    </node>
    <node type="Variable" id="632451464064027874" name="callRef" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="CallReference" refType="reference" name="Metreos.Providers.SccpProxy.CallState">callRef</Properties>
    </node>
    <node type="Variable" id="632451464064027875" name="lineInstance" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="LineInstance" refType="reference" name="Metreos.Providers.SccpProxy.CallState">lineInstance</Properties>
    </node>
    <node type="Variable" id="632453864792265136" name="localConnectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="0" refType="reference">localConnectionId</Properties>
    </node>
    <node type="Variable" id="632453864792265137" name="localRxIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">localRxIP</Properties>
    </node>
    <node type="Variable" id="632453864792265138" name="localRxPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">localRxPort</Properties>
    </node>
    <node type="Variable" id="632453864792265139" name="remoteConnectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="0" refType="reference">remoteConnectionId</Properties>
    </node>
    <node type="Variable" id="632453864792265140" name="remoteRxIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">remoteRxIP</Properties>
    </node>
    <node type="Variable" id="632453864792265141" name="remoteRxPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">remoteRxPort</Properties>
    </node>
    <node type="Variable" id="632453864792265142" name="mmsId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">mmsId</Properties>
    </node>
    <node type="Variable" id="632453864792265143" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="0" refType="reference">conferenceId</Properties>
    </node>
    <node type="Variable" id="632457368103348298" name="callInstance" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">callInstance</Properties>
    </node>
    <node type="Variable" id="632457368103348299" name="directoryNumber" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">directoryNumber</Properties>
    </node>
    <node type="Variable" id="632457368103348302" name="callRecordsTable" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">callRecordsTable</Properties>
    </node>
    <node type="Variable" id="632459760653025970" name="confToDelete" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">confToDelete</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartMediaTransmission" activetab="true" startnode="632451464064027833" treenode="632451464064027834" appnode="632451464064027831" handlerfor="632451464064027830">
    <node type="Start" id="632451464064027833" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="328">
      <linkto id="632453096423948363" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632451464064027868" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="999" y="325">
      <Properties final="true" type="appControl">
        <log condition="entry" on="false" level="Info" type="csharp">(g_depth--).ToString() + "\tOnStartMediaTransmission: Function exit: " + System.DateTime.Now.ToString("hh:mm:ss:ffff");</log>
      </Properties>
    </node>
    <node type="Action" id="632453096423948363" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="237.90625" y="329.5">
      <linkto id="632471981725668449" type="Labeled" style="Bezier" label="esm" />
      <linkto id="632471981725668449" type="Labeled" style="Bezier" label="success" />
      <linkto id="632454165681352862" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="false" level="Info" type="csharp">"OnStartMediaTransmission: CustomCode entry: " + System.DateTime.Now.ToString("hh:mm:ss:ffff");
</log>
        <log condition="exit" on="false" level="Info" type="csharp">"OnStartMediaTransmission: CustomCode exit: " + System.DateTime.Now.ToString("hh:mm:ss:ffff");
</log>public static string Execute(int callRef, ref RegistrationSession g_registration, string TxIP, uint TxPort, ref string conferenceId)
{
		CallSession callSession = g_registration.CallSessions[callRef];
		
		if (callSession.MediaFailureOccured)
			return "default";
		
		Connection conn = callSession.Connection;
		if (conn == null)
			return "esm";
		else
		{
			if ((TxPort == 0) || (TxIP == null))
				return IApp.VALUE_FAILURE;
	
			conferenceId = conn.ConferenceId;
			return IApp.VALUE_SUCCESS;
		}
}
</Properties>
    </node>
    <node type="Action" id="632454165681352862" name="StartMediaTransmission" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="237" y="441">
      <linkto id="632470845070538048" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="MediaIp" type="csharp">string.Empty</ap>
        <ap name="MediaPort" type="literal">0</ap>
        <ap name="Sid" type="variable">g_sid</ap>
        <log condition="entry" on="false" level="Warning" type="csharp">"OnStartMediaTransmission(" + callRef.ToString() + "): Media failure detected, operating in proxy mode.";
</log>
      </Properties>
    </node>
    <node type="Action" id="632454165681352863" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="234" y="678">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632454592647767147" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="841" y="326">
      <linkto id="632451464064027868" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632454592647767149" type="Labeled" style="Bezier" ortho="true" label="db" />
      <Properties language="csharp">
public static string Execute(RegistrationSession g_registration, int callRef, ref string mode)
{
	CallSession callSession = g_registration.CallSessions[callRef];
	Connection conn = callSession.Connection;
	if (conn == null)
		return IApp.VALUE_FAILURE;

	conn.Local.RxOpen = true;
	if (conn.Local.TxOpen)
	{
		if (callSession.WriteDbRecord)
		{
			mode = "w_cr";
			return "db";
		}
		else if (callSession.UpdateDbRecord)
		{
			mode = callSession.UpdateBargeRecord ? "u_cr_br" : "u_cr";
			return "db";
		}
	}
	
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632454592647767149" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="778" y="473" mx="848" my="489">
      <items count="1">
        <item text="WriteRecordToDatabase" />
      </items>
      <linkto id="632451464064027868" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="callRef" type="variable">callRef</ap>
        <ap name="Mode" type="variable">mode</ap>
        <ap name="FunctionName" type="literal">WriteRecordToDatabase</ap>
      </Properties>
    </node>
    <node type="Label" id="632467350589690029" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="121" y="552">
      <linkto id="632470845070538048" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632467350589690042" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="395" y="224" />
    <node type="Action" id="632470845070538046" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="695.7855" y="327">
      <linkto id="632454592647767147" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="false" level="Info" type="csharp">"OnStartMediaTransmission: CustomCode entry: " + System.DateTime.Now.ToString("hh:mm:ss:ffff");

</log>
        <log condition="exit" on="false" level="Info" type="csharp">"OnStartMediaTransmission: CustomCode exit: " + System.DateTime.Now.ToString("hh:mm:ss:ffff");

</log>
public static string Execute(ref RegistrationSession g_registration, string localConnectionId, string localRxIP, uint localRxPort, string conferenceId, uint mmsId, string TxIP, uint TxPort, int callRef)
{
		try
		{
			Connection conn = null;
			if (g_registration.CallSessions[callRef].Connection == null)
			{
				conn = g_registration.CallSessions[callRef].Connection = new Connection();
				conn.MmsId = mmsId;
				conn.ConferenceId = conferenceId;
			}
			else
				conn = g_registration.CallSessions[callRef].Connection;

			conn.Remote.TxIP = TxIP;
			conn.Remote.TxPort = TxPort;

			conn.Local.RxIP = localRxIP;
			conn.Local.RxPort = localRxPort;
			conn.Local.ConnectionId = localConnectionId;

			g_registration.CallSessions[callRef].MediaConnectionsCreated = true;
			g_registration.CallSessions[callRef].MediaFailureOccured = false;
			return IApp.VALUE_SUCCESS;	
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}	
}

</Properties>
    </node>
    <node type="Action" id="632470845070538048" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="236.785522" y="551">
      <linkto id="632454165681352863" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref RegistrationSession g_registration, int callRef)
{
		try
		{
			g_registration.CallSessions[callRef].MediaFailureOccured = true;
			g_registration.CallSessions[callRef].MediaConnectionsCreated = false;
			return IApp.VALUE_SUCCESS;	
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}	
}


</Properties>
    </node>
    <node type="Action" id="632470845070538055" name="StartMediaTransmission" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="545" y="328">
      <linkto id="632470845070538046" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="MediaIp" type="variable">localRxIP</ap>
        <ap name="MediaPort" type="variable">localRxPort</ap>
        <ap name="Sid" type="variable">g_sid</ap>
        <log condition="entry" on="false" level="Info" type="csharp">"OnStartMediaTransmission: instructing phone behind proxy to send RTP data to: " + localRxIP + ":" + localRxPort</log>
      </Properties>
    </node>
    <node type="Action" id="632471981725668449" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="397" y="330">
      <linkto id="632467350589690042" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632470845070538055" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="ReTransmit" type="literal">true</ap>
        <ap name="MediaTxIP" type="variable">TxIP</ap>
        <ap name="MediaTxPort" type="variable">TxPort</ap>
        <ap name="ConnectionId" type="literal">0</ap>
        <rd field="MmsId">mmsId</rd>
        <rd field="MediaRxIP">localRxIP</rd>
        <rd field="MediaRxPort">localRxPort</rd>
        <rd field="ConnectionId">localConnectionId</rd>
        <log condition="entry" on="false" level="Info" type="csharp">"OnStartMediaTransmission: CreateConnection entry: " + System.DateTime.Now.ToString("hh:mm:ss:ffff");

</log>
        <log condition="exit" on="false" level="Info" type="csharp">"OnStartMediaTransmission: CreateConnection exit: " + System.DateTime.Now.ToString("hh:mm:ss:ffff");

</log>
      </Properties>
    </node>
    <node type="Variable" id="632451464064027864" name="callRef" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="CallReference" refType="reference" name="Metreos.Providers.SccpProxy.StartMediaTransmission">callRef</Properties>
    </node>
    <node type="Variable" id="632453096423948369" name="TxIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="MediaIp" refType="reference" name="Metreos.Providers.SccpProxy.StartMediaTransmission">TxIP</Properties>
    </node>
    <node type="Variable" id="632453096423948370" name="TxPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" initWith="MediaPort" refType="reference" name="Metreos.Providers.SccpProxy.StartMediaTransmission">TxPort</Properties>
    </node>
    <node type="Variable" id="632455679473026446" name="confCreated" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" defaultInitWith="true" refType="reference">confCreated</Properties>
    </node>
    <node type="Variable" id="632455679473026447" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="0" refType="reference">conferenceId</Properties>
    </node>
    <node type="Variable" id="632470845070538057" name="localConnectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">localConnectionId</Properties>
    </node>
    <node type="Variable" id="632470845070538060" name="localRxIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">localRxIP</Properties>
    </node>
    <node type="Variable" id="632470845070538061" name="localRxPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">localRxPort</Properties>
    </node>
    <node type="Variable" id="632470845070538062" name="mmsId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">mmsId</Properties>
    </node>
    <node type="Variable" id="632470875922226591" name="mode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">mode</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnOpenReceiveChannelAck" startnode="632451464064027838" treenode="632451464064027839" appnode="632451464064027836" handlerfor="632451464064027835">
    <node type="Start" id="632451464064027838" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="302">
      <linkto id="632453096423948374" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632451464064027860" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1042.07227" y="302">
      <Properties final="true" type="appControl">
        <log condition="entry" on="false" level="Info" type="csharp">(g_depth--).ToString() + "\tOnOpenReceiveChannelAck: Function exit: " + System.DateTime.Now.ToString("hh:mm:ss:ffff");</log>
      </Properties>
    </node>
    <node type="Action" id="632453096423948374" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="175.90625" y="301">
      <linkto id="632472007387147427" type="Labeled" style="Bezier" label="esm" />
      <linkto id="632472007387147427" type="Labeled" style="Bezier" label="success" />
      <linkto id="632454165681352860" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="false" level="Info" type="csharp">"OnOpenReceiveChannelAck: Custom Code entry: " + System.DateTime.Now.ToString("hh:mm:ss:ffff");</log>
        <log condition="exit" on="false" level="Info" type="csharp">"OnOpenReceiveChannelAck: Custom Code exit: " + System.DateTime.Now.ToString("hh:mm:ss:ffff");
</log>public static string Execute(int callRef, ref RegistrationSession g_registration, string TxIP, uint TxPort, ref string conferenceId)
{
		CallSession callSession = g_registration.CallSessions[callRef];
		
		if (callSession.MediaFailureOccured)
			return "default";
		
		Connection conn = callSession.Connection;
		if (conn == null)
			return "esm";
		else
		{
			if ((TxPort == 0) || (TxIP == null))
				return IApp.VALUE_FAILURE;
	
			conferenceId = conn.ConferenceId;
			return IApp.VALUE_SUCCESS;
		}
}

</Properties>
    </node>
    <node type="Action" id="632454165681352860" name="OpenReceiveChannelAck" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="178" y="407">
      <linkto id="632470845070538067" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Sid" type="variable">g_sid</ap>
        <ap name="MediaIp" type="csharp">string.Empty</ap>
        <ap name="MediaPort" type="literal">0</ap>
        <log condition="entry" on="false" level="Warning" type="csharp">"OnOpenReceiveChannelAck(" + callRef.ToString() + "): Media failure detected, operating in proxy mode.";</log>
      </Properties>
    </node>
    <node type="Action" id="632454165681352861" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="178" y="616">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632454592647767148" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="872.4707" y="301">
      <linkto id="632451464064027860" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632454666735455679" type="Labeled" style="Bezier" ortho="true" label="db" />
      <Properties language="csharp">
public static string Execute(RegistrationSession g_registration, int callRef, ref string mode)
{
	CallSession callSession = g_registration.CallSessions[callRef];
	Connection conn = callSession.Connection;
	if (conn == null)
		return IApp.VALUE_FAILURE;

	conn.Local.TxOpen = true;

	if (conn.Local.RxOpen)
	{
		if (callSession.WriteDbRecord)
		{
			mode = "w_cr";
			return "db";
		}
		else if (callSession.UpdateDbRecord)
		{
			mode = callSession.UpdateBargeRecord ? "u_cr_br" : "u_cr";
			return "db";
		}
	}
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632454666735455679" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="806.542969" y="444" mx="877" my="460">
      <items count="1">
        <item text="WriteRecordToDatabase" />
      </items>
      <linkto id="632451464064027860" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="callRef" type="variable">callRef</ap>
        <ap name="Mode" type="variable">mode</ap>
        <ap name="FunctionName" type="literal">WriteRecordToDatabase</ap>
      </Properties>
    </node>
    <node type="Label" id="632467350589690039" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="60" y="509.999969">
      <linkto id="632470845070538067" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632467350589690043" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="356" y="194" />
    <node type="Action" id="632470845070538065" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="674.90625" y="300.25">
      <linkto id="632454592647767148" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref RegistrationSession g_registration, string remoteConnectionId, string remoteRxIP, uint remoteRxPort, string conferenceId, uint mmsId, string TxIP, uint TxPort, int callRef)
{
		try
		{
			Connection conn = null;
			if (g_registration.CallSessions[callRef].Connection == null)
			{
				conn = g_registration.CallSessions[callRef].Connection = new Connection();
				conn.MmsId = mmsId;
				conn.ConferenceId = conferenceId;
			}
			else
				conn = g_registration.CallSessions[callRef].Connection;
			
			conn.Local.TxIP = TxIP;
			conn.Local.TxPort = TxPort;

			conn.Remote.RxIP = remoteRxIP;
			conn.Remote.RxPort = remoteRxPort;
			conn.Remote.ConnectionId = remoteConnectionId;

			g_registration.CallSessions[callRef].MediaConnectionsCreated = true;
			g_registration.CallSessions[callRef].MediaFailureOccured = false;
			return IApp.VALUE_SUCCESS;	
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}	
}


</Properties>
    </node>
    <node type="Action" id="632470845070538067" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="177.905762" y="508.25">
      <linkto id="632454165681352861" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref RegistrationSession g_registration, int callRef)
{
		try
		{
			g_registration.CallSessions[callRef].MediaFailureOccured = true;
			g_registration.CallSessions[callRef].MediaConnectionsCreated = false;
			return IApp.VALUE_SUCCESS;	
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}	
}


</Properties>
    </node>
    <node type="Action" id="632470845070538099" name="OpenReceiveChannelAck" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="510.90625" y="300">
      <linkto id="632470845070538065" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Sid" type="variable">g_sid</ap>
        <ap name="MediaIp" type="variable">remoteRxIP</ap>
        <ap name="MediaPort" type="variable">remoteRxPort</ap>
        <log condition="entry" on="false" level="Info" type="csharp">"OnOpenReceiveChannelAck: instructing phone NOT behind proxy to send RTP data to: " + remoteRxIP + ":" + remoteRxPort
</log>
        <log condition="exit" on="false" level="Info" type="csharp">"OnOpenReceiveChannelAck: OpenReceiveChannelAck exit: " + System.DateTime.Now.ToString("hh:mm:ss:ffff");

</log>
      </Properties>
    </node>
    <node type="Action" id="632472007387147427" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="356" y="300">
      <linkto id="632467350589690043" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632470845070538099" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="ReTransmit" type="literal">true</ap>
        <ap name="MediaTxIP" type="variable">TxIP</ap>
        <ap name="MediaTxPort" type="variable">TxPort</ap>
        <ap name="ConnectionId" type="literal">0</ap>
        <rd field="MmsId">mmsId</rd>
        <rd field="MediaRxIP">remoteRxIP</rd>
        <rd field="MediaRxPort">remoteRxPort</rd>
        <rd field="ConnectionId">remoteConnectionId</rd>
      </Properties>
    </node>
    <node type="Variable" id="632451464064027856" name="callRef" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="CallReference" refType="reference" name="Metreos.Providers.SccpProxy.OpenReceiveChannelAck">callRef</Properties>
    </node>
    <node type="Variable" id="632453096423948380" name="TxIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="MediaIp" refType="reference" name="Metreos.Providers.SccpProxy.OpenReceiveChannelAck">TxIP</Properties>
    </node>
    <node type="Variable" id="632453096423948381" name="TxPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" initWith="MediaPort" refType="reference" name="Metreos.Providers.SccpProxy.OpenReceiveChannelAck">TxPort</Properties>
    </node>
    <node type="Variable" id="632455679473026460" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="0" refType="reference">conferenceId</Properties>
    </node>
    <node type="Variable" id="632466457484345190" name="mode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">mode</Properties>
    </node>
    <node type="Variable" id="632470845070538074" name="remoteConnectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">remoteConnectionId</Properties>
    </node>
    <node type="Variable" id="632470845070538077" name="remoteRxIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">remoteRxIP</Properties>
    </node>
    <node type="Variable" id="632470845070538078" name="remoteRxPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">remoteRxPort</Properties>
    </node>
    <node type="Variable" id="632470845070538079" name="mmsId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">mmsId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnUnregister" startnode="632458302086556225" treenode="632458302086556226" appnode="632458302086556223" handlerfor="632458302086556222">
    <node type="Start" id="632458302086556225" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="45" y="299">
      <linkto id="632458302086556232" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632458302086556232" name="Unregister" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="146" y="299">
      <linkto id="632458302086556233" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Sid" type="variable">g_sid</ap>
      </Properties>
    </node>
    <node type="Action" id="632458302086556233" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="213.052734" y="283" mx="265" my="299">
      <items count="1">
        <item text="CleanUpSessions" />
      </items>
      <linkto id="632466131781361386" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="FunctionName" type="literal">CleanUpSessions</ap>
      </Properties>
    </node>
    <node type="Action" id="632458302086556235" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="504" y="297">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632466131781361386" name="RemoveCallRecord" class="MaxActionNode" group="" path="Metreos.Applications.cBarge.NativeActions" x="391.8776" y="297">
      <linkto id="632458302086556235" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Sid" type="variable">g_sid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnRegisterAck: Removing stale database records for device: " + g_sid</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnUnregisterAck" startnode="632458302086556230" treenode="632458302086556231" appnode="632458302086556228" handlerfor="632458302086556227">
    <node type="Start" id="632458302086556230" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="293">
      <linkto id="632458302086556236" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632458302086556236" name="UnregisterAck" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="131" y="294">
      <linkto id="632458302086556237" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Sid" type="variable">g_sid</ap>
      </Properties>
    </node>
    <node type="Action" id="632458302086556237" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="242" y="296">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="WriteRecordToDatabase" startnode="632452445701826544" treenode="632452445701826545" appnode="632452445701826542" handlerfor="632458302086556227">
    <node type="Start" id="632452445701826544" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="80" y="293">
      <linkto id="632452445701826547" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632452445701826547" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="335" y="298">
      <linkto id="632466457484345193" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632466457484345195" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">WriteRecordToDatabase: function entry</log>public static string Execute(int callRef, ref RegistrationSession g_registration, ref string conferenceId, ref string directoryNumber, ref int lineInstance, ref int callInstance, ref uint mmsId, ref bool writeBargeRecord)
{

		// Add error checking
		CallSession callSession = g_registration.CallSessions[callRef];
		if (callSession.Connection == null)
			return IApp.VALUE_FAILURE;
		writeBargeRecord = callSession.WriteBargeRecord;
		conferenceId = callSession.Connection.ConferenceId;
		mmsId = callSession.Connection.MmsId;
		lineInstance = callSession.LineInstance;
		callInstance = callSession.CallInstance;
		directoryNumber = callSession.DirectoryNumber;

		return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632452445701826558" name="CreateCallRecord" class="MaxActionNode" group="" path="Metreos.Applications.cBarge.NativeActions" x="591" y="171">
      <linkto id="632466457484345192" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="CallReference" type="variable">callRef</ap>
        <ap name="CallInstance" type="variable">callInstance</ap>
        <ap name="LineInstance" type="variable">lineInstance</ap>
        <ap name="DirectoryNumber" type="variable">directoryNumber</ap>
        <ap name="RoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="Sid" type="variable">g_sid</ap>
        <ap name="MmsId" type="variable">mmsId</ap>
        <ap name="ConferenceId" type="variable">conferenceId</ap>
        <ap name="WriteBargeRecord" type="variable">writeBargeRecord</ap>
        <rd field="Timestamp">timestamp</rd>
      </Properties>
    </node>
    <node type="Action" id="632452445701826559" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="856" y="300">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632466457484345185" name="UpdateCallRecord" class="MaxActionNode" group="" path="Metreos.Applications.cBarge.NativeActions" x="594.5508" y="429">
      <linkto id="632466457484345192" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="DirectoryNumber" type="variable">directoryNumber</ap>
        <ap name="CallReference" type="variable">callRef</ap>
        <ap name="UpdateBargeRecord" type="literal">true</ap>
        <ap name="InsertOnUpdateFail" type="literal">true</ap>
        <ap name="LineInstance" type="variable">lineInstance</ap>
        <ap name="RoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="Sid" type="variable">g_sid</ap>
        <ap name="MmsId" type="variable">mmsId</ap>
        <ap name="ConferenceId" type="variable">conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632466457484345186" name="UpdateCallRecord" class="MaxActionNode" group="" path="Metreos.Applications.cBarge.NativeActions" x="588.5508" y="298">
      <linkto id="632466457484345192" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="DirectoryNumber" type="variable">directoryNumber</ap>
        <ap name="CallReference" type="variable">callRef</ap>
        <ap name="UpdateBargeRecord" type="literal">false</ap>
        <ap name="LineInstance" type="variable">lineInstance</ap>
        <ap name="RoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="Sid" type="variable">g_sid</ap>
      </Properties>
    </node>
    <node type="Action" id="632466457484345192" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="745" y="301">
      <linkto id="632452445701826559" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(RegistrationSession g_registration, int callRef)
{
	CallSession callSession = g_registration.CallSessions[callRef];
	callSession.WriteDbRecord = callSession.WriteBargeRecord = false;
	callSession.UpdateDbRecord = callSession.UpdateBargeRecord = false;
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632466457484345193" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="447" y="299">
      <linkto id="632452445701826558" type="Labeled" style="Bezier" ortho="true" label="w_cr" />
      <linkto id="632466457484345186" type="Labeled" style="Bezier" ortho="true" label="u_cr" />
      <linkto id="632466457484345185" type="Labeled" style="Bezier" ortho="true" label="u_cr_br" />
      <linkto id="632466457484345194" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">mode</ap>
      </Properties>
    </node>
    <node type="Action" id="632466457484345194" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="448" y="431">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
        <log condition="entry" on="true" level="Warning" type="csharp">"WriteRecordToDatabase: Session: " + g_sid + ": Unknown database write mode specified. Record was not written."</log>
      </Properties>
    </node>
    <node type="Action" id="632466457484345195" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="335" y="430">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">failure</ap>
        <log condition="entry" on="true" level="Warning" type="csharp">"WriteRecordToDatabase: Session: " + g_sid + ": Error occured while obtaining call information. Record was not written."</log>
      </Properties>
    </node>
    <node type="Variable" id="632452445701826546" name="callRef" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="callRef" refType="reference">callRef</Properties>
    </node>
    <node type="Variable" id="632452445701826548" name="lineInstance" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">lineInstance</Properties>
    </node>
    <node type="Variable" id="632452445701826549" name="callInstance" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">callInstance</Properties>
    </node>
    <node type="Variable" id="632452445701826550" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">conferenceId</Properties>
    </node>
    <node type="Variable" id="632452445701826551" name="directoryNumber" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">directoryNumber</Properties>
    </node>
    <node type="Variable" id="632452462487469422" name="mmsId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">mmsId</Properties>
    </node>
    <node type="Variable" id="632452497320446723" name="timestamp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DateTime" refType="reference">timestamp</Properties>
    </node>
    <node type="Variable" id="632466097545477829" name="writeBargeRecord" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" defaultInitWith="true" refType="reference">writeBargeRecord</Properties>
    </node>
    <node type="Variable" id="632466457484345189" name="mode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Mode" defaultInitWith="w_cr" refType="reference">mode</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="CleanUpSessions" startnode="632455736287139050" treenode="632455736287139051" appnode="632455736287139048" handlerfor="632458302086556227">
    <node type="Start" id="632455736287139050" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="57" y="377">
      <linkto id="632458302086556210" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632455736287139052" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="266" y="494">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">CleanUpCOnnectionPool: No more connections to clean up</log>
      </Properties>
    </node>
    <node type="Action" id="632458302086556210" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="164" y="378">
      <linkto id="632458302086556212" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632455736287139052" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">CleanUpConnectionPool: Cleaning up connections...</log>
public static string Execute(RegistrationSession g_registration, ref int currentPoolSize)
{
	try
	{
		currentPoolSize = g_registration.ConnectionPool.Size;
		return IApp.VALUE_SUCCESS;
	}
	catch
	{
		return IApp.VALUE_FAILURE;
	}
}
</Properties>
    </node>
    <node type="Action" id="632458302086556212" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="266" y="378">
      <linkto id="632458302086556215" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632455736287139052" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">currentPoolSize &gt; 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632458302086556215" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="370" y="379">
      <linkto id="632458302086556216" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632458302086556221" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref RegistrationSession g_registration, ref string localConnectionId, ref string remoteConnectionId)
{
		Connection conn = g_registration.ConnectionPool.GetConnection();
		if (conn != null)
		{
			remoteConnectionId = conn.Remote.ConnectionId;
			localConnectionId = conn.Local.ConnectionId;
			return IApp.VALUE_SUCCESS;
		}

		return IApp.VALUE_FAILURE;
}

</Properties>
    </node>
    <node type="Action" id="632458302086556216" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="486" y="379">
      <linkto id="632458302086556217" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">remoteConnectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632458302086556217" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="616" y="379">
      <linkto id="632458302086556218" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">localConnectionId</ap>
      </Properties>
    </node>
    <node type="Label" id="632458302086556218" text="L" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="732" y="379" />
    <node type="Label" id="632458302086556219" text="L" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="164" y="269">
      <linkto id="632458302086556210" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632458302086556221" text="L" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="371" y="477" />
    <node type="Variable" id="632458302086556211" name="currentPoolSize" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">currentPoolSize</Properties>
    </node>
    <node type="Variable" id="632458302086556213" name="remoteConnectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">remoteConnectionId</Properties>
    </node>
    <node type="Variable" id="632458302086556214" name="localConnectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">localConnectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="EstablishMedia" startnode="632467350589690023" treenode="632467350589690024" appnode="632467350589690021" handlerfor="632458302086556227">
    <node type="Start" id="632467350589690023" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="294">
      <linkto id="632469997478901276" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632469997478901276" name="CreateConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="158.161072" y="294">
      <linkto id="632469997478901277" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632470063913234296" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="SoundToneOnJoin" type="literal">false</ap>
        <ap name="ConnectionId" type="literal">0</ap>
        <ap name="MediaTxIP" type="literal">127.0.0.1</ap>
        <ap name="MediaTxPort" type="literal">60000</ap>
        <rd field="MmsId">mmsId</rd>
        <rd field="MediaRxIP">localRxIP</rd>
        <rd field="MediaRxPort">localRxPort</rd>
        <rd field="ConnectionId">localConnectionId</rd>
        <rd field="ConferenceId">conferenceId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"EstablishMedia: CreateConference entry: " + System.DateTime.Now.ToString("hh:mm:ss:ffff");

</log>
        <log condition="exit" on="true" level="Info" type="csharp">string.Format("EstablishMedia: Local connId: {0}, IP: {1}, Port: {2}", localConnectionId, localRxIP, localRxPort)</log>
        <log condition="success" on="true" level="Info" type="csharp">"!EstablishMedia: Session: " + g_sid + ": ConferenceId: " + conferenceId


</log>
      </Properties>
    </node>
    <node type="Action" id="632469997478901277" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="288.4204" y="295">
      <linkto id="632470063913234290" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632470063913234292" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="literal">0</ap>
        <ap name="ConferenceId" type="variable">conferenceId</ap>
        <ap name="MediaTxIP" type="literal">127.0.0.1</ap>
        <ap name="MediaTxPort" type="literal">60002</ap>
        <rd field="ConnectionId">remoteConnectionId</rd>
        <rd field="MediaRxIP">remoteRxIP</rd>
        <rd field="MediaRxPort">remoteRxPort</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"EstablishMedia: JoinConference entry: " + System.DateTime.Now.ToString("hh:mm:ss:ffff");

</log>
        <log condition="exit" on="true" level="Info" type="csharp">string.Format("EstablishMedia: Remote connId: {0}, IP: {1}, Port: {2}", remoteConnectionId, remoteRxIP, remoteRxPort)</log>
      </Properties>
    </node>
    <node type="Action" id="632470063913234290" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="429" y="293">
      <linkto id="632470063913234294" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632470063913234292" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref RegistrationSession g_registration, string localConnectionId, string localRxIP, uint localRxPort, string remoteConnectionId, string remoteRxIP, uint remoteRxPort, string conferenceId, uint mmsId, int callRef, LogWriter log)
{
		try
		{
			Connection conn = new Connection();
			conn.MmsId = mmsId;
			conn.ConferenceId = conferenceId;
			
			log.Write(TraceLevel.Info, "EstablishMedia: ConferenceId: " + conferenceId);
			conn.Remote.RxIP = remoteRxIP;
			conn.Remote.RxPort = remoteRxPort;
			conn.Remote.ConnectionId = remoteConnectionId;
			log.Write(TraceLevel.Info, string.Format("EstablishMedia: Remote connId: {0}, IP: {1}, Port: {2}", remoteConnectionId, remoteRxIP, remoteRxPort));

			conn.Local.RxIP = localRxIP;
			conn.Local.RxPort = localRxPort;
			conn.Local.ConnectionId = localConnectionId;

			log.Write(TraceLevel.Info, string.Format("EstablishMedia: Local connId: {0}, IP: {1}, Port: {2}", localConnectionId, localRxIP, localRxPort));
			g_registration.CallSessions[callRef].Connection = conn;
			g_registration.CallSessions[callRef].MediaConnectionsCreated = true;
			g_registration.CallSessions[callRef].MediaFailureOccured = false;
			return IApp.VALUE_SUCCESS;	
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}	
}

</Properties>
    </node>
    <node type="Action" id="632470063913234292" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="289" y="392">
      <linkto id="632470063913234296" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConferenceId" type="variable">conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632470063913234294" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="560" y="293">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">success</ap>
      </Properties>
    </node>
    <node type="Action" id="632470063913234295" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="158" y="511">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">failure</ap>
        <log condition="entry" on="true" level="Info" type="literal">EstablishMedia: Failure establishing media...</log>
      </Properties>
    </node>
    <node type="Action" id="632470063913234296" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="159" y="392">
      <linkto id="632470063913234295" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref RegistrationSession g_registration, int callRef)
{
		try
		{
			g_registration.CallSessions[callRef].MediaFailureOccured = true;
			g_registration.CallSessions[callRef].MediaConnectionsCreated = false;
			return IApp.VALUE_SUCCESS;	
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}	
}


</Properties>
    </node>
    <node type="Variable" id="632470063913234282" name="localConnectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">localConnectionId</Properties>
    </node>
    <node type="Variable" id="632470063913234283" name="remoteConnectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">remoteConnectionId</Properties>
    </node>
    <node type="Variable" id="632470063913234284" name="localRxIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">localRxIP</Properties>
    </node>
    <node type="Variable" id="632470063913234285" name="localRxPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">localRxPort</Properties>
    </node>
    <node type="Variable" id="632470063913234286" name="remoteRxIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">remoteRxIP</Properties>
    </node>
    <node type="Variable" id="632470063913234287" name="remoteRxPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">remoteRxPort</Properties>
    </node>
    <node type="Variable" id="632470063913234288" name="mmsId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">mmsId</Properties>
    </node>
    <node type="Variable" id="632470063913234289" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">conferenceId</Properties>
    </node>
    <node type="Variable" id="632470063913234291" name="callRef" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="callRef" defaultInitWith="0" refType="reference">callRef</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>