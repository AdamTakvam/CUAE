<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632547382093097953" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632547382093097950" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632547382093097949" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Core.NoHandler.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632547382093097968" level="2" text="Metreos.Providers.FunctionalTest.Event: E_NonTrigger">
        <node type="function" name="E_NonTrigger" id="632547382093097965" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632547382093097964" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Core.NoHandler.script1.E_NonTrigger</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632547382093097973" level="2" text="Metreos.Providers.FunctionalTest.Event: E_Shutdown">
        <node type="function" name="E_Shutdown" id="632547382093097970" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632547382093097969" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Core.NoHandler.script1.E_Shutdown</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632548946734994467" vid="632547382093097962">
        <Properties type="String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632547382093097952" treenode="632547382093097953" appnode="632547382093097950" handlerfor="632547382093097949">
    <node type="Start" id="632547382093097952" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="409">
      <linkto id="632547382093097994" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632547382093097994" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="359" y="404">
      <linkto id="632547382093097995" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632547382093097995" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="606" y="406">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="E_NonTrigger" startnode="632547382093097967" treenode="632547382093097968" appnode="632547382093097965" handlerfor="632547382093097964">
    <node type="Start" id="632547382093097967" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="394">
      <linkto id="632547382093097997" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632547382093097997" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="389" y="397">
      <linkto id="632547382093097998" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632547382093097998" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="558" y="410">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="E_Shutdown" activetab="true" startnode="632547382093097972" treenode="632547382093097973" appnode="632547382093097970" handlerfor="632547382093097969">
    <node type="Start" id="632547382093097972" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="347">
      <linkto id="632548946734994478" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632547382093097999" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="696" y="374">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632548946734994478" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="330.610016" y="388">
      <linkto id="632547382093097999" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
