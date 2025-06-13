<Application name="HandleRequest" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="HandleRequest">
    <outline>
      <treenode type="evh" id="632566097375090484" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632566097375090481" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632566097375090480" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632566097375090483" treenode="632566097375090484" appnode="632566097375090481" handlerfor="632566097375090480">
    <node type="Start" id="632566097375090483" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="119">
      <linkto id="632566097375090486" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632566097375090485" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="537" y="119">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632566097375090486" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="164" y="119">
      <linkto id="632570260847122598" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
      </Properties>
    </node>
    <node type="Action" id="632570260847122597" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="403" y="122">
      <linkto id="632566097375090485" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="variable">execute</ap>
        <ap name="URL" type="literal">http://10.1.10.68/CGI/Execute</ap>
        <ap name="Username" type="literal">metreos</ap>
        <ap name="Password" type="literal">metreos</ap>
      </Properties>
    </node>
    <node type="Action" id="632570260847122598" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="281" y="122">
      <linkto id="632570260847122597" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">http://www.openssl.org/news/secadv_20040317.txt</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Variable" id="632566097375090487" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632566893506495519" name="connId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connId</Properties>
    </node>
    <node type="Variable" id="632570260847122599" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>