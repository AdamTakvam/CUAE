<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632520303397145547" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632520303397145544" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632520303397145543" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/CreateDirectory</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632520303397145546" treenode="632520303397145547" appnode="632520303397145544" handlerfor="632520303397145543">
    <node type="Loop" id="632520303397145573" name="Loop" text="loop 32x" cx="162" cy="160" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="208" y="316" mx="289" my="396">
      <linkto id="632520303397145574" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632520303397145551" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="literal">32</Properties>
    </node>
    <node type="Start" id="632520303397145546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="395">
      <linkto id="632520303397145570" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632520303397145551" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="436" y="394">
      <linkto id="632520303397145571" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Text</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/CreateText"</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632520303397145553" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="807" y="388">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632520303397145554" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="682" y="389">
      <linkto id="632520303397145553" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">directory.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632520303397145570" name="CreateDirectory" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="141" y="396">
      <linkto id="632520303397145573" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Create Directory</ap>
        <ap name="Prompt" type="literal">Options</ap>
        <rd field="ResultData">directory</rd>
        <log condition="entry" on="true" level="Info" type="literal">Creating Directory</log>
      </Properties>
    </node>
    <node type="Action" id="632520303397145571" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="555" y="396">
      <linkto id="632520303397145554" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632520303397145574" name="AddDirectoryEntry" container="632520303397145573" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="285" y="391">
      <linkto id="632520303397145573" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">"DirNum " + loopIndex</ap>
        <ap name="Telephone" type="literal">2000</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Variable" id="632520303397145548" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632520303397145549" name="directory" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Directory" refType="reference">directory</Properties>
    </node>
    <node type="Variable" id="632520303397145552" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
