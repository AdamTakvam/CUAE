<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632520303397145547" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632520303397145544" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632520303397145543" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/CreateStatus</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632520303397145546" treenode="632520303397145547" appnode="632520303397145544" handlerfor="632520303397145543">
    <node type="Start" id="632520303397145546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="45" y="387">
      <linkto id="632527174907346894" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632520303397145553" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="548" y="381">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632520303397145554" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="423" y="382">
      <linkto id="632520303397145553" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">status.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632527174907346894" name="CreateStatus" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="181" y="385">
      <linkto id="632520303397145554" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Text" type="literal">IVT Status</ap>
        <ap name="Timer" type="literal">10</ap>
        <ap name="LocationX" type="literal">-1</ap>
        <ap name="LocationY" type="literal">-1</ap>
        <ap name="Height" type="literal">21</ap>
        <ap name="Width" type="literal">106</ap>
        <ap name="Depth" type="literal">2</ap>
        <ap name="Data" type="literal">0000400500000000BD0100000000000000000000000000000000000040AA16000000D01B00000000000054000000000000000000000000F9FF06000000FE010000000000801B0000000000000000000000E0FFBF010000D01F000000000000F9060000000000000000000040FFFA6F000000FD010000000000807F0000000000000000000000F45BF90B0000D01F000000000000F40B00000000000000000000807F40BE010004FD02000000000040BE0100000000000050050000F80790060090EA6F000000000000E01B000000006900A9BE000540BF00000040FEFF6F40A91A005400FE01000000D01BE4FF1BB000E41B000000E0FFFF47F9FF06906AE56F55550000FE81FFBF001F00FE02000000E9FF7FF9FF7F05FEFFFFFFFF1B00E01FF8BF06F006D06F01000000E5BFE5FFAABA95FFFFFFFFFF0240BE41FF6F007F00F8AFAA050000FD46FE56E57FA5FFFFFFFF1B00F40BE4FF1BF01B40FEFFFF0100D02FF41B40FE1B54E56F556A00906F00A4FF0BFE0690FFFF6F0000FD42BF41F9FF0100FD02E40600FD0200E4FF91BF01A4FFFF0600D01FE4AFFAFF6F00902F00BE01E51F0054FA2FF86F55EAFF2F0000F901FEFFFFFF1B00F906E06FE9BF01A5FEFF42FEAFFEFFBF0100401A90FFFFAAFF02806F00FDFFFF06D0FFFF2F90FEFFFFFF0600001000A4BE16942F00F80780FFFF1B00FDFFAF0190FFFFAA0500000000005405005401407F00A0FF1A0050AA56010090AA15000000000000000000000000E40600A415000000000000</ap>
        <rd field="ResultData">status</rd>
      </Properties>
    </node>
    <node type="Variable" id="632520303397145548" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632520303397145549" name="status" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Status" initWith="" refType="reference">status</Properties>
    </node>
    <node type="Variable" id="632520303397145552" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
