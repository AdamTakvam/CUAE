<Application name="script1" trigger="Metreos.Providers.SalesforceDemo.MakeCallRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632983417216714834" level="1" text="Metreos.Providers.SalesforceDemo.MakeCallRequest (trigger): OnMakeCallRequest">
        <node type="function" name="OnMakeCallRequest" id="632983417216714831" path="Metreos.StockTools" />
        <node type="event" name="MakeCallRequest" id="632983417216714830" path="Metreos.Providers.SalesforceDemo.MakeCallRequest" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632983417216714847" level="2" text="Metreos.Providers.SalesforceDemo.HangupRequest: OnHangupRequest">
        <node type="function" name="OnHangupRequest" id="632983417216714844" path="Metreos.StockTools" />
        <node type="event" name="HangupRequest" id="632983417216714843" path="Metreos.Providers.SalesforceDemo.HangupRequest" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632989461822628841" level="2" text="Metreos.Providers.JTapi.JTapiCallActive: OnJTapiCallActive">
        <node type="function" name="OnJTapiCallActive" id="632989461822628838" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallActive" id="632989461822628837" path="Metreos.Providers.JTapi.JTapiCallActive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632989461822628846" level="2" text="Metreos.Providers.JTapi.JTapiCallInactive: OnJTapiCallInactive">
        <node type="function" name="OnJTapiCallInactive" id="632989461822628843" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallInactive" id="632989461822628842" path="Metreos.Providers.JTapi.JTapiCallInactive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632989461822628851" level="2" text="Metreos.Providers.JTapi.JTapiHangup: OnJTapiHangup">
        <node type="function" name="OnJTapiHangup" id="632989461822628848" path="Metreos.StockTools" />
        <node type="event" name="JTapiHangup" id="632989461822628847" path="Metreos.Providers.JTapi.JTapiHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="633071510444337275" vid="632983417216714839">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_from" id="633071510444337277" vid="633004250508108301">
        <Properties type="String">g_from</Properties>
      </treenode>
      <treenode text="g_recId" id="633071510444337279" vid="633004250508108947">
        <Properties type="Int" defaultInitWith="-1">g_recId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnMakeCallRequest" startnode="632983417216714833" treenode="632983417216714834" appnode="632983417216714831" handlerfor="632983417216714830">
    <node type="Start" id="632983417216714833" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="60" y="283">
      <linkto id="633004250508108303" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632983417216714838" name="JTapiMakeCall" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="227" y="282">
      <linkto id="632983417216714841" type="Labeled" style="Vector" label="default" />
      <linkto id="632989461822628834" type="Labeled" style="Vector" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="From" type="variable">from</ap>
        <ap name="To" type="variable">to</ap>
        <ap name="DeviceName" type="variable">deviceName</ap>
        <rd field="CallId">g_callId</rd>
      </Properties>
    </node>
    <node type="Action" id="632983417216714841" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="520" y="464">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632983417216714842" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="672" y="160">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632989461822628834" name="NotifyInitiate" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="384" y="160">
      <linkto id="632989461822628836" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_callId</ap>
        <ap name="DeviceName" type="variable">deviceName</ap>
        <ap name="From" type="variable">from</ap>
        <ap name="CallId" type="variable">g_callId</ap>
        <rd field="IsSubscriber">isSub</rd>
      </Properties>
    </node>
    <node type="Action" id="632989461822628836" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="520" y="160">
      <linkto id="632983417216714841" type="Labeled" style="Vector" label="false" />
      <linkto id="632983417216714842" type="Labeled" style="Vector" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">isSub</ap>
      </Properties>
    </node>
    <node type="Action" id="633004250508108303" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="138.048828" y="282">
      <linkto id="632983417216714838" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">from</ap>
        <rd field="ResultData">g_from</rd>
      </Properties>
    </node>
    <node type="Variable" id="632983417216714835" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.Providers.SalesforceDemo.MakeCallRequest">to</Properties>
    </node>
    <node type="Variable" id="632983417216714836" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" defaultInitWith="1000" refType="reference" name="Metreos.Providers.SalesforceDemo.MakeCallRequest">from</Properties>
    </node>
    <node type="Variable" id="632983417216714837" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.SalesforceDemo.MakeCallRequest">deviceName</Properties>
    </node>
    <node type="Variable" id="632989461822628835" name="isSub" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">isSub</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangupRequest" startnode="632983417216714846" treenode="632983417216714847" appnode="632983417216714844" handlerfor="632983417216714843">
    <node type="Start" id="632983417216714846" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="80" y="296">
      <linkto id="632983417216714848" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632983417216714848" name="JTapiHangup" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="232" y="296">
      <linkto id="633004250508108719" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632983417216714849" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="568" y="296">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633004250508108719" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="424" y="296">
      <linkto id="632983417216714849" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("DELETE FROM activecalls WHERE id = {0}", g_recId)</ap>
        <ap name="Name" type="literal">activecalls</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallActive" startnode="632989461822628840" treenode="632989461822628841" appnode="632989461822628838" handlerfor="632989461822628837">
    <node type="Start" id="632989461822628840" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="120" y="440">
      <linkto id="632989461822628963" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632989461822628960" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="816" y="440">
      <linkto id="632989461822628961" type="Labeled" style="Vector" label="true" />
      <linkto id="633071510444336687" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">isSubscribed</ap>
      </Properties>
    </node>
    <node type="Action" id="632989461822628961" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="960" y="320">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632989461822628963" name="NotifyCallActive" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="322.835938" y="440">
      <linkto id="633071510444336926" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">to</ap>
        <ap name="DeviceName" type="variable">deviceName</ap>
        <ap name="CallId" type="variable">callId</ap>
        <rd field="IsSubscriber">isSubscribed</rd>
      </Properties>
    </node>
    <node type="Action" id="633004250508108943" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="560" y="440">
      <linkto id="633004250508108944" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("INSERT INTO activecalls (devicename, to_number, from_number, active) VALUES ('{0}', '{1}', '{2}', NOW())", deviceName, to, g_from)</ap>
        <ap name="Name" type="literal">activecalls</ap>
      </Properties>
    </node>
    <node type="Action" id="633004250508108944" name="ExecuteScalar" class="MaxActionNode" group="" path="Metreos.Native.Database" x="688" y="440">
      <linkto id="632989461822628960" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="literal">SELECT LAST_INSERT_ID()</ap>
        <ap name="Name" type="literal">activecalls</ap>
        <rd field="Scalar">g_recId</rd>
      </Properties>
    </node>
    <node type="Action" id="633071510444336687" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="952" y="552">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633071510444336925" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="440" y="616">
      <linkto id="632989461822628960" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("UPDATE activecalls SET state = 1 WHERE id = {0}", g_recId)</ap>
        <ap name="Name" type="literal">activecalls</ap>
      </Properties>
    </node>
    <node type="Action" id="633071510444336926" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="440" y="440">
      <linkto id="633071510444336925" type="Labeled" style="Vector" label="false" />
      <linkto id="633004250508108943" type="Labeled" style="Vector" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_recId == -1</ap>
      </Properties>
    </node>
    <node type="Variable" id="632989461822628968" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.Providers.JTapi.JTapiCallActive">to</Properties>
    </node>
    <node type="Variable" id="632989461822628969" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.JTapi.JTapiCallActive">deviceName</Properties>
    </node>
    <node type="Variable" id="632989461822628970" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.Providers.JTapi.JTapiCallActive">callId</Properties>
    </node>
    <node type="Variable" id="632989461822628971" name="isSubscribed" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">isSubscribed</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallInactive" activetab="true" startnode="632989461822628845" treenode="632989461822628846" appnode="632989461822628843" handlerfor="632989461822628842">
    <node type="Start" id="632989461822628845" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="103" y="445">
      <linkto id="632989461822629091" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632989461822629088" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="688" y="440">
      <linkto id="632989461822629089" type="Labeled" style="Vector" label="true" />
      <linkto id="633071510444336686" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">isSubscribed</ap>
      </Properties>
    </node>
    <node type="Action" id="632989461822629089" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="832" y="328">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632989461822629091" name="NotifyCallInactive" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="363.6569" y="443">
      <linkto id="633071510444336684" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DeviceName" type="variable">deviceName</ap>
        <ap name="InUse" type="variable">inUse</ap>
        <rd field="IsSubscriber">isSubscribed</rd>
      </Properties>
    </node>
    <node type="Action" id="633071510444336684" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="528" y="440">
      <linkto id="632989461822629088" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("UPDATE activecalls SET state = 0 WHERE id = {0}", g_recId)</ap>
        <ap name="Name" type="literal">activecalls</ap>
      </Properties>
    </node>
    <node type="Action" id="633071510444336686" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="831" y="549">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632989461822629096" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInactive">callId</Properties>
    </node>
    <node type="Variable" id="632989461822629097" name="inUse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" initWith="InUse" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInactive">inUse</Properties>
    </node>
    <node type="Variable" id="632989461822629098" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInactive">deviceName</Properties>
    </node>
    <node type="Variable" id="632989461822629099" name="isSubscribed" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">isSubscribed</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiHangup" startnode="632989461822628850" treenode="632989461822628851" appnode="632989461822628848" handlerfor="632989461822628847">
    <node type="Start" id="632989461822628850" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="151" y="461">
      <linkto id="632989461822629225" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632989461822629224" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="712" y="464">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632989461822629225" name="NotifyHangup" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="424.432922" y="461">
      <linkto id="633004250508108715" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Cause" type="variable">cause</ap>
        <ap name="DeviceName" type="variable">deviceName</ap>
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="633004250508108715" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="568" y="464">
      <linkto id="632989461822629224" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("DELETE FROM activecalls WHERE id = {0}", g_recId)</ap>
        <ap name="Name" type="literal">activecalls</ap>
      </Properties>
    </node>
    <node type="Variable" id="632989461822629228" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.Providers.JTapi.JTapiHangup">callId</Properties>
    </node>
    <node type="Variable" id="632989461822629229" name="cause" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Cause" refType="reference" name="Metreos.Providers.JTapi.JTapiHangup">cause</Properties>
    </node>
    <node type="Variable" id="632989461822629230" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.JTapi.JTapiHangup">deviceName</Properties>
    </node>
    <node type="Variable" id="632989461822629231" name="isSubscriber" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">isSubscriber</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>