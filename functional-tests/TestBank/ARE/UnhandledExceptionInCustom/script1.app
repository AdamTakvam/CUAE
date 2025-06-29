<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632520303397145547" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632520303397145544" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632520303397145543" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.UnhandledExceptionInCustom.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Trigger" id="632586025783141106" vid="632586003784784160">
        <Properties type="String" initWith="S_Trigger">S_Trigger</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632520303397145546" treenode="632520303397145547" appnode="632520303397145544" handlerfor="632520303397145543">
    <node type="Start" id="632520303397145546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="395">
      <linkto id="632586014586747802" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632586006736037983" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="490" y="398">
      <linkto id="632586006736037984" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Trigger</ap>
      </Properties>
    </node>
    <node type="Action" id="632586006736037984" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="630" y="394">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632586014586747800" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="297" y="265">
      <linkto id="632586006736037983" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
public static string Execute()
{
	int zero = 0;
	int nevar = 5/zero;
	return "";
}
</Properties>
    </node>
    <node type="Action" id="632586014586747802" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="151" y="394">
      <linkto id="632586014586747800" type="Labeled" style="Bevel" label="default" />
      <linkto id="632586006736037983" type="Labeled" style="Bevel" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">branch == "branch"</ap>
      </Properties>
    </node>
    <node type="Variable" id="632586014586747801" name="branch" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="branch" refType="reference">branch</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>