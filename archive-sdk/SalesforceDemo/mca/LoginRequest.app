<Application name="LoginRequest" trigger="Metreos.Providers.SalesforceDemo.LoginRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="LoginRequest">
    <outline>
      <treenode type="evh" id="632995636458482832" level="1" text="Metreos.Providers.SalesforceDemo.LoginRequest (trigger): OnLoginRequest">
        <node type="function" name="OnLoginRequest" id="632995636458482829" path="Metreos.StockTools" />
        <node type="event" name="LoginRequest" id="632995636458482828" path="Metreos.Providers.SalesforceDemo.LoginRequest" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_ccmVersion" id="633071262887419301" vid="632995636458482851">
        <Properties type="String" initWith="CallManagerVersion">g_ccmVersion</Properties>
      </treenode>
      <treenode text="g_ccmIP" id="633071262887419303" vid="632995636458482868">
        <Properties type="String" initWith="CallManagerIP">g_ccmIP</Properties>
      </treenode>
      <treenode text="g_ccmUser" id="633071262887419305" vid="632995636458482870">
        <Properties type="String" initWith="CallManagerUser">g_ccmUser</Properties>
      </treenode>
      <treenode text="g_ccmPass" id="633071262887419307" vid="632995636458482872">
        <Properties type="String" initWith="CallManagerPass">g_ccmPass</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnLoginRequest" activetab="true" startnode="632995636458482831" treenode="632995636458482832" appnode="632995636458482829" handlerfor="632995636458482828">
    <node type="Loop" id="632995636458482881" name="Loop" text="loop (expr)" cx="314" cy="201" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="652" y="173" mx="809" my="274">
      <linkto id="632996096160252304" fromport="1" type="Basic" style="Vector" />
      <linkto id="632995636458482878" fromport="3" type="Labeled" style="Vector" label="default" />
      <Properties iteratorType="enum" type="csharp">phoneRes333.Response.@return.device.lines.Items</Properties>
    </node>
    <node type="Loop" id="632996472594914372" name="Loop" text="loop (expr)" cx="314" cy="201" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="645" y="421" mx="802" my="522">
      <linkto id="632996472594914377" fromport="1" type="Basic" style="Vector" />
      <linkto id="632995636458482878" fromport="3" type="Labeled" style="Vector" label="default" />
      <Properties iteratorType="enum" type="csharp">phoneRes413.Response.@return.device.lines.Items</Properties>
    </node>
    <node type="Loop" id="632996472594914378" name="Loop" text="loop (expr)" cx="322.969727" cy="199" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="649.9723" y="670" mx="811" my="770">
      <linkto id="632996472594914379" fromport="1" type="Basic" style="Vector" />
      <linkto id="632995636458482878" fromport="3" type="Labeled" style="Vector" label="default" />
      <Properties iteratorType="enum" type="csharp">phoneRes504.Response.@return.device.lines.Items</Properties>
    </node>
    <node type="Start" id="632995636458482831" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="72" y="527">
      <linkto id="632995636458482837" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632995636458482834" name="GetPhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="384" y="272">
      <linkto id="632995636458482885" type="Labeled" style="Vector" label="default" />
      <linkto id="633070572769829296" type="Labeled" style="Vector" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="variable">deviceName</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIP</ap>
        <ap name="AdminUsername" type="variable">g_ccmUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmPass</ap>
        <rd field="GetPhoneResponse">phoneRes333</rd>
      </Properties>
    </node>
    <node type="Action" id="632995636458482835" name="GetPhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="402" y="523">
      <linkto id="632996472594914384" type="Labeled" style="Vector" label="true" />
      <linkto id="633070572769829292" type="Labeled" style="Vector" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="variable">deviceName</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIP</ap>
        <ap name="AdminUsername" type="variable">g_ccmUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmPass</ap>
        <rd field="GetPhoneResponse">phoneRes413</rd>
      </Properties>
    </node>
    <node type="Action" id="632995636458482836" name="GetPhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap504" x="401" y="771">
      <linkto id="633070572769829289" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="variable">deviceName</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIP</ap>
        <ap name="AdminUsername" type="variable">g_ccmUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmPass</ap>
        <rd field="GetPhoneResponse">phoneRes504</rd>
      </Properties>
    </node>
    <node type="Action" id="632995636458482837" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="184" y="526">
      <linkto id="632995636458482834" type="Labeled" style="Vector" label="3.3" />
      <linkto id="632995636458482835" type="Labeled" style="Vector" label="4.1" />
      <linkto id="632995636458482836" type="Labeled" style="Vector" label="5.0" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">g_ccmVersion</ap>
      </Properties>
    </node>
    <node type="Action" id="632995636458482878" name="NotifyLogin" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="1189" y="524">
      <linkto id="632995636458482880" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="DeviceName" type="variable">deviceName</ap>
        <ap name="Lines" type="variable">lines</ap>
      </Properties>
    </node>
    <node type="Action" id="632995636458482880" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1309" y="524">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632995636458482883" text="Check for lines" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="446" y="113" />
    <node type="Label" id="632995636458482884" text="n" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="463" y="154" />
    <node type="Action" id="632995636458482885" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="520" y="272">
      <linkto id="632995636458482884" type="Labeled" style="Vector" label="0" />
      <linkto id="632995636458482881" port="1" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
	public static string Execute(Metreos.Types.AxlSoap333.GetPhoneResponse phoneRes333, ref int numLines)
	{
		numLines = 0;

		// if we can find no line information, then this device must have
            // no lines, and so is not useful for this demo.
		
		if(phoneRes333 != null &amp;&amp;
		   phoneRes333.Response != null &amp;&amp;
		   phoneRes333.Response.@return != null &amp;&amp;
               phoneRes333.Response.@return.device != null &amp;&amp;
               phoneRes333.Response.@return.device.lines != null &amp;&amp;
               phoneRes333.Response.@return.device.lines.Items != null)
		{
                 numLines = phoneRes333.Response.@return.device.lines.Items.Length;
            }

		return numLines.ToString();
	}
</Properties>
    </node>
    <node type="Label" id="632995636458482887" text="n" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1261" y="388">
      <linkto id="632995636458482878" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632995636458482889" name="CustomCode" container="632995636458482881" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="862" y="277">
      <linkto id="632995636458482881" port="3" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.AxlSoap333.GetLineResponse lineRes333, ArrayList lines, IEnumerator loopEnum, LogWriter log, string deviceName)
{
	Metreos.AxlSoap333.XLine xLine = loopEnum.Current as Metreos.AxlSoap333.XLine;
	
	string pattern = null;

	if(lineRes333 != null &amp;&amp;
		lineRes333.Response.@return != null &amp;&amp;
		lineRes333.Response.@return.directoryNumber != null)
	{
		pattern = lineRes333.Response.@return.directoryNumber.pattern;

		lines.Add(pattern);

		log.Write(TraceLevel.Verbose, "Found line DN {0} on phone {1}", pattern, deviceName );
 
		return "success";
	}
	else
	{
		return "failure";
	}
}
</Properties>
    </node>
    <node type="Action" id="632996096160252304" name="GetLine" container="632995636458482881" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap333" x="759" y="271">
      <linkto id="632995636458482889" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Uuid" type="csharp">(loopEnum.Current as Metreos.AxlSoap333.XLine).Item.uuid</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIP</ap>
        <ap name="AdminUsername" type="variable">g_ccmUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmPass</ap>
        <rd field="GetLineResponse">lineRes333</rd>
      </Properties>
    </node>
    <node type="Action" id="632996472594914373" name="CustomCode" container="632996472594914372" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="868" y="520">
      <linkto id="632996472594914372" port="3" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.AxlSoap413.GetLineResponse lineRes413, ArrayList lines, IEnumerator loopEnum, LogWriter log, string deviceName)
{
	Metreos.AxlSoap413.XLine xLine = loopEnum.Current as Metreos.AxlSoap413.XLine;
	
	string pattern = null;

	if(lineRes413 != null &amp;&amp;
		lineRes413.Response.@return != null &amp;&amp;
		lineRes413.Response.@return.directoryNumber != null)
	{
		pattern = lineRes413.Response.@return.directoryNumber.pattern;

		lines.Add(pattern);

		log.Write(TraceLevel.Verbose, "Found line DN {0} on phone {1}", pattern, deviceName );
 
		return "success";
	}
	else
	{
		return "failure";
	}
}
</Properties>
    </node>
    <node type="Action" id="632996472594914377" name="GetLine" container="632996472594914372" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="758.9723" y="520">
      <linkto id="632996472594914373" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Uuid" type="csharp">(loopEnum.Current as Metreos.AxlSoap413.XLine).Item.uuid</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIP</ap>
        <ap name="AdminUsername" type="variable">g_ccmUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmPass</ap>
        <rd field="GetLineResponse">lineRes413</rd>
      </Properties>
    </node>
    <node type="Action" id="632996472594914379" name="GetLine" container="632996472594914378" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap504" x="756.9723" y="766">
      <linkto id="632996472594914380" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Uuid" type="csharp">(loopEnum.Current as Metreos.AxlSoap504.XLine).Item.uuid</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIP</ap>
        <ap name="AdminUsername" type="variable">g_ccmUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmPass</ap>
        <rd field="GetLineResponse">lineRes504</rd>
      </Properties>
    </node>
    <node type="Action" id="632996472594914380" name="CustomCode" container="632996472594914378" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="867" y="767">
      <linkto id="632996472594914378" port="3" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.AxlSoap504.GetLineResponse lineRes504, ArrayList lines, IEnumerator loopEnum, LogWriter log, string deviceName)
{
	Metreos.AxlSoap504.XLine xLine = loopEnum.Current as Metreos.AxlSoap504.XLine;
	
	string pattern = null;

	if(lineRes504 != null &amp;&amp;
		lineRes504.Response.@return != null &amp;&amp;
		lineRes504.Response.@return.directoryNumber != null)
	{
		pattern = lineRes504.Response.@return.directoryNumber.pattern;

		lines.Add(pattern);

		log.Write(TraceLevel.Verbose, "Found line DN {0} on phone {1}", pattern, deviceName );
 
		return "success";
	}
	else
	{
		return "failure";
	}
}
</Properties>
    </node>
    <node type="Comment" id="632996472594914382" text="Check for lines" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="481" y="346" />
    <node type="Label" id="632996472594914383" text="n" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="507" y="387" />
    <node type="Action" id="632996472594914384" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="572" y="521">
      <linkto id="632996472594914383" type="Labeled" style="Vector" label="0" />
      <linkto id="632996472594914372" port="1" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
	public static string Execute(Metreos.Types.AxlSoap413.GetPhoneResponse phoneRes413, ref int numLines)
	{
		numLines = 0;

		// if we can find no line information, then this device must have
            // no lines, and so is not useful for this demo.
		
		if(phoneRes413 != null &amp;&amp;
		   phoneRes413.Response != null &amp;&amp;
		   phoneRes413.Response.@return != null &amp;&amp;
               phoneRes413.Response.@return.device != null &amp;&amp;
               phoneRes413.Response.@return.device.lines != null &amp;&amp;
               phoneRes413.Response.@return.device.lines.Items != null)
		{
                 numLines = phoneRes413.Response.@return.device.lines.Items.Length;
            }

		return numLines.ToString();
	}
</Properties>
    </node>
    <node type="Comment" id="632996472594914388" text="Check for lines" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="491" y="607" />
    <node type="Label" id="632996472594914389" text="n" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="508" y="648" />
    <node type="Action" id="632996472594914390" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="563" y="773">
      <linkto id="632996472594914389" type="Labeled" style="Vector" label="0" />
      <linkto id="632996472594914378" port="1" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
	public static string Execute(Metreos.Types.AxlSoap504.GetPhoneResponse phoneRes504, ref int numLines)
	{
		numLines = 0;

		// if we can find no line information, then this device must have
            // no lines, and so is not useful for this demo.
		if(phoneRes504 != null &amp;&amp;
		   phoneRes504.Response != null &amp;&amp;
		   phoneRes504.Response.@return != null &amp;&amp;
               phoneRes504.Response.@return.device != null &amp;&amp;
               phoneRes504.Response.@return.device.lines != null &amp;&amp;
               phoneRes504.Response.@return.device.lines.Items != null)
            {
                 numLines = phoneRes504.Response.@return.device.lines.Items.Length;
            }

		return numLines.ToString();
	}
</Properties>
    </node>
    <node type="Action" id="633070572769829289" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="482" y="772">
      <linkto id="632996472594914390" type="Labeled" style="Bezier" label="true" />
      <linkto id="633070572769829290" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">phoneRes504 != null</ap>
      </Properties>
    </node>
    <node type="Label" id="633070572769829290" text="n" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="460" y="652" />
    <node type="Action" id="633070572769829292" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="439" y="441">
      <linkto id="633070572769829293" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">phoneRes413 != null</ap>
      </Properties>
    </node>
    <node type="Label" id="633070572769829293" text="n" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="417" y="321" />
    <node type="Action" id="633070572769829296" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="287" y="289">
      <linkto id="633070572769829297" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">phoneRes333 != null</ap>
      </Properties>
    </node>
    <node type="Label" id="633070572769829297" text="n" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="265" y="169" />
    <node type="Variable" id="632995636458482833" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.SalesforceDemo.LoginRequest">deviceName</Properties>
    </node>
    <node type="Variable" id="632995636458482874" name="phoneRes333" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap333.GetPhoneResponse" refType="reference">phoneRes333</Properties>
    </node>
    <node type="Variable" id="632995636458482875" name="phoneRes413" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.GetPhoneResponse" refType="reference">phoneRes413</Properties>
    </node>
    <node type="Variable" id="632995636458482876" name="phoneRes504" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap504.GetPhoneResponse" refType="reference">phoneRes504</Properties>
    </node>
    <node type="Variable" id="632995636458482879" name="lines" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="ArrayList" refType="reference">lines</Properties>
    </node>
    <node type="Variable" id="632995636458482886" name="numLines" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">numLines</Properties>
    </node>
    <node type="Variable" id="632996096160252305" name="lineRes333" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap333.GetLineResponse" refType="reference">lineRes333</Properties>
    </node>
    <node type="Variable" id="632996096160252306" name="lineRes413" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.GetLineResponse" refType="reference">lineRes413</Properties>
    </node>
    <node type="Variable" id="632996096160252307" name="lineRes504" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap504.GetLineResponse" refType="reference">lineRes504</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>