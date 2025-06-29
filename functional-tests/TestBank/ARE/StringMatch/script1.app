<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632145265340625196" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632145265340625185" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632145265340625184" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.StringMatch.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_One" id="632224814145469277" vid="632145265340625198">
        <Properties type="Metreos.Types.String" initWith="S_One">S_One</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" varsy="692" startnode="632145265340625186" treenode="632145265340625196" appnode="632145265340625185" handlerfor="632145265340625184">
    <node type="Start" id="632145265340625186" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632145265340625200" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632145265340625200" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="307" y="212">
      <linkto id="632224814145469282" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_One</ap>
      </Properties>
    </node>
    <node type="Action" id="632224814145469282" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="598" y="214">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
