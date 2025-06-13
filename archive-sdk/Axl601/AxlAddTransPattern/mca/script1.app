<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632921408319788134" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632921408319788131" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632921408319788130" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/axladdtranspattern</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_user" id="633120523495937965" vid="632921408319788145">
        <Properties type="String" initWith="ccmUsername">g_user</Properties>
      </treenode>
      <treenode text="g_pass" id="633120523495937967" vid="632921408319788147">
        <Properties type="String" initWith="ccmPassword">g_pass</Properties>
      </treenode>
      <treenode text="g_ccmIp" id="633120523495937969" vid="632921408319788149">
        <Properties type="String" initWith="callManagerIP">g_ccmIp</Properties>
      </treenode>
      <treenode text="g_pattern" id="633120523495937971" vid="632921408319788151">
        <Properties type="String" initWith="pattern">g_pattern</Properties>
      </treenode>
      <treenode text="g_partition" id="633120523495937973" vid="632921408319788153">
        <Properties type="String" initWith="partition">g_partition</Properties>
      </treenode>
      <treenode text="g_newPattern" id="633120523495937975" vid="632921408319788178">
        <Properties type="String" initWith="newPattern">g_newPattern</Properties>
      </treenode>
      <treenode text="g_newPartition" id="633120523495937977" vid="632921408319788180">
        <Properties type="String" initWith="newPartition">g_newPartition</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632921408319788133" treenode="632921408319788134" appnode="632921408319788131" handlerfor="632921408319788130">
    <node type="Start" id="632921408319788133" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="147" y="243">
      <linkto id="632921408319788155" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632921408319788155" name="AddTranslationPattern" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap601" x="405" y="243">
      <linkto id="633120523495937991" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="633120523495937992" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="PatternUrgency" type="literal">true</ap>
        <ap name="RoutePartitionName" type="variable">g_partition</ap>
        <ap name="Pattern" type="variable">g_pattern</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIp</ap>
        <ap name="AdminUsername" type="variable">g_user</ap>
        <ap name="AdminPassword" type="variable">g_pass</ap>
        <rd field="FaultMessage">fault</rd>
        <rd field="FaultCode">code</rd>
      </Properties>
    </node>
    <node type="Action" id="632921408319788156" name="UpdateTranslationPattern" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap601" x="740" y="349">
      <linkto id="633120523495937993" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="633120523495937994" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PatternUrgency" type="literal">true</ap>
        <ap name="RoutePartitionName" type="variable">g_partition</ap>
        <ap name="NewRoutePartitionName" type="variable">g_newPartition</ap>
        <ap name="Pattern" type="variable">g_pattern</ap>
        <ap name="NewPattern" type="variable">g_newPattern</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIp</ap>
        <ap name="AdminUsername" type="variable">g_user</ap>
        <ap name="AdminPassword" type="variable">g_pass</ap>
        <rd field="FaultMessage">fault</rd>
        <rd field="FaultCode">code</rd>
      </Properties>
    </node>
    <node type="Action" id="632921408319788182" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1034" y="348">
      <linkto id="632921408319788187" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Success!  Check for new translation pattern</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632921408319788185" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="405" y="599">
      <linkto id="632921408319788189" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"AddTranslationPattern failure.  Fault: " + fault + ", Code: " + code</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632921408319788186" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="742" y="614">
      <linkto id="632921408319788188" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">"UpdateTranslationPattern failure.  Fault: " + fault + ", Code: " + code</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632921408319788187" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1034" y="478">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632921408319788188" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="742" y="750">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632921408319788189" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="405" y="750">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633120523495937991" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="404.942139" y="480.091125">
      <linkto id="632921408319788185" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute( LogWriter log)
{
	log.Write(TraceLevel.Info, "*****Here 1******");

return "success";
}
</Properties>
    </node>
    <node type="Action" id="633120523495937992" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="738.942139" y="243.091125">
      <linkto id="632921408319788156" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute( LogWriter log)
{
	log.Write(TraceLevel.Info, "*****Here 2******");

return "success";
}</Properties>
    </node>
    <node type="Action" id="633120523495937993" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="897.942139" y="348.091125">
      <linkto id="632921408319788182" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute( LogWriter log)
{
	log.Write(TraceLevel.Info, "*****Here 3******");

return "success";
}</Properties>
    </node>
    <node type="Action" id="633120523495937994" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="741.942139" y="505.091125">
      <linkto id="632921408319788186" type="Labeled" style="Bezier" label="default" />
      <Properties language="csharp">
public static string Execute( LogWriter log)
{
	log.Write(TraceLevel.Info, "*****Here 4******");

return "success";
}</Properties>
    </node>
    <node type="Variable" id="632921408319788135" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632921408319788183" name="code" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">code</Properties>
    </node>
    <node type="Variable" id="632921408319788184" name="fault" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">fault</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>