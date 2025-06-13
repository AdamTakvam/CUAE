<Application name="CallControl" trigger="Metreos.Events.cBridge.HandleCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="CallControl">
    <outline>
      <treenode type="evh" id="632392235808125179" level="1" text="Metreos.Events.cBridge.HandleCall (trigger): OnHandleCall">
        <node type="function" name="OnHandleCall" id="632398483700922838" path="Metreos.StockTools" />
        <node type="event" name="HandleCall" id="632398483700922837" path="Metreos.Events.cBridge.HandleCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632393218473594144" level="2" text="Metreos.Providers.H323.SignallingChange: OnSignallingChange">
        <node type="function" name="OnSignallingChange" id="632393218473594141" path="Metreos.StockTools" />
        <node type="event" name="SignallingChange" id="632393218473594140" path="Metreos.Providers.H323.SignallingChange" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632398698077670371" level="2" text="Metreos.Providers.H323.Hangup: OnHangup">
        <node type="function" name="OnHangup" id="632398698077670368" path="Metreos.StockTools" />
        <node type="event" name="Hangup" id="632398698077670367" path="Metreos.Providers.H323.Hangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632398698077670387" level="2" text="Metreos.Providers.H323.Hangup_Complete: OnHangup_Complete">
        <node type="function" name="OnHangup_Complete" id="632398698077670384" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Complete" id="632398698077670383" path="Metreos.Providers.H323.Hangup_Complete" />
        <references>
          <ref id="632410496944481690" actid="632398698077670393" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632398698077670392" level="2" text="Metreos.Providers.H323.Hangup_Failed: OnHangup_Failed">
        <node type="function" name="OnHangup_Failed" id="632398698077670389" path="Metreos.StockTools" />
        <node type="event" name="Hangup_Failed" id="632398698077670388" path="Metreos.Providers.H323.Hangup_Failed" />
        <references>
          <ref id="632410496944481691" actid="632398698077670393" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632409765012272016" level="2" text="Metreos.Providers.MediaServer.PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632409765012272013" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632409765012272012" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <references>
          <ref id="632410496944481658" actid="632409765012272022" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632409765012272021" level="2" text="Metreos.Providers.MediaServer.PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632409765012272018" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632409765012272017" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <references>
          <ref id="632410496944481659" actid="632409765012272022" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_dbName" id="632410496944481601" vid="632392235808125180">
        <Properties type="String" initWith="DbName">g_dbName</Properties>
      </treenode>
      <treenode text="g_dbConnectionName" id="632410496944481603" vid="632392235808125182">
        <Properties type="String" initWith="DbConnectionName">g_dbConnectionName</Properties>
      </treenode>
      <treenode text="g_dbHost" id="632410496944481605" vid="632392235808125184">
        <Properties type="String" initWith="DbServer">g_dbHost</Properties>
      </treenode>
      <treenode text="g_dbPort" id="632410496944481607" vid="632392235808125186">
        <Properties type="String" initWith="DbPort">g_dbPort</Properties>
      </treenode>
      <treenode text="g_dbUsername" id="632410496944481609" vid="632392235808125188">
        <Properties type="String" initWith="DbUsername">g_dbUsername</Properties>
      </treenode>
      <treenode text="g_dbPassword" id="632410496944481611" vid="632392235808125190">
        <Properties type="String" initWith="DbPassword">g_dbPassword</Properties>
      </treenode>
      <treenode text="g_callIdMaster" id="632410496944481613" vid="632392235808125192">
        <Properties type="String">g_callIdMaster</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632410496944481615" vid="632392235808125237">
        <Properties type="Int">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_connectionIdMaster" id="632410496944481617" vid="632392235808125246">
        <Properties type="Int">g_connectionIdMaster</Properties>
      </treenode>
      <treenode text="g_to" id="632410496944481619" vid="632392235808125254">
        <Properties type="String">g_to</Properties>
      </treenode>
      <treenode text="g_from" id="632410496944481621" vid="632392235808125256">
        <Properties type="String">g_from</Properties>
      </treenode>
      <treenode text="g_isRecorded" id="632410496944481623" vid="632397151914076564">
        <Properties type="Bool">g_isRecorded</Properties>
      </treenode>
      <treenode text="g_timeStamp" id="632410496944481625" vid="632397151914076566">
        <Properties type="Long" defaultInitWith="0">g_timeStamp</Properties>
      </treenode>
      <treenode text="g_lineId" id="632410496944481627" vid="632397175889232823">
        <Properties type="String">g_lineId</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="632410496944481629" vid="632397175889232826">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
      <treenode text="g_callType" id="632410496944481631" vid="632397175889232842">
        <Properties type="String">g_callType</Properties>
      </treenode>
      <treenode text="g_connectionMap" id="632410496944481633" vid="632398698077670376">
        <Properties type="Hashtable">g_connectionMap</Properties>
      </treenode>
      <treenode text="g_announceToConf" id="632410496944481635" vid="632409765012271555">
        <Properties type="Bool" initWith="AnnounceToConf">g_announceToConf</Properties>
      </treenode>
      <treenode text="g_numSleepSeconds" id="632410496944481637" vid="632410496944481567">
        <Properties type="Int" initWith="UpdateConfSleep">g_numSleepSeconds</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnHandleCall" startnode="632392235808125178" treenode="632392235808125179" appnode="632398483700922838" handlerfor="632398483700922837">
    <node type="Start" id="632392235808125178" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="325">
      <linkto id="632398595693266667" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632397151914076570" text="O" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="39.4707031" y="620">
      <linkto id="632398641792358105" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632397151914076572" text="I" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="39" y="530">
      <linkto id="632398641792358105" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632397175889232831" text="What if call rings out before other party answers?" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="462" y="90" />
    <node type="Comment" id="632397175889232845" text="MAKE SURE TO CREATE WAV FILE" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="507" y="139" />
    <node type="Action" id="632398483700922839" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="297" y="324">
      <linkto id="632398641792358100" type="Labeled" style="Bezier" ortho="true" label="incoming" />
      <linkto id="632398641792358101" type="Labeled" style="Bezier" ortho="true" label="outgoing" />
      <linkto id="632398641792358103" type="Labeled" style="Bezier" ortho="true" label="bridge" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">callType</ap>
      </Properties>
    </node>
    <node type="Action" id="632398595693266667" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="115" y="325">
      <linkto id="632398483700922839" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="respondingGuid" type="variable">routingGuid</ap>
        <ap name="EventName" type="literal">Metreos.Events.cBridge.HandleCallResponse</ap>
        <ap name="ToGuid" type="variable">requestingGuid</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHandleCall: sending response back with routingGuid...</log>
      </Properties>
    </node>
    <node type="Label" id="632398641792358100" text="I" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="298" y="132" />
    <node type="Label" id="632398641792358101" text="O" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="430" y="132" />
    <node type="Label" id="632398641792358103" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="169" y="132" />
    <node type="Action" id="632398641792358105" name="CreateConnectionConference" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="155" y="530">
      <linkto id="632398641792358106" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionIdMaster</ap>
        <ap name="conferenceId" type="literal">0</ap>
        <rd field="connectionId">g_connectionIdMaster</rd>
        <rd field="conferenceId">g_conferenceId</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnHandleCall: Adding master to conference</log>
      </Properties>
    </node>
    <node type="Action" id="632398641792358106" name="CreateConnectionConference" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="343" y="530">
      <linkto id="632398641792358107" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionIdSlave</ap>
        <ap name="conferenceId" type="variable">g_conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632398641792358107" name="CreateConference" class="MaxActionNode" group="" path="Metreos.Native.CBridge" x="500" y="531">
      <linkto id="632398698077670378" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">lineId</ap>
        <ap name="IsRecorded" type="literal">false</ap>
        <ap name="RoutingGuid" type="variable">routingGuid</ap>
        <rd field="Timestamp">g_timeStamp</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnHandleCall: Creating conference record</log>
      </Properties>
    </node>
    <node type="Action" id="632398683558451615" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="736" y="530">
      <linkto id="632398683558451617" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">callType</ap>
        <ap name="Value2" type="variable">routingGuid</ap>
        <ap name="Value3" type="variable">callIdMaster</ap>
        <ap name="Value4" type="variable">lineId</ap>
        <rd field="ResultData">g_callType</rd>
        <rd field="ResultData2">g_routingGuid</rd>
        <rd field="ResultData3">g_callIdMaster</rd>
        <rd field="ResultData4">g_lineId</rd>
      </Properties>
    </node>
    <node type="Action" id="632398683558451617" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="844" y="531">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Label" id="632398683558451618" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="35" y="768">
      <linkto id="632398698077670364" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632398698077670364" name="CreateConnectionConference" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="165" y="768">
      <linkto id="632398698077670379" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionIdSlave</ap>
        <ap name="conferenceId" type="variable">g_conferenceId</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHandleCall: Adding bridge to conference...</log>
      </Properties>
    </node>
    <node type="Action" id="632398698077670378" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="625" y="531">
      <linkto id="632398683558451615" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="csharp">"OnHandleCall: adding slave with connectionId: " + connectionIdSlave + " to callId-connectionId map."</log>
	public static string Execute(ref Hashtable g_connectionMap, string callIdSlave, int connectionIdSlave, LogWriter log)
	{
		if (g_connectionMap == null)
			g_connectionMap = new Hashtable();
		try
		{
			log.Write(TraceLevel.Info, "Slave callId: " + callIdSlave + ". Slave connectionId: " + Convert.ToString(connectionIdSlave));
			g_connectionMap.Add(callIdSlave, connectionIdSlave);
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
		
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632398698077670379" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="305" y="768">
      <linkto id="632409765012272025" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">OnHandleCall: adding bridge slave to callId-connectionId map</log>	public static string Execute(Hashtable g_connectionMap, string callIdSlave, int connectionIdSlave)
	{
		try
		{
			g_connectionMap.Add(callIdSlave, connectionIdSlave);
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
		
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632409765012272022" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="468" y="750" mx="561" my="766">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632409765012272016" />
        <item text="OnPlayAnnouncement_Failed" treenode="632409765012272021" />
      </items>
      <linkto id="632410424886044209" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxTime" type="literal">10000</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="conferenceId" type="variable">g_conferenceId</ap>
        <ap name="filename" type="literal">beep.wav</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHandleCall: playing tone to conference</log>
      </Properties>
    </node>
    <node type="Action" id="632409765012272025" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="413" y="767">
      <linkto id="632409765012272022" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632410424886044209" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_announceToConf</ap>
      </Properties>
    </node>
    <node type="Action" id="632410424886044209" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="413" y="902">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632392235808125239" name="lineId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="lineId" refType="reference">lineId</Properties>
    </node>
    <node type="Variable" id="632392235808125240" name="timestamp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Long" initWith="timestamp" refType="reference">timestamp</Properties>
    </node>
    <node type="Variable" id="632392235808125241" name="callType" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callType" refType="reference">callType</Properties>
    </node>
    <node type="Variable" id="632392235808125244" name="callIdMaster" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callIdMaster" defaultInitWith="NONE" refType="reference">callIdMaster</Properties>
    </node>
    <node type="Variable" id="632392235808125245" name="callIdSlave" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callIdSlave" refType="reference">callIdSlave</Properties>
    </node>
    <node type="Variable" id="632392235808125279" name="connectionIdMaster" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="connectionIdMaster" defaultInitWith="0" refType="reference">connectionIdMaster</Properties>
    </node>
    <node type="Variable" id="632392235808125280" name="connectionIdSlave" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="connectionIdSlave" refType="reference">connectionIdSlave</Properties>
    </node>
    <node type="Variable" id="632398595693266668" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632398600382485336" name="requestingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="requestingGuid" refType="reference">requestingGuid</Properties>
    </node>
    <node type="Variable" id="632398617403891835" name="fromNumberMaster" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="fromNumberMaster" defaultInitWith="0" refType="reference">fromNumberMaster</Properties>
    </node>
    <node type="Variable" id="632398617403891836" name="fromNumberSlave" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="fromNumberSlave" refType="reference">fromNumberSlave</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSignallingChange" activetab="true" startnode="632393218473594143" treenode="632393218473594144" appnode="632393218473594141" handlerfor="632393218473594140">
    <node type="Start" id="632393218473594143" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="320">
      <linkto id="632405516153521539" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632397175889232836" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="420" y="322">
      <linkto id="632399169121861418" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="variable">mediaPort</ap>
        <ap name="connectionId" type="variable">connectionId</ap>
        <ap name="callId" type="variable">callId</ap>
        <ap name="remoteIp" type="variable">mediaIP</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnSingalingChange: rebinding connectionId for callId: " + callId + " and connectionId: " + connectionId</log>
      </Properties>
    </node>
    <node type="Action" id="632399169121861416" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="279" y="322">
      <linkto id="632397175889232836" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties language="csharp">
	public static string Execute(Hashtable g_connectionMap, ref int connectionId, string callId, LogWriter log)
	{
		try
		{
			connectionId = Convert.ToInt32(g_connectionMap[callId]);
			log.Write(TraceLevel.Info, "ConnectionId to rebind is: " + Convert.ToString(connectionId));
			return IApp.VALUE_SUCCESS;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
	}
</Properties>
    </node>
    <node type="Action" id="632399169121861418" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="556" y="323">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632405516153521539" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="146" y="321">
      <linkto id="632399169121861416" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632405516153521540" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_callIdMaster</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnSignalingChange: callId: " + callId + " ip: " + mediaIP + " port: " + mediaPort.ToString() + " guid " + routingGuid</log>
      </Properties>
    </node>
    <node type="Action" id="632405516153521540" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="147" y="458">
      <linkto id="632405611167427470" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632405516153521541" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="variable">mediaPort</ap>
        <ap name="connectionId" type="variable">g_connectionIdMaster</ap>
        <ap name="callId" type="variable">g_callIdMaster</ap>
        <ap name="remoteIp" type="variable">mediaIP</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnSignalingChange: signaling change for master callId: " + g_callIdMaster + " with connectionId " + g_connectionIdMaster</log>
      </Properties>
    </node>
    <node type="Action" id="632405516153521541" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="411" y="458">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632405611167427470" name="UpdateConference" class="MaxActionNode" group="" path="Metreos.Native.CBridge" x="285" y="555">
      <linkto id="632405516153521541" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">g_lineId</ap>
        <ap name="Timestamp" type="variable">g_timeStamp</ap>
        <ap name="RoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="SleepTime" type="variable">g_numSleepSeconds</ap>
        <rd field="NewTimestamp">g_timeStamp</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnSignalingChange: UpdateConference for lineId: " + g_lineId + " and timestamp: " + g_timeStamp.ToString() + ". The routingGuid of this instance is: " + routingGuid</log>
        <log condition="exit" on="true" level="Info" type="csharp">"OnSignalingChange: UpdateConference returned for lineId: " + g_lineId + " with new timestamp: " + g_timeStamp.ToString() + ". The routingGuid of this instance is: " + routingGuid</log>
      </Properties>
    </node>
    <node type="Variable" id="632397175889232833" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632397175889232834" name="mediaIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="mediaIP" refType="reference">mediaIP</Properties>
    </node>
    <node type="Variable" id="632397175889232835" name="mediaPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UShort" initWith="mediaPort" refType="reference">mediaPort</Properties>
    </node>
    <node type="Variable" id="632399169121861417" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">connectionId</Properties>
    </node>
    <node type="Variable" id="632405516153521542" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup" startnode="632398698077670370" treenode="632398698077670371" appnode="632398698077670368" handlerfor="632398698077670367">
    <node type="Loop" id="632405559928208719" name="Loop" text="loop (var)" cx="256" cy="252" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="599.2388" y="192" mx="727" my="318">
      <linkto id="632398698077670393" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632398698077670403" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="dictEnum" type="variable">g_connectionMap</Properties>
    </node>
    <node type="Start" id="632398698077670370" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="317">
      <linkto id="632398698077670375" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632398698077670375" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="127" y="317">
      <linkto id="632398698077670381" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632398698077670399" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_callIdMaster</ap>
      </Properties>
    </node>
    <node type="Action" id="632398698077670381" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="248" y="317">
      <linkto id="632398698077670396" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="conferenceId" type="variable">g_conferenceId</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Deleting conference</log>
        <log condition="failure" on="true" level="Error" type="literal">OnHangup: FAILED TO DELETE CONFERENCE</log>
      </Properties>
    </node>
    <node type="Action" id="632398698077670393" name="Hangup" container="632405559928208719" class="MaxAsyncActionNode" group="" path="Metreos.Providers.H323" x="653" y="302" mx="715" my="318">
      <items count="2">
        <item text="OnHangup_Complete" treenode="632398698077670387" />
        <item text="OnHangup_Failed" treenode="632398698077670392" />
      </items>
      <linkto id="632405559928208719" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="csharp">loopDictEnum.Key as string;</ap>
        <ap name="userData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnHangup: hanging up call with callId: " + (loopDictEnum.Key as string)</log>
      </Properties>
    </node>
    <node type="Action" id="632398698077670396" name="DestroyConference" class="MaxActionNode" group="" path="Metreos.Native.CBridge" x="394" y="316">
      <linkto id="632405559928208717" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">g_lineId</ap>
        <ap name="Timestamp" type="variable">g_timeStamp</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: Destroying conference record</log>
      </Properties>
    </node>
    <node type="Action" id="632398698077670399" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="127" y="520">
      <linkto id="632398698077670400" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">OnHangup: looking up connectionId for hanging-up user...</log>
	public static string Execute(Hashtable g_connectionMap, ref int connectionIdToDelete, string callId, LogWriter log)
	{
		try
		{
			connectionIdToDelete = Convert.ToInt32(g_connectionMap[callId]);
			log.Write(TraceLevel.Info, "Slave callId: " + callId + ". Slave 					connectionId: " + Convert.ToString(connectionIdToDelete));
			g_connectionMap.Remove(callId);
			return IApp.VALUE_SUCCESS;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
	}
</Properties>
    </node>
    <node type="Action" id="632398698077670400" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="302" y="520">
      <linkto id="632405265288850096" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionIdToDelete</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnHangup: Deleting MMS connection with connectionId: " + connectionIdToDelete</log>
      </Properties>
    </node>
    <node type="Action" id="632398698077670403" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="948.768066" y="318">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632405265288850096" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="439" y="521">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632405559928208717" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="507.23877" y="318">
      <linkto id="632405559928208718" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632405559928208719" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">g_connectionMap.Count == 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632405559928208718" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="506.23877" y="150">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632398698077670372" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632398698077670398" name="connectionIdToDelete" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">connectionIdToDelete</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup_Complete" startnode="632398698077670386" treenode="632398698077670387" appnode="632398698077670384" handlerfor="632398698077670383">
    <node type="Start" id="632398698077670386" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="417">
      <linkto id="632404555731062473" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632398698077670402" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="254" y="533">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632398698077670407" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="254" y="413">
      <linkto id="632398698077670402" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632398698077670408" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">g_connectionMap.Count == 0</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup_Complete: hangup succeeded</log>
      </Properties>
    </node>
    <node type="Action" id="632398698077670408" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="395" y="415">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632404555731062473" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="144.90625" y="414">
      <linkto id="632398698077670407" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="csharp">"OnHangup_Complete: Removing callId: " + callId + " from callId-&gt;connectionId map."</log>
	public static string Execute(Hashtable g_connectionMap, string callId)
	{
		try
		{
			g_connectionMap.Remove(callId);
			return IApp.VALUE_SUCCESS;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
	}

</Properties>
    </node>
    <node type="Variable" id="632404555731062475" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callId" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup_Failed" startnode="632398698077670391" treenode="632398698077670392" appnode="632398698077670389" handlerfor="632398698077670388">
    <node type="Start" id="632398698077670391" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="69" y="375">
      <linkto id="632404555731062472" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632398698077670409" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="277.4707" y="496">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632398698077670410" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="275.4707" y="374">
      <linkto id="632398698077670409" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632398698077670411" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">g_connectionMap.Count == 0</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnHangup_Complete: hangup failed</log>
      </Properties>
    </node>
    <node type="Action" id="632398698077670411" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="410.4707" y="373">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632404555731062472" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="169" y="374">
      <linkto id="632398698077670410" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="csharp">"OnHangup_Failed: Removing callId: " + callId + " from callId-&gt;connectionId map."</log>
	public static string Execute(Hashtable g_connectionMap, string callId)
	{
		try
		{
			g_connectionMap.Remove(callId);
			return IApp.VALUE_SUCCESS;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
	}

</Properties>
    </node>
    <node type="Variable" id="632404555731062471" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callId" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Complete" startnode="632409765012272015" treenode="632409765012272016" appnode="632409765012272013" handlerfor="632409765012272012">
    <node type="Start" id="632409765012272015" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="326">
      <linkto id="632410424886044210" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632410424886044210" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="154" y="324">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Failed" startnode="632409765012272020" treenode="632409765012272021" appnode="632409765012272018" handlerfor="632409765012272017">
    <node type="Start" id="632409765012272020" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="363">
      <linkto id="632410424886044211" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632410424886044211" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="134" y="363">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>