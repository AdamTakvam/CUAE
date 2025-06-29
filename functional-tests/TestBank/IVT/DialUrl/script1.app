<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632520913769310313" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632520913769310310" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632520913769310309" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">IVT.DialUrl.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Signal" id="632527174907346166" vid="632520913769310322">
        <Properties type="String" initWith="S_Signal">S_Signal</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632520913769310312" treenode="632520913769310313" appnode="632520913769310310" handlerfor="632520913769310309">
    <node type="Start" id="632520913769310312" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="368">
      <linkto id="632520913769310328" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632520913769310328" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="200" y="368">
      <linkto id="632520913769310329" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"Dial:" + to</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632520913769310329" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="336" y="368">
      <linkto id="632520913769310331" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632520913769310332" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">ip</ap>
        <ap name="Username" type="variable">username</ap>
        <ap name="Password" type="variable">password</ap>
      </Properties>
    </node>
    <node type="Action" id="632520913769310331" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="464" y="368">
      <linkto id="632520913769310334" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_Signal</ap>
      </Properties>
    </node>
    <node type="Action" id="632520913769310332" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="336" y="480">
      <linkto id="632520913769310334" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_Signal</ap>
      </Properties>
    </node>
    <node type="Action" id="632520913769310334" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="464" y="480">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632520913769310324" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632520913769310325" name="ip" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ip" refType="reference">ip</Properties>
    </node>
    <node type="Variable" id="632520913769310326" name="username" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="username" refType="reference">username</Properties>
    </node>
    <node type="Variable" id="632520913769310327" name="password" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="password" refType="reference">password</Properties>
    </node>
    <node type="Variable" id="632520913769310330" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
