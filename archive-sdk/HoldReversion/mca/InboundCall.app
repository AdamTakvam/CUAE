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
    </outline>
    <variables>
      <treenode text="g_CallHeld" id="632575523141979275" vid="632575510106029569">
        <Properties type="Bool" defaultInitWith="false">g_CallHeld</Properties>
      </treenode>
      <treenode text="g_TimerPeriod" id="632575523141979277" vid="632575510106029571">
        <Properties type="Int" defaultInitWith="0" initWith="ReversionTime">g_TimerPeriod</Properties>
      </treenode>
      <treenode text="g_TimerId" id="632575523141979279" vid="632575510106029576">
        <Properties type="String">g_TimerId</Properties>
      </treenode>
      <treenode text="g_PhoneIP" id="632575523141979281" vid="632575523141979023">
        <Properties type="String">g_PhoneIP</Properties>
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
      </Properties>
    </node>
    <node type="Action" id="632575523141979028" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="286" y="159">
      <linkto id="632575510106029573" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575523141979029" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(DataTable dlxResult, ref string g_PhoneIP)
{
    g_PhoneIP = String.Empty;

    if(dlxResult == null || dlxResult.Rows == null || dlxResult.Rows.Count != 1)
        return IApp.VALUE_FAILURE;

    g_PhoneIP = dlxResult.Rows[0][ICiscoDeviceList.FIELD_IP] as string;

    if(g_PhoneIP == null)
        g_PhoneIP = String.Empty;
    if(g_PhoneIP == String.Empty)
        return IApp.VALUE_FAILURE;
    
    return IApp.VALUE_SUCCESS;
}</Properties>
    </node>
    <node type="Action" id="632575523141979029" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="211" y="289">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="csharp">"Could not obtain IP address for device: " + deviceName</log>
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
    <node type="Action" id="632575510106029574" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="149" y="160">
      <linkto id="632575510106029575" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632575510106029578" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_CallHeld</ap>
      </Properties>
    </node>
    <node type="Action" id="632575510106029575" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="270" y="160">
      <linkto id="632575510106029579" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">g_TimerId</ap>
      </Properties>
    </node>
    <node type="Action" id="632575510106029578" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="148" y="298">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575510106029579" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="394" y="160">
      <linkto id="632575510106029580" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_CallHeld</rd>
      </Properties>
    </node>
    <node type="Action" id="632575510106029580" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="514" y="160">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallInactive" activetab="true" startnode="632575510106029555" treenode="632575510106029556" appnode="632575510106029553" handlerfor="632575510106029552">
    <node type="Start" id="632575510106029555" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="49" y="156">
      <linkto id="632575523141979306" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575510106029581" name="AddTriggerTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="287" y="156">
      <linkto id="632575510106029631" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575510106029632" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerDateTime" type="csharp">DateTime.Now.Add(new TimeSpan(0,0,g_TimerPeriod))</ap>
        <ap name="timerRecurrenceInterval" type="csharp">new TimeSpan(0,0,g_TimerPeriod)</ap>
        <ap name="timerUserData" type="variable">g_PhoneIP</ap>
        <rd field="timerId">g_TimerId</rd>
      </Properties>
    </node>
    <node type="Action" id="632575510106029631" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="413" y="156">
      <linkto id="632575510106029634" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_CallHeld</rd>
      </Properties>
    </node>
    <node type="Action" id="632575510106029632" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="286" y="283">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Warning" type="literal">Unable to set hold reverion timer</log>
      </Properties>
    </node>
    <node type="Action" id="632575510106029634" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="540" y="156">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575523141979233" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="155" y="281">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575523141979306" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="156" y="156">
      <linkto id="632575510106029581" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632575523141979233" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_TimerPeriod == 0 ? "true" : "false";</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiHangup" startnode="632575510106029560" treenode="632575510106029561" appnode="632575510106029558" handlerfor="632575510106029557">
    <node type="Start" id="632575510106029560" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="134">
      <linkto id="632575510106029568" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575510106029568" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="278" y="134">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiGotDigits" startnode="632575510106029565" treenode="632575510106029566" appnode="632575510106029563" handlerfor="632575510106029562">
    <node type="Start" id="632575510106029565" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="47" y="159">
      <linkto id="632575510106029567" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575510106029567" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="304" y="156">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>