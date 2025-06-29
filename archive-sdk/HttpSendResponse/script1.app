<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="633040429304687958" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="633040429304687955" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633040429304687954" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/HTTP</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="testMedia" id="633163179445772516" vid="633163179445772506">
        <Properties type="Bool" initWith="Config.testMedia">testMedia</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="633040429304687957" treenode="633040429304687958" appnode="633040429304687955" handlerfor="633040429304687954">
    <node type="Start" id="633040429304687957" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="51">
      <linkto id="633163179445772480" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="633040429304687965" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="664.5891" y="55">
      <linkto id="633040429304687971" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">OK</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <log condition="entry" on="true" level="Warning" type="literal">got request</log>
      </Properties>
    </node>
    <node type="Action" id="633040429304687971" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="665" y="283">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633163085330988271" name="ReserveConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="230" y="168">
      <linkto id="633163085330988272" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="633163085330988376" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <Properties final="false" type="provider" log="On">
        <ap name="timeout" type="literal">5000</ap>
        <rd field="ConnectionId">connId1</rd>
      </Properties>
    </node>
    <node type="Action" id="633163085330988272" name="ReserveConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="233" y="343">
      <linkto id="633163085330988273" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="633163085330988377" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <Properties final="false" type="provider" log="On">
        <ap name="timeout" type="literal">5000</ap>
        <rd field="ConnectionId">connId2</rd>
      </Properties>
    </node>
    <node type="Action" id="633163085330988273" name="CreateConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="351" y="550">
      <linkto id="633163085330988274" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="633163085330988378" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connId2</ap>
        <ap name="MediaTxIP" type="literal">127.0.0.1</ap>
        <ap name="MediaTxPort" type="literal">24000</ap>
        <rd field="ConnectionId">connId2</rd>
        <rd field="ConferenceId">confId</rd>
      </Properties>
    </node>
    <node type="Action" id="633163085330988274" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="476" y="347">
      <linkto id="633163085330988281" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connId1</ap>
        <ap name="ConferenceId" type="variable">confId</ap>
        <ap name="MediaTxIP" type="literal">127.0.0.1</ap>
        <ap name="MediaTxPort" type="literal">24000</ap>
        <rd field="ConnectionId">connId1</rd>
      </Properties>
    </node>
    <node type="Action" id="633163085330988281" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="475" y="227">
      <linkto id="633163085330988282" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connId1</ap>
      </Properties>
    </node>
    <node type="Action" id="633163085330988282" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="476" y="98">
      <linkto id="633040429304687965" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connId2</ap>
      </Properties>
    </node>
    <node type="Action" id="633163085330988376" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="39" y="167">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633163085330988377" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="40" y="342">
      <linkto id="633163085330988376" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connId1</ap>
      </Properties>
    </node>
    <node type="Action" id="633163085330988378" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="479" y="550">
      <linkto id="633163085330988379" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connId1</ap>
      </Properties>
    </node>
    <node type="Action" id="633163085330988379" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="663" y="550">
      <linkto id="633040429304687971" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connId2</ap>
      </Properties>
    </node>
    <node type="Action" id="633163179445772480" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="230" y="51">
      <linkto id="633163085330988271" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="633040429304687965" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">testMedia</ap>
      </Properties>
    </node>
    <node type="Variable" id="633040429304687972" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="633163085330988275" name="connId1" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">connId1</Properties>
    </node>
    <node type="Variable" id="633163085330988276" name="connId2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">connId2</Properties>
    </node>
    <node type="Variable" id="633163085330988277" name="confId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">confId</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>