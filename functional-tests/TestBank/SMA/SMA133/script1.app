<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632434838897777170" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632143672065468922" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632143672065468921" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">SMA.SMA133.script1</ep>
        </Properties>
      </treenode>
      <treenode type="fun" id="632434838897777171" level="1" text="RegularFunction">
        <node type="function" name="RegularFunction" id="632143672065468930" path="Metreos.StockTools" />
        <calls>
          <ref actid="632143672065468929" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_BeforeExit" id="632434838897777160" vid="632143672065468925">
        <Properties type="Metreos.Types.String" initWith="S_BeforeExit">S_BeforeExit</Properties>
      </treenode>
      <treenode text="S_AfterExit" id="632434838897777162" vid="632143672065468927">
        <Properties type="Metreos.Types.String" initWith="S_AfterExit">S_AfterExit</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632143672065468923" treenode="632146103040000154" appnode="632143672065468922" handlerfor="632143672065468921">
    <node type="Start" id="632143672065468923" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632143672065468929" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632143672065468929" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="187.902344" y="194" mx="236" my="210">
      <items count="1">
        <item text="RegularFunction" />
      </items>
      <linkto id="632143672065468935" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="FunctionName" type="literal">RegularFunction</ap>
        <log condition="exit" on="true" level="Error" type="literal">This log should not occur. It is after an exit.</log>
      </Properties>
    </node>
    <node type="Action" id="632143672065468935" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="369" y="192">
      <linkto id="632224881663907851" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_AfterExit</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907851" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="533" y="194">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="RegularFunction" activetab="true" startnode="632143672065468931" treenode="632146103040000156" appnode="632143672065468930" handlerfor="632143672065468921">
    <node type="Start" id="632143672065468931" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632143672065468933" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632143672065468933" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="327" y="211">
      <linkto id="632434838897777172" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_BeforeExit</ap>
      </Properties>
    </node>
    <node type="Action" id="632434838897777172" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="506" y="209">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
