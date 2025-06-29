<Application name="OnHoldTimer" trigger="Metreos.Providers.TimerFacility.TimerFire" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="OnHoldTimer">
    <outline>
      <treenode type="evh" id="632835430197446283" level="1" text="Metreos.Providers.TimerFacility.TimerFire (trigger): OnTimerFire">
        <node type="function" name="OnTimerFire" id="632835430197446280" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632835430197446279" path="Metreos.Providers.TimerFacility.TimerFire" trigger="true" />
        <Properties type="hybrid">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_Username" id="632835430197446517" vid="632835430197446284">
        <Properties type="String" initWith="Username">g_Username</Properties>
      </treenode>
      <treenode text="g_Password" id="632835430197446519" vid="632835430197446286">
        <Properties type="String" initWith="Password">g_Password</Properties>
      </treenode>
      <treenode text="g_Filename" id="632835430197446521" vid="632835430197446288">
        <Properties type="String" initWith="Filename">g_Filename</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTimerFire" activetab="true" startnode="632835430197446282" treenode="632835430197446283" appnode="632835430197446280" handlerfor="632835430197446279">
    <node type="Start" id="632835430197446282" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="126">
      <linkto id="632835430197446290" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632835430197446290" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="160.0983" y="127">
      <linkto id="632835430197446291" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632835430197446292" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"Play:" + g_Filename</ap>
        <rd field="ResultData">executeMsg</rd>
      </Properties>
    </node>
    <node type="Action" id="632835430197446291" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="286.0983" y="126">
      <linkto id="632835430197446292" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632835430197446293" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="variable">executeMsg</ap>
        <ap name="URL" type="csharp">"http://" + phoneIP + "/CGI/Execute"</ap>
        <ap name="Username" type="variable">g_Username</ap>
        <ap name="Password" type="variable">g_Password</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"DEBUG: Sending to phone:\n" + executeMsg</log>
      </Properties>
    </node>
    <node type="Action" id="632835430197446292" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="215.0983" y="239">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="csharp">"Failed to send execute message to phone: " + phoneIP</log>
      </Properties>
    </node>
    <node type="Action" id="632835430197446293" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="426.0983" y="126">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632835430197446294" text="Create a command &#xD;&#xA;to send to the IP phone" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="76" y="63" />
    <node type="Comment" id="632835430197446295" text="Send the command&#xD;&#xA;to the IP phone" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="263" y="66" />
    <node type="Variable" id="632835430197446529" name="phoneIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="timerUserData" refType="reference" name="Metreos.Providers.TimerFacility.TimerFire">phoneIP</Properties>
    </node>
    <node type="Variable" id="632835430197446530" name="executeMsg" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">executeMsg</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>