<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632149491589375169" level="1" text="GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632149491589375167" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632149491589375166" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ContinuousSendResponse</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="expectedIncomingBody" id="632224855787500234" vid="632149582067968908">
        <Properties type="Metreos.Types.String" initWith="expectedIncomingBody">expectedIncomingBody</Properties>
      </treenode>
      <treenode text="outgoingBody" id="632224855787500236" vid="632149582067968910">
        <Properties type="Metreos.Types.String" initWith="outgoingBody">outgoingBody</Properties>
      </treenode>
      <treenode text="checkBody" id="632224855787500238" vid="632149582067968912">
        <Properties type="Metreos.Types.String" initWith="checkBody">checkBody</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" varsy="798" startnode="632149491589375168" treenode="632149491589375169" appnode="632149491589375167" handlerfor="632149491589375166">
    <node type="Start" id="632149491589375168" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="45" y="232">
      <linkto id="632149491589375185" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Variable" id="632149491589375173" name="actualBody" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="816">
      <Properties type="Metreos.Types.String" initWith="body" refType="reference">actualBody</Properties>
    </node>
    <node type="Action" id="632149491589375180" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="506" y="228">
      <linkto id="632224855787500248" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="success" type="literal">true</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="variable">outgoingBody</ap>
      </Properties>
    </node>
    <node type="Variable" id="632149491589375181" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="117.048172" y="816">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Action" id="632149491589375185" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="166" y="232">
      <linkto id="632149491589375186" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632149491589375180" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="csharp">checkBody.ToString().ToLower()</ap>
      </Properties>
    </node>
    <node type="Action" id="632149491589375186" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="334" y="433">
      <linkto id="632149554846250164" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632149491589375180" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">expectedIncomingBody</ap>
        <ap name="Value2" type="variable">actualBody</ap>
      </Properties>
    </node>
    <node type="Action" id="632149554846250164" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="515" y="434">
      <linkto id="632224855787500248" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="success" type="literal">false</ap>
        <ap name="reason" type="literal">IncomingBodyCheckFailure</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="variable">outgoingBody</ap>
      </Properties>
    </node>
    <node type="Action" id="632224855787500248" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="641" y="316">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
