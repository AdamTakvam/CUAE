<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.7" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632277828331719153" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632277828331719150" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632277828331719149" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.SessionCleanConnections.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632277828331719152" treenode="632277828331719153" appnode="632277828331719150" handlerfor="632277828331719149">
    <node type="Start" id="632277828331719152" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="167">
      <linkto id="632277828331719162" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277828331719162" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="312" y="166">
      <linkto id="632277828331719164" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="literal">0</ap>
        <ap name="connectionId" type="literal">0</ap>
        <ap name="remoteIp" type="literal">0</ap>
      </Properties>
    </node>
    <node type="Action" id="632277828331719164" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="457" y="169">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
