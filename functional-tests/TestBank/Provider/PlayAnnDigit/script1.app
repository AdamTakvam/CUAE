<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632150611285781381" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632150611285781379" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632150611285781378" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.PlayAnnDigit.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632150611285781403" level="2" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632150611285781401" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632150611285781400" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632150611285781407" level="2" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632150611285781405" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632150611285781404" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632150839508750320" level="1" text="Event: SendDigit">
        <node type="function" name="SendDigit" id="632150839508750318" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632150839508750317" path="Metreos.Providers.FunctionalTest.Event" />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.PlayAnnDigit.script1.E_SendDigit</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="connectionId1" id="632224881663906481" vid="632150611285781382">
        <Properties type="Metreos.Types.Int">connectionId1</Properties>
      </treenode>
      <treenode text="connectionId2" id="632224881663906483" vid="632150611285781384">
        <Properties type="Metreos.Types.Int">connectionId2</Properties>
      </treenode>
      <treenode text="mediaIp1" id="632224881663906485" vid="632150611285781386">
        <Properties type="Metreos.Types.String">mediaIp1</Properties>
      </treenode>
      <treenode text="mediaIp2" id="632224881663906487" vid="632150611285781388">
        <Properties type="Metreos.Types.String">mediaIp2</Properties>
      </treenode>
      <treenode text="mediaPort1" id="632224881663906489" vid="632150611285781390">
        <Properties type="Metreos.Types.Int">mediaPort1</Properties>
      </treenode>
      <treenode text="mediaPort2" id="632224881663906491" vid="632150611285781392">
        <Properties type="Metreos.Types.Int">mediaPort2</Properties>
      </treenode>
      <treenode text="S_FailureToStart" id="632224881663906493" vid="632150611285781398">
        <Properties type="Metreos.Types.String" initWith="S_FailureToStart">S_FailureToStart</Properties>
      </treenode>
      <treenode text="S_Started" id="632224881663906495" vid="632150611285781412">
        <Properties type="Metreos.Types.String" initWith="S_Started">S_Started</Properties>
      </treenode>
      <treenode text="S_PlayFailed" id="632224881663906497" vid="632150611285781418">
        <Properties type="Metreos.Types.String" initWith="S_PlayFailed">S_PlayFailed</Properties>
      </treenode>
      <treenode text="S_PlaySuccess" id="632224881663906499" vid="632150611285781422">
        <Properties type="Metreos.Types.String" initWith="S_PlaySuccess">S_PlaySuccess</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" varsy="692" startnode="632150611285781380" treenode="632150611285781381" appnode="632150611285781379" handlerfor="632150611285781378">
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
        <item text="OnPlayAnnouncement_Complete" treenode="632150611285781403" />
        <item text="OnPlayAnnouncement_Failed" treenode="632150611285781407" />
      </items>
      <linkto id="632150611285781416" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632150611285781578" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondDigit" type="literal">4</ap>
        <ap name="connectionId" type="variable">connectionId1</ap>
        <ap name="filename" type="literal">oneMinuteNoisy.wav</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Comment" id="632150611285781410" text="5 minute connection timeout" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="188" y="114" />
    <node type="Comment" id="632150611285781411" text="60 second play,&#xD;&#xA;digit of '4' will stop play." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="399" y="109" />
    <node type="Action" id="632150611285781415" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="439" y="410">
      <linkto id="632224881663906534" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_FailureToStart</ap>
      </Properties>
    </node>
    <node type="Action" id="632150611285781416" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="696" y="221">
      <linkto id="632224881663906535" type="Labeled" style="Bezier" ortho="true" label="default" />
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
      <linkto id="632150839508750163" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632150611285781408" type="Labeled" style="Bezier" ortho="true" label="success" />
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
    <node type="Action" id="632224881663906534" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="427" y="575">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632224881663906535" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="842.7611" y="235">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Complete" varsy="667" startnode="632150611285781402" treenode="632150611285781403" appnode="632150611285781401" handlerfor="632150611285781400">
    <node type="Start" id="632150611285781402" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632150611285781426" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150611285781426" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="175" y="179">
      <linkto id="632151253429844040" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId1</ap>
        <rd field="resultCode">resultCode</rd>
      </Properties>
    </node>
    <node type="Variable" id="632150611285781428" name="resultCode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="685">
      <Properties type="Metreos.Types.String" refType="reference">resultCode</Properties>
    </node>
    <node type="Action" id="632150611285781429" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="484" y="176">
      <linkto id="632224881663906536" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="resultCode" type="variable">resultCode</ap>
        <ap name="signalName" type="variable">S_PlaySuccess</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429844040" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="304" y="178">
      <linkto id="632150611285781429" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId2</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663906536" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="699" y="176">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Failed" varsy="692" startnode="632150611285781406" treenode="632150611285781407" appnode="632150611285781405" handlerfor="632150611285781404">
    <node type="Start" id="632150611285781406" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632150611285781579" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150611285781420" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="394" y="178">
      <linkto id="632224881663906532" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_PlayFailed</ap>
      </Properties>
    </node>
    <node type="Action" id="632150611285781579" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="172" y="180">
      <linkto id="632151253429844041" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429844041" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="285" y="179">
      <linkto id="632150611285781420" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId2</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663906532" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="533" y="174">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="SendDigit" varsy="692" startnode="632150839508750319" treenode="632150839508750320" appnode="632150839508750318" handlerfor="632150839508750317">
    <node type="Start" id="632150839508750319" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632150839508750321" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150839508750321" name="SendDigits" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="228" y="116">
      <linkto id="632224881663906533" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="digits" type="literal">4</ap>
        <ap name="connectionId" type="variable">connectionId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663906533" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="636" y="199">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
