<Application name="PushCallStats" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="PushCallStats">
    <outline>
      <treenode type="evh" id="632516905419448599" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632516905419448596" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632516905419448595" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/PushCallStats</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632516905419448632" level="2" text="Metreos.Providers.Http.GotRequest: OnGotRequest1">
        <node type="function" name="OnGotRequest1" id="632516905419448629" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632516905419448628" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/RefreshCallStats</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_randomCallQueueCount" id="632517020544186312" vid="632516905419448616">
        <Properties type="Int">g_randomCallQueueCount</Properties>
      </treenode>
      <treenode text="g_randomLongestStartTime" id="632517020544186314" vid="632516905419448619">
        <Properties type="DateTime">g_randomLongestStartTime</Properties>
      </treenode>
      <treenode text="g_refreshRate" id="632517020544186316" vid="632516905419448665">
        <Properties type="String" initWith="refreshRate">g_refreshRate</Properties>
      </treenode>
      <treenode text="g_appServerIp" id="632517020544186318" vid="632516987579207068">
        <Properties type="String" initWith="appServerIp">g_appServerIp</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632516905419448598" treenode="632516905419448599" appnode="632516905419448596" handlerfor="632516905419448595">
    <node type="Start" id="632516905419448598" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="352">
      <linkto id="632516905419448618" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632516905419448618" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="144" y="352">
      <linkto id="632516905419448622" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="csharp">"host: " + host</log>
        <log condition="exit" on="true" level="Info" type="csharp">"Refresh URL: " + refreshUrl</log>
public static string Execute(ref int g_randomCallQueueCount, ref DateTime g_randomLongestStartTime, ref string formattedText, ref string refreshUrl, string g_appServerIp, string routingGuid, LogWriter log)
{
	System.Random rand = new Random();
          g_randomCallQueueCount = Convert.ToInt32(rand.Next(0, 6));

	int minutesInPast = Convert.ToInt32(rand.Next(3, 25));
	int secondsInPast = Convert.ToInt32(rand.Next(0, 60));

	g_randomLongestStartTime = DateTime.Now.Subtract(new TimeSpan(0, 0, minutesInPast, secondsInPast , 0));

	TimeSpan waitTime = DateTime.Now.Subtract(g_randomLongestStartTime);

      string secondsFormatted = waitTime.Seconds &lt; 10 ? "0" + waitTime.Seconds : waitTime.Seconds.ToString();

	formattedText = String.Format("{0} {3} in Queue\n\nLongest Wait Time {1}:{2}", g_randomCallQueueCount, waitTime.Minutes, secondsFormatted, g_randomCallQueueCount == 1 ? "Caller" : "Callers");

	refreshUrl = "http://" + g_appServerIp + ":8000/RefreshCallStats?metreosSessionId=" + routingGuid;
	log.Write(TraceLevel.Info, "Refresh URL: " + refreshUrl);

	return "";
}
</Properties>
    </node>
    <node type="Action" id="632516905419448622" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="264" y="352">
      <linkto id="632516905419448624" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Title" type="literal">Call Stats</ap>
        <ap name="Prompt" type="literal">Options</ap>
        <ap name="Text" type="variable">formattedText</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632516905419448624" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="416" y="352">
      <linkto id="632516905419448626" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Refresh</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="variable">refreshUrl</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632516905419448626" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="560" y="352">
      <linkto id="632516905419448634" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632516905419448634" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="688" y="352">
      <linkto id="632516905419448718" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Refresh" type="csharp">String.Format("{0}; URL={1}", g_refreshRate, refreshUrl);</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632516905419448718" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="840" y="352">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632516905419448600" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">queryParams</Properties>
    </node>
    <node type="Variable" id="632516905419448601" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632516905419448602" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632516905419448603" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632516905419448621" name="formattedText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">formattedText</Properties>
    </node>
    <node type="Variable" id="632516905419448623" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" initWith="RoutingGuid" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632516905419448625" name="refreshUrl" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">refreshUrl</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotRequest1" activetab="true" startnode="632516905419448631" treenode="632516905419448632" appnode="632516905419448629" handlerfor="632516905419448628">
    <node type="Start" id="632516905419448631" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="360">
      <linkto id="632516938773896185" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632516938773896185" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="141.90625" y="360">
      <linkto id="632516938773896193" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref int g_randomCallQueueCount, ref DateTime g_randomLongestStartTime, ref string formattedText, ref string refreshUrl2, string g_appServerIp, string routingGuid)
{
	System.Random rand = new Random();
	int rollDiceForRandomQueueLeave = Convert.ToInt32(rand.Next(0, 100));
	// default rate is 12 requests a minute. If we have 9% chance, then chance of leave is 1 - 91% ^ 12,

      TimeSpan waitTime = DateTime.Now.Subtract(g_randomLongestStartTime);

	bool decremented = false;
	if(g_randomCallQueueCount &gt; 0 &amp;&amp; rollDiceForRandomQueueLeave &lt; 9)
	{
          	g_randomCallQueueCount--; 	
		decremented = true;
	}

	// Random queue join
      int rollDiceForRandomQueueJoin = Convert.ToInt32(rand.Next(0, 100));
	// default rate is 12 requests a minute. If we have 9% chance, then chance of leave is 1 - 91% ^ 12

   	if(rollDiceForRandomQueueJoin &lt; 9)
	{
          	g_randomCallQueueCount++; 	
	}

      if(g_randomCallQueueCount == 0 &amp;&amp; decremented)
	{
			int mins = Convert.ToInt32(rand.Next(0, 3));
			int secs = Convert.ToInt32(rand.Next(0, 60));
			waitTime = new TimeSpan(0, 0, mins, secs, 0);
	}





	
	
	
	string secondsFormatted = waitTime.Seconds &lt; 10 ? "0" + waitTime.Seconds : waitTime.Seconds.ToString();
	formattedText = String.Format("{0} {3} in Queue\n\nLongest Wait Time {1}:{2}", g_randomCallQueueCount, waitTime.Minutes, secondsFormatted, g_randomCallQueueCount == 1 ? "Caller" : "Callers");

		refreshUrl2 = "http://" + g_appServerIp + ":8000/RefreshCallStats?metreosSessionId=" + routingGuid;
	return "";
}
</Properties>
    </node>
    <node type="Action" id="632516938773896193" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="249.975372" y="359">
      <linkto id="632516938773896194" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Title" type="literal">Call Stats</ap>
        <ap name="Prompt" type="literal">Options</ap>
        <ap name="Text" type="variable">formattedText</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632516938773896194" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="401.975342" y="359">
      <linkto id="632516938773896195" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Refresh</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="variable">refreshUrl2</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632516938773896195" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="545.975342" y="359">
      <linkto id="632516938773896196" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632516938773896196" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="673.9752" y="359">
      <linkto id="632516938773896197" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Refresh" type="csharp">String.Format("{0}; URL={1}", g_refreshRate, refreshUrl2);</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632516938773896197" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="825.9752" y="359">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632516938773896187" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632516938773896188" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632516938773896189" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632516938773896190" name="refreshUrl2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">refreshUrl2</Properties>
    </node>
    <node type="Variable" id="632516938773896191" name="formattedText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">formattedText</Properties>
    </node>
    <node type="Variable" id="632516938773896192" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" initWith="" refType="reference">text</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>