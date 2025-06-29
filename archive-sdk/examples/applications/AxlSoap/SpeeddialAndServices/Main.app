<Application name="Main" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="Main">
    <outline>
      <treenode type="evh" id="632423474098293106" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632423474098293103" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632423474098293102" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/speeddialservice</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632423474098293105" treenode="632423474098293106" appnode="632423474098293103" handlerfor="632423474098293102">
    <node type="Start" id="632423474098293105" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="368">
      <linkto id="632423474098293107" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632423474098293107" name="AddSpeeddialItem" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="136" y="368">
      <linkto id="632423474098293112" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="DirectoryNumber" type="literal">1000</ap>
        <ap name="Label" type="literal">Speedy</ap>
        <ap name="Index" type="literal">1</ap>
        <rd field="Speeddial">speeddials</rd>
      </Properties>
    </node>
    <node type="Action" id="632423474098293108" name="AddServiceItem" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="552" y="368">
      <linkto id="632423474098293114" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Service2</ap>
        <ap name="Url" type="csharp">host + "/testscript"</ap>
        <ap name="UrlButtonIndex" type="literal">1</ap>
        <ap name="UrlLabel" type="literal">Do2</ap>
        <ap name="TelecasterServiceName" type="literal">Service2</ap>
        <rd field="Service">services</rd>
      </Properties>
    </node>
    <node type="Action" id="632423474098293112" name="AddSpeeddialItem" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="264" y="368">
      <linkto id="632423474098293113" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="DirectoryNumber" type="literal">1001</ap>
        <ap name="Label" type="literal">Speedier</ap>
        <ap name="Index" type="literal">2</ap>
        <rd field="Speeddial">speeddials</rd>
      </Properties>
    </node>
    <node type="Action" id="632423474098293113" name="AddServiceItem" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="400" y="368">
      <linkto id="632423474098293108" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Service1</ap>
        <ap name="Url" type="csharp">host + "/testscript"</ap>
        <ap name="UrlButtonIndex" type="literal">2</ap>
        <ap name="UrlLabel" type="literal">Do</ap>
        <ap name="TelecasterServiceName" type="literal">Service1</ap>
        <rd field="Service">services</rd>
        <log condition="entry" on="true" level="Info" type="csharp"> host + "/testscript"</log>
      </Properties>
    </node>
    <node type="Action" id="632423474098293114" name="UpdatePhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="688" y="368">
      <linkto id="632423474098293116" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="PhoneName" type="literal">SEP001243F504B3</ap>
        <ap name="Speeddials" type="variable">speeddials</ap>
        <ap name="Services" type="variable">services</ap>
        <ap name="CallManagerIP" type="literal">10.1.10.25</ap>
        <ap name="AdminPassword" type="literal">metreos</ap>
      </Properties>
    </node>
    <node type="Action" id="632423474098293116" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="800" y="368">
      <linkto id="632423474098293117" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Test</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
      </Properties>
    </node>
    <node type="Action" id="632423474098293117" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="904" y="368">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632423474098293109" name="speeddials" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap333.Speeddials" refType="reference">speeddials</Properties>
    </node>
    <node type="Variable" id="632423474098293110" name="services" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap333.Services" refType="reference">services</Properties>
    </node>
    <node type="Variable" id="632423474098293111" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632423474098293118" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632423474098293149" name="getPhoneResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap333.GetPhoneResponse" refType="reference">getPhoneResponse</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>