<Application name="Portal" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
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
          <ref id="632888319179552461" actid="632875147412707601" />
          <ref id="632888319179552490" actid="632875291003497556" />
          <ref id="632888319179552509" actid="632875304714864906" />
          <ref id="632888319179552522" actid="632875376037936230" />
          <ref id="632888319179552538" actid="632884805233125592" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707600" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632875147412707597" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632875147412707596" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632888319179552462" actid="632875147412707601" />
          <ref id="632888319179552491" actid="632875291003497556" />
          <ref id="632888319179552510" actid="632875304714864906" />
          <ref id="632888319179552523" actid="632875376037936230" />
          <ref id="632888319179552539" actid="632884805233125592" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707610" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632875147412707607" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632875147412707606" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632888319179552599" actid="632875376037937736" />
        </references>
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
      <treenode type="evh" id="632875147412707907" level="2" text="Metreos.Providers.Http.GotRequest: CheckIn">
        <node type="function" name="CheckIn" id="632875147412707904" path="Metreos.StockTools" />
        <calls>
          <ref actid="632875304714864938" />
        </calls>
        <node type="event" name="GotRequest" id="632875147412707903" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/CheckIn</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707912" level="2" text="Metreos.Providers.Http.GotRequest: CheckOut">
        <node type="function" name="CheckOut" id="632875147412707909" path="Metreos.StockTools" />
        <calls>
          <ref actid="632875376037936254" />
        </calls>
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
      <treenode type="evh" id="632875304714864922" level="2" text="Metreos.Providers.Http.GotRequest: CheckInSubmit">
        <node type="function" name="CheckInSubmit" id="632875304714864919" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875304714864918" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/CheckInSubmit</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875376037936249" level="2" text="Metreos.Providers.Http.GotRequest: CheckOutSubmit">
        <node type="function" name="CheckOutSubmit" id="632875376037936246" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875376037936245" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/CheckOutSubmit</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875376037937410" level="2" text="Metreos.Providers.Http.GotRequest: Call">
        <node type="function" name="Call" id="632875376037937407" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875376037937406" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/Call</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875376037937730" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632875376037937727" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632875376037937726" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632888319179552597" actid="632875376037937736" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632875376037937735" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632875376037937732" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632875376037937731" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632888319179552598" actid="632875376037937736" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="fun" id="632875376037936544" level="1" text="UpdateImage">
        <node type="function" name="UpdateImage" id="632875376037936541" path="Metreos.StockTools" />
        <calls>
          <ref actid="632875376037936540" />
          <ref actid="632875376037936565" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="632888319179552440" vid="632875147412707543">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632888319179552442" vid="632875147412707546">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632888319179552444" vid="632875147412707548">
        <Properties type="String">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_phoneIP" id="632888319179552446" vid="632875147412707550">
        <Properties type="String">g_phoneIP</Properties>
      </treenode>
      <treenode text="g_appServerIP" id="632888319179552448" vid="632875147412707552">
        <Properties type="String">g_appServerIP</Properties>
      </treenode>
      <treenode text="g_phoneUser" id="632888319179552450" vid="632875147412707558">
        <Properties type="String" initWith="PhoneUser">g_phoneUser</Properties>
      </treenode>
      <treenode text="g_phonePass" id="632888319179552452" vid="632875147412707560">
        <Properties type="String" initWith="PhonePass">g_phonePass</Properties>
      </treenode>
      <treenode text="g_operationId" id="632888319179552454" vid="632875147412707604">
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
      <linkto id="632875304714864926" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">g_phoneIP</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
        <log condition="entry" on="true" level="Info" type="literal">"Sent command to phone IP:  " + g_phoneIP</log>
      </Properties>
    </node>
    <node type="Action" id="632875147412707590" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1040" y="312">
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
    <node type="Action" id="632875304714864926" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="848" y="312">
      <linkto id="632875304714864929" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="literal">RetailDemo</ap>
        <ap name="Server" type="literal">127.0.0.1</ap>
        <ap name="Port" type="literal">3306</ap>
        <ap name="Username" type="literal">root</ap>
        <ap name="Password" type="literal">metreos</ap>
        <ap name="Pooling" type="literal">true</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632875304714864929" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="952" y="312">
      <linkto id="632875147412707590" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">RetailDemo</ap>
        <ap name="Type" type="literal">mysql</ap>
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
    <node type="Variable" id="632875304714864928" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
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
      <linkto id="632876838082819330" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707611" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="696" y="296">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632875147412707612" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="448" y="120">
      <linkto id="632875147412707613" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">Key:Services</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707613" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="696" y="120">
      <linkto id="632875147412707611" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">g_phoneIP</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="632875260826760121" name="GetModeInfo" class="MaxActionNode" group="" path="Metreos.Native.PhoneActions" x="280" y="296">
      <linkto id="632875260826760123" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632875147412707611" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneIP" type="variable">g_phoneIP</ap>
        <ap name="PhoneUser" type="variable">g_phoneUser</ap>
        <ap name="PhonePass" type="variable">g_phonePass</ap>
        <rd field="Title">title</rd>
      </Properties>
    </node>
    <node type="Action" id="632875260826760123" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="280" y="120">
      <linkto id="632875147412707612" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632875147412707611" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">0 == String.Compare(title != null ? title.Trim() : "", "Retail Portal", true)</ap>
      </Properties>
    </node>
    <node type="Comment" id="632875260826760124" text="Clear off the screen if &#xD;&#xA;&quot;Retail Portal&quot; text is showing &#xD;&#xA;on the phone" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="120" y="48" />
    <node type="Action" id="632876838082819330" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="144" y="296">
      <linkto id="632876838082819331" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632875260826760121" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">callId == g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632876838082819331" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="144" y="400">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632875147412707614" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="632875260826760122" name="title" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">title</Properties>
    </node>
    <node type="Variable" id="632875376037937743" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="StoreInventory" startnode="632875147412707886" treenode="632875147412707887" appnode="632875147412707884" handlerfor="632875147412707883">
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
  <canvas type="Function" name="CheckIn" startnode="632875147412707906" treenode="632875147412707907" appnode="632875147412707904" handlerfor="632875147412707903">
    <node type="Start" id="632875147412707906" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="288">
      <linkto id="632875304714864905" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875304714864904" name="CreateInput" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="456" y="288">
      <linkto id="632875304714864907" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Prompt" type="literal">Check In</ap>
        <ap name="URL" type="csharp">"http://" + g_appServerIP + ":8000/Retail/CheckInSubmit?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632875304714864905" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="192" y="288">
      <linkto id="632875304714864906" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Block" type="literal">true</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="OperationId" type="variable">g_operationId</ap>
      </Properties>
    </node>
    <node type="Action" id="632875304714864906" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="280" y="272" mx="333" my="288">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632875147412707595" />
        <item text="OnPlay_Failed" treenode="632875147412707600" />
      </items>
      <linkto id="632875304714864904" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">Checking in</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="OperationId">g_operationId</rd>
      </Properties>
    </node>
    <node type="Action" id="632875304714864907" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="568" y="288">
      <linkto id="632875304714864932" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">UserID</ap>
        <ap name="QueryStringParam" type="literal">userId</ap>
        <ap name="InputFlags" type="literal">N</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632875304714864908" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="760" y="288">
      <linkto id="632875304714864917" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">input.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875304714864917" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="872" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632875304714864932" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="656" y="288">
      <linkto id="632875304714864908" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">Ext</ap>
        <ap name="QueryStringParam" type="literal">ext</ap>
        <ap name="InputFlags" type="literal">N</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Variable" id="632875147412707921" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875147412707956" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632875304714864916" name="input" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Input" refType="reference">input</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="CheckOut" startnode="632875147412707911" treenode="632875147412707912" appnode="632875147412707909" handlerfor="632875147412707908">
    <node type="Start" id="632875147412707911" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="288">
      <linkto id="632875376037936229" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875376037936228" name="CreateInput" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="432" y="288">
      <linkto id="632875376037936231" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Prompt" type="literal">Check Out</ap>
        <ap name="URL" type="csharp">"http://" + g_appServerIP + ":8000/Retail/CheckOutSubmit?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936229" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="184" y="288">
      <linkto id="632875376037936230" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Block" type="literal">true</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="OperationId" type="variable">g_operationId</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936230" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="272" y="272" mx="325" my="288">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632875147412707595" />
        <item text="OnPlay_Failed" treenode="632875147412707600" />
      </items>
      <linkto id="632875376037936228" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">Checking out</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="OperationId">g_operationId</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936231" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="544" y="288">
      <linkto id="632875376037936232" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">UserID</ap>
        <ap name="QueryStringParam" type="literal">userId</ap>
        <ap name="InputFlags" type="literal">N</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936232" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="664" y="288">
      <linkto id="632875376037936233" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">input.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936233" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="776" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632875147412707923" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875147412707963" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632875376037936244" name="input" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Input" refType="reference">input</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="StoreInventorySKU" activetab="true" startnode="632875291003497564" treenode="632875291003497565" appnode="632875291003497562" handlerfor="632875291003497561">
    <node type="Start" id="632875291003497564" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34.166687" y="272.625">
      <linkto id="632883742306501463" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632883382612676454" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="984" y="271">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632883742306501463" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="184" y="272">
      <linkto id="632883742306501473" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632884701052635515" type="Labeled" style="Bezier" ortho="true" label="a" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Prompt" type="literal">Make a selection</ap>
        <ap name="Text" type="csharp">"SKU: " + query["sku"] + "\nName:  Hot New Product\nIn Store: 0\nIn Transit:4 (Sept-12-06)\nSubject Expert: John Doe"</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632883742306501471" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="824" y="273">
      <linkto id="632883382612676454" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632883742306501473" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="658" y="272">
      <linkto id="632883742306501471" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Menu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">"http://" + g_appServerIP + ":8000/Retail/Menu?appId=" + routingGuid</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632883742306501475" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="528" y="415">
      <linkto id="632883742306501473" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Dial</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="csharp">"http://" + g_appServerIP + ":8000/Retail/Call?ext=" + dial + "&amp;metreosSessionId=" + routingGuid + "&amp;sku=" + query["sku"]</ap>
        <rd field="ResultData">text</rd>
        <log condition="entry" on="true" level="Error" type="csharp">"http://" + g_appServerIP + ":8000/Retail/Call?ext=" + dial + "&amp;metreosSessionId=" + routingGuid + "&amp;sku=" + query["sku"]</log>
      </Properties>
    </node>
    <node type="Action" id="632884701052635515" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="256" y="416">
      <linkto id="632884805233125592" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">100004</ap>
        <ap name="Value2" type="literal">Text to speech...</ap>
        <rd field="ResultData">dial</rd>
        <rd field="ResultData2">speak</rd>
      </Properties>
    </node>
    <node type="Action" id="632884805233125592" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="320" y="400" mx="373" my="416">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632875147412707595" />
        <item text="OnPlay_Failed" treenode="632875147412707600" />
      </items>
      <linkto id="632883742306501475" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="csharp">query["noplay"] == null ? speak : "."</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
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
    <node type="Variable" id="632883742306501464" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632884701052635514" name="dial" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dial</Properties>
    </node>
    <node type="Variable" id="632884805233125595" name="speak" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">speak</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="CheckInSubmit" startnode="632875304714864921" treenode="632875304714864922" appnode="632875304714864919" handlerfor="632875304714864918">
    <node type="Start" id="632875304714864921" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="56" y="288">
      <linkto id="632875304714864937" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632875304714864924" text="Find user in user database" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="152" y="152" />
    <node type="Action" id="632875304714864930" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="304" y="288">
      <linkto id="632875304714864935" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632875304714864938" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">"SELECT username FROM Users WHERE UserID = '" + query["userId"] + "'" </ap>
        <ap name="Name" type="literal">RetailDemo</ap>
        <rd field="ResultSet">checkUserResults</rd>
      </Properties>
    </node>
    <node type="Action" id="632875304714864935" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="472" y="288">
      <linkto id="632875304714864936" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632875304714864938" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">checkUserResults.Rows.Count != 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632875304714864936" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="608" y="288">
      <linkto id="632875376037936221" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Text" type="csharp">"Logging in at " + query["ext"]</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875304714864937" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="168" y="288">
      <linkto id="632875304714864938" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632875304714864930" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">query["ext"] == String.Empty || query["userId"] == String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632875304714864938" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="136" y="392" mx="173" my="408">
      <items count="1">
        <item text="CheckIn" treenode="632875147412707907" />
      </items>
      <linkto id="632875304714864940" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="RoutingGuid" type="variable">routingGuid</ap>
        <ap name="FunctionName" type="literal">CheckIn</ap>
      </Properties>
    </node>
    <node type="Action" id="632875304714864940" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="168" y="544">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632875304714864941" text="If user submits empty parameters,&#xD;&#xA;the check user query fails, or&#xD;&#xA;if there is no userId of the entered ID,&#xD;&#xA;re-prompt with CheckIn" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="209" y="410" />
    <node type="Action" id="632875376037936221" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="752" y="288">
      <linkto id="632875376037936222" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936222" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="848" y="288">
      <linkto id="632875376037936227" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">1000</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936223" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1224" y="288">
      <linkto id="632875376037936224" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"http://" + g_appServerIP + ":8000/Retail/Menu?appId=" + routingGuid</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936224" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1344" y="288">
      <linkto id="632875376037936226" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">g_phoneIP</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936226" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1448" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632875376037936227" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="976" y="288">
      <linkto id="632875376037936540" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">"UPDATE Users SET extension = '" + query["ext"] + "', checkedIn = 1 WHERE userId = '" + query["userId"] + "'"</ap>
        <ap name="Name" type="literal">RetailDemo</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936540" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1072" y="272" mx="1112" my="288">
      <items count="1">
        <item text="UpdateImage" />
      </items>
      <linkto id="632875376037936223" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">UpdateImage</ap>
      </Properties>
    </node>
    <node type="Variable" id="632875304714864923" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875304714864931" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">query</Properties>
    </node>
    <node type="Variable" id="632875304714864934" name="checkUserResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkUserResults</Properties>
    </node>
    <node type="Variable" id="632875304714864939" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632875376037936220" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632875376037936225" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="CheckOutSubmit" startnode="632875376037936248" treenode="632875376037936249" appnode="632875376037936246" handlerfor="632875376037936245">
    <node type="Start" id="632875376037936248" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="296">
      <linkto id="632875376037936253" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875376037936250" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="296" y="296">
      <linkto id="632875376037936251" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632875376037936254" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">"SELECT username FROM Users WHERE UserID = '" + query["userId"] + "'" </ap>
        <ap name="Name" type="literal">RetailDemo</ap>
        <rd field="ResultSet">checkUserResults</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936251" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="464" y="296">
      <linkto id="632875376037936252" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632875376037936254" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">checkUserResults.Rows.Count != 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936252" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="600" y="296">
      <linkto id="632875376037936257" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Text" type="csharp">"Logging out " + checkUserResults.Rows[0][0] </ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936253" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="160" y="296">
      <linkto id="632875376037936254" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632875376037936250" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">query["userId"] == String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936254" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="128" y="400" mx="165" my="416">
      <items count="1">
        <item text="CheckOut" treenode="632875147412707912" />
      </items>
      <linkto id="632875376037936255" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="RoutingGuid" type="variable">routingGuid</ap>
        <ap name="FunctionName" type="literal">CheckOut</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936255" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="160" y="552">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632875376037936256" text="If user submits empty parameters,&#xD;&#xA;the check user query fails, or&#xD;&#xA;if there is no userId of the entered ID,&#xD;&#xA;re-prompt with CheckIn" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="208" y="416" />
    <node type="Action" id="632875376037936257" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="744" y="296">
      <linkto id="632875376037936258" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936258" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="840" y="296">
      <linkto id="632875376037936262" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">1000</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936259" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1168" y="296">
      <linkto id="632875376037936260" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"http://" + g_appServerIP + ":8000/Retail/Menu?appId=" + routingGuid</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936260" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1288" y="296">
      <linkto id="632875376037936261" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">g_phoneIP</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936261" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1392" y="296">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632875376037936262" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="952" y="296">
      <linkto id="632875376037936565" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">"UPDATE Users SET checkedIn = 0 WHERE userId = '" + query["userId"] + "'"</ap>
        <ap name="Name" type="literal">RetailDemo</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936565" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1032" y="280" mx="1072" my="296">
      <items count="1">
        <item text="UpdateImage" />
      </items>
      <linkto id="632875376037936259" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">UpdateImage</ap>
      </Properties>
    </node>
    <node type="Variable" id="632875376037936276" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875376037936277" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632875376037936278" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632875376037936279" name="checkUserResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkUserResults</Properties>
    </node>
    <node type="Variable" id="632875376037936280" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="632875376037936281" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Call" startnode="632875376037937409" treenode="632875376037937410" appnode="632875376037937407" handlerfor="632875376037937406">
    <node type="Start" id="632875376037937409" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="96" y="296">
      <linkto id="632875376037937736" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875376037937416" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="408" y="296">
      <linkto id="632875376037937417" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Text" type="csharp">"Calling " + query["ext"]</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037937417" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="544" y="297">
      <linkto id="632875376037937418" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037937418" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="694" y="298">
      <linkto id="632875376037937419" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">1000</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037937419" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="838" y="298">
      <linkto id="632875376037937422" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">query["sku"] != null ? "http://" + g_appServerIP + ":8000/Retail/StoreInventorySKU?metreosSessionId=" + routingGuid + "&amp;noplay=1&amp;sku=" + query["sku"] : "http://" + g_appServerIP + ":8000/Retail/Menu?appId=" + routingGuid</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037937422" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="958" y="298">
      <linkto id="632875376037937740" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">g_phoneIP</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037937736" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="168" y="280" mx="234" my="296">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632875376037937730" />
        <item text="OnMakeCall_Failed" treenode="632875376037937735" />
        <item text="OnRemoteHangup" treenode="632875147412707610" />
      </items>
      <linkto id="632875376037937416" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="csharp">query["ext"]</ap>
        <ap name="From" type="literal">Retail Portal</ap>
        <ap name="DisplayName" type="literal">Retail Portal</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037937740" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1102" y="298">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632875376037937411" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632875376037937412" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875376037937414" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632875376037937415" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="632875376037937725" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632875376037937729" treenode="632875376037937730" appnode="632875376037937727" handlerfor="632875376037937726">
    <node type="Start" id="632875376037937729" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="88" y="344">
      <linkto id="632875376037937741" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875376037937741" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="328" y="344">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632875376037937734" treenode="632875376037937735" appnode="632875376037937732" handlerfor="632875376037937731">
    <node type="Start" id="632875376037937734" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="72" y="360">
      <linkto id="632875376037937742" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875376037937742" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="216" y="360">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="UpdateImage" startnode="632875376037936543" treenode="632875376037936544" appnode="632875376037936541" handlerfor="632875376037937731">
    <node type="Loop" id="632875376037936559" name="Loop" text="loop (expr)" cx="545" cy="191" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="648" y="56" mx="920" my="152">
      <linkto id="632875376037936558" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632875376037936554" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">checkLoggedIn.Rows</Properties>
    </node>
    <node type="Start" id="632875376037936543" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="304">
      <linkto id="632875376037936545" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875376037936545" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="120" y="304">
      <linkto id="632875376037936550" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="literal">SELECT username, extension, userId from users where checkedIn = 1</ap>
        <ap name="Name" type="literal">RetailDemo</ap>
        <rd field="ResultSet">checkLoggedIn</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936548" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="584" y="304">
      <linkto id="632875376037936559" port="1" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632875376037936554" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">checkLoggedIn.Rows.Count &gt; 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936550" name="CreateImageBuilder" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="256" y="304">
      <linkto id="632875376037936552" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <rd field="ImageBuilder">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936552" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="424" y="304">
      <linkto id="632875376037936548" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="literal">c:\images\background2.png</ap>
        <ap name="Top" type="literal">0</ap>
        <ap name="Left" type="literal">0</ap>
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <rd field="Image">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936554" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1336" y="304">
      <linkto id="632875376037936563" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Imaging.ImageBuilder image)
{

	image.Save("c:\\metreos\\mceadmin\\public\\menu.png");
	return "";
}
</Properties>
    </node>
    <node type="Action" id="632875376037936558" name="AddStandardImageRegion" container="632875376037936559" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="752" y="152">
      <linkto id="632875376037936561" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="literal">c:\images\button2.png</ap>
        <ap name="Top" type="literal">99</ap>
        <ap name="Left" type="csharp">1 + count * 68 + count * 8</ap>
        <ap name="Width" type="literal">68</ap>
        <ap name="Height" type="literal">68</ap>
        <rd field="Image">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936560" name="Assign" container="632875376037936559" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1120" y="152">
      <linkto id="632875376037936559" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">count + 1</ap>
        <rd field="ResultData">count</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936561" name="AddTextRegion" container="632875376037936559" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="896" y="152">
      <linkto id="632875376037936562" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Text" type="csharp">((loopEnum.Current as DataRow)["username"] as string).Split()[0]</ap>
        <ap name="Font" type="literal">Arial</ap>
        <ap name="FontSize" type="literal">14</ap>
        <ap name="Top" type="csharp">99 + 20</ap>
        <ap name="Left" type="csharp">1 + count * 68 + count * 8 + 3</ap>
        <ap name="Color" type="csharp">System.Drawing.Color.Black</ap>
        <rd field="Image">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936562" name="AddTextRegion" container="632875376037936559" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="1008" y="152">
      <linkto id="632875376037936560" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Text" type="csharp">((loopEnum.Current as DataRow)["username"] as string).Split()[1]</ap>
        <ap name="Font" type="literal">Arial</ap>
        <ap name="FontSize" type="literal">14</ap>
        <ap name="Top" type="csharp">99 + 40</ap>
        <ap name="Left" type="csharp">1 + count * 68 + count * 8 + 3</ap>
        <ap name="Color" type="csharp">System.Drawing.Color.Black</ap>
        <rd field="Image">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936563" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1440" y="304">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632875376037936546" name="checkLoggedIn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkLoggedIn</Properties>
    </node>
    <node type="Variable" id="632875376037936549" name="image" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Imaging.ImageBuilder" refType="reference">image</Properties>
    </node>
    <node type="Variable" id="632875376037936557" name="count" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" defaultInitWith="0" refType="reference">count</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>