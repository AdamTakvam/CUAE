<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632150611285781381" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632150611285781379" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632150611285781378" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.RecordSilence.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632151253429843916" level="2" text="RecordAudio_Complete: OnRecordAudio_Complete">
        <node type="function" name="OnRecordAudio_Complete" id="632151253429843914" path="Metreos.StockTools" />
        <node type="event" name="RecordAudio_Complete" id="632151253429843913" path="Metreos.Providers.MediaServer.RecordAudio_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632151253429844209" level="3" text="PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632151253429844207" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632151253429844206" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632151253429844213" level="3" text="PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632151253429844211" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632151253429844210" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632151253429843920" level="2" text="RecordAudio_Failed: OnRecordAudio_Failed">
        <node type="function" name="OnRecordAudio_Failed" id="632151253429843918" path="Metreos.StockTools" />
        <node type="event" name="RecordAudio_Failed" id="632151253429843917" path="Metreos.Providers.MediaServer.RecordAudio_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632156534533437896" level="1" text="Event: SendDigit">
        <node type="function" name="SendDigit" id="632156534533437894" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632156534533437893" path="Metreos.Providers.FunctionalTest.Event" />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.RecordSilence.script1.E_SendDigit</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="connectionId1" id="632224881663907201" vid="632150611285781382">
        <Properties type="Metreos.Types.Int">connectionId1</Properties>
      </treenode>
      <treenode text="connectionId2" id="632224881663907203" vid="632150611285781384">
        <Properties type="Metreos.Types.Int">connectionId2</Properties>
      </treenode>
      <treenode text="mediaIp1" id="632224881663907205" vid="632150611285781386">
        <Properties type="Metreos.Types.String">mediaIp1</Properties>
      </treenode>
      <treenode text="mediaIp2" id="632224881663907207" vid="632150611285781388">
        <Properties type="Metreos.Types.String">mediaIp2</Properties>
      </treenode>
      <treenode text="mediaPort1" id="632224881663907209" vid="632150611285781390">
        <Properties type="Metreos.Types.Int">mediaPort1</Properties>
      </treenode>
      <treenode text="mediaPort2" id="632224881663907211" vid="632150611285781392">
        <Properties type="Metreos.Types.Int">mediaPort2</Properties>
      </treenode>
      <treenode text="S_FailureToStart" id="632224881663907213" vid="632150611285781398">
        <Properties type="Metreos.Types.String" initWith="S_FailureToStart">S_FailureToStart</Properties>
      </treenode>
      <treenode text="S_Started" id="632224881663907215" vid="632150611285781412">
        <Properties type="Metreos.Types.String" initWith="S_Started">S_Started</Properties>
      </treenode>
      <treenode text="S_RecordFailed" id="632224881663907217" vid="632150611285781418">
        <Properties type="Metreos.Types.String" initWith="S_RecordFailed">S_RecordFailed</Properties>
      </treenode>
      <treenode text="S_RecordSuccess" id="632224881663907219" vid="632150611285781422">
        <Properties type="Metreos.Types.String" initWith="S_RecordSuccess">S_RecordSuccess</Properties>
      </treenode>
      <treenode text="firstPlayDone" id="632224881663907221" vid="632151253429844218">
        <Properties type="Metreos.Types.Bool">firstPlayDone</Properties>
      </treenode>
      <treenode text="S_FirstPlayDone" id="632224881663907223" vid="632151253429844334">
        <Properties type="Metreos.Types.String" initWith="S_FirstPlayDone">S_FirstPlayDone</Properties>
      </treenode>
      <treenode text="conferenceId" id="632224881663907225" vid="632155560786094080">
        <Properties type="Metreos.Types.String">conferenceId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" varsy="692" startnode="632150611285781380" treenode="632150611285781381" appnode="632150611285781379" handlerfor="632150611285781378">
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
    <node type="Comment" id="632150611285781410" text="5 minute connection timeout" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="188" y="114" />
    <node type="Comment" id="632150611285781411" text="60 second record,&#xD;&#xA;120 second silence term" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="399" y="109" />
    <node type="Action" id="632150611285781415" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="439" y="410">
      <linkto id="632224881663907289" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_FailureToStart</ap>
      </Properties>
    </node>
    <node type="Action" id="632150611285781416" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="1225" y="225">
      <linkto id="632224881663907291" type="Labeled" style="Bezier" ortho="true" label="default" />
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
      <linkto id="632155560786094078" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="variable">mediaPort2</ap>
        <ap name="connectionId" type="variable">connectionId1</ap>
        <ap name="remoteIp" type="variable">mediaIp2</ap>
      </Properties>
    </node>
    <node type="Action" id="632150839508750163" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="775" y="412">
      <linkto id="632150611285781578" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId2</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429843921" name="RecordAudio" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="982" y="212" mx="1058" my="228">
      <items count="2">
        <item text="OnRecordAudio_Complete" treenode="632151253429843916" />
        <item text="OnRecordAudio_Failed" treenode="632151253429843920" />
      </items>
      <linkto id="632150839508750163" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632150611285781416" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="termCondSilence" type="literal">20000</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="connectionId" type="variable">connectionId1</ap>
        <ap name="filename" type="literal">temp.wav</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632155560786094078" name="CreateConnectionConference" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="554" y="222">
      <linkto id="632155560786094079" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632150839508750163" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId1</ap>
        <ap name="conferenceId" type="literal">0</ap>
        <rd field="conferenceId">conferenceId</rd>
      </Properties>
    </node>
    <node type="Action" id="632155560786094079" name="CreateConnectionConference" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="844" y="218">
      <linkto id="632151253429843921" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632150839508750163" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId2</ap>
        <ap name="conferenceId" type="variable">conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907289" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="432" y="582">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632224881663907290" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1518.761" y="225">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632224881663907291" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1359.761" y="219">
      <linkto id="632224881663907290" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">	public static string Execute(bool firstPlayDone)
	{
firstPlayDone = false;
return String.Empty;
	}</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecordAudio_Complete" varsy="692" startnode="632151253429843915" treenode="632151253429843916" appnode="632151253429843914" handlerfor="632151253429843913">
    <node type="Start" id="632151253429843915" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632151253429844237" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632151253429843922" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="255" y="397">
      <linkto id="632151253429843925" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429843925" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="411" y="396">
      <linkto id="632151253429844242" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId2</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429844214" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="236" y="202" mx="329" my="218">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632151253429844209" />
        <item text="OnPlayAnnouncement_Failed" treenode="632151253429844213" />
      </items>
      <linkto id="632151253429844238" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632151253429844336" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId1</ap>
        <ap name="filename" type="literal">temp.wav</ap>
        <ap name="audioFileFormat" type="literal">wav</ap>
        <ap name="userData" type="literal">second</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429844237" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="103" y="223">
      <linkto id="632151253429844214" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="value1" type="literal">true</ap>
        <ap name="value2" type="csharp">firstPlayDone.ToString().ToLower()</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429844238" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="103" y="320">
      <linkto id="632151253429844239" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429844239" name="Sleep" class="MaxActionNode" group="" path="Metreos.Native.ApplicationControl" x="104" y="398">
      <linkto id="632151253429843922" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="timeout" type="literal">500</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429844242" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="532" y="394">
      <linkto id="632224881663907293" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_RecordFailed</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429844336" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="519" y="218">
      <linkto id="632224881663907292" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_FirstPlayDone</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907292" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="675" y="211">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632224881663907293" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="658" y="392">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Complete" varsy="667" startnode="632151253429844208" treenode="632151253429844209" appnode="632151253429844207" handlerfor="632151253429844206">
    <node type="Start" id="632151253429844208" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="38" y="174">
      <linkto id="632151253429844223" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632151253429844223" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="381" y="183">
      <linkto id="632151253429844225" type="Labeled" style="Bezier" ortho="true" label="second" />
      <linkto id="632224881663907296" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="switchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Variable" id="632151253429844224" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="685">
      <Properties type="Metreos.Types.String" initWith="userData" refType="reference">userData</Properties>
    </node>
    <node type="Action" id="632151253429844225" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="396" y="328">
      <linkto id="632151253429844226" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429844226" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="538" y="327">
      <linkto id="632151253429844245" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId2</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429844245" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="634.7611" y="329">
      <linkto id="632224881663907295" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_RecordSuccess</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907294" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="734.7611" y="171">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632224881663907295" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="726.7611" y="326">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632224881663907296" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="575.7611" y="179">
      <linkto id="632224881663907294" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">	public static string Execute(bool firstPlayDone)
	{
firstPlayDone = true;
return String.Empty;
	}</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Failed" varsy="667" startnode="632151253429844212" treenode="632151253429844213" appnode="632151253429844211" handlerfor="632151253429844210">
    <node type="Start" id="632151253429844212" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632151253429844229" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632151253429844229" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="207" y="165">
      <linkto id="632151253429844232" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="switchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Variable" id="632151253429844230" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="685">
      <Properties type="Metreos.Types.String" initWith="userData" refType="reference">userData</Properties>
    </node>
    <node type="Action" id="632151253429844232" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="329" y="165">
      <linkto id="632151253429844233" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429844233" name="Sleep" class="MaxActionNode" group="" path="Metreos.Native.ApplicationControl" x="474" y="167">
      <linkto id="632151253429844234" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="timeout" type="literal">500</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429844234" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="590" y="165">
      <linkto id="632151253429844235" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429844235" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="739" y="166">
      <linkto id="632151253429844246" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId2</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429844246" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="829.0762" y="167">
      <linkto id="632224881663907297" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_RecordFailed</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907297" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="917.0762" y="169">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecordAudio_Failed" varsy="692" startnode="632151253429843919" treenode="632151253429843920" appnode="632151253429843918" handlerfor="632151253429843917">
    <node type="Start" id="632151253429843919" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632151253429843926" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632151253429843926" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="146" y="170">
      <linkto id="632151253429843927" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429843927" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="324" y="166">
      <linkto id="632151253429843928" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId2</ap>
      </Properties>
    </node>
    <node type="Action" id="632151253429843928" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="466" y="164">
      <linkto id="632224881663907298" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_RecordFailed</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907298" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="651" y="171">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="SendDigit" activetab="true" varsy="692" startnode="632156534533437895" treenode="632156534533437896" appnode="632156534533437894" handlerfor="632156534533437893">
    <node type="Start" id="632156534533437895" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632156534533437897" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632156534533437897" name="SendDigits" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="274" y="188">
      <linkto id="632224881663907299" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="digits" type="literal">#</ap>
        <ap name="connectionId" type="variable">connectionId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907299" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="625" y="206">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
