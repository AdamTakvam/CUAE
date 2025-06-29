<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632141067691718913" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632141067691718872" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632141067691718871" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.OneSignal.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632224814145468996" vid="632141067691718875">
        <Properties type="Metreos.Types.String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" varsy="692" startnode="632141067691718873" treenode="632141067691718913" appnode="632141067691718872" handlerfor="632141067691718871">
    <node type="Start" id="632141067691718873" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632141067691718877" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632141067691718877" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="318" y="172">
      <linkto id="632224814145469001" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple</ap>
        <log condition="exit" on="true" level="Info" type="literal">Signal sent</log>
      </Properties>
    </node>
    <node type="Action" id="632224814145469001" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="491" y="156">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
