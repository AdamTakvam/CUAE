<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632521058645571467" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632521058645571464" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632521058645571463" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/SpecificText</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632521064764204373" level="2" text="Metreos.Providers.FunctionalTest.Event: OnEvent">
        <node type="function" name="OnEvent" id="632521064764204370" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632521064764204369" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">SpecificText</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_remoteHost" id="632521064764204360" vid="632521063257319714">
        <Properties type="String">g_remoteHost</Properties>
      </treenode>
      <treenode text="g_host" id="632521064764204362" vid="632521063257319716">
        <Properties type="String">g_host</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632521058645571466" treenode="632521058645571467" appnode="632521058645571464" handlerfor="632521058645571463">
    <node type="Start" id="632521058645571466" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="360">
      <linkto id="632521063257319713" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632521063257319713" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="285" y="356">
      <linkto id="632521064764204367" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">remoteHost</ap>
        <ap name="Value2" type="variable">host</ap>
        <rd field="ResultData">g_remoteHost</rd>
        <rd field="ResultData2">g_host</rd>
      </Properties>
    </node>
    <node type="Action" id="632521064764204367" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="440" y="356">
      <linkto id="632521064764204368" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="literal">IVT.SendExecute.script1.S_Signal</ap>
      </Properties>
    </node>
    <node type="Action" id="632521064764204368" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="599" y="355">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632521058645571468" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632521058645571469" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent" startnode="632521064764204372" treenode="632521064764204373" appnode="632521064764204370" handlerfor="632521064764204369">
    <node type="Start" id="632521064764204372" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="344">
      <linkto id="632521064764204377" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632521064764204377" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="256" y="344">
      <linkto id="632521064764204379" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="variable">title</ap>
        <ap name="Prompt" type="variable">prompt</ap>
        <ap name="Text" type="variable">text</ap>
        <rd field="ResultData">textXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632521064764204379" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="448" y="344">
      <linkto id="632521064764204380" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">g_remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">textXml.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632521064764204380" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="616" y="344">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632521064764204374" name="title" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="title" refType="reference">title</Properties>
    </node>
    <node type="Variable" id="632521064764204375" name="prompt" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="prompt" refType="reference">prompt</Properties>
    </node>
    <node type="Variable" id="632521064764204376" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632521064764204378" name="textXml" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">textXml</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
