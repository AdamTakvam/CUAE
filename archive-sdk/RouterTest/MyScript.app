<Application name="MyScript" trigger="Metreos.Providers.Http.GotRequest" version="0.7" single="false">
  <!--//// app tree ////-->
  <global name="MyScript">
    <outline>
      <treenode type="evh" id="632337930720075162" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632337930720075159" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632337930720075158" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="632337930720075172" level="2" text="MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632337930720075169" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632337930720075168" path="Metreos.CallControl.MakeCall_Complete" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632337930720075177" level="2" text="MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632337930720075174" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632337930720075173" path="Metreos.CallControl.MakeCall_Failed" />
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632337930720075182" level="2" text="Hangup: OnHangup">
        <node type="function" name="OnHangup" id="632337930720075179" path="Metreos.StockTools" />
        <node type="event" name="Hangup" id="632337930720075178" path="Metreos.CallControl.Hangup" />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632337930720075161" treenode="632337930720075162" appnode="632337930720075159" handlerfor="632337930720075158">
    <node type="Start" id="632337930720075161" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="96" y="194">
      <linkto id="632337930720075163" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632337930720075163" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="209.999985" y="173.166687">
      <linkto id="632337930720075183" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">Hello.</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <ap name="remoteHost" type="variable">remotehost</ap>
      </Properties>
    </node>
    <node type="Action" id="632337930720075183" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="259.1667" y="228.3333" mx="325" my="244">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632337930720075172" />
        <item text="OnMakeCall_Failed" treenode="632337930720075177" />
        <item text="OnHangup" treenode="632337930720075182" />
      </items>
      <linkto id="632337930720075184" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632337930720075189" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <Properties final="false" type="provider">
        <ap name="to" type="literal">1006@192.168.1.250</ap>
        <ap name="connectionId" type="literal">1</ap>
        <ap name="mediaIP" type="literal">192.168.1.107</ap>
        <ap name="mediaPort" type="literal">5554</ap>
        <ap name="from" type="literal">blah</ap>
        <ap name="userData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632337930720075184" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="489.999969" y="188.333328">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632337930720075189" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="482.499969" y="320">
      <linkto id="632337930720075184" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="literal">MakeCall failed</ap>
        <ap name="LogLevel" type="literal">Error</ap>
      </Properties>
    </node>
    <node type="Variable" id="632337930720075167" name="remotehost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remotehost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632337930720075171" treenode="632337930720075172" appnode="632337930720075169" handlerfor="632337930720075168">
    <node type="Start" id="632337930720075171" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="140" y="207">
      <linkto id="632337930720075185" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632337930720075185" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="274" y="206">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Error" type="literal">MakeCall Complete</log>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632337930720075176" treenode="632337930720075177" appnode="632337930720075174" handlerfor="632337930720075173">
    <node type="Start" id="632337930720075176" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="152" y="200">
      <linkto id="632337930720075188" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632337930720075188" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="284" y="199">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup" startnode="632337930720075181" treenode="632337930720075182" appnode="632337930720075179" handlerfor="632337930720075178">
    <node type="Start" id="632337930720075181" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="138" y="145">
      <linkto id="632337930720075187" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632337930720075187" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="222" y="142">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Error" type="literal">Hangup</log>
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>