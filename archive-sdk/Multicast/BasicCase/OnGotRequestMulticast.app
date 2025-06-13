<Application name="OnGotRequestMulticast" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="OnGotRequestMulticast">
    <outline>
      <treenode type="evh" id="633117973469848845" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="633117973469848842" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633117973469848841" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/playmulticast</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="633117973469848850" level="2" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="633117973469848847" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="633117973469848846" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="633118112722568770" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="633118112722568767" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="633118112722568766" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="633149975799085791" actid="633118112722568776" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633118112722568775" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="633118112722568772" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="633118112722568771" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="633149975799085792" actid="633118112722568776" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_RemoteHost" id="633149975799085783" vid="633131217613047091">
        <Properties type="String">g_RemoteHost</Properties>
      </treenode>
      <treenode text="g_ErrorCode" id="633149975799085785" vid="633131217613047093">
        <Properties type="String">g_ErrorCode</Properties>
      </treenode>
      <treenode text="g_ConnectionId" id="633149975799085787" vid="633131217613047101">
        <Properties type="String">g_ConnectionId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="633117973469848844" treenode="633117973469848845" appnode="633117973469848842" handlerfor="633117973469848841">
    <node type="Start" id="633117973469848844" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="39" y="175">
      <linkto id="633131217613047095" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="633117973469848854" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="293" y="175">
      <linkto id="633131217613047080" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxFramesize" type="literal">20</ap>
        <ap name="MediaTxCodec" type="literal">G711u</ap>
        <ap name="MediaTxIP" type="literal">234.10.10.10</ap>
        <ap name="MediaTxPort" type="literal">22480</ap>
        <ap name="MediaRxCodec" type="literal">G711u</ap>
        <ap name="MediaRxFramesize" type="literal">20</ap>
        <rd field="ConnectionId">g_ConnectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="633118112722568776" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="591" y="159" mx="644" my="175">
      <items count="2">
        <item text="OnPlay_Complete" treenode="633118112722568770" />
        <item text="OnPlay_Failed" treenode="633118112722568775" />
      </items>
      <linkto id="633131217613047074" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <linkto id="633131217613047087" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">welcome.wav</ap>
        <ap name="Prompt2" type="literal">welcome.wav</ap>
        <ap name="Prompt3" type="literal">welcome.wav</ap>
        <ap name="AudioFileSampleSize" type="literal">8</ap>
        <ap name="AudioFileEncoding" type="literal">ulaw</ap>
        <ap name="ConnectionId" type="variable">g_ConnectionId</ap>
        <ap name="AudioFileSampleRate" type="literal">8</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
        <rd field="ResultCode">g_ErrorCode</rd>
      </Properties>
    </node>
    <node type="Action" id="633131217613047074" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="818.2949" y="176">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633131217613047079" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="539.419434" y="176">
      <linkto id="633118112722568776" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">Ipexec.ToString()</ap>
        <ap name="URL" type="literal">10.77.34.231</ap>
        <ap name="Username" type="literal">kartkuma</ap>
        <ap name="Password" type="literal">cisco123</ap>
      </Properties>
    </node>
    <node type="Action" id="633131217613047080" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="432.098267" y="176">
      <linkto id="633131217613047079" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">RTPMRx:234.10.10.10:22480:100</ap>
        <ap name="Priority1" type="literal">0</ap>
        <rd field="ResultData">Ipexec</rd>
      </Properties>
    </node>
    <node type="Action" id="633131217613047087" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="640.7656" y="334">
      <linkto id="633131217613047089" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="responsePhrase" type="literal">ok</ap>
        <ap name="body" type="variable">g_ErrorCode</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <ap name="remoteHost" type="variable">f_RemoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
      </Properties>
    </node>
    <node type="Action" id="633131217613047089" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="835.7656" y="334">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633131217613047095" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="146" y="175">
      <linkto id="633117973469848854" type="Labeled" style="Bevel" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">f_RemoteHost</ap>
        <rd field="ResultData">g_RemoteHost</rd>
      </Properties>
    </node>
    <node type="Variable" id="633131217613047078" name="Ipexec" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">Ipexec</Properties>
    </node>
    <node type="Variable" id="633131217613047088" name="f_RemoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">f_RemoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="633117973469848849" treenode="633117973469848850" appnode="633117973469848847" handlerfor="633117973469848846">
    <node type="Start" id="633117973469848849" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="62" y="284">
      <linkto id="633117973469848851" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633117973469848851" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="303" y="287">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="633118112722568769" treenode="633118112722568770" appnode="633118112722568767" handlerfor="633118112722568766">
    <node type="Start" id="633118112722568769" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="172">
      <linkto id="633149975799085808" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633149110368357905" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="240" y="173">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633149975799085808" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="123" y="176">
      <linkto id="633149110368357905" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_ConnectionId</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="633118112722568774" treenode="633118112722568775" appnode="633118112722568772" handlerfor="633118112722568771">
    <node type="Start" id="633118112722568774" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="148">
      <linkto id="633131217613047096" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="633131217613047096" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="182.589172" y="148">
      <linkto id="633131217613047097" type="Labeled" style="Bevel" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="responsePhrase" type="literal">ok</ap>
        <ap name="body" type="variable">g_ErrorCode</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <ap name="remoteHost" type="variable">g_RemoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
      </Properties>
    </node>
    <node type="Action" id="633131217613047097" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="377.589172" y="148">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>