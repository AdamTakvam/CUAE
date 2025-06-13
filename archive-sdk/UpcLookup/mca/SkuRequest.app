<Application name="SkuRequest" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="SkuRequest">
    <outline>
      <treenode type="evh" id="632478085924325268" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632478085924325265" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632478085924325264" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632478085924325277" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632478085924325274" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632478085924325273" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632517157947274770" actid="632517157947274696" />
          <ref id="632517157947274803" actid="632479516327941386" />
          <ref id="632517157947274810" actid="632510619951044448" />
          <ref id="632517157947274815" actid="632517157947274704" />
          <ref id="632517157947274842" actid="632517157947274841" />
          <ref id="632517157947274845" actid="632517157947274844" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632478085924325282" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632478085924325279" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632478085924325278" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632517157947274771" actid="632517157947274696" />
          <ref id="632517157947274804" actid="632479516327941386" />
          <ref id="632517157947274811" actid="632510619951044448" />
          <ref id="632517157947274816" actid="632517157947274704" />
          <ref id="632517157947274843" actid="632517157947274841" />
          <ref id="632517157947274846" actid="632517157947274844" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632478085924325290" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632478085924325287" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632478085924325286" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632517157947274776" actid="632478085924325296" />
          <ref id="632517157947274783" actid="632517157947274684" />
          <ref id="632517157947274786" actid="632517157947274687" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632478085924325295" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632478085924325292" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632478085924325291" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632517157947274777" actid="632478085924325296" />
          <ref id="632517157947274784" actid="632517157947274684" />
          <ref id="632517157947274787" actid="632517157947274687" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632479532361371358" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632479532361371355" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632479532361371354" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_connectionId" id="632517157947274740" vid="632478085924325271">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_callId" id="632517157947274742" vid="632478085924325301">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_from_email_address" id="632517157947274744" vid="632479671795329419">
        <Properties type="String" initWith="FromEmailAddress">g_from_email_address</Properties>
      </treenode>
      <treenode text="g_dbUsername" id="632517157947274746" vid="632510619951044541">
        <Properties type="String" initWith="dbUsername">g_dbUsername</Properties>
      </treenode>
      <treenode text="g_dbPassword" id="632517157947274748" vid="632510619951044543">
        <Properties type="String" initWith="dbPassword">g_dbPassword</Properties>
      </treenode>
      <treenode text="g_dbHostName" id="632517157947274750" vid="632510619951044545">
        <Properties type="String" initWith="dbHostName">g_dbHostName</Properties>
      </treenode>
      <treenode text="g_smtpUsername" id="632517157947274752" vid="632510651629622474">
        <Properties type="String" defaultInitWith="NONE" initWith="smtpUserName">g_smtpUsername</Properties>
      </treenode>
      <treenode text="g_smtpPassword" id="632517157947274754" vid="632510651629622476">
        <Properties type="String" defaultInitWith="NONE" initWith="smtpPassword">g_smtpPassword</Properties>
      </treenode>
      <treenode text="g_smtpHostName" id="632517157947274756" vid="632510651629622478">
        <Properties type="String" initWith="smtpHostName">g_smtpHostName</Properties>
      </treenode>
      <treenode text="g_accountCode" id="632517157947274758" vid="632517157947274692">
        <Properties type="String">g_accountCode</Properties>
      </treenode>
      <treenode text="g_pin" id="632517157947274760" vid="632517157947274694">
        <Properties type="String">g_pin</Properties>
      </treenode>
      <treenode text="g_userId" id="632517157947274840" vid="632517157947274839">
        <Properties type="Int">g_userId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632478085924325267" treenode="632478085924325268" appnode="632478085924325265" handlerfor="632478085924325264">
    <node type="Start" id="632478085924325267" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="312">
      <linkto id="632510651629622480" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632478085924325269" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="496" y="304">
      <linkto id="632517157947274696" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DisplayName" type="literal">Sku Check</ap>
        <rd field="ConnectionId">g_connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632478085924325300" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="344" y="304">
      <linkto id="632478085924325269" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">callId</ap>
        <rd field="ResultData">g_callId</rd>
      </Properties>
    </node>
    <node type="Action" id="632478085924325303" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="824" y="304">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632479532361371350" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="184" y="312">
      <linkto id="632478085924325300" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632479532361371352" type="Labeled" style="Bezier" ortho="true" label="ending" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632479532361371352" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="290" y="430">
      <linkto id="632479532361371353" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632479532361371353" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="583" y="425">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632510651629622480" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="118" y="310">
      <linkto id="632479532361371350" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string g_smtpUsername, ref string g_smtpPassword)
{
	if(g_smtpUsername == "NONE") g_smtpUsername = String.Empty;
	if(g_smtpPassword == "NONE") g_smtpPassword = String.Empty;

	return String.Empty;
}
</Properties>
    </node>
    <node type="Action" id="632517157947274696" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="637.4707" y="293" mx="690" my="309">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632478085924325277" />
        <item text="OnPlay_Failed" treenode="632478085924325282" />
      </items>
      <linkto id="632478085924325303" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="literal">enter_acct.wav</ap>
        <ap name="Prompt2" type="literal">pound_sign.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">account_code</ap>
      </Properties>
    </node>
    <node type="Variable" id="632478085924325270" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632479532361371351" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632478085924325276" treenode="632478085924325277" appnode="632478085924325274" handlerfor="632478085924325273">
    <node type="Start" id="632478085924325276" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="280">
      <linkto id="632479722911415648" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632478085924325296" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="280" y="264" mx="354" my="280">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632478085924325290" />
        <item text="OnGatherDigits_Failed" treenode="632478085924325295" />
      </items>
      <linkto id="632478085924325305" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">15</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">upc</ap>
        <log condition="entry" on="true" level="Info" type="literal">Play complete</log>
      </Properties>
    </node>
    <node type="Action" id="632478085924325305" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="512" y="351">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632479722911415648" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="185" y="287">
      <linkto id="632478085924325296" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632479722911415649" type="Labeled" style="Bezier" ortho="true" label="ending" />
      <linkto id="632517157947274684" type="Labeled" style="Bezier" ortho="true" label="account_code" />
      <linkto id="632517157947274687" type="Labeled" style="Bezier" ortho="true" label="pin" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632479722911415649" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="262" y="135">
      <linkto id="632479722911415650" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632479722911415650" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="371" y="140">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632517157947274684" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="353" y="424" mx="427" my="440">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632478085924325290" />
        <item text="OnGatherDigits_Failed" treenode="632478085924325295" />
      </items>
      <linkto id="632517157947274690" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">account_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632517157947274687" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="157" y="485" mx="231" my="501">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632478085924325290" />
        <item text="OnGatherDigits_Failed" treenode="632478085924325295" />
      </items>
      <linkto id="632517157947274691" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">pin</ap>
      </Properties>
    </node>
    <node type="Action" id="632517157947274690" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="647" y="476">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632517157947274691" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="464" y="587">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632479722911415647" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632478085924325281" treenode="632478085924325282" appnode="632478085924325279" handlerfor="632478085924325278">
    <node type="Start" id="632478085924325281" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632478085924325299" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632478085924325299" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="336" y="152">
      <linkto id="632478085924325304" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_callId</ap>
        <log condition="entry" on="true" level="Info" type="literal">Play failed</log>
      </Properties>
    </node>
    <node type="Action" id="632478085924325304" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="546" y="348">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" activetab="true" startnode="632478085924325289" treenode="632478085924325290" appnode="632478085924325287" handlerfor="632478085924325286">
    <node type="Loop" id="632510619951044447" name="Loop" text="loop (var)" cx="120" cy="120" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="904.4707" y="498" mx="964" my="558">
      <linkto id="632479516327941385" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632478127294768648" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="variable">addresses</Properties>
    </node>
    <node type="Start" id="632478085924325289" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="312">
      <linkto id="632479516327941384" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632478127294768644" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="605" y="314">
      <linkto id="632479516327941386" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632510619951044440" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties language="csharp">
public static string Execute(LogWriter log, string digits,  string name, string type, ref string message)
{

if((name == null || name == String.Empty) &amp;&amp; type == String.Empty)
{
log.Write(TraceLevel.Warning, "No item with the UPC of " + digits);
message = "No item with the UPC of '" + digits + "' found";
return "failure";
}

if(name == String.Empty || name == null)
{
message = "No item with the UPC of '" + digits + "' found";
return "failure";
}

log.Write(TraceLevel.Info, "The name of the item " + name + ".  \nThe type of the item " + type);

if(type == String.Empty || type == null)
{
message = "Name: " + name;
}
else
{
message = "Name: " + name;
message += "Additional: " + type;
}

return "success";
}</Properties>
    </node>
    <node type="Action" id="632478127294768648" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="915.259766" y="323">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632479493455468716" name="Lookup" class="MaxActionNode" group="" path="Metreos.Native.Upc" x="488" y="317">
      <linkto id="632478127294768644" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Upc" type="variable">digits</ap>
        <rd field="Name">name</rd>
        <rd field="Type">type</rd>
      </Properties>
    </node>
    <node type="Action" id="632479516327941383" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="323" y="320">
      <linkto id="632479493455468716" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="csharp">"Digits entered: " + digits</ap>
        <ap name="LogLevel" type="literal">Info</ap>
      </Properties>
    </node>
    <node type="Action" id="632479516327941384" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="129" y="317">
      <linkto id="632517157947274700" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string digits)
{

if(digits.EndsWith("#"))
{
digits = digits.Substring(0, digits.Length - 1);
}

return "";
}
</Properties>
    </node>
    <node type="Action" id="632479516327941385" name="Send" container="632510619951044447" class="MaxActionNode" group="" path="Metreos.Native.Mail" x="965" y="557">
      <linkto id="632510619951044447" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="To" type="csharp">loopEnum.Current.ToString()</ap>
        <ap name="From" type="variable">g_from_email_address</ap>
        <ap name="MailServer" type="variable">g_smtpHostName</ap>
        <ap name="Username" type="variable">g_smtpUsername</ap>
        <ap name="Password" type="variable">g_smtpPassword</ap>
        <ap name="Subject" type="csharp">"Lookup for '" + digits + "'";</ap>
        <ap name="Body" type="variable">message</ap>
        <ap name="SendAsHtml" type="literal">false</ap>
      </Properties>
    </node>
    <node type="Action" id="632479516327941386" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="692" y="165" mx="745" my="181">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632478085924325277" />
        <item text="OnPlay_Failed" treenode="632478085924325282" />
      </items>
      <linkto id="632478127294768648" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="literal">try_again.wav</ap>
        <ap name="Prompt2" type="literal">enter_skew2.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
        <log condition="entry" on="true" level="Info" type="literal">Playing failed to lookup response</log>
      </Properties>
    </node>
    <node type="Action" id="632510619951044440" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="345.4707" y="434">
      <linkto id="632510619951044441" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="literal">demosite</ap>
        <ap name="Server" type="variable">g_dbHostName</ap>
        <ap name="Username" type="variable">g_dbUsername</ap>
        <ap name="Password" type="variable">g_dbPassword</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632510619951044441" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="466.4707" y="436">
      <linkto id="632510619951044442" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">addresses</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Action" id="632510619951044442" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="587.4707" y="437">
      <linkto id="632510619951044445" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Query" type="csharp">"SELECT * FROM upc_addresses WHERE users_id = " + g_userId</ap>
        <ap name="Name" type="literal">addresses</ap>
        <rd field="ResultSet">results</rd>
      </Properties>
    </node>
    <node type="Action" id="632510619951044445" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="691.4707" y="436">
      <linkto id="632478127294768648" type="Labeled" style="Bezier" ortho="true" label="none" />
      <linkto id="632510619951044448" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(DataTable results, ArrayList addresses)
{
	if(results == null ||
	   results.Rows.Count == 0)	return "none";

	DataRow row = results.Rows[0];
	
	if(row[0] != null &amp;&amp; row[0] != String.Empty &amp;&amp; !Convert.IsDBNull(row[0]))
	{
		addresses.Add(row[0].ToString());
	}

	if(row[1] != null &amp;&amp; row[1] != String.Empty &amp;&amp; !Convert.IsDBNull(row[1]))
	{
		addresses.Add(row[1].ToString());
	}

	if(row[2] != null &amp;&amp; row[2] != String.Empty &amp;&amp; !Convert.IsDBNull(row[2]))
	{
		addresses.Add(row[2].ToString());
	}

	if(row[3] != null &amp;&amp; row[3] != String.Empty &amp;&amp; !Convert.IsDBNull(row[3]))
	{
		addresses.Add(row[3].ToString());
	}

	if(row[4] != null &amp;&amp; row[4] != String.Empty &amp;&amp; !Convert.IsDBNull(row[4]))
	{
		addresses.Add(row[4].ToString());
	}	

	return "";

}
</Properties>
    </node>
    <node type="Action" id="632510619951044448" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="542" y="540" mx="595" my="556">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632478085924325277" />
        <item text="OnPlay_Failed" treenode="632478085924325282" />
      </items>
      <linkto id="632510619951044447" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Prompt1" type="literal">sending_info.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">ending</ap>
      </Properties>
    </node>
    <node type="Action" id="632517157947274700" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="229" y="318">
      <linkto id="632479516327941383" type="Labeled" style="Bezier" ortho="true" label="upc" />
      <linkto id="632517157947274703" type="Labeled" style="Bezier" ortho="true" label="account_code" />
      <linkto id="632517157947274708" type="Labeled" style="Bezier" ortho="true" label="pin" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632517157947274703" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="137" y="505">
      <linkto id="632517157947274704" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">digits</ap>
        <rd field="ResultData">g_accountCode</rd>
      </Properties>
    </node>
    <node type="Action" id="632517157947274704" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="81" y="598" mx="134" my="614">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632478085924325277" />
        <item text="OnPlay_Failed" treenode="632478085924325282" />
      </items>
      <linkto id="632517157947274707" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="literal">enter_pin.wav</ap>
        <ap name="Prompt2" type="literal">pound_sign.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">pin</ap>
      </Properties>
    </node>
    <node type="Action" id="632517157947274707" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="286" y="761">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632517157947274708" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="259" y="516">
      <linkto id="632517157947274833" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">digits</ap>
        <rd field="ResultData">g_pin</rd>
      </Properties>
    </node>
    <node type="Action" id="632517157947274710" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="394.4707" y="646">
      <linkto id="632517157947274835" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">addresses2</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Action" id="632517157947274833" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="321.4707" y="569">
      <linkto id="632517157947274710" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="literal">demosite</ap>
        <ap name="Server" type="variable">g_dbHostName</ap>
        <ap name="Username" type="variable">g_dbUsername</ap>
        <ap name="Password" type="variable">g_dbPassword</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632517157947274835" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="478.4707" y="713">
      <linkto id="632517157947274837" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Query" type="csharp">"SELECT id FROM users where account_code = " + g_accountCode + " AND pin = " + g_pin</ap>
        <ap name="Name" type="literal">addresses2</ap>
        <rd field="ResultSet">results</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"SELECT id FROM users where account_code = " + g_accountCode + " AND pin = " + g_pin</log>
      </Properties>
    </node>
    <node type="Action" id="632517157947274837" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="646.4707" y="715">
      <linkto id="632517157947274841" type="Labeled" style="Bezier" ortho="true" label="valid" />
      <linkto id="632517157947274844" type="Labeled" style="Bezier" ortho="true" label="invalid" />
      <Properties language="csharp">
public static string Execute(DataTable results, ref int g_userId)
{
	if(results == null ||
	   results.Rows.Count == 0)	return "invalid";

	DataRow row = results.Rows[0];
	
	if(row[0] != null &amp;&amp; row[0] != String.Empty &amp;&amp; !Convert.IsDBNull(row[0]))
	{
		g_userId = Convert.ToInt32(row[0]);
		return "valid";
	}
	else
	{
		return "invalid";
	}
}
</Properties>
    </node>
    <node type="Action" id="632517157947274841" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="799" y="706" mx="852" my="722">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632478085924325277" />
        <item text="OnPlay_Failed" treenode="632478085924325282" />
      </items>
      <linkto id="632517157947274847" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="literal">enter_skew2.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632517157947274844" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="674" y="790" mx="727" my="806">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632478085924325277" />
        <item text="OnPlay_Failed" treenode="632478085924325282" />
      </items>
      <linkto id="632517157947274847" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="literal">error_login.wav</ap>
        <ap name="Prompt2" type="literal">enter_acct.wav</ap>
        <ap name="Prompt3" type="literal">pound_sign.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">account_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632517157947274847" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="908" y="838">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632478085924325306" name="terminationCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">terminationCondition</Properties>
    </node>
    <node type="Variable" id="632478085924325307" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">digits</Properties>
    </node>
    <node type="Variable" id="632479493455468712" name="name" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">name</Properties>
    </node>
    <node type="Variable" id="632479493455468713" name="type" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">type</Properties>
    </node>
    <node type="Variable" id="632479532361371281" name="message" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">message</Properties>
    </node>
    <node type="Variable" id="632510619951044443" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632510619951044444" name="results" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">results</Properties>
    </node>
    <node type="Variable" id="632510619951044446" name="addresses" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="ArrayList" refType="reference">addresses</Properties>
    </node>
    <node type="Variable" id="632517157947274699" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632478085924325294" treenode="632478085924325295" appnode="632478085924325292" handlerfor="632478085924325291">
    <node type="Start" id="632478085924325294" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="344">
      <linkto id="632478127294768649" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632478127294768649" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="440" y="359">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632479532361371357" treenode="632479532361371358" appnode="632479532361371355" handlerfor="632479532361371354">
    <node type="Start" id="632479532361371357" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632479532361371359" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632479532361371359" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="610" y="318">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>