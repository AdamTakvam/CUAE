<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632651848548273338" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632651848548273335" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632651848548273334" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">App.MakeCall.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632651848548273347" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632651848548273344" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632651848548273343" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632697499562687427" actid="632651848548273358" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632651848548273352" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632651848548273349" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632651848548273348" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632697499562687428" actid="632651848548273358" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632651848548273357" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632651848548273354" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632651848548273353" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632697499562687429" actid="632651848548273358" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632651848548273387" level="2" text="Metreos.Providers.FunctionalTest.Event: OnEvent">
        <node type="function" name="OnEvent" id="632651848548273384" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632651848548273383" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">App.MakeCall.script1.E_Hangup</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_MakeCall" id="632697499562687416" vid="632651848548273339">
        <Properties type="String" initWith="S_MakeCall">S_MakeCall</Properties>
      </treenode>
      <treenode text="S_MakeCallComplete" id="632697499562687418" vid="632651848548273445">
        <Properties type="String" initWith="S_MakeCallComplete">S_MakeCallComplete</Properties>
      </treenode>
      <treenode text="S_Hangup" id="632697499562687420" vid="632651848548273447">
        <Properties type="String" initWith="S_Hangup">S_Hangup</Properties>
      </treenode>
      <treenode text="g_id" id="632697499562687422" vid="632694904871796754">
        <Properties type="Long">g_id</Properties>
      </treenode>
      <treenode text="g_callId" id="632697499562687424" vid="632694904871796761">
        <Properties type="String">g_callId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632651848548273337" treenode="632651848548273338" appnode="632651848548273335" handlerfor="632651848548273334">
    <node type="Start" id="632651848548273337" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="347">
      <linkto id="632694904871796757" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632651848548273358" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="171" y="329" mx="237" my="345">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632651848548273347" />
        <item text="OnMakeCall_Failed" treenode="632651848548273352" />
        <item text="OnRemoteHangup" treenode="632651848548273357" />
      </items>
      <linkto id="632651848548273364" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632651848548273362" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">to</ap>
        <ap name="From" type="variable">from</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_callId</rd>
      </Properties>
    </node>
    <node type="Action" id="632651848548273362" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="444.610016" y="348">
      <linkto id="632651848548273366" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="callId" type="variable">g_callId</ap>
        <ap name="id" type="variable">g_id</ap>
        <ap name="signalName" type="variable">S_MakeCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632651848548273364" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="235.609985" y="510">
      <linkto id="632651848548273367" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="callId" type="variable">g_callId</ap>
        <ap name="id" type="variable">id</ap>
        <ap name="signalName" type="variable">S_MakeCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632651848548273366" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="728" y="359">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632651848548273367" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="239" y="686">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632694904871796757" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="120" y="345">
      <linkto id="632651848548273358" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">id</ap>
        <rd field="ResultData">g_id</rd>
      </Properties>
    </node>
    <node type="Variable" id="632651848548273342" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632651848548273449" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="from" refType="reference">from</Properties>
    </node>
    <node type="Variable" id="632651848548273450" name="id" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Long" initWith="id" refType="reference">id</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632651848548273346" treenode="632651848548273347" appnode="632651848548273344" handlerfor="632651848548273343">
    <node type="Start" id="632651848548273346" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="369">
      <linkto id="632651848548273369" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632651848548273369" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="295.610046" y="362">
      <linkto id="632651848548273377" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="callId" type="variable">callId</ap>
        <ap name="id" type="variable">g_id</ap>
        <ap name="signalName" type="variable">S_MakeCallComplete</ap>
      </Properties>
    </node>
    <node type="Action" id="632651848548273377" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="444" y="366">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632651848548273376" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" activetab="true" startnode="632651848548273351" treenode="632651848548273352" appnode="632651848548273349" handlerfor="632651848548273348">
    <node type="Start" id="632651848548273351" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="389">
      <linkto id="632651848548273372" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632651848548273372" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="299.610016" y="379">
      <linkto id="632651848548273375" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="callId" type="variable">callId</ap>
        <ap name="endReason" type="variable">endReason</ap>
        <ap name="id" type="variable">g_id</ap>
        <ap name="signalName" type="variable">S_MakeCallComplete</ap>
      </Properties>
    </node>
    <node type="Action" id="632651848548273375" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="697" y="369">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632651848548273374" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.MakeCall_Failed">callId</Properties>
    </node>
    <node type="Variable" id="632691184891307196" name="endReason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="EndReason" refType="reference" name="Metreos.CallControl.MakeCall_Failed">endReason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632651848548273356" treenode="632651848548273357" appnode="632651848548273354" handlerfor="632651848548273353">
    <node type="Start" id="632651848548273356" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="78" y="353">
      <linkto id="632651848548273379" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632651848548273379" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="204.610016" y="354">
      <linkto id="632651848548273380" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="callId" type="variable">callId</ap>
        <ap name="id" type="variable">g_id</ap>
        <ap name="signalName" type="variable">S_Hangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632651848548273380" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="417.610046" y="356">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632651848548273378" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent" startnode="632651848548273386" treenode="632651848548273387" appnode="632651848548273384" handlerfor="632651848548273383">
    <node type="Start" id="632651848548273386" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="349">
      <linkto id="632651848548273388" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632651848548273388" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="199.352859" y="341">
      <linkto id="632651848548273389" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632651848548273389" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="385.352844" y="337">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>