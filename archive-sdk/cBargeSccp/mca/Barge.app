<Application name="Barge" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Barge">
    <outline>
      <treenode type="evh" id="632452368884448232" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632452368884448229" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632452368884448228" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632452462487469981" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632452462487469978" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632452462487469977" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632457412825385161" level="2" text="Metreos.MediaControl.PlayTone_Complete: OnPlayTone_Complete">
        <node type="function" name="OnPlayTone_Complete" id="632457412825385158" path="Metreos.StockTools" />
        <node type="event" name="PlayTone_Complete" id="632457412825385157" path="Metreos.MediaControl.PlayTone_Complete" />
        <references>
          <ref id="632472631815048260" actid="632457412825385167" />
          <ref id="632472631815048263" actid="632457412825385184" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632457412825385166" level="2" text="Metreos.MediaControl.PlayTone_Failed: OnPlayTone_Failed">
        <node type="function" name="OnPlayTone_Failed" id="632457412825385163" path="Metreos.StockTools" />
        <node type="event" name="PlayTone_Failed" id="632457412825385162" path="Metreos.MediaControl.PlayTone_Failed" />
        <references>
          <ref id="632472631815048261" actid="632457412825385167" />
          <ref id="632472631815048264" actid="632457412825385184" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="632472631815048223" vid="632452445701826997">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_originalTo" id="632472631815048225" vid="632452462487469435">
        <Properties type="String">g_originalTo</Properties>
      </treenode>
      <treenode text="g_callRecordsTable" id="632472631815048227" vid="632452462487469442">
        <Properties type="DataTable">g_callRecordsTable</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632472631815048229" vid="632452462487469445">
        <Properties type="String">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_mmsId" id="632472631815048231" vid="632452462487469447">
        <Properties type="Int">g_mmsId</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632472631815048233" vid="632452462487469449">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_playBeepTo" id="632472631815048235" vid="632452462487469963">
        <Properties type="String" defaultInitWith="NoOne" initWith="PlayBeepTo">g_playBeepTo</Properties>
      </treenode>
      <treenode text="g_beepFileName" id="632472631815048237" vid="632455313807653558">
        <Properties type="String" defaultInitWith="C:\Workspace\MediaServer\Audio\beep.wav">g_beepFileName</Properties>
      </treenode>
      <treenode text="g_amplitude1" id="632472631815048239" vid="632457412825385549">
        <Properties type="Int" initWith="Amplitude1">g_amplitude1</Properties>
      </treenode>
      <treenode text="g_amplitude2" id="632472631815048241" vid="632457412825385551">
        <Properties type="Int" initWith="Amplitude2">g_amplitude2</Properties>
      </treenode>
      <treenode text="g_frequency1" id="632472631815048243" vid="632457412825385553">
        <Properties type="UInt" initWith="Frequency1">g_frequency1</Properties>
      </treenode>
      <treenode text="g_frequency2" id="632472631815048245" vid="632457412825385555">
        <Properties type="UInt" initWith="Frequency2">g_frequency2</Properties>
      </treenode>
      <treenode text="g_duration" id="632472631815048247" vid="632457412825385557">
        <Properties type="UInt" initWith="ToneDuration">g_duration</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632452368884448231" treenode="632452368884448232" appnode="632452368884448229" handlerfor="632452368884448228">
    <node type="Start" id="632452368884448231" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="317">
      <linkto id="632452445701826996" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632452445701826996" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="174" y="318">
      <linkto id="632452462487469437" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632464275946754568" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref string originalTo, ref int lineInstance)
	{
		try
		{
			string lastChar = originalTo.Substring(originalTo.Length - 1);
			originalTo = originalTo.Substring(1, originalTo.Length - 2);
			lineInstance = Convert.ToInt32(lastChar);
			return IApp.VALUE_SUCCESS;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
	}

</Properties>
    </node>
    <node type="Action" id="632452462487469437" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="280" y="317">
      <linkto id="632464275946754566" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">callId</ap>
        <ap name="Value2" type="variable">originalTo</ap>
        <rd field="ResultData">g_callId</rd>
        <rd field="ResultData2">g_originalTo</rd>
      </Properties>
    </node>
    <node type="Action" id="632452462487469438" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="616" y="169">
      <linkto id="632457412825385184" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632458477908994479" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="MmsId" type="variable">g_mmsId</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <rd field="ConnectionId">g_connectionId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: took Conference path at switch, conference id is: " + g_conferenceId</log>
      </Properties>
    </node>
    <node type="Action" id="632452462487469444" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="496" y="315">
      <linkto id="632455313807653488" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632464275946754567" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="failure" on="true" level="Error" type="csharp">"OnIncomingCall: Failure. The value of conferenceId is: " + g_conferenceId</log>
	public static string Execute(DataTable g_callRecordsTable, ref string g_conferenceId, ref int g_mmsId)
	{
		try
		{
			DataRow row = g_callRecordsTable.Rows[0];
			if (Convert.IsDBNull(row[CBargeRecords.ConferenceId]))
				return IApp.VALUE_FAILURE;

			g_conferenceId = row[CBargeRecords.ConferenceId] as string;
			g_mmsId = Convert.ToInt32(row[CBargeRecords.MmsId]);

			if (g_conferenceId == null)
				return IApp.VALUE_FAILURE;

			return IApp.VALUE_SUCCESS;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
	}

</Properties>
    </node>
    <node type="Action" id="632452462487469968" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="908" y="513">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632452462487469969" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="904" y="167">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632455313807653488" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="615.4707" y="314">
      <linkto id="632452462487469438" type="Labeled" style="Bezier" ortho="true" label="Conference" />
      <linkto id="632455313807653489" type="Labeled" style="Bezier" ortho="true" label="NewPartyOnly" />
      <linkto id="632455313807653560" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">g_playBeepTo</ap>
      </Properties>
    </node>
    <node type="Action" id="632455313807653489" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="616" y="514">
      <linkto id="632457412825385167" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632458477908994480" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="MmsId" type="variable">g_mmsId</ap>
        <rd field="ConnectionId">g_connectionId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: took NewPartyOnly path at switch, conference id is: " + g_conferenceId
</log>
      </Properties>
    </node>
    <node type="Action" id="632455313807653554" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="908.4707" y="314">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632455313807653560" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="769" y="315">
      <linkto id="632455313807653554" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632458477908994480" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="MmsId" type="variable">g_mmsId</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <rd field="ConnectionId">g_connectionId</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: took default path at switch</log>
      </Properties>
    </node>
    <node type="Action" id="632457412825385167" name="PlayTone" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="701" y="498" mx="767" my="514">
      <items count="2">
        <item text="OnPlayTone_Complete" treenode="632457412825385161" />
        <item text="OnPlayTone_Failed" treenode="632457412825385166" />
      </items>
      <linkto id="632452462487469968" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Frequency1" type="variable">g_frequency1</ap>
        <ap name="TermCondMaxTime" type="literal">5000</ap>
        <ap name="Frequency2" type="variable">g_frequency2</ap>
        <ap name="Amplitude1" type="literal">-40</ap>
        <ap name="Amplitude2" type="literal">-40</ap>
        <ap name="Duration" type="variable">g_duration</ap>
        <ap name="UserData" type="literal">party</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: PlayTone invoked</log>
      </Properties>
    </node>
    <node type="Action" id="632457412825385184" name="PlayTone" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="705.4707" y="151" mx="772" my="167">
      <items count="2">
        <item text="OnPlayTone_Complete" treenode="632457412825385161" />
        <item text="OnPlayTone_Failed" treenode="632457412825385166" />
      </items>
      <linkto id="632452462487469969" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Frequency1" type="variable">g_frequency1</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="TermCondMaxTime" type="literal">5000</ap>
        <ap name="Frequency2" type="variable">g_frequency2</ap>
        <ap name="Amplitude1" type="variable">g_amplitude1</ap>
        <ap name="Amplitude2" type="variable">g_amplitude2</ap>
        <ap name="Duration" type="variable">g_duration</ap>
        <ap name="UserData" type="literal">conf</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: the value of g_duration is: " + g_duration.ToString()</log>
      </Properties>
    </node>
    <node type="Action" id="632458477908994478" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="442.4707" y="530">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: could not obtain conference parameters for call!</log>
      </Properties>
    </node>
    <node type="Action" id="632458477908994479" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="615.4707" y="55">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Error" type="literal">OnIncomingCall: Failed to answer call!</log>
      </Properties>
    </node>
    <node type="Action" id="632458477908994480" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="768.4707" y="408">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Error" type="literal">OnIncomingCall: Failed to answer call!</log>
      </Properties>
    </node>
    <node type="Action" id="632459598713239426" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="175" y="529">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Warning" type="literal">OnIncomingCall: Could not determine line instance to barge from originally dialed number.</log>
      </Properties>
    </node>
    <node type="Action" id="632464275946754566" name="GetBargeRecords" class="MaxActionNode" group="" path="Metreos.Applications.cBarge.NativeActions" x="383" y="316">
      <linkto id="632452462487469444" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632464275946754567" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="DirectoryNumber" type="variable">originalTo</ap>
        <ap name="LineInstance" type="variable">lineInstance</ap>
        <rd field="CallRecordsTable">g_callRecordsTable</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: Retrieving call records for directory number: " + originalTo + " and line instance: " + lineInstance</log>
      </Properties>
    </node>
    <node type="Action" id="632464275946754567" name="RejectCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="442" y="421">
      <linkto id="632458477908994478" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: rejecting incoming call</log>
      </Properties>
    </node>
    <node type="Action" id="632464275946754568" name="RejectCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="175" y="420">
      <linkto id="632459598713239426" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: rejecting incoming call</log>
      </Properties>
    </node>
    <node type="Variable" id="632452445701826995" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632452462487469434" name="originalTo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="OriginalTo" refType="reference" name="Metreos.CallControl.IncomingCall">originalTo</Properties>
    </node>
    <node type="Variable" id="632459598713239425" name="lineInstance" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">lineInstance</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632452462487469980" treenode="632452462487469981" appnode="632452462487469978" handlerfor="632452462487469977">
    <node type="Start" id="632452462487469980" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="56" y="308">
      <linkto id="632458943186777219" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632458943186777219" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="138" y="308">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnRemoteHangup: barge script exiting</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayTone_Complete" startnode="632457412825385160" treenode="632457412825385161" appnode="632457412825385158" handlerfor="632457412825385157">
    <node type="Start" id="632457412825385160" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="287">
      <linkto id="632457412825385171" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632457412825385170" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="270.6745" y="391">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnPlayTone_Complete: exiting function</log>
      </Properties>
    </node>
    <node type="Action" id="632457412825385171" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="148.674469" y="289">
      <linkto id="632457412825385172" type="Labeled" style="Bezier" ortho="true" label="party" />
      <linkto id="632457412825385170" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">userData</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayTone_Complete: Function entry.</log>
      </Properties>
    </node>
    <node type="Action" id="632457412825385172" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="269.674469" y="290">
      <linkto id="632457412825385170" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632458943186777216" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayTone_Complete: adding party to conference</log>
      </Properties>
    </node>
    <node type="Action" id="632458943186777216" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="396" y="288">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Error" type="literal">OnPlayTone_Complete: caller could not be added to conference</log>
      </Properties>
    </node>
    <node type="Variable" id="632457412825385183" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayTone_Failed" startnode="632457412825385165" treenode="632457412825385166" appnode="632457412825385163" handlerfor="632457412825385162">
    <node type="Start" id="632457412825385165" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="300">
      <linkto id="632457412825385177" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632457412825385176" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="262.6745" y="404">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632457412825385177" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="116.674477" y="302">
      <linkto id="632457412825385178" type="Labeled" style="Bezier" ortho="true" label="party" />
      <linkto id="632457412825385176" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">userData</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnPlayTone_Failed: Function entry.</log>
      </Properties>
    </node>
    <node type="Action" id="632457412825385178" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="262.674438" y="301">
      <linkto id="632457412825385176" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632458943186777217" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632458943186777217" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="416.942047" y="301">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Error" type="literal">OnPlayTone_Failed: caller could not be added to conference</log>
      </Properties>
    </node>
    <node type="Variable" id="632457412825385182" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>