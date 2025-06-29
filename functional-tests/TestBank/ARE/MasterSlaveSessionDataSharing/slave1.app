<Application name="slave1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="slave1">
    <outline>
      <treenode type="evh" id="632230109687707613" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632230109687707610" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632230109687707609" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.MasterSlaveSessionDataSharing.slave1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_QuerySuccess" id="632580888381527992" vid="632230109687707618">
        <Properties type="Metreos.Types.String" initWith="S_QuerySuccess">S_QuerySuccess</Properties>
      </treenode>
      <treenode text="S_QueryFailure" id="632580888381527994" vid="632230109687707620">
        <Properties type="Metreos.Types.String" initWith="S_QueryFailure">S_QueryFailure</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632230109687707612" treenode="632230109687707613" appnode="632230109687707610" handlerfor="632230109687707609">
    <node type="Start" id="632230109687707612" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="380">
      <linkto id="632230109687707614" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632230109687707614" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="240" y="382">
      <linkto id="632230109687707616" type="Labeled" style="Bevel" label="valuebad" />
      <linkto id="632230109687707615" type="Labeled" style="Bevel" label="idbad" />
      <linkto id="632230109687707617" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
        <log condition="exit" on="true" level="Info" type="literal">Queried db</log>
	public static string Execute(SessionData sessionData)
	{
		IDbConnection connection = sessionData.DbConnections["database1"]; 
		connection.Open();
		IDbCommand command = connection.CreateCommand(); 
		command.CommandText = "select * from sample;"; 
		IDataReader reader = command.ExecuteReader(); 

		if(!reader.Read()) 
		{
			connection.Close();
			return "failure"; 
		}

		int id = reader.GetInt16(0); 

		if(id != 1) 
		{
			connection.Close();
			return "idbad";
		} 

            string dataValue = reader.GetString(1); 
		if(dataValue != "1") 
		{
			connection.Close();
			return "valuebad"; 
		}

		command.Dispose(); 
		reader.Close(); 
		connection.Close();

		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632230109687707615" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="307" y="604">
      <linkto id="632230109687707622" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="literal">idbad</ap>
        <ap name="LogLevel" type="literal">Error</ap>
      </Properties>
    </node>
    <node type="Action" id="632230109687707616" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="413" y="537">
      <linkto id="632230109687707622" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="literal">value bad</ap>
        <ap name="LogLevel" type="literal">Error</ap>
      </Properties>
    </node>
    <node type="Action" id="632230109687707617" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="533" y="385">
      <linkto id="632230109687707623" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_QuerySuccess</ap>
      </Properties>
    </node>
    <node type="Action" id="632230109687707622" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="534" y="536">
      <linkto id="632230109687707623" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_QueryFailure</ap>
      </Properties>
    </node>
    <node type="Action" id="632230109687707623" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="730" y="453">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="slave" instanceType="multiInstance" desc="">
  </Properties>
</Application>