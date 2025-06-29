<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632748427252386380" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632748427252386377" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632748427252386376" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/NativeTest</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632748427252386387" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632748427252386384" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632748427252386383" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632852047309000507" actid="632748427252386398" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632748427252386392" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632748427252386389" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632748427252386388" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632852047309000508" actid="632748427252386398" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632748427252386397" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632748427252386394" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632748427252386393" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632852047309000509" actid="632748427252386398" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632845847712227900" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632845847712227897" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632845847712227896" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632852047309000529" actid="632845847712227906" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632845847712227905" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632845847712227902" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632845847712227901" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632852047309000530" actid="632845847712227906" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_DefaultTest" id="632852047309000541" vid="632852047309000540">
        <Properties type="String" defaultInitWith="VALID" initWith="DefaultTest">g_DefaultTest</Properties>
      </treenode>
      <treenode text="g_AssignTest" id="632852047309000543" vid="632852047309000542">
        <Properties type="String" defaultInitWith="VALID" initWith="AssignTest">g_AssignTest</Properties>
      </treenode>
      <treenode text="g_QAtype" id="632852047309000546" vid="632852047309000545">
        <Properties type="Metreos.Types.FunctionalTests.QAtype" initWith="DefaultTest">g_QAtype</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632748427252386379" treenode="632748427252386380" appnode="632748427252386377" handlerfor="632748427252386376">
    <node type="Start" id="632748427252386379" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="427">
      <linkto id="632852047309000547" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632748427252386398" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="109" y="410" mx="175" my="426">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632748427252386387" />
        <item text="OnMakeCall_Failed" treenode="632748427252386392" />
        <item text="OnRemoteHangup" treenode="632748427252386397" />
      </items>
      <linkto id="632852013346095415" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <linkto id="632852036374220413" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="csharp">queryParams["to"]</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_QAtype</rd>
      </Properties>
    </node>
    <node type="Action" id="632748427252386402" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="708" y="236">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632748427252386425" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="568" y="237">
      <linkto id="632748427252386402" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">"Making A Call To " + queryParams["to"]</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632845838604102905" name="FileSize" class="MaxActionNode" group="" path="Metreos.Native.FunctionalTests" x="353" y="238">
      <linkto id="632845969246116664" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SomeInput" type="csharp">"c:\\Metreos\\MediaServer\\Audio\\doorbell.wav"</ap>
        <rd field="SomeOutput">g_QAtype</rd>
      </Properties>
    </node>
    <node type="Comment" id="632845838604102906" text="QA Native Action test:&#xD;&#xA;Report the file size of the audio file&#xD;&#xA;doorbell.wav" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="288" y="279" />
    <node type="Comment" id="632845838604102910" text="QA Native type test:&#xD;&#xA;Assign a string to a&#xD;&#xA;a NativeType." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="94" y="160" />
    <node type="Action" id="632845969246116664" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="447" y="237">
      <linkto id="632748427252386425" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"*** DEBUG: The value of qaTypeStr after the NativeAction FileSize is: " + g_QAtype</ap>
        <ap name="LogLevel" type="literal">Verbose</ap>
      </Properties>
    </node>
    <node type="Action" id="632846029817330414" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="160" y="238">
      <linkto id="632846799699775419" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">g_AssignTest</ap>
        <rd field="ResultData">g_QAtype</rd>
      </Properties>
    </node>
    <node type="Comment" id="632846799699775418" text="Note: This test is executed using a URL to the appliance&#xD;&#xA;and passing in a DN to call&#xD;&#xA;i.e.  http://clarke:8000/NativeTest?To=12808" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="274" y="69" />
    <node type="Action" id="632846799699775419" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="260" y="238">
      <linkto id="632845838604102905" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"*** DEBUG: The value of qaTypeStr after the Assign Action is: " + g_QAtype</ap>
        <ap name="LogLevel" type="literal">Verbose</ap>
      </Properties>
    </node>
    <node type="Comment" id="632852013346095414" text="QA Native type test:&#xD;&#xA;Assign a object returned from an action ie. CallId to a NativeType " class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="17" y="517" />
    <node type="Action" id="632852013346095415" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="391" y="426">
      <linkto id="632748427252386402" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"*** DEBUG: failure to put the result data (CallId) from makecall into the g_QAtype?"</ap>
        <ap name="LogLevel" type="literal">Warning</ap>
      </Properties>
    </node>
    <node type="Action" id="632852036374220413" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="174" y="329">
      <linkto id="632846029817330414" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"*** DEBUG: The value of qaTypeStr after the MakeCall Assignment is : " + g_QAtype</ap>
        <ap name="LogLevel" type="literal">Verbose</ap>
      </Properties>
    </node>
    <node type="Action" id="632852047309000547" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="98" y="427">
      <linkto id="632748427252386398" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"*** DEBUG: The value of g_QAtype following the default value assignment is: " + g_QAtype</ap>
        <ap name="LogLevel" type="literal">Verbose</ap>
      </Properties>
    </node>
    <node type="Variable" id="632748427252386381" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632748427252386382" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">queryParams</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632748427252386386" treenode="632748427252386387" appnode="632748427252386384" handlerfor="632748427252386383">
    <node type="Start" id="632748427252386386" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="47" y="98">
      <linkto id="632845847712227906" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632748427252386420" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="352" y="100">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632845847712227906" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="130" y="82" mx="183" my="98">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632845847712227900" />
        <item text="OnPlay_Failed" treenode="632845847712227905" />
      </items>
      <linkto id="632748427252386420" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">doorbell.wav</ap>
        <ap name="ConnectionId" type="variable">connectionID</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Variable" id="632748427252386419" name="connectionID" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connectionID</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632748427252386391" treenode="632748427252386392" appnode="632748427252386389" handlerfor="632748427252386388">
    <node type="Start" id="632748427252386391" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="51" y="122">
      <linkto id="632748427252386421" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632748427252386421" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="194" y="123">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632748427252386396" treenode="632748427252386397" appnode="632748427252386394" handlerfor="632748427252386393">
    <node type="Start" id="632748427252386396" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="144">
      <linkto id="632748427252386422" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632748427252386422" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="189" y="143">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632845847712227899" treenode="632845847712227900" appnode="632845847712227897" handlerfor="632845847712227896">
    <node type="Start" id="632845847712227899" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="80">
      <linkto id="632845847712227909" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632845847712227909" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="183" y="80">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632845847712227904" treenode="632845847712227905" appnode="632845847712227902" handlerfor="632845847712227901">
    <node type="Start" id="632845847712227904" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="77">
      <linkto id="632845847712227910" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632845847712227910" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="186" y="78">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>