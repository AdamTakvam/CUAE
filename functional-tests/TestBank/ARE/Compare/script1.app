<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632358885290781470" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632144407167812639" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632144407167812638" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.Compare.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632358885290781471" level="1" text="Metreos.Providers.FunctionalTest.Event: NotEqualScenario">
        <node type="function" name="NotEqualScenario" id="632144407167812659" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632144407167812658" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">ARE.Compare.script1.E_NotEqual</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_CompareResult" id="632358897212031457" vid="632144407167812642">
        <Properties type="Metreos.Types.String" initWith="S_CompareResult">S_CompareResult</Properties>
      </treenode>
      <treenode text="S_CompareResultNotEqual" id="632358897212031459" vid="632144407167812684">
        <Properties type="Metreos.Types.String" initWith="S_CompareResultNotEqual">S_CompareResultNotEqual</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632144407167812640" treenode="632358885290781470" appnode="632144407167812639" handlerfor="632144407167812638">
    <node type="Start" id="632144407167812640" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632144407167812644" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632144407167812644" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="258" y="188">
      <linkto id="632144407167812646" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632144407167812645" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">true</ap>
        <ap name="Value2" type="variable">trueVar</ap>
      </Properties>
    </node>
    <node type="Action" id="632144407167812645" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="259" y="379">
      <linkto id="632224788543125239" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="value" type="literal">notEqual</ap>
        <ap name="signalName" type="variable">S_CompareResult</ap>
      </Properties>
    </node>
    <node type="Action" id="632144407167812646" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="624" y="186">
      <linkto id="632224788543125239" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="value" type="literal">equal</ap>
        <ap name="signalName" type="variable">S_CompareResult</ap>
      </Properties>
    </node>
    <node type="Action" id="632224788543125239" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="622" y="379">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632358885290781472" name="trueVar" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" defaultInitWith="true" refType="reference">trueVar</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="NotEqualScenario" activetab="true" startnode="632144407167812660" treenode="632358885290781471" appnode="632144407167812659" handlerfor="632144407167812658">
    <node type="Start" id="632144407167812660" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632144407167812662" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632144407167812662" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="252" y="199">
      <linkto id="632144407167812664" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632144407167812665" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">true</ap>
        <ap name="Value2" type="variable">falseVar</ap>
      </Properties>
    </node>
    <node type="Action" id="632144407167812664" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="250" y="426">
      <linkto id="632224788543125238" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="value" type="literal">notEqual</ap>
        <ap name="signalName" type="variable">S_CompareResultNotEqual</ap>
      </Properties>
    </node>
    <node type="Action" id="632144407167812665" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="613" y="201">
      <linkto id="632224788543125238" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="value" type="literal">equal</ap>
        <ap name="signalName" type="variable">S_CompareResultNotEqual</ap>
      </Properties>
    </node>
    <node type="Action" id="632224788543125238" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="615" y="426">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632358885290781473" name="falseVar" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" defaultInitWith="false" refType="reference">falseVar</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
