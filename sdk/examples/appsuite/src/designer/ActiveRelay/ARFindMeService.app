<Application name="ARFindMeService" trigger="Metreos.Events.ActiveRelay.FindMeServiceRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="ARFindMeService">
    <outline>
      <treenode type="evh" id="632760600834015580" level="1" text="Metreos.Events.ActiveRelay.FindMeServiceRequest (trigger): OnFindMeServiceRequest">
        <node type="function" name="OnFindMeServiceRequest" id="632760600834015577" path="Metreos.StockTools" />
        <node type="event" name="FindMeServiceRequest" id="632760600834015576" path="Metreos.Events.ActiveRelay.FindMeServiceRequest" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632761363549648363" level="1" text="OpenDBConnection">
        <node type="function" name="OpenDBConnection" id="632761363549648360" path="Metreos.StockTools" />
        <calls>
          <ref actid="632761363549648359" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_DbWriteEnabled" id="632802867924429685" vid="632659276838564881">
        <Properties type="Bool" defaultInitWith="true">g_DbWriteEnabled</Properties>
      </treenode>
      <treenode text="db_poolConnections" id="632802867924429687" vid="632674146742755441">
        <Properties type="Bool" defaultInitWith="true" initWith="DbConnPooling">db_poolConnections</Properties>
      </treenode>
      <treenode text="db_ConnectionName" id="632802867924429689" vid="632347619057191312">
        <Properties type="String" initWith="DbConnectionName">db_ConnectionName</Properties>
      </treenode>
      <treenode text="db_Master_DbName" id="632802867924429691" vid="632346722572969731">
        <Properties type="String" initWith="DbName">db_Master_DbName</Properties>
      </treenode>
      <treenode text="db_Master_DbServer" id="632802867924429693" vid="632346722572969733">
        <Properties type="String" initWith="Server">db_Master_DbServer</Properties>
      </treenode>
      <treenode text="db_Master_Port" id="632802867924429695" vid="632346722572969735">
        <Properties type="UInt" initWith="Port">db_Master_Port</Properties>
      </treenode>
      <treenode text="db_Master_Username" id="632802867924429697" vid="632346722572969737">
        <Properties type="String" initWith="Username">db_Master_Username</Properties>
      </treenode>
      <treenode text="db_Master_Password" id="632802867924429699" vid="632346722572969739">
        <Properties type="String" initWith="Password">db_Master_Password</Properties>
      </treenode>
      <treenode text="db_Slave_DbName" id="632802867924429701" vid="632346722572969731">
        <Properties type="String" initWith="SlaveDBName">db_Slave_DbName</Properties>
      </treenode>
      <treenode text="db_Slave_DbServer" id="632802867924429703" vid="632346722572969733">
        <Properties type="String" initWith="SlaveDBServerAddress">db_Slave_DbServer</Properties>
      </treenode>
      <treenode text="db_Slave_Port" id="632802867924429705" vid="632346722572969735">
        <Properties type="UInt" initWith="SlaveDBServerPort">db_Slave_Port</Properties>
      </treenode>
      <treenode text="db_Slave_Username" id="632802867924429707" vid="632346722572969737">
        <Properties type="String" initWith="SlaveDBServerUsername">db_Slave_Username</Properties>
      </treenode>
      <treenode text="db_Slave_Password" id="632802867924429709" vid="632346722572969739">
        <Properties type="String" initWith="SlaveDBServerPassword">db_Slave_Password</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnFindMeServiceRequest" activetab="true" startnode="632760600834015579" treenode="632760600834015580" appnode="632760600834015577" handlerfor="632760600834015576">
    <node type="Start" id="632760600834015579" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="157">
      <linkto id="632761363549648359" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632761363549648359" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="91.59473" y="141" mx="149" my="157">
      <items count="1">
        <item text="OpenDBConnection" />
      </items>
      <linkto id="632761363549648684" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632761363549648685" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">OpenDBConnection</ap>
      </Properties>
    </node>
    <node type="Action" id="632761363549648676" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="403" y="157">
      <linkto id="632761363549648677" type="Labeled" style="Bezier" ortho="true" label="enable_all" />
      <linkto id="632761363549648678" type="Labeled" style="Bezier" ortho="true" label="disable_all" />
      <linkto id="632761363549648679" type="Labeled" style="Bezier" ortho="true" label="enable_vmail" />
      <linkto id="632761363549648680" type="Labeled" style="Bezier" ortho="true" label="disable_except_vmail" />
      <linkto id="632762549216809418" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">operation</ap>
      </Properties>
    </node>
    <node type="Label" id="632761363549648677" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="402" y="39" />
    <node type="Label" id="632761363549648678" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="624" y="37" />
    <node type="Label" id="632761363549648679" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="627" y="158" />
    <node type="Label" id="632761363549648680" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="629" y="301" />
    <node type="Action" id="632761363549648681" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="145" y="407">
      <linkto id="632761363549648683" type="Labeled" style="Bezier" label="success" />
      <linkto id="632761363549648683" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Response" type="literal">Failure</ap>
        <ap name="TimerId" type="variable">timerId</ap>
        <ap name="EventName" type="literal">Metreos.Events.ActiveRelay.FindMeServiceRequestResp</ap>
        <ap name="ToGuid" type="variable">senderGuid</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnFindMeServiceRequest: SendEvent(Failure) to GUID '" + senderGuid + "'"
</log>
        <log condition="default" on="true" level="Info" type="csharp">"OnFindMeServiceRequest: SendEvent(Failure) to GUID '" + senderGuid + "' failed."
</log>
      </Properties>
    </node>
    <node type="Label" id="632761363549648682" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="51" y="407">
      <linkto id="632761363549648681" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632761363549648683" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="267" y="406">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632761363549648684" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="146" y="279" />
    <node type="Action" id="632761363549648685" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="275" y="157">
      <linkto id="632761363549648676" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632761363549648686" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_DbWriteEnabled</ap>
      </Properties>
    </node>
    <node type="Label" id="632761363549648686" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="276" y="276" />
    <node type="Label" id="632761363549648687" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="56" y="558">
      <linkto id="632762531330090476" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632761363549648688" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="572" y="560">
      <linkto id="632762531330090477" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632761363549648689" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="85" y="855">
      <linkto id="632762549216809229" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632761363549648690" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="63" y="1195">
      <linkto id="632762549216809233" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632762531330090476" name="SetFindMeStatus" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="181" y="558">
      <linkto id="632762549216809393" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632762549216809395" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">userId</ap>
        <ap name="Filter" type="literal">%</ap>
        <ap name="NewValue" type="literal">true</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnFindMeServiceRequest: Enabling all FindMe numbers.</log>
      </Properties>
    </node>
    <node type="Action" id="632762531330090477" name="SetFindMeStatus" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="676" y="560">
      <linkto id="632762549216809394" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632762549216809396" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">userId</ap>
        <ap name="Filter" type="literal">%</ap>
        <ap name="NewValue" type="csharp">false</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnFindMeServiceRequest: Disabling all FindMe numbers.
</log>
        <log condition="default" on="false" level="Info" type="csharp">OnFindMeServiceRequest: Disabling all FindMe numbers.

</log>
      </Properties>
    </node>
    <node type="Action" id="632762549216809229" name="GetCorporateNumberForUser" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="223" y="856">
      <linkto id="632762549216809231" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632762549216809408" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632802867924429772" type="Labeled" style="Bezier" ortho="true" label="NoNumberDefined" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">userId</ap>
        <rd field="Number">corporateNumber</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"Retrieving corporate number for user with userId: " + userId</log>
        <log condition="default" on="true" level="Info" type="csharp">"OnFindMeServiceRequest: Unable to retrieve corporate number for userId: " + userId</log>
      </Properties>
    </node>
    <node type="Action" id="632762549216809231" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="374" y="856">
      <linkto id="632762549216809232" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632762549216809404" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="default" on="true" level="Info" type="literal">OnFindMeServiceRequest: The retrieved corporate number was not valid.</log>
public static string Execute(string corporateNumber)
{
	if (corporateNumber == null || corporateNumber == string.Empty)
		return "default";
	else
		return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632762549216809232" name="SetFindMeStatus" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="509" y="856">
      <linkto id="632762549216809402" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632762549216809410" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">userId</ap>
        <ap name="Filter" type="variable">corporateNumber</ap>
        <ap name="NewValue" type="literal">true</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnFindMeServiceRequest: Enabling FindMe # " + corporateNumber + " for userId: " + userId
</log>
        <log condition="default" on="true" level="Info" type="csharp">"OnFindMeServiceRequest: Failed to enable FindMe # " + corporateNumber + " for userId: " + userId

</log>
      </Properties>
    </node>
    <node type="Action" id="632762549216809233" name="GetCorporateNumberForUser" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="207" y="1195">
      <linkto id="632762549216809406" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632762549216809235" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632802867924429773" type="Labeled" style="Bezier" ortho="true" label="NoNumberDefined" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">userId</ap>
        <rd field="Number">corporateNumber</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"Retrieving corporate number for user with userId: " + userId
</log>
        <log condition="default" on="true" level="Info" type="csharp">"OnFindMeServiceRequest: Unable to retrieve corporate number for userId: " + userId</log>
      </Properties>
    </node>
    <node type="Action" id="632762549216809234" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="523" y="1193">
      <linkto id="632762549216809415" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632762549216809239" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties language="csharp">
        <log condition="default" on="true" level="Info" type="literal">OnFindMeServiceRequest: The retrieved corporate number was not valid.</log>
public static string Execute(string corporateNumber)
{
	if (corporateNumber == null || corporateNumber == string.Empty)
		return "default";
	else
		return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632762549216809235" name="SetFindMeStatus" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="375" y="1194">
      <linkto id="632762549216809234" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632762549216809234" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">userId</ap>
        <ap name="Filter" type="literal">%</ap>
        <ap name="NewValue" type="literal">false</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnFindMeServiceRequest: Disabling all FindMe numbers.</log>
        <log condition="default" on="true" level="Info" type="csharp">"OnFindMeServiceRequest: Failed to disable all numbers for userId: " + userId</log>
      </Properties>
    </node>
    <node type="Action" id="632762549216809239" name="SetFindMeStatus" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="668" y="1193">
      <linkto id="632762549216809400" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632762549216809417" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">userId</ap>
        <ap name="Filter" type="variable">corporateNumber</ap>
        <ap name="NewValue" type="literal">true</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnFindMeServiceRequest: Enabling FindMe # " + corporateNumber + " for userId: " + userId

</log>
        <log condition="default" on="true" level="Info" type="csharp">"OnFindMeServiceRequest: Failed to enable FindMe # " + corporateNumber + " for userId: " + userId


</log>
      </Properties>
    </node>
    <node type="Label" id="632762549216809388" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="419" y="411">
      <linkto id="632762549216809389" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632762549216809389" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="522" y="411">
      <linkto id="632762549216809392" type="Labeled" style="Bezier" label="success" />
      <linkto id="632762549216809392" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Response" type="literal">Success</ap>
        <ap name="TimerId" type="variable">timerId</ap>
        <ap name="EventName" type="literal">Metreos.Events.ActiveRelay.FindMeServiceRequestResp</ap>
        <ap name="ToGuid" type="variable">senderGuid</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnFindMeServiceRequest: SendEvent(Success) to GUID '" + senderGuid + "'"</log>
        <log condition="default" on="true" level="Info" type="csharp">"OnFindMeServiceRequest: SendEvent(Success) to GUID '" + senderGuid + "' failed."</log>
      </Properties>
    </node>
    <node type="Action" id="632762549216809392" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="634" y="411">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632762549216809393" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="297" y="558" />
    <node type="Label" id="632762549216809394" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="805.9981" y="560" />
    <node type="Label" id="632762549216809395" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="180" y="663" />
    <node type="Label" id="632762549216809396" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="677" y="672" />
    <node type="Label" id="632762549216809400" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="669" y="1329" />
    <node type="Label" id="632762549216809402" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="510" y="972" />
    <node type="Label" id="632762549216809404" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="374" y="971" />
    <node type="Label" id="632762549216809406" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="207" y="1322.5" />
    <node type="Label" id="632762549216809408" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="224" y="974" />
    <node type="Label" id="632762549216809410" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="626.9981" y="855.5" />
    <node type="Label" id="632762549216809415" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="523" y="1327" />
    <node type="Label" id="632762549216809417" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="788.9981" y="1193" />
    <node type="Label" id="632762549216809418" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="402.9981" y="274" />
    <node type="Label" id="632802867924429767" text="N" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="827.3727" y="414">
      <linkto id="632802867924429768" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632802867924429768" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="920" y="415">
      <linkto id="632802867924429770" type="Labeled" style="Bezier" label="success" />
      <linkto id="632802867924429770" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Response" type="literal">MailboxNotDefined</ap>
        <ap name="TimerId" type="variable">timerId</ap>
        <ap name="EventName" type="literal">Metreos.Events.ActiveRelay.FindMeServiceRequestResp</ap>
        <ap name="ToGuid" type="variable">senderGuid</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnFindMeServiceRequest: SendEvent(MailboxNotDefined) to GUID '" + senderGuid + "'"</log>
        <log condition="default" on="true" level="Info" type="csharp">"OnFindMeServiceRequest: SendEvent(MailboxNotDefined) to GUID '" + senderGuid + "' failed."</log>
      </Properties>
    </node>
    <node type="Action" id="632802867924429770" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1043.81775" y="415">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632802867924429772" text="N" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="223" y="756" />
    <node type="Label" id="632802867924429773" text="N" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="207" y="1088.5" />
    <node type="Variable" id="632779600652172281" name="userId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" initWith="UserId" refType="reference" name="Metreos.Events.ActiveRelay.FindMeServiceRequest">userId</Properties>
    </node>
    <node type="Variable" id="632779600652172282" name="operation" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="OperationType" refType="reference" name="Metreos.Events.ActiveRelay.FindMeServiceRequest">operation</Properties>
    </node>
    <node type="Variable" id="632779600652172283" name="senderGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="SenderRoutingGuid" refType="reference" name="Metreos.Events.ActiveRelay.FindMeServiceRequest">senderGuid</Properties>
    </node>
    <node type="Variable" id="632779600652172284" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TimerId" refType="reference" name="Metreos.Events.ActiveRelay.FindMeServiceRequest">timerId</Properties>
    </node>
    <node type="Variable" id="632779600652172285" name="corporateNumber" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">corporateNumber</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OpenDBConnection" startnode="632761363549648362" treenode="632761363549648363" appnode="632761363549648360" handlerfor="632760600834015576">
    <node type="Start" id="632761363549648362" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="74" y="95">
      <linkto id="632761363549648365" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632761363549648364" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="363.188477" y="94.5">
      <linkto id="632761363549648368" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632761363549648369" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="variable">db_ConnectionName</ap>
        <ap name="Type" type="literal">mysql</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OpenDBConnection: attempting connect to Primary</log>
        <log condition="default" on="true" level="Warning" type="literal">OpenDBConnection: Connection to Primary failed.</log>
      </Properties>
    </node>
    <node type="Action" id="632761363549648365" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="191.188782" y="95.5">
      <linkto id="632761363549648364" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632761363549648369" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">db_Master_DbName</ap>
        <ap name="Server" type="variable">db_Master_DbServer</ap>
        <ap name="Port" type="variable">db_Master_Port</ap>
        <ap name="Username" type="variable">db_Master_Username</ap>
        <ap name="Password" type="variable">db_Master_Password</ap>
        <ap name="Pooling" type="variable">db_poolConnections</ap>
        <ap name="ConnectionTimeout" type="literal">1</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632761363549648366" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="652.1886" y="372.5">
      <linkto id="632761363549648368" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632761363549648370" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="variable">db_ConnectionName</ap>
        <ap name="Type" type="literal">mysql</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OpenDBConnection: attempting connect to Secondary</log>
        <log condition="default" on="true" level="Info" type="literal">OpenDBConnection: Connection to Secondary failed.
</log>
      </Properties>
    </node>
    <node type="Action" id="632761363549648367" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="498.188843" y="373.5">
      <linkto id="632761363549648366" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="632761363549648370" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">db_Slave_DbName</ap>
        <ap name="Server" type="variable">db_Slave_DbServer</ap>
        <ap name="Port" type="variable">db_Slave_Port</ap>
        <ap name="Username" type="variable">db_Slave_Username</ap>
        <ap name="Password" type="variable">db_Slave_Password</ap>
        <ap name="Pooling" type="variable">db_poolConnections</ap>
        <ap name="ConnectionTimeout" type="literal">1</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632761363549648368" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="645.9997" y="94">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OpenDBConnection: connection to database established.</log>
      </Properties>
    </node>
    <node type="Action" id="632761363549648369" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="365.9997" y="271">
      <linkto id="632761363549648371" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_DbWriteEnabled</rd>
      </Properties>
    </node>
    <node type="Action" id="632761363549648370" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="651.9997" y="572">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
        <log condition="entry" on="true" level="Error" type="literal">OpenDBConnection: AppSuite DB connections failed. Check application settings.</log>
      </Properties>
    </node>
    <node type="Action" id="632761363549648371" name="SetSessionData" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="497.999756" y="271">
      <linkto id="632761363549648367" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="AllowDBWrite" type="csharp">false</ap>
      </Properties>
    </node>
    <node type="Variable" id="632761363549648675" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>