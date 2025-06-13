<Application name="WorkerBee" trigger="Metreos.Events.Reserve.LaunchWorkerBee" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="WorkerBee">
    <outline>
      <treenode type="evh" id="632831347185302635" level="1" text="Metreos.Events.Reserve.LaunchWorkerBee (trigger): OnLaunchWorkerBee">
        <node type="function" name="OnLaunchWorkerBee" id="632831347185302632" path="Metreos.StockTools" />
        <node type="event" name="LaunchWorkerBee" id="632831347185302631" path="Metreos.Events.Reserve.LaunchWorkerBee" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632831349333412515" level="1" text="Metreos.Events.Reserve.Send: OnSend">
        <node type="function" name="OnSend" id="632831349333412512" path="Metreos.StockTools" />
        <node type="event" name="Send" id="632831349333412511" path="Metreos.Events.Reserve.Send" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632831349333412520" level="1" text="Metreos.Events.Reserve.Exit: OnExit">
        <node type="function" name="OnExit" id="632831349333412517" path="Metreos.StockTools" />
        <node type="event" name="Exit" id="632831349333412516" path="Metreos.Events.Reserve.Exit" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnLaunchWorkerBee" startnode="632831347185302634" treenode="632831347185302635" appnode="632831347185302632" handlerfor="632831347185302631">
    <node type="Start" id="632831347185302634" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="270">
      <linkto id="632831347185302637" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632831347185302637" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="245" y="271">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">WorkerBee online</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSend" activetab="true" startnode="632831349333412514" treenode="632831349333412515" appnode="632831349333412512" handlerfor="632831349333412511">
    <node type="Start" id="632831349333412514" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="272">
      <linkto id="632831349333412553" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632831349333412553" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="176" y="272">
      <linkto id="632831349333412556" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632831349333412557" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">device</ap>
        <ap name="Status" type="literal">Registered</ap>
        <rd field="ResultData">results</rd>
        <rd field="Count">resultCount</rd>
      </Properties>
    </node>
    <node type="Action" id="632831349333412556" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="352" y="272">
      <linkto id="632831349333412559" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632831349333412561" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">resultCount != 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632831349333412557" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="176" y="400">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="csharp">"Failed to Query Device List X for " + device</log>
      </Properties>
    </node>
    <node type="Action" id="632831349333412559" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="352" y="400">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="csharp">"No results found for device " + device</log>
      </Properties>
    </node>
    <node type="Action" id="632831349333412561" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="464" y="272">
      <linkto id="632831349333412716" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="variable">title</ap>
        <ap name="Text" type="csharp">text.Length &gt; 110 ? text.Substring(110) : text</ap>
        <rd field="ResultData">textXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632831349333412562" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="736" y="272">
      <linkto id="632831349333412719" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">textXml.ToString()</ap>
        <ap name="URL" type="csharp">results.Rows[0]["IP"] as string</ap>
        <ap name="Username" type="variable">user</ap>
        <ap name="Password" type="variable">pin</ap>
      </Properties>
    </node>
    <node type="Action" id="632831349333412716" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="584" y="272">
      <linkto id="632831349333412562" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Confirm</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Confirm/" + appendIp + "?d=" + device + "&amp;r=" + recordId + "&amp;u=" + user</ap>
        <rd field="ResultData">textXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632831349333412719" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="864" y="272">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632831349333412522" name="title" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Title" refType="reference" name="Metreos.Events.Reserve.Send">title</Properties>
    </node>
    <node type="Variable" id="632831349333412523" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Text" refType="reference" name="Metreos.Events.Reserve.Send">text</Properties>
    </node>
    <node type="Variable" id="632831349333412524" name="device" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Events.Reserve.Send">device</Properties>
    </node>
    <node type="Variable" id="632831349333412554" name="results" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">results</Properties>
    </node>
    <node type="Variable" id="632831349333412555" name="resultCount" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">resultCount</Properties>
    </node>
    <node type="Variable" id="632831349333412558" name="user" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="User" refType="reference">user</Properties>
    </node>
    <node type="Variable" id="632831349333412563" name="textXml" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">textXml</Properties>
    </node>
    <node type="Variable" id="632831349333412718" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632846669383433846" name="pin" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Pin" refType="reference">pin</Properties>
    </node>
    <node type="Variable" id="632846669383434653" name="appendIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="AppendIp" refType="reference">appendIp</Properties>
    </node>
    <node type="Variable" id="632852893472969488" name="recordId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RecordId" refType="reference">recordId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnExit" startnode="632831349333412519" treenode="632831349333412520" appnode="632831349333412517" handlerfor="632831349333412516">
    <node type="Start" id="632831349333412519" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="104" y="304">
      <linkto id="632831349333412521" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632831349333412521" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="256" y="304">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">WorkerBee offline</log>
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>