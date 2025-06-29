<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632531424449612727" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632531424449612724" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632531424449612723" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">IVT.EMLogout.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Logout" id="632532119448906632" vid="632531424449612740">
        <Properties type="String" initWith="S_Logout">S_Logout</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632531424449612726" treenode="632531424449612727" appnode="632531424449612724" handlerfor="632531424449612723">
    <node type="Start" id="632531424449612726" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="230">
      <linkto id="632532119448906579" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632531424449612739" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="349" y="232">
      <linkto id="632531424449612766" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_Logout</ap>
      </Properties>
    </node>
    <node type="Action" id="632531424449612764" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="196" y="342">
      <linkto id="632531424449612766" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="errormessage" type="variable">errormessage</ap>
        <ap name="errorcode" type="variable">errorcode</ap>
        <ap name="signalName" type="variable">S_Logout</ap>
      </Properties>
    </node>
    <node type="Action" id="632531424449612766" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="348" y="348">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632532119448906579" name="Logout" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="198" y="233">
      <linkto id="632531424449612739" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632531424449612764" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AppId" type="variable">username</ap>
        <ap name="AppCertificate" type="variable">password</ap>
        <ap name="DeviceName" type="variable">devicename</ap>
        <ap name="Url" type="variable">url</ap>
        <rd field="ErrorCode">errorcode</rd>
        <rd field="ErrorMessage">errormessage</rd>
      </Properties>
    </node>
    <node type="Variable" id="632531424449612729" name="username" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="username" refType="reference">username</Properties>
    </node>
    <node type="Variable" id="632531424449612730" name="password" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="password" refType="reference">password</Properties>
    </node>
    <node type="Variable" id="632531424449612731" name="devicename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="devicename" refType="reference">devicename</Properties>
    </node>
    <node type="Variable" id="632531424449612735" name="url" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="url" refType="reference">url</Properties>
    </node>
    <node type="Variable" id="632531424449612737" name="errorcode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">errorcode</Properties>
    </node>
    <node type="Variable" id="632531424449612738" name="errormessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">errormessage</Properties>
    </node>
    <node type="Variable" id="632531424449612767" name="shim" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoExtensionMobility.QueryDeviceResults" refType="reference">shim</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
