<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632225765656718909" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632225765656718906" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632225765656718905" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.Database.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632580609502868700" vid="632225765656718927">
        <Properties type="Metreos.Types.String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632225765656718908" treenode="632225765656718909" appnode="632225765656718906" handlerfor="632225765656718905">
    <node type="Start" id="632225765656718908" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="110" y="258">
      <linkto id="632225765656718926" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632225765656718926" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="393" y="259">
      <linkto id="632225765656718941" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632225765656718941" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="672" y="261">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>