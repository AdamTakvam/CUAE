<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632435065749391069" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632146103040000187" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632146103040000186" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">SMA.SMA70.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632435065749391070" level="1" text="Metreos.Providers.FunctionalTest.Event: OnEvent">
        <node type="function" name="OnEvent" id="632146103040000198" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632146103040000197" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">SMA.SMA70.script1.E_Event</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632435065749391071" level="1" text="Metreos.Providers.FunctionalTest.Event: Shutdown">
        <node type="function" name="Shutdown" id="632146103040000202" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632146103040000201" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">SMA.SMA70.script1.E_Shutdown</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple1" id="632435065749391058" vid="632146103040000190">
        <Properties type="Metreos.Types.String" initWith="S_Simple1">S_Simple1</Properties>
      </treenode>
      <treenode text="S_Simple2" id="632435065749391060" vid="632146103040000192">
        <Properties type="Metreos.Types.String" initWith="S_Simple2">S_Simple2</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632146103040000188" treenode="632146103040000189" appnode="632146103040000187" handlerfor="632146103040000186">
    <node type="Start" id="632146103040000188" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="47" y="344">
      <linkto id="632146103040000194" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632146103040000194" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="287" y="339">
      <linkto id="632224881663907793" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple1</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907793" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="571" y="339">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent" startnode="632146103040000199" treenode="632146103040000200" appnode="632146103040000198" handlerfor="632146103040000197">
    <node type="Start" id="632146103040000199" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632146103040000205" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632146103040000205" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="236" y="110">
      <linkto id="632224881663907794" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple2</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907794" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="465" y="105">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Shutdown" startnode="632146103040000203" treenode="632146103040000204" appnode="632146103040000202" handlerfor="632146103040000201">
    <node type="Start" id="632146103040000203" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224881663907795" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632224881663907795" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="400" y="250">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
