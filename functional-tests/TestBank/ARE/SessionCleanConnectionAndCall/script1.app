<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.7" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632278556281250174" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632278556281250171" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632278556281250170" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.SessionCleanConnectionAndCall.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632278556281250208" level="2" text="MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632278556281250205" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632278556281250204" path="Metreos.CallControl.MakeCall_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632278556281250213" level="2" text="MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632278556281250210" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632278556281250209" path="Metreos.CallControl.MakeCall_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632278556281250218" level="2" text="Hangup: OnHangup">
        <node type="function" name="OnHangup" id="632278556281250215" path="Metreos.StockTools" />
        <node type="event" name="Hangup" id="632278556281250214" path="Metreos.CallControl.Hangup" />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="connectionId" id="632278566255312687" vid="632278556281250226">
        <Properties type="Metreos.Types.Int">connectionId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632278556281250173" treenode="632278556281250174" appnode="632278556281250171" handlerfor="632278556281250170">
    <node type="Start" id="632278556281250173" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="342">
      <linkto id="632278556281250183" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632278556281250183" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="225" y="339">
      <linkto id="632278556281250219" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remotePort" type="literal">0</ap>
        <ap name="connectionId" type="literal">0</ap>
        <ap name="remoteIp" type="literal">0</ap>
        <rd field="connectionId">connectionId</rd>
        <rd field="port">mediaPort</rd>
        <rd field="ipAddress">mediaIp</rd>
      </Properties>
    </node>
    <node type="Action" id="632278556281250219" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="442" y="317" mx="508" my="333">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632278556281250208" />
        <item text="OnMakeCall_Failed" treenode="632278556281250213" />
        <item text="OnHangup" treenode="632278556281250218" />
      </items>
      <linkto id="632278556281250222" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="to" type="variable">phoneNumber</ap>
        <ap name="connectionId" type="variable">connectionId</ap>
        <ap name="mediaIP" type="variable">mediaIp</ap>
        <ap name="mediaPort" type="variable">mediaPort</ap>
        <ap name="from" type="literal">Test Framework</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632278556281250222" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="778" y="339">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632278556281250220" name="mediaIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">mediaIp</Properties>
    </node>
    <node type="Variable" id="632278556281250221" name="mediaPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Int" refType="reference">mediaPort</Properties>
    </node>
    <node type="Variable" id="632278566255312704" name="phoneNumber" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="phoneNumber" refType="reference">phoneNumber</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632278556281250207" treenode="632278556281250208" appnode="632278556281250205" handlerfor="632278556281250204">
    <node type="Start" id="632278556281250207" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632278556281250225" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632278556281250223" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="642" y="274">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632278556281250225" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.Providers.MediaServer" x="382" y="142">
      <linkto id="632278556281250223" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">callId</ap>
        <ap name="remotePort" type="variable">mediaPort</ap>
        <ap name="connectionId" type="variable">connectionId</ap>
        <ap name="remoteIp" type="variable">mediaIp</ap>
      </Properties>
    </node>
    <node type="Variable" id="632278556281250228" name="mediaPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Int" refType="reference">mediaPort</Properties>
    </node>
    <node type="Variable" id="632278556281250229" name="mediaIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">mediaIp</Properties>
    </node>
    <node type="Variable" id="632278556281250230" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632278556281250212" treenode="632278556281250213" appnode="632278556281250210" handlerfor="632278556281250209">
    <node type="Start" id="632278556281250212" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632278556281250224" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632278556281250224" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="641" y="272">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup" startnode="632278556281250217" treenode="632278556281250218" appnode="632278556281250215" handlerfor="632278556281250214">
    <node type="Start" id="632278556281250217" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632278556281250231" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632278556281250231" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="777" y="292">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
