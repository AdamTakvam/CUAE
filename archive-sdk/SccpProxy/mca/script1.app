<Application name="script1" trigger="Metreos.Providers.SccpProxy.Register" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632433835678272432" level="1" text="Metreos.Providers.SccpProxy.Register (trigger): OnRegister">
        <node type="function" name="OnRegister" id="632433835678272429" path="Metreos.StockTools" />
        <node type="event" name="Register" id="632433835678272428" path="Metreos.Providers.SccpProxy.Register" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632434838463593707" level="2" text="Metreos.Providers.SccpProxy.RegisterAck: OnRegisterAck">
        <node type="function" name="OnRegisterAck" id="632434838463593704" path="Metreos.StockTools" />
        <node type="event" name="RegisterAck" id="632434838463593703" path="Metreos.Providers.SccpProxy.RegisterAck" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632434838463593728" level="2" text="Metreos.Providers.SccpProxy.SessionFailure: OnSessionFailure">
        <node type="function" name="OnSessionFailure" id="632434838463593725" path="Metreos.StockTools" />
        <node type="event" name="SessionFailure" id="632434838463593724" path="Metreos.Providers.SccpProxy.SessionFailure" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632435638902512121" level="2" text="Metreos.Providers.SccpProxy.OpenReceiveChannelAck: OnOpenReceiveChannelAck">
        <node type="function" name="OnOpenReceiveChannelAck" id="632435638902512118" path="Metreos.StockTools" />
        <node type="event" name="OpenReceiveChannelAck" id="632435638902512117" path="Metreos.Providers.SccpProxy.OpenReceiveChannelAck" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632435638902512131" level="2" text="Metreos.Providers.SccpProxy.StartMediaTransmission: OnStartMediaTransmission">
        <node type="function" name="OnStartMediaTransmission" id="632435638902512128" path="Metreos.StockTools" />
        <node type="event" name="StartMediaTransmission" id="632435638902512127" path="Metreos.Providers.SccpProxy.StartMediaTransmission" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632449548638951360" level="2" text="Metreos.Providers.SccpProxy.CallState: OnCallState">
        <node type="function" name="OnCallState" id="632449548638951357" path="Metreos.StockTools" />
        <node type="event" name="CallState" id="632449548638951356" path="Metreos.Providers.SccpProxy.CallState" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_sid" id="632564421400145112" vid="632451429663860298">
        <Properties type="String" defaultInitWith="string.Empty()">g_sid</Properties>
      </treenode>
      <treenode text="g_ccmIP" id="632564421400145114" vid="632489933881824714">
        <Properties type="String" initWith="CCM_IP">g_ccmIP</Properties>
      </treenode>
      <treenode text="g_ccmPort" id="632564421400145116" vid="632489933881824716">
        <Properties type="Int" initWith="CCM_Port">g_ccmPort</Properties>
      </treenode>
      <treenode text="g_numRingIn" id="632564421400145118" vid="632506423628045635">
        <Properties type="Int">g_numRingIn</Properties>
      </treenode>
      <treenode text="g_numRingOut" id="632564421400145120" vid="632506423628045637">
        <Properties type="Int">g_numRingOut</Properties>
      </treenode>
      <treenode text="g_numBusy" id="632564421400145122" vid="632506423628045639">
        <Properties type="Int">g_numBusy</Properties>
      </treenode>
      <treenode text="g_numConnected" id="632564421400145124" vid="632506423628045641">
        <Properties type="Int">g_numConnected</Properties>
      </treenode>
      <treenode text="g_configDbName" id="632564421400145126" vid="632506423628045645">
        <Properties type="String" initWith="Db_Name">g_configDbName</Properties>
      </treenode>
      <treenode text="g_configDbUser" id="632564421400145128" vid="632506423628045647">
        <Properties type="String" initWith="Db_User">g_configDbUser</Properties>
      </treenode>
      <treenode text="g_configDbPass" id="632564421400145130" vid="632506423628045649">
        <Properties type="String" initWith="Db_Pass">g_configDbPass</Properties>
      </treenode>
      <treenode text="g_configDbAddress" id="632564421400145132" vid="632506423628045651">
        <Properties type="String" initWith="Db_Hostname">g_configDbAddress</Properties>
      </treenode>
      <treenode text="g_configDbType" id="632564421400145134" vid="632506423628045784">
        <Properties type="String" initWith="Db_Type">g_configDbType</Properties>
      </treenode>
      <treenode text="g_registrationRowId" id="632564421400145136" vid="632506499182809841">
        <Properties type="Int" defaultInitWith="0">g_registrationRowId</Properties>
      </treenode>
      <treenode text="g_dbConnection" id="632564421400145138" vid="632507988738484097">
        <Properties type="ArrayList">g_dbConnection</Properties>
      </treenode>
      <treenode text="g_logToDb" id="632564421400145175" vid="632564421400145174">
        <Properties type="Bool" initWith="Log_to_db">g_logToDb</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnRegister" startnode="632433835678272431" treenode="632433835678272432" appnode="632433835678272429" handlerfor="632433835678272428">
    <node type="Start" id="632433835678272431" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="240">
      <linkto id="632451850060572184" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632433835678272433" name="Register" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="267" y="238">
      <linkto id="632564421400145176" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Sid" type="variable">g_sid</ap>
        <ap name="ToIp" type="variable">g_ccmIP</ap>
        <ap name="ToPort" type="variable">g_ccmPort</ap>
        <ap name="Subscribe" type="literal">RegisterAck</ap>
        <ap name="Subscribe" type="literal">OpenReceiveChannelAck</ap>
        <ap name="Subscribe" type="literal">StartMediaTransmission</ap>
        <ap name="Subscribe" type="literal">CallState</ap>
      </Properties>
    </node>
    <node type="Action" id="632434838463593709" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1036" y="243">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632451850060572184" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="160" y="240">
      <linkto id="632433835678272433" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">l_sid</ap>
        <rd field="ResultData">g_sid</rd>
      </Properties>
    </node>
    <node type="Action" id="632506423628045644" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="462" y="239">
      <linkto id="632506423628045781" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="variable">g_configDbType</ap>
        <ap name="DatabaseName" type="variable">g_configDbName</ap>
        <ap name="Server" type="variable">g_configDbAddress</ap>
        <ap name="Username" type="variable">g_configDbUser</ap>
        <ap name="Password" type="variable">g_configDbPass</ap>
        <ap name="Pooling" type="csharp">true</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632506423628045781" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="608" y="240">
      <linkto id="632506499182809847" type="Labeled" style="Bezier" label="default" />
      <linkto id="632538547619143371" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">registrations</ap>
        <ap name="Type" type="variable">g_configDbType</ap>
      </Properties>
    </node>
    <node type="Action" id="632506499182809847" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="607" y="439">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="literal">Unable to create database registration entry.</log>
      </Properties>
    </node>
    <node type="Action" id="632538547619143371" name="WriteRegistrationStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="814.4707" y="241">
      <linkto id="632434838463593709" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632506499182809847" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Sid" type="variable">g_sid</ap>
        <ap name="CallManagerIp" type="variable">g_ccmIP</ap>
        <rd field="RegistrationId">g_registrationRowId</rd>
      </Properties>
    </node>
    <node type="Action" id="632564421400145176" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="352" y="237">
      <linkto id="632506423628045644" type="Labeled" style="Vector" label="true" />
      <linkto id="632564421400145177" type="Labeled" style="Vector" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_logToDb</ap>
      </Properties>
    </node>
    <node type="Action" id="632564421400145177" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="357" y="388">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632451429663860300" name="l_sid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Sid" refType="reference">l_sid</Properties>
    </node>
    <node type="Variable" id="632506423628045643" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRegisterAck" startnode="632434838463593706" treenode="632434838463593707" appnode="632434838463593704" handlerfor="632434838463593703">
    <node type="Start" id="632434838463593706" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632434838463593711" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632434838463593711" name="RegisterAck" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="191" y="95">
      <linkto id="632434838463593712" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Sid" type="variable">g_sid</ap>
      </Properties>
    </node>
    <node type="Action" id="632434838463593712" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="427" y="105">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionFailure" activetab="true" startnode="632434838463593727" treenode="632434838463593728" appnode="632434838463593725" handlerfor="632434838463593724">
    <node type="Start" id="632434838463593727" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="224">
      <linkto id="632506499182809848" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632506499182809831" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="325.188782" y="224">
      <linkto id="632538547619143372" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="variable">g_configDbType</ap>
        <ap name="DatabaseName" type="variable">g_configDbName</ap>
        <ap name="Server" type="variable">g_configDbAddress</ap>
        <ap name="Username" type="variable">g_configDbUser</ap>
        <ap name="Password" type="variable">g_configDbPass</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632506499182809840" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="603" y="225">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632506499182809848" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="216" y="224">
      <linkto id="632506499182809831" type="Labeled" style="Bezier" label="true" />
      <linkto id="632506499182809849" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_registrationRowId != 0 &amp;&amp; g_logToDb</ap>
      </Properties>
    </node>
    <node type="Action" id="632506499182809849" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="217" y="397">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">SessionFailure: Exiting without attempting to record unregistration statistics because record id is undefined.</log>
      </Properties>
    </node>
    <node type="Action" id="632538547619143372" name="WriteRegistrationStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="444" y="224">
      <linkto id="632506499182809840" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="NumRingIn" type="variable">g_numRingIn</ap>
        <ap name="NumRingOut" type="variable">g_numRingOut</ap>
        <ap name="NumBusy" type="variable">g_numBusy</ap>
        <ap name="NumConnected" type="variable">g_numConnected</ap>
        <ap name="RegistrationId" type="variable">g_registrationRowId</ap>
      </Properties>
    </node>
    <node type="Variable" id="632506499182809845" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnOpenReceiveChannelAck" startnode="632435638902512120" treenode="632435638902512121" appnode="632435638902512118" handlerfor="632435638902512117">
    <node type="Start" id="632435638902512120" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632435638902512139" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632435638902512139" name="OpenReceiveChannelAck" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="263" y="115">
      <linkto id="632435638902512148" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaIp" type="literal">127.0.0.1</ap>
        <ap name="MediaPort" type="literal">0</ap>
        <ap name="Sid" type="variable">g_sid</ap>
      </Properties>
    </node>
    <node type="Action" id="632435638902512148" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="445" y="114">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartMediaTransmission" startnode="632435638902512130" treenode="632435638902512131" appnode="632435638902512128" handlerfor="632435638902512127">
    <node type="Start" id="632435638902512130" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632435638902512141" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632435638902512141" name="StartMediaTransmission" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="271" y="82">
      <linkto id="632435638902512147" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaPort" type="literal">0</ap>
        <ap name="Sid" type="variable">g_sid</ap>
        <ap name="MediaIp" type="literal">127.0.0.1</ap>
      </Properties>
    </node>
    <node type="Action" id="632435638902512147" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="508" y="77">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnCallState" startnode="632449548638951359" treenode="632449548638951360" appnode="632449548638951357" handlerfor="632449548638951356">
    <node type="Start" id="632449548638951359" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="288">
      <linkto id="632449548638951367" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632449548638951367" name="CallState" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="240" y="288">
      <linkto id="632506423628045633" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Sid" type="variable">g_sid</ap>
      </Properties>
    </node>
    <node type="Action" id="632449548638951368" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="680" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632506423628045633" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="400" y="288">
      <linkto id="632449548638951368" type="Labeled" style="Bezier" label="default" />
      <linkto id="632506423628045923" type="Labeled" style="Bezier" label="RingIn" />
      <linkto id="632506423628045924" type="Labeled" style="Bezier" label="RingOut" />
      <linkto id="632506423628045925" type="Labeled" style="Bezier" label="Busy" />
      <linkto id="632506423628045926" type="Labeled" style="Bezier" label="Connected" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">callState</ap>
      </Properties>
    </node>
    <node type="Action" id="632506423628045923" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="488" y="64">
      <linkto id="632449548638951368" type="Labeled" style="Bezier" label="default" />
      <Properties language="csharp">
public static string Execute(ref int g_numRingIn)
{
	g_numRingIn++;

	return String.Empty;
}
</Properties>
    </node>
    <node type="Action" id="632506423628045924" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="563" y="183">
      <linkto id="632449548638951368" type="Labeled" style="Bezier" label="default" />
      <Properties language="csharp">
public static string Execute(ref int g_numRingOut)
{
	g_numRingOut++;

	return String.Empty;
}
</Properties>
    </node>
    <node type="Action" id="632506423628045925" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="576" y="368">
      <linkto id="632449548638951368" type="Labeled" style="Bezier" label="default" />
      <Properties language="csharp">
public static string Execute(ref int g_numBusy)
{
	g_numBusy++;

	return String.Empty;
}
</Properties>
    </node>
    <node type="Action" id="632506423628045926" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="504" y="488">
      <linkto id="632449548638951368" type="Labeled" style="Bezier" label="default" />
      <Properties language="csharp">
public static string Execute(ref int g_numConnected)
{
	g_numConnected++;

	return String.Empty;
}
</Properties>
    </node>
    <node type="Variable" id="632506423628045523" name="callState" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallState" refType="reference">callState</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>