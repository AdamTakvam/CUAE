<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632951671357325710" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632951671357325707" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632951671357325706" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/doauthenticateuser</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_ccmIp" id="632954132384806441" vid="632951671357325719">
        <Properties type="String" initWith="callManagerIP">g_ccmIp</Properties>
      </treenode>
      <treenode text="g_ccmUser" id="632954132384806443" vid="632951671357325721">
        <Properties type="String" initWith="ccmUsername">g_ccmUser</Properties>
      </treenode>
      <treenode text="g_ccmPass" id="632954132384806445" vid="632951671357325723">
        <Properties type="String" initWith="ccmPassword">g_ccmPass</Properties>
      </treenode>
      <treenode text="g_userId" id="632954132384806447" vid="632951671357325725">
        <Properties type="String" initWith="userid">g_userId</Properties>
      </treenode>
      <treenode text="g_userPassword" id="632954132384806449" vid="632951671357325727">
        <Properties type="String" defaultInitWith="nothing" initWith="userIdPassword">g_userPassword</Properties>
      </treenode>
      <treenode text="g_userPin" id="632954132384806451" vid="632951671357325729">
        <Properties type="String" defaultInitWith="nothing" initWith="userIdPin">g_userPin</Properties>
      </treenode>
      <treenode text="g_testPassword" id="632954132384806453" vid="632951671357325731">
        <Properties type="Bool" initWith="TestPassword">g_testPassword</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632951671357325709" treenode="632951671357325710" appnode="632951671357325707" handlerfor="632951671357325706">
    <node type="Start" id="632951671357325709" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="45" y="264">
      <linkto id="632952063524170384" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632951676773610730" name="DoAuthenticateUser" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap601" x="269" y="32">
      <linkto id="632951676773610734" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632951676773610735" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <linkto id="632951676773610736" type="Labeled" style="Bezier" ortho="true" label="fault" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
        <ap name="UserPassword" type="variable">g_userPassword</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIp</ap>
        <ap name="AdminUsername" type="variable">g_ccmUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmPass</ap>
        <rd field="Authenticated">response</rd>
        <rd field="FaultMessage">message</rd>
        <rd field="FaultCode">fault</rd>
      </Properties>
    </node>
    <node type="Action" id="632951676773610734" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="529" y="229">
      <linkto id="632951676773610738" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">String.Format("Authenticated user {0} {1}", g_userId, response ? "successfully" : "unsuccessfully")</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632951676773610735" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="530" y="358">
      <linkto id="632951676773610738" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="literal">Generic failure in DoAuthenticateUser.  Check log.</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632951676773610736" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="536" y="513">
      <linkto id="632951676773610738" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">"Soap Fault!  Message:" + message + ", Fault:" + fault</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632951676773610738" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="707" y="364">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632952063524170382" name="DoAuthenticateUser" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap601" x="248" y="633">
      <linkto id="632951676773610734" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632951676773610735" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <linkto id="632951676773610736" type="Labeled" style="Bezier" ortho="true" label="fault" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
        <ap name="UserPin" type="variable">g_userPin</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIp</ap>
        <ap name="AdminUsername" type="variable">g_ccmUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmPass</ap>
        <rd field="Authenticated">response</rd>
        <rd field="FaultMessage">message</rd>
        <rd field="FaultCode">fault</rd>
      </Properties>
    </node>
    <node type="Action" id="632952063524170384" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="140" y="264">
      <linkto id="632951676773610730" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632952063524170382" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_testPassword</ap>
      </Properties>
    </node>
    <node type="Comment" id="632952063524170385" text="Test Pin" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="230" y="716" />
    <node type="Comment" id="632952063524170386" text="Test Password" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="340" y="32" />
    <node type="Variable" id="632951671357325733" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632951676773610731" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632951676773610732" name="fault" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">fault</Properties>
    </node>
    <node type="Variable" id="632951676773610733" name="message" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">message</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>