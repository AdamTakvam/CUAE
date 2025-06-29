<Application name="InitiateRetrieveControlKeys" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="InitiateRetrieveControlKeys">
    <outline>
      <treenode type="evh" id="632346881090781715" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632346881090781712" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632346881090781711" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="method" type="literal">InternalSendEvent</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_ccm_device_username" id="632347576248441478" vid="632346881090781722">
        <Properties type="String" initWith="CCM_Device_Username">g_ccm_device_username</Properties>
      </treenode>
      <treenode text="g_ccm_device_password" id="632347576248441480" vid="632346881090781724">
        <Properties type="String" initWith="CCM_Device_Password">g_ccm_device_password</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632346881090781714" treenode="632346881090781715" appnode="632346881090781712" handlerfor="632346881090781711">
    <node type="Start" id="632346881090781714" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="288">
      <linkto id="632346881090781718" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632346881090781718" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="248" y="288">
      <linkto id="632346881090781721" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="URL1" type="csharp">"http://" + host + "/RequestKeys?metreosSessionId=" + parentRoutingGuid + "&amp;perspective=" + perspective + "&amp;command=" + command</ap>
        <rd field="ResultData">execute</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"Sending to phone " + phoneIp + " URL: " + "http://" + host + "/RequestKeys?metreosSessionId=" + parentRoutingGuid + "&amp;perspective=" + perspective + "&amp;command=" + command</log>
      </Properties>
    </node>
    <node type="Action" id="632346881090781721" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="416" y="288">
      <linkto id="632346881090781726" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="variable">execute</ap>
        <ap name="URL" type="variable">phoneIp</ap>
        <ap name="Username" type="variable">g_ccm_device_username</ap>
        <ap name="Password" type="variable">g_ccm_device_password</ap>
      </Properties>
    </node>
    <node type="Action" id="632346881090781726" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="560" y="288">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632346881090781716" name="phoneIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="phoneIp" refType="reference">phoneIp</Properties>
    </node>
    <node type="Variable" id="632346881090781717" name="parentRoutingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="parentRoutingGuid" refType="reference">parentRoutingGuid</Properties>
    </node>
    <node type="Variable" id="632346881090781719" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="632346881090781720" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="applicationServerIpAndPort" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632346908118126201" name="perspective" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="perspective" refType="reference">perspective</Properties>
    </node>
    <node type="Variable" id="632346908118127694" name="command" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="command" refType="reference">command</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>