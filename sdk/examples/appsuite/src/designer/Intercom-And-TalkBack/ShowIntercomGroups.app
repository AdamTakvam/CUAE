<Application name="ShowIntercomGroups" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="ShowIntercomGroups">
    <outline>
      <treenode type="evh" id="632347278629466188" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632347278629466185" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632347278629466184" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ShowIntercomGroups</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_dbUsername" id="632766857578773095" vid="632347358471072828">
        <Properties type="String" initWith="Username">g_dbUsername</Properties>
      </treenode>
      <treenode text="g_dbHost" id="632766857578773097" vid="632347358471072830">
        <Properties type="String" initWith="Server">g_dbHost</Properties>
      </treenode>
      <treenode text="g_dbPort" id="632766857578773099" vid="632347358471072832">
        <Properties type="UInt" initWith="Port">g_dbPort</Properties>
      </treenode>
      <treenode text="g_dbPassword" id="632766857578773101" vid="632348158383489633">
        <Properties type="String" initWith="Password">g_dbPassword</Properties>
      </treenode>
      <treenode text="g_dbName" id="632766857578773103" vid="632348158383489635">
        <Properties type="String" defaultInitWith="application_suite">g_dbName</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632347278629466187" treenode="632347278629466188" appnode="632347278629466185" handlerfor="632347278629466184">
    <node type="Loop" id="632347358471072847" name="Loop" text="loop (expr)" cx="483" cy="174" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="1283" y="97" mx="1524" my="184">
      <linkto id="632347358471072850" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632347387592247044" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="csharp">intercomGroupsIds.Count</Properties>
    </node>
    <node type="Start" id="632347278629466187" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="184">
      <linkto id="632347358471072815" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632347358471072815" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="176" y="184">
      <linkto id="632347358471072816" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632347358471072826" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">queryParams["device"] == null</ap>
      </Properties>
    </node>
    <node type="Action" id="632347358471072816" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="176" y="320">
      <linkto id="632347358471072818" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">No device ID specified!</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Comment" id="632347358471072817" text="queryParams[&quot;deviceId&quot;] == null" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="96" y="136" />
    <node type="Action" id="632347358471072818" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="176" y="456">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632347358471072820" name="GetUserByDeviceMac" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="528" y="184">
      <linkto id="632347358471072822" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632766857578772906" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Mac" type="csharp">queryParams["device"]</ap>
        <ap name="IsPrimary" type="literal">true</ap>
        <rd field="UserId">userId</rd>
        <rd field="UserStatus">userStatus</rd>
        <log condition="success" on="false" level="Info" type="csharp">"Found user '" + userId.ToString() + "' for device Id '" + queryParams["deviceId"] + "'"</log>
      </Properties>
    </node>
    <node type="Action" id="632347358471072822" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="528" y="320">
      <linkto id="632347358471072823" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"No user found with device Id '" + queryParams["device"] + "'"</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632347358471072823" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="528" y="456">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632347358471072824" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2095" y="184">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632347358471072825" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1991" y="184">
      <linkto id="632347358471072824" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Expires" type="csharp">DateTime.Now.AddMinutes(-10).ToString("r")</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menuItem.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632347358471072826" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="280" y="184">
      <linkto id="632347358471072827" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">g_dbName</ap>
        <ap name="Server" type="variable">g_dbHost</ap>
        <ap name="Port" type="variable">g_dbPort</ap>
        <ap name="Username" type="variable">g_dbUsername</ap>
        <ap name="Password" type="variable">g_dbPassword</ap>
        <rd field="DSN">dbDsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632347358471072827" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="392" y="184">
      <linkto id="632347358471072820" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632347358471072835" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dbDsn</ap>
        <ap name="Name" type="literal">ApplicationSuite</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Action" id="632347358471072835" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="392" y="320">
      <linkto id="632347358471072836" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Unable to establish a connection with the database.</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632347358471072836" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="392" y="456">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632347358471072837" name="GetIntercomGroupsForUser" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="912" y="186">
      <linkto id="632347358471072839" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632347358471072840" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">userId</ap>
        <rd field="IntercomGroupsIds">intercomGroupsIds</rd>
      </Properties>
    </node>
    <node type="Action" id="632347358471072839" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1072" y="185">
      <linkto id="632347358471072840" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632347358471072848" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">intercomGroupsIds.Count &lt;= 0</ap>
        <log condition="entry" on="false" level="Info" type="csharp">intercomGroupsIds.Count.ToString()</log>
      </Properties>
    </node>
    <node type="Action" id="632347358471072840" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1072" y="321">
      <linkto id="632347358471072841" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">No intercom groups found for this device</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632347358471072841" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1072" y="449">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632347358471072842" text="intercomGroupsIds.Count &lt;= 0" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="992" y="145" />
    <node type="Comment" id="632347358471072844" text="Error: No user associated&#xD;&#xA;with this device." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="466" y="101" />
    <node type="Comment" id="632347358471072845" text="Error: Can not&#xD;&#xA;connect to DB" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="288" y="296" />
    <node type="Comment" id="632347358471072846" text="Error: No deviceId &#xD;&#xA;specified in request." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="40" y="296" />
    <node type="Action" id="632347358471072848" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1192" y="185">
      <linkto id="632347358471072847" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Your Intercom Groups</ap>
        <ap name="Prompt" type="literal">Select Intercom Group</ap>
        <rd field="ResultData">menuItem</rd>
      </Properties>
    </node>
    <node type="Action" id="632347387592247044" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1887" y="184">
      <linkto id="632347387592247047" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632347358471072825" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">menuHasItems</ap>
      </Properties>
    </node>
    <node type="Label" id="632347387592247045" text="N" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1176" y="321">
      <linkto id="632347358471072840" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632347387592247046" text="Error: No intercom groups." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1096" y="281" />
    <node type="Label" id="632347387592247047" text="N" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1887" y="288" />
    <node type="Action" id="632347358471072849" name="AddMenuItem" container="632347358471072847" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1595" y="177">
      <linkto id="632347387592247048" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">intercomName</ap>
        <ap name="URL" type="csharp">"http://" + appServerIpAddress + ":8000/LaunchIntercom?hostUserId=" + userId + "&amp;hostDeviceId=" + queryParams["device"] + "&amp;id=" + intercomGroupsIds[loopIndex]</ap>
        <rd field="ResultData">menuItem</rd>
      </Properties>
    </node>
    <node type="Action" id="632347358471072850" name="GetIntercomGroup" container="632347358471072847" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1379" y="177">
      <linkto id="632347387592247041" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632347358471072847" port="4" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="IntercomGroupsId" type="csharp">intercomGroupsIds[loopIndex]</ap>
        <rd field="Name">intercomName</rd>
        <rd field="IsEnabled">isEnabled</rd>
      </Properties>
    </node>
    <node type="Action" id="632347387592247041" name="If" container="632347358471072847" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1523" y="177">
      <linkto id="632347358471072849" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632347358471072847" port="4" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">isEnabled</ap>
      </Properties>
    </node>
    <node type="Action" id="632347387592247048" name="Assign" container="632347358471072847" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1683" y="177">
      <linkto id="632347358471072847" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">menuHasItems</rd>
      </Properties>
    </node>
    <node type="Comment" id="632347387592247050" text="menuHasItems = true" container="632347358471072847" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1627" y="129" />
    <node type="Action" id="632766857578772906" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="709" y="184">
      <linkto id="632347358471072837" type="Labeled" style="Bezier" ortho="true" label="Ok" />
      <linkto id="632766857578773148" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">Enum.GetName(typeof(Metreos.ApplicationSuite.Storage.UserStatus), userStatus)</ap>
        <log condition="entry" on="true" level="Info" type="literal">Validating user account status</log>
        <log condition="default" on="true" level="Info" type="literal">User account state disallows operation.</log>
      </Properties>
    </node>
    <node type="Action" id="632766857578773148" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="709" y="319">
      <linkto id="632347358471072823" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"Your account status does not permit access to this system."</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Variable" id="632347279097939821" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632347279097939822" name="menuItem" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">menuItem</Properties>
    </node>
    <node type="Variable" id="632347358471072814" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParams</Properties>
    </node>
    <node type="Variable" id="632347358471072819" name="userId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">userId</Properties>
    </node>
    <node type="Variable" id="632347358471072834" name="dbDsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dbDsn</Properties>
    </node>
    <node type="Variable" id="632347358471072838" name="intercomGroupsIds" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="StringCollection" refType="reference">intercomGroupsIds</Properties>
    </node>
    <node type="Variable" id="632347358471072851" name="intercomName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">intercomName</Properties>
    </node>
    <node type="Variable" id="632347387592247042" name="isEnabled" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">isEnabled</Properties>
    </node>
    <node type="Variable" id="632347387592247049" name="menuHasItems" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">menuHasItems</Properties>
    </node>
    <node type="Variable" id="632347387592247051" name="appServerIpAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="hostname" refType="reference">appServerIpAddress</Properties>
    </node>
    <node type="Variable" id="632766857578772905" name="userStatus" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">userStatus</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>