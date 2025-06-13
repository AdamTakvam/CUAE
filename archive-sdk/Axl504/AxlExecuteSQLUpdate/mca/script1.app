<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632921155131710424" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632921155131710421" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632921155131710420" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/executeSQLUpdate</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_user" id="632921158665234325" vid="632921155131710446">
        <Properties type="String" initWith="Username">g_user</Properties>
      </treenode>
      <treenode text="g_pass" id="632921158665234327" vid="632921155131710448">
        <Properties type="String" initWith="Password">g_pass</Properties>
      </treenode>
      <treenode text="g_ccmIp" id="632921158665234329" vid="632921155131710450">
        <Properties type="String" initWith="CallManagerIP">g_ccmIp</Properties>
      </treenode>
      <treenode text="g_update" id="632921158665234331" vid="632921155131710452">
        <Properties type="String" initWith="updateQuery">g_update</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632921155131710423" treenode="632921155131710424" appnode="632921155131710421" handlerfor="632921155131710420">
    <node type="Start" id="632921155131710423" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="449">
      <linkto id="632921158665234283" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632921158665234283" name="ExecuteSQLUpdate" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap504" x="252" y="449">
      <linkto id="632921158665234286" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632921158665234288" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <linkto id="632921158665234290" type="Labeled" style="Bezier" ortho="true" label="fault" />
      <Properties final="false" type="native" log="On">
        <ap name="Update" type="variable">g_update</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIp</ap>
        <ap name="AdminUsername" type="variable">g_user</ap>
        <ap name="AdminPassword" type="variable">g_pass</ap>
        <rd field="ExecuteSQLUpdateResponse">executeSqlUpdateResponse</rd>
        <rd field="FaultMessage">fault</rd>
        <rd field="FaultCode">code</rd>
      </Properties>
    </node>
    <node type="Action" id="632921158665234286" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="506" y="315">
      <linkto id="632921158665234287" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">executeSqlUpdateResponse.Response.@return.rowsUpdated</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632921158665234287" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="751" y="446">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632921158665234288" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="502" y="450">
      <linkto id="632921158665234287" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">General Failure</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632921158665234290" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="501" y="587">
      <linkto id="632921158665234287" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"Failure to execute update command.  Fault: " + fault + ", Code: " + code</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Variable" id="632921155131710425" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632921155131710426" name="executeSqlUpdateResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap504.ExecuteSQLUpdateResponse" refType="reference">executeSqlUpdateResponse</Properties>
    </node>
    <node type="Variable" id="632921158665234284" name="fault" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">fault</Properties>
    </node>
    <node type="Variable" id="632921158665234285" name="code" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" defaultInitWith="0" refType="reference">code</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>