<Application name="BotCommand" trigger="Metreos.Providers.JabberProvider.BotCommand" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="BotCommand">
    <outline>
      <treenode type="evh" id="632988933569454120" level="1" text="Metreos.Providers.JabberProvider.BotCommand (trigger): OnBotCommand">
        <node type="function" name="OnBotCommand" id="632988933569454117" path="Metreos.StockTools" />
        <node type="event" name="BotCommand" id="632988933569454116" path="Metreos.Providers.JabberProvider.BotCommand" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnBotCommand" activetab="true" startnode="632988933569454119" treenode="632988933569454120" appnode="632988933569454117" handlerfor="632988933569454116">
    <node type="Start" id="632988933569454119" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="106" y="438">
      <linkto id="632988942644862473" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632988933569454125" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="405" y="441">
      <linkto id="632988942644862475" type="Labeled" style="Vector" label="help" />
      <linkto id="632988942644862476" type="Labeled" style="Vector" label="unknown" />
      <linkto id="632989238027940791" type="Labeled" style="Vector" label="remind" />
      <linkto id="632989238027940840" type="Labeled" style="Vector" label="pwn" />
      <Properties language="csharp">
public static string Execute(string command)
{
	command = command.ToLower();

	if(command.StartsWith("help"))
	{
		return "help";
	}
	if(command.StartsWith("remind"))
	{
		return "remind";
	}
	if(command.StartsWith("pwn"))
	{
		return "pwn";
	}

	return "unknown";
}
</Properties>
    </node>
    <node type="Action" id="632988942644862473" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="248" y="438">
      <linkto id="632988942644862474" type="Labeled" style="Vector" label="default" />
      <linkto id="632988933569454125" type="Labeled" style="Vector" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">delay</ap>
      </Properties>
    </node>
    <node type="Action" id="632988942644862474" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="245" y="641">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632988942644862475" name="SendChat" class="MaxActionNode" group="" path="Metreos.Providers.JabberProvider" x="606" y="445">
      <linkto id="632988942644862478" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Body" type="literal">Valid commands /help /remind /pwn</ap>
      </Properties>
    </node>
    <node type="Action" id="632988942644862476" name="SendChat" class="MaxActionNode" group="" path="Metreos.Providers.JabberProvider" x="566" y="602">
      <linkto id="632988942644862478" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Body" type="literal">Unknown command</ap>
      </Properties>
    </node>
    <node type="Action" id="632988942644862478" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="936" y="567">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632989238027940791" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="651" y="175">
      <linkto id="632989238027940794" type="Labeled" style="Vector" label="set" />
      <linkto id="632989238027940792" type="Labeled" style="Vector" label="malformed" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.JabberProvider.Tokenizer tokens, ref string reminderMessage, ref int reminderMinutes)
{
	if(tokens.Tokens.Length == 1)
	{
		// No minutes to remind!
		return "malformed";
	}
	else
	{
		string minutesStr = tokens.Tokens[1];

		try
		{
			reminderMinutes = Int32.Parse(minutesStr);
		}
		catch
		{
			return "malformed";
		}

		if(tokens.Tokens.Length &gt; 2)
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			for(int i = 2; i &lt; tokens.Tokens.Length; i++)
			{
				builder.Append(tokens.Tokens[i]);
				builder.Append(" ");
			}

			reminderMessage = builder.ToString();
		}
		else
		{
			reminderMessage = "Hey! Do something after " + reminderMinutes + " ago";
		}
		

		return "set";
	}

}
</Properties>
    </node>
    <node type="Action" id="632989238027940792" name="SendChat" class="MaxActionNode" group="" path="Metreos.Providers.JabberProvider" x="828" y="51">
      <linkto id="632988942644862478" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Body" type="literal">/remind INT_MINUTES All_follows_becomes_the_reminder</ap>
      </Properties>
    </node>
    <node type="Action" id="632989238027940794" name="SendChat" class="MaxActionNode" group="" path="Metreos.Providers.JabberProvider" x="738" y="294">
      <linkto id="632989238027940796" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Body" type="csharp">"Reminder set for " + nick</ap>
      </Properties>
    </node>
    <node type="Action" id="632989238027940796" name="AddTriggerTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="810" y="393">
      <linkto id="632988942644862478" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerDateTime" type="csharp">DateTime.Now.AddMinutes(reminderMinutes)</ap>
        <ap name="timerUserData" type="csharp">"jabber:" + fullNick + ":" + reminderMessage</ap>
      </Properties>
    </node>
    <node type="Action" id="632989238027940839" name="SendChat" class="MaxActionNode" group="" path="Metreos.Providers.JabberProvider" x="503" y="756">
      <linkto id="632988942644862478" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Body" type="csharp">"Unholy battle is wrought upon " + nickToPwn + " in the form of pestulence, chaos, and a nasty-chat-a-gram"</ap>
      </Properties>
    </node>
    <node type="Action" id="632989238027940840" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="411" y="585">
      <linkto id="632989238027940839" type="Labeled" style="Vector" label="set" />
      <linkto id="632989238027940843" type="Labeled" style="Vector" label="malformed" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.JabberProvider.Tokenizer tokens, ref string nickToPwn)
{
	if(tokens.Tokens.Length == 1)
	{
		// No pwn message!
		return "malformed";
	}
	else
	{
		nickToPwn = tokens.Tokens[1];
	
		return "set";
	}
}
</Properties>
    </node>
    <node type="Action" id="632989238027940843" name="SendChat" class="MaxActionNode" group="" path="Metreos.Providers.JabberProvider" x="356" y="769.369263">
      <linkto id="632988942644862478" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Body" type="literal">/pwn nick_to_pwn</ap>
      </Properties>
    </node>
    <node type="Variable" id="632988933569454121" name="nick" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Nick" defaultInitWith="NO_NICK" refType="reference" name="Metreos.Providers.JabberProvider.BotCommand">nick</Properties>
    </node>
    <node type="Variable" id="632988933569454122" name="type" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Type" refType="reference" name="Metreos.Providers.JabberProvider.BotCommand">type</Properties>
    </node>
    <node type="Variable" id="632988933569454123" name="command" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Command" defaultInitWith="NO_COMMAND" refType="reference" name="Metreos.Providers.JabberProvider.BotCommand">command</Properties>
    </node>
    <node type="Variable" id="632988942644862472" name="delay" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" initWith="Delay" refType="reference" name="Metreos.Providers.JabberProvider.BotCommand">delay</Properties>
    </node>
    <node type="Variable" id="632989236432959144" name="fullNick" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="FullNick" refType="reference" name="Metreos.Providers.JabberProvider.BotCommand">fullNick</Properties>
    </node>
    <node type="Variable" id="632989238027940790" name="tokens" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.JabberProvider.Tokenizer" initWith="Command" refType="reference" name="Metreos.Providers.JabberProvider.BotCommand">tokens</Properties>
    </node>
    <node type="Variable" id="632989238027940797" name="reminderMinutes" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">reminderMinutes</Properties>
    </node>
    <node type="Variable" id="632989238027940798" name="reminderMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">reminderMessage</Properties>
    </node>
    <node type="Variable" id="632989238027940842" name="nickToPwn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">nickToPwn</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>