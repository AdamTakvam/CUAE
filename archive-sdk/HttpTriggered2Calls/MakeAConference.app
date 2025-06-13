<Application name="MakeAConference" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="MakeAConference">
    <outline>
      <treenode type="evh" id="632636666388758000" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632636666388757997" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632636666388757996" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/MakeConf</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632636666388758021" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632636666388758018" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632636666388758017" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="633111372405823805" actid="632636666388758032" />
          <ref id="633111372405823832" actid="632636666388758064" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632636666388758026" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632636666388758023" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632636666388758022" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="633111372405823806" actid="632636666388758032" />
          <ref id="633111372405823833" actid="632636666388758064" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632636666388758031" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632636666388758028" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632636666388758027" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="633111372405823807" actid="632636666388758032" />
          <ref id="633111372405823834" actid="632636666388758064" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632636666388758045" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632636666388758042" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632636666388758041" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="633111372405823813" actid="632636666388758051" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632636666388758050" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632636666388758047" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632636666388758046" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="633111372405823814" actid="632636666388758051" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_toCall1" id="633111372405823792" vid="632636666388758009">
        <Properties type="String" initWith="toCall1">g_toCall1</Properties>
      </treenode>
      <treenode text="g_toCall2" id="633111372405823794" vid="632636666388758011">
        <Properties type="String" initWith="toCall2">g_toCall2</Properties>
      </treenode>
      <treenode text="g_callId1" id="633111372405823796" vid="632636666388758013">
        <Properties type="String">g_callId1</Properties>
      </treenode>
      <treenode text="g_callId2" id="633111372405823798" vid="632636666388758015">
        <Properties type="String">g_callId2</Properties>
      </treenode>
      <treenode text="g_connectionId1" id="633111372405823800" vid="632636666388758068">
        <Properties type="String">g_connectionId1</Properties>
      </treenode>
      <treenode text="g_conferenceId1" id="633111372405823802" vid="632636666388758070">
        <Properties type="String">g_conferenceId1</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632636666388757999" treenode="632636666388758000" appnode="632636666388757997" handlerfor="632636666388757996">
    <node type="Start" id="632636666388757999" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="309">
      <linkto id="632636666388758032" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632636666388758032" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="266" y="290" mx="332" my="306">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632636666388758021" />
        <item text="OnMakeCall_Failed" treenode="632636666388758026" />
        <item text="OnRemoteHangup" treenode="632636666388758031" />
      </items>
      <linkto id="632636666388758038" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_toCall1</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="literal">0</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_callId1</rd>
      </Properties>
    </node>
    <node type="Action" id="632636666388758038" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="584" y="304">
      <linkto id="632636666388758039" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="literal">Call Initiated</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632636666388758039" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="840" y="312">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632636666388758037" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632636666388758020" treenode="632636666388758021" appnode="632636666388758018" handlerfor="632636666388758017">
    <node type="Start" id="632636666388758020" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="328">
      <linkto id="632636666388758075" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632636666388758051" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="392" y="312" mx="445" my="328">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632636666388758045" />
        <item text="OnPlay_Failed" treenode="632636666388758050" />
      </items>
      <linkto id="632636666388758054" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">welcome.wav</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632636666388758054" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="752" y="336">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632636666388758073" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="232" y="328">
      <linkto id="632636666388758051" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">conferenceId</ap>
        <rd field="ResultData">g_conferenceId1</rd>
      </Properties>
    </node>
    <node type="Action" id="632636666388758075" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="128" y="328">
      <linkto id="632636666388758073" type="Labeled" style="Bezier" label="true" />
      <linkto id="632636666388758077" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">callId == g_callId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632636666388758077" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="124" y="540">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632636666388758040" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connectionId</Properties>
    </node>
    <node type="Variable" id="632636666388758072" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConferenceId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">conferenceId</Properties>
    </node>
    <node type="Variable" id="632636666388758076" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632636666388758025" treenode="632636666388758026" appnode="632636666388758023" handlerfor="632636666388758022">
    <node type="Start" id="632636666388758025" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632636666388758062" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632636666388758062" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="585" y="358">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632636666388758030" treenode="632636666388758031" appnode="632636666388758028" handlerfor="632636666388758027">
    <node type="Start" id="632636666388758030" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="320">
      <linkto id="632636666388758079" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632636666388758079" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="224" y="328">
      <linkto id="632636666388758080" type="Labeled" style="Bezier" label="true" />
      <linkto id="632636666388758081" type="Labeled" style="Bezier" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">callId == g_callId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632636666388758080" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="440" y="248">
      <linkto id="632636666388758083" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId2</ap>
      </Properties>
    </node>
    <node type="Action" id="632636666388758081" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="432" y="448">
      <linkto id="632636666388758083" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632636666388758083" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="688" y="344">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632636666388758078" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" activetab="true" startnode="632636666388758044" treenode="632636666388758045" appnode="632636666388758042" handlerfor="632636666388758041">
    <node type="Start" id="632636666388758044" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="312">
      <linkto id="632636666388758064" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632636666388758064" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="240" y="280" mx="306" my="296">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632636666388758021" />
        <item text="OnMakeCall_Failed" treenode="632636666388758026" />
        <item text="OnRemoteHangup" treenode="632636666388758031" />
      </items>
      <linkto id="632636666388758074" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_toCall2</ap>
        <ap name="ProxyDTMFCallId" type="variable">g_callId1</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId1</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_callId2</rd>
      </Properties>
    </node>
    <node type="Action" id="632636666388758074" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="720" y="327">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632636666388758049" treenode="632636666388758050" appnode="632636666388758047" handlerfor="632636666388758046">
    <node type="Start" id="632636666388758049" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632636666388758061" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632636666388758061" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="803" y="359">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>