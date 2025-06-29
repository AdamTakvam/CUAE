<Application name="script2" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script2">
    <outline>
      <treenode type="evh" id="632497019601842287" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632497019601842284" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632497019601842283" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/RemoteAgentConcept</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632497019601842286" treenode="632497019601842287" appnode="632497019601842284" handlerfor="632497019601842283">
    <node type="Start" id="632497019601842286" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="401">
      <linkto id="632497025201910466" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632497025201910462" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="534" y="400">
      <linkto id="632497025201910465" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Command" type="csharp">"Update extensiontable set callto='" + form["extension"]+ "', sccpextension='" + random + "'";</ap>
        <ap name="Name" type="literal">RemoteAgentConcept</ap>
      </Properties>
    </node>
    <node type="Action" id="632497025201910465" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="712" y="467">
      <linkto id="632497025201910468" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">"&lt;html&gt;&lt;body&gt;&lt;p&gt;Your remote extension is '&lt;b&gt;" + random + "&lt;/b&gt;'&lt;/p&gt;&lt;/body&gt;&lt;/html&gt;"</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632497025201910466" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="221" y="395">
      <linkto id="632497025201910462" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string random)
{
		Random rand = new Random();
            random = rand.Next(2000, 3000).ToString();
		return "";
}
</Properties>
    </node>
    <node type="Action" id="632497025201910468" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="744" y="208">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632497019601842290" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632497025201910464" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632497025201910467" name="random" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">random</Properties>
    </node>
    <node type="Variable" id="632497025201910469" name="form" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.FormCollection" initWith="body" refType="reference" name="Metreos.Providers.Http.GotRequest">form</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>