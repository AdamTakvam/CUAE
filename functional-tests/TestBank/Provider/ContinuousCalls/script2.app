<Application name="script2" trigger="Metreos.CallControl.IncomingCall" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script2">
    <outline>
      <treenode type="evh" id="632155715076406529" level="1" text="IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632155715076406527" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632155715076406526" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632155715076406539" level="2" text="AnswerCall_Complete: OnAnswerCall_Complete">
        <node type="function" name="OnAnswerCall_Complete" id="632155715076406537" path="Metreos.StockTools" />
        <node type="event" name="AnswerCall_Complete" id="632155715076406536" path="Metreos.CallControl.AnswerCall_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632155715076406543" level="2" text="AnswerCall_Failed: OnAnswerCall_Failed">
        <node type="function" name="OnAnswerCall_Failed" id="632155715076406541" path="Metreos.StockTools" />
        <node type="event" name="AnswerCall_Failed" id="632155715076406540" path="Metreos.CallControl.AnswerCall_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632155715076406547" level="2" text="Hangup: OnHangup">
        <node type="function" name="OnHangup" id="632155715076406545" path="Metreos.StockTools" />
        <node type="event" name="Hangup" id="632155715076406544" path="Metreos.CallControl.Hangup" />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_ReceivedCall" id="632224814145469583" vid="632155715076406550">
        <Properties type="Metreos.Types.String" initWith="S_ReceivedCall">S_ReceivedCall</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" varsy="667" startnode="632155715076406528" treenode="632155715076406529" appnode="632155715076406527" handlerfor="632155715076406526">
    <node type="Start" id="632155715076406528" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="109">
      <linkto id="632155715076406534" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632155715076406534" name="SetMedia" class="MaxActionNode" group="" path="Metreos.CallControl" x="167" y="111">
      <linkto id="632155715076406548" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="mediaPort" type="literal">20000</ap>
        <ap name="mediaIP" type="literal">localhost</ap>
        <ap name="callId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Variable" id="632155715076406535" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="685">
      <Properties type="Metreos.Types.String" initWith="callId" refType="reference">callId</Properties>
    </node>
    <node type="Action" id="632155715076406548" name="AnswerCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="287" y="91" mx="358" my="107">
      <items count="3">
        <item text="OnAnswerCall_Complete" treenode="632155715076406539" />
        <item text="OnAnswerCall_Failed" treenode="632155715076406543" />
        <item text="OnHangup" treenode="632155715076406547" />
      </items>
      <linkto id="632155715076406549" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="callId" type="variable">callId</ap>
        <ap name="answer" type="literal">true</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632155715076406549" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="515" y="107">
      <linkto id="632224814145469490" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_ReceivedCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632224814145469490" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="644" y="100">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnAnswerCall_Complete" varsy="692" startnode="632155715076406538" treenode="632155715076406539" appnode="632155715076406537" handlerfor="632155715076406536">
    <node type="Start" id="632155715076406538" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224814145469599" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632224814145469599" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="486" y="287">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnAnswerCall_Failed" varsy="692" startnode="632155715076406542" treenode="632155715076406543" appnode="632155715076406541" handlerfor="632155715076406540">
    <node type="Start" id="632155715076406542" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224814145469600" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632224814145469600" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="410" y="322">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup" varsy="692" startnode="632155715076406546" treenode="632155715076406547" appnode="632155715076406545" handlerfor="632155715076406544">
    <node type="Start" id="632155715076406546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632224814145469601" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632224814145469601" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="550" y="257">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
