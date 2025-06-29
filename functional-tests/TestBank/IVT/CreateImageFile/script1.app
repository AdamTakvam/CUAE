<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632520413815259555" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632520413815259552" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632520413815259551" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/CreateImageFile</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632520413815259554" treenode="632520413815259555" appnode="632520413815259552" handlerfor="632520413815259551">
    <node type="Start" id="632520413815259554" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="384">
      <linkto id="632520413815259559" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632520413815259559" name="CreateImageFile" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="226" y="385">
      <linkto id="632520413815259589" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">IVT Image File</ap>
        <ap name="Prompt" type="literal">Options</ap>
        <ap name="LocationX" type="literal">-1</ap>
        <ap name="LocationY" type="literal">-1</ap>
        <ap name="URL" type="csharp">host + "/SampleImage"</ap>
        <rd field="ResultData">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632520413815259589" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="353.173828" y="382">
      <linkto id="632520413815259592" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Text</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/CreateText"</ap>
        <rd field="ResultData">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632520413815259590" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="724.1738" y="376">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632520413815259591" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="599.1738" y="377">
      <linkto id="632520413815259590" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">image.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632520413815259592" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="472.173828" y="384">
      <linkto id="632520413815259591" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">image</rd>
      </Properties>
    </node>
    <node type="Variable" id="632520413815259556" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632520413815259557" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632520413815259558" name="image" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.ImageFile" refType="reference">image</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
