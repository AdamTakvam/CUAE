<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632520413815259555" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632520413815259552" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632520413815259551" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/CreateGraphicFileMenu</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632520413815259554" treenode="632520413815259555" appnode="632520413815259552" handlerfor="632520413815259551">
    <node type="Start" id="632520413815259554" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="384">
      <linkto id="632520413815259632" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632520413815259589" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="553.1738" y="383">
      <linkto id="632520413815259592" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Text</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/CreateText"</ap>
        <rd field="ResultData">graphicFileMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632520413815259590" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="946.6372" y="391">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632520413815259591" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="804.521362" y="390">
      <linkto id="632520413815259590" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">graphicFileMenu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632520413815259592" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="681.1738" y="383">
      <linkto id="632520413815259591" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">graphicFileMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632520413815259632" name="CreateGraphicFileMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="188" y="383">
      <linkto id="632520413815259633" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">IVT Graphic Menu File</ap>
        <ap name="Prompt" type="literal">Options</ap>
        <ap name="LocationX" type="literal">-1</ap>
        <ap name="LocationY" type="literal">-1</ap>
        <ap name="URL" type="csharp">hostIp + "/graphicfile.png"</ap>
        <rd field="ResultData">graphicFileMenu</rd>
        <log condition="entry" on="true" level="Info" type="csharp">host + "/graphicfile.png"</log>
      </Properties>
    </node>
    <node type="Action" id="632520413815259633" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="318.463379" y="381">
      <linkto id="632520413815259634" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">One</ap>
        <ap name="URL" type="csharp">host + "/HappyText"</ap>
        <ap name="TouchAreaX1" type="literal">24</ap>
        <ap name="TouchAreaX2" type="literal">130</ap>
        <ap name="TouchAreaY1" type="literal">24</ap>
        <ap name="TouchAreaY2" type="literal">150</ap>
        <rd field="ResultData">graphicFileMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632520413815259634" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="439.463379" y="379">
      <linkto id="632520413815259589" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Sad</ap>
        <ap name="URL" type="csharp">host + "/SadText"</ap>
        <ap name="TouchAreaX1" type="literal">166</ap>
        <ap name="TouchAreaX2" type="literal">270</ap>
        <ap name="TouchAreaY1" type="literal">24</ap>
        <ap name="TouchAreaY2" type="literal">150</ap>
        <rd field="ResultData">graphicFileMenu</rd>
      </Properties>
    </node>
    <node type="Variable" id="632520413815259556" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632520413815259557" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632520413815259558" name="graphicFileMenu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.GraphicFileMenu" refType="reference">graphicFileMenu</Properties>
    </node>
    <node type="Variable" id="632527174907346061" name="hostIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="hostname" refType="reference" name="Metreos.Providers.Http.GotRequest">hostIp</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
