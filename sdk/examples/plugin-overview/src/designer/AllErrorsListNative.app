<Application name="AllErrorsListNative" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="AllErrorsListNative">
    <outline>
      <treenode type="evh" id="632588502418711560" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632588502418711557" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632588502418711556" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/showerrorsnative</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632588502418711559" treenode="632588502418711560" appnode="632588502418711557" handlerfor="632588502418711556">
    <node type="Start" id="632588502418711559" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="254">
      <linkto id="632588639320468976" type="Basic" style="Bevel" />
    </node>
    <node type="Comment" id="632588502418711563" text="Specifying no start or end time&#xD;&#xA;indicates to the action to query all &#xD;&#xA;records with any timestamp value." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="245" y="152" />
    <node type="Action" id="632588502418711564" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="488" y="254">
      <linkto id="632588502418711567" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.DatabaseScraper.ErrorDataCollection errors, ref string responseText)
{
	responseText = "&lt;html&gt;&lt;body&gt;&lt;table&gt;&lt;th&gt;Time&lt;/th&gt;&lt;th&gt;Description&lt;/th&gt;";
	
	foreach(Metreos.DatabaseScraper.Common.ErrorData error in errors.Collection)
	{
		responseText += "&lt;tr&gt;&lt;td&gt;" + error.Time.ToLongDateString() + "&lt;/td&gt;&lt;td&gt;" + error.Description + "&lt;/td&gt;&lt;/tr&gt;";
	}
	
	responseText += "&lt;/table&gt;&lt;/body&gt;&lt;/html&gt;";

	return "";
}
</Properties>
    </node>
    <node type="Action" id="632588502418711567" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="663" y="256">
      <linkto id="632588502418711568" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="variable">responseText</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632588502418711568" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="835" y="254">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632588639320468976" name="Query" class="MaxActionNode" group="" path="Metreos.Native.DatabaseScraper" x="319" y="257">
      <linkto id="632588502418711564" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <rd field="Data">errors</rd>
      </Properties>
    </node>
    <node type="Variable" id="632588502418711561" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632588502418711565" name="errors" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.DatabaseScraper.ErrorDataCollection" refType="reference">errors</Properties>
    </node>
    <node type="Variable" id="632588502418711566" name="responseText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">responseText</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>