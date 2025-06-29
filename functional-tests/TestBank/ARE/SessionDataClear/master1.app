<Application name="master1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="master1">
    <outline>
      <treenode type="evh" id="632520303397145547" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632520303397145544" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632520303397145543" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.SessionDataClear.master1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="11322296" level="2" text="Metreos.Providers.FunctionalTest.Event: OnShutdown">
        <node type="function" name="OnShutdown" id="11322296" path="Metreos.StockTools" />
        <node type="event" name="Event" id="11322296" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">ARE.SessionDataClear.master1.E_Shutdown</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_EnabledSlaves" id="632585926585314318" vid="11322281">
        <Properties type="String" initWith="S_EnabledSlaves">S_EnabledSlaves</Properties>
      </treenode>
      <treenode text="S_Shutdown" id="632585926585314320" vid="14411718">
        <Properties type="String" initWith="S_Shutdown">S_Shutdown</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632520303397145546" treenode="632520303397145547" appnode="632520303397145544" handlerfor="632520303397145543">
    <node type="Loop" id="632585887478645267" name="Loop" text="loop (var)" cx="120" cy="120" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="388" y="330" mx="448" my="390">
      <linkto id="632585887478645268" fromport="1" type="Basic" style="Bevel" />
      <linkto id="632585887478645270" fromport="3" type="Labeled" style="Bevel" label="default" />
      <Properties iteratorType="int" type="variable">count</Properties>
    </node>
    <node type="Start" id="632520303397145546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="395">
      <linkto id="632585887478645264" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632585887478645264" name="SetSessionData" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="254" y="391">
      <linkto id="632585887478645267" port="1" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="testValue" type="variable">testValue</ap>
      </Properties>
    </node>
    <node type="Action" id="632585887478645268" name="EnableScript" container="632585887478645267" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="447" y="389">
      <linkto id="632585887478645267" port="3" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="ScriptName" type="literal">slave1</ap>
      </Properties>
    </node>
    <node type="Action" id="632585887478645269" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="697" y="386">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632585887478645270" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="581" y="384">
      <linkto id="632585887478645269" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_EnabledSlaves</ap>
      </Properties>
    </node>
    <node type="Variable" id="632585887478645265" name="count" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="count" refType="reference">count</Properties>
    </node>
    <node type="Variable" id="632585887478645266" name="testValue" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="testValue" refType="reference">testValue</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnShutdown" startnode="11322296" treenode="11322296" appnode="11322296" handlerfor="11322296">
    <node type="Start" id="11322296" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="395">
      <linkto id="632585856576775485" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632585856576775485" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="485" y="337">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>