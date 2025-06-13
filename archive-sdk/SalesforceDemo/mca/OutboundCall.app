<Application name="OutboundCall" trigger="Metreos.Providers.JTapi.JTapiCallInitiated" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="OutboundCall">
    <outline>
      <treenode type="evh" id="632983074993061880" level="1" text="Metreos.Providers.JTapi.JTapiCallInitiated (trigger): OnJTapiCallInitiated">
        <node type="function" name="OnJTapiCallInitiated" id="632983074993061877" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallInitiated" id="632983074993061876" path="Metreos.Providers.JTapi.JTapiCallInitiated" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632983074993061890" level="2" text="Metreos.Providers.JTapi.JTapiCallActive: OnJTapiCallActive">
        <node type="function" name="OnJTapiCallActive" id="632983074993061887" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallActive" id="632983074993061886" path="Metreos.Providers.JTapi.JTapiCallActive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632983074993061895" level="2" text="Metreos.Providers.JTapi.JTapiCallInactive: OnJTapiCallInactive">
        <node type="function" name="OnJTapiCallInactive" id="632983074993061892" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallInactive" id="632983074993061891" path="Metreos.Providers.JTapi.JTapiCallInactive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632983074993061906" level="2" text="Metreos.Providers.JTapi.JTapiHangup: OnJTapiHangup">
        <node type="function" name="OnJTapiHangup" id="632983074993061903" path="Metreos.StockTools" />
        <node type="event" name="JTapiHangup" id="632983074993061902" path="Metreos.Providers.JTapi.JTapiHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632989423340680752" level="2" text="Metreos.Providers.SalesforceDemo.HangupRequest: OnHangupRequest">
        <node type="function" name="OnHangupRequest" id="632989423340680749" path="Metreos.StockTools" />
        <node type="event" name="HangupRequest" id="632989423340680748" path="Metreos.Providers.SalesforceDemo.HangupRequest" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="633071510444337196" vid="632983417216714903">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="hashtable" id="633071510444337198" vid="632985069719295903">
        <Properties type="Hashtable">hashtable</Properties>
      </treenode>
      <treenode text="g_accountCode" id="633071510444337200" vid="632985069719295918">
        <Properties type="String">g_accountCode</Properties>
      </treenode>
      <treenode text="g_from" id="633071510444337202" vid="633004250508108235">
        <Properties type="String">g_from</Properties>
      </treenode>
      <treenode text="g_recId" id="633071510444337204" vid="633004250508108870">
        <Properties type="Int" defaultInitWith="-1">g_recId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnJTapiCallInitiated" startnode="632983074993061879" treenode="632983074993061880" appnode="632983074993061877" handlerfor="632983074993061876">
    <node type="Start" id="632983074993061879" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="77" y="278">
      <linkto id="633004250508108237" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632983410695910286" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="426" y="279">
      <linkto id="632983417216714905" type="Labeled" style="Vector" label="true" />
      <linkto id="633071510444336852" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">isSubscribed</ap>
      </Properties>
    </node>
    <node type="Action" id="632983410695910287" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="650" y="163">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632983417216714905" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="540" y="163">
      <linkto id="632983410695910287" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">callId</ap>
        <rd field="ResultData">g_callId</rd>
      </Properties>
    </node>
    <node type="Action" id="632989461822629430" name="NotifyInitiate" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="272" y="280">
      <linkto id="632983410695910286" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">to</ap>
        <ap name="DeviceName" type="variable">deviceName</ap>
        <ap name="From" type="variable">from</ap>
        <ap name="CallId" type="variable">callId</ap>
        <rd field="IsSubscriber">isSubscribed</rd>
        <log condition="entry" on="true" level="Info" type="csharp">String.Format("Call Initiated! to: {0}, from: {1}, deviceName: {2}, callId: {3}", to, from, deviceName, callId)</log>
      </Properties>
    </node>
    <node type="Action" id="633004250508108237" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="182.048828" y="278">
      <linkto id="632989461822629430" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">from</ap>
        <rd field="ResultData">g_from</rd>
      </Properties>
    </node>
    <node type="Action" id="633071510444336852" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="536" y="408">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632983074993061881" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInitiated">to</Properties>
    </node>
    <node type="Variable" id="632983074993061882" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInitiated">from</Properties>
    </node>
    <node type="Variable" id="632983074993061883" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInitiated">deviceName</Properties>
    </node>
    <node type="Variable" id="632983074993061885" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInitiated">callId</Properties>
    </node>
    <node type="Variable" id="632983410695910285" name="isSubscribed" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">isSubscribed</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallActive" startnode="632983074993061889" treenode="632983074993061890" appnode="632983074993061887" handlerfor="632983074993061886">
    <node type="Start" id="632983074993061889" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="96" y="304">
      <linkto id="632983410695910296" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632983410695910290" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="728" y="304">
      <linkto id="632983410695910291" type="Labeled" style="Vector" label="true" />
      <linkto id="633071510444336445" type="Labeled" style="Vector" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">isSubscribed</ap>
      </Properties>
    </node>
    <node type="Action" id="632983410695910291" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="872" y="176">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632983410695910296" name="NotifyCallActive" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="224" y="304">
      <linkto id="633071510444336441" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">to</ap>
        <ap name="DeviceName" type="variable">deviceName</ap>
        <ap name="CallId" type="variable">callId</ap>
        <rd field="IsSubscriber">isSubscribed</rd>
      </Properties>
    </node>
    <node type="Action" id="633004250508108872" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="504" y="304">
      <linkto id="633004250508108873" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("INSERT INTO activecalls (devicename, to_number, from_number, active, direction, state) VALUES ('{0}', '{1}', '{2}', NOW(), {3}, {4})", deviceName, to, g_from, 1, 1)</ap>
        <ap name="Name" type="literal">activecalls</ap>
      </Properties>
    </node>
    <node type="Action" id="633004250508108873" name="ExecuteScalar" class="MaxActionNode" group="" path="Metreos.Native.Database" x="640" y="304">
      <linkto id="632983410695910290" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="literal">SELECT LAST_INSERT_ID()</ap>
        <ap name="Name" type="literal">activecalls</ap>
        <rd field="Scalar">g_recId</rd>
      </Properties>
    </node>
    <node type="Action" id="633071510444336441" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="368" y="304">
      <linkto id="633071510444336442" type="Labeled" style="Vector" label="false" />
      <linkto id="633004250508108872" type="Labeled" style="Vector" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_recId == -1</ap>
      </Properties>
    </node>
    <node type="Action" id="633071510444336442" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="368" y="520">
      <linkto id="632983410695910290" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("UPDATE activecalls SET state = 1 WHERE id = {0}", g_recId)</ap>
        <ap name="Name" type="literal">activecalls</ap>
      </Properties>
    </node>
    <node type="Action" id="633071510444336445" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="872" y="424">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632983074993061896" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.Providers.JTapi.JTapiCallActive">to</Properties>
    </node>
    <node type="Variable" id="632983074993061897" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.JTapi.JTapiCallActive">deviceName</Properties>
    </node>
    <node type="Variable" id="632983074993061898" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.Providers.JTapi.JTapiCallActive">callId</Properties>
    </node>
    <node type="Variable" id="632983410695910289" name="isSubscribed" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">isSubscribed</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallInactive" startnode="632983074993061894" treenode="632983074993061895" appnode="632983074993061892" handlerfor="632983074993061891">
    <node type="Start" id="632983074993061894" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="110" y="303">
      <linkto id="632983410695910303" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632983410695910297" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="568" y="304">
      <linkto id="632983410695910298" type="Labeled" style="Vector" label="true" />
      <linkto id="633071510444336851" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">isSubscribed</ap>
      </Properties>
    </node>
    <node type="Action" id="632983410695910298" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="712" y="184">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632983410695910303" name="NotifyCallInactive" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="262" y="303">
      <linkto id="633071510444336446" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DeviceName" type="variable">deviceName</ap>
        <ap name="InUse" type="variable">inUse</ap>
        <rd field="IsSubscriber">isSubscribed</rd>
      </Properties>
    </node>
    <node type="Action" id="633071510444336446" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="424" y="304">
      <linkto id="632983410695910297" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("UPDATE activecalls SET state = 0 WHERE id = {0}", g_recId)</ap>
        <ap name="Name" type="literal">activecalls</ap>
      </Properties>
    </node>
    <node type="Action" id="633071510444336851" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="712" y="424">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632983074993061899" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInactive">callId</Properties>
    </node>
    <node type="Variable" id="632983074993061900" name="inUse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" initWith="InUse" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInactive">inUse</Properties>
    </node>
    <node type="Variable" id="632983074993061901" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInactive">deviceName</Properties>
    </node>
    <node type="Variable" id="632983410695910304" name="isSubscribed" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">isSubscribed</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiHangup" activetab="true" startnode="632983074993061905" treenode="632983074993061906" appnode="632983074993061903" handlerfor="632983074993061902">
    <node type="Start" id="632983074993061905" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="168" y="288">
      <linkto id="632983410695910311" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632983410695910307" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="600" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632983410695910311" name="NotifyHangup" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="328" y="288">
      <linkto id="633004250508108648" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Cause" type="variable">cause</ap>
        <ap name="DeviceName" type="variable">deviceName</ap>
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="633004250508108648" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="464" y="288">
      <linkto id="632983410695910307" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("DELETE FROM activecalls WHERE id = {0}", g_recId)</ap>
        <ap name="Name" type="literal">activecalls</ap>
      </Properties>
    </node>
    <node type="Variable" id="632983074993061907" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.Providers.JTapi.JTapiHangup">callId</Properties>
    </node>
    <node type="Variable" id="632983074993061908" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.JTapi.JTapiHangup">deviceName</Properties>
    </node>
    <node type="Variable" id="632983074993061909" name="cause" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Cause" refType="reference" name="Metreos.Providers.JTapi.JTapiHangup">cause</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangupRequest" startnode="632989423340680751" treenode="632989423340680752" appnode="632989423340680749" handlerfor="632989423340680748">
    <node type="Start" id="632989423340680751" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="97" y="476">
      <linkto id="632989423340680753" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632989423340680753" name="JTapiHangup" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="304" y="480">
      <linkto id="633004250508108646" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632989423340680755" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="656" y="480">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633004250508108646" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="504" y="480">
      <linkto id="632989423340680755" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("DELETE FROM activecalls WHERE id = {0}", g_recId)</ap>
        <ap name="Name" type="literal">activecalls</ap>
      </Properties>
    </node>
    <node type="Variable" id="632989423340680754" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference">callId</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>