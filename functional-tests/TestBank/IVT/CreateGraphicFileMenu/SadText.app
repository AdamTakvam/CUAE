<Application name="SadText" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="SadText">
    <outline>
      <treenode type="evh" id="632520913769310295" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632520913769310292" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632520913769310291" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/SadText</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632520913769310294" treenode="632520913769310295" appnode="632520913769310292" handlerfor="632520913769310291">
    <node type="Start" id="632520913769310294" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="152" y="256">
      <linkto id="632520913769310296" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632520913769310296" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="272" y="256">
      <linkto id="632520913769310297" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">IVT Test</ap>
        <ap name="Prompt" type="literal">Options</ap>
        <ap name="Text" type="csharp">"You chose the sad face!"</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632520913769310297" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="400" y="256">
      <linkto id="632520913769310299" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Back</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632520913769310298" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="648" y="256">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632520913769310299" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="520" y="256">
      <linkto id="632520913769310298" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Variable" id="632520913769310304" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632520913769310305" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632520913769310306" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" initWith="RoutingGuid" refType="reference">text</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
