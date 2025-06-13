<Application name="displayMenu" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="displayMenu">
    <outline>
      <treenode type="evh" id="632895921133701292" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632895921133701289" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632895921133701288" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/lunchOrder3</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632897813125414028" level="2" text="Metreos.Providers.Http.GotRequest: OnGotRequest1">
        <node type="function" name="OnGotRequest1" id="632897813125414025" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632897813125414024" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/showMenu</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632897813125414081" level="2" text="Metreos.Providers.Http.GotRequest: OnGotRequest2">
        <node type="function" name="OnGotRequest2" id="632897813125414078" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632897813125414077" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/menuSelection</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632895921133701291" treenode="632895921133701292" appnode="632895921133701289" handlerfor="632895921133701288">
    <node type="Start" id="632895921133701291" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="32">
      <linkto id="632897616695359022" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632897616695359022" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="109" y="32">
      <linkto id="632897813125413977" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Lunch Menu</ap>
        <ap name="Prompt" type="literal">Choose an option</ap>
        <ap name="Text" type="literal">Welcome to the Deli</ap>
        <rd field="ResultData">textXML</rd>
      </Properties>
    </node>
    <node type="Action" id="632897616695359026" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="403.089172" y="32">
      <linkto id="632897813125414029" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">textXML.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632897813125413977" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="199" y="32">
      <linkto id="632897813125413990" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Breakfast</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">"http://" + host + "/showMenu?metreosSessionId=" + routingGuid + "&amp;value=Breakfast"</ap>
        <rd field="ResultData">textXML</rd>
      </Properties>
    </node>
    <node type="Action" id="632897813125413990" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="304.173828" y="32">
      <linkto id="632897616695359026" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Lunch</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="csharp">"http://" + host + "/showMenu?metreosSessionId=" + routingGuid + "&amp;value=Lunch"</ap>
        <rd field="ResultData">textXML</rd>
      </Properties>
    </node>
    <node type="Action" id="632897813125414029" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="491" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632897616695359023" name="textXML" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">textXML</Properties>
    </node>
    <node type="Variable" id="632897616695359024" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632897813125413978" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632897813125413979" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotRequest1" startnode="632897813125414027" treenode="632897813125414028" appnode="632897813125414025" handlerfor="632897813125414024">
    <node type="Start" id="632897813125414027" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="102">
      <linkto id="632897813125414032" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632897813125414032" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="113" y="102">
      <linkto id="632897813125414033" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="csharp">query["value"] + " Menu"</ap>
        <ap name="Prompt" type="literal">Please make a selection</ap>
        <rd field="ResultData">menuXML</rd>
      </Properties>
    </node>
    <node type="Action" id="632897813125414033" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="186" y="102">
      <linkto id="632897813125414034" type="Labeled" style="Bezier" ortho="true" label="Breakfast" />
      <linkto id="632897813125414068" type="Labeled" style="Bezier" ortho="true" label="Lunch" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">query["value"]</ap>
      </Properties>
    </node>
    <node type="Action" id="632897813125414034" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="271" y="53">
      <linkto id="632897813125414066" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Pancakes</ap>
        <ap name="URL" type="csharp">"http://" + host + "/menuSelection?metreosSessionId=" + routingGuid + "&amp;value=Pancakes"</ap>
        <rd field="ResultData">menuXML</rd>
      </Properties>
    </node>
    <node type="Action" id="632897813125414066" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="368" y="53">
      <linkto id="632897813125414074" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Eggs</ap>
        <ap name="URL" type="csharp">"http://" + host + "/menuSelection?metreosSessionId=" + routingGuid + "&amp;value=Eggs"</ap>
        <rd field="ResultData">menuXML</rd>
      </Properties>
    </node>
    <node type="Action" id="632897813125414068" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="274" y="127">
      <linkto id="632897813125414070" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Pizza</ap>
        <ap name="URL" type="csharp">"http://" + host + "/menuSelection?metreosSessionId=" + routingGuid + "&amp;value=Pizza"</ap>
        <rd field="ResultData">menuXML</rd>
      </Properties>
    </node>
    <node type="Action" id="632897813125414070" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="372" y="127">
      <linkto id="632897813125414074" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Burger</ap>
        <ap name="URL" type="csharp">"http://" + host + "/menuSelection?metreosSessionId=" + routingGuid + "&amp;value=Burger"</ap>
        <rd field="ResultData">menuXML</rd>
      </Properties>
    </node>
    <node type="Action" id="632897813125414074" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="460.589172" y="88">
      <linkto id="632897813125414091" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">menuXML.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632897813125414091" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="547" y="88">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632897813125414030" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">query</Properties>
    </node>
    <node type="Variable" id="632897813125414031" name="menuXML" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">menuXML</Properties>
    </node>
    <node type="Variable" id="632897813125414035" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632897813125414063" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632897813125414072" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotRequest2" activetab="true" startnode="632897813125414080" treenode="632897813125414081" appnode="632897813125414078" handlerfor="632897813125414077">
    <node type="Start" id="632897813125414080" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632897813125414082" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632897813125414082" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="113" y="32">
      <linkto id="632897813125414084" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Order Received</ap>
        <ap name="Prompt" type="literal">Thank You</ap>
        <ap name="Text" type="csharp">"Your order:\n" + query["value"] + "\n\nEnjoy!!"</ap>
        <rd field="ResultData">textXML</rd>
      </Properties>
    </node>
    <node type="Action" id="632897813125414083" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="315" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632897813125414084" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="216" y="32">
      <linkto id="632897813125414083" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">textXML.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Variable" id="632897813125414088" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">query</Properties>
    </node>
    <node type="Variable" id="632897813125414089" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632897813125414090" name="textXML" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">textXML</Properties>
    </node>
  </canvas>
  <Properties desc="Example 3 - IP Phone Applications Core Developemnt Concepts">
  </Properties>
</Application>