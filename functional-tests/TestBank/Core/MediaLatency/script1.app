<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632675947861651707" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632675947861651704" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632675947861651703" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Core.MediaLatency.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632675947861651722" level="2" text="Metreos.Providers.FunctionalTest.Event: OnEvent">
        <node type="function" name="OnEvent" id="632675947861651719" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632675947861651718" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Core.MediaLatency.script1.E_Shutdown</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632679038858419077" vid="632675947861651716">
        <Properties type="String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
      <treenode text="conferenceId" id="632679038858419079" vid="632675947861651731">
        <Properties type="String">conferenceId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632675947861651706" treenode="632675947861651707" appnode="632675947861651704" handlerfor="632675947861651703">
    <node type="Start" id="632675947861651706" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="406">
      <linkto id="632675947861651723" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632675947861651723" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="217" y="407">
      <linkto id="632675947861651733" type="Labeled" style="Bevel" label="Success" />
      <linkto id="632675947861651754" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxIP" type="literal">192.168.1.105</ap>
        <ap name="MediaTxPort" type="literal">4000</ap>
        <ap name="ConnectionId" type="literal">0</ap>
        <rd field="ConnectionId">connectionId1</rd>
        <rd field="MediaRxIP">rxIp</rd>
        <rd field="MediaRxPort">rxPort</rd>
      </Properties>
    </node>
    <node type="Action" id="632675947861651724" name="CreateConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="619" y="407">
      <linkto id="632675947861651739" type="Labeled" style="Bevel" label="Success" />
      <linkto id="632675947861651764" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Hairpin" type="variable">hairpin</ap>
        <ap name="SoundToneOnJoin" type="literal">false</ap>
        <ap name="ConnectionId" type="variable">connectionId1</ap>
        <rd field="ConferenceId">conferenceId</rd>
      </Properties>
    </node>
    <node type="Action" id="632675947861651733" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="377" y="406">
      <linkto id="632675947861651736" type="Labeled" style="Bevel" label="Success" />
      <linkto id="632675947861651758" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxIP" type="variable">testClientIp</ap>
        <ap name="MediaTxPort" type="variable">testClientPort</ap>
        <ap name="ConnectionId" type="literal">0</ap>
        <rd field="ConnectionId">connectionId2</rd>
      </Properties>
    </node>
    <node type="Action" id="632675947861651736" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="474" y="406">
      <linkto id="632675947861651724" type="Labeled" style="Bevel" label="true" />
      <linkto id="632675947861651737" type="Labeled" style="Bevel" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">hairpin</ap>
      </Properties>
    </node>
    <node type="Action" id="632675947861651737" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="532" y="512">
      <linkto id="632675947861651724" type="Labeled" style="Bevel" label="Success" />
      <linkto id="632675947861651756" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxIP" type="literal">192.168.1.106</ap>
        <ap name="MediaTxPort" type="literal">4050</ap>
        <rd field="ConnectionId">connectionId3</rd>
      </Properties>
    </node>
    <node type="Action" id="632675947861651739" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="739" y="408">
      <linkto id="632675947861651741" type="Labeled" style="Bevel" label="Success" />
      <linkto id="632675947861651762" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Hairpin" type="variable">hairpin</ap>
        <ap name="ConnectionId" type="variable">connectionId2</ap>
        <ap name="ConferenceId" type="variable">conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632675947861651740" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="896.1449" y="512">
      <linkto id="632675947861651743" type="Labeled" style="Bevel" label="Success" />
      <linkto id="632675947861651760" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId3</ap>
        <ap name="ConferenceId" type="variable">conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632675947861651741" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="840" y="409">
      <linkto id="632675947861651743" type="Labeled" style="Bevel" label="true" />
      <linkto id="632675947861651740" type="Labeled" style="Bevel" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">hairpin</ap>
      </Properties>
    </node>
    <node type="Action" id="632675947861651743" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="1001.35742" y="412">
      <linkto id="632675947861651753" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="rxIp" type="variable">rxIp</ap>
        <ap name="rxPort" type="variable">rxPort</ap>
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632675947861651744" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="1021.35742" y="609">
      <linkto id="632675947861651752" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632675947861651746" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="692" y="604">
      <linkto id="632675947861651747" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632675947861651747" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="806" y="601">
      <linkto id="632675947861651749" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId2</ap>
      </Properties>
    </node>
    <node type="Action" id="632675947861651749" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="917" y="603">
      <linkto id="632675947861651744" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connectionId3</ap>
      </Properties>
    </node>
    <node type="Label" id="632675947861651751" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="593" y="603">
      <linkto id="632675947861651746" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632675947861651752" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1103.62012" y="609">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632675947861651753" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1096.62" y="357">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632675947861651754" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="220" y="547" />
    <node type="Label" id="632675947861651756" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="531" y="577" />
    <node type="Label" id="632675947861651758" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="380" y="542" />
    <node type="Label" id="632675947861651760" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="989" y="553" />
    <node type="Label" id="632675947861651762" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="725" y="516" />
    <node type="Label" id="632675947861651764" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="643" y="547" />
    <node type="Variable" id="632675947861651725" name="testClientIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="testClientIp" refType="reference">testClientIp</Properties>
    </node>
    <node type="Variable" id="632675947861651726" name="testClientPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="testClientPort" refType="reference">testClientPort</Properties>
    </node>
    <node type="Variable" id="632675947861651727" name="rxIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">rxIp</Properties>
    </node>
    <node type="Variable" id="632675947861651728" name="rxPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">rxPort</Properties>
    </node>
    <node type="Variable" id="632675947861651729" name="hairpin" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" initWith="hairpin" refType="reference">hairpin</Properties>
    </node>
    <node type="Variable" id="632675947861651730" name="connectionId1" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId1</Properties>
    </node>
    <node type="Variable" id="632675947861651735" name="connectionId2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId2</Properties>
    </node>
    <node type="Variable" id="632675947861651738" name="connectionId3" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId3</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent" startnode="632675947861651721" treenode="632675947861651722" appnode="632675947861651719" handlerfor="632675947861651718">
    <node type="Start" id="632675947861651721" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="85" y="356">
      <linkto id="632675947861651766" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632675947861651766" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="317" y="356">
      <linkto id="632675947861651767" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConferenceId" type="variable">conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632675947861651767" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="506" y="357">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>