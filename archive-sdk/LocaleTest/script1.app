<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="633066092683750382" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="633066092683750379" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633066092683750378" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/localetest</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="633096582826719223" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="633096582826719220" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="633096582826719219" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="633096588292344183" actid="633096582826719234" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633096582826719228" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="633096582826719225" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="633096582826719224" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="633096588292344184" actid="633096582826719234" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633096582826719233" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="633096582826719230" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="633096582826719229" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="633096588292344185" actid="633096582826719234" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="633096582826719914" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="633096582826719911" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="633096582826719910" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="633096588292344196" actid="633096582826719920" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633096582826719919" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="633096582826719916" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="633096582826719915" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="633096588292344197" actid="633096582826719920" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="gtest1" id="633096588292344161" vid="633066106186250402">
        <Properties type="String" initWith="Locale.myString">gtest1</Properties>
      </treenode>
      <treenode text="gtest2" id="633096588292344163" vid="633066132041250459">
        <Properties type="String" initWith="Locale.myString">gtest2</Properties>
      </treenode>
      <treenode text="gChangeLocale" id="633096588292344165" vid="633070767784844205">
        <Properties type="String" initWith="Config.blnChangeLocale">gChangeLocale</Properties>
      </treenode>
      <treenode text="gMyLocale" id="633096588292344167" vid="633070767784844235">
        <Properties type="String" initWith="Config.mylocale">gMyLocale</Properties>
      </treenode>
      <treenode text="g_DN" id="633096588292344169" vid="633096582826719342">
        <Properties type="String" initWith="Config.DN">g_DN</Properties>
      </treenode>
      <treenode text="g_CallId" id="633096588292344171" vid="633096582826719668">
        <Properties type="String">g_CallId</Properties>
      </treenode>
      <treenode text="g_ConnId" id="633096588292344173" vid="633096582826719789">
        <Properties type="String">g_ConnId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="633066092683750381" treenode="633066092683750382" appnode="633066092683750379" handlerfor="633066092683750378">
    <node type="Start" id="633066092683750381" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="49" y="115">
      <linkto id="633066153295625407" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633066106186250392" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="748" y="157">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633066106186250416" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="477" y="229">
      <linkto id="633096582826719234" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">"Default locale ID: " + defaultLocale + ", Default locale string: " + strBuf + "\nNew locale ID: " + sessionData.Culture.IetfLanguageTag + ", new locale string: " + strBuf2</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="633066153295625407" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="134" y="114">
      <linkto id="633070767784844149" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">gtest1</ap>
        <ap name="Value2" type="csharp">sessionData.Culture.IetfLanguageTag</ap>
        <rd field="ResultData">strBuf</rd>
        <rd field="ResultData2">defaultLocale</rd>
      </Properties>
    </node>
    <node type="Action" id="633066153295625408" name="ChangeLocale" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="231" y="229">
      <linkto id="633066153295625427" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Locale" type="variable">gMyLocale</ap>
        <ap name="ResetStrings" type="literal">true</ap>
        <log condition="failure" on="true" level="Error" type="literal">changed locale failed</log>
      </Properties>
    </node>
    <node type="Action" id="633066153295625427" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="348" y="229">
      <linkto id="633066106186250416" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">gtest2</ap>
        <rd field="ResultData">strBuf2</rd>
      </Properties>
    </node>
    <node type="Action" id="633070767784844149" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="230" y="113">
      <linkto id="633066153295625408" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="633070767784844150" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">gChangeLocale</ap>
      </Properties>
    </node>
    <node type="Action" id="633070767784844150" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="469" y="108">
      <linkto id="633096582826719234" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">"Default locale ID: " + defaultLocale + ", Default locale string: " + strBuf</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="633096582826719234" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="531.4707" y="142" mx="598" my="158">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="633096582826719223" />
        <item text="OnMakeCall_Failed" treenode="633096582826719228" />
        <item text="OnRemoteHangup" treenode="633096582826719233" />
      </items>
      <linkto id="633066106186250392" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="633096582826719556" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_DN</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
        <rd field="CallId">g_CallId</rd>
      </Properties>
    </node>
    <node type="Action" id="633096582826719556" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="571.4707" y="292">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633066106186250417" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="633066106186250447" name="strBuf" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">strBuf</Properties>
    </node>
    <node type="Variable" id="633066153295625429" name="strBuf2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">strBuf2</Properties>
    </node>
    <node type="Variable" id="633071362559844151" name="defaultLocale" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="sessionData.Culture.IetfLanguageTag" refType="value">defaultLocale</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" activetab="true" startnode="633096582826719222" treenode="633096582826719223" appnode="633096582826719220" handlerfor="633096582826719219">
    <node type="Start" id="633096582826719222" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="78">
      <linkto id="633096582826719784" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633096582826719783" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="262.048828" y="236">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633096582826719784" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="148.048828" y="80">
      <linkto id="633096582826719920" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">connId</ap>
        <rd field="ResultData">g_ConnId</rd>
      </Properties>
    </node>
    <node type="Action" id="633096582826719785" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="411.048828" y="92">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633096582826719920" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="215" y="66" mx="268" my="82">
      <items count="2">
        <item text="OnPlay_Complete" treenode="633096582826719914" />
        <item text="OnPlay_Failed" treenode="633096582826719919" />
      </items>
      <linkto id="633096582826719785" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="633096582826719783" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">welcome.wav</ap>
        <ap name="ConnectionId" type="variable">g_ConnId</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Variable" id="633096582826719909" name="connId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="633096582826719227" treenode="633096582826719228" appnode="633096582826719225" handlerfor="633096582826719224">
    <node type="Start" id="633096582826719227" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="84" y="222">
      <linkto id="633096582826720055" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633096582826720055" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="311.942047" y="218">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="633096582826719232" treenode="633096582826719233" appnode="633096582826719230" handlerfor="633096582826719229">
    <node type="Start" id="633096582826719232" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="111" y="141">
      <linkto id="633096582826720191" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633096582826720190" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="420.2272" y="141">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633096582826720191" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="290.2272" y="141">
      <linkto id="633096582826720190" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_ConnId</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="633096582826719913" treenode="633096582826719914" appnode="633096582826719911" handlerfor="633096582826719910">
    <node type="Start" id="633096582826719913" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="155">
      <linkto id="633096582826720469" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633096582826720468" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="386.352844" y="150">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633096582826720469" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="245.352859" y="150">
      <linkto id="633096582826720468" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_CallId</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="633096582826719918" treenode="633096582826719919" appnode="633096582826719916" handlerfor="633096582826719915">
    <node type="Start" id="633096582826719918" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="119" y="199">
      <linkto id="633096582826720473" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633096582826720472" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="392.352844" y="208">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633096582826720473" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="251.352859" y="208">
      <linkto id="633096582826720472" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_CallId</ap>
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>