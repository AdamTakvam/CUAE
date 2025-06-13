<Application name="script2" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="script2">
    <outline>
      <treenode type="evh" id="632834840330577211" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632834840330577208" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632834840330577207" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/performwalk</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_user" id="632834840330577729" vid="632834840330577510">
        <Properties type="String" initWith="user">g_user</Properties>
      </treenode>
      <treenode text="g_pass" id="632834840330577731" vid="632834840330577512">
        <Properties type="String" initWith="pass">g_pass</Properties>
      </treenode>
      <treenode text="g_delay" id="632834840330577733" vid="632834840330577514">
        <Properties type="Int" initWith="interCommandDelay">g_delay</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632834840330577210" treenode="632834840330577211" appnode="632834840330577208" handlerfor="632834840330577207">
    <node type="Loop" id="632834840330577360" name="Loop" text="loop (var)" cx="400.36" cy="235" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="688" y="32" mx="888" my="150">
      <linkto id="632834840330577363" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632834840330577516" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="variable">commands</Properties>
    </node>
    <node type="Start" id="632834840330577210" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632834840330577365" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632834840330577361" name="SendExecute" container="632834840330577360" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="960" y="128">
      <linkto id="632834840330577360" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">phoneIp</ap>
        <ap name="Username" type="variable">g_user</ap>
        <ap name="Password" type="variable">g_pass</ap>
      </Properties>
    </node>
    <node type="Action" id="632834840330577362" name="CreateExecute" container="632834840330577360" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="888" y="208">
      <linkto id="632834840330577361" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">loopEnum.Current as string</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632834840330577363" name="If" container="632834840330577360" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="776" y="136">
      <linkto id="632834840330577364" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632834840330577362" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">loopEnum.Current as string == ","</ap>
      </Properties>
    </node>
    <node type="Action" id="632834840330577364" name="Sleep" container="632834840330577360" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="880" y="80">
      <linkto id="632834840330577360" port="2" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="variable">g_delay</ap>
      </Properties>
    </node>
    <node type="Action" id="632834840330577365" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="152" y="160">
      <linkto id="632834840330577366" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632834840330577368" type="Labeled" style="Bezier" ortho="true" label="nodevice" />
      <linkto id="632834840330577369" type="Labeled" style="Bezier" ortho="true" label="nocommand" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Http.FormCollection query, ref string phoneIp, ArrayList commands)
{
	if( (query["phoneName"] == null || query["phoneName"] == "") &amp;&amp; (query["phoneIP"] == null &amp;&amp; query["phoneIP"] == ""))
	{
		// no device specified!
		return "nodevice";
	}

	if(query["command"] == null || query["command"] == "")
	{
		// no command specified!
		return "nocommand";
	}
	
	if(query["phoneIP"] != null &amp;&amp; query["phoneIP"] != "")
	{
		phoneIp = query["phoneIP"];
	}

	// Parse command
	
	System.IO.StringReader reader = new System.IO.StringReader(query["command"]);
	while(reader.Peek() &gt; -1)
	{
		string line = reader.ReadLine();
		if(line == "" || line == null) continue;
		if(line.StartsWith(","))
		{
			foreach(char character in line)
			{
				if(character == ',')
				{
					commands.Add(",");
				}
			}
		}
		commands.Add(line);
	}
	return "success";

}
</Properties>
    </node>
    <node type="Action" id="632834840330577366" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="255.360016" y="160">
      <linkto id="632834840330577367" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632834840330577519" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">phoneIp == null || phoneIp == ""</ap>
      </Properties>
    </node>
    <node type="Action" id="632834840330577367" name="QueryByDevice" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="335.360016" y="344">
      <linkto id="632834840330577371" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DeviceName" type="csharp">query["device"]</ap>
        <rd field="ResultData">results</rd>
      </Properties>
    </node>
    <node type="Action" id="632834840330577368" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="79.3600159" y="312">
      <linkto id="632834840330577370" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="literal">No Device Specified (specify ?ip= or ?device=)</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632834840330577369" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="200" y="320">
      <linkto id="632834840330577370" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="literal">No Command Specified (specify ?command=)</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632834840330577370" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="143.360016" y="448">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632834840330577371" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="471.360016" y="344">
      <linkto id="632834840330577372" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632834840330577373" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">results.Rows.Count &gt; 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632834840330577372" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="607.36" y="352">
      <linkto id="632834840330577519" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">results.Rows[0]["IP"]</ap>
        <rd field="ResultData">phoneIp</rd>
      </Properties>
    </node>
    <node type="Action" id="632834840330577373" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="471.360016" y="432">
      <linkto id="632834840330577374" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">"The device " + query["device"] + " could not be found in real-time cache"</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632834840330577374" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="471.360016" y="544">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632834840330577516" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1152" y="264">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632834840330577519" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="600" y="184">
      <linkto id="632834840330577360" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Set-Cookie" type="csharp">"phoneWalkerCookie=" + query["command"] + "|" + query["phoneIP"] + "|" + query["phoneName"]</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"&lt;html&gt;&lt;body&gt;&lt;p&gt;&lt;blink&gt;Starting&lt;/blink&gt;&lt;/p&gt;&lt;/body&gt;&lt;/html&gt;"</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Variable" id="632834840330577389" name="phoneIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">phoneIp</Properties>
    </node>
    <node type="Variable" id="632834840330577390" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.FormCollection" initWith="body" refType="reference" name="Metreos.Providers.Http.GotRequest">query</Properties>
    </node>
    <node type="Variable" id="632834840330577391" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632834840330577509" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="632834840330577518" name="commands" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="ArrayList" refType="reference">commands</Properties>
    </node>
    <node type="Variable" id="632834840330577578" name="results" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">results</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>