<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632520913769310402" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632520913769310399" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632520913769310398" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">IVT.SendExecute.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Signal" id="633180562903085064" vid="632520913769310427">
        <Properties type="String" initWith="S_Signal">S_Signal</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632520913769310401" treenode="632520913769310402" appnode="632520913769310399" handlerfor="632520913769310398">
    <node type="Start" id="632520913769310401" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="72" y="304">
      <linkto id="632520913769310412" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632520913769310412" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="184" y="304">
      <linkto id="632520913769310413" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="variable">url</ap>
        <rd field="ResultData">execute</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"Url: " + url</log>
      </Properties>
    </node>
    <node type="Action" id="632520913769310413" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="320" y="304">
      <linkto id="632520913769310414" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632520913769310415" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">ip</ap>
        <ap name="Username" type="variable">username</ap>
        <ap name="Password" type="variable">password</ap>
      </Properties>
    </node>
    <node type="Action" id="632520913769310414" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="448" y="304">
      <linkto id="632520913769310416" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_Signal</ap>
      </Properties>
    </node>
    <node type="Action" id="632520913769310415" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="320" y="416">
      <linkto id="632520913769310416" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_Signal</ap>
      </Properties>
    </node>
    <node type="Action" id="632520913769310416" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="448" y="416">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632520913769310422" name="url" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="url" refType="reference" name="Metreos.Providers.Http.GotRequest">url</Properties>
    </node>
    <node type="Variable" id="632520913769310423" name="ip" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ip" refType="reference">ip</Properties>
    </node>
    <node type="Variable" id="632520913769310424" name="username" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="username" refType="reference">username</Properties>
    </node>
    <node type="Variable" id="632520913769310425" name="password" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="password" refType="reference">password</Properties>
    </node>
    <node type="Variable" id="632520913769310426" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" initWith="" refType="reference">execute</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>