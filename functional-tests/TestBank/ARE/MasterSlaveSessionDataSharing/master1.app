<Application name="master1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="master1">
    <outline>
      <treenode type="evh" id="632471201480427703" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632230109687707581" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632230109687707580" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.MasterSlaveSessionDataSharing.master1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632471201480427704" level="1" text="Metreos.Providers.FunctionalTest.Event: OnEvent">
        <node type="function" name="OnEvent" id="632230109687707603" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632230109687707602" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">ARE.MasterSlaveSessionDataSharing.master1.E_Event</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Inserted" id="632580888381527976" vid="632230109687707585">
        <Properties type="Metreos.Types.String" initWith="S_Inserted">S_Inserted</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632230109687707583" treenode="632471201480427703" appnode="632230109687707581" handlerfor="632230109687707580">
    <node type="Start" id="632230109687707583" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="354">
      <linkto id="632236683529062713" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632230109687707598" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="283" y="352">
      <linkto id="632230109687707600" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
        <log condition="exit" on="true" level="Info" type="literal">Inserted into db</log>
	public static string Execute(SessionData sessionData)
	{
IDbConnection connection = sessionData.DbConnections["database1"]; 
connection.Open();
IDbCommand command = connection.CreateCommand();
command.CommandText = "insert into sample (samplekey, sampledata) values(1, '1');"; 

command.ExecuteNonQuery(); 
command.Dispose(); 
connection.Close();
return "success"; 


	}
</Properties>
    </node>
    <node type="Action" id="632230109687707599" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="662" y="354">
      <linkto id="632230109687707601" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Inserted</ap>
      </Properties>
    </node>
    <node type="Action" id="632230109687707600" name="EnableScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="460" y="348">
      <linkto id="632230109687707599" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="ScriptName" type="literal">slave1</ap>
        <log condition="entry" on="true" level="Info" type="literal">Start EnableScript action</log>
        <log condition="exit" on="true" level="Info" type="literal">Stop EnableScript action</log>
      </Properties>
    </node>
    <node type="Action" id="632230109687707601" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="836" y="356">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632236683529062713" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="122" y="354">
      <linkto id="632230109687707598" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="literal">Hum....</ap>
        <ap name="LogLevel" type="literal">Info</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent" startnode="632230109687707605" treenode="632471201480427704" appnode="632230109687707603" handlerfor="632230109687707602">
    <node type="Start" id="632230109687707605" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632230109687707642" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632230109687707642" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="602" y="272">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>