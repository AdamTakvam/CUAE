<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632604015970438831" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632143577250937631" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632143577250937630" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.OneEvent2.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632604015970438832" level="1" text="Metreos.Providers.FunctionalTest.Event: OnEvent">
        <node type="function" name="OnEvent" id="632143577250937635" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632143577250937634" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">ARE.OneEvent2.script1.E_Simple</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632604265497963455" vid="632143577250937640">
        <Properties type="Metreos.Types.String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632143577250937632" treenode="632604015970438831" appnode="632143577250937631" handlerfor="632143577250937630">
    <node type="Start" id="632143577250937632" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632143577250937678" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632143577250937678" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="193" y="128">
      <linkto id="632224814145468978" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="count" type="variable">count</ap>
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632224814145468978" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="433" y="130">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632604265497963570" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="375" y="242">
      <Properties final="false" type="native" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632604019549288857" name="count" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="count" defaultInitWith="0" refType="reference">count</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent" startnode="632143577250937636" treenode="632604015970438832" appnode="632143577250937635" handlerfor="632143577250937634">
    <node type="Start" id="632143577250937636" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224814145468979" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632224814145468979" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="512" y="210">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>