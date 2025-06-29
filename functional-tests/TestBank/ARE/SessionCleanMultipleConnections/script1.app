<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.7" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632277828331719153" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632277828331719150" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632277828331719149" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.SessionCleanMultipleConnections.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632277828331719152" treenode="632277828331719153" appnode="632277828331719150" handlerfor="632277828331719149">
    <node type="Loop" id="632278358485781464" name="Loop" text="loop (var)" cx="356" cy="193" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="107" y="81" mx="285" my="178">
      <linkto id="632277828331719162" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632277828331719164" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">numConnections</Properties>
    </node>
    <node type="Start" id="632277828331719152" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="167">
      <linkto id="632278358485781464" port="1" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277828331719162" name="CreateConnection" container="632278358485781464" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="188" y="175">
      <linkto id="632277828331719185" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="literal">0</ap>
        <ap name="connectionId" type="literal">0</ap>
        <ap name="remoteIp" type="literal">0</ap>
        <rd field="connectionId">connection</rd>
      </Properties>
    </node>
    <node type="Action" id="632277828331719164" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="570" y="173">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632277828331719185" name="CreateConnection" container="632278358485781464" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="344" y="175">
      <linkto id="632278358485781464" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="csharp">5000 + loopIndex * 2</ap>
        <ap name="connectionId" type="variable">connection</ap>
        <ap name="remoteIp" type="literal">127.0.0.1</ap>
      </Properties>
    </node>
    <node type="Variable" id="632277828331719184" name="connection" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Int" refType="reference">connection</Properties>
    </node>
    <node type="Variable" id="632278358485781465" name="numConnections" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Int" initWith="numConnections" refType="reference">numConnections</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
