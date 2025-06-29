<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.7" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632278647224531424" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632278647224531421" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632278647224531420" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.BadParamInNativeAction.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Success" id="632279169344531445" vid="632279169344531444">
        <Properties type="Metreos.Types.String" initWith="S_Success">S_Success</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632278647224531423" treenode="632278647224531424" appnode="632278647224531421" handlerfor="632278647224531420">
    <node type="Start" id="632278647224531423" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="367">
      <linkto id="632279169344531426" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632279169344531426" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="331" y="372">
      <linkto id="632279169344531429" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="URL1" type="literal">aoeu</ap>
        <ap name="Priority1" type="variable">S_Success</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632279169344531428" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="653" y="391">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632279169344531429" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="504" y="364">
      <linkto id="632279169344531428" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Success</ap>
      </Properties>
    </node>
    <node type="Variable" id="632279169344531427" name="badVarType" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Double" defaultInitWith="3.234" refType="reference">badVarType</Properties>
    </node>
    <node type="Variable" id="632279169344531446" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
