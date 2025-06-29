<Application name="InboundCall" trigger="Metreos.Providers.JTapi.JTapiIncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="InboundCall">
    <outline>
      <treenode type="evh" id="632575510106029531" level="1" text="Metreos.Providers.JTapi.JTapiIncomingCall (trigger): OnJTapiIncomingCall">
        <node type="function" name="OnJTapiIncomingCall" id="632575510106029528" path="Metreos.StockTools" />
        <node type="event" name="JTapiIncomingCall" id="632575510106029527" path="Metreos.Providers.JTapi.JTapiIncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575510106029551" level="2" text="Metreos.Providers.JTapi.JTapiCallActive: OnJTapiCallActive">
        <node type="function" name="OnJTapiCallActive" id="632575510106029548" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallActive" id="632575510106029547" path="Metreos.Providers.JTapi.JTapiCallActive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575510106029556" level="2" text="Metreos.Providers.JTapi.JTapiCallInactive: OnJTapiCallInactive">
        <node type="function" name="OnJTapiCallInactive" id="632575510106029553" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallInactive" id="632575510106029552" path="Metreos.Providers.JTapi.JTapiCallInactive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575510106029561" level="2" text="Metreos.Providers.JTapi.JTapiHangup: OnJTapiHangup">
        <node type="function" name="OnJTapiHangup" id="632575510106029558" path="Metreos.StockTools" />
        <node type="event" name="JTapiHangup" id="632575510106029557" path="Metreos.Providers.JTapi.JTapiHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575510106029566" level="2" text="Metreos.Providers.JTapi.JTapiGotDigits: OnJTapiGotDigits">
        <node type="function" name="OnJTapiGotDigits" id="632575510106029563" path="Metreos.StockTools" />
        <node type="event" name="JTapiGotDigits" id="632575510106029562" path="Metreos.Providers.JTapi.JTapiGotDigits" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632627251825472642" level="2" text="Metreos.Providers.TimerFacility.TimerFire: OnTimerFire">
        <node type="function" name="OnTimerFire" id="632627251825472639" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632627251825472638" path="Metreos.Providers.TimerFacility.TimerFire" />
        <references>
          <ref id="632629260761032353" actid="632627251825472643" />
          <ref id="632629260761032372" actid="632627251825472824" />
        </references>
        <Properties type="hybrid">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_TimerDelay" id="632629260761032316" vid="632575510106029571">
        <Properties type="Int" defaultInitWith="0" initWith="ReversionDelay">g_TimerDelay</Properties>
      </treenode>
      <treenode text="g_TimerPeriodicity" id="632629260761032318" vid="632627251825472645">
        <Properties type="Int" defaultInitWith="0" initWith="ReversionInterval">g_TimerPeriodicity</Properties>
      </treenode>
      <treenode text="g_TimerId" id="632629260761032320" vid="632575510106029576">
        <Properties type="String">g_TimerId</Properties>
      </treenode>
      <treenode text="g_TimerCreated" id="632629260761032322" vid="632627251825472713">
        <Properties type="Bool" defaultInitWith="false">g_TimerCreated</Properties>
      </treenode>
      <treenode text="g_PhoneIP" id="632629260761032324" vid="632575523141979023">
        <Properties type="String">g_PhoneIP</Properties>
      </treenode>
      <treenode text="g_Username" id="632629260761032326" vid="632627251825472812">
        <Properties type="String" initWith="Username">g_Username</Properties>
      </treenode>
      <treenode text="g_Password" id="632629260761032328" vid="632627251825472808">
        <Properties type="String" initWith="Password">g_Password</Properties>
      </treenode>
      <treenode text="g_Filename" id="632629260761032330" vid="632627251825472810">
        <Properties type="String" initWith="Filename">g_Filename</Properties>
      </treenode>
      <treenode text="g_ExecuteMsg" id="632629260761032332" vid="632627251825472814">
        <Properties type="Metreos.Types.CiscoIpPhone.Execute">g_ExecuteMsg</Properties>
      </treenode>
      <treenode text="g_ExecuteCreated" id="632629260761032334" vid="632627251825472816">
        <Properties type="Bool" defaultInitWith="false">g_ExecuteCreated</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnJTapiIncomingCall" startnode="632575510106029530" treenode="632575510106029531" appnode="632575510106029528" handlerfor="632575510106029527">
    <node type="Start" id="632575510106029530" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="38" y="159">
      <linkto id="632575523141979026" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575510106029573" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="464" y="159">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575523141979026" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="155" y="159">
      <linkto id="632575523141979028" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575523141979029" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">deviceName</ap>
        <rd field="ResultData">dlxResult</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnJTapiIncomingCall: Querying DLX for device: " + deviceName</log>
        <log condition="default" on="true" level="Warning" type="csharp">"OnJTapiIncomingCall: DeviceListX query FAILED for device: " + deviceName</log>
      </Properties>
    </node>
    <node type="Action" id="632575523141979028" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="286" y="159">
      <linkto id="632575510106029573" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575523141979029" type="Labeled" style="Bezier" ortho="true" label="default" />
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
    <node type="Action" id="632575523141979029" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="157" y="310">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiIncomingCall: Ending script.</log>
      </Properties>
    </node>
    <node type="Variable" id="632575523141979025" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.JTapi.JTapiIncomingCall">deviceName</Properties>
    </node>
    <node type="Variable" id="632575523141979027" name="dlxResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">dlxResult</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallActive" startnode="632575510106029550" treenode="632575510106029551" appnode="632575510106029548" handlerfor="632575510106029547">
    <node type="Start" id="632575510106029550" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="160">
      <linkto id="632575510106029574" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575510106029574" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="148" y="161">
      <linkto id="632575510106029575" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632575510106029578" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_TimerCreated</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnJTapiCallActive: Checking whether call was previously on hold...</log>
      </Properties>
    </node>
    <node type="Action" id="632575510106029575" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="274" y="161">
      <linkto id="632575510106029579" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">g_TimerId</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnJTapiCallActive: Removing timer with timerId: " + g_TimerId</log>
      </Properties>
    </node>
    <node type="Action" id="632575510106029578" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="148" y="298">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575510106029579" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="399" y="160">
      <linkto id="632575510106029580" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_TimerCreated</rd>
        <log condition="entry" on="false" level="Verbose" type="literal">OnJTapiCallActive: assigning 'false' to g_CallHeld</log>
      </Properties>
    </node>
    <node type="Action" id="632575510106029580" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="529" y="159">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallInactive" startnode="632575510106029555" treenode="632575510106029556" appnode="632575510106029553" handlerfor="632575510106029552">
    <node type="Start" id="632575510106029555" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="155">
      <linkto id="632627251825472712" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575510106029631" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="572" y="156">
      <linkto id="632575510106029634" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_TimerCreated</rd>
      </Properties>
    </node>
    <node type="Action" id="632575510106029634" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="701" y="156">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575523141979233" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="292" y="295">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632627251825472643" name="AddNonTriggerTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="366" y="140" mx="428" my="156">
      <items count="1">
        <item text="OnTimerFire" treenode="632627251825472642" />
      </items>
      <linkto id="632575510106029631" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575523141979233" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now.AddSeconds(g_TimerDelay)</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="timerId">g_TimerId</rd>
        <log condition="entry" on="true" level="Verbose" type="literal">OnJTapiCallInactive: Creating timer</log>
        <log condition="default" on="true" level="Warning" type="literal">OnJTapiCallInactive: Timer could not be created.</log>
      </Properties>
    </node>
    <node type="Action" id="632627251825472711" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="292" y="156">
      <linkto id="632627251825472643" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632575523141979233" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_TimerDelay  &gt; 0</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnJTapiCallInactive: Checking that g_TimerDelay &gt; 0</log>
        <log condition="default" on="true" level="Verbose" type="literal">OnJTapiCallInactive: Timer delay set to value &lt;= 0. Timer not set. Check the ReversionDelay configuration setting.</log>
      </Properties>
    </node>
    <node type="Action" id="632627251825472712" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="156" y="155">
      <linkto id="632627251825472711" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632575523141979233" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_TimerCreated</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnJTapiCallInactive: Checking whether timer was already created...</log>
        <log condition="default" on="true" level="Warning" type="literal">OnJTapiCallInactive: Timer already created, did we receive a duplicate JTapiCallInactive message?</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiHangup" startnode="632575510106029560" treenode="632575510106029561" appnode="632575510106029558" handlerfor="632575510106029557">
    <node type="Start" id="632575510106029560" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="139">
      <linkto id="632628162532249907" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575510106029568" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="305" y="272">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="literal">OnJTapiHangup: Ending script.</log>
      </Properties>
    </node>
    <node type="Action" id="632628162532249907" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="142" y="139">
      <linkto id="632628162532249909" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632575510106029568" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_TimerCreated</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnJTapiHangup: Checking if timer was created.</log>
      </Properties>
    </node>
    <node type="Action" id="632628162532249909" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="304" y="139">
      <linkto id="632575510106029568" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">g_TimerId</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnJTapiHangup: Removing timer with TimerId: " + g_TimerId</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiGotDigits" startnode="632575510106029565" treenode="632575510106029566" appnode="632575510106029563" handlerfor="632575510106029562">
    <node type="Start" id="632575510106029565" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="47" y="159">
      <linkto id="632575510106029567" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575510106029567" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="200" y="159">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnTimerFire" activetab="true" startnode="632627251825472641" treenode="632627251825472642" appnode="632627251825472639" handlerfor="632627251825472638">
    <node type="Start" id="632627251825472641" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="168">
      <linkto id="632627251825472828" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632627251825472799" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="411.098267" y="300">
      <linkto id="632627251825472819" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632628162532249905" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"Play:" + g_Filename.Trim()</ap>
        <rd field="ResultData">g_ExecuteMsg</rd>
        <log condition="entry" on="true" level="Verbose" type="literal">OnTimerFire: creating Execute message</log>
        <log condition="default" on="true" level="Warning" type="literal">OnTimerFire: Execute message could not be created</log>
      </Properties>
    </node>
    <node type="Action" id="632627251825472800" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="670.098267" y="168">
      <linkto id="632627251825472820" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632628162532249901" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="variable">g_ExecuteMsg</ap>
        <ap name="URL" type="csharp">"http://" + g_PhoneIP + "/CGI/Execute"</ap>
        <ap name="Username" type="variable">g_Username</ap>
        <ap name="Password" type="variable">g_Password</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnTimerFire: Sending Execute message to phone.</log>
        <log condition="default" on="true" level="Warning" type="literal">OnTimerFire: SendExecute action failed</log>
      </Properties>
    </node>
    <node type="Action" id="632627251825472818" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="411" y="169">
      <linkto id="632627251825472800" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632627251825472799" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_ExecuteCreated</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnTimerFire: checking whether Execute message has already been created</log>
      </Properties>
    </node>
    <node type="Action" id="632627251825472819" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="511" y="300">
      <linkto id="632627251825472821" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_ExecuteCreated</rd>
      </Properties>
    </node>
    <node type="Action" id="632627251825472820" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="801" y="169">
      <linkto id="632627251825472824" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632628162532249901" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_TimerPeriodicity &gt; 0</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnTimerFire: checking that ReversionInterval &gt; 0</log>
        <log condition="default" on="true" level="Verbose" type="literal">OnTimerFire: ReversionInterval &lt;= 0. Timer not reset.</log>
      </Properties>
    </node>
    <node type="Label" id="632627251825472821" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="512" y="417" />
    <node type="Label" id="632627251825472822" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="669" y="87">
      <linkto id="632627251825472800" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632627251825472823" text="We do not remove the timer here because non-recurring timers are automatically disposed." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="41" y="497" />
    <node type="Action" id="632627251825472824" name="AddNonTriggerTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="879" y="153" mx="941" my="169">
      <items count="1">
        <item text="OnTimerFire" treenode="632627251825472642" />
      </items>
      <linkto id="632628162532249901" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632628162532249904" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now.AddSeconds(g_TimerPeriodicity)</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="timerId">g_TimerId</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnTimerFire: Resetting timer to have it go off in: " + g_TimerPeriodicity + " seconds"</log>
        <log condition="default" on="true" level="Warning" type="literal">OnTimerFire: failed to create timer.</log>
      </Properties>
    </node>
    <node type="Action" id="632627251825472826" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1205" y="168">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632627251825472828" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="140" y="169">
      <linkto id="632627251825472830" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632628162532249903" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_TimerCreated</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnTimerFire: Checking for g_TimerCreated</log>
        <log condition="default" on="true" level="Verbose" type="literal">OnTimerFire: g_TimerCreated is false.</log>
      </Properties>
    </node>
    <node type="Comment" id="632627251825472829" text="Handling case in which timer fires and this handler is queued up&#xD;&#xA;to execute RIGHT before a CallActive executes." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="86" />
    <node type="Action" id="632627251825472830" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="141" y="299">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="literal">OnTimerFire: Exiting function...</log>
      </Properties>
    </node>
    <node type="Action" id="632628162532249901" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="801.4707" y="299">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="literal">OnTimerFire: Exiting function...</log>
      </Properties>
    </node>
    <node type="Action" id="632628162532249903" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="268" y="169">
      <linkto id="632627251825472818" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_TimerCreated</rd>
      </Properties>
    </node>
    <node type="Action" id="632628162532249904" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1080.4707" y="169">
      <linkto id="632627251825472826" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_TimerCreated</rd>
      </Properties>
    </node>
    <node type="Action" id="632628162532249905" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="412" y="402">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Verbose" type="literal">OnTimerFire: Exiting function...</log>
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>