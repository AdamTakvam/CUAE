<Application name="OutboundCall" trigger="Metreos.Providers.JTapi.JTapiCallInitiated" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="OutboundCall">
    <outline>
      <treenode type="evh" id="632575510106029538" level="1" text="Metreos.Providers.JTapi.JTapiCallInitiated (trigger): OnJTapiCallInitiated">
        <node type="function" name="OnJTapiCallInitiated" id="632575510106029535" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallInitiated" id="632575510106029534" path="Metreos.Providers.JTapi.JTapiCallInitiated" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575523141979606" level="2" text="Metreos.Providers.JTapi.JTapiCallActive: OnJTapiCallActive">
        <node type="function" name="OnJTapiCallActive" id="632575523141979603" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallActive" id="632575523141979602" path="Metreos.Providers.JTapi.JTapiCallActive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575523141979611" level="2" text="Metreos.Providers.JTapi.JTapiCallInactive: OnJTapiCallInactive">
        <node type="function" name="OnJTapiCallInactive" id="632575523141979608" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallInactive" id="632575523141979607" path="Metreos.Providers.JTapi.JTapiCallInactive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575523141979616" level="2" text="Metreos.Providers.JTapi.JTapiHangup: OnJTapiHangup">
        <node type="function" name="OnJTapiHangup" id="632575523141979613" path="Metreos.StockTools" />
        <node type="event" name="JTapiHangup" id="632575523141979612" path="Metreos.Providers.JTapi.JTapiHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575523141979621" level="2" text="Metreos.Providers.JTapi.JTapiGotDigits: OnJTapiGotDigits">
        <node type="function" name="OnJTapiGotDigits" id="632575523141979618" path="Metreos.StockTools" />
        <node type="event" name="JTapiGotDigits" id="632575523141979617" path="Metreos.Providers.JTapi.JTapiGotDigits" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632628246963717012" level="2" text="Metreos.Providers.TimerFacility.TimerFire: OnTimerFire">
        <node type="function" name="OnTimerFire" id="632628246963717009" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632628246963717008" path="Metreos.Providers.TimerFacility.TimerFire" />
        <references>
          <ref id="632629260761032445" actid="632628246963717016" />
          <ref id="632629260761032468" actid="632628246963717051" />
        </references>
        <Properties type="hybrid">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_TimerDelay" id="632629260761032408" vid="632575510106029571">
        <Properties type="Int" defaultInitWith="0" initWith="ReversionDelay">g_TimerDelay</Properties>
      </treenode>
      <treenode text="g_TimerPeriodicity" id="632629260761032410" vid="632627251825472645">
        <Properties type="Int" defaultInitWith="0" initWith="ReversionInterval">g_TimerPeriodicity</Properties>
      </treenode>
      <treenode text="g_TimerId" id="632629260761032412" vid="632575510106029576">
        <Properties type="String">g_TimerId</Properties>
      </treenode>
      <treenode text="g_TimerCreated" id="632629260761032414" vid="632627251825472713">
        <Properties type="Bool" defaultInitWith="false">g_TimerCreated</Properties>
      </treenode>
      <treenode text="g_PhoneIP" id="632629260761032416" vid="632575523141979023">
        <Properties type="String">g_PhoneIP</Properties>
      </treenode>
      <treenode text="g_Username" id="632629260761032418" vid="632627251825472812">
        <Properties type="String" initWith="Username">g_Username</Properties>
      </treenode>
      <treenode text="g_Password" id="632629260761032420" vid="632627251825472808">
        <Properties type="String" initWith="Password">g_Password</Properties>
      </treenode>
      <treenode text="g_Filename" id="632629260761032422" vid="632627251825472810">
        <Properties type="String" initWith="Filename">g_Filename</Properties>
      </treenode>
      <treenode text="g_ExecuteMsg" id="632629260761032424" vid="632627251825472814">
        <Properties type="Metreos.Types.CiscoIpPhone.Execute">g_ExecuteMsg</Properties>
      </treenode>
      <treenode text="g_ExecuteCreated" id="632629260761032426" vid="632627251825472816">
        <Properties type="Bool" defaultInitWith="false">g_ExecuteCreated</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnJTapiCallInitiated" startnode="632575510106029537" treenode="632575510106029538" appnode="632575510106029535" handlerfor="632575510106029534">
    <node type="Start" id="632575510106029537" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="173">
      <linkto id="632628246963716991" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632628246963716990" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="474.942017" y="172">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632628246963716991" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="165.942047" y="172">
      <linkto id="632628246963716992" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632628246963716993" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">deviceName</ap>
        <rd field="ResultData">dlxResult</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnJTapiIncomingCall: Querying DLX for device: " + deviceName</log>
        <log condition="default" on="true" level="Warning" type="csharp">"OnJTapiIncomingCall: DeviceListX query FAILED for device: " + deviceName</log>
      </Properties>
    </node>
    <node type="Action" id="632628246963716992" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="296.942047" y="172">
      <linkto id="632628246963716990" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632628246963716993" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(DataTable dlxResult, ref string g_PhoneIP, LogWriter log)
{
    g_PhoneIP = String.Empty;

    if(dlxResult == null || dlxResult.Rows == null)
    {
	  log.Write(TraceLevel.Warning, "OnJTapiIncomingCall: DeviceListX query returned empty data set.");
        return IApp.VALUE_FAILURE;
    }
    else if (dlxResult.Rows.Count != 1)
    {
        log.Write(TraceLevel.Warning, "OnJTapiIncomingCall: DeviceListX query returned more than one matching record. Cannot proceed.");
        return IApp.VALUE_FAILURE;
    }

    g_PhoneIP = dlxResult.Rows[0][ICiscoDeviceList.FIELD_IP] as string;

    if(g_PhoneIP == null)
        g_PhoneIP = String.Empty;
    if(g_PhoneIP == String.Empty)
        return IApp.VALUE_FAILURE;
    
    return IApp.VALUE_SUCCESS;
}</Properties>
    </node>
    <node type="Action" id="632628246963716993" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="167.942047" y="323">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiIncomingCall: Ending script.</log>
      </Properties>
    </node>
    <node type="Variable" id="632575523141980055" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInitiated">deviceName</Properties>
    </node>
    <node type="Variable" id="632575523141980056" name="dlxResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">dlxResult</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallActive" startnode="632575523141979605" treenode="632575523141979606" appnode="632575523141979603" handlerfor="632575523141979602">
    <node type="Start" id="632575523141979605" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="96" y="197">
      <linkto id="632628246963716998" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632628246963716998" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="212.4707" y="196">
      <linkto id="632628246963716999" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632628246963717000" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_TimerCreated</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnJTapiCallActive: Checking whether call was previously on hold...</log>
      </Properties>
    </node>
    <node type="Action" id="632628246963716999" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="338.4707" y="196">
      <linkto id="632628246963717001" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">g_TimerId</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnJTapiCallActive: Removing timer with timerId: " + g_TimerId</log>
      </Properties>
    </node>
    <node type="Action" id="632628246963717000" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="212.4707" y="333">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632628246963717001" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="463.4707" y="195">
      <linkto id="632628246963717002" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_TimerCreated</rd>
        <log condition="entry" on="false" level="Verbose" type="literal">OnJTapiCallActive: assigning 'false' to g_CallHeld</log>
      </Properties>
    </node>
    <node type="Action" id="632628246963717002" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="593.4707" y="194">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallInactive" startnode="632575523141979610" treenode="632575523141979611" appnode="632575523141979608" handlerfor="632575523141979607">
    <node type="Start" id="632575523141979610" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="89" y="264">
      <linkto id="632628246963717018" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632628246963717013" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="660" y="266">
      <linkto id="632628246963717014" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_TimerCreated</rd>
      </Properties>
    </node>
    <node type="Action" id="632628246963717014" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="789" y="266">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632628246963717015" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="380" y="405">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632628246963717016" name="AddNonTriggerTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="454" y="250" mx="516" my="266">
      <items count="1">
        <item text="OnTimerFire" treenode="632628246963717012" />
      </items>
      <linkto id="632628246963717013" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632628246963717015" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now.AddSeconds(g_TimerDelay)</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="timerId">g_TimerId</rd>
        <log condition="entry" on="true" level="Verbose" type="literal">OnJTapiCallInactive: Creating timer</log>
        <log condition="default" on="true" level="Warning" type="literal">OnJTapiCallInactive: Timer could not be created.</log>
      </Properties>
    </node>
    <node type="Action" id="632628246963717017" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="380" y="266">
      <linkto id="632628246963717016" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632628246963717015" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_TimerDelay  &gt; 0</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnJTapiCallInactive: Checking that g_TimerDelay &gt; 0</log>
        <log condition="default" on="true" level="Verbose" type="literal">OnJTapiCallInactive: Timer delay set to value &lt;= 0. Timer not set. Check the ReversionDelay configuration setting.</log>
      </Properties>
    </node>
    <node type="Action" id="632628246963717018" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="244" y="265">
      <linkto id="632628246963717017" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632628246963717015" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_TimerCreated</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnJTapiCallInactive: Checking whether timer was already created...</log>
        <log condition="default" on="true" level="Warning" type="literal">OnJTapiCallInactive: Timer already created, did we receive a duplicate JTapiCallInactive message?</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiHangup" startnode="632575523141979615" treenode="632575523141979616" appnode="632575523141979613" handlerfor="632575523141979612">
    <node type="Start" id="632575523141979615" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="188">
      <linkto id="632628246963717027" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632628246963717026" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="319" y="321">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="literal">OnJTapiHangup: Ending script.</log>
      </Properties>
    </node>
    <node type="Action" id="632628246963717027" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="156" y="188">
      <linkto id="632628246963717028" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632628246963717026" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_TimerCreated</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnJTapiHangup: Checking if timer was created.</log>
      </Properties>
    </node>
    <node type="Action" id="632628246963717028" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="318" y="189">
      <linkto id="632628246963717026" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">g_TimerId</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnJTapiHangup: Removing timer with TimerId: " + g_TimerId</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiGotDigits" startnode="632575523141979620" treenode="632575523141979621" appnode="632575523141979618" handlerfor="632575523141979617">
    <node type="Start" id="632575523141979620" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="54" y="157">
      <linkto id="632575523141979632" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575523141979632" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="172" y="158">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnTimerFire" activetab="true" startnode="632628246963717011" treenode="632628246963717012" appnode="632628246963717009" handlerfor="632628246963717008">
    <node type="Start" id="632628246963717011" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="53" y="250">
      <linkto id="632628246963717032" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632628246963717032" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="196" y="250">
      <linkto id="632628246963717034" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632628246963717036" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_TimerCreated</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnTimerFire: Checking for g_TimerCreated</log>
        <log condition="default" on="true" level="Verbose" type="literal">OnTimerFire: g_TimerCreated is false.</log>
      </Properties>
    </node>
    <node type="Comment" id="632628246963717033" text="Handling case in which timer fires and this handler is queued up&#xD;&#xA;to execute RIGHT before a CallActive executes." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="87" y="158" />
    <node type="Action" id="632628246963717034" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="196" y="377">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="literal">OnTimerFire: Exiting function...</log>
      </Properties>
    </node>
    <node type="Action" id="632628246963717036" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="359" y="250">
      <linkto id="632628246963717046" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_TimerCreated</rd>
      </Properties>
    </node>
    <node type="Comment" id="632628246963717042" text="We do not remove the timer here because non-recurring timers are automatically disposed." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="48" y="586" />
    <node type="Action" id="632628246963717044" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="503.098267" y="377.5">
      <linkto id="632628246963717047" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632628246963717054" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"Play:" + g_Filename.Trim()</ap>
        <rd field="ResultData">g_ExecuteMsg</rd>
        <log condition="entry" on="true" level="Verbose" type="literal">OnTimerFire: creating Execute message</log>
        <log condition="default" on="true" level="Warning" type="literal">OnTimerFire: Execute message could not be created</log>
      </Properties>
    </node>
    <node type="Action" id="632628246963717045" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="729.0983" y="249.5">
      <linkto id="632628246963717048" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632628246963717052" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="variable">g_ExecuteMsg</ap>
        <ap name="URL" type="csharp">"http://" + g_PhoneIP + "/CGI/Execute"</ap>
        <ap name="Username" type="variable">g_Username</ap>
        <ap name="Password" type="variable">g_Password</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnTimerFire: Sending Execute message to phone.</log>
        <log condition="default" on="true" level="Warning" type="literal">OnTimerFire: SendExecute action failed</log>
      </Properties>
    </node>
    <node type="Action" id="632628246963717046" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="502.000061" y="250.5">
      <linkto id="632628246963717045" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632628246963717044" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_ExecuteCreated</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnTimerFire: checking whether Execute message has already been created</log>
      </Properties>
    </node>
    <node type="Action" id="632628246963717047" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="603.000061" y="377.5">
      <linkto id="632628246963717049" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_ExecuteCreated</rd>
      </Properties>
    </node>
    <node type="Action" id="632628246963717048" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="860" y="250.5">
      <linkto id="632628246963717051" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632628246963717052" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_TimerPeriodicity &gt; 0</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnTimerFire: checking that ReversionInterval setting &gt; 0</log>
        <log condition="default" on="true" level="Verbose" type="literal">OnTimerFire: ReversionInterval &lt;= 0. Timer not reset.</log>
      </Properties>
    </node>
    <node type="Label" id="632628246963717049" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="604.000061" y="494.5" />
    <node type="Label" id="632628246963717050" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="728.000061" y="168.5">
      <linkto id="632628246963717045" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632628246963717051" name="AddNonTriggerTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="938" y="234.5" mx="1000" my="250">
      <items count="1">
        <item text="OnTimerFire" treenode="632628246963717012" />
      </items>
      <linkto id="632628246963717052" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632628246963717053" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now.AddSeconds(g_TimerPeriodicity)</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="timerId">g_TimerId</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnTimerFire: Resetting timer to have it go off in: " + g_TimerPeriodicity + " seconds"</log>
        <log condition="default" on="true" level="Warning" type="literal">OnTimerFire: failed to create timer.</log>
      </Properties>
    </node>
    <node type="Action" id="632628246963717052" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="860.4707" y="380.5">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="literal">OnTimerFire: Exiting function...</log>
      </Properties>
    </node>
    <node type="Action" id="632628246963717053" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1128.4707" y="249.5">
      <linkto id="632628246963717067" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_TimerCreated</rd>
      </Properties>
    </node>
    <node type="Action" id="632628246963717054" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="504.000061" y="479.5">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="literal">OnTimerFire: Exiting function...</log>
      </Properties>
    </node>
    <node type="Action" id="632628246963717067" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1262.99023" y="250">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>