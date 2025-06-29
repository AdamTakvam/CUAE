<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.7" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632277489508468922" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632277489508468919" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632277489508468918" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.HairPinning1.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632277512596750210" level="2" text="MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632277512596750207" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632277512596750206" path="Metreos.CallControl.MakeCall_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632277646588593970" level="3" text="ReceiveDigits_Complete: OnReceiveDigits_Complete">
        <node type="function" name="OnReceiveDigits_Complete" id="632277646588593967" path="Metreos.StockTools" />
        <node type="event" name="ReceiveDigits_Complete" id="632277646588593966" path="Metreos.Providers.MediaServer.ReceiveDigits_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632277646588593975" level="3" text="ReceiveDigits_Failed: OnReceiveDigits_Failed">
        <node type="function" name="OnReceiveDigits_Failed" id="632277646588593972" path="Metreos.StockTools" />
        <node type="event" name="ReceiveDigits_Failed" id="632277646588593971" path="Metreos.Providers.MediaServer.ReceiveDigits_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632277512596750215" level="2" text="MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632277512596750212" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632277512596750211" path="Metreos.CallControl.MakeCall_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632277512596750220" level="2" text="Hangup: OnHangup">
        <node type="function" name="OnHangup" id="632277512596750217" path="Metreos.StockTools" />
        <node type="event" name="Hangup" id="632277512596750216" path="Metreos.CallControl.Hangup" />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632277512596750178" level="1" text="GotDigits: OnGotDigits">
        <node type="function" name="OnGotDigits" id="632277512596750175" path="Metreos.StockTools" />
        <node type="event" name="GotDigits" id="632277512596750174" path="Metreos.CallControl.GotDigits" />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="connectionId" id="632277828331719008" vid="632277512596750202">
        <Properties type="Metreos.Types.Int">connectionId</Properties>
      </treenode>
      <treenode text="callId" id="632277828331719010" vid="632277512596750204">
        <Properties type="Metreos.Types.String">callId</Properties>
      </treenode>
      <treenode text="S_Signal" id="632277828331719012" vid="632277512596750234">
        <Properties type="Metreos.Types.String" initWith="S_Signal">S_Signal</Properties>
      </treenode>
      <treenode text="S_Failed" id="632277828331719014" vid="632277646588594041">
        <Properties type="Metreos.Types.String" initWith="S_Failed">S_Failed</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632277489508468921" treenode="632277489508468922" appnode="632277489508468919" handlerfor="632277489508468918">
    <node type="Start" id="632277489508468921" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="367">
      <linkto id="632277512596750179" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277512596750179" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="160" y="368">
      <linkto id="632277512596750221" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="literal">0</ap>
        <ap name="connectionId" type="literal">0</ap>
        <ap name="remoteIp" type="literal">0</ap>
        <rd field="connectionId">connectionId</rd>
        <rd field="port">mediaPort</rd>
        <rd field="ipAddress">mediaIp</rd>
      </Properties>
    </node>
    <node type="Action" id="632277512596750221" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="392" y="356" mx="458" my="372">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632277512596750210" />
        <item text="OnMakeCall_Failed" treenode="632277512596750215" />
        <item text="OnHangup" treenode="632277512596750220" />
      </items>
      <linkto id="632277512596750396" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="to" type="variable">phoneNumber</ap>
        <ap name="mediaIP" type="variable">mediaIp</ap>
        <ap name="mediaPort" type="variable">mediaPort</ap>
        <ap name="from" type="literal">Test Framework</ap>
        <ap name="userData" type="literal">none</ap>
        <rd field="callId">callId</rd>
      </Properties>
    </node>
    <node type="Action" id="632277512596750396" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="606" y="375">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632277512596750180" name="mediaIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">mediaIp</Properties>
    </node>
    <node type="Variable" id="632277512596750181" name="mediaPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.UShort" refType="reference">mediaPort</Properties>
    </node>
    <node type="Variable" id="632277512596750182" name="phoneNumber" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="phoneNumber" refType="reference">phoneNumber</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" activetab="true" startnode="632277512596750209" treenode="632277512596750210" appnode="632277512596750207" handlerfor="632277512596750206">
    <node type="Start" id="632277512596750209" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="314">
      <linkto id="632277512596750397" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277512596750397" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="208" y="314">
      <linkto id="632277646588593976" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">callId</ap>
        <ap name="remotePort" type="variable">mediaPort</ap>
        <ap name="connectionId" type="variable">connectionId</ap>
        <ap name="remoteIp" type="variable">mediaIp</ap>
      </Properties>
    </node>
    <node type="Action" id="632277512596750401" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="473" y="313">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632277646588593976" name="ReceiveDigits" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="280" y="301" mx="358" my="317">
      <items count="2">
        <item text="OnReceiveDigits_Complete" treenode="632277646588593970" />
        <item text="OnReceiveDigits_Failed" treenode="632277646588593975" />
      </items>
      <linkto id="632277512596750401" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="termCondMaxDigits" type="literal">1</ap>
        <ap name="connectionId" type="variable">connectionId</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Variable" id="632277512596750398" name="mediaIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="mediaIP" refType="reference">mediaIp</Properties>
    </node>
    <node type="Variable" id="632277512596750399" name="mediaPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Int" initWith="mediaPort" refType="reference">mediaPort</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnReceiveDigits_Failed" startnode="632277646588593974" treenode="632277646588593975" appnode="632277646588593972" handlerfor="632277646588593971">
    <node type="Start" id="632277646588593974" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632277646588594043" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277646588594043" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="478" y="203">
      <linkto id="632277646588594044" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632277646588594044" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="619" y="204">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632277512596750214" treenode="632277512596750215" appnode="632277512596750212" handlerfor="632277512596750211">
    <node type="Start" id="632277512596750214" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632277512596750402" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277512596750402" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="171" y="372">
      <linkto id="632277512596750404" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632277512596750404" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="423" y="370">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup" startnode="632277512596750219" treenode="632277512596750220" appnode="632277512596750217" handlerfor="632277512596750216">
    <node type="Start" id="632277512596750219" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="353">
      <linkto id="632277512596750405" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277512596750405" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="245" y="359">
      <linkto id="632277512596750406" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632277512596750406" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="444" y="374">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotDigits" startnode="632277512596750177" treenode="632277512596750178" appnode="632277512596750175" handlerfor="632277512596750174">
    <node type="Start" id="632277512596750177" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="351">
      <linkto id="632277512596750233" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277512596750233" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="267" y="354">
      <linkto id="632277512596750407" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="dtmf" type="variable">dtmf</ap>
        <ap name="signalName" type="variable">S_Failed</ap>
      </Properties>
    </node>
    <node type="Action" id="632277512596750407" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="497" y="348">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632277512596750457" name="dtmf" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="digits" refType="reference">dtmf</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <canvas type="Function" name="OnReceiveDigits_Complete" show="false" startnode="632277646588593969" treenode="632277646588593970" appnode="632277646588593967" handlerfor="632277646588593966">
    <node type="Start" id="632277646588593969" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632277646588594039" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277646588594039" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="400" y="210">
      <linkto id="632277646588594045" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="digits" type="variable">digits</ap>
        <ap name="signalName" type="variable">S_Signal</ap>
      </Properties>
    </node>
    <node type="Action" id="632277646588594045" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="609" y="220">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632277646588594040" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="digits" refType="reference">digits</Properties>
    </node>
  </canvas>
</Application>
