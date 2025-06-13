<Application name="Presence" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Presence">
    <outline>
      <treenode type="evh" id="633168247082500530" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="633168247082500527" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633168247082500526" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/presence</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="633168247082500570" level="2" text="Metreos.Providers.Presence.SubscriptionTerminated: OnSubscriptionTerminated">
        <node type="function" name="OnSubscriptionTerminated" id="633168247082500567" path="Metreos.StockTools" />
        <node type="event" name="SubscriptionTerminated" id="633168247082500566" path="Metreos.Providers.Presence.SubscriptionTerminated" />
        <references />
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="633168247082500575" level="2" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="633168247082500572" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="633168247082500571" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="633175108346250657" level="2" text="Metreos.Providers.Presence.Notify: OnNotify">
        <node type="function" name="OnNotify" id="633175108346250654" path="Metreos.StockTools" />
        <node type="event" name="Notify" id="633175108346250653" path="Metreos.Providers.Presence.Notify" />
        <references />
        <Properties type="hybrid">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_subscriber" id="633185530407500613" vid="633168247082500531">
        <Properties type="String" initWith="Config.subscriber">g_subscriber</Properties>
      </treenode>
      <treenode text="g_requestUri" id="633185530407500615" vid="633168247082500533">
        <Properties type="String" initWith="Config.requestUri">g_requestUri</Properties>
      </treenode>
      <treenode text="g_passwd" id="633185530407500617" vid="633168247082500535">
        <Properties type="String" initWith="Config.password">g_passwd</Properties>
      </treenode>
      <treenode text="g_action" id="633185530407500619" vid="633168247082500551">
        <Properties type="String" defaultInitWith="NotSet" initWith="Config.action">g_action</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="633168247082500529" treenode="633168247082500530" appnode="633168247082500527" handlerfor="633168247082500526">
    <node type="Start" id="633168247082500529" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="32">
      <linkto id="633168247082500553" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="633168247082500553" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="32" y="185">
      <linkto id="633168247082500554" type="Labeled" style="Bezier" label="TS" />
      <linkto id="633168247082500555" type="Labeled" style="Bezier" label="U" />
      <linkto id="633177751028595445" type="Labeled" style="Bezier" label="default" />
      <linkto id="633179379973750530" type="Labeled" style="Bezier" label="NTS" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">g_action</ap>
      </Properties>
    </node>
    <node type="Action" id="633168247082500554" name="TriggeringSubscribe" class="MaxActionNode" group="" path="Metreos.Providers.Presence" x="257" y="32">
      <linkto id="633168247082500565" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="password" type="variable">g_passwd</ap>
        <ap name="requestUri" type="variable">g_requestUri</ap>
        <ap name="subscriber" type="variable">g_subscriber</ap>
        <rd field="resultCode">actionStatus</rd>
      </Properties>
    </node>
    <node type="Action" id="633168247082500555" name="Unsubscribe" class="MaxActionNode" group="" path="Metreos.Providers.Presence" x="261" y="218">
      <linkto id="633168247082500565" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="subscriber" type="variable">g_subscriber</ap>
        <ap name="password" type="variable">g_passwd</ap>
        <ap name="requestUri" type="variable">g_requestUri</ap>
        <ap name="triggering" type="literal">true</ap>
        <rd field="resultCode">actionStatus</rd>
      </Properties>
    </node>
    <node type="Action" id="633168247082500565" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="476" y="163">
      <linkto id="633177751028595446" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"returnCode: " + actionStatus + " Action: " + g_action</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="633168247082500582" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="665.4707" y="50">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="633174329661875835" text="Run whichever action the user specified in the application where &#xD;&#xA;TS = TriggeringSubscribe, NTS = NonTriggeringSubscribe, and&#xD;&#xA;U = Unsubscribe.  Post the returnCode and action type to a web&#xD;&#xA;page specified in the triggering parameters of the application." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="303" y="303" />
    <node type="Action" id="633177751028595445" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="257" y="294">
      <linkto id="633168247082500565" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="literal">Invalid action type</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">Presence </ap>
      </Properties>
    </node>
    <node type="Action" id="633177751028595446" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="593.4707" y="163">
      <linkto id="633168247082500582" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"Action: " + g_action + " returnCode: " + actionStatus
</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">Presence </ap>
      </Properties>
    </node>
    <node type="Action" id="633179379973750530" name="NonTriggeringSubscribe" class="MaxActionNode" group="" path="Metreos.Providers.Presence" x="262" y="118">
      <linkto id="633168247082500565" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="password" type="variable">g_passwd</ap>
        <ap name="requestUri" type="variable">g_requestUri</ap>
        <ap name="subscriber" type="variable">g_subscriber</ap>
        <rd field="resultCode">actionStatus</rd>
      </Properties>
    </node>
    <node type="Variable" id="633168247082500563" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="633168318673594356" name="actionStatus" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="-1" refType="reference">actionStatus</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSubscriptionTerminated" startnode="633168247082500569" treenode="633168247082500570" appnode="633168247082500567" handlerfor="633168247082500566">
    <node type="Start" id="633168247082500569" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="127">
      <linkto id="633175108346250634" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633168247082500580" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="293" y="127">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="633174329661875836" text="If a SubscriptionTerminated event is received,&#xD;&#xA;log a message to appserver log and quit." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="33" y="189" />
    <node type="Action" id="633175108346250634" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="159" y="127">
      <linkto id="633168247082500580" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="literal">Subscription Terminated</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal"> PresenceScript1 </ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="633168247082500574" treenode="633168247082500575" appnode="633168247082500572" handlerfor="633168247082500571">
    <node type="Start" id="633168247082500574" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="116">
      <linkto id="633168247082500576" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633168247082500576" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="169" y="116">
      <linkto id="633185530407500568" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="literal">Session Expired</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">PresenceScript1 </ap>
      </Properties>
    </node>
    <node type="Comment" id="633174329661875837" text="If a SessionExpired event is received, log a &#xD;&#xA;message to appserver log and quit." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="41" y="165" />
    <node type="Action" id="633185530407500568" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="291" y="114">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnNotify" startnode="633175108346250656" treenode="633175108346250657" appnode="633175108346250654" handlerfor="633175108346250653">
    <node type="Start" id="633175108346250656" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="32">
      <linkto id="633175108346250658" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="633175108346250658" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="175" y="138">
      <linkto id="633175108346250659" type="Labeled" style="Bezier" label="default" />
      <Properties language="csharp">public static string Execute(Metreos.Types.Presence.PresenceNotification status, LogWriter log)
{
  if(status == null)
  {
      log.Write(TraceLevel.Warning, "Status is null");
      return IApp.VALUE_FAILURE;
  }

  if(status.ResourceList != null)
  {
    foreach(Resource r in status.ResourceList.Resources.Values)
    {
      if(r.Presence != null &amp;&amp; r.Presence.tuple != null &amp;&amp; r.Presence.tuple.Length &gt; 0 &amp;&amp;
         r.Presence.tuple[0] != null &amp;&amp; r.Presence.tuple[0].status != null)
      {
        log.Write(TraceLevel.Info, "Resource: {0} Status: {1}", r.Uri, 
          r.Presence.tuple[0].status.basic);
      }
      else if(r.Uri != null)
      {
        log.Write(TraceLevel.Warning, "Received Notify for {0} with no resource information", r.Uri);
      }
    }
  }
  else if(status.Resource != null)
  {
    Resource r = status.Resource;
    if(r.Presence != null &amp;&amp; r.Presence.tuple != null &amp;&amp; r.Presence.tuple.Length &gt; 0 &amp;&amp;
         r.Presence.tuple[0] != null &amp;&amp; r.Presence.tuple[0].status != null)

    {
      log.Write(TraceLevel.Info, "Resource: {0} Status: {1}", status.Resource.Uri, 
        r.Presence.tuple[0].status.basic);
    }
    else if(r.Uri != null)
    {
      log.Write(TraceLevel.Warning, "Received Notify for {0} with no resource information", r.Uri);
    }
  }
  else
  {
    log.Write(TraceLevel.Warning, "No presence information present in Notify message");
    return IApp.VALUE_FAILURE;
  }
  return IApp.VALUE_SUCCESS;
}



</Properties>
    </node>
    <node type="Action" id="633175108346250659" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="312" y="45.90886">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633175134512656901" name="status" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Presence.PresenceNotification" initWith="status" refType="reference" name="Metreos.Providers.Presence.Notify">status</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>