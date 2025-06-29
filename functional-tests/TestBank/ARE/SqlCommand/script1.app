<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632277756578281510" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632277756578281507" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632277756578281506" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.SqlCommand.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Success" id="632580652409918973" vid="632277756578281521">
        <Properties type="Metreos.Types.String" initWith="S_Success">S_Success</Properties>
      </treenode>
      <treenode text="S_Failure" id="632580652409918975" vid="632277756578281523">
        <Properties type="Metreos.Types.String" initWith="S_Failure">S_Failure</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632277756578281509" treenode="632277756578281510" appnode="632277756578281507" handlerfor="632277756578281506">
    <node type="Start" id="632277756578281509" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="366">
      <linkto id="632277756578281519" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632277756578281519" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="154" y="366">
      <linkto id="632279254591718938" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">whatever</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Action" id="632277756578281525" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="412" y="370">
      <linkto id="632277756578281527" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">sqlBuilder.ToString()</ap>
        <ap name="Name" type="literal">whatever</ap>
        <rd field="ResultSet">resultSet</rd>
      </Properties>
    </node>
    <node type="Action" id="632277756578281527" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="546" y="367">
      <linkto id="632277756578281528" type="Labeled" style="Bevel" label="equal" />
      <linkto id="632277756578281529" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">resultSet.Rows[0]["value"]</ap>
        <ap name="Value2" type="literal">value1</ap>
      </Properties>
    </node>
    <node type="Action" id="632277756578281528" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="608" y="235">
      <linkto id="632277756578281531" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632277756578281529" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="611" y="463">
      <linkto id="632277756578281531" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Failure</ap>
      </Properties>
    </node>
    <node type="Action" id="632277756578281531" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="719.4713" y="384">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632279254591718938" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="278" y="366">
      <linkto id="632277756578281525" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">

	public static string Execute(Metreos.Types.Database.SqlStatement sqlBuilder)
	{
		sqlBuilder.Table = "main";
		sqlBuilder.Method = Metreos.Utilities.SqlBuilder.Method.SELECT;
		sqlBuilder.FieldNames.Add("name");
		sqlBuilder.FieldNames.Add("value");

		sqlBuilder.Where["name"] = "name1"; 

		return String.Empty;
	}
</Properties>
    </node>
    <node type="Variable" id="632277756578281520" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="dsn" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632277756578281526" name="resultSet" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.DataTable" refType="reference">resultSet</Properties>
    </node>
    <node type="Variable" id="632279240448281437" name="sqlBuilder" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Database.SqlStatement" refType="reference">sqlBuilder</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>