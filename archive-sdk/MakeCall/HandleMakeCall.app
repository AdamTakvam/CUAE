<Application name="HandleMakeCall" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="HandleMakeCall">
    <outline>
      <treenode type="evh" id="632471146299099787" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632471146299099784" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632471146299099783" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/MakeCall</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632471146299099798" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632471146299099795" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632471146299099794" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632489885866777387" actid="632471146299099809" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632471146299099803" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632471146299099800" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632471146299099799" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632489885866777388" actid="632471146299099809" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632471146299099808" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632471146299099805" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632471146299099804" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632489885866777389" actid="632471146299099809" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632471146299099820" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632471146299099817" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632471146299099816" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632489885866777395" actid="632471146299099826" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632471146299099825" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632471146299099822" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632471146299099821" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632489885866777396" actid="632471146299099826" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_CallId" id="632489885866777381" vid="632471146299099788">
        <Properties type="String">g_CallId</Properties>
      </treenode>
      <treenode text="g_ConnId" id="632489885866777383" vid="632471146299099834">
        <Properties type="String">g_ConnId</Properties>
      </treenode>
      <treenode text="g_DN" id="632489885866777413" vid="632489885866777412">
        <Properties type="String" initWith="DN">g_DN</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632471146299099786" treenode="632471146299099787" appnode="632471146299099784" handlerfor="632471146299099783">
    <node type="Start" id="632471146299099786" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="137">
      <linkto id="632471146299099793" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632471146299099793" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="154" y="137">
      <linkto id="632471146299099809" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
      </Properties>
    </node>
    <node type="Action" id="632471146299099809" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="241" y="121" mx="307" my="137">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632471146299099798" />
        <item text="OnMakeCall_Failed" treenode="632471146299099803" />
        <item text="OnRemoteHangup" treenode="632471146299099808" />
      </items>
      <linkto id="632471146299099814" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632471146299099813" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider">
        <ap name="To" type="variable">g_DN</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_CallId</rd>
        <log condition="failure" on="true" level="Error" type="literal">MakeCall Failed</log>
      </Properties>
    </node>
    <node type="Action" id="632471146299099813" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="482" y="135">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632471146299099814" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="305" y="297">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632471146299099792" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632471146299099797" treenode="632471146299099798" appnode="632471146299099795" handlerfor="632471146299099794">
    <node type="Start" id="632471146299099797" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="146">
      <linkto id="632471146299099836" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632471146299099826" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="181" y="130" mx="234" my="146">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632471146299099820" />
        <item text="OnPlay_Failed" treenode="632471146299099825" />
      </items>
      <linkto id="632471146299099830" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632471146299099829" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Prompt1" type="literal">welcome.wav</ap>
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="UserData" type="literal">none</ap>
        <log condition="failure" on="true" level="Error" type="literal">Play Failed</log>
      </Properties>
    </node>
    <node type="Action" id="632471146299099829" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="231" y="290">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632471146299099830" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="380" y="146">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632471146299099836" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="117" y="146">
      <linkto id="632471146299099826" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">connId</ap>
        <rd field="ResultData">g_ConnId</rd>
      </Properties>
    </node>
    <node type="Variable" id="632471146299099815" name="connId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632471146299099802" treenode="632471146299099803" appnode="632471146299099800" handlerfor="632471146299099799">
    <node type="Start" id="632471146299099802" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632471146299099831" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632471146299099831" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="271" y="74">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632471146299099807" treenode="632471146299099808" appnode="632471146299099805" handlerfor="632471146299099804">
    <node type="Start" id="632471146299099807" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="121">
      <linkto id="632471146299099833" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632471146299099832" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="318" y="121">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632471146299099833" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="188" y="121">
      <linkto id="632471146299099832" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">g_ConnId</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632471146299099819" treenode="632471146299099820" appnode="632471146299099817" handlerfor="632471146299099816">
    <node type="Start" id="632471146299099819" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="120">
      <linkto id="632471146299099837" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632471146299099837" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="143" y="119">
      <linkto id="632471146299099838" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_CallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632471146299099838" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="265" y="118">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632471146299099824" treenode="632471146299099825" appnode="632471146299099822" handlerfor="632471146299099821">
    <node type="Start" id="632471146299099824" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="141">
      <linkto id="632471146299099840" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632471146299099839" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="285" y="141">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632471146299099840" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="144" y="141">
      <linkto id="632471146299099839" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_CallId</ap>
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>