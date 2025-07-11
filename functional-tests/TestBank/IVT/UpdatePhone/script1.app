<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632531378747997245" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632531378747997242" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632531378747997241" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">IVT.UpdatePhone.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_UpdatePhoneResponse" id="632531378747997855" vid="632531378747997303">
        <Properties type="String" initWith="S_UpdatePhoneResponse">S_UpdatePhoneResponse</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632531378747997244" treenode="632531378747997245" appnode="632531378747997242" handlerfor="632531378747997241">
    <node type="Start" id="632531378747997244" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="372">
      <linkto id="632531378747997843" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632531378747997315" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="595" y="373">
      <linkto id="632531378747997318" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="message" type="literal">Check ccmadmin for update effect</ap>
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_UpdatePhoneResponse</ap>
      </Properties>
    </node>
    <node type="Action" id="632531378747997316" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="441" y="504">
      <linkto id="632531378747997318" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="message" type="variable">fault</ap>
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_UpdatePhoneResponse</ap>
      </Properties>
    </node>
    <node type="Action" id="632531378747997318" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="591" y="504">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632531378747997843" name="UpdatePhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="439" y="371">
      <linkto id="632531378747997316" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632531378747997315" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="Description" type="variable">description</ap>
        <ap name="PhoneName" type="variable">devicename</ap>
        <ap name="CallingSearchSpaceName" type="variable">callingSearchSpace</ap>
        <ap name="CallManagerIP" type="variable">callmanagerIp</ap>
        <ap name="AdminUsername" type="variable">username</ap>
        <ap name="AdminPassword" type="variable">password</ap>
      </Properties>
    </node>
    <node type="Variable" id="632531378747997305" name="devicename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="devicename" refType="reference">devicename</Properties>
    </node>
    <node type="Variable" id="632531378747997307" name="callmanagerIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callmanagerIp" refType="reference">callmanagerIp</Properties>
    </node>
    <node type="Variable" id="632531378747997308" name="username" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="username" refType="reference">username</Properties>
    </node>
    <node type="Variable" id="632531378747997309" name="password" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="password" refType="reference">password</Properties>
    </node>
    <node type="Variable" id="632531378747997310" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap333.UpdatePhoneResponse" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632531378747997311" name="fault" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">fault</Properties>
    </node>
    <node type="Variable" id="632531378747997313" name="message" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">message</Properties>
    </node>
    <node type="Variable" id="632531378747997844" name="description" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="description" refType="reference">description</Properties>
    </node>
    <node type="Variable" id="632531378747997845" name="callingSearchSpace" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callingSearchSpace" refType="reference">callingSearchSpace</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
