<Application name="ReminderTimer" trigger="Metreos.Providers.TimerFacility.TimerFire" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="ReminderTimer">
    <outline>
      <treenode type="evh" id="632989238027940805" level="1" text="Metreos.Providers.TimerFacility.TimerFire (trigger): OnTimerFire">
        <node type="function" name="OnTimerFire" id="632989238027940802" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632989238027940801" path="Metreos.Providers.TimerFacility.TimerFire" trigger="true" />
        <Properties type="hybrid">
          <ep name="timerUserData" type="literal">regex:jabber</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTimerFire" activetab="true" startnode="632989238027940804" treenode="632989238027940805" appnode="632989238027940802" handlerfor="632989238027940801">
    <node type="Start" id="632989238027940804" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="224" y="447">
      <linkto id="632989238027940807" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632989238027940807" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="429" y="451">
      <linkto id="632989238027940810" type="Labeled" style="Vector" label="good" />
      <linkto id="632989238027940811" type="Labeled" style="Vector" label="bad" />
      <Properties language="csharp">
public static string Execute(string userData, ref string fullNick, ref string reminderMessage)
{
	string[] tokens = userData.Split(new char[] {':'});

	if(tokens.Length &lt; 3)
	{
		return "bad";
	}
	else
	{
		fullNick = tokens[1];
		reminderMessage = tokens[2];
		return "good";
	}
}
</Properties>
    </node>
    <node type="Action" id="632989238027940810" name="SendChat" class="MaxActionNode" group="" path="Metreos.Providers.JabberProvider" x="631" y="313">
      <linkto id="632989238027940812" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Body" type="variable">reminderMessage</ap>
      </Properties>
    </node>
    <node type="Action" id="632989238027940811" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="622" y="642">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="csharp">"Unable to parse command from timer: " + userData</log>
      </Properties>
    </node>
    <node type="Action" id="632989238027940812" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="897" y="307">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632989238027940806" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="timerUserData" refType="reference" name="Metreos.Providers.TimerFacility.TimerFire">userData</Properties>
    </node>
    <node type="Variable" id="632989238027940808" name="fullNick" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">fullNick</Properties>
    </node>
    <node type="Variable" id="632989238027940809" name="reminderMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">reminderMessage</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>