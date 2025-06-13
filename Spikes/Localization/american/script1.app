<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="633141423147891414" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="633141423147891411" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633141423147891410" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="testLdap" id="633141434529922778" vid="633141434529922777">
        <Properties type="Bool">testLdap</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="633141423147891413" treenode="633141423147891414" appnode="633141423147891411" handlerfor="633141423147891410">
    <node type="Start" id="633141423147891413" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="139" y="36">
      <linkto id="633141434529922758" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633141423147891415" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="140" y="424">
      <linkto id="633141434529922676" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="633141434529922678" type="Labeled" style="Bezier" ortho="true" label="-1" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="csharp">"SD:" + sampledata</ap>
        <ap name="Prompt" type="literal">See Japanese Characters?</ap>
        <ap name="Text" type="literal">ディズニーランド</ap>
        <rd field="ResultData">myText</rd>
        <log condition="entry" on="true" level="Info" type="literal">日本の</log>
      </Properties>
    </node>
    <node type="Action" id="633141423147891418" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="352" y="574">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633141434529922676" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="140" y="573">
      <linkto id="633141423147891418" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">myText.ToString()</ap>
        <ap name="URL" type="literal">10.89.30.232</ap>
        <ap name="Username" type="literal">jdliau1</ap>
        <ap name="Password" type="literal">12345</ap>
      </Properties>
    </node>
    <node type="Action" id="633141434529922678" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="351" y="424">
      <linkto id="633141423147891418" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="body" type="csharp">myText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <ap name="remoteHost" type="variable">rh</ap>
        <ap name="responseCode" type="literal">200</ap>
      </Properties>
    </node>
    <node type="Action" id="633141434529922707" name="ExecuteScalar" class="MaxActionNode" group="" path="Metreos.Native.Database" x="141" y="279">
      <linkto id="633141423147891415" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="literal">select sampledata from sample</ap>
        <ap name="Name" type="literal">database1</ap>
        <rd field="Scalar">sampledata</rd>
        <log condition="exit" on="true" level="Info" type="csharp">"SampleData: " + sampledata</log>
      </Properties>
    </node>
    <node type="Action" id="633141434529922758" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="139" y="123">
      <linkto id="633141434529922779" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="633141434529922707" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">testLdap</ap>
      </Properties>
    </node>
    <node type="Action" id="633141434529922779" name="Query" class="MaxActionNode" group="" path="Metreos.Native.Ldap" x="241" y="121">
      <linkto id="633141434529922707" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="LdapServerHost" type="literal">1</ap>
        <rd field="SearchResults">ldapResults</rd>
      </Properties>
    </node>
    <node type="Variable" id="633141423147891416" name="myText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">myText</Properties>
    </node>
    <node type="Variable" id="633141434529922675" name="rh" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">rh</Properties>
    </node>
    <node type="Variable" id="633141434529922708" name="sampledata" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">sampledata</Properties>
    </node>
    <node type="Variable" id="633141434529922780" name="ldapResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">ldapResults</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>