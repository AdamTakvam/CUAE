<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.7" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632278647224531424" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632278647224531421" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632278647224531420" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.ReferenceTypeParam.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Success" id="632279169344531509" vid="632279169344531444">
        <Properties type="Metreos.Types.String" initWith="S_Success">S_Success</Properties>
      </treenode>
      <treenode text="S_Failure" id="632279169344531517" vid="632279169344531516">
        <Properties type="Metreos.Types.String" initWith="S_Failure">S_Failure</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632278647224531423" treenode="632278647224531424" appnode="632278647224531421" handlerfor="632278647224531420">
    <node type="Start" id="632278647224531423" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="367">
      <linkto id="632279169344531521" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632279169344531428" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="693" y="370">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632279169344531429" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="542" y="308">
      <linkto id="632279169344531428" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632279169344531518" name="FormatAddress" class="MaxActionNode" group="" path="Metreos.Native.DialPlan" x="321" y="367">
      <linkto id="632279169344531522" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native">
        <ap name="DialedNumber" type="literal">9009000</ap>
        <ap name="CM_Address" type="literal">192.168.1.250</ap>
        <ap name="DialingRules" type="variable">rules</ap>
        <rd field="ResultData">rulesApplied</rd>
      </Properties>
    </node>
    <node type="Action" id="632279169344531521" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="176" y="368">
      <linkto id="632279169344531518" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(Hashtable rules)
	{
		rules["0"] = "1";
		return String.Empty;
	}
</Properties>
    </node>
    <node type="Action" id="632279169344531522" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="424" y="367">
      <linkto id="632279169344531429" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632279169344531523" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="literal">9119111@192.168.1.250</ap>
        <ap name="Value2" type="variable">rulesApplied</ap>
      </Properties>
    </node>
    <node type="Action" id="632279169344531523" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="545" y="428">
      <linkto id="632279169344531428" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Failure</ap>
      </Properties>
    </node>
    <node type="Variable" id="632279169344531427" name="badVarType" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Double" defaultInitWith="3.234" refType="reference">badVarType</Properties>
    </node>
    <node type="Variable" id="632279169344531446" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="632279169344531519" name="rules" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Hashtable" refType="reference">rules</Properties>
    </node>
    <node type="Variable" id="632279169344531520" name="rulesApplied" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">rulesApplied</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
