<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632144263780000303" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632144263780000293" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632144263780000292" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.Sleep.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_AfterSleep" id="632471134873590410" vid="632144263780000296">
        <Properties type="Metreos.Types.String" initWith="S_AfterSleep">S_AfterSleep</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632144263780000294" treenode="632144263780000303" appnode="632144263780000293" handlerfor="632144263780000292">
    <node type="Start" id="632144263780000294" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="67">
      <linkto id="632471134873590415" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632144263780000300" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="538" y="213">
      <linkto id="632224814145469214" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_AfterSleep</ap>
      </Properties>
    </node>
    <node type="Action" id="632224814145469214" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="663" y="213">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632471134873590415" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="331" y="205">
      <linkto id="632144263780000300" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SleepTime" type="literal">20000</ap>
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
