<Application name="MainApp" trigger="Metreos.Providers.JTapi.JTapiIncomingCall" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="MainApp">
    <outline>
      <treenode type="evh" id="632651698846057979" level="1" text="Metreos.Providers.JTapi.JTapiIncomingCall (trigger): OnJTapiIncomingCall">
        <node type="function" name="OnJTapiIncomingCall" id="632651698846057976" path="Metreos.StockTools" />
        <node type="event" name="JTapiIncomingCall" id="632651698846057975" path="Metreos.Providers.JTapi.JTapiIncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_CallId" id="632651698846057995" vid="632651698846057994">
        <Properties type="String">g_CallId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnJTapiIncomingCall" activetab="true" startnode="632651698846057978" treenode="632651698846057979" appnode="632651698846057976" handlerfor="632651698846057975">
    <node type="Start" id="632651698846057978" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632651698846057981" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632651698846057981" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="96" y="32">
      <linkto id="632651698846057985" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">CallId</ap>
        <rd field="ResultData">g_CallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632651698846057983" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="480" y="32">
      <linkto id="632651698846057987" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">CallId</ap>
        <rd field="ConferenceId">ConnectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632651698846057985" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="224" y="32">
      <linkto id="632651698846057991" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632651698846057983" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">CallId == null || CallId == String.Empty || CallId == "0"</ap>
      </Properties>
    </node>
    <node type="Action" id="632651698846057987" name="Send" class="MaxActionNode" group="" path="Metreos.Native.Mail" x="480" y="128">
      <linkto id="632651698846057997" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
      </Properties>
    </node>
    <node type="Action" id="632651698846057991" name="Send" class="MaxActionNode" group="" path="Metreos.Native.Mail" x="224" y="128">
      <linkto id="632651698846057993" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
      </Properties>
    </node>
    <node type="Action" id="632651698846057993" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="224" y="256">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632651698846057997" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="480" y="256">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632651698846057998" text="The purpose of this app is to anwser an incoming JTAPI call and &#xD;&#xA;email test status to jan@metreos.com." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="14" y="608" />
    <node type="Comment" id="632651698846057999" text="Send Fail Email" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="256" y="128" />
    <node type="Comment" id="632651698846058001" text="Send Success Email" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="512" y="128" />
    <node type="Variable" id="632651698846057996" name="CallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">CallId</Properties>
    </node>
    <node type="Variable" id="632651698846058003" name="EmailAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">EmailAddress</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>