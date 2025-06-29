<Application name="AmazonWebServices" trigger="Metreos.Providers.Http.GotRequest" version="0.7" single="false">
  <!--//// app tree ////-->
  <global name="AmazonWebServices">
    <outline>
      <treenode type="evh" id="632283679947031425" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632283679947031422" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632283679947031421" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="variable">g_mainUrl</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632283716589843954" level="1" text="GotRequest: ItemListing">
        <node type="function" name="ItemListing" id="632283716589843951" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632283716589843950" path="Metreos.Providers.Http.GotRequest" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/AmazonWebService/ItemListing</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632283716589843971" level="1" text="GotRequest: ItemSearch">
        <node type="function" name="ItemSearch" id="632283716589843968" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632283716589843967" path="Metreos.Providers.Http.GotRequest" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/AmazonWebService/ItemSearch</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632283977184687751" level="1" text="GotRequest: ItemDescription">
        <node type="function" name="ItemDescription" id="632283977184687748" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632283977184687747" path="Metreos.Providers.Http.GotRequest" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/AmazonWebService/ItemDescription</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632283977184687767" level="1" text="SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="632283977184687764" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="632283977184687763" path="Metreos.Providers.Http.SessionExpired" />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_devtag" id="632302536409688563" vid="632283716589843973">
        <Properties type="Metreos.Types.String" initWith="Amazon_devtag">g_devtag</Properties>
      </treenode>
      <treenode text="g_currentPage" id="632302536409688565" vid="632283716589843975">
        <Properties type="Metreos.Types.UShort">g_currentPage</Properties>
      </treenode>
      <treenode text="g_numMorePages" id="632302536409688567" vid="632283716589843977">
        <Properties type="Metreos.Types.UShort">g_numMorePages</Properties>
      </treenode>
      <treenode text="g_proxyUri" id="632302536409688569" vid="632283716589844147">
        <Properties type="Metreos.Types.String" initWith="Proxy_Uri">g_proxyUri</Properties>
      </treenode>
      <treenode text="g_mainUrl" id="632302536409688647" vid="632302536409688646">
        <Properties type="String" initWith="mainUrl">g_mainUrl</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632283679947031424" treenode="632283679947031425" appnode="632283679947031422" handlerfor="632283679947031421">
    <node type="Start" id="632283679947031424" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632283679947031426" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632283679947031426" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="171" y="355">
      <linkto id="632283679947031428" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Title" type="literal">Search Amazon</ap>
        <ap name="Prompt" type="literal">Choose a search</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283679947031428" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="290" y="355">
      <linkto id="632283679947031429" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">All Products</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemListing?item=none&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283679947031429" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="420" y="355">
      <linkto id="632283679947031437" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Books</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemListing?item=books&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283679947031430" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="920" y="356">
      <linkto id="632283679947031431" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Outdoor Living</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemListing?item=garden&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283679947031431" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="292" y="486">
      <linkto id="632283679947031443" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Kitchen &amp; Housewares</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemListing?item=kitchen&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283679947031433" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="917" y="480">
      <linkto id="632283679947031446" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Software</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemListing?item=software&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283679947031434" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="657" y="481">
      <linkto id="632283679947031445" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Computers</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemListing?item=pc-hardware&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283679947031435" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="794" y="356">
      <linkto id="632283679947031430" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Electronics</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemListing?item=electronics&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283679947031436" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="663" y="356">
      <linkto id="632283679947031435" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">DVD</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemListing?item=dvd&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283679947031437" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="544" y="356">
      <linkto id="632283679947031436" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Classical Music</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemListing?item=classical&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Comment" id="632283679947031440" text="All Products&#xD;&#xA;Books&#xD;&#xA;Classical Music&#xD;&#xA;DVD&#xD;&#xA;Electronics&#xD;&#xA;Outdoor Living&#xD;&#xA;Kitchen &amp; Housewares&#xD;&#xA;Magazines&#xD;&#xA;Popular Music&#xD;&#xA;Computers&#xD;&#xA;Camera &amp; Photo&#xD;&#xA;Software&#xD;&#xA;Toys &amp; Games&#xD;&#xA;Tools &amp; Hardware&#xD;&#xA;Video&#xD;&#xA;Computer &amp; Video Games" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="495" y="54" />
    <node type="Action" id="632283679947031443" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="416" y="483">
      <linkto id="632283679947031444" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Magazines</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemListing?item=magazines&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283679947031444" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="543" y="482">
      <linkto id="632283679947031434" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Popular Music</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemListing?item=music&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283679947031445" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="792" y="480">
      <linkto id="632283679947031433" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Camera &amp; Photo</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemListing?item=photo&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283679947031446" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="290" y="596">
      <linkto id="632283679947031448" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Toys &amp; Games</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemListing?item=toys&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283679947031448" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="416" y="594">
      <linkto id="632283679947031453" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Tools &amp; Hardware</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemListing?item=universal&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283679947031453" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="548" y="593">
      <linkto id="632283679947031455" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Video</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemListing?item=vhs&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283679947031455" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="663" y="593">
      <linkto id="632283679947031457" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Computers &amp; Video Games</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemListing?item=videogames&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283679947031457" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="793" y="590">
      <linkto id="632283679947031458" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
      </Properties>
    </node>
    <node type="Action" id="632283679947031458" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="924" y="590">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632302505097187990" text="Creating listing on phone of these following topics" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="431" y="32" />
    <node type="Variable" id="632283679947031427" name="menu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">menu</Properties>
    </node>
    <node type="Variable" id="632283679947031438" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632283679947031441" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632283679947031442" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="host" refType="reference">host</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ItemListing" startnode="632283716589843953" treenode="632283716589843954" appnode="632283716589843951" handlerfor="632283716589843950">
    <node type="Start" id="632283716589843953" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="371">
      <linkto id="632283716589843958" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632283716589843958" name="CreateInput" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="232" y="369">
      <linkto id="632283716589843964" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Title" type="literal">Search Amazon</ap>
        <ap name="Prompt" type="literal">Enter search phrase</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemSearch?item=" + queryParams["item"] + "&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632283716589843964" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="394" y="370">
      <linkto id="632283716589843965" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="DisplayName" type="literal">Search:</ap>
        <ap name="QueryStringParam" type="literal">keyword</ap>
        <ap name="InputFlags" type="literal">A</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632283716589843965" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="546" y="369">
      <linkto id="632283716589843966" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">input.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
      </Properties>
    </node>
    <node type="Action" id="632283716589843966" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="684" y="369">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632284002586562771" text="Allow the user to enter a search phrase" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="309" y="260" />
    <node type="Variable" id="632283716589843959" name="input" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Input" refType="reference">input</Properties>
    </node>
    <node type="Variable" id="632283716589843960" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632283716589843961" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632283716589843962" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632283716589843998" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParams</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ItemSearch" startnode="632283716589843970" treenode="632283716589843971" appnode="632283716589843968" handlerfor="632283716589843967">
    <node type="Loop" id="632283716589843982" name="Loop" text="loop (expr)" cx="120" cy="120" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="431" y="314" mx="491" my="374">
      <linkto id="632283716589843983" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632283716589843994" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="csharp">results.Count</Properties>
    </node>
    <node type="Start" id="632283716589843970" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="374">
      <linkto id="632283716589843972" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632283716589843972" name="SearchByKeyword" class="MaxActionNode" group="" path="Metreos.Native.AmazonWebServices" x="149" y="374">
      <linkto id="632283716589843985" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Keyword" type="csharp">queryParams["keyword"]</ap>
        <ap name="Devtag" type="variable">g_devtag</ap>
        <ap name="ItemType" type="csharp">queryParams["item"]</ap>
        <ap name="Page" type="literal">1</ap>
        <ap name="Proxy" type="variable">g_proxyUri</ap>
        <rd field="ResultData">results</rd>
        <rd field="NumMorePages">g_numMorePages</rd>
      </Properties>
    </node>
    <node type="Action" id="632283716589843983" name="AddMenuItem" container="632283716589843982" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="493" y="374">
      <linkto id="632283716589843982" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="csharp">(results[loopIndex] as string[])[1]</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/ItemDescription?asin=" + (results[loopIndex] as string[])[0] + "&amp;metreosSessionId=" + routingGuid + "&amp;item=" + queryParams["item"]</ap>
        <rd field="ResultData">menu</rd>
        <log condition="exit" on="true" level="Info" type="csharp">(results[loopIndex] as string[])[1] + " " +host + "/AmazonWebService/ItemDescription?asin=" + (results[loopIndex] as string[])[0] </log>
      </Properties>
    </node>
    <node type="Action" id="632283716589843986" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="348" y="373">
      <linkto id="632283716589843982" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Title" type="literal">Item Display</ap>
        <ap name="Prompt" type="literal">Choose Item</ap>
        <rd field="ResultData">menu</rd>
        <log condition="entry" on="true" level="Info" type="literal">Making Item List</log>
      </Properties>
    </node>
    <node type="Action" id="632283716589843987" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="252" y="487">
      <linkto id="632283716589843989" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Title" type="literal">Amazon Search</ap>
        <ap name="Prompt" type="literal">Choose option</ap>
        <ap name="Text" type="literal">No results for search.  Try a new search</ap>
        <rd field="ResultData">errorText</rd>
        <log condition="entry" on="true" level="Info" type="literal">No results</log>
      </Properties>
    </node>
    <node type="Action" id="632283716589843989" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="253" y="604">
      <linkto id="632283716589843990" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">MainMenu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebService/MainMenu"</ap>
        <rd field="ResultData">errorText</rd>
      </Properties>
    </node>
    <node type="Action" id="632283716589843990" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="254" y="721">
      <linkto id="632283716589843992" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">errorText</rd>
      </Properties>
    </node>
    <node type="Action" id="632283716589843992" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="255" y="812">
      <linkto id="632283716589843993" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">errorText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
      </Properties>
    </node>
    <node type="Action" id="632283716589843993" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="257" y="938">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632283716589843994" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="658" y="375">
      <linkto id="632283716589843996" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
      </Properties>
    </node>
    <node type="Action" id="632283716589843996" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="780" y="374">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632283716589843985" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="253" y="374">
      <linkto id="632283716589843986" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632283716589843987" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">results.Count &gt; 0</ap>
      </Properties>
    </node>
    <node type="Comment" id="632284002586562772" text="If no results were returned, &#xD;&#xA;prompt the user to &#xD;&#xA;perform a new search" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="320" y="470" />
    <node type="Comment" id="632284002586562775" text="Query Amazon with the search phrase&#xD;&#xA;and category of item chosen" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="58" y="268" />
    <node type="Comment" id="632284002586562773" text="Create menu listing of the items &#xD;&#xA;returned by the SearchByKeyword &#xD;&#xA;action." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="397" y="154" />
    <node type="Comment" id="632284002586562776" text="Send the user on to &#xD;&#xA;the ItemDescription event if they&#xD;&#xA;choose a menu item" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="399" y="227" />
    <node type="Variable" id="632283716589843979" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632283716589843980" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632283716589843981" name="results" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.ArrayList" refType="reference">results</Properties>
    </node>
    <node type="Variable" id="632283716589843984" name="menu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">menu</Properties>
    </node>
    <node type="Variable" id="632283716589843988" name="errorText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">errorText</Properties>
    </node>
    <node type="Variable" id="632283716589843997" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParams</Properties>
    </node>
    <node type="Variable" id="632283977184687770" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ItemDescription" startnode="632283977184687750" treenode="632283977184687751" appnode="632283977184687748" handlerfor="632283977184687747">
    <node type="Start" id="632283977184687750" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="332">
      <linkto id="632283977184687752" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632283977184687752" name="GetItemDescription" class="MaxActionNode" group="" path="Metreos.Native.AmazonWebServices" x="247" y="335">
      <linkto id="632283977184687757" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Devtag" type="variable">g_devtag</ap>
        <ap name="ItemType" type="csharp">queryParams["item"]</ap>
        <ap name="Asin" type="csharp">queryParams["asin"]</ap>
        <ap name="Proxy" type="variable">g_proxyUri</ap>
        <rd field="ResultData">itemDescription</rd>
      </Properties>
    </node>
    <node type="Action" id="632283977184687757" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="388" y="335">
      <linkto id="632283977184687759" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Title" type="literal">Amazon Product</ap>
        <ap name="Prompt" type="literal">Choose option</ap>
        <ap name="Text" type="variable">itemDescription</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632283977184687759" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="510" y="335">
      <linkto id="632283977184687760" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">MainMenu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/AmazonWebServices/MainMenu?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632283977184687760" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="625" y="336">
      <linkto id="632283977184687761" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632283977184687761" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="740" y="336">
      <linkto id="632283977184687762" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
      </Properties>
    </node>
    <node type="Action" id="632283977184687762" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="868.4121" y="336">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632284002586562774" text="Make a request to the Amazon Web Service based on the ASIN, &#xD;&#xA;or Amazon Standard Item Number" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="316" y="213" />
    <node type="Variable" id="632283977184687753" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632283977184687754" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632283977184687755" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632283977184687756" name="itemDescription" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">itemDescription</Properties>
    </node>
    <node type="Variable" id="632283977184687758" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632283977184687769" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParams</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="632283977184687766" treenode="632283977184687767" appnode="632283977184687764" handlerfor="632283977184687763">
    <node type="Start" id="632283977184687766" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632283977184687768" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632283977184687768" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="530" y="273">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>