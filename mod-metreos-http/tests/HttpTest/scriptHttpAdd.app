<Application name="scriptHttpAdd" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="scriptHttpAdd">
    <outline>
      <treenode type="evh" id="632514932224651690" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632514932224651687" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632514932224651686" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/myadd</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632514932224651689" treenode="632514932224651690" appnode="632514932224651687" handlerfor="632514932224651686">
    <node type="Start" id="632514932224651689" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632514932224651712" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632514932224651708" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="221" y="253">
      <linkto id="632514932224651709" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remoteHost" type="variable">rHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="variable">ret</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632514932224651709" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="540" y="290">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632514932224651712" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="397" y="63">
      <linkto id="632514932224651708" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(Metreos.Types.Http.QueryParamCollection rQuery, ref string ret)
{
	// TODO: add parameters with same name and type as variables
	// TODO: add function body
	if (rQuery.Count != 2)
	{
		ret = "Invalid Query!, Try /myadd?a=1&amp;b=2";
		return string.Empty;
	}
	try 
	{
	int addRet = System.Convert.ToInt32(rQuery[0]) + System.Convert.ToInt32(rQuery[1]);
	ret =  "The result is " + addRet.ToString();
	}
	catch
	{
		ret = "Invalid Query Data Format! Try string concatenation and result is " + rQuery[0].ToString() + rQuery[1].ToString();
	}

	return string.Empty;
}
</Properties>
    </node>
    <node type="Variable" id="632514932224651710" name="rHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">rHost</Properties>
    </node>
    <node type="Variable" id="632514932224651713" name="rQuery" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">rQuery</Properties>
    </node>
    <node type="Variable" id="632514932224651714" name="ret" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">ret</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>