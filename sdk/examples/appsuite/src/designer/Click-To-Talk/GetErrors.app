<Application name="GetErrors" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="GetErrors">
    <outline>
      <treenode type="evh" id="632130671664219349" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632116075241171834" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632116075241171833" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/click-to-talk/errors.xml</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632116075241171835" treenode="632130671664219349" appnode="632116075241171834" handlerfor="632116075241171833">
    <node type="Loop" id="632116852951182305" name="Loop" text="loop (expr)" cx="147" cy="136" entry="2" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="360" y="200" mx="434" my="268">
      <linkto id="632116852951182304" fromport="2" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632116852951182304" fromport="2" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632116852951182301" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="csharp">errors.Count</Properties>
    </node>
    <node type="Start" id="632116075241171835" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632536540248685494" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632116852951182298" name="GetConferenceErrors" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="152" y="104">
      <linkto id="632116852951182299" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632116852951182300" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">dbConferenceId</ap>
        <rd field="ResultData">errors</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"Retrieving errors for conference started by host at address: " + hostIP</log>
      </Properties>
    </node>
    <node type="Action" id="632116852951182299" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="288" y="104">
      <linkto id="632116852951182300" type="Labeled" style="Bezier" ortho="true" label="0" />
      <linkto id="632116852951182303" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">errors.Count</ap>
      </Properties>
    </node>
    <node type="Action" id="632116852951182300" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="152" y="304">
      <linkto id="632341438297501398" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">hostIP</ap>
        <ap name="responseCode" type="literal">200</ap>
      </Properties>
    </node>
    <node type="Action" id="632116852951182301" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="632" y="272">
      <linkto id="632341438297501399" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">hostIP</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="variable">errorMenu</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Sending menu to: " + hostIP</log>
      </Properties>
    </node>
    <node type="Action" id="632116852951182303" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="432" y="104">
      <linkto id="632116852951182305" port="2" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Conference errors</ap>
        <rd field="ResultData">errorMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632341438297501398" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="152" y="416">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632116852951182304" name="AddMenuItem" container="632116852951182305" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="408" y="272">
      <linkto id="632116852951182305" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">errors[loopIndex]</ap>
        <rd field="ResultData">errorMenu</rd>
        <log condition="default" on="true" level="Info" type="csharp">errors[loopIndex]</log>
      </Properties>
    </node>
    <node type="Action" id="632341438297501399" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="752" y="272">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632536540248685494" name="GetConferenceId" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="32" y="104">
      <linkto id="632116852951182298" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="HostIP" type="variable">hostIP</ap>
        <rd field="ConferenceId">dbConferenceId</rd>
      </Properties>
    </node>
    <node type="Variable" id="632116852951182295" name="hostIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">hostIP</Properties>
    </node>
    <node type="Variable" id="632116852951182296" name="errors" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.StringCollection" refType="reference">errors</Properties>
    </node>
    <node type="Variable" id="632116852951182297" name="errorMenu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">errorMenu</Properties>
    </node>
    <node type="Variable" id="632536540248685495" name="dbConferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">dbConferenceId</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>