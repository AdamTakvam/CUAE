<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632497633078356583" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632145265340625133" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632145265340625132" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.OneSignalAndWait.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632497633078356584" level="1" text="Metreos.Providers.FunctionalTest.Event: Shutdown">
        <node type="function" name="Shutdown" id="632145265340625139" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632145265340625138" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">ARE.OneSignalAndWait.script1.E_Shutdown</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632580609502868964" vid="632145265340625136">
        <Properties type="Metreos.Types.String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632145265340625134" treenode="632497633078356583" appnode="632145265340625133" handlerfor="632145265340625132">
    <node type="Start" id="632145265340625134" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="68" y="110">
      <linkto id="632145265340625142" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632145265340625142" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="420" y="198">
      <linkto id="632224814145469030" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632224814145469030" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="603" y="197">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Shutdown" startnode="632145265340625140" treenode="632497633078356584" appnode="632145265340625139" handlerfor="632145265340625138">
    <node type="Start" id="632145265340625140" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224814145469031" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632224814145469031" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="342" y="222">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>