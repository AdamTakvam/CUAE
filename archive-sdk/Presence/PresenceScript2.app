<Application name="PresenceScript2" trigger="Metreos.Providers.Presence.Notify" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="PresenceScript2">
    <outline>
      <treenode type="evh" id="633177751028594588" level="1" text="Metreos.Providers.Presence.Notify (trigger): OnNotify">
        <node type="function" name="OnNotify" id="633177751028594585" path="Metreos.StockTools" />
        <node type="event" name="Notify" id="633177751028594584" path="Metreos.Providers.Presence.Notify" trigger="true" />
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="633177751028594595" level="2" text="Metreos.Providers.Presence.SubscriptionTerminated: OnSubscriptionTerminated">
        <node type="function" name="OnSubscriptionTerminated" id="633177751028594592" path="Metreos.StockTools" />
        <node type="event" name="SubscriptionTerminated" id="633177751028594591" path="Metreos.Providers.Presence.SubscriptionTerminated" />
        <references />
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="633177751028594925" level="2" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="633177751028594922" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="633177751028594921" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnNotify" startnode="633177751028594587" treenode="633177751028594588" appnode="633177751028594585" handlerfor="633177751028594584">
    <node type="Start" id="633177751028594587" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="75">
      <linkto id="633177751028594663" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="633177751028594663" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="175" y="141.543427">
      <linkto id="633177751028594664" type="Labeled" style="Bezier" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="false" level="Info" type="csharp">"Status = " + statusStr</log>
        <log condition="default" on="false" level="Info" type="variable">Presence status: </log>public static string Execute(Metreos.Types.Presence.PresenceNotification status, LogWriter log)
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
    <node type="Action" id="633177751028594664" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="333" y="107.543427">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633177751028594671" name="status" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Presence.PresenceNotification" initWith="status" refType="reference" name="Metreos.Providers.Presence.Notify">status</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSubscriptionTerminated" startnode="633177751028594594" treenode="633177751028594595" appnode="633177751028594592" handlerfor="633177751028594591">
    <node type="Start" id="633177751028594594" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="89">
      <linkto id="633177751028594673" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="633177751028594672" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="243" y="90">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633177751028594673" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="137" y="157">
      <linkto id="633177751028594672" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="literal">Subscription Terminated </ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">PresenceScript2 </ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" activetab="true" startnode="633177751028594924" treenode="633177751028594925" appnode="633177751028594922" handlerfor="633177751028594921">
    <node type="Start" id="633177751028594924" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="101">
      <linkto id="633177751028594927" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="633177751028594927" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="149" y="149">
      <linkto id="633185530407500593" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="literal">Session Expired </ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">PresenceScript2 </ap>
      </Properties>
    </node>
    <node type="Action" id="633185530407500593" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="293" y="93">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>