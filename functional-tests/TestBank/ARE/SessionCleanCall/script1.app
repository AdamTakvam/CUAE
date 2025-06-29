<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.7" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632278525362031424" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632278525362031421" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632278525362031420" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.SessionCleanCall.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632278525362031437" level="2" text="MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632278525362031434" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632278525362031433" path="Metreos.CallControl.MakeCall_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632278525362031442" level="2" text="MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632278525362031439" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632278525362031438" path="Metreos.CallControl.MakeCall_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632278525362031447" level="2" text="Hangup: OnHangup">
        <node type="function" name="OnHangup" id="632278525362031444" path="Metreos.StockTools" />
        <node type="event" name="Hangup" id="632278525362031443" path="Metreos.CallControl.Hangup" />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632278525362031423" treenode="632278525362031424" appnode="632278525362031421" handlerfor="632278525362031420">
    <node type="Start" id="632278525362031423" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="247">
      <linkto id="632278525362031448" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632278525362031448" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="239" y="230" mx="305" my="246">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632278525362031437" />
        <item text="OnMakeCall_Failed" treenode="632278525362031442" />
        <item text="OnHangup" treenode="632278525362031447" />
      </items>
      <linkto id="632278525362031453" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="to" type="variable">phoneNumber</ap>
        <ap name="mediaIP" type="literal">127.0.0.1</ap>
        <ap name="mediaPort" type="literal">5000</ap>
        <ap name="from" type="literal">Test Framework</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632278525362031453" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="624" y="242">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632278525362031449" name="phoneNumber" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="phoneNumber" refType="reference">phoneNumber</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632278525362031436" treenode="632278525362031437" appnode="632278525362031434" handlerfor="632278525362031433">
    <node type="Start" id="632278525362031436" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632278525362031456" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632278525362031456" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="783" y="285">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632278525362031441" treenode="632278525362031442" appnode="632278525362031439" handlerfor="632278525362031438">
    <node type="Start" id="632278525362031441" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632278525362031454" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632278525362031454" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="594" y="264">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup" startnode="632278525362031446" treenode="632278525362031447" appnode="632278525362031444" handlerfor="632278525362031443">
    <node type="Start" id="632278525362031446" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632278525362031455" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632278525362031455" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="750" y="260">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
