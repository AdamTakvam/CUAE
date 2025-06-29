<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632531424449612727" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632531424449612724" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632531424449612723" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">IVT.EMQueryDevices.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_QueryDevices" id="632532119448906681" vid="632531424449612740">
        <Properties type="String" initWith="S_QueryDevices">S_QueryDevices</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632531424449612726" treenode="632531424449612727" appnode="632531424449612724" handlerfor="632531424449612723">
    <node type="Start" id="632531424449612726" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="230">
      <linkto id="632532119448906671" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632531424449612739" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="455" y="225">
      <linkto id="632531424449612766" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="message" type="csharp">String.Format("User {0} associated with device", user)</ap>
        <ap name="signalName" type="variable">S_QueryDevices</ap>
      </Properties>
    </node>
    <node type="Action" id="632531424449612764" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="194" y="351">
      <linkto id="632531424449612766" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="errormessage" type="variable">errormessage</ap>
        <ap name="signalName" type="variable">S_QueryDevices</ap>
      </Properties>
    </node>
    <node type="Action" id="632531424449612766" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="453" y="351">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632532119448906671" name="QueryDevices" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="194" y="229">
      <linkto id="632531424449612764" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632532119448906672" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="AppId" type="variable">username</ap>
        <ap name="AppCertificate" type="variable">password</ap>
        <ap name="DeviceNames" type="csharp">new string[] { devicename }</ap>
        <ap name="Url" type="variable">url</ap>
        <rd field="QueryDevicesResult">response</rd>
        <rd field="ErrorMessage">errormessage</rd>
      </Properties>
    </node>
    <node type="Action" id="632532119448906672" name="GetDeviceStatus" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="317" y="82">
      <linkto id="632531424449612739" type="Labeled" style="Bezier" ortho="true" label="LoggedIn" />
      <linkto id="632532119448906699" type="Labeled" style="Bezier" ortho="true" label="NoDevice" />
      <linkto id="632532119448906701" type="Labeled" style="Bezier" ortho="true" label="NoUser" />
      <linkto id="632531424449612764" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <Properties final="false" type="native" log="On">
        <ap name="DeviceName" type="variable">devicename</ap>
        <ap name="QueryDeviceResults" type="variable">response</ap>
        <rd field="Username">user</rd>
      </Properties>
    </node>
    <node type="Action" id="632532119448906699" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="451" y="33">
      <linkto id="632531424449612766" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="message" type="literal">Device not found</ap>
        <ap name="signalName" type="variable">S_QueryDevices</ap>
      </Properties>
    </node>
    <node type="Action" id="632532119448906701" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="451" y="114">
      <linkto id="632531424449612766" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="message" type="literal">No user associated with device</ap>
        <ap name="signalName" type="variable">S_QueryDevices</ap>
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
    <node type="Variable" id="632531424449612738" name="errormessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">errormessage</Properties>
    </node>
    <node type="Variable" id="632531424449612767" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoExtensionMobility.QueryDeviceResults" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632532119448906703" name="user" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">user</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
