<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632149445069218961" level="1" text="GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632147089529844003" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632147089529844002" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/SendResponseTest</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Success" id="632224881663907618" vid="632147089529844022">
        <Properties type="Metreos.Types.String" initWith="S_Success">S_Success</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" varsy="798" startnode="632147089529844004" treenode="632149445069218961" appnode="632147089529844003" handlerfor="632147089529844002">
    <node type="Start" id="632147089529844004" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="266">
      <linkto id="632147089529844010" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Variable" id="632147089529844006" name="header1" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="816">
      <Properties type="Metreos.Types.String" initWith="blah" refType="reference">header1</Properties>
    </node>
    <node type="Variable" id="632147089529844007" name="header2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="99.93229" y="816">
      <Properties type="Metreos.Types.String" initWith="blahism" refType="reference">header2</Properties>
    </node>
    <node type="Variable" id="632147089529844008" name="header3" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="182.864578" y="816">
      <Properties type="Metreos.Types.String" initWith="AcceptMuchoCookies" refType="reference">header3</Properties>
    </node>
    <node type="Action" id="632147089529844010" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="142" y="265">
      <linkto id="632147089529844011" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632147089529844017" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="literal">This is the precise body of a http request sent by the client.</ap>
        <ap name="Value2" type="variable">body</ap>
      </Properties>
    </node>
    <node type="Action" id="632147089529844011" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="243" y="264">
      <linkto id="632147089529844012" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632147089529844018" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">header1</ap>
        <ap name="Value2" type="literal">yes?</ap>
      </Properties>
    </node>
    <node type="Action" id="632147089529844012" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="351" y="265">
      <linkto id="632147089529844013" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632147089529844019" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">header2</ap>
        <ap name="Value2" type="literal">no!!</ap>
      </Properties>
    </node>
    <node type="Action" id="632147089529844013" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="460" y="264">
      <linkto id="632147089529844015" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632147089529844020" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">header3</ap>
        <ap name="Value2" type="literal">CookiesYum</ap>
      </Properties>
    </node>
    <node type="Variable" id="632147089529844014" name="body" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="265.796875" y="816">
      <Properties type="Metreos.Types.String" initWith="body" refType="reference">body</Properties>
    </node>
    <node type="Action" id="632147089529844015" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="556" y="264">
      <linkto id="632147089529844016" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="success" type="literal">true</ap>
        <ap name="signalName" type="variable">S_Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632147089529844016" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="640" y="265">
      <linkto id="632224881663907637" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="serverHeader1" type="literal">serverValue1</ap>
        <ap name="serverHeader2" type="literal">serverValue2</ap>
        <ap name="serverHeader3" type="literal">serverValue3</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="literal">This is a precise body of a http request returned by the server.</ap>
      </Properties>
    </node>
    <node type="Action" id="632147089529844017" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="141" y="418">
      <linkto id="632224881663907637" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="success" type="literal">false</ap>
        <ap name="reason" type="csharp">"The expected value for the body did not equal the actual body.   The expected body was: " + "This is the precise body of a http request sent by the client." + " and the actual response was: " + body.ToString()</ap>
        <ap name="signalName" type="variable">S_Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632147089529844018" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="240" y="415">
      <linkto id="632224881663907637" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="success" type="literal">false</ap>
        <ap name="reason" type="csharp">" Could not match the first header.  The expected value was: yes?.  The actual value was: " + header1.ToString()</ap>
        <ap name="signalName" type="variable">S_Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632147089529844019" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="350" y="415">
      <linkto id="632224881663907637" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="success" type="literal">false</ap>
        <ap name="reason" type="csharp">" Could not match the first header.  The expected value was: no!!.  The actual value was: " + header2.ToString()</ap>
        <ap name="signalName" type="variable">S_Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632147089529844020" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="460" y="411">
      <linkto id="632224881663907637" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="success" type="literal">false</ap>
        <ap name="reason" type="csharp">" Could not match the first header.  The expected value was: CookiesYum.  The actual value was: " + header3.ToString()</ap>
        <ap name="signalName" type="variable">S_Success</ap>
      </Properties>
    </node>
    <node type="Variable" id="632147089529844024" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="328.709625" y="816">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Action" id="632224881663907637" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="341" y="683">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
