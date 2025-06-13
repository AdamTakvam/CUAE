<Application name="Menu" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Menu">
    <outline>
      <treenode type="evh" id="632875147412707623" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632875147412707620" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875147412707619" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/Menu</ep>
        </Properties>
      </treenode>
      <treenode type="fun" id="632875376037936885" level="1" text="CheckFileExists">
        <node type="function" name="CheckFileExists" id="632875376037936882" path="Metreos.StockTools" />
        <calls>
          <ref actid="632875376037936881" />
        </calls>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632875147412707622" treenode="632875147412707623" appnode="632875147412707620" handlerfor="632875147412707619">
    <node type="Loop" id="632875376037936320" name="Loop" text="loop (expr)" cx="154" cy="148" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="1432" y="48" mx="1509" my="122">
      <linkto id="632875376037936321" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632875147412707696" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">checkedLoggedIn.Rows</Properties>
    </node>
    <node type="Loop" id="632875376037936327" name="Loop" text="loop (expr)" cx="175" cy="164" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="1416" y="480" mx="1504" my="562">
      <linkto id="632875376037936328" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632875376037936881" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">checkedLoggedIn.Rows</Properties>
    </node>
    <node type="Start" id="632875147412707622" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="72" y="304">
      <linkto id="632875376037936314" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707686" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="544" y="304">
      <linkto id="632875147412707687" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632875376037936304" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">accept != null ? accept.IndexOf("image/png") &gt; -1 : false</ap>
      </Properties>
    </node>
    <node type="Action" id="632875147412707687" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="696" y="232">
      <linkto id="632875147412707689" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Prompt" type="literal">Make your selection</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707689" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="824" y="232">
      <linkto id="632875147412707694" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Store Inventory</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/StoreInventory?metreosSessionId=" + query["appId"]</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707693" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1232" y="232">
      <linkto id="632875376037936319" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Check Out</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/CheckOut?metreosSessionId=" + query["appId"]</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707694" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1096" y="232">
      <linkto id="632875147412707693" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Check In</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/CheckIn?metreosSessionId=" + query["appId"]</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875147412707696" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1816" y="232">
      <linkto id="632875376037936312" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936304" name="CreateGraphicFileMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="688" y="416">
      <linkto id="632875376037936306" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Prompt" type="literal">Make your selection</ap>
        <ap name="LocationX" type="literal">-1</ap>
        <ap name="LocationY" type="literal">-1</ap>
        <ap name="URL" type="csharp">"http://" + hostname + "/mceadmin/menu.png"</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936306" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="824" y="416">
      <linkto id="632875376037936324" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Store Inventory</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/StoreInventory?metreosSessionId=" + query["appId"]</ap>
        <ap name="TouchAreaX1" type="literal">1</ap>
        <ap name="TouchAreaX2" type="literal">69</ap>
        <ap name="TouchAreaY1" type="literal">1</ap>
        <ap name="TouchAreaY2" type="literal">69</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936310" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1816" y="416">
      <linkto id="632875376037936312" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">graphicMenu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936312" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1944" y="352">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632875376037936314" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="200" y="304">
      <linkto id="632875376037936316" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="literal">RetailDemo</ap>
        <ap name="Server" type="literal">127.0.0.1</ap>
        <ap name="Port" type="literal">3306</ap>
        <ap name="Username" type="literal">root</ap>
        <ap name="Password" type="literal">metreos</ap>
        <ap name="Pooling" type="literal">true</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936316" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="336" y="304">
      <linkto id="632875376037936317" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">Retail</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936317" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="448" y="304">
      <linkto id="632875147412707686" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="literal">SELECT username, extension, userId from users where checkedIn = 1</ap>
        <ap name="Name" type="literal">Retail</ap>
        <rd field="ResultSet">checkedLoggedIn</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936319" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1368" y="232">
      <linkto id="632875376037936320" port="1" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632875147412707696" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">checkedLoggedIn.Rows.Count &gt; 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936321" name="AddMenuItem" container="632875376037936320" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1520" y="120">
      <linkto id="632875376037936320" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">(loopEnum.Current as DataRow)["username"].ToString()</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/Call?metreosSessionId=" + query["appId"] + "&amp;ext=" + (loopEnum.Current as DataRow)["extension"] + "&amp;userId=" + (loopEnum.Current as DataRow)["userId"]</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936323" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1208" y="416">
      <linkto id="632875376037936330" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Check Out</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/CheckOut?metreosSessionId=" + query["appId"]</ap>
        <ap name="TouchAreaX1" type="literal">249</ap>
        <ap name="TouchAreaX2" type="literal">297</ap>
        <ap name="TouchAreaY1" type="literal">65</ap>
        <ap name="TouchAreaY2" type="literal">112</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936324" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1072" y="416">
      <linkto id="632875376037936323" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Check In</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/CheckIn?metreosSessionId=" + query["appId"]</ap>
        <ap name="TouchAreaX1" type="literal">207</ap>
        <ap name="TouchAreaX2" type="literal">249</ap>
        <ap name="TouchAreaY1" type="literal">65</ap>
        <ap name="TouchAreaY2" type="literal">112</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936328" name="AddMenuItem" container="632875376037936327" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1472" y="560">
      <linkto id="632875376037936332" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">(loopEnum.Current as DataRow)["username"].ToString()</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/Call?metreosSessionId=" + query["appId"] + "&amp;ext=" + (loopEnum.Current as DataRow)["extension"] + "&amp;userId=" + (loopEnum.Current as DataRow)["userId"]</ap>
        <ap name="TouchAreaX1" type="csharp">1 + 68 * count + 8 * count</ap>
        <ap name="TouchAreaX2" type="csharp">1 + 68 * count + 8 * count + 68</ap>
        <ap name="TouchAreaY1" type="literal">99</ap>
        <ap name="TouchAreaY2" type="literal">167</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936330" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1320" y="416">
      <linkto id="632875376037936327" port="1" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632875376037936881" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">checkedLoggedIn.Rows.Count &gt; 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936332" name="Assign" container="632875376037936327" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1552" y="560">
      <linkto id="632875376037936327" port="4" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">count + 1</ap>
        <rd field="ResultData">count</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936881" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1624" y="400" mx="1671" my="416">
      <items count="1">
        <item text="CheckFileExists" />
      </items>
      <linkto id="632875376037936310" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">CheckFileExists</ap>
      </Properties>
    </node>
    <node type="Variable" id="632875147412707685" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">query</Properties>
    </node>
    <node type="Variable" id="632875147412707688" name="menu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">menu</Properties>
    </node>
    <node type="Variable" id="632875147412707695" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632875147412707697" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875376037936303" name="accept" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Accept" defaultInitWith="NONE" refType="reference">accept</Properties>
    </node>
    <node type="Variable" id="632875376037936305" name="graphicMenu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.GraphicFileMenu" refType="reference">graphicMenu</Properties>
    </node>
    <node type="Variable" id="632875376037936313" name="hostname" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="hostname" refType="reference" name="Metreos.Providers.Http.GotRequest">hostname</Properties>
    </node>
    <node type="Variable" id="632875376037936315" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632875376037936318" name="checkedLoggedIn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkedLoggedIn</Properties>
    </node>
    <node type="Variable" id="632875376037936331" name="count" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" defaultInitWith="0" refType="reference">count</Properties>
    </node>
    <node type="Variable" id="632875376037936606" name="image" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Imaging.ImageBuilder" refType="reference">image</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="CheckFileExists" startnode="632875376037936884" treenode="632875376037936885" appnode="632875376037936882" handlerfor="632875147412707619">
    <node type="Start" id="632875376037936884" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="328">
      <linkto id="632875376037936889" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875376037936886" name="CreateImageBuilder" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="360" y="240">
      <linkto id="632875376037936887" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <rd field="ImageBuilder">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936887" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="512" y="240">
      <linkto id="632875376037936888" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="literal">c:\images\background2.png</ap>
        <ap name="Top" type="literal">0</ap>
        <ap name="Left" type="literal">0</ap>
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <rd field="Image">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936888" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="672" y="240">
      <linkto id="632875376037936896" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Imaging.ImageBuilder image)
{

	image.Save("c:\\metreos\\mceadmin\\public\\menu.png");
	return "";
}
</Properties>
    </node>
    <node type="Action" id="632875376037936889" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="240" y="328">
      <linkto id="632875376037936886" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632875376037936896" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties language="csharp">
public static string Execute()
{
	return System.IO.File.Exists("c:\\metreos\\mceadmin\\public\\menu.png").ToString();
}
</Properties>
    </node>
    <node type="Comment" id="632875376037936890" text="Check if the basic background menu exists..." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="344" y="366" />
    <node type="Action" id="632875376037936896" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="760" y="328">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632875376037936897" name="image" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Imaging.ImageBuilder" refType="reference">image</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>