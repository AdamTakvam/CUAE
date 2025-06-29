<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.7" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632278574451250174" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632278574451250171" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632278574451250170" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.MediaFileProvisioning.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632278574451250173" treenode="632278574451250174" appnode="632278574451250171" handlerfor="632278574451250170">
    <node type="Start" id="632278574451250173" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="369">
      <linkto id="632278574451250175" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632278574451250175" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="492" y="368">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
