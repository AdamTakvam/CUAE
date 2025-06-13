<Application name="InboundCall" trigger="Metreos.Providers.JTapi.JTapiIncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="InboundCall">
    <outline>
      <treenode type="evh" id="632983074993061880" level="1" text="Metreos.Providers.JTapi.JTapiIncomingCall (trigger): OnJTapiIncomingCall">
        <node type="function" name="OnJTapiIncomingCall" id="632983074993061877" path="Metreos.StockTools" />
        <node type="event" name="JTapiIncomingCall" id="632983074993061876" path="Metreos.Providers.JTapi.JTapiIncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632983074993061890" level="2" text="Metreos.Providers.JTapi.JTapiCallActive: OnJTapiCallActive">
        <node type="function" name="OnJTapiCallActive" id="632983074993061887" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallActive" id="632983074993061886" path="Metreos.Providers.JTapi.JTapiCallActive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632983074993061895" level="2" text="Metreos.Providers.JTapi.JTapiCallInactive: OnJTapiCallInactive">
        <node type="function" name="OnJTapiCallInactive" id="632983074993061892" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallInactive" id="632983074993061891" path="Metreos.Providers.JTapi.JTapiCallInactive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632983074993061906" level="2" text="Metreos.Providers.JTapi.JTapiHangup: OnJTapiHangup">
        <node type="function" name="OnJTapiHangup" id="632983074993061903" path="Metreos.StockTools" />
        <node type="event" name="JTapiHangup" id="632983074993061902" path="Metreos.Providers.JTapi.JTapiHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632989423340680752" level="2" text="Metreos.Providers.SalesforceDemo.HangupRequest: OnHangupRequest">
        <node type="function" name="OnHangupRequest" id="632989423340680749" path="Metreos.StockTools" />
        <node type="event" name="HangupRequest" id="632989423340680748" path="Metreos.Providers.SalesforceDemo.HangupRequest" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="633074064873750549" level="2" text="Metreos.Providers.SalesforceDemo.ConferenceRequest: OnConferenceRequest">
        <node type="function" name="OnConferenceRequest" id="633074064873750546" path="Metreos.StockTools" />
        <node type="event" name="ConferenceRequest" id="633074064873750545" path="Metreos.Providers.SalesforceDemo.ConferenceRequest" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="633074067383281821" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="633074067383281818" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="633074067383281817" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="633086135845414294" actid="633074067383281827" />
          <ref id="633086135845414466" actid="633085462900292230" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633074067383281826" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="633074067383281823" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="633074067383281822" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="633086135845414295" actid="633074067383281827" />
          <ref id="633086135845414467" actid="633085462900292230" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633082867945088530" level="2" text="Metreos.Providers.Http.GotRequest: ShowMap">
        <node type="function" name="ShowMap" id="633082867945088527" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633082867945088526" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/SalesforceDemo/ShowMap</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="633083736164897465" level="2" text="Metreos.Providers.zshakeel.ShowMap_Complete: OnShowMap_Complete">
        <node type="function" name="OnShowMap_Complete" id="633083736164897462" path="Metreos.StockTools" />
        <node type="event" name="ShowMap_Complete" id="633083736164897461" path="Metreos.Providers.zshakeel.ShowMap_Complete" />
        <references>
          <ref id="633086135845414338" actid="633083736164897471" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633083736164897470" level="2" text="Metreos.Providers.zshakeel.ShowMap_Failed: OnShowMap_Failed">
        <node type="function" name="OnShowMap_Failed" id="633083736164897467" path="Metreos.StockTools" />
        <node type="event" name="ShowMap_Failed" id="633083736164897466" path="Metreos.Providers.zshakeel.ShowMap_Failed" />
        <references>
          <ref id="633086135845414339" actid="633083736164897471" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633083925444684566" level="2" text="Metreos.Providers.Http.GotRequest: MoveMap">
        <node type="function" name="MoveMap" id="633083925444684563" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633083925444684562" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/SalesforceDemo/MoveMap</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="633083925444684575" level="2" text="Metreos.Providers.zshakeel.MoveMap_Complete: OnMoveMap_Complete">
        <node type="function" name="OnMoveMap_Complete" id="633083925444684572" path="Metreos.StockTools" />
        <node type="event" name="MoveMap_Complete" id="633083925444684571" path="Metreos.Providers.zshakeel.MoveMap_Complete" />
        <references>
          <ref id="633086135845414392" actid="633083925444684581" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633083925444684580" level="2" text="Metreos.Providers.zshakeel.MoveMap_Failed: OnMoveMap_Failed">
        <node type="function" name="OnMoveMap_Failed" id="633083925444684577" path="Metreos.StockTools" />
        <node type="event" name="MoveMap_Failed" id="633083925444684576" path="Metreos.Providers.zshakeel.MoveMap_Failed" />
        <references>
          <ref id="633086135845414393" actid="633083925444684581" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633084397433931078" level="2" text="Metreos.Providers.zshakeel.ShowReseller_Complete: OnShowReseller_Complete">
        <node type="function" name="OnShowReseller_Complete" id="633084397433931075" path="Metreos.StockTools" />
        <node type="event" name="ShowReseller_Complete" id="633084397433931074" path="Metreos.Providers.zshakeel.ShowReseller_Complete" />
        <references>
          <ref id="633086135845414240" actid="633084397433931084" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633084397433931083" level="2" text="Metreos.Providers.zshakeel.ShowReseller_Failed: OnShowReseller_Failed">
        <node type="function" name="OnShowReseller_Failed" id="633084397433931080" path="Metreos.StockTools" />
        <node type="event" name="ShowReseller_Failed" id="633084397433931079" path="Metreos.Providers.zshakeel.ShowReseller_Failed" />
        <references>
          <ref id="633086135845414241" actid="633084397433931084" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633084517770577907" level="2" text="Metreos.Providers.Http.GotRequest: ResellerSelected">
        <node type="function" name="ResellerSelected" id="633084517770577904" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633084517770577903" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/SalesforceDemo/ResellerSelected</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="633086135845414142" vid="632983417216714903">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="hashtable" id="633086135845414144" vid="632985069719295903">
        <Properties type="Hashtable">hashtable</Properties>
      </treenode>
      <treenode text="g_accountCode" id="633086135845414146" vid="632985069719295918">
        <Properties type="String">g_accountCode</Properties>
      </treenode>
      <treenode text="g_from" id="633086135845414148" vid="633004250508108165">
        <Properties type="String">g_from</Properties>
      </treenode>
      <treenode text="g_recId" id="633086135845414150" vid="633004250508108572">
        <Properties type="Int" defaultInitWith="-1">g_recId</Properties>
      </treenode>
      <treenode text="g_to" id="633086135845414152" vid="633074064873750776">
        <Properties type="String">g_to</Properties>
      </treenode>
      <treenode text="g_bargeConnId" id="633086135845414154" vid="633074064873750779">
        <Properties type="String">g_bargeConnId</Properties>
      </treenode>
      <treenode text="g_bargeCallId" id="633086135845414156" vid="633074064873750781">
        <Properties type="String">g_bargeCallId</Properties>
      </treenode>
      <treenode text="g_deviceName" id="633086135845414158" vid="633074067383281811">
        <Properties type="String">g_deviceName</Properties>
      </treenode>
      <treenode text="g_bargeConfId" id="633086135845414160" vid="633074067383281813">
        <Properties type="String" defaultInitWith="0">g_bargeConfId</Properties>
      </treenode>
      <treenode text="g_barged" id="633086135845414162" vid="633074067383281845">
        <Properties type="Bool" defaultInitWith="false">g_barged</Properties>
      </treenode>
      <treenode text="g_useBarge" id="633086135845414164" vid="633074086161877495">
        <Properties type="Bool" initWith="UseBargeForConference">g_useBarge</Properties>
      </treenode>
      <treenode text="g_phoneUser" id="633086135845414166" vid="633074086161878303">
        <Properties type="String" initWith="PhoneUsername">g_phoneUser</Properties>
      </treenode>
      <treenode text="g_phonePass" id="633086135845414168" vid="633074086161878305">
        <Properties type="String" initWith="PhonePassword">g_phonePass</Properties>
      </treenode>
      <treenode text="g_phoneIp" id="633086135845414170" vid="633082851502061630">
        <Properties type="String">g_phoneIp</Properties>
      </treenode>
      <treenode text="g_AppServerIP" id="633086135845414172" vid="633082867945088326">
        <Properties type="String" initWith="AppServerIP">g_AppServerIP</Properties>
      </treenode>
      <treenode text="g_street" id="633086135845414174" vid="633083736164897474">
        <Properties type="String">g_street</Properties>
      </treenode>
      <treenode text="g_city" id="633086135845414176" vid="633083736164897476">
        <Properties type="String">g_city</Properties>
      </treenode>
      <treenode text="g_state" id="633086135845414178" vid="633083736164897478">
        <Properties type="String">g_state</Properties>
      </treenode>
      <treenode text="g_zip" id="633086135845414180" vid="633083736164897480">
        <Properties type="String">g_zip</Properties>
      </treenode>
      <treenode text="g_country" id="633086135845414182" vid="633083736164897482">
        <Properties type="String">g_country</Properties>
      </treenode>
      <treenode text="g_lat" id="633086135845414184" vid="633083736164897484">
        <Properties type="String">g_lat</Properties>
      </treenode>
      <treenode text="g_long" id="633086135845414186" vid="633083736164897486">
        <Properties type="String">g_long</Properties>
      </treenode>
      <treenode text="g_showMapHost" id="633086135845414188" vid="633083736164897524">
        <Properties type="String">g_showMapHost</Properties>
      </treenode>
      <treenode text="g_showMapRemoteHost" id="633086135845414190" vid="633083736164897526">
        <Properties type="String">g_showMapRemoteHost</Properties>
      </treenode>
      <treenode text="g_showMapQuery" id="633086135845414192" vid="633083736164897530">
        <Properties type="Metreos.Types.Http.QueryParamCollection">g_showMapQuery</Properties>
      </treenode>
      <treenode text="g_currentLat" id="633086135845414194" vid="633083925444684591">
        <Properties type="String">g_currentLat</Properties>
      </treenode>
      <treenode text="g_currentLong" id="633086135845414196" vid="633083925444684593">
        <Properties type="String">g_currentLong</Properties>
      </treenode>
      <treenode text="g_currentTopLeftLat" id="633086135845414198" vid="633083925444684595">
        <Properties type="String">g_currentTopLeftLat</Properties>
      </treenode>
      <treenode text="g_currentTopLeftLong" id="633086135845414200" vid="633083925444684597">
        <Properties type="String">g_currentTopLeftLong</Properties>
      </treenode>
      <treenode text="g_currentBottomRightLat" id="633086135845414202" vid="633083925444684599">
        <Properties type="String">g_currentBottomRightLat</Properties>
      </treenode>
      <treenode text="g_currentBottomRightLong" id="633086135845414204" vid="633083925444684601">
        <Properties type="String">g_currentBottomRightLong</Properties>
      </treenode>
      <treenode text="g_showMapHostname" id="633086135845414206" vid="633083955135956736">
        <Properties type="String">g_showMapHostname</Properties>
      </treenode>
      <treenode text="g_firstName" id="633086135845414208" vid="633083955135956748">
        <Properties type="String">g_firstName</Properties>
      </treenode>
      <treenode text="g_lastName" id="633086135845414210" vid="633083955135956750">
        <Properties type="String">g_lastName</Properties>
      </treenode>
      <treenode text="g_resellerLat" id="633086135845414212" vid="633084517770577908">
        <Properties type="String">g_resellerLat</Properties>
      </treenode>
      <treenode text="g_resellerLong" id="633086135845414214" vid="633084517770577910">
        <Properties type="String">g_resellerLong</Properties>
      </treenode>
      <treenode text="g_resellerLatOffsetHardCoded" id="633086135845414216" vid="633085161013817735">
        <Properties type="String" initWith="ResellerLatOffestFixed">g_resellerLatOffsetHardCoded</Properties>
      </treenode>
      <treenode text="g_resellerLongOffsetHardCoded" id="633086135845414218" vid="633085161013817737">
        <Properties type="String" initWith="ResellerLongOffsetFixed">g_resellerLongOffsetHardCoded</Properties>
      </treenode>
      <treenode text="g_resellerPhoneNumber" id="633086135845414220" vid="633085161013818108">
        <Properties type="String" initWith="ResellerPhoneNumber">g_resellerPhoneNumber</Properties>
      </treenode>
      <treenode text="g_resellerCompanyName" id="633086135845414222" vid="633085161013818852">
        <Properties type="String" initWith="ResellerCompanyName">g_resellerCompanyName</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnJTapiIncomingCall" startnode="632983074993061879" treenode="632983074993061880" appnode="632983074993061877" handlerfor="632983074993061876">
    <node type="Start" id="632983074993061879" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="77" y="278">
      <linkto id="633004250508108167" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632983410695910284" name="NotifyIncomingCall" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="275" y="279">
      <linkto id="633082570944688971" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="From" type="variable">from</ap>
        <ap name="CallId" type="variable">callId</ap>
        <ap name="To" type="variable">to</ap>
        <ap name="OriginalTo" type="variable">originalTo</ap>
        <ap name="DeviceName" type="variable">deviceName</ap>
        <rd field="IsSubscriber">isSubscribed</rd>
      </Properties>
    </node>
    <node type="Action" id="632983410695910286" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="658" y="609">
      <linkto id="632983417216714905" type="Labeled" style="Vector" label="true" />
      <linkto id="633071510444336770" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">isSubscribed</ap>
      </Properties>
    </node>
    <node type="Action" id="632983410695910287" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="630" y="759">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632983417216714905" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="520" y="759">
      <linkto id="632983410695910287" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">callId</ap>
        <rd field="ResultData">g_callId</rd>
      </Properties>
    </node>
    <node type="Action" id="633004250508108167" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="163" y="278">
      <linkto id="632983410695910284" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">from</ap>
        <ap name="Value2" type="variable">to</ap>
        <ap name="Value3" type="variable">deviceName</ap>
        <ap name="Value4" type="literal">0</ap>
        <rd field="ResultData">g_from</rd>
        <rd field="ResultData2">g_to</rd>
        <rd field="ResultData3">g_deviceName</rd>
        <rd field="ResultData4">g_bargeConfId</rd>
      </Properties>
    </node>
    <node type="Action" id="633071510444336770" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="840" y="758">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633082570944688551" name="GetResellerContacts" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="968" y="280">
      <linkto id="632983410695910286" type="Labeled" style="Vector" label="default" />
      <linkto id="633082570944688981" type="Labeled" style="Bezier" label="Success" />
      <linkto id="633084397433931084" type="Labeled" style="Bezier" label="comment" />
      <Properties final="false" type="provider" log="On">
        <rd field="Resellers">resellers</rd>
      </Properties>
    </node>
    <node type="Action" id="633082570944688552" name="CustomerLookup" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="808" y="280">
      <linkto id="633082570944688551" type="Labeled" style="Vector" label="Success" />
      <linkto id="632983410695910286" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="PhoneNumber" type="variable">from</ap>
        <rd field="FirstName">g_firstName</rd>
        <rd field="Street">g_street</rd>
        <rd field="Country">g_country</rd>
        <rd field="Latitude">g_lat</rd>
        <rd field="LastName">g_lastName</rd>
        <rd field="City">g_city</rd>
        <rd field="State">g_state</rd>
        <rd field="PostalCode">g_zip</rd>
        <rd field="Longitude">g_long</rd>
      </Properties>
    </node>
    <node type="Action" id="633082570944688971" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="389.4876" y="279.4245">
      <linkto id="633082570944688972" type="Labeled" style="Vector" label="Success" />
      <linkto id="632983410695910286" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">deviceName</ap>
        <rd field="ResultData">deviceIpResults</rd>
      </Properties>
    </node>
    <node type="Action" id="633082570944688972" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="552" y="280">
      <linkto id="633082570944688973" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">deviceIpResults.Rows.Count == 0 ? String.Empty : deviceIpResults.Rows[0]["IP"] as string</ap>
        <rd field="ResultData">g_phoneIp</rd>
      </Properties>
    </node>
    <node type="Action" id="633082570944688973" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="664" y="280">
      <linkto id="632983410695910286" type="Labeled" style="Vector" label="false" />
      <linkto id="633082570944688552" type="Labeled" style="Vector" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_phoneIp != null &amp;&amp; g_phoneIp != String.Empty</ap>
      </Properties>
    </node>
    <node type="Comment" id="633082570944688979" text="if we can find a device associated with this device name..." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="734" />
    <node type="Action" id="633082570944688981" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1144" y="280">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="633082570944688982" text="Send Gmap Provider Request for image here..." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="928" y="160" />
    <node type="Comment" id="633082570944688983" text=" reader[0] as string, /* Contact SF Id */&#xD;&#xA;															 reader[1] as string, /* First Name */&#xD;&#xA;															 reader[2] as string, /* Last Name */&#xD;&#xA;															 reader[3] as string, /* Phone Number */&#xD;&#xA;															 reader[4] as string, /* Street */&#xD;&#xA;															 reader[5] as string, /* City */&#xD;&#xA;															 reader[6] as string, /* State */&#xD;&#xA;															 reader[7] as string, /* PostalCode */&#xD;&#xA;															 reader[8] as string, /* Country */&#xD;&#xA;															 reader[9] as string, /* Latitude */&#xD;&#xA;															 reader[10] as string, /* Longitude */&#xD;&#xA;															 reader[11] as string, /* Account SF ID */&#xD;&#xA;															 reader[12] as string, /* Account Name */&#xD;&#xA;															 reader[13] as string /* Account Type */" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="944" y="328" />
    <node type="Action" id="633084397433931084" name="ShowReseller" class="MaxAsyncActionNode" group="" path="Metreos.Providers.zshakeel" x="967.4707" y="16" mx="1046" my="32">
      <items count="2">
        <item text="OnShowReseller_Complete" treenode="633084397433931078" />
        <item text="OnShowReseller_Failed" treenode="633084397433931083" />
      </items>
      <linkto id="633082570944688981" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Resellers" type="variable">resellers</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Variable" id="632983074993061881" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.Providers.JTapi.JTapiIncomingCall">to</Properties>
    </node>
    <node type="Variable" id="632983074993061882" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.Providers.JTapi.JTapiIncomingCall">from</Properties>
    </node>
    <node type="Variable" id="632983074993061883" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.JTapi.JTapiIncomingCall">deviceName</Properties>
    </node>
    <node type="Variable" id="632983074993061884" name="originalTo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="OriginalTo" refType="reference" name="Metreos.Providers.JTapi.JTapiIncomingCall">originalTo</Properties>
    </node>
    <node type="Variable" id="632983074993061885" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.Providers.JTapi.JTapiIncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632983410695910285" name="isSubscribed" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">isSubscribed</Properties>
    </node>
    <node type="Variable" id="633082570944688977" name="deviceIpResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">deviceIpResults</Properties>
    </node>
    <node type="Variable" id="633082570944688980" name="resellers" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="ArrayList" refType="reference">resellers</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallActive" startnode="632983074993061889" treenode="632983074993061890" appnode="632983074993061887" handlerfor="632983074993061886">
    <node type="Start" id="632983074993061889" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="85" y="304">
      <linkto id="632983410695910296" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632983410695910290" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="784" y="440">
      <linkto id="633082867945088149" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">isSubscribed</ap>
      </Properties>
    </node>
    <node type="Action" id="632983410695910296" name="NotifyCallActive" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="208" y="304">
      <linkto id="633071510444336287" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="DeviceName" type="variable">deviceName</ap>
        <ap name="CallId" type="variable">callId</ap>
        <ap name="To" type="variable">to</ap>
        <rd field="IsSubscriber">isSubscribed</rd>
      </Properties>
    </node>
    <node type="Action" id="633004250508108164" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="504" y="304">
      <linkto id="633004250508108571" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("INSERT INTO activecalls (devicename, to_number, from_number, active, direction, state) VALUES ('{0}', '{1}', '{2}', NOW(), {3}, {4})", deviceName, to, g_from, 0, 1)</ap>
        <ap name="Name" type="literal">activecalls</ap>
      </Properties>
    </node>
    <node type="Action" id="633004250508108571" name="ExecuteScalar" class="MaxActionNode" group="" path="Metreos.Native.Database" x="640" y="304">
      <linkto id="632983410695910290" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="literal">SELECT LAST_INSERT_ID()</ap>
        <ap name="Name" type="literal">activecalls</ap>
        <rd field="Scalar">g_recId</rd>
      </Properties>
    </node>
    <node type="Comment" id="633071262887419501" text="&#xD;&#xA;CREATE TABLE activecalls&#xD;&#xA;(&#xD;&#xA;  id INT unsigned NOT NULL auto_increment,&#xD;&#xA;  devicename VARCHAR(25),&#xD;&#xA;  to_number VARCHAR(25) NOT NULL DEFAULT '',&#xD;&#xA;  from_number VARCHAR(25) NOT NULL DEFAULT '',&#xD;&#xA;	active TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',&#xD;&#xA;  direction tinyint(1) unsigned NOT NULL default '0', /* 0=inbound, 1=outbound */&#xD;&#xA;	state tinyint(1) unsigned NOT NULL default '0', /* 0=hold, 1=active */&#xD;&#xA;	PRIMARY KEY(id)&#xD;&#xA;);" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="32" />
    <node type="Action" id="633071510444336287" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="336" y="304">
      <linkto id="633071510444336288" type="Labeled" style="Vector" label="default" />
      <linkto id="633004250508108164" type="Labeled" style="Vector" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_recId == 0</ap>
        <log condition="entry" on="true" level="Info" type="literal">g_recId</log>
      </Properties>
    </node>
    <node type="Action" id="633071510444336288" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="336" y="440">
      <linkto id="632983410695910290" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("UPDATE activecalls SET state = 1 WHERE id = {0}", g_recId)</ap>
        <ap name="Name" type="literal">activecalls</ap>
      </Properties>
    </node>
    <node type="Comment" id="633071510444336291" text="EndScript would be nice, but&#xD;&#xA;I need to worry about removing&#xD;&#xA;the call on hangup from&#xD;&#xA;activecalls db" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="715" y="278" />
    <node type="Comment" id="633073282571208173" text="first check if cache has customer" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="512" y="121" />
    <node type="Action" id="633082867945088149" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="859.7418" y="505">
      <linkto id="633082867945088329" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteIPAddress" type="variable">g_phoneIp</ap>
        <ap name="url" type="csharp">"http://" + g_AppServerIP + ":8000/SalesforceDemo/ShowMap?metreosSessionId=" + routingGuid</ap>
        <ap name="EventName" type="literal">Metreos.Providers.SF.Worker</ap>
      </Properties>
    </node>
    <node type="Action" id="633082867945088329" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="959.9918" y="505.091125">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632983074993061896" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.Providers.JTapi.JTapiCallActive">to</Properties>
    </node>
    <node type="Variable" id="632983074993061897" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.JTapi.JTapiCallActive">deviceName</Properties>
    </node>
    <node type="Variable" id="632983074993061898" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.Providers.JTapi.JTapiCallActive">callId</Properties>
    </node>
    <node type="Variable" id="632983410695910289" name="isSubscribed" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">isSubscribed</Properties>
    </node>
    <node type="Variable" id="633073282571208174" name="checkHasContactInfo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">checkHasContactInfo</Properties>
    </node>
    <node type="Variable" id="633082867945088328" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallInactive" startnode="632983074993061894" treenode="632983074993061895" appnode="632983074993061892" handlerfor="632983074993061891">
    <node type="Start" id="632983074993061894" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="110" y="303">
      <linkto id="632983410695910303" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632983410695910297" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="580" y="303">
      <linkto id="632983410695910298" type="Labeled" style="Vector" label="true" />
      <linkto id="633071510444336771" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">isSubscribed</ap>
      </Properties>
    </node>
    <node type="Action" id="632983410695910298" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="723" y="181">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632983410695910303" name="NotifyCallInactive" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="262" y="303">
      <linkto id="633071262887419502" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="InUse" type="variable">inUse</ap>
        <ap name="DeviceName" type="variable">deviceName</ap>
        <ap name="CallId" type="variable">callId</ap>
        <rd field="IsSubscriber">isSubscribed</rd>
      </Properties>
    </node>
    <node type="Action" id="633071262887419502" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="450.611328" y="303">
      <linkto id="632983410695910297" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("UPDATE activecalls SET state = 0 WHERE id = {0}", g_recId)</ap>
        <ap name="Name" type="literal">activecalls</ap>
      </Properties>
    </node>
    <node type="Action" id="633071510444336771" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="723" y="436">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632983074993061899" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInactive">callId</Properties>
    </node>
    <node type="Variable" id="632983074993061900" name="inUse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" initWith="InUse" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInactive">inUse</Properties>
    </node>
    <node type="Variable" id="632983074993061901" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.JTapi.JTapiCallInactive">deviceName</Properties>
    </node>
    <node type="Variable" id="632983410695910304" name="isSubscribed" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" refType="reference">isSubscribed</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiHangup" startnode="632983074993061905" treenode="632983074993061906" appnode="632983074993061903" handlerfor="632983074993061902">
    <node type="Start" id="632983074993061905" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="145" y="286">
      <linkto id="632983410695910311" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632983410695910307" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="616" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632983410695910311" name="NotifyHangup" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="331" y="290">
      <linkto id="633004250508108569" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="DeviceName" type="variable">deviceName</ap>
        <ap name="Cause" type="variable">cause</ap>
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="633004250508108569" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="464" y="288">
      <linkto id="632983410695910307" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("DELETE FROM activecalls WHERE id = {0}", g_recId)</ap>
        <ap name="Name" type="literal">activecalls</ap>
      </Properties>
    </node>
    <node type="Variable" id="632983074993061907" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.Providers.JTapi.JTapiHangup">callId</Properties>
    </node>
    <node type="Variable" id="632983074993061908" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference" name="Metreos.Providers.JTapi.JTapiHangup">deviceName</Properties>
    </node>
    <node type="Variable" id="632983074993061909" name="cause" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Cause" refType="reference" name="Metreos.Providers.JTapi.JTapiHangup">cause</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangupRequest" startnode="632989423340680751" treenode="632989423340680752" appnode="632989423340680749" handlerfor="632989423340680748">
    <node type="Start" id="632989423340680751" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="96" y="480">
      <linkto id="632989423340680753" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632989423340680753" name="JTapiHangup" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="272" y="480">
      <linkto id="633004250508108574" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632989423340680755" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="632" y="480">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633004250508108574" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="480" y="480">
      <linkto id="632989423340680755" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("DELETE FROM activecalls WHERE id = {0}", g_recId)</ap>
        <ap name="Name" type="literal">activecalls</ap>
      </Properties>
    </node>
    <node type="Variable" id="632989423340680754" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnConferenceRequest" startnode="633074064873750548" treenode="633074064873750549" appnode="633074064873750546" handlerfor="633074064873750545">
    <node type="Start" id="633074064873750548" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="50" y="175">
      <linkto id="633074067383281847" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633074064873750552" name="Barge" class="MaxActionNode" group="" path="Metreos.CallControl" x="291" y="370">
      <linkto id="633074064873750783" type="Labeled" style="Vector" label="default" />
      <linkto id="633074067383281848" type="Labeled" style="Vector" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="From" type="variable">g_to</ap>
        <rd field="CallId">g_bargeCallId</rd>
        <rd field="ConnectionId">g_bargeConnId</rd>
        <log condition="entry" on="true" level="Info" type="variable">g_to</log>
      </Properties>
    </node>
    <node type="Action" id="633074064873750783" name="ConferenceInitiated" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="414" y="584">
      <linkto id="633074067383281834" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="DeviceName" type="variable">g_deviceName</ap>
        <ap name="Success" type="csharp">false</ap>
      </Properties>
    </node>
    <node type="Action" id="633074067383281827" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="446" y="354" mx="512" my="370">
      <items count="2">
        <item text="OnMakeCall_Complete" treenode="633074067383281821" />
        <item text="OnMakeCall_Failed" treenode="633074067383281826" />
      </items>
      <linkto id="633074064873750783" type="Labeled" style="Vector" label="default" />
      <linkto id="633074067383281830" type="Labeled" style="Vector" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">to</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_bargeConfId</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
        <rd field="CallId">outboundCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="633074067383281830" name="ConferenceInitiated" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="708" y="373">
      <linkto id="633074067383281833" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="DeviceName" type="variable">g_deviceName</ap>
        <ap name="FirstPartyCallId" type="variable">outboundCallId</ap>
        <ap name="Success" type="csharp">true</ap>
      </Properties>
    </node>
    <node type="Action" id="633074067383281833" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="883" y="374">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633074067383281834" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="411" y="700.0911">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633074067383281847" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="176" y="176">
      <linkto id="633074067383281827" type="Labeled" style="Vector" label="true" />
      <linkto id="633074086161878307" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_barged</ap>
      </Properties>
    </node>
    <node type="Action" id="633074067383281848" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="504" y="264">
      <linkto id="633074067383281827" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <rd field="ResultData">g_barged</rd>
      </Properties>
    </node>
    <node type="Comment" id="633074067383281849" text="if already barged..." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="148" y="129.091125" />
    <node type="Action" id="633074086161877875" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="624" y="712">
      <linkto id="633074086161877876" type="Labeled" style="Vector" label="Success" />
      <linkto id="633074086161877879" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">g_deviceName</ap>
        <rd field="ResultData">deviceIpResults</rd>
      </Properties>
    </node>
    <node type="Action" id="633074086161877876" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="784" y="712">
      <linkto id="633074086161877877" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">deviceIpResults.Rows.Count == 0 ? String.Empty : deviceIpResults.Rows[0]["IP"] as string</ap>
        <rd field="ResultData">deviceIp</rd>
      </Properties>
    </node>
    <node type="Action" id="633074086161877877" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="888" y="712">
      <linkto id="633074086161877881" type="Labeled" style="Vector" label="default" />
      <linkto id="633074086161878284" type="Labeled" style="Vector" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">deviceIp != null &amp;&amp; deviceIp != String.Empty</ap>
      </Properties>
    </node>
    <node type="Label" id="633074086161877879" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="624" y="928" />
    <node type="Label" id="633074086161877881" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="888" y="928" />
    <node type="Label" id="633074086161877889" text="t" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="104" y="384" />
    <node type="Label" id="633074086161877890" text="t" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="520" y="712">
      <linkto id="633074086161877875" type="Basic" style="Vector" />
    </node>
    <node type="Label" id="633074086161877892" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="286.005859" y="586">
      <linkto id="633074064873750783" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633074086161878284" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1040" y="712">
      <linkto id="633074086161878292" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(ref int chosenPort)
{
	System.Random rand = new System.Random();
	chosenPort = rand.Next(20480, 32000);
	if(chosenPort % 2 == 1)
	{
		chosenPort = chosenPort + 1;
	}
	return "success";
}
</Properties>
    </node>
    <node type="Action" id="633074086161878285" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1352" y="712">
      <linkto id="633074086161878286" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"RTPRx:" + mediaEngineIp + ":" + chosenPort</ap>
        <ap name="URL2" type="csharp">"RTPTx:" + mediaEngineIp + ":" + mediaEnginePort</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="633074086161878286" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1512" y="712">
      <linkto id="633074086161878297" type="Labeled" style="Vector" label="default" />
      <linkto id="633074086161878300" type="Labeled" style="Vector" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">deviceIp</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="633074086161878292" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="1184" y="712">
      <linkto id="633074086161878285" type="Labeled" style="Vector" label="Success" />
      <linkto id="633074086161878295" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxIP" type="variable">deviceIp</ap>
        <ap name="MediaTxPort" type="variable">chosenPort</ap>
        <rd field="MediaRxIP">mediaEngineIp</rd>
        <rd field="MediaRxPort">mediaEnginePort</rd>
        <rd field="ConnectionId">g_bargeConnId</rd>
      </Properties>
    </node>
    <node type="Label" id="633074086161878295" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1184" y="920" />
    <node type="Label" id="633074086161878297" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1512" y="920" />
    <node type="Label" id="633074086161878300" text="j" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1640" y="712" />
    <node type="Label" id="633074086161878301" text="j" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="435" y="196">
      <linkto id="633074067383281848" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633074086161878307" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="104" y="240">
      <linkto id="633074086161877889" type="Labeled" style="Vector" label="default" />
      <linkto id="633074064873750552" type="Labeled" style="Vector" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_useBarge</ap>
      </Properties>
    </node>
    <node type="Variable" id="633074064873750550" name="jtapiCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="JTAPICallId" refType="reference" name="Metreos.Providers.SalesforceDemo.ConferenceRequest">jtapiCallId</Properties>
    </node>
    <node type="Variable" id="633074064873750551" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.Providers.SalesforceDemo.ConferenceRequest">to</Properties>
    </node>
    <node type="Variable" id="633074064873750778" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
    <node type="Variable" id="633074067383281832" name="outboundCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">outboundCallId</Properties>
    </node>
    <node type="Variable" id="633074086161877894" name="deviceIpResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">deviceIpResults</Properties>
    </node>
    <node type="Variable" id="633074086161877896" name="deviceIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">deviceIp</Properties>
    </node>
    <node type="Variable" id="633074086161878290" name="chosenPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">chosenPort</Properties>
    </node>
    <node type="Variable" id="633074086161878291" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="633074086161878293" name="mediaEngineIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">mediaEngineIp</Properties>
    </node>
    <node type="Variable" id="633074086161878294" name="mediaEnginePort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">mediaEnginePort</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="633074067383281820" treenode="633074067383281821" appnode="633074067383281818" handlerfor="633074067383281817">
    <node type="Start" id="633074067383281820" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="57" y="343">
      <linkto id="633074067383281850" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633074067383281838" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="336" y="345">
      <linkto id="633074067383281840" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_bargeConnId</ap>
        <ap name="ConferenceId" type="variable">g_bargeConfId</ap>
      </Properties>
    </node>
    <node type="Action" id="633074067383281840" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="537" y="346">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633074067383281850" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="186" y="342">
      <linkto id="633074067383281838" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">confId</ap>
        <rd field="ResultData">g_bargeConfId</rd>
      </Properties>
    </node>
    <node type="Variable" id="633074067383281839" name="confId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConferenceId" refType="reference">confId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="633074067383281825" treenode="633074067383281826" appnode="633074067383281823" handlerfor="633074067383281822">
    <node type="Start" id="633074067383281825" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="68" y="390">
      <linkto id="633074067383281843" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633074067383281843" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="341" y="391">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ShowMap" startnode="633082867945088529" treenode="633082867945088530" appnode="633082867945088527" handlerfor="633082867945088526">
    <node type="Start" id="633082867945088529" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="39">
      <linkto id="633083736164897471" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="633083736164897471" name="ShowMap" class="MaxAsyncActionNode" group="" path="Metreos.Providers.zshakeel" x="53" y="152" mx="121" my="168">
      <items count="2">
        <item text="OnShowMap_Complete" treenode="633083736164897465" />
        <item text="OnShowMap_Failed" treenode="633083736164897470" />
      </items>
      <linkto id="633083736164897532" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="state" type="variable">g_state</ap>
        <ap name="street" type="variable">g_street</ap>
        <ap name="city" type="variable">g_city</ap>
        <ap name="zip" type="variable">g_zip</ap>
        <ap name="country" type="variable">g_country</ap>
        <ap name="lat" type="variable">g_lat</ap>
        <ap name="long" type="variable">g_long</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="633083736164897532" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="117" y="291">
      <linkto id="633083736164897535" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">host</ap>
        <ap name="Value2" type="variable">remoteHost</ap>
        <ap name="Value3" type="variable">hostname</ap>
        <ap name="Value4" type="variable">query</ap>
        <rd field="ResultData">g_showMapHost</rd>
        <rd field="ResultData2">g_showMapRemoteHost</rd>
        <rd field="ResultData3">g_showMapHostname</rd>
        <rd field="ResultData4">g_showMapQuery</rd>
      </Properties>
    </node>
    <node type="Action" id="633083736164897535" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="126" y="375">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633082867945088578" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="633082867945088579" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="633082867945088580" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="633082867945088581" name="imageBuilderHandle" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Imaging.ImageBuilder" refType="reference">imageBuilderHandle</Properties>
    </node>
    <node type="Variable" id="633082867945088582" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">query</Properties>
    </node>
    <node type="Variable" id="633082867945088583" name="incomingImageName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">incomingImageName</Properties>
    </node>
    <node type="Variable" id="633082867945088584" name="graphicFileMenuHandle" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.GraphicFileMenu" refType="reference">graphicFileMenuHandle</Properties>
    </node>
    <node type="Variable" id="633083955135956738" name="hostname" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="hostname" refType="reference">hostname</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnShowMap_Complete" startnode="633083736164897464" treenode="633083736164897465" appnode="633083736164897462" handlerfor="633083736164897461">
    <node type="Start" id="633083736164897464" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="63">
      <linkto id="633083925444684588" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633083736164897496" name="CreateGraphicFileMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="375" y="962.794434">
      <linkto id="633083736164897497" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Caller Location</ap>
        <ap name="Prompt" type="literal">Click Any Item</ap>
        <ap name="LocationX" type="literal">-1</ap>
        <ap name="LocationY" type="literal">-1</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHostname + "/mceadmin/images/newMap.png"</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"http://" + g_showMapHost + "/mceadmin/images/newMap.png"</log>
      </Properties>
    </node>
    <node type="Action" id="633083736164897497" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="375" y="846.794434">
      <linkto id="633083736164897500" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">DisplayName</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHost + "/SalesforceDemo/MoveMap?metreosSessionId=" + routingGuid + "&amp;value=up"</ap>
        <ap name="TouchAreaX1" type="literal">140</ap>
        <ap name="TouchAreaX2" type="literal">156</ap>
        <ap name="TouchAreaY1" type="literal">1</ap>
        <ap name="TouchAreaY2" type="literal">17</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083736164897498" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="425" y="371.794373">
      <linkto id="633083736164897499" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">g_showMapRemoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">graphicFileMenuHandle.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="633083736164897499" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="534" y="449.794373">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633083736164897500" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="373" y="749.794434">
      <linkto id="633083736164897501" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">DisplayName</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHost + "/SalesforceDemo/MoveMap?metreosSessionId=" + routingGuid + "&amp;value=down"</ap>
        <ap name="TouchAreaX1" type="literal">140</ap>
        <ap name="TouchAreaX2" type="literal">156</ap>
        <ap name="TouchAreaY1" type="literal">151</ap>
        <ap name="TouchAreaY2" type="literal">167</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"http://" + g_showMapHost + "/SalesforceDemo/MoveMap?metreosSessionId=" + routingGuid + "&amp;value=down"</log>
      </Properties>
    </node>
    <node type="Action" id="633083736164897501" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="373" y="657.794434">
      <linkto id="633083736164897502" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="Off">
        <ap name="Name" type="literal">DisplayName</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHost + "/SalesforceDemo/MoveMap?metreosSessionId=" + routingGuid + "&amp;value=left"</ap>
        <ap name="TouchAreaX1" type="literal">0</ap>
        <ap name="TouchAreaX2" type="literal">16</ap>
        <ap name="TouchAreaY1" type="literal">76</ap>
        <ap name="TouchAreaY2" type="literal">92</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083736164897502" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="373" y="554.794434">
      <linkto id="633085462900292178" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">DisplayName</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHost + "/SalesforceDemo/MoveMap?metreosSessionId=" + routingGuid + "&amp;value=right"</ap>
        <ap name="TouchAreaX1" type="literal">281</ap>
        <ap name="TouchAreaX2" type="literal">297</ap>
        <ap name="TouchAreaY1" type="literal">76</ap>
        <ap name="TouchAreaY2" type="literal">92</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083736164897503" name="CreateImageBuilder" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="110" y="339.794373">
      <linkto id="633083736164897504" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <rd field="ImageBuilder">imageBuilderHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083736164897504" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="218" y="325.794373">
      <linkto id="633083955135956742" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="variable">incomingFilename</ap>
        <ap name="Top" type="literal">0</ap>
        <ap name="Left" type="literal">0</ap>
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <rd field="Image">imageBuilderHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083736164897505" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="218" y="443.88562">
      <linkto id="633083736164897508" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="literal">c:\SFDemo\arrow-up.gif</ap>
        <ap name="Top" type="literal">1</ap>
        <ap name="Left" type="literal">140</ap>
        <ap name="Width" type="literal">16</ap>
        <ap name="Height" type="literal">16</ap>
        <rd field="Image">imageBuilderHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083736164897506" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="217" y="655.8856">
      <linkto id="633083736164897507" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="literal">c:\SFDemo\arrow-left.gif</ap>
        <ap name="Top" type="literal">76</ap>
        <ap name="Left" type="literal">0</ap>
        <ap name="Width" type="literal">16</ap>
        <ap name="Height" type="literal">16</ap>
        <rd field="Image">imageBuilderHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083736164897507" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="216" y="730.8856">
      <linkto id="633083736164897509" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="literal">c:\SFDemo\arrow-right.gif</ap>
        <ap name="Top" type="literal">76</ap>
        <ap name="Left" type="literal">281</ap>
        <ap name="Width" type="literal">16</ap>
        <ap name="Height" type="literal">16</ap>
        <rd field="Image">imageBuilderHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083736164897508" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="218" y="545.8856">
      <linkto id="633083736164897506" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="literal">c:\SFDemo\arrow-down.gif</ap>
        <ap name="Top" type="literal">151</ap>
        <ap name="Left" type="literal">140</ap>
        <ap name="Width" type="literal">16</ap>
        <ap name="Height" type="literal">16</ap>
        <rd field="Image">imageBuilderHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083736164897509" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="186" y="961.885559">
      <linkto id="633083736164897496" type="Labeled" style="Bezier" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Imaging.ImageBuilder imageBuilderHandle)
{
	imageBuilderHandle.Save("C:\\Program Files\\Metreos\\mceadmin\\public\\images\\newMap.png");
	return "";
}
</Properties>
    </node>
    <node type="Action" id="633083925444684588" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="73" y="156">
      <linkto id="633083955135956752" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">incomingLat</ap>
        <ap name="Value2" type="variable">incomingLong</ap>
        <ap name="Value3" type="variable">incomingTopLeftLat</ap>
        <ap name="Value4" type="variable">incomingTopLeftLong</ap>
        <rd field="ResultData">g_currentLat</rd>
        <rd field="ResultData2">g_currentLong</rd>
        <rd field="ResultData3">g_currentTopLeftLat</rd>
        <rd field="ResultData4">g_currentTopLeftLong</rd>
      </Properties>
    </node>
    <node type="Action" id="633083925444684589" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="81" y="241">
      <linkto id="633083736164897503" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">incomingBottomRightLat</ap>
        <ap name="Value2" type="variable">incomingBottomRightLong</ap>
        <rd field="ResultData">g_currentBottomRightLat</rd>
        <rd field="ResultData2">g_currentBottomRightLong</rd>
      </Properties>
    </node>
    <node type="Action" id="633083955135956742" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="358" y="322.885559">
      <linkto id="633083736164897505" type="Labeled" style="Vector" label="false" />
      <linkto id="633083955135956747" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(string g_lat, string g_long, string g_currentLat, string g_currentLong, string g_currentTopLeftLat, string g_currentTopLeftLong, string g_currentBottomRightLat, string g_currentBottomRightLong, ref int callerLeft, ref int callerTop, LogWriter log)
{
	double callerLat = Convert.ToDouble(g_lat);
	double callerLong = Convert.ToDouble(g_long);

	double latitude = Convert.ToDouble(g_currentLat);
	double longitude = Convert.ToDouble(g_currentLong);

	// Do some math to figure out offset amount.
	double leftLong = Convert.ToDouble(g_currentTopLeftLong);
	double rightLong = Convert.ToDouble(g_currentBottomRightLong);
	double longWidth = leftLong - rightLong;

	double topLat = Convert.ToDouble(g_currentTopLeftLat);
	double bottomLat = Convert.ToDouble(g_currentBottomRightLat);

	double latHeight = topLat - bottomLat;

	double normalizedLong = (leftLong - callerLong) / longWidth;
	
	bool onscreen = true;

	if(normalizedLong &gt; 0d &amp;&amp; normalizedLong &lt; 1.0d)
	{
		callerLeft = (int)(298d * normalizedLong);
	}
	else
	{
		onscreen = false;
	}

	double normalizedLat = (topLat - callerLat) / latHeight;
	
	if(normalizedLat &gt; 0d &amp;&amp; normalizedLat &lt; 1.0d)
	{
		callerTop = (int)(168d * normalizedLat);
	}
	else
	{
		onscreen = false;
	}

	log.Write(TraceLevel.Verbose, String.Format(@"
callerLat {0}
callerLong {1}
mapLat {2}
mapLong {3}
leftLong {4}
rightLong {5}
longWidth {6}
topLat {7}
bottomLat {8}
latHeight {9}
normalizedLong {10}
normalizedLat {11}
callerLeft {12}
callerTop {13}
oncreen {14}",
callerLat,
callerLong,
latitude,
longitude,
leftLong,
rightLong,
longWidth,
topLat,
bottomLat,
latHeight,
normalizedLong,
normalizedLat,
callerLeft,
callerTop,
onscreen));
 

	return onscreen.ToString();
}
</Properties>
    </node>
    <node type="Action" id="633083955135956747" name="AddBorderedTextRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="72" y="412.885559">
      <linkto id="633084517770577919" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Text" type="csharp">g_firstName + " " +  g_lastName</ap>
        <ap name="Font" type="literal">Arial</ap>
        <ap name="FontSize" type="literal">10</ap>
        <ap name="Top" type="variable">callerTop</ap>
        <ap name="Left" type="variable">callerLeft</ap>
        <ap name="RoundedCorners" type="literal">true</ap>
        <ap name="HorizontalPadding" type="literal">3</ap>
        <ap name="VerticalPadding" type="literal">3</ap>
        <ap name="TextColor" type="csharp">Color.Black</ap>
        <ap name="BorderColor" type="csharp">Color.Black</ap>
        <ap name="FillColor" type="csharp">Color.Yellow</ap>
        <ap name="BorderWidth" type="csharp">1</ap>
        <rd field="Image">imageBuilderHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083955135956752" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="197" y="192">
      <linkto id="633083925444684589" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">incomingLat</ap>
        <ap name="Value2" type="variable">incomingLong</ap>
        <rd field="ResultData">g_lat</rd>
        <rd field="ResultData2">g_long</rd>
      </Properties>
    </node>
    <node type="Action" id="633084517770577919" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="88.31249" y="496">
      <linkto id="633084517770577920" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">callerLeft - 40</ap>
        <ap name="Value2" type="csharp">callerTop - 40</ap>
        <rd field="ResultData">resellerLeft</rd>
        <rd field="ResultData2">resellerTop</rd>
      </Properties>
    </node>
    <node type="Action" id="633084517770577920" name="AddBorderedTextRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="83.73046" y="593">
      <linkto id="633083736164897505" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Text" type="variable">g_resellerCompanyName</ap>
        <ap name="Font" type="literal">Arial</ap>
        <ap name="FontSize" type="literal">10</ap>
        <ap name="Top" type="variable">resellerTop</ap>
        <ap name="Left" type="variable">resellerLeft</ap>
        <ap name="RoundedCorners" type="literal">true</ap>
        <ap name="HorizontalPadding" type="literal">3</ap>
        <ap name="VerticalPadding" type="literal">3</ap>
        <ap name="TextColor" type="csharp">Color.Black</ap>
        <ap name="BorderColor" type="csharp">Color.Black</ap>
        <ap name="FillColor" type="csharp">Color.Yellow</ap>
        <ap name="BorderWidth" type="csharp">1</ap>
        <rd field="Image">imageBuilderHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633085462900292178" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="374" y="463.794434">
      <linkto id="633083736164897498" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">DisplayName</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHost + "/SalesforceDemo/ResellerSelected?metreosSessionId=" + routingGuid</ap>
        <ap name="TouchAreaX1" type="variable">resellerLeft</ap>
        <ap name="TouchAreaX2" type="csharp">resellerLeft + 32</ap>
        <ap name="TouchAreaY1" type="variable">resellerTop</ap>
        <ap name="TouchAreaY2" type="csharp">resellerTop + 32</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
      </Properties>
    </node>
    <node type="Variable" id="633083736164897488" name="incomingFilename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="filename" refType="reference">incomingFilename</Properties>
    </node>
    <node type="Variable" id="633083736164897489" name="incomingLat" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="lat" refType="reference">incomingLat</Properties>
    </node>
    <node type="Variable" id="633083736164897490" name="incomingLong" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="long" refType="reference">incomingLong</Properties>
    </node>
    <node type="Variable" id="633083736164897491" name="incomingTopLeftLat" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="topleftlat" refType="reference">incomingTopLeftLat</Properties>
    </node>
    <node type="Variable" id="633083736164897492" name="incomingTopLeftLong" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="topleftlong" refType="reference">incomingTopLeftLong</Properties>
    </node>
    <node type="Variable" id="633083736164897493" name="incomingBottomRightLat" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="bottomrightlat" refType="reference">incomingBottomRightLat</Properties>
    </node>
    <node type="Variable" id="633083736164897494" name="incomingBottomRightLong" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="bottomrightlong" refType="reference">incomingBottomRightLong</Properties>
    </node>
    <node type="Variable" id="633083736164897533" name="imageBuilderHandle" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Imaging.ImageBuilder" refType="reference">imageBuilderHandle</Properties>
    </node>
    <node type="Variable" id="633083736164897534" name="graphicFileMenuHandle" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.GraphicFileMenu" refType="reference">graphicFileMenuHandle</Properties>
    </node>
    <node type="Variable" id="633083736164897536" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="633083955135956745" name="callerLeft" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">callerLeft</Properties>
    </node>
    <node type="Variable" id="633083955135956746" name="callerTop" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">callerTop</Properties>
    </node>
    <node type="Variable" id="633084517770577923" name="resellerLeft" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">resellerLeft</Properties>
    </node>
    <node type="Variable" id="633084517770577924" name="resellerTop" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">resellerTop</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnShowMap_Failed" startnode="633083736164897469" treenode="633083736164897470" appnode="633083736164897467" handlerfor="633083736164897466">
    <node type="Start" id="633083736164897469" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633083736164897537" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="633083736164897537" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="165" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="MoveMap" startnode="633083925444684565" treenode="633083925444684566" appnode="633083925444684563" handlerfor="633083925444684562">
    <node type="Start" id="633083925444684565" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633083925444684581" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633083925444684568" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="161.4707" y="435">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633083925444684581" name="MoveMap" class="MaxAsyncActionNode" group="" path="Metreos.Providers.zshakeel" x="105" y="183" mx="172" my="199">
      <items count="2">
        <item text="OnMoveMap_Complete" treenode="633083925444684575" />
        <item text="OnMoveMap_Failed" treenode="633083925444684580" />
      </items>
      <linkto id="633083955135956739" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="bottomRightLat" type="variable">g_currentBottomRightLat</ap>
        <ap name="bottomRightLong" type="variable">g_currentBottomRightLong</ap>
        <ap name="direction" type="csharp">query["value"]</ap>
        <ap name="long" type="variable">g_currentLong</ap>
        <ap name="lat" type="variable">g_currentLat</ap>
        <ap name="topLeftLat" type="variable">g_currentTopLeftLat</ap>
        <ap name="topLeftLong" type="variable">g_currentTopLeftLong</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="633083955135956739" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="170.048828" y="335.2375">
      <linkto id="633083925444684568" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">host</ap>
        <ap name="Value2" type="variable">remoteHost</ap>
        <ap name="Value3" type="variable">hostname</ap>
        <ap name="Value4" type="variable">query</ap>
        <rd field="ResultData">g_showMapHost</rd>
        <rd field="ResultData2">g_showMapRemoteHost</rd>
        <rd field="ResultData3">g_showMapHostname</rd>
        <rd field="ResultData4">g_showMapQuery</rd>
      </Properties>
    </node>
    <node type="Variable" id="633083925444684584" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="633083925444684585" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="633083925444684586" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="633083955135956741" name="hostname" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="hostname" refType="reference">hostname</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMoveMap_Complete" startnode="633083925444684574" treenode="633083925444684575" appnode="633083925444684572" handlerfor="633083925444684571">
    <node type="Start" id="633083925444684574" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="68">
      <linkto id="633083925444684635" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633083925444684603" name="CreateGraphicFileMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="547.910156" y="868.794434">
      <linkto id="633083925444684650" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Caller Location</ap>
        <ap name="Prompt" type="literal">Click Any Item</ap>
        <ap name="LocationX" type="literal">-1</ap>
        <ap name="LocationY" type="literal">-1</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHostname + "/mceadmin/images/newMap.png"</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"http://" + g_showMapHost + "/mceadmin/images/newMap.png"</log>
      </Properties>
    </node>
    <node type="Action" id="633083925444684605" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="544" y="296">
      <linkto id="633083925444684606" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">g_showMapRemoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">graphicFileMenuHandle.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="633083925444684606" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="712" y="304">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633083925444684610" name="CreateImageBuilder" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="282.910156" y="245.794373">
      <linkto id="633083925444684611" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <rd field="ImageBuilder">imageBuilderHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083925444684611" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="358.910156" y="353.794373">
      <linkto id="633084244845316326" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="variable">incomingFilename</ap>
        <ap name="Top" type="literal">0</ap>
        <ap name="Left" type="literal">0</ap>
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <rd field="Image">imageBuilderHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083925444684612" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="358.910156" y="471.88562">
      <linkto id="633083925444684615" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="literal">c:\SFDemo\arrow-up.gif</ap>
        <ap name="Top" type="literal">1</ap>
        <ap name="Left" type="literal">140</ap>
        <ap name="Width" type="literal">16</ap>
        <ap name="Height" type="literal">16</ap>
        <rd field="Image">imageBuilderHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083925444684613" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="357.910156" y="683.8856">
      <linkto id="633083925444684614" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="literal">c:\SFDemo\arrow-left.gif</ap>
        <ap name="Top" type="literal">76</ap>
        <ap name="Left" type="literal">0</ap>
        <ap name="Width" type="literal">16</ap>
        <ap name="Height" type="literal">16</ap>
        <rd field="Image">imageBuilderHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083925444684614" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="357.910156" y="777.8856">
      <linkto id="633083925444684616" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="literal">c:\SFDemo\arrow-right.gif</ap>
        <ap name="Top" type="literal">76</ap>
        <ap name="Left" type="literal">281</ap>
        <ap name="Width" type="literal">16</ap>
        <ap name="Height" type="literal">16</ap>
        <rd field="Image">imageBuilderHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083925444684615" name="AddStandardImageRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="358.910156" y="573.8856">
      <linkto id="633083925444684613" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Location" type="literal">c:\SFDemo\arrow-down.gif</ap>
        <ap name="Top" type="literal">151</ap>
        <ap name="Left" type="literal">140</ap>
        <ap name="Width" type="literal">16</ap>
        <ap name="Height" type="literal">16</ap>
        <rd field="Image">imageBuilderHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083925444684616" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="358.910156" y="867.8855">
      <linkto id="633083925444684603" type="Labeled" style="Bezier" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Imaging.ImageBuilder imageBuilderHandle)
{
	imageBuilderHandle.Save("C:\\Program Files\\Metreos\\mceadmin\\public\\images\\newMap.png");
	return "";
}
</Properties>
    </node>
    <node type="Action" id="633083925444684635" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="253.048828" y="82">
      <linkto id="633083925444684636" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">incomingLat</ap>
        <ap name="Value2" type="variable">incomingLong</ap>
        <ap name="Value3" type="variable">incomingTopLeftLat</ap>
        <ap name="Value4" type="variable">incomingTopLeftLong</ap>
        <rd field="ResultData">g_currentLat</rd>
        <rd field="ResultData2">g_currentLong</rd>
        <rd field="ResultData3">g_currentTopLeftLat</rd>
        <rd field="ResultData4">g_currentTopLeftLong</rd>
      </Properties>
    </node>
    <node type="Action" id="633083925444684636" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="261.048828" y="167">
      <linkto id="633083925444684610" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">incomingBottomRightLat</ap>
        <ap name="Value2" type="variable">incomingBottomRightLong</ap>
        <rd field="ResultData">g_currentBottomRightLat</rd>
        <rd field="ResultData2">g_currentBottomRightLong</rd>
      </Properties>
    </node>
    <node type="Action" id="633083925444684650" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="549.045532" y="770">
      <linkto id="633083925444684651" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">DisplayName</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHost + "/SalesforceDemo/MoveMap?metreosSessionId=" + routingGuid + "&amp;value=up"</ap>
        <ap name="TouchAreaX1" type="literal">140</ap>
        <ap name="TouchAreaX2" type="literal">156</ap>
        <ap name="TouchAreaY1" type="literal">1</ap>
        <ap name="TouchAreaY2" type="literal">17</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083925444684651" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="547.045532" y="673">
      <linkto id="633083925444684652" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">DisplayName</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHost + "/SalesforceDemo/MoveMap?metreosSessionId=" + routingGuid + "&amp;value=down"</ap>
        <ap name="TouchAreaX1" type="literal">140</ap>
        <ap name="TouchAreaX2" type="literal">156</ap>
        <ap name="TouchAreaY1" type="literal">151</ap>
        <ap name="TouchAreaY2" type="literal">167</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083925444684652" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="547.045532" y="581">
      <linkto id="633083925444684653" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="Off">
        <ap name="Name" type="literal">DisplayName</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHost + "/SalesforceDemo/MoveMap?metreosSessionId=" + routingGuid + "&amp;value=left"</ap>
        <ap name="TouchAreaX1" type="literal">0</ap>
        <ap name="TouchAreaX2" type="literal">16</ap>
        <ap name="TouchAreaY1" type="literal">76</ap>
        <ap name="TouchAreaY2" type="literal">92</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633083925444684653" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="547.045532" y="478">
      <linkto id="633085462900293047" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">DisplayName</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHost + "/SalesforceDemo/MoveMap?metreosSessionId=" + routingGuid + "&amp;value=right"</ap>
        <ap name="TouchAreaX1" type="literal">281</ap>
        <ap name="TouchAreaX2" type="literal">297</ap>
        <ap name="TouchAreaY1" type="literal">76</ap>
        <ap name="TouchAreaY2" type="literal">92</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633084244845316326" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="165.417969" y="422">
      <linkto id="633084244845316327" type="Labeled" style="Vector" label="true" />
      <linkto id="633083925444684612" type="Labeled" style="Bezier" label="false" />
      <Properties language="csharp">
public static string Execute(string g_lat, string g_long, string g_currentLat, string g_currentLong, string g_currentTopLeftLat, string g_currentTopLeftLong, string g_currentBottomRightLat, string g_currentBottomRightLong, ref int callerLeft, ref int callerTop, LogWriter log)
{
	double callerLat = Convert.ToDouble(g_lat);
	double callerLong = Convert.ToDouble(g_long);

	double latitude = Convert.ToDouble(g_currentLat);
	double longitude = Convert.ToDouble(g_currentLong);

	// Do some math to figure out offset amount.
	double leftLong = Convert.ToDouble(g_currentTopLeftLong);
	double rightLong = Convert.ToDouble(g_currentBottomRightLong);
	double longWidth = rightLong - leftLong;

	double topLat = Convert.ToDouble(g_currentTopLeftLat);
	double bottomLat = Convert.ToDouble(g_currentBottomRightLat);

	double latHeight = topLat - bottomLat;

	double normalizedLong = (rightLong - callerLong) / longWidth;
	
	bool onscreen = true;

	if(normalizedLong &gt; 0d &amp;&amp; normalizedLong &lt; 1.0d)
	{
		callerLeft = (int)(298d * normalizedLong);
	}
	else
	{
		onscreen = false;
	}

	double normalizedLat = (topLat - callerLat) / latHeight;
	
	if(normalizedLat &gt; 0d &amp;&amp; normalizedLat &lt; 1.0d)
	{
		callerTop = (int)(168d * normalizedLat);
	}
	else
	{
		onscreen = false;
	}

	log.Write(TraceLevel.Verbose, String.Format(@"
callerLat {0}
callerLong {1}
mapLat {2}
mapLong {3}
leftLong {4}
rightLong {5}
longWidth {6}
topLat {7}
bottomLat {8}
latHeight {9}
normalizedLong {10}
normalizedLat {11}
callerLeft {12}
callerTop {13}
oncreen {14}",
callerLat,
callerLong,
latitude,
longitude,
leftLong,
rightLong,
longWidth,
topLat,
bottomLat,
latHeight,
normalizedLong,
normalizedLat,
callerLeft,
callerTop,
onscreen));
 

	return onscreen.ToString();
}
</Properties>
    </node>
    <node type="Action" id="633084244845316327" name="AddBorderedTextRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="157.417969" y="567">
      <linkto id="633084517770577917" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Text" type="csharp">g_firstName + " " +  g_lastName</ap>
        <ap name="Font" type="literal">Arial</ap>
        <ap name="FontSize" type="literal">10</ap>
        <ap name="Top" type="variable">callerTop</ap>
        <ap name="Left" type="variable">callerLeft</ap>
        <ap name="RoundedCorners" type="literal">true</ap>
        <ap name="HorizontalPadding" type="literal">3</ap>
        <ap name="VerticalPadding" type="literal">3</ap>
        <ap name="TextColor" type="csharp">Color.Black</ap>
        <ap name="BorderColor" type="csharp">Color.Black</ap>
        <ap name="FillColor" type="csharp">Color.Yellow</ap>
        <ap name="BorderWidth" type="csharp">1</ap>
        <rd field="Image">imageBuilderHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633084517770577913" name="AddBorderedTextRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="150.417969" y="753">
      <linkto id="633083925444684612" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Text" type="variable">g_resellerCompanyName</ap>
        <ap name="Font" type="literal">Arial</ap>
        <ap name="FontSize" type="literal">10</ap>
        <ap name="Top" type="variable">resellerTop</ap>
        <ap name="Left" type="variable">resellerLeft</ap>
        <ap name="RoundedCorners" type="literal">true</ap>
        <ap name="HorizontalPadding" type="literal">3</ap>
        <ap name="VerticalPadding" type="literal">3</ap>
        <ap name="TextColor" type="csharp">Color.Black</ap>
        <ap name="BorderColor" type="csharp">Color.Black</ap>
        <ap name="FillColor" type="csharp">Color.Yellow</ap>
        <ap name="BorderWidth" type="csharp">1</ap>
        <rd field="Image">imageBuilderHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633084517770577917" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="155" y="656">
      <linkto id="633084517770577913" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">callerLeft - 40</ap>
        <ap name="Value2" type="csharp">callerTop - 40</ap>
        <rd field="ResultData">resellerLeft</rd>
        <rd field="ResultData2">resellerTop</rd>
      </Properties>
    </node>
    <node type="Action" id="633085462900293047" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="544" y="384">
      <linkto id="633083925444684605" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">DisplayName</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHost + "/SalesforceDemo/ResellerSelected?metreosSessionId=" + routingGuid</ap>
        <ap name="TouchAreaX1" type="variable">resellerLeft</ap>
        <ap name="TouchAreaX2" type="csharp">resellerLeft + 32</ap>
        <ap name="TouchAreaY1" type="variable">resellerTop</ap>
        <ap name="TouchAreaY2" type="csharp">resellerTop + 32</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
      </Properties>
    </node>
    <node type="Variable" id="633083925444684639" name="incomingLat" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="lat" refType="reference">incomingLat</Properties>
    </node>
    <node type="Variable" id="633083925444684640" name="incomingLong" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="long" refType="reference">incomingLong</Properties>
    </node>
    <node type="Variable" id="633083925444684641" name="incomingTopLeftLat" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="topLeftLat" refType="reference">incomingTopLeftLat</Properties>
    </node>
    <node type="Variable" id="633083925444684642" name="incomingTopLeftLong" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="topLeftLong" refType="reference">incomingTopLeftLong</Properties>
    </node>
    <node type="Variable" id="633083925444684643" name="incomingBottomRightLat" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="bottomRightLat" refType="reference">incomingBottomRightLat</Properties>
    </node>
    <node type="Variable" id="633083925444684644" name="incomingBottomRightLong" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="bottomRightLong" refType="reference">incomingBottomRightLong</Properties>
    </node>
    <node type="Variable" id="633083925444684645" name="imageBuilderHandle" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Imaging.ImageBuilder" refType="reference">imageBuilderHandle</Properties>
    </node>
    <node type="Variable" id="633083925444684646" name="incomingFilename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="filename" refType="reference">incomingFilename</Properties>
    </node>
    <node type="Variable" id="633083925444684647" name="graphicFileMenuHandle" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.GraphicFileMenu" refType="reference">graphicFileMenuHandle</Properties>
    </node>
    <node type="Variable" id="633083925444684649" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="633084244845316331" name="callerLeft" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">callerLeft</Properties>
    </node>
    <node type="Variable" id="633084244845316332" name="callerTop" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">callerTop</Properties>
    </node>
    <node type="Variable" id="633084517770577915" name="resellerTop" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">resellerTop</Properties>
    </node>
    <node type="Variable" id="633084517770577916" name="resellerLeft" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">resellerLeft</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMoveMap_Failed" startnode="633083925444684579" treenode="633083925444684580" appnode="633083925444684577" handlerfor="633083925444684576">
    <node type="Start" id="633083925444684579" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633083925444684648" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633083925444684648" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="580" y="525">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnShowReseller_Complete" startnode="633084397433931077" treenode="633084397433931078" appnode="633084397433931075" handlerfor="633084397433931074">
    <node type="Start" id="633084397433931077" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633084397433931087" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="633084397433931087" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="215" y="81">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="csharp">"lkhdsaljfdjas" + (list[0] as String[])[9]</log>
      </Properties>
    </node>
    <node type="Variable" id="633084397433931089" name="list" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="ArrayList" initWith="Resellers" refType="reference" name="Metreos.Providers.zshakeel.ShowReseller_Complete">list</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnShowReseller_Failed" startnode="633084397433931082" treenode="633084397433931083" appnode="633084397433931080" handlerfor="633084397433931079">
    <node type="Start" id="633084397433931082" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633084397433931088" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="633084397433931088" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="345" y="260">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ResellerSelected" startnode="633084517770577906" treenode="633084517770577907" appnode="633084517770577904" handlerfor="633084517770577903">
    <node type="Start" id="633084517770577906" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633085462900292185" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="633085462900292180" name="Barge" class="MaxActionNode" group="" path="Metreos.CallControl" x="257" y="272.908875">
      <linkto id="633085462900292186" type="Labeled" style="Vector" label="Success" />
      <linkto id="633085462900292184" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="From" type="variable">g_to</ap>
        <rd field="CallId">g_bargeCallId</rd>
        <rd field="ConnectionId">g_bargeConnId</rd>
        <log condition="entry" on="true" level="Info" type="variable">g_to</log>
      </Properties>
    </node>
    <node type="Action" id="633085462900292184" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="592" y="496">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="csharp">"Failure in conference request"</log>
      </Properties>
    </node>
    <node type="Action" id="633085462900292185" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="142" y="78.9088745">
      <linkto id="633085462900292204" type="Labeled" style="Vector" label="default" />
      <linkto id="633085462900292230" type="Labeled" style="Bezier" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_barged</ap>
      </Properties>
    </node>
    <node type="Action" id="633085462900292186" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="416" y="272">
      <linkto id="633085462900292230" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <rd field="ResultData">g_barged</rd>
      </Properties>
    </node>
    <node type="Comment" id="633085462900292187" text="if already barged..." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="114" y="32" />
    <node type="Action" id="633085462900292188" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="590" y="614.9089">
      <linkto id="633085462900292189" type="Labeled" style="Vector" label="Success" />
      <linkto id="633085462900292191" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">g_deviceName</ap>
        <rd field="ResultData">deviceIpResults</rd>
      </Properties>
    </node>
    <node type="Action" id="633085462900292189" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="750" y="614.9089">
      <linkto id="633085462900292190" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">deviceIpResults.Rows.Count == 0 ? String.Empty : deviceIpResults.Rows[0]["IP"] as string</ap>
        <rd field="ResultData">deviceIp</rd>
      </Properties>
    </node>
    <node type="Action" id="633085462900292190" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="854" y="614.9089">
      <linkto id="633085462900292192" type="Labeled" style="Vector" label="default" />
      <linkto id="633085462900292196" type="Labeled" style="Vector" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">deviceIp != null &amp;&amp; deviceIp != String.Empty</ap>
      </Properties>
    </node>
    <node type="Label" id="633085462900292191" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="590" y="830.9089" />
    <node type="Label" id="633085462900292192" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="854" y="830.9089" />
    <node type="Label" id="633085462900292193" text="t" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="70" y="286.908875" />
    <node type="Label" id="633085462900292194" text="t" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="486" y="614.9089">
      <linkto id="633085462900292188" type="Basic" style="Vector" />
    </node>
    <node type="Label" id="633085462900292195" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="392" y="480">
      <linkto id="633085462900292184" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="633085462900292196" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1006" y="614.9089">
      <linkto id="633085462900292199" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(ref int chosenPort)
{
	System.Random rand = new System.Random();
	chosenPort = rand.Next(20480, 32000);
	if(chosenPort % 2 == 1)
	{
		chosenPort = chosenPort + 1;
	}
	return "success";
}
</Properties>
    </node>
    <node type="Action" id="633085462900292197" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1318" y="614.9089">
      <linkto id="633085462900292198" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"RTPRx:" + mediaEngineIp + ":" + chosenPort</ap>
        <ap name="URL2" type="csharp">"RTPTx:" + mediaEngineIp + ":" + mediaEnginePort</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="633085462900292198" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1478" y="614.9089">
      <linkto id="633085462900292201" type="Labeled" style="Vector" label="default" />
      <linkto id="633085462900292202" type="Labeled" style="Vector" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">deviceIp</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="633085462900292199" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="1150" y="614.9089">
      <linkto id="633085462900292197" type="Labeled" style="Vector" label="Success" />
      <linkto id="633085462900292200" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxIP" type="variable">deviceIp</ap>
        <ap name="MediaTxPort" type="variable">chosenPort</ap>
        <rd field="MediaRxIP">mediaEngineIp</rd>
        <rd field="MediaRxPort">mediaEnginePort</rd>
        <rd field="ConnectionId">g_bargeConnId</rd>
      </Properties>
    </node>
    <node type="Label" id="633085462900292200" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1150" y="822.9089" />
    <node type="Label" id="633085462900292201" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1478" y="822.9089" />
    <node type="Label" id="633085462900292202" text="j" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1606" y="614.9089" />
    <node type="Label" id="633085462900292203" text="j" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="318" y="206.908875">
      <linkto id="633085462900292186" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633085462900292204" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="70" y="142.908875">
      <linkto id="633085462900292193" type="Labeled" style="Vector" label="default" />
      <linkto id="633085462900292180" type="Labeled" style="Vector" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_useBarge</ap>
      </Properties>
    </node>
    <node type="Action" id="633085462900292230" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="456" y="256" mx="522" my="272">
      <items count="2">
        <item text="OnMakeCall_Complete" treenode="633074067383281821" />
        <item text="OnMakeCall_Failed" treenode="633074067383281826" />
      </items>
      <linkto id="633085462900292184" type="Labeled" style="Bezier" label="default" />
      <linkto id="633085462900293086" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_resellerPhoneNumber</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_bargeConfId</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
        <rd field="CallId">g_bargeCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="633085462900293065" name="CreateGraphicFileMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="696" y="272">
      <linkto id="633085462900293068" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Caller Location</ap>
        <ap name="Prompt" type="literal">Click Any Item</ap>
        <ap name="LocationX" type="literal">-1</ap>
        <ap name="LocationY" type="literal">-1</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHostname + "/mceadmin/images/newMap.png"</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"http://" + g_showMapHost + "/mceadmin/images/newMap.png"</log>
      </Properties>
    </node>
    <node type="Action" id="633085462900293066" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1312" y="192">
      <linkto id="633085462900293067" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">graphicFileMenuHandle.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="633085462900293067" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1472" y="272">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633085462900293068" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="824" y="272">
      <linkto id="633085462900293069" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">DisplayName</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHost + "/SalesforceDemo/MoveMap?metreosSessionId=" + routingGuid + "&amp;value=up"</ap>
        <ap name="TouchAreaX1" type="literal">140</ap>
        <ap name="TouchAreaX2" type="literal">156</ap>
        <ap name="TouchAreaY1" type="literal">1</ap>
        <ap name="TouchAreaY2" type="literal">17</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633085462900293069" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="960" y="288">
      <linkto id="633085462900293070" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">DisplayName</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHost + "/SalesforceDemo/MoveMap?metreosSessionId=" + routingGuid + "&amp;value=down"</ap>
        <ap name="TouchAreaX1" type="literal">140</ap>
        <ap name="TouchAreaX2" type="literal">156</ap>
        <ap name="TouchAreaY1" type="literal">151</ap>
        <ap name="TouchAreaY2" type="literal">167</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633085462900293070" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1080" y="288">
      <linkto id="633085462900293071" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="Off">
        <ap name="Name" type="literal">DisplayName</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHost + "/SalesforceDemo/MoveMap?metreosSessionId=" + routingGuid + "&amp;value=left"</ap>
        <ap name="TouchAreaX1" type="literal">0</ap>
        <ap name="TouchAreaX2" type="literal">16</ap>
        <ap name="TouchAreaY1" type="literal">76</ap>
        <ap name="TouchAreaY2" type="literal">92</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633085462900293071" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1208" y="280">
      <linkto id="633085462900293072" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">DisplayName</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHost + "/SalesforceDemo/MoveMap?metreosSessionId=" + routingGuid + "&amp;value=right"</ap>
        <ap name="TouchAreaX1" type="literal">281</ap>
        <ap name="TouchAreaX2" type="literal">297</ap>
        <ap name="TouchAreaY1" type="literal">76</ap>
        <ap name="TouchAreaY2" type="literal">92</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633085462900293072" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1304" y="272">
      <linkto id="633085462900293066" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">DisplayName</ap>
        <ap name="URL" type="csharp">"http://" + g_showMapHost + "/SalesforceDemo/ResellerSelected?metreosSessionId=" + routingGuid</ap>
        <ap name="TouchAreaX1" type="variable">resellerLeft</ap>
        <ap name="TouchAreaX2" type="csharp">resellerLeft + 32</ap>
        <ap name="TouchAreaY1" type="variable">resellerTop</ap>
        <ap name="TouchAreaY2" type="csharp">resellerTop + 32</ap>
        <rd field="ResultData">graphicFileMenuHandle</rd>
      </Properties>
    </node>
    <node type="Action" id="633085462900293086" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="576" y="168">
      <linkto id="633085462900293087" type="Labeled" style="Bezier" label="default" />
      <Properties language="csharp">
public static string Execute(string g_lat, string g_long, string g_currentLat, string g_currentLong, string g_currentTopLeftLat, string g_currentTopLeftLong, string g_currentBottomRightLat, string g_currentBottomRightLong, ref int callerLeft, ref int callerTop, LogWriter log)
{
	double callerLat = Convert.ToDouble(g_lat);
	double callerLong = Convert.ToDouble(g_long);

	double latitude = Convert.ToDouble(g_currentLat);
	double longitude = Convert.ToDouble(g_currentLong);

	// Do some math to figure out offset amount.
	double leftLong = Convert.ToDouble(g_currentTopLeftLong);
	double rightLong = Convert.ToDouble(g_currentBottomRightLong);
	double longWidth = rightLong - leftLong;

	double topLat = Convert.ToDouble(g_currentTopLeftLat);
	double bottomLat = Convert.ToDouble(g_currentBottomRightLat);

	double latHeight = topLat - bottomLat;

	double normalizedLong = (rightLong - callerLong) / longWidth;
	
	bool onscreen = true;

	if(normalizedLong &gt; 0d &amp;&amp; normalizedLong &lt; 1.0d)
	{
		callerLeft = (int)(298d * normalizedLong);
	}
	else
	{
		onscreen = false;
	}

	double normalizedLat = (topLat - callerLat) / latHeight;
	
	if(normalizedLat &gt; 0d &amp;&amp; normalizedLat &lt; 1.0d)
	{
		callerTop = (int)(168d * normalizedLat);
	}
	else
	{
		onscreen = false;
	}

	log.Write(TraceLevel.Verbose, String.Format(@"
callerLat {0}
callerLong {1}
mapLat {2}
mapLong {3}
leftLong {4}
rightLong {5}
longWidth {6}
topLat {7}
bottomLat {8}
latHeight {9}
normalizedLong {10}
normalizedLat {11}
callerLeft {12}
callerTop {13}
oncreen {14}",
callerLat,
callerLong,
latitude,
longitude,
leftLong,
rightLong,
longWidth,
topLat,
bottomLat,
latHeight,
normalizedLong,
normalizedLat,
callerLeft,
callerTop,
onscreen));
 

	return onscreen.ToString();
}
</Properties>
    </node>
    <node type="Action" id="633085462900293087" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="680" y="168">
      <linkto id="633085462900293065" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">callerLeft - 40</ap>
        <ap name="Value2" type="csharp">callerTop - 40</ap>
        <rd field="ResultData">resellerLeft</rd>
        <rd field="ResultData2">resellerTop</rd>
      </Properties>
    </node>
    <node type="Variable" id="633085462900292632" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
    <node type="Variable" id="633085462900292633" name="outboundCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">outboundCallId</Properties>
    </node>
    <node type="Variable" id="633085462900292634" name="deviceIpResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">deviceIpResults</Properties>
    </node>
    <node type="Variable" id="633085462900292635" name="deviceIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">deviceIp</Properties>
    </node>
    <node type="Variable" id="633085462900292636" name="chosenPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">chosenPort</Properties>
    </node>
    <node type="Variable" id="633085462900292637" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="633085462900292638" name="mediaEngineIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">mediaEngineIp</Properties>
    </node>
    <node type="Variable" id="633085462900292639" name="mediaEnginePort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">mediaEnginePort</Properties>
    </node>
    <node type="Variable" id="633085462900293081" name="graphicFileMenuHandle" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.GraphicFileMenu" refType="reference">graphicFileMenuHandle</Properties>
    </node>
    <node type="Variable" id="633085462900293082" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="633085462900293083" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="633085462900293084" name="resellerLeft" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">resellerLeft</Properties>
    </node>
    <node type="Variable" id="633085462900293085" name="resellerTop" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">resellerTop</Properties>
    </node>
    <node type="Variable" id="633085462900293090" name="callerLeft" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">callerLeft</Properties>
    </node>
    <node type="Variable" id="633085462900293091" name="callerTop" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">callerTop</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>