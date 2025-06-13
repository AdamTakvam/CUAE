<Application name="OnCreateConference" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="OnCreateConference">
    <outline>
      <treenode type="evh" id="632544570032115902" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632544570032115899" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632544570032115898" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/conference/create</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632544570032115979" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632544570032115976" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632544570032115975" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632550567076605508" actid="632544570032115990" />
          <ref id="632550567076605585" actid="632545089775390479" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632544570032115984" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632544570032115981" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632544570032115980" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632550567076605509" actid="632544570032115990" />
          <ref id="632550567076605586" actid="632545089775390479" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632544570032115989" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632544570032115986" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632544570032115985" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632550567076605510" actid="632544570032115990" />
          <ref id="632550567076605587" actid="632545089775390479" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632544570032116015" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632544570032116012" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632544570032116011" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632550567076605527" actid="632544570032116021" />
          <ref id="632550567076605530" actid="632544570032116024" />
          <ref id="632550567076605609" actid="632545089775390510" />
          <ref id="632550567076605643" actid="632545089775390568" />
          <ref id="632550567076605646" actid="632545089775390571" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632544570032116020" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632544570032116017" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632544570032116016" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632550567076605528" actid="632544570032116021" />
          <ref id="632550567076605531" actid="632544570032116024" />
          <ref id="632550567076605610" actid="632545089775390510" />
          <ref id="632550567076605644" actid="632545089775390568" />
          <ref id="632550567076605647" actid="632545089775390571" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632545089775390476" level="2" text="Metreos.Providers.Http.GotRequest: OnJoinConference">
        <node type="function" name="OnJoinConference" id="632545089775390473" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632545089775390472" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/conference/join</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632545089775390500" level="2" text="Metreos.Providers.Http.GotRequest: OnKickParticipant">
        <node type="function" name="OnKickParticipant" id="632545089775390497" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632545089775390496" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/conference/kick</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632545089775390530" level="2" text="Metreos.Providers.Http.GotRequest: OnMuteParticipant">
        <node type="function" name="OnMuteParticipant" id="632545089775390527" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632545089775390526" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/conference/mute</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632545089775390606" level="2" text="Metreos.Providers.Http.GotRequest: OnTerminate">
        <node type="function" name="OnTerminate" id="632545089775390603" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632545089775390602" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/conference/terminate</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_dbConferenceId" id="632550567076605493" vid="632544570032115966">
        <Properties type="Int">g_dbConferenceId</Properties>
      </treenode>
      <treenode text="g_mmsConferenceId" id="632550567076605495" vid="632544570032115968">
        <Properties type="String">g_mmsConferenceId</Properties>
      </treenode>
      <treenode text="g_hostParticipantId" id="632550567076605497" vid="632544570032116007">
        <Properties type="Int">g_hostParticipantId</Properties>
      </treenode>
      <treenode text="callIdParticipantsTable" id="632550567076605499" vid="632545089775390460">
        <Properties type="Hashtable">callIdParticipantsTable</Properties>
      </treenode>
      <treenode text="callIdConnectionTable" id="632550567076605501" vid="632545089775390517">
        <Properties type="Hashtable">callIdConnectionTable</Properties>
      </treenode>
      <treenode text="g_outstandingKickPlays" id="632550567076605717" vid="632550567076605716">
        <Properties type="Int">g_outstandingKickPlays</Properties>
      </treenode>
      <treenode text="g_outstandingMakeCalls" id="632550567076605730" vid="632550567076605729">
        <Properties type="Int">g_outstandingMakeCalls</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632544570032115901" treenode="632544570032115902" appnode="632544570032115899" handlerfor="632544570032115898">
    <node type="Start" id="632544570032115901" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="304">
      <linkto id="632544570032115964" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632544570032115964" name="CreateConference" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="168" y="304">
      <linkto id="632544570032115970" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632544570032115995" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="SessionId" type="variable">routingGuid</ap>
        <rd field="ConferenceId">g_dbConferenceId</rd>
        <log condition="entry" on="true" level="Info" type="literal">New conference initiation</log>
        <log condition="default" on="true" level="Info" type="literal">Unable to create conference record</log>
      </Properties>
    </node>
    <node type="Action" id="632544570032115970" name="CreateWebResponse" class="MaxActionNode" group="" path="Metreos.Native.Mec" x="168" y="432">
      <linkto id="632544570032115972" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RequestType" type="literal">create</ap>
        <ap name="Result" type="literal">failure</ap>
        <rd field="ResultData">webResponse</rd>
      </Properties>
    </node>
    <node type="Action" id="632544570032115972" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="168" y="544">
      <linkto id="632549681394323597" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="metreosSessionId" type="variable">routingGuid</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">webResponse.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632544570032115973" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="175" y="768">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632544570032115990" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="379" y="286" mx="445" my="302">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632544570032115979" />
        <item text="OnMakeCall_Failed" treenode="632544570032115984" />
        <item text="OnRemoteHangup" treenode="632544570032115989" />
      </items>
      <linkto id="632549736716574968" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632544570032115970" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="csharp">createCommand.GetContactAddress(0)</ap>
        <ap name="UserData" type="variable">g_hostParticipantId</ap>
        <rd field="CallId">callId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"Adding host participant " + g_hostParticipantId + " to conference"</log>
      </Properties>
    </node>
    <node type="Action" id="632544570032115995" name="AddParticipant" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="304" y="302">
      <linkto id="632544570032115990" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">g_dbConferenceId</ap>
        <ap name="MmsConnectionId" type="csharp">0</ap>
        <ap name="CallId" type="variable">callId</ap>
        <ap name="PhoneNumber" type="csharp">createCommand.GetContactAddress(0)</ap>
        <ap name="Description" type="csharp">createCommand.GetDescription(0)</ap>
        <ap name="IsHost" type="csharp">true</ap>
        <rd field="ParticipantId">g_hostParticipantId</rd>
      </Properties>
    </node>
    <node type="Action" id="632544570032115997" name="CreateWebResponse" class="MaxActionNode" group="" path="Metreos.Native.Mec" x="789" y="302">
      <linkto id="632544570032115998" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RequestType" type="literal">create</ap>
        <ap name="Result" type="literal">success</ap>
        <ap name="ConferenceId" type="variable">g_dbConferenceId</ap>
        <ap name="ParticipantId" type="variable">g_hostParticipantId</ap>
        <ap name="PhoneNumber" type="csharp">createCommand.GetContactAddress(0)</ap>
        <rd field="ResultData">webResponse</rd>
      </Properties>
    </node>
    <node type="Action" id="632544570032115998" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="917" y="302">
      <linkto id="632544570032116002" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="metreosSessionId" type="variable">routingGuid</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">webResponse.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632544570032116002" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1037" y="302">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632545089775390462" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="673" y="302">
      <linkto id="632544570032115997" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Hashtable callIdParticipantsTable, string callId, int g_hostParticipantId, ref int g_outstandingMakeCalls)
{
	g_outstandingMakeCalls++;
	callIdParticipantsTable[callId] = g_hostParticipantId;
	return "";
}
</Properties>
    </node>
    <node type="Action" id="632549681394323597" name="EndConference" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="172" y="650">
      <linkto id="632544570032115973" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">g_dbConferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632549736716574968" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="557" y="302">
      <linkto id="632545089775390462" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">"update participants set callid = '" + callId + "' where id = " + g_hostParticipantId</ap>
        <ap name="Name" type="literal">mec</ap>
      </Properties>
    </node>
    <node type="Variable" id="632544570032115903" name="createCommand" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Mec.WebMessageRequest" initWith="body" refType="reference" name="Metreos.Providers.Http.GotRequest">createCommand</Properties>
    </node>
    <node type="Variable" id="632544570032115965" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632544570032115971" name="webResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Mec.WebMessageResponse" refType="reference">webResponse</Properties>
    </node>
    <node type="Variable" id="632544570032115974" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632544570032115994" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632544570032115978" treenode="632544570032115979" appnode="632544570032115976" handlerfor="632544570032115975">
    <node type="Start" id="632544570032115978" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="344">
      <linkto id="632545089775390519" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632544570032116005" name="SetParticipantStatus" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="299" y="345">
      <linkto id="632544570032116010" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ParticipantId" type="variable">participantId</ap>
        <ap name="Status" type="literal">Online</ap>
      </Properties>
    </node>
    <node type="Action" id="632544570032116010" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="456" y="344">
      <linkto id="632544570032116021" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632544570032116024" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_hostParticipantId</ap>
        <ap name="Value2" type="variable">participantId</ap>
      </Properties>
    </node>
    <node type="Action" id="632544570032116021" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="624" y="240" mx="677" my="256">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632544570032116015" />
        <item text="OnPlay_Failed" treenode="632544570032116020" />
      </items>
      <linkto id="632544570032116027" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">HostConnect.vox</ap>
        <ap name="State" type="literal">join</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="UserData" type="variable">participantId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Playing host connect media to participant " + participantId</log>
      </Properties>
    </node>
    <node type="Action" id="632544570032116024" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="624" y="440" mx="677" my="456">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632544570032116015" />
        <item text="OnPlay_Failed" treenode="632544570032116020" />
      </items>
      <linkto id="632544570032116028" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">NormalConnect.vox</ap>
        <ap name="State" type="literal">join</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="UserData" type="variable">participantId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Playing non-host connect media to participant " + participantId</log>
      </Properties>
    </node>
    <node type="Action" id="632544570032116027" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="800" y="256">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632544570032116028" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="800" y="456">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632545089775390519" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="144" y="344">
      <linkto id="632544570032116005" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="exit" on="true" level="Info" type="csharp">"Finish calling new participant " + participantId</log>
public static string Execute(Hashtable callIdConnectionTable, string callId, string connectionId, ref int g_outstandingMakeCalls)
{
	g_outstandingMakeCalls--;
	callIdConnectionTable[callId] = connectionId;
	return String.Empty;
}
</Properties>
    </node>
    <node type="Variable" id="632544570032116003" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">callId</Properties>
    </node>
    <node type="Variable" id="632544570032116004" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connectionId</Properties>
    </node>
    <node type="Variable" id="632544570032116006" name="participantId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="UserData" refType="reference">participantId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632544570032115983" treenode="632544570032115984" appnode="632544570032115981" handlerfor="632544570032115980">
    <node type="Start" id="632544570032115983" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="365">
      <linkto id="632550567076605686" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632544570032116029" name="SetParticipantStatus" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="286" y="364">
      <linkto id="632544570032116031" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ParticipantId" type="variable">participantId</ap>
        <ap name="Status" type="literal">Disconnected</ap>
      </Properties>
    </node>
    <node type="Action" id="632544570032116031" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="446" y="364">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632550567076605686" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="139" y="365">
      <linkto id="632544570032116029" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Hashtable callIdParticipantsTable, string callId)
{
	callIdParticipantsTable.Remove(callId);
	return "";
}
</Properties>
    </node>
    <node type="Variable" id="632544570032116030" name="participantId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="UserData" refType="reference">participantId</Properties>
    </node>
    <node type="Variable" id="632550567076605687" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.MakeCall_Failed">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632544570032115988" treenode="632544570032115989" appnode="632544570032115986" handlerfor="632544570032115985">
    <node type="Start" id="632544570032115988" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="384">
      <linkto id="632545089775390465" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632545089775390465" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="208" y="384">
      <linkto id="632545089775390467" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">(int) callIdParticipantsTable[callId] </ap>
        <rd field="ResultData">partipantId</rd>
      </Properties>
    </node>
    <node type="Action" id="632545089775390466" name="SetParticipantStatus" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="440" y="384">
      <linkto id="632545089775390468" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ParticipantId" type="variable">partipantId</ap>
        <ap name="Status" type="literal">Disconnected</ap>
      </Properties>
    </node>
    <node type="Action" id="632545089775390467" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="312" y="384">
      <linkto id="632545089775390466" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string callId, Hashtable callIdParticipantsTable)
{
	callIdParticipantsTable.Remove(callId);
	return "";
}
</Properties>
    </node>
    <node type="Action" id="632545089775390468" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="576" y="384">
      <linkto id="632545089775390469" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632545089775390470" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">callIdParticipantsTable.Count</ap>
        <ap name="Value2" type="csharp">0</ap>
      </Properties>
    </node>
    <node type="Action" id="632545089775390469" name="EndConference" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="715" y="383">
      <linkto id="632545089775390471" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">g_dbConferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632545089775390470" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="576" y="488">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632545089775390471" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="832" y="384">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632545089775390463" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">callId</Properties>
    </node>
    <node type="Variable" id="632545089775390464" name="partipantId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">partipantId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" activetab="true" startnode="632544570032116014" treenode="632544570032116015" appnode="632544570032116012" handlerfor="632544570032116011">
    <node type="Start" id="632544570032116014" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="360">
      <linkto id="632544570032116032" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632544570032116032" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="152" y="360">
      <linkto id="632544570032116039" type="Labeled" style="Bezier" ortho="true" label="kick" />
      <linkto id="632544570032116049" type="Labeled" style="Bezier" ortho="true" label="join" />
      <linkto id="632549513992628544" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">state</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Play complete for participant " + callIdParticipantsTable[userData] + ", state " + state</log>
      </Properties>
    </node>
    <node type="Action" id="632544570032116035" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="304" y="240">
      <linkto id="632544570032116038" type="Labeled" style="Bezier" ortho="true" label="userstop" />
      <linkto id="632544570032116045" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632544570032116036" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="736" y="240">
      <linkto id="632544570032116042" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632544570032116043" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_hostParticipantId.ToString()</ap>
        <ap name="Value2" type="csharp">userData.ToString()</ap>
      </Properties>
    </node>
    <node type="Action" id="632544570032116038" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="304" y="88">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632544570032116039" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="154" y="485">
      <linkto id="632550567076605718" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632544570032116042" name="SetParticipantStatus" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="840" y="160">
      <linkto id="632544570032116044" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ParticipantId" type="variable">userData</ap>
        <ap name="Status" type="literal">Conferenced</ap>
      </Properties>
    </node>
    <node type="Action" id="632544570032116043" name="SetParticipantStatus" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="848" y="336">
      <linkto id="632544570032116057" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ParticipantId" type="variable">userData</ap>
        <ap name="Status" type="literal">Conferenced</ap>
      </Properties>
    </node>
    <node type="Action" id="632544570032116044" name="StartConference" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="992" y="160">
      <linkto id="632544570032116056" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">g_dbConferenceId</ap>
        <ap name="MmsConferenceId" type="variable">g_mmsConferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632544570032116045" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="440" y="240">
      <linkto id="632544570032116046" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632544570032116048" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">String.Empty</ap>
        <ap name="Value2" type="variable">g_mmsConferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632544570032116046" name="CreateConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="568" y="112">
      <linkto id="632544570032116036" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632544570032116052" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <rd field="ConferenceId">g_mmsConferenceId</rd>
      </Properties>
    </node>
    <node type="Action" id="632544570032116048" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="576" y="376">
      <linkto id="632544570032116036" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632544570032116053" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="ConferenceId" type="variable">g_mmsConferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632544570032116049" name="GetParticipant" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="152" y="240">
      <linkto id="632544570032116035" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ParticipantId" type="variable">userData</ap>
        <rd field="CallId">callIdWhenJoin</rd>
      </Properties>
    </node>
    <node type="Action" id="632544570032116051" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="592" y="592">
      <linkto id="632549513992628543" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callIdWhenJoin</ap>
      </Properties>
    </node>
    <node type="Label" id="632544570032116052" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="568" y="32" />
    <node type="Label" id="632544570032116053" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="576" y="464" />
    <node type="Label" id="632544570032116054" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="379" y="594">
      <linkto id="632544570032116051" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632544570032116056" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1136" y="160">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632544570032116057" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="992" y="336">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632549513992628542" name="SetParticipantStatus" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="155" y="651">
      <linkto id="632550567076605723" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ParticipantId" type="csharp">(int) callIdParticipantsTable[userData]</ap>
        <ap name="Status" type="literal">Disconnected</ap>
      </Properties>
    </node>
    <node type="Action" id="632549513992628543" name="SetParticipantStatus" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="688" y="592">
      <linkto id="632550567076605699" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ParticipantId" type="csharp">(int) callIdParticipantsTable[callIdWhenJoin]</ap>
        <ap name="Status" type="literal">Disconnected</ap>
      </Properties>
    </node>
    <node type="Action" id="632549513992628544" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="416" y="355">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632550567076605698" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="877.999756" y="640">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632550567076605699" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="803.9999" y="595">
      <linkto id="632550567076605698" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632550567076605700" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">0</ap>
        <ap name="Value2" type="csharp">callIdConnectionTable.Count</ap>
      </Properties>
    </node>
    <node type="Action" id="632550567076605700" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="873" y="535">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632550567076605718" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="155" y="570">
      <linkto id="632549513992628542" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref int g_outstandingKickPlays)
{
	g_outstandingKickPlays--;
	return "";
}
</Properties>
    </node>
    <node type="Action" id="632550567076605721" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="103.4707" y="813">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632550567076605722" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="210.4707" y="816">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632550567076605723" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="154.5293" y="719">
      <linkto id="632550567076605722" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632550567076605721" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_outstandingKickPlays == 0 &amp;&amp; callIdConnectionTable.Count == 0 &amp;&amp; g_outstandingMakeCalls == 0</ap>
      </Properties>
    </node>
    <node type="Variable" id="632544570032116033" name="state" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="state" refType="reference">state</Properties>
    </node>
    <node type="Variable" id="632544570032116034" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Play_Complete">termCond</Properties>
    </node>
    <node type="Variable" id="632544570032116040" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632544570032116047" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.MediaControl.Play_Complete">connectionId</Properties>
    </node>
    <node type="Variable" id="632544570032116050" name="callIdWhenJoin" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">callIdWhenJoin</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632544570032116019" treenode="632544570032116020" appnode="632544570032116017" handlerfor="632544570032116016">
    <node type="Start" id="632544570032116019" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632544570032116058" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632544570032116058" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="633" y="300">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJoinConference" startnode="632545089775390475" treenode="632545089775390476" appnode="632545089775390473" handlerfor="632545089775390472">
    <node type="Start" id="632545089775390475" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="336">
      <linkto id="632545089775390485" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632545089775390479" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="248" y="322" mx="314" my="338">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632544570032115979" />
        <item text="OnMakeCall_Failed" treenode="632544570032115984" />
        <item text="OnRemoteHangup" treenode="632544570032115989" />
      </items>
      <linkto id="632545089775390489" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632549736716574973" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="csharp">joinCommand.GetContactAddress(0)</ap>
        <ap name="UserData" type="variable">participantId</ap>
        <rd field="CallId">callId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"Adding a new participant " + participantId + " to the conference"</log>
      </Properties>
    </node>
    <node type="Action" id="632545089775390484" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="544" y="336">
      <linkto id="632545089775390487" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Hashtable callIdParticipantsTable, string callId, int participantId)
{
	callIdParticipantsTable[callId] = participantId;
	return "";
}
</Properties>
    </node>
    <node type="Action" id="632545089775390485" name="AddParticipant" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="185" y="341">
      <linkto id="632545089775390479" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">g_dbConferenceId</ap>
        <ap name="MmsConnectionId" type="csharp">String.Empty</ap>
        <ap name="CallId" type="variable">callId</ap>
        <ap name="PhoneNumber" type="csharp">joinCommand.GetContactAddress(0)</ap>
        <ap name="Description" type="csharp">joinCommand.GetDescription(0)</ap>
        <ap name="IsHost" type="literal">false</ap>
        <rd field="ParticipantId">participantId</rd>
      </Properties>
    </node>
    <node type="Action" id="632545089775390487" name="CreateWebResponse" class="MaxActionNode" group="" path="Metreos.Native.Mec" x="720" y="336">
      <linkto id="632545089775390492" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RequestType" type="literal">join</ap>
        <ap name="Result" type="literal">success</ap>
        <ap name="ConferenceId" type="variable">g_dbConferenceId</ap>
        <ap name="ParticipantId" type="variable">participantId</ap>
        <ap name="PhoneNumber" type="csharp">joinCommand.GetContactAddress(0)</ap>
        <rd field="ResultData">response</rd>
      </Properties>
    </node>
    <node type="Action" id="632545089775390489" name="CreateWebResponse" class="MaxActionNode" group="" path="Metreos.Native.Mec" x="313" y="488">
      <linkto id="632545089775390491" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RequestType" type="literal">join</ap>
        <ap name="Result" type="literal">failure</ap>
        <rd field="ResultData">response</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"MakeCall failed for participant " + participantId</log>
      </Properties>
    </node>
    <node type="Action" id="632545089775390491" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="313" y="592">
      <linkto id="632545089775390494" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="metreosSessionId" type="variable">routingGuid</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">response.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632545089775390492" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="864" y="336">
      <linkto id="632545089775390495" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="metreosSessionId" type="variable">routingGuid</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">response.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632545089775390494" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="313" y="680">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632545089775390495" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1000" y="336">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632549736716574973" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="447.840454" y="338">
      <linkto id="632545089775390484" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">"update participants set callid = '" + callId + "' where id = " + participantId</ap>
        <ap name="Name" type="literal">mec</ap>
      </Properties>
    </node>
    <node type="Variable" id="632545089775390477" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632545089775390478" name="joinCommand" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Mec.WebMessageRequest" initWith="body" refType="reference" name="Metreos.Providers.Http.GotRequest">joinCommand</Properties>
    </node>
    <node type="Variable" id="632545089775390483" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632545089775390486" name="participantId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">participantId</Properties>
    </node>
    <node type="Variable" id="632545089775390488" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Mec.WebMessageResponse" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632549532198292665" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnKickParticipant" startnode="632545089775390499" treenode="632545089775390500" appnode="632545089775390497" handlerfor="632545089775390496">
    <node type="Start" id="632545089775390499" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="336">
      <linkto id="632545089775390504" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632545089775390504" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="160" y="336">
      <linkto id="632545089775390513" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">int.Parse(kickCommand.GetLocationId(0))</ap>
        <rd field="ResultData">participantId</rd>
        <log condition="exit" on="true" level="Info" type="csharp">"Kick ID: " + participantId</log>
      </Properties>
    </node>
    <node type="Action" id="632545089775390506" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="877" y="332">
      <linkto id="632550567076605719" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="metreosSessionId" type="variable">routingGuid</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">response.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Playing kick media to " + participantId</log>
      </Properties>
    </node>
    <node type="Action" id="632545089775390507" name="CreateWebResponse" class="MaxActionNode" group="" path="Metreos.Native.Mec" x="539" y="334">
      <linkto id="632550567076605705" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RequestType" type="literal">kick</ap>
        <ap name="Result" type="literal">success</ap>
        <rd field="ResultData">response</rd>
      </Properties>
    </node>
    <node type="Action" id="632545089775390508" name="CreateWebResponse" class="MaxActionNode" group="" path="Metreos.Native.Mec" x="941" y="468">
      <linkto id="632545089775390522" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RequestType" type="literal">kick</ap>
        <ap name="Result" type="literal">failure</ap>
        <rd field="ResultData">response</rd>
      </Properties>
    </node>
    <node type="Action" id="632545089775390510" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="717" y="316" mx="770" my="332">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632544570032116015" />
        <item text="OnPlay_Failed" treenode="632544570032116020" />
      </items>
      <linkto id="632545089775390521" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632545089775390506" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">Kick.vox</ap>
        <ap name="State" type="literal">kick</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="UserData" type="variable">callId</ap>
        <rd field="ResultCode">resultCode</rd>
      </Properties>
    </node>
    <node type="Action" id="632545089775390513" name="GetParticipant" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="256" y="336">
      <linkto id="632545089775390536" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ParticipantId" type="variable">participantId</ap>
        <rd field="CallId">callId</rd>
        <log condition="exit" on="true" level="Info" type="csharp">"Kick CallId: " + callId</log>
      </Properties>
    </node>
    <node type="Action" id="632545089775390515" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="773" y="588">
      <linkto id="632545089775390510" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632545089775390508" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Block" type="csharp">true</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Stopping media for participant " + participantId</log>
      </Properties>
    </node>
    <node type="Action" id="632545089775390521" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="813" y="468">
      <linkto id="632545089775390515" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632545089775390508" type="Labeled" style="Bezier" ortho="true" label="WHATSHOLUDTHISBE" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">resultCode</ap>
      </Properties>
    </node>
    <node type="Action" id="632545089775390522" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1085" y="468">
      <linkto id="632545089775390524" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="metreosSessionId" type="variable">routingGuid</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">response.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632545089775390524" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1213" y="468">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632545089775390525" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1082" y="329">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632545089775390536" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="352" y="336">
      <linkto id="632550567076605694" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">callIdConnectionTable[callId] as string</ap>
        <rd field="ResultData">connectionId</rd>
        <log condition="exit" on="true" level="Info" type="csharp">"Kick connectionId: " + connectionId</log>
      </Properties>
    </node>
    <node type="Action" id="632550567076605694" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="436.90625" y="336">
      <linkto id="632545089775390507" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Hashtable callIdConnectionTable, string callId)
{
	callIdConnectionTable.Remove(callId);
	return "";
}
</Properties>
    </node>
    <node type="Action" id="632550567076605705" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="647.4707" y="336">
      <linkto id="632545089775390510" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632550567076605727" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">connectionId == null || connectionId == String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632550567076605706" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="645.9414" y="550">
      <linkto id="632550567076605708" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Kicking participant " + participantId + " who is being called presently"</log>
      </Properties>
    </node>
    <node type="Action" id="632550567076605707" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="598.9414" y="889">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632550567076605708" name="SetParticipantStatus" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="646.9414" y="668">
      <linkto id="632550567076605720" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ParticipantId" type="variable">participantId</ap>
        <ap name="Status" type="literal">Disconnected</ap>
      </Properties>
    </node>
    <node type="Action" id="632550567076605710" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="705.9414" y="892">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632550567076605719" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="976" y="331">
      <linkto id="632545089775390525" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref int g_outstandingKickPlays)
{
	g_outstandingKickPlays++;
	return "";
}
</Properties>
    </node>
    <node type="Action" id="632550567076605720" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="650" y="795">
      <linkto id="632550567076605710" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632550567076605707" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_outstandingKickPlays == 0 &amp;&amp; callIdConnectionTable.Count == 0 &amp;&amp; g_outstandingMakeCalls == 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632550567076605727" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="645" y="437">
      <linkto id="632550567076605706" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="metreosSessionId" type="variable">routingGuid</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">response.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Playing kick media to " + participantId</log>
      </Properties>
    </node>
    <node type="Variable" id="632545089775390501" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632545089775390502" name="kickCommand" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Mec.WebMessageRequest" initWith="body" refType="reference" name="Metreos.Providers.Http.GotRequest">kickCommand</Properties>
    </node>
    <node type="Variable" id="632545089775390503" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Mec.WebMessageResponse" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632545089775390505" name="participantId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">participantId</Properties>
    </node>
    <node type="Variable" id="632545089775390514" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632545089775390516" name="resultCode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">resultCode</Properties>
    </node>
    <node type="Variable" id="632545089775390520" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
    <node type="Variable" id="632549532198292666" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMuteParticipant" startnode="632545089775390529" treenode="632545089775390530" appnode="632545089775390527" handlerfor="632545089775390526">
    <node type="Start" id="632545089775390529" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="448">
      <linkto id="632545089775390537" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632545089775390537" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="152" y="448">
      <linkto id="632545089775390538" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">int.Parse(muteCommand.GetLocationId(0))</ap>
        <rd field="ResultData">participantId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"Muting " + int.Parse(muteCommand.GetLocationId(0))</log>
      </Properties>
    </node>
    <node type="Action" id="632545089775390538" name="GetParticipant" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="256" y="448">
      <linkto id="632545089775390539" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ParticipantId" type="variable">participantId</ap>
        <rd field="CallId">callId</rd>
        <rd field="Status">status</rd>
      </Properties>
    </node>
    <node type="Action" id="632545089775390539" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="352" y="448">
      <linkto id="632545089775390547" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">callIdConnectionTable[callId] as string</ap>
        <ap name="Value2" type="csharp">status == 4</ap>
        <rd field="ResultData">connectionId</rd>
        <rd field="ResultData2">isMuted</rd>
      </Properties>
    </node>
    <node type="Action" id="632545089775390547" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="480" y="448">
      <linkto id="632545089775390557" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632545089775390556" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">isMuted</ap>
      </Properties>
    </node>
    <node type="Action" id="632545089775390548" name="Mute" class="MaxActionNode" group="" path="Metreos.MediaControl" x="936" y="232">
      <linkto id="632545089775390567" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632545089775390582" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="ConferenceId" type="variable">g_mmsConferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632545089775390549" name="CreateWebResponse" class="MaxActionNode" group="" path="Metreos.Native.Mec" x="848" y="592">
      <linkto id="632545089775390550" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RequestType" type="literal">mute</ap>
        <ap name="Result" type="literal">success</ap>
        <ap name="ParticipantId" type="variable">participantId</ap>
        <rd field="ResultData">response</rd>
      </Properties>
    </node>
    <node type="Action" id="632545089775390550" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="848" y="704">
      <linkto id="632545089775390551" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="metreosSessionId" type="variable">routingGuid</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">response.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632545089775390551" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="848" y="808">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632545089775390552" text="U" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="936" y="136">
      <linkto id="632545089775390548" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632545089775390553" name="Unmute" class="MaxActionNode" group="" path="Metreos.MediaControl" x="744" y="232">
      <linkto id="632545089775390566" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632545089775390583" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="ConferenceId" type="variable">g_mmsConferenceId</ap>
      </Properties>
    </node>
    <node type="Label" id="632545089775390554" text="M" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="744" y="128">
      <linkto id="632545089775390553" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632545089775390556" text="M" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="592" y="544" />
    <node type="Label" id="632545089775390557" text="U" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="600" y="352" />
    <node type="Action" id="632545089775390566" name="SetParticipantStatus" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="744" y="328">
      <linkto id="632545089775390568" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ParticipantId" type="variable">participantId</ap>
        <ap name="Status" type="literal">Conferenced</ap>
      </Properties>
    </node>
    <node type="Action" id="632545089775390567" name="SetParticipantStatus" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="936" y="328">
      <linkto id="632545089775390571" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ParticipantId" type="variable">participantId</ap>
        <ap name="Status" type="literal">Mute</ap>
      </Properties>
    </node>
    <node type="Action" id="632545089775390568" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="696" y="424" mx="749" my="440">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632544570032116015" />
        <item text="OnPlay_Failed" treenode="632544570032116020" />
      </items>
      <linkto id="632545089775390549" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632545089775390584" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">Mute.vox</ap>
        <ap name="State" type="literal">mute</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="UserData" type="variable">callId</ap>
        <rd field="ResultCode">resultCode</rd>
      </Properties>
    </node>
    <node type="Action" id="632545089775390571" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="888" y="424" mx="941" my="440">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632544570032116015" />
        <item text="OnPlay_Failed" treenode="632544570032116020" />
      </items>
      <linkto id="632545089775390549" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632545089775390585" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">UnMute.vox</ap>
        <ap name="State" type="literal">mute</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="UserData" type="variable">callId</ap>
        <rd field="ResultCode">resultCode</rd>
      </Properties>
    </node>
    <node type="Action" id="632545089775390575" name="CreateWebResponse" class="MaxActionNode" group="" path="Metreos.Native.Mec" x="1136" y="240">
      <linkto id="632545089775390576" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RequestType" type="literal">mute</ap>
        <ap name="Result" type="literal">failure</ap>
        <ap name="ParticipantId" type="variable">participantId</ap>
        <rd field="ResultData">response</rd>
      </Properties>
    </node>
    <node type="Action" id="632545089775390576" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1136" y="336">
      <linkto id="632545089775390577" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="metreosSessionId" type="variable">routingGuid</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">response.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632545089775390577" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1136" y="440">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632545089775390581" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1132.47388" y="136">
      <linkto id="632545089775390575" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632545089775390582" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1032" y="232" />
    <node type="Label" id="632545089775390583" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="832" y="232" />
    <node type="Label" id="632545089775390584" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="824" y="440" />
    <node type="Label" id="632545089775390585" text="T" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1032" y="440" />
    <node type="Action" id="632545089775390586" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="1032" y="728">
      <linkto id="632545089775390589" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Block" type="csharp">true</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
      </Properties>
    </node>
    <node type="Label" id="632545089775390587" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1032" y="511.999969">
      <linkto id="632545089775390588" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632545089775390588" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1032" y="616">
      <linkto id="632545089775390586" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632545089775390549" type="Labeled" style="Bezier" ortho="true" label="WHAT SHOULD THIS BE" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">resultCode</ap>
      </Properties>
    </node>
    <node type="Label" id="632545089775390589" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1032" y="856" />
    <node type="Label" id="632545089775390590" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="688" y="440">
      <linkto id="632545089775390568" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632545089775390593" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="1128" y="768">
      <linkto id="632545089775390596" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Block" type="csharp">true</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
      </Properties>
    </node>
    <node type="Label" id="632545089775390594" text="T" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1128" y="511.999969">
      <linkto id="632545089775390595" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632545089775390595" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1128" y="656">
      <linkto id="632545089775390593" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632545089775390549" type="Labeled" style="Bezier" ortho="true" label="WHAT SHOLUD THIS BE" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">resultCode</ap>
      </Properties>
    </node>
    <node type="Label" id="632545089775390596" text="Q" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1128" y="896" />
    <node type="Label" id="632545089775390601" text="Q" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="878.4739" y="438.5">
      <linkto id="632545089775390571" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Variable" id="632545089775390531" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632545089775390532" name="muteCommand" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Mec.WebMessageRequest" initWith="body" refType="reference" name="Metreos.Providers.Http.GotRequest">muteCommand</Properties>
    </node>
    <node type="Variable" id="632545089775390533" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Mec.WebMessageResponse" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632545089775390535" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632545089775390543" name="participantId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">participantId</Properties>
    </node>
    <node type="Variable" id="632545089775390544" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
    <node type="Variable" id="632545089775390545" name="isMuted" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">isMuted</Properties>
    </node>
    <node type="Variable" id="632545089775390546" name="status" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">status</Properties>
    </node>
    <node type="Variable" id="632545089775390574" name="resultCode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">resultCode</Properties>
    </node>
    <node type="Variable" id="632549532198292667" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnTerminate" startnode="632545089775390605" treenode="632545089775390606" appnode="632545089775390603" handlerfor="632545089775390602">
    <node type="Loop" id="632545089775390607" name="Loop" text="loop (expr)" cx="387" cy="314" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="336" y="248" mx="530" my="405">
      <linkto id="632549513992628538" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632549513992628535" fromport="3" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties iteratorType="dictEnum" type="csharp">callIdConnectionTable</Properties>
    </node>
    <node type="Start" id="632545089775390605" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="408">
      <linkto id="632550567076605693" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632549513992628535" name="CreateWebResponse" class="MaxActionNode" group="" path="Metreos.Native.Mec" x="840" y="408">
      <linkto id="632549513992628540" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RequestType" type="literal">terminate</ap>
        <ap name="Result" type="literal">success</ap>
        <ap name="ConferenceId" type="variable">g_dbConferenceId</ap>
        <rd field="ResultData">response</rd>
      </Properties>
    </node>
    <node type="Action" id="632549513992628538" name="Hangup" container="632545089775390607" class="MaxActionNode" group="" path="Metreos.CallControl" x="464" y="400">
      <linkto id="632549513992628541" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="csharp">loopDictEnum.Key as string</ap>
      </Properties>
    </node>
    <node type="Action" id="632549513992628539" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1226" y="407">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632549513992628540" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="984" y="408">
      <linkto id="632549681394323598" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="metreosSessionId" type="variable">routingGuid</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">response.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632549513992628541" name="SetParticipantStatus" container="632545089775390607" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="576" y="400">
      <linkto id="632545089775390607" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ParticipantId" type="csharp">(int) callIdParticipantsTable[loopDictEnum.Key]</ap>
        <ap name="Status" type="literal">Disconnected</ap>
      </Properties>
    </node>
    <node type="Action" id="632549681394323598" name="EndConference" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1100" y="409">
      <linkto id="632549513992628539" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">g_dbConferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632550567076605693" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="182" y="401">
      <linkto id="632545089775390607" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"Terminating entire conference"</ap>
        <ap name="LogLevel" type="literal">Info</ap>
      </Properties>
    </node>
    <node type="Variable" id="632545089775390608" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632549513992628534" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Mec.WebMessageResponse" initWith="RoutingGuid" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632549532198292668" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>