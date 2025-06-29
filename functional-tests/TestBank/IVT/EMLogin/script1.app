<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632531424449612727" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632531424449612724" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632531424449612723" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">IVT.EMLogin.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Login" id="632532273616837867" vid="632531424449612740">
        <Properties type="String" initWith="S_Login">S_Login</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632531424449612726" treenode="632531424449612727" appnode="632531424449612724" handlerfor="632531424449612723">
    <node type="Start" id="632531424449612726" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="230">
      <linkto id="632532255208247679" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632531424449612728" name="Login" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="197" y="229">
      <linkto id="632531424449612764" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632531424449612739" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="AppId" type="variable">username</ap>
        <ap name="AppCertificate" type="variable">password</ap>
        <ap name="UserId" type="variable">userid</ap>
        <ap name="DeviceName" type="variable">devicename</ap>
        <ap name="DeviceProfile" type="csharp">profilename == String.Empty ? null : profilename</ap>
        <ap name="Timeout" type="csharp">notimeout == true ? null : timeout</ap>
        <ap name="NoTimeout" type="variable">notimeout</ap>
        <ap name="Url" type="variable">url</ap>
        <rd field="ErrorCode">errorcode</rd>
        <rd field="ErrorMessage">errormessage</rd>
      </Properties>
    </node>
    <node type="Action" id="632531424449612739" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="349" y="232">
      <linkto id="632531424449612766" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_Login</ap>
      </Properties>
    </node>
    <node type="Action" id="632531424449612764" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="196" y="342">
      <linkto id="632531424449612766" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="errormessage" type="variable">errormessage</ap>
        <ap name="errorcode" type="variable">errorcode</ap>
        <ap name="signalName" type="variable">S_Login</ap>
      </Properties>
    </node>
    <node type="Action" id="632531424449612766" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="348" y="348">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632532255208247679" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="119" y="231">
      <linkto id="632531424449612728" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string username, string password, string userid, string url, string devicename, string profilename, string timeout, bool notimeout, LogWriter log)
{
	log.Write(TraceLevel.Info, String.Format(@"
Username {0}
Password {1}
UserId   {2}
Url      {3}
Device   {4}
Profile  {5}
Timeout  {6}
NoTimeO  {7}", username, password, userid, url, devicename, profilename, timeout, notimeout));

	
	return "";
}
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
    <node type="Variable" id="632531424449612732" name="profilename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="profilename" refType="reference">profilename</Properties>
    </node>
    <node type="Variable" id="632531424449612733" name="timeout" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="timeout" refType="reference">timeout</Properties>
    </node>
    <node type="Variable" id="632531424449612734" name="notimeout" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" initWith="notimeout" refType="reference">notimeout</Properties>
    </node>
    <node type="Variable" id="632531424449612735" name="url" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="url" refType="reference">url</Properties>
    </node>
    <node type="Variable" id="632531424449612736" name="userid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="userid" refType="reference">userid</Properties>
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
