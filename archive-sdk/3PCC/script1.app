<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632575405144373455" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632575405144373452" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632575405144373451" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575405144373462" level="2" text="Metreos.Providers.JTapi.JTapiCallActive: OnJTapiCallActive">
        <node type="function" name="OnJTapiCallActive" id="632575405144373459" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallActive" id="632575405144373458" path="Metreos.Providers.JTapi.JTapiCallActive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575405144373467" level="2" text="Metreos.Providers.JTapi.JTapiCallInactive: OnJTapiCallInactive">
        <node type="function" name="OnJTapiCallInactive" id="632575405144373464" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallInactive" id="632575405144373463" path="Metreos.Providers.JTapi.JTapiCallInactive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632575405144373472" level="2" text="Metreos.Providers.JTapi.JTapiHangup: OnJTapiHangup">
        <node type="function" name="OnJTapiHangup" id="632575405144373469" path="Metreos.StockTools" />
        <node type="event" name="JTapiHangup" id="632575405144373468" path="Metreos.Providers.JTapi.JTapiHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_InCall" id="632575459599174646" vid="632575405144373479">
        <Properties type="Bool">g_InCall</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632575405144373454" treenode="632575405144373455" appnode="632575405144373452" handlerfor="632575405144373451">
    <node type="Start" id="632575405144373454" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="38" y="193">
      <linkto id="632575405144373477" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575405144373456" name="JTapiMakeCall" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="267" y="193">
      <linkto id="632575405144373457" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632575405144373476" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="literal">1697</ap>
        <ap name="DeviceName" type="literal">SEP0012DAAD3732</ap>
      </Properties>
    </node>
    <node type="Action" id="632575405144373457" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="412" y="193">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575405144373476" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="265" y="341">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632575405144373477" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="134" y="194">
      <linkto id="632575405144373456" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remotehost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
      </Properties>
    </node>
    <node type="Variable" id="632575405144373478" name="remotehost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remotehost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallActive" activetab="true" startnode="632575405144373461" treenode="632575405144373462" appnode="632575405144373459" handlerfor="632575405144373458">
    <node type="Start" id="632575405144373461" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="126">
      <linkto id="632575459599174661" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575405144373473" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="205" y="256">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Error" type="literal">Transfer Failed</log>
      </Properties>
    </node>
    <node type="Action" id="632575459599174661" name="JTapiBlindTransfer" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="205" y="125">
      <linkto id="632575405144373473" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632575459599174662" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="To" type="literal">102004</ap>
        <log condition="entry" on="true" level="Warning" type="literal">Call Active, Initiating transfer</log>
      </Properties>
    </node>
    <node type="Action" id="632575459599174662" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="392" y="125">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632575459599174663" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Long" initWith="CallId" refType="reference" name="Metreos.Providers.JTapi.JTapiCallActive">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallInactive" startnode="632575405144373466" treenode="632575405144373467" appnode="632575405144373464" handlerfor="632575405144373463">
    <node type="Start" id="632575405144373466" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="51" y="185">
      <linkto id="632575405144373474" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575405144373474" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="189" y="184">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Warning" type="literal">Call Inactive</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiHangup" startnode="632575405144373471" treenode="632575405144373472" appnode="632575405144373469" handlerfor="632575405144373468">
    <node type="Start" id="632575405144373471" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="54" y="167">
      <linkto id="632575405144373475" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632575405144373475" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="172" y="167">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Warning" type="literal">Call Terminated</log>
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>