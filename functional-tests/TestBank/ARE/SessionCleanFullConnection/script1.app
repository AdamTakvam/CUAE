<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.7" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632277828331719153" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632277828331719150" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632277828331719149" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.SessionCleanFullConnection.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632277828331719152" treenode="632277828331719153" appnode="632277828331719150" handlerfor="632277828331719149">
    <node type="Start" id="632277828331719152" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="167">
      <linkto id="632277828331719162" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277828331719162" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="160" y="164">
      <linkto id="632277828331719185" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="literal">0</ap>
        <ap name="connectionId" type="literal">0</ap>
        <ap name="remoteIp" type="literal">0</ap>
        <rd field="connectionId">connection</rd>
      </Properties>
    </node>
    <node type="Action" id="632277828331719164" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="457" y="169">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632277828331719185" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="307" y="169">
      <linkto id="632277828331719164" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="literal">5000</ap>
        <ap name="connectionId" type="variable">connection</ap>
        <ap name="remoteIp" type="literal">127.0.0.1</ap>
      </Properties>
    </node>
    <node type="Variable" id="632277828331719184" name="connection" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Int" refType="reference">connection</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
