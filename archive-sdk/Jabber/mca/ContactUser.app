<Application name="ContactUser" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="ContactUser">
    <outline>
      <treenode type="evh" id="632989288426989162" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632989288426989159" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632989288426989158" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/JabberApp/ReachUser</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632989288426989180" level="2" text="Metreos.Providers.Http.GotRequest: PostMessage">
        <node type="function" name="PostMessage" id="632989288426989177" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632989288426989176" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/JabberApp/PostMessage</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632989288426989191" level="2" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="632989288426989188" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="632989288426989187" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_user" id="632989288426989182" vid="632989288426989181">
        <Properties type="String">g_user</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632989288426989161" treenode="632989288426989162" appnode="632989288426989159" handlerfor="632989288426989158">
    <node type="Start" id="632989288426989161" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="112" y="384">
      <linkto id="632989288426989167" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632989288426989164" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="536" y="384">
      <linkto id="632989288426989171" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Http.QueryParamCollection query, ref string body, string host, string routingGuid, ref string g_user)
{
	string user = query["user"];
	g_user = user;

	body = String.Format(@"
&lt;html&gt;
&lt;head&gt;
&lt;title&gt;Impromptu Chat Session with {0}&lt;/title&gt;
&lt;script&gt;
&lt;!-- A wee bit of Ajax --&gt;
&lt;/script&gt;
&lt;/head&gt;
&lt;body&gt;
&lt;p&gt;Post a message to {0}:&lt;/p&gt;
&lt;form method='POST' action='{1}'&gt;
&lt;label&gt;Message&lt;/label&gt;&lt;br /&gt;
&lt;textarea rows=5 cols=40 name='message'&gt;&lt;/textarea&gt;
&lt;input type='Submit' value='Submit' name='Submit' /&gt;
&lt;/form&gt;
&lt;/body&gt;
&lt;/html&gt;", user, "http://" + host + "/JabberApp/PostMessage?metreosSessionId=" + routingGuid);

return "good";

}
</Properties>
    </node>
    <node type="Comment" id="632989288426989165" text="First discover if he's in chat" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="240" y="201" />
    <node type="Action" id="632989288426989166" name="SendChat" class="MaxActionNode" group="" path="Metreos.Providers.JabberProvider" x="376" y="384">
      <linkto id="632989288426989164" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Body" type="csharp">"Someone has clicked the profile link for " + query["user"]</ap>
      </Properties>
    </node>
    <node type="Action" id="632989288426989167" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="232" y="384">
      <linkto id="632989288426989166" type="Labeled" style="Vector" label="true" />
      <linkto id="632989288426989168" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">query["user"] != null &amp;&amp; query["user"] != String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632989288426989168" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="224" y="576">
      <linkto id="632989288426989170" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">&lt;html&gt;&lt;body&gt;&lt;p&gt;Sorry, this user has not set up their account correctly for this service&lt;/p&gt;&lt;/body&gt;&lt;/html&gt;</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632989288426989170" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="224" y="704">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632989288426989171" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="720" y="384">
      <linkto id="632989288426989172" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="variable">body</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632989288426989172" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="847" y="384">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632989288426989163" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632989288426989169" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632989288426989173" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632989288426989174" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632989288426989175" name="body" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">body</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="PostMessage" activetab="true" startnode="632989288426989179" treenode="632989288426989180" appnode="632989288426989177" handlerfor="632989288426989176">
    <node type="Start" id="632989288426989179" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="160" y="376">
      <linkto id="632989288426989195" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632989288426989183" name="SendChat" class="MaxActionNode" group="" path="Metreos.Providers.JabberProvider" x="360" y="376">
      <linkto id="632989288426989194" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Body" type="csharp">g_user + ": " + post["message"]</ap>
      </Properties>
    </node>
    <node type="Action" id="632989288426989186" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="624" y="376">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632989288426989194" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="472" y="376">
      <linkto id="632989288426989186" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="variable">body</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632989288426989195" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="240" y="376">
      <linkto id="632989288426989183" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(ref string body, string host, string routingGuid, string g_user)
{
	string user = g_user;

	body = String.Format(@"
&lt;html&gt;
&lt;head&gt;
&lt;title&gt;Impromptu Chat Session with {0}&lt;/title&gt;
&lt;script&gt;
&lt;!-- A wee bit of Ajax --&gt;
&lt;/script&gt;
&lt;/head&gt;
&lt;body&gt;
&lt;p&gt;Post a message to {0}:&lt;/p&gt;
&lt;form method='POST' action='{1}'&gt;
&lt;label&gt;Message&lt;/label&gt;&lt;br /&gt;
&lt;textarea rows=5 cols=40 name='message'&gt;&lt;/textarea&gt;
&lt;input type='Submit' value='Submit' name='Submit' /&gt;
&lt;/form&gt;
&lt;/body&gt;
&lt;/html&gt;", user, "http://" + host + "/JabberApp/PostMessage?metreosSessionId=" + routingGuid);

return "good";

}

</Properties>
    </node>
    <node type="Variable" id="632989288426989184" name="chatWindow" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">chatWindow</Properties>
    </node>
    <node type="Variable" id="632989288426989185" name="post" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.FormCollection" initWith="body" refType="reference">post</Properties>
    </node>
    <node type="Variable" id="632989288426989193" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632989288426989196" name="body" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">body</Properties>
    </node>
    <node type="Variable" id="632989288426989197" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632989288426989198" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="632989288426989190" treenode="632989288426989191" appnode="632989288426989188" handlerfor="632989288426989187">
    <node type="Start" id="632989288426989190" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632989288426989192" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632989288426989192" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="397" y="363">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>