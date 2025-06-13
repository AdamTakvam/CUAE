<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="633149293255707122" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="633149293255707119" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="633149293255707118" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="633153384743586360" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="633153384743586357" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="633153384743586356" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="633153462815841346" actid="633153384743586366" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633153384743586365" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="633153384743586362" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="633153384743586361" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="633153462815841347" actid="633153384743586366" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_CallId" id="633153462815841337" vid="633149293255707124">
        <Properties type="String">g_CallId</Properties>
      </treenode>
      <treenode text="g_ConnectionId" id="633153462815841339" vid="633153384743586353">
        <Properties type="String">g_ConnectionId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="633149293255707121" treenode="633149293255707122" appnode="633149293255707119" handlerfor="633149293255707118">
    <node type="Start" id="633149293255707121" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="245">
      <linkto id="633150115837077793" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633150115837077793" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="148" y="242">
      <linkto id="633150115837077794" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="633153384743586376" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">CallId</ap>
        <ap name="WaitForMedia" type="literal">Tx</ap>
        <rd field="CallId">g_CallId</rd>
        <rd field="ConnectionId">ConnId</rd>
      </Properties>
    </node>
    <node type="Action" id="633150115837077794" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="150" y="432">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633150862089111580" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="420" y="236">
      <linkto id="633150862089111582" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">RTPMRx:234.10.10.10:22480:100</ap>
        <ap name="Priority1" type="literal">0</ap>
        <rd field="ResultData">IpExec</rd>
      </Properties>
    </node>
    <node type="Action" id="633150862089111582" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="551" y="235">
      <linkto id="633153384743586366" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">IpExec.ToString()</ap>
        <ap name="URL" type="literal">10.77.34.231</ap>
        <ap name="Username" type="literal">kartkuma</ap>
        <ap name="Password" type="literal">cisco123</ap>
      </Properties>
    </node>
    <node type="Action" id="633153384743586366" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="620" y="217" mx="673" my="233">
      <items count="2">
        <item text="OnPlay_Complete" treenode="633153384743586360" />
        <item text="OnPlay_Failed" treenode="633153384743586365" />
      </items>
      <linkto id="633153384743586369" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="633153384743586370" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="AudioFileSampleRate" type="literal">8</ap>
        <ap name="Prompt2" type="literal">welcome.wav</ap>
        <ap name="Prompt3" type="literal">welcome.wav</ap>
        <ap name="AudioFileSampleSize" type="literal">8</ap>
        <ap name="AudioFileEncoding" type="literal">ulaw</ap>
        <ap name="Prompt1" type="literal">welcome.wav</ap>
        <ap name="ConnectionId" type="variable">g_ConnectionId</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="633153384743586369" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="823.3535" y="231">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633153384743586370" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="673" y="423">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633153384743586376" name="ModifyConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="287" y="237">
      <linkto id="633150862089111580" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">ConnId</ap>
        <ap name="CallId" type="variable">CallId</ap>
        <ap name="MediaTxIP" type="literal">234.10.10.10</ap>
        <ap name="MediaTxPort" type="literal">22480</ap>
        <ap name="MediaRxCodec" type="literal">G711u</ap>
        <ap name="MediaRxFramesize" type="literal">20</ap>
        <ap name="MediaTxCodec" type="literal">G711u</ap>
        <ap name="MediaTxFramesize" type="literal">20</ap>
        <rd field="ConnectionId">g_ConnectionId</rd>
      </Properties>
    </node>
    <node type="Variable" id="633149986211681631" name="CallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">CallId</Properties>
    </node>
    <node type="Variable" id="633150862089111581" name="IpExec" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">IpExec</Properties>
    </node>
    <node type="Variable" id="633153384743586377" name="ConnId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">ConnId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="633153384743586359" treenode="633153384743586360" appnode="633153384743586357" handlerfor="633153384743586356">
    <node type="Start" id="633153384743586359" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="87" y="268">
      <linkto id="633153384743586373" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633153384743586373" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="203" y="265">
      <linkto id="633153384743586374" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_ConnectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="633153384743586374" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="366" y="261">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="633153384743586364" treenode="633153384743586365" appnode="633153384743586362" handlerfor="633153384743586361">
    <node type="Start" id="633153384743586364" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="117" y="220">
      <linkto id="633153384743586372" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633153384743586372" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="314" y="218">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>