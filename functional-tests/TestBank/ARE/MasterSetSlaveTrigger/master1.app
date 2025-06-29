<Application name="master1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="master1">
    <outline>
      <treenode type="evh" id="632520303397145547" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632520303397145544" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632520303397145543" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.MasterSetSlaveTrigger.master1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="6503484" level="2" text="Metreos.Providers.FunctionalTest.Event: OnShutdown">
        <node type="function" name="OnShutdown" id="6502703" path="Metreos.StockTools" />
        <node type="event" name="Event" id="6502406" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_EnabledSlave" id="632585836981456841" vid="5029578">
        <Properties type="String" initWith="S_EnabledSlave">S_EnabledSlave</Properties>
      </treenode>
      <treenode text="S_Shutdown" id="632585836981456843" vid="5029578">
        <Properties type="String" initWith="S_Shutdown">S_Shutdown</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632520303397145546" treenode="632520303397145547" appnode="632520303397145544" handlerfor="632520303397145543">
    <node type="Start" id="632520303397145546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="336">
      <linkto id="632585808781670058" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632585808781670058" name="EnableScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="353" y="334">
      <linkto id="632585808781670098" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="testScriptName" type="literal">somevalue</ap>
        <ap name="ScriptName" type="literal">slave1</ap>
      </Properties>
    </node>
    <node type="Action" id="632585808781670059" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="560" y="334">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632585808781670098" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="453" y="332">
      <linkto id="632585808781670059" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_EnabledSlave</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnShutdown" startnode="6526968" treenode="6503484" appnode="6502703" handlerfor="6502406">
    <node type="Start" id="6526968" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="395">
      <linkto id="632585808781670057" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632585808781670057" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="699" y="357">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>