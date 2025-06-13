<Application name="RecWithBargeControl" trigger="Metreos.Events.RecWithBarge.RecWithBargeCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="RecWithBargeControl">
    <outline>
      <treenode type="evh" id="632641167141715444" level="1" text="Metreos.Events.RecWithBarge.RecWithBargeCall (trigger): OnRecWithBargeCall">
        <node type="function" name="OnRecWithBargeCall" id="632641167141715441" path="Metreos.StockTools" />
        <node type="event" name="RecWithBargeCall" id="632641167141715440" path="Metreos.Events.RecWithBarge.RecWithBargeCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632641251906402697" level="2" text="Metreos.Providers.JTapi.JTapiCallActive: OnJTapiCallActive">
        <node type="function" name="OnJTapiCallActive" id="632641251906402694" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallActive" id="632641251906402693" path="Metreos.Providers.JTapi.JTapiCallActive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632641251906402702" level="2" text="Metreos.Providers.JTapi.JTapiCallInactive: OnJTapiCallInactive">
        <node type="function" name="OnJTapiCallInactive" id="632641251906402699" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallInactive" id="632641251906402698" path="Metreos.Providers.JTapi.JTapiCallInactive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632642785229682347" level="2" text="Metreos.Providers.JTapi.JTapiHangup: OnJTapiHangup">
        <node type="function" name="OnJTapiHangup" id="632642785229682344" path="Metreos.StockTools" />
        <node type="event" name="JTapiHangup" id="632642785229682343" path="Metreos.Providers.JTapi.JTapiHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632643055809088410" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632643055809088407" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632643055809088406" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632643055809088415" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632643055809088412" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632643055809088411" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632643055809088420" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632643055809088417" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632643055809088416" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632643055809088425" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632643055809088422" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632643055809088421" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632643055809088430" level="2" text="Metreos.CallControl.GotDigits: OnGotDigits">
        <node type="function" name="OnGotDigits" id="632643055809088427" path="Metreos.StockTools" />
        <node type="event" name="GotDigits" id="632643055809088426" path="Metreos.CallControl.GotDigits" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632643055809088435" level="2" text="Metreos.CallControl.CallChanged: OnCallChanged">
        <node type="function" name="OnCallChanged" id="632643055809088432" path="Metreos.StockTools" />
        <node type="event" name="CallChanged" id="632643055809088431" path="Metreos.CallControl.CallChanged" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="632767263316803250" vid="632641251906402676">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_lineId" id="632767263316803252" vid="632641251906402678">
        <Properties type="String">g_lineId</Properties>
      </treenode>
      <treenode text="g_to" id="632767263316803254" vid="632641251906402680">
        <Properties type="String">g_to</Properties>
      </treenode>
      <treenode text="g_originalTo" id="632767263316803256" vid="632641251906402682">
        <Properties type="String">g_originalTo</Properties>
      </treenode>
      <treenode text="g_from" id="632767263316803258" vid="632641251906402684">
        <Properties type="String">g_from</Properties>
      </treenode>
      <treenode text="g_callType" id="632767263316803260" vid="632641251906402686">
        <Properties type="String">g_callType</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="632767263316803262" vid="632641251906402688">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632767263316803264" vid="632642785229682328">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632767263316803266" vid="632642785229682330">
        <Properties type="String">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_mmsId" id="632767263316803268" vid="632642785229682332">
        <Properties type="UInt">g_mmsId</Properties>
      </treenode>
      <treenode text="g_mmsIP" id="632767263316803270" vid="632642785229682334">
        <Properties type="String">g_mmsIP</Properties>
      </treenode>
      <treenode text="g_mmsPort" id="632767263316803272" vid="632642785229682336">
        <Properties type="UInt" defaultInitWith="0">g_mmsPort</Properties>
      </treenode>
      <treenode text="g_callWentActive" id="632767263316803274" vid="632642785229682338">
        <Properties type="Bool" defaultInitWith="false">g_callWentActive</Properties>
      </treenode>
      <treenode text="g_bargedCallId" id="632767263316803276" vid="632642785229682341">
        <Properties type="String">g_bargedCallId</Properties>
      </treenode>
      <treenode text="g_dbRecord" id="632767263316803278" vid="632644651432640490">
        <Properties type="Bool" defaultInitWith="false">g_dbRecord</Properties>
      </treenode>
      <treenode text="g_mediaBucketIP" id="632767263316803280" vid="632644651432642159">
        <Properties type="String" initWith="MediaBucketIP">g_mediaBucketIP</Properties>
      </treenode>
      <treenode text="g_mediaBucketPort" id="632767263316803282" vid="632644651432642161">
        <Properties type="UInt" defaultInitWith="0" initWith="MediaBucketPort">g_mediaBucketPort</Properties>
      </treenode>
      <treenode text="g_isBarged" id="632767263316803284" vid="632645798363016376">
        <Properties type="Bool" defaultInitWith="false">g_isBarged</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnRecWithBargeCall" startnode="632641167141715443" treenode="632641167141715444" appnode="632641167141715441" handlerfor="632641167141715440">
    <node type="Start" id="632641167141715443" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="282">
      <linkto id="632641251906402675" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632641251906402675" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="143" y="282">
      <linkto id="632641251906402690" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnRecWithBargeCall: LineId is: " + lineId</log>
public static string Execute(string callId, string lineId, string callType, LogWriter log)
{
	bool success = true;

	if (callId	== null || callId == string.Empty)
	{
		log.Write(TraceLevel.Warning, "OnRecWithBargeCall: Received CallId is invalid.");
		success = false;
	}

	if (lineId == null || lineId == string.Empty)
	{
		log.Write(TraceLevel.Warning, "OnRecWithBargeCall: Received LineId is invalid.");
		success = false;
	}

	if (callType == null || callType == string.Empty)
	{
		log.Write(TraceLevel.Warning, "OnRecWithBargeCall: Received CallType is invalid.");
		success = false;
	}

	return success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
}
</Properties>
    </node>
    <node type="Action" id="632641251906402690" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="273" y="282">
      <linkto id="632641251906402691" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">callId</ap>
        <ap name="Value2" type="variable">lineId</ap>
        <ap name="Value3" type="variable">to</ap>
        <ap name="Value4" type="variable">originalTo</ap>
        <rd field="ResultData">g_callId</rd>
        <rd field="ResultData2">g_lineId</rd>
        <rd field="ResultData3">g_to</rd>
        <rd field="ResultData4">g_originalTo</rd>
        <log condition="exit" on="true" level="Verbose" type="csharp">"OnRecWithBargeCall: g_lineId is: " + g_lineId</log>
      </Properties>
    </node>
    <node type="Action" id="632641251906402691" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="383" y="282">
      <linkto id="632642785229682327" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">from</ap>
        <ap name="Value2" type="variable">callType</ap>
        <ap name="Value3" type="variable">routingGuid</ap>
        <rd field="ResultData">g_from</rd>
        <rd field="ResultData2">g_callType</rd>
        <rd field="ResultData3">g_routingGuid</rd>
      </Properties>
    </node>
    <node type="Action" id="632641251906402692" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="911" y="282">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632642785229682327" name="CreateConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="520" y="282">
      <linkto id="632644651432640489" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="SoundToneOnJoin" type="csharp">false</ap>
        <ap name="MediaTxIP" type="variable">g_mediaBucketIP</ap>
        <ap name="MediaTxPort" type="variable">g_mediaBucketPort</ap>
        <ap name="ConnectionId" type="literal">0</ap>
        <rd field="ConnectionId">g_connectionId</rd>
        <rd field="ConferenceId">g_conferenceId</rd>
        <rd field="MmsId">g_mmsId</rd>
        <rd field="MediaRxIP">g_mmsIP</rd>
        <rd field="MediaRxPort">g_mmsPort</rd>
      </Properties>
    </node>
    <node type="Action" id="632644651432640489" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="658" y="283">
      <linkto id="632644651432640492" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632644651432640493" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">"INSERT INTO rec_with_barge (line_id, conference_id, mms_id) VALUES (" + g_lineId + ", " + g_conferenceId + ", " + g_mmsId + ")"</ap>
        <ap name="Name" type="literal">RecordingWithBarge</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432640492" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="778" y="283">
      <linkto id="632641251906402692" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_dbRecord</rd>
      </Properties>
    </node>
    <node type="Action" id="632644651432640493" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="658" y="396">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632641167141715445" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632641167141715446" name="lineId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="LineId" refType="reference">lineId</Properties>
    </node>
    <node type="Variable" id="632641167141715447" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632641248863962061" name="originalTo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="OriginalTo" refType="reference">originalTo</Properties>
    </node>
    <node type="Variable" id="632641248863962062" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference">from</Properties>
    </node>
    <node type="Variable" id="632641248863962063" name="callType" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallType" refType="reference">callType</Properties>
    </node>
    <node type="Variable" id="632641251906402674" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallActive" startnode="632641251906402696" treenode="632641251906402697" appnode="632641251906402694" handlerfor="632641251906402693">
    <node type="Start" id="632641251906402696" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="263">
      <linkto id="632642785229682340" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632642785229682326" name="Barge" class="MaxActionNode" group="" path="Metreos.CallControl" x="251" y="264">
      <linkto id="632645798363016378" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632642785229682351" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="From" type="variable">g_lineId</ap>
        <ap name="MediaRxIP" type="variable">g_mmsIP</ap>
        <ap name="MediaRxPort" type="variable">g_mmsPort</ap>
        <rd field="CallId">g_bargedCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632642785229682340" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="144" y="263">
      <linkto id="632642785229682326" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_callWentActive</rd>
      </Properties>
    </node>
    <node type="Action" id="632642785229682351" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="349" y="361">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632645798363016378" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="349" y="264">
      <linkto id="632642785229682351" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <rd field="ResultData">g_isBarged</rd>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallInactive" activetab="true" startnode="632641251906402701" treenode="632641251906402702" appnode="632641251906402699" handlerfor="632641251906402698">
    <node type="Start" id="632641251906402701" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="88" y="259">
      <linkto id="632645798363016380" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632642785229682352" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="181" y="404">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632645798363016380" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="180" y="259">
      <linkto id="632645798363016381" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632642785229682352" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">isInUse &amp;&amp; g_isBarged</ap>
      </Properties>
    </node>
    <node type="Action" id="632645798363016381" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="306" y="258">
      <linkto id="632645798363016382" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_bargedCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632645798363016382" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="306.048828" y="404">
      <linkto id="632642785229682352" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">false</ap>
        <rd field="ResultData">g_isBarged</rd>
      </Properties>
    </node>
    <node type="Variable" id="632645798363016379" name="isInUse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" initWith="InUse" defaultInitWith="true" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInactive">isInUse</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiHangup" startnode="632642785229682346" treenode="632642785229682347" appnode="632642785229682344" handlerfor="632642785229682343">
    <node type="Start" id="632642785229682346" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="45" y="265">
      <linkto id="632643055809088443" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632643055809088405" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="411" y="266">
      <linkto id="632645798363016384" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_bargedCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632643055809088443" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="153" y="266">
      <linkto id="632643055809088444" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632643055809088444" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="256" y="265">
      <linkto id="632643055809088405" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632644651432640494" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_callWentActive</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432640494" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="414" y="491">
      <linkto id="632644651432640920" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632644651432640921" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_dbRecord</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432640920" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="555" y="492">
      <linkto id="632644651432640921" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">"DELETE FROM rec_with_barge WHERE line_id='" + g_lineId + "'"</ap>
        <ap name="Name" type="literal">RecordingWithBarge</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432640921" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="555" y="639">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632645798363016384" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="411.048828" y="363">
      <linkto id="632644651432640494" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">false</ap>
        <rd field="ResultData">g_isBarged</rd>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632643055809088409" treenode="632643055809088410" appnode="632643055809088407" handlerfor="632643055809088406">
    <node type="Start" id="632643055809088409" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="288">
      <linkto id="632643055809088441" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632643055809088441" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="109" y="289">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632643055809088414" treenode="632643055809088415" appnode="632643055809088412" handlerfor="632643055809088411">
    <node type="Start" id="632643055809088414" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632643055809088436" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632643055809088436" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="91" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632643055809088419" treenode="632643055809088420" appnode="632643055809088417" handlerfor="632643055809088416">
    <node type="Start" id="632643055809088419" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632643055809088437" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632643055809088437" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="107" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632643055809088424" treenode="632643055809088425" appnode="632643055809088422" handlerfor="632643055809088421">
    <node type="Start" id="632643055809088424" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632643055809088438" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632643055809088438" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="112" y="33">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotDigits" startnode="632643055809088429" treenode="632643055809088430" appnode="632643055809088427" handlerfor="632643055809088426">
    <node type="Start" id="632643055809088429" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632643055809088439" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632643055809088439" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="111" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnCallChanged" startnode="632643055809088434" treenode="632643055809088435" appnode="632643055809088432" handlerfor="632643055809088431">
    <node type="Start" id="632643055809088434" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632643055809088440" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632643055809088440" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="124" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>