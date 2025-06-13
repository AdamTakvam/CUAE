<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="633128803436875464" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="633128803436875461" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633128803436875460" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ts</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="633154304678594400" level="2" text="Metreos.Providers.Presence.SubscriptionTerminated: OnSubscriptionTerminated">
        <node type="function" name="OnSubscriptionTerminated" id="633154304678594397" path="Metreos.StockTools" />
        <node type="event" name="SubscriptionTerminated" id="633154304678594396" path="Metreos.Providers.Presence.SubscriptionTerminated" />
        <references />
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="633154304678595180" level="2" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="633154304678595177" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="633154304678595176" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_subscriber" id="633185530407501497" vid="633128803436875474">
        <Properties type="String" initWith="Config.subscriber">g_subscriber</Properties>
      </treenode>
      <treenode text="g_passwd" id="633185530407501499" vid="633128803436875495">
        <Properties type="String" initWith="Config.password">g_passwd</Properties>
      </treenode>
      <treenode text="g_requestUri" id="633185530407501501" vid="633128803436875497">
        <Properties type="String" initWith="Config.requestUri">g_requestUri</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="633128803436875463" treenode="633128803436875464" appnode="633128803436875461" handlerfor="633128803436875460">
    <node type="Start" id="633128803436875463" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="39">
      <linkto id="633169206872969369" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633128803436875472" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="379" y="148">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633167467859531873" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="238" y="148">
      <linkto id="633128803436875472" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"Action: TriggeringSubscribe returnCode: " + actionStatus</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="633169206872969369" name="TriggeringSubscribe" class="MaxActionNode" group="" path="Metreos.Providers.Presence" x="102" y="149">
      <linkto id="633167467859531873" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="password" type="variable">g_passwd</ap>
        <ap name="requestUri" type="variable">g_requestUri</ap>
        <ap name="subscriber" type="variable">g_subscriber</ap>
        <rd field="resultCode">actionStatus</rd>
      </Properties>
    </node>
    <node type="Variable" id="633154304678594707" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="633175142028750618" name="actionStatus" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Int" defaultInitWith="-1" refType="reference">actionStatus</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSubscriptionTerminated" startnode="633154304678594399" treenode="633154304678594400" appnode="633154304678594397" handlerfor="633154304678594396">
    <node type="Start" id="633154304678594399" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633154304678595181" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633154304678594402" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="268" y="34">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633154304678595181" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="137" y="79">
      <linkto id="633154304678594402" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="literal">Subscription Terminated</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">PresenceTriggeringSubscribe: </ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="633154304678595179" treenode="633154304678595180" appnode="633154304678595177" handlerfor="633154304678595176">
    <node type="Start" id="633154304678595179" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633185530407500817" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633185530407500817" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="211" y="38">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>