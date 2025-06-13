<Application name="displayText" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="displayText">
    <outline>
      <treenode type="evh" id="632895921133701292" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632895921133701289" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632895921133701288" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/lunchOrder</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632895921133701291" treenode="632895921133701292" appnode="632895921133701289" handlerfor="632895921133701288">
    <node type="Start" id="632895921133701291" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632897616695359022" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632897616695359022" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="119" y="32">
      <linkto id="632897616695359026" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Lunch Menu</ap>
        <ap name="Prompt" type="literal">Choose an option</ap>
        <ap name="Text" type="literal">Welcome to the Deli</ap>
        <rd field="ResultData">textXML</rd>
      </Properties>
    </node>
    <node type="Action" id="632897616695359026" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="220.089172" y="32">
      <linkto id="632897616695359029" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">textXML.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632897616695359029" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="301.25" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632897616695359023" name="textXML" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">textXML</Properties>
    </node>
    <node type="Variable" id="632897616695359024" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <Properties desc="Example 1 - IP Phone Applications Core Developemnt Concepts">
  </Properties>
</Application>