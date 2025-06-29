<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632145265340625252" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632145265340625227" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632145265340625226" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.RegexMatch.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_One" id="632224814145469096" vid="632145265340625254">
        <Properties type="Metreos.Types.String" initWith="S_One">S_One</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" varsy="692" startnode="632145265340625228" treenode="632145265340625252" appnode="632145265340625227" handlerfor="632145265340625226">
    <node type="Start" id="632145265340625228" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632145265340625256" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632145265340625256" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="330" y="205">
      <linkto id="632224814145469101" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_One</ap>
      </Properties>
    </node>
    <node type="Action" id="632224814145469101" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="537" y="188">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
