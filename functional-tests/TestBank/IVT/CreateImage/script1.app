<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632520303397145547" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632520303397145544" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632520303397145543" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/CreateImage</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632520303397145546" treenode="632520303397145547" appnode="632520303397145544" handlerfor="632520303397145543">
    <node type="Start" id="632520303397145546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="395">
      <linkto id="632520334149048395" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632520303397145551" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="289" y="391">
      <linkto id="632520303397145571" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Text</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/CreateText"</ap>
        <rd field="ResultData">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632520303397145553" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="631" y="384">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632520303397145554" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="535" y="386">
      <linkto id="632520303397145553" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">image.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632520303397145571" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="408" y="393">
      <linkto id="632520303397145554" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">image</rd>
      </Properties>
    </node>
    <node type="Action" id="632520334149048395" name="CreateImage" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="185" y="393">
      <linkto id="632520303397145551" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">IVT Image</ap>
        <ap name="Prompt" type="literal">Options</ap>
        <ap name="LocationX" type="literal">-1</ap>
        <ap name="LocationY" type="literal">-1</ap>
        <ap name="Height" type="literal">65</ap>
        <ap name="Width" type="literal">98</ap>
        <ap name="Depth" type="literal">2</ap>
        <ap name="Data" type="literal">FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFAFFFFFFFF7FBFFFFFFFFBFFBFFEFFFFFFFFFFFFFFFFFFFFFBFFFFFFFFFFF7FBFFFEFFFFFFBFFFFFFFFFFFFFFFFFFFFFEFFFFFFFFFFF7FEFFFEFFFFFFFFFAFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFFFFFBFEFFFFFFFFFBFFFBFFFFFFFFFFFFFFFFFFEFFFBEAFFAAFAAFFFFBFFFFFFFFFBFFFFFFFFFFFFFFFFFFFFFFEEFFFFFFFFEBFFFFEFBFFFFFFFFFBFFFFFFFFFFFFFFFFFFEFFFEBFFFFDFFFFBEFFFFFFFFFEFBFFFFFFFFFFFFFFFFFFFFFFFFFFBFFFFFEBFBFFBEFFFFFFFFFFBFFFFFFFFFFFFFFFFFFBFFEFFFDFFEFFFFEBBFFFDFFFFFFDFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFEEFFFEFBFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFBFFFFFFFEFFFFFABFEFBFFFFFFFEFFFFFFFFFFFFFF9FAAAAAAFEFFFFFFFEFFFFFFBFFFFFFEBFAAAAFFFFFFFFFFFFFEFFFFFFFFFFFFEFBFFFFFFFFFFFFFFFFFFFFBFFFFFFFFFFEFFFFFFFFFEFFFFFFEFBAFAAFFFFBFEFFFFFBFFFFFFFFFFFFFFEFFFFFFBFFFFFEFEFBFFFFFEFFFFFEFFFFFFBFFFFFFFFFFAFAAAAAAAAFEFFAAFDAAFEFFAABD6AFFABAAAAFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF9FAAFFAAFEABDAA7AAFFBFAAAAFAFFBFAAAA6AFFFFFFFFFFFFFEFBFBBFBFFFEEFFFBFFFFFFFFFFFFFEFFFFFBFFFFFFFFFFFFFFBFFFFBFFFFFEBFFEFFFFFFFFFEFBFFFFBFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFFFEFBFFFFFFFFBFFFFFFFFFFBFFFEEFFFFFDFBFFFFFEFFBFFFFEFFFEBFA9AAFFFFFFFFFFFFFBEFFEFFEEEFFBFFBFFFFFFBEFFFEFFFFFFFFFFFFFFFFFFFFFFEEAFF9FFFBEFFFEFBFF7FFFFEFFFFBFFFFFFFFFFFFFFFFFEFFFFFFFFFEFFEFBFFFFFFFFFFEFBFFFFFFEFFFFFFFFFFFFFFFBFFFFFFBFEFBFFBFFFFFFFFFFFEEFFFEFFFFFFFFFFFFFFFBFFFFFFFFFFFFFBEFFFEFFFFFFFBFFDFFFFBFFFFFFFFFFFFFFFFFFEFFFFFFFEFFBEFFFFFFFEFFFFFFBFFFFFFFFFFFFFFFFFFFFBFFAFFBFFFAAFFFBFFEBFFFABAAAFFFFFFFFFFFFFFFFFFEFFFBBFFEFF7FFFFBFFFBFFBFFFFFFFFBFFFFFFFFFFFFFFFFFFEFFFFFFBEFFFFFFFBFFFBFFFFFFFFFFFBFFFFFFFFFFFFFFBFFFFEFFFBFEFFBFFFFFBF7FFFFFFFFFEFFFFFFFFFFFFFFFFFFBEFEFBFEFEFFFF7EFFFF7AFFFFFFFAFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0F</ap>
        <rd field="ResultData">image</rd>
      </Properties>
    </node>
    <node type="Variable" id="632520303397145548" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632520303397145549" name="image" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Image" initWith="" refType="reference">image</Properties>
    </node>
    <node type="Variable" id="632520303397145552" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
