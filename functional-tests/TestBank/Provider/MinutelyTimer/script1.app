<Application name="script1" trigger="Metreos.Providers.TimerFacility.TimerFire" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632150328113281381" level="1" text="TimerFire (trigger): OnTimerFire">
        <node type="function" name="OnTimerFire" id="632150328113281379" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632150328113281378" path="Metreos.Providers.TimerFacility.TimerFire" trigger="true" />
        <Properties type="hybrid">
          <ep name="timerId" type="literal">Minutely_Timer</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Fired" id="632224855787500320" vid="632150328113281382">
        <Properties type="Metreos.Types.String" initWith="S_Fired">S_Fired</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTimerFire" varsy="692" startnode="632150328113281380" treenode="632150328113281381" appnode="632150328113281379" handlerfor="632150328113281378">
    <node type="Start" id="632150328113281380" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632150328113281384" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150328113281384" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="298" y="203">
      <linkto id="632224855787500325" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Fired</ap>
      </Properties>
    </node>
    <node type="Action" id="632224855787500325" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="470" y="232">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
