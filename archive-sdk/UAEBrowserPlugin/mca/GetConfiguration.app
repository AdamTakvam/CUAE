<Application name="GetConfiguration" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="GetConfiguration">
    <outline>
      <treenode type="evh" id="632923994523549140" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632923994523549137" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632923994523549136" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/CiscoDirectoryPlugin/GetConfiguration</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callbackSupported" id="632923994523549163" vid="632923994523549162">
        <Properties type="String" initWith="CallbackSupported">g_callbackSupported</Properties>
      </treenode>
      <treenode text="g_phoneControlSupported" id="632923994523549165" vid="632923994523549164">
        <Properties type="String" initWith="PhoneControlSupported">g_phoneControlSupported</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632923994523549139" treenode="632923994523549140" appnode="632923994523549137" handlerfor="632923994523549136">
    <node type="Start" id="632923994523549139" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="53" y="445">
      <linkto id="632923994523549167" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632923994523549167" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="208" y="445">
      <linkto id="632923994523549168" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">g_phoneControlSupported + ":" + g_callbackSupported</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632923994523549168" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="380" y="446">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632923994523549166" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>