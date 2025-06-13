<Application name="PresenceUnsubscribe" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="PresenceUnsubscribe">
    <outline>
      <treenode type="evh" id="633167467859531887" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="633167467859531884" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633167467859531883" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="633167467859531899" level="2" text="Metreos.Providers.Presence.SubscriptionTerminated: OnSubscriptionTerminated">
        <node type="function" name="OnSubscriptionTerminated" id="633167467859531896" path="Metreos.StockTools" />
        <node type="event" name="SubscriptionTerminated" id="633167467859531895" path="Metreos.Providers.Presence.SubscriptionTerminated" />
        <references />
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="633175142028750711" level="2" text="Metreos.Providers.Presence.Notify: OnNotify">
        <node type="function" name="OnNotify" id="633175142028750708" path="Metreos.StockTools" />
        <node type="event" name="Notify" id="633175142028750707" path="Metreos.Providers.Presence.Notify" />
        <references />
        <Properties type="hybrid">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_subscriber" id="633175142028750852" vid="633167467859531889">
        <Properties type="String" initWith="Config.subscriber">g_subscriber</Properties>
      </treenode>
      <treenode text="g_requestUri" id="633175142028750854" vid="633167467859531891">
        <Properties type="String" initWith="Config.requestUri">g_requestUri</Properties>
      </treenode>
      <treenode text="g_passwd" id="633175142028750856" vid="633167467859531893">
        <Properties type="String" initWith="Config.password">g_passwd</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="633167467859531886" treenode="633167467859531887" appnode="633167467859531884" handlerfor="633167467859531883">
    <node type="Start" id="633167467859531886" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633167467859531888" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633167467859531888" name="Unsubscribe" class="MaxActionNode" group="" path="Metreos.Providers.Presence" x="134" y="89">
      <linkto id="633167467859531900" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="requestUri" type="variable">g_requestUri</ap>
        <ap name="subscriber" type="variable">g_subscriber</ap>
        <ap name="password" type="variable">g_passwd</ap>
        <rd field="resultCode">actionStatus</rd>
      </Properties>
    </node>
    <node type="Action" id="633167467859531901" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="407" y="249">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633167467859531900" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="282" y="176">
      <linkto id="633167467859531901" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"Action: Unsubscribe returnCode: " + actionStatus
</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Variable" id="633167467859531902" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="633175142028750706" name="actionStatus" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Int" defaultInitWith="-1" refType="reference">actionStatus</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSubscriptionTerminated" startnode="633167467859531898" treenode="633167467859531899" appnode="633167467859531896" handlerfor="633167467859531895">
    <node type="Start" id="633167467859531898" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633175142028750704" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633167467859531912" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="245" y="46">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633175142028750704" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="135" y="100">
      <linkto id="633167467859531912" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="literal">Subscription Terminated</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">PresenseUnsubscribe: </ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnNotify" startnode="633175142028750710" treenode="633175142028750711" appnode="633175142028750708" handlerfor="633175142028750707">
    <node type="Start" id="633175142028750710" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633175142028750712" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633175142028750712" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="171" y="103">
      <linkto id="633175142028750713" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="literal">Notify received</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">PresenceUnsubscribe: </ap>
      </Properties>
    </node>
    <node type="Action" id="633175142028750713" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="321" y="53">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>