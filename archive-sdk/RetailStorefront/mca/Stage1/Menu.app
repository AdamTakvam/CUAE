<Application name="Menu" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Menu">
    <outline>
      <treenode type="evh" id="632875147412707623" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632875147412707620" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875147412707619" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/Menu</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632875147412707622" treenode="632875147412707623" appnode="632875147412707620" handlerfor="632875147412707619">
    <node type="Start" id="632875147412707622" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="296">
      <linkto id="632875147412707686" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707686" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="168" y="296">
      <linkto id="632875147412707687" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="literal">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632875147412707687" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="320" y="224">
      <linkto id="632875147412707689" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Prompt" type="literal">Make your selection</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707689" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="449" y="224">
      <linkto id="632875147412707690" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Store Inventory</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/StoreInventory?metreosSessionId=" + query["appId"]</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707690" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="584" y="224">
      <linkto id="632875147412707691" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Website Inventory</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/WebInventory?metreosSessionId=" + query["appId"]</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707691" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="752" y="224">
      <linkto id="632875147412707692" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Floor Manager</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/FloorManager?metreosSessionId=" + query["appId"]</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707692" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="896" y="224">
      <linkto id="632875147412707694" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Warehouse Manager</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/WarehouseManager?metreosSessionId=" + query["appId"]</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707693" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1152" y="224">
      <linkto id="632875147412707696" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Check Out</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/CheckOut?metreosSessionId=" + query["appId"]</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707694" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1016" y="224">
      <linkto id="632875147412707693" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Check In</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/CheckIn?metreosSessionId=" + query["appId"]</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707696" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1288" y="296">
      <linkto id="632875147412707698" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875147412707698" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1408" y="296">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632875147412707685" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">query</Properties>
    </node>
    <node type="Variable" id="632875147412707688" name="menu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">menu</Properties>
    </node>
    <node type="Variable" id="632875147412707695" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632875147412707697" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>