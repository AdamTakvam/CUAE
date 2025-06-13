<Application name="script2" trigger="Metreos.Providers.SccpProxy.RegisterTokenReq" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script2">
    <outline>
      <treenode type="evh" id="632454557648845162" level="1" text="Metreos.Providers.SccpProxy.RegisterTokenReq (trigger): OnRegisterTokenReq">
        <node type="function" name="OnRegisterTokenReq" id="632454557648845159" path="Metreos.StockTools" />
        <node type="event" name="RegisterTokenReq" id="632454557648845158" path="Metreos.Providers.SccpProxy.RegisterTokenReq" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632454557648845861" level="2" text="Metreos.Providers.SccpProxy.SessionFailure: OnSessionFailure">
        <node type="function" name="OnSessionFailure" id="632454557648845858" path="Metreos.StockTools" />
        <node type="event" name="SessionFailure" id="632454557648845857" path="Metreos.Providers.SccpProxy.SessionFailure" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632454610983822263" level="2" text="Metreos.Providers.SccpProxy.RegisterTokenAck: OnRegisterTokenAck">
        <node type="function" name="OnRegisterTokenAck" id="632454610983822260" path="Metreos.StockTools" />
        <node type="event" name="RegisterTokenAck" id="632454610983822259" path="Metreos.Providers.SccpProxy.RegisterTokenAck" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632454610983822268" level="2" text="Metreos.Providers.SccpProxy.RegisterTokenReject: OnRegisterTokenReject">
        <node type="function" name="OnRegisterTokenReject" id="632454610983822265" path="Metreos.StockTools" />
        <node type="event" name="RegisterTokenReject" id="632454610983822264" path="Metreos.Providers.SccpProxy.RegisterTokenReject" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_sid" id="632489933881824737" vid="632454557648845278">
        <Properties type="String" defaultInitWith="string.Empty">g_sid</Properties>
      </treenode>
      <treenode text="g_ccmIP" id="632489933881824752" vid="632489933881824751">
        <Properties type="String" initWith="CCM_IP">g_ccmIP</Properties>
      </treenode>
      <treenode text="g_ccmPort" id="632489933881824754" vid="632489933881824753">
        <Properties type="Int" initWith="CCM_Port">g_ccmPort</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnRegisterTokenReq" startnode="632454557648845161" treenode="632454557648845162" appnode="632454557648845159" handlerfor="632454557648845158">
    <node type="Start" id="632454557648845161" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632454557648845624" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632454557648845624" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="122" y="92">
      <linkto id="632454610983822258" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">l_sid</ap>
        <rd field="ResultData">g_sid</rd>
      </Properties>
    </node>
    <node type="Action" id="632454610983822258" name="RegisterTokenReq" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="255" y="153">
      <linkto id="632454610983822273" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Sid" type="variable">g_sid</ap>
        <ap name="ToIp" type="variable">g_ccmIP</ap>
        <ap name="ToPort" type="variable">g_ccmPort</ap>
      </Properties>
    </node>
    <node type="Action" id="632454610983822273" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="359" y="109">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632454557648845508" name="l_sid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Sid" refType="reference" name="Metreos.Providers.SccpProxy.RegisterTokenReq">l_sid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionFailure" startnode="632454557648845860" treenode="632454557648845861" appnode="632454557648845858" handlerfor="632454557648845857">
    <node type="Start" id="632454557648845860" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632454557648845983" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632454557648845983" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="317" y="81">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRegisterTokenAck" startnode="632454610983822262" treenode="632454610983822263" appnode="632454610983822260" handlerfor="632454610983822259">
    <node type="Start" id="632454610983822262" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632454610983822269" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632454610983822269" name="RegisterTokenAck" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="169" y="122">
      <linkto id="632454610983822272" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Sid" type="variable">g_sid</ap>
      </Properties>
    </node>
    <node type="Action" id="632454610983822272" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="347" y="96">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRegisterTokenReject" activetab="true" startnode="632454610983822267" treenode="632454610983822268" appnode="632454610983822265" handlerfor="632454610983822264">
    <node type="Start" id="632454610983822267" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632454610983822270" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632454610983822270" name="RegisterTokenReject" class="MaxActionNode" group="" path="Metreos.Providers.SccpProxy" x="199" y="89">
      <linkto id="632454610983822271" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Sid" type="variable">g_sid</ap>
      </Properties>
    </node>
    <node type="Action" id="632454610983822271" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="355" y="73">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>