<Application name="script2" trigger="Metreos.Providers.Presence.Notify" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script2">
    <outline>
      <treenode type="evh" id="633177702497813180" level="1" text="Metreos.Providers.Presence.Notify (trigger): OnNotify">
        <node type="function" name="OnNotify" id="633177702497813177" path="Metreos.StockTools" />
        <node type="event" name="Notify" id="633177702497813176" path="Metreos.Providers.Presence.Notify" trigger="true" />
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="633177702497813767" level="2" text="Metreos.Providers.Presence.SubscriptionTerminated: OnSubscriptionTerminated">
        <node type="function" name="OnSubscriptionTerminated" id="633177702497813764" path="Metreos.StockTools" />
        <node type="event" name="SubscriptionTerminated" id="633177702497813763" path="Metreos.Providers.Presence.SubscriptionTerminated" />
        <references />
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="633177702497813772" level="2" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="633177702497813769" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="633177702497813768" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnNotify" activetab="true" startnode="633177702497813179" treenode="633177702497813180" appnode="633177702497813177" handlerfor="633177702497813176">
    <node type="Start" id="633177702497813179" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="34">
      <linkto id="633177702497813283" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633177702497813283" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="163" y="121">
      <linkto id="633177702497813518" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">public static string Execute(Metreos.Types.Presence.PresenceNotification status, LogWriter log)

{ 
  if(status == null || status.ResourceList == null) 
  { 
      log.Write(TraceLevel.Warning, "No presence information present in Notify message"); 
      return IApp.VALUE_FAILURE; 
  } 
 
  foreach(Metreos.Types.Presence.Resource r in status.ResourceList.Resources.Values) 
  { 
    log.Write(TraceLevel.Info, "Resource: {0} Status: {1}", r.Uri,  
     r.Presence.tuple[0].status.basic); 
  } 
 
  return IApp.VALUE_SUCCESS; 
}
</Properties>
    </node>
    <node type="Action" id="633177702497813518" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="320" y="70">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633177702497813346" name="status" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Presence.PresenceNotification" initWith="status" refType="reference" name="Metreos.Providers.Presence.Notify">status</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSubscriptionTerminated" startnode="633177702497813766" treenode="633177702497813767" appnode="633177702497813764" handlerfor="633177702497813763">
    <node type="Start" id="633177702497813766" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633185530407501011" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633177702497813774" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="217" y="35">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633185530407501011" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="131" y="115">
      <linkto id="633177702497813774" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="literal">Subscription Terminated</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">PresenceTriggeringSubscribe: </ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="633177702497813771" treenode="633177702497813772" appnode="633177702497813769" handlerfor="633177702497813768">
    <node type="Start" id="633177702497813771" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="45" y="53">
      <linkto id="633185530407500672" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633185530407500672" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="250" y="102">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>