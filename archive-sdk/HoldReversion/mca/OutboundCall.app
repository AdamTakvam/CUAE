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
    </outline>
    <variables>
      <treenode text="g_CallHeld" id="632575523141980026" vid="632575523141979450">
        <Properties type="Bool" defaultInitWith="false">g_CallHeld</Properties>
      </treenode>
      <treenode text="g_TimerId" id="632575523141980028" vid="632575523141979452">
        <Properties type="String">g_TimerId</Properties>
      </treenode>
      <treenode text="g_PhoneIP" id="632575523141980030" vid="632575523141979454">
        <Properties type="String">g_PhoneIP</Properties>
      </treenode>
      <treenode text="g_TimerPeriod" id="632575523141980032" vid="632575523141979527">
        <Properties type="Int" defaultInitWith="0" initWith="ReversionTime">g_TimerPeriod</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnJTapiCallInitiated" activetab="true" startnode="632575510106029537" treenode="632575510106029538" appnode="632575510106029535" handlerfor="632575510106029534">
    <node type="Start" id="632575510106029537" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="144">
      <linkto id="632575523141979378" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575523141979377" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="443.1543" y="144">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575523141979378" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="134.1543" y="144">
      <linkto id="632575523141979379" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575523141979380" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">deviceName</ap>
        <rd field="ResultData">dlxResult</rd>
      </Properties>
    </node>
    <node type="Action" id="632575523141979379" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="265.1543" y="144">
      <linkto id="632575523141979377" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575523141979380" type="Labeled" style="Bezier" ortho="true" label="default" />
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
    <node type="Action" id="632575523141979380" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="190.1543" y="274">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="csharp">"Could not obtain IP address for device: " + deviceName</log>
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
    <node type="Start" id="632575523141979605" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="163">
      <linkto id="632575523141979622" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575523141979622" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="130.4707" y="162">
      <linkto id="632575523141979623" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632575523141979624" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_CallHeld</ap>
      </Properties>
    </node>
    <node type="Action" id="632575523141979623" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="251.4707" y="162">
      <linkto id="632575523141979625" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">g_TimerId</ap>
      </Properties>
    </node>
    <node type="Action" id="632575523141979624" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="129.4707" y="300">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575523141979625" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="375.4707" y="162">
      <linkto id="632575523141979626" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_CallHeld</rd>
      </Properties>
    </node>
    <node type="Action" id="632575523141979626" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="495.4707" y="162">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallInactive" startnode="632575523141979610" treenode="632575523141979611" appnode="632575523141979608" handlerfor="632575523141979607">
    <node type="Start" id="632575523141979610" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="38" y="158">
      <linkto id="632575523141979836" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575523141979831" name="AddTriggerTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="261.4707" y="157">
      <linkto id="632575523141979832" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575523141979833" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerDateTime" type="csharp">DateTime.Now.Add(new TimeSpan(0,0,g_TimerPeriod))</ap>
        <ap name="timerRecurrenceInterval" type="csharp">new TimeSpan(0,0,g_TimerPeriod)</ap>
        <ap name="timerUserData" type="variable">g_PhoneIP</ap>
        <rd field="timerId">g_TimerId</rd>
      </Properties>
    </node>
    <node type="Action" id="632575523141979832" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="387.4707" y="157">
      <linkto id="632575523141979834" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_CallHeld</rd>
      </Properties>
    </node>
    <node type="Action" id="632575523141979833" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="260.4707" y="284">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Warning" type="literal">Unable to set hold reverion timer</log>
      </Properties>
    </node>
    <node type="Action" id="632575523141979834" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="514.4707" y="157">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575523141979835" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="129.4707" y="282">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575523141979836" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="130.4707" y="157">
      <linkto id="632575523141979831" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632575523141979835" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_TimerPeriod == 0 ? "true" : "false";</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiHangup" startnode="632575523141979615" treenode="632575523141979616" appnode="632575523141979613" handlerfor="632575523141979612">
    <node type="Start" id="632575523141979615" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="175">
      <linkto id="632575523141979948" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575523141979948" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="163" y="175">
      <Properties final="true" type="appControl" log="On">
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
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>