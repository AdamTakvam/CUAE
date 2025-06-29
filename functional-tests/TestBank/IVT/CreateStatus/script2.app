<Application name="script2" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script2">
    <outline>
      <treenode type="evh" id="632527174907346806" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632527174907346803" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632527174907346802" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">IVT.CreateStatus.script2</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632527174907346805" treenode="632527174907346806" appnode="632527174907346803" handlerfor="632527174907346802">
    <node type="Start" id="632527174907346805" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="362">
      <linkto id="632527174907346818" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632527174907346818" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="246" y="355">
      <linkto id="632527174907346819" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">Init:AppStatus</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632527174907346819" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="432" y="358">
      <linkto id="632527174907346821" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">phoneIp</ap>
        <ap name="Username" type="variable">username</ap>
        <ap name="Password" type="variable">password</ap>
      </Properties>
    </node>
    <node type="Action" id="632527174907346821" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="629" y="359">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632527174907346815" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="632527174907346816" name="phoneIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ip" refType="reference">phoneIp</Properties>
    </node>
    <node type="Variable" id="632527174907346822" name="username" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="username" refType="reference">username</Properties>
    </node>
    <node type="Variable" id="632527174907346823" name="password" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="password" refType="reference">password</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
