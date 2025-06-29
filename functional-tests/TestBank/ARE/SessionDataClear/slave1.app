<Application name="slave1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="slave1">
    <outline>
      <treenode type="evh" id="632520303397145547" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632520303397145544" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632520303397145543" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.SessionDataClear.slave1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="1" level="2" text="Metreos.Providers.FunctionalTest.Event: OnShutdown">
        <node type="function" name="OnShutdown" id="2" path="Metreos.StockTools" />
        <node type="event" name="Event" id="3" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">ARE.SessionDataClear.slave1.E_Shutdown2</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="4" level="2" text="Metreos.Providers.FunctionalTest.Event: OnCheckData">
        <node type="function" name="OnCheckData" id="5" path="Metreos.StockTools" />
        <node type="event" name="Event" id="6" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">ARE.SessionDataClear.slave1.E_CheckData</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Triggered" id="632585926585314345" vid="11322296">
        <Properties type="String" initWith="S_Triggered">S_Triggered</Properties>
      </treenode>
      <treenode text="S_CheckData" id="632585926585314347" vid="11322297">
        <Properties type="String" initWith="S_CheckData">S_CheckData</Properties>
      </treenode>
      <treenode text="S_Shutdown2" id="632585926585314349" vid="14411718">
        <Properties type="String" initWith="S_Shutdown2">S_Shutdown2</Properties>
      </treenode>
      <treenode text="g_id" id="632585926585314351" vid="632585887478645295">
        <Properties type="Int">g_id</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632520303397145546" treenode="632520303397145547" appnode="632520303397145544" handlerfor="632520303397145543">
    <node type="Start" id="632520303397145546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="395">
      <linkto id="632585887478645297" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632585887478645297" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="195" y="393">
      <linkto id="632585887478645302" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">id</ap>
        <rd field="ResultData">g_id</rd>
      </Properties>
    </node>
    <node type="Action" id="632585887478645298" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="427" y="393">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632585887478645302" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="309" y="390">
      <linkto id="632585887478645298" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="id" type="variable">id</ap>
        <ap name="signalName" type="variable">S_Triggered</ap>
      </Properties>
    </node>
    <node type="Variable" id="632585887478645294" name="id" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="id" refType="reference">id</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnShutdown" startnode="100" treenode="1" appnode="2" handlerfor="3">
    <node type="Start" id="100" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="395">
      <linkto id="632585856576775469" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632585856576775469" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="549" y="305">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnCheckData" activetab="true" startnode="101" treenode="4" appnode="5" handlerfor="6">
    <node type="Start" id="101" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="347">
      <linkto id="632585887478645299" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632585887478645299" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="263" y="347">
      <linkto id="632585887478645301" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
public static string Execute(SessionData sessionData, ref int testValue, LogWriter log)
{
      testValue = 0;

	if(sessionData == null)
	{
		log.Write(TraceLevel.Error, "sessionData is null");
		return "";
	}
	if(sessionData.CustomData == null)
	{	
		log.Write(TraceLevel.Error, "sessionData.CustomData is null");
		return "";
	}
	log.Write(TraceLevel.Info, "About to retrieve the test value from the CustomData hashtable");

	object obj = sessionData.CustomData["testValue"];

	if(obj == null)
	{
		log.Write(TraceLevel.Error, "The testValue could not be found.  The test should fail");
		testValue = 0;
		return "";
	}
	else
	{
		testValue = (int) sessionData.CustomData["testValue"];
		log.Write(TraceLevel.Info, "The testValue was found: " + testValue );	
		return "";
	}
}
</Properties>
    </node>
    <node type="Action" id="632585887478645301" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="474" y="344">
      <linkto id="632585887478645303" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="testValue" type="variable">testValue</ap>
        <ap name="signalName" type="variable">S_CheckData</ap>
      </Properties>
    </node>
    <node type="Action" id="632585887478645303" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="705" y="333">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632585887478645300" name="testValue" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">testValue</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="slave" instanceType="multiInstance" desc="">
  </Properties>
</Application>