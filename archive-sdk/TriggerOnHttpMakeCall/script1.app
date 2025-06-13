<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="633150759049375525" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="633150759049375522" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633150759049375521" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="633150759049375531" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="633150759049375528" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="633150759049375527" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="633154406176812304" actid="633150759049375542" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633150759049375536" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="633150759049375533" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="633150759049375532" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="633154406176812305" actid="633150759049375542" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633150759049375541" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="633150759049375538" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="633150759049375537" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="633154406176812306" actid="633150759049375542" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="633150759049375554" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="633150759049375551" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="633150759049375550" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="633154406176812313" actid="633150759049375560" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="633150759049375559" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="633150759049375556" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="633150759049375555" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="633154406176812314" actid="633150759049375560" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="633150759049375524" treenode="633150759049375525" appnode="633150759049375522" handlerfor="633150759049375521">
    <node type="Start" id="633150759049375524" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633150759049375542" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633150759049375542" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="418" y="377" mx="484" my="393">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="633150759049375531" />
        <item text="OnMakeCall_Failed" treenode="633150759049375536" />
        <item text="OnRemoteHangup" treenode="633150759049375541" />
      </items>
      <linkto id="633150759049375546" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="633154406176812326" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="csharp">query["to"]</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="633150759049375546" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="737" y="400">
      <linkto id="633150759049375548" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remotehost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">query["to"]</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="633150759049375548" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1084" y="412">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633154406176812326" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="721" y="646">
      <linkto id="633154406176812328" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remotehost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">"Error"</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
        <log condition="entry" on="true" level="Error" type="literal">Error! Make Call Provisional Failed</log>
      </Properties>
    </node>
    <node type="Action" id="633154406176812328" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="932.4707" y="634">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633150759049375526" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">query</Properties>
    </node>
    <node type="Variable" id="633150759049375547" name="remotehost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remotehost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="633150759049375530" treenode="633150759049375531" appnode="633150759049375528" handlerfor="633150759049375527">
    <node type="Start" id="633150759049375530" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="245" y="392">
      <linkto id="633150759049375560" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633150759049375560" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="492" y="433" mx="545" my="449">
      <items count="2">
        <item text="OnPlay_Complete" treenode="633150759049375554" />
        <item text="OnPlay_Failed" treenode="633150759049375559" />
      </items>
      <linkto id="633150759049375568" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">makecall_good_bye.wav</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
        <ap name="HandlerId" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="633150759049375568" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="800" y="460">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633150759049375549" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="633150759049375535" treenode="633150759049375536" appnode="633150759049375533" handlerfor="633150759049375532">
    <node type="Start" id="633150759049375535" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="134" y="293">
      <linkto id="633150759049375567" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633150759049375567" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="580" y="386">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="csharp">"MakeCall Failed: " + endReason</log>
      </Properties>
    </node>
    <node type="Variable" id="633150759049375566" name="endReason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="EndReason" refType="reference" name="Metreos.CallControl.MakeCall_Failed">endReason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="633150759049375540" treenode="633150759049375541" appnode="633150759049375538" handlerfor="633150759049375537">
    <node type="Start" id="633150759049375540" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633150759049375565" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633150759049375565" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="890" y="344">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="633150759049375553" treenode="633150759049375554" appnode="633150759049375551" handlerfor="633150759049375550">
    <node type="Start" id="633150759049375553" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633150759049375563" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633150759049375563" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="791" y="300">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" activetab="true" startnode="633150759049375558" treenode="633150759049375559" appnode="633150759049375556" handlerfor="633150759049375555">
    <node type="Start" id="633150759049375558" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="633154406176812330" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633154406176812330" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="828" y="368">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>