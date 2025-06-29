<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632367333784687760" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632150611285781379" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632150611285781378" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.PlayAnn.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632367333784687761" level="2" text="Metreos.Providers.MediaServer.PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632150611285781401" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632150611285781400" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <references>
          <ref id="632367333784687762" actid="632150611285781408" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632367333784687763" level="2" text="Metreos.Providers.MediaServer.PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632150611285781405" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632150611285781404" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <references>
          <ref id="632367333784687764" actid="632150611285781408" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="connectionId1" id="632367333784687717" vid="632150611285781382">
        <Properties type="Metreos.Types.Int">connectionId1</Properties>
      </treenode>
      <treenode text="connectionId2" id="632367333784687719" vid="632150611285781384">
        <Properties type="Metreos.Types.Int">connectionId2</Properties>
      </treenode>
      <treenode text="mediaIp1" id="632367333784687721" vid="632150611285781386">
        <Properties type="Metreos.Types.String">mediaIp1</Properties>
      </treenode>
      <treenode text="mediaIp2" id="632367333784687723" vid="632150611285781388">
        <Properties type="Metreos.Types.String">mediaIp2</Properties>
      </treenode>
      <treenode text="mediaPort1" id="632367333784687725" vid="632150611285781390">
        <Properties type="Metreos.Types.Int">mediaPort1</Properties>
      </treenode>
      <treenode text="mediaPort2" id="632367333784687727" vid="632150611285781392">
        <Properties type="Metreos.Types.Int">mediaPort2</Properties>
      </treenode>
      <treenode text="S_FailureToStart" id="632367333784687729" vid="632150611285781398">
        <Properties type="Metreos.Types.String" initWith="S_FailureToStart">S_FailureToStart</Properties>
      </treenode>
      <treenode text="S_Started" id="632367333784687731" vid="632150611285781412">
        <Properties type="Metreos.Types.String" initWith="S_Started">S_Started</Properties>
      </treenode>
      <treenode text="S_PlayFailed" id="632367333784687733" vid="632150611285781418">
        <Properties type="Metreos.Types.String" initWith="S_PlayFailed">S_PlayFailed</Properties>
      </treenode>
      <treenode text="S_PlaySuccess" id="632367333784687735" vid="632150611285781422">
        <Properties type="Metreos.Types.String" initWith="S_PlaySuccess">S_PlaySuccess</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632150611285781380" treenode="632150611285781381" appnode="632150611285781379" handlerfor="632150611285781378">
    <node type="Start" id="632150611285781380" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="224">
      <linkto id="632150611285781394" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150611285781394" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="140" y="224">
      <linkto id="632150611285781415" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632150839508750161" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="literal">0</ap>
        <ap name="connectionId" type="literal">0</ap>
        <ap name="remoteIp" type="literal">0</ap>
        <rd field="connectionId">connectionId1</rd>
        <rd field="port">mediaPort1</rd>
        <rd field="ipAddress">mediaIp1</rd>
      </Properties>
    </node>
    <node type="Action" id="632150611285781408" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="495" y="205" mx="588" my="221">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632367333784687761" />
        <item text="OnPlayAnnouncement_Failed" treenode="632367333784687763" />
      </items>
      <linkto id="632150611285781416" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632150611285781578" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxTime" type="literal">300000</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="connectionId" type="variable">connectionId1</ap>
        <ap name="filename" type="literal">oneMinuteNoisy.wav</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Comment" id="632150611285781410" text="5 minute connection timeout" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="188" y="114" />
    <node type="Comment" id="632150611285781411" text="60 second play,&#xD;&#xA;30 second silence term" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="399" y="109" />
    <node type="Action" id="632150611285781415" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="418" y="428">
      <linkto id="632224881663906459" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_FailureToStart</ap>
      </Properties>
    </node>
    <node type="Action" id="632150611285781416" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="696" y="221">
      <linkto id="632224881663906461" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Started</ap>
      </Properties>
    </node>
    <node type="Action" id="632150611285781578" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="570" y="409">
      <linkto id="632150611285781415" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632150839508750161" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="294" y="224">
      <linkto id="632150839508750162" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632150611285781578" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="variable">mediaPort1</ap>
        <ap name="connectionId" type="literal">0</ap>
        <ap name="remoteIp" type="variable">mediaIp1</ap>
        <rd field="connectionId">connectionId2</rd>
        <rd field="port">mediaPort2</rd>
        <rd field="ipAddress">mediaIp2</rd>
      </Properties>
    </node>
    <node type="Action" id="632150839508750162" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="434" y="224">
      <linkto id="632150611285781408" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632150839508750163" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="variable">mediaPort2</ap>
        <ap name="connectionId" type="variable">connectionId1</ap>
        <ap name="remoteIp" type="variable">mediaIp2</ap>
      </Properties>
    </node>
    <node type="Action" id="632150839508750163" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="499" y="302">
      <linkto id="632150611285781578" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId2</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663906459" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="443" y="552">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632224881663906461" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="793.7611" y="225">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Complete" startnode="632150611285781402" treenode="632150611285781403" appnode="632150611285781401" handlerfor="632150611285781400">
    <node type="Start" id="632150611285781402" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632150611285781426" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150611285781426" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="226" y="176">
      <linkto id="632151253429843981" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId1</ap>
        <rd field="resultCode">resultCode</rd>
      </Properties>
    </node>
    <node type="Action" id="632150611285781429" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="484" y="176">
      <linkto id="632224881663906460" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="resultCode" type="variable">resultCode</ap>
        <ap name="signalName" type="variable">S_PlaySuccess</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429843981" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="357" y="173">
      <linkto id="632150611285781429" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId2</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663906460" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="619" y="174">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632150611285781428" name="resultCode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">resultCode</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Failed" startnode="632150611285781406" treenode="632150611285781407" appnode="632150611285781405" handlerfor="632150611285781404">
    <node type="Start" id="632150611285781406" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632150611285781579" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150611285781420" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="306" y="180">
      <linkto id="632224881663906457" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_PlayFailed</ap>
      </Properties>
    </node>
    <node type="Action" id="632150611285781579" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="110" y="183">
      <linkto id="632151253429843982" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429843982" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="218" y="181">
      <linkto id="632150611285781420" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId2</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663906457" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="506" y="187">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
