<Application name="script2" trigger="Metreos.Providers.TimerFacility.TimerFire" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script2">
    <outline>
      <treenode type="evh" id="632149769125156370" level="1" text="TimerFire (trigger): OnTimerFire">
        <node type="function" name="OnTimerFire" id="632149769125156368" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632149769125156367" path="Metreos.Providers.TimerFacility.TimerFire" trigger="true" />
        <Properties type="hybrid">
          <ep name="timerUserData" type="literal">onlyTimer</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Fired" id="632224881663907699" vid="632149778033281366">
        <Properties type="Metreos.Types.String" initWith="S_Fired">S_Fired</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTimerFire" activetab="true" varsy="692" startnode="632149769125156369" treenode="632149769125156370" appnode="632149769125156368" handlerfor="632149769125156367">
    <node type="Start" id="632149769125156369" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="88">
      <linkto id="632149778033281365" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632149778033281365" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="416" y="173">
      <linkto id="632224881663907704" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Fired</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907704" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="570" y="184">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
