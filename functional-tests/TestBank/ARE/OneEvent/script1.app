<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632143577250937667" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632143577250937631" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632143577250937630" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.OneEvent.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632143577250937670" level="1" text="Event: OnEvent">
        <node type="function" name="OnEvent" id="632143577250937635" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632143577250937634" path="Metreos.Providers.FunctionalTest.Event" />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">ARE.OneEvent.script1.E_Simple</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632224814145468969" vid="632143577250937640">
        <Properties type="Metreos.Types.String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" varsy="692" startnode="632143577250937632" treenode="632143577250937667" appnode="632143577250937631" handlerfor="632143577250937630">
    <node type="Start" id="632143577250937632" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632143577250937678" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632143577250937678" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="193" y="128">
      <linkto id="632224814145468978" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632224814145468978" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="433" y="130">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent" varsy="692" startnode="632143577250937636" treenode="632143577250937670" appnode="632143577250937635" handlerfor="632143577250937634">
    <node type="Start" id="632143577250937636" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632143577250937639" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632143577250937639" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="240" y="206">
      <linkto id="632224814145468979" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple</ap>
        <log condition="entry" on="true" level="Info" type="literal">Sent signal</log>
      </Properties>
    </node>
    <node type="Action" id="632224814145468979" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="512" y="210">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
