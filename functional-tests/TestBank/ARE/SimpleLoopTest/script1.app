<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632143672065468888" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632143649841875187" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632143649841875186" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.SimpleLoopTest.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_LoopCountValue" id="632224814145469186" vid="632143649841875190">
        <Properties type="Metreos.Types.String" initWith="S_LoopCountValue">S_LoopCountValue</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" varsy="692" startnode="632143649841875188" treenode="632143672065468888" appnode="632143649841875187" handlerfor="632143649841875186">
    <node type="Loop" id="632143649841875192" name="Loop" text="loop 10x" cx="220" cy="208" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="108" y="142" mx="218" my="246">
      <linkto id="632143649841875193" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632224814145469191" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="literal">10</Properties>
    </node>
    <node type="Start" id="632143649841875188" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632143649841875192" port="1" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632143649841875193" name="Signal" container="632143649841875192" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="217" y="244">
      <linkto id="632143649841875192" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="loopCountValue" type="csharp">loopIndex.ToString()</ap>
        <ap name="signalName" type="variable">S_LoopCountValue</ap>
      </Properties>
    </node>
    <node type="Action" id="632224814145469191" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="478" y="264">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
