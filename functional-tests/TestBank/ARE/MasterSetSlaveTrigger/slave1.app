<Application name="slave1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="slave1">
    <outline>
      <treenode type="evh" id="632520303397145547" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632520303397145544" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632520303397145543" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.MasterSetSlaveTrigger.slave1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="6158640" level="2" text="Metreos.Providers.FunctionalTest.Event: OnShutdown">
        <node type="function" name="OnShutdown" id="6158640" path="Metreos.StockTools" />
        <node type="event" name="Event" id="6158640" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Triggered" id="632585824411104720" vid="5029578">
        <Properties type="String" initWith="S_Triggered">S_Triggered</Properties>
      </treenode>
      <treenode text="S_Shutdown2" id="632585824411104722" vid="5029578">
        <Properties type="String" initWith="S_Shutdown2">S_Shutdown2</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632520303397145546" treenode="632520303397145547" appnode="632520303397145544" handlerfor="632520303397145543">
    <node type="Start" id="632520303397145546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="338">
      <linkto id="632585808781670077" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632585808781670077" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="348" y="331">
      <linkto id="632585808781670078" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Triggered</ap>
      </Properties>
    </node>
    <node type="Action" id="632585808781670078" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="465" y="392">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnShutdown" startnode="6158640" treenode="6158640" appnode="6158640" handlerfor="6158640">
    <node type="Start" id="6158640" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="395">
      <linkto id="632585808781670076" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632585808781670076" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="438" y="354">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="slave" instanceType="multiInstance" desc="">
  </Properties>
</Application>