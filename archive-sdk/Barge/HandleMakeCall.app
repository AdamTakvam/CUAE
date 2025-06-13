<Application name="HandleMakeCall" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="HandleMakeCall">
    <outline>
      <treenode type="evh" id="632471146299099787" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632471146299099784" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632471146299099783" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/MakeCall</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632624938077955230" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632624938077955227" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632624938077955226" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632627439710960446" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632627439710960443" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632627439710960442" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632627498124469426" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632627498124469423" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632627498124469422" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632471146299099786" treenode="632471146299099787" appnode="632471146299099784" handlerfor="632471146299099783">
    <node type="Start" id="632471146299099786" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="137">
      <linkto id="632471146299099793" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632471146299099793" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="154" y="137">
      <linkto id="632624938077955225" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
      </Properties>
    </node>
    <node type="Action" id="632471146299099813" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="482" y="135">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632624938077955225" name="Barge" class="MaxActionNode" group="" path="Metreos.CallControl" x="314" y="136">
      <linkto id="632471146299099813" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632627368487690157" type="Labeled" style="Bezier" label="Failure" />
      <linkto id="632627368487690157" type="Labeled" style="Bezier" label="Timeout" />
      <Properties final="false" type="provider" log="On">
        <ap name="From" type="literal">3146</ap>
        <ap name="MediaRxIP" type="literal">10.1.12.105</ap>
        <ap name="MediaRxPort" type="literal">254</ap>
      </Properties>
    </node>
    <node type="Action" id="632627368487690157" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="483" y="251">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632471146299099792" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632624938077955229" treenode="632624938077955230" appnode="632624938077955227" handlerfor="632624938077955226">
    <node type="Start" id="632624938077955229" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632624938077955231" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632624938077955231" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="374" y="41">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632627439710960445" treenode="632627439710960446" appnode="632627439710960443" handlerfor="632627439710960442">
    <node type="Start" id="632627439710960445" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632627446821627314" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632627446821627314" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="311" y="54">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" activetab="true" startnode="632627498124469425" treenode="632627498124469426" appnode="632627498124469423" handlerfor="632627498124469422">
    <node type="Start" id="632627498124469425" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632627498124469427" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632627498124469427" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="309" y="57">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>