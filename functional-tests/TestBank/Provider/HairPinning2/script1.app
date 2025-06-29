<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632514021171074017" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632277489508468919" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632277489508468918" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties>
          <ep name="testScriptName" type="literal">Provider.HairPinning2.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632514021171074018" level="2" text="Metreos.CallControl.Hangup: OnHangup">
        <node type="function" name="OnHangup" id="632277512596750217" path="Metreos.StockTools" />
        <node type="event" name="Hangup" id="632277512596750216" path="Metreos.CallControl.Hangup" />
        <references>
          <ref id="632514021171074019" actid="632277512596750221" />
        </references>
        <Properties>
        </Properties>
      </treenode>
      <treenode type="evh" id="632514021171074020" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632277512596750207" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632277512596750206" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632514021171074021" actid="632277512596750221" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632514021171074022" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632277512596750212" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632277512596750211" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632514021171074023" actid="632277512596750221" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632514021171074024" level="2" text="Metreos.Providers.MediaServer.PlayAnnouncement_Complete: OnPlayAnnouncement_Complete">
        <node type="function" name="OnPlayAnnouncement_Complete" id="632277646588594190" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Complete" id="632277646588594189" path="Metreos.Providers.MediaServer.PlayAnnouncement_Complete" />
        <references>
          <ref id="632514021171074025" actid="632277646588594199" />
        </references>
        <Properties>
        </Properties>
      </treenode>
      <treenode type="evh" id="632514021171074026" level="2" text="Metreos.Providers.MediaServer.PlayAnnouncement_Failed: OnPlayAnnouncement_Failed">
        <node type="function" name="OnPlayAnnouncement_Failed" id="632277646588594195" path="Metreos.StockTools" />
        <node type="event" name="PlayAnnouncement_Failed" id="632277646588594194" path="Metreos.Providers.MediaServer.PlayAnnouncement_Failed" />
        <references>
          <ref id="632514021171074027" actid="632277646588594199" />
        </references>
        <Properties>
        </Properties>
      </treenode>
      <treenode type="evh" id="632514021171074028" level="1" text="Metreos.CallControl.GotDigits: OnGotDigits">
        <node type="function" name="OnGotDigits" id="632277512596750175" path="Metreos.StockTools" />
        <node type="event" name="GotDigits" id="632277512596750174" path="Metreos.CallControl.GotDigits" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632514021171074029" level="1" text="Metreos.CallControl.SignallingChange: OnSignallingChange">
        <node type="function" name="OnSignallingChange" id="632277646588594185" path="Metreos.StockTools" />
        <node type="event" name="SignallingChange" id="632277646588594184" path="Metreos.CallControl.SignallingChange" />
        <references />
        <Properties>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="connectionId" id="632514021171073975" vid="632277512596750202">
        <Properties type="Metreos.Types.Int">connectionId</Properties>
      </treenode>
      <treenode text="callId" id="632514021171073977" vid="632277512596750204">
        <Properties type="Metreos.Types.String">callId</Properties>
      </treenode>
      <treenode text="S_Signal" id="632514021171073979" vid="632277512596750234">
        <Properties type="Metreos.Types.String" initWith="S_Signal">S_Signal</Properties>
      </treenode>
      <treenode text="S_Failed" id="632514021171073981" vid="632277646588594041">
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
        <item text="OnMakeCall_Complete" treenode="632514021171074020" />
        <item text="OnMakeCall_Failed" treenode="632514021171074022" />
        <item text="OnHangup" treenode="632514021171074018" />
      </items>
      <linkto id="632277512596750396" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="To" type="variable">phoneNumber</ap>
        <ap name="From" type="literal">Test Framework</ap>
        <ap name="UserData" type="literal">none</ap>
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
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632277512596750209" treenode="632277512596750210" appnode="632277512596750207" handlerfor="632277512596750206">
    <node type="Start" id="632277512596750209" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="314">
      <linkto id="632277512596750397" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277512596750397" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="208" y="314">
      <linkto id="632277646588594199" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="variable">mediaPort</ap>
        <ap name="connectionId" type="variable">connectionId</ap>
        <ap name="callId" type="variable">callId</ap>
        <ap name="remoteIp" type="variable">mediaIp</ap>
      </Properties>
    </node>
    <node type="Action" id="632277512596750401" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="510" y="316">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632277646588594199" name="PlayAnnouncement" class="MaxAsyncActionNode" group="" path="Metreos.Providers.MediaServer" x="247" y="300" mx="340" my="316">
      <items count="2">
        <item text="OnPlayAnnouncement_Complete" treenode="632514021171074024" />
        <item text="OnPlayAnnouncement_Failed" treenode="632514021171074026" />
      </items>
      <linkto id="632277512596750401" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId</ap>
        <ap name="filename" type="literal">kylie.vox</ap>
        <ap name="audioFileFormat" type="literal">vox</ap>
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
  <canvas type="Function" name="OnPlayAnnouncement_Complete" startnode="632277646588594192" treenode="632277646588594193" appnode="632277646588594190" handlerfor="632277646588594189">
    <node type="Start" id="632277646588594192" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632277646588594201" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277646588594201" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="629" y="241">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlayAnnouncement_Failed" startnode="632277646588594197" treenode="632277646588594198" appnode="632277646588594195" handlerfor="632277646588594194">
    <node type="Start" id="632277646588594197" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632277646588594200" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277646588594200" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="482" y="272">
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
      <linkto id="632277646588594203" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="connectionId" type="variable">connectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632277512596750406" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="456" y="363">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632277646588594203" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="343" y="362">
      <linkto id="632277512596750406" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Signal</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotDigits" startnode="632277512596750177" treenode="632277512596750178" appnode="632277512596750175" handlerfor="632277512596750174">
    <node type="Start" id="632277512596750177" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="351">
      <linkto id="632277512596750407" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277512596750407" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="490" y="358">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632277512596750457" name="dtmf" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="digits" refType="reference">dtmf</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSignallingChange" startnode="632277646588594187" treenode="632277646588594188" appnode="632277646588594185" handlerfor="632277646588594184">
    <node type="Start" id="632277646588594187" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="360">
      <linkto id="632277646588594202" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632277646588594202" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="493" y="281">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
