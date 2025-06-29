<Application name="OnHoldTimer" trigger="Metreos.Providers.TimerFacility.TimerFire" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="OnHoldTimer">
    <outline>
      <treenode type="evh" id="632575523141978977" level="1" text="Metreos.Providers.TimerFacility.TimerFire (trigger): OnTimerFire">
        <node type="function" name="OnTimerFire" id="632575523141978974" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632575523141978973" path="Metreos.Providers.TimerFacility.TimerFire" trigger="true" />
        <Properties type="hybrid">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_Username" id="632575602609116566" vid="632575523141979113">
        <Properties type="String" initWith="Username">g_Username</Properties>
      </treenode>
      <treenode text="g_Password" id="632575602609116568" vid="632575523141979115">
        <Properties type="String" initWith="Password">g_Password</Properties>
      </treenode>
      <treenode text="g_Filename" id="632575602609116570" vid="632575602609116556">
        <Properties type="String" initWith="Filename">g_Filename</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTimerFire" activetab="true" startnode="632575523141978976" treenode="632575523141978977" appnode="632575523141978974" handlerfor="632575523141978973">
    <node type="Start" id="632575523141978976" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="45" y="181">
      <linkto id="632575523141979098" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575523141979098" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="164" y="180">
      <linkto id="632575523141979100" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575523141979117" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"Play:" + g_Filename</ap>
        <rd field="ResultData">executeMsg</rd>
      </Properties>
    </node>
    <node type="Action" id="632575523141979100" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="290" y="179">
      <linkto id="632575523141979117" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632575523141979118" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="variable">executeMsg</ap>
        <ap name="URL" type="csharp">"http://" + phoneIP + "/CGI/Execute"</ap>
        <ap name="Username" type="variable">g_Username</ap>
        <ap name="Password" type="variable">g_Password</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"Sending to phone:\n" + executeMsg</log>
      </Properties>
    </node>
    <node type="Action" id="632575523141979117" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="219" y="292">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="csharp">"Failed to send execute message to phone: " + phoneIP</log>
      </Properties>
    </node>
    <node type="Action" id="632575523141979118" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="430" y="179">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632575523141979037" name="phoneIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="timerUserData" refType="reference" name="Metreos.Providers.TimerFacility.TimerFire">phoneIP</Properties>
    </node>
    <node type="Variable" id="632575523141979099" name="executeMsg" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">executeMsg</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>