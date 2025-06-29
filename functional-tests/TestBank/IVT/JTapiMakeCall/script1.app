<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632521168379182555" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632521168379182552" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632521168379182551" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">IVT.JTapiMakeCall.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632524560713517651" level="2" text="Metreos.Providers.FunctionalTest.Event: OnEvent">
        <node type="function" name="OnEvent" id="632524560713517648" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632524560713517647" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.JTapiMakeCall.script1.E_Hangup</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_MakeCallComplete" id="632527174907346723" vid="632521168379182583">
        <Properties type="String" initWith="S_MakeCallComplete">S_MakeCallComplete</Properties>
      </treenode>
      <treenode text="callId" id="632527174907346725" vid="632526159164677029">
        <Properties type="String">callId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632521168379182554" treenode="632521168379182555" appnode="632521168379182552" handlerfor="632521168379182551">
    <node type="Start" id="632521168379182554" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="344">
      <linkto id="632521168379182564" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632521168379182564" name="JTapiMakeCall" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="352" y="344">
      <linkto id="632521168379182568" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632521168379182585" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="From" type="variable">from</ap>
        <ap name="To" type="variable">to</ap>
        <ap name="DeviceName" type="variable">devicename</ap>
        <rd field="CallId">callId</rd>
      </Properties>
    </node>
    <node type="Action" id="632521168379182568" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="503.999969" y="345">
      <linkto id="632526159164677031" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_MakeCallComplete</ap>
      </Properties>
    </node>
    <node type="Action" id="632521168379182585" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="352" y="464">
      <linkto id="632526159164677031" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_MakeCallComplete</ap>
      </Properties>
    </node>
    <node type="Action" id="632526159164677031" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="504" y="464">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632521168379182565" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632521168379182566" name="devicename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="devicename" refType="reference">devicename</Properties>
    </node>
    <node type="Variable" id="632521168379182567" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="from" refType="reference">from</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent" startnode="632524560713517650" treenode="632524560713517651" appnode="632524560713517648" handlerfor="632524560713517647">
    <node type="Start" id="632524560713517650" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="336">
      <linkto id="632524560713517674" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632524560713517674" name="JTapiHangup" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="232" y="336">
      <linkto id="632526159164677032" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632526159164677032" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="424" y="336">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
