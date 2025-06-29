<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632520303397145547" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632520303397145544" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632520303397145543" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.ProviderActionTimeout.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Trigger" id="632616270785254448" vid="632586003784784160">
        <Properties type="String" initWith="S_Trigger">S_Trigger</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632520303397145546" treenode="632520303397145547" appnode="632520303397145544" handlerfor="632520303397145543">
    <node type="Start" id="632520303397145546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="395">
      <linkto id="632586014586747802" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632586006736037983" name="SignalTimeout" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="496" y="264">
      <linkto id="632586031725904289" type="Labeled" style="Bezier" ortho="true" label="timeout" />
      <linkto id="632586031725904290" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Trigger</ap>
      </Properties>
    </node>
    <node type="Action" id="632586006736037900" name="SignalTimeout" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="495" y="507">
      <linkto id="632586031725904289" type="Labeled" style="Bezier" ortho="true" label="timeout" />
      <linkto id="632586031725904290" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Trigger</ap>
        <ap name="timeout" type="literal">10000</ap>
      </Properties>
    </node>
    <node type="Action" id="632586014586747802" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="151" y="394">
      <linkto id="632586006736037983" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632586006736037900" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">useDefault</ap>
      </Properties>
    </node>
    <node type="Action" id="632586031725904289" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="611" y="400">
      <linkto id="632586031725904290" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timeout" type="literal">999</ap>
        <ap name="signalName" type="variable">S_Trigger</ap>
      </Properties>
    </node>
    <node type="Action" id="632586031725904290" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="728" y="400">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632586014586747801" name="timeout" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="timeout" defaultInitWith="1000" refType="reference">timeout</Properties>
    </node>
    <node type="Variable" id="632586031725904288" name="useDefault" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" initWith="useDefault" refType="reference">useDefault</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>