<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632520303397145547" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632520303397145544" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632520303397145543" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/CreateGraphicMenu</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632520303397145546" treenode="632520303397145547" appnode="632520303397145544" handlerfor="632520303397145543">
    <node type="Start" id="632520303397145546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="395">
      <linkto id="632520303397145591" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632520303397145551" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="521" y="396">
      <linkto id="632520303397145571" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Text</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/CreateText"</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632520303397145553" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="863" y="389">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632520303397145554" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="767" y="391">
      <linkto id="632520303397145553" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">graphicMenu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632520303397145571" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="640" y="398">
      <linkto id="632520303397145554" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632520303397145591" name="CreateGraphicMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="191" y="393">
      <linkto id="632520303397145592" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">IVT Graphic Menu</ap>
        <ap name="Prompt" type="literal">Options</ap>
        <ap name="LocationX" type="literal">-1</ap>
        <ap name="LocationY" type="literal">-1</ap>
        <ap name="Height" type="literal">65</ap>
        <ap name="Width" type="literal">133</ap>
        <ap name="Depth" type="literal">2</ap>
        <ap name="Data" type="literal">00F0030000000000000000000000000000000000000000000000000040BF00000000D00F0000000000000000000000000000000000000000000000000000FE00000000E03F0000000000A5AA010000000000000000000000D06F0500000000FC03000000D03F00000090FEFFFFFFAF01000000000000000000F4FFFF1F000000E02F00000040BF000000E0FFFFFFFFFFBF0100000000E4FF56E5FFFFFFFFFFAB0140FF07000000FD030000C0FFFFAFFAFFFF7F000000D0FFFFFFFFFFBFFEFFFFFF0F00D07F000000F03F000000BE16000040FAFFAFAAAAFFFFFFFFFFFF1B00E0FFFF3F0000FD0B000040FF010000000000000000F4FFFFFFFFFF6FF9FFBF00000000A96A0000D03F000000F807000000000000000000FDFFFFFF7F01000000000000000000000040FF000000F41F00000000000000000040AAAAAA01000000000000000000000000E4FF020000F41F0000000000000000000000000000000000000000000000000000F4FF010000F02F0000000000000000000000000000000000000000000000000000F4FF010000E01F0000000000000000000000000000000000000000000000000000F06F000000D02F0000000000000000000000000000000000000000000000000000C07F000000C07F000000003800000000FFFFFF00000000000F000000000000000000FF1F0000007F00000000F800000000FCFFFF03000000003C000000000000000000D0FF070000FC00000000FF0300000000C0030000000000F000000000000000000000FEFF0100F01F000000FC0F00000000000F90BF002EE0F2FF03000000000000000080FF7F0080FF020000003C00000000003CE0FF0FF0C1C3FF0F000000000000000000D0FF0200F80F000000F00000000000F0D007B800CF033C0000000000000000000000FD0B00C03F000000C00300000000C0C303C003B407F00000000000000000000000FD1F0080BF000000000F00000000000FFFFF0FC00BC00300000000000000000000F81B0000FF000000003C00000000003CFCFF3F003E000F00000000000000000000E0FF0200FD01000000F00000000000F0F0000000ED023C00000000000000000000E0FF0F00F803000000C003C0030000C0431F40032C0FF001000000000000000000E0FF1F00E00F000000FCFF030F0000000FF8FF0F3CF480FF030000000000000000D0FF060040FF010000F0FF0F3C0000003C40FE0BB8800BF80F000000000000000040FFFF0100F40B000000000000000000000000000000000000000000000000000000FDFF0F00C01F00000000000000000000000000000000000000000000000000000000FD3F00803F00000000000000000000000000000000000000000000000000000000F0FF0000BF0000000000000000000000000000000000000000000000000000000080FF6F00FC0100000000000000000000000000000000000000000000000000000000FDFFBFF00B0000000000000000000000000000000000000000000000000000000040FEFF837F000000000000000000000000000000000000000000000000000000000080FF0FFC0300000000000000000000000000000000000000000000000000000000000039E00F00000000000000000000000000000000000000000000000000000000000000802F0000000000000000000000000000000000000000000000000000000000000000FE000000F91B00000000FCBF01F000000000000000C00300000000000000000000F40B0000FCFF01000000F0FF2FC003000000000000000F00000000000000000000407F000070800F000000C003F40200000000000000003C0000000000000000000000FD020000003C000000000F001F3CF0F443FE02D06FFCFFD07F004F3F1FD0030000FC07000000F0000000003C00B4F0C0FF8FFF3FE0FFF3FFE3FF0BFCFFB480070000F807000000D001000000F000C0C3037F401FE0D2070D0FD0077DF007C0030F0000F007000000D002000000C003000F0F3C000F00CF07003CC007D0C303001E2D0000D00F000000D002000000000F003C3CF000FCFF3F0F00F0000F000F0F00B478000040BF000000D002000000003C00B4F0C003F0FFFF3C00C0033C003C3C00C0F3000000FC030000E00200000000F000F0C1030FC00300F001000FF001F4F00000EE020000F80B0000E00100F00000C003F4020F3C007D008D1F347C401FF4C10300F0030000F40F0000C0FFFFC0030000FFFF023CF000E0FF3FFCFFE0FFF8FF020F00C00B0000F01F000000FFFF030F0000FCBF01F0C00300F92F80BF01FE43FF013C00001D0000C01F0000000000000000000000000000000000000000000000000000000038000000BF00000000000000000000000000000000000000000000000000000000B0000000F41F000000000000000000000000000000000000000000000000000000F000000080FF000000000000000000000000000000000000000000000000000000D003000040FF03000000000000000000000000000000000000000000000000000000000000C0FF0B00000000000000000000000000000000000000000000000000000000000050FF1B000000800B0000000040FEFFFFFFFFFF5F55550500000000000000000000E0FFFF6F000000FF02000000E4FFFFFFFFFFFFFFFFFFFFFFFFFF6B010000000000F4FFFFFF0B0000F81F000040FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF1B0080AFEAFF1FA4FFBF000080FFABEAFFFFFF565555555555FFFFFFFFFFFFFFFFFF00D0FFFFFF1F0040FE1F0000F4FFFFFFFF1F00000000000000000000545595FEFF0380FFFFFF1F000040FF010000FEFFFFFF0B0000000000000000000000000000400640FFAAAA06000000F81F000090AA6A01000000000000000000000000000000000000FE01000000000080FF000000000000000000000000000000000000000000000000F403000000000000F8030000000000000000000000000000000000000000000000E00F00000000</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632520303397145592" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="329" y="394">
      <linkto id="632520334149048292" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Nothing</ap>
        <ap name="URL" type="csharp">host + "/CreateText"</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632520334149048292" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="422" y="395">
      <linkto id="632520303397145551" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Nothing2</ap>
        <ap name="URL" type="csharp">host + "/CreateDirectory"</ap>
        <rd field="ResultData">graphicMenu</rd>
      </Properties>
    </node>
    <node type="Variable" id="632520303397145548" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632520303397145549" name="graphicMenu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.GraphicMenu" initWith="" refType="reference">graphicMenu</Properties>
    </node>
    <node type="Variable" id="632520303397145552" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
