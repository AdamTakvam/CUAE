<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632575382506462286" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632575382506462283" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632575382506462282" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/timedholdservice</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632575382506462285" treenode="632575382506462286" appnode="632575382506462283" handlerfor="632575382506462282">
    <node type="Start" id="632575382506462285" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632575382506462295" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575382506462291" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="217.999359" y="415">
      <linkto id="632575382506462292" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Test</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632575382506462292" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="357" y="415">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575382506462295" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="75" y="204">
      <linkto id="632575382506462298" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">Key:Services</ap>
        <ap name="URL2" type="literal">Key:Soft3</ap>
        <ap name="URL3" type="literal">Dial:7880</ap>
        <rd field="ResultData">transferExecuteItem</rd>
      </Properties>
    </node>
    <node type="Action" id="632575382506462297" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="355" y="205">
      <linkto id="632575382506462299" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">Key:Soft3</ap>
        <rd field="ResultData">finalExecuteItem</rd>
      </Properties>
    </node>
    <node type="Action" id="632575382506462298" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="210" y="206">
      <linkto id="632575382506462354" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">transferExecuteItem.ToString()</ap>
        <ap name="URL" type="variable">remotePhoneIp</ap>
        <ap name="Username" type="literal">metreos</ap>
        <ap name="Password" type="literal">metreos</ap>
      </Properties>
    </node>
    <node type="Action" id="632575382506462299" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="506" y="206">
      <linkto id="632575382506462291" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">finalExecuteItem.ToString()</ap>
        <ap name="URL" type="variable">remotePhoneIp</ap>
        <ap name="Username" type="literal">metreos</ap>
        <ap name="Password" type="literal">metreos</ap>
      </Properties>
    </node>
    <node type="Action" id="632575382506462354" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="290" y="115">
      <linkto id="632575382506462297" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">1000</ap>
      </Properties>
    </node>
    <node type="Variable" id="632575382506462293" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632575382506462294" name="transferExecuteItem" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">transferExecuteItem</Properties>
    </node>
    <node type="Variable" id="632575382506462296" name="finalExecuteItem" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">finalExecuteItem</Properties>
    </node>
    <node type="Variable" id="632575382506462300" name="remotePhoneIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteIpAddress" refType="reference" name="Metreos.Providers.Http.GotRequest">remotePhoneIp</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>