<Application name="SampleImage" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="SampleImage">
    <outline>
      <treenode type="evh" id="632520413815259603" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632520413815259600" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632520413815259599" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/SampleImage</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632520413815259602" treenode="632520413815259603" appnode="632520413815259600" handlerfor="632520413815259599">
    <node type="Start" id="632520413815259602" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="346">
      <linkto id="632520413815259605" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632520413815259605" name="CreateImageBuilder" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="221" y="348">
      <linkto id="632520413815259606" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Width" type="literal">298</ap>
        <ap name="Height" type="literal">168</ap>
        <ap name="BackgroundColor" type="csharp">Color.Black</ap>
        <rd field="ImageBuilder">builder</rd>
      </Properties>
    </node>
    <node type="Action" id="632520413815259606" name="AddTextRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="351" y="347">
      <linkto id="632527174907346151" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Text" type="literal">.............. _@@@__</ap>
        <ap name="Font" type="literal">Courier New</ap>
        <ap name="FontSize" type="literal">16</ap>
        <ap name="Top" type="literal">20</ap>
        <ap name="Left" type="literal">0</ap>
        <ap name="Color" type="csharp">Color.FromArgb(0xfc, 0xff, 0x00)</ap>
        <rd field="Image">builder</rd>
      </Properties>
    </node>
    <node type="Action" id="632520413815259608" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="589" y="352">
      <linkto id="632520413815259609" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">System.Text.Encoding.Default.GetString(builder.ImageData)</ap>
        <ap name="Content-Type" type="literal">image/png</ap>
      </Properties>
    </node>
    <node type="Action" id="632520413815259609" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="725.884033" y="350">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632527174907346151" name="AddTextRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="216" y="426">
      <linkto id="632527174907346153" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Text" type="literal">......_____//____?__\________</ap>
        <ap name="Font" type="literal">Courier New</ap>
        <ap name="FontSize" type="literal">16</ap>
        <ap name="Top" type="literal">39</ap>
        <ap name="Left" type="literal">0</ap>
        <ap name="Color" type="csharp">Color.FromArgb(0xff, 0x00, 0xfc)</ap>
        <rd field="Image">builder</rd>
      </Properties>
    </node>
    <node type="Action" id="632527174907346153" name="AddTextRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="359" y="450">
      <linkto id="632527174907346155" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Text" type="literal">- ---o--------CARE-POLICE----@)</ap>
        <ap name="Font" type="literal">Courier New</ap>
        <ap name="FontSize" type="literal">16</ap>
        <ap name="Top" type="literal">58</ap>
        <ap name="Left" type="literal">0</ap>
        <ap name="Color" type="csharp">Color.FromArgb(0x00, 0xff, 0x18)</ap>
        <rd field="Image">builder</rd>
      </Properties>
    </node>
    <node type="Action" id="632527174907346155" name="AddTextRegion" class="MaxActionNode" group="" path="Metreos.Native.ImageBuilder" x="498" y="452">
      <linkto id="632520413815259608" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Text" type="literal">-----` --()======+====()--x</ap>
        <ap name="Font" type="literal">Courier New</ap>
        <ap name="FontSize" type="literal">16</ap>
        <ap name="Top" type="literal">77</ap>
        <ap name="Left" type="literal">0</ap>
        <ap name="Color" type="csharp">Color.FromArgb(0x00, 0xff, 0xEA)</ap>
        <rd field="Image">builder</rd>
      </Properties>
    </node>
    <node type="Variable" id="632520413815259604" name="builder" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Imaging.ImageBuilder" refType="reference">builder</Properties>
    </node>
    <node type="Variable" id="632520413815259607" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
