<Application name="StopRecording" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="StopRecording">
    <outline>
      <treenode type="evh" id="632130703199843870" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632127022749568492" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632127022749568491" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/click-to-talk/stopRecord</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632127022749568493" treenode="632130703199843870" appnode="632127022749568492" handlerfor="632127022749568491">
    <node type="Start" id="632127022749568493" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632127123824150228" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632127123824150228" name="GetConferenceData" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="192" y="96">
      <linkto id="632477159022886129" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632127131572091002" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">dbConferenceId</ap>
        <rd field="ResultData">conferenceData</rd>
        <log condition="default" on="true" level="Error" type="csharp">"Could not retrieve data for conference:" + dbConferenceId</log>
      </Properties>
    </node>
    <node type="Action" id="632127131572091002" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="265" y="208">
      <linkto id="632341438297501940" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">404</ap>
        <ap name="responsePhrase" type="literal">NOT FOUND</ap>
      </Properties>
    </node>
    <node type="Action" id="632127131572091005" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="624" y="96">
      <linkto id="632341438297501941" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
      </Properties>
    </node>
    <node type="Action" id="632127131572091007" name="UpdateConferenceData" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="488" y="96">
      <linkto id="632127131572091005" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ID" type="variable">dbConferenceId</ap>
        <ap name="RecordEnded" type="literal">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632341438297501940" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="265" y="312">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632341438297501941" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="744" y="96">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632477159022886129" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="328" y="96">
      <linkto id="632127131572091007" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632127131572091002" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="csharp">conferenceData.RecordConnId</ap>
      </Properties>
    </node>
    <node type="Variable" id="632127123824150227" name="conferenceData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.ClickToTalk.ConferenceData" refType="reference">conferenceData</Properties>
    </node>
    <node type="Variable" id="632127131572091004" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632536540248685547" name="dbConferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" initWith="body" refType="reference" name="Metreos.Providers.Http.GotRequest">dbConferenceId</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>