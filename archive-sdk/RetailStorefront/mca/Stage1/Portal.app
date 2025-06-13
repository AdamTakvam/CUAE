<Application name="Portal" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="Portal">
    <outline>
      <treenode type="evh" id="632875147412707541" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632875147412707538" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632875147412707537" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707595" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632875147412707592" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632875147412707591" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632875291003497486" actid="632875147412707601" />
          <ref id="632875291003497557" actid="632875291003497556" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707600" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632875147412707597" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632875147412707596" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632875291003497487" actid="632875147412707601" />
          <ref id="632875291003497558" actid="632875291003497556" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707610" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632875147412707607" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632875147412707606" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707887" level="2" text="Metreos.Providers.Http.GotRequest: StoreInventory">
        <node type="function" name="StoreInventory" id="632875147412707884" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875147412707883" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/StoreInventory</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707892" level="2" text="Metreos.Providers.Http.GotRequest: WebInventory">
        <node type="function" name="WebInventory" id="632875147412707889" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875147412707888" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/WebInventory</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707897" level="2" text="Metreos.Providers.Http.GotRequest: FloorManager">
        <node type="function" name="FloorManager" id="632875147412707894" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875147412707893" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/FloorManager</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707902" level="2" text="Metreos.Providers.Http.GotRequest: WarehouseManager">
        <node type="function" name="WarehouseManager" id="632875147412707899" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875147412707898" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/WarehouseManager</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707907" level="2" text="Metreos.Providers.Http.GotRequest: CheckIn">
        <node type="function" name="CheckIn" id="632875147412707904" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875147412707903" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/CheckIn</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707912" level="2" text="Metreos.Providers.Http.GotRequest: CheckOut">
        <node type="function" name="CheckOut" id="632875147412707909" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875147412707908" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/CheckOut</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875291003497565" level="2" text="Metreos.Providers.Http.GotRequest: StoreInventorySKU">
        <node type="function" name="StoreInventorySKU" id="632875291003497562" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875291003497561" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/StoreInventorySKU</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="632875291003497465" vid="632875147412707543">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632875291003497467" vid="632875147412707546">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632875291003497469" vid="632875147412707548">
        <Properties type="String">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_phoneIP" id="632875291003497471" vid="632875147412707550">
        <Properties type="String">g_phoneIP</Properties>
      </treenode>
      <treenode text="g_appServerIP" id="632875291003497473" vid="632875147412707552">
        <Properties type="String">g_appServerIP</Properties>
      </treenode>
      <treenode text="g_phoneUser" id="632875291003497475" vid="632875147412707558">
        <Properties type="String" initWith="PhoneUser">g_phoneUser</Properties>
      </treenode>
      <treenode text="g_phonePass" id="632875291003497477" vid="632875147412707560">
        <Properties type="String" initWith="PhonePass">g_phonePass</Properties>
      </treenode>
      <treenode text="g_operationId" id="632875291003497479" vid="632875147412707604">
        <Properties type="String">g_operationId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632875147412707540" treenode="632875147412707541" appnode="632875147412707538" handlerfor="632875147412707537">
    <node type="Start" id="632875147412707540" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="312">
      <linkto id="632875147412707545" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707545" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="232" y="312">
      <linkto id="632875147412707601" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="Conference" type="literal">true</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="ConnectionId">g_connectionId</rd>
        <rd field="ConferenceId">g_conferenceId</rd>
        <rd field="MediaTxIP">g_phoneIP</rd>
        <rd field="MediaRxIP">g_appServerIP</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707554" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="568" y="312">
      <linkto id="632875147412707557" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"http://" + g_appServerIP + ":8000/Retail/Menu?appId=" + routingGuid</ap>
        <rd field="ResultData">execute</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"http://" + g_appServerIP + ":8000/Retail/Menu?appId=" + routingGuid</log>
      </Properties>
    </node>
    <node type="Action" id="632875147412707557" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="720" y="312">
      <linkto id="632875147412707590" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">g_phoneIP</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="632875147412707590" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="848" y="312">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632875147412707601" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="328" y="296" mx="381" my="312">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632875147412707595" />
        <item text="OnPlay_Failed" treenode="632875147412707600" />
      </items>
      <linkto id="632875147412707554" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">Please make a selection</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="OperationId">g_operationId</rd>
      </Properties>
    </node>
    <node type="Variable" id="632875147412707542" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632875147412707555" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632875147412707556" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632875147412707594" treenode="632875147412707595" appnode="632875147412707592" handlerfor="632875147412707591">
    <node type="Start" id="632875147412707594" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="56" y="288">
      <linkto id="632875147412707615" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707615" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="288" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632875147412707599" treenode="632875147412707600" appnode="632875147412707597" handlerfor="632875147412707596">
    <node type="Start" id="632875147412707599" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="56" y="288">
      <linkto id="632875147412707616" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707616" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="240" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632875147412707609" treenode="632875147412707610" appnode="632875147412707607" handlerfor="632875147412707606">
    <node type="Start" id="632875147412707609" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="296">
      <linkto id="632875260826760121" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707611" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="696" y="296">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632875147412707612" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="400" y="208">
      <linkto id="632875147412707613" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">Key:Services</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707613" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="544" y="208">
      <linkto id="632875147412707611" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">g_phoneIP</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="632875260826760121" name="GetModeInfo" class="MaxActionNode" group="" path="Metreos.Native.PhoneActions" x="152" y="296">
      <linkto id="632875260826760123" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632875147412707611" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneIP" type="variable">g_phoneIP</ap>
        <ap name="PhoneUser" type="variable">g_phoneUser</ap>
        <ap name="PhonePass" type="variable">g_phonePass</ap>
        <rd field="Title">title</rd>
      </Properties>
    </node>
    <node type="Action" id="632875260826760123" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="264" y="248">
      <linkto id="632875147412707612" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632875147412707611" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">0 == String.Compare(title != null ? title.Trim() : "", "Retail Portal", true)</ap>
      </Properties>
    </node>
    <node type="Comment" id="632875260826760124" text="Clear off the screen if &#xD;&#xA;&quot;Retail Portal&quot; text is showing &#xD;&#xA;on the phone" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="272" y="64" />
    <node type="Variable" id="632875147412707614" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="632875260826760122" name="title" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">title</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="StoreInventory" activetab="true" startnode="632875147412707886" treenode="632875147412707887" appnode="632875147412707884" handlerfor="632875147412707883">
    <node type="Start" id="632875147412707886" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="72" y="288">
      <linkto id="632875291003497555" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707983" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="832" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632875291003497552" name="CreateInput" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="464" y="288">
      <linkto id="632875291003497559" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Prompt" type="literal">Enter SKU</ap>
        <ap name="URL" type="csharp">"http://" + g_appServerIP + ":8000/Retail/StoreInventorySKU?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632875291003497555" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="200" y="288">
      <linkto id="632875291003497556" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Block" type="literal">true</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="OperationId" type="variable">g_operationId</ap>
      </Properties>
    </node>
    <node type="Action" id="632875291003497556" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="288" y="272" mx="341" my="288">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632875147412707595" />
        <item text="OnPlay_Failed" treenode="632875147412707600" />
      </items>
      <linkto id="632875291003497552" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">Please enter the skew code to check from the store inventory</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="OperationId">g_operationId</rd>
      </Properties>
    </node>
    <node type="Action" id="632875291003497559" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="576" y="288">
      <linkto id="632875291003497560" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">SKU</ap>
        <ap name="QueryStringParam" type="literal">sku</ap>
        <ap name="InputFlags" type="literal">N</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632875291003497560" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="704" y="288">
      <linkto id="632875147412707983" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">input.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Variable" id="632875147412707913" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875147412707928" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632875291003497553" name="input" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Input" refType="reference">input</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="WebInventory" startnode="632875147412707891" treenode="632875147412707892" appnode="632875147412707889" handlerfor="632875147412707888">
    <node type="Start" id="632875147412707891" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="288">
      <linkto id="632875147412707929" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707929" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="184" y="288">
      <linkto id="632875147412707930" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Text" type="literal">Not yet implemented</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707930" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="344" y="288">
      <linkto id="632875147412707978" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Menu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">"http://" + g_appServerIP + ":8000/Retail/Menu?appId=" + routingGuid</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707978" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="472" y="288">
      <linkto id="632875147412707979" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875147412707979" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="592" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632875147412707914" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875147412707917" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632875147412707935" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="FloorManager" startnode="632875147412707896" treenode="632875147412707897" appnode="632875147412707894" handlerfor="632875147412707893">
    <node type="Start" id="632875147412707896" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="288">
      <linkto id="632875147412707936" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707936" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="184" y="288">
      <linkto id="632875147412707937" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Text" type="literal">Not yet implemented</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707937" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="344" y="288">
      <linkto id="632875147412707974" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Menu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">"http://" + g_appServerIP + ":8000/Retail/Menu?appId=" + routingGuid</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707974" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="472" y="288">
      <linkto id="632875147412707975" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875147412707975" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="592" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632875147412707915" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875147412707918" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632875147412707942" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="WarehouseManager" startnode="632875147412707901" treenode="632875147412707902" appnode="632875147412707899" handlerfor="632875147412707898">
    <node type="Start" id="632875147412707901" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="288">
      <linkto id="632875147412707943" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707943" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="184" y="288">
      <linkto id="632875147412707944" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Text" type="literal">Not yet implemented</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707944" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="344" y="288">
      <linkto id="632875147412707970" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Menu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">"http://" + g_appServerIP + ":8000/Retail/Menu?appId=" + routingGuid</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707970" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="464" y="288">
      <linkto id="632875147412707971" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875147412707971" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="584" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632875147412707919" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875147412707920" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632875147412707949" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="CheckIn" startnode="632875147412707906" treenode="632875147412707907" appnode="632875147412707904" handlerfor="632875147412707903">
    <node type="Start" id="632875147412707906" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="288">
      <linkto id="632875147412707950" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707950" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="176" y="288">
      <linkto id="632875147412707951" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Text" type="literal">Not yet implemented</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707951" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="336" y="288">
      <linkto id="632875147412707966" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Menu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">"http://" + g_appServerIP + ":8000/Retail/Menu?appId=" + routingGuid</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707966" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="456" y="288">
      <linkto id="632875147412707967" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875147412707967" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="576" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632875147412707921" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875147412707922" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632875147412707956" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="CheckOut" startnode="632875147412707911" treenode="632875147412707912" appnode="632875147412707909" handlerfor="632875147412707908">
    <node type="Start" id="632875147412707911" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="288">
      <linkto id="632875147412707957" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707957" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="192" y="288">
      <linkto id="632875147412707958" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Text" type="literal">Not yet implemented</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707958" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="352" y="288">
      <linkto id="632875147412707964" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Menu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">"http://" + g_appServerIP + ":8000/Retail/Menu?appId=" + routingGuid</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707964" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="488" y="288">
      <linkto id="632875147412707965" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875147412707965" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="608" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632875147412707923" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875147412707924" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632875147412707963" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="StoreInventorySKU" startnode="632875291003497564" treenode="632875291003497565" appnode="632875291003497562" handlerfor="632875291003497561">
    <node type="Start" id="632875291003497564" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="288">
      <linkto id="632875291003497569" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875291003497569" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="184" y="288">
      <linkto id="632875291003497570" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Text" type="literal">Not yet implemented</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875291003497570" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="344" y="288">
      <linkto id="632875291003497571" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Menu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">"http://" + g_appServerIP + ":8000/Retail/Menu?appId=" + routingGuid</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875291003497571" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="464" y="288">
      <linkto id="632875291003497572" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875291003497572" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="584" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632875291003497566" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632875291003497567" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875291003497568" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>