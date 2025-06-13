<Application name="Portal" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Portal">
    <outline>
      <treenode type="evh" id="632875147412707541" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632875147412707538" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875147412707537" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/Start</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707595" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632875147412707592" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632875147412707591" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="633168961358426010" actid="632875147412707601" />
          <ref id="633168961358426098" actid="632875291003497556" />
          <ref id="633168961358426110" actid="632875304714864906" />
          <ref id="633168961358426124" actid="632875376037936230" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707600" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632875147412707597" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632875147412707596" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="633168961358426011" actid="632875147412707601" />
          <ref id="633168961358426099" actid="632875291003497556" />
          <ref id="633168961358426111" actid="632875304714864906" />
          <ref id="633168961358426125" actid="632875376037936230" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707610" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632875147412707607" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632875147412707606" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707887" level="2" text="Metreos.Providers.Http.GotRequest: StoreInventory">
        <node type="function" name="StoreInventory" id="632875147412707884" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875147412707883" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/StoreInventory</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707907" level="2" text="Metreos.Providers.Http.GotRequest: CheckIn">
        <node type="function" name="CheckIn" id="632875147412707904" path="Metreos.StockTools" />
        <calls>
          <ref actid="632875304714864938" />
        </calls>
        <node type="event" name="GotRequest" id="632875147412707903" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/CheckIn</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875147412707912" level="2" text="Metreos.Providers.Http.GotRequest: CheckOut">
        <node type="function" name="CheckOut" id="632875147412707909" path="Metreos.StockTools" />
        <calls>
          <ref actid="632875376037936254" />
        </calls>
        <node type="event" name="GotRequest" id="632875147412707908" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/CheckOut</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875291003497565" level="2" text="Metreos.Providers.Http.GotRequest: StoreInventorySKU">
        <node type="function" name="StoreInventorySKU" id="632875291003497562" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875291003497561" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/StoreInventorySKU</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875304714864922" level="2" text="Metreos.Providers.Http.GotRequest: CheckInSubmit">
        <node type="function" name="CheckInSubmit" id="632875304714864919" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875304714864918" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/CheckInSubmit</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875376037936249" level="2" text="Metreos.Providers.Http.GotRequest: CheckOutSubmit">
        <node type="function" name="CheckOutSubmit" id="632875376037936246" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875376037936245" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/CheckOutSubmit</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875376037937410" level="2" text="Metreos.Providers.Http.GotRequest: Call">
        <node type="function" name="Call" id="632875376037937407" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632875376037937406" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/Call</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632875376037937730" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632875376037937727" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632875376037937726" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="633168961358426199" actid="632875376037937736" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632875376037937735" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632875376037937732" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632875376037937731" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="633168961358426200" actid="632875376037937736" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632889791566825608" level="2" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="632889791566825605" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="632889791566825604" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632896050178119781" level="2" text="Metreos.Providers.Http.GotRequest: UpdateStatus">
        <node type="function" name="UpdateStatus" id="632896050178119778" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632896050178119777" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Retail/UpdateStatus</ep>
        </Properties>
      </treenode>
      <treenode type="fun" id="632875376037936544" level="1" text="UpdateImage">
        <node type="function" name="UpdateImage" id="632875376037936541" path="Metreos.StockTools" />
        <calls>
          <ref actid="632875376037936540" />
          <ref actid="632875376037936565" />
        </calls>
      </treenode>
      <treenode type="fun" id="632889956132003337" level="1" text="CheckFileExists">
        <node type="function" name="CheckFileExists" id="632889956132003334" path="Metreos.StockTools" />
        <calls>
          <ref actid="632889956132002983" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_connectionId" id="633168961358425976" vid="632875147412707546">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="633168961358425978" vid="632875147412707548">
        <Properties type="String">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_phoneIP" id="633168961358425980" vid="632875147412707550">
        <Properties type="String">g_phoneIP</Properties>
      </treenode>
      <treenode text="g_appServerIP" id="633168961358425982" vid="632875147412707552">
        <Properties type="String">g_appServerIP</Properties>
      </treenode>
      <treenode text="g_phoneUser" id="633168961358425984" vid="632875147412707558">
        <Properties type="String" initWith="PhoneUser">g_phoneUser</Properties>
      </treenode>
      <treenode text="g_phonePass" id="633168961358425986" vid="632875147412707560">
        <Properties type="String" initWith="PhonePass">g_phonePass</Properties>
      </treenode>
      <treenode text="g_operationId" id="633168961358425988" vid="632875147412707604">
        <Properties type="String">g_operationId</Properties>
      </treenode>
      <treenode text="g_mainMenuPath" id="633168961358425990" vid="632896050178119016">
        <Properties type="String" initWith="MainMenuFilePath">g_mainMenuPath</Properties>
      </treenode>
      <treenode text="g_skuLookupPath" id="633168961358425992" vid="632896050178119018">
        <Properties type="String" initWith="SkuItemFilePath">g_skuLookupPath</Properties>
      </treenode>
      <treenode text="g_greenIconPath" id="633168961358425994" vid="632896050178119020">
        <Properties type="String" initWith="AvailableIconFilePath">g_greenIconPath</Properties>
      </treenode>
      <treenode text="g_redIconPath" id="633168961358425996" vid="632896050178119022">
        <Properties type="String" initWith="UnavailableIconFilePath">g_redIconPath</Properties>
      </treenode>
      <treenode text="g_device" id="633168961358425998" vid="632898037366593163">
        <Properties type="String">g_device</Properties>
      </treenode>
      <treenode text="g_mainMenuClosedPath" id="633168961358426000" vid="632898160147302411">
        <Properties type="String" initWith="MainMenuClosedFilePath">g_mainMenuClosedPath</Properties>
      </treenode>
      <treenode text="g_dbUsername" id="633168961358426002" vid="632986053913956201">
        <Properties type="String" initWith="DatabaseUser">g_dbUsername</Properties>
      </treenode>
      <treenode text="g_dbPassword" id="633168961358426004" vid="632986053913956203">
        <Properties type="String" initWith="DatabasePassword">g_dbPassword</Properties>
      </treenode>
      <treenode text="g_mceadminInstallPath" id="633168961358426281" vid="633168961358426280">
        <Properties type="String" initWith="Config.MceAdminInstallPath">g_mceadminInstallPath</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632875147412707540" treenode="632875147412707541" appnode="632875147412707538" handlerfor="632875147412707537">
    <node type="Loop" id="632889956132002968" name="Loop" text="loop (expr)" cx="154" cy="148" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="1384.18884" y="57" mx="1461" my="131">
      <linkto id="632889956132002969" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632889956132002976" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">checkedLoggedIn.Rows</Properties>
    </node>
    <node type="Loop" id="632889956132002970" name="Loop" text="loop (expr)" cx="280.188843" cy="164" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="1228" y="493" mx="1368" my="575">
      <linkto id="632896050178119798" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632889956132002983" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">checkedLoggedIn.Rows</Properties>
    </node>
    <node type="Loop" id="632889956132003698" name="Loop" text="loop (expr)" cx="175" cy="164" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="1329.18884" y="756" mx="1417" my="838">
      <linkto id="632889956132003699" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632889956132002983" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">checkedLoggedIn.Rows</Properties>
    </node>
    <node type="Start" id="632875147412707540" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="312">
      <linkto id="632889791566825602" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707554" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="2271" y="396">
      <linkto id="632875147412707557" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"RTPRx:" + g_appServerIP +  ":" + txPort</ap>
        <ap name="URL2" type="csharp">"RTPTx:" + g_appServerIP +  ":" + rxPort</ap>
        <rd field="ResultData">execute</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"RTPRx:" + hostname +  ":" + txPort + "\n" + "RTPTx:" + g_appServerIP +  ":" + rxPort</log>
      </Properties>
    </node>
    <node type="Action" id="632875147412707557" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="2396" y="395">
      <linkto id="632889956132003681" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">g_phoneIP</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Sent command to phone IP:  " + g_phoneIP</log>
      </Properties>
    </node>
    <node type="Action" id="632875147412707590" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2740" y="396">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632875147412707601" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="2583" y="379" mx="2636" my="395">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632875147412707595" />
        <item text="OnPlay_Failed" treenode="632875147412707600" />
      </items>
      <linkto id="632875147412707590" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="literal">Please make a selection</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Comment" id="632889791566825599" text="Establish media channel with phone--&#xD;&#xA;we know it's IP address.  Send that IP a &#xD;&#xA;SendExecute a 'RTPRx' command to&#xD;&#xA;create a media connection to bridge&#xD;&#xA;to phone and CU Media Engine" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="2240" y="439" />
    <node type="Action" id="632889791566825600" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="2157" y="396">
      <linkto id="632875147412707554" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxIP" type="variable">remoteIP</ap>
        <ap name="MediaTxPort" type="variable">txPort</ap>
        <rd field="MediaRxIP">g_appServerIP</rd>
        <rd field="MediaRxPort">rxPort</rd>
        <rd field="ConnectionId">g_connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632889791566825602" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="136" y="312">
      <linkto id="632889956132002987" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">remoteIP</ap>
        <ap name="Value2" type="csharp">new Random(System.Environment.TickCount).Next(20482, 32768)</ap>
        <ap name="Value3" type="csharp">query["device"]</ap>
        <rd field="ResultData">g_phoneIP</rd>
        <rd field="ResultData2">txPort</rd>
        <rd field="ResultData3">g_device</rd>
      </Properties>
    </node>
    <node type="Comment" id="632889791566825613" text="Choose a port on the IP phone to use using random number generator.&#xD;&#xA;&#xD;&#xA;Some IP phones strictly enforce range of 20480 - 32768, so, this is our range to use" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="47" y="369" />
    <node type="Action" id="632889956132002969" name="AddMenuItem" container="632889956132002968" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1472.18884" y="129">
      <linkto id="632889956132002968" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">(loopEnum.Current as DataRow)["username"].ToString()</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/Call?metreosSessionId=" + routingGuid + "&amp;ext=" + (loopEnum.Current as DataRow)["extension"] + "&amp;userId=" + (loopEnum.Current as DataRow)["userId"]</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632889956132002971" name="AddMenuItem" container="632889956132002970" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1384" y="528">
      <linkto id="632889956132002972" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">(loopEnum.Current as DataRow)["username"].ToString()</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/Call?metreosSessionId=" + routingGuid + "&amp;ext=" + (loopEnum.Current as DataRow)["extension"] + "&amp;userId=" + (loopEnum.Current as DataRow)["userId"]</ap>
        <ap name="TouchAreaX1" type="csharp">(positions[(loopEnum.Current as DataRow)["userId"]] as string[])[0]</ap>
        <ap name="TouchAreaX2" type="csharp">(positions[(loopEnum.Current as DataRow)["userId"]] as string[])[2]</ap>
        <ap name="TouchAreaY1" type="csharp">(positions[(loopEnum.Current as DataRow)["userId"]] as string[])[1]</ap>
        <ap name="TouchAreaY2" type="csharp">(positions[(loopEnum.Current as DataRow)["userId"]] as string[])[3]</ap>
        <rd field="ResultData">graphicMenu</rd>
        <log condition="entry" on="true" level="Info" type="csharp">(loopEnum.Current as DataRow)["username"]</log>
      </Properties>
    </node>
    <node type="Action" id="632889956132002972" name="Assign" container="632889956132002970" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1470.18884" y="574">
      <linkto id="632889956132002970" port="4" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">count + 1</ap>
        <rd field="ResultData">count</rd>
      </Properties>
    </node>
    <node type="Action" id="632889956132002973" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="883.1888" y="240">
      <linkto id="632889956132002975" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Store Inventory</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/StoreInventory?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632889956132002974" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1130.18884" y="238">
      <linkto id="632889956132002979" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Check Out</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/CheckOut?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632889956132002975" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1003.18872" y="239">
      <linkto id="632889956132002974" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Check In</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/CheckIn?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632889956132002976" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1768.18884" y="241">
      <linkto id="632889956132003677" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632889956132002977" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="883.1888" y="425">
      <linkto id="632898037366592279" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Store Inventory</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/StoreInventory?metreosSessionId=" + routingGuid</ap>
        <ap name="TouchAreaX1" type="literal">4</ap>
        <ap name="TouchAreaX2" type="literal">69</ap>
        <ap name="TouchAreaY1" type="literal">5</ap>
        <ap name="TouchAreaY2" type="literal">95</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632889956132002978" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1948.18884" y="579">
      <linkto id="632889956132003678" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">graphicMenu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632889956132002979" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1320.18884" y="241">
      <linkto id="632889956132002968" port="1" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632889956132002976" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">checkedLoggedIn.Rows.Count &gt; 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632889956132002980" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1218.189" y="424">
      <linkto id="632889956132002982" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Update Status</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/UpdateStatus?metreosSessionId=" + routingGuid</ap>
        <ap name="TouchAreaX1" type="literal">257</ap>
        <ap name="TouchAreaX2" type="literal">291</ap>
        <ap name="TouchAreaY1" type="literal">5</ap>
        <ap name="TouchAreaY2" type="literal">32</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632889956132002982" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1326.18884" y="424">
      <linkto id="632889956132002970" port="1" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632889956132002983" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">open</ap>
      </Properties>
    </node>
    <node type="Action" id="632889956132002983" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1765.40369" y="561" mx="1812" my="577">
      <items count="1">
        <item text="CheckFileExists" />
      </items>
      <linkto id="632889956132002978" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">CheckFileExists</ap>
      </Properties>
    </node>
    <node type="Action" id="632889956132002984" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="597.1888" y="312">
      <linkto id="632889956132002985" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632889956132003684" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">accept != null ? accept.IndexOf("image/png") &gt; -1 : false</ap>
      </Properties>
    </node>
    <node type="Action" id="632889956132002985" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="755.1888" y="240">
      <linkto id="632889956132002973" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Prompt" type="literal">Make your selection</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632889956132002986" name="CreateGraphicFileMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="747.1888" y="424">
      <linkto id="632889956132002977" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Prompt" type="literal">Make your selection</ap>
        <ap name="LocationX" type="literal">-1</ap>
        <ap name="LocationY" type="literal">-1</ap>
        <ap name="URL" type="csharp">open ? "http://" + hostname + "/mceadmin/menu.png" : "http://" + hostname + "/mceadmin/menuClosed.png"</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632889956132002987" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="259.189453" y="312">
      <linkto id="632889956132002988" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="literal">RetailDemo</ap>
        <ap name="Server" type="literal">127.0.0.1</ap>
        <ap name="Port" type="literal">3306</ap>
        <ap name="Username" type="variable">g_dbUsername</ap>
        <ap name="Password" type="variable">g_dbPassword</ap>
        <ap name="Pooling" type="literal">true</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632889956132002988" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="396.188782" y="312">
      <linkto id="632889956132002989" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">RetailDemo</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Action" id="632889956132002989" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="509.188721" y="313">
      <linkto id="632898037366592272" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="literal">SELECT username, extension, userId, checkedIn from users</ap>
        <ap name="Name" type="literal">RetailDemo</ap>
        <rd field="ResultSet">checkedLoggedIn</rd>
      </Properties>
    </node>
    <node type="Label" id="632889956132003677" text="b" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1900.778" y="234" />
    <node type="Label" id="632889956132003678" text="b" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="2053.77783" y="579" />
    <node type="Label" id="632889956132003679" text="b" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1975" y="397">
      <linkto id="632891776627983164" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632889956132003681" name="CreateConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="2509" y="396">
      <linkto id="632875147412707601" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <rd field="ConferenceId">g_conferenceId</rd>
      </Properties>
    </node>
    <node type="Comment" id="632889956132003682" text="Sample HTTP request from 7921: &#xD;&#xA;GET /Retail/Start HTTP/1.0&#xD;&#xA;User-Agent: Allegro-Software-WebClient/3.12&#xD;&#xA;Host:10.89.31.78:8000&#xD;&#xA;x-CiscoIPPhoneModelName: CP-7921G&#xD;&#xA;x-CiscoIPPhoneDisplay: 176, 140, 16, C&#xD;&#xA;x-CiscoIPPhoneSDKVersion: 4.1.3&#xD;&#xA;Accept: x-CiscoIPPhone/Directory, x-CiscoIPPhone/Execute, x-CiscoIPPhone/IconMenu, x-CiscoIPPhone/IconFileMenu, x-CiscoIPPhone/Image, x-CiscoIPPhone/ImageFile, x-CiscoIPPhone/GraphicMenu, x-CiscoIPPhone/GraphicFileMenu, x-CiscoIPPhone/Input, x-CiscoIPPhone/Menu, x-CiscoIPPhone/Text, image/png, text/*&#xD;&#xA;Accept-Language: en&#xD;&#xA;Accept-Charset: iso-8859-1&#xD;&#xA;Connection: Close&#xD;&#xA;Cookie: ASPSESSIONIDQQDQSBSQ=GMCFNOAAHHPBBOHLABNHCDAF&#xD;&#xA;&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="401.777954" y="1229" />
    <node type="Action" id="632889956132003684" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="600" y="612">
      <linkto id="632889956132002986" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632889956132003690" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">modelNameHeader == "CP-7921G"</ap>
      </Properties>
    </node>
    <node type="Comment" id="632889956132003685" text="Determine if 7921" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="471" y="604" />
    <node type="Action" id="632889956132003686" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="879.1888" y="761">
      <linkto id="632889956132003688" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Store Inventory</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/StoreInventory?metreosSessionId=" + routingGuid</ap>
        <ap name="TouchAreaX1" type="literal">1</ap>
        <ap name="TouchAreaX2" type="literal">69</ap>
        <ap name="TouchAreaY1" type="literal">1</ap>
        <ap name="TouchAreaY2" type="literal">69</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632889956132003687" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1175.18884" y="762">
      <linkto id="632889956132003689" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Check Out</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/CheckOut?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632889956132003688" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1023.18872" y="761">
      <linkto id="632889956132003687" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Check In</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/CheckIn?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632889956132003689" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1268.18884" y="762">
      <linkto id="632889956132002983" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632889956132003698" port="1" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">checkedLoggedIn.Rows.Count &gt; 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632889956132003690" name="CreateGraphicFileMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="743.1888" y="761">
      <linkto id="632889956132003686" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Prompt" type="literal">Make your selection</ap>
        <ap name="LocationX" type="literal">-1</ap>
        <ap name="LocationY" type="literal">-1</ap>
        <ap name="URL" type="csharp">"http://" + hostname + "/mceadmin/menu.png"</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Comment" id="632889956132003691" text="Build 7921-menu&#xD;&#xA;(scaled down png graphics and&#xD;&#xA;no touch areas defined)" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="675" y="806" />
    <node type="Action" id="632889956132003699" name="AddMenuItem" container="632889956132003698" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1385.18884" y="836">
      <linkto id="632889956132003700" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">(loopEnum.Current as DataRow)["username"].ToString()</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/Call?metreosSessionId=" + routingGuid + "&amp;ext=" + (loopEnum.Current as DataRow)["extension"] + "&amp;userId=" + (loopEnum.Current as DataRow)["userId"]</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632889956132003700" name="Assign" container="632889956132003698" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1465.18884" y="836">
      <linkto id="632889956132003698" port="4" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">count + 1</ap>
        <rd field="ResultData">count</rd>
      </Properties>
    </node>
    <node type="Comment" id="632890909264297925" text="Build 7970/IP Communicator menu&#xD;&#xA;(png graphics)" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="647" y="361" />
    <node type="Comment" id="632890909264297927" text="Build no-graphic IP phone menu&#xD;&#xA;(7940/1 7960/1, 7920)" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="590" y="219" />
    <node type="Comment" id="632890909264297929" text="With the menu built, and &#xD;&#xA;sent to the phone, now create&#xD;&#xA;a second command for the phone,&#xD;&#xA;'RTPRx'" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1945" y="295" />
    <node type="Action" id="632891776627983164" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2056" y="398">
      <linkto id="632889791566825600" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">(txPort % 2 == 1 ? txPort - 1 : txPort)</ap>
        <rd field="ResultData">txPort</rd>
      </Properties>
    </node>
    <node type="Action" id="632896050178119776" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="604" y="150">
      <linkto id="632889956132002984" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(DataTable checkedLoggedIn, Hashtable positions, ref bool open, DataTable checkPref, ref string callName, ref string callNumber, ref string callId)
{
	// x1, y1, x2, y2

	// check preference for this device
	open =  !(checkPref == null || checkPref.Rows.Count == 0 || checkPref.Rows[0]["open"].ToString() == "0");

	// check if inventory helpdesk should be present with callName and callNumber
	if(checkedLoggedIn != null)
	{
		foreach(DataRow row in checkedLoggedIn.Rows)
		{
			if(row["checkedIn"].ToString() == "1")
			{
				callName = row["username"] as string;
				callNumber = row["extension"] as string;
				callId = row["userId"] as string;
				break;
			}
		}
	}
	

	positions["11111"] = new string[] {"7", "104", "206", "124"};
	positions["22222"] = new string[] {"7", "125", "206", "144"};
	positions["33333"] = new string[] {"7", "145", "206", "164"};

	return String.Empty;
}
</Properties>
    </node>
    <node type="Action" id="632896050178119798" name="If" container="632889956132002970" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1312" y="576">
      <linkto id="632889956132002971" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632889956132002972" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">(loopEnum.Current as DataRow)["checkedIn"].ToString() == "1"</ap>
      </Properties>
    </node>
    <node type="Action" id="632898037366592272" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="492.188721" y="151">
      <linkto id="632896050178119776" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">"SELECT open from preferences WHERE device = '" + g_device + "'"</ap>
        <ap name="Name" type="literal">RetailDemo</ap>
        <rd field="ResultSet">checkPref</rd>
      </Properties>
    </node>
    <node type="Action" id="632898037366592279" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="966" y="424">
      <linkto id="632898037366592282" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632898037366592284" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">callNumber != "NONE"</ap>
      </Properties>
    </node>
    <node type="Action" id="632898037366592282" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1038" y="552">
      <linkto id="632898037366592284" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">callName</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/Call?metreosSessionId=" + routingGuid + "&amp;ext=" + callNumber + "&amp;userId=" + callId + "&amp;name=" + callName.Replace(" ", "%20")</ap>
        <ap name="TouchAreaX1" type="literal">75</ap>
        <ap name="TouchAreaX2" type="literal">141</ap>
        <ap name="TouchAreaY1" type="literal">5</ap>
        <ap name="TouchAreaY2" type="literal">95</ap>
        <rd field="ResultData">graphicMenu</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"http://" + host + "/Retail/Call?metreosSessionId=" + routingGuid + "&amp;ext=" + callNumber + "&amp;userId=" + callId + "&amp;name=" + callName.Replace(" ", "%20")</log>
      </Properties>
    </node>
    <node type="Action" id="632898037366592284" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1123" y="424">
      <linkto id="632889956132002980" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Expand</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Retail/Menu?appId=" + routingGuid + "&amp;open=" + (open ? "0" : "1") + "&amp;device=" + g_device</ap>
        <ap name="TouchAreaX1" type="literal">148</ap>
        <ap name="TouchAreaX2" type="literal">213</ap>
        <ap name="TouchAreaY1" type="literal">5</ap>
        <ap name="TouchAreaY2" type="literal">95</ap>
        <rd field="ResultData">graphicMenu</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"http://" + host + "/Retail/Menu?appId=" + routingGuid + "&amp;open=" + (open ? "0" : "1") + "&amp;device=" + g_device</log>
      </Properties>
    </node>
    <node type="Comment" id="632898037366592286" text="SKU Lookup button" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="832" y="459" />
    <node type="Comment" id="632898037366592287" text="Call inv helpdesk group" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="981" y="589" />
    <node type="Comment" id="632898037366592289" text="Toggle helpdesk" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1071" y="382" />
    <node type="Variable" id="632875147412707555" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632875147412707556" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="632875304714864928" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632889791566825601" name="remoteIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteIpAddress" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteIP</Properties>
    </node>
    <node type="Variable" id="632889791566825612" name="txPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">txPort</Properties>
    </node>
    <node type="Variable" id="632889956132002670" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632889956132003010" name="checkedLoggedIn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkedLoggedIn</Properties>
    </node>
    <node type="Variable" id="632889956132003011" name="accept" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Accept" refType="reference">accept</Properties>
    </node>
    <node type="Variable" id="632889956132003329" name="graphicMenu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.GraphicFileMenu" refType="reference">graphicMenu</Properties>
    </node>
    <node type="Variable" id="632889956132003330" name="menu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">menu</Properties>
    </node>
    <node type="Variable" id="632889956132003331" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632889956132003332" name="hostname" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="hostname" refType="reference">hostname</Properties>
    </node>
    <node type="Variable" id="632889956132003333" name="count" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">count</Properties>
    </node>
    <node type="Variable" id="632889956132003683" name="modelNameHeader" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="x-CiscoIPPhoneModelName" defaultInitWith="NONE" refType="reference">modelNameHeader</Properties>
    </node>
    <node type="Variable" id="632891776627983165" name="rxPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">rxPort</Properties>
    </node>
    <node type="Variable" id="632896050178119775" name="positions" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Hashtable" refType="reference">positions</Properties>
    </node>
    <node type="Variable" id="632898037366592271" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632898037366592274" name="checkPref" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkPref</Properties>
    </node>
    <node type="Variable" id="632898037366592276" name="open" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">open</Properties>
    </node>
    <node type="Variable" id="632898037366592277" name="callName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="NONE" refType="reference">callName</Properties>
    </node>
    <node type="Variable" id="632898037366592278" name="callNumber" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="NONE" refType="reference">callNumber</Properties>
    </node>
    <node type="Variable" id="632898160147302413" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="NONE" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632875147412707594" treenode="632875147412707595" appnode="632875147412707592" handlerfor="632875147412707591">
    <node type="Start" id="632875147412707594" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="56" y="288">
      <linkto id="632875147412707615" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707615" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="288" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632890909264297931" text="Play just finished...do nothing." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="246" y="143" />
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632875147412707599" treenode="632875147412707600" appnode="632875147412707597" handlerfor="632875147412707596">
    <node type="Start" id="632875147412707599" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="56" y="288">
      <linkto id="632875147412707616" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707616" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="240" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632875147412707609" treenode="632875147412707610" appnode="632875147412707607" handlerfor="632875147412707606">
    <node type="Start" id="632875147412707609" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="296">
      <linkto id="632889791566825603" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632889791566825603" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="245" y="295">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632890909264297932" text="RemoteHangup will only occur if a true call was made.&#xD;&#xA;When the application initiating phone stops the &#xD;&#xA;RTPRx, no hangup message occurs. (Or any message for that matter, &#xD;&#xA;unless you are using JTAPI to monitor phone UI events, which this app doesn't)" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="83" y="182" />
  </canvas>
  <canvas type="Function" name="StoreInventory" startnode="632875147412707886" treenode="632875147412707887" appnode="632875147412707884" handlerfor="632875147412707883">
    <node type="Start" id="632875147412707886" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="72" y="288">
      <linkto id="632875291003497555" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875147412707983" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="832" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632875291003497552" name="CreateInput" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="464" y="288">
      <linkto id="632875291003497559" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Prompt" type="literal">Enter SKU</ap>
        <ap name="URL" type="csharp">"http://" + g_appServerIP + ":8000/Retail/StoreInventorySKU?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632875291003497555" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="200" y="288">
      <linkto id="632875291003497556" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="OperationId" type="variable">g_operationId</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Block" type="literal">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632875291003497556" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="288" y="272" mx="341" my="288">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632875147412707595" />
        <item text="OnPlay_Failed" treenode="632875147412707600" />
      </items>
      <linkto id="632875291003497552" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="literal">Enter a skew code</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="OperationId">g_operationId</rd>
      </Properties>
    </node>
    <node type="Action" id="632875291003497559" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="576" y="288">
      <linkto id="632875291003497560" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">SKU</ap>
        <ap name="QueryStringParam" type="literal">sku</ap>
        <ap name="InputFlags" type="literal">N</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632875291003497560" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="704" y="288">
      <linkto id="632875147412707983" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">input.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Comment" id="632890909264297930" text="Stop any existing play commands, play a prompt, and respond with a 'input sku' screen" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="224" y="200" />
    <node type="Variable" id="632875147412707913" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875147412707928" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632875291003497553" name="input" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Input" refType="reference">input</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="CheckIn" startnode="632875147412707906" treenode="632875147412707907" appnode="632875147412707904" handlerfor="632875147412707903">
    <node type="Start" id="632875147412707906" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="288">
      <linkto id="632875304714864905" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875304714864904" name="CreateInput" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="456" y="288">
      <linkto id="632875304714864907" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Prompt" type="literal">Check In</ap>
        <ap name="URL" type="csharp">"http://" + g_appServerIP + ":8000/Retail/CheckInSubmit?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632875304714864905" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="192" y="288">
      <linkto id="632875304714864906" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="OperationId" type="variable">g_operationId</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Block" type="literal">true</ap>
        <rd field="ResultCode">g_operationId</rd>
      </Properties>
    </node>
    <node type="Action" id="632875304714864906" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="280" y="272" mx="333" my="288">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632875147412707595" />
        <item text="OnPlay_Failed" treenode="632875147412707600" />
      </items>
      <linkto id="632875304714864904" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="literal">Checking in</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="OperationId">g_operationId</rd>
      </Properties>
    </node>
    <node type="Action" id="632875304714864907" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="568" y="288">
      <linkto id="632875304714864932" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">UserID</ap>
        <ap name="QueryStringParam" type="literal">userId</ap>
        <ap name="InputFlags" type="literal">N</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632875304714864908" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="760" y="288">
      <linkto id="632875304714864917" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">input.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875304714864917" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="872" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632875304714864932" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="656" y="288">
      <linkto id="632875304714864908" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">Ext</ap>
        <ap name="QueryStringParam" type="literal">ext</ap>
        <ap name="InputFlags" type="literal">N</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Comment" id="632890909264297933" text="Play a simple prompt, and  display 'input userID and extension' screen" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="304" y="179" />
    <node type="Variable" id="632875147412707921" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875147412707956" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632875304714864916" name="input" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Input" refType="reference">input</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="CheckOut" startnode="632875147412707911" treenode="632875147412707912" appnode="632875147412707909" handlerfor="632875147412707908">
    <node type="Start" id="632875147412707911" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="288">
      <linkto id="632875376037936229" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875376037936228" name="CreateInput" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="431" y="288">
      <linkto id="632875376037936231" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Prompt" type="literal">Check Out</ap>
        <ap name="URL" type="csharp">"http://" + g_appServerIP + ":8000/Retail/CheckOutSubmit?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936229" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="184" y="288">
      <linkto id="632875376037936230" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="OperationId" type="variable">g_operationId</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Block" type="literal">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936230" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="272" y="272" mx="325" my="288">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632875147412707595" />
        <item text="OnPlay_Failed" treenode="632875147412707600" />
      </items>
      <linkto id="632875376037936228" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Prompt1" type="literal">Checking out</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="OperationId">g_operationId</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936231" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="544" y="288">
      <linkto id="632875376037936232" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">UserID</ap>
        <ap name="QueryStringParam" type="literal">userId</ap>
        <ap name="InputFlags" type="literal">N</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936232" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="664" y="288">
      <linkto id="632875376037936233" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">input.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936233" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="776" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632890909264297935" text="Play a simple prompt, and  display 'input userID' screen" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="249" y="232" />
    <node type="Variable" id="632875147412707923" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875147412707963" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632875376037936244" name="input" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Input" refType="reference">input</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="StoreInventorySKU" startnode="632875291003497564" treenode="632875291003497565" appnode="632875291003497562" handlerfor="632875291003497561">
    <node type="Start" id="632875291003497564" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34.166687" y="272.625">
      <linkto id="632896050178121837" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632883382612676454" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="807" y="271">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632883742306501471" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="647" y="273">
      <linkto id="632883382612676454" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">graphicMenu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632883742306501473" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="481" y="272">
      <linkto id="632883742306501471" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Menu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">"http://" + g_appServerIP + ":8000/Retail/Menu?appId=" + routingGuid + "&amp;device=" + g_device</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632896050178121837" name="CreateGraphicFileMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="208" y="272">
      <linkto id="632883742306501473" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Prompt" type="literal">Make a selection</ap>
        <ap name="LocationX" type="literal">-1</ap>
        <ap name="LocationY" type="literal">-1</ap>
        <ap name="URL" type="csharp">"http://" + hostname + "/mceadmin/" + g_skuLookupPath</ap>
        <rd field="ResultData">graphicMenu</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"http://" + hostname + "/mceadmin/" + g_skuLookupPath</log>
      </Properties>
    </node>
    <node type="Comment" id="632907454652098369" text="&quot;http://&quot; + g_appServerIP + &quot;:8000/Retail/Call?ext=&quot; + dial + &quot;&amp;metreosSessionId=&quot; + routingGuid + &quot;&amp;sku=&quot; + query[&quot;sku&quot;]" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="49" y="842" />
    <node type="Variable" id="632875291003497566" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632875291003497567" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632883742306501464" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632884701052635514" name="dial" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dial</Properties>
    </node>
    <node type="Variable" id="632896050178121836" name="graphicMenu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.GraphicFileMenu" refType="reference">graphicMenu</Properties>
    </node>
    <node type="Variable" id="632896050178123316" name="hostname" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="hostname" refType="reference">hostname</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="CheckInSubmit" startnode="632875304714864921" treenode="632875304714864922" appnode="632875304714864919" handlerfor="632875304714864918">
    <node type="Start" id="632875304714864921" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="56" y="288">
      <linkto id="632875304714864937" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632875304714864924" text="Find user in user database" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="152" y="152" />
    <node type="Action" id="632875304714864930" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="303" y="288">
      <linkto id="632875304714864935" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632875304714864938" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">"SELECT username FROM Users WHERE UserID = '" + query["userId"] + "'" </ap>
        <ap name="Name" type="literal">RetailDemo</ap>
        <rd field="ResultSet">checkUserResults</rd>
      </Properties>
    </node>
    <node type="Action" id="632875304714864935" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="472" y="288">
      <linkto id="632875304714864936" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632875304714864938" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">checkUserResults.Rows.Count != 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632875304714864936" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="608" y="288">
      <linkto id="632875376037936221" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Text" type="csharp">"Logging in at " + query["ext"]</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875304714864937" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="168" y="288">
      <linkto id="632875304714864938" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632875304714864930" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">query["ext"] == String.Empty || query["userId"] == String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632875304714864938" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="136" y="392" mx="173" my="408">
      <items count="1">
        <item text="CheckIn" treenode="632875147412707907" />
      </items>
      <linkto id="632875304714864940" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="RoutingGuid" type="variable">routingGuid</ap>
        <ap name="FunctionName" type="literal">CheckIn</ap>
      </Properties>
    </node>
    <node type="Action" id="632875304714864940" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="168" y="544">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632875304714864941" text="If user submits empty parameters,&#xD;&#xA;the check user query fails, or&#xD;&#xA;if there is no userId of the entered ID,&#xD;&#xA;re-prompt with CheckIn" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="209" y="410" />
    <node type="Action" id="632875376037936221" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="752" y="288">
      <linkto id="632875376037936222" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936222" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="848" y="288">
      <linkto id="632875376037936227" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="literal">1000</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936223" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1224" y="288">
      <linkto id="632875376037936224" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"http://" + host + "/Retail/Menu?appId=" + routingGuid + "&amp;device=" + g_device</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936224" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1344" y="288">
      <linkto id="632875376037936226" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">g_phoneIP</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936226" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1448" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632875376037936227" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="976" y="288">
      <linkto id="632875376037936540" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">"UPDATE Users SET extension = '" + query["ext"] + "', checkedIn = 1 WHERE userId = '" + query["userId"] + "'"</ap>
        <ap name="Name" type="literal">RetailDemo</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936540" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1072" y="272" mx="1112" my="288">
      <items count="1">
        <item text="UpdateImage" />
      </items>
      <linkto id="632875376037936223" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="x-CiscoIPPhoneModelName" type="variable">modelHeaderName</ap>
        <ap name="FunctionName" type="literal">UpdateImage</ap>
      </Properties>
    </node>
    <node type="Variable" id="632875304714864923" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875304714864931" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">query</Properties>
    </node>
    <node type="Variable" id="632875304714864934" name="checkUserResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkUserResults</Properties>
    </node>
    <node type="Variable" id="632875304714864939" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632875376037936220" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632875376037936225" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="632891776627983163" name="modelHeaderName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="x-CiscoIPPhoneModelName" defaultInitWith="NONE" refType="reference">modelHeaderName</Properties>
    </node>
    <node type="Variable" id="632898160147303809" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="CheckOutSubmit" startnode="632875376037936248" treenode="632875376037936249" appnode="632875376037936246" handlerfor="632875376037936245">
    <node type="Start" id="632875376037936248" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="296">
      <linkto id="632875376037936253" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875376037936250" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="296" y="296">
      <linkto id="632875376037936251" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632875376037936254" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">"SELECT username FROM Users WHERE UserID = '" + query["userId"] + "'" </ap>
        <ap name="Name" type="literal">RetailDemo</ap>
        <rd field="ResultSet">checkUserResults</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936251" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="464" y="296">
      <linkto id="632875376037936252" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632875376037936254" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">checkUserResults.Rows.Count != 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936252" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="600" y="296">
      <linkto id="632875376037936257" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Text" type="csharp">"Logging out " + checkUserResults.Rows[0][0] </ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936253" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="160" y="296">
      <linkto id="632875376037936254" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632875376037936250" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">query["userId"] == String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936254" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="128" y="400" mx="165" my="416">
      <items count="1">
        <item text="CheckOut" treenode="632875147412707912" />
      </items>
      <linkto id="632875376037936255" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="RoutingGuid" type="variable">routingGuid</ap>
        <ap name="FunctionName" type="literal">CheckOut</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936255" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="160" y="552">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632875376037936256" text="If user submits empty parameters,&#xD;&#xA;the check user query fails, or&#xD;&#xA;if there is no userId of the entered ID,&#xD;&#xA;re-prompt with CheckIn" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="208" y="416" />
    <node type="Action" id="632875376037936257" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="744" y="296">
      <linkto id="632875376037936258" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936258" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="840" y="296">
      <linkto id="632875376037936262" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="literal">1000</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936259" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1168" y="296">
      <linkto id="632875376037936260" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"http://" + host + "/Retail/Menu?appId=" + routingGuid + "&amp;device=" + g_device</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037936260" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1288" y="296">
      <linkto id="632875376037936261" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">g_phoneIP</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936261" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1392" y="296">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632875376037936262" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="952" y="296">
      <linkto id="632875376037936565" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">"UPDATE Users SET checkedIn = 0 WHERE userId = '" + query["userId"] + "'"</ap>
        <ap name="Name" type="literal">RetailDemo</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037936565" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1032" y="280" mx="1072" my="296">
      <items count="1">
        <item text="UpdateImage" />
      </items>
      <linkto id="632875376037936259" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="x-CiscoIPPhoneModelName" type="variable">modelHeaderName</ap>
        <ap name="FunctionName" type="literal">UpdateImage</ap>
      </Properties>
    </node>
    <node type="Variable" id="632875376037936276" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875376037936277" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632875376037936278" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632875376037936279" name="checkUserResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkUserResults</Properties>
    </node>
    <node type="Variable" id="632875376037936280" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="632875376037936281" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632891776627983162" name="modelHeaderName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="x-CiscoIPPhoneModelName" defaultInitWith="NONE" refType="reference">modelHeaderName</Properties>
    </node>
    <node type="Variable" id="632898160147303810" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Call" startnode="632875376037937409" treenode="632875376037937410" appnode="632875376037937407" handlerfor="632875376037937406">
    <node type="Start" id="632875376037937409" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="96" y="296">
      <linkto id="632875376037937736" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875376037937416" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="427" y="298">
      <linkto id="632875376037937417" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Text" type="csharp">"Calling " + query["ext"]</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037937417" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="544" y="297">
      <linkto id="632875376037937418" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037937418" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="694" y="298">
      <linkto id="632875376037937419" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SleepTime" type="literal">1000</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037937419" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="838" y="298">
      <linkto id="632875376037937422" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">query["sku"] != null ? "http://" + host + "/Retail/StoreInventorySKU?metreosSessionId=" + routingGuid + "&amp;noplay=1&amp;sku=" + query["sku"] : "http://" + host + "/Retail/Menu?appId=" + routingGuid + "&amp;device=" + g_device</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="632875376037937422" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="958" y="298">
      <linkto id="632875376037937740" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">g_phoneIP</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037937736" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="168" y="280" mx="234" my="296">
      <items count="2">
        <item text="OnMakeCall_Complete" treenode="632875376037937730" />
        <item text="OnMakeCall_Failed" treenode="632875376037937735" />
      </items>
      <linkto id="632898037366592291" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="csharp">query["ext"]</ap>
        <ap name="From" type="literal">Retail Portal</ap>
        <ap name="DisplayName" type="literal">Retail Portal</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632875376037937740" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1102" y="298">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632898037366592291" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="339" y="297">
      <linkto id="632875376037937416" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632898037366592292" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">query["name"] == null || query["name"] == String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632898037366592292" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="422" y="431">
      <linkto id="632875376037937417" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Text" type="csharp">"Calling " + query["name"] + " at " + query["ext"]</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Variable" id="632875376037937411" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632875376037937412" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632875376037937414" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632875376037937415" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="632875376037937725" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632898160147303811" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632875376037937729" treenode="632875376037937730" appnode="632875376037937727" handlerfor="632875376037937726">
    <node type="Start" id="632875376037937729" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="88" y="344">
      <linkto id="632875376037937741" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875376037937741" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="328" y="344">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632875376037937734" treenode="632875376037937735" appnode="632875376037937732" handlerfor="632875376037937731">
    <node type="Start" id="632875376037937734" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="72" y="360">
      <linkto id="632875376037937742" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632875376037937742" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="216" y="360">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="632889791566825607" treenode="632889791566825608" appnode="632889791566825605" handlerfor="632889791566825604">
    <node type="Start" id="632889791566825607" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="143" y="290">
      <linkto id="632889791566825610" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632889791566825609" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="496" y="289">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632889791566825610" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="362" y="290">
      <linkto id="632889791566825609" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
      </Properties>
    </node>
    <node type="Comment" id="632889791566825611" text="HTTP session has expired, clean up the all-important media resources" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="196" y="196" />
  </canvas>
  <canvas type="Function" name="UpdateStatus" startnode="632896050178119780" treenode="632896050178119781" appnode="632896050178119778" handlerfor="632896050178119777">
    <node type="Loop" id="632896050178119791" name="Loop" text="loop (expr)" cx="306" cy="273" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="528" y="200" mx="681" my="336">
      <linkto id="632896050178119792" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632896050178119794" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">getAllUsers.Rows</Properties>
    </node>
    <node type="Start" id="632896050178119780" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="336">
      <linkto id="632896050178119788" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632896050178119784" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="360" y="336">
      <linkto id="632896050178119791" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Retail Portal</ap>
        <ap name="Prompt" type="literal">Update Status</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632896050178119788" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="216" y="336">
      <linkto id="632896050178119784" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="literal">SELECT username, extension, userId, checkedIn from users</ap>
        <ap name="Name" type="literal">RetailDemo</ap>
        <rd field="ResultSet">getAllUsers</rd>
      </Properties>
    </node>
    <node type="Action" id="632896050178119792" name="AddMenuItem" container="632896050178119791" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="640" y="336">
      <linkto id="632896050178119797" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">(loopEnum.Current as DataRow)["username"] + " - " + (((loopEnum.Current as DataRow)["checkedIn"].ToString()) == "1" ? "Available" : "Unavailable")</ap>
        <ap name="URL" type="csharp">(loopEnum.Current as DataRow)["checkedIn"].ToString() == "1" ? "http://" + host + "/Retail/CheckOut?metreosSessionId=" + routingGuid : "http://" + host + "/Retail/CheckIn?metreosSessionId=" + routingGuid </ap>
        <rd field="ResultData">menu</rd>
        <log condition="entry" on="true" level="Info" type="csharp">(loopEnum.Current as DataRow)["username"] + " (" + ((loopEnum.Current as DataRow)["checkedIn"].ToString()) == "1" ? "In)" : "Out)"</log>
      </Properties>
    </node>
    <node type="Action" id="632896050178119794" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="952" y="336">
      <linkto id="632896050178119795" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632896050178119795" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1072" y="336">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632896050178119797" name="Assign" container="632896050178119791" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="736" y="336">
      <linkto id="632896050178119791" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">count + 1</ap>
        <rd field="ResultData">count</rd>
      </Properties>
    </node>
    <node type="Variable" id="632896050178119782" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632896050178119783" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632896050178119785" name="menu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">menu</Properties>
    </node>
    <node type="Variable" id="632896050178119790" name="getAllUsers" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">getAllUsers</Properties>
    </node>
    <node type="Variable" id="632896050178119793" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632896050178119796" name="count" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" defaultInitWith="0" refType="reference">count</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="UpdateImage" startnode="632875376037936543" treenode="632875376037936544" appnode="632875376037936541" handlerfor="632896050178119777">
    <node type="Loop" id="632898160147301903" name="Loop" text="loop (expr)" cx="545" cy="191" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="1120.01367" y="32" mx="1393" my="128">
      <linkto id="632898160147301907" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632898160147301925" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">checkLoggedIn.Rows</Properties>
    </node>
    <node type="Loop" id="632898160147301908" name="Loop" text="loop (expr)" cx="555" cy="218" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="1142.01367" y="710" mx="1420" my="819">
      <linkto id="632898160147301909" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632898160147301917" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">1</Properties>
    </node>
    <node type="Start" id="632875376037936543" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="410">
      <linkto id="632898160147301913" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632898160147301904" name="AddStandardImageRegion" container="632898160147301903" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="1396.01367" y="60">
      <linkto id="632898160147301905" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="variable">g_greenIconPath</ap>
        <ap name="Top" type="csharp">(positions[count] as string[])[1]</ap>
        <ap name="Left" type="csharp">(positions[count] as string[])[0]</ap>
        <ap name="Width" type="literal">12</ap>
        <ap name="Height" type="literal">11</ap>
        <rd field="Image">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632898160147301905" name="Assign" container="632898160147301903" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1592.01367" y="124">
      <linkto id="632898160147301903" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">count + 1</ap>
        <rd field="ResultData">count</rd>
      </Properties>
    </node>
    <node type="Action" id="632898160147301906" name="AddStandardImageRegion" container="632898160147301903" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="1400.01367" y="165">
      <linkto id="632898160147301905" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="variable">g_redIconPath</ap>
        <ap name="Top" type="csharp">(positions[count] as string[])[1]</ap>
        <ap name="Left" type="csharp">(positions[count] as string[])[0]</ap>
        <ap name="Width" type="literal">12</ap>
        <ap name="Height" type="literal">11</ap>
        <rd field="Image">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632898160147301907" name="If" container="632898160147301903" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1190.01367" y="123">
      <linkto id="632898160147301904" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632898160147301906" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">((loopEnum.Current as DataRow)["checkedIn"].ToString()) == "1"</ap>
      </Properties>
    </node>
    <node type="Action" id="632898160147301909" name="AddStandardImageRegion" container="632898160147301908" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="1254.01367" y="830">
      <linkto id="632898160147301910" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="literal">c:\images\button2.png</ap>
        <ap name="Top" type="literal">108</ap>
        <ap name="Left" type="csharp">1 + count * 33 + count * 4</ap>
        <ap name="Width" type="literal">33</ap>
        <ap name="Height" type="literal">70</ap>
        <rd field="Image">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632898160147301910" name="AddTextRegion" container="632898160147301908" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="1398.01367" y="830">
      <linkto id="632898160147301911" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Text" type="csharp">((loopEnum.Current as DataRow)["username"] as string).Split()[0]</ap>
        <ap name="Font" type="literal">Arial</ap>
        <ap name="FontSize" type="literal">8</ap>
        <ap name="Top" type="csharp">108 + 20</ap>
        <ap name="Left" type="csharp">1 + count * 33 + count * 4 + 2</ap>
        <ap name="Color" type="csharp">System.Drawing.Color.Black</ap>
        <rd field="Image">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632898160147301911" name="AddTextRegion" container="632898160147301908" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="1510.01367" y="830">
      <linkto id="632898160147301912" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Text" type="csharp">((loopEnum.Current as DataRow)["username"] as string).Split()[1]</ap>
        <ap name="Font" type="literal">Arial</ap>
        <ap name="FontSize" type="literal">8</ap>
        <ap name="Top" type="csharp">108 + 40</ap>
        <ap name="Left" type="csharp">1 + count * 33 + count * 4 + 2</ap>
        <ap name="Color" type="csharp">System.Drawing.Color.Black</ap>
        <rd field="Image">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632898160147301912" name="Assign" container="632898160147301908" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1622.01367" y="830">
      <linkto id="632898160147301908" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">count + 1</ap>
        <rd field="ResultData">count</rd>
      </Properties>
    </node>
    <node type="Action" id="632898160147301913" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="159.013672" y="411">
      <linkto id="632898160147301924" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="literal">SELECT username, extension, userId, checkedIn from users</ap>
        <ap name="Name" type="literal">RetailDemo</ap>
        <rd field="ResultSet">checkLoggedIn</rd>
      </Properties>
    </node>
    <node type="Action" id="632898160147301915" name="CreateImageBuilder" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="608.0137" y="152">
      <linkto id="632898160147301916" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <rd field="ImageBuilder">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632898160147301916" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="786.0137" y="152">
      <linkto id="632898160147301923" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="variable">g_mainMenuPath</ap>
        <ap name="Top" type="literal">0</ap>
        <ap name="Left" type="literal">0</ap>
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <rd field="Image">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632898160147301917" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1885.01367" y="302">
      <linkto id="632898160147301918" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
          public static string Execute(Metreos.Types.Imaging.ImageBuilder image, Metreos.Types.Imaging.ImageBuilder imageClosed, string g_mceadminInstallPath)
          {

          image.Save(g_mceadminInstallPath + "\\public\\menu.png");
          imageClosed.Save(g_mceadminInstallPath + "\\public\\menuClosed.png");

          return "";
          }
      </Properties>
    </node>
    <node type="Action" id="632898160147301918" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1989.01367" y="302">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632898160147301919" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="540.0137" y="408">
      <linkto id="632898160147301915" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632898160147301921" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">modelNameHeader == "CP-7921G"</ap>
      </Properties>
    </node>
    <node type="Action" id="632898160147301920" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="932.0137" y="528">
      <linkto id="632898160147301908" port="1" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632898160147301917" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">checkLoggedIn.Rows.Count &gt; 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632898160147301921" name="CreateImageBuilder" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="604.0137" y="528">
      <linkto id="632898160147301922" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Width" type="literal">140</ap>
        <ap name="Height" type="literal">176</ap>
        <rd field="ImageBuilder">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632898160147301922" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="772.0137" y="528">
      <linkto id="632898160147301920" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="literal">c:\images\background_7921.png</ap>
        <ap name="Top" type="literal">0</ap>
        <ap name="Left" type="literal">0</ap>
        <ap name="Width" type="literal">140</ap>
        <ap name="Height" type="literal">176</ap>
        <rd field="Image">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632898160147301923" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="940.0137" y="151">
      <linkto id="632898160147301903" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ArrayList positions)
{

	positions.Add(new string[] {"15", "109"});
	positions.Add(new string[] {"15", "129"});
	positions.Add(new string[] {"15", "149"});

	return String.Empty;
}
</Properties>
    </node>
    <node type="Action" id="632898160147301924" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="272.013672" y="411">
      <linkto id="632898160147301926" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">"SELECT open FROM preferences WHERE device = '" + g_device + "'"</ap>
        <ap name="Name" type="literal">RetailDemo</ap>
        <rd field="ResultSet">checkPref</rd>
      </Properties>
    </node>
    <node type="Action" id="632898160147301925" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="1860.01367" y="129">
      <linkto id="632898555827032458" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="csharp">anyAvailable ? g_greenIconPath : g_redIconPath</ap>
        <ap name="Top" type="csharp">78</ap>
        <ap name="Left" type="csharp">82</ap>
        <ap name="Width" type="literal">12</ap>
        <ap name="Height" type="literal">11</ap>
        <rd field="Image">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632898160147301926" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="403" y="411">
      <linkto id="632898160147301919" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(DataTable checkLoggedIn, DataTable checkPref, ref bool anyAvailable, ref bool open)
{
	anyAvailable = false;

	if(checkLoggedIn != null)
	{
		foreach(DataRow row in checkLoggedIn.Rows)
		{
			anyAvailable = row["checkedIn"].ToString() == "1";
			if(anyAvailable)
			{
				break;
			}
		}
	}

	open = false;
	if(checkPref != null &amp;&amp; checkPref.Rows.Count &gt; 0)
	{
		open = checkPref.Rows[0]["open"] == "1";
	}

	return String.Empty;
}
</Properties>
    </node>
    <node type="Action" id="632898555827032457" name="CreateImageBuilder" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="1211" y="302">
      <linkto id="632898555827032460" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <rd field="ImageBuilder">imageClosed</rd>
      </Properties>
    </node>
    <node type="Label" id="632898555827032458" text="c" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1979" y="126" />
    <node type="Label" id="632898555827032459" text="c" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1122" y="302">
      <linkto id="632898555827032457" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632898555827032460" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="1411.01367" y="302">
      <linkto id="632898555827032462" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="variable">g_mainMenuClosedPath</ap>
        <ap name="Top" type="literal">0</ap>
        <ap name="Left" type="literal">0</ap>
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <rd field="Image">imageClosed</rd>
      </Properties>
    </node>
    <node type="Action" id="632898555827032462" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="1598.01367" y="302">
      <linkto id="632898160147301917" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="csharp">anyAvailable ? g_greenIconPath : g_redIconPath</ap>
        <ap name="Top" type="csharp">78</ap>
        <ap name="Left" type="csharp">82</ap>
        <ap name="Width" type="literal">12</ap>
        <ap name="Height" type="literal">11</ap>
        <rd field="Image">imageClosed</rd>
      </Properties>
    </node>
    <node type="Variable" id="632875376037936546" name="checkLoggedIn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkLoggedIn</Properties>
    </node>
    <node type="Variable" id="632875376037936549" name="image" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Imaging.ImageBuilder" refType="reference">image</Properties>
    </node>
    <node type="Variable" id="632875376037936557" name="count" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" defaultInitWith="0" refType="reference">count</Properties>
    </node>
    <node type="Variable" id="632889956132004051" name="modelNameHeader" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="x-CiscoIPPhoneModelName" defaultInitWith="NONE" refType="reference">modelNameHeader</Properties>
    </node>
    <node type="Variable" id="632896050178119028" name="positions" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="ArrayList" refType="reference">positions</Properties>
    </node>
    <node type="Variable" id="632898160147302409" name="checkPref" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkPref</Properties>
    </node>
    <node type="Variable" id="632898160147302410" name="anyAvailable" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" defaultInitWith="false" refType="reference">anyAvailable</Properties>
    </node>
    <node type="Variable" id="632898160147306686" name="open" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" defaultInitWith="false" refType="reference">open</Properties>
    </node>
    <node type="Variable" id="632898555827032456" name="imageClosed" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Imaging.ImageBuilder" refType="reference">imageClosed</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="CheckFileExists" startnode="632889956132003336" treenode="632889956132003337" appnode="632889956132003334" handlerfor="632896050178119777">
    <node type="Start" id="632889956132003336" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="298">
      <linkto id="632889956132003667" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632889956132003664" name="CreateImageBuilder" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="277.90625" y="212">
      <linkto id="632889956132003665" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <ap name="BackgroundColor" type="csharp">System.Drawing.Color.Black</ap>
        <rd field="ImageBuilder">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632889956132003665" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="430.90625" y="212">
      <linkto id="632889956132003666" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="variable">g_mainMenuPath</ap>
        <ap name="Top" type="literal">0</ap>
        <ap name="Left" type="literal">0</ap>
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <rd field="Image">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632889956132003666" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="559.90625" y="212">
      <linkto id="632898555827033306" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
          public static string Execute(Metreos.Types.Imaging.ImageBuilder image, string g_mceadminInstallPath)
          {

          image.Save(g_mceadminInstallPath + "\\public\\menu.png");

          return String.Empty;
          }
      </Properties>
    </node>
    <node type="Action" id="632889956132003667" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="158.90625" y="300">
      <linkto id="632889956132003664" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632898555827033306" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
          public static string Execute(string g_mceadminInstallPath)
          {
          return System.IO.File.Exists(g_mceadminInstallPath + "\\public\\menu.png").ToString();
          }
      </Properties>
    </node>
    <node type="Comment" id="632889956132003668" text="Check if the basic background menu exists..." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="262.90625" y="338" />
    <node type="Action" id="632889956132003669" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1190.06836" y="301">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632898555827032464" name="CreateImageBuilder" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="742" y="212">
      <linkto id="632898555827032467" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <ap name="BackgroundColor" type="csharp">System.Drawing.Color.Black</ap>
        <rd field="ImageBuilder">imageClosed</rd>
      </Properties>
    </node>
    <node type="Action" id="632898555827032467" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="899.90625" y="213">
      <linkto id="632898555827032925" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="variable">g_mainMenuClosedPath</ap>
        <ap name="Top" type="literal">0</ap>
        <ap name="Left" type="literal">0</ap>
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <rd field="Image">imageClosed</rd>
      </Properties>
    </node>
    <node type="Action" id="632898555827032925" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1052.90625" y="213">
      <linkto id="632889956132003669" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
          public static string Execute(Metreos.Types.Imaging.ImageBuilder imageClosed, string g_mceadminInstallPath)
          {

          imageClosed.Save(g_mceadminInstallPath + "\\public\\menuClosed.png");

          return "";
          }
      </Properties>
    </node>
    <node type="Action" id="632898555827033306" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="650" y="298">
      <linkto id="632898555827032464" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632889956132003669" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
          public static string Execute(string g_mceadminInstallPath)
          {
          return System.IO.File.Exists(g_mceadminInstallPath + "\\public\\menuClosed.png").ToString();
          }
      </Properties>
    </node>
    <node type="Variable" id="632889956132003676" name="image" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Imaging.ImageBuilder" refType="reference">image</Properties>
    </node>
    <node type="Variable" id="632898555827032466" name="imageClosed" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Imaging.ImageBuilder" refType="reference">imageClosed</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>