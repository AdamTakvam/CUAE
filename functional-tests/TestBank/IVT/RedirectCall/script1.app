<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632526323733971716" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632526323733971713" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632526323733971712" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Redirect" id="632527174907346667" vid="632526323733971734">
        <Properties type="String" initWith="S_Redirect">S_Redirect</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632526323733971715" treenode="632526323733971716" appnode="632526323733971713" handlerfor="632526323733971712">
    <node type="Start" id="632526323733971715" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="337">
      <linkto id="632526323733971718" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632526323733971718" name="Redirect" class="MaxActionNode" group="" path="Metreos.CallControl" x="233" y="338">
      <linkto id="632526323733971721" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="To" type="csharp">to.Substring(4)</ap>
        <log condition="entry" on="true" level="Info" type="csharp">to.Substring(4)</log>
      </Properties>
    </node>
    <node type="Action" id="632526323733971720" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="624" y="338">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632526323733971721" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="430" y="338">
      <linkto id="632526323733971720" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Redirect</ap>
      </Properties>
    </node>
    <node type="Variable" id="632526323733971717" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632526323733971736" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to" refType="reference">to</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
