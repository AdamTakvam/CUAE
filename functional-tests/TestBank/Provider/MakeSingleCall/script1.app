<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632651831281701903" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632146831375468922" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632146831375468921" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.MakeSingleCall.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632651831281701904" level="2" text="Metreos.CallControl.Hangup: OnHangup">
        <node type="function" name="OnHangup" id="632146831375468934" path="Metreos.StockTools" />
        <node type="event" name="Hangup" id="632146831375468933" path="Metreos.CallControl.Hangup" />
        <references>
          <ref id="632651831281701905" actid="632146831375468937" />
        </references>
        <Properties>
        </Properties>
      </treenode>
      <treenode type="evh" id="632651831281701906" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632146831375468926" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632146831375468925" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632651831281701907" actid="632146831375468937" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632651831281701908" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632146831375468930" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632146831375468929" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632651831281701909" actid="632146831375468937" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="callId" id="632651831281701885" vid="632146831375468939">
        <Properties type="Metreos.Types.String">callId</Properties>
      </treenode>
      <treenode text="S_Simple" id="632651831281701887" vid="632146831375468969">
        <Properties type="Metreos.Types.String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
      <treenode text="S_Failed" id="632651831281701889" vid="632146831375469063">
        <Properties type="Metreos.Types.String" initWith="S_Failed">S_Failed</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632146831375468923" treenode="632146831375469067" appnode="632146831375468922" handlerfor="632146831375468921">
    <node type="Start" id="632146831375468923" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632146831375468937" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632146831375468937" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="325" y="181" mx="391" my="197">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632651831281701906" />
        <item text="OnMakeCall_Failed" treenode="632651831281701908" />
        <item text="OnHangup" treenode="632651831281701904" />
      </items>
      <linkto id="632224855787500299" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">to</ap>
        <ap name="From" type="literal">METREOS</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632224855787500299" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="554" y="193">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632146831375468938" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="to" refType="reference">to</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632146831375468927" treenode="632146831375469070" appnode="632146831375468926" handlerfor="632146831375468925">
    <node type="Start" id="632146831375468927" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632146831375469061" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632146831375469061" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="278" y="158">
      <linkto id="632224855787500300" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632224855787500300" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="539" y="244">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632146831375468931" treenode="632146831375469073" appnode="632146831375468930" handlerfor="632146831375468929">
    <node type="Start" id="632146831375468931" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632146831375469062" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632146831375469062" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="176" y="296">
      <linkto id="632224855787500301" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="reason" type="variable">reason</ap>
        <ap name="signalName" type="variable">S_Failed</ap>
      </Properties>
    </node>
    <node type="Action" id="632224855787500301" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="602" y="344">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632146831375468943" name="reason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="reason" refType="reference">reason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup" startnode="632146831375468935" treenode="632146831375469076" appnode="632146831375468934" handlerfor="632146831375468933">
    <node type="Start" id="632146831375468935" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224855787500302" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632224855787500302" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="546" y="322">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>