<Application name="master1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="master1">
    <outline>
      <treenode type="evh" id="632229847309375151" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632229847309375148" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632229847309375147" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.SessionDataDatabaseCreation.master1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_SuccessInsert" id="632580888381528024" vid="632229847309375194">
        <Properties type="Metreos.Types.String" initWith="S_SuccessInsert">S_SuccessInsert</Properties>
      </treenode>
      <treenode text="S_FailureInsert" id="632580888381528026" vid="632229847309375196">
        <Properties type="Metreos.Types.String" initWith="S_FailureInsert">S_FailureInsert</Properties>
      </treenode>
      <treenode text="S_SuccessQuery" id="632580888381528028" vid="632229878726093961">
        <Properties type="Metreos.Types.String" initWith="S_SuccessQuery">S_SuccessQuery</Properties>
      </treenode>
      <treenode text="S_FailureQuery" id="632580888381528030" vid="632229878726093963">
        <Properties type="Metreos.Types.String" initWith="S_FailureQuery">S_FailureQuery</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632229847309375150" treenode="632229847309375151" appnode="632229847309375148" handlerfor="632229847309375147">
    <node type="Start" id="632229847309375150" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="341">
      <linkto id="632229847309375152" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632229847309375152" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="348" y="336">
      <linkto id="632229847309375186" type="Labeled" style="Bevel" label="failure" />
      <linkto id="632229847309375188" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
	public static string Execute(SessionData sessionData)
	{
if(sessionData.DbConnections["database1"] == null)

return "failure";

IDbConnection connection = sessionData.DbConnections["database1"];
connection.Open();
IDbCommand command = connection.CreateCommand();

command.CommandText = "insert into sample (samplekey, sampledata) values(1, 'test');";
command.ExecuteNonQuery();

command.Dispose();
connection.Close();

return "success";
	}
</Properties>
    </node>
    <node type="Action" id="632229847309375186" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="346" y="482">
      <linkto id="632229847309375187" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_FailureInsert</ap>
      </Properties>
    </node>
    <node type="Action" id="632229847309375187" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="346" y="587">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632229847309375188" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="526" y="334">
      <linkto id="632229847309375189" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_SuccessInsert</ap>
      </Properties>
    </node>
    <node type="Action" id="632229847309375189" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="677" y="332">
      <linkto id="632229847309375190" type="Labeled" style="Bevel" label="default" />
      <linkto id="632229847309375191" type="Labeled" style="Bevel" label="idbad" />
      <linkto id="632229847309375192" type="Labeled" style="Bevel" label="valuebad" />
      <linkto id="632229847309375193" type="Labeled" style="Bevel" label="failure" />
      <Properties language="csharp">
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

int id = Convert.ToInt32(reader.GetValue(0));

if(id != 1)
{
connection.Close();
return "idbad";
}

string dataValue = reader.GetString(1);
if(dataValue != "test")
{
connection.Close();
return "valuebad";
}

command.Dispose();
reader.Close();
connection.Close();

return "success";
	}

</Properties>
    </node>
    <node type="Action" id="632229847309375190" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="799" y="128">
      <linkto id="632229847309375219" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_SuccessQuery</ap>
      </Properties>
    </node>
    <node type="Action" id="632229847309375191" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="795" y="259">
      <linkto id="632229847309375219" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="reason" type="literal">idbad</ap>
        <ap name="signalName" type="variable">S_FailureQuery</ap>
      </Properties>
    </node>
    <node type="Action" id="632229847309375192" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="798" y="483">
      <linkto id="632229847309375219" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="reason" type="literal">valuebad</ap>
        <ap name="signalName" type="variable">S_FailureQuery</ap>
      </Properties>
    </node>
    <node type="Action" id="632229847309375193" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="802" y="625">
      <linkto id="632229847309375219" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="reason" type="literal">dbfailure</ap>
        <ap name="signalName" type="variable">S_FailureQuery</ap>
      </Properties>
    </node>
    <node type="Action" id="632229847309375219" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="904.1992" y="378">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>