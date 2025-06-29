<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632520303397145547" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632520303397145544" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632520303397145543" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.NativeTypeToProvider.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Trigger" id="632585943802347006" vid="632585943697509664">
        <Properties type="String" initWith="S_Trigger">S_Trigger</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632520303397145546" treenode="632520303397145547" appnode="632520303397145544" handlerfor="632520303397145543">
    <node type="Start" id="632520303397145546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="322">
      <linkto id="632585943802347009" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632585943802347009" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="399" y="345">
      <linkto id="632585943802347010" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="testValue" type="variable">ipPhoneMenu</ap>
        <ap name="signalName" type="variable">S_Trigger</ap>
      </Properties>
    </node>
    <node type="Action" id="632585943802347010" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="655" y="342">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632585943802347008" name="ipPhoneMenu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" initWith="RoutingGuid" refType="reference">ipPhoneMenu</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>