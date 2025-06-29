<Application name="Responder" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Responder">
    <outline>
      <treenode type="evh" id="632497639089375045" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632497639089375042" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632497639089375041" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632497757780318318" vid="632497639089375054">
        <Properties type="String" defaultInitWith="ARE.ScriptInitRate.Responder" initWith="S_Simple">S_Simple</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632497639089375044" treenode="632497639089375045" appnode="632497639089375042" handlerfor="632497639089375041">
    <node type="Start" id="632497639089375044" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="128">
      <linkto id="632497639089375056" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632497639089375056" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="150" y="128">
      <linkto id="632497713476850420" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="TimeStamp" type="variable">timeStamp</ap>
        <ap name="signalName" type="literal">ARE.ScriptInitRate.Responder.S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632497713476850420" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="274" y="128">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632497639089375057" name="timeStamp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DateTime" initWith="TimeStamp" refType="reference">timeStamp</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
