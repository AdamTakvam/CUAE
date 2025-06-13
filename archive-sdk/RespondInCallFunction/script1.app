<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632539350297424347" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632539350297424344" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632539350297424343" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/respondincallfunction</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632539356975544609" level="2" text="Metreos.Providers.Http.GotRequest: OnGotRequest1">
        <node type="function" name="OnGotRequest1" id="632539356975544606" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632539356975544605" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/second</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632539356975544625" level="2" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="632539356975544622" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="632539356975544621" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632539350297424356" level="1" text="Respond">
        <node type="function" name="Respond" id="632539350297424353" path="Metreos.StockTools" />
        <calls>
          <ref actid="632539350297424352" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_remoteHost" id="632539356975544592" vid="632539350297424349">
        <Properties type="String">g_remoteHost</Properties>
      </treenode>
      <treenode text="g_host" id="632539356975544604" vid="632539356975544603">
        <Properties type="String">g_host</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="632539356975544613" vid="632539356975544612">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632539350297424346" treenode="632539350297424347" appnode="632539350297424344" handlerfor="632539350297424343">
    <node type="Start" id="632539350297424346" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="352">
      <linkto id="632539350297424351" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632539350297424351" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="174" y="351">
      <linkto id="632539350297424352" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">remoteHost</ap>
        <ap name="Value2" type="variable">host</ap>
        <ap name="Value3" type="variable">routingGuid</ap>
        <rd field="ResultData">g_remoteHost</rd>
        <rd field="ResultData2">g_host</rd>
        <rd field="ResultData3">g_routingGuid</rd>
      </Properties>
    </node>
    <node type="Action" id="632539350297424352" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="330.825836" y="333" mx="368" my="349">
      <items count="1">
        <item text="Respond" />
      </items>
      <linkto id="632539356975544619" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Respond</ap>
      </Properties>
    </node>
    <node type="Action" id="632539356975544619" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="514" y="338">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632539350297424348" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632539356975544602" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632539356975544614" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Respond" startnode="632539350297424355" treenode="632539350297424356" appnode="632539350297424353" handlerfor="632539350297424343">
    <node type="Start" id="632539350297424355" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="89" y="270">
      <linkto id="632539350297424359" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632539350297424359" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="369" y="269">
      <linkto id="632539350297424360" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">g_remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">"&lt;html&gt;&lt;body&gt;&lt;p&gt;&lt;a href=\"http://" + g_host + ":8000/second?metreosSessionId=" + g_routingGuid + "\"&gt;Ping&lt;/a&gt;&lt;/p&gt;&lt;/body&gt;&lt;/html&gt;"</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632539350297424360" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="511" y="269">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotRequest1" activetab="true" startnode="632539356975544608" treenode="632539356975544609" appnode="632539356975544606" handlerfor="632539356975544605">
    <node type="Start" id="632539356975544608" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="275">
      <linkto id="632539356975544615" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632539356975544615" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="322.589172" y="272">
      <linkto id="632539356975544620" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">"&lt;html&gt;&lt;body&gt;&lt;p&gt;&lt;a href=\"http://" + g_host + ":8000/second?metreosSessionId=" + g_routingGuid + "\"&gt;Ping&lt;/a&gt;&lt;/p&gt;&lt;/body&gt;&lt;/html&gt;"</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632539356975544620" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="512" y="277">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632539356975544618" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="632539356975544624" treenode="632539356975544625" appnode="632539356975544622" handlerfor="632539356975544621">
    <node type="Start" id="632539356975544624" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632539356975544626" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632539356975544626" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="491" y="321">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>